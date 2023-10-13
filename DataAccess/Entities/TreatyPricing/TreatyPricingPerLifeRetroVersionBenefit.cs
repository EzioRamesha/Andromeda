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
    [Table("TreatyPricingPerLifeRetroVersionBenefits")]
    public class TreatyPricingPerLifeRetroVersionBenefit
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingPerLifeRetroVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingPerLifeRetroVersion TreatyPricingPerLifeRetroVersion { get; set; }

        [Required, Index]
        public int BenefitId { get; set; }
        [ExcludeTrail]
        public virtual Benefit Benefit { get; set; }

        public int? ArrangementRetrocessionnaireTypePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail ArrangementRetrocessionnaireTypePickListDetail { get; set; }

        public string TotalMortality { get; set; }
        
        public string MlreRetention { get; set; }

        [MaxLength(128)]
        public string RetrocessionnaireShare { get; set; }

        public int? AgeBasisPickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail AgeBasisPickListDetail { get; set; }
        
        public int? MinIssueAge { get; set; }
        
        public int? MaxIssueAge { get; set; }

        [MaxLength(128)]
        public string MaxExpiryAge { get; set; }

        public string RetrocessionaireDiscount { get; set; }

        public string RateTablePercentage { get; set; }
        
        public string ClaimApprovalLimit { get; set; }

        public string AutoBindingLimit { get; set; }

        [Index]
        public bool IsProfitCommission { get; set; }

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

        public TreatyPricingPerLifeRetroVersionBenefit()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingPerLifeRetroVersionBenefits.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingPerLifeRetroVersionBenefit Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingPerLifeRetroVersionBenefits.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingPerLifeRetroVersionBenefits.Add(this);
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

                entity.TreatyPricingPerLifeRetroVersionId = TreatyPricingPerLifeRetroVersionId;
                entity.BenefitId = BenefitId;
                entity.ArrangementRetrocessionnaireTypePickListDetailId = ArrangementRetrocessionnaireTypePickListDetailId;
                entity.TotalMortality = TotalMortality;
                entity.MlreRetention = MlreRetention;
                entity.RetrocessionnaireShare = RetrocessionnaireShare;
                entity.AgeBasisPickListDetailId = AgeBasisPickListDetailId;
                entity.MinIssueAge = MinIssueAge;
                entity.MaxIssueAge = MaxIssueAge;
                entity.MaxExpiryAge = MaxExpiryAge;
                entity.RetrocessionaireDiscount = RetrocessionaireDiscount;
                entity.RateTablePercentage = RateTablePercentage;
                entity.ClaimApprovalLimit = ClaimApprovalLimit;
                entity.AutoBindingLimit = AutoBindingLimit;
                entity.IsProfitCommission = IsProfitCommission;
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
                db.TreatyPricingPerLifeRetroVersionBenefits.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
