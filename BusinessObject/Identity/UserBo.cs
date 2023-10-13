using Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessObject.Identity
{
    public class UserBo
    {
        public int Id { get; set; }

        public int Status { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordHash { get; set; }

        public string FullName { get; set; }

        public string SessionId { get; set; }
        
        public DateTime? PasswordExpiresAt { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public int? CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public int? DepartmentId { get; set; }

        public DepartmentBo DepartmentBo { get; set; }

        public int LoginMethod { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual IList<UserAccessGroupBo> UserAccessGroupBos { get; set; }

        public static int DefaultSuperUserId = 1;

        public const int StatusPending = 1;
        public const int StatusActive = 2;
        public const int StatusSuspend = 3;
        public const int StatusDelete = 4;

        public const int MaxStatus = 4;

        public const int LoginMethodPassword = 1;
        public const int LoginMethodAD = 2;

        public const int MaxLoginMethod = 2;

        public const int RequestTypeNew = 1;
        public const int RequestTypeAmendment = 2;

        public const int MaxRequestType = 2;

        public UserBo()
        {
            Status = StatusActive;
            UserAccessGroupBos = new List<UserAccessGroupBo>() { };
        }

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusPending:
                    return "Pending";
                case StatusActive:
                    return "Active";
                case StatusSuspend:
                    return "Suspended";
                case StatusDelete:
                    return "Deleted";
                default:
                    return "";
            }
        }
        
        public static string GetLoginMethodName(int key)
        {
            switch (key)
            {
                case LoginMethodPassword:
                    return "Password";
                case LoginMethodAD:
                    return "Active Directory";
                default:
                    return "";
            }
        }

        public static string GetRequestTypeName(int key)
        {
            switch (key)
            {
                case RequestTypeNew:
                    return "Create New";
                case RequestTypeAmendment:
                    return "Amendments";
                default:
                    return "";
            }
        }

        public static string GetStatusClass(int key)
        {
            switch (key)
            {
                case StatusPending:
                    return "status-processing-badge";
                case StatusActive:
                    return "status-success-badge";
                case StatusSuspend:
                    return "status-fail-badge";
                default:
                    return "";
            }
        }

        public static string GetPasswordRequirements()
        {
            List<string> passwordRequirements = new List<string>();

            int passwordMinLength = int.Parse(Util.GetConfig("PasswordMinLength"));
            int passwordMaxLength = int.Parse(Util.GetConfig("PasswordMaxLength"));
            passwordRequirements.Add(string.Format(MessageBag.PasswordLengthRequirement, passwordMinLength, passwordMaxLength));

            List<string> passwordContains = new List<string>();
            if (Boolean.Parse(Util.GetConfig("RequireNonLetterOrDigit")))
            {
                passwordContains.Add("1 special character");
            }
            
            if (Boolean.Parse(Util.GetConfig("PasswordRequireDigit")))
            {
                passwordContains.Add("1 number");
            }
            
            if (Boolean.Parse(Util.GetConfig("PasswordRequireLowercase")))
            {
                passwordContains.Add("1 lowercase character");
            }

            if (Boolean.Parse(Util.GetConfig("PasswordRequireUppercase")))
            {
                passwordContains.Add("1 uppercase character");
            }

            if (passwordContains.Count() > 0)
            {
                string passwordContain = string.Join(", ", passwordContains);
                passwordRequirements.Add(string.Format(MessageBag.PasswordContainRequirement, passwordContain));
            }

            string passwordRequirement = "Password should " + string.Join(", ", passwordRequirements);

            return passwordRequirement;
        }

        public bool IsPasswordExpired()
        {
            return false;
        }
    }
}
