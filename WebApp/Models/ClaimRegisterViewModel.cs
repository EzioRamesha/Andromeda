using BusinessObject;
using BusinessObject.Claims;
using BusinessObject.Identity;
using BusinessObject.RiDatas;
using DataAccess.Entities;
using DataAccess.Entities.Identity;
using Services;
using Services.Identity;
using Shared;
using Shared.Forms.Attributes;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class ClaimRegisterViewModel : IValidatableObject
    {
        public int Id { get; set; }

        public int? ClaimDataBatchId { get; set; }

        public int? ClaimDataId { get; set; }

        public int? SoaDataBatchId { get; set; }

        public ClaimDataBo ClaimDataBo { get; set; }

        [DisplayName("Claim Data Config")]
        public int? ClaimDataConfigId { get; set; }

        public ClaimDataConfigBo ClaimDataConfigBo { get; set; }

        [DisplayName("RI Data Warehouse")]
        public int? RiDataWarehouseId { get; set; }
        public int? ReferralRiDataId { get; set; }

        public RiDataWarehouseBo RiDataWarehouseBo { get; set; }

        [DisplayName("Claim Status")]
        public int ClaimStatus { get; set; }

        [DisplayName("Claim Decision")]
        public int? ClaimDecisionStatus { get; set; }

        [DisplayName("Claim Decision")]
        public string ClaimDecisionStatusName { get; set; }
        public int? ReferralClaimId { get; set; }

        [DisplayName("Claims Data Mapping")]
        public int? OriginalClaimRegisterId { get; set; }

        [DisplayName("Claim Reason (Decline/Adjustment)")]
        public int? ClaimReasonId { get; set; }

        public ClaimReasonBo ClaimReasonBo { get; set; }

        public ClaimReason ClaimReason { get; set; }

        [DisplayName("Claim Code")]
        public string ClaimCode { get; set; }

        [DisplayName("Person In-Charge (Claims)")]
        public int? PicClaimId { get; set; }

        public UserBo PicClaimBo { get; set; }

        [DisplayName("Next Person In-Charge")]
        public int? NextPicClaimId { get; set; }

        public User PicClaim { get; set; }

        [DisplayName("Person In-Charge (DA&A)")]
        public int? PicDaaId { get; set; }

        public UserBo PicDaaBo { get; set; }

        public User PicDaa { get; set; }

        [DisplayName("Provision Status")]
        public int ProvisionStatus { get; set; }

        [DisplayName("Direct Retro Provision Status")]
        public int DrProvisionStatus { get; set; }

        [DisplayName("Target Date To Issue Invoice")]
        public DateTime? TargetDateToIssueInvoice { get; set; }

        [DisplayName("Referral Case")]
        public bool IsReferralCase { get; set; }

        public bool HasRedFlag { get; set; }

        [DisplayName("Claim ID")]
        public string ClaimId { get; set; }

        [DisplayName("Mapping Status")]
        public int MappingStatus { get; set; }

        [DisplayName("Mapping Error")]
        public string MappingErrors { get; set; }

        [DisplayName("Processing Status")]
        public int ProcessingStatus { get; set; }

        [DisplayName("Processing Error")]
        public string ProcessingErrors { get; set; }

        [DisplayName("Duplication Check")]
        public int DuplicationCheckStatus { get; set; }

        [DisplayName("Claims Data Mapping Error")]
        public string OriginalClaimRegisterError { get; set; }

        [DisplayName("Post-Computation Status")]
        public int PostComputationStatus { get; set; }

        [DisplayName("Post-Computation Error")]
        public string PostComputationErrors { get; set; }

        [DisplayName("Post-Validation Status")]
        public int PostValidationStatus { get; set; }

        [DisplayName("Post-Validation Error")]
        public string PostValidationErrors { get; set; }

        public string Errors { get; set; }

        public string ProvisionErrors { get; set; }

        public string RedFlagWarnings { get; set; }

        public bool RequestUnderwriterReview { get; set; }

        public int? UnderwriterFeedback { get; set; }

        [DisplayName("Policy Number")]
        [StringLength(150)]
        public string PolicyNumber { get; set; }

        [DisplayName("Policy Term")]
        public double? PolicyTerm { get; set; }

        [DisplayName("Claim Recovery Amount")]
        public double? ClaimRecoveryAmt { get; set; }

        [DisplayName("Claim Transaction Type")]
        public string ClaimTransactionType { get; set; }

        [DisplayName("Treaty Code")]
        public string TreatyCode { get; set; }

        [DisplayName("Treaty Type")]
        public string TreatyType { get; set; }

        [DisplayName("Business Origin")]
        public string BusinessOrigin { get; set; }

        [DisplayName("AAR Payable")]
        public double? AarPayable { get; set; }

        [DisplayName("Annual RI Premium")]
        public double? AnnualRiPrem { get; set; }

        [DisplayName("Cause Of Event")]
        [StringLength(255)]
        public string CauseOfEvent { get; set; }

        [DisplayName("Cedant Claim Event Code")]
        [StringLength(30)]
        public string CedantClaimEventCode { get; set; }

        [DisplayName("Cedant Claim Type")]
        [StringLength(30)]
        public string CedantClaimType { get; set; }

        [DisplayName("Cedant Date Of Notification")]
        public DateTime? CedantDateOfNotification { get; set; }

        [DisplayName("Ceding Benefit Risk Code")]
        [StringLength(30)]
        public string CedingBenefitRiskCode { get; set; }

        [DisplayName("Ceding Benefit Type Code")]
        [StringLength(30)]
        public string CedingBenefitTypeCode { get; set; }

        [DisplayName("Ceding Claim Type")]
        [StringLength(30)]
        public string CedingClaimType { get; set; }

        [DisplayName("Ceding Company")]
        public string CedingCompany { get; set; }

        [DisplayName("Ceding Event Code")]
        [StringLength(30)]
        public string CedingEventCode { get; set; }

        [DisplayName("Ceding Plan Code")]
        [StringLength(30)]
        public string CedingPlanCode { get; set; }

        [DisplayName("Currency Rate")]
        public double? CurrencyRate { get; set; }

        [DisplayName("Currency Code")]
        public string CurrencyCode { get; set; }

        [DisplayName("Date Approved")]
        public DateTime? DateApproved { get; set; }

        [DisplayName("Date of Event")]
        public DateTime? DateOfEvent { get; set; }

        [DisplayName("Date of Register")]
        public DateTime? DateOfRegister { get; set; }

        [DisplayName("Date of Reported")]
        public DateTime? DateOfReported { get; set; }

        [DisplayName("Entry No.")]
        public string EntryNo { get; set; }

        [DisplayName("Ex Gratia")]
        public double? ExGratia { get; set; }

        [DisplayName("Foreign Claim Recovery Amount")]
        public double? ForeignClaimRecoveryAmt { get; set; }

        [DisplayName("Funds Accounting Type Code")]
        public string FundsAccountingTypeCode { get; set; }

        [DisplayName("Insured Date of Birth")]
        public DateTime? InsuredDateOfBirth { get; set; }

        [DisplayName("Insured Gender Code")]
        public string InsuredGenderCode { get; set; }

        [DisplayName("Insured Name")]
        [StringLength(128)]
        public string InsuredName { get; set; }

        [DisplayName("Insured Tobacco Use")]
        public string InsuredTobaccoUse { get; set; }

        [DisplayName("Last Transaction Date")]
        public DateTime? LastTransactionDate { get; set; }

        [DisplayName("Last Transaction Quarter")]
        public string LastTransactionQuarter { get; set; }

        [DisplayName("Late Interest")]
        public double? LateInterest { get; set; }

        [DisplayName("Layer1 Sum Reinsured")]
        public double? Layer1SumRein { get; set; }

        [DisplayName("MFRS17 Annual Cohort")]
        public int? Mfrs17AnnualCohort { get; set; }

        [DisplayName("MFRS17 Contract Code")]
        [StringLength(30)]
        public string Mfrs17ContractCode { get; set; }

        [DisplayName("MLRe Benefit Code")]
        public string MlreBenefitCode { get; set; }

        [DisplayName("MLRe Event Code")]
        public string MlreEventCode { get; set; }

        [DisplayName("MLRe Invoice Date")]
        public DateTime? MlreInvoiceDate { get; set; }

        [DisplayName("MLRe Invoice Number")]
        public string MlreInvoiceNumber { get; set; }

        [DisplayName("MLRe Retain Amount")]
        public double? MlreRetainAmount { get; set; }

        [DisplayName("MLRe Share")]
        public double? MlreShare { get; set; }

        [DisplayName("Offset Status")]
        public int OffsetStatus { get; set; }

        [DisplayName("Pending Provision Day")]
        public int? PendingProvisionDay { get; set; }

        [DisplayName("Policy Duration")]
        public int? PolicyDuration { get; set; }

        [DisplayName("Record Type")]
        public string RecordType { get; set; }

        [DisplayName("Reinsurance Basis Code")]
        public string ReinsBasisCode { get; set; }

        [DisplayName("Date of Commencement")]
        public DateTime? ReinsEffDatePol { get; set; }

        [DisplayName("Retro Party 1")]
        public string RetroParty1 { get; set; }

        [DisplayName("Retro Party 2")]
        public string RetroParty2 { get; set; }

        [DisplayName("Retro Party 3")]
        public string RetroParty3 { get; set; }

        [DisplayName("Retro Recovery 1")]
        public double? RetroRecovery1 { get; set; }

        [DisplayName("Retro Recovery 2")]
        public double? RetroRecovery2 { get; set; }

        [DisplayName("Retro Recovery 3")]
        public double? RetroRecovery3 { get; set; }

        [DisplayName("Retro Statement Date 1")]
        public DateTime? RetroStatementDate1 { get; set; }

        [DisplayName("Retro Statement Date 2")]
        public DateTime? RetroStatementDate2 { get; set; }

        [DisplayName("Retro Statement Date 3")]
        public DateTime? RetroStatementDate3 { get; set; }

        [DisplayName("Retro Statement Id 1")]
        public string RetroStatementId1 { get; set; }

        [DisplayName("Retro Statement Id 2")]
        public string RetroStatementId2 { get; set; }

        [DisplayName("Retro Statement Id 3")]
        public string RetroStatementId3 { get; set; }

        [DisplayName("Retro MLRe Share 1")]
        public double? RetroShare1 { get; set; }

        [DisplayName("Retro MLRe Share 2")]
        public double? RetroShare2 { get; set; }

        [DisplayName("Retro MLRe Share 3")]
        public double? RetroShare3 { get; set; }

        [DisplayName("Risk Period Month")]
        public int? RiskPeriodMonth { get; set; }

        [DisplayName("Risk Period Year")]
        public int? RiskPeriodYear { get; set; }

        [DisplayName("Risk Quarter")]
        public string RiskQuarter { get; set; }

        [DisplayName("SA Factor")]
        public double? SaFactor { get; set; }

        [DisplayName("SOA Quarter")]
        public string SoaQuarter { get; set; }

        [DisplayName("Sum Insured")]
        public double? SumIns { get; set; }

        [DisplayName("Temp A 1")]
        public double? TempA1 { get; set; }

        [DisplayName("Temp A 2")]
        public double? TempA2 { get; set; }

        [DisplayName("Temp D 1")]
        public DateTime? TempD1 { get; set; }

        [DisplayName("Temp D 2")]
        public DateTime? TempD2 { get; set; }

        [DisplayName("Temp I 1")]
        public int? TempI1 { get; set; }

        [DisplayName("Temp I 2")]
        public int? TempI2 { get; set; }

        [DisplayName("Temp S 1")]
        public string TempS1 { get; set; }

        [DisplayName("Temp S 2")]
        public string TempS2 { get; set; }

        [DisplayName("Transaction Date Wop")]
        public DateTime? TransactionDateWop { get; set; }

        [DisplayName("MLRe Reference No")]
        [StringLength(30)]
        public string MlreReferenceNo { get; set; }

        [DisplayName("Add Info")]
        [StringLength(30)]
        public string AddInfo { get; set; }

        [DisplayName("Remark 1")]
        [StringLength(128)]
        public string Remark1 { get; set; }

        [DisplayName("Remark 2")]
        [StringLength(128)]
        public string Remark2 { get; set; }

        [DisplayName("Policy Issue Date")]
        public DateTime? IssueDatePol { get; set; }

        [DisplayName("Policy Expiry Date")]
        public DateTime? PolicyExpiryDate { get; set; }

        [DisplayName("Claim Assessor")]
        public int? ClaimAssessorId { get; set; }

        public UserBo ClaimAssessorBo { get; set; }

        public User ClaimAssessor { get; set; }

        [DisplayName("Comment")]
        [StringLength(128)]
        public string Comment { get; set; }

        [DisplayName("Sign-Off by")]
        public int? SignOffById { get; set; }

        public UserBo SignOffByBo { get; set; }

        public User SignOffBy { get; set; }

        [DisplayName("Date Sign-Off")]
        public DateTime? SignOffDate { get; set; }

        [DisplayName("Ceding Treaty Code")]
        [StringLength(30)]
        public string CedingTreatyCode { get; set; }

        [DisplayName("Campaign Code")]
        [StringLength(10)]
        public string CampaignCode { get; set; }

        [DisplayName("Date Of Intimation")]
        public DateTime? DateOfIntimation { get; set; }

        // Double Str
        [DisplayName("Claim Recovery Amount")]
        [ValidateDouble]
        public string ClaimRecoveryAmtStr { get; set; }

        [DisplayName("AAR Payable")]
        [ValidateDouble]
        public string AarPayableStr { get; set; }

        [DisplayName("Annual RI Premium")]
        [ValidateDouble]
        public string AnnualRiPremStr { get; set; }

        [DisplayName("Currency Rate")]
        [ValidateDouble]
        public string CurrencyRateStr { get; set; }

        [DisplayName("Ex Gratia")]
        [ValidateDouble]
        public string ExGratiaStr { get; set; }

        [DisplayName("Foreign Claim Recovery Amount")]
        [ValidateDouble]
        public string ForeignClaimRecoveryAmtStr { get; set; }

        [DisplayName("Late Interest")]
        [ValidateDouble]
        public string LateInterestStr { get; set; }

        [DisplayName("Layer1 Sum Reinsured")]
        [ValidateDouble]
        public string Layer1SumReinStr { get; set; }

        [DisplayName("MLRe Retain Amount")]
        [ValidateDouble]
        public string MlreRetainAmountStr { get; set; }

        [DisplayName("MLRe Share")]
        [ValidateDouble]
        public string MlreShareStr { get; set; }

        [DisplayName("Retro Recovery 1")]
        [ValidateDouble]
        public string RetroRecovery1Str { get; set; }

        [DisplayName("Retro Recovery 2")]
        [ValidateDouble]
        public string RetroRecovery2Str { get; set; }

        [DisplayName("Retro Recovery 3")]
        [ValidateDouble]
        public string RetroRecovery3Str { get; set; }

        [DisplayName("Retro MLRe Share 1")]
        public string RetroShare1Str { get; set; }

        [DisplayName("Retro MLRe Share 2")]
        public string RetroShare2Str { get; set; }

        [DisplayName("Retro MLRe Share 3")]
        public string RetroShare3Str { get; set; }

        [DisplayName("SA Factor")]
        [ValidateDouble]
        public string SaFactorStr { get; set; }

        [DisplayName("Sum Insured")]
        [ValidateDouble]
        public string SumInsStr { get; set; }

        [DisplayName("Temp A 1")]
        [ValidateDouble]
        public string TempA1Str { get; set; }

        [DisplayName("Temp A 2")]
        [ValidateDouble]
        public string TempA2Str { get; set; }

        // DateTime Str
        [DisplayName("Target Date To Issue Invoice")]
        public string TargetDateToIssueInvoiceStr { get; set; }

        [DisplayName("Cedant Date Of Notification")]
        public string CedantDateOfNotificationStr { get; set; }

        [DisplayName("Date Approved")]
        public string DateApprovedStr { get; set; }

        [DisplayName("Date of Event")]
        public string DateOfEventStr { get; set; }

        [DisplayName("Date of Register")]
        public string DateOfRegisterStr { get; set; }

        [DisplayName("Date of Reported")]
        public string DateOfReportedStr { get; set; }

        [DisplayName("Insured Date of Birth")]
        public string InsuredDateOfBirthStr { get; set; }

        [DisplayName("Last Transaction Date")]
        public string LastTransactionDateStr { get; set; }

        [DisplayName("MLRe Invoice Date")]
        public string MlreInvoiceDateStr { get; set; }

        [DisplayName("Reinsurance Effective Date Policy")]
        public string ReinsEffDatePolStr { get; set; }

        [DisplayName("Retro Statement Date 1")]
        public string RetroStatementDate1Str { get; set; }

        [DisplayName("Retro Statement Date 2")]
        public string RetroStatementDate2Str { get; set; }

        [DisplayName("Retro Statement Date 3")]
        public string RetroStatementDate3Str { get; set; }

        [DisplayName("Temp D 1")]
        public string TempD1Str { get; set; }

        [DisplayName("Temp D 2")]
        public string TempD2Str { get; set; }

        [DisplayName("Transaction Date Wop")]
        public string TransactionDateWopStr { get; set; }

        [DisplayName("Policy Issue Date")]
        public string IssueDatePolStr { get; set; }

        [DisplayName("Policy Expiry Date")]
        public string PolicyExpiryDateStr { get; set; }

        [DisplayName("Date Sign-Off")]
        public string SignOffDateStr { get; set; }

        [DisplayName("Date Of Intimation")]
        public string DateOfIntimationStr { get; set; }

        // Ex Gratia
        [DisplayName("Comments on Chronology of Event")]
        public string EventChronologyComment { get; set; }

        [DisplayName("Recommendation by Claim Assessor")]
        public string ClaimAssessorRecommendation { get; set; }

        [DisplayName("Comments from Claims Committee")]
        [StringLength(255)]
        public string ClaimCommitteeComment1 { get; set; }

        [DisplayName("Comments from Claims Committee")]
        [StringLength(255)]
        public string ClaimCommitteeComment2 { get; set; }

        [DisplayName("Claims Committee User")]
        public int? ClaimCommitteeUser1Id { get; set; }

        [StringLength(128)]
        public string ClaimCommitteeUser1Name { get; set; }

        [DisplayName("Claims Committee User")]
        public int? ClaimCommitteeUser2Id { get; set; }

        [StringLength(128)]
        public string ClaimCommitteeUser2Name { get; set; }

        [DisplayName("Date Commented by Claims Committee")]
        public DateTime? ClaimCommitteeDateCommented1 { get; set; }

        [DisplayName("Date Commented by Claims Committee")]
        public string ClaimCommitteeDateCommented1Str { get; set; }

        [DisplayName("Date Commented by Claims Committee")]
        public DateTime? ClaimCommitteeDateCommented2 { get; set; }

        [DisplayName("Date Commented by Claims Committee")]
        public string ClaimCommitteeDateCommented2Str { get; set; }

        [DisplayName("Approval / Repudiation by CEO")]
        public int? CeoClaimReasonId { get; set; }

        public virtual ClaimReasonBo CeoClaimReasonBo { get; set; }

        [DisplayName("Comments by CEO")]
        [StringLength(255)]
        public string CeoComment { get; set; }

        [DisplayName("Updated By (On Behalf)")]
        public int? UpdatedOnBehalfById { get; set; }
        public UserBo UpdatedOnBehalfByBo { get; set; }

        public DateTime? UpdatedOnBehalfAt { get; set; }
        [DisplayName("Updated At")]
        public string UpdatedOnBehalfAtStr { get; set; }

        // Checklist
        public string Checklist { get; set; }

        public int ModuleId { get; set; }

        public bool IsClaim { get; set; }

        public ClaimRegisterViewModel()
        {
            Set();
            CurrencyCode = PickListDetailBo.CurrencyCodeMyr;
            CurrencyRate = 1;
            CurrencyRateStr = Util.DoubleToString(CurrencyRate);
        }

        public ClaimRegisterViewModel(bool isClaim = false)
        {
            Set(isClaim: isClaim);
        }

        public ClaimRegisterViewModel(ClaimRegisterBo claimRegisterBo, bool isClaim = false)
        {
            Set(claimRegisterBo, isClaim);
        }

        public void Set(ClaimRegisterBo claimRegisterBo = null, bool isClaim = false)
        {
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.ClaimRegister.ToString());
            ModuleId = moduleBo.Id;
            IsClaim = isClaim;
            if (claimRegisterBo != null)
            {
                Id = claimRegisterBo.Id;

                ClaimStatus = claimRegisterBo.ClaimStatus;
                ClaimDecisionStatus = claimRegisterBo.ClaimDecisionStatus;
                ClaimDecisionStatusName = ClaimRegisterBo.GetClaimDecisionStatusName(claimRegisterBo.ClaimDecisionStatus);
                MappingStatus = claimRegisterBo.MappingStatus;
                ProcessingStatus = claimRegisterBo.ProcessingStatus;
                DuplicationCheckStatus = claimRegisterBo.DuplicationCheckStatus;
                PostComputationStatus = claimRegisterBo.PostComputationStatus;
                PostValidationStatus = claimRegisterBo.PostValidationStatus;
                ProvisionStatus = claimRegisterBo.ProvisionStatus;
                DrProvisionStatus = claimRegisterBo.DrProvisionStatus;
                OffsetStatus = claimRegisterBo.OffsetStatus;
                Errors = claimRegisterBo.Errors;
                ProvisionErrors = claimRegisterBo.ProvisionErrors;
                RedFlagWarnings = claimRegisterBo.RedFlagWarnings;
                HasRedFlag = claimRegisterBo.HasRedFlag;

                // In Underwriting tab (Claims Department View Only)
                RequestUnderwriterReview = claimRegisterBo.RequestUnderwriterReview;
                UnderwriterFeedback = claimRegisterBo.UnderwriterFeedback;

                SoaQuarter = claimRegisterBo.SoaQuarter;
                CedingCompany = claimRegisterBo.CedingCompany;
                IsReferralCase = claimRegisterBo.IsReferralCase;
                TreatyCode = claimRegisterBo.TreatyCode;
                BusinessOrigin = claimRegisterBo.BusinessOrigin;
                ClaimDataConfigId = claimRegisterBo.ClaimDataConfigId;
                CedingEventCode = claimRegisterBo.CedingEventCode;
                ClaimTransactionType = claimRegisterBo.ClaimTransactionType;
                //ClaimTransactionType = IsClaim ? PickListDetailService.GetNameByPickListIdCode(PickListBo.ClaimTransactionType, claimRegisterBo.ClaimTransactionType) : claimRegisterBo.ClaimTransactionType;
                CedantClaimType = claimRegisterBo.CedantClaimType;
                PicDaaId = claimRegisterBo.PicDaaId;
                PicDaaBo = claimRegisterBo.PicDaaBo;
                PicClaimId = claimRegisterBo.PicClaimId;
                PicClaimBo = claimRegisterBo.PicClaimBo;
                MlreEventCode = claimRegisterBo.MlreEventCode;
                ReferralClaimId = claimRegisterBo.ReferralClaimId;
                OriginalClaimRegisterId = claimRegisterBo.OriginalClaimRegisterId;

                EntryNo = claimRegisterBo.EntryNo;
                ClaimId = claimRegisterBo.ClaimId;
                InsuredName = claimRegisterBo.InsuredName;
                PolicyNumber = claimRegisterBo.PolicyNumber;
                InsuredGenderCode = claimRegisterBo.InsuredGenderCode;
                InsuredTobaccoUse = claimRegisterBo.InsuredTobaccoUse;
                DateOfRegister = claimRegisterBo.DateOfRegister;
                CedantDateOfNotification = claimRegisterBo.CedantDateOfNotification;
                InsuredDateOfBirth = claimRegisterBo.InsuredDateOfBirth;
                ReinsEffDatePol = claimRegisterBo.ReinsEffDatePol;
                ReinsBasisCode = claimRegisterBo.ReinsBasisCode;
                MlreBenefitCode = claimRegisterBo.MlreBenefitCode;
                ClaimCode = claimRegisterBo.ClaimCode;
                ForeignClaimRecoveryAmt = claimRegisterBo.ForeignClaimRecoveryAmt;
                FundsAccountingTypeCode = claimRegisterBo.FundsAccountingTypeCode;
                ClaimRecoveryAmt = claimRegisterBo.ClaimRecoveryAmt;
                AnnualRiPrem = claimRegisterBo.AnnualRiPrem;
                RiskQuarter = claimRegisterBo.RiskQuarter;
                SaFactor = claimRegisterBo.SaFactor;
                LastTransactionDate = claimRegisterBo.LastTransactionDate;
                DateOfReported = claimRegisterBo.DateOfReported;
                DateOfEvent = claimRegisterBo.DateOfEvent;
                CauseOfEvent = claimRegisterBo.CauseOfEvent;
                TreatyType = claimRegisterBo.TreatyType;
                TransactionDateWop = claimRegisterBo.TransactionDateWop;
                MlreReferenceNo = claimRegisterBo.MlreReferenceNo;
                MlreShare = claimRegisterBo.MlreShare;
                DateApproved = claimRegisterBo.DateApproved;
                AddInfo = claimRegisterBo.AddInfo;
                Remark1 = claimRegisterBo.Remark1;
                Remark2 = claimRegisterBo.Remark2;
                CurrencyRate = claimRegisterBo.CurrencyRate;
                CurrencyCode = claimRegisterBo.CurrencyCode;
                CedingBenefitTypeCode = claimRegisterBo.CedingBenefitTypeCode;
                CedingPlanCode = claimRegisterBo.CedingPlanCode;
                CedingBenefitRiskCode = claimRegisterBo.CedingBenefitRiskCode;
                RetroParty1 = claimRegisterBo.RetroParty1;
                RetroRecovery1 = claimRegisterBo.RetroRecovery1;
                RetroParty2 = claimRegisterBo.RetroParty2;
                RetroRecovery2 = claimRegisterBo.RetroRecovery2;
                RetroParty3 = claimRegisterBo.RetroParty3;
                RetroRecovery3 = claimRegisterBo.RetroRecovery3;
                MlreRetainAmount = claimRegisterBo.MlreRetainAmount;
                LateInterest = claimRegisterBo.LateInterest;
                ExGratia = claimRegisterBo.ExGratia;
                Layer1SumRein = claimRegisterBo.Layer1SumRein;
                TempD1 = claimRegisterBo.TempD1;
                TempD2 = claimRegisterBo.TempD2;
                TempS1 = claimRegisterBo.TempS1;
                TempS2 = claimRegisterBo.TempS2;
                TempA1 = claimRegisterBo.TempA1;
                TempA2 = claimRegisterBo.TempA2;
                CedantClaimEventCode = claimRegisterBo.CedantClaimEventCode;
                IssueDatePol = claimRegisterBo.IssueDatePol;
                PolicyExpiryDate = claimRegisterBo.PolicyExpiryDate;
                PolicyTerm = claimRegisterBo.PolicyTerm;
                ClaimReasonId = claimRegisterBo.ClaimReasonId;
                PolicyDuration = claimRegisterBo.PolicyDuration;
                ClaimAssessorId = claimRegisterBo.ClaimAssessorId;
                Comment = claimRegisterBo.Comment;
                SignOffById = claimRegisterBo.SignOffById;
                SignOffByBo = claimRegisterBo.SignOffByBo;
                SignOffDate = claimRegisterBo.SignOffDate;
                CedingTreatyCode = claimRegisterBo.CedingTreatyCode;
                CampaignCode = claimRegisterBo.CampaignCode;
                DateOfIntimation = claimRegisterBo.DateOfIntimation;

                RiDataWarehouseId = claimRegisterBo.RiDataWarehouseId;
                ReferralRiDataId = claimRegisterBo.ReferralRiDataId;
                RiDataWarehouseBo = claimRegisterBo.RiDataWarehouseBo;

                RetroStatementDate1 = claimRegisterBo.RetroStatementDate1;
                RetroStatementDate2 = claimRegisterBo.RetroStatementDate2;
                RetroStatementDate3 = claimRegisterBo.RetroStatementDate3;
                RetroStatementId1 = claimRegisterBo.RetroStatementId1;
                RetroStatementId2 = claimRegisterBo.RetroStatementId2;
                RetroStatementId3 = claimRegisterBo.RetroStatementId3;

                // Ex Gratia
                EventChronologyComment = claimRegisterBo.EventChronologyComment;
                ClaimAssessorRecommendation = claimRegisterBo.ClaimAssessorRecommendation;
                ClaimCommitteeComment1 = claimRegisterBo.ClaimCommitteeComment1;
                ClaimCommitteeComment2 = claimRegisterBo.ClaimCommitteeComment2;
                ClaimCommitteeUser1Id = claimRegisterBo.ClaimCommitteeUser1Id;
                ClaimCommitteeUser1Name = claimRegisterBo.ClaimCommitteeUser1Name;
                ClaimCommitteeUser2Id = claimRegisterBo.ClaimCommitteeUser2Id;
                ClaimCommitteeUser2Name = claimRegisterBo.ClaimCommitteeUser2Name;
                ClaimCommitteeDateCommented1 = claimRegisterBo.ClaimCommitteeDateCommented1;
                ClaimCommitteeDateCommented2 = claimRegisterBo.ClaimCommitteeDateCommented2;
                CeoClaimReasonId = claimRegisterBo.CeoClaimReasonId;
                CeoComment = claimRegisterBo.CeoComment;
                UpdatedOnBehalfById = claimRegisterBo.UpdatedOnBehalfById;
                UpdatedOnBehalfAt = claimRegisterBo.UpdatedOnBehalfAt;

                // Checklist
                Checklist = claimRegisterBo.Checklist;

                // Not in page
                SoaDataBatchId = claimRegisterBo.SoaDataBatchId;
                ClaimDataBatchId = claimRegisterBo.ClaimDataBatchId;
                ClaimDataId = claimRegisterBo.ClaimDataId;
                MlreInvoiceNumber = claimRegisterBo.MlreInvoiceNumber;
                TargetDateToIssueInvoice = claimRegisterBo.TargetDateToIssueInvoice;
                AarPayable = claimRegisterBo.AarPayable;
                CedingClaimType = claimRegisterBo.CedingClaimType;
                LastTransactionQuarter = claimRegisterBo.LastTransactionQuarter;
                Mfrs17AnnualCohort = claimRegisterBo.Mfrs17AnnualCohort;
                Mfrs17ContractCode = claimRegisterBo.Mfrs17ContractCode;
                MlreInvoiceDate = claimRegisterBo.MlreInvoiceDate;
                PendingProvisionDay = claimRegisterBo.PendingProvisionDay;
                RecordType = claimRegisterBo.RecordType;
                RetroShare1 = claimRegisterBo.RetroShare1;
                RetroShare2 = claimRegisterBo.RetroShare2;
                RetroShare3 = claimRegisterBo.RetroShare3;
                RiskPeriodMonth = claimRegisterBo.RiskPeriodMonth;
                RiskPeriodYear = claimRegisterBo.RiskPeriodYear;
                SumIns = claimRegisterBo.SumIns;
                TempI1 = claimRegisterBo.TempI1;
                TempI2 = claimRegisterBo.TempI2;

                int? precision = isClaim ? 2 : (int?)null;
                ClaimRecoveryAmtStr = Util.DoubleToString(claimRegisterBo.ClaimRecoveryAmt, 2);
                AarPayableStr = Util.DoubleToString(claimRegisterBo.AarPayable, precision);
                AnnualRiPremStr = Util.DoubleToString(claimRegisterBo.AnnualRiPrem, precision);
                CurrencyRateStr = Util.DoubleToString(claimRegisterBo.CurrencyRate);
                ExGratiaStr = Util.DoubleToString(claimRegisterBo.ExGratia, precision);
                ForeignClaimRecoveryAmtStr = Util.DoubleToString(claimRegisterBo.ForeignClaimRecoveryAmt, precision);
                LateInterestStr = Util.DoubleToString(claimRegisterBo.LateInterest, precision);
                Layer1SumReinStr = Util.DoubleToString(claimRegisterBo.Layer1SumRein, precision);
                MlreRetainAmountStr = Util.DoubleToString(claimRegisterBo.MlreRetainAmount, precision);
                MlreShareStr = Util.DoubleToString(claimRegisterBo.MlreShare, precision);
                RetroRecovery1Str = Util.DoubleToString(claimRegisterBo.RetroRecovery1, 2);
                RetroRecovery2Str = Util.DoubleToString(claimRegisterBo.RetroRecovery2, 2);
                RetroRecovery3Str = Util.DoubleToString(claimRegisterBo.RetroRecovery3, 2);
                RetroShare1Str = Util.DoubleToString(claimRegisterBo.RetroShare1, precision);
                RetroShare2Str = Util.DoubleToString(claimRegisterBo.RetroShare2, precision);
                RetroShare3Str = Util.DoubleToString(claimRegisterBo.RetroShare3, precision);
                SaFactorStr = Util.DoubleToString(claimRegisterBo.SaFactor, precision);
                SumInsStr = Util.DoubleToString(claimRegisterBo.SumIns, precision);
                TempA1Str = Util.DoubleToString(claimRegisterBo.TempA1, precision);
                TempA2Str = Util.DoubleToString(claimRegisterBo.TempA2, precision);

                TargetDateToIssueInvoiceStr = claimRegisterBo.TargetDateToIssueInvoice?.ToString(Util.GetDateFormat());
                CedantDateOfNotificationStr = claimRegisterBo.CedantDateOfNotification?.ToString(Util.GetDateFormat());
                DateApprovedStr = claimRegisterBo.DateApproved?.ToString(Util.GetDateFormat());
                DateOfEventStr = claimRegisterBo.DateOfEvent?.ToString(Util.GetDateFormat());
                DateOfRegisterStr = claimRegisterBo.DateOfRegister?.ToString(Util.GetDateFormat());
                DateOfReportedStr = claimRegisterBo.DateOfReported?.ToString(Util.GetDateFormat());
                InsuredDateOfBirthStr = claimRegisterBo.InsuredDateOfBirth?.ToString(Util.GetDateFormat());
                LastTransactionDateStr = claimRegisterBo.LastTransactionDate?.ToString(Util.GetDateFormat());
                MlreInvoiceDateStr = claimRegisterBo.MlreInvoiceDate?.ToString(Util.GetDateFormat());
                ReinsEffDatePolStr = claimRegisterBo.ReinsEffDatePol?.ToString(Util.GetDateFormat());
                RetroStatementDate1Str = claimRegisterBo.RetroStatementDate1?.ToString(Util.GetDateFormat());
                RetroStatementDate2Str = claimRegisterBo.RetroStatementDate2?.ToString(Util.GetDateFormat());
                RetroStatementDate3Str = claimRegisterBo.RetroStatementDate3?.ToString(Util.GetDateFormat());
                TempD1Str = claimRegisterBo.TempD1?.ToString(Util.GetDateFormat());
                TempD2Str = claimRegisterBo.TempD2?.ToString(Util.GetDateFormat());
                TransactionDateWopStr = claimRegisterBo.TransactionDateWop?.ToString(Util.GetDateFormat());
                IssueDatePolStr = claimRegisterBo.IssueDatePol?.ToString(Util.GetDateFormat());
                PolicyExpiryDateStr = claimRegisterBo.PolicyExpiryDate?.ToString(Util.GetDateFormat());
                SignOffDateStr = claimRegisterBo.SignOffDate?.ToString(Util.GetDateFormat());
                ClaimCommitteeDateCommented1Str = claimRegisterBo.ClaimCommitteeDateCommented1?.ToString(Util.GetDateFormat());
                ClaimCommitteeDateCommented2Str = claimRegisterBo.ClaimCommitteeDateCommented2?.ToString(Util.GetDateFormat());
                DateOfIntimationStr = claimRegisterBo.DateOfIntimation?.ToString(Util.GetDateFormat());
                UpdatedOnBehalfAtStr = claimRegisterBo.UpdatedOnBehalfAt?.ToString(Util.GetDateFormat());
            }
            else
            {
                ClaimStatus = IsClaim ? ClaimRegisterBo.StatusRegistered : ClaimRegisterBo.StatusReported;
                ProvisionStatus = ClaimRegisterBo.ProvisionStatusPending;
                DrProvisionStatus = ClaimRegisterBo.DrProvisionStatusPending;
                ClaimTransactionType = IsClaim ? PickListDetailBo.ClaimTransactionTypeAdjustment : null;
            }
        }

        public ClaimRegisterBo FormBo(int authUserId, ClaimRegisterBo bo = null)
        {
            bool isNew = bo == null;
            if (isNew)
            {
                bo = new ClaimRegisterBo()
                {
                    ClaimStatus = IsClaim ? ClaimRegisterBo.StatusRegistered : ClaimRegisterBo.StatusReported,
                    CreatedById = authUserId,
                    MappingStatus = ClaimRegisterBo.MappingStatusSuccess
                };

                if (IsClaim)
                {
                    bo.ClaimTransactionType = PickListDetailBo.ClaimTransactionTypeAdjustment;
                }
                else
                {
                    bo.ClaimTransactionType = ClaimTransactionType;
                }
            }
            else
            {
                bo.ClaimStatus = ClaimStatus;
            }

            // Header Section
            //bo.IsReferralCase = IsReferralCase; Cannot be set on front end
            bo.OriginalClaimRegisterId = OriginalClaimRegisterId;

            // General Tab
            //bo.EntryNo = EntryNo; Cannot be set on front end
            //bo.ClaimId = ClaimId; Cannot be set on front end
            //bo.ReferralClaimId = ReferralClaimId; Cannot be set on front end
            bo.PolicyNumber = PolicyNumber;
            bo.InsuredName = InsuredName;
            bo.InsuredDateOfBirth = Util.GetParseDateTime(InsuredDateOfBirthStr);
            bo.InsuredGenderCode = InsuredGenderCode;
            bo.InsuredTobaccoUse = InsuredTobaccoUse;
            bo.ClaimCode = ClaimCode;
            bo.CauseOfEvent = CauseOfEvent;
            bo.AddInfo = AddInfo;
            bo.Remark1 = Remark1;
            bo.Remark2 = Remark2;
            bo.DateOfIntimation = Util.GetParseDateTime(DateOfIntimationStr);

            // RI Data Tab
            bo.RiDataWarehouseId = RiDataWarehouseId;

            if (IsClaim)
            {
                bo.RecordType = RecordType;
                //bo.RequestUnderwriterReview = RequestUnderwriterReview;
                if (bo.ClaimStatus == ClaimRegisterBo.StatusPostUnderwritingReview)
                    bo.UnderwriterFeedback = UnderwriterFeedback;

                bool isExGratia = bo.RecordType == PickListDetailBo.RecordTypeExGratia;
                bo.EventChronologyComment = isExGratia ? EventChronologyComment : null;
                bo.ClaimAssessorRecommendation = isExGratia ? ClaimAssessorRecommendation : null;
                bo.ClaimCommitteeComment1 = isExGratia ? ClaimCommitteeComment1 : null;
                bo.ClaimCommitteeComment2 = isExGratia ? ClaimCommitteeComment2 : null;
                bo.ClaimCommitteeUser1Id = isExGratia ? ClaimCommitteeUser1Id : null;
                bo.ClaimCommitteeUser1Name = isExGratia ? ClaimCommitteeUser1Name : null;
                bo.ClaimCommitteeUser2Id = isExGratia ? ClaimCommitteeUser2Id : null;
                bo.ClaimCommitteeUser2Name = isExGratia ? ClaimCommitteeUser2Name : null;
                bo.ClaimCommitteeDateCommented1 = isExGratia ? Util.GetParseDateTime(ClaimCommitteeDateCommented1Str) : null;
                bo.ClaimCommitteeDateCommented2 = isExGratia ? Util.GetParseDateTime(ClaimCommitteeDateCommented2Str) : null;
                bo.CeoClaimReasonId = isExGratia ? CeoClaimReasonId : null;
                bo.CeoComment = isExGratia ? CeoComment : null;
                bo.UpdatedOnBehalfById = isExGratia ? UpdatedOnBehalfById : null;
                bo.UpdatedOnBehalfAt = isExGratia ? Util.GetParseDateTime(UpdatedOnBehalfAtStr) : null;

                bo.PicClaimId = PicClaimId;

                bo.ClaimAssessorId = ClaimAssessorId;
                bo.ClaimReasonId = ClaimReasonId;

                bo.SignOffById = SignOffById;
                bo.SignOffDate = Util.GetParseDateTime(SignOffDateStr);
            }

            if (!IsClaim || isNew)
            {
                if (!isNew)
                    bo.ClaimTransactionType = ClaimTransactionType;
                bo.SoaQuarter = SoaQuarter;
                bo.CedingCompany = CedingCompany;
                bo.TreatyCode = TreatyCode;
                bo.ProvisionStatus = ProvisionStatus;
                bo.DrProvisionStatus = DrProvisionStatus;
                bo.ClaimDataConfigId = ClaimDataConfigId;
                bo.CedingEventCode = CedingEventCode;
                bo.CedantClaimType = CedantClaimType;
                bo.MlreEventCode = MlreEventCode;

                bo.ClaimDataConfigId = ClaimDataConfigId;
                bo.SoaDataBatchId = SoaDataBatchId;
                bo.PicDaaId = PicDaaId;

                bo.DateOfRegister = Util.GetParseDateTime(DateOfRegisterStr);
                bo.CedantDateOfNotification = Util.GetParseDateTime(CedantDateOfNotificationStr);
                bo.ReinsEffDatePol = Util.GetParseDateTime(ReinsEffDatePolStr);
                bo.ReinsBasisCode = ReinsBasisCode;
                bo.MlreBenefitCode = MlreBenefitCode;
                bo.ForeignClaimRecoveryAmt = Util.StringToDouble(ForeignClaimRecoveryAmtStr);
                bo.FundsAccountingTypeCode = FundsAccountingTypeCode;
                bo.ClaimRecoveryAmt = Util.StringToDouble(ClaimRecoveryAmtStr, precision: 2);
                bo.AnnualRiPrem = Util.StringToDouble(AnnualRiPremStr);
                bo.RiskQuarter = RiskQuarter;
                bo.SaFactor = Util.StringToDouble(SaFactorStr);
                bo.LastTransactionDate = Util.GetParseDateTime(LastTransactionDateStr);
                bo.RiskPeriodYear = RiskPeriodYear;
                bo.RiskPeriodMonth = RiskPeriodMonth;
                bo.DateOfEvent = Util.GetParseDateTime(DateOfEventStr);
                bo.DateOfReported = Util.GetParseDateTime(DateOfReportedStr);
                bo.TreatyType = TreatyType;
                bo.AarPayable = Util.StringToDouble(AarPayableStr);
                bo.CedingClaimType = CedingClaimType;
                bo.MlreShare = Util.StringToDouble(MlreShareStr);
                bo.TransactionDateWop = Util.GetParseDateTime(TransactionDateWopStr);
                bo.MlreReferenceNo = MlreReferenceNo;
                bo.DateApproved = Util.GetParseDateTime(DateApprovedStr);
                bo.CurrencyRate = Util.StringToDouble(CurrencyRateStr);
                bo.CurrencyCode = CurrencyCode;
                bo.CedingBenefitTypeCode = CedingBenefitTypeCode;
                bo.CedingBenefitRiskCode = CedingBenefitRiskCode;
                bo.CedingPlanCode = CedingPlanCode;
                bo.RetroParty1 = RetroParty1;
                bo.RetroParty2 = RetroParty2;
                bo.RetroParty3 = RetroParty3;
                bo.RetroRecovery1 = Util.StringToDouble(RetroRecovery1Str, precision: 2);
                bo.RetroRecovery2 = Util.StringToDouble(RetroRecovery2Str, precision: 2);
                bo.RetroRecovery3 = Util.StringToDouble(RetroRecovery3Str, precision: 2);
                bo.MlreRetainAmount = Util.StringToDouble(MlreRetainAmountStr);
                bo.LateInterest = Util.StringToDouble(LateInterestStr);
                bo.ExGratia = Util.StringToDouble(ExGratiaStr);
                bo.Layer1SumRein = Util.StringToDouble(Layer1SumReinStr);
                bo.Mfrs17AnnualCohort = Mfrs17AnnualCohort;
                bo.Mfrs17ContractCode = Mfrs17ContractCode;
                bo.TempA1 = Util.StringToDouble(TempA1Str);
                bo.TempA2 = Util.StringToDouble(TempA2Str);
                bo.TempD1 = Util.GetParseDateTime(TempD1Str);
                bo.TempD2 = Util.GetParseDateTime(TempD2Str);
                bo.TempS1 = TempS1;
                bo.TempS2 = TempS2;
                bo.CedantClaimEventCode = CedantClaimEventCode;
                bo.IssueDatePol = Util.GetParseDateTime(IssueDatePolStr);
                bo.PolicyExpiryDate = Util.GetParseDateTime(PolicyExpiryDateStr);
                bo.PolicyTerm = PolicyTerm;
                bo.PolicyDuration = PolicyDuration;
                bo.Comment = Comment;
                bo.CedingTreatyCode = CedingTreatyCode;
                bo.CampaignCode = CampaignCode;
                bo.TargetDateToIssueInvoice = Util.GetParseDateTime(TargetDateToIssueInvoiceStr);
            }


            // Values below are commented to prevent updating null value

            // Cannot be edited
            //bo.SignOffById = SignOffById;
            //bo.SignOffDate = Util.GetParseDateTime(SignOffDateStr);
            //bo.Errors = Errors;
            //bo.ProvisionErrors = ProvisionErrors;
            //bo.RetroStatementDate1 = Util.GetParseDateTime(RetroStatementDate1Str);
            //bo.RetroStatementDate2 = Util.GetParseDateTime(RetroStatementDate2Str);
            //bo.RetroStatementDate3 = Util.GetParseDateTime(RetroStatementDate3Str);
            //bo.RetroStatementId1 = RetroStatementId1;
            //bo.RetroStatementId2 = RetroStatementId2;
            //bo.RetroStatementId3 = RetroStatementId3;

            // Not in page
            //bo.ClaimDataBatchId = ClaimDataBatchId;
            //bo.ClaimDataId = ClaimDataId;
            //bo.LastTransactionQuarter = LastTransactionQuarter;
            //bo.MlreInvoiceDate = Util.GetParseDateTime(MlreInvoiceDateStr);
            //bo.MlreInvoiceNumber = MlreInvoiceNumber;
            //bo.PendingProvisionDay = PendingProvisionDay;
            //bo.RetroShare1 = RetroShare1;
            //bo.RetroShare2 = RetroShare2;
            //bo.RetroShare3 = RetroShare3;
            //bo.SumIns = Util.StringToDouble(SumInsStr);
            //bo.TempI1 = TempI1;
            //bo.TempI2 = TempI2;

            bo.UpdatedById = authUserId;

            return bo;
        }

        public static Expression<Func<ClaimRegister, ClaimRegisterViewModel>> Expression()
        {
            return entity => new ClaimRegisterViewModel
            {
                Id = entity.Id,
                //ClaimDataBatchId = entity.ClaimDataBatchId,
                //ClaimDataId = entity.ClaimDataId,
                //ClaimDataConfigId = entity.ClaimDataConfigId,
                RiDataWarehouseId = entity.RiDataWarehouseId,
                ClaimStatus = entity.ClaimStatus,
                //OriginalClaimRegisterId = entity.OriginalClaimRegisterId,
                //ClaimReasonId = entity.ClaimReasonId,
                //ClaimCode = entity.ClaimCode,
                PicDaaId = entity.PicDaaId,
                PicDaa = entity.PicDaa,
                PicClaimId = entity.PicClaimId,
                PicClaim = entity.PicClaim,
                ProvisionStatus = entity.ProvisionStatus,
                OffsetStatus = entity.OffsetStatus,
                TargetDateToIssueInvoice = entity.TargetDateToIssueInvoice,
                IsReferralCase = entity.IsReferralCase,
                ClaimId = entity.ClaimId,
                //MappingStatus = entity.MappingStatus,
                //ProcessingStatus = entity.ProcessingStatus,
                DuplicationCheckStatus = entity.DuplicationCheckStatus,
                //PostComputationStatus = entity.PostComputationStatus,
                //PostValidationStatus = entity.PostValidationStatus,
                //Errors = entity.Errors,
                //ProvisionErrors = entity.ProvisionErrors,
                //RequestUnderwriterReview = entity.RequestUnderwriterReview,
                //UnderwriterFeedback = entity.UnderwriterFeedback,
                HasRedFlag = entity.HasRedFlag,

                PolicyNumber = entity.PolicyNumber,
                //PolicyTerm = entity.PolicyTerm,
                ClaimRecoveryAmt = entity.ClaimRecoveryAmt,
                ClaimTransactionType = entity.ClaimTransactionType,
                TreatyCode = entity.TreatyCode,
                //TreatyType = entity.TreatyType,
                //AarPayable = entity.AarPayable,
                //AnnualRiPrem = entity.AnnualRiPrem,
                CauseOfEvent = entity.CauseOfEvent,
                //CedantClaimEventCode = entity.CedantClaimEventCode,
                //CedantClaimType = entity.CedantClaimType,
                CedantDateOfNotification = entity.CedantDateOfNotification,
                //CedingBenefitRiskCode = entity.CedingBenefitRiskCode,
                //CedingBenefitTypeCode = entity.CedingBenefitTypeCode,
                //CedingClaimType = entity.CedingClaimType,
                CedingCompany = entity.CedingCompany,
                //CedingEventCode = entity.CedingEventCode,
                //CedingPlanCode = entity.CedingPlanCode,
                //CurrencyRate = entity.CurrencyRate,
                //DateApproved = entity.DateApproved,
                DateOfEvent = entity.DateOfEvent,
                DateOfRegister = entity.DateOfRegister,
                DateOfReported = entity.DateOfReported,
                EntryNo = entity.EntryNo,
                //ExGratia = entity.ExGratia,
                //ForeignClaimRecoveryAmt = entity.ForeignClaimRecoveryAmt,
                //FundsAccountingTypeCode = entity.FundsAccountingTypeCode,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                //InsuredGenderCode = entity.InsuredGenderCode,
                InsuredName = entity.InsuredName,
                //InsuredTobaccoUse = entity.InsuredTobaccoUse,
                LastTransactionDate = entity.LastTransactionDate,
                //LastTransactionQuarter = entity.LastTransactionQuarter,
                //LateInterest = entity.LateInterest,
                Layer1SumRein = entity.Layer1SumRein,
                //Mfrs17AnnualCohort = entity.Mfrs17AnnualCohort,
                //Mfrs17ContractCode = entity.Mfrs17ContractCode,
                //MlreBenefitCode = entity.MlreBenefitCode,
                //MlreEventCode = entity.MlreEventCode,
                //MlreInvoiceDate = entity.MlreInvoiceDate,
                //MlreInvoiceNumber = entity.MlreInvoiceNumber,
                //MlreRetainAmount = entity.MlreRetainAmount,
                //MlreShare = entity.MlreShare,
                //PendingProvisionDay = entity.PendingProvisionDay,
                PolicyDuration = entity.PolicyDuration,
                RecordType = entity.RecordType,
                //ReinsBasisCode = entity.ReinsBasisCode,
                ReinsEffDatePol = entity.ReinsEffDatePol, // Date of Commencement
                //RetroParty1 = entity.RetroParty1,
                //RetroParty2 = entity.RetroParty2,
                //RetroParty3 = entity.RetroParty3,
                //RetroRecovery1 = entity.RetroRecovery1,
                //RetroRecovery2 = entity.RetroRecovery2,
                //RetroRecovery3 = entity.RetroRecovery3,
                //RetroStatementDate1 = entity.RetroStatementDate1,
                //RetroStatementDate2 = entity.RetroStatementDate2,
                //RetroStatementDate3 = entity.RetroStatementDate3,
                //RetroStatementId1 = entity.RetroStatementId1,
                //RetroStatementId2 = entity.RetroStatementId2,
                //RetroStatementId3 = entity.RetroStatementId3,
                //RetroShare1 = entity.RetroShare1,
                //RetroShare2 = entity.RetroShare2,
                //RetroShare3 = entity.RetroShare3,
                //RiskPeriodMonth = entity.RiskPeriodMonth,
                //RiskPeriodYear = entity.RiskPeriodYear,
                //RiskQuarter = entity.RiskQuarter,
                //SaFactor = entity.SaFactor,
                SoaQuarter = entity.SoaQuarter,
                //SumIns = entity.SumIns,
                //TempA1 = entity.TempA1,
                //TempA2 = entity.TempA2,
                //TempD1 = entity.TempD1,
                //TempD2 = entity.TempD2,
                //TempI1 = entity.TempI1,
                //TempI2 = entity.TempI2,
                //TempS1 = entity.TempS1,
                //TempS2 = entity.TempS2,
                //TransactionDateWop = entity.TransactionDateWop,
                //MlreReferenceNo = entity.MlreReferenceNo,
                //AddInfo = entity.AddInfo,
                //Remark1 = entity.Remark1,
                //Remark2 = entity.Remark2,
                //IssueDatePol = entity.IssueDatePol,
                //PolicyExpiryDate = entity.PolicyExpiryDate,
                //ClaimAssessorId = entity.ClaimAssessorId,
                //ClaimAssessor = entity.ClaimAssessor,
                //Comment = entity.Comment,
                //SignOffById = entity.SignOffById,
                //SignOffBy = entity.SignOffBy,
                //SignOffDate = entity.SignOffDate
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            List<string> requiredFields = new List<string>()
            {
                "ClaimTransactionType"
            };

            if (IsClaim)
            {
                requiredFields.Add("RecordType");
            }
            else
            {
                if (ClaimTransactionType != PickListDetailBo.ClaimTransactionTypeBulk)
                    requiredFields.Add("ClaimDataConfigId");

                //requiredFields.Add("CedingEventCode");
                //requiredFields.Add("CedantClaimType");
                //requiredFields.Add("SoaQuarter");
                requiredFields.Add("CedingCompany");
                requiredFields.Add("TreatyCode");
                if (Id != 0)
                    requiredFields.Add("TreatyType");

                if (CedingEventCode == null && CedingClaimType == null)
                {
                    results.Add(new ValidationResult(string.Format("At least 1 of Ceding Event Code and Cedant Claim Type has to be filled"), new[] { nameof(CedingEventCode) }));
                    results.Add(new ValidationResult(string.Format("At least 1 of Ceding Event Code and Cedant Claim Type has to be filled"), new[] { nameof(CedingClaimType) }));
                }

                if (IsReferralCase && SoaDataBatchId != null)
                {
                    results.Add(new ValidationResult(string.Format("SOA Data cannot be linked for claims registered from referral"), new[] { nameof(SoaDataBatchId) }));
                }
            }

            if (ClaimTransactionType == PickListDetailBo.ClaimTransactionTypeAdjustment)
                requiredFields.Add("OriginalClaimRegisterId");

            foreach (string propertyName in requiredFields)
            {
                if (this.GetPropertyValue(propertyName) != null)
                    continue;

                string displayName = this.GetAttributeFrom<DisplayNameAttribute>(propertyName).DisplayName;
                results.Add(new ValidationResult(string.Format(MessageBag.Required, displayName), new[] { propertyName }));
            }

            return results;
        }
    }
}