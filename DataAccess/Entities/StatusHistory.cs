using BusinessObject;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
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
    [Table("StatusHistories")]
    public class StatusHistory
    {
        [Key]
        [Column(TypeName = "bigint")]
        public int Id { get; set; }

        [Required, Index]
        public int ModuleId { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(ModuleId))]
        public virtual Module Module { get; set; }

        [Required, Index]
        public int ObjectId { get; set; }

        [MaxLength(64), Index]
        public string SubModuleController { get; set; }

        [Index]
        public int? SubObjectId { get; set; }

        [Index]
        public int? Version { get; set; }

        [Required, Index]
        public int Status { get; set; }

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

        public StatusHistory()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public StatusHistory(StatusHistoryBo statusHistoryBo) : this()
        {
            Id = statusHistoryBo.Id;
            ModuleId = statusHistoryBo.ModuleId;
            ObjectId = statusHistoryBo.ObjectId;
            Status = statusHistoryBo.Status;
            CreatedAt = statusHistoryBo.CreatedAt;
            CreatedById = statusHistoryBo.CreatedById;
            UpdatedById = statusHistoryBo.UpdatedById;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.StatusHistories.Any(q => q.Id == id);
            }
        }

        public static StatusHistory Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.StatusHistories.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static StatusHistory FindByModuleIdObjectIdStatus(int moduleId, int objectId, int status)
        {
            using (var db = new AppDbContext())
            {
                return db.StatusHistories.Where(q => q.ModuleId == moduleId).Where(q => q.ObjectId == objectId).Where(q => q.Status == status).FirstOrDefault();
            }
        }

        public static StatusHistory FindLatestByModuleIdObjectId(int moduleId, int objectId)
        {
            using (var db = new AppDbContext())
            {
                return db.StatusHistories.Where(q => q.ModuleId == moduleId && q.ObjectId == objectId).OrderByDescending(q => q.CreatedAt).FirstOrDefault();
            }
        }

        public static IList<StatusHistory> GetStatusHistoriesByModuleIdObjectId(int moduleId, int objectId)
        {
            using (var db = new AppDbContext())
            {
                return db.StatusHistories.Where(q => q.ModuleId == moduleId && q.ObjectId == objectId).OrderByDescending(q => q.CreatedAt).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("StatusHistory");

                connectionStrategy.Execute(() =>
                {
                    db.StatusHistories.Add(this);
                    db.SaveChanges();
                });

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                StatusHistory statusHistory = StatusHistory.Find(Id);
                if (statusHistory == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(statusHistory, this);

                statusHistory.ModuleId = ModuleId;
                statusHistory.ObjectId = ObjectId;
                statusHistory.SubModuleController = SubModuleController;
                statusHistory.SubObjectId = SubObjectId;
                statusHistory.Version = Version;
                statusHistory.Status = Status;
                statusHistory.UpdatedAt = DateTime.Now;
                statusHistory.UpdatedById = UpdatedById ?? statusHistory.UpdatedById;

                db.Entry(statusHistory).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                StatusHistory statusHistory = db.StatusHistories.Where(q => q.Id == id).FirstOrDefault();
                if (statusHistory == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(statusHistory, true);

                db.Entry(statusHistory).State = EntityState.Deleted;
                db.StatusHistories.Remove(statusHistory);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByModuleIdObjectId(int moduleId, int objectId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.StatusHistories.Where(q => q.ModuleId == moduleId && q.ObjectId == objectId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (StatusHistory statusHistory in query.ToList())
                {
                    DataTrail trail = new DataTrail(statusHistory, true);
                    trails.Add(trail);

                    db.Entry(statusHistory).State = EntityState.Deleted;
                    db.StatusHistories.Remove(statusHistory);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
