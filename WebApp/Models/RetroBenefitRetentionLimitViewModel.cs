using BusinessObject.Retrocession;
using DataAccess.Entities.Retrocession;
using PagedList;
using Services.Retrocession;
using Shared;
using Shared.DataAccess;
using Shared.Forms.Attributes;
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
    public class RetroBenefitRetentionLimitViewModel : IValidatableObject
    {

        //Search criteria
        //public RetroBenefitRetentionLimitListingViewModel Individual { get; set; }
        //public RetroBenefitRetentionLimitListingViewModel Group { get; set; }

        ////Search results
        //public int? SearchResultsIndividual { get; set; }
        //public int? SearchResultsGroup { get; set; }
        //public IPagedList<RetroBenefitRetentionLimitListingViewModel> Individuals { get; set; }
        //public IPagedList<RetroBenefitRetentionLimitListingViewModel> Groups { get; set; }
        public int? ActiveTab { get; set; }

        public int Id { get; set; }

        [Required, DisplayName("Retro Benefit")]
        public int RetroBenefitCodeId { get; set; }

        public RetroBenefitCode RetroBenefitCode { get; set; }

        public RetroBenefitCodeBo RetroBenefitCodeBo { get; set; }

        [Required]
        public int Type { get; set; }

        [Required, StringLength(255), DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Effective Start Date")]
        public DateTime? EffectiveStartDate { get; set; }

        [Required, DisplayName("Effective Start Date")]
        [ValidateDate]
        public string EffectiveStartDateStr { get; set; }

        [DisplayName("Effective End Date")]
        public DateTime? EffectiveEndDate { get; set; }

        [Required, DisplayName("Effective End Date")]
        [ValidateDate]
        public string EffectiveEndDateStr { get; set; }

        [DisplayName("Minimum Retention Limit")]
        public double MinRetentionLimit { get; set; }

        [Required, DisplayName("Minimum Retention Limit")]
        [ValidateDouble]
        public string MinRetentionLimitStr { get; set; }

        // Detail

        [DisplayName("Reinsurance Effective Start Date")]
        public DateTime? ReinsEffStartDate { get; set; }

        [DisplayName("Reinsurance Effective Start Date")]
        public string ReinsEffStartDateStr { get; set; }

        [DisplayName("Reinsurance Effective End Date")]
        public DateTime? ReinsEffEndDate { get; set; }

        [DisplayName("Reinsurance Effective End Date")]
        public string ReinsEffEndDateStr { get; set; }

        [DisplayName("Minimum Issue Age")]
        public int? MinIssueAge { get; set; }
        [DisplayName("Maximum Issue Age")]
        public int? MaxIssueAge { get; set; }

        [DisplayName("Mortality Limit From")]
        public double? MortalityLimitFrom { get; set; }
        [DisplayName("Mortality Limit To")]
        public double? MortalityLimitTo { get; set; }

        [DisplayName("MLRe Retention (MYR)")]
        public double? MlreRetentionAmount { get; set; }

        [DisplayName("Minimum Reinsurance Amount (MYR)")]
        public double? MinReinsAmount { get; set; }

        public RetroBenefitRetentionLimitViewModel() { }

        public RetroBenefitRetentionLimitViewModel(RetroBenefitRetentionLimitBo retroBenefitRetentionLimitBo)
        {
            Set(retroBenefitRetentionLimitBo);
        }

        public void Set(RetroBenefitRetentionLimitBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                RetroBenefitCodeId = bo.RetroBenefitCodeId;
                RetroBenefitCodeBo = bo.RetroBenefitCodeBo;
                Type = bo.Type;
                Description = bo.Description;

                EffectiveStartDate = bo.EffectiveStartDate;
                EffectiveStartDateStr = bo.EffectiveStartDate.ToString(Util.GetDateFormat());

                EffectiveEndDate = bo.EffectiveEndDate;
                EffectiveEndDateStr = bo.EffectiveEndDate.ToString(Util.GetDateFormat());

                MinRetentionLimit = bo.MinRetentionLimit;
                MinRetentionLimitStr = Util.DoubleToString(bo.MinRetentionLimit);
            }
        }

        public RetroBenefitRetentionLimitBo FormBo(int createdById, int updatedById)
        {
            return new RetroBenefitRetentionLimitBo
            {
                RetroBenefitCodeId = RetroBenefitCodeId,
                RetroBenefitCodeBo = RetroBenefitCodeService.Find(RetroBenefitCodeId),
                Type = Type,
                Description = Description,

                EffectiveStartDate = DateTime.Parse(EffectiveStartDateStr),
                EffectiveEndDate = DateTime.Parse(EffectiveEndDateStr),

                MinRetentionLimit = Util.StringToDouble(MinRetentionLimitStr).Value,
                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<RetroBenefitRetentionLimit, RetroBenefitRetentionLimitViewModel>> Expression()
        {
            return entity => new RetroBenefitRetentionLimitViewModel
            {
                Id = entity.Id,
                RetroBenefitCodeId = entity.RetroBenefitCodeId,
                RetroBenefitCode = entity.RetroBenefitCode,
                Type = entity.Type,
                Description = entity.Description,
                EffectiveStartDate = entity.EffectiveStartDate,
                EffectiveEndDate = entity.EffectiveEndDate,
                MinRetentionLimit = entity.MinRetentionLimit,
            };
        }

        public static Expression<Func<RetroBenefitRetentionLimitWithDetail, RetroBenefitRetentionLimitViewModel>> ExpressionWithDetail()
        {
            return entity => new RetroBenefitRetentionLimitViewModel
            {
                Id = entity.RetroBenefitRetentionLimit.Id,
                RetroBenefitCodeId = entity.RetroBenefitRetentionLimit.RetroBenefitCodeId,
                RetroBenefitCode = entity.RetroBenefitRetentionLimit.RetroBenefitCode,
                Type = entity.RetroBenefitRetentionLimit.Type,
                Description = entity.RetroBenefitRetentionLimit.Description,
                EffectiveStartDate = entity.RetroBenefitRetentionLimit.EffectiveStartDate,
                EffectiveEndDate = entity.RetroBenefitRetentionLimit.EffectiveEndDate,
                MinRetentionLimit = entity.RetroBenefitRetentionLimit.MinRetentionLimit,

                // Detail
                ReinsEffStartDate = entity.RetroBenefitRetentionLimitDetail.ReinsEffStartDate,
                ReinsEffEndDate = entity.RetroBenefitRetentionLimitDetail.ReinsEffEndDate,
                MinIssueAge = entity.RetroBenefitRetentionLimitDetail.MinIssueAge,
                MaxIssueAge = entity.RetroBenefitRetentionLimitDetail.MaxIssueAge,
                MortalityLimitFrom = entity.RetroBenefitRetentionLimitDetail.MortalityLimitFrom,
                MortalityLimitTo = entity.RetroBenefitRetentionLimitDetail.MortalityLimitTo,
                MlreRetentionAmount = entity.RetroBenefitRetentionLimitDetail.MlreRetentionAmount,
                MinReinsAmount = entity.RetroBenefitRetentionLimitDetail.MinReinsAmount
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            DateTime? start = Util.GetParseDateTime(EffectiveStartDateStr);
            DateTime? end = Util.GetParseDateTime(EffectiveEndDateStr);

            if (start != null && end != null && end <= start)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.StartDateEarlier, "Effective"),
                    new[] { nameof(EffectiveStartDateStr) }));
                results.Add(new ValidationResult(
                string.Format(MessageBag.EndDateLater, "Effective"),
                new[] { nameof(EffectiveEndDateStr) }));
            }

            return results;
        }

        public List<RetroBenefitRetentionLimitDetailBo> GetRetroBenefitRetentionLimitDetails(FormCollection form, ref Result result)
        {
            int index = 0;
            int maxIndex = int.Parse(form.Get("retroBenefitRetentionLimitDetailMaxIndex"));
            List<RetroBenefitRetentionLimitDetailBo> retroBenefitRetentionLimitDetailBos = new List<RetroBenefitRetentionLimitDetailBo> { };

            while (index <= maxIndex)
            {
                string minIssueAge = form.Get(string.Format("minIssueAgeStr[{0}]", index));
                string maxIssueAge = form.Get(string.Format("maxIssueAgeStr[{0}]", index));
                string mortalityLimitFrom = form.Get(string.Format("mortalityLimitFromStr[{0}]", index));
                string mortalityLimitTo = form.Get(string.Format("mortalityLimitToStr[{0}]", index));
                string reinsEffStartDate = form.Get(string.Format("reinsEffStartDateStr[{0}]", index));
                string reinsEffEndDate = form.Get(string.Format("reinsEffEndDateStr[{0}]", index));
                string mlreRetentionAmount = form.Get(string.Format("mlreRetentionAmountStr[{0}]", index));
                string minReinsAmount = form.Get(string.Format("minReinsAmountStr[{0}]", index));
                string id = form.Get(string.Format("retroBenefitRetentionLimitDetailId[{0}]", index));

                int retroBenefitRetentionLimitDetailId = 0;
                if (!string.IsNullOrEmpty(id))
                    retroBenefitRetentionLimitDetailId = int.Parse(id);

                if (string.IsNullOrEmpty(minIssueAge) &&
                    string.IsNullOrEmpty(maxIssueAge) &&
                    string.IsNullOrEmpty(mortalityLimitFrom) &&
                    string.IsNullOrEmpty(mortalityLimitTo) &&
                    string.IsNullOrEmpty(reinsEffStartDate) &&
                    string.IsNullOrEmpty(reinsEffEndDate) &&
                    string.IsNullOrEmpty(mlreRetentionAmount) &&
                    string.IsNullOrEmpty(minReinsAmount) &&
                    retroBenefitRetentionLimitDetailId == 0)
                {
                    index++;
                    continue;
                }

                RetroBenefitRetentionLimitDetailBo retroBenefitRetentionLimitDetailBo = new RetroBenefitRetentionLimitDetailBo
                {
                    RetroBenefitRetentionLimitId = Id,
                    MinIssueAgeStr = minIssueAge,
                    MaxIssueAgeStr = maxIssueAge,
                    MortalityLimitFromStr = mortalityLimitFrom,
                    MortalityLimitToStr = mortalityLimitTo,
                    ReinsEffStartDateStr = reinsEffStartDate,
                    ReinsEffEndDateStr = reinsEffEndDate,
                    MlreRetentionAmountStr = mlreRetentionAmount,
                    MinReinsAmountStr = minReinsAmount,
                };

                Result validateResult = retroBenefitRetentionLimitDetailBo.Validate(index + 1);

                result.AddErrorRange(validateResult.ToErrorArray());

                if (retroBenefitRetentionLimitDetailId != 0)
                {
                    retroBenefitRetentionLimitDetailBo.Id = retroBenefitRetentionLimitDetailId;
                }

                retroBenefitRetentionLimitDetailBos.Add(retroBenefitRetentionLimitDetailBo);
                index++;
            }
            return retroBenefitRetentionLimitDetailBos;
        }

        public void ValidateDuplicate(List<RetroBenefitRetentionLimitDetailBo> retroBenefitRetentionLimitDetailBos, ref Result result)
        {
            int count = 1;
            List<RetroBenefitRetentionLimitDetailBo> tempBos = new List<RetroBenefitRetentionLimitDetailBo> { };

            foreach (var retroBenefitRetentionLimitDetailBo in retroBenefitRetentionLimitDetailBos)
            {
                if (count == 1)
                {
                    tempBos.Add(retroBenefitRetentionLimitDetailBo);
                    count++;
                    continue;
                }

                bool isduplicate = tempBos.Where(q =>
                        q.MinIssueAge <= retroBenefitRetentionLimitDetailBo.MinIssueAge && q.MaxIssueAge >= retroBenefitRetentionLimitDetailBo.MinIssueAge
                        ||
                        q.MinIssueAge <= retroBenefitRetentionLimitDetailBo.MaxIssueAge && q.MaxIssueAge >= retroBenefitRetentionLimitDetailBo.MaxIssueAge
                    )
                    .Where(q =>
                        q.ReinsEffStartDate <= retroBenefitRetentionLimitDetailBo.ReinsEffStartDate && q.ReinsEffEndDate >= retroBenefitRetentionLimitDetailBo.ReinsEffStartDate
                        ||
                        q.ReinsEffStartDate <= retroBenefitRetentionLimitDetailBo.ReinsEffEndDate && q.ReinsEffEndDate >= retroBenefitRetentionLimitDetailBo.ReinsEffEndDate
                    )
                    //.Where(q =>
                    //    q.MortalityLimitFrom <= retroBenefitRetentionLimitDetailBo.MortalityLimitFrom && q.MortalityLimitTo >= retroBenefitRetentionLimitDetailBo.MortalityLimitFrom
                    //    ||
                    //    q.MortalityLimitFrom <= retroBenefitRetentionLimitDetailBo.MortalityLimitTo && q.MortalityLimitTo >= retroBenefitRetentionLimitDetailBo.MortalityLimitTo
                    //)
                    .Where(q => q.MortalityLimitFrom == retroBenefitRetentionLimitDetailBo.MortalityLimitFrom)
                    .Where(q => q.MortalityLimitTo == retroBenefitRetentionLimitDetailBo.MortalityLimitTo)
                    .Any();

                if (isduplicate)
                {
                    int idx = retroBenefitRetentionLimitDetailBos.IndexOf(retroBenefitRetentionLimitDetailBo);
                    result.AddError(string.Format("Duplicate Data Found at row #{0}", idx + 1));
                }

                tempBos.Add(retroBenefitRetentionLimitDetailBo);
                count++;
            }
        }

        public void ProcessRetroBenefitRetentionLimitDetails(List<RetroBenefitRetentionLimitDetailBo> retroBenefitRetentionLimitDetailBos, int authUserId, ref TrailObject trail)
        {
            List<int> savedIds = new List<int> { };

            foreach (RetroBenefitRetentionLimitDetailBo bo in retroBenefitRetentionLimitDetailBos)
            {
                RetroBenefitRetentionLimitDetailBo retroBenefitRetentionLimitDetailBo = bo;
                retroBenefitRetentionLimitDetailBo.RetroBenefitRetentionLimitId = Id;
                retroBenefitRetentionLimitDetailBo.CreatedById = authUserId;
                retroBenefitRetentionLimitDetailBo.UpdatedById = authUserId;

                RetroBenefitRetentionLimitDetailService.Save(ref retroBenefitRetentionLimitDetailBo, ref trail);
                savedIds.Add(retroBenefitRetentionLimitDetailBo.Id);
            }
            RetroBenefitRetentionLimitDetailService.DeleteByRetroBenefitRetentionLimitIdExcept(Id, savedIds, ref trail);
        }
    }
}