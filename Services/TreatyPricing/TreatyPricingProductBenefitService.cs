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
using System.Text;
using System.Threading.Tasks;

namespace Services.TreatyPricing
{
    public class TreatyPricingProductBenefitService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingProductBenefit)),
                Controller = ModuleBo.ModuleController.TreatyPricingProductBenefit.ToString()
            };
        }

        public static TreatyPricingProductBenefitBo FormBo(TreatyPricingProductBenefit entity = null, bool foreign = false)
        {
            if (entity == null)
                return null;

            string uwLimitSelect = null;
            if (entity.TreatyPricingUwLimitId.HasValue && entity.TreatyPricingUwLimitVersionId.HasValue)
            {
                uwLimitSelect = string.Format("{0}|{1}", entity.TreatyPricingUwLimitVersionId, entity.TreatyPricingUwLimitId);
            }

            string claimApprovalLimitSelect = null;
            if (entity.TreatyPricingClaimApprovalLimitId.HasValue && entity.TreatyPricingClaimApprovalLimitVersionId.HasValue)
            {
                claimApprovalLimitSelect = string.Format("{0}|{1}", entity.TreatyPricingClaimApprovalLimitVersionId, entity.TreatyPricingClaimApprovalLimitId);
            }

            string rateTableSelect = null;
            if (entity.TreatyPricingRateTableId.HasValue && entity.TreatyPricingRateTableVersionId.HasValue)
            {
                rateTableSelect = string.Format("{0}|{1}", entity.TreatyPricingRateTableVersionId, entity.TreatyPricingRateTableId);
            }

            string benefitAndExclusionSelect = null;
            if (entity.TreatyPricingDefinitionAndExclusionId.HasValue && entity.TreatyPricingDefinitionAndExclusionVersionId.HasValue)
            {
                benefitAndExclusionSelect = string.Format("{0}|{1}", entity.TreatyPricingDefinitionAndExclusionVersionId, entity.TreatyPricingDefinitionAndExclusionId);
            }

            var benefitBo = BenefitService.Find(entity.BenefitId);

            var bo = new TreatyPricingProductBenefitBo
            {
                Id = entity.Id,
                Code = string.Format("{0} - {1}", benefitBo?.Code, entity.Name),
                TreatyPricingProductVersionId = entity.TreatyPricingProductVersionId,
                TreatyPricingProductVersionBo = foreign ? TreatyPricingProductVersionService.Find(entity.TreatyPricingProductVersionId) : null,
                BenefitId = entity.BenefitId,
                BenefitCode = benefitBo?.Code,
                BenefitBo = benefitBo,
                Name = entity.Name,
                BasicRiderPickListDetailId = entity.BasicRiderPickListDetailId,
                BasicRiderPickListDetailBo = PickListDetailService.Find(entity.BasicRiderPickListDetailId),
                BasicRider = entity.BasicRiderPickListDetailId.HasValue ? PickListDetailService.Find(entity.BasicRiderPickListDetailId).Description : "",
                // General Tab
                PayoutTypePickListDetailId = entity.PayoutTypePickListDetailId,
                PayoutTypePickListDetailBo = PickListDetailService.Find(entity.PayoutTypePickListDetailId),
                PayoutType = entity.PayoutTypePickListDetailId.HasValue ? PickListDetailService.Find(entity.PayoutTypePickListDetailId).Description : "",
                RiderAttachmentRatio = entity.RiderAttachmentRatio,
                AgeBasisPickListDetailId = entity.AgeBasisPickListDetailId,
                AgeBasisPickListDetailBo = PickListDetailService.Find(entity.AgeBasisPickListDetailId),
                AgeBasis = entity.AgeBasisPickListDetailId.HasValue ? PickListDetailService.Find(entity.AgeBasisPickListDetailId).Description : "",
                MinimumEntryAge = entity.MinimumEntryAge,
                MaximumEntryAge = entity.MaximumEntryAge,
                MaximumExpiryAge = entity.MaximumExpiryAge,
                MinimumPolicyTerm = entity.MinimumPolicyTerm,
                MaximumPolicyTerm = entity.MaximumPolicyTerm,
                PremiumPayingTerm = entity.PremiumPayingTerm,
                MinimumSumAssured = entity.MinimumSumAssured,
                MaximumSumAssured = entity.MaximumSumAssured,
                TreatyPricingUwLimitId = entity.TreatyPricingUwLimitId,
                TreatyPricingUwLimitBo = entity.TreatyPricingUwLimitId.HasValue ? TreatyPricingUwLimitService.Find(entity.TreatyPricingUwLimitId) : null,
                TreatyPricingUwLimitVersionId = entity.TreatyPricingUwLimitVersionId,
                TreatyPricingUwLimitVersionBo = entity.TreatyPricingUwLimitVersionId.HasValue ? TreatyPricingUwLimitVersionService.Find(entity.TreatyPricingUwLimitVersionId) : null,
                TreatyPricingUwLimitSelect = uwLimitSelect,
                Others = entity.Others,
                TreatyPricingClaimApprovalLimitId = entity.TreatyPricingClaimApprovalLimitId,
                TreatyPricingClaimApprovalLimitBo = entity.TreatyPricingClaimApprovalLimitId.HasValue ? TreatyPricingClaimApprovalLimitService.Find(entity.TreatyPricingClaimApprovalLimitId) : null,
                TreatyPricingClaimApprovalLimitVersionId = entity.TreatyPricingClaimApprovalLimitVersionId,
                TreatyPricingClaimApprovalLimitVersionBo = entity.TreatyPricingClaimApprovalLimitVersionId.HasValue ? TreatyPricingClaimApprovalLimitVersionService.Find(entity.TreatyPricingClaimApprovalLimitVersionId) : null,
                TreatyPricingClaimApprovalLimitSelect = claimApprovalLimitSelect,
                IfOthers1 = entity.IfOthers1,
                TreatyPricingDefinitionAndExclusionId = entity.TreatyPricingDefinitionAndExclusionId,
                TreatyPricingDefinitionAndExclusionBo = entity.TreatyPricingDefinitionAndExclusionId.HasValue ? TreatyPricingDefinitionAndExclusionService.Find(entity.TreatyPricingDefinitionAndExclusionId) : null,
                TreatyPricingDefinitionAndExclusionVersionId = entity.TreatyPricingDefinitionAndExclusionVersionId,
                TreatyPricingDefinitionAndExclusionVersionBo = entity.TreatyPricingDefinitionAndExclusionVersionId.HasValue ? TreatyPricingDefinitionAndExclusionVersionService.Find(entity.TreatyPricingDefinitionAndExclusionVersionId.Value) : null,
                TreatyPricingDefinitionAndExclusionSelect = benefitAndExclusionSelect,
                IfOthers2 = entity.IfOthers2,
                RefundOfPremium = entity.RefundOfPremium,
                PreExistingCondition = entity.PreExistingCondition,
                // Pricing Tab
                PricingArrangementReinsuranceTypePickListDetailId = entity.PricingArrangementReinsuranceTypePickListDetailId,
                PricingArrangementReinsuranceType = entity.PricingArrangementReinsuranceTypePickListDetailId.HasValue ? PickListDetailService.Find(entity.PricingArrangementReinsuranceTypePickListDetailId).Description : "",
                BenefitPayout = entity.BenefitPayout,
                CedantRetention = entity.CedantRetention,
                ReinsuranceShare = entity.ReinsuranceShare,
                CoinsuranceCedantRetention = entity.CoinsuranceCedantRetention,
                CoinsuranceReinsuranceShare = entity.CoinsuranceReinsuranceShare,
                RequestedCoinsuranceRiDiscount = entity.RequestedCoinsuranceRiDiscount,
                ProfitMargin = entity.ProfitMargin,
                ExpenseMargin = entity.ExpenseMargin,
                CommissionMargin = entity.CommissionMargin,
                GroupProfitCommissionLoading = entity.GroupProfitCommissionLoading,
                TabarruLoading = entity.TabarruLoading,
                RiskPatternSumPickListDetailId = entity.RiskPatternSumPickListDetailId,
                RiskPatternSumPickListDetailBo = PickListDetailService.Find(entity.RiskPatternSumPickListDetailId),
                RiskPatternSum = entity.RiskPatternSumPickListDetailId.HasValue ? PickListDetailService.Find(entity.RiskPatternSumPickListDetailId).Description : "",
                PricingUploadFileName = entity.PricingUploadFileName,
                PricingUploadHashFileName = entity.PricingUploadHashFileName,
                IsProfitCommission = entity.IsProfitCommission,
                IsAdvantageProgram = entity.IsAdvantageProgram,
                TreatyPricingRateTableId = entity.TreatyPricingRateTableId,
                TreatyPricingRateTableBo = entity.TreatyPricingRateTableId.HasValue ? TreatyPricingRateTableService.Find(entity.TreatyPricingRateTableId) : null,
                TreatyPricingRateTableVersionId = entity.TreatyPricingRateTableVersionId,
                TreatyPricingRateTableVersionBo = entity.TreatyPricingRateTableVersionId.HasValue ? TreatyPricingRateTableVersionService.Find(entity.TreatyPricingRateTableVersionId) : null,
                TreatyPricingRateTableSelect = rateTableSelect,
                RequestedRateGuarantee = entity.RequestedRateGuarantee,
                RequestedReinsuranceDiscount = entity.RequestedReinsuranceDiscount,
                // Direct Retro Tab
                TreatyPricingProductBenefitDirectRetroBos = TreatyPricingProductBenefitDirectRetroService.GetByBenefitId(entity.Id),
                // Inward Retro Tab
                InwardRetroParty = entity.InwardRetroParty,
                InwardRetroArrangementReinsuranceTypePickListDetailId = entity.InwardRetroArrangementReinsuranceTypePickListDetailId,
                InwardRetroArrangementReinsuranceTypePickListDetailBo = PickListDetailService.Find(entity.InwardRetroArrangementReinsuranceTypePickListDetailId),
                InwardRetroArrangementReinsuranceType = entity.InwardRetroArrangementReinsuranceTypePickListDetailId.HasValue ? PickListDetailService.Find(entity.InwardRetroArrangementReinsuranceTypePickListDetailId).Description : "",
                InwardRetroRetention = entity.InwardRetroRetention,
                MlreShare = entity.MlreShare,
                IsRetrocessionProfitCommission = entity.IsRetrocessionProfitCommission,
                IsRetrocessionAdvantageProgram = entity.IsRetrocessionAdvantageProgram,
                RetrocessionRateTable = entity.RetrocessionRateTable,
                NewBusinessRateGuarantee = entity.NewBusinessRateGuarantee,
                RenewalBusinessRateGuarantee = entity.RenewalBusinessRateGuarantee,
                RetrocessionDiscount = entity.RetrocessionDiscount,
                AdditionalDiscount = entity.AdditionalDiscount,
                AdditionalLoading = entity.AdditionalLoading,
                // Retakaful Service Tab
                WakalahFee = entity.WakalahFee,
                MlreServiceFee = entity.MlreServiceFee,
                // Others
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                #region Product & Benefit Comparison
                IsProfitCommissionStr = entity.IsProfitCommission ? "Yes" : "No",
                IsAdvantageProgramStr = entity.IsAdvantageProgram ? "Yes" : "No",
                IsRetrocessionProfitCommissionStr = entity.IsRetrocessionProfitCommission ? "Yes" : "No",
                IsRetrocessionAdvantageProgramStr = entity.IsRetrocessionAdvantageProgram ? "Yes" : "No",
                #endregion
            };

            bo.CanDownloadFile = System.IO.File.Exists(bo.GetLocalPath());

            return bo;
        }

        public static TreatyPricingProductBenefitBo FormBoForDropDownProductBenefit(TreatyPricingProductBenefit entity = null, bool foreign = false)
        {
            if (entity == null)
                return null;

            var benefitBo = BenefitService.FindForDropDownProductBenefit(entity.BenefitId);

            var bo = new TreatyPricingProductBenefitBo
            {
                Id = entity.Id,
                BenefitBo = benefitBo
            };

            return bo;
        }

        public static IList<TreatyPricingProductBenefitBo> FormBos(IList<TreatyPricingProductBenefit> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingProductBenefitBo> bos = new List<TreatyPricingProductBenefitBo>() { };
            foreach (TreatyPricingProductBenefit entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static IList<TreatyPricingProductBenefitBo> FormBosForDropDownProductBenefit(IList<TreatyPricingProductBenefit> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingProductBenefitBo> bos = new List<TreatyPricingProductBenefitBo>() { };
            foreach (TreatyPricingProductBenefit entity in entities)
            {
                bos.Add(FormBoForDropDownProductBenefit(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingProductBenefit FormEntity(TreatyPricingProductBenefitBo bo = null, TreatyPricingProductBenefit entity = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingProductBenefit
            {
                Id = bo.Id,
                TreatyPricingProductVersionId = bo.TreatyPricingProductVersionId,
                BenefitId = bo.BenefitId,
                Name = bo.Name,
                BasicRiderPickListDetailId = bo.BasicRiderPickListDetailId,
                // General Tab
                PayoutTypePickListDetailId = bo.PayoutTypePickListDetailId,
                RiderAttachmentRatio = bo.RiderAttachmentRatio,
                AgeBasisPickListDetailId = bo.AgeBasisPickListDetailId,
                MinimumEntryAge = bo.MinimumEntryAge,
                MaximumEntryAge = bo.MaximumEntryAge,
                MaximumExpiryAge = bo.MaximumExpiryAge,
                MinimumPolicyTerm = bo.MinimumPolicyTerm,
                MaximumPolicyTerm = bo.MaximumPolicyTerm,
                PremiumPayingTerm = bo.PremiumPayingTerm,
                MinimumSumAssured = bo.MinimumSumAssured,
                MaximumSumAssured = bo.MaximumSumAssured,
                TreatyPricingUwLimitId = bo.TreatyPricingUwLimitId,
                TreatyPricingUwLimitVersionId = bo.TreatyPricingUwLimitVersionId,
                Others = bo.Others,
                TreatyPricingClaimApprovalLimitId = bo.TreatyPricingClaimApprovalLimitId,
                TreatyPricingClaimApprovalLimitVersionId = bo.TreatyPricingClaimApprovalLimitVersionId,
                IfOthers1 = bo.IfOthers1,
                TreatyPricingDefinitionAndExclusionId = bo.TreatyPricingDefinitionAndExclusionId,
                TreatyPricingDefinitionAndExclusionVersionId = bo.TreatyPricingDefinitionAndExclusionVersionId,
                IfOthers2 = bo.IfOthers2,
                RefundOfPremium = bo.RefundOfPremium,
                PreExistingCondition = bo.PreExistingCondition,
                // Pricing Tab
                PricingArrangementReinsuranceTypePickListDetailId = bo.PricingArrangementReinsuranceTypePickListDetailId,
                BenefitPayout = bo.BenefitPayout,
                CedantRetention = bo.CedantRetention,
                ReinsuranceShare = bo.ReinsuranceShare,
                CoinsuranceCedantRetention = bo.CoinsuranceCedantRetention,
                CoinsuranceReinsuranceShare = bo.CoinsuranceReinsuranceShare,
                RequestedCoinsuranceRiDiscount = bo.RequestedCoinsuranceRiDiscount,
                ProfitMargin = bo.ProfitMargin,
                ExpenseMargin = bo.ExpenseMargin,
                CommissionMargin = bo.CommissionMargin,
                GroupProfitCommissionLoading = bo.GroupProfitCommissionLoading,
                TabarruLoading = bo.TabarruLoading,
                RiskPatternSumPickListDetailId = bo.RiskPatternSumPickListDetailId,
                PricingUploadFileName = bo.PricingUploadFileName,
                PricingUploadHashFileName = bo.PricingUploadHashFileName,
                IsProfitCommission = bo.IsProfitCommission,
                IsAdvantageProgram = bo.IsAdvantageProgram,
                TreatyPricingRateTableId = bo.TreatyPricingRateTableId,
                TreatyPricingRateTableVersionId = bo.TreatyPricingRateTableVersionId,
                RequestedRateGuarantee = bo.RequestedRateGuarantee,
                RequestedReinsuranceDiscount = bo.RequestedReinsuranceDiscount,
                // Direct Retro Tab
                // Inward Retro Tab
                InwardRetroParty = bo.InwardRetroParty,
                InwardRetroArrangementReinsuranceTypePickListDetailId = bo.InwardRetroArrangementReinsuranceTypePickListDetailId,
                InwardRetroRetention = bo.InwardRetroRetention,
                MlreShare = bo.MlreShare,
                IsRetrocessionProfitCommission = bo.IsRetrocessionProfitCommission,
                IsRetrocessionAdvantageProgram = bo.IsRetrocessionAdvantageProgram,
                RetrocessionRateTable = bo.RetrocessionRateTable,
                NewBusinessRateGuarantee = bo.NewBusinessRateGuarantee,
                RenewalBusinessRateGuarantee = bo.RenewalBusinessRateGuarantee,
                RetrocessionDiscount = bo.RetrocessionDiscount,
                AdditionalDiscount = bo.AdditionalDiscount,
                AdditionalLoading = bo.AdditionalLoading,
                // Retakaful Service Tab
                WakalahFee = bo.WakalahFee,
                MlreServiceFee = bo.MlreServiceFee,
                // Others
                CreatedAt = entity != null ? entity.CreatedAt : DateTime.Now,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingProductBenefit.IsExists(id);
        }

        public static TreatyPricingProductBenefitBo Find(int? id)
        {
            return FormBo(TreatyPricingProductBenefit.Find(id));
        }

        public static IList<TreatyPricingProductBenefitBo> GetByVersionId(int versionId, List<int> exceptions = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingProductBenefits
                    .Where(q => q.TreatyPricingProductVersionId == versionId);

                if (exceptions != null)
                {
                    query = query.Where(q => !exceptions.Contains(q.Id));
                }

                return FormBos(query.ToList());
            }
        }

        public static IList<TreatyPricingProductBenefitBo> GetByVersionIdBenefits(int versionId, List<int> benefitIds)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingProductBenefits
                    .Where(q => q.TreatyPricingProductVersionId == versionId && benefitIds.Contains(q.BenefitId));

                return FormBos(query.ToList());
            }
        }

        public static List<int> GetDistinctProductVersionIdByRateTableVersionId(int rateTableVersionId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductBenefits
                    .Where(q => q.TreatyPricingRateTableVersionId == rateTableVersionId)
                    .GroupBy(q => q.TreatyPricingProductVersionId)
                    .Select(r => r.FirstOrDefault())
                    .Select(q => q.TreatyPricingProductVersionId)
                    .ToList();
            }
        }

        public static string GetLinkedProductsByUwLimitVersionId(int versionId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingProductBenefits
                    .Where(q => q.TreatyPricingUwLimitVersionId == versionId)
                    .Select(q => q.TreatyPricingProductVersionId);

                string linkedProducts = "";
                List<string> linkedProductsList = new List<string>();
                foreach (int productVersionId in query.ToList())
                {
                    var productVersionBo = TreatyPricingProductVersionService.Find(productVersionId);
                    var productBo = TreatyPricingProductService.Find(productVersionBo.TreatyPricingProductId);

                    linkedProductsList.Add(productBo.Code + " - " + productBo.Name);
                }

                linkedProducts = string.Join(",", linkedProductsList.Where(c => !string.IsNullOrWhiteSpace(c)).Distinct());

                return linkedProducts;
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

        // RateTableId
        public static List<int> GetRateTableVersionIdByProductVersionIds(List<int> productVersionIds)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductBenefits
                    .Where(q => productVersionIds.Contains(q.TreatyPricingProductVersionId))
                    .Where(q => q.TreatyPricingRateTableVersionId.HasValue)
                    .GroupBy(q => q.TreatyPricingRateTableVersionId)
                    .Select(r => r.FirstOrDefault())
                    .OrderBy(q => q.TreatyPricingRateTableVersionId)
                    .Select(q => q.TreatyPricingRateTableVersionId.Value)
                    .ToList();
            }
        }

        public static List<int> GetRateTableVersionIdByProductIds(List<int> productIds)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductBenefits
                    .Where(q => productIds.Contains(q.TreatyPricingProductVersion.TreatyPricingProductId))
                    .Where(q => q.TreatyPricingRateTableVersionId.HasValue)
                    .GroupBy(q => q.TreatyPricingRateTableVersionId)
                    .Select(r => r.FirstOrDefault())
                    .OrderBy(q => q.TreatyPricingRateTableVersionId)
                    .Select(q => q.TreatyPricingRateTableVersionId.Value)
                    .ToList();
            }
        }

        public static IList<TreatyPricingProductBenefitBo> GetByProductVersionIdsRateTableVersionId(List<int> productVersionIds, int rateVersionIds)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingProductBenefits
                    .Where(q => productVersionIds.Contains(q.TreatyPricingProductVersionId))
                    .Where(q => q.TreatyPricingRateTableVersionId.HasValue)
                    .Where(q => q.TreatyPricingRateTableVersionId == rateVersionIds)
                    .ToList());
            }
        }

        public static IList<TreatyPricingProductBenefitBo> GetByProductVersionIdTreatyPricingProductId(int productVersionId, int productId, bool foreign = false)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingProductBenefits
                    .Where(q => q.TreatyPricingProductVersionId == productVersionId)
                    .Where(q => q.TreatyPricingProductVersion.TreatyPricingProductId == productId)
                    .ToList(), foreign);
            }
        }

        public static IList<TreatyPricingProductBenefitBo> GetByProductVersionIdTreatyPricingProductIdForDropDownProductBenefit(int productVersionId, int productId, bool foreign = false)
        {
            using (var db = new AppDbContext())
            {
                return FormBosForDropDownProductBenefit(db.TreatyPricingProductBenefits
                    .Where(q => q.TreatyPricingProductVersionId == productVersionId)
                    .Where(q => q.TreatyPricingProductVersion.TreatyPricingProductId == productId)
                    .ToList(), foreign);
            }
        }

        public static Result Save(ref TreatyPricingProductBenefitBo bo)
        {
            if (!IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref TreatyPricingProductBenefitBo bo, ref TrailObject trail)
        {
            if (!IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingProductBenefitBo bo)
        {
            TreatyPricingProductBenefit entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingProductBenefitBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingProductBenefitBo bo)
        {
            Result result = Result();

            TreatyPricingProductBenefit entity = TreatyPricingProductBenefit.Find(bo.Id);
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

        public static Result Update(ref TreatyPricingProductBenefitBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingProductBenefitBo bo)
        {
            TreatyPricingProductBenefit.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingProductBenefitBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingProductBenefit.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static void DeleteByVersionExcept(int versionId, List<int> exceptions, ref TrailObject trail)
        {
            foreach (var bo in GetByVersionId(versionId, exceptions))
            {
                TreatyPricingProductBenefitDirectRetroService.DeleteByBenefitExcept(bo.Id, ref trail);
                Delete(bo, ref trail);
            }
        }
    }
}
