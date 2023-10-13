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
    public class PerLifeRetroConfigurationTreatyService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PerLifeRetroConfigurationTreaty)),
                Controller = ModuleBo.ModuleController.PerLifeRetroConfigurationTreaty.ToString()
            };
        }

        public static Expression<Func<PerLifeRetroConfigurationTreaty, PerLifeRetroConfigurationTreatyBo>> Expression()
        {
            return entity => new PerLifeRetroConfigurationTreatyBo
            {
                Id = entity.Id,
                TreatyCodeId = entity.TreatyCodeId,
                TreatyCode = entity.TreatyCode.Code,
                TreatyTypePickListDetailId = entity.TreatyTypePickListDetailId,
                TreatyType = entity.TreatyTypePickListDetail.Code,
                FundsAccountingTypePickListDetailId = entity.FundsAccountingTypePickListDetailId,
                FundsAccountingType = entity.FundsAccountingTypePickListDetail.Code,
                ReinsEffectiveStartDate = entity.ReinsEffectiveStartDate,
                ReinsEffectiveEndDate = entity.ReinsEffectiveEndDate,
                RiskQuarterStartDate = entity.RiskQuarterStartDate,
                RiskQuarterEndDate = entity.RiskQuarterEndDate,
                IsToAggregate = entity.IsToAggregate,
                Remark = entity.Remark,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static PerLifeRetroConfigurationTreatyBo FormBo(PerLifeRetroConfigurationTreaty entity = null)
        {
            if (entity == null)
                return null;
            return new PerLifeRetroConfigurationTreatyBo
            {
                Id = entity.Id,
                TreatyCodeId = entity.TreatyCodeId,
                TreatyCodeBo = TreatyCodeService.Find(entity.TreatyCodeId),
                TreatyTypePickListDetailId = entity.TreatyTypePickListDetailId,
                TreatyTypePickListDetailBo = PickListDetailService.Find(entity.TreatyTypePickListDetailId),
                FundsAccountingTypePickListDetailId = entity.FundsAccountingTypePickListDetailId,
                FundsAccountingTypePickListDetailBo = PickListDetailService.Find(entity.FundsAccountingTypePickListDetailId),
                ReinsEffectiveStartDate = entity.ReinsEffectiveStartDate,
                ReinsEffectiveEndDate = entity.ReinsEffectiveEndDate,
                RiskQuarterStartDate = entity.RiskQuarterStartDate,
                RiskQuarterEndDate = entity.RiskQuarterEndDate,
                IsToAggregate = entity.IsToAggregate,
                Remark = entity.Remark,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,

                ReinsEffectiveStartDateStr = entity.ReinsEffectiveStartDate.ToString(Util.GetDateFormat()),
                ReinsEffectiveEndDateStr = entity.ReinsEffectiveEndDate.ToString(Util.GetDateFormat()),
                RiskQuarterStartDateStr = entity.RiskQuarterStartDate.ToString(Util.GetDateFormat()),
                RiskQuarterEndDateStr = entity.RiskQuarterEndDate.ToString(Util.GetDateFormat()),
            };
        }

        public static IList<PerLifeRetroConfigurationTreatyBo> FormBos(IList<PerLifeRetroConfigurationTreaty> entities = null)
        {
            if (entities == null)
                return null;
            IList<PerLifeRetroConfigurationTreatyBo> bos = new List<PerLifeRetroConfigurationTreatyBo>() { };
            foreach (PerLifeRetroConfigurationTreaty entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static PerLifeRetroConfigurationTreaty FormEntity(PerLifeRetroConfigurationTreatyBo bo = null)
        {
            if (bo == null)
                return null;
            return new PerLifeRetroConfigurationTreaty
            {
                Id = bo.Id,
                TreatyCodeId = bo.TreatyCodeId,
                TreatyTypePickListDetailId = bo.TreatyTypePickListDetailId,
                FundsAccountingTypePickListDetailId = bo.FundsAccountingTypePickListDetailId,
                ReinsEffectiveStartDate = bo.ReinsEffectiveStartDate,
                ReinsEffectiveEndDate = bo.ReinsEffectiveEndDate,
                RiskQuarterStartDate = bo.RiskQuarterStartDate,
                RiskQuarterEndDate = bo.RiskQuarterEndDate,
                IsToAggregate = bo.IsToAggregate,
                Remark = bo.Remark,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return PerLifeRetroConfigurationTreaty.IsExists(id);
        }

        public static PerLifeRetroConfigurationTreatyBo Find(int? id)
        {
            return FormBo(PerLifeRetroConfigurationTreaty.Find(id));
        }

        public static PerLifeRetroConfigurationTreatyBo FindByParam(PerLifeRetroConfigurationTreatyBo perLifeRetroConfigurationTreatyBo)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.PerLifeRetroConfigurationTreaties
                    .Where(q => q.TreatyCodeId == perLifeRetroConfigurationTreatyBo.TreatyCodeId)
                    .Where(q => q.TreatyTypePickListDetailId == perLifeRetroConfigurationTreatyBo.TreatyTypePickListDetailId)
                    .Where(q => q.FundsAccountingTypePickListDetailId == perLifeRetroConfigurationTreatyBo.FundsAccountingTypePickListDetailId)
                    .Where(q => q.ReinsEffectiveStartDate == perLifeRetroConfigurationTreatyBo.ReinsEffectiveStartDate)
                    .Where(q => q.ReinsEffectiveEndDate == perLifeRetroConfigurationTreatyBo.ReinsEffectiveEndDate)
                    .Where(q => q.RiskQuarterStartDate == perLifeRetroConfigurationTreatyBo.RiskQuarterStartDate)
                    .Where(q => q.RiskQuarterEndDate == perLifeRetroConfigurationTreatyBo.RiskQuarterEndDate)
                    .Where(q => q.IsToAggregate == perLifeRetroConfigurationTreatyBo.IsToAggregate)
                    .FirstOrDefault());
            }
        }

        public static IList<PerLifeRetroConfigurationTreatyBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeRetroConfigurationTreaties.OrderBy(q => q.TreatyCode.Code).ToList());
            }
        }

        public static IList<PerLifeRetroConfigurationTreatyBo> GetByParam(string treatyCode, int? treatyTypeId, bool? isToAggregate, int? retroTreatyId)
        {
            using (var db = new AppDbContext())
            {
                IQueryable<PerLifeRetroConfigurationTreaty> query = db.PerLifeRetroConfigurationTreaties;

                if (!string.IsNullOrEmpty(treatyCode))
                {
                    query = query.Where(q => q.TreatyCode.Code == treatyCode);
                }

                if (treatyTypeId.HasValue)
                {
                    query = query.Where(q => q.TreatyTypePickListDetailId == treatyTypeId);
                }

                if (isToAggregate.HasValue)
                {
                    query = query.Where(q => q.IsToAggregate == isToAggregate);
                }

                if (retroTreatyId.HasValue)
                {
                    var ids = db.RetroTreatyDetails.Where(q => q.RetroTreatyId == retroTreatyId).Select(q => q.PerLifeRetroConfigurationTreatyId).ToList();
                    query = query.Where(q => !ids.Contains(q.Id));
                }

                return FormBos(query.OrderBy(q => q.TreatyCode.Code).ToList());
            }
        }

        public static int CountByParam(string treatyCode, int? treatyTypeId, bool? isToAggregate)
        {
            using (var db = new AppDbContext())
            {
                IQueryable<PerLifeRetroConfigurationTreaty> query = db.PerLifeRetroConfigurationTreaties;

                if (!string.IsNullOrEmpty(treatyCode))
                {
                    query = query.Where(q => q.TreatyCode.Code == treatyCode);
                }

                if (treatyTypeId.HasValue)
                {
                    query = query.Where(q => q.TreatyTypePickListDetailId == treatyTypeId);
                }

                if (isToAggregate.HasValue)
                {
                    query = query.Where(q => q.IsToAggregate == isToAggregate);
                }

                return query.Count();
            }
        }

        public static Result Save(ref PerLifeRetroConfigurationTreatyBo bo)
        {
            if (!PerLifeRetroConfigurationTreaty.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref PerLifeRetroConfigurationTreatyBo bo, ref TrailObject trail)
        {
            if (!PerLifeRetroConfigurationTreaty.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static bool IsDuplicate(PerLifeRetroConfigurationTreaty perLifeRetroConfigurationTreaty)
        {
            return perLifeRetroConfigurationTreaty.IsDuplicate();
        }

        public static Result Create(ref PerLifeRetroConfigurationTreatyBo bo)
        {
            PerLifeRetroConfigurationTreaty entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicate(entity))
            {
                result.AddError("Existing Per Life Retro Treaty Combination found");
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref PerLifeRetroConfigurationTreatyBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref PerLifeRetroConfigurationTreatyBo bo)
        {
            Result result = Result();

            PerLifeRetroConfigurationTreaty entity = PerLifeRetroConfigurationTreaty.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicate(FormEntity(bo)))
            {
                result.AddError("Existing Per Life Retro Treaty Combination found");
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyCodeId = bo.TreatyCodeId;
                entity.TreatyTypePickListDetailId = bo.TreatyTypePickListDetailId;
                entity.FundsAccountingTypePickListDetailId = bo.FundsAccountingTypePickListDetailId;
                entity.ReinsEffectiveStartDate = bo.ReinsEffectiveStartDate;
                entity.ReinsEffectiveEndDate = bo.ReinsEffectiveEndDate;
                entity.RiskQuarterStartDate = bo.RiskQuarterStartDate;
                entity.RiskQuarterEndDate = bo.RiskQuarterEndDate;
                entity.IsToAggregate = bo.IsToAggregate;
                entity.Remark = bo.Remark;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref PerLifeRetroConfigurationTreatyBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(PerLifeRetroConfigurationTreatyBo bo)
        {
            PerLifeRetroConfigurationTreaty.Delete(bo.Id);
        }

        public static Result Delete(PerLifeRetroConfigurationTreatyBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (RetroTreatyDetailService.CountByPerLifeRetroConfigurationTreatyId(bo.Id) > 0)
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                DataTrail dataTrail = PerLifeRetroConfigurationTreaty.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
