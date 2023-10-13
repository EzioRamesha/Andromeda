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
    [Table("DirectRetroStatusFiles")]
    public class DirectRetroStatusFile
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int DirectRetroId { get; set; }

        [ExcludeTrail]
        public virtual DirectRetro DirectRetro { get; set; }

        [Required, Index]
        public int StatusHistoryId { get; set; }

        [ExcludeTrail]
        public virtual StatusHistory StatusHistory { get; set; }

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

        public DirectRetroStatusFile()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.DirectRetroStatusFiles.Any(q => q.Id == id);
            }
        }

        public static DirectRetroStatusFile Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.DirectRetroStatusFiles.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.DirectRetroStatusFiles.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                DirectRetroStatusFile directRetroStatusFile = Find(Id);
                if (directRetroStatusFile == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(directRetroStatusFile, this);

                directRetroStatusFile.UpdatedAt = DateTime.Now;
                directRetroStatusFile.UpdatedById = UpdatedById ?? directRetroStatusFile.UpdatedById;

                db.Entry(directRetroStatusFile).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                DirectRetroStatusFile directRetroStatusFile = Find(id);
                if (directRetroStatusFile == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(directRetroStatusFile, true);

                db.Entry(directRetroStatusFile).State = EntityState.Deleted;
                db.DirectRetroStatusFiles.Remove(directRetroStatusFile);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByDirectRetroId(int directRetroId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.DirectRetroStatusFiles.Where(q => q.DirectRetroId == directRetroId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (DirectRetroStatusFile directRetroStatusFile in query.ToList())
                {
                    DataTrail trail = new DataTrail(directRetroStatusFile, true);
                    trails.Add(trail);

                    db.Entry(directRetroStatusFile).State = EntityState.Deleted;
                    db.DirectRetroStatusFiles.Remove(directRetroStatusFile);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
