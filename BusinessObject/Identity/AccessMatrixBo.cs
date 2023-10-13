using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessObject.Identity
{
    public class AccessMatrixBo
    {
        public int AccessGroupId { get; set; }

        public int ModuleId { get; set; }

        public string Power { get; set; }

        public string DepartmentName { get; set; }

        public virtual AccessGroupBo AccessGroupBo { get; set; }
        public virtual ModuleBo ModuleBo { get; set; }
        public virtual AccessMatrixBo DbAccessMatrixBo { get; set; }

        public IList<string> Powers { get; set; }

        public enum AccessMatrixCRUD
        {
            C,
            R,
            U,
            D,
        }

        public enum AccessMatrixController
        {
            AccessGroup,
            User,
            Department,
            UserReport,
        }

        public static string[] DefaultPowers = {
            AccessMatrixCRUD.C.ToString(),
            AccessMatrixCRUD.R.ToString(),
            AccessMatrixCRUD.U.ToString(),
            AccessMatrixCRUD.D.ToString(),
        };

        public static char Delimiter = ',';
        public static string DefaultDelimiter = string.Format("{0}", Delimiter);
        public static string DefaultPower = string.Join(DefaultDelimiter, DefaultPowers);

        public static string CheckBoxNameFormat = "{0}_{1}";

        public const string PowerAddDepartment = "AD";
        public const string PowerInactiveUserReport = "IUR";
        public const string PowerApproval = "A";

        public const string PowerManagerClaimDashboard = "MCD";
        public const string PowerIndividualClaimDashboard = "ICD";

        public const string PowerCeoApproval = "CA";
        public const string PowerApprovalOnBehalfCeo = "ABC";
        public const string PowerAssignClaim = "AC";
        public const string PowerUnderwritingFeedback = "UWF";

        public const string PowerApprovalDirectRetro = "ADR";
        public const string PowerApprovalRetroRegister = "ARR";
        public const string PowerApprovalRetroSOA = "ASA";

        public const string PowerCompleteChecklist = "CC";

        public const string PowerUpload = "UL";

        public const string PowerUltimaApproverGroup = "UAG";
        public const string PowerUltimaApproverReviewer = "UAR";
        public const string PowerUltimaApproverHod = "UAH";
        public const string PowerUltimaApproverCeo = "UAC";

        public const string PowerGroupPricing = "GP";

        public AccessMatrixBo() { }

        public AccessMatrixBo(int moduleId)
        {
            ModuleId = moduleId;
            Power = DefaultPower;
        }

        public AccessMatrixBo(int moduleId, int accessGroupId)
        {
            ModuleId = moduleId;
            AccessGroupId = accessGroupId;
            Power = DefaultPower;
        }

        public string PrimaryKey()
        {
            return string.Format("{0}|{1}", ModuleId, AccessGroupId);
        }

        public static string GetPowerName(string power)
        {
            switch (power)
            {
                case "C":
                    return "Create";
                case "R":
                    return "Read";
                case "U":
                    return "Update";
                case "D":
                    return "Delete";

                case PowerAddDepartment:
                    return "All Departments";
                
                case PowerInactiveUserReport:
                    return "Receive Inactive User Report";
                
                case PowerApproval:
                    return "Approval";

                case PowerManagerClaimDashboard:
                    return "Manager";

                case PowerIndividualClaimDashboard:
                    return "Individual";

                case PowerCeoApproval:
                    return "CEO Approval";

                case PowerApprovalOnBehalfCeo:
                    return "Approval on Behalf of CEO";

                case PowerAssignClaim:
                    return "Assign Claim";

                case PowerUnderwritingFeedback:
                    return "Underwriting Feedback";

                case PowerApprovalDirectRetro:
                    return "Direct Retro Approval";

                case PowerApprovalRetroRegister:
                    return "Retro Register Approval";

                case PowerUpload:
                    return "Upload";

                case PowerApprovalRetroSOA:
                    return "Per Life SOA Approval";

                case PowerCompleteChecklist:
                    return "Complete Checklist";

                case PowerUltimaApproverGroup:
                    return "Group Team Approver";

                case PowerUltimaApproverReviewer:
                    return "Reviewer Approver";

                case PowerUltimaApproverHod:
                    return "HOD Approver";

                case PowerUltimaApproverCeo:
                    return "CEO Approver";

                case PowerGroupPricing:
                    return "Group Pricing";

                // example for additional power
                //case "AU":
                //    return "CRUD Admin User";
                //case "MU":
                //    return "CRUD Member User";

                default:
                    return "";
            }
        }

        public bool IsPowerExists(string power)
        {
            if (GetPowers().Contains<String>(power))
            {
                return true;
            }
            return false;
        }

        public void ResetPowers()
        {
            Powers = new List<string> { };
        }

        public string GetPowerFromPowers()
        {
            return string.Join(DefaultDelimiter, Powers);
        }
        
        public string GetPowersAsString()
        {
            IList<string> PowerStrings = new List<string> { };
            foreach (string key in Powers)
            {
                PowerStrings.Add(GetPowerName(key));
            }
            return string.Join(DefaultDelimiter + " ", PowerStrings);
        }

        public string[] GetPowers()
        {
            return Power.Split(Delimiter);
        }

        public void SetPowerFromPowers()
        {
            Power = GetPowerFromPowers();
        }
    }
}
