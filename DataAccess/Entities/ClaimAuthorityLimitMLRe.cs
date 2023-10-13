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
    [Table("ClaimAuthorityLimitMLRe")]
    public class ClaimAuthorityLimitMLRe
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int DepartmentId { get; set; }

        [ExcludeTrail]
        public virtual Department Department { get; set; }

        [Required, Index]
        public int UserId { get; set; }

        [ExcludeTrail]
        public virtual User User { get; set; }

        [MaxLength(255)]
        public string Position { get; set; }

        [Index]
        public bool IsAllowOverwriteApproval { get; set; }

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

        [ExcludeTrail]
        public virtual ICollection<ClaimAuthorityLimitMLReDetail> ClaimAuthorityLimitMLReDetails { get; set; }

        public ClaimAuthorityLimitMLRe()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimAuthorityLimitMLRe.Any(q => q.Id == id);
            }
        }

        public static ClaimAuthorityLimitMLRe Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimAuthorityLimitMLRe.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static int Count()
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimAuthorityLimitMLRe.Count();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ClaimAuthorityLimitMLRe.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                ClaimAuthorityLimitMLRe claimAuthorityLimitMLRe = ClaimAuthorityLimitMLRe.Find(Id);
                if (claimAuthorityLimitMLRe == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimAuthorityLimitMLRe, this);

                claimAuthorityLimitMLRe.DepartmentId = DepartmentId;
                claimAuthorityLimitMLRe.UserId = UserId;
                claimAuthorityLimitMLRe.Position = Position;
                claimAuthorityLimitMLRe.IsAllowOverwriteApproval = IsAllowOverwriteApproval;
                claimAuthorityLimitMLRe.UpdatedAt = DateTime.Now;
                claimAuthorityLimitMLRe.UpdatedById = UpdatedById ?? claimAuthorityLimitMLRe.UpdatedById;

                db.Entry(claimAuthorityLimitMLRe).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                ClaimAuthorityLimitMLRe claimAuthorityLimitMLRe = db.ClaimAuthorityLimitMLRe.Where(q => q.Id == id).FirstOrDefault();
                if (claimAuthorityLimitMLRe == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimAuthorityLimitMLRe, true);

                db.Entry(claimAuthorityLimitMLRe).State = EntityState.Deleted;
                db.ClaimAuthorityLimitMLRe.Remove(claimAuthorityLimitMLRe);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
