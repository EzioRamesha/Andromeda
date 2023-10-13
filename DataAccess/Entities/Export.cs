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
    [Table("Exports")]
    public class Export
    {
        [Key]
        [Column(TypeName = "bigint")]
        public int Id { get; set; }

        public int Type { get; set; }

        public int Status { get; set; }

        public int? ObjectId { get; set; }

        public int Total { get; set; }

        public int Processed { get; set; }

        public string Parameters { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? GenerateStartAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? GenerateEndAt { get; set; }

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

        public Export()
        {
            Type = 1;
            Status = 1;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Exports.Any(q => q.Id == id);
            }
        }

        public static Export Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Exports.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static Export Find(int id, AppDbContext db)
        {
            return db.Exports.Where(q => q.Id == id).FirstOrDefault();
        }

        public static Export FindByStatus(int status, int? id = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.Exports.Where(q => q.Status == status);
                if (id != null)
                    query = query.Where(q => q.Id == id);
                return query.FirstOrDefault();
            }
        }

        public static int Count()
        {
            using (var db = new AppDbContext())
            {
                return db.Exports.Count();
            }
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.Exports.Where(q => q.Status == status).Count();
            }
        }

        public static IList<Export> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.Exports.OrderBy(q => q.Id).ToList();
            }
        }

        public static IList<Export> GetByStatus(int? status = null, int? selectedId = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.Exports.AsQueryable();

                if (status != null)
                {
                    if (selectedId != null)
                        query = query.Where(q => q.Status == status || q.Id == selectedId);
                    else
                        query = query.Where(q => q.Status == status);
                }

                return query.OrderBy(q => q.Id).ToList();
            }
        }

        public static IList<Export> GetFailedByHours()
        {
            var hoursToCheck = Util.GetConfigInteger("ExportFailCheckHours");

            using (var db = new AppDbContext())
            {
                var query = db.Exports.AsQueryable()
                    .Where(q => q.Status == ExportBo.StatusGenerating)
                    .Where(q => DbFunctions.DiffHours(q.GenerateEndAt, DateTime.Now) >= hoursToCheck);

                return query.ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.Exports.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Create(AppDbContext db, bool save = true)
        {
            db.Exports.Add(this);
            if (save)
                db.SaveChanges();

            DataTrail trail = new DataTrail(this);
            return trail;
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var entity = Export.Find(Id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }
                var trail = new DataTrail(entity, this);
                entity.Type = Type;
                entity.Status = Status;
                entity.ObjectId = ObjectId;
                entity.Total = Total;
                entity.Processed = Processed;
                entity.Parameters = Parameters;
                entity.GenerateStartAt = GenerateStartAt;
                entity.GenerateEndAt = GenerateEndAt;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = UpdatedById ?? entity.UpdatedById;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                return trail;
            }
        }

        public DataTrail Update(AppDbContext db, bool save = true)
        {
            var entity = Export.Find(Id, db);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            var trail = new DataTrail(entity, this);
            entity.Type = Type;
            entity.Status = Status;
            entity.ObjectId = ObjectId;
            entity.Total = Total;
            entity.Processed = Processed;
            entity.Parameters = Parameters;
            entity.GenerateStartAt = GenerateStartAt;
            entity.GenerateEndAt = GenerateEndAt;
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedById = UpdatedById ?? entity.UpdatedById;

            db.Entry(entity).State = EntityState.Modified;
            if (save)
                db.SaveChanges();
            return trail;
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                var entity = db.Exports.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }
                var trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.Exports.Remove(entity);
                db.SaveChanges();
                return trail;
            }
        }
    }
}
