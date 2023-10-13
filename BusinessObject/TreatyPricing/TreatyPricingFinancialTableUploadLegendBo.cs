namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingFinancialTableUploadLegendBo
    {
        public int Id { get; set; }

        public int TreatyPricingFinancialTableVersionDetailId { get; set; }

        public virtual TreatyPricingFinancialTableVersionDetailBo TreatyPricingFinancialTableVersionDetailBo { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string StatusName { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedAtStr { get; set; }
    }
}
