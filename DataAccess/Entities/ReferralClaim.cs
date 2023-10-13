using DataAccess.Entities.Identity;
using DataAccess.Entities.RiDatas;
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
    [Table("ReferralClaims")]
    public class ReferralClaim
    {
        [Key]
        public int Id { get; set; }

        [Index]
        public int? ClaimRegisterId { get; set; }
        [ExcludeTrail]
        public virtual ClaimRegister ClaimRegister { get; set; }

        [Index]
        public int? RiDataWarehouseId { get; set; }
        [ExcludeTrail]
        public virtual RiDataWarehouse RiDataWarehouse { get; set; }

        [Index]
        public int? ReferralRiDataId { get; set; }
        [ExcludeTrail]
        public virtual ReferralRiData ReferralRiData { get; set; }

        [Index]
        public int Status { get; set; }

        public string ReferralId { get; set; }

        [MaxLength(30)]
        [Index]
        public string RecordType { get; set; }

        public string InsuredName { get; set; }

        public string PolicyNumber { get; set; }

        [MaxLength(1)]
        [Index]
        public string InsuredGenderCode { get; set; }

        [MaxLength(1)]
        [Index]
        public string InsuredTobaccoUsage { get; set; }

        [Index]
        public int? ReferralReasonId { get; set; }
        [ExcludeTrail]
        public virtual ClaimReason ReferralReason { get; set; }

        public string GroupName { get; set; }

        [Index]
        [Column(TypeName = "datetime2")]
        public DateTime? DateReceivedFullDocuments { get; set; }

        [Index]
        [Column(TypeName = "datetime2")]
        public DateTime? InsuredDateOfBirth { get; set; }

        [MaxLength(15)]
        public string InsuredIcNumber { get; set; }

        [Index]
        [Column(TypeName = "datetime2")]
        public DateTime? DateOfCommencement { get; set; }

        [MaxLength(30)]
        [Index]
        public string CedingCompany { get; set; }

        [MaxLength(30)]
        [Index]
        public string ClaimCode { get; set; }

        [MaxLength(30)]
        [Index]
        public string CedingPlanCode { get; set; }

        [Index]
        public double? SumInsured { get; set; }

        [Index]
        public double? SumReinsured { get; set; }

        public string BenefitSubCode { get; set; }

        [Index]
        [Column(TypeName = "datetime2")]
        public DateTime? DateOfEvent { get; set; }

        [MaxLength(30)]
        [Index]
        public string RiskQuarter { get; set; }

        [MaxLength(255)]
        public string CauseOfEvent { get; set; }

        [MaxLength(30)]
        [Index]
        public string MlreBenefitCode { get; set; }

        [Index]
        public double? ClaimRecoveryAmount { get; set; }

        [MaxLength(30)]
        [Index]
        public string ReinsBasisCode { get; set; }

        [Index]
        public int? ClaimCategoryId { get; set; }
        [ExcludeTrail]
        public virtual ClaimCategory ClaimCategory { get; set; }

        [Index]
        public bool IsRgalRetakaful { get; set; }

        [Index]
        [Column(TypeName = "datetime2")]
        public DateTime? ReceivedAt { get; set; }

        [Index]
        [Column(TypeName = "datetime2")]
        public DateTime? RespondedAt { get; set; }

        [Index]
        [Column(TypeName = "datetime2")]
        public DateTime? DocRespondedAt { get; set; }

        [Index]
        public long? TurnAroundTime { get; set; }

        [Index]
        public long? DocTurnAroundTime { get; set; }

        [Index]
        public int? DelayReasonId { get; set; }
        [ExcludeTrail]
        public virtual ClaimReason DelayReason { get; set; }

        [Index]
        public int? DocDelayReasonId { get; set; }
        [ExcludeTrail]
        public virtual ClaimReason DocDelayReason { get; set; }

        [Index]
        public bool IsRetro { get; set; }

        public string RetrocessionaireName { get; set; }

        [Index]
        public double? RetrocessionaireShare { get; set; }

        [Index]
        public int? RetroReferralReasonId { get; set; }
        [ExcludeTrail]
        public virtual ClaimReason RetroReferralReason { get; set; }

        [Index]
        public int? MlreReferralReasonId { get; set; }
        [ExcludeTrail]
        public virtual ClaimReason MlreReferralReason { get; set; }

        [MaxLength(255)]
        public string RetroReviewedBy { get; set; }

        [Index]
        [Column(TypeName = "datetime2")]
        public DateTime? RetroReviewedAt { get; set; }

        [Index]
        public bool IsValueAddedService { get; set; }

        public string ValueAddedServiceDetails { get; set; }

        [Index]
        public bool IsClaimCaseStudy { get; set; }

        [Index]
        [Column(TypeName = "datetime2")]
        public DateTime? CompletedCaseStudyMaterialAt { get; set; }

        [Index]
        public int? AssessedById { get; set; }
        [ExcludeTrail]
        public virtual User AssessedBy { get; set; }

        [Index]
        [Column(TypeName = "datetime2")]
        public DateTime? AssessedAt { get; set; }

        public string AssessorComments { get; set; }

        [Index]
        public int? ReviewedById { get; set; }
        [ExcludeTrail]
        public virtual User ReviewedBy { get; set; }

        [Index]
        [Column(TypeName = "datetime2")]
        public DateTime? ReviewedAt { get; set; }

        public string ReviewerComments { get; set; }
        public int? ClaimsDecision { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ClaimsDecisionDate { get; set; }

        [Index]
        public int? AssignedById { get; set; }
        [ExcludeTrail]
        public virtual User AssignedBy { get; set; }

        [Index]
        [Column(TypeName = "datetime2")]
        public DateTime? AssignedAt { get; set; }

        [MaxLength(35)]
        [Index]
        public string TreatyCode { get; set; }

        [MaxLength(35)]
        [Index]
        public string TreatyType { get; set; }

        [Index]
        public double? TreatyShare { get; set; }

        public string Checklist { get; set; }

        public string Error { get; set; }

        public int? PersonInChargeId { get; set; }
        [ExcludeTrail]
        public virtual User PersonInCharge { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? UpdatedAt { get; set; }

        public int CreatedById { get; set; }
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        public int? UpdatedById { get; set; }
        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }


        public ReferralClaim()
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

        public static ReferralClaim Find(int id, AppDbContext db)
        {
            return db.ReferralClaims.Where(q => q.Id == id).FirstOrDefault();
        }

        public static ReferralClaim Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return Find(id, db);
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ReferralClaims.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(Id, db);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, this);

                entity.ClaimRegisterId = ClaimRegisterId;
                entity.RiDataWarehouseId = RiDataWarehouseId;
                entity.ReferralRiDataId = ReferralRiDataId;
                entity.Status = Status;
                entity.ReferralId = ReferralId;
                entity.RecordType = RecordType;
                entity.InsuredName = InsuredName;
                entity.PolicyNumber = PolicyNumber;
                entity.InsuredGenderCode = InsuredGenderCode;
                entity.InsuredTobaccoUsage = InsuredTobaccoUsage;
                entity.ReferralReasonId = ReferralReasonId;
                entity.GroupName = GroupName;
                entity.DateReceivedFullDocuments = DateReceivedFullDocuments;
                entity.InsuredDateOfBirth = InsuredDateOfBirth;
                entity.InsuredIcNumber = InsuredIcNumber;
                entity.DateOfCommencement = DateOfCommencement;
                entity.CedingCompany = CedingCompany;
                entity.ClaimCode = ClaimCode;
                entity.CedingPlanCode = CedingPlanCode;
                entity.SumInsured = SumInsured;
                entity.SumReinsured = SumReinsured;
                entity.BenefitSubCode = BenefitSubCode;
                entity.DateOfEvent = DateOfEvent;
                entity.RiskQuarter = RiskQuarter;
                entity.CauseOfEvent = CauseOfEvent;
                entity.MlreBenefitCode = MlreBenefitCode;
                entity.ClaimRecoveryAmount = ClaimRecoveryAmount;
                entity.ReinsBasisCode = ReinsBasisCode;
                entity.ClaimCategoryId = ClaimCategoryId;
                entity.IsRgalRetakaful = IsRgalRetakaful;
                entity.ReceivedAt = ReceivedAt;
                entity.RespondedAt = RespondedAt;
                entity.TurnAroundTime = TurnAroundTime;
                entity.DelayReasonId = DelayReasonId;
                entity.IsRetro = IsRetro;
                entity.RetrocessionaireName = RetrocessionaireName;
                entity.RetrocessionaireShare = RetrocessionaireShare;
                entity.RetroReferralReasonId = RetroReferralReasonId;
                entity.MlreReferralReasonId = MlreReferralReasonId;
                entity.RetroReviewedBy = RetroReviewedBy;
                entity.RetroReviewedAt = RetroReviewedAt;
                entity.IsValueAddedService = IsValueAddedService;
                entity.ValueAddedServiceDetails = ValueAddedServiceDetails;
                entity.IsClaimCaseStudy = IsClaimCaseStudy;
                entity.CompletedCaseStudyMaterialAt = CompletedCaseStudyMaterialAt;
                entity.AssessedById = AssessedById;
                entity.AssessedAt = AssessedAt;
                entity.AssessorComments = AssessorComments;
                entity.ReviewedById = ReviewedById;
                entity.ReviewedAt = ReviewedAt;
                entity.ReviewerComments = ReviewerComments;
                entity.ClaimsDecision = ClaimsDecision;
                entity.ClaimsDecisionDate = ClaimsDecisionDate;
                entity.AssignedById = AssignedById;
                entity.AssignedAt = AssignedAt;
                entity.TreatyCode = TreatyCode;
                entity.TreatyType = TreatyType;
                entity.TreatyShare = TreatyShare;
                entity.Checklist = Checklist;
                entity.Error = Error;
                entity.PersonInChargeId = PersonInChargeId;
                entity.DocTurnAroundTime = DocTurnAroundTime;
                entity.DocDelayReasonId = DocDelayReasonId;
                entity.DocRespondedAt = DocRespondedAt;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = UpdatedById ?? entity.UpdatedById;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                ReferralClaim entity = Find(id, db);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.ReferralClaims.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static int CountByBenefitCode(int claimReasonId)
        {
            using (var db = new AppDbContext())
            {
                var count = 0;
                count = db.ReferralClaims.Where(q => q.DelayReasonId == claimReasonId).Count() +
                    db.ReferralClaims.Where(q => q.DocDelayReasonId == claimReasonId).Count() +
                    db.ReferralClaims.Where(q => q.MlreReferralReasonId == claimReasonId).Count() +
                    db.ReferralClaims.Where(q => q.RetroReferralReasonId == claimReasonId).Count() +
                    db.ReferralClaims.Where(q => q.ReferralReasonId == claimReasonId).Count();

                return count;
            }
        }
    }
}
