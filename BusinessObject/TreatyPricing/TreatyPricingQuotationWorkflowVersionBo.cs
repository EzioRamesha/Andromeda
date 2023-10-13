using Shared;
using Shared.Trails.Attributes;
using System;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingQuotationWorkflowVersionBo
    {
        public int Id { get; set; }

        public int TreatyPricingQuotationWorkflowId { get; set; }

        public virtual TreatyPricingQuotationWorkflowBo TreatyPricingQuotationWorkflowBo { get; set; }

        public int Version { get; set; }

        #region Quotation
        public int? BDPersonInChargeId { get; set; }

        public int? QuoteValidityDay { get; set; }

        public string QuoteSpecTemplate { get; set; }

        public string RateTableTemplate { get; set; }

        public string QuoteSpecSharePointLink { get; set; }

        public string QuoteSpecSharePointFolderPath { get; set; }

        public string RateTableSharePointLink { get; set; }

        public string RateTableSharePointFolderPath { get; set; }

        public string FinalQuoteSpecFileName { get; set; }

        public string FinalQuoteSpecHashFileName { get; set; }

        public string FinalRateTableFileName { get; set; }

        public string FinalRateTableHashFileName { get; set; }

        public string BDPersonInChargeName { get; set; }

        public bool ChecklistFinalised { get; set; }
        #endregion

        #region Pricing
        public int? PersonInChargeId { get; set; }

        public int? PersonInChargeTechReviewerId { get; set; }

        public int? PersonInChargePeerReviewerId { get; set; }

        public int? PersonInChargePricingAuthorityReviewerId { get; set; }

        public string PendingOn { get; set; }

        public DateTime? RequestDate { get; set; }

        public DateTime? TargetPricingDueDate { get; set; }

        public DateTime? RevisedPricingDueDate { get; set; }

        public DateTime? PricingCompletedDate { get; set; }

        public double? ProfitMargin { get; set; }

        public double? FirstYearPremium { get; set; }

        public double? PVProfit { get; set; }

        public double? ROE { get; set; }

        public double? ExpenseMargin { get; set; }

        public string ProfitMarginStr { get; set; }

        public string FirstYearPremiumStr { get; set; }

        public string PVProfitStr { get; set; }

        public string ROEStr { get; set; }

        public string ExpenseMarginStr { get; set; }

        public string FileLocationPricingMemo { get; set; }

        public string FileLocationNBChecklist { get; set; }

        public string FileLocationTechnicalChecklist { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string RequestDateStr { get; set; }

        public string TargetPricingDueDateStr { get; set; }

        public string RevisedPricingDueDateStr { get; set; }

        public string PricingCompletedDateStr { get; set; }

        public string PersonInChargeName { get; set; }

        public string PersonInChargeTechReviewerName { get; set; }

        public string PersonInChargePeerReviewerName { get; set; }

        public string PersonInChargePricingAuthorityReviewerName { get; set; }
        #endregion

        [IsJsonProperty("QuotationChecklistId")]
        public string QuotationChecklists { get; set; }

        public TreatyPricingQuotationWorkflowVersionBo()
        {

        }

        public TreatyPricingQuotationWorkflowVersionBo(TreatyPricingQuotationWorkflowVersionBo bo)
        {
            TreatyPricingQuotationWorkflowId = bo.TreatyPricingQuotationWorkflowId;

            //Quotation
            BDPersonInChargeId = bo.BDPersonInChargeId;
            QuoteValidityDay = bo.QuoteValidityDay;
            QuoteSpecTemplate = bo.QuoteSpecTemplate;
            RateTableTemplate = bo.RateTableTemplate;
            QuoteSpecSharePointLink = bo.QuoteSpecSharePointLink;
            QuoteSpecSharePointFolderPath = bo.QuoteSpecSharePointFolderPath;
            RateTableSharePointLink = bo.RateTableSharePointLink;
            RateTableSharePointFolderPath = bo.RateTableSharePointFolderPath;
            FinalQuoteSpecFileName = bo.FinalQuoteSpecFileName;
            FinalQuoteSpecHashFileName = bo.FinalQuoteSpecHashFileName;
            FinalRateTableFileName = bo.FinalRateTableFileName;
            FinalRateTableHashFileName = bo.FinalRateTableHashFileName;
            QuotationChecklists = bo.QuotationChecklists;
            ChecklistFinalised = bo.ChecklistFinalised;

            //Pricing
            PersonInChargeId = bo.PersonInChargeId;
            PersonInChargeTechReviewerId = bo.PersonInChargeTechReviewerId;
            PersonInChargePeerReviewerId = bo.PersonInChargePeerReviewerId;
            PersonInChargePricingAuthorityReviewerId = bo.PersonInChargePricingAuthorityReviewerId;
            PendingOn = bo.PendingOn;
            RequestDate = bo.RequestDate;
            TargetPricingDueDate = bo.TargetPricingDueDate;
            RevisedPricingDueDate = bo.RevisedPricingDueDate;
            PricingCompletedDate = bo.PricingCompletedDate;
            ProfitMargin = bo.ProfitMargin;
            FirstYearPremium = bo.FirstYearPremium;
            PVProfit = bo.PVProfit;
            ROE = bo.ROE;
            ExpenseMargin = bo.ExpenseMargin;
            FileLocationPricingMemo = bo.FileLocationPricingMemo;
            FileLocationNBChecklist = bo.FileLocationNBChecklist;
            FileLocationTechnicalChecklist = bo.FileLocationTechnicalChecklist;

            RequestDateStr = bo.RequestDateStr;
            TargetPricingDueDateStr = bo.TargetPricingDueDateStr;
            RevisedPricingDueDateStr = bo.RevisedPricingDueDateStr;
            PricingCompletedDateStr = bo.PricingCompletedDateStr;
        }

        public string GetLocalPath(string type)
        {
            string hashFileName = type == "QuoteSpec" ? FinalQuoteSpecHashFileName : FinalRateTableHashFileName;
            string path = hashFileName == null ? "" : Path.Combine(Util.GetTreatyPricingQuotationWorkflowUploadPath(type), hashFileName);

            return path;
        }
    }
}
