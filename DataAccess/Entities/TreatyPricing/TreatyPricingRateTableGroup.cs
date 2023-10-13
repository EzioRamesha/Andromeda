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
    [Table("TreatyPricingRateTableGroups")]
    public class TreatyPricingRateTableGroup
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

        [MaxLength(255), Index]
        public string Description { get; set; }

        [MaxLength(255), Index]
        public string FileName { get; set; }

        [MaxLength(255), Index]
        public string HashFileName { get; set; }

        [Index]
        public int NoOfRateTable { get; set; }

        [Index]
        public int Status { get; set; }

        [Column(TypeName = "ntext")]
        public string Errors { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UploadedAt { get; set; }

        public int UploadedById { get; set; }
        [ExcludeTrail]
        public virtual User UploadedBy { get; set; }

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

        public TreatyPricingRateTableGroup()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingRateTableGroups.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateCode()
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingRateTableGroups.Where(q => q.Code == Code);
                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public static TreatyPricingRateTableGroup Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingRateTableGroups.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingRateTableGroups.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                TreatyPricingRateTableGroup treatyPricingRateTableGroup = Find(Id);
                if (treatyPricingRateTableGroup == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyPricingRateTableGroup, this);

                treatyPricingRateTableGroup.TreatyPricingCedantId = TreatyPricingCedantId;
                treatyPricingRateTableGroup.Code = Code;
                treatyPricingRateTableGroup.Name = Name;
                treatyPricingRateTableGroup.Description = Description;
                treatyPricingRateTableGroup.FileName = FileName;
                treatyPricingRateTableGroup.HashFileName = HashFileName;
                treatyPricingRateTableGroup.NoOfRateTable = NoOfRateTable;
                treatyPricingRateTableGroup.Status = Status;
                treatyPricingRateTableGroup.Errors = Errors;
                treatyPricingRateTableGroup.UploadedAt = UploadedAt;
                treatyPricingRateTableGroup.UploadedById = UploadedById;
                treatyPricingRateTableGroup.UpdatedAt = DateTime.Now;
                treatyPricingRateTableGroup.UpdatedById = UpdatedById ?? treatyPricingRateTableGroup.UpdatedById;

                db.Entry(treatyPricingRateTableGroup).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                TreatyPricingRateTableGroup treatyPricingRateTableGroup = db.TreatyPricingRateTableGroups.Where(q => q.Id == id).FirstOrDefault();
                if (treatyPricingRateTableGroup == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyPricingRateTableGroup, true);

                db.Entry(treatyPricingRateTableGroup).State = EntityState.Deleted;
                db.TreatyPricingRateTableGroups.Remove(treatyPricingRateTableGroup);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
