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
    [Table("TreatyPricingCedants")]
    public class TreatyPricingCedant
    {
        [Key]
        public int Id { get; set; }

        public int CedantId { get; set; }
        [ExcludeTrail]
        public virtual Cedant Cedant { get; set; }

        public int ReinsuranceTypePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail ReinsuranceTypePickListDetail { get; set; }

        [Required, MaxLength(30), Index]
        public string Code { get; set; }

        public int NoOfProduct { get; set; }

        public int NoOfDocument { get; set; }

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

        public TreatyPricingCedant()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingCedants.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicate()
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingCedants
                    .Where(q => q.CedantId == CedantId)
                    .Where(q => q.ReinsuranceTypePickListDetailId == ReinsuranceTypePickListDetailId);
                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public static TreatyPricingCedant Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingCedants.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static TreatyPricingCedant FindByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingCedants.Where(q => q.Code == code).FirstOrDefault();
            }
        }

        public static TreatyPricingCedant FindByCedantIdReinsuranceType(int cedantId, int reinsuranceTypePickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingCedants
                    .Where(q => q.CedantId == cedantId 
                        && q.ReinsuranceTypePickListDetailId == reinsuranceTypePickListDetailId)
                    .FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingCedants.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                TreatyPricingCedant treatyPricingCedant = Find(Id);
                if (treatyPricingCedant == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyPricingCedant, this);

                treatyPricingCedant.CedantId = CedantId;
                treatyPricingCedant.ReinsuranceTypePickListDetailId = ReinsuranceTypePickListDetailId;
                treatyPricingCedant.Code = Code;
                treatyPricingCedant.NoOfDocument = NoOfDocument;
                treatyPricingCedant.NoOfProduct = NoOfProduct;
                treatyPricingCedant.UpdatedAt = DateTime.Now;
                treatyPricingCedant.UpdatedById = UpdatedById ?? treatyPricingCedant.UpdatedById;

                db.Entry(treatyPricingCedant).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                TreatyPricingCedant treatyPricingCedant = db.TreatyPricingCedants.Where(q => q.Id == id).FirstOrDefault();
                if (treatyPricingCedant == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyPricingCedant, true);

                db.Entry(treatyPricingCedant).State = EntityState.Deleted;
                db.TreatyPricingCedants.Remove(treatyPricingCedant);
                db.SaveChanges();

                return trail;
            }
        }

        public override string ToString()
        {
            return Code;
        }
    }
}
