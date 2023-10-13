using Shared;
using System;
using System.Collections.Generic;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingProfitCommissionBo : ObjectVersion
    {
        public int Id { get; set; }

        public int TreatyPricingCedantId { get; set; }

        public TreatyPricingCedantBo TreatyPricingCedantBo { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string BenefitCode { get; set; }

        public string Description { get; set; }

        public int Status { get; set; }

        public string StatusName { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Entitlement { get; set; }

        public string Remark { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public bool IsDuplicateExisting { get; set; }
        public int? DuplicateTreatyPricingProfitCommissionId { get; set; }
        public int? DuplicateTreatyPricingProfitCommissionVersionId { get; set; }
        public bool DuplicateFromList { get; set; } = false;

        public IList<TreatyPricingProfitCommissionVersionBo> TreatyPricingProfitCommissionVersionBos { get; set; }

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
