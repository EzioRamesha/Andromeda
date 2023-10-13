using BusinessObject;
using DataAccess.Entities;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace Services
{
    public class TreatyBenefitCodeMappingService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyBenefitCodeMapping)),
                Controller = ModuleBo.ModuleController.ProductFeatureMapping.ToString()
            };
        }

        public static Expression<Func<TreatyBenefitCodeMapping, TreatyBenefitCodeMappingBo>> Expression()
        {
            return entity => new TreatyBenefitCodeMappingBo
            {
                Id = entity.Id,

                TreatyBenefitCodeMappingUploadId = entity.TreatyBenefitCodeMappingUploadId,

                CedantId = entity.CedantId,
                CedantCode = entity.Cedant.Code,

                BenefitId = entity.BenefitId,
                BenefitCode = entity.Benefit.Code,

                TreatyCodeId = entity.TreatyCodeId,
                TreatyCode = entity.TreatyCode.Code,

                CedingPlanCode = entity.CedingPlanCode,
                Description = entity.Description,
                CedingBenefitTypeCode = entity.CedingBenefitTypeCode,
                CedingBenefitRiskCode = entity.CedingBenefitRiskCode,
                CedingTreatyCode = entity.CedingTreatyCode,
                CampaignCode = entity.CampaignCode,

                ReinsEffDatePolStartDate = entity.ReinsEffDatePolStartDate,
                ReinsEffDatePolEndDate = entity.ReinsEffDatePolEndDate,

                ReinsBasisCodePickListDetailId = entity.ReinsBasisCodePickListDetailId,
                ReinsBasisCode = entity.ReinsBasisCodePickListDetail.Code,

                AttainedAgeFrom = entity.AttainedAgeFrom,
                AttainedAgeTo = entity.AttainedAgeTo,

                ReportingStartDate = entity.ReportingStartDate,
                ReportingEndDate = entity.ReportingEndDate,

                // Phase 2
                ProfitCommPickListDetailId = entity.ProfitCommPickListDetailId,
                ProfitComm = entity.ProfitCommPickListDetail.Code,
                MaxExpiryAge = entity.MaxExpiryAge,
                MinIssueAge = entity.MinIssueAge,
                MaxIssueAge = entity.MaxIssueAge,
                MaxUwRating = entity.MaxUwRating,
                ApLoading = entity.ApLoading,
                MinAar = entity.MinAar,
                MaxAar = entity.MaxAar,
                AblAmount = entity.AblAmount,
                RetentionShare = entity.RetentionShare,
                RetentionCap = entity.RetentionCap,
                RiShare = entity.RiShare,
                RiShareCap = entity.RiShareCap,
                ServiceFee = entity.ServiceFee,
                WakalahFee = entity.WakalahFee,
                UnderwriterRatingFrom = entity.UnderwriterRatingFrom,
                UnderwriterRatingTo = entity.UnderwriterRatingTo,
                RiShare2 = entity.RiShare2,
                RiShareCap2 = entity.RiShareCap2,

                OriSumAssuredFrom = entity.OriSumAssuredFrom,
                OriSumAssuredTo = entity.OriSumAssuredTo,

                EffectiveDate = entity.EffectiveDate,

                ReinsuranceIssueAgeFrom = entity.ReinsuranceIssueAgeFrom,
                ReinsuranceIssueAgeTo = entity.ReinsuranceIssueAgeTo,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyBenefitCodeMappingBo FormBo(TreatyBenefitCodeMapping entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyBenefitCodeMappingBo
            {
                Id = entity.Id,
                TreatyBenefitCodeMappingUploadId = entity.TreatyBenefitCodeMappingUploadId,
                TreatyBenefitCodeMappingUploadBo = TreatyBenefitCodeMappingUploadService.Find(entity.TreatyBenefitCodeMappingUploadId),

                CedantId = entity.CedantId,
                CedantBo = CedantService.Find(entity.CedantId),

                BenefitId = entity.BenefitId,
                BenefitBo = BenefitService.Find(entity.BenefitId),

                TreatyCodeId = entity.TreatyCodeId,
                TreatyCodeBo = TreatyCodeService.Find(entity.TreatyCodeId),

                CedingPlanCode = entity.CedingPlanCode,
                Description = entity.Description,
                CedingBenefitTypeCode = entity.CedingBenefitTypeCode,
                CedingBenefitRiskCode = entity.CedingBenefitRiskCode,
                CedingTreatyCode = entity.CedingTreatyCode,
                CampaignCode = entity.CampaignCode,

                ReinsEffDatePolStartDate = entity.ReinsEffDatePolStartDate,
                ReinsEffDatePolEndDate = entity.ReinsEffDatePolEndDate,

                ReinsBasisCodePickListDetailId = entity.ReinsBasisCodePickListDetailId,
                ReinsBasisCodePickListDetailBo = PickListDetailService.Find(entity.ReinsBasisCodePickListDetailId),

                AttainedAgeFrom = entity.AttainedAgeFrom,
                AttainedAgeTo = entity.AttainedAgeTo,

                ReportingStartDate = entity.ReportingStartDate,
                ReportingEndDate = entity.ReportingEndDate,

                // Phase 2
                //ProfitComm = entity.ProfitComm,
                ProfitCommPickListDetailId = entity.ProfitCommPickListDetailId,
                ProfitCommPickListDetailBo = PickListDetailService.Find(entity.ProfitCommPickListDetailId),
                MaxExpiryAge = entity.MaxExpiryAge,
                MinIssueAge = entity.MinIssueAge,
                MaxIssueAge = entity.MaxIssueAge,
                MaxUwRating = entity.MaxUwRating,
                ApLoading = entity.ApLoading,
                MinAar = entity.MinAar,
                MaxAar = entity.MaxAar,
                AblAmount = entity.AblAmount,
                RetentionShare = entity.RetentionShare,
                RetentionCap = entity.RetentionCap,
                RiShare = entity.RiShare,
                RiShareCap = entity.RiShareCap,
                ServiceFee = entity.ServiceFee,
                WakalahFee = entity.WakalahFee,
                UnderwriterRatingFrom = entity.UnderwriterRatingFrom,
                UnderwriterRatingTo = entity.UnderwriterRatingTo,
                RiShare2 = entity.RiShare2,
                RiShareCap2 = entity.RiShareCap2,
                
                OriSumAssuredFrom = entity.OriSumAssuredFrom,
                OriSumAssuredTo = entity.OriSumAssuredTo,

                EffectiveDate = entity.EffectiveDate,

                ReinsuranceIssueAgeFrom = entity.ReinsuranceIssueAgeFrom,
                ReinsuranceIssueAgeTo = entity.ReinsuranceIssueAgeTo,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<TreatyBenefitCodeMappingBo> FormBos(IList<TreatyBenefitCodeMapping> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyBenefitCodeMappingBo> bos = new List<TreatyBenefitCodeMappingBo>() { };
            foreach (TreatyBenefitCodeMapping entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyBenefitCodeMapping FormEntity(TreatyBenefitCodeMappingBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyBenefitCodeMapping
            {
                Id = bo.Id,

                CedantId = bo.CedantId,
                TreatyBenefitCodeMappingUploadId = bo.TreatyBenefitCodeMappingUploadId,
                BenefitId = bo.BenefitId,
                TreatyCodeId = bo.TreatyCodeId,

                CedingPlanCode = bo.CedingPlanCode,
                Description = bo.Description?.Trim(),
                CedingBenefitTypeCode = bo.CedingBenefitTypeCode,
                CedingBenefitRiskCode = bo.CedingBenefitRiskCode,
                CedingTreatyCode = bo.CedingTreatyCode,
                CampaignCode = bo.CampaignCode,

                ReinsEffDatePolStartDate = bo.ReinsEffDatePolStartDate,
                ReinsEffDatePolEndDate = bo.ReinsEffDatePolEndDate,

                ReinsBasisCodePickListDetailId = bo.ReinsBasisCodePickListDetailId,

                AttainedAgeFrom = bo.AttainedAgeFrom,
                AttainedAgeTo = bo.AttainedAgeTo,

                ReportingStartDate = bo.ReportingStartDate,
                ReportingEndDate = bo.ReportingEndDate,

                // Phase 2
                //ProfitComm = bo.ProfitComm,
                ProfitCommPickListDetailId = bo.ProfitCommPickListDetailId,
                MaxExpiryAge = bo.MaxExpiryAge,
                MinIssueAge = bo.MinIssueAge,
                MaxIssueAge = bo.MaxIssueAge,
                MaxUwRating = bo.MaxUwRating,
                ApLoading = bo.ApLoading,
                MinAar = bo.MinAar,
                MaxAar = bo.MaxAar,
                AblAmount = bo.AblAmount,
                RetentionShare = bo.RetentionShare,
                RetentionCap = bo.RetentionCap,
                RiShare = bo.RiShare,
                RiShareCap = bo.RiShareCap,
                ServiceFee = bo.ServiceFee,
                WakalahFee = bo.WakalahFee,
                UnderwriterRatingFrom = bo.UnderwriterRatingFrom,
                UnderwriterRatingTo = bo.UnderwriterRatingTo,
                RiShare2 = bo.RiShare2,
                RiShareCap2 = bo.RiShareCap2,

                OriSumAssuredFrom = bo.OriSumAssuredFrom,
                OriSumAssuredTo = bo.OriSumAssuredTo,

                EffectiveDate = bo.EffectiveDate,

                ReinsuranceIssueAgeFrom = bo.ReinsuranceIssueAgeFrom,
                ReinsuranceIssueAgeTo = bo.ReinsuranceIssueAgeTo,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyBenefitCodeMapping.IsExists(id);
        }

        public static TreatyBenefitCodeMappingBo Find(int id)
        {
            return FormBo(TreatyBenefitCodeMapping.Find(id));
        }

        public static int CountByTreatyCodeId(int treatyCodeId)
        {
            return TreatyBenefitCodeMapping.CountByTreatyCodeId(treatyCodeId);
        }

        public static int CountByCedingBenefitTypeCode(string cedingBenefitTypeCode)
        {
            return TreatyBenefitCodeMapping.CountByCedingBenefitTypeCode(cedingBenefitTypeCode);
        }

        public static int CountByReinsBasisCodePickListDetailId(int reinsBasisCodePickListDetailId)
        {
            return TreatyBenefitCodeMapping.CountByReinsBasisCodePickListDetailId(reinsBasisCodePickListDetailId);
        }

        public static IList<TreatyBenefitCodeMappingBo> GetByCedantId(int cedantId)
        {
            return FormBos(TreatyBenefitCodeMapping.GetByCedantId(cedantId));
        }

        public static Result ValidateRange(TreatyBenefitCodeMappingBo bo)
        {
            Result result = new Result();

            if (bo.ReinsEffDatePolEndDate != null && bo.ReinsEffDatePolStartDate == null)
            {
                result.AddError(string.Format(MessageBag.Required, "The Policy Reinsurance Start Date Field"));
            }
            else if (bo.ReinsEffDatePolStartDate != null && bo.ReinsEffDatePolEndDate != null)
            {
                if (bo.ReinsEffDatePolEndDate <= bo.ReinsEffDatePolStartDate)
                {
                    result.AddError(string.Format(MessageBag.EndDateLater, "Policy Reinsurance"));
                }
            }

            if (bo.AttainedAgeTo != null && bo.AttainedAgeFrom == null)
            {
                result.AddError(string.Format(MessageBag.Required, "The Attained Age From Field"));
            }
            else if (bo.AttainedAgeFrom != null && bo.AttainedAgeTo == null)
            {
                result.AddError(string.Format(MessageBag.Required, "The Attained Age To Field"));
            }
            else if (bo.AttainedAgeTo != null && bo.AttainedAgeFrom != null && bo.AttainedAgeTo < bo.AttainedAgeFrom)
            {
                result.AddError(string.Format(MessageBag.GreaterOrEqualTo, "Attained Age To", "Attained Age From"));
            }

            if (bo.ReportingEndDate != null && bo.ReportingStartDate == null)
            {
                result.AddError(string.Format(MessageBag.Required, "The Reporting Start Date Field"));
            }
            else if (bo.ReportingStartDate != null && bo.ReportingEndDate == null)
            {
                if (DateTime.Parse(Util.GetDefaultEndDate()) <= bo.ReportingStartDate)
                {
                    result.AddError(string.Format(MessageBag.EndDefaultDate, "Reporting", Util.GetDefaultEndDate()));
                }
            }
            else if (bo.ReportingStartDate != null && bo.ReportingEndDate != null)
            {
                if (bo.ReportingEndDate <= bo.ReportingStartDate)
                {
                    result.AddError(string.Format(MessageBag.EndDateLater, "Reporting"));
                }
            }

            if (bo.MaxIssueAge != null && bo.MinIssueAge != null && bo.MaxIssueAge < bo.MinIssueAge)
            {
                result.AddError(string.Format(MessageBag.GreaterOrEqualTo, "Maximum Issue Age ", "Minimum Issue Age"));
            }

            if (bo.MaxAar != null && bo.MinAar != null && bo.MaxAar < bo.MinAar)
            {
                result.AddError(string.Format(MessageBag.GreaterOrEqualTo, "Maximum AAR", "Minimum AAR"));
            }

            if (bo.UnderwriterRatingTo != null && bo.UnderwriterRatingFrom == null)
            {
                result.AddError(string.Format(MessageBag.Required, "The Underwriter Rating From Field"));
            }
            else if (bo.UnderwriterRatingFrom != null && bo.UnderwriterRatingTo == null)
            {
                result.AddError(string.Format(MessageBag.Required, "The Underwriter Rating To Field"));
            }
            else if (bo.UnderwriterRatingTo != null && bo.UnderwriterRatingFrom != null && bo.UnderwriterRatingTo < bo.UnderwriterRatingFrom)
            {
                result.AddError(string.Format(MessageBag.GreaterOrEqualTo, "Underwriter Rating To", "Underwriter Rating From"));
            }

            if (bo.OriSumAssuredTo != null && bo.OriSumAssuredFrom == null)
            {
                result.AddError(string.Format(MessageBag.Required, "The Ori Sum Assured From Field"));
            }
            else if (bo.OriSumAssuredFrom != null && bo.OriSumAssuredTo == null)
            {
                result.AddError(string.Format(MessageBag.Required, "The Ori Sum Assured To Field"));
            }
            else if (bo.OriSumAssuredTo != null && bo.OriSumAssuredFrom != null && bo.OriSumAssuredTo < bo.OriSumAssuredFrom)
            {
                result.AddError(string.Format(MessageBag.GreaterOrEqualTo, "Ori Sum Assured To", "Ori Sum Assured From"));
            }

            if (bo.ReinsuranceIssueAgeTo != null && bo.ReinsuranceIssueAgeFrom == null)
            {
                result.AddError(string.Format(MessageBag.Required, "The Reinsurance Issue Age From Field"));
            }
            else if (bo.ReinsuranceIssueAgeFrom != null && bo.ReinsuranceIssueAgeTo == null)
            {
                result.AddError(string.Format(MessageBag.Required, "The Reinsurance Issue Age To Field"));
            }
            else if (bo.ReinsuranceIssueAgeTo != null && bo.ReinsuranceIssueAgeFrom != null && bo.ReinsuranceIssueAgeTo < bo.ReinsuranceIssueAgeFrom)
            {
                result.AddError(string.Format(MessageBag.GreaterOrEqualTo, "Reinsurance Issue Age To", "Reinsurance Issue Age From"));
            }

            return result;
        }

        public static Result Save(ref TreatyBenefitCodeMappingBo bo)
        {
            if (!TreatyBenefitCodeMapping.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyBenefitCodeMappingBo bo, ref TrailObject trail)
        {
            if (!TreatyBenefitCodeMapping.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyBenefitCodeMappingBo bo)
        {
            TreatyBenefitCodeMapping entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyBenefitCodeMappingBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyBenefitCodeMappingBo bo)
        {
            Result result = Result();

            TreatyBenefitCodeMapping entity = TreatyBenefitCodeMapping.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.TreatyBenefitCodeMappingUploadId = bo.TreatyBenefitCodeMappingUploadId;
                entity.CedantId = bo.CedantId;
                entity.BenefitId = bo.BenefitId;
                entity.TreatyCodeId = bo.TreatyCodeId;

                entity.CedingPlanCode = bo.CedingPlanCode;
                entity.Description = bo.Description;
                entity.CedingBenefitTypeCode = bo.CedingBenefitTypeCode;
                entity.CedingBenefitRiskCode = bo.CedingBenefitRiskCode;
                entity.CedingTreatyCode = bo.CedingTreatyCode;
                entity.CampaignCode = bo.CampaignCode;

                entity.ReinsEffDatePolStartDate = bo.ReinsEffDatePolStartDate;
                entity.ReinsEffDatePolEndDate = bo.ReinsEffDatePolEndDate;

                entity.ReinsBasisCodePickListDetailId = bo.ReinsBasisCodePickListDetailId;

                entity.AttainedAgeFrom = bo.AttainedAgeFrom;
                entity.AttainedAgeTo = bo.AttainedAgeTo;

                entity.ReportingStartDate = bo.ReportingStartDate;
                entity.ReportingEndDate = bo.ReportingEndDate;

                // Phase 2
                //entity.ProfitComm = bo.ProfitComm;
                entity.ProfitCommPickListDetailId = bo.ProfitCommPickListDetailId;
                entity.MaxExpiryAge = bo.MaxExpiryAge;
                entity.MinIssueAge = bo.MinIssueAge;
                entity.MaxIssueAge = bo.MaxIssueAge;
                entity.MaxUwRating = bo.MaxUwRating;
                entity.ApLoading = bo.ApLoading;
                entity.MinAar = bo.MinAar;
                entity.MaxAar = bo.MaxAar;
                entity.AblAmount = bo.AblAmount;
                entity.RetentionShare = bo.RetentionShare;
                entity.RetentionCap = bo.RetentionCap;
                entity.RiShare = bo.RiShare;
                entity.RiShareCap = bo.RiShareCap;
                entity.ServiceFee = bo.ServiceFee;
                entity.WakalahFee = bo.WakalahFee;
                entity.UnderwriterRatingFrom = bo.UnderwriterRatingFrom;
                entity.UnderwriterRatingTo = bo.UnderwriterRatingTo;
                entity.RiShare2 = bo.RiShare2;
                entity.RiShareCap2 = bo.RiShareCap2;

                entity.OriSumAssuredFrom = bo.OriSumAssuredFrom;
                entity.OriSumAssuredTo = bo.OriSumAssuredTo;

                entity.EffectiveDate = bo.EffectiveDate;

                entity.ReinsuranceIssueAgeFrom = bo.ReinsuranceIssueAgeFrom;
                entity.ReinsuranceIssueAgeTo = bo.ReinsuranceIssueAgeTo;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyBenefitCodeMappingBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyBenefitCodeMappingBo bo)
        {
            TreatyBenefitCodeMappingDetailService.DeleteAllByTreatyBenefitCodeMappingId(bo.Id);
            TreatyBenefitCodeMapping.Delete(bo.Id);
        }

        public static Result Delete(TreatyBenefitCodeMappingBo bo, ref TrailObject trail)
        {
            Result result = Result();

            TreatyBenefitCodeMappingDetailService.DeleteAllByTreatyBenefitCodeMappingId(bo.Id); // DO NOT TRAIL
            DataTrail dataTrail = TreatyBenefitCodeMapping.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static Result ValidateMapping(TreatyBenefitCodeMappingBo bo)
        {
            Result result = new Result();
            var details = CreateDetails(bo);
            var treaty = details.Where(q => q.Type == TreatyBenefitCodeMappingDetailBo.CombinationTypeTreaty).ToList();
            var benefit = details.Where(q => q.Type == TreatyBenefitCodeMappingDetailBo.CombinationTypeBenefit).ToList();
            var feature = details.Where(q => q.Type == TreatyBenefitCodeMappingDetailBo.CombinationTypeProductFeature).ToList();

            var list = new Dictionary<string, List<string>> { };

            foreach (var detail in details)
            {
                var d = detail;
                TrimMaxLength(ref d, ref list);
                if (list.Count > 0)
                {
                    foreach (var prop in list)
                    {
                        result.AddError(string.Format("Exceeded Max Length: {0}", prop.Key));
                    }
                    break;
                }
            }

            foreach (var detail in treaty)
            {
                //var count = TreatyBenefitCodeMappingDetailService.CountCombinationForTreaty(
                //    detail.Combination,
                //    bo
                //);
                var count = TreatyBenefitCodeMappingDetailService.CountDuplicateByParamsForTreaty(
                    bo,
                    detail
                );
                if (count > 0)
                {
                    result.AddError("Existing Treaty Mapping Combination Found");
                    break;
                }
            }
            foreach (var detail in benefit)
            {
                //var count = TreatyBenefitCodeMappingDetailService.CountCombinationForBenefit(
                //    detail.Combination,
                //    bo
                //);
                var count = TreatyBenefitCodeMappingDetailService.CountDuplicateByParamsForBenefit(
                    bo,
                    detail
                );
                if (count > 0)
                {
                    result.AddError("Existing Benefit Mapping Combination Found");
                    break;
                }
            }
            foreach (var detail in feature)
            {
                //var count = TreatyBenefitCodeMappingDetailService.CountCombinationForProductFeature(
                //    detail.Combination,
                //    bo
                //);
                var count = TreatyBenefitCodeMappingDetailService.CountDuplicateByParamsForProductFeature(
                    bo,
                    detail
                );
                if (count > 0)
                {
                    result.AddError("Existing Feature Mapping Combination Found");
                    break;
                }
            }
            return result;
        }

        public static IList<TreatyBenefitCodeMappingDetailBo> CreateDetails(TreatyBenefitCodeMappingBo bo, int createdById = 0)
        {
            var details = new List<TreatyBenefitCodeMappingDetailBo> { };

            // Treaty Code Mapping
            CartesianProduct<string> treadyCodeMappings = new CartesianProduct<string>(
               bo.CedingPlanCode.ToArraySplitTrim(),
               bo.CedingBenefitTypeCode.ToArraySplitTrim(),
               bo.CedingBenefitRiskCode.ToArraySplitTrim(),
               bo.CedingTreatyCode.ToArraySplitTrim(),
               bo.CampaignCode.ToArraySplitTrim()
            );
            foreach (var item in treadyCodeMappings.Get())
            {
                var cedingPlanCode = item[0];
                var cedingBenefitTypeCode = item[1];
                var cedingBenefitRiskCode = item[2];
                var cedingTreatyCode = item[3];
                var campaignCode = item[4];
                var items = new List<string>
                {
                    cedingPlanCode,
                    cedingBenefitTypeCode,
                    cedingBenefitRiskCode,
                    cedingTreatyCode,
                    campaignCode,
                    bo.ReinsBasisCodePickListDetailBo != null ? bo.ReinsBasisCodePickListDetailBo.Code : "",
                };
                details.Add(new TreatyBenefitCodeMappingDetailBo
                {
                    Type = TreatyBenefitCodeMappingDetailBo.CombinationTypeTreaty,
                    TreatyBenefitCodeMappingId = bo.Id,
                    Combination = string.Join("|", items),
                    CreatedById = createdById,
                    CreatedAt = bo.CreatedAt,

                    CedingPlanCode = string.IsNullOrEmpty(cedingPlanCode) ? null : cedingPlanCode,
                    CedingBenefitTypeCode = string.IsNullOrEmpty(cedingBenefitTypeCode) ? null : cedingBenefitTypeCode,
                    CedingBenefitRiskCode = string.IsNullOrEmpty(cedingBenefitRiskCode) ? null : cedingBenefitRiskCode,
                    CedingTreatyCode = string.IsNullOrEmpty(cedingTreatyCode) ? null : cedingTreatyCode,
                    CampaignCode = string.IsNullOrEmpty(campaignCode) ? null : campaignCode,
                });
            }

            // Benefit Code Mapping
            CartesianProduct<string> benefitCodeMappings = new CartesianProduct<string>(
                bo.CedingPlanCode.ToArraySplitTrim(),
                bo.CedingBenefitTypeCode.ToArraySplitTrim(),
                bo.CedingBenefitRiskCode.ToArraySplitTrim(),
                bo.CedingTreatyCode.ToArraySplitTrim(),
                bo.CampaignCode.ToArraySplitTrim()
            );
            foreach (var item in benefitCodeMappings.Get())
            {
                var cedingPlanCode = item[0];
                var cedingBenefitTypeCode = item[1];
                var cedingBenefitRiskCode = item[2];
                var cedingTreatyCode = item[3];
                var campaignCode = item[4];
                var items = new List<string>
                {
                    cedingPlanCode,
                    cedingBenefitTypeCode,
                    cedingBenefitRiskCode,
                    cedingTreatyCode,
                    campaignCode,
                    bo.ReinsBasisCodePickListDetailBo != null ? bo.ReinsBasisCodePickListDetailBo.Code : "",
                };
                details.Add(new TreatyBenefitCodeMappingDetailBo
                {
                    Type = TreatyBenefitCodeMappingDetailBo.CombinationTypeBenefit,
                    TreatyBenefitCodeMappingId = bo.Id,
                    Combination = string.Join("|", items),
                    CreatedById = createdById,
                    CreatedAt = bo.CreatedAt,

                    CedingPlanCode = string.IsNullOrEmpty(cedingPlanCode) ? null : cedingPlanCode,
                    CedingBenefitTypeCode = string.IsNullOrEmpty(cedingBenefitTypeCode) ? null : cedingBenefitTypeCode,
                    CedingBenefitRiskCode = string.IsNullOrEmpty(cedingBenefitRiskCode) ? null : cedingBenefitRiskCode,
                    CedingTreatyCode = string.IsNullOrEmpty(cedingTreatyCode) ? null : cedingTreatyCode,
                    CampaignCode = string.IsNullOrEmpty(campaignCode) ? null : campaignCode,
                });
            }

            // Features - Phase 2
            CartesianProduct<string> featureMappings = new CartesianProduct<string>(
                bo.CedingPlanCode.ToArraySplitTrim(),
                bo.CedingBenefitTypeCode.ToArraySplitTrim(),
                bo.CedingBenefitRiskCode.ToArraySplitTrim(),
                bo.CedingTreatyCode.ToArraySplitTrim(),
                bo.CampaignCode.ToArraySplitTrim()
            );
            foreach (var item in featureMappings.Get())
            {
                // Initial combination
                //cedingPlanCode
                //campaignCode
                var cedingPlanCode = item[0];
                var cedingBenefitTypeCode = item[1];
                var cedingBenefitRiskCode = item[2];
                var cedingTreatyCode = item[3];
                var campaignCode = item[4];
                var items = new List<string>
                {
                    cedingPlanCode,
                    cedingBenefitTypeCode,
                    cedingBenefitRiskCode,
                    cedingTreatyCode,
                    campaignCode,
                    bo.ReinsBasisCodePickListDetailBo != null ? bo.ReinsBasisCodePickListDetailBo.Code : "",
                };
                details.Add(new TreatyBenefitCodeMappingDetailBo
                {
                    Type = TreatyBenefitCodeMappingDetailBo.CombinationTypeProductFeature,
                    TreatyBenefitCodeMappingId = bo.Id,
                    Combination = string.Join("|", items),
                    CreatedById = createdById,
                    CreatedAt = bo.CreatedAt,

                    CedingPlanCode = string.IsNullOrEmpty(cedingPlanCode) ? null : cedingPlanCode,
                    CedingBenefitTypeCode = string.IsNullOrEmpty(cedingBenefitTypeCode) ? null : cedingBenefitTypeCode,
                    CedingBenefitRiskCode = string.IsNullOrEmpty(cedingBenefitRiskCode) ? null : cedingBenefitRiskCode,
                    CedingTreatyCode = string.IsNullOrEmpty(cedingTreatyCode) ? null : cedingTreatyCode,
                    CampaignCode = string.IsNullOrEmpty(campaignCode) ? null : campaignCode,
                });
            }

            return details;
        }

        public static void TrimMaxLength(ref TreatyBenefitCodeMappingDetailBo detailBo, ref Dictionary<string, List<string>> list)
        {
            var entity = new TreatyBenefitCodeMappingDetail();
            foreach (var property in (typeof(TreatyBenefitCodeMappingDetailBo)).GetProperties())
            {
                var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>(property.Name);
                if (maxLengthAttr != null)
                {
                    var value = property.GetValue(detailBo, null);
                    if (value != null && value is string @string && !string.IsNullOrEmpty(@string))
                    {
                        if (@string.Length > maxLengthAttr.Length)
                        {
                            string propName = string.Format("{0}({1})", property.Name, maxLengthAttr.Length);

                            if (!list.ContainsKey(propName))
                                list.Add(propName, new List<string> { });

                            var oldValue = @string;
                            var newValue = @string.Substring(0, maxLengthAttr.Length);
                            var formatValue = string.Format("{0}|{1}", oldValue, newValue);

                            if (!list[propName].Contains(formatValue))
                                list[propName].Add(formatValue);

                            property.SetValue(detailBo, newValue);
                        }
                    }
                }
            }
        }

        public static void ProcessMappingDetail(TreatyBenefitCodeMappingBo bo, int createdById)
        {
            TreatyBenefitCodeMappingDetailService.DeleteAllByTreatyBenefitCodeMappingId(bo.Id);
            foreach (var detail in CreateDetails(bo, createdById))
            {
                var d = detail;
                TreatyBenefitCodeMappingDetailService.Create(ref d);
            }
        }

        public static void ProcessMappingDetail(TreatyBenefitCodeMappingBo bo, int createdById, ref TrailObject trail)
        {
            TreatyBenefitCodeMappingDetailService.DeleteAllByTreatyBenefitCodeMappingId(bo.Id);
            foreach (var detail in CreateDetails(bo, createdById))
            {
                var d = detail;
                TreatyBenefitCodeMappingDetailService.Create(ref d, ref trail);
            }
        }
    }
}
