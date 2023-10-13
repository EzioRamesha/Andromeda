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
    [Table("Treaties")]
    public class Treaty
    {
        [Key]
        public int Id { get; set; }

        // User input
        [Required, MaxLength(30), Index]
        public string TreatyIdCode { get; set; }

        [Required]
        public int CedantId { get; set; }

        [ForeignKey(nameof(CedantId))]
        [ExcludeTrail]
        public virtual Cedant Cedant { get; set; }

        [MaxLength(255), Index]
        public string Description { get; set; }

        public int? BusinessOriginPickListDetailId { get; set; }

        [ForeignKey(nameof(BusinessOriginPickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail BusinessOriginPickListDetail { get; set; }

        public int? LineOfBusinessPickListDetailId { get; set; }

        [ForeignKey(nameof(LineOfBusinessPickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail LineOfBusinessPickListDetail { get; set; }

        [MaxLength(255), Index]
        public string BlockDescription { get; set; }

        [MaxLength(255), Index]
        public string Remarks { get; set; }

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

        public Treaty()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Treaties.Any(q => q.Id == id);
            }
        }

        public static Treaty Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Treaties.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static Treaty Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.Treaties.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static Treaty FindByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return db.Treaties.Where(q => q.TreatyIdCode.Trim() == code.Trim()).FirstOrDefault();
            }
        }

        public static int CountByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return db.Treaties.Where(q => q.TreatyIdCode.Trim() == code.Trim()).Count();
            }
        }

        public static int CountByCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.Treaties.Where(q => q.CedantId == cedantId).Count();
            }
        }

        public static int CountByBusinessOriginPickListDetailId(int businessOriginPickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.Treaties.Where(q => q.BusinessOriginPickListDetailId == businessOriginPickListDetailId).Count();
            }
        }

        public static int CountByLineOfBusinessPickListDetailId(int lineOfBusinessPickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.Treaties.Where(q => q.LineOfBusinessPickListDetailId == lineOfBusinessPickListDetailId).Count();
            }
        }

        public static IList<Treaty> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.Treaties.OrderBy(q => q.TreatyIdCode).ToList();
            }
        }

        public static IList<Treaty> GetByCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                if (cedantId == 0)
                {
                    return db.Treaties.OrderBy(q => q.TreatyIdCode).ToList();
                }
                else
                {
                    return db.Treaties
                        .Where(q => q.CedantId == cedantId)
                        .OrderBy(q => q.TreatyIdCode)
                        .ToList();
                }
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.Treaties.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                Treaty treaties = Treaty.Find(Id);
                if (treaties == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treaties, this);

                treaties.TreatyIdCode = TreatyIdCode;
                treaties.CedantId = CedantId;
                treaties.Description = Description;
                treaties.BusinessOriginPickListDetailId = BusinessOriginPickListDetailId;
                treaties.LineOfBusinessPickListDetailId = LineOfBusinessPickListDetailId;
                treaties.BlockDescription = BlockDescription;
                treaties.Remarks = Remarks;
                treaties.UpdatedAt = DateTime.Now;
                treaties.UpdatedById = UpdatedById ?? treaties.UpdatedById;

                db.Entry(treaties).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                Treaty treaties = db.Treaties.Where(q => q.Id == id).FirstOrDefault();
                if (treaties == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treaties, true);

                db.Entry(treaties).State = EntityState.Deleted;
                db.Treaties.Remove(treaties);
                db.SaveChanges();

                return trail;
            }
        }
    }
}