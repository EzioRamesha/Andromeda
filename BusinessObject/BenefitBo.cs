using System;
using Shared.ProcessFile;
using System.Collections.Generic;

namespace BusinessObject
{
    public class BenefitBo
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public DateTime? EffectiveStartDate { get; set; }

        public DateTime? EffectiveEndDate { get; set; }

        public int Status { get; set; }

        public int? ValuationBenefitCodePickListDetailId { get; set; }

        public PickListDetailBo ValuationBenefitCodePickListDetailBo { get; set; }

        public int? BenefitCategoryPickListDetailId { get; set; }

        public PickListDetailBo BenefitCategoryPickListDetailBo { get; set; }

        public bool GST { get; set; } = false;

        public string StatusStr { get; set; }

        public string ValuationBenefitCode { get; set; }

        public string BenefitCategoryCode { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        // For Drop Down
        public int? Value { get; set; }
        public string Text { get; set; }

        public const int StatusActive = 1;
        public const int StatusInactive = 2;
        public const int MaxStatus = 2;

        public const string StatusActiveName = "Active";
        public const string StatusInactiveName = "Inactive";

        public const string CodeDTH = "DTH";
        public const string CodeTPA = "TPA";
        public const string CodeTPS = "TPS";
        public const string CodeTPD = "TPD";
        public const string CodePPD = "PPD";
        public const string CodeCCA = "CCA";
        public const string CodeCCS = "CCS";
        public const string CodePA = "PA";
        public const string CodeHS = "H&S";
        public const string CodeHTH = "HTH";
        public const string CodeHCB = "HCB";
        public const string CodeCI = "CI";

        public const int ColumnId = 1;
        public const int ColumnCode = 2;
        public const int ColumnBenefitType = 3;
        public const int ColumnEffectiveStartDate = 4;
        public const int ColumnEffectiveEndDate = 5;
        public const int ColumnValuationBenefitCode = 6;
        public const int ColumnBenefitCategoryCode = 7;
        public const int ColumnGST = 8;
        public const int ColumnDescription = 9;
        public const int ColumnStatus = 10;
        public const int ColumnMLReEventCode = 11;
        public const int ColumnClaimCode = 12;
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
                    Header = "MLRe Benefit Code",
                    ColIndex = ColumnCode,
                    Property = "Code",
                },
                new Column
                {
                    Header = "Benefit Type",
                    ColIndex = ColumnBenefitType,
                    Property = "Type",
                },
                new Column
                {
                    Header = "Effective Start Date",
                    ColIndex = ColumnEffectiveStartDate,
                    Property = "EffectiveStartDate",
                },
                new Column
                {
                    Header = "Effective End Date",
                    ColIndex = ColumnEffectiveEndDate,
                    Property = "EffectiveEndDate",
                },
                new Column
                {
                    Header = "Valuation Benefit Code",
                    ColIndex = ColumnValuationBenefitCode,
                    Property = "ValuationBenefitCodePickListDetailId",
                },
                new Column
                {
                    Header = "Benefit Category",
                    ColIndex = ColumnBenefitCategoryCode,
                    Property = "BenefitCategoryPickListDetailId",
                },
                new Column
                {
                    Header = "GST",
                    ColIndex = ColumnGST,
                    Property = "GST",
                },
                new Column
                {
                    Header = "Description",
                    ColIndex = ColumnDescription,
                    Property = "Description",
                },
                new Column
                {
                    Header = "Status",
                    ColIndex = ColumnStatus,
                    Property = "Status",
                },
                new Column
                {
                    Header = "MLRe Event Code",
                    ColIndex = ColumnMLReEventCode,
                    Property = "MLReEventCode",
                },
                new Column
                {
                    Header = "Claim Code",
                    ColIndex = ColumnClaimCode,
                    Property = "ClaimCode",
                },
                new Column
                {
                    Header = "Action",
                    ColIndex = ColumnAction,
                },
            };
        }

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusActive:
                    return StatusActiveName;
                case StatusInactive:
                    return StatusInactiveName;
                default:
                    return "";
            }
        }

        public static int GetStatusKey(string name)
        {
            switch (name)
            {
                case StatusActiveName:
                    return StatusActive;
                case StatusInactiveName:
                    return StatusInactive;
                default:
                    return 0;
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

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Description))
            {
                return Code;
            }
            return string.Format("{0} - {1}", Code, Description);
        }
    }
}
