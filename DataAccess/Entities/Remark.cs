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
    [Table("Remarks")]
    public class Remark
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int ModuleId { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(ModuleId))]
        public virtual Module Module { get; set; }

        [Required, Index]
        public int ObjectId { get; set; }

        public string SubModuleController { get; set; }

        [Index]
        public int? SubObjectId { get; set; }

        [Index]
        public int? StatusHistoryId { get; set; }
        [ExcludeTrail]
        public virtual StatusHistory StatusHistory { get; set; }

        [Required, Index]
        public int Status { get; set; }

        [Required]
        public string Content { get; set; }

        [MaxLength(128), Index]
        public string Subject { get; set; }

        [Index]
        public int? Version { get; set; }

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

        public Remark()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public Remark(RemarkBo remarkBo) : this()
        {
            Id = remarkBo.Id;
            ModuleId = remarkBo.ModuleId;
            ObjectId = remarkBo.ObjectId;
            Status = remarkBo.Status;
            Content = remarkBo.Content;
            CreatedById = remarkBo.CreatedById;
            UpdatedById = remarkBo.UpdatedById;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Remarks.Any(q => q.Id == id);
            }
        }

        public static Remark Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Remarks.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<Remark> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.Remarks.OrderByDescending(q => q.CreatedAt).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.Remarks.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                Remark remark = Remark.Find(Id);
                if (remark == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(remark, this);

                remark.ModuleId = ModuleId;
                remark.ObjectId = ObjectId;
                remark.SubModuleController = SubModuleController;
                remark.SubObjectId = SubObjectId;
                remark.StatusHistoryId = StatusHistoryId;
                remark.Status = Status;
                remark.Content = Content;
                remark.Subject = Subject;
                remark.Version = Version;
                remark.UpdatedAt = DateTime.Now;
                remark.UpdatedById = UpdatedById ?? remark.UpdatedById;

                db.Entry(remark).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                Remark remark = db.Remarks.Where(q => q.Id == id).FirstOrDefault();
                if (remark == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(remark, true);

                db.Entry(remark).State = EntityState.Deleted;
                db.Remarks.Remove(remark);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByModuleIdObjectId(int moduleId, int objectId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.Remarks.Where(q => q.ModuleId == moduleId && q.ObjectId == objectId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (Remark remark in query.ToList())
                {
                    DataTrail trail = new DataTrail(remark, true);
                    trails.Add(trail);

                    db.Entry(remark).State = EntityState.Deleted;
                    db.Remarks.Remove(remark);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
