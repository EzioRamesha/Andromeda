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

namespace DataAccess.Entities.TreatyPricing
{
    [Table("TreatyPricingPerLifeRetroProducts")]
    public  class TreatyPricingPerLifeRetroProduct
    {
        [Key, Column(Order = 0)]
        public int TreatyPricingPerLifeRetroId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingPerLifeRetro TreatyPricingPerLifeRetro { get; set; }

        [Key, Column(Order = 1)]
        public int TreatyPricingProductId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingProduct TreatyPricingProduct { get; set; }

        public string PrimaryKey()
        {
            return string.Format("{0}|{1}", TreatyPricingPerLifeRetroId, TreatyPricingProductId);
        }

        public static bool IsExists(int treatyPricingPerLifeRetroId, int treatyPricingProductId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingPerLifeRetroProducts
                    .Any(q => q.TreatyPricingPerLifeRetroId == treatyPricingPerLifeRetroId && q.TreatyPricingProductId == treatyPricingProductId);
            }
        }

        public static TreatyPricingPerLifeRetroProduct Find(int treatyPricingPerLifeRetroId, int treatyPricingProductId)
        {
            if (treatyPricingPerLifeRetroId == 0 || treatyPricingProductId == 0)
                return null;

            using (var db = new AppDbContext())
            {
                return db.TreatyPricingPerLifeRetroProducts
                    .Where(q => q.TreatyPricingPerLifeRetroId == treatyPricingPerLifeRetroId && q.TreatyPricingProductId == treatyPricingProductId)
                    .FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext(false))
            {
                DataTrail trail = new DataTrail(this);

                db.TreatyPricingPerLifeRetroProducts.Add(this);
                db.SaveChanges();

                return trail;
            }
        }

        public void Create(AppDbContext db)
        {
            db.TreatyPricingPerLifeRetroProducts.Add(this);
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingPerLifeRetroId(int treatyPricingPerLifeRetroId)
        {
            using (var db = new AppDbContext(false))
            {
                List<DataTrail> trails = new List<DataTrail>();

                db.Database.ExecuteSqlCommand("DELETE FROM [TreatyPricingPerLifeRetroProducts] WHERE [TreatyPricingPerLifeRetroId] = {0}", treatyPricingPerLifeRetroId);
                db.SaveChanges();

                return trails;
            }
        }

        public static void DeleteAllByTreatyPricingPerLifeRetroId(int treatyPricingPerLifeRetroId, ref TrailObject trail)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingPerLifeRetroProducts.Where(q => q.TreatyPricingPerLifeRetroId == treatyPricingPerLifeRetroId);

                foreach (TreatyPricingPerLifeRetroProduct TreatyPricingPerLifeRetroProduct in query.ToList())
                {
                    var dataTrail = new DataTrail(TreatyPricingPerLifeRetroProduct, true);
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingPerLifeRetro)), TreatyPricingPerLifeRetroProduct.PrimaryKey());

                    db.Entry(TreatyPricingPerLifeRetroProduct).State = EntityState.Deleted;
                    db.TreatyPricingPerLifeRetroProducts.Remove(TreatyPricingPerLifeRetroProduct);
                }

                db.SaveChanges();
            }
        }
    }
}
