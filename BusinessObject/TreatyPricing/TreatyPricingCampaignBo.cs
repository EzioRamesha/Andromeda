using Shared;
using System;
using System.Collections.Generic;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingCampaignBo : ObjectVersion
    {
        public int Id { get; set; }

        public int TreatyPricingCedantId { get; set; }
        public virtual TreatyPricingCedantBo TreatyPricingCedantBo { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int Status { get; set; }

        public string Type { get; set; }

        public string Purpose { get; set; }

        public DateTime? PeriodStartDate { get; set; }

        public DateTime? PeriodEndDate { get; set; }

        public string Period { get; set; }

        public string Duration { get; set; }

        public string TargetTakeUpRate { get; set; }

        public string AverageSumAssured { get; set; }

        public string RiPremiumReceivable { get; set; }

        public string NoOfPolicy { get; set; }

        public string TreatyPricingCampaignProduct { get; set; }
        public List<TreatyPricingCampaignProductBo> TreatyPricingCampaignProductBo { get; set; }
        public List<string> TreatypricingCampaignProducts { get; set; }

        public string Remarks { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string StatusName { get; set; }
        public string PeriodStartDateStr { get; set; }
        public string PeriodEndDateStr { get; set; }
        public string AverageSumAssuredStr { get; set; }
        public string RiPremiumReceivableStr { get; set; }

        public bool IsDuplicateExisting { get; set; }
        public int? DuplicateTreatyPricingCampaignId { get; set; }
        public int? DuplicateTreatyPricingCampaignVersionId { get; set; }
        public bool DuplicateFromList { get; set; } = false;

        public IList<TreatyPricingCampaignVersionBo> TreatyPricingCampaignVersionBos { get; set; }

        public const int StatusActive = 1;
        public const int StatusInactive = 2;
        public const int StatusMax = 2;

        public const int TypeNML = 1;
        public const int TypeNFL = 2;
        public const int TypeComplimentarySA = 3;
        public const int TypeUpSell = 4;
        public const int TypeCrossSell = 5;
        public const int TypeGIO = 6;
        public const int TypeSIO = 7;
        public const int TypeAdvantageProgram = 8;
        public const int TypeUnderwritingProgram = 9;
        public const int TypeMax = 9;

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

        public static string GetTypeName(int key)
        {
            switch (key)
            {
                case TypeNML:
                    return "NML";
                case TypeNFL:
                    return "NFL";
                case TypeComplimentarySA:
                    return "Complimentary SA";
                case TypeUpSell:
                    return "Up-sell";
                case TypeCrossSell:
                    return "Cross-Sell";
                case TypeGIO:
                    return "GIO";
                case TypeSIO:
                    return "SIO";
                case TypeAdvantageProgram:
                    return "Advantage Program";
                case TypeUnderwritingProgram:
                    return "Underwriting Program";
                default:
                    return "";
            }
        }
    }
}
