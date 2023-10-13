using BusinessObject;
using DataAccess.Entities;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Services
{
    public class FacMasterListingService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(FacMasterListing)),
                Controller = ModuleBo.ModuleController.FacMasterListing.ToString()
            };
        }

        public static Expression<Func<FacMasterListing, FacMasterListingBo>> Expression()
        {
            return entity => new FacMasterListingBo
            {
                Id = entity.Id,
                UniqueId = entity.UniqueId,
                EwarpNumber = entity.EwarpNumber,
                InsuredName = entity.InsuredName,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                InsuredGenderCodePickListDetailId = entity.InsuredGenderCodePickListDetailId,
                InsuredGenderCode = entity.InsuredGenderCodePickListDetail.Code,
                CedantId = entity.CedantId,
                CedantCode = entity.Cedant.Code,
                PolicyNumber = entity.PolicyNumber,
                FlatExtraAmountOffered = entity.FlatExtraAmountOffered,
                FlatExtraDuration = entity.FlatExtraDuration,
                BenefitCode = entity.BenefitCode,
                SumAssuredOffered = entity.SumAssuredOffered,
                EwarpActionCode = entity.EwarpActionCode,
                UwRatingOffered = entity.UwRatingOffered,
                OfferLetterSentDate = entity.OfferLetterSentDate,
                UwOpinion = entity.UwOpinion,
                Remark = entity.Remark,
                CedingBenefitTypeCode = entity.CedingBenefitTypeCode,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static FacMasterListingBo FormBo(FacMasterListing entity = null)
        {
            if (entity == null)
                return null;
            return new FacMasterListingBo
            {
                Id = entity.Id,
                UniqueId = entity.UniqueId,
                EwarpNumber = entity.EwarpNumber,
                InsuredName = entity.InsuredName,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                InsuredGenderCodePickListDetailId = entity.InsuredGenderCodePickListDetailId,
                InsuredGenderCodePickListDetailBo = PickListDetailService.Find(entity.InsuredGenderCodePickListDetailId),
                CedantId = entity.CedantId,
                CedantBo = CedantService.Find(entity.CedantId),
                PolicyNumber = entity.PolicyNumber,
                FlatExtraAmountOffered = entity.FlatExtraAmountOffered,
                FlatExtraDuration = entity.FlatExtraDuration,
                BenefitCode = entity.BenefitCode,
                SumAssuredOffered = entity.SumAssuredOffered,
                EwarpActionCode = entity.EwarpActionCode,
                UwRatingOffered = entity.UwRatingOffered,
                OfferLetterSentDate = entity.OfferLetterSentDate,
                UwOpinion = entity.UwOpinion,
                Remark = entity.Remark,
                CedingBenefitTypeCode = entity.CedingBenefitTypeCode,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<FacMasterListingBo> FormBos(IList<FacMasterListing> entities = null)
        {
            if (entities == null)
                return null;
            IList<FacMasterListingBo> bos = new List<FacMasterListingBo>() { };
            foreach (FacMasterListing entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static FacMasterListing FormEntity(FacMasterListingBo bo = null)
        {
            if (bo == null)
                return null;
            return new FacMasterListing
            {
                Id = bo.Id,
                UniqueId = bo.UniqueId?.Trim(),
                EwarpNumber = bo.EwarpNumber,
                InsuredName = bo.InsuredName?.Trim(),
                InsuredDateOfBirth = bo.InsuredDateOfBirth,
                InsuredGenderCodePickListDetailId = bo.InsuredGenderCodePickListDetailId,
                CedantId = bo.CedantId,
                PolicyNumber = bo.PolicyNumber,
                FlatExtraAmountOffered = bo.FlatExtraAmountOffered,
                FlatExtraDuration = bo.FlatExtraDuration,
                BenefitCode = bo.BenefitCode,
                SumAssuredOffered = bo.SumAssuredOffered,
                EwarpActionCode = bo.EwarpActionCode?.Trim(),
                UwRatingOffered = bo.UwRatingOffered,
                OfferLetterSentDate = bo.OfferLetterSentDate,
                UwOpinion = bo.UwOpinion?.Trim(),
                Remark = bo.Remark?.Trim(),
                CedingBenefitTypeCode = bo.CedingBenefitTypeCode,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return FacMasterListing.IsExists(id);
        }

        public static bool IsDuplicateUniqueId(FacMasterListing facMasterListing)
        {
            return facMasterListing.IsDuplicateUniqueId();
        }

        public static FacMasterListingBo Find(int id)
        {
            return FormBo(FacMasterListing.Find(id));
        }

        public static int CountByInsuredGenderCodePickListDetailId(int insuredGenderCodePickListDetailId)
        {
            return FacMasterListing.CountByInsuredGenderCodePickListDetailId(insuredGenderCodePickListDetailId);
        }

        public static int CountByCedantId(int cedantId)
        {
            return FacMasterListing.CountByCedantId(cedantId);
        }

        public static IList<FacMasterListingBo> Get()
        {
            return FormBos(FacMasterListing.Get());
        }

        public static Result Save(ref FacMasterListingBo bo)
        {
            if (!FacMasterListing.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref FacMasterListingBo bo, ref TrailObject trail)
        {
            if (!FacMasterListing.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref FacMasterListingBo bo)
        {
            FacMasterListing entity = FormEntity(bo);

            Result result = Result();

            if (IsDuplicateUniqueId(entity))
            {
                result.AddTakenError("Unique Id", entity.UniqueId);
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref FacMasterListingBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref FacMasterListingBo bo)
        {
            Result result = Result();

            FacMasterListing entity = FacMasterListing.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (IsDuplicateUniqueId(FormEntity(bo)))
            {
                result.AddTakenError("Unique Id", entity.UniqueId);
            }

            if (result.Valid)
            {
                entity.UniqueId = bo.UniqueId;
                entity.EwarpNumber = bo.EwarpNumber;
                entity.InsuredName = bo.InsuredName;
                entity.InsuredDateOfBirth = bo.InsuredDateOfBirth;
                entity.InsuredGenderCodePickListDetailId = bo.InsuredGenderCodePickListDetailId;
                entity.CedantId = bo.CedantId;
                entity.PolicyNumber = bo.PolicyNumber;
                entity.FlatExtraAmountOffered = bo.FlatExtraAmountOffered;
                entity.FlatExtraDuration = bo.FlatExtraDuration;
                entity.BenefitCode = bo.BenefitCode;
                entity.SumAssuredOffered = bo.SumAssuredOffered;
                entity.EwarpActionCode = bo.EwarpActionCode;
                entity.UwRatingOffered = bo.UwRatingOffered;
                entity.OfferLetterSentDate = bo.OfferLetterSentDate;
                entity.UwOpinion = bo.UwOpinion;
                entity.Remark = bo.Remark;
                entity.CedingBenefitTypeCode = bo.CedingBenefitTypeCode;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();

            }
            return result;
        }

        public static Result Update(ref FacMasterListingBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(FacMasterListingBo bo)
        {
            FacMasterListingDetailService.DeleteByFacMasterListingId(bo.Id);
            FacMasterListing.Delete(bo.Id);
        }

        public static Result Delete(FacMasterListingBo bo, ref TrailObject trail)
        {
            Result result = Result();

            FacMasterListingDetailService.DeleteByFacMasterListingId(bo.Id); // DO NOT TRAIL
            DataTrail dataTrail = FacMasterListing.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static Result ValidateMapping(FacMasterListingBo bo)
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
            }
            return result;
        }

        public static IList<FacMasterListingDetailBo> CreateDetails(FacMasterListingBo bo, int createdById = 0)
        {
            var details = new List<FacMasterListingDetailBo> { };
            CartesianProduct<string> facMasterListings = new CartesianProduct<string>(
               bo.PolicyNumber.ToArraySplitTrim(),
               bo.BenefitCode.ToArraySplitTrim(),
               bo.CedingBenefitTypeCode.ToArraySplitTrim()
            );
            foreach (var item in facMasterListings.Get())
            {
                var policyNumber = item[0];
                var benefitCode = item[1];
                var cedingBenefitTypeCode = item[2];
                details.Add(new FacMasterListingDetailBo
                {
                    FacMasterListingId = bo.Id,
                    PolicyNumber = string.IsNullOrEmpty(policyNumber) ? null : policyNumber,
                    BenefitCode = string.IsNullOrEmpty(benefitCode) ? null : benefitCode,
                    CedingBenefitTypeCode = string.IsNullOrEmpty(cedingBenefitTypeCode) ? null : cedingBenefitTypeCode,
                    CreatedById = createdById,
                });
            }
            return details;
        }

        public static void TrimMaxLength(ref FacMasterListingDetailBo detailBo, ref Dictionary<string, List<string>> list)
        {
            var entity = new FacMasterListingDetail();
            foreach (var property in (typeof(FacMasterListingDetailBo)).GetProperties())
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

        public static void ProcessMappingDetail(FacMasterListingBo bo, int createdById)
        {
            FacMasterListingDetailService.DeleteByFacMasterListingId(bo.Id);
            foreach (var detail in CreateDetails(bo, createdById))
            {
                var d = detail;
                FacMasterListingDetailService.Create(ref d);
            }
        }

        public static void ProcessMappingDetail(FacMasterListingBo bo, int createdById, ref TrailObject trail)
        {
            FacMasterListingDetailService.DeleteByFacMasterListingId(bo.Id);
            foreach (var detail in CreateDetails(bo, createdById))
            {
                var d = detail;
                FacMasterListingDetailService.Create(ref d, ref trail);
            }
        }
    }
}
