using BusinessObject;
using BusinessObject.Retrocession;
using DataAccess.Entities.Retrocession;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace Services.Retrocession
{
    public class PerLifeAggregationConflictListingService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PerLifeAggregationConflictListing)),
                Controller = ModuleBo.ModuleController.PerLifeAggregationConflictListing.ToString()
            };
        }

        public static PerLifeAggregationConflictListingBo FormBo(PerLifeAggregationConflictListing entity = null)
        {
            if (entity == null)
                return null;
            return new PerLifeAggregationConflictListingBo
            {
                Id = entity.Id,
                TreatyCodeId = entity.TreatyCodeId,
                TreatyCodeBo = TreatyCodeService.Find(entity.TreatyCodeId),
                RiskYear = entity.RiskYear,
                RiskMonth = entity.RiskMonth,
                InsuredName = entity.InsuredName,
                InsuredGenderCodePickListDetailId = entity.InsuredGenderCodePickListDetailId,
                InsuredGenderCodePickListDetailBo = PickListDetailService.Find(entity.InsuredGenderCodePickListDetailId),
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                InsuredDateOfBirthStr = entity.InsuredDateOfBirth.HasValue ? entity.InsuredDateOfBirth.Value.ToString(Util.GetDateFormat()) : "",
                PolicyNumber = entity.PolicyNumber,
                ReinsEffectiveDatePol = entity.ReinsEffectiveDatePol,
                ReinsEffectiveDatePolStr = entity.ReinsEffectiveDatePol.HasValue ? entity.ReinsEffectiveDatePol.Value.ToString(Util.GetDateFormat()) : "",
                AAR = entity.AAR,
                AARStr = Util.DoubleToString(entity.AAR),
                GrossPremium = entity.GrossPremium,
                GrossPremiumStr = Util.DoubleToString(entity.GrossPremium),
                NetPremium = entity.NetPremium,
                NetPremiumStr = Util.DoubleToString(entity.NetPremium),
                PremiumFrequencyModePickListDetailId = entity.PremiumFrequencyModePickListDetailId,
                PremiumFrequencyModePickListDetailBo = PickListDetailService.Find(entity.PremiumFrequencyModePickListDetailId),
                RetroPremiumFrequencyModePickListDetailId = entity.InsuredGenderCodePickListDetailId,
                RetroPremiumFrequencyModePickListDetailBo = PickListDetailService.Find(entity.InsuredGenderCodePickListDetailId),
                CedantPlanCode = entity.CedantPlanCode,
                CedingBenefitTypeCode = entity.CedingBenefitTypeCode,
                CedingBenefitRiskCode = entity.CedingBenefitRiskCode,
                MLReBenefitCodeId = entity.MLReBenefitCodeId,
                MLReBenefitCodeBo = BenefitService.Find(entity.MLReBenefitCodeId),
                TerritoryOfIssueCodePickListDetailId = entity.TerritoryOfIssueCodePickListDetailId,
                TerritoryOfIssueCodePickListDetailBo = PickListDetailService.Find(entity.TerritoryOfIssueCodePickListDetailId),

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<PerLifeAggregationConflictListingBo> FormBos(IList<PerLifeAggregationConflictListing> entities = null)
        {
            if (entities == null)
                return null;
            IList<PerLifeAggregationConflictListingBo> bos = new List<PerLifeAggregationConflictListingBo>() { };
            foreach (PerLifeAggregationConflictListing entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }
        public static PerLifeAggregationConflictListing FormEntity(PerLifeAggregationConflictListingBo bo = null)
        {
            if (bo == null)
                return null;
            return new PerLifeAggregationConflictListing
            {
                Id = bo.Id,
                TreatyCodeId = bo.TreatyCodeId,
                RiskYear = bo.RiskYear,
                RiskMonth = bo.RiskMonth,
                InsuredName = bo.InsuredName,
                InsuredGenderCodePickListDetailId = bo.InsuredGenderCodePickListDetailId,
                InsuredDateOfBirth = bo.InsuredDateOfBirth,
                PolicyNumber = bo.PolicyNumber,
                ReinsEffectiveDatePol = bo.ReinsEffectiveDatePol,
                AAR = bo.AAR,
                GrossPremium = bo.GrossPremium,
                NetPremium = bo.NetPremium,
                PremiumFrequencyModePickListDetailId = bo.PremiumFrequencyModePickListDetailId,
                RetroPremiumFrequencyModePickListDetailId = bo.RetroPremiumFrequencyModePickListDetailId,
                CedantPlanCode = bo.CedantPlanCode,
                CedingBenefitTypeCode = bo.CedingBenefitTypeCode,
                CedingBenefitRiskCode = bo.CedingBenefitRiskCode,
                MLReBenefitCodeId = bo.MLReBenefitCodeId,
                TerritoryOfIssueCodePickListDetailId = bo.TerritoryOfIssueCodePickListDetailId,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return PerLifeAggregationConflictListing.IsExists(id);
        }

        public static PerLifeAggregationConflictListingBo Find(int? id)
        {
            return FormBo(PerLifeAggregationConflictListing.Find(id));
        }

        public static IList<PerLifeAggregationConflictListingBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeAggregationConflictListings.OrderBy(q => q.TreatyCodeId).ToList());
            }
        }

        public static Result Save(ref PerLifeAggregationConflictListingBo bo)
        {
            if (!PerLifeAggregationConflictListing.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref PerLifeAggregationConflictListingBo bo, ref TrailObject trail)
        {
            if (!PerLifeAggregationConflictListing.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref PerLifeAggregationConflictListingBo bo)
        {
            PerLifeAggregationConflictListing entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref PerLifeAggregationConflictListingBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref PerLifeAggregationConflictListingBo bo)
        {
            Result result = Result();

            PerLifeAggregationConflictListing entity = PerLifeAggregationConflictListing.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyCodeId = bo.TreatyCodeId;
                entity.RiskYear = bo.RiskYear;
                entity.RiskMonth = bo.RiskMonth;
                entity.InsuredName = bo.InsuredName;
                entity.InsuredGenderCodePickListDetailId = bo.InsuredGenderCodePickListDetailId;
                entity.InsuredDateOfBirth = bo.InsuredDateOfBirth;
                entity.PolicyNumber = bo.PolicyNumber;
                entity.ReinsEffectiveDatePol = bo.ReinsEffectiveDatePol;
                entity.AAR = bo.AAR;
                entity.GrossPremium = bo.GrossPremium;
                entity.NetPremium = bo.NetPremium;
                entity.PremiumFrequencyModePickListDetailId = bo.PremiumFrequencyModePickListDetailId;
                entity.RetroPremiumFrequencyModePickListDetailId = bo.RetroPremiumFrequencyModePickListDetailId;
                entity.CedantPlanCode = bo.CedantPlanCode;
                entity.CedingBenefitTypeCode = bo.CedingBenefitTypeCode;
                entity.CedingBenefitRiskCode = bo.CedingBenefitRiskCode;
                entity.MLReBenefitCodeId = bo.MLReBenefitCodeId;
                entity.TerritoryOfIssueCodePickListDetailId = bo.TerritoryOfIssueCodePickListDetailId;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref PerLifeAggregationConflictListingBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(PerLifeAggregationConflictListingBo bo)
        {
            PerLifeAggregationConflictListing.Delete(bo.Id);
        }

        public static Result Delete(PerLifeAggregationConflictListingBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = PerLifeAggregationConflictListing.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

    }
}
