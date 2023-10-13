using BusinessObject;
using BusinessObject.Retrocession;
using DataAccess.Entities;
using DataAccess.Entities.Retrocession;
using Services;
using Services.Retrocession;
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
    public class RetroBenefitCodeMappingViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required, DisplayName("MLRe Benefit Code")]
        public int BenefitId { get; set; }

        public Benefit Benefit { get; set; }

        public BenefitBo BenefitBo { get; set; }

        [Required, DisplayName("PerAnnum")]
        public bool IsPerAnnum { get; set; }

        [DisplayName("Treaty Code")]
        public string TreatyCode { get; set; }

        public virtual ICollection<RetroBenefitCodeMappingTreaty> RetroBenefitCodeMappingTreaties { get; set; }

        public virtual ICollection<RetroBenefitCodeMappingDetail> RetroBenefitCodeMappingDetails { get; set; }

        public RetroBenefitCodeMappingViewModel() { }

        public RetroBenefitCodeMappingViewModel(RetroBenefitCodeMappingBo retroBenefitCodeMappingBo)
        {
            Set(retroBenefitCodeMappingBo);
        }

        public void Set(RetroBenefitCodeMappingBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                BenefitId = bo.BenefitId;
                BenefitBo = bo.BenefitBo;
                IsPerAnnum = bo.IsPerAnnum;
                TreatyCode = bo.TreatyCode;
            }
        }

        public RetroBenefitCodeMappingBo FormBo(int createdById, int updatedById)
        {
            return new RetroBenefitCodeMappingBo
            {
                Id = Id,
                BenefitId = BenefitId,
                BenefitBo = BenefitService.Find(BenefitId),
                IsPerAnnum = IsPerAnnum,
                TreatyCode = TreatyCode,
                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<RetroBenefitCodeMapping, RetroBenefitCodeMappingViewModel>> Expression()
        {
            return entity => new RetroBenefitCodeMappingViewModel
            {
                Id = entity.Id,
                BenefitId = entity.BenefitId,
                Benefit = entity.Benefit,
                IsPerAnnum = entity.IsPerAnnum,
                RetroBenefitCodeMappingTreaties = entity.RetroBenefitCodeMappingTreaties,
                RetroBenefitCodeMappingDetails = entity.RetroBenefitCodeMappingDetails,
            };
        }

        public List<RetroBenefitCodeMappingDetailBo> GetRetroBenefitCodeMappingDetails(FormCollection form, ref Result result)
        {
            int index = 0;
            int maxIndex = int.Parse(form.Get("retroBenefitCodeMappingDetailMaxIndex"));
            List<RetroBenefitCodeMappingDetailBo> retroBenefitCodeMappingDetailBos = new List<RetroBenefitCodeMappingDetailBo> { };

            while (index <= maxIndex)
            {
                string retroBenefitCodeId = form.Get(string.Format("retroBenefitCodeId[{0}]", index));
                string isComputePremium = form.Get(string.Format("isComputePremium[{0}]", index));
                string id = form.Get(string.Format("retroBenefitRetentionLimitDetailId[{0}]", index));

                int retroBenefitCodeMappingDetailId = 0;
                if (!string.IsNullOrEmpty(id))
                    retroBenefitCodeMappingDetailId = int.Parse(id);

                if (string.IsNullOrEmpty(retroBenefitCodeId) &&
                    string.IsNullOrEmpty(isComputePremium) && //Always have value
                    retroBenefitCodeMappingDetailId == 0)
                {
                    index++;
                    continue;
                }

                RetroBenefitCodeMappingDetailBo retroBenefitCodeMappingDetailBo = new RetroBenefitCodeMappingDetailBo
                {
                    RetroBenefitCodeMappingId = Id,
                    RetroBenefitCodeId = Util.GetParseInt(retroBenefitCodeId) ?? 0,
                    IsComputePremium = bool.Parse(isComputePremium),
                };

                if (retroBenefitCodeMappingDetailBo.RetroBenefitCodeId == 0)
                {
                    result.AddError(string.Format("Retro Benefit Code is required at row #{0}", index + 1));
                }

                if (retroBenefitCodeMappingDetailId != 0)
                {
                    retroBenefitCodeMappingDetailBo.Id = retroBenefitCodeMappingDetailId;
                }

                retroBenefitCodeMappingDetailBos.Add(retroBenefitCodeMappingDetailBo);
                index++;
            }

            if (retroBenefitCodeMappingDetailBos.IsNullOrEmpty())
            {
                result.AddError("At least one Retro Benefit Code is required");
            }

            return retroBenefitCodeMappingDetailBos;
        }

        public void ValidateDuplicate(List<RetroBenefitCodeMappingDetailBo> retroBenefitCodeMappingDetailBos, ref Result result)
        {
            int count = 1;
            List<RetroBenefitCodeMappingDetailBo> tempBos = new List<RetroBenefitCodeMappingDetailBo> { };

            foreach (var retroBenefitCodeMappingDetailBo in retroBenefitCodeMappingDetailBos)
            {
                if (count == 1)
                {
                    tempBos.Add(retroBenefitCodeMappingDetailBo);
                    count++;
                    continue;
                }

                if (tempBos.Where(q => q.RetroBenefitCodeId == retroBenefitCodeMappingDetailBo.RetroBenefitCodeId).Any())
                {
                    int idx = retroBenefitCodeMappingDetailBos.IndexOf(retroBenefitCodeMappingDetailBo);
                    result.AddError(string.Format("Duplicate Data Found at row #{0}", idx + 1));
                }

                tempBos.Add(retroBenefitCodeMappingDetailBo);
                count++;
            }
        }

        public void ProcessRetroBenefitCodeMappingDetails(List<RetroBenefitCodeMappingDetailBo> retroBenefitCodeMappingDetailBos, int authUserId, ref TrailObject trail)
        {
            List<int> savedIds = new List<int> { };

            foreach (RetroBenefitCodeMappingDetailBo bo in retroBenefitCodeMappingDetailBos)
            {
                RetroBenefitCodeMappingDetailBo retroBenefitCodeMappingDetailBo = bo;
                retroBenefitCodeMappingDetailBo.RetroBenefitCodeMappingId = Id;
                retroBenefitCodeMappingDetailBo.CreatedById = authUserId;
                retroBenefitCodeMappingDetailBo.UpdatedById = authUserId;

                RetroBenefitCodeMappingDetailService.Save(ref retroBenefitCodeMappingDetailBo, ref trail);
                savedIds.Add(retroBenefitCodeMappingDetailBo.Id);
            }
            RetroBenefitCodeMappingDetailService.DeleteByRetroBenefitCodeMappingIdExcept(Id, savedIds, ref trail);
        }

        public void ProcessRetroBenefitCodeMappingTreaties(int authUserId, ref TrailObject trail)
        {
            RetroBenefitCodeMappingTreatyService.DeleteByRetroBenefitCodeMappingId(Id, ref trail);

            if (IsPerAnnum)
            {
                var treatyCodeArr = TreatyCode.ToArraySplitTrim();

                foreach (var treatyCode in treatyCodeArr)
                {
                    var bo = new RetroBenefitCodeMappingTreatyBo()
                    {
                        RetroBenefitCodeMappingId = Id,
                        TreatyCode = treatyCode,
                        CreatedById = authUserId,
                        UpdatedById = authUserId,
                    };
                    RetroBenefitCodeMappingTreatyService.Create(ref bo, ref trail);
                }
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (IsPerAnnum)
            {
                if (string.IsNullOrEmpty(TreatyCode))
                {
                    results.Add(new ValidationResult(
                        string.Format(MessageBag.Required, "Treaty Code"),
                        new[] { nameof(TreatyCode) }
                    ));
                }
            }

            return results;
        }
    }
}