using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.TreatyPricing
{
    public class TreatyPricingCampaignService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingCampaign)),
                Controller = ModuleBo.ModuleController.TreatyPricingCampaign.ToString()
            };
        }

        public static TreatyPricingCampaignBo FormBo(TreatyPricingCampaign entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            var bo = new TreatyPricingCampaignBo
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                TreatyPricingCedantBo = foreign ? TreatyPricingCedantService.Find(entity.TreatyPricingCedantId) : null,
                Code = entity.Code,
                Name = entity.Name,
                Status = entity.Status,
                Type = entity.Type,
                Purpose = entity.Purpose,
                PeriodStartDate = entity.PeriodStartDate,
                PeriodEndDate = entity.PeriodEndDate,
                Duration = entity.Duration,
                TargetTakeUpRate = entity.TargetTakeUpRate,
                AverageSumAssured = entity.AverageSumAssured,
                RiPremiumReceivable = entity.RiPremiumReceivable,
                NoOfPolicy = entity.NoOfPolicy,
                Remarks = entity.Remarks,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                StatusName = TreatyPricingCampaignBo.GetStatusName(entity.Status),
                TreatyPricingCampaignVersionBos = foreign ? TreatyPricingCampaignVersionService.GetByTreatyPricingCampaignId(entity.Id) : null,
                PeriodStartDateStr = entity.PeriodStartDate?.ToString(Util.GetDateFormat()),
                PeriodEndDateStr = entity.PeriodEndDate?.ToString(Util.GetDateFormat()),
                AverageSumAssuredStr = Util.DoubleToString(entity.AverageSumAssured),
                RiPremiumReceivableStr = Util.DoubleToString(entity.RiPremiumReceivable),
            };
            return bo;
        }

        public static IList<TreatyPricingCampaignBo> FormBos(IList<TreatyPricingCampaign> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingCampaignBo> bos = new List<TreatyPricingCampaignBo>() { };
            foreach (TreatyPricingCampaign entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingCampaign FormEntity(TreatyPricingCampaignBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingCampaign
            {
                Id = bo.Id,
                TreatyPricingCedantId = bo.TreatyPricingCedantId,
                Code = bo.Code,
                Name = bo.Name,
                Status = bo.Status,
                Type = bo.Type,
                Purpose = bo.Purpose,
                PeriodStartDate = bo.PeriodStartDate,
                PeriodEndDate = bo.PeriodEndDate,
                Duration = bo.Duration,
                TargetTakeUpRate = bo.TargetTakeUpRate,
                AverageSumAssured = bo.AverageSumAssured,
                RiPremiumReceivable = bo.RiPremiumReceivable,
                NoOfPolicy = bo.NoOfPolicy,
                Remarks = bo.Remarks,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingCampaign.IsExists(id);
        }

        public static TreatyPricingCampaignBo Find(int? id)
        {
            return FormBo(TreatyPricingCampaign.Find(id));
        }

        public static IList<TreatyPricingCampaignBo> FindByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingCampaigns.Where(q => q.Status == status).ToList());
            }
        }

        public static IList<TreatyPricingCampaignBo> GetByTreatyPricingCedantId(int treatyPricingCedantId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingCampaigns
                    .Where(q => q.TreatyPricingCedantId == treatyPricingCedantId)
                    .ToList(), false);
            }
        }

        public static string GetNextObjectId(int treatyPricingCedantId)
        {
            using (var db = new AppDbContext())
            {
                var tpCedant = db.TreatyPricingCedants.Where(q => q.Id == treatyPricingCedantId).FirstOrDefault();
                string cedantCode = tpCedant.Code;
                int year = DateTime.Today.Year;

                string prefix = string.Format("{0}_CPG_{1}_", cedantCode, year);

                var entity = db.TreatyPricingCampaigns.Where(q => !string.IsNullOrEmpty(q.Code) && q.Code.StartsWith(prefix)).OrderByDescending(q => q.Code).FirstOrDefault();

                int nextIndex = 0;
                if (entity != null)
                {
                    string code = entity.Code;
                    string currentIndexStr = code.Substring(code.LastIndexOf('_') + 1);

                    int.TryParse(currentIndexStr, out nextIndex);
                }
                nextIndex++;
                string nextIndexStr = nextIndex.ToString().PadLeft(3, '0');

                return prefix + nextIndexStr;
            }
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingCampaigns.Where(q => q.Status == status).Count();
            }
        }

        public static Result Save(ref TreatyPricingCampaignBo bo)
        {
            if (!TreatyPricingCampaign.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingCampaignBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingCampaign.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingCampaignBo bo)
        {
            TreatyPricingCampaign entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingCampaignBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingCampaignBo bo)
        {
            Result result = Result();

            TreatyPricingCampaign entity = TreatyPricingCampaign.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity = FormEntity(bo);
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingCampaignBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingCampaignBo bo)
        {
            TreatyPricingCampaign.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingCampaignBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingCampaign.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
