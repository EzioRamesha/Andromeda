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

namespace Services
{
    public class RetroSummaryService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RetroSummary)),
                Controller = ModuleBo.ModuleController.RetroSummary.ToString()
            };
        }

        public static Expression<Func<RetroSummary, RetroSummaryBo>> Expression()
        {
            return entity => new RetroSummaryBo
            {
                Id = entity.Id,
                DirectRetroId = entity.DirectRetroId,
                RiskQuarter = entity.RiskQuarter,
                Month = entity.Month,
                Year = entity.Year,
                Type = entity.Type,
                NoOfPolicy = entity.NoOfPolicy,
                TotalSar = entity.TotalSar,
                TotalRiPremium = entity.TotalRiPremium,
                TotalDiscount = entity.TotalDiscount,
                NoOfClaims = entity.NoOfClaims,
                TotalClaims = entity.TotalClaims,
                RetroParty1 = entity.RetroParty1,
                RetroParty2 = entity.RetroParty2,
                RetroParty3 = entity.RetroParty3,
                RetroShare1 = entity.RetroShare1,
                RetroShare2 = entity.RetroShare2,
                RetroShare3 = entity.RetroShare3,
                RetroRiPremium1 = entity.RetroRiPremium1,
                RetroRiPremium2 = entity.RetroRiPremium2,
                RetroRiPremium3 = entity.RetroRiPremium3,
                RetroDiscount1 = entity.RetroDiscount1,
                RetroDiscount2 = entity.RetroDiscount2,
                RetroDiscount3 = entity.RetroDiscount3,
                RetroClaims1 = entity.RetroClaims1,
                RetroClaims2 = entity.RetroClaims2,
                RetroClaims3 = entity.RetroClaims3,
                TreatyCode = entity.TreatyCode,
                RetroPremiumSpread1 = entity.RetroPremiumSpread1,
                RetroPremiumSpread2 = entity.RetroPremiumSpread2,
                RetroPremiumSpread3 = entity.RetroPremiumSpread3,
                TotalDirectRetroAar = entity.TotalDirectRetroAar,
                ReportingType = entity.ReportingType,
                Mfrs17AnnualCohort = entity.Mfrs17AnnualCohort,
                Mfrs17ContractCode = entity.Mfrs17ContractCode,
                CreatedById = entity.CreatedById,
            };
        }

        public static RetroSummaryBo FormBo(RetroSummary entity = null, bool foreign = true, bool formatOutput = false)
        {
            if (entity == null)
                return null;
            RetroSummaryBo retroSummaryBo = new RetroSummaryBo
            {
                Id = entity.Id,
                DirectRetroId = entity.DirectRetroId,
                RiskQuarter = entity.RiskQuarter,
                Month = entity.Month,
                Year = entity.Year,
                Type = entity.Type,
                NoOfPolicy = entity.NoOfPolicy,
                TotalSar = entity.TotalSar,
                TotalRiPremium = entity.TotalRiPremium,
                TotalDiscount = entity.TotalDiscount,
                NoOfClaims = entity.NoOfClaims,
                TotalClaims = entity.TotalClaims,
                RetroParty1 = entity.RetroParty1,
                RetroParty2 = entity.RetroParty2,
                RetroParty3 = entity.RetroParty3,
                RetroShare1 = entity.RetroShare1,
                RetroShare2 = entity.RetroShare2,
                RetroShare3 = entity.RetroShare3,
                RetroRiPremium1 = entity.RetroRiPremium1,
                RetroRiPremium2 = entity.RetroRiPremium2,
                RetroRiPremium3 = entity.RetroRiPremium3,
                RetroDiscount1 = entity.RetroDiscount1,
                RetroDiscount2 = entity.RetroDiscount2,
                RetroDiscount3 = entity.RetroDiscount3,
                RetroClaims1 = entity.RetroClaims1,
                RetroClaims2 = entity.RetroClaims2,
                RetroClaims3 = entity.RetroClaims3,
                TreatyCode = entity.TreatyCode,
                RetroPremiumSpread1 = entity.RetroPremiumSpread1,
                RetroPremiumSpread2 = entity.RetroPremiumSpread2,
                RetroPremiumSpread3 = entity.RetroPremiumSpread3,
                TotalDirectRetroAar = entity.TotalDirectRetroAar,
                ReportingType = entity.ReportingType,
                Mfrs17AnnualCohort = entity.Mfrs17AnnualCohort,
                Mfrs17ContractCode = entity.Mfrs17ContractCode,
            };

            if (foreign)
            {
                retroSummaryBo.DirectRetroBo = DirectRetroService.Find(entity.DirectRetroId);
            }

            if (formatOutput)
            {
                retroSummaryBo.TotalSarStr = Util.DoubleToString(entity.TotalSar, 2);
                retroSummaryBo.TotalRiPremiumStr = Util.DoubleToString(entity.TotalRiPremium, 2);
                retroSummaryBo.TotalDiscountStr = Util.DoubleToString(entity.TotalDiscount, 2);
                retroSummaryBo.RetroShare1Str = Util.DoubleToString(entity.RetroShare1);
                retroSummaryBo.RetroShare2Str = Util.DoubleToString(entity.RetroShare2);
                retroSummaryBo.RetroShare3Str = Util.DoubleToString(entity.RetroShare3);
                retroSummaryBo.RetroRiPremium1Str = Util.DoubleToString(entity.RetroRiPremium1, 2);
                retroSummaryBo.RetroRiPremium2Str = Util.DoubleToString(entity.RetroRiPremium2, 2);
                retroSummaryBo.RetroRiPremium3Str = Util.DoubleToString(entity.RetroRiPremium3, 2);
                retroSummaryBo.RetroDiscount1Str = Util.DoubleToString(entity.RetroDiscount1, 2);
                retroSummaryBo.RetroDiscount2Str = Util.DoubleToString(entity.RetroDiscount2, 2);
                retroSummaryBo.RetroDiscount3Str = Util.DoubleToString(entity.RetroDiscount3, 2);
                retroSummaryBo.RetroClaims1Str = Util.DoubleToString(entity.RetroClaims1, 2);
                retroSummaryBo.RetroClaims2Str = Util.DoubleToString(entity.RetroClaims2, 2);
                retroSummaryBo.RetroClaims3Str = Util.DoubleToString(entity.RetroClaims3, 2);
                retroSummaryBo.RetroPremiumSpread1Str = Util.DoubleToString(entity.RetroPremiumSpread1);
                retroSummaryBo.RetroPremiumSpread2Str = Util.DoubleToString(entity.RetroPremiumSpread2);
                retroSummaryBo.RetroPremiumSpread3Str = Util.DoubleToString(entity.RetroPremiumSpread3);
                retroSummaryBo.TotalDirectRetroAarStr = Util.DoubleToString(entity.TotalDirectRetroAar, 2);
            }

            return retroSummaryBo;
        }

        public static IList<RetroSummaryBo> FormBos(IList<RetroSummary> entities = null, bool foreign = true, bool formatOutput = false)
        {
            if (entities == null)
                return null;
            IList<RetroSummaryBo> bos = new List<RetroSummaryBo>() { };
            foreach (RetroSummary entity in entities)
            {
                bos.Add(FormBo(entity, foreign, formatOutput));
            }
            return bos;
        }

        public static RetroSummary FormEntity(RetroSummaryBo bo = null)
        {
            if (bo == null)
                return null;
            return new RetroSummary
            {
                Id = bo.Id,
                DirectRetroId = bo.DirectRetroId,
                RiskQuarter = bo.RiskQuarter,
                Month = bo.Month,
                Year = bo.Year,
                Type = bo.Type,
                NoOfPolicy = bo.NoOfPolicy,
                TotalSar = bo.TotalSar,
                TotalRiPremium = bo.TotalRiPremium,
                TotalDiscount = bo.TotalDiscount,
                NoOfClaims = bo.NoOfClaims,
                TotalClaims = bo.TotalClaims,
                RetroParty1 = bo.RetroParty1,
                RetroParty2 = bo.RetroParty2,
                RetroParty3 = bo.RetroParty3,
                RetroShare1 = bo.RetroShare1,
                RetroShare2 = bo.RetroShare2,
                RetroShare3 = bo.RetroShare3,
                RetroRiPremium1 = bo.RetroRiPremium1,
                RetroRiPremium2 = bo.RetroRiPremium2,
                RetroRiPremium3 = bo.RetroRiPremium3,
                RetroDiscount1 = bo.RetroDiscount1,
                RetroDiscount2 = bo.RetroDiscount2,
                RetroDiscount3 = bo.RetroDiscount3,
                RetroClaims1 = bo.RetroClaims1,
                RetroClaims2 = bo.RetroClaims2,
                RetroClaims3 = bo.RetroClaims3,
                TreatyCode = bo.TreatyCode,
                RetroPremiumSpread1 = bo.RetroPremiumSpread1,
                RetroPremiumSpread2 = bo.RetroPremiumSpread2,
                RetroPremiumSpread3 = bo.RetroPremiumSpread3,
                TotalDirectRetroAar = bo.TotalDirectRetroAar,
                ReportingType = bo.ReportingType,
                Mfrs17AnnualCohort = bo.Mfrs17AnnualCohort,
                Mfrs17ContractCode = bo.Mfrs17ContractCode,
                CreatedById = bo.CreatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return RetroSummary.IsExists(id);
        }

        public static RetroSummaryBo Find(int id)
        {
            return FormBo(RetroSummary.Find(id));
        }

        public static int CountByDirectRetroId(int directRetroId)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroSummaries.Where(q => q.DirectRetroId == directRetroId).Count();
            }
        }

        public static IList<RetroSummaryBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.RetroSummaries.ToList());
            }
        }

        public static IList<RetroSummaryBo> GetByDirectRetroId(int directRetroId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.RetroSummaries
                    .Where(q => q.DirectRetroId == directRetroId)
                    .OrderBy(q => q.TreatyCode)
                    .ThenByDescending(q => q.RiskQuarter)
                    .ThenByDescending(q => q.Year)
                    .ThenBy(q => q.Month)
                    .ToList(), false, true);
            }
        }

        public static IList<RetroSummaryBo> GetByDirectRetroIdReportingType(int directRetroId, int reportingType)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.RetroSummaries
                    .Where(q => q.DirectRetroId == directRetroId)
                    .Where(q => q.ReportingType == reportingType)
                    .OrderBy(q => q.TreatyCode)
                    .ThenByDescending(q => q.RiskQuarter)
                    .ThenByDescending(q => q.Year)
                    .ThenBy(q => q.Month)
                    .ToList(), false, true);
            }
        }

        public static void Create(RetroSummaryBo bo)
        {
            RetroSummary entity = FormEntity(bo);
            entity.Create();
        }

        public static Result Create(ref RetroSummaryBo bo)
        {
            RetroSummary entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RetroSummaryBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RetroSummaryBo bo)
        {
            RetroSummary.Delete(bo.Id);
        }

        public static Result Delete(RetroSummaryBo bo, ref TrailObject trail)
        {
            Result result = Result();
            DataTrail dataTrail = RetroSummary.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static IList<DataTrail> DeleteByDirectRetroId(int directRetroId)
        {
            return RetroSummary.DeleteByDirectRetroId(directRetroId);
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
