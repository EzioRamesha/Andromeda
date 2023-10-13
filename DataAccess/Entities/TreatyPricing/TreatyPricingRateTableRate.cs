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
    [Table("TreatyPricingRateTableRates")]
    public class TreatyPricingRateTableRate
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

        public TreatyPricingRateTableRate()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingRateTableRates.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingRateTableRate Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingRateTableRates.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingRateTableRates.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                TreatyPricingRateTableRate treatyPricingRateTableRate = Find(Id);
                if (treatyPricingRateTableRate == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyPricingRateTableRate, this);

                treatyPricingRateTableRate.TreatyPricingRateTableVersionId = TreatyPricingRateTableVersionId;
                treatyPricingRateTableRate.MaleNonSmoker = MaleNonSmoker;
                treatyPricingRateTableRate.MaleSmoker = MaleSmoker;
                treatyPricingRateTableRate.FemaleNonSmoker = FemaleNonSmoker;
                treatyPricingRateTableRate.FemaleSmoker = FemaleSmoker;
                treatyPricingRateTableRate.Male = Male;
                treatyPricingRateTableRate.Female = Female;
                treatyPricingRateTableRate.Unisex = Unisex;
                treatyPricingRateTableRate.UnitRate = UnitRate;
                treatyPricingRateTableRate.OccupationClass = OccupationClass;
                treatyPricingRateTableRate.UpdatedAt = DateTime.Now;
                treatyPricingRateTableRate.UpdatedById = UpdatedById ?? treatyPricingRateTableRate.UpdatedById;

                db.Entry(treatyPricingRateTableRate).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                TreatyPricingRateTableRate treatyPricingRateTableRate = db.TreatyPricingRateTableRates.Where(q => q.Id == id).FirstOrDefault();
                if (treatyPricingRateTableRate == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyPricingRateTableRate, true);

                db.Entry(treatyPricingRateTableRate).State = EntityState.Deleted;
                db.TreatyPricingRateTableRates.Remove(treatyPricingRateTableRate);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
