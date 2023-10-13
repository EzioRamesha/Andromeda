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

namespace DataAccess.Entities.TreatyPricing
{
    [Table("TreatyPricingFinancialTableUploads")]
    public class TreatyPricingFinancialTableUpload
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingFinancialTableVersionDetailId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingFinancialTableVersionDetail TreatyPricingFinancialTableVersionDetail { get; set; }

        [Index]
        public int MinimumSumAssured { get; set; }

        [Index]
        public int MaximumSumAssured { get; set; }

        [MaxLength(255), Index]
        public string Code { get; set; }

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

        public TreatyPricingFinancialTableUpload()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingFinancialTableUploads.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingFinancialTableUpload Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingFinancialTableUploads.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingFinancialTableUploads.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
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

                DataTrail trail = new DataTrail(entity, this);

                entity.TreatyPricingFinancialTableVersionDetailId = TreatyPricingFinancialTableVersionDetailId;
                entity.MinimumSumAssured = MinimumSumAssured;
                entity.MaximumSumAssured = MaximumSumAssured;
                entity.Code = Code;
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

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.TreatyPricingFinancialTableUploads.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingFinancialTableVersionDetailId(int treatyPricingFinancialTableVersionDetailId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingFinancialTableUploads.Where(q => q.TreatyPricingFinancialTableVersionDetailId == treatyPricingFinancialTableVersionDetailId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (TreatyPricingFinancialTableUpload entity in query.ToList())
                {
                    DataTrail trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.TreatyPricingFinancialTableUploads.Remove(entity);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
