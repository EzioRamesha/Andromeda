using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Shared;
using Shared.Trails;
using Shared.Trails.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Windows.Input;

namespace DataAccess.Entities.Sanctions
{
    [Table("SanctionExclusions")]
    public class SanctionExclusion
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(255), Index]
        public string Keyword { get; set; }

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

        public SanctionExclusion()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionExclusions.Any(q => q.Id == id);
            }
        }

        public static SanctionExclusion Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionExclusions.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.SanctionExclusions.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                SanctionExclusion sanctionExclusion = Find(Id);
                if (sanctionExclusion == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(sanctionExclusion, this);

                sanctionExclusion.Keyword = Keyword;
                sanctionExclusion.UpdatedAt = DateTime.Now;
                sanctionExclusion.UpdatedById = UpdatedById ?? sanctionExclusion.UpdatedById;

                db.Entry(sanctionExclusion).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                SanctionExclusion sanctionExclusion = db.SanctionExclusions.Where(q => q.Id == id).FirstOrDefault();
                if (sanctionExclusion == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(sanctionExclusion, true);

                db.Entry(sanctionExclusion).State = EntityState.Deleted;
                db.SanctionExclusions.Remove(sanctionExclusion);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
