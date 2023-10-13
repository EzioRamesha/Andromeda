using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Shared;
using Shared.Trails;
using Shared.Trails.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataAccess.Entities.TreatyPricing
{
    [Table("TreatyPricingGroupReferrals")]
    public class TreatyPricingGroupReferral
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int CedantId { get; set; }
        [ExcludeTrail]
        public virtual Cedant Cedant { get; set; }

        [Required, MaxLength(30), Index]
        public string Code { get; set; }

        [Required, MaxLength(255)]
        public string Description { get; set; }

        public int? RiArrangementPickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail RiArrangementPickListDetail { get; set; }

        [Required, Index]
        public int InsuredGroupNameId { get; set; }
        [ExcludeTrail]
        public virtual InsuredGroupName InsuredGroupName { get; set; }

        [Index]
        public int Status { get; set; }

        public int? WorkflowStatus { get; set; }

        [Required, Index]
        public int PrimaryTreatyPricingProductId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingProduct PrimaryTreatyPricingProduct { get; set; }
        [Required, Index]
        public int PrimaryTreatyPricingProductVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingProductVersion PrimaryTreatyPricingProductVersion { get; set; }

        [Index]
        public int? SecondaryTreatyPricingProductId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingProduct SecondaryTreatyPricingProduct { get; set; }
        [Index]
        public int? SecondaryTreatyPricingProductVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingProductVersion SecondaryTreatyPricingProductVersion { get; set; }

        public string PolicyNumber { get; set; }

        //[Column(TypeName = "datetime2")]
        //public DateTime? FirstRequestDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? FirstReferralDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CoverageStartDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CoverageEndDate { get; set; }

        public int? IndustryNamePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail IndustryNamePickListDetail { get; set; }

        public int? ReferredTypePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail ReferredTypePickListDetail { get; set; }

        [MaxLength(128)]
        public string WonVersion { get; set; }

        public bool HasRiGroupSlip { get; set; }

        [MaxLength(30), Index]
        public string RiGroupSlipCode { get; set; }

        public int? RiGroupSlipStatus { get; set; }

        public int? RiGroupSlipPersonInChargeId { get; set; }
        [ExcludeTrail]
        public virtual User RiGroupSlipPersonInCharge { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? RiGroupSlipConfirmationDate { get; set; }

        public int? RiGroupSlipVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingGroupReferralVersion RiGroupSlipVersion { get; set; }

        public int? RiGroupSlipTemplateId { get; set; }
        [ExcludeTrail]
        public virtual Template RiGroupSlipTemplate { get; set; }

        [Column(TypeName = "ntext")]
        public string RiGroupSlipSharePointLink { get; set; }

        [Column(TypeName = "ntext")]
        public string RiGroupSlipSharePointFolderPath { get; set; }

        [MaxLength(128)]
        public string QuotationPath { get; set; }

        public int? ReplyVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingGroupReferralVersion ReplyVersion { get; set; }

        public int? ReplyTemplateId { get; set; }
        [ExcludeTrail]
        public virtual Template ReplyTemplate { get; set; }

        [Column(TypeName = "ntext")]
        public string ReplySharePointLink { get; set; }

        [Column(TypeName = "ntext")]
        public string ReplySharePointFolderPath { get; set; }

        [Index]
        public int? TreatyPricingGroupMasterLetterId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingGroupMasterLetter TreatyPricingGroupMasterLetter { get; set; }

        public double? AverageScore { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }

        public TreatyPricingGroupReferral()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupReferrals.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingGroupReferral Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupReferrals.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingGroupReferrals.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(Id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, this);

                entity.CedantId = CedantId;
                entity.Code = Code;
                entity.Description = Description;
                entity.RiArrangementPickListDetailId = RiArrangementPickListDetailId;
                entity.InsuredGroupNameId = InsuredGroupNameId;
                entity.Status = Status;
                entity.WorkflowStatus = WorkflowStatus;
                entity.PrimaryTreatyPricingProductId = PrimaryTreatyPricingProductId;
                entity.PrimaryTreatyPricingProductVersionId = PrimaryTreatyPricingProductVersionId;
                entity.SecondaryTreatyPricingProductId = SecondaryTreatyPricingProductId;
                entity.SecondaryTreatyPricingProductVersionId = SecondaryTreatyPricingProductVersionId;
                entity.PolicyNumber = PolicyNumber;
                entity.FirstReferralDate = FirstReferralDate;
                entity.CoverageStartDate = CoverageStartDate;
                entity.CoverageEndDate = CoverageEndDate;
                entity.IndustryNamePickListDetailId = IndustryNamePickListDetailId;
                entity.ReferredTypePickListDetailId = ReferredTypePickListDetailId;
                entity.WonVersion = WonVersion;
                entity.HasRiGroupSlip = HasRiGroupSlip;
                entity.RiGroupSlipCode = RiGroupSlipCode;
                entity.RiGroupSlipStatus = RiGroupSlipStatus;
                entity.RiGroupSlipPersonInChargeId = RiGroupSlipPersonInChargeId;
                entity.RiGroupSlipConfirmationDate = RiGroupSlipConfirmationDate;
                entity.RiGroupSlipVersionId = RiGroupSlipVersionId;
                entity.RiGroupSlipTemplateId = RiGroupSlipTemplateId;
                entity.RiGroupSlipSharePointLink = RiGroupSlipSharePointLink;
                entity.RiGroupSlipSharePointFolderPath = RiGroupSlipSharePointFolderPath;
                entity.QuotationPath = QuotationPath;
                entity.ReplyVersionId = ReplyVersionId;
                entity.ReplyTemplateId = ReplyTemplateId;
                entity.ReplySharePointLink = ReplySharePointLink;
                entity.ReplySharePointFolderPath = ReplySharePointFolderPath;
                entity.TreatyPricingGroupMasterLetterId = TreatyPricingGroupMasterLetterId;
                entity.AverageScore = AverageScore;
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
                var entity = db.TreatyPricingGroupReferrals.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.TreatyPricingGroupReferrals.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
