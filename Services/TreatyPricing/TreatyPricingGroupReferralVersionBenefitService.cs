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
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace Services.TreatyPricing
{
    public class TreatyPricingGroupReferralVersionBenefitService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingGroupReferralVersionBenefit)),
                Controller = ModuleBo.ModuleController.TreatyPricingGroupReferralVersionBenefit.ToString()
            };
        }

        public static TreatyPricingGroupReferralVersionBenefitBo FormBoForProcessRiGroupSlip(TreatyPricingGroupReferralVersionBenefit entity = null)
        {
            if (entity == null)
                return null;

            return new TreatyPricingGroupReferralVersionBenefitBo
            {
                Id = entity.Id,
                TreatyPricingUwLimitId = entity.TreatyPricingUwLimitId,
                TreatyPricingUwLimitVersionBo = TreatyPricingUwLimitVersionService.Find(entity.TreatyPricingUwLimitId),
                BenefitId = entity.BenefitId,
                BenefitBo = BenefitService.Find(entity.BenefitId),
                BenefitCode = entity.Benefit.ToString(),
                RequestedFreeCoverLimitNonCI = entity.RequestedFreeCoverLimitNonCI,
                RequestedFreeCoverLimitCI = entity.RequestedFreeCoverLimitCI,
                GroupFreeCoverLimitAgeNonCI = entity.GroupFreeCoverLimitAgeNonCI,
                GroupFreeCoverLimitAgeCI = entity.GroupFreeCoverLimitAgeCI,
                MinimumEntryAge = entity.MinimumEntryAge,
                MaximumEntryAge = entity.MaximumEntryAge,
                MaximumExpiryAge = entity.MaximumExpiryAge,
                OtherSpecialReinsuranceArrangement = entity.OtherSpecialReinsuranceArrangement,
                CedantRetention = entity.CedantRetention,
                ReinsuranceShare = entity.ReinsuranceShare,
                AgeBasisPickListDetailBo = PickListDetailService.Find(entity.AgeBasisPickListDetailId),
                OtherSpecialTerms = entity.OtherSpecialTerms,
                OverwriteGroupProfitCommissionRemarks = entity.OverwriteGroupProfitCommissionRemarks,
                CoinsuranceCedantRetention = entity.CoinsuranceCedantRetention,
                CoinsuranceReinsurerShare = entity.CoinsuranceReinsurerShare,
                CoinsuranceRiDiscount = entity.CoinsuranceRiDiscount,
                IsOverwriteGroupProfitCommission = entity.IsOverwriteGroupProfitCommission,
                GroupFreeCoverLimitNonCI = Util.EncodeString(entity.GroupFreeCoverLimitNonCI),
                GroupFreeCoverLimitCI = Util.EncodeString(entity.GroupFreeCoverLimitCI),
                RiDiscount = entity.RiDiscount,
            };

        }

        public static TreatyPricingGroupReferralVersionBenefitBo FormBo(TreatyPricingGroupReferralVersionBenefit entity = null, bool foreign = false)
        {
            if (entity == null)
                return null;

            string autoBindingLimitSelect = null;
            if (entity.TreatyPricingUwLimitId.HasValue && entity.TreatyPricingUwLimitVersionId.HasValue)
            {
                autoBindingLimitSelect = string.Format("{0}|{1}", entity.TreatyPricingUwLimitVersionId, entity.TreatyPricingUwLimitId);
            }

            return new TreatyPricingGroupReferralVersionBenefitBo
            {
                Id = entity.Id,
                TreatyPricingGroupReferralVersionId = entity.TreatyPricingGroupReferralVersionId,
                TreatyPricingGroupReferralVersionBo = foreign ? TreatyPricingGroupReferralVersionService.Find(entity.TreatyPricingGroupReferralVersionId) : null,
                TreatyPricingGroupReferralBo = foreign ? TreatyPricingGroupReferralService.Find(TreatyPricingGroupReferralVersionService.Find(entity.TreatyPricingGroupReferralVersionId).TreatyPricingGroupReferralId) : null,
                BenefitId = entity.BenefitId,
                BenefitBo = foreign ? BenefitService.Find(entity.BenefitId) : null,
                BenefitCode = entity.Benefit.ToString(),
                ReinsuranceArrangementPickListDetailId = entity.ReinsuranceArrangementPickListDetailId,
                AgeBasisPickListDetailId = entity.AgeBasisPickListDetailId,
                AgeBasisPickListDetailBo = foreign ? PickListDetailService.Find(entity.AgeBasisPickListDetailId) : null,
                MinimumEntryAge = entity.MinimumEntryAge,
                MaximumEntryAge = entity.MaximumEntryAge,
                MaximumExpiryAge = entity.MaximumExpiryAge,
                TreatyPricingUwLimitId = entity.TreatyPricingUwLimitId,
                TreatyPricingUwLimitVersionBo = foreign ? TreatyPricingUwLimitVersionService.Find(entity.TreatyPricingUwLimitId) : null,
                TreatyPricingUwLimitVersionId = entity.TreatyPricingUwLimitVersionId,
                TreatyPricingUwLimitSelect = autoBindingLimitSelect,
                GroupFreeCoverLimitNonCI = Util.EncodeString(entity.GroupFreeCoverLimitNonCI),
                RequestedFreeCoverLimitNonCI = entity.RequestedFreeCoverLimitNonCI,
                GroupFreeCoverLimitCI = Util.EncodeString(entity.GroupFreeCoverLimitCI),
                RequestedFreeCoverLimitCI = entity.RequestedFreeCoverLimitCI,
                GroupFreeCoverLimitAgeNonCI = entity.GroupFreeCoverLimitAgeNonCI,
                GroupFreeCoverLimitAgeCI = entity.GroupFreeCoverLimitAgeCI,
                OtherSpecialReinsuranceArrangement = entity.OtherSpecialReinsuranceArrangement,
                OtherSpecialReinsurnaceArrangementStr = foreign ? TreatyPricingGroupReferralVersionBenefitBo.GetOtherSpecialReinsuranceArrangementName(entity.OtherSpecialReinsuranceArrangement) : null,
                OtherSpecialTerms = entity.OtherSpecialTerms,
                ProfitMargin = entity.ProfitMargin,
                ExpenseMargin = entity.ExpenseMargin,
                CommissionMargin = entity.CommissionMargin,
                ProfitCommissionLoading = entity.ProfitCommissionLoading,
                AdditionalLoading = entity.AdditionalLoading,
                CoinsuranceRiDiscount = entity.CoinsuranceRiDiscount,
                CoinsuranceCedantRetention = entity.CoinsuranceCedantRetention,
                CoinsuranceReinsurerShare = entity.CoinsuranceReinsurerShare,
                HasProfitCommission = entity.HasProfitCommission,
                HasGroupProfitCommission = entity.HasGroupProfitCommission,
                IsOverwriteGroupProfitCommission = entity.IsOverwriteGroupProfitCommission,
                IsOverwriteGroupProfitCommissionStr = entity.BenefitId == 0 && entity.IsOverwriteGroupProfitCommission == false ? null : entity.IsOverwriteGroupProfitCommission.ToString(),
                OverwriteGroupProfitCommissionRemarks = entity.OverwriteGroupProfitCommissionRemarks,
                GroupProfitCommission = Util.EncodeString(entity.GroupProfitCommission),
                AdditionalLoadingYRTLayer = entity.AdditionalLoadingYRTLayer,
                RiDiscount = entity.RiDiscount,
                CedantRetention = entity.CedantRetention,
                ReinsuranceShare = entity.ReinsuranceShare,
                TabarruLoading = entity.TabarruLoading,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingGroupReferralVersionBenefitBo FormBoForProductAndBenefitDetailsComparison(TreatyPricingGroupReferralVersionBenefit entity = null)
        {
            if (entity == null)
                return null;

            string autoBindingLimitSelect = null;
            if (entity.TreatyPricingUwLimitId.HasValue && entity.TreatyPricingUwLimitVersionId.HasValue)
            {
                autoBindingLimitSelect = string.Format("{0}|{1}", entity.TreatyPricingUwLimitVersionId, entity.TreatyPricingUwLimitId);
            }

            return new TreatyPricingGroupReferralVersionBenefitBo
            {
                Id = entity.Id,
                TreatyPricingGroupReferralVersionId = entity.TreatyPricingGroupReferralVersionId,
                BenefitId = entity.BenefitId,
                BenefitBo = BenefitService.Find(entity.BenefitId),
                BenefitCode = entity.Benefit.ToString(),
                ReinsuranceArrangementPickListDetailId = entity.ReinsuranceArrangementPickListDetailId,
                AgeBasisPickListDetailId = entity.AgeBasisPickListDetailId,
                MinimumEntryAge = entity.MinimumEntryAge,
                MaximumEntryAge = entity.MaximumEntryAge,
                MaximumExpiryAge = entity.MaximumExpiryAge,
                TreatyPricingUwLimitId = entity.TreatyPricingUwLimitId,
                TreatyPricingUwLimitVersionId = entity.TreatyPricingUwLimitVersionId,
                TreatyPricingUwLimitSelect = autoBindingLimitSelect,
                GroupFreeCoverLimitNonCI = Util.EncodeString(entity.GroupFreeCoverLimitNonCI),
                RequestedFreeCoverLimitNonCI = entity.RequestedFreeCoverLimitNonCI,
                GroupFreeCoverLimitCI = Util.EncodeString(entity.GroupFreeCoverLimitCI),
                RequestedFreeCoverLimitCI = entity.RequestedFreeCoverLimitCI,
                GroupFreeCoverLimitAgeNonCI = entity.GroupFreeCoverLimitAgeNonCI,
                GroupFreeCoverLimitAgeCI = entity.GroupFreeCoverLimitAgeCI,
                OtherSpecialReinsuranceArrangement = entity.OtherSpecialReinsuranceArrangement,
                OtherSpecialReinsurnaceArrangementStr = TreatyPricingGroupReferralVersionBenefitBo.GetOtherSpecialReinsuranceArrangementName(entity.OtherSpecialReinsuranceArrangement),
                OtherSpecialTerms = entity.OtherSpecialTerms,
                ProfitMargin = entity.ProfitMargin,
                ExpenseMargin = entity.ExpenseMargin,
                CommissionMargin = entity.CommissionMargin,
                ProfitCommissionLoading = entity.ProfitCommissionLoading,
                AdditionalLoading = entity.AdditionalLoading,
                CoinsuranceRiDiscount = entity.CoinsuranceRiDiscount,
                CoinsuranceCedantRetention = entity.CoinsuranceCedantRetention,
                CoinsuranceReinsurerShare = entity.CoinsuranceReinsurerShare,
                HasProfitCommission = entity.HasProfitCommission,
                HasGroupProfitCommission = entity.HasGroupProfitCommission,
                IsOverwriteGroupProfitCommission = entity.IsOverwriteGroupProfitCommission,
                IsOverwriteGroupProfitCommissionStr = entity.BenefitId == 0 && entity.IsOverwriteGroupProfitCommission == false ? null : entity.IsOverwriteGroupProfitCommission.ToString(),
                OverwriteGroupProfitCommissionRemarks = entity.OverwriteGroupProfitCommissionRemarks,
                GroupProfitCommission = Util.EncodeString(entity.GroupProfitCommission),
                AdditionalLoadingYRTLayer = entity.AdditionalLoadingYRTLayer,
                RiDiscount = entity.RiDiscount,
                CedantRetention = entity.CedantRetention,
                ReinsuranceShare = entity.ReinsuranceShare,
                TabarruLoading = entity.TabarruLoading
            };
        }

        public static IList<TreatyPricingGroupReferralVersionBenefitBo> FormBos(IList<TreatyPricingGroupReferralVersionBenefit> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingGroupReferralVersionBenefitBo> bos = new List<TreatyPricingGroupReferralVersionBenefitBo>() { };
            foreach (TreatyPricingGroupReferralVersionBenefit entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static IList<TreatyPricingGroupReferralVersionBenefitBo> FormBosForProcessRiGroupSlip(IList<TreatyPricingGroupReferralVersionBenefit> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingGroupReferralVersionBenefitBo> bos = new List<TreatyPricingGroupReferralVersionBenefitBo>() { };
            foreach (TreatyPricingGroupReferralVersionBenefit entity in entities)
            {
                bos.Add(FormBoForProcessRiGroupSlip(entity));
            }
            return bos;
        }

        public static IList<TreatyPricingGroupReferralVersionBenefitBo> FormBosForProductAndBenefitDetailsComparison(IList<TreatyPricingGroupReferralVersionBenefit> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingGroupReferralVersionBenefitBo> bos = new List<TreatyPricingGroupReferralVersionBenefitBo>() { };
            foreach (TreatyPricingGroupReferralVersionBenefit entity in entities)
            {
                bos.Add(FormBoForProductAndBenefitDetailsComparison(entity));
            }
            return bos;
        }

        public static TreatyPricingGroupReferralVersionBenefit FormEntity(TreatyPricingGroupReferralVersionBenefitBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingGroupReferralVersionBenefit
            {
                Id = bo.Id,
                TreatyPricingGroupReferralVersionId = bo.TreatyPricingGroupReferralVersionId,
                BenefitId = bo.BenefitId,
                ReinsuranceArrangementPickListDetailId = bo.ReinsuranceArrangementPickListDetailId,
                AgeBasisPickListDetailId = bo.AgeBasisPickListDetailId,
                MinimumEntryAge = bo.MinimumEntryAge,
                MaximumEntryAge = bo.MaximumEntryAge,
                MaximumExpiryAge = bo.MaximumExpiryAge,
                TreatyPricingUwLimitId = bo.TreatyPricingUwLimitId,
                TreatyPricingUwLimitVersionId = bo.TreatyPricingUwLimitVersionId,
                GroupFreeCoverLimitNonCI = Util.DecodeString(bo.GroupFreeCoverLimitNonCI),
                RequestedFreeCoverLimitNonCI = bo.RequestedFreeCoverLimitNonCI,
                GroupFreeCoverLimitCI = Util.DecodeString(bo.GroupFreeCoverLimitCI),
                RequestedFreeCoverLimitCI = bo.RequestedFreeCoverLimitCI,
                GroupFreeCoverLimitAgeNonCI = bo.GroupFreeCoverLimitAgeNonCI,
                GroupFreeCoverLimitAgeCI = bo.GroupFreeCoverLimitAgeCI,
                OtherSpecialReinsuranceArrangement = bo.OtherSpecialReinsuranceArrangement,
                OtherSpecialTerms = bo.OtherSpecialTerms,
                ProfitMargin = bo.ProfitMargin,
                ExpenseMargin = bo.ExpenseMargin,
                CommissionMargin = bo.CommissionMargin,
                ProfitCommissionLoading = bo.ProfitCommissionLoading,
                AdditionalLoading = bo.AdditionalLoading,
                CoinsuranceRiDiscount = bo.CoinsuranceRiDiscount,
                CoinsuranceCedantRetention = bo.CoinsuranceCedantRetention,
                CoinsuranceReinsurerShare = bo.CoinsuranceReinsurerShare,
                HasProfitCommission = bo.HasProfitCommission,
                HasGroupProfitCommission = bo.HasGroupProfitCommission,
                IsOverwriteGroupProfitCommission = bo.IsOverwriteGroupProfitCommission,
                OverwriteGroupProfitCommissionRemarks = bo.OverwriteGroupProfitCommissionRemarks,
                GroupProfitCommission = Util.DecodeString(bo.GroupProfitCommission),
                AdditionalLoadingYRTLayer = bo.AdditionalLoadingYRTLayer,
                RiDiscount = bo.RiDiscount,
                CedantRetention = bo.CedantRetention,
                ReinsuranceShare = bo.ReinsuranceShare,
                TabarruLoading = bo.TabarruLoading,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingGroupReferralVersionBenefit.IsExists(id);
        }

        public static TreatyPricingGroupReferralVersionBenefitBo Find(int id)
        {
            return FormBo(TreatyPricingGroupReferralVersionBenefit.Find(id));
        }

        public static TreatyPricingGroupReferralVersionBenefitBo FindForProductAndBenefitDetailsComparison(int id)
        {
            return FormBoForProductAndBenefitDetailsComparison(TreatyPricingGroupReferralVersionBenefit.Find(id));
        }

        public static IList<TreatyPricingGroupReferralVersionBenefitBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingGroupReferralVersionBenefits.ToList());
            }
        }

        public static IList<TreatyPricingGroupReferralVersionBenefitBo> GetByTreatyPricingGroupReferralVersionId(int? treatyPricingGroupReferralVersionId, bool foreign = false)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingGroupReferralVersionBenefits
                    .Where(q => q.TreatyPricingGroupReferralVersionId == treatyPricingGroupReferralVersionId)
                    .ToList(), foreign);
            }
        }

        public static IList<TreatyPricingGroupReferralVersionBenefitBo> GetByTreatyPricingGroupReferralVersionIdForProcessRiGroupSlip(int? treatyPricingGroupReferralVersionId, bool foreign = false)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBosForProcessRiGroupSlip(db.TreatyPricingGroupReferralVersionBenefits
                    .Where(q => q.TreatyPricingGroupReferralVersionId == treatyPricingGroupReferralVersionId)
                    .ToList());
            }
        }

        public static IList<TreatyPricingGroupReferralVersionBenefitBo> GetByTreatyPricingGroupReferralVersionIdForProductAndBenefitDetailsComparison(int? treatyPricingGroupReferralVersionId)
        {
            using (var db = new AppDbContext())
            {
                return FormBosForProductAndBenefitDetailsComparison(db.TreatyPricingGroupReferralVersionBenefits
                    .Where(q => q.TreatyPricingGroupReferralVersionId == treatyPricingGroupReferralVersionId)
                    .ToList());
            }
        }

        public static IList<TreatyPricingGroupReferralVersionBenefitBo> GetByTreatyPricingGroupReferralVersionIdExcept(int treatyPricingGroupReferralVersionId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingGroupReferralVersionBenefits
                    .Where(q => q.TreatyPricingGroupReferralVersionId == treatyPricingGroupReferralVersionId)
                    .Where(q => !ids.Contains(q.Id))
                    .ToList());
            }
        }

        public static TreatyPricingGroupReferralVersionBenefitBo GetByTreatyPricingGroupReferralVersionIdBenefitCode(int? treatyPricingGroupReferralVersionId, string benefitCode)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBo(db.TreatyPricingGroupReferralVersionBenefits
                    .Where(q => q.TreatyPricingGroupReferralVersionId == treatyPricingGroupReferralVersionId)
                    .Where(q => q.Benefit.Code == benefitCode)
                    .FirstOrDefault());
            }
        }

        public static TreatyPricingGroupReferralVersionBenefitBo GetByTreatyPricingGroupReferralVersionIdBenefitCodeForProcessRiGroupSlip(int? treatyPricingGroupReferralVersionId, string benefitCode)
        {
            using (var db = new AppDbContext())
            {
                return FormBoForProcessRiGroupSlip(db.TreatyPricingGroupReferralVersionBenefits
                    .Where(q => q.TreatyPricingGroupReferralVersionId == treatyPricingGroupReferralVersionId)
                    .Where(q => q.Benefit.Code == benefitCode)
                    .FirstOrDefault());
            }
        }

        public static string GetJsonByVersionId(int versionId)
        {
            using (var db = new AppDbContext())
            {
                var bos = GetByTreatyPricingGroupReferralVersionId(versionId);
                return JsonConvert.SerializeObject(bos);
            }
        }

        public static string GetBenefitCommissionMargin(int? treatyPricingGroupReferralVersionId, string benefitCode)
        {
            var bo = GetByTreatyPricingGroupReferralVersionIdBenefitCode(treatyPricingGroupReferralVersionId, benefitCode);
            if (bo != null)
                return bo.CommissionMargin;

            return "";
        }

        public static string GetBenefitExpenseMargin(int? treatyPricingGroupReferralVersionId, string benefitCode)
        {
            var bo = GetByTreatyPricingGroupReferralVersionIdBenefitCode(treatyPricingGroupReferralVersionId, benefitCode);
            if (bo != null)
                return bo.ExpenseMargin;

            return "";
        }

        public static string GetBenefitProfitMargin(int? treatyPricingGroupReferralVersionId, string benefitCode)
        {
            var bo = GetByTreatyPricingGroupReferralVersionIdBenefitCode(treatyPricingGroupReferralVersionId, benefitCode);
            if (bo != null)
                return bo.ProfitMargin;

            return "";
        }

        public static bool ExistByTreatyPricingGroupReferralVersionIdBenefitCode(int? treatyPricingGroupReferralVersionId, string benefitCode)
        {
            var bo = GetByTreatyPricingGroupReferralVersionIdBenefitCode(treatyPricingGroupReferralVersionId, benefitCode);
            return bo != null ? true : false;
        }

        public static Result Save(ref TreatyPricingGroupReferralVersionBenefitBo bo)
        {
            if (!TreatyPricingGroupReferralVersionBenefit.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref TreatyPricingGroupReferralVersionBenefitBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingGroupReferralVersionBenefit.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingGroupReferralVersionBenefitBo bo)
        {
            TreatyPricingGroupReferralVersionBenefit entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(TreatyPricingGroupReferralVersionBenefitBo bo)
        {
            TreatyPricingGroupReferralVersionBenefit entity = FormEntity(bo);
            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingGroupReferralVersionBenefitBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingGroupReferralVersionBenefitBo bo)
        {
            Result result = Result();

            TreatyPricingGroupReferralVersionBenefit entity = TreatyPricingGroupReferralVersionBenefit.Find(bo.Id);
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

        public static Result Update(TreatyPricingGroupReferralVersionBenefitBo bo)
        {
            Result result = Result();

            TreatyPricingGroupReferralVersionBenefit entity = TreatyPricingGroupReferralVersionBenefit.Find(bo.Id);
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

        public static Result Update(ref TreatyPricingGroupReferralVersionBenefitBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingGroupReferralVersionBenefitBo bo)
        {
            TreatyPricingGroupReferralVersionBenefit.Delete(bo.Id);
        }

        public static void DeleteAllByTreatyPricingGroupReferralVersionId(int id)
        {
            foreach (var ver in GetByTreatyPricingGroupReferralVersionId(id))
            {
                TreatyPricingGroupReferralVersionBenefit.Delete(ver.Id);
            }
        }

        public static List<int> GetDistinctBenefits()
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupReferralVersionBenefits.Select(q => q.BenefitId).Distinct().ToList();
            }
        }

        public static Result Delete(TreatyPricingGroupReferralVersionBenefitBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingGroupReferralVersionBenefit.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static void Create(string json, int parentId, int? reinsuranceArrangementId, int authUserId, ref TrailObject trail)
        {
            var riArrangementBo = PickListDetailService.Find(reinsuranceArrangementId);

            List<int> ids = new List<int> { };
            if (!string.IsNullOrEmpty(json))
                ids = JsonConvert.DeserializeObject<List<int>>(json);

            foreach (int id in ids)
            {
                var bo = new TreatyPricingGroupReferralVersionBenefitBo();

                var productBenefit = TreatyPricingProductBenefitService.Find(id);
                if (productBenefit == null)
                    return;

                bo.ReinsuranceArrangementPickListDetailId = reinsuranceArrangementId;
                bo.OtherSpecialReinsuranceArrangement = TreatyPricingGroupReferralVersionBenefitBo.OtherSpecialReinsuranceArrangementNormal;

                bo.BenefitId = productBenefit.BenefitId;
                bo.TreatyPricingGroupReferralVersionId = parentId;
                bo.AgeBasisPickListDetailId = productBenefit.AgeBasisPickListDetailId;
                bo.MinimumEntryAge = productBenefit.MinimumEntryAge;
                bo.MaximumEntryAge = productBenefit.MaximumEntryAge;
                bo.MaximumExpiryAge = productBenefit.MaximumExpiryAge;
                bo.TreatyPricingUwLimitId = productBenefit.TreatyPricingUwLimitId;
                bo.TreatyPricingUwLimitVersionId = productBenefit.TreatyPricingUwLimitVersionId;
                bo.CoinsuranceCedantRetention = productBenefit.CoinsuranceCedantRetention;
                bo.CoinsuranceReinsurerShare = productBenefit.CoinsuranceReinsuranceShare;
                bo.CedantRetention = productBenefit.CedantRetention;
                bo.ReinsuranceShare = productBenefit.ReinsuranceShare;
                bo.CoinsuranceRiDiscount = productBenefit.RequestedCoinsuranceRiDiscount;
                bo.TabarruLoading = productBenefit.TabarruLoading;
                bo.ProfitCommissionLoading = productBenefit.GroupProfitCommissionLoading;

                var productVersion = TreatyPricingProductVersionService.Find(productBenefit.TreatyPricingProductVersionId);
                bo.GroupFreeCoverLimitNonCI = productVersion.GroupFreeCoverLimitNonCi;
                bo.GroupFreeCoverLimitCI = productVersion.GroupFreeCoverLimitCi;
                bo.GroupFreeCoverLimitAgeNonCI = productVersion.GroupFreeCoverLimitAgeNonCi;
                bo.GroupFreeCoverLimitAgeCI = productVersion.GroupFreeCoverLimitAgeCi;
                bo.GroupProfitCommission = productVersion.GroupProfitCommission;

                if (riArrangementBo != null)
                {
                    if (riArrangementBo.Code == PickListDetailBo.RiArrangementCoinsuranceYRT || riArrangementBo.Code == PickListDetailBo.RiArrangementCoinsurance)
                    {
                        bo.ProfitMargin = "10%";
                        bo.ExpenseMargin = "10%";
                        bo.CommissionMargin = "10%";
                    }
                    else
                    {
                        bo.ProfitMargin = productBenefit.ProfitMargin;
                        bo.ExpenseMargin = productBenefit.ExpenseMargin;
                        bo.CommissionMargin = productBenefit.CommissionMargin;
                    }
                }
                else
                {
                    bo.ProfitMargin = productBenefit.ProfitMargin;
                    bo.ExpenseMargin = productBenefit.ExpenseMargin;
                    bo.CommissionMargin = productBenefit.CommissionMargin;
                }

                bo.CreatedById = authUserId;
                bo.UpdatedById = authUserId;

                Save(ref bo, ref trail);
            }
        }

        public static void Save(string json, int parentId, int authUserId, ref TrailObject trail, bool resetId = false)
        {
            List<TreatyPricingGroupReferralVersionBenefitBo> bos = new List<TreatyPricingGroupReferralVersionBenefitBo>();
            if (!string.IsNullOrEmpty(json))
                bos = JsonConvert.DeserializeObject<List<TreatyPricingGroupReferralVersionBenefitBo>>(json);

            foreach (var detailBo in bos)
            {
                var bo = detailBo;
                if (resetId)
                    bo.Id = 0;

                bo.TreatyPricingGroupReferralVersionId = parentId;
                bo.UpdatedById = authUserId;

                if (bo.Id == 0)
                    bo.CreatedById = authUserId;

                Save(ref bo, ref trail);
            }
        }

        public static void Update(string json, int authUserId, ref TrailObject trail, bool resetId = false)
        {
            List<TreatyPricingGroupReferralVersionBenefitBo> bos = new List<TreatyPricingGroupReferralVersionBenefitBo>();
            if (!string.IsNullOrEmpty(json))
                bos = JsonConvert.DeserializeObject<List<TreatyPricingGroupReferralVersionBenefitBo>>(json);

            if (!bos.IsNullOrEmpty())
            {
                var benefitIds = bos.Where(a => a.Id != 0).Select(a => a.Id).ToList();
                var benefitToBeDeletedBo = GetByTreatyPricingGroupReferralVersionId(bos[0].TreatyPricingGroupReferralVersionId).Where(a => !benefitIds.Contains(a.Id)).ToList();

                if (benefitToBeDeletedBo.Count > 0)
                {
                    foreach (var deleteBenefitBo in benefitToBeDeletedBo)
                    {
                        Delete(deleteBenefitBo);
                    }
                }

                foreach (var detailBo in bos)
                {
                    var bo = detailBo;
                    if (resetId)
                        bo.Id = 0;

                    bo.UpdatedById = authUserId;
                    bo.SetSelectValues();

                    if (bo.Id == 0)
                        bo.CreatedById = authUserId;

                    Save(ref bo, ref trail);
                }
            }
        }

        public static List<string> Validate(string benefit)
        {
            List<string> errors = new List<string>();
            PickListDetailBo riArrangementBo = null;

            if (string.IsNullOrEmpty(benefit))
            {
                errors.Add("At least 1 Benefit Code has to be added");
            }
            else
            {
                List<TreatyPricingGroupReferralVersionBenefitBo> bos = JsonConvert.DeserializeObject<List<TreatyPricingGroupReferralVersionBenefitBo>>(benefit);
                foreach (var benefitBo in bos)
                {
                    var bo = benefitBo;
                    var benefitCode = bo.BenefitCode;

                    if (bo.ReinsuranceArrangementPickListDetailId.HasValue)
                        riArrangementBo = PickListDetailService.Find(bo.ReinsuranceArrangementPickListDetailId);

                    foreach (string propertyName in TreatyPricingGroupReferralVersionBenefitBo.GetRequiredProperties())
                    {
                        if (riArrangementBo != null)
                        {
                            if (riArrangementBo.Code == PickListDetailBo.RiArrangementYRT && (propertyName == "ProfitMargin" || propertyName == "ExpenseMargin" || propertyName == "CommissionMargin"))
                                continue;
                        }

                        var value = bo.GetPropertyValue(propertyName);
                        if (value == null || string.IsNullOrEmpty(value.ToString()))
                        {
                            string displayName = bo.GetAttributeFrom<DisplayNameAttribute>(propertyName).DisplayName;
                            errors.Add(string.Format("{0} is required for Benefit {1}", displayName, benefitCode));
                        }
                    }
                }

                var benefitIds = bos.Select(a => a.BenefitId).ToList();
                var duplicateItems = benefitIds.GroupBy(x => x).SelectMany(g => g.Skip(1)).Distinct().ToList();
                if (duplicateItems.Count > 0)
                {
                    var duplicateItem = "";
                    foreach (var item in duplicateItems)
                    {
                        duplicateItem = item + ", " + duplicateItem;
                    }
                    errors.Add("Duplicate Benefits is Found: " + duplicateItem);
                }
            }

            errors = errors.Distinct().ToList();

            return errors;
        }
    }
}
