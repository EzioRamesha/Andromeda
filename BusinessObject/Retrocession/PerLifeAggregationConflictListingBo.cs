using System;

namespace BusinessObject.Retrocession
{
    public class PerLifeAggregationConflictListingBo
    {
        public int Id { get; set; }
        public string TreatyCodeStr { get; set; }
        public int? TreatyCodeId { get; set; }
        public TreatyCodeBo TreatyCodeBo { get; set; }

        public int? RiskYear { get; set; }

        public int? RiskMonth { get; set; }

        public string InsuredName { get; set; }

        public string InsuredGenderCode { get; set; }
        public int? InsuredGenderCodePickListDetailId { get; set; }
        public PickListDetailBo InsuredGenderCodePickListDetailBo { get; set; }

        public DateTime? InsuredDateOfBirth { get; set; }
        public string InsuredDateOfBirthStr { get; set; }

        public string PolicyNumber { get; set; }

        public DateTime? ReinsEffectiveDatePol { get; set; }
        public string ReinsEffectiveDatePolStr { get; set; }

        public double? AAR { get; set; }
        public string AARStr { get; set; }

        public double? GrossPremium { get; set; }
        public string GrossPremiumStr { get; set; }

        public double? NetPremium { get; set; }
        public string NetPremiumStr { get; set; }

        public string PremiumFrequencyMode { get; set; }
        public int? PremiumFrequencyModePickListDetailId { get; set; }
        public PickListDetailBo PremiumFrequencyModePickListDetailBo { get; set; }

        public string RetroPremiumFrequencyMode { get; set; }
        public int? RetroPremiumFrequencyModePickListDetailId { get; set; }
        public PickListDetailBo RetroPremiumFrequencyModePickListDetailBo { get; set; }

        public string CedantPlanCode { get; set; }

        public string CedingBenefitRiskCode { get; set; }

        public string CedingBenefitTypeCode { get; set; }

        public string MLReBenefitCode { get; set; }
        public string MLReBenefitCodeStr { get; set; }
        public int? MLReBenefitCodeId { get; set; }
        public BenefitBo MLReBenefitCodeBo { get; set; }

        public string TerritoryOfIssueCode { get; set; }
        public int? TerritoryOfIssueCodePickListDetailId { get; set; }
        public PickListDetailBo TerritoryOfIssueCodePickListDetailBo { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public const int RiskMonthJanuary = 1;
        public const int RiskMonthFebuary = 2;
        public const int RiskMonthMarch = 3;
        public const int RiskMonthApril = 4;
        public const int RiskMonthMay = 5;
        public const int RiskMonthJune = 6;
        public const int RiskMonthJuly = 7;
        public const int RiskMonthAugust = 8;
        public const int RiskMonthSeptember = 9;
        public const int RiskMonthOctober = 10;
        public const int RiskMonthNovember = 11;
        public const int RiskMonthDecember = 12;

        public static string GetRiskMonthName(int key)
        {
            switch (key)
            {
                case RiskMonthJanuary:
                    return "January";
                case RiskMonthFebuary:
                    return "Febuary";
                case RiskMonthMarch:
                    return "March";
                case RiskMonthApril:
                    return "April";
                case RiskMonthMay:
                    return "May";
                case RiskMonthJune:
                    return "June";
                case RiskMonthJuly:
                    return "July";
                case RiskMonthAugust:
                    return "August";
                case RiskMonthSeptember:
                    return "September";
                case RiskMonthOctober:
                    return "October";
                case RiskMonthNovember:
                    return "November";
                case RiskMonthDecember:
                    return "December";
                default:
                    return "";
            }
                
        }

    }
}
