using Shared.ProcessFile;
using System.Collections.Generic;

namespace BusinessObject
{
    public class AccountCodeMappingBo
    {
        public int Id { get; set; }

        public int ReportType { get; set; }

        public string ReportTypeName { get; set; }

        public int? Type { get; set; }

        public string TypeName { get; set; }

        public string TreatyType { get; set; }

        public string TreatyNumber { get; set; }

        public int? TreatyCodeId { get; set; }

        public TreatyCodeBo TreatyCodeBo { get; set; }

        public string TreatyCode { get; set; }        

        public string ClaimCode { get; set; }

        public string BusinessOrigin { get; set; }

        public int AccountCodeId { get; set; }

        public AccountCodeBo AccountCodeBo { get; set; }

        public string AccountCode { get; set; }

        public int? DebitCreditIndicatorPositive { get; set; }
        public string DebitCreditIndicatorPositiveStr { get; set; }

        public int? DebitCreditIndicatorNegative { get; set; }
        public string DebitCreditIndicatorNegativeStr { get; set; }

        public int? TransactionTypeCodePickListDetailId { get; set; }

        public PickListDetailBo TransactionTypeCodePickListDetailBo { get; set; }

        public string TransactionTypeCode { get; set; }

        public int? RetroRegisterFieldPickListDetailId { get; set; }

        public PickListDetailBo RetroRegisterFieldPickListDetailBo { get; set; }

        public string RetroRegisterField { get; set; }

        public int? ModifiedContractCodeId { get; set; }

        public Mfrs17ContractCodeBo ModifiedContractCodeBo { get; set; }

        public string ModifiedContractCode { get; set; }

        public string InvoiceField { get; set; }

