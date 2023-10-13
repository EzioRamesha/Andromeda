using BusinessObject;
using DataAccess.Entities;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.IO;

namespace Services
{
    public class RetroRegisterBatchService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RetroRegisterBatch)),
                Controller = ModuleBo.ModuleController.RetroRegisterBatch.ToString()
            };
        }

        public static RetroRegisterBatchBo FormBo(RetroRegisterBatch entity = null)
        {
            if (entity == null)
                return null;
            return new RetroRegisterBatchBo
            {
                Id = entity.Id,
                BatchNo = entity.BatchNo,
                BatchDate = entity.BatchDate,
                Type = entity.Type,
                Status = entity.Status,
                TotalInvoice = entity.TotalInvoice,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<RetroRegisterBatchBo> FormBos(IList<RetroRegisterBatch> entities = null)
        {
            if (entities == null)
                return null;
            IList<RetroRegisterBatchBo> bos = new List<RetroRegisterBatchBo>() { };
            foreach (RetroRegisterBatch entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RetroRegisterBatch FormEntity(RetroRegisterBatchBo bo = null)
        {
            if (bo == null)
                return null;
            return new RetroRegisterBatch
            {
                Id = bo.Id,
                BatchNo = bo.BatchNo,
                BatchDate = bo.BatchDate,
                Type = bo.Type,
                Status = bo.Status,
                TotalInvoice = bo.TotalInvoice,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsDuplicateCode(RetroRegisterBatch RetroRegisterBatch)
        {
            return RetroRegisterBatch.IsDuplicateCode();
        }

        public static bool IsExists(int id)
        {
            return RetroRegisterBatch.IsExists(id);
        }

        public static RetroRegisterBatchBo Find(int? id)
        {
            return FormBo(RetroRegisterBatch.Find(id));
        }

        public static RetroRegisterBatchBo FindByBatchNo(int batchNo)
        {
            return FormBo(RetroRegisterBatch.FindByBatchNo(batchNo));
        }

        public static RetroRegisterBatchBo FindByStatus(int status)
        {
            return FormBo(RetroRegisterBatch.FindByStatus(status));
        }

        public static IList<RetroRegisterBatchBo> Get()
        {
            return FormBos(RetroRegisterBatch.Get());
        }

        public static int CountByStatus(int status)
        {
            return RetroRegisterBatch.CountByStatus(status);
        }

        public static int GetMaxId()
        {
            return RetroRegisterBatch.GetMaxId();
        }

        public static Result Save(ref RetroRegisterBatchBo bo)
        {
            if (!RetroRegisterBatch.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref RetroRegisterBatchBo bo, ref TrailObject trail)
        {
            if (!RetroRegisterBatch.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RetroRegisterBatchBo bo)
        {
            RetroRegisterBatch entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RetroRegisterBatchBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RetroRegisterBatchBo bo)
        {
            Result result = Result();

            RetroRegisterBatch entity = RetroRegisterBatch.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.BatchNo = bo.BatchNo;
                entity.BatchDate = bo.BatchDate;
                entity.Type = bo.Type;
                entity.Status = bo.Status;
                entity.TotalInvoice = bo.TotalInvoice;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RetroRegisterBatchBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RetroRegisterBatchBo bo)
        {
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.RetroRegister.ToString());

            var directRetroIds = RetroRegisterBatchDirectRetroService.GetIdsByRetroRegisterBatchId(bo.Id);
            foreach (var directRetroId in directRetroIds)
            {
                var dr = DirectRetroService.Find(directRetroId);
                if (dr != null)
                {
                    var drBo = dr;
                    drBo.RetroStatus = DirectRetroBo.RetroStatusApproved;
                    drBo.UpdatedById = bo.UpdatedById;
                    DirectRetroService.Save(ref drBo);
                }
            }

            RetroRegisterBatchDirectRetroService.DeleteAllByRetroRegisterBatchId(bo.Id);
            var files = RetroRegisterBatchFileService.GetByRetroRegisterBatchId(bo.Id);
            foreach (RetroRegisterBatchFileBo file in files)
            {
                string fileE2 = Path.Combine(Util.GetE2Path(), file.HashFileName);
                if (File.Exists(fileE2))
                    File.Delete(fileE2);
            }
            RetroRegisterBatchFileService.DeleteAllByRetroRegisterBatchId(bo.Id);
            RetroRegisterService.DeleteAllByRetroRegisterBatchId(bo.Id);
            StatusHistoryService.DeleteAllByModuleIdObjectId(moduleBo.Id, bo.Id);
            RemarkService.DeleteAllByModuleIdObjectId(moduleBo.Id, bo.Id);

            RetroRegisterBatch.Delete(bo.Id);
        }

        public static Result Delete(RetroRegisterBatchBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.RetroRegister.ToString());

                var directRetroIds = RetroRegisterBatchDirectRetroService.GetIdsByRetroRegisterBatchId(bo.Id);
                foreach (var directRetroId in directRetroIds)
                {
                    var dr = DirectRetroService.Find(directRetroId);
                    if (dr != null)
                    {
                        var drBo = dr;
                        drBo.RetroStatus = DirectRetroBo.RetroStatusApproved;
                        drBo.UpdatedById = bo.UpdatedById;
                        DirectRetroService.Save(ref drBo);
                    }
                }

                RetroRegisterBatchDirectRetroService.DeleteAllByRetroRegisterBatchId(bo.Id, ref trail);
                var files = RetroRegisterBatchFileService.GetByRetroRegisterBatchId(bo.Id);
                foreach (RetroRegisterBatchFileBo file in files)
                {
                    string fileE2 = Path.Combine(Util.GetE2Path(), file.HashFileName);
                    if (File.Exists(fileE2))
                        File.Delete(fileE2);
                }
                RetroRegisterBatchFileService.DeleteAllByRetroRegisterBatchId(bo.Id, ref trail);
                RetroRegisterService.DeleteAllByRetroRegisterBatchId(bo.Id, ref trail);
                RetroRegisterBatchFileService.DeleteAllByRetroRegisterBatchId(bo.Id);
                RetroRegisterBatchStatusFileService.DeleteByRetroRegisterBatchId(bo.Id);
                StatusHistoryService.DeleteAllByModuleIdObjectId(moduleBo.Id, bo.Id, ref trail);
                RemarkService.DeleteAllByModuleIdObjectId(moduleBo.Id, bo.Id, ref trail);

                DataTrail dataTrail = RetroRegisterBatch.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
