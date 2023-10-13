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
    public class RetroBenefitRetentionLimitService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RetroBenefitRetentionLimit)),
                Controller = ModuleBo.ModuleController.RetroBenefitRetentionLimit.ToString()
            };
        }

        public static Expression<Func<RetroBenefitRetentionLimit, RetroBenefitRetentionLimitBo>> Expression()
        {
            return entity => new RetroBenefitRetentionLimitBo
            {
                Id = entity.Id,
                RetroBenefitCodeId = entity.RetroBenefitCodeId,
                Type = entity.Type,
                Description = entity.Description,
                EffectiveStartDate = entity.EffectiveStartDate,
                EffectiveEndDate = entity.EffectiveEndDate,
                MinRetentionLimit = entity.MinRetentionLimit,
                
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static Expression<Func<RetroBenefitRetentionLimitWithDetail, RetroBenefitRetentionLimitBo>> ExpressionWithDetail()
        {
            return entity => new RetroBenefitRetentionLimitBo
            {
                Id = entity.RetroBenefitRetentionLimit.Id,
                RetroBenefitCodeId = entity.RetroBenefitRetentionLimit.RetroBenefitCodeId,
                RetroBenefitCode = entity.RetroBenefitRetentionLimit.RetroBenefitCode.Code,
                Type = entity.RetroBenefitRetentionLimit.Type,
                Description = entity.RetroBenefitRetentionLimit.Description,
                EffectiveStartDate = entity.RetroBenefitRetentionLimit.EffectiveStartDate,
                EffectiveEndDate = entity.RetroBenefitRetentionLimit.EffectiveEndDate,
                MinRetentionLimit = entity.RetroBenefitRetentionLimit.MinRetentionLimit,

                // Detail
                ReinsEffStartDate = entity.RetroBenefitRetentionLimitDetail.ReinsEffStartDate,
                ReinsEffEndDate = entity.RetroBenefitRetentionLimitDetail.ReinsEffEndDate,
                MinIssueAge = entity.RetroBenefitRetentionLimitDetail.MinIssueAge,
                MaxIssueAge = entity.RetroBenefitRetentionLimitDetail.MaxIssueAge,
                MortalityLimitFrom = entity.RetroBenefitRetentionLimitDetail.MortalityLimitFrom,
                MortalityLimitTo = entity.RetroBenefitRetentionLimitDetail.MortalityLimitTo,
                MlreRetentionAmount = entity.RetroBenefitRetentionLimitDetail.MlreRetentionAmount,
                MinReinsAmount = entity.RetroBenefitRetentionLimitDetail.MinReinsAmount
            };
        }

        public static RetroBenefitRetentionLimitBo FormBo(RetroBenefitRetentionLimit entity = null)
        {
            if (entity == null)
                return null;
            return new RetroBenefitRetentionLimitBo
            {
                Id = entity.Id,
                RetroBenefitCodeId = entity.RetroBenefitCodeId,
                RetroBenefitCodeBo = RetroBenefitCodeService.Find(entity.RetroBenefitCodeId),
                Type = entity.Type,
                Description = entity.Description,
                EffectiveStartDate = entity.EffectiveStartDate,
                EffectiveEndDate = entity.EffectiveEndDate,
                MinRetentionLimit = entity.MinRetentionLimit,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<RetroBenefitRetentionLimitBo> FormBos(IList<RetroBenefitRetentionLimit> entities = null)
        {
            if (entities == null)
                return null;
            IList<RetroBenefitRetentionLimitBo> bos = new List<RetroBenefitRetentionLimitBo>() { };
            foreach (RetroBenefitRetentionLimit entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RetroBenefitRetentionLimit FormEntity(RetroBenefitRetentionLimitBo bo = null)
        {
            if (bo == null)
                return null;
            return new RetroBenefitRetentionLimit
            {
                Id = bo.Id,
                RetroBenefitCodeId = bo.RetroBenefitCodeId,
                Type = bo.Type,
                Description = bo.Description,
                EffectiveStartDate = bo.EffectiveStartDate,
                EffectiveEndDate = bo.EffectiveEndDate,
                MinRetentionLimit = bo.MinRetentionLimit,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return RetroBenefitRetentionLimit.IsExists(id);
        }

        public static RetroBenefitRetentionLimitBo Find(int? id)
        {
            return FormBo(RetroBenefitRetentionLimit.Find(id));
        }

        public static IList<RetroBenefitRetentionLimitBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.RetroBenefitRetentionLimits.OrderBy(q => q.RetroBenefitCode.Code).ToList());
            }
        }

        public static int CountByRetroBenefitCodeId(int retroBenefitCodeId)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroBenefitRetentionLimits.Where(q => q.RetroBenefitCodeId == retroBenefitCodeId).Count();
            }
        }

        public static Result Save(ref RetroBenefitRetentionLimitBo bo)
        {
            if (!RetroBenefitRetentionLimit.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref RetroBenefitRetentionLimitBo bo, ref TrailObject trail)
        {
            if (!RetroBenefitRetentionLimit.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static bool IsDuplicate(RetroBenefitRetentionLimit retroBenefitRetentionLimit)
        {
            return retroBenefitRetentionLimit.IsDuplicate();
        }

        public static Result Create(ref RetroBenefitRetentionLimitBo bo)
        {
            RetroBenefitRetentionLimit entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicate(entity))
            {
                result.AddError("Existing Retro Benefit Code's record found");
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RetroBenefitRetentionLimitBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RetroBenefitRetentionLimitBo bo)
        {
            Result result = Result();

            RetroBenefitRetentionLimit entity = RetroBenefitRetentionLimit.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicate(FormEntity(bo)))
            {
                result.AddError("Existing Retro Benefit Code's record found");
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.RetroBenefitCodeId = bo.RetroBenefitCodeId;
                entity.Type = bo.Type;
                entity.Description = bo.Description;
                entity.EffectiveStartDate = bo.EffectiveStartDate;
                entity.EffectiveEndDate = bo.EffectiveEndDate;
                entity.MinRetentionLimit = bo.MinRetentionLimit;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RetroBenefitRetentionLimitBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RetroBenefitRetentionLimitBo bo)
        {
            RetroBenefitRetentionLimitDetailService.DeleteByRetroBenefitRetentionLimitId(bo.Id);
            RetroBenefitRetentionLimit.Delete(bo.Id);
        }

        public static Result Delete(RetroBenefitRetentionLimitBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                RetroBenefitRetentionLimitDetailService.DeleteByRetroBenefitRetentionLimitId(bo.Id);
                DataTrail dataTrail = RetroBenefitRetentionLimit.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }

    public class RetroBenefitRetentionLimitWithDetail
    {
        public RetroBenefitRetentionLimit RetroBenefitRetentionLimit { get; set; }

        public RetroBenefitRetentionLimitDetail RetroBenefitRetentionLimitDetail { get; set; }
    }
}
