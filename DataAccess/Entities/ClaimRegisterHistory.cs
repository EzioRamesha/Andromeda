using DataAccess.Entities.Claims;
using DataAccess.Entities.Identity;
using DataAccess.Entities.RiDatas;
using DataAccess.Entities.SoaDatas;
using DataAccess.EntityFramework;
using Shared;
using Shared.Trails.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DataAccess.Entities
{
    [Table("ClaimRegisterHistories")]
    public class ClaimRegisterHistory
    {
        [Key]
        [Column(TypeName = "bigint")]
        public int Id { get; set; }


        [Required, Index]
        public int CutOffId { get; set; }
        [ExcludeTrail]
        public virtual CutOff CutOff { get; set; }


        [Required, Index]
        public int ClaimRegisterId { get; set; }
        [ExcludeTrail]
        public virtual ClaimRegister ClaimRegister { get; set; }


        [Index]
        public int? ClaimDataBatchId { get; set; }
        [ExcludeTrail]
        public virtual ClaimDataBatch ClaimDataBatch { get; set; }

        [Index]
        public int? ClaimDataId { get; set; }
        [ExcludeTrail]
        public virtual ClaimData ClaimData { get; set; }

        [Index]
        public int? ClaimDataConfigId { get; set; }
        [ExcludeTrail]
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
        public virtual RiDataWarehouse RiDataWarehouse { get; set; }

        [Index]
        public int? ReferralRiDataId { get; set; }
        [ExcludeTrail]
        public virtual ReferralRiData ReferralRiData { get; set; }

        [Index]
        public int? ReferralClaimId { get; set; }
        [ExcludeTrail]
        public virtual ReferralClaim ReferralClaim { get; set; }

        [Index]
        public int? OriginalClaimRegisterId { get; set; }
        [ExcludeTrail]
        public virtual ClaimRegister OriginalClaimRegister { get; set; }

        [Index]
        public int? ClaimReasonId { get; set; }
        [ExcludeTrail]
        public virtual ClaimReason ClaimReason { get; set; }

        [Index]
        public int? PicClaimId { get; set; }
        [ExcludeTrail]
        public virtual User PicClaim { get; set; }

        [Index]
        public int? PicDaaId { get; set; }
        [ExcludeTrail]
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

        public static string Script(int cutOffId)
        {
            string[] fields = Fields();

            string script = "INSERT INTO [dbo].[ClaimRegisterHistories]";
            string field = string.Format("({0})", string.Join(",", fields));
            string select = string.Format("SELECT {0},[Id],{1} FROM [dbo].[ClaimRegister]", cutOffId, string.Join(",", fields.Skip(2).ToArray()));

            return string.Format("{0}\n{1}\n{2};", script, field, select);
        }

        public static string[] Fields()
        {
            return new string[]
            {
                "[CutOffId]",
                "[ClaimRegisterId]",
                "[ClaimDataBatchId]",
                "[ClaimDataId]",
                "[ClaimDataConfigId]",
                "[SoaDataBatchId]",
                "[RiDataId]",
                "[RiDataWarehouseId]",
                "[ReferralRiDataId]",
                "[ReferralClaimId]",
                "[OriginalClaimRegisterId]",
                "[ClaimReasonId]",
                "[PicClaimId]",
                "[PicDaaId]",
                "[ClaimStatus]",
                "[ClaimDecisionStatus]",
                "[ProvisionStatus]",
                "[DrProvisionStatus]",
                "[OffsetStatus]",
                "[MappingStatus]",
                "[ProcessingStatus]",
                "[DuplicationCheckStatus]",
                "[PostComputationStatus]",
                "[PostValidationStatus]",
                "[Errors]",
                "[ProvisionErrors]",
                "[RedFlagWarnings]",
                "[IsReferralCase]",
                "[HasRedFlag]",
                "[TargetDateToIssueInvoice]",
                "[ClaimId]",
                "[ClaimCode]",
                "[PolicyNumber]",
                "[PolicyTerm]",
                "[ClaimRecoveryAmt]",
                "[ClaimTransactionType]",
                "[TreatyCode]",
                "[TreatyType]",
                "[AarPayable]",
                "[AnnualRiPrem]",
                "[CauseOfEvent]",
                "[CedantClaimEventCode]",
                "[CedantClaimType]",
                "[CedantDateOfNotification]",
                "[CedingBenefitRiskCode]",
                "[CedingBenefitTypeCode]",
                "[CedingClaimType]",
                "[CedingCompany]",
                "[CedingEventCode]",
                "[CedingPlanCode]",
                "[CurrencyRate]",
                "[CurrencyCode]",
                "[DateApproved]",
                "[DateOfEvent]",
                "[DateOfRegister]",
                "[DateOfReported]",
                "[EntryNo]",
                "[ExGratia]",
                "[ForeignClaimRecoveryAmt]",
                "[FundsAccountingTypeCode]",
                "[InsuredDateOfBirth]",
                "[InsuredGenderCode]",
                "[InsuredName]",
                "[InsuredTobaccoUse]",
                "[LastTransactionDate]",
                "[LastTransactionQuarter]",
                "[LateInterest]",
                "[Layer1SumRein]",
                "[Mfrs17AnnualCohort]",
                "[Mfrs17ContractCode]",
                "[MlreBenefitCode]",
                "[MlreEventCode]",
                "[MlreInvoiceDate]",
                "[MlreInvoiceNumber]",
                "[MlreRetainAmount]",
                "[MlreShare]",
                "[PendingProvisionDay]",
                "[PolicyDuration]",
                "[RecordType]",
                "[ReinsBasisCode]",
                "[ReinsEffDatePol]",
                "[RetroParty1]",
                "[RetroParty2]",
                "[RetroParty3]",
                "[RetroRecovery1]",
                "[RetroRecovery2]",
                "[RetroRecovery3]",
                "[RetroStatementDate1]",
                "[RetroStatementDate2]",
                "[RetroStatementDate3]",
                "[RetroShare1]",
                "[RetroShare2]",
                "[RetroShare3]",
                "[RetroStatementId1]",
                "[RetroStatementId2]",
                "[RetroStatementId3]",
                "[RiskPeriodMonth]",
                "[RiskPeriodYear]",
                "[RiskQuarter]",
                "[SaFactor]",
                "[SoaQuarter]",
                "[SumIns]",
                "[TempA1]",
                "[TempA2]",
                "[TempD1]",
                "[TempD2]",
                "[TempI1]",
                "[TempI2]",
                "[TempS1]",
                "[TempS2]",
                "[TransactionDateWop]",
                "[MlreReferenceNo]",
                "[AddInfo]",
                "[Remark1]",
                "[Remark2]",
                "[IssueDatePol]",
                "[PolicyExpiryDate]",
                "[ClaimAssessorId]",
                "[Comment]",
                "[RequestUnderwriterReview]",
                "[UnderwriterFeedback]",
                "[EventChronologyComment]",
                "[ClaimAssessorRecommendation]",
                "[ClaimCommitteeComment1]",
                "[ClaimCommitteeComment2]",
                "[ClaimCommitteeUser1Id]",
                "[ClaimCommitteeUser1Name]",
                "[ClaimCommitteeUser2Id]",
                "[ClaimCommitteeUser2Name]",
                "[ClaimCommitteeDateCommented1]",
                "[ClaimCommitteeDateCommented2]",
                "[CeoClaimReasonId]",
                "[CeoComment]",
                "[UpdatedOnBehalfById]",
                "[UpdatedOnBehalfAt]",
                "[Checklist]",
                "[SignOffById]",
                "[SignOffDate]",
                "[CedingTreatyCode]",
                "[CampaignCode]",
                "[DateOfIntimation]",
                "[CreatedAt]",
                "[UpdatedAt]",
                "[CreatedById]",
                "[UpdatedById]",
            };
        }

        public static ClaimRegisterHistory Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimRegisterHistories.Where(q => q.Id == id).FirstOrDefault();
            }
        }
    }
}
