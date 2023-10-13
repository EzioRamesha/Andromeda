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
    [Table("TreatyPricingMedicalTableVersions")]
    public class TreatyPricingMedicalTableVersion
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingMedicalTableId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingMedicalTable TreatyPricingMedicalTable { get; set; }

        [Required, Index]
        public int Version { get; set; }

        [Required, Index]
        public int? PersonInChargeId { get; set; }
        [ExcludeTrail]
        public virtual User PersonInCharge { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? EffectiveAt { get; set; }

        [MaxLength(255), Index]
        public string Remarks { get; set; }

        [MaxLength(255), Index]
        public string AggregationNote { get; set; }

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

        public TreatyPricingMedicalTableVersion()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingMedicalTableVersions.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingMedicalTableVersion Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingMedicalTableVersions.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingMedicalTableVersions.Add(this);
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

                entity.TreatyPricingMedicalTableId = TreatyPricingMedicalTableId;
                entity.Version = Version;
                entity.PersonInChargeId = PersonInChargeId;
                entity.EffectiveAt = EffectiveAt;
                entity.Remarks = Remarks;
                entity.AggregationNote = AggregationNote;
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
                db.TreatyPricingMedicalTableVersions.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingMedicalTableId(int treatyPricingMedicalTableId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingMedicalTableVersions.Where(q => q.TreatyPricingMedicalTableId == treatyPricingMedicalTableId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (TreatyPricingMedicalTableVersion entity in query.ToList())
                {
                    DataTrail trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.TreatyPricingMedicalTableVersions.Remove(entity);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
