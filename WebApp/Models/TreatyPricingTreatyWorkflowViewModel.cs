using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities;
using DataAccess.Entities.Identity;
using DataAccess.Entities.TreatyPricing;
using Services;
using Services.TreatyPricing;
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
    public class TreatyPricingTreatyWorkflowViewModel : ObjectVersion
    {
        public int Id { get; set; }

        public int TreatyPricingTreatyWorkflowId { get; set; }

        public int ModuleId { get; set; }

        public IList<TreatyPricingWorkflowObjectBo> TreatyPricingWorkflowObjectBos { get; set; }

        [Required, DisplayName("Document Type")]
        public int DocumentType { get; set; }
        public string DocumentTypeName { get; set; }

        [Required, DisplayName("Reinsurance Type")]
        public int ReinsuranceTypePickListDetailId { get; set; }
        [DisplayName("Reinsurance Type")]
        public PickListDetail ReinsuranceTypePickListDetail { get; set; }
        public PickListDetailBo ReinsuranceTypePickListDetailBo { get; set; }

        [Required,DisplayName("Counter Party")]
        public int CounterPartyDetailId { get; set; }
        [DisplayName("Counter Party")]
        public Cedant CounterPartyDetail { get; set; }
        public CedantBo CounterPartyDetailBo { get; set; }

        [DisplayName("Inward Retro Party")]
        public int? InwardRetroPartyDetailId { get; set; }
        [DisplayName("Inward Retro Party")]
        public RetroParty InwardRetroPartyDetail { get; set; }
        public RetroPartyBo InwardRetroPartyDetailBo { get; set; }

        [DisplayName("Business Origin")]
        public int? BusinessOriginPickListDetailId { get; set; }
        [DisplayName("Business Origin")]
        public PickListDetail BusinessOriginPickListDetail { get; set; }
        public PickListDetailBo BusinessOriginPickListDetailBo { get; set; }

        [DisplayName("Type of Business")]
        public string TypeOfBusiness { get; set; }

        [DisplayName("Country Origin")]
        public string CountryOrigin { get; set; }

        [DisplayName("Document ID")]
        public string DocumentId { get; set; }

        [DisplayName("Treaty Code")]
        public string TreatyCode { get; set; }

        [DisplayName("Coverage Status")]
        public int? CoverageStatus { get; set; }
        [DisplayName("Coverage Status")]
        public string CoverageStatusName { get; set; }


        [DisplayName("Document Status")]
        public int? DocumentStatus { get; set; }
        [DisplayName("Document Status")]
        public string DocumentStatusName { get; set; }

        [DisplayName("Drafting Status")]
        public int? DraftingStatus { get; set; }
        [DisplayName("Drafting Status")]
        public string DraftingStatusName { get; set; }

        [DisplayName("Drafting Status Category")]
        public int? DraftingStatusCategory { get; set; }
        [DisplayName("Drafting Status Category")]
        public string DraftingStatusCategoryName { get; set; }

        [DisplayName("Effective Date")]
        public DateTime? EffectiveAt { get; set; }
        [DisplayName("Effective Date")]
        public string EffectiveAtStr { get; set; }

        [DisplayName("ORION Group")]
        public string OrionGroupStr { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Sharepoint Link")]
        public string SharepointLink { get; set; }

        public TreatyPricingTreatyWorkflow TreatyPricingTreatyWorkflow { get; set; }

        public string Reviewer { get; set; }

        [DisplayName("Latest Version")]
        public int LatestVersion { get; set; }

        public int VersionId { get; set; }
        public int Version { get; set; }


        // General Tab
        [DisplayName("Request Date")]
        public DateTime? RequestDate { get; set; }
        [Required, DisplayName("Request Date")]
        public string RequestDateStr { get; set; }

        [DisplayName("Target Sent Date")]
        public DateTime? TargetSentDate { get; set; }
        [DisplayName("Target Sent Date")]
        public string TargetSentDateStr { get; set; }

        [DisplayName("Sent Date to Reviewer (1st)")]
        public DateTime? DateSentToReviewer1st { get; set; }
        [DisplayName("Sent Date to Reviewer (1st)")]
        public string DateSentToReviewer1stStr { get; set; }

        [DisplayName("Sent Date to Client (1st)")]
        public DateTime? DateSentToClient1st { get; set; }
        [DisplayName("Sent Date to Client (1st)")]
        public string DateSentToClient1stStr { get; set; }

        [DisplayName("Latest Revision Date")]
        public DateTime? LatestRevisionDate { get; set; }
        [DisplayName("Latest Revision Date")]
        public string LatestRevisionDateStr { get; set; }

        [DisplayName("Signed Date")]
        public DateTime? SignedDate { get; set; }
        [DisplayName("Signed Date")]
        public string SignedDateStr { get; set; }

        [DisplayName("Reported Date")]
        public DateTime? ReportedDate { get; set; }
        [DisplayName("Reported Date")]
        public string ReportedDateStr { get; set; }

        [DisplayName("Person In-Charge")]
        public int? PersonInChargeId { get; set; }
        public User PersonInCharge { get; set; }
        [DisplayName("Person In-Charge")]
        public string PersonInChargeName { get; set; }

        [DisplayName("Date Created")]
        public DateTime? CreatedAt { get; set; }

        [DisplayName("Date Created")]
        public string CreatedAtStr { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public TreatyPricingTreatyWorkflowViewModel() {
            Set();
        }

        public TreatyPricingTreatyWorkflowViewModel(TreatyPricingTreatyWorkflowBo treatyPricingTreatyWorkflowBo)
        {
            Set(treatyPricingTreatyWorkflowBo);
            SetVersionObjects(treatyPricingTreatyWorkflowBo.TreatyPricingTreatyWorkflowVersionBos);

            ModuleId = ModuleService.FindByController(ModuleBo.ModuleController.TreatyPricingTreatyWorkflow.ToString()).Id;
        }

        public void Set(TreatyPricingTreatyWorkflowBo bo = null)
        {
            if (bo != null)
            {
                Id = bo.Id;
                DocumentType = bo.DocumentType;
                DocumentTypeName = TreatyPricingTreatyWorkflowBo.GetDocumentTypeName(bo.DocumentType);
                ReinsuranceTypePickListDetailId = bo.ReinsuranceTypePickListDetailId;
                ReinsuranceTypePickListDetailBo = bo.ReinsuranceTypePickListDetailBo;
                CounterPartyDetailId = bo.CounterPartyDetailId;
                CounterPartyDetailBo = bo.CounterPartyDetailBo;
                InwardRetroPartyDetailId = bo.InwardRetroPartyDetailId;
                InwardRetroPartyDetailBo = bo.InwardRetroPartyDetailBo;
                BusinessOriginPickListDetailId = bo.BusinessOriginPickListDetailId;
                BusinessOriginPickListDetailBo = bo.BusinessOriginPickListDetailBo;
                TypeOfBusiness = bo.TypeOfBusiness;
                CountryOrigin = bo.CountryOrigin;
                DocumentId = bo.DocumentId;
                TreatyCode = bo.TreatyCode;
                CoverageStatus = bo.CoverageStatus;
                CoverageStatusName = TreatyPricingTreatyWorkflowBo.GetCoverageStatusName(bo.CoverageStatus);
                DocumentStatus = bo.DocumentStatus;
                DocumentStatusName = TreatyPricingTreatyWorkflowBo.GetDocumentStatusName(bo.DocumentStatus);
                DraftingStatus = bo.DraftingStatus;
                DraftingStatusName = TreatyPricingTreatyWorkflowBo.GetDraftingStatusName(bo.DraftingStatus);
                DraftingStatusCategory = bo.DraftingStatusCategory;
                DraftingStatusCategoryName = TreatyPricingTreatyWorkflowBo.GetDraftingStatusCategoryName(bo.DraftingStatusCategory);
                EffectiveAt = bo.EffectiveAt;
                EffectiveAtStr = bo.EffectiveAt?.ToString(Util.GetDateFormat());
                OrionGroupStr = bo.OrionGroupStr;
                Description = bo.Description;
                SharepointLink = bo.SharepointLink;
                Reviewer = bo.Reviewer;
                LatestVersion = bo.LatestVersion;

                //Object
                TreatyPricingWorkflowObjectBos = bo.TreatyPricingWorkflowObjectBos;
                CreatedAt = bo.CreatedAt;
                CreatedAtStr = bo.CreatedAt.ToString(Util.GetDateFormat());
            }
        }

        public TreatyPricingTreatyWorkflowBo FormBo(TreatyPricingTreatyWorkflowBo bo)
        {
            bo.DocumentType = DocumentType;
            bo.ReinsuranceTypePickListDetailId = ReinsuranceTypePickListDetailId;
            bo.CounterPartyDetailId = CounterPartyDetailId;
            bo.InwardRetroPartyDetailId = InwardRetroPartyDetailId;
            bo.BusinessOriginPickListDetailId = BusinessOriginPickListDetailId;
            bo.TypeOfBusiness = TypeOfBusiness;
            bo.CountryOrigin = CountryOrigin;
            bo.DocumentId = DocumentId;
            bo.TreatyCode = TreatyCode;
            bo.CoverageStatus = CoverageStatus;
            bo.DocumentStatus = DocumentStatus;
            bo.DraftingStatus = DraftingStatus;
            bo.DraftingStatusCategory = DraftingStatusCategory;
            bo.EffectiveAt = EffectiveAtStr is null || EffectiveAtStr == "" ? null : Util.GetParseDateTime(EffectiveAtStr);
            bo.OrionGroupStr = OrionGroupStr;
            bo.Description = Description;
            bo.SharepointLink = SharepointLink;
            bo.Reviewer = Reviewer;

            return bo;
        }

        public static Expression<Func<TreatyPricingTreatyWorkflow, TreatyPricingTreatyWorkflowViewModel>> Expression()
        {
            return entity => new TreatyPricingTreatyWorkflowViewModel
            {
                Id = entity.Id,
                ReinsuranceTypePickListDetailId = entity.ReinsuranceTypePickListDetailId,
                ReinsuranceTypePickListDetail = entity.ReinsuranceTypePickListDetail,
                CounterPartyDetailId = entity.CounterPartyDetailId,
                CounterPartyDetail = entity.CounterPartyDetail,
                InwardRetroPartyDetailId = entity.InwardRetroPartyDetailId,
                InwardRetroPartyDetail = entity.InwardRetroPartyDetail,
                DocumentType = entity.DocumentType,
                DocumentId = entity.DocumentId,
                BusinessOriginPickListDetailId = entity.BusinessOriginPickListDetailId,
                TypeOfBusiness = entity.TypeOfBusiness,
                CountryOrigin = entity.CountryOrigin,
                Description = entity.Description,
                LatestVersion = entity.LatestVersion,
                TreatyCode = entity.TreatyCode,
                CoverageStatus = entity.CoverageStatus,
                EffectiveAt = entity.EffectiveAt,
                OrionGroupStr = entity.OrionGroupStr,
                DocumentStatus = entity.DocumentStatus,
                DraftingStatus = entity.DraftingStatus,
                DraftingStatusCategory = entity.DraftingStatusCategory
            };
        }

        public static Expression<Func<IGrouping<int, TreatyPricingTreatyWorkflowVersion>, TreatyPricingTreatyWorkflowViewModel>> VersionExpression()
        {
            return entity => new TreatyPricingTreatyWorkflowViewModel
            {
                Id = entity.Key,
                TreatyPricingTreatyWorkflowId = entity.FirstOrDefault().TreatyPricingTreatyWorkflow.Id,
                ReinsuranceTypePickListDetailId = entity.FirstOrDefault().TreatyPricingTreatyWorkflow.ReinsuranceTypePickListDetailId,
                ReinsuranceTypePickListDetail = entity.FirstOrDefault().TreatyPricingTreatyWorkflow.ReinsuranceTypePickListDetail,
                CounterPartyDetailId = entity.FirstOrDefault().TreatyPricingTreatyWorkflow.CounterPartyDetailId,
                CounterPartyDetail = entity.FirstOrDefault().TreatyPricingTreatyWorkflow.CounterPartyDetail,
                InwardRetroPartyDetailId = entity.FirstOrDefault().TreatyPricingTreatyWorkflow.InwardRetroPartyDetailId,
                InwardRetroPartyDetail = entity.FirstOrDefault().TreatyPricingTreatyWorkflow.InwardRetroPartyDetail,
                DocumentType = entity.FirstOrDefault().TreatyPricingTreatyWorkflow.DocumentType,
                DocumentId = entity.FirstOrDefault().TreatyPricingTreatyWorkflow.DocumentId,
                BusinessOriginPickListDetailId = entity.FirstOrDefault().TreatyPricingTreatyWorkflow.BusinessOriginPickListDetailId,
                TypeOfBusiness = entity.FirstOrDefault().TreatyPricingTreatyWorkflow.TypeOfBusiness,
                CountryOrigin = entity.FirstOrDefault().TreatyPricingTreatyWorkflow.CountryOrigin,
                Description = entity.FirstOrDefault().TreatyPricingTreatyWorkflow.Description,
                LatestVersion = entity.FirstOrDefault().TreatyPricingTreatyWorkflow.LatestVersion,
                TreatyCode = entity.FirstOrDefault().TreatyPricingTreatyWorkflow.TreatyCode,
                CoverageStatus = entity.FirstOrDefault().TreatyPricingTreatyWorkflow.CoverageStatus,
                EffectiveAt = entity.FirstOrDefault().TreatyPricingTreatyWorkflow.EffectiveAt,
                OrionGroupStr = entity.FirstOrDefault().TreatyPricingTreatyWorkflow.OrionGroupStr,
                DocumentStatus = entity.FirstOrDefault().TreatyPricingTreatyWorkflow.DocumentStatus,
                DraftingStatus = entity.FirstOrDefault().TreatyPricingTreatyWorkflow.DraftingStatus,
                DraftingStatusCategory = entity.FirstOrDefault().TreatyPricingTreatyWorkflow.DraftingStatusCategory,
                PersonInChargeId = entity.OrderByDescending(q => q.Version).FirstOrDefault().PersonInChargeId,
                PersonInCharge = entity.OrderByDescending(q => q.Version).FirstOrDefault().PersonInCharge,
                LatestRevisionDate = entity.OrderByDescending(q => q.Version).FirstOrDefault().LatestRevisionDate,
                TreatyPricingTreatyWorkflow = entity.FirstOrDefault().TreatyPricingTreatyWorkflow,
                Reviewer = entity.FirstOrDefault().TreatyPricingTreatyWorkflow.Reviewer
            };
        }

        public TreatyPricingTreatyWorkflowVersionBo GetVersionBo(TreatyPricingTreatyWorkflowVersionBo bo)
        {
            //General
            bo.RequestDate = RequestDateStr is null || RequestDateStr == "" ? null : Util.GetParseDateTime(RequestDateStr);
            bo.RequestDateStr = RequestDateStr;
            bo.TargetSentDate = TargetSentDateStr is null || TargetSentDateStr == "" ? null : Util.GetParseDateTime(TargetSentDateStr);
            bo.TargetSentDateStr = TargetSentDateStr;
            bo.DateSentToReviewer1st = DateSentToReviewer1stStr is null || DateSentToReviewer1stStr == "" ? null : Util.GetParseDateTime(DateSentToReviewer1stStr);
            bo.DateSentToReviewer1stStr = DateSentToReviewer1stStr;
            bo.DateSentToClient1st = DateSentToClient1stStr is null || DateSentToClient1stStr == "" ? null : Util.GetParseDateTime(DateSentToClient1stStr);
            bo.DateSentToClient1stStr = DateSentToClient1stStr;
            bo.LatestRevisionDate = LatestRevisionDateStr is null || LatestRevisionDateStr == "" ? null : Util.GetParseDateTime(LatestRevisionDateStr);
            bo.LatestRevisionDateStr = LatestRevisionDateStr;
            bo.SignedDate = SignedDateStr is null || SignedDateStr == "" ? null : Util.GetParseDateTime(SignedDateStr);
            bo.SignedDateStr = SignedDateStr;
            bo.ReportedDate = ReportedDateStr is null || ReportedDateStr == "" ? null : Util.GetParseDateTime(ReportedDateStr);
            bo.ReportedDateStr = ReportedDateStr;
            bo.PersonInChargeId = PersonInChargeId;
            bo.PersonInChargeName = PersonInChargeName;
            bo.TreatyPricingTreatyWorkflowBo = TreatyPricingTreatyWorkflowService.Find(Id);

            return bo;
        }

    }
}