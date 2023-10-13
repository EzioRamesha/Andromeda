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
    [Table("TreatyPricingDefinitionAndExclusion")]
    public class TreatyPricingDefinitionAndExclusion
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingCedantId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingCedant TreatyPricingCedant { get; set; }

        [Required, MaxLength(255), Index]
        public string Code { get; set; }

        [Index]
        public int Status { get; set; }

        [MaxLength(255), Index]
        public string Name { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "ntext")]
        public string BenefitCode { get; set; }
        
        public string AdditionalRemarks { get; set; }

        [Column(TypeName = "ntext")]
        public string Errors { get; set; }

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

        public TreatyPricingDefinitionAndExclusion()
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

        public bool IsDuplicateCode()
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingDefinitionAndExclusions.Where(q => q.Code == Code);
                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public static TreatyPricingDefinitionAndExclusion Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingDefinitionAndExclusions.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingDefinitionAndExclusions.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                TreatyPricingDefinitionAndExclusion treatyPricingDefinitionAndExclusion = Find(Id);
                if (treatyPricingDefinitionAndExclusion == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyPricingDefinitionAndExclusion, this);

                treatyPricingDefinitionAndExclusion.TreatyPricingCedantId = TreatyPricingCedantId;
                treatyPricingDefinitionAndExclusion.Code = Code;
                treatyPricingDefinitionAndExclusion.Status = Status;
                treatyPricingDefinitionAndExclusion.Name = Name;
                treatyPricingDefinitionAndExclusion.Description = Description;
                treatyPricingDefinitionAndExclusion.BenefitCode = BenefitCode;
                treatyPricingDefinitionAndExclusion.AdditionalRemarks = AdditionalRemarks;
                treatyPricingDefinitionAndExclusion.UpdatedAt = DateTime.Now;
                treatyPricingDefinitionAndExclusion.UpdatedById = UpdatedById ?? treatyPricingDefinitionAndExclusion.UpdatedById;

                db.Entry(treatyPricingDefinitionAndExclusion).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                TreatyPricingDefinitionAndExclusion treatyPricingDefinitionAndExclusion = db.TreatyPricingDefinitionAndExclusions.Where(q => q.Id == id).FirstOrDefault();
                if (treatyPricingDefinitionAndExclusion == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyPricingDefinitionAndExclusion, true);

                db.Entry(treatyPricingDefinitionAndExclusion).State = EntityState.Deleted;
                db.TreatyPricingDefinitionAndExclusions.Remove(treatyPricingDefinitionAndExclusion);
                db.SaveChanges();

                return trail;
            }
        }

        
    }
}
