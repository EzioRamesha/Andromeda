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
    [Table("TreatyPricingUwQuestionnaires")]
    public class TreatyPricingUwQuestionnaire
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingCedantId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingCedant TreatyPricingCedant { get; set; }

        [Required, MaxLength(255), Index]
        public string Code { get; set; }

        [MaxLength(255), Index]
        public string Name { get; set; }

        [MaxLength(255), Index]
        public string Description { get; set; }

        [Index]
        public int Status { get; set; }

        [Column(TypeName = "ntext")]
        public string BenefitCode { get; set; }

        [Column(TypeName = "ntext")]
        public string DistributionChannel { get; set; }

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

        public TreatyPricingUwQuestionnaire()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingUwQuestionnaires.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingUwQuestionnaire Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingUwQuestionnaires.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingUwQuestionnaires.Add(this);
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

                entity.TreatyPricingCedantId = TreatyPricingCedantId;
                entity.Code = Code;
                entity.Name = Name;
                entity.Description = Description;
                entity.Status = Status;
                entity.BenefitCode = BenefitCode;
                entity.DistributionChannel = DistributionChannel;
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
                db.TreatyPricingUwQuestionnaires.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingCedantId(int treatyPricingCedantId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingUwQuestionnaires.Where(q => q.TreatyPricingCedantId == treatyPricingCedantId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (TreatyPricingUwQuestionnaire entity in query.ToList())
                {
                    DataTrail trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.TreatyPricingUwQuestionnaires.Remove(entity);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
