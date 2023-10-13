using BusinessObject;
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

namespace DataAccess.Entities
{
    [Table("PremiumSpreadTables")]
    public class PremiumSpreadTable
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(30), Index]
        public string Rule { get; set; }

        public int Type { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

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

        public PremiumSpreadTable()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PremiumSpreadTables.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateRule()
        {
            using (var db = new AppDbContext())
            {
                var query = db.PremiumSpreadTables.Where(q => q.Type == Type).Where(q => q.Rule.Trim().Equals(Rule.Trim(), StringComparison.OrdinalIgnoreCase));
                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public static PremiumSpreadTable Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.PremiumSpreadTables.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<PremiumSpreadTable> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.PremiumSpreadTables.ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PremiumSpreadTables.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                PremiumSpreadTable premiumSpreadTables = Find(Id);
                if (premiumSpreadTables == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(premiumSpreadTables, this);

                premiumSpreadTables.Rule = Rule;
                premiumSpreadTables.Type = Type;
                premiumSpreadTables.Description = Description;
                premiumSpreadTables.UpdatedAt = DateTime.Now;
                premiumSpreadTables.UpdatedById = UpdatedById ?? premiumSpreadTables.UpdatedById;

                db.Entry(premiumSpreadTables).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                PremiumSpreadTable premiumSpreadTable = db.PremiumSpreadTables.Where(q => q.Id == id).FirstOrDefault();
                if (premiumSpreadTable == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(premiumSpreadTable, true);

                db.Entry(premiumSpreadTable).State = EntityState.Deleted;
                db.PremiumSpreadTables.Remove(premiumSpreadTable);
                db.SaveChanges();

                return trail;
            }
        }

        public override string ToString()
        {
            return PremiumSpreadTableBo.GetTypeName(Type) + " - " + Rule;
        }
    }
}
