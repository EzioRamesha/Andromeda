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
    [Table("TreatyPricingCampaignProducts")]
    public class TreatyPricingCampaignProduct
    {
        [Key, Column(Order = 0)]
        public int TreatyPricingCampaignId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingCampaign TreatyPricingCampaign { get; set; }

        [Key, Column(Order = 1)]
        public int TreatyPricingProductId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingProduct TreatyPricingProduct { get; set; }

        public string PrimaryKey()
        {
            return string.Format("{0}|{1}", TreatyPricingCampaignId, TreatyPricingProductId);
        }

        public static bool IsExists(int treatyPricingCampaignId, int treatyPricingProductId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingCampaignProducts
                    .Any(q => q.TreatyPricingCampaignId == treatyPricingCampaignId && q.TreatyPricingProductId == treatyPricingProductId);
            }
        }

        public static TreatyPricingCampaignProduct Find(int treatyPricingCampaignId, int treatyPricingProductId)
        {
            if (treatyPricingCampaignId == 0 || treatyPricingProductId == 0)
                return null;

            using (var db = new AppDbContext())
            {
                return db.TreatyPricingCampaignProducts
                    .Where(q => q.TreatyPricingCampaignId == treatyPricingCampaignId && q.TreatyPricingProductId == treatyPricingProductId)
                    .FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext(false))
            {
                DataTrail trail = new DataTrail(this);

                db.TreatyPricingCampaignProducts.Add(this);
                db.SaveChanges();

                return trail;
            }
        }

        public void Create(AppDbContext db)
        {
            db.TreatyPricingCampaignProducts.Add(this);
        }

        public static DataTrail Delete(int treatyPricingCampaignId, int treatyPricingProductId)
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(treatyPricingCampaignId, treatyPricingProductId);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.TreatyPricingCampaignProducts.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingCampaignId(int treatyPricingCampaignId)
        {
            using (var db = new AppDbContext(false))
            {
                List<DataTrail> trails = new List<DataTrail>();

                db.Database.ExecuteSqlCommand("DELETE FROM [TreatyPricingCampaignProducts] WHERE [TreatyPricingCampaignId] = {0}", treatyPricingCampaignId);
                db.SaveChanges();

                return trails;
            }
        }

        public static void DeleteAllByTreatyPricingCampaignId(int treatyPricingCampaignId, ref TrailObject trail)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingCampaignProducts.Where(q => q.TreatyPricingCampaignId == treatyPricingCampaignId);

                foreach (TreatyPricingCampaignProduct treatyPricingCampaignProduct in query.ToList())
                {
                    var dataTrail = new DataTrail(treatyPricingCampaignProduct, true);
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingCampaign)), treatyPricingCampaignProduct.PrimaryKey());

                    db.Entry(treatyPricingCampaignProduct).State = EntityState.Deleted;
                    db.TreatyPricingCampaignProducts.Remove(treatyPricingCampaignProduct);
                }

                db.SaveChanges();
            }
        }
    }
}
