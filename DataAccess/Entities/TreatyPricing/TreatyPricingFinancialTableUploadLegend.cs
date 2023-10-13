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
    [Table("TreatyPricingFinancialTableUploadLegends")]
    public class TreatyPricingFinancialTableUploadLegend
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingFinancialTableVersionDetailId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingFinancialTableVersionDetail TreatyPricingFinancialTableVersionDetail { get; set; }

        [MaxLength(30), Index]
        public string Code { get; set; }

        [MaxLength(5000)]
        public string Description { get; set; }

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

        public TreatyPricingFinancialTableUploadLegend()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingFinancialTableUploadLegends.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingFinancialTableUploadLegend Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingFinancialTableUploadLegends.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingFinancialTableUploadLegends.Add(this);
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
                entity.Code = Code;
                entity.Description = Description;
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
                db.TreatyPricingFinancialTableUploadLegends.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingFinancialTableVersionDetailId(int treatyPricingFinancialTableVersionDetailId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingFinancialTableUploadLegends.Where(q => q.TreatyPricingFinancialTableVersionDetailId == treatyPricingFinancialTableVersionDetailId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (TreatyPricingFinancialTableUploadLegend entity in query.ToList())
                {
                    DataTrail trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.TreatyPricingFinancialTableUploadLegends.Remove(entity);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
