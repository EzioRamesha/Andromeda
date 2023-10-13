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
    [Table("RetroBenefitCodeMappingTreaties")]
    public class RetroBenefitCodeMappingTreaty
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int RetroBenefitCodeMappingId { get; set; }

        [ForeignKey(nameof(RetroBenefitCodeMappingId))]
        [ExcludeTrail]
        public virtual RetroBenefitCodeMapping RetroBenefitCodeMapping { get; set; }

        [MaxLength(35)]
        public string TreatyCode { get; set; }

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

        public RetroBenefitCodeMappingTreaty()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroBenefitCodeMappingTreaties.Any(q => q.Id == id);
            }
        }

        public static RetroBenefitCodeMappingTreaty Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroBenefitCodeMappingTreaties.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RetroBenefitCodeMappingTreaties.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                RetroBenefitCodeMappingTreaty retroBenefitCodeMappingTreaty = Find(Id);
                if (retroBenefitCodeMappingTreaty == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(retroBenefitCodeMappingTreaty, this);

                retroBenefitCodeMappingTreaty.RetroBenefitCodeMappingId = RetroBenefitCodeMappingId;
                retroBenefitCodeMappingTreaty.TreatyCode = TreatyCode;
                retroBenefitCodeMappingTreaty.UpdatedAt = DateTime.Now;
                retroBenefitCodeMappingTreaty.UpdatedById = UpdatedById ?? retroBenefitCodeMappingTreaty.UpdatedById;

                db.Entry(retroBenefitCodeMappingTreaty).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                RetroBenefitCodeMappingTreaty retroBenefitCodeMappingTreaty = db.RetroBenefitCodeMappingTreaties.Where(q => q.Id == id).FirstOrDefault();
                if (retroBenefitCodeMappingTreaty == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(retroBenefitCodeMappingTreaty, true);

                db.Entry(retroBenefitCodeMappingTreaty).State = EntityState.Deleted;
                db.RetroBenefitCodeMappingTreaties.Remove(retroBenefitCodeMappingTreaty);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByRetroBenefitCodeMappingId(int retroBenefitCodeMappingId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroBenefitCodeMappingTreaties.Where(q => q.RetroBenefitCodeMappingId == retroBenefitCodeMappingId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (RetroBenefitCodeMappingTreaty retroBenefitCodeMappingTreaty in query.ToList())
                {
                    DataTrail trail = new DataTrail(retroBenefitCodeMappingTreaty, true);
                    trails.Add(trail);

                    db.Entry(retroBenefitCodeMappingTreaty).State = EntityState.Deleted;
                    db.RetroBenefitCodeMappingTreaties.Remove(retroBenefitCodeMappingTreaty);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
