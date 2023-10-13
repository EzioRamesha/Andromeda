using BusinessObject;
using BusinessObject.Retrocession;
using DataAccess.Entities;
using DataAccess.Entities.Retrocession;
using Services;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class PerLifeAggregationDuplicationListingViewModel
    {
        public int Id { get; set; }

        [DisplayName("Treaty Code")]
        public int? TreatyCodeId { get; set; }
        public TreatyCode TreatyCode { get; set; }
        public string TreatyCodeName { get; set; }

        [Required, DisplayName("Treaty Code")]
        public string TreatyCodeStr { get; set; }

        [Required, DisplayName("Ceding Plan Code")]
        public string CedantPlanCode { get; set; }

        [DisplayName("Insured Name")]
        public string InsuredName { get; set; }

        [DisplayName("Insured Date of Birth")]
        public DateTime? InsuredDateOfBirth { get; set; }
        [Required]
        public string InsuredDateOfBirthStr { get; set; }

        [Required, DisplayName("Policy Number")]
        public string PolicyNumber { get; set; }

        [Required, DisplayName("Insured Gender Code")]
        public int? InsuredGenderCodePickListDetailId { get; set; }
        public PickListDetail InsuredGenderCodePickListDetail { get; set; }
        public PickListDetailBo InsuredGenderCodePickListDetailBo { get; set; }

        [DisplayName("MLRe Benefit Code")]
        public int? MLReBenefitCodeId { get; set; }
        [Required, DisplayName("MLRe Benefit Code")]
        public string MLReBenefitCodeStr { get; set; }
        public Benefit MLReBenefitCode { get; set; }
        public BenefitBo MLReBenefitCodeBo { get; set; }

        [Required, DisplayName("Ceding Benefit Risk Code")]
        public string CedingBenefitRiskCode { get; set; }

        [Required, DisplayName("Ceding Benefit Type Code")]
        public string CedingBenefitTypeCode { get; set; }

        [DisplayName("Reinsurance Effective Date")]
        public DateTime? ReinsuranceEffectiveDate { get; set; }
        [Required]
        public string ReinsuranceEffectiveDateStr { get; set; }

        [Required, DisplayName("FDS Accounting Type")]
        public int? FundsAccountingTypePickListDetailId { get; set; }
        public PickListDetailBo FundsAccountingTypePickListDetailBo { get; set; }

        [Required, DisplayName("Reinsurance Risk Basis")]
        public int? ReinsBasisCodePickListDetailId { get; set; }
        public PickListDetailBo ReinsBasisCodePickListDetailBo { get; set; }

        [Required, DisplayName("Transaction Type")]
        public int? TransactionTypePickListDetailId { get; set; }
        public PickListDetailBo TransactionTypePickListDetailBo { get; set; }

        [DisplayName("Proceed To Aggregate")]
        public int? ProceedToAggregate { get; set; }

        [DisplayName("Date Updated")]
        public DateTime? DateUpdated { get; set; }
        public string DateUpdatedStr { get; set; }

        [DisplayName("Exception Status")]
        public int? ExceptionStatusPickListDetailId { get; set; }
        public PickListDetailBo ExceptionStatusPickListDetailBo { get; set; }

        [DisplayName("Remarks")]
        public string Remarks { get; set; }

        [DisplayName("Date Created")]
        public DateTime? CreatedAt { get; set; }

        [DisplayName("Date Created")]
        public string CreatedAtStr { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public PerLifeAggregationDuplicationListingViewModel()
        {
            Set();
        }

        public PerLifeAggregationDuplicationListingViewModel(PerLifeAggregationDuplicationListingBo PerLifeAggregationDuplicationListingBo)
        {
            Set(PerLifeAggregationDuplicationListingBo);
        }

        public void Set(PerLifeAggregationDuplicationListingBo bo = null)
        {
            if (bo != null)
            {
                Id = bo.Id;
                TreatyCodeId = bo.TreatyCodeId;
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
                ProceedToAggregate = bo.ProceedToAggregate;
                DateUpdated = bo.DateUpdated;
                DateUpdatedStr = bo.DateUpdated?.ToString(Util.GetDateFormat());
                ExceptionStatusPickListDetailId = bo.ExceptionStatusPickListDetailId;
                ExceptionStatusPickListDetailBo = bo.ExceptionStatusPickListDetailBo;
                Remarks = bo.Remarks;

                CreatedAt = bo.CreatedAt;
                CreatedAtStr = bo.CreatedAt.ToString(Util.GetDateFormat());
            }
        }

        public PerLifeAggregationDuplicationListingBo FormBo(int createdById, int updatedById)
        {
            return new PerLifeAggregationDuplicationListingBo
            {
                TreatyCodeId = TreatyCodeId,
                TreatyCodeStr = TreatyCodeStr,
                CedantPlanCode = CedantPlanCode,
                InsuredName = InsuredName,
                InsuredDateOfBirth = InsuredDateOfBirthStr is null || InsuredDateOfBirthStr == "" ? null : Util.GetParseDateTime(InsuredDateOfBirthStr),
                InsuredDateOfBirthStr = InsuredDateOfBirthStr,
                PolicyNumber = PolicyNumber,
                InsuredGenderCodePickListDetailId = InsuredGenderCodePickListDetailId,
                MLReBenefitCodeId = MLReBenefitCodeId,
                MLReBenefitCodeStr = MLReBenefitCodeStr,
                CedingBenefitRiskCode = CedingBenefitRiskCode,
                CedingBenefitTypeCode = CedingBenefitTypeCode,
                ReinsuranceEffectiveDate = ReinsuranceEffectiveDateStr is null || ReinsuranceEffectiveDateStr == "" ? null : Util.GetParseDateTime(ReinsuranceEffectiveDateStr),
                ReinsuranceEffecitveDateStr = ReinsuranceEffectiveDateStr,
                FundsAccountingTypePickListDetailId = FundsAccountingTypePickListDetailId,
                ReinsBasisCodePickListDetailId = ReinsBasisCodePickListDetailId,
                TransactionTypePickListDetailId = TransactionTypePickListDetailId,
                ProceedToAggregate = ProceedToAggregate,
                DateUpdated = DateUpdatedStr is null || DateUpdatedStr == "" ? null : Util.GetParseDateTime(DateUpdatedStr),
                DateUpdatedStr = DateUpdatedStr,
                ExceptionStatusPickListDetailId = ExceptionStatusPickListDetailId,
                Remarks = Remarks,

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<PerLifeAggregationDuplicationListing, PerLifeAggregationDuplicationListingViewModel>> Expression()
        {
            return entity => new PerLifeAggregationDuplicationListingViewModel
            {
                Id = entity.Id,
                TreatyCodeId = entity.TreatyCodeId,
                TreatyCode = entity.TreatyCode,
                CedantPlanCode = entity.CedantPlanCode,
                InsuredName = entity.InsuredName,
                InsuredGenderCodePickListDetailId = entity.InsuredGenderCodePickListDetailId,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                PolicyNumber = entity.PolicyNumber,
                MLReBenefitCodeId = entity.MLReBenefitCodeId,
                MLReBenefitCode = entity.MLReBenefitCode,
                CedingBenefitRiskCode = entity.CedingBenefitRiskCode,
                CedingBenefitTypeCode = entity.CedingBenefitTypeCode,
                ReinsuranceEffectiveDate = entity.ReinsuranceEffectiveDate,
                FundsAccountingTypePickListDetailId = entity.FundsAccountingTypePickListDetailId,
                ReinsBasisCodePickListDetailId = entity.ReinsBasisCodePickListDetailId,
                TransactionTypePickListDetailId = entity.TransactionTypePickListDetailId,
                ProceedToAggregate = entity.ProceedToAggregate,
                DateUpdated = entity.DateUpdated,
                ExceptionStatusPickListDetailId = entity.ExceptionStatusPickListDetailId,
                Remarks = entity.Remarks
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