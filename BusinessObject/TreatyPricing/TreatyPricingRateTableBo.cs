using Shared;
using System;
using System.Collections.Generic;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingRateTableBo : ObjectVersion
    {
        public int Id { get; set; }

        public int TreatyPricingRateTableGroupId { get; set; }

        public TreatyPricingRateTableGroupBo TreatyPricingRateTableGroupBo { get; set; }

        public int BenefitId { get; set; }

        public BenefitBo BenefitBo { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Status { get; set; }

        public string StatusName { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public bool IsDuplicateExisting { get; set; }

        public int? DuplicateTreatyPricingRateTableId { get; set; }

        public IList<TreatyPricingRateTableVersionBo> TreatyPricingRateTableVersionBos { get; set; }

        public const int StatusActive = 1;
        public const int StatusInactive = 2;

        public const int StatusMax = 2;

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusActive:
                    return "Active";
                case StatusInactive:
                    return "Inactive";
                default:
                    return "";
            }
        }

        public static string GetStatusClass(int key)
        {
            switch (key)
            {
                case StatusActive:
                    return "status-success-badge";
                case StatusInactive:
                    return "status-fail-badge";
                default:
                    return "";
            }
        }
    }
}
