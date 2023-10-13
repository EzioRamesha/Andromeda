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
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities.TreatyPricing
{
    [Table("TreatyPricingRateTableOriginalRates")]
    public class TreatyPricingRateTableOriginalRate
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingRateTableVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingRateTableVersion TreatyPricingRateTableVersion { get; set; }

        [Required, Index]
        public int Age { get; set; }

        [Index]
        public double? MaleNonSmoker { get; set; }

        [Index]
        public double? MaleSmoker { get; set; }

        [Index]
        public double? FemaleNonSmoker { get; set; }

        [Index]
        public double? FemaleSmoker { get; set; }

        [Index]
        public double? Male { get; set; }

        [Index]
        public double? Female { get; set; }

        [Index]
        public double? Unisex { get; set; }

        [Index]
        public double? UnitRate { get; set; }

        [Index]
        public double? OccupationClass { get; set; }

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

        public TreatyPricingRateTableOriginalRate()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingRateTableOriginalRates.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingRateTableOriginalRate Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingRateTableOriginalRates.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingRateTableOriginalRates.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                TreatyPricingRateTableOriginalRate treatyPricingRateTableOriginalRate = Find(Id);
                if (treatyPricingRateTableOriginalRate == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyPricingRateTableOriginalRate, this);

                treatyPricingRateTableOriginalRate.TreatyPricingRateTableVersionId = TreatyPricingRateTableVersionId;
                treatyPricingRateTableOriginalRate.MaleNonSmoker = MaleNonSmoker;
                treatyPricingRateTableOriginalRate.MaleSmoker = MaleSmoker;
                treatyPricingRateTableOriginalRate.FemaleNonSmoker = FemaleNonSmoker;
                treatyPricingRateTableOriginalRate.FemaleSmoker = FemaleSmoker;
                treatyPricingRateTableOriginalRate.Male = Male;
                treatyPricingRateTableOriginalRate.Female = Female;
                treatyPricingRateTableOriginalRate.Unisex = Unisex;
                treatyPricingRateTableOriginalRate.UnitRate = UnitRate;
                treatyPricingRateTableOriginalRate.OccupationClass = OccupationClass;
                treatyPricingRateTableOriginalRate.UpdatedAt = DateTime.Now;
                treatyPricingRateTableOriginalRate.UpdatedById = UpdatedById ?? treatyPricingRateTableOriginalRate.UpdatedById;

                db.Entry(treatyPricingRateTableOriginalRate).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                TreatyPricingRateTableOriginalRate treatyPricingRateTableOriginalRate = db.TreatyPricingRateTableOriginalRates.Where(q => q.Id == id).FirstOrDefault();
                if (treatyPricingRateTableOriginalRate == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyPricingRateTableOriginalRate, true);

                db.Entry(treatyPricingRateTableOriginalRate).State = EntityState.Deleted;
                db.TreatyPricingRateTableOriginalRates.Remove(treatyPricingRateTableOriginalRate);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
