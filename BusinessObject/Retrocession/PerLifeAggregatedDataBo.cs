using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Retrocession
{
    public class PerLifeAggregatedDataBo
    {
        public int Id { get; set; }

        public int PerLifeAggregationDetailId { get; set; }

        public PerLifeAggregationDetailBo PerLifeAggregationDetailBo { get; set; }

        public string TreatyCode { get; set; }

        public string ReinsBasisCode { get; set; }

        public string FundsAccountingTypeCode { get; set; }

        public string PremiumFrequencyCode { get; set; }

        public int? ReportPeriodMonth { get; set; }

        public int? ReportPeriodYear { get; set; }

        public int? RiskPeriodMonth { get; set; }

        public int? RiskPeriodYear { get; set; }

        public string TransactionTypeCode { get; set; }

        public string PolicyNumber { get; set; }

        public DateTime? IssueDatePol { get; set; }

        public DateTime? IssueDateBen { get; set; }

        public DateTime? ReinsEffDatePol { get; set; }

        public DateTime? ReinsEffDateBen { get; set; }

        public string CedingPlanCode { get; set; }

        public string CedingBenefitTypeCode { get; set; }

        public string CedingBenefitRiskCode { get; set; }

        public string MlreBenefitCode { get; set; }

        public double? OriSumAssured { get; set; }

        public double? CurrSumAssured { get; set; }

        public double? AmountCededB4MlreShare { get; set; }

        public double? AarOri { get; set; }

        public double? Aar { get; set; }

        public string InsuredName { get; set; }

        public string InsuredGenderCode { get; set; }

        public string InsuredTobaccoUse { get; set; }

        public DateTime? InsuredDateOfBirth { get; set; }

        public string InsuredOccupationCode { get; set; }

        public string InsuredRegisterNo { get; set; }

        public int? InsuredAttainedAge { get; set; }

        public string InsuredNewIcNumber { get; set; }

        public string InsuredOldIcNumber { get; set; }

        public int? ReinsuranceIssueAge { get; set; }

        public double? PolicyTerm { get; set; }

        public DateTime? PolicyExpiryDate { get; set; }

        public string LoadingType { get; set; }

        public double? UnderwriterRating { get; set; }

        public double? FlatExtraAmount { get; set; }

        public double? StandardPremium { get; set; }

        public double? SubstandardPremium { get; set; }

        public double? FlatExtraPremium { get; set; }

        public double? GrossPremium { get; set; }

        public double? StandardDiscount { get; set; }

        public double? SubstandardDiscount { get; set; }

        public double? NetPremium { get; set; }

        public string PolicyNumberOld { get; set; }

        public int? PolicyLifeNumber { get; set; }

        public string FundCode { get; set; }

        public int? RiderNumber { get; set; }

        public string CampaignCode { get; set; }

        public string Nationality { get; set; }

        public string TerritoryOfIssueCode { get; set; }

        public string CurrencyCode { get; set; }

        public string StaffPlanIndicator { get; set; }

        public string CedingPlanCodeOld { get; set; }

        public string CedingBasicPlanCode { get; set; }

        public string GroupPolicyNumber { get; set; }

        public string GroupPolicyName { get; set; }

        public string GroupSubsidiaryName { get; set; }

        public string GroupSubsidiaryNo { get; set; }

        public string CedingPlanCode2 { get; set; }

        public string DependantIndicator { get; set; }

        public string Mfrs17BasicRider { get; set; }

        public string Mfrs17CellName { get; set; }

        public string Mfrs17ContractCode { get; set; }

        public string LoaCode { get; set; }

        public DateTime? RiskPeriodStartDate { get; set; }

        public DateTime? RiskPeriodEndDate { get; set; }

        public int? Mfrs17AnnualCohort { get; set; }

        public double? BrokerageFee { get; set; }

        public double? ApLoading { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public double? AnnuityFactor { get; set; }

        public string EndingPolicyStatus { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        public string TreatyType { get; set; }

        public string TreatyNumber { get; set; }

        public string RetroPremFreq { get; set; }

        public bool LifeBenefitFlag { get; set; }

        public string RiskQuarter { get; set; }

        public DateTime? ProcessingDate { get; set; }

        public string UniqueKeyPerLife { get; set; }

        public string RetroBenefitCode { get; set; }

        public double? RetroRatio { get; set; }

        public double? AccumulativeRetainAmount { get; set; }

        public double? RetroRetainAmount { get; set; }

        public double? RetroAmount { get; set; }

        public double? RetroGrossPremium { get; set; }

        public double? RetroNetPremium { get; set; }

        public double? RetroDiscount { get; set; }

        public double? RetroExtraPremium { get; set; }

        public double? RetroExtraComm { get; set; }

        public double? RetroGst { get; set; }

        public string RetroTreaty { get; set; }

        public string RetroClaimId { get; set; }

        public string Soa { get; set; }

        public bool RetroIndicator { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }
    }
}
