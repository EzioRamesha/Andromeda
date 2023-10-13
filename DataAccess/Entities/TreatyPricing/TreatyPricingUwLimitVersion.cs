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
    [Table("TreatyPricingUwLimitVersions")]
    public class TreatyPricingUwLimitVersion
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingUwLimitId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingUwLimit TreatyPricingUwLimit { get; set; }

        [Required, Index]
        public int Version { get; set; }

        [Required, Index]
        public int? PersonInChargeId { get; set; }
        [ExcludeTrail]
        public virtual User PersonInCharge { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? EffectiveAt { get; set; }

        [MaxLength(3)]
        [Index]
        public string CurrencyCode { get; set; }

        [MaxLength(255), Index]
        public string UwLimit { get; set; }

        [MaxLength(255), Index]
        public string Remarks1 { get; set; }

        [MaxLength(255), Index]
        public string AblSumAssured { get; set; }

        [MaxLength(255), Index]
        public string Remarks2 { get; set; }

        [MaxLength(255), Index]
        public string AblMaxUwRating { get; set; }

        [MaxLength(255), Index]
        public string Remarks3 { get; set; }

        [MaxLength(255), Index]
        public string MaxSumAssured { get; set; }

        [Index]
        public bool PerLifePerIndustry { get; set; }

        [Index]
        public bool IssuePayoutLimit { get; set; }

        [MaxLength(255), Index]
        public string Remarks4 { get; set; }

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

        public TreatyPricingUwLimitVersion()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingUwLimitVersions.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingUwLimitVersion Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingUwLimitVersions.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingUwLimitVersions.Add(this);
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

                entity.TreatyPricingUwLimitId = TreatyPricingUwLimitId;
                entity.Version = Version;
                entity.PersonInChargeId = PersonInChargeId;
                entity.EffectiveAt = EffectiveAt;
                entity.CurrencyCode = CurrencyCode;
                entity.UwLimit = UwLimit;
                entity.Remarks1 = Remarks1;
                entity.AblSumAssured = AblSumAssured;
                entity.Remarks2 = Remarks2;
                entity.AblMaxUwRating = AblMaxUwRating;
                entity.Remarks3 = Remarks3;
                entity.MaxSumAssured = MaxSumAssured;
                entity.PerLifePerIndustry = PerLifePerIndustry;
                entity.IssuePayoutLimit = IssuePayoutLimit;
                entity.Remarks4 = Remarks4;
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
                db.TreatyPricingUwLimitVersions.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingUwLimitId(int treatyPricingUwLimitId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingUwLimitVersions.Where(q => q.TreatyPricingUwLimitId == treatyPricingUwLimitId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (TreatyPricingUwLimitVersion entity in query.ToList())
                {
                    DataTrail trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.TreatyPricingUwLimitVersions.Remove(entity);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
