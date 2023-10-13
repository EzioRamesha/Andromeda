using BusinessObject.Identity;
using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class EmailBo
    {
        public int Id { get; set; }

        public int? RecipientUserId { get; set; }

        public virtual UserBo RecipientUserBo { get; set; }

        public string ModuleController { get; set; }

        public int? ObjectId { get; set; }

        public int Type { get; set; }

        public int Status { get; set; }

        public string EmailAddress { get; set; }

        public string Data { get; set; }

        public List<string> DataList { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public const int StatusPending = 1;
        public const int StatusSent = 2;
        public const int StatusFailed = 3;

        public const int TypeNewPasswordUser = 1;
        public const int TypeNewActiveDirectoryUser = 2;
        public const int TypeChangePassword = 3;
        public const int TypeRequestTemporaryPassword = 4;
        public const int TypeResetTemporaryPassword = 5;
        public const int TypeClaimRegisterPendingComment = 6;
        public const int TypeClaimRegisterPendingApproval = 7;
        public const int TypeInactiveUserReport = 8;
        public const int TypePasswordExpiryReminder = 9;
        public const int TypeNotifyTreatyWorkflowStatusUpdate = 10;
        public const int TypeNotifyQuotationWorkflowChecklist = 11;
        public const int TypeNotifyGroupReferralChecklist = 12;
        public const int TypeNotifyQuotationWorkflowPricingStatusUpdate = 13;
        public const int TypeNotifyQuotationWorkflowStatusUpdate = 14;
        public const int TypeNotifyTreatyWorkflowPicUpdate = 15;
        public const int TypeNotifyGroupReferralStatusUpdate = 16;

        public EmailBo()
        {
            Status = StatusPending;
            DataList = new List<string>();
        }

        public EmailBo(int type, string emailAddress)
        {
            Status = StatusPending;
            Type = type;
            EmailAddress = emailAddress;
            DataList = new List<string>();
        }

        public static string GetSubject(int type)
        {
            switch (type)
            {
                case TypeNewPasswordUser:
                case TypeNewActiveDirectoryUser:
                    return "New User";
                case TypeChangePassword:
                    return "Password Changed";
                case TypeRequestTemporaryPassword:
                case TypeResetTemporaryPassword:
                    return "Temporary Password";
                case TypeClaimRegisterPendingComment:
                    return "Claim Register Pending Comment";
                case TypeClaimRegisterPendingApproval:
                    return "Claim Register Pending Approval";
                case TypeInactiveUserReport:
                    return "Inactive User Report";
                case TypePasswordExpiryReminder:
                    return "Password Expiry Reminder";
                case TypeNotifyTreatyWorkflowStatusUpdate:
                    return "Treaty Workflow Status Update";
                case TypeNotifyQuotationWorkflowChecklist:
                    return "Quotation Workflow Checklist Request";
                case TypeNotifyGroupReferralChecklist:
                    return "Group Referral Checklist Request";
                case TypeNotifyQuotationWorkflowPricingStatusUpdate:
                    return "Quotation Workflow Pricing Status Update";
                case TypeNotifyQuotationWorkflowStatusUpdate:
                    return "Quotation Workflow Status Update";
                case TypeNotifyTreatyWorkflowPicUpdate:
                    return "Treaty Workflow PIC Assigned";
                case TypeNotifyGroupReferralStatusUpdate:
                    return "Group Referral Status Update";
                default:
                    return "";
            }
        }

        public static string GetTemplate(int type)
        {
            switch (type)
            {
                case TypeNewPasswordUser:
                    return "A new user has been created with this email. The user credentials are as below:- \n\nUsername: {0} \nPassword: {1}";
                case TypeNewActiveDirectoryUser:
                    return "A new user has been created with this email. The user credentials are as below:- \n\nUsername: {0}";
                case TypeChangePassword:
                    return "Your password has been changed by the System Administrator. Your new password is {0}";
                case TypeRequestTemporaryPassword:
                case TypeResetTemporaryPassword:
                    return "Your temporary password is {0}";
                case TypeClaimRegisterPendingComment:
                    return "You have a claim register({0}) pending your comment.";
                case TypeClaimRegisterPendingApproval:
                    return "You have a claim register({0}) pending your approval.";
                case TypeInactiveUserReport:
                    return "Attached below is the latest Inactive User Report";
                case TypePasswordExpiryReminder:
                    return "Hi {0}, \n\nYour password will expire in {1} days on {2}. Please remember to change your password.";
                case TypeNotifyTreatyWorkflowStatusUpdate:
                    return "Hi{0}, \n\nThe status for Treaty Workflow (Document ID: {1}) has been changed to {2}. \n\nLink: {3} \n\nStatus Remarks: {4}";
                case TypeNotifyQuotationWorkflowChecklist:
                    return "Hi {0}, \n\nA quotation is pending {1}. \n\nQuotation ID: {2} \n\nLink: {3}";
                case TypeNotifyGroupReferralChecklist:
                    return "Hi {0}, \n\nA referral is pending your review/sign off. \n\nGroup Referral ID: {1} \n\nLink: {2}";
                case TypeNotifyQuotationWorkflowPricingStatusUpdate:
                    return "Hi{0}, \n\nPricing status for Quotation ID: {1} has been updated to {2}. \n\nStatus Remarks: {3} \n\nLink: {4}";
                case TypeNotifyQuotationWorkflowStatusUpdate:
                    return "Hi{0}, \n\nStatus for Quotation ID: {1} has been updated to {2}. \n\nStatus Remarks: {3} \n\nLink: {4}";
                case TypeNotifyTreatyWorkflowPicUpdate:
                    return "Hi {0}, \n\nPerson In-Charge for Treaty Workflow (Document ID: {1}) has been assigned to you. \n\nLink: {2}";
                case TypeNotifyGroupReferralStatusUpdate:
                    return "Hi{0}, \n\nStatus for Group Referral ID: {1} has been updated to {2}. \n\nLink: {3} \n\nStatus Remarks: {4}";
                default:
                    return "";
            }
        }

        public void AddData(string newData)
        {
            DataList.Add(newData);
            Data = JsonConvert.SerializeObject(DataList);
        }

        public void AddData(List<string> newData)
        {
            DataList.AddRange(newData);
            Data = JsonConvert.SerializeObject(DataList);
        }

        public void ParseData()
        {
            DataList = string.IsNullOrEmpty(Data) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(Data);
        }

        public void MaskData(int index)
        {
            ParseData();
            if (index < DataList.Count())
            {
                DataList[index] = "**********";
                Data = JsonConvert.SerializeObject(DataList);
            }
        }

        public Mail GenerateMail()
        {
            ParseData();
            string body = string.Format(GetTemplate(Type), DataList.ToArray());

            Mail mail = new Mail()
            {
                Subject = GetSubject(Type),
                Body = body
            };
            mail.To.Add(EmailAddress);

            return mail;
        }
    }
}
