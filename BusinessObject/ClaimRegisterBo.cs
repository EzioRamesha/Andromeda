using BusinessObject.Claims;
using BusinessObject.Identity;
using BusinessObject.RiDatas;
using BusinessObject.SoaDatas;
using Newtonsoft.Json;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;

namespace BusinessObject
{
    public class ClaimRegisterBo
    {
        public int Id { get; set; }

        public int? ClaimDataBatchId { get; set; }
        public ClaimDataBatchBo ClaimDataBatchBo { get; set; }

        public int? ClaimDataId { get; set; }
        public ClaimDataBo ClaimDataBo { get; set; }

        public int? ClaimDataConfigId { get; set; }
        public ClaimDataConfigBo ClaimDataConfigBo { get; set; }

        public int? SoaDataBatchId { get; set; }
        public SoaDataBatchBo SoaDataBatchBo { get; set; }

        [DisplayName("RI Data")]
        public int? RiDataWarehouseId { get; set; }
        public int? ReferralRiDataId { get; set; }
        public virtual RiDataWarehouseBo RiDataWarehouseBo { get; set; }

        public int? ReferralClaimId { get; set; }
        public ReferralClaimBo ReferralClaimBo { get; set; }

        public int? OriginalClaimRegisterId { get; set; }
        public ClaimRegisterBo OriginalClaimRegisterBo { get; set; }

        public int? ClaimReasonId { get; set; }
        public ClaimReasonBo ClaimReasonBo { get; set; }

        public int? PicClaimId { get; set; }
        public UserBo PicClaimBo { get; set; }

        public int? PicDaaId { get; set; }
        public UserBo PicDaaBo { get; set; }

        public int ClaimStatus { get; set; }

        public int? ClaimDecisionStatus { get; set; }

        public int ProvisionStatus { get; set; }

        public int DrProvisionStatus { get; set; }

        public int OffsetStatus { get; set; }

        public int MappingStatus { get; set; }

        public int ProcessingStatus { get; set; }

        public int DuplicationCheckStatus { get; set; }

        public int PostComputationStatus { get; set; }

        public int PostValidationStatus { get; set; }

        public string Errors { get; set; }

        public dynamic ErrorObject { get; set; }

        public IDictionary<string, object> ErrorDictionary { get; set; }

        public string ProvisionErrors { get; set; }

        public string RedFlagWarnings { get; set; }

        public bool IsReferralCase { get; set; }

        public bool HasRedFlag { get; set; }

        public DateTime? TargetDateToIssueInvoice { get; set; }

        public string ClaimId { get; set; }

        public string ClaimCode { get; set; }
        public ClaimCodeBo ClaimCodeBo { get; set; }

        public string PolicyNumber { get; set; }

        public double? PolicyTerm { get; set; }

        [DisplayName("Claim Recovery Amount")]
        public double? ClaimRecoveryAmt { get; set; }

        public string ClaimTransactionType { get; set; }

        public string TreatyCode { get; set; }

        public TreatyCodeBo TreatyCodeBo { get; set; }

        [DisplayName("Treaty Type")]
        public string TreatyType { get; set; }
        public PickListDetailBo TreatyTypePickListDetailBo { get; set; }

        public string BusinessOrigin { get; set; }

        public double? AarPayable { get; set; }

        public double? AnnualRiPrem { get; set; }

        [MaxLength(255)]
        public string CauseOfEvent { get; set; }

        [MaxLength(30)]
        public string CedantClaimEventCode { get; set; }

        [MaxLength(30)]
        public string CedantClaimType { get; set; }

        public DateTime? CedantDateOfNotification { get; set; }

        [MaxLength(30)]
        public string CedingBenefitRiskCode { get; set; }

        [MaxLength(30)]
        public string CedingBenefitTypeCode { get; set; }

        [MaxLength(30)]
        public string CedingClaimType { get; set; }

        [MaxLength(30)]
        public string CedingCompany { get; set; }

        [MaxLength(30)]
        public string CedingEventCode { get; set; }

        [MaxLength(30)]
        public string CedingPlanCode { get; set; }

        public double? CurrencyRate { get; set; }

        [MaxLength(3)]
        public string CurrencyCode { get; set; }

        public DateTime? DateApproved { get; set; }

        [DisplayName("Date Of Event")]
        public DateTime? DateOfEvent { get; set; }

        public DateTime? DateOfRegister { get; set; }

        [DisplayName("Date Of Reported")]
        public DateTime? DateOfReported { get; set; }

        [MaxLength(30)]
        public string EntryNo { get; set; }

        public double? ExGratia { get; set; }

        public double? ForeignClaimRecoveryAmt { get; set; }

        [MaxLength(30)]
        public string FundsAccountingTypeCode { get; set; }

        public DateTime? InsuredDateOfBirth { get; set; }

        [MaxLength(1)]
        public string InsuredGenderCode { get; set; }

        [MaxLength(128)]
        public string InsuredName { get; set; }

        [MaxLength(1)]
        public string InsuredTobaccoUse { get; set; }

        public DateTime? LastTransactionDate { get; set; }

        [MaxLength(30)]
        public string LastTransactionQuarter { get; set; }

        public double? LateInterest { get; set; }

        public double? Layer1SumRein { get; set; }

        public int? Mfrs17AnnualCohort { get; set; }

        [MaxLength(30)]
        public string Mfrs17ContractCode { get; set; }

        [MaxLength(30)]
        [DisplayName("MLRe Benefit Code")]
        public string MlreBenefitCode { get; set; }

        [MaxLength(30)]
        public string MlreEventCode { get; set; }

        public DateTime? MlreInvoiceDate { get; set; }

        [MaxLength(30)]
        public string MlreInvoiceNumber { get; set; }

        public double? MlreRetainAmount { get; set; }

        public double? MlreShare { get; set; }

        public int? PendingProvisionDay { get; set; }

        public int? PolicyDuration { get; set; }

        [MaxLength(30)]
        public string RecordType { get; set; }

        [MaxLength(30)]
        public string ReinsBasisCode { get; set; }

        [DisplayName("Reins Effective Date Pol")]
        public DateTime? ReinsEffDatePol { get; set; }

        [MaxLength(128)]
        public string RetroParty1 { get; set; }

        [MaxLength(128)]
        public string RetroParty2 { get; set; }

        [MaxLength(128)]
        public string RetroParty3 { get; set; }

        public double? RetroRecovery1 { get; set; }

        public double? RetroRecovery2 { get; set; }

        public double? RetroRecovery3 { get; set; }

        public DateTime? RetroStatementDate1 { get; set; }

        public DateTime? RetroStatementDate2 { get; set; }

        public DateTime? RetroStatementDate3 { get; set; }

        [MaxLength(30)]
        public string RetroStatementId1 { get; set; }

        [MaxLength(30)]
        public string RetroStatementId2 { get; set; }

        [MaxLength(30)]
        public string RetroStatementId3 { get; set; }

        public double? RetroShare1 { get; set; }

        public double? RetroShare2 { get; set; }

        public double? RetroShare3 { get; set; }

        public int? RiskPeriodMonth { get; set; }

        public int? RiskPeriodYear { get; set; }

        [MaxLength(30)]
        public string RiskQuarter { get; set; }

        public double? SaFactor { get; set; }

        [MaxLength(30)]
        public string SoaQuarter { get; set; }

        public double? SumIns { get; set; }

        public double? TempA1 { get; set; }

        public double? TempA2 { get; set; }

        public DateTime? TempD1 { get; set; }

        public DateTime? TempD2 { get; set; }

        public int? TempI1 { get; set; }

        public int? TempI2 { get; set; }

