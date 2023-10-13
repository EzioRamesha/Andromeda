using BusinessObject.InvoiceRegisters;
using DataAccess.Entities.InvoiceRegisters;
using DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.InvoiceRegisters
{
    public class InvoiceRegisterHistoryService
    {
        public static InvoiceRegisterHistoryBo FormBo(InvoiceRegisterHistory entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            var bo = new InvoiceRegisterHistoryBo
            {
                Id = entity.Id,
                CutOffId = entity.CutOffId,
                InvoiceRegisterId = entity.InvoiceRegisterId,
                InvoiceRegisterBatchId = entity.InvoiceRegisterBatchId,
                InvoiceType = entity.InvoiceType,
                InvoiceReference = entity.InvoiceReference,
                InvoiceNumber = entity.InvoiceNumber,
                InvoiceDate = entity.InvoiceDate,
                StatementReceivedDate = entity.StatementReceivedDate,
                CedantId = entity.CedantId,
                RiskQuarter = entity.RiskQuarter,
                TreatyCodeId = entity.TreatyCodeId,
                AccountDescription = entity.AccountDescription,
                TotalPaid = entity.TotalPaid,
                PaymentReference = entity.PaymentReference,
                PaymentAmount = entity.PaymentAmount,
                PaymentReceivedDate = entity.PaymentReceivedDate,

                Year1st = entity.Year1st,
                Renewal = entity.Renewal,
                Gross1st = entity.Gross1st,
                GrossRenewal = entity.GrossRenewal,
                AltPremium = entity.AltPremium,
                Discount1st = entity.Discount1st,
                DiscountRen = entity.DiscountRen,
                DiscountAlt = entity.DiscountAlt,

                RiskPremium = entity.RiskPremium,
                NoClaimBonus = entity.NoClaimBonus,
                Levy = entity.Levy,
                Claim = entity.Claim,
                ProfitComm = entity.ProfitComm,
                SurrenderValue = entity.SurrenderValue,
                Gst = entity.Gst,
                ModcoReserveIncome = entity.ModcoReserveIncome,
                ReinsDeposit = entity.ReinsDeposit,
                DatabaseCommission = entity.DatabaseCommission,
                AdministrationContribution = entity.AdministrationContribution,
                ShareOfRiCommissionFromCompulsoryCession = entity.ShareOfRiCommissionFromCompulsoryCession,
                RecaptureFee = entity.RecaptureFee,
                CreditCardCharges = entity.CreditCardCharges,
                BrokerageFee = entity.BrokerageFee,

                NBCession = entity.NBCession,
                NBSumReins = entity.NBSumReins,
                RNCession = entity.RNCession,
                RNSumReins = entity.RNSumReins,
                AltCession = entity.AltCession,
                AltSumReins = entity.AltSumReins,

                DTH = entity.DTH,
                TPA = entity.TPA,
                TPS = entity.TPS,
                PPD = entity.PPD,
                CCA = entity.CCA,
                CCS = entity.CCS,
                PA = entity.PA,
                HS = entity.HS,
                TPD = entity.TPD,
                CI = entity.CI,

                Frequency = entity.Frequency,
                PreparedById = entity.PreparedById,
                CurrencyCode = entity.CurrencyCode,
                CurrencyRate = entity.CurrencyRate,
                SoaQuarter = entity.SoaQuarter,

                ValuationGross1st = entity.ValuationGross1st,
                ValuationGrossRen = entity.ValuationGrossRen,
                ValuationDiscount1st = entity.ValuationDiscount1st,
                ValuationDiscountRen = entity.ValuationDiscountRen,
                ValuationCom1st = entity.ValuationCom1st,
                ValuationComRen = entity.ValuationComRen,
                ValuationClaims = entity.ValuationClaims,
                ValuationProfitCom = entity.ValuationProfitCom,
                ValuationMode = entity.ValuationMode,
                ValuationRiskPremium = entity.ValuationRiskPremium,

                Remark = entity.Remark,
                ReasonOfAdjustment1 = entity.ReasonOfAdjustment1,
                ReasonOfAdjustment2 = entity.ReasonOfAdjustment2,
                InvoiceNumber1 = entity.InvoiceNumber1,
                InvoiceDate1 = entity.InvoiceDate1,
                Amount1 = entity.Amount1,
                InvoiceNumber2 = entity.InvoiceNumber2,
                InvoiceDate2 = entity.InvoiceDate2,
                Amount2 = entity.Amount2,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                SoaDataBatchId = entity.SoaDataBatchId,

                ContractCode = entity.ContractCode,
                AnnualCohort = entity.AnnualCohort,
                ReportingType = entity.ReportingType,
            };
            if (foreign)
            {
                bo.CutOffBo = CutOffService.Find(entity.CutOffId);
                bo.CedantBo = CedantService.Find(entity.CedantId);
                bo.TreatyCodeBo = TreatyCodeService.Find(entity.TreatyCodeId);
            }
            return bo;
        }

        public static IList<InvoiceRegisterHistoryBo> FormBos(IList<InvoiceRegisterHistory> entities = null)
        {
            if (entities == null)
                return null;
            IList<InvoiceRegisterHistoryBo> bos = new List<InvoiceRegisterHistoryBo>() { };
            foreach (InvoiceRegisterHistory entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static IList<InvoiceRegisterHistoryBo> InvoiceRegisterHistoryReportParams(int reportingType)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.InvoiceRegisterHistories
                    .Where(q => q.ReportingType == reportingType);

                return FormBos(query.ToList());
            }
        }
    }
}
