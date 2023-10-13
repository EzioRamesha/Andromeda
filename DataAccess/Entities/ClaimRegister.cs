using DataAccess.Entities.Claims;
using DataAccess.Entities.Identity;
using DataAccess.Entities.RiDatas;
using DataAccess.Entities.SoaDatas;
using DataAccess.EntityFramework;
using Shared;
using Shared.Trails;
using Shared.Trails.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataAccess.Entities
{
    [Table("ClaimRegister")]
    public class ClaimRegister
    {
        [Key]
        [Column(TypeName = "bigint")]
        public int Id { get; set; }

        [Index]
        public int? ClaimDataBatchId { get; set; }
        [ExcludeTrail]
        [ForeignKey(nameof(ClaimDataBatchId))]
        public virtual ClaimDataBatch ClaimDataBatch { get; set; }

        [Index]
        public int? ClaimDataId { get; set; }
        [ExcludeTrail]
        [ForeignKey(nameof(ClaimDataId))]
        public virtual ClaimData ClaimData { get; set; }

        [Index]
        public int? ClaimDataConfigId { get; set; }
        [ExcludeTrail]
        [ForeignKey(nameof(ClaimDataConfigId))]
        public virtual ClaimDataConfig ClaimDataConfig { get; set; }

        [Index]
        public int? SoaDataBatchId { get; set; }
        [ExcludeTrail]
        [ForeignKey(nameof(SoaDataBatchId))]
        public virtual SoaDataBatch SoaDataBatch { get; set; }

        public int? RiDataId { get; set; }

        [Index]
        public int? RiDataWarehouseId { get; set; }
        [ExcludeTrail]
        [ForeignKey(nameof(RiDataWarehouseId))]
        public virtual RiDataWarehouse RiDataWarehouse { get; set; }

        [Index]
        public int? ReferralRiDataId { get; set; }
        [ExcludeTrail]
        [ForeignKey(nameof(ReferralRiDataId))]
        public virtual ReferralRiData ReferralRiData { get; set; }

        [Index]
        public int? ReferralClaimId { get; set; }
        [ExcludeTrail]
        [ForeignKey(nameof(ReferralClaimId))]
        public virtual ReferralClaim ReferralClaim { get; set; }

        [Index]
        public int? OriginalClaimRegisterId { get; set; }
        [ExcludeTrail]
        [ForeignKey(nameof(OriginalClaimRegisterId))]
        public virtual ClaimRegister OriginalClaimRegister { get; set; }

        [Index]
        public int? ClaimReasonId { get; set; }
        [ExcludeTrail]
        [ForeignKey(nameof(ClaimReasonId))]
        public virtual ClaimReason ClaimReason { get; set; }

        [Index]
        public int? PicClaimId { get; set; }
        [ExcludeTrail]
        [ForeignKey(nameof(PicClaimId))]
        public virtual User PicClaim { get; set; }

        [Index]
        public int? PicDaaId { get; set; }
        [ExcludeTrail]
        [ForeignKey(nameof(PicDaaId))]
        public virtual User PicDaa { get; set; }

        [Index]
        public int ClaimStatus { get; set; }

        [Index]
        public int? ClaimDecisionStatus { get; set; }

        [Index]
        public int ProvisionStatus { get; set; }

        [Index]
        public int DrProvisionStatus { get; set; }

        [Index]
        public int OffsetStatus { get; set; }

        [Index]
        public int MappingStatus { get; set; }

        [Index]
        public int ProcessingStatus { get; set; }

        [Index]
        public int DuplicationCheckStatus { get; set; }

        [Index]
        public int PostComputationStatus { get; set; }

        [Index]
        public int PostValidationStatus { get; set; }

        public string Errors { get; set; }

        public string ProvisionErrors { get; set; }

        public string RedFlagWarnings { get; set; }

        [Index]
        public bool IsReferralCase { get; set; }

        [Index]
        public bool HasRedFlag { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? TargetDateToIssueInvoice { get; set; }

        [MaxLength(30)]
        [Index]
        public string ClaimId { get; set; }

        [MaxLength(30)]
        [Index]
        public string ClaimCode { get; set; }

        [MaxLength(150)]
        [Index]
        public string PolicyNumber { get; set; }

        [Index]
        public double? PolicyTerm { get; set; }

        [Index]
        public double? ClaimRecoveryAmt { get; set; }

        [MaxLength(30)]
        [Index]
        public string ClaimTransactionType { get; set; }

        [MaxLength(35)]
        [Index]
        public string TreatyCode { get; set; }

        [MaxLength(35)]
        [Index]
        public string TreatyType { get; set; }

        [Index]
        public double? AarPayable { get; set; }

        [Index]
        public double? AnnualRiPrem { get; set; }

        [MaxLength(255)]
        [Index]
        public string CauseOfEvent { get; set; }

        [MaxLength(30)]
        [Index]
        public string CedantClaimEventCode { get; set; }

        [MaxLength(30)]
        [Index]
        public string CedantClaimType { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? CedantDateOfNotification { get; set; }

        [MaxLength(30)]
        [Index]
        public string CedingBenefitRiskCode { get; set; }

        [MaxLength(30)]
        [Index]
        public string CedingBenefitTypeCode { get; set; }

        [MaxLength(30)]
        [Index]
        public string CedingClaimType { get; set; }

        [MaxLength(30)]
        [Index]
        public string CedingCompany { get; set; }

        [MaxLength(30)]
        [Index]
        public string CedingEventCode { get; set; }

        [MaxLength(30)]
        [Index]
        public string CedingPlanCode { get; set; }

        [Index]
        public double? CurrencyRate { get; set; }

        [MaxLength(3)]
        [Index]
        public string CurrencyCode { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? DateApproved { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? DateOfEvent { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? DateOfRegister { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? DateOfReported { get; set; }

        [MaxLength(30)]
        [Index]
        public string EntryNo { get; set; }

        [Index]
        public double? ExGratia { get; set; }

        [Index]
        public double? ForeignClaimRecoveryAmt { get; set; }

        [MaxLength(30)]
        [Index]
        public string FundsAccountingTypeCode { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? InsuredDateOfBirth { get; set; }

        [MaxLength(1)]
        [Index]
        public string InsuredGenderCode { get; set; }

        [MaxLength(128)]
        [Index]
        public string InsuredName { get; set; }

        [MaxLength(1)]
        [Index]
        public string InsuredTobaccoUse { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? LastTransactionDate { get; set; }

        [MaxLength(30)]
        [Index]
        public string LastTransactionQuarter { get; set; }

        [Index]
        public double? LateInterest { get; set; }

        [Index]
        public double? Layer1SumRein { get; set; }

        [Index]
        public int? Mfrs17AnnualCohort { get; set; }

        [MaxLength(30)]
        [Index]
        public string Mfrs17ContractCode { get; set; }

        [MaxLength(30)]
        [Index]
        public string MlreBenefitCode { get; set; }

        [MaxLength(30)]
        [Index]
        public string MlreEventCode { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? MlreInvoiceDate { get; set; }

        [MaxLength(30)]
        [Index]
        public string MlreInvoiceNumber { get; set; }

        [Index]
        public double? MlreRetainAmount { get; set; }

        [Index]
        public double? MlreShare { get; set; }

        [Index]
        public int? PendingProvisionDay { get; set; }

        [Index]
        public int? PolicyDuration { get; set; }

        [MaxLength(30)]
        [Index]
        public string RecordType { get; set; }

        [MaxLength(30)]
        [Index]
        public string ReinsBasisCode { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? ReinsEffDatePol { get; set; }

        [MaxLength(128)]
        [Index]
        public string RetroParty1 { get; set; }

        [MaxLength(128)]
        [Index]
        public string RetroParty2 { get; set; }

        [MaxLength(128)]
        [Index]
        public string RetroParty3 { get; set; }

        [Index]
        public double? RetroRecovery1 { get; set; }

        [Index]
        public double? RetroRecovery2 { get; set; }

        [Index]
        public double? RetroRecovery3 { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? RetroStatementDate1 { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? RetroStatementDate2 { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? RetroStatementDate3 { get; set; }

        [Index]
        public double? RetroShare1 { get; set; }

        [Index]
        public double? RetroShare2 { get; set; }

        [Index]
        public double? RetroShare3 { get; set; }

        [MaxLength(30)]
        [Index]
        public string RetroStatementId1 { get; set; }

        [MaxLength(30)]
        [Index]
        public string RetroStatementId2 { get; set; }

        [MaxLength(30)]
        [Index]
        public string RetroStatementId3 { get; set; }

        [Index]
        public int? RiskPeriodMonth { get; set; }

        [Index]
        public int? RiskPeriodYear { get; set; }

        [MaxLength(30)]
        [Index]
        public string RiskQuarter { get; set; }

        [Index]
        public double? SaFactor { get; set; }

        [MaxLength(30)]
        [Index]
        public string SoaQuarter { get; set; }

        [Index]
        public double? SumIns { get; set; }

        [Index]
        public double? TempA1 { get; set; }

        [Index]
        public double? TempA2 { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? TempD1 { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? TempD2 { get; set; }

        [Index]
        public int? TempI1 { get; set; }

        [Index]
        public int? TempI2 { get; set; }

        [MaxLength(150)]
        [Index]
        public string TempS1 { get; set; }

        [MaxLength(50)]
        [Index]
        public string TempS2 { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? TransactionDateWop { get; set; }

        [MaxLength(30)]
        public string MlreReferenceNo { get; set; }

        [MaxLength(30)]
        public string AddInfo { get; set; }

        [MaxLength(128)]
        public string Remark1 { get; set; }

        [MaxLength(128)]
        public string Remark2 { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? IssueDatePol { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? PolicyExpiryDate { get; set; }

        [Index]
        public int? ClaimAssessorId { get; set; }
        [ExcludeTrail]
        public virtual User ClaimAssessor { get; set; }

        [MaxLength(128)]
        public string Comment { get; set; }

        // Underwriting
        public bool RequestUnderwriterReview { get; set; }

        public int? UnderwriterFeedback { get; set; }

        // Ex Gratia 
        public string EventChronologyComment { get; set; }

        public string ClaimAssessorRecommendation { get; set; }

        public string ClaimCommitteeComment1 { get; set; }

        public string ClaimCommitteeComment2 { get; set; }

        public int? ClaimCommitteeUser1Id { get; set; }
        [ExcludeTrail]
        public virtual User ClaimCommitteeUser1 { get; set; }

        [Index, MaxLength(128)]
        public string ClaimCommitteeUser1Name { get; set; }

        public int? ClaimCommitteeUser2Id { get; set; }
        [ExcludeTrail]
        public virtual User ClaimCommitteeUser2 { get; set; }

        [Index, MaxLength(128)]
        public string ClaimCommitteeUser2Name { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? ClaimCommitteeDateCommented1 { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? ClaimCommitteeDateCommented2 { get; set; }

        [Index]
        public int? CeoClaimReasonId { get; set; }
        [ExcludeTrail]
        public virtual ClaimReason CeoClaimReason { get; set; }

        public string CeoComment { get; set; }

        public int? UpdatedOnBehalfById { get; set; }
        [ExcludeTrail]
        public virtual User UpdatedOnBehalfBy { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? UpdatedOnBehalfAt { get; set; }

        // Checklist
        public string Checklist { get; set; }

        [Index]
        public int? SignOffById { get; set; }
        [ExcludeTrail]
        public virtual User SignOffBy { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? SignOffDate { get; set; }

        [MaxLength(30)]
        [Index]
        public string CedingTreatyCode { get; set; }

        [MaxLength(10)]
        [Index]
        public string CampaignCode { get; set; }
        
        [Column(TypeName = "datetime2"), Index]
        public DateTime? DateOfIntimation { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        public int? UpdatedById { get; set; }
        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }

        public ClaimRegister()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimRegister.Any(q => q.Id == id);
            }
        }

        public static ClaimRegister Find(int id, bool isClaimOnly = false)
        {
            using (var db = new AppDbContext())
            {
                return db.GetClaimRegister(isClaimOnly).Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static int CountByClaimDataId(int claimDataId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimRegister.Where(q => q.ClaimDataId == claimDataId).Count();
            }
        }

        public static int CountByRiDataId(int riDataId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimRegister.Where(q => q.RiDataId == riDataId).Count();
            }
        }

        public static int CountByRiDataWarehouseId(int riDataWarehouseId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimRegister.Where(q => q.RiDataWarehouseId == riDataWarehouseId).Count();
            }
        }

        public static int CountByOriginalClaimRegisterId(int originalClaimDataId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimRegister.Where(q => q.OriginalClaimRegisterId == originalClaimDataId).Count();
            }
        }

        public static int CountByClaimReasonId(int claimReasonId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimRegister.Where(q => q.ClaimReasonId == claimReasonId).Count();
            }
        }

        public static int CountByClaimCode(string claimCode)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimRegister.Where(q => q.ClaimCode == claimCode).Count();
            }
        }

        public static int CountByPicClaimId(int picClaimId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimRegister.Where(q => q.PicClaimId == picClaimId).Count();
            }
        }

        public static int CountByPicDaaId(int picDaaId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimRegister.Where(q => q.PicDaaId == picDaaId).Count();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ClaimRegister.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public void Create(AppDbContext db)
        {
            db.ClaimRegister.Add(this);
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                ClaimRegister claimRegister = Find(Id);
                if (claimRegister == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimRegister, this);

                claimRegister.ClaimDataId = ClaimDataId;
                claimRegister.ClaimDataBatchId = ClaimDataBatchId;
                claimRegister.ClaimDataConfigId = ClaimDataConfigId;
                claimRegister.SoaDataBatchId = SoaDataBatchId;
                claimRegister.RiDataWarehouseId = RiDataWarehouseId;
                claimRegister.ReferralRiDataId = ReferralRiDataId;
                claimRegister.ClaimStatus = ClaimStatus;
                claimRegister.ClaimDecisionStatus = ClaimDecisionStatus;
                claimRegister.ReferralClaimId = ReferralClaimId;
                claimRegister.OriginalClaimRegisterId = OriginalClaimRegisterId;
                claimRegister.ClaimReasonId = ClaimReasonId;
                claimRegister.PicDaaId = PicDaaId;
                claimRegister.PicClaimId = PicClaimId;
                claimRegister.ProvisionStatus = ProvisionStatus;
                claimRegister.DrProvisionStatus = DrProvisionStatus;
                claimRegister.TargetDateToIssueInvoice = TargetDateToIssueInvoice;
                claimRegister.IsReferralCase = IsReferralCase;
                claimRegister.ClaimId = ClaimId;
                claimRegister.ClaimCode = ClaimCode;
                claimRegister.MappingStatus = MappingStatus;
                claimRegister.ProcessingStatus = ProcessingStatus;
                claimRegister.DuplicationCheckStatus = DuplicationCheckStatus;
                claimRegister.PostComputationStatus = PostComputationStatus;
                claimRegister.PostValidationStatus = PostValidationStatus;
                claimRegister.Errors = Errors;
                claimRegister.ProvisionErrors = ProvisionErrors;
                claimRegister.RedFlagWarnings = RedFlagWarnings;
                claimRegister.RequestUnderwriterReview = RequestUnderwriterReview;
                claimRegister.UnderwriterFeedback = UnderwriterFeedback;
                claimRegister.HasRedFlag = HasRedFlag;

                claimRegister.PolicyNumber = PolicyNumber;
                claimRegister.PolicyTerm = PolicyTerm;
                claimRegister.ClaimRecoveryAmt = ClaimRecoveryAmt;
                claimRegister.ClaimTransactionType = ClaimTransactionType;
                claimRegister.TreatyCode = TreatyCode;
                claimRegister.TreatyType = TreatyType;
                claimRegister.AarPayable = AarPayable;
                claimRegister.AnnualRiPrem = AnnualRiPrem;
                claimRegister.CauseOfEvent = CauseOfEvent;
                claimRegister.CedantClaimEventCode = CedantClaimEventCode;
                claimRegister.CedantClaimType = CedantClaimType;
                claimRegister.CedantDateOfNotification = CedantDateOfNotification;
                claimRegister.CedingBenefitRiskCode = CedingBenefitRiskCode;
                claimRegister.CedingBenefitTypeCode = CedingBenefitTypeCode;
                claimRegister.CedingClaimType = CedingClaimType;
                claimRegister.CedingCompany = CedingCompany;
                claimRegister.CedingEventCode = CedingEventCode;
                claimRegister.CedingPlanCode = CedingPlanCode;
                claimRegister.CurrencyRate = CurrencyRate;
                claimRegister.CurrencyCode = CurrencyCode;
                claimRegister.DateApproved = DateApproved;
                claimRegister.DateOfEvent = DateOfEvent;
                claimRegister.DateOfRegister = DateOfRegister;
                claimRegister.DateOfReported = DateOfReported;
                claimRegister.EntryNo = EntryNo;
                claimRegister.ExGratia = ExGratia;
                claimRegister.ForeignClaimRecoveryAmt = ForeignClaimRecoveryAmt;
                claimRegister.FundsAccountingTypeCode = FundsAccountingTypeCode;
                claimRegister.InsuredDateOfBirth = InsuredDateOfBirth;
                claimRegister.InsuredGenderCode = InsuredGenderCode;
                claimRegister.InsuredName = InsuredName;
                claimRegister.InsuredTobaccoUse = InsuredTobaccoUse;
                claimRegister.LastTransactionDate = LastTransactionDate;
                claimRegister.LastTransactionQuarter = LastTransactionQuarter;
                claimRegister.LateInterest = LateInterest;
                claimRegister.Layer1SumRein = Layer1SumRein;
                claimRegister.Mfrs17AnnualCohort = Mfrs17AnnualCohort;
                claimRegister.Mfrs17ContractCode = Mfrs17ContractCode;
                claimRegister.MlreBenefitCode = MlreBenefitCode;
                claimRegister.MlreEventCode = MlreEventCode;
                claimRegister.MlreInvoiceDate = MlreInvoiceDate;
                claimRegister.MlreInvoiceNumber = MlreInvoiceNumber;
                claimRegister.MlreRetainAmount = MlreRetainAmount;
                claimRegister.MlreShare = MlreShare;
                claimRegister.OffsetStatus = OffsetStatus;
                claimRegister.PendingProvisionDay = PendingProvisionDay;
                claimRegister.PolicyDuration = PolicyDuration;
                claimRegister.RecordType = RecordType;
                claimRegister.ReinsBasisCode = ReinsBasisCode;
                claimRegister.ReinsEffDatePol = ReinsEffDatePol;
                claimRegister.RetroParty1 = RetroParty1;
                claimRegister.RetroParty2 = RetroParty2;
                claimRegister.RetroParty3 = RetroParty3;
                claimRegister.RetroRecovery1 = RetroRecovery1;
                claimRegister.RetroRecovery2 = RetroRecovery2;
                claimRegister.RetroRecovery3 = RetroRecovery3;
                claimRegister.RetroStatementDate1 = RetroStatementDate1;
                claimRegister.RetroStatementDate2 = RetroStatementDate2;
                claimRegister.RetroStatementDate3 = RetroStatementDate3;
                claimRegister.RetroStatementId1 = RetroStatementId1;
                claimRegister.RetroStatementId2 = RetroStatementId2;
                claimRegister.RetroStatementId3 = RetroStatementId3;
                claimRegister.RetroShare1 = RetroShare1;
                claimRegister.RetroShare2 = RetroShare2;
                claimRegister.RetroShare3 = RetroShare3;
                claimRegister.RiskPeriodMonth = RiskPeriodMonth;
                claimRegister.RiskPeriodYear = RiskPeriodYear;
                claimRegister.RiskQuarter = RiskQuarter;
                claimRegister.SaFactor = SaFactor;
                claimRegister.SoaQuarter = SoaQuarter;
                claimRegister.SumIns = SumIns;
                claimRegister.TempA1 = TempA1;
                claimRegister.TempA2 = TempA2;
                claimRegister.TempD1 = TempD1;
                claimRegister.TempD2 = TempD2;
                claimRegister.TempI1 = TempI1;
                claimRegister.TempI2 = TempI2;
                claimRegister.TempS1 = TempS1;
                claimRegister.TempS2 = TempS2;
                claimRegister.TransactionDateWop = TransactionDateWop;
                claimRegister.MlreReferenceNo = MlreReferenceNo;
                claimRegister.AddInfo = AddInfo;
                claimRegister.Remark1 = Remark1;
                claimRegister.Remark2 = Remark2;
                claimRegister.IssueDatePol = IssueDatePol;
                claimRegister.PolicyExpiryDate = PolicyExpiryDate;
                claimRegister.ClaimAssessorId = ClaimAssessorId;
                claimRegister.Comment = Comment;
                claimRegister.SignOffById = SignOffById;
                claimRegister.SignOffDate = SignOffDate;
                claimRegister.CedingTreatyCode = CedingTreatyCode;
                claimRegister.CampaignCode = CampaignCode;
                claimRegister.DateOfIntimation = DateOfIntimation;

                // Ex Gratia
                claimRegister.EventChronologyComment = EventChronologyComment;
                claimRegister.ClaimAssessorRecommendation = ClaimAssessorRecommendation;
                claimRegister.ClaimCommitteeComment1 = ClaimCommitteeComment1;
                claimRegister.ClaimCommitteeComment2 = ClaimCommitteeComment2;
                claimRegister.ClaimCommitteeUser1Id = ClaimCommitteeUser1Id;
                claimRegister.ClaimCommitteeUser1Name = ClaimCommitteeUser1Name;
                claimRegister.ClaimCommitteeUser2Id = ClaimCommitteeUser2Id;
                claimRegister.ClaimCommitteeUser2Name = ClaimCommitteeUser2Name;
                claimRegister.ClaimCommitteeDateCommented1 = ClaimCommitteeDateCommented1;
                claimRegister.ClaimCommitteeDateCommented2 = ClaimCommitteeDateCommented2;
                claimRegister.CeoClaimReasonId = CeoClaimReasonId;
                claimRegister.CeoComment = CeoComment;
                claimRegister.UpdatedOnBehalfById = UpdatedOnBehalfById;
                claimRegister.UpdatedOnBehalfAt = UpdatedOnBehalfAt;

                // Checklist
                claimRegister.Checklist = Checklist;

                claimRegister.UpdatedAt = DateTime.Now;
                claimRegister.UpdatedById = UpdatedById ?? claimRegister.UpdatedById;

                db.Entry(claimRegister).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                ClaimRegister claimRegister = db.ClaimRegister.Where(q => q.Id == id).FirstOrDefault();
                if (claimRegister == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimRegister, true);

                db.Entry(claimRegister).State = EntityState.Deleted;
                db.ClaimRegister.Remove(claimRegister);
                db.SaveChanges();

                return trail;
            }
        }

        public static int CountByBenefitCode(int claimReasonId)
        {
            using (var db = new AppDbContext())
            {
                var count = 0;
                count = db.ClaimRegister.Where(q => q.CeoClaimReasonId == claimReasonId).Count() +
                    db.ClaimRegister.Where(q => q.ClaimReasonId == claimReasonId).Count();

                return count;
            }
        }
    }
}
