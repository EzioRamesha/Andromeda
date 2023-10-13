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
    [Table("TreatyPricingFinancialTableVersionFiles")]
    public class TreatyPricingFinancialTableVersionFile
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingFinancialTableVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingFinancialTableVersion TreatyPricingFinancialTableVersion { get; set; }

        [Required, Index]
        public int DistributionTierPickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail DistributionTierPickListDetail { get; set; }

        [MaxLength(255), Index]
        public string Description { get; set; }

        [MaxLength(255), Index]
        public string FileName { get; set; }

        [MaxLength(255), Index]
        public string HashFileName { get; set; }

        [Required, Index]
        public int Status { get; set; }

        [Column(TypeName = "ntext")]
        public string Errors { get; set; }

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

        public TreatyPricingFinancialTableVersionFile()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingFinancialTableVersionFiles.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingFinancialTableVersionFile Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingFinancialTableVersionFiles.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingFinancialTableVersionFiles.Add(this);
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

                entity.TreatyPricingFinancialTableVersionId = TreatyPricingFinancialTableVersionId;
                entity.DistributionTierPickListDetailId = DistributionTierPickListDetailId;
                entity.Description = Description;
                entity.FileName = FileName;
                entity.HashFileName = HashFileName;
                entity.Status = Status;
                entity.Errors = Errors;
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
                db.TreatyPricingFinancialTableVersionFiles.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingFinancialTableVersionId(int treatyPricingFinancialTableVersionId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingFinancialTableVersionFiles.Where(q => q.TreatyPricingFinancialTableVersionId == treatyPricingFinancialTableVersionId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (TreatyPricingFinancialTableVersionFile entity in query.ToList())
                {
                    DataTrail trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.TreatyPricingFinancialTableVersionFiles.Remove(entity);
                }

                db.SaveChanges();
                return trails;
            }
        }

        public string GetDistributionTier(int distributionTierPickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.PickListDetails.Where(q => q.Id == distributionTierPickListDetailId).FirstOrDefault().Description;
            }
        }
    }
}
