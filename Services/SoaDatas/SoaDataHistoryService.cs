using BusinessObject;
using BusinessObject.SoaDatas;
using DataAccess.Entities.SoaDatas;
using DataAccess.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace Services.SoaDatas
{
    public class SoaDataHistoryService
    {
        public static SoaDataHistoryBo FormBo(SoaDataHistory entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            var bo = new SoaDataHistoryBo
            {
                Id = entity.Id,

                CutOffId = entity.CutOffId,

                SoaDataBatchId = entity.SoaDataBatchId,
                SoaDataFileId = entity.SoaDataFileId,

                MappingStatus = entity.MappingStatus,
                Errors = entity.Errors,

                CompanyName = entity.CompanyName,
                BusinessCode = entity.BusinessCode,
                TreatyId = entity.TreatyId,
                TreatyCode = entity.TreatyCode,
                TreatyMode = entity.TreatyMode,
                TreatyType = entity.TreatyType,
                PlanBlock = entity.PlanBlock,
                RiskMonth = entity.RiskMonth,
                SoaQuarter = entity.SoaQuarter,
                RiskQuarter = entity.RiskQuarter,
                NbPremium = entity.NbPremium,
                RnPremium = entity.RnPremium,
                AltPremium = entity.AltPremium,
                GrossPremium = entity.GrossPremium,
                TotalDiscount = entity.TotalDiscount,
                RiskPremium = entity.RiskPremium,
                NoClaimBonus = entity.NoClaimBonus,
                Levy = entity.Levy,
                Claim = entity.Claim,
                ProfitComm = entity.ProfitComm,
                SurrenderValue = entity.SurrenderValue,
                Gst = entity.Gst,
                ModcoReserveIncome = entity.ModcoReserveIncome,
                RiDeposit = entity.RiDeposit,
                DatabaseCommission = entity.DatabaseCommission,
                AdministrationContribution = entity.AdministrationContribution,
                ShareOfRiCommissionFromCompulsoryCession = entity.ShareOfRiCommissionFromCompulsoryCession,
                RecaptureFee = entity.RecaptureFee,
                CreditCardCharges = entity.CreditCardCharges,
                BrokerageFee = entity.BrokerageFee,
                TotalCommission = entity.TotalCommission,
                NetTotalAmount = entity.NetTotalAmount,
                SoaReceivedDate = entity.SoaReceivedDate,
                BordereauxReceivedDate = entity.BordereauxReceivedDate,
                StatementStatus = entity.StatementStatus,
                Remarks1 = entity.Remarks1,
                CurrencyCode = entity.CurrencyCode,
                CurrencyRate = entity.CurrencyRate,
                SoaStatus = entity.SoaStatus,
                ConfirmationDate = entity.ConfirmationDate,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };

            if (foreign)
            {
                bo.CutOff = CutOffService.Find(entity.CutOffId);
                bo.SoaDataBatchHistoryBo = SoaDataBatchHistoryService.FindBySoaDataBatchIdCutOffId(entity.SoaDataBatchId, entity.CutOffId);
            }

            return bo;
        }

        public static IList<SoaDataHistoryBo> FormBos(IList<SoaDataHistory> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            IList<SoaDataHistoryBo> bos = new List<SoaDataHistoryBo>() { };
            foreach (SoaDataHistory entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static IList<SoaDataHistoryBo> SoaDataHistoryReportParams()
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.SoaDataHistories
                    .Where(q => q.CurrencyCode == PickListDetailBo.CurrencyCodeMyr);

                List<int> cutOffIds = new List<int> { };
                List<int> soaDataBatchIds = new List<int> { };

                var SoaDataBatchHistoryBos = SoaDataBatchHistoryService.GetStatusNotInvoice();
                if (SoaDataBatchHistoryBos != null)
                {
                    cutOffIds = SoaDataBatchHistoryBos.Select(b => b.CutOffId).Distinct().ToList();
                    soaDataBatchIds = SoaDataBatchHistoryBos.Select(b => b.SoaDataBatchId).Distinct().ToList();

                    query = query.Where(q => cutOffIds.Contains(q.CutOffId) && soaDataBatchIds.Contains(q.SoaDataBatchId));
                }

                return FormBos(query.ToList());
            }
        }
    }
}
