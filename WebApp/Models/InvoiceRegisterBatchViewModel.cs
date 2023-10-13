using BusinessObject;
using BusinessObject.InvoiceRegisters;
using DataAccess.EntityFramework;
using Services;
using Services.InvoiceRegisters;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class InvoiceRegisterBatchViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Batch No")]
        public int? BatchNo { get; set; }
        
        [Display(Name = "Batch Date")]
        public DateTime BatchDate { get; set; }

        [Required, Display(Name = "Batch Date")]
        public string BatchDateStr { get; set; }

        public int Status { get; set; }

        public int ModuleId { get; set; }

        public int TotalInvoice { get; set; }

        public InvoiceRegisterBatchViewModel() { Set(); }

        public InvoiceRegisterBatchViewModel(InvoiceRegisterBatchBo invoiceRegisterBatchBo)
        {
            Set(invoiceRegisterBatchBo);
        }

        public void Set(InvoiceRegisterBatchBo invoiceRegisterBatchBo = null)
        {
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.InvoiceRegister.ToString());
            ModuleId = moduleBo.Id;

            if (invoiceRegisterBatchBo != null)
            {
                Id = invoiceRegisterBatchBo.Id;
                BatchNo = invoiceRegisterBatchBo.BatchNo;
                BatchDate = invoiceRegisterBatchBo.BatchDate;
                BatchDateStr = invoiceRegisterBatchBo.BatchDate.ToString(Util.GetDateFormat());
                Status = invoiceRegisterBatchBo.Status;
                TotalInvoice = invoiceRegisterBatchBo.TotalInvoice;
            }
            else
            {
                Status = InvoiceRegisterBatchBo.StatusPending;
                BatchDateStr = DateTime.Now.ToString(Util.GetDateFormat());
            }
        }

        public InvoiceRegisterBatchBo FormBo(int createdById, int updatedById)
        {
            return new InvoiceRegisterBatchBo
            {
                Id = Id,
                BatchDate = DateTime.Parse(BatchDateStr),
                Status = Status,
                TotalInvoice = TotalInvoice,

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public void ProcessSoaDetails(FormCollection form, int authUserId)
        {
            int maxIndex = int.Parse(form.Get("soaDataDetailsMaxIndex"));
            int index = 0;

            if (maxIndex != index)
            {
                // Delete all
                InvoiceRegisterBatchSoaDataService.DeleteAllByInvoiceRegisterBatchId(Id);

                while (index <= maxIndex)
                {
                    // Create
                    string soaIdStr = form.Get(string.Format("soaDataDetailsId[{0}]", index));
                    if (!string.IsNullOrEmpty(soaIdStr))
                    {
                        int soaId = int.Parse(soaIdStr);
                        InvoiceRegisterBatchSoaDataBo bo = new InvoiceRegisterBatchSoaDataBo
                        {
                            InvoiceRegisterBatchId = Id,
                            SoaDataBatchId = soaId,
                        };
                        InvoiceRegisterBatchSoaDataService.Create(ref bo);

                        List<int> allowStatus = new List<int> { RetroRegisterBatchBo.StatusPending, RetroRegisterBatchBo.StatusSubmitForProcessing };
                        if (allowStatus.Contains(Status))
                        {
                            using (var db = new AppDbContext(false))
                            {
                                // Update status to 'Pending Invoicing' into ClaimRegister by SoaDataBatchId
                                db.Database.ExecuteSqlCommand("UPDATE [ClaimRegister] SET [OffsetStatus] = {0}, [UpdatedById] = {1}, [UpdatedAt] = {2} WHERE [SoaDataBatchId] = {3}",
                                    ClaimRegisterBo.OffsetStatusPendingInvoicing, authUserId, DateTime.Now, soaId);
                                db.SaveChanges();
                            }
                        }
                    }
                    index++;
                }
            }
        }

        public void ProcessStatusHistory(int authUserId, ref TrailObject trail)
        {
            bool isChangeStatus = false;
            StatusHistoryBo latestStatusHistoryBo = StatusHistoryService.FindLatestByModuleIdObjectId(ModuleId, Id);

            if (latestStatusHistoryBo == null || latestStatusHistoryBo.Status != Status)
            {
                isChangeStatus = true;
            }

            if (latestStatusHistoryBo == null && Status == InvoiceRegisterBatchBo.StatusSubmitForProcessing)
            {
                StatusHistoryBo statusHistoryBo = new StatusHistoryBo
                {
                    ModuleId = ModuleId,
                    ObjectId = Id,
                    Status = InvoiceRegisterBatchBo.StatusPending,
                    CreatedById = authUserId,
                    UpdatedById = authUserId,
                };
                StatusHistoryService.Save(ref statusHistoryBo, ref trail);
                statusHistoryBo = new StatusHistoryBo
                {
                    ModuleId = ModuleId,
                    ObjectId = Id,
                    Status = Status,
                    CreatedById = authUserId,
                    UpdatedById = authUserId,
                };
                StatusHistoryService.Save(ref statusHistoryBo, ref trail);
            }
            else if (isChangeStatus)
            {
                StatusHistoryBo statusHistoryBo = new StatusHistoryBo
                {
                    ModuleId = ModuleId,
                    ObjectId = Id,
                    Status = Status,
                    CreatedById = authUserId,
                    UpdatedById = authUserId,
                };
                StatusHistoryService.Save(ref statusHistoryBo, ref trail);
            }
        }

        public void ProcessUploadSunglFile(FormCollection form, int authUserId, ref TrailObject trail)
        {
            int index = 0;
            while (form.AllKeys.Contains(string.Format("uploadFileName[{0}]", index)))
            {
                string filename = form.Get(string.Format("uploadFileName[{0}]", index));
                string hashFilename = form.Get(string.Format("uploadHashFileName[{0}]", index));
                string id = form.Get(string.Format("uploadId[{0}]", index));
                string type = form.Get(string.Format("uploadType[{0}]", index));

                InvoiceRegisterBatchFileBo bo = new InvoiceRegisterBatchFileBo
                {
                    FileName = filename,
                    HashFileName = hashFilename,
                    Status = InvoiceRegisterBatchFileBo.StatusPending,
                    DataUpdate = true,
                    CreatedById = authUserId,
                    UpdatedById = authUserId,
                    InvoiceRegisterBatchId = Id,
                };

                if (!string.IsNullOrEmpty(type)) bo.Type = int.Parse(type);
                if (!string.IsNullOrEmpty(id)) bo.Id = int.Parse(id);

                string path = bo.GetLocalPath();
                string tempPath = bo.GetTempPath("Uploads");

                if (System.IO.File.Exists(tempPath))
                {
                    Util.MakeDir(path);
                    System.IO.File.Move(tempPath, path);
                }

                InvoiceRegisterBatchFileService.Save(ref bo, ref trail);
                index++;
            }
        }
    }
}