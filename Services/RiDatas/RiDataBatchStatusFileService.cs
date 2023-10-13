using BusinessObject;
using BusinessObject.RiDatas;
using DataAccess.Entities.RiDatas;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;

namespace Services.RiDatas
{
    public class RiDataBatchStatusFileService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RiDataBatchStatusFile)),
                Controller = ModuleBo.ModuleController.RiDataBatchStatusFile.ToString(),
            };
        }

        public static RiDataBatchStatusFileBo FormBo(RiDataBatchStatusFile entity = null)
        {
            if (entity == null)
                return null;
            return new RiDataBatchStatusFileBo
            {
                Id = entity.Id,
                RiDataBatchId = entity.RiDataBatchId,
                RiDataBatchBo = RiDataBatchService.Find(entity.RiDataBatchId),
                StatusHistoryId = entity.StatusHistoryId,
                StatusHistoryBo = StatusHistoryService.Find(entity.StatusHistoryId),
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<RiDataBatchStatusFileBo> FormBos(IList<RiDataBatchStatusFile> entities = null)
        {
            if (entities == null)
                return null;
            IList<RiDataBatchStatusFileBo> bos = new List<RiDataBatchStatusFileBo>() { };
            foreach (RiDataBatchStatusFile entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RiDataBatchStatusFile FormEntity(RiDataBatchStatusFileBo bo = null)
        {
            if (bo == null)
                return null;
            return new RiDataBatchStatusFile
            {
                Id = bo.Id,
                RiDataBatchId = bo.RiDataBatchId,
                StatusHistoryId = bo.StatusHistoryId,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static RiDataBatchStatusFileBo Find(int id)
        {
            return FormBo(RiDataBatchStatusFile.Find(id));
        }

        public static RiDataBatchStatusFileBo FindByRiDataBatchIdStatusHistoryId(int riDataBatchId, int statusHistoryId)
        {
            return FormBo(RiDataBatchStatusFile.FindByRiDataBatchIdStatusHistoryId(riDataBatchId, statusHistoryId));
        }

        public static IList<RiDataBatchStatusFileBo> GetRIDataBatchStatusByRiDataBatchId(int riDataBatchId)
        {
            return FormBos(RiDataBatchStatusFile.GetRiDataBatchStatusFileByRiDataBatchId(riDataBatchId));
        }

        public static Result Save(ref RiDataBatchStatusFileBo bo)
        {
            if (!RiDataBatchStatusFile.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref RiDataBatchStatusFileBo bo, ref TrailObject trail)
        {
            if (!RiDataBatchStatusFile.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RiDataBatchStatusFileBo bo)
        {
            RiDataBatchStatusFile entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RiDataBatchStatusFileBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RiDataBatchStatusFileBo bo)
        {
            Result result = Result();

            RiDataBatchStatusFile entity = RiDataBatchStatusFile.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.RiDataBatchId = bo.RiDataBatchId;
                entity.StatusHistoryId = bo.StatusHistoryId;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RiDataBatchStatusFileBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RiDataBatchStatusFileBo bo)
        {
            RiDataBatchStatusFile.Delete(bo.Id);
        }

        public static IList<DataTrail> DeleteAllByRiDataBatchId(int riDataBatchId)
        {
            return RiDataBatchStatusFile.DeleteAllByRiDataBatchId(riDataBatchId);
        }

        public static void DeleteAllByRiDataBatchId(int riDataBatchId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByRiDataBatchId(riDataBatchId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(RiDataFile)));
                }
            }
        }
    }
}
