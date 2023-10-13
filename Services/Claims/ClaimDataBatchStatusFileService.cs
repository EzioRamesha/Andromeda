using BusinessObject;
using BusinessObject.Claims;
using DataAccess.Entities.Claims;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Claims
{
    public class ClaimDataBatchStatusFileService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ClaimDataBatchStatusFile)),
                Controller = ModuleBo.ModuleController.ClaimDataBatchStatusFile.ToString(),
            };
        }

        public static ClaimDataBatchStatusFileBo FormBo(ClaimDataBatchStatusFile entity = null)
        {
            if (entity == null)
                return null;
            return new ClaimDataBatchStatusFileBo
            {
                Id = entity.Id,
                ClaimDataBatchId = entity.ClaimDataBatchId,
                ClaimDataBatchBo = ClaimDataBatchService.Find(entity.ClaimDataBatchId),
                StatusHistoryId = entity.StatusHistoryId,
                StatusHistoryBo = StatusHistoryService.Find(entity.StatusHistoryId),
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<ClaimDataBatchStatusFileBo> FormBos(IList<ClaimDataBatchStatusFile> entities = null)
        {
            if (entities == null)
                return null;
            IList<ClaimDataBatchStatusFileBo> bos = new List<ClaimDataBatchStatusFileBo>() { };
            foreach (ClaimDataBatchStatusFile entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static ClaimDataBatchStatusFile FormEntity(ClaimDataBatchStatusFileBo bo = null)
        {
            if (bo == null)
                return null;
            return new ClaimDataBatchStatusFile
            {
                Id = bo.Id,
                ClaimDataBatchId = bo.ClaimDataBatchId,
                StatusHistoryId = bo.StatusHistoryId,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static ClaimDataBatchStatusFileBo Find(int id)
        {
            return FormBo(ClaimDataBatchStatusFile.Find(id));
        }

        public static ClaimDataBatchStatusFileBo FindByClaimDataBatchIdStatusHistoryId(int claimDataBatchId, int statusHistoryId)
        {
            return FormBo(ClaimDataBatchStatusFile.FindByClaimDataBatchIdStatusHistoryId(claimDataBatchId, statusHistoryId));
        }

        public static IList<ClaimDataBatchStatusFileBo> GetByClaimDataBatchId(int claimDataBatchId)
        {
            return FormBos(ClaimDataBatchStatusFile.GetByClaimDataBatchId(claimDataBatchId));
        }

        public static Result Save(ref ClaimDataBatchStatusFileBo bo)
        {
            if (!ClaimDataBatchStatusFile.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref ClaimDataBatchStatusFileBo bo, ref TrailObject trail)
        {
            if (!ClaimDataBatchStatusFile.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref ClaimDataBatchStatusFileBo bo)
        {
            ClaimDataBatchStatusFile entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref ClaimDataBatchStatusFileBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ClaimDataBatchStatusFileBo bo)
        {
            Result result = Result();

            ClaimDataBatchStatusFile entity = ClaimDataBatchStatusFile.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.ClaimDataBatchId = bo.ClaimDataBatchId;
                entity.StatusHistoryId = bo.StatusHistoryId;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref ClaimDataBatchStatusFileBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ClaimDataBatchStatusFileBo bo)
        {
            ClaimDataBatchStatusFile.Delete(bo.Id);
        }

        public static IList<DataTrail> DeleteAllByClaimDataBatchId(int claimDataBatchId)
        {
            return ClaimDataBatchStatusFile.DeleteAllByClaimDataBatchId(claimDataBatchId);
        }

        public static void DeleteAllByClaimDataBatchId(int claimDataBatchId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByClaimDataBatchId(claimDataBatchId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(ClaimDataBatchStatusFile)));
                }
            }
        }
    }
}