        [MaxLength(150)]
        public string TempS1 { get; set; }

        [MaxLength(50)]
        public string TempS2 { get; set; }

        public DateTime? TransactionDateWop { get; set; }

        [MaxLength(30)]
        public string MlreReferenceNo { get; set; }

        [MaxLength(30)]
        public string AddInfo { get; set; }

        [MaxLength(128)]
        public string Remark1 { get; set; }

        [MaxLength(128)]
        public string Remark2 { get; set; }

        [DisplayName("Issue Date Policy")]
        public DateTime? IssueDatePol { get; set; }

        public DateTime? PolicyExpiryDate { get; set; }

        public int? ClaimAssessorId { get; set; }

        public UserBo ClaimAssessorBo { get; set; }

        [MaxLength(128)]
        public string Comment { get; set; }

        public int? SignOffById { get; set; }

        public UserBo SignOffByBo { get; set; }

        public DateTime? SignOffDate { get; set; }

        [MaxLength(30)]
        public string CedingTreatyCode { get; set; }

        [MaxLength(10)]
        public string CampaignCode { get; set; }

        public string ClaimRecoveryAmtStr { get; set; }

        public string AarPayableStr { get; set; }

        public string AnnualRiPremStr { get; set; }

        public string CurrencyRateStr { get; set; }

        public string ExGratiaStr { get; set; }

        public string ForeignClaimRecoveryAmtStr { get; set; }

        public string LateInterestStr { get; set; }

        public string Layer1SumReinStr { get; set; }

        public string MlreRetainAmountStr { get; set; }

        public string MlreShareStr { get; set; }

        public string RetroRecovery1Str { get; set; }

        public string RetroRecovery2Str { get; set; }

        public string RetroRecovery3Str { get; set; }

        public string RetroShare1Str { get; set; }

        public string RetroShare2Str { get; set; }

        public string RetroShare3Str { get; set; }

        public string SaFactorStr { get; set; }

        public string SumInsStr { get; set; }

        public string TempA1Str { get; set; }

        public string TempA2Str { get; set; }

        public string TargetDateToIssueInvoiceStr { get; set; }

        public string CedantDateOfNotificationStr { get; set; }

        public string DateApprovedStr { get; set; }

        public string DateOfEventStr { get; set; }

        public string DateOfRegisterStr { get; set; }

        public string DateOfReportedStr { get; set; }

        public string InsuredDateOfBirthStr { get; set; }

        public string LastTransactionDateStr { get; set; }

        public string MlreInvoiceDateStr { get; set; }

        public string ReinsEffDatePolStr { get; set; }

        public string RetroStatementDate1Str { get; set; }

        public string RetroStatementDate2Str { get; set; }

        public string RetroStatementDate3Str { get; set; }

        public string TempD1Str { get; set; }

        public string TempD2Str { get; set; }

        public string TransactionDateWopStr { get; set; }

        public string IssueDatePolStr { get; set; }

        public string PolicyExpiryDateStr { get; set; }

        public string SignOffDateStr { get; set; }

        public string PostComputationStatusStr { get; set; }

        public string PostValidationStatusStr { get; set; }

        // Underwriting
        public bool RequestUnderwriterReview { get; set; }

        public int? UnderwriterFeedback { get; set; }

        // Ex Gratia 
        public string EventChronologyComment { get; set; }

        public string ClaimAssessorRecommendation { get; set; }

        public string ClaimCommitteeComment1 { get; set; }

        public string ClaimCommitteeComment2 { get; set; }

        public int? ClaimCommitteeUser1Id { get; set; }
        public UserBo ClaimCommitteeUser1Bo { get; set; }

        public string ClaimCommitteeUser1Name { get; set; }

        public int? ClaimCommitteeUser2Id { get; set; }
        public UserBo ClaimCommitteeUser2Bo { get; set; }

        public string ClaimCommitteeUser2Name { get; set; }

        public DateTime? ClaimCommitteeDateCommented1 { get; set; }

        public string ClaimCommitteeDateCommented1Str { get; set; }

        public DateTime? ClaimCommitteeDateCommented2 { get; set; }

        public string ClaimCommitteeDateCommented2Str { get; set; }

        public int? CeoClaimReasonId { get; set; }

        public virtual ClaimReasonBo CeoClaimReasonBo { get; set; }

        public string CeoComment { get; set; }

        public int? UpdatedOnBehalfById { get; set; }
        public UserBo UpdatedOnBehalfByBo { get; set; }

        public DateTime? UpdatedOnBehalfAt { get; set; }
        public string UpdatedOnBehalfAtStr { get; set; }

        public DateTime? DateOfIntimation { get; set; }
        public string DateOfIntimationStr { get; set; }

