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

namespace DataAccess.Entities.TreatyPricing
{
    [Table("TreatyPricingProfitCommissions")]
    public class TreatyPricingProfitCommission
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingCedantId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingCedant TreatyPricingCedant { get; set; }

        [Required, MaxLength(255), Index]
        public string Code { get; set; }

        [MaxLength(255), Index]
        public string Name { get; set; }

        [Column(TypeName = "ntext")]
        public string BenefitCode { get; set; }

        [MaxLength(255), Index]
        public string Description { get; set; }

        [Index]
        public int Status { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? EffectiveDate { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? EndDate { get; set; }

        [Column(TypeName = "ntext")]
        public string Entitlement { get; set; }

        [Column(TypeName = "ntext")]
        public string Remark { get; set; }

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

        public TreatyPricingProfitCommission()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProfitCommissions.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateCode()
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingProfitCommissions.Where(q => q.Code == Code);
                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public static TreatyPricingProfitCommission Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProfitCommissions.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingProfitCommissions.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                TreatyPricingProfitCommission treatyPricingProfitCommission = Find(Id);
                if (treatyPricingProfitCommission == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyPricingProfitCommission, this);

                treatyPricingProfitCommission.TreatyPricingCedantId = TreatyPricingCedantId;
                treatyPricingProfitCommission.Code = Code;
                treatyPricingProfitCommission.Name = Name;
                treatyPricingProfitCommission.BenefitCode = BenefitCode;
                treatyPricingProfitCommission.Description = Description;
                treatyPricingProfitCommission.Status = Status;
                treatyPricingProfitCommission.EffectiveDate = EffectiveDate;
                treatyPricingProfitCommission.StartDate = StartDate;
                treatyPricingProfitCommission.EndDate = EndDate;
                treatyPricingProfitCommission.Entitlement = Entitlement;
                treatyPricingProfitCommission.Remark = Remark;
                treatyPricingProfitCommission.UpdatedAt = DateTime.Now;
                treatyPricingProfitCommission.UpdatedById = UpdatedById ?? treatyPricingProfitCommission.UpdatedById;

                db.Entry(treatyPricingProfitCommission).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                TreatyPricingProfitCommission treatyPricingProfitCommission = db.TreatyPricingProfitCommissions.Where(q => q.Id == id).FirstOrDefault();
                if (treatyPricingProfitCommission == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyPricingProfitCommission, true);

                db.Entry(treatyPricingProfitCommission).State = EntityState.Deleted;
                db.TreatyPricingProfitCommissions.Remove(treatyPricingProfitCommission);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
