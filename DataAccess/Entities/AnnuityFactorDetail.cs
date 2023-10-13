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

namespace DataAccess.Entities
{
    [Table("AnnuityFactorDetails")]
    public class AnnuityFactorDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int AnnuityFactorId { get; set; }

        [ExcludeTrail]
        public virtual AnnuityFactor AnnuityFactor { get; set; }

        [Index]
        public double? PolicyTermRemain { get; set; }

        [Index]
        public int? InsuredGenderCodePickListDetailId { get; set; }

        [ForeignKey(nameof(InsuredGenderCodePickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail InsuredGenderCodePickListDetail { get; set; }

        [Index]
        public int? InsuredTobaccoUsePickListDetailId { get; set; }

        [ForeignKey(nameof(InsuredTobaccoUsePickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail InsuredTobaccoUsePickListDetail { get; set; }

        [Index]
        public int? InsuredAttainedAge { get; set; }

        [Index]
        public double? PolicyTerm { get; set; }

        [Required, Index]
        public double AnnuityFactorValue { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        [ForeignKey(nameof(CreatedById))]
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        [ForeignKey(nameof(UpdatedById))]
        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }

        public AnnuityFactorDetail()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.AnnuityFactorDetails.Any(q => q.Id == id);
            }
        }

        public static AnnuityFactorDetail Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.AnnuityFactorDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static AnnuityFactorDetail Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.AnnuityFactorDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static int CountByAnnuityFactorIdByParams(
            int annuityFactorId,
            double? policyTermRemain = null,
            int? insuredGenderCodeId = null,
            int? insuredTobaccoUseId = null,
            int? insuredAttainedAge = null,
            double? policyTerm = null
        )
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataRowProcessingInstance(116, "AnnuityFactorDetail");
                var query = connectionStrategy.Execute(() => db.AnnuityFactorDetails.Where(q => q.AnnuityFactorId == annuityFactorId));

                if (policyTermRemain.HasValue)
                {
                    connectionStrategy.Reset(121);
                    query = connectionStrategy.Execute(() => query.Where(q => q.PolicyTermRemain == policyTermRemain || q.PolicyTermRemain == null));
                }
                else
                {
                    connectionStrategy.Reset(126);
                    query = connectionStrategy.Execute(() => query.Where(q => q.PolicyTermRemain == null));
                }

                if (insuredGenderCodeId.HasValue)
                {
                    connectionStrategy.Reset(131);
                    query = connectionStrategy.Execute(() => query.Where(q => q.InsuredGenderCodePickListDetailId == insuredGenderCodeId || q.InsuredGenderCodePickListDetailId == null));
                }
                else
                {
                    connectionStrategy.Reset(137);
                    query = connectionStrategy.Execute(() => query.Where(q => q.InsuredGenderCodePickListDetailId == null));
                }

                if (insuredTobaccoUseId.HasValue)
                {
                    connectionStrategy.Reset(142);
                    query = connectionStrategy.Execute(() => query.Where(q => q.InsuredTobaccoUsePickListDetailId == insuredTobaccoUseId || q.InsuredTobaccoUsePickListDetailId == null));
                }
                else
                {
                    connectionStrategy.Reset(148);
                    query = connectionStrategy.Execute(() => query.Where(q => q.InsuredTobaccoUsePickListDetailId == null));
                }

                if (insuredAttainedAge.HasValue)
                {
                    connectionStrategy.Reset(154);
                    query = connectionStrategy.Execute(() => query.Where(q => q.InsuredAttainedAge == insuredAttainedAge || q.InsuredAttainedAge == null));
                }
                else
                {
                    connectionStrategy.Reset(158);
                    query = connectionStrategy.Execute(() => query.Where(q => q.InsuredAttainedAge == null));
                }

                if (policyTerm.HasValue)
                {
                    connectionStrategy.Reset(165);
                    query = connectionStrategy.Execute(() => query.Where(q => q.PolicyTerm == policyTerm || q.PolicyTerm == null));
                }
                else
                {
                    connectionStrategy.Reset(170);
                    query = connectionStrategy.Execute(() => query.Where(q => q.PolicyTerm == null));
                }

                return query.Count();
            }
        }

        public static AnnuityFactorDetail FindByAnnuityFactorIdByParams(
            int annuityFactorId,
            double? policyTermRemain = null,
            int? insuredGenderCodeId = null,
            int? insuredTobaccoUseId = null,
            int? insuredAttainedAge = null,
            double? policyTerm = null
        )
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataRowProcessingInstance(191, "AnnuityFactorDetail");
                var query = db.AnnuityFactorDetails.Where(q => q.AnnuityFactorId == annuityFactorId);

                if (policyTermRemain.HasValue)
                {
                    connectionStrategy.Reset(196);
                    query = connectionStrategy.Execute(() => query.Where(q => q.PolicyTermRemain == policyTermRemain || q.PolicyTermRemain == null));
                }
                else
                {
                    connectionStrategy.Reset(202);
                    query = connectionStrategy.Execute(() => query.Where(q => q.PolicyTermRemain == null));
                }

                if (insuredGenderCodeId.HasValue)
                {
                    connectionStrategy.Reset(207);
                    query = connectionStrategy.Execute(() => query.Where(q => q.InsuredGenderCodePickListDetailId == insuredGenderCodeId || q.InsuredGenderCodePickListDetailId == null));
                }
                else
                {
                    connectionStrategy.Reset(212);
                    query = connectionStrategy.Execute(() => query.Where(q => q.InsuredGenderCodePickListDetailId == null));
                }

                if (insuredTobaccoUseId.HasValue)
                {
                    connectionStrategy.Reset(218);
                    query = connectionStrategy.Execute(() => query.Where(q => q.InsuredTobaccoUsePickListDetailId == insuredTobaccoUseId || q.InsuredTobaccoUsePickListDetailId == null));
                }
                else
                {
                    connectionStrategy.Reset(223);
                    query = connectionStrategy.Execute(() => query.Where(q => q.InsuredTobaccoUsePickListDetailId == null));
                }

                if (insuredAttainedAge.HasValue)
                {
                    connectionStrategy.Reset(229);
                    query = connectionStrategy.Execute(() => query.Where(q => q.InsuredAttainedAge == insuredAttainedAge || q.InsuredAttainedAge == null));
                }
                else
                {
                    connectionStrategy.Reset(233);
                    query = connectionStrategy.Execute(() => query.Where(q => q.InsuredAttainedAge == null));
                }

                if (policyTerm.HasValue)
                {
                    connectionStrategy.Reset(240);
                    query = connectionStrategy.Execute(() => query.Where(q => q.PolicyTerm == policyTerm || q.PolicyTerm == null));
                }
                else
                {
                    connectionStrategy.Reset(245);
                    query = connectionStrategy.Execute(() => query.Where(q => q.PolicyTerm == null));
                }

                return query.FirstOrDefault();
            }
        }

