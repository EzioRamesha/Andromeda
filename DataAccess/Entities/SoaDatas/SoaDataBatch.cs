using BusinessObject.SoaDatas;
using DataAccess.Entities.Claims;
using DataAccess.Entities.Identity;
using DataAccess.Entities.RiDatas;
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
    [Table("SoaDataBatches")]
    public class SoaDataBatch
    {
        [Key]
        public int Id { get; set; }


        [Required, Index]
        public int CedantId { get; set; }
        [ExcludeTrail]
        public virtual Cedant Cedant { get; set; }


        [Index]
        public int? TreatyId { get; set; }
        [ExcludeTrail]
        public virtual Treaty Treaty { get; set; }


        [MaxLength(64), Index]
        public string Quarter { get; set; }

        public int? CurrencyCodePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail CurrencyCodePickListDetail { get; set; }

        public double? CurrencyRate { get; set; }

        [Index]
        public int Type { get; set; }

        [Required, Index]
        public int Status { get; set; }

        [Required, Index]
        public int DataUpdateStatus { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime StatementReceivedAt { get; set; }

        [Index]
        public int? RiDataBatchId { get; set; }
        [ExcludeTrail]
        public virtual RiDataBatch RiDataBatch { get; set; }

        [Index]
        public int? ClaimDataBatchId { get; set; }
        [ExcludeTrail]
        public virtual ClaimDataBatch ClaimDataBatch { get; set; }

        public int DirectStatus { get; set; }
        public int InvoiceStatus { get; set; }

        public int TotalRecords { get; set; }
        public int TotalMappingFailedStatus { get; set; }

        public bool IsAutoCreate { get; set; } = false;
        public bool IsClaimDataAutoCreate { get; set; } = false;

        public bool IsProfitCommissionData { get; set; } = false;

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

        public SoaDataBatch()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.GetSoaDataBatches().Any(q => q.Id == id);
            }
        }

        public static SoaDataBatch Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.GetSoaDataBatches().Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<SoaDataBatch> GetByParam(int? cedantId, int? treatyId, string quarter)
        {
            using (var db = new AppDbContext())
            {
                return db.GetSoaDataBatches()
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
                db.SoaDataBatches.Add(this);
                db.SaveChanges();

                return new DataTrail(this);
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(Id);
                if (entity == null)
                    throw new Exception(MessageBag.NoRecordFound);

                var trail = new DataTrail(entity, this);

                entity.RiDataBatchId = RiDataBatchId;
                entity.ClaimDataBatchId = ClaimDataBatchId;
                entity.CedantId = CedantId;
                entity.TreatyId = TreatyId;
                entity.CurrencyCodePickListDetailId = CurrencyCodePickListDetailId;
                entity.CurrencyRate = CurrencyRate;
                entity.Status = Status;
                entity.DataUpdateStatus = DataUpdateStatus;
                entity.Quarter = Quarter;
                entity.Type = Type;
                entity.StatementReceivedAt = StatementReceivedAt;
                entity.DirectStatus = DirectStatus;
                entity.InvoiceStatus = InvoiceStatus;
                entity.IsAutoCreate = IsAutoCreate;
                entity.IsClaimDataAutoCreate = IsClaimDataAutoCreate;
                entity.TotalRecords = TotalRecords;
                entity.TotalMappingFailedStatus = TotalMappingFailedStatus;
                entity.IsProfitCommissionData = IsProfitCommissionData;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = UpdatedById ?? entity.UpdatedById;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                SoaDataBatch soaDataBatch = db.SoaDataBatches.Where(q => q.Id == id).FirstOrDefault();
                if (soaDataBatch == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(soaDataBatch, true);

                db.Entry(soaDataBatch).State = EntityState.Deleted;
                db.SoaDataBatches.Remove(soaDataBatch);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<SoaDataBatch> GetProcessingFailedByHours(bool dataUpdate = false)
        {
            var hoursToCheck = Util.GetConfigInteger("ProcessSoaDataFailCheckHours");

            using (var db = new AppDbContext())
            {
                var query = db.GetSoaDataBatches();

                if (dataUpdate)
                    query = query.Where(q => q.DataUpdateStatus == SoaDataBatchBo.DataUpdateStatusDataUpdating);
                else
                    query = query.Where(q => q.Status == SoaDataBatchBo.StatusProcessing);

                return query
                    .Where(q => DbFunctions.DiffHours(q.UpdatedAt, DateTime.Now) >= hoursToCheck)
                    .ToList();
            }
        }
    }
}
