using BusinessObject;
using BusinessObject.RiDatas;
using BusinessObject.SoaDatas;
using DataAccess.Entities.RiDatas;
using DataAccess.Entities.SoaDatas;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;

namespace Services.SoaDatas
{
    public class SoaDataBatchStatusFileService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(SoaDataBatchStatusFile)),
                Controller = ModuleBo.ModuleController.SoaDataBatchStatusFile.ToString(),
            };
        }

        public static SoaDataBatchStatusFileBo FormBo(SoaDataBatchStatusFile entity = null)
        {
            if (entity == null)
                return null;
            return new SoaDataBatchStatusFileBo
            {
                Id = entity.Id,
                SoaDataBatchId = entity.SoaDataBatchId,
                SoaDataBatchBo = SoaDataBatchService.Find(entity.SoaDataBatchId),
                StatusHistoryId = entity.StatusHistoryId,
                StatusHistoryBo = StatusHistoryService.Find(entity.StatusHistoryId),
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<SoaDataBatchStatusFileBo> FormBos(IList<SoaDataBatchStatusFile> entities = null)
        {
            if (entities == null)
                return null;
            IList<SoaDataBatchStatusFileBo> bos = new List<SoaDataBatchStatusFileBo>() { };
            foreach (SoaDataBatchStatusFile entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static SoaDataBatchStatusFile FormEntity(SoaDataBatchStatusFileBo bo = null)
        {
            if (bo == null)
                return null;
            return new SoaDataBatchStatusFile
            {
                Id = bo.Id,
                SoaDataBatchId = bo.SoaDataBatchId,
                StatusHistoryId = bo.StatusHistoryId,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static SoaDataBatchStatusFileBo Find(int id)
        {
            return FormBo(SoaDataBatchStatusFile.Find(id));
        }

        public static SoaDataBatchStatusFileBo FindBySoaDataBatchIdStatusHistoryId(int soaDataBatchId, int statusHistoryId)
        {
            return FormBo(SoaDataBatchStatusFile.FindBySoaDataBatchIdStatusHistoryId(soaDataBatchId, statusHistoryId));
        }

        public static IList<SoaDataBatchStatusFileBo> GetSoaDataBatchStatusBySoaDataBatchId(int soaDataBatchId)
        {
            return FormBos(SoaDataBatchStatusFile.GetSoaDataBatchStatusFileBySoaDataBatchId(soaDataBatchId));
        }

        public static Result Save(ref SoaDataBatchStatusFileBo bo)
        {
            if (!SoaDataBatchStatusFile.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref SoaDataBatchStatusFileBo bo, ref TrailObject trail)
        {
            if (!SoaDataBatchStatusFile.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref SoaDataBatchStatusFileBo bo)
        {
            SoaDataBatchStatusFile entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref SoaDataBatchStatusFileBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref SoaDataBatchStatusFileBo bo)
        {
            Result result = Result();

            SoaDataBatchStatusFile entity = SoaDataBatchStatusFile.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.SoaDataBatchId = bo.SoaDataBatchId;
                entity.StatusHistoryId = bo.StatusHistoryId;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref SoaDataBatchStatusFileBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(SoaDataBatchStatusFileBo bo)
        {
            SoaDataBatchStatusFile.Delete(bo.Id);
        }

        public static IList<DataTrail> DeleteAllBySoaDataBatchId(int soaDataBatchId)
        {
            return SoaDataBatchStatusFile.DeleteAllBySoaDataBatchId(soaDataBatchId);
        }

        public static void DeleteAllBySoaDataBatchId(int soaDataBatchId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllBySoaDataBatchId(soaDataBatchId);
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
