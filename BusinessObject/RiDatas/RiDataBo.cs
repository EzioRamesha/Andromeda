
using Newtonsoft.Json;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Globalization;
using System.Linq;

namespace BusinessObject.RiDatas
{
    public class RiDataBo
    {
        public LogRiData Log { get; set; }

        public int Id { get; set; }

        public int? RiDataBatchId { get; set; }
        public RiDataBatchBo RiDataBatchBo { get; set; }

        public int? RiDataFileId { get; set; }
        public RiDataFileBo RiDataFileBo { get; set; }

        // Added in Phase 2
        public int RecordType { get; set; }
        public int? OriginalEntryId { get; set; }

        public RiDataWarehouseBo OriginalEntryBo { get; set; }

        public string OriginalEntryQuarter { get; set; }

        public const int ValidateKeyFormula = 1;
        public const int ValidateKeyMapping = 2;
        public const int ValidateKeyTreatyCodeMapping = 3;
        public const int ValidateKeyBenefitCodeMapping = 4;
        public const int ValidateKeyProductFeatureMapping = 5;
        public const int ValidateKeyCellMapping = 6;
        public const int ValidateKeyRateTableMapping = 7;
        public const int ValidateKeyAnnuityFactorMapping = 8;
        public const int ValidateKeyRiDataLookup = 9;
        public const int ValidateKeyRiskDate = 10;
        public const int ValidateKeyFacMapping = 11;
        public const int ValidateKeyTreatyNumberTreatyTypeMapping = 12;

        public List<RiDataComputationValidation> FormulaValidates { get; set; }
        public List<RiDataComputationValidation> TreatyCodeMappingValidates { get; set; }
        public List<RiDataComputationValidation> BenefitCodeMappingValidates { get; set; }
        public List<RiDataComputationValidation> ProductFeatureMappingValidates { get; set; }
        public List<RiDataComputationValidation> CellMappingValidates { get; set; }
        public List<RiDataComputationValidation> RateTableMappingValidates { get; set; }
        public List<RiDataComputationValidation> AnnuityFactorMappingValidates { get; set; }
        public List<RiDataComputationValidation> RiDataLookupValidates { get; set; }
        public List<RiDataComputationValidation> RiskDateValidates { get; set; }
        public List<RiDataComputationValidation> FacMappingValidates { get; set; }
        public List<RiDataComputationValidation> TreatyNumberTreatyTypeMappingValidates { get; set; }

        public bool IgnoreFinalise { get; set; }

        public bool FormulaValidate { get; set; } = true;
        public bool MappingValidate { get; set; } = true;
        public bool TreatyCodeMappingValidate { get; set; } = true;
        public bool TreatyNumberMappingValidate { get; set; } = true;
        public bool BenefitCodeMappingValidate { get; set; } = true;
        public bool ProductFeatureMappingValidate { get; set; } = true;
        public bool CellMappingValidate { get; set; } = true;
        public bool RateTableMappingValidate { get; set; } = true;
        public bool AnnuityFactorMappingValidate { get; set; } = true;
        public bool RiDataLookupValidate { get; set; } = true;
        public bool RiskDateValidate { get; set; } = true;
        public bool FacMappingValidate { get; set; } = true;

        public int MappingStatus { get; set; }
        public int PreComputation1Status { get; set; }
        public int PreComputation2Status { get; set; }
        public int PreValidationStatus { get; set; }
        public int PostComputationStatus { get; set; }
        public int PostValidationStatus { get; set; }
        public int FinaliseStatus { get; set; }
        public int ProcessWarehouseStatus { get; set; }

        public string Errors { get; set; }
        public dynamic ErrorObject { get; set; }
        public IDictionary<string, object> ErrorDictionary { get; set; }
        public string CustomField { get; set; }
        public dynamic CustomFieldObject { get; set; }
        public IDictionary<string, object> CustomFieldDictionary { get; set; }

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

