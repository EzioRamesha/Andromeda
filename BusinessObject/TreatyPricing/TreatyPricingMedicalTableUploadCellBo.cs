namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingMedicalTableUploadCellBo
    {
        public int Id { get; set; }

        public int TreatyPricingMedicalTableUploadColumnId { get; set; }

        public virtual TreatyPricingMedicalTableUploadColumnBo TreatyPricingMedicalTableUploadColumnBo { get; set; }

        public int TreatyPricingMedicalTableUploadRowId { get; set; }

        public virtual TreatyPricingMedicalTableUploadRowBo TreatyPricingMedicalTableUploadRowBo { get; set; }

        public string Code { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string StatusName { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedAtStr { get; set; }
    }
}
