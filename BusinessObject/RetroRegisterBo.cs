using BusinessObject.Identity;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject
{
    public class RetroRegisterBo
    {
        public int Id { get; set; }

        public int Type { get; set; }

        public int? RetroRegisterBatchId { get; set; }

        public virtual RetroRegisterBatchBo RetroRegisterBatchBo { get; set; }

        public int RetroStatementType { get; set; }

        public string RetroStatementNo { get; set; }

        public DateTime? RetroStatementDate { get; set; }

        public DateTime? ReportCompletedDate { get; set; }

        public DateTime? SendToRetroDate { get; set; }

        public int? RetroPartyId { get; set; }

        public virtual RetroPartyBo RetroPartyBo { get; set; }

        public string RiskQuarter { get; set; }

        public int? CedantId { get; set; }

        public virtual CedantBo CedantBo { get; set; }

        public int? TreatyCodeId { get; set; }

        public virtual TreatyCodeBo TreatyCodeBo { get; set; }

        public string TreatyNumber { get; set; }

        public string Schedule { get; set; }

        public string TreatyType { get; set; }

        public string LOB { get; set; }

        public string AccountFor { get; set; }

        public double? Year1st { get; set; } = 0;

        public double? Renewal { get; set; } = 0;

        public double? ReserveCededBegin { get; set; } = 0;

        public double? ReserveCededEnd { get; set; } = 0;

        public double? RiskChargeCededBegin { get; set; } = 0;

        public double? RiskChargeCededEnd { get; set; } = 0;

        public double? AverageReserveCeded { get; set; } = 0;

        public double? Gross1st { get; set; } = 0;

        public double? GrossRen { get; set; } = 0;

        public double? AltPremium { get; set; } = 0;

        public double? Discount1st { get; set; } = 0;

        public double? DiscountRen { get; set; } = 0;

        public double? DiscountAlt { get; set; } = 0;

        public double? RiskPremium { get; set; } = 0;

        public double? Claims { get; set; } = 0;

        public double? ProfitCommission { get; set; } = 0;

        public double? SurrenderVal { get; set; } = 0;

        public double? NoClaimBonus { get; set; } = 0;

        public double? RetrocessionMarketingFee { get; set; } = 0;

        public double? AgreedDBCommission { get; set; } = 0;

        public double? GstPayable { get; set; } = 0;

        public double? NetTotalAmount { get; set; } = 0;

        public int? NbCession { get; set; } = 0;

        public double? NbSumReins { get; set; } = 0;

        public int? RnCession { get; set; } = 0;

        public double? RnSumReins { get; set; } = 0;

        public int? AltCession { get; set; } = 0;

        public double? AltSumReins { get; set; } = 0;

        public string Frequency { get; set; }

        public int? PreparedById { get; set; }

        public virtual UserBo PreparedByBo { get; set; }

        public string OriginalSoaQuarter { get; set; }

        public DateTime? RetroConfirmationDate { get; set; }

        public double? ValuationGross1st { get; set; } = 0;

        public double? ValuationGrossRen { get; set; } = 0;

        public double? ValuationDiscount1st { get; set; } = 0;

        public double? ValuationDiscountRen { get; set; } = 0;

        public double? ValuationCom1st { get; set; } = 0;

        public double? ValuationComRen { get; set; } = 0;

        public string Remark { get; set; }

        public int? Status { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public int? DirectRetroId { get; set; }
        public virtual DirectRetroBo DirectRetroBo { get; set; }

        public string ContractCode { get; set; }
        public int? AnnualCohort { get; set; }

        public int ReportingType { get; set; }


        public string RetroName { get; set; }
        public string PartyCode { get; set; }
        public int? RetroStatus { get; set; }

        public const int RetroTypeWM = 1;
        public const int RetroTypeOM = 2;
        public const int RetroTypeSF = 3;
        public const int RetroTypeMax = 3;

        public const int TypeDirectRetro = 1;
        public const int TypePerLifeRetro = 2;
        public const int TypeMax = 2;

        public const int StatusPendingApproval = 1;
        public const int StatusApproved = 2;
        public const int StatusRejected = 3;
        public const int StatusMax = 3;

        public const int ReportingTypeIFRS17 = 1;
        public const int ReportingTypeIFRS4 = 2;
        public const int ReportingTypeMax = 2;

        public const int ColumnId = 1;        
        public const int ColumnInvoiceNo = 3;
        public const int ColumnConfirmationDate = 54;

        public static string GetRetroTypeName(int? key)
        {
            switch (key)
            {
                case RetroTypeWM:
                    return "WM";
                case RetroTypeOM:
                    return "OM";
                case RetroTypeSF:
                    return "SF";
                default:
                    return "";
            }
        }

        public static string GetTypeName(int key)
        {
            switch (key)
            {
                case TypeDirectRetro:
                    return "Direct Retro";
                case TypePerLifeRetro:
                    return "Per Life Retro";
                default:
                    return "";
            }
        }

        public static string GetStatusApprovalName(int? key)
        {
            switch (key)
            {
                case StatusPendingApproval:
                    return "Pending Approval";
                case StatusApproved:
                    return "Approved";
                case StatusRejected:
                    return "Rejected";
                default:
                    return "";
            }
        }

        public static string GetStatusApprovalClass(int? key)
        {
            switch (key)
            {
                case StatusPendingApproval:
                    return "status-pending-badge";
                case StatusApproved:
                    return "status-success-badge";
                case StatusRejected:
                    return "status-fail-badge";
                default:
                    return "";
            }
        }

        public static int GetTypeByCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return 0;

            var codes = new List<string> { "WM", "OM", "SF" };
            return codes.FindIndex(q => q == code) + 1;
        }

        public static string GetReportingTypeName(int key)
        {
            switch (key)
            {
                case ReportingTypeIFRS17:
                    return "IFRS17";
                case ReportingTypeIFRS4:
                    return "IFRS4";
                default:
                    return "";
            }
        }

        public double? GetYear1st()
        {
            Year1st = Gross1st.GetValueOrDefault();
            return Year1st;
        }

        public double? GetRenewal()
        {
            Renewal = GrossRen.GetValueOrDefault() + AltPremium.GetValueOrDefault() + RiskPremium.GetValueOrDefault();
            if (Equals(double.NaN, Renewal)) Renewal = 0;

            return Renewal = Util.RoundNullableValue(Renewal, 2);
        }

        public double? GetNetTotalAmount()
        {
            NetTotalAmount = (Gross1st.GetValueOrDefault() + GrossRen.GetValueOrDefault() + AltPremium.GetValueOrDefault() 
                - Discount1st.GetValueOrDefault() - DiscountRen.GetValueOrDefault() - DiscountAlt.GetValueOrDefault() 
                + RiskPremium.GetValueOrDefault() - Claims.GetValueOrDefault() - ProfitCommission.GetValueOrDefault() 
                - SurrenderVal.GetValueOrDefault() - NoClaimBonus.GetValueOrDefault() + RetrocessionMarketingFee.GetValueOrDefault() 
                - AgreedDBCommission.GetValueOrDefault() + GstPayable.GetValueOrDefault());
            if (Equals(double.NaN, NetTotalAmount)) NetTotalAmount = 0;

            return NetTotalAmount = Util.RoundNullableValue(NetTotalAmount, 2);
        }

        public double? GetValuationGross1st()
        {
            ValuationGross1st = Gross1st.GetValueOrDefault();
            return ValuationGross1st;
        }

        public double? GetValuationGrossRen()
        {
            ValuationGrossRen = GrossRen.GetValueOrDefault() + AltPremium.GetValueOrDefault();
            if (Equals(double.NaN, ValuationGrossRen)) ValuationGrossRen = 0;

            return ValuationGrossRen = Util.RoundNullableValue(ValuationGrossRen, 2);
        }

        public double? GetValuationDiscount1st()
        {
            ValuationDiscount1st = Discount1st.GetValueOrDefault();
            return ValuationDiscount1st;
        }

        public double? GetValuationDiscountRen()
        {
            ValuationDiscountRen = DiscountRen.GetValueOrDefault() + DiscountAlt.GetValueOrDefault();
            if (Equals(double.NaN, ValuationDiscountRen)) ValuationDiscountRen = 0;

            return ValuationDiscountRen = Util.RoundNullableValue(ValuationDiscountRen, 2);
        }

        public double? GetValuationCom1st()
        {
            ValuationCom1st = ((ValuationGross1st.GetValueOrDefault() / (ValuationGross1st.GetValueOrDefault() + ValuationGrossRen.GetValueOrDefault())) 
                * (RetrocessionMarketingFee.GetValueOrDefault() + AgreedDBCommission.GetValueOrDefault()));
            if (Equals(double.NaN, ValuationCom1st)) ValuationCom1st = 0;

            return ValuationCom1st = Util.RoundNullableValue(ValuationCom1st, 2);
        }

        public double? GetValuationComRen()
        {
            ValuationComRen = ((ValuationGrossRen.GetValueOrDefault() / (ValuationGross1st.GetValueOrDefault() + ValuationGrossRen.GetValueOrDefault()))
                * (RetrocessionMarketingFee.GetValueOrDefault() + AgreedDBCommission.GetValueOrDefault()));
            if (Equals(double.NaN, ValuationComRen)) ValuationComRen = 0;

            return ValuationComRen = Util.RoundNullableValue(ValuationComRen, 2);
        }

        public static List<string> GetIncomeTypeItems()
        {
            return new List<string>
            {
                "Gross1st",
                "GrossRen",
                "AltPremium",
                "RiskPremium",
                "RetrocessionMarketingFee",
                "GstPayable",
            };
        }

        public static List<string> GetOutgoTypeItems()
        {
            return new List<string>
            {
                "Discount1st",
                "DiscountRen",
                "DiscountAlt",
                "Claims",
                "ProfitCommission",
                "SurrenderVal",
                "NoClaimBonus",
                "AgreedDBCommission",
            };
        }

        public static List<string> GetBalanceSheetPremiumItems()
        {
            return new List<string>
            {
                "Gross1st",
                "GrossRen",
                "AltPremium",
                "Discount1st",
                "DiscountRen",
                "DiscountAlt",
                "RiskPremium",
            };
        }

        public static List<string> GetBalanceSheetCreditorItems()
        {
            return new List<string>
            {
                "Claims",
                "RetrocessionMarketingFee",
                "GstPayable",
                "ProfitCommission",
                "SurrenderVal",
                "NoClaimBonus",
                "AgreedDBCommission",
            };
        }

        public static string GetFormatValueByCode(string key)
        {
            if (GetIncomeTypeItems().Contains(key))
                return "+";

            if (GetOutgoTypeItems().Contains(key))
                return "-";

            return "";
        }

        public static List<Column> GetColumns()
        {
            var columns = new List<Column>
            {
                new Column
                {
                    Header = "ID",
                    Property = "Id",
                    ColIndex = ColumnId,
                },
                new Column
                {
                    Header = "Invoice Type",
                    Property = "RetroStatementType",
                    ColIndex = ColumnInvoiceNo,
                },
                new Column
                {
                    Header = "Invoice No",
                    Property = "RetroStatementNo",
                },
                new Column
                {
                    Header = "Invoice Date",
                    Property = "RetroStatementDate",
                },
                new Column
                {
                    Header = "Date Report Completed",
                    Property = "ReportCompletedDate",
                },
                new Column
                {
                    Header = "Date Send to Retro",
                    Property = "SendToRetroDate",
                },
                new Column
                {
                    Header = "Ceding Company",
                    Property = "CedantId",
                },
                new Column
                {
                    Header = "Retro Party",
                    Property = "RetroPartyId",
                },
                new Column
                {
                    Header = "Retro Name",
                    Property = "RetroName",
                },
                new Column
                {
                    Header = "Party Code",
                    Property = "PartyCode",
                },
                new Column
                {
                    Header = "Treaty Code",
                    Property = "TreatyCodeId",
                },
                new Column
                {
                    Header = "Treaty No",
                    Property = "TreatyNumber",
                },
                new Column
                {
                    Header = "Treaty Type",
                    Property = "TreatyType",
                },
                new Column
                {
                    Header = "Schedule",
                    Property = "Schedule",
                },
                new Column
                {
                    Header = "Lob",
                    Property = "LOB",
                },
                new Column
                {
                    Header = "Risk Quarter",
                    Property = "RiskQuarter",
                },
                new Column
                {
                    Header = "Accounts for",
                    Property = "AccountFor",
                },
                new Column
                {
                    Header = "Net Total Amount",
                    Property = "NetTotalAmount",
                },
                new Column
                {
                    Header = "1st Year",
                    Property = "Year1st",
                },
                new Column
                {
                    Header = "Renewal",
                    Property = "Renewal",
                },
                new Column
                {
                    Header = "Reserve Ceded Beginning",
                    Property = "ReserveCededBegin",
                },
                new Column
                {
                    Header = "Reserve Ceded End",
                    Property = "ReserveCededEnd",
                },
                new Column
                {
                    Header = "Risk Charge Ceded Beginning",
                    Property = "RiskChargeCededBegin",
                },
                new Column
                {
                    Header = "Risk Charge Ceded End",
                    Property = "RiskChargeCededEnd",
                },
                new Column
                {
                    Header = "Average Reserve Ceded",
                    Property = "AverageReserveCeded",
                },
                new Column
                {
                    Header = "Gross - 1st",
                    Property = "Gross1st",
                },
                new Column
                {
                    Header = "Gross - Renewal",
                    Property = "GrossRen",
                },
                new Column
                {
                    Header = "Alt - Premium",
                    Property = "AltPremium",
                },
                new Column
                {
                    Header = "Discount - 1st",
                    Property = "Discount1st",
                },
                new Column
                {
                    Header = "Discount - Renewal",
                    Property = "DiscountRen",
                },
                new Column
                {
                    Header = "Discount - Alt",
                    Property = "DiscountAlt",
                },
                new Column
                {
                    Header = "Risk Premium",
                    Property = "RiskPremium",
                },
                new Column
                {
                    Header = "Claim",
                    Property = "Claims",
                },
                new Column
                {
                    Header = "Profit Commission",
                    Property = "ProfitCommission",
                },
                new Column
                {
                    Header = "Surrender Value",
                    Property = "SurrenderVal",
                },
                new Column
                {
                    Header = "GST",
                    Property = "GstPayable",
                },
                new Column
                {
                    Header = "No Claim Bonus",
                    Property = "NoClaimBonus",
                },
                new Column
                {
                    Header = "Retrocession Marketing Fee",
                    Property = "RetrocessionMarketingFee",
                },
                new Column
                {
                    Header = "Agreed Database Commission",
                    Property = "AgreedDBCommission",
                },
                new Column
                {
                    Header = "Nb Cession",
                    Property = "NbCession",
                },
                new Column
                {
                    Header = "Nb Sum Reins",
                    Property = "NbSumReins",
                },
                new Column
                {
                    Header = "Rn Cession",
                    Property = "RnCession",
                },
                new Column
                {
                    Header = "Rn Sum Reins",
                    Property = "RnSumReins",
                },
                new Column
                {
                    Header = "Alt Cession",
                    Property = "AltCession",
                },
                new Column
                {
                    Header = "Alt Sum Reins",
                    Property = "AltSumReins",
                },
                new Column
                {
                    Header = "Frequency",
                    Property = "Frequency",
                },
                new Column
                {
                    Header = "Valuation Gross 1st",
                    Property = "ValuationGross1st",
                },
                new Column
                {
                    Header = "Valuation Gross Ren",
                    Property = "ValuationGrossRen",
                },
                new Column
                {
                    Header = "Valuation Discount 1st",
                    Property = "ValuationDiscount1st",
                },
                new Column
                {
                    Header = "Valuation Discount Ren",
                    Property = "ValuationDiscountRen",
                },
                new Column
                {
                    Header = "Valuation Com 1st",
                    Property = "ValuationCom1st",
                },
                new Column
                {
                    Header = "Valuation Com Ren",
                    Property = "ValuationComRen",
                },
                new Column
                {
                    Header = "Original Soa Quarter",
                    Property = "OriginalSoaQuarter",
                },
                new Column
                {
                    Header = "Retro Confirmation Date",
                    Property = "RetroConfirmationDate",
                    ColIndex = ColumnConfirmationDate,
                },
                new Column
                {
                    Header = "Prepared By",
                    Property = "PreparedById",
                },
                new Column
                {
                    Header = "Remark",
                    Property = "Remark",
                },
            };

            return columns;
        }
    }
}
