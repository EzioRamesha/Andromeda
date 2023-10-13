using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class RetroStatementService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RetroStatement)),
                Controller = ModuleBo.ModuleController.RetroStatement.ToString()
            };
        }

        public static Expression<Func<RetroStatement, RetroStatementBo>> Expression()
        {
            return entity => new RetroStatementBo
            {
                Id = entity.Id,
                DirectRetroId = entity.DirectRetroId,
                RetroPartyId = entity.RetroPartyId,
                Status = entity.Status,
                MlreRef = entity.MlreRef,
                CedingCompany = entity.CedingCompany,
                TreatyCode = entity.TreatyCode,
                TreatyNo = entity.TreatyNo,
                Schedule = entity.Schedule,
                TreatyType = entity.TreatyType,
                FromMlreTo = entity.FromMlreTo,
                AccountsFor = entity.AccountsFor,
                DateReportCompleted = entity.DateReportCompleted,
                DateSendToRetro = entity.DateSendToRetro,

                // Data Set 1
                AccountingPeriod = entity.AccountingPeriod,
                ReserveCededBegin = entity.ReserveCededBegin,
                ReserveCededEnd = entity.ReserveCededEnd,
                RiskChargeCededBegin = entity.RiskChargeCededBegin,
                RiskChargeCededEnd = entity.RiskChargeCededEnd,
                AverageReserveCeded = entity.AverageReserveCeded,
                RiPremiumNB = entity.RiPremiumNB,
                RiPremiumRN = entity.RiPremiumRN,
                RiPremiumALT = entity.RiPremiumALT,
                QuarterlyRiskPremium = entity.QuarterlyRiskPremium,
                RetrocessionMarketingFee = entity.RetrocessionMarketingFee,
                RiDiscountNB = entity.RiDiscountNB,
                RiDiscountRN = entity.RiDiscountRN,
                RiDiscountALT = entity.RiDiscountALT,
                AgreedDatabaseComm = entity.AgreedDatabaseComm,
                GstPayable = entity.GstPayable,
                NoClaimBonus = entity.NoClaimBonus,
                Claims = entity.Claims,
                ProfitComm = entity.ProfitComm,
                SurrenderValue = entity.SurrenderValue,
                PaymentToTheReinsurer = entity.PaymentToTheReinsurer,
                TotalNoOfPolicyNB = entity.TotalNoOfPolicyNB,
                TotalNoOfPolicyRN = entity.TotalNoOfPolicyRN,
                TotalNoOfPolicyALT = entity.TotalNoOfPolicyALT,
                TotalSumReinsuredNB = entity.TotalSumReinsuredNB,
                TotalSumReinsuredRN = entity.TotalSumReinsuredRN,
                TotalSumReinsuredALT = entity.TotalSumReinsuredALT,

                // Data Set 2
                AccountingPeriod2 = entity.AccountingPeriod2,
                ReserveCededBegin2 = entity.ReserveCededBegin2,
                ReserveCededEnd2 = entity.ReserveCededEnd2,
                RiskChargeCededBegin2 = entity.RiskChargeCededBegin2,
                RiskChargeCededEnd2 = entity.RiskChargeCededEnd2,
                AverageReserveCeded2 = entity.AverageReserveCeded2,
                RiPremiumNB2 = entity.RiPremiumNB2,
                RiPremiumRN2 = entity.RiPremiumRN2,
                RiPremiumALT2 = entity.RiPremiumALT2,
                QuarterlyRiskPremium2 = entity.QuarterlyRiskPremium2,
                RetrocessionMarketingFee2 = entity.RetrocessionMarketingFee2,
                RiDiscountNB2 = entity.RiDiscountNB2,
                RiDiscountRN2 = entity.RiDiscountRN2,
                RiDiscountALT2 = entity.RiDiscountALT2,
                AgreedDatabaseComm2 = entity.AgreedDatabaseComm2,
                GstPayable2 = entity.GstPayable2,
                NoClaimBonus2 = entity.NoClaimBonus2,
                Claims2 = entity.Claims2,
                ProfitComm2 = entity.ProfitComm2,
                SurrenderValue2 = entity.SurrenderValue2,
                PaymentToTheReinsurer2 = entity.PaymentToTheReinsurer2,
                TotalNoOfPolicyNB2 = entity.TotalNoOfPolicyNB2,
                TotalNoOfPolicyRN2 = entity.TotalNoOfPolicyRN2,
                TotalNoOfPolicyALT2 = entity.TotalNoOfPolicyALT2,
                TotalSumReinsuredNB2 = entity.TotalSumReinsuredNB2,
                TotalSumReinsuredRN2 = entity.TotalSumReinsuredRN2,
                TotalSumReinsuredALT2 = entity.TotalSumReinsuredALT2,

                // Data Set 3
                AccountingPeriod3 = entity.AccountingPeriod3,
                ReserveCededBegin3 = entity.ReserveCededBegin3,
                ReserveCededEnd3 = entity.ReserveCededEnd3,
                RiskChargeCededBegin3 = entity.RiskChargeCededBegin3,
                RiskChargeCededEnd3 = entity.RiskChargeCededEnd3,
                AverageReserveCeded3 = entity.AverageReserveCeded3,
                RiPremiumNB3 = entity.RiPremiumNB3,
                RiPremiumRN3 = entity.RiPremiumRN3,
                RiPremiumALT3 = entity.RiPremiumALT3,
                QuarterlyRiskPremium3 = entity.QuarterlyRiskPremium3,
                RetrocessionMarketingFee3 = entity.RetrocessionMarketingFee3,
                RiDiscountNB3 = entity.RiDiscountNB3,
                RiDiscountRN3 = entity.RiDiscountRN3,
                RiDiscountALT3 = entity.RiDiscountALT3,
                AgreedDatabaseComm3 = entity.AgreedDatabaseComm3,
                GstPayable3 = entity.GstPayable3,
                NoClaimBonus3 = entity.NoClaimBonus3,
                Claims3 = entity.Claims3,
                ProfitComm3 = entity.ProfitComm3,
                SurrenderValue3 = entity.SurrenderValue3,
                PaymentToTheReinsurer3 = entity.PaymentToTheReinsurer3,
                TotalNoOfPolicyNB3 = entity.TotalNoOfPolicyNB3,
                TotalNoOfPolicyRN3 = entity.TotalNoOfPolicyRN3,
                TotalNoOfPolicyALT3 = entity.TotalNoOfPolicyALT3,
                TotalSumReinsuredNB3 = entity.TotalSumReinsuredNB3,
                TotalSumReinsuredRN3 = entity.TotalSumReinsuredRN3,
                TotalSumReinsuredALT3 = entity.TotalSumReinsuredALT3,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static RetroStatementBo FormBo(RetroStatement entity = null)
        {
            if (entity == null)
                return null;
            return new RetroStatementBo
            {
                Id = entity.Id,
                DirectRetroId = entity.DirectRetroId,
                DirectRetroBo = DirectRetroService.Find(entity.DirectRetroId),
                RetroPartyId = entity.RetroPartyId,
                RetroPartyBo = RetroPartyService.Find(entity.RetroPartyId),
                Status = entity.Status,
                StatusName = RetroStatementBo.GetStatusName(entity.Status),
                MlreRef = entity.MlreRef,
                CedingCompany = entity.CedingCompany,
                TreatyCode = entity.TreatyCode,
                TreatyNo = entity.TreatyNo,
                Schedule = entity.Schedule,
                TreatyType = entity.TreatyType,
                FromMlreTo = entity.FromMlreTo,
                AccountsFor = entity.AccountsFor,
                DateReportCompleted = entity.DateReportCompleted,
                DateSendToRetro = entity.DateSendToRetro,

                // Data Set 1
                AccountingPeriod = entity.AccountingPeriod,
                ReserveCededBegin = entity.ReserveCededBegin,
                ReserveCededEnd = entity.ReserveCededEnd,
                RiskChargeCededBegin = entity.RiskChargeCededBegin,
                RiskChargeCededEnd = entity.RiskChargeCededEnd,
                AverageReserveCeded = entity.AverageReserveCeded,
                RiPremiumNB = entity.RiPremiumNB,
                RiPremiumRN = entity.RiPremiumRN,
                RiPremiumALT = entity.RiPremiumALT,
                QuarterlyRiskPremium = entity.QuarterlyRiskPremium,
                RetrocessionMarketingFee = entity.RetrocessionMarketingFee,
                RiDiscountNB = entity.RiDiscountNB,
                RiDiscountRN = entity.RiDiscountRN,
                RiDiscountALT = entity.RiDiscountALT,
                AgreedDatabaseComm = entity.AgreedDatabaseComm,
                GstPayable = entity.GstPayable,
                NoClaimBonus = entity.NoClaimBonus,
                Claims = entity.Claims,
                ProfitComm = entity.ProfitComm,
                SurrenderValue = entity.SurrenderValue,
                PaymentToTheReinsurer = entity.PaymentToTheReinsurer,
                TotalNoOfPolicyNB = entity.TotalNoOfPolicyNB,
                TotalNoOfPolicyRN = entity.TotalNoOfPolicyRN,
                TotalNoOfPolicyALT = entity.TotalNoOfPolicyALT,
                TotalSumReinsuredNB = entity.TotalSumReinsuredNB,
                TotalSumReinsuredRN = entity.TotalSumReinsuredRN,
                TotalSumReinsuredALT = entity.TotalSumReinsuredALT,

                // Data Set 2
                AccountingPeriod2 = entity.AccountingPeriod2,
                ReserveCededBegin2 = entity.ReserveCededBegin2,
                ReserveCededEnd2 = entity.ReserveCededEnd2,
                RiskChargeCededBegin2 = entity.RiskChargeCededBegin2,
                RiskChargeCededEnd2 = entity.RiskChargeCededEnd2,
                AverageReserveCeded2 = entity.AverageReserveCeded2,
                RiPremiumNB2 = entity.RiPremiumNB2,
                RiPremiumRN2 = entity.RiPremiumRN2,
                RiPremiumALT2 = entity.RiPremiumALT2,
                QuarterlyRiskPremium2 = entity.QuarterlyRiskPremium2,
                RetrocessionMarketingFee2 = entity.RetrocessionMarketingFee2,
                RiDiscountNB2 = entity.RiDiscountNB2,
                RiDiscountRN2 = entity.RiDiscountRN2,
                RiDiscountALT2 = entity.RiDiscountALT2,
                AgreedDatabaseComm2 = entity.AgreedDatabaseComm2,
                GstPayable2 = entity.GstPayable2,
                NoClaimBonus2 = entity.NoClaimBonus2,
                Claims2 = entity.Claims2,
                ProfitComm2 = entity.ProfitComm2,
                SurrenderValue2 = entity.SurrenderValue2,
                PaymentToTheReinsurer2 = entity.PaymentToTheReinsurer2,
                TotalNoOfPolicyNB2 = entity.TotalNoOfPolicyNB2,
                TotalNoOfPolicyRN2 = entity.TotalNoOfPolicyRN2,
                TotalNoOfPolicyALT2 = entity.TotalNoOfPolicyALT2,
                TotalSumReinsuredNB2 = entity.TotalSumReinsuredNB2,
                TotalSumReinsuredRN2 = entity.TotalSumReinsuredRN2,
                TotalSumReinsuredALT2 = entity.TotalSumReinsuredALT2,

                // Data Set 3
                AccountingPeriod3 = entity.AccountingPeriod3,
                ReserveCededBegin3 = entity.ReserveCededBegin3,
                ReserveCededEnd3 = entity.ReserveCededEnd3,
                RiskChargeCededBegin3 = entity.RiskChargeCededBegin3,
                RiskChargeCededEnd3 = entity.RiskChargeCededEnd3,
                AverageReserveCeded3 = entity.AverageReserveCeded3,
                RiPremiumNB3 = entity.RiPremiumNB3,
                RiPremiumRN3 = entity.RiPremiumRN3,
                RiPremiumALT3 = entity.RiPremiumALT3,
                QuarterlyRiskPremium3 = entity.QuarterlyRiskPremium3,
                RetrocessionMarketingFee3 = entity.RetrocessionMarketingFee3,
                RiDiscountNB3 = entity.RiDiscountNB3,
                RiDiscountRN3 = entity.RiDiscountRN3,
                RiDiscountALT3 = entity.RiDiscountALT3,
                AgreedDatabaseComm3 = entity.AgreedDatabaseComm3,
                GstPayable3 = entity.GstPayable3,
                NoClaimBonus3 = entity.NoClaimBonus3,
                Claims3 = entity.Claims3,
                ProfitComm3 = entity.ProfitComm3,
                SurrenderValue3 = entity.SurrenderValue3,
                PaymentToTheReinsurer3 = entity.PaymentToTheReinsurer3,
                TotalNoOfPolicyNB3 = entity.TotalNoOfPolicyNB3,
                TotalNoOfPolicyRN3 = entity.TotalNoOfPolicyRN3,
                TotalNoOfPolicyALT3 = entity.TotalNoOfPolicyALT3,
                TotalSumReinsuredNB3 = entity.TotalSumReinsuredNB3,
                TotalSumReinsuredRN3 = entity.TotalSumReinsuredRN3,
                TotalSumReinsuredALT3 = entity.TotalSumReinsuredALT3,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<RetroStatementBo> FormBos(IList<RetroStatement> entities = null)
        {
            if (entities == null)
                return null;
            IList<RetroStatementBo> bos = new List<RetroStatementBo>() { };
            foreach (RetroStatement entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RetroStatement FormEntity(RetroStatementBo bo = null)
        {
            if (bo == null)
                return null;
            return new RetroStatement
            {
                Id = bo.Id,
                DirectRetroId = bo.DirectRetroId,
                RetroPartyId = bo.RetroPartyId,
                Status = bo.Status,
                MlreRef = bo.MlreRef,
                CedingCompany = bo.CedingCompany,
                TreatyCode = bo.TreatyCode,
                TreatyNo = bo.TreatyNo,
                Schedule = bo.Schedule,
                TreatyType = bo.TreatyType,
                FromMlreTo = bo.FromMlreTo,
                AccountsFor = bo.AccountsFor,
                DateReportCompleted = bo.DateReportCompleted,
                DateSendToRetro = bo.DateSendToRetro,

                // Data Set 1
                AccountingPeriod = bo.AccountingPeriod,
                ReserveCededBegin = bo.ReserveCededBegin,
                ReserveCededEnd = bo.ReserveCededEnd,
                RiskChargeCededBegin = bo.RiskChargeCededBegin,
                RiskChargeCededEnd = bo.RiskChargeCededEnd,
                AverageReserveCeded = bo.AverageReserveCeded,
                RiPremiumNB = bo.RiPremiumNB,
                RiPremiumRN = bo.RiPremiumRN,
                RiPremiumALT = bo.RiPremiumALT,
                QuarterlyRiskPremium = bo.QuarterlyRiskPremium,
                RetrocessionMarketingFee = bo.RetrocessionMarketingFee,
                RiDiscountNB = bo.RiDiscountNB,
                RiDiscountRN = bo.RiDiscountRN,
                RiDiscountALT = bo.RiDiscountALT,
                AgreedDatabaseComm = bo.AgreedDatabaseComm,
                GstPayable = bo.GstPayable,
                NoClaimBonus = bo.NoClaimBonus,
                Claims = bo.Claims,
                ProfitComm = bo.ProfitComm,
                SurrenderValue = bo.SurrenderValue,
                PaymentToTheReinsurer = bo.PaymentToTheReinsurer,
                TotalNoOfPolicyNB = bo.TotalNoOfPolicyNB,
                TotalNoOfPolicyRN = bo.TotalNoOfPolicyRN,
                TotalNoOfPolicyALT = bo.TotalNoOfPolicyALT,
                TotalSumReinsuredNB = bo.TotalSumReinsuredNB,
                TotalSumReinsuredRN = bo.TotalSumReinsuredRN,
                TotalSumReinsuredALT = bo.TotalSumReinsuredALT,

                // Data Set 2
                AccountingPeriod2 = bo.AccountingPeriod2,
                ReserveCededBegin2 = bo.ReserveCededBegin2,
                ReserveCededEnd2 = bo.ReserveCededEnd2,
                RiskChargeCededBegin2 = bo.RiskChargeCededBegin2,
                RiskChargeCededEnd2 = bo.RiskChargeCededEnd2,
                AverageReserveCeded2 = bo.AverageReserveCeded2,
                RiPremiumNB2 = bo.RiPremiumNB2,
                RiPremiumRN2 = bo.RiPremiumRN2,
                RiPremiumALT2 = bo.RiPremiumALT2,
                QuarterlyRiskPremium2 = bo.QuarterlyRiskPremium2,
                RetrocessionMarketingFee2 = bo.RetrocessionMarketingFee2,
                RiDiscountNB2 = bo.RiDiscountNB2,
                RiDiscountRN2 = bo.RiDiscountRN2,
                RiDiscountALT2 = bo.RiDiscountALT2,
                AgreedDatabaseComm2 = bo.AgreedDatabaseComm2,
                GstPayable2 = bo.GstPayable2,
                NoClaimBonus2 = bo.NoClaimBonus2,
                Claims2 = bo.Claims2,
                ProfitComm2 = bo.ProfitComm2,
                SurrenderValue2 = bo.SurrenderValue2,
                PaymentToTheReinsurer2 = bo.PaymentToTheReinsurer2,
                TotalNoOfPolicyNB2 = bo.TotalNoOfPolicyNB2,
                TotalNoOfPolicyRN2 = bo.TotalNoOfPolicyRN2,
                TotalNoOfPolicyALT2 = bo.TotalNoOfPolicyALT2,
                TotalSumReinsuredNB2 = bo.TotalSumReinsuredNB2,
                TotalSumReinsuredRN2 = bo.TotalSumReinsuredRN2,
                TotalSumReinsuredALT2 = bo.TotalSumReinsuredALT2,

                // Data Set 3
                AccountingPeriod3 = bo.AccountingPeriod3,
                ReserveCededBegin3 = bo.ReserveCededBegin3,
                ReserveCededEnd3 = bo.ReserveCededEnd3,
                RiskChargeCededBegin3 = bo.RiskChargeCededBegin3,
                RiskChargeCededEnd3 = bo.RiskChargeCededEnd3,
                AverageReserveCeded3 = bo.AverageReserveCeded3,
                RiPremiumNB3 = bo.RiPremiumNB3,
                RiPremiumRN3 = bo.RiPremiumRN3,
                RiPremiumALT3 = bo.RiPremiumALT3,
                QuarterlyRiskPremium3 = bo.QuarterlyRiskPremium3,
                RetrocessionMarketingFee3 = bo.RetrocessionMarketingFee3,
                RiDiscountNB3 = bo.RiDiscountNB3,
                RiDiscountRN3 = bo.RiDiscountRN3,
                RiDiscountALT3 = bo.RiDiscountALT3,
                AgreedDatabaseComm3 = bo.AgreedDatabaseComm3,
                GstPayable3 = bo.GstPayable3,
                NoClaimBonus3 = bo.NoClaimBonus3,
                Claims3 = bo.Claims3,
                ProfitComm3 = bo.ProfitComm3,
                SurrenderValue3 = bo.SurrenderValue3,
                PaymentToTheReinsurer3 = bo.PaymentToTheReinsurer3,
                TotalNoOfPolicyNB3 = bo.TotalNoOfPolicyNB3,
                TotalNoOfPolicyRN3 = bo.TotalNoOfPolicyRN3,
                TotalNoOfPolicyALT3 = bo.TotalNoOfPolicyALT3,
                TotalSumReinsuredNB3 = bo.TotalSumReinsuredNB3,
                TotalSumReinsuredRN3 = bo.TotalSumReinsuredRN3,
                TotalSumReinsuredALT3 = bo.TotalSumReinsuredALT3,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return RetroStatement.IsExists(id);
        }

        public static RetroStatementBo Find(int id)
        {
            return FormBo(RetroStatement.Find(id));
        }

        public static RetroStatementBo FindByDirectRetroId(int directRetroId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.RetroStatements.Where(q => q.DirectRetroId == directRetroId).FirstOrDefault());
            }
        }

        public static int CountByDirectRetroId(int directRetroId)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroStatements.Where(q => q.DirectRetroId == directRetroId).Count();
            }
        }

        public static int CountByDirectRetroIdByStatus(int directRetroId, int status)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroStatements
                    .Where(q => q.DirectRetroId == directRetroId)
                    .Where(q => q.Status == status)
                    .Count();
            }
        }

        public static int CountByRetroPartyId(int retroPartyId)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroStatements.Where(q => q.RetroPartyId == retroPartyId).Count();
            }
        }

        public static IList<RetroStatementBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.RetroStatements.ToList());
            }
        }

        public static IList<RetroStatementBo> GetByDirectRetroId(int directRetroId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.RetroStatements.Where(q => q.DirectRetroId == directRetroId).ToList());
            }
        }

        public static Result ValidateDuplicate(RetroStatementBo bo)
        {
            Result result = new Result();
            int count = 0;

            using (var db = new AppDbContext())
            {
                var query = db.RetroStatements
                    .Where(q => q.DirectRetroId == bo.DirectRetroId)
                    .Where(q => q.RetroPartyId == bo.RetroPartyId);

                if (bo.Id != 0)
                {
                    query = query.Where(q => q.Id != bo.Id);
                }

                count = query.Count();
            }

            if (count > 0)
                result.AddError("Existing Retro Statement Found");

            return result;
        }

        public static Result Save(ref RetroStatementBo bo)
        {
            if (!RetroStatement.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref RetroStatementBo bo, ref TrailObject trail)
        {
            if (!RetroStatement.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RetroStatementBo bo)
        {
            RetroStatement entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RetroStatementBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RetroStatementBo bo)
        {
            Result result = Result();

            RetroStatement entity = RetroStatement.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.DirectRetroId = bo.DirectRetroId;
                entity.RetroPartyId = bo.RetroPartyId;
                entity.Status = bo.Status;
                entity.MlreRef = bo.MlreRef;
                entity.CedingCompany = bo.CedingCompany;
                entity.TreatyCode = bo.TreatyCode;
                entity.TreatyNo = bo.TreatyNo;
                entity.Schedule = bo.Schedule;
                entity.TreatyType = bo.TreatyType;
                entity.FromMlreTo = bo.FromMlreTo;
                entity.AccountsFor = bo.AccountsFor;
                entity.DateReportCompleted = bo.DateReportCompleted;
                entity.DateSendToRetro = bo.DateSendToRetro;

                // Data Set 1
                entity.AccountingPeriod = bo.AccountingPeriod;
                entity.ReserveCededBegin = bo.ReserveCededBegin;
                entity.ReserveCededEnd = bo.ReserveCededEnd;
                entity.RiskChargeCededBegin = bo.RiskChargeCededBegin;
                entity.RiskChargeCededEnd = bo.RiskChargeCededEnd;
                entity.AverageReserveCeded = bo.AverageReserveCeded;
                entity.RiPremiumNB = bo.RiPremiumNB;
                entity.RiPremiumRN = bo.RiPremiumRN;
                entity.RiPremiumALT = bo.RiPremiumALT;
                entity.QuarterlyRiskPremium = bo.QuarterlyRiskPremium;
                entity.RetrocessionMarketingFee = bo.RetrocessionMarketingFee;
                entity.RiDiscountNB = bo.RiDiscountNB;
                entity.RiDiscountRN = bo.RiDiscountRN;
                entity.RiDiscountALT = bo.RiDiscountALT;
                entity.AgreedDatabaseComm = bo.AgreedDatabaseComm;
                entity.GstPayable = bo.GstPayable;
                entity.NoClaimBonus = bo.NoClaimBonus;
                entity.Claims = bo.Claims;
                entity.ProfitComm = bo.ProfitComm;
                entity.SurrenderValue = bo.SurrenderValue;
                entity.PaymentToTheReinsurer = bo.PaymentToTheReinsurer;
                entity.TotalNoOfPolicyNB = bo.TotalNoOfPolicyNB;
                entity.TotalNoOfPolicyRN = bo.TotalNoOfPolicyRN;
                entity.TotalNoOfPolicyALT = bo.TotalNoOfPolicyALT;
                entity.TotalSumReinsuredNB = bo.TotalSumReinsuredNB;
                entity.TotalSumReinsuredRN = bo.TotalSumReinsuredRN;
                entity.TotalSumReinsuredALT = bo.TotalSumReinsuredALT;

                // Data Set 2
                entity.AccountingPeriod2 = bo.AccountingPeriod2;
                entity.ReserveCededBegin2 = bo.ReserveCededBegin2;
                entity.ReserveCededEnd2 = bo.ReserveCededEnd2;
                entity.RiskChargeCededBegin2 = bo.RiskChargeCededBegin2;
                entity.RiskChargeCededEnd2 = bo.RiskChargeCededEnd2;
                entity.AverageReserveCeded2 = bo.AverageReserveCeded2;
                entity.RiPremiumNB2 = bo.RiPremiumNB2;
                entity.RiPremiumRN2 = bo.RiPremiumRN2;
                entity.RiPremiumALT2 = bo.RiPremiumALT2;
                entity.QuarterlyRiskPremium2 = bo.QuarterlyRiskPremium2;
                entity.RetrocessionMarketingFee2 = bo.RetrocessionMarketingFee2;
                entity.RiDiscountNB2 = bo.RiDiscountNB2;
                entity.RiDiscountRN2 = bo.RiDiscountRN2;
                entity.RiDiscountALT2 = bo.RiDiscountALT2;
                entity.AgreedDatabaseComm2 = bo.AgreedDatabaseComm2;
                entity.GstPayable2 = bo.GstPayable2;
                entity.NoClaimBonus2 = bo.NoClaimBonus2;
                entity.Claims2 = bo.Claims2;
                entity.ProfitComm2 = bo.ProfitComm2;
                entity.SurrenderValue2 = bo.SurrenderValue2;
                entity.PaymentToTheReinsurer2 = bo.PaymentToTheReinsurer2;
                entity.TotalNoOfPolicyNB2 = bo.TotalNoOfPolicyNB2;
                entity.TotalNoOfPolicyRN2 = bo.TotalNoOfPolicyRN2;
                entity.TotalNoOfPolicyALT2 = bo.TotalNoOfPolicyALT2;
                entity.TotalSumReinsuredNB2 = bo.TotalSumReinsuredNB2;
                entity.TotalSumReinsuredRN2 = bo.TotalSumReinsuredRN2;
                entity.TotalSumReinsuredALT2 = bo.TotalSumReinsuredALT2;

                // Data Set 3
                entity.AccountingPeriod3 = bo.AccountingPeriod3;
                entity.ReserveCededBegin3 = bo.ReserveCededBegin3;
                entity.ReserveCededEnd3 = bo.ReserveCededEnd3;
                entity.RiskChargeCededBegin3 = bo.RiskChargeCededBegin3;
                entity.RiskChargeCededEnd3 = bo.RiskChargeCededEnd3;
                entity.AverageReserveCeded3 = bo.AverageReserveCeded3;
                entity.RiPremiumNB3 = bo.RiPremiumNB3;
                entity.RiPremiumRN3 = bo.RiPremiumRN3;
                entity.RiPremiumALT3 = bo.RiPremiumALT3;
                entity.QuarterlyRiskPremium3 = bo.QuarterlyRiskPremium3;
                entity.RetrocessionMarketingFee3 = bo.RetrocessionMarketingFee3;
                entity.RiDiscountNB3 = bo.RiDiscountNB3;
                entity.RiDiscountRN3 = bo.RiDiscountRN3;
                entity.RiDiscountALT3 = bo.RiDiscountALT3;
                entity.AgreedDatabaseComm3 = bo.AgreedDatabaseComm3;
                entity.GstPayable3 = bo.GstPayable3;
                entity.NoClaimBonus3 = bo.NoClaimBonus3;
                entity.Claims3 = bo.Claims3;
                entity.ProfitComm3 = bo.ProfitComm3;
                entity.SurrenderValue3 = bo.SurrenderValue3;
                entity.PaymentToTheReinsurer3 = bo.PaymentToTheReinsurer3;
                entity.TotalNoOfPolicyNB3 = bo.TotalNoOfPolicyNB3;
                entity.TotalNoOfPolicyRN3 = bo.TotalNoOfPolicyRN3;
                entity.TotalNoOfPolicyALT3 = bo.TotalNoOfPolicyALT3;
                entity.TotalSumReinsuredNB3 = bo.TotalSumReinsuredNB3;
                entity.TotalSumReinsuredRN3 = bo.TotalSumReinsuredRN3;
                entity.TotalSumReinsuredALT3 = bo.TotalSumReinsuredALT3;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RetroStatementBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RetroStatementBo bo)
        {
            RetroStatement.Delete(bo.Id);
        }

        public static Result Delete(RetroStatementBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (bo.Status == RetroStatementBo.StatusFinalised)
            {
                result.AddError("Finalised Retro Statement can not be deleted");
            }

            if (result.Valid)
            {
                DataTrail dataTrail = RetroStatement.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteByDirectRetroId(int directRetroId)
        {
            return RetroStatement.DeleteByDirectRetroId(directRetroId);
        }

        public static void DeleteByDirectRetroId(int directRetroId, ref TrailObject trail)
        {
            Result result = Result();
            IList<DataTrail> dataTrails = DeleteByDirectRetroId(directRetroId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, result.Table);
                }
            }
        }
    }
}
