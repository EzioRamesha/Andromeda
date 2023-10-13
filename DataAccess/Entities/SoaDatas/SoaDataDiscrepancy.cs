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

namespace DataAccess.Entities.SoaDatas
{
    [Table("SoaDataDiscrepancies")]
    public class SoaDataDiscrepancy
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int SoaDataBatchId { get; set; }
        [ExcludeTrail]
        public virtual SoaDataBatch SoaDataBatch { get; set; }

        [Required, Index]
        public int Type { get; set; }

        [MaxLength(35), Index]
        public string TreatyCode { get; set; }
        [MaxLength(35), Index]
        public string CedingPlanCode { get; set; }

        public double? CedantAmount { get; set; }
        public double? MlreChecking { get; set; }
        public double? Discrepancy { get; set; }

        [MaxLength(3), Index]
        public string CurrencyCode { get; set; }
        public double? CurrencyRate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }
        [Required]
        public int CreatedById { get; set; }
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }


        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedById { get; set; }
        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }

        public SoaDataDiscrepancy()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.SoaDataDiscrepancies.Any(q => q.Id == id);
            }
        }

        public static SoaDataDiscrepancy Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.SoaDataDiscrepancies.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<SoaDataDiscrepancy> GetBySoaDataBatchIdType(int soaDataBatchId, int type)
        {
            using (var db = new AppDbContext())
            {
                return db.SoaDataDiscrepancies.Where(q => q.SoaDataBatchId == soaDataBatchId && q.Type == type).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.SoaDataDiscrepancies.Add(this);
                db.SaveChanges();

                return new DataTrail(this);
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

                entity.SoaDataBatchId = SoaDataBatchId;
                entity.Type = Type;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = UpdatedById ?? entity.UpdatedById;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.SoaDataDiscrepancies.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
