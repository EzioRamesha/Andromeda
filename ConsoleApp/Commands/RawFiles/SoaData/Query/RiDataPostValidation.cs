namespace ConsoleApp.Commands.RawFiles.SoaData.Query
{
    public class RiDataPostValidation
    {
        public string TreatyCode { get; set; }
        public int? RiskPeriodMonth { get; set; }
        public int? RiskPeriodYear { get; set; }

        public double? NoClaimBonus { get; set; }
        public double? SurrenderValue { get; set; }

        public double? NetPremium { get; set; }
        public double? TransactionPremium { get; set; }
        public double? MlreGrossPremium { get; set; }

        public string CedingPlanCode { get; set; }

        public string CurrencyCode { get; set; }
        public double? CurrencyRate { get; set; }

        public string  PremiumFrequencyCode { get; set; }
        public string ContractCode { get; set; }
        public int? AnnualCohort { get; set; }
        public string Mfrs17CellName { get; set; }
    }
}
