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

namespace DataAccess.Entities.Sanctions
{
    [Table("SanctionKeywordDetails")]
    public class SanctionKeywordDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int SanctionKeywordId { get; set; }
        [ExcludeTrail]
        public virtual SanctionKeyword SanctionKeyword { get; set; }

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

        public SanctionKeywordDetail()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionKeywordDetails.Any(q => q.Id == id);
            }
        }

        public static SanctionKeywordDetail Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionKeywordDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.SanctionKeywordDetails.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                SanctionKeywordDetail sanctionKeywordDetail = Find(Id);
                if (sanctionKeywordDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(sanctionKeywordDetail, this);

                sanctionKeywordDetail.SanctionKeywordId = SanctionKeywordId;
                sanctionKeywordDetail.Keyword = Keyword;
                sanctionKeywordDetail.UpdatedAt = DateTime.Now;
                sanctionKeywordDetail.UpdatedById = UpdatedById ?? sanctionKeywordDetail.UpdatedById;

                db.Entry(sanctionKeywordDetail).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                SanctionKeywordDetail sanctionKeywordDetail = db.SanctionKeywordDetails.Where(q => q.Id == id).FirstOrDefault();
                if (sanctionKeywordDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(sanctionKeywordDetail, true);

                db.Entry(sanctionKeywordDetail).State = EntityState.Deleted;
                db.SanctionKeywordDetails.Remove(sanctionKeywordDetail);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteBySanctionKeywordId(int sanctionKeywordId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.SanctionKeywordDetails.Where(q => q.SanctionKeywordId == sanctionKeywordId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (SanctionKeywordDetail sanctionKeywordDetail in query.ToList())
                {
                    DataTrail trail = new DataTrail(sanctionKeywordDetail, true);
                    trails.Add(trail);

                    db.Entry(sanctionKeywordDetail).State = EntityState.Deleted;
                    db.SanctionKeywordDetails.Remove(sanctionKeywordDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
