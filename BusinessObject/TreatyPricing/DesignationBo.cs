namespace BusinessObject.TreatyPricing
{
    public class DesignationBo
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }
    }
}