        public bool IsBalanceSheet { get; set; }
        public string IsBalanceSheetStr { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public const int ReportTypeIfrs4 = 1;
        public const int ReportTypeIfrs17 = 2;

        public const string ReportTypeIfrs4Name = "IFRS4";
        public const string ReportTypeIfrs17Name = "IFRS17";

        public const int TypeClaimProvision = 1;
        public const int TypeClaimRecovery = 2;
        public const int TypeDirectRetro = 3;
        public const int TypePerLifeRetro = 4;
        public const int TypeCedantAccountCode = 5;

        public const string TypeClaimProvisionName = "Claim Provision";
        public const string TypeClaimRecoveryName = "Claim Recovery";
        public const string TypeDirectRetroName = "Direct Retro";
        public const string TypePerLifeRetroName = "Per Life Retro";
        public const string TypeCedantAccountCodeName = "Cedant Account Code";

        public const int DebitCreditIndicatorC = 1;
        public const int DebitCreditIndicatorD = 2;

        public const string DebitCreditIndicatorCName = "C";
        public const string DebitCreditIndicatorDName = "D";

        public static string GetReportTypeName(int? key)
        {
            switch (key)
            {
                case ReportTypeIfrs4:
                    return ReportTypeIfrs4Name;
                case ReportTypeIfrs17:
                    return ReportTypeIfrs17Name;
                default:
                    return "";
            }
        }
        public static int GetReportTypeByName(string name)
        {
            switch (name)
            {
                case ReportTypeIfrs4Name:
                    return ReportTypeIfrs4;
                case ReportTypeIfrs17Name:
                    return ReportTypeIfrs17;
                default:
                    return 0;
            }
        }

        public static string GetTypeName(int? key)
        {
            switch (key)
            {
                case TypeClaimProvision:
                    return TypeClaimProvisionName;
                case TypeClaimRecovery:
                    return TypeClaimRecoveryName;
                case TypeDirectRetro:
                    return TypeDirectRetroName;
                case TypePerLifeRetro:
                    return TypePerLifeRetroName;
                case TypeCedantAccountCode:
                    return TypeCedantAccountCodeName;
                default:
                    return "";
            }
        }

        public static int GetTypeByName(string name)
        {
            switch (name)
            {
                case TypeClaimProvisionName:
                    return TypeClaimProvision;
                case TypeClaimRecoveryName:
                    return TypeClaimRecovery;
                case TypeDirectRetroName:
                    return TypeDirectRetro;
                case TypePerLifeRetroName:
                    return TypePerLifeRetro;
                case TypeCedantAccountCodeName:
                    return TypeCedantAccountCode;
                default:
                    return 0;
            }
        }

        public static string GetDebitCreditIndicatorName(int key)
        {
            switch (key)
            {
                case DebitCreditIndicatorC:
                    return DebitCreditIndicatorCName;
                case DebitCreditIndicatorD:
                    return DebitCreditIndicatorDName;
                default:
                    return "";
            }
        }

        public static int GetDebitCreditIndicatorByName(string key)
        {
            switch (key)
            {
                case DebitCreditIndicatorCName:
                    return DebitCreditIndicatorC;
                case DebitCreditIndicatorDName:
                    return DebitCreditIndicatorD;
                default:
                    return 0;
            }
        }

        public static string GetPLBSName(bool key)
        {
            return key ? "BS" : "P&L";
        }

        public static List<int> GetDaaTypes()
        {
            return new List<int> { TypeClaimProvision, TypeClaimRecovery, TypeCedantAccountCode };
        }

        public static List<int> GetRetroTypes()
        {
            return new List<int> { TypeDirectRetro, TypePerLifeRetro };
        }

        public const int ColumnId = 1;
        public const int ColumnReportType = 2;
        public const int ColumnType = 3;
        public const int ColumnTreatyType = 4;
        public const int ColumnTreatyCode = 5;
        public const int ColumnClaimCode = 6;
        public const int ColumnBusinessOrigin = 7;
        public const int ColumnTransactionTypeCode = 8;
        public const int ColumnModifiedContractCode = 9;
        public const int ColumnInvoiceField = 10;
        public const int ColumnAccountCode = 11;
        public const int ColumnIsBalanceSheet = 12;
        public const int ColumnDebitCreditIndicatorPositive = 13;
        public const int ColumnDebitCreditIndicatorNegative = 14;
        public const int ColumnAction = 15;

        public const int RetroColumnId = 1;
        public const int RetroColumnReportType = 2;
        public const int RetroColumnType = 3;
        public const int RetroColumnTreatyType = 4;
        public const int RetroColumnTreatyNo = 5;
        public const int RetroColumnTreatyCode = 6;
        public const int RetroColumnClaimCode = 7;
        public const int RetroColumnBusinessOrigin = 8;
        public const int RetroColumnTransactionTypeCode = 9;
        public const int RetroColumnRetroRegisterField = 10;
        public const int RetroColumnModifiedContractCode = 11;
        public const int RetroColumnAccountCode = 12;
        public const int RetroColumnIsBalanceSheet = 13;
        public const int RetroColumnDebitCreditIndicatorPositive = 14;
        public const int RetroColumnDebitCreditIndicatorNegative = 15;
        public const int RetroColumnAction = 16;

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
                    Header = "Report Type",
                    ColIndex = ColumnReportType,
                    Property = "ReportTypeName",
                },
                new Column
                {
                    Header = "Type",
                    ColIndex = ColumnType,
                    Property = "TypeName",
                },
                new Column
                {
                    Header = "Treaty Type",
                    ColIndex = ColumnTreatyType,
                    Property = "TreatyType",
                },
                new Column
                {
                    Header = "Treaty Code",
                    ColIndex = ColumnTreatyCode,
                    Property = "TreatyCode",
                },
                new Column
                {
                    Header = "Claim Code",
                    ColIndex = ColumnClaimCode,
                    Property = "ClaimCode",
                },
                new Column
                {
                    Header = "Business Origin",
                    ColIndex = ColumnBusinessOrigin,
                    Property = "BusinessOrigin",
                },
                new Column
                {
                    Header = "Transaction Type Code",
                    ColIndex = ColumnTransactionTypeCode,
                    Property = "TransactionTypeCode",
                },
                new Column
                {
                    Header = "Modified Contract Code",
                    ColIndex = ColumnModifiedContractCode,
                    Property = "ModifiedContractCode",
                },
                new Column
                {
                    Header = "Invoice Field",
                    ColIndex = ColumnInvoiceField,
                    Property = "InvoiceField",
                },
                new Column
                {
                    Header = "Account Code",
                    ColIndex = ColumnAccountCode,
                    Property = "AccountCode",
                },
                new Column
                {
                    Header = "Grouping Indicator",
                    ColIndex = ColumnIsBalanceSheet,
                    Property = "IsBalanceSheetStr"
                },
                new Column
                {
                    Header = "Debit/Credit Indicator - Positive",
                    ColIndex = ColumnDebitCreditIndicatorPositive,
                    Property = "DebitCreditIndicatorPositive",
                },
                new Column
                {
                    Header = "Debit/Credit Indicator - Negative",
                    ColIndex = ColumnDebitCreditIndicatorNegative,
                    Property = "DebitCreditIndicatorNegative",
                },
                new Column
                {
                    Header = "Action",
                    ColIndex = ColumnAction,
                },
            };
        }

        public static List<Column> GetRetroColumns()
        {
            return new List<Column>
            {
                new Column
                {
                    Header = "ID",
                    ColIndex = RetroColumnId,
                    Property = "Id",
                },
                new Column
                {
                    Header = "Report Type",
                    ColIndex = RetroColumnReportType,
                    Property = "ReportTypeName",
                },
                new Column
                {
                    Header = "Type",
                    ColIndex = RetroColumnType,
                    Property = "TypeName",
                },
                new Column
                {
                    Header = "Treaty Type",
                    ColIndex = RetroColumnTreatyType,
                    Property = "TreatyType",
                },
                new Column
                {
                    Header = "Treaty Number",
                    ColIndex = RetroColumnTreatyNo,
                    Property = "TreatyNumber",
                },
                new Column
                {
                    Header = "Treaty Code",
                    ColIndex = RetroColumnTreatyCode,
                    Property = "TreatyCode",
                },
                new Column
                {
                    Header = "Claim Code",
                    ColIndex = RetroColumnClaimCode,
                    Property = "ClaimCode",
                },
                new Column
                {
                    Header = "Business Origin",
                    ColIndex = RetroColumnBusinessOrigin,
                    Property = "BusinessOrigin",
                },
                new Column
                {
                    Header = "Transaction Type Code",
                    ColIndex = RetroColumnTransactionTypeCode,
                    Property = "TransactionTypeCode",
                },
                new Column
                {
                    Header = "Retro Register Field",
                    ColIndex = RetroColumnRetroRegisterField,
                    Property = "RetroRegisterField",
                },
                new Column
                {
                    Header = "Modified Contract Code",
                    ColIndex = RetroColumnModifiedContractCode,
                    Property = "ModifiedContractCode",
                },
                new Column
                {
                    Header = "Account Code",
                    ColIndex = RetroColumnAccountCode,
                    Property = "AccountCode",
                },
                new Column
                {
                    Header = "Grouping Indicator",
                    ColIndex = RetroColumnIsBalanceSheet,
                    Property = "IsBalanceSheetStr"
                },
                new Column
                {
                    Header = "Debit/Credit Indicator - Positive",
                    ColIndex = RetroColumnDebitCreditIndicatorPositive,
                    Property = "DebitCreditIndicatorPositive",
                },
                new Column
                {
                    Header = "Debit/Credit Indicator - Negative",
                    ColIndex = RetroColumnDebitCreditIndicatorNegative,
                    Property = "DebitCreditIndicatorNegative",
                },
                new Column
                {
                    Header = "Action",
                    ColIndex = RetroColumnAction,
                },
            };
        }
    }
}
