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
    [Table("BenefitDetails")]
    public class BenefitDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int BenefitId { get; set; }

        [ExcludeTrail]
        public virtual Benefit Benefit { get; set; }

        [Required, Index]
        public int ClaimCodeId { get; set; }

        [ExcludeTrail]
        public virtual ClaimCode ClaimCode { get; set; }

        [Required, Index]
        public int EventCodeId { get; set; }

        [ExcludeTrail]
        public virtual EventCode EventCode { get; set; }

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

        public BenefitDetail()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.BenefitDetails.Any(q => q.Id == id);
            }
        }

        public static BenefitDetail Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.BenefitDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<BenefitDetail> GetByBenefitId(int benefitId)
        {
            using (var db = new AppDbContext())
            {
                return db.BenefitDetails.Where(q => q.BenefitId == benefitId).OrderBy(q => q.Id).ToList();
            }
        }

        public static IList<BenefitDetail> GetByBenefitIdExcept(int benefitId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.BenefitDetails.Where(q => q.BenefitId == benefitId && !ids.Contains(q.Id)).ToList();
            }
        }

        public static int CountByClaimCode(int claimCodeId)
        {
            using (var db = new AppDbContext())
            {
                return db.BenefitDetails.Where(q => q.ClaimCodeId == claimCodeId).Count();
            }
        }

        public static int CountByEventCode(int eventCodeId)
        {
            using (var db = new AppDbContext())
            {
                return db.BenefitDetails.Where(q => q.EventCodeId == eventCodeId).Count();
            }
        }

        public static IQueryable<BenefitDetail> QueryByParams(
            AppDbContext db,
            string mlreEventCode = null,
            string mlreBenefitCode = null
        )
        {
            var query = db.BenefitDetails
                .Where(q => q.EventCode.Code == mlreEventCode)
                .Where(q => q.Benefit.Code == mlreBenefitCode);

            return query;
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.BenefitDetails.Add(this);
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

                entity.BenefitId = BenefitId;
                entity.EventCodeId = EventCodeId;
                entity.ClaimCodeId = ClaimCodeId;
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
                db.BenefitDetails.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByBenefitId(int benefitId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.BenefitDetails.Where(q => q.BenefitId == benefitId);

                var trails = new List<DataTrail>();
                foreach (var benefitDetail in query.ToList())
                {
                    var trail = new DataTrail(benefitDetail, true);
                    trails.Add(trail);

                    db.Entry(benefitDetail).State = EntityState.Deleted;
                    db.BenefitDetails.Remove(benefitDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}