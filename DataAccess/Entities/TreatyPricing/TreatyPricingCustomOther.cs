using BusinessObject.TreatyPricing;
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
    [Table("TreatyPricingCustomerOthers")]
    public class TreatyPricingCustomOther
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingCedantId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingCedant TreatyPricingCedant { get; set; }

        [Required, MaxLength(255), Index]
        public string Code { get; set; }

        [Index]
        public int Status { get; set; }

        [MaxLength(255), Index]
        public string Name { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "ntext")]
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

        public TreatyPricingCustomOther()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingCustomOthers.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateCode()
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingCustomOthers.Where(q => q.Code == Code);
                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public static TreatyPricingCustomOther Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingCustomOthers.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingCustomOthers.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                TreatyPricingCustomOther treatyPricingCustomOther = Find(Id);
                if (treatyPricingCustomOther == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyPricingCustomOther, this);

                treatyPricingCustomOther.TreatyPricingCedantId = TreatyPricingCedantId;
                treatyPricingCustomOther.Code = Code;
                treatyPricingCustomOther.Status = Status;
                treatyPricingCustomOther.Name = Name;
                treatyPricingCustomOther.Description = Description;
                treatyPricingCustomOther.UpdatedAt = DateTime.Now;
                treatyPricingCustomOther.UpdatedById = UpdatedById ?? treatyPricingCustomOther.UpdatedById;

                db.Entry(treatyPricingCustomOther).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                TreatyPricingCustomOther treatyPricingCustomOther = db.TreatyPricingCustomOthers.Where(q => q.Id == id).FirstOrDefault();
                if (treatyPricingCustomOther == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyPricingCustomOther, true);

                db.Entry(treatyPricingCustomOther).State = EntityState.Deleted;
                db.TreatyPricingCustomOthers.Remove(treatyPricingCustomOther);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
