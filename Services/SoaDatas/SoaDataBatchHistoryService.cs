using BusinessObject.SoaDatas;
using DataAccess.Entities.SoaDatas;
using DataAccess.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace Services.SoaDatas
{
    public class SoaDataBatchHistoryService
    {
        public static SoaDataBatchHistoryBo FormBo(SoaDataBatchHistory entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            var bo = new SoaDataBatchHistoryBo
            {
                Id = entity.Id,
                CutOffId = entity.CutOffId,
                SoaDataBatchId = entity.SoaDataBatchId,
                RiDataBatchId = entity.RiDataBatchId,
                ClaimDataBatchId = entity.ClaimDataBatchId,
                CedantId = entity.CedantId,
                TreatyId = entity.TreatyId,
                CurrencyCodePickListDetailId = entity.CurrencyCodePickListDetailId,
                CurrencyRate = entity.CurrencyRate,
                Status = entity.Status,
                DataUpdateStatus = entity.DataUpdateStatus,
                Quarter = entity.Quarter,
                Type = entity.Type,
                StatementReceivedAt = entity.StatementReceivedAt,
                DirectStatus = entity.DirectStatus,
                InvoiceStatus = entity.InvoiceStatus,
                TotalRecords = entity.TotalRecords,
                TotalMappingFailedStatus = entity.TotalMappingFailedStatus,
                IsRiDataAutoCreate = entity.IsAutoCreate,
                IsClaimDataAutoCreate = entity.IsClaimDataAutoCreate,
                IsProfitCommissionData = entity.IsProfitCommissionData,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
            if (foreign)
            {
                bo.CutOff = CutOffService.Find(entity.CutOffId);
                bo.CedantBo = CedantService.Find(entity.CedantId);
            }
            return bo;
        }

        public static IList<SoaDataBatchHistoryBo> FormBos(IList<SoaDataBatchHistory> entities = null)
        {
            if (entities == null)
                return null;
            IList<SoaDataBatchHistoryBo> bos = new List<SoaDataBatchHistoryBo>() { };
            foreach (SoaDataBatchHistory entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static SoaDataBatchHistoryBo FindBySoaDataBatchIdCutOffId(int? soaDataBatchId, int? cutOffId)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBo(db.SoaDataBatchHistories
                    .Where(q => q.SoaDataBatchId == soaDataBatchId)
                    .Where(q => q.CutOffId == cutOffId)
                    .FirstOrDefault());
            }
        }

        public static IList<SoaDataBatchHistoryBo> GetStatusNotInvoice()
        {
            using (var db = new AppDbContext(false))
            {
                return FormBos(db.SoaDataBatchHistories
                    .Where(q => q.Status != SoaDataBatchBo.StatusPendingDelete)
                    .Where(q => q.InvoiceStatus == 0)
                    .ToList());
            }
        }
    }
}
