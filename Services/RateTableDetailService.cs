using BusinessObject;
using BusinessObject.RiDatas;
using DataAccess.Entities;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;

namespace Services
{
    public class RateTableDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RateTableDetail)),
            };
        }

        public static RateTableDetailBo FormBo(RateTableDetail entity = null)
        {
            if (entity == null)
                return null;
            return new RateTableDetailBo
            {
                Id = entity.Id,
                RateTableId = entity.RateTableId,
                RateTableBo = RateTableService.Find(entity.RateTableId),
                Combination = entity.Combination,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,

                TreatyCode = entity.TreatyCode,
                CedingPlanCode = entity.CedingPlanCode,
                CedingTreatyCode = entity.CedingTreatyCode,
                CedingPlanCode2 = entity.CedingPlanCode2,
                CedingBenefitTypeCode = entity.CedingBenefitTypeCode,
                CedingBenefitRiskCode = entity.CedingBenefitRiskCode,
                GroupPolicyNumber = entity.GroupPolicyNumber,
            };
        }

        public static IList<RateTableDetailBo> FormBos(IList<RateTableDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<RateTableDetailBo> bos = new List<RateTableDetailBo>() { };
            foreach (RateTableDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RateTableDetail FormEntity(RateTableDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new RateTableDetail
            {
                Id = bo.Id,
                RateTableId = bo.RateTableId,
                Combination = bo.Combination?.Trim(),
                CreatedById = bo.CreatedById,
                CreatedAt = bo.CreatedAt,

                TreatyCode = bo.TreatyCode?.Trim(),
                CedingPlanCode = bo.CedingPlanCode?.Trim(),
                CedingTreatyCode = bo.CedingTreatyCode?.Trim(),
                CedingPlanCode2 = bo.CedingPlanCode2?.Trim(),
                CedingBenefitTypeCode = bo.CedingBenefitTypeCode?.Trim(),
                CedingBenefitRiskCode = bo.CedingBenefitRiskCode?.Trim(),
                GroupPolicyNumber = bo.GroupPolicyNumber?.Trim(),
            };
        }

        public static bool IsExists(int id)
        {
            return RateTableDetail.IsExists(id);
        }

        public static RateTableDetailBo Find(int id)
        {
            return FormBo(RateTableDetail.Find(id));
        }

        public static RateTableDetailBo FindByCombination(
            string combination,
            double? policyAmountFrom,
            double? policyAmountTo,
            int? attainedAgeFrom,
            int? attainedAgeTo,
            double? aarFrom,
            double? aarTo,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            double? policyTermFrom,
            double? policyTermTo,
            double? policyDurationFrom,
            double? policyDurationTo,
            int? rateTableId = null
        )
        {
            return FormBo(RateTableDetail.FindByCombination(
                combination,
                policyAmountFrom,
                policyAmountTo,
                attainedAgeFrom,
                attainedAgeTo,
                aarFrom,
                aarTo,
                reinsEffDatePolStartDate,
                reinsEffDatePolEndDate,
                reportingStartDate,
                reportingEndDate,
                policyTermFrom,
                policyTermTo,
                policyDurationFrom,
                policyDurationTo,
                rateTableId
            ));
        }

        public static RateTableDetailBo FindByCombination(string combination, RiDataBo riData, DateTime? reportingDate)
        {
            return FindByCombination(
                combination,
                riData.OriSumAssured,
                riData.OriSumAssured,
                riData.InsuredAttainedAge,
                riData.InsuredAttainedAge,
                riData.Aar,
                riData.Aar,
                riData.ReinsEffDatePol,
                riData.ReinsEffDatePol,
                reportingDate,
                reportingDate,
                riData.PolicyTerm,
                riData.PolicyTerm,
                riData.DurationMonth,
                riData.DurationMonth
            );
        }

        public static RateTableDetailBo FindByCombination(string combination, RateTableBo rateTableBo)
        {
            return FindByCombination(
                combination,
                rateTableBo.PolicyAmountFrom,
                rateTableBo.PolicyAmountTo,
                rateTableBo.AttainedAgeFrom,
                rateTableBo.AttainedAgeTo,
                rateTableBo.AarFrom,
                rateTableBo.AarTo,
                rateTableBo.ReinsEffDatePolStartDate,
                rateTableBo.ReinsEffDatePolEndDate,
                rateTableBo.ReportingStartDate,
                rateTableBo.ReportingEndDate,
                rateTableBo.PolicyTermFrom,
                rateTableBo.PolicyTermTo,
                rateTableBo.PolicyDurationFrom,
                rateTableBo.PolicyDurationTo,
                rateTableBo.Id
            );
        }

        public static RateTableDetailBo FindByParams(
            string treatyCode,
            string cedingPlanCode = null,
            string cedingTreatyCode = null,
            string cedingPlanCode2 = null,
            string cedingBenefitTypeCode = null,
            string cedingBenefitRiskCode = null,
            string groupPolicyNumber = null,
            string mlreBenefitCode = null,
            int? reinsBasisCodeId = null,
            int? premiumFrequencyCodeId = null,
            int? insuredAttainedAge = null,
            double? oriSumAssured = null,
            double? aar = null,
            DateTime? reinsEffDatePol = null,
            DateTime? reportingDate = null,
            double? policyTerm = null,
            double? policyDuration = null,
            bool groupById = false
        )
        {
            return FormBo(RateTableDetail.FindByParams(
                treatyCode,
                cedingPlanCode,
                cedingTreatyCode,
                cedingPlanCode2,
                cedingBenefitTypeCode,
                cedingBenefitRiskCode,
                groupPolicyNumber,
                mlreBenefitCode,
                reinsBasisCodeId,
                premiumFrequencyCodeId,
                insuredAttainedAge,
                oriSumAssured,
                aar,
                reinsEffDatePol,
                reportingDate,
                policyTerm,
                policyDuration,
                groupById
            ));
        }

        public static RateTableDetailBo FindByParams(
            RiDataBo riData,
            CacheService pickListCache,
            DateTime? reportingDate,
            bool groupById = false
        )
        {
            return FindByParams(
                riData.TreatyCode,
                riData.CedingPlanCode,
                riData.CedingTreatyCode,
                riData.CedingPlanCode2,
                riData.CedingBenefitTypeCode,
                riData.CedingBenefitRiskCode,
                riData.GroupPolicyNumber,
                riData.MlreBenefitCode,
                pickListCache.GetReinsBasisCodeId(riData),
                pickListCache.GetPremiumFrequencyCodeId(riData),
                riData.InsuredAttainedAge,
                riData.OriSumAssured,
                riData.Aar,
                riData.ReinsEffDatePol,
                reportingDate,
                riData.PolicyTerm,
                riData.DurationMonth,
                groupById
            );
        }

        public static int CountByCombination(
            string combination,
            double? policyAmountFrom,
            double? policyAmountTo,
            int? attainedAgeFrom,
            int? attainedAgeTo,
            double? aarFrom,
            double? aarTo,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            double? policyTermFrom,
            double? policyTermTo,
            double? policyDurationFrom,
            double? policyDurationTo,
            int? rateTableId = null
        )
        {
            return RateTableDetail.CountByCombination(
                combination,
                policyAmountFrom,
                policyAmountTo,
                attainedAgeFrom,
                attainedAgeTo,
                aarFrom,
                aarTo,
                reinsEffDatePolStartDate,
                reinsEffDatePolEndDate,
                reportingStartDate,
                reportingEndDate,
                policyTermFrom,
                policyTermTo,
                policyDurationFrom,
                policyDurationTo,
                rateTableId
            );
        }

        public static int CountByCombination(string combination, RateTableBo rateTableBo)
        {
            return CountByCombination(
                combination,
                rateTableBo.PolicyAmountFrom,
                rateTableBo.PolicyAmountTo,
                rateTableBo.AttainedAgeFrom,
                rateTableBo.AttainedAgeTo,
                rateTableBo.AarFrom,
                rateTableBo.AarTo,
                rateTableBo.ReinsEffDatePolStartDate,
                rateTableBo.ReinsEffDatePolEndDate,
                rateTableBo.ReportingStartDate,
                rateTableBo.ReportingEndDate,
                rateTableBo.PolicyTermFrom,
                rateTableBo.PolicyTermTo,
                rateTableBo.PolicyDurationFrom,
                rateTableBo.PolicyDurationTo,
                rateTableBo.Id
            );
        }

        public static int CountByParams(
            string treatyCode,
            string cedingPlanCode = null,
            string cedingTreatyCode = null,
            string cedingPlanCode2 = null,
            string cedingBenefitTypeCode = null,
            string cedingBenefitRiskCode = null,
            string groupPolicyNumber = null,
            string mlreBenefitCode = null,
            int? reinsBasisCodeId = null,
            int? premiumFrequencyCodeId = null,
            int? insuredAttainedAge = null,
            double? oriSumAssured = null,
            double? aar = null,
            DateTime? reinsEffDatePol = null,
            DateTime? reportingDate = null,
            double? PolicyTerm = null,
            double? policyDuration = null,
            bool groupById = false
        )
        {
            return RateTableDetail.CountByParams(
                treatyCode,
                cedingPlanCode,
                cedingTreatyCode,
                cedingPlanCode2,
                cedingBenefitTypeCode,
                cedingBenefitRiskCode,
                groupPolicyNumber,
                mlreBenefitCode,
                reinsBasisCodeId,
                premiumFrequencyCodeId,
                insuredAttainedAge,
                oriSumAssured,
                aar,
                reinsEffDatePol,
                reportingDate,
                PolicyTerm,
                policyDuration,
                groupById
            );
        }

        public static int CountByParams(
            RiDataBo riData,
            CacheService pickListCache,
            DateTime? reportingDate,
            bool groupById = false
        )
        {
            return CountByParams(
                riData.TreatyCode,
                riData.CedingPlanCode,
                riData.CedingTreatyCode,
                riData.CedingPlanCode2,
                riData.CedingBenefitTypeCode,
                riData.CedingBenefitRiskCode,
                riData.GroupPolicyNumber,
                riData.MlreBenefitCode,
                pickListCache.GetReinsBasisCodeId(riData),
                pickListCache.GetPremiumFrequencyCodeId(riData),
                riData.InsuredAttainedAge,
                riData.OriSumAssured,
                riData.Aar,
                riData.ReinsEffDatePol,
                reportingDate,
                riData.PolicyTerm,
                riData.DurationMonth,
                groupById
            );
        }

        public static IList<RateTableDetailBo> GetByParams(
            string treatyCode,
            string cedingPlanCode = null,
            string cedingTreatyCode = null,
            string cedingPlanCode2 = null,
            string cedingBenefitTypeCode = null,
            string cedingBenefitRiskCode = null,
            string groupPolicyNumber = null,
            string mlreBenefitCode = null,
            int? reinsBasisCodeId = null,
            int? premiumFrequencyCodeId = null,
            int? insuredAttainedAge = null,
            double? oriSumAssured = null,
            double? aar = null,
            DateTime? reinsEffDatePol = null,
            DateTime? reportingDate = null,
            double? policyTerm = null,
            double? policyDuration = null,
            bool groupById = false
        )
        {
            return FormBos(RateTableDetail.GetByParams(
                treatyCode,
                cedingPlanCode,
                cedingTreatyCode,
                cedingPlanCode2,
                cedingBenefitTypeCode,
                cedingBenefitRiskCode,
                groupPolicyNumber,
                mlreBenefitCode,
                reinsBasisCodeId,
                premiumFrequencyCodeId,
                insuredAttainedAge,
                oriSumAssured,
                aar,
                reinsEffDatePol,
                reportingDate,
                policyTerm,
                policyDuration,
                groupById
            ));
        }

        public static IList<RateTableDetailBo> GetByParams(
            RiDataBo riData,
            CacheService pickListCache,
            DateTime? reportingDate,
            bool groupById = false
        )
        {
            return GetByParams(
                riData.TreatyCode,
                riData.CedingPlanCode,
                riData.CedingTreatyCode,
                riData.CedingPlanCode2,
                riData.CedingBenefitTypeCode,
                riData.CedingBenefitRiskCode,
                riData.GroupPolicyNumber,
                riData.MlreBenefitCode,
                pickListCache.GetReinsBasisCodeId(riData),
                pickListCache.GetPremiumFrequencyCodeId(riData),
                riData.InsuredAttainedAge,
                riData.OriSumAssured,
                riData.Aar,
                riData.ReinsEffDatePol,
                reportingDate,
                riData.PolicyTerm,
                riData.DurationMonth,
                groupById
            );
        }

        public static int CountDuplicateByParams(
            string treatyCode,
            string cedingPlanCode,
            string cedingTreatyCode,
            string cedingPlanCode2,
            string cedingBenefitTypeCode,
            string cedingBenefitRiskCode,
            string groupPolicyNumber,
            int? benefitId,
            int? premiumFrequencyCodePickListDetailId,
            int? reinsBasisCodePickListDetailId,
            double? policyAmountFrom,
            double? policyAmountTo,
            int? attainedAgeFrom,
            int? attainedAgeTo,
            double? aarFrom,
            double? aarTo,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            double? policyTermFrom,
            double? policyTermTo,
            double? policyDurationFrom,
            double? policyDurationTo,
            int? rateTableId = null
        )
        {
            return RateTableDetail.CountDuplicateByParams(
                treatyCode,
                cedingPlanCode,
                cedingTreatyCode,
                cedingPlanCode2,
                cedingBenefitTypeCode,
                cedingBenefitRiskCode,
                groupPolicyNumber,
                benefitId,
                premiumFrequencyCodePickListDetailId,
                reinsBasisCodePickListDetailId,
                policyAmountFrom,
                policyAmountTo,
                attainedAgeFrom,
                attainedAgeTo,
                aarFrom,
                aarTo,
                reinsEffDatePolStartDate,
                reinsEffDatePolEndDate,
                reportingStartDate,
                reportingEndDate,
                policyTermFrom,
                policyTermTo,
                policyDurationFrom,
                policyDurationTo,
                rateTableId
            );
        }

        public static int CountDuplicateByParams(RateTableBo rateTableBo, RateTableDetailBo rateTableDetailBo)
        {
            return CountDuplicateByParams(
                rateTableDetailBo.TreatyCode,
                rateTableDetailBo.CedingPlanCode,
                rateTableDetailBo.CedingTreatyCode,
                rateTableDetailBo.CedingPlanCode2,
                rateTableDetailBo.CedingBenefitTypeCode,
                rateTableDetailBo.CedingBenefitRiskCode,
                rateTableDetailBo.GroupPolicyNumber,
                rateTableBo.BenefitId,
                rateTableBo.PremiumFrequencyCodePickListDetailId,
                rateTableBo.ReinsBasisCodePickListDetailId,
                rateTableBo.PolicyAmountFrom,
                rateTableBo.PolicyAmountTo,
                rateTableBo.AttainedAgeFrom,
                rateTableBo.AttainedAgeTo,
                rateTableBo.AarFrom,
                rateTableBo.AarTo,
                rateTableBo.ReinsEffDatePolStartDate,
                rateTableBo.ReinsEffDatePolEndDate,
                rateTableBo.ReportingStartDate,
                rateTableBo.ReportingEndDate,
                rateTableBo.PolicyTermFrom,
                rateTableBo.PolicyTermTo,
                rateTableBo.PolicyDurationFrom,
                rateTableBo.PolicyDurationTo,
                rateTableBo.Id
            );
        }

        public static Result Save(ref RateTableDetailBo bo)
        {
            if (!RateTableDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref RateTableDetailBo bo, ref TrailObject trail)
        {
            if (!RateTableDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RateTableDetailBo bo)
        {
            RateTableDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RateTableDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RateTableDetailBo bo)
        {
            Result result = Result();

            RateTableDetail entity = RateTableDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.RateTableId = bo.RateTableId;
                entity.Combination = bo.Combination;
                entity.TreatyCode = bo.TreatyCode;
                entity.CedingPlanCode = bo.CedingPlanCode;
                entity.CedingTreatyCode = bo.CedingTreatyCode;
                entity.CedingPlanCode2 = bo.CedingPlanCode2;
                entity.CedingBenefitTypeCode = bo.CedingBenefitTypeCode;
                entity.CedingBenefitRiskCode = bo.CedingBenefitRiskCode;
                entity.GroupPolicyNumber = bo.GroupPolicyNumber;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RateTableDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RateTableDetailBo bo)
        {
            RateTableDetail.Delete(bo.Id);
        }

        public static Result Delete(RateTableDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = RateTableDetail.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static IList<DataTrail> DeleteByRateTableId(int rateTableId)
        {
            return RateTableDetail.DeleteByRateTableId(rateTableId);
        }

        public static void DeleteByRateTableId(int rateTableId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteByRateTableId(rateTableId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(RateTableDetail)));
                }
            }
        }
    }
}
