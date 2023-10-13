using System;

namespace BusinessObject
{
    public class RetroRegisterHistoryBo
    {
        public int Id { get; set; }

        public int CutOffId { get; set; }
        public virtual CutOffBo CutOffBo { get; set; }

        public int Type { get; set; }

        public int RetroRegisterId { get; set; }
        public int? RetroRegisterBatchId { get; set; }

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

        public string ContractCode { get; set; }
        public int? AnnualCohort { get; set; }

        public int ReportingType { get; set; }


        public const int ReportingTypeIFRS17 = 1;
        public const int ReportingTypeIFRS4 = 2;
        public const int ReportingTypeMax = 2;
    }
}