        [Column(TypeName = "datetime2")]
        public DateTime? TempD1 { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? TempD2 { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? TempD3 { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? TempD4 { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? TempD5 { get; set; }

        [MaxLength(50)]
        public string TempS1 { get; set; }

        [MaxLength(50)]
        public string TempS2 { get; set; }

        [MaxLength(50)]
        public string TempS3 { get; set; }

        [MaxLength(50)]
        public string TempS4 { get; set; }

        [MaxLength(50)]
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

        // Phase 2
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

        public double? AarShare2 { get; set; }

        public double? AarCap2 { get; set; }

        public double? WakalahFeePercentage { get; set; }

        [MaxLength(128)]
        public string TreatyNumber { get; set; }

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

        public string TempD1Str { get; set; }

        public string TempD2Str { get; set; }

        public string TempD3Str { get; set; }

        public string TempD4Str { get; set; }

        public string TempD5Str { get; set; }

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

        public string TempA1Str { get; set; }

        public string TempA2Str { get; set; }

        public string TempA3Str { get; set; }

        public string TempA4Str { get; set; }

        public string TempA5Str { get; set; }

        public string TempA6Str { get; set; }

        public string TempA7Str { get; set; }

        public string TempA8Str { get; set; }

        public string DurationDayStr { get; set; }

        public string DurationMonthStr { get; set; }

        public string RiCovPeriodStr { get; set; }

        // Phase 2
        // Double
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

        // Date
        public string EffectiveDateStr { get; set; }

        public string OfferLetterSentDateStr { get; set; }

        public string RiskPeriodStartDateStr { get; set; }

        public string RiskPeriodEndDateStr { get; set; }

        // Retro Double
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

        // Type & Status Name
        public string RecordTypeStr { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        // Added in Phase 2
        public string Quarter { get; set; }

        public int? RiDataBatchStatus { get; set; }

        public int? SoaDataBatchId { get; set; }

        public int ConflictType { get; set; }

        // ProductFeatureMapping
        public int CedantId { get; set; }

        public const int MappingStatusPending = 1;
        public const int MappingStatusSuccess = 2;
        public const int MappingStatusFailed = 3;
        public const int MappingStatusMax = 3;

        // Added in Phase 2
        public const int PreComputation1StatusPending = 1;
        public const int PreComputation1StatusSuccess = 2;
        public const int PreComputation1StatusFailed = 3;
        public const int PreComputation1StatusMax = 3;

        // Added in Phase 2
        public const int PreComputation2StatusPending = 1;
        public const int PreComputation2StatusSuccess = 2;
        public const int PreComputation2StatusFailed = 3;
        public const int PreComputation2StatusMax = 3;

        public const int PreValidationStatusPending = 1;
        public const int PreValidationStatusSuccess = 2;
        public const int PreValidationStatusFailed = 3;
        public const int PreValidationStatusMax = 3;

        // Added in Phase 2
        public const int PostComputationStatusPending = 1;
        public const int PostComputationStatusSuccess = 2;
        public const int PostComputationStatusFailed = 3;
        public const int PostComputationStatusMax = 3;

        // Added in Phase 2
        public const int PostValidationStatusPending = 1;
        public const int PostValidationStatusSuccess = 2;
        public const int PostValidationStatusFailed = 3;
        public const int PostValidationStatusMax = 3;

        public const int FinaliseStatusPending = 1;
        public const int FinaliseStatusSuccess = 2;
        public const int FinaliseStatusFailed = 3;
        public const int FinaliseStatusMax = 3;

        // Added in Phase 2
        public const int ProcessWarehouseStatusPending = 1;
        public const int ProcessWarehouseStatusSuccess = 2;
        public const int ProcessWarehouseStatusFailed = 3;
        public const int ProcessWarehouseStatusMax = 3;

        public const int StatusDraft = 1;
        public const int StatusPending = 2;
        public const int StatusApproved = 3;
        public const int StatusRejected = 4;
        public const int StatusDisabled = 5;

        // Added in Phase 2
        public const int RecordTypeNew = 1;
        public const int RecordTypeAdjustment = 2;
        public const int RecordTypeMax = 2;

        public const string BenefitCodeMappingTitle = "Computation: Benefit Code Mapping: ";
        public const string TreatyCodeMappingTitle = "Computation: Treaty Code Mapping: ";
        public const string TreatyNumberTreatyTypeMappingTitle = "Computation: Treaty Number Treaty Type Mapping: ";
        public const string ProductFeatureMappingTitle = "Computation: Product Feature Mapping: ";
        public const string CellMappingTitle = "Computation: Cell Mapping: ";
        public const string RateTableMappingTitle = "Computation: Rate Table Mapping: ";
        public const string RateMappingTitle = "Computation: Rate table: ";
        public const string RiDiscountMappingTitle = "Computation: Ri Discount: ";
        public const string LargeDiscountMappingTitle = "Computation: Large SA Discount: ";
        public const string GroupDiscountMappingTitle = "Computation: Group Size Discount: ";
        public const string AnnuityFactorMappingTitle = "Computation: Annuity Factor Mapping: ";
        public const string AnnuityFactorRateTitle = "Computation: Annuity Factor Rate: ";
        public const string RiDataLookupTitle = "Computation: RI Data Lookup: ";
        public const string RiskDateTitle = "Computation: Risk Date: ";
        public const string FacMappingTitle = "Computation: FAC Mapping: ";

        public const int ConflictTypeGenderWithinBatch = 1;
        public const int ConflictTypeCountryWithinBatch = 2;
        public const int ConflictTypeGenderWithinWarehouse = 3;
        public const int ConflictTypeCountryWithinWarehouse = 4;
        public const int ConflictTypeGenderCountryWithinBatch = 5;
        public const int ConflictTypeGenderCountryWithinWarehouse = 6;
        public const int ConflictTypeGenderWithinBatchCountryWithinWarehouse = 7;
        public const int ConflictTypeGenderWithinWarehouseCountryWithinBatch = 8;

        public const int ConflictTypeMax = 8;

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusDraft:
                    return "Draft";
                case StatusPending:
                    return "Pending";
                case StatusApproved:
                    return "Approved";
                case StatusRejected:
                    return "Rejected";
                case StatusDisabled:
                    return "Disabled";
                default:
                    return "";
            }
        }

        public RiDataBo()
        {
            Log = new LogRiData();

            ErrorObject = new ExpandoObject();
            ErrorDictionary = (IDictionary<string, object>)ErrorObject;
            CustomFieldObject = new ExpandoObject();
            CustomFieldDictionary = (IDictionary<string, object>)CustomFieldObject;

            MappingStatus = MappingStatusPending;
            PreComputation1Status = PreComputation1StatusPending;
            PreComputation2Status = PreComputation2StatusPending;
            PreValidationStatus = PreValidationStatusPending;
            FinaliseStatus = FinaliseStatusPending;
            ProcessWarehouseStatus = ProcessWarehouseStatusPending;
            PostComputationStatus = PostComputationStatusPending;
            PostValidationStatus = PostValidationStatusPending;

            IgnoreFinalise = false;
            FormulaValidate = true;
            MappingValidate = true;

            FormulaValidates = new List<RiDataComputationValidation> { };
            BenefitCodeMappingValidates = new List<RiDataComputationValidation> { };
            TreatyCodeMappingValidates = new List<RiDataComputationValidation> { };
            ProductFeatureMappingValidates = new List<RiDataComputationValidation> { };
            CellMappingValidates = new List<RiDataComputationValidation> { };
            RateTableMappingValidates = new List<RiDataComputationValidation> { };
            AnnuityFactorMappingValidates = new List<RiDataComputationValidation> { };
            RiDataLookupValidates = new List<RiDataComputationValidation> { };
            RiskDateValidates = new List<RiDataComputationValidation> { };
            FacMappingValidates = new List<RiDataComputationValidation> { };
            TreatyNumberTreatyTypeMappingValidates = new List<RiDataComputationValidation> { };
            for (int i = 1; i <= RiDataComputationBo.MaxStep; i++)
            {
                FormulaValidates.Add(new RiDataComputationValidation { Step = i });
                BenefitCodeMappingValidates.Add(new RiDataComputationValidation { Step = i });
                TreatyCodeMappingValidates.Add(new RiDataComputationValidation { Step = i });
                ProductFeatureMappingValidates.Add(new RiDataComputationValidation { Step = i });
                CellMappingValidates.Add(new RiDataComputationValidation { Step = i });
                RateTableMappingValidates.Add(new RiDataComputationValidation { Step = i });
                AnnuityFactorMappingValidates.Add(new RiDataComputationValidation { Step = i });
                RiDataLookupValidates.Add(new RiDataComputationValidation { Step = i });
                RiskDateValidates.Add(new RiDataComputationValidation { Step = i });
                FacMappingValidates.Add(new RiDataComputationValidation { Step = i });
                TreatyNumberTreatyTypeMappingValidates.Add(new RiDataComputationValidation { Step = i });
            }
        }

        // Rearrange RI Data fields in the download file according to sequence
        public static List<int> ColumnSequence()
        {
            return new List<int> {
                StandardOutputBo.TypeTreatyCode,
                StandardOutputBo.TypeTreatyType,
                StandardOutputBo.TypeTreatyNumber,
                StandardOutputBo.TypeReinsBasisCode,
                StandardOutputBo.TypeFundsAccountingTypeCode,
                StandardOutputBo.TypePremiumFrequencyCode,
                StandardOutputBo.TypeReportPeriodMonth,
                StandardOutputBo.TypeReportPeriodYear,
                StandardOutputBo.TypeRiskPeriodMonth,
                StandardOutputBo.TypeRiskPeriodYear,
                StandardOutputBo.TypeTransactionTypeCode,
                StandardOutputBo.TypePolicyNumber,
                StandardOutputBo.TypePolicyNumberOld,
                StandardOutputBo.TypeIssueDatePol,
                StandardOutputBo.TypeIssueDateBen,
                StandardOutputBo.TypeReinsEffDatePol,
                StandardOutputBo.TypeReinsEffDateBen,
                StandardOutputBo.TypeCedingPlanCode,
                StandardOutputBo.TypeCedingPlanCode2,
                StandardOutputBo.TypeCedingBenefitTypeCode,
                StandardOutputBo.TypeCedingBenefitRiskCode,
                StandardOutputBo.TypeCedingTreatyCode,
                StandardOutputBo.TypeCedingPlanCodeOld,
                StandardOutputBo.TypeCedingBasicPlanCode,
                StandardOutputBo.TypeRiderNumber,
                StandardOutputBo.TypeCampaignCode,
                StandardOutputBo.TypeMlreBenefitCode,
                StandardOutputBo.TypeOriSumAssured,
                StandardOutputBo.TypeCurrSumAssured,
                StandardOutputBo.TypeAmountCededB4MlreShare,
                StandardOutputBo.TypeAmountCededB4MlreShare2,
                StandardOutputBo.TypeRetentionAmount,
                StandardOutputBo.TypeAarOri,
                StandardOutputBo.TypeAar,
                StandardOutputBo.TypeAarSpecial1,
                StandardOutputBo.TypeAarSpecial2,
                StandardOutputBo.TypeAarSpecial3,
                StandardOutputBo.TypeInsuredName,
                StandardOutputBo.TypeInsuredGenderCode,
                StandardOutputBo.TypeInsuredTobaccoUse,
                StandardOutputBo.TypeInsuredDateOfBirth,
                StandardOutputBo.TypeInsuredOccupationCode,
                StandardOutputBo.TypeInsuredRegisterNo,
                StandardOutputBo.TypeInsuredAttainedAge,
                StandardOutputBo.TypeInsuredNewIcNumber,
                StandardOutputBo.TypeInsuredOldIcNumber,
                StandardOutputBo.TypeInsuredName2nd,
                StandardOutputBo.TypeInsuredGenderCode2nd,
                StandardOutputBo.TypeInsuredTobaccoUse2nd,
                StandardOutputBo.TypeInsuredDateOfBirth2nd,
                StandardOutputBo.TypeInsuredAttainedAge2nd,
                StandardOutputBo.TypeInsuredNewIcNumber2nd,
                StandardOutputBo.TypeInsuredOldIcNumber2nd,
                StandardOutputBo.TypeReinsuranceIssueAge,
                StandardOutputBo.TypeReinsuranceIssueAge2nd,
                StandardOutputBo.TypePolicyTerm,
                StandardOutputBo.TypePolicyExpiryDate,
                StandardOutputBo.TypeDurationYear,
                StandardOutputBo.TypeDurationDay,
                StandardOutputBo.TypeDurationMonth,
                StandardOutputBo.TypePremiumCalType,
                StandardOutputBo.TypeCedantRiRate,
                StandardOutputBo.TypeAgeRatedUp,
                StandardOutputBo.TypeDiscountRate,
                StandardOutputBo.TypeLoadingType,
                StandardOutputBo.TypeUnderwriterRating,
                StandardOutputBo.TypeUnderwriterRatingUnit,
                StandardOutputBo.TypeUnderwriterRatingTerm,
                StandardOutputBo.TypeUnderwriterRating2,
                StandardOutputBo.TypeUnderwriterRatingUnit2,
                StandardOutputBo.TypeUnderwriterRatingTerm2,
                StandardOutputBo.TypeUnderwriterRating3,
                StandardOutputBo.TypeUnderwriterRatingUnit3,
                StandardOutputBo.TypeUnderwriterRatingTerm3,
                StandardOutputBo.TypeFlatExtraAmount,
                StandardOutputBo.TypeFlatExtraUnit,
                StandardOutputBo.TypeFlatExtraTerm,
                StandardOutputBo.TypeFlatExtraAmount2,
                StandardOutputBo.TypeFlatExtraTerm2,
                StandardOutputBo.TypeStandardPremium,
                StandardOutputBo.TypeSubstandardPremium,
                StandardOutputBo.TypeFlatExtraPremium,
                StandardOutputBo.TypeGrossPremium,
                StandardOutputBo.TypeStandardDiscount,
                StandardOutputBo.TypeSubstandardDiscount,
                StandardOutputBo.TypeVitalityDiscount,
                StandardOutputBo.TypeTotalDiscount,
                StandardOutputBo.TypeNetPremium,
                StandardOutputBo.TypeGrossPremiumAlt,
                StandardOutputBo.TypeNetPremiumAlt,
                StandardOutputBo.TypeTransactionPremium,
                StandardOutputBo.TypeOriginalPremium,
                StandardOutputBo.TypeTransactionDiscount,
                StandardOutputBo.TypeOriginalDiscount,
                StandardOutputBo.TypeNoClaimBonus,
                StandardOutputBo.TypeSurrenderValue,
                StandardOutputBo.TypeDatabaseCommision,
                StandardOutputBo.TypeBrokerageFee,
                StandardOutputBo.TypeAnnualRiPrem,
                StandardOutputBo.TypeRiCovPeriod,
                StandardOutputBo.TypeAdjBeginDate,
                StandardOutputBo.TypeAdjEndDate,
                StandardOutputBo.TypePolicyStatusCode,
                StandardOutputBo.TypePolicyGrossPremium,
                StandardOutputBo.TypePolicyStandardPremium,
                StandardOutputBo.TypePolicySubstandardPremium,
                StandardOutputBo.TypePolicyTermRemain,
                StandardOutputBo.TypePolicyAmountDeath,
                StandardOutputBo.TypePolicyAmountSubstandard,
                StandardOutputBo.TypePolicyReserve,
                StandardOutputBo.TypePolicyPaymentMethod,
                StandardOutputBo.TypePolicyLifeNumber,
                StandardOutputBo.TypeFundCode,
                StandardOutputBo.TypeLineOfBusiness,
                StandardOutputBo.TypeApLoading,
                StandardOutputBo.TypeLoanInterestRate,
                StandardOutputBo.TypeDefermentPeriod,
                StandardOutputBo.TypeNationality,
                StandardOutputBo.TypeTerritoryOfIssueCode,
                StandardOutputBo.TypeCurrencyCode,
                StandardOutputBo.TypeCurrencyRate,
                StandardOutputBo.TypeStaffPlanIndicator,
                StandardOutputBo.TypeCedantSar,
                StandardOutputBo.TypeCedantReinsurerCode,
                StandardOutputBo.TypeCessionCode,
                StandardOutputBo.TypeCedantRemark,
                StandardOutputBo.TypeGroupPolicyNumber,
                StandardOutputBo.TypeGroupPolicyName,
                StandardOutputBo.TypeNoOfEmployee,
                StandardOutputBo.TypePolicyTotalLive,
                StandardOutputBo.TypeGroupSubsidiaryName,
                StandardOutputBo.TypeGroupSubsidiaryNo,
                StandardOutputBo.TypeGroupEmployeeBasicSalary,
                StandardOutputBo.TypeGroupEmployeeJobType,
                StandardOutputBo.TypeGroupEmployeeJobCode,
                StandardOutputBo.TypeGroupEmployeeBasicSalaryRevise,
                StandardOutputBo.TypeGroupEmployeeBasicSalaryMultiplier,
                StandardOutputBo.TypeDependantIndicator,
                StandardOutputBo.TypeGhsRoomBoard,
                StandardOutputBo.TypeLayer1RiShare,
                StandardOutputBo.TypeLayer1InsuredAttainedAge,
                StandardOutputBo.TypeLayer1InsuredAttainedAge2nd,
                StandardOutputBo.TypeLayer1StandardPremium,
                StandardOutputBo.TypeLayer1SubstandardPremium,
                StandardOutputBo.TypeLayer1FlatExtraPremium,
                StandardOutputBo.TypeLayer1GrossPremium,
                StandardOutputBo.TypeLayer1StandardDiscount,
                StandardOutputBo.TypeLayer1SubstandardDiscount,
                StandardOutputBo.TypeLayer1TotalDiscount,
                StandardOutputBo.TypeLayer1NetPremium,
                StandardOutputBo.TypeLayer1GrossPremiumAlt,
                StandardOutputBo.TypeLayer1TotalDiscountAlt,
                StandardOutputBo.TypeLayer1NetPremiumAlt,
                StandardOutputBo.TypeSpecialIndicator1,
                StandardOutputBo.TypeSpecialIndicator2,
                StandardOutputBo.TypeSpecialIndicator3,
                StandardOutputBo.TypeIndicatorJointLife,
                StandardOutputBo.TypeTaxAmount,
                StandardOutputBo.TypeGstIndicator,
                StandardOutputBo.TypeGstGrossPremium,
                StandardOutputBo.TypeGstTotalDiscount,
                StandardOutputBo.TypeGstVitality,
                StandardOutputBo.TypeGstAmount,
                StandardOutputBo.TypeProfitComm,
                StandardOutputBo.TypeRiskPeriodStartDate,
                StandardOutputBo.TypeRiskPeriodEndDate,
                StandardOutputBo.TypeMlreInsuredAttainedAgeAtCurrentMonth,
                StandardOutputBo.TypeMlreInsuredAttainedAgeAtPreviousMonth,
                StandardOutputBo.TypeInsuredAttainedAgeCheck,
                StandardOutputBo.TypeMlrePolicyIssueAge,
                StandardOutputBo.TypePolicyIssueAgeCheck,
                StandardOutputBo.TypeMaxExpiryAge,
                StandardOutputBo.TypeMaxExpiryAgeCheck,
                StandardOutputBo.TypeMinIssueAge,
                StandardOutputBo.TypeMinIssueAgeCheck,
                StandardOutputBo.TypeMaxIssueAge,
                StandardOutputBo.TypeMaxIssueAgeCheck,
                StandardOutputBo.TypeMaxUwRating,
                StandardOutputBo.TypeMaxUwRatingCheck,
                StandardOutputBo.TypeEffectiveDate,
                StandardOutputBo.TypeEffectiveDateCheck,
                StandardOutputBo.TypeAarShare,
                StandardOutputBo.TypeAarCap,
                StandardOutputBo.TypeAarShare2,
                StandardOutputBo.TypeAarCap2,
                StandardOutputBo.TypeAarCheck,
                StandardOutputBo.TypeAbl,
                StandardOutputBo.TypeAblCheck,
                StandardOutputBo.TypeRetentionShare,
                StandardOutputBo.TypeRetentionCap,
                StandardOutputBo.TypeRetentionCheck,
                StandardOutputBo.TypeMinAar,
                StandardOutputBo.TypeMinAarCheck,
                StandardOutputBo.TypeMaxAar,
                StandardOutputBo.TypeMaxAarCheck,
                StandardOutputBo.TypeCorridorLimit,
                StandardOutputBo.TypeCorridorLimitCheck,
                StandardOutputBo.TypeMaxApLoading,
                StandardOutputBo.TypeApLoadingCheck,
                StandardOutputBo.TypeEwarpNumber,
                StandardOutputBo.TypeEwarpActionCode,
                StandardOutputBo.TypeOfferLetterSentDate,
                StandardOutputBo.TypeValidityDayCheck,
                StandardOutputBo.TypeSumAssuredOffered,
                StandardOutputBo.TypeSumAssuredOfferedCheck,
                StandardOutputBo.TypeUwRatingOffered,
                StandardOutputBo.TypeUwRatingCheck,
                StandardOutputBo.TypeFlatExtraAmountOffered,
                StandardOutputBo.TypeFlatExtraAmountCheck,
                StandardOutputBo.TypeFlatExtraDuration,
                StandardOutputBo.TypeFlatExtraDurationCheck,
                StandardOutputBo.TypeRiRate,
                StandardOutputBo.TypeRiRate2,
                StandardOutputBo.TypeRatePerBasisUnit,
                StandardOutputBo.TypeRiDiscountRate,
                StandardOutputBo.TypeLargeSaDiscount,
                StandardOutputBo.TypeGroupSizeDiscount,
                StandardOutputBo.TypeAnnuityFactor,
                StandardOutputBo.TypeMlreStandardPremium,
                StandardOutputBo.TypeMlreSubstandardPremium,
                StandardOutputBo.TypeMlreFlatExtraPremium,
                StandardOutputBo.TypeMlreGrossPremium,
                StandardOutputBo.TypeMlreStandardDiscount,
                StandardOutputBo.TypeMlreSubstandardDiscount,
                StandardOutputBo.TypeMlreLargeSaDiscount,
                StandardOutputBo.TypeMlreGroupSizeDiscount,
                StandardOutputBo.TypeMlreVitalityDiscount,
                StandardOutputBo.TypeMlreTotalDiscount,
                StandardOutputBo.TypeMlreNetPremium,
                StandardOutputBo.TypeNetPremiumCheck,
                StandardOutputBo.TypeMlreBrokerageFee,
                StandardOutputBo.TypeMlreDatabaseCommission,
                StandardOutputBo.TypeWakalahFeePercentage,
                StandardOutputBo.TypeServiceFeePercentage,
                StandardOutputBo.TypeServiceFee,
                StandardOutputBo.TypeTotalDirectRetroAar,
                StandardOutputBo.TypeTotalDirectRetroGrossPremium,
                StandardOutputBo.TypeTotalDirectRetroDiscount,
                StandardOutputBo.TypeTotalDirectRetroNetPremium,
                StandardOutputBo.TypeRateTable,
                StandardOutputBo.TypeMfrs17BasicRider,
                StandardOutputBo.TypeMfrs17CellName,
                StandardOutputBo.TypeMfrs17TreatyCode,
                StandardOutputBo.TypeMfrs17AnnualCohort,
                StandardOutputBo.TypeLoaCode,
                StandardOutputBo.TypeTempD1,
                StandardOutputBo.TypeTempD2,
                StandardOutputBo.TypeTempD3,
                StandardOutputBo.TypeTempD4,
                StandardOutputBo.TypeTempD5,
                StandardOutputBo.TypeTempS1,
                StandardOutputBo.TypeTempS2,
                StandardOutputBo.TypeTempS3,
                StandardOutputBo.TypeTempS4,
                StandardOutputBo.TypeTempS5,
                StandardOutputBo.TypeTempI1,
                StandardOutputBo.TypeTempI2,
                StandardOutputBo.TypeTempI3,
                StandardOutputBo.TypeTempI4,
                StandardOutputBo.TypeTempI5,
                StandardOutputBo.TypeTempA1,
                StandardOutputBo.TypeTempA2,
                StandardOutputBo.TypeTempA3,
                StandardOutputBo.TypeTempA4,
                StandardOutputBo.TypeTempA5,
                StandardOutputBo.TypeTempA6,
                StandardOutputBo.TypeTempA7,
                StandardOutputBo.TypeTempA8,
                StandardOutputBo.TypeRecordType,
            };
        }

        public static List<Column> GetColumns()
        {
            var columns = new List<Column>
            {
                new Column
                {
                    Header = "ID",
                    Property = "Id",
                },
                new Column
                {
                    Header = "Mapping Status",
                    Property = "MappingStatus",
                },
                new Column
                {
                    Header = "Pre-Computation 1 Status",
                    Property = "PreComputation1Status",
                },
                new Column
                {
                    Header = "Pre-Computation 2 Status",
                    Property = "PreComputation2Status",
                },
                new Column
                {
                    Header = "Pre-Validation Status",
                    Property = "PreValidationStatus",
                },
                new Column
                {
                    Header = "Conflict",
                    Property = "ConflictType",
                },
                new Column
                {
                    Header = "Post Computation Status",
                    Property = "PostComputationStatus",
                },
                new Column
                {
                    Header = "Post Validation Status",
                    Property = "PostValidationStatus",
                },
                new Column
                {
                    Header = "Finalise Status",
                    Property = "FinaliseStatus",
                },
            };

            // add all standard fields
            foreach (int i in ColumnSequence())
            {
                columns.Add(new Column
                {
                    Header = StandardOutputBo.GetCodeByType(i),
                    Property = StandardOutputBo.GetPropertyNameByType(i),
                });
            }

            columns.Add(new Column
            {
                Header = "Errors",
                Property = "Errors",
            });

            return columns;
        }

        public static string GetMappingStatusName(int? key)
        {
            switch (key)
            {
                case MappingStatusPending:
                    return "Pending";
                case MappingStatusSuccess:
                    return "Success";
                case MappingStatusFailed:
                    return "Failed";
                default:
                    return "";
            }
        }

        public static string GetPreComputation1StatusName(int? key)
        {
            switch (key)
            {
                case PreComputation1StatusPending:
                    return "Pending";
                case PreComputation1StatusSuccess:
                    return "Success";
                case PreComputation1StatusFailed:
                    return "Failed";
                default:
                    return "";
            }
        }

        public static string GetPreComputation2StatusName(int? key)
        {
            switch (key)
            {
                case PreComputation2StatusPending:
                    return "Pending";
                case PreComputation2StatusSuccess:
                    return "Success";
                case PreComputation2StatusFailed:
                    return "Failed";
                default:
                    return "";
            }
        }

        public static string GetPreValidationStatusName(int? key)
        {
            switch (key)
            {
                case PreValidationStatusPending:
                    return "Pending";
                case PreValidationStatusSuccess:
                    return "Success";
                case PreValidationStatusFailed:
                    return "Failed";
                default:
                    return "";
            }
        }

        public static string GetPostComputationStatusName(int? key)
        {
            switch (key)
            {
                case PostComputationStatusPending:
                    return "Pending";
                case PostComputationStatusSuccess:
                    return "Success";
                case PostComputationStatusFailed:
                    return "Failed";
                default:
                    return "";
            }
        }

        public static string GetPostValidationStatusName(int? key)
        {
            switch (key)
            {
                case PostValidationStatusPending:
                    return "Pending";
                case PostValidationStatusSuccess:
                    return "Success";
                case PostValidationStatusFailed:
                    return "Failed";
                default:
                    return "";
            }
        }

        public static string GetFinaliseStatusName(int? key)
        {
            switch (key)
            {
                case FinaliseStatusPending:
                    return "Pending";
                case FinaliseStatusSuccess:
                    return "Success";
                case FinaliseStatusFailed:
                    return "Failed";
                default:
                    return "";
            }
        }

        public static string GetProcessWarehouseStatusName(int? key)
        {
            switch (key)
            {
                case ProcessWarehouseStatusPending:
                    return "Pending";
                case ProcessWarehouseStatusSuccess:
                    return "Success";
                case ProcessWarehouseStatusFailed:
                    return "Failed";
                default:
                    return "";
            }
        }

        public static string GetRecordTypeName(int key)
        {
            switch (key)
            {
                case RecordTypeNew:
                    return "New";
                case RecordTypeAdjustment:
                    return "Adjustment";
                default:
                    return "";
            }
        }

        public static List<int> GetConflictTypeOrder()
        {
            return new List<int>()
            {
                ConflictTypeGenderCountryWithinBatch,
                ConflictTypeGenderCountryWithinWarehouse,
                ConflictTypeGenderWithinBatchCountryWithinWarehouse,
                ConflictTypeGenderWithinWarehouseCountryWithinBatch,
                ConflictTypeGenderWithinBatch,
                ConflictTypeCountryWithinBatch,
                ConflictTypeGenderWithinWarehouse,
                ConflictTypeCountryWithinWarehouse,
            };
        }

        public static string GetConflictTypeName(int key)
        {
            switch (key)
            {
                case ConflictTypeGenderWithinBatch:
                    return "Insured Gender Code conflict within same batch";
                case ConflictTypeCountryWithinBatch:
                    return "Territory Issue Code conflict within same batch";
                case ConflictTypeGenderWithinWarehouse:
                    return "Insured Gender Code conflict with warehouse";
                case ConflictTypeCountryWithinWarehouse:
                    return "Territory Issue Code conflict with warehouse";
                case ConflictTypeGenderCountryWithinBatch:
                    return "Insured Gender Code & Territory Issue Code conflict within same batch";
                case ConflictTypeGenderCountryWithinWarehouse:
                    return "Insured Gender Code & Territory Issue Code conflict with warehouse";
                case ConflictTypeGenderWithinBatchCountryWithinWarehouse:
                    return "Insured Gender Code conflict within same batch & Territory Issue Code conflict with warehouse";
                case ConflictTypeGenderWithinWarehouseCountryWithinBatch:
                    return "Insured Gender Code conflict with warehouse & Territory Issue Code conflict within same batch";
                default:
                    return "";
            }
        }

        public static bool IsPropertyExists(string name)
        {
            return typeof(RiDataBo).GetProperty(name) != null;
        }

        public string GetCombinations(List<int> types)
        {
            var combinations = new List<string> { };
            foreach (var type in types)
            {
                var value = this.GetPropertyValue(StandardOutputBo.GetPropertyNameByType(type));
                if (value != null)
                    combinations.Add(value.ToString().Trim());
                else
                    combinations.Add("");
            }
            return string.Join("|", combinations);
        }

        public string GetTreatyCombination()
        {
            // Required fields
            // - TypeCedingPlanCode
            // - TypeCedingBenefitTypeCode

            var types = new List<int>
            {
                StandardOutputBo.TypeCedingPlanCode,
                StandardOutputBo.TypeCedingBenefitTypeCode,
                StandardOutputBo.TypeCedingBenefitRiskCode,
                StandardOutputBo.TypeCedingTreatyCode,
                StandardOutputBo.TypeCampaignCode,
                StandardOutputBo.TypeReinsBasisCode,
            };
            return GetCombinations(types);
        }

        public string GetBenefitCombination()
        {
            // Required fields
            // - TypeCedingPlanCode
            // - TypeCedingBenefitTypeCode

            var types = new List<int>
            {
                StandardOutputBo.TypeCedingPlanCode,
                StandardOutputBo.TypeCedingBenefitTypeCode,
                StandardOutputBo.TypeCedingBenefitRiskCode,
            };
            return string.Join("|", GetCombinations(types));
        }

        public string GetProductFeatureCombination()
        {
            var types = new List<int>
            {
                StandardOutputBo.TypeCedingPlanCode,
                StandardOutputBo.TypeCampaignCode,
            };
            return string.Join("|", GetCombinations(types));
        }

        public string GetCellCombination()
        {
            var types = new List<int>
            {
                StandardOutputBo.TypeCedingPlanCode,
                StandardOutputBo.TypeMlreBenefitCode,
                StandardOutputBo.TypeReinsBasisCode,
                StandardOutputBo.TypeTreatyCode,
            };
            return GetCombinations(types);
        }

        public string GetRateTableCombination()
        {
            var types = new List<int>
            {
                StandardOutputBo.TypeTreatyCode,
                StandardOutputBo.TypeCedingPlanCode,
                StandardOutputBo.TypeCedingTreatyCode,
                StandardOutputBo.TypeCedingPlanCode2,
                StandardOutputBo.TypeCedingBenefitTypeCode,
                StandardOutputBo.TypeCedingBenefitRiskCode,
                StandardOutputBo.TypeGroupPolicyNumber,
                StandardOutputBo.TypeMlreBenefitCode,
                StandardOutputBo.TypePremiumFrequencyCode,
                StandardOutputBo.TypeReinsBasisCode,
            };
            return GetCombinations(types);
        }

        public string GetAnnuityFactorCombination()
        {
            var types = new List<int>
            {
                StandardOutputBo.TypeCedingPlanCode,
            };
            return GetCombinations(types);
        }

        // For display data only (detail table not exist)
        public string GetRiDataLookupCombination()
        {
            var types = new List<int>
            {
                StandardOutputBo.TypePolicyNumber,
                StandardOutputBo.TypeCedingPlanCode,
                StandardOutputBo.TypeRiskPeriodMonth,
                StandardOutputBo.TypeRiskPeriodYear,
                StandardOutputBo.TypeMlreBenefitCode,
                StandardOutputBo.TypeTreatyCode,
                StandardOutputBo.TypeRiderNumber,
            };
            return GetCombinations(types);
        }

        public string GetFacMappingCombination()
        {
            var types = new List<int>
            {
                StandardOutputBo.TypePolicyNumber,
                StandardOutputBo.TypeInsuredName,
                StandardOutputBo.TypeMlreBenefitCode,
            };
            return string.Join("|", GetCombinations(types));
        }

        public void FormatYearMonth(string year, string month, out DateTime? date, out string error)
        {
            error = "";

            if (string.IsNullOrEmpty(year) && (string.IsNullOrEmpty(month) || month.Equals("00")))
            {
                date = null;
                return;
            }

            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                date = DateTime.ParseExact(string.Format("{0}{1}", year, month), "yyyyMM", provider);
            }
            catch (Exception e)
            {
                date = null;
                error = e.Message;
            }
        }

        public void GetReportPeriodDate(out DateTime? date, out string error)
        {
            string year = ReportPeriodYear.ToString();
            string month = ReportPeriodMonth.ToString().PadLeft(2, '0');
            FormatYearMonth(year, month, out date, out error);
        }

        public void GetRiskPeriodDate(out DateTime? date, out string error)
        {
            string year = RiskPeriodYear.ToString();
            string month = RiskPeriodMonth.ToString().PadLeft(2, '0');
            FormatYearMonth(year, month, out date, out error);
        }

        public void GetRiskPeriodStartEndDate(int option, out DateTime? startDate, out DateTime? endDate)
        {
            startDate = null;
            endDate = null;

            int newMonth = RiskPeriodMonth.Value;

            // Updated 2020-02-19: Both Option start start month won't affected by Premium Frequency Code
            //if (option == RiDataComputationBo.RiskDateOption1StartDate || option == RiDataComputationBo.RiskDateOption1EndDate)
            //{
            //    switch (PremiumFrequencyCode)
            //    {
            //        case PickListDetailBo.PremiumFrequencyCodeQuarter:
            //            QuarterObject quarterObject = new QuarterObject(RiskPeriodMonth.Value, RiskPeriodYear.Value);
            //            newMonth = quarterObject.MonthStart;
            //            break;
            //        case PickListDetailBo.PremiumFrequencyCodeSemiAnnual:
            //            newMonth = newMonth < 7 ? 1 : 7;
            //            break;
            //        case PickListDetailBo.PremiumFrequencyCodeAnnual:
            //            newMonth = 1;
            //            break;
            //    }
            //}

            var maxDay = DateTime.DaysInMonth(RiskPeriodYear.Value, newMonth);
            bool isDayExceeded = ReinsEffDatePol.Value.Day > maxDay;
            DateTime start = new DateTime(RiskPeriodYear.Value, newMonth, isDayExceeded ? maxDay : ReinsEffDatePol.Value.Day);
            DateTime end = new DateTime(RiskPeriodYear.Value, newMonth, isDayExceeded ? maxDay : ReinsEffDatePol.Value.Day);

            switch (option)
            {
                case RiDataComputationBo.RiskDateOption1StartDate:
                case RiDataComputationBo.RiskDateOption1EndDate:
                    // opt 1
                    // RiskPeriodStartDate = 
                    //   take ReinsEffDatePol (day) 
                    //   take RiskPeriodMonth (month)
                    //   take RiskPeriodYear (month)
                    // e.g.
                    //   ReinsEffDatePol = 2020 Oct 13
                    //   RiskPeriodMonth = Nov
                    //   RiskPeriodYear = 2020
                    // result:
                    //   RiskPeriodStartDate = 2020 Nov 13

                    // RiskPeriodEndDate = 
                    // RiskPeriodStartDate + PremiumFrequencyCode
                    // M = Monthly
                    // Q = Quarter
                    // S = Semi Annual
                    // A = Annual
                    // e.g.
                    //   PremiumFrequencyCode = Q
                    //   RiskPeriodStartDate = 2020 Nov 13
                    // result:
                    //   RiskPeriodEndDate = 2021 Feb 12

                    switch (PremiumFrequencyCode)
                    {
                        case PickListDetailBo.PremiumFrequencyCodeMonthly:
                            end = end.AddMonths(1).AddDays(-1);
                            break;
                        case PickListDetailBo.PremiumFrequencyCodeQuarter:
                            end = end.AddMonths(3).AddDays(-1);
                            break;
                        case PickListDetailBo.PremiumFrequencyCodeSemiAnnual:
                            end = end.AddMonths(6).AddDays(-1);
                            break;
                        case PickListDetailBo.PremiumFrequencyCodeAnnual:
                            end = end.AddMonths(12).AddDays(-1);
                            break;
                    }

                    if (isDayExceeded)
                    {
                        var maxEndDay = DateTime.DaysInMonth(end.Year, end.Month);
                        end = new DateTime(end.Year, end.Month, (ReinsEffDatePol.Value.Day - 1) > maxEndDay ? maxEndDay : ReinsEffDatePol.Value.Day - 1);
                    }

                    startDate = start;
                    endDate = end;
                    break;
                case RiDataComputationBo.RiskDateOption2StartDate:
                case RiDataComputationBo.RiskDateOption2EndDate:
                    // opt 2
                    // RiskPeriodStartDate = 
                    //   take ReinsEffDatePol (day) 
                    //   take RiskPeriodMonth (month)
                    //   take RiskPeriodYear (month)
                    // e.g.
                    //   ReinsEffDatePol = 2020 Oct 13
                    //   RiskPeriodMonth = Nov
                    //   RiskPeriodYear = 2020
                    // result:
                    //   RiskPeriodStartDate = 2020 Nov 1

                    // RiskPeriodEndDate = 
                    // RiskPeriodStartDate + PremiumFrequencyCode
                    // M = Monthly
                    // Q = Quarter
                    // S = Semi Annual
                    // A = Annual
                    // e.g.
                    //   PremiumFrequencyCode = Q
                    //   RiskPeriodStartDate = 2020 Nov 1
                    // result:
                    //   RiskPeriodEndDate = 2021 Feb 28 (Incorrect) --> 2021 Jan 31 (Correct)

                    start = new DateTime(start.Year, start.Month, 1);
                    end = new DateTime(end.Year, end.Month, 1);
                    switch (PremiumFrequencyCode)
                    {
                        case PickListDetailBo.PremiumFrequencyCodeMonthly:
                            end = end.AddMonths(1).AddDays(-1);
                            break;
                        case PickListDetailBo.PremiumFrequencyCodeQuarter:
                            end = end.AddMonths(3).AddDays(-1);
                            break;
                        case PickListDetailBo.PremiumFrequencyCodeSemiAnnual:
                            end = end.AddMonths(6).AddDays(-1);
                            break;
                        case PickListDetailBo.PremiumFrequencyCodeAnnual:
                            end = end.AddMonths(12).AddDays(-1);
                            break;
                    }

                    startDate = start;
                    endDate = end;
                    break;
            }
        }

        public void SetError(string property, dynamic value)
        {
            ErrorDictionary[property] = value;
            Errors = JsonConvert.SerializeObject(ErrorObject);
        }

        public bool SetCustomField(object value, RiDataMappingBo mapping)
        {
            if (value == null)
                CustomFieldDictionary[mapping.RawColumnName] = null;
            else
                CustomFieldDictionary[mapping.RawColumnName] = value.ToString();
            CustomField = JsonConvert.SerializeObject(CustomFieldObject);
            return true;
        }

        public bool SetRiData(int datatype, string property, object value, RiDataMappingBo mapping = null)
        {
            if (value is string valueStr)
                value = valueStr.Trim();
            switch (datatype)
            {
                // DropDown can be null or empty to do mapping with the PickListDetails
                case StandardOutputBo.DataTypeDropDown:
                    break;
                default:
                    if (value == null || value is string @string && string.IsNullOrEmpty(@string))
                    {
                        this.SetPropertyValue(property, null);
                        return true;
                    }
                    break;
            }

            switch (datatype)
            {
                case StandardOutputBo.DataTypeDate:
                    return SetDate(property, value, mapping);
                case StandardOutputBo.DataTypeString:
                    return SetString(property, value, mapping);
                case StandardOutputBo.DataTypeAmount:
                    return SetAmount(property, value, mapping);
                case StandardOutputBo.DataTypePercentage:
                    return SetPercentange(property, value, mapping);
                case StandardOutputBo.DataTypeInteger:
                    return SetInteger(property, value, mapping);
                case StandardOutputBo.DataTypeDropDown:
                    return SetDropDown(property, value, mapping);
                case StandardOutputBo.DataTypeBoolean:
                    return SetBoolean(property, value, mapping);
            }
            return false;
        }

        public bool SetDate(string property, object value, RiDataMappingBo mapping = null)
        {
            DateTime? date = null;
            if (mapping != null)
            {
                if (mapping.TransformFormula == RiDataMappingBo.TransformFormulaFixedValue)
                {
                    // QUESTION: what is user input date format? 2020-06-24? 20200624? 2020/06/24?
                    date = DateTime.Parse(mapping.DefaultValue);
                    this.SetPropertyValue(property, date);
                    return true;
                }

                switch (value.GetType().Name)
                {
                    case "String":
                        if (mapping.TransformFormula == RiDataMappingBo.TransformFormulaInputFormat)
                        {
                            CultureInfo provider = CultureInfo.InvariantCulture;
                            date = DateTime.ParseExact((string)value, mapping.DefaultValue, provider);
                        }
                        else
                        {
                            date = DateTime.Parse((string)value);
                        }
                        break;
                    case "Double":
                        if (mapping.TransformFormula == RiDataMappingBo.TransformFormulaInputFormat)
                        {
                            CultureInfo provider = CultureInfo.InvariantCulture;
                            date = DateTime.ParseExact(value.ToString(), mapping.DefaultValue, provider);
                        }
                        else
                        {
                            // This is for excel format double variable type
                            date = DateTime.FromOADate((double)value);
                        }
                        break;
                    case "DateTime":
                        date = (DateTime)value;
                        break;
                    default:
                        break;
                }
                this.SetPropertyValue(property, date);
            }
            else
            {
                date = DateTime.Parse(value.ToString());
                this.SetPropertyValue(property, date);
            }
            return true;
        }

        public bool SetString(string property, object value, RiDataMappingBo mapping = null)
        {
            string s = value?.ToString();
            string output;
            if (mapping != null && !string.IsNullOrEmpty(s))
            {
                if (mapping.TransformFormula == RiDataMappingBo.TransformFormulaSubstring)
                {
                    if (string.IsNullOrEmpty(mapping.DefaultValue))
                    {
                        throw new Exception("Substring params is empty");
                    }

                    mapping.DefaultValue.GetSubStringParams(out int start, out int subStringLength);
                    if (s.Length > start)
                    {
                        string sub;
                        if (subStringLength == 0)
                            sub = s.Substring(start);
                        else
                            sub = s.Substring(start, subStringLength);
                        output = sub;
                    }
                    else
                    {
                        throw new Exception(string.Format(MessageBag.SubStringPosition, start, s.Length));
                    }
                }
                else if (mapping.TransformFormula == RiDataMappingBo.TransformFormulaFixedValue)
                {
                    output = mapping.DefaultValue;
                }
                else
                {
                    output = s;
                }
            }
            else
            {
                output = s;
            }

            if (output != null)
            {
                int length = output.Length;
                var maxLengthAttr = this.GetAttributeFrom<MaxLengthAttribute>(property);
                if (maxLengthAttr != null && length > 0 && length > maxLengthAttr.Length)
                {
                    throw new Exception(string.Format(MessageBag.MaxLength, length, maxLengthAttr.Length));
                }
            }
            this.SetPropertyValue(property, output);

            return true;
        }

        public bool SetAmount(string property, object value, RiDataMappingBo mapping = null)
        {
            string s = value.ToString();
            if (mapping != null)
            {
                if (mapping.TransformFormula == RiDataMappingBo.TransformFormulaFixedValue)
                {
                    if (Util.IsValidDouble(mapping.DefaultValue, out double? output, out string error, true))
                    {
                        this.SetPropertyValue(property, output);
                    }
                    else
                    {
                        throw new Exception(error);
                    }
                    return true;
                }

                if (mapping.TransformFormula == RiDataMappingBo.TransformFormulaDivide100)
                {
                    if (Util.IsValidDouble(s, out double? output, out string error))
                    {
                        if (output.HasValue)
                        {
                            double d = Util.RoundValue(output.Value / 100);
                            this.SetPropertyValue(property, d);
                        }
                    }
                    else
                    {
                        throw new Exception(error);
                    }
                }
                else
                {
                    if (Util.IsValidDouble(s, out double? output, out string error, true))
                    {
                        this.SetPropertyValue(property, output);
                    }
                    else
                    {
                        throw new Exception(error);
                    }
                }
            }
            else
            {
                if (Util.IsValidDouble(s, out double? output, out string error, true))
                {
                    this.SetPropertyValue(property, output);
                }
                else
                {
                    throw new Exception(error);
                }
            }

            return true;
        }

        public bool SetPercentange(string property, object value, RiDataMappingBo mapping = null)
        {
            string s = value.ToString();
            if (mapping != null)
            {
                if (mapping.TransformFormula == RiDataMappingBo.TransformFormulaFixedValue)
                {
                    // QUESTION: what is user input format? 90%? 80 (80%)? 10.50%? 10.50 (10.50%)?
                    if (Util.IsValidDouble(mapping.DefaultValue, out double? d, out string e, true))
                    {
                        this.SetPropertyValue(property, d);
                    }
                    else
                    {
                        throw new Exception(e);
                    }
                    return true;
                }

                s = s.Replace('%', '\0');
                if (Util.IsValidDouble(s, out double? output, out string error, true))
                {
                    this.SetPropertyValue(property, output);
                }
                else
                {
                    throw new Exception(error);
                }
            }
            else
            {
                if (Util.IsValidDouble(s, out double? output, out string error, true))
                {
                    this.SetPropertyValue(property, output);
                }
                else
                {
                    throw new Exception(error);
                }
            }
            return true;
        }

        public bool SetInteger(string property, object value, RiDataMappingBo mapping = null)
        {
            string s = value.ToString();
            if (mapping != null)
            {
                if (mapping.TransformFormula == RiDataMappingBo.TransformFormulaSubstring)
                {
                    if (string.IsNullOrEmpty(mapping.DefaultValue))
                    {
                        throw new Exception("Substring params is empty");
                    }

                    mapping.DefaultValue.GetSubStringParams(out int start, out int subStringLength);
                    if (s.Length > start)
                    {
                        if (subStringLength == 0)
                            s = s.Substring(start);
                        else
                            s = s.Substring(start, subStringLength);

                        if (Int32.TryParse(s, out int integer))
                        {
                            this.SetPropertyValue(property, integer);
                        }
                        else
                        {
                            throw new Exception("The value is not numeric: " + s.ToString());
                        }
                    }
                    else
                    {
                        throw new Exception(string.Format(MessageBag.SubStringPosition, start, s.Length));
                    }
                }
                else if (mapping.TransformFormula == RiDataMappingBo.TransformFormulaFixedValue)
                {
                    double d = double.Parse(mapping.DefaultValue);
                    this.SetPropertyValue(property, Convert.ToInt32(d));
                }
                else
                {
                    double d = double.Parse(s);
                    this.SetPropertyValue(property, Convert.ToInt32(d));
                }
            }
            else
            {
                this.SetPropertyValue(property, int.Parse(s));
            }
            return true;
        }

        public bool SetDropDown(string property, object value, RiDataMappingBo mapping = null)
        {
            string s = value?.ToString();
            string output;
            if (mapping != null)
            {
                if (mapping.TransformFormula == RiDataMappingBo.TransformFormulaFixedValue)
                {
                    output = mapping.DefaultValue;
                }
                else if (mapping.RiDataMappingDetailBos != null && mapping.RiDataMappingDetailBos.Count > 0)
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        var emptyRawValues = mapping.RiDataMappingDetailBos.Where(q => q.IsRawValueEmpty).ToList();
                        if (emptyRawValues.Count > 0)
                        {
                            if (emptyRawValues.Count == 1)
                            {
                                var emptyRawValue = emptyRawValues[0];
                                if (emptyRawValue.IsPickDetailIdEmpty)
                                    output = null;
                                else
                                    output = emptyRawValue.PickListDetailBo.Code;
                            }
                            else
                            {
                                throw new Exception("The Empty Raw Value Details more than one");
                            }
                        }
                        else
                        {
                            output = null;
                        }
                    }
                    else
                    {
                        var detailBos = mapping.RiDataMappingDetailBos.Where(q => !q.IsRawValueEmpty && q.RawValue.ToLower() == s.ToLower()).ToList();
                        if (detailBos.Count > 0)
                        {
                            if (detailBos.Count == 1)
                            {
                                var detailBo = detailBos[0];
                                if (detailBo.IsPickDetailIdEmpty)
                                    output = null;
                                else
                                    output = detailBo.PickListDetailBo.Code;
                            }
                            else
                            {
                                throw new Exception("The Mapping Raw Value Details more than one");
                            }
                        }
                        else
                        {
                            throw new Exception(string.Format("The Raw Value not found in Mapping Detail: {0}", s ?? Util.Null));
                        }
                    }
                }
                else
                {
                    throw new Exception("No mapping detail configured");
                }
            }
            else
            {
                output = s;
            }

            if (output != null)
            {
                int length = output.Length;
                var maxLengthAttr = this.GetAttributeFrom<MaxLengthAttribute>(property);
                if (maxLengthAttr != null && length > 0 && length > maxLengthAttr.Length)
                {
                    throw new Exception(string.Format(MessageBag.MaxLength, length, maxLengthAttr.Length));
                }
            }
            this.SetPropertyValue(property, output);

            return true;
        }

