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
    [Table("TreatyPricingTreatyWorkflowVersions")]
    public class TreatyPricingTreatyWorkflowVersion
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingTreatyWorkflowId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingTreatyWorkflow TreatyPricingTreatyWorkflow { get; set; }

        [Required, Index]
        public int Version { get; set; }

        #region General
        [Column(TypeName = "datetime2")]
        public DateTime? RequestDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? TargetSentDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DateSentToReviewer1st { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DateSentToClient1st { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? LatestRevisionDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? SignedDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ReportedDate { get; set; }

        public int? PersonInChargeId { get; set; }
        [ExcludeTrail]
        public virtual User PersonInCharge { get; set; }

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
        #endregion

        public TreatyPricingTreatyWorkflowVersion()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflowVersions.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingTreatyWorkflowVersion Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflowVersions.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingTreatyWorkflowVersions.Add(this);
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

                entity.TreatyPricingTreatyWorkflowId = TreatyPricingTreatyWorkflowId;
                entity.Version = Version;

                //General
                entity.RequestDate = RequestDate;
                entity.TargetSentDate = TargetSentDate;
                entity.DateSentToClient1st = DateSentToClient1st;
                entity.DateSentToReviewer1st = DateSentToReviewer1st;
                entity.LatestRevisionDate = LatestRevisionDate;
                entity.SignedDate = SignedDate;
                entity.ReportedDate = ReportedDate;
                entity.PersonInChargeId = PersonInChargeId;

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
                var entity = Find(id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.TreatyPricingTreatyWorkflowVersions.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingTreatyWorkflowId(int treatyPricingTreatyWorkflowId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingTreatyWorkflowVersions.Where(q => q.TreatyPricingTreatyWorkflowId == treatyPricingTreatyWorkflowId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (TreatyPricingTreatyWorkflowVersion entity in query.ToList())
                {
                    DataTrail trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.TreatyPricingTreatyWorkflowVersions.Remove(entity);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
