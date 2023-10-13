using Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessObject
{
    public class StandardSoaDataOutputBo
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

        public const int TypeCompanyName = 1;
        public const int TypeBusinessCode = 2;
        public const int TypeTreatyId = 3;
        public const int TypeTreatyCode = 4;
        public const int TypeTreatyMode = 5;
        public const int TypeTreatyType = 6;
        public const int TypePlanBlock = 7;
        public const int TypeRiskMonth = 8;
        public const int TypeSoaQuarter = 9;
        public const int TypeRiskQuarter = 10;
        public const int TypeNbPremium = 11;
        public const int TypeRnPremium = 12;
        public const int TypeAltPremium = 13;
        public const int TypeGrossPremium = 14;
        public const int TypeTotalDiscount = 15;
        public const int TypeRiskPremium = 16;
        public const int TypeNoClaimBonus = 17;
        public const int TypeLevy = 18;
        public const int TypeClaim = 19;
        public const int TypeProfitComm = 20;
        public const int TypeSurrenderValue = 21;
        public const int TypeGst = 22;
        public const int TypeModcoReserveIncome = 23;
        public const int TypeRiDeposit = 24;
        public const int TypeDatabaseCommission = 25;
        public const int TypeAdministrationContribution = 26;
        public const int TypeShareOfRiCommissionFromCompulsoryCession = 27;
        public const int TypeRecaptureFee = 28;
        public const int TypeCreditCardCharges = 29;
        public const int TypeBrokerageFee = 30;
        public const int TypeTotalCommission = 31;
        public const int TypeNetTotalAmount = 32;
        public const int TypeSoaReceivedDate = 33;
        public const int TypeBordereauxReceivedDate = 34;
        public const int TypeStatementStatus = 35;
        public const int TypeRemarks1 = 36;
        public const int TypeCurrencyCode = 37;
        public const int TypeCurrencyRate = 38;
        public const int TypeSoaStatus = 39;
        public const int TypeConfirmationDate = 40;

        public const int TypeMax = 40;

        public static List<string> GetNames()
        {
            return new List<string>
            {
                "Company Name",
                "Business Code",
                "Treaty Id",
                "Treaty Code",
                "Treaty Mode",
                "Treaty Type",
                "Plan / Block",
                "Risk Month",
                "Soa Quarter",
                "Risk Quarter",
                "NB Premium",
                "RN Premium",
                "ALT Premium",
                "Gross Premium",
                "Total Discount",
                "Risk Premium",
                "No Claim Bonus",
                "Levy",
                "Claim",
                "Profit Commission",
                "Surrender Value",
                "GST",
                "MODCO Reserve Income",
                "RI Deposit",
                "Database Commission",
                "Administration Contribution",
                "Share Of RI Commission From Compulsory Cession",
                "Recapture Fee",
                "Credit Card Charges",
                "Brokerage Fee",
                "Total Commission",
                "Net Total Amount",
                "SOA Received Date",
                "Bordereaux Received Date",
                "Statement Status",
                "Remarks 1",
                "Currency Code",
                "Currency Rate",
                "SOA Status",
                "Confirmation Date",
            };
        }

        public static List<string> GetPropertyNames()
        {
            return new List<string>
            {
                "CompanyName",
                "BusinessCode",
                "TreatyId",
                "TreatyCode",
                "TreatyMode",
                "TreatyType",
                "PlanBlock",
                "RiskMonth",
                "SoaQuarter",
                "RiskQuarter",
                "NbPremium",
                "RnPremium",
                "AltPremium",
                "GrossPremium",
                "TotalDiscount",
                "RiskPremium",
                "NoClaimBonus",
                "Levy",
                "Claim",
                "ProfitComm",
                "SurrenderValue",
                "Gst",
                "ModcoReserveIncome",
                "RiDeposit",
                "DatabaseCommission",
                "AdministrationContribution",
                "ShareOfRiCommissionFromCompulsoryCession",
                "RecaptureFee",
                "CreditCardCharges",
                "BrokerageFee",
                "TotalCommission",
                "NetTotalAmount",
                "SoaReceivedDate",
                "BordereauxReceivedDate",
                "StatementStatus",
                "Remarks1",
                "CurrencyCode",
                "CurrencyRate",
                "SoaStatus",
                "ConfirmationDate",
            };
        }

        public static List<string> GetCodes()
        {
            return new List<string>
            {
                "COMPANY_NAME",
                "BUSINESS_CODE",
                "TREATY_ID",
                "TREATY_CODE",
                "TREATY_MODE",
                "TREATY_TYPE",
                "PLAN_BLOCK",
                "RISK_MONTH",
                "SOA_QUARTER",
                "RISK_QUARTER",
                "NB_PREMIUM",
                "RN_PREMIUM",
                "ALT_PREMIUM",
                "GROSS_PREMIUM",
                "TOTAL_DISCOUNT",
                "RISK_PREMIUM",
                "NO_CLAIM_BONUS",
                "LEVY",
                "CLAIM",
                "PROFIT_COMM",
                "SURRENDER_VALUE",
                "GST",
                "MODCO_RESERVE_INCOME",
                "RI_DEPOSIT",
                "DATABASE_COMMISSION",
                "ADMINISTRATION_CONTRIBUTION",
                "SHARE_OF_RI_COMMISSION_FROM_COMPULSORY_CESSION",
                "RECAPTURE_FEE",
                "CREDIT_CARD_CHARGES",
                "BROKERAGE_FEE",
                "TOTAL_COMMISSION",
                "NET_TOTAL_AMOUNT",
                "SOA_RECEIVED_DATE",
                "BORDEREAUX_RECEIVED_DATE",
                "STATEMENT_STATUS",
                "REMARKS_1",
                "CURRENCY_CODE",
                "CURRENCY_RATE",
                "SOA_STATUS",
                "CONFIRMATION_DATE",
            };
        }

        public static string GetTypeByName(int key)
        {
            List<string> names = GetNames();
            if (key > 0 && key <= names.Count)
                return names[key - 1];
            return "";
        }

        public static int GetTypeByProperty(string property)
        {
            List<string> properties = GetPropertyNames();
            if (!properties.IsNullOrEmpty())
                return properties.FindIndex(q => q == property) + 1;
            return 0;
        }

        public static int? GetTypeByProperty(string property, bool integer = false)
        {
            int type = GetTypeByProperty(property);
            if (type <= 0 && !integer)
            {
                return null;
            }
            return type;
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
                TypeSoaReceivedDate,
                TypeBordereauxReceivedDate,
                TypeConfirmationDate,
            };
        }

        public static List<int> GetStringTypes()
        {
            return new List<int>
            {
                TypeCompanyName,
                TypeBusinessCode,
                TypeTreatyId,
                TypeTreatyCode,
                TypeTreatyMode,
                TypeTreatyType,
                TypePlanBlock,
                TypeSoaQuarter,
                TypeRiskQuarter,
                TypeRemarks1,
                TypeSoaStatus,
            };
        }

        public static List<int> GetAmountTypes()
        {
            return new List<int>
            {
                TypeNbPremium,
                TypeRnPremium,
                TypeAltPremium,
                TypeGrossPremium,
                TypeTotalDiscount,
                TypeNoClaimBonus,
                TypeLevy,
                TypeClaim,
                TypeProfitComm,
                TypeSurrenderValue,
                TypeGst,
                TypeModcoReserveIncome,
                TypeRiDeposit,
                TypeDatabaseCommission,
                TypeAdministrationContribution,
                TypeShareOfRiCommissionFromCompulsoryCession,
                TypeRecaptureFee,
                TypeCreditCardCharges,
                TypeBrokerageFee,
                TypeTotalCommission,
                TypeNetTotalAmount,
                TypeCurrencyRate,
                TypeRiskPremium,
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
                TypeRiskMonth,
            };
        }

        public static List<int> GetDropDownTypes()
        {
            return new List<int>
            {
                TypeStatementStatus,
                TypeCurrencyCode,
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