        public bool SetBoolean(string property, object value, RiDataMappingBo mapping = null)
        {
            string s = value?.ToString();
            bool? bl;
            if (mapping != null)
            {
                if (mapping.TransformFormula == RiDataMappingBo.TransformFormulaFixedValue)
                {
                    bl = bool.Parse(mapping.DefaultValue);
                    this.SetPropertyValue(property, bl);
                    return true;
                }
                else
                {
                    if (bool.TryParse(s, out bool b))
                    {
                        bl = b;
                    }
                    else
                    {
                        bl = null;
                    }
                    this.SetPropertyValue(property, bl);
                }
            }
            else
            {
                if (bool.TryParse(s, out bool b))
                {
                    bl = b;
                }
                else
                {
                    bl = null;
                }
                this.SetPropertyValue(property, bl);
            }
            return true;
        }

        public bool ProcessRiData(RiDataMappingBo mapping, object value, out StandardOutputBo so, out object before, out object after, out string error)
        {
            var success = true;
            so = null;
            before = null;
            after = value;
            error = null;
            if (mapping == null)
                return false;
            so = mapping.GetStandardOutputBo();
            if (so == null)
                return false;

            before = this.GetPropertyValue(so.Property);
            if (so.Type == StandardOutputBo.TypeCustomField)
            {
                SetCustomField(value, mapping);
                return success;
            }

            try
            {
                SetRiData(so.DataType, so.Property, value, mapping);
            }
            catch (Exception e)
            {
                success = false;
                MappingValidate = false;
                SetError(so.Property, string.Format("Mapping Error: {0}", e.Message));
            }
            return success;
        }

