using BusinessObject;
using BusinessObject.Claims;
using BusinessObject.InvoiceRegisters;
using BusinessObject.Retrocession;
using BusinessObject.RiDatas;
using BusinessObject.TreatyPricing;
using DataAccess.Entities;
using DataAccess.Entities.Retrocession;
using DataAccess.Entities.RiDatas;
using DataAccess.EntityFramework;
using Services.Claims;
using Services.InvoiceRegisters;
using Services.Retrocession;
using Services.RiDatas;
using Services.TreatyPricing;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Services
{
    public class ExportService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(Export)),
                Controller = ModuleBo.ModuleController.Export.ToString(),
            };
        }

        public static Expression<Func<Export, ExportBo>> Expression()
        {
            return entity => new ExportBo
            {
                Id = entity.Id,
                Type = entity.Type,
                Status = entity.Status,
                ObjectId = entity.ObjectId,
                Total = entity.Total,
                Processed = entity.Processed,
                Parameters = entity.Parameters,
                GenerateStartAt = entity.GenerateStartAt,
                GenerateEndAt = entity.GenerateEndAt,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static ExportBo FormBo(Export entity = null)
        {
            if (entity == null)
                return null;
            return new ExportBo
            {
                Id = entity.Id,
                Type = entity.Type,
                Status = entity.Status,
                ObjectId = entity.ObjectId,
                Total = entity.Total,
                Processed = entity.Processed,
                Parameters = entity.Parameters,
                GenerateStartAt = entity.GenerateStartAt,
                GenerateEndAt = entity.GenerateEndAt,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<ExportBo> FormBos(IList<Export> entities = null)
        {
            if (entities == null)
                return null;
            IList<ExportBo> bos = new List<ExportBo>() { };
            foreach (Export entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static Export FormEntity(ExportBo bo = null)
        {
            if (bo == null)
                return null;
            return new Export
            {
                Id = bo.Id,
                Type = bo.Type,
                Status = bo.Status,
                ObjectId = bo.ObjectId,
                Total = bo.Total,
                Processed = bo.Processed,
                Parameters = bo.Parameters,
                GenerateStartAt = bo.GenerateStartAt,
                GenerateEndAt = bo.GenerateEndAt,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return Export.IsExists(id);
        }

        public static ExportBo Find(int id)
        {
            return FormBo(Export.Find(id));
        }

        public static ExportBo Find(int? id)
        {
            if (id == null)
                return null;
            return FormBo(Export.Find(id.Value));
        }

        public static ExportBo FindByStatus(int status, int? id = null)
        {
            return FormBo(Export.FindByStatus(status, id));
        }

        public static int Count()
        {
            return Export.Count();
        }

        public static int CountByStatus(int status)
        {
            return Export.CountByStatus(status);
        }

        public static int CountQueryTotal(ref ExportBo exportBo, AppDbContext db, IQueryable<object> query = null)
        {
            int count = 0;

            if (query == null)
                query = GetQuery(ref exportBo, db);

            if (query != null)
                count = query.Count();

            exportBo.Total = count;

            return count;
        }

        public static IQueryable<object> GetQuery(ref ExportBo exportBo, AppDbContext db)
        {
            switch (exportBo.Type)
            {
                case ExportBo.TypeRiData:
                    return GetRiDataQuery(ref exportBo, db);
                case ExportBo.TypeClaimData:
                    return GetClaimDataQuery(ref exportBo, db);
                case ExportBo.TypeClaimRegister:
                    return GetClaimRegisterQuery(ref exportBo, db);
                case ExportBo.TypeRiDataWarehouse:
                    return GetRiDataWarehouseQuery(ref exportBo, db);
                case ExportBo.TypeRiDataSearch:
                    return GetRiDataSearchQuery(ref exportBo, db);
                case ExportBo.TypeClaimRegisterSearch:
                    return GetClaimRegisterSearchQuery(ref exportBo, db);
                case ExportBo.TypeReferralClaim:
                    return GetReferralClaimQuery(ref exportBo, db);
                case ExportBo.TypeInvoiceRegister:
                    return GetInvoiceRegisterQuery(ref exportBo, db);
                case ExportBo.TypeRetroRegister:
                    return GetRetroRegisterQuery(ref exportBo, db);
                case ExportBo.TypeClaimRegisterHistorySearch:
                    return GetClaimRegisterHistorySearchQuery(ref exportBo, db);
                case ExportBo.TypeQuotationWorkflow:
                    return GetQuotationWorkflowQuery(ref exportBo, db);
                case ExportBo.TypeTreatyWorkflow:
                    return GetTreatyWorkflowQuery(ref exportBo, db);
                case ExportBo.TypeRiDataWarehouseHistory:
                    return GetRiDataWarehouseHistoryQuery(ref exportBo, db);
                case ExportBo.TypeGroupReferral:
                    return GetGroupReferralQuery(ref exportBo, db);
                case ExportBo.TypePerLifeAggregationConflictListing:
                    return GetPerLifeAggregationConflictListingQuery(ref exportBo, db);
                case ExportBo.TypePerLifeAggregationDuplicationListing:
                    return GetPerLifeAggregationDuplicationListingQuery(ref exportBo, db);
                case ExportBo.TypePerLifeAggregationRiData:
                    return GetPerLifeAggregationRiDataQuery(ref exportBo, db);
                case ExportBo.TypePerLifeAggregationException:
                    return GetPerLifeAggregationExceptionQuery(ref exportBo, db);
                case ExportBo.TypePerLifeAggregationRetroRiData:
                    return GetPerLifeAggregationRetroRiDataQuery(ref exportBo, db);
                case ExportBo.TypePerLifeAggregationRetentionPremium:
                    return GetPerLifeAggregationRetentionPremiumQuery(ref exportBo, db);
                case ExportBo.TypePerLifeAggregationRetroSummaryExcludedRecord:
                case ExportBo.TypePerLifeAggregationSummaryExcludedRecord:
                    return GetPerLifeAggregationDetailDataQuery(ref exportBo, db);
                case ExportBo.TypePerLifeAggregationRetroSummaryRetro:
                case ExportBo.TypePerLifeAggregationSummaryRetro:
                    return GetPerLifeAggregationMonthlyDataQuery(ref exportBo, db);
                case ExportBo.TypeGroupReferralTrackingCase:
                    return GetGroupReferralTrackingQuery(ref exportBo, db);
                case ExportBo.TypeFacMasterListing:
                    return GetFacMasterListingQuery(ref exportBo, db);
                case ExportBo.TypeRateDetail:
                    return GetRateDetailQuery(ref exportBo, db);
            }
            return null;
        }

        public static IQueryable<RiDataBo> GetRiDataQuery(ref ExportBo exportBo, AppDbContext db)
        {
            if (exportBo.ObjectId == null)
                return null;

            var objectId = exportBo.ObjectId;
            var distinctTreatyCodes = RiDataBatchService.GetDistinctTreatyCodesById(objectId.Value);
            var paramObject = exportBo.ParameterObject;
            var riDataBatch = db.RiDataBatches.Where(q => q.Id == objectId);
            var treatyIdCode = riDataBatch.Select(q => q.Treaty.TreatyIdCode).FirstOrDefault().Split(' ')[0];
            var cedantCode = riDataBatch.Select(q => q.Cedant.Code).FirstOrDefault();
            var query = db.RiData.Where(q => q.RiDataBatchId == objectId && (distinctTreatyCodes.Contains(q.TreatyCode) || string.IsNullOrEmpty(q.TreatyCode))).Select(RiDataService.Expression());
            if (paramObject != null)
            {
                string TransactionTypeCode = Util.HasProperty(paramObject, "TransactionTypeCode") ? paramObject.TransactionTypeCode : null;
                string TreatyCode = Util.HasProperty(paramObject, "TreatyCode") ? paramObject.TreatyCode : null;
                string InsuredName = Util.HasProperty(paramObject, "InsuredName") ? paramObject.InsuredName : null;
                string PolicyNumber = Util.HasProperty(paramObject, "PolicyNumber") ? paramObject.PolicyNumber : null;
                string PolicyTerm = Util.HasProperty(paramObject, "PolicyTerm") ? paramObject.PolicyTerm : null;
                string ConflictType = Util.HasProperty(paramObject, "ConflictType") ? paramObject.ConflictType : null;

                int r = 0;
                int? RecordType = null;
                if (Util.HasProperty(paramObject, "RecordType") && paramObject.RecordType != null && int.TryParse(paramObject.RecordType.ToString(), out r))
                {
                    RecordType = r;
                }
                int? TransactionTypeCodeId = null;
                if (Util.HasProperty(paramObject, "TransactionTypeCodeId") && paramObject.TransactionTypeCodeId != null && int.TryParse(paramObject.TransactionTypeCodeId.ToString(), out r))
                {
                    TransactionTypeCodeId = r;
                }
                int? ReinsBasicCodeId = null;
                if (Util.HasProperty(paramObject, "ReinsBasicCodeId") && paramObject.ReinsBasicCodeId != null && int.TryParse(paramObject.ReinsBasicCodeId.ToString(), out r))
                {
                    ReinsBasicCodeId = r;
                }
                int? ReportPeriodMonth = null;
                if (Util.HasProperty(paramObject, "ReportPeriodMonth") && paramObject.ReportPeriodMonth != null && int.TryParse(paramObject.ReportPeriodMonth.ToString(), out r))
                {
                    ReportPeriodMonth = r;
                }
                int? ReportPeriodYear = null;
                if (Util.HasProperty(paramObject, "ReportPeriodYear") && paramObject.ReportPeriodYear != null && int.TryParse(paramObject.ReportPeriodYear.ToString(), out r))
                {
                    ReportPeriodYear = r;
                }
                int? RiskPeriodMonth = null;
                if (Util.HasProperty(paramObject, "RiskPeriodMonth") && paramObject.RiskPeriodMonth != null && int.TryParse(paramObject.RiskPeriodMonth.ToString(), out r))
                {
                    RiskPeriodMonth = r;
                }
                int? RiskPeriodYear = null;
                if (Util.HasProperty(paramObject, "RiskPeriodYear") && paramObject.RiskPeriodYear != null && int.TryParse(paramObject.RiskPeriodYear.ToString(), out r))
                {
                    RiskPeriodYear = r;
                }
                int? PreComputation1Status = null;
                if (Util.HasProperty(paramObject, "PreComputation1Status") && paramObject.PreComputation1Status != null && int.TryParse(paramObject.PreComputation1Status.ToString(), out r))
                {
                    PreComputation1Status = r;
                }
                int? PreComputation2Status = null;
                if (Util.HasProperty(paramObject, "PreComputation2Status") && paramObject.PreComputation2Status != null && int.TryParse(paramObject.PreComputation2Status.ToString(), out r))
                {
                    PreComputation2Status = r;
                }
                int? PreValidationStatus = null;
                if (Util.HasProperty(paramObject, "PreValidationStatus") && paramObject.PreValidationStatus != null && int.TryParse(paramObject.PreValidationStatus.ToString(), out r))
                {
                    PreValidationStatus = r;
                }
                int? PostComputationStatus = null;
                if (Util.HasProperty(paramObject, "PostComputationStatus") && paramObject.PostComputationStatus != null && int.TryParse(paramObject.PostComputationStatus.ToString(), out r))
                {
                    PostComputationStatus = r;
                }
                int? PostValidationStatus = null;
                if (Util.HasProperty(paramObject, "PostValidationStatus") && paramObject.PostValidationStatus != null && int.TryParse(paramObject.PostValidationStatus.ToString(), out r))
                {
                    PostValidationStatus = r;
                }
                int? MappingStatus = null;
                if (Util.HasProperty(paramObject, "MappingStatus") && paramObject.MappingStatus != null && int.TryParse(paramObject.MappingStatus.ToString(), out r))
                {
                    MappingStatus = r;
                }
                int? FinaliseStatus = null;
                if (Util.HasProperty(paramObject, "FinaliseStatus") && paramObject.FinaliseStatus != null && int.TryParse(paramObject.FinaliseStatus.ToString(), out r))
                {
                    FinaliseStatus = r;
                }

                if (!string.IsNullOrEmpty(TransactionTypeCode)) query = query.Where(q => !string.IsNullOrEmpty(q.TransactionTypeCode) && q.TreatyCode.Contains(TransactionTypeCode));
                if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => !string.IsNullOrEmpty(q.TreatyCode) && q.TreatyCode.Contains(TreatyCode));
                if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => !string.IsNullOrEmpty(q.InsuredName) && q.InsuredName.Contains(InsuredName));
                if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => !string.IsNullOrEmpty(q.PolicyNumber) && q.PolicyNumber.Contains(PolicyNumber));
                if (!string.IsNullOrEmpty(PolicyTerm)) query = query.Where(q => !string.IsNullOrEmpty(q.PolicyTerm.ToString()) && q.PolicyTerm.ToString().Contains(PolicyTerm));
                if (RecordType != null) query = query.Where(q => q.RecordType == RecordType);
                if (TransactionTypeCodeId != null)
                {
                    PickListDetailBo pickListDetailBo = PickListDetailService.Find(TransactionTypeCodeId);
                    query = query.Where(q => !string.IsNullOrEmpty(q.TransactionTypeCode) && q.TransactionTypeCode.Contains(pickListDetailBo.Code));
                }
                if (ReinsBasicCodeId != null)
                {
                    PickListDetailBo pickListDetailBo = PickListDetailService.Find(ReinsBasicCodeId);
                    query = query.Where(q => !string.IsNullOrEmpty(q.ReinsBasisCode) && q.ReinsBasisCode.Contains(pickListDetailBo.Code));
                }
                if (ReportPeriodMonth != null) query = query.Where(q => q.ReportPeriodMonth == ReportPeriodMonth);
                if (ReportPeriodYear != null) query = query.Where(q => q.ReportPeriodYear == ReportPeriodYear);
                if (RiskPeriodMonth != null) query = query.Where(q => q.RiskPeriodMonth == RiskPeriodMonth);
                if (RiskPeriodYear != null) query = query.Where(q => q.RiskPeriodYear == RiskPeriodYear);
                if (PreComputation1Status != null) query = query.Where(q => q.PreComputation1Status == PreComputation1Status);
                if (PreComputation2Status != null) query = query.Where(q => q.PreComputation2Status == PreComputation2Status);
                if (PreValidationStatus != null) query = query.Where(q => q.PreValidationStatus == PreValidationStatus);

                if (!string.IsNullOrEmpty(ConflictType))
                {
                    string[] ConflictTypes = Util.ToArraySplitTrim(ConflictType);
                    query = query.Where(q => ConflictTypes.Contains(q.ConflictType.ToString()));
                }

                if (PostComputationStatus != null) query = query.Where(q => q.PostComputationStatus == PostComputationStatus);
                if (PostValidationStatus != null) query = query.Where(q => q.PostValidationStatus == PostValidationStatus);
                if (MappingStatus != null) query = query.Where(q => q.MappingStatus == MappingStatus);
                if (FinaliseStatus != null) query = query.Where(q => q.FinaliseStatus == FinaliseStatus);
            }

            return query;
        }

        public static IQueryable<ClaimDataBo> GetClaimDataQuery(ref ExportBo exportBo, AppDbContext db)
        {
            var objectId = exportBo.ObjectId;
            var paramObject = exportBo.ParameterObject;
            var query = db.ClaimData.Where(q => q.ClaimDataBatchId == objectId).Select(ClaimDataService.Expression());
            if (paramObject != null)
            {
                string MlreEventCode = Util.HasProperty(paramObject, "MlreEventCode") ? paramObject.MlreEventCode : null;
                string ClaimCode = Util.HasProperty(paramObject, "ClaimCode") ? paramObject.ClaimCode : null;
                string PolicyNumber = Util.HasProperty(paramObject, "PolicyNumber") ? paramObject.PolicyNumber : null;
                string TreatyType = Util.HasProperty(paramObject, "TreatyType") ? paramObject.TreatyType : null;
                string InsuredName = Util.HasProperty(paramObject, "InsuredName") ? paramObject.InsuredName : null;
                string InsuredGenderCode = Util.HasProperty(paramObject, "InsuredGenderCode") ? paramObject.InsuredGenderCode : null;

                int i = 0;
                int? MappingStatus = null;
                if (Util.HasProperty(paramObject, "MappingStatus") && paramObject.MappingStatus != null && int.TryParse(paramObject.MappingStatus.ToString(), out i))
                {
                    MappingStatus = i;
                }

                int? PreComputationStatus = null;
                if (Util.HasProperty(paramObject, "ComputationStatus") && paramObject.ComputationStatus != null && int.TryParse(paramObject.ComputationStatus.ToString(), out i))
                {
                    PreComputationStatus = i;
                }

                int? PreValidationStatus = null;
                if (Util.HasProperty(paramObject, "PreValidationStatus") && paramObject.PreValidationStatus != null && int.TryParse(paramObject.PreValidationStatus.ToString(), out i))
                {
                    PreValidationStatus = i;
                }

                double d = 0;
                double? Layer1SumRein = null;
                if (Util.HasProperty(paramObject, "Layer1SumRein") && paramObject.Layer1SumRein != null && double.TryParse(paramObject.Layer1SumRein.ToString(), out d))
                {
                    Layer1SumRein = d;
                }

                if (!string.IsNullOrEmpty(MlreEventCode)) query = query.Where(q => !string.IsNullOrEmpty(q.EntryNo) && q.EntryNo.Contains(MlreEventCode));
                if (!string.IsNullOrEmpty(ClaimCode)) query = query.Where(q => !string.IsNullOrEmpty(q.SoaQuarter) && q.SoaQuarter == ClaimCode);
                if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => !string.IsNullOrEmpty(q.PolicyNumber) && q.PolicyNumber.Contains(PolicyNumber));
                if (!string.IsNullOrEmpty(TreatyType)) query = query.Where(q => !string.IsNullOrEmpty(q.TreatyType) && q.TreatyType == TreatyType);
                if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => !string.IsNullOrEmpty(q.InsuredName) && q.InsuredName.Contains(InsuredName));
                if (!string.IsNullOrEmpty(InsuredGenderCode)) query = query.Where(q => !string.IsNullOrEmpty(q.InsuredGenderCode) && q.InsuredGenderCode == InsuredGenderCode);

                if (MappingStatus.HasValue) query = query.Where(q => q.MappingStatus == MappingStatus);
                if (PreComputationStatus.HasValue) query = query.Where(q => q.PreComputationStatus == PreComputationStatus);
                if (PreValidationStatus.HasValue) query = query.Where(q => q.PreValidationStatus == PreValidationStatus);

                if (Layer1SumRein.HasValue) query = query.Where(q => q.Layer1SumRein == Layer1SumRein);
            }

            return query;
        }

        public static IQueryable<ClaimRegisterBo> GetClaimRegisterQuery(ref ExportBo exportBo, AppDbContext db)
        {
            var objectId = exportBo.ObjectId;
            var paramObject = exportBo.ParameterObject;
            var query = db.ClaimRegister.Select(ClaimRegisterService.Expression());
            if (paramObject != null)
            {
                string EntryNo = Util.HasProperty(paramObject, "EntryNo") ? paramObject.EntryNo : null;
                string SoaQuarter = Util.HasProperty(paramObject, "SoaQuarter") ? paramObject.SoaQuarter : null;
                string ClaimId = Util.HasProperty(paramObject, "ClaimId") ? paramObject.ClaimId : null;
                string ClaimTransactionType = Util.HasProperty(paramObject, "ClaimTransactionType") ? paramObject.ClaimTransactionType : null;
                string RecordType = Util.HasProperty(paramObject, "RecordType") ? paramObject.RecordType : null;
                string TreatyCode = Util.HasProperty(paramObject, "TreatyCode") ? paramObject.TreatyCode : null;
                string PolicyNumber = Util.HasProperty(paramObject, "PolicyNumber") ? paramObject.PolicyNumber : null;
                string CedingCompany = Util.HasProperty(paramObject, "CedingCompany") ? paramObject.CedingCompany : null;
                string InsuredName = Util.HasProperty(paramObject, "InsuredName") ? paramObject.InsuredName : null;
                string CauseOfEvent = Util.HasProperty(paramObject, "CauseOfEvent") ? paramObject.CauseOfEvent : null;

                int i = 0;
                int? RiDataWarehouseId = null;
                if (Util.HasProperty(paramObject, "RiDataWarehouseId") && paramObject.RiDataWarehouseId != null && int.TryParse(paramObject.RiDataWarehouseId.ToString(), out i))
                {
                    RiDataWarehouseId = i;
                }

                int? PicClaimId = null;
                if (Util.HasProperty(paramObject, "PicClaimId") && paramObject.PicClaimId != null && int.TryParse(paramObject.PicClaimId.ToString(), out i))
                {
                    PicClaimId = i;
                }

                int? PicDaaId = null;
                if (Util.HasProperty(paramObject, "PicDaaId") && paramObject.PicDaaId != null && int.TryParse(paramObject.PicDaaId.ToString(), out i))
                {
                    PicDaaId = i;
                }

                int? ClaimStatus = null;
                if (Util.HasProperty(paramObject, "ClaimStatus") && paramObject.ClaimStatus != null && int.TryParse(paramObject.ClaimStatus.ToString(), out i))
                {
                    ClaimStatus = i;
                }

                int? DuplicationCheckStatus = null;
                if (Util.HasProperty(paramObject, "DuplicationCheckStatus") && paramObject.DuplicationCheckStatus != null && int.TryParse(paramObject.DuplicationCheckStatus.ToString(), out i))
                {
                    DuplicationCheckStatus = i;
                }

                int? ProvisionStatus = null;
                if (Util.HasProperty(paramObject, "ProvisionStatus") && paramObject.ProvisionStatus != null && int.TryParse(paramObject.ProvisionStatus.ToString(), out i))
                {
                    ProvisionStatus = i;
                }

                int? OffsetStatus = null;
                if (Util.HasProperty(paramObject, "OffsetStatus") && paramObject.OffsetStatus != null && int.TryParse(paramObject.OffsetStatus.ToString(), out i))
                {
                    OffsetStatus = i;
                }

                bool b = false;
                bool? IsReferralCase = null;
                if (Util.HasProperty(paramObject, "IsReferralCase") && paramObject.IsReferralCase != null && bool.TryParse(paramObject.IsReferralCase.ToString(), out b))
                {
                    IsReferralCase = b;
                }

                bool? IsClaimOnly = null;
                if (Util.HasProperty(paramObject, "IsClaimOnly") && paramObject.IsClaimOnly != null && bool.TryParse(paramObject.IsClaimOnly.ToString(), out b))
                {
                    IsClaimOnly = b;
                }

                DateTime dt = DateTime.Today;
                DateTime? InsuredDateOfBirth = null;
                if (Util.HasProperty(paramObject, "InsuredDateOfBirth") && paramObject.InsuredDateOfBirth != null && DateTime.TryParse(paramObject.InsuredDateOfBirth.ToString(), out dt))
                {
                    InsuredDateOfBirth = dt;
                }

                DateTime? LastTransactionDate = null;
                if (Util.HasProperty(paramObject, "LastTransactionDate") && paramObject.LastTransactionDate != null && DateTime.TryParse(paramObject.LastTransactionDate.ToString(), out dt))
                {
                    LastTransactionDate = dt;
                }

                DateTime? DateOfReported = null;
                if (Util.HasProperty(paramObject, "DateOfReported") && paramObject.DateOfReported != null && DateTime.TryParse(paramObject.DateOfReported.ToString(), out dt))
                {
                    DateOfReported = dt;
                }

                DateTime? CedantDateOfNotification = null;
                if (Util.HasProperty(paramObject, "CedantDateOfNotification") && paramObject.CedantDateOfNotification != null && DateTime.TryParse(paramObject.CedantDateOfNotification.ToString(), out dt))
                {
                    CedantDateOfNotification = dt;
                }

                DateTime? DateOfRegister = null;
                if (Util.HasProperty(paramObject, "DateOfRegister") && paramObject.DateOfRegister != null && DateTime.TryParse(paramObject.DateOfRegister.ToString(), out dt))
                {
                    DateOfRegister = dt;
                }

                DateTime? DateOfCommencement = null;
                if (Util.HasProperty(paramObject, "DateOfCommencement") && paramObject.DateOfCommencement != null && DateTime.TryParse(paramObject.DateOfCommencement.ToString(), out dt))
                {
                    DateOfCommencement = dt;
                }

                DateTime? DateOfEvent = null;
                if (Util.HasProperty(paramObject, "DateOfEvent") && paramObject.DateOfEvent != null && DateTime.TryParse(paramObject.DateOfEvent.ToString(), out dt))
                {
                    DateOfEvent = dt;
                }

                DateTime? TargetDateToIssueInvoice = null;
                if (Util.HasProperty(paramObject, "TargetDateToIssueInvoice") && paramObject.TargetDateToIssueInvoice != null && DateTime.TryParse(paramObject.TargetDateToIssueInvoice.ToString(), out dt))
                {
                    TargetDateToIssueInvoice = dt;
                }

                double d = 0;
                double? ClaimRecoveryAmt = null;
                if (Util.HasProperty(paramObject, "ClaimRecoveryAmt") && paramObject.ClaimRecoveryAmt != null && double.TryParse(paramObject.ClaimRecoveryAmt.ToString(), out d))
                {
                    ClaimRecoveryAmt = d;
                }

                if (!string.IsNullOrEmpty(EntryNo)) query = query.Where(q => !string.IsNullOrEmpty(q.EntryNo) && q.EntryNo.Contains(EntryNo));
                if (!string.IsNullOrEmpty(SoaQuarter)) query = query.Where(q => !string.IsNullOrEmpty(q.SoaQuarter) && q.SoaQuarter == SoaQuarter);
                if (!string.IsNullOrEmpty(ClaimId)) query = query.Where(q => !string.IsNullOrEmpty(q.ClaimId) && q.ClaimId == ClaimId);
                if (!string.IsNullOrEmpty(ClaimTransactionType)) query = query.Where(q => !string.IsNullOrEmpty(q.ClaimTransactionType) && q.ClaimTransactionType == ClaimTransactionType);
                if (!string.IsNullOrEmpty(RecordType)) query = query.Where(q => !string.IsNullOrEmpty(q.RecordType) && q.RecordType.Contains(RecordType));
                if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => !string.IsNullOrEmpty(q.TreatyCode) && q.TreatyCode == TreatyCode);
                if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => !string.IsNullOrEmpty(q.PolicyNumber) && q.PolicyNumber.Contains(PolicyNumber));
                if (!string.IsNullOrEmpty(CedingCompany)) query = query.Where(q => !string.IsNullOrEmpty(q.CedingCompany) && q.CedingCompany.Contains(CedingCompany));
                if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => !string.IsNullOrEmpty(q.InsuredName) && q.InsuredName.Contains(InsuredName));
                if (!string.IsNullOrEmpty(CauseOfEvent)) query = query.Where(q => !string.IsNullOrEmpty(q.CauseOfEvent) && q.CauseOfEvent.Contains(CauseOfEvent));

                if (RiDataWarehouseId.HasValue) query = query.Where(q => q.RiDataWarehouseId == RiDataWarehouseId);
                if (PicClaimId.HasValue) query = query.Where(q => q.PicClaimId == PicClaimId);
                if (PicDaaId.HasValue) query = query.Where(q => q.PicDaaId == PicDaaId);
                if (ClaimStatus.HasValue) query = query.Where(q => q.ClaimStatus == ClaimStatus);
                if (DuplicationCheckStatus.HasValue) query = query.Where(q => q.DuplicationCheckStatus == DuplicationCheckStatus);
                else if (IsClaimOnly.HasValue && IsClaimOnly.Value)
                {
                    List<int> statuses = ClaimRegisterBo.GetClaimDepartmentStatus();
                    query = query.Where(q => statuses.Contains(q.ClaimStatus));
                }
                if (ProvisionStatus.HasValue) query = query.Where(q => q.ProvisionStatus == ProvisionStatus);
                if (OffsetStatus.HasValue) query = query.Where(q => q.OffsetStatus == OffsetStatus);

                if (IsReferralCase.HasValue) query = query.Where(q => q.IsReferralCase == IsReferralCase);

                if (InsuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == InsuredDateOfBirth);
                if (LastTransactionDate.HasValue) query = query.Where(q => q.LastTransactionDate == LastTransactionDate);
                if (DateOfReported.HasValue) query = query.Where(q => q.DateOfReported == DateOfReported);
                if (CedantDateOfNotification.HasValue) query = query.Where(q => q.CedantDateOfNotification == CedantDateOfNotification);
                if (DateOfRegister.HasValue) query = query.Where(q => q.DateOfRegister == DateOfRegister);
                if (DateOfCommencement.HasValue) query = query.Where(q => q.ReinsEffDatePol == DateOfCommencement);
                if (DateOfEvent.HasValue) query = query.Where(q => q.DateOfEvent == DateOfEvent);
                if (TargetDateToIssueInvoice.HasValue) query = query.Where(q => q.TargetDateToIssueInvoice == TargetDateToIssueInvoice);

                if (ClaimRecoveryAmt.HasValue) query = query.Where(q => q.ClaimRecoveryAmt == ClaimRecoveryAmt);
            }

            return query;
        }

        public static IQueryable<RiDataWarehouseBo> GetRiDataWarehouseQuery(ref ExportBo exportBo, AppDbContext db)
        {
            var objectId = exportBo.ObjectId;
            var paramObject = exportBo.ParameterObject;
            var query = db.RiDataWarehouse.Select(RiDataWarehouseService.Expression());
            if (paramObject != null)
            {
                string TreatyCode = Util.HasProperty(paramObject, "TreatyCode") ? paramObject.TreatyCode : null;
                string SoaQuarter = Util.HasProperty(paramObject, "SoaQuarter") ? paramObject.SoaQuarter : null;
                string InsuredName = Util.HasProperty(paramObject, "InsuredName") ? paramObject.InsuredName : null;
                string PolicyNumber = Util.HasProperty(paramObject, "PolicyNumber") ? paramObject.PolicyNumber : null;
                string CedingPlanCode = Util.HasProperty(paramObject, "CedingPlanCode") ? paramObject.CedingPlanCode : null;
                string MlreBenefitCode = Util.HasProperty(paramObject, "BenefitCode") ? paramObject.BenefitCode : null;
                string TreatyCodeFilter = Util.HasProperty(paramObject, "TreatyCodeFilter") ? paramObject.TreatyCodeFilter : null;
                string BenefitCodeFilter = Util.HasProperty(paramObject, "BenefitCodeFilter") ? paramObject.BenefitCodeFilter : null;
                string ReinsBasisCode = Util.HasProperty(paramObject, "ReinsBasisCode") ? paramObject.ReinsBasisCode : null;
                string FundsAccountingTypeCode = Util.HasProperty(paramObject, "FundsAccountingTypeCode") ? paramObject.FundsAccountingTypeCode : null;
                string PremiumFrequencyCode = Util.HasProperty(paramObject, "PremiumFrequencyCode") ? paramObject.PremiumFrequencyCode : null;
                string TransactionTypeCode = Util.HasProperty(paramObject, "TransactionTypeCode") ? paramObject.TransactionTypeCode : null;
                string PolicyNumberFilter = Util.HasProperty(paramObject, "PolicyNumberFilter") ? paramObject.PolicyNumberFilter : null;

                string[] treatyCodes = (string.IsNullOrEmpty(TreatyCode)) ? new string[] { } : Util.ToArraySplitTrim(TreatyCode, emptyString: false);
                string[] benefitCodes = Util.ToArraySplitTrim(MlreBenefitCode);

                int i = 0;
                int? CedantId = null;
                if (Util.HasProperty(paramObject, "CedantId") && paramObject.CedantId != null && int.TryParse(paramObject.CedantId.ToString(), out i))
                {
                    CedantId = i;
                }

                int? ReportPeriodMonth = null;
                if (Util.HasProperty(paramObject, "ReportPeriodMonth") && paramObject.ReportPeriodMonth != null && int.TryParse(paramObject.ReportPeriodMonth.ToString(), out i))
                {
                    ReportPeriodMonth = i;
                }

                int? ReportPeriodYear = null;
                if (Util.HasProperty(paramObject, "ReportPeriodYear") && paramObject.ReportPeriodYear != null && int.TryParse(paramObject.ReportPeriodYear.ToString(), out i))
                {
                    ReportPeriodYear = i;
                }

                DateTime dt = DateTime.Today;
                DateTime? RiskPeriodStartDate = null;
                if (Util.HasProperty(paramObject, "RiskPeriodStartDate") && paramObject.RiskPeriodStartDate != null && DateTime.TryParse(paramObject.RiskPeriodStartDate.ToString(), out dt))
                {
                    RiskPeriodStartDate = dt;
                }

                DateTime? RiskPeriodEndDate = null;
                if (Util.HasProperty(paramObject, "RiskPeriodEndDate") && paramObject.RiskPeriodEndDate != null && DateTime.TryParse(paramObject.RiskPeriodEndDate.ToString(), out dt))
                {
                    RiskPeriodEndDate = dt;
                }

                DateTime? InsuredDateOfBirth = null;
                if (Util.HasProperty(paramObject, "InsuredDateOfBirth") && paramObject.InsuredDateOfBirth != null && DateTime.TryParse(paramObject.InsuredDateOfBirth.ToString(), out dt))
                {
                    InsuredDateOfBirth = dt;
                }

                DateTime? IssueDatePol = null;
                if (Util.HasProperty(paramObject, "IssueDatePol") && paramObject.IssueDatePol != null && DateTime.TryParse(paramObject.IssueDatePol.ToString(), out dt))
                {
                    IssueDatePol = dt;
                }

                DateTime? IssueDateBen = null;
                if (Util.HasProperty(paramObject, "IssueDateBen") && paramObject.IssueDateBen != null && DateTime.TryParse(paramObject.IssueDateBen.ToString(), out dt))
                {
                    IssueDateBen = dt;
                }

                DateTime? ReinsEffDatePol = null;
                if (Util.HasProperty(paramObject, "ReinsEffDatePol") && paramObject.ReinsEffDatePol != null && DateTime.TryParse(paramObject.ReinsEffDatePol.ToString(), out dt))
                {
                    ReinsEffDatePol = dt;
                }

                if (CedantId.HasValue && string.IsNullOrEmpty(TreatyCode))
                {
                    var treatyCodeBos = TreatyCodeService.GetByCedantId(CedantId.Value);
                    treatyCodes = !treatyCodeBos.IsNullOrEmpty() ? treatyCodeBos.Select(q => q.Code).ToArray() : new string[] { };
                }

                if (!CedantId.HasValue &&
                    string.IsNullOrEmpty(TreatyCode) &&
                    string.IsNullOrEmpty(SoaQuarter) &&
                    !RiskPeriodStartDate.HasValue &&
                    !RiskPeriodEndDate.HasValue &&
                    string.IsNullOrEmpty(InsuredName) &&
                    string.IsNullOrEmpty(PolicyNumber) &&
                    !InsuredDateOfBirth.HasValue &&
                    string.IsNullOrEmpty(CedingPlanCode) &&
                    string.IsNullOrEmpty(MlreBenefitCode))
                {
                    query = query.Where(q => q.Id == 0); // Dont get any data
                }
                else
                {
                    if (!treatyCodes.IsNullOrEmpty()) query = query.Where(q => treatyCodes.Contains(q.TreatyCode));
                    if (!string.IsNullOrEmpty(SoaQuarter))
                    {
                        string[] quarterStr = SoaQuarter.Split(' ');
                        List<int> months = new List<int> { };

                        switch (quarterStr[1])
                        {
                            case "Q1": months = new List<int> { 1, 2, 3 }; break;
                            case "Q2": months = new List<int> { 4, 5, 6 }; break;
                            case "Q3": months = new List<int> { 7, 8, 9 }; break;
                            case "Q4": months = new List<int> { 10, 11, 12 }; break;
                        }

                        int quarterYear = Convert.ToInt32(quarterStr[0]);
                        query = query.Where(q => months.Contains(q.ReportPeriodMonth.Value) && q.ReportPeriodYear == quarterYear);
                    }
                    if (RiskPeriodStartDate.HasValue) query = query.Where(q => q.RiskPeriodStartDate >= RiskPeriodStartDate);
                    if (RiskPeriodEndDate.HasValue) query = query.Where(q => q.RiskPeriodEndDate <= RiskPeriodEndDate);
                    if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => q.InsuredName == InsuredName);
                    if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber == PolicyNumber);
                    if (InsuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == InsuredDateOfBirth);
                    if (!string.IsNullOrEmpty(CedingPlanCode)) query = query.Where(q => q.CedingPlanCode == CedingPlanCode);
                    if (!string.IsNullOrEmpty(MlreBenefitCode)) query = query.Where(q => benefitCodes.Contains(q.MlreBenefitCode));
                    if (!string.IsNullOrEmpty(TreatyCodeFilter)) query = query.Where(q => q.TreatyCode == TreatyCodeFilter);
                    if (!string.IsNullOrEmpty(BenefitCodeFilter)) query = query.Where(q => q.MlreBenefitCode == BenefitCodeFilter);
                    if (!string.IsNullOrEmpty(ReinsBasisCode)) query = query.Where(q => q.ReinsBasisCode == ReinsBasisCode);
                    if (!string.IsNullOrEmpty(FundsAccountingTypeCode)) query = query.Where(q => q.FundsAccountingTypeCode == FundsAccountingTypeCode);
                    if (!string.IsNullOrEmpty(PremiumFrequencyCode)) query = query.Where(q => q.PremiumFrequencyCode == PremiumFrequencyCode);
                    if (ReportPeriodMonth.HasValue) query = query.Where(q => q.ReportPeriodMonth == ReportPeriodMonth);
                    if (ReportPeriodYear.HasValue) query = query.Where(q => q.ReportPeriodYear == ReportPeriodYear);
                    if (!string.IsNullOrEmpty(TransactionTypeCode)) query = query.Where(q => q.TransactionTypeCode == TransactionTypeCode);
                    if (!string.IsNullOrEmpty(PolicyNumberFilter)) query = query.Where(q => q.PolicyNumber == PolicyNumberFilter);
                    if (IssueDatePol.HasValue) query = query.Where(q => q.IssueDatePol == IssueDatePol);
                    if (IssueDateBen.HasValue) query = query.Where(q => q.IssueDateBen == IssueDateBen);
                    if (ReinsEffDatePol.HasValue) query = query.Where(q => q.ReinsEffDatePol == ReinsEffDatePol);
                }
            }

            return query;
        }

        public static IQueryable<RiDataWarehouseHistoryBo> GetRiDataWarehouseHistoryQuery(ref ExportBo exportBo, AppDbContext db)
        {
            var objectId = exportBo.ObjectId;
            var paramObject = exportBo.ParameterObject;
            var query = db.RiDataWarehouseHistories.Select(RiDataWarehouseHistoryService.Expression());
            if (paramObject != null)
            {
                string TreatyCode = Util.HasProperty(paramObject, "TreatyCode") ? paramObject.TreatyCode : null;
                string SoaQuarter = Util.HasProperty(paramObject, "SoaQuarter") ? paramObject.SoaQuarter : null;
                string InsuredName = Util.HasProperty(paramObject, "InsuredName") ? paramObject.InsuredName : null;
                string PolicyNumber = Util.HasProperty(paramObject, "PolicyNumber") ? paramObject.PolicyNumber : null;
                string CedingPlanCode = Util.HasProperty(paramObject, "CedingPlanCode") ? paramObject.CedingPlanCode : null;
                string MlreBenefitCode = Util.HasProperty(paramObject, "BenefitCode") ? paramObject.BenefitCode : null;
                string TreatyCodeFilter = Util.HasProperty(paramObject, "TreatyCodeFilter") ? paramObject.TreatyCodeFilter : null;
                string BenefitCodeFilter = Util.HasProperty(paramObject, "BenefitCodeFilter") ? paramObject.BenefitCodeFilter : null;
                string ReinsBasisCode = Util.HasProperty(paramObject, "ReinsBasisCode") ? paramObject.ReinsBasisCode : null;
                string FundsAccountingTypeCode = Util.HasProperty(paramObject, "FundsAccountingTypeCode") ? paramObject.FundsAccountingTypeCode : null;
                string PremiumFrequencyCode = Util.HasProperty(paramObject, "PremiumFrequencyCode") ? paramObject.PremiumFrequencyCode : null;
                string TransactionTypeCode = Util.HasProperty(paramObject, "TransactionTypeCode") ? paramObject.TransactionTypeCode : null;
                string PolicyNumberFilter = Util.HasProperty(paramObject, "PolicyNumberFilter") ? paramObject.PolicyNumberFilter : null;

                string[] treatyCodes = (string.IsNullOrEmpty(TreatyCode)) ? new string[] { } : Util.ToArraySplitTrim(TreatyCode, emptyString: false);
                string[] benefitCodes = Util.ToArraySplitTrim(MlreBenefitCode);

                int i = 0;
                int? CutOffId = null;
                if (Util.HasProperty(paramObject, "CutOffId") && paramObject.CutOffId != null && int.TryParse(paramObject.CutOffId.ToString(), out i))
                {
                    CutOffId = i;
                }

                int? CedantId = null;
                if (Util.HasProperty(paramObject, "CedantId") && paramObject.CedantId != null && int.TryParse(paramObject.CedantId.ToString(), out i))
                {
                    CedantId = i;
                }

                int? ReportPeriodMonth = null;
                if (Util.HasProperty(paramObject, "ReportPeriodMonth") && paramObject.ReportPeriodMonth != null && int.TryParse(paramObject.ReportPeriodMonth.ToString(), out i))
                {
                    ReportPeriodMonth = i;
                }

                int? ReportPeriodYear = null;
                if (Util.HasProperty(paramObject, "ReportPeriodYear") && paramObject.ReportPeriodYear != null && int.TryParse(paramObject.ReportPeriodYear.ToString(), out i))
                {
                    ReportPeriodYear = i;
                }

                DateTime dt = DateTime.Today;
                DateTime? RiskPeriodStartDate = null;
                if (Util.HasProperty(paramObject, "RiskPeriodStartDate") && paramObject.RiskPeriodStartDate != null && DateTime.TryParse(paramObject.RiskPeriodStartDate.ToString(), out dt))
                {
                    RiskPeriodStartDate = dt;
                }

                DateTime? RiskPeriodEndDate = null;
                if (Util.HasProperty(paramObject, "RiskPeriodEndDate") && paramObject.RiskPeriodEndDate != null && DateTime.TryParse(paramObject.RiskPeriodEndDate.ToString(), out dt))
                {
                    RiskPeriodEndDate = dt;
                }

                DateTime? InsuredDateOfBirth = null;
                if (Util.HasProperty(paramObject, "InsuredDateOfBirth") && paramObject.InsuredDateOfBirth != null && DateTime.TryParse(paramObject.InsuredDateOfBirth.ToString(), out dt))
                {
                    InsuredDateOfBirth = dt;
                }

                DateTime? IssueDatePol = null;
                if (Util.HasProperty(paramObject, "IssueDatePol") && paramObject.IssueDatePol != null && DateTime.TryParse(paramObject.IssueDatePol.ToString(), out dt))
                {
                    IssueDatePol = dt;
                }

                DateTime? IssueDateBen = null;
                if (Util.HasProperty(paramObject, "IssueDateBen") && paramObject.IssueDateBen != null && DateTime.TryParse(paramObject.IssueDateBen.ToString(), out dt))
                {
                    IssueDateBen = dt;
                }

                DateTime? ReinsEffDatePol = null;
                if (Util.HasProperty(paramObject, "ReinsEffDatePol") && paramObject.ReinsEffDatePol != null && DateTime.TryParse(paramObject.ReinsEffDatePol.ToString(), out dt))
                {
                    ReinsEffDatePol = dt;
                }

                if (CedantId.HasValue && string.IsNullOrEmpty(TreatyCode))
                {
                    var treatyCodeBos = TreatyCodeService.GetByCedantId(CedantId.Value);
                    treatyCodes = !treatyCodeBos.IsNullOrEmpty() ? treatyCodeBos.Select(q => q.Code).ToArray() : new string[] { };
                }

                bool IsSnapshotVersion = false;
                if (Util.HasProperty(paramObject, "IsSnapshotVersion") && paramObject.IsSnapshotVersion != null)
                {
                    if (!Util.StringToBool(paramObject.IsSnapshotVersion.ToString(), out bool bl))
                    {
                        IsSnapshotVersion = false;
                    }
                    else
                    {
                        IsSnapshotVersion = bl;
                    }
                }

                if (!IsSnapshotVersion &&
                    !CedantId.HasValue &&
                    string.IsNullOrEmpty(TreatyCode) &&
                    string.IsNullOrEmpty(SoaQuarter) &&
                    !RiskPeriodStartDate.HasValue &&
                    !RiskPeriodEndDate.HasValue &&
                    string.IsNullOrEmpty(InsuredName) &&
                    string.IsNullOrEmpty(PolicyNumber) &&
                    !InsuredDateOfBirth.HasValue &&
                    string.IsNullOrEmpty(CedingPlanCode) &&
                    string.IsNullOrEmpty(MlreBenefitCode))
                {
                    query = query.Where(q => q.Id == 0); // Dont get any data
                }
                else
                {
                    if (IsSnapshotVersion)
                    {
                        var cutOffId = CutOffId ?? 0;
                        query = query.Where(q => q.CutOffId == cutOffId);
                    }

                    if (!treatyCodes.IsNullOrEmpty()) query = query.Where(q => treatyCodes.Contains(q.TreatyCode));
                    if (!string.IsNullOrEmpty(SoaQuarter))
                    {
                        string[] quarterStr = SoaQuarter.Split(' ');
                        List<int> months = new List<int> { };

                        switch (quarterStr[1])
                        {
                            case "Q1": months = new List<int> { 1, 2, 3 }; break;
                            case "Q2": months = new List<int> { 4, 5, 6 }; break;
                            case "Q3": months = new List<int> { 7, 8, 9 }; break;
                            case "Q4": months = new List<int> { 10, 11, 12 }; break;
                        }

                        int quarterYear = Convert.ToInt32(quarterStr[0]);
                        query = query.Where(q => months.Contains(q.ReportPeriodMonth.Value) && q.ReportPeriodYear == quarterYear);
                    }
                    if (RiskPeriodStartDate.HasValue) query = query.Where(q => q.RiskPeriodStartDate >= RiskPeriodStartDate);
                    if (RiskPeriodEndDate.HasValue) query = query.Where(q => q.RiskPeriodEndDate <= RiskPeriodEndDate);
                    if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => q.InsuredName == InsuredName);
                    if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber == PolicyNumber);
                    if (InsuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == InsuredDateOfBirth);
                    if (!string.IsNullOrEmpty(CedingPlanCode)) query = query.Where(q => q.CedingPlanCode == CedingPlanCode);
                    if (!string.IsNullOrEmpty(MlreBenefitCode)) query = query.Where(q => benefitCodes.Contains(q.MlreBenefitCode));
                    if (!string.IsNullOrEmpty(TreatyCodeFilter)) query = query.Where(q => q.TreatyCode == TreatyCodeFilter);
                    if (!string.IsNullOrEmpty(BenefitCodeFilter)) query = query.Where(q => q.MlreBenefitCode == BenefitCodeFilter);
                    if (!string.IsNullOrEmpty(ReinsBasisCode)) query = query.Where(q => q.ReinsBasisCode == ReinsBasisCode);
                    if (!string.IsNullOrEmpty(FundsAccountingTypeCode)) query = query.Where(q => q.FundsAccountingTypeCode == FundsAccountingTypeCode);
                    if (!string.IsNullOrEmpty(PremiumFrequencyCode)) query = query.Where(q => q.PremiumFrequencyCode == PremiumFrequencyCode);
                    if (ReportPeriodMonth.HasValue) query = query.Where(q => q.ReportPeriodMonth == ReportPeriodMonth);
                    if (ReportPeriodYear.HasValue) query = query.Where(q => q.ReportPeriodYear == ReportPeriodYear);
                    if (!string.IsNullOrEmpty(TransactionTypeCode)) query = query.Where(q => q.TransactionTypeCode == TransactionTypeCode);
                    if (!string.IsNullOrEmpty(PolicyNumberFilter)) query = query.Where(q => q.PolicyNumber == PolicyNumberFilter);
                    if (IssueDatePol.HasValue) query = query.Where(q => q.IssueDatePol == IssueDatePol);
                    if (IssueDateBen.HasValue) query = query.Where(q => q.IssueDateBen == IssueDateBen);
                    if (ReinsEffDatePol.HasValue) query = query.Where(q => q.ReinsEffDatePol == ReinsEffDatePol);
                }
            }

            return query;
        }

        public static IQueryable<RiDataBo> GetRiDataSearchQuery(ref ExportBo exportBo, AppDbContext db)
        {
            var objectId = exportBo.ObjectId;
            var paramObject = exportBo.ParameterObject;
            var query = db.RiData.Where(q => q.FinaliseStatus == RiDataBo.FinaliseStatusSuccess).Select(RiDataService.Expression());
            if (paramObject != null)
            {
                string TreatyCode = Util.HasProperty(paramObject, "TreatyCode") ? paramObject.TreatyCode : null;
                string SoaQuarter = Util.HasProperty(paramObject, "SoaQuarter") ? paramObject.SoaQuarter : null;
                string InsuredName = Util.HasProperty(paramObject, "InsuredName") ? paramObject.InsuredName : null;
                string PolicyNumber = Util.HasProperty(paramObject, "PolicyNumber") ? paramObject.PolicyNumber : null;
                string CedingPlanCode = Util.HasProperty(paramObject, "CedingPlanCode") ? paramObject.CedingPlanCode : null;
                string BenefitCode = Util.HasProperty(paramObject, "BenefitCode") ? paramObject.BenefitCode : null;
                string TreatyCodeFilter = Util.HasProperty(paramObject, "TreatyCodeFilter") ? paramObject.TreatyCodeFilter : null;
                string ReinsBasisCode = Util.HasProperty(paramObject, "ReinsBasisCode") ? paramObject.ReinsBasisCode : null;
                string FundsAccountingTypeCode = Util.HasProperty(paramObject, "FundsAccountingTypeCode") ? paramObject.FundsAccountingTypeCode : null;
                string PremiumFrequencyCode = Util.HasProperty(paramObject, "PremiumFrequencyCode") ? paramObject.PremiumFrequencyCode : null;
                string TransactionTypeCode = Util.HasProperty(paramObject, "TransactionTypeCode") ? paramObject.TransactionTypeCode : null;
                string PolicyNumberFilter = Util.HasProperty(paramObject, "PolicyNumberFilter") ? paramObject.PolicyNumberFilter : null;
                string CedingPlanCodeFilter = Util.HasProperty(paramObject, "CedingPlanCodeFilter") ? paramObject.CedingPlanCodeFilter : null;
                string CedingBenefitTypeCode = Util.HasProperty(paramObject, "CedingBenefitTypeCode") ? paramObject.CedingBenefitTypeCode : null;
                string CedingBenefitRiskCode = Util.HasProperty(paramObject, "CedingBenefitRiskCode") ? paramObject.CedingBenefitRiskCode : null;
                string BenefitCodeFilter = Util.HasProperty(paramObject, "BenefitCodeFilter") ? paramObject.BenefitCodeFilter : null;
                string InsuredNameFilter = Util.HasProperty(paramObject, "InsuredNameFilter") ? paramObject.InsuredNameFilter : null;
                string InsuredGenderCode = Util.HasProperty(paramObject, "InsuredGenderCode") ? paramObject.InsuredGenderCode : null;
                string InsuredTobaccoUse = Util.HasProperty(paramObject, "InsuredTobaccoUse") ? paramObject.InsuredTobaccoUse : null;
                string InsuredOccupationCode = Util.HasProperty(paramObject, "InsuredOccupationCode") ? paramObject.InsuredOccupationCode : null;

                string[] treatyCodes = Util.ToArraySplitTrim(TreatyCode);
                string[] benefitCodes = Util.ToArraySplitTrim(BenefitCode);

                int i = 0;
                int? ReportPeriodMonth = null;
                if (Util.HasProperty(paramObject, "ReportPeriodMonth") && paramObject.ReportPeriodMonth != null && int.TryParse(paramObject.ReportPeriodMonth.ToString(), out i))
                {
                    ReportPeriodMonth = i;
                }

                int? ReportPeriodYear = null;
                if (Util.HasProperty(paramObject, "ReportPeriodYear") && paramObject.ReportPeriodYear != null && int.TryParse(paramObject.ReportPeriodYear.ToString(), out i))
                {
                    ReportPeriodYear = i;
                }
                int? RiskPeriodMonth = null;
                if (Util.HasProperty(paramObject, "RiskPeriodMonth") && paramObject.RiskPeriodMonth != null && int.TryParse(paramObject.RiskPeriodMonth.ToString(), out i))
                {
                    RiskPeriodMonth = i;
                }

                int? RiskPeriodYear = null;
                if (Util.HasProperty(paramObject, "RiskPeriodYear") && paramObject.RiskPeriodYear != null && int.TryParse(paramObject.RiskPeriodYear.ToString(), out i))
                {
                    RiskPeriodYear = i;
                }

                int? InsuredAttainedAge = null;
                if (Util.HasProperty(paramObject, "InsuredAttainedAge") && paramObject.InsuredAttainedAge != null && int.TryParse(paramObject.InsuredAttainedAge.ToString(), out i))
                {
                    InsuredAttainedAge = i;
                }

                DateTime dt = DateTime.Today;
                DateTime? RiskPeriodStartDate = null;
                if (Util.HasProperty(paramObject, "RiskPeriodStartDate") && paramObject.RiskPeriodStartDate != null && DateTime.TryParse(paramObject.RiskPeriodStartDate.ToString(), out dt))
                {
                    RiskPeriodStartDate = dt;
                }

                DateTime? RiskPeriodEndDate = null;
                if (Util.HasProperty(paramObject, "RiskPeriodEndDate") && paramObject.RiskPeriodEndDate != null && DateTime.TryParse(paramObject.RiskPeriodEndDate.ToString(), out dt))
                {
                    RiskPeriodEndDate = dt;
                }

                DateTime? InsuredDateOfBirth = null;
                if (Util.HasProperty(paramObject, "InsuredDateOfBirth") && paramObject.InsuredDateOfBirth != null && DateTime.TryParse(paramObject.InsuredDateOfBirth.ToString(), out dt))
                {
                    InsuredDateOfBirth = dt;
                }

                DateTime? InsuredDateOfBirthFilter = null;
                if (Util.HasProperty(paramObject, "InsuredDateOfBirthFilter") && paramObject.InsuredDateOfBirthFilter != null && DateTime.TryParse(paramObject.InsuredDateOfBirthFilter.ToString(), out dt))
                {
                    InsuredDateOfBirthFilter = dt;
                }

                DateTime? IssueDatePol = null;
                if (Util.HasProperty(paramObject, "IssueDatePol") && paramObject.IssueDatePol != null && DateTime.TryParse(paramObject.IssueDatePol.ToString(), out dt))
                {
                    IssueDatePol = dt;
                }

                DateTime? IssueDateBen = null;
                if (Util.HasProperty(paramObject, "IssueDateBen") && paramObject.IssueDateBen != null && DateTime.TryParse(paramObject.IssueDateBen.ToString(), out dt))
                {
                    IssueDateBen = dt;
                }

                DateTime? ReinsEffDatePol = null;
                if (Util.HasProperty(paramObject, "ReinsEffDatePol") && paramObject.ReinsEffDatePol != null && DateTime.TryParse(paramObject.ReinsEffDatePol.ToString(), out dt))
                {
                    ReinsEffDatePol = dt;
                }

                DateTime? ReinsEffDateBen = null;
                if (Util.HasProperty(paramObject, "ReinsEffDateBen") && paramObject.ReinsEffDateBen != null && DateTime.TryParse(paramObject.ReinsEffDateBen.ToString(), out dt))
                {
                    ReinsEffDateBen = dt;
                }

                double d = 0;
                double? OriSumAssured = null;
                if (Util.HasProperty(paramObject, "OriSumAssured") && paramObject.OriSumAssured != null && double.TryParse(paramObject.OriSumAssured.ToString(), out d))
                {
                    OriSumAssured = d;
                }

                double? CurrSumAssured = null;
                if (Util.HasProperty(paramObject, "CurrSumAssured") && paramObject.CurrSumAssured != null && double.TryParse(paramObject.CurrSumAssured.ToString(), out d))
                {
                    CurrSumAssured = d;
                }

                double? AmountCededB4MlreShare = null;
                if (Util.HasProperty(paramObject, "AmountCededB4MlreShare") && paramObject.AmountCededB4MlreShare != null && double.TryParse(paramObject.AmountCededB4MlreShare.ToString(), out d))
                {
                    AmountCededB4MlreShare = d;
                }

                double? RetentionAmount = null;
                if (Util.HasProperty(paramObject, "RetentionAmount") && paramObject.RetentionAmount != null && double.TryParse(paramObject.RetentionAmount.ToString(), out d))
                {
                    RetentionAmount = d;
                }

                double? AarOri = null;
                if (Util.HasProperty(paramObject, "AarOri") && paramObject.AarOri != null && double.TryParse(paramObject.AarOri.ToString(), out d))
                {
                    AarOri = d;
                }

                double? Aar = null;
                if (Util.HasProperty(paramObject, "Aar") && paramObject.Aar != null && double.TryParse(paramObject.Aar.ToString(), out d))
                {
                    Aar = d;
                }


                if (string.IsNullOrEmpty(TreatyCode) &&
                string.IsNullOrEmpty(SoaQuarter) &&
                !RiskPeriodStartDate.HasValue &&
                !RiskPeriodEndDate.HasValue &&
                string.IsNullOrEmpty(InsuredName) &&
                string.IsNullOrEmpty(PolicyNumber) &&
                !InsuredDateOfBirth.HasValue &&
                string.IsNullOrEmpty(CedingPlanCode) &&
                string.IsNullOrEmpty(BenefitCode))
                {
                    query = query.Where(q => q.Id == 0); // Dont get any data
                }
                else
                {
                    if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => treatyCodes.Contains(q.TreatyCode));
                    if (!string.IsNullOrEmpty(SoaQuarter))
                    {
                        string[] quarterStr = SoaQuarter.Split(' ');
                        List<int> months = new List<int> { };

                        switch (quarterStr[1])
                        {
                            case "Q1": months = new List<int> { 1, 2, 3 }; break;
                            case "Q2": months = new List<int> { 4, 5, 6 }; break;
                            case "Q3": months = new List<int> { 7, 8, 9 }; break;
                            case "Q4": months = new List<int> { 10, 11, 12 }; break;
                        }

                        int quarterYear = Convert.ToInt32(quarterStr[0]);
                        query = query.Where(q => months.Contains(q.ReportPeriodMonth.Value) && q.ReportPeriodYear == quarterYear);
                    }
                    if (RiskPeriodStartDate.HasValue) query = query.Where(q => q.RiskPeriodStartDate >= RiskPeriodStartDate);
                    if (RiskPeriodEndDate.HasValue) query = query.Where(q => q.RiskPeriodEndDate <= RiskPeriodEndDate);
                    if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => q.InsuredName == InsuredName);
                    if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber == PolicyNumber);
                    if (InsuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == InsuredDateOfBirth);
                    if (!string.IsNullOrEmpty(CedingPlanCode)) query = query.Where(q => q.CedingPlanCode == CedingPlanCode);
                    if (!string.IsNullOrEmpty(BenefitCode)) query = query.Where(q => benefitCodes.Contains(q.MlreBenefitCode));
                    if (!string.IsNullOrEmpty(TreatyCodeFilter)) query = query.Where(q => q.TreatyCode == TreatyCodeFilter);
                    if (!string.IsNullOrEmpty(ReinsBasisCode)) query = query.Where(q => q.ReinsBasisCode == ReinsBasisCode);
                    if (!string.IsNullOrEmpty(FundsAccountingTypeCode)) query = query.Where(q => q.FundsAccountingTypeCode == FundsAccountingTypeCode);
                    if (!string.IsNullOrEmpty(PremiumFrequencyCode)) query = query.Where(q => q.PremiumFrequencyCode == PremiumFrequencyCode);
                    if (ReportPeriodMonth.HasValue) query = query.Where(q => q.ReportPeriodMonth == ReportPeriodMonth);
                    if (ReportPeriodYear.HasValue) query = query.Where(q => q.ReportPeriodYear == ReportPeriodYear);
                    if (RiskPeriodMonth.HasValue) query = query.Where(q => q.RiskPeriodMonth == RiskPeriodMonth);
                    if (RiskPeriodYear.HasValue) query = query.Where(q => q.RiskPeriodYear == RiskPeriodYear);
                    if (!string.IsNullOrEmpty(TransactionTypeCode)) query = query.Where(q => q.TransactionTypeCode == TransactionTypeCode);
                    if (!string.IsNullOrEmpty(PolicyNumberFilter)) query = query.Where(q => q.PolicyNumber == PolicyNumberFilter);
                    if (IssueDatePol.HasValue) query = query.Where(q => q.IssueDatePol == IssueDatePol);
                    if (IssueDateBen.HasValue) query = query.Where(q => q.IssueDateBen == IssueDateBen);
                    if (ReinsEffDatePol.HasValue) query = query.Where(q => q.ReinsEffDatePol == ReinsEffDatePol);
                    if (ReinsEffDateBen.HasValue) query = query.Where(q => q.ReinsEffDateBen == ReinsEffDateBen);
                    if (!string.IsNullOrEmpty(CedingPlanCodeFilter)) query = query.Where(q => q.CedingPlanCode == CedingPlanCodeFilter);
                    if (!string.IsNullOrEmpty(CedingBenefitTypeCode)) query = query.Where(q => q.CedingBenefitTypeCode == CedingBenefitTypeCode);
                    if (!string.IsNullOrEmpty(CedingBenefitRiskCode)) query = query.Where(q => q.CedingBenefitRiskCode == CedingBenefitRiskCode);
                    if (!string.IsNullOrEmpty(BenefitCodeFilter)) query = query.Where(q => q.MlreBenefitCode == BenefitCodeFilter);
                    if (OriSumAssured.HasValue) query = query.Where(q => q.OriSumAssured == OriSumAssured);
                    if (CurrSumAssured.HasValue) query = query.Where(q => q.CurrSumAssured == CurrSumAssured);
                    if (AmountCededB4MlreShare.HasValue) query = query.Where(q => q.AmountCededB4MlreShare == AmountCededB4MlreShare);
                    if (RetentionAmount.HasValue) query = query.Where(q => q.RetentionAmount == RetentionAmount);
                    if (AarOri.HasValue) query = query.Where(q => q.AarOri == AarOri);
                    if (Aar.HasValue) query = query.Where(q => q.Aar == Aar);
                    if (!string.IsNullOrEmpty(InsuredNameFilter)) query = query.Where(q => q.InsuredName == InsuredNameFilter);
                    if (!string.IsNullOrEmpty(InsuredGenderCode)) query = query.Where(q => q.InsuredGenderCode == InsuredGenderCode);
                    if (!string.IsNullOrEmpty(InsuredTobaccoUse)) query = query.Where(q => q.InsuredTobaccoUse == InsuredTobaccoUse);
                    if (InsuredDateOfBirthFilter.HasValue) query = query.Where(q => q.InsuredDateOfBirth == InsuredDateOfBirthFilter);
                    if (!string.IsNullOrEmpty(InsuredOccupationCode)) query = query.Where(q => q.InsuredOccupationCode == InsuredOccupationCode);
                    if (InsuredAttainedAge.HasValue) query = query.Where(q => q.InsuredAttainedAge == InsuredAttainedAge);
                }
            }

            return query;
        }

        public static IQueryable<ClaimRegisterBo> GetClaimRegisterSearchQuery(ref ExportBo exportBo, AppDbContext db)
        {
            var objectId = exportBo.ObjectId;
            var paramObject = exportBo.ParameterObject;
            var query = db.ClaimRegister.Select(ClaimRegisterService.Expression());
            if (paramObject != null)
            {
                string ClaimId = Util.HasProperty(paramObject, "ClaimId") ? paramObject.ClaimId : null;
                string InsuredName = Util.HasProperty(paramObject, "InsuredName") ? paramObject.InsuredName : null;
                string PolicyNumber = Util.HasProperty(paramObject, "PolicyNumber") ? paramObject.PolicyNumber : null;
                string TreatyCode = Util.HasProperty(paramObject, "TreatyCode") ? paramObject.TreatyCode : null;
                string EntryNo = Util.HasProperty(paramObject, "EntryNo") ? paramObject.EntryNo : null;
                string SoaQuarter = Util.HasProperty(paramObject, "SoaQuarter") ? paramObject.SoaQuarter : null;
                string ClaimIdFilter = Util.HasProperty(paramObject, "ClaimIdFilter") ? paramObject.ClaimIdFilter : null;
                string ClaimTransactionType = Util.HasProperty(paramObject, "ClaimTransactionType") ? paramObject.ClaimTransactionType : null;
                string RecordType = Util.HasProperty(paramObject, "RecordType") ? paramObject.RecordType : null;
                string TreatyCodeFilter = Util.HasProperty(paramObject, "TreatyCodeFilter") ? paramObject.TreatyCodeFilter : null;
                string PolicyNumberFilter = Util.HasProperty(paramObject, "PolicyNumberFilter") ? paramObject.PolicyNumberFilter : null;
                string CedingCompany = Util.HasProperty(paramObject, "CedingCompany") ? paramObject.CedingCompany : null;

                string[] treatyCodes = Util.ToArraySplitTrim(TreatyCode);

                int i = 0;
                int? RiDataWarehouseId = null;
                if (Util.HasProperty(paramObject, "RiDataWarehouseId") && paramObject.RiDataWarehouseId != null && int.TryParse(paramObject.RiDataWarehouseId.ToString(), out i))
                {
                    RiDataWarehouseId = i;
                }

                double d = 0;
                double? ClaimRecoveryAmount = null;
                if (Util.HasProperty(paramObject, "ClaimRecoveryAmount") && paramObject.ClaimRecoveryAmount != null && double.TryParse(paramObject.ClaimRecoveryAmount.ToString(), out d))
                {
                    ClaimRecoveryAmount = d;
                }

                DateTime dt = DateTime.Today;
                DateTime? InsuredDateOfBirth = null;
                if (Util.HasProperty(paramObject, "InsuredDateOfBirth") && paramObject.InsuredDateOfBirth != null && DateTime.TryParse(paramObject.InsuredDateOfBirth.ToString(), out dt))
                {
                    InsuredDateOfBirth = dt;
                }

                DateTime? ProvisionAt = null;
                if (Util.HasProperty(paramObject, "ProvisionAt") && paramObject.ProvisionAt != null && DateTime.TryParse(paramObject.ProvisionAt.ToString(), out dt))
                {
                    ProvisionAt = dt;
                }

                bool? HasRedFlag = null;
                if (Util.HasProperty(paramObject, "HasRedFlag") && paramObject.HasRedFlag != null)
                {
                    if (!Util.StringToBool(paramObject.HasRedFlag.ToString(), out bool bl))
                    {
                        HasRedFlag = null;
                    }
                    else
                    {
                        HasRedFlag = bl;
                    }
                }

                bool? IsReferralCase = null;
                if (Util.HasProperty(paramObject, "IsReferralCase") && paramObject.IsReferralCase != null)
                {
                    if (!Util.StringToBool(paramObject.IsReferralCase.ToString(), out bool bl))
                    {
                        IsReferralCase = null;
                    }
                    else
                    {
                        IsReferralCase = bl;
                    }
                }

                bool? IsWithAdjustmentDetail = null;
                if (Util.HasProperty(paramObject, "IsWithAdjustmentDetail") && paramObject.IsWithAdjustmentDetail != null)
                {
                    if (!Util.StringToBool(paramObject.IsWithAdjustmentDetail.ToString(), out bool bl))
                    {
                        IsWithAdjustmentDetail = null;
                    }
                    else
                    {
                        IsWithAdjustmentDetail = bl;
                    }
                }

                exportBo.TempFlag = IsWithAdjustmentDetail.HasValue ? IsWithAdjustmentDetail.Value : false;
                if (exportBo.TempFlag)
                {
                    query = db.FinanceProvisioningTransactions.Select(FinanceProvisioningTransactionService.ClaimRegisterExpression());
                }

                query = query.Where(q => q.ClaimTransactionType != PickListDetailBo.ClaimTransactionTypeBulk);
                if (!string.IsNullOrEmpty(ClaimId)) query = query.Where(q => q.ClaimId == ClaimId);
                if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => q.InsuredName == InsuredName);
                if (InsuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == InsuredDateOfBirth);
                if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber == PolicyNumber);
                if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => treatyCodes.Contains(q.TreatyCode));
                if (HasRedFlag.HasValue) query = query.Where(q => q.HasRedFlag == HasRedFlag);
                if (!string.IsNullOrEmpty(EntryNo)) query = query.Where(q => q.EntryNo == EntryNo);
                if (!string.IsNullOrEmpty(SoaQuarter)) query = query.Where(q => q.SoaQuarter == SoaQuarter);
                if (!string.IsNullOrEmpty(ClaimIdFilter)) query = query.Where(q => q.ClaimId == ClaimIdFilter);
                if (!string.IsNullOrEmpty(ClaimTransactionType)) query = query.Where(q => q.ClaimTransactionType == ClaimTransactionType);
                if (ClaimRecoveryAmount.HasValue) query = query.Where(q => q.ClaimRecoveryAmt == ClaimRecoveryAmount);
                if (IsReferralCase.HasValue) query = query.Where(q => q.IsReferralCase == IsReferralCase);
                if (RiDataWarehouseId.HasValue) query = query.Where(q => q.RiDataWarehouseId == RiDataWarehouseId);
                if (!string.IsNullOrEmpty(RecordType)) query = query.Where(q => q.RecordType == RecordType);
                if (!string.IsNullOrEmpty(TreatyCodeFilter)) query = query.Where(q => q.TreatyCode == TreatyCodeFilter);
                if (!string.IsNullOrEmpty(PolicyNumberFilter)) query = query.Where(q => q.PolicyNumber == PolicyNumberFilter);
                if (!string.IsNullOrEmpty(CedingCompany)) query = query.Where(q => q.CedingCompany == CedingCompany);
                if (ProvisionAt.HasValue) query = query.Where(q => DbFunctions.TruncateTime(q.ProvisionAt) == DbFunctions.TruncateTime(ProvisionAt));
            }

            return query;
        }

        public static IQueryable<ClaimRegisterHistoryBo> GetClaimRegisterHistorySearchQuery(ref ExportBo exportBo, AppDbContext db)
        {
            var objectId = exportBo.ObjectId;
            var paramObject = exportBo.ParameterObject;
            var query = db.ClaimRegisterHistories.Select(ClaimRegisterHistoryService.Expression());
            if (paramObject != null)
            {
                string ClaimId = Util.HasProperty(paramObject, "ClaimId") ? paramObject.ClaimId : null;
                string InsuredName = Util.HasProperty(paramObject, "InsuredName") ? paramObject.InsuredName : null;
                string PolicyNumber = Util.HasProperty(paramObject, "PolicyNumber") ? paramObject.PolicyNumber : null;
                string TreatyCode = Util.HasProperty(paramObject, "TreatyCode") ? paramObject.TreatyCode : null;
                string EntryNo = Util.HasProperty(paramObject, "EntryNo") ? paramObject.EntryNo : null;
                string SoaQuarter = Util.HasProperty(paramObject, "SoaQuarter") ? paramObject.SoaQuarter : null;
                string ClaimIdFilter = Util.HasProperty(paramObject, "ClaimIdFilter") ? paramObject.ClaimIdFilter : null;
                string ClaimTransactionType = Util.HasProperty(paramObject, "ClaimTransactionType") ? paramObject.ClaimTransactionType : null;
                string RecordType = Util.HasProperty(paramObject, "RecordType") ? paramObject.RecordType : null;
                string TreatyCodeFilter = Util.HasProperty(paramObject, "TreatyCodeFilter") ? paramObject.TreatyCodeFilter : null;
                string PolicyNumberFilter = Util.HasProperty(paramObject, "PolicyNumberFilter") ? paramObject.PolicyNumberFilter : null;
                string CedingCompany = Util.HasProperty(paramObject, "CedingCompany") ? paramObject.CedingCompany : null;

                string[] treatyCodes = Util.ToArraySplitTrim(TreatyCode);

                int i = 0;
                int? RiDataWarehouseId = null;
                if (Util.HasProperty(paramObject, "RiDataWarehouseId") && paramObject.RiDataWarehouseId != null && int.TryParse(paramObject.RiDataWarehouseId.ToString(), out i))
                {
                    RiDataWarehouseId = i;
                }

                int? CutOffId = null;
                if (Util.HasProperty(paramObject, "CutOffId") && paramObject.CutOffId != null && int.TryParse(paramObject.CutOffId.ToString(), out i))
                {
                    CutOffId = i;
                }

                double d = 0;
                double? ClaimRecoveryAmount = null;
                if (Util.HasProperty(paramObject, "ClaimRecoveryAmount") && paramObject.ClaimRecoveryAmount != null && double.TryParse(paramObject.ClaimRecoveryAmount.ToString(), out d))
                {
                    ClaimRecoveryAmount = d;
                }

                DateTime dt = DateTime.Today;
                DateTime? InsuredDateOfBirth = null;
                if (Util.HasProperty(paramObject, "InsuredDateOfBirth") && paramObject.InsuredDateOfBirth != null && DateTime.TryParse(paramObject.InsuredDateOfBirth.ToString(), out dt))
                {
                    InsuredDateOfBirth = dt;
                }

                DateTime? ProvisionAt = null;
                if (Util.HasProperty(paramObject, "ProvisionAt") && paramObject.ProvisionAt != null && DateTime.TryParse(paramObject.ProvisionAt.ToString(), out dt))
                {
                    ProvisionAt = dt;
                }

                bool? HasRedFlag = null;
                if (Util.HasProperty(paramObject, "HasRedFlag") && paramObject.HasRedFlag != null)
                {
                    if (!Util.StringToBool(paramObject.HasRedFlag.ToString(), out bool bl))
                    {
                        HasRedFlag = null;
                    }
                    else
                    {
                        HasRedFlag = bl;
                    }
                }

                bool? IsSnapshotVersion = null;
                if (Util.HasProperty(paramObject, "IsSnapshotVersion") && paramObject.IsSnapshotVersion != null)
                {
                    if (!Util.StringToBool(paramObject.IsSnapshotVersion.ToString(), out bool bl))
                    {
                        IsSnapshotVersion = null;
                    }
                    else
                    {
                        IsSnapshotVersion = bl;
                    }
                }

                bool? IsReferralCase = null;
                if (Util.HasProperty(paramObject, "IsReferralCase") && paramObject.IsReferralCase != null)
                {
                    if (!Util.StringToBool(paramObject.IsReferralCase.ToString(), out bool bl))
                    {
                        IsReferralCase = null;
                    }
                    else
                    {
                        IsReferralCase = bl;
                    }
                }

                bool? IsWithAdjustmentDetail = null;
                if (Util.HasProperty(paramObject, "IsWithAdjustmentDetail") && paramObject.IsWithAdjustmentDetail != null)
                {
                    if (!Util.StringToBool(paramObject.IsWithAdjustmentDetail.ToString(), out bool bl))
                    {
                        IsWithAdjustmentDetail = null;
                    }
                    else
                    {
                        IsWithAdjustmentDetail = bl;
                    }
                }

                exportBo.TempFlag = IsWithAdjustmentDetail.HasValue ? IsWithAdjustmentDetail.Value : false;
                if (exportBo.TempFlag)
                {
                    query = db.FinanceProvisioningTransactions.Join(db.ClaimRegisterHistories, t => t.ClaimRegisterId, h => h.ClaimRegisterId, (t, h) => new ClaimRegisterHistoryTransaction { Transaction = t, History = h })
                        .Select(FinanceProvisioningTransactionService.ClaimRegisterHistoryExpression());
                }

                if (IsSnapshotVersion.HasValue && IsSnapshotVersion.Value)
                {
                    var cutOffId = CutOffId ?? 0;
                    query = query.Where(q => q.CutOffId == cutOffId);

                    if (exportBo.TempFlag)
                    {
                        query = query.Where(q => DbFunctions.TruncateTime(q.ProvisionAt) <= DbFunctions.TruncateTime(q.CutOffAt)).Where(q => q.FinanceProvisioningStatus != FinanceProvisioningBo.StatusPending);
                    }
                }
                query = query.Where(q => q.ClaimTransactionType != PickListDetailBo.ClaimTransactionTypeBulk);
                if (!string.IsNullOrEmpty(ClaimId)) query = query.Where(q => q.ClaimId == ClaimId);
                if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => q.InsuredName == InsuredName);
                if (InsuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == InsuredDateOfBirth);
                if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber == PolicyNumber);
                if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => treatyCodes.Contains(q.TreatyCode));
                if (HasRedFlag.HasValue) query = query.Where(q => q.HasRedFlag == HasRedFlag);
                if (!string.IsNullOrEmpty(EntryNo)) query = query.Where(q => q.EntryNo == EntryNo);
                if (!string.IsNullOrEmpty(SoaQuarter)) query = query.Where(q => q.SoaQuarter == SoaQuarter);
                if (!string.IsNullOrEmpty(ClaimIdFilter)) query = query.Where(q => q.ClaimId == ClaimIdFilter);
                if (!string.IsNullOrEmpty(ClaimTransactionType)) query = query.Where(q => q.ClaimTransactionType == ClaimTransactionType);
                if (ClaimRecoveryAmount.HasValue) query = query.Where(q => q.ClaimRecoveryAmt == ClaimRecoveryAmount);
                if (IsReferralCase.HasValue) query = query.Where(q => q.IsReferralCase == IsReferralCase);
                if (RiDataWarehouseId.HasValue) query = query.Where(q => q.RiDataWarehouseId == RiDataWarehouseId);
                if (!string.IsNullOrEmpty(RecordType)) query = query.Where(q => q.RecordType == RecordType);
                if (!string.IsNullOrEmpty(TreatyCodeFilter)) query = query.Where(q => q.TreatyCode == TreatyCodeFilter);
                if (!string.IsNullOrEmpty(PolicyNumberFilter)) query = query.Where(q => q.PolicyNumber == PolicyNumberFilter);
                if (!string.IsNullOrEmpty(CedingCompany)) query = query.Where(q => q.CedingCompany == CedingCompany);
                if (ProvisionAt.HasValue) query = query.Where(q => DbFunctions.TruncateTime(q.ProvisionAt) == DbFunctions.TruncateTime(ProvisionAt));
            }

            return query;
        }

        public static IQueryable<ReferralClaimBo> GetReferralClaimQuery(ref ExportBo exportBo, AppDbContext db)
        {
            var paramObject = exportBo.ParameterObject;
            var query = db.ReferralClaims.Select(ReferralClaimService.Expression());
            if (paramObject != null)
            {
                string ReferralId = Util.HasProperty(paramObject, "ReferralId") ? paramObject.ReferralId : null;
                string PolicyNumber = Util.HasProperty(paramObject, "PolicyNumber") ? paramObject.PolicyNumber : null;
                string TreatyCode = Util.HasProperty(paramObject, "TreatyCode") ? paramObject.TreatyCode : null;
                string RecordType = Util.HasProperty(paramObject, "RecordType") ? paramObject.RecordType : null;
                string InsuredName = Util.HasProperty(paramObject, "InsuredName") ? paramObject.InsuredName : null;
                string CedingCompany = Util.HasProperty(paramObject, "CedingCompany") ? paramObject.CedingCompany : null;

                int i = 0;
                int? TurnAroundTime = null;
                if (Util.HasProperty(paramObject, "TurnAroundTime") && paramObject.TurnAroundTime != null && int.TryParse(paramObject.TurnAroundTime.ToString(), out i))
                {
                    TurnAroundTime = i;
                }

                int? PersonInChargeId = null;
                if (Util.HasProperty(paramObject, "PersonInChargeId") && paramObject.PersonInChargeId != null && int.TryParse(paramObject.PersonInChargeId.ToString(), out i))
                {
                    PersonInChargeId = i;
                }

                int? Status = null;
                if (Util.HasProperty(paramObject, "Status") && paramObject.Status != null && int.TryParse(paramObject.Status.ToString(), out i))
                {
                    Status = i;
                }

                DateTime dt = DateTime.Today;
                DateTime? ReceivedAt = null;
                if (Util.HasProperty(paramObject, "ReceivedAt") && paramObject.ReceivedAt != null && DateTime.TryParse(paramObject.ReceivedAt.ToString(), out dt))
                {
                    ReceivedAt = dt;
                }

                DateTime? RespondedAt = null;
                if (Util.HasProperty(paramObject, "RespondedAt") && paramObject.RespondedAt != null && DateTime.TryParse(paramObject.RespondedAt.ToString(), out dt))
                {
                    RespondedAt = dt;
                }

                DateTime? DateReceivedFullDocuments = null;
                if (Util.HasProperty(paramObject, "DateReceivedFullDocuments") && paramObject.DateReceivedFullDocuments != null && DateTime.TryParse(paramObject.DateReceivedFullDocuments.ToString(), out dt))
                {
                    DateReceivedFullDocuments = dt;
                }

                DateTime? DateOfCommencement = null;
                if (Util.HasProperty(paramObject, "DateOfCommencement") && paramObject.DateOfCommencement != null && DateTime.TryParse(paramObject.DateOfCommencement.ToString(), out dt))
                {
                    DateOfCommencement = dt;
                }

                DateTime? DateOfEvent = null;
                if (Util.HasProperty(paramObject, "DateOfEvent") && paramObject.DateOfEvent != null && DateTime.TryParse(paramObject.DateOfEvent.ToString(), out dt))
                {
                    DateOfEvent = dt;
                }

                double d = 0;
                double? ClaimRecoveryAmount = null;
                if (Util.HasProperty(paramObject, "ClaimRecoveryAmount") && paramObject.ClaimRecoveryAmount != null && double.TryParse(paramObject.ClaimRecoveryAmountClaimRecoveryAmountToString(), out d))
                {
                    ClaimRecoveryAmount = d;
                }

                if (!string.IsNullOrEmpty(ReferralId)) query = query.Where(q => q.ReferralId.Contains(ReferralId));
                if (TurnAroundTime.HasValue)
                {
                    long ticks;
                    switch (TurnAroundTime)
                    {
                        case ReferralClaimBo.FilterTat1Day:
                            ticks = (new TimeSpan(1, 0, 0, 0)).Ticks;
                            query = query.Where(q => q.TurnAroundTime <= ticks);
                            break;
                        case ReferralClaimBo.FilterTat2Day:
                            ticks = (new TimeSpan(2, 0, 0, 0)).Ticks;
                            query = query.Where(q => q.TurnAroundTime <= ticks);
                            break;
                        case ReferralClaimBo.FilterTatMoreThan2Day:
                            ticks = (new TimeSpan(2, 0, 0, 0)).Ticks;
                            query = query.Where(q => q.TurnAroundTime > ticks);
                            break;
                        default:
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => q.InsuredName.Contains(InsuredName));
                if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber.Contains(PolicyNumber));
                if (ReceivedAt.HasValue) query = query.Where(q => q.ReceivedAt == ReceivedAt);
                if (RespondedAt.HasValue) query = query.Where(q => q.RespondedAt == RespondedAt);
                if (DateReceivedFullDocuments.HasValue) query = query.Where(q => q.DateReceivedFullDocuments == DateReceivedFullDocuments);
                if (DateOfCommencement.HasValue) query = query.Where(q => q.DateOfCommencement == DateOfCommencement);
                if (DateOfEvent.HasValue) query = query.Where(q => q.DateOfCommencement == DateOfEvent);
                if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => q.TreatyCode == TreatyCode);
                if (!string.IsNullOrEmpty(RecordType)) query = query.Where(q => q.RecordType == RecordType);
                if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => q.InsuredName.Contains(InsuredName));
                if (!string.IsNullOrEmpty(CedingCompany)) query = query.Where(q => q.CedingCompany == CedingCompany);
                if (ClaimRecoveryAmount.HasValue) query = query.Where(q => q.ClaimRecoveryAmount == ClaimRecoveryAmount);
                if (PersonInChargeId.HasValue) query = query.Where(q => q.PersonInChargeId == PersonInChargeId);
                if (Status.HasValue) query = query.Where(q => q.Status == Status);
            }

            return query;
        }

        public static IQueryable<InvoiceRegisterBo> GetInvoiceRegisterQuery(ref ExportBo exportBo, AppDbContext db)
        {
            var paramObject = exportBo.ParameterObject;
            var query = db.InvoiceRegisters.Select(InvoiceRegisterService.Expression())
                .Where(q => q.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                .Where(q => q.ReportingType == InvoiceRegisterBo.ReportingTypeIFRS4);

            if (paramObject != null)
            {
                string InvoiceNumber = Util.HasProperty(paramObject, "InvoiceNumber") ? paramObject.InvoiceNumber : null;
                string RiskQuarter = Util.HasProperty(paramObject, "RiskQuarter") ? paramObject.RiskQuarter : null;
                string AccountFor = Util.HasProperty(paramObject, "AccountFor") ? paramObject.AccountFor : null;

                int r = 0;
                int? TreatyCodeId = null;
                if (Util.HasProperty(paramObject, "TreatyCodeId") && paramObject.TreatyCodeId != null && int.TryParse(paramObject.TreatyCodeId.ToString(), out r))
                {
                    TreatyCodeId = r;
                }

                int? CedantId = null;
                if (Util.HasProperty(paramObject, "CedantId") && paramObject.CedantId != null && int.TryParse(paramObject.CedantId.ToString(), out r))
                {
                    CedantId = r;
                }

                int? PreparedById = null;
                if (Util.HasProperty(paramObject, "PreparedById") && paramObject.PreparedById != null && int.TryParse(paramObject.PreparedById.ToString(), out r))
                {
                    PreparedById = r;
                }

                int? InvoiceType = null;
                if (Util.HasProperty(paramObject, "InvoiceType") && paramObject.InvoiceType != null && int.TryParse(paramObject.InvoiceType.ToString(), out r))
                {
                    InvoiceType = r;
                }

                int? Status = null;
                if (Util.HasProperty(paramObject, "Status") && paramObject.Status != null && int.TryParse(paramObject.Status.ToString(), out r))
                {
                    Status = r;
                }

                DateTime dt = DateTime.Today;
                DateTime? InvoiceAt = null;
                if (Util.HasProperty(paramObject, "InvoiceDate") && paramObject.InvoiceDate != null && DateTime.TryParse(paramObject.InvoiceDate.ToString(), out dt))
                {
                    InvoiceAt = dt;
                }

                DateTime? StatementReceivedAt = null;
                if (Util.HasProperty(paramObject, "SttReceivedDate") && paramObject.SttReceivedDate != null && DateTime.TryParse(paramObject.SttReceivedDate.ToString(), out dt))
                {
                    StatementReceivedAt = dt;
                }

                double d = 0;
                double? TotalPaid = null;
                if (Util.HasProperty(paramObject, "TotalPaid") && paramObject.TotalPaid != null && double.TryParse(paramObject.TotalPaid.ToString(), out d))
                {
                    TotalPaid = d;
                }

                double? Year1st = null;
                if (Util.HasProperty(paramObject, "Year1st") && paramObject.Year1st != null && double.TryParse(paramObject.Year1st.ToString(), out d))
                {
                    Year1st = d;
                }

                double? Renewal = null;
                if (Util.HasProperty(paramObject, "Renewal") && paramObject.Renewal != null && double.TryParse(paramObject.Renewal.ToString(), out d))
                {
                    Renewal = d;
                }

                double? Gross1st = null;
                if (Util.HasProperty(paramObject, "Gross1st") && paramObject.Gross1st != null && double.TryParse(paramObject.Gross1st.ToString(), out d))
                {
                    Gross1st = d;
                }

                double? GrossRen = null;
                if (Util.HasProperty(paramObject, "GrossRen") && paramObject.GrossRen != null && double.TryParse(paramObject.GrossRen.ToString(), out d))
                {
                    GrossRen = d;
                }

                if (!string.IsNullOrEmpty(InvoiceNumber)) query = query.Where(q => q.InvoiceNumber.Contains(InvoiceNumber));
                if (!string.IsNullOrEmpty(RiskQuarter)) query = query.Where(q => q.RiskQuarter.Contains(RiskQuarter));
                if (InvoiceAt.HasValue) query = query.Where(q => q.InvoiceDate == InvoiceAt);
                if (StatementReceivedAt.HasValue) query = query.Where(q => q.StatementReceivedDate == StatementReceivedAt);
                if (!string.IsNullOrEmpty(AccountFor)) query = query.Where(q => q.AccountDescription == AccountFor);
                if (TotalPaid.HasValue) query = query.Where(q => q.TotalPaid == TotalPaid);
                if (Year1st.HasValue) query = query.Where(q => q.Year1st == Year1st);
                if (Renewal.HasValue) query = query.Where(q => q.Renewal == Renewal);
                if (Gross1st.HasValue) query = query.Where(q => q.Gross1st == Gross1st);
                if (GrossRen.HasValue) query = query.Where(q => q.GrossRenewal == GrossRen);
                if (TreatyCodeId.HasValue) query = query.Where(q => q.TreatyCodeId == TreatyCodeId);
                if (CedantId.HasValue) query = query.Where(q => q.CedantId == CedantId);
                if (PreparedById.HasValue) query = query.Where(q => q.PreparedById == PreparedById);
                if (InvoiceType.HasValue) query = query.Where(q => q.InvoiceType == InvoiceType);
                if (Status.HasValue) query = query.Where(q => q.Status == Status);
            }

            return query;
        }

        public static IQueryable<RetroRegisterBo> GetRetroRegisterQuery(ref ExportBo exportBo, AppDbContext db)
        {
            var paramObject = exportBo.ParameterObject;
            var query = db.RetroRegisters
                .Where(q => q.ReportingType != RetroRegisterBo.ReportingTypeIFRS17)
                .Where(q => q.Type == RetroRegisterBo.TypeDirectRetro || (q.Type == RetroRegisterBo.TypePerLifeRetro && q.TreatyType == "L-YRT"))
                .Select(RetroRegisterService.Expression());

            if (paramObject != null)
            {
                string InvoiceNumber = Util.HasProperty(paramObject, "InvoiceNumber") ? paramObject.InvoiceNumber : null;
                string RiskQuarter = Util.HasProperty(paramObject, "RiskQuarter") ? paramObject.RiskQuarter : null;
                string AccountFor = Util.HasProperty(paramObject, "AccountFor") ? paramObject.AccountFor : null;

                int r = 0;
                int? Type = null;
                if (Util.HasProperty(paramObject, "Type") && paramObject.Type != null && int.TryParse(paramObject.Type.ToString(), out r))
                {
                    Type = r;
                }

                int? TreatyCodeId = null;
                if (Util.HasProperty(paramObject, "TreatyCodeId") && paramObject.TreatyCodeId != null && int.TryParse(paramObject.TreatyCodeId.ToString(), out r))
                {
                    TreatyCodeId = r;
                }

                int? CedantId = null;
                if (Util.HasProperty(paramObject, "CedantId") && paramObject.CedantId != null && int.TryParse(paramObject.CedantId.ToString(), out r))
                {
                    CedantId = r;
                }

                int? RetroCodeId = null;
                if (Util.HasProperty(paramObject, "RetroPartyId") && paramObject.RetroPartyId != null && int.TryParse(paramObject.RetroPartyId.ToString(), out r))
                {
                    RetroCodeId = r;
                }

                int? PreparedById = null;
                if (Util.HasProperty(paramObject, "PreparedById") && paramObject.PreparedById != null && int.TryParse(paramObject.PreparedById.ToString(), out r))
                {
                    PreparedById = r;
                }

                int? ApprovalStatus = null;
                if (Util.HasProperty(paramObject, "ApprovalStatus") && paramObject.ApprovalStatus != null && int.TryParse(paramObject.ApprovalStatus.ToString(), out r))
                {
                    ApprovalStatus = r;
                }

                int? RetroStatus = null;
                if (Util.HasProperty(paramObject, "RetroStatus") && paramObject.RetroStatus != null && int.TryParse(paramObject.RetroStatus.ToString(), out r))
                {
                    RetroStatus = r;
                }

                DateTime dt = DateTime.Today;
                DateTime? InvoiceAt = null;
                if (Util.HasProperty(paramObject, "InvoiceDate") && paramObject.InvoiceDate != null && DateTime.TryParse(paramObject.InvoiceDate.ToString(), out dt))
                {
                    InvoiceAt = dt;
                }

                DateTime? ReportCompletedAt = null;
                if (Util.HasProperty(paramObject, "ReportCompletedDate") && paramObject.ReportCompletedDate != null && DateTime.TryParse(paramObject.ReportCompletedDate.ToString(), out dt))
                {
                    ReportCompletedAt = dt;
                }

                DateTime? RetroConfirmationDateAt = null;
                if (Util.HasProperty(paramObject, "RetroConfirmationDate") && paramObject.RetroConfirmationDate != null && DateTime.TryParse(paramObject.RetroConfirmationDate.ToString(), out dt))
                {
                    RetroConfirmationDateAt = dt;
                }

                double d = 0;
                double? TotalPaid = null;
                if (Util.HasProperty(paramObject, "TotalPaid") && paramObject.TotalPaid != null && double.TryParse(paramObject.TotalPaid.ToString(), out d))
                {
                    TotalPaid = d;
                }

                double? Year1st = null;
                if (Util.HasProperty(paramObject, "Year1st") && paramObject.Year1st != null && double.TryParse(paramObject.Year1st.ToString(), out d))
                {
                    Year1st = d;
                }

                double? Renewal = null;
                if (Util.HasProperty(paramObject, "Renewal") && paramObject.Renewal != null && double.TryParse(paramObject.Renewal.ToString(), out d))
                {
                    Renewal = d;
                }

                double? Gross1st = null;
                if (Util.HasProperty(paramObject, "Gross1st") && paramObject.Gross1st != null && double.TryParse(paramObject.Gross1st.ToString(), out d))
                {
                    Gross1st = d;
                }

                double? GrossRen = null;
                if (Util.HasProperty(paramObject, "GrossRenewal") && paramObject.GrossRenewal != null && double.TryParse(paramObject.GrossRenewal.ToString(), out d))
                {
                    GrossRen = d;
                }

                if (Type.HasValue) query = query.Where(q => q.Type == Type);
                if (!string.IsNullOrEmpty(InvoiceNumber)) query = query.Where(q => q.RetroStatementNo == InvoiceNumber);
                if (InvoiceAt.HasValue) query = query.Where(q => q.RetroStatementDate == InvoiceAt);
                if (ReportCompletedAt.HasValue) query = query.Where(q => q.ReportCompletedDate == ReportCompletedAt);
                if (RetroConfirmationDateAt.HasValue) query = query.Where(q => q.RetroConfirmationDate == RetroConfirmationDateAt);
                if (CedantId.HasValue) query = query.Where(q => q.CedantId == CedantId);
                if (RetroCodeId.HasValue) query = query.Where(q => q.RetroPartyId == RetroCodeId);
                if (TreatyCodeId.HasValue) query = query.Where(q => q.TreatyCodeId == TreatyCodeId);
                if (!string.IsNullOrEmpty(RiskQuarter)) query = query.Where(q => q.RiskQuarter == RiskQuarter);
                if (!string.IsNullOrEmpty(AccountFor)) query = query.Where(q => q.AccountFor == AccountFor);
                if (TotalPaid.HasValue) query = query.Where(q => q.NetTotalAmount == TotalPaid);
                if (Year1st.HasValue) query = query.Where(q => q.Year1st == Year1st);
                if (Renewal.HasValue) query = query.Where(q => q.Renewal == Renewal);
                if (Gross1st.HasValue) query = query.Where(q => q.Gross1st == Gross1st);
                if (GrossRen.HasValue) query = query.Where(q => q.GrossRen == GrossRen);
                if (PreparedById.HasValue) query = query.Where(q => q.PreparedById == PreparedById);
                if (ApprovalStatus.HasValue) query = query.Where(q => q.Status == ApprovalStatus);
                if (RetroStatus.HasValue) query = query.Where(q => q.RetroStatus == RetroStatus);
            }

            return query;
        }

        public static IQueryable<TreatyPricingQuotationWorkflowBo> GetQuotationWorkflowQuery(ref ExportBo exportBo, AppDbContext db)
        {
            var objectId = exportBo.ObjectId;
            var paramObject = exportBo.ParameterObject;
            var query = db.TreatyPricingQuotationWorkflows.Select(TreatyPricingQuotationWorkflowService.Expression());
            if (paramObject != null)
            {
                string QuotationId = Util.HasProperty(paramObject, "QuotationId") ? paramObject.QuotationId : null;
                string Name = Util.HasProperty(paramObject, "Name") ? paramObject.Name : null;
                string Description = Util.HasProperty(paramObject, "Description") ? paramObject.Description : null;

                int i = 0;
                int? CedantId = null;
                if (Util.HasProperty(paramObject, "CedantId") && paramObject.CedantId != null && int.TryParse(paramObject.CedantId.ToString(), out i))
                {
                    CedantId = i;
                }

                int? ReinsuranceTypePickListDetailId = null;
                if (Util.HasProperty(paramObject, "ReinsuranceTypePickListDetailId") && paramObject.ReinsuranceTypePickListDetailId != null && int.TryParse(paramObject.ReinsuranceTypePickListDetailId.ToString(), out i))
                {
                    ReinsuranceTypePickListDetailId = i;
                }

                int? LatestVersion = null;
                if (Util.HasProperty(paramObject, "LatestVersion") && paramObject.LatestVersion != null && int.TryParse(paramObject.LatestVersion.ToString(), out i))
                {
                    LatestVersion = i;
                }

                int? Status = null;
                if (Util.HasProperty(paramObject, "Status") && paramObject.Status != null && int.TryParse(paramObject.Status.ToString(), out i))
                {
                    Status = i;
                }

                int? PricingStatus = null;
                if (Util.HasProperty(paramObject, "PricingStatus") && paramObject.PricingStatus != null && int.TryParse(paramObject.PricingStatus.ToString(), out i))
                {
                    PricingStatus = i;
                }

                int? PricingTeamPickListDetailId = null;
                if (Util.HasProperty(paramObject, "PricingTeamPickListDetailId") && paramObject.PricingTeamPickListDetailId != null && int.TryParse(paramObject.PricingTeamPickListDetailId.ToString(), out i))
                {
                    PricingTeamPickListDetailId = i;
                }

                int? BDPersonInChargeId = null;
                if (Util.HasProperty(paramObject, "BDPersonInChargeId") && paramObject.BDPersonInChargeId != null && int.TryParse(paramObject.BDPersonInChargeId.ToString(), out i))
                {
                    BDPersonInChargeId = i;
                }

                int? PersonInChargeId = null;
                if (Util.HasProperty(paramObject, "PersonInChargeId") && paramObject.PersonInChargeId != null && int.TryParse(paramObject.PersonInChargeId.ToString(), out i))
                {
                    PersonInChargeId = i;
                }

                int? InternalTeam = null;
                if (Util.HasProperty(paramObject, "InternalTeam") && paramObject.InternalTeam != null && int.TryParse(paramObject.InternalTeam.ToString(), out i))
                {
                    InternalTeam = i;
                }

                DateTime dt = DateTime.Today;
                DateTime? CreatedAt = null;
                if (Util.HasProperty(paramObject, "CreatedAt") && paramObject.CreatedAt != null && DateTime.TryParse(paramObject.CreatedAt.ToString(), out dt))
                {
                    CreatedAt = dt;
                }

                if (!string.IsNullOrEmpty(QuotationId)) query = query.Where(q => !string.IsNullOrEmpty(q.QuotationId) && q.QuotationId.Contains(QuotationId));
                if (CreatedAt.HasValue) query = query.Where(q => q.CreatedAt == CreatedAt);
                if (CedantId.HasValue) query = query.Where(q => q.CedantId == CedantId);
                if (ReinsuranceTypePickListDetailId.HasValue) query = query.Where(q => q.ReinsuranceTypePickListDetailId == ReinsuranceTypePickListDetailId);
                if (!string.IsNullOrEmpty(Name)) query = query.Where(q => !string.IsNullOrEmpty(q.Name) && q.Name.Contains(Name));
                if (!string.IsNullOrEmpty(Description)) query = query.Where(q => !string.IsNullOrEmpty(q.Description) && q.Description.Contains(Description));
                if (LatestVersion.HasValue) query = query.Where(q => q.LatestVersion == LatestVersion);
                if (Status.HasValue) query = query.Where(q => q.Status == Status);
                if (PricingStatus.HasValue) query = query.Where(q => q.PricingStatus == PricingStatus);
                if (PricingTeamPickListDetailId.HasValue) query = query.Where(q => q.PricingTeamPickListDetailId == PricingTeamPickListDetailId);

                if (BDPersonInChargeId.HasValue)
                {
                    if (BDPersonInChargeId == 9999999) query = query.Where(q => !q.BDPersonInChargeId.HasValue);
                    else query = query.Where(q => q.BDPersonInChargeId == BDPersonInChargeId);
                }
                if (PersonInChargeId.HasValue)
                {
                    if (PersonInChargeId == 9999999) query = query.Where(q => !q.PersonInChargeId.HasValue);
                    else query = query.Where(q => q.PersonInChargeId == PersonInChargeId);
                }
                if (InternalTeam.HasValue)
                {
                    if (InternalTeam.Value == TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamCEO) query = query.Where(q => q.CEOPending > 1);
                    if (InternalTeam.Value == TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamPricing) query = query.Where(q => q.PricingPending > 1);
                    if (InternalTeam.Value == TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamUnderwriting) query = query.Where(q => q.UnderwritingPending > 1);
                    if (InternalTeam.Value == TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamHealth) query = query.Where(q => q.HealthPending > 1);
                    if (InternalTeam.Value == TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamClaims) query = query.Where(q => q.ClaimsPending > 1);
                    if (InternalTeam.Value == TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamBD) query = query.Where(q => q.BDPending > 1);
                    if (InternalTeam.Value == TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamCR) query = query.Where(q => q.TGPending > 1);
                }
            }

            return query;
        }
        public static IQueryable<TreatyPricingTreatyWorkflowBo> GetTreatyWorkflowQuery(ref ExportBo exportBo, AppDbContext db)
        {
            var objectId = exportBo.ObjectId;
            var paramObject = exportBo.ParameterObject;
            var query = db.TreatyPricingTreatyWorkflows.Select(TreatyPricingTreatyWorkflowService.Expression());
            if (paramObject != null)
            {
                int i = 0;

                int? ReinsuranceTypePickListDetailId = null;
                if (Util.HasProperty(paramObject, "ReinsuranceTypePickListDetailId") && paramObject.ReinsuranceTypePickListDetailId != null && int.TryParse(paramObject.ReinsuranceTypePickListDetailId.ToString(), out i))
                {
                    ReinsuranceTypePickListDetailId = i;
                }

                int? InwardRetroPartyDetailId = null;
                if (Util.HasProperty(paramObject, "InwardRetroPartyDetailId") && paramObject.InwardRetroPartyDetailId != null && int.TryParse(paramObject.InwardRetroPartyDetailId.ToString(), out i))
                {
                    InwardRetroPartyDetailId = i;
                }

                string DocumentId = Util.HasProperty(paramObject, "DocumentId") ? paramObject.DocumentId : null;

                int? DocumentType = null;
                if (Util.HasProperty(paramObject, "DocumentType") && paramObject.DocumentType != null && int.TryParse(paramObject.DocumentType.ToString(), out i))
                {
                    DocumentType = i;
                }

                string TypeOfBusiness = Util.HasProperty(paramObject, "TypeOfBusiness") ? paramObject.TypeOfBusiness : null;
                string CountryOrigin = Util.HasProperty(paramObject, "CountryOrigin") ? paramObject.CountryOrigin : null;

                string Description = Util.HasProperty(paramObject, "Description") ? paramObject.Description : null;
                string TreatyCode = Util.HasProperty(paramObject, "TreatyCode") ? paramObject.TreatyCode : null;

                DateTime dt = DateTime.Today;
                DateTime? EffectiveAt = null;
                if (Util.HasProperty(paramObject, "EffectiveAtStr") && paramObject.EffectiveAtStr != null && DateTime.TryParse(paramObject.EffectiveAtStr.ToString(), out dt))
                {
                    EffectiveAt = dt;
                }

                int? CoverageStatus = null;
                if (Util.HasProperty(paramObject, "CoverageStatus") && paramObject.CoverageStatus != null && int.TryParse(paramObject.CoverageStatus.ToString(), out i))
                {
                    CoverageStatus = i;
                }

                int? DocumentStatus = null;
                if (Util.HasProperty(paramObject, "DocumentStatus") && paramObject.DocumentStatus != null && int.TryParse(paramObject.DocumentStatus.ToString(), out i))
                {
                    DocumentStatus = i;
                }

                int? DraftingStatus = null;
                if (Util.HasProperty(paramObject, "DraftingStatus") && paramObject.DraftingStatus != null && int.TryParse(paramObject.DraftingStatus.ToString(), out i))
                {
                    DraftingStatus = i;
                }

                int? DraftingStatusCategory = null;
                if (Util.HasProperty(paramObject, "DraftingStatusCategory") && paramObject.DraftingStatusCategory != null && int.TryParse(paramObject.DraftingStatusCategory.ToString(), out i))
                {
                    DraftingStatusCategory = i;
                }

                if (ReinsuranceTypePickListDetailId.HasValue) query = query.Where(q => q.ReinsuranceTypePickListDetailId == ReinsuranceTypePickListDetailId);
                if (InwardRetroPartyDetailId.HasValue) query = query.Where(q => q.InwardRetroPartyDetailId == InwardRetroPartyDetailId);
                if (!string.IsNullOrEmpty(DocumentId)) query = query.Where(q => q.DocumentId == DocumentId);
                if (DocumentType.HasValue) query = query.Where(q => q.DocumentType == DocumentType);
                if (!string.IsNullOrEmpty(TypeOfBusiness)) query = query.Where(q => q.TypeOfBusiness.Contains(TypeOfBusiness));
                if (!string.IsNullOrEmpty(CountryOrigin)) query = query.Where(q => q.TypeOfBusiness.Contains(CountryOrigin));
                if (!string.IsNullOrEmpty(Description)) query = query.Where(q => !string.IsNullOrEmpty(q.Description) && q.Description.Contains(Description));
                if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => !string.IsNullOrEmpty(q.TreatyCode) && q.TreatyCode.Contains(TreatyCode));
                if (EffectiveAt.HasValue) query = query.Where(q => q.EffectiveAt == EffectiveAt);
                if (CoverageStatus.HasValue) query = query.Where(q => q.CoverageStatus == CoverageStatus);
                if (DocumentStatus.HasValue) query = query.Where(q => q.DocumentStatus == DocumentStatus);
                if (DraftingStatus.HasValue) query = query.Where(q => q.DraftingStatus == DraftingStatus);
                if (DraftingStatusCategory.HasValue) query = query.Where(q => q.DraftingStatusCategory == DraftingStatusCategory);
            }

            return query;
        }

        public static IQueryable<TreatyPricingGroupReferralVersionBo> GetGroupReferralQuery(ref ExportBo exportBo, AppDbContext db)
        {
            var paramObject = exportBo.ParameterObject;
            var query = db.TreatyPricingGroupReferralVersions.GroupBy(p => p.TreatyPricingGroupReferralId)
                  .Select(g => g.OrderByDescending(p => p.Version).FirstOrDefault())
                  .Select(TreatyPricingGroupReferralVersionService.Expression());

            if (paramObject != null)
            {
                string GroupReferralCode = Util.HasProperty(paramObject, "GroupReferralCode") ? paramObject.GroupReferralCode : null;
                string Description = Util.HasProperty(paramObject, "Description") ? paramObject.Description : null;
                string RiGroupSlipCode = Util.HasProperty(paramObject, "RiGroupSlipCode") ? paramObject.RiGroupSlipCode : null;
                string WonVersion = Util.HasProperty(paramObject, "WonVersion") ? paramObject.RiGroupSlipCode : null;

                int r = 0;
                int? CedantId = null;
                if (Util.HasProperty(paramObject, "CedantId") && paramObject.CedantId != null && int.TryParse(paramObject.CedantId.ToString(), out r))
                {
                    CedantId = r;
                }

                int? InsuredGroupNameId = null;
                if (Util.HasProperty(paramObject, "InsuredGroupNameId") && paramObject.InsuredGroupNameId != null && int.TryParse(paramObject.InsuredGroupNameId.ToString(), out r))
                {
                    InsuredGroupNameId = r;
                }

                int? IndustryNameId = null;
                if (Util.HasProperty(paramObject, "IndustryNameId") && paramObject.IndustryNameId != null && int.TryParse(paramObject.IndustryNameId.ToString(), out r))
                {
                    IndustryNameId = r;
                }

                int? ReferredTypeId = null;
                if (Util.HasProperty(paramObject, "ReferredTypeId") && paramObject.ReferredTypeId != null && int.TryParse(paramObject.ReferredTypeId.ToString(), out r))
                {
                    ReferredTypeId = r;
                }

                int? RiGroupSlipPICId = null;
                if (Util.HasProperty(paramObject, "RiGroupSlipPICId") && paramObject.RiGroupSlipPICId != null && int.TryParse(paramObject.RiGroupSlipPICId.ToString(), out r))
                {
                    RiGroupSlipPICId = r;
                }

                int? RiGroupSlipStatusId = null;
                if (Util.HasProperty(paramObject, "RiGroupSlipStatusId") && paramObject.RiGroupSlipStatusId != null && int.TryParse(paramObject.RiGroupSlipStatusId.ToString(), out r))
                {
                    RiGroupSlipStatusId = r;
                }

                int? GroupMasterLetterId = null;
                if (Util.HasProperty(paramObject, "GroupMasterLetterId") && paramObject.GroupMasterLetterId != null && int.TryParse(paramObject.GroupMasterLetterId.ToString(), out r))
                {
                    GroupMasterLetterId = r;
                }

                int? Status = null;
                if (Util.HasProperty(paramObject, "Status") && paramObject.Status != null && int.TryParse(paramObject.Status.ToString(), out r))
                {
                    Status = r;
                }

                int? RequestTypeId = null;
                if (Util.HasProperty(paramObject, "RequestTypeId") && paramObject.RequestTypeId != null && int.TryParse(paramObject.RequestTypeId.ToString(), out r))
                {
                    RequestTypeId = r;
                }

                int? VersionNo = null;
                if (Util.HasProperty(paramObject, "VersionNo") && paramObject.VersionNo != null && int.TryParse(paramObject.VersionNo.ToString(), out r))
                {
                    VersionNo = r;
                }

                int? GroupReferralPICId = null;
                if (Util.HasProperty(paramObject, "GroupReferralPIC") && paramObject.GroupReferralPIC != null && int.TryParse(paramObject.GroupReferralPIC.ToString(), out r))
                {
                    GroupReferralPICId = r;
                }

                int? QuotationTat = null;
                if (Util.HasProperty(paramObject, "QuotationTAT") && paramObject.QuotationTAT != null && int.TryParse(paramObject.QuotationTAT.ToString(), out r))
                {
                    QuotationTat = r;
                }

                int? InternalTat = null;
                if (Util.HasProperty(paramObject, "InternalTAT") && paramObject.InternalTAT != null && int.TryParse(paramObject.InternalTAT.ToString(), out r))
                {
                    InternalTat = r;
                }

                int? WorkflowStatus = null;
                if (Util.HasProperty(paramObject, "WorkflowStatusId") && paramObject.WorkflowStatusId != null && int.TryParse(paramObject.WorkflowStatusId.ToString(), out r))
                {
                    WorkflowStatus = r;
                }

                int? ChecklistPendingId = null;
                if (Util.HasProperty(paramObject, "ChecklistPending") && paramObject.ChecklistPending != null && int.TryParse(paramObject.ChecklistPending.ToString(), out r))
                {
                    ChecklistPendingId = r;
                }

                int? ScoreNo = null;
                if (Util.HasProperty(paramObject, "Score") && paramObject.Score != null && int.TryParse(paramObject.Score.ToString(), out r))
                {
                    ScoreNo = r;
                }

                DateTime dt = DateTime.Today;
                DateTime? RiGroupSlipConfirmationAt = null;
                if (Util.HasProperty(paramObject, "RiGroupSlipConfirmationDate") && paramObject.RiGroupSlipConfirmationDate != null && DateTime.TryParse(paramObject.RiGroupSlipConfirmationDate.ToString(), out dt))
                {
                    RiGroupSlipConfirmationAt = dt;
                }

                DateTime? FirstReferralAt = null;
                if (Util.HasProperty(paramObject, "FirstReferralDate") && paramObject.FirstReferralDate != null && DateTime.TryParse(paramObject.FirstReferralDate.ToString(), out dt))
                {
                    FirstReferralAt = dt;
                }

                DateTime? CoverageStartAt = null;
                if (Util.HasProperty(paramObject, "CoverageStartDate") && paramObject.CoverageStartDate != null && DateTime.TryParse(paramObject.CoverageStartDate.ToString(), out dt))
                {
                    CoverageStartAt = dt;
                }

                DateTime? CoverageEndAt = null;
                if (Util.HasProperty(paramObject, "CoverageEndDate") && paramObject.CoverageEndDate != null && DateTime.TryParse(paramObject.CoverageEndDate.ToString(), out dt))
                {
                    CoverageEndAt = dt;
                }

                double d = 0;
                double? GrossRiskPrem = null;
                if (Util.HasProperty(paramObject, "GrossRiskPremium") && paramObject.GrossRiskPremium != null && double.TryParse(paramObject.GrossRiskPremium.ToString(), out d))
                {
                    GrossRiskPrem = d;
                }

                double? ReinsPremium = null;
                if (Util.HasProperty(paramObject, "ReinsurancePremium") && paramObject.ReinsurancePremium != null && double.TryParse(paramObject.ReinsurancePremium.ToString(), out d))
                {
                    ReinsPremium = d;
                }

                double? GroupSize = null;
                if (Util.HasProperty(paramObject, "GroupSize") && paramObject.GroupSize != null && double.TryParse(paramObject.GroupSize.ToString(), out d))
                {
                    GroupSize = d;
                }

                if (CedantId.HasValue) query = query.Where(q => q.CedantId == CedantId);
                if (InsuredGroupNameId.HasValue) query = query.Where(q => q.InsuredGroupNameId == InsuredGroupNameId);
                if (IndustryNameId.HasValue) query = query.Where(q => q.IndustryNamePickListDetailId == IndustryNameId);
                if (ReferredTypeId.HasValue) query = query.Where(q => q.ReferredTypePickListDetailId == ReferredTypeId);
                if (RiGroupSlipPICId.HasValue) query = query.Where(q => q.RiGroupSlipPersonInChargeId == RiGroupSlipPICId);
                if (RiGroupSlipStatusId.HasValue) query = query.Where(q => q.RiGroupSlipStatus == RiGroupSlipStatusId);
                if (GroupMasterLetterId.HasValue) query = query.Where(q => q.TreatyPricingGroupMasterLetterId == GroupMasterLetterId);
                if (Status.HasValue) query = query.Where(q => q.GroupReferralStatus == Status);
                if (RequestTypeId.HasValue) query = query.Where(q => q.RequestTypePickListDetailId == RequestTypeId);
                if (VersionNo.HasValue) query = query.Where(q => q.Version == VersionNo);
                if (GroupReferralPICId.HasValue)
                {
                    if (GroupReferralPICId == 9999999) query = query.Where(q => !q.GroupReferralPersonInChargeId.HasValue);
                    else query = query.Where(q => q.GroupReferralPersonInChargeId == GroupReferralPICId);
                }
                if (!string.IsNullOrEmpty(GroupReferralCode)) query = query.Where(q => q.GroupReferralCode.Contains(GroupReferralCode));
                if (!string.IsNullOrEmpty(Description)) query = query.Where(q => q.GroupReferralDescription.Contains(Description));
                if (!string.IsNullOrEmpty(RiGroupSlipCode)) query = query.Where(q => q.RiGroupSlipCode.Contains(RiGroupSlipCode));
                if (RiGroupSlipConfirmationAt.HasValue) query = query.Where(q => q.RiGroupSlipConfirmationDate == RiGroupSlipConfirmationAt);
                if (FirstReferralAt.HasValue) query = query.Where(q => q.FirstReferralDate == FirstReferralAt);
                if (CoverageStartAt.HasValue) query = query.Where(q => q.CoverageStartDate == CoverageStartAt);
                if (CoverageEndAt.HasValue) query = query.Where(q => q.CoverageEndDate == CoverageEndAt);
                if (GroupSize.HasValue) query = query.Where(q => q.GroupSize == GroupSize);
                if (GrossRiskPrem.HasValue) query = query.Where(q => q.GrossRiskPremium == GrossRiskPrem);
                if (ReinsPremium.HasValue) query = query.Where(q => q.ReinsurancePremium == ReinsPremium);
                if (!string.IsNullOrEmpty(WonVersion)) query = query.Where(q => q.WonVersion == WonVersion);
                if (ScoreNo.HasValue) query = query.Where(q => q.AverageScore == ScoreNo);
                if (QuotationTat.HasValue)
                {
                    if (QuotationTat == 4) query = query.Where(q => q.QuotationTAT.Value > 3);
                    else query = query.Where(q => q.QuotationTAT == QuotationTat);
                }
                if (InternalTat.HasValue)
                {
                    if (InternalTat == 4) query = query.Where(q => q.InternalTAT.Value > 3);
                    else query = query.Where(q => q.InternalTAT == InternalTat);
                }
                if (WorkflowStatus.HasValue) query = query.Where(q => q.WorkflowStatusId == WorkflowStatus);
                if (ChecklistPendingId.HasValue)
                {
                    if (ChecklistPendingId == TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamUnderwriting) query = query.Where(q => q.ChecklistPendingUnderwriting == true);
                    if (ChecklistPendingId == TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamHealth) query = query.Where(q => q.ChecklistPendingHealth == true);
                    if (ChecklistPendingId == TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamClaims) query = query.Where(q => q.ChecklistPendingClaims == true);
                    if (ChecklistPendingId == TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamBD) query = query.Where(q => q.ChecklistPendingBD == true);
                    if (ChecklistPendingId == TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamCR) query = query.Where(q => q.ChecklistPendingCR == true);
                }
            }

            return query;
        }

        public static IQueryable<PerLifeAggregationDetailDataBo> GetPerLifeAggregationConflictListingQuery(ref ExportBo exportBo, AppDbContext db)
        {
            var paramObject = exportBo.ParameterObject;
            var query = db.PerLifeAggregationDetailData
                .Where(q => q.IsException == true)
                .Where(q => q.ExceptionType == PerLifeAggregationDetailDataBo.ExceptionTypeConflictCheck)
                .Select(PerLifeAggregationDetailDataService.Expression());

            if (paramObject != null)
            {
                string TreatyCode = Util.HasProperty(paramObject, "TreatyCode") ? paramObject.TreatyCode : null;
                string InsuredName = Util.HasProperty(paramObject, "InsuredName") ? paramObject.InsuredName : null;
                string InsuredGender = Util.HasProperty(paramObject, "InsuredGender") ? paramObject.InsuredGender : null;
                string PolicyNumber = Util.HasProperty(paramObject, "PolicyNumber") ? paramObject.PolicyNumber : null;
                string PremiumFrequencyMode = Util.HasProperty(paramObject, "PremiumFrequencyMode") ? paramObject.PremiumFrequencyMode : null;
                string RetroPremiumFrequencyMode = Util.HasProperty(paramObject, "RetroPremiumFrequencyMode") ? paramObject.RetroPremiumFrequencyMode : null;
                string CedingPlanCode = Util.HasProperty(paramObject, "CedingPlanCode") ? paramObject.CedingPlanCode : null;
                string CedingBenefitTypeCode = Util.HasProperty(paramObject, "CedingBenefitTypeCode") ? paramObject.CedingBenefitTypeCode : null;
                string CedingBenefitRiskCode = Util.HasProperty(paramObject, "CedingBenefitRiskCode") ? paramObject.CedingBenefitRiskCode : null;
                string MLReBenefitCode = Util.HasProperty(paramObject, "MLReBenefitCode") ? paramObject.MLReBenefitCode : null;
                string TerritoryOfIssueCode = Util.HasProperty(paramObject, "TerritoryOfIssueCode") ? paramObject.TerritoryOfIssueCode : null;

                int r = 0;
                int? RiskYear = null;
                if (Util.HasProperty(paramObject, "RiskYear") && paramObject.RiskYear != null && int.TryParse(paramObject.RiskYear.ToString(), out r))
                {
                    RiskYear = r;
                }

                int? RiskMonth = null;
                if (Util.HasProperty(paramObject, "RiskMonth") && paramObject.RiskMonth != null && int.TryParse(paramObject.RiskMonth.ToString(), out r))
                {
                    RiskMonth = r;
                }

                DateTime dt = DateTime.Today;
                DateTime? InsuredDateOfBirth = null;
                if (Util.HasProperty(paramObject, "InsuredDateOfBirth") && paramObject.InsuredDateOfBirth != null && DateTime.TryParse(paramObject.InsuredDateOfBirth.ToString(), out dt))
                {
                    InsuredDateOfBirth = dt;
                }

                DateTime? ReinsEffDatePol = null;
                if (Util.HasProperty(paramObject, "ReinsEffDatePol") && paramObject.ReinsEffDatePol != null && DateTime.TryParse(paramObject.ReinsEffDatePol.ToString(), out dt))
                {
                    ReinsEffDatePol = dt;
                }

                double d = 0;
                double? Aar = null;
                if (Util.HasProperty(paramObject, "Aar") && paramObject.Aar != null && double.TryParse(paramObject.Aar.ToString(), out d))
                {
                    Aar = d;
                }

                double? GrossPremium = null;
                if (Util.HasProperty(paramObject, "GrossPremium") && paramObject.GrossPremium != null && double.TryParse(paramObject.GrossPremium.ToString(), out d))
                {
                    GrossPremium = d;
                }

                double? NetPremium = null;
                if (Util.HasProperty(paramObject, "NetPremium") && paramObject.NetPremium != null && double.TryParse(paramObject.NetPremium.ToString(), out d))
                {
                    NetPremium = d;
                }

                if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => q.TreatyCode == TreatyCode);
                if (RiskYear.HasValue) query = query.Where(q => q.RiskPeriodYear == RiskYear);
                if (RiskMonth.HasValue) query = query.Where(q => q.RiskPeriodMonth == RiskMonth);
                if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => q.InsuredName.Contains(InsuredName));
                if (!string.IsNullOrEmpty(InsuredGender)) query = query.Where(q => q.InsuredGenderCode == InsuredGender);
                if (InsuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == InsuredDateOfBirth);
                if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber.Contains(PolicyNumber));
                if (ReinsEffDatePol.HasValue) query = query.Where(q => q.ReinsEffDatePol == ReinsEffDatePol);
                if (Aar.HasValue) query = query.Where(q => q.Aar == Aar);
                if (GrossPremium.HasValue) query = query.Where(q => q.GrossPremium == GrossPremium);
                if (NetPremium.HasValue) query = query.Where(q => q.NetPremium == NetPremium);
                if (!string.IsNullOrEmpty(PremiumFrequencyMode)) query = query.Where(q => q.PremiumFrequencyCode == PremiumFrequencyMode);
                //if (RetroPremiumFrequencyMode.HasValue) query = query.Where(q => q.PremiumFrequencyCode == RetroPremiumFrequencyMode);
                if (!string.IsNullOrEmpty(CedingPlanCode)) query = query.Where(q => q.CedingPlanCode.Contains(CedingPlanCode));
                if (!string.IsNullOrEmpty(CedingBenefitTypeCode)) query = query.Where(q => q.CedingBenefitTypeCode == CedingBenefitTypeCode);
                if (!string.IsNullOrEmpty(CedingBenefitRiskCode)) query = query.Where(q => q.CedingBenefitRiskCode.Contains(CedingBenefitRiskCode));
                if (!string.IsNullOrEmpty(MLReBenefitCode)) query = query.Where(q => q.MlreBenefitCode == MLReBenefitCode);
                if (!string.IsNullOrEmpty(TerritoryOfIssueCode)) query = query.Where(q => q.TerritoryOfIssueCode == TerritoryOfIssueCode);
            }

            return query;
        }

        public static IQueryable<PerLifeAggregationDetailDataBo> GetPerLifeAggregationDuplicationListingQuery(ref ExportBo exportBo, AppDbContext db)
        {
            var paramObject = exportBo.ParameterObject;
            var query = db.PerLifeAggregationDetailData
                .Where(q => q.IsException == true)
                .Where(q => q.ExceptionType == PerLifeAggregationDetailDataBo.ExceptionTypeDuplicationCheck)
                .Select(PerLifeAggregationDetailDataService.Expression());

            if (paramObject != null)
            {
                string TreatyCode = Util.HasProperty(paramObject, "TreatyCode") ? paramObject.TreatyCode : null;
                string InsuredName = Util.HasProperty(paramObject, "TreatyCode") ? paramObject.TreatyCode : null;
                string InsuredGender = Util.HasProperty(paramObject, "TreatyCode") ? paramObject.TreatyCode : null;
                string PolicyNumber = Util.HasProperty(paramObject, "TreatyCode") ? paramObject.TreatyCode : null;
                string FundsAccountingType = Util.HasProperty(paramObject, "TreatyCode") ? paramObject.TreatyCode : null;
                string ReinsuranceRiskBasis = Util.HasProperty(paramObject, "TreatyCode") ? paramObject.TreatyCode : null;
                string CedingPlanCode = Util.HasProperty(paramObject, "TreatyCode") ? paramObject.TreatyCode : null;
                string MLReBenefitCode = Util.HasProperty(paramObject, "TreatyCode") ? paramObject.TreatyCode : null;
                string CedingBenefitRiskCode = Util.HasProperty(paramObject, "TreatyCode") ? paramObject.TreatyCode : null;
                string CedingBenefitTypeCode = Util.HasProperty(paramObject, "TreatyCode") ? paramObject.TreatyCode : null;
                string TransactionType = Util.HasProperty(paramObject, "TreatyCode") ? paramObject.TreatyCode : null;
                string Remarks = Util.HasProperty(paramObject, "TreatyCode") ? paramObject.TreatyCode : null;

                int r = 0;
                int? ExceptionStatus = null;
                if (Util.HasProperty(paramObject, "ExceptionStatus") && paramObject.ExceptionStatus != null && int.TryParse(paramObject.ExceptionStatus.ToString(), out r))
                {
                    ExceptionStatus = r;
                }

                int? ProceedToAggregate = null;
                if (Util.HasProperty(paramObject, "ProceedToAggregate") && paramObject.ProceedToAggregate != null && int.TryParse(paramObject.ProceedToAggregate.ToString(), out r))
                {
                    ProceedToAggregate = r;
                }

                DateTime dt = DateTime.Today;
                DateTime? InsuredDateOfBirth = null;
                if (Util.HasProperty(paramObject, "InsuredDateOfBirth") && paramObject.InsuredDateOfBirth != null && DateTime.TryParse(paramObject.InsuredDateOfBirth.ToString(), out dt))
                {
                    InsuredDateOfBirth = dt;
                }

                DateTime? ReinsuranceEffectiveDate = null;
                if (Util.HasProperty(paramObject, "ReinsEffDatePol") && paramObject.ReinsuranceEffectiveDate != null && DateTime.TryParse(paramObject.ReinsuranceEffectiveDate.ToString(), out dt))
                {
                    ReinsuranceEffectiveDate = dt;
                }

                DateTime? DateUpdated = null;
                if (Util.HasProperty(paramObject, "DateUpdated") && paramObject.DateUpdated != null && DateTime.TryParse(paramObject.DateUpdated.ToString(), out dt))
                {
                    DateUpdated = dt;
                }

                if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => q.TreatyCode == TreatyCode);
                if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => q.InsuredName.Contains(InsuredName));
                if (!string.IsNullOrEmpty(InsuredGender)) query = query.Where(q => q.InsuredGenderCode == InsuredGender);
                if (InsuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == InsuredDateOfBirth);
                if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber.Contains(PolicyNumber));
                if (ReinsuranceEffectiveDate.HasValue) query = query.Where(q => q.ReinsEffDatePol == ReinsuranceEffectiveDate);
                if (!string.IsNullOrEmpty(FundsAccountingType)) query = query.Where(q => q.FundsAccountingTypeCode == FundsAccountingType);
                if (!string.IsNullOrEmpty(ReinsuranceRiskBasis)) query = query.Where(q => q.ReinsBasisCode == ReinsuranceRiskBasis);
                if (!string.IsNullOrEmpty(CedingPlanCode)) query = query.Where(q => q.CedingPlanCode.Contains(CedingPlanCode));
                if (!string.IsNullOrEmpty(MLReBenefitCode)) query = query.Where(q => q.MlreBenefitCode == MLReBenefitCode);
                if (!string.IsNullOrEmpty(CedingBenefitRiskCode)) query = query.Where(q => q.CedingBenefitRiskCode.Contains(CedingBenefitRiskCode));
                if (!string.IsNullOrEmpty(CedingBenefitTypeCode)) query = query.Where(q => q.CedingBenefitTypeCode == CedingBenefitTypeCode);
                if (!string.IsNullOrEmpty(TransactionType)) query = query.Where(q => q.TransactionTypeCode == TransactionType);
                if (ProceedToAggregate.HasValue) query = query.Where(q => q.ProceedStatus == ProceedToAggregate);
                if (DateUpdated.HasValue) query = query.Where(q => q.UpdatedAt == DateUpdated);
                //if (ExceptionStatus.HasValue) query = query.Where(q => q.ExceptionType == ExceptionStatus);
                if (!string.IsNullOrEmpty(Remarks)) query = query.Where(q => q.Remarks.Contains(Remarks));
            }

            return query;
        }

        public static IQueryable<PerLifeAggregationDetailDataBo> GetPerLifeAggregationRiDataQuery(ref ExportBo exportBo, AppDbContext db)
        {
            if (exportBo.ObjectId == null)
                return null;

            var objectId = exportBo.ObjectId;
            var paramObject = exportBo.ParameterObject;
            var query = db.PerLifeAggregationDetailData.Where(q => q.PerLifeAggregationDetailTreaty.PerLifeAggregationDetailId == objectId).Select(PerLifeAggregationDetailDataService.Expression());

            if (paramObject != null)
            {
                // Search RI Data
                string SearchRiskMonth = Util.HasProperty(paramObject, "SearchRiskMonth") ? paramObject.SearchRiskMonth : null;
                string SearchTreatyCode = Util.HasProperty(paramObject, "SearchTreatyCode") ? paramObject.SearchTreatyCode : null;
                string SearchInsuredName = Util.HasProperty(paramObject, "SearchInsuredName") ? paramObject.SearchInsuredName : null;
                string SearchInsuredDateOfBirth = Util.HasProperty(paramObject, "SearchInsuredDateOfBirth") ? paramObject.SearchInsuredDateOfBirth : null;
                string SearchPolicyNumber = Util.HasProperty(paramObject, "SearchPolicyNumber") ? paramObject.SearchPolicyNumber : null;
                string SearchMlreBenefitCode = Util.HasProperty(paramObject, "SearchMlreBenefitCode") ? paramObject.SearchMlreBenefitCode : null;
                string SearchTransactionTypeCode = Util.HasProperty(paramObject, "SearchTransactionTypeCode") ? paramObject.SearchTransactionTypeCode : null;
                // Filter RI Data
                string TreatyCode = Util.HasProperty(paramObject, "TreatyCode") ? paramObject.TreatyCode : null;
                string CedingTreatyCode = Util.HasProperty(paramObject, "CedingTreatyCode") ? paramObject.CedingTreatyCode : null;
                string InsuredName = Util.HasProperty(paramObject, "InsuredName") ? paramObject.InsuredName : null;
                string InsuredGenderCode = Util.HasProperty(paramObject, "InsuredGenderCode") ? paramObject.InsuredGenderCode : null;
                string InsuredDateOfBirth = Util.HasProperty(paramObject, "InsuredDateOfBirth") ? paramObject.InsuredDateOfBirth : null;
                string PolicyNumber = Util.HasProperty(paramObject, "PolicyNumber") ? paramObject.PolicyNumber : null;
                string CurrencyCode = Util.HasProperty(paramObject, "CurrencyCode") ? paramObject.CurrencyCode : null;
                string TerritoryOfIssueCode = Util.HasProperty(paramObject, "TerritoryOfIssueCode") ? paramObject.TerritoryOfIssueCode : null;
                string CedingPlanCode = Util.HasProperty(paramObject, "CedingPlanCode") ? paramObject.CedingPlanCode : null;
                string CedingBenefitRiskCode = Util.HasProperty(paramObject, "CedingBenefitRiskCode") ? paramObject.CedingBenefitRiskCode : null;
                string CedingBenefitTypeCode = Util.HasProperty(paramObject, "CedingBenefitTypeCode") ? paramObject.CedingBenefitTypeCode : null;
                string StandardPremium = Util.HasProperty(paramObject, "StandardPremium") ? paramObject.StandardPremium : null;
                string SubstandardPremium = Util.HasProperty(paramObject, "SubstandardPremium") ? paramObject.SubstandardPremium : null;
                string StandardDiscount = Util.HasProperty(paramObject, "StandardDiscount") ? paramObject.StandardDiscount : null;
                string SubstandardDiscount = Util.HasProperty(paramObject, "SubstandardDiscount") ? paramObject.SubstandardDiscount : null;
                string FlatExtraPremium = Util.HasProperty(paramObject, "FlatExtraPremium") ? paramObject.FlatExtraPremium : null;
                string FlatExtraAmount = Util.HasProperty(paramObject, "FlatExtraAmount") ? paramObject.FlatExtraAmount : null;
                string BrokerageFee = Util.HasProperty(paramObject, "BrokerageFee") ? paramObject.BrokerageFee : null;
                string RiskPeriodStartDate = Util.HasProperty(paramObject, "RiskPeriodStartDate") ? paramObject.RiskPeriodStartDate : null;
                string RiskPeriodEndDate = Util.HasProperty(paramObject, "RiskPeriodEndDate") ? paramObject.RiskPeriodEndDate : null;
                string TransactionTypeCode = Util.HasProperty(paramObject, "TransactionTypeCode") ? paramObject.TransactionTypeCode : null;
                string EffectiveDate = Util.HasProperty(paramObject, "EffectiveDate") ? paramObject.EffectiveDate : null;
                string PolicyTerm = Util.HasProperty(paramObject, "PolicyTerm") ? paramObject.PolicyTerm : null;
                string PolicyExpiryDate = Util.HasProperty(paramObject, "PolicyExpiryDate") ? paramObject.PolicyExpiryDate : null;

                // Search RI Data
                int? searchRiskMonth = Util.GetParseInt(SearchRiskMonth);
                DateTime? searchInsuredDateOfBirth = Util.GetParseDateTime(SearchInsuredDateOfBirth);

                // Filter RI Data
                DateTime? insuredDateOfBirth = Util.GetParseDateTime(InsuredDateOfBirth);
                DateTime? riskPeriodStartDate = Util.GetParseDateTime(RiskPeriodStartDate);
                DateTime? riskPeriodEndDate = Util.GetParseDateTime(RiskPeriodEndDate);
                DateTime? effectiveDate = Util.GetParseDateTime(EffectiveDate);
                DateTime? policyExpiryDate = Util.GetParseDateTime(PolicyExpiryDate);

                double? standardPremium = Util.StringToDouble(StandardPremium);
                double? substandardPremium = Util.StringToDouble(SubstandardPremium);
                double? standardDiscount = Util.StringToDouble(StandardDiscount);
                double? substandardDiscount = Util.StringToDouble(SubstandardDiscount);
                double? flatExtraPremium = Util.StringToDouble(FlatExtraPremium);
                double? flatExtraAmount = Util.StringToDouble(FlatExtraAmount);
                double? brokerageFee = Util.StringToDouble(BrokerageFee);
                double? policyTerm = Util.StringToDouble(PolicyTerm);

                //Search Parameter
                if (searchRiskMonth.HasValue) query = query.Where(q => q.RiskPeriodMonth == searchRiskMonth);
                if (!string.IsNullOrEmpty(SearchTreatyCode)) query = query.Where(q => q.TreatyCode == SearchTreatyCode);
                if (!string.IsNullOrEmpty(SearchInsuredName)) query = query.Where(q => q.InsuredName == SearchInsuredName);
                if (searchInsuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == searchInsuredDateOfBirth);
                if (!string.IsNullOrEmpty(SearchPolicyNumber)) query = query.Where(q => q.PolicyNumber == SearchPolicyNumber);
                if (!string.IsNullOrEmpty(SearchMlreBenefitCode)) query = query.Where(q => q.MlreBenefitCode == SearchMlreBenefitCode);
                if (!string.IsNullOrEmpty(SearchTransactionTypeCode)) query = query.Where(q => q.TransactionTypeCode == SearchTransactionTypeCode);

                // Filter Parameters
                if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => q.TreatyCode == TreatyCode);
                if (!string.IsNullOrEmpty(CedingTreatyCode)) query = query.Where(q => q.CedingTreatyCode == CedingTreatyCode);
                if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => q.InsuredName == InsuredName);
                if (!string.IsNullOrEmpty(InsuredGenderCode)) query = query.Where(q => q.InsuredGenderCode == InsuredGenderCode);
                if (insuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == insuredDateOfBirth);
                if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber == PolicyNumber);
                if (!string.IsNullOrEmpty(CurrencyCode)) query = query.Where(q => q.CurrencyCode == CurrencyCode);
                if (!string.IsNullOrEmpty(InsuredGenderCode)) query = query.Where(q => q.InsuredGenderCode == InsuredGenderCode);
                if (!string.IsNullOrEmpty(TerritoryOfIssueCode)) query = query.Where(q => q.TerritoryOfIssueCode == TerritoryOfIssueCode);
                if (!string.IsNullOrEmpty(CedingPlanCode)) query = query.Where(q => q.CedingPlanCode == CedingPlanCode);
                if (!string.IsNullOrEmpty(CedingBenefitRiskCode)) query = query.Where(q => q.CedingBenefitRiskCode == CedingBenefitRiskCode);
                if (!string.IsNullOrEmpty(CedingBenefitTypeCode)) query = query.Where(q => q.CedingBenefitTypeCode == CedingBenefitTypeCode);
                if (standardPremium.HasValue) query = query.Where(q => q.StandardPremium == standardPremium);
                if (substandardPremium.HasValue) query = query.Where(q => q.SubstandardPremium == substandardPremium);
                if (standardDiscount.HasValue) query = query.Where(q => q.StandardDiscount == standardDiscount);
                if (substandardDiscount.HasValue) query = query.Where(q => q.SubstandardDiscount == substandardDiscount);
                if (flatExtraPremium.HasValue) query = query.Where(q => q.FlatExtraPremium == flatExtraPremium);
                if (flatExtraAmount.HasValue) query = query.Where(q => q.FlatExtraAmount == flatExtraAmount);
                if (brokerageFee.HasValue) query = query.Where(q => q.BrokerageFee == brokerageFee);
                if (riskPeriodStartDate.HasValue) query = query.Where(q => q.RiskPeriodStartDate == riskPeriodStartDate);
                if (riskPeriodEndDate.HasValue) query = query.Where(q => q.RiskPeriodEndDate == riskPeriodEndDate);
                if (!string.IsNullOrEmpty(TransactionTypeCode)) query = query.Where(q => q.TransactionTypeCode == TransactionTypeCode);
                if (effectiveDate.HasValue) query = query.Where(q => q.EffectiveDate == effectiveDate);
                if (policyTerm.HasValue) query = query.Where(q => q.PolicyTerm == policyTerm);
                if (policyExpiryDate.HasValue) query = query.Where(q => q.PolicyExpiryDate == policyExpiryDate);
            }

            return query;
        }

        public static IQueryable<PerLifeAggregationDetailDataBo> GetPerLifeAggregationExceptionQuery(ref ExportBo exportBo, AppDbContext db)
        {
            if (exportBo.ObjectId == null)
                return null;

            var objectId = exportBo.ObjectId;
            var paramObject = exportBo.ParameterObject;
            var query = db.PerLifeAggregationDetailData.Where(q => q.PerLifeAggregationDetailTreaty.PerLifeAggregationDetailId == objectId).Where(q => q.IsException == true).Select(PerLifeAggregationDetailDataService.Expression());

            if (paramObject != null)
            {
                string RecordType = Util.HasProperty(paramObject, "RecordType") ? paramObject.RecordType : null;
                string PolicyNumber = Util.HasProperty(paramObject, "PolicyNumber") ? paramObject.PolicyNumber : null;
                string IssueDatePol = Util.HasProperty(paramObject, "IssueDatePol") ? paramObject.IssueDatePol : null;
                string TreatyCode = Util.HasProperty(paramObject, "TreatyCode") ? paramObject.TreatyCode : null;
                string ReinsEffDatePol = Util.HasProperty(paramObject, "ReinsEffDatePol") ? paramObject.ReinsEffDatePol : null;
                string InsuredName = Util.HasProperty(paramObject, "InsuredName") ? paramObject.InsuredName : null;
                string InsuredGenderCode = Util.HasProperty(paramObject, "InsuredGenderCode") ? paramObject.InsuredGenderCode : null;
                string InsuredDateOfBirth = Util.HasProperty(paramObject, "InsuredDateOfBirth") ? paramObject.InsuredDateOfBirth : null;
                string CedingPlanCode = Util.HasProperty(paramObject, "CedingPlanCode") ? paramObject.CedingPlanCode : null;
                string CedingBenefitTypeCode = Util.HasProperty(paramObject, "CedingBenefitTypeCode") ? paramObject.CedingBenefitTypeCode : null;
                string CedingBenefitRiskCode = Util.HasProperty(paramObject, "CedingBenefitRiskCode") ? paramObject.CedingBenefitRiskCode : null;
                string MlreBenefitCode = Util.HasProperty(paramObject, "MlreBenefitCode") ? paramObject.MlreBenefitCode : null;
                string Aar = Util.HasProperty(paramObject, "Aar") ? paramObject.Aar : null;
                string ExceptionType = Util.HasProperty(paramObject, "ExceptionType") ? paramObject.ExceptionType : null;
                string ProceedStatus = Util.HasProperty(paramObject, "ProceedStatus") ? paramObject.ProceedStatus : null;

                int? recordType = Util.GetParseInt(RecordType);
                int? exceptionType = Util.GetParseInt(ExceptionType);
                int? proceedStatus = Util.GetParseInt(ProceedStatus);

                DateTime? issueDatePol = Util.GetParseDateTime(IssueDatePol);
                DateTime? reinsEffDatePol = Util.GetParseDateTime(ReinsEffDatePol);
                DateTime? insuredDateOfBirth = Util.GetParseDateTime(InsuredDateOfBirth);

                double? aar = Util.StringToDouble(Aar);

                if (recordType.HasValue) query = query.Where(q => q.RecordType == recordType);
                if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber == PolicyNumber);
                if (issueDatePol.HasValue) query = query.Where(q => q.IssueDatePol == issueDatePol);
                if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => q.TreatyCode == TreatyCode);
                if (reinsEffDatePol.HasValue) query = query.Where(q => q.ReinsEffDatePol == reinsEffDatePol);
                if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => q.InsuredName == InsuredName);
                if (!string.IsNullOrEmpty(InsuredGenderCode)) query = query.Where(q => q.InsuredGenderCode == InsuredGenderCode);
                if (insuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == insuredDateOfBirth);
                if (!string.IsNullOrEmpty(CedingPlanCode)) query = query.Where(q => q.CedingPlanCode == CedingPlanCode);
                if (!string.IsNullOrEmpty(CedingBenefitTypeCode)) query = query.Where(q => q.CedingBenefitTypeCode == CedingBenefitTypeCode);
                if (!string.IsNullOrEmpty(CedingBenefitRiskCode)) query = query.Where(q => q.CedingBenefitRiskCode == CedingBenefitRiskCode);
                if (!string.IsNullOrEmpty(MlreBenefitCode)) query = query.Where(q => q.MlreBenefitCode == MlreBenefitCode);
                if (aar.HasValue) query = query.Where(q => q.Aar == aar);
                if (exceptionType.HasValue) query = query.Where(q => q.ExceptionType == exceptionType);
                if (proceedStatus.HasValue) query = query.Where(q => q.ProceedStatus == proceedStatus);
            }

            return query;
        }

        public static IQueryable<PerLifeAggregationMonthlyDataBo> GetPerLifeAggregationRetroRiDataQuery(ref ExportBo exportBo, AppDbContext db)
        {
            if (exportBo.ObjectId == null)
                return null;

            var objectId = exportBo.ObjectId;
            var paramObject = exportBo.ParameterObject;
            var query = db.PerLifeAggregationMonthlyData.Where(q => q.PerLifeAggregationDetailData.PerLifeAggregationDetailTreaty.PerLifeAggregationDetailId == objectId).Select(PerLifeAggregationMonthlyDataService.Expression());

            if (paramObject != null)
            {
                string TreatyCode = Util.HasProperty(paramObject, "TreatyCode") ? paramObject.TreatyCode : null;
                string RiskYear = Util.HasProperty(paramObject, "RiskYear") ? paramObject.RiskYear : null;
                string RiskMonth = Util.HasProperty(paramObject, "RiskMonth") ? paramObject.RiskMonth : null;
                string InsuredGenderCode = Util.HasProperty(paramObject, "InsuredGenderCode") ? paramObject.InsuredGenderCode : null;
                string InsuredDateOfBirth = Util.HasProperty(paramObject, "InsuredDateOfBirth") ? paramObject.InsuredDateOfBirth : null;
                string PolicyNumber = Util.HasProperty(paramObject, "PolicyNumber") ? paramObject.PolicyNumber : null;
                string ReinsEffDatePol = Util.HasProperty(paramObject, "ReinsEffDatePol") ? paramObject.ReinsEffDatePol : null;
                string Aar = Util.HasProperty(paramObject, "Aar") ? paramObject.Aar : null;
                string GrossPremium = Util.HasProperty(paramObject, "GrossPremium") ? paramObject.GrossPremium : null;
                string NetPremium = Util.HasProperty(paramObject, "NetPremium") ? paramObject.NetPremium : null;
                string PremiumFrequencyCode = Util.HasProperty(paramObject, "PremiumFrequencyCode") ? paramObject.PremiumFrequencyCode : null;
                string RetroPremFreq = Util.HasProperty(paramObject, "RetroPremFreq") ? paramObject.RetroPremFreq : null;
                string CedingPlanCode = Util.HasProperty(paramObject, "CedingPlanCode") ? paramObject.CedingPlanCode : null;
                string CedingBenefitTypeCode = Util.HasProperty(paramObject, "CedingBenefitTypeCode") ? paramObject.CedingBenefitTypeCode : null;
                string CedingBenefitRiskCode = Util.HasProperty(paramObject, "CedingBenefitRiskCode") ? paramObject.CedingBenefitRiskCode : null;
                string MlreBenefitCode = Util.HasProperty(paramObject, "MlreBenefitCode") ? paramObject.MlreBenefitCode : null;
                string RetroBenefitCode = Util.HasProperty(paramObject, "RetroBenefitCode") ? paramObject.RetroBenefitCode : null;

                int? riskYear = Util.GetParseInt(RiskYear);
                int? riskMonth = Util.GetParseInt(RiskMonth);

                DateTime? insuredDateOfBirth = Util.GetParseDateTime(InsuredDateOfBirth);
                DateTime? reinsEffDatePol = Util.GetParseDateTime(ReinsEffDatePol);

                double? aar = Util.StringToDouble(Aar);
                double? grossPremium = Util.StringToDouble(GrossPremium);
                double? netPremium = Util.StringToDouble(NetPremium);

                if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => q.TreatyCode == TreatyCode);
                if (riskYear.HasValue) query = query.Where(q => q.RiskYear == riskYear);
                if (riskMonth.HasValue) query = query.Where(q => q.RiskMonth == riskMonth);
                if (!string.IsNullOrEmpty(InsuredGenderCode)) query = query.Where(q => q.InsuredGenderCode == InsuredGenderCode);
                if (insuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == insuredDateOfBirth);
                if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber == PolicyNumber);
                if (reinsEffDatePol.HasValue) query = query.Where(q => q.ReinsEffDatePol == reinsEffDatePol);
                if (aar.HasValue) query = query.Where(q => q.Aar == aar);
                if (grossPremium.HasValue) query = query.Where(q => q.GrossPremium == grossPremium);
                if (netPremium.HasValue) query = query.Where(q => q.NetPremium == netPremium);
                if (!string.IsNullOrEmpty(PremiumFrequencyCode)) query = query.Where(q => q.PremiumFrequencyCode == PremiumFrequencyCode);
                if (!string.IsNullOrEmpty(RetroPremFreq)) query = query.Where(q => q.RetroPremFreq == RetroPremFreq);
                if (!string.IsNullOrEmpty(CedingPlanCode)) query = query.Where(q => q.CedingPlanCode == CedingPlanCode);
                if (!string.IsNullOrEmpty(CedingBenefitTypeCode)) query = query.Where(q => q.CedingBenefitTypeCode == CedingBenefitTypeCode);
                if (!string.IsNullOrEmpty(CedingBenefitRiskCode)) query = query.Where(q => q.CedingBenefitRiskCode == CedingBenefitRiskCode);
                if (!string.IsNullOrEmpty(MlreBenefitCode)) query = query.Where(q => q.MlreBenefitCode == MlreBenefitCode);
                if (!string.IsNullOrEmpty(RetroBenefitCode)) query = query.Where(q => q.RetroBenefitCode == RetroBenefitCode);
            }

            return query;
        }

        public static IQueryable<PerLifeAggregationMonthlyDataBo> GetPerLifeAggregationRetentionPremiumQuery(ref ExportBo exportBo, AppDbContext db)
        {
            if (exportBo.ObjectId == null)
                return null;

            var objectId = exportBo.ObjectId;
            var paramObject = exportBo.ParameterObject;
            var query = db.PerLifeAggregationMonthlyData.Where(q => q.PerLifeAggregationDetailData.PerLifeAggregationDetailTreaty.PerLifeAggregationDetailId == objectId).Select(PerLifeAggregationMonthlyDataService.Expression());

            if (paramObject != null)
            {
                string UniqueKeyPerLife = Util.HasProperty(paramObject, "UniqueKeyPerLife") ? paramObject.UniqueKeyPerLife : null;
                string PolicyNumber = Util.HasProperty(paramObject, "PolicyNumber") ? paramObject.PolicyNumber : null;
                string InsuredGenderCode = Util.HasProperty(paramObject, "InsuredGenderCode") ? paramObject.InsuredGenderCode : null;
                string MlreBenefitCode = Util.HasProperty(paramObject, "MlreBenefitCode") ? paramObject.MlreBenefitCode : null;
                string RetroBenefitCode = Util.HasProperty(paramObject, "RetroBenefitCode") ? paramObject.RetroBenefitCode : null;
                string TerritoryOfIssueCode = Util.HasProperty(paramObject, "TerritoryOfIssueCode") ? paramObject.TerritoryOfIssueCode : null;
                string CurrencyCode = Util.HasProperty(paramObject, "CurrencyCode") ? paramObject.CurrencyCode : null;
                string InsuredTobaccoUse = Util.HasProperty(paramObject, "InsuredTobaccoUse") ? paramObject.InsuredTobaccoUse : null;
                string ReinsuranceIssueAge = Util.HasProperty(paramObject, "ReinsuranceIssueAge") ? paramObject.ReinsuranceIssueAge : null;
                string ReinsEffDatePol = Util.HasProperty(paramObject, "ReinsEffDatePol") ? paramObject.ReinsEffDatePol : null;
                string UnderwriterRating = Util.HasProperty(paramObject, "UnderwriterRating") ? paramObject.UnderwriterRating : null;
                string RetroPremFreq = Util.HasProperty(paramObject, "RetroPremFreq") ? paramObject.RetroPremFreq : null;
                string SumOfNetPremium = Util.HasProperty(paramObject, "SumOfNetPremium") ? paramObject.SumOfNetPremium : null;
                string NetPremium = Util.HasProperty(paramObject, "NetPremium") ? paramObject.NetPremium : null;
                string SumOfAar = Util.HasProperty(paramObject, "SumOfAar") ? paramObject.SumOfAar : null;
                string Aar = Util.HasProperty(paramObject, "Aar") ? paramObject.Aar : null;
                string RetentionLimit = Util.HasProperty(paramObject, "RetentionLimit") ? paramObject.RetentionLimit : null;
                string DistributedRetentionLimit = Util.HasProperty(paramObject, "DistributedRetentionLimit") ? paramObject.DistributedRetentionLimit : null;
                string RetroAmount = Util.HasProperty(paramObject, "RetroAmount") ? paramObject.RetroAmount : null;
                string DistributedRetroAmount = Util.HasProperty(paramObject, "DistributedRetroAmount") ? paramObject.DistributedRetroAmount : null;
                string AccumulativeRetainAmount = Util.HasProperty(paramObject, "AccumulativeRetainAmount") ? paramObject.AccumulativeRetainAmount : null;
                string Errors = Util.HasProperty(paramObject, "Errors") ? paramObject.Errors : null;

                int? reinsuranceIssueAge = Util.GetParseInt(ReinsuranceIssueAge);

                DateTime? reinsEffDatePol = Util.GetParseDateTime(ReinsEffDatePol);

                double? underwriterRating = Util.StringToDouble(UnderwriterRating);
                double? sumOfNetPremium = Util.StringToDouble(SumOfNetPremium);
                double? netPremium = Util.StringToDouble(NetPremium);
                double? sumOfAar = Util.StringToDouble(SumOfAar);
                double? aar = Util.StringToDouble(Aar);
                double? retentionLimit = Util.StringToDouble(RetentionLimit);
                double? distributedRetentionLimit = Util.StringToDouble(DistributedRetentionLimit);
                double? retroAmount = Util.StringToDouble(RetroAmount);
                double? distributedRetroAmount = Util.StringToDouble(DistributedRetroAmount);
                double? accumulativeRetainAmount = Util.StringToDouble(AccumulativeRetainAmount);

                //bool? errors = bool.TryParse(Errors);

                if (!string.IsNullOrEmpty(UniqueKeyPerLife)) query = query.Where(q => UniqueKeyPerLife.Contains(q.UniqueKeyPerLife));
                if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber == PolicyNumber);
                if (!string.IsNullOrEmpty(InsuredGenderCode)) query = query.Where(q => q.InsuredGenderCode == InsuredGenderCode);
                if (!string.IsNullOrEmpty(MlreBenefitCode)) query = query.Where(q => q.MlreBenefitCode == MlreBenefitCode);
                if (!string.IsNullOrEmpty(RetroBenefitCode)) query = query.Where(q => q.RetroBenefitCode == RetroBenefitCode);
                if (!string.IsNullOrEmpty(TerritoryOfIssueCode)) query = query.Where(q => q.TerritoryOfIssueCode == TerritoryOfIssueCode);
                if (!string.IsNullOrEmpty(CurrencyCode)) query = query.Where(q => q.CurrencyCode == CurrencyCode);
                if (!string.IsNullOrEmpty(InsuredTobaccoUse)) query = query.Where(q => q.InsuredTobaccoUse == InsuredTobaccoUse);
                if (reinsuranceIssueAge.HasValue) query = query.Where(q => q.ReinsuranceIssueAge == reinsuranceIssueAge);
                if (reinsEffDatePol.HasValue) query = query.Where(q => q.ReinsEffDatePol == reinsEffDatePol);
                if (underwriterRating.HasValue) query = query.Where(q => q.UnderwriterRating == underwriterRating);
                if (!string.IsNullOrEmpty(RetroPremFreq)) query = query.Where(q => q.RetroPremFreq == RetroPremFreq);
                if (sumOfNetPremium.HasValue) query = query.Where(q => q.SumOfNetPremium == sumOfNetPremium);
                if (netPremium.HasValue) query = query.Where(q => q.NetPremium == netPremium);
                if (sumOfAar.HasValue) query = query.Where(q => q.SumOfAar == sumOfAar);
                if (aar.HasValue) query = query.Where(q => q.Aar == aar);
                if (retentionLimit.HasValue) query = query.Where(q => q.RetentionLimit == retentionLimit);
                if (distributedRetentionLimit.HasValue) query = query.Where(q => q.DistributedRetentionLimit == distributedRetentionLimit);
                if (retroAmount.HasValue) query = query.Where(q => q.RetroAmount == retroAmount);
                if (distributedRetroAmount.HasValue) query = query.Where(q => q.DistributedRetroAmount == distributedRetroAmount);
                if (accumulativeRetainAmount.HasValue) query = query.Where(q => q.AccumulativeRetainAmount == accumulativeRetainAmount);
                //if (errors.HasValue && errors.Value == true) query = query.Where(q => q.Errors != null);
                //if (errors.HasValue && errors.Value == false) query = query.Where(q => q.Errors == null);
            }

            return query;
        }

        public static IQueryable<PerLifeAggregationDetailDataBo> GetPerLifeAggregationDetailDataQuery(ref ExportBo exportBo, AppDbContext db)
        {
            if (exportBo.ObjectId == null)
                return null;

            var objectId = exportBo.ObjectId;
            var paramObject = exportBo.ParameterObject;
            IQueryable<PerLifeAggregationDetailData> detailQuery = db.PerLifeAggregationDetailData;

            if (exportBo.Type == ExportBo.TypePerLifeAggregationRetroSummaryExcludedRecord)
                detailQuery = detailQuery.Where(q => q.PerLifeAggregationDetailTreaty.PerLifeAggregationDetail.PerLifeAggregationId == objectId);
            else if (exportBo.Type == ExportBo.TypePerLifeAggregationSummaryExcludedRecord)
                detailQuery = detailQuery.Where(q => q.PerLifeAggregationDetailTreaty.PerLifeAggregationDetailId == objectId);

            var query = detailQuery.Where(q => q.ProceedStatus != PerLifeAggregationDetailDataBo.ProceedStatusProceed)
                .Select(PerLifeAggregationDetailDataService.Expression());

            if (paramObject != null)
            {
                string TreatyCode = Util.HasProperty(paramObject, "TreatyCode") ? paramObject.TreatyCode : null;
                string PolicyNumber = Util.HasProperty(paramObject, "PolicyNumber") ? paramObject.PolicyNumber : null;
                string PolicyStatusCode = Util.HasProperty(paramObject, "PolicyStatusCode") ? paramObject.PolicyStatusCode : null;
                string Aar = Util.HasProperty(paramObject, "Aar") ? paramObject.Aar : null;
                string NetPremium = Util.HasProperty(paramObject, "NetPremium") ? paramObject.NetPremium : null;
                string PremiumFrequencyCode = Util.HasProperty(paramObject, "PremiumFrequencyCode") ? paramObject.PremiumFrequencyCode : null;
                string RiskPeriodMonth = Util.HasProperty(paramObject, "RiskPeriodMonth") ? paramObject.RiskPeriodMonth : null;
                string RiskPeriodYear = Util.HasProperty(paramObject, "RiskPeriodYear") ? paramObject.RiskPeriodYear : null;
                string LastUpdatedDate = Util.HasProperty(paramObject, "LastUpdatedDate") ? paramObject.LastUpdatedDate : null;
                string RiskPeriodStartDate = Util.HasProperty(paramObject, "RiskPeriodStartDate") ? paramObject.RiskPeriodStartDate : null;
                string RiskPeriodEndDate = Util.HasProperty(paramObject, "RiskPeriodEndDate") ? paramObject.RiskPeriodEndDate : null;
                string MlreBenefitCode = Util.HasProperty(paramObject, "MlreBenefitCode") ? paramObject.MlreBenefitCode : null;
                string ExceptionType = Util.HasProperty(paramObject, "ExceptionType") ? paramObject.ExceptionType : null;

                double? aar = Util.StringToDouble(Aar);
                double? netPremium = Util.StringToDouble(NetPremium);

                int? riskPeriodMonth = Util.GetParseInt(RiskPeriodMonth);
                int? riskPeriodYear = Util.GetParseInt(RiskPeriodYear);
                int? exceptionType = Util.GetParseInt(ExceptionType);

                DateTime? lastUpdatedDate = Util.GetParseDateTime(LastUpdatedDate);
                DateTime? riskPeriodStartDate = Util.GetParseDateTime(RiskPeriodStartDate);
                DateTime? riskPeriodEndDate = Util.GetParseDateTime(RiskPeriodEndDate);

                if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => q.TreatyCode == TreatyCode);
                if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber == PolicyNumber);
                if (!string.IsNullOrEmpty(PolicyStatusCode)) query = query.Where(q => q.PolicyStatusCode == PolicyStatusCode);
                if (aar.HasValue) query = query.Where(q => q.Aar == aar);
                if (netPremium.HasValue) query = query.Where(q => q.NetPremium == netPremium);
                if (!string.IsNullOrEmpty(PremiumFrequencyCode)) query = query.Where(q => q.PremiumFrequencyCode == PremiumFrequencyCode);
                if (riskPeriodMonth.HasValue) query = query.Where(q => q.RiskPeriodMonth == riskPeriodMonth);
                if (riskPeriodYear.HasValue) query = query.Where(q => q.RiskPeriodYear == riskPeriodYear);
                if (lastUpdatedDate.HasValue) query = query.Where(q => q.LastUpdatedDate == lastUpdatedDate);
                if (riskPeriodStartDate.HasValue) query = query.Where(q => q.RiskPeriodStartDate == riskPeriodStartDate);
                if (riskPeriodEndDate.HasValue) query = query.Where(q => q.RiskPeriodEndDate == riskPeriodEndDate);
                if (!string.IsNullOrEmpty(MlreBenefitCode)) query = query.Where(q => q.MlreBenefitCode == MlreBenefitCode);
                if (exceptionType.HasValue) query = query.Where(q => q.ExceptionType == exceptionType);
            }

            return query;
        }

        public static IQueryable<PerLifeAggregationMonthlyDataBo> GetPerLifeAggregationMonthlyDataQuery(ref ExportBo exportBo, AppDbContext db)
        {
            if (exportBo.ObjectId == null)
                return null;

            var objectId = exportBo.ObjectId;
            var paramObject = exportBo.ParameterObject;
            IQueryable<PerLifeAggregationMonthlyData> detailQuery = db.PerLifeAggregationMonthlyData;

            if (exportBo.Type == ExportBo.TypePerLifeAggregationRetroSummaryRetro)
                detailQuery = detailQuery.Where(q => q.PerLifeAggregationDetailData.PerLifeAggregationDetailTreaty.PerLifeAggregationDetail.PerLifeAggregationId == objectId);
            else if (exportBo.Type == ExportBo.TypePerLifeAggregationSummaryRetro)
                detailQuery = detailQuery.Where(q => q.PerLifeAggregationDetailData.PerLifeAggregationDetailTreaty.PerLifeAggregationDetailId == objectId);

            var query = detailQuery.Select(PerLifeAggregationMonthlyDataService.Expression());

            if (paramObject != null)
            {
                string TreatyCode = Util.HasProperty(paramObject, "TreatyCode") ? paramObject.TreatyCode : null;
                string ReinsBasisCode = Util.HasProperty(paramObject, "ReinsBasisCode") ? paramObject.ReinsBasisCode : null;
                string FundsAccountingTypeCode = Util.HasProperty(paramObject, "FundsAccountingTypeCode") ? paramObject.FundsAccountingTypeCode : null;
                string PremiumFrequencyCode = Util.HasProperty(paramObject, "PremiumFrequencyCode") ? paramObject.PremiumFrequencyCode : null;
                string ReportPeriodMonth = Util.HasProperty(paramObject, "ReportPeriodMonth") ? paramObject.ReportPeriodMonth : null;
                string ReportPeriodYear = Util.HasProperty(paramObject, "ReportPeriodYear") ? paramObject.ReportPeriodYear : null;
                string TransactionTypeCode = Util.HasProperty(paramObject, "TransactionTypeCode") ? paramObject.TransactionTypeCode : null;
                string PolicyNumber = Util.HasProperty(paramObject, "PolicyNumber") ? paramObject.PolicyNumber : null;
                string IssueDatePol = Util.HasProperty(paramObject, "IssueDatePol") ? paramObject.IssueDatePol : null;
                string IssueDateBen = Util.HasProperty(paramObject, "IssueDateBen") ? paramObject.IssueDateBen : null;
                string ReinsEffDatePol = Util.HasProperty(paramObject, "ReinsEffDatePol") ? paramObject.ReinsEffDatePol : null;
                string ReinsEffDateBen = Util.HasProperty(paramObject, "ReinsEffDateBen") ? paramObject.ReinsEffDateBen : null;
                string CedingPlanCode = Util.HasProperty(paramObject, "CedingPlanCode") ? paramObject.CedingPlanCode : null;
                string CedingBenefitTypeCode = Util.HasProperty(paramObject, "CedingBenefitTypeCode") ? paramObject.CedingBenefitTypeCode : null;
                string CedingBenefitRiskCode = Util.HasProperty(paramObject, "CedingBenefitRiskCode") ? paramObject.CedingBenefitRiskCode : null;
                string MlreBenefitCode = Util.HasProperty(paramObject, "MlreBenefitCode") ? paramObject.MlreBenefitCode : null;

                int? reportPeriodMonth = Util.GetParseInt(ReportPeriodMonth);
                int? reportPeriodYear = Util.GetParseInt(ReportPeriodYear);

                DateTime? issueDatePol = Util.GetParseDateTime(IssueDatePol);
                DateTime? issueDateBen = Util.GetParseDateTime(IssueDateBen);
                DateTime? reinsEffDatePol = Util.GetParseDateTime(ReinsEffDatePol);
                DateTime? reinsEffDateBen = Util.GetParseDateTime(ReinsEffDateBen);

                if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => q.TreatyCode == TreatyCode);
                if (!string.IsNullOrEmpty(ReinsBasisCode)) query = query.Where(q => q.ReinsBasisCode == ReinsBasisCode);
                if (!string.IsNullOrEmpty(FundsAccountingTypeCode)) query = query.Where(q => q.FundsAccountingTypeCode == FundsAccountingTypeCode);
                if (!string.IsNullOrEmpty(PremiumFrequencyCode)) query = query.Where(q => q.PremiumFrequencyCode == PremiumFrequencyCode);
                if (reportPeriodMonth.HasValue) query = query.Where(q => q.ReportPeriodMonth == reportPeriodMonth);
                if (reportPeriodYear.HasValue) query = query.Where(q => q.ReportPeriodYear == reportPeriodYear);
                if (!string.IsNullOrEmpty(TransactionTypeCode)) query = query.Where(q => q.TransactionTypeCode == TransactionTypeCode);
                if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber == PolicyNumber);
                if (issueDatePol.HasValue) query = query.Where(q => q.IssueDatePol == issueDatePol);
                if (issueDateBen.HasValue) query = query.Where(q => q.IssueDateBen == issueDateBen);
                if (reinsEffDatePol.HasValue) query = query.Where(q => q.ReinsEffDatePol == reinsEffDatePol);
                if (reinsEffDateBen.HasValue) query = query.Where(q => q.ReinsEffDateBen == reinsEffDateBen);
                if (!string.IsNullOrEmpty(CedingPlanCode)) query = query.Where(q => q.CedingPlanCode == CedingPlanCode);
                if (!string.IsNullOrEmpty(CedingBenefitTypeCode)) query = query.Where(q => q.CedingBenefitTypeCode == CedingBenefitTypeCode);
                if (!string.IsNullOrEmpty(CedingBenefitRiskCode)) query = query.Where(q => q.CedingBenefitRiskCode == CedingBenefitRiskCode);
                if (!string.IsNullOrEmpty(MlreBenefitCode)) query = query.Where(q => q.MlreBenefitCode == MlreBenefitCode);
            }

            return query;
        }

        public static IQueryable<PerLifeAggregationDetailTreatyBo> GetPerLifeAggregationDetailExlcudedRecordQuery(ref ExportBo exportBo, AppDbContext db)
        {
            if (exportBo.ObjectId == null)
                return null;

            var objectId = exportBo.ObjectId;
            IQueryable<PerLifeAggregationDetailData> detailQuery = db.PerLifeAggregationDetailData;

            if (exportBo.Type == ExportBo.TypePerLifeAggregationRetroSummaryExcludedRecord)
                detailQuery = detailQuery.Where(q => q.PerLifeAggregationDetailTreaty.PerLifeAggregationDetail.PerLifeAggregationId == objectId);
            else if (exportBo.Type == ExportBo.TypePerLifeAggregationSummaryExcludedRecord)
                detailQuery = detailQuery.Where(q => q.PerLifeAggregationDetailTreaty.PerLifeAggregationDetailId == objectId);

            var query = detailQuery.Where(q => q.ProceedStatus != PerLifeAggregationDetailDataBo.ProceedStatusProceed)
                .GroupBy(
                    q => q.PerLifeAggregationDetailTreatyId,
                    (key, DetailData) => new PerLifeAggregationDetailTreatyBo
                    {
                        RiskQuarter = DetailData.Select(q => q.PerLifeAggregationDetailTreaty.PerLifeAggregationDetail.RiskQuarter).FirstOrDefault(),
                        TreatyCode = DetailData.Select(q => q.PerLifeAggregationDetailTreaty.TreatyCode).FirstOrDefault(),
                        Count = DetailData.Count(),
                        TotalAar = DetailData.Sum(q => q.RiDataWarehouseHistory.Aar) ?? 0,
                        TotalGrossPremium = DetailData.Sum(q => q.RiDataWarehouseHistory.GrossPremium) ?? 0,
                        TotalNetPremium = DetailData.Sum(q => q.RiDataWarehouseHistory.NetPremium) ?? 0,
                    }
                )
                .Select(x => new PerLifeAggregationDetailTreatyBo
                {
                    RiskQuarter = x.RiskQuarter,
                    TreatyCode = x.TreatyCode,
                    Count = x.Count,
                    TotalAar = x.TotalAar,
                    TotalGrossPremium = x.TotalGrossPremium,
                    TotalNetPremium = x.TotalNetPremium,
                });

            return query;
        }

        public static IQueryable<PerLifeAggregationDetailTreatyBo> GetPerLifeAggregationDetailRetroQuery(ref ExportBo exportBo, AppDbContext db)
        {
            if (exportBo.ObjectId == null)
                return null;

            var objectId = exportBo.ObjectId;
            IQueryable<PerLifeAggregationMonthlyData> detailQuery = db.PerLifeAggregationMonthlyData;

            if (exportBo.Type == ExportBo.TypePerLifeAggregationRetroSummaryRetro)
                detailQuery = detailQuery.Where(q => q.PerLifeAggregationDetailData.PerLifeAggregationDetailTreaty.PerLifeAggregationDetail.PerLifeAggregationId == objectId);
            else if (exportBo.Type == ExportBo.TypePerLifeAggregationSummaryRetro)
                detailQuery = detailQuery.Where(q => q.PerLifeAggregationDetailData.PerLifeAggregationDetailTreaty.PerLifeAggregationDetailId == objectId);

            var query = detailQuery
                .GroupBy(
                    q => q.PerLifeAggregationDetailData.PerLifeAggregationDetailTreatyId,
                    (key, MonthlyData) => new PerLifeAggregationDetailTreatyBo
                    {
                        RiskQuarter = MonthlyData.Select(q => q.PerLifeAggregationDetailData.PerLifeAggregationDetailTreaty.PerLifeAggregationDetail.RiskQuarter).FirstOrDefault(),
                        TreatyCode = MonthlyData.Select(q => q.PerLifeAggregationDetailData.PerLifeAggregationDetailTreaty.TreatyCode).FirstOrDefault(),
                        Count = MonthlyData.Count(),
                        TotalAar = MonthlyData.Sum(q => q.Aar),
                        TotalGrossPremium = MonthlyData.Sum(q => q.RetroGrossPremium) ?? 0,
                        TotalNetPremium = MonthlyData.Sum(q => q.NetPremium),
                        Count2 = MonthlyData.Where(q => q.RetroIndicator == false).Count(),
                        TotalAar2 = MonthlyData.Where(q => q.RetroIndicator == false).Sum(q => q.Aar),
                        TotalGrossPremium2 = MonthlyData.Where(q => q.RetroIndicator == false).Sum(q => q.RetroGrossPremium) ?? 0,
                        TotalNetPremium2 = MonthlyData.Where(q => q.RetroIndicator == false).Sum(q => q.NetPremium),
                        Count3 = MonthlyData.Where(q => q.RetroIndicator == true).Count(),
                        TotalRetroAmount3 = MonthlyData.Where(q => q.RetroIndicator == true).Sum(q => q.RetroAmount) ?? 0,
                        TotalGrossPremium3 = MonthlyData.Where(q => q.RetroIndicator == true).Sum(q => q.PerLifeAggregationMonthlyRetroData.Sum(r => r.RetroGrossPremium)) ?? 0,
                        TotalNetPremium3 = MonthlyData.Where(q => q.RetroIndicator == true).Sum(q => q.PerLifeAggregationMonthlyRetroData.Sum(r => r.RetroNetPremium)) ?? 0,
                        TotalDiscount3 = MonthlyData.Where(q => q.RetroIndicator == true).Sum(q => q.PerLifeAggregationMonthlyRetroData.Sum(r => r.RetroDiscount)) ?? 0,
                    }
                )
                .Select(x => new PerLifeAggregationDetailTreatyBo
                {
                    RiskQuarter = x.RiskQuarter,
                    TreatyCode = x.TreatyCode,
                    Count = x.Count,
                    TotalAar = x.TotalAar,
                    TotalGrossPremium = x.TotalGrossPremium,
                    TotalNetPremium = x.TotalNetPremium,
                    Count2 = x.Count2,
                    TotalAar2 = x.TotalAar2,
                    TotalGrossPremium2 = x.TotalGrossPremium2,
                    TotalNetPremium2 = x.TotalNetPremium2,
                    Count3 = x.Count3,
                    TotalRetroAmount3 = x.TotalRetroAmount3,
                    TotalGrossPremium3 = x.TotalGrossPremium3,
                    TotalNetPremium3 = x.TotalNetPremium3,
                    TotalDiscount3 = x.TotalDiscount3,
                });

            return query;
        }

        public static IQueryable<TreatyPricingGroupReferralVersionBo> GetGroupReferralTrackingQuery(ref ExportBo exportBo, AppDbContext db)
        {
            var paramObject = exportBo.ParameterObject;
            var query = db.TreatyPricingGroupReferralVersions.GroupBy(p => p.TreatyPricingGroupReferralId)
                  .Select(g => g.OrderByDescending(p => p.Version).FirstOrDefault())
                  .Select(TreatyPricingGroupReferralVersionService.Expression());

            if (paramObject != null)
            {
                int r = 0;
                int? CedantId = null;
                if (Util.HasProperty(paramObject, "CedantId") && paramObject.CedantId != null && int.TryParse(paramObject.CedantId.ToString(), out r))
                {
                    CedantId = r;
                }

                DateTime dt = DateTime.Today;
                DateTime? CoverageStartAt = null;
                if (Util.HasProperty(paramObject, "CoverageStartDate") && paramObject.CoverageStartDate != null && DateTime.TryParse(paramObject.CoverageStartDate.ToString(), out dt))
                {
                    CoverageStartAt = dt;
                }

                DateTime? CoverageEndAt = null;
                if (Util.HasProperty(paramObject, "CoverageEndDate") && paramObject.CoverageEndDate != null && DateTime.TryParse(paramObject.CoverageEndDate.ToString(), out dt))
                {
                    CoverageEndAt = dt;
                }

                DateTime? RequestReceivedStartAt = null;
                if (Util.HasProperty(paramObject, "RequestReceivedStartDate") && paramObject.RequestReceivedStartDate != null && DateTime.TryParse(paramObject.RequestReceivedStartDate.ToString(), out dt))
                {
                    RequestReceivedStartAt = dt;
                }

                DateTime? RequestReceivedEndAt = null;
                if (Util.HasProperty(paramObject, "RequestReceivedEndDate") && paramObject.RequestReceivedEndDate != null && DateTime.TryParse(paramObject.RequestReceivedEndDate.ToString(), out dt))
                {
                    RequestReceivedEndAt = dt;
                }

                bool b = false;
                bool? IsCoverageBlankDate = null;
                if (Util.HasProperty(paramObject, "CoverageBlankDate") && paramObject.CoverageBlankDate != null && bool.TryParse(paramObject.CoverageBlankDate.ToString(), out b))
                {
                    IsCoverageBlankDate = b;
                }

                bool? IsRequestReceivedBlankDate = null;
                if (Util.HasProperty(paramObject, "RequestReceivedBlankDate") && paramObject.RequestReceivedBlankDate != null && bool.TryParse(paramObject.RequestReceivedBlankDate.ToString(), out b))
                {
                    IsRequestReceivedBlankDate = b;
                }

                if (CedantId.HasValue) query = query.Where(q => q.CedantId == CedantId.Value);
                if (CoverageStartAt.HasValue) query = query.Where(q => q.CoverageStartDate >= CoverageStartAt || q.CoverageStartDate.HasValue == !IsCoverageBlankDate);
                if (CoverageEndAt.HasValue) query = query.Where(q => q.CoverageEndDate <= CoverageEndAt || q.CoverageEndDate.HasValue == !IsCoverageBlankDate);
                if (!CoverageStartAt.HasValue && CoverageEndAt.HasValue) query = query.Where(q => q.CoverageStartDate.HasValue == !IsCoverageBlankDate && q.CoverageEndDate.HasValue == !IsCoverageBlankDate);
                if (RequestReceivedStartAt.HasValue) query = query.Where(q => q.RequestReceivedDate >= RequestReceivedStartAt || q.RequestReceivedDate.HasValue == !IsRequestReceivedBlankDate);
                if (RequestReceivedEndAt.HasValue) query = query.Where(q => q.RequestReceivedDate <= RequestReceivedEndAt || q.RequestReceivedDate.HasValue == !IsRequestReceivedBlankDate);
                if (!RequestReceivedStartAt.HasValue && RequestReceivedEndAt.HasValue) query = query.Where(q => q.RequestReceivedDate.HasValue == !IsRequestReceivedBlankDate);
            }

            return query;
        }

        public static IQueryable<FacMasterListingBo> GetFacMasterListingQuery(ref ExportBo exportBo, AppDbContext db)
        {
            var paramObject = exportBo.ParameterObject;
            var query = db.FacMasterListings.Select(FacMasterListingService.Expression());

            if (paramObject != null)
            {
                string UniqueId = Util.HasProperty(paramObject, "UniqueId") ? paramObject.UniqueId : null;
                string InsuredName = Util.HasProperty(paramObject, "InsuredName") ? paramObject.InsuredName : null;
                string PolicyNumber = Util.HasProperty(paramObject, "PolicyNumber") ? paramObject.PolicyNumber : null;
                string BenefitCode = Util.HasProperty(paramObject, "BenefitCode") ? paramObject.BenefitCode : null;
                string EwarpActionCode = Util.HasProperty(paramObject, "EwarpActionCode") ? paramObject.EwarpActionCode : null;
                string UwOpinion = Util.HasProperty(paramObject, "UwOpinion") ? paramObject.UwOpinion : null;
                string Remark = Util.HasProperty(paramObject, "Remark") ? paramObject.Remark : null;

                int r = 0;
                int? EwarpNumber = null;
                if (Util.HasProperty(paramObject, "EwarpNumber") && paramObject.EwarpNumber != null && int.TryParse(paramObject.EwarpNumber.ToString(), out r))
                {
                    EwarpNumber = r;
                }

                int? InsuredGenderCodeId = null;
                if (Util.HasProperty(paramObject, "InsuredGenderCodePickListDetailId") && paramObject.InsuredGenderCodePickListDetailId != null && int.TryParse(paramObject.InsuredGenderCodePickListDetailId.ToString(), out r))
                {
                    InsuredGenderCodeId = r;
                }

                int? CedantId = null;
                if (Util.HasProperty(paramObject, "CedantId") && paramObject.CedantId != null && int.TryParse(paramObject.CedantId.ToString(), out r))
                {
                    CedantId = r;
                }

                int? FlatExtraDuration = null;
                if (Util.HasProperty(paramObject, "FlatExtraDuration") && paramObject.FlatExtraDuration != null && int.TryParse(paramObject.FlatExtraDuration.ToString(), out r))
                {
                    FlatExtraDuration = r;
                }

                int? Type = null;
                if (Util.HasProperty(paramObject, "Type") && paramObject.Type != null && int.TryParse(paramObject.Type.ToString(), out r))
                {
                    Type = r;
                }

                DateTime dt = DateTime.Today;
                DateTime? InsuredDateOfBirth = null;
                if (Util.HasProperty(paramObject, "InsuredDateOfBirth") && paramObject.InsuredDateOfBirth != null && DateTime.TryParse(paramObject.InsuredDateOfBirth.ToString(), out dt))
                {
                    InsuredDateOfBirth = dt;
                }

                DateTime? OfferLetterSentDate = null;
                if (Util.HasProperty(paramObject, "OfferLetterSentDate") && paramObject.OfferLetterSentDate != null && DateTime.TryParse(paramObject.OfferLetterSentDate.ToString(), out dt))
                {
                    OfferLetterSentDate = dt;
                }

                double d = 0;
                double? FlatExtraAmountOffered = null;
                if (Util.HasProperty(paramObject, "FlatExtraAmountOffered") && paramObject.FlatExtraAmountOffered != null && double.TryParse(paramObject.FlatExtraAmountOffered.ToString(), out d))
                {
                    FlatExtraAmountOffered = d;
                }

                double? SumAssuredOffered = null;
                if (Util.HasProperty(paramObject, "SumAssuredOffered") && paramObject.SumAssuredOffered != null && double.TryParse(paramObject.SumAssuredOffered.ToString(), out d))
                {
                    SumAssuredOffered = d;
                }

                double? UwRatingOffered = null;
                if (Util.HasProperty(paramObject, "UwRatingOffered") && paramObject.UwRatingOffered != null && double.TryParse(paramObject.UwRatingOffered.ToString(), out d))
                {
                    UwRatingOffered = d;
                }

                if (!string.IsNullOrEmpty(UniqueId)) query = query.Where(q => q.UniqueId == UniqueId);
                if (EwarpNumber.HasValue) query = query.Where(q => q.EwarpNumber == EwarpNumber);
                if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => q.InsuredName.Contains(InsuredName));
                if (InsuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == InsuredDateOfBirth);
                if (InsuredGenderCodeId.HasValue) query = query.Where(q => q.InsuredGenderCodePickListDetailId == InsuredGenderCodeId);
                if (CedantId.HasValue) query = query.Where(q => q.CedantId == CedantId);
                if (!string.IsNullOrEmpty(PolicyNumber))
                {
                    var facMasterListingIds = db.FacMasterListingDetails.Where(q => q.PolicyNumber == PolicyNumber).Select(q => q.FacMasterListingId).ToList();
                    if (facMasterListingIds.Any()) query = query.Where(q => facMasterListingIds.Contains(q.Id));
                }
                if (FlatExtraAmountOffered.HasValue) query = query.Where(q => q.FlatExtraAmountOffered == FlatExtraAmountOffered);
                if (FlatExtraDuration.HasValue) query = query.Where(q => q.FlatExtraDuration == FlatExtraDuration);
                if (!string.IsNullOrEmpty(BenefitCode))
                {
                    var facMasterListingIds = db.FacMasterListingDetails.Where(q => q.BenefitCode == BenefitCode).Select(q => q.FacMasterListingId).ToList();
                    if (facMasterListingIds.Any()) query = query.Where(q => facMasterListingIds.Contains(q.Id));
                }
                if (SumAssuredOffered.HasValue) query = query.Where(q => q.SumAssuredOffered == SumAssuredOffered);
                if (!string.IsNullOrEmpty(EwarpActionCode)) query = query.Where(q => q.EwarpActionCode.Contains(EwarpActionCode));
                if (UwRatingOffered.HasValue) query = query.Where(q => q.UwRatingOffered == UwRatingOffered);
                if (OfferLetterSentDate.HasValue) query = query.Where(q => q.OfferLetterSentDate == OfferLetterSentDate);
                if (!string.IsNullOrEmpty(UwOpinion)) query = query.Where(q => q.UwOpinion.Contains(UwOpinion));
                if (!string.IsNullOrEmpty(Remark)) query = query.Where(q => q.Remark.Contains(Remark));
            }

            return query;
        }

        public static IQueryable<RateDetailBo> GetRateDetailQuery(ref ExportBo exportBo, AppDbContext db)
        {
            var paramObject = exportBo.ParameterObject;
            var query = db.RateDetails.Select(RateDetailService.Expression());

            if (paramObject != null)
            {
                int r = 0;
                int? RateId = null;
                if (Util.HasProperty(paramObject, "RateId") && paramObject.RateId != null && int.TryParse(paramObject.RateId.ToString(), out r))
                {
                    RateId = r;
                }

                if (RateId.HasValue) query = query.Where(q => q.RateId == RateId);
            }
            return query;
        }

        public static IList<ExportBo> Get()
        {
            return FormBos(Export.Get());
        }

        public static IList<ExportBo> GetByStatus(int? status = null, int? selectedId = null)
        {
            return FormBos(Export.GetByStatus(status, selectedId));
        }

        public static IList<ExportBo> GetFailedByHours()
        {
            return FormBos(Export.GetFailedByHours());
        }

        public static Result Save(ref ExportBo bo)
        {
            if (!Export.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref ExportBo bo, ref TrailObject trail)
        {
            if (!Export.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref ExportBo bo)
        {
            var entity = FormEntity(bo);
            var result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }
            return result;
        }

        public static Result Create(ref ExportBo bo, AppDbContext db, bool save = true)
        {
            var entity = FormEntity(bo);
            var result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create(db, save);
                bo.Id = entity.Id;
            }
            return result;
        }

        public static Result Create(ref ExportBo bo, ref TrailObject trail)
        {
            var result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Create(ref ExportBo bo, ref TrailObject trail, AppDbContext db, bool save = true)
        {
            var result = Create(ref bo, db, save);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ExportBo bo)
        {
            var result = Result();
            var entity = Export.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.Type = bo.Type;
                entity.Status = bo.Status;
                entity.ObjectId = bo.ObjectId;
                entity.Total = bo.Total;
                entity.Processed = bo.Processed;
                entity.Parameters = bo.Parameters;
                entity.GenerateStartAt = bo.GenerateStartAt;
                entity.GenerateEndAt = bo.GenerateEndAt;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref ExportBo bo, AppDbContext db, bool save = true)
        {
            var result = Result();
            var entity = Export.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.Type = bo.Type;
                entity.Status = bo.Status;
                entity.ObjectId = bo.ObjectId;
                entity.Total = bo.Total;
                entity.Processed = bo.Processed;
                entity.Parameters = bo.Parameters;
                entity.GenerateStartAt = bo.GenerateStartAt;
                entity.GenerateEndAt = bo.GenerateEndAt;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update(db, save);
            }
            return result;
        }

        public static Result Update(ref ExportBo bo, ref TrailObject trail)
        {
            var result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ExportBo bo, ref TrailObject trail, AppDbContext db, bool save = true)
        {
            var result = Update(ref bo, db, save);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ExportBo bo)
        {
            Export.Delete(bo.Id);
        }

        public static Result Delete(ExportBo bo, ref TrailObject trail)
        {
            var result = Result();

            if (result.Valid)
            {
                if (bo.IsFileExists())
                    File.Delete(bo.GetPath());

                var dataTrail = Export.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
