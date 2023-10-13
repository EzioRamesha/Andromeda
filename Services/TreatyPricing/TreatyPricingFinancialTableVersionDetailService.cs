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
    public class TreatyPricingFinancialTableVersionDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingFinancialTableVersionDetail)),
                Controller = ModuleBo.ModuleController.TreatyPricingFinancialTableVersionDetail.ToString()
            };
        }

        public static Expression<Func<TreatyPricingFinancialTableVersionDetail, TreatyPricingFinancialTableVersionDetailBo>> Expression()
        {
            return entity => new TreatyPricingFinancialTableVersionDetailBo
            {
                Id = entity.Id,
                TreatyPricingFinancialTableVersionId = entity.TreatyPricingFinancialTableVersionId,
                DistributionTierPickListDetailId = entity.DistributionTierPickListDetailId,
                Description = entity.Description,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingFinancialTableVersionDetailBo FormBo(TreatyPricingFinancialTableVersionDetail entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingFinancialTableVersionDetailBo
            {
                Id = entity.Id,
                TreatyPricingFinancialTableVersionId = entity.TreatyPricingFinancialTableVersionId,
                TreatyPricingFinancialTableVersionBo = TreatyPricingFinancialTableVersionService.Find(entity.TreatyPricingFinancialTableVersionId),
                DistributionTierPickListDetailId = entity.DistributionTierPickListDetailId,
                DistributionTierPickListDetailBo = PickListDetailService.Find(entity.DistributionTierPickListDetailId),
                Description = entity.Description,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                DistributionTier = entity.GetDistributionTier(entity.DistributionTierPickListDetailId),
            };
        }

        public static IList<TreatyPricingFinancialTableVersionDetailBo> FormBos(IList<TreatyPricingFinancialTableVersionDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingFinancialTableVersionDetailBo> bos = new List<TreatyPricingFinancialTableVersionDetailBo>() { };
            foreach (TreatyPricingFinancialTableVersionDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingFinancialTableVersionDetail FormEntity(TreatyPricingFinancialTableVersionDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingFinancialTableVersionDetail
            {
                Id = bo.Id,
                TreatyPricingFinancialTableVersionId = bo.TreatyPricingFinancialTableVersionId,
                DistributionTierPickListDetailId = bo.DistributionTierPickListDetailId,
                Description = bo.Description,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingFinancialTableVersionDetail.IsExists(id);
        }

        public static TreatyPricingFinancialTableVersionDetailBo Find(int id)
        {
            return FormBo(TreatyPricingFinancialTableVersionDetail.Find(id));
        }

        public static IList<TreatyPricingFinancialTableVersionDetailBo> GetByTreatyPricingFinancialTableVersionId(int treatyPricingFinancialTableVersionId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingFinancialTableVersionDetails
                    .Where(q => q.TreatyPricingFinancialTableVersionId == treatyPricingFinancialTableVersionId)
                    .ToList());
            }
        }

        public static TreatyPricingFinancialTableVersionDetailBo GetByVersionIdDistributionTierPickListDetailId(int versionId, int distributionTierPickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingFinancialTableVersionDetails
                    .Where(q => q.TreatyPricingFinancialTableVersionId == versionId && q.DistributionTierPickListDetailId == distributionTierPickListDetailId)
                    .FirstOrDefault());
            }
        }

        public static Result Save(ref TreatyPricingFinancialTableVersionDetailBo bo)
        {
            if (!TreatyPricingFinancialTableVersionDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingFinancialTableVersionDetailBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingFinancialTableVersionDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingFinancialTableVersionDetailBo bo)
        {
            TreatyPricingFinancialTableVersionDetail entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingFinancialTableVersionDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingFinancialTableVersionDetailBo bo)
        {
            Result result = Result();

            TreatyPricingFinancialTableVersionDetail entity = TreatyPricingFinancialTableVersionDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingFinancialTableVersionId = bo.TreatyPricingFinancialTableVersionId;
                entity.DistributionTierPickListDetailId = bo.DistributionTierPickListDetailId;
                entity.Description = bo.Description;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingFinancialTableVersionDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingFinancialTableVersionDetailBo bo)
        {
            TreatyPricingFinancialTableVersionDetail.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingFinancialTableVersionDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingFinancialTableVersionDetail.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingFinancialTableVersionId(int treatyPricingFinancialTableVersionId)
        {
            return TreatyPricingFinancialTableVersionDetail.DeleteAllByTreatyPricingFinancialTableVersionId(treatyPricingFinancialTableVersionId);
        }

        public static void DeleteAllByTreatyPricingFinancialTableVersionId(int treatyPricingFinancialTableVersionId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingFinancialTableVersionId(treatyPricingFinancialTableVersionId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingFinancialTableVersionDetail)));
                }
            }
        }
    }
}
