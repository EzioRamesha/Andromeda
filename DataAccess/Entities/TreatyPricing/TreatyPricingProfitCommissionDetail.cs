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
    [Table("TreatyPricingProfitCommissionDetails")]
    public class TreatyPricingProfitCommissionDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingProfitCommissionVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingProfitCommissionVersion TreatyPricingProfitCommissionVersion { get; set; }

        [Required, Index]
        public int SortIndex { get; set; }

        [Required, Index]
        public int Item { get; set; }

        [MaxLength(255)]
        public string Component { get; set; }

        public bool IsComponentEditable { get; set; } = false;

        [MaxLength(255)]
        public string ComponentDescription { get; set; }

        public bool IsComponentDescriptionEditable { get; set; } = false;

        public bool IsDropDown { get; set; } = false;

        public int? DropDownValue { get; set; }

        public bool? IsEnabled { get; set; }

        public bool IsNetGrossRequired { get; set; } = false;

        public bool? IsNetGross { get; set; }

        [MaxLength(255)]
        public string Value { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        public int? UpdatedById { get; set; }
        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }

        public TreatyPricingProfitCommissionDetail()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProfitCommissionDetails.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingProfitCommissionDetail Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProfitCommissionDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingProfitCommissionDetails.Add(this);
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

                entity.TreatyPricingProfitCommissionVersionId = TreatyPricingProfitCommissionVersionId;
                entity.SortIndex = SortIndex;
                entity.Item = Item;
                entity.Component = Component;
                entity.IsComponentEditable = IsComponentEditable;
                entity.ComponentDescription = ComponentDescription;
                entity.IsComponentDescriptionEditable = IsComponentDescriptionEditable;
                entity.IsDropDown = IsDropDown;
                entity.DropDownValue = DropDownValue;
                entity.IsEnabled = IsEnabled;
                entity.IsNetGrossRequired = IsNetGrossRequired;
                entity.IsNetGross = IsNetGross;
                entity.Value = Value;
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
                var entity = db.TreatyPricingProfitCommissionDetails.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.TreatyPricingProfitCommissionDetails.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
