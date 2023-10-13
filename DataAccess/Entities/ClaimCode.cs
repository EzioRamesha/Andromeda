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
    [Table("ClaimCodes")]
    public class ClaimCode
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(30), Index]
        public string Code { get; set; }

        [Required, MaxLength(255), Index]
        public string Description { get; set; }

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

        public ClaimCode()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimCodes.Any(q => q.Id == id);
            }
        }

        public static ClaimCode Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimCodes.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static ClaimCode FindByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimCodes.Where(q => q.Code.Trim() == code.Trim()).FirstOrDefault();
            }
        }

        public static int Count()
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimCodes.Count();
            }
        }

        public static IList<ClaimCode> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimCodes.OrderBy(q => q.Code).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ClaimCodes.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                ClaimCode claimCodes = Find(Id);
                if (claimCodes == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimCodes, this);

                claimCodes.Code = Code;
                claimCodes.Description = Description;
                claimCodes.Status = Status;
                claimCodes.UpdatedAt = DateTime.Now;
                claimCodes.UpdatedById = UpdatedById ?? claimCodes.UpdatedById;

                db.Entry(claimCodes).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                ClaimCode claimCodes = db.ClaimCodes.Where(q => q.Id == id).FirstOrDefault();
                if (claimCodes == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimCodes, true);

                db.Entry(claimCodes).State = EntityState.Deleted;
                db.ClaimCodes.Remove(claimCodes);
                db.SaveChanges();

                return trail;
            }
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Description))
            {
                return Code;
            }
            return string.Format("{0} - {1}", Code, Description);
        }
    }
}
