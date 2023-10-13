using BusinessObject;
using BusinessObject.Claims;
using BusinessObject.Identity;
using BusinessObject.Retrocession;
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
    public class PerLifeClaimRetroDataViewModel
    {
        public int Id { get; set; }
        public int ModuleId { get; set; }


        [DisplayName("Per Life Claim Data ID")]
        public int PerLifeClaimDataId { get; set; }
        public virtual PerLifeClaimDataBo PerLifeClaimDataBo { get; set; }


        [DisplayName("MLRe Share")]
        public double? MlreShare { get; set; }

        [DisplayName("Retro Claim Recovery Amount")]
        public double? RetroClaimRecoveryAmount { get; set; }

        [DisplayName("Late Interest")]
        public double? LateInterest { get; set; }

        [DisplayName("Ex Gratia")]
        public double? ExGratia { get; set; }

        [DisplayName("RETRO_RECOVERY_ID")]
        public int? RetroRecoveryId { get; set; }

        [DisplayName("RETRO_TREATY")]
        public int? RetroTreatyId { get; set; }
        public virtual RetroTreatyBo RetroTreatyBo { get; set; }

        [DisplayName("RETRO_RATIO")]
        public double? RetroRatio { get; set; }

        [DisplayName("AAR")]
        public double? Aar { get; set; }

        [DisplayName("COMPUTED_RETRO_RECOVERY_AMOUNT")]
        public double? ComputedRetroRecoveryAmount { get; set; }

        [DisplayName("COMPUTED_RETRO_LATE_INTEREST")]
        public double? ComputedRetroLateInterest { get; set; }

        [DisplayName("COMPUTED_RETRO_EX_GRATIA")]
        public double? ComputedRetroExGratia { get; set; }

        [DisplayName("REPORTED_SOA_QUARTER")]
        public string ReportedSoaQuarter { get; set; }

        [DisplayName("RETRO_RECOVERY_AMOUNT")]
        public double? RetroRecoveryAmount { get; set; }

        [DisplayName("RETRO_LATE_INTEREST")]
        public double? RetroLateInterest { get; set; }

        [DisplayName("RETRO_EX_GRATIA")]
        public double? RetroExGratia { get; set; }

        [DisplayName("COMPUTED_CLAIM_CATEGORY")]
        public int ComputedClaimCategory { get; set; }

        [DisplayName("CLAIM_CATEGORY")]
        public int ClaimCategory { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }

        public virtual User CreatedBy { get; set; }
        public virtual User UpdatedBy { get; set; }

        // General and RI Data tab
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



        public PerLifeClaimRetroDataViewModel() { }

        public PerLifeClaimRetroDataViewModel(PerLifeClaimRetroDataBo perLifeClaimRetroDataBo)
        {
            Set(perLifeClaimRetroDataBo);
            ModuleId = ModuleService.FindByController(ModuleBo.ModuleController.PerLifeClaim.ToString()).Id;
        }

        public void Set(PerLifeClaimRetroDataBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                PerLifeClaimDataId = bo.PerLifeClaimDataId;
                PerLifeClaimDataBo = bo.PerLifeClaimDataBo;
                MlreShare = bo.MlreShare;
                RetroClaimRecoveryAmount = bo.RetroClaimRecoveryAmount;
                LateInterest = bo.LateInterest;
                ExGratia = bo.ExGratia;
                RetroRecoveryId = bo.RetroRecoveryId;
                RetroTreatyId = bo.RetroTreatyId;
                RetroRatio = bo.RetroRatio;
                Aar = bo.Aar;
                ComputedRetroRecoveryAmount = bo.ComputedRetroRecoveryAmount;
                ComputedRetroLateInterest = bo.ComputedRetroLateInterest;
                ComputedRetroExGratia = bo.ComputedRetroExGratia;
                ReportedSoaQuarter = bo.ReportedSoaQuarter;
                RetroRecoveryAmount = bo.RetroRecoveryAmount;
                RetroLateInterest = bo.RetroLateInterest;
                RetroExGratia = bo.RetroExGratia;
                ComputedClaimCategory = bo.ComputedClaimCategory;
                ClaimCategory = bo.ClaimCategory;
            }
        }

        //public PerLifeClaimRetroDataBo FormBo(PerLifeClaimRetroDataBo bo, )
        //{
        //    bo.PerLifeClaimDataId = PerLifeClaimDataId;
        //    bo.MlreShare = MlreShare;
        //    bo.RetroClaimRecoveryAmount = RetroClaimRecoveryAmount;
        //    bo.LateInterest = LateInterest;
        //    bo.ExGratia = ExGratia;
        //    bo.RetroRecoveryId = RetroRecoveryId;
        //    bo.RetroTreatyId = RetroTreatyId;
        //    bo.RetroRatio = RetroRatio;
        //    bo.Aar = Aar;
        //    bo.ComputedRetroRecoveryAmount = ComputedRetroRecoveryAmount;
        //    bo.ComputedRetroLateInterest = ComputedRetroLateInterest;
        //    bo.ComputedRetroExGratia = ComputedRetroExGratia;
        //    bo.ReportedSoaQuarter = ReportedSoaQuarter;
        //    bo.RetroRecoveryAmount = RetroRecoveryAmount;
        //    bo.RetroLateInterest = RetroLateInterest;
        //    bo.RetroExGratia = RetroExGratia;
        //    bo.ComputedClaimCategory = ComputedClaimCategory;
        //    bo.ClaimCategory = ClaimCategory;


        //    return bo;
        //}

        public PerLifeClaimRetroDataBo FormBo(int createdById, int updatedById)
        {
            return new PerLifeClaimRetroDataBo
            {
                PerLifeClaimDataId = PerLifeClaimDataId,
                MlreShare = MlreShare,
                RetroClaimRecoveryAmount = RetroClaimRecoveryAmount,
                LateInterest = LateInterest,
                ExGratia = ExGratia,
                RetroRecoveryId = RetroRecoveryId,
                RetroTreatyId = RetroTreatyId,
                RetroRatio = RetroRatio,
                Aar = Aar,
                ComputedRetroRecoveryAmount = ComputedRetroRecoveryAmount,
                ComputedRetroLateInterest = ComputedRetroLateInterest,
                ComputedRetroExGratia = ComputedRetroExGratia,
                ReportedSoaQuarter = ReportedSoaQuarter,
                RetroRecoveryAmount = RetroRecoveryAmount,
                RetroLateInterest = RetroLateInterest,
                RetroExGratia = RetroExGratia,
                ComputedClaimCategory = ComputedClaimCategory,
                ClaimCategory = ClaimCategory,

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }
    }
}