        public string FormatTreatyMappingError(int type, string msg)
        {
            return FormatTreatyMappingError(StandardOutputBo.GetTypeName(type), msg);
        }

        public string FormatTreatyMappingError(string field, string msg)
        {
            return string.Format("{0}{1}: {2}", TreatyCodeMappingTitle, field, msg);
        }

        public string FormatTreatyMappingError(string msg)
        {
            return string.Format("{0}{1}", TreatyCodeMappingTitle, msg);
        }

        public static List<int> ParamsTreatyMapping()
        {
            return new List<int> {
                StandardOutputBo.TypeCedingPlanCode,
                StandardOutputBo.TypeCedingBenefitTypeCode, // optional field
                StandardOutputBo.TypeCedingBenefitRiskCode, // optional field
                StandardOutputBo.TypeCedingTreatyCode, // optional field
                StandardOutputBo.TypeCampaignCode, // optional field
                StandardOutputBo.TypeReinsEffDatePol,
                StandardOutputBo.TypeReinsBasisCode, // optional field

                // Updated on 20210119
                StandardOutputBo.TypeInsuredAttainedAge, // optional field
                StandardOutputBo.TypeReportPeriodMonth, // optional field
                StandardOutputBo.TypeReportPeriodYear, // optional field
                StandardOutputBo.TypeUnderwriterRating, // optional field
                StandardOutputBo.TypeOriSumAssured, // optional field

                // Updated on 20210311
                StandardOutputBo.TypeReinsuranceIssueAge, // optional field
            };
        }

