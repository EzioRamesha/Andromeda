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
    [Table("TreatyPricingProfitCommissionVersions")]
    public class TreatyPricingProfitCommissionVersion
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingProfitCommissionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingProfitCommission TreatyPricingProfitCommission { get; set; }

        [Required, Index]
        public int Version { get; set; }

        [Required, Index]
        public int PersonInChargeId { get; set; }
        [ExcludeTrail]
        public virtual User PersonInCharge { get; set; }

        public int? ProfitSharing { get; set; }

        [MaxLength(255)]
        public string ProfitDescription { get; set; }

        public double? NetProfitPercentage { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        public int? UpdatedById { get; set; }
        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }

        public TreatyPricingProfitCommissionVersion()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProfitCommissionVersions.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingProfitCommissionVersion Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProfitCommissionVersions.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingProfitCommissionVersions.Add(this);
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

                entity.TreatyPricingProfitCommissionId = TreatyPricingProfitCommissionId;
                entity.Version = Version;
                entity.PersonInChargeId = PersonInChargeId;
                entity.ProfitSharing = ProfitSharing;
                entity.ProfitDescription = ProfitDescription;
                entity.NetProfitPercentage = NetProfitPercentage;

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
                var entity = db.TreatyPricingProfitCommissionVersions.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.TreatyPricingProfitCommissionVersions.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
