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

namespace DataAccess.Entities
{
    [Table("Salutations")]
    public class Salutation
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(30)]
        public string Name  { get; set; }

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

        public Salutation()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Salutations.Any(q => q.Id == id);
            }
        }

        public static Salutation Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Salutations.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.Salutations.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                Salutation salutations = Find(Id);
                if (salutations == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(salutations, this);

                salutations.Name = Name;
                salutations.UpdatedAt = DateTime.Now;
                salutations.UpdatedById = UpdatedById ?? salutations.UpdatedById;

                db.Entry(salutations).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                Salutation salutations = db.Salutations.Where(q => q.Id == id).FirstOrDefault();
                if (salutations == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(salutations, true);

                db.Entry(salutations).State = EntityState.Deleted;
                db.Salutations.Remove(salutations);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
