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

namespace DataAccess.Entities.Retrocession
{
    [Table("RetroBenefitCodeMappings")]
    public class RetroBenefitCodeMapping
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int BenefitId { get; set; }

        [ForeignKey(nameof(BenefitId))]
        [ExcludeTrail]
        public virtual Benefit Benefit { get; set; }

        [Index]
        public bool IsPerAnnum { get; set; } = false;

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

        [ExcludeTrail]
        public virtual ICollection<RetroBenefitCodeMappingTreaty> RetroBenefitCodeMappingTreaties { get; set; }

        [ExcludeTrail]
        public virtual ICollection<RetroBenefitCodeMappingDetail> RetroBenefitCodeMappingDetails { get; set; }

        public RetroBenefitCodeMapping()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroBenefitCodeMappings.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicate()
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroBenefitCodeMappings
                    .Where(q => q.BenefitId == BenefitId)
                    .Where(q => q.IsPerAnnum == IsPerAnnum);
                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public static RetroBenefitCodeMapping Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroBenefitCodeMappings.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RetroBenefitCodeMappings.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                RetroBenefitCodeMapping retroBenefitCodeMapping = Find(Id);
                if (retroBenefitCodeMapping == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(retroBenefitCodeMapping, this);

                retroBenefitCodeMapping.BenefitId = BenefitId;
                retroBenefitCodeMapping.IsPerAnnum = IsPerAnnum;
                retroBenefitCodeMapping.UpdatedAt = DateTime.Now;
                retroBenefitCodeMapping.UpdatedById = UpdatedById ?? retroBenefitCodeMapping.UpdatedById;

                db.Entry(retroBenefitCodeMapping).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                RetroBenefitCodeMapping retroBenefitCodeMapping = db.RetroBenefitCodeMappings.Where(q => q.Id == id).FirstOrDefault();
                if (retroBenefitCodeMapping == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(retroBenefitCodeMapping, true);

                db.Entry(retroBenefitCodeMapping).State = EntityState.Deleted;
                db.RetroBenefitCodeMappings.Remove(retroBenefitCodeMapping);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
