using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
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
    public class RateTableService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RateTable)),
                Controller = ModuleBo.ModuleController.RateTableMapping.ToString()
            };
        }

        public static Expression<Func<RateTable, RateTableBo>> Expression()
        {
            return entity => new RateTableBo
            {
                Id = entity.Id,
                RateTableMappingUploadId = entity.RateTableMappingUploadId,
                TreatyCode = entity.TreatyCode,
                CedingTreatyCode = entity.CedingTreatyCode,

                BenefitId = entity.BenefitId,
                BenefitCode = entity.Benefit.Code,
                CedingPlanCode = entity.CedingPlanCode,
                CedingPlanCode2 = entity.CedingPlanCode2,
                CedingBenefitTypeCode = entity.CedingBenefitTypeCode,
                CedingBenefitRiskCode = entity.CedingBenefitRiskCode,
                GroupPolicyNumber = entity.GroupPolicyNumber,

                PremiumFrequencyCodePickListDetailId = entity.PremiumFrequencyCodePickListDetailId,
                PremiumFrequencyCode = entity.PremiumFrequencyCodePickListDetail.Code,

                ReinsBasisCodePickListDetailId = entity.ReinsBasisCodePickListDetailId,
                ReinsBasisCode = entity.ReinsBasisCodePickListDetail.Code,

                PolicyAmountFrom = entity.PolicyAmountFrom,
                PolicyAmountTo = entity.PolicyAmountTo,
                AttainedAgeFrom = entity.AttainedAgeFrom,
                AttainedAgeTo = entity.AttainedAgeTo,
                AarFrom = entity.AarFrom,
                AarTo = entity.AarTo,
                ReinsEffDatePolStartDate = entity.ReinsEffDatePolStartDate,
                ReinsEffDatePolEndDate = entity.ReinsEffDatePolEndDate,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                // Phase 2
                PolicyTermFrom = entity.PolicyTermFrom,
                PolicyTermTo = entity.PolicyTermTo,
                PolicyDurationFrom = entity.PolicyDurationFrom,
                PolicyDurationTo = entity.PolicyDurationTo,
                RateId = entity.RateId,
                RateCode = entity.Rate.Code,
                CedantId = entity.CedantId,
                CedantCode = entity.Cedant.Code,
                RiDiscountCode = entity.RiDiscountCode,
                LargeDiscountCode = entity.LargeDiscountCode,
                GroupDiscountCode = entity.GroupDiscountCode,

                ReportingStartDate = entity.ReportingStartDate,
                ReportingEndDate = entity.ReportingEndDate,
            };
        }

        public static RateTableBo FormBo(RateTable entity = null)
        {
            if (entity == null)
                return null;
            return new RateTableBo
            {
                Id = entity.Id,
                RateTableMappingUploadId = entity.RateTableMappingUploadId,
                RateTableMappingUploadBo = RateTableMappingUploadService.Find(entity.RateTableMappingUploadId),
                TreatyCode = entity.TreatyCode,
                CedingTreatyCode = entity.CedingTreatyCode,

                BenefitId = entity.BenefitId,
                BenefitBo = BenefitService.Find(entity.BenefitId),
                CedingPlanCode = entity.CedingPlanCode,
                CedingPlanCode2 = entity.CedingPlanCode2,
                CedingBenefitTypeCode = entity.CedingBenefitTypeCode,
                CedingBenefitRiskCode = entity.CedingBenefitRiskCode,
                GroupPolicyNumber = entity.GroupPolicyNumber,

                ReinsBasisCodePickListDetailId = entity.ReinsBasisCodePickListDetailId,
                ReinsBasisCodePickListDetailBo = PickListDetailService.Find(entity.ReinsBasisCodePickListDetailId),

                PremiumFrequencyCodePickListDetailId = entity.PremiumFrequencyCodePickListDetailId,
                PremiumFrequencyCodePickListDetailBo = PickListDetailService.Find(entity.PremiumFrequencyCodePickListDetailId),

                PolicyAmountFrom = entity.PolicyAmountFrom,
                PolicyAmountTo = entity.PolicyAmountTo,
                AttainedAgeFrom = entity.AttainedAgeFrom,
                AttainedAgeTo = entity.AttainedAgeTo,
                AarFrom = entity.AarFrom,
                AarTo = entity.AarTo,
                ReinsEffDatePolStartDate = entity.ReinsEffDatePolStartDate,
                ReinsEffDatePolEndDate = entity.ReinsEffDatePolEndDate,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,

                // Phase 2
                PolicyTermFrom = entity.PolicyTermFrom,
                PolicyTermTo = entity.PolicyTermTo,
                PolicyDurationFrom = entity.PolicyDurationFrom,
                PolicyDurationTo = entity.PolicyDurationTo,
                RateId = entity.RateId,
                RateBo = RateService.Find(entity.RateId),
                CedantId = entity.CedantId,
                CedantBo = CedantService.Find(entity.CedantId),

                RiDiscountCode = entity.RiDiscountCode,
                LargeDiscountCode = entity.LargeDiscountCode,
                GroupDiscountCode = entity.GroupDiscountCode,

                ReportingStartDate = entity.ReportingStartDate,
                ReportingEndDate = entity.ReportingEndDate,
            };
        }

        public static IList<RateTableBo> FormBos(IList<RateTable> entities = null)
        {
            if (entities == null)
                return null;
            IList<RateTableBo> bos = new List<RateTableBo>() { };
            foreach (RateTable entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RateTable FormEntity(RateTableBo bo = null)
        {
            if (bo == null)
                return null;
            return new RateTable
            {
                Id = bo.Id,
                RateTableMappingUploadId = bo.RateTableMappingUploadId,
                TreatyCode = bo.TreatyCode,
                CedingTreatyCode = bo.CedingTreatyCode,
                BenefitId = bo.BenefitId,
                CedingPlanCode = bo.CedingPlanCode,
                CedingPlanCode2 = bo.CedingPlanCode2,
                CedingBenefitTypeCode = bo.CedingBenefitTypeCode,
                CedingBenefitRiskCode = bo.CedingBenefitRiskCode,
                GroupPolicyNumber = bo.GroupPolicyNumber,
                PremiumFrequencyCodePickListDetailId = bo.PremiumFrequencyCodePickListDetailId,
                ReinsBasisCodePickListDetailId = bo.ReinsBasisCodePickListDetailId,
                PolicyAmountFrom = bo.PolicyAmountFrom,
                PolicyAmountTo = bo.PolicyAmountTo,
                AttainedAgeFrom = bo.AttainedAgeFrom,
                AttainedAgeTo = bo.AttainedAgeTo,
                AarFrom = bo.AarFrom,
                AarTo = bo.AarTo,
                ReinsEffDatePolStartDate = bo.ReinsEffDatePolStartDate,
                ReinsEffDatePolEndDate = bo.ReinsEffDatePolEndDate,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,

                // Phase 2
                PolicyTermFrom = bo.PolicyTermFrom,
                PolicyTermTo = bo.PolicyTermTo,
                PolicyDurationFrom = bo.PolicyDurationFrom,
                PolicyDurationTo = bo.PolicyDurationTo,
                RateId = bo.RateId,
                CedantId = bo.CedantId,
                RiDiscountCode = bo.RiDiscountCode,
                LargeDiscountCode = bo.LargeDiscountCode,
                GroupDiscountCode = bo.GroupDiscountCode,

                ReportingStartDate = bo.ReportingStartDate,
                ReportingEndDate = bo.ReportingEndDate,
            };
        }

        public static bool IsExists(int id)
        {
            return RateTable.IsExists(id);
        }

        public static RateTableBo Find(int id)
        {
            return FormBo(RateTable.Find(id));
        }

        public static int CountByPremiumFrequencyCodePickListDetailId(int premiumFrequencyCodePickListDetailId)
        {
            return RateTable.CountByPremiumFrequencyCodePickListDetailId(premiumFrequencyCodePickListDetailId);
        }

        public static int CountByReinsBasisCodePickListDetailId(int reinsBasisCodePickListDetailId)
        {
            return RateTable.CountByReinsBasisCodePickListDetailId(reinsBasisCodePickListDetailId);
        }

        public static int CountByRateId(int rateId)
        {
            return RateTable.CountByRateId(rateId);
        }

        public static int CountByCedantId(int cedantId)
        {
            return RateTable.CountByCedantId(cedantId);
        }

        //public static int CountByRiDiscountId(int riDiscountId)
        //{
        //    return RateTable.CountByRiDiscountId(riDiscountId);
        //}

        //public static int CountByLargeDiscountId(int largeDiscountId)
        //{
        //    return RateTable.CountByLargeDiscountId(largeDiscountId);
        //}

        //public static int CountByGroupDiscountId(int groupDiscountId)
        //{
        //    return RateTable.CountByGroupDiscountId(groupDiscountId);
        //}

        public static IEnumerable<string> GetRateTableCodes()
        {
            using (var db = new AppDbContext())
            {
                return db.RateTables.Select(q => q.RateTableCode).ToList();
            }
        }

        public static Result Save(ref RateTableBo bo)
        {
            if (!RateTable.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref RateTableBo bo, ref TrailObject trail)
        {
            if (!RateTable.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RateTableBo bo)
        {
            RateTable entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RateTableBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RateTableBo bo)
        {
            Result result = Result();

            RateTable entity = RateTable.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.RateTableMappingUploadId = bo.RateTableMappingUploadId;
                entity.TreatyCode = bo.TreatyCode;
                entity.CedingTreatyCode = bo.CedingTreatyCode;
                entity.BenefitId = bo.BenefitId;
                entity.CedingPlanCode = bo.CedingPlanCode;
                entity.CedingPlanCode2 = bo.CedingPlanCode2;
                entity.CedingBenefitTypeCode = bo.CedingBenefitTypeCode;
                entity.CedingBenefitRiskCode = bo.CedingBenefitRiskCode;
                entity.GroupPolicyNumber = bo.GroupPolicyNumber;
                entity.PremiumFrequencyCodePickListDetailId = bo.PremiumFrequencyCodePickListDetailId;
                entity.ReinsBasisCodePickListDetailId = bo.ReinsBasisCodePickListDetailId;
                entity.PolicyAmountFrom = bo.PolicyAmountFrom;
                entity.PolicyAmountTo = bo.PolicyAmountTo;
                entity.AttainedAgeFrom = bo.AttainedAgeFrom;
                entity.AttainedAgeTo = bo.AttainedAgeTo;
                entity.AarFrom = bo.AarFrom;
                entity.AarTo = bo.AarTo;
                entity.ReinsEffDatePolStartDate = bo.ReinsEffDatePolStartDate;
                entity.ReinsEffDatePolEndDate = bo.ReinsEffDatePolEndDate;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

                // Phase 2
                entity.PolicyTermFrom = bo.PolicyTermFrom;
                entity.PolicyTermTo = bo.PolicyTermTo;
                entity.PolicyDurationFrom = bo.PolicyDurationFrom;
                entity.PolicyDurationTo = bo.PolicyDurationTo;
                entity.RateId = bo.RateId;
                entity.CedantId = bo.CedantId;
                entity.RiDiscountCode = bo.RiDiscountCode;
                entity.LargeDiscountCode = bo.LargeDiscountCode;
                entity.GroupDiscountCode = bo.GroupDiscountCode;

                entity.ReportingStartDate = bo.ReportingStartDate;
                entity.ReportingEndDate = bo.ReportingEndDate;

                result.DataTrail = entity.Update();

            }
            return result;
        }

        public static Result Update(ref RateTableBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RateTableBo bo)
        {
            RateTableDetailService.DeleteByRateTableId(bo.Id);
            RateTable.Delete(bo.Id);
        }

        public static Result Delete(RateTableBo bo, ref TrailObject trail)
        {
            Result result = Result();

            RateTableDetailService.DeleteByRateTableId(bo.Id); // DO NOT TRAIL
            DataTrail dataTrail = RateTable.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static Result ValidateRange(RateTableBo bo)
        {
            Result result = new Result();

            if (bo.ReinsEffDatePolEndDate != null && bo.ReinsEffDatePolStartDate == null)
            {
                result.AddError(string.Format(MessageBag.Required, "The Reinsurance Effective Start Date Field"));
            }
            else if (bo.ReinsEffDatePolStartDate != null && bo.ReinsEffDatePolEndDate == null)
            {
                if (DateTime.Parse(Util.GetDefaultEndDate()) <= bo.ReinsEffDatePolStartDate)
                {
                    result.AddError(string.Format(MessageBag.EndDefaultDate, "Reinsurance Effective", Util.GetDefaultEndDate()));
                }
            }
            else if (bo.ReinsEffDatePolStartDate != null && bo.ReinsEffDatePolEndDate != null)
            {
                if (bo.ReinsEffDatePolEndDate <= bo.ReinsEffDatePolStartDate)
                {
                    result.AddError(string.Format(MessageBag.EndDateLater, "Reinsurance Effective"));
                }
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

            if (bo.PolicyAmountTo != null && bo.PolicyAmountFrom == null)
            {
                result.AddError(string.Format(MessageBag.Required, "The ORI Sum Assured From Field"));
            }
            else if (bo.PolicyAmountFrom != null && bo.PolicyAmountTo == null)
            {
                result.AddError(string.Format(MessageBag.Required, "The ORI Sum Assured To Field"));
            }
            else if (bo.PolicyAmountTo != null && bo.PolicyAmountFrom != null && bo.PolicyAmountTo < bo.PolicyAmountFrom)
            {
                result.AddError(string.Format(MessageBag.GreaterOrEqualTo, "ORI Sum Assured To", "ORI Sum Assured From"));
            }

            if (bo.AarTo != null && bo.AarFrom == null)
            {
                result.AddError(string.Format(MessageBag.Required, "The AAR From Field"));
            }
            else if (bo.AarFrom != null && bo.AarTo == null)
            {
                result.AddError(string.Format(MessageBag.Required, "The AAR To Field"));
            }
            else if (bo.AarTo != null && bo.AarFrom != null && bo.AarTo < bo.AarFrom)
            {
                result.AddError(string.Format(MessageBag.GreaterOrEqualTo, "AAR To", "AAR From"));
            }

            if (bo.PolicyTermTo != null && bo.PolicyTermFrom == null)
            {
                result.AddError(string.Format(MessageBag.Required, "The Policy Term From Field"));
            }
            else if (bo.PolicyTermFrom != null && bo.PolicyTermTo == null)
            {
                result.AddError(string.Format(MessageBag.Required, "The Policy Term To Field"));
            }
            else if (bo.PolicyTermTo != null && bo.PolicyTermFrom != null && bo.PolicyTermTo < bo.PolicyTermFrom)
            {
                result.AddError(string.Format(MessageBag.GreaterOrEqualTo, "Policy Term To", "Policy Term From"));
            }

            if (bo.PolicyDurationTo != null && bo.PolicyDurationFrom == null)
            {
                result.AddError(string.Format(MessageBag.Required, "The Policy Duration From Field"));
            }
            else if (bo.PolicyDurationFrom != null && bo.PolicyDurationTo == null)
            {
                result.AddError(string.Format(MessageBag.Required, "The Policy Duration To Field"));
            }
            else if (bo.PolicyDurationTo != null && bo.PolicyDurationFrom != null && bo.PolicyDurationTo < bo.PolicyDurationFrom)
            {
                result.AddError(string.Format(MessageBag.GreaterOrEqualTo, "Policy Duration To", "Policy Duration From"));
            }

            return result;
        }

        public static Result ValidateMapping(RateTableBo bo)
        {
            Result result = new Result();
            var list = new Dictionary<string, List<string>> { };

            foreach (var detail in CreateDetails(bo))
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

                //if (RateTableDetailService.CountByCombination(detail.Combination, bo) > 0)
                //{
                //    result.AddError("Existing Rate Table Mapping Combination Found");
                //    break;
                //}

                if (RateTableDetailService.CountDuplicateByParams(bo, detail) > 0)
                {
                    result.AddError("Existing Rate Table Mapping Combination Found");
                    break;
                }
            }
            return result;
        }

        public static Result ValidateMappedValue(RateTableBo bo)
        {
            Result result = new Result();

            if (!bo.RateId.HasValue &&
                string.IsNullOrEmpty(bo.RiDiscountCode) &&
                string.IsNullOrEmpty(bo.LargeDiscountCode) &&
                string.IsNullOrEmpty(bo.GroupDiscountCode)
            )
            {
                result.AddError("Please enter at least one mapped values");
            }

            return result;
        }

        public static IList<RateTableDetailBo> CreateDetails(RateTableBo bo, int createdById = 0)
        {
            var details = new List<RateTableDetailBo> { };
            CartesianProduct<string> rateTableMappings = new CartesianProduct<string>(
               bo.TreatyCode.ToArraySplitTrim(),
               bo.CedingPlanCode.ToArraySplitTrim(),
               bo.CedingTreatyCode.ToArraySplitTrim(),
               bo.CedingPlanCode2.ToArraySplitTrim(),
               bo.CedingBenefitTypeCode.ToArraySplitTrim(),
               bo.CedingBenefitRiskCode.ToArraySplitTrim(),
               bo.GroupPolicyNumber.ToArraySplitTrim()
            );
            foreach (var item in rateTableMappings.Get())
            {
                var treatyCode = item[0];
                var cedingPlanCode = item[1];
                var cedingTreatyCode = item[2];
                var cedingPlanCode2 = item[3];
                var cedingBenefitTypeCode = item[4];
                var cedingBenefitRiskCode = item[5];
                var groupPolicyNumber = item[6];
                var items = new List<string>
                {
                    treatyCode,
                    cedingPlanCode,
                    cedingTreatyCode,
                    cedingPlanCode2,
                    cedingBenefitTypeCode,
                    cedingBenefitRiskCode,
                    groupPolicyNumber,
                    bo.BenefitBo != null ? bo.BenefitBo.Code : "",
                    bo.PremiumFrequencyCodePickListDetailBo != null ? bo.PremiumFrequencyCodePickListDetailBo.Code : "",
                    bo.ReinsBasisCodePickListDetailBo != null ? bo.ReinsBasisCodePickListDetailBo.Code : "",
                };
                details.Add(new RateTableDetailBo
                {
                    RateTableId = bo.Id,
                    Combination = string.Join("|", items),
                    CreatedById = createdById,
                    CreatedAt = bo.CreatedAt,
                    TreatyCode = string.IsNullOrEmpty(treatyCode) ? null : treatyCode,
                    CedingPlanCode = string.IsNullOrEmpty(cedingPlanCode) ? null : cedingPlanCode,
                    CedingTreatyCode = string.IsNullOrEmpty(cedingTreatyCode) ? null : cedingTreatyCode,
                    CedingPlanCode2 = string.IsNullOrEmpty(cedingPlanCode2) ? null : cedingPlanCode2,
                    CedingBenefitTypeCode = string.IsNullOrEmpty(cedingBenefitTypeCode) ? null : cedingBenefitTypeCode,
                    CedingBenefitRiskCode = string.IsNullOrEmpty(cedingBenefitRiskCode) ? null : cedingBenefitRiskCode,
                    GroupPolicyNumber = string.IsNullOrEmpty(groupPolicyNumber) ? null : groupPolicyNumber,
                });
            }
            return details;
        }

        public static void TrimMaxLength(ref RateTableDetailBo detailBo, ref Dictionary<string, List<string>> list)
        {
            var entity = new RateTableDetail();
            foreach(var property in (typeof(RateTableDetailBo)).GetProperties())
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

        public static void ProcessMappingDetail(RateTableBo bo, int createdById)
        {
            RateTableDetailService.DeleteByRateTableId(bo.Id);
            foreach (var detail in CreateDetails(bo, createdById))
            {
                var d = detail;
                RateTableDetailService.Create(ref d);
            }
        }

        public static void ProcessMappingDetail(RateTableBo bo, int createdById, ref TrailObject trail)
        {
            RateTableDetailService.DeleteByRateTableId(bo.Id);
            foreach (var detail in CreateDetails(bo, createdById))
            {
                var d = detail;
                RateTableDetailService.Create(ref d, ref trail);
            }
        }
    }
}
