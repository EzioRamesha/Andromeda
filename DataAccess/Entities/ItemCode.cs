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
    [Table("ItemCodes")]
    public class ItemCode
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(10), Index]
        public string Code { get; set; }

        [Required, Index]
        public int ReportingType { get; set; }

        [Required, MaxLength(255)]
        public string Description { get; set; }

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

        [Index]
        public int? BusinessOriginPickListDetailId { get; set; }

        [ForeignKey(nameof(BusinessOriginPickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail BusinessOriginPickListDetail { get; set; }

        public ItemCode()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ItemCodes.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicate()
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(Code))
                {
                    var query = db.ItemCodes
                        .Where(q => q.Code.Trim().Equals(Code.Trim(), StringComparison.OrdinalIgnoreCase))
                        .Where(q => q.ReportingType == ReportingType)
                        .Where(q => q.BusinessOriginPickListDetailId == BusinessOriginPickListDetailId);

                    if (Id != 0)
                    {
                        query = query.Where(q => q.Id != Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static ItemCode Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ItemCodes.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static ItemCode FindByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return db.ItemCodes.Where(q => q.Code == code).FirstOrDefault();
            }
        }

        public static IList<ItemCode> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.ItemCodes.OrderBy(q => q.Code).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ItemCodes.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                ItemCode itemCode = ItemCode.Find(Id);
                if (itemCode == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(itemCode, this);

                itemCode.Code = Code;
                itemCode.ReportingType = ReportingType;
                itemCode.Description = Description;
                itemCode.UpdatedAt = DateTime.Now;
                itemCode.UpdatedById = UpdatedById ?? itemCode.UpdatedById;
                itemCode.BusinessOriginPickListDetailId = BusinessOriginPickListDetailId;

                db.Entry(itemCode).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                ItemCode itemCodes = db.ItemCodes.Where(q => q.Id == id).FirstOrDefault();
                if (itemCodes == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(itemCodes, true);

                db.Entry(itemCodes).State = EntityState.Deleted;
                db.ItemCodes.Remove(itemCodes);
                db.SaveChanges();

                return trail;
            }
        }

        public override string ToString()
        {
            string boCode = BusinessOriginPickListDetail != null ? BusinessOriginPickListDetail.Code.ToString() : "";

            if (string.IsNullOrEmpty(Description))
            {
                return string.Format("{0} - {1}", boCode, Code);
            }

            return string.Format("{0} - {1} - {2}", boCode, Code, Description);
        }
    }
}
