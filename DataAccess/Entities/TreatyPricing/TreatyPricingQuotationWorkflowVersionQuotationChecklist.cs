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
    [Table("TreatyPricingQuotationWorkflowVersionQuotationChecklists")]
    public class TreatyPricingQuotationWorkflowVersionQuotationChecklist
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingQuotationWorkflowVersionId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingQuotationWorkflowVersion TreatyPricingQuotationWorkflowVersion { get; set; }

        [MaxLength(255), Index]
        public string InternalTeam { get; set; }

        [MaxLength(255), Index]
        public string InternalTeamPersonInCharge { get; set; }

        [Index]
        public int Status { get; set; }

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

        public TreatyPricingQuotationWorkflowVersionQuotationChecklist()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingQuotationWorkflowVersionQuotationChecklists.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingQuotationWorkflowVersionQuotationChecklist Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingQuotationWorkflowVersionQuotationChecklists.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingQuotationWorkflowVersionQuotationChecklists.Add(this);
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

                entity.TreatyPricingQuotationWorkflowVersionId = TreatyPricingQuotationWorkflowVersionId;
                entity.InternalTeam = InternalTeam;
                entity.InternalTeamPersonInCharge = InternalTeamPersonInCharge;
                entity.Status = Status;
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
                db.TreatyPricingQuotationWorkflowVersionQuotationChecklists.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingQuotationWorkflowVersionId(int treatyPricingQuotationWorkflowVersionId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingQuotationWorkflowVersionQuotationChecklists.Where(q => q.TreatyPricingQuotationWorkflowVersionId == treatyPricingQuotationWorkflowVersionId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (TreatyPricingQuotationWorkflowVersionQuotationChecklist entity in query.ToList())
                {
                    DataTrail trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.TreatyPricingQuotationWorkflowVersionQuotationChecklists.Remove(entity);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
