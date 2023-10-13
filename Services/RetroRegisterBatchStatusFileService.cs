using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class RetroRegisterBatchStatusFileService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RetroRegisterBatchStatusFile)),
                Controller = ModuleBo.ModuleController.RetroRegisterBatchStatusFile.ToString(),
            };
        }

        public static RetroRegisterBatchStatusFileBo FormBo(RetroRegisterBatchStatusFile entity = null)
        {
            if (entity == null)
                return null;
            return new RetroRegisterBatchStatusFileBo
            {
                Id = entity.Id,
                RetroRegisterBatchId = entity.RetroRegisterBatchId,
                RetroRegisterBatchBo = RetroRegisterBatchService.Find(entity.RetroRegisterBatchId),
                StatusHistoryId = entity.StatusHistoryId,
                StatusHistoryBo = StatusHistoryService.Find(entity.StatusHistoryId),
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<RetroRegisterBatchStatusFileBo> FormBos(IList<RetroRegisterBatchStatusFile> entities = null)
        {
            if (entities == null)
                return null;
            IList<RetroRegisterBatchStatusFileBo> bos = new List<RetroRegisterBatchStatusFileBo>() { };
            foreach (RetroRegisterBatchStatusFile entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RetroRegisterBatchStatusFile FormEntity(RetroRegisterBatchStatusFileBo bo = null)
        {
            if (bo == null)
                return null;
            return new RetroRegisterBatchStatusFile
            {
                Id = bo.Id,
                RetroRegisterBatchId = bo.RetroRegisterBatchId,
                StatusHistoryId = bo.StatusHistoryId,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static RetroRegisterBatchStatusFileBo Find(int id)
        {
            return FormBo(RetroRegisterBatchStatusFile.Find(id));
        }

        public static RetroRegisterBatchStatusFileBo FindByRetroRegisterBatchIdStatusHistoryId(int retroRegisterBatchId, int statusHistoryId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.RetroRegisterBatchStatusFiles.Where(q => q.RetroRegisterBatchId == retroRegisterBatchId && q.StatusHistoryId == statusHistoryId).FirstOrDefault());
            }
        }

        public static IList<RetroRegisterBatchStatusFileBo> GetByRetroRegisterBatchId(int retroRegisterBatchId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.RetroRegisterBatchStatusFiles.Where(q => q.RetroRegisterBatchId == retroRegisterBatchId).OrderByDescending(q => q.CreatedAt).ToList());
            }
        }

        public static Result Save(ref RetroRegisterBatchStatusFileBo bo)
        {
            if (!RetroRegisterBatchStatusFile.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref RetroRegisterBatchStatusFileBo bo, ref TrailObject trail)
        {
            if (!RetroRegisterBatchStatusFile.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RetroRegisterBatchStatusFileBo bo)
        {
            RetroRegisterBatchStatusFile entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RetroRegisterBatchStatusFileBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RetroRegisterBatchStatusFileBo bo)
        {
            Result result = Result();

            RetroRegisterBatchStatusFile entity = RetroRegisterBatchStatusFile.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.RetroRegisterBatchId = bo.RetroRegisterBatchId;
                entity.StatusHistoryId = bo.StatusHistoryId;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RetroRegisterBatchStatusFileBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RetroRegisterBatchStatusFileBo bo)
        {
            RetroRegisterBatchStatusFile.Delete(bo.Id);
        }

        public static IList<DataTrail> DeleteByRetroRegisterBatchId(int RetroRegisterBatchId)
        {
            return RetroRegisterBatchStatusFile.DeleteAllByRetroRegisterBatchId(RetroRegisterBatchId);
        }

        public static void DeleteByRetroRegisterBatchId(int RetroRegisterBatchId, ref TrailObject trail)
        {
            Result result = Result();
            IList<DataTrail> dataTrails = DeleteByRetroRegisterBatchId(RetroRegisterBatchId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, result.Table);
                }
            }
        }
    }
}
