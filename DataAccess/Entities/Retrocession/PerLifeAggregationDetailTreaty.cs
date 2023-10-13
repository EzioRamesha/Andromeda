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
    [Table("PerLifeAggregationDetailTreaties")]
    public class PerLifeAggregationDetailTreaty
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int PerLifeAggregationDetailId { get; set; }

        [ForeignKey(nameof(PerLifeAggregationDetailId))]
        [ExcludeTrail]
        public virtual PerLifeAggregationDetail PerLifeAggregationDetail { get; set; }

        [Required, MaxLength(35), Index]
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

        public PerLifeAggregationDetailTreaty()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeAggregationDetailTreaties.Any(q => q.Id == id);
            }
        }

        public static PerLifeAggregationDetailTreaty Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeAggregationDetailTreaties.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PerLifeAggregationDetailTreaties.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                PerLifeAggregationDetailTreaty perLifeAggregationDetailTreaty = Find(Id);
                if (perLifeAggregationDetailTreaty == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeAggregationDetailTreaty, this);

                perLifeAggregationDetailTreaty.PerLifeAggregationDetailId = PerLifeAggregationDetailId;
                perLifeAggregationDetailTreaty.TreatyCode = TreatyCode;
                perLifeAggregationDetailTreaty.UpdatedAt = DateTime.Now;
                perLifeAggregationDetailTreaty.UpdatedById = UpdatedById ?? perLifeAggregationDetailTreaty.UpdatedById;

                db.Entry(perLifeAggregationDetailTreaty).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                PerLifeAggregationDetailTreaty perLifeAggregationDetailTreaty = db.PerLifeAggregationDetailTreaties.Where(q => q.Id == id).FirstOrDefault();
                if (perLifeAggregationDetailTreaty == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeAggregationDetailTreaty, true);

                db.Entry(perLifeAggregationDetailTreaty).State = EntityState.Deleted;
                db.PerLifeAggregationDetailTreaties.Remove(perLifeAggregationDetailTreaty);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByPerLifeAggregationDetailId(int perLifeAggregationDetailId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.PerLifeAggregationDetailTreaties.Where(q => q.PerLifeAggregationDetailId == perLifeAggregationDetailId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (PerLifeAggregationDetailTreaty perLifeAggregationDetailTreaty in query.ToList())
                {
                    DataTrail trail = new DataTrail(perLifeAggregationDetailTreaty, true);
                    trails.Add(trail);

                    db.Entry(perLifeAggregationDetailTreaty).State = EntityState.Deleted;
                    db.PerLifeAggregationDetailTreaties.Remove(perLifeAggregationDetailTreaty);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
