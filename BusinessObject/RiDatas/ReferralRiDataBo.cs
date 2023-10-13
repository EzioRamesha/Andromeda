using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.RiDatas
{
    public class ReferralRiDataBo
    {
        public int Id { get; set; }

        public int ReferralRiDataFileId { get; set; }
        public ReferralRiDataFileBo ReferralRiDataFileBo { get; set; }

        [MaxLength(35)]
        public string TreatyCode { get; set; }

        [MaxLength(30)]
        public string ReinsBasisCode { get; set; }

        [MaxLength(30)]
        public string FundsAccountingTypeCode { get; set; }

        [MaxLength(10)]
        public string PremiumFrequencyCode { get; set; }

        public int? ReportPeriodMonth { get; set; }

        public int? ReportPeriodYear { get; set; }

        public int? RiskPeriodMonth { get; set; }

        public int? RiskPeriodYear { get; set; }

        [MaxLength(2)]
        public string TransactionTypeCode { get; set; }

        [MaxLength(150)]
        public string PolicyNumber { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? IssueDatePol { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? IssueDateBen { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ReinsEffDatePol { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ReinsEffDateBen { get; set; }

        [MaxLength(30)]
        public string CedingPlanCode { get; set; }

        [MaxLength(30)]
        public string CedingBenefitTypeCode { get; set; }

        [MaxLength(50)]
        public string CedingBenefitRiskCode { get; set; }

        [MaxLength(10)]
        public string MlreBenefitCode { get; set; }

        public double? OriSumAssured { get; set; }

        public double? CurrSumAssured { get; set; }

        public double? AmountCededB4MlreShare { get; set; }

        public double? RetentionAmount { get; set; }

        public double? AarOri { get; set; }

        public double? Aar { get; set; }

        public double? AarSpecial1 { get; set; }

        public double? AarSpecial2 { get; set; }

        public double? AarSpecial3 { get; set; }

        [MaxLength(128)]
        public string InsuredName { get; set; }

        [MaxLength(1)]
        public string InsuredGenderCode { get; set; }

        [MaxLength(1)]
        public string InsuredTobaccoUse { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? InsuredDateOfBirth { get; set; }

        [MaxLength(5)]
        public string InsuredOccupationCode { get; set; }

        [MaxLength(30)]
        public string InsuredRegisterNo { get; set; }

        public int? InsuredAttainedAge { get; set; }

        [MaxLength(15)]
        public string InsuredNewIcNumber { get; set; }

        [MaxLength(15)]
        public string InsuredOldIcNumber { get; set; }

        [MaxLength(128)]
        public string InsuredName2nd { get; set; }

        [MaxLength(1)]
        public string InsuredGenderCode2nd { get; set; }

        [MaxLength(1)]
        public string InsuredTobaccoUse2nd { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? InsuredDateOfBirth2nd { get; set; }

        public int? InsuredAttainedAge2nd { get; set; }

        [MaxLength(15)]
        public string InsuredNewIcNumber2nd { get; set; }

        [MaxLength(15)]
        public string InsuredOldIcNumber2nd { get; set; }

        public int? ReinsuranceIssueAge { get; set; }

        public int? ReinsuranceIssueAge2nd { get; set; }

        public double? PolicyTerm { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? PolicyExpiryDate { get; set; }

        public double? DurationYear { get; set; }

        public double? DurationDay { get; set; }

        public double? DurationMonth { get; set; }

        [MaxLength(5)]
        public string PremiumCalType { get; set; }

        public double? CedantRiRate { get; set; }

        [MaxLength(50)]
        public string RateTable { get; set; }

        public int? AgeRatedUp { get; set; }

        public double? DiscountRate { get; set; }

        [MaxLength(15)]
        public string LoadingType { get; set; }

        public double? UnderwriterRating { get; set; }

        public double? UnderwriterRatingUnit { get; set; }

        public int? UnderwriterRatingTerm { get; set; }

        public double? UnderwriterRating2 { get; set; }

        public double? UnderwriterRatingUnit2 { get; set; }

        public int? UnderwriterRatingTerm2 { get; set; }

        public double? UnderwriterRating3 { get; set; }

        public double? UnderwriterRatingUnit3 { get; set; }

        public int? UnderwriterRatingTerm3 { get; set; }

        public double? FlatExtraAmount { get; set; }

        public double? FlatExtraUnit { get; set; }

        public int? FlatExtraTerm { get; set; }

        public double? FlatExtraAmount2 { get; set; }

        public int? FlatExtraTerm2 { get; set; }

        public double? StandardPremium { get; set; }

        public double? SubstandardPremium { get; set; }

        public double? FlatExtraPremium { get; set; }

        public double? GrossPremium { get; set; }

        public double? StandardDiscount { get; set; }

        public double? SubstandardDiscount { get; set; }

        public double? VitalityDiscount { get; set; }

        public double? TotalDiscount { get; set; }

        public double? NetPremium { get; set; }

        public double? AnnualRiPrem { get; set; }

        public double? RiCovPeriod { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AdjBeginDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AdjEndDate { get; set; }

        [MaxLength(150)]
        public string PolicyNumberOld { get; set; }

        [MaxLength(20)]
        public string PolicyStatusCode { get; set; }

        public double? PolicyGrossPremium { get; set; }

        public double? PolicyStandardPremium { get; set; }

        public double? PolicySubstandardPremium { get; set; }

        public double? PolicyTermRemain { get; set; }

        public double? PolicyAmountDeath { get; set; }

        public double? PolicyReserve { get; set; }

        [MaxLength(10)]
        public string PolicyPaymentMethod { get; set; }

        public int? PolicyLifeNumber { get; set; }

        [MaxLength(25)]
        public string FundCode { get; set; }

        [MaxLength(5)]
        public string LineOfBusiness { get; set; }

        public double? ApLoading { get; set; }

        public double? LoanInterestRate { get; set; }

        public int? DefermentPeriod { get; set; }

        public int? RiderNumber { get; set; }

        [MaxLength(10)]
        public string CampaignCode { get; set; }

        [MaxLength(20)]
        public string Nationality { get; set; }

        [MaxLength(20)]
        public string TerritoryOfIssueCode { get; set; }

        [MaxLength(3)]
        public string CurrencyCode { get; set; }

        [MaxLength(1)]
        public string StaffPlanIndicator { get; set; }

        [MaxLength(30)]
        public string CedingTreatyCode { get; set; }

        [MaxLength(30)]
        public string CedingPlanCodeOld { get; set; }

        [MaxLength(30)]
        public string CedingBasicPlanCode { get; set; }

        public double? CedantSar { get; set; }

        [MaxLength(10)]
        public string CedantReinsurerCode { get; set; }

        public double? AmountCededB4MlreShare2 { get; set; }

        [MaxLength(10)]
        public string CessionCode { get; set; }

        [MaxLength(255)]
        public string CedantRemark { get; set; }

        [MaxLength(30)]
        public string GroupPolicyNumber { get; set; }

        [MaxLength(128)]
        public string GroupPolicyName { get; set; }

        public int? NoOfEmployee { get; set; }

        public int? PolicyTotalLive { get; set; }

        [MaxLength(128)]
        public string GroupSubsidiaryName { get; set; }

        [MaxLength(30)]
        public string GroupSubsidiaryNo { get; set; }

        public double? GroupEmployeeBasicSalary { get; set; }

        [MaxLength(10)]
        public string GroupEmployeeJobType { get; set; }

        [MaxLength(10)]
        public string GroupEmployeeJobCode { get; set; }

        public double? GroupEmployeeBasicSalaryRevise { get; set; }

        public double? GroupEmployeeBasicSalaryMultiplier { get; set; }

        [MaxLength(30)]
        public string CedingPlanCode2 { get; set; }

        [MaxLength(2)]
        public string DependantIndicator { get; set; }

        public int? GhsRoomBoard { get; set; }

        public double? PolicyAmountSubstandard { get; set; }

        public double? Layer1RiShare { get; set; }

        public int? Layer1InsuredAttainedAge { get; set; }

        public int? Layer1InsuredAttainedAge2nd { get; set; }

        public double? Layer1StandardPremium { get; set; }

        public double? Layer1SubstandardPremium { get; set; }

        public double? Layer1GrossPremium { get; set; }

        public double? Layer1StandardDiscount { get; set; }

        public double? Layer1SubstandardDiscount { get; set; }

        public double? Layer1TotalDiscount { get; set; }

        public double? Layer1NetPremium { get; set; }

        public double? Layer1GrossPremiumAlt { get; set; }

        public double? Layer1TotalDiscountAlt { get; set; }

        public double? Layer1NetPremiumAlt { get; set; }

        public string SpecialIndicator1 { get; set; }

        public string SpecialIndicator2 { get; set; }

        public string SpecialIndicator3 { get; set; }

        [MaxLength(1)]
        public string IndicatorJointLife { get; set; }

        public double? TaxAmount { get; set; }

        [MaxLength(3)]
        public string GstIndicator { get; set; }

        public double? GstGrossPremium { get; set; }

        public double? GstTotalDiscount { get; set; }

        public double? GstVitality { get; set; }

        public double? GstAmount { get; set; }

        [MaxLength(5)]
        public string Mfrs17BasicRider { get; set; }

        [MaxLength(50)]
        public string Mfrs17CellName { get; set; }

        [MaxLength(25)]
        public string Mfrs17TreatyCode { get; set; }

        [MaxLength(20)]
        public string LoaCode { get; set; }

        public double? CurrencyRate { get; set; }

        public double? NoClaimBonus { get; set; }

        public double? SurrenderValue { get; set; }

        public double? DatabaseCommision { get; set; }

        public double? GrossPremiumAlt { get; set; }

        public double? NetPremiumAlt { get; set; }

        public double? Layer1FlatExtraPremium { get; set; }

        public double? TransactionPremium { get; set; }

        public double? OriginalPremium { get; set; }

        public double? TransactionDiscount { get; set; }

        public double? OriginalDiscount { get; set; }

        public double? BrokerageFee { get; set; }

        public double? MaxUwRating { get; set; }

        public double? RetentionCap { get; set; }

        public double? AarCap { get; set; }

        public double? RiRate { get; set; }

        public double? RiRate2 { get; set; }

        public double? AnnuityFactor { get; set; }


        public double? SumAssuredOffered { get; set; }

        public double? UwRatingOffered { get; set; }

        public double? FlatExtraAmountOffered { get; set; }

        public double? FlatExtraDuration { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? EffectiveDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? OfferLetterSentDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? RiskPeriodStartDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? RiskPeriodEndDate { get; set; }

        public int? Mfrs17AnnualCohort { get; set; }

        public int? MaxExpiryAge { get; set; }

        public int? MinIssueAge { get; set; }

        public int? MaxIssueAge { get; set; }

        public double? MinAar { get; set; }

        public double? MaxAar { get; set; }

        public double? CorridorLimit { get; set; }

        public double? Abl { get; set; }

        public int? RatePerBasisUnit { get; set; }

        public double? RiDiscountRate { get; set; }

        public double? LargeSaDiscount { get; set; }

        public double? GroupSizeDiscount { get; set; }

        public int? EwarpNumber { get; set; }

        [MaxLength(10)]
        public string EwarpActionCode { get; set; }

        public double? RetentionShare { get; set; }

        public double? AarShare { get; set; }

        [MaxLength(1)]
        public string ProfitComm { get; set; }

        public double? TotalDirectRetroAar { get; set; }

        public double? TotalDirectRetroGrossPremium { get; set; }

        public double? TotalDirectRetroDiscount { get; set; }

        public double? TotalDirectRetroNetPremium { get; set; }

        public double? AarShare2 { get; set; }

        public double? AarCap2 { get; set; }

        public double? WakalahFeePercentage { get; set; }

        [MaxLength(20)]
        public string TreatyType { get; set; }

        public double? MaxApLoading { get; set; }

        public int? MlreInsuredAttainedAgeAtCurrentMonth { get; set; }

        public int? MlreInsuredAttainedAgeAtPreviousMonth { get; set; }

        public bool? InsuredAttainedAgeCheck { get; set; }

        public bool? MaxExpiryAgeCheck { get; set; }

        public int? MlrePolicyIssueAge { get; set; }

        public bool? PolicyIssueAgeCheck { get; set; }

        public bool? MinIssueAgeCheck { get; set; }

        public bool? MaxIssueAgeCheck { get; set; }

        public bool? MaxUwRatingCheck { get; set; }

        public bool? ApLoadingCheck { get; set; }

        public bool? EffectiveDateCheck { get; set; }

        public bool? MinAarCheck { get; set; }

        public bool? MaxAarCheck { get; set; }

        public bool? CorridorLimitCheck { get; set; }

        public bool? AblCheck { get; set; }

        public bool? RetentionCheck { get; set; }

        public bool? AarCheck { get; set; }

        public double? MlreStandardPremium { get; set; }

        public double? MlreSubstandardPremium { get; set; }

        public double? MlreFlatExtraPremium { get; set; }

        public double? MlreGrossPremium { get; set; }

        public double? MlreStandardDiscount { get; set; }

        public double? MlreSubstandardDiscount { get; set; }

        public double? MlreLargeSaDiscount { get; set; }

        public double? MlreGroupSizeDiscount { get; set; }

        public double? MlreVitalityDiscount { get; set; }

        public double? MlreTotalDiscount { get; set; }

        public double? MlreNetPremium { get; set; }

        public double? NetPremiumCheck { get; set; }

        public double? ServiceFeePercentage { get; set; }

        public double? ServiceFee { get; set; }

        public double? MlreBrokerageFee { get; set; }

        public double? MlreDatabaseCommission { get; set; }

        public bool? ValidityDayCheck { get; set; }

        public bool? SumAssuredOfferedCheck { get; set; }

        public bool? UwRatingCheck { get; set; }

        public bool? FlatExtraAmountCheck { get; set; }

        public bool? FlatExtraDurationCheck { get; set; }

        [MaxLength(128)]
        public string TreatyNumber { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime LastUpdatedDate { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

    }
}
