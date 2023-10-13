namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingPerLifeRetroProductBo
    {
        public int TreatyPricingPerLifeRetroId { get; set; }

        public virtual TreatyPricingPerLifeRetroBo TreatyPricingPerLifeRetroBo { get; set; }

        public int TreatyPricingProductId { get; set; }

        public virtual TreatyPricingProductBo TreatyPricingProductBo { get; set; }

        public string PrimaryKey()
        {
            return string.Format("{0}{1}", TreatyPricingPerLifeRetroId, TreatyPricingProductId);
        }
    }
}
