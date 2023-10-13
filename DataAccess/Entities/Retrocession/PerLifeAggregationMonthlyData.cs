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
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities.Retrocession
{
    [Table("PerLifeAggregationMonthlyData")]
    public class PerLifeAggregationMonthlyData
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int PerLifeAggregationDetailDataId { get; set; }

        [ForeignKey(nameof(PerLifeAggregationDetailDataId))]
        [ExcludeTrail]
        public virtual PerLifeAggregationDetailData PerLifeAggregationDetailData { get; set; }

        [Index]
        public int RiskYear { get; set; }

        [Index]
        public int RiskMonth { get; set; }

        public string UniqueKeyPerLife { get; set; }

        public string RetroPremFreq { get; set; }

        public double Aar { get; set; }

        public double? SumOfAar { get; set; }

        public double NetPremium { get; set; }

        public double? SumOfNetPremium { get; set; }

        public double RetroRatio { get; set; }

        public double? RetentionLimit { get; set; }

        public double? DistributedRetentionLimit { get; set; }

        public double? RetroAmount { get; set; }

        public double? DistributedRetroAmount { get; set; }

        public double? AccumulativeRetainAmount { get; set; }

        public double? RetroGrossPremium { get; set; }

        public double? RetroNetPremium { get; set; }

        public double? RetroDiscount { get; set; }

        public bool RetroIndicator { get; set; } = true;

        [Column(TypeName = "ntext")]
        public string Errors { get; set; }

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
        public virtual ICollection<PerLifeAggregationMonthlyRetroData> PerLifeAggregationMonthlyRetroData { get; set; }

        public PerLifeAggregationMonthlyData()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeAggregationMonthlyData.Any(q => q.Id == id);
            }
        }

        public static PerLifeAggregationMonthlyData Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeAggregationMonthlyData.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PerLifeAggregationMonthlyData.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                PerLifeAggregationMonthlyData entity = Find(Id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, this);

                entity.PerLifeAggregationDetailDataId = PerLifeAggregationDetailDataId;
                entity.RiskYear = RiskYear;
                entity.RiskMonth = RiskMonth;
                entity.UniqueKeyPerLife = UniqueKeyPerLife;
                entity.RetroPremFreq = RetroPremFreq;
                entity.Aar = Aar;
                entity.SumOfAar = SumOfAar;
                entity.NetPremium = NetPremium;
                entity.SumOfNetPremium = SumOfNetPremium;
                entity.RetroRatio = RetroRatio;
                entity.RetentionLimit = RetentionLimit;
                entity.DistributedRetentionLimit = DistributedRetentionLimit;
                entity.RetroAmount = RetroAmount;
                entity.DistributedRetroAmount = DistributedRetroAmount;
                entity.AccumulativeRetainAmount = AccumulativeRetainAmount;
                entity.RetroGrossPremium = RetroGrossPremium;
                entity.RetroNetPremium = RetroNetPremium;
                entity.RetroDiscount = RetroDiscount;
                entity.RetroIndicator = RetroIndicator;
                entity.Errors = Errors;
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
                PerLifeAggregationMonthlyData entity = db.PerLifeAggregationMonthlyData.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.PerLifeAggregationMonthlyData.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByPerLifeAggregationDetailDataId(int perLifeAggregationDetailDataId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.PerLifeAggregationMonthlyData.Where(q => q.PerLifeAggregationDetailDataId == perLifeAggregationDetailDataId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (PerLifeAggregationMonthlyData perLifeAggregationMonthlyData in query.ToList())
                {
                    DataTrail trail = new DataTrail(perLifeAggregationMonthlyData, true);
                    trails.Add(trail);

                    db.Entry(perLifeAggregationMonthlyData).State = EntityState.Deleted;
                    db.PerLifeAggregationMonthlyData.Remove(perLifeAggregationMonthlyData);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