        public List<string> ValidateTreatyMapping(out DateTime? reportDate)
        {
            // Mapping Columns
            var required = new List<int>
            {
                StandardOutputBo.TypeCedingPlanCode,
                StandardOutputBo.TypeCedingBenefitTypeCode,
                //StandardOutputBo.TypeCedingBenefitRiskCode, // optional field
                //StandardOutputBo.TypeCedingTreatyCode, // optional field
                //StandardOutputBo.TypeCampaignCode, // optional field
                StandardOutputBo.TypeReinsEffDatePol,
                //StandardOutputBo.TypeReinsBasisCode, // optional field

                // Updated on 20210119
                //StandardOutputBo.TypeInsuredAttainedAge, // optional field
                //StandardOutputBo.TypeReportPeriodMonth, // optional field
                //StandardOutputBo.TypeReportPeriodYear, // optional field
                //StandardOutputBo.TypeUnderwriterRating, // optional field
                //StandardOutputBo.TypeOriSumAssured, // optional field

                // Updated on 20210311
                //StandardOutputBo.TypeReinsuranceIssueAge, // optional field
            };

            var errors = new List<string> { };
            foreach (int type in required)
            {
                var empty = FormatTreatyMappingError(type, "Empty");
                var property = StandardOutputBo.GetPropertyNameByType(type);
                var value = this.GetPropertyValue(property);
                if (value == null)
                    errors.Add(empty);
                else if (value is string @string && string.IsNullOrEmpty(@string))
                    errors.Add(empty);
            }

            GetReportPeriodDate(out reportDate, out string reportDateError);
            if (!string.IsNullOrEmpty(reportDateError))
                errors.Add(FormatTreatyMappingError("Report Period Date", reportDateError));

            TreatyCodeMappingValidate = errors.Count == 0;
            return errors;
        }

        public string FormatTreatyNumberTreatyTypeMappingError(int type, string msg)
        {
            return FormatTreatyNumberTreatyTypeMappingError(StandardOutputBo.GetTypeName(type), msg);
        }

        public string FormatTreatyNumberTreatyTypeMappingError(string field, string msg)
        {
            return string.Format("{0}{1}: {2}", TreatyNumberTreatyTypeMappingTitle, field, msg);
        }

        public string FormatTreatyNumberTreatyTypeMappingError(string msg)
        {
            return string.Format("{0}{1}", TreatyNumberTreatyTypeMappingTitle, msg);
        }

        public static List<int> ParamsTreatyNumberTreatyTypeMapping()
        {
            return new List<int> {
                StandardOutputBo.TypeTreatyCode,
            };
        }

        public List<string> ValidateTreatyNumberTreatyTypeMapping()
        {
            // Mapping Columns
            var required = new List<int>
            {
                StandardOutputBo.TypeTreatyCode,
            };

            var errors = new List<string> { };
            foreach (int type in required)
            {
                var empty = FormatTreatyNumberTreatyTypeMappingError(type, "Empty");
                var property = StandardOutputBo.GetPropertyNameByType(type);
                var value = this.GetPropertyValue(property);
                if (value == null)
                    errors.Add(empty);
                else if (value is string @string && string.IsNullOrEmpty(@string))
                    errors.Add(empty);
            }

            TreatyNumberMappingValidate = errors.Count == 0;
            return errors;
        }

        public string FormatBenefitMappingError(int type, string msg)
        {
            return FormatBenefitMappingError(StandardOutputBo.GetTypeName(type), msg);
        }

        public string FormatBenefitMappingError(string field, string msg)
        {
            return string.Format("{0}{1}: {2}", BenefitCodeMappingTitle, field, msg);
        }

        public string FormatBenefitMappingError(string msg)
        {
            return string.Format("{0}{1}", BenefitCodeMappingTitle, msg);
        }

        public static List<int> ParamsBenefitMapping()
        {
            return new List<int> {
                StandardOutputBo.TypeCedingPlanCode,
                StandardOutputBo.TypeCedingBenefitTypeCode,
                StandardOutputBo.TypeTreatyCode,
                StandardOutputBo.TypeCedingBenefitRiskCode, // optional field
                StandardOutputBo.TypeInsuredAttainedAge, // optional field
                StandardOutputBo.TypeReportPeriodMonth, // optional field
                StandardOutputBo.TypeReportPeriodYear, // optional field

                // Updated on 20210119
                StandardOutputBo.TypeReinsEffDatePol,
                StandardOutputBo.TypeCedingTreatyCode, // optional field
                StandardOutputBo.TypeCampaignCode, // optional field
                StandardOutputBo.TypeReinsBasisCode, // optional field
                StandardOutputBo.TypeUnderwriterRating, // optional field
                StandardOutputBo.TypeOriSumAssured, // optional field

                // Updated on 20210311
                StandardOutputBo.TypeReinsuranceIssueAge, // optional field
            };
        }

        public List<string> ValidateBenefitMapping(out DateTime? reportDate)
        {
            // Mapping Columns
            var required = new List<int>
            {
                StandardOutputBo.TypeCedingPlanCode,
                StandardOutputBo.TypeCedingBenefitTypeCode,
                StandardOutputBo.TypeTreatyCode,
                //StandardOutputBo.TypeCedingBenefitRiskCode, // optional field
                //StandardOutputBo.TypeInsuredAttainedAge, // optional field
                //StandardOutputBo.TypeReportPeriodMonth, // optional field
                //StandardOutputBo.TypeReportPeriodYear, // optional field

                // Updated on 20200119
                StandardOutputBo.TypeReinsEffDatePol,
                //StandardOutputBo.TypeCedingTreatyCode, // optional field
                //StandardOutputBo.TypeCampaignCode, // optional field
                //StandardOutputBo.TypeReinsBasisCode, // optional field
                //StandardOutputBo.TypeUnderwriterRating, // optional field
                //StandardOutputBo.TypeOriSumAssured, // optional field

                // Updated on 20210311
                //StandardOutputBo.TypeReinsuranceIssueAge, // optional field
            };

            var errors = new List<string> { };
            foreach (int type in required)
            {
                var empty = FormatTreatyMappingError(type, "Empty");
                var property = StandardOutputBo.GetPropertyNameByType(type);
                var value = this.GetPropertyValue(property);
                if (value == null)
                    errors.Add(empty);
                else if (value is string @string && string.IsNullOrEmpty(@string))
                    errors.Add(empty);
            }

            GetReportPeriodDate(out reportDate, out string reportDateError);
            if (!string.IsNullOrEmpty(reportDateError))
                errors.Add(FormatBenefitMappingError("Report Period Date", reportDateError));

            BenefitCodeMappingValidate = errors.Count == 0;
            return errors;
        }

        public string FormatProductFeatureMappingError(int type, string msg)
        {
            return FormatProductFeatureMappingError(StandardOutputBo.GetTypeName(type), msg);
        }

        public string FormatProductFeatureMappingError(string field, string msg)
        {
            return string.Format("{0}{1}: {2}", ProductFeatureMappingTitle, field, msg);
        }

        public string FormatProductFeatureMappingError(string msg)
        {
            return string.Format("{0}{1}", ProductFeatureMappingTitle, msg);
        }

        public static List<int> ParamsProductFeatureMapping()
        {
            return new List<int> {
                StandardOutputBo.TypeCedingPlanCode,
                StandardOutputBo.TypeReinsEffDatePol,
                StandardOutputBo.TypeCampaignCode, // optional field
                StandardOutputBo.TypeInsuredAttainedAge, // optional field
                StandardOutputBo.TypeTreatyCode,
                StandardOutputBo.TypeMlreBenefitCode,
                StandardOutputBo.TypeUnderwriterRating, // optional field
                StandardOutputBo.TypeOriSumAssured, // optional field

                // Updated on 20210119
                StandardOutputBo.TypeCedingBenefitTypeCode,
                StandardOutputBo.TypeCedingBenefitRiskCode, // optional field
                StandardOutputBo.TypeReportPeriodMonth, // optional field
                StandardOutputBo.TypeReportPeriodYear, // optional field
                StandardOutputBo.TypeCedingTreatyCode, // optional field
                StandardOutputBo.TypeReinsBasisCode, // optional field

                // Updated on 20210311
                StandardOutputBo.TypeReinsuranceIssueAge, // optional field
            };
        }

        public List<string> ValidateProductFeatureMapping(out DateTime? reportDate)
        {
            // Mapping Columns
            var required = new List<int>
            {
                StandardOutputBo.TypeCedingPlanCode,
                StandardOutputBo.TypeReinsEffDatePol,
                //StandardOutputBo.TypeCampaignCode, // optional field
                //StandardOutputBo.TypeInsuredAttainedAge, // optional field
                StandardOutputBo.TypeTreatyCode,
                StandardOutputBo.TypeMlreBenefitCode,
                //StandardOutputBo.TypeUnderwriterRating, // optional field
                //StandardOutputBo.TypeOriSumAssured, // optional field

                // Updated on 20210119
                StandardOutputBo.TypeCedingBenefitTypeCode,
                //StandardOutputBo.TypeCedingBenefitRiskCode, // optional field
                //StandardOutputBo.TypeReportPeriodMonth, // optional field
                //StandardOutputBo.TypeReportPeriodYear, // optional field
                //StandardOutputBo.TypeCedingTreatyCode, // optional field
                //StandardOutputBo.TypeReinsBasisCode, // optional field

                // Updated on 20210311
                //StandardOutputBo.TypeReinsuranceIssueAge, // optional field
            };

            var errors = new List<string> { };
            foreach (int type in required)
            {
                var empty = FormatProductFeatureMappingError(type, "Empty");
                var property = StandardOutputBo.GetPropertyNameByType(type);
                var value = this.GetPropertyValue(property);
                if (value == null)
                    errors.Add(empty);
                else if (value is string @string && string.IsNullOrEmpty(@string))
                    errors.Add(empty);
            }

            GetReportPeriodDate(out reportDate, out string reportDateError);
            if (!string.IsNullOrEmpty(reportDateError))
                errors.Add(FormatProductFeatureMappingError("Report Period Date", reportDateError));

            ProductFeatureMappingValidate = errors.Count == 0;
            return errors;
        }

        public string FormatCellMappingError(int type, string msg)
        {
            return FormatCellMappingError(StandardOutputBo.GetTypeName(type), msg);
        }

        public string FormatCellMappingError(string field, string msg)
        {
            return string.Format("{0}{1}: {2}", CellMappingTitle, field, msg);
        }

        public string FormatCellMappingError(string msg)
        {
            return string.Format("{0}{1}", CellMappingTitle, msg);
        }

        public static List<int> ParamsCellMapping()
        {
            return new List<int> {
                StandardOutputBo.TypeTreatyCode,
                StandardOutputBo.TypeProfitComm,
                StandardOutputBo.TypeReinsBasisCode,
                StandardOutputBo.TypeCedingPlanCode, // optional field
                StandardOutputBo.TypeMlreBenefitCode, // optional field
                StandardOutputBo.TypeReinsEffDatePol, // optional field
                StandardOutputBo.TypeRateTable, // optional field
            };
        }

        public List<string> ValidateCellMapping()
        {
            // Mapping Columns
            var required = new List<int>
            {
                StandardOutputBo.TypeTreatyCode,
                StandardOutputBo.TypeProfitComm,
                StandardOutputBo.TypeReinsBasisCode,
                //StandardOutputBo.TypeCedingPlanCode, // optional field
                //StandardOutputBo.TypeMlreBenefitCode, // optional field
                //StandardOutputBo.TypeReinsEffDatePol, // optional field
                //StandardOutputBo.TypeRateTable, // optional field
            };

            var errors = new List<string> { };
            foreach (int type in required)
            {
                var empty = FormatCellMappingError(type, "Empty");
                var property = StandardOutputBo.GetPropertyNameByType(type);
                var value = this.GetPropertyValue(property);
                if (value == null)
                    errors.Add(empty);
                else if (value is string @string && string.IsNullOrEmpty(@string))
                    errors.Add(empty);
            }

            CellMappingValidate = errors.Count == 0;
            return errors;
        }

        public string FormatRateTableMappingError(int type, string msg)
        {
            return FormatRateTableMappingError(StandardOutputBo.GetTypeName(type), msg);
        }

        public string FormatRateTableMappingError(string field, string msg)
        {
            return string.Format("{0}{1}: {2}", RateTableMappingTitle, field, msg);
        }

        public string FormatRateTableMappingError(string msg)
        {
            return string.Format("{0}{1}", RateTableMappingTitle, msg);
        }

        public static List<int> ParamsRateTableMapping()
        {
            return new List<int> {
                StandardOutputBo.TypeTreatyCode,
                StandardOutputBo.TypeCedingPlanCode, // optional field
                StandardOutputBo.TypeCedingTreatyCode, // optional field
                StandardOutputBo.TypeCedingPlanCode2, // optional field
                StandardOutputBo.TypeCedingBenefitTypeCode, // optional field
                StandardOutputBo.TypeCedingBenefitRiskCode, // optional field
                StandardOutputBo.TypeGroupPolicyNumber, // optional field
                StandardOutputBo.TypeReinsBasisCode, // optional field
                StandardOutputBo.TypeMlreBenefitCode, // optional field

                // Removed in Phase 2
                //StandardOutputBo.TypeInsuredGenderCode, // optional field
                //StandardOutputBo.TypeInsuredTobaccoUse, // optional field
                //StandardOutputBo.TypeInsuredOccupationCode, // optional field

                StandardOutputBo.TypeInsuredAttainedAge, // optional field
                StandardOutputBo.TypePremiumFrequencyCode, // optional field
                StandardOutputBo.TypeAar, // optional field
                StandardOutputBo.TypeOriSumAssured, // optional field
                StandardOutputBo.TypeReinsEffDatePol, // optional field
                
                // Added in Phase 2
                StandardOutputBo.TypePolicyTerm, // optional field
                StandardOutputBo.TypeDurationMonth, // optional field
                
                StandardOutputBo.TypeReportPeriodMonth, // optional field
                StandardOutputBo.TypeReportPeriodYear, // optional field
            };
        }

