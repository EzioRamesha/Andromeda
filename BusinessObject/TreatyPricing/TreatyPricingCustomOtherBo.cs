using BusinessObject.Identity;
using Shared;
using System;
using System.Collections.Generic;
using System.IO;


namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingCustomOtherBo : ObjectVersion
    {
        public int Id { get; set; }

        public int TreatyPricingCedantId { get; set; }

        public virtual TreatyPricingCedantBo TreatyPricingCedantBo { get; set; }

        public string Code { get; set; }

        public int Status { get; set; }
        public string StatusName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Errors { get; set; }

        public int CreatedById { get; set; }
        public virtual UserBo CreatedByBo { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public string CreatedAtStr { get; set; }

        public string CreatedByStr { get; set; }

        public bool IsDuplicateExisting { get; set; }
        public int? DuplicateTreatyPricingCustomOtherId { get; set; }
        public int? DuplicateTreatyPricingCustomOtherVersionId { get; set; }
        public bool DuplicateFromList { get; set; } = false;

        public IList<TreatyPricingCustomOtherVersionBo> TreatyPricingCustomOtherVersionBos { get; set; }

        public const int StatusActive = 1;
        public const int StatusInactive = 2;

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
