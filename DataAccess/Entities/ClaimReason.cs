using BusinessObject;
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

namespace DataAccess.Entities
{
    [Table("ClaimReasons")]
    public class ClaimReason
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int Type { get; set; }

        [MaxLength(255)]
        public string Reason { get; set; }

        [MaxLength(255)]
        public string Remark { get; set; }

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

        public ClaimReason()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimReasons.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateReason()
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimReasons.Where(q => q.Type == Type)
                    .Where(q => q.Reason.Trim().Equals(Reason.Trim(), StringComparison.OrdinalIgnoreCase));
                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public static ClaimReason Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimReasons.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<ClaimReason> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimReasons.OrderBy(q => q.Type).ToList();
            }
        }

        public static IList<ClaimReason> GetByType(int type)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimReasons.Where(q => q.Type == type).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ClaimReasons.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                ClaimReason claimReason = Find(Id);
                if (claimReason == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimReason, this);

                claimReason.Type = Type;
                claimReason.Reason = Reason;
                claimReason.Remark = Remark;
                claimReason.UpdatedAt = DateTime.Now;
                claimReason.UpdatedById = UpdatedById ?? claimReason.UpdatedById;

                db.Entry(claimReason).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                ClaimReason claimReason = db.ClaimReasons.Where(q => q.Id == id).FirstOrDefault();
                if (claimReason == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimReason, true);

                db.Entry(claimReason).State = EntityState.Deleted;
                db.ClaimReasons.Remove(claimReason);
                db.SaveChanges();

                return trail;
            }
        }

        public static void SeedClaimReason(User superUser)
        {
            var trail = new TrailObject();
            string table = UtilAttribute.GetTableName(typeof(Module));

            foreach (var claimReasonBo in ClaimReasonBo.GetClaimReasons())
            {
                //trail = new TrailObject();
                //DataTrail dataTrail = null;

                var entity = new ClaimReason();
                entity.Type = claimReasonBo.Type;
                entity.Reason = claimReasonBo.Reason;
                entity.Remark = claimReasonBo.Remark;
                entity.CreatedById = superUser.Id;
                entity.UpdatedById = superUser.Id;
                entity.CreatedAt = DateTime.Now;
                entity.UpdatedAt = DateTime.Now;

                if (entity.IsDuplicateReason())
                    continue;

                entity.Create();
            }
        }
    }
}
