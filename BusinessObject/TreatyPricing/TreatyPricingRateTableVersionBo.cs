using BusinessObject.Identity;
using Shared.Trails.Attributes;
using System;
using System.Collections.Generic;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingRateTableVersionBo
    {
        public int Id { get; set; }

        public int TreatyPricingRateTableId { get; set; }

        public TreatyPricingRateTableBo TreatyPricingRateTableBo { get; set; }

        public int Version { get; set; }

        public int PersonInChargeId { get; set; }

        public UserBo PersonInChargeBo { get; set; }

        public string BenefitName { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public string EffectiveDateStr { get; set; }

        public int? AgeBasisPickListDetailId { get; set; }

        public PickListDetailBo AgeBasisPickListDetailBo { get; set; }

        public string RiDiscount { get; set; }

        public string CoinsuranceRiDiscount { get; set; }

        public int? RateGuaranteePickListDetailId { get; set; }

        public PickListDetailBo RateGuaranteePickListDetailBo { get; set; }

        public string RateGuaranteeForNewBusiness { get; set; }

        public string RateGuaranteeForRenewalBusiness { get; set; }

        public string AdvantageProgram { get; set; }

        public string ProfitCommission { get; set; }

        public string LargeSizeDiscount { get; set; }

        public string JuvenileLien { get; set; }

        public string SpecialLien { get; set; }

        public string AdditionalRemark { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        [ExcludeTrail]
        public string RateTableRate { get; set; }

        public IList<TreatyPricingRateTableDetailBo> TreatyPricingRateTableDetailBos { get; set; }

        // Rate Comparison Report
        public IList<TreatyPricingRateTableRateBo> TreatyPricingRateTableRateBos { get; set; }

        public List<RateDifferencePercentage> RateDifferencePercentages { get; set; }

        public string CedantName { get; set; }

        public string UnderwritingMethod { get; set; }

        public string ProductName { get; set; }

        public string ReinsuranceShare { get; set; }

        public string CedantRetention { get; set; }

        public string RecaptureClause { get; set; }

        public string MaxExpiryAge { get; set; }

        public string UnitRatePercentStr { get; set; }

        public TreatyPricingRateTableVersionBo() { }

        public TreatyPricingRateTableVersionBo(TreatyPricingRateTableVersionBo bo)
        {
            TreatyPricingRateTableId = bo.TreatyPricingRateTableId;
            Version = bo.Version;
            PersonInChargeId = bo.PersonInChargeId;
            BenefitName = bo.BenefitName;
            EffectiveDate = bo.EffectiveDate;
            EffectiveDateStr = bo.EffectiveDateStr;
            AgeBasisPickListDetailId = bo.AgeBasisPickListDetailId;
            RiDiscount = bo.RiDiscount;
            CoinsuranceRiDiscount = bo.CoinsuranceRiDiscount;
            RateGuaranteePickListDetailId = bo.RateGuaranteePickListDetailId;
            RateGuaranteeForNewBusiness = bo.RateGuaranteeForNewBusiness;
            RateGuaranteeForRenewalBusiness = bo.RateGuaranteeForRenewalBusiness;
            AdvantageProgram = bo.AdvantageProgram;
            ProfitCommission = bo.ProfitCommission;
            AdditionalRemark = bo.AdditionalRemark;
            LargeSizeDiscount = bo.LargeSizeDiscount;
            JuvenileLien = bo.JuvenileLien;
            SpecialLien = bo.SpecialLien;
            AdditionalRemark = bo.AdditionalRemark;
            RateTableRate = bo.RateTableRate;
        }
    }

    public class RateComparisonAgeRange
    {
        public int? Minimum { get; set; }

        public int? Maximum { get; set; }
    }

    public class RateDifferencePercentage
    {
        public string MalePercentStr { get; set; }

        public string FemalePercentStr { get; set; }

        public string UnisexPercentStr { get; set; }

        public string OccupationClassPercentStr { get; set; }

        public int Age { get; set; }
    }
}
