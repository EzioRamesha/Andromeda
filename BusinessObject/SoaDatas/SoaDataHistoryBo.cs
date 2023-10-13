using System;

namespace BusinessObject.SoaDatas
{
    public class SoaDataHistoryBo
    {
        public int Id { get; set; }

        public int CutOffId { get; set; }
        public virtual CutOffBo CutOff { get; set; }

        public int SoaDataBatchId { get; set; }
        public virtual SoaDataBatchHistoryBo SoaDataBatchHistoryBo { get; set; }

        public int? SoaDataFileId { get; set; }

        public int MappingStatus { get; set; }
        public string Errors { get; set; }


        public string CompanyName { get; set; }

        public string BusinessCode { get; set; }

        public string TreatyId { get; set; }

        public string TreatyCode { get; set; }

        public string TreatyMode { get; set; }

        public string TreatyType { get; set; }

        public string PlanBlock { get; set; }

        public int? RiskMonth { get; set; }

        public string SoaQuarter { get; set; }

        public string RiskQuarter { get; set; }

        public double? NbPremium { get; set; }

        public double? RnPremium { get; set; }

        public double? AltPremium { get; set; }

        public double? GrossPremium { get; set; }

        public double? TotalDiscount { get; set; }

        public double? RiskPremium { get; set; }

        public double? NoClaimBonus { get; set; }

        public double? Levy { get; set; }

        public double? Claim { get; set; }

        public double? ProfitComm { get; set; }

        public double? SurrenderValue { get; set; }

        public double? Gst { get; set; }

        public double? ModcoReserveIncome { get; set; }

        public double? RiDeposit { get; set; }

        public double? DatabaseCommission { get; set; }

        public double? AdministrationContribution { get; set; }

        public double? ShareOfRiCommissionFromCompulsoryCession { get; set; }

        public double? RecaptureFee { get; set; }

        public double? CreditCardCharges { get; set; }

        public double? BrokerageFee { get; set; }

        public double? TotalCommission { get; set; }

        public double? NetTotalAmount { get; set; }

        public DateTime? SoaReceivedDate { get; set; }

        public DateTime? BordereauxReceivedDate { get; set; }

        public string StatementStatus { get; set; }

        public string Remarks1 { get; set; }

        public string CurrencyCode { get; set; }

        public double? CurrencyRate { get; set; }

        public string SoaStatus { get; set; }

        public DateTime? ConfirmationDate { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }
    }
}
