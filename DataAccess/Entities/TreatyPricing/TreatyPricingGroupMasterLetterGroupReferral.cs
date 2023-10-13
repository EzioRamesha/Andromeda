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
    [Table("TreatyPricingGroupMasterLetterGroupReferrals")]
    public class TreatyPricingGroupMasterLetterGroupReferral
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingGroupMasterLetterId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingGroupMasterLetter TreatyPricingGroupMasterLetter { get; set; }

        [Required, Index]
        public int TreatyPricingGroupReferralId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingGroupReferral TreatyPricingGroupReferral { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        public TreatyPricingGroupMasterLetterGroupReferral()
        {
            CreatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupMasterLetterGroupReferrals.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingGroupMasterLetterGroupReferral Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupMasterLetterGroupReferrals.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingGroupMasterLetterGroupReferrals.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
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
                db.TreatyPricingGroupMasterLetterGroupReferrals.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
