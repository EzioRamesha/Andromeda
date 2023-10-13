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
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities.TreatyPricing
{
    [Table("TreatyPricingProducts")]
    public class TreatyPricingProduct
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingCedantId { get; set; }

        [ExcludeTrail]
        public virtual TreatyPricingCedant TreatyPricingCedant { get; set; }

        [Required, MaxLength(60), Index]
        public string Code { get; set; }

        [MaxLength(255), Index]
        public string Name { get; set; }

        [Index]
        public DateTime? EffectiveDate { get; set; }

        [Column(TypeName = "ntext")]
        public string Summary { get; set; }

        [MaxLength(255), Index]
        public string QuotationName { get; set; }

        [Index]
        public bool HasPerLifeRetro { get; set; }

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

        public TreatyPricingProduct()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProducts.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingProduct Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProducts.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingProducts.Add(this);
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

                entity.TreatyPricingCedantId = TreatyPricingCedantId;
                entity.Code = Code;
                entity.Name = Name;
                entity.EffectiveDate = EffectiveDate;
                entity.Summary = Summary;
                entity.QuotationName = QuotationName;
                entity.HasPerLifeRetro = HasPerLifeRetro;
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
                var entity = db.TreatyPricingProducts.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.TreatyPricingProducts.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
