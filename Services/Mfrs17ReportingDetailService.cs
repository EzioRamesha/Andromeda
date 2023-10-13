using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class Mfrs17ReportingDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(Mfrs17ReportingDetail)),
                Controller = ModuleBo.ModuleController.Mfrs17ReportingDetail.ToString()
            };
        }

        public static Mfrs17ReportingDetailBo FormBo(Mfrs17ReportingDetail entity = null)
        {
            if (entity == null)
                return null;
            Mfrs17ReportingDetailBo mfrs17ReportingDetailBo = new Mfrs17ReportingDetailBo
            {
                Id = entity.Id,
                Mfrs17ReportingId = entity.Mfrs17ReportingId,
                CedantId = entity.CedantId,
                CedantBo = CedantService.Find(entity.CedantId),
                TreatyCode = entity.TreatyCode,
                PremiumFrequencyCodePickListDetailId = entity.PremiumFrequencyCodePickListDetailId,
                PremiumFrequencyCodePickListDetailBo = PickListDetailService.Find(entity.PremiumFrequencyCodePickListDetailId),
                RiskQuarter = entity.RiskQuarter,
                LatestDataStartDate = entity.LatestDataStartDate,
                LatestDataStartDateStr = entity.LatestDataStartDate.ToString(Util.GetDateFormat()),
                LatestDataEndDate = entity.LatestDataEndDate,
                LatestDataEndDateStr = entity.LatestDataEndDate.ToString(Util.GetDateFormat()),
                Record = entity.Record,
                Mfrs17TreatyCode = entity.Mfrs17TreatyCode,
                CedingPlanCode = entity.CedingPlanCode,
                Status = entity.Status,
                IsModified = entity.IsModified,
                GenerateStatus = entity.GenerateStatus,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
            return mfrs17ReportingDetailBo;
        }

        public static Mfrs17ReportingDetailBo FormBoFromDetailData(Mfrs17ReportingDetailData entity = null)
        {
            if (entity == null)
                return null;
            Mfrs17ReportingDetailBo mfrs17ReportingDetailBo = new Mfrs17ReportingDetailBo
            {
                CedantId = entity.CedantId,
                CedantBo = CedantService.Find(entity.CedantId),
                TreatyCode = entity.TreatyCode,
                PremiumFrequencyCodePickListDetailId = entity.PremiumFrequencyCodePickListDetailId,
                PremiumFrequencyCodePickListDetailBo = PickListDetailService.Find(entity.PremiumFrequencyCodePickListDetailId),
                RiskQuarter = entity.RiskQuarter,
                LatestDataStartDate = entity.LatestDataStartDate,
                LatestDataStartDateStr = entity.LatestDataStartDate.ToString(Util.GetDateFormat()),
                LatestDataEndDate = entity.LatestDataEndDate,
                LatestDataEndDateStr = entity.LatestDataEndDate.ToString(Util.GetDateFormat()),
                Record = entity.Record,
                Mfrs17TreatyCode = entity.Mfrs17TreatyCode,
                CedingPlanCode = entity.CedingPlanCode,
                Status = entity.Status,
            };
            return mfrs17ReportingDetailBo;
        }

        public static IList<Mfrs17ReportingDetailBo> FormBos(IList<Mfrs17ReportingDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<Mfrs17ReportingDetailBo> bos = new List<Mfrs17ReportingDetailBo>() { };
            foreach (Mfrs17ReportingDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static IList<Mfrs17ReportingDetailBo> FormBosFromDetailData(IList<Mfrs17ReportingDetailData> entities = null)
        {
            if (entities == null)
                return null;
            IList<Mfrs17ReportingDetailBo> bos = new List<Mfrs17ReportingDetailBo>() { };
            foreach (Mfrs17ReportingDetailData entity in entities)
            {
                bos.Add(FormBoFromDetailData(entity));
            }
            return bos;
        }

        public static Mfrs17ReportingDetail FormEntity(Mfrs17ReportingDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new Mfrs17ReportingDetail
            {
                Id = bo.Id,
                Mfrs17ReportingId = bo.Mfrs17ReportingId,
                CedantId = bo.CedantId,
                TreatyCode = bo.TreatyCode,
                PremiumFrequencyCodePickListDetailId = bo.PremiumFrequencyCodePickListDetailId,
                RiskQuarter = bo.RiskQuarter,
                LatestDataStartDate = bo.LatestDataStartDate,
                LatestDataEndDate = bo.LatestDataEndDate,
                Record = bo.Record,
                Mfrs17TreatyCode = bo.Mfrs17TreatyCode,
                CedingPlanCode = bo.CedingPlanCode,
                Status = bo.Status,
                IsModified = bo.IsModified,
                GenerateStatus = bo.GenerateStatus,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static Mfrs17ReportingDetailBo Find(int id)
        {
            return FormBo(Mfrs17ReportingDetail.Find(id));
        }

        public static Mfrs17ReportingDetailBo FindByMfrs17ReportingIdTreatyCodePaymentMode(int mfrs17ReportingId, string treatyCode, int paymentMode)
        {
            return FormBo(Mfrs17ReportingDetail.FindByMfrs17ReportingIdTreatyCodePaymentMode(mfrs17ReportingId, treatyCode, paymentMode));
        }

        public static int CountByMfrs17ReportingId(int mfrs17ReportingId)
        {
            return Mfrs17ReportingDetail.CountByMfrs17ReportingId(mfrs17ReportingId);
        }

        public static int CountByTreatyCodeId(int treatyCodeId)
        {
            return Mfrs17ReportingDetail.CountByTreatyCodeId(treatyCodeId);
        }

        public static Dictionary<int, DateTime> GetLatestEndDateByMfrs17ReportingId(int mfrs17Reportingid)
        {
            return Mfrs17ReportingDetail.GetLatestEndDateByMfrs17ReportingId(mfrs17Reportingid);
        }

        public static IList<Mfrs17ReportingDetailBo> GetByMfrs17ReportingId(int mfrs17Reportingid)
        {
            return FormBos(Mfrs17ReportingDetail.GetByMfrs17ReportingId(mfrs17Reportingid));
        }

        public static IList<Mfrs17ReportingDetailBo> GetByMfrs17ReportingIdStatus(int mfrs17Reportingid, int status)
        {
            List<int> statuses = new List<int>() { status };
            return FormBos(Mfrs17ReportingDetail.GetByMfrs17ReportingIdStatus(mfrs17Reportingid, statuses));
        }

        public static IList<Mfrs17ReportingDetailBo> GetByMfrs17ReportingIdStatus(int mfrs17Reportingid, List<int> statuses)
        {
            return FormBos(Mfrs17ReportingDetail.GetByMfrs17ReportingIdStatus(mfrs17Reportingid, statuses));
        }

        public static IList<Mfrs17ReportingDetailBo> GetByMfrs17ReportingId(int mfrs17Reportingid, int skip, int take)
        {
            return FormBos(Mfrs17ReportingDetail.GetByMfrs17ReportingId(mfrs17Reportingid, skip, take));
        }

        public static IList<Mfrs17ReportingDetailBo> GetByGroupedDetail(int mfrs17Reportingid, int cedantId, string treatyCode, int premiumFrequencyCodePickListDetailId, string riskQuarter, string cedingPlanCode, DateTime? latestDataStartDate = null, DateTime? latestDataEndDate = null)
        {
            return FormBos(Mfrs17ReportingDetail.GetByGroupedDetail(mfrs17Reportingid, cedantId, treatyCode, premiumFrequencyCodePickListDetailId, riskQuarter, cedingPlanCode, latestDataStartDate, latestDataEndDate));
        }

        // Count Data
        public static int CountDataByMfrs17ReportingId(int mfrs17Reportingid, bool isDefault = true)
        {
            return Mfrs17ReportingDetail.CountDataByMfrs17ReportingId(mfrs17Reportingid, isDefault);
        }

        public static int SumRecordsByMfrs17ReportingId(int mfrs17ReportingId)
        {
            return Mfrs17ReportingDetail.SumRecordsByMfrs17ReportingId(mfrs17ReportingId);
        }

        // Get Data
        public static IList<Mfrs17ReportingDetailBo> GetDataByMfrs17ReportingId(int mfrs17Reportingid, bool isDefault = true)
        {
            return FormBosFromDetailData(Mfrs17ReportingDetail.GetDataByMfrs17ReportingId(mfrs17Reportingid, isDefault));
        }

        public static IList<Mfrs17ReportingDetailBo> GetDataByMfrs17ReportingId(int mfrs17Reportingid, int skip, int take, bool isDefault = true)
        {
            return FormBosFromDetailData(Mfrs17ReportingDetail.GetDataByMfrs17ReportingId(mfrs17Reportingid, skip, take, isDefault));
        }

        public static IList<Mfrs17ReportingDetailBo> GetModifieBydMfrs17TreatyCode(int mfrs17Reportingid, string mfrs17TreatyCode, bool? modifiedOnly = false)
        {
            return FormBos(Mfrs17ReportingDetail.GetModifieBydMfrs17TreatyCode(mfrs17Reportingid, mfrs17TreatyCode, modifiedOnly.Value));
        }

        public static bool IsExistDeletedByMfrs17TreatyCodes(int mfrs17ReportingId, string mfrs17TreatyCode)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetailService");

                return connectionStrategy.Execute(() =>
                {
                    return db.Mfrs17ReportingDetails
                        .Where(q => q.Mfrs17ReportingId == mfrs17ReportingId)
                        .Where(q => q.Mfrs17TreatyCode == mfrs17TreatyCode)
                        .Where(q => q.Status == Mfrs17ReportingDetailBo.StatusDeleted)
                        .Any();
                });
            }
        }

        public static bool IsExistNonDeletedByMfrs17TreatyCodes(int mfrs17ReportingId, string mfrs17TreatyCode)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetailService");

                return connectionStrategy.Execute(() =>
                {
                    return db.Mfrs17ReportingDetails
                        .Where(q => q.Mfrs17ReportingId == mfrs17ReportingId)
                        .Where(q => q.Mfrs17TreatyCode == mfrs17TreatyCode)
                        .Where(q => q.Status != Mfrs17ReportingDetailBo.StatusDeleted)
                        .Any();
                });
            }
        }

        public static List<string> GetDistinctMfrs17TreatyCodes(int mfrs17Reportingid, bool? modifiedOnly = false, bool? resume = false)
        {
            return Mfrs17ReportingDetail.GetDistinctMfrs17TreatyCodes(mfrs17Reportingid, modifiedOnly.Value, resume.Value);
        }

        public static Result Save(ref Mfrs17ReportingDetailBo bo)
        {
            if (!Mfrs17ReportingDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref Mfrs17ReportingDetailBo bo, ref TrailObject trail)
        {
            if (!Mfrs17ReportingDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref Mfrs17ReportingDetailBo bo)
        {
            Mfrs17ReportingDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }
            return result;
        }

        public static Result Create(ref Mfrs17ReportingDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Create(Mfrs17ReportingDetailBo bo)
        {
            Mfrs17ReportingDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }
            return result;
        }

        public static Result Create(Mfrs17ReportingDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref Mfrs17ReportingDetailBo bo)
        {
            Result result = Result();

            Mfrs17ReportingDetail entity = Mfrs17ReportingDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Mfrs17ReportingId = bo.Mfrs17ReportingId;
                entity.CedantId = bo.CedantId;
                entity.TreatyCode = bo.TreatyCode;
                entity.PremiumFrequencyCodePickListDetailId = bo.PremiumFrequencyCodePickListDetailId;
                entity.RiskQuarter = bo.RiskQuarter;
                entity.LatestDataStartDate = bo.LatestDataStartDate;
                entity.LatestDataEndDate = bo.LatestDataEndDate;
                entity.Record = bo.Record;
                entity.Mfrs17TreatyCode = bo.Mfrs17TreatyCode;
                entity.CedingPlanCode = bo.CedingPlanCode;
                entity.Status = bo.Status;
                entity.IsModified = bo.IsModified;
                entity.GenerateStatus = bo.GenerateStatus;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref Mfrs17ReportingDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Delete(Mfrs17ReportingDetailBo bo)
        {
            Mfrs17ReportingDetailRiDataService.DeleteAllByMfrs17ReportingDetailId(bo.Id);
            Mfrs17ReportingDetail.Delete(bo.Id);

            return Result();
        }

        public static Result Delete(Mfrs17ReportingDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();
            DataTrail dataTrail = Mfrs17ReportingDetail.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }
    }
}
