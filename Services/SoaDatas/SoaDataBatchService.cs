using BusinessObject;
using BusinessObject.Claims;
using BusinessObject.RiDatas;
using BusinessObject.SoaDatas;
using DataAccess.Entities.SoaDatas;
using DataAccess.EntityFramework;
using Services.Claims;
using Services.Identity;
using Services.InvoiceRegisters;
using Services.RiDatas;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Services.SoaDatas
{
    public class SoaDataBatchService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(SoaDataBatch)),
                Controller = ModuleBo.ModuleController.SoaDataBatch.ToString()
            };
        }

        public static SoaDataBatchBo FormBo(SoaDataBatch entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            var bo = new SoaDataBatchBo
            {
                Id = entity.Id,
                RiDataBatchId = entity.RiDataBatchId,
                ClaimDataBatchId = entity.ClaimDataBatchId,
                CedantId = entity.CedantId,
                TreatyId = entity.TreatyId,
                CurrencyCodePickListDetailId = entity.CurrencyCodePickListDetailId,
                CurrencyRate = entity.CurrencyRate,
                Status = entity.Status,
                DataUpdateStatus = entity.DataUpdateStatus,
                Quarter = entity.Quarter,
                Type = entity.Type,
                StatementReceivedAt = entity.StatementReceivedAt,
                DirectStatus = entity.DirectStatus,
                InvoiceStatus = entity.InvoiceStatus,
                TotalRecords = entity.TotalRecords,
                TotalMappingFailedStatus = entity.TotalMappingFailedStatus,
                IsRiDataAutoCreate = entity.IsAutoCreate,
                IsClaimDataAutoCreate = entity.IsClaimDataAutoCreate,
                IsProfitCommissionData = entity.IsProfitCommissionData,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
            bo.GetQuarterObject();
            if (foreign)
            {
                bo.CedantBo = CedantService.Find(entity.CedantId);
                bo.TreatyBo = TreatyService.Find(entity.TreatyId);
                bo.CurrencyCodePickListDetailBo = PickListDetailService.Find(entity.CurrencyCodePickListDetailId);
                bo.CreatedByBo = UserService.Find(entity.CreatedById);
                if (entity.ClaimDataBatchId.HasValue)
                    bo.ClaimDataBatchBo = ClaimDataBatchService.Find(entity.ClaimDataBatchId.Value);
                if (entity.RiDataBatchId.HasValue)
                    bo.RiDataBatchBo = RiDataBatchService.Find(entity.RiDataBatchId.Value);
            }
            return bo;
        }

        public static IList<SoaDataBatchBo> FormBos(IList<SoaDataBatch> entities = null)
        {
            if (entities == null)
                return null;
            IList<SoaDataBatchBo> bos = new List<SoaDataBatchBo>() { };
            foreach (SoaDataBatch entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static SoaDataBatch FormEntity(SoaDataBatchBo bo = null)
        {
            if (bo == null)
                return null;
            return new SoaDataBatch
            {
                Id = bo.Id,
                CedantId = bo.CedantId,
                TreatyId = bo.TreatyId,
                CurrencyCodePickListDetailId = bo.CurrencyCodePickListDetailId,
                CurrencyRate = bo.CurrencyRate,
                Status = bo.Status,
                DataUpdateStatus = bo.DataUpdateStatus,
                Quarter = bo.Quarter,
                Type = bo.Type,
                StatementReceivedAt = bo.StatementReceivedAt,
                DirectStatus = bo.DirectStatus,
                InvoiceStatus = bo.InvoiceStatus,
                RiDataBatchId = bo.RiDataBatchId,
                ClaimDataBatchId = bo.ClaimDataBatchId,
                TotalRecords = bo.TotalRecords,
                TotalMappingFailedStatus = bo.TotalMappingFailedStatus,
                IsAutoCreate = bo.IsRiDataAutoCreate,
                IsClaimDataAutoCreate = bo.IsClaimDataAutoCreate,
                IsProfitCommissionData = bo.IsProfitCommissionData,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return SoaDataBatch.IsExists(id);
        }

        public static SoaDataBatchBo Find(int? id)
        {
            return FormBo(SoaDataBatch.Find(id));
        }

        public static SoaDataBatchBo FindByStatus(int status)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.GetSoaDataBatches().Where(q => q.Status == status && (q.IsAutoCreate == false && q.IsClaimDataAutoCreate == false)).ToList();
                if (query.Count > 0)
                {
                    return FormBo(db.GetSoaDataBatches().Where(q => q.Status == status && (q.IsAutoCreate == false && q.IsClaimDataAutoCreate == false)).FirstOrDefault());
                }
                else
                {
                    List<int> outputRiStatus = new List<int> { RiDataBatchBo.StatusPreSuccess, RiDataBatchBo.StatusPreFailed, RiDataBatchBo.StatusPostSuccess, RiDataBatchBo.StatusPostFailed, RiDataBatchBo.StatusFinalised, RiDataBatchBo.StatusFinaliseFailed };
                    List<int> outputClaimStatus = new List<int> { ClaimDataBatchBo.StatusSuccess, ClaimDataBatchBo.StatusFailed, ClaimDataBatchBo.StatusReportedClaim, ClaimDataBatchBo.StatusReportingClaimFailed };
                    return FormBo(db.GetSoaDataBatches().Where(q => q.Status == status 
                        && ((q.IsAutoCreate == true && (q.RiDataBatchId != null && outputRiStatus.Contains(q.RiDataBatch.Status))) 
                        || (q.IsClaimDataAutoCreate == true && (q.ClaimDataBatchId != null && outputClaimStatus.Contains(q.ClaimDataBatch.Status))))).FirstOrDefault());
                }
            }
        }        

        public static SoaDataBatchBo FindByDataUpdateStatus(int dataUpdateStatus)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBo(db.GetSoaDataBatches().Where(q => q.DataUpdateStatus == dataUpdateStatus).FirstOrDefault());
            }
        }

        public static IList<SoaDataBatchBo> FindByCedantIdTreatyIdQuarter(int? cedantId, int? treatyId, string quarter)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.GetSoaDataBatches()
                    .Where(q => q.CedantId == cedantId)
                    .Where(q => q.Status == SoaDataBatchBo.StatusApproved || q.Status == SoaDataBatchBo.StatusConditionalApproval || q.Status == SoaDataBatchBo.StatusProvisionalApproval);

                if (treatyId.HasValue)
                    query = query.Where(q => q.TreatyId == treatyId);

                if (!string.IsNullOrEmpty(quarter))
                    query = query.Where(q => q.Quarter == quarter);

                return FormBos(query.OrderBy(q => q.Cedant.Name).ThenBy(q => q.Quarter).ToList());
            }
        }

        public static int FindIdByTreatyIdQuarterExceptStatus(int? treatyId, string quarter, int status)
        {
            using (var db = new AppDbContext(false))
            {
                return db.GetSoaDataBatches()
                    .Where(q => q.TreatyId == treatyId)
                    .Where(q => q.Quarter == quarter)
                    .Where(q => q.Status != status)
                    .Select(q => q.Id)
                    .FirstOrDefault();
            }
        }

        public static IList<SoaDataBatchBo> GetByIds(List<int> ids)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBos(db.GetSoaDataBatches().Where(q => ids.Contains(q.Id)).ToList());
            }
        }

        public static IList<SoaDataBatchBo> GetByParam(int? cedantId, int? treatyId, string quarter)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBos(SoaDataBatch.GetByParam(cedantId, treatyId, quarter));
            }
        }

        public static IList<SoaDataBatchBo> GetNotSOAApproved(int? cedantId, int? treatyId, string quarter)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.GetSoaDataBatches()
                    .Where(q => q.CedantId == cedantId)
                    .Where(q => q.TreatyId == treatyId)
                    .Where(q => q.Quarter == quarter)
                    .Where(q => q.Status != SoaDataBatchBo.StatusApproved)
                    .ToList());
            }
        }

        // SOA Data Overview
        public static IList<SoaDataBatchBo> GetTotalCaseGroupByCedantId()
        {
            using (var db = new AppDbContext())
            {
                var bos = db.GetSoaDataBatches()
                    .GroupBy(q => new { q.CedantId })
                    .Select(r => new SoaDataBatchBo
                    {
                        CedantId = r.Key.CedantId,
                        NoOfCase = r.Count(),
                    })
                    .ToList();

                foreach (var bo in bos)
                {
                    bo.CedantBo = CedantService.Find(bo.CedantId);
                }

                return bos;
            }
        }

        public static IList<SoaDataBatchBo> GetDetailByCedantIdGroupByStatusByTreatyId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                var bos = db.GetSoaDataBatches()
                    .Where(q => q.CedantId == cedantId)
                    .GroupBy(q => new { q.Status, q.TreatyId, q.Quarter })
                    .Select(r => new SoaDataBatchBo
                    {
                        Status = r.Key.Status,
                        TreatyId = r.Key.TreatyId,
                        Quarter = r.Key.Quarter,
                        NoOfCase = r.Count(),
                    })
                    .OrderBy(q => q.TreatyId)
                    .ThenBy(q => q.Status)
                    .ToList();

                foreach (var bo in bos)
                {
                    bo.TreatyBo = TreatyService.Find(bo.TreatyId);
                    bo.StatusName = SoaDataBatchBo.GetStatusName(bo.Status);
                }

                return bos;
            }
        }

        public static IList<SoaDataBatchBo> GetSoaTrackingOverview()
        {
            using (var db = new AppDbContext())
            {
                var bos = db.SoaData
                    .Where(q => q.SoaDataBatch.Status != SoaDataBatchBo.StatusApproved)
                    .Where(q => q.SoaDataBatch.Status != SoaDataBatchBo.StatusConditionalApproval)
                    .GroupBy(q => new { q.SoaDataBatch.CedantId })
                    .Select(r => new SoaDataBatchBo
                    {
                        CedantId = r.Key.CedantId,
                        TotalGrossPremium = r.Sum(q => q.GrossPremium)?? 0,
                    })
                    .ToList();

                foreach (var bo in bos)
                {
                    bo.CedantBo = CedantService.Find(bo.CedantId);
                    bo.NoOfCase = CountByCedantIdExceptStatuses(bo.CedantId, new List<int> { SoaDataBatchBo.StatusApproved, SoaDataBatchBo.StatusConditionalApproval });
                }

                return bos;
            }
        }

        public static IList<SoaDataBatchBo> GetSoaTrackingOverviewDetailByCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                var soaDataBatches = FormBos(db.GetSoaDataBatches()
                    .Where(q => q.CedantId == cedantId)
                    .Where(q => q.Status != SoaDataBatchBo.StatusApproved)
                    .Where(q => q.Status != SoaDataBatchBo.StatusConditionalApproval)
                    .ToList());

                foreach (var soaDataBatch in soaDataBatches)
                {
                    soaDataBatch.TotalGrossPremium = db.SoaData.Where(q => q.SoaDataBatchId == soaDataBatch.Id).Sum(q => q.GrossPremium) ?? 0;
                }

                return soaDataBatches;
            }
        }

        public static IList<SoaDataBatchBo> GetTotalBookedByQuarter(int year)
        {
            using (var db = new AppDbContext())
            {
                string yearStr = year.ToString();
                var bos = db.SoaData
                    .Where(q => q.SoaDataBatch.Quarter.Contains(yearStr))
                    .Where(q => q.SoaDataBatch.Status == SoaDataBatchBo.StatusApproved || q.SoaDataBatch.Status == SoaDataBatchBo.StatusConditionalApproval)
                    .GroupBy(q => new { q.SoaDataBatch.Quarter })
                    .Select(r => new SoaDataBatchBo
                    {
                        Quarter = r.Key.Quarter,
                        TotalGrossPremium = r.Sum(q => q.GrossPremium)?? 0,
                    })
                    .ToList();

                return bos;
            }
        }

        public static IList<SoaDataBatchBo> GetTotalBookedByCedant(int year)
        {
            using (var db = new AppDbContext())
            {
                string yearStr = year.ToString();
                var bos = db.SoaData
                    .Where(q => q.SoaDataBatch.Quarter.Contains(yearStr))
                    .Where(q => q.SoaDataBatch.Status == SoaDataBatchBo.StatusApproved || q.SoaDataBatch.Status == SoaDataBatchBo.StatusConditionalApproval)
                    .GroupBy(q => new { q.SoaDataBatch.CedantId })
                    .Select(r => new SoaDataBatchBo
                    {
                        CedantId = r.Key.CedantId,
                        TotalGrossPremium = r.Sum(q => q.GrossPremium)?? 0,
                    })
                    .ToList();

                foreach (var bo in bos)
                {
                    bo.CedantBo = CedantService.Find(bo.CedantId);
                }

                return bos;
            }
        }

        public static IList<SoaDataBatchBo> FindEmptyIdByStatusAutoCreate(int status)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBos(db.GetSoaDataBatches().Where(q => q.Status == status && ((q.IsAutoCreate == true && !q.RiDataBatchId.HasValue) || (q.IsClaimDataAutoCreate == true && !q.ClaimDataBatchId.HasValue))).ToList());

            }
        }

        public static IList<SoaDataBatchBo> FindNotExistIdByStatusAutoCreate(int status)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBos(db.GetSoaDataBatches().Where(q => q.Status == status && ((q.IsAutoCreate == true && q.RiDataBatchId.HasValue) || (q.IsClaimDataAutoCreate == true && q.ClaimDataBatchId.HasValue))).ToList());

            }
        }

        public static int Count()
        {
            using (var db = new AppDbContext(false))
            {
                return db.GetSoaDataBatches().Count();
            }
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext(false))
            {
                return db.GetSoaDataBatches().Where(q => q.Status == status).Count();
            }
        }

        public static int CountByCedantIdExceptStatuses(int cedantId, List<int> statuses)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.GetSoaDataBatches().Where(q => q.CedantId == cedantId);

                foreach (int status in statuses)
                {
                    query = query.Where(q => q.Status != status);
                }

                return query.Count();
            }
        }

        public static int CountByDataUpdateStatus(int dataUpdateStatus)
        {
            using (var db = new AppDbContext(false))
            {
                return db.GetSoaDataBatches().Where(q => q.DataUpdateStatus == dataUpdateStatus).Count();
            }
        }

        public static int CountByTreatyId(int treatyId)
        {
            using (var db = new AppDbContext(false))
            {
                return db.GetSoaDataBatches().Where(q => q.TreatyId == treatyId).Count();
            }
        }

        public static int CountByCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.GetSoaDataBatches().Where(q => q.CedantId == cedantId).Count();
            }
        }

        public static int CountByQuarterByExceptStatusApproved(string quarter)
        {
            using (var db = new AppDbContext())
            {
                List<int> approveStatus = new List<int> { SoaDataBatchBo.StatusApproved, SoaDataBatchBo.StatusConditionalApproval };
                return db.GetSoaDataBatches().Where(q => q.Quarter == quarter && !approveStatus.Contains(q.Status)).Count();
            }
        }

        public static bool FindByRiDataBatchId(int riDataBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.GetSoaDataBatches().Where(q => q.RiDataBatchId != riDataBatchId && q.RiDataBatch.Status != RiDataBatchBo.StatusPendingDelete).Any();
            }
        }

        public static void CountTotalFailed(ref SoaDataBatchBo bo, AppDbContext db)
        {
            bo.TotalMappingFailedStatus = RiDataService.CountByRiDataBatchIdMappingStatusFailed(bo.Id, db);
        }

        public static Result Save(ref SoaDataBatchBo bo)
        {
            if (!SoaDataBatch.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref SoaDataBatchBo bo, ref TrailObject trail)
        {
            if (!SoaDataBatch.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref SoaDataBatchBo bo)
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

        public static Result Create(ref SoaDataBatchBo bo, ref TrailObject trail)
        {
            var result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref SoaDataBatchBo bo)
        {
            var result = Result();

            var entity = SoaDataBatch.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            switch (bo.Status)
            {
                case SoaDataBatchBo.StatusPendingDelete:
                    if (InvoiceRegisterBatchSoaDataService.CountBySoaDataBatchId(bo.Id) > 0 || 
                        DirectRetroService.CountBySoaDataBatchId(bo.Id) > 0 ||  
                        InvoiceRegisterBatchSoaDataService.CountBySoaDataBatchId(bo.Id) > 0)
                    {
                        result.AddErrorRecordInUsed();
                    }
                    break;
                case SoaDataBatchBo.StatusSubmitForApproval:
                case SoaDataBatchBo.StatusConditionalApproval:
                    if (bo.ClaimDataBatchId.HasValue)
                    {
                        if (ClaimRegisterService.GetNotApproveClaimRegister(bo.ClaimDataBatchId.Value))
                            result.AddError("Pending for Claim Approval.");
                    }
                    else if (bo.RiDataBatchId.HasValue)
                    {
                        if (bo.RiDataBatchBo?.Status != RiDataBatchBo.StatusFinalised)
                            result.AddError("Pending for Ri Data to Submitted for finalise.");
                    }
                    break;
                case SoaDataBatchBo.StatusProvisionalApproval:
                    if (bo.RiDataBatchId.HasValue)
                    {
                        if (bo.RiDataBatchBo?.Status != RiDataBatchBo.StatusFinalised)
                            result.AddError("Pending for Ri Data to Submitted for finalise.");
                    }
                    break;
                case SoaDataBatchBo.StatusApproved:
                    if (bo.RiDataBatchId.HasValue)
                    {
                        if ((entity.Status == SoaDataBatchBo.StatusConditionalApproval || entity.Status == SoaDataBatchBo.StatusProvisionalApproval) 
                                && bo.RiDataBatchBo?.Status != RiDataBatchBo.StatusFinalised)
                            result.AddError("Pending for Ri Data to Submitted for finalise.");
                    }
                    else if (bo.ClaimDataBatchId.HasValue)
                    {
                        if (entity.Status == SoaDataBatchBo.StatusConditionalApproval
                                && ClaimRegisterService.GetNotApproveClaimRegister(bo.ClaimDataBatchId.Value))
                            result.AddError("Pending for Claim Approval.");
                    }
                    else if (!GetAuthorizationLimitByUserAccessGroup(bo.UpdatedById.Value))
                        result.AddError("You do not have the authority to approve this detail.");
                    else if (!CheckApprovalAllowed(bo.Id, bo.UpdatedById.Value))
                        result.AddError("Approval is not allowed due to exceeds the set amount.");
                    break;
            }


            if (result.Valid)
            {
                entity.CedantId = bo.CedantId;
                entity.TreatyId = bo.TreatyId;
                entity.CurrencyCodePickListDetailId = bo.CurrencyCodePickListDetailId;
                entity.CurrencyRate = bo.CurrencyRate;
                entity.Status = bo.Status;
                entity.DataUpdateStatus = bo.DataUpdateStatus;
                entity.Quarter = bo.Quarter;
                entity.Type = bo.Type;
                entity.StatementReceivedAt = bo.StatementReceivedAt;
                entity.DirectStatus = bo.DirectStatus;
                entity.InvoiceStatus = bo.InvoiceStatus;
                entity.RiDataBatchId = bo.RiDataBatchId;
                entity.ClaimDataBatchId = bo.ClaimDataBatchId;
                entity.TotalRecords = bo.TotalRecords;
                entity.TotalMappingFailedStatus = bo.TotalMappingFailedStatus;
                entity.IsAutoCreate = bo.IsRiDataAutoCreate;
                entity.IsClaimDataAutoCreate = bo.IsClaimDataAutoCreate;
                entity.IsProfitCommissionData = bo.IsProfitCommissionData;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref SoaDataBatchBo bo, ref TrailObject trail)
        {
            var result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(SoaDataBatchBo bo)
        {
            var moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.SoaDataBatch.ToString());
            var rawFileIds = new List<int> { };
            var soaDataFiles = SoaDataFileService.GetBySoaDataBatchId(bo.Id);
            foreach (SoaDataFileBo soaDataFile in soaDataFiles)
            {
                if (File.Exists(soaDataFile.RawFileBo.GetLocalPath()))
                {
                    File.Delete(soaDataFile.RawFileBo.GetLocalPath());
                    rawFileIds.Add(soaDataFile.RawFileId);
                }
            }

            SoaDataService.DeleteBySoaDataBatchId(bo.Id);
            SoaDataFileService.DeleteAllBySoaDataBatchId(bo.Id);
            RawFileService.DeleteByIds(rawFileIds);
            SoaDataBatchStatusFileService.DeleteAllBySoaDataBatchId(bo.Id);
            StatusHistoryService.DeleteAllByModuleIdObjectId(moduleBo.Id, bo.Id);
            RemarkService.DeleteAllByModuleIdObjectId(moduleBo.Id, bo.Id);

            SoaDataBatch.Delete(bo.Id);
        }

        public static Result Delete(SoaDataBatchBo bo, ref TrailObject trail)
        {
            var result = Result();
            var moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.SoaData.ToString());
            var rawFileIds = new List<int> { };
            var soaDataFiles = SoaDataFileService.GetBySoaDataBatchId(bo.Id);
            foreach (var soaDataFile in soaDataFiles)
            {
                if (File.Exists(soaDataFile.RawFileBo.GetLocalPath()))
                {
                    File.Delete(soaDataFile.RawFileBo.GetLocalPath());
                    rawFileIds.Add(soaDataFile.RawFileId);
                }
            }

            SoaDataService.DeleteBySoaDataBatchId(bo.Id);
            SoaDataFileService.DeleteAllBySoaDataBatchId(bo.Id, ref trail);
            RawFileService.DeleteByIds(rawFileIds, ref trail);
            SoaDataBatchStatusFileService.DeleteAllBySoaDataBatchId(bo.Id, ref trail);
            StatusHistoryService.DeleteAllByModuleIdObjectId(moduleBo.Id, bo.Id, ref trail);
            RemarkService.DeleteAllByModuleIdObjectId(moduleBo.Id, bo.Id, ref trail);

            var dataTrail = SoaDataBatch.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static bool GetAuthorizationLimitByUserAccessGroup(int authUserId)
        {
            var userBo = UserService.Find(authUserId);

            int userAccessGroupId = 0;
            if (userBo.UserAccessGroupBos != null && userBo.UserAccessGroupBos.Count > 0)
                userAccessGroupId = userBo.UserAccessGroupBos[0].AccessGroupId;

            var authorizationLimit = AuthorizationLimitService.GetByAccessGroupId(userAccessGroupId);
            if (authorizationLimit != null)
                return true;
            return false;
        }

        public static bool CheckApprovalAllowed(int soaDataBatchId, int authUserId)
        {            
            var userBo = UserService.Find(authUserId);

            int userAccessGroupId = 0;
            if (userBo.UserAccessGroupBos != null && userBo.UserAccessGroupBos.Count > 0)
                userAccessGroupId = userBo.UserAccessGroupBos[0].AccessGroupId;

            var authorizationLimit = AuthorizationLimitService.GetByAccessGroupId(userAccessGroupId);

            var bos = SoaDataPostValidationDifferenceService.GetBySoaDataBatchId(soaDataBatchId);
            var amounts = bos.GroupBy(x => x.TreatyCode).Select(x => new { GrossPremium = x.Sum(d => d.GrossPremium), DifferenceNetTotalAmount = x.Sum(d => d.DifferenceNetTotalAmount) }).ToList();
            foreach (var amount in amounts)
            {
                var totalGrossPrem = (authorizationLimit.Percentage.GetValueOrDefault() / 100) * amount.GrossPremium.GetValueOrDefault();
                if (amount.DifferenceNetTotalAmount > 0)
                {
                    if (!Between(totalGrossPrem, authorizationLimit.PositiveAmountFrom.GetValueOrDefault(), authorizationLimit.PositiveAmountTo.GetValueOrDefault()))
                        return false;
                }
                else if (amount.DifferenceNetTotalAmount < 0)
                {
                    if (!Between(totalGrossPrem, authorizationLimit.NegativeAmountFrom.GetValueOrDefault(), authorizationLimit.NegativeAmountTo.GetValueOrDefault()))
                        return false;
                }
                else if (amount.DifferenceNetTotalAmount == 0)
                {
                    if (!CedantWorkgroupUserService.IsUserExists(authUserId))
                        return false;
                }
            }
            return true;
        }

        public static bool Between(double amount, double amountMin, double amountMax)
        {
            return amount >= amountMin && amount <= amountMax;
        }

        public static IList<SoaDataBatchBo> GetProcessingFailedByHours(bool dataUpdate = false)
        {
            return FormBos(SoaDataBatch.GetProcessingFailedByHours(dataUpdate));
        }
    }
}
