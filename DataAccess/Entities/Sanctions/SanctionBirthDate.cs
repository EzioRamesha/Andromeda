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
    [Table("SanctionBirthDates")]
    public class SanctionBirthDate
    {
        [Key]
        [Column(TypeName = "bigint")]
        public int Id { get; set; }

        [Required, Index]
        public int SanctionId { get; set; }
        [ExcludeTrail]
        public virtual Sanction Sanction { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? DateOfBirth { get; set; }

        [Index]
        public int? YearOfBirth { get; set; }

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

        public SanctionBirthDate()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionBirthDates.Any(q => q.Id == id);
            }
        }

        public static SanctionBirthDate Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionBirthDates.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.SanctionBirthDates.Add(this);
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

                entity.SanctionId = SanctionId;
                entity.DateOfBirth = DateOfBirth;
                entity.YearOfBirth = YearOfBirth;
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
                var entity = db.SanctionBirthDates.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.SanctionBirthDates.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteBySanctionId(int sanctionId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.SanctionBirthDates.Where(q => q.SanctionId == sanctionId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (var entity in query.ToList())
                {
                    DataTrail trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.SanctionBirthDates.Remove(entity);
                }

                db.SaveChanges();
                return trails;
            }
        }

        public static IList<DataTrail> DeleteBySanctionBatchId(int sanctionBatchId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.SanctionBirthDates.Where(q => q.Sanction.SanctionBatchId == sanctionBatchId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (var entity in query.ToList())
                {
                    DataTrail trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.SanctionBirthDates.Remove(entity);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
