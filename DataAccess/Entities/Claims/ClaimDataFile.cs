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

namespace DataAccess.Entities.Claims
{
    [Table("ClaimDataFiles")]
    public class ClaimDataFile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClaimDataBatchId { get; set; }

        [ForeignKey(nameof(ClaimDataBatchId))]
        [ExcludeTrail]
        public virtual ClaimDataBatch ClaimDataBatch { get; set; }

        [Required]
        public int RawFileId { get; set; }

        [ForeignKey(nameof(RawFileId))]
        [ExcludeTrail]
        public virtual RawFile RawFile { get; set; }

        public int? TreatyId { get; set; }

        [ForeignKey(nameof(TreatyId))]
        [ExcludeTrail]
        public virtual Treaty Treaty { get; set; }

        [Index]
        public int? ClaimDataConfigId { get; set; }

        [ForeignKey(nameof(ClaimDataConfigId))]
        [ExcludeTrail]
        public virtual ClaimDataConfig ClaimDataConfig { get; set; }

        [Index]
        public int? CurrencyCodeId { get; set; }

        [ForeignKey(nameof(CurrencyCodeId))]
        [ExcludeTrail]
        public virtual PickListDetail CurrencyCode { get; set; }

        [Index]
        public double? CurrencyRate { get; set; }

        [Required, Index]
        public int Status { get; set; }

        [Required]
        public int Mode { get; set; }

        public string Configs { get; set; }

        public string OverrideProperties { get; set; }

        public string Errors { get; set; }

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

        public ClaimDataFile()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataFiles.Any(q => q.Id == id);
            }
        }

        public static ClaimDataFile Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataFiles.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static ClaimDataFile FindByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataFiles.Where(q => q.Status == status).FirstOrDefault();
            }
        }

        public static IList<ClaimDataFile> GetByClaimDataBatchId(int claimDataBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataFiles.Where(q => q.ClaimDataBatchId == claimDataBatchId).OrderByDescending(q => q.CreatedAt).ToList();
            }
        }

        public static IList<ClaimDataFile> GetByClaimDataBatchIdMode(int claimDataBatchId, int Mode)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataFiles.Where(q => q.ClaimDataBatchId == claimDataBatchId && q.Mode == Mode).OrderByDescending(q => q.CreatedAt).ToList();
            }
        }

        public static int CountByClaimDataConfigIdStatus(int claimDataConfigId, int[] status)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataFiles.Where(q => q.ClaimDataConfigId == claimDataConfigId && status.Contains(q.ClaimDataBatch.Status)).Count();
            }
        }
        
        public static int CountByClaimDataBatchId(int claimDataBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataFiles.Where(q => q.ClaimDataBatchId == claimDataBatchId).Count();
            }
        }

        public static IList<ClaimDataFile> GetByClaimDataBatchIdExcept(int claimDataBatchId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataFiles.Where(q => q.ClaimDataBatchId == claimDataBatchId && !ids.Contains(q.Id)).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ClaimDataFiles.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                ClaimDataFile claimDataFile = Find(Id);
                if (claimDataFile == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimDataFile, this);

                claimDataFile.ClaimDataBatchId = ClaimDataBatchId;
                claimDataFile.RawFileId = RawFileId;
                claimDataFile.TreatyId = TreatyId;
                claimDataFile.Configs = Configs;
                claimDataFile.ClaimDataConfigId = ClaimDataConfigId;
                claimDataFile.CurrencyCodeId = CurrencyCodeId;
                claimDataFile.CurrencyRate = CurrencyRate;
                claimDataFile.OverrideProperties = OverrideProperties;
                claimDataFile.Mode = Mode;
                claimDataFile.Status = Status;
                claimDataFile.Errors = Errors;
                claimDataFile.UpdatedAt = DateTime.Now;
                claimDataFile.UpdatedById = UpdatedById ?? claimDataFile.UpdatedById;

                db.Entry(claimDataFile).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                ClaimDataFile claimDataFile = Find(id);
                if (claimDataFile == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimDataFile, true);

                db.Entry(claimDataFile).State = EntityState.Deleted;
                db.ClaimDataFiles.Remove(claimDataFile);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByClaimDataBatchId(int claimDataBatchId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimDataFiles.Where(q => q.ClaimDataBatchId == claimDataBatchId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (ClaimDataFile claimDataFile in query.ToList())
                {
                    DataTrail trail = new DataTrail(claimDataFile, true);
                    trails.Add(trail);

                    db.Entry(claimDataFile).State = EntityState.Deleted;
                    db.ClaimDataFiles.Remove(claimDataFile);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
