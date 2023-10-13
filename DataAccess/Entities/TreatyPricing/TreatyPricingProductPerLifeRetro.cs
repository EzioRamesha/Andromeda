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
    [Table("TreatyPricingProductPerLifeRetros")]
    public class TreatyPricingProductPerLifeRetro
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingProductId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingProduct TreatyPricingProduct { get; set; }

        [Required, Index]
        public int TreatyPricingPerLifeRetroId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingPerLifeRetro TreatyPricingPerLifeRetro { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        public TreatyPricingProductPerLifeRetro()
        {
            CreatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductPerLifeRetros.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingProductPerLifeRetro Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductPerLifeRetros.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingProductPerLifeRetros.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
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
                db.TreatyPricingProductPerLifeRetros.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}