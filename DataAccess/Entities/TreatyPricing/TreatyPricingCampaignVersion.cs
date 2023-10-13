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
    [Table("TreatyPricingCampaignVersions")]
    public class TreatyPricingCampaignVersion
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingCampaignId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingCampaign TreatyPricingCampaign { get; set; }

        [Required, Index]
        public int Version { get; set; }

        [Required, Index]
        public int PersonInChargeId { get; set; }
        [ExcludeTrail]
        public virtual User PersonInCharge { get; set; }

        [Index]
        public bool IsPerBenefit { get; set; } = true;
        [MaxLength(128)]
        public string BenefitRemark { get; set; }

        public bool IsPerCedantRetention { get; set; } = true;
        [MaxLength(128)]
        public string CedantRetention { get; set; }

        public bool IsPerMlreShare { get; set; } = true;
        [MaxLength(128)]
        public string MlreShare { get; set; }

        public bool IsPerDistributionChannel { get; set; } = true;
        [Column(TypeName = "ntext")]
        public string DistributionChannel { get; set; }

        public bool IsPerAgeBasis { get; set; } = true;
        public int? AgeBasisPickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail AgeBasisPickListDetail { get; set; }

        public bool IsPerMinEntryAge { get; set; } = true;
        public string MinEntryAge { get; set; }

        public bool IsPerMaxEntryAge { get; set; } = true;
        public string MaxEntryAge { get; set; }

        public bool IsPerMaxExpiryAge { get; set; } = true;
        public string MaxExpiryAge { get; set; }

        public bool IsPerMinSumAssured { get; set; } = true;
        public string MinSumAssured { get; set; }

        public bool IsPerMaxSumAssured { get; set; } = true;
        public string MaxSumAssured { get; set; }

        public bool IsPerReinsuranceRate { get; set; } = true;
        [Index]
        public int? ReinsRateTreatyPricingRateTableId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingRateTable ReinsRateTreatyPricingRateTable { get; set; }
        [Index]
        public int? ReinsRateTreatyPricingRateTableVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingRateTableVersion ReinsRateTreatyPricingRateTableVersion { get; set; }
        [MaxLength(128)]
        public string ReinsRateNote { get; set; }

        public bool IsPerReinsuranceDiscount { get; set; } = true;
        [Index]
        public int? ReinsDiscountTreatyPricingRateTableId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingRateTable ReinsDiscountTreatyPricingRateTable { get; set; }
        [Index]
        public int? ReinsDiscountTreatyPricingRateTableVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingRateTableVersion ReinsDiscountTreatyPricingRateTableVersion { get; set; }
        [MaxLength(128)]
        public string ReinsDiscountNote { get; set; }

        public bool IsPerProfitComm { get; set; } = true;
        [Index]
        public int? TreatyPricingProfitCommissionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingProfitCommission TreatyPricingProfitCommission { get; set; }
        [Index]
        public int? TreatyPricingProfitCommissionVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingProfitCommissionVersion TreatyPricingProfitCommissionVersion { get; set; }
        [MaxLength(128)]
        public string ProfitCommNote { get; set; }

        [MaxLength(128)]
        public string CampaignFundByMlre { get; set; }

        [MaxLength(128)]
        public string ComplimentarySumAssured { get; set; }


        public bool IsPerUnderwritingMethod { get; set; } = true;
        [Column(TypeName = "ntext")]
        public string UnderwritingMethod { get; set; }

        public bool IsPerUnderwritingQuestion { get; set; } = true;
        [Index]
        public int? TreatyPricingUwQuestionnaireId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingUwQuestionnaire TreatyPricingUwQuestionnaire { get; set; }
        [Index]
        public int? TreatyPricingUwQuestionnaireVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingUwQuestionnaireVersion TreatyPricingUwQuestionnaireVersion { get; set; }
        [MaxLength(128)]
        public string UnderwritingQuestionNote { get; set; }

        public bool IsPerMedicalTable { get; set; } = true;
        [Index]
        public int? TreatyPricingMedicalTableId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingMedicalTable TreatyPricingMedicalTable { get; set; }
        [Index]
        public int? TreatyPricingMedicalTableVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingMedicalTableVersion TreatyPricingMedicalTableVersion { get; set; }
        [MaxLength(128)]
        public string MedicalTableNote { get; set; }

        public bool IsPerFinancialTable { get; set; } = true;
        [Index]
        public int? TreatyPricingFinancialTableId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingFinancialTable TreatyPricingFinancialTable { get; set; }
        [Index]
        public int? TreatyPricingFinancialTableVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingFinancialTableVersion TreatyPricingFinancialTableVersion { get; set; }
        [MaxLength(128)]
        public string FinancialTableNote { get; set; }

        public bool IsPerAggregationNotes { get; set; } = true;
        [MaxLength(128)]
        public string AggregationNotes { get; set; }

        public bool IsPerAdvantageProgram { get; set; } = true;
        [Index]
        public int? TreatyPricingAdvantageProgramId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingAdvantageProgram TreatyPricingAdvantageProgram { get; set; }
        [Index]
        public int? TreatyPricingAdvantageProgramVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingAdvantageProgramVersion TreatyPricingAdvantageProgramVersion { get; set; }
        [MaxLength(128)]
        public string AdvantageProgramNote { get; set; }


        public bool IsPerWaitingPeriod { get; set; } = true;
        [MaxLength(128)]
        public string WaitingPeriod { get; set; }

        public bool IsPerSurvivalPeriod { get; set; } = true;
        [MaxLength(128)]
        public string SurvivalPeriod { get; set; }

        [MaxLength(128)]
        public string OtherCampaignCriteria { get; set; }

        [MaxLength(128)]
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

        public TreatyPricingCampaignVersion()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingCampaignVersions.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingCampaignVersion Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingCampaignVersions.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingCampaignVersions.Add(this);
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

                entity.TreatyPricingCampaignId = TreatyPricingCampaignId;
                entity.Version = Version;
                entity.PersonInChargeId = PersonInChargeId;
                entity.IsPerBenefit = IsPerBenefit;
                entity.BenefitRemark = BenefitRemark;
                entity.IsPerCedantRetention = IsPerCedantRetention;
                entity.CedantRetention = CedantRetention;
                entity.IsPerMlreShare = IsPerMlreShare;
                entity.MlreShare = MlreShare;
                entity.IsPerDistributionChannel = IsPerDistributionChannel;
                entity.DistributionChannel = DistributionChannel;
                entity.IsPerAgeBasis = IsPerAgeBasis;
                entity.AgeBasisPickListDetailId = AgeBasisPickListDetailId;
                entity.IsPerMinEntryAge = IsPerMinEntryAge;
                entity.MinEntryAge = MinEntryAge;
                entity.IsPerMaxEntryAge = IsPerMaxEntryAge;
                entity.MaxEntryAge = MaxEntryAge;
                entity.IsPerMaxExpiryAge = IsPerMaxExpiryAge;
                entity.MaxExpiryAge = MaxExpiryAge;
                entity.IsPerMinSumAssured = IsPerMinSumAssured;
                entity.MinSumAssured = MinSumAssured;
                entity.IsPerMaxSumAssured = IsPerMaxSumAssured;
                entity.MaxSumAssured = MaxSumAssured;
                entity.IsPerReinsuranceRate = IsPerReinsuranceRate;
                entity.ReinsRateTreatyPricingRateTableId = ReinsRateTreatyPricingRateTableId;
                entity.ReinsRateTreatyPricingRateTableVersionId = ReinsRateTreatyPricingRateTableVersionId;
                entity.ReinsRateNote = ReinsRateNote;
                entity.IsPerReinsuranceDiscount = IsPerReinsuranceDiscount;
                entity.ReinsDiscountTreatyPricingRateTableId = ReinsDiscountTreatyPricingRateTableId;
                entity.ReinsDiscountTreatyPricingRateTableVersionId = ReinsDiscountTreatyPricingRateTableVersionId;
                entity.ReinsDiscountNote = ReinsDiscountNote;
                entity.IsPerProfitComm = IsPerProfitComm;
                entity.TreatyPricingProfitCommissionId = TreatyPricingProfitCommissionId;
                entity.TreatyPricingProfitCommissionVersionId = TreatyPricingProfitCommissionVersionId;
                entity.ProfitCommNote = ProfitCommNote;
                entity.CampaignFundByMlre = CampaignFundByMlre;
                entity.ComplimentarySumAssured = ComplimentarySumAssured;
                entity.IsPerUnderwritingMethod = IsPerUnderwritingMethod;
                entity.UnderwritingMethod = UnderwritingMethod;
                entity.IsPerUnderwritingQuestion = IsPerUnderwritingQuestion;
                entity.TreatyPricingUwQuestionnaireId = TreatyPricingUwQuestionnaireId;
                entity.TreatyPricingUwQuestionnaireVersionId = TreatyPricingUwQuestionnaireVersionId;
                entity.UnderwritingQuestionNote = UnderwritingQuestionNote;
                entity.IsPerMedicalTable = IsPerMedicalTable;
                entity.TreatyPricingMedicalTableId = TreatyPricingMedicalTableId;
                entity.TreatyPricingMedicalTableVersionId = TreatyPricingMedicalTableVersionId;
                entity.MedicalTableNote = MedicalTableNote;
                entity.IsPerFinancialTable = IsPerFinancialTable;
                entity.TreatyPricingFinancialTableId = TreatyPricingFinancialTableId;
                entity.TreatyPricingFinancialTableVersionId = TreatyPricingFinancialTableVersionId;
                entity.FinancialTableNote = FinancialTableNote;
                entity.IsPerAggregationNotes = IsPerAggregationNotes;
                entity.AggregationNotes = AggregationNotes;
                entity.IsPerAdvantageProgram = IsPerAdvantageProgram;
                entity.TreatyPricingAdvantageProgramId = TreatyPricingAdvantageProgramId;
                entity.TreatyPricingAdvantageProgramVersionId = TreatyPricingAdvantageProgramVersionId;
                entity.AdvantageProgramNote = AdvantageProgramNote;
                entity.IsPerWaitingPeriod = IsPerWaitingPeriod;
                entity.WaitingPeriod = WaitingPeriod;
                entity.IsPerSurvivalPeriod = IsPerSurvivalPeriod;
                entity.SurvivalPeriod = SurvivalPeriod;
                entity.OtherCampaignCriteria = OtherCampaignCriteria;
                entity.AdditionalRemark = AdditionalRemark;
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
                db.TreatyPricingCampaignVersions.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
