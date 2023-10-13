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

namespace DataAccess.Entities
{
    [Table("ClaimCodeMappingDetails")]
    public class ClaimCodeMappingDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int ClaimCodeMappingId { get; set; }

        [ForeignKey(nameof(ClaimCodeMappingId))]
        [ExcludeTrail]
        public virtual ClaimCodeMapping ClaimCodeMapping { get; set; }

        [Required, MaxLength(255), Index]
        public string Combination { get; set; }

        [MaxLength(10), Index]
        public string MlreEventCode { get; set; }

        [MaxLength(10), Index]
        public string MlreBenefitCode { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        [ForeignKey(nameof(CreatedById))]
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        public ClaimCodeMappingDetail()
        {
            CreatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimCodeMappingDetails.Any(q => q.Id == id);
            }
        }

        public static IQueryable<ClaimCodeMappingDetail> QueryByCombination(
            AppDbContext db,
            string combination,
            int? claimCodeMappingId = null
        )
        {
            var query = db.ClaimCodeMappingDetails
                .Where(q => q.Combination == combination);

            if (claimCodeMappingId != null)
            {
                query = query.Where(q => q.ClaimCodeMappingId != claimCodeMappingId);
            }
            return query;
        }

        public static IQueryable<ClaimCodeMappingDetail> QueryByParams(
            AppDbContext db,
            string mlreEventCode = null,
            string mlreBenefitCode = null,
            bool groupById = false
        )
        {
            var query = db.ClaimCodeMappingDetails
                .Where(q => q.MlreEventCode == mlreEventCode)
                .Where(q => q.MlreBenefitCode == mlreBenefitCode);

            // NOTE: Group by should put at the end of query
            if (groupById)
            {
                query = query.GroupBy(q => q.ClaimCodeMappingId).Select(q => q.FirstOrDefault());
            }

            return query;
        }

        public static ClaimCodeMappingDetail Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimCodeMappingDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static ClaimCodeMappingDetail FindByCombination(
            string combination,
            int? claimCodeMappingId = null
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByCombination(
                        db,
                        combination,
                        claimCodeMappingId
                    ).FirstOrDefault();
            }
        }

        public static ClaimCodeMappingDetail FindByParams(
            string mlreEventCode = null,
            string mlreBenefitCode = null,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByParams(
                    db,
                    mlreEventCode,
                    mlreBenefitCode,
                    groupById
                ).FirstOrDefault();
            }
        }

        public static int CountByEventCodeId(int eventCodeId)
        {
            using (var db = new AppDbContext())
            {
                EventCode eventCode = EventCode.Find(eventCodeId);
                if (eventCode != null)
                    return db.ClaimCodeMappingDetails.Where(q => q.MlreEventCode == eventCode.Code).Count();
                return 0;
            }
        }

        public static int CountByCombination(
            string combination,
            int? claimCodeMappingId = null
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByCombination(
                        db,
                        combination,
                        claimCodeMappingId
                    ).Count();
            }
        }

        public static int CountByParams(
            string mlreEventCode = null,
            string mlreBenefitCode = null,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByParams(
                    db,
                    mlreEventCode,
                    mlreBenefitCode,
                    groupById
                ).Count();
            }
        }

        public static IList<ClaimCodeMappingDetail> GetByParams(
            string mlreEventCode = null,
            string mlreBenefitCode = null,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByParams(
                    db,
                    mlreEventCode,
                    mlreBenefitCode,
                    groupById
                ).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ClaimCodeMappingDetails.Add(this);
                db.SaveChanges();

                var trail = new DataTrail(this);
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

                var trail = new DataTrail(entity, this);

                entity.ClaimCodeMappingId = ClaimCodeMappingId;
                entity.Combination = Combination;
                entity.MlreEventCode = MlreEventCode;
                entity.MlreBenefitCode = MlreBenefitCode;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                var entity = db.ClaimCodeMappingDetails.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.ClaimCodeMappingDetails.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByClaimCodeMappingId(int claimCodeMappingId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimCodeMappingDetails.Where(q => q.ClaimCodeMappingId == claimCodeMappingId);

                var trails = new List<DataTrail>();
                foreach (ClaimCodeMappingDetail claimCodeMappingDetail in query.ToList())
                {
                    var trail = new DataTrail(claimCodeMappingDetail, true);
                    trails.Add(trail);

                    db.Entry(claimCodeMappingDetail).State = EntityState.Deleted;
                    db.ClaimCodeMappingDetails.Remove(claimCodeMappingDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
