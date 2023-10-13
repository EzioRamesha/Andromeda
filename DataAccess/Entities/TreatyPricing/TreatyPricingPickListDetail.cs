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
    [Table("TreatyPricingPickListDetails")]
    public class TreatyPricingPickListDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int PickListId { get; set; }
        [ExcludeTrail]
        public virtual PickList PickList { get; set; }

        [Required, Index]
        // Constant in TreatyPricingCedantBo
        public int ObjectType { get; set; }

        [Required, Index]
        public int ObjectId { get; set; }

        public string PickListDetailCode { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        public TreatyPricingPickListDetail()
        {
            CreatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingPickListDetails.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingPickListDetail Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingPickListDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<TreatyPricingPickListDetail> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingPickListDetails.ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingPickListDetails.Add(this);
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
                db.TreatyPricingPickListDetails.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