        // Checklist
        public string Checklist { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        // Dashboard
        public int NoOfCase { get; set; }

        public string StatusName { get; set; }

        public int PendingCase { get; set; }

        public int OverdueCase { get; set; }

        public int MaxDay { get; set; }

        public int UnassignedCase { get; set; }

        public string FollowUpWith { get; set; }

        public string FollowUpDateStr { get; set; }

        public string Remark { get; set; }

        // Related Claims
        public string ReferralId { get; set; }

        public double? TotalRetroAmount { get; set; }

        public string TotalRetroAmountStr { get; set; }

        // To Use in Batch Selection
        public string BatchSelectionError { get; set; }

        // To use in export
        public int SortIndex { get; set; }
        public DateTime? ProvisionAt { get; set; }

        public const int MappingStatusPending = 1;
        public const int MappingStatusSuccess = 2;
        public const int MappingStatusFailed = 3;
        public const int MappingStatusMax = 3;

        public const int ProcessingStatusPending = 1;
        public const int ProcessingStatusSuccess = 2;
        public const int ProcessingStatusFailed = 3;
        public const int ProcessingStatusMax = 3;

        public const int DuplicationCheckStatusPending = 1;
        public const int DuplicationCheckStatusNoDuplicate = 2;
        public const int DuplicationCheckStatusHasDuplicate = 3;
        public const int DuplicationCheckStatusMax = 3;

        public const int PostComputationStatusPending = 1;
        public const int PostComputationStatusSuccess = 2;
        public const int PostComputationStatusFailed = 3;
        public const int PostComputationStatusMax = 3;

        public const int PostValidationStatusPending = 1;
        public const int PostValidationStatusSuccess = 2;
        public const int PostValidationStatusFailed = 3;
        public const int PostValidationStatusMax = 3;

        public const int ProvisionStatusPending = 1;
        public const int ProvisionStatusProvisioning = 2;
        public const int ProvisionStatusFailed = 3;
        public const int ProvisionStatusPendingReprovision = 4;
        public const int ProvisionStatusProvisioned = 5;
        public const int ProvisionStatusPendingReprocess = 6;
        public const int ProvisionStatusMax = 6;

        public const int DrProvisionStatusPending = 1;
        public const int DrProvisionStatusSuccess = 2;
        public const int DrProvisionStatusFailed = 3;
        public const int DrProvisionStatusMax = 3;

        public const int OffsetStatusPending = 1;
        public const int OffsetStatusOffset = 2;
        public const int OffsetStatusPendingInvoicing = 3;
        public const int OffsetStatusNotRequired = 4;
        public const int OffsetStatusMax = 4;

        // Initial Status
        public const int StatusReported = 1;
        // Processing Status (Include mapping, processing, post computation and post validation)
        public const int StatusProcessing = 2;
        public const int StatusSuccess = 3;
        public const int StatusFailed = 4;
        // Mid
        public const int StatusPendingClarification = 5;
        public const int StatusClosed = 6;
        public const int StatusSuspectedDuplication = 7;
        // After registered
        public const int StatusRegistered = 8;
        public const int StatusPostUnderwritingReview = 9;
        public const int StatusApprovalByLimit = 10;
        public const int StatusPendingCeoApproval = 11;
        public const int StatusApprovedByCeo = 12;          // old 11
        public const int StatusApproved = 13;               // old 12
        // Referral Claim
        public const int StatusApprovedReferralClaim = 14;  // old 13
        public const int StatusDeclinedReferralClaim = 15;  // old 14
        public const int StatusClosedReferralClaim = 16;    // old 15
        // Decline
        public const int StatusDeclinedByClaim = 17;        // old 16
        public const int StatusPendingCeoSignOff = 18;
        //public const int StatusPendingDecline = 17;
        public const int StatusDeclined = 19;               // old 18
        public const int StatusMax = 19;

        public const int ClaimDecisionStatusApproved = 1;
        public const int ClaimDecisionStatusDeclined = 2;
        public const int ClaimDecisionStatusApprovedOverwrite = 3;
        public const int ClaimDecisionStatusMax = 3;

        public const int ClaimTransactionTypeNew = 1;
        public const int ClaimTransactionTypeBulk = 2;
        public const int ClaimTransactionTypeAdj = 3;
        public const int ClaimTransactionTypeMax = 3;

        public const string ClaimTransactionTypeAdjName = "ADJ";
        public const string ClaimTransactionTypeNewName = "NEW";
        public const string ClaimTransactionTypeBulkName = "BULK";

        public const int UnderwriterFeedbackAcceptedNoAlteration = 1;
        public const int UnderwriterFeedbackAcceptedAlteration = 2;
        public const int UnderwriterFeedbackAppliedExclusion = 3;
        public const int UnderwriterFeedbackDeclined = 4;
        public const int UnderwriterFeedbackOthers = 5;
        public const int UnderwriterFeedbackMax = 5;

        public const int ChecklistStatusPending = 1;
        public const int ChecklistStatusNotApplicable = 2;
        public const int ChecklistStatusCompleted = 3;
        public const int ChecklistStatusMax = 3;

        public const string SubModuleUnderwriting = "Underwriting";

        public const string EventClaimCodeMappingTitle = "Event & Claim Code Mapping";
        public const string ClaimCodeMappingTitle = "Claim Code Mapping";
        public const string RiDataMappingTitle = "Ri Data Mapping";

        public const int RedFlagEarlyClaims = 1;
        public const int RedFlagFacClaims = 2;
        public const int RedFlagClaimExceed200000 = 3;
        public const int RedFlagClaimExceed50000 = 4;
        public const int RedFlagDuplicateDiffClaimCode = 5;
        public const int RedFlagDuplicateDiffCOE = 6;
        public const int RedFlagClaimNotified6Years = 7;
        public const int RedFlagCreateAdjustment = 8;
        public const int RedFlagExceedTreatyShare = 9;
        public const int RedFlagExceedCedantLimit = 10;
        public const int RedFlagEarlyClaims2 = 11;

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusReported:
                    return "Reported";
                case StatusProcessing:
                    return "Processing";
                case StatusSuccess:
                    return "Success";
                case StatusFailed:
                    return "Failed";
                case StatusPendingClarification:
                    return "Pending Clarification";
                case StatusClosed:
                    return "Closed";
                case StatusSuspectedDuplication:
                    return "Suspected Duplication";
                case StatusRegistered:
                    return "Registered";
                case StatusPostUnderwritingReview:
                    return "Post Underwriting Review";
                case StatusApprovalByLimit:
                    return "Approval By Limit";
                case StatusPendingCeoApproval:
                    return "Pending CEO Approval";
                case StatusApprovedByCeo:
                    return "Approved By CEO";
                case StatusApproved:
                    return "Approved";
                case StatusApprovedReferralClaim:
                    return "Approved Referral Claim";
                case StatusDeclinedReferralClaim:
                    return "Declined Referral Claim";
                case StatusClosedReferralClaim:
                    return "Closed Referral Claim";
                case StatusDeclinedByClaim:
                    return "Declined (By Claim)";
                //case StatusPendingDecline:
                //    return "Pending Decline";
                case StatusPendingCeoSignOff:
                    return "Pending CEO Sign Off";
                case StatusDeclined:
                    return "Declined";
                default:
                    return "";
            }
        }

        public static string GetClaimDecisionStatusName(int? key)
        {
            switch (key)
            {
                case ClaimDecisionStatusApproved:
                    return "Approved";
                case ClaimDecisionStatusDeclined:
                    return "Declined";
                case ClaimDecisionStatusApprovedOverwrite:
                    return "Approved (Overwrite)";
                default:
                    return "";
            }
        }

        public static string GetStatusClass(int key)
        {
            switch (key)
            {
                case StatusReported:
                    return "status-pending-badge";
                case StatusProcessing:
                    return "status-processing-badge";
                case StatusSuccess:
                    return "status-success-badge";
                case StatusFailed:
                    return "status-fail-badge";
                case StatusPendingClarification:
                    return "status-pending-badge";
                case StatusClosed:
                    return "status-fail-badge";
                case StatusSuspectedDuplication:
                    return "status-processing-badge";
                case StatusRegistered:
                    return "status-success-badge";
                case StatusPostUnderwritingReview:
                    return "status-pending-badge";
                case StatusApprovalByLimit:
                    return "status-pending-badge";
                case StatusPendingCeoApproval:
                    return "status-pending-badge";
                case StatusApprovedByCeo:
                    return "status-processing-badge";
                case StatusApproved:
                    return "status-success-badge";
                case StatusApprovedReferralClaim:
                    return "status-success-badge";
                case StatusDeclinedReferralClaim:
                    return "status-fail-badge";
                case StatusClosedReferralClaim:
                    return "status-fail-badge";
                case StatusDeclinedByClaim:
                    return "status-fail-badge";
                //case StatusPendingDecline:
                //    return "status-pending-badge";
                case StatusPendingCeoSignOff:
                    return "status-pending-badge";
                case StatusDeclined:
                    return "status-fail-badge";
                default:
                    return "";
            }
        }

        public static string GetMappingStatusName(int? key)
        {
            switch (key)
            {
                case MappingStatusPending:
                    return "Pending";
                case MappingStatusSuccess:
                    return "Success";
                case MappingStatusFailed:
                    return "Failed";
                default:
                    return "";
            }
        }

        public static string GetProcessingStatusName(int? key)
        {
            switch (key)
            {
                case ProcessingStatusPending:
                    return "Pending";
                case ProcessingStatusSuccess:
                    return "Success";
                case ProcessingStatusFailed:
                    return "Failed";
                default:
                    return "";
            }
        }

        public static string GetDuplicationCheckStatusName(int? key)
        {
            switch (key)
            {
                case DuplicationCheckStatusPending:
                    return "Pending";
                case DuplicationCheckStatusNoDuplicate:
                    return "No Duplicate";
                case DuplicationCheckStatusHasDuplicate:
                    return "Has Duplicate";
                default:
                    return "";
            }
        }

        public static string GetDuplicationCheckStatusClass(int key)
        {
            switch (key)
            {
                case DuplicationCheckStatusPending:
                    return "status-pending-badge";
                case DuplicationCheckStatusNoDuplicate:
                    return "status-success-badge";
                case DuplicationCheckStatusHasDuplicate:
                    return "status-fail-badge";
                default:
                    return "";
            }
        }

        public static string GetPostComputationStatusName(int? key)
        {
            switch (key)
            {
                case PostComputationStatusPending:
                    return "Pending";
                case PostComputationStatusSuccess:
                    return "Success";
                case PostComputationStatusFailed:
                    return "Failed";
                default:
                    return "";
            }
        }

