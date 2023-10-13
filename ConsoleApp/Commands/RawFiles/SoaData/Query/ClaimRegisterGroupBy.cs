namespace ConsoleApp.Commands.RawFiles.SoaData.Query
{
    public class ClaimRegisterGroupBy
    {
        public string TreatyCode { get; set; }
        public string TreatyType { get; set; }
        public string SoaQuarter { get; set; }
        public string RiskQuarter { get; set; }
        public int? RiskPeriodMonth { get; set; }
        public int? RiskPeriodYear { get; set; }
        public double? ClaimRecoveryAmt { get; set; }
        public double? ForeignClaimRecoveryAmt { get; set; }

        public string ContractCode { get; set; }
        public int? AnnualCohort { get; set; }
    }
}
