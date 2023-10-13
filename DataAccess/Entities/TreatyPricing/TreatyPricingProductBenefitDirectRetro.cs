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
    [Table("TreatyPricingProductBenefitDirectRetros")]
    public class TreatyPricingProductBenefitDirectRetro
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingProductBenefitId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingProductBenefit TreatyPricingProductBenefit { get; set; }

        [Required, Index]
        public int RetroPartyId { get; set; }
        [ExcludeTrail]
        public virtual RetroParty RetroParty { get; set; }

        [Index]
        public int? ArrangementRetrocessionTypePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail ArrangementRetrocessionTypePickListDetail { get; set; }

        [MaxLength(512), Index]
        public string MlreRetention { get; set; }

        [MaxLength(512), Index]
        public string RetrocessionShare { get; set; }

        [Index]
        public bool IsRetrocessionProfitCommission { get; set; }

        [Index]
        public bool IsRetrocessionAdvantageProgram { get; set; }

        [MaxLength(256), Index]
        public string RetrocessionRateTable { get; set; }

        [MaxLength(256), Index]
        public string NewBusinessRateGuarantee { get; set; }

        [MaxLength(256), Index]
        public string RenewalBusinessRateGuarantee { get; set; }

        [MaxLength(128), Index]
        public string RetrocessionDiscount { get; set; }

        [MaxLength(256), Index]
        public string AdditionalDiscount { get; set; }

        [MaxLength(128), Index]
        public string AdditionalLoading { get; set; }

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

        public TreatyPricingProductBenefitDirectRetro()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductBenefitDirectRetros.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingProductBenefitDirectRetro Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductBenefitDirectRetros.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingProductBenefitDirectRetros.Add(this);
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

                entity.TreatyPricingProductBenefitId = TreatyPricingProductBenefitId;
                entity.RetroPartyId = RetroPartyId;
                entity.ArrangementRetrocessionTypePickListDetailId = ArrangementRetrocessionTypePickListDetailId;
                entity.MlreRetention = MlreRetention;
                entity.MlreRetention = MlreRetention;
                entity.RetrocessionShare = RetrocessionShare;
                entity.IsRetrocessionProfitCommission = IsRetrocessionProfitCommission;
                entity.IsRetrocessionAdvantageProgram = IsRetrocessionAdvantageProgram;
                entity.RetrocessionRateTable = RetrocessionRateTable;
                entity.NewBusinessRateGuarantee = NewBusinessRateGuarantee;
                entity.RenewalBusinessRateGuarantee = RenewalBusinessRateGuarantee;
                entity.RetrocessionDiscount = RetrocessionDiscount;
                entity.AdditionalDiscount = AdditionalDiscount;
                entity.AdditionalLoading = AdditionalLoading;
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
                var entity = db.TreatyPricingProductBenefitDirectRetros.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.TreatyPricingProductBenefitDirectRetros.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