        public static string GetPostValidationStatusName(int? key)
        {
            switch (key)
            {
                case PostValidationStatusPending:
                    return "Pending";
                case PostValidationStatusSuccess:
                    return "Success";
                case PostValidationStatusFailed:
                    return "Failed";
                default:
                    return "";
            }
        }

        public static string GetProvisionStatusName(int? key)
        {
            switch (key)
            {
                case ProvisionStatusPending:
                    return "Pending Provision";
                case ProvisionStatusProvisioning:
                    return "Provisioning";
                case ProvisionStatusFailed:
                    return "Provision Failed";
                case ProvisionStatusPendingReprovision:
                    return "Pending Reprovision";
                case ProvisionStatusProvisioned:
                    return "Provisioned";
                case ProvisionStatusPendingReprocess:
                    return "Pending Reprocess Provision";
                default:
                    return "";
            }
        }

        public static string GetProvisionStatusClass(int key)
        {
            switch (key)
            {
                case ProvisionStatusPending:
                    return "status-pending-badge";
                case ProvisionStatusProvisioning:
                    return "status-processing-badge";
                case ProvisionStatusFailed:
                    return "status-fail-badge";
                case ProvisionStatusPendingReprovision:
                    return "status-pending-badge";
                case ProvisionStatusProvisioned:
                    return "status-success-badge";
                case ProvisionStatusPendingReprocess:
                    return "status-pending-badge";
                default:
                    return "";
            }
        }

        public static string GetDrProvisionStatusName(int? key)
        {
            switch (key)
            {
                case DrProvisionStatusPending:
                    return "Pending";
                case DrProvisionStatusSuccess:
                    return "Success";
                case DrProvisionStatusFailed:
                    return "Failed";
                default:
                    return "";
            }
        }

        public static string GetDrProvisionStatusClass(int key)
        {
            switch (key)
            {
                case DrProvisionStatusPending:
                    return "status-pending-badge";
                case DrProvisionStatusSuccess:
                    return "status-success-badge";
                case DrProvisionStatusFailed:
                    return "status-fail-badge";
                default:
                    return "";
            }
        }

        public static string GetOffsetStatusName(int? key)
        {
            switch (key)
            {
                case OffsetStatusPending:
                    return "Pending Offset";
                case OffsetStatusOffset:
                    return "OffSet";
                case OffsetStatusPendingInvoicing:
                    return "Pending Invoicing";
                case OffsetStatusNotRequired:
                    return "Offset Not Required";
                default:
                    return "";
            }
        }

        public static string GetOffsetStatusClass(int key)
        {
            switch (key)
            {
                case OffsetStatusPending:
                    return "status-pending-badge";
                case OffsetStatusPendingInvoicing:
                    return "status-processing-badge";
                case OffsetStatusOffset:
                    return "status-success-badge";
                case OffsetStatusNotRequired:
                    return "status-fail-badge";
                default:
                    return "";
            }
        }

        public static string GetClaimTransactionTypeName(int? key)
        {
            switch (key)
            {
                case ClaimTransactionTypeNew:
                    return "New";
                case ClaimTransactionTypeBulk:
                    return "Bulk";
                case ClaimTransactionTypeAdj:
                    return "Adjustment";
                default:
                    return "";
            }
        }

        public static int GetKeyByClaimTransactionTypeName(string name)
        {
            switch (name?.ToLower())
            {
                case "new":
                    return ClaimTransactionTypeNew;
                case "bulk":
                    return ClaimTransactionTypeBulk;
                case "adjustment":
                    return ClaimTransactionTypeAdj;
                default:
                    return 0;
            }
        }

        public static string GetUnderwriterFeedbackName(int? key)
        {
            switch (key)
            {
                case UnderwriterFeedbackAcceptedNoAlteration:
                    return "Accepted with no alteration";
                case UnderwriterFeedbackAcceptedAlteration:
                    return "Accepted with alteration";
                case UnderwriterFeedbackAppliedExclusion:
                    return "Applied an exclusion";
                case UnderwriterFeedbackDeclined:
                    return "Declined Application";
                case UnderwriterFeedbackOthers:
                    return "Others";
                default:
                    return "";
            }
        }

        public static string GetChecklistStatusName(int key)
        {
            switch (key)
            {
                case ChecklistStatusPending:
                    return "Pending";
                case ChecklistStatusNotApplicable:
                    return "Not Applicable";
                case ChecklistStatusCompleted:
                    return "Completed";
                default:
                    return "";
            }
        }

        public static string GetRedFlagWarnings(int key)
        {
            switch (key)
            {
                case RedFlagEarlyClaims:
                    return "Early Claims less than 24 months from DOC to DOE";
                case RedFlagFacClaims:
                    return "Facultative Claim";
                case RedFlagClaimExceed200000:
                    return "Claim exceed RM200,000";
                case RedFlagClaimExceed50000:
                    return "Claim exceed RM50,000";
                case RedFlagDuplicateDiffClaimCode:
                    return "Multiple Claims found with Same Name and DOE but Different Claim Code";
                case RedFlagDuplicateDiffCOE:
                    return "Multiple Claims found with Same Name, DOE and Claim Code with Different Cause of Events";
                case RedFlagClaimNotified6Years:
                    return "Claims notified above 6 years from DOE";
                case RedFlagCreateAdjustment:
                    return "Create adjustment claim to reverse current amount";
                case RedFlagExceedTreatyShare:
                    return "Treaty Share exceeded amount";
                case RedFlagExceedCedantLimit:
                    return "Claim exceed cedant limit";
                case RedFlagEarlyClaims2:
                    return "Reinsurance Effective Date Policy is earlier than Date of Event";
                default:
                    return "";
            }
        }

        public static List<int> GetClaimDepartmentStatus(bool includeDaa = true)
        {
            var status = new List<int>()
            {
                // After registered
                StatusRegistered,
                StatusPostUnderwritingReview,
                StatusApprovalByLimit,
                StatusPendingCeoApproval,
                StatusApprovedByCeo,
                StatusApproved,
                // Referral Claim
                StatusApprovedReferralClaim,
                StatusDeclinedReferralClaim,
                StatusClosedReferralClaim,
                // Decline
                StatusDeclinedByClaim,
                StatusPendingCeoSignOff,
                StatusDeclined
            };

            if (!includeDaa)
                return status;

            status.Insert(0, StatusSuspectedDuplication);
            status.Insert(0, StatusPendingClarification);
            //status.Add(StatusPendingDecline);
            //status.Add(StatusDeclined);
            return status;
        }

        public ClaimRegisterBo()
        {
            ErrorObject = new ExpandoObject();
            ErrorDictionary = (IDictionary<string, object>)ErrorObject;

            ClaimStatus = StatusReported;
            MappingStatus = MappingStatusPending;
            ProcessingStatus = ProcessingStatusPending;
            DuplicationCheckStatus = DuplicationCheckStatusPending;
            PostComputationStatus = PostComputationStatusPending;
            PostValidationStatus = PostValidationStatusPending;
            ProvisionStatus = ProvisionStatusPending;
            OffsetStatus = OffsetStatusPending;

            RequestUnderwriterReview = false;
        }

