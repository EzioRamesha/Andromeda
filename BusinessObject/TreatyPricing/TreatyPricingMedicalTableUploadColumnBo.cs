namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingMedicalTableUploadColumnBo
    {
        public int Id { get; set; }

        public int TreatyPricingMedicalTableVersionDetailId { get; set; }

        public virtual TreatyPricingMedicalTableVersionDetailBo TreatyPricingMedicalTableVersionDetailBo { get; set; }

        public int MinimumAge { get; set; }

        public int MaximumAge { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string StatusName { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedAtStr { get; set; }
    }
}
