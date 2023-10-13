using Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessObject
{
    public class StandardOutputBo
    {
        public int Id { get; set; }

        public int Type { get; set; }
        public string TypeName { get; set; }

        public int DataType { get; set; }
        public string DataTypeName { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }

        public string Property { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string DummyValue { get; set; }

        public const string StartingDelimiter = "{";
        public const string EndingDelimiter = "}";

        // Used in Eval only
        public bool IsOriginal { get; set; }

        public const int TypeCustomField = 1;
        public const int TypeTreatyCode = 2;
        public const int TypeReinsBasisCode = 3;
        public const int TypeFundsAccountingTypeCode = 4;
        public const int TypePremiumFrequencyCode = 5;
        public const int TypeReportPeriodMonth = 6;
        public const int TypeReportPeriodYear = 7;
        public const int TypeRiskPeriodMonth = 8;
        public const int TypeRiskPeriodYear = 9;
        public const int TypeTransactionTypeCode = 10;
        public const int TypePolicyNumber = 11;
        public const int TypeIssueDatePol = 12;
        public const int TypeIssueDateBen = 13;
        public const int TypeReinsEffDatePol = 14;
        public const int TypeReinsEffDateBen = 15;
        public const int TypeCedingPlanCode = 16;
        public const int TypeCedingBenefitTypeCode = 17;
        public const int TypeCedingBenefitRiskCode = 18;
        public const int TypeMlreBenefitCode = 19;
        public const int TypeOriSumAssured = 20;
        public const int TypeCurrSumAssured = 21;
        public const int TypeAmountCededB4MlreShare = 22;
        public const int TypeRetentionAmount = 23;
        public const int TypeAarOri = 24;
        public const int TypeAar = 25;
        public const int TypeAarSpecial1 = 26;
        public const int TypeAarSpecial2 = 27;
        public const int TypeAarSpecial3 = 28;
        public const int TypeInsuredName = 29;
        public const int TypeInsuredGenderCode = 30;
        public const int TypeInsuredTobaccoUse = 31;
        public const int TypeInsuredDateOfBirth = 32;
        public const int TypeInsuredOccupationCode = 33;
        public const int TypeInsuredRegisterNo = 34;
        public const int TypeInsuredAttainedAge = 35;
        public const int TypeInsuredNewIcNumber = 36;
        public const int TypeInsuredOldIcNumber = 37;
        public const int TypeInsuredName2nd = 38;
        public const int TypeInsuredGenderCode2nd = 39;
        public const int TypeInsuredTobaccoUse2nd = 40;
        public const int TypeInsuredDateOfBirth2nd = 41;
        public const int TypeInsuredAttainedAge2nd = 42;
        public const int TypeInsuredNewIcNumber2nd = 43;
        public const int TypeInsuredOldIcNumber2nd = 44;
        public const int TypeReinsuranceIssueAge = 45;
        public const int TypeReinsuranceIssueAge2nd = 46;
        public const int TypePolicyTerm = 47;
        public const int TypePolicyExpiryDate = 48;
        public const int TypeDurationYear = 49;
        public const int TypeDurationDay = 50;
        public const int TypeDurationMonth = 51;
        public const int TypePremiumCalType = 52;
        public const int TypeCedantRiRate = 53;
        public const int TypeRateTable = 54;
        public const int TypeAgeRatedUp = 55;
        public const int TypeDiscountRate = 56;
        public const int TypeLoadingType = 57;
        public const int TypeUnderwriterRating = 58;
        public const int TypeUnderwriterRatingUnit = 59;
        public const int TypeUnderwriterRatingTerm = 60;
        public const int TypeUnderwriterRating2 = 61;
        public const int TypeUnderwriterRatingUnit2 = 62;
        public const int TypeUnderwriterRatingTerm2 = 63;
        public const int TypeUnderwriterRating3 = 64;
        public const int TypeUnderwriterRatingUnit3 = 65;
        public const int TypeUnderwriterRatingTerm3 = 66;
        public const int TypeFlatExtraAmount = 67;
        public const int TypeFlatExtraUnit = 68;
        public const int TypeFlatExtraTerm = 69;
        public const int TypeFlatExtraAmount2 = 70;
        public const int TypeFlatExtraTerm2 = 71;
        public const int TypeStandardPremium = 72;
        public const int TypeSubstandardPremium = 73;
        public const int TypeFlatExtraPremium = 74;
        public const int TypeGrossPremium = 75;
        public const int TypeStandardDiscount = 76;
        public const int TypeSubstandardDiscount = 77;
        public const int TypeVitalityDiscount = 78;
        public const int TypeTotalDiscount = 79;
        public const int TypeNetPremium = 80;
        public const int TypeAnnualRiPrem = 81;
        public const int TypeRiCovPeriod = 82;
        public const int TypeAdjBeginDate = 83;
        public const int TypeAdjEndDate = 84;
        public const int TypePolicyNumberOld = 85;
        public const int TypePolicyStatusCode = 86;
        public const int TypePolicyGrossPremium = 87;
        public const int TypePolicyStandardPremium = 88;
        public const int TypePolicySubstandardPremium = 89;
        public const int TypePolicyTermRemain = 90;
        public const int TypePolicyAmountDeath = 91;
        public const int TypePolicyReserve = 92;
        public const int TypePolicyPaymentMethod = 93;
        public const int TypePolicyLifeNumber = 94;
        public const int TypeFundCode = 95;
        public const int TypeLineOfBusiness = 96;
        public const int TypeApLoading = 97;
        public const int TypeLoanInterestRate = 98;
        public const int TypeDefermentPeriod = 99;
        public const int TypeRiderNumber = 100;
        public const int TypeCampaignCode = 101;
        public const int TypeNationality = 102;
        public const int TypeTerritoryOfIssueCode = 103;
        public const int TypeCurrencyCode = 104;
        public const int TypeStaffPlanIndicator = 105;
        public const int TypeCedingTreatyCode = 106;
        public const int TypeCedingPlanCodeOld = 107;
        public const int TypeCedingBasicPlanCode = 108;
        public const int TypeCedantSar = 109;
        public const int TypeCedantReinsurerCode = 110;
        public const int TypeAmountCededB4MlreShare2 = 111;
        public const int TypeCessionCode = 112;
        public const int TypeCedantRemark = 113;
        public const int TypeGroupPolicyNumber = 114;
        public const int TypeGroupPolicyName = 115;
        public const int TypeNoOfEmployee = 116;
        public const int TypePolicyTotalLive = 117;
        public const int TypeGroupSubsidiaryName = 118;
        public const int TypeGroupSubsidiaryNo = 119;
        public const int TypeGroupEmployeeBasicSalary = 120;
        public const int TypeGroupEmployeeJobType = 121;
        public const int TypeGroupEmployeeJobCode = 122;
        public const int TypeGroupEmployeeBasicSalaryRevise = 123;
        public const int TypeGroupEmployeeBasicSalaryMultiplier = 124;
        public const int TypeCedingPlanCode2 = 125;
        public const int TypeDependantIndicator = 126;
        public const int TypeGhsRoomBoard = 127;
        public const int TypePolicyAmountSubstandard = 128;
        public const int TypeLayer1RiShare = 129;
        public const int TypeLayer1InsuredAttainedAge = 130;
        public const int TypeLayer1InsuredAttainedAge2nd = 131;
        public const int TypeLayer1StandardPremium = 132;
        public const int TypeLayer1SubstandardPremium = 133;
        public const int TypeLayer1GrossPremium = 134;
        public const int TypeLayer1StandardDiscount = 135;
        public const int TypeLayer1SubstandardDiscount = 136;
        public const int TypeLayer1TotalDiscount = 137;
        public const int TypeLayer1NetPremium = 138;
        public const int TypeLayer1GrossPremiumAlt = 139;
        public const int TypeLayer1TotalDiscountAlt = 140;
        public const int TypeLayer1NetPremiumAlt = 141;
        public const int TypeSpecialIndicator1 = 142;
        public const int TypeSpecialIndicator2 = 143;
        public const int TypeSpecialIndicator3 = 144;
        public const int TypeIndicatorJointLife = 145;
        public const int TypeTaxAmount = 146;
        public const int TypeGstIndicator = 147;
        public const int TypeGstGrossPremium = 148;
        public const int TypeGstTotalDiscount = 149;
        public const int TypeGstVitality = 150;
        public const int TypeGstAmount = 151;
        public const int TypeMfrs17BasicRider = 152;
        public const int TypeMfrs17CellName = 153;
        public const int TypeMfrs17TreatyCode = 154;
        public const int TypeLoaCode = 155;
        public const int TypeTempD1 = 156;
        public const int TypeTempD2 = 157;
        public const int TypeTempD3 = 158;
        public const int TypeTempD4 = 159;
        public const int TypeTempD5 = 160;
        public const int TypeTempS1 = 161;
        public const int TypeTempS2 = 162;
        public const int TypeTempS3 = 163;
        public const int TypeTempS4 = 164;
        public const int TypeTempS5 = 165;
        public const int TypeTempI1 = 166;
        public const int TypeTempI2 = 167;
        public const int TypeTempI3 = 168;
        public const int TypeTempI4 = 169;
        public const int TypeTempI5 = 170;
        public const int TypeTempA1 = 171;
        public const int TypeTempA2 = 172;
        public const int TypeTempA3 = 173;
        public const int TypeTempA4 = 174;
        public const int TypeTempA5 = 175;
        public const int TypeTempA6 = 176;
        public const int TypeTempA7 = 177;
        public const int TypeTempA8 = 178;

        // Phase 2
        public const int TypeCurrencyRate = 179;
        public const int TypeNoClaimBonus = 180;
        public const int TypeSurrenderValue = 181;
        public const int TypeDatabaseCommision = 182;
        public const int TypeGrossPremiumAlt = 183;
        public const int TypeNetPremiumAlt = 184;
        public const int TypeLayer1FlatExtraPremium = 185;
        public const int TypeTransactionPremium = 186;
        public const int TypeOriginalPremium = 187;
        public const int TypeTransactionDiscount = 188;
        public const int TypeOriginalDiscount = 189;
        public const int TypeBrokerageFee = 190;
        public const int TypeMaxUwRating = 191;
        public const int TypeRetentionCap = 192;
        public const int TypeAarCap = 193;
        public const int TypeRiRate = 194;
        public const int TypeRiRate2 = 195;
        public const int TypeAnnuityFactor = 196;
        public const int TypeSumAssuredOffered = 197;
        public const int TypeUwRatingOffered = 198;
        public const int TypeFlatExtraAmountOffered = 199;
        public const int TypeFlatExtraDuration = 200;
        public const int TypeEffectiveDate = 201;
        public const int TypeOfferLetterSentDate = 202;
        public const int TypeRiskPeriodStartDate = 203;
        public const int TypeRiskPeriodEndDate = 204;
        public const int TypeMfrs17AnnualCohort = 205;
        public const int TypeMaxExpiryAge = 206;
        public const int TypeMinIssueAge = 207;
        public const int TypeMaxIssueAge = 208;
        public const int TypeMinAar = 209;
        public const int TypeMaxAar = 210;
        public const int TypeCorridorLimit = 211;
        public const int TypeAbl = 212;
        public const int TypeRatePerBasisUnit = 213;
        public const int TypeRiDiscountRate = 214;
        public const int TypeLargeSaDiscount = 215;
        public const int TypeGroupSizeDiscount = 216;
        public const int TypeEwarpNumber = 217;
        public const int TypeEwarpActionCode = 218;
        public const int TypeRetentionShare = 219;
        public const int TypeAarShare = 220;
        public const int TypeProfitComm = 221;
        public const int TypeTotalDirectRetroAar = 222;
        public const int TypeTotalDirectRetroGrossPremium = 223;
        public const int TypeTotalDirectRetroDiscount = 224;
        public const int TypeTotalDirectRetroNetPremium = 225;
        public const int TypeTreatyType = 226;
        public const int TypeMaxApLoading = 227;
        public const int TypeMlreInsuredAttainedAgeAtCurrentMonth = 228;
        public const int TypeMlreInsuredAttainedAgeAtPreviousMonth = 229;
        public const int TypeInsuredAttainedAgeCheck = 230;
        public const int TypeMaxExpiryAgeCheck = 231;
        public const int TypeMlrePolicyIssueAge = 232;
        public const int TypePolicyIssueAgeCheck = 233;
        public const int TypeMinIssueAgeCheck = 234;
        public const int TypeMaxIssueAgeCheck = 235;
        public const int TypeMaxUwRatingCheck = 236;
        public const int TypeApLoadingCheck = 237;
        public const int TypeEffectiveDateCheck = 238;
        public const int TypeMinAarCheck = 239;
        public const int TypeMaxAarCheck = 240;
        public const int TypeCorridorLimitCheck = 241;
        public const int TypeAblCheck = 242;
        public const int TypeRetentionCheck = 243;
        public const int TypeAarCheck = 244;
        public const int TypeMlreStandardPremium = 245;
        public const int TypeMlreSubstandardPremium = 246;
        public const int TypeMlreFlatExtraPremium = 247;
        public const int TypeMlreGrossPremium = 248;
        public const int TypeMlreStandardDiscount = 249;
        public const int TypeMlreSubstandardDiscount = 250;
        public const int TypeMlreLargeSaDiscount = 251;
        public const int TypeMlreGroupSizeDiscount = 252;
        public const int TypeMlreVitalityDiscount = 253;
        public const int TypeMlreTotalDiscount = 254;
        public const int TypeMlreNetPremium = 255;
        public const int TypeNetPremiumCheck = 256;
        public const int TypeServiceFeePercentage = 257;
        public const int TypeServiceFee = 258;
        public const int TypeMlreBrokerageFee = 259;
        public const int TypeMlreDatabaseCommission = 260;
        public const int TypeValidityDayCheck = 261;
        public const int TypeSumAssuredOfferedCheck = 262;
        public const int TypeUwRatingCheck = 263;
        public const int TypeFlatExtraAmountCheck = 264;
        public const int TypeFlatExtraDurationCheck = 265;
        public const int TypeAarShare2 = 266;
        public const int TypeAarCap2 = 267;
        public const int TypeWakalahFeePercentage = 268;
        public const int TypeTreatyNumber = 269;
        public const int TypeRecordType = 270;

        public const int TypeMax = 270;

        public const int DataTypeDate = 1;
        public const int DataTypeString = 2;
        public const int DataTypeAmount = 3;
        public const int DataTypePercentage = 4;
        public const int DataTypeInteger = 5;
        public const int DataTypeDropDown = 6;
        public const int DataTypeBoolean = 7;

        public const int DataTypeMax = 7; // Phase 2

        public static List<string> GetNames()
        {
            return new List<string>
            {
                "Custom Field",
                "Treaty Code",
                "Reins Basis Code",
                "Funds Accounting Type Code",
                "Premium Frequency Code",
                "Report Period Month",
                "Report Period Year",
                "Risk Period Month",
                "Risk Period Year",
                "Transaction Type Code",
                "Policy Number",
                "Issue Date Pol",
                "Issue Date Ben",
                "Reins Eff Date Pol",
                "Reins Eff Date Ben",
                "Ceding Plan Code",
                "Ceding Benefit Type Code",
                "Ceding Benefit Risk Code",
                "MLRE Benefit Code",
                "Ori Sum Assured",
                "Curr Sum Assured",
                "Amount Ceded B4 MLRE Share",
                "Retention Amount",
                "AAR Ori",
                "AAR",
                "AAR Special 1",
                "AAR Special 2",
                "AAR Special 3",
                "Insured Name",
                "Insured Gender Code",
                "Insured Tobacco Use",
                "Insured Date Of Birth",
                "Insured Occupation Code",
                "Insured Register No.",
                "Insured Attained Age",
                "Insured New Ic Number",
                "Insured Old Ic Number",
                "Insured Name 2",
                "Insured Gender Code 2",
                "Insured Tobacco Use 2",
                "Insured Date Of Birth 2",
                "Insured Attained Age 2",
                "Insured New Ic Number 2",
                "Insured Old Ic Number 2",
                "Reinsurance Issue Age",
                "Reinsurance Issue Age 2",
                "Policy Term",
                "Policy Expiry Date",
                "Duration Year",
                "Duration Day",
                "Duration Month",
                "Premium Cal Type",
                "Cedant Ri Rate",
                "Rate Table",
                "Age Rated Up",
                "Discount Rate",
                "Loading Type",
                "Underwriter Rating",
                "Underwriter Rating Unit",
                "Underwriter Rating Term",
                "Underwriter Rating 2",
                "Underwriter Rating Unit 2",
                "Underwriter Rating Term 2",
                "Underwriter Rating 3",
                "Underwriter Rating Unit 3",
                "Underwriter Rating Term 3",
                "Flat Extra Amount",
                "Flat Extra Unit",
                "Flat Extra Term",
                "Flat Extra Amount 2",
                "Flat Extra Term 2",
                "Standard Premium",
                "Substandard Premium",
                "Flat Extra Premium",
                "Gross Premium",
                "Standard Discount",
                "Substandard Discount",
                "Vitality Discount",
                "Total Discount",
                "Net Premium",
                "Annual Ri Prem",
                "Ri Cov Period",
                "Adj. Begin Date",
                "Adj. End Date",
                "Policy Number Old",
                "Policy Status Code",
                "Policy Gross Premium",
                "Policy Standard Premium",
                "Policy Substandard Premium",
                "Policy Term Remain",
                "Policy Amount Death",
                "Policy Reserve",
                "Policy Payment Method",
                "Policy Life Number",
                "Fund Code",
                "Line Of Business",
                "Ap Loading",
                "Loan Interest Rate",
                "Deferment Period",
                "Rider Number",
                "Campaign Code",
                "Nationality",
                "Territory Of Issue Code",
                "Currency Code",
                "Staff Plan Indicator",
                "Ceding Treaty Code",
                "Ceding Plan Code Old",
                "Ceding Basic Plan Code",
                "Cedant Sar",
                "Cedant Reinsurer Code",
                "Amount Ceded B4 MLRE Share T1",
                "Cession Code",
                "Cedant Remark",
                "Group Policy Number",
                "Group Policy Name",
                "No. Of Employee",
                "Policy Total Live",
                "Group Subsidiary Name",
                "Group Subsidiary No.",
                "Group Employee Basic Salary",
                "Group Employee Job Type",
                "Group Employee Job Code",
                "Group Employee Basic Salary Revise",
                "Group Employee Basic Salary Multiplier",
                "Ceding Plan Code 2",
                "Dependant Indicator",
                "Ghs Room Board",
                "Policy Amount Substandard",
                "Layer 1 Ri Share",
                "Layer 1 Insured Attained Age",
                "Layer 1 Insured Attained Age 2",
                "Layer 1 Standard Premium",
                "Layer 1 Substandard Premium",
                "Layer 1 Gross Premium",
                "Layer 1 Standard Discount",
                "Layer 1 Substandard Discount",
                "Layer 1 Total Discount",
                "Layer 1 Net Premium",
                "Layer 1 Gross Premium Alt",
                "Layer 1 Total Discount Alt",
                "Layer 1 Net Premium Alt",
                "Special Indicator 1",
                "Special Indicator 2",
                "Special Indicator 3",
                "Indicator Joint Life",
                "Tax Amount",
                "Gst Indicator",
                "Gst Gross Premium",
                "Gst Total Discount",
                "Gst Vitality",
                "Gst Amount",
                "MFRS17 Basic Rider",
                "MFRS17 Cell Name",
                "MFRS17 Contract Code", // Phase 2: MFRS17 Contract Code
                "Loa Code",
                "Temp D1",
                "Temp D2",
                "Temp D3",
                "Temp D4",
                "Temp D5",
                "Temp S1",
                "Temp S2",
                "Temp S3",
                "Temp S4",
                "Temp S5",
                "Temp I1",
                "Temp I2",
                "Temp I3",
                "Temp I4",
                "Temp I5",
                "Temp A1",
                "Temp A2",
                "Temp A3",
                "Temp A4",
                "Temp A5",
                "Temp A6",
                "Temp A7",
                "Temp A8",

                // Phase 2
                "Currency Rate",
                "No Claim Bonus",
                "Surrender Value",
                "Database Commision",
                "Gross Premium Alt",
                "Net Premium Alt",
                "Layer 1 Flat Extra Premium",
                "Transaction Premium",
                "Original Premium",
                "Transaction Discount",
                "Original Discount",
                "Brokerage Fee",
                "Max UW Rating",
                "Retention Cap",
                "AAR Cap 1",
                "RI Rate",
                "RI Rate 2",
                "Annuity Factor",
                "Sum Assured Offered",
                "UW Rating Offered",
                "Flat Extra Amount Offered",
                "Flat Extra Duration",
                "Effective Date",
                "Offer Letter Sent Date",
                "Risk Period Start Date",
                "Risk Period End Date",
                "MFRS17 Annual Cohort",
                "Max Expiry Age",
                "Min Issue Age",
                "Max Issue Age",
                "Min AAR",
                "Max AAR",
                "Corridor Limit",
                "ABL",
                "Rate Per Basis Unit",
                "RI Discount Rate",
                "Large SA Discount",
                "Group Size Discount",
                "eWarp Number",
                "eWarp Action Code",
                "Retention Share",
                "AAR Share 1",
                "Profit Comm",
                "Total Direct Retro AAR",
                "Total Direct Retro Gross Premium",
                "Total Direct Retro Discount",
                "Total Direct Retro Net Premium",
                "Treaty Type",
                "Max AP Loading",
                "MLRe Insured Attained Age at Current Month",
                "MLRe Insured Attained Age at Previous Month",
                "Insured Attained Age Check",
                "Max Expiry Age Check",
                "MLRe Policy Issue Age",
                "Policy Issue Age Check",
                "Min Issue Age Check",
                "Max Issue Age Check",
                "Max UW Rating Check",
                "AP Loading Check",
                "Effective Date Check",
                "Min AAR Check",
                "Max AAR Check",
                "Corridor Limit Check",
                "ABL Check",
                "Retention Check",
                "AAR Check",
                "MLRe Standard Premium",
                "MLRe Substandard Premium",
                "MLRe Flat Extra Premium",
                "MLRe Gross Premium",
                "MLRe Standard Discount",
                "MLRe Substandard Discount",
                "MLRe Large SA Discount",
                "MLRe Group Size Discount",
                "MLRe Vitality Discount",
                "MLRe Total Discount",
                "MLRe Net Premium",
                "Net Premium Check",
                "Service Fee %",
                "Service Fee",
                "MLRe Brokerage Fee",
                "MLRe Database Commission",
                "Validity Day Check",
                "Sum Assured Offered Check",
                "UW Rating Check",
                "Flat Extra Amount Check",
                "Flat Extra Duration Check",
                "AAR Share 2",
                "AAR Cap 2",
                "Wakalah Fee %",
                "Treaty Number",
                "Record Type",
            };
        }

        public static List<string> GetPropertyNames()
        {
            return new List<string>
            {
                "CustomField",
                "TreatyCode",
                "ReinsBasisCode",
                "FundsAccountingTypeCode",
                "PremiumFrequencyCode",
                "ReportPeriodMonth",
                "ReportPeriodYear",
                "RiskPeriodMonth",
                "RiskPeriodYear",
                "TransactionTypeCode",
                "PolicyNumber",
                "IssueDatePol",
                "IssueDateBen",
                "ReinsEffDatePol",
                "ReinsEffDateBen",
                "CedingPlanCode",
                "CedingBenefitTypeCode",
                "CedingBenefitRiskCode",
                "MlreBenefitCode",
                "OriSumAssured",
                "CurrSumAssured",
                "AmountCededB4MlreShare",
                "RetentionAmount",
                "AarOri",
                "Aar",
                "AarSpecial1",
                "AarSpecial2",
                "AarSpecial3",
                "InsuredName",
                "InsuredGenderCode",
                "InsuredTobaccoUse",
                "InsuredDateOfBirth",
                "InsuredOccupationCode",
                "InsuredRegisterNo",
                "InsuredAttainedAge",
                "InsuredNewIcNumber",
                "InsuredOldIcNumber",
                "InsuredName2nd",
                "InsuredGenderCode2nd",
                "InsuredTobaccoUse2nd",
                "InsuredDateOfBirth2nd",
                "InsuredAttainedAge2nd",
                "InsuredNewIcNumber2nd",
                "InsuredOldIcNumber2nd",
                "ReinsuranceIssueAge",
                "ReinsuranceIssueAge2nd",
                "PolicyTerm",
                "PolicyExpiryDate",
                "DurationYear",
                "DurationDay",
                "DurationMonth",
                "PremiumCalType",
                "CedantRiRate",
                "RateTable",
                "AgeRatedUp",
                "DiscountRate",
                "LoadingType",
                "UnderwriterRating",
                "UnderwriterRatingUnit",
                "UnderwriterRatingTerm",
                "UnderwriterRating2",
                "UnderwriterRatingUnit2",
                "UnderwriterRatingTerm2",
                "UnderwriterRating3",
                "UnderwriterRatingUnit3",
                "UnderwriterRatingTerm3",
                "FlatExtraAmount",
                "FlatExtraUnit",
                "FlatExtraTerm",
                "FlatExtraAmount2",
                "FlatExtraTerm2",
                "StandardPremium",
                "SubstandardPremium",
                "FlatExtraPremium",
                "GrossPremium",
                "StandardDiscount",
                "SubstandardDiscount",
                "VitalityDiscount",
                "TotalDiscount",
                "NetPremium",
                "AnnualRiPrem",
                "RiCovPeriod",
                "AdjBeginDate",
                "AdjEndDate",
                "PolicyNumberOld",
                "PolicyStatusCode",
                "PolicyGrossPremium",
                "PolicyStandardPremium",
                "PolicySubstandardPremium",
                "PolicyTermRemain",
                "PolicyAmountDeath",
                "PolicyReserve",
                "PolicyPaymentMethod",
                "PolicyLifeNumber",
                "FundCode",
                "LineOfBusiness",
                "ApLoading",
                "LoanInterestRate",
                "DefermentPeriod",
                "RiderNumber",
                "CampaignCode",
                "Nationality",
                "TerritoryOfIssueCode",
                "CurrencyCode",
                "StaffPlanIndicator",
                "CedingTreatyCode",
                "CedingPlanCodeOld",
                "CedingBasicPlanCode",
                "CedantSar",
                "CedantReinsurerCode",
                "AmountCededB4MlreShare2",
                "CessionCode",
                "CedantRemark",
                "GroupPolicyNumber",
                "GroupPolicyName",
                "NoOfEmployee",
                "PolicyTotalLive",
                "GroupSubsidiaryName",
                "GroupSubsidiaryNo",
                "GroupEmployeeBasicSalary",
                "GroupEmployeeJobType",
                "GroupEmployeeJobCode",
                "GroupEmployeeBasicSalaryRevise",
                "GroupEmployeeBasicSalaryMultiplier",
                "CedingPlanCode2",
                "DependantIndicator",
                "GhsRoomBoard",
                "PolicyAmountSubstandard",
                "Layer1RiShare",
                "Layer1InsuredAttainedAge",
                "Layer1InsuredAttainedAge2nd",
                "Layer1StandardPremium",
                "Layer1SubstandardPremium",
                "Layer1GrossPremium",
                "Layer1StandardDiscount",
                "Layer1SubstandardDiscount",
                "Layer1TotalDiscount",
                "Layer1NetPremium",
                "Layer1GrossPremiumAlt",
                "Layer1TotalDiscountAlt",
                "Layer1NetPremiumAlt",
                "SpecialIndicator1",
                "SpecialIndicator2",
                "SpecialIndicator3",
                "IndicatorJointLife",
                "TaxAmount",
                "GstIndicator",
                "GstGrossPremium",
                "GstTotalDiscount",
                "GstVitality",
                "GstAmount",
                "Mfrs17BasicRider",
                "Mfrs17CellName",
                "Mfrs17TreatyCode",
                "LoaCode",
                "TempD1",
                "TempD2",
                "TempD3",
                "TempD4",
                "TempD5",
                "TempS1",
                "TempS2",
                "TempS3",
                "TempS4",
                "TempS5",
                "TempI1",
                "TempI2",
                "TempI3",
                "TempI4",
                "TempI5",
                "TempA1",
                "TempA2",
                "TempA3",
                "TempA4",
                "TempA5",
                "TempA6",
                "TempA7",
                "TempA8",
                
                // Phase 2
                "CurrencyRate",
                "NoClaimBonus",
                "SurrenderValue",
                "DatabaseCommision",
                "GrossPremiumAlt",
                "NetPremiumAlt",
                "Layer1FlatExtraPremium",
                "TransactionPremium",
                "OriginalPremium",
                "TransactionDiscount",
                "OriginalDiscount",
                "BrokerageFee",
                "MaxUwRating",
                "RetentionCap",
                "AarCap",
                "RiRate",
                "RiRate2",
                "AnnuityFactor",
                "SumAssuredOffered",
                "UwRatingOffered",
                "FlatExtraAmountOffered",
                "FlatExtraDuration",
                "EffectiveDate",
                "OfferLetterSentDate",
                "RiskPeriodStartDate",
                "RiskPeriodEndDate",
                "Mfrs17AnnualCohort",
                "MaxExpiryAge",
                "MinIssueAge",
                "MaxIssueAge",
                "MinAar",
                "MaxAar",
                "CorridorLimit",
                "Abl",
                "RatePerBasisUnit",
                "RiDiscountRate",
                "LargeSaDiscount",
                "GroupSizeDiscount",
                "EwarpNumber",
                "EwarpActionCode",
                "RetentionShare",
                "AarShare",
                "ProfitComm",
                "TotalDirectRetroAar",
                "TotalDirectRetroGrossPremium",
                "TotalDirectRetroDiscount",
                "TotalDirectRetroNetPremium",
                "TreatyType",
                "MaxApLoading",
                "MlreInsuredAttainedAgeAtCurrentMonth",
                "MlreInsuredAttainedAgeAtPreviousMonth",
                "InsuredAttainedAgeCheck",
                "MaxExpiryAgeCheck",
                "MlrePolicyIssueAge",
                "PolicyIssueAgeCheck",
                "MinIssueAgeCheck",
                "MaxIssueAgeCheck",
                "MaxUwRatingCheck",
                "ApLoadingCheck",
                "EffectiveDateCheck",
                "MinAarCheck",
                "MaxAarCheck",
                "CorridorLimitCheck",
                "AblCheck",
                "RetentionCheck",
                "AarCheck",
                "MlreStandardPremium",
                "MlreSubstandardPremium",
                "MlreFlatExtraPremium",
                "MlreGrossPremium",
                "MlreStandardDiscount",
                "MlreSubstandardDiscount",
                "MlreLargeSaDiscount",
                "MlreGroupSizeDiscount",
                "MlreVitalityDiscount",
                "MlreTotalDiscount",
                "MlreNetPremium",
                "NetPremiumCheck",
                "ServiceFeePercentage",
                "ServiceFee",
                "MlreBrokerageFee",
                "MlreDatabaseCommission",
                "ValidityDayCheck",
                "SumAssuredOfferedCheck",
                "UwRatingCheck",
                "FlatExtraAmountCheck",
                "FlatExtraDurationCheck",
                "AarShare2",
                "AarCap2",
                "WakalahFeePercentage",
                "TreatyNumber",
                "RecordType",
            };
        }

        public static List<string> GetCodes()
        {
            return new List<string>
            {
                "CUSTOM_FIELD",
                "TREATY_CODE",
                "REINS_BASIS_CODE",
                "FUNDS_ACCOUNTING_TYPE_CODE",
                "PREMIUM_FREQUENCY_CODE",
                "REPORT_PERIOD_MONTH",
                "REPORT_PERIOD_YEAR",
                "RISK_PERIOD_MONTH",
                "RISK_PERIOD_YEAR",
                "TRANSACTION_TYPE_CODE",
                "POLICY_NUMBER",
                "ISSUE_DATE_POL",
                "ISSUE_DATE_BEN",
                "REINS_EFF_DATE_POL",
                "REINS_EFF_DATE_BEN",
                "CEDING_PLAN_CODE",
                "CEDING_BENEFIT_TYPE_CODE",
                "CEDING_BENEFIT_RISK_CODE",
                "MLRE_BENEFIT_CODE",
                "ORI_SUM_ASSURED",
                "CURR_SUM_ASSURED",
                "AMOUNT_CEDED_B4_MLRE_SHARE",
                "RETENTION_AMOUNT",
                "AAR_ORI",
                "AAR",
                "AAR_SPECIAL_1",
                "AAR_SPECIAL_2",
                "AAR_SPECIAL_3",
                "INSURED_NAME",
                "INSURED_GENDER_CODE",
                "INSURED_TOBACCO_USE",
                "INSURED_DATE_OF_BIRTH",
                "INSURED_OCCUPATION_CODE",
                "INSURED_REGISTER_NO",
                "INSURED_ATTAINED_AGE",
                "INSURED_NEW_IC_NUMBER",
                "INSURED_OLD_IC_NUMBER",
                "INSURED_NAME_2ND",
                "INSURED_GENDER_CODE_2ND",
                "INSURED_TOBACCO_USE_2ND",
                "INSURED_DATE_OF_BIRTH_2ND",
                "INSURED_ATTAINED_AGE_2ND",
                "INSURED_NEW_IC_NUMBER_2ND",
                "INSURED_OLD_IC_NUMBER_2ND",
                "REINSURANCE_ISSUE_AGE",
                "REINSURANCE_ISSUE_AGE_2ND",
                "POLICY_TERM",
                "POLICY_EXPIRY_DATE",
                "DURATION_YEAR",
                "DURATION_DAY",
                "DURATION_MONTH",
                "PREMIUM_CAL_TYPE",
                "CEDANT_RI_RATE",
                "RATE_TABLE",
                "AGE_RATED_UP",
                "DISCOUNT_RATE",
                "LOADING_TYPE",
                "UNDERWRITER_RATING",
                "UNDERWRITER_RATING_UNIT",
                "UNDERWRITER_RATING_TERM",
                "UNDERWRITER_RATING_2",
                "UNDERWRITER_RATING_UNIT_2",
                "UNDERWRITER_RATING_TERM_2",
                "UNDERWRITER_RATING_3",
                "UNDERWRITER_RATING_UNIT_3",
                "UNDERWRITER_RATING_TERM_3",
                "FLAT_EXTRA_AMOUNT",
                "FLAT_EXTRA_UNIT",
                "FLAT_EXTRA_TERM",
                "FLAT_EXTRA_AMOUNT_2",
                "FLAT_EXTRA_TERM_2",
                "STANDARD_PREMIUM",
                "SUBSTANDARD_PREMIUM",
                "FLAT_EXTRA_PREMIUM",
                "GROSS_PREMIUM",
                "STANDARD_DISCOUNT",
                "SUBSTANDARD_DISCOUNT",
                "VITALITY_DISCOUNT",
                "TOTAL_DISCOUNT",
                "NET_PREMIUM",
                "ANNUAL_RI_PREM",
                "RI_COV_PERIOD",
                "ADJ_BEGIN_DATE",
                "ADJ_END_DATE",
                "POLICY_NUMBER_OLD",
                "POLICY_STATUS_CODE",
                "POLICY_GROSS_PREMIUM",
                "POLICY_STANDARD_PREMIUM",
                "POLICY_SUBSTANDARD_PREMIUM",
                "POLICY_TERM_REMAIN",
                "POLICY_AMOUNT_DEATH",
                "POLICY_RESERVE",
                "POLICY_PAYMENT_METHOD",
                "POLICY_LIFE_NUMBER",
                "FUND_CODE",
                "LINE_OF_BUSINESS",
                "AP_LOADING",
                "LOAN_INTEREST_RATE",
                "DEFERMENT_PERIOD",
                "RIDER_NUMBER",
                "CAMPAIGN_CODE",
                "NATIONALITY",
                "TERRITORY_OF_ISSUE_CODE",
                "CURRENCY_CODE",
                "STAFF_PLAN_INDICATOR",
                "CEDING_TREATY_CODE",
                "CEDING_PLAN_CODE_OLD",
                "CEDING_BASIC_PLAN_CODE",
                "CEDANT_SAR",
                "CEDANT_REINSURER_CODE",
                "AMOUNT_CEDED_B4_MLRE_SHARE_T1",
                "CESSION_CODE",
                "CEDANT_REMARK",
                "GROUP_POLICY_NUMBER",
                "GROUP_POLICY_NAME",
                "NO_OF_EMPLOYEE",
                "POLICY_TOTAL_LIVE",
                "GROUP_SUBSIDIARY_NAME",
                "GROUP_SUBSIDIARY_NO",
                "GROUP_EMPLOYEE_BASIC_SALARY",
                "GROUP_EMPLOYEE_JOB_TYPE",
                "GROUP_EMPLOYEE_JOB_CODE",
                "GROUP_EMPLOYEE_BASIC_SALARY_REVISE",
                "GROUP_EMPLOYEE_BASIC_SALARY_MULTIPLIER",
                "CEDING_PLAN_CODE_2",
                "DEPENDANT_INDICATOR",
                "GHS_ROOM_BOARD",
                "POLICY_AMOUNT_SUBSTANDARD",
                "LAYER_1_RI_SHARE",
                "LAYER_1_INSURED_ATTAINED_AGE",
                "LAYER_1_INSURED_ATTAINED_AGE_2ND",
                "LAYER_1_STANDARD_PREMIUM",
                "LAYER_1_SUBSTANDARD_PREMIUM",
                "LAYER_1_GROSS_PREMIUM",
                "LAYER_1_STANDARD_DISCOUNT",
                "LAYER_1_SUBSTANDARD_DISCOUNT",
                "LAYER_1_TOTAL_DISCOUNT",
                "LAYER_1_NET_PREMIUM",
                "LAYER_1_GROSS_PREMIUM_ALT",
                "LAYER_1_TOTAL_DISCOUNT_ALT",
                "LAYER_1_NET_PREMIUM_ALT",
                "SPECIAL_INDICATOR_1",
                "SPECIAL_INDICATOR_2",
                "SPECIAL_INDICATOR_3",
                "INDICATOR_JOINT_LIFE",
                "TAX_AMOUNT",
                "GST_INDICATOR",
                "GST_GROSS_PREMIUM",
                "GST_TOTAL_DISCOUNT",
                "GST_VITALITY",
                "GST_AMOUNT",
                "MFRS17_BASIC_RIDER",
                "MFRS17_CELL_NAME",
                "MFRS17_CONTRACT_CODE", // Phase 2: MFRS17_CONTRACT_CODE
                "LOA_CODE",
                "TEMP_D_1",
                "TEMP_D_2",
                "TEMP_D_3",
                "TEMP_D_4",
                "TEMP_D_5",
                "TEMP_S_1",
                "TEMP_S_2",
                "TEMP_S_3",
                "TEMP_S_4",
                "TEMP_S_5",
                "TEMP_I_1",
                "TEMP_I_2",
                "TEMP_I_3",
                "TEMP_I_4",
                "TEMP_I_5",
                "TEMP_A_1",
                "TEMP_A_2",
                "TEMP_A_3",
                "TEMP_A_4",
                "TEMP_A_5",
                "TEMP_A_6",
                "TEMP_A_7",
                "TEMP_A_8",
                
                // Phase 2
                "CURRENCY_RATE",
                "NO_CLAIM_BONUS",
                "SURRENDER_VALUE",
                "DATABASE_COMMISSION",
                "GROSS_PREMIUM_ALT",
                "NET_PREMIUM_ALT",
                "LAYER_1_FLAT_EXTRA_PREMIUM",
                "TRANSACTION_PREMIUM",
                "ORIGINAL_PREMIUM",
                "TRANSACTION_DISCOUNT",
                "ORIGINAL_DISCOUNT",
                "BROKERAGE_FEE",
                "MAX_UW_RATING",
                "RETENTION_CAP",
                "AAR_CAP_1",
                "RI_RATE",
                "RI_RATE_2",
                "ANNUITY_FACTOR",
                "SUM_ASSURED_OFFERED",
                "UW_RATING_OFFERED",
                "FLAT_EXTRA_AMOUNT_OFFERED",
                "FLAT_EXTRA_DURATION",
                "EFFECTIVE_DATE",
                "OFFER_LETTER_SENT_DATE",
                "RISK_PERIOD_START_DATE",
                "RISK_PERIOD_END_DATE",
                "MFRS17_ANNUAL_COHORT",
                "MAX_EXPIRY_AGE",
                "MIN_ISSUE_AGE",
                "MAX_ISSUE_AGE",
                "MIN_AAR",
                "MAX_AAR",
                "CORRIDOR_LIMIT",
                "ABL",
                "RATE_PER_BASIS_UNIT",
                "RI_DISCOUNT_RATE",
                "LARGE_SA_DISCOUNT",
                "GROUP_SIZE_DISCOUNT",
                "EWARP_NUMBER",
                "EWARP_ACTION_CODE",
                "RETENTION_SHARE",
                "AAR_SHARE_1",
                "PROFIT_COMM",
                "TOTAL_DIRECT_RETRO_AAR",
                "TOTAL_DIRECT_RETRO_GROSS_PREMIUM",
                "TOTAL_DIRECT_RETRO_DISCOUNT",
                "TOTAL_DIRECT_RETRO_NET_PREMIUM",
                "TREATY_TYPE",
                "MAX_AP_LOADING",
                "MLRE_INSURED_ATTAINED_AGE_AT_CURRENT_MONTH",
                "MLRE_INSURED_ATTAINED_AGE_AT_PREVIOUS_MONTH",
                "INSURED_ATTAINED_AGE_CHECK",
                "MAX_EXPIRY_AGE_CHECK",
                "MLRE_POLICY_ISSUE_AGE",
                "POLICY_ISSUE_AGE_CHECK",
                "MIN_ISSUE_AGE_CHECK",
                "MAX_ISSUE_AGE_CHECK",
                "MAX_UW_RATING_CHECK",
                "AP_LOADING_CHECK",
                "EFFECTIVE_DATE_CHECK",
                "MIN_AAR_CHECK",
                "MAX_AAR_CHECK",
                "CORRIDOR_LIMIT_CHECK",
                "ABL_CHECK",
                "RETENTION_CHECK",
                "AAR_CHECK",
                "MLRE_STANDARD_PREMIUM",
                "MLRE_SUBSTANDARD_PREMIUM",
                "MLRE_FLAT_EXTRA_PREMIUM",
                "MLRE_GROSS_PREMIUM",
                "MLRE_STANDARD_DISCOUNT",
                "MLRE_SUBSTANDARD_DISCOUNT",
                "MLRE_LARGE_SA_DISCOUNT",
                "MLRE_GROUP_SIZE_DISCOUNT",
                "MLRE_VITALITY_DISCOUNT",
                "MLRE_TOTAL_DISCOUNT",
                "MLRE_NET_PREMIUM",
                "NET_PREMIUM_CHECK",
                "SERVICE_FEE_%",
                "SERVICE_FEE",
                "MLRE_BROKERAGE_FEE",
                "MLRE_DATABASE_COMMISSION",
                "VALIDITY_DAY_CHECK",
                "SUM_ASSURED_OFFERED_CHECK",
                "UW_RATING_CHECK",
                "FLAT_EXTRA_AMOUNT_CHECK",
                "FLAT_EXTRA_DURATION_CHECK",
                "AAR_SHARE_2",
                "AAR_CAP_2",
                "WAKALAH_FEE_%",
                "TREATY_NUMBER",
                "RECORD_TYPE",
            };
        }

        public static StandardOutputBo GetByType(int type)
        {
            if (type < 1)
                return null;
            if (type > TypeMax)
                return null;

            return new StandardOutputBo
            {
                Id = type,
                Type = type,
                TypeName = GetTypeName(type),
                DataType = GetDataTypeByType(type),
                DataTypeName = GetDataTypeName(type),
                Code = GetCodeByType(type),
                Name = GetTypeName(type),
                Property = GetPropertyNameByType(type),
            };
        }

        public static string GetTypeName(int key)
        {
            List<string> names = GetNames();
            if (key > 0 && key <= names.Count)
                return names[key - 1];
            return "";
        }

        public static string GetCodeByType(int key)
        {
            List<string> codes = GetCodes();
            if (key > 0 && key <= codes.Count)
                return codes[key - 1];
            return "";
        }

        public static string GetPropertyNameByType(int key)
        {
            List<string> properties = GetPropertyNames();
            if (key > 0 && key <= properties.Count)
                return properties[key - 1];
            return "";
        }

        public static int GetMaxLengthPropertyName()
        {
            List<string> properties = GetPropertyNames();
            return properties.Max(q => q.Length);
        }

        public static int GetTypeByCode(string code)
        {
            var codes = GetCodes();
            return codes.FindIndex(q => q == code) + 1;
        }

        public static int? GetTypeByCode(string code, bool integer = false)
        {
            int type = GetTypeByCode(code);
            if (type <= 0 && !integer)
            {
                return null;
            }
            return type;
        }

        public static int GetTypeByPropertyName(string name)
        {
            List<string> properties = GetPropertyNames();
            return properties.FindIndex(q => q == name) + 1;
        }

        public static int GetDataTypeByType(int key)
        {
            if (GetDateTypes().Contains(key))
                return DataTypeDate;

            if (GetStringTypes().Contains(key))
                return DataTypeString;

            if (GetAmountTypes().Contains(key))
                return DataTypeAmount;

            if (GetPercentageTypes().Contains(key))
                return DataTypePercentage;

            if (GetIntegerTypes().Contains(key))
                return DataTypeInteger;

            if (GetDropDownTypes().Contains(key))
                return DataTypeDropDown;

            if (GetBooleanTypes().Contains(key))
                return DataTypeBoolean;

            return DataTypeString;
        }

        public static List<int> GetDateTypes()
        {
            return new List<int>
            {
                TypeIssueDatePol,
                TypeIssueDateBen,
                TypeReinsEffDatePol,
                TypeReinsEffDateBen,
                TypeInsuredDateOfBirth,
                TypeInsuredDateOfBirth2nd,
                TypePolicyExpiryDate,
                TypeAdjBeginDate,
                TypeAdjEndDate,
                TypeTempD1,
                TypeTempD2,
                TypeTempD3,
                TypeTempD4,
                TypeTempD5,
                TypeEffectiveDate,
                TypeOfferLetterSentDate,
                TypeRiskPeriodStartDate,
                TypeRiskPeriodEndDate,
            };
        }

        public static List<int> GetStringTypes()
        {
            return new List<int>
            {
                TypeTreatyCode,
                TypePolicyNumber,
                TypeCedingPlanCode,
                TypeCedingBenefitRiskCode,
                TypeMlreBenefitCode,
                TypeInsuredName,
                TypeInsuredRegisterNo,
                TypeInsuredNewIcNumber,
                TypeInsuredOldIcNumber,
                TypeInsuredName2nd,
                TypeInsuredNewIcNumber2nd,
                TypeInsuredOldIcNumber2nd,
                TypePremiumCalType,
                TypeRateTable,
                TypeLoadingType,
                TypePolicyNumberOld,
                TypeCampaignCode,
                TypeNationality,
                TypeCedingTreatyCode,
                TypeCedingPlanCodeOld,
                TypeCedingBasicPlanCode,
                TypeCedantReinsurerCode,
                TypeCessionCode,
                TypeCedantRemark,
                TypeGroupPolicyNumber,
                TypeGroupPolicyName,
                TypeGroupSubsidiaryName,
                TypeGroupSubsidiaryNo,
                TypeGroupEmployeeJobType,
                TypeGroupEmployeeJobCode,
                TypeCedingPlanCode2,
                TypeMfrs17CellName,
                TypeMfrs17TreatyCode,
                TypeLoaCode,
                TypeTempS1,
                TypeTempS2,
                TypeTempS3,
                TypeTempS4,
                TypeTempS5,
                TypeEwarpActionCode,
                TypeTreatyNumber,
            };
        }

        public static List<int> GetAmountTypes()
        {
            return new List<int>
            {
                TypeOriSumAssured,
                TypeCurrSumAssured,
                TypeAmountCededB4MlreShare,
                TypeRetentionAmount,
                TypeAarOri,
                TypeAar,
                TypeAarSpecial1,
                TypeAarSpecial2,
                TypeAarSpecial3,
                TypePolicyTerm,
                TypeDurationYear,
                TypeCedantRiRate,
                TypeDiscountRate,
                TypeUnderwriterRating,
                TypeUnderwriterRatingUnit,
                TypeUnderwriterRating2,
                TypeUnderwriterRatingUnit2,
                TypeUnderwriterRating3,
                TypeUnderwriterRatingUnit3,
                TypeFlatExtraAmount,
                TypeFlatExtraUnit,
                TypeFlatExtraAmount2,
                TypeStandardPremium,
                TypeSubstandardPremium,
                TypeFlatExtraPremium,
                TypeGrossPremium,
                TypeStandardDiscount,
                TypeSubstandardDiscount,
                TypeVitalityDiscount,
                TypeTotalDiscount,
                TypeNetPremium,
                TypeAnnualRiPrem,
                TypePolicyGrossPremium,
                TypePolicyStandardPremium,
                TypePolicySubstandardPremium,
                TypePolicyTermRemain,
                TypePolicyAmountDeath,
                TypePolicyReserve,
                TypeApLoading,
                TypeLoanInterestRate,
                TypeCedantSar,
                TypeAmountCededB4MlreShare2,
                TypeGroupEmployeeBasicSalary,
                TypeGroupEmployeeBasicSalaryRevise,
                TypeGroupEmployeeBasicSalaryMultiplier,
                TypePolicyAmountSubstandard,
                TypeLayer1RiShare,
                TypeLayer1StandardPremium,
                TypeLayer1SubstandardPremium,
                TypeLayer1GrossPremium,
                TypeLayer1StandardDiscount,
                TypeLayer1SubstandardDiscount,
                TypeLayer1TotalDiscount,
                TypeLayer1NetPremium,
                TypeLayer1GrossPremiumAlt,
                TypeLayer1TotalDiscountAlt,
                TypeLayer1NetPremiumAlt,
                TypeTaxAmount,
                TypeGstGrossPremium,
                TypeGstTotalDiscount,
                TypeGstVitality,
                TypeGstAmount,
                TypeDurationDay,
                TypeDurationMonth,
                TypeRiCovPeriod,
                TypeTempA1,
                TypeTempA2,
                TypeTempA3,
                TypeTempA4,
                TypeTempA5,
                TypeTempA6,
                TypeTempA7,
                TypeTempA8,
                TypeCurrencyRate,
                TypeNoClaimBonus,
                TypeSurrenderValue,
                TypeDatabaseCommision,
                TypeGrossPremiumAlt,
                TypeNetPremiumAlt,
                TypeLayer1FlatExtraPremium,
                TypeTransactionPremium,
                TypeOriginalPremium,
                TypeTransactionDiscount,
                TypeOriginalDiscount,
                TypeBrokerageFee,
                TypeMaxUwRating,
                TypeRetentionCap,
                TypeAarCap,
                TypeRiRate,
                TypeRiRate2,
                TypeAnnuityFactor,
                TypeSumAssuredOffered,
                TypeUwRatingOffered,
                TypeFlatExtraAmountOffered,
                TypeTotalDirectRetroAar,
                TypeTotalDirectRetroGrossPremium,
                TypeTotalDirectRetroDiscount,
                TypeTotalDirectRetroNetPremium,
                TypeMinAar,
                TypeMaxAar,
                TypeCorridorLimit,
                TypeRiDiscountRate,
                TypeLargeSaDiscount,
                TypeGroupSizeDiscount,
                TypeAbl,
                TypeMaxApLoading,
                TypeMlreStandardPremium,
                TypeMlreSubstandardPremium,
                TypeMlreFlatExtraPremium,
                TypeMlreGrossPremium,
                TypeMlreStandardDiscount,
                TypeMlreSubstandardDiscount,
                TypeMlreLargeSaDiscount,
                TypeMlreGroupSizeDiscount,
                TypeMlreVitalityDiscount,
                TypeMlreTotalDiscount,
                TypeMlreNetPremium,
                TypeServiceFee,
                TypeMlreBrokerageFee,
                TypeMlreDatabaseCommission,
                TypeNetPremiumCheck,
                TypeAarCap2,
            };
        }

        public static List<int> GetPercentageTypes()
        {
            return new List<int>
            {
                TypeRetentionShare,
                TypeAarShare,
                TypeServiceFeePercentage,
                TypeAarShare2,
                TypeWakalahFeePercentage,
            };
        }

        public static List<int> GetIntegerTypes()
        {
            return new List<int>
            {
                TypeReportPeriodMonth,
                TypeReportPeriodYear,
                TypeRiskPeriodMonth,
                TypeRiskPeriodYear,
                TypeInsuredAttainedAge,
                TypeInsuredAttainedAge2nd,
                TypeReinsuranceIssueAge,
                TypeReinsuranceIssueAge2nd,
                TypeAgeRatedUp,
                TypeUnderwriterRatingTerm,
                TypeUnderwriterRatingTerm2,
                TypeUnderwriterRatingTerm3,
                TypeFlatExtraTerm,
                TypeFlatExtraTerm2,
                TypePolicyLifeNumber,
                TypeDefermentPeriod,
                TypeRiderNumber,
                TypeNoOfEmployee,
                TypePolicyTotalLive,
                TypeGhsRoomBoard,
                TypeLayer1InsuredAttainedAge,
                TypeLayer1InsuredAttainedAge2nd,
                TypeTempI1,
                TypeTempI2,
                TypeTempI3,
                TypeTempI4,
                TypeTempI5,
                TypeMfrs17AnnualCohort,
                TypeMaxExpiryAge,
                TypeMinIssueAge,
                TypeMaxIssueAge,
                TypeRatePerBasisUnit,
                TypeEwarpNumber,
                TypeMlreInsuredAttainedAgeAtCurrentMonth,
                TypeMlreInsuredAttainedAgeAtPreviousMonth,
                TypeMlrePolicyIssueAge,
                TypeFlatExtraDuration,
                TypeRecordType,
            };
        }

        public static List<int> GetDropDownTypes()
        {
            return new List<int>
            {
                TypeFundsAccountingTypeCode,
                TypeReinsBasisCode,
                TypePremiumFrequencyCode,
                TypeTransactionTypeCode,
                TypeCedingBenefitTypeCode,
                TypeInsuredGenderCode,
                TypeInsuredTobaccoUse,
                TypeInsuredGenderCode2nd,
                TypeInsuredTobaccoUse2nd,
                TypeInsuredOccupationCode,
                TypePolicyStatusCode,
                TypePolicyPaymentMethod,
                TypeDependantIndicator,
                TypeSpecialIndicator1,
                TypeSpecialIndicator2,
                TypeSpecialIndicator3,
                TypeMfrs17BasicRider,
                TypeFundCode,
                TypeLineOfBusiness,
                TypeGstIndicator,
                TypeStaffPlanIndicator,
                TypeIndicatorJointLife,
                TypeTerritoryOfIssueCode,
                TypeCurrencyCode,
                TypeProfitComm,
                TypeTreatyType,
            };
        }

        public static List<int> GetBooleanTypes()
        {
            return new List<int>
            {
                TypeInsuredAttainedAgeCheck,
                TypeMaxExpiryAgeCheck,
                TypePolicyIssueAgeCheck,
                TypeMinIssueAgeCheck,
                TypeMaxIssueAgeCheck,
                TypeMaxUwRatingCheck,
                TypeApLoadingCheck,
                TypeEffectiveDateCheck,
                TypeMinAarCheck,
                TypeMaxAarCheck,
                TypeCorridorLimitCheck,
                TypeAblCheck,
                TypeRetentionCheck,
                TypeAarCheck,
                TypeValidityDayCheck,
                TypeSumAssuredOfferedCheck,
                TypeUwRatingCheck,
                TypeFlatExtraAmountCheck,
                TypeFlatExtraDurationCheck,
            };
        }

        public static List<int> GetWarehouseExcludedTypes()
        {
            return new List<int>
            {
                TypeCustomField,
                TypeTempA1,
                TypeTempA2,
                TypeTempA3,
                TypeTempA4,
                TypeTempA5,
                TypeTempA6,
                TypeTempA7,
                TypeTempA8,
                TypeTempD1,
                TypeTempD2,
                TypeTempD3,
                TypeTempD4,
                TypeTempD5,
                TypeTempI1,
                TypeTempI2,
                TypeTempI3,
                TypeTempI4,
                TypeTempI5,
                TypeTempS1,
                TypeTempS2,
                TypeTempS3,
                TypeTempS4,
                TypeTempS5,
                TypeRecordType, // Default already have this field
            };
        }

        public static string GetDataTypeName(int key)
        {
            switch (key)
            {
                case DataTypeDate:
                    return "Date";
                case DataTypeString:
                    return "String";
                case DataTypeAmount:
                    return "Amount";
                case DataTypePercentage:
                    return "Percentage";
                case DataTypeInteger:
                    return "Integer";
                case DataTypeDropDown:
                    return "Drop Down";
                case DataTypeBoolean:
                    return "Boolean";
                default:
                    return "";
            }
        }

        public static object GetRandomValueByType(int key)
        {
            int dataType = GetDataTypeByType(key);
            if (dataType == DataTypeInteger)
            {
                string code = GetCodeByType(key);
                if (code.Contains("YEAR"))
                {
                    return GetRandomValueByDataType(dataType, 2020, 2020);
                }
                else if (code.Contains("MONTH"))
                {
                    List<object> months = new List<object>() { 3, 5, 7, 8, 10 };
                    return GetRandomValueByDataType(dataType, set: months);
                }
            }
            return GetRandomValueByDataType(dataType);
        }

        public static object GetRandomValueByDataType(int key, int minValue = 1, int maxValue = 100, List<object> set = null)
        {
            if (!set.IsNullOrEmpty())
            {
                int length = set.Count();
                return set[new Random().Next(length)];
            }

            switch (key)
            {
                case DataTypeDate:
                    return DateTime.Now;
                case DataTypeString:
                case DataTypeDropDown:
                    int randomStringLength = Util.GetConfigInteger("RandomStringLength", 20);
                    return Util.GenerateRandomString(randomStringLength, randomStringLength, true);
                case DataTypeAmount:
                case DataTypePercentage:
                    return new Random().Next(100);
                case DataTypeInteger:
                    return new Random().Next(minValue, maxValue);
                case DataTypeBoolean:
                    return false;
                default:
                    return null;
            }
        }

        public bool IsDataTypeString(string title, out string error)
        {
            error = null;
            switch (DataType)
            {
                case DataTypeString:
                case DataTypeDropDown:
                    return true;
                default:
                    error = string.Format("{0}{1}: {2}", title, GetTypeName(Type), MessageBag.UnableSetToStringValue);
                    return false;
            }
        }

        public bool IsDataTypeInt(string title, out string error)
        {
            error = null;
            switch (DataType)
            {
                case DataTypeInteger:
                    return true;
                default:
                    error = string.Format("{0}{1}: {2}", title, GetTypeName(Type), MessageBag.UnableSetToIntValue);
                    return false;
            }
        }

        public bool IsDataTypeDouble(string title, out string error)
        {
            error = null;
            switch (DataType)
            {
                case DataTypeAmount:
                case DataTypePercentage:
                    return true;
                default:
                    error = string.Format("{0}{1}: {2}", title, GetTypeName(Type), MessageBag.UnableSetToDoubleValue);
                    return false;
            }
        }

        public bool IsDataTypeDate(string title, out string error)
        {
            error = null;
            switch (DataType)
            {
                case DataTypeDate:
                    return true;
                default:
                    error = string.Format("{0}{1}: {2}", title, GetTypeName(Type), MessageBag.UnableSetToDateTimeValue);
                    return false;
            }
        }

        public bool IsDataTypeBoolean(string title, out string error)
        {
            error = null;
            switch (DataType)
            {
                case DataTypeBoolean:
                    return true;
                default:
                    error = string.Format("{0}{1}: {2}", title, GetTypeName(Type), MessageBag.UnableSetToBooleanValue);
                    return false;
            }
        }
    }
}
