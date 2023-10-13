namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingGroupMasterLetterBo
    {
        public int Id { get; set; }

        public int CedantId { get; set; }
        public CedantBo CedantBo { get; set; }

        public string Code { get; set; }

        public int NoOfRiGroupSlip { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }
    }
}
