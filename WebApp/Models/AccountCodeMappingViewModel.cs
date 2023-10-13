using BusinessObject;
using DataAccess.Entities;
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
    public class AccountCodeMappingViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Report Type")]
        public int ReportType { get; set; }

        [Required]
        [DisplayName("Type")]
        public int? Type { get; set; }

        [DisplayName("Treaty Type")]
        [ValidateTreatyType]
        public string TreatyType { get; set; }

        [DisplayName("Claim Code")]
        [ValidateClaimCode]
        public string ClaimCode { get; set; }

        [DisplayName("Treaty Code")]
        public int? TreatyCodeId { get; set; }

        public virtual TreatyCode TreatyCode { get; set; }

        public TreatyCodeBo TreatyCodeBo { get; set; }

        [DisplayName("Business Origin")]
        [ValidateBusinessOrigin]
        public string BusinessOrigin { get; set; }

        [Required]
        [DisplayName("Account Code")]
        public int AccountCodeId { get; set; }

        public virtual AccountCode AccountCode { get; set; }

        public AccountCodeBo AccountCodeBo { get; set; }

        [DisplayName("Debit / Credit Indicator - Positive")]
        public int? DebitCreditIndicatorPositive { get; set; }

        [DisplayName("Debit / Credit Indicator - Negative")]
        public int? DebitCreditIndicatorNegative { get; set; }

        [DisplayName("Transaction Type Code")]
        public int? TransactionTypeCodePickListDetailId { get; set; }

        public virtual PickListDetail TransactionTypeCodePickListDetail { get; set; }

        public PickListDetailBo TransactionTypeCodePickListDetailBo { get; set; }

        [DisplayName("Retro Register Field")]
        public int? RetroRegisterFieldPickListDetailId { get; set; }

        public virtual PickListDetail RetroRegisterFieldPickListDetail { get; set; }

        public PickListDetailBo RetroRegisterFieldPickListDetailBo { get; set; }

        public virtual ICollection<AccountCodeMappingDetail> AccountCodeMappingDetails { get; set; }

        [DisplayName("Modified Contract Code")]
        public int? ModifiedContractCodeId { get; set; }
        public Mfrs17ContractCodeBo ModifiedContractCodeBo { get; set; }
        public Mfrs17ContractCode ModifiedContractCode { get; set; }

        [DisplayName("Invoice Field")]
        [ValidateInvoiceField]
        public string InvoiceField { get; set; }

        [DisplayName("Grouping Indicator")]
        public bool IsBalanceSheet { get; set; }

        [DisplayName("Treaty Number")]
        public string TreatyNumber { get; set; }

        public AccountCodeMappingViewModel() { }

        public AccountCodeMappingViewModel(AccountCodeMappingBo accountCodeMappingBo)
        {
            if (accountCodeMappingBo != null)
            {
                Id = accountCodeMappingBo.Id;
                ReportType = accountCodeMappingBo.ReportType;
                Type = accountCodeMappingBo.Type;
                TreatyType = accountCodeMappingBo.TreatyType;
                ClaimCode = accountCodeMappingBo.ClaimCode;
                TreatyCodeId = accountCodeMappingBo.TreatyCodeId;
                BusinessOrigin = accountCodeMappingBo.BusinessOrigin;
                AccountCodeId = accountCodeMappingBo.AccountCodeId;
                DebitCreditIndicatorPositive = accountCodeMappingBo.DebitCreditIndicatorPositive;
                DebitCreditIndicatorNegative = accountCodeMappingBo.DebitCreditIndicatorNegative;
                TransactionTypeCodePickListDetailId = accountCodeMappingBo.TransactionTypeCodePickListDetailId;
                RetroRegisterFieldPickListDetailId = accountCodeMappingBo.RetroRegisterFieldPickListDetailId;
                ModifiedContractCodeId = accountCodeMappingBo.ModifiedContractCodeId;
                InvoiceField = accountCodeMappingBo.InvoiceField;
                IsBalanceSheet = accountCodeMappingBo.IsBalanceSheet;
                TreatyNumber = accountCodeMappingBo.TreatyNumber;

                TreatyCodeBo = accountCodeMappingBo.TreatyCodeBo;
                AccountCodeBo = accountCodeMappingBo.AccountCodeBo;
                TransactionTypeCodePickListDetailBo = accountCodeMappingBo.TransactionTypeCodePickListDetailBo;
                RetroRegisterFieldPickListDetailBo = accountCodeMappingBo.RetroRegisterFieldPickListDetailBo;
                ModifiedContractCodeBo = accountCodeMappingBo.ModifiedContractCodeBo;
            }
        }

        public AccountCodeMappingBo FormBo(int createdById, int updatedById)
        {
            var bo = new AccountCodeMappingBo
            {
                Id = Id,
                ReportType = ReportType,
                Type = Type,
                TreatyType = TreatyType,
                TreatyNumber = TreatyNumber,
                TreatyCodeId = TreatyCodeId,
                TreatyCodeBo = TreatyCodeService.Find(TreatyCodeId),
                ClaimCode = ClaimCode,
                BusinessOrigin = BusinessOrigin,
                AccountCodeId = AccountCodeId,
                AccountCodeBo = AccountCodeService.Find(AccountCodeId),
                DebitCreditIndicatorPositive = DebitCreditIndicatorPositive,
                DebitCreditIndicatorNegative = DebitCreditIndicatorNegative,
                TransactionTypeCodePickListDetailId = TransactionTypeCodePickListDetailId,
                TransactionTypeCodePickListDetailBo = PickListDetailService.Find(TransactionTypeCodePickListDetailId),
                RetroRegisterFieldPickListDetailId = RetroRegisterFieldPickListDetailId,
                RetroRegisterFieldPickListDetailBo = PickListDetailService.Find(RetroRegisterFieldPickListDetailId),
                ModifiedContractCodeId = ModifiedContractCodeId,
                ModifiedContractCodeBo = ModifiedContractCodeId.HasValue ? Mfrs17ContractCodeService.Find(ModifiedContractCodeId.Value) : null,
                InvoiceField = InvoiceField,
                IsBalanceSheet = IsBalanceSheet,
                CreatedById = createdById,
                UpdatedById = updatedById,
            };

            return bo;
        }

        public static Expression<Func<AccountCodeMapping, AccountCodeMappingViewModel>> Expression()
        {
            return entity => new AccountCodeMappingViewModel
            {
                Id = entity.Id,
                ReportType = entity.ReportType,
                Type = entity.Type,
                TreatyType = entity.TreatyType,
                TreatyNumber = entity.TreatyNumber,
                ClaimCode = entity.ClaimCode,
                TreatyCodeId = entity.TreatyCodeId,
                BusinessOrigin = entity.BusinessOrigin,
                AccountCodeId = entity.AccountCodeId,
                DebitCreditIndicatorPositive = entity.DebitCreditIndicatorPositive,
                DebitCreditIndicatorNegative = entity.DebitCreditIndicatorNegative,
                TransactionTypeCodePickListDetailId = entity.TransactionTypeCodePickListDetailId,
                RetroRegisterFieldPickListDetailId = entity.RetroRegisterFieldPickListDetailId,
                AccountCodeMappingDetails = entity.AccountCodeMappingDetails,
                ModifiedContractCodeId = entity.ModifiedContractCodeId,
                InvoiceField = entity.InvoiceField,
                IsBalanceSheet = entity.IsBalanceSheet,

                TransactionTypeCodePickListDetail = entity.TransactionTypeCodePickListDetail,
                RetroRegisterFieldPickListDetail = entity.RetroRegisterFieldPickListDetail,
                ModifiedContractCode = entity.ModifiedContractCode,
                TreatyCode = entity.TreatyCode,
                AccountCode = entity.AccountCode,
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (Type != AccountCodeMappingBo.TypeDirectRetro && Type != AccountCodeMappingBo.TypePerLifeRetro)
            {
                if (ReportType == AccountCodeMappingBo.ReportTypeIfrs17 && (ModifiedContractCodeId == null || ModifiedContractCodeId == 0))
                {
                    results.Add(new ValidationResult(
                        string.Format(MessageBag.Required, "Modified Contract Code"),
                        new[] { nameof(ModifiedContractCodeId) }));
                }
            }
            else
            {
                if (ReportType == AccountCodeMappingBo.ReportTypeIfrs17 && string.IsNullOrEmpty(TreatyNumber) && IsBalanceSheet)
                {
                    results.Add(new ValidationResult(
                        string.Format(MessageBag.Required, "Treaty Number"),
                        new[] { nameof(TreatyNumber) }));
                }
            }

            return results;
        }
    }

    public class ValidateTreatyTypeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string[] treatyTypes = value.ToString().ToArraySplitTrim();
                foreach (var treatyType in treatyTypes)
                {
                    if (PickListDetailService.FindByPickListIdCode(PickListBo.TreatyType, treatyType) == null)
                    {
                        return new ValidationResult("Please enter valid Treaty Type.");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }

    public class ValidateClaimCodeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string[] claimCodes = value.ToString().ToArraySplitTrim();
                foreach (var claimCode in claimCodes)
                {
                    if (ClaimCodeService.CountByCodeStatus(claimCode, ClaimCodeBo.StatusActive) == 0)
                    {
                        return new ValidationResult("Please enter valid Claim Code and the status is Active.");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }

    public class ValidateInvoiceFieldAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string[] invoiceFields = value.ToString().ToArraySplitTrim();
                foreach (var invoiceField in invoiceFields)
                {
                    if (PickListDetailService.FindByPickListIdCode(PickListBo.InvoiceField, invoiceField) == null)
                    {
                        return new ValidationResult("Please enter valid Invoice Field.");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }

    public class ValidateBusinessOriginAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string[] businessOrigins = value.ToString().ToArraySplitTrim();
                foreach (var businessOrigin in businessOrigins)
                {
                    if (PickListDetailService.FindByPickListIdCode(PickListBo.BusinessOrigin, businessOrigin) == null)
                    {
                        return new ValidationResult("Please enter valid Business Origin.");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}