using System;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingQuotationWorkflowVersionQuotationChecklistBo
    {
        public int Id { get; set; }

        public int TreatyPricingQuotationWorkflowVersionId { get; set; }

        public virtual TreatyPricingQuotationWorkflowVersionBo TreatyPricingQuotationWorkflowVersionBo { get; set; }

        public string InternalTeam { get; set; }

        public string InternalTeamPersonInCharge { get; set; }

        public int Status { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string StatusName { get; set; }

        public bool DisableButtons { get; set; }

        public bool DisableRequest { get; set; }

        public bool DisablePersonInCharge { get; set; }

        public const int StatusNotRequired = 1;
        public const int StatusRequested = 2;
        public const int StatusCompleted = 3;
        public const int StatusPendingSignOff = 4;
        public const int StatusApproved = 5;

        public const int DefaultInternalTeamCEO = 1;
        public const int DefaultInternalTeamBD = 6;
        public const int DefaultInternalTeamPricing = 2;
        public const int DefaultInternalTeamUnderwriting = 3;
        public const int DefaultInternalTeamHealth = 4;
        public const int DefaultInternalTeamClaims = 5;
        public const int DefaultInternalTeamCR = 7;

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusNotRequired:
                    return "Not Required";
                case StatusRequested:
                    return "Requested";
                case StatusCompleted:
                    return "Completed";
                case StatusPendingSignOff:
                    return "Pending Sign-Off";
                case StatusApproved:
                    return "Approved";
                default:
                    return "";
            }
        }

        public static string GetInternalTeamName(int key)
        {
            switch (key)
            {
                case DefaultInternalTeamCEO:
                    return "CEO";
                case DefaultInternalTeamBD:
                    return "Business Development & Group";
                case DefaultInternalTeamPricing:
                    return "Product Pricing";
                case DefaultInternalTeamUnderwriting:
                    return "Underwriting";
                case DefaultInternalTeamHealth:
                    return "Health";
                case DefaultInternalTeamClaims:
                    return "Claims";
                case DefaultInternalTeamCR:
                    return "Compliance & Risk";
                default:
                    return "";
            }
        }

        public static int GetInternalTeamId(string key)
        {
            switch (key)
            {
                case "CEO":
                    return DefaultInternalTeamCEO;
                case "Business Development & Group":
                    return DefaultInternalTeamBD;
                case "Product Pricing":
                    return DefaultInternalTeamPricing;
                case "Underwriting":
                    return DefaultInternalTeamUnderwriting;
                case "Health":
                    return DefaultInternalTeamHealth;
                case "Claims":
                    return DefaultInternalTeamClaims;
                case "Compliance & Risk":
                    return DefaultInternalTeamCR;
                default:
                    return 0;
            }
        }
    }
}