        public List<string> ValidateRateTableMapping(out DateTime? reportDate)
        {
            // Mapping Columns
            var required = new List<int>
            {
                StandardOutputBo.TypeTreatyCode,
                //StandardOutputBo.TypeCedingPlanCode, // optional field

                //StandardOutputBo.TypeCedingTreatyCode, // optional field (new)
                //StandardOutputBo.TypeCedingPlanCode2, // optional field (new)
                //StandardOutputBo.TypeCedingBenefitTypeCode, // optional field (new)
                //StandardOutputBo.TypeCedingBenefitRiskCode, // optional field (new)
                //StandardOutputBo.TypeGroupPolicyNumber, // optional field (new)
                //StandardOutputBo.TypeReinsBasisCode, // optional field (new)

                //StandardOutputBo.TypeMlreBenefitCode, // optional field

                // Removed in Phase 2
                //StandardOutputBo.TypeInsuredGenderCode, // optional field
                //StandardOutputBo.TypeInsuredTobaccoUse, // optional field
                //StandardOutputBo.TypeInsuredOccupationCode, // optional field

                //StandardOutputBo.TypeInsuredAttainedAge, // optional field
                //StandardOutputBo.TypePremiumFrequencyCode, // optional field

                //StandardOutputBo.TypeAar, // optional field (new)

                //StandardOutputBo.TypeOriSumAssured, // optional field
                //StandardOutputBo.TypeReinsEffDatePol, // optional field

                // Added in Phase 2
                //StandardOutputBo.TypePolicyTerm, // optional field
                //StandardOutputBo.TypeDurationMonth, // optional field
                
                //StandardOutputBo.TypeReportPeriodMonth, // optional field
                //StandardOutputBo.TypeReportPeriodYear, // optional field
            };

            var errors = new List<string> { };
            foreach (int type in required)
            {
                var empty = FormatRateTableMappingError(type, "Empty");
                var property = StandardOutputBo.GetPropertyNameByType(type);
                var value = this.GetPropertyValue(property);
                if (value == null)
                    errors.Add(empty);
                else if (value is string @string && string.IsNullOrEmpty(@string))
                    errors.Add(empty);
            }

            GetReportPeriodDate(out reportDate, out string reportDateError);
            if (!string.IsNullOrEmpty(reportDateError))
                errors.Add(FormatRateTableMappingError("Report Period Date", reportDateError));

            RateTableMappingValidate = errors.Count == 0;
            return errors;
        }

        public string FormatRateMappingError(int type, string msg)
        {
            return FormatRateMappingError(StandardOutputBo.GetTypeName(type), msg);
        }

        public string FormatRateMappingError(string field, string msg)
        {
            return string.Format("{0}{1}: {2}", RateMappingTitle, field, msg);
        }

        public string FormatRateMappingError(string msg)
        {
            return string.Format("{0}{1}", RateMappingTitle, msg);
        }

        public static List<int> ParamsRateMapping(List<int> fields)
        {
            return fields;
        }

        public List<string> ValidateRateMapping(List<int> fields)
        {
            // Mapping Columns
            List<int> required = fields;

            List<string> errors = new List<string> { };
            foreach (int type in required)
            {
                string property = StandardOutputBo.GetPropertyNameByType(type);
                object value = this.GetPropertyValue(property);
                if (value == null)
                {
                    errors.Add(FormatRateTableMappingError(type, "Empty"));
                }
                else if (value is string @string && string.IsNullOrEmpty(@string))
                {
                    errors.Add(FormatRateTableMappingError(type, "Empty"));
                }
            }
            return errors;
        }

        public string FormatRiDiscountMappingError(int type, string msg)
        {
            return FormatRiDiscountMappingError(StandardOutputBo.GetTypeName(type), msg);
        }

        public string FormatRiDiscountMappingError(string field, string msg)
        {
            return string.Format("{0}{1}: {2}", RiDiscountMappingTitle, field, msg);
        }

        public string FormatRiDiscountMappingError(string msg)
        {
            return string.Format("{0}{1}", RiDiscountMappingTitle, msg);
        }

        public static List<int> ParamsRiDiscountMapping()
        {
            return new List<int> {
                StandardOutputBo.TypeReinsEffDatePol,
                StandardOutputBo.TypeDurationMonth
            };
        }

        public List<string> ValidateRiDiscountMapping()
        {
            // Mapping Columns
            List<int> required = new List<int>
            {
                //StandardOutputBo.TypeReinsEffDatePol,
                //StandardOutputBo.TypeDurationMonth,
            };

            List<string> errors = new List<string> { };
            foreach (int type in required)
            {
                string property = StandardOutputBo.GetPropertyNameByType(type);
                object value = this.GetPropertyValue(property);
                if (value == null)
                {
                    errors.Add(FormatRiDiscountMappingError(type, "Empty"));
                }
                else if (value is string @string && string.IsNullOrEmpty(@string))
                {
                    errors.Add(FormatRiDiscountMappingError(type, "Empty"));
                }
            }

            RateTableMappingValidate = errors.Count == 0;

            return errors;
        }

        public string FormatLargeDiscountMappingError(int type, string msg)
        {
            return FormatLargeDiscountMappingError(StandardOutputBo.GetTypeName(type), msg);
        }

        public string FormatLargeDiscountMappingError(string field, string msg)
        {
            return string.Format("{0}{1}: {2}", LargeDiscountMappingTitle, field, msg);
        }

        public string FormatLargeDiscountMappingError(string msg)
        {
            return string.Format("{0}{1}", LargeDiscountMappingTitle, msg);
        }

        public static List<int> ParamsLargeDiscountMapping()
        {
            return new List<int> {
                StandardOutputBo.TypeReinsEffDatePol,
                StandardOutputBo.TypeOriSumAssured
            };
        }

        public List<string> ValidateLargeDiscountMapping()
        {
            // Mapping Columns
            List<int> required = new List<int>
            {
                //StandardOutputBo.TypeReinsEffDatePol,
                StandardOutputBo.TypeOriSumAssured,
            };

            List<string> errors = new List<string> { };
            foreach (int type in required)
            {
                string property = StandardOutputBo.GetPropertyNameByType(type);
                object value = this.GetPropertyValue(property);
                if (value == null)
                {
                    errors.Add(FormatLargeDiscountMappingError(type, "Empty"));
                }
                else if (value is string @string && string.IsNullOrEmpty(@string))
                {
                    errors.Add(FormatLargeDiscountMappingError(type, "Empty"));
                }
            }

            RateTableMappingValidate = errors.Count == 0;

            return errors;
        }

        public string FormatGroupDiscountMappingError(int type, string msg)
        {
            return FormatGroupDiscountMappingError(StandardOutputBo.GetTypeName(type), msg);
        }

        public string FormatGroupDiscountMappingError(string field, string msg)
        {
            return string.Format("{0}{1}: {2}", GroupDiscountMappingTitle, field, msg);
        }

        public string FormatGroupDiscountMappingError(string msg)
        {
            return string.Format("{0}{1}", GroupDiscountMappingTitle, msg);
        }

        public static List<int> ParamsGroupDiscountMapping()
        {
            return new List<int> {
                StandardOutputBo.TypeReinsEffDatePol,
                StandardOutputBo.TypePolicyTotalLive
            };
        }

        public List<string> ValidateGroupDiscountMapping()
        {
            // Mapping Columns
            List<int> required = new List<int>
            {
                //StandardOutputBo.TypeReinsEffDatePol,
                StandardOutputBo.TypePolicyTotalLive,
            };

            List<string> errors = new List<string> { };
            foreach (int type in required)
            {
                string property = StandardOutputBo.GetPropertyNameByType(type);
                object value = this.GetPropertyValue(property);
                if (value == null)
                {
                    errors.Add(FormatGroupDiscountMappingError(type, "Empty"));
                }
                else if (value is string @string && string.IsNullOrEmpty(@string))
                {
                    errors.Add(FormatGroupDiscountMappingError(type, "Empty"));
                }
            }

            RateTableMappingValidate = errors.Count == 0;

            return errors;
        }

        public string FormatAnnuityFactorMappingError(int type, string msg)
        {
            return FormatAnnuityFactorMappingError(StandardOutputBo.GetTypeName(type), msg);
        }

        public string FormatAnnuityFactorMappingError(string field, string msg)
        {
            return string.Format("{0}{1}: {2}", AnnuityFactorMappingTitle, field, msg);
        }

        public string FormatAnnuityFactorMappingError(string msg)
        {
            return string.Format("{0}{1}", AnnuityFactorMappingTitle, msg);
        }

        public string FormatAnnuityFactorRateError(int type, string msg)
        {
            return FormatAnnuityFactorRateError(StandardOutputBo.GetTypeName(type), msg);
        }

        public string FormatAnnuityFactorRateError(string field, string msg)
        {
            return string.Format("{0}{1}: {2}", AnnuityFactorRateTitle, field, msg);
        }

        public string FormatAnnuityFactorRateError(string msg)
        {
            return string.Format("{0}{1}", AnnuityFactorRateTitle, msg);
        }

        public static List<int> ParamsAnnuityFactorMapping()
        {
            return new List<int> {
                StandardOutputBo.TypeCedingPlanCode,
                StandardOutputBo.TypeReinsEffDatePol,
                StandardOutputBo.TypePolicyTermRemain, //Optional
                StandardOutputBo.TypeInsuredGenderCode, //Optional
                StandardOutputBo.TypeInsuredTobaccoUse, //Optional
                StandardOutputBo.TypeInsuredAttainedAge, //Optional
                StandardOutputBo.TypePolicyTerm, //Optional
            };
        }

        public List<string> ValidateAnnuityFactorMapping()
        {
            // Mapping Columns
            List<int> required = new List<int>
            {
                StandardOutputBo.TypeCedingPlanCode,
                StandardOutputBo.TypeReinsEffDatePol,
                //StandardOutputBo.TypePolicyTermRemain, //Optional

                // Added 2020-03-02
                //StandardOutputBo.TypeInsuredGenderCode, //Optional
                //StandardOutputBo.TypeInsuredTobaccoUse, //Optional
                //StandardOutputBo.TypeInsuredAttainedAge, //Optional
                //StandardOutputBo.TypePolicyTerm, //Optional
            };

            List<string> errors = new List<string> { };
            foreach (int type in required)
            {
                string property = StandardOutputBo.GetPropertyNameByType(type);
                object value = this.GetPropertyValue(property);
                if (value == null)
                {
                    errors.Add(FormatAnnuityFactorMappingError(type, "Empty"));
                }
                else if (value is string @string && string.IsNullOrEmpty(@string))
                {
                    errors.Add(FormatAnnuityFactorMappingError(type, "Empty"));
                }
            }

            AnnuityFactorMappingValidate = errors.Count == 0;

            return errors;
        }

        public string FormatRiDataLookupError(int type, string msg)
        {
            return FormatRiDataLookupError(StandardOutputBo.GetTypeName(type), msg);
        }

        public string FormatRiDataLookupError(string field, string msg)
        {
            return string.Format("{0}{1}: {2}", RiDataLookupTitle, field, msg);
        }

        public string FormatRiDataLookupError(string msg)
        {
            return string.Format("{0}{1}", RiDataLookupTitle, msg);
        }

        public static List<int> ParamsRiDataLookup()
        {
            return new List<int> {
                StandardOutputBo.TypePolicyNumber,
                StandardOutputBo.TypeCedingPlanCode,
                StandardOutputBo.TypeRiskPeriodMonth,
                StandardOutputBo.TypeRiskPeriodYear,
                StandardOutputBo.TypeMlreBenefitCode,
                StandardOutputBo.TypeTreatyCode,
                StandardOutputBo.TypeRiderNumber,
            };
        }

        public List<string> ValidateRiDataLookup()
        {
            // Mapping Columns
            List<int> required = new List<int>
            {
                StandardOutputBo.TypePolicyNumber,
                StandardOutputBo.TypeCedingPlanCode,
                StandardOutputBo.TypeRiskPeriodMonth,
                StandardOutputBo.TypeRiskPeriodYear,
                StandardOutputBo.TypeMlreBenefitCode,
                StandardOutputBo.TypeTreatyCode,
                //StandardOutputBo.TypeRiderNumber, //Optional
            };

            List<string> errors = new List<string> { };
            foreach (int type in required)
            {
                string property = StandardOutputBo.GetPropertyNameByType(type);
                object value = this.GetPropertyValue(property);
                if (value == null)
                {
                    errors.Add(FormatRiDataLookupError(type, "Empty"));
                }
                else if (value is string @string && string.IsNullOrEmpty(@string))
                {
                    errors.Add(FormatRiDataLookupError(type, "Empty"));
                }
            }

            RiDataLookupValidate = errors.Count == 0;

            return errors;
        }

        public string FormatRiskDateError(int type, string msg)
        {
            return FormatRiskDateError(StandardOutputBo.GetTypeName(type), msg);
        }

        public string FormatRiskDateError(string field, string msg)
        {
            return string.Format("{0}{1}: {2}", RiskDateTitle, field, msg);
        }

        public string FormatRiskDateError(string msg)
        {
            return string.Format("{0}{1}", RiskDateTitle, msg);
        }

        public static List<int> ParamsRiskDate()
        {
            return new List<int> {
                StandardOutputBo.TypeReinsEffDatePol,
                StandardOutputBo.TypeRiskPeriodMonth,
                StandardOutputBo.TypeRiskPeriodYear,
                StandardOutputBo.TypePremiumFrequencyCode,
            };
        }

        public List<string> ValidateRiskDate()
        {
            // Mapping Columns
            List<int> required = new List<int>
            {
                StandardOutputBo.TypeReinsEffDatePol,
                StandardOutputBo.TypeRiskPeriodMonth,
                StandardOutputBo.TypeRiskPeriodYear,
                StandardOutputBo.TypePremiumFrequencyCode,
            };

            List<string> errors = new List<string> { };
            foreach (int type in required)
            {
                string property = StandardOutputBo.GetPropertyNameByType(type);
                object value = this.GetPropertyValue(property);
                if (value == null)
                {
                    errors.Add(FormatRiskDateError(type, "Empty"));
                }
                else if (value is string @string && string.IsNullOrEmpty(@string))
                {
                    errors.Add(FormatRiskDateError(type, "Empty"));
                }
            }

            RiskDateValidate = errors.Count == 0;

            return errors;
        }

