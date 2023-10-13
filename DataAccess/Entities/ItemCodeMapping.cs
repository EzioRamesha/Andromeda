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
    [Table("ItemCodeMappings")]
    public class ItemCodeMapping
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int ItemCodeId { get; set; }

        [ForeignKey(nameof(ItemCodeId))]
        [ExcludeTrail]
        public virtual ItemCode ItemCode { get; set; }

        [Index]
        public int? InvoiceFieldPickListDetailId { get; set; }

        [ForeignKey(nameof(InvoiceFieldPickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail InvoiceFieldPickListDetail { get; set; }

        public string TreatyType { get; set; }

        public string TreatyCode { get; set; }

        [Index]
        public int? BusinessOriginPickListDetailId { get; set; }

        [ForeignKey(nameof(BusinessOriginPickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail BusinessOriginPickListDetail { get; set; }

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

        [ExcludeTrail]
        public virtual ICollection<ItemCodeMappingDetail> ItemCodeMappingDetails { get; set; }

        public ItemCodeMapping()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ItemCodeMappings.Any(q => q.Id == id);
            }
        }

        //public bool IsDuplicateCode()
        //{
        //    using (var db = new AppDbContext())
        //    {
        //        if (TreatyTypePickListDetailId != 0 && InvoiceFieldPickListDetailId != 0)
        //        {
        //            var query = db.ItemCodeMappings.Where(q => q.TreatyTypePickListDetailId == TreatyTypePickListDetailId && q.InvoiceFieldPickListDetailId == InvoiceFieldPickListDetailId);
        //            if (Id != 0)
        //            {
        //                query = query.Where(q => q.Id != Id);
        //            }
        //            return query.Count() > 0;
        //        }
        //        return false;
        //    }
        //}

        public static ItemCodeMapping Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ItemCodeMappings.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        //public static int CountByParams(int itemCodeId, int treatyTypePickListDetailId, int? id = null)
        //{
        //    using (var db = new AppDbContext())
        //    {
        //        var query = db.ItemCodeMappings
        //            .Where(q => q.ItemCodeId == itemCodeId)
        //            .Where(q => q.TreatyTypePickListDetailId == treatyTypePickListDetailId);
        //        if (id != 0)
        //        {
        //            query = query.Where(q => q.Id != id);
        //        }
        //        return query.Count();
        //    }
        //}

        public static int CountByItemCodeId(int itemCodeId)
        {
            using (var db = new AppDbContext())
            {
                return db.ItemCodeMappings.Where(q => q.ItemCodeId == itemCodeId).Count();
            }
        }

        //public static int CountByTreatyTypePickListDetailId(int treatyTypePickListDetailId)
        //{
        //    using (var db = new AppDbContext())
        //    {
        //        return db.ItemCodeMappings.Where(q => q.TreatyTypePickListDetailId == treatyTypePickListDetailId).Count();
        //    }
        //}

        //public static IList<ItemCodeMapping> GetByTreatyTypePickListDetailId(int treatyTypePickListDetailId)
        //{
        //    using (var db = new AppDbContext())
        //    {
        //        return db.ItemCodeMappings.Where(q => q.TreatyTypePickListDetailId == treatyTypePickListDetailId).ToList();
        //    }
        //}

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ItemCodeMappings.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                ItemCodeMapping itemCodeMappings = ItemCodeMapping.Find(Id);
                if (itemCodeMappings == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(itemCodeMappings, this);

                itemCodeMappings.ItemCodeId = ItemCodeId;
                itemCodeMappings.InvoiceFieldPickListDetailId = InvoiceFieldPickListDetailId;
                itemCodeMappings.TreatyType = TreatyType;
                itemCodeMappings.TreatyCode = TreatyCode;
                itemCodeMappings.BusinessOriginPickListDetailId = BusinessOriginPickListDetailId;
                itemCodeMappings.UpdatedAt = DateTime.Now;
                itemCodeMappings.UpdatedById = UpdatedById ?? itemCodeMappings.UpdatedById;

                db.Entry(itemCodeMappings).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                ItemCodeMapping itemCodeMappings = db.ItemCodeMappings.Where(q => q.Id == id).FirstOrDefault();
                if (itemCodeMappings == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(itemCodeMappings, true);

                db.Entry(itemCodeMappings).State = EntityState.Deleted;
                db.ItemCodeMappings.Remove(itemCodeMappings);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
