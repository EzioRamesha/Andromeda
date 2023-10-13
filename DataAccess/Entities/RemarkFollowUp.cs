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
    [Table("RemarkFollowUps")]
    public class RemarkFollowUp
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int RemarkId { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(RemarkId))]
        public virtual Remark Remark { get; set; }

        public int Status { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? FollowUpAt { get; set; }

        public int FollowUpUserId { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(FollowUpUserId))]
        public virtual User FollowUpUser { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(CreatedById))]
        public virtual User CreatedBy { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(UpdatedById))]
        public virtual User UpdatedBy { get; set; }

        public RemarkFollowUp()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static RemarkFollowUp Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RemarkFollowUps.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static RemarkFollowUp FindByRemarkId(int remarkId)
        {
            using (var db = new AppDbContext())
            {
                return db.RemarkFollowUps.Where(q => q.RemarkId == remarkId).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RemarkFollowUps.Add(this);
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

                entity.RemarkId = RemarkId;
                entity.Status = Status;
                entity.FollowUpAt = FollowUpAt;
                entity.FollowUpUserId = FollowUpUserId;
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
                var entity = db.RemarkFollowUps.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.RemarkFollowUps.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
