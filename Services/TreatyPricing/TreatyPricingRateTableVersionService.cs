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
    public class TreatyPricingRateTableVersionService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingRateTableVersion)),
                Controller = ModuleBo.ModuleController.TreatyPricingRateTableVersion.ToString()
            };
        }

        public static Expression<Func<TreatyPricingRateTableVersion, TreatyPricingRateTableVersionBo>> Expression()
        {
            return entity => new TreatyPricingRateTableVersionBo
            {
                Id = entity.Id,
                TreatyPricingRateTableId = entity.TreatyPricingRateTableId,
                Version = entity.Version,
                PersonInChargeId = entity.PersonInChargeId,
                BenefitName = entity.BenefitName,
                EffectiveDate = entity.EffectiveDate,
                AgeBasisPickListDetailId = entity.AgeBasisPickListDetailId,
                RiDiscount = entity.RiDiscount,
                CoinsuranceRiDiscount = entity.CoinsuranceRiDiscount,
                RateGuaranteePickListDetailId = entity.RateGuaranteePickListDetailId,
                RateGuaranteeForNewBusiness = entity.RateGuaranteeForNewBusiness,
                RateGuaranteeForRenewalBusiness = entity.RateGuaranteeForRenewalBusiness,
                AdvantageProgram = entity.AdvantageProgram,
                ProfitCommission = entity.ProfitCommission,
                AdditionalRemark = entity.AdditionalRemark,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingRateTableVersionBo FormBo(TreatyPricingRateTableVersion entity = null, bool foreign = false)
        {
            if (entity == null)
                return null;
            return new TreatyPricingRateTableVersionBo
            {
                Id = entity.Id,
                TreatyPricingRateTableId = entity.TreatyPricingRateTableId,
                TreatyPricingRateTableBo = foreign ? TreatyPricingRateTableService.Find(entity.TreatyPricingRateTableId) : null,
                Version = entity.Version,
                PersonInChargeId = entity.PersonInChargeId,
                PersonInChargeBo = foreign ? UserService.Find(entity.PersonInChargeId) : null,
                BenefitName = entity.BenefitName,
                EffectiveDate = entity.EffectiveDate,
                AgeBasisPickListDetailId = entity.AgeBasisPickListDetailId,
                AgeBasisPickListDetailBo = foreign ? PickListDetailService.Find(entity.AgeBasisPickListDetailId) : null,
                RiDiscount = entity.RiDiscount,
                CoinsuranceRiDiscount = entity.CoinsuranceRiDiscount,
                RateGuaranteePickListDetailId = entity.RateGuaranteePickListDetailId,
                RateGuaranteePickListDetailBo = foreign ? PickListDetailService.Find(entity.RateGuaranteePickListDetailId) : null,
                RateGuaranteeForNewBusiness = entity.RateGuaranteeForNewBusiness,
                RateGuaranteeForRenewalBusiness = entity.RateGuaranteeForRenewalBusiness,
                AdvantageProgram = entity.AdvantageProgram,
                ProfitCommission = entity.ProfitCommission,
                LargeSizeDiscount = foreign ? TreatyPricingRateTableDetailService.GetJsonByTreatyPricingRateTableVersionIdType(entity.Id, TreatyPricingRateTableDetailBo.TypeLargeSizeDiscount) : null,
                JuvenileLien = foreign ? TreatyPricingRateTableDetailService.GetJsonByTreatyPricingRateTableVersionIdType(entity.Id, TreatyPricingRateTableDetailBo.TypeJuvenileLien) : null,
                SpecialLien = foreign ? TreatyPricingRateTableDetailService.GetJsonByTreatyPricingRateTableVersionIdType(entity.Id, TreatyPricingRateTableDetailBo.TypeSpecialLien) : null,
                AdditionalRemark = entity.AdditionalRemark,
                RateTableRate = foreign ? TreatyPricingRateTableRateService.GetJsonByTreatyPricingRateTableVersionId(entity.Id) : null,

                // Date to string
                EffectiveDateStr = entity.EffectiveDate?.ToString(Util.GetDateFormat()),

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<TreatyPricingRateTableVersionBo> FormBos(IList<TreatyPricingRateTableVersion> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingRateTableVersionBo> bos = new List<TreatyPricingRateTableVersionBo>() { };
            foreach (TreatyPricingRateTableVersion entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingRateTableVersion FormEntity(TreatyPricingRateTableVersionBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingRateTableVersion
            {
                Id = bo.Id,
                TreatyPricingRateTableId = bo.TreatyPricingRateTableId,
                Version = bo.Version,
                PersonInChargeId = bo.PersonInChargeId,
                BenefitName = bo.BenefitName,
                EffectiveDate = bo.EffectiveDate,
                AgeBasisPickListDetailId = bo.AgeBasisPickListDetailId,
                RiDiscount = bo.RiDiscount,
                CoinsuranceRiDiscount = bo.CoinsuranceRiDiscount,
                RateGuaranteePickListDetailId = bo.RateGuaranteePickListDetailId,
                RateGuaranteeForNewBusiness = bo.RateGuaranteeForNewBusiness,
                RateGuaranteeForRenewalBusiness = bo.RateGuaranteeForRenewalBusiness,
                AdvantageProgram = bo.AdvantageProgram,
                ProfitCommission = bo.ProfitCommission,
                AdditionalRemark = bo.AdditionalRemark,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingRateTableVersion.IsExists(id);
        }

        public static TreatyPricingRateTableVersionBo Find(int? id, bool foreign = false)
        {
            return FormBo(TreatyPricingRateTableVersion.Find(id), foreign);
        }

        public static TreatyPricingRateTableVersionBo FindByParentIdVersion(int parentId, int version)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingRateTableVersions
                    .Where(q => q.TreatyPricingRateTableId == parentId)
                    .Where(q => q.Version == version)
                    .FirstOrDefault());
            }
        }

        public static TreatyPricingRateTableVersionBo FindLatestByTreatyPricingRateTableId(int treatyPricingRateTableId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingRateTableVersions.Where(q => q.TreatyPricingRateTableId == treatyPricingRateTableId).OrderByDescending(q => q.CreatedAt).FirstOrDefault());
            }
        }

        public static IList<TreatyPricingRateTableVersionBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingRateTableVersions.ToList());
            }
        }

        public static List<int> GetVersionByTreatyPricingRateTableId(int treatyPricingRateTableId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingRateTableVersions.Where(q => q.TreatyPricingRateTableId == treatyPricingRateTableId).OrderBy(q => q.Version).Select(q => q.Version).ToList();
            }
        }

        public static IList<TreatyPricingRateTableVersionBo> GetByTreatyPricingRateTableId(int treatyPricingRateTableId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingRateTableVersions.Where(q => q.TreatyPricingRateTableId == treatyPricingRateTableId).ToList());
            }
        }

        public static IList<TreatyPricingRateTableVersionBo> GetByTreatyPricingCedantId(int treatyPricingCedantId, bool foreign = false)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingRateTableVersions
                    .Where(q => q.TreatyPricingRateTable.TreatyPricingRateTableGroup.TreatyPricingCedantId == treatyPricingCedantId)
                    .OrderBy(q => q.TreatyPricingRateTableId).ThenBy(q => q.Version)
                    .ToList(), foreign);
            }
        }

        public static List<int> GetIdByRateTableVersionIdsBenefitId(List<int> rateTableVersionIds, int benefitId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingRateTableVersions
                    .Where(q => rateTableVersionIds.Contains(q.Id))
                    .Where(q => q.TreatyPricingRateTable.BenefitId == benefitId)
                    .Select(q => q.Id)
                    .ToList();
            }
        }

        public static List<int> GetIdByRateTableVersionIdsBenefitName(List<int> rateTableVersionIds, string benefitName)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingRateTableVersions
                    .Where(q => rateTableVersionIds.Contains(q.Id))
                    .Where(q => !string.IsNullOrEmpty(q.BenefitName))
                    .Where(q => q.BenefitName == benefitName)
                    .Select(q => q.Id)
                    .ToList();
            }
        }

        public static List<int> GetIdByRateTableVersionIdsEffectiveDate(List<int> rateTableVersionIds, DateTime effectiveDate)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingRateTableVersions
                    .Where(q => rateTableVersionIds.Contains(q.Id))
                    .Where(q => q.EffectiveDate.HasValue)
                    .Where(q => q.EffectiveDate == effectiveDate)
                    .Select(q => q.Id)
                    .ToList();
            }
        }

        public static List<string> GetDistinctBenefitNameByRateTableVersionIds(List<int> rateTableVersionIds)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingRateTableVersions
                    .Where(q => rateTableVersionIds.Contains(q.Id))
                    .Where(q => !string.IsNullOrEmpty(q.BenefitName))
                    .GroupBy(q => q.BenefitName)
                    .Select(r => r.FirstOrDefault())
                    .OrderBy(q => q.BenefitName)
                    .Select(q => q.BenefitName)
                    .ToList();
            }
        }

        public static List<string> GetDistinctEffectiveDateByRateTableVersionIds(List<int> rateTableVersionIds)
        {
            using (var db = new AppDbContext())
            {
                var effectiveDates = new List<string> { };
                var dates = db.TreatyPricingRateTableVersions
                    .Where(q => rateTableVersionIds.Contains(q.Id))
                    .Where(q => q.EffectiveDate.HasValue)
                    .GroupBy(q => q.EffectiveDate)
                    .Select(g => g.FirstOrDefault())
                    .OrderBy(q => q.EffectiveDate)
                    .Select(q => q.EffectiveDate.Value)
                    .ToList();

                foreach (var date in dates)
                {
                    effectiveDates.Add(date.ToString(Util.GetDateFormat()));
                }

                return effectiveDates;
            }
        }

        public static IList<TreatyPricingRateTableBo> GetRateTableByRateTableVersionIds(List<int> rateTableVersionIds)
        {
            using (var db = new AppDbContext())
            {
                var ids = db.TreatyPricingRateTableVersions
                    .Where(q => rateTableVersionIds.Contains(q.Id))
                    .GroupBy(q => q.TreatyPricingRateTableId)
                    .Select(g => g.FirstOrDefault())
                    .OrderBy(q => q.TreatyPricingRateTableId)
                    .Select(q => q.TreatyPricingRateTableId)
                    .ToList();

                return TreatyPricingRateTableService.FormBos(db.TreatyPricingRateTables
                    .Where(q => ids.Contains(q.Id))
                    .ToList());
            }
        }

        public static List<int> GetVersionByRateTableVersionIdsRateTableId(List<int> rateTableVersionIds, int rateTableId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingRateTableVersions
                    .Where(q => rateTableVersionIds.Contains(q.Id))
                    .Where(q => q.TreatyPricingRateTableId == rateTableId)
                    .Select(q => q.Version)
                    .ToList();
            }
        }

        public static int CountByTreatyPricingRateTableId(int treatyPricingRateTableId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingRateTableVersions.Where(q => q.TreatyPricingRateTableId == treatyPricingRateTableId).Count();
            }
        }

        public static int CountByAgeBasisPickListDetailId(int ageBasisPickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingRateTableVersions.Where(q => q.AgeBasisPickListDetailId == ageBasisPickListDetailId).Count();
            }
        }

        public static int CountByRateGuaranteePickListDetailId(int rateGuaranteePickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingRateTableVersions.Where(q => q.RateGuaranteePickListDetailId == rateGuaranteePickListDetailId).Count();
            }
        }

        public static Result Save(ref TreatyPricingRateTableVersionBo bo)
        {
            if (!TreatyPricingRateTableVersion.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingRateTableVersionBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingRateTableVersion.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingRateTableVersionBo bo)
        {
            TreatyPricingRateTableVersion entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingRateTableVersionBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingRateTableVersionBo bo)
        {
            Result result = Result();

            TreatyPricingRateTableVersion entity = TreatyPricingRateTableVersion.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingRateTableId = bo.TreatyPricingRateTableId;
                entity.Version = bo.Version;
                entity.PersonInChargeId = bo.PersonInChargeId;
                entity.BenefitName = bo.BenefitName;
                entity.EffectiveDate = bo.EffectiveDate;
                entity.AgeBasisPickListDetailId = bo.AgeBasisPickListDetailId;
                entity.RiDiscount = bo.RiDiscount;
                entity.CoinsuranceRiDiscount = bo.CoinsuranceRiDiscount;
                entity.RateGuaranteePickListDetailId = bo.RateGuaranteePickListDetailId;
                entity.RateGuaranteeForNewBusiness = bo.RateGuaranteeForNewBusiness;
                entity.RateGuaranteeForRenewalBusiness = bo.RateGuaranteeForRenewalBusiness;
                entity.AdvantageProgram = bo.AdvantageProgram;
                entity.ProfitCommission = bo.ProfitCommission;
                entity.AdditionalRemark = bo.AdditionalRemark;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingRateTableVersionBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingRateTableVersionBo bo)
        {
            TreatyPricingRateTableVersion.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingRateTableVersionBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingRateTableVersion.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
