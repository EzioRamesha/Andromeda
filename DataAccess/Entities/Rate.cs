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
    [Table("Rates")]
    public class Rate
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50), Index]
        public string Code { get; set; }

        [Required, Index]
        public int ValuationRate { get; set; }

        [Required, Index]
        public int RatePerBasis { get; set; }

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

        public Rate()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Rates.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateCode()
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(Code?.Trim()))
                {
                    var query = db.Rates.Where(q => q.Code.Trim().Equals(Code.Trim(), StringComparison.OrdinalIgnoreCase));
                    if (Id != 0)
                    {
                        query = query.Where(q => q.Id != Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static Rate Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Rates.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static Rate Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.Rates.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static Rate FindByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return db.Rates.Where(q => q.Code.Trim() == code.Trim()).FirstOrDefault();
            }
        }

        public static int CountByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return db.Rates.Where(q => q.Code.Trim() == code.Trim()).Count();
            }
        }

        public static IList<Rate> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.Rates.OrderBy(q => q.Code).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.Rates.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                Rate rate = Rate.Find(Id);
                if (rate == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(rate, this);

                rate.Code = Code;
                rate.ValuationRate = ValuationRate;
                rate.RatePerBasis = RatePerBasis;
                rate.UpdatedAt = DateTime.Now;
                rate.UpdatedById = UpdatedById ?? rate.UpdatedById;

                db.Entry(rate).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                Rate rate = db.Rates.Where(q => q.Id == id).FirstOrDefault();
                if (rate == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(rate, true);

                db.Entry(rate).State = EntityState.Deleted;
                db.Rates.Remove(rate);
                db.SaveChanges();

                return trail;
            }
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Code))
            {
                return Code;
            }
            return string.Format("{0}", Code);
        }
    }
}
