using BusinessObject;
using DataAccess.Entities;
using Newtonsoft.Json;
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
    public class RateViewModel
    {
        public int Id { get; set; }

        [Required, DisplayName("Rate Table Code"), StringLength(50)]
        public string Code { get; set; }

        [Required, DisplayName("Valuation Rate Table")]
        public int ValuationRate { get; set; }

        [Required, DisplayName("Rate Per Basis Table")]
        public int RatePerBasis { get; set; }

        public RateViewModel() { }

        public RateViewModel(RateBo rateBo)
        {
            Set(rateBo);
        }

        public void Set(RateBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                Code = bo.Code;
                ValuationRate = bo.ValuationRate;
                RatePerBasis = bo.RatePerBasis;
            }
        }

        public RateBo FormBo(int createdById, int updatedById)
        {
            return new RateBo
            {
                Code = Code?.Trim(),
                ValuationRate = ValuationRate,
                RatePerBasis = RatePerBasis,
                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<Rate, RateViewModel>> Expression()
        {
            return entity => new RateViewModel
            {
                Id = entity.Id,
                Code = entity.Code,
                ValuationRate = entity.ValuationRate,
                RatePerBasis = entity.RatePerBasis,
            };
        }

        public List<RateDetailBo> GetRateDetails(FormCollection form, ref Result result)
        {
            int index = 0;
            int maxIndex = int.Parse(form.Get("rateDetailMaxIndex"));
            List<RateDetailBo> rateDetailBos = new List<RateDetailBo> { };
            int valuationRate = ValuationRate;

            while (index <= maxIndex)
            {
                string genderCodeId = form.Get(string.Format("genderCodeId[{0}]", index));
                string cedingTobaccoUseId = form.Get(string.Format("cedingTobaccoUseId[{0}]", index));
                string cedingOccupationCodeId = form.Get(string.Format("cedingOccupationCodeId[{0}]", index));
                string attainedAge = form.Get(string.Format("attainedAge[{0}]", index));
                string issueAge = form.Get(string.Format("issueAge[{0}]", index));
                string policyTerm = form.Get(string.Format("policyTerm[{0}]", index));
                string policyTermRemain = form.Get(string.Format("policyTermRemain[{0}]", index));
                string rateValue = form.Get(string.Format("rateValue[{0}]", index));
                string id = form.Get(string.Format("rateDetailId[{0}]", index));

                string genderCode = form.Get(string.Format("genderCode[{0}]", index));
                string cedingTobaccoUse = form.Get(string.Format("cedingTobaccoUse[{0}]", index));
                string cedingOccupationCode = form.Get(string.Format("cedingOccupationCode[{0}]", index));

                int rateDetailId = 0;
                if (!string.IsNullOrEmpty(id))
                    rateDetailId = int.Parse(id);

                if (string.IsNullOrEmpty(genderCodeId) &&
                    string.IsNullOrEmpty(cedingTobaccoUseId) &&
                    string.IsNullOrEmpty(cedingOccupationCodeId) &&
                    string.IsNullOrEmpty(attainedAge) &&
                    string.IsNullOrEmpty(issueAge) &&
                    string.IsNullOrEmpty(policyTerm) &&
                    string.IsNullOrEmpty(policyTermRemain) &&
                    string.IsNullOrEmpty(rateValue) &&
                    rateDetailId == 0)
                {
                    index++;
                    continue;
                }

                RateDetailBo rateDetailBo = new RateDetailBo
                {
                    RateId = Id,
                    InsuredGenderCodePickListDetailId = Util.GetParseInt(genderCodeId),
                    CedingTobaccoUsePickListDetailId = Util.GetParseInt(cedingTobaccoUseId),
                    CedingOccupationCodePickListDetailId = Util.GetParseInt(cedingOccupationCodeId),
                    AttainedAge = Util.GetParseInt(attainedAge),
                    IssueAge = Util.GetParseInt(issueAge),
                    PolicyTerm = Util.StringToDouble(policyTerm),
                    PolicyTermRemain = Util.StringToDouble(policyTermRemain),
                    RateValueStr = rateValue,

                    InsuredGenderCode = genderCode,
                    CedingTobaccoUse = cedingTobaccoUse,
                    CedingOccupationCode = cedingOccupationCode,
                };

                Result validateResult = rateDetailBo.Validate(valuationRate, index + 1);

                result.AddErrorRange(validateResult.ToErrorArray());

                if (result.Valid)
                {
                    rateDetailBo.RateValue = Util.StringToDouble(rateDetailBo.RateValueStr).Value;
                }

                if (rateDetailId != 0)
                {
                    rateDetailBo.Id = rateDetailId;
                }

                rateDetailBos.Add(rateDetailBo);
                index++;
            }
            return rateDetailBos;
        }

        public void ValidateDuplicate(List<RateDetailBo> rateDetailBos, ref Result result)
        {
            List<RateDetailBo> duplicates = rateDetailBos.GroupBy(
                q => new
                {
                    q.InsuredGenderCodePickListDetailId,
                    q.CedingTobaccoUsePickListDetailId,
                    q.CedingOccupationCodePickListDetailId,
                    q.AttainedAge,
                    q.IssueAge,
                    q.PolicyTerm,
                    q.PolicyTermRemain
                }).Where(g => g.Count() > 1)
                .Select(r => new RateDetailBo
                {
                    InsuredGenderCodePickListDetailId = r.Key.InsuredGenderCodePickListDetailId,
                    CedingTobaccoUsePickListDetailId = r.Key.CedingTobaccoUsePickListDetailId,
                    CedingOccupationCodePickListDetailId = r.Key.CedingOccupationCodePickListDetailId,
                    AttainedAge = r.Key.AttainedAge,
                    IssueAge = r.Key.IssueAge,
                    PolicyTerm = r.Key.PolicyTerm,
                    PolicyTermRemain = r.Key.PolicyTermRemain
                }).ToList();

            foreach (RateDetailBo duplicate in duplicates)
            {
                RateDetailBo rateDetailBo = rateDetailBos
                    .Where(q => q.InsuredGenderCodePickListDetailId == duplicate.InsuredGenderCodePickListDetailId)
                    .Where(q => q.CedingTobaccoUsePickListDetailId == duplicate.CedingTobaccoUsePickListDetailId)
                    .Where(q => q.CedingOccupationCodePickListDetailId == duplicate.CedingOccupationCodePickListDetailId)
                    .Where(q => q.AttainedAge == duplicate.AttainedAge)
                    .Where(q => q.IssueAge == duplicate.IssueAge)
                    .Where(q => q.PolicyTerm == duplicate.PolicyTerm)
                    .Where(q => q.PolicyTermRemain == duplicate.PolicyTermRemain)
                    .LastOrDefault();
                int idx = rateDetailBos.IndexOf(rateDetailBo);
                result.AddError(string.Format("Duplicate Data Found at row #{0}", idx + 1));
            }
        }

        public void ProcessRateDetails(List<RateDetailBo> rateDetailBos, int authUserId, ref TrailObject trail)
        {
            List<int> savedIds = new List<int> { };

            foreach (RateDetailBo bo in rateDetailBos)
            {
                RateDetailBo rateDetailBo = bo;
                rateDetailBo.RateId = Id;
                rateDetailBo.CreatedById = authUserId;
                rateDetailBo.UpdatedById = authUserId;

                RateDetailService.Save(ref rateDetailBo, ref trail);
                savedIds.Add(rateDetailBo.Id);
            }
            RateDetailService.DeleteByRateDetailIdExcept(Id, savedIds, ref trail);
        }

        // to update fields based on valuation Rate
        public List<RateDetailBo> UpdateRateDetailFields(List<RateDetailBo> rateDetailBos)
        {
            foreach (RateDetailBo bo in rateDetailBos)
            {
                switch (ValuationRate)
                {
                    case RateBo.ValuationRate1:
                        bo.InsuredGenderCodePickListDetailId = null;
                        bo.CedingTobaccoUsePickListDetailId = null;
                        bo.CedingOccupationCodePickListDetailId = null;
                        bo.PolicyTermRemain = null;
                        break;
                    case RateBo.ValuationRate2:
                        bo.CedingTobaccoUsePickListDetailId = null;
                        bo.CedingOccupationCodePickListDetailId = null;
                        bo.IssueAge = null;
                        bo.PolicyTerm = null;
                        bo.PolicyTermRemain = null;
                        break;
                    case RateBo.ValuationRate3:
                        bo.InsuredGenderCodePickListDetailId = null;
                        bo.CedingOccupationCodePickListDetailId = null;
                        bo.IssueAge = null;
                        bo.PolicyTerm = null;
                        bo.PolicyTermRemain = null;
                        break;
                    case RateBo.ValuationRate4:
                        bo.CedingOccupationCodePickListDetailId = null;
                        bo.IssueAge = null;
                        bo.PolicyTerm = null;
                        bo.PolicyTermRemain = null;
                        break;
                    case RateBo.ValuationRate5:
                        bo.InsuredGenderCodePickListDetailId = null;
                        bo.CedingTobaccoUsePickListDetailId = null;
                        bo.AttainedAge = null;
                        bo.IssueAge = null;
                        bo.PolicyTerm = null;
                        bo.PolicyTermRemain = null;
                        break;
                    case RateBo.ValuationRate6:
                        bo.CedingOccupationCodePickListDetailId = null;
                        bo.IssueAge = null;
                        bo.PolicyTerm = null;
                        break;
                    case RateBo.ValuationRate7:
                        bo.InsuredGenderCodePickListDetailId = null;
                        bo.CedingTobaccoUsePickListDetailId = null;
                        bo.CedingOccupationCodePickListDetailId = null;
                        bo.PolicyTerm = null;
                        break;
                    case RateBo.ValuationRate8:
                        bo.CedingOccupationCodePickListDetailId = null;
                        bo.PolicyTermRemain = null;
                        break;
                    case RateBo.ValuationRate9:
                        bo.CedingTobaccoUsePickListDetailId = null;
                        bo.PolicyTerm = null;
                        bo.PolicyTermRemain = null;
                        break;
                    case RateBo.ValuationRate10:
                        bo.PolicyTerm = null;
                        bo.PolicyTermRemain = null;
                        break;
                }
            }

            return rateDetailBos;
        }

        public List<RateDetailUploadBo> GetRateDetailUploads(FormCollection form)
        {
            string uploads = form.Get("rateDetailUploadsJsonString");
            if (!string.IsNullOrEmpty(uploads))
                return JsonConvert.DeserializeObject<List<RateDetailUploadBo>>(uploads);
            else
                return new List<RateDetailUploadBo>();
        }

        public void SaveRateDetailUploads(List<RateDetailUploadBo> rateDetailUploadBos, int authUserId)
        {
            foreach (RateDetailUploadBo bo in rateDetailUploadBos)
            {
                if (bo.Id == 0 || bo.Status == 0)
                {
                    RateDetailUploadBo rateDetailUploadBo = bo;
                    rateDetailUploadBo.RateId = Id;
                    rateDetailUploadBo.Status = RateDetailUploadBo.StatusPendingProcess;
                    rateDetailUploadBo.CreatedById = authUserId;
                    rateDetailUploadBo.UpdatedById = authUserId;

                    string path = rateDetailUploadBo.GetLocalPath();
                    string tempPath = rateDetailUploadBo.GetTempPath("Uploads");
                    if (System.IO.File.Exists(tempPath))
                    {
                        Util.MakeDir(path);
                        System.IO.File.Move(tempPath, path);
                    }

                    RateDetailUploadService.Save(ref rateDetailUploadBo);
                }
            }
        }
    }
}