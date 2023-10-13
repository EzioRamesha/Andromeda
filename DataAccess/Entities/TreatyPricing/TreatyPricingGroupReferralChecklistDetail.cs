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
    [Table("TreatyPricingGroupReferralChecklistDetails")]
    public class TreatyPricingGroupReferralChecklistDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingGroupReferralVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingGroupReferralVersion TreatyPricingGroupReferralVersion { get; set; }

        [Index]
        public int InternalItem { get; set; }

        public bool Underwriting { get; set; }
        public bool Health { get; set; }
        public bool Claim { get; set; }
        public bool BD { get; set; }
        public bool CnR { get; set; }
        public int? UltimateApprover { get; set; }
        public bool GroupTeamApprover { get; set; }
        public bool ReviewerApprover { get; set; }
        public bool HODApprover { get; set; }
        public bool CEOApprover { get; set; }

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

        public TreatyPricingGroupReferralChecklistDetail()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupReferralChecklistDetails.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingGroupReferralChecklistDetail Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupReferralChecklistDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<TreatyPricingGroupReferralChecklistDetail> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupReferralChecklistDetails.OrderBy(q => q.InternalItem).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingGroupReferralChecklistDetails.Add(this);
                db.SaveChanges();

                return new DataTrail(this);
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(Id);
                if (entity == null)
                    throw new Exception(MessageBag.NoRecordFound);

                var trail = new DataTrail(entity, this);

                entity.TreatyPricingGroupReferralVersionId = TreatyPricingGroupReferralVersionId;
                entity.InternalItem = InternalItem;
                entity.Underwriting = Underwriting;
                entity.Health = Health;
                entity.Claim = Claim;
                entity.BD = BD;
                entity.CnR = CnR;
                entity.UltimateApprover = UltimateApprover;
                entity.GroupTeamApprover = GroupTeamApprover;
                entity.ReviewerApprover = ReviewerApprover;
                entity.HODApprover = HODApprover;
                entity.CEOApprover = CEOApprover;
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
                    throw new Exception(MessageBag.NoRecordFound);

                var trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.TreatyPricingGroupReferralChecklistDetails.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
