using BusinessObject.TreatyPricing;
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
    [Table("TreatyPricingCustomOtherVersions")]
    public class TreatyPricingCustomOtherVersion
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingCustomOtherId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingCustomOther TreatyPricingCustomOther { get; set; }

        [Required, Index]
        public int Version { get; set; }

        public int? PersonInChargeId { get; set; }
        [ExcludeTrail]
        public virtual User PersonInCharge { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? EffectiveAt { get; set; }

        [MaxLength(255), Index]
        public string FileName { get; set; }

        [MaxLength(255), Index]
        public string HashFileName { get; set; }

        public string AdditionalRemarks { get; set; }

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

        public TreatyPricingCustomOtherVersion()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingCustomOtherVersions.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingCustomOtherVersion Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingCustomOtherVersions.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingCustomOtherVersions.Add(this);
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

                entity.TreatyPricingCustomOtherId = TreatyPricingCustomOtherId;
                entity.Version = Version;
                entity.PersonInChargeId = PersonInChargeId;
                entity.EffectiveAt = EffectiveAt;
                entity.FileName = FileName;
                entity.HashFileName = HashFileName;
                entity.AdditionalRemarks = AdditionalRemarks;
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
                var entity = Find(id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.TreatyPricingCustomOtherVersions.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingCustomOtherId(int treatyPricingCustomOtherId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingCustomOtherVersions.Where(q => q.TreatyPricingCustomOtherId == treatyPricingCustomOtherId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (TreatyPricingCustomOtherVersion entity in query.ToList())
                {
                    DataTrail trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.TreatyPricingCustomOtherVersions.Remove(entity);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
