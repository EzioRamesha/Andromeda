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
    [Table("PerLifeClaims")]
    public class PerLifeClaim
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int CutOffId { get; set; }
        [ExcludeTrail]
        public virtual CutOff CutOff { get; set; }

        [Required, Index]
        public int FundsAccountingTypePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail FundsAccountingTypePickListDetail { get; set; }

        [Index]
        public int Status { get; set; }

        [MaxLength(30)]
        [Index]
        public string SoaQuarter { get; set; }

        [Index]
        public int? PersonInChargeId { get; set; }

        [Index]
        [Column(TypeName = "datetime2")]
        public DateTime? ProcessingDate { get; set; }

        [ForeignKey(nameof(PersonInChargeId))]
        [ExcludeTrail]
        public virtual User PersonInCharge { get; set; }

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

        public PerLifeClaim()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeClaims.Any(q => q.Id == id);
            }
        }

        public static PerLifeClaim Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeClaims.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PerLifeClaims.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
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

                DataTrail trail = new DataTrail(entity, this);

                entity.CutOffId = CutOffId;
                entity.FundsAccountingTypePickListDetailId = FundsAccountingTypePickListDetailId;
                entity.Status = Status;
                entity.SoaQuarter = SoaQuarter;
                entity.PersonInChargeId = PersonInChargeId;
                entity.ProcessingDate = ProcessingDate;
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
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.PerLifeClaims.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
