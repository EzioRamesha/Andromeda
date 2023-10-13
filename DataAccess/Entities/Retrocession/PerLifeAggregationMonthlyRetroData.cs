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
    [Table("PerLifeAggregationMonthlyRetroData")]
    public class PerLifeAggregationMonthlyRetroData
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int PerLifeAggregationMonthlyDataId { get; set; }

        [ForeignKey(nameof(PerLifeAggregationMonthlyDataId))]
        [ExcludeTrail]
        public virtual PerLifeAggregationMonthlyData PerLifeAggregationMonthlyData { get; set; }

        [MaxLength(50)]
        public string RetroParty { get; set; }

        public double? RetroAmount { get; set; }

        public double? RetroGrossPremium { get; set; }

        public double? RetroNetPremium { get; set; }

        public double? RetroDiscount { get; set; }

        public double? RetroGst { get; set; }

        public double? MlreShare { get; set; }

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

        public PerLifeAggregationMonthlyRetroData()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeAggregationMonthlyRetroData.Any(q => q.Id == id);
            }
        }

        public static PerLifeAggregationMonthlyRetroData Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeAggregationMonthlyRetroData.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PerLifeAggregationMonthlyRetroData.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                PerLifeAggregationMonthlyRetroData perLifeAggregationMonthlyRetroData = Find(Id);
                if (perLifeAggregationMonthlyRetroData == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeAggregationMonthlyRetroData, this);

                perLifeAggregationMonthlyRetroData.PerLifeAggregationMonthlyDataId = PerLifeAggregationMonthlyDataId;
                perLifeAggregationMonthlyRetroData.RetroParty = RetroParty;
                perLifeAggregationMonthlyRetroData.RetroAmount = RetroAmount;
                perLifeAggregationMonthlyRetroData.RetroGrossPremium = RetroGrossPremium;
                perLifeAggregationMonthlyRetroData.RetroNetPremium = RetroNetPremium;
                perLifeAggregationMonthlyRetroData.RetroDiscount = RetroDiscount;
                perLifeAggregationMonthlyRetroData.RetroGst = RetroGst;
                perLifeAggregationMonthlyRetroData.MlreShare = MlreShare;
                perLifeAggregationMonthlyRetroData.UpdatedAt = DateTime.Now;
                perLifeAggregationMonthlyRetroData.UpdatedById = UpdatedById ?? perLifeAggregationMonthlyRetroData.UpdatedById;

                db.Entry(perLifeAggregationMonthlyRetroData).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                PerLifeAggregationMonthlyRetroData perLifeAggregationMonthlyRetroData = db.PerLifeAggregationMonthlyRetroData.Where(q => q.Id == id).FirstOrDefault();
                if (perLifeAggregationMonthlyRetroData == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeAggregationMonthlyRetroData, true);

                db.Entry(perLifeAggregationMonthlyRetroData).State = EntityState.Deleted;
                db.PerLifeAggregationMonthlyRetroData.Remove(perLifeAggregationMonthlyRetroData);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByPerLifeAggregationMonthlyDataId(int perLifeAggregationMonthlyDataId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.PerLifeAggregationMonthlyRetroData.Where(q => q.PerLifeAggregationMonthlyDataId == perLifeAggregationMonthlyDataId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (PerLifeAggregationMonthlyRetroData perLifeAggregationMonthlyRetroData in query.ToList())
                {
                    DataTrail trail = new DataTrail(perLifeAggregationMonthlyRetroData, true);
                    trails.Add(trail);

                    db.Entry(perLifeAggregationMonthlyRetroData).State = EntityState.Deleted;
                    db.PerLifeAggregationMonthlyRetroData.Remove(perLifeAggregationMonthlyRetroData);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
