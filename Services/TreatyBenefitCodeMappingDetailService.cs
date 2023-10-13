using BusinessObject;
using BusinessObject.Claims;
using BusinessObject.RiDatas;
using DataAccess.Entities;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;

namespace Services
{
    public class TreatyBenefitCodeMappingDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyBenefitCodeMappingDetail)),
            };
        }

        public static TreatyBenefitCodeMappingDetailBo FormBo(TreatyBenefitCodeMappingDetail entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyBenefitCodeMappingDetailBo
            {
                Id = entity.Id,
                Type = entity.Type,
                TreatyBenefitCodeMappingId = entity.TreatyBenefitCodeMappingId,
                TreatyBenefitCodeMappingBo = TreatyBenefitCodeMappingService.Find(entity.TreatyBenefitCodeMappingId),
                Combination = entity.Combination,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,

                CedingPlanCode = entity.CedingPlanCode,
                CedingBenefitTypeCode = entity.CedingBenefitTypeCode,
                CedingBenefitRiskCode = entity.CedingBenefitRiskCode,
                CedingTreatyCode = entity.CedingTreatyCode,
                CampaignCode = entity.CampaignCode,
            };
        }

        public static IList<TreatyBenefitCodeMappingDetailBo> FormBos(IList<TreatyBenefitCodeMappingDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyBenefitCodeMappingDetailBo> bos = new List<TreatyBenefitCodeMappingDetailBo>() { };
            foreach (TreatyBenefitCodeMappingDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyBenefitCodeMappingDetail FormEntity(TreatyBenefitCodeMappingDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyBenefitCodeMappingDetail
            {
                Id = bo.Id,
                Type = bo.Type,
                TreatyBenefitCodeMappingId = bo.TreatyBenefitCodeMappingId,
                Combination = bo.Combination?.Trim(),
                CreatedById = bo.CreatedById,
                CreatedAt = bo.CreatedAt,

                CedingPlanCode = bo.CedingPlanCode?.Trim(),
                CedingBenefitTypeCode = bo.CedingBenefitTypeCode?.Trim(),
                CedingBenefitRiskCode = bo.CedingBenefitRiskCode?.Trim(),
                CedingTreatyCode = bo.CedingTreatyCode?.Trim(),
                CampaignCode = bo.CampaignCode?.Trim(),
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyBenefitCodeMappingDetail.IsExists(id);
        }

        public static TreatyBenefitCodeMappingDetailBo Find(int id)
        {
            return FormBo(TreatyBenefitCodeMappingDetail.Find(id));
        }

        public static TreatyBenefitCodeMappingDetailBo FindCombinationForTreaty(
            string combination,
            TreatyBenefitCodeMappingBo bo
        )
        {
            return FindCombinationForTreaty(
                bo.CedantId,
                combination,
                bo.ReinsEffDatePolStartDate,
                bo.ReinsEffDatePolEndDate,
                bo.AttainedAgeFrom,
                bo.AttainedAgeTo,
                bo.ReportingStartDate,
                bo.ReportingEndDate,
                bo.UnderwriterRatingFrom,
                bo.UnderwriterRatingTo,
                bo.OriSumAssuredFrom,
                bo.OriSumAssuredTo,
                bo.ReinsuranceIssueAgeFrom,
                bo.ReinsuranceIssueAgeTo,
                bo.Id
            );
        }

        public static TreatyBenefitCodeMappingDetailBo FindCombinationForTreaty(
            int cedantId,
            string combination,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            int? attAgeFrom = null,
            int? attAgeTo = null,
            DateTime? reportingStartDate = null,
            DateTime? reportingEndDate = null,
            double? underwriterRatingFrom = null,
            double? underwriterRatingTo = null,
            double? oriSumAssuredFrom = null,
            double? oriSumAssuredTo = null,
            int? reinsuranceIssueAgeFrom = null,
            int? reinsuranceIssueAgeTo = null,
            int? treatyBenefitCodeMappingId = null
        )
        {
            return FormBo(TreatyBenefitCodeMappingDetail.FindCombinationForTreaty(
                cedantId,
                combination,
                reinsEffDatePolStartDate,
                reinsEffDatePolEndDate,
                attAgeFrom,
                attAgeTo,
                reportingStartDate,
                reportingEndDate,
                underwriterRatingFrom,
                underwriterRatingTo,
                oriSumAssuredFrom,
                oriSumAssuredTo,
                reinsuranceIssueAgeFrom,
                reinsuranceIssueAgeTo,
                treatyBenefitCodeMappingId
            ));
        }

        public static TreatyBenefitCodeMappingDetailBo FindByTreatyParams(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            DateTime? reinsEffDatePol,
            string cedingBenefitRiskCode = null,
            string cedingTreatyCode = null,
            string campaignCode = null,
            int? reinsBasisCodeId = null,
            int? insuredAttAge = null,
            DateTime? reportDate = null,
            double? underwriterRating = null,
            double? oriSumAssured = null,
            int? reinsuranceIssueAge = null,
            bool groupById = false
        )
        {
            return FormBo(TreatyBenefitCodeMappingDetail.FindByTreatyParams(
                cedantId,
                cedingPlanCode,
                cedingBenefitTypeCode,
                reinsEffDatePol,
                cedingBenefitRiskCode,
                cedingTreatyCode,
                campaignCode,
                reinsBasisCodeId,
                insuredAttAge,
                reportDate,
                underwriterRating,
                oriSumAssured,
                reinsuranceIssueAge,
                groupById
            ));
        }

        public static TreatyBenefitCodeMappingDetailBo FindByTreatyParams(
            int cedantId,
            RiDataBo riData,
            CacheService pickListCache,
            DateTime? reportDate = null,
            bool groupById = false
        )
        {
            return FindByTreatyParams(
                cedantId,
                riData.CedingPlanCode,
                riData.CedingBenefitTypeCode,
                riData.ReinsEffDatePol,
                riData.CedingBenefitRiskCode,
                riData.CedingTreatyCode,
                riData.CampaignCode,
                pickListCache.GetReinsBasisCodeId(riData),
                riData.InsuredAttainedAge,
                reportDate,
                riData.UnderwriterRating,
                riData.OriSumAssured,
                riData.ReinsuranceIssueAge,
                groupById
            );
        }

        public static TreatyBenefitCodeMappingDetailBo FindByTreatyParamsForClaim(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            DateTime? reinsEffDatePol,
            string cedingBenefitRiskCode = null,
            string cedingTreatyCode = null,
            string campaignCode = null,
            int? reinsBasisCodeId = null,
            bool groupById = false
        )
        {
            return FormBo(TreatyBenefitCodeMappingDetail.FindByTreatyParamsForClaim(
                cedantId,
                cedingPlanCode,
                cedingBenefitTypeCode,
                reinsEffDatePol,
                cedingBenefitRiskCode,
                cedingTreatyCode,
                campaignCode,
                reinsBasisCodeId,
                groupById
            ));
        }

        public static TreatyBenefitCodeMappingDetailBo FindByTreatyParamsForClaim(
            int cedantId,
            ClaimDataBo claimData,
            CacheService pickListCache,
            bool groupById = false
        )
        {
            return FindByTreatyParamsForClaim(
                cedantId,
                claimData.CedingPlanCode,
                claimData.CedingBenefitTypeCode,
                claimData.ReinsEffDatePol,
                claimData.CedingBenefitRiskCode,
                claimData.CedingTreatyCode,
                claimData.CampaignCode,
                pickListCache.GetReinsBasisCodeId(claimData),
                groupById
            );
        }

        public static TreatyBenefitCodeMappingDetailBo FindCombinationForBenefit(
            int cedantId,
            string combination,
            TreatyBenefitCodeMappingBo bo
        )
        {
            return FindCombinationForBenefit(
                cedantId,
                combination,
                bo.AttainedAgeFrom,
                bo.AttainedAgeTo,
                bo.ReportingStartDate,
                bo.ReportingEndDate,
                bo.TreatyCodeId,
                bo.ReinsEffDatePolStartDate,
                bo.ReinsEffDatePolEndDate,
                bo.UnderwriterRatingFrom,
                bo.UnderwriterRatingTo,
                bo.OriSumAssuredFrom,
                bo.OriSumAssuredTo,
                bo.ReinsuranceIssueAgeFrom,
                bo.ReinsuranceIssueAgeTo,
                bo.Id
            );
        }

        public static TreatyBenefitCodeMappingDetailBo FindCombinationForBenefit(
            int cedantId,
            string combination,
            int? attainedAgeFrom,
            int? attainedAgeTo,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            int? treatyCodeId,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            double? underwriterRatingFrom,
            double? underwriterRatingTo,
            double? oriSumAssuredFrom,
            double? oriSumAssuredTo,
            int? reinsuranceIssueAgeFrom,
            int? reinsuranceIssueAgeTo,
            int? treatyBenefitCodeMappingId = null
        )
        {
            return FormBo(TreatyBenefitCodeMappingDetail.FindCombinationForBenefit(
                cedantId,
                combination,
                attainedAgeFrom,
                attainedAgeTo,
                reportingStartDate,
                reportingEndDate,
                treatyCodeId,
                reinsEffDatePolStartDate,
                reinsEffDatePolEndDate,
                underwriterRatingFrom,
                underwriterRatingTo,
                oriSumAssuredFrom,
                oriSumAssuredTo,
                reinsuranceIssueAgeFrom,
                reinsuranceIssueAgeTo,
                treatyBenefitCodeMappingId
            ));
        }

        public static TreatyBenefitCodeMappingDetailBo FindByBenefitParams(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            DateTime? reinsEffDatePol,
            string cedingBenefitRiskCode = null,
            int? insuredAttainedAge = null,
            DateTime? reportDate = null,
            string treatyCode = null,
            string cedingTreatyCode = null,
            string campaignCode = null,
            int? reinsBasisCodeId = null,
            double? underwriterRating = null,
            double? oriSumAssured = null,
            int? reinsuranceIssueAge = null,
            bool groupById = false
        )
        {
            return FormBo(TreatyBenefitCodeMappingDetail.FindByBenefitParams(
                cedantId,
                cedingPlanCode,
                cedingBenefitTypeCode,
                reinsEffDatePol,
                cedingBenefitRiskCode,
                insuredAttainedAge,
                reportDate,
                treatyCode,
                cedingTreatyCode,
                campaignCode,
                reinsBasisCodeId,
                underwriterRating,
                oriSumAssured,
                reinsuranceIssueAge,
                groupById
            ));
        }

        public static TreatyBenefitCodeMappingDetailBo FindByBenefitParams(
            int cedantId,
            RiDataBo riData,
            CacheService pickListCache,
            DateTime? reportDate = null,
            bool groupById = false
        )
        {
            return FindByBenefitParams(
                cedantId,
                riData.CedingPlanCode,
                riData.CedingBenefitTypeCode,
                riData.ReinsEffDatePol,
                riData.CedingBenefitRiskCode,
                riData.InsuredAttainedAge,
                reportDate,
                riData.TreatyCode,
                riData.CedingTreatyCode,
                riData.CampaignCode,
                pickListCache.GetReinsBasisCodeId(riData),
                riData.UnderwriterRating,
                riData.OriSumAssured,
                riData.ReinsuranceIssueAge,
                groupById
            );
        }

        public static TreatyBenefitCodeMappingDetailBo FindCombinationForProductFeature(
            int cedantId,
            string combination,
            TreatyBenefitCodeMappingBo bo
        )
        {
            return FindCombinationForProductFeature(
                cedantId,
                combination,
                bo.ReinsEffDatePolStartDate,
                bo.ReinsEffDatePolEndDate,
                bo.AttainedAgeFrom,
                bo.AttainedAgeTo,
                bo.TreatyCodeId,
                bo.BenefitId,
                bo.UnderwriterRatingFrom,
                bo.UnderwriterRatingTo,
                bo.OriSumAssuredFrom,
                bo.OriSumAssuredTo,
                bo.ReinsuranceIssueAgeFrom,
                bo.ReinsuranceIssueAgeTo,
                bo.ReportingStartDate,
                bo.ReportingEndDate,
                bo.Id
            );
        }

        public static TreatyBenefitCodeMappingDetailBo FindCombinationForProductFeature(
            int cedantId,
            string combination,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            int? attAgeFrom = null,
            int? attAgeTo = null,
            int? treatyCodeId = null,
            int? benefitId = null,
            double? underwriterRatingFrom = null,
            double? underwriterRatingTo = null,
            double? oriSumAssuredFrom = null,
            double? oriSumAssuredTo = null,
            int? reinsuranceIssueAgeFrom = null,
            int? reinsuranceIssueAgeTo = null,
            DateTime? reportingStartDate = null,
            DateTime? reportingEndDate = null,
            int? treatyBenefitCodeMappingId = null
        )
        {
            return FormBo(TreatyBenefitCodeMappingDetail.FindCombinationForProductFeature(
                cedantId,
                combination,
                reinsEffDatePolStartDate,
                reinsEffDatePolEndDate,
                attAgeFrom,
                attAgeTo,
                treatyCodeId,
                benefitId,
                underwriterRatingFrom,
                underwriterRatingTo,
                oriSumAssuredFrom,
                oriSumAssuredTo,
                reinsuranceIssueAgeFrom,
                reinsuranceIssueAgeTo,
                reportingStartDate,
                reportingEndDate,
                treatyBenefitCodeMappingId
            ));
        }

        public static TreatyBenefitCodeMappingDetailBo FindByProductFeatureParams(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            string treatyCode,
            string benefitCode,
            DateTime? reinsEffDatePol,
            int? insuredAttAge = null,
            string campaignCode = null,
            double? underwriterRating = null,
            double? oriSumAssured = null,
            int? reinsuranceIssueAge = null,
            DateTime? reportDate = null,
            string cedingBenefitRiskCode = null,
            string cedingTreatyCode = null,
            int? reinsBasisCodeId = null,
            bool groupById = false
        )
        {
            return FormBo(TreatyBenefitCodeMappingDetail.FindByProductFeatureParams(
                cedantId,
                cedingPlanCode,
                cedingBenefitTypeCode,
                treatyCode,
                benefitCode,
                reinsEffDatePol,
                insuredAttAge,
                campaignCode,
                underwriterRating,
                oriSumAssured,
                reinsuranceIssueAge,
                reportDate,
                cedingBenefitRiskCode,
                cedingTreatyCode,
                reinsBasisCodeId,
                groupById
            ));
        }

        public static TreatyBenefitCodeMappingDetailBo FindByProductFeatureParams(
            int cedantId,
            RiDataBo riData,
            CacheService pickListCache,
            DateTime? reportDate = null,
            bool groupById = false
        )
        {
            return FindByProductFeatureParams(
                cedantId,
                riData.CedingPlanCode,
                riData.CedingBenefitTypeCode,
                riData.TreatyCode,
                riData.MlreBenefitCode,
                riData.ReinsEffDatePol,
                riData.InsuredAttainedAge,
                riData.CampaignCode,
                riData.UnderwriterRating,
                riData.OriSumAssured,
                riData.ReinsuranceIssueAge,
                reportDate,
                riData.CedingBenefitRiskCode,
                riData.CedingTreatyCode,
                pickListCache.GetReinsBasisCodeId(riData),
                groupById
            );
        }

        public static int CountCombinationForTreaty(
            string combination,
            TreatyBenefitCodeMappingBo bo
        )
        {
            return CountCombinationForTreaty(
                bo.CedantId,
                combination,
                bo.ReinsEffDatePolStartDate,
                bo.ReinsEffDatePolEndDate,
                bo.AttainedAgeFrom,
                bo.AttainedAgeTo,
                bo.ReportingStartDate,
                bo.ReportingEndDate,
                bo.UnderwriterRatingFrom,
                bo.UnderwriterRatingTo,
                bo.OriSumAssuredFrom,
                bo.OriSumAssuredTo,
                bo.ReinsuranceIssueAgeFrom,
                bo.ReinsuranceIssueAgeTo,
                bo.Id
            );
        }

        public static int CountCombinationForTreaty(
            int cedantId,
            string combination,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            int? attAgeFrom,
            int? attAgeTo,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            double? underwriterRatingFrom,
            double? underwriterRatingTo,
            double? oriSumAssuredFrom,
            double? oriSumAssuredTo,
            int? reinsuranceIssueAgeFrom,
            int? reinsuranceIssueAgeTo,
            int? treatyBenefitCodeMappingId = null
        )
        {
            return TreatyBenefitCodeMappingDetail.CountCombinationForTreaty(
                cedantId,
                combination,
                reinsEffDatePolStartDate,
                reinsEffDatePolEndDate,
                attAgeFrom,
                attAgeTo,
                reportingStartDate,
                reportingEndDate,
                underwriterRatingFrom,
                underwriterRatingTo,
                oriSumAssuredFrom,
                oriSumAssuredTo,
                reinsuranceIssueAgeFrom,
                reinsuranceIssueAgeTo,
                treatyBenefitCodeMappingId
            );
        }

        public static int CountByTreatyParams(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            DateTime? reinsEffDatePol,
            string cedingBenefitRiskCode = null,
            string cedingTreatyCode = null,
            string campaignCode = null,
            int? reinsBasisCodeId = null,
            int? insuredAttAge = null,
            DateTime? reportDate = null,
            double? underwriterRating = null,
            double? oriSumAssured = null,
            int? reinsuranceIssueAge = null,
            bool groupById = false
        )
        {
            return TreatyBenefitCodeMappingDetail.CountByTreatyParams(
                cedantId,
                cedingPlanCode,
                cedingBenefitTypeCode,
                reinsEffDatePol,
                cedingBenefitRiskCode,
                cedingTreatyCode,
                campaignCode,
                reinsBasisCodeId,
                insuredAttAge,
                reportDate,
                underwriterRating,
                oriSumAssured,
                reinsuranceIssueAge,
                groupById
            );
        }

        public static int CountByTreatyParams(
            int cedantId,
            RiDataBo riData,
            CacheService pickListCache,
            DateTime? reportDate = null,
            bool groupById = false
        )
        {
            return CountByTreatyParams(
                cedantId,
                riData.CedingPlanCode,
                riData.CedingBenefitTypeCode,
                riData.ReinsEffDatePol,
                riData.CedingBenefitRiskCode,
                riData.CedingTreatyCode,
                riData.CampaignCode,
                pickListCache.GetReinsBasisCodeId(riData),
                riData.InsuredAttainedAge,
                reportDate,
                riData.UnderwriterRating,
                riData.OriSumAssured,
                riData.ReinsuranceIssueAge,
                groupById
            );
        }

        public static int CountByTreatyParamsForClaim(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            DateTime? reinsEffDatePol,
            string cedingBenefitRiskCode = null,
            string cedingTreatyCode = null,
            string campaignCode = null,
            int? reinsBasisCodeId = null,
            bool groupById = false
        )
        {
            return TreatyBenefitCodeMappingDetail.CountByTreatyParamsForClaim(
                cedantId,
                cedingPlanCode,
                cedingBenefitTypeCode,
                reinsEffDatePol,
                cedingBenefitRiskCode,
                cedingTreatyCode,
                campaignCode,
                reinsBasisCodeId,
                groupById
            );
        }

        public static int CountByTreatyParamsForClaim(
            int cedantId,
            ClaimDataBo claimData,
            CacheService pickListCache,
            bool groupById = false
        )
        {
            return CountByTreatyParamsForClaim(
                cedantId,
                claimData.CedingPlanCode,
                claimData.CedingBenefitTypeCode,
                claimData.ReinsEffDatePol,
                claimData.CedingBenefitRiskCode,
                claimData.CedingTreatyCode,
                claimData.CampaignCode,
                pickListCache.GetReinsBasisCodeId(claimData),
                groupById
            );
        }

        public static int CountCombinationForBenefit(
            string combination,
            TreatyBenefitCodeMappingBo bo
        )
        {
            return CountCombinationForBenefit(
                bo.CedantId,
                combination,
                bo.AttainedAgeFrom,
                bo.AttainedAgeTo,
                bo.ReportingStartDate,
                bo.ReportingEndDate,
                bo.TreatyCodeId,
                bo.ReinsEffDatePolStartDate,
                bo.ReinsEffDatePolEndDate,
                bo.UnderwriterRatingFrom,
                bo.UnderwriterRatingTo,
                bo.OriSumAssuredFrom,
                bo.OriSumAssuredTo,
                bo.ReinsuranceIssueAgeFrom,
                bo.ReinsuranceIssueAgeTo,
                bo.Id
            );
        }

        public static int CountCombinationForBenefit(
            int cedantId,
            string combination,
            int? attainedAgeFrom,
            int? attainedAgeTo,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            int? treatyCodeId,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            double? underwriterRatingFrom,
            double? underwriterRatingTo,
            double? oriSumAssuredFrom,
            double? oriSumAssuredTo,
            int? reinsuranceIssueAgeFrom,
            int? reinsuranceIssueAgeTo,
            int? treatyBenefitCodeMappingId = null
        )
        {
            return TreatyBenefitCodeMappingDetail.CountCombinationForBenefit(
                cedantId,
                combination,
                attainedAgeFrom,
                attainedAgeTo,
                reportingStartDate,
                reportingEndDate,
                treatyCodeId,
                reinsEffDatePolStartDate,
                reinsEffDatePolEndDate,
                underwriterRatingFrom,
                underwriterRatingTo,
                oriSumAssuredFrom,
                oriSumAssuredTo,
                reinsuranceIssueAgeFrom,
                reinsuranceIssueAgeTo,
                treatyBenefitCodeMappingId
            );
        }

        public static int CountByBenefitParams(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            DateTime? reinsEffDatePol,
            string cedingBenefitRiskCode = null,
            int? insuredAttainedAge = null,
            DateTime? reportDate = null,
            string treatyCode = null,
            string cedingTreatyCode = null,
            string campaignCode = null,
            int? reinsBasisCodeId = null,
            double? underwriterRating = null,
            double? oriSumAssured = null,
            int? reinsuranceIssueAge = null,
            bool groupById = false
        )
        {
            return TreatyBenefitCodeMappingDetail.CountByBenefitParams(
                cedantId,
                cedingPlanCode,
                cedingBenefitTypeCode,
                reinsEffDatePol,
                cedingBenefitRiskCode,
                insuredAttainedAge,
                reportDate,
                treatyCode,
                cedingTreatyCode,
                campaignCode,
                reinsBasisCodeId,
                underwriterRating,
                oriSumAssured,
                reinsuranceIssueAge,
                groupById
            );
        }

        public static int CountByBenefitParams(
            int cedantId,
            RiDataBo riData,
            CacheService pickListCache,
            DateTime? reportDate = null,
            bool groupById = false
        )
        {
            return CountByBenefitParams(
                cedantId,
                riData.CedingPlanCode,
                riData.CedingBenefitTypeCode,
                riData.ReinsEffDatePol,
                riData.CedingBenefitRiskCode,
                riData.InsuredAttainedAge,
                reportDate,
                riData.TreatyCode,
                riData.CedingTreatyCode,
                riData.CampaignCode,
                pickListCache.GetReinsBasisCodeId(riData),
                riData.UnderwriterRating,
                riData.OriSumAssured,
                riData.ReinsuranceIssueAge,
                groupById
            );
        }

        public static int CountCombinationForProductFeature(
            string combination,
            TreatyBenefitCodeMappingBo bo
        )
        {
            return CountCombinationForProductFeature(
                bo.CedantId,
                combination,
                bo.ReinsEffDatePolStartDate,
                bo.ReinsEffDatePolEndDate,
                bo.AttainedAgeFrom,
                bo.AttainedAgeTo,
                bo.TreatyCodeId,
                bo.BenefitId,
                bo.UnderwriterRatingFrom,
                bo.UnderwriterRatingTo,
                bo.OriSumAssuredFrom,
                bo.OriSumAssuredTo,
                bo.ReinsuranceIssueAgeFrom,
                bo.ReinsuranceIssueAgeTo,
                bo.ReportingStartDate,
                bo.ReportingEndDate,
                bo.Id
            );
        }

        public static int CountCombinationForProductFeature(
            int cedantId,
            string combination,
            DateTime? reinsEffDatePolStartDate,
            DateTime? ReinsEffDatePolEndDate,
            int? attainedAgeFrom,
            int? attainedAgeTo,
            int? treatyCodeId,
            int? benefitId,
            double? underwriterRatingFrom,
            double? underwriterRatingTo,
            double? oriSumAssuredFrom,
            double? oriSumAssuredTo,
            int? reinsuranceIssueAgeFrom,
            int? reinsuranceIssueAgeTo,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            int? treatyBenefitCodeMappingId = null
        )
        {
            return TreatyBenefitCodeMappingDetail.CountCombinationForProductFeature(
                cedantId,
                combination,
                reinsEffDatePolStartDate,
                ReinsEffDatePolEndDate,
                attainedAgeFrom,
                attainedAgeTo,
                treatyCodeId,
                benefitId,
                underwriterRatingFrom,
                underwriterRatingTo,
                oriSumAssuredFrom,
                oriSumAssuredTo,
                reinsuranceIssueAgeFrom,
                reinsuranceIssueAgeTo,
                reportingStartDate,
                reportingEndDate,
                treatyBenefitCodeMappingId
            );
        }

        public static int CountByProductFeatureParams(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            string treatyCode,
            string benefitCode,
            DateTime? reinsEffDatePol,
            int? insuredAttAge = null,
            string campaignCode = null,
            double? underwriterRating = null,
            double? oriSumAssured = null,
            int? reinsuranceIssueAge = null,
            DateTime? reportDate = null,
            string cedingBenefitRiskCode = null,
            string cedingTreatyCode = null,
            int? reinsBasisCodeId = null,
            bool groupById = false
        )
        {
            return TreatyBenefitCodeMappingDetail.CountByProductFeatureParams(
                cedantId,
                cedingPlanCode,
                cedingBenefitTypeCode,
                treatyCode,
                benefitCode,
                reinsEffDatePol,
                insuredAttAge,
                campaignCode,
                underwriterRating,
                oriSumAssured,
                reinsuranceIssueAge,
                reportDate,
                cedingBenefitRiskCode,
                cedingTreatyCode,
                reinsBasisCodeId,
                groupById
            );
        }

        public static int CountByProductFeatureParams(
            int cedantId,
            RiDataBo riData,
            CacheService pickListCache,
            DateTime? reportDate = null,
            bool groupById = false
        )
        {
            return CountByProductFeatureParams(
                cedantId,
                riData.CedingPlanCode,
                riData.CedingBenefitTypeCode,
                riData.TreatyCode,
                riData.MlreBenefitCode,
                riData.ReinsEffDatePol,
                riData.InsuredAttainedAge,
                riData.CampaignCode,
                riData.UnderwriterRating,
                riData.OriSumAssured,
                riData.ReinsuranceIssueAge,
                reportDate,
                riData.CedingBenefitRiskCode,
                riData.CedingTreatyCode,
                pickListCache.GetReinsBasisCodeId(riData),
                groupById
            );
        }

        public static IList<TreatyBenefitCodeMappingDetailBo> GetByTreatyParams(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            DateTime? reinsEffDatePol,
            string cedingBenefitRiskCode = null,
            string cedingTreatyCode = null,
            string campaignCode = null,
            int? reinsBasisCodeId = null,
            int? insuredAttAge = null,
            DateTime? reportDate = null,
            double? underwriterRating = null,
            double? oriSumAssured = null,
            int? reinsuranceIssueAge = null,
            bool groupById = false
        )
        {
            return FormBos(TreatyBenefitCodeMappingDetail.GetByTreatyParams(
                cedantId,
                cedingPlanCode,
                cedingBenefitTypeCode,
                reinsEffDatePol,
                cedingBenefitRiskCode,
                cedingTreatyCode,
                campaignCode,
                reinsBasisCodeId,
                insuredAttAge,
                reportDate,
                underwriterRating,
                oriSumAssured,
                reinsuranceIssueAge,
                groupById
            ));
        }

        public static IList<TreatyBenefitCodeMappingDetailBo> GetByTreatyParams(
            int cedantId,
            RiDataBo riData,
            CacheService pickListCache,
            DateTime? reportDate = null,
            bool groupById = false
        )
        {
            return GetByTreatyParams(
                cedantId,
                riData.CedingPlanCode,
                riData.CedingBenefitTypeCode,
                riData.ReinsEffDatePol,
                riData.CedingBenefitRiskCode,
                riData.CedingTreatyCode,
                riData.CampaignCode,
                pickListCache.GetReinsBasisCodeId(riData),
                riData.InsuredAttainedAge,
                reportDate,
                riData.UnderwriterRating,
                riData.OriSumAssured,
                riData.ReinsuranceIssueAge,
                groupById
            );
        }

        public static IList<TreatyBenefitCodeMappingDetailBo> GetByBenefitParams(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            DateTime? reinsEffDatePol,
            string cedingBenefitRiskCode = null,
            int? insuredAttainedAge = null,
            DateTime? reportDate = null,
            string treatyCode = null,
            string cedingTreatyCode = null,
            string campaignCode = null,
            int? reinsBasisCodeId = null,
            double? underwriterRating = null,
            double? oriSumAssured = null,
            int? reinsuranceIssueAge = null,
            bool groupById = false
        )
        {
            return FormBos(TreatyBenefitCodeMappingDetail.GetByBenefitParams(
                cedantId,
                cedingPlanCode,
                cedingBenefitTypeCode,
                reinsEffDatePol,
                cedingBenefitRiskCode,
                insuredAttainedAge,
                reportDate,
                treatyCode,
                cedingTreatyCode,
                campaignCode,
                reinsBasisCodeId,
                underwriterRating,
                oriSumAssured,
                reinsuranceIssueAge,
                groupById
            ));
        }

        public static IList<TreatyBenefitCodeMappingDetailBo> GetByBenefitParams(
            int cedantId,
            RiDataBo riData,
            CacheService pickListCache,
            DateTime? reportDate = null,
            bool groupById = false
        )
        {
            return GetByBenefitParams(
                cedantId,
                riData.CedingPlanCode,
                riData.CedingBenefitTypeCode,
                riData.ReinsEffDatePol,
                riData.CedingBenefitRiskCode,
                riData.InsuredAttainedAge,
                reportDate,
                riData.TreatyCode,
                riData.CedingTreatyCode,
                riData.CampaignCode,
                pickListCache.GetReinsBasisCodeId(riData),
                riData.UnderwriterRating,
                riData.OriSumAssured,
                riData.ReinsuranceIssueAge,
                groupById
            );
        }

        public static IList<TreatyBenefitCodeMappingDetailBo> GetByProductFeatureParams(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            string treatyCode,
            string benefitCode,
            DateTime? reinsEffDatePol,
            int? insuredAttAge = null,
            string campaignCode = null,
            double? underwriterRating = null,
            double? oriSumAssured = null,
            int? reinsuranceIssueAge = null,
            DateTime? reportDate = null,
            string cedingBenefitRiskCode = null,
            string cedingTreatyCode = null,
            int? reinsBasisCodeId = null,
            bool groupById = false
        )
        {
            return FormBos(TreatyBenefitCodeMappingDetail.GetByProductFeatureParams(
                cedantId,
                cedingPlanCode,
                cedingBenefitTypeCode,
                treatyCode,
                benefitCode,
                reinsEffDatePol,
                insuredAttAge,
                campaignCode,
                underwriterRating,
                oriSumAssured,
                reinsuranceIssueAge,
                reportDate,
                cedingBenefitRiskCode,
                cedingTreatyCode,
                reinsBasisCodeId,
                groupById
            ));
        }

        public static IList<TreatyBenefitCodeMappingDetailBo> GetByProductFeatureParams(
            int cedantId,
            RiDataBo riData,
            CacheService pickListCache,
            DateTime? reportDate = null,
            bool groupById = false
        )
        {
            return GetByProductFeatureParams(
                cedantId,
                riData.CedingPlanCode,
                riData.CedingBenefitTypeCode,
                riData.TreatyCode,
                riData.MlreBenefitCode,
                riData.ReinsEffDatePol,
                riData.InsuredAttainedAge,
                riData.CampaignCode,
                riData.UnderwriterRating,
                riData.OriSumAssured,
                riData.ReinsuranceIssueAge,
                reportDate,
                riData.CedingBenefitRiskCode,
                riData.CedingTreatyCode,
                pickListCache.GetReinsBasisCodeId(riData),
                groupById
            );
        }

        public static int CountDuplicateByParamsForTreaty(
            TreatyBenefitCodeMappingBo bo,
            TreatyBenefitCodeMappingDetailBo detailBo
        )
        {
            return CountDuplicateByParamsForTreaty(
                bo.CedantId,
                detailBo.CedingPlanCode,
                detailBo.CedingBenefitTypeCode,
                detailBo.CedingBenefitRiskCode,
                detailBo.CedingTreatyCode,
                detailBo.CampaignCode,
                bo.ReinsBasisCodePickListDetailId,
                bo.ReinsEffDatePolStartDate,
                bo.ReinsEffDatePolEndDate,
                bo.AttainedAgeFrom,
                bo.AttainedAgeTo,
                bo.ReportingStartDate,
                bo.ReportingEndDate,
                bo.UnderwriterRatingFrom,
                bo.UnderwriterRatingTo,
                bo.OriSumAssuredFrom,
                bo.OriSumAssuredTo,
                bo.ReinsuranceIssueAgeFrom,
                bo.ReinsuranceIssueAgeTo,
                bo.Id
            );
        }

        public static int CountDuplicateByParamsForTreaty(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            string cedingBenefitRiskCode,
            string cedingTreatyCode,
            string campaignCode,
            int? reinsBasisCodePickListDetailId,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            int? attAgeFrom,
            int? attAgeTo,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            double? underwriterRatingFrom,
            double? underwriterRatingTo,
            double? oriSumAssuredFrom,
            double? oriSumAssuredTo,
            int? reinsuranceIssueAgeFrom,
            int? reinsuranceIssueAgeTo,
            int? treatyBenefitCodeMappingId = null
        )
        {
            return TreatyBenefitCodeMappingDetail.CountDuplicateByParamsForTreaty(
                cedantId,
                cedingPlanCode,
                cedingBenefitTypeCode,
                cedingBenefitRiskCode,
                cedingTreatyCode,
                campaignCode,
                reinsBasisCodePickListDetailId,
                reinsEffDatePolStartDate,
                reinsEffDatePolEndDate,
                attAgeFrom,
                attAgeTo,
                reportingStartDate,
                reportingEndDate,
                underwriterRatingFrom,
                underwriterRatingTo,
                oriSumAssuredFrom,
                oriSumAssuredTo,
                reinsuranceIssueAgeFrom,
                reinsuranceIssueAgeTo,
                treatyBenefitCodeMappingId
            );
        }

        public static int CountDuplicateByParamsForBenefit(
            TreatyBenefitCodeMappingBo bo,
            TreatyBenefitCodeMappingDetailBo detailBo
        )
        {
            return CountDuplicateByParamsForBenefit(
                bo.CedantId,
                detailBo.CedingPlanCode,
                detailBo.CedingBenefitTypeCode,
                detailBo.CedingBenefitRiskCode,
                detailBo.CedingTreatyCode,
                detailBo.CampaignCode,
                bo.ReinsBasisCodePickListDetailId,
                bo.AttainedAgeFrom,
                bo.AttainedAgeTo,
                bo.ReportingStartDate,
                bo.ReportingEndDate,
                bo.TreatyCodeId,
                bo.ReinsEffDatePolStartDate,
                bo.ReinsEffDatePolEndDate,
                bo.UnderwriterRatingFrom,
                bo.UnderwriterRatingTo,
                bo.OriSumAssuredFrom,
                bo.OriSumAssuredTo,
                bo.ReinsuranceIssueAgeFrom,
                bo.ReinsuranceIssueAgeTo,
                bo.Id
            );
        }

        public static int CountDuplicateByParamsForBenefit(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            string cedingBenefitRiskCode,
            string cedingTreatyCode,
            string campaignCode,
            int? reinsBasisCodePickListDetailId,
            int? attainedAgeFrom,
            int? attainedAgeTo,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            int? treatyCodeId,
            DateTime? reinsEffDatePolStartDate,
            DateTime? reinsEffDatePolEndDate,
            double? underwriterRatingFrom,
            double? underwriterRatingTo,
            double? oriSumAssuredFrom,
            double? oriSumAssuredTo,
            int? reinsuranceIssueAgeFrom,
            int? reinsuranceIssueAgeTo,
            int? treatyBenefitCodeMappingId = null
        )
        {
            return TreatyBenefitCodeMappingDetail.CountDuplicateByParamsForBenefit(
                cedantId,
                cedingPlanCode,
                cedingBenefitTypeCode,
                cedingBenefitRiskCode,
                cedingTreatyCode,
                campaignCode,
                reinsBasisCodePickListDetailId,
                attainedAgeFrom,
                attainedAgeTo,
                reportingStartDate,
                reportingEndDate,
                treatyCodeId,
                reinsEffDatePolStartDate,
                reinsEffDatePolEndDate,
                underwriterRatingFrom,
                underwriterRatingTo,
                oriSumAssuredFrom,
                oriSumAssuredTo,
                reinsuranceIssueAgeFrom,
                reinsuranceIssueAgeTo,
                treatyBenefitCodeMappingId
            );
        }

        public static int CountDuplicateByParamsForProductFeature(
            TreatyBenefitCodeMappingBo bo,
            TreatyBenefitCodeMappingDetailBo detailBo
        )
        {
            return CountDuplicateByParamsForProductFeature(
                bo.CedantId,
                detailBo.CedingPlanCode,
                detailBo.CedingBenefitTypeCode,
                detailBo.CedingBenefitRiskCode,
                detailBo.CedingTreatyCode,
                detailBo.CampaignCode,
                bo.ReinsBasisCodePickListDetailId,
                bo.ReinsEffDatePolStartDate,
                bo.ReinsEffDatePolEndDate,
                bo.AttainedAgeFrom,
                bo.AttainedAgeTo,
                bo.TreatyCodeId,
                bo.BenefitId,
                bo.UnderwriterRatingFrom,
                bo.UnderwriterRatingTo,
                bo.OriSumAssuredFrom,
                bo.OriSumAssuredTo,
                bo.ReinsuranceIssueAgeFrom,
                bo.ReinsuranceIssueAgeTo,
                bo.ReportingStartDate,
                bo.ReportingEndDate,
                bo.Id
            );
        }

        public static int CountDuplicateByParamsForProductFeature(
            int cedantId,
            string cedingPlanCode,
            string cedingBenefitTypeCode,
            string cedingBenefitRiskCode,
            string cedingTreatyCode,
            string campaignCode,
            int? reinsBasisCodePickListDetailId,
            DateTime? reinsEffDatePolStartDate,
            DateTime? ReinsEffDatePolEndDate,
            int? attainedAgeFrom,
            int? attainedAgeTo,
            int? treatyCodeId,
            int? benefitId,
            double? underwriterRatingFrom,
            double? underwriterRatingTo,
            double? oriSumAssuredFrom,
            double? oriSumAssuredTo,
            int? reinsuranceIssueAgeFrom,
            int? reinsuranceIssueAgeTo,
            DateTime? reportingStartDate,
            DateTime? reportingEndDate,
            int? treatyBenefitCodeMappingId = null
        )
        {
            return TreatyBenefitCodeMappingDetail.CountDuplicateByParamsForProductFeature(
                cedantId,
                cedingPlanCode,
                cedingBenefitTypeCode,
                cedingBenefitRiskCode,
                cedingTreatyCode,
                campaignCode,
                reinsBasisCodePickListDetailId,
                reinsEffDatePolStartDate,
                ReinsEffDatePolEndDate,
                attainedAgeFrom,
                attainedAgeTo,
                treatyCodeId,
                benefitId,
                underwriterRatingFrom,
                underwriterRatingTo,
                oriSumAssuredFrom,
                oriSumAssuredTo,
                reinsuranceIssueAgeFrom,
                reinsuranceIssueAgeTo,
                reportingStartDate,
                reportingEndDate,
                treatyBenefitCodeMappingId
            );
        }

        public static Result Save(ref TreatyBenefitCodeMappingDetailBo bo)
        {
            if (!TreatyBenefitCodeMappingDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyBenefitCodeMappingDetailBo bo, ref TrailObject trail)
        {
            if (!TreatyBenefitCodeMappingDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyBenefitCodeMappingDetailBo bo)
        {
            TreatyBenefitCodeMappingDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyBenefitCodeMappingDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyBenefitCodeMappingDetailBo bo)
        {
            Result result = Result();

            TreatyBenefitCodeMappingDetail entity = TreatyBenefitCodeMappingDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Type = bo.Type;
                entity.TreatyBenefitCodeMappingId = bo.TreatyBenefitCodeMappingId;
                entity.Combination = bo.Combination;
                entity.CedingPlanCode = bo.CedingPlanCode;
                entity.CedingBenefitTypeCode = bo.CedingBenefitTypeCode;
                entity.CedingBenefitRiskCode = bo.CedingBenefitRiskCode;
                entity.CedingTreatyCode = bo.CedingTreatyCode;
                entity.CampaignCode = bo.CampaignCode;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyBenefitCodeMappingDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyBenefitCodeMappingDetailBo bo)
        {
            TreatyBenefitCodeMappingDetail.Delete(bo.Id);
        }

        public static Result Delete(TreatyBenefitCodeMappingDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = TreatyBenefitCodeMappingDetail.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyBenefitCodeMappingId(int treatyBenefitCodeMappingId)
        {
            return TreatyBenefitCodeMappingDetail.DeleteAllByTreatyBenefitCodeMappingId(treatyBenefitCodeMappingId);
        }

        public static void DeleteAllByTreatyBenefitCodeMappingId(int treatyBenefitCodeMappingId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyBenefitCodeMappingId(treatyBenefitCodeMappingId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyBenefitCodeMappingDetail)));
                }
            }
        }
    }
}
