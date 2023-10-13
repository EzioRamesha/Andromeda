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
    [Table("EventClaimCodeMappings")]
    public class EventClaimCodeMapping
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int CedantId { get; set; }

        [ForeignKey(nameof(CedantId))]
        [ExcludeTrail]
        public virtual Cedant Cedant { get; set; }

        [Required, Index]
        public int EventCodeId { get; set; }

        [ForeignKey(nameof(EventCodeId))]
        [ExcludeTrail]
        public virtual EventCode EventCode { get; set; }

        public string CedingEventCode { get; set; }

        public string CedingClaimType { get; set; }

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

        [ExcludeTrail]
        public virtual ICollection<EventClaimCodeMappingDetail> EventClaimCodeMappingDetails { get; set; }

        public EventClaimCodeMapping()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.EventClaimCodeMappings.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicate()
        {
            using (var db = new AppDbContext())
            {
                if (CedantId != 0)
                {
                    var query = db.EventClaimCodeMappings.Where(q => q.CedantId == CedantId && q.EventCodeId == EventCodeId);
                    if (Id != 0)
                    {
                        query = query.Where(q => q.Id != Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static EventClaimCodeMapping Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.EventClaimCodeMappings.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<EventClaimCodeMapping> GetByCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.EventClaimCodeMappings.Where(q => q.CedantId == cedantId).OrderBy(q => q.Id).ToList();
            }
        }

        public static IList<EventClaimCodeMapping> GetByCedantIdEventCodeId(int cedantId, int eventCodeId)
        {
            using (var db = new AppDbContext())
            {
                return db.EventClaimCodeMappings.Where(q => q.CedantId == cedantId && q.EventCodeId == eventCodeId).OrderBy(q => q.Id).ToList();
            }
        }

        public static int CountByEventCode(int eventCodeId)
        {
            using (var db = new AppDbContext())
            {
                return db.EventClaimCodeMappings.Where(q => q.EventCodeId == eventCodeId).Count();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.EventClaimCodeMappings.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                EventClaimCodeMapping eventClaimCodeMappings = EventClaimCodeMapping.Find(Id);
                if (eventClaimCodeMappings == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(eventClaimCodeMappings, this);

                eventClaimCodeMappings.CedantId = CedantId;
                eventClaimCodeMappings.EventCodeId = EventCodeId;
                eventClaimCodeMappings.CedingEventCode = CedingEventCode;
                eventClaimCodeMappings.CedingClaimType = CedingClaimType;
                eventClaimCodeMappings.UpdatedAt = DateTime.Now;
                eventClaimCodeMappings.UpdatedById = UpdatedById ?? eventClaimCodeMappings.UpdatedById;

                db.Entry(eventClaimCodeMappings).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                EventClaimCodeMapping eventClaimCodeMappings = db.EventClaimCodeMappings.Where(q => q.Id == id).FirstOrDefault();
                if (eventClaimCodeMappings == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(eventClaimCodeMappings, true);

                db.Entry(eventClaimCodeMappings).State = EntityState.Deleted;
                db.EventClaimCodeMappings.Remove(eventClaimCodeMappings);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
