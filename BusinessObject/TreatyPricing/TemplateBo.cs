namespace BusinessObject.TreatyPricing
{
    public class TemplateBo
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public int CedantId { get; set; }

        public virtual CedantBo CedantBo { get; set; }

        public string DocumentTypeId { get; set; }

        public string Description { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }
    }
}
