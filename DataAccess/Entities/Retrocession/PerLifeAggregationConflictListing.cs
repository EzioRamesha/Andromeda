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

namespace DataAccess.Entities.Retrocession
{
    [Table("PerLifeAggregationConflictListings")]
    public class PerLifeAggregationConflictListing
    {
        [Key]
        public int Id { get; set; }

        [Index]
        public int? TreatyCodeId { get; set; } // Get treatyId, TreatyCode, treatyType, LOB
        [ExcludeTrail]
        public virtual TreatyCode TreatyCode { get; set; }

        [Index]
        public int? RiskYear { get; set; }

        [Index]
        public int? RiskMonth { get; set; }

        [Index]
        [MaxLength(255)]
        public string InsuredName { get; set; }

        [Index]
        public int? InsuredGenderCodePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail InsuredGenderCodePickListDetail { get; set; }

        [Index]
        public DateTime? InsuredDateOfBirth { get; set; }

        [MaxLength(50), Index]
        public string PolicyNumber { get; set; }

        [Index]
        public DateTime? ReinsEffectiveDatePol { get; set; }

        [Index]
        public double? AAR { get; set; }

        [Index]
        public double? GrossPremium { get; set; }

        [Index]
        public double? NetPremium { get; set; }

        [Index]
        public int? PremiumFrequencyModePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail PremiumFrequencyModePickListDetail { get; set; }

        [Index]
        public int? RetroPremiumFrequencyModePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail RetroPremiumFrequencyModePickListDetail { get; set; }

        [MaxLength(255), Index]
        public string CedantPlanCode { get; set; }

        [MaxLength(30)]
        [Index]
        public string CedingBenefitRiskCode { get; set; }

        [MaxLength(30)]
        [Index]
        public string CedingBenefitTypeCode { get; set; }

        [Index]
        public int? MLReBenefitCodeId { get; set; }
        [ExcludeTrail]
        public virtual Benefit MLReBenefitCode { get; set; }

        [Index]
        public int? TerritoryOfIssueCodePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail TerritoryOfIssueCodePickListDetail { get; set; }

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

        public PerLifeAggregationConflictListing()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeAggregationConflictListings.Any(q => q.Id == id);
            }
        }

        public static PerLifeAggregationConflictListing Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeAggregationConflictListings.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PerLifeAggregationConflictListings.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                PerLifeAggregationConflictListing perLifeAggregationConflictListing = Find(Id);
                if (perLifeAggregationConflictListing == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeAggregationConflictListing, this);

                perLifeAggregationConflictListing.TreatyCodeId = TreatyCodeId;
                perLifeAggregationConflictListing.RiskYear = RiskYear;
                perLifeAggregationConflictListing.RiskMonth = RiskMonth;
                perLifeAggregationConflictListing.InsuredName = InsuredName;
                perLifeAggregationConflictListing.InsuredGenderCodePickListDetailId = InsuredGenderCodePickListDetailId;
                perLifeAggregationConflictListing.InsuredDateOfBirth = InsuredDateOfBirth;
                perLifeAggregationConflictListing.PolicyNumber = PolicyNumber;
                perLifeAggregationConflictListing.ReinsEffectiveDatePol = ReinsEffectiveDatePol;
                perLifeAggregationConflictListing.AAR = AAR;
                perLifeAggregationConflictListing.GrossPremium = GrossPremium;
                perLifeAggregationConflictListing.NetPremium = NetPremium;
                perLifeAggregationConflictListing.PremiumFrequencyModePickListDetailId = PremiumFrequencyModePickListDetailId;
                perLifeAggregationConflictListing.RetroPremiumFrequencyModePickListDetailId = RetroPremiumFrequencyModePickListDetailId;
                perLifeAggregationConflictListing.CedantPlanCode = CedantPlanCode;
                perLifeAggregationConflictListing.CedingBenefitRiskCode = CedingBenefitRiskCode;
                perLifeAggregationConflictListing.CedingBenefitTypeCode = CedingBenefitTypeCode;
                perLifeAggregationConflictListing.MLReBenefitCodeId = MLReBenefitCodeId;
                perLifeAggregationConflictListing.TerritoryOfIssueCodePickListDetailId = TerritoryOfIssueCodePickListDetailId;



                perLifeAggregationConflictListing.UpdatedAt = DateTime.Now;
                perLifeAggregationConflictListing.UpdatedById = UpdatedById ?? perLifeAggregationConflictListing.UpdatedById;

                db.Entry(perLifeAggregationConflictListing).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                PerLifeAggregationConflictListing PerLifeAggregationConflictListing = db.PerLifeAggregationConflictListings.Where(q => q.Id == id).FirstOrDefault();
                if (PerLifeAggregationConflictListing == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(PerLifeAggregationConflictListing, true);

                db.Entry(PerLifeAggregationConflictListing).State = EntityState.Deleted;
                db.PerLifeAggregationConflictListings.Remove(PerLifeAggregationConflictListing);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
