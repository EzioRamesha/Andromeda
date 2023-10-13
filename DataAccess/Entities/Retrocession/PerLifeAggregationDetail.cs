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
    [Table("PerLifeAggregationDetails")]
    public class PerLifeAggregationDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int PerLifeAggregationId { get; set; }

        [ForeignKey(nameof(PerLifeAggregationId))]
        [ExcludeTrail]
        public virtual PerLifeAggregation PerLifeAggregation { get; set; }

        [Index]
        [MaxLength(64)]
        public string RiskQuarter { get; set; }

        [Index]
        [Column(TypeName = "datetime2")]
        public DateTime? ProcessingDate { get; set; }

        [Required, Index]
        public int Status { get; set; }

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

        public PerLifeAggregationDetail()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeAggregationDetails.Any(q => q.Id == id);
            }
        }

        public static PerLifeAggregationDetail Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeAggregationDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PerLifeAggregationDetails.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                PerLifeAggregationDetail perLifeAggregationDetail = Find(Id);
                if (perLifeAggregationDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeAggregationDetail, this);

                perLifeAggregationDetail.PerLifeAggregationId = PerLifeAggregationId;
                perLifeAggregationDetail.RiskQuarter = RiskQuarter;
                perLifeAggregationDetail.ProcessingDate = ProcessingDate;
                perLifeAggregationDetail.Status = Status;
                perLifeAggregationDetail.UpdatedAt = DateTime.Now;
                perLifeAggregationDetail.UpdatedById = UpdatedById ?? perLifeAggregationDetail.UpdatedById;

                db.Entry(perLifeAggregationDetail).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                PerLifeAggregationDetail perLifeAggregationDetail = db.PerLifeAggregationDetails.Where(q => q.Id == id).FirstOrDefault();
                if (perLifeAggregationDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeAggregationDetail, true);

                db.Entry(perLifeAggregationDetail).State = EntityState.Deleted;
                db.PerLifeAggregationDetails.Remove(perLifeAggregationDetail);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByPerLifeAggregationId(int perLifeAggregationId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.PerLifeAggregationDetails.Where(q => q.PerLifeAggregationId == perLifeAggregationId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (PerLifeAggregationDetail perLifeAggregationDetail in query.ToList())
                {
                    DataTrail trail = new DataTrail(perLifeAggregationDetail, true);
                    trails.Add(trail);

                    db.Entry(perLifeAggregationDetail).State = EntityState.Deleted;
                    db.PerLifeAggregationDetails.Remove(perLifeAggregationDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
