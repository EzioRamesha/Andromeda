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
    [Table("Benefits")]
    public class Benefit
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(30), Index]
        public string Type { get; set; }

        [Required, MaxLength(10), Index]
        public string Code { get; set; }

        [Required, MaxLength(255), Index]
        public string Description { get; set; }

        [Required, Index]
        public int Status { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? EffectiveStartDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? EffectiveEndDate { get; set; }

        public int? ValuationBenefitCodePickListDetailId { get; set; }

        [ExcludeTrail]
        public virtual PickListDetail ValuationBenefitCodePickListDetail { get; set; }

        public int? BenefitCategoryPickListDetailId { get; set; }

        [ExcludeTrail]
        public virtual PickListDetail BenefitCategoryPickListDetail { get; set; }

        public bool GST { get; set; } = false;

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
        public virtual ICollection<BenefitDetail> BenefitDetails { get; set; }

        public Benefit()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Benefits.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateCode()
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(Code?.Trim()))
                {
                    var query = db.Benefits.Where(q => q.Code.Trim().Equals(Code.Trim(), StringComparison.OrdinalIgnoreCase));
                    if (Id != 0)
                    {
                        query = query.Where(q => q.Id != Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static Benefit Find(int id)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Benefit", 109);
                return connectionStrategy.Execute(() => db.Benefits.Where(q => q.Id == id).FirstOrDefault());
            }
        }

        public static Benefit FindByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return db.Benefits.Where(q => q.Code.Trim() == code.Trim()).FirstOrDefault();
            }
        }

        public static int CountByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return db.Benefits.Where(q => q.Code.Trim() == code.Trim()).Count();
            }
        }

        public static int CountByValuationBenefitCodePickListDetailId(int valuationBenefitCodePickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.Benefits.Where(q => q.ValuationBenefitCodePickListDetailId == valuationBenefitCodePickListDetailId).Count();
            }
        }

        public static IList<Benefit> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.Benefits.OrderBy(q => q.Code).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.Benefits.Add(this);
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
                entity.Type = Type;
                entity.Code = Code;
                entity.Description = Description;
                entity.Status = Status;
                entity.EffectiveStartDate = EffectiveStartDate;
                entity.EffectiveEndDate = EffectiveEndDate;
                entity.ValuationBenefitCodePickListDetailId = ValuationBenefitCodePickListDetailId;
                entity.BenefitCategoryPickListDetailId = BenefitCategoryPickListDetailId;
                entity.GST = GST;
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
                db.Benefits.Remove(entity);
                db.SaveChanges();
                return trail;
            }
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Description))
                return Code;
            return string.Format("{0} - {1}", Code, Description);
        }
    }
}
