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
    [Table("RateDetails")]
    public class RateDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int RateId { get; set; }

        [ForeignKey(nameof(RateId))]
        [ExcludeTrail]
        public virtual Rate Rate { get; set; }

        [Index]
        public int? InsuredGenderCodePickListDetailId { get; set; }

        [ForeignKey(nameof(InsuredGenderCodePickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail InsuredGenderCodePickListDetail { get; set; }

        [Index]
        public int? CedingTobaccoUsePickListDetailId { get; set; }

        [ForeignKey(nameof(CedingTobaccoUsePickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail CedingTobaccoUsePickListDetail { get; set; }

        [Index]
        public int? CedingOccupationCodePickListDetailId { get; set; }

        [ForeignKey(nameof(CedingOccupationCodePickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail CedingOccupationCodePickListDetail { get; set; }

        [Index]
        public int? AttainedAge { get; set; }

        [Index]
        public int? IssueAge { get; set; }

        [Index]
        public double? PolicyTerm { get; set; }

        [Index]
        public double? PolicyTermRemain { get; set; }

        public double RateValue { get; set; }

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

        public RateDetail()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RateDetails.Any(q => q.Id == id);
            }
        }

        public static RateDetail Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RateDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static int CountByParams(
            int rateId,
            int? genderCodeId = null,
            int? tobaccoUseId = null,
            int? occupationCodeId = null,
            int? attainedAge = null,
            int? issueAge = null,
            double? policyTerm = null,
            double? policyTermRemain = null
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByRateIdByParams(
                    db,
                    rateId,
                    genderCodeId,
                    tobaccoUseId,
                    occupationCodeId,
                    attainedAge,
                    issueAge,
                    policyTerm,
                    policyTermRemain
                ).Count();
            }
        }

        public static RateDetail FindByParams(
            int rateId,
            int? genderCodeId = null,
            int? tobaccoUseId = null,
            int? occupationCodeId = null,
            int? attainedAge = null,
            int? issueAge = null,
            double? policyTerm = null,
            double? policyTermRemain = null
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByRateIdByParams(
                    db,
                    rateId,
                    genderCodeId,
                    tobaccoUseId,
                    occupationCodeId,
                    attainedAge,
                    issueAge,
                    policyTerm,
                    policyTermRemain
                ).FirstOrDefault();
            }
        }

        public static IQueryable<RateDetail> QueryByRateIdByParams(
            AppDbContext db,
            int rateId,
            int? genderCodeId = null,
            int? tobaccoUseId = null,
            int? occupationCodeId = null,
            int? attainedAge = null,
            int? issueAge = null,
            double? policyTerm = null,
            double? policyTermRemain = null
        )
        {
            db.Database.CommandTimeout = 0;

            EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("RateDetail");
            var query = connectionStrategy.Execute(() => db.RateDetails.Where(q => q.RateId == rateId));

            if (genderCodeId.HasValue)
            {
                connectionStrategy.Reset();
                query = connectionStrategy.Execute(() => query.Where(q => q.InsuredGenderCodePickListDetailId == genderCodeId.Value || q.InsuredGenderCodePickListDetailId == null));
            }
            else
            {
                connectionStrategy.Reset();
                query = connectionStrategy.Execute(() => query.Where(q => q.InsuredGenderCodePickListDetailId == null));
            }

            if (tobaccoUseId.HasValue)
            {
                connectionStrategy.Reset();
                query = connectionStrategy.Execute(() => query.Where(q => q.CedingTobaccoUsePickListDetailId == tobaccoUseId.Value || q.CedingTobaccoUsePickListDetailId == null));
            }
            else
            {
                connectionStrategy.Reset();
                query = connectionStrategy.Execute(() => query.Where(q => q.CedingTobaccoUsePickListDetailId == null));
            }

            if (occupationCodeId.HasValue)
            {
                connectionStrategy.Reset();
                query = connectionStrategy.Execute(() => query.Where(q => q.CedingOccupationCodePickListDetailId == occupationCodeId.Value || q.CedingOccupationCodePickListDetailId == null));
            }
            else
            {
                connectionStrategy.Reset();
                query = connectionStrategy.Execute(() => query.Where(q => q.CedingOccupationCodePickListDetailId == null));
            }

            if (attainedAge.HasValue)
            {
                connectionStrategy.Reset();
                query = connectionStrategy.Execute(() => query.Where(q => q.AttainedAge == attainedAge.Value || q.AttainedAge == null));
            }
            else
            {
                connectionStrategy.Reset();
                query = connectionStrategy.Execute(() => query.Where(q => q.AttainedAge == null));
            }

            if (issueAge.HasValue)
            {
                connectionStrategy.Reset();
                query = connectionStrategy.Execute(() => query.Where(q => q.IssueAge == issueAge.Value || q.IssueAge == null));
            }
            else
            {
                connectionStrategy.Reset();
                query = connectionStrategy.Execute(() => query.Where(q => q.IssueAge == null));
            }

            if (policyTerm.HasValue)
            {
                connectionStrategy.Reset();
                query = connectionStrategy.Execute(() => query.Where(q => q.PolicyTerm == policyTerm.Value || q.PolicyTerm == null));
            }
            else
            {
                connectionStrategy.Reset();
                query = connectionStrategy.Execute(() => query.Where(q => q.PolicyTerm == null));
            }

            if (policyTermRemain.HasValue)
            {
                connectionStrategy.Reset();
                query = connectionStrategy.Execute(() => query.Where(q => q.PolicyTermRemain == policyTermRemain.Value || q.PolicyTermRemain == null));
            }
            else
            {
                connectionStrategy.Reset();
                query = connectionStrategy.Execute(() => query.Where(q => q.PolicyTermRemain == null));
            }

            return query;
        }

        public static IList<RateDetail> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.RateDetails.ToList();
            }
        }

        public static IList<RateDetail> GetByRateId(int rateId)
        {
            using (var db = new AppDbContext())
            {
                return db.RateDetails.Where(q => q.RateId == rateId).ToList();
            }
        }

        public static IList<RateDetail> GetNextByRateId(int rateId, int skip, int take)
        {
            using (var db = new AppDbContext())
            {
                return db.RateDetails.Where(q => q.RateId == rateId).OrderBy(q => q.Id).Skip(skip).Take(take).ToList();
            }
        }

        public static IList<RateDetail> GetByRateIdExcept(int rateId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.RateDetails.Where(q => q.RateId == rateId && !ids.Contains(q.Id)).ToList();
            }
        }

        public static int CountByInsuredGenderCodePickListDetailId(int insuredGenderCodePickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.RateDetails.Where(q => q.InsuredGenderCodePickListDetailId == insuredGenderCodePickListDetailId).Count();
            }
        }

        public static int CountByCedingTobaccoUsePickListDetailId(int cedingTobaccoUsePickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.RateDetails.Where(q => q.CedingTobaccoUsePickListDetailId == cedingTobaccoUsePickListDetailId).Count();
            }
        }

        public static int CountByCedingOccupationCodePickListDetailId(int cedingOccupationCodePickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.RateDetails.Where(q => q.CedingOccupationCodePickListDetailId == cedingOccupationCodePickListDetailId).Count();
            }
        }

        public static int CountByRateId(int rateId)
        {
            using (var db = new AppDbContext())
            {
                return db.RateDetails.Where(q => q.RateId == rateId).Count();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RateDetails.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                RateDetail rateDetail = RateDetail.Find(Id);
                if (rateDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(rateDetail, this);

                rateDetail.RateId = RateId;
                rateDetail.InsuredGenderCodePickListDetailId = InsuredGenderCodePickListDetailId;
                rateDetail.CedingTobaccoUsePickListDetailId = CedingTobaccoUsePickListDetailId;
                rateDetail.CedingOccupationCodePickListDetailId = CedingOccupationCodePickListDetailId;
                rateDetail.AttainedAge = AttainedAge;
                rateDetail.IssueAge = IssueAge;
                rateDetail.PolicyTerm = PolicyTerm;
                rateDetail.PolicyTermRemain = PolicyTermRemain;
                rateDetail.RateValue = RateValue;
                rateDetail.UpdatedAt = DateTime.Now;
                rateDetail.UpdatedById = UpdatedById ?? rateDetail.UpdatedById;

                db.Entry(rateDetail).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                RateDetail rateDetail = db.RateDetails.Where(q => q.Id == id).FirstOrDefault();
                if (rateDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(rateDetail, true);

                db.Entry(rateDetail).State = EntityState.Deleted;
                db.RateDetails.Remove(rateDetail);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByRateId(int rateId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RateDetails.Where(q => q.RateId == rateId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (RateDetail rateDetail in query.ToList())
                {
                    DataTrail trail = new DataTrail(rateDetail, true);
                    trails.Add(trail);

                    db.Entry(rateDetail).State = EntityState.Deleted;
                    db.RateDetails.Remove(rateDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
