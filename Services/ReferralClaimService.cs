using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Services.Identity;
using Services.RiDatas;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ReferralClaimService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ReferralClaim)),
                Controller = ModuleBo.ModuleController.ReferralClaim.ToString(),
            };
        }

        public static ReferralClaimBo FormBo(ReferralClaim entity = null, bool foreign = true, bool formatOutput = false)
        {
            if (entity == null)
                return null;

            ReferralClaimBo referralClaimBo = new ReferralClaimBo()
            {
                Id = entity.Id,
                //ClaimRegisterId = entity.ClaimRegisterId,
                RiDataWarehouseId = entity.RiDataWarehouseId,
                ReferralRiDataId = entity.ReferralRiDataId,
                Status = entity.Status,
                StatusName = ReferralClaimBo.GetStatusName(entity.Status),
                ReferralId = entity.ReferralId,
                RecordType = entity.RecordType,
                InsuredName = entity.InsuredName,
                PolicyNumber = entity.PolicyNumber,
                InsuredGenderCode = entity.InsuredGenderCode,
                InsuredTobaccoUsage = entity.InsuredTobaccoUsage,
                ReferralReasonId = entity.ReferralReasonId,
                GroupName = entity.GroupName,
                DateReceivedFullDocuments = entity.DateReceivedFullDocuments,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                InsuredIcNumber = entity.InsuredIcNumber,
                DateOfCommencement = entity.DateOfCommencement,
                CedingCompany = entity.CedingCompany,
                ClaimCode = entity.ClaimCode,
                CedingPlanCode = entity.CedingPlanCode,
                SumInsured = entity.SumInsured,
                SumReinsured = entity.SumReinsured,
                BenefitSubCode = entity.BenefitSubCode,
                DateOfEvent = entity.DateOfEvent,
                RiskQuarter = entity.RiskQuarter,
                CauseOfEvent = entity.CauseOfEvent,
                MlreBenefitCode = entity.MlreBenefitCode,
                ClaimRecoveryAmount = entity.ClaimRecoveryAmount,
                ReinsBasisCode = entity.ReinsBasisCode,
                ClaimCategoryId = entity.ClaimCategoryId,
                IsRgalRetakaful = entity.IsRgalRetakaful,
                ReceivedAt = entity.ReceivedAt,
                RespondedAt = entity.RespondedAt,
                TurnAroundTime = entity.TurnAroundTime,
                DelayReasonId = entity.DelayReasonId,
                IsRetro = entity.IsRetro,
                RetrocessionaireName = entity.RetrocessionaireName,
                RetrocessionaireShare = entity.RetrocessionaireShare,
                RetroReferralReasonId = entity.RetroReferralReasonId,
                MlreReferralReasonId = entity.MlreReferralReasonId,
                RetroReviewedBy = entity.RetroReviewedBy,
                RetroReviewedAt = entity.RetroReviewedAt,
                IsValueAddedService = entity.IsValueAddedService,
                ValueAddedServiceDetails = entity.ValueAddedServiceDetails,
                IsClaimCaseStudy = entity.IsClaimCaseStudy,
                CompletedCaseStudyMaterialAt = entity.CompletedCaseStudyMaterialAt,
                AssessedById = entity.AssessedById,
                AssessedAt = entity.AssessedAt,
                AssessorComments = entity.AssessorComments,
                ReviewedById = entity.ReviewedById,
                ReviewedAt = entity.ReviewedAt,
                ReviewerComments = entity.ReviewerComments,
                ClaimsDecision = entity.ClaimsDecision,
                ClaimsDecisionDate = entity.ClaimsDecisionDate,
                AssignedById = entity.AssignedById,
                AssignedAt = entity.AssignedAt,
                TreatyCode = entity.TreatyCode,
                TreatyType = entity.TreatyType,
                TreatyShare = entity.TreatyShare,
                Checklist = entity.Checklist,
                Error = entity.Error,
                PersonInChargeId = entity.PersonInChargeId,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
                DocTurnAroundTime = entity.DocTurnAroundTime,
                DocRespondedAt = entity.DocRespondedAt,
                DocDelayReasonId = entity.DocDelayReasonId,
            };

            if (foreign)
            {
                //referralClaimBo.ClaimRegisterBo = ClaimRegisterService.Find(entity.ClaimRegisterId);
                referralClaimBo.RiDataWarehouseBo = RiDataWarehouseService.Find(entity.RiDataWarehouseId);
                //referralClaimBo.ClaimRegisterBo = ClaimRegisterService.Find(entity.ClaimRegisterId);
                referralClaimBo.ReferralReasonBo = ClaimReasonService.Find(entity.ReferralReasonId);
                referralClaimBo.ClaimCategoryBo = entity.ClaimCategoryId.HasValue ? ClaimCategoryService.Find(entity.ClaimCategoryId.Value) : null;
                referralClaimBo.DelayReasonBo = ClaimReasonService.Find(entity.DelayReasonId);
                referralClaimBo.RetroReferralReasonBo = ClaimReasonService.Find(entity.RetroReferralReasonId);
                referralClaimBo.MlreReferralReasonBo = ClaimReasonService.Find(entity.MlreReferralReasonId);
                referralClaimBo.AssessedByBo = UserService.Find(entity.AssessedById);
                referralClaimBo.ReviewedByBo = UserService.Find(entity.ReviewedById);
                referralClaimBo.AssignedByBo = UserService.Find(entity.AssignedById);
                referralClaimBo.PersonInChargeBo = UserService.Find(entity.PersonInChargeId);
            }

            if (formatOutput)
            {
                referralClaimBo.InsuredDateOfBirthStr = entity.InsuredDateOfBirth?.ToString(Util.GetDateFormat());
                referralClaimBo.RegisteredAtStr = entity.CreatedAt.ToString(Util.GetDateFormat());
                referralClaimBo.ClaimRecoveryAmountStr = Util.DoubleToString(entity.ClaimRecoveryAmount, 2);
            }

            return referralClaimBo;
        }

        public static IList<ReferralClaimBo> FormBos(IList<ReferralClaim> entities = null, bool foreign = true, bool formatOutput = false)
        {
            if (entities == null)
                return null;
            IList<ReferralClaimBo> bos = new List<ReferralClaimBo>() { };
            foreach (ReferralClaim entity in entities)
            {
                bos.Add(FormBo(entity, foreign, formatOutput));
            }
            return bos;
        }

        public static ReferralClaimBo FormBo(ClaimRegister entity = null)
        {
            if (entity == null)
                return null;

            ReferralClaimBo referralClaimBo = new ReferralClaimBo()
            {
                Id = entity.Id,
                ClaimId = entity.ClaimId,
                TotalRetroAmountStr = Util.DoubleToString((entity.RetroRecovery1 + entity.RetroRecovery2 + entity.RetroRecovery3),2),
                CauseOfEvent = entity.CauseOfEvent,
                PersonInChargeBo = UserService.Find(entity.PicClaimId),
                SumInsStr = Util.DoubleToString(entity.SumIns, 2),
                ReferralId = entity.ClaimId,
                InsuredName = entity.InsuredName,
                CedingCompany = entity.CedingCompany,
                PolicyNumber = entity.PolicyNumber,
                InsuredDateOfBirthStr = entity.InsuredDateOfBirth?.ToString(Util.GetDateFormat()),
                RegisteredAtStr = entity.DateOfRegister?.ToString(Util.GetDateFormat()),
                ClaimCode = entity.ClaimCode,
                TreatyCode = entity.TreatyCode,
                ClaimRecoveryAmountStr = Util.DoubleToString(entity.ClaimRecoveryAmt, 2),
                StatusName = ClaimRegisterBo.GetStatusName(entity.ClaimStatus),
            };

            return referralClaimBo;
        }

        public static IList<ReferralClaimBo> FormBos(IList<ClaimRegister> entities = null, bool foreign = true, bool formatOutput = false)
        {
            if (entities == null)
                return null;
            IList<ReferralClaimBo> bos = new List<ReferralClaimBo>() { };
            foreach (ClaimRegister entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static Expression<Func<ReferralClaim, ReferralClaimBo>> Expression()
        {
            return entity => new ReferralClaimBo
            {
                Id = entity.Id,
                //ClaimRegisterId = entity.ClaimRegisterId,
                RiDataWarehouseId = entity.RiDataWarehouseId,
                ReferralRiDataId = entity.ReferralRiDataId,
                Status = entity.Status,
                ReferralId = entity.ReferralId,
                RecordType = entity.RecordType,
                InsuredName = entity.InsuredName,
                PolicyNumber = entity.PolicyNumber,
                InsuredGenderCode = entity.InsuredGenderCode,
                InsuredTobaccoUsage = entity.InsuredTobaccoUsage,
                ReferralReasonId = entity.ReferralReasonId,
                GroupName = entity.GroupName,
                DateReceivedFullDocuments = entity.DateReceivedFullDocuments,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                InsuredIcNumber = entity.InsuredIcNumber,
                DateOfCommencement = entity.DateOfCommencement,
                CedingCompany = entity.CedingCompany,
                ClaimCode = entity.ClaimCode,
                CedingPlanCode = entity.CedingPlanCode,
                SumInsured = entity.SumInsured,
                SumReinsured = entity.SumReinsured,
                BenefitSubCode = entity.BenefitSubCode,
                DateOfEvent = entity.DateOfEvent,
                RiskQuarter = entity.RiskQuarter,
                CauseOfEvent = entity.CauseOfEvent,
                MlreBenefitCode = entity.MlreBenefitCode,
                ClaimRecoveryAmount = entity.ClaimRecoveryAmount,
                ReinsBasisCode = entity.ReinsBasisCode,
                ClaimCategoryId = entity.ClaimCategoryId,
                IsRgalRetakaful = entity.IsRgalRetakaful,
                ReceivedAt = entity.ReceivedAt,
                RespondedAt = entity.RespondedAt,
                TurnAroundTime = entity.TurnAroundTime,
                DelayReasonId = entity.DelayReasonId,
                IsRetro = entity.IsRetro,
                RetrocessionaireName = entity.RetrocessionaireName,
                RetrocessionaireShare = entity.RetrocessionaireShare,
                RetroReferralReasonId = entity.RetroReferralReasonId,
                MlreReferralReasonId = entity.MlreReferralReasonId,
                RetroReviewedBy = entity.RetroReviewedBy,
                RetroReviewedAt = entity.RetroReviewedAt,
                IsValueAddedService = entity.IsValueAddedService,
                ValueAddedServiceDetails = entity.ValueAddedServiceDetails,
                IsClaimCaseStudy = entity.IsClaimCaseStudy,
                CompletedCaseStudyMaterialAt = entity.CompletedCaseStudyMaterialAt,
                AssessedById = entity.AssessedById,
                AssessedAt = entity.AssessedAt,
                AssessorComments = entity.AssessorComments,
                ReviewedById = entity.ReviewedById,
                ReviewedAt = entity.ReviewedAt,
                ReviewerComments = entity.ReviewerComments,
                ClaimsDecision = entity.ClaimsDecision,
                ClaimsDecisionDate = entity.ClaimsDecisionDate,
                AssignedById = entity.AssignedById,
                AssignedAt = entity.AssignedAt,
                TreatyCode = entity.TreatyCode,
                TreatyType = entity.TreatyType,
                TreatyShare = entity.TreatyShare,
                Checklist = entity.Checklist,
                Error = entity.Error,
                PersonInChargeId = entity.PersonInChargeId,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
                DocTurnAroundTime = entity.DocTurnAroundTime,
                DocRespondedAt = entity.DocRespondedAt,
                DocDelayReasonId = entity.DocDelayReasonId,
            };
        }

        public static ReferralClaim FormEntity(ReferralClaimBo bo = null)
        {
            if (bo == null)
                return null;

            return new ReferralClaim()
            {
                Id = bo.Id,
                //ClaimRegisterId = bo.ClaimRegisterId,
                RiDataWarehouseId = bo.RiDataWarehouseId,
                ReferralRiDataId = bo.ReferralRiDataId,
                Status = bo.Status,
                ReferralId = bo.ReferralId,
                RecordType = bo.RecordType,
                InsuredName = bo.InsuredName,
                PolicyNumber = bo.PolicyNumber,
                InsuredGenderCode = bo.InsuredGenderCode,
                InsuredTobaccoUsage = bo.InsuredTobaccoUsage,
                ReferralReasonId = bo.ReferralReasonId,
                GroupName = bo.GroupName,
                DateReceivedFullDocuments = bo.DateReceivedFullDocuments,
                InsuredDateOfBirth = bo.InsuredDateOfBirth,
                InsuredIcNumber = bo.InsuredIcNumber,
                DateOfCommencement = bo.DateOfCommencement,
                CedingCompany = bo.CedingCompany,
                ClaimCode = bo.ClaimCode,
                CedingPlanCode = bo.CedingPlanCode,
                SumInsured = bo.SumInsured,
                SumReinsured = bo.SumReinsured,
                BenefitSubCode = bo.BenefitSubCode,
                DateOfEvent = bo.DateOfEvent,
                RiskQuarter = bo.RiskQuarter,
                CauseOfEvent = bo.CauseOfEvent,
                MlreBenefitCode = bo.MlreBenefitCode,
                ClaimRecoveryAmount = bo.ClaimRecoveryAmount,
                ReinsBasisCode = bo.ReinsBasisCode,
                ClaimCategoryId = bo.ClaimCategoryId,
                IsRgalRetakaful = bo.IsRgalRetakaful,
                ReceivedAt = bo.ReceivedAt,
                RespondedAt = bo.RespondedAt,
                TurnAroundTime = bo.TurnAroundTime,
                DelayReasonId = bo.DelayReasonId,
                IsRetro = bo.IsRetro,
                RetrocessionaireName = bo.RetrocessionaireName,
                RetrocessionaireShare = bo.RetrocessionaireShare,
                RetroReferralReasonId = bo.RetroReferralReasonId,
                MlreReferralReasonId = bo.MlreReferralReasonId,
                RetroReviewedBy = bo.RetroReviewedBy,
                RetroReviewedAt = bo.RetroReviewedAt,
                IsValueAddedService = bo.IsValueAddedService,
                ValueAddedServiceDetails = bo.ValueAddedServiceDetails,
                IsClaimCaseStudy = bo.IsClaimCaseStudy,
                CompletedCaseStudyMaterialAt = bo.CompletedCaseStudyMaterialAt,
                AssessedById = bo.AssessedById,
                AssessedAt = bo.AssessedAt,
                AssessorComments = bo.AssessorComments,
                ReviewedById = bo.ReviewedById,
                ReviewedAt = bo.ReviewedAt,
                ReviewerComments = bo.ReviewerComments,
                ClaimsDecision = bo.ClaimsDecision,
                ClaimsDecisionDate = bo.ClaimsDecisionDate,
                AssignedById = bo.AssignedById,
                AssignedAt = bo.AssignedAt,
                TreatyCode = bo.TreatyCode,
                TreatyType = bo.TreatyType,
                TreatyShare = bo.TreatyShare,
                Checklist = bo.Checklist,
                Error = bo.Error,
                PersonInChargeId = bo.PersonInChargeId,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
                DocTurnAroundTime = bo.DocTurnAroundTime,
                DocRespondedAt = bo.DocRespondedAt,
                DocDelayReasonId = bo.DocDelayReasonId,
            };
        }

        public static bool IsExists(int id)
        {
            return ReferralClaim.IsExists(id);
        }

        public static bool HasDuplicate(ReferralClaimBo bo)
        {
            using (var db = new AppDbContext())
            {
                return db.ReferralClaims
                    .Where(q => q.InsuredName == bo.InsuredName)
                    .Where(q => q.DateOfEvent == bo.DateOfEvent)
                    .Where(q => q.ClaimCode == bo.ClaimCode)
                    .Where(q => q.CauseOfEvent != bo.CauseOfEvent)
                    .Any();
            }
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.ReferralClaims.Where(q => q.Status == status).Count();
            }
        }

        public static IList<ReferralClaimBo> GetByStatus(int status, int skip, int take)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.ReferralClaims.Where(q => q.Status == status).OrderBy(q => q.Id).Skip(skip).Take(take).ToList(), false);
            }
        }

        public static int CountByClaimCategoryId(int claimCategoryId)
        {
            using (var db = new AppDbContext())
            {
                return db.ReferralClaims.Where(q => q.ClaimCategoryId == claimCategoryId).Count();
            }
        }

        public static IList<ReferralClaimBo> GetRelatedClaimRegisters(int id)
        {
            using (var db = new AppDbContext())
            {
                IList<ReferralClaimBo> bos = FormBos(db.ClaimRegister.Where(q => q.ReferralClaimId == id).ToList());
                return bos;
            }
        }


        public static IList<ReferralClaimBo> GetDuplicate(ReferralClaimBo bo, bool foreign = true, bool formatOutput = false)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ReferralClaims
                    .Where(q => q.Id != bo.Id)
                    .Where(q => q.PolicyNumber == bo.PolicyNumber)
                    .Where(q => q.InsuredName == bo.InsuredName)
                    .Where(q => (q.InsuredDateOfBirth.HasValue && q.InsuredDateOfBirth == bo.InsuredDateOfBirth) || (!string.IsNullOrEmpty(q.InsuredIcNumber) && q.InsuredIcNumber == bo.InsuredIcNumber))
                    .Where(q => q.TreatyCode == bo.TreatyCode)
                    .Where(q => q.ClaimCode == bo.ClaimCode)
                    .Where(q => q.CedingPlanCode == bo.CedingPlanCode);

                return FormBos(query.ToList(), foreign, formatOutput);
            }
        }

        public static ReferralClaimBo Find(int id)
        {
            return FormBo(ReferralClaim.Find(id));
        }

        public static ClaimRegisterBo FindAsClaimRegister(int id)
        {
            using (var db = new AppDbContext())
            {
                return ClaimRegisterService.FormBo(ReferralClaim.Find(id));
            }
        }

        public static ReferralClaimBo FindByReferralId(string referralId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ReferralClaims
                    .Where(q => !string.IsNullOrEmpty(q.ReferralId) && q.ReferralId == referralId);

                return FormBo(query.FirstOrDefault());
            }
        }

        public static ReferralClaimBo FindForClaimRegisterLink(string treatyCode, string insuredName, string claimCode)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ReferralClaims
                    .Where(q => q.TreatyCode == treatyCode)
                    .Where(q => q.InsuredName == insuredName)
                    .Where(q => q.ClaimCode == claimCode);

                return FormBo(query.FirstOrDefault());
            }
        }

        public static int GetCurrentReferralIdCount(DateTime today, string prefix)
        {
            using (var db = new AppDbContext())
            {
                ReferralClaim referralClaim = db.ReferralClaims.Where(q => !string.IsNullOrEmpty(q.ReferralId) && q.ReferralId.StartsWith(prefix)).OrderByDescending(q => q.ReferralId).FirstOrDefault();

                int count = 0;
                if (referralClaim != null)
                {
                    string referralId = referralClaim.ReferralId;
                    string[] referralIdInfo = referralId.Split('/');

                    string countStr;
                    if (referralIdInfo.Length == 4)
                    {
                        countStr = referralIdInfo[3];
                        int.TryParse(countStr, out count);
                    }
                }

                return count;
            }
        }

        public static double SumClaimByReferralClaim(ReferralClaimBo bo)
        {
            using (var db = new AppDbContext())
            {
                var referralClaims = db.ReferralClaims
                    .Where(q => q.MlreBenefitCode == BenefitBo.CodeHTH)
                    .Where(q => q.InsuredName == bo.InsuredName)
                    .Where(q => q.DateOfEvent == bo.DateOfEvent)
                    .Where(q => q.ClaimRecoveryAmount.HasValue)
                    .Where(q => !string.IsNullOrEmpty(q.ClaimCode))
                    .ToList();

                double totalClaim = 0;
                List<string> claimCodes = new List<string>();
                foreach (var referralClaim in referralClaims)
                {
                    if (claimCodes.Contains(referralClaim.ClaimCode))
                        continue;

                    totalClaim += referralClaim.ClaimRecoveryAmount.Value;
                    claimCodes.Add(referralClaim.ClaimCode);
                }

                foreach (var claimRegister in ClaimRegisterService.GetByReferralClaim(bo))
                {
                    if (claimCodes.Contains(claimRegister.ClaimCode))
                        continue;

                    totalClaim += claimRegister.ClaimRecoveryAmt.Value;
                    claimCodes.Add(claimRegister.ClaimCode);
                }

                return totalClaim;
            }
        }

        // Assigned Case Overview
        public static IList<ReferralClaimBo> GetAssignedCaseOverview()
        {
            List<int> statuses = ReferralClaimBo.GetOperationalDashboardDisplayStatus();
            using (var db = new AppDbContext(false))
            {
                var bos = db.ReferralClaims
                    .Where(q => q.PersonInChargeId != null)
                    .Where(q => statuses.Contains(q.Status))
                    .GroupBy(g => new { g.PersonInChargeId })
                    .Select(r => new ReferralClaimBo
                    {
                        PersonInChargeId = r.Key.PersonInChargeId,
                        NoOfCase = r.Count(),
                    })
                    .ToList();

                foreach (ReferralClaimBo bo in bos)
                {
                    bo.PersonInChargeBo = UserService.Find(bo.PersonInChargeId);
                }

                return bos;
            }
        }

        public static IList<ReferralClaimBo> GetAssignedCaseByPicId(int personInChargeId)
        {
            var statuses = ReferralClaimBo.GetOperationalDashboardDisplayStatus();

            using (var db = new AppDbContext(false))
            {
                var bos = db.ReferralClaims
                    .Where(q => q.PersonInChargeId == personInChargeId)
                    .Where(q => statuses.Contains(q.Status))
                    .GroupBy(g => new { g.Status })
                    .Select(r => new ReferralClaimBo
                    {
                        Status = r.Key.Status,
                        NoOfCase = r.Count(),
                    })
                    .OrderBy(q => q.Status)
                    .ToList();

                foreach (var bo in bos)
                {
                    bo.StatusName = ReferralClaimBo.GetStatusName(bo.Status);
                }

                return bos;
            }
        }

        public static int CountTotalAssignedCaseOverview()
        {
            List<int> statuses = ReferralClaimBo.GetOperationalDashboardDisplayStatus();
            using (var db = new AppDbContext(false))
            {
                return db.ReferralClaims
                    .Where(q => q.PersonInChargeId != null)
                    .Where(q => statuses.Contains(q.Status))
                    .Count();
            }
        }

        // Claims Operational Dashboard
        public static List<ReferralClaimBo> GetOperationalDashboard()
        {
            List<int> statuses = ReferralClaimBo.GetOperationalDashboardDisplayStatus();
            List<ReferralClaimBo> bos = new List<ReferralClaimBo>();
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.ReferralClaim.ToString());

            using (var db = new AppDbContext(false))
            {
                foreach (int status in statuses)
                {
                    ReferralClaimBo bo = new ReferralClaimBo();
                    List<int> caseIds = db.ReferralClaims.Where(q => q.Status == status).Select(q => q.Id).ToList();

                    bo.Status = status;
                    bo.StatusName = ReferralClaimBo.GetStatusName(status);
                    bo.NoOfCase = caseIds.Count();
                    if (caseIds != null && caseIds.Count > 0)
                    {
                        var histories = db.StatusHistories
                            .Where(q => q.ModuleId == moduleBo.Id)
                            .Where(q => q.Status == status)
                            .Where(q => caseIds.Contains(q.ObjectId))
                            .GroupBy(q => q.ObjectId)
                            .Select(g => g.OrderByDescending(q => q.Id).FirstOrDefault())
                            .ToList();

                        if (histories != null && histories.Count > 0)
                        {
                            int statusSlaDay = ClaimRegisterBo.GetStatusSlaDay(status);
                            bo.OverdueCase = histories.Where(q => (DateTime.Now.Date - q.CreatedAt.Date).Days > statusSlaDay).Count();
                            bo.MaxDay = histories.Max(q => (DateTime.Now.Date - q.CreatedAt.Date).Days);
                        }
                    }
                    bo.UnassignedCase = db.ReferralClaims
                            .Where(q => q.Status == status)
                            .Where(q => q.PersonInChargeId == null)
                            .Count();

                    bos.Add(bo);
                }
            }

            return bos;
        }

        // Pending Follow Up
        public static IList<ReferralClaimBo> GetPendingFollowUp(int? personInChargeId = null)
        {
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.ReferralClaim.ToString());
            List<ReferralClaimBo> bos = new List<ReferralClaimBo>();

            using (var db = new AppDbContext(false))
            {
                var remarkFollowUps = db.RemarkFollowUps
                    .Where(q => q.Remark.ModuleId == moduleBo.Id)
                    .Where(q => q.Status == RemarkFollowUpBo.StatusPending)
                    .GroupBy(q => q.Remark.ObjectId)
                    .Select(g => g.OrderByDescending(q => q.CreatedAt).FirstOrDefault())
                    .ToList();

                foreach (var remarkFollowUp in remarkFollowUps)
                {
                    var query = db.ReferralClaims.Where(q => q.Id == remarkFollowUp.Remark.ObjectId);

                    if (personInChargeId.HasValue)
                        query = query.Where(q => q.PersonInChargeId == personInChargeId);

                    var referralClaimBo = FormBo(query.FirstOrDefault());

                    if (referralClaimBo != null)
                    {
                        referralClaimBo.FollowUpWith = remarkFollowUp.FollowUpUser?.FullName;
                        referralClaimBo.FollowUpDateStr = remarkFollowUp.FollowUpAt?.ToString(Util.GetDateFormat());
                        referralClaimBo.Remark = Util.GetTruncatedValue(remarkFollowUp.Remark.Content, 60);
                        bos.Add(referralClaimBo);
                    }
                }

                return bos;
            }
        }

        // Pending Registration by Days
        public static int CountPendingRegistrationByDays(int day, bool isExceed = false, int? personInChargeId = null)
        {
            var date = DateTime.Today;
            var dayCount = 0;

            while (dayCount < day)
            {
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    date = date.AddDays(-1);
                    continue;
                }

                if (PublicHolidayDetailService.IsExists(date))
                {
                    date = date.AddDays(-1);
                    continue;
                }

                date = date.AddDays(-1);
                dayCount++;
            }

            using (var db = new AppDbContext(false))
            {
                var query = db.ReferralClaims.Where(q => q.Status != ReferralClaimBo.StatusClosedRegistered);

                if (isExceed)
                    query = query.Where(q => DbFunctions.TruncateTime(q.ReceivedAt) < DbFunctions.TruncateTime(date));
                else
                    query = query.Where(q => DbFunctions.TruncateTime(q.ReceivedAt) == DbFunctions.TruncateTime(date));

                if (personInChargeId.HasValue)
                    query = query.Where(q => q.PersonInChargeId.HasValue && q.PersonInChargeId == personInChargeId);

                return query.Count();
            }
        }

        public static IList<ReferralClaimBo> GetPendingRegistrationByDays(int day, bool isExceed = false, int? personInChargeId = null, int? take = null)
        {
            var date = DateTime.Today;
            var dayCount = 0;

            while (dayCount < day)
            {
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    date = date.AddDays(-1);
                    continue;
                }

                if (PublicHolidayDetailService.IsExists(date))
                {
                    date = date.AddDays(-1);
                    continue;
                }

                date = date.AddDays(-1);
                dayCount++;
            }

            using (var db = new AppDbContext(false))
            {
                var query = db.ReferralClaims.Where(q => q.Status != ReferralClaimBo.StatusClosedRegistered);

                if (isExceed)
                    query = query.Where(q => DbFunctions.TruncateTime(q.ReceivedAt) < DbFunctions.TruncateTime(date));
                else
                    query = query.Where(q => DbFunctions.TruncateTime(q.ReceivedAt) == DbFunctions.TruncateTime(date));

                if (personInChargeId.HasValue)
                    query = query.Where(q => q.PersonInChargeId.HasValue && q.PersonInChargeId == personInChargeId);

                if (take.HasValue)
                    query = query.Take(take.Value);

                return FormBos(query.ToList());
            }
        }

        public static int CountByFilterTatDay(int day)
        {
            using (var db = new AppDbContext(false))
            {
                long ticksStart;
                long ticks;
                switch (day)
                {
                    case ReferralClaimBo.FilterTat1Day:
                        ticks = (new TimeSpan(1, 0, 0, 0)).Ticks;
                        return db.ReferralClaims.Where(q => q.TurnAroundTime <= ticks).Count();
                    case ReferralClaimBo.FilterTat2Day:
                        ticksStart = (new TimeSpan(1, 0, 0, 0)).Ticks;
                        ticks = (new TimeSpan(2, 0, 0, 0)).Ticks;
                        return db.ReferralClaims.Where(q => q.TurnAroundTime > ticksStart).Where(q => q.TurnAroundTime <= ticks).Count();
                    case ReferralClaimBo.FilterTatMoreThan2Day:
                        ticks = (new TimeSpan(2, 0, 0, 0)).Ticks;
                        return db.ReferralClaims.Where(q => q.TurnAroundTime > ticks).Count();
                    default:
                        return 0;
                }
            }
        }

        public static ReferralClaimBo Find(int? id)
        {
            if (id.HasValue)
                return FormBo(ReferralClaim.Find(id.Value));
            return null;
        }

        public static Result Save(ref ReferralClaimBo bo)
        {
            if (!ReferralClaim.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref ReferralClaimBo bo, ref TrailObject trail)
        {
            if (!IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref ReferralClaimBo bo)
        {
            ReferralClaim entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref ReferralClaimBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ReferralClaimBo bo)
        {
            Result result = Result();

            ReferralClaim entity = ReferralClaim.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (!result.Valid)
                return result;

            entity = FormEntity(bo);
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

            result.DataTrail = entity.Update();
            return result;
        }

        public static Result Update(ref ReferralClaimBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ReferralClaimBo bo)
        {
            ReferralClaim.Delete(bo.Id);
        }

        public static Result Delete(ReferralClaimBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = ReferralClaim.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }
    }
}
