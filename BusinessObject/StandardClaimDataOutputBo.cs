using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class StandardClaimDataOutputBo
    {
        public int Id { get; set; }

        public int Type { get; set; }

        public string TypeName { get; set; }

        public int DataType { get; set; }

        public string DataTypeName { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string DummyValue { get; set; }

        public string Property { get; set; }

        // Used in Eval only
        public bool IsRiData { get; set; }
        public bool IsOriginal { get; set; }

        public const int TypePolicyNumber = 1;
        public const int TypePolicyTerm = 2;
        public const int TypeClaimRecoveryAmt = 3;
        public const int TypeClaimTransactionType = 4;
        public const int TypeTreatyCode = 5;
        public const int TypeTreatyType = 6;
        public const int TypeAarPayable = 7;
        public const int TypeAnnualRiPrem = 8;
        public const int TypeCauseOfEvent = 9;
        public const int TypeCedantClaimEventCode = 10;
        public const int TypeCedantClaimType = 11;
        public const int TypeCedantDateOfNotification = 12;
        public const int TypeCedingBenefitRiskCode = 13;
        public const int TypeCedingBenefitTypeCode = 14;
        public const int TypeCedingClaimType = 15;
        public const int TypeCedingCompany = 16;
        public const int TypeCedingEventCode = 17;
        public const int TypeCedingPlanCode = 18;
        public const int TypeCurrencyRate = 19;
        public const int TypeDateApproved = 20;
        public const int TypeDateOfEvent = 21;
        public const int TypeDateOfRegister = 22;
        public const int TypeDateOfReported = 23;
        public const int TypeEntryNo = 24;
        public const int TypeExGratia = 25;
        public const int TypeForeignClaimRecoveryAmt = 26;
        public const int TypeFundsAccountingTypeCode = 27;
        public const int TypeInsuredDateOfBirth = 28;
        public const int TypeInsuredGenderCode = 29;
        public const int TypeInsuredName = 30;
        public const int TypeInsuredTobaccoUse = 31;
        public const int TypeLastTransactionDate = 32;
        public const int TypeLastTransactionQuarter = 33;
        public const int TypeLateInterest = 34;
        public const int TypeLayer1SumRein = 35;
        public const int TypeMfrs17AnnualCohort = 36;
        public const int TypeMfrs17ContractCode = 37;
        public const int TypeMlreBenefitCode = 38;
        public const int TypeMlreEventCode = 39;
        public const int TypeMlreInvoiceDate = 40;
        public const int TypeMlreInvoiceNumber = 41;
        public const int TypeMlreRetainAmount = 42;
        public const int TypeMlreShare = 43;
        public const int TypePendingProvisionDay = 44;
        public const int TypePolicyDuration = 45;
        public const int TypeRecordType = 46;
        public const int TypeReinsBasisCode = 47;
        public const int TypeReinsEffDatePol = 48;
        public const int TypeRetroParty1 = 49;
        public const int TypeRetroParty2 = 50;
        public const int TypeRetroParty3 = 51;
        public const int TypeRetroRecovery1 = 52;
        public const int TypeRetroRecovery2 = 53;
        public const int TypeRetroRecovery3 = 54;
        public const int TypeRetroStatementDate1 = 55;
        public const int TypeRetroStatementDate2 = 56;
        public const int TypeRetroStatementDate3 = 57;
        public const int TypeRetroStatementId1 = 58;
        public const int TypeRetroStatementId2 = 59;
        public const int TypeRetroStatementId3 = 60;
        public const int TypeRiskPeriodMonth = 61;
        public const int TypeRiskPeriodYear = 62;
        public const int TypeRiskQuarter = 63;
        public const int TypeSaFactor = 64;
        public const int TypeSoaQuarter = 65;
        public const int TypeSumIns = 66;
        public const int TypeTempA1 = 67;
        public const int TypeTempA2 = 68;
        public const int TypeTempD1 = 69;
        public const int TypeTempD2 = 70;
        public const int TypeTempI1 = 71;
        public const int TypeTempI2 = 72;
        public const int TypeTempS1 = 73;
        public const int TypeTempS2 = 74;
        public const int TypeTransactionDateWop = 75;
        public const int TypeClaimId = 76;
        public const int TypeClaimCode = 77;
        public const int TypeCurrencyCode = 78;

        public const int TypeCustomField = 79;

        public const int TypeIssueDatePol = 80;
        public const int TypeCedingTreatyCode = 81;
        public const int TypeCampaignCode = 82;

        public const int TypeDateOfIntimation = 83;

        public const int TypePolicyExpiryDate = 84;

        public const int TypeMax = 84;

        public static StandardClaimDataOutputBo GetByType(int type)
        {
            if (type < 1)
                return null;
            if (type > TypeMax)
                return null;

            return new StandardClaimDataOutputBo
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

        public static List<string> GetNames()
        {
            return new List<string>
            {
                "Policy Number",
                "Policy Term",
                "Claim Recovery Amt",
                "Claim Transaction Type",
                "Treaty Code",
                "Treaty Type",
                "Aar Payable",
                "Annual Ri Prem",
                "Cause Of Event",
                "Cedant Claim Event Code",
                "Cedant Claim Type",
                "Cedant Date Of Notification",
                "Ceding Benefit Risk Code",
                "Ceding Benefit Type Code",
                "Ceding Claim Type",
                "Ceding Company",
                "Ceding Event Code",
                "Ceding Plan Code",
                "Currency Rate",
                "Date Approved",
                "Date Of Event",
                "Date Of Register",
                "Date Of Reported",
                "Entry No",
                "Ex Gratia",
                "Foreign Claim Recovery Amt",
                "Funds Accounting Type Code",
                "Insured Date Of Birth",
                "Insured Gender Code",
                "Insured Name",
                "Insured Tobacco Use",
                "Last Transaction Date",
                "Last Transaction Quarter",
                "Late Interest",
                "Layer 1 Sum Rein",
                "Mfrs17 Annual Cohort",
                "Mfrs17 Contract Code",
                "MLRe Benefit Code",
                "MLRe Event Code",
                "MLRe Invoice Date",
                "MLRe Invoice Number",
                "MLRe Retain Amount",
                "MLRe Share",
                "Pending Provision Day",
                "Policy Duration",
                "Record Type",
                "Reins Basis Code",
                "Reins Eff Date Pol",
                "Retro Party 1",
                "Retro Party 2",
                "Retro Party 3",
                "Retro Recovery 1",
                "Retro Recovery 2",
                "Retro Recovery 3",
                "Retro Statement Date 1",
                "Retro Statement Date 2",
                "Retro Statement Date 3",
                "Retro Statement Id 1",
                "Retro Statement Id 2",
                "Retro Statement Id 3",
                "Risk Period Month",
                "Risk Period Year",
                "Risk Quarter",
                "Sa Factor",
                "Soa Quarter",
                "Sum Ins",
                "Temp A 1",
                "Temp A 2",
                "Temp D 1",
                "Temp D 2",
                "Temp I 1",
                "Temp I 2",
                "Temp S 1",
                "Temp S 2",
                "Transaction Date Wop",
                "Claim Id",
                "Claim Code",
                "Currency Code",

                "Custom Field",

                "Issue Date Pol",
                "Ceding Treaty Code",
                "Campaign Code",

                "Date Of Intimation",

                "Policy Expiry Date",
            };
        }

        public static List<string> GetPropertyNames()
        {
            return new List<string>
            {
                "PolicyNumber",
                "PolicyTerm",
                "ClaimRecoveryAmt",
                "ClaimTransactionType",
                "TreatyCode",
                "TreatyType",
                "AarPayable",
                "AnnualRiPrem",
                "CauseOfEvent",
                "CedantClaimEventCode",
                "CedantClaimType",
                "CedantDateOfNotification",
                "CedingBenefitRiskCode",
                "CedingBenefitTypeCode",
                "CedingClaimType",
                "CedingCompany",
                "CedingEventCode",
                "CedingPlanCode",
                "CurrencyRate",
                "DateApproved",
                "DateOfEvent",
                "DateOfRegister",
                "DateOfReported",
                "EntryNo",
                "ExGratia",
                "ForeignClaimRecoveryAmt",
                "FundsAccountingTypeCode",
                "InsuredDateOfBirth",
                "InsuredGenderCode",
                "InsuredName",
                "InsuredTobaccoUse",
                "LastTransactionDate",
                "LastTransactionQuarter",
                "LateInterest",
                "Layer1SumRein",
                "Mfrs17AnnualCohort",
                "Mfrs17ContractCode",
                "MlreBenefitCode",
                "MlreEventCode",
                "MlreInvoiceDate",
                "MlreInvoiceNumber",
                "MlreRetainAmount",
                "MlreShare",
                "PendingProvisionDay",
                "PolicyDuration",
                "RecordType",
                "ReinsBasisCode",
                "ReinsEffDatePol",
                "RetroParty1",
                "RetroParty2",
                "RetroParty3",
                "RetroRecovery1",
                "RetroRecovery2",
                "RetroRecovery3",
                "RetroStatementDate1",
                "RetroStatementDate2",
                "RetroStatementDate3",
                "RetroStatementId1",
                "RetroStatementId2",
                "RetroStatementId3",
                "RiskPeriodMonth",
                "RiskPeriodYear",
                "RiskQuarter",
                "SaFactor",
                "SoaQuarter",
                "SumIns",
                "TempA1",
                "TempA2",
                "TempD1",
                "TempD2",
                "TempI1",
                "TempI2",
                "TempS1",
                "TempS2",
                "TransactionDateWop",
                "ClaimId",
                "ClaimCode",
                "CurrencyCode",

                "CustomField",

                "IssueDatePol",
                "CedingTreatyCode",
                "CampaignCode",

                "DateOfIntimation",

                "PolicyExpiryDate",
            };
        }

        public static List<string> GetCodes()
        {
            return new List<string>
            {
                "POLICY_NUMBER",
                "POLICY_TERM",
                "CLAIM_RECOVERY_AMT",
                "CLAIM_TRANSACTION_TYPE",
                "TREATY_CODE",
                "TREATY_TYPE",
                "AAR_PAYABLE",
                "ANNUAL_RI_PREM",
                "CAUSE_OF_EVENT",
                "CEDANT_CLAIM_EVENT_CODE",
                "CEDANT_CLAIM_TYPE",
                "CEDANT_DATE_OF_NOTIFICATION",
                "CEDING_BENEFIT_RISK_CODE",
                "CEDING_BENEFIT_TYPE_CODE",
                "CEDING_CLAIM_TYPE",
                "CEDING_COMPANY",
                "CEDING_EVENT_CODE",
                "CEDING_PLAN_CODE",
                "CURRENCY_RATE",
                "DATE_APPROVED",
                "DATE_OF_EVENT",
                "DATE_OF_REGISTER",
                "DATE_OF_REPORTED",
                "ENTRY_NO",
                "EX_GRATIA",
                "FOREIGN_CLAIM_RECOVERY_AMT",
                "FUNDS_ACCOUNTING_TYPE_CODE",
                "INSURED_DATE_OF_BIRTH",
                "INSURED_GENDER_CODE",
                "INSURED_NAME",
                "INSURED_TOBACCO_USE",
                "LAST_TRANSACTION_DATE",
                "LAST_TRANSACTION_QUARTER",
                "LATE_INTEREST",
                "LAYER_1_SUM_REIN",
                "MFRS17_ANNUAL_COHORT",
                "MFRS17_CONTRACT_CODE",
                "MLRE_BENEFIT_CODE",
                "MLRE_EVENT_CODE",
                "MLRE_INVOICE_DATE",
                "MLRE_INVOICE_NUMBER",
                "MLRE_RETAIN_AMOUNT",
                "MLRE_SHARE",
                "PENDING_PROVISION_DAY",
                "POLICY_DURATION",
                "RECORD_TYPE",
                "REINS_BASIS_CODE",
                "REINS_EFF_DATE_POL",
                "RETRO_PARTY_1",
                "RETRO_PARTY_2",
                "RETRO_PARTY_3",
                "RETRO_RECOVERY_1",
                "RETRO_RECOVERY_2",
                "RETRO_RECOVERY_3",
                "RETRO_STATEMENT_DATE_1",
                "RETRO_STATEMENT_DATE_2",
                "RETRO_STATEMENT_DATE_3",
                "RETRO_STATEMENT_ID_1",
                "RETRO_STATEMENT_ID_2",
                "RETRO_STATEMENT_ID_3",
                "RISK_PERIOD_MONTH",
                "RISK_PERIOD_YEAR",
                "RISK_QUARTER",
                "SA_FACTOR",
                "SOA_QUARTER",
                "SUM_INS",
                "TEMP_A_1",
                "TEMP_A_2",
                "TEMP_D_1",
                "TEMP_D_2",
                "TEMP_I_1",
                "TEMP_I_2",
                "TEMP_S_1",
                "TEMP_S_2",
                "TRANSACTION_DATE_WOP",
                "CLAIM_ID",
                "CLAIM_CODE",
                "CURRENCY_CODE",

                "CUSTOM_FIELD",

                "ISSUE_DATE_POL",
                "CEDING_TREATY_CODE",
                "CAMPAIGN_CODE",

                "DATE_OF_INTIMATION",

                "POLICY_EXPIRY_DATE",
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

        public static List<int> GetDateTypes()
        {
            return new List<int>
            {
                TypeCedantDateOfNotification,
                TypeDateApproved,
                TypeDateOfEvent,
                TypeDateOfRegister,
                TypeDateOfReported,
                TypeInsuredDateOfBirth,
                TypeLastTransactionDate,
                TypeMlreInvoiceDate,
                TypeReinsEffDatePol,
                TypeRetroStatementDate1,
                TypeRetroStatementDate2,
                TypeRetroStatementDate3,
                TypeTempD1,
                TypeTempD2,
                TypeTransactionDateWop,
                TypeIssueDatePol,
                TypeDateOfIntimation,
                TypePolicyExpiryDate,
            };
        }

        public static List<int> GetStringTypes()
        {
            return new List<int>
            {
                TypePolicyNumber,
                TypeTreatyCode,
                TypeCauseOfEvent,
                TypeCedantClaimEventCode,
                TypeCedantClaimType,
                TypeCedingBenefitRiskCode,
                TypeCedingClaimType,
                TypeCedingCompany,
                TypeCedingEventCode,
                TypeCedingPlanCode,
                TypeEntryNo,
                TypeInsuredName,
                TypeLastTransactionQuarter,
                TypeMfrs17ContractCode,
                TypeMlreBenefitCode,
                TypeMlreEventCode,
                TypeMlreInvoiceNumber,
                TypeRetroParty1,
                TypeRetroParty2,
                TypeRetroParty3,
                TypeRetroStatementId1,
                TypeRetroStatementId2,
                TypeRetroStatementId3,
                TypeRiskQuarter,
                TypeSoaQuarter,
                TypeTempS1,
                TypeTempS2,
                TypeClaimId,
                TypeClaimCode,
                TypeCedingTreatyCode,
                TypeCampaignCode,
            };
        }
        
        public static List<int> GetAmountTypes()
        {
            return new List<int>
            {
                TypePolicyTerm,
                TypeClaimRecoveryAmt,
                TypeAarPayable,
                TypeAnnualRiPrem,
                TypeCurrencyRate,
                TypeExGratia,
                TypeForeignClaimRecoveryAmt,
                TypeLateInterest,
                TypeLayer1SumRein,
                TypeMlreRetainAmount,
                TypeMlreShare,
                TypeRetroRecovery1,
                TypeRetroRecovery2,
                TypeRetroRecovery3,
                TypeSaFactor,
                TypeSumIns,
                TypeTempA1,
                TypeTempA2,
            };
        }
        
        public static List<int> GetExportFileOrder()
        {
            return new List<int>
            {
                TypeEntryNo,
                TypeClaimId,
                TypeClaimTransactionType,
                TypeRecordType,
                TypeDateOfRegister,
                TypeDateOfReported,
                TypeCedantDateOfNotification,
                TypeDateApproved,
                TypePolicyNumber,
                TypeInsuredName,
                TypeInsuredGenderCode,
                TypeInsuredTobaccoUse,
                TypeInsuredDateOfBirth,
                TypeIssueDatePol,
                TypeReinsEffDatePol,
                TypePolicyExpiryDate,
                TypeDateOfEvent,
                TypeCauseOfEvent,
                TypeClaimRecoveryAmt,
                TypeLateInterest,
                TypeAarPayable,
                TypeSumIns,
                TypeCedingPlanCode,
                TypeClaimCode,
                TypeMlreBenefitCode,
                TypeMlreEventCode,
                TypeCedingClaimType,
                TypeCedingEventCode,
                TypeCedantClaimEventCode,
                TypeCedantClaimType,
                TypeCedingBenefitRiskCode,
                TypeCedingBenefitTypeCode,
                TypeCedingTreatyCode,
                TypeCampaignCode,
                TypeCedingCompany,
                TypeTreatyCode,
                TypeTreatyType,
                TypeReinsBasisCode,
                TypeFundsAccountingTypeCode,
                TypeSoaQuarter,
                TypeRiskQuarter,
                TypeRiskPeriodYear,
                TypeRiskPeriodMonth,
                TypePolicyTerm,
                TypeAnnualRiPrem,
                TypeTransactionDateWop,
                TypeSaFactor,
                TypeCurrencyCode,
                TypeCurrencyRate,
                TypeForeignClaimRecoveryAmt,
                TypeExGratia,
                TypeDateOfIntimation,
                TypeLastTransactionDate,
                TypeLastTransactionQuarter,
                TypeMfrs17AnnualCohort,
                TypeMfrs17ContractCode,
                TypePendingProvisionDay,
                TypePolicyDuration,
                TypeLayer1SumRein,
                TypeMlreShare,
                TypeMlreRetainAmount,
                TypeRetroParty1,
                TypeRetroParty2,
                TypeRetroParty3,
                TypeRetroRecovery1,
                TypeRetroRecovery2,
                TypeRetroRecovery3,
                TypeRetroStatementDate1,
                TypeRetroStatementDate2,
                TypeRetroStatementDate3,
                TypeRetroStatementId1,
                TypeRetroStatementId2,
                TypeRetroStatementId3,
                TypeMlreInvoiceNumber,
                TypeMlreInvoiceDate,
                TypeTempA1,
                TypeTempA2,
                TypeTempD1,
                TypeTempD2,
                TypeTempI1,
                TypeTempI2,
                TypeTempS1,
                TypeTempS2,
                TypeCustomField
            };
        }

        public static List<int> GetPercentageTypes()
        {
            return new List<int>
            {
            };
        }

        public static List<int> GetIntegerTypes()
        {
            return new List<int>
            {
                TypeMfrs17AnnualCohort,
                TypePendingProvisionDay,
                TypePolicyDuration,
                TypeRiskPeriodMonth,
                TypeRiskPeriodYear,
                TypeTempI1,
                TypeTempI2,
            };
        }
        
        public static List<int> GetDropDownTypes()
        {
            return new List<int>
            {
                TypeTreatyType,
                TypeCedingBenefitTypeCode,
                TypeFundsAccountingTypeCode,
                TypeInsuredGenderCode,
                TypeInsuredTobaccoUse,
                TypeReinsBasisCode,
                TypeCurrencyCode,
                TypeClaimTransactionType,
                TypeRecordType,
            };
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

        public static object GetRandomValueByType(int key)
        {
            int dataType = GetDataTypeByType(key);
            if (dataType == StandardOutputBo.DataTypeInteger)
            {
                string code = GetCodeByType(key);
                if (code.Contains("YEAR"))
                {
                    return GetRandomValueByDataType(dataType, 2020, 2020);
                }
                else if (code.Contains("MONTH"))
                {
                    return GetRandomValueByDataType(dataType, 1, 12);
                }
            }
            return GetRandomValueByDataType(dataType);
        }

        public static object GetRandomValueByDataType(int key, int minValue = 1, int maxValue = 100)
        {
            switch (key)
            {
                case StandardOutputBo.DataTypeDate:
                    return DateTime.Now;
                case StandardOutputBo.DataTypeString:
                case StandardOutputBo.DataTypeDropDown:
                    return Util.GenerateRandomString(20, 20, true);
                case StandardOutputBo.DataTypeAmount:
                case StandardOutputBo.DataTypePercentage:
                    return new Random().Next(100);
                case StandardOutputBo.DataTypeInteger:
                    return new Random().Next(minValue, maxValue);
                default:
                    return null;
            }
        }
    }
}
