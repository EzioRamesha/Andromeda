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
using System.Text;
using System.Threading.Tasks;

namespace Services.Retrocession
{
    public class PerLifeRetroConfigurationRatioService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PerLifeRetroConfigurationRatio)),
                Controller = ModuleBo.ModuleController.PerLifeRetroConfigurationRatio.ToString()
            };
        }

        public static Expression<Func<PerLifeRetroConfigurationRatio, PerLifeRetroConfigurationRatioBo>> Expression()
        {
            return entity => new PerLifeRetroConfigurationRatioBo
            {
                Id = entity.Id,
                TreatyCodeId = entity.TreatyCodeId,
                TreatyCode = entity.TreatyCode.Code,
                RetroRatio = entity.RetroRatio,
                MlreRetainRatio = entity.MlreRetainRatio,
                ReinsEffectiveStartDate = entity.ReinsEffectiveStartDate,
                ReinsEffectiveEndDate = entity.ReinsEffectiveEndDate,
                RiskQuarterStartDate = entity.RiskQuarterStartDate,
                RiskQuarterEndDate = entity.RiskQuarterEndDate,
                RuleEffectiveDate = entity.RuleEffectiveDate,
                RuleCeaseDate = entity.RuleCeaseDate,
                RuleValue = entity.RuleValue,
                Description = entity.Description,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static PerLifeRetroConfigurationRatioBo FormBo(PerLifeRetroConfigurationRatio entity = null)
        {
            if (entity == null)
                return null;
            return new PerLifeRetroConfigurationRatioBo
            {
                Id = entity.Id,
                TreatyCodeId = entity.TreatyCodeId,
                TreatyCodeBo = TreatyCodeService.Find(entity.TreatyCodeId),
                RetroRatio = entity.RetroRatio,
                MlreRetainRatio = entity.MlreRetainRatio,
                ReinsEffectiveStartDate = entity.ReinsEffectiveStartDate,
                ReinsEffectiveEndDate = entity.ReinsEffectiveEndDate,
                RiskQuarterStartDate = entity.RiskQuarterStartDate,
                RiskQuarterEndDate = entity.RiskQuarterEndDate,
                RuleEffectiveDate = entity.RuleEffectiveDate,
                RuleCeaseDate = entity.RuleCeaseDate,
                RuleValue = entity.RuleValue,
                Description = entity.Description,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<PerLifeRetroConfigurationRatioBo> FormBos(IList<PerLifeRetroConfigurationRatio> entities = null)
        {
            if (entities == null)
                return null;
            IList<PerLifeRetroConfigurationRatioBo> bos = new List<PerLifeRetroConfigurationRatioBo>() { };
            foreach (PerLifeRetroConfigurationRatio entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static PerLifeRetroConfigurationRatio FormEntity(PerLifeRetroConfigurationRatioBo bo = null)
        {
            if (bo == null)
                return null;
            return new PerLifeRetroConfigurationRatio
            {
                Id = bo.Id,
                TreatyCodeId = bo.TreatyCodeId,
                RetroRatio = bo.RetroRatio,
                MlreRetainRatio = bo.MlreRetainRatio,
                ReinsEffectiveStartDate = bo.ReinsEffectiveStartDate,
                ReinsEffectiveEndDate = bo.ReinsEffectiveEndDate,
                RiskQuarterStartDate = bo.RiskQuarterStartDate,
                RiskQuarterEndDate = bo.RiskQuarterEndDate,
                RuleEffectiveDate = bo.RuleEffectiveDate,
                RuleCeaseDate = bo.RuleCeaseDate,
                RuleValue = bo.RuleValue,
                Description = bo.Description,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return PerLifeRetroConfigurationRatio.IsExists(id);
        }

        public static PerLifeRetroConfigurationRatioBo Find(int? id)
        {
            return FormBo(PerLifeRetroConfigurationRatio.Find(id));
        }

        public static IList<PerLifeRetroConfigurationRatioBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeRetroConfigurationRatios.OrderBy(q => q.TreatyCode.Code).ToList());
            }
        }

        public static Result Save(ref PerLifeRetroConfigurationRatioBo bo)
        {
            if (!PerLifeRetroConfigurationRatio.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref PerLifeRetroConfigurationRatioBo bo, ref TrailObject trail)
        {
            if (!PerLifeRetroConfigurationRatio.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static bool IsDuplicate(PerLifeRetroConfigurationRatio PerLifeRetroConfigurationRatio)
        {
            return PerLifeRetroConfigurationRatio.IsDuplicate();
        }

        public static Result Create(ref PerLifeRetroConfigurationRatioBo bo)
        {
            PerLifeRetroConfigurationRatio entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicate(entity))
            {
                result.AddError("Existing Per Life Retro Ratio Combination found");
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref PerLifeRetroConfigurationRatioBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref PerLifeRetroConfigurationRatioBo bo)
        {
            Result result = Result();

            PerLifeRetroConfigurationRatio entity = PerLifeRetroConfigurationRatio.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicate(FormEntity(bo)))
            {
                result.AddError("Existing Per Life Retro Ratio Combination found");
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyCodeId = bo.TreatyCodeId;
                entity.RetroRatio = bo.RetroRatio;
                entity.MlreRetainRatio = bo.MlreRetainRatio;
                entity.ReinsEffectiveStartDate = bo.ReinsEffectiveStartDate;
                entity.ReinsEffectiveEndDate = bo.ReinsEffectiveEndDate;
                entity.RiskQuarterStartDate = bo.RiskQuarterStartDate;
                entity.RiskQuarterEndDate = bo.RiskQuarterEndDate;
                entity.RuleEffectiveDate = bo.RuleEffectiveDate;
                entity.RuleCeaseDate = bo.RuleCeaseDate;
                entity.RuleValue = bo.RuleValue;
                entity.Description = bo.Description;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref PerLifeRetroConfigurationRatioBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(PerLifeRetroConfigurationRatioBo bo)
        {
            PerLifeRetroConfigurationRatio.Delete(bo.Id);
        }

        public static Result Delete(PerLifeRetroConfigurationRatioBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: Add validation here
            //if (

            //)
            //{
            //    result.AddErrorRecordInUsed();
            //}

            if (result.Valid)
            {
                DataTrail dataTrail = PerLifeRetroConfigurationRatio.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