        public ClaimRegisterBo(bool registered = false)
        {
            ErrorObject = new ExpandoObject();
            ErrorDictionary = (IDictionary<string, object>)ErrorObject;

            ClaimStatus = registered ? StatusRegistered : StatusReported;
            MappingStatus = registered ? MappingStatusSuccess : MappingStatusPending;
            ProcessingStatus = registered ? ProcessingStatusSuccess : ProcessingStatusPending;
            DuplicationCheckStatus = registered ? DuplicationCheckStatusNoDuplicate : DuplicationCheckStatusPending;
            PostComputationStatus = registered ? PostComputationStatusSuccess : PostComputationStatusPending;
            PostValidationStatus = registered ? PostValidationStatusSuccess : PostValidationStatusPending;
            ProvisionStatus = ProvisionStatusPending;
            OffsetStatus = OffsetStatusPending;

            RequestUnderwriterReview = false;
        }

        // Format Errors
        public static string FormatEmptyError(int type, string title)
        {
            return FormatEmptyError(StandardClaimDataOutputBo.GetTypeName(type), title);
        }

        public static string FormatEmptyError(string field, string title)
        {
            return FormatError(string.Format(MessageBag.IsEmpty, field), title);
        }

        public static string FormatError(string msg, string title)
        {
            return string.Format("{0}: {1}", title, msg);
        }

        // Validation
        public List<string> ValidateEventClaimCodeMapping()
        {
            List<int> required = new List<int>
            {
                StandardClaimDataOutputBo.TypeCedingEventCode,
                StandardClaimDataOutputBo.TypeCedingClaimType,
            };

            List<string> errors = new List<string> { };
            if (string.IsNullOrEmpty(CedingEventCode) && string.IsNullOrEmpty(CedingClaimType))
            {
                string cedingEventCodeName = StandardClaimDataOutputBo.GetTypeName(StandardClaimDataOutputBo.TypeCedingEventCode);
                string cedingClaimTypeName = StandardClaimDataOutputBo.GetTypeName(StandardClaimDataOutputBo.TypeCedingClaimType);
                string emptyField = string.Format("{0} & {1}", cedingEventCodeName, cedingClaimTypeName);

                errors.Add(FormatEmptyError(emptyField, EventClaimCodeMappingTitle));
            }

            return errors;
        }

        public List<string> ValidateClaimCodeMapping()
        {
            List<int> required = new List<int>
            {
                StandardClaimDataOutputBo.TypeMlreEventCode,
                StandardClaimDataOutputBo.TypeMlreBenefitCode,
            };

            List<string> errors = new List<string> { };
            foreach (int type in required)
            {
                string property = StandardClaimDataOutputBo.GetPropertyNameByType(type);
                object value = this.GetPropertyValue(property);
                if (value == null)
                {
                    errors.Add(FormatEmptyError(type, ClaimCodeMappingTitle));
                }
                else if (value is string @string && string.IsNullOrEmpty(@string))
                {
                    errors.Add(FormatEmptyError(type, ClaimCodeMappingTitle));
                }
            }

            return errors;
        }

        public List<string> ValidateRiDataMapping()
        {
            List<int> required = new List<int>
            {
                StandardClaimDataOutputBo.TypeFundsAccountingTypeCode,
                StandardClaimDataOutputBo.TypeTreatyCode,
                StandardClaimDataOutputBo.TypePolicyNumber,
                StandardClaimDataOutputBo.TypeInsuredName,
                //StandardClaimDataOutputBo.TypeCedingPlanCode,
                StandardClaimDataOutputBo.TypeMlreEventCode,
                StandardClaimDataOutputBo.TypeDateOfEvent,
            };

            List<string> errors = new List<string> { };
            foreach (int type in required)
            {
                string property = StandardClaimDataOutputBo.GetPropertyNameByType(type);
                object value = this.GetPropertyValue(property);
                if (value == null)
                {
                    errors.Add(FormatEmptyError(type, RiDataMappingTitle));
                }
                else if (value is string @string && string.IsNullOrEmpty(@string))
                {
                    errors.Add(FormatEmptyError(type, RiDataMappingTitle));
                }
            }

            return errors;
        }

        public bool SetClaimData(int datatype, string property, object value, RiDataMappingBo mapping = null)
        {
            if (value is string valueStr)
            {
                value = valueStr.Trim();
            }
            switch (datatype)
            {
                case StandardOutputBo.DataTypeDropDown:
                    break;
                default:
                    if (value == null || value is string @string && string.IsNullOrEmpty(@string))
                    {
                        this.SetPropertyValue(property, null);
                        return true;
                    }
                    break;
            }

            switch (datatype)
            {
                case StandardOutputBo.DataTypeDate:
                    return SetDate(property, value);
                case StandardOutputBo.DataTypeString:
                    return SetString(property, value);
                case StandardOutputBo.DataTypeAmount:
                case StandardOutputBo.DataTypePercentage:
                    return SetDouble(property, value);
                case StandardOutputBo.DataTypeInteger:
                    return SetInteger(property, value);
                case StandardOutputBo.DataTypeDropDown:
                    return SetDropDown(property, value);
            }
            return false;
        }

        public bool SetDate(string property, object value)
        {
            DateTime? date = DateTime.Parse(value.ToString());
            this.SetPropertyValue(property, date);
            return true;
        }

        public bool SetString(string property, object value)
        {
            string output = value?.ToString();
            if (output != null)
            {
                int length = output.Length;
                var maxLengthAttr = this.GetAttributeFrom<MaxLengthAttribute>(property);
                if (maxLengthAttr != null && length > 0 && length > maxLengthAttr.Length)
                {
                    throw new Exception(string.Format(MessageBag.MaxLength, length, maxLengthAttr.Length));
                }
            }
            this.SetPropertyValue(property, output);

            return true;
        }

        public bool SetDouble(string property, object value)
        {
            string s = value.ToString();
            if (Util.IsValidDouble(s, out double? output, out string error, true))
            {
                this.SetPropertyValue(property, output);
            }
            else
            {
                throw new Exception(error);
            }

            return true;
        }

        public bool SetInteger(string property, object value)
        {
            string s = value.ToString();
            this.SetPropertyValue(property, int.Parse(s));
            return true;
        }

        public bool SetDropDown(string property, object value)
        {
            string output = value?.ToString();
            if (output != null)
            {
                int length = output.Length;
                var maxLengthAttr = this.GetAttributeFrom<MaxLengthAttribute>(property);
                if (maxLengthAttr != null && length > 0 && length > maxLengthAttr.Length)
                {
                    throw new Exception(string.Format(MessageBag.MaxLength, length, maxLengthAttr.Length));
                }
            }
            this.SetPropertyValue(property, output);

            return true;
        }

        public void SetError(string property, dynamic value)
        {
            ErrorDictionary[property] = value;
            Errors = JsonConvert.SerializeObject(ErrorObject);
        }

