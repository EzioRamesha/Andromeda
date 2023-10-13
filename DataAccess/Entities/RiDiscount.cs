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
    [Table("RiDiscounts")]
    public class RiDiscount
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

        [Index]
        public double? DurationFrom { get; set; }

        [Index]
        public double? DurationTo { get; set; }

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

        public RiDiscount()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDiscounts.Any(q => q.Id == id);
            }
        }

        public static RiDiscount Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDiscounts.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static RiDiscount Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDiscounts.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static RiDiscount FindByDiscountCode(string discountCode)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDiscounts.Where(q => q.DiscountCode.Trim() == discountCode.Trim()).FirstOrDefault();
            }
        }

        public static IList<RiDiscount> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.RiDiscounts.ToList();
            }
        }

        public static IList<RiDiscount> GetByDiscountTableId(int discountTableId)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDiscounts.Where(q => q.DiscountTableId == discountTableId).ToList();
            }
        }

        public static IList<RiDiscount> GetByDiscountTableIdExcept(int discountTableId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDiscounts.Where(q => q.DiscountTableId == discountTableId && !ids.Contains(q.Id)).ToList();
            }
        }

        public static IList<RiDiscount> GetByCedantId(int cedantId, bool isDistinctCode = false)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RiDiscounts.Where(q => q.DiscountTable.CedantId == cedantId);

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
                db.RiDiscounts.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                RiDiscount riDiscount = RiDiscount.Find(Id);
                if (riDiscount == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(riDiscount, this);

                riDiscount.DiscountTableId = DiscountTableId;
                riDiscount.DiscountCode = DiscountCode;
                riDiscount.EffectiveStartDate = EffectiveStartDate;
                riDiscount.EffectiveEndDate = EffectiveEndDate;
                riDiscount.DurationFrom = DurationFrom;
                riDiscount.DurationTo = DurationTo;
                riDiscount.Discount = Discount;
                riDiscount.UpdatedAt = DateTime.Now;
                riDiscount.UpdatedById = UpdatedById ?? riDiscount.UpdatedById;

                db.Entry(riDiscount).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                RiDiscount riDiscount = db.RiDiscounts.Where(q => q.Id == id).FirstOrDefault();
                if (riDiscount == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(riDiscount, true);

                db.Entry(riDiscount).State = EntityState.Deleted;
                db.RiDiscounts.Remove(riDiscount);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByDiscountTableId(int discountTableId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RiDiscounts.Where(q => q.DiscountTableId == discountTableId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (RiDiscount riDiscount in query.ToList())
                {
                    DataTrail trail = new DataTrail(riDiscount, true);
                    trails.Add(trail);

                    db.Entry(riDiscount).State = EntityState.Deleted;
                    db.RiDiscounts.Remove(riDiscount);
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
