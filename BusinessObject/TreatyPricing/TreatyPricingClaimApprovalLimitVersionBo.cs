using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingClaimApprovalLimitVersionBo
    {
        public int Id { get; set; }

        public int TreatyPricingClaimApprovalLimitId { get; set; }

        public virtual TreatyPricingClaimApprovalLimitBo TreatyPricingClaimApprovalLimitBo { get; set; }

        public int Version { get; set; }

        public int? PersonInChargeId { get; set; }

        public string PersonInChargeName { get; set; }

        public DateTime? EffectiveAt { get; set; }

        public string EffectiveAtStr { get; set; }

        public string Amount { get; set; }

        public string AdditionalRemarks { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public TreatyPricingClaimApprovalLimitVersionBo()
        {

        }

        public TreatyPricingClaimApprovalLimitVersionBo(TreatyPricingClaimApprovalLimitVersionBo bo)
        {
            TreatyPricingClaimApprovalLimitId = bo.TreatyPricingClaimApprovalLimitId;
            PersonInChargeId = bo.PersonInChargeId;
            PersonInChargeName = bo.PersonInChargeName;
            EffectiveAt = bo.EffectiveAt;
            EffectiveAtStr = bo.EffectiveAtStr;
            Amount = bo.Amount;
            AdditionalRemarks = bo.AdditionalRemarks;
        }
    }
}
