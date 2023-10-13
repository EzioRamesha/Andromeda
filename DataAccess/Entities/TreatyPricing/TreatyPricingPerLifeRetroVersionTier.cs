using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Shared;
using Shared.Trails;
using Shared.Trails.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataAccess.Entities.TreatyPricing
{
    [Table("TreatyPricingPerLifeRetroVersionTiers")]
    public class TreatyPricingPerLifeRetroVersionTier
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingPerLifeRetroVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingPerLifeRetroVersion TreatyPricingPerLifeRetroVersion { get; set; }

        [MaxLength(255)]
        public string Col1 { get; set; }

        [MaxLength(255)]
        public string Col2 { get; set; }

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

        public TreatyPricingPerLifeRetroVersionTier()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingPerLifeRetroVersionTiers.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingPerLifeRetroVersionTier Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingPerLifeRetroVersionTiers.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingPerLifeRetroVersionTiers.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                TreatyPricingPerLifeRetroVersionTier treatyPricingPerLifeRetroVersionTier = Find(Id);
                if (treatyPricingPerLifeRetroVersionTier == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyPricingPerLifeRetroVersionTier, this);

                treatyPricingPerLifeRetroVersionTier.TreatyPricingPerLifeRetroVersionId = TreatyPricingPerLifeRetroVersionId;
                treatyPricingPerLifeRetroVersionTier.Col1 = Col1;
                treatyPricingPerLifeRetroVersionTier.Col2 = Col2;
                treatyPricingPerLifeRetroVersionTier.UpdatedAt = DateTime.Now;
                treatyPricingPerLifeRetroVersionTier.UpdatedById = UpdatedById ?? treatyPricingPerLifeRetroVersionTier.UpdatedById;

                db.Entry(treatyPricingPerLifeRetroVersionTier).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                TreatyPricingPerLifeRetroVersionTier treatyPricingPerLifeRetroVersionTier = Find(id);
                if (treatyPricingPerLifeRetroVersionTier == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyPricingPerLifeRetroVersionTier, true);

                db.Entry(treatyPricingPerLifeRetroVersionTier).State = EntityState.Deleted;
                db.TreatyPricingPerLifeRetroVersionTiers.Remove(treatyPricingPerLifeRetroVersionTier);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
