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
    [Table("PerLifeSoaData")]
    public class PerLifeSoaData
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int PerLifeSoaId { get; set; }
        [ExcludeTrail]
        public virtual PerLifeSoa PerLifeSoa { get; set; }

        [Index]
        public int? PerLifeAggregationDetailDataId { get; set; }
        [ExcludeTrail]
        public virtual PerLifeAggregationDetailData PerLifeAggregationDetailData { get; set; }

        [Index]
        public int? PerLifeClaimDataId { get; set; }
        [ExcludeTrail]
        public virtual PerLifeClaimData PerLifeClaimData { get; set; }

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

        public PerLifeSoaData()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeSoaData.Any(q => q.Id == id);
            }
        }

        public static PerLifeSoaData Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeSoaData.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PerLifeSoaData.Add(this);
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

                entity.PerLifeSoaId = PerLifeSoaId;
                entity.PerLifeAggregationDetailDataId = PerLifeAggregationDetailDataId;
                entity.PerLifeClaimDataId = PerLifeClaimDataId;
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
                db.PerLifeSoaData.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByPerLifeSoaId(int perLifeSoaId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.PerLifeSoaData.Where(q => q.PerLifeSoaId == perLifeSoaId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (PerLifeSoaData entity in query.ToList())
                {
                    DataTrail trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.PerLifeSoaData.Remove(entity);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
