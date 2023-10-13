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
    [Table("PerLifeClaimRetroData")]
    public class PerLifeClaimRetroData
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int PerLifeClaimDataId { get; set; }
        [ExcludeTrail]
        public virtual PerLifeClaimData PerLifeClaimData { get; set; }

        public double? MlreShare { get; set; }

        public double? RetroClaimRecoveryAmount { get; set; }

        public double? LateInterest { get; set; }

        public double? ExGratia { get; set; }

        [Index]
        public int? RetroRecoveryId { get; set; }

        [Index]
        public int? RetroTreatyId { get; set; }
        [ExcludeTrail]
        public virtual RetroTreaty RetroTreaty { get; set; }

        public double? RetroRatio { get; set; }

        public double? Aar { get; set; }

        public double? ComputedRetroRecoveryAmount { get; set; }

        public double? ComputedRetroLateInterest { get; set; }

        public double? ComputedRetroExGratia { get; set; }

        [MaxLength(30)]
        [Index]
        public string ReportedSoaQuarter { get; set; }

        public double? RetroRecoveryAmount { get; set; }

        public double? RetroLateInterest { get; set; }

        public double? RetroExGratia { get; set; }

        [Index]
        public int ComputedClaimCategory { get; set; }

        [Index]
        public int ClaimCategory { get; set; }

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

        public PerLifeClaimRetroData()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeClaimRetroData.Any(q => q.Id == id);
            }
        }

        public static PerLifeClaimRetroData Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeClaimRetroData.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PerLifeClaimRetroData.Add(this);
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

                entity.PerLifeClaimDataId = PerLifeClaimDataId;
                entity.MlreShare = MlreShare;
                entity.RetroClaimRecoveryAmount = RetroClaimRecoveryAmount;
                entity.LateInterest = LateInterest;
                entity.ExGratia = ExGratia;
                entity.RetroRecoveryId = RetroRecoveryId;
                entity.RetroTreatyId = RetroTreatyId;
                entity.Aar = Aar;
                entity.RetroRatio = RetroRatio;
                entity.ComputedRetroRecoveryAmount = ComputedRetroRecoveryAmount;
                entity.ComputedRetroLateInterest = ComputedRetroLateInterest;
                entity.ComputedRetroExGratia = ComputedRetroExGratia;
                entity.ReportedSoaQuarter = ReportedSoaQuarter;
                entity.RetroRecoveryAmount = RetroRecoveryAmount;
                entity.RetroLateInterest = RetroLateInterest;
                entity.RetroExGratia = RetroExGratia;
                entity.ComputedClaimCategory = ComputedClaimCategory;
                entity.ClaimCategory = ClaimCategory;
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
                db.PerLifeClaimRetroData.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByPerLifeClaimDataId(int perLifeClaimDataId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.PerLifeClaimRetroData.Where(q => q.PerLifeClaimDataId == perLifeClaimDataId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (PerLifeClaimRetroData entity in query.ToList())
                {
                    DataTrail trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.PerLifeClaimRetroData.Remove(entity);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
