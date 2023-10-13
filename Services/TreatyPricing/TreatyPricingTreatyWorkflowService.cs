using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.TreatyPricing
{
    public class TreatyPricingTreatyWorkflowService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingTreatyWorkflow)),
                Controller = ModuleBo.ModuleController.TreatyPricingTreatyWorkflow.ToString()
            };
        }

        public static Expression<Func<TreatyPricingTreatyWorkflow, TreatyPricingTreatyWorkflowBo>> Expression()
        {
            return entity => new TreatyPricingTreatyWorkflowBo
            {
                Id = entity.Id,
                DocumentType = entity.DocumentType,
                ReinsuranceTypePickListDetailId = entity.ReinsuranceTypePickListDetailId,
                CounterPartyDetailId = entity.CounterPartyDetailId,
                InwardRetroPartyDetailId = entity.InwardRetroPartyDetailId,
                BusinessOriginPickListDetailId = entity.BusinessOriginPickListDetailId,
                TypeOfBusiness = entity.TypeOfBusiness,
                CountryOrigin = entity.CountryOrigin,
                DocumentId = entity.DocumentId,
                TreatyCode = entity.TreatyCode,
                CoverageStatus = entity.CoverageStatus,
                DocumentStatus = entity.DocumentStatus,
                DraftingStatus = entity.DraftingStatus,
                DraftingStatusCategory = entity.DraftingStatusCategory,
                EffectiveAt = entity.EffectiveAt,
                OrionGroupStr = entity.OrionGroupStr,
                Description = entity.Description,
                SharepointLink = entity.SharepointLink,
                LatestVersion = entity.LatestVersion,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingTreatyWorkflowBo FormBo(TreatyPricingTreatyWorkflow entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            return new TreatyPricingTreatyWorkflowBo
            {
                Id = entity.Id,
                DocumentType = entity.DocumentType,
                DocumentTypeName = TreatyPricingTreatyWorkflowBo.GetDocumentTypeName(entity.DocumentType),
                ReinsuranceTypePickListDetailId = entity.ReinsuranceTypePickListDetailId,
                ReinsuranceTypePickListDetailBo = PickListDetailService.Find(entity.ReinsuranceTypePickListDetailId),
                CounterPartyDetailId = entity.CounterPartyDetailId,
                CounterPartyDetailBo = CedantService.Find(entity.CounterPartyDetailId),
                InwardRetroPartyDetailId = entity.InwardRetroPartyDetailId,
                InwardRetroPartyDetailBo = RetroPartyService.Find(entity.InwardRetroPartyDetailId),
                BusinessOriginPickListDetailId = entity.BusinessOriginPickListDetailId,
                BusinessOriginPickListDetailBo = PickListDetailService.Find(entity.BusinessOriginPickListDetailId),
                TypeOfBusiness = entity.TypeOfBusiness,
                CountryOrigin = entity.CountryOrigin,
                DocumentId = entity.DocumentId,
                TreatyCode = entity.TreatyCode,
                CoverageStatus = entity.CoverageStatus,
                CoverageStatusName = TreatyPricingTreatyWorkflowBo.GetCoverageStatusName(entity.CoverageStatus),
                DocumentStatus = entity.DocumentStatus,
                DocumentStatusName = TreatyPricingTreatyWorkflowBo.GetDocumentStatusName(entity.DocumentStatus),
                DraftingStatus = entity.DraftingStatus,
                DraftingStatusName = TreatyPricingTreatyWorkflowBo.GetDraftingStatusName(entity.DraftingStatus),
                DraftingStatusCategory = entity.DraftingStatusCategory,
                DraftingStatusCategoryName = TreatyPricingTreatyWorkflowBo.GetDraftingStatusCategoryName(entity.DraftingStatusCategory),
                EffectiveAt = entity.EffectiveAt,
                EffectiveAtStr = entity.EffectiveAt.HasValue ? entity.EffectiveAt.Value.ToString(Util.GetDateFormat()) : "",
                OrionGroupStr = entity.OrionGroupStr,
                Description = entity.Description,
                SharepointLink = entity.SharepointLink,
                Reviewer = entity.Reviewer,
                LatestVersion = entity.LatestVersion,
                TreatyPricingTreatyWorkflowVersionBos = TreatyPricingTreatyWorkflowVersionService.GetByTreatyPricingTreatyWorkflowId(entity.Id),
                TreatyPricingWorkflowObjectBos = TreatyPricingWorkflowObjectService.GetByTypeWorkflowId(TreatyPricingWorkflowObjectBo.TypeTreaty, entity.Id),

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingTreatyWorkflowBo FormBoForTreatyDashboard(TreatyPricingTreatyWorkflow entity = null)
        {
            if (entity == null)
                return null;

            return new TreatyPricingTreatyWorkflowBo
            {
                Id = entity.Id,

                DocumentType = entity.DocumentType,
                DocumentTypeName = TreatyPricingTreatyWorkflowBo.GetDocumentTypeName(entity.DocumentType),
                ReinsuranceTypePickListDetailId = entity.ReinsuranceTypePickListDetailId,
                CounterPartyDetailId = entity.CounterPartyDetailId,
                InwardRetroPartyDetailId = entity.InwardRetroPartyDetailId,
                BusinessOriginPickListDetailId = entity.BusinessOriginPickListDetailId,
                TypeOfBusiness = entity.TypeOfBusiness,
                CountryOrigin = entity.CountryOrigin,
                DocumentId = entity.DocumentId,
                TreatyCode = entity.TreatyCode,
                CoverageStatus = entity.CoverageStatus,
                CoverageStatusName = TreatyPricingTreatyWorkflowBo.GetCoverageStatusName(entity.CoverageStatus),
                DocumentStatus = entity.DocumentStatus,
                DocumentStatusName = TreatyPricingTreatyWorkflowBo.GetDocumentStatusName(entity.DocumentStatus),
                DraftingStatus = entity.DraftingStatus,
                DraftingStatusName = TreatyPricingTreatyWorkflowBo.GetDraftingStatusName(entity.DraftingStatus),
                DraftingStatusCategory = entity.DraftingStatusCategory,
                DraftingStatusCategoryName = TreatyPricingTreatyWorkflowBo.GetDraftingStatusCategoryName(entity.DraftingStatusCategory),
                EffectiveAt = entity.EffectiveAt,
                EffectiveAtStr = entity.EffectiveAt.HasValue ? entity.EffectiveAt.Value.ToString(Util.GetDateFormat()) : "",
                OrionGroupStr = entity.OrionGroupStr,
                Reviewer = entity.Reviewer,
            };
        }

        public static IList<TreatyPricingTreatyWorkflowBo> FormBos(IList<TreatyPricingTreatyWorkflow> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingTreatyWorkflowBo> bos = new List<TreatyPricingTreatyWorkflowBo>() { };
            foreach (TreatyPricingTreatyWorkflow entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingTreatyWorkflow FormEntity(TreatyPricingTreatyWorkflowBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingTreatyWorkflow
            {
                Id = bo.Id,
                DocumentType = bo.DocumentType,
                ReinsuranceTypePickListDetailId = bo.ReinsuranceTypePickListDetailId,
                CounterPartyDetailId = bo.CounterPartyDetailId,
                InwardRetroPartyDetailId = bo.InwardRetroPartyDetailId,
                BusinessOriginPickListDetailId = bo.BusinessOriginPickListDetailId,
                TypeOfBusiness = bo.TypeOfBusiness,
                CountryOrigin = bo.CountryOrigin,
                DocumentId = bo.DocumentId,
                TreatyCode = bo.TreatyCode,
                CoverageStatus = bo.CoverageStatus,
                DocumentStatus = bo.DocumentStatus,
                DraftingStatus = bo.DraftingStatus,
                DraftingStatusCategory = bo.DraftingStatusCategory,
                EffectiveAt = bo.EffectiveAt,
                OrionGroupStr = bo.OrionGroupStr,
                Description = bo.Description,
                SharepointLink = bo.SharepointLink,
                Reviewer = bo.Reviewer,
                LatestVersion = bo.LatestVersion,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static int GetLatestVersion(int workflowId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflowVersions.Where(q => q.TreatyPricingTreatyWorkflowId == workflowId).Select(q => q.Version).Max();
            }
        }


        public static bool IsExists(int id)
        {
            return TreatyPricingTreatyWorkflow.IsExists(id);
        }

        public static TreatyPricingTreatyWorkflowBo Find(int? id)
        {
            return FormBo(TreatyPricingTreatyWorkflow.Find(id));
        }

        public static TreatyPricingTreatyWorkflowBo FindForTreatyDashboard(int id)
        {
            return FormBoForTreatyDashboard(TreatyPricingTreatyWorkflow.Find(id));
        }

        public static TreatyPricingTreatyWorkflowBo FindByInwardRetroParty(int? id)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingTreatyWorkflows.Where(q => q.InwardRetroPartyDetailId == id).FirstOrDefault());
            }
        }

        public static IList<TreatyPricingTreatyWorkflowBo> GetAll()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingTreatyWorkflows.ToList());
            }
        }

        public static IList<TreatyPricingWorkflowObjectBo> GetWorkflowObjects(TreatyPricingWorkflowObjectBo bo)
        {
            using (var db = new AppDbContext())
            {
                List<int> workflowIds = db.TreatyPricingWorkflowObjects
                    .Where(q => q.Type == TreatyPricingWorkflowObjectBo.TypeTreaty)
                    .Where(q => q.ObjectType == bo.ObjectType)
                    .Where(q => q.ObjectId == bo.ObjectId)
                    .Where(q => q.ObjectVersionId == bo.ObjectVersionId)
                    .Select(q => q.WorkflowId)
                    .ToList();
                var query = db.TreatyPricingTreatyWorkflows.Where(q => !workflowIds.Contains(q.Id));
                if (!string.IsNullOrEmpty(bo.WorkflowCode))
                {
                    query = query.Where(q => q.DocumentId.Contains(bo.WorkflowCode));
                }

                int type = TreatyPricingWorkflowObjectBo.TypeTreaty;
                string typeName = TreatyPricingWorkflowObjectBo.GetTypeName(type);

                return query.Select(
                    q => new TreatyPricingWorkflowObjectBo()
                    {
                        Type = type,
                        TypeName = typeName,
                        ObjectType = bo.ObjectType,
                        ObjectId = bo.ObjectId,
                        ObjectVersionId = bo.ObjectVersionId,
                        WorkflowId = q.Id,
                        WorkflowCode = q.DocumentId
                    }
                ).ToList();
            }
        }

        public static Result Save(ref TreatyPricingTreatyWorkflowBo bo)
        {
            if (!TreatyPricingTreatyWorkflow.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingTreatyWorkflowBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingTreatyWorkflow.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingTreatyWorkflowBo bo)
        {
            TreatyPricingTreatyWorkflow entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingTreatyWorkflowBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingTreatyWorkflowBo bo)
        {
            Result result = Result();

            TreatyPricingTreatyWorkflow entity = TreatyPricingTreatyWorkflow.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.DocumentType = bo.DocumentType;
                entity.ReinsuranceTypePickListDetailId = bo.ReinsuranceTypePickListDetailId;
                entity.CounterPartyDetailId = bo.CounterPartyDetailId;
                entity.InwardRetroPartyDetailId = bo.InwardRetroPartyDetailId;
                entity.BusinessOriginPickListDetailId = bo.BusinessOriginPickListDetailId;
                entity.TypeOfBusiness = bo.TypeOfBusiness;
                entity.CountryOrigin = bo.CountryOrigin;
                entity.DocumentId = bo.DocumentId;
                entity.TreatyCode = bo.TreatyCode;
                entity.CoverageStatus = bo.CoverageStatus;
                entity.DocumentStatus = bo.DocumentStatus;
                entity.DraftingStatus = bo.DraftingStatus;
                entity.DraftingStatusCategory = bo.DraftingStatusCategory;
                entity.EffectiveAt = bo.EffectiveAt;
                entity.OrionGroupStr = bo.OrionGroupStr;
                entity.Description = bo.Description;
                entity.SharepointLink = bo.SharepointLink;
                entity.Reviewer = bo.Reviewer;
                entity.LatestVersion = bo.LatestVersion;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingTreatyWorkflowBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingTreatyWorkflowBo bo)
        {
            TreatyPricingTreatyWorkflow.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingTreatyWorkflowBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingTreatyWorkflow.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }


        public static int GetDraftingStatusCategory(int? draftingStatus)
        {
            if (draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusDrafting ||
                draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusTreatyPICOk ||
                draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusPricingPICOk ||
                draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusHODOk ||
                draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusCAndROk ||
                draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusRGAOk ||
                draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusUWOk ||
                draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusClaimOk ||
                draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusHealthOk)
            {
                return TreatyPricingTreatyWorkflowBo.DraftingStatusCategoryDrafting;
            }
            else if (draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusOnHold)
            {
                return TreatyPricingTreatyWorkflowBo.DraftingStatusCategoryOnHold;
            }
            else if (draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusPendingTreatyPeerReview1st ||
                draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusPendingProductPricingPICReview1st ||
                draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusPendingGroupPricingPICReview1st ||
                draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusPendingHODReview ||
                draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusPendingCAndRReview ||
                draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusPendingRGAReview ||
                draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusPendingUWReview ||
                draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusPendingClaimReview ||
                draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusPendingHealthReview ||
                draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusPendingBDReview ||
                draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusPendingTreatyPeerReviewRevised ||
                draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusPendingProductPricingPICReviewRevised ||
                draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusPendingGroupPricingPICReviewRevised)
            {
                return TreatyPricingTreatyWorkflowBo.DraftingStatusCategoryInternalReview;
            }
            else if (draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusEXTFirstDraftSent ||
                draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusEXTRevisedDraftSent)
            {
                return TreatyPricingTreatyWorkflowBo.DraftingStatusCategorySent;
            }
            else if (draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusToReviewClientFeedback)
            {
                return TreatyPricingTreatyWorkflowBo.DraftingStatusCategoryRevisedDrafting;
            }
            else if (draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusINTCountersigning ||
                draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusEXTCountersigning)
            {
                return TreatyPricingTreatyWorkflowBo.DraftingStatusCategoryRevisedDrafting;
            }
            else if (draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusSigned)
            {
                return TreatyPricingTreatyWorkflowBo.DraftingStatusCategorySigned;
            }
            else if (draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusUnassigned)
            {
                return TreatyPricingTreatyWorkflowBo.DraftingStatusCategoryUnassigned;
            }
            else if (draftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusCancelled)
            {
                return TreatyPricingTreatyWorkflowBo.DraftingStatusCategoryCancelled;
            }
            else
            {
                return 0;
            }
        }

        // less than 6 months = "<= 6 months"
        // less than 12 months = "<= 12 months"
        // more than 12 months = "> 12 months"


        #region Treaty Weekly/Monthly/Quarterly Report
        public static int GetReportingStatus(DateTime? reportedDate, DateTime? dateSentToClient1st, DateTime effStartDate, DateTime effEndDate)
        {
            if (reportedDate.HasValue)
            {
                if (reportedDate > effStartDate && reportedDate < effEndDate)
                {
                    return TreatyPricingTreatyWorkflowBo.ReportingStatusSigned;
                }
            }

            if (dateSentToClient1st.HasValue)
            {
                if (dateSentToClient1st > effStartDate && dateSentToClient1st < effEndDate)
                {
                    return TreatyPricingTreatyWorkflowBo.ReportingStatusDone;
                }
            }

            return TreatyPricingTreatyWorkflowBo.ReportingStatusDoing;

        }
        #endregion

        #region Draft Status Overview by Business Origin
        public static int GetSignedCountByInwardRetroPartyDetaiIdInTreatyWM(int? id, DateTime effStartDate, DateTime effEndDate)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows
                .Where(a => a.InwardRetroPartyDetailId == id)
                .Where(a => a.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty)
                .Where(a => a.DocumentStatus == TreatyPricingTreatyWorkflowBo.DocumentStatusSigned)
                .Where(a => a.EffectiveAt > effStartDate && a.EffectiveAt < effEndDate)
                .Where(a => a.BusinessOriginPickListDetail.Code == "WM")
                .Where(a => a.ReinsuranceTypePickListDetail.Code == "RL")
                .Where(a => a.CoverageStatus != TreatyPricingTreatyWorkflowBo.CoverageStatusTerminated)
                .Count();
            }
        }

        public static int GetLessThan6MonthsCountByInwardRetroPartyDetailIdInTreatyWM(int? id, DateTime effStartDate, DateTime effEndDate)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows.Where(a => a.InwardRetroPartyDetailId == id)
                .Where(a => a.OrionGroupStr == "<= 6 months")
                .Where(a => a.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty)
                .Where(a => a.DocumentStatus == TreatyPricingTreatyWorkflowBo.DocumentStatusDrafting)
                .Where(a => a.EffectiveAt > effStartDate && a.EffectiveAt < effEndDate)
                .Where(a => a.BusinessOriginPickListDetail.Code == "WM")
                .Where(a => a.ReinsuranceTypePickListDetail.Code == "RL")
                .Count();
            }
        }

        public static int GetLessThan12MonthsCountByInwardRetroPartyDetailIdInTreatyWM(int? id, DateTime effStartDate, DateTime effEndDate)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows
                .Where(a => a.InwardRetroPartyDetailId == id)
                .Where(a => a.OrionGroupStr == "<= 12 months")
                .Where(a => a.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty)
                .Where(a => a.DocumentStatus == TreatyPricingTreatyWorkflowBo.DocumentStatusDrafting)
                .Where(a => a.EffectiveAt > effStartDate && a.EffectiveAt < effEndDate)
                .Where(a => a.BusinessOriginPickListDetail.Code == "WM")
                .Where(a => a.ReinsuranceTypePickListDetail.Code == "RL")
                .Count();
            }
        }

        public static int GetMoreThan12MonthsCountByInwardRetroPartyDetailIdInTreatyWM(int? id, DateTime effStartDate, DateTime effEndDate)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows
                .Where(a => a.InwardRetroPartyDetailId == id)
                .Where(a => a.OrionGroupStr == "> 12 months")
                .Where(a => a.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty)
                .Where(a => a.DocumentStatus == TreatyPricingTreatyWorkflowBo.DocumentStatusDrafting)
                .Where(a => a.EffectiveAt > effStartDate && a.EffectiveAt < effEndDate)
                .Where(a => a.BusinessOriginPickListDetail.Code == "WM")
                .Where(a => a.ReinsuranceTypePickListDetail.Code == "RL")
                .Count();
            }
        }

        public static int GetSignedCountByInwardRetroPartyDetaiIdInAddendumWM(int? id, DateTime effStartDate, DateTime effEndDate)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows
                .Where(a => a.InwardRetroPartyDetailId == id)
                .Where(a => a.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum)
                .Where(a => a.DocumentStatus == TreatyPricingTreatyWorkflowBo.DocumentStatusSigned)
                .Where(a => a.EffectiveAt > effStartDate && a.EffectiveAt < effEndDate)
                .Where(a => a.BusinessOriginPickListDetail.Code == "WM")
                .Where(a => a.ReinsuranceTypePickListDetail.Code == "RL")
                .Where(a => a.CoverageStatus != TreatyPricingTreatyWorkflowBo.CoverageStatusTerminated)
                .Count();
            }
        }

        public static int GetLessThan6MonthsCountByInwardRetroPartyDetailIdInAddendumWM(int? id, DateTime effStartDate, DateTime effEndDate)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows.Where(a => a.InwardRetroPartyDetailId == id)
                .Where(a => a.OrionGroupStr == "<= 6 months")
                .Where(a => a.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum)
                .Where(a => a.DocumentStatus == TreatyPricingTreatyWorkflowBo.DocumentStatusDrafting)
                .Where(a => a.EffectiveAt > effStartDate && a.EffectiveAt < effEndDate)
                .Where(a => a.BusinessOriginPickListDetail.Code == "WM")
                .Where(a => a.ReinsuranceTypePickListDetail.Code == "RL")
                .Count();
            }
        }

        public static int GetLessThan12MonthsCountByInwardRetroPartyDetailIdInAddendumWM(int? id, DateTime effStartDate, DateTime effEndDate)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows
                .Where(a => a.InwardRetroPartyDetailId == id)
                .Where(a => a.OrionGroupStr == "<= 12 months")
                .Where(a => a.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum)
                .Where(a => a.DocumentStatus == TreatyPricingTreatyWorkflowBo.DocumentStatusDrafting)
                .Where(a => a.EffectiveAt > effStartDate && a.EffectiveAt < effEndDate)
                .Where(a => a.BusinessOriginPickListDetail.Code == "WM")
                .Where(a => a.ReinsuranceTypePickListDetail.Code == "RL")
                .Count();
            }
        }

        public static int GetMoreThan12MonthsCountByInwardRetroPartyDetailIdInAddendumWM(int? id, DateTime effStartDate, DateTime effEndDate)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows
                .Where(a => a.InwardRetroPartyDetailId == id)
                .Where(a => a.OrionGroupStr == "> 12 months")
                .Where(a => a.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum)
                .Where(a => a.DocumentStatus == TreatyPricingTreatyWorkflowBo.DocumentStatusDrafting)
                .Where(a => a.EffectiveAt > effStartDate && a.EffectiveAt < effEndDate)
                .Where(a => a.BusinessOriginPickListDetail.Code == "WM")
                .Where(a => a.ReinsuranceTypePickListDetail.Code == "RL")
                .Count();
            }
        }

        public static int GetSignedCountByInwardRetroPartyDetaiIdInTreatyOM(int? id, DateTime effStartDate, DateTime effEndDate)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows
                .Where(a => a.InwardRetroPartyDetailId == id)
                .Where(a => a.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty)
                .Where(a => a.DocumentStatus == TreatyPricingTreatyWorkflowBo.DocumentStatusSigned)
                .Where(a => a.EffectiveAt > effStartDate && a.EffectiveAt < effEndDate)
                .Where(a => a.BusinessOriginPickListDetail.Code == "OM")
                .Where(a => a.CoverageStatus != TreatyPricingTreatyWorkflowBo.CoverageStatusTerminated)
                .Count();
            }
        }

        public static int GetLessThan6MonthsCountByInwardRetroPartyDetailIdInTreatyOM(int? id, DateTime effStartDate, DateTime effEndDate)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows.Where(a => a.InwardRetroPartyDetailId == id)
                .Where(a => a.OrionGroupStr == "<= 6 months")
                .Where(a => a.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty)
                .Where(a => a.DocumentStatus == TreatyPricingTreatyWorkflowBo.DocumentStatusDrafting)
                .Where(a => a.EffectiveAt > effStartDate && a.EffectiveAt < effEndDate)
                .Where(a => a.BusinessOriginPickListDetail.Code == "OM")
                .Count();
            }
        }

        public static int GetLessThan12MonthsCountByInwardRetroPartyDetailIdInTreatyOM(int? id, DateTime effStartDate, DateTime effEndDate)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows
                .Where(a => a.InwardRetroPartyDetailId == id)
                .Where(a => a.OrionGroupStr == "<= 12 months")
                .Where(a => a.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty)
                .Where(a => a.DocumentStatus == TreatyPricingTreatyWorkflowBo.DocumentStatusDrafting)
                .Where(a => a.EffectiveAt > effStartDate && a.EffectiveAt < effEndDate)
                .Where(a => a.BusinessOriginPickListDetail.Code == "OM")
                .Count();
            }
        }

        public static int GetMoreThan12MonthsCountByInwardRetroPartyDetailIdInTreatyOM(int? id, DateTime effStartDate, DateTime effEndDate)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows
                .Where(a => a.InwardRetroPartyDetailId == id)
                .Where(a => a.OrionGroupStr == "> 12 months")
                .Where(a => a.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty)
                .Where(a => a.DocumentStatus == TreatyPricingTreatyWorkflowBo.DocumentStatusDrafting)
                .Where(a => a.EffectiveAt > effStartDate && a.EffectiveAt < effEndDate)
                .Where(a => a.BusinessOriginPickListDetail.Code == "OM")
                .Count();
            }
        }

        public static int GetSignedCountByInwardRetroPartyDetaiIdInAddendumOM(int? id, DateTime effStartDate, DateTime effEndDate)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows
                .Where(a => a.InwardRetroPartyDetailId == id)
                .Where(a => a.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum)
                .Where(a => a.DocumentStatus == TreatyPricingTreatyWorkflowBo.DocumentStatusSigned)
                .Where(a => a.EffectiveAt > effStartDate && a.EffectiveAt < effEndDate)
                .Where(a => a.BusinessOriginPickListDetail.Code == "OM")
                .Where(a => a.CoverageStatus != TreatyPricingTreatyWorkflowBo.CoverageStatusTerminated)
                .Count();
            }
        }

        public static int GetLessThan6MonthsCountByInwardRetroPartyDetailIdInAddendumOM(int? id, DateTime effStartDate, DateTime effEndDate)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows.Where(a => a.InwardRetroPartyDetailId == id)
                .Where(a => a.OrionGroupStr == "<= 6 months")
                .Where(a => a.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum)
                .Where(a => a.DocumentStatus == TreatyPricingTreatyWorkflowBo.DocumentStatusDrafting)
                .Where(a => a.EffectiveAt > effStartDate && a.EffectiveAt < effEndDate)
                .Where(a => a.BusinessOriginPickListDetail.Code == "OM")
                .Count();
            }
        }

        public static int GetLessThan12MonthsCountByInwardRetroPartyDetailIdInAddendumOM(int? id, DateTime effStartDate, DateTime effEndDate)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows
                .Where(a => a.InwardRetroPartyDetailId == id)
                .Where(a => a.OrionGroupStr == "<= 12 months")
                .Where(a => a.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum)
                .Where(a => a.DocumentStatus == TreatyPricingTreatyWorkflowBo.DocumentStatusDrafting)
                .Where(a => a.EffectiveAt > effStartDate && a.EffectiveAt < effEndDate)
                .Where(a => a.BusinessOriginPickListDetail.Code == "OM")
                .Count();
            }
        }

        public static int GetMoreThan12MonthsCountByInwardRetroPartyDetailIdInAddendumOM(int? id, DateTime effStartDate, DateTime effEndDate)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows
                .Where(a => a.InwardRetroPartyDetailId == id)
                .Where(a => a.OrionGroupStr == "> 12 months")
                .Where(a => a.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum)
                .Where(a => a.DocumentStatus == TreatyPricingTreatyWorkflowBo.DocumentStatusDrafting)
                .Where(a => a.EffectiveAt > effStartDate && a.EffectiveAt < effEndDate)
                .Where(a => a.BusinessOriginPickListDetail.Code == "OM")
                .Count();
            }
        }


        #endregion

        #region Draft Status Overview by Retro Party
        public static int GetSignedCountByInwardRetroPartyDetaiIdInTreaty(int? id, DateTime effStartDate, DateTime effEndDate)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows
                .Where(a => a.InwardRetroPartyDetailId == id)
                .Where(a => a.DocumentStatus == TreatyPricingTreatyWorkflowBo.DocumentStatusSigned)
                .Where(a => a.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty)
                .Where(a => a.EffectiveAt > effStartDate && a.EffectiveAt < effEndDate)
                .Where(a => a.ReinsuranceTypePickListDetail.Description.ToLower().Contains(("Retro").ToLower()))
                .Where(a => a.CoverageStatus != TreatyPricingTreatyWorkflowBo.CoverageStatusTerminated)
                .Count();
            }
        }

        public static int GetLessThan6MonthsCountByInwardRetroPartyDetailIdInTreaty(int? id, DateTime effStartDate, DateTime effEndDate)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows
                .Where(a => a.InwardRetroPartyDetailId == id)
                .Where(a => a.OrionGroupStr == "<= 6 months")
                .Where(a => a.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty)
                .Where(a => a.EffectiveAt > effStartDate && a.EffectiveAt < effEndDate)
                .Where(a => a.DocumentStatus == TreatyPricingTreatyWorkflowBo.DocumentStatusDrafting)
                .Where(a => a.ReinsuranceTypePickListDetail.Description.ToLower().Contains(("Retro").ToLower()))
                .Count();
            }
        }

        public static int GetLessThan12MonthsCountByInwardRetroPartyDetailIdInTreaty(int? id, DateTime effStartDate, DateTime effEndDate)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows
                .Where(a => a.InwardRetroPartyDetailId == id)
                .Where(a => a.OrionGroupStr == "<= 12 months")
                .Where(a => a.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty)
                .Where(a => a.EffectiveAt > effStartDate && a.EffectiveAt < effEndDate)
                .Where(a => a.DocumentStatus == TreatyPricingTreatyWorkflowBo.DocumentStatusDrafting)
                .Where(a => a.ReinsuranceTypePickListDetail.Description.ToLower().Contains(("Retro").ToLower()))
                .Count();
            }
        }

        public static int GetMoreThan12MonthsCountByInwardRetroPartyDetailIdInTreaty(int? id, DateTime effStartDate, DateTime effEndDate)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows
                .Where(a => a.InwardRetroPartyDetailId == id)
                .Where(a => a.OrionGroupStr == "> 12 months")
                .Where(a => a.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty)
                .Where(a => a.EffectiveAt > effStartDate && a.EffectiveAt < effEndDate)
                .Where(a => a.DocumentStatus == TreatyPricingTreatyWorkflowBo.DocumentStatusDrafting)
                .Where(a => a.ReinsuranceTypePickListDetail.Description.ToLower().Contains(("Retro").ToLower()))
                .Count();
            }
        }

        public static int GetSignedCountByInwardRetroPartyDetaiIdInAddendum(int? id, DateTime effStartDate, DateTime effEndDate)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows
                .Where(a => a.InwardRetroPartyDetailId == id)
                .Where(a => a.DocumentStatus == TreatyPricingTreatyWorkflowBo.DocumentStatusSigned)
                .Where(a => a.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum)
                .Where(a => a.EffectiveAt > effStartDate && a.EffectiveAt < effEndDate)
                .Where(a => a.CoverageStatus != TreatyPricingTreatyWorkflowBo.CoverageStatusTerminated)
                .Count();
            }
        }

        public static int GetLessThan6MonthsCountByInwardRetroPartyDetailIdInAddendum(int? id, DateTime effStartDate, DateTime effEndDate)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows
                .Where(a => a.InwardRetroPartyDetailId == id)
                .Where(a => a.OrionGroupStr == "<= 6 months")
                .Where(a => a.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum)
                .Where(a => a.EffectiveAt > effStartDate && a.EffectiveAt < effEndDate)
                .Where(a => a.DocumentStatus == TreatyPricingTreatyWorkflowBo.DocumentStatusDrafting)
                .Count();
            }
        }

        public static int GetLessThan12MonthsCountByInwardRetroPartyDetailIdInAddendum(int? id, DateTime effStartDate, DateTime effEndDate)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows
                .Where(a => a.InwardRetroPartyDetailId == id)
                .Where(a => a.OrionGroupStr == "<= 12 months")
                .Where(a => a.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum)
                .Where(a => a.EffectiveAt > effStartDate && a.EffectiveAt < effEndDate)
                .Where(a => a.DocumentStatus == TreatyPricingTreatyWorkflowBo.DocumentStatusDrafting)
                .Count();
            }
        }

        public static int GetMoreThan12MonthsCountByInwardRetroPartyDetailIdInAddendum(int? id, DateTime effStartDate, DateTime effEndDate)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows
                .Where(a => a.InwardRetroPartyDetailId == id)
                .Where(a => a.OrionGroupStr == "> 12 months")
                .Where(a => a.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum)
                .Where(a => a.EffectiveAt > effStartDate && a.EffectiveAt < effEndDate)
                .Where(a => a.DocumentStatus == TreatyPricingTreatyWorkflowBo.DocumentStatusDrafting)
                .Count();
            }
        }
        #endregion

        #region Dashboard Draft Status Overview
        public static int GetCountByDraftingStatusCategory(int draftingStatusCategoryConst)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows
                .Where(a => a.DraftingStatusCategory == draftingStatusCategoryConst)
                .Count();
            }
        }

        public static int GetLessThan6MonthCountByDraftingStatusCategory(int draftingStatusCategoryConst)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows
                .Where(a => a.DraftingStatusCategory == draftingStatusCategoryConst)
                .Where(a => a.OrionGroupStr == "<= 6 months")
                .Count();
            }
        }

        public static int GetLessThan12MonthCountByDraftingStatusCategory(int draftingStatusCategoryConst)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows
                .Where(a => a.DraftingStatusCategory == draftingStatusCategoryConst)
                .Where(a => a.OrionGroupStr == "<= 12 months")
                .Count();
            }
        }

        public static int GetMoreThan12MonthCountByDraftingStatusCategory(int draftingStatusCategoryConst)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflows
                .Where(a => a.DraftingStatusCategory == draftingStatusCategoryConst)
                .Where(a => a.OrionGroupStr == "> 12 months")
                .Count();
            }
        }

        #endregion

        #region Draft Overview PIC
        public static int GetCountDraftOverviewByPic(int? picId, string orionGroup, int? documentType, int? draftingStatusCategory)
        {
            using (var db = new AppDbContext())
            {
                var count = 0;

                int[] treatyAndAddendum = { TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty, TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum };

                if (picId.HasValue)
                {
                    var verBos = TreatyPricingTreatyWorkflowVersionService.GetLatestVersionOfTreatyWorkflow()
                        .Where(a => a.PersonInChargeId == picId.Value);

                    foreach (var verBo in verBos)
                    {
                        if (documentType.HasValue)
                        {
                            count = count + db.TreatyPricingTreatyWorkflows.Where(q => q.Id == verBo.TreatyPricingTreatyWorkflowId)
                                .Where(q => q.OrionGroupStr == orionGroup)
                                .Where(q => q.DocumentType == documentType.Value).Count();
                        }
                        else if (draftingStatusCategory.HasValue)
                        {
                            count = count + db.TreatyPricingTreatyWorkflows.Where(q => q.Id == verBo.TreatyPricingTreatyWorkflowId)
                                .Where(q => q.OrionGroupStr.Contains(orionGroup))
                                .Where(q => q.DraftingStatusCategory == draftingStatusCategory.Value)
                                .Count();
                        }
                        else
                        {
                            count = count + db.TreatyPricingTreatyWorkflows.Where(q => q.Id == verBo.TreatyPricingTreatyWorkflowId)
                                .Where(q => q.OrionGroupStr.Contains(orionGroup))
                                .Where(q => !treatyAndAddendum.Contains(q.DocumentType))
                                .Count();
                        }
                    }
                }
                else
                {
                    if (documentType.HasValue)
                    {
                        count = db.TreatyPricingTreatyWorkflows.Where(q => q.OrionGroupStr == orionGroup).Where(q => q.DocumentType == documentType.Value).Count();
                    }
                    else if (draftingStatusCategory.HasValue)
                    {
                        count = db.TreatyPricingTreatyWorkflows
                            .Where(q => q.OrionGroupStr.Contains(orionGroup))
                            .Where(q => q.DraftingStatusCategory == draftingStatusCategory.Value)
                            .Count();
                    }
                    else
                    {
                        count = db.TreatyPricingTreatyWorkflows
                            .Where(q => q.OrionGroupStr.Contains(orionGroup))
                            .Where(q => !treatyAndAddendum.Contains(q.DocumentType))
                            .Count();
                    }

                }

                return count;
            }
        }
        #endregion

        #region Draft Overview Pending Reviewer
        public static int GetCountDraftOverviewPendingReviewer(int? reviewerId, string orionGroup, int? documentType)
        {
            using (var db = new AppDbContext())
            {
                var reviewerIdStr = reviewerId.Value.ToString();
                var list = db.TreatyPricingTreatyWorkflows
                            .Where(q => q.DraftingStatusCategory == TreatyPricingTreatyWorkflowBo.DraftingStatusCategoryInternalReview)
                            .Where(q => q.OrionGroupStr.Contains(orionGroup))
                            .ToList();

                list = list.Where(q => !string.IsNullOrEmpty(q.Reviewer) && q.Reviewer.Split(',').Select(r => r.Trim()).ToArray().Contains(reviewerIdStr)).ToList();

                var count = 0;

                int[] treatyAndAddendum = { TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty, TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum };

                foreach (var reviewerBo in list)
                {
                    if (documentType.HasValue)
                    {
                        count = list
                            .Where(q => q.DocumentType == documentType.Value)
                            .Count();
                    }
                    else
                    {
                        count = list
                            .Where(q => !treatyAndAddendum.Contains(q.DocumentType))
                            .Count();
                    }
                }

                return count;
            }
        }

        #endregion

        #region Draft Overview Pending HOD
        public static int GetCountDraftOverviewPendingHod(int? reviewerId, string orionGroup, int? documentType)
        {
            using (var db = new AppDbContext())
            {
                var reviewerIdStr = reviewerId.Value.ToString();
                var list = db.TreatyPricingTreatyWorkflows
                            .Where(q => q.DraftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusPendingHODReview)
                            .Where(q => q.OrionGroupStr.Contains(orionGroup))
                            .ToList();

                list = list.Where(q => !string.IsNullOrEmpty(q.Reviewer) && q.Reviewer.Split(',').Select(r => r.Trim()).ToArray().Contains(reviewerIdStr)).ToList();

                var count = 0;

                int[] treatyAndAddendum = { TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty, TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum };

                foreach (var reviewerBo in list)
                {
                    if (documentType.HasValue)
                    {
                        count = list
                            .Where(q => q.DocumentType == documentType.Value)
                            .Count();
                    }
                    else
                    {
                        count = list
                            .Where(q => !treatyAndAddendum.Contains(q.DocumentType))
                            .Count();
                    }
                }

                return count;
            }
        }
        #endregion

        #region Draft Overview Pending Department
        public static int GetCountDraftOverviewPendingDepartment(int? department, string orionGroup, int? documentType)
        {
            using (var db = new AppDbContext())
            {
                var list = db.TreatyPricingTreatyWorkflows
                    .Where(q => q.DraftingStatus == department)
                    .Where(q => q.OrionGroupStr.Contains(orionGroup))
                    .ToList();

                var count = 0;

                int[] treatyAndAddendum = { TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty, TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum };

                foreach (var reviewerBo in list)
                {
                    if (documentType.HasValue)
                    {
                        count = list
                        .Where(q => q.DocumentType == documentType.Value)
                            .Count();
                    }
                    else
                    {
                        count = list
                            .Where(q => !treatyAndAddendum.Contains(q.DocumentType))
                            .Count();
                    }
                }

                return count;
            }
        }
        #endregion

        #region Generate Orion Group 
        public static int GenerateOrionGroup(string effectiveAt)
        {
            if (!string.IsNullOrEmpty(effectiveAt))
            {
                var effectiveAtt = Util.GetParseDateTime(effectiveAt);
                DateTime orionDate;
                DateTime quarter1 = DateTime.Parse("31 Mar " + DateTime.Today.Year.ToString());
                DateTime quarter2 = DateTime.Parse("30 Jun " + DateTime.Today.Year.ToString());
                DateTime quarter3 = DateTime.Parse("30 Sep " + DateTime.Today.Year.ToString());
                DateTime quarter4 = DateTime.Parse("31 Dec " + DateTime.Today.Year.ToString());

                if (DateTime.Now <= quarter1)
                {
                    orionDate = quarter1;
                }
                else if (DateTime.Now <= quarter2)
                {
                    orionDate = quarter2;
                }
                else if (DateTime.Now <= quarter3)
                {
                    orionDate = quarter3;
                }
                else
                {
                    orionDate = quarter4;
                }

                var cal = CalculateOrionGroup(orionDate, effectiveAtt);
                // orionGroupStr 1 = <= 6 months
                // orionGroupStr 2 = <= 12 months
                // orionGroupStr 3 = > 12 months
                int orionGroupStr;
                if (cal > 12)
                {
                    orionGroupStr = 3;
                }
                else if (cal > 6 && cal <= 12)
                {
                    orionGroupStr = 2;
                }
                else if (cal <= 6)
                {
                    orionGroupStr = 1;
                }
                else
                {
                    orionGroupStr = 1;
                }

                return orionGroupStr;
            }
            else
            {
                return 0;
            }
        }
        public static string GenerateOrionGroupStr(DateTime? effectiveAt)
        {
            if (effectiveAt.HasValue)
            {
                DateTime orionDate;
                DateTime quarter1 = DateTime.Parse("31 Mar " + DateTime.Today.Year.ToString());
                DateTime quarter2 = DateTime.Parse("30 Jun " + DateTime.Today.Year.ToString());
                DateTime quarter3 = DateTime.Parse("30 Sep " + DateTime.Today.Year.ToString());
                DateTime quarter4 = DateTime.Parse("31 Dec " + DateTime.Today.Year.ToString());

                if (DateTime.Now <= quarter1)
                {
                    orionDate = quarter1;
                }
                else if (DateTime.Now <= quarter2)
                {
                    orionDate = quarter2;
                }
                else if (DateTime.Now <= quarter3)
                {
                    orionDate = quarter3;
                }
                else
                {
                    orionDate = quarter4;
                }

                var cal = CalculateOrionGroup(orionDate, effectiveAt);
                // orionGroupStr 1 = <= 6 months
                // orionGroupStr 2 = <= 12 months
                // orionGroupStr 3 = > 12 months
                string orionGroupStr;
                if (cal > 12)
                {
                    orionGroupStr = "> 12 months";
                }
                else if (cal > 6 && cal <= 12)
                {
                    orionGroupStr = "<= 12 months";
                }
                else if (cal <= 6)
                {
                    orionGroupStr = "<= 6 months";
                }
                else
                {
                    orionGroupStr = "<= 6 months";
                }

                return orionGroupStr;
            }
            else
            {
                return "";
            }
        }

        private static double CalculateOrionGroup(DateTime orionDate, DateTime? effectiveAt)
        {
            var cal = orionDate - effectiveAt;
            if (cal.HasValue)
            {
                TimeSpan call = cal.Value;

                return call.Days / 356.25 * 12;
            }
            else
            {
                return 0;
            }
        }

        #endregion


    }
}
