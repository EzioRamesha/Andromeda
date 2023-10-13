using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Newtonsoft.Json;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.TreatyPricing
{
    public class TreatyPricingAdvantageProgramVersionBenefitService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingAdvantageProgramVersionBenefit)),
                Controller = ModuleBo.ModuleController.TreatyPricingAdvantageProgramVersionBenefit.ToString()
            };
        }

        public static Expression<Func<TreatyPricingAdvantageProgramVersionBenefit, TreatyPricingAdvantageProgramVersionBenefitBo>> Expression()
        {
            return entity => new TreatyPricingAdvantageProgramVersionBenefitBo
            {
                Id = entity.Id,
                TreatyPricingAdvantageProgramVersionId = entity.TreatyPricingAdvantageProgramVersionId,
                BenefitId = entity.BenefitId,
                ExtraMortality = entity.ExtraMortality,
                SumAssured = entity.SumAssured,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingAdvantageProgramVersionBenefitBo FormBo(TreatyPricingAdvantageProgramVersionBenefit entity = null, bool foreign = false)
        {
            if (entity == null)
                return null;
            return new TreatyPricingAdvantageProgramVersionBenefitBo
            {
                Id = entity.Id,
                TreatyPricingAdvantageProgramVersionId = entity.TreatyPricingAdvantageProgramVersionId,
                TreatyPricingAdvantageProgramVersionBo = foreign ? TreatyPricingAdvantageProgramVersionService.Find(entity.TreatyPricingAdvantageProgramVersionId) : null,
                BenefitId = entity.BenefitId,
                BenefitBo = BenefitService.Find(entity.BenefitId),
                ExtraMortality = entity.ExtraMortality,
                SumAssured = entity.SumAssured,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                ExtraMortalityStr = Util.DoubleToString(entity.ExtraMortality, 2),
                SumAssuredStr = Util.DoubleToString(entity.SumAssured, 2),
                BenefitCode = BenefitService.Find(entity.BenefitId)?.Code,
            };
        }

        public static IList<TreatyPricingAdvantageProgramVersionBenefitBo> FormBos(IList<TreatyPricingAdvantageProgramVersionBenefit> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingAdvantageProgramVersionBenefitBo> bos = new List<TreatyPricingAdvantageProgramVersionBenefitBo>() { };
            foreach (TreatyPricingAdvantageProgramVersionBenefit entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingAdvantageProgramVersionBenefit FormEntity(TreatyPricingAdvantageProgramVersionBenefitBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingAdvantageProgramVersionBenefit
            {
                Id = bo.Id,
                TreatyPricingAdvantageProgramVersionId = bo.TreatyPricingAdvantageProgramVersionId,
                BenefitId = bo.BenefitId,
                ExtraMortality = bo.ExtraMortality,
                SumAssured = bo.SumAssured,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingAdvantageProgramVersionBenefit.IsExists(id);
        }

        public static TreatyPricingAdvantageProgramVersionBenefitBo Find(int id)
        {
            return FormBo(TreatyPricingAdvantageProgramVersionBenefit.Find(id));
        }

        // for listing purpose
        public static TreatyPricingAdvantageProgramVersionBenefitBo GetLatestBenefitSummarised(int id)
        {
            var latestVersionBo = TreatyPricingAdvantageProgramVersionService.GetLatestByTreatyPricingAdvantageProgramId(id);
            if (latestVersionBo == null)
                return null;

            var benefits = GetByTreatyPricingAdvantageProgramVersionId(latestVersionBo.Id);
            if (benefits.IsNullOrEmpty())
                return null;

            var bo = new TreatyPricingAdvantageProgramVersionBenefitBo();

            var benefitCodes = benefits.Select(q => q.BenefitCode).ToArray();
            bo.BenefitCode = string.Join(", ", benefitCodes);

            bo.SumAssured = benefits.Sum(q => q.SumAssured);
            bo.SumAssuredStr = Util.DoubleToString(bo.SumAssured, 2);

            bo.ExtraMortality = benefits.Sum(q => q.ExtraMortality);
            bo.ExtraMortalityStr = Util.DoubleToString(bo.ExtraMortality, 2);

            return bo;
        }

        public static IList<TreatyPricingAdvantageProgramVersionBenefitBo> GetByVersionId(int versionId, List<int> exceptions = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingAdvantageProgramVersionBenefits
                    .Where(q => q.TreatyPricingAdvantageProgramVersionId == versionId);

                if (exceptions != null)
                {
                    query = query.Where(q => !exceptions.Contains(q.Id));
                }

                return FormBos(query.ToList());
            }
        }

        public static IList<TreatyPricingAdvantageProgramVersionBenefitBo> GetByTreatyPricingAdvantageProgramVersionId(int treatyPricingAdvantageProgramVersionId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingAdvantageProgramVersionBenefits
                    .Where(q => q.TreatyPricingAdvantageProgramVersionId == treatyPricingAdvantageProgramVersionId)
                    .ToList());
            }
        }

        public static IList<TreatyPricingAdvantageProgramVersionBenefitBo> GetByTreatyPricingAdvantageProgramVersionIdExcept(int treatyPricingAdvantageProgramVersionId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingAdvantageProgramVersionBenefits
                    .Where(q => q.TreatyPricingAdvantageProgramVersionId == treatyPricingAdvantageProgramVersionId)
                    .Where(q => !ids.Contains(q.Id))
                    .ToList());
            }
        }

        public static int GetMaxCountByTreatyPricingAdvantageProgramVersionIds(List<int> treatyPricingAdvantageProgramVersionIds)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingAdvantageProgramVersionBenefits
                    .Where(q => treatyPricingAdvantageProgramVersionIds.Contains(q.TreatyPricingAdvantageProgramVersionId))
                    .GroupBy(q => q.TreatyPricingAdvantageProgramVersionId)
                    .Select(q => new { TreatyPricingAdvantageProgramVersionId = q.Key, Total = q.Count() })
                    .OrderByDescending(q => q.Total)
                    .ToList();

                return query.FirstOrDefault().Total;
            }
        }

        public static string GetJsonByVersionId(int versionId)
        {
            using (var db = new AppDbContext())
            {
                var bos = GetByVersionId(versionId);

                return JsonConvert.SerializeObject(bos);
            }
        }

        public static Result Save(ref TreatyPricingAdvantageProgramVersionBenefitBo bo)
        {
            if (!TreatyPricingAdvantageProgramVersionBenefit.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref TreatyPricingAdvantageProgramVersionBenefitBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingAdvantageProgramVersionBenefit.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingAdvantageProgramVersionBenefitBo bo)
        {
            TreatyPricingAdvantageProgramVersionBenefit entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingAdvantageProgramVersionBenefitBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingAdvantageProgramVersionBenefitBo bo)
        {
            Result result = Result();

            TreatyPricingAdvantageProgramVersionBenefit entity = TreatyPricingAdvantageProgramVersionBenefit.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingAdvantageProgramVersionId = bo.TreatyPricingAdvantageProgramVersionId;
                entity.BenefitId = bo.BenefitId;
                entity.ExtraMortality = bo.ExtraMortality;
                entity.SumAssured = bo.SumAssured;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingAdvantageProgramVersionBenefitBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingAdvantageProgramVersionBenefitBo bo)
        {
            TreatyPricingAdvantageProgramVersionBenefit.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingAdvantageProgramVersionBenefitBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingAdvantageProgramVersionBenefit.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingAdvantageProgramVersionId(int treatyPricingAdvantageProgramVersionId)
        {
            return TreatyPricingAdvantageProgramVersionBenefit.DeleteAllByTreatyPricingAdvantageProgramVersionId(treatyPricingAdvantageProgramVersionId);
        }

        public static void DeleteAllByTreatyPricingAdvantageProgramVersionId(int treatyPricingAdvantageProgramVersionId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingAdvantageProgramVersionId(treatyPricingAdvantageProgramVersionId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingAdvantageProgramVersionBenefit)));
                }
            }
        }

        public static Result GetByTreatyPricingAdvantageProgramVersionIdExcept(int treatyPricingAdvantageProgramVersionId, List<int> saveIds, ref TrailObject trail)
        {
            Result result = Result();
            IList<TreatyPricingAdvantageProgramVersionBenefitBo> bos = GetByTreatyPricingAdvantageProgramVersionIdExcept(treatyPricingAdvantageProgramVersionId, saveIds);
            foreach (TreatyPricingAdvantageProgramVersionBenefitBo bo in bos)
            {
                DataTrail dataTrail = TreatyPricingAdvantageProgramVersionBenefit.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void DeleteByVersionExcept(int versionId, List<int> exceptions, ref TrailObject trail)
        {
            foreach (var bo in GetByVersionId(versionId, exceptions))
            {
                Delete(bo, ref trail);
            }
        }

        public static void Save(int versionId, string benefit, int authUserId, ref TrailObject trail, bool resetId = false)
        {
            if (string.IsNullOrEmpty(benefit))
                return;

            List<int> ids = new List<int>();

            List<TreatyPricingAdvantageProgramVersionBenefitBo> bos = JsonConvert.DeserializeObject<List<TreatyPricingAdvantageProgramVersionBenefitBo>>(benefit);
            foreach (var benefitBo in bos)
            {
                var bo = benefitBo;
                if (resetId)
                    bo.Id = 0;

                bo.TreatyPricingAdvantageProgramVersionId = versionId;
                bo.UpdatedById = authUserId;

                if (bo.Id == 0)
                    bo.CreatedById = authUserId;

                Result result = Save(ref bo, ref trail);
                if (result.Valid)
                {
                    ids.Add(bo.Id);
                }
            }
            DeleteByVersionExcept(versionId, ids, ref trail);
        }
    }
}
