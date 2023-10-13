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
    [Table("UwQuestionnaireCategories")]
    public class UwQuestionnaireCategory
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

        public UwQuestionnaireCategory()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.UwQuestionnaireCategories.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateCode()
        {
            using (var db = new AppDbContext())
            {
                var query = db.UwQuestionnaireCategories.Where(q => q.Code == Code);
                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public bool IsDuplicateName()
        {
            using (var db = new AppDbContext())
            {
                var query = db.UwQuestionnaireCategories.Where(q => q.Name == Name);
                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public static UwQuestionnaireCategory Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.UwQuestionnaireCategories.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.UwQuestionnaireCategories.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                UwQuestionnaireCategory uwQuestionnaireCategory = Find(Id);
                if (uwQuestionnaireCategory == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(uwQuestionnaireCategory, this);

                uwQuestionnaireCategory.Code = Code;
                uwQuestionnaireCategory.Name = Name;
                uwQuestionnaireCategory.UpdatedAt = DateTime.Now;
                uwQuestionnaireCategory.UpdatedById = UpdatedById ?? uwQuestionnaireCategory.UpdatedById;

                db.Entry(uwQuestionnaireCategory).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                UwQuestionnaireCategory uwQuestionnaireCategory = db.UwQuestionnaireCategories.Where(q => q.Id == id).FirstOrDefault();
                if (uwQuestionnaireCategory == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(uwQuestionnaireCategory, true);

                db.Entry(uwQuestionnaireCategory).State = EntityState.Deleted;
                db.UwQuestionnaireCategories.Remove(uwQuestionnaireCategory);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
