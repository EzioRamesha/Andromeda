using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.RiDatas
{
    public class RiDataWarehouseBo
    {
        public int Id { get; set; }

        public int? EndingPolicyStatus { get; set; }

        public string EndingPolicyStatusCode { get; set; }

        public int RecordType { get; set; }

        public string Quarter { get; set; }

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

        [MaxLength(128)]
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

        public string EwarpActionCode { get; set; }

        public double? RetentionShare { get; set; }

        public double? AarShare { get; set; }

        [MaxLength(1)]
        public string ProfitComm { get; set; }

        public double? TotalDirectRetroAar { get; set; }

        public double? TotalDirectRetroGrossPremium { get; set; }

        public double? TotalDirectRetroDiscount { get; set; }

        public double? TotalDirectRetroNetPremium { get; set; }

        public double? TotalDirectRetroNoClaimBonus { get; set; }

        public double? TotalDirectRetroDatabaseCommission { get; set; }

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

        [Column(TypeName = "datetime2")]
        public DateTime LastUpdatedDate { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        // Direct Retro

        [MaxLength(128)]
        public string RetroParty1 { get; set; }

        [MaxLength(128)]
        public string RetroParty2 { get; set; }

        [MaxLength(128)]
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

        public double? AarShare2 { get; set; }

        public double? AarCap2 { get; set; }

        public double? WakalahFeePercentage { get; set; }

        public string TreatyNumber { get; set; }

        public int ConflictType { get; set; }

        // Date

        public string IssueDatePolStr { get; set; }

        public string IssueDateBenStr { get; set; }

        public string ReinsEffDatePolStr { get; set; }

        public string ReinsEffDateBenStr { get; set; }

        public string InsuredDateOfBirthStr { get; set; }

        public string InsuredDateOfBirth2ndStr { get; set; }

        public string PolicyExpiryDateStr { get; set; }

        public string AdjBeginDateStr { get; set; }

        public string AdjEndDateStr { get; set; }

        public string EffectiveDateStr { get; set; }

        public string OfferLetterSentDateStr { get; set; }

        public string RiskPeriodStartDateStr { get; set; }

        public string RiskPeriodEndDateStr { get; set; }


        // Double
        public string OriSumAssuredStr { get; set; }

        public string CurrSumAssuredStr { get; set; }

        public string AmountCededB4MlreShareStr { get; set; }

        public string RetentionAmountStr { get; set; }

        public string AarOriStr { get; set; }

        public string AarStr { get; set; }

        public string AarSpecial1Str { get; set; }

        public string AarSpecial2Str { get; set; }

        public string AarSpecial3Str { get; set; }

        public string DurationYearStr { get; set; }

        public string CedantRiRateStr { get; set; }

        public string DiscountRateStr { get; set; }

        public string UnderwriterRatingStr { get; set; }

        public string UnderwriterRatingUnitStr { get; set; }

        public string UnderwriterRating2Str { get; set; }

        public string UnderwriterRatingUnit2Str { get; set; }

        public string UnderwriterRating3Str { get; set; }

        public string UnderwriterRatingUnit3Str { get; set; }

        public string FlatExtraAmountStr { get; set; }

        public string FlatExtraUnitStr { get; set; }

        public string FlatExtraAmount2Str { get; set; }

        public string StandardPremiumStr { get; set; }

        public string SubstandardPremiumStr { get; set; }

        public string FlatExtraPremiumStr { get; set; }

        public string GrossPremiumStr { get; set; }

        public string StandardDiscountStr { get; set; }

        public string SubstandardDiscountStr { get; set; }

        public string VitalityDiscountStr { get; set; }

        public string TotalDiscountStr { get; set; }

        public string NetPremiumStr { get; set; }

        public string AnnualRiPremStr { get; set; }

        public string PolicyGrossPremiumStr { get; set; }

        public string PolicyStandardPremiumStr { get; set; }

        public string PolicySubstandardPremiumStr { get; set; }

        public string PolicyAmountDeathStr { get; set; }

        public string PolicyReserveStr { get; set; }

        public string ApLoadingStr { get; set; }

        public string LoanInterestRateStr { get; set; }

        public string CedantSarStr { get; set; }

        public string AmountCededB4MlreShare2Str { get; set; }

        public string GroupEmployeeBasicSalaryStr { get; set; }

        public string GroupEmployeeBasicSalaryReviseStr { get; set; }

        public string GroupEmployeeBasicSalaryMultiplierStr { get; set; }

        public string PolicyAmountSubstandardStr { get; set; }

        public string Layer1RiShareStr { get; set; }

        public string Layer1StandardPremiumStr { get; set; }

        public string Layer1SubstandardPremiumStr { get; set; }

        public string Layer1GrossPremiumStr { get; set; }

        public string Layer1StandardDiscountStr { get; set; }

        public string Layer1SubstandardDiscountStr { get; set; }

        public string Layer1TotalDiscountStr { get; set; }

        public string Layer1NetPremiumStr { get; set; }

        public string Layer1GrossPremiumAltStr { get; set; }

        public string Layer1TotalDiscountAltStr { get; set; }

        public string Layer1NetPremiumAltStr { get; set; }

        public string TaxAmountStr { get; set; }

        public string GstGrossPremiumStr { get; set; }

        public string GstTotalDiscountStr { get; set; }

        public string GstVitalityStr { get; set; }

        public string GstAmountStr { get; set; }

        public string DurationDayStr { get; set; }

        public string DurationMonthStr { get; set; }

        public string RiCovPeriodStr { get; set; }

        public string CurrencyRateStr { get; set; }

        public string NoClaimBonusStr { get; set; }

        public string SurrenderValueStr { get; set; }

        public string DatabaseCommisionStr { get; set; }

        public string GrossPremiumAltStr { get; set; }

        public string NetPremiumAltStr { get; set; }

        public string Layer1FlatExtraPremiumStr { get; set; }

        public string TransactionPremiumStr { get; set; }

        public string OriginalPremiumStr { get; set; }

        public string TransactionDiscountStr { get; set; }

        public string OriginalDiscountStr { get; set; }

        public string BrokerageFeeStr { get; set; }

        public string MaxUwRatingStr { get; set; }

        public string RetentionCapStr { get; set; }

        public string AarCapStr { get; set; }

        public string RiRateStr { get; set; }

        public string RiRate2Str { get; set; }

        public string AnnuityFactorStr { get; set; }

        public string SumAssuredOfferedStr { get; set; }

        public string UwRatingOfferedStr { get; set; }

        public string FlatExtraAmountOfferedStr { get; set; }

        public string FlatExtraDurationStr { get; set; }

        public string RetentionShareStr { get; set; }

        public string AarShareStr { get; set; }

        public string TotalDirectRetroAarStr { get; set; }

        public string TotalDirectRetroGrossPremiumStr { get; set; }

        public string TotalDirectRetroDiscountStr { get; set; }

        public string TotalDirectRetroNetPremiumStr { get; set; }

        public string TotalDirectRetroNoClaimBonusStr { get; set; }

        public string TotalDirectRetroDatabaseCommissionStr { get; set; }

        public string MinAarStr { get; set; }

        public string MaxAarStr { get; set; }

        public string CorridorLimitStr { get; set; }

        public string AblStr { get; set; }

        public string RiDiscountRateStr { get; set; }

        public string LargeSaDiscountStr { get; set; }

        public string GroupSizeDiscountStr { get; set; }

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

        public string MaxApLoadingStr { get; set; }

        public string MlreStandardPremiumStr { get; set; }

        public string MlreSubstandardPremiumStr { get; set; }

        public string MlreFlatExtraPremiumStr { get; set; }

        public string MlreGrossPremiumStr { get; set; }

        public string MlreStandardDiscountStr { get; set; }

        public string MlreSubstandardDiscountStr { get; set; }

        public string MlreLargeSaDiscountStr { get; set; }

        public string MlreGroupSizeDiscountStr { get; set; }

        public string MlreVitalityDiscountStr { get; set; }

        public string MlreTotalDiscountStr { get; set; }

        public string MlreNetPremiumStr { get; set; }

        public string NetPremiumCheckStr { get; set; }

        public string ServiceFeePercentageStr { get; set; }

        public string ServiceFeeStr { get; set; }

        public string MlreBrokerageFeeStr { get; set; }

        public string MlreDatabaseCommissionStr { get; set; }

        public string AarShare2Str { get; set; }

        public string AarCap2Str { get; set; }

        public string WakalahFeePercentageStr { get; set; }

        public string RecordTypeStr { get; set; }

        // For Referral Claim
        public string MlreShareStr { get; set; }

        public const int PolicyStatusInforce = 1;
        public const int PolicyStatusTerminated = 2;
        public const int PolicyStatusReversal = 3;
        public const int PolicyStatusMax = 3;

        public static string GetPolicyStatusName(int key)
        {
            switch(key)
            {
                case PolicyStatusInforce:
                    return "Inforce";
                case PolicyStatusTerminated:
                    return "Terminated";
                case PolicyStatusReversal:
                    return "Reversal";
                default:
                    return "";
            }
        }

        public static List<Column> GetColumns(bool linkReferral = false)
        {
            var columns = new List<Column>();

            if (linkReferral)
            {
                columns = new List<Column>()
                {
                    new Column
                    {
                        Header = "Referral ID",
                        Property = "ReferralId",
                    },
                };
            }
            else
            {
                columns = new List<Column>()
                {
                    new Column
                    {
                        Header = "ID",
                        Property = "Id",
                    },
                    new Column
                    {
                        Header = "Record Type",
                        Property = "RecordType",
                    },
                    new Column
                    {
                        Header = "Ending Policy Status",
                        Property = "EndingPolicyStatusCode",
                    },
                };
            }

            List<int> excludedStdOutput = StandardOutputBo.GetWarehouseExcludedTypes();

            // add all standard fields
            for (int i = 1; i <= StandardOutputBo.TypeMax; i++)
            {
                if (excludedStdOutput.Contains(i))
                    continue;

                columns.Add(new Column
                {
                    Header = StandardOutputBo.GetCodeByType(i),
                    Property = StandardOutputBo.GetPropertyNameByType(i),
                });
            }

            if (!linkReferral)
            {
                columns.Add(new Column
                {
                    Header = "Last Updated Date",
                    Property = "LastUpdatedDate",
                });
            }

            return columns;
        }
    }
}