        public static List<Column> GetColumns(bool isWithAdjustmentDetails = false)
        {
            List<Column> columns = new List<Column>()
            {
                new Column
                {
                    Header = "ID",
                    Property = "Id",
                    Editable = false,
                },
                new Column
                {
                    Header = "CLAIM STATUS",
                    Property = "ClaimStatus",
                    Editable = false,
                },
                new Column
                {
                    Header = "MAPPING STATUS",
                    Property = "MappingStatus",
                    Editable = false,
                },
                new Column
                {
                    Header = "PROCESSING STATUS",
                    Property = "ProcessingStatus",
                    Editable = false,
                },
                new Column
                {
                    Header = "DUPLICATION CHECK STATUS",
                    Property = "DuplicationCheckStatus",
                    Editable = false,
                },
                new Column
                {
                    Header = "POST-COMPUTATION STATUS",
                    Property = "PostComputationStatus",
                    Editable = false,
                },
                new Column
                {
                    Header = "POST-VALIDATION STATUS",
                    Property = "PostValidationStatus",
                    Editable = false,
                },
                new Column
                {
                    Header = "PROVISION STATUS",
                    Property = "ProvisionStatus",
                    Editable = false,
                },
                new Column
                {
                    Header = "DECISION STATUS",
                    Property = "ClaimDecisionStatus",
                    Editable = false,
                },
                new Column
                {
                    Header = "OFFSET STATUS",
                    Property = "OffsetStatus",
                    Editable = false,
                },
                new Column
                {
                    Header = "PERSON IN-CHARGE(CLAIMS)",
                    Property = "PicClaimId",
                    Editable = false,
                },
                new Column
                {
                    Header = "PERSON IN-CHARGE(DA&A)",
                    Property = "PicDaaId",
                    Editable = false,
                },
                new Column
                {
                    Header = "REFERRAL CASE INDICATOR",
                    Property = "IsReferralCase",
                    Editable = false,
                },
            };


            // add all standard fields
            foreach (int i in StandardClaimDataOutputBo.GetExportFileOrder())
            {
                switch (i)
                {
                    case StandardClaimDataOutputBo.TypeDateApproved:
                        columns.Add(new Column
                        {
                            Header = "TARGET DATE TO ISSUE INVOICE",
                            Property = "TargetDateToIssueInvoice",
                        });
                        break;
                    case StandardClaimDataOutputBo.TypeSoaQuarter:
                        columns.Add(new Column
                        {
                            Header = "BUSINESS ORIGIN",
                            Property = "TreatyCode",
                            Editable = false,
                        });
                        break;
                    case StandardClaimDataOutputBo.TypeExGratia:
                        columns.Add(new Column
                        {
                            Header = "CLAIM REASON (DECLINE/ADJUSTMENT)",
                            Property = "ClaimReasonId",
                            Editable = false,
                        });
                        columns.Add(new Column
                        {
                            Header = "COMMENT",
                            Property = "Comment",
                        });
                        break;
                    case StandardClaimDataOutputBo.TypeTempA1:
                        columns.Add(new Column
                        {
                            Header = "RI DATA ID",
                            Property = "RiDataWarehouseId",
                            Editable = false,
                        });
                        columns.Add(new Column
                        {
                            Header = "SOA DATA ID",
                            Property = "SoaDataBatchId"
                        });
                        break;
                    default:
                        break;
                }

                string header = i == StandardClaimDataOutputBo.TypeReinsEffDatePol ? "DATE_OF_COMMENCEMENT" : StandardClaimDataOutputBo.GetCodeByType(i);
                columns.Add(new Column
                {
                    Type = i == StandardClaimDataOutputBo.TypeReinsEffDatePol ? StandardClaimDataOutputBo.TypeReinsEffDatePol : 0, // Use in Process Claim Register
                    Header = header,
                    Property = StandardClaimDataOutputBo.GetPropertyNameByType(i),
                });
            }

            if (isWithAdjustmentDetails)
            {
                columns.Add(new Column
                {
                    Header = "Provision Date",
                    Property = "ProvisionAt",
                    Editable = false,
                });
            }

            columns.Add(new Column
            {
                Header = "Add Info",
                Property = "AddInfo",
            });

            columns.Add(new Column
            {
                Header = "Remark 1",
                Property = "Remark1",
            });

            columns.Add(new Column
            {
                Header = "Remark 2",
                Property = "Remark2",
            });

            columns.Add(new Column
            {
                Header = "Errors",
                Property = "Errors",
                Editable = false,
            });

            return columns;
        }

        public static List<int> GetOperationalDashboardDisplayStatus()
        {
            var list = new List<int>
            {
                StatusPendingClarification,
                StatusClosed,
                StatusSuspectedDuplication,
                StatusRegistered,
                StatusPostUnderwritingReview,
                StatusApprovalByLimit,
                StatusApprovedByCeo,
                StatusApproved,
                StatusApprovedReferralClaim,
                StatusDeclinedReferralClaim,
                StatusClosedReferralClaim,
                StatusDeclinedByClaim,
                //StatusPendingDecline,
                StatusDeclined,
            };

            return list;
        }

        public static List<int> GetOutstandingCaseStatus()
        {
            var list = new List<int>
            {
                StatusReported,
                StatusProcessing,
                StatusSuccess,
                StatusFailed,
                StatusPendingClarification,
                StatusSuspectedDuplication,
                StatusRegistered,
                StatusPostUnderwritingReview,
                StatusPendingCeoApproval,
                StatusPendingCeoSignOff,
            };

            return list;
        }

        public static int GetStatusSlaDay(int key)
        {
            switch (key)
            {
                case StatusPendingClarification:
                    return Util.GetConfigInteger("CrStatusPendingClarification", 2);
                case StatusClosed:
                    return Util.GetConfigInteger("CrStatusClosed", 2);
                case StatusSuspectedDuplication:
                    return Util.GetConfigInteger("CrStatusSuspectedDuplication", 2);
                case StatusRegistered:
                    return Util.GetConfigInteger("CrStatusRegistered", 2);
                case StatusPostUnderwritingReview:
                    return Util.GetConfigInteger("CrStatusPostUnderwritingReview", 2);
                case StatusApprovalByLimit:
                    return Util.GetConfigInteger("CrStatusApprovalByLimit", 2);
                case StatusApprovedByCeo:
                    return Util.GetConfigInteger("CrStatusApprovedByCeo", 2);
                case StatusApproved:
                    return Util.GetConfigInteger("CrStatusApproved", 2);
                case StatusApprovedReferralClaim:
                    return Util.GetConfigInteger("CrStatusApprovedReferralClaim", 2);
                case StatusDeclinedReferralClaim:
                    return Util.GetConfigInteger("CrStatusDeclinedReferralClaim", 2);
                case StatusClosedReferralClaim:
                    return Util.GetConfigInteger("CrStatusClosedReferralClaim", 2);
                case StatusDeclinedByClaim:
                    return Util.GetConfigInteger("CrStatusDeclinedByClaim", 2);
                //case StatusPendingDecline:
                //    return Util.GetConfigInteger("CrStatusPendingDecline", 2);
                case StatusDeclined:
                    return Util.GetConfigInteger("CrStatusDeclined", 2);
                default:
                    return 0;
            }
        }

        public static List<string> GetTransactionTypeList(int? exclude = null)
        {
            List<string> list = new List<string>();
            for (int i = 1; i <= ClaimTransactionTypeMax; i++)
            {
                if (exclude.HasValue && exclude == i)
                    continue;

                list.Add(GetClaimTransactionTypeName(i));
            }
            return list;
        }

