using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingTierProfitCommissionBo
    {
        public int Id { get; set; }

        public int TreatyPricingProfitCommissionVersionId { get; set; }

        public TreatyPricingProfitCommissionVersionBo TreatyPricingProfitCommissionVersionBo { get; set; }

        public string Col1 { get; set; }

        public string Col2 { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public const int DefaultRow = 6;

        public static List<TreatyPricingTierProfitCommissionBo> GetDefaultRow(int parentId, int currentRow)
        {
            List<TreatyPricingTierProfitCommissionBo> bos = new List<TreatyPricingTierProfitCommissionBo> { };
            if (currentRow < DefaultRow)
            {
                for (int i = currentRow; i < DefaultRow; i++)
                {
                    bos.Add(new TreatyPricingTierProfitCommissionBo
                    {
                        TreatyPricingProfitCommissionVersionId = parentId,
                    });
                }
            }

            return bos;
        }
    }
}
