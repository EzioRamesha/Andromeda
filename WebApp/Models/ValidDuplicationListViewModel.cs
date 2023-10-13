using BusinessObject;
using BusinessObject.Retrocession;
using BusinessObject.TreatyPricing;
using DataAccess.Entities;
using DataAccess.Entities.Retrocession;
using DataAccess.Entities.TreatyPricing;
using Services;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class ValidDuplicationListViewModel
    {
        public int Id { get; set; }

        [Required, DisplayName("Treaty Code")]
        public int? TreatyCodeId { get; set; }
        public TreatyCode TreatyCode { get; set; }

        public TreatyCodeBo TreatyCodeBo { get; set; }

        [DisplayName("Treaty Code")]
        public string TreatyCodeName { get; set; }

        [DisplayName("Treaty Code")]
        public string TreatyCodeStr { get; set; }

        [Required, DisplayName("Cedant Plan Code")]
        public string CedantPlanCode { get; set; }

        [Required, DisplayName("Insured Name")]
        public string InsuredName { get; set; }

        [DisplayName("Insured Date of Birth")]
        public DateTime? InsuredDateOfBirth { get; set; }
        [DisplayName("Insured Date of Birth")]
        [Required]
        public string InsuredDateOfBirthStr { get; set; }

        [Required, DisplayName("Policy Number")]
        public string PolicyNumber { get; set; }

        [Required, DisplayName("Insured Gender Code")]
        public int? InsuredGenderCodePickListDetailId { get; set; }
        public PickListDetail InsuredGenderCodePickListDetail { get; set; }
        public PickListDetailBo InsuredGenderCodePickListDetailBo { get; set; }

        [Required, DisplayName("MLRe Benefit Code")]
        public int? MLReBenefitCodeId { get; set; }
        [DisplayName("MLRe Benefit Code")]
        public string MLReBenefitCodeStr { get; set; }
        public Benefit MLReBenefitCode { get; set; }
        public BenefitBo MLReBenefitCodeBo { get; set; }

        [Required, DisplayName("Ceding Benefit Risk Code")]
        public string CedingBenefitRiskCode { get; set; }

        [Required, DisplayName("Ceding Benefit Type Code")]
        public string CedingBenefitTypeCode { get; set; }

        [DisplayName("Reinsurance Effective Date")]
        public DateTime? ReinsuranceEffectiveDate { get; set; }
        [DisplayName("Reinsurance Effective Date")]
        [Required]
        public string ReinsuranceEffectiveDateStr { get; set; }

        [Required, DisplayName("FDS Accounting Type")]
        public int? FundsAccountingTypePickListDetailId { get; set; }
        public PickListDetail FundsAccountingTypePickListDetail { get; set; }
        public PickListDetailBo FundsAccountingTypePickListDetailBo { get; set; }

        [Required, DisplayName("Reinsurance Risk Basis")]
        public int? ReinsBasisCodePickListDetailId { get; set; }
        public PickListDetail ReinsBasisCodePickListDetail { get; set; }
        public PickListDetailBo ReinsBasisCodePickListDetailBo { get; set; }

        [Required, DisplayName("Transaction Type")]
        public int? TransactionTypePickListDetailId { get; set; }
        public PickListDetail TransactionTypePickListDetail { get; set; }
        public PickListDetailBo TransactionTypePickListDetailBo { get; set; }

        [DisplayName("Date Created")]
        public DateTime? CreatedAt { get; set; }

        [DisplayName("Date Created")]
        public string CreatedAtStr { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public ValidDuplicationListViewModel()
        {
            Set();
        }

        public ValidDuplicationListViewModel(ValidDuplicationListBo validDuplicationListBo)
        {
            Set(validDuplicationListBo);
        }

        public void Set(ValidDuplicationListBo bo = null)
        {
            if (bo != null)
            {
                Id = bo.Id;
                TreatyCodeId = bo.TreatyCodeId;
                TreatyCodeBo = bo.TreatyCodeBo;
                TreatyCodeStr = bo.TreatyCodeBo?.Code;
                CedantPlanCode = bo.CedantPlanCode;
                InsuredName = bo.InsuredName;
                InsuredDateOfBirth = bo.InsuredDateOfBirth;
                InsuredDateOfBirthStr = bo.InsuredDateOfBirth?.ToString(Util.GetDateFormat());
                PolicyNumber = bo.PolicyNumber;
                InsuredGenderCodePickListDetailId = bo.InsuredGenderCodePickListDetailId;
                InsuredGenderCodePickListDetailBo = bo.InsuredGenderCodePickListDetailBo;
                MLReBenefitCodeId = bo.MLReBenefitCodeId;
                MLReBenefitCodeBo = bo.MLReBenefitCodeBo;
                MLReBenefitCodeStr = bo.MLReBenefitCodeBo?.Code;
                CedingBenefitRiskCode = bo.CedingBenefitRiskCode;
                CedingBenefitTypeCode = bo.CedingBenefitTypeCode;
                ReinsuranceEffectiveDate = bo.ReinsuranceEffectiveDate;
                ReinsuranceEffectiveDateStr = bo.ReinsuranceEffectiveDate?.ToString(Util.GetDateFormat());
                FundsAccountingTypePickListDetailId = bo.FundsAccountingTypePickListDetailId;
                FundsAccountingTypePickListDetailBo = bo.FundsAccountingTypePickListDetailBo;
                ReinsBasisCodePickListDetailId = bo.ReinsBasisCodePickListDetailId;
                ReinsBasisCodePickListDetailBo = bo.ReinsBasisCodePickListDetailBo;
                TransactionTypePickListDetailId = bo.TransactionTypePickListDetailId;
                TransactionTypePickListDetailBo = bo.TransactionTypePickListDetailBo;

                CreatedAt = bo.CreatedAt;
                CreatedAtStr = bo.CreatedAt.ToString(Util.GetDateFormat());
            }
        }

        public ValidDuplicationListBo FormBo(int createdById, int updatedById)
        {
            return new ValidDuplicationListBo
            {
                TreatyCodeId = TreatyCodeId,
                CedantPlanCode = CedantPlanCode,
                InsuredName = InsuredName,
                InsuredDateOfBirth = InsuredDateOfBirthStr is null || InsuredDateOfBirthStr == "" ? null : Util.GetParseDateTime(InsuredDateOfBirthStr),
                InsuredDateOfBirthStr = InsuredDateOfBirthStr,
                PolicyNumber = PolicyNumber,
                InsuredGenderCodePickListDetailId = InsuredGenderCodePickListDetailId,
                MLReBenefitCodeId = MLReBenefitCodeId,
                CedingBenefitRiskCode = CedingBenefitRiskCode,
                CedingBenefitTypeCode = CedingBenefitTypeCode,
                ReinsuranceEffectiveDate = ReinsuranceEffectiveDateStr is null || ReinsuranceEffectiveDateStr == "" ? null : Util.GetParseDateTime(ReinsuranceEffectiveDateStr),
                ReinsuranceEffectiveDateStr = ReinsuranceEffectiveDateStr,
                FundsAccountingTypePickListDetailId = FundsAccountingTypePickListDetailId,
                ReinsBasisCodePickListDetailId = ReinsBasisCodePickListDetailId,
                TransactionTypePickListDetailId = TransactionTypePickListDetailId,

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<ValidDuplicationList, ValidDuplicationListViewModel>> Expression()
        {
            return entity => new ValidDuplicationListViewModel
            {
                Id = entity.Id,
                TreatyCodeId = entity.TreatyCodeId,
                TreatyCode = entity.TreatyCode,
                CedantPlanCode = entity.CedantPlanCode,
                InsuredName = entity.InsuredName,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                InsuredGenderCodePickListDetailId = entity.InsuredGenderCodePickListDetailId,
                InsuredGenderCodePickListDetail = entity.InsuredGenderCodePickListDetail,
                PolicyNumber = entity.PolicyNumber,
                MLReBenefitCodeId = entity.MLReBenefitCodeId,
                MLReBenefitCode = entity.MLReBenefitCode,
                CedingBenefitRiskCode = entity.CedingBenefitRiskCode,
                CedingBenefitTypeCode = entity.CedingBenefitTypeCode,
                ReinsuranceEffectiveDate = entity.ReinsuranceEffectiveDate,
                FundsAccountingTypePickListDetailId = entity.FundsAccountingTypePickListDetailId,
                FundsAccountingTypePickListDetail = entity.FundsAccountingTypePickListDetail,
                ReinsBasisCodePickListDetailId = entity.ReinsBasisCodePickListDetailId,
                ReinsBasisCodePickListDetail = entity.ReinsBasisCodePickListDetail,
                TransactionTypePickListDetailId = entity.TransactionTypePickListDetailId,
                TransactionTypePickListDetail = entity.TransactionTypePickListDetail,
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();
            var treatyCodeBo = TreatyCodeService.FindByCode(TreatyCodeStr);

            if (treatyCodeBo == null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.NotExistsWithValue, "Treaty Code"),
                    new[] { nameof(TreatyCodeStr) }));
            }

            return results;
        }
    }
}