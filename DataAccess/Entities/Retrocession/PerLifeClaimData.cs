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
    [Table("PerLifeClaimData")]
    public class PerLifeClaimData
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int PerLifeClaimId { get; set; }
        [ExcludeTrail]
        public virtual PerLifeClaim PerLifeClaim { get; set; }

        [Required, Index]
        public int ClaimRegisterHistoryId { get; set; }
        [ExcludeTrail]
        public virtual ClaimRegisterHistory ClaimRegisterHistory { get; set; }

        [Index]
        public int? PerLifeAggregationDetailDataId { get; set; }
        [ExcludeTrail]
        public virtual PerLifeAggregationDetailData PerLifeAggregationDetailData { get; set; }

        [Index]
        public bool IsException { get; set; } = false;

        [Index]
        public int? ClaimCategory { get; set; }

        [Index]
        public bool IsExcludePerformClaimRecovery { get; set; } = false;

        [Index]
        public int? ClaimRecoveryStatus { get; set; }

        [Index]
        public int? ClaimRecoveryDecision { get; set; }

        [Index]
        public int? MovementType { get; set; }

        [MaxLength(255)]
        [Index]
        public string PerLifeRetro { get; set; }

        [Index]
        public int? RetroOutputId { get; set; }

        [Index]
        public int? RetainPoolId { get; set; }

        [Index]
        public int? NoOfRetroTreaty { get; set; }

        [Index]
        public int? RetroRecoveryId { get; set; }

        [Index]
        public bool IsLateInterestShare { get; set; } = false;

        [Index]
        public bool IsExGratiaShare { get; set; } = false;

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

        public PerLifeClaimData()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeClaimData.Any(q => q.Id == id);
            }
        }

        public static PerLifeClaimData Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeClaimData.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PerLifeClaimData.Add(this);
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

                entity.PerLifeClaimId = PerLifeClaimId;
                entity.ClaimRegisterHistoryId = ClaimRegisterHistoryId;
                entity.PerLifeAggregationDetailDataId = PerLifeAggregationDetailDataId;
                entity.IsException = IsException;
                entity.ClaimCategory = ClaimCategory;
                entity.IsExcludePerformClaimRecovery = IsExcludePerformClaimRecovery;
                entity.ClaimRecoveryStatus = ClaimRecoveryStatus;
                entity.ClaimRecoveryDecision = ClaimRecoveryDecision;
                entity.MovementType = MovementType;
                entity.PerLifeRetro = PerLifeRetro;
                entity.RetroOutputId = RetroOutputId;
                entity.RetainPoolId = RetainPoolId;
                entity.NoOfRetroTreaty = NoOfRetroTreaty;
                entity.RetroRecoveryId = RetroRecoveryId;
                entity.IsLateInterestShare = IsLateInterestShare;
                entity.IsExGratiaShare = IsExGratiaShare;
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
                var entity = Find(id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.PerLifeClaimData.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByPerLifeClaimId(int perLifeClaimId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.PerLifeClaimData.Where(q => q.PerLifeClaimId == perLifeClaimId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (PerLifeClaimData entity in query.ToList())
                {
                    DataTrail trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.PerLifeClaimData.Remove(entity);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
