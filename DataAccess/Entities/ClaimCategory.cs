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
    [Table("ClaimCategories")]
    public class ClaimCategory
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(255), Index]
        public string Category { get; set; }

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

        public ClaimCategory()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimCategories.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateCategory()
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimCategories.Where(q => q.Category.Trim().Equals(Category.Trim(), StringComparison.OrdinalIgnoreCase));
                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public static ClaimCategory Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimCategories.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<ClaimCategory> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimCategories.OrderBy(q => q.Category).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ClaimCategories.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                ClaimCategory claimCategory = Find(Id);
                if (claimCategory == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimCategory, this);

                claimCategory.Category = Category;
                claimCategory.Remark = Remark;
                claimCategory.UpdatedAt = DateTime.Now;
                claimCategory.UpdatedById = UpdatedById ?? claimCategory.UpdatedById;

                db.Entry(claimCategory).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                ClaimCategory claimCategory = db.ClaimCategories.Where(q => q.Id == id).FirstOrDefault();
                if (claimCategory == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimCategory, true);

                db.Entry(claimCategory).State = EntityState.Deleted;
                db.ClaimCategories.Remove(claimCategory);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
