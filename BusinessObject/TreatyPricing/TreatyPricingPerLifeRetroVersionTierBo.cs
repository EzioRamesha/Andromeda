using System.Collections.Generic;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingPerLifeRetroVersionTierBo
    {
        public int Id { get; set; }

        public int TreatyPricingPerLifeRetroVersionId { get; set; }

        public TreatyPricingPerLifeRetroVersionBo TreatyPricingPerLifeRetroVersionBo { get; set; }

        public string Col1 { get; set; }

        public string Col2 { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public const int DefaultRow = 6;

        public static List<TreatyPricingPerLifeRetroVersionTierBo> GetDefaultRow(int parentId, int currentRow)
        {
            List<TreatyPricingPerLifeRetroVersionTierBo> bos = new List<TreatyPricingPerLifeRetroVersionTierBo> { };
            if (currentRow < DefaultRow)
            {
                for (int i = currentRow; i < DefaultRow; i++)
                {
                    bos.Add(new TreatyPricingPerLifeRetroVersionTierBo
                    {
                        TreatyPricingPerLifeRetroVersionId = parentId,
                    });
                }
            }

            return bos;
        }
    }
}
