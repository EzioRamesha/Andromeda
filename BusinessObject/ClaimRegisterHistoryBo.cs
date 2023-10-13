using BusinessObject.Claims;
using BusinessObject.Identity;
using BusinessObject.RiDatas;
using BusinessObject.SoaDatas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ClaimRegisterHistoryBo
    {
        public int Id { get; set; }

        public int? CutOffId { get; set; }
        public CutOffBo CutOffBo { get; set; }

        public int ClaimRegisterId { get; set; }
        public virtual ClaimRegisterBo ClaimRegister { get; set; }

        public int? ClaimDataBatchId { get; set; }
        public ClaimDataBatchBo ClaimDataBatchBo { get; set; }

        public int? ClaimDataId { get; set; }
        public ClaimDataBo ClaimDataBo { get; set; }

        public int? ClaimDataConfigId { get; set; }
        public ClaimDataConfigBo ClaimDataConfigBo { get; set; }

        public int? SoaDataBatchId { get; set; }
        public SoaDataBatchBo SoaDataBatchBo { get; set; }

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
        public string ClaimDecisionStatusName { get; set; }

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
        public DateTime? DateOfIntimation { get; set; }

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

        public string DateOfIntimationStr { get; set; }

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

        // Checklist
        public string Checklist { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string StatusName { get; set; }

        // To use in export
        public int FinanceProvisioningStatus { get; set; }
        public int SortIndex { get; set; }
        public DateTime? ProvisionAt { get; set; }
        public DateTime? CutOffAt { get; set; }
    }
}
