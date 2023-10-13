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
    [Table("TreatyPricingTierProfitCommissions")]
    public class TreatyPricingTierProfitCommission
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingProfitCommissionVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingProfitCommissionVersion TreatyPricingProfitCommissionVersion { get; set; }

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

        public TreatyPricingTierProfitCommission()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTierProfitCommissions.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingTierProfitCommission Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTierProfitCommissions.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingTierProfitCommissions.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                TreatyPricingTierProfitCommission treatyPricingTierProfitCommission = Find(Id);
                if (treatyPricingTierProfitCommission == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyPricingTierProfitCommission, this);

                treatyPricingTierProfitCommission.TreatyPricingProfitCommissionVersionId = TreatyPricingProfitCommissionVersionId;
                treatyPricingTierProfitCommission.Col1 = Col1;
                treatyPricingTierProfitCommission.Col2 = Col2;
                treatyPricingTierProfitCommission.UpdatedAt = DateTime.Now;
                treatyPricingTierProfitCommission.UpdatedById = UpdatedById ?? treatyPricingTierProfitCommission.UpdatedById;

                db.Entry(treatyPricingTierProfitCommission).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                TreatyPricingTierProfitCommission treatyPricingTierProfitCommission = db.TreatyPricingTierProfitCommissions.Where(q => q.Id == id).FirstOrDefault();
                if (treatyPricingTierProfitCommission == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyPricingTierProfitCommission, true);

                db.Entry(treatyPricingTierProfitCommission).State = EntityState.Deleted;
                db.TreatyPricingTierProfitCommissions.Remove(treatyPricingTierProfitCommission);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
