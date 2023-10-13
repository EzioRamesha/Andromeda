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
    public class TreatyPricingPerLifeRetroVersionBenefitService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingPerLifeRetroVersionBenefit)),
                Controller = ModuleBo.ModuleController.TreatyPricingPerLifeRetroVersionBenefit.ToString()
            };
        }

        public static Expression<Func<TreatyPricingPerLifeRetroVersionBenefit, TreatyPricingPerLifeRetroVersionBenefitBo>> Expression()
        {
            return entity => new TreatyPricingPerLifeRetroVersionBenefitBo
            {
                Id = entity.Id,
                TreatyPricingPerLifeRetroVersionId = entity.TreatyPricingPerLifeRetroVersionId,
                BenefitId = entity.BenefitId,
                ArrangementRetrocessionnaireTypePickListDetailId = entity.ArrangementRetrocessionnaireTypePickListDetailId,
                TotalMortality = entity.TotalMortality,
                MlreRetention = entity.MlreRetention,
                RetrocessionnaireShare = entity.RetrocessionnaireShare,
                AgeBasisPickListDetailId = entity.AgeBasisPickListDetailId,
                MinIssueAge = entity.MinIssueAge,
                MaxIssueAge = entity.MaxIssueAge,
                MaxExpiryAge = entity.MaxExpiryAge,
                RetrocessionaireDiscount = entity.RetrocessionaireDiscount,
                RateTablePercentage = entity.RateTablePercentage,
                ClaimApprovalLimit = entity.ClaimApprovalLimit,
                AutoBindingLimit = entity.AutoBindingLimit,
                IsProfitCommission = entity.IsProfitCommission,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingPerLifeRetroVersionBenefitBo FormBo(TreatyPricingPerLifeRetroVersionBenefit entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingPerLifeRetroVersionBenefitBo
            {
                Id = entity.Id,
                TreatyPricingPerLifeRetroVersionId = entity.TreatyPricingPerLifeRetroVersionId,
                //TreatyPricingPerLifeRetroVersionBo = TreatyPricingPerLifeRetroVersionService.Find(entity.TreatyPricingPerLifeRetroVersionId),
                BenefitId = entity.BenefitId,
                BenefitBo = BenefitService.Find(entity.BenefitId),
                BenefitCode = BenefitService.Find(entity.BenefitId).Code,
                BenefitName = BenefitService.Find(entity.BenefitId).Description,
                ArrangementRetrocessionnaireTypePickListDetailId = entity.ArrangementRetrocessionnaireTypePickListDetailId,
                TotalMortality = entity.TotalMortality,
                MlreRetention = entity.MlreRetention,
                RetrocessionnaireShare = entity.RetrocessionnaireShare,
                AgeBasisPickListDetailId = entity.AgeBasisPickListDetailId,
                MinIssueAge = entity.MinIssueAge,
                MaxIssueAge = entity.MaxIssueAge,
                MaxExpiryAge = entity.MaxExpiryAge,
                RetrocessionaireDiscount = entity.RetrocessionaireDiscount,
                RateTablePercentage = entity.RateTablePercentage,
                ClaimApprovalLimit = entity.ClaimApprovalLimit,
                AutoBindingLimit = entity.AutoBindingLimit,
                IsProfitCommission = entity.IsProfitCommission,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<TreatyPricingPerLifeRetroVersionBenefitBo> FormBos(IList<TreatyPricingPerLifeRetroVersionBenefit> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingPerLifeRetroVersionBenefitBo> bos = new List<TreatyPricingPerLifeRetroVersionBenefitBo>() { };
            foreach (TreatyPricingPerLifeRetroVersionBenefit entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingPerLifeRetroVersionBenefit FormEntity(TreatyPricingPerLifeRetroVersionBenefitBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingPerLifeRetroVersionBenefit
            {
                Id = bo.Id,
                TreatyPricingPerLifeRetroVersionId = bo.TreatyPricingPerLifeRetroVersionId,
                BenefitId = bo.BenefitId,
                ArrangementRetrocessionnaireTypePickListDetailId = bo.ArrangementRetrocessionnaireTypePickListDetailId,
                TotalMortality = bo.TotalMortality,
                MlreRetention = bo.MlreRetention,
                RetrocessionnaireShare = bo.RetrocessionnaireShare,
                AgeBasisPickListDetailId = bo.AgeBasisPickListDetailId,
                MinIssueAge = bo.MinIssueAge,
                MaxIssueAge = bo.MaxIssueAge,
                MaxExpiryAge = bo.MaxExpiryAge,
                RetrocessionaireDiscount = bo.RetrocessionaireDiscount,
                RateTablePercentage = bo.RateTablePercentage,
                ClaimApprovalLimit = bo.ClaimApprovalLimit,
                AutoBindingLimit = bo.AutoBindingLimit,
                IsProfitCommission = bo.IsProfitCommission,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingPerLifeRetroVersionBenefit.IsExists(id);
        }

        public static TreatyPricingPerLifeRetroVersionBenefitBo Find(int id)
        {
            return FormBo(TreatyPricingPerLifeRetroVersionBenefit.Find(id));
        }

        public static IList<TreatyPricingPerLifeRetroVersionBenefitBo> GetByTreatyPricingPerLifeRetroVersionId(int treatyPricingPerLifeRetroVersionId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingPerLifeRetroVersionBenefits
                    .Where(q => q.TreatyPricingPerLifeRetroVersionId == treatyPricingPerLifeRetroVersionId)
                    .ToList());
            }
        }

        public static IList<TreatyPricingPerLifeRetroVersionBenefitBo> GetByTreatyPricingPerLifeRetroVersionIdExcept(int treatyPricingPerLifeRetroVersionId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingPerLifeRetroVersionBenefits
                    .Where(q => q.TreatyPricingPerLifeRetroVersionId == treatyPricingPerLifeRetroVersionId)
                    .Where(q => !ids.Contains(q.Id))
                    .ToList());
            }
        }

        public static IList<TreatyPricingPerLifeRetroVersionBenefitBo> GetByVersionId(int versionId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingPerLifeRetroVersionBenefits
                    .Where(q => q.TreatyPricingPerLifeRetroVersionId == versionId);

                return FormBos(query.ToList());
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

        public static Result Save(ref TreatyPricingPerLifeRetroVersionBenefitBo bo)
        {
            if (!TreatyPricingPerLifeRetroVersionBenefit.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref TreatyPricingPerLifeRetroVersionBenefitBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingPerLifeRetroVersionBenefit.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingPerLifeRetroVersionBenefitBo bo)
        {
            TreatyPricingPerLifeRetroVersionBenefit entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingPerLifeRetroVersionBenefitBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingPerLifeRetroVersionBenefitBo bo)
        {
            Result result = Result();

            TreatyPricingPerLifeRetroVersionBenefit entity = TreatyPricingPerLifeRetroVersionBenefit.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingPerLifeRetroVersionId = bo.TreatyPricingPerLifeRetroVersionId;
                entity.BenefitId = bo.BenefitId;
                entity.ArrangementRetrocessionnaireTypePickListDetailId = bo.ArrangementRetrocessionnaireTypePickListDetailId;
                entity.TotalMortality = bo.TotalMortality;
                entity.MlreRetention = bo.MlreRetention;
                entity.RetrocessionnaireShare = bo.RetrocessionnaireShare;
                entity.AgeBasisPickListDetailId = bo.AgeBasisPickListDetailId;
                entity.MinIssueAge = bo.MinIssueAge;
                entity.MaxIssueAge = bo.MaxIssueAge;
                entity.MaxExpiryAge = bo.MaxExpiryAge;
                entity.RetrocessionaireDiscount = bo.RetrocessionaireDiscount;
                entity.RateTablePercentage = bo.RateTablePercentage;
                entity.ClaimApprovalLimit = bo.ClaimApprovalLimit;
                entity.AutoBindingLimit = bo.AutoBindingLimit;
                entity.IsProfitCommission = bo.IsProfitCommission;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingPerLifeRetroVersionBenefitBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingPerLifeRetroVersionBenefitBo bo)
        {
            TreatyPricingPerLifeRetroVersionBenefit.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingPerLifeRetroVersionBenefitBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingPerLifeRetroVersionBenefit.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }        

        public static void GetByTreatyPricingPerLifeRetroVersionIdExcept(int treatyPricingPerLifeRetroVersionId, List<int> saveIds, ref TrailObject trail)
        {
            Result result = Result();
            IList<TreatyPricingPerLifeRetroVersionBenefitBo> bos = GetByTreatyPricingPerLifeRetroVersionIdExcept(treatyPricingPerLifeRetroVersionId, saveIds);
            foreach (TreatyPricingPerLifeRetroVersionBenefitBo bo in bos)
            {
                Delete(bo, ref trail);
            }
        }

        public static void Save(string benefit, int versionId, int authUserId, ref TrailObject trail, bool resetId = false)
        {
            if (string.IsNullOrEmpty(benefit))
                return;

            List<int> ids = new List<int>();

            List<TreatyPricingPerLifeRetroVersionBenefitBo> bos = JsonConvert.DeserializeObject<List<TreatyPricingPerLifeRetroVersionBenefitBo>>(benefit);
            foreach (var benefitBo in bos)
            {
                var bo = benefitBo;
                if (resetId)
                    bo.Id = 0;

                bo.TreatyPricingPerLifeRetroVersionId = versionId;
                bo.UpdatedById = authUserId;

                if (bo.Id == 0)
                    bo.CreatedById = authUserId;

                //bo.SetSelectValues();

                Result result = Save(ref bo, ref trail);
                if (result.Valid)
                    ids.Add(bo.Id);
            }
            GetByTreatyPricingPerLifeRetroVersionIdExcept(versionId, ids, ref trail);
        }
    }
}
