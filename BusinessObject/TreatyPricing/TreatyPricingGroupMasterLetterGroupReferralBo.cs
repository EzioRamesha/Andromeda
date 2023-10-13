namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingGroupMasterLetterGroupReferralBo
    {
        public int Id { get; set; }

        public int TreatyPricingGroupMasterLetterId { get; set; }
        public virtual TreatyPricingGroupMasterLetterBo TreatyPricingGroupMasterLetterBo { get; set; }

        public int TreatyPricingGroupReferralId { get; set; }
        public virtual TreatyPricingGroupReferralBo TreatyPricingGroupReferralBo { get; set; }

        public int CreatedById { get; set; }
    }
}
