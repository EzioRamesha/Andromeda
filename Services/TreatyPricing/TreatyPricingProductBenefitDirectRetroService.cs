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
using System.Text;
using System.Threading.Tasks;

namespace Services.TreatyPricing
{
    public class TreatyPricingProductBenefitDirectRetroService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingProductBenefitDirectRetro)),
                Controller = ModuleBo.ModuleController.TreatyPricingProductBenefitDirectRetro.ToString()
            };
        }

        public static TreatyPricingProductBenefitDirectRetroBo FormBo(TreatyPricingProductBenefitDirectRetro entity = null)
        {
            if (entity == null)
                return null;

            return new TreatyPricingProductBenefitDirectRetroBo
            {
                Id = entity.Id,
                TreatyPricingProductBenefitId = entity.TreatyPricingProductBenefitId,
                RetroPartyId = entity.RetroPartyId,
                RetroPartyCode = RetroPartyService.Find(entity.RetroPartyId)?.ToString(),
                ArrangementRetrocessionTypePickListDetailId = entity.ArrangementRetrocessionTypePickListDetailId,
                ArrangementRetrocessionTypePickListDetailBo = PickListDetailService.Find(entity.ArrangementRetrocessionTypePickListDetailId),
                ArrangementRetrocessionType = entity.ArrangementRetrocessionTypePickListDetailId.HasValue ? PickListDetailService.Find(entity.ArrangementRetrocessionTypePickListDetailId).Description : "",
                MlreRetention = entity.MlreRetention,
                RetrocessionShare = entity.RetrocessionShare,
                IsRetrocessionProfitCommission = entity.IsRetrocessionProfitCommission,
                IsRetrocessionAdvantageProgram = entity.IsRetrocessionAdvantageProgram,
                RetrocessionRateTable = entity.RetrocessionRateTable,
                NewBusinessRateGuarantee = entity.NewBusinessRateGuarantee,
                RenewalBusinessRateGuarantee = entity.RenewalBusinessRateGuarantee,
                RetrocessionDiscount = entity.RetrocessionDiscount,
                AdditionalDiscount = entity.AdditionalDiscount,
                AdditionalLoading = entity.AdditionalLoading,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                #region Product & Benefit Comparison
                IsRetrocessionProfitCommissionStr = entity.IsRetrocessionProfitCommission ? "Yes" : "No",
                IsRetrocessionAdvantageProgramStr = entity.IsRetrocessionAdvantageProgram ? "Yes" : "No",
                #endregion
            };
        }

        public static IList<TreatyPricingProductBenefitDirectRetroBo> FormBos(IList<TreatyPricingProductBenefitDirectRetro> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingProductBenefitDirectRetroBo> bos = new List<TreatyPricingProductBenefitDirectRetroBo>() { };
            foreach (TreatyPricingProductBenefitDirectRetro entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingProductBenefitDirectRetro FormEntity(TreatyPricingProductBenefitDirectRetroBo bo = null, TreatyPricingProductBenefitDirectRetro entity = null)
        {
            if (bo == null)
                return null;

            return new TreatyPricingProductBenefitDirectRetro
            {
                Id = bo.Id,
                TreatyPricingProductBenefitId = bo.TreatyPricingProductBenefitId,
                RetroPartyId = bo.RetroPartyId,
                ArrangementRetrocessionTypePickListDetailId = bo.ArrangementRetrocessionTypePickListDetailId,
                MlreRetention = bo.MlreRetention,
                RetrocessionShare = bo.RetrocessionShare,
                IsRetrocessionProfitCommission = bo.IsRetrocessionProfitCommission,
                IsRetrocessionAdvantageProgram = bo.IsRetrocessionAdvantageProgram,
                RetrocessionRateTable = bo.RetrocessionRateTable,
                NewBusinessRateGuarantee = bo.NewBusinessRateGuarantee,
                RenewalBusinessRateGuarantee = bo.RenewalBusinessRateGuarantee,
                RetrocessionDiscount = bo.RetrocessionDiscount,
                AdditionalDiscount = bo.AdditionalDiscount,
                AdditionalLoading = bo.AdditionalLoading,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
                CreatedAt = entity != null ? entity.CreatedAt : DateTime.Now
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingProductBenefitDirectRetro.IsExists(id);
        }

        public static TreatyPricingProductBenefitDirectRetroBo Find(int? id)
        {
            return FormBo(TreatyPricingProductBenefitDirectRetro.Find(id));
        }

        public static IList<TreatyPricingProductBenefitDirectRetroBo> GetByBenefitId(int benefitId, List<int> exceptions = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingProductBenefitDirectRetros
                    .Where(q => q.TreatyPricingProductBenefitId == benefitId);

                if (exceptions != null)
                {
                    query = query.Where(q => !exceptions.Contains(q.Id));
                }

                return FormBos(query.ToList());
            }
        }

        public static TreatyPricingProductBenefitDirectRetroBo GetLatestByBenefitId(int benefitId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingProductBenefitDirectRetros
                    .Where(q => q.TreatyPricingProductBenefitId == benefitId)
                    .OrderByDescending(q => q.Id)
                    .FirstOrDefault();

                return FormBo(query);
            }
        }

        public static Result Save(ref TreatyPricingProductBenefitDirectRetroBo bo)
        {
            if (!IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref TreatyPricingProductBenefitDirectRetroBo bo, ref TrailObject trail)
        {
            if (!IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingProductBenefitDirectRetroBo bo)
        {
            TreatyPricingProductBenefitDirectRetro entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingProductBenefitDirectRetroBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingProductBenefitDirectRetroBo bo)
        {
            Result result = Result();

            TreatyPricingProductBenefitDirectRetro entity = TreatyPricingProductBenefitDirectRetro.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity = FormEntity(bo, entity);
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingProductBenefitDirectRetroBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingProductBenefitDirectRetroBo bo)
        {
            TreatyPricingProductBenefitDirectRetro.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingProductBenefitDirectRetroBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingProductBenefitDirectRetro.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static void DeleteByBenefitExcept(int benefitId, ref TrailObject trail, List<int> exceptions = null)
        {
            foreach (var bo in GetByBenefitId(benefitId, exceptions))
            {
                Delete(bo, ref trail);
            }
        }
    }
}
