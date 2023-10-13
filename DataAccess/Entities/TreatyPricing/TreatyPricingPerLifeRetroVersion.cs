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
    [Table("TreatyPricingPerLifeRetroVersions")]
    public class TreatyPricingPerLifeRetroVersion
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingPerLifeRetroId { get; set; }
        public TreatyPricingPerLifeRetro TreatyPricingPerLifeRetro { get; set; }

        [Required, Index]
        public int Version { get; set; }

        [Required]
        public int PersonInChargeId { get; set; }
        public User PersonInCharge { get; set; }

        [Index]
        public int? RetrocessionaireRetroPartyId { get; set; }
        [ExcludeTrail]
        public virtual RetroParty RetrocessionaireRetroParty { get; set; }

        [MaxLength(128)]
        public string RefundofUnearnedPremium { get; set; }

        [MaxLength(128)]
        public string TerminationPeriod { get; set; }

        [MaxLength(128)]
        public string ResidenceCountry { get; set; }

        public int? PaymentRetrocessionairePremiumPickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail PaymentRetrocessionairePremiumPickListDetail { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? EffectiveDate { get; set; }

        public int? JumboLimitCurrencyCodePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail JumboLimitCurrencyCodePickListDetail { get; set; }

        public double? JumboLimit { get; set; }

        [MaxLength(255)]
        public string Remarks { get; set; }

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

        public int? UpdatedById { get; set; }

        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }

        public TreatyPricingPerLifeRetroVersion()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingPerLifeRetroVersions.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingPerLifeRetroVersion Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingPerLifeRetroVersions.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingPerLifeRetroVersions.Add(this);
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

                entity.TreatyPricingPerLifeRetroId = TreatyPricingPerLifeRetroId;
                entity.Version = Version;
                entity.PersonInChargeId = PersonInChargeId;
                entity.RetrocessionaireRetroPartyId = RetrocessionaireRetroPartyId;
                entity.RefundofUnearnedPremium = RefundofUnearnedPremium;
                entity.TerminationPeriod = TerminationPeriod;
                entity.ResidenceCountry = ResidenceCountry;
                entity.PaymentRetrocessionairePremiumPickListDetailId = PaymentRetrocessionairePremiumPickListDetailId;
                entity.EffectiveDate = EffectiveDate;
                entity.JumboLimitCurrencyCodePickListDetailId = JumboLimitCurrencyCodePickListDetailId;
                entity.JumboLimit = JumboLimit;
                entity.Remarks = Remarks;
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
                var entity = db.TreatyPricingPerLifeRetroVersions.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.TreatyPricingPerLifeRetroVersions.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
