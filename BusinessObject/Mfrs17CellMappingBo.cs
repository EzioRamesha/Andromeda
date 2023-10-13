using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject
{
    public class Mfrs17CellMappingBo
    {
        public int Id { get; set; }

        public string TreatyCode { get; set; }

        public int ReinsBasisCodePickListDetailId { get; set; }

        public virtual PickListDetailBo ReinsBasisCodePickListDetailBo { get; set; }

        public string ReinsBasisCode { get; set; }

        public DateTime? ReinsEffDatePolStartDate { get; set; }

        public DateTime? ReinsEffDatePolEndDate { get; set; }

        public string CedingPlanCode { get; set; }

        public string BenefitCode { get; set; }

        public int BasicRiderPickListDetailId { get; set; }

        public virtual PickListDetailBo BasicRiderPickListDetailBo { get; set; }

        public string BasicRider { get; set; }

        public string CellName { get; set; }

        //public string Mfrs17TreatyCode { get; set; }

        public int? Mfrs17ContractCodeDetailId { get; set; }

        public Mfrs17ContractCodeDetailBo Mfrs17ContractCodeDetailBo { get; set; }

        public string Mfrs17ContractCode { get; set; }

        public string LoaCode { get; set; }

        public string ProfitComm { get; set; }

        public int? ProfitCommPickListDetailId { get; set; }

        public PickListDetailBo ProfitCommPickListDetailBo { get; set; }

        public string RateTable { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public const int ColumnId = 1;
        public const int ColumnTreatyCode = 2;
        public const int ColumnReinsEffDatePolStartDate = 3;
        public const int ColumnReinsEffDatePolEndDate = 4;
        public const int ColumnReinsBasisCode = 5;
        public const int ColumnCedingPlanCode = 6;
        public const int ColumnBenefitCode = 7;
        public const int ColumnProfitComm = 8;
        public const int ColumnRateTable = 9;
        public const int ColumnBasicRider = 10;
        public const int ColumnCellName = 11;
        public const int ColumnMfrs17ContractCode = 12;
        public const int ColumnLoaCode = 13;
        public const int ColumnAction = 14;

        public Mfrs17CellMappingBo()
        {
            CreatedAt = DateTime.Now;
        }

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
                    Header = "Policy Reinsurance Effective Start Date",
                    ColIndex = ColumnReinsEffDatePolStartDate,
                    Property = "ReinsEffDatePolStartDate",
                },
                new Column
                {
                    Header = "Policy Reinsurance Effective End Date",
                    ColIndex = ColumnReinsEffDatePolEndDate,
                    Property = "ReinsEffDatePolEndDate",
                },
                new Column
                {
                    Header = "Reinsurance Basis Code",
                    ColIndex = ColumnReinsBasisCode,
                    Property = "ReinsBasisCode",
                },
                new Column
                {
                    Header = "Ceding Plan Code",
                    ColIndex = ColumnCedingPlanCode,
                    Property = "CedingPlanCode",
                },
                new Column
                {
                    Header = "MLRe Benefit Code",
                    ColIndex = ColumnBenefitCode,
                    Property = "BenefitCode",
                },
                new Column
                {
                    Header = "Profit Commission",
                    ColIndex = ColumnProfitComm,
                    Property = "ProfitComm",
                },
                new Column
                {
                    Header = "Rate Table",
                    ColIndex = ColumnRateTable,
                    Property = "RateTable",
                },
                new Column
                {
                    Header = "MFRS17 Basic Rider",
                    ColIndex = ColumnBasicRider,
                    Property = "BasicRider",
                },
                new Column
                {
                    Header = "MFRS17 Cell Name",
                    ColIndex = ColumnCellName,
                    Property = "CellName",
                },
                new Column
                {
                    Header = "Mfrs17 Contract Code",
                    ColIndex = ColumnMfrs17ContractCode,
                    Property = "Mfrs17ContractCode",
                }, 
                new Column
                {
                    Header = "LOA Code",
                    ColIndex = ColumnLoaCode,
                    Property = "LoaCode",
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
