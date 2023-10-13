using DataAccess.Entities.Identity;
using DataAccess.Entities.RiDatas;
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
    [Table("PerLifeAggregationDetailData")]
    public class PerLifeAggregationDetailData
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int PerLifeAggregationDetailTreatyId { get; set; }

        [ForeignKey(nameof(PerLifeAggregationDetailTreatyId))]
        [ExcludeTrail]
        public virtual PerLifeAggregationDetailTreaty PerLifeAggregationDetailTreaty { get; set; }

        [Required, Index]
        public int RiDataWarehouseHistoryId { get; set; }

        //[ForeignKey(nameof(RiDataWarehouseHistoryId))]
        [ExcludeTrail]
        public virtual RiDataWarehouseHistory RiDataWarehouseHistory { get; set; }

        // Follow PerLifeRetroGender
        [MaxLength(15)]
        public string ExpectedGenderCode { get; set; }

        [MaxLength(30)]
        public string RetroBenefitCode { get; set; }

        // Follow PerLifeRetroCountry
        [MaxLength(50)]
        public string ExpectedTerritoryOfIssueCode { get; set; }

        public int? FlagCode { get; set; }

        public int? ExceptionType { get; set; }

        public int? ExceptionErrorType { get; set; }

        [Index]
        public bool IsException { get; set; } = false;

        [Column(TypeName = "ntext")]
        public string Errors { get; set; }

        [Index]
        public int ProceedStatus { get; set; }

        [Index]
        public bool IsToAggregate { get; set; }

        [Column(TypeName = "ntext")]
        public string Remarks { get; set; }

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

        public PerLifeAggregationDetailData()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeAggregationDetailData.Any(q => q.Id == id);
            }
        }

        public static PerLifeAggregationDetailData Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeAggregationDetailData.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PerLifeAggregationDetailData.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                PerLifeAggregationDetailData perLifeAggregationDetailData = Find(Id);
                if (perLifeAggregationDetailData == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeAggregationDetailData, this);

                perLifeAggregationDetailData.PerLifeAggregationDetailTreatyId = PerLifeAggregationDetailTreatyId;
                perLifeAggregationDetailData.RiDataWarehouseHistoryId = RiDataWarehouseHistoryId;
                perLifeAggregationDetailData.ExpectedGenderCode = ExpectedGenderCode;
                perLifeAggregationDetailData.RetroBenefitCode = RetroBenefitCode;
                perLifeAggregationDetailData.ExpectedTerritoryOfIssueCode = ExpectedTerritoryOfIssueCode;
                perLifeAggregationDetailData.FlagCode = FlagCode;
                perLifeAggregationDetailData.ExceptionType = ExceptionType;
                perLifeAggregationDetailData.ExceptionErrorType = ExceptionErrorType;
                perLifeAggregationDetailData.IsException = IsException;
                perLifeAggregationDetailData.Errors = Errors;
                perLifeAggregationDetailData.ProceedStatus = ProceedStatus;
                perLifeAggregationDetailData.Remarks = Remarks;
                perLifeAggregationDetailData.UpdatedAt = DateTime.Now;
                perLifeAggregationDetailData.UpdatedById = UpdatedById ?? perLifeAggregationDetailData.UpdatedById;

                db.Entry(perLifeAggregationDetailData).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                PerLifeAggregationDetailData perLifeAggregationDetailData = db.PerLifeAggregationDetailData.Where(q => q.Id == id).FirstOrDefault();
                if (perLifeAggregationDetailData == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeAggregationDetailData, true);

                db.Entry(perLifeAggregationDetailData).State = EntityState.Deleted;
                db.PerLifeAggregationDetailData.Remove(perLifeAggregationDetailData);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByPerLifeAggregationDetailTreatyId(int perLifeAggregationDetailTreatyId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.PerLifeAggregationDetailData.Where(q => q.PerLifeAggregationDetailTreatyId == perLifeAggregationDetailTreatyId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (PerLifeAggregationDetailData perLifeAggregationDetailData in query.ToList())
                {
                    DataTrail trail = new DataTrail(perLifeAggregationDetailData, true);
                    trails.Add(trail);

                    db.Entry(perLifeAggregationDetailData).State = EntityState.Deleted;
                    db.PerLifeAggregationDetailData.Remove(perLifeAggregationDetailData);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
