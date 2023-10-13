using BusinessObject;
using BusinessObject.SoaDatas;
using DataAccess.Entities.SoaDatas;
using DataAccess.EntityFramework;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.SoaDatas
{
    public class SoaDataCompiledSummaryService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(SoaDataCompiledSummary)),
            };
        }

        public static Expression<Func<SoaDataCompiledSummary, SoaDataCompiledSummaryBo>> Expression()
        {
            return entity => new SoaDataCompiledSummaryBo
            {
                Id = entity.Id,
                SoaDataBatchId = entity.SoaDataBatchId,
                InvoiceType = entity.InvoiceType,
                InvoiceDate = entity.InvoiceDate,
                StatementReceivedDate = entity.StatementReceivedDate,

                BusinessCode = entity.BusinessCode,
                TreatyCode = entity.TreatyCode,
                SoaQuarter = entity.SoaQuarter,
                RiskQuarter = entity.RiskQuarter,
                AccountDescription = entity.AccountDescription,

                NbPremium = entity.NbPremium,
                RnPremium = entity.RnPremium,
                AltPremium = entity.AltPremium,

                NbDiscount = entity.NbDiscount,
                RnDiscount = entity.RnDiscount,
                AltDiscount = entity.AltDiscount,

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
                NetTotalAmount = entity.NetTotalAmount,

                ReasonOfAdjustment1 = entity.ReasonOfAdjustment1,
                ReasonOfAdjustment2 = entity.ReasonOfAdjustment2,
                InvoiceNumber1 = entity.InvoiceNumber1,
                InvoiceDate1 = entity.InvoiceDate1,
                Amount1 = entity.Amount1,
                InvoiceNumber2 = entity.InvoiceNumber2,
                InvoiceDate2 = entity.InvoiceDate2,
                Amount2 = entity.Amount2,
                FilingReference = entity.FilingReference,
                ServiceFeePercentage = entity.ServiceFeePercentage,
                ServiceFee = entity.ServiceFee,
                Sst = entity.Sst,
                TotalAmount = entity.TotalAmount,

                NbCession = entity.NbCession,
                RnCession = entity.RnCession,
                AltCession = entity.AltCession,

                NbSar = entity.NbSar,
                RnSar = entity.RnSar,
                AltSar = entity.AltSar,

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

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                CurrencyCode = entity.CurrencyCode,
                CurrencyRate = entity.CurrencyRate,
                ContractCode = entity.ContractCode,
                AnnualCohort = entity.AnnualCohort,
                Frequency = entity.Frequency,
                ReportingType = entity.ReportingType,
            };
        }

        public static SoaDataCompiledSummaryBo FormBo(SoaDataCompiledSummary entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            var bo = new SoaDataCompiledSummaryBo
            {
                Id = entity.Id,
                SoaDataBatchId = entity.SoaDataBatchId,
                InvoiceType = entity.InvoiceType,
                InvoiceDate = entity.InvoiceDate,
                StatementReceivedDate = entity.StatementReceivedDate,

                BusinessCode = entity.BusinessCode,
                TreatyCode = entity.TreatyCode,
                SoaQuarter = entity.SoaQuarter,
                RiskQuarter = entity.RiskQuarter,
                AccountDescription = entity.AccountDescription,

                NbPremium = entity.NbPremium,
                RnPremium = entity.RnPremium,
                AltPremium = entity.AltPremium,

                NbDiscount = entity.NbDiscount,
                RnDiscount = entity.RnDiscount,
                AltDiscount = entity.AltDiscount,

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
                NetTotalAmount = entity.NetTotalAmount,

                ReasonOfAdjustment1 = entity.ReasonOfAdjustment1,
                ReasonOfAdjustment2 = entity.ReasonOfAdjustment2,
                InvoiceNumber1 = entity.InvoiceNumber1,
                InvoiceDate1 = entity.InvoiceDate1,
                Amount1 = entity.Amount1,
                InvoiceNumber2 = entity.InvoiceNumber2,
                InvoiceDate2 = entity.InvoiceDate2,
                Amount2 = entity.Amount2,
                FilingReference = entity.FilingReference,
                ServiceFeePercentage = entity.ServiceFeePercentage,
                ServiceFee = entity.ServiceFee,
                Sst = entity.Sst,
                TotalAmount = entity.TotalAmount,

                NbCession = entity.NbCession,
                RnCession = entity.RnCession,
                AltCession = entity.AltCession,

                NbSar = entity.NbSar,
                RnSar = entity.RnSar,
                AltSar = entity.AltSar,

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

                ContractCode = entity.ContractCode,
                AnnualCohort = entity.AnnualCohort,
                Frequency = entity.Frequency,
                Mfrs17CellName = entity.Mfrs17CellName,
                ReportingType = entity.ReportingType,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                StatementReceivedDateStr = entity.StatementReceivedDate?.ToString(Util.GetDateFormat()),
                InvoiceDate1Str = entity.InvoiceDate1?.ToString(Util.GetDateFormat()),
                Amount1Str = Util.DoubleToString(entity.Amount1, 2),
                InvoiceDate2Str = entity.InvoiceDate2?.ToString(Util.GetDateFormat()),
                Amount2Str = Util.DoubleToString(entity.Amount2, 2),

                NbPremiumStr = Util.DoubleToString(entity.NbPremium, 2),
                RnPremiumStr = Util.DoubleToString(entity.RnPremium, 2),
                AltPremiumStr = Util.DoubleToString(entity.AltPremium, 2),

                NbDiscountStr = Util.DoubleToString(entity.NbDiscount, 2),
                RnDiscountStr = Util.DoubleToString(entity.RnDiscount, 2),
                AltDiscountStr = Util.DoubleToString(entity.AltDiscount, 2),

                TotalDiscountStr = Util.DoubleToString(entity.TotalDiscount, 2),
                RiskPremiumStr = Util.DoubleToString(entity.RiskPremium, 2),
                LevyStr = Util.DoubleToString(entity.Levy, 2),
                ProfitCommStr = Util.DoubleToString(entity.ProfitComm, 2),
                ClaimStr = Util.DoubleToString(entity.Claim, 2),
                SurrenderValueStr = Util.DoubleToString(entity.SurrenderValue, 2),
                NoClaimBonusStr = Util.DoubleToString(entity.NoClaimBonus, 2),
                DatabaseCommissionStr = Util.DoubleToString(entity.DatabaseCommission, 2),
                BrokerageFeeStr = Util.DoubleToString(entity.BrokerageFee, 2),
                ServiceFeeStr = Util.DoubleToString(entity.ServiceFee, 2),
                GstStr = Util.DoubleToString(entity.Gst, 2),
                ModcoReserveIncomeStr = Util.DoubleToString(entity.ModcoReserveIncome, 2),
                RiDepositStr = Util.DoubleToString(entity.RiDeposit, 2),
                AdministrationContributionStr = Util.DoubleToString(entity.AdministrationContribution, 2),
                ShareOfRiCommissionFromCompulsoryCessionStr = Util.DoubleToString(entity.ShareOfRiCommissionFromCompulsoryCession, 2),
                RecaptureFeeStr = Util.DoubleToString(entity.RecaptureFee, 2),
                CreditCardChargesStr = Util.DoubleToString(entity.CreditCardCharges, 2),
                NetTotalAmountStr = Util.DoubleToString(entity.NetTotalAmount, 2),

                CurrencyCode = entity.CurrencyCode,
                CurrencyRate = entity.CurrencyRate,
                CurrencyRateStr = Util.DoubleToString(entity.CurrencyRate, 5),

                DTHStr = Util.DoubleToString(entity.DTH, 2),
                TPAStr = Util.DoubleToString(entity.TPA, 2),
                TPSStr = Util.DoubleToString(entity.TPS, 2),
                PPDStr = Util.DoubleToString(entity.PPD, 2),
                CCAStr = Util.DoubleToString(entity.CCA, 2),
                CCSStr = Util.DoubleToString(entity.CCS, 2),
                PAStr = Util.DoubleToString(entity.PA, 2),
                HSStr = Util.DoubleToString(entity.HS, 2),
                TPDStr = Util.DoubleToString(entity.TPD, 2),
                CIStr = Util.DoubleToString(entity.CI, 2),

                NbSarStr = Util.DoubleToString(entity.NbSar, 2),
                RnSarStr = Util.DoubleToString(entity.RnSar, 2),
                AltSarStr = Util.DoubleToString(entity.AltSar, 2),
            };
            if (foreign)
            {
                bo.SoaDataBatchBo = SoaDataBatchService.Find(entity.SoaDataBatchId);
                bo.CreatedByBo = UserService.Find(entity.CreatedById);
            }
            bo.GetTotalPremium();

            return bo;
        }

        public static SoaDataCompiledSummary FormEntity(SoaDataCompiledSummaryBo bo = null)
        {
            if (bo == null)
                return null;
            return new SoaDataCompiledSummary
            {
                Id = bo.Id,
                SoaDataBatchId = bo.SoaDataBatchId,
                InvoiceType = bo.InvoiceType,
                InvoiceDate = bo.InvoiceDate,
                StatementReceivedDate = bo.StatementReceivedDate,

                BusinessCode = bo.BusinessCode,
                TreatyCode = bo.TreatyCode,
                SoaQuarter = bo.SoaQuarter,
                RiskQuarter = bo.RiskQuarter,
                AccountDescription = bo.AccountDescription,

                NbPremium = bo.NbPremium,
                RnPremium = bo.RnPremium,
                AltPremium = bo.AltPremium,

                NbDiscount = bo.NbDiscount,
                RnDiscount = bo.RnDiscount,
                AltDiscount = bo.AltDiscount,

                TotalDiscount = bo.TotalDiscount,
                RiskPremium = bo.RiskPremium,
                NoClaimBonus = bo.NoClaimBonus,
                Levy = bo.Levy,
                Claim = bo.Claim,
                ProfitComm = bo.ProfitComm,
                SurrenderValue = bo.SurrenderValue,
                Gst = bo.Gst,
                ModcoReserveIncome = bo.ModcoReserveIncome,
                RiDeposit = bo.RiDeposit,
                DatabaseCommission = bo.DatabaseCommission,
                AdministrationContribution = bo.AdministrationContribution,
                ShareOfRiCommissionFromCompulsoryCession = bo.ShareOfRiCommissionFromCompulsoryCession,
                RecaptureFee = bo.RecaptureFee,
                CreditCardCharges = bo.CreditCardCharges,
                BrokerageFee = bo.BrokerageFee,
                NetTotalAmount = bo.NetTotalAmount,

                ReasonOfAdjustment1 = bo.ReasonOfAdjustment1,
                ReasonOfAdjustment2 = bo.ReasonOfAdjustment2,
                InvoiceNumber1 = bo.InvoiceNumber1,
                InvoiceDate1 = bo.InvoiceDate1,
                Amount1 = bo.Amount1,
                InvoiceNumber2 = bo.InvoiceNumber2,
                InvoiceDate2 = bo.InvoiceDate2,
                Amount2 = bo.Amount2,
                FilingReference = bo.FilingReference,
                ServiceFeePercentage = bo.ServiceFeePercentage,
                ServiceFee = bo.ServiceFee,
                Sst = bo.Sst,
                TotalAmount = bo.TotalAmount,

                NbCession = bo.NbCession,
                RnCession = bo.RnCession,
                AltCession = bo.AltCession,

                NbSar = bo.NbSar,
                RnSar = bo.RnSar,
                AltSar = bo.AltSar,

                DTH = bo.DTH,
                TPA = bo.TPA,
                TPS = bo.TPS,
                PPD = bo.PPD,
                CCA = bo.CCA,
                CCS = bo.CCS,
                PA = bo.PA,
                HS = bo.HS,
                TPD = bo.TPD,
                CI = bo.CI,

                ContractCode = bo.ContractCode,
                AnnualCohort = bo.AnnualCohort,
                CurrencyCode = bo.CurrencyCode,
                CurrencyRate = bo.CurrencyRate,
                Mfrs17CellName = bo.Mfrs17CellName,
                Frequency = bo.Frequency,
                ReportingType = bo.ReportingType,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static SoaDataCompiledSummaryBo FormBoForCompiledSummaryTab(SoaDataCompiledSummary entity = null, bool foreign = false)
        {
            if (entity == null)
                return null;
            var bo = new SoaDataCompiledSummaryBo
            {
                Id = entity.Id,
                SoaDataBatchId = entity.SoaDataBatchId,
                InvoiceType = entity.InvoiceType,

                BusinessCode = entity.BusinessCode,
                TreatyCode = entity.TreatyCode,
                SoaQuarter = entity.SoaQuarter,
                RiskQuarter = entity.RiskQuarter,
                AccountDescription = string.IsNullOrEmpty(entity.AccountDescription) ? "" : entity.AccountDescription,

                ReasonOfAdjustment1 = string.IsNullOrEmpty(entity.ReasonOfAdjustment1) ? "" : entity.ReasonOfAdjustment1,
                ReasonOfAdjustment2 = string.IsNullOrEmpty(entity.ReasonOfAdjustment2) ? "" : entity.ReasonOfAdjustment2,
                InvoiceNumber1 = entity.InvoiceNumber1,
                InvoiceNumber2 = entity.InvoiceNumber2,
                FilingReference = entity.FilingReference,
                ServiceFeePercentage = entity.ServiceFeePercentage,
                ServiceFee = entity.ServiceFee,
                Sst = entity.Sst,

                NbCession = entity.NbCession,
                RnCession = entity.RnCession,
                AltCession = entity.AltCession,

                ContractCode = entity.ContractCode,
                AnnualCohort = entity.AnnualCohort,
                //Frequency = entity.Frequency,
                //Mfrs17CellName = entity.Mfrs17CellName,
                ReportingType = entity.ReportingType,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                StatementReceivedDateStr = entity.StatementReceivedDate?.ToString(Util.GetDateFormat()),
                InvoiceDate1Str = entity.InvoiceDate1?.ToString(Util.GetDateFormat()),
                Amount1Str = Util.DoubleToString(entity.Amount1, 2),
                InvoiceDate2Str = entity.InvoiceDate2?.ToString(Util.GetDateFormat()),
                Amount2Str = Util.DoubleToString(entity.Amount2, 2),

                NbPremiumStr = Util.DoubleToString(entity.NbPremium, 2),
                RnPremiumStr = Util.DoubleToString(entity.RnPremium, 2),
                AltPremiumStr = Util.DoubleToString(entity.AltPremium, 2),

                NbDiscountStr = Util.DoubleToString(entity.NbDiscount, 2),
                RnDiscountStr = Util.DoubleToString(entity.RnDiscount, 2),
                AltDiscountStr = Util.DoubleToString(entity.AltDiscount, 2),

                TotalDiscountStr = Util.DoubleToString(entity.TotalDiscount, 2),
                RiskPremiumStr = Util.DoubleToString(entity.RiskPremium, 2),
                LevyStr = Util.DoubleToString(entity.Levy, 2),
                ProfitCommStr = Util.DoubleToString(entity.ProfitComm, 2),
                ClaimStr = Util.DoubleToString(entity.Claim, 2),
                SurrenderValueStr = Util.DoubleToString(entity.SurrenderValue, 2),
                NoClaimBonusStr = Util.DoubleToString(entity.NoClaimBonus, 2),
                DatabaseCommissionStr = Util.DoubleToString(entity.DatabaseCommission, 2),
                BrokerageFeeStr = Util.DoubleToString(entity.BrokerageFee, 2),
                ServiceFeeStr = Util.DoubleToString(entity.ServiceFee, 2),
                GstStr = Util.DoubleToString(entity.Gst, 2),
                ModcoReserveIncomeStr = Util.DoubleToString(entity.ModcoReserveIncome, 2),
                RiDepositStr = Util.DoubleToString(entity.RiDeposit, 2),
                AdministrationContributionStr = Util.DoubleToString(entity.AdministrationContribution, 2),
                ShareOfRiCommissionFromCompulsoryCessionStr = Util.DoubleToString(entity.ShareOfRiCommissionFromCompulsoryCession, 2),
                RecaptureFeeStr = Util.DoubleToString(entity.RecaptureFee, 2),
                CreditCardChargesStr = Util.DoubleToString(entity.CreditCardCharges, 2),
                NetTotalAmountStr = Util.DoubleToString(entity.NetTotalAmount, 2),
                ServiceFeePercentageStr = Util.DoubleToString(entity.ServiceFeePercentage, 2),
                SstStr = Util.DoubleToString(entity.Sst, 2),

                CurrencyCode = entity.CurrencyCode,
                CurrencyRateStr = Util.DoubleToString(entity.CurrencyRate, 5),

                DTHStr = Util.DoubleToString(entity.DTH, 2),
                TPAStr = Util.DoubleToString(entity.TPA, 2),
                TPSStr = Util.DoubleToString(entity.TPS, 2),
                PPDStr = Util.DoubleToString(entity.PPD, 2),
                CCAStr = Util.DoubleToString(entity.CCA, 2),
                CCSStr = Util.DoubleToString(entity.CCS, 2),
                PAStr = Util.DoubleToString(entity.PA, 2),
                HSStr = Util.DoubleToString(entity.HS, 2),
                TPDStr = Util.DoubleToString(entity.TPD, 2),
                CIStr = Util.DoubleToString(entity.CI, 2),

                NbSarStr = Util.DoubleToString(entity.NbSar, 2),
                RnSarStr = Util.DoubleToString(entity.RnSar, 2),
                AltSarStr = Util.DoubleToString(entity.AltSar, 2),
            };
            if (foreign)
            {
                bo.SoaDataBatchBo = SoaDataBatchService.Find(entity.SoaDataBatchId);
                bo.CreatedByBo = UserService.Find(entity.CreatedById);
            }
            bo.TotalAmountStr = Util.DoubleToString(bo.GetTotalPremium(), 2);
            bo.CreatedByPerson = UserService.Find(entity.CreatedById)?.FullName;

            return bo;
        }

        public static IList<SoaDataCompiledSummaryBo> FormBos(IList<SoaDataCompiledSummary> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            var bos = new List<SoaDataCompiledSummaryBo>() { };
            foreach (SoaDataCompiledSummary entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static IList<SoaDataCompiledSummaryBo> FormBosForCompiledSummaryTab(IList<SoaDataCompiledSummary> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            var bos = new List<SoaDataCompiledSummaryBo>() { };
            foreach (SoaDataCompiledSummary entity in entities)
            {
                bos.Add(FormBoForCompiledSummaryTab(entity, foreign));
            }
            return bos;
        }

        public static bool IsExists(int id)
        {
            return SoaDataCompiledSummary.IsExists(id);
        }

        public static SoaDataCompiledSummaryBo Find(int id)
        {
            return FormBo(SoaDataCompiledSummary.Find(id));
        }

        public static IList<SoaDataCompiledSummaryBo> GetBySoaDataBatchIdCode(int soaDataBatchId, int reportingType, bool retakaful, bool originalCurrency = false)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.SoaDataCompiledSummaries.Where(q => q.SoaDataBatchId == soaDataBatchId)
                    .Where(q => q.ReportingType == reportingType);

                if (retakaful)
                    query = query.Where(q => q.BusinessCode == PickListDetailBo.BusinessOriginCodeServiceFee);
                else
                    query = query.Where(q => q.BusinessCode != PickListDetailBo.BusinessOriginCodeServiceFee);

                if (originalCurrency)
                    query = query.Where(q => !string.IsNullOrEmpty(q.CurrencyCode) && q.CurrencyCode != PickListDetailBo.CurrencyCodeMyr);
                else
                    query = query.Where(q => string.IsNullOrEmpty(q.CurrencyCode) || q.CurrencyCode == PickListDetailBo.CurrencyCodeMyr);

                return FormBosForCompiledSummaryTab(query.ToList());
            }
        }

        public static IList<SoaDataCompiledSummaryBo> GetBySoaDataBatchIdsCode(List<int> soaDataBatchIds, int reportingType, bool retakaful, bool originalCurrency = false)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.SoaDataCompiledSummaries.Where(q => soaDataBatchIds.Contains(q.SoaDataBatchId))
                    .Where(q => q.ReportingType == reportingType);

                if (retakaful)
                    query = query.Where(q => q.BusinessCode == PickListDetailBo.BusinessOriginCodeServiceFee);
                else
                    query = query.Where(q => q.BusinessCode != PickListDetailBo.BusinessOriginCodeServiceFee);

                if (originalCurrency)
                    query = query.Where(q => !string.IsNullOrEmpty(q.CurrencyCode) && q.CurrencyCode != PickListDetailBo.CurrencyCodeMyr);
                else
                    query = query.Where(q => string.IsNullOrEmpty(q.CurrencyCode) || q.CurrencyCode == PickListDetailBo.CurrencyCodeMyr);

                return FormBos(query.ToList());
            }
        }

        public static IList<SoaDataCompiledSummaryBo> GetBySoaDataBatchIds(List<int> soaDataBatchIds, int reportingType, bool originalCurrency = false)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.SoaDataCompiledSummaries.Where(q => soaDataBatchIds.Contains(q.SoaDataBatchId))
                    .Where(q => q.ReportingType == reportingType);

                if (originalCurrency)
                    query = query.Where(q => !string.IsNullOrEmpty(q.CurrencyCode) && q.CurrencyCode != PickListDetailBo.CurrencyCodeMyr);
                else
                    query = query.Where(q => string.IsNullOrEmpty(q.CurrencyCode) || q.CurrencyCode == PickListDetailBo.CurrencyCodeMyr);

                return FormBos(query.ToList());
            }
        }

        public static IList<SoaDataCompiledSummaryBo> GetBySoaDataBatchId(int soaDataBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBos(
                    db.SoaDataCompiledSummaries
                    .Where(q => q.SoaDataBatchId == soaDataBatchId)
                    .ToList()
                );
            }
        }

        public static IList<string> GetTreatyCodeBySoaDataBatchId(int soaDataBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                return db.SoaDataCompiledSummaries
                    .Where(q => q.SoaDataBatchId == soaDataBatchId)
                    .Select(q => q.TreatyCode)
                    .Distinct()
                    .ToList();
            }
        }

        public static double? TotalPremiumBySoaDataBatchIdCode(int soaDataBatchId, int reportingType, bool retakaful, bool originalCurrency = false)
        {
            double premiumAmount = 0;

            using (var db = new AppDbContext(false))
            {
                var query = db.SoaDataCompiledSummaries.Where(q => q.SoaDataBatchId == soaDataBatchId)
                    .Where(q => q.ReportingType == reportingType);

                if (retakaful)
                    query = query.Where(q => q.BusinessCode == PickListDetailBo.BusinessOriginCodeServiceFee);
                else
                    query = query.Where(q => q.BusinessCode != PickListDetailBo.BusinessOriginCodeServiceFee);

                if (originalCurrency)
                    query = query.Where(q => !string.IsNullOrEmpty(q.CurrencyCode) && q.CurrencyCode != PickListDetailBo.CurrencyCodeMyr);
                else
                    query = query.Where(q => string.IsNullOrEmpty(q.CurrencyCode) || q.CurrencyCode == PickListDetailBo.CurrencyCodeMyr);

                var bo = query.ToList();
                premiumAmount += bo.Sum(q => q.NbPremium) ?? 0;
                premiumAmount += bo.Sum(q => q.RnPremium) ?? 0;
                premiumAmount += bo.Sum(q => q.AltPremium) ?? 0;

                return premiumAmount;
            }
        }

        public static Result Save(ref SoaDataCompiledSummaryBo bo)
        {
            if (!SoaDataCompiledSummary.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref SoaDataCompiledSummaryBo bo, ref TrailObject trail)
        {
            if (!SoaDataCompiledSummary.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref SoaDataCompiledSummaryBo bo)
        {
            var entity = FormEntity(bo);
            var result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }
            return result;
        }

        public static Result Create(ref SoaDataCompiledSummaryBo bo, ref TrailObject trail)
        {
            var result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref SoaDataCompiledSummaryBo bo)
        {
            var result = Result();
            var entity = SoaDataCompiledSummary.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (result.Valid)
            {
                entity.SoaDataBatchId = bo.SoaDataBatchId;
                entity.InvoiceType = bo.InvoiceType;
                entity.InvoiceDate = bo.InvoiceDate;
                entity.StatementReceivedDate = bo.StatementReceivedDate;

                entity.BusinessCode = bo.BusinessCode;
                entity.TreatyCode = bo.TreatyCode;
                entity.SoaQuarter = bo.SoaQuarter;
                entity.RiskQuarter = bo.RiskQuarter;
                entity.AccountDescription = bo.AccountDescription;

                entity.NbPremium = bo.NbPremium;
                entity.RnPremium = bo.RnPremium;
                entity.AltPremium = bo.AltPremium;

                entity.NbDiscount = bo.NbDiscount;
                entity.RnDiscount = bo.RnDiscount;
                entity.AltDiscount = bo.AltDiscount;

                entity.TotalDiscount = bo.TotalDiscount;
                entity.RiskPremium = bo.RiskPremium;
                entity.NoClaimBonus = bo.NoClaimBonus;
                entity.Levy = bo.Levy;
                entity.Claim = bo.Claim;
                entity.ProfitComm = bo.ProfitComm;
                entity.SurrenderValue = bo.SurrenderValue;
                entity.Gst = bo.Gst;
                entity.ModcoReserveIncome = bo.ModcoReserveIncome;
                entity.RiDeposit = bo.RiDeposit;
                entity.DatabaseCommission = bo.DatabaseCommission;
                entity.AdministrationContribution = bo.AdministrationContribution;
                entity.ShareOfRiCommissionFromCompulsoryCession = bo.ShareOfRiCommissionFromCompulsoryCession;
                entity.RecaptureFee = bo.RecaptureFee;
                entity.CreditCardCharges = bo.CreditCardCharges;
                entity.BrokerageFee = bo.BrokerageFee;
                entity.NetTotalAmount = bo.NetTotalAmount;

                entity.ReasonOfAdjustment1 = bo.ReasonOfAdjustment1;
                entity.ReasonOfAdjustment2 = bo.ReasonOfAdjustment2;
                entity.InvoiceNumber1 = bo.InvoiceNumber1;
                entity.InvoiceDate1 = bo.InvoiceDate1;
                entity.Amount1 = bo.Amount1;
                entity.InvoiceNumber2 = bo.InvoiceNumber2;
                entity.InvoiceDate2 = bo.InvoiceDate2;
                entity.Amount2 = bo.Amount2;
                entity.FilingReference = bo.FilingReference;
                entity.ServiceFeePercentage = bo.ServiceFeePercentage;
                entity.ServiceFee = bo.ServiceFee;
                entity.Sst = bo.Sst;
                entity.TotalAmount = bo.TotalAmount;

                entity.NbCession = bo.NbCession;
                entity.RnCession = bo.RnCession;
                entity.AltCession = bo.AltCession;

                entity.NbSar = bo.NbSar;
                entity.RnSar = bo.RnSar;
                entity.AltSar = bo.AltSar;

                entity.DTH = bo.DTH;
                entity.TPA = bo.TPA;
                entity.TPS = bo.TPS;
                entity.PPD = bo.PPD;
                entity.CCA = bo.CCA;
                entity.CCS = bo.CCS;
                entity.PA = bo.PA;
                entity.HS = bo.HS;
                entity.TPD = bo.TPD;
                entity.CI = bo.CI;

                entity.ContractCode = bo.ContractCode;
                entity.AnnualCohort = bo.AnnualCohort;
                entity.CurrencyCode = bo.CurrencyCode;
                entity.CurrencyRate = bo.CurrencyRate;
                entity.Mfrs17CellName = bo.Mfrs17CellName;
                entity.Frequency = bo.Frequency;
                entity.ReportingType = bo.ReportingType;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref SoaDataCompiledSummaryBo bo, ref TrailObject trail)
        {
            var result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(SoaDataCompiledSummaryBo bo)
        {
            SoaDataCompiledSummary.Delete(bo.Id);
        }

        public static Result Delete(SoaDataCompiledSummaryBo bo, ref TrailObject trail)
        {
            var result = Result();
            var dataTrail = SoaDataCompiledSummary.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);
            return result;
        }

        public static IList<DataTrail> DeleteBySoaDataBatchId(int soaDataBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                var trails = new List<DataTrail>();
                //var query = db.SoaDataCompiledSummaries.Where(q => q.SoaDataBatchId == soaDataBatchId);
                // DO NOT TRAIL since this is mass data
                /*
                foreach (var soaDataCompiledSummary in query.ToList())
                {
                    var trail = new DataTrail(soaDataCompiledSummary, true);
                    trails.Add(trail);

                    db.Entry(soaDataCompiledSummary).State = EntityState.Deleted;
                    db.SoaDataCompiledSummaries.Remove(soaDataCompiledSummary);
                }
                */
                //db.SoaDataCompiledSummaries.RemoveRange(query);

                db.Database.ExecuteSqlCommand("DELETE FROM [SoaDataCompiledSummaries] WHERE [SoaDataBatchId] = {0}", soaDataBatchId);
                db.SaveChanges();

                return trails;
            }
        }
    }
}
