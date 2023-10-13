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
    public class PremiumSpreadTableViewModel
    {
        public int Id { get; set; }

        [Required, DisplayName("Premium Spread Rule"), StringLength(30)]
        public string Rule { get; set; }

        [Required]
        public int Type { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public PremiumSpreadTableViewModel() { }

        public PremiumSpreadTableViewModel(PremiumSpreadTableBo premiumSpreadTableBo)
        {
            Set(premiumSpreadTableBo);
        }

        public void Set(PremiumSpreadTableBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                Rule = bo.Rule;
                Type = bo.Type;
                Description = bo.Description;
            }
        }

        public PremiumSpreadTableBo FormBo(int createdById, int updatedById)
        {
            return new PremiumSpreadTableBo
            {
                Rule = Rule?.Trim(),
                Type = Type,
                Description = Description,
                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<PremiumSpreadTable, PremiumSpreadTableViewModel>> Expression()
        {
            return entity => new PremiumSpreadTableViewModel
            {
                Id = entity.Id,
                Rule = entity.Rule,
                Type = entity.Type,
                Description = entity.Description,
            };
        }

        public List<PremiumSpreadTableDetailBo> GetPremiumSpreadTableDetails(FormCollection form, ref Result result)
        {
            int index = 0;
            int maxIndex = int.Parse(form.Get("premiumSpreadTableDetailMaxIndex"));
            List<PremiumSpreadTableDetailBo> premiumSpreadTableDetailBos = new List<PremiumSpreadTableDetailBo> { };

            while (index <= maxIndex)
            {
                string cedingPlanCode = form.Get(string.Format("cedingPlanCode[{0}]", index));
                //string benefitId = form.Get(string.Format("benefitId[{0}]", index));
                string benefitCode = form.Get(string.Format("benefitCode[{0}]", index));
                string ageFromStr = form.Get(string.Format("ageFromStr[{0}]", index));
                string ageToStr = form.Get(string.Format("ageToStr[{0}]", index));
                string premiumSpreadStr = form.Get(string.Format("premiumSpreadStr[{0}]", index));
                string id = form.Get(string.Format("premiumSpreadTableDetailId[{0}]", index));

                int premiumSpreadTableDetailId = 0;
                if (!string.IsNullOrEmpty(id))
                    premiumSpreadTableDetailId = int.Parse(id);

                if (string.IsNullOrEmpty(cedingPlanCode) &&
                    string.IsNullOrEmpty(benefitCode) &&
                    //(string.IsNullOrEmpty(benefitId) || benefitId == "0") &&
                    string.IsNullOrEmpty(ageFromStr) &&
                    string.IsNullOrEmpty(ageToStr) &&
                    string.IsNullOrEmpty(premiumSpreadStr) &&
                    premiumSpreadTableDetailId == 0)
                {
                    index++;
                    continue;
                }

                PremiumSpreadTableDetailBo premiumSpreadTableDetailBo = new PremiumSpreadTableDetailBo
                {
                    PremiumSpreadTableId = Id,
                    CedingPlanCode = cedingPlanCode,
                    BenefitCode = benefitCode,
                    //BenefitId = Util.GetParseInt(benefitId) ?? 0,
                    AgeFromStr = ageFromStr,
                    AgeToStr = ageToStr,
                    PremiumSpreadStr = premiumSpreadStr,
                };

                Result validateResult = premiumSpreadTableDetailBo.Validate(index + 1);

                result.AddErrorRange(validateResult.ToErrorArray());

                if (result.Valid)
                {
                    premiumSpreadTableDetailBo.AgeFrom = Util.GetParseInt(ageFromStr);
                    premiumSpreadTableDetailBo.AgeTo = Util.GetParseInt(ageToStr);
                    premiumSpreadTableDetailBo.PremiumSpread = Util.StringToDouble(premiumSpreadTableDetailBo.PremiumSpreadStr).Value;
                }

                if (premiumSpreadTableDetailId != 0)
                {
                    premiumSpreadTableDetailBo.Id = premiumSpreadTableDetailId;
                }

                premiumSpreadTableDetailBos.Add(premiumSpreadTableDetailBo);
                index++;
            }
            return premiumSpreadTableDetailBos;
        }

        public void ValidateDuplicate(List<PremiumSpreadTableDetailBo> premiumSpreadTableDetailBos, ref Result result)
        {
            int count = 1;
            List<PremiumSpreadAgeRange> ageRanges = new List<PremiumSpreadAgeRange> { };

            foreach (PremiumSpreadTableDetailBo premiumSpreadTableDetailBo in premiumSpreadTableDetailBos)
            {
                if (count == 1)
                {
                    ageRanges.Add(new PremiumSpreadAgeRange
                    {
                        CedingPlanCode = premiumSpreadTableDetailBo.CedingPlanCode,
                        BenefitCode = premiumSpreadTableDetailBo.BenefitCode,
                        //BenefitId = premiumSpreadTableDetailBo.BenefitId,
                        AgeFrom = premiumSpreadTableDetailBo.AgeFrom,
                        AgeTo = premiumSpreadTableDetailBo.AgeTo
                    });
                }
                else
                {
                    var cedingPlanCodes = Util.ToArraySplitTrim(premiumSpreadTableDetailBo.CedingPlanCode).ToList();
                    var benefitCodes = Util.ToArraySplitTrim(premiumSpreadTableDetailBo.BenefitCode).ToList();

                    var list = new List<PremiumSpreadAgeRange>();
                    if (premiumSpreadTableDetailBo.AgeFrom.HasValue && premiumSpreadTableDetailBo.AgeTo.HasValue)
                    {
                        list = ageRanges.Where(q =>
                            (
                                q.AgeFrom <= premiumSpreadTableDetailBo.AgeFrom && q.AgeTo >= premiumSpreadTableDetailBo.AgeFrom
                                ||
                                q.AgeFrom <= premiumSpreadTableDetailBo.AgeTo && q.AgeTo >= premiumSpreadTableDetailBo.AgeTo
                            )
                            || (q.AgeFrom == null && q.AgeTo == null)
                            ).ToList();
                    }
                    else
                    {
                        list = ageRanges.Where(q => q.AgeFrom == null && q.AgeTo == null).ToList();
                    }

                    if (!list.IsNullOrEmpty())
                    {
                        foreach (var ageRange in list)
                        {
                            var cpcList = Util.ToArraySplitTrim(ageRange.CedingPlanCode).ToList();
                            var bcList = Util.ToArraySplitTrim(ageRange.BenefitCode).ToList();

                            var cpcIntersect = cpcList.Intersect(cedingPlanCodes);
                            var bcIntersect = bcList.Intersect(benefitCodes);

                            if (cpcIntersect.Count() > 0 && string.IsNullOrEmpty(premiumSpreadTableDetailBo.BenefitCode))
                            {
                                int idx = premiumSpreadTableDetailBos.IndexOf(premiumSpreadTableDetailBo);
                                result.AddError(string.Format("Duplicate Premium Spread Found at row #{0}", idx + 1));
                                break;
                            }
                            else if (cpcIntersect.Count() > 0 && bcIntersect.Count() > 0)
                            {
                                int idx = premiumSpreadTableDetailBos.IndexOf(premiumSpreadTableDetailBo);
                                result.AddError(string.Format("Duplicate Premium Spread Found at row #{0}", idx + 1));
                                break;
                            }
                        }
                    }

                    ageRanges.Add(new PremiumSpreadAgeRange { AgeFrom = premiumSpreadTableDetailBo.AgeFrom, AgeTo = premiumSpreadTableDetailBo.AgeTo, CedingPlanCode = premiumSpreadTableDetailBo.CedingPlanCode, BenefitCode = premiumSpreadTableDetailBo.BenefitCode });
                }
                count++;
            }
        }

        public void ProcessPremiumSpreadTableDetails(List<PremiumSpreadTableDetailBo> premiumSpreadTableDetailBos, int authUserId, ref TrailObject trail)
        {
            List<int> savedIds = new List<int> { };

            foreach (PremiumSpreadTableDetailBo bo in premiumSpreadTableDetailBos)
            {
                PremiumSpreadTableDetailBo premiumSpreadTableDetailBo = bo;
                premiumSpreadTableDetailBo.PremiumSpreadTableId = Id;
                premiumSpreadTableDetailBo.CreatedById = authUserId;
                premiumSpreadTableDetailBo.UpdatedById = authUserId;

                PremiumSpreadTableDetailService.Save(ref premiumSpreadTableDetailBo, ref trail);
                savedIds.Add(premiumSpreadTableDetailBo.Id);
            }
            PremiumSpreadTableDetailService.DeleteByPremiumSpreadTableIdExcept(Id, savedIds, ref trail);
        }
    }
}