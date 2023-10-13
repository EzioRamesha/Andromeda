using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class SoaDataCompiledSummaryViewModel
    {
        public int Id { get; set; }

        public string InvoiceType { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public DateTime? StatementReceivedDate { get; set; }

        public string RiskQuarter { get; set; }

        public string TreatyCode { get; set; }

        public string AccountsFor { get; set; }

        public double? NBPremium { get; set; }

        public double? RNPremium { get; set; }

        public double? ALTPremium { get; set; }

        public double? NBDiscount { get; set; }

        public double? RNDiscount { get; set; }

        public double? ALTDiscount { get; set; }

        public double? RiskPremium { get; set; }

        public double? Claim { get; set; }

        public double? ProfitComm { get; set; }

        public double? Levy { get; set; }

        public double? SurrenderValue { get; set; }

        public double? Gst { get; set; }

        public double? MODCOReserveIncome { get; set; }

        public double? RiDeposit { get; set; }

        public double? NoClaimBonus { get; set; }

        public double? DatabaseCommission { get; set; }

        public double? AdministrationContribution { get; set; }

        public double? ShareOfRiCommissionFromCompulsoryCession { get; set; }

        public double? RecaptureFee { get; set; }

        public double? CreditCardCharges { get; set; }

        public double? BrokerageFee { get; set; }

        public double? NetTotalAmount { get; set; }

        public string PlanName { get; set; }

        public string CurrencyCode { get; set; }

        public double? CurrencyRate { get; set; }

        public int? NBCession { get; set; }

        public int? RNCession { get; set; }

        public int? ALTCession { get; set; }

        public double? NBSar { get; set; }

        public double? RNSar { get; set; }

        public double? ALTSar { get; set; }

        public string PreparedBy { get; set; }

        public double? DTH { get; set; }

        public double? TPA { get; set; }

        public double? TPS { get; set; }

        public double? PPD { get; set; }

        public double? CCA { get; set; }

        public double? CCS { get; set; }

        public double? PA { get; set; }

        public double? HS { get; set; }

        public string SoaQuarter { get; set; }

        public string ReasonOfAdjustment1 { get; set; }

        public string ReasonOfAdjustment2 { get; set; }

        public string InvoiceNo1 { get; set; }

        public DateTime? InvoiceDate1 { get; set; }

        public double? Amount1 { get; set; }

        public string InvoiceNo2 { get; set; }

        public DateTime? InvoiceDate2 { get; set; }

        public double? Amount2 { get; set; }

        public string FilingReference { get; set; }

        public double? PercentageOfServiceFee { get; set; }

        public double? ServiceFee { get; set; }

        public double? SSTPpayable { get; set; }

        public double? TotalAmount { get; set; }
    }
}