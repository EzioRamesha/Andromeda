using BusinessObject;
using BusinessObject.Claims;
using DataAccess.Entities.Identity;
using DataAccess.Entities.SoaDatas;
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
    [Table("ClaimDataBatches")]
    public class ClaimDataBatch
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int Status { get; set; }

        [Required, Index]
        public int CedantId { get; set; }

        [ForeignKey(nameof(CedantId))]
        [ExcludeTrail]
        public virtual Cedant Cedant { get; set; }

        [Index]
        public int? TreatyId { get; set; }

        [ForeignKey(nameof(TreatyId))]
        [ExcludeTrail]
        public virtual Treaty Treaty { get; set; }

        [Required, Index]
        public int ClaimDataConfigId { get; set; }

        [ForeignKey(nameof(ClaimDataConfigId))]
        [ExcludeTrail]
        public virtual ClaimDataConfig ClaimDataConfig { get; set; }

        [Index]
        public int? SoaDataBatchId { get; set; }

        [ForeignKey(nameof(SoaDataBatchId))]
        [ExcludeTrail]
        public virtual SoaDataBatch SoaDataBatch { get; set; }

        [Index]
        public int? ClaimTransactionTypePickListDetailId { get; set; }

        [ForeignKey(nameof(ClaimTransactionTypePickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail ClaimTransactionTypePickListDetail { get; set; }

        public string Configs { get; set; }

        public string OverrideProperties { get; set; }

        [MaxLength(64), Index]
        public string Quarter { get; set; }

        public int TotalMappingFailedStatus { get; set; }
        public int TotalPreComputationFailedStatus { get; set; }
        public int TotalPreValidationFailedStatus { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ReceivedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }
        [Required]
        public int CreatedById { get; set; }
        [ForeignKey(nameof(CreatedById))]
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedById { get; set; }
        [ForeignKey(nameof(UpdatedById))]
        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }

        public ClaimDataBatch()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.GetClaimDataBatches().Any(q => q.Id == id);
            }
        }

        public static ClaimDataBatch Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.GetClaimDataBatches().Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static ClaimDataBatch FindByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.GetClaimDataBatches().Where(q => q.Status == status).FirstOrDefault();
            }
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.GetClaimDataBatches().Where(q => q.Status == status).Count();
            }
        }

        public static int CountByCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.GetClaimDataBatches().Where(q => q.CedantId == cedantId).Count();
            }
        }

        public static int CountByTreatyId(int treatyId)
        {
            using (var db = new AppDbContext())
            {
                return db.GetClaimDataBatches().Where(q => q.TreatyId == treatyId).Count();
            }
        }

        public static int CountByClaimDataConfigId(int claimDataConfigId, bool deleted = false)
        {
            using (var db = new AppDbContext())
            {
                return db.GetClaimDataBatches(deleted).Where(q => q.ClaimDataConfigId == claimDataConfigId).Count();
            }
        }

        public static IList<ClaimDataBatch> GetByParam(int? cedantId, int? treatyId, string quarter)
        {
            using (var db = new AppDbContext())
            {
                return db.GetClaimDataBatches()
                    .Where(q => q.CedantId == cedantId)
                    .Where(q => q.TreatyId == treatyId)
                    .Where(q => q.Quarter == quarter)
                    .ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ClaimDataBatches.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                ClaimDataBatch claimDataBatch = Find(Id);
                if (claimDataBatch == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimDataBatch, this);

                claimDataBatch.CedantId = CedantId;
                claimDataBatch.TreatyId = TreatyId;
                claimDataBatch.ClaimDataConfigId = ClaimDataConfigId;
                claimDataBatch.SoaDataBatchId = SoaDataBatchId;
                claimDataBatch.ClaimTransactionTypePickListDetailId = ClaimTransactionTypePickListDetailId;
                claimDataBatch.Status = Status;
                claimDataBatch.Quarter = Quarter;
                claimDataBatch.Configs = Configs;
                claimDataBatch.OverrideProperties = OverrideProperties;
                claimDataBatch.UpdatedAt = DateTime.Now;
                claimDataBatch.ReceivedAt = ReceivedAt;
                claimDataBatch.UpdatedById = UpdatedById ?? claimDataBatch.UpdatedById;

                db.Entry(claimDataBatch).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                ClaimDataBatch claimDataBatch = db.ClaimDataBatches.Where(q => q.Id == id).FirstOrDefault();
                if (claimDataBatch == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimDataBatch, true);

                db.Entry(claimDataBatch).State = EntityState.Deleted;
                db.ClaimDataBatches.Remove(claimDataBatch);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<ClaimDataBatch> GetProcessingFailedByHours()
        {
            var hoursToCheck = Util.GetConfigInteger("ProcessClaimDataFailCheckHours");

            using (var db = new AppDbContext())
            {
                return db.GetClaimDataBatches()
                    .Where(q => q.Status == ClaimDataBatchBo.StatusProcessing)
                    .Where(q => DbFunctions.DiffHours(q.UpdatedAt, DateTime.Now) >= hoursToCheck)
                    .ToList();
            }
        }
    }
}
