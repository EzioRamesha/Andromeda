using System;

namespace BusinessObject.InvoiceRegisters
{
    public class InvoiceRegisterHistoryBo
    {
        public int Id { get; set; }

        public int CutOffId { get; set; }
        public virtual CutOffBo CutOffBo { get; set; }

        public int InvoiceRegisterId { get; set; }
        public int InvoiceRegisterBatchId { get; set; }
        public int? SoaDataBatchId { get; set; }

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


        public const int ReportingTypeIFRS17 = 1;
        public const int ReportingTypeIFRS4 = 2;
        public const int ReportingTypeCNIFRS17 = 3;
        public const int ReportingTypeMax = 2;
    }
}
