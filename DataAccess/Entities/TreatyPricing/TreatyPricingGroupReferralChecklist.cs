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
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities.TreatyPricing
{
    [Table("TreatyPricingGroupReferralChecklists")]
    public class TreatyPricingGroupReferralChecklist
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingGroupReferralVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingGroupReferralVersion TreatyPricingGroupReferralVersion { get; set; }

        [Index]
        public int InternalTeam { get; set; }

        [MaxLength(255), Index]
        public string InternalTeamPersonInCharge { get; set; }

        [Index]
        public int Status { get; set; }

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

        public TreatyPricingGroupReferralChecklist()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupReferralChecklists.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingGroupReferralChecklist Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupReferralChecklists.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<TreatyPricingGroupReferralChecklist> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupReferralChecklists.OrderBy(q => q.InternalTeam).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingGroupReferralChecklists.Add(this);
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
                entity.InternalTeam = InternalTeam;
                entity.InternalTeamPersonInCharge = InternalTeamPersonInCharge;
                entity.Status = Status;
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
                db.TreatyPricingGroupReferralChecklists.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
