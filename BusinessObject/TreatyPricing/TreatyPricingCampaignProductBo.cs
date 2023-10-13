using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingCampaignProductBo
    {
        public int TreatyPricingCampaignId { get; set; }

        public virtual TreatyPricingCampaignBo TreatyPricingCampaignBo { get; set; }

        public int TreatyPricingProductId { get; set; }

        public virtual TreatyPricingProductBo TreatyPricingProductBo { get; set; }

        public string PrimaryKey()
        {
            return string.Format("{0}{1}", TreatyPricingCampaignId, TreatyPricingProductId);
        }
    }
}
