using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingUwLimitVersionBo
    {
        public int Id { get; set; }

        public int TreatyPricingUwLimitId { get; set; }

        public virtual TreatyPricingUwLimitBo TreatyPricingUwLimitBo { get; set; }

        public int Version { get; set; }

        public int? PersonInChargeId { get; set; }

        public DateTime? EffectiveAt { get; set; }

        [MaxLength(3)]
        public string CurrencyCode { get; set; }

        public string UwLimit { get; set; }

        public string Remarks1 { get; set; }

        public string AblSumAssured { get; set; }

        public string Remarks2 { get; set; }

        public string AblMaxUwRating { get; set; }

        public string Remarks3 { get; set; }

        public string MaxSumAssured { get; set; }

        public bool PerLifePerIndustry { get; set; }

        public bool IssuePayoutLimit { get; set; }

        public string Remarks4 { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string EffectiveAtStr { get; set; }
        public string PersonInChargeName { get; set; }

        public const int PerLifeIndustryPerLife = 0;
        public const int PerLifeIndustryPerLifeIndustry = 1;

        public const int IssuePayoutLimitIssue = 0;
        public const int IssuePayoutLimitPayout = 1;

        public TreatyPricingUwLimitVersionBo()
        {

        }

        public TreatyPricingUwLimitVersionBo(TreatyPricingUwLimitVersionBo bo)
        {
            TreatyPricingUwLimitId = bo.TreatyPricingUwLimitId;
            PersonInChargeId = bo.PersonInChargeId;
            EffectiveAt = bo.EffectiveAt;
            EffectiveAtStr = bo.EffectiveAtStr;
            CurrencyCode = bo.CurrencyCode;
            UwLimit = bo.UwLimit;
            Remarks1 = bo.Remarks1;
            AblSumAssured = bo.AblSumAssured;
            Remarks2 = bo.Remarks2;
            AblMaxUwRating = bo.AblMaxUwRating;
            Remarks3 = bo.Remarks3;
            MaxSumAssured = bo.MaxSumAssured;
            PerLifePerIndustry = bo.PerLifePerIndustry;
            IssuePayoutLimit = bo.IssuePayoutLimit;
            Remarks4 = bo.Remarks4;
        }

        public static string GetPerLifeIndustryName(int key)
        {
            switch (key)
            {
                case PerLifeIndustryPerLife:
                    return "Per Life";
                case PerLifeIndustryPerLifeIndustry:
                    return "Per Life Industry";
                default:
                    return "";
            }
        }

        public static string GetIssuePayoutLimitName(int key)
        {
            switch (key)
            {
                case IssuePayoutLimitIssue:
                    return "Issue";
                case IssuePayoutLimitPayout:
                    return "Payout";
                default:
                    return "";
            }
        }
    }
}
