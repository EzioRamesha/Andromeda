using BusinessObject.RiDatas;
using DataAccess.Entities.RiDatas;
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
    public class RiDataSearchViewModel
    {
        public int Id { get; set; }

        [DisplayName("Treaty Code")]
        public string TreatyCode { get; set; }

        [DisplayName("SOA Quarter")]
        public string SoaQuarter { get; set; }

        [DisplayName("Risk Period Start Date")]
        public DateTime? RiskPeriodStartDate { get; set; }

        [DisplayName("Risk Period End Date")]
        public DateTime? RiskPeriodEndDate { get; set; }

        [DisplayName("Risk Period Start Date")]
        public string RiskPeriodStartDateStr { get; set; }

        [DisplayName("Risk Period End Date")]
        public string RiskPeriodEndDateStr { get; set; }

        [DisplayName("Insured Name")]
        public string InsuredName { get; set; }

        [DisplayName("Insured Gender Code")]
        public string InsuredGenderCode { get; set; }

        [DisplayName("Insured Tobacco Use")]
        public string InsuredTobaccoUse { get; set; }

        [DisplayName("Policy Number")]
        public string PolicyNumber { get; set; }

        [DisplayName("Insured Date of Birth")]
        public DateTime? InsuredDateOfBirth { get; set; }

        [DisplayName("Insured Date of Birth")]
        public string InsuredDateOfBirthStr { get; set; }

        [DisplayName("Insured Occuption Code")]
        public string InsuredOccupationCode { get; set; }

        [DisplayName("Insured Attained Age")]
        public int? InsuredAttainedAge { get; set; }

        [DisplayName("Ceding Plan Code")]
        public string CedingPlanCode { get; set; }

        [DisplayName("Ceding Benefit Type Code")]
        public string CedingBenefitTypeCode { get; set; }

        [DisplayName("Ceding Benefit Risk Code")]
        public string CedingBenefitRiskCode { get; set; }

        [DisplayName("MLRe Benefit Code")]
        public string BenefitCode { get; set; }

        [DisplayName("Ori Sum Assured")]
        public double? OriSumAssured { get; set; }

        [DisplayName("Current Sum Assured")]
        public double? CurrSumAssured { get; set; }

        [DisplayName("Amount Ceded B4 MLRe Share")]
        public double? AmountCededB4MlreShare { get; set; }

        [DisplayName("Retention Amount")]
        public double? RetentionAmount { get; set; }

        [DisplayName("AAR Ori")]
        public double? AarOri { get; set; }

        [DisplayName("AAR")]
        public double? Aar { get; set; }

        [DisplayName("Reinsurance Basis Code")]
        public string ReinsBasisCode { get; set; }

        [DisplayName("Funds Accounting Type Code")]
        public string FundsAccountingTypeCode { get; set; }

        [DisplayName("Premium Frequency Code")]
        public string PremiumFrequencyCode { get; set; }

        [DisplayName("Report Period Month")]
        public int? ReportPeriodMonth { get; set; }

        [DisplayName("Report Period Year")]
        public int? ReportPeriodYear { get; set; }

        [DisplayName("Risk Period Month")]
        public int? RiskPeriodMonth { get; set; }

        [DisplayName("Risk Period Year")]
        public int? RiskPeriodYear { get; set; }

        [DisplayName("Transaction Type Code")]
        public string TransactionTypeCode { get; set; }

        [DisplayName("Issue Date Policy")]
        public DateTime? IssueDatePol { get; set; }

        [DisplayName("Issue Date Benefit")]
        public DateTime? IssueDateBen { get; set; }

        [DisplayName("Reinsurance Effective Date Policy")]
        public DateTime? ReinsEffDatePol { get; set; }

        [DisplayName("Reinsurance Effective Date Benefit")]
        public DateTime? ReinsEffDateBen { get; set; }

        public static Expression<Func<RiData, RiDataSearchViewModel>> Expression()
        {
            return entity => new RiDataSearchViewModel
            {
                Id = entity.Id,
                TreatyCode = entity.TreatyCode,
                //SoaQuarter = entity.RiDataBatch.Quarter,
                RiskPeriodStartDate = entity.RiskPeriodStartDate,
                RiskPeriodEndDate = entity.RiskPeriodEndDate,
                InsuredName = entity.InsuredName,
                InsuredGenderCode = entity.InsuredGenderCode,
                InsuredTobaccoUse = entity.InsuredTobaccoUse,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                InsuredOccupationCode = entity.InsuredOccupationCode,
                InsuredAttainedAge = entity.InsuredAttainedAge,
                CedingPlanCode = entity.CedingPlanCode,
                CedingBenefitTypeCode = entity.CedingBenefitTypeCode,
                CedingBenefitRiskCode = entity.CedingBenefitRiskCode,
                BenefitCode = entity.MlreBenefitCode,
                OriSumAssured = entity.OriSumAssured,
                CurrSumAssured = entity.CurrSumAssured,
                AmountCededB4MlreShare = entity.AmountCededB4MlreShare,
                RetentionAmount = entity.RetentionAmount,
                AarOri = entity.AarOri,
                Aar = entity.Aar,
                ReinsBasisCode = entity.ReinsBasisCode,
                FundsAccountingTypeCode = entity.FundsAccountingTypeCode,
                PremiumFrequencyCode = entity.PremiumFrequencyCode,
                ReportPeriodMonth = entity.ReportPeriodMonth,
                ReportPeriodYear = entity.ReportPeriodYear,
                RiskPeriodMonth = entity.RiskPeriodMonth,
                RiskPeriodYear = entity.RiskPeriodYear,
                TransactionTypeCode = entity.TransactionTypeCode,
                PolicyNumber = entity.PolicyNumber,
                IssueDatePol = entity.IssueDatePol,
                IssueDateBen = entity.IssueDateBen,
                ReinsEffDatePol = entity.ReinsEffDatePol,
                ReinsEffDateBen = entity.ReinsEffDateBen,
            };
        }
    }
}