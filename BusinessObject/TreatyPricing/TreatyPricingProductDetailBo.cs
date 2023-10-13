using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingProductDetailBo
    {
        public int Id { get; set; }

        public int TreatyPricingProductVersionId { get; set; }

        public TreatyPricingProductVersionBo TreatyPricingProductVersionBo { get; set; }

        public int Type { get; set; }

        public string Col1 { get; set; }

        public string Col2 { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public const int TypeJuvenileLien = 1;
        public const int TypeSpecialLien = 2;

        public const int TypeMax = 2;

        public static string GetTypeName(int key)
        {
            switch (key)
            {
                case TypeJuvenileLien:
                    return "Juvenile Lien";
                case TypeSpecialLien:
                    return "Special Lien";
                default:
                    return "";
            }
        }
    }
}
