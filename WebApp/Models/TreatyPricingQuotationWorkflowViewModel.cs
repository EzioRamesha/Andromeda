using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities;
using DataAccess.Entities.Identity;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Services;
using Services.TreatyPricing;
using Shared;
using Shared.Forms.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class TreatyPricingQuotationWorkflowViewModel : ObjectVersion
    {
        public int ModuleId { get; set; }

        #region Properties - QuotationWorkflow
        public int Id { get; set; }

        public IList<TreatyPricingWorkflowObjectBo> TreatyPricingWorkflowObjectBos { get; set; }

        public int CedantId { get; set; }

        public Cedant Cedant { get; set; }

        public CedantBo CedantBo { get; set; }

        [Required, DisplayName("Quotation ID")]
        public string QuotationId { get; set; }

        [Required, DisplayName("Reinsurance Type")]
        public int? ReinsuranceTypePickListDetailId { get; set; }

        public PickListDetail ReinsuranceTypePickListDetail { get; set; }

        [DisplayName("Reinsurance Type")]
        public string ReinsuranceType { get; set; }

        [Required, StringLength(255), DisplayName("Quotation Name")]
        public string Name { get; set; }

        [Required, DisplayName("Product Summary")]
        public string Summary { get; set; }

        [Required, DisplayName("Quotation Status")]
        public int Status { get; set; }

        [DisplayName("Quotation Status")]
        public string StatusName { get; set; }

        [DisplayName("Quotation Status Remark")]
        public string StatusRemarks { get; set; }

        [DisplayName("Target Send Date")]
        public DateTime? TargetSendDate { get; set; }

        [Required, DisplayName("Target Send Date")]
        public string TargetSendDateStr { get; set; }

        [DisplayName("Latest Revision Date")]
        public DateTime? LatestRevisionDate { get; set; }

        [DisplayName("Latest Revision Date")]
        public string LatestRevisionDateStr { get; set; }

        [Required, DisplayName("Pricing Team")]
        public int? PricingTeamPickListDetailId { get; set; }

        public PickListDetail PricingTeamPickListDetail { get; set; }

        [DisplayName("Pricing Status")]
        public int? PricingStatus { get; set; }

        [DisplayName("Pricing Status")]
        public string PricingStatusName { get; set; }

        [DisplayName("Target Release Date to Client")]
        public DateTime? TargetClientReleaseDate { get; set; }

        [Required, DisplayName("Target Release Date to Client")]
        public string TargetClientReleaseDateStr { get; set; }

        [DisplayName("Target Rate Completion Date")]
        public DateTime? TargetRateCompletionDate { get; set; }

        [DisplayName("Target Rate Completion Date")]
        public string TargetRateCompletionDateStr { get; set; }

        [DisplayName("Finalise Date")]
        public DateTime? FinaliseDate { get; set; }

        [DisplayName("Finalise Date")]
        public string FinaliseDateStr { get; set; }

        [StringLength(255), DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Date Created")]
        public DateTime? CreatedAt { get; set; }

        [DisplayName("Date Created")]
        public string CreatedAtStr { get; set; }

        [DisplayName("Latest Version")]
        public int LatestVersion { get; set; }
        #endregion

        public int VersionId { get; set; }

        public int Version { get; set; }

        #region Properties - QuotationWorkflowVersion - Quotation
        [Required, DisplayName("Business Development Person In-Charge")]
        public int? BDPersonInChargeId { get; set; }

        public string BDPersonInChargeName { get; set; }

        [Required, DisplayName("Quote Validity Day")]
        public int? QuoteValidityDay { get; set; }

        [DisplayName("Template")]
        public string QuoteSpecTemplate { get; set; }

        [DisplayName("Template")]
        public string RateTableTemplate { get; set; }

        [DisplayName("SharePoint Link")]
        public string QuoteSpecSharePointLink { get; set; }

        [DisplayName("SharePoint Path")]
        public string QuoteSpecSharePointFolderPath { get; set; }

        [DisplayName("SharePoint Link")]
        public string RateTableSharePointLink { get; set; }

        [DisplayName("SharePoint Path")]
        public string RateTableSharePointFolderPath { get; set; }

        [DisplayName("Final Campaign / Quote Spec")]
        public string FinalQuoteSpecFileName { get; set; }
        public string FinalQuoteSpecHashFileName { get; set; }

        [DisplayName("Final Rate Table")]
        public string FinalRateTableFileName { get; set; }
        public string FinalRateTableHashFileName { get; set; }
        
        public bool ChecklistFinalised { get; set; }
        #endregion

        #region Properties - QuotationWorkflowVersion - Pricing
        [Required, DisplayName("Pricing")]
        public int? PersonInChargeId { get; set; }

        public string PersonInChargeName { get; set; }

        [DisplayName("Tech Reviewer")]
        public int? PersonInChargeTechReviewerId { get; set; }

        public string PersonInChargeTechReviewerName { get; set; }

        [DisplayName("Peer Reviewer")]
        public int? PersonInChargePeerReviewerId { get; set; }

        public string PersonInChargePeerReviewerName { get; set; }

        [DisplayName("Pricing Authority Reviewer")]
        public int? PersonInChargePricingAuthorityReviewerId { get; set; }

        public string PersonInChargePricingAuthorityReviewerName { get; set; }

        [DisplayName("Pending On")]
        public string PendingOn { get; set; }

        [DisplayName("Request Date")]
        public DateTime? RequestDate { get; set; }

        [DisplayName("Request Date")]
        public string RequestDateStr { get; set; }

        [DisplayName("Target Pricing Due Date")]
        public DateTime? TargetPricingDueDate { get; set; }

        [DisplayName("Target Pricing Due Date")]
        public string TargetPricingDueDateStr { get; set; }

        [DisplayName("Revised Pricing Due Date")]
        public DateTime? RevisedPricingDueDate { get; set; }

        [DisplayName("Revised Pricing Due Date")]
        public string RevisedPricingDueDateStr { get; set; }

        [DisplayName("Pricing Completed Date")]
        public DateTime? PricingCompletedDate { get; set; }

        [DisplayName("Pricing Completed Date")]
        public string PricingCompletedDateStr { get; set; }

        [DisplayName("Profit Margin")]
        public double? ProfitMargin { get; set; }

        [DisplayName("First Year Premium")]
        public double? FirstYearPremium { get; set; }

        [DisplayName("PV of Profit")]
        public double? PVProfit { get; set; }

        [DisplayName("ROE")]
        public double? ROE { get; set; }

        [DisplayName("Expense Margin")]
        public double? ExpenseMargin { get; set; }

        [DisplayName("Profit Margin")]
        [ValidateDouble]
        public string ProfitMarginStr { get; set; }

        [DisplayName("First Year Premium")]
        [ValidateDouble]
        public string FirstYearPremiumStr { get; set; }

        [DisplayName("PV of Profit")]
        [ValidateDouble]
        public string PVProfitStr { get; set; }

        [DisplayName("ROE")]
        [ValidateDouble]
        public string ROEStr { get; set; }

        [DisplayName("Expense Margin")]
        [ValidateDouble]
        public string ExpenseMarginStr { get; set; }

        [DisplayName("Pricing Memo")]
        public string FileLocationPricingMemo { get; set; }

        [DisplayName("NB Checklist")]
        public string FileLocationNBChecklist { get; set; }

        [DisplayName("Technical Checklist")]
        public string FileLocationTechnicalChecklist { get; set; }
        #endregion

        public string QuotationChecklists { get; set; }

        #region Properties - Quotation and Pricing dashboard
        public int CEOPending { get; set; }

        public int PricingPending { get; set; }

        public int UnderwritingPending { get; set; }

        public int HealthPending { get; set; }

        public int ClaimsPending { get; set; }

        public int BDPending { get; set; }

        public int TGPending { get; set; }

        public DateTime? PricingDueDate { get; set; }
        #endregion

        public User BDPersonInCharge { get; set; }

        public User PersonInCharge { get; set; }

        public TreatyPricingQuotationWorkflowViewModel()
        {
            Set();
        }

        public TreatyPricingQuotationWorkflowViewModel(TreatyPricingQuotationWorkflowBo QuotationWorkflowBo)
        {
            Set(QuotationWorkflowBo);
            SetVersionObjects(QuotationWorkflowBo.TreatyPricingQuotationWorkflowVersionBos);
            ModuleId = ModuleService.FindByController(ModuleBo.ModuleController.TreatyPricingQuotationWorkflow.ToString()).Id;
        }

        public void Set(TreatyPricingQuotationWorkflowBo QuotationWorkflowBo = null)
        {
            if (QuotationWorkflowBo != null)
            {
                Id = QuotationWorkflowBo.Id;
                TreatyPricingWorkflowObjectBos = QuotationWorkflowBo.TreatyPricingWorkflowObjectBos;
                CedantId = QuotationWorkflowBo.CedantId;
                CedantBo = QuotationWorkflowBo.CedantBo;
                QuotationId = QuotationWorkflowBo.QuotationId;
                ReinsuranceTypePickListDetailId = QuotationWorkflowBo.ReinsuranceTypePickListDetailId;
                Name = QuotationWorkflowBo.Name;
                Summary = QuotationWorkflowBo.Summary;
                Status = QuotationWorkflowBo.Status;
                StatusRemarks = QuotationWorkflowBo.StatusRemarks;
                TargetSendDate = QuotationWorkflowBo.TargetSendDate;
                TargetSendDateStr = QuotationWorkflowBo.TargetSendDateStr;
                LatestRevisionDate = QuotationWorkflowBo.LatestRevisionDate;
                LatestRevisionDateStr = QuotationWorkflowBo.LatestRevisionDateStr;
                PricingTeamPickListDetailId = QuotationWorkflowBo.PricingTeamPickListDetailId;
                PricingStatus = QuotationWorkflowBo.PricingStatus;
                TargetClientReleaseDate = QuotationWorkflowBo.TargetClientReleaseDate;
                TargetClientReleaseDateStr = QuotationWorkflowBo.TargetClientReleaseDateStr;
                TargetRateCompletionDate = QuotationWorkflowBo.TargetRateCompletionDate;
                TargetRateCompletionDateStr = QuotationWorkflowBo.TargetRateCompletionDateStr;
                FinaliseDate = QuotationWorkflowBo.FinaliseDate;
                FinaliseDateStr = QuotationWorkflowBo.FinaliseDateStr;
                Description = QuotationWorkflowBo.Description;
                LatestVersion = QuotationWorkflowBo.LatestVersion;
                CreatedAt = QuotationWorkflowBo.CreatedAt;
                CreatedAtStr = QuotationWorkflowBo.CreatedAt.ToString(Util.GetDateFormat());

                StatusName = TreatyPricingQuotationWorkflowBo.GetStatusName(QuotationWorkflowBo.Status);
                PricingStatusName = TreatyPricingQuotationWorkflowBo.GetPricingStatusName(QuotationWorkflowBo.PricingStatus);
            }
        }

        public TreatyPricingQuotationWorkflowBo FormBo(TreatyPricingQuotationWorkflowBo bo)
        {
            bo.QuotationId = QuotationId;
            bo.CedantId = CedantId;
            bo.ReinsuranceTypePickListDetailId = ReinsuranceTypePickListDetailId;
            bo.Name = Name;
            bo.Summary = Summary;
            bo.Status = Status;
            bo.StatusRemarks = StatusRemarks;
            bo.TargetSendDate = Util.GetParseDateTime(TargetSendDateStr);
            bo.LatestRevisionDate = Util.GetParseDateTime(LatestRevisionDateStr);
            bo.PricingTeamPickListDetailId = PricingTeamPickListDetailId;
            bo.PricingStatus = PricingStatus;
            bo.TargetClientReleaseDate = Util.GetParseDateTime(TargetClientReleaseDateStr);
            bo.TargetRateCompletionDate = Util.GetParseDateTime(TargetRateCompletionDateStr);
            bo.FinaliseDate = Util.GetParseDateTime(FinaliseDateStr);
            bo.Description = Description;

            return bo;
        }
        
        public static Expression<Func<TreatyPricingQuotationWorkflow, TreatyPricingQuotationWorkflowViewModel>> Expression()
        {
            using (var db = new AppDbContext())
            {
                return entity => new TreatyPricingQuotationWorkflowViewModel()
                {
                    Id = entity.Id,
                    QuotationId = entity.QuotationId,
                    CreatedAt = entity.CreatedAt,
                    CedantId = entity.CedantId,
                    Cedant = entity.Cedant,
                    ReinsuranceTypePickListDetailId = entity.ReinsuranceTypePickListDetailId,
                    ReinsuranceTypePickListDetail = entity.ReinsuranceTypePickListDetail,
                    Name = entity.Name,
                    Description = entity.Description,
                    LatestVersion = entity.LatestVersion,

                    Status = entity.Status,
                    PricingStatus = entity.PricingStatus,

                    //Quotation and Pricing dashboard
                    PricingTeamPickListDetailId = entity.PricingTeamPickListDetailId,
                    PricingTeamPickListDetail = entity.PricingTeamPickListDetail,
                    BDPersonInChargeId = entity.BDPersonInChargeId,
                    PersonInChargeId = entity.PersonInChargeId,
                    CEOPending = entity.CEOPending,
                    PricingPending = entity.PricingPending,
                    UnderwritingPending = entity.UnderwritingPending,
                    HealthPending = entity.HealthPending,
                    ClaimsPending = entity.ClaimsPending,
                    BDPending = entity.BDPending,
                    TGPending = entity.TGPending,
                    PricingDueDate = entity.PricingDueDate,
                    BDPersonInCharge = entity.BDPersonInCharge,
                    PersonInCharge = entity.PersonInCharge,
                };
            }
        }

        public TreatyPricingQuotationWorkflowVersionBo GetVersionBo(TreatyPricingQuotationWorkflowVersionBo bo)
        {
            //Quotation
            bo.BDPersonInChargeId = BDPersonInChargeId;
            bo.BDPersonInChargeName = BDPersonInChargeName;
            bo.QuoteValidityDay = QuoteValidityDay;
            bo.QuoteSpecTemplate = QuoteSpecTemplate;
            bo.RateTableTemplate = RateTableTemplate;
            bo.QuoteSpecSharePointLink = QuoteSpecSharePointLink;
            bo.QuoteSpecSharePointFolderPath = QuoteSpecSharePointFolderPath;
            bo.RateTableSharePointLink = RateTableSharePointLink;
            bo.RateTableSharePointFolderPath = RateTableSharePointFolderPath;
            bo.FinalQuoteSpecFileName = FinalQuoteSpecFileName;
            bo.FinalQuoteSpecHashFileName = FinalQuoteSpecHashFileName;
            bo.FinalRateTableFileName = FinalRateTableFileName;
            bo.FinalRateTableHashFileName = FinalRateTableHashFileName;
            bo.QuotationChecklists = QuotationChecklists;
            bo.ChecklistFinalised = ChecklistFinalised;

            //Pricing
            bo.PersonInChargeId = PersonInChargeId;
            bo.PersonInChargeName = PersonInChargeName;
            bo.PersonInChargeTechReviewerId = PersonInChargeTechReviewerId;
            bo.PersonInChargeTechReviewerName = PersonInChargeTechReviewerName;
            bo.PersonInChargePeerReviewerId = PersonInChargePeerReviewerId;
            bo.PersonInChargePeerReviewerName = PersonInChargePeerReviewerName;
            bo.PersonInChargePricingAuthorityReviewerId = PersonInChargePricingAuthorityReviewerId;
            bo.PersonInChargePricingAuthorityReviewerName = PersonInChargePricingAuthorityReviewerName;
            bo.PendingOn = PendingOn;
            bo.RequestDateStr = RequestDateStr;
            bo.RequestDate = RequestDateStr is null || RequestDateStr == "" ? null : Util.GetParseDateTime(RequestDateStr);
            bo.TargetPricingDueDateStr = TargetPricingDueDateStr;
            bo.TargetPricingDueDate = TargetPricingDueDateStr is null || TargetPricingDueDateStr == "" ? null : Util.GetParseDateTime(TargetPricingDueDateStr);
            bo.RevisedPricingDueDateStr = RevisedPricingDueDateStr;
            bo.RevisedPricingDueDate = RevisedPricingDueDateStr is null || RevisedPricingDueDateStr == "" ? null : Util.GetParseDateTime(RevisedPricingDueDateStr);
            bo.PricingCompletedDateStr = PricingCompletedDateStr;
            bo.PricingCompletedDate = PricingCompletedDateStr is null || PricingCompletedDateStr == "" ? null : Util.GetParseDateTime(PricingCompletedDateStr);
            bo.ProfitMargin = ProfitMarginStr is null || ProfitMarginStr == "" ? null : Util.StringToDouble(ProfitMarginStr);
            bo.FirstYearPremium = FirstYearPremiumStr is null || FirstYearPremiumStr == "" ? null : Util.StringToDouble(FirstYearPremiumStr);
            bo.PVProfit = PVProfitStr is null || PVProfitStr == "" ? null : Util.StringToDouble(PVProfitStr);
            bo.ROE = ROEStr is null || ROEStr == "" ? null : Util.StringToDouble(ROEStr);
            bo.ExpenseMargin = ExpenseMarginStr is null || ExpenseMarginStr == "" ? null : Util.StringToDouble(ExpenseMarginStr);
            bo.FileLocationPricingMemo = FileLocationPricingMemo;
            bo.FileLocationNBChecklist = FileLocationNBChecklist;
            bo.FileLocationTechnicalChecklist = FileLocationTechnicalChecklist;

            return bo;
        }
    }
}