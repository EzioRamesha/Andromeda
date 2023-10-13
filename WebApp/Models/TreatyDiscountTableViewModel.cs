using BusinessObject;
using DataAccess.Entities;
using Services;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class TreatyDiscountTableViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required, DisplayName("Treaty Discount Rule"), StringLength(30)]
        public string Rule { get; set; }

        [Required]
        public int Type { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public TreatyDiscountTableViewModel() { }

        public TreatyDiscountTableViewModel(TreatyDiscountTableBo treatyDiscountTableBo)
        {
            Set(treatyDiscountTableBo);
        }

        public void Set(TreatyDiscountTableBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                Rule = bo.Rule;
                Type = bo.Type;
                Description = bo.Description;
            }
        }

        public TreatyDiscountTableBo FormBo(int createdById, int updatedById)
        {
            return new TreatyDiscountTableBo
            {
                Rule = Rule?.Trim(),
                Type = Type,
                Description = Description,
                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<TreatyDiscountTable, TreatyDiscountTableViewModel>> Expression()
        {
            return entity => new TreatyDiscountTableViewModel
            {
                Id = entity.Id,
                Rule = entity.Rule,
                Type = entity.Type,
                Description = entity.Description
            };
        }

        public List<TreatyDiscountTableDetailBo> GetTreatyDiscountTableDetails(FormCollection form, ref Result result)
        {
            int index = 0;
            int maxIndex = int.Parse(form.Get("treatyDiscountTableDetailMaxIndex"));
            List<TreatyDiscountTableDetailBo> treatyDiscountTableDetailBos = new List<TreatyDiscountTableDetailBo> { };

            while (index <= maxIndex)
            {
                string cedingPlanCode = form.Get(string.Format("cedingPlanCode[{0}]", index));
                string benefitCode = form.Get(string.Format("benefitCode[{0}]", index));
                string ageFromStr = form.Get(string.Format("ageFromStr[{0}]", index));
                string ageToStr = form.Get(string.Format("ageToStr[{0}]", index));
                string aarFromStr = form.Get(string.Format("AARFromStr[{0}]", index));
                string aarToStr = form.Get(string.Format("AARToStr[{0}]", index));
                string discountStr = form.Get(string.Format("discountStr[{0}]", index));
                string id = form.Get(string.Format("treatyDiscountTableDetailId[{0}]", index));

                int treatyDiscountTableDetailId = 0;
                if (!string.IsNullOrEmpty(id))
                    treatyDiscountTableDetailId = int.Parse(id);

                if (string.IsNullOrEmpty(cedingPlanCode) &&
                    string.IsNullOrEmpty(benefitCode) &&
                    string.IsNullOrEmpty(ageFromStr) &&
                    string.IsNullOrEmpty(ageToStr) &&
                    string.IsNullOrEmpty(aarFromStr) &&
                    string.IsNullOrEmpty(aarToStr) &&
                    string.IsNullOrEmpty(discountStr) &&
                    treatyDiscountTableDetailId == 0)
                {
                    index++;
                    continue;
                }

                TreatyDiscountTableDetailBo treatyDiscountTableDetailBo = new TreatyDiscountTableDetailBo
                {
                    TreatyDiscountTableId = Id,
                    CedingPlanCode = cedingPlanCode,
                    BenefitCode = benefitCode,
                    AgeFromStr = ageFromStr,
                    AgeToStr = ageToStr,
                    AARFromStr = aarFromStr,
                    AARToStr = aarToStr,
                    DiscountStr = discountStr,
                };

                Result validateResult = treatyDiscountTableDetailBo.Validate(index + 1);

                result.AddErrorRange(validateResult.ToErrorArray());

                if (result.Valid)
                {
                    treatyDiscountTableDetailBo.AgeFrom = Util.GetParseInt(ageFromStr);
                    treatyDiscountTableDetailBo.AgeTo = Util.GetParseInt(ageToStr);
                    treatyDiscountTableDetailBo.AARFrom = Util.StringToDouble(aarFromStr);
                    treatyDiscountTableDetailBo.AARTo = Util.StringToDouble(aarToStr);
                    treatyDiscountTableDetailBo.Discount = Util.StringToDouble(treatyDiscountTableDetailBo.DiscountStr).Value;
                }

                if (treatyDiscountTableDetailId != 0)
                {
                    treatyDiscountTableDetailBo.Id = treatyDiscountTableDetailId;
                }

                treatyDiscountTableDetailBos.Add(treatyDiscountTableDetailBo);
                index++;
            }
            return treatyDiscountTableDetailBos;
        }

        public void ValidateDuplicate(List<TreatyDiscountTableDetailBo> treatyDiscountTableDetailBos, ref Result result)
        {
            int count = 1;
            List<TreatyDiscountAgeRange> ageRanges = new List<TreatyDiscountAgeRange> { };

            foreach (TreatyDiscountTableDetailBo treatyDiscountTableDetailBo in treatyDiscountTableDetailBos)
            {
                if (count == 1)
                {
                    ageRanges.Add(new TreatyDiscountAgeRange
                    {
                        CedingPlanCode = treatyDiscountTableDetailBo.CedingPlanCode,
                        BenefitCode = treatyDiscountTableDetailBo.BenefitCode,
                        AgeFrom = treatyDiscountTableDetailBo.AgeFrom,
                        AgeTo = treatyDiscountTableDetailBo.AgeTo,
                        AARFrom = treatyDiscountTableDetailBo.AARFrom,
                        AARTo = treatyDiscountTableDetailBo.AARTo
                    });
                }
                else
                {
                    var cedingPlanCodes = Util.ToArraySplitTrim(treatyDiscountTableDetailBo.CedingPlanCode).ToList();
                    var benefitCodes = Util.ToArraySplitTrim(treatyDiscountTableDetailBo.BenefitCode).ToList();

                    var list = new List<TreatyDiscountAgeRange>();
                    if (treatyDiscountTableDetailBo.AgeFrom.HasValue && treatyDiscountTableDetailBo.AgeTo.HasValue)
                    {
                        list = ageRanges.Where(q =>
                            (
                                q.AgeFrom <= treatyDiscountTableDetailBo.AgeFrom && q.AgeTo >= treatyDiscountTableDetailBo.AgeFrom
                                ||
                                q.AgeFrom <= treatyDiscountTableDetailBo.AgeTo && q.AgeTo >= treatyDiscountTableDetailBo.AgeTo
                            )
                            || (q.AgeFrom == null && q.AgeTo == null)
                            ).ToList();
                    }
                    else if (treatyDiscountTableDetailBo.AARFrom.HasValue && treatyDiscountTableDetailBo.AARTo.HasValue)
                    {
                        list = ageRanges.Where(q =>
                            (
                                q.AARFrom <= treatyDiscountTableDetailBo.AARFrom && q.AARTo >= treatyDiscountTableDetailBo.AARFrom
                                ||
                                q.AARFrom <= treatyDiscountTableDetailBo.AARTo && q.AARTo >= treatyDiscountTableDetailBo.AARTo
                            )
                            || (q.AARFrom == null && q.AARTo == null)
                            ).ToList();
                    }
                    else
                    {
                        list = ageRanges;
                    }

                    if (!list.IsNullOrEmpty())
                    {
                        foreach (var ageRange in list)
                        {
                            var cpcList = Util.ToArraySplitTrim(ageRange.CedingPlanCode).ToList();
                            var bcList = Util.ToArraySplitTrim(ageRange.BenefitCode).ToList();

                            var cpcIntersect = cpcList.Intersect(cedingPlanCodes);
                            var bcIntersect = bcList.Intersect(benefitCodes);

                            if (cpcIntersect.Count() > 0 && string.IsNullOrEmpty(treatyDiscountTableDetailBo.BenefitCode))
                            {
                                int idx = treatyDiscountTableDetailBos.IndexOf(treatyDiscountTableDetailBo);
                                result.AddError(string.Format("Duplicate Treaty Discount Found at row #{0}", idx + 1));
                                break;
                            }
                            else if (cpcIntersect.Count() > 0 && bcIntersect.Count() > 0)
                            {
                                int idx = treatyDiscountTableDetailBos.IndexOf(treatyDiscountTableDetailBo);
                                result.AddError(string.Format("Duplicate Treaty Discount Found at row #{0}", idx + 1));
                                break;
                            }
                        }
                    }

                    ageRanges.Add(new TreatyDiscountAgeRange { AgeFrom = treatyDiscountTableDetailBo.AgeFrom, AgeTo = treatyDiscountTableDetailBo.AgeTo, AARFrom = treatyDiscountTableDetailBo.AARFrom, AARTo = treatyDiscountTableDetailBo.AARTo, CedingPlanCode = treatyDiscountTableDetailBo.CedingPlanCode, BenefitCode = treatyDiscountTableDetailBo.BenefitCode });
                }
                count++;
            }
        }

        public void ProcessTreatyDiscountTableDetails(List<TreatyDiscountTableDetailBo> treatyDiscountTableDetailBos, int authUserId, ref TrailObject trail)
        {
            List<int> savedIds = new List<int> { };

            foreach (TreatyDiscountTableDetailBo bo in treatyDiscountTableDetailBos)
            {
                TreatyDiscountTableDetailBo treatyDiscountTableDetailBo = bo;
                treatyDiscountTableDetailBo.TreatyDiscountTableId = Id;
                treatyDiscountTableDetailBo.CreatedById = authUserId;
                treatyDiscountTableDetailBo.UpdatedById = authUserId;

                TreatyDiscountTableDetailService.Save(ref treatyDiscountTableDetailBo, ref trail);
                savedIds.Add(treatyDiscountTableDetailBo.Id);
            }
            TreatyDiscountTableDetailService.DeleteByTreatyDiscountTableIdExcept(Id, savedIds, ref trail);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (Type == 0)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "Type"),
                    new[] { nameof(Type) }));
            }

            return results;
        }
    }
}