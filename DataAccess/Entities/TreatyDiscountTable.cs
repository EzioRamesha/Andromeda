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
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    [Table("TreatyDiscountTables")]
    public class TreatyDiscountTable
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(30), Index]
        public string Rule { get; set; }

        public int Type { get; set; }

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

        public TreatyDiscountTable()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyDiscountTables.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateRule()
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyDiscountTables.Where(q => q.Type == Type).Where(q => q.Rule.Trim().Equals(Rule.Trim(), StringComparison.OrdinalIgnoreCase));
                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public static TreatyDiscountTable Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyDiscountTables.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<TreatyDiscountTable> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyDiscountTables.ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyDiscountTables.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                TreatyDiscountTable treatyDiscountTables = Find(Id);
                if (treatyDiscountTables == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyDiscountTables, this);

                treatyDiscountTables.Rule = Rule;
                treatyDiscountTables.Type = Type;
                treatyDiscountTables.Description = Description;
                treatyDiscountTables.UpdatedAt = DateTime.Now;
                treatyDiscountTables.UpdatedById = UpdatedById ?? treatyDiscountTables.UpdatedById;

                db.Entry(treatyDiscountTables).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                TreatyDiscountTable treatyDiscountTable = db.TreatyDiscountTables.Where(q => q.Id == id).FirstOrDefault();
                if (treatyDiscountTable == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyDiscountTable, true);

                db.Entry(treatyDiscountTable).State = EntityState.Deleted;
                db.TreatyDiscountTables.Remove(treatyDiscountTable);
                db.SaveChanges();

                return trail;
            }
        }

        public override string ToString()
        {
            return TreatyDiscountTableBo.GetTypeName(Type) + " - " + Rule;
        }
    }
}
