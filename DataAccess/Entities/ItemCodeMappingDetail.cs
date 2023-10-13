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
    [Table("ItemCodeMappingDetails")]
    public class ItemCodeMappingDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int ItemCodeMappingId { get; set; }

        [ForeignKey(nameof(ItemCodeMappingId))]
        [ExcludeTrail]
        public virtual ItemCodeMapping ItemCodeMapping { get; set; }

        [MaxLength(20), Index]
        public string TreatyType { get; set; }

        [MaxLength(35), Index]
        public string TreatyCode { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        [ForeignKey(nameof(CreatedById))]
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        public ItemCodeMappingDetail()
        {
            CreatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ItemCodeMappingDetails.Any(q => q.Id == id);
            }
        }

        public static bool IsDuplicate(
            int? invoiceFieldId,
            string treatyType,
            string treatyCode = null,
            int? businessOriginId = null,
            int? itemCodeMappingId = null,
            int? ReportingType = null
        )
        {
            using (var db = new AppDbContext())
            {
                var query = db.ItemCodeMappingDetails
                          .Where(q => q.TreatyType == treatyType)
                          .Where(q => (q.ItemCodeMapping.InvoiceFieldPickListDetailId.HasValue &&
                          q.ItemCodeMapping.InvoiceFieldPickListDetailId == invoiceFieldId) ||
                          !q.ItemCodeMapping.InvoiceFieldPickListDetailId.HasValue)
                          .Where(q => (q.ItemCodeMapping.BusinessOriginPickListDetailId.HasValue &&
                          q.ItemCodeMapping.BusinessOriginPickListDetailId == businessOriginId) ||
                          !q.ItemCodeMapping.BusinessOriginPickListDetailId.HasValue)
                          .Where(q => q.ItemCodeMapping.ItemCode.ReportingType == ReportingType);

                if (!string.IsNullOrEmpty(treatyCode))
                {
                    query = query.Where(q => (!string.IsNullOrEmpty(q.TreatyCode) && q.TreatyCode == treatyCode) || string.IsNullOrEmpty(q.TreatyCode));
                }

                if (itemCodeMappingId != null)
                {
                    query = query.Where(q => q.ItemCodeMappingId != itemCodeMappingId);
                }

                return query.Any();
            }
        }

        public static ItemCodeMappingDetail Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ItemCodeMappingDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ItemCodeMappingDetails.Add(this);
                db.SaveChanges();

                var trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(Id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(entity, this);

                entity.ItemCodeMappingId = ItemCodeMappingId;
                entity.TreatyType = TreatyType;
                entity.TreatyCode = TreatyCode;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                var entity = db.ItemCodeMappingDetails.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.ItemCodeMappingDetails.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByItemCodeMappingId(int itemCodeMappingId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ItemCodeMappingDetails.Where(q => q.ItemCodeMappingId == itemCodeMappingId);

                var trails = new List<DataTrail>();
                foreach (ItemCodeMappingDetail itemCodeMappingDetail in query.ToList())
                {
                    var trail = new DataTrail(itemCodeMappingDetail, true);
                    trails.Add(trail);

                    db.Entry(itemCodeMappingDetail).State = EntityState.Deleted;
                    db.ItemCodeMappingDetails.Remove(itemCodeMappingDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
