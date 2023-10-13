using BusinessObject;
using BusinessObject.Sanctions;
using DataAccess.Entities.Sanctions;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.Sanctions
{
    public class SanctionVerificationDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(SanctionVerificationDetail)),
                Controller = ModuleBo.ModuleController.SanctionVerificationDetail.ToString()
            };
        }

        public static Expression<Func<SanctionVerificationDetail, SanctionVerificationDetailBo>> Expression()
        {
            return entity => new SanctionVerificationDetailBo
            {
                Id = entity.Id,
                SanctionVerificationId = entity.SanctionVerificationId,
                Source = entity.SanctionVerification.Source.Name,
                ModuleId = entity.ModuleId,
                ObjectId = entity.ObjectId,
                Rule = entity.Rule,
                UploadDate = entity.UploadDate,
                Category = entity.Category,
                CedingCompany = entity.CedingCompany,
                TreatyCode = entity.TreatyCode,
                CedingPlanCode = entity.CedingPlanCode,
                PolicyNumber = entity.PolicyNumber,
                InsuredName = entity.InsuredName,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                InsuredIcNumber = entity.InsuredIcNumber,
                SoaQuarter = entity.SoaQuarter,
                SumReins = entity.SumReins,
                ClaimAmount = entity.ClaimAmount,
                IsWhitelist = entity.IsWhitelist,
                IsExactMatch = entity.IsExactMatch,
                BatchId = entity.BatchId,
                SanctionRefNumber = entity.SanctionRefNumber,
                SanctionIdNumber = entity.SanctionIdNumber,
                SanctionAddress = entity.SanctionAddress,
                LineOfBusiness = entity.LineOfBusiness,
                PolicyCommencementDate = entity.PolicyCommencementDate,
                PolicyStatusCode = entity.PolicyStatusCode,
                RiskCoverageEndDate = entity.RiskCoverageEndDate,
                GrossPremium = entity.GrossPremium,
                Remark = entity.Remark,
                PreviousDecision = entity.PreviousDecision,
                PreviousDecisionRemark = entity.PreviousDecisionRemark,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static SanctionVerificationDetailBo FormBo(SanctionVerificationDetail entity = null)
        {
            if (entity == null)
                return null;
            var bo = new SanctionVerificationDetailBo
            {
                Id = entity.Id,
                SanctionVerificationId = entity.SanctionVerificationId,
                ModuleId = entity.ModuleId,
                ObjectId = entity.ObjectId,
                Rule = entity.Rule,
                UploadDate = entity.UploadDate,
                Category = entity.Category,
                CedingCompany = entity.CedingCompany,
                TreatyCode = entity.TreatyCode,
                CedingPlanCode = entity.CedingPlanCode,
                PolicyNumber = entity.PolicyNumber,
                InsuredName = entity.InsuredName,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                InsuredIcNumber = entity.InsuredIcNumber,
                SoaQuarter = entity.SoaQuarter,
                SumReins = entity.SumReins,
                ClaimAmount = entity.ClaimAmount,
                IsWhitelist = entity.IsWhitelist,
                IsExactMatch = entity.IsExactMatch,
                BatchId = entity.BatchId,
                SanctionRefNumber = entity.SanctionRefNumber,
                SanctionIdNumber = entity.SanctionIdNumber,
                SanctionAddress = entity.SanctionAddress,
                LineOfBusiness = entity.LineOfBusiness,
                PolicyCommencementDate = entity.PolicyCommencementDate,
                PolicyStatusCode = entity.PolicyStatusCode,
                RiskCoverageEndDate = entity.RiskCoverageEndDate,
                GrossPremium = entity.GrossPremium,
                Remark = entity.Remark,
                PreviousDecision = entity.PreviousDecision,
                PreviousDecisionRemark = entity.PreviousDecisionRemark,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };

            bo.GetPreviousDecisionStr();
            return bo;
        }

        public static IList<SanctionVerificationDetailBo> FormBos(IList<SanctionVerificationDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<SanctionVerificationDetailBo> bos = new List<SanctionVerificationDetailBo>() { };
            foreach (SanctionVerificationDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static SanctionVerificationDetail FormEntity(SanctionVerificationDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new SanctionVerificationDetail
            {
                Id = bo.Id,
                SanctionVerificationId = bo.SanctionVerificationId,
                ModuleId = bo.ModuleId,
                ObjectId = bo.ObjectId,
                Rule = bo.Rule,
                UploadDate = bo.UploadDate,
                Category = bo.Category,
                CedingCompany = bo.CedingCompany,
                TreatyCode = bo.TreatyCode,
                CedingPlanCode = bo.CedingPlanCode,
                PolicyNumber = bo.PolicyNumber,
                InsuredName = bo.InsuredName,
                InsuredDateOfBirth = bo.InsuredDateOfBirth,
                InsuredIcNumber = bo.InsuredIcNumber,
                SoaQuarter = bo.SoaQuarter,
                SumReins = bo.SumReins,
                ClaimAmount = bo.ClaimAmount,
                IsWhitelist = bo.IsWhitelist,
                IsExactMatch = bo.IsExactMatch,
                BatchId = bo.BatchId,
                SanctionRefNumber = bo.SanctionRefNumber,
                SanctionIdNumber = bo.SanctionIdNumber,
                SanctionAddress = bo.SanctionAddress,
                LineOfBusiness = bo.LineOfBusiness,
                PolicyCommencementDate = bo.PolicyCommencementDate,
                PolicyStatusCode = bo.PolicyStatusCode,
                RiskCoverageEndDate = bo.RiskCoverageEndDate,
                GrossPremium = bo.GrossPremium,
                Remark = bo.Remark,
                PreviousDecision = bo.PreviousDecision,
                PreviousDecisionRemark = bo.PreviousDecisionRemark,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return SanctionVerificationDetail.IsExists(id);
        }

        public static bool IsExists(int sourceId, int moduleId, int objectId)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionVerificationDetails
                    .Where(q => q.SanctionVerification.Type == SanctionVerificationBo.TypeData)
                    .Where(q => q.SanctionVerification.SourceId == sourceId)
                    .Where(q => q.ModuleId == moduleId)
                    .Where(q => q.ObjectId == objectId)
                    .Any();
            }
        }

        public static bool IsAllExists(int sourceId, int moduleId, List<int> objectIds)
        {
            using (var db = new AppDbContext())
            {
                bool allExists = true;
                foreach (int objectId in objectIds)
                {
                    bool exists = db.SanctionVerificationDetails
                        .Where(q => q.SanctionVerification.Type == SanctionVerificationBo.TypeData)
                        .Where(q => q.SanctionVerification.SourceId == sourceId)
                        .Where(q => q.ModuleId == moduleId)
                        .Where(q => q.ObjectId == objectId)
                        .Any();

                    if (!exists)
                    {
                        allExists = false;
                        break;
                    }
                }

                return allExists;
            }
        }

        public static bool IsWhitelistOrExactMatch(int sourceId, int moduleId, int objectId)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionVerificationDetails
                    .Where(q => q.SanctionVerification.SourceId == sourceId)
                    .Where(q => q.ModuleId == moduleId)
                    .Where(q => q.ObjectId == objectId)
                    .Where(q => q.IsExactMatch || q.IsWhitelist)
                    .Any();
            }
        }

        public static SanctionVerificationDetailBo Find(int id)
        {
            return FormBo(SanctionVerificationDetail.Find(id));
        }

        public static SanctionVerificationDetailBo FindDataByObject(int sourceId, int moduleId, int objectId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.SanctionVerificationDetails
                    .Where(q => q.SanctionVerification.Type == SanctionVerificationBo.TypeData)
                    .Where(q => q.SanctionVerification.SourceId == sourceId)
                    .Where(q => q.ModuleId == moduleId)
                    .Where(q => q.ObjectId == objectId)
                    .FirstOrDefault());
            }
        }

        public static IList<SanctionVerificationDetailBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.SanctionVerificationDetails.ToList());
            }
        }

        public static int CountByParentId(int sanctionVerificationId, AppDbContext db)
        {
            return db.SanctionVerificationDetails
                .Where(q => q.SanctionVerificationId == sanctionVerificationId)
                .Count();
        }

        public static int CountUnprocessedByParentId(int sanctionVerificationId)
        {
            using (var db = new AppDbContext())
            {
                return CountUnprocessedByParentId(sanctionVerificationId, db);
            }
        }

        public static int CountUnprocessedByParentId(int sanctionVerificationId, AppDbContext db)
        {
            return db.SanctionVerificationDetails
                .Where(q => q.SanctionVerificationId == sanctionVerificationId)
                .Where(q => !q.IsExactMatch)
                .Where(q => !q.IsWhitelist)
                .Count();
        }

        public static IList<SanctionVerificationDetailBo> GetBySanctionVerificationId(int sanctionVerificationId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.SanctionVerificationDetails.Where(q => q.SanctionVerificationId == sanctionVerificationId).ToList());
            }
        }

        public static int CountByIsWhitelistIsExactMatch(bool isWhitelist = false, bool isExactMatch = false)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionVerificationDetails
                    .Where(q => q.IsWhitelist == isWhitelist)
                    .Where(q => q.IsExactMatch == isExactMatch)
                    .Count();
            }
        }

        public static Result Save(ref SanctionVerificationDetailBo bo)
        {
            if (!SanctionVerificationDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref SanctionVerificationDetailBo bo, ref TrailObject trail)
        {
            if (!SanctionVerificationDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref SanctionVerificationDetailBo bo)
        {
            SanctionVerificationDetail entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref SanctionVerificationDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void SetPreviousDecision(ref SanctionVerificationDetailBo bo)
        {
            if (string.IsNullOrEmpty(bo.InsuredName) || string.IsNullOrEmpty(bo.PolicyNumber))
                return;

            var sanctionWhitelistBo = SanctionWhitelistService.Find(bo.PolicyNumber, bo.InsuredName);
            if (sanctionWhitelistBo != null)
            {
                bo.PreviousDecision = SanctionVerificationDetailBo.PreviousDecisionWhitelist;
                bo.PreviousDecisionRemark = sanctionWhitelistBo.Reason;
                return;
            }
               
            bool isBlacklist = SanctionBlacklistService.IsExists(bo.PolicyNumber, bo.InsuredName);
            bo.PreviousDecision = isBlacklist ? SanctionVerificationDetailBo.PreviousDecisionExactMatch : SanctionVerificationDetailBo.PreviousDecisionPending;
        }

        public static Result Update(ref SanctionVerificationDetailBo bo)
        {
            Result result = Result();

            SanctionVerificationDetail entity = SanctionVerificationDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.SanctionVerificationId = bo.SanctionVerificationId;
                entity.ModuleId = bo.ModuleId;
                entity.ObjectId = bo.ObjectId;
                entity.Rule = bo.Rule;
                entity.UploadDate = bo.UploadDate;
                entity.Category = bo.Category;
                entity.CedingCompany = bo.CedingCompany;
                entity.TreatyCode = bo.TreatyCode;
                entity.CedingPlanCode = bo.CedingPlanCode;
                entity.PolicyNumber = bo.PolicyNumber;
                entity.InsuredName = bo.InsuredName;
                entity.InsuredDateOfBirth = bo.InsuredDateOfBirth;
                entity.InsuredIcNumber = bo.InsuredIcNumber;
                entity.SoaQuarter = bo.SoaQuarter;
                entity.SumReins = bo.SumReins;
                entity.ClaimAmount = bo.ClaimAmount;
                entity.IsWhitelist = bo.IsWhitelist;
                entity.IsExactMatch = bo.IsExactMatch;
                entity.BatchId = bo.BatchId;
                entity.SanctionRefNumber = bo.SanctionRefNumber;
                entity.SanctionIdNumber = bo.SanctionIdNumber;
                entity.SanctionAddress = bo.SanctionAddress;
                entity.LineOfBusiness = bo.LineOfBusiness;
                entity.PolicyCommencementDate = bo.PolicyCommencementDate;
                entity.PolicyStatusCode = bo.PolicyStatusCode;
                entity.RiskCoverageEndDate = bo.RiskCoverageEndDate;
                entity.GrossPremium = bo.GrossPremium;
                entity.Remark = bo.Remark;
                entity.PreviousDecision = bo.PreviousDecision;
                entity.PreviousDecisionRemark = bo.PreviousDecisionRemark;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref SanctionVerificationDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(SanctionVerificationDetailBo bo)
        {
            SanctionVerificationDetail.Delete(bo.Id);
        }

        public static Result Delete(SanctionVerificationDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                DataTrail dataTrail = SanctionVerificationDetail.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteBySanctionVerificationId(int SanctionVerificationId)
        {
            return SanctionVerificationDetail.DeleteBySanctionVerificationId(SanctionVerificationId);
        }

        public static void DeleteBySanctionVerificationId(int sanctionVerificationId, ref TrailObject trail)
        {
            Result result = Result();
            IList<DataTrail> dataTrails = DeleteBySanctionVerificationId(sanctionVerificationId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, result.Table);
                }
            }
        }
    }
}
