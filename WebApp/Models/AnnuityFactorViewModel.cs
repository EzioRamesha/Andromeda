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
using System.Web;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class AnnuityFactorViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required, DisplayName("Ceding Company")]
        public int CedantId { get; set; }

        public Cedant Cedant { get; set; }

        public CedantBo CedantBo { get; set; }

        [Required, DisplayName("Ceding Plan Code")]
        public string CedingPlanCode { get; set; }

        [DisplayName("Reinsurance Effective Start Date")]
        public DateTime? ReinsEffDatePolStartDate { get; set; }

        [Required, DisplayName("Reinsurance Effective Start Date")]
        public string ReinsEffDatePolStartDateStr { get; set; }

        [DisplayName("Reinsurance Effective End Date")]
        public DateTime? ReinsEffDatePolEndDate { get; set; }

        [Required, DisplayName("Reinsurance Effective End Date")]
        public string ReinsEffDatePolEndDateStr { get; set; }

        public virtual ICollection<AnnuityFactorMapping> AnnuityFactorMappings { get; set; }

        public AnnuityFactorViewModel() { }

        public AnnuityFactorViewModel(AnnuityFactorBo annuityFactorBo)
        {
            Set(annuityFactorBo);
        }

        public void Set(AnnuityFactorBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                CedantId = bo.CedantId;
                CedantBo = bo.CedantBo;
                CedingPlanCode = bo.CedingPlanCode;
                ReinsEffDatePolStartDate = bo.ReinsEffDatePolStartDate;
                ReinsEffDatePolStartDateStr = bo.ReinsEffDatePolStartDate?.ToString(Util.GetDateFormat());
                ReinsEffDatePolEndDate = bo.ReinsEffDatePolEndDate;
                ReinsEffDatePolEndDateStr = bo.ReinsEffDatePolEndDate?.ToString(Util.GetDateFormat());
            }
        }

        public AnnuityFactorBo FormBo(int createdById, int updatedById)
        {
            var bo = new AnnuityFactorBo
            {
                CedantId = CedantId,
                CedantBo = CedantService.Find(CedantId),
                CedingPlanCode = CedingPlanCode,
                ReinsEffDatePolStartDate = ReinsEffDatePolStartDate,
                ReinsEffDatePolEndDate = ReinsEffDatePolEndDate,
                CreatedById = createdById,
                UpdatedById = updatedById,
            };

            if (!string.IsNullOrEmpty(ReinsEffDatePolStartDateStr))
            {
                bo.ReinsEffDatePolStartDate = DateTime.Parse(ReinsEffDatePolStartDateStr);
                if (!string.IsNullOrEmpty(ReinsEffDatePolEndDateStr))
                {
                    bo.ReinsEffDatePolEndDate = DateTime.Parse(ReinsEffDatePolEndDateStr);
                }
                else
                {
                    bo.ReinsEffDatePolEndDate = DateTime.Parse(Util.GetDefaultEndDate());
                }
            }

            return bo;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            DateTime? start = Util.GetParseDateTime(ReinsEffDatePolStartDateStr);
            DateTime? end = Util.GetParseDateTime(ReinsEffDatePolEndDateStr);

            if (end != null && start == null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "The Reinsurance Effective"),
                    new[] { nameof(ReinsEffDatePolStartDateStr) }));
            }
            else if (start != null && end == null)
            {
                if (DateTime.Parse(Util.GetDefaultEndDate()) <= start)
                {
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.EndDefaultDate, "Reinsurance Effective", Util.GetDefaultEndDate()),
                    new[] { nameof(ReinsEffDatePolStartDateStr) }));
                }
            }
            else if (start != null && end != null)
            {
                if (end <= start)
                {
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.StartDateEarlier, "Reinsurance Effective"),
                    new[] { nameof(ReinsEffDatePolStartDateStr) }));
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.EndDateLater, "Reinsurance Effective"),
                    new[] { nameof(ReinsEffDatePolEndDateStr) }));
                }
            }

            return results;
        }

        public static Expression<Func<AnnuityFactor, AnnuityFactorViewModel>> Expression()
        {
            return entity => new AnnuityFactorViewModel
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                Cedant = entity.Cedant,
                CedingPlanCode = entity.CedingPlanCode,

                ReinsEffDatePolStartDate = entity.ReinsEffDatePolStartDate,
                ReinsEffDatePolEndDate = entity.ReinsEffDatePolEndDate,

                AnnuityFactorMappings = entity.AnnuityFactorMappings,
            };
        }

        public List<AnnuityFactorDetailBo> GetAnnuityFactorDetails(FormCollection form, ref Result result)
        {
            int index = 0;
            int maxIndex = int.Parse(form.Get("annuityFactorDetailMaxIndex"));
            List<AnnuityFactorDetailBo> annuityFactorDetailBos = new List<AnnuityFactorDetailBo> { };

            while (index <= maxIndex)
            {
                string policyTermRemainStr = form.Get(string.Format("policyTermRemainStr[{0}]", index));
                string genderCode = form.Get(string.Format("genderCode[{0}]", index));
                string insuredTobaccoUse = form.Get(string.Format("insuredTobaccoUse[{0}]", index));
                string insuredAttainedAge = form.Get(string.Format("insuredAttainedAge[{0}]", index));
                string policyTermStr = form.Get(string.Format("policyTermStr[{0}]", index));
                string annuityFactorValueStr = form.Get(string.Format("annuityFactorValueStr[{0}]", index));
                string id = form.Get(string.Format("annuityFactorDetailId[{0}]", index));

                int annuityFactorDetailId = 0;
                if (!string.IsNullOrEmpty(id))
                    annuityFactorDetailId = int.Parse(id);

                if (string.IsNullOrEmpty(policyTermRemainStr) &&
                    string.IsNullOrEmpty(genderCode) &&
                    string.IsNullOrEmpty(insuredTobaccoUse) &&
                    string.IsNullOrEmpty(insuredAttainedAge) &&
                    string.IsNullOrEmpty(policyTermStr) &&
                    string.IsNullOrEmpty(annuityFactorValueStr) &&
                    annuityFactorDetailId == 0)
                {
                    index++;
                    continue;
                }

                double? termRemain = null;
                double? policyTerm = null;
                double? annuityFactorValue = null;

                //if (string.IsNullOrEmpty(policyTermRemainStr) || string.IsNullOrWhiteSpace(policyTermRemainStr))
                //{
                //    result.AddError(string.Format("Term Remain is required at row #{0}", index + 1));
                //}
                //else if (Util.IsValidDouble(policyTermRemainStr, out double? d, out _))
                //{
                //    termRemain = d;
                //}
                //else
                //{
                //    result.AddError(string.Format("Term Remain format is not valid at row #{0}", index + 1));
                //}

                if (!string.IsNullOrEmpty(policyTermRemainStr) && !string.IsNullOrWhiteSpace(policyTermRemainStr))
                {
                    if (Util.IsValidDouble(policyTermRemainStr, out double? d, out _))
                        termRemain = d;
                    else
                        result.AddError(string.Format("Term Remain format is not valid at row #{0}", index + 1));
                }

                if (!string.IsNullOrEmpty(policyTermStr) && !string.IsNullOrWhiteSpace(policyTermStr))
                {
                    if (Util.IsValidDouble(policyTermStr, out double? d, out _))
                        policyTerm = d;
                    else
                        result.AddError(string.Format("Policy Term format is not valid at row #{0}", index + 1));
                }

                if (string.IsNullOrEmpty(annuityFactorValueStr) || string.IsNullOrWhiteSpace(annuityFactorValueStr))
                {
                    result.AddError(string.Format("Annuity Factor is required at row #{0}", index + 1));
                }
                else if (Util.IsValidDouble(annuityFactorValueStr, out double? d, out _))
                {
                    annuityFactorValue = d;
                }
                else
                {
                    result.AddError(string.Format("Annuity Factor format is not valid at row #{0}", index + 1));
                }

                AnnuityFactorDetailBo annuityFactorDetailBo = new AnnuityFactorDetailBo
                {
                    AnnuityFactorId = Id,
                    PolicyTermRemain = termRemain ?? null,
                    PolicyTermRemainStr = policyTermRemainStr,
                    InsuredGenderCodePickListDetailId = Util.GetParseInt(genderCode),
                    InsuredTobaccoUsePickListDetailId = Util.GetParseInt(insuredTobaccoUse),
                    InsuredAttainedAge = Util.GetParseInt(insuredAttainedAge),
                    PolicyTerm = policyTerm ?? null,
                    PolicyTermStr = policyTermStr,
                    AnnuityFactorValue = annuityFactorValue ?? 0,
                    AnnuityFactorValueStr = annuityFactorValueStr,
                };

                if (annuityFactorDetailId != 0)
                {
                    annuityFactorDetailBo.Id = annuityFactorDetailId;
                }

                annuityFactorDetailBos.Add(annuityFactorDetailBo);
                index++;
            }
            return annuityFactorDetailBos;
        }

        public void ValidateDuplicate(List<AnnuityFactorDetailBo> annuityFactorDetailBos, ref Result result)
        {
            int count = 1;
            List<AnnuityFactorDetailBo> tempBos = new List<AnnuityFactorDetailBo> { };

            foreach (var annuityFactorDetailBo in annuityFactorDetailBos)
            {
                if (count == 1)
                {
                    tempBos.Add(annuityFactorDetailBo);
                    count++;
                    continue;
                }

                var query = tempBos.AsEnumerable();

                if (annuityFactorDetailBo.PolicyTermRemain.HasValue)
                {
                    query = query.Where(q => q.PolicyTermRemain == annuityFactorDetailBo.PolicyTermRemain || q.PolicyTermRemain == null);
                }
                else
                {
                    query = query.Where(q => q.PolicyTermRemain.HasValue || !q.PolicyTermRemain.HasValue);
                }

                if (annuityFactorDetailBo.InsuredGenderCodePickListDetailId.HasValue)
                {
                    query = query.Where(q => q.InsuredGenderCodePickListDetailId == annuityFactorDetailBo.InsuredGenderCodePickListDetailId || q.InsuredGenderCodePickListDetailId == null);
                }
                else
                {
                    query = query.Where(q => q.InsuredGenderCodePickListDetailId.HasValue || !q.InsuredGenderCodePickListDetailId.HasValue);
                }

                if (annuityFactorDetailBo.InsuredTobaccoUsePickListDetailId.HasValue)
                {
                    query = query.Where(q => q.InsuredTobaccoUsePickListDetailId == annuityFactorDetailBo.InsuredTobaccoUsePickListDetailId || q.InsuredTobaccoUsePickListDetailId == null);
                }
                else
                {
                    query = query.Where(q => q.InsuredTobaccoUsePickListDetailId.HasValue || !q.InsuredTobaccoUsePickListDetailId.HasValue);
                }

                if (annuityFactorDetailBo.InsuredAttainedAge.HasValue)
                {
                    query = query.Where(q => q.InsuredAttainedAge == annuityFactorDetailBo.InsuredAttainedAge || q.InsuredAttainedAge == null);
                }
                else
                {
                    query = query.Where(q => q.InsuredAttainedAge.HasValue || !q.InsuredAttainedAge.HasValue);
                }

                if (annuityFactorDetailBo.PolicyTerm.HasValue)
                {
                    query = query.Where(q => q.PolicyTerm == annuityFactorDetailBo.PolicyTerm || q.PolicyTerm == null);
                }
                else
                {
                    query = query.Where(q => q.PolicyTerm.HasValue || !q.PolicyTerm.HasValue);
                }

                if (query.Any())
                {
                    int idx = annuityFactorDetailBos.IndexOf(annuityFactorDetailBo);
                    result.AddError(string.Format("Duplicate Data Found at row #{0}", idx + 1));
                }

                tempBos.Add(annuityFactorDetailBo);
                count++;
            }
        }

        public void ProcessAnnuityFactorDetails(List<AnnuityFactorDetailBo> annuityFactorDetailBos, int authUserId, ref TrailObject trail)
        {
            List<int> savedIds = new List<int> { };

            foreach (AnnuityFactorDetailBo bo in annuityFactorDetailBos)
            {
                AnnuityFactorDetailBo annuityFactorDetailBo = bo;
                annuityFactorDetailBo.AnnuityFactorId = Id;
                annuityFactorDetailBo.CreatedById = authUserId;
                annuityFactorDetailBo.UpdatedById = authUserId;

                AnnuityFactorDetailService.Save(ref annuityFactorDetailBo, ref trail);
                savedIds.Add(annuityFactorDetailBo.Id);
            }
            AnnuityFactorDetailService.DeleteByAnnuityFactorIdExcept(Id, savedIds, ref trail);
        }
    }
}