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
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    [Table("GroupDiscounts")]
    public class GroupDiscount
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
        public int GroupSizeFrom { get; set; }

        [Required, Index]
        public int GroupSizeTo { get; set; }

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

        public GroupDiscount()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.GroupDiscounts.Any(q => q.Id == id);
            }
        }

        public static GroupDiscount Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.GroupDiscounts.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static GroupDiscount Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.GroupDiscounts.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static GroupDiscount FindByDiscountCode(string discountCode)
        {
            using (var db = new AppDbContext())
            {
                return db.GroupDiscounts.Where(q => q.DiscountCode.Trim() == discountCode.Trim()).FirstOrDefault();
            }
        }

        public static IList<GroupDiscount> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.GroupDiscounts.ToList();
            }
        }

        public static IList<GroupDiscount> GetByDiscountTableId(int discountTableId)
        {
            using (var db = new AppDbContext())
            {
                return db.GroupDiscounts.Where(q => q.DiscountTableId == discountTableId).ToList();
            }
        }

        public static IList<GroupDiscount> GetByDiscountTableIdExcept(int discountTableId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.GroupDiscounts.Where(q => q.DiscountTableId == discountTableId && !ids.Contains(q.Id)).ToList();
            }
        }

        public static IList<GroupDiscount> GetByCedantId(int cedantId, bool isDistinctCode = false)
        {
            using (var db = new AppDbContext())
            {
                var query = db.GroupDiscounts.Where(q => q.DiscountTable.CedantId == cedantId).ToList();

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
                db.GroupDiscounts.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                GroupDiscount groupDiscount = GroupDiscount.Find(Id);
                if (groupDiscount == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(groupDiscount, this);

                groupDiscount.DiscountTableId = DiscountTableId;
                groupDiscount.DiscountCode = DiscountCode;
                groupDiscount.EffectiveStartDate = EffectiveStartDate;
                groupDiscount.EffectiveEndDate = EffectiveEndDate;
                groupDiscount.GroupSizeFrom = GroupSizeFrom;
                groupDiscount.GroupSizeTo = GroupSizeTo;
                groupDiscount.Discount = Discount;
                groupDiscount.UpdatedAt = DateTime.Now;
                groupDiscount.UpdatedById = UpdatedById ?? groupDiscount.UpdatedById;

                db.Entry(groupDiscount).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                GroupDiscount groupDiscount = db.GroupDiscounts.Where(q => q.Id == id).FirstOrDefault();
                if (groupDiscount == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(groupDiscount, true);

                db.Entry(groupDiscount).State = EntityState.Deleted;
                db.GroupDiscounts.Remove(groupDiscount);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByDiscountTableId(int discountTableId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.GroupDiscounts.Where(q => q.DiscountTableId == discountTableId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (GroupDiscount groupDiscount in query.ToList())
                {
                    DataTrail trail = new DataTrail(groupDiscount, true);
                    trails.Add(trail);

                    db.Entry(groupDiscount).State = EntityState.Deleted;
                    db.GroupDiscounts.Remove(groupDiscount);
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
