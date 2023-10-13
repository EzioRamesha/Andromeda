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
    [Table("GstMaintenances")]
    public class GstMaintenance
    {
        [Key]
        public int Id { get; set; }

        [Index]
        [Column(TypeName = "datetime2")]
        public DateTime? EffectiveStartDate { get; set; }

        [Index]
        [Column(TypeName = "datetime2")]
        public DateTime? EffectiveEndDate { get; set; }

        [Index]
        [Column(TypeName = "datetime2")]
        public DateTime? RiskEffectiveStartDate { get; set; }

        [Index]
        [Column(TypeName = "datetime2")]
        public DateTime? RiskEffectiveEndDate { get; set; }

        [Index]
        public double Rate { get; set; }

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

        public GstMaintenance()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.GstMaintenances.Any(q => q.Id == id);
            }
        }

        public static GstMaintenance Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.GstMaintenances.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public bool IsDuplicate()
        {
            using (var db = new AppDbContext())
            {
                var query = db.GstMaintenances
                    .Where(
                        q => DbFunctions.TruncateTime(q.EffectiveStartDate) <= DbFunctions.TruncateTime(EffectiveStartDate)
                        && DbFunctions.TruncateTime(q.EffectiveEndDate) >= DbFunctions.TruncateTime(EffectiveStartDate)
                        ||
                        DbFunctions.TruncateTime(q.EffectiveStartDate) <= DbFunctions.TruncateTime(EffectiveEndDate)
                        && DbFunctions.TruncateTime(q.EffectiveEndDate) >= DbFunctions.TruncateTime(EffectiveEndDate)
                    )
                    .Where(
                        q => DbFunctions.TruncateTime(q.RiskEffectiveStartDate) <= DbFunctions.TruncateTime(RiskEffectiveStartDate)
                        && DbFunctions.TruncateTime(q.RiskEffectiveEndDate) >= DbFunctions.TruncateTime(RiskEffectiveStartDate)
                        ||
                        DbFunctions.TruncateTime(q.RiskEffectiveStartDate) <= DbFunctions.TruncateTime(RiskEffectiveEndDate)
                        && DbFunctions.TruncateTime(q.RiskEffectiveEndDate) >= DbFunctions.TruncateTime(RiskEffectiveEndDate)
                    );

                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.GstMaintenances.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                GstMaintenance gstMaintenances = Find(Id);
                if (gstMaintenances == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(gstMaintenances, this);

                gstMaintenances.EffectiveStartDate = EffectiveStartDate;
                gstMaintenances.EffectiveEndDate = EffectiveEndDate;
                gstMaintenances.RiskEffectiveStartDate = RiskEffectiveStartDate;
                gstMaintenances.RiskEffectiveEndDate = RiskEffectiveEndDate;
                gstMaintenances.Rate = Rate;

                gstMaintenances.UpdatedAt = DateTime.Now;
                gstMaintenances.UpdatedById = UpdatedById ?? gstMaintenances.UpdatedById;

                db.Entry(gstMaintenances).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                GstMaintenance gstMaintenance = db.GstMaintenances.Where(q => q.Id == id).FirstOrDefault();
                if (gstMaintenance == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(gstMaintenance, true);

                db.Entry(gstMaintenance).State = EntityState.Deleted;
                db.GstMaintenances.Remove(gstMaintenance);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
