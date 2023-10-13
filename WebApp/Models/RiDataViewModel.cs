using BusinessObject.RiDatas;
using DataAccess.Entities.RiDatas;
using Shared;
using Shared.Forms.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class RiDataViewModel
    {
        public int Id { get; set; }

        public int? RiDataBatchId { get; set; }

        public RiDataBatchBo RiDataBatchBo { get; set; }

        public int? RiDataFileId { get; set; }

        public RiDataFileBo RiDataFileBo { get; set; }

        public int RecordType { get; set; }

        public int? OriginalEntryId { get; set; }

        public bool IgnoreFinalise { get; set; }

        [Display(Name = "Mapping Status")]
        public int MappingStatus { get; set; }

        [Display(Name = "Pre-Computation 1 Status")]
        public int PreComputation1Status { get; set; }

        [Display(Name = "Pre-Computation 2 Status")]
        public int PreComputation2Status { get; set; }

        [Display(Name = "Pre-Validation Status")]
        public int PreValidationStatus { get; set; }

        [Display(Name = "Post Computation Status")]
        public int PostComputationStatus { get; set; }

        [Display(Name = "Post Validation Status")]
        public int PostValidationStatus { get; set; }

        [Display(Name = "Finalise Status")]
        public int FinaliseStatus { get; set; }

        [Display(Name = "Process Warehouse Status")]
        public int ProcessWarehouseStatus { get; set; }

        public string Errors { get; set; }

        public string CustomField { get; set; }

        [Display(Name = "Treaty Code")]
        public string TreatyCode { get; set; }

        public string ReinsBasisCode { get; set; }

        public string FundsAccountingTypeCode { get; set; }

        public string PremiumFrequencyCode { get; set; }

        public int? ReportPeriodMonth { get; set; }

        public int? ReportPeriodYear { get; set; }

        public int? RiskPeriodMonth { get; set; }

        public int? RiskPeriodYear { get; set; }

        public string TransactionTypeCode { get; set; }

        [Display(Name = "Policy Number")]
        public string PolicyNumber { get; set; }

        [Display(Name = "Policy Issue Date")]
        public DateTime? IssueDatePol { get; set; }

        public DateTime? IssueDateBen { get; set; }

        [Display(Name = "Reinsurance Effective Date")]
        public DateTime? ReinsEffDatePol { get; set; }

        public DateTime? ReinsEffDateBen { get; set; }

        public string CedingPlanCode { get; set; }

        public string CedingBenefitTypeCode { get; set; }

        public string CedingBenefitRiskCode { get; set; }

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

        public string InsuredName { get; set; }

        public string InsuredGenderCode { get; set; }

        public string InsuredTobaccoUse { get; set; }

        public DateTime? InsuredDateOfBirth { get; set; }

        public string InsuredOccupationCode { get; set; }

        public string InsuredRegisterNo { get; set; }

        public int? InsuredAttainedAge { get; set; }

        public string InsuredNewIcNumber { get; set; }

        public string InsuredOldIcNumber { get; set; }

        public string InsuredName2nd { get; set; }

        public string InsuredGenderCode2nd { get; set; }

        public string InsuredTobaccoUse2nd { get; set; }

        public DateTime? InsuredDateOfBirth2nd { get; set; }

        public int? InsuredAttainedAge2nd { get; set; }

        public string InsuredNewIcNumber2nd { get; set; }

        public string InsuredOldIcNumber2nd { get; set; }

        public int? ReinsuranceIssueAge { get; set; }

        public int? ReinsuranceIssueAge2nd { get; set; }

        [Display(Name = "Policy Term")]
        public double? PolicyTerm { get; set; }

        [Display(Name = "Policy Expiry Date")]
        public DateTime? PolicyExpiryDate { get; set; }

        public double? DurationYear { get; set; }

        public double? DurationDay { get; set; }

        public double? DurationMonth { get; set; }

        public string PremiumCalType { get; set; }

        public double? CedantRiRate { get; set; }

        public string RateTable { get; set; }

        public int? AgeRatedUp { get; set; }

        public double? DiscountRate { get; set; }

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

        public DateTime? AdjBeginDate { get; set; }

        public DateTime? AdjEndDate { get; set; }

        public string PolicyNumberOld { get; set; }

        public string PolicyStatusCode { get; set; }

        public double? PolicyGrossPremium { get; set; }

        public double? PolicyStandardPremium { get; set; }

        public double? PolicySubstandardPremium { get; set; }

        public double? PolicyTermRemain { get; set; }

        public double? PolicyAmountDeath { get; set; }

        public double? PolicyReserve { get; set; }

        public string PolicyPaymentMethod { get; set; }

        public int? PolicyLifeNumber { get; set; }

        public string FundCode { get; set; }

        public string LineOfBusiness { get; set; }

        public double? ApLoading { get; set; }

        public double? LoanInterestRate { get; set; }

        public int? DefermentPeriod { get; set; }

        public int? RiderNumber { get; set; }

        public string CampaignCode { get; set; }

        public string Nationality { get; set; }

        public string TerritoryOfIssueCode { get; set; }

        public string CurrencyCode { get; set; }

        public string StaffPlanIndicator { get; set; }

        public string CedingTreatyCode { get; set; }

        public string CedingPlanCodeOld { get; set; }

        public string CedingBasicPlanCode { get; set; }

        public double? CedantSar { get; set; }

        public string CedantReinsurerCode { get; set; }

        public double? AmountCededB4MlreShare2 { get; set; }

        public string CessionCode { get; set; }

        public string CedantRemark { get; set; }

        public string GroupPolicyNumber { get; set; }

        public string GroupPolicyName { get; set; }

        public int? NoOfEmployee { get; set; }

        public int? PolicyTotalLive { get; set; }

        public string GroupSubsidiaryName { get; set; }

        public string GroupSubsidiaryNo { get; set; }

        public double? GroupEmployeeBasicSalary { get; set; }

        public string GroupEmployeeJobType { get; set; }

        public string GroupEmployeeJobCode { get; set; }

        public double? GroupEmployeeBasicSalaryRevise { get; set; }

        public double? GroupEmployeeBasicSalaryMultiplier { get; set; }

        public string CedingPlanCode2 { get; set; }

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

        public string IndicatorJointLife { get; set; }

        public double? TaxAmount { get; set; }

        public string GstIndicator { get; set; }

        public double? GstGrossPremium { get; set; }

        public double? GstTotalDiscount { get; set; }

        public double? GstVitality { get; set; }

        public double? GstAmount { get; set; }

        [Display(Name = "MFRS17 Basis Rider")]
        public string Mfrs17BasicRider { get; set; }

        [Display(Name = "MFRS17 Cell Name")]
        public string Mfrs17CellName { get; set; }

        [Display(Name = "MFRS17 Contract Code")]
        public string Mfrs17TreatyCode { get; set; }

        public string LoaCode { get; set; }

        public DateTime? TempD1 { get; set; }

        public DateTime? TempD2 { get; set; }

        public DateTime? TempD3 { get; set; }

        public DateTime? TempD4 { get; set; }

        public DateTime? TempD5 { get; set; }

        public string TempS1 { get; set; }

        public string TempS2 { get; set; }

        public string TempS3 { get; set; }

        public string TempS4 { get; set; }

        public string TempS5 { get; set; }

        public int? TempI1 { get; set; }

        public int? TempI2 { get; set; }

        public int? TempI3 { get; set; }

        public int? TempI4 { get; set; }

        public int? TempI5 { get; set; }

        public double? TempA1 { get; set; }

        public double? TempA2 { get; set; }

        public double? TempA3 { get; set; }

        public double? TempA4 { get; set; }

        public double? TempA5 { get; set; }

        public double? TempA6 { get; set; }

        public double? TempA7 { get; set; }

        public double? TempA8 { get; set; }

        public string IssueDatePolStr { get; set; }

        public string IssueDateBenStr { get; set; }

        public string ReinsEffDatePolStr { get; set; }

        public string ReinsEffDateBenStr { get; set; }

        public string InsuredDateOfBirthStr { get; set; }

        public string InsuredDateOfBirth2ndStr { get; set; }

        public string PolicyExpiryDateStr { get; set; }

        public string AdjBeginDateStr { get; set; }

        public string AdjEndDateStr { get; set; }

        public string TempD1Str { get; set; }

        public string TempD2Str { get; set; }

        public string TempD3Str { get; set; }

        public string TempD4Str { get; set; }

        public string TempD5Str { get; set; }

        [ValidateDouble]
        public string OriSumAssuredStr { get; set; }

        [ValidateDouble]
        public string CurrSumAssuredStr { get; set; }

        [ValidateDouble]
        public string AmountCededB4MlreShareStr { get; set; }

        [ValidateDouble]
        public string RetentionAmountStr { get; set; }

        [ValidateDouble]
        public string AarOriStr { get; set; }

        [ValidateDouble]
        public string AarStr { get; set; }

        [ValidateDouble]
        public string AarSpecial1Str { get; set; }

        [ValidateDouble]
        public string AarSpecial2Str { get; set; }

        [ValidateDouble]
        public string AarSpecial3Str { get; set; }

        [ValidateDouble]
        public string DurationYearStr { get; set; }

        [ValidateDouble]
        public string CedantRiRateStr { get; set; }

        [ValidateDouble]
        public string DiscountRateStr { get; set; }

        [ValidateDouble]
        public string UnderwriterRatingStr { get; set; }

        [ValidateDouble]
        public string UnderwriterRatingUnitStr { get; set; }

        [ValidateDouble]
        public string UnderwriterRating2Str { get; set; }

        [ValidateDouble]
        public string UnderwriterRatingUnit2Str { get; set; }

        [ValidateDouble]
        public string UnderwriterRating3Str { get; set; }

        [ValidateDouble]
        public string UnderwriterRatingUnit3Str { get; set; }

        [ValidateDouble]
        public string FlatExtraAmountStr { get; set; }

        [ValidateDouble]
        public string FlatExtraUnitStr { get; set; }

        [ValidateDouble]
        public string FlatExtraAmount2Str { get; set; }

        [ValidateDouble]
        public string StandardPremiumStr { get; set; }

        [ValidateDouble]
        public string SubstandardPremiumStr { get; set; }

        [ValidateDouble]
        public string FlatExtraPremiumStr { get; set; }

        [ValidateDouble]
        public string GrossPremiumStr { get; set; }

        [ValidateDouble]
        public string StandardDiscountStr { get; set; }

        [ValidateDouble]
        public string SubstandardDiscountStr { get; set; }

        [ValidateDouble]
        public string VitalityDiscountStr { get; set; }

        [ValidateDouble]
        public string TotalDiscountStr { get; set; }

        [ValidateDouble]
        public string NetPremiumStr { get; set; }

        [ValidateDouble]
        public string AnnualRiPremStr { get; set; }

        [ValidateDouble]
        public string PolicyGrossPremiumStr { get; set; }

        [ValidateDouble]
        public string PolicyStandardPremiumStr { get; set; }

        [ValidateDouble]
        public string PolicySubstandardPremiumStr { get; set; }

        [ValidateDouble]
        public string PolicyAmountDeathStr { get; set; }

        [ValidateDouble]
        public string PolicyReserveStr { get; set; }

        [ValidateDouble]
        public string ApLoadingStr { get; set; }

        [ValidateDouble]
        public string LoanInterestRateStr { get; set; }

        [ValidateDouble]
        public string CedantSarStr { get; set; }

        [ValidateDouble]
        public string AmountCededB4MlreShare2Str { get; set; }

        [ValidateDouble]
        public string GroupEmployeeBasicSalaryStr { get; set; }

        [ValidateDouble]
        public string GroupEmployeeBasicSalaryReviseStr { get; set; }

        [ValidateDouble]
        public string GroupEmployeeBasicSalaryMultiplierStr { get; set; }

        [ValidateDouble]
        public string PolicyAmountSubstandardStr { get; set; }

        [ValidateDouble]
        public string Layer1RiShareStr { get; set; }

        [ValidateDouble]
        public string Layer1StandardPremiumStr { get; set; }

        [ValidateDouble]
        public string Layer1SubstandardPremiumStr { get; set; }

        [ValidateDouble]
        public string Layer1GrossPremiumStr { get; set; }

        [ValidateDouble]
        public string Layer1StandardDiscountStr { get; set; }

        [ValidateDouble]
        public string Layer1SubstandardDiscountStr { get; set; }

        [ValidateDouble]
        public string Layer1TotalDiscountStr { get; set; }

        [ValidateDouble]
        public string Layer1NetPremiumStr { get; set; }

        [ValidateDouble]
        public string Layer1GrossPremiumAltStr { get; set; }

        [ValidateDouble]
        public string Layer1TotalDiscountAltStr { get; set; }

        [ValidateDouble]
        public string Layer1NetPremiumAltStr { get; set; }

        [ValidateDouble]
        public string TaxAmountStr { get; set; }

        [ValidateDouble]
        public string GstGrossPremiumStr { get; set; }

        [ValidateDouble]
        public string GstTotalDiscountStr { get; set; }

        [ValidateDouble]
        public string GstVitalityStr { get; set; }

        [ValidateDouble]
        public string GstAmountStr { get; set; }

        [ValidateDouble]
        public string TempA1Str { get; set; }

        [ValidateDouble]
        public string TempA2Str { get; set; }

        [ValidateDouble]
        public string TempA3Str { get; set; }

        [ValidateDouble]
        public string TempA4Str { get; set; }

        [ValidateDouble]
        public string TempA5Str { get; set; }

        [ValidateDouble]
        public string TempA6Str { get; set; }

        [ValidateDouble]
        public string TempA7Str { get; set; }

        [ValidateDouble]
        public string TempA8Str { get; set; }

        [ValidateDouble]
        public string DurationDayStr { get; set; }

        [ValidateDouble]
        public string DurationMonthStr { get; set; }

        [ValidateDouble]
        public string RiCovPeriodStr { get; set; }

        public RiDataBatch RiDataBatch { get; set; }

        public RiDataFile RiDataFile { get; set; }

        // phase 2
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

        public int? FlatExtraDuration { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? OfferLetterSentDate { get; set; }

        public DateTime? RiskPeriodStartDate { get; set; }

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

        public string EwarpActionCode { get; set; }

        public double? RetentionShare { get; set; }

        public double? AarShare { get; set; }

        public string ProfitComm { get; set; }

        public double? TotalDirectRetroAar { get; set; }

        public double? TotalDirectRetroGrossPremium { get; set; }

        public double? TotalDirectRetroDiscount { get; set; }

        public double? TotalDirectRetroNetPremium { get; set; }

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

        public double? AarShare2 { get; set; }

        public double? AarCap2 { get; set; }

        public double? WakalahFeePercentage { get; set; }

        public string TreatyNumber { get; set; }

        [ValidateDouble]
        public string CurrencyRateStr { get; set; }

        [ValidateDouble]
        public string NoClaimBonusStr { get; set; }

        [ValidateDouble]
        public string SurrenderValueStr { get; set; }

        [ValidateDouble]
        public string DatabaseCommisionStr { get; set; }

        [ValidateDouble]
        public string GrossPremiumAltStr { get; set; }

        [ValidateDouble]
        public string NetPremiumAltStr { get; set; }

        [ValidateDouble]
        public string Layer1FlatExtraPremiumStr { get; set; }

        [ValidateDouble]
        public string TransactionPremiumStr { get; set; }

        [ValidateDouble]
        public string OriginalPremiumStr { get; set; }

        [ValidateDouble]
        public string TransactionDiscountStr { get; set; }

        [ValidateDouble]
        public string OriginalDiscountStr { get; set; }

        [ValidateDouble]
        public string BrokerageFeeStr { get; set; }

        [ValidateDouble]
        public string MaxUwRatingStr { get; set; }

        [ValidateDouble]
        public string RetentionCapStr { get; set; }

        [ValidateDouble]
        public string AarCapStr { get; set; }

        [ValidateDouble]
        public string RiRateStr { get; set; }

        [ValidateDouble]
        public string RiRate2Str { get; set; }

        [ValidateDouble]
        public string AnnuityFactorStr { get; set; }

        [ValidateDouble]
        public string SumAssuredOfferedStr { get; set; }

        [ValidateDouble]
        public string UwRatingOfferedStr { get; set; }

        [ValidateDouble]
        public string FlatExtraAmountOfferedStr { get; set; }

        [ValidateDouble]
        public string RetentionShareStr { get; set; }

        [ValidateDouble]
        public string AarShareStr { get; set; }

        [ValidateDouble]
        public string TotalDirectRetroAarStr { get; set; }

        [ValidateDouble]
        public string TotalDirectRetroGrossPremiumStr { get; set; }

        [ValidateDouble]
        public string TotalDirectRetroDiscountStr { get; set; }

        [ValidateDouble]
        public string TotalDirectRetroNetPremiumStr { get; set; }

        [ValidateDouble]
        public string MinAarStr { get; set; }

        [ValidateDouble]
        public string MaxAarStr { get; set; }

        [ValidateDouble]
        public string CorridorLimitStr { get; set; }

        [ValidateDouble]
        public string AblStr { get; set; }

        [ValidateDouble]
        public string RiDiscountRateStr { get; set; }

        [ValidateDouble]
        public string LargeSaDiscountStr { get; set; }

        [ValidateDouble]
        public string GroupSizeDiscountStr { get; set; }

        [ValidateDouble]
        public string MaxApLoadingStr { get; set; }

        [ValidateDouble]
        public string MlreStandardPremiumStr { get; set; }

        [ValidateDouble]
        public string MlreSubstandardPremiumStr { get; set; }

        [ValidateDouble]
        public string MlreFlatExtraPremiumStr { get; set; }

        [ValidateDouble]
        public string MlreGrossPremiumStr { get; set; }

        [ValidateDouble]
        public string MlreStandardDiscountStr { get; set; }

        [ValidateDouble]
        public string MlreSubstandardDiscountStr { get; set; }

        [ValidateDouble]
        public string MlreLargeSaDiscountStr { get; set; }

        [ValidateDouble]
        public string MlreGroupSizeDiscountStr { get; set; }

        [ValidateDouble]
        public string MlreVitalityDiscountStr { get; set; }

        [ValidateDouble]
        public string MlreTotalDiscountStr { get; set; }

        [ValidateDouble]
        public string MlreNetPremiumStr { get; set; }

        [ValidateDouble]
        public string NetPremiumCheckStr { get; set; }

        [ValidateDouble]
        public string ServiceFeePercentageStr { get; set; }

        [ValidateDouble]
        public string ServiceFeeStr { get; set; }

        [ValidateDouble]
        public string MlreBrokerageFeeStr { get; set; }

        [ValidateDouble]
        public string MlreDatabaseCommissionStr { get; set; }

        [ValidateDouble]
        public string AarShare2Str { get; set; }

        [ValidateDouble]
        public string AarCap2Str { get; set; }

        [ValidateDouble]
        public string WakalahFeePercentageStr { get; set; }

        public string EffectiveDateStr { get; set; }

        public string OfferLetterSentDateStr { get; set; }

        public string RiskPeriodStartDateStr { get; set; }

        public string RiskPeriodEndDateStr { get; set; }

        [ValidateDouble]
        public string PolicyTermStr { get; set; }

        [ValidateDouble]
        public string PolicyTermRemainStr { get; set; }

        // Direct Retro
        public string RetroParty1 { get; set; }

        public string RetroParty2 { get; set; }

        public string RetroParty3 { get; set; }

        public double? RetroShare1 { get; set; }

        public double? RetroShare2 { get; set; }

        public double? RetroShare3 { get; set; }

        public double? RetroPremiumSpread1 { get; set; }

        public double? RetroPremiumSpread2 { get; set; }

        public double? RetroPremiumSpread3 { get; set; }

        public double? RetroAar1 { get; set; }

        public double? RetroAar2 { get; set; }

        public double? RetroAar3 { get; set; }

        public double? RetroReinsurancePremium1 { get; set; }

        public double? RetroReinsurancePremium2 { get; set; }

        public double? RetroReinsurancePremium3 { get; set; }

        public double? RetroDiscount1 { get; set; }

        public double? RetroDiscount2 { get; set; }

        public double? RetroDiscount3 { get; set; }

        public double? RetroNetPremium1 { get; set; }

        public double? RetroNetPremium2 { get; set; }

        public double? RetroNetPremium3 { get; set; }

        public double? RetroNoClaimBonus1 { get; set; }

        public double? RetroNoClaimBonus2 { get; set; }

        public double? RetroNoClaimBonus3 { get; set; }

        public double? RetroDatabaseCommission1 { get; set; }

        public double? RetroDatabaseCommission2 { get; set; }

        public double? RetroDatabaseCommission3 { get; set; }

        // Direct Retro Double String

        public string RetroShare1Str { get; set; }

        public string RetroShare2Str { get; set; }

        public string RetroShare3Str { get; set; }

        public string RetroPremiumSpread1Str { get; set; }

        public string RetroPremiumSpread2Str { get; set; }

        public string RetroPremiumSpread3Str { get; set; }

        public string RetroAar1Str { get; set; }

        public string RetroAar2Str { get; set; }

        public string RetroAar3Str { get; set; }

        public string RetroReinsurancePremium1Str { get; set; }

        public string RetroReinsurancePremium2Str { get; set; }

        public string RetroReinsurancePremium3Str { get; set; }

        public string RetroDiscount1Str { get; set; }

        public string RetroDiscount2Str { get; set; }

        public string RetroDiscount3Str { get; set; }

        public string RetroNetPremium1Str { get; set; }

        public string RetroNetPremium2Str { get; set; }

        public string RetroNetPremium3Str { get; set; }

        public string RetroNoClaimBonus1Str { get; set; }

        public string RetroNoClaimBonus2Str { get; set; }

        public string RetroNoClaimBonus3Str { get; set; }

        public string RetroDatabaseCommission1Str { get; set; }

        public string RetroDatabaseCommission2Str { get; set; }

        public string RetroDatabaseCommission3Str { get; set; }

        public int ConflictType { get; set; }

        public double? TotalDirectRetroNoClaimBonus { get; set; }

        public double? TotalDirectRetroDatabaseCommission { get; set; }

        public string TotalDirectRetroNoClaimBonusStr { get; set; }

        public string TotalDirectRetroDatabaseCommissionStr { get; set; }

        public bool FinishLoading { get; set; } = false;

        public RiDataViewModel() { }

        public RiDataViewModel(RiDataBo riDataBo)
        {
            Set(riDataBo);

            FinishLoading = true;
        }

        public void Set(RiDataBo riDataBo)
        {
            if (riDataBo != null)
            {
                Id = riDataBo.Id;
                RiDataBatchId = riDataBo.RiDataBatchId;
                RiDataBatchBo = riDataBo.RiDataBatchBo;
                RiDataFileId = riDataBo.RiDataFileId;
                RiDataFileBo = riDataBo.RiDataFileBo;
                RecordType = riDataBo.RecordType;
                OriginalEntryId = riDataBo.OriginalEntryId;
                IgnoreFinalise = riDataBo.IgnoreFinalise;
                MappingStatus = riDataBo.MappingStatus;
                PreComputation1Status = riDataBo.PreComputation1Status;
                PreComputation2Status = riDataBo.PreComputation2Status;
                PreValidationStatus = riDataBo.PreValidationStatus;
                PostComputationStatus = riDataBo.PostComputationStatus;
                PostValidationStatus = riDataBo.PostValidationStatus;
                FinaliseStatus = riDataBo.FinaliseStatus;
                ProcessWarehouseStatus = riDataBo.ProcessWarehouseStatus;
                Errors = riDataBo.Errors;
                CustomField = riDataBo.CustomField;
                TreatyCode = riDataBo.TreatyCode;
                ReinsBasisCode = riDataBo.ReinsBasisCode;
                FundsAccountingTypeCode = riDataBo.FundsAccountingTypeCode;
                PremiumFrequencyCode = riDataBo.PremiumFrequencyCode;
                ReportPeriodMonth = riDataBo.ReportPeriodMonth;
                ReportPeriodYear = riDataBo.ReportPeriodYear;
                RiskPeriodMonth = riDataBo.RiskPeriodMonth;
                RiskPeriodYear = riDataBo.RiskPeriodYear;
                TransactionTypeCode = riDataBo.TransactionTypeCode;
                PolicyNumber = riDataBo.PolicyNumber;
                IssueDatePol = riDataBo.IssueDatePol;
                IssueDateBen = riDataBo.IssueDateBen;
                ReinsEffDatePol = riDataBo.ReinsEffDatePol;
                ReinsEffDateBen = riDataBo.ReinsEffDateBen;
                CedingPlanCode = riDataBo.CedingPlanCode;
                CedingBenefitTypeCode = riDataBo.CedingBenefitTypeCode;
                CedingBenefitRiskCode = riDataBo.CedingBenefitRiskCode;
                MlreBenefitCode = riDataBo.MlreBenefitCode;
                OriSumAssured = riDataBo.OriSumAssured;
                CurrSumAssured = riDataBo.CurrSumAssured;
                AmountCededB4MlreShare = riDataBo.AmountCededB4MlreShare;
                RetentionAmount = riDataBo.RetentionAmount;
                AarOri = riDataBo.AarOri;
                Aar = riDataBo.Aar;
                AarSpecial1 = riDataBo.AarSpecial1;
                AarSpecial2 = riDataBo.AarSpecial2;
                AarSpecial3 = riDataBo.AarSpecial3;
                InsuredName = riDataBo.InsuredName;
                InsuredGenderCode = riDataBo.InsuredGenderCode;
                InsuredTobaccoUse = riDataBo.InsuredTobaccoUse;
                InsuredDateOfBirth = riDataBo.InsuredDateOfBirth;
                InsuredOccupationCode = riDataBo.InsuredOccupationCode;
                InsuredRegisterNo = riDataBo.InsuredRegisterNo;
                InsuredAttainedAge = riDataBo.InsuredAttainedAge;
                InsuredNewIcNumber = riDataBo.InsuredNewIcNumber;
                InsuredOldIcNumber = riDataBo.InsuredOldIcNumber;
                InsuredName2nd = riDataBo.InsuredName2nd;
                InsuredGenderCode2nd = riDataBo.InsuredGenderCode2nd;
                InsuredTobaccoUse2nd = riDataBo.InsuredTobaccoUse2nd;
                InsuredDateOfBirth2nd = riDataBo.InsuredDateOfBirth2nd;
                InsuredAttainedAge2nd = riDataBo.InsuredAttainedAge2nd;
                InsuredNewIcNumber2nd = riDataBo.InsuredNewIcNumber2nd;
                InsuredOldIcNumber2nd = riDataBo.InsuredOldIcNumber2nd;
                ReinsuranceIssueAge = riDataBo.ReinsuranceIssueAge;
                ReinsuranceIssueAge2nd = riDataBo.ReinsuranceIssueAge2nd;
                PolicyTerm = riDataBo.PolicyTerm;
                PolicyExpiryDate = riDataBo.PolicyExpiryDate;
                DurationYear = riDataBo.DurationYear;
                DurationDay = riDataBo.DurationDay;
                DurationMonth = riDataBo.DurationMonth;
                PremiumCalType = riDataBo.PremiumCalType;
                CedantRiRate = riDataBo.CedantRiRate;
                RateTable = riDataBo.RateTable;
                AgeRatedUp = riDataBo.AgeRatedUp;
                DiscountRate = riDataBo.DiscountRate;
                LoadingType = riDataBo.LoadingType;
                UnderwriterRating = riDataBo.UnderwriterRating;
                UnderwriterRatingUnit = riDataBo.UnderwriterRatingUnit;
                UnderwriterRatingTerm = riDataBo.UnderwriterRatingTerm;
                UnderwriterRating2 = riDataBo.UnderwriterRating2;
                UnderwriterRatingUnit2 = riDataBo.UnderwriterRatingUnit2;
                UnderwriterRatingTerm2 = riDataBo.UnderwriterRatingTerm2;
                UnderwriterRating3 = riDataBo.UnderwriterRating3;
                UnderwriterRatingUnit3 = riDataBo.UnderwriterRatingUnit3;
                UnderwriterRatingTerm3 = riDataBo.UnderwriterRatingTerm3;
                FlatExtraAmount = riDataBo.FlatExtraAmount;
                FlatExtraUnit = riDataBo.FlatExtraUnit;
                FlatExtraTerm = riDataBo.FlatExtraTerm;
                FlatExtraAmount2 = riDataBo.FlatExtraAmount2;
                FlatExtraTerm2 = riDataBo.FlatExtraTerm2;
                StandardPremium = riDataBo.StandardPremium;
                SubstandardPremium = riDataBo.SubstandardPremium;
                FlatExtraPremium = riDataBo.FlatExtraPremium;
                GrossPremium = riDataBo.GrossPremium;
                StandardDiscount = riDataBo.StandardDiscount;
                SubstandardDiscount = riDataBo.SubstandardDiscount;
                VitalityDiscount = riDataBo.VitalityDiscount;
                TotalDiscount = riDataBo.TotalDiscount;
                NetPremium = riDataBo.NetPremium;
                AnnualRiPrem = riDataBo.AnnualRiPrem;
                RiCovPeriod = riDataBo.RiCovPeriod;
                AdjBeginDate = riDataBo.AdjBeginDate;
                AdjEndDate = riDataBo.AdjEndDate;
                PolicyNumberOld = riDataBo.PolicyNumberOld;
                PolicyStatusCode = riDataBo.PolicyStatusCode;
                PolicyGrossPremium = riDataBo.PolicyGrossPremium;
                PolicyStandardPremium = riDataBo.PolicyStandardPremium;
                PolicySubstandardPremium = riDataBo.PolicySubstandardPremium;
                PolicyTermRemain = riDataBo.PolicyTermRemain;
                PolicyAmountDeath = riDataBo.PolicyAmountDeath;
                PolicyReserve = riDataBo.PolicyReserve;
                PolicyPaymentMethod = riDataBo.PolicyPaymentMethod;
                PolicyLifeNumber = riDataBo.PolicyLifeNumber;
                FundCode = riDataBo.FundCode;
                LineOfBusiness = riDataBo.LineOfBusiness;
                ApLoading = riDataBo.ApLoading;
                LoanInterestRate = riDataBo.LoanInterestRate;
                DefermentPeriod = riDataBo.DefermentPeriod;
                RiderNumber = riDataBo.RiderNumber;
                CampaignCode = riDataBo.CampaignCode;
                Nationality = riDataBo.Nationality;
                TerritoryOfIssueCode = riDataBo.TerritoryOfIssueCode;
                CurrencyCode = riDataBo.CurrencyCode;
                StaffPlanIndicator = riDataBo.StaffPlanIndicator;
                CedingTreatyCode = riDataBo.CedingTreatyCode;
                CedingPlanCodeOld = riDataBo.CedingPlanCodeOld;
                CedingBasicPlanCode = riDataBo.CedingBasicPlanCode;
                CedantSar = riDataBo.CedantSar;
                CedantReinsurerCode = riDataBo.CedantReinsurerCode;
                AmountCededB4MlreShare2 = riDataBo.AmountCededB4MlreShare2;
                CessionCode = riDataBo.CessionCode;
                CedantRemark = riDataBo.CedantRemark;
                GroupPolicyNumber = riDataBo.GroupPolicyNumber;
                GroupPolicyName = riDataBo.GroupPolicyName;
                NoOfEmployee = riDataBo.NoOfEmployee;
                PolicyTotalLive = riDataBo.PolicyTotalLive;
                GroupSubsidiaryName = riDataBo.GroupSubsidiaryName;
                GroupSubsidiaryNo = riDataBo.GroupSubsidiaryNo;
                GroupEmployeeBasicSalary = riDataBo.GroupEmployeeBasicSalary;
                GroupEmployeeJobType = riDataBo.GroupEmployeeJobType;
                GroupEmployeeJobCode = riDataBo.GroupEmployeeJobCode;
                GroupEmployeeBasicSalaryRevise = riDataBo.GroupEmployeeBasicSalaryRevise;
                GroupEmployeeBasicSalaryMultiplier = riDataBo.GroupEmployeeBasicSalaryMultiplier;
                CedingPlanCode2 = riDataBo.CedingPlanCode2;
                DependantIndicator = riDataBo.DependantIndicator;
                GhsRoomBoard = riDataBo.GhsRoomBoard;
                PolicyAmountSubstandard = riDataBo.PolicyAmountSubstandard;
                Layer1RiShare = riDataBo.Layer1RiShare;
                Layer1InsuredAttainedAge = riDataBo.Layer1InsuredAttainedAge;
                Layer1InsuredAttainedAge2nd = riDataBo.Layer1InsuredAttainedAge2nd;
                Layer1StandardPremium = riDataBo.Layer1StandardPremium;
                Layer1SubstandardPremium = riDataBo.Layer1SubstandardPremium;
                Layer1GrossPremium = riDataBo.Layer1GrossPremium;
                Layer1StandardDiscount = riDataBo.Layer1StandardDiscount;
                Layer1SubstandardDiscount = riDataBo.Layer1SubstandardDiscount;
                Layer1TotalDiscount = riDataBo.Layer1TotalDiscount;
                Layer1NetPremium = riDataBo.Layer1NetPremium;
                Layer1GrossPremiumAlt = riDataBo.Layer1GrossPremiumAlt;
                Layer1TotalDiscountAlt = riDataBo.Layer1TotalDiscountAlt;
                Layer1NetPremiumAlt = riDataBo.Layer1NetPremiumAlt;
                SpecialIndicator1 = riDataBo.SpecialIndicator1;
                SpecialIndicator2 = riDataBo.SpecialIndicator2;
                SpecialIndicator3 = riDataBo.SpecialIndicator3;
                IndicatorJointLife = riDataBo.IndicatorJointLife;
                TaxAmount = riDataBo.TaxAmount;
                GstIndicator = riDataBo.GstIndicator;
                GstGrossPremium = riDataBo.GstGrossPremium;
                GstTotalDiscount = riDataBo.GstTotalDiscount;
                GstVitality = riDataBo.GstVitality;
                GstAmount = riDataBo.GstAmount;
                Mfrs17BasicRider = riDataBo.Mfrs17BasicRider;
                Mfrs17CellName = riDataBo.Mfrs17CellName;
                Mfrs17TreatyCode = riDataBo.Mfrs17TreatyCode;
                LoaCode = riDataBo.LoaCode;
                TempD1 = riDataBo.TempD1;
                TempD2 = riDataBo.TempD2;
                TempD3 = riDataBo.TempD3;
                TempD4 = riDataBo.TempD4;
                TempD5 = riDataBo.TempD5;
                TempS1 = riDataBo.TempS1;
                TempS2 = riDataBo.TempS2;
                TempS3 = riDataBo.TempS3;
                TempS4 = riDataBo.TempS4;
                TempS5 = riDataBo.TempS5;
                TempI1 = riDataBo.TempI1;
                TempI2 = riDataBo.TempI2;
                TempI3 = riDataBo.TempI3;
                TempI4 = riDataBo.TempI4;
                TempI5 = riDataBo.TempI5;
                TempA1 = riDataBo.TempA1;
                TempA2 = riDataBo.TempA2;
                TempA3 = riDataBo.TempA3;
                TempA4 = riDataBo.TempA4;
                TempA5 = riDataBo.TempA5;
                TempA6 = riDataBo.TempA6;
                TempA7 = riDataBo.TempA7;
                TempA8 = riDataBo.TempA8;

                // Phase 2
                UwRatingOffered = riDataBo.UwRatingOffered;
                FlatExtraAmountOffered = riDataBo.FlatExtraAmountOffered;
                FlatExtraDuration = riDataBo.FlatExtraDuration;
                EffectiveDate = riDataBo.EffectiveDate;
                OfferLetterSentDate = riDataBo.OfferLetterSentDate;
                RiskPeriodStartDate = riDataBo.RiskPeriodStartDate;
                RiskPeriodEndDate = riDataBo.RiskPeriodEndDate;
                Mfrs17AnnualCohort = riDataBo.Mfrs17AnnualCohort;
                MaxExpiryAge = riDataBo.MaxExpiryAge;
                MinIssueAge = riDataBo.MinIssueAge;
                MaxIssueAge = riDataBo.MaxIssueAge;
                MinAar = riDataBo.MinAar;
                MaxAar = riDataBo.MaxAar;
                CorridorLimit = riDataBo.CorridorLimit;
                Abl = riDataBo.Abl;
                RatePerBasisUnit = riDataBo.RatePerBasisUnit;
                RiDiscountRate = riDataBo.RiDiscountRate;
                LargeSaDiscount = riDataBo.LargeSaDiscount;
                GroupSizeDiscount = riDataBo.GroupSizeDiscount;
                EwarpNumber = riDataBo.EwarpNumber;
                EwarpActionCode = riDataBo.EwarpActionCode;
                RetentionShare = riDataBo.RetentionShare;
                AarShare = riDataBo.AarShare;
                ProfitComm = riDataBo.ProfitComm;
                TotalDirectRetroAar = riDataBo.TotalDirectRetroAar;
                TotalDirectRetroGrossPremium = riDataBo.TotalDirectRetroGrossPremium;
                TotalDirectRetroDiscount = riDataBo.TotalDirectRetroDiscount;
                TotalDirectRetroNetPremium = riDataBo.TotalDirectRetroNetPremium;
                TotalDirectRetroNoClaimBonus = riDataBo.TotalDirectRetroNoClaimBonus;
                TotalDirectRetroDatabaseCommission = riDataBo.TotalDirectRetroDatabaseCommission;
                TreatyType = riDataBo.TreatyType;
                MaxApLoading = riDataBo.MaxApLoading;
                MlreInsuredAttainedAgeAtCurrentMonth = riDataBo.MlreInsuredAttainedAgeAtCurrentMonth;
                MlreInsuredAttainedAgeAtPreviousMonth = riDataBo.MlreInsuredAttainedAgeAtPreviousMonth;
                InsuredAttainedAgeCheck = riDataBo.InsuredAttainedAgeCheck;
                MaxExpiryAgeCheck = riDataBo.MaxExpiryAgeCheck;
                MlrePolicyIssueAge = riDataBo.MlrePolicyIssueAge;
                PolicyIssueAgeCheck = riDataBo.PolicyIssueAgeCheck;
                MinIssueAgeCheck = riDataBo.MinIssueAgeCheck;
                MaxIssueAgeCheck = riDataBo.MaxIssueAgeCheck;
                MaxUwRatingCheck = riDataBo.MaxUwRatingCheck;
                ApLoadingCheck = riDataBo.ApLoadingCheck;
                EffectiveDateCheck = riDataBo.EffectiveDateCheck;
                MinAarCheck = riDataBo.MinAarCheck;
                MaxAarCheck = riDataBo.MaxAarCheck;
                CorridorLimitCheck = riDataBo.CorridorLimitCheck;
                AblCheck = riDataBo.AblCheck;
                RetentionCheck = riDataBo.RetentionCheck;
                AarCheck = riDataBo.AarCheck;
                MlreStandardPremium = riDataBo.MlreStandardPremium;
                MlreSubstandardPremium = riDataBo.MlreSubstandardPremium;
                MlreFlatExtraPremium = riDataBo.MlreFlatExtraPremium;
                MlreGrossPremium = riDataBo.MlreGrossPremium;
                MlreStandardDiscount = riDataBo.MlreStandardDiscount;
                MlreSubstandardDiscount = riDataBo.MlreSubstandardDiscount;
                MlreLargeSaDiscount = riDataBo.MlreLargeSaDiscount;
                MlreGroupSizeDiscount = riDataBo.MlreGroupSizeDiscount;
                MlreVitalityDiscount = riDataBo.MlreVitalityDiscount;
                MlreTotalDiscount = riDataBo.MlreTotalDiscount;
                MlreNetPremium = riDataBo.MlreNetPremium;
                NetPremiumCheck = riDataBo.NetPremiumCheck;
                ServiceFeePercentage = riDataBo.ServiceFeePercentage;
                ServiceFee = riDataBo.ServiceFee;
                MlreBrokerageFee = riDataBo.MlreBrokerageFee;
                MlreDatabaseCommission = riDataBo.MlreDatabaseCommission;
                ValidityDayCheck = riDataBo.ValidityDayCheck;
                SumAssuredOfferedCheck = riDataBo.SumAssuredOfferedCheck;
                UwRatingCheck = riDataBo.UwRatingCheck;
                FlatExtraAmountCheck = riDataBo.FlatExtraAmountCheck;
                FlatExtraDurationCheck = riDataBo.FlatExtraDurationCheck;
                AarShare2 = riDataBo.AarShare2;
                AarCap2 = riDataBo.AarCap2;
                WakalahFeePercentage = riDataBo.WakalahFeePercentage;
                TreatyNumber = riDataBo.TreatyNumber;
                ConflictType = riDataBo.ConflictType;

                // Direct Retro
                RetroParty1 = riDataBo.RetroParty1;
                RetroParty2 = riDataBo.RetroParty2;
                RetroParty3 = riDataBo.RetroParty3;
                RetroShare1 = riDataBo.RetroShare1;
                RetroShare2 = riDataBo.RetroShare2;
                RetroShare3 = riDataBo.RetroShare3;
                RetroPremiumSpread1 = riDataBo.RetroPremiumSpread1;
                RetroPremiumSpread2 = riDataBo.RetroPremiumSpread2;
                RetroPremiumSpread3 = riDataBo.RetroPremiumSpread3;
                RetroAar1 = riDataBo.RetroAar1;
                RetroAar2 = riDataBo.RetroAar2;
                RetroAar3 = riDataBo.RetroAar3;
                RetroReinsurancePremium1 = riDataBo.RetroReinsurancePremium1;
                RetroReinsurancePremium2 = riDataBo.RetroReinsurancePremium2;
                RetroReinsurancePremium3 = riDataBo.RetroReinsurancePremium3;
                RetroDiscount1 = riDataBo.RetroDiscount1;
                RetroDiscount2 = riDataBo.RetroDiscount2;
                RetroDiscount3 = riDataBo.RetroDiscount3;
                RetroNetPremium1 = riDataBo.RetroNetPremium1;
                RetroNetPremium2 = riDataBo.RetroNetPremium2;
                RetroNetPremium3 = riDataBo.RetroNetPremium3;
                RetroNoClaimBonus1 = riDataBo.RetroNoClaimBonus1;
                RetroNoClaimBonus2 = riDataBo.RetroNoClaimBonus2;
                RetroNoClaimBonus3 = riDataBo.RetroNoClaimBonus3;
                RetroDatabaseCommission1 = riDataBo.RetroDatabaseCommission1;
                RetroDatabaseCommission2 = riDataBo.RetroDatabaseCommission2;
                RetroDatabaseCommission3 = riDataBo.RetroDatabaseCommission3;

                IssueDatePolStr = riDataBo.IssueDatePol?.ToString(Util.GetDateFormat());
                IssueDateBenStr = riDataBo.IssueDateBen?.ToString(Util.GetDateFormat());
                ReinsEffDatePolStr = riDataBo.ReinsEffDatePol?.ToString(Util.GetDateFormat());
                ReinsEffDateBenStr = riDataBo.ReinsEffDateBen?.ToString(Util.GetDateFormat());
                InsuredDateOfBirthStr = riDataBo.InsuredDateOfBirth?.ToString(Util.GetDateFormat());
                InsuredDateOfBirth2ndStr = riDataBo.InsuredDateOfBirth2nd?.ToString(Util.GetDateFormat());
                PolicyExpiryDateStr = riDataBo.PolicyExpiryDate?.ToString(Util.GetDateFormat());
                AdjBeginDateStr = riDataBo.AdjBeginDate?.ToString(Util.GetDateFormat());
                AdjEndDateStr = riDataBo.AdjEndDate?.ToString(Util.GetDateFormat());
                TempD1Str = riDataBo.TempD1?.ToString(Util.GetDateFormat());
                TempD2Str = riDataBo.TempD2?.ToString(Util.GetDateFormat());
                TempD3Str = riDataBo.TempD3?.ToString(Util.GetDateFormat());
                TempD4Str = riDataBo.TempD4?.ToString(Util.GetDateFormat());
                TempD5Str = riDataBo.TempD5?.ToString(Util.GetDateFormat());

                // Phase 2
                EffectiveDateStr = riDataBo.EffectiveDate?.ToString(Util.GetDateFormat());
                OfferLetterSentDateStr = riDataBo.OfferLetterSentDate?.ToString(Util.GetDateFormat());
                RiskPeriodStartDateStr = riDataBo.RiskPeriodStartDate?.ToString(Util.GetDateFormat());
                RiskPeriodEndDateStr = riDataBo.RiskPeriodEndDate?.ToString(Util.GetDateFormat());

                DurationDayStr = Util.DoubleToString(riDataBo.DurationDay);
                DurationMonthStr = Util.DoubleToString(riDataBo.DurationMonth);
                RiCovPeriodStr = Util.DoubleToString(riDataBo.RiCovPeriod);
                OriSumAssuredStr = Util.DoubleToString(riDataBo.OriSumAssured);
                CurrSumAssuredStr = Util.DoubleToString(riDataBo.CurrSumAssured);
                AmountCededB4MlreShareStr = Util.DoubleToString(riDataBo.AmountCededB4MlreShare);
                RetentionAmountStr = Util.DoubleToString(riDataBo.RetentionAmount);
                AarOriStr = Util.DoubleToString(riDataBo.AarOri);
                AarStr = Util.DoubleToString(riDataBo.Aar);
                AarSpecial1Str = Util.DoubleToString(riDataBo.AarSpecial1);
                AarSpecial2Str = Util.DoubleToString(riDataBo.AarSpecial2);
                AarSpecial3Str = Util.DoubleToString(riDataBo.AarSpecial3);
                DurationYearStr = Util.DoubleToString(riDataBo.DurationYear);
                CedantRiRateStr = Util.DoubleToString(riDataBo.CedantRiRate);
                DiscountRateStr = Util.DoubleToString(riDataBo.DiscountRate);
                UnderwriterRatingStr = Util.DoubleToString(riDataBo.UnderwriterRating);
                UnderwriterRatingUnitStr = Util.DoubleToString(riDataBo.UnderwriterRatingUnit);
                UnderwriterRating2Str = Util.DoubleToString(riDataBo.UnderwriterRating2);
                UnderwriterRatingUnit2Str = Util.DoubleToString(riDataBo.UnderwriterRatingUnit2);
                UnderwriterRating3Str = Util.DoubleToString(riDataBo.UnderwriterRating3);
                UnderwriterRatingUnit3Str = Util.DoubleToString(riDataBo.UnderwriterRatingUnit3);
                FlatExtraAmountStr = Util.DoubleToString(riDataBo.FlatExtraAmount);
                FlatExtraUnitStr = Util.DoubleToString(riDataBo.FlatExtraUnit);
                FlatExtraAmount2Str = Util.DoubleToString(riDataBo.FlatExtraAmount2);
                StandardPremiumStr = Util.DoubleToString(riDataBo.StandardPremium);
                SubstandardPremiumStr = Util.DoubleToString(riDataBo.SubstandardPremium);
                FlatExtraPremiumStr = Util.DoubleToString(riDataBo.FlatExtraPremium);
                GrossPremiumStr = Util.DoubleToString(riDataBo.GrossPremium);
                StandardDiscountStr = Util.DoubleToString(riDataBo.StandardDiscount);
                SubstandardDiscountStr = Util.DoubleToString(riDataBo.SubstandardDiscount);
                VitalityDiscountStr = Util.DoubleToString(riDataBo.VitalityDiscount);
                TotalDiscountStr = Util.DoubleToString(riDataBo.TotalDiscount);
                NetPremiumStr = Util.DoubleToString(riDataBo.NetPremium);
                AnnualRiPremStr = Util.DoubleToString(riDataBo.AnnualRiPrem);
                PolicyGrossPremiumStr = Util.DoubleToString(riDataBo.PolicyGrossPremium);
                PolicyStandardPremiumStr = Util.DoubleToString(riDataBo.PolicyStandardPremium);
                PolicySubstandardPremiumStr = Util.DoubleToString(riDataBo.PolicySubstandardPremium);
                PolicyAmountDeathStr = Util.DoubleToString(riDataBo.PolicyAmountDeath);
                PolicyReserveStr = Util.DoubleToString(riDataBo.PolicyReserve);
                ApLoadingStr = Util.DoubleToString(riDataBo.ApLoading);
                LoanInterestRateStr = Util.DoubleToString(riDataBo.LoanInterestRate);
                CedantSarStr = Util.DoubleToString(riDataBo.CedantSar);
                AmountCededB4MlreShare2Str = Util.DoubleToString(riDataBo.AmountCededB4MlreShare2);
                GroupEmployeeBasicSalaryStr = Util.DoubleToString(riDataBo.GroupEmployeeBasicSalary);
                GroupEmployeeBasicSalaryReviseStr = Util.DoubleToString(riDataBo.GroupEmployeeBasicSalaryRevise);
                GroupEmployeeBasicSalaryMultiplierStr = Util.DoubleToString(riDataBo.GroupEmployeeBasicSalaryMultiplier);
                PolicyAmountSubstandardStr = Util.DoubleToString(riDataBo.PolicyAmountSubstandard);
                Layer1RiShareStr = Util.DoubleToString(riDataBo.Layer1RiShare);
                Layer1StandardPremiumStr = Util.DoubleToString(riDataBo.Layer1StandardPremium);
                Layer1SubstandardPremiumStr = Util.DoubleToString(riDataBo.Layer1SubstandardPremium);
                Layer1GrossPremiumStr = Util.DoubleToString(riDataBo.Layer1GrossPremium);
                Layer1StandardDiscountStr = Util.DoubleToString(riDataBo.Layer1StandardDiscount);
                Layer1SubstandardDiscountStr = Util.DoubleToString(riDataBo.Layer1SubstandardDiscount);
                Layer1TotalDiscountStr = Util.DoubleToString(riDataBo.Layer1TotalDiscount);
                Layer1NetPremiumStr = Util.DoubleToString(riDataBo.Layer1NetPremium);
                Layer1GrossPremiumAltStr = Util.DoubleToString(riDataBo.Layer1GrossPremiumAlt);
                Layer1TotalDiscountAltStr = Util.DoubleToString(riDataBo.Layer1TotalDiscountAlt);
                Layer1NetPremiumAltStr = Util.DoubleToString(riDataBo.Layer1NetPremiumAlt);
                TaxAmountStr = Util.DoubleToString(riDataBo.TaxAmount);
                GstGrossPremiumStr = Util.DoubleToString(riDataBo.GstGrossPremium);
                GstTotalDiscountStr = Util.DoubleToString(riDataBo.GstTotalDiscount);
                GstVitalityStr = Util.DoubleToString(riDataBo.GstVitality);
                GstAmountStr = Util.DoubleToString(riDataBo.GstAmount);
                TempA1Str = Util.DoubleToString(riDataBo.TempA1);
                TempA2Str = Util.DoubleToString(riDataBo.TempA2);
                TempA3Str = Util.DoubleToString(riDataBo.TempA3);
                TempA4Str = Util.DoubleToString(riDataBo.TempA4);
                TempA5Str = Util.DoubleToString(riDataBo.TempA5);
                TempA6Str = Util.DoubleToString(riDataBo.TempA6);
                TempA7Str = Util.DoubleToString(riDataBo.TempA7);
                TempA8Str = Util.DoubleToString(riDataBo.TempA8);
                PolicyTermStr = Util.DoubleToString(riDataBo.PolicyTerm);
                PolicyTermRemainStr = Util.DoubleToString(riDataBo.PolicyTermRemain);

                // Phase 2
                CurrencyRateStr = Util.DoubleToString(riDataBo.CurrencyRate);
                NoClaimBonusStr = Util.DoubleToString(riDataBo.NoClaimBonus);
                SurrenderValueStr = Util.DoubleToString(riDataBo.SurrenderValue);
                DatabaseCommisionStr = Util.DoubleToString(riDataBo.DatabaseCommision);
                GrossPremiumAltStr = Util.DoubleToString(riDataBo.GrossPremiumAlt);
                NetPremiumAltStr = Util.DoubleToString(riDataBo.NetPremiumAlt);
                Layer1FlatExtraPremiumStr = Util.DoubleToString(riDataBo.Layer1FlatExtraPremium);
                TransactionPremiumStr = Util.DoubleToString(riDataBo.TransactionPremium);
                OriginalPremiumStr = Util.DoubleToString(riDataBo.OriginalPremium);
                TransactionDiscountStr = Util.DoubleToString(riDataBo.TransactionDiscount);
                OriginalDiscountStr = Util.DoubleToString(riDataBo.OriginalDiscount);
                BrokerageFeeStr = Util.DoubleToString(riDataBo.BrokerageFee);
                MaxUwRatingStr = Util.DoubleToString(riDataBo.MaxUwRating);
                RetentionCapStr = Util.DoubleToString(riDataBo.RetentionCap);
                AarCapStr = Util.DoubleToString(riDataBo.AarCap);
                RiRateStr = Util.DoubleToString(riDataBo.RiRate);
                RiRate2Str = Util.DoubleToString(riDataBo.RiRate2);
                AnnuityFactorStr = Util.DoubleToString(riDataBo.AnnuityFactor);
                SumAssuredOfferedStr = Util.DoubleToString(riDataBo.SumAssuredOffered);
                UwRatingOfferedStr = Util.DoubleToString(riDataBo.UwRatingOffered);
                FlatExtraAmountOfferedStr = Util.DoubleToString(riDataBo.FlatExtraAmountOffered);
                RetentionShareStr = Util.DoubleToString(riDataBo.RetentionShare);
                AarShareStr = Util.DoubleToString(riDataBo.AarShare);
                TotalDirectRetroAarStr = Util.DoubleToString(riDataBo.TotalDirectRetroAar);
                TotalDirectRetroGrossPremiumStr = Util.DoubleToString(riDataBo.TotalDirectRetroGrossPremium);
                TotalDirectRetroDiscountStr = Util.DoubleToString(riDataBo.TotalDirectRetroDiscount);
                TotalDirectRetroNetPremiumStr = Util.DoubleToString(riDataBo.TotalDirectRetroNetPremium);
                TotalDirectRetroNoClaimBonusStr = Util.DoubleToString(riDataBo.TotalDirectRetroNoClaimBonus);
                TotalDirectRetroDatabaseCommissionStr = Util.DoubleToString(riDataBo.TotalDirectRetroDatabaseCommission);
                MinAarStr = Util.DoubleToString(riDataBo.MinAar);
                MaxAarStr = Util.DoubleToString(riDataBo.MaxAar);
                CorridorLimitStr = Util.DoubleToString(riDataBo.CorridorLimit);
                AblStr = Util.DoubleToString(riDataBo.Abl);
                RiDiscountRateStr = Util.DoubleToString(riDataBo.RiDiscountRate);
                LargeSaDiscountStr = Util.DoubleToString(riDataBo.LargeSaDiscount);
                GroupSizeDiscountStr = Util.DoubleToString(riDataBo.GroupSizeDiscount);

                MaxApLoadingStr = Util.DoubleToString(riDataBo.MaxApLoading);
                MlreStandardPremiumStr = Util.DoubleToString(riDataBo.MlreStandardPremium);
                MlreSubstandardPremiumStr = Util.DoubleToString(riDataBo.MlreSubstandardPremium);
                MlreFlatExtraPremiumStr = Util.DoubleToString(riDataBo.MlreFlatExtraPremium);
                MlreGrossPremiumStr = Util.DoubleToString(riDataBo.MlreGrossPremium);
                MlreStandardDiscountStr = Util.DoubleToString(riDataBo.MlreStandardDiscount);
                MlreSubstandardDiscountStr = Util.DoubleToString(riDataBo.MlreSubstandardDiscount);
                MlreLargeSaDiscountStr = Util.DoubleToString(riDataBo.MlreLargeSaDiscount);
                MlreGroupSizeDiscountStr = Util.DoubleToString(riDataBo.MlreGroupSizeDiscount);
                MlreVitalityDiscountStr = Util.DoubleToString(riDataBo.MlreVitalityDiscount);
                MlreTotalDiscountStr = Util.DoubleToString(riDataBo.MlreTotalDiscount);
                MlreNetPremiumStr = Util.DoubleToString(riDataBo.MlreNetPremium);
                NetPremiumCheckStr = Util.DoubleToString(riDataBo.NetPremiumCheck);
                ServiceFeePercentageStr = Util.DoubleToString(riDataBo.ServiceFeePercentage);
                ServiceFeeStr = Util.DoubleToString(riDataBo.ServiceFee);
                MlreBrokerageFeeStr = Util.DoubleToString(riDataBo.MlreBrokerageFee);
                MlreDatabaseCommissionStr = Util.DoubleToString(riDataBo.MlreDatabaseCommission);
                AarShare2Str = Util.DoubleToString(riDataBo.AarShare2);
                AarCap2Str = Util.DoubleToString(riDataBo.AarCap2);
                WakalahFeePercentageStr = Util.DoubleToString(riDataBo.WakalahFeePercentage);

                // Direct Retro
                RetroShare1Str = Util.DoubleToString(riDataBo.RetroShare1);
                RetroShare2Str = Util.DoubleToString(riDataBo.RetroShare2);
                RetroShare3Str = Util.DoubleToString(riDataBo.RetroShare3);
                RetroPremiumSpread1Str = Util.DoubleToString(riDataBo.RetroPremiumSpread1);
                RetroPremiumSpread2Str = Util.DoubleToString(riDataBo.RetroPremiumSpread2);
                RetroPremiumSpread3Str = Util.DoubleToString(riDataBo.RetroPremiumSpread3);
                RetroAar1Str = Util.DoubleToString(riDataBo.RetroAar1);
                RetroAar2Str = Util.DoubleToString(riDataBo.RetroAar2);
                RetroAar3Str = Util.DoubleToString(riDataBo.RetroAar3);
                RetroReinsurancePremium1Str = Util.DoubleToString(riDataBo.RetroReinsurancePremium1);
                RetroReinsurancePremium2Str = Util.DoubleToString(riDataBo.RetroReinsurancePremium2);
                RetroReinsurancePremium3Str = Util.DoubleToString(riDataBo.RetroReinsurancePremium3);
                RetroDiscount1Str = Util.DoubleToString(riDataBo.RetroDiscount1);
                RetroDiscount2Str = Util.DoubleToString(riDataBo.RetroDiscount2);
                RetroDiscount3Str = Util.DoubleToString(riDataBo.RetroDiscount3);
                RetroNetPremium1Str = Util.DoubleToString(riDataBo.RetroNetPremium1);
                RetroNetPremium2Str = Util.DoubleToString(riDataBo.RetroNetPremium2);
                RetroNetPremium3Str = Util.DoubleToString(riDataBo.RetroNetPremium3);
                RetroNoClaimBonus1Str = Util.DoubleToString(riDataBo.RetroNoClaimBonus1);
                RetroNoClaimBonus2Str = Util.DoubleToString(riDataBo.RetroNoClaimBonus2);
                RetroNoClaimBonus3Str = Util.DoubleToString(riDataBo.RetroNoClaimBonus3);
                RetroDatabaseCommission1Str = Util.DoubleToString(riDataBo.RetroDatabaseCommission1);
                RetroDatabaseCommission2Str = Util.DoubleToString(riDataBo.RetroDatabaseCommission2);
                RetroDatabaseCommission3Str = Util.DoubleToString(riDataBo.RetroDatabaseCommission3);
            }
        }

        public void SetBos(RiDataWarehouseBo bo)
        {
            TreatyCode = bo.TreatyCode;
            ReinsBasisCode = bo.ReinsBasisCode;
            FundsAccountingTypeCode = bo.FundsAccountingTypeCode;
            PremiumFrequencyCode = bo.PremiumFrequencyCode;
            ReportPeriodMonth = bo.ReportPeriodMonth;
            ReportPeriodYear = bo.ReportPeriodYear;
            RiskPeriodMonth = bo.RiskPeriodMonth;
            RiskPeriodYear = bo.RiskPeriodYear;
            TransactionTypeCode = bo.TransactionTypeCode;
            PolicyNumber = bo.PolicyNumber;
            IssueDatePol = bo.IssueDatePol;
            IssueDateBen = bo.IssueDateBen;
            ReinsEffDatePol = bo.ReinsEffDatePol;
            ReinsEffDateBen = bo.ReinsEffDateBen;
            CedingPlanCode = bo.CedingPlanCode;
            CedingBenefitTypeCode = bo.CedingBenefitTypeCode;
            CedingBenefitRiskCode = bo.CedingBenefitRiskCode;
            MlreBenefitCode = bo.MlreBenefitCode;
            OriSumAssured = bo.OriSumAssured;
            CurrSumAssured = bo.CurrSumAssured;
            AmountCededB4MlreShare = bo.AmountCededB4MlreShare;
            RetentionAmount = bo.RetentionAmount;
            AarOri = bo.AarOri;
            Aar = bo.Aar;
            AarSpecial1 = bo.AarSpecial1;
            AarSpecial2 = bo.AarSpecial2;
            AarSpecial3 = bo.AarSpecial3;
            InsuredName = bo.InsuredName;
            InsuredGenderCode = bo.InsuredGenderCode;
            InsuredTobaccoUse = bo.InsuredTobaccoUse;
            InsuredDateOfBirth = bo.InsuredDateOfBirth;
            InsuredOccupationCode = bo.InsuredOccupationCode;
            InsuredRegisterNo = bo.InsuredRegisterNo;
            InsuredAttainedAge = bo.InsuredAttainedAge;
            InsuredNewIcNumber = bo.InsuredNewIcNumber;
            InsuredOldIcNumber = bo.InsuredOldIcNumber;
            InsuredName2nd = bo.InsuredName2nd;
            InsuredGenderCode2nd = bo.InsuredGenderCode2nd;
            InsuredTobaccoUse2nd = bo.InsuredTobaccoUse2nd;
            InsuredDateOfBirth2nd = bo.InsuredDateOfBirth2nd;
            InsuredAttainedAge2nd = bo.InsuredAttainedAge2nd;
            InsuredNewIcNumber2nd = bo.InsuredNewIcNumber2nd;
            InsuredOldIcNumber2nd = bo.InsuredOldIcNumber2nd;
            ReinsuranceIssueAge = bo.ReinsuranceIssueAge;
            ReinsuranceIssueAge2nd = bo.ReinsuranceIssueAge2nd;
            PolicyTerm = bo.PolicyTerm;
            PolicyExpiryDate = bo.PolicyExpiryDate;
            DurationYear = bo.DurationYear;
            DurationDay = bo.DurationDay;
            DurationMonth = bo.DurationMonth;
            PremiumCalType = bo.PremiumCalType;
            CedantRiRate = bo.CedantRiRate;
            RateTable = bo.RateTable;
            AgeRatedUp = bo.AgeRatedUp;
            DiscountRate = bo.DiscountRate;
            LoadingType = bo.LoadingType;
            UnderwriterRating = bo.UnderwriterRating;
            UnderwriterRatingUnit = bo.UnderwriterRatingUnit;
            UnderwriterRatingTerm = bo.UnderwriterRatingTerm;
            UnderwriterRating2 = bo.UnderwriterRating2;
            UnderwriterRatingUnit2 = bo.UnderwriterRatingUnit2;
            UnderwriterRatingTerm2 = bo.UnderwriterRatingTerm2;
            UnderwriterRating3 = bo.UnderwriterRating3;
            UnderwriterRatingUnit3 = bo.UnderwriterRatingUnit3;
            UnderwriterRatingTerm3 = bo.UnderwriterRatingTerm3;
            FlatExtraAmount = bo.FlatExtraAmount;
            FlatExtraUnit = bo.FlatExtraUnit;
            FlatExtraTerm = bo.FlatExtraTerm;
            FlatExtraAmount2 = bo.FlatExtraAmount2;
            FlatExtraTerm2 = bo.FlatExtraTerm2;
            StandardPremium = bo.StandardPremium;
            SubstandardPremium = bo.SubstandardPremium;
            FlatExtraPremium = bo.FlatExtraPremium;
            GrossPremium = bo.GrossPremium;
            StandardDiscount = bo.StandardDiscount;
            SubstandardDiscount = bo.SubstandardDiscount;
            VitalityDiscount = bo.VitalityDiscount;
            TotalDiscount = bo.TotalDiscount;
            NetPremium = bo.NetPremium;
            AnnualRiPrem = bo.AnnualRiPrem;
            RiCovPeriod = bo.RiCovPeriod;
            AdjBeginDate = bo.AdjBeginDate;
            AdjEndDate = bo.AdjEndDate;
            PolicyNumberOld = bo.PolicyNumberOld;
            PolicyStatusCode = bo.PolicyStatusCode;
            PolicyGrossPremium = bo.PolicyGrossPremium;
            PolicyStandardPremium = bo.PolicyStandardPremium;
            PolicySubstandardPremium = bo.PolicySubstandardPremium;
            PolicyTermRemain = bo.PolicyTermRemain;
            PolicyAmountDeath = bo.PolicyAmountDeath;
            PolicyReserve = bo.PolicyReserve;
            PolicyPaymentMethod = bo.PolicyPaymentMethod;
            PolicyLifeNumber = bo.PolicyLifeNumber;
            FundCode = bo.FundCode;
            LineOfBusiness = bo.LineOfBusiness;
            ApLoading = bo.ApLoading;
            LoanInterestRate = bo.LoanInterestRate;
            DefermentPeriod = bo.DefermentPeriod;
            RiderNumber = bo.RiderNumber;
            CampaignCode = bo.CampaignCode;
            Nationality = bo.Nationality;
            TerritoryOfIssueCode = bo.TerritoryOfIssueCode;
            CurrencyCode = bo.CurrencyCode;
            StaffPlanIndicator = bo.StaffPlanIndicator;
            CedingTreatyCode = bo.CedingTreatyCode;
            CedingPlanCodeOld = bo.CedingPlanCodeOld;
            CedingBasicPlanCode = bo.CedingBasicPlanCode;
            CedingBenefitTypeCode = bo.CedingBenefitTypeCode;
            CedantSar = bo.CedantSar;
            CedantReinsurerCode = bo.CedantReinsurerCode;
            AmountCededB4MlreShare2 = bo.AmountCededB4MlreShare2;
            CessionCode = bo.CessionCode;
            CedantRemark = bo.CedantRemark;
            GroupPolicyNumber = bo.GroupPolicyNumber;
            GroupPolicyName = bo.GroupPolicyName;
            NoOfEmployee = bo.NoOfEmployee;
            PolicyTotalLive = bo.PolicyTotalLive;
            GroupSubsidiaryName = bo.GroupSubsidiaryName;
            GroupSubsidiaryNo = bo.GroupSubsidiaryNo;
            GroupEmployeeBasicSalary = bo.GroupEmployeeBasicSalary;
            GroupEmployeeJobType = bo.GroupEmployeeJobType;
            GroupEmployeeJobCode = bo.GroupEmployeeJobCode;
            GroupEmployeeBasicSalaryRevise = bo.GroupEmployeeBasicSalaryRevise;
            GroupEmployeeBasicSalaryMultiplier = bo.GroupEmployeeBasicSalaryMultiplier;
            CedingPlanCode2 = bo.CedingPlanCode2;
            DependantIndicator = bo.DependantIndicator;
            GhsRoomBoard = bo.GhsRoomBoard;
            PolicyAmountSubstandard = bo.PolicyAmountSubstandard;
            Layer1RiShare = bo.Layer1RiShare;
            Layer1InsuredAttainedAge = bo.Layer1InsuredAttainedAge;
            Layer1InsuredAttainedAge2nd = bo.Layer1InsuredAttainedAge2nd;
            Layer1StandardPremium = bo.Layer1StandardPremium;
            Layer1SubstandardPremium = bo.Layer1SubstandardPremium;
            Layer1GrossPremium = bo.Layer1GrossPremium;
            Layer1StandardDiscount = bo.Layer1StandardDiscount;
            Layer1SubstandardDiscount = bo.Layer1SubstandardDiscount;
            Layer1TotalDiscount = bo.Layer1TotalDiscount;
            Layer1NetPremium = bo.Layer1NetPremium;
            Layer1GrossPremiumAlt = bo.Layer1GrossPremiumAlt;
            Layer1TotalDiscountAlt = bo.Layer1TotalDiscountAlt;
            Layer1NetPremiumAlt = bo.Layer1NetPremiumAlt;
            SpecialIndicator1 = bo.SpecialIndicator1;
            SpecialIndicator2 = bo.SpecialIndicator2;
            SpecialIndicator3 = bo.SpecialIndicator3;
            IndicatorJointLife = bo.IndicatorJointLife;
            TaxAmount = bo.TaxAmount;
            GstIndicator = bo.GstIndicator;
            GstGrossPremium = bo.GstGrossPremium;
            GstTotalDiscount = bo.GstTotalDiscount;
            GstVitality = bo.GstVitality;
            GstAmount = bo.GstAmount;
            Mfrs17BasicRider = bo.Mfrs17BasicRider;
            Mfrs17CellName = bo.Mfrs17CellName;
            Mfrs17TreatyCode = bo.Mfrs17TreatyCode;
            LoaCode = bo.LoaCode;
            CurrencyRate = bo.CurrencyRate;
            NoClaimBonus = bo.NoClaimBonus;
            SurrenderValue = bo.SurrenderValue;
            DatabaseCommision = bo.DatabaseCommision;
            GrossPremiumAlt = bo.GrossPremiumAlt;
            NetPremiumAlt = bo.NetPremiumAlt;
            Layer1FlatExtraPremium = bo.Layer1FlatExtraPremium;
            TransactionPremium = bo.TransactionPremium;
            OriginalPremium = bo.OriginalPremium;
            TransactionDiscount = bo.TransactionDiscount;
            OriginalDiscount = bo.OriginalDiscount;
            BrokerageFee = bo.BrokerageFee;
            MaxUwRating = bo.MaxUwRating;
            RetentionCap = bo.RetentionCap;
            AarCap = bo.AarCap;
            RiRate = bo.RiRate;
            RiRate2 = bo.RiRate2;
            AnnuityFactor = bo.AnnuityFactor;
            SumAssuredOffered = bo.SumAssuredOffered;
            UwRatingOffered = bo.UwRatingOffered;
            FlatExtraAmountOffered = bo.FlatExtraAmountOffered;
            FlatExtraDuration = (int?)bo.FlatExtraDuration;
            EffectiveDate = bo.EffectiveDate;
            OfferLetterSentDate = bo.OfferLetterSentDate;
            RiskPeriodStartDate = bo.RiskPeriodStartDate;
            RiskPeriodEndDate = bo.RiskPeriodEndDate;
            Mfrs17AnnualCohort = bo.Mfrs17AnnualCohort;
            MaxExpiryAge = bo.MaxExpiryAge;
            MinIssueAge = bo.MinIssueAge;
            MaxIssueAge = bo.MaxIssueAge;
            MinAar = bo.MinAar;
            MaxAar = bo.MaxAar;
            CorridorLimit = bo.CorridorLimit;
            Abl = bo.Abl;
            RatePerBasisUnit = bo.RatePerBasisUnit;
            RiDiscountRate = bo.RiDiscountRate;
            LargeSaDiscount = bo.LargeSaDiscount;
            GroupSizeDiscount = bo.GroupSizeDiscount;
            EwarpNumber = bo.EwarpNumber;
            EwarpActionCode = bo.EwarpActionCode;
            RetentionShare = bo.RetentionShare;
            AarShare = bo.AarShare;
            ProfitComm = bo.ProfitComm;
            TotalDirectRetroAar = bo.TotalDirectRetroAar;
            TotalDirectRetroGrossPremium = bo.TotalDirectRetroGrossPremium;
            TotalDirectRetroDiscount = bo.TotalDirectRetroDiscount;
            TotalDirectRetroNetPremium = bo.TotalDirectRetroNetPremium;
            TotalDirectRetroNoClaimBonus = bo.TotalDirectRetroNoClaimBonus;
            TotalDirectRetroDatabaseCommission = bo.TotalDirectRetroDatabaseCommission;
            TreatyType = bo.TreatyType;
            AarShare2 = bo.AarShare2;
            AarCap2 = bo.AarCap2;
            WakalahFeePercentage = bo.WakalahFeePercentage;
            TreatyNumber = bo.TreatyNumber;
            ConflictType = bo.ConflictType;

            //Direct Retro
            RetroParty1 = bo.RetroParty1;
            RetroParty2 = bo.RetroParty2;
            RetroParty3 = bo.RetroParty3;
            RetroShare1 = bo.RetroShare1;
            RetroShare2 = bo.RetroShare2;
            RetroShare3 = bo.RetroShare3;
            RetroPremiumSpread1 = bo.RetroPremiumSpread1;
            RetroPremiumSpread2 = bo.RetroPremiumSpread2;
            RetroPremiumSpread3 = bo.RetroPremiumSpread3;
            RetroAar1 = bo.RetroAar1;
            RetroAar2 = bo.RetroAar2;
            RetroAar3 = bo.RetroAar3;
            RetroReinsurancePremium1 = bo.RetroReinsurancePremium1;
            RetroReinsurancePremium2 = bo.RetroReinsurancePremium2;
            RetroReinsurancePremium3 = bo.RetroReinsurancePremium3;
            RetroDiscount1 = bo.RetroDiscount1;
            RetroDiscount2 = bo.RetroDiscount2;
            RetroDiscount3 = bo.RetroDiscount3;
            RetroNetPremium1 = bo.RetroNetPremium1;
            RetroNetPremium2 = bo.RetroNetPremium2;
            RetroNetPremium3 = bo.RetroNetPremium3;
            RetroNoClaimBonus1 = bo.RetroNoClaimBonus1;
            RetroNoClaimBonus2 = bo.RetroNoClaimBonus2;
            RetroNoClaimBonus3 = bo.RetroNoClaimBonus3;
            RetroDatabaseCommission1 = bo.RetroDatabaseCommission1;
            RetroDatabaseCommission2 = bo.RetroDatabaseCommission2;
            RetroDatabaseCommission3 = bo.RetroDatabaseCommission3;

            // Phase 2
            IssueDatePolStr = bo.IssueDatePolStr;
            IssueDateBenStr = bo.IssueDateBenStr;
            ReinsEffDatePolStr = bo.ReinsEffDatePolStr;
            ReinsEffDateBenStr = bo.ReinsEffDateBenStr;
            InsuredDateOfBirthStr = bo.InsuredDateOfBirthStr;
            InsuredDateOfBirth2ndStr = bo.InsuredDateOfBirth2ndStr;
            PolicyExpiryDateStr = bo.PolicyExpiryDateStr;
            AdjBeginDateStr = bo.AdjBeginDateStr;
            AdjEndDateStr = bo.AdjEndDateStr;
            EffectiveDateStr = bo.EffectiveDateStr;
            OfferLetterSentDateStr = bo.OfferLetterSentDateStr;
            RiskPeriodStartDateStr = bo.RiskPeriodStartDateStr;
            RiskPeriodEndDateStr = bo.RiskPeriodEndDateStr;
            DurationDayStr = bo.DurationDayStr;
            DurationMonthStr = bo.DurationMonthStr;
            RiCovPeriodStr = bo.RiCovPeriodStr;
            OriSumAssuredStr = bo.OriSumAssuredStr;
            CurrSumAssuredStr = bo.CurrSumAssuredStr;
            AmountCededB4MlreShareStr = bo.AmountCededB4MlreShareStr;
            RetentionAmountStr = bo.RetentionAmountStr;
            AarOriStr = bo.AarOriStr;
            AarStr = bo.AarStr;
            AarSpecial1Str = bo.AarSpecial1Str;
            AarSpecial2Str = bo.AarSpecial2Str;
            AarSpecial3Str = bo.AarSpecial3Str;
            DurationYearStr = bo.DurationYearStr;
            CedantRiRateStr = bo.CedantRiRateStr;
            DiscountRateStr = bo.DiscountRateStr;
            UnderwriterRatingStr = bo.UnderwriterRatingStr;
            UnderwriterRatingUnitStr = bo.UnderwriterRatingUnitStr;
            UnderwriterRating2Str = bo.UnderwriterRating2Str;
            UnderwriterRatingUnit2Str = bo.UnderwriterRatingUnit2Str;
            UnderwriterRating3Str = bo.UnderwriterRating3Str;
            UnderwriterRatingUnit3Str = bo.UnderwriterRatingUnit3Str;
            FlatExtraAmountStr = bo.FlatExtraAmountStr;
            FlatExtraUnitStr = bo.FlatExtraUnitStr;
            FlatExtraAmount2Str = bo.FlatExtraAmount2Str;
            StandardPremiumStr = bo.StandardPremiumStr;
            SubstandardPremiumStr = bo.SubstandardPremiumStr;
            FlatExtraPremiumStr = bo.FlatExtraPremiumStr;
            GrossPremiumStr = bo.GrossPremiumStr;
            StandardDiscountStr = bo.StandardDiscountStr;
            SubstandardDiscountStr = bo.SubstandardDiscountStr;
            VitalityDiscountStr = bo.VitalityDiscountStr;
            TotalDiscountStr = bo.TotalDiscountStr;
            NetPremiumStr = bo.NetPremiumStr;
            AnnualRiPremStr = bo.AnnualRiPremStr;
            PolicyGrossPremiumStr = bo.PolicyGrossPremiumStr;
            PolicyStandardPremiumStr = bo.PolicyStandardPremiumStr;
            PolicySubstandardPremiumStr = bo.PolicySubstandardPremiumStr;
            PolicyAmountDeathStr = bo.PolicyAmountDeathStr;
            PolicyReserveStr = bo.PolicyReserveStr;
            ApLoadingStr = bo.ApLoadingStr;
            LoanInterestRateStr = bo.LoanInterestRateStr;
            CedantSarStr = bo.CedantSarStr;
            AmountCededB4MlreShare2Str = bo.AmountCededB4MlreShare2Str;
            GroupEmployeeBasicSalaryStr = bo.GroupEmployeeBasicSalaryStr;
            GroupEmployeeBasicSalaryReviseStr = bo.GroupEmployeeBasicSalaryReviseStr;
            GroupEmployeeBasicSalaryMultiplierStr = bo.GroupEmployeeBasicSalaryMultiplierStr;
            PolicyAmountSubstandardStr = bo.PolicyAmountSubstandardStr;
            Layer1RiShareStr = bo.Layer1RiShareStr;
            Layer1StandardPremiumStr = bo.Layer1StandardPremiumStr;
            Layer1SubstandardPremiumStr = bo.Layer1SubstandardPremiumStr;
            Layer1GrossPremiumStr = bo.Layer1GrossPremiumStr;
            Layer1StandardDiscountStr = bo.Layer1StandardDiscountStr;
            Layer1SubstandardDiscountStr = bo.Layer1SubstandardDiscountStr;
            Layer1TotalDiscountStr = bo.Layer1TotalDiscountStr;
            Layer1NetPremiumStr = bo.Layer1NetPremiumStr;
            Layer1GrossPremiumAltStr = bo.Layer1GrossPremiumAltStr;
            Layer1TotalDiscountAltStr = bo.Layer1TotalDiscountAltStr;
            Layer1NetPremiumAltStr = bo.Layer1NetPremiumAltStr;
            TaxAmountStr = bo.TaxAmountStr;
            GstGrossPremiumStr = bo.GstGrossPremiumStr;
            GstTotalDiscountStr = bo.GstTotalDiscountStr;
            GstVitalityStr = bo.GstVitalityStr;
            GstAmountStr = bo.GstAmountStr;
            CurrencyRateStr = bo.CurrencyRateStr;
            NoClaimBonusStr = bo.NoClaimBonusStr;
            SurrenderValueStr = bo.SurrenderValueStr;
            DatabaseCommisionStr = bo.DatabaseCommisionStr;
            GrossPremiumAltStr = bo.GrossPremiumAltStr;
            NetPremiumAltStr = bo.NetPremiumAltStr;
            Layer1FlatExtraPremiumStr = bo.Layer1FlatExtraPremiumStr;
            TransactionPremiumStr = bo.TransactionPremiumStr;
            OriginalPremiumStr = bo.OriginalPremiumStr;
            TransactionDiscountStr = bo.TransactionDiscountStr;
            OriginalDiscountStr = bo.OriginalDiscountStr;
            BrokerageFeeStr = bo.BrokerageFeeStr;
            MaxUwRatingStr = bo.MaxUwRatingStr;
            RetentionCapStr = bo.RetentionCapStr;
            AarCapStr = bo.AarCapStr;
            RiRateStr = bo.RiRateStr;
            RiRate2Str = bo.RiRate2Str;
            AnnuityFactorStr = bo.AnnuityFactorStr;
            SumAssuredOfferedStr = bo.SumAssuredOfferedStr;
            UwRatingOfferedStr = bo.UwRatingOfferedStr;
            FlatExtraAmountOfferedStr = bo.FlatExtraAmountOfferedStr;
            RetentionShareStr = bo.RetentionShareStr;
            AarShareStr = bo.AarShareStr;
            TotalDirectRetroAarStr = bo.TotalDirectRetroAarStr;
            TotalDirectRetroGrossPremiumStr = bo.TotalDirectRetroGrossPremiumStr;
            TotalDirectRetroDiscountStr = bo.TotalDirectRetroDiscountStr;
            TotalDirectRetroNetPremiumStr = bo.TotalDirectRetroNetPremiumStr;
            TotalDirectRetroNoClaimBonusStr = bo.TotalDirectRetroNoClaimBonusStr;
            TotalDirectRetroDatabaseCommissionStr = bo.TotalDirectRetroDatabaseCommissionStr;
            MinAarStr = bo.MinAarStr;
            MaxAarStr = bo.MaxAarStr;
            CorridorLimitStr = bo.CorridorLimitStr;
            AblStr = bo.AblStr;
            RiDiscountRateStr = bo.RiDiscountRateStr;
            LargeSaDiscountStr = bo.LargeSaDiscountStr;
            GroupSizeDiscountStr = bo.GroupSizeDiscountStr;
            AarShare2Str = bo.AarShare2Str;
            AarCap2Str = bo.AarCap2Str;
            WakalahFeePercentageStr = bo.WakalahFeePercentageStr;
        }
    }

    public class RiDataListingViewModel
    {
        public int Id { get; set; }

        public int? RiDataBatchId { get; set; }

        public int? RiDataFileId { get; set; }

        public int RecordType { get; set; }

        public int? OriginalEntryId { get; set; }
        
        public bool IgnoreFinalise { get; set; }

        public int MappingStatus { get; set; }

        public int PreComputation1Status { get; set; }

        public int PreComputation2Status { get; set; }

        public int PreValidationStatus { get; set; }

        public int ConflictType { get; set; }

        public int PostComputationStatus { get; set; }

        public int PostValidationStatus { get; set; }

        public int FinaliseStatus { get; set; }

        public int ProcessWarehouseStatus { get; set; }

        public string TreatyCode { get; set; }

        public string PolicyNumber { get; set; }

        public string InsuredName { get; set; }

        public string TransactionTypeCode { get; set; }

        public string ReinsBasicCode { get; set; }

        public int? ReportPeriodMonth { get; set; }

        public int? ReportPeriodYear { get; set; }

        public int? RiskPeriodMonth { get; set; }

        public int? RiskPeriodYear { get; set; }

        public static Expression<Func<RiData, RiDataListingViewModel>> Expression()
        {
            return entity => new RiDataListingViewModel
            {
                Id = entity.Id,
                RiDataBatchId = entity.RiDataBatchId,
                RiDataFileId = entity.RiDataFileId,
                OriginalEntryId = entity.OriginalEntryId,
                MappingStatus = entity.MappingStatus,
                PreComputation1Status = entity.PreComputation1Status,
                PreComputation2Status = entity.PreComputation2Status,
                PreValidationStatus = entity.PreValidationStatus,
                ConflictType = entity.ConflictType,
                FinaliseStatus = entity.FinaliseStatus,
                TreatyCode = entity.TreatyCode,
                PolicyNumber = entity.PolicyNumber,
                InsuredName = entity.InsuredName,

                IgnoreFinalise = entity.IgnoreFinalise,
                RecordType = entity.RecordType,
                PostComputationStatus = entity.PostComputationStatus,
                PostValidationStatus = entity.PostValidationStatus,
                ProcessWarehouseStatus = entity.ProcessWarehouseStatus,

                TransactionTypeCode = entity.TransactionTypeCode,
                ReinsBasicCode = entity.ReinsBasisCode,
                ReportPeriodMonth = entity.ReportPeriodMonth,
                ReportPeriodYear = entity.ReportPeriodYear,
                RiskPeriodMonth = entity.RiskPeriodMonth,
                RiskPeriodYear = entity.RiskPeriodYear,
            };
        }
    }
}