        public static List<Column> GetDirectRetroColumns()
        {
            var columns = new List<Column>
            {
                new Column
                {
                    Header = "ENTRY_NO",
                    Property = "EntryNo",
                },
                new Column
                {
                    Header = "CLAIM_ID",
                    Property = "ClaimId",
                },
                new Column
                {
                    Header = "CLAIM_TRANSACTION_TYPE",
                    Property = "ClaimTransactionType",
                },
                new Column
                {
                    Header = "POLICY_NUMBER",
                    Property = "PolicyNumber",
                },
                new Column
                {
                    Header = "INSURED_NAME",
                    Property = "InsuredName",
                },
                new Column
                {
                    Header = "INSURED_GENDER_CODE",
                    Property = "InsuredGenderCode",
                },
                new Column
                {
                    Header = "INSURED_TOBACCO_USE",
                    Property = "InsuredTobaccoUse",
                },
                new Column
                {
                    Header = "INSURED_DATE_OF_BIRTH",
                    Property = "InsuredDateOfBirth",
                },
                new Column
                {
                    Header = "ISSUE_DATE_POL",
                    Property = "IssueDatePol",
                },
                new Column
                {
                    Header = "DATE_OF_COMMENCEMENT",
                    Property = "ReinsEffDatePol",
                },
                new Column
                {
                    Header = "POLICY_EXPIRY_DATE",
                    Property = "PolicyExpiryDate",
                },
                new Column
                {
                    Header = "DATE_OF_EVENT",
                    Property = "DateOfEvent",
                },
                new Column
                {
                    Header = "CAUSE_OF_EVENT",
                    Property = "CauseOfEvent",
                },
                new Column
                {
                    Header = "CLAIM_RECOVERY_AMT",
                    Property = "ClaimRecoveryAmt",
                },
                new Column
                {
                    Header = "LATE_INTEREST",
                    Property = "LateInterest",
                },
                new Column
                {
                    Header = "AAR_PAYABLE",
                    Property = "AarPayable",
                },
                new Column
                {
                    Header = "SUM_INS",
                    Property = "SumIns",
                },
                new Column
                {
                    Header = "CEDING_PLAN_CODE",
                    Property = "CedingPlanCode",
                },
                new Column
                {
                    Header = "CLAIM_CODE",
                    Property = "ClaimCode",
                },
                new Column
                {
                    Header = "MLRE_BENEFIT_CODE",
                    Property = "MlreBenefitCode",
                },
                new Column
                {
                    Header = "MLRE_EVENT_CODE",
                    Property = "MlreEventCode",
                },
                new Column
                {
                    Header = "CEDING_CLAIM_TYPE",
                    Property = "CedingClaimType",
                },
                new Column
                {
                    Header = "CEDING_EVENT_CODE",
                    Property = "CedingEventCode",
                },
                new Column
                {
                    Header = "CEDANT_CLAIM_EVENT_CODE",
                    Property = "CedantClaimEventCode",
                },
                new Column
                {
                    Header = "CEDANT_CLAIM_TYPE",
                    Property = "CedantClaimType",
                },
                new Column
                {
                    Header = "CEDING_BENEFIT_RISK_CODE",
                    Property = "CedingBenefitRiskCode",
                },
                new Column
                {
                    Header = "CEDING_BENEFIT_TYPE_CODE",
                    Property = "CedingBenefitTypeCode",
                },
                new Column
                {
                    Header = "CEDING_TREATY_CODE",
                    Property = "CedingTreatyCode",
                },
                new Column
                {
                    Header = "CAMPAIGN_CODE",
                    Property = "CampaignCode",
                },
                new Column
                {
                    Header = "CEDING_COMPANY",
                    Property = "CedingCompany",
                },
                new Column
                {
                    Header = "TREATY_CODE",
                    Property = "TreatyCode",
                },
                new Column
                {
                    Header = "TREATY_TYPE",
                    Property = "TreatyType",
                },
                new Column
                {
                    Header = "REINS_BASIS_CODE",
                    Property = "ReinsBasisCode",
                },
                new Column
                {
                    Header = "FUNDS_ACCOUNTING_TYPE_CODE",
                    Property = "FundsAccountingTypeCode",
                },
                new Column
                {
                    Header = "SOA_QUARTER",
                    Property = "SoaQuarter",
                },
                new Column
                {
                    Header = "RISK_QUARTER",
                    Property = "RiskQuarter",
                },
                new Column
                {
                    Header = "RISK_PERIOD_YEAR",
                    Property = "RiskPeriodYear",
                },
                new Column
                {
                    Header = "RISK_PERIOD_MONTH",
                    Property = "RiskPeriodMonth",
                },
                new Column
                {
                    Header = "POLICY_TERM",
                    Property = "PolicyTerm",
                },
                new Column
                {
                    Header = "ANNUAL_RI_PREM",
                    Property = "AnnualRiPrem",
                },
                new Column
                {
                    Header = "TRANSACTION_DATE_WOP",
                    Property = "TransactionDateWop",
                },
                new Column
                {
                    Header = "SA_FACTOR",
                    Property = "SaFactor",
                },
                new Column
                {
                    Header = "CURRENCY_CODE",
                    Property = "CurrencyCode",
                },
                new Column
                {
                    Header = "CURRENCY_RATE",
                    Property = "CurrencyRate",
                },
                new Column
                {
                    Header = "FOREIGN_CLAIM_RECOVERY_AMT",
                    Property = "ForeignClaimRecoveryAmt",
                },
                new Column
                {
                    Header = "LAST_TRANSACTION_DATE",
                    Property = "LastTransactionDate",
                },
                new Column
                {
                    Header = "LAST_TRANSACTION_QUARTER",
                    Property = "LastTransactionQuarter",
                },
                new Column
                {
                    Header = "MFRS17_ANNUAL_COHORT",
                    Property = "Mfrs17AnnualCohort",
                },
                new Column
                {
                    Header = "MFRS17_CONTRACT_CODE",
                    Property = "Mfrs17ContractCode",
                },
                new Column
                {
                    Header = "POLICY_DURATION",
                    Property = "PolicyDuration",
                },
                new Column
                {
                    Header = "RETRO_PARTY_1",
                    Property = "RetroParty1",
                },
                new Column
                {
                    Header = "RETRO_SHARE_1",
                    Property = "RetroShare1",
                },
                new Column
                {
                    Header = "RETRO_RECOVERY_1",
                    Property = "RetroRecovery1",
                },
                new Column
                {
                    Header = "RETRO_PARTY_2",
                    Property = "RetroParty2",
                },
                new Column
                {
                    Header = "RETRO_SHARE_2",
                    Property = "RetroShare2",
                },
                new Column
                {
                    Header = "RETRO_RECOVERY_2",
                    Property = "RetroRecovery2",
                },
                new Column
                {
                    Header = "RETRO_PARTY_3",
                    Property = "RetroParty3",
                },
                new Column
                {
                    Header = "RETRO_SHARE_3",
                    Property = "RetroShare3",
                },
                new Column
                {
                    Header = "RETRO_RECOVERY_3",
                    Property = "RetroRecovery3",
                }
            };

            return columns;
        }

        public static List<Column> GetFinanceProvisioningColumns()
        {
            var columns = new List<Column>
            {
                new Column
                {
                    Header = "Has Red Flag",
                    Property = "HasRedFlag",
                },
                new Column
                {
                    Header = "Entry No",
                    Property = "EntryNo",
                },
                new Column
                {
                    Header = "SOA Quarter",
                    Property = "SOA Quarter",
                },
                new Column
                {
                    Header = "Claim ID",
                    Property = "ClaimId",
                },
                new Column
                {
                    Header = "Claim Transaction Type",
                    Property = "ClaimTransactionType",
                },
                new Column
                {
                    Header = "Refferal Case Indicator",
                    Property = "IsReferralCase",
                },
                new Column
                {
                    Header = "RI Data Warehouse",
                    Property = "RiDataWarehouseId",
                },
                new Column
                {
                    Header = "Record Type",
                    Property = "RecordType",
                },
                new Column
                {
                    Header = "Treaty Code",
                    Property = "TreatyCode",
                },
                new Column
                {
                    Header = "Policy No",
                    Property = "PolicyNumber",
                },
                new Column
                {
                    Header = "Ceding Company",
                    Property = "CedingCompany",
                },
                new Column
                {
                    Header = "Insured Name",
                    Property = "InsuredName",
                },
                new Column
                {
                    Header = "Insured Date of Birth",
                    Property = "InsuredDateOfBirth",
                },
                new Column
                {
                    Header = "Last Transaction Date",
                    Property = "LastTransactionDate",
                },
                new Column
                {
                    Header = "Date of Reported",
                    Property = "DateOfReported",
                },
                new Column
                {
                    Header = "Cedant Date of Notification",
                    Property = "CedantDateOfNotification",
                },
                new Column
                {
                    Header = "Date of Register",
                    Property = "DateOfRegister",
                },
                new Column
                {
                    Header = "Date of Commencement",
                    Property = "ReinsEffDatePol",
                },
                new Column
                {
                    Header = "Date of Event",
                    Property = "DateOfEvent",
                },
                new Column
                {
                    Header = "Policy Duration",
                    Property = "PolicyDuration",
                },
                new Column
                {
                    Header = "Target Date To Issue Invoice",
                    Property = "TargetDateToIssueInvoice",
                },
                new Column
                {
                    Header = "Sum Reinsured (MYR)",
                    Property = "SumIns",
                },
                new Column
                {
                    Header = "Cause of Event",
                    Property = "CauseOfEvent",
                },
                new Column
                {
                    Header = "Person In-Charge (Claims)",
                    Property = "PicClaimId",
                },
                new Column
                {
                    Header = "Person In-Charge (DA&A)",
                    Property = "PicDaaId",
                },
                new Column
                {
                    Header = "Claim Status",
                    Property = "ClaimStatus",
                },
                new Column
                {
                    Header = "Provision Status",
                    Property = "ProvisionStatus",
                },
                new Column
                {
                    Header = "Offset Status",
                    Property = "OffsetStatus",
                },
            };

            return columns;
        }

