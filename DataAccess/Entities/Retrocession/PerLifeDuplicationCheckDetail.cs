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
    [Table("PerLifeDuplicationCheckDetails")]
    public class PerLifeDuplicationCheckDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int PerLifeDuplicationCheckId { get; set; }

        [ForeignKey(nameof(PerLifeDuplicationCheckId))]
        [ExcludeTrail]
        public virtual PerLifeDuplicationCheck PerLifeDuplicationCheck { get; set; }

        [MaxLength(255), Index]
        public string TreatyCode { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        [ForeignKey(nameof(CreatedById))]
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        public PerLifeDuplicationCheckDetail()
        {
            CreatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeDuplicationCheckDetails.Any(q => q.Id == id);
            }
        }

        public static PerLifeDuplicationCheckDetail Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeDuplicationCheckDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PerLifeDuplicationCheckDetails.Add(this);
                db.SaveChanges();

                var trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var entity = PerLifeDuplicationCheckDetail.Find(Id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(entity, this);

                entity.PerLifeDuplicationCheckId = PerLifeDuplicationCheckId;
                entity.TreatyCode = TreatyCode;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                var entity = db.PerLifeDuplicationCheckDetails.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.PerLifeDuplicationCheckDetails.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByPerLifeDuplicationCheckId(int PerLifeDuplicationCheckId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.PerLifeDuplicationCheckDetails.Where(q => q.PerLifeDuplicationCheckId == PerLifeDuplicationCheckId);

                var trails = new List<DataTrail>();
                foreach (PerLifeDuplicationCheckDetail perLifeDuplicationCheckDetail in query.ToList())
                {
                    var trail = new DataTrail(perLifeDuplicationCheckDetail, true);
                    trails.Add(trail);

                    db.Entry(perLifeDuplicationCheckDetail).State = EntityState.Deleted;
                    db.PerLifeDuplicationCheckDetails.Remove(perLifeDuplicationCheckDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }

        public static int CountByTreatyCode(
            string treatyCode,
            DateTime? startDate,
            DateTime? endDate,
            int? duplicationCheckId = null)
        {
            using (var db = new AppDbContext())
            {
                return QueryByTreatyCode(
                    db,
                    treatyCode,
                    startDate,
                    endDate,
                    duplicationCheckId
                    ).Count();
            }
        }

        public static IQueryable<PerLifeDuplicationCheckDetail> QueryByTreatyCode(
            AppDbContext db,
            string treatyCode,
            DateTime? startDate,
            DateTime? endDate,
            int? perLifeDuplicationCheckId = null
        )
        {
            var query = db.PerLifeDuplicationCheckDetails
                .Where(q => string.Equals(q.TreatyCode, treatyCode));

            if (startDate != null && endDate != null)
            {
                query = query
                    .Where(q =>
                    (
                        DbFunctions.TruncateTime(q.PerLifeDuplicationCheck.ReinsuranceEffectiveStartDate) <= DbFunctions.TruncateTime(startDate)
                        && DbFunctions.TruncateTime(q.PerLifeDuplicationCheck.ReinsuranceEffectiveEndDate) >= DbFunctions.TruncateTime(startDate)
                        ||
                        DbFunctions.TruncateTime(q.PerLifeDuplicationCheck.ReinsuranceEffectiveStartDate) <= DbFunctions.TruncateTime(endDate)
                        && DbFunctions.TruncateTime(q.PerLifeDuplicationCheck.ReinsuranceEffectiveEndDate) >= DbFunctions.TruncateTime(endDate)
                    )
                    || (q.PerLifeDuplicationCheck.ReinsuranceEffectiveStartDate == null && q.PerLifeDuplicationCheck.ReinsuranceEffectiveEndDate == null)
                );
            }

            if (perLifeDuplicationCheckId != null)
            {
                query = query.Where(q => q.PerLifeDuplicationCheckId != perLifeDuplicationCheckId);
            }

            return query;
        }
    }
}
