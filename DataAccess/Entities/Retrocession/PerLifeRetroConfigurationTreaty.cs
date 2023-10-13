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
    [Table("PerLifeRetroConfigurationTreaties")]
    public class PerLifeRetroConfigurationTreaty
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyCodeId { get; set; }

        [ForeignKey(nameof(TreatyCodeId))]
        [ExcludeTrail]
        public virtual TreatyCode TreatyCode { get; set; }

        [Required, Index]
        public int TreatyTypePickListDetailId { get; set; }

        [ForeignKey(nameof(TreatyTypePickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail TreatyTypePickListDetail { get; set; }

        [Required, Index]
        public int FundsAccountingTypePickListDetailId { get; set; }

        [ForeignKey(nameof(FundsAccountingTypePickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail FundsAccountingTypePickListDetail { get; set; }

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
        public bool IsToAggregate { get; set; } = false;

        [Column(TypeName = "ntext")]
        public string Remark { get; set; }

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

        public PerLifeRetroConfigurationTreaty()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeRetroConfigurationTreaties.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicate()
        {
            using (var db = new AppDbContext())
            {
                var treatyCode = db.TreatyCodes.Find(TreatyCodeId);

                var query = db.PerLifeRetroConfigurationTreaties
                    .Where(q => q.TreatyCode.Code == treatyCode.Code)
                    .Where(q => q.TreatyTypePickListDetailId == TreatyTypePickListDetailId)
                    .Where(q => q.FundsAccountingTypePickListDetailId == FundsAccountingTypePickListDetailId)
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
                    );

                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public static PerLifeRetroConfigurationTreaty Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeRetroConfigurationTreaties.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PerLifeRetroConfigurationTreaties.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                PerLifeRetroConfigurationTreaty perLifeRetroConfigurationTreaty = Find(Id);
                if (perLifeRetroConfigurationTreaty == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeRetroConfigurationTreaty, this);

                perLifeRetroConfigurationTreaty.TreatyCodeId = TreatyCodeId;
                perLifeRetroConfigurationTreaty.TreatyTypePickListDetailId = TreatyTypePickListDetailId;
                perLifeRetroConfigurationTreaty.FundsAccountingTypePickListDetailId = FundsAccountingTypePickListDetailId;
                perLifeRetroConfigurationTreaty.ReinsEffectiveStartDate = ReinsEffectiveStartDate;
                perLifeRetroConfigurationTreaty.ReinsEffectiveEndDate = ReinsEffectiveEndDate;
                perLifeRetroConfigurationTreaty.RiskQuarterStartDate = RiskQuarterStartDate;
                perLifeRetroConfigurationTreaty.RiskQuarterEndDate = RiskQuarterEndDate;
                perLifeRetroConfigurationTreaty.IsToAggregate = IsToAggregate;
                perLifeRetroConfigurationTreaty.Remark = Remark;
                perLifeRetroConfigurationTreaty.UpdatedAt = DateTime.Now;
                perLifeRetroConfigurationTreaty.UpdatedById = UpdatedById ?? perLifeRetroConfigurationTreaty.UpdatedById;

                db.Entry(perLifeRetroConfigurationTreaty).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                PerLifeRetroConfigurationTreaty perLifeRetroConfigurationTreaty = db.PerLifeRetroConfigurationTreaties.Where(q => q.Id == id).FirstOrDefault();
                if (perLifeRetroConfigurationTreaty == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeRetroConfigurationTreaty, true);

                db.Entry(perLifeRetroConfigurationTreaty).State = EntityState.Deleted;
                db.PerLifeRetroConfigurationTreaties.Remove(perLifeRetroConfigurationTreaty);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
