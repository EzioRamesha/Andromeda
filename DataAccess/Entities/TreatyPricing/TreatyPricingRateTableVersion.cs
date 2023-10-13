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
    [Table("TreatyPricingRateTableVersions")]
    public class TreatyPricingRateTableVersion
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingRateTableId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingRateTable TreatyPricingRateTable { get; set; }

        [Required, Index]
        public int Version { get; set; }

        [Required, Index]
        public int PersonInChargeId { get; set; }
        [ExcludeTrail]
        public virtual User PersonInCharge { get; set; }

        [MaxLength(255)]
        public string BenefitName { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? EffectiveDate { get; set; }

        [Index]
        public int? AgeBasisPickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail AgeBasisPickListDetail { get; set; }

        [MaxLength(255)]
        public string RiDiscount { get; set; }

        [MaxLength(255)]
        public string CoinsuranceRiDiscount { get; set; }

        [Index]
        public int? RateGuaranteePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail RateGuaranteePickListDetail { get; set; }

        [MaxLength(255)]
        public string RateGuaranteeForNewBusiness { get; set; }

        [MaxLength(255)]
        public string RateGuaranteeForRenewalBusiness { get; set; }

        [MaxLength(255)]
        public string AdvantageProgram { get; set; }

        [MaxLength(255)]
        public string ProfitCommission { get; set; }

        [Column(TypeName = "ntext")]
        public string AdditionalRemark { get; set; }

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

        public TreatyPricingRateTableVersion()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingRateTableVersions.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingRateTableVersion Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingRateTableVersions.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingRateTableVersions.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                TreatyPricingRateTableVersion treatyPricingRateTableVersion = Find(Id);
                if (treatyPricingRateTableVersion == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyPricingRateTableVersion, this);

                treatyPricingRateTableVersion.TreatyPricingRateTableId = TreatyPricingRateTableId;
                treatyPricingRateTableVersion.Version = Version;
                treatyPricingRateTableVersion.PersonInChargeId = PersonInChargeId;
                treatyPricingRateTableVersion.BenefitName = BenefitName;
                treatyPricingRateTableVersion.EffectiveDate = EffectiveDate;
                treatyPricingRateTableVersion.AgeBasisPickListDetailId = AgeBasisPickListDetailId;
                treatyPricingRateTableVersion.RiDiscount = RiDiscount;
                treatyPricingRateTableVersion.CoinsuranceRiDiscount = CoinsuranceRiDiscount;
                treatyPricingRateTableVersion.RateGuaranteePickListDetailId = RateGuaranteePickListDetailId;
                treatyPricingRateTableVersion.RateGuaranteeForNewBusiness = RateGuaranteeForNewBusiness;
                treatyPricingRateTableVersion.RateGuaranteeForRenewalBusiness = RateGuaranteeForRenewalBusiness;
                treatyPricingRateTableVersion.AdvantageProgram = AdvantageProgram;
                treatyPricingRateTableVersion.ProfitCommission = ProfitCommission;
                treatyPricingRateTableVersion.AdditionalRemark = AdditionalRemark;
                treatyPricingRateTableVersion.UpdatedAt = DateTime.Now;
                treatyPricingRateTableVersion.UpdatedById = UpdatedById ?? treatyPricingRateTableVersion.UpdatedById;

                db.Entry(treatyPricingRateTableVersion).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                TreatyPricingRateTableVersion treatyPricingRateTableVersion = db.TreatyPricingRateTableVersions.Where(q => q.Id == id).FirstOrDefault();
                if (treatyPricingRateTableVersion == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyPricingRateTableVersion, true);

                db.Entry(treatyPricingRateTableVersion).State = EntityState.Deleted;
                db.TreatyPricingRateTableVersions.Remove(treatyPricingRateTableVersion);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
