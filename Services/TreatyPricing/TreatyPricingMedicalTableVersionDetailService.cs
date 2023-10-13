using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.TreatyPricing
{
    public class TreatyPricingMedicalTableVersionDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingMedicalTableVersionDetail)),
                Controller = ModuleBo.ModuleController.TreatyPricingMedicalTableVersionDetail.ToString()
            };
        }

        public static Expression<Func<TreatyPricingMedicalTableVersionDetail, TreatyPricingMedicalTableVersionDetailBo>> Expression()
        {
            return entity => new TreatyPricingMedicalTableVersionDetailBo
            {
                Id = entity.Id,
                TreatyPricingMedicalTableVersionId = entity.TreatyPricingMedicalTableVersionId,
                DistributionTierPickListDetailId = entity.DistributionTierPickListDetailId,
                Description = entity.Description,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingMedicalTableVersionDetailBo FormBo(TreatyPricingMedicalTableVersionDetail entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingMedicalTableVersionDetailBo
            {
                Id = entity.Id,
                TreatyPricingMedicalTableVersionId = entity.TreatyPricingMedicalTableVersionId,
                TreatyPricingMedicalTableVersionBo = TreatyPricingMedicalTableVersionService.Find(entity.TreatyPricingMedicalTableVersionId),
                DistributionTierPickListDetailId = entity.DistributionTierPickListDetailId,
                DistributionTierPickListDetailBo = PickListDetailService.Find(entity.DistributionTierPickListDetailId),
                Description = entity.Description,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                DistributionTier = entity.GetDistributionTier(entity.DistributionTierPickListDetailId),
            };
        }

        public static IList<TreatyPricingMedicalTableVersionDetailBo> FormBos(IList<TreatyPricingMedicalTableVersionDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingMedicalTableVersionDetailBo> bos = new List<TreatyPricingMedicalTableVersionDetailBo>() { };
            foreach (TreatyPricingMedicalTableVersionDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingMedicalTableVersionDetail FormEntity(TreatyPricingMedicalTableVersionDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingMedicalTableVersionDetail
            {
                Id = bo.Id,
                TreatyPricingMedicalTableVersionId = bo.TreatyPricingMedicalTableVersionId,
                DistributionTierPickListDetailId = bo.DistributionTierPickListDetailId,
                Description = bo.Description,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingMedicalTableVersionDetail.IsExists(id);
        }

        public static TreatyPricingMedicalTableVersionDetailBo Find(int id)
        {
            return FormBo(TreatyPricingMedicalTableVersionDetail.Find(id));
        }

        public static IList<TreatyPricingMedicalTableVersionDetailBo> GetByTreatyPricingMedicalTableVersionId(int treatyPricingMedicalTableVersionId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingMedicalTableVersionDetails
                    .Where(q => q.TreatyPricingMedicalTableVersionId == treatyPricingMedicalTableVersionId)
                    .ToList());
            }
        }

        public static TreatyPricingMedicalTableVersionDetailBo GetByVersionIdDistributionTierPickListDetailId(int versionId, int distributionTierPickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingMedicalTableVersionDetails
                    .Where(q => q.TreatyPricingMedicalTableVersionId == versionId && q.DistributionTierPickListDetailId == distributionTierPickListDetailId)
                    .FirstOrDefault());
            }
        }

        public static Result Save(ref TreatyPricingMedicalTableVersionDetailBo bo)
        {
            if (!TreatyPricingMedicalTableVersionDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingMedicalTableVersionDetailBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingMedicalTableVersionDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingMedicalTableVersionDetailBo bo)
        {
            TreatyPricingMedicalTableVersionDetail entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingMedicalTableVersionDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingMedicalTableVersionDetailBo bo)
        {
            Result result = Result();

            TreatyPricingMedicalTableVersionDetail entity = TreatyPricingMedicalTableVersionDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingMedicalTableVersionId = bo.TreatyPricingMedicalTableVersionId;
                entity.DistributionTierPickListDetailId = bo.DistributionTierPickListDetailId;
                entity.Description = bo.Description;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingMedicalTableVersionDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingMedicalTableVersionDetailBo bo)
        {
            TreatyPricingMedicalTableVersionDetail.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingMedicalTableVersionDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingMedicalTableVersionDetail.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingMedicalTableVersionId(int treatyPricingMedicalTableVersionId)
        {
            return TreatyPricingMedicalTableVersionDetail.DeleteAllByTreatyPricingMedicalTableVersionId(treatyPricingMedicalTableVersionId);
        }

        public static void DeleteAllByTreatyPricingMedicalTableVersionId(int treatyPricingMedicalTableVersionId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingMedicalTableVersionId(treatyPricingMedicalTableVersionId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingMedicalTableVersionDetail)));
                }
            }
        }
    }
}
