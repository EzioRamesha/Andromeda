﻿using DataAccess.Entities.Identity;
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
    [Table("TreatyPricingMedicalTables")]
    public class TreatyPricingMedicalTable
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingCedantId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingCedant TreatyPricingCedant { get; set; }

        [MaxLength(60)]
        [Index]
        public string MedicalTableId { get; set; }

        [Index]
        public int Status { get; set; }

        [MaxLength(255), Index]
        public string Name { get; set; }

        [MaxLength(255), Index]
        public string Description { get; set; }

        [Column(TypeName = "ntext")]
        public string BenefitCode { get; set; }

        [Column(TypeName = "ntext")]
        public string DistributionChannel { get; set; }

        [MaxLength(3)]
        [Index]
        public string CurrencyCode { get; set; }

        [Index]
        public int? AgeDefinitionPickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail AgeDefinitionPickListDetail { get; set; }

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

        public TreatyPricingMedicalTable()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingMedicalTables.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingMedicalTable Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingMedicalTables.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingMedicalTables.Add(this);
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
                entity.MedicalTableId = MedicalTableId;
                entity.Status = Status;
                entity.Name = Name;
                entity.Description = Description;
                entity.BenefitCode = BenefitCode;
                entity.DistributionChannel = DistributionChannel;
                entity.CurrencyCode = CurrencyCode;
                entity.AgeDefinitionPickListDetailId = AgeDefinitionPickListDetailId;
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
                db.TreatyPricingMedicalTables.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingCedantId(int treatyPricingCedantId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingMedicalTables.Where(q => q.TreatyPricingCedantId == treatyPricingCedantId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (TreatyPricingMedicalTable entity in query.ToList())
                {
                    DataTrail trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.TreatyPricingMedicalTables.Remove(entity);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
