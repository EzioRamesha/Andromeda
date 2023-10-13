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

namespace DataAccess.Entities.Sanctions
{
    [Table("SanctionKeywords")]
    public class SanctionKeyword
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(10), Index]
        public string Code { get; set; }

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

        [ExcludeTrail]
        public virtual ICollection<SanctionKeywordDetail> SanctionKeywordDetails { get; set; }

        public SanctionKeyword()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionKeywords.Any(q => q.Id == id);
            }
        }

        public static SanctionKeyword Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionKeywords.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.SanctionKeywords.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                SanctionKeyword sanctionKeyword = Find(Id);
                if (sanctionKeyword == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(sanctionKeyword, this);

                sanctionKeyword.Code = Code;
                sanctionKeyword.UpdatedAt = DateTime.Now;
                sanctionKeyword.UpdatedById = UpdatedById ?? sanctionKeyword.UpdatedById;

                db.Entry(sanctionKeyword).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                SanctionKeyword sanctionKeyword = db.SanctionKeywords.Where(q => q.Id == id).FirstOrDefault();
                if (sanctionKeyword == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(sanctionKeyword, true);

                db.Entry(sanctionKeyword).State = EntityState.Deleted;
                db.SanctionKeywords.Remove(sanctionKeyword);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
