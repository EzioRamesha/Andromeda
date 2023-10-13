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
    [Table("TreatyPricingRateTableDetails")]
    public class TreatyPricingRateTableDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingRateTableVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingRateTableVersion TreatyPricingRateTableVersion { get; set; }

        [Required, Index]
        public int Type { get; set; }

        [Required, MaxLength(255)]
        public string Col1 { get; set; }

        [Required, MaxLength(255)]
        public string Col2 { get; set; }

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

        public TreatyPricingRateTableDetail()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingRateTableDetails.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingRateTableDetail Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingRateTableDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingRateTableDetails.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                TreatyPricingRateTableDetail treatyPricingRateTableDetail = Find(Id);
                if (treatyPricingRateTableDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyPricingRateTableDetail, this);

                treatyPricingRateTableDetail.TreatyPricingRateTableVersionId = TreatyPricingRateTableVersionId;
                treatyPricingRateTableDetail.Type = Type;
                treatyPricingRateTableDetail.Col1 = Col1;
                treatyPricingRateTableDetail.Col2 = Col2;
                treatyPricingRateTableDetail.UpdatedAt = DateTime.Now;
                treatyPricingRateTableDetail.UpdatedById = UpdatedById ?? treatyPricingRateTableDetail.UpdatedById;

                db.Entry(treatyPricingRateTableDetail).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                TreatyPricingRateTableDetail treatyPricingRateTableDetail = db.TreatyPricingRateTableDetails.Where(q => q.Id == id).FirstOrDefault();
                if (treatyPricingRateTableDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyPricingRateTableDetail, true);

                db.Entry(treatyPricingRateTableDetail).State = EntityState.Deleted;
                db.TreatyPricingRateTableDetails.Remove(treatyPricingRateTableDetail);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
