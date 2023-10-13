using BusinessObject;
using BusinessObject.Retrocession;
using DataAccess.Entities.Retrocession;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.Retrocession
{
    public class RetroBenefitRetentionLimitDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RetroBenefitRetentionLimitDetail)),
                Controller = ModuleBo.ModuleController.RetroBenefitRetentionLimitDetail.ToString()
            };
        }

        public static Expression<Func<RetroBenefitRetentionLimitDetail, RetroBenefitRetentionLimitDetailBo>> Expression()
        {
            return entity => new RetroBenefitRetentionLimitDetailBo
            {
                Id = entity.Id,
                RetroBenefitRetentionLimitId = entity.RetroBenefitRetentionLimitId,
                MinIssueAge = entity.MinIssueAge,
                MaxIssueAge = entity.MaxIssueAge,
                MortalityLimitFrom = entity.MortalityLimitFrom,
                MortalityLimitTo = entity.MortalityLimitTo,
                ReinsEffStartDate = entity.ReinsEffStartDate,
                ReinsEffEndDate = entity.ReinsEffEndDate,
                MlreRetentionAmount = entity.MlreRetentionAmount,
                MinReinsAmount = entity.MinReinsAmount,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static RetroBenefitRetentionLimitDetailBo FormBo(RetroBenefitRetentionLimitDetail entity = null)
        {
            if (entity == null)
                return null;

            return new RetroBenefitRetentionLimitDetailBo
            {
                Id = entity.Id,
                RetroBenefitRetentionLimitId = entity.RetroBenefitRetentionLimitId,
                RetroBenefitRetentionLimitBo = RetroBenefitRetentionLimitService.Find(entity.RetroBenefitRetentionLimitId),
                MinIssueAge = entity.MinIssueAge,
                MaxIssueAge = entity.MaxIssueAge,
                MortalityLimitFrom = entity.MortalityLimitFrom,
                MortalityLimitTo = entity.MortalityLimitTo,
                ReinsEffStartDate = entity.ReinsEffStartDate,
                ReinsEffEndDate = entity.ReinsEffEndDate,
                MlreRetentionAmount = entity.MlreRetentionAmount,
                MinReinsAmount = entity.MinReinsAmount,

                MinIssueAgeStr = entity.MinIssueAge.ToString(),
                MaxIssueAgeStr = entity.MaxIssueAge.ToString(),
                MortalityLimitFromStr = Util.DoubleToString(entity.MortalityLimitFrom),
                MortalityLimitToStr = Util.DoubleToString(entity.MortalityLimitTo),
                ReinsEffStartDateStr = entity.ReinsEffStartDate.ToString(Util.GetDateFormat()),
                ReinsEffEndDateStr = entity.ReinsEffEndDate.ToString(Util.GetDateFormat()),
                MlreRetentionAmountStr = Util.DoubleToString(entity.MlreRetentionAmount),
                MinReinsAmountStr = Util.DoubleToString(entity.MinReinsAmount),

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<RetroBenefitRetentionLimitDetailBo> FormBos(IList<RetroBenefitRetentionLimitDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<RetroBenefitRetentionLimitDetailBo> bos = new List<RetroBenefitRetentionLimitDetailBo>() { };
            foreach (RetroBenefitRetentionLimitDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RetroBenefitRetentionLimitDetail FormEntity(RetroBenefitRetentionLimitDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new RetroBenefitRetentionLimitDetail
            {
                Id = bo.Id,
                RetroBenefitRetentionLimitId = bo.RetroBenefitRetentionLimitId,
                MinIssueAge = bo.MinIssueAge,
                MaxIssueAge = bo.MaxIssueAge,
                MortalityLimitFrom = bo.MortalityLimitFrom,
                MortalityLimitTo = bo.MortalityLimitTo,
                ReinsEffStartDate = bo.ReinsEffStartDate,
                ReinsEffEndDate = bo.ReinsEffEndDate,
                MlreRetentionAmount = bo.MlreRetentionAmount,
                MinReinsAmount = bo.MinReinsAmount,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return RetroBenefitRetentionLimitDetail.IsExists(id);
        }

        public static RetroBenefitRetentionLimitDetailBo Find(int id)
        {
            return FormBo(RetroBenefitRetentionLimitDetail.Find(id));
        }

        public static IList<RetroBenefitRetentionLimitDetailBo> GetByRetroBenefitRetentionLimitId(int retroBenefitRetentionLimitId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.RetroBenefitRetentionLimitDetails.Where(q => q.RetroBenefitRetentionLimitId == retroBenefitRetentionLimitId).ToList());
            }
        }

        public static IList<RetroBenefitRetentionLimitDetailBo> GetByRetroBenefitRetentionLimitIdExcept(int retroBenefitRetentionLimitId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.RetroBenefitRetentionLimitDetails.Where(q => q.RetroBenefitRetentionLimitId == retroBenefitRetentionLimitId && !ids.Contains(q.Id)).ToList());
            }
        }

        public static Result Save(ref RetroBenefitRetentionLimitDetailBo bo)
        {
            if (!RetroBenefitRetentionLimitDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref RetroBenefitRetentionLimitDetailBo bo, ref TrailObject trail)
        {
            if (!RetroBenefitRetentionLimitDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RetroBenefitRetentionLimitDetailBo bo)
        {
            RetroBenefitRetentionLimitDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RetroBenefitRetentionLimitDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RetroBenefitRetentionLimitDetailBo bo)
        {
            Result result = Result();

            RetroBenefitRetentionLimitDetail entity = RetroBenefitRetentionLimitDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.RetroBenefitRetentionLimitId = bo.RetroBenefitRetentionLimitId;
                entity.MinIssueAge = bo.MinIssueAge;
                entity.MaxIssueAge = bo.MaxIssueAge;
                entity.MortalityLimitFrom = bo.MortalityLimitFrom;
                entity.MortalityLimitTo = bo.MortalityLimitTo;
                entity.ReinsEffStartDate = bo.ReinsEffStartDate;
                entity.ReinsEffEndDate = bo.ReinsEffEndDate;
                entity.MlreRetentionAmount = bo.MlreRetentionAmount;
                entity.MinReinsAmount = bo.MinReinsAmount;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RetroBenefitRetentionLimitDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RetroBenefitRetentionLimitDetailBo bo)
        {
            RetroBenefitRetentionLimitDetail.Delete(bo.Id);
        }

        public static Result Delete(RetroBenefitRetentionLimitDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = RetroBenefitRetentionLimitDetail.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static Result DeleteByRetroBenefitRetentionLimitIdExcept(int retroBenefitRetentionLimitId, List<int> saveIds, ref TrailObject trail)
        {
            Result result = Result();
            IList<RetroBenefitRetentionLimitDetail> retroBenefitRetentionLimitDetails = RetroBenefitRetentionLimitDetail.GetByRetroBenefitRetentionLimitIdExcept(retroBenefitRetentionLimitId, saveIds);
            foreach (RetroBenefitRetentionLimitDetail retroBenefitRetentionLimitDetail in retroBenefitRetentionLimitDetails)
            {
                DataTrail dataTrail = RetroBenefitRetentionLimitDetail.Delete(retroBenefitRetentionLimitDetail.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static IList<DataTrail> DeleteByRetroBenefitRetentionLimitId(int retroBenefitRetentionLimitId)
        {
            return RetroBenefitRetentionLimitDetail.DeleteByRetroBenefitRetentionLimitId(retroBenefitRetentionLimitId);
        }

        public static void DeleteByRetroBenefitRetentionLimitId(int retroBenefitRetentionLimitId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteByRetroBenefitRetentionLimitId(retroBenefitRetentionLimitId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(RetroBenefitRetentionLimitDetail)));
                }
            }
        }
    }
}
