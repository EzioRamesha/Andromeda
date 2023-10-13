using BusinessObject;
using BusinessObject.InvoiceRegisters;
using DataAccess.Entities.InvoiceRegisters;
using DataAccess.EntityFramework;
using Services.Identity;
using Services.SoaDatas;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.InvoiceRegisters
{
    public class InvoiceRegisterService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(InvoiceRegister)),
                //Controller = ModuleBo.ModuleController.InvoiceRegister.ToString()
            };
        }

        public static Expression<Func<InvoiceRegister, InvoiceRegisterBo>> Expression()
        {
            return entity => new InvoiceRegisterBo
            {
                Id = entity.Id,
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

                TreatyTypeId = entity.TreatyCode.TreatyTypePickListDetailId,
                LobId = entity.TreatyCode.Treaty.BusinessOriginPickListDetailId,
                Status = entity.SoaDataBatch.InvoiceStatus,

                ContractCode = entity.ContractCode,
                AnnualCohort = entity.AnnualCohort,
                ReportingType = entity.ReportingType,
            };
        }
        public static InvoiceRegisterBo FormBo(InvoiceRegister entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            var bo = new InvoiceRegisterBo
            {
                Id = entity.Id,
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
                Mfrs17CellName = entity.Mfrs17CellName,
            };
            if (foreign)
            {
                bo.InvoiceRegisterBatchBo = InvoiceRegisterBatchService.Find(entity.InvoiceRegisterBatchId);
                bo.CedantBo = CedantService.Find(entity.CedantId);
                bo.TreatyCodeBo = TreatyCodeService.Find(entity.TreatyCodeId);
                bo.PreparedByBo = UserService.Find(entity.PreparedById);
                bo.SoaDataBatchBo = SoaDataBatchService.Find(entity.SoaDataBatchId);
            }

            if (!string.IsNullOrEmpty(bo.ContractCode))
            {
                var modifiedContractCode = bo.ContractCode;
                if (bo.ContractCode.Length > 3)
                    modifiedContractCode = modifiedContractCode.Substring(0, modifiedContractCode.Length - 3);

                bo.ModifiedContractCode = modifiedContractCode;

                var accountCodeMappingBo = AccountCodeMappingService.FindCedantAccountCodeByModifiedContractCode(modifiedContractCode);
                bo.AccountCode = (accountCodeMappingBo == null ? "" : accountCodeMappingBo.AccountCodeBo.Code);
            }

            return bo;
        }

        public static IList<InvoiceRegisterBo> FormBos(IList<InvoiceRegister> entities = null)
        {
            if (entities == null)
                return null;
            IList<InvoiceRegisterBo> bos = new List<InvoiceRegisterBo>() { };
            foreach (InvoiceRegister entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static InvoiceRegister FormEntity(InvoiceRegisterBo bo = null)
        {
            if (bo == null)
                return null;
            return new InvoiceRegister
            {
                Id = bo.Id,
                InvoiceRegisterBatchId = bo.InvoiceRegisterBatchId,
                InvoiceType = bo.InvoiceType,
                InvoiceReference = bo.InvoiceReference,
                InvoiceNumber = bo.InvoiceNumber,
                InvoiceDate = bo.InvoiceDate,
                StatementReceivedDate = bo.StatementReceivedDate,
                CedantId = bo.CedantId,
                RiskQuarter = bo.RiskQuarter,
                TreatyCodeId = bo.TreatyCodeId,
                AccountDescription = bo.AccountDescription,
                TotalPaid = bo.TotalPaid,
                PaymentReference = bo.PaymentReference,
                PaymentAmount = bo.PaymentAmount,
                PaymentReceivedDate = bo.PaymentReceivedDate,

                Year1st = bo.Year1st,
                Renewal = bo.Renewal,
                Gross1st = bo.Gross1st,
                GrossRenewal = bo.GrossRenewal,
                AltPremium = bo.AltPremium,
                Discount1st = bo.Discount1st,
                DiscountRen = bo.DiscountRen,
                DiscountAlt = bo.DiscountAlt,

                RiskPremium = bo.RiskPremium,
                NoClaimBonus = bo.NoClaimBonus,
                Levy = bo.Levy,
                Claim = bo.Claim,
                ProfitComm = bo.ProfitComm,
                SurrenderValue = bo.SurrenderValue,
                Gst = bo.Gst,
                ModcoReserveIncome = bo.ModcoReserveIncome,
                ReinsDeposit = bo.ReinsDeposit,
                DatabaseCommission = bo.DatabaseCommission,
                AdministrationContribution = bo.AdministrationContribution,
                ShareOfRiCommissionFromCompulsoryCession = bo.ShareOfRiCommissionFromCompulsoryCession,
                RecaptureFee = bo.RecaptureFee,
                CreditCardCharges = bo.CreditCardCharges,
                BrokerageFee = bo.BrokerageFee,

                NBCession = bo.NBCession,
                NBSumReins = bo.NBSumReins,
                RNCession = bo.RNCession,
                RNSumReins = bo.RNSumReins,
                AltCession = bo.AltCession,
                AltSumReins = bo.AltSumReins,

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

                Frequency = bo.Frequency,
                PreparedById = bo.PreparedById,
                CurrencyCode = bo.CurrencyCode,
                CurrencyRate = bo.CurrencyRate,
                SoaQuarter = bo.SoaQuarter,

                ValuationGross1st = bo.ValuationGross1st,
                ValuationGrossRen = bo.ValuationGrossRen,
                ValuationDiscount1st = bo.ValuationDiscount1st,
                ValuationDiscountRen = bo.ValuationDiscountRen,
                ValuationCom1st = bo.ValuationCom1st,
                ValuationComRen = bo.ValuationComRen,
                ValuationClaims = bo.ValuationClaims,
                ValuationProfitCom = bo.ValuationProfitCom,
                ValuationMode = bo.ValuationMode,
                ValuationRiskPremium = bo.ValuationRiskPremium,

                Remark = bo.Remark,
                ReasonOfAdjustment1 = bo.ReasonOfAdjustment1,
                ReasonOfAdjustment2 = bo.ReasonOfAdjustment2,
                InvoiceNumber1 = bo.InvoiceNumber1,
                InvoiceDate1 = bo.InvoiceDate1,
                Amount1 = bo.Amount1,
                InvoiceNumber2 = bo.InvoiceNumber2,
                InvoiceDate2 = bo.InvoiceDate2,
                Amount2 = bo.Amount2,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,

                SoaDataBatchId = bo.SoaDataBatchId,

                ContractCode = bo.ContractCode,
                AnnualCohort = bo.AnnualCohort,
                ReportingType = bo.ReportingType,
                Mfrs17CellName = bo.Mfrs17CellName,
            };
        }

        public static bool IsExists(int id)
        {
            return InvoiceRegister.IsExists(id);
        }

        public static InvoiceRegisterBo Find(int id)
        {
            return FormBo(InvoiceRegister.Find(id));
        }

        public static IList<InvoiceRegisterBo> Get()
        {
            return FormBos(InvoiceRegister.Get());
        }

        public static IList<InvoiceRegisterBo> GetByInvoiceRegisterBatchId(int invoiceRegisterBatchId, int reportingType, bool originalCurrency)
        {
            using (var db = new AppDbContext())
            {
                var query = db.InvoiceRegisters.Where(q => q.InvoiceRegisterBatchId == invoiceRegisterBatchId)
                    .Where(q => q.ReportingType == reportingType);

                if (originalCurrency)
                    query = query.Where(q => !string.IsNullOrEmpty(q.CurrencyCode) && q.CurrencyCode != PickListDetailBo.CurrencyCodeMyr);
                else
                    query = query.Where(q => string.IsNullOrEmpty(q.CurrencyCode) || q.CurrencyCode == PickListDetailBo.CurrencyCodeMyr);

                return FormBos(query.OrderBy(q => q.TreatyCode.Description).ToList());
            }
        }

        public static IList<InvoiceRegisterBo> GetByBatchIdInvoiceTypeCurrencyCode(int invoiceRegisterBatchId, int treatyCodeId, int type, int skip = 0, int take = 50)
        {
            using (var db = new AppDbContext())
            {
                List<int> OMtype = new List<int> { InvoiceRegisterBo.InvoiceTypeOM, InvoiceRegisterBo.InvoiceTypeCNOM, InvoiceRegisterBo.InvoiceTypeDNOM };

                var query = db.InvoiceRegisters
                    .Where(q => q.InvoiceRegisterBatchId == invoiceRegisterBatchId)
                    .Where(q => q.TreatyCodeId == treatyCodeId)
                    .Where(q => q.InvoiceType == type);

                if (OMtype.Contains(type))
                    query = query.Where(q => string.IsNullOrEmpty(q.CurrencyCode) || q.CurrencyCode == PickListDetailBo.CurrencyCodeMyr);

                return FormBos(query.OrderBy(q => q.Id).Skip(skip).Take(take).ToList());
            }
        }

        public static IList<int> GetTreatyCodeByBatchIdInvoiceType(int invoiceRegisterBatchId, int type)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisters
                    .Where(q => q.InvoiceRegisterBatchId == invoiceRegisterBatchId)
                    .Where(q => q.InvoiceType == type)
                    .GroupBy(q => q.TreatyCodeId)
                    .Select(q => q.Key.Value)
                    .ToList();
            }
        }

        public static IList<int> GetTreatyCodeByBatchIdInvoiceTypeCurrencyCode(int invoiceRegisterBatchId, int type)
        {
            using (var db = new AppDbContext())
            {
                List<int> OMtype = new List<int> { InvoiceRegisterBo.InvoiceTypeOM, InvoiceRegisterBo.InvoiceTypeCNOM, InvoiceRegisterBo.InvoiceTypeDNOM };

                var query = db.InvoiceRegisters
                    .Where(q => q.InvoiceRegisterBatchId == invoiceRegisterBatchId)
                    .Where(q => q.InvoiceType == type);

                if (OMtype.Contains(type))
                    query = query.Where(q => string.IsNullOrEmpty(q.CurrencyCode) || q.CurrencyCode == PickListDetailBo.CurrencyCodeMyr);

                return query.GroupBy(q => q.TreatyCodeId).Select(q => q.Key.Value).ToList();
            }
        }

        public static int CountByBatchIdInvoiceTypeCurrencyCode(int invoiceRegisterBatchId, int treatyCodeId, int type)
        {
            using (var db = new AppDbContext())
            {
                List<int> OMtype = new List<int> { InvoiceRegisterBo.InvoiceTypeOM, InvoiceRegisterBo.InvoiceTypeCNOM, InvoiceRegisterBo.InvoiceTypeDNOM };

                var query = db.InvoiceRegisters
                    .Where(q => q.InvoiceRegisterBatchId == invoiceRegisterBatchId)
                    .Where(q => q.TreatyCodeId == treatyCodeId)
                    .Where(q => q.InvoiceType == type);

                if (OMtype.Contains(type))
                    query = query.Where(q => string.IsNullOrEmpty(q.CurrencyCode) || q.CurrencyCode == PickListDetailBo.CurrencyCodeMyr);

                return query.Count();
            }
        }

        public static int CountByInvoiceRegisterBatchId(int invoiceRegisterBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisters.Where(q => q.InvoiceRegisterBatchId == invoiceRegisterBatchId).Count();
            }
        }

        public static int CountCurrentInvoiceRegisterByInvoiceDate(int year)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisters.Where(q => q.InvoiceDate.Year == year)
                    .Where(q => !string.IsNullOrEmpty(q.InvoiceReference))
                    .Where(q => string.IsNullOrEmpty(q.CurrencyCode) || q.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                    .Count();
            }
        }

        public static string GetNextReferenceNo(int year)
        {
            using (var db = new AppDbContext())
            {
                string prefix = string.Format("{0}-", year);

                var invoiceRegister = db.InvoiceRegisters.Where(q => q.InvoiceDate.Year == year)
                     .Where(q => string.IsNullOrEmpty(q.CurrencyCode) || q.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                    .Where(q => !string.IsNullOrEmpty(q.InvoiceReference) && q.InvoiceReference.StartsWith(prefix))
                    .OrderByDescending(a => a.InvoiceReference.Length)
                    .ThenByDescending(a => a.InvoiceReference)
                    .FirstOrDefault();

                int count = 0;
                if (invoiceRegister != null)
                {
                    string referenceNo = invoiceRegister.InvoiceReference;
                    string[] referenceNoInfo = referenceNo.Split('-');

                    if (referenceNoInfo.Length == 2)
                    {
                        string countStr = referenceNoInfo[1];
                        int.TryParse(countStr, out count);
                    }
                }
                count++;
                string nextCountStr = count.ToString();

                return prefix + nextCountStr;
            }
        }

        public static InvoiceRegisterBo FindByLookupParams(int invoiceRegisterBatchId, int type, string riskQuarter, int treatyCodeId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(
                    db.InvoiceRegisters
                    .Where(q => q.InvoiceRegisterBatchId == invoiceRegisterBatchId)
                    .Where(q => q.InvoiceType == type)
                    .Where(q => q.RiskQuarter == riskQuarter)
                    .Where(q => q.TreatyCodeId == treatyCodeId)
                    .FirstOrDefault());
            }
        }

        public static InvoiceRegisterBo FindByInvoiceReferenceIfrs4(string referenceNo, bool originalCurrency = false)
        {
            using (var db = new AppDbContext())
            {
                var query = db.InvoiceRegisters.Where(q => q.InvoiceReference == referenceNo)
                    .Where(q => q.ReportingType == InvoiceRegisterBo.ReportingTypeIFRS4);

                if (originalCurrency)
                    query = query.Where(q => !string.IsNullOrEmpty(q.CurrencyCode) && q.CurrencyCode != PickListDetailBo.CurrencyCodeMyr);
                else
                    query = query.Where(q => q.CurrencyCode == PickListDetailBo.CurrencyCodeMyr);

                return FormBo(query.FirstOrDefault());
            }
        }
        public static IList<InvoiceRegisterBo> FindByInvoiceReferenceIfrs17(string referenceNo, int reportingTypeIFRS17)
        {
            using (var db = new AppDbContext())
            {
                var query = db.InvoiceRegisters.Where(q => q.InvoiceReference == referenceNo)
                    .Where(q => q.ReportingType == reportingTypeIFRS17);

                return FormBos(query.ToList());
            }
        }

        public static IList<InvoiceRegisterBo> InvoiceRegisterReportParams(int reportingType)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.InvoiceRegisters
                    .Where(q => q.ReportingType == reportingType);

                return FormBos(query.ToList());
            }
        }

        public static Result Save(ref InvoiceRegisterBo bo)
        {
            if (!InvoiceRegister.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref InvoiceRegisterBo bo, ref TrailObject trail)
        {
            if (!InvoiceRegister.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref InvoiceRegisterBo bo)
        {
            InvoiceRegister entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref InvoiceRegisterBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref InvoiceRegisterBo bo)
        {
            Result result = Result();

            InvoiceRegister entity = InvoiceRegister.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.InvoiceRegisterBatchId = bo.InvoiceRegisterBatchId;
                entity.InvoiceType = bo.InvoiceType;
                entity.InvoiceReference = bo.InvoiceReference;
                entity.InvoiceNumber = bo.InvoiceNumber;
                entity.InvoiceDate = bo.InvoiceDate;
                entity.StatementReceivedDate = bo.StatementReceivedDate;
                entity.CedantId = bo.CedantId;
                entity.RiskQuarter = bo.RiskQuarter;
                entity.TreatyCodeId = bo.TreatyCodeId;
                entity.AccountDescription = bo.AccountDescription;
                entity.TotalPaid = bo.TotalPaid;
                entity.PaymentReference = bo.PaymentReference;
                entity.PaymentAmount = bo.PaymentAmount;
                entity.PaymentReceivedDate = bo.PaymentReceivedDate;

                entity.Year1st = bo.Year1st;
                entity.Renewal = bo.Renewal;
                entity.Gross1st = bo.Gross1st;
                entity.GrossRenewal = bo.GrossRenewal;
                entity.AltPremium = bo.AltPremium;
                entity.Discount1st = bo.Discount1st;
                entity.DiscountRen = bo.DiscountRen;
                entity.DiscountAlt = bo.DiscountAlt;

                entity.RiskPremium = bo.RiskPremium;
                entity.NoClaimBonus = bo.NoClaimBonus;
                entity.Levy = bo.Levy;
                entity.Claim = bo.Claim;
                entity.ProfitComm = bo.ProfitComm;
                entity.SurrenderValue = bo.SurrenderValue;
                entity.Gst = bo.Gst;
                entity.ModcoReserveIncome = bo.ModcoReserveIncome;
                entity.ReinsDeposit = bo.ReinsDeposit;
                entity.DatabaseCommission = bo.DatabaseCommission;
                entity.AdministrationContribution = bo.AdministrationContribution;
                entity.ShareOfRiCommissionFromCompulsoryCession = bo.ShareOfRiCommissionFromCompulsoryCession;
                entity.RecaptureFee = bo.RecaptureFee;
                entity.CreditCardCharges = bo.CreditCardCharges;
                entity.BrokerageFee = bo.BrokerageFee;

                entity.NBCession = bo.NBCession;
                entity.NBSumReins = bo.NBSumReins;
                entity.RNCession = bo.RNCession;
                entity.RNSumReins = bo.RNSumReins;
                entity.AltCession = bo.AltCession;
                entity.AltSumReins = bo.AltSumReins;

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

                entity.Frequency = bo.Frequency;
                entity.PreparedById = bo.PreparedById;

                entity.ValuationGross1st = bo.ValuationGross1st;
                entity.ValuationGrossRen = bo.ValuationGrossRen;
                entity.ValuationDiscount1st = bo.ValuationDiscount1st;
                entity.ValuationDiscountRen = bo.ValuationDiscountRen;
                entity.ValuationCom1st = bo.ValuationCom1st;
                entity.ValuationComRen = bo.ValuationComRen;
                entity.ValuationClaims = bo.ValuationClaims;
                entity.ValuationProfitCom = bo.ValuationProfitCom;
                entity.ValuationMode = bo.ValuationMode;
                entity.ValuationRiskPremium = bo.ValuationRiskPremium;

                entity.Remark = bo.Remark;
                entity.CurrencyCode = bo.CurrencyCode;
                entity.CurrencyRate = bo.CurrencyRate;
                entity.SoaQuarter = bo.SoaQuarter;

                entity.ReasonOfAdjustment1 = bo.ReasonOfAdjustment1;
                entity.ReasonOfAdjustment2 = bo.ReasonOfAdjustment2;
                entity.InvoiceNumber1 = bo.InvoiceNumber1;
                entity.InvoiceDate1 = bo.InvoiceDate1;
                entity.Amount1 = bo.Amount1;
                entity.InvoiceNumber2 = bo.InvoiceNumber2;
                entity.InvoiceDate2 = bo.InvoiceDate2;
                entity.Amount2 = bo.Amount2;               

                entity.SoaDataBatchId = bo.SoaDataBatchId;
                entity.ContractCode = bo.ContractCode;
                entity.AnnualCohort = bo.AnnualCohort;
                entity.ReportingType = bo.ReportingType;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref InvoiceRegisterBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(InvoiceRegisterBo bo)
        {
            InvoiceRegister.Delete(bo.Id);
        }

        public static Result Delete(InvoiceRegisterBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                DataTrail dataTrail = InvoiceRegister.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByInvoiceRegisterBatchId(int invoiceRegisterBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                List<DataTrail> trails = new List<DataTrail>();

                db.Database.ExecuteSqlCommand("DELETE FROM [InvoiceRegister] WHERE [InvoiceRegisterBatchId] = {0}", invoiceRegisterBatchId);
                db.SaveChanges();

                return trails;
            }
        }

        public static void DeleteAllByInvoiceRegisterBatchId(int invoiceRegisterBatchId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByInvoiceRegisterBatchId(invoiceRegisterBatchId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(InvoiceRegister)));
                }
            }
        }
    }
}
