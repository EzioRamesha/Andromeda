using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.TreatyPricing
{
    public class TreatyPricingCampaignVersionService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingCampaignVersion)),
                Controller = ModuleBo.ModuleController.TreatyPricingCampaignVersion.ToString()
            };
        }

        public static TreatyPricingCampaignVersionBo FormBo(TreatyPricingCampaignVersion entity = null, bool foreign = false)
        {
            if (entity == null)
                return null;

            string medicalTableSelect = null;
            string medicalTableSelectStr = null;
            if (entity.TreatyPricingMedicalTableId.HasValue && entity.TreatyPricingMedicalTableVersionId.HasValue)
            {
                medicalTableSelect = string.Format("{0}|{1}", entity.TreatyPricingMedicalTableVersionId, entity.TreatyPricingMedicalTableId);

                if (foreign)
                {
                    var medicalTable = TreatyPricingMedicalTableVersionService.Find(entity.TreatyPricingMedicalTableVersionId.Value, true);
                    medicalTableSelectStr = string.Format("{0} - {1} v{2}.0", medicalTable.TreatyPricingMedicalTableBo.MedicalTableId, medicalTable.TreatyPricingMedicalTableBo.Name, medicalTable.Version);
                }

            }

            string financialTableSelect = null;
            string financialTableSelectStr = null;
            if (entity.TreatyPricingFinancialTableId.HasValue && entity.TreatyPricingFinancialTableVersionId.HasValue)
            {
                financialTableSelect = string.Format("{0}|{1}", entity.TreatyPricingFinancialTableVersionId, entity.TreatyPricingFinancialTableId);

                if (foreign)
                {
                    var financialTable = TreatyPricingFinancialTableVersionService.Find(entity.TreatyPricingFinancialTableVersionId.Value, true);
                    financialTableSelectStr = string.Format("{0} - {1} v{2}.0", financialTable.TreatyPricingFinancialTableBo.FinancialTableId, financialTable.TreatyPricingFinancialTableBo.Name, financialTable.Version);
                }

            }

            string uwQuestionnaireSelect = null;
            string uwQuestionnaireSelectStr = null;
            if (entity.TreatyPricingUwQuestionnaireId.HasValue && entity.TreatyPricingUwQuestionnaireVersionId.HasValue)
            {
                uwQuestionnaireSelect = string.Format("{0}|{1}", entity.TreatyPricingUwQuestionnaireVersionId, entity.TreatyPricingUwQuestionnaireId);

                if (foreign)
                {
                    var uwQuestionnaire = TreatyPricingUwQuestionnaireVersionService.Find(entity.TreatyPricingUwQuestionnaireVersionId.Value, true);
                    uwQuestionnaireSelectStr = string.Format("{0} - {1} v{2}.0", uwQuestionnaire.TreatyPricingUwQuestionnaireBo.Code, uwQuestionnaire.TreatyPricingUwQuestionnaireBo.Name, uwQuestionnaire.Version);
                }
            }

            string advantageProgramSelect = null;
            string advantageProgramSelectStr = null;
            if (entity.TreatyPricingAdvantageProgramId.HasValue && entity.TreatyPricingAdvantageProgramVersionId.HasValue)
            {
                advantageProgramSelect = string.Format("{0}|{1}", entity.TreatyPricingAdvantageProgramVersionId, entity.TreatyPricingAdvantageProgramId);

                if (foreign)
                {
                    var advantageProgram = TreatyPricingAdvantageProgramVersionService.Find(entity.TreatyPricingAdvantageProgramVersionId.Value, true);
                    advantageProgramSelectStr = string.Format("{0} - {1} v{2}.0", advantageProgram.TreatyPricingAdvantageProgramBo.Code, advantageProgram.TreatyPricingAdvantageProgramBo.Name, advantageProgram.Version);
                }
            }

            string profitCommissionSelect = null;
            string profitCommissionSelectStr = null;
            if (entity.TreatyPricingProfitCommissionId.HasValue && entity.TreatyPricingProfitCommissionVersionId.HasValue)
            {
                profitCommissionSelect = string.Format("{0}|{1}", entity.TreatyPricingProfitCommissionVersionId, entity.TreatyPricingProfitCommissionId);

                if (foreign)
                {
                    var profitCommission = TreatyPricingProfitCommissionVersionService.Find(entity.TreatyPricingProfitCommissionVersionId.Value, true);
                    profitCommissionSelectStr = string.Format("{0} - {1} v{2}.0", profitCommission.TreatyPricingProfitCommissionBo.Code, profitCommission.TreatyPricingProfitCommissionBo.Name, profitCommission.Version);
                }
            }

            string reinsRateSelect = null;
            string reinsRateSelectStr = null;
            if (entity.ReinsRateTreatyPricingRateTableId.HasValue && entity.ReinsRateTreatyPricingRateTableVersionId.HasValue)
            {
                reinsRateSelect = string.Format("{0}|{1}", entity.ReinsRateTreatyPricingRateTableVersionId, entity.ReinsRateTreatyPricingRateTableId);

                if (foreign)
                {
                    var reinsRate = TreatyPricingRateTableVersionService.Find(entity.ReinsRateTreatyPricingRateTableVersionId.Value, true);
                    reinsRateSelectStr = string.Format("{0} - {1} v{2}.0", reinsRate.TreatyPricingRateTableBo.Code, reinsRate.TreatyPricingRateTableBo.Name, reinsRate.Version);
                }
            }

            string reinsDiscountSelect = null;
            string reinsDiscountSelectStr = null;
            if (entity.ReinsDiscountTreatyPricingRateTableId.HasValue && entity.ReinsDiscountTreatyPricingRateTableVersionId.HasValue)
            {
                reinsDiscountSelect = string.Format("{0}|{1}", entity.ReinsDiscountTreatyPricingRateTableVersionId, entity.ReinsDiscountTreatyPricingRateTableId);

                if (foreign)
                {
                    var reinsDiscount = TreatyPricingRateTableVersionService.Find(entity.ReinsDiscountTreatyPricingRateTableVersionId.Value, true);
                    reinsDiscountSelectStr = string.Format("{0} - {1} v{2}.0", reinsDiscount.TreatyPricingRateTableBo.Code, reinsDiscount.TreatyPricingRateTableBo.Name, reinsDiscount.Version);
                }
            }

            return new TreatyPricingCampaignVersionBo
            {
                Id = entity.Id,
                TreatyPricingCampaignId = entity.TreatyPricingCampaignId,
                //TreatyPricingCampaignBo = TreatyPricingCampaignService.Find(entity.TreatyPricingCampaignId),
                Version = entity.Version,
                PersonInChargeId = entity.PersonInChargeId,
                PersonInChargeBo = UserService.Find(entity.PersonInChargeId),
                IsPerBenefit = entity.IsPerBenefit,
                BenefitRemark = entity.BenefitRemark,
                IsPerCedantRetention = entity.IsPerCedantRetention,
                CedantRetention = entity.CedantRetention,
                IsPerMlreShare = entity.IsPerMlreShare,
                MlreShare = entity.MlreShare,
                IsPerDistributionChannel = entity.IsPerDistributionChannel,
                DistributionChannel = entity.DistributionChannel,
                IsPerAgeBasis = entity.IsPerAgeBasis,
                AgeBasisPickListDetailId = entity.AgeBasisPickListDetailId,
                IsPerMinEntryAge = entity.IsPerMinEntryAge,
                MinEntryAge = entity.MinEntryAge,
                IsPerMaxEntryAge = entity.IsPerMaxEntryAge,
                MaxEntryAge = entity.MaxEntryAge,
                IsPerMaxExpiryAge = entity.IsPerMaxExpiryAge,
                MaxExpiryAge = entity.MaxExpiryAge,
                IsPerMinSumAssured = entity.IsPerMinSumAssured,
                MinSumAssured = entity.MinSumAssured,
                IsPerMaxSumAssured = entity.IsPerMaxSumAssured,
                MaxSumAssured = entity.MaxSumAssured,
                IsPerReinsuranceRate = entity.IsPerReinsuranceRate,
                ReinsRateTreatyPricingRateTableId = entity.ReinsRateTreatyPricingRateTableId,
                ReinsRateTreatyPricingRateTableVersionId = entity.ReinsRateTreatyPricingRateTableVersionId,
                ReinsRateTreatyPricingRateTableSelect = reinsRateSelect,
                ReinsRateTreatyPricingRateTableSelectStr = reinsRateSelectStr,
                ReinsRateNote = entity.ReinsRateNote,
                IsPerReinsuranceDiscount = entity.IsPerReinsuranceDiscount,
                ReinsDiscountTreatyPricingRateTableId = entity.ReinsDiscountTreatyPricingRateTableId,
                ReinsDiscountTreatyPricingRateTableVersionId = entity.ReinsDiscountTreatyPricingRateTableVersionId,
                ReinsDiscountTreatyPricingRateTableSelect = reinsDiscountSelect,
                ReinsDiscountTreatyPricingRateTableSelectStr = reinsDiscountSelectStr,
                ReinsDiscountNote = entity.ReinsDiscountNote,
                IsPerProfitComm = entity.IsPerProfitComm,
                TreatyPricingProfitCommissionId = entity.TreatyPricingProfitCommissionId,
                TreatyPricingProfitCommissionVersionId = entity.TreatyPricingProfitCommissionVersionId,
                TreatyPricingProfitCommissionSelect = profitCommissionSelect,
                TreatyPricingProfitCommissionSelectStr = profitCommissionSelectStr,
                ProfitCommNote = entity.ProfitCommNote,
                CampaignFundByMlre = entity.CampaignFundByMlre,
                ComplimentarySumAssured = entity.ComplimentarySumAssured,
                IsPerUnderwritingMethod = entity.IsPerUnderwritingMethod,
                UnderwritingMethod = entity.UnderwritingMethod,
                IsPerUnderwritingQuestion = entity.IsPerUnderwritingQuestion,
                TreatyPricingUwQuestionnaireId = entity.TreatyPricingUwQuestionnaireId,
                TreatyPricingUwQuestionnaireVersionId = entity.TreatyPricingUwQuestionnaireVersionId,
                TreatyPricingUwQuestionnaireSelect = uwQuestionnaireSelect,
                TreatyPricingUwQuestionnaireSelectStr = uwQuestionnaireSelectStr,
                UnderwritingQuestionNote = entity.UnderwritingQuestionNote,
                IsPerMedicalTable = entity.IsPerMedicalTable,
                TreatyPricingMedicalTableId = entity.TreatyPricingMedicalTableId,
                TreatyPricingMedicalTableVersionId = entity.TreatyPricingMedicalTableVersionId,
                TreatyPricingMedicalTableSelect = medicalTableSelect,
                TreatyPricingMedicalTableSelectStr = medicalTableSelectStr,
                MedicalTableNote = entity.MedicalTableNote,
                IsPerFinancialTable = entity.IsPerFinancialTable,
                TreatyPricingFinancialTableId = entity.TreatyPricingFinancialTableId,
                TreatyPricingFinancialTableVersionId = entity.TreatyPricingFinancialTableVersionId,
                TreatyPricingFinancialTableSelect = financialTableSelect,
                TreatyPricingFinancialTableSelectStr = financialTableSelectStr,
                FinancialTableNote = entity.FinancialTableNote,
                IsPerAggregationNotes = entity.IsPerAggregationNotes,
                AggregationNotes = entity.AggregationNotes,
                IsPerAdvantageProgram = entity.IsPerAdvantageProgram,
                TreatyPricingAdvantageProgramId = entity.TreatyPricingAdvantageProgramId,
                TreatyPricingAdvantageProgramVersionId = entity.TreatyPricingAdvantageProgramVersionId,
                TreatyPricingAdvantageProgramSelect = advantageProgramSelect,
                TreatyPricingAdvantageProgramSelectStr = advantageProgramSelectStr,
                AdvantageProgramNote = entity.AdvantageProgramNote,
                IsPerWaitingPeriod = entity.IsPerWaitingPeriod,
                WaitingPeriod = entity.WaitingPeriod,
                IsPerSurvivalPeriod = entity.IsPerSurvivalPeriod,
                SurvivalPeriod = entity.SurvivalPeriod,
                OtherCampaignCriteria = entity.OtherCampaignCriteria,
                AdditionalRemark = entity.AdditionalRemark,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<TreatyPricingCampaignVersionBo> FormBos(IList<TreatyPricingCampaignVersion> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingCampaignVersionBo> bos = new List<TreatyPricingCampaignVersionBo>() { };
            foreach (TreatyPricingCampaignVersion entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingCampaignVersion FormEntity(TreatyPricingCampaignVersionBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingCampaignVersion
            {
                Id = bo.Id,
                TreatyPricingCampaignId = bo.TreatyPricingCampaignId,
                Version = bo.Version,
                PersonInChargeId = bo.PersonInChargeId,
                IsPerBenefit = bo.IsPerBenefit,
                BenefitRemark = bo.BenefitRemark,
                IsPerCedantRetention = bo.IsPerCedantRetention,
                CedantRetention = bo.CedantRetention,
                IsPerMlreShare = bo.IsPerMlreShare,
                MlreShare = bo.MlreShare,
                IsPerDistributionChannel = bo.IsPerDistributionChannel,
                DistributionChannel = bo.DistributionChannel,
                IsPerAgeBasis = bo.IsPerAgeBasis,
                AgeBasisPickListDetailId = bo.AgeBasisPickListDetailId,
                IsPerMinEntryAge = bo.IsPerMinEntryAge,
                MinEntryAge = bo.MinEntryAge,
                IsPerMaxEntryAge = bo.IsPerMaxEntryAge,
                MaxEntryAge = bo.MaxEntryAge,
                IsPerMaxExpiryAge = bo.IsPerMaxExpiryAge,
                MaxExpiryAge = bo.MaxExpiryAge,
                IsPerMinSumAssured = bo.IsPerMinSumAssured,
                MinSumAssured = bo.MinSumAssured,
                IsPerMaxSumAssured = bo.IsPerMaxSumAssured,
                MaxSumAssured = bo.MaxSumAssured,
                IsPerReinsuranceRate = bo.IsPerReinsuranceRate,
                ReinsRateTreatyPricingRateTableId = bo.ReinsRateTreatyPricingRateTableId,
                ReinsRateTreatyPricingRateTableVersionId = bo.ReinsRateTreatyPricingRateTableVersionId,
                ReinsRateNote = bo.ReinsRateNote,
                IsPerReinsuranceDiscount = bo.IsPerReinsuranceDiscount,
                ReinsDiscountTreatyPricingRateTableId = bo.ReinsDiscountTreatyPricingRateTableId,
                ReinsDiscountTreatyPricingRateTableVersionId = bo.ReinsDiscountTreatyPricingRateTableVersionId,
                ReinsDiscountNote = bo.ReinsDiscountNote,
                IsPerProfitComm = bo.IsPerProfitComm,
                TreatyPricingProfitCommissionId = bo.TreatyPricingProfitCommissionId,
                TreatyPricingProfitCommissionVersionId = bo.TreatyPricingProfitCommissionVersionId,
                ProfitCommNote = bo.ProfitCommNote,
                CampaignFundByMlre = bo.CampaignFundByMlre,
                ComplimentarySumAssured = bo.ComplimentarySumAssured,
                IsPerUnderwritingMethod = bo.IsPerUnderwritingMethod,
                UnderwritingMethod = bo.UnderwritingMethod,
                IsPerUnderwritingQuestion = bo.IsPerUnderwritingQuestion,
                TreatyPricingUwQuestionnaireId = bo.TreatyPricingUwQuestionnaireId,
                TreatyPricingUwQuestionnaireVersionId = bo.TreatyPricingUwQuestionnaireVersionId,
                UnderwritingQuestionNote = bo.UnderwritingQuestionNote,
                IsPerMedicalTable = bo.IsPerMedicalTable,
                TreatyPricingMedicalTableId = bo.TreatyPricingMedicalTableId,
                TreatyPricingMedicalTableVersionId = bo.TreatyPricingMedicalTableVersionId,
                MedicalTableNote = bo.MedicalTableNote,
                IsPerFinancialTable = bo.IsPerFinancialTable,
                TreatyPricingFinancialTableId = bo.TreatyPricingFinancialTableId,
                TreatyPricingFinancialTableVersionId = bo.TreatyPricingFinancialTableVersionId,
                FinancialTableNote = bo.FinancialTableNote,
                IsPerAggregationNotes = bo.IsPerAggregationNotes,
                AggregationNotes = bo.AggregationNotes,
                IsPerAdvantageProgram = bo.IsPerAdvantageProgram,
                TreatyPricingAdvantageProgramId = bo.TreatyPricingAdvantageProgramId,
                TreatyPricingAdvantageProgramVersionId = bo.TreatyPricingAdvantageProgramVersionId,
                AdvantageProgramNote = bo.AdvantageProgramNote,
                IsPerWaitingPeriod = bo.IsPerWaitingPeriod,
                WaitingPeriod = bo.WaitingPeriod,
                IsPerSurvivalPeriod = bo.IsPerSurvivalPeriod,
                SurvivalPeriod = bo.SurvivalPeriod,
                OtherCampaignCriteria = bo.OtherCampaignCriteria,
                AdditionalRemark = bo.AdditionalRemark,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingCampaignVersion.IsExists(id);
        }

        public static TreatyPricingCampaignVersionBo Find(int id, bool foreign = false)
        {
            return FormBo(TreatyPricingCampaignVersion.Find(id), foreign);
        }

        public static TreatyPricingCampaignVersionBo FindByParentIdVersion(int parentId, int? version)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingCampaignVersions
                    .Where(q => q.TreatyPricingCampaignId == parentId)
                    .Where(q => q.Version == version)
                    .FirstOrDefault());
            }
        }

        public static IList<TreatyPricingCampaignVersionBo> GetByTreatyPricingCampaignId(int? treatyPricingCampaignId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingCampaignVersions
                    .Where(q => q.TreatyPricingCampaignId == treatyPricingCampaignId)
                    .ToList());
            }
        }

        public static IList<TreatyPricingCampaignVersionBo> GetByTreatyPricingCampaignVersion(int version)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingCampaignVersions.Where(q => q.Id == version).ToList());
            }
        }

        public static Result Save(ref TreatyPricingCampaignVersionBo bo)
        {
            if (!TreatyPricingCampaignVersion.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingCampaignVersionBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingCampaignVersion.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingCampaignVersionBo bo)
        {
            TreatyPricingCampaignVersion entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingCampaignVersionBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingCampaignVersionBo bo)
        {
            Result result = Result();

            TreatyPricingCampaignVersion entity = TreatyPricingCampaignVersion.Find(bo.Id);
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

        public static Result Update(ref TreatyPricingCampaignVersionBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingCampaignVersionBo bo)
        {
            TreatyPricingCampaignVersion.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingCampaignVersionBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingCampaignVersion.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
