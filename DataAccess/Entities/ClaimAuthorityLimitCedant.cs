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
    [Table("ClaimAuthorityLimitCedants")]
    public class ClaimAuthorityLimitCedant
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int CedantId { get; set; }

        [ExcludeTrail]
        public virtual Cedant Cedant { get; set; }

        [MaxLength(255), Index]
        public string Remarks { get; set; }

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
        public virtual ICollection<ClaimAuthorityLimitCedantDetail> ClaimAuthorityLimitCedantDetails { get; set; }

        public ClaimAuthorityLimitCedant()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimAuthorityLimitCedants.Any(q => q.Id == id);
            }
        }

        public static ClaimAuthorityLimitCedant Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimAuthorityLimitCedants.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static ClaimAuthorityLimitCedant FindByCedant(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimAuthorityLimitCedants.Where(q => q.CedantId == cedantId).FirstOrDefault();
            }
        }

        public static int Count()
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimAuthorityLimitCedants.Count();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ClaimAuthorityLimitCedants.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                ClaimAuthorityLimitCedant claimAuthorityLimitCedants = Find(Id);
                if (claimAuthorityLimitCedants == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimAuthorityLimitCedants, this);

                claimAuthorityLimitCedants.CedantId = CedantId;
                claimAuthorityLimitCedants.Remarks = Remarks;
                claimAuthorityLimitCedants.UpdatedAt = DateTime.Now;
                claimAuthorityLimitCedants.UpdatedById = UpdatedById ?? claimAuthorityLimitCedants.UpdatedById;

                db.Entry(claimAuthorityLimitCedants).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                ClaimAuthorityLimitCedant claimAuthorityLimitCedants = db.ClaimAuthorityLimitCedants.Where(q => q.Id == id).FirstOrDefault();
                if (claimAuthorityLimitCedants == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimAuthorityLimitCedants, true);

                db.Entry(claimAuthorityLimitCedants).State = EntityState.Deleted;
                db.ClaimAuthorityLimitCedants.Remove(claimAuthorityLimitCedants);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
