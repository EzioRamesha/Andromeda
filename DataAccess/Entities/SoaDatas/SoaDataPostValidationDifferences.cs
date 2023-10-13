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
    [Table("SoaDataPostValidationDifferences")]
    public class SoaDataPostValidationDifference
    {
        [Key]
        public int Id { get; set; }


        [Required, Index]
        public int SoaDataBatchId { get; set; }
        [ExcludeTrail]
        public virtual SoaDataBatch SoaDataBatch { get; set; }


        [Required, Index]
        public int Type { get; set; }


        [MaxLength(12), Index]
        public string BusinessCode { get; set; }
        [MaxLength(35), Index]
        public string TreatyCode { get; set; }
        [MaxLength(32), Index]
        public string SoaQuarter { get; set; }
        [MaxLength(32), Index]
        public string RiskQuarter { get; set; }
        [Index]
        public int? RiskMonth { get; set; }


        public double? GrossPremium { get; set; } = 0;
        public double? DifferenceNetTotalAmount { get; set; } = 0;
        public double? DifferencePercetage { get; set; } = 0;


        [MaxLength(3), Index]
        public string CurrencyCode { get; set; }
        public double? CurrencyRate { get; set; }

        [MaxLength(128)]
        public string Remark { get; set; }
        [MaxLength(128)]
        public string Check { get; set; }


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


        public SoaDataPostValidationDifference()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
        
        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.SoaDataPostValidationDifferences.Any(q => q.Id == id);
            }
        }

        public static SoaDataPostValidationDifference Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.SoaDataPostValidationDifferences.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.SoaDataPostValidationDifferences.Add(this);
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

                entity.CurrencyCode = CurrencyCode;
                entity.CurrencyRate = CurrencyRate;

                entity.BusinessCode = BusinessCode;
                entity.TreatyCode = TreatyCode;
                entity.SoaQuarter = SoaQuarter;
                entity.RiskQuarter = RiskQuarter;
                entity.RiskMonth = RiskMonth;

                entity.GrossPremium = GrossPremium;
                entity.DifferenceNetTotalAmount = DifferenceNetTotalAmount;
                entity.DifferencePercetage = DifferencePercetage;

                entity.Remark = Remark;
                entity.Check = Check;

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
                db.SoaDataPostValidationDifferences.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
