using Shared;
using System.Collections.Generic;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingPerLifeRetroBo : ObjectVersion
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public int? RetroPartyId { get; set; }
        public RetroPartyBo RetroPartyBo { get; set; }

        public int Type { get; set; }

        public double? RetrocessionaireShare { get; set; }

        public string Description { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public IList<TreatyPricingPerLifeRetroVersionBo> TreatyPricingPerLifeRetroVersionBos { get; set; }

        public string RetrocessionaireShareStr { get; set; }

        public const int TypeIndividual = 1;
        public const int TypeGroup = 2;
        public const int TypeMax = 2;

        public static string GetTypeName(int key)
        {
            switch (key)
            {
                case TypeIndividual:
                    return "Individual";
                case TypeGroup:
                    return "Group";
                default:
                    return "";
            }
        }

    }
}
