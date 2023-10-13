using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingProductPerLifeRetroBo
    {
        public int Id { get; set; }

        public int TreatyPricingProductId { get; set; }

        public TreatyPricingProductBo TreatyPricingProductBo { get; set; }

        public int TreatyPricingPerLifeRetroId { get; set; }

        public string TreatyPricingPerLifeRetroCode { get; set; }

        public string Warning { get; set; }

        public int CreatedById { get; set; }
    }
}
