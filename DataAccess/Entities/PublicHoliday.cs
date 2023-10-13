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
    [Table("PublicHolidays")]
    public class PublicHoliday
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int Year { get; set; }

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

        public PublicHoliday()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PublicHolidays.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateYear()
        {
            using (var db = new AppDbContext())
            {
                if (Year != 0)
                {
                    var query = db.PublicHolidays.Where(q => q.Year == Year);
                    if (Id != 0)
                    {
                        query = query.Where(q => q.Id != Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static PublicHoliday Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PublicHolidays.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static PublicHoliday FindByYear(int year)
        {
            using (var db = new AppDbContext())
            {
                return db.PublicHolidays.Where(q => q.Year == year).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PublicHolidays.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                PublicHoliday publicHoliday = PublicHoliday.Find(Id);
                if (publicHoliday == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(publicHoliday, this);

                publicHoliday.Year = Year;
                publicHoliday.UpdatedAt = DateTime.Now;
                publicHoliday.UpdatedById = UpdatedById ?? publicHoliday.UpdatedById;

                db.Entry(publicHoliday).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                PublicHoliday publicHoliday = db.PublicHolidays.Where(q => q.Id == id).FirstOrDefault();
                if (publicHoliday == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(publicHoliday, true);

                db.Entry(publicHoliday).State = EntityState.Deleted;
                db.PublicHolidays.Remove(publicHoliday);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
