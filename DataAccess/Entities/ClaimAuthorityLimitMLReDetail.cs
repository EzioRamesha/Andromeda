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
    [Table("ClaimAuthorityLimitMLReDetails")]
    public class ClaimAuthorityLimitMLReDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int ClaimAuthorityLimitMLReId { get; set; }

        [ExcludeTrail]
        public virtual ClaimAuthorityLimitMLRe ClaimAuthorityLimitMLRe { get; set; }

        [Required, Index]
        public int ClaimCodeId { get; set; }

        [ExcludeTrail]
        public virtual ClaimCode ClaimCode { get; set; }

        [Required]
        public double ClaimAuthorityLimitValue { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        public ClaimAuthorityLimitMLReDetail()
        {
            CreatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimAuthorityLimitMLReDetails.Any(q => q.Id == id);
            }
        }

        public static ClaimAuthorityLimitMLReDetail Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimAuthorityLimitMLReDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static int CountByClaimAuthorityLimitMLReId(int calMLReId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimAuthorityLimitMLReDetails.Where(q => q.ClaimAuthorityLimitMLReId == calMLReId).Count();
            }
        }

        public static int CountByClaimCodeId(int claimCodeId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimAuthorityLimitMLReDetails.Where(q => q.ClaimCodeId == claimCodeId).Count();
            }
        }

        public static IList<ClaimAuthorityLimitMLReDetail> GetByClaimAuthorityLimitMLReId(int calMLReId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimAuthorityLimitMLReDetails.Where(q => q.ClaimAuthorityLimitMLReId == calMLReId).OrderBy(q => q.Id).ToList();
            }
        }

        public static IList<ClaimAuthorityLimitMLReDetail> GetByClaimAuthorityLimitMLReIdExcept(int calMLReId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimAuthorityLimitMLReDetails.Where(q => q.ClaimAuthorityLimitMLReId == calMLReId && !ids.Contains(q.Id)).ToList();
            }
        }

        public static IList<ClaimAuthorityLimitMLReDetail> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimAuthorityLimitMLReDetails.OrderBy(q => q.ClaimAuthorityLimitValue).ToList();
            }
        }

        public static List<string> GetDistinctClaimCodeForClaimAuthorityLimitMLRe()
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimAuthorityLimitMLReDetails
                    .GroupBy(q => q.ClaimCodeId)
                    .Select(q => q.FirstOrDefault().ClaimCode.Code)
                    .ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ClaimAuthorityLimitMLReDetails.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                ClaimAuthorityLimitMLReDetail calMLReDetails = Find(Id);
                if (calMLReDetails == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(calMLReDetails, this);

                calMLReDetails.ClaimAuthorityLimitMLReId = ClaimAuthorityLimitMLReId;
                calMLReDetails.ClaimCodeId = ClaimCodeId;
                calMLReDetails.ClaimAuthorityLimitValue = ClaimAuthorityLimitValue;

                db.Entry(calMLReDetails).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                ClaimAuthorityLimitMLReDetail calMLReDetails = db.ClaimAuthorityLimitMLReDetails.Where(q => q.Id == id).FirstOrDefault();
                if (calMLReDetails == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(calMLReDetails, true);

                db.Entry(calMLReDetails).State = EntityState.Deleted;
                db.ClaimAuthorityLimitMLReDetails.Remove(calMLReDetails);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByClaimAuthorityLimitMLReId(int calMLReId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimAuthorityLimitMLReDetails.Where(q => q.ClaimAuthorityLimitMLReId == calMLReId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (ClaimAuthorityLimitMLReDetail calMLReDetail in query.ToList())
                {
                    DataTrail trail = new DataTrail(calMLReDetail, true);
                    trails.Add(trail);

                    db.Entry(calMLReDetail).State = EntityState.Deleted;
                    db.ClaimAuthorityLimitMLReDetails.Remove(calMLReDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