        public string FormatFacMappingError(int type, string msg)
        {
            return FormatFacMappingError(StandardOutputBo.GetTypeName(type), msg);
        }

        public string FormatFacMappingError(string field, string msg)
        {
            return string.Format("{0}{1}: {2}", FacMappingTitle, field, msg);
        }

        public string FormatFacMappingError(string msg)
        {
            return string.Format("{0}{1}", FacMappingTitle, msg);
        }

        public static List<int> ParamsFacMapping()
        {
            return new List<int> {
                StandardOutputBo.TypePolicyNumber,
                StandardOutputBo.TypeInsuredName,
                StandardOutputBo.TypeMlreBenefitCode,
            };
        }

        public List<string> ValidateFacMapping()
        {
            // Mapping Columns
            List<int> required = new List<int>
            {
                StandardOutputBo.TypePolicyNumber,
                StandardOutputBo.TypeInsuredName,
                StandardOutputBo.TypeMlreBenefitCode,
            };

            List<string> errors = new List<string> { };
            foreach (int type in required)
            {
                string property = StandardOutputBo.GetPropertyNameByType(type);
                object value = this.GetPropertyValue(property);
                if (value == null)
                {
                    errors.Add(FormatFacMappingError(type, "Empty"));
                }
                else if (value is string @string && string.IsNullOrEmpty(@string))
                {
                    errors.Add(FormatFacMappingError(type, "Empty"));
                }
            }

            FacMappingValidate = errors.Count == 0;

            return errors;
        }

        public string FormatDropDownError(string title, int type, string msg)
        {
            return FormatDropDownError(title, StandardOutputBo.GetTypeName(type), msg);
        }

        public string FormatDropDownError(string title, string field, string msg)
        {
            return string.Format("{0}{1}: {2}", title, field, msg);
        }

        public string FormatFinaliseError(int type, string msg)
        {
            return FormatFinaliseError(StandardOutputBo.GetTypeName(type), msg);
        }

        public string FormatFinaliseError(string field, string msg)
        {
            return string.Format("{0}: {1}", field, msg);
        }

        public List<string> ValidateFinalise(string soaQuarter)
        {
            var required = new List<int>
            {
                StandardOutputBo.TypeTreatyCode,
                StandardOutputBo.TypeReinsBasisCode,
                StandardOutputBo.TypeFundsAccountingTypeCode,
                StandardOutputBo.TypePremiumFrequencyCode,
                StandardOutputBo.TypeReportPeriodMonth,
                StandardOutputBo.TypeReportPeriodYear,
                StandardOutputBo.TypeRiskPeriodMonth,
                StandardOutputBo.TypeRiskPeriodYear,
                StandardOutputBo.TypeReportPeriodMonth,
                StandardOutputBo.TypeReportPeriodYear,
                StandardOutputBo.TypeTransactionTypeCode,
                StandardOutputBo.TypePolicyNumber,
                StandardOutputBo.TypeReinsEffDatePol,
                //StandardOutputBo.TypeCedingPlanCode,
                StandardOutputBo.TypeCedingBenefitTypeCode,
                //StandardOutputBo.TypeCedingBenefitRiskCode,
                StandardOutputBo.TypeMlreBenefitCode,
                //StandardOutputBo.TypeCurrSumAssured,
                StandardOutputBo.TypeAar,
                //StandardOutputBo.TypeInsuredGenderCode,
                //StandardOutputBo.TypeInsuredTobaccoUse,
                StandardOutputBo.TypeInsuredDateOfBirth,
                //StandardOutputBo.TypeInsuredOccupationCode,
                //StandardOutputBo.TypeInsuredAttainedAge,
                StandardOutputBo.TypeRateTable,
                StandardOutputBo.TypeUnderwriterRating,
                StandardOutputBo.TypeGrossPremium,
                StandardOutputBo.TypeNetPremium,
                StandardOutputBo.TypeMfrs17BasicRider,
                StandardOutputBo.TypeMfrs17CellName,
                StandardOutputBo.TypeMfrs17TreatyCode,
                // Added in Phase 2
                StandardOutputBo.TypeLoaCode,
                StandardOutputBo.TypeTerritoryOfIssueCode,
                StandardOutputBo.TypeCurrencyCode,
                StandardOutputBo.TypeTransactionPremium,
                StandardOutputBo.TypeTransactionDiscount,
                StandardOutputBo.TypePolicyStatusCode,
                StandardOutputBo.TypeProfitComm,
            };

            var errors = new List<string> { };
            foreach (int type in required)
            {
                string property = StandardOutputBo.GetPropertyNameByType(type);
                object value = this.GetPropertyValue(property);
                string error = FormatFinaliseError(type, "Empty");
                if (value == null)
                {
                    errors.Add(error);
                    SetError(property, error);
                }
                else if (value is string @string && string.IsNullOrEmpty(@string))
                {
                    errors.Add(error);
                    SetError(property, error);
                }
            }

            GetRiskPeriodDate(out _, out string riskPeriodDate);
            if (!string.IsNullOrEmpty(riskPeriodDate))
            {
                string error = FormatFinaliseError("Risk Period Date", riskPeriodDate);
                errors.Add(error);

                string propertyMonth = StandardOutputBo.GetPropertyNameByType(StandardOutputBo.TypeRiskPeriodMonth);
                string propertyYear = StandardOutputBo.GetPropertyNameByType(StandardOutputBo.TypeRiskPeriodYear);
                SetError(propertyMonth, error);
                SetError(propertyYear, error);
            }

            return errors;
        }

        public string ValidateReportPeriodDate(string soaQuarter)
        {
            DateTime? reportPeriodDate;
            GetReportPeriodDate(out reportPeriodDate, out string reportDateError);
            if (!string.IsNullOrEmpty(reportDateError))
                return string.Format("{0} {1}", "Report Period Date", reportDateError);
            else if (reportPeriodDate.HasValue && Util.MonthYearToQuarter(reportPeriodDate.Value.Year, reportPeriodDate.Value.Month) != soaQuarter)
                return string.Format("Report Period Month & Year does not match Quarter");

            return "";
        }

        public string FormatValuationRateName(string rateTableCode, int valuationRate)
        {
            switch (valuationRate)
            {
                case RateBo.ValuationRate1:
                    return string.Format("{0}", rateTableCode);
                case RateBo.ValuationRate2:
                    return string.Format("{0}_{1}", rateTableCode, InsuredGenderCode);
                case RateBo.ValuationRate3:
                    return string.Format("{0}_{1}", rateTableCode, InsuredTobaccoUse);
                case RateBo.ValuationRate4:
                    return string.Format("{0}_{1}_{2}", rateTableCode, InsuredGenderCode, InsuredTobaccoUse);
                case RateBo.ValuationRate5:
                    return string.Format("{0}_{1}", rateTableCode, InsuredOccupationCode);
                case RateBo.ValuationRate6:
                    return string.Format("{0}_{1}_{2}_{3}", rateTableCode, InsuredGenderCode, InsuredTobaccoUse, PolicyTermRemain);
                case RateBo.ValuationRate7:
                    return string.Format("{0}_{1}_{2}", rateTableCode, MlrePolicyIssueAge, PolicyTermRemain);
                case RateBo.ValuationRate8:
                    return string.Format("{0}_{1}_{2}_{3}_{4}", rateTableCode, PolicyTerm, MlrePolicyIssueAge, InsuredGenderCode, InsuredTobaccoUse);
                case RateBo.ValuationRate9:
                    return string.Format("{0}_{1}_{2}", rateTableCode, InsuredGenderCode, InsuredOccupationCode);
                case RateBo.ValuationRate10:
                    return string.Format("{0}_{1}_{2}_{3}", rateTableCode, InsuredGenderCode, InsuredTobaccoUse, InsuredOccupationCode);
                default:
                    return "";
            }
        }

        public bool IsComputationSuccess(int step = 1)
        {
            var name = InsuredName;
            var formulaValidate = FormulaValidates.Where(q => q.Step == step).Select(q => q.Validate).FirstOrDefault();
            var benefitCodeMappingValidate = BenefitCodeMappingValidates.Where(q => q.Step == step).Select(q => q.Validate).FirstOrDefault();
            var treatyCodeMappingValidate = TreatyCodeMappingValidates.Where(q => q.Step == step).Select(q => q.Validate).FirstOrDefault();
            var productFeatureMappingValidate = ProductFeatureMappingValidates.Where(q => q.Step == step).Select(q => q.Validate).FirstOrDefault();
            var cellMappingValidate = CellMappingValidates.Where(q => q.Step == step).Select(q => q.Validate).FirstOrDefault();
            var rateTableMappingValidate = RateTableMappingValidates.Where(q => q.Step == step).Select(q => q.Validate).FirstOrDefault();
            var annuityFactorMappingValidate = AnnuityFactorMappingValidates.Where(q => q.Step == step).Select(q => q.Validate).FirstOrDefault();
            var riDataLookupValidate = RiDataLookupValidates.Where(q => q.Step == step).Select(q => q.Validate).FirstOrDefault();
            var riskDateValidate = RiskDateValidates.Where(q => q.Step == step).Select(q => q.Validate).FirstOrDefault();
            var facMappingValidate = FacMappingValidates.Where(q => q.Step == step).Select(q => q.Validate).FirstOrDefault();
            var treatyNumberMappingValidate = TreatyNumberTreatyTypeMappingValidates.Where(q => q.Step == step).Select(q => q.Validate).FirstOrDefault();

            return formulaValidate &&
                   benefitCodeMappingValidate &&
                   treatyCodeMappingValidate &&
                   productFeatureMappingValidate &&
                   cellMappingValidate &&
                   rateTableMappingValidate &&
                   annuityFactorMappingValidate &&
                   riDataLookupValidate &&
                   riskDateValidate &&
                   facMappingValidate &&
                   treatyNumberMappingValidate;
        }

        public void UpdateComputationStatus(int step = RiDataComputationBo.StepPreComputation1)
        {
            if (IsComputationSuccess(step))
            {
                switch (step)
                {
                    case RiDataComputationBo.StepPreComputation1:
                        PreComputation1Status = PreComputation1StatusSuccess;
                        break;
                    case RiDataComputationBo.StepPreComputation2:
                        PreComputation2Status = PreComputation2StatusSuccess;
                        break;
                    case RiDataComputationBo.StepPostComputation:
                        PostComputationStatus = PostComputationStatusSuccess;
                        break;
                }
            }
            else
            {
                switch (step)
                {
                    case RiDataComputationBo.StepPreComputation1:
                        PreComputation1Status = PreComputation1StatusFailed;
                        break;
                    case RiDataComputationBo.StepPreComputation2:
                        PreComputation2Status = PreComputation2StatusFailed;
                        break;
                    case RiDataComputationBo.StepPostComputation:
                        PostComputationStatus = PostComputationStatusFailed;
                        break;
                }
            }
        }

        public void SetComputationValidates(int step = RiDataComputationBo.StepPreComputation1, int key = ValidateKeyFormula, bool status = false)
        {
            switch (key)
            {
                case ValidateKeyFormula:
                    foreach (var validate in FormulaValidates.Where(q => q.Step == step).ToList())
                        validate.Validate = status;
                    break;
                case ValidateKeyMapping:
                    MappingValidate = status;
                    break;
                case ValidateKeyTreatyCodeMapping:
                    foreach (var validate in TreatyCodeMappingValidates.Where(q => q.Step == step).ToList())
                        validate.Validate = status;
                    break;

                case ValidateKeyBenefitCodeMapping:
                    foreach (var validate in BenefitCodeMappingValidates.Where(q => q.Step == step).ToList())
                        validate.Validate = status;
                    break;
                case ValidateKeyProductFeatureMapping:
                    foreach (var validate in ProductFeatureMappingValidates.Where(q => q.Step == step).ToList())
                        validate.Validate = status;
                    break;
                case ValidateKeyCellMapping:
                    foreach (var validate in CellMappingValidates.Where(q => q.Step == step).ToList())
                        validate.Validate = status;
                    break;
                case ValidateKeyRateTableMapping:
                    foreach (var validate in RateTableMappingValidates.Where(q => q.Step == step).ToList())
                        validate.Validate = status;
                    break;
                case ValidateKeyAnnuityFactorMapping:
                    foreach (var validate in AnnuityFactorMappingValidates.Where(q => q.Step == step).ToList())
                        validate.Validate = status;
                    break;
                case ValidateKeyRiDataLookup:
                    foreach (var validate in RiDataLookupValidates.Where(q => q.Step == step).ToList())
                        validate.Validate = status;
                    break;
                case ValidateKeyRiskDate:
                    foreach (var validate in RiskDateValidates.Where(q => q.Step == step).ToList())
                        validate.Validate = status;
                    break;
                case ValidateKeyFacMapping:
                    foreach (var validate in FacMappingValidates.Where(q => q.Step == step).ToList())
                        validate.Validate = status;
                    break;
                case ValidateKeyTreatyNumberTreatyTypeMapping:
                    foreach (var validate in TreatyNumberTreatyTypeMappingValidates.Where(q => q.Step == step).ToList())
                        validate.Validate = status;
                    break;
            }
        }

        public void ResetDirectRetroValue()
        {
            RetroParty1 = null;
            RetroParty2 = null;
            RetroParty3 = null;
            RetroShare1 = null;
            RetroShare2 = null;
            RetroShare3 = null;
            RetroPremiumSpread1 = null;
            RetroPremiumSpread2 = null;
            RetroPremiumSpread3 = null;
            RetroAar1 = null;
            RetroAar2 = null;
            RetroAar3 = null;
            RetroReinsurancePremium1 = null;
            RetroReinsurancePremium2 = null;
            RetroReinsurancePremium3 = null;
            RetroDiscount1 = null;
            RetroDiscount2 = null;
            RetroDiscount3 = null;
            RetroNetPremium1 = null;
            RetroNetPremium2 = null;
            RetroNetPremium3 = null;
            RetroNoClaimBonus1 = null;
            RetroNoClaimBonus2 = null;
            RetroNoClaimBonus3 = null;
            RetroDatabaseCommission1 = null;
            RetroDatabaseCommission2 = null;
            RetroDatabaseCommission3 = null;

            // Total
            TotalDirectRetroAar = null;
            TotalDirectRetroGrossPremium = null;
            TotalDirectRetroDiscount = null;
            TotalDirectRetroNetPremium = null;
            TotalDirectRetroNoClaimBonus = null;
            TotalDirectRetroDatabaseCommission = null;
        }

