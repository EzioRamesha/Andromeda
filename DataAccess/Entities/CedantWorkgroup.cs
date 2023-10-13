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
    [Table("CedantWorkgroups")]
    public class CedantWorkgroup
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20), Index]
        public string Code { get; set; }
        
        [Required]
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

        public CedantWorkgroup()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.CedantWorkgroups.Any(q => q.Id == id);
            }
        }

        public static CedantWorkgroup Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.CedantWorkgroups.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public bool IsDuplicateCode()
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(Code?.Trim()))
                {
                    var query = db.CedantWorkgroups.Where(q => q.Code.Trim().Equals(Code.Trim(), StringComparison.OrdinalIgnoreCase));
                    if (Id != 0)
                    {
                        query = query.Where(q => q.Id != Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }
        
        public static CedantWorkgroup FindByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return db.CedantWorkgroups.Where(q => q.Code.Trim() == code.Trim()).FirstOrDefault();
            }
        }

        public static int CountByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return db.CedantWorkgroups.Where(q => q.Code.Trim() == code.Trim()).Count();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.CedantWorkgroups.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                CedantWorkgroup cedantWorkgroup = Find(Id);
                if (cedantWorkgroup == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(cedantWorkgroup, this);

                cedantWorkgroup.Code = Code;
                cedantWorkgroup.Description = Description;
                cedantWorkgroup.UpdatedAt = DateTime.Now;
                cedantWorkgroup.UpdatedById = UpdatedById != 0 ? UpdatedById : cedantWorkgroup.UpdatedById;

                db.Entry(cedantWorkgroup).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                CedantWorkgroup cedantWorkgroup = Find(id);
                if (cedantWorkgroup == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(cedantWorkgroup, true);

                db.Entry(cedantWorkgroup).State = EntityState.Deleted;
                db.CedantWorkgroups.Remove(cedantWorkgroup);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
