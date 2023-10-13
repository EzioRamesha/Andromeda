using Shared;
using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingWorkflowObjectBo
    {
        public int Id { get; set; }

        public int Type { get; set; }

        public string TypeName { get; set; }

        public int WorkflowId { get; set; }

        public int ObjectType { get; set; }

        public int ObjectId { get; set; }

        public int ObjectVersionId { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        // Object Details
        public string ObjectTypeName { get; set; }
        public string ObjectClassName { get; set; }
        public string ObjectCode { get; set; }
        public string ObjectName { get; set; }
        public int ObjectVersion { get; set; }

        // Workflow Details
        public string WorkflowCode { get; set; }
        public string WorkflowStatus { get; set; }

        public const int TypeQuotation = 1;
        public const int TypeTreaty = 2;
        public const int MaxType = 2;

        public const int ObjectTypeAdvantageProgram = 1;
        public const int ObjectTypeCampaign = 2;
        public const int ObjectTypeClaimApprovalLimit = 3;
        public const int ObjectTypeCustomOther = 4;
        public const int ObjectTypeDefinitionAndExclusion = 5;
        public const int ObjectTypeFinancialTable = 6;
        public const int ObjectTypeMedicalTable = 7;
        public const int ObjectTypeProduct = 8;
        public const int ObjectTypeProfitCommission = 9;
        public const int ObjectTypeRateTable = 10;
        public const int ObjectTypeUwLimit = 11;
        public const int ObjectTypeUwQuestionnaire = 12;
        public const int ObjectTypeMax = 12;

        public static string GetTypeName(int key)
        {
            switch (key)
            {
                case TypeQuotation:
                    return "Quotation";
                case TypeTreaty:
                    return "Treaty";
                default:
                    return "";
            }
        }

        public static string GetObjectTypeName(int key)
        {
            switch (key)
            {
                case ObjectTypeAdvantageProgram:
                    return "Advantage Program";
                case ObjectTypeCampaign:
                    return "Campaign";
                case ObjectTypeClaimApprovalLimit:
                    return "Claim Approval Limit";
                case ObjectTypeCustomOther:
                    return "Custom / Other";
                case ObjectTypeDefinitionAndExclusion:
                    return "Definitions & Exclusions";
                case ObjectTypeFinancialTable:
                    return "Financial Table";
                case ObjectTypeMedicalTable:
                    return "Medical Table";
                case ObjectTypeProduct:
                    return "Product";
                case ObjectTypeProfitCommission:
                    return "Profit Commission";
                case ObjectTypeRateTable:
                    return "Rate Table";
                case ObjectTypeUwLimit:
                    return "Underwriting Limit";
                case ObjectTypeUwQuestionnaire:
                    return "Underwriting Questionnaire";
                default:
                    return "";
            }
        }

        public static string GetObjectTypeClassName(int key)
        {
            switch (key)
            {
                case ObjectTypeAdvantageProgram:
                    return "TreatyPricingAdvantageProgram";
                case ObjectTypeCampaign:
                    return "TreatyPricingCampaign";
                case ObjectTypeClaimApprovalLimit:
                    return "TreatyPricingClaimApprovalLimit";
                case ObjectTypeCustomOther:
                    return "TreatyPricingCustomOther";
                case ObjectTypeDefinitionAndExclusion:
                    return "TreatyPricingDefinitionAndExclusion";
                case ObjectTypeFinancialTable:
                    return "TreatyPricingFinancialTable";
                case ObjectTypeMedicalTable:
                    return "TreatyPricingMedicalTable";
                case ObjectTypeProduct:
                    return "TreatyPricingProduct";
                case ObjectTypeProfitCommission:
                    return "TreatyPricingProfitCommission";
                case ObjectTypeRateTable:
                    return "TreatyPricingRateTable";
                case ObjectTypeUwLimit:
                    return "TreatyPricingUwLimit";
                case ObjectTypeUwQuestionnaire:
                    return "TreatyPricingUwQuestionnaire";
                default:
                    return "";
            }
        }

        public static int GetObjectTypeFromController(string controller)
        {
            switch (controller)
            {
                case "TreatyPricingAdvantageProgram":
                    return ObjectTypeAdvantageProgram;
                case "TreatyPricingCampaign":
                    return ObjectTypeCampaign;
                case "TreatyPricingClaimApprovalLimit":
                    return ObjectTypeClaimApprovalLimit;
                case "TreatyPricingCustomOther":
                    return ObjectTypeCustomOther;
                case "TreatyPricingDefinitionAndExclusion":
                    return ObjectTypeDefinitionAndExclusion;
                case "TreatyPricingFinancialTable":
                    return ObjectTypeFinancialTable;
                case "TreatyPricingMedicalTable":
                    return ObjectTypeMedicalTable;
                case "TreatyPricingProduct":
                    return ObjectTypeProduct;
                case "TreatyPricingProfitCommission":
                    return ObjectTypeProfitCommission;
                case "TreatyPricingRateTable":
                    return ObjectTypeRateTable;
                case "TreatyPricingUwLimit":
                    return ObjectTypeUwLimit;
                case "TreatyPricingUwQuestionnaire":
                    return ObjectTypeUwQuestionnaire;
                default:
                    return 0;
            }
        }

        public static string GetCode(int type, object obj)
        {
            switch (type)
            {
                case ObjectTypeFinancialTable:
                    return obj.GetPropertyValue("FinancialTableId")?.ToString();
                case ObjectTypeMedicalTable:
                    return obj.GetPropertyValue("MedicalTableId")?.ToString();
                case ObjectTypeUwLimit:
                    return obj.GetPropertyValue("LimitId")?.ToString();
                default:
                    return obj.GetPropertyValue("Code")?.ToString();
            }
        }

        public static string GetName(int type, object obj)
        {
            switch (type)
            {
                case ObjectTypeProfitCommission:
                case ObjectTypeRateTable:
                    return obj.GetPropertyValue("Description")?.ToString();
                default:
                    return obj.GetPropertyValue("Name")?.ToString();
            }
        }

        public void SetObjectDetails(object obj, object versionObj)
        {
            ObjectTypeName = GetObjectTypeName(ObjectType);
            ObjectCode = GetCode(ObjectType, obj);
            ObjectName = GetName(ObjectType, obj);
            ObjectVersion = int.Parse(versionObj.GetPropertyValue("Version").ToString());
        }
    }
}
