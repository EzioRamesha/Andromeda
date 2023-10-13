using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingFinancialTableBo : ObjectVersion
    {
        public int Id { get; set; }

        public int TreatyPricingCedantId { get; set; }

        public virtual TreatyPricingCedantBo TreatyPricingCedantBo { get; set; }

        public string FinancialTableId { get; set; }

        public int Status { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string BenefitCode { get; set; }

        public string DistributionChannel { get; set; }

        [MaxLength(3)]
        public string CurrencyCode { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public bool IsDuplicateExisting { get; set; }
        public int? DuplicateTreatyPricingFinancialTableId { get; set; }
        public int? DuplicateTreatyPricingFinancialTableVersionId { get; set; }
        public bool DuplicateFromList { get; set; } = false;

        public IList<TreatyPricingFinancialTableVersionBo> TreatyPricingFinancialTableVersionBos { get; set; }

        public string StatusName { get; set; }

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

        #region Financial Table Comparison Report
        public TreatyPricingFinancialTableVersionBo VersionBo { get; set; }

        public TreatyPricingFinancialTableVersionDetailBo DetailBo { get; set; }

        public List<string> FinancialTableDetailComparisons { get; set; }

        public string CedantInfo { get; set; }

        public string LinkedProducts { get; set; }

        public string Legends { get; set; }
        #endregion
    }
}
