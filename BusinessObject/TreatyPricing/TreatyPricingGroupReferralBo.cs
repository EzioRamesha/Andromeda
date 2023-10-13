using BusinessObject.Identity;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingGroupReferralBo : ObjectVersion
    {
        public int Id { get; set; }

        public int CedantId { get; set; }
        public CedantBo CedantBo { get; set; }
        public string CedantName { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public int? RiArrangementPickListDetailId { get; set; }
        public PickListDetailBo RiArrangementPickListDetailBo { get; set; }

        public int InsuredGroupNameId { get; set; }
        public InsuredGroupNameBo InsuredGroupNameBo { get; set; }
        public string InsuredGroupNameName { get; set; }

        public int Status { get; set; }
        public int? WorkflowStatus { get; set; }

        public int PrimaryTreatyPricingProductId { get; set; }
        public TreatyPricingProductBo PrimaryTreatyPricingProductBo { get; set; }
        public int PrimaryTreatyPricingProductVersionId { get; set; }
        public TreatyPricingProductVersionBo PrimaryTreatyPricingProductVersionBo { get; set; }
        public string PrimaryTreatyPricingProductSelect { get; set; }

        public int? SecondaryTreatyPricingProductId { get; set; }
        public virtual TreatyPricingProductBo SecondaryTreatyPricingProductBo { get; set; }
        public int? SecondaryTreatyPricingProductVersionId { get; set; }
        public virtual TreatyPricingProductVersionBo SecondaryTreatyPricingProductVersionBo { get; set; }
        public string SecondaryTreatyPricingProductSelect { get; set; }

        public DateTime? FirstReferralDate { get; set; }

        public DateTime? CoverageStartDate { get; set; }

        public DateTime? CoverageEndDate { get; set; }

        public int? IndustryNamePickListDetailId { get; set; }
        public PickListDetailBo IndustryNamePickListDetailBo { get; set; }

        public int? ReferredTypePickListDetailId { get; set; }
        public PickListDetailBo ReferredTypePickListDetailBo { get; set; }

        public string PolicyNumber { get; set; }

        public string WonVersion { get; set; }

        public bool HasRiGroupSlip { get; set; }

        public string RiGroupSlipCode { get; set; }

        public int? RiGroupSlipStatus { get; set; }

        public int? RiGroupSlipPersonInChargeId { get; set; }
        public UserBo RiGroupSlipPersonInChargeBo { get; set; }

        public DateTime? RiGroupSlipConfirmationDate { get; set; }

        public int? RiGroupSlipVersionId { get; set; }
        public TreatyPricingGroupReferralVersionBo RiGroupSlipVersion { get; set; }

        public int? RiGroupSlipTemplateId { get; set; }
        public virtual TemplateBo RiGroupSlipTemplateBo { get; set; }

        public string RiGroupSlipSharePointLink { get; set; }

        public string RiGroupSlipSharePointFolderPath { get; set; }

        public string QuotationPath { get; set; }

        public int? ReplyVersionId { get; set; }
        public TreatyPricingGroupReferralVersionBo ReplyVersion { get; set; }

        public int? ReplyTemplateId { get; set; }
        public virtual TemplateBo ReplyTemplateBo { get; set; }

        public string ReplySharePointLink { get; set; }

        public string ReplySharePointFolderPath { get; set; }

        public int? TreatyPricingGroupMasterLetterId { get; set; }
        public TreatyPricingGroupMasterLetterBo TreatyPricingGroupMasterLetterBo { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string FirstReferralDateStr { get; set; }
        public string CoverageStartDateStr { get; set; }
        public string CoverageEndDateStr { get; set; }
        public string RiGroupSlipConfirmationDateStr { get; set; }

        public IList<TreatyPricingGroupReferralVersionBo> TreatyPricingGroupReferralVersionBos { get; set; }
        public IList<TreatyPricingGroupReferralHipsTableBo> TreatyPricingGroupReferralHipsTableBos { get; set; }
        public List<TreatyPricingGroupReferralVersionBenefitBo> TreatyPricingGroupReferralVersionBenefitBos { get; set; }
        public TreatyPricingGroupReferralVersionBo LatestTreatyPricingGroupReferralVersionBo { get; set; }


        // Group Overall TAT Report display
        public int NoOfDays0Cedant { get; set; }
        public int NoOfDays1Cedant { get; set; }
        public int NoOfDays2Cedant { get; set; }
        public int NoOfDays3Cedant { get; set; }
        public int NoOfDays4Cedant { get; set; }
        public int NoOfDays0Internal { get; set; }
        public int NoOfDays1Internal { get; set; }
        public int NoOfDays2Internal { get; set; }
        public int NoOfDays3Internal { get; set; }
        public int NoOfDays4Internal { get; set; }

        // Dashboard
        public int UnassignedTotal { get; set; }
        public int UnassignedCedant { get; set; }
        public int UnassignedInternal { get; set; }
        public int NoOfCasesByPic { get; set; }
        public int NoOfActiveCasesByPic { get; set; }
        public int ActiveCaseWithNoOfDays4 { get; set; }
        public double? AverageScore { get; set; }
        public List<string> PendingItems { get; set; }
        public string PersonInChargeName { get; set; }
        public int TotalNoOfCasesByPic { get; set; }
        public int TotalNoOfActiveCasesbyPic { get; set; }
        public int TotalActiveCaseWithNoOfDays4 { get; set; }
        public string DepartmentName { get; set; }
        public string TriggerDateStr { get; set; }
        public int PersonInChargeId { get; set; }
        public int DepartmentId { get; set; }
        public int Version { get; set; }

        // Upload
        public string CommissionMarginDEA { get; set; }
        public string CommissionMarginMSE { get; set; }
        public string ExpenseMarginDEA { get; set; }
        public string ExpenseMarginMSE { get; set; }
        public string ProfitMarginDEA { get; set; }
        public string ProfitMarginMSE { get; set; }


        public const int StatusQuoting = 1;
        public const int StatusWon = 2;
        public const int StatusLoss = 3;
        public const int StatusQuotationSent = 4;
        public const int StatusArchived = 5;
        public const int StatusMax = 5;

        public const string StatusQuotingName = "Quoting";
        public const string StatusWonName = "Won";
        public const string StatusLossName = "Loss";
        public const string StatusQuotationSentName = "Quotation Sent";
        public const string StatusArchivedName = "Archived";

        public const int WorkflowStatusQuoting = 1;
        public const int WorkflowStatusPendingClient = 2;
        public const int WorkflowStatusPendingGroupTeamsApproval = 3;
        public const int WorkflowStatusGroupTeamApprovedRejected = 4;
        public const int WorkflowStatusPendingReviewersApproval = 5;
        public const int WorkflowStatusReviewerApprovedRejected = 6;
        public const int WorkflowStatusPendingHODsApproval = 7;
        public const int WorkflowStatusHODApprovedRejected = 8;
        public const int WorkflowStatusPendingCEOsApproval = 9;
        public const int WorkflowStatusCEOApprovedRejected = 10;
        public const int WorkflowStatusQuotationSent = 11;
        public const int WorkflowStatusPrepareRIGroupSlip = 12;
        public const int WorkflowStatusCompleted = 13;
        public const int WorkflowStatusMax = 13;

        public const int RiGroupSlipStatusDraft = 1;
        public const int RiGroupSlipStatusPendingClient = 2;
        public const int RiGroupSlipStatusPendingReview = 3;
        public const int RiGroupSlipStatusCompleted = 4;
        public const int RiGroupSlipStatusMax = 4;

        public const int CoverageGtl = 1;
        public const int CoverageGhs = 2;
        public const int CoverageMax = 2;

        // Active Tab
        public const int ActiveTabList = 1;
        public const int ActiveTabUpload = 2;

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusQuoting:
                    return "Quoting";
                case StatusWon:
                    return "Won";
                case StatusLoss:
                    return "Loss";
                case StatusQuotationSent:
                    return "Quotation Sent";
                case StatusArchived:
                    return "Archived";
                default:
                    return "";
            }
        }

        public static string GetStatusClass(int key)
        {
            switch (key)
            {
                case StatusQuoting:
                    return "status-pending-badge";
                case StatusWon:
                    return "status-success-badge";
                case StatusLoss:
                    return "status-fail-badge";
                case StatusQuotationSent:
                    return "status-processing-badge";
                case StatusArchived:
                    return "status-finalize-badge";
                default:
                    return "";
            }
        }

        public static int GetStatusKey(string name)
        {
            switch (name)
            {
                case StatusQuotingName:
                    return StatusQuoting;
                case StatusWonName:
                    return StatusWon;
                case StatusLossName:
                    return StatusLoss;
                case StatusQuotationSentName:
                    return StatusQuotationSent;
                case StatusArchivedName:
                    return StatusArchived;
                default:
                    return 0;
            }
        }

        public static string GetWorkflowStatusName(int? key)
        {
            switch (key)
            {
                case WorkflowStatusQuoting:
                    return "Quoting";
                case WorkflowStatusPendingClient:
                    return "Pending Client";
                case WorkflowStatusPendingGroupTeamsApproval:
                    return "Pending Group Team's Approval";
                case WorkflowStatusGroupTeamApprovedRejected:
                    return "Group Team Approved / Rejected";
                case WorkflowStatusPendingReviewersApproval:
                    return "Pending Reviewer's Approval";
                case WorkflowStatusReviewerApprovedRejected:
                    return "Reviewer Approved / Rejected";
                case WorkflowStatusPendingHODsApproval:
                    return "Pending HOD's Approval";
                case WorkflowStatusHODApprovedRejected:
                    return "HOD Approved / Rejected";
                case WorkflowStatusPendingCEOsApproval:
                    return "Pending CEO's Approval";
                case WorkflowStatusCEOApprovedRejected:
                    return "CEO Approved / Rejected";
                case WorkflowStatusQuotationSent:
                    return "Quotation Sent";
                case WorkflowStatusPrepareRIGroupSlip:
                    return "Prepare RI Group Slip";
                case WorkflowStatusCompleted:
                    return "Completed";
                default:
                    return "";
            }
        }

        public static string GetWorkflowStatusClass(int? key)
        {
            switch (key)
            {
                case WorkflowStatusQuoting:
                    return "status-pending-badge";
                case WorkflowStatusPendingClient:
                case WorkflowStatusPendingGroupTeamsApproval:
                case WorkflowStatusPendingReviewersApproval:
                case WorkflowStatusPendingHODsApproval:
                case WorkflowStatusPendingCEOsApproval:
                case WorkflowStatusQuotationSent:
                    return "status-submitprocess-badge";
                case WorkflowStatusGroupTeamApprovedRejected:
                case WorkflowStatusReviewerApprovedRejected:
                case WorkflowStatusHODApprovedRejected:
                case WorkflowStatusCEOApprovedRejected:
                    return "status-success-badge";
                case WorkflowStatusPrepareRIGroupSlip:
                    return "status-processing-badge";
                case WorkflowStatusCompleted:
                    return "status-finalize-badge";
                default:
                    return "";
            }
        }

        public static string GetRiGroupSlipStatusName(int? key)
        {
            switch (key)
            {
                case RiGroupSlipStatusDraft:
                    return "Drafting";
                case RiGroupSlipStatusPendingClient:
                    return "Pending Client";
                case RiGroupSlipStatusPendingReview:
                    return "Pending Review";
                case RiGroupSlipStatusCompleted:
                    return "Complete";
                default:
                    return "";
            }
        }

        public static string GetRiGroupSlipStatusClass(int? key)
        {
            switch (key)
            {
                case RiGroupSlipStatusDraft:
                    return "status-pending-badge";
                case RiGroupSlipStatusPendingClient:
                    return "status-submitprocess-badge";
                case RiGroupSlipStatusPendingReview:
                    return "status-finalising-badge";
                case RiGroupSlipStatusCompleted:
                    return "status-finalize-badge";
                default:
                    return "";
            }
        }

        public static string GetCoverageName(int key)
        {
            switch (key)
            {
                case CoverageGtl:
                    return "GTL";
                case CoverageGhs:
                    return "GHS";
                default:
                    return "";
            }
        }

        public void SetSelectValues()
        {
            SetSelectValue("PrimaryTreatyPricingProduct");
            SetSelectValue("SecondaryTreatyPricingProduct");
        }

        public void SetSelectValue(string property)
        {
            string objectProperty = property + "Id";
            string versionProperty = property + "VersionId";
            string selectProperty = property + "Select";

            this.SetPropertyValue(objectProperty, null);
            this.SetPropertyValue(versionProperty, null);

            object value = this.GetPropertyValue(selectProperty);
            if (value == null)
                return;

            string[] values = value.ToString().Split('|');
            if (values.Length == 2)
            {
                int? versionId = Util.GetParseInt(values[0]);
                int? objectId = Util.GetParseInt(values[1]);

                if (versionId.HasValue && objectId.HasValue)
                {
                    this.SetPropertyValue(objectProperty, objectId);
                    this.SetPropertyValue(versionProperty, versionId);
                }
            }
        }
    }
}
