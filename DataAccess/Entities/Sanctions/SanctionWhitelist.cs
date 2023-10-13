﻿using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Shared;
using Shared.Trails;
using Shared.Trails.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataAccess.Entities.Sanctions
{
    [Table("SanctionWhitelists")]
    public class SanctionWhitelist
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150), Index]
        public string PolicyNumber { get; set; }

        [Required, MaxLength(128), Index]
        public string InsuredName { get; set; }

        public string Reason { get; set; }

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

        public SanctionWhitelist()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionWhitelists.Any(q => q.Id == id);
            }
        }

        public static bool IsExists(string policyNumber, string insuredName)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionWhitelists.Any(q => q.PolicyNumber == policyNumber && q.InsuredName == insuredName);
            }
        }

        public static SanctionWhitelist Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return Find(id, db);
            }
        }

        public static SanctionWhitelist Find(int id, AppDbContext db)
        {
            return db.SanctionWhitelists.Where(q => q.Id == id).FirstOrDefault();
        }

        public static SanctionWhitelist Find(string policyNumber, string insuredName)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("SanctionWhitelist");

                return connectionStrategy.Execute(() => db.SanctionWhitelists.Where(q => q.PolicyNumber == policyNumber && q.InsuredName == insuredName).FirstOrDefault());
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.SanctionWhitelists.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                SanctionWhitelist entity = Find(Id, db);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, this);

                entity.PolicyNumber = PolicyNumber;
                entity.InsuredName = InsuredName;
                entity.Reason = Reason;
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
                SanctionWhitelist entity = Find(id, db);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.SanctionWhitelists.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
