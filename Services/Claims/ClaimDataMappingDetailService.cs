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
    public class ClaimDataMappingDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ClaimDataMappingDetail)),
                Controller = ModuleBo.ModuleController.ClaimDataMappingDetail.ToString(),
            };
        }

        public static ClaimDataMappingDetailBo FormBo(ClaimDataMappingDetail entity = null)
        {
            if (entity == null)
                return null;

            PickListDetailBo pickListDetailBo = PickListDetailService.Find(entity.PickListDetailId);
            return new ClaimDataMappingDetailBo
            {
                Id = entity.Id,
                ClaimDataMappingId = entity.ClaimDataMappingId,
                //ClaimDataMappingBo = ClaimDataMappingService.Find(entity.ClaimDataMappingId), // DO NOT LOAD THIS. IT WILL CAUSE INFINITE LOOP.
                PickListDetailId = entity.PickListDetailId,
                PickListDetailBo = pickListDetailBo,
                PickListDetailCode = pickListDetailBo?.Code,
                RawValue = entity.RawValue,
                IsPickDetailIdEmpty = entity.IsPickDetailIdEmpty,
                IsRawValueEmpty = entity.IsRawValueEmpty,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<ClaimDataMappingDetailBo> FormBos(IList<ClaimDataMappingDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<ClaimDataMappingDetailBo> bos = new List<ClaimDataMappingDetailBo>() { };
            foreach (ClaimDataMappingDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static ClaimDataMappingDetail FormEntity(ClaimDataMappingDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new ClaimDataMappingDetail
            {
                Id = bo.Id,
                ClaimDataMappingId = bo.ClaimDataMappingId,
                PickListDetailId = bo.PickListDetailId,
                RawValue = bo.RawValue,
                IsPickDetailIdEmpty = bo.IsPickDetailIdEmpty,
                IsRawValueEmpty = bo.IsRawValueEmpty,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static ClaimDataMappingDetailBo Find(int id)
        {
            return FormBo(ClaimDataMappingDetail.Find(id));
        }

        public static int CountByPickListDetailId(int pickListDetailId)
        {
            return ClaimDataMappingDetail.CountByPickListDetailId(pickListDetailId);
        }

        public static IList<ClaimDataMappingDetailBo> GetByClaimDataMappingId(int claimDataMappingId)
        {
            return FormBos(ClaimDataMappingDetail.GetByClaimDataMappingId(claimDataMappingId));
        }

        public static Result Save(ref ClaimDataMappingDetailBo bo)
        {
            if (!ClaimDataMapping.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref ClaimDataMappingDetailBo bo, ref TrailObject trail)
        {
            if (!ClaimDataMapping.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref ClaimDataMappingDetailBo bo)
        {
            ClaimDataMappingDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref ClaimDataMappingDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ClaimDataMappingDetailBo bo)
        {
            Result result = Result();

            ClaimDataMappingDetail entity = ClaimDataMappingDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.ClaimDataMappingId = bo.ClaimDataMappingId;
                entity.PickListDetailId = bo.PickListDetailId;
                entity.RawValue = bo.RawValue;
                entity.IsPickDetailIdEmpty = bo.IsPickDetailIdEmpty;
                entity.IsRawValueEmpty = bo.IsRawValueEmpty;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref ClaimDataMappingDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ClaimDataMappingDetailBo bo)
        {
            ClaimDataMappingDetail.Delete(bo.Id);
        }

        public static Result Delete(ClaimDataMappingDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();
            DataTrail dataTrail = ClaimDataMappingDetail.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);
            return result;
        }

        public static Result DeleteByDataMappingIdExcept(int claimDataMappingId, List<int> ids, ref TrailObject trail)
        {
            Result result = Result();
            IList<ClaimDataMappingDetail> claimDataMappingDetails = ClaimDataMappingDetail.GetByClaimDataMappingIdExcept(claimDataMappingId, ids);
            foreach (ClaimDataMappingDetail claimDataMappingDetail in claimDataMappingDetails)
            {
                DataTrail dataTrail = ClaimDataMappingDetail.Delete(claimDataMappingDetail.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static IList<DataTrail> DeleteAllByClaimDataMappingId(int claimDataMappingId)
        {
            return ClaimDataMappingDetail.DeleteAllByClaimDataMappingId(claimDataMappingId);
        }

        public static void DeleteAllByClaimDataMappingId(int claimDataMappingId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByClaimDataConfigId(claimDataMappingId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(ClaimDataMappingDetail)));
                }
            }
        }

        public static IList<DataTrail> DeleteAllByClaimDataConfigId(int claimDataMappingId)
        {
            return ClaimDataMappingDetail.DeleteAllByClaimDataConfigId(claimDataMappingId);
        }

        public static void DeleteAllByClaimDataConfigId(int claimDataConfigId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByClaimDataConfigId(claimDataConfigId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(ClaimDataMappingDetail)));
                }
            }
        }
    }
}
