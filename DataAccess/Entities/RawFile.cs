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
    [Table("RawFiles")]
    public class RawFile
    {
        [Key]
        [Column(TypeName = "bigint")]
        public int Id { get; set; }

        public int Type { get; set; }

        public int Status { get; set; }

        [MaxLength(255), Index]
        public string FileName { get; set; }

        [MaxLength(255), Index]
        public string HashFileName { get; set; }

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

        public RawFile()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RawFiles.Any(q => q.Id == id);
            }
        }

        public static RawFile Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RawFiles.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static RawFile FindByTypeStatus(int type, int status = 0)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RawFiles.Where(q => q.Type == type);
                if (status != 0)
                    query.Where(q => q.Status == status);

                return query.FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RawFiles.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                RawFile rawFile = RawFile.Find(Id);
                if (rawFile == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(rawFile, this);

                rawFile.Type = Type;
                rawFile.Status = Status;
                rawFile.FileName = FileName;
                rawFile.HashFileName = HashFileName;
                rawFile.UpdatedAt = DateTime.Now;
                rawFile.UpdatedById = UpdatedById ?? rawFile.UpdatedById;

                db.Entry(rawFile).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                RawFile rawFile = RawFile.Find(id);
                if (rawFile == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(rawFile, true);

                db.Entry(rawFile).State = EntityState.Deleted;
                db.RawFiles.Remove(rawFile);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
