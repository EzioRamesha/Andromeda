using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject.Retrocession
{
    public class PerLifeRetroConfigurationRatioBo
    {
        public int Id { get; set; }

        public int TreatyCodeId { get; set; }

        public TreatyCodeBo TreatyCodeBo { get; set; }

        public string TreatyCode { get; set; }

        public double RetroRatio { get; set; }

        public double MlreRetainRatio { get; set; }

        public DateTime ReinsEffectiveStartDate { get; set; }

        public string ReinsEffectiveStartDateStr { get; set; }

        public DateTime ReinsEffectiveEndDate { get; set; }

        public string ReinsEffectiveEndDateStr { get; set; }

        public DateTime RiskQuarterStartDate { get; set; }

        public string RiskQuarterStartDateStr { get; set; }

        public DateTime RiskQuarterEndDate { get; set; }

        public string RiskQuarterEndDateStr { get; set; }

        public DateTime RuleEffectiveDate { get; set; }

        public string RuleEffectiveDateStr { get; set; }

        public DateTime RuleCeaseDate { get; set; }

        public string RuleCeaseDateStr { get; set; }

        public double RuleValue { get; set; }

        public string Description { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public const int ColumnId = 1;
        public const int ColumnTreatyCode = 2;
        public const int ColumnRetroRatio = 3;
        public const int ColumnMlreRetainRatio = 4;
        public const int ColumnRuleValue = 5;
        public const int ColumnReinsEffectiveStartDate = 6;
        public const int ColumnReinsEffectiveEndDate = 7;
        public const int ColumnRiskQuarterStartDate = 8;
        public const int ColumnRiskQuarterEndDate = 9;
        public const int ColumnRuleEffectiveDate = 10;
        public const int ColumnRuleCeaseDate = 11;
        public const int ColumnDescription = 12;
        public const int ColumnAction = 13;

        public static List<Column> GetColumns()
        {
            return new List<Column>
            {
                new Column
                {
                    Header = "ID",
                    ColIndex = ColumnId,
                    Property = "Id",
                },
                new Column
                {
                    Header = "Treaty Code",
                    ColIndex = ColumnTreatyCode,
                    Property = "TreatyCode",
                },
                new Column
                {
                    Header = "Retro Ratio",
                    ColIndex = ColumnRetroRatio,
                    Property = "RetroRatio",
                },
                new Column
                {
                    Header = "MLRe Retain Ratio",
                    ColIndex = ColumnMlreRetainRatio,
                    Property = "MlreRetainRatio",
                },
                new Column
                {
                    Header = "Rule Value",
                    ColIndex = ColumnRuleValue,
                    Property = "RuleValue",
                },
                new Column
                {
                    Header = "Reinsurance Effective Start Date",
                    ColIndex = ColumnReinsEffectiveStartDate,
                    Property = "ReinsEffectiveStartDate",
                },
                new Column
                {
                    Header = "Reinsurance Effective End Date",
                    ColIndex = ColumnReinsEffectiveEndDate,
                    Property = "ReinsEffectiveEndDate",
                },
                new Column
                {
                    Header = "Risk Quarter Start Date",
                    ColIndex = ColumnRiskQuarterStartDate,
                    Property = "RiskQuarterStartDate",
                },
                new Column
                {
                    Header = "Risk Quarter End Date",
                    ColIndex = ColumnRiskQuarterEndDate,
                    Property = "RiskQuarterEndDate",
                },
                new Column
                {
                    Header = "Rule Effective Date",
                    ColIndex = ColumnRuleEffectiveDate,
                    Property = "RuleEffectiveDate",
                },
                new Column
                {
                    Header = "Rule Cease Date",
                    ColIndex = ColumnRuleCeaseDate,
                    Property = "RuleCeaseDate",
                },
                new Column
                {
                    Header = "Description",
                    ColIndex = ColumnDescription,
                    Property = "Description",
                },
                new Column
                {
                    Header = "Action",
                    ColIndex = ColumnAction,
                },
            };
        }
    }
}
