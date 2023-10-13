using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject.Retrocession
{
    public class RetroBenefitRetentionLimitBo
    {
        public int Id { get; set; }

        public int RetroBenefitCodeId { get; set; }

        public RetroBenefitCodeBo RetroBenefitCodeBo { get; set; }

        public string RetroBenefitCode { get; set; }

        public int Type { get; set; }

        public string Description { get; set; }

        public DateTime EffectiveStartDate { get; set; }

        public DateTime EffectiveEndDate { get; set; }

        public double MinRetentionLimit { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        // Detail

        public DateTime? ReinsEffStartDate { get; set; }

        public DateTime? ReinsEffEndDate { get; set; }

        public int? MinIssueAge { get; set; }

        public int? MaxIssueAge { get; set; }

        public double? MortalityLimitFrom { get; set; }

        public double? MortalityLimitTo { get; set; }

        public double? MlreRetentionAmount { get; set; }

        public double? MinReinsAmount { get; set; }

        public const int TypeIndividual = 1;
        public const int TypeGroup = 2;

        public const int TypeMax = 2;

        public static string GetTypeName(int key)
        {
            switch (key)
            {
                case TypeIndividual:
                    return "Individual";
                case TypeGroup:
                    return "Group";
                default:
                    return "";
            }
        }

        public static List<Column> GetColumns(bool isWithDetail = false)
        {
            var cols = new List<Column>
            {
                new Column
                {
                    Header = "ID",
                    Property = "Id",
                },
                new Column
                {
                    Header = "Retro Benefit Code",
                    Property = "RetroBenefitCode",
                },
                new Column
                {
                    Header = "Description",
                    Property = "Description",
                },
                new Column
                {
                    Header = "Effective Start Date",
                    Property = "EffectiveStartDate",
                },
                new Column
                {
                    Header = "Effective End Date",
                    Property = "EffectiveEndDate",
                },
                new Column
                {
                    Header = "Minimum Retention Limit",
                    Property = "MinRetentionLimit",
                },
            };

            if (isWithDetail)
            {
                var detailCols = new List<Column>
                {
                    new Column
                    {
                        Header = "Reinsurance Effective Start Date",
                        Property = "ReinsEffStartDate",
                    },
                    new Column
                    {
                        Header = "Reinsurance Effective End Date",
                        Property = "ReinsEffEndDate",
                    },
                    new Column
                    {
                        Header = "Min Issue Age",
                        Property = "MinIssueAge",
                    },
                    new Column
                    {
                        Header = "Max Issue Age",
                        Property = "MaxIssueAge",
                    },
                    new Column
                    {
                        Header = "Mortality Limit (STD %)",
                        Property = "MortalityLimitFrom",
                    },
                    new Column
                    {
                        Header = "Mortality Limit (TM %)",
                        Property = "MortalityLimitTo",
                    },
                    new Column
                    {
                        Header = "MLRe Retention Amount",
                        Property = "MlreRetentionAmount",
                    },
                    new Column
                    {
                        Header = "Minimum Reinsurance Amount",
                        Property = "MinReinsAmount",
                    }
                };

                cols.AddRange(detailCols);
            }

            return cols;
        }
    }
}
