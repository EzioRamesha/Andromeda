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
    [Table("ClaimChecklists")]
    public class ClaimChecklist
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int ClaimCodeId { get; set; }

        [ForeignKey(nameof(ClaimCodeId))]
        [ExcludeTrail]
        public virtual ClaimCode ClaimCode { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        [ForeignKey(nameof(CreatedById))]
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        [ForeignKey(nameof(UpdatedById))]
        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }

        public ClaimChecklist()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimChecklists.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateClaimCode()
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimChecklists.Where(q => q.ClaimCodeId == ClaimCodeId);
                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public static ClaimChecklist Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimChecklists.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<ClaimChecklist> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimChecklists.OrderBy(q => q.ClaimCode.Code).ToList();
            }
        }

        public static IList<ClaimChecklist> GetByClaimCodeId(int claimCodeId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimChecklists.Where(q => q.ClaimCodeId == claimCodeId).ToList();
            }
        }

        public static int CountByClaimCodeId(int claimCodeId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimChecklists.Where(q => q.ClaimCodeId == claimCodeId).Count();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ClaimChecklists.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                ClaimChecklist claimChecklist = ClaimChecklist.Find(Id);
                if (claimChecklist == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimChecklist, this);

                claimChecklist.ClaimCodeId = ClaimCodeId;
                claimChecklist.UpdatedAt = DateTime.Now;
                claimChecklist.UpdatedById = UpdatedById ?? claimChecklist.UpdatedById;

                db.Entry(claimChecklist).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                ClaimChecklist claimChecklist = db.ClaimChecklists.Where(q => q.Id == id).FirstOrDefault();
                if (claimChecklist == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimChecklist, true);

                db.Entry(claimChecklist).State = EntityState.Deleted;
                db.ClaimChecklists.Remove(claimChecklist);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
