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

namespace DataAccess.Entities.SoaDatas
{
    [Table("SoaDataFiles")]
    public class SoaDataFile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SoaDataBatchId { get; set; }

        [ExcludeTrail]
        public virtual SoaDataBatch SoaDataBatch { get; set; }

        [Required]
        public int RawFileId { get; set; }

        [ExcludeTrail]
        public virtual RawFile RawFile { get; set; }

        public int? TreatyId { get; set; }

        [ExcludeTrail]
        public virtual Treaty Treaty { get; set; }

        [Required]
        public int Mode { get; set; }

        [Required, Index]
        public int Status { get; set; }

        public string Errors { get; set; }


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

        public SoaDataFile()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RawFiles.Any(q => q.Id == id);
            }
        }

        public static SoaDataFile Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.SoaDataFiles.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static SoaDataFile FindByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.SoaDataFiles.Where(q => q.Status == status).FirstOrDefault();
            }
        }

        public static IList<SoaDataFile> GetBySoaDataBatchId(int soaDataBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.SoaDataFiles.Where(q => q.SoaDataBatchId == soaDataBatchId).OrderByDescending(q => q.CreatedAt).ToList();
            }
        }

        public static IList<SoaDataFile> GetBySoaDataBatchIdMode(int soaDataBatchId, int Mode)
        {
            using (var db = new AppDbContext())
            {
                return db.SoaDataFiles.Where(q => q.SoaDataBatchId == soaDataBatchId && q.Mode == Mode).OrderByDescending(q => q.CreatedAt).ToList();
            }
        }

        public static IList<SoaDataFile> GetBySoaDataBatchIdExcept(int soaDataBatchId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.SoaDataFiles.Where(q => q.SoaDataBatchId == soaDataBatchId && !ids.Contains(q.Id)).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.SoaDataFiles.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                SoaDataFile soaDataFile = Find(Id);
                if (soaDataFile == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(soaDataFile, this);

                soaDataFile.SoaDataBatchId = SoaDataBatchId;
                soaDataFile.RawFileId = RawFileId;
                soaDataFile.TreatyId = TreatyId;
                soaDataFile.Mode = Mode;
                soaDataFile.Status = Status;
                soaDataFile.Errors = Errors;
                soaDataFile.UpdatedAt = DateTime.Now;
                soaDataFile.UpdatedById = UpdatedById ?? soaDataFile.UpdatedById;

                db.Entry(soaDataFile).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                SoaDataFile soaDataFile = Find(id);
                if (soaDataFile == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(soaDataFile, true);

                db.Entry(soaDataFile).State = EntityState.Deleted;
                db.SoaDataFiles.Remove(soaDataFile);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllBySoaDataBatchId(int soaDataBatchId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.SoaDataFiles.Where(q => q.SoaDataBatchId == soaDataBatchId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (SoaDataFile soaDataFile in query.ToList())
                {
                    DataTrail trail = new DataTrail(soaDataFile, true);
                    trails.Add(trail);

                    db.Entry(soaDataFile).State = EntityState.Deleted;
                    db.SoaDataFiles.Remove(soaDataFile);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
