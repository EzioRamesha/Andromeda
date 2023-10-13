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
    [Table("EventClaimCodeMappingDetails")]
    public class EventClaimCodeMappingDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int EventClaimCodeMappingId { get; set; }

        [ForeignKey(nameof(EventClaimCodeMappingId))]
        [ExcludeTrail]
        public virtual EventClaimCodeMapping EventClaimCodeMapping { get; set; }

        [MaxLength(255), Index]
        public string Combination { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        [ForeignKey(nameof(CreatedById))]
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        [MaxLength(30), Index]
        public string CedingEventCode { get; set; }

        [MaxLength(30), Index]
        public string CedingClaimType { get; set; }

        public EventClaimCodeMappingDetail()
        {
            CreatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.EventClaimCodeMappingDetails.Any(q => q.Id == id);
            }
        }

        public static IQueryable<EventClaimCodeMappingDetail> QueryByCombination(
            AppDbContext db,
            string combination,
            int? cedantId = null,
            int? eventClaimCodeMappingId = null
        )
        {
            var query = db.EventClaimCodeMappingDetails.Where(q => q.Combination.Trim() == combination.Trim());

            if (cedantId != null)
            {
                query = query
                    .Where(q => q.EventClaimCodeMapping.CedantId == cedantId);
            }
            else
            {
                query = query
                    .Where(q => q.EventClaimCodeMapping.Cedant == null);
            }

            if (eventClaimCodeMappingId != null)
            {
                query = query.Where(q => q.EventClaimCodeMappingId != eventClaimCodeMappingId);
            }

            return query;
        }

        public static IQueryable<EventClaimCodeMappingDetail> QueryByParams(
            AppDbContext db,
            string cedingEventCode = null,
            string cedingClaimType = null,
            int? cedantId = null,
            bool groupById = false
        )
        {
            var query = db.EventClaimCodeMappingDetails.AsQueryable();

            if (cedantId != null)
            {
                query = query
                    .Where(q => (q.EventClaimCodeMapping.CedantId == cedantId));
            }

            if (!string.IsNullOrEmpty(cedingEventCode))
            {
                query = query
                    .Where(q => (q.CedingEventCode != null && q.CedingEventCode.Trim() == cedingEventCode.Trim()) || q.CedingEventCode == null);
            }
            else
            {
                query = query
                    .Where(q => q.CedingEventCode == null);
            }

            if (!string.IsNullOrEmpty(cedingClaimType))
            {
                query = query
                    .Where(q => (q.CedingClaimType != null && q.CedingClaimType.Trim() == cedingClaimType.Trim()) || q.CedingClaimType == null);
            }
            else
            {
                query = query
                    .Where(q => q.CedingClaimType == null);
            }

            // NOTE: Group by should put at the end of query
            if (groupById)
            {
                query = query.GroupBy(q => q.EventClaimCodeMappingId).Select(q => q.FirstOrDefault());
            }

            return query;
        }

        public static EventClaimCodeMappingDetail Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.EventClaimCodeMappingDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static EventClaimCodeMappingDetail FindByCombination(
            string combination,
            int cedantId,
            int? eventClaimCodeMappingId = null
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByCombination(
                    db,
                    combination,
                    cedantId,
                    eventClaimCodeMappingId
                ).FirstOrDefault();
            }
        }

        public static EventClaimCodeMappingDetail FindByParams(
            string cedingEventCode = null,
            string cedingClaimType = null,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByParams(
                    db,
                    cedingEventCode,
                    cedingClaimType,
                    groupById: groupById
                ).FirstOrDefault();
            }
        }

        public static EventClaimCodeMappingDetail FindByCedantParams(
           int cedantId,
           string cedingEventCode = null,
           string cedingClaimType = null,
           bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByParams(
                    db,
                    cedingEventCode,
                    cedingClaimType,
                    cedantId,
                    groupById
                ).FirstOrDefault();
            }
        }

        public static int CountByCombination(
            string combination,
            int cedantId,
            int? eventClaimCodeMappingId = null
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByCombination(
                    db,
                    combination,
                    cedantId,
                    eventClaimCodeMappingId
                ).Count();
            }
        }

        public static int CountByParams(
           string cedingEventCode = null,
           string cedingClaimType = null,
           bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByParams(
                    db,
                    cedingEventCode,
                    cedingClaimType,
                    groupById: groupById
                ).Count();
            }
        }
        
        public static int CountByCedantParams(
           int cedantId,
           string cedingEventCode = null,
           string cedingClaimType = null,
           bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByParams(
                    db,
                    cedingEventCode,
                    cedingClaimType,
                    cedantId,
                    groupById
                ).Count();
            }
        }

        public static IList<EventClaimCodeMappingDetail> GetByParams(
            string cedingEventCode = null,
            string cedingClaimType = null,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByParams(
                    db,
                    cedingEventCode,
                    cedingClaimType,
                    groupById: groupById
                ).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.EventClaimCodeMappingDetails.Add(this);
                db.SaveChanges();

                var trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var entity = EventClaimCodeMappingDetail.Find(Id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(entity, this);

                entity.EventClaimCodeMappingId = EventClaimCodeMappingId;
                entity.Combination = Combination;
                entity.CedingEventCode = CedingEventCode;
                entity.CedingClaimType = CedingClaimType;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                var entity = db.EventClaimCodeMappingDetails.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.EventClaimCodeMappingDetails.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByEventClaimCodeMappingId(int eventClaimCodeMappingId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.EventClaimCodeMappingDetails.Where(q => q.EventClaimCodeMappingId == eventClaimCodeMappingId);

                var trails = new List<DataTrail>();
                foreach (EventClaimCodeMappingDetail eventClaimCodeMappingDetail in query.ToList())
                {
                    var trail = new DataTrail(eventClaimCodeMappingDetail, true);
                    trails.Add(trail);

                    db.Entry(eventClaimCodeMappingDetail).State = EntityState.Deleted;
                    db.EventClaimCodeMappingDetails.Remove(eventClaimCodeMappingDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
