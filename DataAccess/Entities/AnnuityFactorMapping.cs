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
    [Table("AnnuityFactorMappings")]
    public class AnnuityFactorMapping
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int AnnuityFactorId { get; set; }

        [ExcludeTrail]
        public virtual AnnuityFactor AnnuityFactor { get; set; }

        [Required, MaxLength(255), Index]
        public string Combination { get; set; }

        [Required, MaxLength(30), Index]
        public string CedingPlanCode { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        public AnnuityFactorMapping()
        {
            CreatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.AnnuityFactorMappings.Any(q => q.Id == id);
            }
        }

        public static IQueryable<AnnuityFactorMapping> QueryByCombination(
            AppDbContext db,
            string combination,
            DateTime? reinsEffDatePolStartDate = null,
            DateTime? reinsEffDatePolEndDate = null,
            int? annuityFactorId = null
        )
        {
            db.Database.CommandTimeout = 0;

            EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("AnnuityFactorMapping");
            var query = connectionStrategy.Execute(() => db.AnnuityFactorMappings
                .Where(q => q.Combination.Trim() == combination.Trim()));

            if (reinsEffDatePolStartDate != null && reinsEffDatePolEndDate != null)
            {
                connectionStrategy.Reset();
                query = connectionStrategy.Execute(() => query
                    .Where(q =>
                    (
                        DbFunctions.TruncateTime(q.AnnuityFactor.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePolStartDate)
                        && DbFunctions.TruncateTime(q.AnnuityFactor.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePolStartDate)
                        ||
                        DbFunctions.TruncateTime(q.AnnuityFactor.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePolEndDate)
                        && DbFunctions.TruncateTime(q.AnnuityFactor.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePolEndDate)
                    )
                    || (q.AnnuityFactor.ReinsEffDatePolStartDate == null && q.AnnuityFactor.ReinsEffDatePolEndDate == null)
                ));
            }

            if (annuityFactorId != null)
            {
                connectionStrategy.Reset();
                query = connectionStrategy.Execute(() => query.Where(q => q.AnnuityFactorId != annuityFactorId));
            }
            return query;
        }

        public static IQueryable<AnnuityFactorMapping> QueryByParams(
            AppDbContext db,
            string cedingPlanCode,
            DateTime? reinsEffDatePol = null,
            bool groupById = false
        )
        {
            db.Database.CommandTimeout = 0;

            EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataRowProcessingInstance(104, "AnnuityFactorMapping");

            return connectionStrategy.Execute(() =>
            {
                var query = db.AnnuityFactorMappings.Where(q => (!string.IsNullOrEmpty(q.CedingPlanCode) && q.CedingPlanCode.Trim() == cedingPlanCode.Trim()) || q.CedingPlanCode == null);

                if (reinsEffDatePol != null)
                {
                    connectionStrategy.Reset(111);
                    query = query.Where(q =>
                            (
                                DbFunctions.TruncateTime(q.AnnuityFactor.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePol)
                                && DbFunctions.TruncateTime(q.AnnuityFactor.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePol)
                                ||
                                DbFunctions.TruncateTime(q.AnnuityFactor.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePol)
                                && DbFunctions.TruncateTime(q.AnnuityFactor.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePol)
                            )
                            || (q.AnnuityFactor.ReinsEffDatePolStartDate == null && q.AnnuityFactor.ReinsEffDatePolEndDate == null)
                        );
                }
                else
                {
                    connectionStrategy.Reset(124);
                    query = query.Where(q => q.AnnuityFactor.ReinsEffDatePolStartDate == null && q.AnnuityFactor.ReinsEffDatePolEndDate == null);
                }

                // NOTE: Group by should put at the end of query
                if (groupById)
                {
                    connectionStrategy.Reset(131);
                    query = query.GroupBy(q => q.AnnuityFactorId).Select(q => q.FirstOrDefault());
                }

                return query;

            });
        }

        public static AnnuityFactorMapping Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.AnnuityFactorMappings.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static AnnuityFactorMapping FindByCombination(
            string combination,
            DateTime? reinsEffDatePolStartDate = null,
            DateTime? reinsEffDatePolEndDate = null,
            int? annuityFactorId = null
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByCombination(
                        db,
                        combination,
                        reinsEffDatePolStartDate,
                        reinsEffDatePolEndDate,
                        annuityFactorId
                    ).FirstOrDefault();
            }
        }

        public static AnnuityFactorMapping FindByParams(
            string cedingPlanCode,
            DateTime? reinsEffDatePol = null,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByParams(
                    db,
                    cedingPlanCode,
                    reinsEffDatePol,
                    groupById
                ).FirstOrDefault();
            }
        }

        public static int CountByCombination(
            string combination,
            DateTime? reinsEffDatePolStartDate = null,
            DateTime? reinsEffDatePolEndDate = null,
            int? annuityFactorId = null
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByCombination(
                        db,
                        combination,
                        reinsEffDatePolStartDate,
                        reinsEffDatePolEndDate,
                        annuityFactorId
                    ).Count();
            }
        }

        public static int CountByParams(
            string cedingPlanCode,
            DateTime? reinsEffDatePol = null,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByParams(
                    db,
                    cedingPlanCode,
                    reinsEffDatePol,
                    groupById
                ).Count();
            }
        }

        public static IList<AnnuityFactorMapping> GetByParams(
            string cedingPlanCode,
            DateTime? reinsEffDatePol = null,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByParams(
                    db,
                    cedingPlanCode,
                    reinsEffDatePol,
                    groupById
                ).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.AnnuityFactorMappings.Add(this);
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
                entity.Combination = Combination;
                entity.CedingPlanCode = CedingPlanCode;

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
                db.AnnuityFactorMappings.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByAnnuityFactorId(int annuityFactorId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.AnnuityFactorMappings.Where(q => q.AnnuityFactorId == annuityFactorId);

                var trails = new List<DataTrail>();
                foreach (AnnuityFactorMapping annuityFactorMapping in query.ToList())
                {
                    var trail = new DataTrail(annuityFactorMapping, true);
                    trails.Add(trail);

                    db.Entry(annuityFactorMapping).State = EntityState.Deleted;
                    db.AnnuityFactorMappings.Remove(annuityFactorMapping);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
