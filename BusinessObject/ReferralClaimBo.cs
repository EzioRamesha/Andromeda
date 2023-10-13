using BusinessObject.Identity;
using BusinessObject.RiDatas;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ReferralClaimBo
    {
        public int Id { get; set; }

        public int? ClaimRegisterId { get; set; }
        public ClaimRegisterBo ClaimRegisterBo { get; set; }

        public string ClaimId { get; set; }

        public int? RiDataWarehouseId { get; set; }
        public RiDataWarehouseBo RiDataWarehouseBo { get; set; }

        public int? ReferralRiDataId { get; set; }
        //public ReferralRiDataBo ReferralRiDataBo { get; set; }

        public int Status { get; set; }
        public string StatusName { get; set; }

        public string ReferralId { get; set; }

        public string RecordType { get; set; }

        public string InsuredName { get; set; }

        public string PolicyNumber { get; set; }

        public string InsuredGenderCode { get; set; }

        public string InsuredTobaccoUsage { get; set; }

        public int? ReferralReasonId { get; set; }
        public ClaimReasonBo ReferralReasonBo { get; set; }

        public string GroupName { get; set; }

        public DateTime? DateReceivedFullDocuments { get; set; }

        public DateTime? InsuredDateOfBirth { get; set; }
        public string InsuredDateOfBirthStr { get; set; }

        public string InsuredIcNumber { get; set; }

        [DisplayName("Date Of Commencement")]
        public DateTime? DateOfCommencement { get; set; }
        public string DateOfCommencementStr { get; set; }

        public string CedingCompany { get; set; }

        [DisplayName("Claim Code")]
        public string ClaimCode { get; set; }

        [DisplayName("Ceding Plan Code")]
        public string CedingPlanCode { get; set; }

        public double? SumInsured { get; set; }

        public double? SumReinsured { get; set; }
        public string SumReinsuredStr { get; set; }

        public string SumInsStr { get; set; }

        public string BenefitSubCode { get; set; }

        [DisplayName("Date Of Event")]
        public DateTime? DateOfEvent { get; set; }
        public string DateOfEventStr { get; set; }

        [MaxLength(30)]
        public string RiskQuarter { get; set; }

        [DisplayName("Cause of Event")]
        public string CauseOfEvent { get; set; }

        [DisplayName("MLRe Benefit Code")]
        public string MlreBenefitCode { get; set; }

        [DisplayName("Claim Recovery Amount")]
        public double? ClaimRecoveryAmount { get; set; }
        public string ClaimRecoveryAmountStr { get; set; }

        public string ReinsBasisCode { get; set; }

        public int? ClaimCategoryId { get; set; }
        public ClaimCategoryBo ClaimCategoryBo { get; set; }

        public bool IsRgalRetakaful { get; set; }

        [DisplayName("Date Received")]
        public DateTime? ReceivedAt { get; set; }

        [DisplayName("Date Responded")]
        public DateTime? RespondedAt { get; set; }

        [DisplayName("Date Responded")]
        public DateTime? DocRespondedAt { get; set; }

        public long? TurnAroundTime { get; set; }
        public long? DocTurnAroundTime { get; set; }

        public int? DelayReasonId { get; set; }
        public ClaimReasonBo DelayReasonBo { get; set; }

        public int? DocDelayReasonId { get; set; }
        public ClaimReasonBo DocDelayReasonBo { get; set; }

        public bool IsRetro { get; set; }

        public string RetrocessionaireName { get; set; }

        public double? RetrocessionaireShare { get; set; }

        public int? RetroReferralReasonId { get; set; }
        public ClaimReasonBo RetroReferralReasonBo { get; set; }

        public int? MlreReferralReasonId { get; set; }
        public ClaimReasonBo MlreReferralReasonBo { get; set; }

        public string RetroReviewedBy { get; set; }

        public DateTime? RetroReviewedAt { get; set; }

        public bool IsValueAddedService { get; set; }

        public string ValueAddedServiceDetails { get; set; }

        public bool IsClaimCaseStudy { get; set; }

        public DateTime? CompletedCaseStudyMaterialAt { get; set; }

        [DisplayName("Assessed By")]
        public int? AssessedById { get; set; }
        public UserBo AssessedByBo { get; set; }

        public DateTime? AssessedAt { get; set; }

        public string AssessorComments { get; set; }

        public int? ReviewedById { get; set; }
        public UserBo ReviewedByBo { get; set; }

        public DateTime? ReviewedAt { get; set; }

        public string ReviewerComments { get; set; }

        public int? ClaimsDecision { get; set; }

        public DateTime? ClaimsDecisionDate { get; set; }

        public int? AssignedById { get; set; }
        public UserBo AssignedByBo { get; set; }

        public DateTime? AssignedAt { get; set; }

        public string TreatyCode { get; set; }

        [DisplayName("Treaty Type")]
        public string TreatyType { get; set; }

        public double? TreatyShare { get; set; }

        public string Checklist { get; set; }

        public string Error { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string RegisteredAtStr { get; set; }

        public int? PersonInChargeId { get; set; }

        public UserBo PersonInChargeBo { get; set; }

        // Dashboard
        public int NoOfCase { get; set; }

        public int OverdueCase { get; set; }

        public int MaxDay { get; set; }

        public int UnassignedCase { get; set; }

        public double? TotalRetroAmount { get; set; }

        public string TotalRetroAmountStr { get; set; }

        public string FollowUpWith { get; set; }

        public string FollowUpDateStr { get; set; }

        public string Remark { get; set; }

        public const int StatusNewCase = 1;
        public const int StatusPendingAssessment = 2;
        public const int StatusPendingClarification = 3;
        public const int StatusPendingChecklist = 4;
        public const int StatusClosed = 5;
        public const int StatusClosedRegistered = 6;
        public const int StatusMax = 6;

        public const int FilterTat1Day = 1;
        public const int FilterTat2Day = 2;
        public const int FilterTatMoreThan2Day = 3;
        public const int FilterTatMax = 3;

        public const int ClaimsDecisionApproved = 1;
        public const int ClaimsDecisionPending = 2;
        public const int ClaimsDecisionDeclined = 3;
        public const int ClaimsDecisionMax = 3;

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusNewCase:
                    return "New Case";
                case StatusPendingAssessment:
                    return "Pending Assessment";
                case StatusPendingClarification:
                    return "Pending Clarification - Client";
                case StatusPendingChecklist:
                    return "Pending Checklist";
                case StatusClosed:
                    return "Closed";
                case StatusClosedRegistered:
                    return "Closed - Registered";
                default:
                    return "";
            }
        }

        public static string GetFilterTatName(int key)
        {
            switch (key)
            {
                case FilterTat1Day:
                    return "1 Day";
                case FilterTat2Day:
                    return "2 Day";
                case FilterTatMoreThan2Day:
                    return "> 2 Days";
                default:
                    return "";
            }
        }

        public static long GetFilterTatTicks(int key)
        {
            switch (key)
            {
                case FilterTat1Day:
                    return (new TimeSpan(1, 0, 0, 0)).Ticks;
                case FilterTat2Day:
                case FilterTatMoreThan2Day:
                    return (new TimeSpan(2, 0, 0, 0)).Ticks;
                default:
                    return 0;
            }
        }

        public static string GetStatusClass(int key)
        {
            switch (key)
            {
                case StatusNewCase:
                    return "status-success-badge";
                case StatusPendingAssessment:
                    return "status-processing-badge";
                case StatusPendingClarification:
                    return "status-pending-badge";
                case StatusPendingChecklist:
                    return "status-pending-badge";
                case StatusClosed:
                    return "status-fail-badge";
                case StatusClosedRegistered:
                    return "status-success-badge";
                default:
                    return "";
            }
        }

        public static List<int> GetOperationalDashboardDisplayStatus()
        {
            var list = new List<int>
            {
                StatusNewCase,
                StatusPendingAssessment,
                StatusPendingClarification,
                StatusPendingChecklist,
                StatusClosed,
                StatusClosedRegistered,
            };

            return list;
        }

        public static List<string> GetRequiredFields(int status)
        {
            List<string> requiredFields = new List<string>()
            {
                "InsuredName",
                "CedingCompany",
                "PolicyNumber",
                "ReceivedAt",
                "RespondedAt",
                "ClaimCode",
                "PersonInChargeId"
            };

            if (status >= StatusClosed)
            {
                requiredFields.Add("InsuredDateOfBirthStr");
                requiredFields.Add("InsuredGenderCode");
                requiredFields.Add("DateOfCommencementStr");
                requiredFields.Add("DateOfEventStr");
                requiredFields.Add("CauseOfEvent");
                requiredFields.Add("SumInsuredStr");
                requiredFields.Add("RecordType");
                requiredFields.Add("AssessedById");
            }

            if (status == StatusClosedRegistered)
            {
                requiredFields.Add("SumReinsuredStr");
                requiredFields.Add("CedingPlanCode");
                requiredFields.Add("TreatyShareStr");
                requiredFields.Add("TreatyCode");
                requiredFields.Add("TreatyType");
                requiredFields.Add("MlreBenefitCode");
                requiredFields.Add("ClaimsDecision");
                requiredFields.Add("ClaimsDecisionDateStr");
            }

            return requiredFields;
        }

        public static int GetStatusSlaDay(int key)
        {
            switch (key)
            {
                case StatusNewCase:
                    return Util.GetConfigInteger("RcStatusNewCase", 2);
                case StatusPendingAssessment:
                    return Util.GetConfigInteger("RcStatusPendingAssessment", 2);
                case StatusPendingClarification:
                    return Util.GetConfigInteger("RcStatusPendingClarification", 2);
                case StatusPendingChecklist:
                    return Util.GetConfigInteger("RcStatusPendingChecklist", 2);
                case StatusClosed:
                    return Util.GetConfigInteger("RcStatusClosed", 2);
                case StatusClosedRegistered:
                    return Util.GetConfigInteger("RcStatusClosedRegistered", 2);
                default:
                    return 0;
            }
        }

        public static string GetClaimsDecisionName(int key)
        {
            switch (key)
            {
                case ClaimsDecisionApproved:
                    return "Approved";
                case ClaimsDecisionPending:
                    return "Pending";
                case ClaimsDecisionDeclined:
                    return "Declined";
                default:
                    return "";
            }
        }

        public static string GetClaimsDecisionClass(int key)
        {
            switch (key)
            {
                case ClaimsDecisionApproved:
                    return "status-success-badge";
                case ClaimsDecisionPending:
                    return "status-pending-badge";
                case ClaimsDecisionDeclined:
                    return "status-fail-badge";
                default:
                    return "";
            }
        }

        public ReferralClaimBo()
        {
            Status = StatusNewCase;
        }

        public static List<Column> GetColumns()
        {
            return new List<Column>
            {
                new Column()
                {
                    Header = "ID",
                    Property = "Id",
                },
                new Column()
                {
                    Header = "Status",
                    Property = "Status",
                },
                new Column()
                {
                    Header = "Referral ID",
                    Property = "ReferralId",
                },
                new Column()
                {
                    Header = "Record Type",
                    Property = "RecordType",
                },
                new Column()
                {
                    Header = "Insured Name",
                    Property = "InsuredName",
                },
                new Column()
                {
                    Header = "Policy No",
                    Property = "PolicyNumber",
                },
                new Column()
                {
                    Header = "Insured Gender Code",
                    Property = "InsuredGenderCode",
                },
                new Column()
                {
                    Header = "Insured Tobacco Usage",
                    Property = "InsuredTobaccoUsage",
                },
                new Column()
                {
                    Header = "Referral Reason",
                    Property = "ReferralReasonId",
                },
                new Column()
                {
                    Header = "Group Name",
                    Property = "GroupName",
                },
                new Column()
                {
                    Header = "Date Received Full Documents",
                    Property = "DateReceivedFullDocuments",
                },
                new Column()
                {
                    Header = "Insured Date Of Birth",
                    Property = "InsuredDateOfBirth",
                },
                new Column()
                {
                    Header = "Insured IC No",
                    Property = "InsuredIcNumber",
                },
                new Column()
                {
                    Header = "Date Of Commencement",
                    Property = "DateOfCommencement",
                },
                new Column()
                {
                    Header = "Ceding Company",
                    Property = "CedingCompany",
                },
                new Column()
                {
                    Header = "Claim Code",
                    Property = "ClaimCode",
                },
                new Column()
                {
                    Header = "Ceding Plan Code",
                    Property = "CedingPlanCode",
                },
                new Column()
                {
                    Header = "Sum Insured",
                    Property = "SumInsured",
                },
                new Column()
                {
                    Header = "AAR Payable",
                    Property = "SumReinsured",
                },
                new Column()
                {
                    Header = "Benefit SubCode",
                    Property = "BenefitSubCode",
                },
                new Column()
                {
                    Header = "Date Of Event",
                    Property = "DateOfEvent",
                },
                new Column()
                {
                    Header = "Risk Quarter",
                    Property = "RiskQuarter",
                },
                new Column()
                {
                    Header = "Cause Of Event",
                    Property = "CauseOfEvent",
                },
                new Column()
                {
                    Header = "MLRe Benefit Code",
                    Property = "MlreBenefitCode",
                },
                new Column()
                {
                    Header = "Claim Recovery Amount",
                    Property = "ClaimRecoveryAmount",
                },
                new Column()
                {
                    Header = "Reins Basis Code",
                    Property = "ReinsBasisCode",
                },
                new Column()
                {
                    Header = "Claim Category",
                    Property = "ClaimCategoryId",
                },
                new Column()
                {
                    Header = "Rgal Retakaful",
                    Property = "IsRgalRetakaful",
                },
                new Column()
                {
                    Header = "Date Received",
                    Property = "ReceivedAt",
                },
                new Column()
                {
                    Header = "1st Date Responded",
                    Property = "RespondedAt",
                },
                new Column()
                {
                    Header = "2nd Date Responded",
                    Property = "DocRespondedAt",
                },
                new Column()
                {
                    Header = "1st Turn Around Time",
                    Property = "TurnAroundTime",
                },
                new Column()
                {
                    Header = "2nd Turn Around Time",
                    Property = "DocTurnAroundTime",
                },
                new Column()
                {
                    Header = "1st Delay Reason",
                    Property = "DelayReasonId",
                },
                new Column()
                {
                    Header = "2nd Delay Reason",
                    Property = "DocDelayReasonId",
                },
                new Column()
                {
                    Header = "Is Retro",
                    Property = "IsRetro",
                },
                new Column()
                {
                    Header = "Retrocessionaire Name",
                    Property = "RetrocessionaireName",
                },
                new Column()
                {
                    Header = "Retrocessionaire Share",
                    Property = "RetrocessionaireShare",
                },
                new Column()
                {
                    Header = "Retro Referral Reason",
                    Property = "RetroReferralReasonId",
                },
                new Column()
                {
                    Header = "MLRe Referral Reason",
                    Property = "MlreReferralReasonId",
                },
                new Column()
                {
                    Header = "Retro Reviewed By",
                    Property = "RetroReviewedBy",
                },
                new Column()
                {
                    Header = "Retro Reviewed At",
                    Property = "RetroReviewedAt",
                },
                new Column()
                {
                    Header = "Is Value Added Service",
                    Property = "IsValueAddedService",
                },
                new Column()
                {
                    Header = "Value Added Service Details",
                    Property = "ValueAddedServiceDetails",
                },
                new Column()
                {
                    Header = "Is Claim Case Study",
                    Property = "IsClaimCaseStudy",
                },
                new Column()
                {
                    Header = "Completed Case Study Material At",
                    Property = "CompletedCaseStudyMaterialAt",
                },
                new Column()
                {
                    Header = "Assessed By",
                    Property = "AssessedById",
                },
                new Column()
                {
                    Header = "Assessed At",
                    Property = "AssessedAt",
                },
                new Column()
                {
                    Header = "Assessor Comments",
                    Property = "AssessorComments",
                },
                new Column()
                {
                    Header = "Reviewed By",
                    Property = "ReviewedById",
                },
                new Column()
                {
                    Header = "Reviewed At",
                    Property = "ReviewedAt",
                },
                new Column()
                {
                    Header = "Reviewer Comments",
                    Property = "ReviewerComments",
                },
                new Column()
                {
                    Header = "Claims Decision",
                    Property = "ClaimsDecision",
                },
                new Column()
                {
                    Header = "Claims Decision Date",
                    Property = "ClaimsDecisionDate",
                },
                new Column()
                {
                    Header = "Assigned By",
                    Property = "AssignedById",
                },
                new Column()
                {
                    Header = "Assigned At",
                    Property = "AssignedAt",
                },
                new Column()
                {
                    Header = "Treaty Code",
                    Property = "TreatyCode",
                },
                new Column()
                {
                    Header = "Treaty Type",
                    Property = "TreatyType",
                },
                new Column()
                {
                    Header = "MLRe Share",
                    Property = "TreatyShare",
                },
                new Column()
                {
                    Header = "Person In-Charge",
                    Property = "PersonInChargeId",
                },
            };
        }
    }
}