        public List<string> GetRedFlagWarnings()
        {
            List<string> warnings = new List<string>();
            if (!string.IsNullOrEmpty(RedFlagWarnings))
            {
                warnings = JsonConvert.DeserializeObject<List<string>>(RedFlagWarnings);
            }

            return warnings;
        }

        public void ReValidateRedFlagWarnings()
        {
            List<string> warnings = GetRedFlagWarnings();
            if (warnings.IsNullOrEmpty())
                return;

            List<string> required = new List<string>
            {
                StandardClaimDataOutputBo.GetPropertyNameByType(StandardClaimDataOutputBo.TypeReinsEffDatePol),
                StandardClaimDataOutputBo.GetPropertyNameByType(StandardClaimDataOutputBo.TypeDateOfEvent),
                StandardClaimDataOutputBo.GetPropertyNameByType(StandardClaimDataOutputBo.TypeMlreBenefitCode),
                StandardClaimDataOutputBo.GetPropertyNameByType(StandardClaimDataOutputBo.TypeTreatyType),
                StandardClaimDataOutputBo.GetPropertyNameByType(StandardClaimDataOutputBo.TypeClaimRecoveryAmt),
                StandardClaimDataOutputBo.GetPropertyNameByType(StandardClaimDataOutputBo.TypeReinsBasisCode),
                StandardClaimDataOutputBo.GetPropertyNameByType(StandardClaimDataOutputBo.TypeDateOfReported),
                StandardClaimDataOutputBo.GetPropertyNameByType(StandardClaimDataOutputBo.TypeClaimCode),
                StandardClaimDataOutputBo.GetPropertyNameByType(StandardClaimDataOutputBo.TypeFundsAccountingTypeCode),
                "RiDataWarehouseId"
            };

            foreach (string propertyName in required)
            {
                if (this.GetPropertyValue(propertyName) != null)
                {
                    DisplayNameAttribute attribute = this.GetAttributeFrom<DisplayNameAttribute>(propertyName);
                    string name = attribute != null ? attribute.DisplayName : propertyName;
                    string warning = string.Format(MessageBag.IsEmpty, name);
                    string warning2 = string.Format(MessageBag.IsEmpty, propertyName);

                    if (warnings.Contains(warning))
                        warnings.Remove(warning);
                    else if (warnings.Contains(warning2))
                        warnings.Remove(warning2);
                }
            }

            RedFlagWarnings = JsonConvert.SerializeObject(warnings);
            if (warnings.IsNullOrEmpty())
                HasRedFlag = false;
        }

        public void AddRedFlagWarning(string warning)
        {
            List<string> warnings = GetRedFlagWarnings();
            warnings.Add(warning);

            RedFlagWarnings = JsonConvert.SerializeObject(warnings);
            HasRedFlag = true;
        }

        public void RemoveRedFlagWarning(string warning)
        {
            List<string> warnings = GetRedFlagWarnings();
            if (warnings.Contains(warning))
            {
                warnings.Remove(warning);
            }

            RedFlagWarnings = JsonConvert.SerializeObject(warnings);
            if (warnings.IsNullOrEmpty())
                HasRedFlag = false;
        }

        public void ClearRedFlagWarning()
        {
            RedFlagWarnings = null;
            HasRedFlag = false;
        }

        public void SetRegisteredValues()
        {
            DateOfRegister = DateTime.Today;
            if (!ReinsEffDatePol.HasValue || !DateOfEvent.HasValue)
                return;

            PolicyDuration = (DateOfEvent.Value - ReinsEffDatePol.Value).Days;
        }

        public void ReverseClaim()
        {
            ClaimRecoveryAmt = 0;
            if (ProvisionStatus == ProvisionStatusProvisioned || ProvisionStatus == ProvisionStatusProvisioning)
            {
                ProvisionStatus = ProvisionStatusPendingReprovision;
            }
        }

        public static string ParseRedFlagWarning(string redFlag)
        {
            int type;
            if (!int.TryParse(redFlag, out type))
                return redFlag;

            return GetRedFlagWarnings(type);
        }

        public static bool CanApproveReject(int status)
        {
            int[] statuses = { StatusRegistered, StatusApprovedByCeo, StatusApprovalByLimit };
            return statuses.Contains(status);
        }

        public static void GetApproveRejectUrls(ref Dictionary<string, int> statuses, bool isReferralCase, string approveText = "APPROVE", string rejectText = "REJECT")
        {
            if (isReferralCase)
            {
                statuses.Add(approveText, StatusApprovedReferralClaim);
                statuses.Add(rejectText, StatusDeclinedReferralClaim);
            }
            else
            {
                statuses.Add(approveText, StatusApproved);
                statuses.Add(rejectText, StatusDeclinedByClaim);
            }
        }

        public bool ValidateApproveReject(int authUserId, bool hasCeoApprovalPower)
        {
            if (PicClaimId != authUserId)
            {
                BatchSelectionError = "You are not assigned to this claim";
                return false;
            }

            if (!CanApproveReject(ClaimStatus))
            {
                BatchSelectionError = "Claim cannot be approved/rejected";
                return false;
            }

            if (ClaimStatus == StatusApprovedByCeo && !hasCeoApprovalPower)
            {
                BatchSelectionError = "You are not authorized to approve this claim";
                return false;
            }

            return true;
        }

        public void SignOff(bool clearRedFlag = false)
        {
            //SignOffById = authUserId;
            SignOffDate = DateTime.Today;
            if (clearRedFlag)
                ClearRedFlagWarning();
        }

        public bool DuplicateParamsChanged(ClaimRegisterBo dbBo)
        {
            List<string> duplicateParams = new List<string>()
            {
                "PolicyNumber",
                "InsuredName",
                "CedingPlanCode",
                "ClaimRecoveryAmt",
                "DateOfEvent",
                "SoaQuarter",
                "InsuredDateOfBirth",
                "TreatyCode",
                "ClaimCode",
                "CauseOfEvent",
                "TransactionDateWop",
            };

            foreach (string param in duplicateParams)
            {
                if (this.GetPropertyValue(param) != dbBo.GetPropertyValue(param))
                    return true;
            }
            return false;
        }
    }
}
