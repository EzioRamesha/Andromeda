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
    [Table("LargeDiscounts")]
    public class LargeDiscount
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int DiscountTableId { get; set; }

        [ForeignKey(nameof(DiscountTableId))]
        [ExcludeTrail]
        public virtual DiscountTable DiscountTable { get; set; }

        [Required, MaxLength(30), Index]
        public string DiscountCode { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? EffectiveStartDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? EffectiveEndDate { get; set; }

        [Required, Index]
        public double AarFrom { get; set; }

        [Required, Index]
        public double AarTo { get; set; }

        [Required, Index]
        public double Discount { get; set; }

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

        public LargeDiscount()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.LargeDiscounts.Any(q => q.Id == id);
            }
        }

        public static LargeDiscount Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.LargeDiscounts.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static LargeDiscount Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.LargeDiscounts.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static LargeDiscount FindByDiscountCode(string discountCode)
        {
            using (var db = new AppDbContext())
            {
                return db.LargeDiscounts.Where(q => q.DiscountCode.Trim() == discountCode.Trim()).FirstOrDefault();
            }
        }

        public static IList<LargeDiscount> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.LargeDiscounts.ToList();
            }
        }

        public static IList<LargeDiscount> GetByDiscountTableId(int discountTableId)
        {
            using (var db = new AppDbContext())
            {
                return db.LargeDiscounts.Where(q => q.DiscountTableId == discountTableId).ToList();
            }
        }

        public static IList<LargeDiscount> GetByDiscountTableIdExcept(int discountTableId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.LargeDiscounts.Where(q => q.DiscountTableId == discountTableId && !ids.Contains(q.Id)).ToList();
            }
        }

        public static IList<LargeDiscount> GetByCedantId(int cedantId, bool isDistinctCode = false)
        {
            using (var db = new AppDbContext())
            {
                var query = db.LargeDiscounts.Where(q => q.DiscountTable.CedantId == cedantId);

                if (isDistinctCode)
                {
                    return query.GroupBy(q => q.DiscountCode).Select(q => q.FirstOrDefault()).OrderBy(q => q.DiscountCode).ToList();
                }

                return query.OrderBy(q => q.DiscountCode).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.LargeDiscounts.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                LargeDiscount largeDiscount = LargeDiscount.Find(Id);
                if (largeDiscount == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(largeDiscount, this);

                largeDiscount.DiscountTableId = DiscountTableId;
                largeDiscount.DiscountCode = DiscountCode;
                largeDiscount.EffectiveStartDate = EffectiveStartDate;
                largeDiscount.EffectiveEndDate = EffectiveEndDate;
                largeDiscount.AarFrom = AarFrom;
                largeDiscount.AarTo = AarTo;
                largeDiscount.Discount = Discount;
                largeDiscount.UpdatedAt = DateTime.Now;
                largeDiscount.UpdatedById = UpdatedById ?? largeDiscount.UpdatedById;

                db.Entry(largeDiscount).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                LargeDiscount largeDiscount = db.LargeDiscounts.Where(q => q.Id == id).FirstOrDefault();
                if (largeDiscount == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(largeDiscount, true);

                db.Entry(largeDiscount).State = EntityState.Deleted;
                db.LargeDiscounts.Remove(largeDiscount);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByDiscountTableId(int discountTableId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.LargeDiscounts.Where(q => q.DiscountTableId == discountTableId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (LargeDiscount largeDiscount in query.ToList())
                {
                    DataTrail trail = new DataTrail(largeDiscount, true);
                    trails.Add(trail);

                    db.Entry(largeDiscount).State = EntityState.Deleted;
                    db.LargeDiscounts.Remove(largeDiscount);
                }

                db.SaveChanges();
                return trails;
            }
        }

        public override string ToString()
        {
            return DiscountCode;
        }
    }
}
