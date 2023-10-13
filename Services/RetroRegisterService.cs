using BusinessObject;
using DataAccess.Entities;
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

namespace Services
{
    public class RetroRegisterService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RetroRegister)),
                Controller = ModuleBo.ModuleController.RetroRegister.ToString()
            };
        }

        public static Expression<Func<RetroRegister, RetroRegisterBo>> Expression()
        {
            return entity => new RetroRegisterBo
            {
                Id = entity.Id,
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

                RetroName = entity.RetroParty.Name,
                PartyCode = entity.RetroParty.Party,
                RetroStatus = entity.DirectRetro.RetroStatus,
            };
        }

        public static RetroRegisterBo FormBo(RetroRegister entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            RetroRegisterBo retroRegisterBo = new RetroRegisterBo()
            {
                Id = entity.Id,
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
                retroRegisterBo.RetroRegisterBatchBo = RetroRegisterBatchService.Find(entity.RetroRegisterBatchId);
                retroRegisterBo.RetroPartyBo = RetroPartyService.Find(entity.RetroPartyId);
                retroRegisterBo.CedantBo = CedantService.Find(entity.CedantId);
                retroRegisterBo.TreatyCodeBo = TreatyCodeService.Find(entity.TreatyCodeId);
                retroRegisterBo.PreparedByBo = UserService.Find(entity.PreparedById);
                if (entity.DirectRetroId.HasValue)
                    retroRegisterBo.DirectRetroBo = DirectRetroService.Find(entity.DirectRetroId.Value);
            }
            return retroRegisterBo;
        }

        public static IList<RetroRegisterBo> FormBos(IList<RetroRegister> entities = null)
        {
            if (entities == null)
                return null;
            IList<RetroRegisterBo> bos = new List<RetroRegisterBo>() { };
            foreach (RetroRegister entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RetroRegister FormEntity(RetroRegisterBo bo = null)
        {
            if (bo == null)
                return null;
            return new RetroRegister
            {
                Id = bo.Id,
                Type = bo.Type,
                RetroRegisterBatchId = bo.RetroRegisterBatchId,
                RetroStatementType = bo.RetroStatementType,
                RetroStatementNo = bo.RetroStatementNo,
                RetroStatementDate = bo.RetroStatementDate,
                ReportCompletedDate = bo.ReportCompletedDate,
                SendToRetroDate = bo.SendToRetroDate,
                RetroPartyId = bo.RetroPartyId,
                RiskQuarter = bo.RiskQuarter,
                CedantId = bo.CedantId,
                TreatyCodeId = bo.TreatyCodeId,
                TreatyNumber = bo.TreatyNumber,
                Schedule = bo.Schedule,
                TreatyType = bo.TreatyType,
                LOB = bo.LOB,
                AccountFor = bo.AccountFor,
                Year1st = bo.Year1st,
                Renewal = bo.Renewal,
                ReserveCededBegin = bo.ReserveCededBegin,
                ReserveCededEnd = bo.ReserveCededEnd,
                RiskChargeCededBegin = bo.RiskChargeCededBegin,
                RiskChargeCededEnd = bo.RiskChargeCededEnd,
                AverageReserveCeded = bo.AverageReserveCeded,
                Gross1st = bo.Gross1st,
                GrossRen = bo.GrossRen,
                AltPremium = bo.AltPremium,
                Discount1st = bo.Discount1st,
                DiscountRen = bo.DiscountRen,
                DiscountAlt = bo.DiscountAlt,
                RiskPremium = bo.RiskPremium,
                Claims = bo.Claims,
                ProfitCommission = bo.ProfitCommission,
                SurrenderVal = bo.SurrenderVal,
                NoClaimBonus = bo.NoClaimBonus,
                RetrocessionMarketingFee = bo.RetrocessionMarketingFee,
                AgreedDBCommission = bo.AgreedDBCommission,
                GstPayable = bo.GstPayable,
                NetTotalAmount = bo.NetTotalAmount,
                NbCession = bo.NbCession,
                NbSumReins = bo.NbSumReins,
                RnCession = bo.RnCession,
                RnSumReins = bo.RnSumReins,
                AltCession = bo.AltCession,
                AltSumReins = bo.AltSumReins,
                Frequency = bo.Frequency,
                PreparedById = bo.PreparedById,
                OriginalSoaQuarter = bo.OriginalSoaQuarter,
                RetroConfirmationDate = bo.RetroConfirmationDate,
                ValuationGross1st = bo.ValuationGross1st,
                ValuationGrossRen = bo.ValuationGrossRen,
                ValuationDiscount1st = bo.ValuationDiscount1st,
                ValuationDiscountRen = bo.ValuationDiscountRen,
                ValuationCom1st = bo.ValuationCom1st,
                ValuationComRen = bo.ValuationComRen,
                Remark = bo.Remark,
                Status = bo.Status,
                AnnualCohort = bo.AnnualCohort,
                ContractCode = bo.ContractCode,
                ReportingType = bo.ReportingType,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
                DirectRetroId = bo.DirectRetroId,
            };
        }

        public static bool IsExists(int id)
        {
            return RetroRegister.IsExists(id);
        }

        public static RetroRegisterBo Find(int id)
        {
            return FormBo(RetroRegister.Find(id));
        }

        public static RetroRegisterBo FindByDirectRetroIdRetroPartyIdRiskQuarter(int directRetroId, int retroPartyId, string riskQuarter)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.RetroRegisters
                    .Where(q => q.DirectRetroId.HasValue && q.DirectRetroId == directRetroId)
                    .Where(q => q.RetroPartyId.HasValue && q.RetroPartyId == retroPartyId)
                    .Where(q => q.RiskQuarter == riskQuarter)
                    .FirstOrDefault());
            }
        }

        public static RetroRegisterBo FindByInvoiceReferenceIfrs4(string statementNo)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroRegisters.Where(q => q.RetroStatementNo == statementNo)
                    .Where(q => q.ReportingType != RetroRegisterBo.ReportingTypeIFRS17);

                return FormBo(query.FirstOrDefault());
            }
        }
        public static IList<RetroRegisterBo> FindByInvoiceReferenceIfrs17(string statementNo)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroRegisters.Where(q => q.RetroStatementNo == statementNo)
                    .Where(q => q.ReportingType == RetroRegisterBo.ReportingTypeIFRS17);

                return FormBos(query.ToList());
            }
        }

        public static IList<RetroRegisterBo> GetByRetroRegisterBatchId(int retroRegisterBatchId, int skip = 0, int take = 50)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.RetroRegisters.Where(q => q.RetroRegisterBatchId == retroRegisterBatchId).OrderBy(q => q.Id).Skip(skip).Take(take).ToList());
            }
        }

        public static int CountByRetroRegisterBatchId(int retroRegisterBatchId, int reportingType)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroRegisters.Where(q => q.RetroRegisterBatchId == retroRegisterBatchId)
                    .Where(q => q.ReportingType == reportingType)
                    .Count();
            }
        }

        public static RetroRegisterBo GetLastRetroRegisterCreated(int year)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.RetroRegisters.Where(q => q.CreatedAt.Year == year).OrderByDescending(q => q.CreatedAt).FirstOrDefault());
            }
        }

        public static string GetNextStatementNo(int year, string businessOriginCode)
        {
            using (var db = new AppDbContext())
            {
                string prefix = string.Format("R{0}/{1}/", businessOriginCode, year);

                int type = RetroRegisterBo.GetTypeByCode(businessOriginCode);

                var retroRegister = db.RetroRegisters.Where(q => q.CreatedAt.Year == year)
                     .Where(q => q.RetroStatementType == type)
                    .Where(q => !string.IsNullOrEmpty(q.RetroStatementNo) && q.RetroStatementNo.StartsWith(prefix))
                    .OrderByDescending(a => a.RetroStatementNo.Length)
                    .ThenByDescending(a => a.RetroStatementNo)
                    .FirstOrDefault();

                int count = 0;
                if (retroRegister != null)
                {
                    string referenceNo = retroRegister.RetroStatementNo;
                    string[] referenceNoInfo = referenceNo.Split('/');

                    if (referenceNoInfo.Length == 3)
                    {
                        string countStr = referenceNoInfo[2];
                        int.TryParse(countStr, out count);
                    }
                }
                count++;
                string nextCountStr = count.ToString();

                return prefix + nextCountStr;
            }
        }

        public static int CountByRetroPartyId(int retroPartyId)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroRegisters.Where(q => q.RetroPartyId == retroPartyId).Count();
            }
        }

        public static IList<RetroRegisterBo> RetroRegisterReportParams()
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroRegisters
                   .Where(q => q.ReportingType != RetroRegisterBo.ReportingTypeIFRS17);

                return FormBos(query.ToList());
            }
        }

        public static Result Save(ref RetroRegisterBo bo)
        {
            if (!RetroRegister.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref RetroRegisterBo bo, ref TrailObject trail)
        {
            if (!RetroRegister.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RetroRegisterBo bo)
        {
            RetroRegister entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RetroRegisterBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RetroRegisterBo bo)
        {
            Result result = Result();

            RetroRegister entity = RetroRegister.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Type = bo.Type;
                entity.RetroRegisterBatchId = bo.RetroRegisterBatchId;
                entity.RetroStatementType = bo.RetroStatementType;
                entity.RetroStatementNo = bo.RetroStatementNo;
                entity.RetroStatementDate = bo.RetroStatementDate;
                entity.ReportCompletedDate = bo.ReportCompletedDate;
                entity.SendToRetroDate = bo.SendToRetroDate;
                entity.RetroPartyId = bo.RetroPartyId;
                entity.RiskQuarter = bo.RiskQuarter;
                entity.CedantId = bo.CedantId;
                entity.TreatyCodeId = bo.TreatyCodeId;
                entity.TreatyNumber = bo.TreatyNumber;
                entity.Schedule = bo.Schedule;
                entity.TreatyType = bo.TreatyType;
                entity.LOB = bo.LOB;
                entity.AccountFor = bo.AccountFor;
                entity.Year1st = bo.Year1st;
                entity.Renewal = bo.Renewal;
                entity.ReserveCededBegin = bo.ReserveCededBegin;
                entity.ReserveCededEnd = bo.ReserveCededEnd;
                entity.RiskChargeCededBegin = bo.RiskChargeCededBegin;
                entity.RiskChargeCededEnd = bo.RiskChargeCededEnd;
                entity.AverageReserveCeded = bo.AverageReserveCeded;
                entity.Gross1st = bo.Gross1st;
                entity.GrossRen = bo.GrossRen;
                entity.AltPremium = bo.AltPremium;
                entity.Discount1st = bo.Discount1st;
                entity.DiscountRen = bo.DiscountRen;
                entity.DiscountAlt = bo.DiscountAlt;
                entity.RiskPremium = bo.RiskPremium;
                entity.Claims = bo.Claims;
                entity.ProfitCommission = bo.ProfitCommission;
                entity.SurrenderVal = bo.SurrenderVal;
                entity.NoClaimBonus = bo.NoClaimBonus;
                entity.RetrocessionMarketingFee = bo.RetrocessionMarketingFee;
                entity.AgreedDBCommission = bo.AgreedDBCommission;
                entity.GstPayable = bo.GstPayable;
                entity.NetTotalAmount = bo.NetTotalAmount;
                entity.NbCession = bo.NbCession;
                entity.NbSumReins = bo.NbSumReins;
                entity.RnCession = bo.RnCession;
                entity.RnSumReins = bo.RnSumReins;
                entity.AltCession = bo.AltCession;
                entity.AltSumReins = bo.AltSumReins;
                entity.Frequency = bo.Frequency;
                entity.PreparedById = bo.PreparedById;
                entity.OriginalSoaQuarter = bo.OriginalSoaQuarter;
                entity.RetroConfirmationDate = bo.RetroConfirmationDate;
                entity.ValuationGross1st = bo.ValuationGross1st;
                entity.ValuationGrossRen = bo.ValuationGrossRen;
                entity.ValuationDiscount1st = bo.ValuationDiscount1st;
                entity.ValuationDiscountRen = bo.ValuationDiscountRen;
                entity.ValuationCom1st = bo.ValuationCom1st;
                entity.ValuationComRen = bo.ValuationComRen;
                entity.Remark = bo.Remark;
                entity.Status = bo.Status;
                entity.AnnualCohort = bo.AnnualCohort;
                entity.ContractCode = bo.ContractCode;
                entity.ReportingType = bo.ReportingType;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                entity.DirectRetroId = bo.DirectRetroId;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RetroRegisterBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RetroRegisterBo bo)
        {
            RetroRegister.Delete(bo.Id);
        }

        public static Result Delete(RetroRegisterBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                DataTrail dataTrail = RetroRegister.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByRetroRegisterBatchId(int retroRegisterBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                List<DataTrail> trails = new List<DataTrail>();

                db.Database.ExecuteSqlCommand("DELETE FROM [RetroRegister] WHERE [RetroRegisterBatchId] = {0}", retroRegisterBatchId);
                db.SaveChanges();

                return trails;
            }
        }

        public static void DeleteAllByRetroRegisterBatchId(int retroRegisterBatchId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByRetroRegisterBatchId(retroRegisterBatchId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(RetroRegister)));
                }
            }
        }
    }
}
