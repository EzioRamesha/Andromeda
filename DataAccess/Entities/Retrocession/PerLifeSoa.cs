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
    [Table("PerLifeSoa")]
    public class PerLifeSoa
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int RetroPartyId { get; set; }
        [ExcludeTrail]
        public virtual RetroParty RetroParty { get; set; }

        [Required, Index]
        public int RetroTreatyId { get; set; }
        [ExcludeTrail]
        public virtual RetroTreaty RetroTreaty { get; set; }

        [Index]
        public int Status { get; set; }

        [MaxLength(30)]
        [Index]
        public string SoaQuarter { get; set; }

        [Index]
        public int InvoiceStatus { get; set; }

        [Index]
        public int? PersonInChargeId { get; set; }
        [ExcludeTrail]
        public virtual User PersonInCharge { get; set; }

        [Index]
        [Column(TypeName = "datetime2")]
        public DateTime? ProcessingDate { get; set; }

        public bool IsProfitCommissionData { get; set; } = false;

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

        [Index]
        public int? PerLifeAggregationId { get; set; }
        [ExcludeTrail]
        public virtual PerLifeAggregation PerLifeAggregation { get; set; }

        public PerLifeSoa()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeSoa.Any(q => q.Id == id);
            }
        }

        public static PerLifeSoa Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeSoa.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PerLifeSoa.Add(this);
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

                entity.RetroPartyId = RetroPartyId;
                entity.RetroTreatyId = RetroTreatyId;
                entity.Status = Status;
                entity.SoaQuarter = SoaQuarter;
                entity.InvoiceStatus = InvoiceStatus;
                entity.PersonInChargeId = PersonInChargeId;
                entity.ProcessingDate = ProcessingDate;
                entity.IsProfitCommissionData = IsProfitCommissionData;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = UpdatedById ?? entity.UpdatedById;
                entity.PerLifeAggregationId = PerLifeAggregationId;

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
                db.PerLifeSoa.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
