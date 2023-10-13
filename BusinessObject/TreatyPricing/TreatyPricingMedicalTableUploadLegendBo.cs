namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingMedicalTableUploadLegendBo
    {
        public int Id { get; set; }

        public int TreatyPricingMedicalTableVersionDetailId { get; set; }

        public virtual TreatyPricingMedicalTableVersionDetailBo TreatyPricingMedicalTableVersionDetailBo { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string StatusName { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedAtStr { get; set; }
    }
}
