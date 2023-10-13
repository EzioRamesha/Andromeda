using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.TreatyPricing
{
    public class TreatyPricingUwLimitVersionService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingUwLimitVersion)),
                Controller = ModuleBo.ModuleController.TreatyPricingUwLimitVersion.ToString()
            };
        }

        public static TreatyPricingUwLimitVersionBo FormBo(TreatyPricingUwLimitVersion entity = null, bool foreign = false)
        {
            if (entity == null)
                return null;
            var bo = new TreatyPricingUwLimitVersionBo
            {
                Id = entity.Id,
                TreatyPricingUwLimitId = entity.TreatyPricingUwLimitId,
                Version = entity.Version,
                PersonInChargeId = entity.PersonInChargeId,
                EffectiveAt = entity.EffectiveAt,
                CurrencyCode = entity.CurrencyCode,
                UwLimit = entity.UwLimit,
                Remarks1 = entity.Remarks1,
                AblSumAssured = entity.AblSumAssured,
                Remarks2 = entity.Remarks2,
                AblMaxUwRating = entity.AblMaxUwRating,
                Remarks3 = entity.Remarks3,
                MaxSumAssured = entity.MaxSumAssured,
                PerLifePerIndustry = entity.PerLifePerIndustry,
                IssuePayoutLimit = entity.IssuePayoutLimit,
                Remarks4 = entity.Remarks4,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                //PersonInChargeName = UserService.Find(entity.PersonInChargeId).FullName,
            };
            if (bo.EffectiveAt.HasValue)
                bo.EffectiveAtStr = bo.EffectiveAt.Value.ToString(Util.GetDateFormat());

            if (foreign)
            {
                bo.TreatyPricingUwLimitBo = TreatyPricingUwLimitService.Find(entity.TreatyPricingUwLimitId);
                bo.PersonInChargeName = UserService.Find(entity.PersonInChargeId).FullName;
            }

            return bo;
        }

        public static TreatyPricingUwLimitVersionBo FormBoForProductAndBenefitDetailComparison(TreatyPricingUwLimitVersion entity = null, bool foreign = false)
        {
            if (entity == null)
                return null;
            var bo = new TreatyPricingUwLimitVersionBo
            {
                Id = entity.Id,
                TreatyPricingUwLimitId = entity.TreatyPricingUwLimitId,
                AblSumAssured = entity.AblSumAssured,
            };

            return bo;
        }

        public static IList<TreatyPricingUwLimitVersionBo> FormBos(IList<TreatyPricingUwLimitVersion> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingUwLimitVersionBo> bos = new List<TreatyPricingUwLimitVersionBo>() { };
            foreach (TreatyPricingUwLimitVersion entity in entities.OrderBy(i => i.Version))
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingUwLimitVersion FormEntity(TreatyPricingUwLimitVersionBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingUwLimitVersion
            {
                Id = bo.Id,
                TreatyPricingUwLimitId = bo.TreatyPricingUwLimitId,
                Version = bo.Version,
                PersonInChargeId = bo.PersonInChargeId,
                EffectiveAt = bo.EffectiveAt,
                CurrencyCode = bo.CurrencyCode,
                UwLimit = bo.UwLimit,
                Remarks1 = bo.Remarks1,
                AblSumAssured = bo.AblSumAssured,
                Remarks2 = bo.Remarks2,
                AblMaxUwRating = bo.AblMaxUwRating,
                Remarks3 = bo.Remarks3,
                MaxSumAssured = bo.MaxSumAssured,
                PerLifePerIndustry = bo.PerLifePerIndustry,
                IssuePayoutLimit = bo.IssuePayoutLimit,
                Remarks4 = bo.Remarks4,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingUwLimitVersion.IsExists(id);
        }

        public static TreatyPricingUwLimitVersionBo Find(int? id)
        {
            if (!id.HasValue)
                return null;

            return FormBo(TreatyPricingUwLimitVersion.Find(id.Value));
        }

        public static TreatyPricingUwLimitVersionBo FindForProductAndBenefitDetailComparison(int? id)
        {
            if (!id.HasValue)
                return null;

            return FormBoForProductAndBenefitDetailComparison(TreatyPricingUwLimitVersion.Find(id.Value));
        }

        public static IList<TreatyPricingUwLimitVersionBo> GetByTreatyPricingUwLimitId(int treatyPricingUwLimitId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingUwLimitVersions
                    .Where(q => q.TreatyPricingUwLimitId == treatyPricingUwLimitId)
                    .OrderByDescending(q => q.Version)
                    .ToList());
            }
        }

        public static TreatyPricingUwLimitVersionBo GetLatestByTreatyPricingUwLimitId(int treatyPricingUwLimitId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingUwLimitVersions
                    .Where(q => q.TreatyPricingUwLimitId == treatyPricingUwLimitId)
                    .OrderByDescending(q => q.Version)
                    .FirstOrDefault());
            }
        }

        public static IList<TreatyPricingUwLimitVersionBo> GetByTreatyPricingCedantId(int treatyPricingCedantId, bool foreign = false)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingUwLimitVersions
                    .Where(q => q.TreatyPricingUwLimit.TreatyPricingCedantId == treatyPricingCedantId)
                    .OrderBy(q => q.TreatyPricingUwLimitId).ThenBy(q => q.Version)
                    .ToList(), foreign);
            }
        }

        public static IList<TreatyPricingUwLimitVersionBo> GetByTreatyPricingCedantIds(List<int> treatyPricingCedantIds, bool foreign = false)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingUwLimitVersions
                    .Where(q => treatyPricingCedantIds.Contains(q.TreatyPricingUwLimit.TreatyPricingCedantId))
                    .OrderBy(q => q.TreatyPricingUwLimitId).ThenBy(q => q.Version)
                    .ToList(), foreign);
            }
        }

        public static IList<TreatyPricingUwLimitVersionBo> GetByCedantId(int cedantId, bool foreign = false)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingUwLimitVersions
                    .Where(q => q.TreatyPricingUwLimit.TreatyPricingCedant.CedantId == cedantId)
                    .OrderBy(q => q.TreatyPricingUwLimitId).ThenBy(q => q.Version)
                    .ToList(), foreign);
            }
        }

        public static Result Save(ref TreatyPricingUwLimitVersionBo bo)
        {
            if (!TreatyPricingUwLimitVersion.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingUwLimitVersionBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingUwLimitVersion.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingUwLimitVersionBo bo)
        {
            TreatyPricingUwLimitVersion entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingUwLimitVersionBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingUwLimitVersionBo bo)
        {
            Result result = Result();

            TreatyPricingUwLimitVersion entity = TreatyPricingUwLimitVersion.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity = FormEntity(bo);
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingUwLimitVersionBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingUwLimitVersionBo bo)
        {
            TreatyPricingUwLimitVersion.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingUwLimitVersionBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingUwLimitVersion.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingUwLimitId(int treatyPricingUwLimitId)
        {
            return TreatyPricingUwLimitVersion.DeleteAllByTreatyPricingUwLimitId(treatyPricingUwLimitId);
        }

        public static void DeleteAllByTreatyPricingUwLimitId(int treatyPricingUwLimitId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingUwLimitId(treatyPricingUwLimitId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingUwLimitVersion)));
                }
            }
        }
    }
}
