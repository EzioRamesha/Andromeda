namespace ConsoleApp.Commands.RawFiles.SoaData.Query
{
    public class RiDataGroupBy
    {
        public string TreatyCode { get; set; }
        public int? RiskPeriodMonth { get; set; }
        public int? RiskPeriodYear { get; set; }
        public double? TotalDiscount { get; set; }
        public double? NoClaimBonus { get; set; }
        public double? SurrenderValue { get; set; }
        public double? GstAmount { get; set; }
        public double? DatabaseCommission { get; set; }
        public double? BrokerageFee { get; set; }
        public double? ServiceFee { get; set; }
        public string CurrencyCode { get; set; }
        public double? CurrencyRate { get; set; }
        public double? TransactionDiscount { get; set; }

        public string ContractCode { get; set; }
        public int? AnnualCohort { get; set; }
        public string Mfrs17CellName { get; set; }

        // to link during Auto Created SOA
        public string PremiumFrequencyCode { get; set; }
        public string TreatyType { get; set; }
    }
}
