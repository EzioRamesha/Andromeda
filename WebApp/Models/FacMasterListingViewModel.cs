using BusinessObject;
using DataAccess.Entities;
using Services;
using Shared;
using Shared.Forms.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class FacMasterListingViewModel
    {
        public int Id { get; set; }

        [DisplayName("Unique ID")]
        [Required]
        public string UniqueId { get; set; }

        [DisplayName("eWarp Number")]
        [Required]
        public int? EwarpNumber { get; set; }

        [DisplayName("Insured Name")]
        [Required]
        public string InsuredName { get; set; }

        [DisplayName("Insured Date Of Birth")]
        public DateTime? InsuredDateOfBirth { get; set; }

        [DisplayName("Insured Date Of Birth")]
        public string InsuredDateOfBirthStr { get; set; }

        [DisplayName("Insured Gender Code")]
        public int? InsuredGenderCodePickListDetailId { get; set; }

        public PickListDetailBo InsuredGenderCodePickListDetailBo { get; set; }

        public PickListDetail InsuredGenderCodePickListDetail { get; set; }

        [DisplayName("Ceding Company")]
        [Required]
        public int? CedantId { get; set; }

        public CedantBo CedantBo { get; set; }

        public Cedant Cedant { get; set; }

        [DisplayName("Policy Number")]
        [Required]
        public string PolicyNumber { get; set; }

        [DisplayName("Flat Extra Amount Offered")]
        public double? FlatExtraAmountOffered { get; set; }

        [DisplayName("Flat Extra Amount Offered")]
        [ValidateDouble]
        public string FlatExtraAmountOfferedStr { get; set; }

        [DisplayName("Flat Extra Duration")]
        public int? FlatExtraDuration { get; set; }

        [DisplayName("MLRe Benefit Code")]
        [Required]
        [ValidateMlreBenefitCode]
        public string BenefitCode { get; set; }

        [DisplayName("Sum Assured Offered")]
        public double? SumAssuredOffered { get; set; }

        [DisplayName("Sum Assured Offered")]
        [ValidateDouble]
        public string SumAssuredOfferedStr { get; set; }

        [DisplayName("eWarp Action Code")]
        [Required]
        public string EwarpActionCode { get; set; }

        [DisplayName("UW Rating Offered")]
        public double? UwRatingOffered { get; set; }

        [DisplayName("UW Rating Offered")]
        [ValidateDouble]
        public string UwRatingOfferedStr { get; set; }

        [DisplayName("Offer Letter Sent Date")]
        public DateTime? OfferLetterSentDate { get; set; }

        [DisplayName("Offer Letter Sent Date")]
        public string OfferLetterSentDateStr { get; set; }

        [DisplayName("UW Opinion")]
        public string UwOpinion { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }

        [DisplayName("Ceding Benefit Type Code")]
        public string CedingBenefitTypeCode { get; set; }

        public virtual ICollection<FacMasterListingDetail> FacMasterListingDetails { get; set; }

        public FacMasterListingViewModel() { }

        public FacMasterListingViewModel(FacMasterListingBo facMasterListingBo)
        {
            Set(facMasterListingBo);
        }

        public void Set(FacMasterListingBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                UniqueId = bo.UniqueId;
                EwarpNumber = bo.EwarpNumber;
                InsuredName = bo.InsuredName;
                InsuredDateOfBirth = bo.InsuredDateOfBirth;
                InsuredDateOfBirthStr = bo.InsuredDateOfBirth?.ToString(Util.GetDateFormat());
                InsuredGenderCodePickListDetailId = bo.InsuredGenderCodePickListDetailId;
                InsuredGenderCodePickListDetailBo = bo.InsuredGenderCodePickListDetailBo;
                CedantId = bo.CedantId;
                CedantBo = bo.CedantBo;
                PolicyNumber = bo.PolicyNumber;
                FlatExtraAmountOffered = bo.FlatExtraAmountOffered;
                FlatExtraAmountOfferedStr = Util.DoubleToString(bo.FlatExtraAmountOffered);
                FlatExtraDuration = bo.FlatExtraDuration;
                BenefitCode = bo.BenefitCode;
                SumAssuredOffered = bo.SumAssuredOffered;
                SumAssuredOfferedStr = Util.DoubleToString(bo.SumAssuredOffered);
                EwarpActionCode = bo.EwarpActionCode;
                UwRatingOffered = bo.UwRatingOffered;
                UwRatingOfferedStr = Util.DoubleToString(bo.UwRatingOffered);
                OfferLetterSentDate = bo.OfferLetterSentDate;
                OfferLetterSentDateStr = bo.OfferLetterSentDate?.ToString(Util.GetDateFormat());
                UwOpinion = bo.UwOpinion;
                Remark = bo.Remark;
                CedingBenefitTypeCode = bo.CedingBenefitTypeCode;
            }
        }

        public FacMasterListingBo FormBo(int createdById, int updatedById)
        {
            var bo = new FacMasterListingBo
            {
                Id = Id,
                UniqueId = UniqueId?.Trim(),
                EwarpNumber = EwarpNumber,
                InsuredName = InsuredName?.Trim(),
                InsuredGenderCodePickListDetailId = InsuredGenderCodePickListDetailId,
                InsuredGenderCodePickListDetailBo = PickListDetailService.Find(InsuredGenderCodePickListDetailId),
                CedantId = CedantId,
                CedantBo = CedantService.Find(CedantId),
                PolicyNumber = PolicyNumber,
                FlatExtraDuration = FlatExtraDuration,
                BenefitCode = BenefitCode,
                EwarpActionCode = EwarpActionCode?.Trim(),
                UwOpinion = UwOpinion?.Trim(),
                Remark = Remark?.Trim(),
                CedingBenefitTypeCode = CedingBenefitTypeCode,

                CreatedById = createdById,
                UpdatedById = updatedById,
            };

            if (InsuredDateOfBirthStr != null)
            {
                bo.InsuredDateOfBirth = DateTime.Parse(InsuredDateOfBirthStr);
            }

            if (OfferLetterSentDateStr != null)
            {
                bo.OfferLetterSentDate = DateTime.Parse(OfferLetterSentDateStr);
            }

            double? d = Util.StringToDouble(FlatExtraAmountOfferedStr);
            bo.FlatExtraAmountOffered = d;

            d = Util.StringToDouble(SumAssuredOfferedStr);
            bo.SumAssuredOffered = d;

            d = Util.StringToDouble(UwRatingOfferedStr);
            bo.UwRatingOffered = d;

            return bo;
        }

        public static Expression<Func<FacMasterListing, FacMasterListingViewModel>> Expression()
        {
            return entity => new FacMasterListingViewModel
            {
                Id = entity.Id,
                UniqueId = entity.UniqueId,
                EwarpNumber = entity.EwarpNumber,
                InsuredName = entity.InsuredName,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                InsuredGenderCodePickListDetailId = entity.InsuredGenderCodePickListDetailId,
                InsuredGenderCodePickListDetail = entity.InsuredGenderCodePickListDetail,
                CedantId = entity.CedantId,
                Cedant = entity.Cedant,
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

                FacMasterListingDetails = entity.FacMasterListingDetails,
            };
        }
    }
}