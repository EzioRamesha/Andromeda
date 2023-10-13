using BusinessObject;
using BusinessObject.Retrocession;
using DataAccess.Entities;
using DataAccess.Entities.Retrocession;
using Shared;
using Shared.Forms.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class PerLifeDataCorrectionViewModel
    {
        public int Id { get; set; }

        [Required, DisplayName("Treaty Code")]
        public int TreatyCodeId { get; set; }

        public TreatyCodeBo TreatyCodeBo { get; set; }

        public TreatyCode TreatyCode { get; set; }

        [Required, StringLength(128), DisplayName("Insured Name")]
        public string InsuredName { get; set; }

        [DisplayName("Insured Date of Birth")]
        public DateTime? InsuredDateOfBirth { get; set; }

        [Required, DisplayName("Insured Date of Birth")]
        [ValidateDate]
        public string InsuredDateOfBirthStr { get; set; }

        [Required, StringLength(128), DisplayName("Policy Number")]
        public string PolicyNumber { get; set; }

        [Required, DisplayName("Org Gender Code")]
        public int InsuredGenderCodePickListDetailId { get; set; }

        public PickListDetailBo InsuredGenderCodePickListDetailBo { get; set; }

        public PickListDetail InsuredGenderCodePickListDetail { get; set; }

        public string InsuredGenderCode { get; set; }

        [Required, DisplayName("Org Territory of Issue ID")]
        public int TerritoryOfIssueCodePickListDetailId { get; set; }

        public PickListDetailBo TerritoryOfIssueCodePickListDetailBo { get; set; }

        public PickListDetail TerritoryOfIssueCodePickListDetail { get; set; }

        public string TerritoryOfIssueCode { get; set; }

        [Required, DisplayName("Expected Gender Code")]
        public int PerLifeRetroGenderId { get; set; }

        public PerLifeRetroGenderBo PerLifeRetroGenderBo { get; set; }

        public PerLifeRetroGender PerLifeRetroGender { get; set; }

        public string PerLifeRetroGenderStr { get; set; }

        [Required, DisplayName("Expected Territory of Issue ID")]
        public int PerLifeRetroCountryId { get; set; }

        public PerLifeRetroCountryBo PerLifeRetroCountryBo { get; set; }

        public PerLifeRetroCountry PerLifeRetroCountry { get; set; }

        public string PerLifeRetroCountryStr { get; set; }

        [DisplayName("1st Date of The Policy Exist In The System")]
        public DateTime? DateOfPolicyExist { get; set; }

        [Required, DisplayName("1st Date of The Policy Exist In The System")]
        [ValidateDate]
        public string DateOfPolicyExistStr { get; set; }

        [Required, DisplayName("Proceed to Aggregate")]
        public bool IsProceedToAggregate { get; set; } = false;

        [DisplayName("Date of Exception Detected")]
        public DateTime? DateOfExceptionDetected { get; set; }

        [Required, DisplayName("Date of Exception Detected")]
        [ValidateDate]
        public string DateOfExceptionDetectedStr { get; set; }

        [Required, DisplayName("Exception Status")]
        public int ExceptionStatusPickListDetailId { get; set; }

        public PickListDetailBo ExceptionStatusPickListDetailBo { get; set; }

        public PickListDetail ExceptionStatusPickListDetail { get; set; }

        public string ExceptionStatus { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }

        [DisplayName("Date Updated")]
        public DateTime? DateUpdated { get; set; }

        [Required, DisplayName("Date Updated")]
        [ValidateDate]
        public string DateUpdatedStr { get; set; }

        public PerLifeDataCorrectionViewModel() { }

        public PerLifeDataCorrectionViewModel(PerLifeDataCorrectionBo perLifeDataCorrectionBo)
        {
            Set(perLifeDataCorrectionBo);
        }

        public void Set(PerLifeDataCorrectionBo bo)
        {
            var dateFormat = Util.GetDateFormat();
            if (bo != null)
            {
                Id = bo.Id;
                TreatyCodeId = bo.TreatyCodeId;
                TreatyCodeBo = bo.TreatyCodeBo;
                InsuredName = bo.InsuredName;
                InsuredDateOfBirth = bo.InsuredDateOfBirth;
                InsuredDateOfBirthStr = bo.InsuredDateOfBirth.ToString(dateFormat);
                PolicyNumber = bo.PolicyNumber;
                InsuredGenderCodePickListDetailId = bo.InsuredGenderCodePickListDetailId;
                InsuredGenderCodePickListDetailBo = bo.InsuredGenderCodePickListDetailBo;
                TerritoryOfIssueCodePickListDetailId = bo.TerritoryOfIssueCodePickListDetailId;
                TerritoryOfIssueCodePickListDetailBo = bo.TerritoryOfIssueCodePickListDetailBo;
                PerLifeRetroGenderId = bo.PerLifeRetroGenderId;
                PerLifeRetroGenderBo = bo.PerLifeRetroGenderBo;
                PerLifeRetroCountryId = bo.PerLifeRetroCountryId;
                PerLifeRetroCountryBo = bo.PerLifeRetroCountryBo;
                DateOfPolicyExist = bo.DateOfPolicyExist;
                DateOfPolicyExistStr = bo.DateOfPolicyExist.ToString(dateFormat);
                IsProceedToAggregate = bo.IsProceedToAggregate;
                DateOfExceptionDetected = bo.DateOfExceptionDetected;
                DateOfExceptionDetectedStr = bo.DateOfExceptionDetected.ToString(dateFormat);
                ExceptionStatusPickListDetailId = bo.ExceptionStatusPickListDetailId;
                ExceptionStatusPickListDetailBo = bo.ExceptionStatusPickListDetailBo;
                Remark = bo.Remark;
                DateUpdated = bo.DateUpdated;
                DateUpdatedStr = bo.DateUpdated.ToString(dateFormat);
            }
        }

        public PerLifeDataCorrectionBo FormBo(int createdById, int updatedById)
        {
            var bo = new PerLifeDataCorrectionBo
            {
                Id = Id,
                TreatyCodeId = TreatyCodeId,
                TreatyCodeBo = TreatyCodeBo,
                InsuredName = InsuredName,
                InsuredDateOfBirth = DateTime.Parse(InsuredDateOfBirthStr),
                PolicyNumber = PolicyNumber,
                InsuredGenderCodePickListDetailId = InsuredGenderCodePickListDetailId,
                InsuredGenderCodePickListDetailBo = InsuredGenderCodePickListDetailBo,
                TerritoryOfIssueCodePickListDetailId = TerritoryOfIssueCodePickListDetailId,
                TerritoryOfIssueCodePickListDetailBo = TerritoryOfIssueCodePickListDetailBo,
                PerLifeRetroGenderId = PerLifeRetroGenderId,
                PerLifeRetroGenderBo = PerLifeRetroGenderBo,
                PerLifeRetroCountryId = PerLifeRetroCountryId,
                PerLifeRetroCountryBo = PerLifeRetroCountryBo,
                DateOfPolicyExist = DateTime.Parse(DateOfPolicyExistStr),
                IsProceedToAggregate = IsProceedToAggregate,
                DateOfExceptionDetected = DateTime.Parse(DateOfExceptionDetectedStr),
                ExceptionStatusPickListDetailId = ExceptionStatusPickListDetailId,
                Remark = Remark,
                DateUpdated = DateTime.Parse(DateUpdatedStr),

                CreatedById = createdById,
                UpdatedById = updatedById,
            };

            return bo;
        }

        public static Expression<Func<PerLifeDataCorrection, PerLifeDataCorrectionViewModel>> Expression()
        {
            return entity => new PerLifeDataCorrectionViewModel
            {
                Id = entity.Id,
                TreatyCodeId = entity.TreatyCodeId,
                TreatyCode = entity.TreatyCode,
                InsuredName = entity.InsuredName,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                PolicyNumber = entity.PolicyNumber,
                InsuredGenderCodePickListDetailId = entity.InsuredGenderCodePickListDetailId,
                InsuredGenderCodePickListDetail = entity.InsuredGenderCodePickListDetail,
                TerritoryOfIssueCodePickListDetailId = entity.TerritoryOfIssueCodePickListDetailId,
                TerritoryOfIssueCodePickListDetail = entity.TerritoryOfIssueCodePickListDetail,
                PerLifeRetroGenderId = entity.PerLifeRetroGenderId,
                PerLifeRetroGender = entity.PerLifeRetroGender,
                PerLifeRetroCountryId = entity.PerLifeRetroCountryId,
                PerLifeRetroCountry = entity.PerLifeRetroCountry,
                DateOfPolicyExist = entity.DateOfPolicyExist,
                IsProceedToAggregate = entity.IsProceedToAggregate,
                DateOfExceptionDetected = entity.DateOfExceptionDetected,
                ExceptionStatusPickListDetailId = entity.ExceptionStatusPickListDetailId,
                ExceptionStatusPickListDetail = entity.ExceptionStatusPickListDetail,
                Remark = entity.Remark,
                DateUpdated = entity.DateUpdated,
            };
        }
    }
}