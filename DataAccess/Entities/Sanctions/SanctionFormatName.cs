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

namespace DataAccess.Entities.Sanctions
{
    [Table("SanctionFormatNames")]
    public class SanctionFormatName
    {
        [Key]
        [Column(TypeName = "bigint")]
        public int Id { get; set; }

        [Required, Index]
        public int SanctionId { get; set; }
        [ExcludeTrail]
        public virtual Sanction Sanction { get; set; }

        [Required, Index]
        public int SanctionNameId { get; set; }
        [ExcludeTrail]
        public virtual SanctionName SanctionName { get; set; }

        [Index]
        public int Type { get; set; }

        [Index]
        public int? TypeIndex { get; set; } // for Type 4 only

        [Required, MaxLength(255), Index]
        public string Name { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        public SanctionFormatName()
        {
            CreatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionFormatNames.Any(q => q.Id == id);
            }
        }

        public static SanctionFormatName Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionFormatNames.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.SanctionFormatNames.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                SanctionFormatName entity = db.SanctionFormatNames.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.SanctionFormatNames.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteBySanctionId(int sanctionId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.SanctionFormatNames.Where(q => q.SanctionId == sanctionId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (SanctionFormatName entity in query.ToList())
                {
                    DataTrail trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.SanctionFormatNames.Remove(entity);
                }

                db.SaveChanges();
                return trails;
            }
        }

        public static IList<DataTrail> DeleteBySanctionBatchId(int sanctionBatchId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.SanctionFormatNames.Where(q => q.Sanction.SanctionBatchId == sanctionBatchId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (SanctionFormatName entity in query.ToList())
                {
                    DataTrail trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.SanctionFormatNames.Remove(entity);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
