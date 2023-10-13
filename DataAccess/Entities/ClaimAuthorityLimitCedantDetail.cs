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
    [Table("ClaimAuthorityLimitCedantDetails")]
    public class ClaimAuthorityLimitCedantDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int ClaimAuthorityLimitCedantId { get; set; }

        [ExcludeTrail]
        public virtual ClaimAuthorityLimitCedant ClaimAuthorityLimitCedant { get; set; }

        [Required, Index]
        public int ClaimCodeId { get; set; }

        [ExcludeTrail]
        public virtual ClaimCode ClaimCode { get; set; }

        [Required, Index]
        public int Type { get; set; }

        [Required]
        public double ClaimAuthorityLimitValue { get; set; }

        [Required]
        public int FundsAccountingTypePickListDetailId { get; set; }

        [ExcludeTrail]
        public virtual PickListDetail FundsAccountingTypePickListDetail { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime EffectiveDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        public ClaimAuthorityLimitCedantDetail()
        {
            CreatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimAuthorityLimitCedantDetails.Any(q => q.Id == id);
            }
        }

        public static ClaimAuthorityLimitCedantDetail Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimAuthorityLimitCedantDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static int CountByClaimAuthorityLimitCedantId(int calCedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimAuthorityLimitCedantDetails.Where(q => q.ClaimAuthorityLimitCedantId == calCedantId).Count();
            }
        }

        public static int CountByClaimCodeId(int claimCodeId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimAuthorityLimitCedantDetails.Where(q => q.ClaimCodeId == claimCodeId).Count();
            }
        }

        public static IList<ClaimAuthorityLimitCedantDetail> GetByClaimAuthorityLimitCedantId(int calCedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimAuthorityLimitCedantDetails.Where(q => q.ClaimAuthorityLimitCedantId == calCedantId).OrderBy(q => q.Id).ToList();
            }
        }

        public static IList<ClaimAuthorityLimitCedantDetail> GetByClaimAuthorityLimitCedantIdExcept(int calCedantId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimAuthorityLimitCedantDetails.Where(q => q.ClaimAuthorityLimitCedantId == calCedantId && !ids.Contains(q.Id)).ToList();
            }
        }

        public static IList<ClaimAuthorityLimitCedantDetail> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimAuthorityLimitCedantDetails.OrderBy(q => q.ClaimAuthorityLimitValue).ToList();
            }
        }

        public static List<string> GetDistinctClaimCodeForClaimAuthorityLimitCedant()
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimAuthorityLimitCedantDetails
                    .GroupBy(q => q.ClaimCodeId)
                    .Select(q => q.FirstOrDefault().ClaimCode.Code)
                    .ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ClaimAuthorityLimitCedantDetails.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                ClaimAuthorityLimitCedantDetail calCedantDetails = Find(Id);
                if (calCedantDetails == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(calCedantDetails, this);

                calCedantDetails.ClaimAuthorityLimitCedantId = ClaimAuthorityLimitCedantId;
                calCedantDetails.ClaimCodeId = ClaimCodeId;
                calCedantDetails.Type = Type;
                calCedantDetails.ClaimAuthorityLimitValue = ClaimAuthorityLimitValue;
                calCedantDetails.FundsAccountingTypePickListDetailId = FundsAccountingTypePickListDetailId;
                calCedantDetails.EffectiveDate = EffectiveDate;

                db.Entry(calCedantDetails).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                ClaimAuthorityLimitCedantDetail calCedantDetails = db.ClaimAuthorityLimitCedantDetails.Where(q => q.Id == id).FirstOrDefault();
                if (calCedantDetails == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(calCedantDetails, true);

                db.Entry(calCedantDetails).State = EntityState.Deleted;
                db.ClaimAuthorityLimitCedantDetails.Remove(calCedantDetails);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByClaimAuthorityLimitCedantId(int calCedantId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimAuthorityLimitCedantDetails.Where(q => q.ClaimAuthorityLimitCedantId == calCedantId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (ClaimAuthorityLimitCedantDetail calCedantDetail in query.ToList())
                {
                    DataTrail trail = new DataTrail(calCedantDetail, true);
                    trails.Add(trail);

                    db.Entry(calCedantDetail).State = EntityState.Deleted;
                    db.ClaimAuthorityLimitCedantDetails.Remove(calCedantDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