        public void ResetPostComputationValue()
        {
            InsuredAttainedAgeCheck = null;
            PolicyIssueAgeCheck = null;
            MaxExpiryAgeCheck = null;
            MinIssueAgeCheck = null;
            MaxIssueAgeCheck = null;
            MaxUwRatingCheck = null;
            EffectiveDateCheck = null;
            AarCheck = null;
            AblCheck = null;
            RetentionCheck = null;
            MinAarCheck = null;
            MaxAarCheck = null;
            CorridorLimitCheck = null;
            ApLoadingCheck = null;
            ValidityDayCheck = null;
            SumAssuredOfferedCheck = null;
            UwRatingCheck = null;
            FlatExtraAmountCheck = null;
            FlatExtraDurationCheck = null;

            MlreStandardPremium = null;
            MlreSubstandardPremium = null;
            MlreFlatExtraPremium = null;
            MlreGrossPremium = null;
            MlreStandardDiscount = null;
            MlreSubstandardDiscount = null;
            MlreLargeSaDiscount = null;
            MlreGroupSizeDiscount = null;
            MlreVitalityDiscount = null;
            MlreTotalDiscount = null;
            MlreNetPremium = null;
            NetPremiumCheck = null;
            MlreBrokerageFee = null;
            MlreDatabaseCommission = null;
            ServiceFee = null;
            Mfrs17BasicRider = null;
            Mfrs17CellName = null;
            Mfrs17TreatyCode = null;
            Mfrs17AnnualCohort = null;
            LoaCode = null;

            PostComputationStatus = PostComputationStatusPending;
            PostValidationStatus = PostValidationStatusPending;
        }

        public static List<Column> GetDirectRetroColumns()
        {
            var columns = new List<Column> { };

            // add all standard fields
            foreach (int i in ColumnSequence())
            {
                columns.Add(new Column
                {
                    Header = StandardOutputBo.GetCodeByType(i),
                    Property = StandardOutputBo.GetPropertyNameByType(i),
                });
            }

            columns.Add(new Column
            {
                Header = "RETRO_PARTY_1",
                Property = "RetroParty1",
            });

            columns.Add(new Column
            {
                Header = "RETRO_SHARE_1",
                Property = "RetroShare1",
            });

            columns.Add(new Column
            {
                Header = "DIRECT_RETRO_AAR_1",
                Property = "RetroAar1",
            });

            columns.Add(new Column
            {
                Header = "DIRECT_RETRO_REINS_PREMIUM_1",
                Property = "RetroReinsurancePremium1",
            });

            columns.Add(new Column
            {
                Header = "DIRECT_RETRO_DISCOUNT_1",
                Property = "RetroDiscount1",
            });

            columns.Add(new Column
            {
                Header = "DIRECT_RETRO_NET_PREMIUM_1",
                Property = "RetroNetPremium1",
            });

            columns.Add(new Column
            {
                Header = "RETRO_PARTY_2",
                Property = "RetroParty2",
            });

            columns.Add(new Column
            {
                Header = "RETRO_SHARE_2",
                Property = "RetroShare2",
            });

            columns.Add(new Column
            {
                Header = "DIRECT_RETRO_AAR_2",
                Property = "RetroAar2",
            });

            columns.Add(new Column
            {
                Header = "DIRECT_RETRO_REINS_PREMIUM_2",
                Property = "RetroReinsurancePremium2",
            });

            columns.Add(new Column
            {
                Header = "DIRECT_RETRO_DISCOUNT_2",
                Property = "RetroDiscount2",
            });

            columns.Add(new Column
            {
                Header = "DIRECT_RETRO_NET_PREMIUM_2",
                Property = "RetroNetPremium2",
            });

            columns.Add(new Column
            {
                Header = "RETRO_PARTY_3",
                Property = "RetroParty3",
            });

            columns.Add(new Column
            {
                Header = "RETRO_SHARE_3",
                Property = "RetroShare3",
            });

            columns.Add(new Column
            {
                Header = "DIRECT_RETRO_AAR_3",
                Property = "RetroAar3",
            });

            columns.Add(new Column
            {
                Header = "DIRECT_RETRO_REINS_PREMIUM_3",
                Property = "RetroReinsurancePremium3",
            });

            columns.Add(new Column
            {
                Header = "DIRECT_RETRO_DISCOUNT_3",
                Property = "RetroDiscount3",
            });

            columns.Add(new Column
            {
                Header = "DIRECT_RETRO_NET_PREMIUM_3",
                Property = "RetroNetPremium3",
            });

            return columns;
        }

        public static string ConflictScriptGenderCountryWithinBatch(int batchId)
        {
            string select = string.Format("SELECT r.[Id] FROM [RiData] AS r LEFT JOIN [RiDataBatches] b ON b.Id = r.RiDataBatchId LEFT JOIN [Cedants] c ON c.Id = b.CedantId WHERE r.[RiDataBatchId] = {0} AND r.[TreatyCode] LIKE CONCAT('%', c.Code, '%') AND EXISTS(SELECT  1 FROM [RiData] rd  LEFT JOIN [RiDataBatches] rdb ON rdb.Id = rd.RiDataBatchId LEFT JOIN [Cedants] cd ON c.Id = rdb.CedantId WHERE rd.[RiDataBatchId] = {0} AND rd.[TreatyCode] LIKE CONCAT('%', cd.Code, '%') AND rd.[InsuredName] = r.InsuredName AND rd.[InsuredDateOfBirth] = r.InsuredDateOfBirth AND rd.[InsuredGenderCode] <> r.InsuredGenderCode AND rd.[TerritoryOfIssueCode] <> r.TerritoryOfIssueCode)", batchId);
            string script = string.Format("UPDATE [RiData] SET [ConflictType] = 5 WHERE [Id] IN ({0})", select);

            return script;
        }

        public static string ConflictScriptGenderCountryWithWarehouse(int batchId)
        {
            string select = string.Format("SELECT r.[Id] FROM [RiData] AS r LEFT JOIN [RiDataBatches] b ON b.Id = r.RiDataBatchId LEFT JOIN [Cedants] c ON c.Id = b.CedantId WHERE r.[RiDataBatchId] = {0} AND r.[TreatyCode] LIKE CONCAT('%', c.Code, '%') AND ConflictType = 0 AND EXISTS(SELECT 1 FROM [RiDataWarehouse] WHERE [TreatyCode] LIKE CONCAT('%', c.Code, '%') AND [InsuredName] = r.InsuredName AND [InsuredDateOfBirth] = r.InsuredDateOfBirth AND [InsuredGenderCode] <> r.InsuredGenderCode AND [TerritoryOfIssueCode] <> r.TerritoryOfIssueCode)", batchId);
            string script = string.Format("UPDATE [RiData] SET [ConflictType] = 6 WHERE [Id] IN ({0})", select);

            return script;
        }

        public static string ConflictScriptGenderWithinBatchCountryWithWarehouse(int batchId)
        {
            string select = string.Format("SELECT r.Id FROM [RiData] AS r LEFT JOIN [RiDataBatches] b ON b.Id = r.RiDataBatchId LEFT JOIN [Cedants] c ON c.Id = b.CedantId WHERE r.[RiDataBatchId] = {0} AND r.[TreatyCode] LIKE CONCAT('%', c.Code, '%') AND r.ConflictType = 0 AND EXISTS(SELECT 1 FROM [RiDataWarehouse] AS w JOIN [RiData] AS rd ON w.InsuredName = rd.InsuredName AND w.InsuredDateOfBirth = rd.InsuredDateOfBirth JOIN [RiDataBatches] rdb ON rdb.Id = rd.RiDataBatchId JOIN [Cedants] cd ON cd.Id = rdb.CedantId WHERE rd.RiDataBatchId = {0} AND rd.TreatyCode LIKE CONCAT('%', cd.Code, '%') AND w.TreatyCode LIKE CONCAT('%', cd.Code, '%') AND r.InsuredName = w.InsuredName AND r.InsuredDateOfBirth = w.InsuredDateOfBirth AND w.InsuredGenderCode = r.InsuredGenderCode AND rd.InsuredGenderCode <> r.InsuredGenderCode AND rd.TerritoryOfIssueCode = r.TerritoryOfIssueCode AND w.TerritoryOfIssueCode <> r.TerritoryOfIssueCode)", batchId);
            string script = string.Format("UPDATE [RiData] SET [ConflictType] = 7 WHERE [Id] IN ({0})", select);

            return script;
        }

        public static string ConflictScriptGenderWithWarehouseCountryWithinBatch(int batchId)
        {
            string select = string.Format("SELECT r.Id FROM [RiData] AS r LEFT JOIN [RiDataBatches] b ON b.Id = r.RiDataBatchId LEFT JOIN [Cedants] c ON c.Id = b.CedantId WHERE r.[RiDataBatchId] = {0} AND r.[TreatyCode] LIKE CONCAT('%', c.Code, '%') AND r.ConflictType = 0 AND EXISTS(SELECT 1 FROM [RiDataWarehouse] AS w JOIN [RiData] AS rd ON w.InsuredName = rd.InsuredName AND w.InsuredDateOfBirth = rd.InsuredDateOfBirth JOIN [RiDataBatches] rdb ON rdb.Id = rd.RiDataBatchId JOIN [Cedants] cd ON cd.Id = rdb.CedantId WHERE rd.RiDataBatchId = {0} AND rd.TreatyCode LIKE CONCAT('%', cd.Code, '%') AND w.TreatyCode LIKE CONCAT('%', cd.Code, '%') AND r.InsuredName = w.InsuredName AND r.InsuredDateOfBirth = w.InsuredDateOfBirth AND rd.InsuredGenderCode = r.InsuredGenderCode AND w.InsuredGenderCode <> r.InsuredGenderCode AND w.TerritoryOfIssueCode = r.TerritoryOfIssueCode AND rd.TerritoryOfIssueCode <> r.TerritoryOfIssueCode)", batchId);
            string script = string.Format("UPDATE [RiData] SET [ConflictType] = 8 WHERE [Id] IN ({0})", select);

            return script;
        }

        public static string ConflictScriptGenderWithinBatch(int batchId)
        {
            string select = string.Format("SELECT r.Id FROM [RiData] AS r LEFT JOIN [RiDataBatches] b ON b.Id = r.RiDataBatchId LEFT JOIN [Cedants] c ON c.Id = b.CedantId WHERE r.[RiDataBatchId] = {0} AND r.[TreatyCode] LIKE CONCAT('%', c.Code, '%') AND r.ConflictType = 0 AND EXISTS(SELECT 1 FROM [RiData] rd JOIN [RiDataBatches] rdb ON rdb.Id = rd.RiDataBatchId JOIN [Cedants] cd ON cd.Id = rdb.CedantId WHERE [RiDataBatchId] = {0} AND rd.TreatyCode LIKE CONCAT('%', cd.Code, '%') AND rd.[InsuredName] = r.InsuredName AND [InsuredDateOfBirth] = r.InsuredDateOfBirth AND rd.[InsuredGenderCode] <> r.InsuredGenderCode)", batchId);
            string script = string.Format("UPDATE [RiData] SET [ConflictType] = 1 WHERE [Id] IN ({0})", select);

            return script;
        }

        public static string ConflictScriptCountryWithinBatch(int batchId)
        {
            string select = string.Format("SELECT r.Id FROM [RiData] AS r LEFT JOIN [RiDataBatches] b ON b.Id = r.RiDataBatchId LEFT JOIN [Cedants] c ON c.Id = b.CedantId WHERE r.[RiDataBatchId] = {0} AND r.[TreatyCode] LIKE CONCAT('%', c.Code, '%') AND r.ConflictType = 0 AND EXISTS(SELECT 1 FROM [RiData] rd JOIN [RiDataBatches] rdb ON rdb.Id = rd.RiDataBatchId JOIN [Cedants] cd ON cd.Id = rdb.CedantId WHERE [RiDataBatchId] = {0} AND rd.TreatyCode LIKE CONCAT('%', cd.Code, '%') AND rd.[InsuredName] = r.InsuredName AND [InsuredDateOfBirth] = r.InsuredDateOfBirth AND rd.[TerritoryOfIssueCode] <> r.TerritoryOfIssueCode)", batchId);
            string script = string.Format("UPDATE [RiData] SET [ConflictType] = 2 WHERE [Id] IN ({0})", select);

            return script;
        }

        public static string ConflictScriptGenderWithWarehouse(int batchId)
        {
            string select = string.Format("SELECT r.Id FROM [RiData] AS r LEFT JOIN [RiDataBatches] b ON b.Id = r.RiDataBatchId LEFT JOIN [Cedants] c ON c.Id = b.CedantId WHERE r.[RiDataBatchId] = {0} AND r.[TreatyCode] LIKE CONCAT('%', c.Code, '%') AND r.ConflictType = 0 AND EXISTS(SELECT 1 FROM [RiDataWarehouse] WHERE [InsuredName] = r.InsuredName AND [InsuredDateOfBirth] = r.InsuredDateOfBirth AND [InsuredGenderCode] <> r.InsuredGenderCode)", batchId);
            string script = string.Format("UPDATE [RiData] SET [ConflictType] = 3 WHERE [Id] IN ({0})", select);

            return script;
        }

        public static string ConflictScriptCountryWithWarehouse(int batchId)
        {
            string select = string.Format("SELECT r.Id FROM [RiData] AS r LEFT JOIN [RiDataBatches] b ON b.Id = r.RiDataBatchId LEFT JOIN [Cedants] c ON c.Id = b.CedantId WHERE r.[RiDataBatchId] = {0} AND r.[TreatyCode] LIKE CONCAT('%', c.Code, '%') AND r.ConflictType = 0 AND EXISTS(SELECT 1 FROM [RiDataWarehouse] WHERE [InsuredName] = r.InsuredName AND [InsuredDateOfBirth] = r.InsuredDateOfBirth AND [TerritoryOfIssueCode] <> r.TerritoryOfIssueCode)", batchId);
            string script = string.Format("UPDATE [RiData] SET [ConflictType] = 4 WHERE [Id] IN ({0})", select);

            return script;
        }
    }
}
