namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingFinancialTableUploadBo
    {
        public int Id { get; set; }

        public int TreatyPricingFinancialTableVersionDetailId { get; set; }

        public virtual TreatyPricingFinancialTableVersionDetailBo TreatyPricingFinancialTableVersionDetailBo { get; set; }

        public int MinimumSumAssured { get; set; }

        public int MaximumSumAssured { get; set; }

        public string Code { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string StatusName { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedAtStr { get; set; }
    }
}
