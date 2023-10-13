using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Shared;
using Shared.Trails;
using Shared.Trails.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataAccess.Entities.TreatyPricing
{
    [Table("HipsCategories")]
    public class HipsCategory
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(255), Index]
        public string Code { get; set; }

        [Required, MaxLength(255), Index]
        public string Name { get; set; }

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

        public HipsCategory()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.HipsCategories.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateCode()
        {
            using (var db = new AppDbContext())
            {
                var query = db.HipsCategories.Where(q => q.Code == Code);
                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public static HipsCategory Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.HipsCategories.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.HipsCategories.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                HipsCategory hipsCategory = Find(Id);
                if (hipsCategory == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(hipsCategory, this);

                hipsCategory.Code = Code;
                hipsCategory.Name = Name;
                hipsCategory.UpdatedAt = DateTime.Now;
                hipsCategory.UpdatedById = UpdatedById ?? hipsCategory.UpdatedById;

                db.Entry(hipsCategory).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                HipsCategory hipsCategory = db.HipsCategories.Where(q => q.Id == id).FirstOrDefault();
                if (hipsCategory == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(hipsCategory, true);

                db.Entry(hipsCategory).State = EntityState.Deleted;
                db.HipsCategories.Remove(hipsCategory);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
