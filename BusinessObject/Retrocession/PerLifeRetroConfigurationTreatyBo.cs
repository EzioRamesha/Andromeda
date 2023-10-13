using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject.Retrocession
{
    public class PerLifeRetroConfigurationTreatyBo
    {
        public int Id { get; set; }

        public int TreatyCodeId { get; set; }

        public TreatyCodeBo TreatyCodeBo { get; set; }

        public string TreatyCode { get; set; }

        public int TreatyTypePickListDetailId { get; set; }

        public PickListDetailBo TreatyTypePickListDetailBo { get; set; }

        public string TreatyType { get; set; }

        public int FundsAccountingTypePickListDetailId { get; set; }

        public PickListDetailBo FundsAccountingTypePickListDetailBo { get; set; }

        public string FundsAccountingType { get; set; }

        public DateTime ReinsEffectiveStartDate { get; set; }

        public DateTime ReinsEffectiveEndDate { get; set; }

        public DateTime RiskQuarterStartDate { get; set; }

        public DateTime RiskQuarterEndDate { get; set; }

        public bool IsToAggregate { get; set; }

        public string Remark { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public string ReinsEffectiveStartDateStr { get; set; }

        public string ReinsEffectiveEndDateStr { get; set; }

        public string RiskQuarterStartDateStr { get; set; }

        public string RiskQuarterEndDateStr { get; set; }

        public const int ColumnId = 1;
        public const int ColumnTreatyCode = 2;
        public const int ColumnTreatyType = 3;
        public const int ColumnFundsAccountingType = 4;
        public const int ColumnReinsEffectiveStartDate = 5;
        public const int ColumnReinsEffectiveEndDate = 6;
        public const int ColumnRiskQuarterStartDate = 7;
        public const int ColumnRiskQuarterEndDate = 8;
        public const int ColumnIsToAggregate = 9;
        public const int ColumnRemark = 10;
        public const int ColumnAction = 11;

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
                    Header = "Treaty Type",
                    ColIndex = ColumnTreatyType,
                    Property = "TreatyType",
                },
                new Column
                {
                    Header = "Funds Accounting Type",
                    ColIndex = ColumnFundsAccountingType,
                    Property = "FundsAccountingType",
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
                    Header = "To Aggregate",
                    ColIndex = ColumnIsToAggregate,
                    Property = "IsToAggregate",
                },
                new Column
                {
                    Header = "Remark",
                    ColIndex = ColumnRemark,
                    Property = "Remark",
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
