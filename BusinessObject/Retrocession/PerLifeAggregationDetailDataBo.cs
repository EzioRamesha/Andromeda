using BusinessObject.RiDatas;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject.Retrocession
{
    public class PerLifeAggregationDetailDataBo
    {
        public int Id { get; set; }

        public int PerLifeAggregationDetailTreatyId { get; set; }

        public PerLifeAggregationDetailTreatyBo PerLifeAggregationDetailTreatyBo { get; set; }

        public int RiDataWarehouseHistoryId { get; set; }

        public RiDataWarehouseHistoryBo RiDataWarehouseHistoryBo { get; set; }

        public string ExpectedGenderCode { get; set; }

        public string RetroBenefitCode { get; set; }

        public string ExpectedTerritoryOfIssueCode { get; set; }

        public int? FlagCode { get; set; }

        public int? ExceptionType { get; set; }

        public int? ExceptionErrorType { get; set; }

        public bool IsException { get; set; }

        public string Errors { get; set; }

        public int ProceedStatus { get; set; }

        public string Remarks { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? UpdatedById { get; set; }

        // RI Data Warehouse History
        public string Quarter { get; set; }

        public int? EndingPolicyStatus { get; set; }

        public int RecordType { get; set; }

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

        public double? PolicyTerm { get; set; }

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

        public string Mfrs17BasicRider { get; set; }

        public string Mfrs17CellName { get; set; }

        public string Mfrs17TreatyCode { get; set; }

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

        public DateTime LastUpdatedDate { get; set; }

        public string RetroParty1 { get; set; }

        public string RetroParty2 { get; set; }

        public string RetroParty3 { get; set; }

        public double? RetroShare1 { get; set; }

        public double? RetroShare2 { get; set; }

        public double? RetroShare3 { get; set; }

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

        public double? AarShare2 { get; set; }

        public double? AarCap2 { get; set; }

        public double? WakalahFeePercentage { get; set; }

        public string TreatyNumber { get; set; }

        public const int ExceptionTypeFieldValidation = 1;
        public const int ExceptionTypeBasicCheck = 2;
        public const int ExceptionTypeConflictCheck = 3;
        public const int ExceptionTypeDuplicationCheck = 4;
        public const int ExceptionTypeRetroBenefitCodeMapping = 5;
        public const int ExceptionTypeRetroBenefitRetentionLimit = 6;

        public const int ExceptionTypeMax = 6;

        // FieldValidation
        public const int ExceptionErrorTypeMissingInfo = 1; // Value empty (Name, DOB, Gender, Currency Code)
        public const int ExceptionErrorTypeAarNull = 2; // AAR = null
        public const int ExceptionErrorTypeNetPremiumNull = 3; // Net Premium = null
        public const int ExceptionErrorTypeAarLessThanZero = 4; // AAR < 0
        public const int ExceptionErrorTypeNetPremiumLessThanZero = 5; // Net Premium < 0
        public const int ExceptionErrorTypeMissingNetPremiumZeroAarLessThanMinRetentionAmount = 6; // Net Premium = 0, AAR < Retention
        public const int ExceptionErrorTypeMissingNetPremiumZeroAarMoreOrEqualMinRetentionAmount = 7; // Net Premium = 0, AAR >= Retention

        // BasicCheck
        public const int ExceptionErrorTypeGenderNullOrNotPermitted = 8; // Gender = null || Gender not in maintenance
        public const int ExceptionErrorTypeTerritoryCodeNullOrNotPermitted = 9; // Territory Code = null || Territory Code not in maintenance
        public const int ExceptionErrorTypeReinsEffDateNullOrGreaterThanSystem = 10; // Reinsurance Effeective Date = null || Reinsurance Effeective Date > System Date (Now)
        public const int ExceptionErrorTypeUwNullOrLessThanZero = 11; // Underwriting Rating = null || Underwriting Rating < 100
        public const int ExceptionErrorTypeCedingTreatyCodeNull = 12; // Ceding Treaty Code = null
        public const int ExceptionErrorTypeTreatyCodeNull = 13; // Treaty Code = null

        // ConflictCheck
        public const int ExceptionErrorTypeSameLifeAssureConflictInGenderOrTerrritoryCode = 14; // Same LA with different Gender or Territory Code

        // DuplicationCheck
        public const int ExceptionErrorTypeDuplicationRecord = 15; // Duplicate record with same Risk Period Start Date and Risk Period End Date

        // RetroBenefitCodeMapping
        public const int ExceptionErrorTypeRetroBenefitCodeMapping = 16; // RetroBenefitCodeMapping not found

        // RetroBenefitRetentionLimit
        public const int ExceptionErrorTypeRetroBenefitRetentionLimit = 17; // RetroBenefitRetentionLimit not found

        public const int ExceptionErrorTypeMax = 17;

        public const int FlagCodeBad = 1;
        public const int FlagCodeGood1 = 2;
        public const int FlagCodeQ1 = 3;
        public const int FlagCodeQ2d1 = 4;
        public const int FlagCodeQ2d2 = 5;
        public const int FlagCodeQ2d3 = 6;
        public const int FlagCodeQ2d4 = 7;
        public const int FlagCodeQ2dA = 8;

        public const int FlagCodeMax = 8;

        public const int ProceedStatusProceed = 1;
        public const int ProceedStatusNotProceed = 2;
        public const int ProceedStatusPutOnHold = 3;

        public const int ProceedStatusMax = 3;

        public static string GetExceptionTypeName(int? key)
        {
            if (!key.HasValue)
                return "";

            switch (key.Value)
            {
                case ExceptionTypeFieldValidation:
                    return "Field Validation";
                case ExceptionTypeBasicCheck:
                    return "Basic Check";
                case ExceptionTypeConflictCheck:
                    return "Conflict Check";
                case ExceptionTypeDuplicationCheck:
                    return "Duplication Check";
                case ExceptionTypeRetroBenefitCodeMapping:
                    return "Retro Benefit Code Mapping not found";
                case ExceptionTypeRetroBenefitRetentionLimit:
                    return "Retro Benefit Retention Limit not found";
                default:
                    return "";
            }
        }

        public static string GetFlagCodeName(int? key)
        {
            if (!key.HasValue)
                return "";

            switch (key.Value)
            {
                case FlagCodeBad:
                    return "BAD";
                case FlagCodeGood1:
                    return "GOOD1";
                case FlagCodeQ1:
                    return "Q1";
                case FlagCodeQ2d1:
                    return "Q2-1";
                case FlagCodeQ2d2:
                    return "Q2-2";
                case FlagCodeQ2d3:
                    return "Q2-3";
                case FlagCodeQ2d4:
                    return "Q2-4";
                case FlagCodeQ2dA:
                    return "Q2-A";
                default:
                    return "";
            }
        }

        public static string GetProceedStatusName(int? key)
        {
            if (!key.HasValue)
                return "";

            switch (key.Value)
            {
                case ProceedStatusProceed:
                    return "Yes";
                case ProceedStatusNotProceed:
                    return "No";
                case ProceedStatusPutOnHold:
                    return "Put On Hold";
                default:
                    return "";
            }
        }

        public static List<Column> GetColumns()
        {
            return new List<Column>
            {
                new Column
                {
                    Header = "ID",
                    Property = "Id",
                },
                new Column
                {
                    Header = "RI_SOA",
                    //Property = "Id",
                },
                new Column
                {
                    Header = "TREATY_ID",
                    //Property = "Id",
                },
                new Column
                {
                    Header = "TREATY_CODE",
                    Property = "TreatyCode",
                },
                new Column
                {
                    Header = "CEDING_TREATY_CODE",
                    Property = "CedingTreatyCode",
                },
                new Column
                {
                    Header = "FAMILY_NAME",
                    Property = "InsuredName",
                },
                new Column
                {
                    Header = "GENDER_ID",
                    Property = "InsuredGenderCode",
                },
                new Column
                {
                    Header = "DATE_OF_BIRTH",
                    Property = "InsuredDateOfBirth",
                },
                new Column
                {
                    Header = "POLICY_NUMBER",
                    Property = "PolicyNumber",
                },
                new Column
                {
                    Header = "CURRENCY_ID",
                    Property = "CurrencyCode",
                },
                new Column
                {
                    Header = "TERRITORY_OF_ISSUE_ID",
                    Property = "TerritoryOfIssueCode",
                },
                new Column
                {
                    Header = "PLAN_TYPE_ID",
                    Property = "CedingPlanCode",
                },
                new Column
                {
                    Header = "CEDING_BENEFIT_CODE",
                    Property = "CedingBenefitRiskCode",
                },
                new Column
                {
                    Header = "CEDING_TYPE_ID",
                    Property = "CedingBenefitTypeCode",
                },
                new Column
                {
                    Header = "STANDARD_PREMIUM",
                    Property = "StandardPremium",
                },
                new Column
                {
                    Header = "SUBSTANDARD_PREMIUM",
                    Property = "SubstandardPremium",
                },
                new Column
                {
                    Header = "STANDARD_DISCOUNT",
                    Property = "StandardDiscount",
                },
                new Column
                {
                    Header = "SUBSTANDARD_DISCOUNT",
                    Property = "SubstandardDiscount",
                },
                new Column
                {
                    Header = "FLAT_EXTRA_PREMIUM",
                    Property = "FlatExtraPremium",
                },
                new Column
                {
                    Header = "FLAT_AMOUNT",
                    Property = "FlatExtraAmount",
                },
                new Column
                {
                    Header = "FLAT_EXTRA_AMOUNT_PERM",
                    //Property = "FlatExtraAmount",
                },
                new Column
                {
                    Header = "FLAT_EXTRA_AMOUNT_TEMP",
                    //Property = "FlatExtraAmount",
                },
                new Column
                {
                    Header = "BROKERAGE_FEE",
                    Property = "BrokerageFee",
                },
                new Column
                {
                    Header = "RISK_START_DATE",
                    Property = "RiskPeriodStartDate",
                },
                new Column
                {
                    Header = "RISK_END_DATE",
                    Property = "RiskPeriodEndDate",
                },
                new Column
                {
                    Header = "TRANSACTION_TYPE",
                    Property = "TransactionTypeCode",
                },
                new Column
                {
                    Header = "EFFECTIVE_DATE",
                    Property = "EffectiveDate",
                },
                new Column
                {
                    Header = "POLICY_TERM",
                    Property = "PolicyTerm",
                },
                new Column
                {
                    Header = "POLICY_EXPIRY_DATE",
                    Property = "PolicyExpiryDate",
                },
            };
        }

        public static List<Column> GetExceptionColumns()
        {
            return new List<Column>
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
                    Header = "File ID",
                    //Property = "RecordType",
                },
                new Column
                {
                    Header = "Original Entry",
                    //Property = "RecordType",
                },
                new Column
                {
                    Header = "Policy Number",
                    Property = "PolicyNumber",
                },
                new Column
                {
                    Header = "Policy Issue Date",
                    Property = "IssueDatePol",
                },
                new Column
                {
                    Header = "Treaty Code",
                    Property = "TreatyCode",
                },
                new Column
                {
                    Header = "Reinsurance Effective Date",
                    Property = "ReinsEffDatePol",
                },
                new Column
                {
                    Header = "Insured Name",
                    Property = "InsuredName",
                },
                new Column
                {
                    Header = "Insured Gender Code",
                    Property = "InsuredGenderCode",
                },
                new Column
                {
                    Header = "Insured Date of Birth",
                    Property = "InsuredDateOfBirth",
                },
                new Column
                {
                    Header = "Ceding Plan Code",
                    Property = "CedingPlanCode",
                },
                new Column
                {
                    Header = "Ceding Benefit Type Code",
                    Property = "ExCedingBenefitTypeCode",
                },
                new Column
                {
                    Header = "Ceding Benefit Risk Code",
                    Property = "ExCedingBenefitRiskCode",
                },
                new Column
                {
                    Header = "MLRe Benefit Code",
                    Property = "MlreBenefitCode",
                },
                new Column
                {
                    Header = "AAR",
                    Property = "Aar",
                },
                new Column
                {
                    Header = "Status",
                    Property = "ExceptionType",
                },
                new Column
                {
                    Header = "Proceed Status",
                    Property = "ProceedStatus",
                },
            };
        }

        public static List<Column> GetExcludedRecordColumns()
        {
            return new List<Column>
            {
                new Column
                {
                    Header = "ID",
                    Property = "Id",
                },
                new Column
                {
                    Header = "Treaty Code",
                    Property = "TreatyCode",
                },
                new Column
                {
                    Header = "Policy Number",
                    Property = "PolicyNumber",
                },
                new Column
                {
                    Header = "Policy Status",
                    Property = "PolicyStatusCode",
                },
                new Column
                {
                    Header = "AAR",
                    Property = "Aar",
                },
                new Column
                {
                    Header = "Prem",
                    Property = "NetPremium",
                },
                new Column
                {
                    Header = "Frequency Mode",
                    Property = "PremiumFrequencyCode",
                },
                new Column
                {
                    Header = "Risk Month",
                    Property = "RiskPeriodMonth",
                },
                new Column
                {
                    Header = "Risk Year",
                    Property = "RiskPeriodYear",
                },
                new Column
                {
                    Header = "Last Updated Date",
                    Property = "LastUpdatedDate",
                },
                new Column
                {
                    Header = "Risk Start Date",
                    Property = "RiskPeriodStartDate",
                },
                new Column
                {
                    Header = "Risk End Date",
                    Property = "RiskPeriodEndDate",
                },
                new Column
                {
                    Header = "MLRe Benefit Code",
                    Property = "MlreBenefitCode",
                },
                new Column
                {
                    Header = "Exception Type",
                    Property = "ExceptionType",
                },
            };
        }

        public static List<Column> GetDuplicationListingColumns()
        {
            return new List<Column>
            {
                new Column
                {
                    Header = "ID",
                    Property = "Id",
                },
                new Column
                {
                    Header = "Treaty Code",
                    Property = "TreatyCode",
                },
                new Column
                {
                    Header = "Insured Name",
                    Property = "InsuredName",
                },
                new Column
                {
                    Header = "Insured Gender Code",
                    Property = "InsuredGenderCode",
                },
                new Column
                {
                    Header = "Insured Date of Birth",
                    Property = "InsuredDateOfBirth",
                },
                new Column
                {
                    Header = "Policy Number",
                    Property = "PolicyNumber",
                },
                new Column
                {
                    Header = "Reinsurance Eff Date Pol",
                    Property = "ReinsEffDatePol",
                },
                new Column
                {
                    Header = "Funds Accounting Type",
                    Property = "FundsAccountingTypeCode",
                },
                new Column
                {
                    Header = "Reinsurance Risk Basis ID",
                    Property = "ReinsBasisCode",
                },
                new Column
                {
                    Header = "Ceding Plan Code",
                    Property = "CedingPlanCode",
                },
                new Column
                {
                    Header = "MLRe Benefit Code",
                    Property = "MlreBenefitCode",
                },
                new Column
                {
                    Header = "Ceding Benefit Risk Code",
                    Property = "CedingBenefitRiskCode",
                },
                new Column
                {
                    Header = "Ceding Benefit Type Code",
                    Property = "CedingBenefitTypeCode",
                },
                new Column
                {
                    Header = "Transaction Type",
                    Property = "TransactionTypeCode",
                },
                new Column
                {
                    Header = "Proceed To Aggregate",
                    Property = "ProceedStatus",
                },
                new Column
                {
                    Header = "Date Updated",
                    Property = "UpdatedAt",
                },
                new Column
                {
                    Header = "Exception Status",
                    Property = "ExceptionStatus",
                },
                new Column
                {
                    Header = "Remarks",
                    Property = "Remarks",
                },
            };
        }

        public static List<Column> GetConflictListingColumns()
        {
            return new List<Column>
            {
                new Column
                {
                    Header = "ID",
                    Property = "Id",
                },
                new Column
                {
                    Header = "Treaty Code",
                    Property = "TreatyCode",
                },
                new Column
                {
                    Header = "Risk Year",
                    Property = "RiskPeriodYear",
                },
                new Column
                {
                    Header = "Risk Month",
                    Property = "RiskPeriodMonth",
                },
                new Column
                {
                    Header = "Insured Name",
                    Property = "InsuredName",
                },
                new Column
                {
                    Header = "Insured Gender Code",
                    Property = "InsuredGenderCode",
                },
                new Column
                {
                    Header = "Insured Date of Birth",
                    Property = "InsuredDateOfBirth",
                },
                new Column
                {
                    Header = "Policy Number",
                    Property = "PolicyNumber",
                },
                new Column
                {
                    Header = "Reinsurance Eff Date Pol",
                    Property = "ReinsEffDatePol",
                },
                new Column
                {
                    Header = "Aar",
                    Property = "Aar",
                },
                new Column
                {
                    Header = "Gross Premium",
                    Property = "GrossPremium",
                },
                new Column
                {
                    Header = "Net Premium",
                    Property = "NetPremium",
                },
                new Column
                {
                    Header = "Premium Frequency",
                    Property = "PremiumFrequencyCode",
                },
                new Column
                {
                    Header = "Retro Premium Frequency",
                    Property = "RetroPremiumFrequencyMode",
                },
                new Column
                {
                    Header = "Ceding Plan Code",
                    Property = "CedingPlanCode",
                },
                new Column
                {
                    Header = "Ceding Benefit Type Code",
                    Property = "CedingBenefitTypeCode",
                },
                new Column
                {
                    Header = "Ceding Benefit Risk Code",
                    Property = "CedingBenefitRiskCode",
                },
                new Column
                {
                    Header = "MLRe Benefit Code",
                    Property = "MlreBenefitCode",
                },
            };
        }
    }
}
