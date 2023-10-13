using BusinessObject;
using BusinessObject.InvoiceRegisters;
using DataAccess.Entities.InvoiceRegisters;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.IO;

namespace Services.InvoiceRegisters
{
    public class InvoiceRegisterBatchService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(InvoiceRegisterBatch)),
                Controller = ModuleBo.ModuleController.InvoiceRegisterBatch.ToString()
            };
        }

        public static InvoiceRegisterBatchBo FormBo(InvoiceRegisterBatch entity = null)
        {
            if (entity == null)
                return null;
            return new InvoiceRegisterBatchBo
            {
                Id = entity.Id,
                BatchNo = entity.BatchNo,
                BatchDate = entity.BatchDate,
                Status = entity.Status,
                TotalInvoice = entity.TotalInvoice,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<InvoiceRegisterBatchBo> FormBos(IList<InvoiceRegisterBatch> entities = null)
        {
            if (entities == null)
                return null;
            IList<InvoiceRegisterBatchBo> bos = new List<InvoiceRegisterBatchBo>() { };
            foreach (InvoiceRegisterBatch entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static InvoiceRegisterBatch FormEntity(InvoiceRegisterBatchBo bo = null)
        {
            if (bo == null)
                return null;
            return new InvoiceRegisterBatch
            {
                Id = bo.Id,
                BatchNo = bo.BatchNo,
                BatchDate = bo.BatchDate,
                Status = bo.Status,
                TotalInvoice = bo.TotalInvoice,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsDuplicateCode(InvoiceRegisterBatch InvoiceRegisterBatch)
        {
            return InvoiceRegisterBatch.IsDuplicateCode();
        }

        public static bool IsExists(int id)
        {
            return InvoiceRegisterBatch.IsExists(id);
        }

        public static InvoiceRegisterBatchBo Find(int id)
        {
            return FormBo(InvoiceRegisterBatch.Find(id));
        }

        public static InvoiceRegisterBatchBo FindByBatchNo(int batchNo)
        {
            return FormBo(InvoiceRegisterBatch.FindByBatchNo(batchNo));
        }

        public static InvoiceRegisterBatchBo FindByStatus(int status)
        {
            return FormBo(InvoiceRegisterBatch.FindByStatus(status));
        }

        public static IList<InvoiceRegisterBatchBo> Get()
        {
            return FormBos(InvoiceRegisterBatch.Get());
        }

        public static int CountByStatus(int status)
        {
            return InvoiceRegisterBatch.CountByStatus(status);
        }

        public static int GetMaxId()
        {
            return InvoiceRegisterBatch.GetMaxId();
        }

        public static Result Save(ref InvoiceRegisterBatchBo bo)
        {
            if (!InvoiceRegisterBatch.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref InvoiceRegisterBatchBo bo, ref TrailObject trail)
        {
            if (!InvoiceRegisterBatch.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref InvoiceRegisterBatchBo bo)
        {
            InvoiceRegisterBatch entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref InvoiceRegisterBatchBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref InvoiceRegisterBatchBo bo)
        {
            Result result = Result();

            InvoiceRegisterBatch entity = InvoiceRegisterBatch.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.BatchNo = bo.BatchNo;
                entity.BatchDate = bo.BatchDate;
                entity.Status = bo.Status;
                entity.TotalInvoice = bo.TotalInvoice;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref InvoiceRegisterBatchBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(InvoiceRegisterBatchBo bo)
        {
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.InvoiceRegister.ToString());

            InvoiceRegisterBatchSoaDataService.DeleteAllByInvoiceRegisterBatchId(bo.Id);
            var files = InvoiceRegisterBatchFileService.GetAllByInvoiceRegisterBatchId(bo.Id);
            foreach (InvoiceRegisterBatchFileBo file in files)
            {
                if (file.DataUpdate == true)
                {
                    if (File.Exists(file.GetLocalPath()))
                        File.Delete(file.GetLocalPath());
                }
                else
                {
                    string fileE1 = Path.Combine(Util.GetE1Path(), file.HashFileName);
                    if (File.Exists(fileE1))
                        File.Delete(fileE1);
                }
            }
            InvoiceRegisterBatchFileService.DeleteAllByInvoiceRegisterBatchId(bo.Id);
            InvoiceRegisterService.DeleteAllByInvoiceRegisterBatchId(bo.Id);
            StatusHistoryService.DeleteAllByModuleIdObjectId(moduleBo.Id, bo.Id);
            RemarkService.DeleteAllByModuleIdObjectId(moduleBo.Id, bo.Id);

            InvoiceRegisterBatch.Delete(bo.Id);
        }

        public static Result Delete(InvoiceRegisterBatchBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (
                InvoiceRegisterHistory.CountByInvoiceRegisterBatchId(bo.Id) > 0
            )
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.InvoiceRegister.ToString());

                InvoiceRegisterBatchSoaDataService.DeleteAllByInvoiceRegisterBatchId(bo.Id, ref trail);
                var files = InvoiceRegisterBatchFileService.GetAllByInvoiceRegisterBatchId(bo.Id);
                foreach (InvoiceRegisterBatchFileBo file in files)
                {
                    if (file.DataUpdate == true)
                    {
                        if (File.Exists(file.GetLocalPath()))
                            File.Delete(file.GetLocalPath());
                    }
                    else
                    {
                        string fileE1 = Path.Combine(Util.GetE1Path(), file.HashFileName);
                        if (File.Exists(fileE1))
                            File.Delete(fileE1);
                    }
                }
                InvoiceRegisterBatchFileService.DeleteAllByInvoiceRegisterBatchId(bo.Id, ref trail);
                InvoiceRegisterService.DeleteAllByInvoiceRegisterBatchId(bo.Id, ref trail);
                StatusHistoryService.DeleteAllByModuleIdObjectId(moduleBo.Id, bo.Id, ref trail);
                RemarkService.DeleteAllByModuleIdObjectId(moduleBo.Id, bo.Id, ref trail);

                DataTrail dataTrail = InvoiceRegisterBatch.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
