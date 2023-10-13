using BusinessObject;
using DataAccess.Entities;
using Services;
using Shared;
using Shared.Forms.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class Mfrs17CellMappingViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Display(Name = "Treaty Code")]
        [Required]
        [ValidateTreatyCode]
        public string TreatyCode { get; set; }

        [Display(Name = "Reinsurance Basis Code")]
        [Required]
        public int ReinsBasisCodePickListDetailId { get; set; }

        public PickListDetailBo ReinsBasisCodePickListDetailBo { get; set; }

        public PickListDetail ReinsBasisCodePickListDetail { get; set; }

        [Display(Name = "Policy Reinsurance Effective Start Date")]
        public DateTime? ReinsEffDatePolStartDate { get; set; }

        [Display(Name = "Policy Reinsurance Effective Start Date")]
        [ValidateDate]
        public string ReinsEffDatePolStartDateStr { get; set; }

        [Display(Name = "Policy Reinsurance Effective End Date")]
        public DateTime? ReinsEffDatePolEndDate { get; set; }

        [Display(Name = "Policy Reinsurance Effective End Date")]
        [ValidateDate]
        public string ReinsEffDatePolEndDateStr { get; set; }

        [Display(Name = "Ceding Plan Code")]
        public string CedingPlanCode { get; set; }

        [Display(Name = "MLRe Benefit Code")]
        [ValidateMlreBenefitCode]
        public string BenefitCode { get; set; }

        [Display(Name = "MFRS17 Basic Rider")]
        [Required]
        public int BasicRiderPickListDetailId { get; set; }

        public PickListDetailBo BasicRiderPickListDetailBo { get; set; }

        public PickListDetail BasicRiderPickListDetail { get; set; }

        [StringLength(50), Display(Name = "MFRS17 Cell Name")]
        [Required]
        public string CellName { get; set; }

        //[StringLength(25), Display(Name = "MFRS17 Contract Code")]
        //[Required]
        //public string Mfrs17TreatyCode { get; set; }

        [Required, Display(Name = "MFRS17 Contract Code")]
        public int? Mfrs17ContractCodeDetailId { get; set; }

        public Mfrs17ContractCodeDetail Mfrs17ContractCodeDetail { get; set; }

        public Mfrs17ContractCodeDetailBo Mfrs17ContractCodeDetailBo { get; set; }

        public virtual ICollection<Mfrs17CellMappingDetail> Mfrs17CellMappingDetails { get; set; }

        [StringLength(20), Display(Name = "LOA Code")]
        public string LoaCode { get; set; }

        //[Display(Name = "Profit Commission")]
        //[Required]
        //public string ProfitComm { get; set; }

        [Required, Display(Name = "Profit Commission")]
        public int? ProfitCommPickListDetailId { get; set; }

        public PickListDetail ProfitCommPickListDetail { get; set; }

        public PickListDetailBo ProfitCommPickListDetailBo { get; set; }

        [StringLength(128), Display(Name = "Rate Table")]
        public string RateTable { get; set; }

        public Mfrs17CellMappingViewModel() { }

        public Mfrs17CellMappingViewModel(Mfrs17CellMappingBo mfrs17CellMappingBo)
        {
            Set(mfrs17CellMappingBo);
        }

        public void Set(Mfrs17CellMappingBo mfrs17CellMappingBo)
        {
            if (mfrs17CellMappingBo != null)
            {
                Id = mfrs17CellMappingBo.Id;
                TreatyCode = mfrs17CellMappingBo.TreatyCode;

                ReinsBasisCodePickListDetailId = mfrs17CellMappingBo.ReinsBasisCodePickListDetailId;

                ReinsEffDatePolStartDate = mfrs17CellMappingBo.ReinsEffDatePolStartDate;
                ReinsEffDatePolStartDateStr = mfrs17CellMappingBo.ReinsEffDatePolStartDate?.ToString(Util.GetDateFormat());
                ReinsEffDatePolEndDate = mfrs17CellMappingBo.ReinsEffDatePolEndDate;
                ReinsEffDatePolEndDateStr = mfrs17CellMappingBo.ReinsEffDatePolEndDate?.ToString(Util.GetDateFormat());

                CedingPlanCode = mfrs17CellMappingBo.CedingPlanCode;
                BenefitCode = mfrs17CellMappingBo.BenefitCode;
                //ProfitComm = mfrs17CellMappingBo.ProfitComm;
                ProfitCommPickListDetailId = mfrs17CellMappingBo.ProfitCommPickListDetailId;

                BasicRiderPickListDetailId = mfrs17CellMappingBo.BasicRiderPickListDetailId;
                CellName = mfrs17CellMappingBo.CellName;
                //Mfrs17TreatyCode = mfrs17CellMappingBo.Mfrs17TreatyCode;
                Mfrs17ContractCodeDetailId = mfrs17CellMappingBo.Mfrs17ContractCodeDetailId;
                LoaCode = mfrs17CellMappingBo.LoaCode;

                RateTable = mfrs17CellMappingBo.RateTable;

                ReinsBasisCodePickListDetailBo = mfrs17CellMappingBo.ReinsBasisCodePickListDetailBo;
                BasicRiderPickListDetailBo = mfrs17CellMappingBo.BasicRiderPickListDetailBo;
                ProfitCommPickListDetailBo = mfrs17CellMappingBo.ProfitCommPickListDetailBo;
                Mfrs17ContractCodeDetailBo = mfrs17CellMappingBo.Mfrs17ContractCodeDetailBo;
            }
        }

        public Mfrs17CellMappingBo FormBo(int createdById, int updatedById)
        {
            var bo = new Mfrs17CellMappingBo
            {
                Id = Id,
                TreatyCode = TreatyCode,
                ReinsBasisCodePickListDetailId = ReinsBasisCodePickListDetailId,
                ReinsBasisCodePickListDetailBo = PickListDetailService.Find(ReinsBasisCodePickListDetailId),
                CedingPlanCode = CedingPlanCode,
                BenefitCode = BenefitCode,
                //ProfitComm = ProfitComm,
                ProfitCommPickListDetailId = ProfitCommPickListDetailId,
                ProfitCommPickListDetailBo = PickListDetailService.Find(ProfitCommPickListDetailId),

                BasicRiderPickListDetailId = BasicRiderPickListDetailId,
                BasicRiderPickListDetailBo = PickListDetailService.Find(BasicRiderPickListDetailId),
                CellName = CellName?.Trim(),
                //Mfrs17TreatyCode = Mfrs17TreatyCode,
                Mfrs17ContractCodeDetailId = Mfrs17ContractCodeDetailId,
                Mfrs17ContractCodeDetailBo = Mfrs17ContractCodeDetailService.Find(Mfrs17ContractCodeDetailId),
                LoaCode = LoaCode?.Trim(),

                RateTable = RateTable?.Trim(),

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
                    string.Format(MessageBag.Required, "The Policy Reinsurance Effective Start Date Field"),
                    new[] { nameof(ReinsEffDatePolStartDateStr) }));
            }
            else if (start != null && end == null)
            {
                if (DateTime.Parse(Util.GetDefaultEndDate()) <= start)
                {
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.EndDefaultDate, "Policy Reinsurance Effective", Util.GetDefaultEndDate()),
                    new[] { nameof(ReinsEffDatePolStartDateStr) }));
                }
            }
            else if (start != null && end != null)
            {
                if (end <= start)
                {
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.StartDateEarlier, "Policy Reinsurance Effective"),
                    new[] { nameof(ReinsEffDatePolStartDateStr) }));
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.EndDateLater, "Policy Reinsurance Effective"),
                    new[] { nameof(ReinsEffDatePolEndDateStr) }));
                }
            }

            return results;
        }

        public static Expression<Func<Mfrs17CellMapping, Mfrs17CellMappingViewModel>> Expression()
        {
            return entity => new Mfrs17CellMappingViewModel
            {
                Id = entity.Id,
                TreatyCode = entity.TreatyCode,

                ReinsEffDatePolStartDate = entity.ReinsEffDatePolStartDate,
                ReinsEffDatePolEndDate = entity.ReinsEffDatePolEndDate,

                ReinsBasisCodePickListDetailId = entity.ReinsBasisCodePickListDetailId,
                ReinsBasisCodePickListDetail = entity.ReinsBasisCodePickListDetail,
                CedingPlanCode = entity.CedingPlanCode,
                BenefitCode = entity.BenefitCode,
                //ProfitComm = entity.ProfitComm,
                ProfitCommPickListDetailId = entity.ProfitCommPickListDetailId,
                ProfitCommPickListDetail = entity.ProfitCommPickListDetail,

                BasicRiderPickListDetailId = entity.BasicRiderPickListDetailId,
                BasicRiderPickListDetail = entity.BasicRiderPickListDetail,
                CellName = entity.CellName,
                //Mfrs17TreatyCode = entity.Mfrs17TreatyCode,
                Mfrs17ContractCodeDetailId = entity.Mfrs17ContractCodeDetailId,
                Mfrs17ContractCodeDetail = entity.Mfrs17ContractCodeDetail,
                LoaCode = entity.LoaCode,

                RateTable = entity.RateTable,

                Mfrs17CellMappingDetails = entity.Mfrs17CellMappingDetails,
            };
        }
    }

    public class ValidateMlreBenefitCode : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string benefitCode = value.ToString();
                string[] resultsArray = benefitCode.ToArraySplitTrim();
                IList<BenefitBo> benefitBos = BenefitService.Get();
                List<string> benefits = new List<string>();
                if (benefitBos != null)
                {
                    foreach (BenefitBo benefitBo in benefitBos)
                    {
                        benefits.Add(benefitBo.Code);
                    }
                }
                foreach (string result in resultsArray)
                {
                    if (!benefits.Contains(result))
                    {
                        return new ValidationResult("Please Enter Valid MLRe Benefit Code.");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }

    public class ValidateTreatyCodeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string[] treatyCodes = value.ToString().ToArraySplitTrim();
                foreach (var treatyCode in treatyCodes)
                {
                    if (TreatyCodeService.CountByCodeStatus(treatyCode, TreatyCodeBo.StatusActive) == 0)
                    {
                        return new ValidationResult("Please enter valid Treaty Code and the status is Active.");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}