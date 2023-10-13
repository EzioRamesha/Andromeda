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
    [Table("TreatyPricingGroupReferralVersionBenefits")]
    public class TreatyPricingGroupReferralVersionBenefit
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingGroupReferralVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingGroupReferralVersion TreatyPricingGroupReferralVersion { get; set; }

        [Required, Index]
        public int BenefitId { get; set; }
        [ExcludeTrail]
        public virtual Benefit Benefit { get; set; }

        [Index]
        public int? ReinsuranceArrangementPickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail ReinsuranceArrangementPickListDetail { get; set; }

        [Index]
        public int? AgeBasisPickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail AgeBasisPickListDetail { get; set; }

        public string MinimumEntryAge { get; set; }

        public string MaximumEntryAge { get; set; }

        public string MaximumExpiryAge { get; set; }

        [Index]
        public int? TreatyPricingUwLimitId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingUwLimit TreatyPricingUwLimit { get; set; }
        [Index]
        public int? TreatyPricingUwLimitVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingUwLimitVersion TreatyPricingUwLimitVersion { get; set; }

        [Column(TypeName = "ntext")]
        public string GroupFreeCoverLimitNonCI { get; set; } 

        [MaxLength(128)]
        public string RequestedFreeCoverLimitNonCI { get; set; }

        [Column(TypeName = "ntext")]
        public string GroupFreeCoverLimitCI { get; set; }

        [MaxLength(128)]
        public string RequestedFreeCoverLimitCI { get; set; }

        [Index]
        [MaxLength(128)]
        public string GroupFreeCoverLimitAgeNonCI { get; set; }

        [Index]
        [MaxLength(128)]
        public string GroupFreeCoverLimitAgeCI { get; set; }

        [Index]
        public int? OtherSpecialReinsuranceArrangement { get; set; }

        [MaxLength(255)]
        public string OtherSpecialTerms { get; set; }

        public string ProfitMargin { get; set; }

        public string ExpenseMargin { get; set; }

        public string CommissionMargin { get; set; }

        public string ProfitCommissionLoading { get; set; }

        public string AdditionalLoading { get; set; }

        public string CoinsuranceRiDiscount { get; set; }

        public string CoinsuranceCedantRetention { get; set; }

        public string CoinsuranceReinsurerShare { get; set; }

        public bool HasProfitCommission { get; set; }

        public bool HasGroupProfitCommission { get; set; }

        public bool IsOverwriteGroupProfitCommission { get; set; }
        public string OverwriteGroupProfitCommissionRemarks { get; set; }

        [Column(TypeName = "ntext")]
        public string GroupProfitCommission { get; set; }

        public string AdditionalLoadingYRTLayer { get; set; }

        public string RiDiscount { get; set; }

        public string CedantRetention { get; set; }

        [MaxLength(128)]
        public string ReinsuranceShare { get; set; }

        [MaxLength(128)]
        public string TabarruLoading { get; set; }

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

        public TreatyPricingGroupReferralVersionBenefit()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupReferralVersionBenefits.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingGroupReferralVersionBenefit Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupReferralVersionBenefits.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingGroupReferralVersionBenefits.Add(this);
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

                entity.TreatyPricingGroupReferralVersionId = TreatyPricingGroupReferralVersionId;
                entity.BenefitId = BenefitId;
                entity.ReinsuranceArrangementPickListDetailId = ReinsuranceArrangementPickListDetailId;
                entity.AgeBasisPickListDetailId = AgeBasisPickListDetailId;
                entity.MinimumEntryAge = MinimumEntryAge;
                entity.MaximumEntryAge = MaximumEntryAge;
                entity.MaximumExpiryAge = MaximumExpiryAge;
                entity.TreatyPricingUwLimitId = TreatyPricingUwLimitId;
                entity.TreatyPricingUwLimitVersionId = TreatyPricingUwLimitVersionId;
                entity.GroupFreeCoverLimitNonCI = GroupFreeCoverLimitNonCI;
                entity.RequestedFreeCoverLimitNonCI = RequestedFreeCoverLimitNonCI;
                entity.GroupFreeCoverLimitCI = GroupFreeCoverLimitCI;
                entity.RequestedFreeCoverLimitCI = RequestedFreeCoverLimitCI;
                entity.GroupFreeCoverLimitAgeNonCI = GroupFreeCoverLimitAgeNonCI;
                entity.GroupFreeCoverLimitAgeCI = GroupFreeCoverLimitAgeCI;
                entity.OtherSpecialReinsuranceArrangement = OtherSpecialReinsuranceArrangement;
                entity.OtherSpecialTerms = OtherSpecialTerms;
                entity.ProfitMargin = ProfitMargin;
                entity.ExpenseMargin = ExpenseMargin;
                entity.CommissionMargin = CommissionMargin;
                entity.ProfitCommissionLoading = ProfitCommissionLoading;
                entity.AdditionalLoading = AdditionalLoading;
                entity.CoinsuranceRiDiscount = CoinsuranceRiDiscount;
                entity.CoinsuranceCedantRetention = CoinsuranceCedantRetention;
                entity.CoinsuranceReinsurerShare = CoinsuranceReinsurerShare;
                entity.HasProfitCommission = HasProfitCommission;
                entity.HasGroupProfitCommission = HasGroupProfitCommission;
                entity.IsOverwriteGroupProfitCommission = IsOverwriteGroupProfitCommission;
                entity.OverwriteGroupProfitCommissionRemarks = OverwriteGroupProfitCommissionRemarks;
                entity.GroupProfitCommission = GroupProfitCommission;
                entity.AdditionalLoadingYRTLayer = AdditionalLoadingYRTLayer;
                entity.RiDiscount = RiDiscount;
                entity.CedantRetention = CedantRetention;
                entity.ReinsuranceShare = ReinsuranceShare;
                entity.TabarruLoading = TabarruLoading;
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
                db.TreatyPricingGroupReferralVersionBenefits.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
