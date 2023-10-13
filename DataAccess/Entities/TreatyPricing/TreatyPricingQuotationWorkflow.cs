using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Shared;
using Shared.Trails;
using Shared.Trails.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataAccess.Entities.TreatyPricing
{
    [Table("TreatyPricingQuotationWorkflows")]
    public class TreatyPricingQuotationWorkflow
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(30), Index]
        public string QuotationId { get; set; }

        [Required, Index]
        public int CedantId { get; set; }
        [ExcludeTrail]
        public virtual Cedant Cedant { get; set; }

        [Index]
        public int? ReinsuranceTypePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail ReinsuranceTypePickListDetail { get; set; }

        [MaxLength(255), Index]
        public string Name { get; set; }

        [Column(TypeName = "ntext")]
        public string Summary { get; set; }

        [Index]
        public int Status { get; set; }

        [Column(TypeName = "ntext")]
        public string StatusRemarks { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? TargetSendDate { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? LatestRevisionDate { get; set; }

        [Index]
        public int? PricingTeamPickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail PricingTeamPickListDetail { get; set; }

        [Index]
        public int? PricingStatus { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? TargetClientReleaseDate { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? TargetRateCompletionDate { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? FinaliseDate { get; set; }

        [MaxLength(255), Index]
        public string Description { get; set; }

        public int LatestVersion { get; set; }

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

        #region Properties - Quotation and Pricing dashboard
        public int? BDPersonInChargeId { get; set; }

        public int? PersonInChargeId { get; set; }

        [ExcludeTrail]
        public virtual User BDPersonInCharge { get; set; }

        [ExcludeTrail]
        public virtual User PersonInCharge { get; set; }

        public int CEOPending { get; set; }

        public int PricingPending { get; set; }

        public int UnderwritingPending { get; set; }

        public int HealthPending { get; set; }

        public int ClaimsPending { get; set; }

        public int BDPending { get; set; }

        public int TGPending { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? PricingDueDate { get; set; }
        #endregion

        public TreatyPricingQuotationWorkflow()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingQuotationWorkflows.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingQuotationWorkflow Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingQuotationWorkflows.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingQuotationWorkflows.Add(this);
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
                entity.QuotationId = QuotationId;
                entity.ReinsuranceTypePickListDetailId = ReinsuranceTypePickListDetailId;
                entity.Name = Name;
                entity.Summary = Summary;
                entity.Status = Status;
                entity.StatusRemarks = StatusRemarks;
                entity.TargetSendDate = TargetSendDate;
                entity.LatestRevisionDate = LatestRevisionDate;
                entity.PricingTeamPickListDetailId = PricingTeamPickListDetailId;
                entity.PricingStatus = PricingStatus;
                entity.TargetClientReleaseDate = TargetClientReleaseDate;
                entity.TargetRateCompletionDate = TargetRateCompletionDate;
                entity.FinaliseDate = FinaliseDate;
                entity.Description = Description;
                entity.LatestVersion = LatestVersion;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = UpdatedById ?? entity.UpdatedById;

                entity.BDPersonInChargeId = BDPersonInChargeId;
                entity.PersonInChargeId = PersonInChargeId;
                entity.CEOPending = CEOPending;
                entity.PricingPending = PricingPending;
                entity.UnderwritingPending = UnderwritingPending;
                entity.HealthPending = HealthPending;
                entity.ClaimsPending = ClaimsPending;
                entity.BDPending = BDPending;
                entity.TGPending = TGPending;
                entity.PricingDueDate = PricingDueDate;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.TreatyPricingQuotationWorkflows.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public string GetReinsuranceType(int? reinsuranceTypePickListDetailId = 0)
        {
            using (var db = new AppDbContext())
            {
                return db.PickListDetails.Where(q => q.Id == reinsuranceTypePickListDetailId).FirstOrDefault().Code;
            }
        }

        public string GetPricingTeam(int? pricingTeamPickListDetailId = 0)
        {
            using (var db = new AppDbContext())
            {
                if (!pricingTeamPickListDetailId.HasValue || pricingTeamPickListDetailId.Value == 0)
                    return "";

                return db.PickListDetails.Where(q => q.Id == pricingTeamPickListDetailId).FirstOrDefault().Code;
            }
        }
    }
}
