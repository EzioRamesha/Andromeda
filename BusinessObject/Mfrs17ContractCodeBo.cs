using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject
{
    public class Mfrs17ContractCodeBo
    {
        public int Id { get; set; }

        public int CedingCompanyId { get; set; }
        public CedantBo CedingCompanyBo { get; set; }
        public string CedantCode { get; set; }

        public string ModifiedContractCode { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        // Detail
        public int? Mfrs17ContractCodeDetailId { get; set; }

        public string Mfrs17ContractCode { get; set; }

        public const int ColumnId = 1;
        public const int ColumnCedant = 2;
        public const int ColumnModifiedContractCode = 3;
        public const int ColumnAction = 4;
        public const int ColumnContractCodeId = 5;
        public const int ColumnContractCode = 6;
        public const int ColumnContractCodeAction = 7;

        public static List<Column> GetColumns()
        {
            return new List<Column>
            {
                new Column
                {
                    Header = "Modified Contract Code ID",
                    ColIndex = ColumnId,
                    Property = "Id",
                },
                new Column
                {
                    Header = "Cedant Code",
                    ColIndex = ColumnCedant,
                    Property = "CedantCode",
                },
                new Column
                {
                    Header = "Modified Contract Code",
                    ColIndex = ColumnModifiedContractCode,
                    Property = "ModifiedContractCode",
                },
                new Column
                {
                    Header = "Action (Modified Contract Code)",
                    ColIndex = ColumnAction,
                },
                new Column
                {
                    Header = "MFRS17 Contract Code ID",
                    ColIndex = ColumnContractCodeId,
                    Property = "Mfrs17ContractCodeDetailId",
                },
                new Column
                {
                    Header = "MFRS17 Contract Code",
                    ColIndex = ColumnContractCode,
                    Property = "Mfrs17ContractCode",
                },
                new Column
                {
                    Header = "Action (MFRS17 Contract Code)",
                    ColIndex = ColumnContractCodeAction,
                },
            };
        }
    }
}
