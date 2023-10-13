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
    [Table("TreatyPricingCustomOtherProducts")]
    public class TreatyPricingCustomOtherProduct
    {
        [Key, Column(Order = 0)]
        public int TreatyPricingCustomOtherId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingCustomOther TreatyPricingCustomOther { get; set; }

        [Key, Column(Order = 1)]
        public int TreatyPricingProductId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingProduct TreatyPricingProduct { get; set; }

        public string PrimaryKey()
        {
            return string.Format("{0}|{1}", TreatyPricingCustomOtherId, TreatyPricingProductId);
        }

        public static bool IsExists(int treatyPricingCustomOtherId, int treatyPricingProductId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingCustomOtherProducts
                    .Any(q => q.TreatyPricingCustomOtherId == treatyPricingCustomOtherId && q.TreatyPricingProductId == treatyPricingProductId);
            }
        }

        public static TreatyPricingCustomOtherProduct Find(int treatyPricingCustomOtherId, int treatyPricingProductId)
        {
            if (treatyPricingCustomOtherId == 0 || treatyPricingProductId == 0)
                return null;

            using (var db = new AppDbContext())
            {
                return db.TreatyPricingCustomOtherProducts
                    .Where(q => q.TreatyPricingCustomOtherId == treatyPricingCustomOtherId && q.TreatyPricingProductId == treatyPricingProductId)
                    .FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext(false))
            {
                DataTrail trail = new DataTrail(this);

                db.TreatyPricingCustomOtherProducts.Add(this);
                db.SaveChanges();

                return trail;
            }
        }

        public void Create(AppDbContext db)
        {
            db.TreatyPricingCustomOtherProducts.Add(this);
        }

        public static DataTrail Delete(int treatyPricingCustomOtherId, int treatyPricingProductId)
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(treatyPricingCustomOtherId, treatyPricingProductId);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.TreatyPricingCustomOtherProducts.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingCustomOtherId(int treatyPricingCustomOtherId)
        {
            using (var db = new AppDbContext(false))
            {
                List<DataTrail> trails = new List<DataTrail>();

                db.Database.ExecuteSqlCommand("DELETE FROM [TreatyPricingCustomOtherProducts] WHERE [TreatyPricingCustomOtherId] = {0}", treatyPricingCustomOtherId);
                db.SaveChanges();

                return trails;
            }
        }

        public static void DeleteAllByTreatyPricingCustomOtherId(int treatyPricingCustomOtherId, ref TrailObject trail)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingCustomOtherProducts.Where(q => q.TreatyPricingCustomOtherId == treatyPricingCustomOtherId);

                foreach (TreatyPricingCustomOtherProduct treatyPricingCustomOtherProduct in query.ToList())
                {
                    var dataTrail = new DataTrail(treatyPricingCustomOtherProduct, true);
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingCustomOther)), treatyPricingCustomOtherProduct.PrimaryKey());

                    db.Entry(treatyPricingCustomOtherProduct).State = EntityState.Deleted;
                    db.TreatyPricingCustomOtherProducts.Remove(treatyPricingCustomOtherProduct);
                }

                db.SaveChanges();
            }
        }
    }
}
