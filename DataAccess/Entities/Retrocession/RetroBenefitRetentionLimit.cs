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
    [Table("RetroBenefitRetentionLimits")]
    public class RetroBenefitRetentionLimit
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int RetroBenefitCodeId { get; set; }

        [ForeignKey(nameof(RetroBenefitCodeId))]
        [ExcludeTrail]
        public virtual RetroBenefitCode RetroBenefitCode { get; set; }

        [Required, Index]
        public int Type { get; set; }

        [Required, MaxLength(255), Index]
        public string Description { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime EffectiveStartDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime EffectiveEndDate { get; set; }

        [Required, Index]
        public double MinRetentionLimit { get; set; }

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

        public RetroBenefitRetentionLimit()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroBenefitRetentionLimits.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicate()
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroBenefitRetentionLimits
                    .Where(q => q.RetroBenefitCodeId == RetroBenefitCodeId)
                    .Where(q => q.Type == Type)
                    .Where(q =>
                        q.EffectiveStartDate <= EffectiveStartDate && q.EffectiveEndDate >= EffectiveStartDate
                        ||
                        q.EffectiveStartDate <= EffectiveEndDate && q.EffectiveEndDate >= EffectiveEndDate
                    );
                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public static RetroBenefitRetentionLimit Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroBenefitRetentionLimits.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RetroBenefitRetentionLimits.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                RetroBenefitRetentionLimit retroBenefitRetentionLimit = Find(Id);
                if (retroBenefitRetentionLimit == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(retroBenefitRetentionLimit, this);

                retroBenefitRetentionLimit.RetroBenefitCodeId = RetroBenefitCodeId;
                retroBenefitRetentionLimit.Type = Type;
                retroBenefitRetentionLimit.Description = Description;
                retroBenefitRetentionLimit.EffectiveStartDate = EffectiveStartDate;
                retroBenefitRetentionLimit.EffectiveEndDate = EffectiveEndDate;
                retroBenefitRetentionLimit.MinRetentionLimit = MinRetentionLimit;
                retroBenefitRetentionLimit.UpdatedAt = DateTime.Now;
                retroBenefitRetentionLimit.UpdatedById = UpdatedById ?? retroBenefitRetentionLimit.UpdatedById;

                db.Entry(retroBenefitRetentionLimit).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                RetroBenefitRetentionLimit retroBenefitRetentionLimit = db.RetroBenefitRetentionLimits.Where(q => q.Id == id).FirstOrDefault();
                if (retroBenefitRetentionLimit == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(retroBenefitRetentionLimit, true);

                db.Entry(retroBenefitRetentionLimit).State = EntityState.Deleted;
                db.RetroBenefitRetentionLimits.Remove(retroBenefitRetentionLimit);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
