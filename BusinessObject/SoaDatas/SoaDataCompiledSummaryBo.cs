using BusinessObject.Identity;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject.SoaDatas
{
    public class SoaDataCompiledSummaryBo
    {
        public int Id { get; set; }


        public int SoaDataBatchId { get; set; }
        public virtual SoaDataBatchBo SoaDataBatchBo { get; set; }


        public int InvoiceType { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime? StatementReceivedDate { get; set; }


        public string BusinessCode { get; set; }
        public string TreatyCode { get; set; }
        public string SoaQuarter { get; set; }
        public string RiskQuarter { get; set; }


        public string AccountDescription { get; set; }


        public double? NbPremium { get; set; } = 0;
        public double? RnPremium { get; set; } = 0;
        public double? AltPremium { get; set; } = 0;


        public double? NbDiscount { get; set; } = 0;
        public double? RnDiscount { get; set; } = 0;
        public double? AltDiscount { get; set; } = 0;

        public double? TotalPremium { get; set; } = 0;
        public double? TotalDiscount { get; set; } = 0;
        public double? RiskPremium { get; set; } = 0;
        public double? NoClaimBonus { get; set; } = 0;
        public double? Levy { get; set; } = 0;
        public double? Claim { get; set; } = 0;
        public double? ProfitComm { get; set; } = 0;
        public double? SurrenderValue { get; set; } = 0;
        public double? Gst { get; set; } = 0;
        public double? ModcoReserveIncome { get; set; } = 0;
        public double? RiDeposit { get; set; } = 0;
        public double? DatabaseCommission { get; set; } = 0;
        public double? AdministrationContribution { get; set; } = 0;
        public double? ShareOfRiCommissionFromCompulsoryCession { get; set; } = 0;
        public double? RecaptureFee { get; set; } = 0;
        public double? CreditCardCharges { get; set; } = 0;
        public double? BrokerageFee { get; set; } = 0;
        public double? NetTotalAmount { get; set; } = 0;


        public string ReasonOfAdjustment1 { get; set; }
        public string ReasonOfAdjustment2 { get; set; }
        public string InvoiceNumber1 { get; set; }
        public DateTime? InvoiceDate1 { get; set; }
        public double? Amount1 { get; set; } = 0;
        public string InvoiceNumber2 { get; set; }
        public DateTime? InvoiceDate2 { get; set; }
        public double? Amount2 { get; set; } = 0;
        public string FilingReference { get; set; }
        public double? ServiceFeePercentage { get; set; } = 0;
        public double? ServiceFee { get; set; } = 0;
        public double? Sst { get; set; } = 0;
        public double? TotalAmount { get; set; } = 0;


        public int? NbCession { get; set; } = 0;
        public int? RnCession { get; set; } = 0;
        public int? AltCession { get; set; } = 0;

        public double? NbSar { get; set; } = 0;
        public double? RnSar { get; set; } = 0;
        public double? AltSar { get; set; } = 0;

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

        public string ContractCode { get; set; }
        public int? AnnualCohort { get; set; }
        public string Frequency { get; set; }
        public string Mfrs17CellName { get; set; }

        public int ReportingType { get; set; }

        public int CreatedById { get; set; }

        public UserBo CreatedByBo { get; set; }

        public int? UpdatedById { get; set; }

        public string StatementReceivedDateStr { get; set; }
        public string InvoiceDate1Str { get; set; }
        public string Amount1Str { get; set; }
        public string InvoiceDate2Str { get; set; }
        public string Amount2Str { get; set; }

        public string CurrencyCode { get; set; }
        public double? CurrencyRate { get; set; }
        public string CurrencyRateStr { get; set; }

        public string NbPremiumStr { get; set; }
        public string RnPremiumStr { get; set; }
        public string AltPremiumStr { get; set; }

        public string NbDiscountStr { get; set; }
        public string RnDiscountStr { get; set; }
        public string AltDiscountStr { get; set; }

        public string NbSarStr { get; set; }
        public string RnSarStr { get; set; }
        public string AltSarStr { get; set; }


        public string TotalDiscountStr { get; set; }
        public string RiskPremiumStr { get; set; }
        public string ProfitCommStr { get; set; }
        public string LevyStr { get; set; }
        public string ClaimStr { get; set; }
        public string SurrenderValueStr { get; set; }
        public string NoClaimBonusStr { get; set; }
        public string DatabaseCommissionStr { get; set; }
        public string BrokerageFeeStr { get; set; }
        public string ServiceFeeStr { get; set; }
        public string GstStr { get; set; }
        public string ModcoReserveIncomeStr { get; set; }
        public string RiDepositStr { get; set; }
        public string AdministrationContributionStr { get; set; }
        public string ShareOfRiCommissionFromCompulsoryCessionStr { get; set; }
        public string RecaptureFeeStr { get; set; }
        public string CreditCardChargesStr { get; set; }
        public string NetTotalAmountStr { get; set; }
        public string ServiceFeePercentageStr { get; set; }
        public string SstStr { get; set; }
        public string TotalAmountStr { get; set; }

        public string DTHStr { get; set; }
        public string TPAStr { get; set; }
        public string TPSStr { get; set; }
        public string PPDStr { get; set; }
        public string CCAStr { get; set; }
        public string CCSStr { get; set; }
        public string PAStr { get; set; }
        public string HSStr { get; set; }
        public string TPDStr { get; set; }
        public string CIStr { get; set; }

        public string CreatedByPerson { get; set; }


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

        public const int ColumnInvoiceType = 1;
        public const int ColumnCurrencyCode = 32;
        public const int ColumnCurrencyRate = 33;
        public const int ColumnPreparedBy = 37;

        public const int ReportingTypeIFRS17 = 1;
        public const int ReportingTypeIFRS4 = 2;
        public const int ReportingTypeCNIFRS17 = 3;
        public const int ReportingTypeMax = 2;

        public static string GetInvoiceTypeName(int? key)
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

        public double? GetNetTotalAmount()
        {
            NetTotalAmount = NbPremium.GetValueOrDefault()
                + RnPremium.GetValueOrDefault()
                + AltPremium.GetValueOrDefault()
                - NbDiscount.GetValueOrDefault()
                - RnDiscount.GetValueOrDefault()
                - AltDiscount.GetValueOrDefault()
                + RiskPremium.GetValueOrDefault()
                - Claim.GetValueOrDefault()
                - ProfitComm.GetValueOrDefault()
                - Levy.GetValueOrDefault()
                - SurrenderValue.GetValueOrDefault()
                + ModcoReserveIncome.GetValueOrDefault()
                - RiDeposit.GetValueOrDefault()
                - NoClaimBonus.GetValueOrDefault()
                - DatabaseCommission.GetValueOrDefault()
                - AdministrationContribution.GetValueOrDefault()
                + ShareOfRiCommissionFromCompulsoryCession.GetValueOrDefault()
                + RecaptureFee.GetValueOrDefault()
                - CreditCardCharges.GetValueOrDefault()
                - BrokerageFee.GetValueOrDefault()
                + Gst.GetValueOrDefault();
            if (Equals(double.NaN, NetTotalAmount)) NetTotalAmount = 0;
            return NetTotalAmount = Util.RoundNullableValue(NetTotalAmount, 2);
        }

        public double? GetSstPayable()
        {
            Sst = (ServiceFee.GetValueOrDefault() * 6) / 100;
            if (Equals(double.NaN, Sst)) Sst = 0;
            return Sst = Util.RoundNullableValue(Sst, 2);
        }

        public double? GetTotalAmount()
        {
            TotalAmount = ServiceFee.GetValueOrDefault() + Sst.GetValueOrDefault();
            return TotalAmount = Util.RoundNullableValue(TotalAmount, 2);
        }

        public double? GetTotalPremium()
        {
            TotalPremium = NbPremium.GetValueOrDefault() + RnPremium.GetValueOrDefault() + AltPremium.GetValueOrDefault();
            return TotalPremium = Util.RoundNullableValue(TotalPremium, 2);
        }

        public double? GetRiskPremium(double? TotalPremium, double? SoaRiskPremium)
        {
            RiskPremium = ((NbPremium.GetValueOrDefault() + RnPremium.GetValueOrDefault() + AltPremium.GetValueOrDefault()) / TotalPremium.GetValueOrDefault())
                * SoaRiskPremium.GetValueOrDefault();
            if (Equals(double.NaN, RiskPremium)) RiskPremium = 0;
            return RiskPremium = Util.RoundNullableValue(RiskPremium, 2);
        }

        public static List<Column> GetColumns(bool TypeIFRS17 = false)
        {
            var columns = new List<Column>
            {
                new Column
                {
                    Header = "Invoice Type",
                    Property = "InvoiceType",
                },
                new Column
                {
                    Header = "Stt Received Date",
                    Property = "StatementReceivedDate",
                },
                new Column
                {
                    Header = "Risk Qtr",
                    Property = "RiskQuarter",
                },
                new Column
                {
                    Header = "Treaty Code",
                    Property = "TreatyCode",
                },
            };

            if (TypeIFRS17)
            {
                columns.Add(new Column
                {
                    Header = "Contract Code",
                    Property = "ContractCode",
                });
                columns.Add(new Column
                {
                    Header = "Annual Cohort",
                    Property = "AnnualCohort",
                });
            }

            columns.Add(new Column
            {
                Header = "Account For",
                Property = "AccountDescription",
            });
            columns.Add(new Column
            {
                Header = "NB Premium",
                Property = "NbPremium",
            });
            columns.Add(new Column
            {
                Header = "RN Premium",
                Property = "RnPremium",
            });
            columns.Add(new Column
            {
                Header = "ALT Premium",
                Property = "AltPremium",
            });
            columns.Add(new Column
            {
                Header = "NB Discount",
                Property = "NbDiscount",
            });
            columns.Add(new Column
            {
                Header = "RN Discount",
                Property = "RnDiscount",
            });
            columns.Add(new Column
            {
                Header = "ALT Discount",
                Property = "AltDiscount",
            });
            columns.Add(new Column
            {
                Header = "Risk Premium",
                Property = "RiskPremium",
            });
            columns.Add(new Column
            {
                Header = "Claim",
                Property = "Claim",
            });
            columns.Add(new Column
            {
                Header = "Profit Comm",
                Property = "ProfitComm",
            });
            columns.Add(new Column
            {
                Header = "Levy",
                Property = "Levy",
            });
            columns.Add(new Column
            {
                Header = "Surr Value",
                Property = "SurrenderValue",
            });
            columns.Add(new Column
            {
                Header = "GST",
                Property = "Gst",
            });
            columns.Add(new Column
            {
                Header = "Modco Reserve Income",
                Property = "ModcoReserveIncome",
            });
            columns.Add(new Column
            {
                Header = "Ri Deposit",
                Property = "RiDeposit",
            });
            columns.Add(new Column
            {
                Header = "No Claim Bonus",
                Property = "NoClaimBonus",
            });
            columns.Add(new Column
            {
                Header = "Database Commission",
                Property = "DatabaseCommission",
            });
            columns.Add(new Column
            {
                Header = "Administration Contribution",
                Property = "AdministrationContribution",
            });
            columns.Add(new Column
            {
                Header = "Share Of Ri Commission From Compulsory Cession",
                Property = "ShareOfRiCommissionFromCompulsoryCession",
            });
            columns.Add(new Column
            {
                Header = "Recapture Fee",
                Property = "RecaptureFee",
            });
            columns.Add(new Column
            {
                Header = "Credit Card Charges",
                Property = "CreditCardCharges",
            });
            columns.Add(new Column
            {
                Header = "Brokerage Fee",
                Property = "BrokerageFee",
            });
            columns.Add(new Column
            {
                Header = "Net Total Amount",
                Property = "NetTotalAmount",
            });
            columns.Add(new Column
            {
                Header = "Percentage Of Service Fee",
                Property = "ServiceFeePercentage",
            });
            columns.Add(new Column
            {
                Header = "Service Fee",
                Property = "ServiceFee",
            });
            columns.Add(new Column
            {
                Header = "Sst Payable",
                Property = "Sst",
            });
            columns.Add(new Column
            {
                Header = "Total Amount",
                Property = "TotalAmount",
            });
            columns.Add(new Column
            {
                Header = "Currency Code",
                Property = "CurrencyCode",
            });
            columns.Add(new Column
            {
                Header = "Currency Rate",
                Property = "CurrencyRateStr",
            });
            columns.Add(new Column
            {
                Header = "NB Cession",
                Property = "NbCession",
            });
            columns.Add(new Column
            {
                Header = "RN Cession",
                Property = "RnCession",
            });
            columns.Add(new Column
            {
                Header = "ALT Cession",
                Property = "AltCession",
            });
            columns.Add(new Column
            {
                Header = "NB SAR",
                Property = "NbSar",
            });
            columns.Add(new Column
            {
                Header = "RN SAR",
                Property = "RnSar",
            });
            columns.Add(new Column
            {
                Header = "ALT SAR",
                Property = "AltSar",
            });
            columns.Add(new Column
            {
                Header = "Prepared By",
                Property = "CreatedById",
            });
            columns.Add(new Column
            {
                Header = "DTH",
                Property = "DTH",
            });
            columns.Add(new Column
            {
                Header = "TPD",
                Property = "TPD",
            });
            columns.Add(new Column
            {
                Header = "CI",
                Property = "CI",
            });
            columns.Add(new Column
            {
                Header = "PA",
                Property = "PA",
            });
            columns.Add(new Column
            {
                Header = "HS",
                Property = "HS",
            });
            columns.Add(new Column
            {
                Header = "Soa Qtr",
                Property = "SoaQuarter",
            });
            columns.Add(new Column
            {
                Header = "Reason Of Adjustment 1",
                Property = "ReasonOfAdjustment1",
            });
            columns.Add(new Column
            {
                Header = "Reason Of Adjustment 2",
                Property = "ReasonOfAdjustment2",
            });
            columns.Add(new Column
            {
                Header = "Invoice No 1",
                Property = "InvoiceNumber1",
            });
            columns.Add(new Column
            {
                Header = "Invoice Date 1",
                Property = "InvoiceDate1",
            });
            columns.Add(new Column
            {
                Header = "Amount 1",
                Property = "Amount1",
            });
            columns.Add(new Column
            {
                Header = "Invoice No 2",
                Property = "InvoiceNumber2",
            });
            columns.Add(new Column
            {
                Header = "Invoice Date 2",
                Property = "InvoiceDate2",
            });
            columns.Add(new Column
            {
                Header = "Amount 2",
                Property = "Amount2",
            });

            return columns;
        }
    }
}
