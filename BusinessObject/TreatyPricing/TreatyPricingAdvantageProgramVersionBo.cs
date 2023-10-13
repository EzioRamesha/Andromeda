using BusinessObject.Identity;
using Shared.Trails.Attributes;
using System;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingAdvantageProgramVersionBo
    {
        public int Id { get; set; }

        public int TreatyPricingAdvantageProgramId { get; set; }
        public TreatyPricingAdvantageProgramBo TreatyPricingAdvantageProgramBo { get; set; }

        public int Version { get; set; }

        public int PersonInChargeId { get; set; }
        public UserBo PersonInChargeBo { get; set; }

        public DateTime? EffectiveAt { get; set; }

        public string Retention { get; set; }

        public string MlreShare { get; set; }

        public string Remarks { get; set; }

        [IsJsonProperty("BenefitId")]
        public string TreatyPricingAdvantageProgramVersionBenefit { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string EffcetiveAtStr { get; set; }
        public string RetentionStr { get; set; }
        public string MlreShareStr { get; set; }


        public TreatyPricingAdvantageProgramVersionBo() { }

        public TreatyPricingAdvantageProgramVersionBo(TreatyPricingAdvantageProgramVersionBo bo)
        {
            TreatyPricingAdvantageProgramId = bo.TreatyPricingAdvantageProgramId;
            Version = bo.Version;
            PersonInChargeId = bo.PersonInChargeId;
            EffectiveAt = bo.EffectiveAt;
            EffcetiveAtStr = bo.EffcetiveAtStr;
            Retention = bo.Retention;
            MlreShare = bo.MlreShare;
            Remarks = bo.Remarks;
            TreatyPricingAdvantageProgramVersionBenefit = bo.TreatyPricingAdvantageProgramVersionBenefit;
        }
    }
}
