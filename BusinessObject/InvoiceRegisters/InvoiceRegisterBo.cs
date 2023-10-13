using BusinessObject.Identity;
using BusinessObject.SoaDatas;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject.InvoiceRegisters
{
    public class InvoiceRegisterBo
    {
        public int Id { get; set; }

        public int InvoiceRegisterBatchId { get; set; }
        public virtual InvoiceRegisterBatchBo InvoiceRegisterBatchBo { get; set; }

        public int? SoaDataBatchId { get; set; }
        public virtual SoaDataBatchBo SoaDataBatchBo { get; set; }

        public int InvoiceType { get; set; }
        public string InvoiceReference { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime? StatementReceivedDate { get; set; }

        public int? CedantId { get; set; }
        public virtual CedantBo CedantBo { get; set; }

        public string RiskQuarter { get; set; }

        public int? TreatyCodeId { get; set; }
        public virtual TreatyCodeBo TreatyCodeBo { get; set; }

        public string AccountDescription { get; set; }

        public double? TotalPaid { get; set; } = 0;

        public string PaymentReference { get; set; }
        public string PaymentAmount { get; set; }
        public DateTime? PaymentReceivedDate { get; set; }

        public double? Year1st { get; set; } = 0;
        public double? Renewal { get; set; } = 0;
        public double? Gross1st { get; set; } = 0;
        public double? GrossRenewal { get; set; } = 0;
        public double? AltPremium { get; set; } = 0;
        public double? Discount1st { get; set; } = 0;
        public double? DiscountRen { get; set; } = 0;
        public double? DiscountAlt { get; set; } = 0;

        public double? RiskPremium { get; set; } = 0;
        public double? Claim { get; set; } = 0;
        public double? ProfitComm { get; set; } = 0;
        public double? Levy { get; set; } = 0;
        public double? SurrenderValue { get; set; } = 0;
        public double? Gst { get; set; } = 0;
        public double? ModcoReserveIncome { get; set; } = 0;
        public double? ReinsDeposit { get; set; } = 0;
        public double? NoClaimBonus { get; set; } = 0;
        public double? DatabaseCommission { get; set; } = 0;
        public double? AdministrationContribution { get; set; } = 0;
        public double? ShareOfRiCommissionFromCompulsoryCession { get; set; } = 0;
        public double? RecaptureFee { get; set; } = 0;
        public double? CreditCardCharges { get; set; } = 0;
        public double? BrokerageFee { get; set; } = 0;

        public int? NBCession { get; set; } = 0;
        public double? NBSumReins { get; set; } = 0;
        public int? RNCession { get; set; } = 0;
        public double? RNSumReins { get; set; } = 0;
        public int? AltCession { get; set; } = 0;
        public double? AltSumReins { get; set; } = 0;

        public double? DTH { get; set; } = 0;
        public double? TPA { get; set; } = 0;
        public double? TPS { get; set; } = 0;
        public double? PPD { get; set; } = 0;
        public double? CCA { get; set; } = 0;
        public double? CCS { get; set; } = 0;
        public double? PA { get; set; } = 0;
        public double? HS { get; set; } = 0;
        public double? TPD { get; set; } = 0;
        public double? CI { get; set; } = 0;

        public string Frequency { get; set; }
        public int? PreparedById { get; set; }
        public virtual UserBo PreparedByBo { get; set; }

        public string PlanName { get; set; }

        public double? ValuationGross1st { get; set; } = 0;
        public double? ValuationGrossRen { get; set; } = 0;
        public double? ValuationDiscount1st { get; set; } = 0;
        public double? ValuationDiscountRen { get; set; } = 0;
        public double? ValuationCom1st { get; set; } = 0;
        public double? ValuationComRen { get; set; } = 0;
        public double? ValuationClaims { get; set; } = 0;
        public double? ValuationProfitCom { get; set; } = 0;
        public string ValuationMode { get; set; }
        public double? ValuationRiskPremium { get; set; } = 0;

        public string Remark { get; set; }
        public string CurrencyCode { get; set; }
        public double? CurrencyRate { get; set; } = 0;
        public string SoaQuarter { get; set; }

        public string ReasonOfAdjustment1 { get; set; }
        public string ReasonOfAdjustment2 { get; set; }
        public string InvoiceNumber1 { get; set; }
        public DateTime? InvoiceDate1 { get; set; }
        public double? Amount1 { get; set; } = 0;
        public string InvoiceNumber2 { get; set; }
        public DateTime? InvoiceDate2 { get; set; }
        public double? Amount2 { get; set; } = 0;

        public int? TreatyTypeId { get; set; }
        public int? LobId { get; set; }
        public int? Status { get; set; }

        public string ContractCode { get; set; }
        public int? AnnualCohort { get; set; }
        public string Mfrs17CellName { get; set; }

        public int ReportingType { get; set; }

        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }

        // for Ifrs17
        public string ModifiedContractCode { get; set; }
        public string AccountCode { get; set; } 


        public const int InvoiceTypeWM = 1;
        public const int InvoiceTypeCNWM = 2;
        public const int InvoiceTypeDNWM = 3;
        public const int InvoiceTypeOM = 4;
        public const int InvoiceTypeCNOM = 5;
        public const int InvoiceTypeDNOM = 6;
        public const int InvoiceTypeSFWM = 7;
        public const int InvoiceTypeCNSFWM = 8;
        public const int InvoiceTypeDNSFWM = 9;
        public const int InvoiceTypeMax = 9;

        public const int ColumnId = 1;
        public const int ColumnInvoiceRef = 4;
        public const int ColumnInvoiceNo = 5;

        public const int ReportingTypeIFRS17 = 1;
        public const int ReportingTypeIFRS4 = 2;
        public const int ReportingTypeCNIFRS17 = 3;
        public const int ReportingTypeMax = 2;

        public static string GetInvoiceTypeName(int key)
        {
            switch (key)
            {
                case InvoiceTypeWM:
                    return "WM";
                case InvoiceTypeCNWM:
                    return "CNWM";
                case InvoiceTypeDNWM:
                    return "DNWM";
                case InvoiceTypeOM:
                    return "OM";
                case InvoiceTypeCNOM:
                    return "CNOM";
                case InvoiceTypeDNOM:
                    return "DNOM";
                case InvoiceTypeSFWM:
                    return "SFWM";
                case InvoiceTypeCNSFWM:
                    return "CNSFWM";
                case InvoiceTypeDNSFWM:
                    return "DNSFWM";
                default:
                    return "";
            }
        }

        public static string GetBusinessUnit(int key)
        {
            switch (key)
            {
                case InvoiceTypeWM:
                case InvoiceTypeCNWM:
                case InvoiceTypeDNWM:
                case InvoiceTypeSFWM:
                case InvoiceTypeCNSFWM:
                case InvoiceTypeDNSFWM:
                    return "ZFI";
                case InvoiceTypeOM:
                case InvoiceTypeCNOM:
                case InvoiceTypeDNOM:
                    return "ZFO";
                default:
                    return "";
            }
        }

        public static string GetBusinessOriginByInvoiceType(int key)
        {
            switch (key)
            {
                case InvoiceTypeWM:
                case InvoiceTypeCNWM:
                case InvoiceTypeDNWM:
                    return PickListDetailBo.BusinessOriginCodeWithinMalaysia;
                case InvoiceTypeSFWM:
                case InvoiceTypeCNSFWM:
                case InvoiceTypeDNSFWM:
                    return PickListDetailBo.BusinessOriginCodeServiceFee;
                case InvoiceTypeOM:
                case InvoiceTypeCNOM:
                case InvoiceTypeDNOM:
                    return PickListDetailBo.BusinessOriginCodeOutsideMalaysia;
                default:
                    return "";
            }
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

        public double? GetRenewal()
        {
            Renewal = (GrossRenewal.GetValueOrDefault() - AltPremium.GetValueOrDefault() + RiskPremium.GetValueOrDefault());
            if (Equals(double.NaN, Renewal)) Renewal = 0;

            return Renewal = Util.RoundNullableValue(Renewal, 2);
        }

        public double? GetTotalPaid()
        {
            TotalPaid = Gross1st.GetValueOrDefault()
                + GrossRenewal.GetValueOrDefault()
                + AltPremium.GetValueOrDefault()
                - Discount1st.GetValueOrDefault()
                - DiscountRen.GetValueOrDefault()
                - DiscountAlt.GetValueOrDefault()
                + RiskPremium.GetValueOrDefault()
                - Claim.GetValueOrDefault()
                - ProfitComm.GetValueOrDefault()
                - Levy.GetValueOrDefault()
                - SurrenderValue.GetValueOrDefault()
                + ModcoReserveIncome.GetValueOrDefault()
                - ReinsDeposit.GetValueOrDefault()
                - NoClaimBonus.GetValueOrDefault()
                - DatabaseCommission.GetValueOrDefault()
                - AdministrationContribution.GetValueOrDefault()
                + ShareOfRiCommissionFromCompulsoryCession.GetValueOrDefault()
                + RecaptureFee.GetValueOrDefault()
                - CreditCardCharges.GetValueOrDefault()
                - BrokerageFee.GetValueOrDefault()
                + Gst.GetValueOrDefault();
            if (Equals(double.NaN, TotalPaid)) TotalPaid = 0;

            return TotalPaid = Util.RoundNullableValue(TotalPaid, 2);
        }

        public double? GetValuationGross1st()
        {
            ValuationGross1st = Gross1st.GetValueOrDefault();
            return ValuationGross1st;
        }

        public double? GetValuationGrossRen()
        {
            ValuationGrossRen = GrossRenewal.GetValueOrDefault() + AltPremium.GetValueOrDefault();
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
            ValuationCom1st = ((ValuationGross1st.GetValueOrDefault()
                / (ValuationGross1st.GetValueOrDefault() + ValuationGrossRen.GetValueOrDefault()))
                * (DatabaseCommission.GetValueOrDefault() + AdministrationContribution.GetValueOrDefault() 
                - ShareOfRiCommissionFromCompulsoryCession.GetValueOrDefault() + CreditCardCharges.GetValueOrDefault() 
                + BrokerageFee.GetValueOrDefault()));
            if (Equals(double.NaN, ValuationCom1st)) ValuationCom1st = 0;

            return ValuationCom1st = Util.RoundNullableValue(ValuationCom1st, 2);
        }

        public double? GetValuationComRen()
        {
            ValuationComRen = ((ValuationGrossRen.GetValueOrDefault()
                / (ValuationGross1st.GetValueOrDefault() + ValuationGrossRen.GetValueOrDefault()))
                * (DatabaseCommission.GetValueOrDefault() + AdministrationContribution.GetValueOrDefault()
                - ShareOfRiCommissionFromCompulsoryCession.GetValueOrDefault() + CreditCardCharges.GetValueOrDefault()
                + BrokerageFee.GetValueOrDefault()));
            if (Equals(double.NaN, ValuationComRen)) ValuationComRen = 0;

            return ValuationComRen = Util.RoundNullableValue(ValuationComRen, 2);
        }

        public double? GetValuationClaims()
        {
            ValuationClaims = Claim.GetValueOrDefault();
            return ValuationClaims;
        }

        public double? GetValuationProfitCom()
        {
            ValuationProfitCom = ProfitComm.GetValueOrDefault();
            return ValuationProfitCom;
        }

        public double? GetValuationRiskPremium()
        {
            ValuationRiskPremium = RiskPremium.GetValueOrDefault();
            return ValuationRiskPremium;
        }

        public static List<string> GetIncomeTypeItems()
        {
            return new List<string>
            {
                "Gross1st",
                "GrossRenewal",
                "AltPremium",
                "RiskPremium",
                "ModcoReserveIncome",
                "ShareOfRiCommissionFromCompulsor",
                "RecaptureFee",
            };
        }

        public static List<string> GetOutgoTypeItems()
        {
            return new List<string> 
            {
                "Discount1st",
                "DiscountRen",
                "DiscountAlt",
                "Claim",
                "ProfitComm",
                "Levy",
                "SurrenderValue",
                "CreaditCardCharges",
                "BrokerageFee",
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
                    ColIndex = ColumnId,
                    Property = "Id",
                },
                new Column
                {
                    Header = "Status",
                    Property = "Status",
                },
                new Column
                {
                    Header = "Invoice Type",
                    Property = "InvoiceType",
                },
                new Column
                {
                    Header = "Invoice Reference",
                    ColIndex = ColumnInvoiceRef,
                    Property = "InvoiceReference",
                },
                new Column
                {
                    Header = "Invoice Number",
                    ColIndex = ColumnInvoiceNo,
                    Property = "InvoiceNumber",
                },
                new Column
                {
                    Header = "Invoice Date",
                    Property = "InvoiceDate",
                },
                new Column
                {
                    Header = "Statement Received Date",
                    Property = "StatementReceivedDate",
                },
                new Column
                {
                    Header = "Cedant Code",
                    Property = "CedantId",
                },
                new Column
                {
                    Header = "Risk Quarter",
                    Property = "RiskQuarter",
                },
                new Column
                {
                    Header = "Treaty Code",
                    Property = "TreatyCodeId",
                },
                new Column
                {
                    Header = "Treaty Type",
                    Property = "TreatyTypeId",
                },
                new Column
                {
                    Header = "LOB",
                    Property = "LobId",
                },
                new Column
                {
                    Header = "Account Description",
                    Property = "AccountDescription",
                },
                new Column
                {
                    Header = "Total Paid",
                    Property = "TotalPaid",
                },
                new Column
                {
                    Header = "Payment Reference",
                    Property = "PaymentReference",
                },
                new Column
                {
                    Header = "Payment Amount",
                    Property = "PaymentAmount",
                },
                new Column
                {
                    Header = "Payment Received Date",
                    Property = "PaymentReceivedDate",
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
                    Header = "Gross 1st",
                    Property = "Gross1st",
                },
                new Column
                {
                    Header = "Gross Renewal",
                    Property = "GrossRenewal",
                },
                new Column
                {
                    Header = "Alt Premium",
                    Property = "AltPremium",
                },
                new Column
                {
                    Header = "Discount 1st",
                    Property = "Discount1st",
                },
                new Column
                {
                    Header = "Discount Ren",
                    Property = "DiscountRen",
                },
                new Column
                {
                    Header = "Discount Alt",
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
                    Property = "Claim",
                },
                new Column
                {
                    Header = "Profit Comm",
                    Property = "ProfitComm",
                },
                new Column
                {
                    Header = "Levy",
                    Property = "Levy",
                },
                new Column
                {
                    Header = "Surrender Value",
                    Property = "SurrenderValue",
                },
                new Column
                {
                    Header = "GST",
                    Property = "Gst",
                },
                new Column
                {
                    Header = "Modco Reserve Income",
                    Property = "ModcoReserveIncome",
                },
                new Column
                {
                    Header = "Reins Deposit",
                    Property = "ReinsDeposit",
                },
                new Column
                {
                    Header = "No Claim Bonus",
                    Property = "NoClaimBonus",
                },
                new Column
                {
                    Header = "Database Commission",
                    Property = "DatabaseCommission",
                },
                new Column
                {
                    Header = "Administration Contribution",
                    Property = "AdministrationContribution",
                },
                new Column
                {
                    Header = "Share Of Ri Commission From Compulsory Cession",
                    Property = "ShareOfRiCommissionFromCompulsoryCession",
                },
                new Column
                {
                    Header = "Recapture Fee",
                    Property = "RecaptureFee",
                },
                new Column
                {
                    Header = "Credit Card Charges",
                    Property = "CreditCardCharges",
                },
                new Column
                {
                    Header = "Brokerage Fee",
                    Property = "BrokerageFee",
                },
                new Column
                {
                    Header = "NB - Number of cession",
                    Property = "NBCession",
                },
                new Column
                {
                    Header = "NB - Sum Reins",
                    Property = "NBSumReins",
                },
                new Column
                {
                    Header = "RN - Number of cession",
                    Property = "RNCession",
                },
                new Column
                {
                    Header = "RN - Sum Reins",
                    Property = "RNSumReins",
                },
                new Column
                {
                    Header = "ALT - Number of cession",
                    Property = "AltCession",
                },
                new Column
                {
                    Header = "ALT - Sum Reins",
                    Property = "AltSumReins",
                },
                new Column
                {
                    Header = "Frequency",
                    Property = "Frequency",
                },
                new Column
                {
                    Header = "Plan Name",
                    Property = "PlanName",
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
                    Header = "Valuation Claims",
                    Property = "ValuationClaims",
                },
                new Column
                {
                    Header = "Valuation Profit Com",
                    Property = "ValuationProfitCom",
                },
                new Column
                {
                    Header = "Valuation Risk Premium",
                    Property = "ValuationRiskPremium",
                },
                new Column
                {
                    Header = "Remark",
                    Property = "Remark",
                },
                new Column
                {
                    Header = "Currency Code",
                    Property = "CurrencyCode",
                },
                new Column
                {
                    Header = "Currency Rate",
                    Property = "CurrencyRate",
                },
                new Column
                {
                    Header = "Soa Quarter",
                    Property = "SoaQuarter",
                },
                new Column
                {
                    Header = "Reason Of Adjustment 1",
                    Property = "ReasonOfAdjustment1",
                },
                new Column
                {
                    Header = "Reason Of Adjustment 2",
                    Property = "ReasonOfAdjustment2",
                },
                new Column
                {
                    Header = "Invoice Number 1",
                    Property = "InvoiceNumber1",
                },
                new Column
                {
                    Header = "Invoice Date 1",
                    Property = "InvoiceDate1",
                },
                new Column
                {
                    Header = "Amount 1",
                    Property = "Amount1",
                },
                new Column
                {
                    Header = "Invoice Number 2",
                    Property = "InvoiceNumber2",
                },
                new Column
                {
                    Header = "Invoice Date 2",
                    Property = "InvoiceDate2",
                },
                new Column
                {
                    Header = "Amount 2",
                    Property = "Amount2",
                },
                new Column
                {
                    Header = "Prepared By",
                    Property = "PreparedById",
                },
                new Column
                {
                    Header = "DTH",
                    Property = "DTH",
                },
                new Column
                {
                    Header = "TPD",
                    Property = "TPD",
                },
                new Column
                {
                    Header = "CI",
                    Property = "CI",
                },
                new Column
                {
                    Header = "PA",
                    Property = "PA",
                },
                new Column
                {
                    Header = "H&S",
                    Property = "HS",
                },
            };

            return columns;
        }
    }
}
