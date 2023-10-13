using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Shared;
using Shared.Trails;
using Shared.Trails.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataAccess.Entities.Retrocession
{
    [Table("PerLifeAggregations")]
    public class PerLifeAggregation
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int FundsAccountingTypePickListDetailId { get; set; }

        [ForeignKey(nameof(FundsAccountingTypePickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail FundsAccountingTypePickListDetail { get; set; }

        [Required, Index]
        public int CutOffId { get; set; }

        [ExcludeTrail]
        public virtual CutOff CutOff { get; set; }

        [Required, MaxLength(64), Index]
        public string SoaQuarter { get; set; }

        [Index]
        [Column(TypeName = "datetime2")]
        public DateTime? ProcessingDate { get; set; }

        [Required, Index]
        public int Status { get; set; }

        [Index]
        public int? PersonInChargeId { get; set; }

        [ForeignKey(nameof(PersonInChargeId))]
        [ExcludeTrail]
        public virtual User PersonInCharge { get; set; }

        public string Errors { get; set; }

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

        public PerLifeAggregation()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.GetPerLifeAggregations().Any(q => q.Id == id);
            }
        }

        public bool IsDuplicate()
        {
            using (var db = new AppDbContext())
            {
                var query = db.GetPerLifeAggregations()
                    .Where(q => q.FundsAccountingTypePickListDetailId == FundsAccountingTypePickListDetailId)
                    .Where(q => q.SoaQuarter == SoaQuarter)
                    .Where(q => q.CutOffId == CutOffId);
                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public static PerLifeAggregation Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.GetPerLifeAggregations().Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PerLifeAggregations.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                PerLifeAggregation perLifeAggregation = Find(Id);
                if (perLifeAggregation == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeAggregation, this);

                perLifeAggregation.FundsAccountingTypePickListDetailId = FundsAccountingTypePickListDetailId;
                perLifeAggregation.CutOffId = CutOffId;
                perLifeAggregation.SoaQuarter = SoaQuarter;
                perLifeAggregation.ProcessingDate = ProcessingDate;
                perLifeAggregation.Status = Status;
                perLifeAggregation.PersonInChargeId = PersonInChargeId;
                perLifeAggregation.Errors = Errors;
                perLifeAggregation.UpdatedAt = DateTime.Now;
                perLifeAggregation.UpdatedById = UpdatedById ?? perLifeAggregation.UpdatedById;

                db.Entry(perLifeAggregation).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                PerLifeAggregation perLifeAggregation = db.PerLifeAggregations.Where(q => q.Id == id).FirstOrDefault();
                if (perLifeAggregation == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeAggregation, true);

                db.Entry(perLifeAggregation).State = EntityState.Deleted;
                db.PerLifeAggregations.Remove(perLifeAggregation);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
