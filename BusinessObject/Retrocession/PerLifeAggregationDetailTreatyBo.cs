using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject.Retrocession
{
    public class PerLifeAggregationDetailTreatyBo
    {
        public int Id { get; set; }

        public int PerLifeAggregationDetailId { get; set; }

        public PerLifeAggregationDetailBo PerLifeAggregationDetailBo { get; set; }

        public string TreatyCode { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        // Additional
        public string RiskQuarter { get; set; }

        public int Count { get; set; }

        public double? TotalAar { get; set; }

        public double? TotalGrossPremium { get; set; }

        public double? TotalNetPremium { get; set; }

        public string TotalAarStr { get; set; }

        public string TotalGrossPremiumStr { get; set; }

        public string TotalNetPremiumStr { get; set; }

        public int Count2 { get; set; }

        public double? TotalAar2 { get; set; }

        public double? TotalGrossPremium2 { get; set; }

        public double? TotalNetPremium2 { get; set; }

        public string TotalAarStr2 { get; set; }

        public string TotalGrossPremiumStr2 { get; set; }

        public string TotalNetPremiumStr2 { get; set; }

        public int Count3 { get; set; }

        public double? TotalRetroAmount3 { get; set; }

        public double? TotalGrossPremium3 { get; set; }

        public double? TotalNetPremium3 { get; set; }

        public string TotalRetroAmountStr3 { get; set; }

        public string TotalGrossPremiumStr3 { get; set; }

        public string TotalNetPremiumStr3 { get; set; }

        public double? TotalDiscount3 { get; set; }

        public string TotalDiscountStr3 { get; set; }

        public static List<Column> GetColumns()
        {
            return new List<Column>
            {
                new Column
                {
                    Header = "ID",
                    Property = "Id",
                },
                new Column
                {
                    Header = "Per Life Aggregation Detail ID",
                    Property = "PerLifeAggregationDetailId",
                },
                new Column
                {
                    Header = "Treaty Code",
                    Property = "TreatyCode",
                },
            };
        }

        public static List<Column> GetExcludedSummaryColumns()
        {
            return new List<Column>
            {
                new Column
                {
                    Header = "Risk Quarter",
                    Property = "RiskQuarter",
                },
                new Column
                {
                    Header = "Treaty Code",
                    Property = "TreatyCode",
                },
                new Column
                {
                    Header = "Count",
                    Property = "Count",
                },
                new Column
                {
                    Header = "Total AAR",
                    Property = "TotalAar",
                },
                new Column
                {
                    Header = "Total Gross Premium",
                    Property = "TotalGrossPremium",
                },
                new Column
                {
                    Header = "Total Net Premium",
                    Property = "TotalNetPremium",
                },
            };
        }

        public static List<Column> GetRetroSummaryColumns()
        {
            return new List<Column>
            {
                new Column
                {
                    Header = "Risk Quarter",
                    Property = "RiskQuarter",
                },
                new Column
                {
                    Header = "Treaty Code",
                    Property = "TreatyCode",
                },

                new Column
                {
                    Header = "Count - Good",
                    Property = "Count",
                },
                new Column
                {
                    Header = "Total AAR - Good",
                    Property = "TotalAar",
                },
                new Column
                {
                    Header = "Total Gross Premium - Good",
                    Property = "TotalGrossPremium",
                },
                new Column
                {
                    Header = "Total Net Premium - Good",
                    Property = "TotalNetPremium",
                },

                new Column
                {
                    Header = "Count - Retain",
                    Property = "Count2",
                },
                new Column
                {
                    Header = "Total AAR - Retain",
                    Property = "TotalAar2",
                },
                new Column
                {
                    Header = "Total Gross Premium - Retain",
                    Property = "TotalGrossPremium2",
                },
                new Column
                {
                    Header = "Total Net Premium - Retain",
                    Property = "TotalNetPremium2",
                },

                new Column
                {
                    Header = "Count - Retro Party",
                    Property = "Count3",
                },
                new Column
                {
                    Header = "Total Retro Amount - Retro Party",
                    Property = "TotalRetroAmount3",
                },
                new Column
                {
                    Header = "Total Gross Premium - Retro Party",
                    Property = "TotalGrossPremium3",
                },
                new Column
                {
                    Header = "Total Net Premium - Retro Party",
                    Property = "TotalNetPremium3",
                },

                new Column
                {
                    Header = "Total Discount - Retro Party",
                    Property = "TotalDiscount3",
                },
            };
        }
    }
}
