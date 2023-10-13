using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class StandardRetroOutputBo
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

        public bool DisableDummyValue { get; set; } = false;


        public const int TypeTreatyCode = 1;
        public const int TypeReinsBasisCode = 2;
        public const int TypeFundsAccountingTypeCode = 3;
        public const int TypePremiumFrequencyCode = 4;
        public const int TypeReportPeriodMonth = 5;
        public const int TypeReportPeriodYear = 6;
        public const int TypeRiskPeriodMonth = 7;
        public const int TypeRiskPeriodYear = 8;
        public const int TypeTransactionTypeCode = 9;
        public const int TypePolicyNumber = 10;
        public const int TypeIssueDatePol = 11;
        public const int TypeIssueDateBen = 12;
        public const int TypeReinsEffDatePol = 13;
        public const int TypeReinsEffDateBen = 14;
        public const int TypeCedingPlanCode = 15;
        public const int TypeCedingBenefitTypeCode = 16;
        public const int TypeCedingBenefitRiskCode = 17;
        public const int TypeMlreBenefitCode = 18;
        public const int TypeOriSumAssured = 19;
        public const int TypeCurrSumAssured = 20;
        public const int TypeAmountCededB4MlreShare = 21;
        public const int TypeAarOri = 22;
        public const int TypeAar = 23;
        public const int TypeInsuredName = 24;
        public const int TypeInsuredGenderCode = 25;
        public const int TypeInsuredTobaccoUse = 26;
        public const int TypeInsuredDateOfBirth = 27;
        public const int TypeInsuredOccupationCode = 28;
        public const int TypeInsuredRegisterNo = 29;
        public const int TypeInsuredAttainedAge = 30;
        public const int TypeInsuredNewIcNumber = 31;
        public const int TypeInsuredOldIcNumber = 32;
        public const int TypeReinsuranceIssueAge = 33;
        public const int TypePolicyTerm = 34;
        public const int TypePolicyExpiryDate = 35;
        public const int TypeLoadingType = 36;
        public const int TypeUnderwriterRating = 37;
        public const int TypeFlatExtraAmount = 38;
        public const int TypeStandardPremium = 39;
        public const int TypeSubstandardPremium = 40;
        public const int TypeFlatExtraPremium = 41;
        public const int TypeGrossPremium = 42;
        public const int TypeStandardDiscount = 43;
        public const int TypeSubstandardDiscount = 44;
        public const int TypeNetPremium = 45;
        public const int TypePolicyNumberOld = 46;
        public const int TypePolicyLifeNumber = 47;
        public const int TypeFundCode = 48;
        public const int TypeRiderNumber = 49;
        public const int TypeCampaignCode = 50;
        public const int TypeNationality = 51;
        public const int TypeTerritoryOfIssueCode = 52;
        public const int TypeCurrencyCode = 53;
        public const int TypeStaffPlanIndicator = 54;
        public const int TypeCedingPlanCodeOld = 55;
        public const int TypeCedingBasicPlanCode = 56;
        public const int TypeGroupPolicyNumber = 57;
        public const int TypeGroupPolicyName = 58;
        public const int TypeGroupSubsidiaryName = 59;
        public const int TypeGroupSubsidiaryNo = 60;
        public const int TypeCedingPlanCode2 = 61;
        public const int TypeDependantIndicator = 62;
        public const int TypeMfrs17BasicRider = 63;
        public const int TypeMfrs17CellName = 64;
        public const int TypeMfrs17ContractCode = 65;
        public const int TypeLoaCode = 66;
        public const int TypeRiskPeriodStartDate = 67;
        public const int TypeRiskPeriodEndDate = 68;
        public const int TypeMfrs17AnnualCohort = 69;
        public const int TypeBrokerageFee = 70;
        public const int TypeApLoading = 71;
        public const int TypeEffectiveDate = 72;
        public const int TypeAnnuityFactor = 73;
        public const int TypeEndingPolicyStatus = 74;
        public const int TypeLastUpdatedDate = 75;
        public const int TypeTreatyType = 76;
        public const int TypeTreatyNumber = 77;
        public const int TypeRetroPremFreq = 78;
        public const int TypeLifeBenefitFlag = 79;
        public const int TypeRiskQuarter = 80;
        public const int TypeProcessingDate = 81;
        public const int TypeUniqueKeyPerLife = 82;
        public const int TypeRetroBenefitCode = 83;
        public const int TypeRetroRatio = 84;
        public const int TypeAccumulativeRetainAmount = 85;
        public const int TypeRetroRetainAmount = 86;
        public const int TypeRetroAmount = 87;
        public const int TypeRetroGrossPremium = 88;
        public const int TypeRetroNetPremium = 89;
        public const int TypeRetroDiscount = 90;
        public const int TypeRetroExtraPremium = 91;
        public const int TypeRetroExtraComm = 92;
        public const int TypeRetroGst = 93;
        public const int TypeRetroTreaty = 94;
        public const int TypeRetroClaimId = 95;
        public const int TypeSoa = 96;
        public const int TypeRetroIndicator = 97;

        public const int TypeMax = 97;

        public static List<string> GetNames()
        {
            return new List<string>
            {
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
                "MLRe Benefit Code",
                "Ori Sum Assured",
                "Curr Sum Assured",
                "Amount Ceded B4 Mlre Share",
                "AAR Ori",
                "AAR",
                "Insured Name",
                "Insured Gender Code",
                "Insured Tobacco Use",
                "Insured Date Of Birth",
                "Insured Occupation Code",
                "Insured Register No",
                "Insured Attained Age",
                "Insured New Ic Number",
                "Insured Old Ic Number",
                "Reinsurance Issue Age",
                "Policy Term",
                "Policy Expiry Date",
                "Loading Type",
                "Underwriter Rating",
                "Flat Extra Amount",
                "Standard Premium",
                "Substandard Premium",
                "Flat Extra Premium",
                "Gross Premium",
                "Standard Discount",
                "Substandard Discount",
                "Net Premium",
                "Policy Number Old",
                "Policy Life Number",
                "Fund Code",
                "Rider Number",
                "Campaign Code",
                "Nationality",
                "Territory Of Issue Code",
                "Currency Code",
                "Staff Plan Indicator",
                "Ceding Plan Code Old",
                "Ceding Basic Plan Code",
                "Group Policy Number",
                "Group Policy Name",
                "Group Subsidiary Name",
                "Group Subsidiary No",
                "Ceding Plan Code 2",
                "Dependant Indicator",
                "MFRS17 Basic Rider",
                "MFRS17 Cell Name",
                "MFRS17 Contract Code",
                "Loa Code",
                "Risk Period Start Date",
                "Risk Period End Date",
                "MFRS17 Annual Cohort",
                "Brokerage Fee",
                "Ap Loading",
                "Effective Date",
                "Annuity Factor",
                "Ending Policy Status",
                "Last Updated Date",
                "Treaty Type",
                "Treaty Number",
                "Retro Prem Freq",
                "Life Benefit Flag",
                "Risk Quarter",
                "Processing Date",
                "Unique Key Per Life",
                "Retro Benefit Code",
                "Retro Ratio",
                "Accumulative Retain Amount",
                "Retro Retain Amount",
                "Retro Amount",
                "Retro Gross Premium",
                "Retro Net Premium",
                "Retro Discount",
                "Retro Extra Premium",
                "Retro Extra Comm",
                "Retro GST",
                "Retro Treaty",
                "Retro Claim Id",
                "Retro Soa",
                "Retro Indicator",
            };
        }

        public static List<string> GetPropertyNames()
        {
            return new List<string> {
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
                "AarOri",
                "Aar",
                "InsuredName",
                "InsuredGenderCode",
                "InsuredTobaccoUse",
                "InsuredDateOfBirth",
                "InsuredOccupationCode",
                "InsuredRegisterNo",
                "InsuredAttainedAge",
                "InsuredNewIcNumber",
                "InsuredOldIcNumber",
                "ReinsuranceIssueAge",
                "PolicyTerm",
                "PolicyExpiryDate",
                "LoadingType",
                "UnderwriterRating",
                "FlatExtraAmount",
                "StandardPremium",
                "SubstandardPremium",
                "FlatExtraPremium",
                "GrossPremium",
                "StandardDiscount",
                "SubstandardDiscount",
                "NetPremium",
                "PolicyNumberOld",
                "PolicyLifeNumber",
                "FundCode",
                "RiderNumber",
                "CampaignCode",
                "Nationality",
                "TerritoryOfIssueCode",
                "CurrencyCode",
                "StaffPlanIndicator",
                "CedingPlanCodeOld",
                "CedingBasicPlanCode",
                "GroupPolicyNumber",
                "GroupPolicyName",
                "GroupSubsidiaryName",
                "GroupSubsidiaryNo",
                "CedingPlanCode2",
                "DependantIndicator",
                "Mfrs17BasicRider",
                "Mfrs17CellName",
                "Mfrs17ContractCode",
                "LoaCode",
                "RiskPeriodStartDate",
                "RiskPeriodEndDate",
                "Mfrs17AnnualCohort",
                "BrokerageFee",
                "ApLoading",
                "EffectiveDate",
                "AnnuityFactor",
                "EndingPolicyStatus",
                "LastUpdatedDate",
                "TreatyType",
                "TreatyNumber",
                "RetroPremFreq",
                "LifeBenefitFlag",
                "RiskQuarter",
                "ProcessingDate",
                "UniqueKeyPerLife",
                "RetroBenefitCode",
                "RetroRatio",
                "AccumulativeRetainAmount",
                "RetroRetainAmount",
                "DistributedRetroAmount",
                "RetroGrossPremium",
                "RetroNetPremium",
                "RetroDiscount",
                "RetroExtraPremium",
                "RetroExtraComm",
                "RetroGst",
                "RetroTreaty",
                "RetroClaimId",
                "RetroSoa",
                "RetroIndicator",
            };
        }

        public static List<string> GetCodes()
        {
            return new List<string>
            {
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
                "AAR_ORI",
                "AAR",
                "INSURED_NAME",
                "INSURED_GENDER_CODE",
                "INSURED_TOBACCO_USE",
                "INSURED_DATE_OF_BIRTH",
                "INSURED_OCCUPATION_CODE",
                "INSURED_REGISTER_NO",
                "INSURED_ATTAINED_AGE",
                "INSURED_NEW_IC_NUMBER",
                "INSURED_OLD_IC_NUMBER",
                "REINSURANCE_ISSUE_AGE",
                "POLICY_TERM",
                "POLICY_EXPIRY_DATE",
                "LOADING_TYPE",
                "UNDERWRITER_RATING",
                "FLAT_EXTRA_AMOUNT",
                "STANDARD_PREMIUM",
                "SUBSTANDARD_PREMIUM",
                "FLAT_EXTRA_PREMIUM",
                "GROSS_PREMIUM",
                "STANDARD_DISCOUNT",
                "SUBSTANDARD_DISCOUNT",
                "NET_PREMIUM",
                "POLICY_NUMBER_OLD",
                "POLICY_LIFE_NUMBER",
                "FUND_CODE",
                "RIDER_NUMBER",
                "CAMPAIGN_CODE",
                "NATIONALITY",
                "TERRITORY_OF_ISSUE_CODE",
                "CURRENCY_CODE",
                "STAFF_PLAN_INDICATOR",
                "CEDING_PLAN_CODE_OLD",
                "CEDING_BASIC_PLAN_CODE",
                "GROUP_POLICY_NUMBER",
                "GROUP_POLICY_NAME",
                "GROUP_SUBSIDIARY_NAME",
                "GROUP_SUBSIDIARY_NO",
                "CEDING_PLAN_CODE_2",
                "DEPENDANT_INDICATOR",
                "MFRS17_BASIC_RIDER",
                "MFRS17_CELL_NAME",
                "MFRS17_CONTRACT_CODE",
                "LOA_CODE",
                "RISK_PERIOD_START_DATE",
                "RISK_PERIOD_END_DATE",
                "MFRS_17_ANNUAL_COHORT",
                "BROKERAGE_FEE",
                "AP_LOADING",
                "EFFECTIVE_DATE",
                "ANNUITY_FACTOR",
                "ENDING_POLICY_STATUS",
                "LAST_UPDATED_DATE",
                "Treaty_Type",
                "TREATY_NUMBER",
                "RETRO_PREM_FREQ",
                "LIFE_BENEFIT_FLAG",
                "RISK_QUARTER",
                "PROCESSING_DATE",
                "UNIQUEKEY_PERLIFE",
                "RETRO_BENEFIT_CODE",
                "RETRO RATIO",
                "ACCUMULATIVE_RETAIN_AMOUNT",
                "RETRO_RETAIN_AMOUNT",
                "RETRO_AMOUNT",
                "RETRO_GROSS_PREMIUM",
                "RETRO_NET_PREMIUM",
                "RETRO_DISCOUNT",
                "RETRO_EXTRA_PREMIUM",
                "RETRO_EXTRA_COMM",
                "RETRO_GST",
                "RETRO_TREATY",
                "RETRO_CLAIM_id",
                "SOA",
                "RETRO_INDICATOR",
            };
        }

        public static StandardRetroOutputBo GetByType(int type)
        {
            if (type < 1)
                return null;
            if (type > TypeMax)
                return null;

            return new StandardRetroOutputBo
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

        public static int GetDataTypeByType(int key)
        {
            if (GetDateTypes().Contains(key))
                return StandardOutputBo.DataTypeDate;

            if (GetStringTypes().Contains(key))
                return StandardOutputBo.DataTypeString;

            if (GetAmountTypes().Contains(key))
                return StandardOutputBo.DataTypeAmount;

            if (GetPercentageTypes().Contains(key))
                return StandardOutputBo.DataTypePercentage;

            if (GetIntegerTypes().Contains(key))
                return StandardOutputBo.DataTypeInteger;

            if (GetDropDownTypes().Contains(key))
                return StandardOutputBo.DataTypeDropDown;

            return StandardOutputBo.DataTypeString;
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

        public static string GetDataTypeName(int key)
        {
            switch (key)
            {
                case StandardOutputBo.DataTypeDate:
                    return "Date";
                case StandardOutputBo.DataTypeString:
                    return "String";
                case StandardOutputBo.DataTypeAmount:
                    return "Amount";
                case StandardOutputBo.DataTypePercentage:
                    return "Percentage";
                case StandardOutputBo.DataTypeInteger:
                    return "Integer";
                case StandardOutputBo.DataTypeDropDown:
                    return "Drop Down";
                default:
                    return "";
            }
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
                TypePolicyExpiryDate,
                TypeEffectiveDate,
                TypeRiskPeriodStartDate,
                TypeRiskPeriodEndDate,
                TypeProcessingDate
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
                TypeLoadingType,
                TypePolicyNumberOld,
                TypeCampaignCode,
                TypeNationality,
                TypeCedingPlanCodeOld,
                TypeCedingBasicPlanCode,
                TypeGroupPolicyNumber,
                TypeGroupPolicyName,
                TypeGroupSubsidiaryName,
                TypeGroupSubsidiaryNo,
                TypeCedingPlanCode2,
                TypeMfrs17CellName,
                TypeLoaCode,
                TypeTreatyNumber,
                TypeRiskQuarter,
                TypeUniqueKeyPerLife,
                TypeRetroTreaty,
                TypeRetroBenefitCode
            };
        }

        public static List<int> GetAmountTypes()
        {
            return new List<int>
            {
                TypeOriSumAssured,
                TypeCurrSumAssured,
                TypeAmountCededB4MlreShare,
                TypeAarOri,
                TypeAar,
                TypePolicyTerm,
                TypeUnderwriterRating,
                TypeFlatExtraAmount,
                TypeStandardPremium,
                TypeSubstandardPremium,
                TypeFlatExtraPremium,
                TypeGrossPremium,
                TypeStandardDiscount,
                TypeSubstandardDiscount,
                TypeNetPremium,
                TypeApLoading,
                TypeBrokerageFee,
                TypeAnnuityFactor,
                TypeRetroPremFreq,
                TypeAccumulativeRetainAmount,
                TypeRetroRetainAmount,
                TypeRetroAmount,
                TypeRetroGrossPremium,
                TypeRetroNetPremium,
                TypeRetroDiscount,
                TypeRetroExtraPremium,
                TypeRetroExtraComm,
                TypeRetroGst
            };
        }

        public static List<int> GetPercentageTypes()
        {
            return new List<int>
            {
                TypeRetroRatio
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
                TypeReinsuranceIssueAge,
                TypePolicyLifeNumber,
                TypeRiderNumber,
                TypeMfrs17AnnualCohort,
                TypeRetroClaimId,
                TypeSoa
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
                TypeInsuredOccupationCode,
                TypeDependantIndicator,
                TypeMfrs17BasicRider,
                TypeFundCode,
                TypeStaffPlanIndicator,
                TypeTerritoryOfIssueCode,
                TypeCurrencyCode,
                TypeTreatyType,
            };
        }

        public static List<int> GetBooleanTypes()
        {
            return new List<int>
            {
                TypeLifeBenefitFlag,
                TypeRetroIndicator
            };
        }

        public static object GetRandomValueByType(int key)
        {
            int dataType = GetDataTypeByType(key);
            if (dataType == StandardOutputBo.DataTypeInteger)
            {
                string code = GetCodeByType(key);
                if (code.Contains("YEAR"))
                {
                    return StandardOutputBo.GetRandomValueByDataType(dataType, 2020, 2020);
                }
                else if (code.Contains("MONTH"))
                {
                    List<object> months = new List<object>() { 3, 5, 7, 8, 10 };
                    return StandardOutputBo.GetRandomValueByDataType(dataType, set: months);
                }
            }
            return StandardOutputBo.GetRandomValueByDataType(dataType);
        }
    }
}
