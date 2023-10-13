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
    [Table("TreatyPricingDefinitionAndExclusionVersions")]
    public class TreatyPricingDefinitionAndExclusionVersion
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingDefinitionAndExclusionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingDefinitionAndExclusion TreatyPricingDefinitionAndExclusion { get; set; }

        [Required, Index]
        public int Version { get; set; }

        public int? PersonInChargeId { get; set; }
        [ExcludeTrail]
        public virtual User PersonInCharge { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? EffectiveAt { get; set; }

        public string Definitions { get; set; }
        public string Exclusions { get; set; }
        public string DeclinedRisk { get; set; }
        public string ReferredRisk { get; set; }

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

        public TreatyPricingDefinitionAndExclusionVersion()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingDefinitionAndExclusions.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingDefinitionAndExclusionVersion Find (int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingDefinitionAndExclusionVersions.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingDefinitionAndExclusionVersions.Add(this);
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

                entity.TreatyPricingDefinitionAndExclusionId = TreatyPricingDefinitionAndExclusionId;
                entity.Version = Version;
                entity.PersonInChargeId = PersonInChargeId;
                entity.EffectiveAt = EffectiveAt;
                entity.Definitions = Definitions;
                entity.Exclusions = Exclusions;
                entity.DeclinedRisk = DeclinedRisk;
                entity.ReferredRisk = ReferredRisk;
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
                db.TreatyPricingDefinitionAndExclusionVersions.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingDefinitionAndExclusionId(int treatyPricingDefinitionAndExclusionId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingDefinitionAndExclusionVersions.Where(q => q.TreatyPricingDefinitionAndExclusionId == treatyPricingDefinitionAndExclusionId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (TreatyPricingDefinitionAndExclusionVersion entity in query.ToList())
                {
                    DataTrail trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.TreatyPricingDefinitionAndExclusionVersions.Remove(entity);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