        public static IList<AnnuityFactorDetail> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.AnnuityFactorDetails.ToList();
            }
        }

        public static IList<AnnuityFactorDetail> GetByAnnuityFactorId(int annuityFactorId)
        {
            using (var db = new AppDbContext())
            {
                return db.AnnuityFactorDetails.Where(q => q.AnnuityFactorId == annuityFactorId).ToList();
            }
        }

        public static IList<AnnuityFactorDetail> GetByAnnuityFactorIdExcept(int annuityFactorId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.AnnuityFactorDetails.Where(q => q.AnnuityFactorId == annuityFactorId && !ids.Contains(q.Id)).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.AnnuityFactorDetails.Add(this);
                db.SaveChanges();

                return new DataTrail(this);
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(Id);
                if (entity == null)
                    throw new Exception(MessageBag.NoRecordFound);

                var trail = new DataTrail(entity, this);

                entity.AnnuityFactorId = AnnuityFactorId;
                entity.PolicyTermRemain = PolicyTermRemain;
                entity.InsuredGenderCodePickListDetailId = InsuredGenderCodePickListDetailId;
                entity.InsuredTobaccoUsePickListDetailId = InsuredTobaccoUsePickListDetailId;
                entity.InsuredAttainedAge = InsuredAttainedAge;
                entity.PolicyTerm = PolicyTerm;
                entity.AnnuityFactorValue = AnnuityFactorValue;
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
                    throw new Exception(MessageBag.NoRecordFound);

                var trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.AnnuityFactorDetails.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByAnnuityFactorId(int annuityFactorId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.AnnuityFactorDetails.Where(q => q.AnnuityFactorId == annuityFactorId);

                var trails = new List<DataTrail>();
                foreach (var entity in query.ToList())
                {
                    var trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.AnnuityFactorDetails.Remove(entity);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
