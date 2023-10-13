using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class RetroRegisterHistoryService
    {
        public static RetroRegisterHistoryBo FormBo(RetroRegisterHistory entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            RetroRegisterHistoryBo retroRegisterHistoryBo = new RetroRegisterHistoryBo()
            {
                Id = entity.Id,
                CutOffId = entity.CutOffId,
                Type = entity.Type,
                RetroRegisterBatchId = entity.RetroRegisterBatchId,
                RetroStatementType = entity.RetroStatementType,
                RetroStatementNo = entity.RetroStatementNo,
                RetroStatementDate = entity.RetroStatementDate,
                ReportCompletedDate = entity.ReportCompletedDate,
                SendToRetroDate = entity.SendToRetroDate,
                RetroPartyId = entity.RetroPartyId,
                RiskQuarter = entity.RiskQuarter,
                CedantId = entity.CedantId,
                TreatyCodeId = entity.TreatyCodeId,
                TreatyNumber = entity.TreatyNumber,
                Schedule = entity.Schedule,
                TreatyType = entity.TreatyType,
                LOB = entity.LOB,
                AccountFor = entity.AccountFor,
                Year1st = entity.Year1st,
                Renewal = entity.Renewal,
                ReserveCededBegin = entity.ReserveCededBegin,
                ReserveCededEnd = entity.ReserveCededEnd,
                RiskChargeCededBegin = entity.RiskChargeCededBegin,
                RiskChargeCededEnd = entity.RiskChargeCededEnd,
                AverageReserveCeded = entity.AverageReserveCeded,
                Gross1st = entity.Gross1st,
                GrossRen = entity.GrossRen,
                AltPremium = entity.AltPremium,
                Discount1st = entity.Discount1st,
                DiscountRen = entity.DiscountRen,
                DiscountAlt = entity.DiscountAlt,
                RiskPremium = entity.RiskPremium,
                Claims = entity.Claims,
                ProfitCommission = entity.ProfitCommission,
                SurrenderVal = entity.SurrenderVal,
                NoClaimBonus = entity.NoClaimBonus,
                RetrocessionMarketingFee = entity.RetrocessionMarketingFee,
                AgreedDBCommission = entity.AgreedDBCommission,
                GstPayable = entity.GstPayable,
                NetTotalAmount = entity.NetTotalAmount,
                NbCession = entity.NbCession,
                NbSumReins = entity.NbSumReins,
                RnCession = entity.RnCession,
                RnSumReins = entity.RnSumReins,
                AltCession = entity.AltCession,
                AltSumReins = entity.AltSumReins,
                Frequency = entity.Frequency,
                PreparedById = entity.PreparedById,
                OriginalSoaQuarter = entity.OriginalSoaQuarter,
                RetroConfirmationDate = entity.RetroConfirmationDate,
                ValuationGross1st = entity.ValuationGross1st,
                ValuationGrossRen = entity.ValuationGrossRen,
                ValuationDiscount1st = entity.ValuationDiscount1st,
                ValuationDiscountRen = entity.ValuationDiscountRen,
                ValuationCom1st = entity.ValuationCom1st,
                ValuationComRen = entity.ValuationComRen,
                Remark = entity.Remark,
                Status = entity.Status,
                AnnualCohort = entity.AnnualCohort,
                ContractCode = entity.ContractCode,
                ReportingType = entity.ReportingType,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
                DirectRetroId = entity.DirectRetroId,
            };
            if (foreign)
            {
                retroRegisterHistoryBo.CutOffBo = CutOffService.Find(entity.CutOffId);
                retroRegisterHistoryBo.RetroPartyBo = RetroPartyService.Find(entity.RetroPartyId);
                retroRegisterHistoryBo.CedantBo = CedantService.Find(entity.CedantId);
                retroRegisterHistoryBo.TreatyCodeBo = TreatyCodeService.Find(entity.TreatyCodeId);
            }
            return retroRegisterHistoryBo;
        }

        public static IList<RetroRegisterHistoryBo> FormBos(IList<RetroRegisterHistory> entities = null)
        {
            if (entities == null)
                return null;
            IList<RetroRegisterHistoryBo> bos = new List<RetroRegisterHistoryBo>() { };
            foreach (RetroRegisterHistory entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static IList<RetroRegisterHistoryBo> RetroRegisterHistoryReportParams()
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroRegisterHistories
                   .Where(q => q.ReportingType != RetroRegisterHistoryBo.ReportingTypeIFRS17);

                return FormBos(query.ToList());
            }
        }
    }
}
