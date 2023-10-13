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
    [Table("TreatyPricingRateTables")]
    public class TreatyPricingRateTable
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingRateTableGroupId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingRateTableGroup TreatyPricingRateTableGroup { get; set; }

        [Required, MaxLength(255), Index]
        public string Code { get; set; }

        [MaxLength(255), Index]
        public string Name { get; set; }

        [Required, Index]
        public int BenefitId { get; set; }
        [ExcludeTrail]
        public virtual Benefit Benefit { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        [Index]
        public int Status { get; set; }

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

        public TreatyPricingRateTable()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingRateTables.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateCode()
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingRateTables.Where(q => q.Code == Code);
                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public static TreatyPricingRateTable Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingRateTables.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingRateTables.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                TreatyPricingRateTable treatyPricingRateTable = Find(Id);
                if (treatyPricingRateTable == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyPricingRateTable, this);

                treatyPricingRateTable.TreatyPricingRateTableGroupId = TreatyPricingRateTableGroupId;
                treatyPricingRateTable.BenefitId = BenefitId;
                treatyPricingRateTable.Code = Code;
                treatyPricingRateTable.Name = Name;
                treatyPricingRateTable.Description = Description;
                treatyPricingRateTable.Status = Status;
                treatyPricingRateTable.UpdatedAt = DateTime.Now;
                treatyPricingRateTable.UpdatedById = UpdatedById ?? treatyPricingRateTable.UpdatedById;

                db.Entry(treatyPricingRateTable).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                TreatyPricingRateTable treatyPricingRateTable = db.TreatyPricingRateTables.Where(q => q.Id == id).FirstOrDefault();
                if (treatyPricingRateTable == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyPricingRateTable, true);

                db.Entry(treatyPricingRateTable).State = EntityState.Deleted;
                db.TreatyPricingRateTables.Remove(treatyPricingRateTable);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
