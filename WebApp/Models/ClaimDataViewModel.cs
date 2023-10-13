using BusinessObject;
using BusinessObject.Claims;
using DataAccess.Entities.Claims;
using Shared;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class ClaimDataViewModel
    {
        public int Id { get; set; }

        public int ClaimDataBatchId { get; set; }
        public virtual ClaimDataBatchBo ClaimDataBatchBo { get; set; }

        public int? ClaimDataFileId { get; set; }
        public virtual ClaimDataFileBo ClaimDataFileBo { get; set; }

        public string ClaimId { get; set; }
        public string ClaimCode { get; set; }

        public bool CopyAndOverwriteData { get; set; } = false;

        public bool IgnoreFinalise { get; set; } = false;

        [Display(Name = "Mapping Status")]
        public int MappingStatus { get; set; }
        [Display(Name = "Pre-Computation Status")]
        public int PreComputationStatus { get; set; }
        [Display(Name = "Pre-Validation Status")]
        public int PreValidationStatus { get; set; }
        [Display(Name = "Reporting Status")]
        public int ReportingStatus { get; set; }

        public string Errors { get; set; }
        public string CustomField { get; set; }

        [Display(Name = "Policy Number")]
        public string PolicyNumber { get; set; }

        [Display(Name = "Policy Term")]
        public double? PolicyTerm { get; set; }

        public double? ClaimRecoveryAmt { get; set; }
        [Display(Name = "Claim Amount")]
        public string ClaimRecoveryAmtStr { get; set; }

        [Display(Name = "Claim Transaction Type")]
        public string ClaimTransactionType { get; set; }

        [Display(Name = "Treaty Code")]
        public string TreatyCode { get; set; }

        public string TreatyType { get; set; }

        public double? AarPayable { get; set; }

        public double? AnnualRiPrem { get; set; }

        public string CauseOfEvent { get; set; }

        public string CedantClaimEventCode { get; set; }

        public string CedantClaimType { get; set; }

        public DateTime? CedantDateOfNotification { get; set; }

        public string CedingBenefitRiskCode { get; set; }

        public string CedingBenefitTypeCode { get; set; }

        public string CedingClaimType { get; set; }

        public string CedingCompany { get; set; }

        public string CedingEventCode { get; set; }

        public string CedingPlanCode { get; set; }

        public double? CurrencyRate { get; set; }

        public string CurrencyCode { get; set; }

        public DateTime? DateApproved { get; set; }

        public DateTime? DateOfEvent { get; set; }

        public string EntryNo { get; set; }

        public double? ExGratia { get; set; }

        public double? ForeignClaimRecoveryAmt { get; set; }

        public string FundsAccountingTypeCode { get; set; }

        public DateTime? InsuredDateOfBirth { get; set; }

        public string InsuredGenderCode { get; set; }

        public string InsuredName { get; set; }

        public string InsuredTobaccoUse { get; set; }

        public DateTime? LastTransactionDate { get; set; }

        public string LastTransactionQuarter { get; set; }

        public double? LateInterest { get; set; }

        public double? Layer1SumRein { get; set; }

        public int? Mfrs17AnnualCohort { get; set; }

        public string Mfrs17ContractCode { get; set; }

        public string MlreBenefitCode { get; set; }

        public string MlreEventCode { get; set; }

        public DateTime? MlreInvoiceDate { get; set; }

        public string MlreInvoiceNumber { get; set; }

        public double? MlreRetainAmount { get; set; }

        public double? MlreShare { get; set; }

        public int? PendingProvisionDay { get; set; }

        public int? PolicyDuration { get; set; }

        public string ReinsBasisCode { get; set; }

        public DateTime? ReinsEffDatePol { get; set; }

        public string RetroParty1 { get; set; }

        public string RetroParty2 { get; set; }

        public string RetroParty3 { get; set; }

        public double? RetroRecovery1 { get; set; }

        public double? RetroRecovery2 { get; set; }

        public double? RetroRecovery3 { get; set; }

        public DateTime? RetroStatementDate1 { get; set; }

        public DateTime? RetroStatementDate2 { get; set; }

        public DateTime? RetroStatementDate3 { get; set; }

        public string RetroStatementId1 { get; set; }

        public string RetroStatementId2 { get; set; }

        public string RetroStatementId3 { get; set; }

        public int? RiskPeriodMonth { get; set; }

        public int? RiskPeriodYear { get; set; }

        public string RiskQuarter { get; set; }

        public double? SaFactor { get; set; }

        public string SoaQuarter { get; set; }

        public double? SumIns { get; set; }

        public double? TempA1 { get; set; }

        public double? TempA2 { get; set; }

        public DateTime? TempD1 { get; set; }

        public DateTime? TempD2 { get; set; }

        public int? TempI1 { get; set; }

        public int? TempI2 { get; set; }

        public string TempS1 { get; set; }

        public string TempS2 { get; set; }

        public DateTime? TransactionDateWop { get; set; }

        public DateTime? IssueDatePol { get; set; }

        public DateTime? PolicyExpiryDate { get; set; }

        public DateTime? DateOfReported { get; set; }

        public string CedingTreatyCode { get; set; }

        public string CampaignCode { get; set; }

        public DateTime? DateOfIntimation { get; set; }

        public ClaimDataViewModel() {}
        
        public ClaimDataViewModel(ClaimDataBo claimDataBo)
        {
            Set(claimDataBo);
        }

        public void Set(ClaimDataBo claimDataBo)
        {
            foreach (var property in typeof(ClaimDataViewModel).GetProperties())
            {
                string propertyName = property.Name;
                object boValue = claimDataBo.GetPropertyValue(propertyName);
                this.SetPropertyValue(propertyName, boValue);
            }

            if (ClaimRecoveryAmt.HasValue)
                ClaimRecoveryAmtStr = Util.DoubleToString(ClaimRecoveryAmt);
        }

        public object GetHtmlViewData(int dataType)
        {
            string inputClass = "form-control";
            string placeholder = "Type here";
            string type = "text";

            switch (dataType)
            {
                case StandardOutputBo.DataTypeDate:
                    placeholder = "DD MM YYYY";
                    inputClass += " datepicker";
                    break;
                case StandardOutputBo.DataTypeAmount:
                case StandardOutputBo.DataTypePercentage:
                    inputClass += " text-right";
                    break;
                case StandardOutputBo.DataTypeInteger:
                    type = "number";
                    break;
            }

            var viewData = new
            {
                htmlAttributes = new
                {
                    @class = inputClass,
                    placeholder = placeholder,
                    type = type
                }
            };

            return viewData;
        }
    }

    public class ClaimDataListViewModel
    {
        public int Id { get; set; }

        public int ClaimDataBatchId { get; set; }

        public int? ClaimDataFileId { get; set; }

        public string EntryNo { get; set; }

        public string ClaimTransactionType { get; set; }

        public string MlreEventCode { get; set; }

        public string ClaimCode { get; set; }

        public string PolicyNumber { get; set; }

        public string TreatyType { get; set; }

        public string InsuredName { get; set; }

        public string InsuredGenderCode { get; set; }

        public double? Layer1SumRein { get; set; }

        public int MappingStatus { get; set; }
        public int PreComputationStatus { get; set; }
        public int PreValidationStatus { get; set; }
        public int ReportingStatus { get; set; }

        public static Expression<Func<ClaimData, ClaimDataListViewModel>> Expression()
        {
            return entity => new ClaimDataListViewModel
            {
                Id = entity.Id,
                ClaimDataBatchId = entity.ClaimDataBatchId,
                ClaimDataFileId = entity.ClaimDataFileId,
                EntryNo = entity.EntryNo,
                ClaimTransactionType = entity.ClaimTransactionType,
                MlreEventCode = entity.MlreEventCode,
                ClaimCode = entity.ClaimCode,
                PolicyNumber = entity.PolicyNumber,
                TreatyType = entity.TreatyType,
                InsuredName = entity.InsuredName,
                InsuredGenderCode = entity.InsuredGenderCode,
                Layer1SumRein = entity.Layer1SumRein,
                MappingStatus = entity.MappingStatus,
                PreComputationStatus = entity.PreComputationStatus,
                PreValidationStatus = entity.PreValidationStatus,
                ReportingStatus = entity.ReportingStatus,
            };
        }
    }
}