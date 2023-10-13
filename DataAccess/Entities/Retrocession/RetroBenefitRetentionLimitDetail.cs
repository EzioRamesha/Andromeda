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
    [Table("RetroBenefitRetentionLimitDetails")]
    public class RetroBenefitRetentionLimitDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int RetroBenefitRetentionLimitId { get; set; }

        [ForeignKey(nameof(RetroBenefitRetentionLimitId))]
        [ExcludeTrail]
        public virtual RetroBenefitRetentionLimit RetroBenefitRetentionLimit { get; set; }

        [Required, Index]
        public int MinIssueAge { get; set; }

        [Required, Index]
        public int MaxIssueAge { get; set; }

        [Required, Index]
        public double MortalityLimitFrom { get; set; }

        [Required, Index]
        public double MortalityLimitTo { get; set; }

        [Required, Index]
        [Column(TypeName = "datetime2")]
        public DateTime ReinsEffStartDate { get; set; }

        [Required, Index]
        [Column(TypeName = "datetime2")]
        public DateTime ReinsEffEndDate { get; set; }

        [Required, Index]
        public double MlreRetentionAmount { get; set; }

        [Required, Index]
        public double MinReinsAmount { get; set; }

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

        public RetroBenefitRetentionLimitDetail()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroBenefitRetentionLimitDetails.Any(q => q.Id == id);
            }
        }

        public static RetroBenefitRetentionLimitDetail Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroBenefitRetentionLimitDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<RetroBenefitRetentionLimitDetail> GetByRetroBenefitRetentionLimitIdExcept(int retroBenefitRetentionLimitId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroBenefitRetentionLimitDetails.Where(q => q.RetroBenefitRetentionLimitId == retroBenefitRetentionLimitId && !ids.Contains(q.Id)).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RetroBenefitRetentionLimitDetails.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                RetroBenefitRetentionLimitDetail retroBenefitRetentionLimitDetail = Find(Id);
                if (retroBenefitRetentionLimitDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(retroBenefitRetentionLimitDetail, this);

                retroBenefitRetentionLimitDetail.RetroBenefitRetentionLimitId = RetroBenefitRetentionLimitId;
                retroBenefitRetentionLimitDetail.MinIssueAge = MinIssueAge;
                retroBenefitRetentionLimitDetail.MaxIssueAge = MaxIssueAge;
                retroBenefitRetentionLimitDetail.MortalityLimitFrom = MortalityLimitFrom;
                retroBenefitRetentionLimitDetail.MortalityLimitTo = MortalityLimitTo;
                retroBenefitRetentionLimitDetail.ReinsEffStartDate = ReinsEffStartDate;
                retroBenefitRetentionLimitDetail.ReinsEffEndDate = ReinsEffEndDate;
                retroBenefitRetentionLimitDetail.MlreRetentionAmount = MlreRetentionAmount;
                retroBenefitRetentionLimitDetail.MinReinsAmount = MinReinsAmount;
                retroBenefitRetentionLimitDetail.UpdatedAt = DateTime.Now;
                retroBenefitRetentionLimitDetail.UpdatedById = UpdatedById ?? retroBenefitRetentionLimitDetail.UpdatedById;

                db.Entry(retroBenefitRetentionLimitDetail).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                RetroBenefitRetentionLimitDetail retroBenefitRetentionLimitDetail = db.RetroBenefitRetentionLimitDetails.Where(q => q.Id == id).FirstOrDefault();
                if (retroBenefitRetentionLimitDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(retroBenefitRetentionLimitDetail, true);

                db.Entry(retroBenefitRetentionLimitDetail).State = EntityState.Deleted;
                db.RetroBenefitRetentionLimitDetails.Remove(retroBenefitRetentionLimitDetail);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByRetroBenefitRetentionLimitId(int retroBenefitRetentionLimitId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroBenefitRetentionLimitDetails.Where(q => q.RetroBenefitRetentionLimitId == retroBenefitRetentionLimitId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (RetroBenefitRetentionLimitDetail retroBenefitRetentionLimitDetail in query.ToList())
                {
                    DataTrail trail = new DataTrail(retroBenefitRetentionLimitDetail, true);
                    trails.Add(trail);

                    db.Entry(retroBenefitRetentionLimitDetail).State = EntityState.Deleted;
                    db.RetroBenefitRetentionLimitDetails.Remove(retroBenefitRetentionLimitDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
