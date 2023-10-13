using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Shared;
using Shared.Trails;
using Shared.Trails.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace DataAccess.Entities.Retrocession
{
    [Table("PerLifeRetroConfigurationRatios")]
    public class PerLifeRetroConfigurationRatio
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyCodeId { get; set; }

        [ForeignKey(nameof(TreatyCodeId))]
        [ExcludeTrail]
        public virtual TreatyCode TreatyCode { get; set; }

        [Required, Index]
        public double RetroRatio { get; set; }

        [Required, Index]
        public double MlreRetainRatio { get; set; }

        [Required, Index]
        [Column(TypeName = "datetime2")]
        public DateTime ReinsEffectiveStartDate { get; set; }

        [Required, Index]
        [Column(TypeName = "datetime2")]
        public DateTime ReinsEffectiveEndDate { get; set; }

        [Required, Index]
        [Column(TypeName = "datetime2")]
        public DateTime RiskQuarterStartDate { get; set; }

        [Required, Index]
        [Column(TypeName = "datetime2")]
        public DateTime RiskQuarterEndDate { get; set; }

        [Required, Index]
        [Column(TypeName = "datetime2")]
        public DateTime RuleEffectiveDate { get; set; }

        [Required, Index]
        [Column(TypeName = "datetime2")]
        public DateTime RuleCeaseDate { get; set; }

        public double RuleValue { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

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

        public PerLifeRetroConfigurationRatio()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeRetroConfigurationRatios.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicate()
        {
            using (var db = new AppDbContext())
            {
                var treatyCode = db.TreatyCodes.Find(TreatyCodeId);

                var query = db.PerLifeRetroConfigurationRatios
                    .Where(q => q.TreatyCode.Code == treatyCode.Code)
                    .Where(
                        q => DbFunctions.TruncateTime(q.ReinsEffectiveStartDate) <= DbFunctions.TruncateTime(ReinsEffectiveStartDate)
                        && DbFunctions.TruncateTime(q.ReinsEffectiveEndDate) >= DbFunctions.TruncateTime(ReinsEffectiveStartDate)
                        ||
                        DbFunctions.TruncateTime(q.ReinsEffectiveStartDate) <= DbFunctions.TruncateTime(ReinsEffectiveEndDate)
                        && DbFunctions.TruncateTime(q.ReinsEffectiveEndDate) >= DbFunctions.TruncateTime(ReinsEffectiveEndDate)
                    )
                    .Where(
                        q => DbFunctions.TruncateTime(q.RiskQuarterStartDate) <= DbFunctions.TruncateTime(RiskQuarterStartDate)
                        && DbFunctions.TruncateTime(q.RiskQuarterEndDate) >= DbFunctions.TruncateTime(RiskQuarterStartDate)
                        ||
                        DbFunctions.TruncateTime(q.RiskQuarterStartDate) <= DbFunctions.TruncateTime(RiskQuarterEndDate)
                        && DbFunctions.TruncateTime(q.RiskQuarterEndDate) >= DbFunctions.TruncateTime(RiskQuarterEndDate)
                    )
                    .Where(
                        q => DbFunctions.TruncateTime(q.RuleEffectiveDate) <= DbFunctions.TruncateTime(RuleEffectiveDate)
                        && DbFunctions.TruncateTime(q.RuleCeaseDate) >= DbFunctions.TruncateTime(RuleEffectiveDate)
                        ||
                        DbFunctions.TruncateTime(q.RuleEffectiveDate) <= DbFunctions.TruncateTime(RuleCeaseDate)
                        && DbFunctions.TruncateTime(q.RuleCeaseDate) >= DbFunctions.TruncateTime(RuleCeaseDate)
                    );

                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public static PerLifeRetroConfigurationRatio Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeRetroConfigurationRatios.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PerLifeRetroConfigurationRatios.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                PerLifeRetroConfigurationRatio perLifeRetroConfigurationRatio = Find(Id);
                if (perLifeRetroConfigurationRatio == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeRetroConfigurationRatio, this);

                perLifeRetroConfigurationRatio.TreatyCodeId = TreatyCodeId;
                perLifeRetroConfigurationRatio.RetroRatio = RetroRatio;
                perLifeRetroConfigurationRatio.MlreRetainRatio = MlreRetainRatio;
                perLifeRetroConfigurationRatio.ReinsEffectiveStartDate = ReinsEffectiveStartDate;
                perLifeRetroConfigurationRatio.ReinsEffectiveEndDate = ReinsEffectiveEndDate;
                perLifeRetroConfigurationRatio.RiskQuarterStartDate = RiskQuarterStartDate;
                perLifeRetroConfigurationRatio.RiskQuarterEndDate = RiskQuarterEndDate;
                perLifeRetroConfigurationRatio.RuleEffectiveDate = RuleEffectiveDate;
                perLifeRetroConfigurationRatio.RuleCeaseDate = RuleCeaseDate;
                perLifeRetroConfigurationRatio.RuleValue = RuleValue;
                perLifeRetroConfigurationRatio.Description = Description;
                perLifeRetroConfigurationRatio.UpdatedAt = DateTime.Now;
                perLifeRetroConfigurationRatio.UpdatedById = UpdatedById ?? perLifeRetroConfigurationRatio.UpdatedById;

                db.Entry(perLifeRetroConfigurationRatio).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                PerLifeRetroConfigurationRatio perLifeRetroConfigurationRatio = db.PerLifeRetroConfigurationRatios.Where(q => q.Id == id).FirstOrDefault();
                if (perLifeRetroConfigurationRatio == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeRetroConfigurationRatio, true);

                db.Entry(perLifeRetroConfigurationRatio).State = EntityState.Deleted;
                db.PerLifeRetroConfigurationRatios.Remove(perLifeRetroConfigurationRatio);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
