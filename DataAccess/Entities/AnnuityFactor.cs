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
    [Table("AnnuityFactors")]
    public class AnnuityFactor
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int CedantId { get; set; }

        [ExcludeTrail]
        public virtual Cedant Cedant { get; set; }

        public string CedingPlanCode { get; set; }

        [Index]
        [Column(TypeName = "datetime2")]
        public DateTime? ReinsEffDatePolStartDate { get; set; }

        [Index]
        [Column(TypeName = "datetime2")]
        public DateTime? ReinsEffDatePolEndDate { get; set; }

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

        [ExcludeTrail]
        public virtual ICollection<AnnuityFactorMapping> AnnuityFactorMappings { get; set; }

        public AnnuityFactor()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.AnnuityFactors.Any(q => q.Id == id);
            }
        }

        public static AnnuityFactor Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.AnnuityFactors.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static AnnuityFactor Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.AnnuityFactors.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<AnnuityFactor> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.AnnuityFactors.ToList();
            }
        }

        public static int CountByCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.AnnuityFactors.Where(q => q.CedantId == cedantId).Count();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.AnnuityFactors.Add(this);
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

                entity.CedantId = CedantId;
                entity.CedingPlanCode = CedingPlanCode;
                entity.ReinsEffDatePolStartDate = ReinsEffDatePolStartDate;
                entity.ReinsEffDatePolEndDate = ReinsEffDatePolEndDate;
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
                db.AnnuityFactors.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
