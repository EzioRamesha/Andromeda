using Shared;
using System.Collections.Generic;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingAdvantageProgramBo : ObjectVersion
    {
        public int Id { get; set; }

        public int TreatyPricingCedantId { get; set; }
        public virtual TreatyPricingCedantBo TreatyPricingCedantBo { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Status { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string StatusName { get; set; }

        public string BenefitCodeNames { get; set; }
        public string BenefitSumAssured { get; set; }
        public string BenefitEMs { get; set; }

        public bool IsDuplicateExisting { get; set; }
        public int? DuplicateTreatyPricingAdvantageProgramId { get; set; }
        public int? DuplicateTreatyPricingAdvantageProgramVersionId { get; set; }
        public bool DuplicateFromList { get; set; } = false;

        public IList<TreatyPricingAdvantageProgramVersionBo> TreatyPricingAdvantageProgramVersionBos { get; set; }

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
