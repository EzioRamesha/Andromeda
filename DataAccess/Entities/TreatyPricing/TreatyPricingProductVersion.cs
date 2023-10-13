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
    [Table("TreatyPricingProductVersions")]
    public class TreatyPricingProductVersion
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingProductId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingProduct TreatyPricingProduct { get; set; }

        [Required, Index]
        public int Version { get; set; }

        [Required, Index]
        public int PersonInChargeId { get; set; }
        [ExcludeTrail]
        public virtual User PersonInCharge { get; set; }

        [Index]
        public int? ProductTypePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail ProductTypePickListDetail { get; set; }

        [Index]
        public int? BusinessOriginPickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail BusinessOriginPickListDetail { get; set; }

        [Index]
        public int? BusinessTypePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail BusinessTypePickListDetail { get; set; }

        [Index]
        public int? ReinsuranceArrangementPickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail ReinsuranceArrangementPickListDetail { get; set; }

        [Index]
        [MaxLength(128)]
        public string ExpectedAverageSumAssured { get; set; }

        [Index]
        [MaxLength(128)]
        public string ExpectedRiPremium { get; set; }

        [Index]
        public int? TreatyPricingMedicalTableId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingMedicalTable TreatyPricingMedicalTable { get; set; }

        [Index]
        public int? TreatyPricingMedicalTableVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingMedicalTableVersion TreatyPricingMedicalTableVersion { get; set; }

        [Index]
        public int? TreatyPricingFinancialTableId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingFinancialTable TreatyPricingFinancialTable { get; set; }

        [Index]
        public int? TreatyPricingFinancialTableVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingFinancialTableVersion TreatyPricingFinancialTableVersion { get; set; }

        [Index]
        public int? TreatyPricingUwQuestionnaireId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingUwQuestionnaire TreatyPricingUwQuestionnaire { get; set; }

        [Index]
        public int? TreatyPricingUwQuestionnaireVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingUwQuestionnaireVersion TreatyPricingUwQuestionnaireVersion { get; set; }

        [Index]
        public int? TreatyPricingAdvantageProgramId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingAdvantageProgram TreatyPricingAdvantageProgram { get; set; }

        [Index]
        public int? TreatyPricingAdvantageProgramVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingAdvantageProgramVersion TreatyPricingAdvantageProgramVersion { get; set; }

        [Index]
        [MaxLength(128)]
        public string ExpectedPolicyNo { get; set; }

        [Index]
        [MaxLength(128)]
        public string JumboLimit { get; set; }

        [Column(TypeName = "ntext")]
        public string UnderwritingAdditionalRemark { get; set; }

        [Index]
        [MaxLength(256)]
        public string WaitingPeriod { get; set; }

        [Index]
        [MaxLength(128)]
        public string SurvivalPeriod { get; set; }

        [Index]
        public int? TreatyPricingProfitCommissionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingProfitCommission TreatyPricingProfitCommission { get; set; }

        [Index]
        public int? TreatyPricingProfitCommissionVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingProfitCommissionVersion TreatyPricingProfitCommissionVersion { get; set; }

        [Index]
        public int? ReinsurancePremiumPaymentPickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail ReinsurancePremiumPaymentPickListDetail { get; set; }

        [Index]
        public int? UnearnedPremiumRefundPickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail UnearnedPremiumRefundPickListDetail { get; set; }

        [Index]
        [MaxLength(128)]
        public string TerminationClause { get; set; }

        [Index]
        [MaxLength(256)]
        public string RecaptureClause { get; set; }

        //[Index]
        //[MaxLength(128)]
        //public string ResidenceCountry { get; set; }

        [Index]
        public int? TerritoryOfIssueCodePickListDetailId { get; set; }

        [ExcludeTrail]
        public virtual PickListDetail TerritoryOfIssueCodePickListDetail { get; set; }

        [Index]
        [MaxLength(128)]
        public string QuarterlyRiskPremium { get; set; }

        [Column(TypeName = "ntext")]
        public string GroupFreeCoverLimitNonCi { get; set; }

        [Index]
        [MaxLength(128)]
        public string GroupFreeCoverLimitAgeNonCi { get; set; }

        [Column(TypeName = "ntext")]
        public string GroupFreeCoverLimitCi { get; set; }

        [Index]
        [MaxLength(128)]
        public string GroupFreeCoverLimitAgeCi { get; set; }

        [Column(TypeName = "ntext")]
        public string GroupProfitCommission { get; set; }

        public string OccupationalClassification { get; set; }

        [Index]
        public bool IsDirectRetro { get; set; }

        [Index]
        [MaxLength(128)]
        public string DirectRetroProfitCommission { get; set; }

        [Index]
        [MaxLength(128)]
        public string DirectRetroTerminationClause { get; set; }

        [Index]
        [MaxLength(128)]
        public string DirectRetroRecaptureClause { get; set; }

        [Index]
        [MaxLength(128)]
        public string DirectRetroQuarterlyRiskPremium { get; set; }

        [Index]
        public bool IsInwardRetro { get; set; }

        [Index]
        [MaxLength(128)]
        public string InwardRetroProfitCommission { get; set; }

        [Index]
        [MaxLength(128)]
        public string InwardRetroTerminationClause { get; set; }

        [Index]
        [MaxLength(128)]
        public string InwardRetroRecaptureClause { get; set; }

        [Index]
        [MaxLength(128)]
        public string InwardRetroQuarterlyRiskPremium { get; set; }

        [Index]
        public bool IsRetakafulService { get; set; }

        [Index]
        [MaxLength(128)]
        public string InvestmentProfitSharing { get; set; }

        [Index]
        [MaxLength(128)]
        public string RetakafulModel { get; set; }

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

        public TreatyPricingProductVersion()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductVersions.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingProductVersion Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductVersions.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingProductVersions.Add(this);
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

                entity.TreatyPricingProductId = TreatyPricingProductId;
                entity.Version = Version;
                entity.PersonInChargeId = PersonInChargeId;
                entity.ProductTypePickListDetailId = ProductTypePickListDetailId;
                entity.BusinessOriginPickListDetailId = BusinessOriginPickListDetailId;
                entity.BusinessTypePickListDetailId = BusinessTypePickListDetailId;
                entity.ReinsuranceArrangementPickListDetailId = ReinsuranceArrangementPickListDetailId;
                entity.ExpectedAverageSumAssured = ExpectedAverageSumAssured;
                entity.ExpectedRiPremium = ExpectedRiPremium;
                entity.TreatyPricingMedicalTableId = TreatyPricingMedicalTableId;
                entity.TreatyPricingMedicalTableVersionId = TreatyPricingMedicalTableVersionId;
                entity.TreatyPricingFinancialTableId = TreatyPricingFinancialTableId;
                entity.TreatyPricingFinancialTableVersionId = TreatyPricingFinancialTableVersionId;
                entity.TreatyPricingUwQuestionnaireId = TreatyPricingUwQuestionnaireId;
                entity.TreatyPricingUwQuestionnaireVersionId = TreatyPricingUwQuestionnaireVersionId;
                entity.TreatyPricingAdvantageProgramId = TreatyPricingAdvantageProgramId;
                entity.TreatyPricingAdvantageProgramVersionId = TreatyPricingAdvantageProgramVersionId;
                entity.ExpectedPolicyNo = ExpectedPolicyNo;
                entity.JumboLimit = JumboLimit;
                entity.UnderwritingAdditionalRemark = UnderwritingAdditionalRemark;
                entity.WaitingPeriod = WaitingPeriod;
                entity.SurvivalPeriod = SurvivalPeriod;
                entity.TreatyPricingProfitCommissionId = TreatyPricingProfitCommissionId;
                entity.TreatyPricingProfitCommissionVersionId = TreatyPricingProfitCommissionVersionId;
                entity.ReinsurancePremiumPaymentPickListDetailId = ReinsurancePremiumPaymentPickListDetailId;
                entity.UnearnedPremiumRefundPickListDetailId = UnearnedPremiumRefundPickListDetailId;
                entity.TerminationClause = TerminationClause;
                entity.RecaptureClause = RecaptureClause;
                entity.TerritoryOfIssueCodePickListDetailId = TerritoryOfIssueCodePickListDetailId;
                entity.QuarterlyRiskPremium = QuarterlyRiskPremium;
                entity.GroupFreeCoverLimitNonCi = GroupFreeCoverLimitNonCi;
                entity.GroupFreeCoverLimitAgeNonCi = GroupFreeCoverLimitAgeNonCi;
                entity.GroupFreeCoverLimitCi = GroupFreeCoverLimitCi;
                entity.GroupFreeCoverLimitAgeCi = GroupFreeCoverLimitAgeCi;
                entity.GroupProfitCommission = GroupProfitCommission;
                entity.OccupationalClassification = OccupationalClassification;
                entity.IsDirectRetro = IsDirectRetro;
                entity.DirectRetroProfitCommission = DirectRetroProfitCommission;
                entity.DirectRetroTerminationClause = DirectRetroTerminationClause;
                entity.DirectRetroRecaptureClause = DirectRetroRecaptureClause;
                entity.DirectRetroQuarterlyRiskPremium = DirectRetroQuarterlyRiskPremium;
                entity.IsInwardRetro = IsInwardRetro;
                entity.InwardRetroProfitCommission = InwardRetroProfitCommission;
                entity.InwardRetroTerminationClause = InwardRetroTerminationClause;
                entity.InwardRetroRecaptureClause = InwardRetroRecaptureClause;
                entity.InwardRetroQuarterlyRiskPremium = InwardRetroQuarterlyRiskPremium;
                entity.IsRetakafulService = IsRetakafulService;
                entity.InvestmentProfitSharing = InvestmentProfitSharing;
                entity.RetakafulModel = RetakafulModel;
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
                var entity = db.TreatyPricingProductVersions.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.TreatyPricingProductVersions.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
