using BusinessObject;
using BusinessObject.RiDatas;
using DataAccess.Entities;
using DataAccess.Entities.RiDatas;
using Services;
using Shared;
using Shared.DataAccess;
using Shared.Forms.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class RiDataCorrectionViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ceding Company")]
        public int CedantId { get; set; }

        public virtual Cedant Cedant { get; set; }

        [Display(Name = "Ceding Company")]
        public CedantBo CedantBo { get; set; }

        [Display(Name = "Treaty Code")]
        public int? TreatyCodeId { get; set; }

        public virtual TreatyCode TreatyCode { get; set; }

        [Display(Name = "Treaty Code")]
        public TreatyCodeBo TreatyCodeBo { get; set; }

        [Required]
        [Display(Name = "Policy No.")]
        public string PolicyNumber { get; set; }

        [Display(Name = "Insured Register No.")]
        public string InsuredRegisterNo { get; set; }

        [Display(Name = "Insured Gender Code")]
        public int? InsuredGenderCodePickListDetailId { get; set; }

        public virtual PickListDetail InsuredGenderCodePickListDetail { get; set; }

        [Display(Name = "Insured Gender Code")]
        public PickListDetailBo InsuredGenderCodePickListDetailBo { get; set; }

        [Display(Name = "Insured Date of Birth")]
        public DateTime? InsuredDateOfBirth { get; set; }

        [Display(Name = "Insured Date of Birth")]
        [ValidateDate]
        public string InsuredDateOfBirthStr { get; set; }

        [Display(Name = "Insured Name")]
        public string InsuredName { get; set; }

        [Display(Name = "Campaign Code")]
        public string CampaignCode { get; set; }

        [Display(Name = "Reinsurance Basis Code")]
        public int? ReinsBasisCodePickListDetailId { get; set; }

        public virtual PickListDetail ReinsBasisCodePickListDetail { get; set; }

        [Display(Name = "Reinsurance Basis Code")]
        public PickListDetailBo ReinsBasisCodePickListDetailBo { get; set; }

        [Display(Name = "AP Loading")]
        public double? ApLoading { get; set; }

        [Display(Name = "AP Loading")]
        [ValidateDouble]
        public string ApLoadingStr { get; set; }

        public RiDataCorrectionViewModel() { }

        public RiDataCorrectionViewModel(RiDataCorrectionBo riDataCorrectionBo)
        {
            Set(riDataCorrectionBo);
        }

        public void Set(RiDataCorrectionBo riDataCorrectionBo)
        {
            if (riDataCorrectionBo != null)
            {
                Id = riDataCorrectionBo.Id;
                CedantId = riDataCorrectionBo.CedantId;
                CedantBo = riDataCorrectionBo.CedantBo;
                TreatyCodeId = riDataCorrectionBo.TreatyCodeId;
                TreatyCodeBo = riDataCorrectionBo.TreatyCodeBo;
                PolicyNumber = riDataCorrectionBo.PolicyNumber;
                InsuredRegisterNo = riDataCorrectionBo.InsuredRegisterNo;
                InsuredGenderCodePickListDetailId = riDataCorrectionBo.InsuredGenderCodePickListDetailId;
                InsuredGenderCodePickListDetailBo = riDataCorrectionBo.InsuredGenderCodePickListDetailBo;
                InsuredDateOfBirth = riDataCorrectionBo.InsuredDateOfBirth;
                InsuredDateOfBirthStr = riDataCorrectionBo.InsuredDateOfBirth?.ToString(Util.GetDateFormat());
                InsuredName = riDataCorrectionBo.InsuredName;
                CampaignCode = riDataCorrectionBo.CampaignCode;
                ApLoading = riDataCorrectionBo.ApLoading;
                ApLoadingStr = Util.DoubleToString(riDataCorrectionBo.ApLoading);
                ReinsBasisCodePickListDetailId = riDataCorrectionBo.ReinsBasisCodePickListDetailId;
                ReinsBasisCodePickListDetailBo = riDataCorrectionBo.ReinsBasisCodePickListDetailBo;
            }
        }

        public static Expression<Func<RiDataCorrection, RiDataCorrectionViewModel>> Expression()
        {
            return entity => new RiDataCorrectionViewModel
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                Cedant = entity.Cedant,
                TreatyCodeId = entity.TreatyCodeId,
                TreatyCode = entity.TreatyCode,
                PolicyNumber = entity.PolicyNumber,
                InsuredRegisterNo = entity.InsuredRegisterNo,
                InsuredGenderCodePickListDetailId = entity.InsuredGenderCodePickListDetailId,
                InsuredGenderCodePickListDetail = entity.InsuredGenderCodePickListDetail,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                InsuredName = entity.InsuredName,
                CampaignCode = entity.CampaignCode,
                ApLoading = entity.ApLoading,
                ReinsBasisCodePickListDetailId = entity.ReinsBasisCodePickListDetailId,
                ReinsBasisCodePickListDetail = entity.ReinsBasisCodePickListDetail,
            };
        }

        public RiDataCorrectionBo GetMappedValues(RiDataCorrectionViewModel model)
        {
            Result result = new Result();
            return GetMappedValues(model, ref result);
        }

        public RiDataCorrectionBo GetMappedValues(RiDataCorrectionViewModel model, ref Result result)
        {
            if (model.InsuredGenderCodePickListDetailId == null
                && string.IsNullOrEmpty(model.InsuredDateOfBirthStr) 
                && string.IsNullOrEmpty(model.InsuredName)
                && string.IsNullOrEmpty(model.CampaignCode)
                && string.IsNullOrEmpty(model.ApLoadingStr)
                && model.ReinsBasisCodePickListDetailId == null)
            {
                result.AddError("Please enter at least one mapped values");
            }
            return null;
        }
    }
}