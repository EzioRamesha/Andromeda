using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Shared;
using Shared.Trails;
using Shared.Trails.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataAccess.Entities
{
    [Table("CutOff")]
    public class CutOff
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int Status { get; set; }

        public int Month { get; set; }
        public int Year { get; set; }
        [MaxLength(32), Index]
        public string Quarter { get; set; }
        public DateTime? CutOffDateTime { get; set; }

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

        public CutOff()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.CutOff.Any(q => q.Id == id);
            }
        }

        public static CutOff Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.CutOff.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.CutOff.Add(this);
                db.SaveChanges();

                return new DataTrail(this);
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(Id);
                if (entity == null)
                    throw new Exception(MessageBag.NoRecordFound);

                var trail = new DataTrail(entity, this);

                entity.Status = Status;
                entity.Month = Month;
                entity.Year = Year;
                entity.Quarter = Quarter;
                entity.CutOffDateTime = CutOffDateTime;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = UpdatedById != 0 ? UpdatedById : entity.UpdatedById;

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
                    throw new Exception(MessageBag.NoRecordFound);

                var trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.CutOff.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
