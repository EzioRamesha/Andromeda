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
    [Table("PublicHolidayDetails")]
    public class PublicHolidayDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int PublicHolidayId { get; set; }

        [ExcludeTrail]
        public virtual PublicHoliday PublicHoliday { get; set; }

        [Required, Column(TypeName = "datetime2")]
        public DateTime PublicHolidayDate { get; set; }

        [Required, MaxLength(255)]
        public string Description { get; set; }

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

        public PublicHolidayDetail()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PublicHolidayDetails.Any(q => q.Id == id);
            }
        }

        public static PublicHolidayDetail Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PublicHolidayDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static int CountByPublicHolidayId(int publicHolidayId)
        {
            using (var db = new AppDbContext())
            {
                return db.PublicHolidayDetails.Where(q => q.PublicHolidayId == publicHolidayId).Count();
            }
        }

        public static IList<PublicHolidayDetail> GetByPublicHolidayId(int publicHolidayId)
        {
            using (var db = new AppDbContext())
            {
                return db.PublicHolidayDetails.Where(q => q.PublicHolidayId == publicHolidayId).OrderBy(q => q.Id).ToList();
            }
        }

        public static IList<PublicHolidayDetail> GetByPublicHolidayIdExcept(int publicHolidayId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.PublicHolidayDetails.Where(q => q.PublicHolidayId == publicHolidayId && !ids.Contains(q.Id)).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PublicHolidayDetails.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                PublicHolidayDetail publicHolidayDetails = Find(Id);
                if (publicHolidayDetails == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(publicHolidayDetails, this);

                publicHolidayDetails.PublicHolidayId = PublicHolidayId;
                publicHolidayDetails.PublicHolidayDate = PublicHolidayDate;
                publicHolidayDetails.Description = Description;
                publicHolidayDetails.UpdatedById = UpdatedById ?? publicHolidayDetails.UpdatedById;

                db.Entry(publicHolidayDetails).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                PublicHolidayDetail publicHolidayDetails = db.PublicHolidayDetails.Where(q => q.Id == id).FirstOrDefault();
                if (publicHolidayDetails == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(publicHolidayDetails, true);

                db.Entry(publicHolidayDetails).State = EntityState.Deleted;
                db.PublicHolidayDetails.Remove(publicHolidayDetails);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByPublicHolidayId(int publicHolidayId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.PublicHolidayDetails.Where(q => q.PublicHolidayId == publicHolidayId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (PublicHolidayDetail publicHolidayDetail in query.ToList())
                {
                    DataTrail trail = new DataTrail(publicHolidayDetail, true);
                    trails.Add(trail);

                    db.Entry(publicHolidayDetail).State = EntityState.Deleted;
                    db.PublicHolidayDetails.Remove(publicHolidayDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
