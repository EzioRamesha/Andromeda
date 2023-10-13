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
    [Table("TreatyPricingProductBenefits")]
    public class TreatyPricingProductBenefit
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingProductVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingProductVersion TreatyPricingProductVersion { get; set; }

        [Required, Index]
        public int BenefitId { get; set; }
        [ExcludeTrail]
        public virtual Benefit Benefit { get; set; }

        [MaxLength(255), Index]
        public string Name { get; set; }

        [Index]
        public int? BasicRiderPickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail BasicRiderPickListDetail { get; set; }

        // General Tab

        [Index]
        public int? PayoutTypePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail PayoutTypePickListDetail { get; set; }

        [MaxLength(128), Index]
        public string RiderAttachmentRatio { get; set; }

        [Index]
        public int? AgeBasisPickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail AgeBasisPickListDetail { get; set; }

        [MaxLength(128), Index]
        public string MinimumEntryAge { get; set; }

        [MaxLength(128), Index]
        public string MaximumEntryAge { get; set; }

        [MaxLength(128), Index]
        public string MaximumExpiryAge { get; set; }

        [MaxLength(128), Index]
        public string MinimumPolicyTerm { get; set; }

        [MaxLength(128), Index]
        public string MaximumPolicyTerm { get; set; }

        [MaxLength(128), Index]
        public string PremiumPayingTerm { get; set; }

        [MaxLength(256), Index]
        public string MinimumSumAssured { get; set; }

        [MaxLength(256), Index]
        public string MaximumSumAssured { get; set; }

        [Index]
        public int? TreatyPricingUwLimitId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingUwLimit TreatyPricingUwLimit { get; set; }

        [Index]
        public int? TreatyPricingUwLimitVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingUwLimitVersion TreatyPricingUwLimitVersion { get; set; }

        [MaxLength(128), Index]
        public string Others { get; set; }

        [Index]
        public int? TreatyPricingClaimApprovalLimitId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingClaimApprovalLimit TreatyPricingClaimApprovalLimit { get; set; }

        [Index]
        public int? TreatyPricingClaimApprovalLimitVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingClaimApprovalLimitVersion TreatyPricingClaimApprovalLimitVersion { get; set; }

        [MaxLength(128), Index]
        public string IfOthers1 { get; set; }

        [Index]
        public int? TreatyPricingDefinitionAndExclusionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingDefinitionAndExclusion TreatyPricingDefinitionAndExclusion { get; set; }

        [Index]
        public int? TreatyPricingDefinitionAndExclusionVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingDefinitionAndExclusionVersion TreatyPricingDefinitionAndExclusionVersion { get; set; }

        [MaxLength(128), Index]
        public string IfOthers2 { get; set; }

        [MaxLength(128), Index]
        public string RefundOfPremium { get; set; }

        [MaxLength(128), Index]
        public string PreExistingCondition { get; set; }

        // Pricing Tab

        [Index]
        public int? PricingArrangementReinsuranceTypePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail PricingArrangementReinsuranceTypePickListDetail { get; set; }

        [MaxLength(256), Index]
        public string BenefitPayout { get; set; }

        [MaxLength(256), Index]
        public string CedantRetention { get; set; }

        [MaxLength(256), Index]
        public string ReinsuranceShare { get; set; }

        [MaxLength(128), Index]
        public string CoinsuranceCedantRetention { get; set; }

        [MaxLength(128), Index]
        public string CoinsuranceReinsuranceShare { get; set; }

        [MaxLength(128), Index]
        public string RequestedCoinsuranceRiDiscount { get; set; }

        [MaxLength(128), Index]
        public string ProfitMargin { get; set; }

        [MaxLength(128), Index]
        public string ExpenseMargin { get; set; }

        [MaxLength(128), Index]
        public string CommissionMargin { get; set; }

        [MaxLength(128), Index]
        public string GroupProfitCommissionLoading { get; set; }

        [MaxLength(128), Index]
        public string TabarruLoading { get; set; }

        [Index]
        public int? RiskPatternSumPickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail RiskPatternSumPickListDetail { get; set; }

        [MaxLength(255), Index]
        public string PricingUploadFileName { get; set; }

        [MaxLength(255), Index]
        public string PricingUploadHashFileName { get; set; }

        [Index]
        public bool IsProfitCommission { get; set; }

        [Index]
        public bool IsAdvantageProgram { get; set; }

        [Index]
        public int? TreatyPricingRateTableId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingRateTable TreatyPricingRateTable { get; set; }

        [Index]
        public int? TreatyPricingRateTableVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingRateTableVersion TreatyPricingRateTableVersion { get; set; }

        [MaxLength(128), Index]
        public string RequestedRateGuarantee { get; set; }

        [MaxLength(128), Index]
        public string RequestedReinsuranceDiscount { get; set; }

        // Direct Retro Tab


        // Inward Retro Tab

        [MaxLength(128), Index]
        public string InwardRetroParty { get; set; }

        [Index]
        public int? InwardRetroArrangementReinsuranceTypePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail InwardRetroArrangementReinsuranceTypePickListDetail { get; set; }

        [MaxLength(128), Index]
        public string InwardRetroRetention { get; set; }

        [MaxLength(128), Index]
        public string MlreShare { get; set; }

        [Index]
        public bool IsRetrocessionProfitCommission { get; set; }

        [Index]
        public bool IsRetrocessionAdvantageProgram { get; set; }

        [MaxLength(128), Index]
        public string RetrocessionRateTable { get; set; }

        [MaxLength(128), Index]
        public string NewBusinessRateGuarantee { get; set; }

        [MaxLength(128), Index]
        public string RenewalBusinessRateGuarantee { get; set; }

        [MaxLength(128), Index]
        public string RetrocessionDiscount { get; set; }

        [MaxLength(128), Index]
        public string AdditionalDiscount { get; set; }

        [MaxLength(128), Index]
        public string AdditionalLoading { get; set; }

        // Retakaful Service Tab

        [MaxLength(128), Index]
        public string WakalahFee { get; set; }

        [MaxLength(128), Index]
        public string MlreServiceFee { get; set; }

        // Others

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

        public TreatyPricingProductBenefit()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductBenefits.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingProductBenefit Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductBenefits.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingProductBenefits.Add(this);
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

                entity.TreatyPricingProductVersionId = TreatyPricingProductVersionId;
                entity.BenefitId = BenefitId;
                entity.Name = Name;
                entity.BasicRiderPickListDetailId = BasicRiderPickListDetailId;
                // General Tab
                entity.PayoutTypePickListDetailId = PayoutTypePickListDetailId;
                entity.RiderAttachmentRatio = RiderAttachmentRatio;
                entity.AgeBasisPickListDetailId = AgeBasisPickListDetailId;
                entity.MinimumEntryAge = MinimumEntryAge;
                entity.MaximumEntryAge = MaximumEntryAge;
                entity.MaximumExpiryAge = MaximumExpiryAge;
                entity.MinimumPolicyTerm = MinimumPolicyTerm;
                entity.MaximumPolicyTerm = MaximumPolicyTerm;
                entity.PremiumPayingTerm = PremiumPayingTerm;
                entity.MinimumSumAssured = MinimumSumAssured;
                entity.MaximumSumAssured = MaximumSumAssured;
                entity.TreatyPricingUwLimitId = TreatyPricingUwLimitId;
                entity.TreatyPricingUwLimitVersionId = TreatyPricingUwLimitVersionId;
                entity.Others = Others;
                entity.TreatyPricingClaimApprovalLimitId = TreatyPricingClaimApprovalLimitId;
                entity.TreatyPricingClaimApprovalLimitVersionId = TreatyPricingClaimApprovalLimitVersionId;
                entity.IfOthers1 = IfOthers1;
                entity.TreatyPricingDefinitionAndExclusionId = TreatyPricingDefinitionAndExclusionId;
                entity.TreatyPricingDefinitionAndExclusionVersionId = TreatyPricingDefinitionAndExclusionVersionId;
                entity.IfOthers2 = IfOthers2;
                entity.RefundOfPremium = RefundOfPremium;
                entity.PreExistingCondition = PreExistingCondition;
                // Pricing Tab
                entity.PricingArrangementReinsuranceTypePickListDetailId = PricingArrangementReinsuranceTypePickListDetailId;
                entity.BenefitPayout = BenefitPayout;
                entity.CedantRetention = CedantRetention;
                entity.ReinsuranceShare = ReinsuranceShare;
                entity.CoinsuranceCedantRetention = CoinsuranceCedantRetention;
                entity.CoinsuranceReinsuranceShare = CoinsuranceReinsuranceShare;
                entity.RequestedCoinsuranceRiDiscount = RequestedCoinsuranceRiDiscount;
                entity.ProfitMargin = ProfitMargin;
                entity.ExpenseMargin = ExpenseMargin;
                entity.CommissionMargin = CommissionMargin;
                entity.GroupProfitCommissionLoading = GroupProfitCommissionLoading;
                entity.TabarruLoading = TabarruLoading;
                entity.RiskPatternSumPickListDetailId = RiskPatternSumPickListDetailId;
                entity.PricingUploadFileName = PricingUploadFileName;
                entity.PricingUploadHashFileName = PricingUploadHashFileName;
                entity.IsProfitCommission = IsProfitCommission;
                entity.IsAdvantageProgram = IsAdvantageProgram;
                entity.TreatyPricingRateTableId = TreatyPricingRateTableId;
                entity.TreatyPricingRateTableVersionId = TreatyPricingRateTableVersionId;
                entity.RequestedRateGuarantee = RequestedRateGuarantee;
                entity.RequestedReinsuranceDiscount = RequestedReinsuranceDiscount;
                // Direct Retro Tab
                // Inward Retro Tab
                entity.InwardRetroParty = InwardRetroParty;
                entity.InwardRetroArrangementReinsuranceTypePickListDetailId = InwardRetroArrangementReinsuranceTypePickListDetailId;
                entity.InwardRetroRetention = InwardRetroRetention;
                entity.MlreShare = MlreShare;
                entity.IsRetrocessionProfitCommission = IsRetrocessionProfitCommission;
                entity.IsRetrocessionAdvantageProgram = IsRetrocessionAdvantageProgram;
                entity.RetrocessionRateTable = RetrocessionRateTable;
                entity.NewBusinessRateGuarantee = NewBusinessRateGuarantee;
                entity.RenewalBusinessRateGuarantee = RenewalBusinessRateGuarantee;
                entity.RetrocessionDiscount = RetrocessionDiscount;
                entity.AdditionalDiscount = AdditionalDiscount;
                entity.AdditionalLoading = AdditionalLoading;
                // Retakaful Service Tab
                entity.WakalahFee = WakalahFee;
                entity.MlreServiceFee = MlreServiceFee;
                // Others
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
                var entity = db.TreatyPricingProductBenefits.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.TreatyPricingProductBenefits.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
