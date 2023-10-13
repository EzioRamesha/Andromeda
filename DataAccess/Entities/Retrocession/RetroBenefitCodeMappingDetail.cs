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
    [Table("RetroBenefitCodeMappingDetails")]
    public class RetroBenefitCodeMappingDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int RetroBenefitCodeMappingId { get; set; }

        [ForeignKey(nameof(RetroBenefitCodeMappingId))]
        [ExcludeTrail]
        public virtual RetroBenefitCodeMapping RetroBenefitCodeMapping { get; set; }

        [Required, Index]
        public int RetroBenefitCodeId { get; set; }

        [ForeignKey(nameof(RetroBenefitCodeId))]
        [ExcludeTrail]
        public virtual RetroBenefitCode RetroBenefitCode { get; set; }

        [Index]
        public bool IsComputePremium { get; set; } = false;

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

        public RetroBenefitCodeMappingDetail()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroBenefitCodeMappingDetails.Any(q => q.Id == id);
            }
        }

        public static RetroBenefitCodeMappingDetail Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroBenefitCodeMappingDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<RetroBenefitCodeMappingDetail> GetByRetroBenefitCodeMappingIdExcept(int retroBenefitCodeMappingId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroBenefitCodeMappingDetails.Where(q => q.RetroBenefitCodeMappingId == retroBenefitCodeMappingId && !ids.Contains(q.Id)).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RetroBenefitCodeMappingDetails.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                RetroBenefitCodeMappingDetail retroBenefitCodeMappingDetail = Find(Id);
                if (retroBenefitCodeMappingDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(retroBenefitCodeMappingDetail, this);

                retroBenefitCodeMappingDetail.RetroBenefitCodeMappingId = RetroBenefitCodeMappingId;
                retroBenefitCodeMappingDetail.RetroBenefitCodeId = RetroBenefitCodeId;
                retroBenefitCodeMappingDetail.IsComputePremium = IsComputePremium;
                retroBenefitCodeMappingDetail.UpdatedAt = DateTime.Now;
                retroBenefitCodeMappingDetail.UpdatedById = UpdatedById ?? retroBenefitCodeMappingDetail.UpdatedById;

                db.Entry(retroBenefitCodeMappingDetail).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                RetroBenefitCodeMappingDetail retroBenefitCodeMappingDetail = db.RetroBenefitCodeMappingDetails.Where(q => q.Id == id).FirstOrDefault();
                if (retroBenefitCodeMappingDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(retroBenefitCodeMappingDetail, true);

                db.Entry(retroBenefitCodeMappingDetail).State = EntityState.Deleted;
                db.RetroBenefitCodeMappingDetails.Remove(retroBenefitCodeMappingDetail);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByRetroBenefitCodeMappingId(int retroBenefitCodeMappingId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroBenefitCodeMappingDetails.Where(q => q.RetroBenefitCodeMappingId == retroBenefitCodeMappingId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (RetroBenefitCodeMappingDetail retroBenefitCodeMappingDetail in query.ToList())
                {
                    DataTrail trail = new DataTrail(retroBenefitCodeMappingDetail, true);
                    trails.Add(trail);

                    db.Entry(retroBenefitCodeMappingDetail).State = EntityState.Deleted;
                    db.RetroBenefitCodeMappingDetails.Remove(retroBenefitCodeMappingDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
