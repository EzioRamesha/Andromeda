using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject
{
    public class RetroSummaryBo
    {
        public int Id { get; set; }

        public int DirectRetroId { get; set; }

        public DirectRetroBo DirectRetroBo { get; set; }

        public string RiskQuarter { get; set; }

        public int? Month { get; set; }

        public int? Year { get; set; }

        public string Type { get; set; }

        public int? NoOfPolicy { get; set; }

        public double? TotalSar { get; set; }

        public double? TotalRiPremium { get; set; }

        public double? TotalDiscount { get; set; }

        public int? NoOfClaims { get; set; }

        public double? TotalClaims { get; set; }

        public string RetroParty1 { get; set; }

        public string RetroParty2 { get; set; }

        public string RetroParty3 { get; set; }

        public double? RetroShare1 { get; set; }

        public double? RetroShare2 { get; set; }

        public double? RetroShare3 { get; set; }

        public double? RetroRiPremium1 { get; set; }

        public double? RetroRiPremium2 { get; set; }

        public double? RetroRiPremium3 { get; set; }

        public double? RetroDiscount1 { get; set; }

        public double? RetroDiscount2 { get; set; }

        public double? RetroDiscount3 { get; set; }

        public double? RetroClaims1 { get; set; }

        public double? RetroClaims2 { get; set; }

        public double? RetroClaims3 { get; set; }

        public string TreatyCode { get; set; }

        public double? RetroPremiumSpread1 { get; set; }

        public double? RetroPremiumSpread2 { get; set; }

        public double? RetroPremiumSpread3 { get; set; }

        public double? TotalDirectRetroAar { get; set; }

        public int ReportingType { get; set; }

        public int? Mfrs17AnnualCohort { get; set; }

        public string Mfrs17ContractCode { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        // Formatted value
        public string TotalSarStr { get; set; }

        public string TotalRiPremiumStr { get; set; }

        public string TotalDiscountStr { get; set; }

        public string TotalClaimsStr { get; set; }

        public string RetroShare1Str { get; set; }

        public string RetroShare2Str { get; set; }

        public string RetroShare3Str { get; set; }

        public string RetroRiPremium1Str { get; set; }

        public string RetroRiPremium2Str { get; set; }

        public string RetroRiPremium3Str { get; set; }

        public string RetroDiscount1Str { get; set; }

        public string RetroDiscount2Str { get; set; }

        public string RetroDiscount3Str { get; set; }

        public string RetroClaims1Str { get; set; }

        public string RetroClaims2Str { get; set; }

        public string RetroClaims3Str { get; set; }

        public string RetroPremiumSpread1Str { get; set; }

        public string RetroPremiumSpread2Str { get; set; }

        public string RetroPremiumSpread3Str { get; set; }

        public string TotalDirectRetroAarStr { get; set; }

        public const int ReportingTypeIFRS4 = 1;
        public const int ReportingTypeIFRS17 = 2;

        public static List<Column> GetColumns()
        {
            var columns = new List<Column>
            {
                new Column
                {
                    Header = "Treaty Code",
                    Property = "TreatyCode",
                },
                new Column
                {
                    Header = "SOA Quarter",
                    Property = "SoaQuarter",
                },
                new Column
                {
                    Header = "Risk Quarter",
                    Property = "RiskQuarter",
                },
                new Column
                {
                    Header = "Risk Month",
                    Property = "Month",
                },
                new Column
                {
                    Header = "Risk Year",
                    Property = "Year",
                },
                new Column
                {
                    Header = "Type",
                    Property = "Type",
                },
                new Column
                {
                    Header = "No of Policy",
                    Property = "NoOfPolicy",
                },
                new Column
                {
                    Header = "Total SAR(AAR)",
                    Property = "TotalSar",
                },
                new Column
                {
                    Header = "Total Direct Retro AAR",
                    Property = "TotalDirectRetroAar",
                },
                new Column
                {
                    Header = "Total RI Premium",
                    Property = "TotalRiPremium",
                },
                new Column
                {
                    Header = "Total Discount",
                    Property = "TotalDiscount",
                },
                new Column
                {
                    Header = "No of Claims",
                    Property = "NoOfClaims",
                },
                new Column
                {
                    Header = "Retro Party",
                    Property = "RetroParty1",
                },
                new Column
                {
                    Header = "Share (%)",
                    Property = "RetroShare1",
                },
                new Column
                {
                    Header = "Premium Spread",
                    Property = "RetroPremiumSpread1",
                },
                new Column
                {
                    Header = "RI Premium",
                    Property = "RetroRiPremium1",
                },
                new Column
                {
                    Header = "Discount",
                    Property = "RetroDiscount1",
                },
                new Column
                {
                    Header = "Claims",
                    Property = "RetroClaims1",
                },
                new Column
                {
                    Header = "Retro Party",
                    Property = "RetroParty2",
                },
                new Column
                {
                    Header = "Share (%)",
                    Property = "RetroShare2",
                },
                new Column
                {
                    Header = "Premium Spread",
                    Property = "RetroPremiumSpread2",
                },
                new Column
                {
                    Header = "RI Premium",
                    Property = "RetroRiPremium2",
                },
                new Column
                {
                    Header = "Discount",
                    Property = "RetroDiscount2",
                },
                new Column
                {
                    Header = "Claims",
                    Property = "RetroClaims2",
                },
                new Column
                {
                    Header = "Retro Party",
                    Property = "RetroParty3",
                },
                new Column
                {
                    Header = "Share (%)",
                    Property = "RetroShare3",
                },
                new Column
                {
                    Header = "Premium Spread",
                    Property = "RetroPremiumSpread3",
                },
                new Column
                {
                    Header = "RI Premium",
                    Property = "RetroRiPremium3",
                },
                new Column
                {
                    Header = "Discount",
                    Property = "RetroDiscount3",
                },
                new Column
                {
                    Header = "Claims",
                    Property = "RetroClaims3",
                },
            };

            return columns;
        }
    }
}
