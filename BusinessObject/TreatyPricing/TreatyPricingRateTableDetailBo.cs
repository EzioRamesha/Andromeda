using System;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingRateTableDetailBo
    {
        public int Id { get; set; }

        public int TreatyPricingRateTableVersionId { get; set; }

        public TreatyPricingRateTableVersionBo TreatyPricingRateTableVersionBo { get; set; }

        public int Type { get; set; }

        public string Col1 { get; set; }

        public string Col2 { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public const int TypeLargeSizeDiscount = 1;
        public const int TypeJuvenileLien = 2;
        public const int TypeSpecialLien = 3;

        public const int TypeMax = 3;

        public static string GetTypeName(int key)
        {
            switch (key)
            {
                case TypeLargeSizeDiscount:
                    return "Large Size Discount";
                case TypeJuvenileLien:
                    return "Juvenile Lien";
                case TypeSpecialLien:
                    return "Special Lien";
                default:
                    return "";
            }
        }

        public static string[] InsertFields()
        {
            return new string[]
            {
                "[TreatyPricingRateTableVersionId]",
                "[Type]",
                "[Col1]",
                "[Col2]",
                "[CreatedAt]",
                "[UpdatedAt]",
                "[UpdatedById]",
                "[CreatedById]",
            };
        }

        public static string[] QueryFields()
        {
            return new string[]
            {
                "[Type]",
                "[Col1]",
                "[Col2]",
            };
        }
    }
}
