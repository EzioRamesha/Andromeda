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

namespace DataAccess.Entities.Sanctions
{
    [Table("Sanctions")]
    public class Sanction
    {
        [Key]
        [Column(TypeName = "bigint")]
        public int Id { get; set; }

        [Required, Index]
        public int SanctionBatchId { get; set; }
        [ExcludeTrail]
        public virtual SanctionBatch SanctionBatch { get; set; }

        [MaxLength(255), Index]
        public string PublicationInformation { get; set; }

        [MaxLength(128), Index]
        public string Category { get; set; }

        [MaxLength(128), Index]
        public string Name { get; set; }

        [MaxLength(255), Index]
        public string RefNumber { get; set; }

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

        [ExcludeTrail]
        public virtual ICollection<SanctionName> SanctionNames { get; set; }
        [ExcludeTrail]
        public virtual ICollection<SanctionAddress> SanctionAddresses { get; set; }
        [ExcludeTrail]
        public virtual ICollection<SanctionBirthDate> SanctionBirthDates { get; set; }
        [ExcludeTrail]
        public virtual ICollection<SanctionComment> SanctionComments { get; set; }
        [ExcludeTrail]
        public virtual ICollection<SanctionCountry> SanctionCountries { get; set; }
        [ExcludeTrail]
        public virtual ICollection<SanctionIdentity> SanctionIdentities { get; set; }
        [ExcludeTrail]
        public virtual ICollection<SanctionFormatName> SanctionFormatNames { get; set; }

        public Sanction()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Sanctions.Any(q => q.Id == id);
            }
        }

        public static Sanction Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.Sanctions.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.Sanctions.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                Sanction entity = Find(Id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, this);

                entity.SanctionBatchId = SanctionBatchId;
                entity.PublicationInformation = PublicationInformation;
                entity.Category = Category;
                entity.RefNumber = RefNumber;
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
                Sanction entity = db.Sanctions.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.Sanctions.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteBySanctionBatchId(int sanctionBatchId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.Sanctions.Where(q => q.SanctionBatchId == sanctionBatchId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (var entity in query.ToList())
                {
                    DataTrail trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.Sanctions.Remove(entity);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
