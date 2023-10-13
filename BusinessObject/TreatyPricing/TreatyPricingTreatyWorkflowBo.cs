using BusinessObject.Identity;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.IO;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingTreatyWorkflowBo : ObjectVersion
    {
        public int Id { get; set; }

        public int DocumentType { get; set; }
        public string DocumentTypeName { get; set; }

        public int ReinsuranceTypePickListDetailId { get; set; }
        public PickListDetailBo ReinsuranceTypePickListDetailBo { get; set; }
        public string ReinsuranceType { get; set; }

        public int CounterPartyDetailId { get; set; }
        public CedantBo CounterPartyDetailBo { get; set; }
        public string CounterParty { get; set; }

        public int? InwardRetroPartyDetailId { get; set; }
        public RetroPartyBo InwardRetroPartyDetailBo { get; set; }
        public string InwardRetroParty { get; set; }
        public string InwardRetroPartyName { get; set; }

        public int? BusinessOriginPickListDetailId { get; set; }
        public PickListDetailBo BusinessOriginPickListDetailBo { get; set; }
        public string BusinessOrigin { get; set; }

        public string TypeOfBusiness { get; set; }

        public string CountryOrigin { get; set; }

        public string DocumentId { get; set; }

        public string TreatyCode { get; set; }

        public int? CoverageStatus { get; set; }
        public string CoverageStatusName { get; set; }

        public int? DocumentStatus { get; set; }
        public string DocumentStatusName { get; set; }

        public int? DraftingStatus { get; set; }
        public string DraftingStatusName { get; set; }

        public int? DraftingStatusCategory { get; set; }
        public string DraftingStatusCategoryName { get; set; }

        public DateTime? EffectiveAt { get; set; }
        public string EffectiveAtStr { get; set; }

        public string OrionGroupStr { get; set; }

        public string Description { get; set; }

        public string SharepointLink { get; set; }

        public string Reviewer { get; set; }

        public UserBo PersonInChargeBo { get; set; }

        public int LatestVersion { get; set; }

        public IList<TreatyPricingTreatyWorkflowVersionBo> TreatyPricingTreatyWorkflowVersionBos { get; set; }
        public TreatyPricingTreatyWorkflowVersionBo LatestTreatyPricingTreatyWorkflowVersion { get; set; }

        public IList<TreatyPricingWorkflowObjectBo> TreatyPricingWorkflowObjectBos { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        // KPI Monitoring Report
        public string RequestDate { get; set; }
        public int Days1stDraftToReviewer { get; set; }
        public int Days1stDraftToCedant { get; set; }
        public int DaysSigned { get; set; }
        public double FollowUpFrequency { get; set; }
        public string DateSentToReviewer1st { get; set; }
        public string SignedDate { get; set; }
        public string LatestRevisionDate { get; set; }

        // Treaty Weekly/Monthly/Quarterly Report
        public string ReportingStatus { get; set; }
        public string DateSentToClient1st { get; set; }
        public string ReportedDateStr { get; set; }

        // Draft Status Overview 
        public int TotalCountInAddendumOM { get; set; }
        public int TotalSignedInTreatyWM { get; set; }
        public int TotalLessThan6MonthCountInTreatyWM { get; set; }
        public int TotalLessThan12MonthCountInTreatyWM { get; set; }
        public int TotalMoreThan12MonthCountInTreatyWM { get; set; }
        public int TotalSignedInAddendumWM { get; set; }
        public int TotalInTreatyWM { get; set; }
        public int TotalLessThan6MonthCountInAddendumWM { get; set; }
        public int TotalLessThan12MonthCountInAddendumWM { get; set; }
        public int TotalMoreThan12MonthCountInAddendumWM { get; set; }
        public int TotalInAddendumWM { get; set; }
        public int TotalSignedInTreatyOM { get; set; }
        public int TotalLessThan6MonthCountInTreatyOM { get; set; }
        public int TotalLessThan12MonthCountInTreatyOM { get; set; }
        public int TotalMoreThan12MonthCountInTreatyOM { get; set; }
        public int TotalInTreatyOM { get; set; }
        public int TotalSignedInAddendumOM { get; set; }
        public int TotalLessThan6MonthCountInAddendumOM { get; set; }
        public int TotalLessThan12MonthCountInAddendumOM { get; set; }
        public int TotalMoreThan12MonthCountInAddendumOM { get; set; }
        public int TotalInAddendumOM { get; set; }
        public int TotalSignedInTreatyWMandOM { get; set; }
        public int TotalLessThan6MonthCountInTreatyWMandOM { get; set; }
        public int TotalLessThan12MonthCountInTreatyWMandOM { get; set; }
        public int TotalMoreThan12MonthCountInTreatyWMandOM { get; set; }
        public int TotalInTreatyWMandOM { get; set; }
        public int TotalSignedInAddendumWMandOM { get; set; }
        public int TotalLessThan6MonthCountInAddendumWMandOM { get; set; }
        public int TotalLessThan12MonthCountInAddendumWMandOM { get; set; }
        public int TotalMoreThan12MonthCountInAddendumWMandOM { get; set; }
        public int TotalInAddendumWMandOM { get; set; }
        public int SignedCountInTreaty { get; set; }
        public int LessThan6MonthCountInTreaty { get; set; }
        public int LessThan12MonthCountInTreaty { get; set; }
        public int MoreThan12MonthCountInTreaty { get; set; }
        public int TotalCountInTreaty { get; set; }
        public int SignedCountInAddendum { get; set; }
        public int LessThan6MonthCountInAddendum { get; set; }
        public int LessThan12MonthCountInAddendum { get; set; }
        public int MoreThan12MonthCountInAddendum { get; set; }
        public int TotalCountInAddendum { get; set; }

        // Draft Overview by Drafting Status
        public int AllCount { get; set; }
        public int LessThan6MonthCount { get; set; }
        public int LessThan12MonthCount { get; set; }
        public int MoreThan12MonthCount { get; set; }
        public int TotalCountForAll { get; set; }

        // Draft Overview by PIC
        public string PersonInChargeName { get; set; }
        public int? PersonInChargeId { get; set; }
        public int LessThan6MonthCountInOther { get; set; }
        public int LessThan12MonthCountInOther { get; set; }
        public int MoreThan12MonthCountInOther { get; set; }
        public int TotalCountPic { get; set; }
        public int TotalLessThan6MonthsCountInTreaty { get; set; }
        public int TotalLessThan12MonthsCountInTreaty { get; set; }
        public int TotalMoreThan12MonthsCountInTreaty { get; set; }
        public int TotalLessThan6MonthsCountInAddendum { get; set; }
        public int TotalLessThan12MonthsCountInAddendum { get; set; }
        public int TotalMoreThan12MonthsCountInAddendum { get; set; }
        public int TotalLessThan6MonthsCountInOther { get; set; }
        public int TotalLessThan12MonthsCountInOther { get; set; }
        public int TotalMoreThan12MonthsCountInOther { get; set; }
        public int TotalCountForAllPic { get; set; }
        public int TotalLessThan6MonthsCount { get; set; }
        public int TotalLessThan12MonthsCount { get; set; }
        public int TotalMoreThan12MonthsCount { get; set; }

        // Draft Overview Pending Department
        public int DepartmentId { get; set; }
        public string Department { get; set; }

        public string TriggerDateStr { get; set; }

        // Draft Overview Within 2 Weeks
        public string TargetSentDateStr { get; set; }

        // Draft Calendar
        public string Title { get; set; }
        public string Start { get; set; }

        public IList<TreatyPricingTreatyWorkflowBo> TreatyPricingTreatyWorkflowBos { get; set; }

        public const int DocumentTypeTreaty = 1;
        public const int DocumentTypeAddendum = 2;
        public const int DocumentTypeMasterLetter = 3;
        public const int DocumentTypeCampaignSlip = 4;
        public const int DocumentTypeEmail = 5;
        public const int DocumentTypeAddendumGenRe = 6;
        public const int DocumentTypeCoverNote = 7;
        public const int DocumentTypeGeneralAgreement = 8;
        public const int DocumentTypeNovationAgreement = 9;
        public const int DocumentTypeServiceAgreement = 10;
        public const int DocumentTypeSlipContract = 11;
        public const int DocumentTypeLetter = 12;

        public const int ReinsuranceTypeLifeReinsurance = 1;
        public const int ReinsuranceTypeGeneralReinsurance = 2;
        public const int ReinsuranceTypeInwardRetro = 3;
        public const int ReinsuranceTypeRetakafulService = 4;
        public const int ReinsuranceTypeDirectRetro = 5;

        public const int CoverageStatusInForce = 1;
        public const int CoverageStatusRunOff = 2;
        public const int CoverageStatusTerminated = 3;

        public const int DocumentStatusDrafting = 1;
        public const int DocumentStatusSigned = 2;
        public const int DocumentStatusNotUsed = 3;
        public const int DocumentStatusUnassigned = 4;
        public const int DocumentStatusMax = 4;

        public const int DraftingStatusDrafting = 1;
        public const int DraftingStatusOnHold = 2;
        public const int DraftingStatusPendingTreatyPeerReview1st = 3;
        public const int DraftingStatusPendingProductPricingPICReview1st = 4;
        public const int DraftingStatusPendingHODReview = 5;
        public const int DraftingStatusPendingCAndRReview = 6;
        public const int DraftingStatusPendingRGAReview = 7;
        public const int DraftingStatusPendingUWReview = 8;
        public const int DraftingStatusPendingClaimReview = 9;
        public const int DraftingStatusPendingHealthReview = 10;
        public const int DraftingStatusEXTFirstDraftSent = 11;
        public const int DraftingStatusToReviewClientFeedback = 12;
        public const int DraftingStatusEXTRevisedDraftSent = 13;
        public const int DraftingStatusINTCountersigning = 14;
        public const int DraftingStatusEXTCountersigning = 15;
        public const int DraftingStatusPendingBDReview = 16;
        public const int DraftingStatusPendingGroupPricingPICReview1st = 17;
        public const int DraftingStatusTreatyPICComment = 18;
        public const int DraftingStatusProductPricingPICComment = 19;
        public const int DraftingStatusHODComment = 20;
        public const int DraftingStatusCAndRComment = 21;
        public const int DraftingStatusRGAComment = 22;
        public const int DraftingStatusUWComment = 23;
        public const int DraftingStatusClaimComment = 24;
        public const int DraftingStatusHealthComment = 25;
        public const int DraftingStatusBDComment = 26;
        public const int DraftingStatusTreatyPICOk = 27;
        public const int DraftingStatusPricingPICOk = 28;
        public const int DraftingStatusHODOk = 29;
        public const int DraftingStatusCAndROk = 30;
        public const int DraftingStatusRGAOk = 31;
        public const int DraftingStatusUWOk = 32;
        public const int DraftingStatusClaimOk = 33;
        public const int DraftingStatusHealthOk = 34;
        public const int DraftingStatusPendingTreatyPeerReviewRevised = 35;
        public const int DraftingStatusPendingProductPricingPICReviewRevised = 36;
        public const int DraftingStatusPendingGroupPricingPICReviewRevised = 37;
        public const int DraftingStatusSigned = 38;
        public const int DraftingStatusUnassigned = 39;
        public const int DraftingStatusCancelled = 40;

        public const int DraftingStatusCategoryDrafting = 1;
        public const int DraftingStatusCategoryOnHold = 2;
        public const int DraftingStatusCategoryInternalReview = 3;
        public const int DraftingStatusCategorySent = 4;
        public const int DraftingStatusCategoryRevisedDrafting = 5;
        public const int DraftingStatusCategoryCountersigning = 6;
        public const int DraftingStatusCategorySigned = 7;
        public const int DraftingStatusCategoryUnassigned = 8;
        public const int DraftingStatusCategoryCancelled = 9;

        public const int ReportingStatusDone = 1;
        public const int ReportingStatusDoing = 2;
        public const int ReportingStatusSigned = 3;

        public static string GetDocumentTypeName(int? key)
        {
            switch (key)
            {
                case DocumentTypeTreaty:
                    return "Treaty";
                case DocumentTypeAddendum:
                    return "Addendum";
                case DocumentTypeMasterLetter:
                    return "Master Letter";
                case DocumentTypeCampaignSlip:
                    return "Campaign Slip";
                case DocumentTypeEmail:
                    return "Email";
                case DocumentTypeAddendumGenRe:
                    return "Addendum GenRe";
                case DocumentTypeCoverNote:
                    return "Cover Note";
                case DocumentTypeGeneralAgreement:
                    return "General Agreement";
                case DocumentTypeNovationAgreement:
                    return "Novation Agreement";
                case DocumentTypeServiceAgreement:
                    return "Service Agreement";
                case DocumentTypeSlipContract:
                    return "Slip Contract";
                case DocumentTypeLetter:
                    return "Letter";
                default:
                    return "";
            }
        }

        public static string GetReinsuranceTypeName(int? key)
        {
            switch (key)
            {
                case ReinsuranceTypeLifeReinsurance:
                    return "Life Reinsurance";
                case ReinsuranceTypeGeneralReinsurance:
                    return "General Reinsurance";
                case ReinsuranceTypeInwardRetro:
                    return "Inward Retro";
                case ReinsuranceTypeRetakafulService:
                    return "Retakaful Service";
                case ReinsuranceTypeDirectRetro:
                    return "Direct Retro";
                default:
                    return "";
            }
        }

        public static string GetCoverageStatusName(int? key)
        {
            switch (key)
            {
                case CoverageStatusInForce:
                    return "In-Force";
                case CoverageStatusRunOff:
                    return "Run Off";
                case CoverageStatusTerminated:
                    return "Terminated";
                default:
                    return "";
            }
        }
        public static string GetCoverageStatusClass(int? key)
        {
            switch (key)
            {
                case CoverageStatusInForce:
                    return "status-pending-badge";
                case CoverageStatusRunOff:
                    return "status-processing-badge";
                case CoverageStatusTerminated:
                    return "status-finalize-badge";
                default:
                    return "";
            }
        }

        public static string GetDocumentStatusName(int? key)
        {
            switch (key)
            {
                case DocumentStatusDrafting:
                    return "Drafting";
                case DocumentStatusSigned:
                    return "Signed";
                case DocumentStatusNotUsed:
                    return "Not Used";
                case DocumentStatusUnassigned:
                    return "Unassigned";
                default:
                    return "";
            }
        }

        public static string GetDocumentStatusClass(int? key)
        {
            switch (key)
            {
                case DocumentStatusDrafting:
                    return "status-pending-badge";
                case DocumentStatusSigned:
                    return "status-finalize-badge";
                case DocumentStatusNotUsed:
                    return "status-pending-badge";
                case DocumentStatusUnassigned:
                    return "status-pending-badge";
                default:
                    return "";
            }
        }

        public static string GetDraftingStatusName(int? keyy)
        {
            switch (keyy)
            {
                case DraftingStatusDrafting:
                    return "Drafting";
                case DraftingStatusOnHold:
                    return "On Hold";
                case DraftingStatusPendingTreatyPeerReview1st:
                    return "Pending Treaty Peer review (1st)";
                case DraftingStatusPendingProductPricingPICReview1st:
                    return "Pending Product Pricing PIC review (1st)";
                case DraftingStatusPendingHODReview:
                    return "Pending HOD Review";
                case DraftingStatusPendingCAndRReview:
                    return "Pending C&R Review";
                case DraftingStatusPendingRGAReview:
                    return "Pending RGA Review";
                case DraftingStatusPendingUWReview:
                    return "Pending UW Review";
                case DraftingStatusPendingClaimReview:
                    return "Pending Claim Review";
                case DraftingStatusPendingHealthReview:
                    return "Pending Health Review";
                case DraftingStatusEXTFirstDraftSent:
                    return "[EXT] First draft sent";
                case DraftingStatusToReviewClientFeedback:
                    return "To review client feedback";
                case DraftingStatusEXTRevisedDraftSent:
                    return "[EXT] Revised draft sent";
                case DraftingStatusINTCountersigning:
                    return "[INT] Countersigning";
                case DraftingStatusEXTCountersigning:
                    return "[EXT] Countersigning";
                case DraftingStatusPendingBDReview:
                    return "Pending BD Review";
                case DraftingStatusPendingGroupPricingPICReview1st:
                    return "Pending Group Pricing PIC review (1st)";
                case DraftingStatusTreatyPICComment:
                    return "Treaty PIC comment";
                case DraftingStatusProductPricingPICComment:
                    return "Product Pricing PIC comment";
                case DraftingStatusHODComment:
                    return "HOD comment";
                case DraftingStatusCAndRComment:
                    return "C&R comment";
                case DraftingStatusRGAComment:
                    return "RGA comment";
                case DraftingStatusUWComment:
                    return "UW comment";
                case DraftingStatusClaimComment:
                    return "Claim comment";
                case DraftingStatusHealthComment:
                    return "Health comment";
                case DraftingStatusBDComment:
                    return "BD comment";
                case DraftingStatusTreatyPICOk:
                    return "Treaty PIC ok";
                case DraftingStatusPricingPICOk:
                    return "Pricing PIC ok";
                case DraftingStatusHODOk:
                    return "HOD ok";
                case DraftingStatusCAndROk:
                    return "C&R ok";
                case DraftingStatusRGAOk:
                    return "RGA ok";
                case DraftingStatusUWOk:
                    return "UW ok";
                case DraftingStatusClaimOk:
                    return "Claim ok";
                case DraftingStatusHealthOk:
                    return "Health ok";
                case DraftingStatusPendingTreatyPeerReviewRevised:
                    return "Pending Treaty Peer review (revised)";
                case DraftingStatusPendingProductPricingPICReviewRevised:
                    return "Pending Product Pricing PIC review (revised)";
                case DraftingStatusPendingGroupPricingPICReviewRevised:
                    return "Pending Group Pricing PIC review (revised)";
                case DraftingStatusSigned:
                    return "Signed";
                case DraftingStatusUnassigned:
                    return "Unassigned";
                case DraftingStatusCancelled:
                    return "Cancelled";
                default:
                    return "";
            }
        }

        public static string GetDraftingStatusClass(int? keyy)
        {
            switch (keyy)
            {
                case DraftingStatusDrafting:
                    return "status-processing-badge";
                case DraftingStatusOnHold:
                    return "status-processing-badge";
                case DraftingStatusPendingTreatyPeerReview1st:
                    return "status-pending-badge";
                case DraftingStatusPendingProductPricingPICReview1st:
                    return "status-pending-badge";
                case DraftingStatusPendingHODReview:
                    return "status-pending-badge";
                case DraftingStatusPendingCAndRReview:
                    return "status-pending-badge";
                case DraftingStatusPendingRGAReview:
                    return "status-pending-badge";
                case DraftingStatusPendingUWReview:
                    return "status-pending-badge";
                case DraftingStatusPendingClaimReview:
                    return "status-pending-badge";
                case DraftingStatusPendingHealthReview:
                    return "status-pending-badge";
                case DraftingStatusEXTFirstDraftSent:
                    return "status-processing-badge";
                case DraftingStatusToReviewClientFeedback:
                    return "status-pending-badge";
                case DraftingStatusEXTRevisedDraftSent:
                    return "status-processing-badge";
                case DraftingStatusINTCountersigning:
                    return "status-processing-badge";
                case DraftingStatusEXTCountersigning:
                    return "status-processing-badge";
                case DraftingStatusPendingBDReview:
                    return "status-pending-badge";
                case DraftingStatusPendingGroupPricingPICReview1st:
                    return "status-pending-badge";
                case DraftingStatusTreatyPICComment:
                    return "status-processing-badge";
                case DraftingStatusHODComment:
                    return "status-processing-badge";
                case DraftingStatusCAndRComment:
                    return "status-processing-badge";
                case DraftingStatusRGAComment:
                    return "status-processing-badge";
                case DraftingStatusUWComment:
                    return "status-processing-badge";
                case DraftingStatusClaimComment:
                    return "status-processing-badge";
                case DraftingStatusHealthComment:
                    return "status-processing-badge";
                case DraftingStatusBDComment:
                    return "status-processing-badge";
                case DraftingStatusTreatyPICOk:
                    return "status-success-badge";
                case DraftingStatusPricingPICOk:
                    return "status-success-badge";
                case DraftingStatusHODOk:
                    return "status-success-badge";
                case DraftingStatusCAndROk:
                    return "status-success-badge";
                case DraftingStatusRGAOk:
                    return "status-success-badge";
                case DraftingStatusUWOk:
                    return "status-success-badge";
                case DraftingStatusClaimOk:
                    return "status-success-badge";
                case DraftingStatusHealthOk:
                    return "status-success-badge";
                case DraftingStatusPendingTreatyPeerReviewRevised:
                    return "status-pending-badge";
                case DraftingStatusPendingProductPricingPICReviewRevised:
                    return "status-pending-badge";
                case DraftingStatusPendingGroupPricingPICReviewRevised:
                    return "status-finalize-badge";
                case DraftingStatusSigned:
                    return "status-success-badge";
                case DraftingStatusUnassigned:
                    return "status-pending-badge";
                case DraftingStatusCancelled:
                    return "status-success-badge";
                default:
                    return "";
            }
        }

        public static string GetDraftingStatusCategoryName(int? key)
        {
            switch (key)
            {
                case DraftingStatusCategoryDrafting:
                    return "Drafting";
                case DraftingStatusCategoryOnHold:
                    return "On Hold";
                case DraftingStatusCategoryInternalReview:
                    return "Internal Review";
                case DraftingStatusCategorySent:
                    return "Sent";
                case DraftingStatusCategoryRevisedDrafting:
                    return "Revised drafting";
                case DraftingStatusCategoryCountersigning:
                    return "Countersigning";
                case DraftingStatusCategorySigned:
                    return "Signed";
                case DraftingStatusCategoryUnassigned:
                    return "Unassigned";
                case DraftingStatusCategoryCancelled:
                    return "Cancelled";
                default:
                    return "";
            }
        }

        public static string GetDraftingStatusCategoryClass(int? key)
        {
            switch (key)
            {
                case DraftingStatusCategoryDrafting:
                    return "status-processing-badge";
                case DraftingStatusCategoryOnHold:
                    return "status-processing-badge";
                case DraftingStatusCategoryInternalReview:
                    return "status-processing-badge";
                case DraftingStatusCategorySent:
                    return "status-success-badge";
                case DraftingStatusCategoryRevisedDrafting:
                    return "status-pending-badge";
                case DraftingStatusCategoryCountersigning:
                    return "status-pending-badge";
                case DraftingStatusCategorySigned:
                    return "status-finalize-badge";
                case DraftingStatusCategoryUnassigned:
                    return "status-pending-badge";
                case DraftingStatusCategoryCancelled:
                    return "status-success-badge";
                default:
                    return "";
            }
        }

        public static string GetReportingStatusName(int? key)
        {
            switch (key)
            {
                case ReportingStatusDone:
                    return "Done";
                case ReportingStatusDoing:
                    return "Doing";
                case ReportingStatusSigned:
                    return "Signed";
                default:
                    return "";
            }
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
                    Header = "Reinsurance Type",
                    Property = "ReinsuranceTypePickListDetailId",
                },
                new Column()
                {
                    Header = "Counter Party",
                    Property = "CounterPartyDetailId",
                },
                new Column()
                {
                    Header = "Inward Retro Party",
                    Property = "InwardRetroPartyDetailId",
                },
                new Column()
                {
                    Header = "Document ID",
                    Property = "DocumentId",
                },
                new Column()
                {
                    Header = "Type of Business",
                    Property = "TypeOfBusiness",
                },
                new Column()
                {
                    Header = "Description",
                    Property = "Description",
                },
                new Column()
                {
                    Header = "Treaty Code",
                    Property = "TreatyCode",
                },
                new Column()
                {
                    Header = "Effective At",
                    Property = "EffectiveAtStr",
                },
                new Column()
                {
                    Header = "ORION Group",
                    Property = "OrionGroupStr",
                },
                new Column()
                {
                    Header = "Coverage Status",
                    Property = "CoverageStatus",
                },
                new Column()
                {
                    Header = "Document Status",
                    Property = "DocumentStatus",
                },
                new Column()
                {
                    Header = "Drafting Status",
                    Property = "DraftingStatus",
                },
                new Column()
                {
                    Header = "Country Origin",
                    Property = "CountryOrigin",
                },
                new Column()
                {
                    Header = "Drafting Status Category",
                    Property = "DraftingStatusCategory",
                },
                new Column()
                {
                    Header = "Sharepoint Link",
                    Property = "SharepointLink",
                }
            };
        }
    }
}
