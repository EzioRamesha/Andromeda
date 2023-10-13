using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingQuotationWorkflowBo : ObjectVersion
    {
        public int Id { get; set; }

        public int CedantId { get; set; }

        public CedantBo CedantBo { get; set; }

        public string QuotationId { get; set; }

        public int? ReinsuranceTypePickListDetailId { get; set; }

        public PickListDetailBo ReinsuranceTypePickListDetailBo { get; set; }

        public string Name { get; set; }

        public string Summary { get; set; }

        public int Status { get; set; }

        public string StatusRemarks { get; set; }

        public DateTime? TargetSendDate { get; set; }

        public DateTime? LatestRevisionDate { get; set; }

        public int? PricingTeamPickListDetailId { get; set; }

        public PickListDetailBo PricingTeamPickListDetailBo { get; set; }

        public int? PricingStatus { get; set; }

        public DateTime? TargetClientReleaseDate { get; set; }

        public DateTime? TargetRateCompletionDate { get; set; }

        public DateTime? FinaliseDate { get; set; }

        public string Description { get; set; }

        public int LatestVersion { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public IList<TreatyPricingQuotationWorkflowVersionBo> TreatyPricingQuotationWorkflowVersionBos { get; set; }

        public IList<TreatyPricingWorkflowObjectBo> TreatyPricingWorkflowObjectBos { get; set; }

        public string StatusName { get; set; }

        public string PricingStatusName { get; set; }

        public string TargetSendDateStr { get; set; }

        public string LatestRevisionDateStr { get; set; }

        public string TargetClientReleaseDateStr { get; set; }

        public string TargetRateCompletionDateStr { get; set; }

        public string FinaliseDateStr { get; set; }

        public string ReinsuranceType { get; set; }

        public string PricingTeam { get; set; }

        public string CedantCode { get; set; }

        #region Properties - Quotation and Pricing dashboard
        public int NoOfCase { get; set; }

        public int? DueDateOverviewType { get; set; }

        public int? BDPersonInChargeId { get; set; }

        public int? PersonInChargeId { get; set; }

        public int CEOPending { get; set; }

        public int PricingPending { get; set; }

        public int UnderwritingPending { get; set; }

        public int HealthPending { get; set; }

        public int ClaimsPending { get; set; }

        public int BDPending { get; set; }

        public int TGPending { get; set; }
        public string TriggerDateStr { get; set; }

        public DateTime? PricingDueDate { get; set; }
        #endregion

        public const int StatusQuoting = 1;
        public const int StatusQuoted = 2;
        public const int StatusWon = 3;
        public const int StatusWonExisting = 4;
        public const int StatusNotTakenUp = 5;
        public const int StatusLost = 6;
        public const int StatusPostponed = 7;
        public const int StatusMax = 7;

        public const int PricingStatusUnassigned = 1;
        public const int PricingStatusAssessmentInProgress = 2;
        public const int PricingStatusPendingTechReview = 3;
        public const int PricingStatusPendingPeerReview = 4;
        public const int PricingStatusPendingPricingAuthorityReview = 5;
        public const int PricingStatusToUpdateRepo = 6;
        public const int PricingStatusUpdatedRepo = 7;
        public const int PricingStatusMax = 7;

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusQuoting:
                    return "Quoting";
                case StatusQuoted:
                    return "Quoted";
                case StatusWon:
                    return "Won";
                case StatusWonExisting:
                    return "Won & Existing";
                case StatusNotTakenUp:
                    return "Not Taken Up";
                case StatusLost:
                    return "Lost";
                case StatusPostponed:
                    return "Postponed";
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
                case StatusQuoted:
                    return "status-processing-badge";
                case StatusWon:
                    return "status-success-badge";
                case StatusWonExisting:
                    return "status-success-badge";
                case StatusNotTakenUp:
                    return "status-processing-badge";
                case StatusLost:
                    return "status-fail-badge";
                case StatusPostponed:
                    return "status-processing-badge";
                default:
                    return "";
            }
        }

        public static string GetPricingStatusName(int? key)
        {
            switch (key)
            {
                case PricingStatusUnassigned:
                    return "Unassigned";
                case PricingStatusAssessmentInProgress:
                    return "Assessment in Progress";
                case PricingStatusPendingTechReview:
                    return "Pending Tech Review";
                case PricingStatusPendingPeerReview:
                    return "Pending Peer Review";
                case PricingStatusPendingPricingAuthorityReview:
                    return "Pending Pricing Authority Review";
                case PricingStatusToUpdateRepo:
                    return "To Update Repo";
                case PricingStatusUpdatedRepo:
                    return "Updated Repository";
                default:
                    return "";
            }
        }

        public static string GetPricingStatusClass(int? key)
        {
            return "status-pending-badge";
        }

        public static List<Column> GetColumns()
        {
            return new List<Column>
            {
                new Column()
                {
                    Header = "Quotation ID",
                    Property = "QuotationId",
                },
                new Column()
                {
                    Header = "Created At",
                    Property = "CreatedAt",
                },
                new Column()
                {
                    Header = "Ceding Company",
                    Property = "CedantId",
                },
                new Column()
                {
                    Header = "Reinsurance Type",
                    Property = "ReinsuranceTypePickListDetailId",
                },
                new Column()
                {
                    Header = "Quotation Name",
                    Property = "Name",
                },
                new Column()
                {
                    Header = "Description",
                    Property = "Description",
                },
                new Column()
                {
                    Header = "Latest Version",
                    Property = "LatestVersion",
                },
                new Column()
                {
                    Header = "Status",
                    Property = "Status",
                },
                new Column()
                {
                    Header = "Pricing Status",
                    Property = "PricingStatus",
                },
            };
        }

        public static string GetPendingOn(int? key, TreatyPricingQuotationWorkflowVersionBo bo = null)
        {
            switch (key)
            {
                case PricingStatusUnassigned:
                    //return "Pricing Senior Manager/HOD";
                    if (bo != null && bo.PersonInChargeId.HasValue)
                    {
                        return bo.PersonInChargeName;
                    }
                    return "Nil";
                case PricingStatusAssessmentInProgress:
                    if (bo != null && bo.PersonInChargeId.HasValue)
                    {
                        return bo.PersonInChargeName;
                    }
                    return "Nil";
                case PricingStatusPendingTechReview:
                    if (bo != null && bo.PersonInChargeTechReviewerId.HasValue)
                    {
                        return bo.PersonInChargeTechReviewerName;
                    }
                    return "Nil";
                case PricingStatusPendingPeerReview:
                    if (bo != null && bo.PersonInChargePeerReviewerId.HasValue)
                    {
                        return bo.PersonInChargePeerReviewerName;
                    }
                    return "Nil";
                case PricingStatusPendingPricingAuthorityReview:
                    if (bo != null && bo.PersonInChargePricingAuthorityReviewerId.HasValue)
                    {
                        return bo.PersonInChargePricingAuthorityReviewerName;
                    }
                    return "Nil";
                case PricingStatusToUpdateRepo:
                    if (bo != null && bo.PersonInChargeId.HasValue)
                    {
                        return bo.PersonInChargeName;
                    }
                    return "Nil";
                case PricingStatusUpdatedRepo:
                    return "Nil";
                default:
                    return "";
            }
        }
    }
}
