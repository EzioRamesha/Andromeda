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
    [Table("DiscountTables")]
    public class DiscountTable
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int CedantId { get; set; }

        [ForeignKey(nameof(CedantId))]
        [ExcludeTrail]
        public virtual Cedant Cedant { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        [ForeignKey(nameof(CreatedById))]
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        [ForeignKey(nameof(UpdatedById))]
        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }

        public DiscountTable()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.DiscountTables.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateCedant()
        {
            using (var db = new AppDbContext())
            {
                var query = db.DiscountTables.Where(q => q.CedantId == CedantId);
                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public static DiscountTable Find(int id)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;
                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("DiscountTable");
                return connectionStrategy.Execute(() => db.DiscountTables.Where(q => q.Id == id).FirstOrDefault());
            }
        }

        public static DiscountTable FindByCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.DiscountTables.Where(q => q.CedantId == cedantId).FirstOrDefault();
            }
        }

        public static int CountByCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.DiscountTables.Where(q => q.CedantId == cedantId).Count();
            }
        }

        public static IList<DiscountTable> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.DiscountTables.ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.DiscountTables.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                DiscountTable discountTable = DiscountTable.Find(Id);
                if (discountTable == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(discountTable, this);

                discountTable.CedantId = CedantId;
                discountTable.UpdatedAt = DateTime.Now;
                discountTable.UpdatedById = UpdatedById ?? discountTable.UpdatedById;

                db.Entry(discountTable).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                DiscountTable discountTable = db.DiscountTables.Where(q => q.Id == id).FirstOrDefault();
                if (discountTable == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(discountTable, true);

                db.Entry(discountTable).State = EntityState.Deleted;
                db.DiscountTables.Remove(discountTable);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
