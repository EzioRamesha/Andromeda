using BusinessObject;
using Services;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class RetroRegisterBatchViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Batch No")]
        public int? BatchNo { get; set; }

        [Display(Name = "Batch Date")]
        public DateTime BatchDate { get; set; }

        [Required, Display(Name = "Retro Type")]
        public int Type { get; set; }

        [Required, Display(Name = "Batch Date")]
        public string BatchDateStr { get; set; }

        public int Status { get; set; }

        public int ModuleId { get; set; }

        public int TotalInvoice { get; set; }

        public RetroRegisterBatchViewModel() { Set(); }

        public RetroRegisterBatchViewModel(RetroRegisterBatchBo retroRegisterBatchBo)
        {
            Set(retroRegisterBatchBo);
        }

        public void Set(RetroRegisterBatchBo retroRegisterBatchBo = null)
        {
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.RetroRegister.ToString());
            ModuleId = moduleBo.Id;

            if (retroRegisterBatchBo != null)
            {
                Id = retroRegisterBatchBo.Id;
                BatchNo = retroRegisterBatchBo.BatchNo;
                BatchDate = retroRegisterBatchBo.BatchDate;
                BatchDateStr = retroRegisterBatchBo.BatchDate.ToString(Util.GetDateFormat());
                Type = retroRegisterBatchBo.Type;
                Status = retroRegisterBatchBo.Status;
                TotalInvoice = retroRegisterBatchBo.TotalInvoice;
            }
            else
            {
                Status = RetroRegisterBatchBo.StatusPending;
                BatchDateStr = DateTime.Now.ToString(Util.GetDateFormat());
            }
        }

        public RetroRegisterBatchBo FormBo(int createdById, int updatedById)
        {
            return new RetroRegisterBatchBo
            {
                Id = Id,
                BatchDate = DateTime.Parse(BatchDateStr),
                Type = Type,
                Status = Status,
                TotalInvoice = TotalInvoice,

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public void ProcessDirectRetroDetails(FormCollection form, int authUserId)
        {
            int maxIndex = int.Parse(form.Get("retroDataDetailsMaxIndex"));
            int index = 0;

            if (maxIndex != index)
            {
                // Delete all
                var directRetroIds = RetroRegisterBatchDirectRetroService.GetIdsByRetroRegisterBatchId(Id);
                foreach (var directRetroId in directRetroIds)
                {
                    var dr = DirectRetroService.Find(directRetroId);
                    if (dr != null)
                    {
                        var drBo = dr;
                        drBo.RetroStatus = DirectRetroBo.RetroStatusApproved;
                        drBo.UpdatedById = authUserId;
                        DirectRetroService.Save(ref drBo);
                    }
                }
                RetroRegisterBatchDirectRetroService.DeleteAllByRetroRegisterBatchId(Id);

                while (index <= maxIndex)
                {
                    // Create
                    string directRetroIdStr = form.Get(string.Format("directRetroDetailsId[{0}]", index));
                    if (!string.IsNullOrEmpty(directRetroIdStr))
                    {
                        int directRetroId = int.Parse(directRetroIdStr);
                        RetroRegisterBatchDirectRetroBo bo = new RetroRegisterBatchDirectRetroBo
                        {
                            RetroRegisterBatchId = Id,
                            DirectRetroId = directRetroId,
                        };
                        RetroRegisterBatchDirectRetroService.Create(ref bo);

                        List<int> allowStatus = new List<int> { RetroRegisterBatchBo.StatusPending, RetroRegisterBatchBo.StatusSubmitForProcessing };
                        if (allowStatus.Contains(Status))
                        { 
                            var directRetroBo = DirectRetroService.Find(directRetroId);
                            if (directRetroBo != null)
                            {
                                var drBo = directRetroBo;
                                drBo.RetroStatus = DirectRetroBo.RetroStatusStatementIssuing;
                                drBo.UpdatedById = authUserId;
                                DirectRetroService.Save(ref drBo);
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

            if (latestStatusHistoryBo == null && Status == RetroRegisterBatchBo.StatusSubmitForProcessing)
            {
                StatusHistoryBo statusHistoryBo = new StatusHistoryBo
                {
                    ModuleId = ModuleId,
                    ObjectId = Id,
                    Status = RetroRegisterBatchBo.StatusPending,
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
    }
}