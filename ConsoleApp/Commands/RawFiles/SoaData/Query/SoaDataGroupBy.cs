namespace ConsoleApp.Commands.RawFiles.SoaData.Query
{
    public class SoaDataGroupBy
    {
        public string TreatyCode { get; set; }
        public string RiskQuarter { get; set; }
        public double? NbPremium { get; set; }
        public double? RnPremium { get; set; }
        public double? AltPremium { get; set; }
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
        public double? NetTotalAmount { get; set; }

        public string CurrencyCode { get; set; }
        public double? CurrencyRate { get; set; }

        public double? GetNetTotalAmount()
        {
            NetTotalAmount = NbPremium.GetValueOrDefault()
                + RnPremium.GetValueOrDefault()
                - AltPremium.GetValueOrDefault()
                - TotalDiscount.GetValueOrDefault()
                + RiskPremium.GetValueOrDefault()
                - Claim.GetValueOrDefault()
                - NoClaimBonus.GetValueOrDefault()
                - ProfitComm.GetValueOrDefault()
                - Levy.GetValueOrDefault()
                - SurrenderValue.GetValueOrDefault()
                + ModcoReserveIncome.GetValueOrDefault()
                - RiDeposit.GetValueOrDefault()
                - DatabaseCommission.GetValueOrDefault()
                - AdministrationContribution.GetValueOrDefault()
                - ShareOfRiCommissionFromCompulsoryCession.GetValueOrDefault()
                - RecaptureFee.GetValueOrDefault()
                - CreditCardCharges.GetValueOrDefault()
                - BrokerageFee.GetValueOrDefault()
                ;
            return NetTotalAmount;
        }
    }
}
