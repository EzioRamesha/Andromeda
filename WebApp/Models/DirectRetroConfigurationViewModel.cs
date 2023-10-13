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
    public class DirectRetroConfigurationViewModel
    {
        public int Id { get; set; }

        [Required, DisplayName("Direct Retro Configuration")]
        public string Name { get; set; }

        [Required, DisplayName("Treaty Code")]
        public int TreatyCodeId { get; set; }

        public TreatyCode TreatyCode { get; set; }

        public TreatyCodeBo TreatyCodeBo { get; set; }

        [Required, DisplayName("Retro Party")]
        [ValidateRetroParty]
        public string RetroParty { get; set; }

        public virtual ICollection<DirectRetroConfigurationMapping> DirectRetroConfigurationMappings { get; set; }

        public DirectRetroConfigurationViewModel() { }

        public DirectRetroConfigurationViewModel(DirectRetroConfigurationBo directRetroConfigurationBo)
        {
            Set(directRetroConfigurationBo);
        }

        public void Set(DirectRetroConfigurationBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                Name = bo.Name;
                TreatyCodeId = bo.TreatyCodeId;
                TreatyCodeBo = bo.TreatyCodeBo;
                RetroParty = bo.RetroParty;
            }
        }

        public DirectRetroConfigurationBo FormBo(int createdById, int updatedById)
        {
            var bo = new DirectRetroConfigurationBo
            {
                Name = Name?.Trim(),
                TreatyCodeId = TreatyCodeId,
                TreatyCodeBo = TreatyCodeService.Find(TreatyCodeId),
                RetroParty = RetroParty,
                CreatedById = createdById,
                UpdatedById = updatedById,
            };

            return bo;
        }

        public static Expression<Func<DirectRetroConfiguration, DirectRetroConfigurationViewModel>> Expression()
        {
            return entity => new DirectRetroConfigurationViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                TreatyCodeId = entity.TreatyCodeId,
                TreatyCode = entity.TreatyCode,
                RetroParty = entity.RetroParty,

                DirectRetroConfigurationMappings = entity.DirectRetroConfigurationMappings,
            };
        }

        public List<DirectRetroConfigurationDetailBo> GetDirectRetroConfigurationDetails(FormCollection form, ref Result result)
        {
            int index = 0;
            int maxIndex = int.Parse(form.Get("directRetroConfigurationDetailMaxIndex"));
            List<DirectRetroConfigurationDetailBo> directReetroConfigurationDetailBos = new List<DirectRetroConfigurationDetailBo> { };

            while (index <= maxIndex)
            {
                string riskPeriodStartDateStr = form.Get(string.Format("riskPeriodStartDateStr[{0}]", index));
                string riskPeriodEndDateStr = form.Get(string.Format("riskPeriodEndDateStr[{0}]", index));
                string issueDatePolStartDateStr = form.Get(string.Format("issueDatePolStartDateStr[{0}]", index));
                string issueDatePolEndDateStr = form.Get(string.Format("issueDatePolEndDateStr[{0}]", index));
                string reinsEffDatePolStartDateStr = form.Get(string.Format("reinsEffDatePolStartDateStr[{0}]", index));
                string reinsEffDatePolEndDateStr = form.Get(string.Format("reinsEffDatePolEndDateStr[{0}]", index));
                string isDefault = form.Get(string.Format("isDefault[{0}]", index));
                string retroPartyId = form.Get(string.Format("retroPartyId[{0}]", index));
                string treatyNo = form.Get(string.Format("treatyNo[{0}]", index))?.Trim();
                string schedule = form.Get(string.Format("schedule[{0}]", index))?.Trim();
                string shareStr = form.Get(string.Format("shareStr[{0}]", index));
                string premiumSpreadTableId = form.Get(string.Format("premiumSpreadTableId[{0}]", index));
                string treatyDiscountTableId = form.Get(string.Format("treatyDiscountTableId[{0}]", index));
                string id = form.Get(string.Format("directRetroConfigurationDetailId[{0}]", index));

                int directRetroConfigurationDetailId = 0;
                if (!string.IsNullOrEmpty(id))
                    directRetroConfigurationDetailId = int.Parse(id);

                if (string.IsNullOrEmpty(riskPeriodStartDateStr) &&
                    string.IsNullOrEmpty(riskPeriodEndDateStr) &&
                    string.IsNullOrEmpty(issueDatePolStartDateStr) &&
                    string.IsNullOrEmpty(issueDatePolEndDateStr) &&
                    string.IsNullOrEmpty(reinsEffDatePolStartDateStr) &&
                    string.IsNullOrEmpty(reinsEffDatePolEndDateStr) &&
                    //string.IsNullOrEmpty(isDefault) && //Remove from validation due to always have value
                    (string.IsNullOrEmpty(retroPartyId) || retroPartyId == "0") &&
                    string.IsNullOrEmpty(treatyNo) &&
                    string.IsNullOrEmpty(schedule) &&
                    string.IsNullOrEmpty(shareStr) &&
                    (string.IsNullOrEmpty(premiumSpreadTableId) || premiumSpreadTableId == "0") &&
                    (string.IsNullOrEmpty(treatyDiscountTableId) || treatyDiscountTableId == "0") &&
                    directRetroConfigurationDetailId == 0)
                {
                    index++;
                    continue;
                }

                DirectRetroConfigurationDetailBo directRetroConfigurationDetailBo = new DirectRetroConfigurationDetailBo
                {
                    DirectRetroConfigurationId = Id,
                    RiskPeriodStartDateStr = riskPeriodStartDateStr,
                    RiskPeriodEndDateStr = riskPeriodEndDateStr,
                    IssueDatePolStartDateStr = issueDatePolStartDateStr,
                    IssueDatePolEndDateStr = issueDatePolEndDateStr,
                    ReinsEffDatePolStartDateStr = reinsEffDatePolStartDateStr,
                    ReinsEffDatePolEndDateStr = reinsEffDatePolEndDateStr,

                    RiskPeriodStartDate = Util.GetParseDateTime(riskPeriodStartDateStr),
                    RiskPeriodEndDate = Util.GetParseDateTime(riskPeriodEndDateStr),
                    IssueDatePolStartDate = Util.GetParseDateTime(issueDatePolStartDateStr),
                    IssueDatePolEndDate = Util.GetParseDateTime(issueDatePolEndDateStr),
                    ReinsEffDatePolStartDate = Util.GetParseDateTime(reinsEffDatePolStartDateStr),
                    ReinsEffDatePolEndDate = Util.GetParseDateTime(reinsEffDatePolEndDateStr),

                    IsDefault = bool.Parse(isDefault),
                    RetroPartyId = Util.GetParseInt(retroPartyId) ?? 0,
                    TreatyNo = treatyNo,
                    Schedule = schedule,
                    ShareStr = shareStr,
                    PremiumSpreadTableId = Util.GetParseInt(premiumSpreadTableId),
                    TreatyDiscountTableId = Util.GetParseInt(treatyDiscountTableId),
                };

                Result validateResult = directRetroConfigurationDetailBo.Validate(index + 1);

                result.AddErrorRange(validateResult.ToErrorArray());

                if (result.Valid)
                {
                    directRetroConfigurationDetailBo.Share = Util.StringToDouble(directRetroConfigurationDetailBo.ShareStr).Value;
                }

                if (directRetroConfigurationDetailId != 0)
                {
                    directRetroConfigurationDetailBo.Id = directRetroConfigurationDetailId;
                }

                directReetroConfigurationDetailBos.Add(directRetroConfigurationDetailBo);
                index++;
            }
            return directReetroConfigurationDetailBos;
        }

        public void ValidateDuplicate(List<DirectRetroConfigurationDetailBo> directRetroConfigurationDetailBos, ref Result result)
        {
            List<DirectRetroConfigurationDetailBo> duplicates = directRetroConfigurationDetailBos.GroupBy(
                q => new
                {
                    q.IsDefault,
                    q.RetroPartyId,
                }).Where(g => g.Count() > 1)
                .Select(r => new DirectRetroConfigurationDetailBo
                {
                    IsDefault = r.Key.IsDefault,
                    RetroPartyId = r.Key.RetroPartyId
                }).ToList();

            foreach (DirectRetroConfigurationDetailBo duplicate in duplicates)
            {
                List<DirectRetroConfigurationDetailBo> bos = directRetroConfigurationDetailBos
                       .Where(q => q.IsDefault == duplicate.IsDefault)
                       .Where(q => q.RetroPartyId == duplicate.RetroPartyId)
                       .ToList();

                int count = 1;
                List<DirectRetroConfigurationDateRange> dateRanges = new List<DirectRetroConfigurationDateRange> { };

                foreach (DirectRetroConfigurationDetailBo bo in bos)
                {
                    if (count == 1)
                    {
                        dateRanges.Add(new DirectRetroConfigurationDateRange
                        {
                            RiskPeriodStartDate = bo.RiskPeriodStartDate,
                            RiskPeriodEndDate = bo.RiskPeriodEndDate,
                            IssueDatePolStartDate = bo.IssueDatePolStartDate,
                            IssueDatePolEndDate = bo.IssueDatePolEndDate,
                            ReinsEffDatePolStartDate = bo.ReinsEffDatePolStartDate,
                            ReinsEffDatePolEndDate = bo.ReinsEffDatePolEndDate,
                        });
                    }
                    else
                    {
                        foreach (DirectRetroConfigurationDateRange dateRange in dateRanges)
                        {
                            bool isRiskPeriodDate = false;
                            bool isIssueDatePolDate = false;
                            bool isReinsEffDatePolDate = false;

                            if (bo.RiskPeriodStartDate.HasValue && bo.RiskPeriodEndDate.HasValue)
                            {
                                if ((bo.RiskPeriodStartDate <= dateRange.RiskPeriodStartDate && bo.RiskPeriodEndDate >= dateRange.RiskPeriodStartDate) ||
                                (bo.RiskPeriodStartDate <= dateRange.RiskPeriodEndDate && bo.RiskPeriodEndDate >= dateRange.RiskPeriodEndDate) ||
                                dateRange.RiskPeriodStartDate == null && dateRange.RiskPeriodEndDate == null)
                                    isRiskPeriodDate = true;
                            } else
                            {
                                if (dateRange.RiskPeriodStartDate.HasValue && dateRange.RiskPeriodEndDate.HasValue || !dateRange.RiskPeriodStartDate.HasValue && !dateRange.RiskPeriodEndDate.HasValue)
                                    isRiskPeriodDate = true;
                            }

                            if (bo.IssueDatePolStartDate.HasValue && bo.IssueDatePolEndDate.HasValue)
                            {
                                if ((bo.IssueDatePolStartDate <= dateRange.IssueDatePolStartDate && bo.IssueDatePolEndDate >= dateRange.IssueDatePolStartDate) ||
                                (bo.IssueDatePolStartDate <= dateRange.IssueDatePolEndDate && bo.IssueDatePolEndDate >= dateRange.IssueDatePolEndDate) ||
                                dateRange.IssueDatePolStartDate == null && dateRange.IssueDatePolEndDate == null)
                                    isIssueDatePolDate = true;
                            } else
                            {
                                if (dateRange.IssueDatePolStartDate.HasValue && dateRange.IssueDatePolEndDate.HasValue || !dateRange.IssueDatePolStartDate.HasValue && !dateRange.IssueDatePolEndDate.HasValue)
                                    isIssueDatePolDate = true;
                            }

                            if (bo.ReinsEffDatePolStartDate.HasValue && bo.ReinsEffDatePolEndDate.HasValue)
                            {
                                if ((bo.ReinsEffDatePolStartDate <= dateRange.ReinsEffDatePolStartDate && bo.ReinsEffDatePolEndDate >= dateRange.ReinsEffDatePolStartDate) ||
                                (bo.ReinsEffDatePolStartDate <= dateRange.ReinsEffDatePolEndDate && bo.ReinsEffDatePolEndDate >= dateRange.ReinsEffDatePolEndDate) ||
                                dateRange.ReinsEffDatePolStartDate == null && dateRange.ReinsEffDatePolEndDate == null)
                                    isIssueDatePolDate = true;
                            } else
                            {
                                if (dateRange.ReinsEffDatePolStartDate.HasValue && dateRange.ReinsEffDatePolEndDate.HasValue || !dateRange.ReinsEffDatePolStartDate.HasValue && !dateRange.ReinsEffDatePolEndDate.HasValue)
                                    isReinsEffDatePolDate = true;
                            }

                            if (isRiskPeriodDate && isIssueDatePolDate && isReinsEffDatePolDate)
                            {
                                int idx = directRetroConfigurationDetailBos.IndexOf(bo);
                                result.AddError(string.Format("Duplicate Direct Retro Configuration Found at row #{0}", idx + 1));
                                break;
                            }

                            //if (((bo.RiskPeriodStartDate <= dateRange.RiskPeriodStartDate && bo.RiskPeriodEndDate >= dateRange.RiskPeriodStartDate) ||
                            //    (bo.RiskPeriodStartDate <= dateRange.RiskPeriodEndDate && bo.RiskPeriodEndDate >= dateRange.RiskPeriodEndDate)) &&
                            //    ((bo.IssueDatePolStartDate <= dateRange.IssueDatePolStartDate && bo.IssueDatePolEndDate >= dateRange.IssueDatePolStartDate) ||
                            //    (bo.IssueDatePolStartDate <= dateRange.IssueDatePolEndDate && bo.IssueDatePolEndDate >= dateRange.IssueDatePolEndDate)) &&
                            //    ((bo.ReinsEffDatePolStartDate <= dateRange.ReinsEffDatePolStartDate && bo.ReinsEffDatePolEndDate >= dateRange.ReinsEffDatePolStartDate) ||
                            //    (bo.ReinsEffDatePolStartDate <= dateRange.ReinsEffDatePolEndDate && bo.ReinsEffDatePolEndDate >= dateRange.ReinsEffDatePolEndDate))
                            //)
                            //{
                            //    int idx = directRetroConfigurationDetailBos.IndexOf(bo);
                            //    result.AddError(string.Format("Duplicate Direct Retro Configuration Found at row #{0}", idx + 1));
                            //    break;
                            //}
                        }
                        dateRanges.Add(new DirectRetroConfigurationDateRange
                        {
                            RiskPeriodStartDate = bo.RiskPeriodStartDate,
                            RiskPeriodEndDate = bo.RiskPeriodEndDate,
                            IssueDatePolStartDate = bo.IssueDatePolStartDate,
                            IssueDatePolEndDate = bo.IssueDatePolEndDate,
                            ReinsEffDatePolStartDate = bo.ReinsEffDatePolStartDate,
                            ReinsEffDatePolEndDate = bo.ReinsEffDatePolEndDate,
                        });
                    }
                    count++;
                }
            }
        }

        public void ProcessDirectRetroConfigurationDetails(List<DirectRetroConfigurationDetailBo> directRetroConfigurationDetailBos, int authUserId, ref TrailObject trail)
        {
            List<int> savedIds = new List<int> { };

            foreach (DirectRetroConfigurationDetailBo bo in directRetroConfigurationDetailBos)
            {
                DirectRetroConfigurationDetailBo directRetroConfigurationDetailBo = bo;
                directRetroConfigurationDetailBo.DirectRetroConfigurationId = Id;
                directRetroConfigurationDetailBo.CreatedById = authUserId;
                directRetroConfigurationDetailBo.UpdatedById = authUserId;

                DirectRetroConfigurationDetailService.Save(ref directRetroConfigurationDetailBo, ref trail);
                savedIds.Add(directRetroConfigurationDetailBo.Id);
            }
            DirectRetroConfigurationDetailService.DeleteByDirectRetroConfigurationIdExcept(Id, savedIds, ref trail);
        }

        public class ValidateRetroParty : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value != null)
                {
                    string retroParty = value.ToString();
                    string[] resultsArray = retroParty.ToArraySplitTrim();
                    IList<RetroPartyBo> retroPartyBos = RetroPartyService.Get();
                    List<string> retroParties = new List<string>();
                    if (retroPartyBos != null)
                    {
                        foreach (RetroPartyBo retroPartyBo in retroPartyBos)
                        {
                            retroParties.Add(retroPartyBo.Party);
                        }
                    }
                    foreach (string result in resultsArray)
                    {
                        if (!retroParties.Contains(result))
                        {
                            return new ValidationResult("Please Enter Valid Retro Party's Party.");
                        }
                    }
                }
                return ValidationResult.Success;
            }
        }
    }

    public class ValidateRetroParty : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string[] retroParties = value.ToString().ToArraySplitTrim();
                foreach (var retroParty in retroParties)
                {
                    if (RetroPartyService.CountByPartyStatus(retroParty, RetroPartyBo.StatusActive) == 0)
                    {
                        return new ValidationResult("Please enter valid Retro Party's Party and the status is Active.");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}