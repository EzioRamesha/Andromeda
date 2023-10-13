using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Newtonsoft.Json;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.TreatyPricing
{
    public class TreatyPricingGroupReferralChecklistService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingGroupReferralChecklist)),
                Controller = ModuleBo.ModuleController.TreatyPricingGroupReferralChecklist.ToString()
            };
        }

        public static TreatyPricingGroupReferralChecklistBo FormBo(TreatyPricingGroupReferralChecklist entity = null, bool foreign = false)
        {
            if (entity == null)
                return null;
            return new TreatyPricingGroupReferralChecklistBo
            {
                Id = entity.Id,
                TreatyPricingGroupReferralVersionId = entity.TreatyPricingGroupReferralVersionId,
                TreatyPricingGroupReferralVersionBo = foreign ? TreatyPricingGroupReferralVersionService.Find(entity.TreatyPricingGroupReferralVersionId) : null,
                InternalTeam = entity.InternalTeam,
                InternalTeamPersonInCharge = entity.InternalTeamPersonInCharge,
                Status = entity.Status,
                DisableButtons = true,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                StatusName = TreatyPricingGroupReferralChecklistBo.GetStatusName(entity.Status),
            };
        }

        public static TreatyPricingGroupReferralChecklistBo FormBoForGroupDashboard(TreatyPricingGroupReferralChecklist entity = null, bool foreign = false)
        {
            if (entity == null)
                return null;
            return new TreatyPricingGroupReferralChecklistBo
            {
                Id = entity.Id,
                TreatyPricingGroupReferralVersionId = entity.TreatyPricingGroupReferralVersionId,
                TreatyPricingGroupReferralVersionBo = foreign ? TreatyPricingGroupReferralVersionService.FindForGroupDashboard(entity.TreatyPricingGroupReferralVersionId, true) : null,
                InternalTeam = entity.InternalTeam,
                InternalTeamPersonInCharge = entity.InternalTeamPersonInCharge,
                Status = entity.Status
            };
        }

        public static IList<TreatyPricingGroupReferralChecklistBo> FormBos(IList<TreatyPricingGroupReferralChecklist> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingGroupReferralChecklistBo> bos = new List<TreatyPricingGroupReferralChecklistBo>() { };
            foreach (TreatyPricingGroupReferralChecklist entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static IList<TreatyPricingGroupReferralChecklistBo> FormBosForGroupDashboard(IList<TreatyPricingGroupReferralChecklist> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingGroupReferralChecklistBo> bos = new List<TreatyPricingGroupReferralChecklistBo>() { };
            foreach (TreatyPricingGroupReferralChecklist entity in entities)
            {
                bos.Add(FormBoForGroupDashboard(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingGroupReferralChecklist FormEntity(TreatyPricingGroupReferralChecklistBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingGroupReferralChecklist
            {
                Id = bo.Id,
                TreatyPricingGroupReferralVersionId = bo.TreatyPricingGroupReferralVersionId,
                InternalTeam = bo.InternalTeam,
                InternalTeamPersonInCharge = bo.InternalTeamPersonInCharge,
                Status = bo.Status,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingGroupReferralChecklist.IsExists(id);
        }

        public static TreatyPricingGroupReferralChecklistBo Find(int id)
        {
            return FormBo(TreatyPricingGroupReferralChecklist.Find(id));
        }

        public static IList<TreatyPricingGroupReferralChecklistBo> Get(bool foreign = true)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBos(db.TreatyPricingGroupReferralChecklists.ToList(), foreign);
            }
        }

        public static IList<TreatyPricingGroupReferralChecklistBo> GetForGroupDashboard(bool foreign = true)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBosForGroupDashboard(db.TreatyPricingGroupReferralChecklists.ToList(), foreign);
            }
        }

        public static List<int> GetPendingInternalDashboard(UserBo authUser)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupReferralChecklists
                    .Where(a => a.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingApproval || a.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingReview)
                    .Where(a => !string.IsNullOrEmpty(a.InternalTeamPersonInCharge) && a.InternalTeamPersonInCharge.Contains(authUser.UserName))
                    .Select(a => a.TreatyPricingGroupReferralVersionId)
                    .Distinct()
                    .ToList();
            }
        }

        public static List<string> GetPendingInternalDashboardItem(int verId, UserBo user)
        {
            var pendingItemList = new List<string>();
            var dept = user.DepartmentBo.Id;
            int departmentUw = Util.GetConfigInteger("Underwriting");
            int departmentHealth = Util.GetConfigInteger("Health");
            int departmentClaims = Util.GetConfigInteger("Claims");
            int departmentBD = Util.GetConfigInteger("BDAndGroup");
            int departmentCR = Util.GetConfigInteger("ComplianceAndRisk");
            var pendingItems = new List<int>();

            using (var db = new AppDbContext())
            {
                if (dept == departmentUw)
                {
                     pendingItems = TreatyPricingGroupReferralChecklistDetailService.GetByTreatyPricingGroupReferralVersionId(verId)
                        .Where(a => a.Underwriting == true || a.UltimateApprover == user.Id)
                       .Select(a => a.InternalItem).ToList();
                } 
                else if (dept == departmentHealth)
                {
                     pendingItems = TreatyPricingGroupReferralChecklistDetailService.GetByTreatyPricingGroupReferralVersionId(verId)
                        .Where(a => a.Health == true || a.UltimateApprover == user.Id)
                       .Select(a => a.InternalItem).ToList();
                }
                else if (dept == departmentClaims)
                {
                     pendingItems = TreatyPricingGroupReferralChecklistDetailService.GetByTreatyPricingGroupReferralVersionId(verId)
                        .Where(a => a.Claim == true || a.UltimateApprover == user.Id)
                       .Select(a => a.InternalItem).ToList();
                }
                else if (dept == departmentBD)
                {
                     pendingItems = TreatyPricingGroupReferralChecklistDetailService.GetByTreatyPricingGroupReferralVersionId(verId)
                        .Where(a => a.BD == true || a.UltimateApprover == user.Id)
                       .Select(a => a.InternalItem).ToList();
                }
                else if (dept == departmentCR)
                {
                     pendingItems = TreatyPricingGroupReferralChecklistDetailService.GetByTreatyPricingGroupReferralVersionId(verId)
                        .Where(a => a.CnR == true || a.UltimateApprover == user.Id)
                       .Select(a => a.InternalItem).ToList();
                }

                if (pendingItems.Count == 0)
                {
                    var details = TreatyPricingGroupReferralChecklistDetailService.GetByTreatyPricingGroupReferralVersionId(verId);

                    if (details.Where(q => q.UltimateApprover == TreatyPricingGroupReferralChecklistDetailBo.UltimateApproverCEO).Count() != 0)
                    {
                        pendingItems = details.Where(a => a.CEOApprover == true)
                            .Select(a => a.InternalItem).ToList();
                    } 
                    else if (details.Where(q => q.UltimateApprover == TreatyPricingGroupReferralChecklistDetailBo.UltimateApproverHOD).Count() != 0)
                    {

                        pendingItems = details.Where(a => a.HODApprover == true)
                            .Select(a => a.InternalItem).ToList();
                    }
                    else if (details.Where(q => q.UltimateApprover == TreatyPricingGroupReferralChecklistDetailBo.UltimateApproverReviewer).Count() != 0)
                    {

                        pendingItems = details.Where(a => a.ReviewerApprover == true)
                            .Select(a => a.InternalItem).ToList();
                    }
                    else if (details.Where(q => q.UltimateApprover == TreatyPricingGroupReferralChecklistDetailBo.UltimateApproverGroup).Count() != 0)
                    {

                        pendingItems = details.Where(a => a.GroupTeamApprover == true)
                            .Select(a => a.InternalItem).ToList();
                    }

                }

                foreach (var pendingItem in pendingItems)
                {
                    pendingItemList.Add(pendingItem + " - " + TreatyPricingGroupReferralChecklistDetailBo.GetItemPovName(pendingItem));
                }

                return pendingItemList;
            }
        }

        public static IList<TreatyPricingGroupReferralChecklistBo> GetByTreatyPricingGroupReferralVersionId(int treatyPricingGroupReferralVersionId, bool foreign = false)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingGroupReferralChecklists
                    .Where(q => q.TreatyPricingGroupReferralVersionId == treatyPricingGroupReferralVersionId)
                    .ToList(), foreign);
            }
        }

        public static IList<TreatyPricingGroupReferralChecklistBo> GetByTreatyPricingGroupReferralVersionIdForGroupDashboard(int treatyPricingGroupReferralVersionId, bool foreign = false)
        {
            using (var db = new AppDbContext())
            {
                return FormBosForGroupDashboard(db.TreatyPricingGroupReferralChecklists
                    .Where(q => q.TreatyPricingGroupReferralVersionId == treatyPricingGroupReferralVersionId)
                    .ToList(), foreign);
            }
        }

        public static IList<TreatyPricingGroupReferralChecklistBo> GetByPendingReviewer(string reviewer, bool foreign = false)
        {
            var currentYear = DateTime.Now.Year;
            var yearStart = new DateTime(currentYear, 1, 1);
            var yearEnd = new DateTime(currentYear, 12, 31);

            var user = UserService.FindByFullName(reviewer.Trim());

            using (var db = new AppDbContext())
            {
                return FormBosForGroupDashboard(db.TreatyPricingGroupReferralChecklists
                    .Where(q => !string.IsNullOrEmpty(q.InternalTeamPersonInCharge) && q.InternalTeamPersonInCharge.Contains(user.UserName))
                    .Where(a => a.InternalTeam == TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamReviewer)
                    .Where(a => !a.TreatyPricingGroupReferralVersion.QuotationSentDate.HasValue)
                .Where(q => q.TreatyPricingGroupReferralVersion.RequestReceivedDate >= yearStart && q.TreatyPricingGroupReferralVersion.RequestReceivedDate <= yearEnd)
                .Where(q => q.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingApproval || q.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingReview)
                    .ToList(), foreign);
            }
        }

        public static IList<TreatyPricingGroupReferralChecklistBo> GetByPendingDepartment(int dept, bool foreign = false)
        {
            var currentYear = DateTime.Now.Year;
            var yearStart = new DateTime(currentYear, 1, 1);
            var yearEnd = new DateTime(currentYear, 12, 31);

            using (var db = new AppDbContext())
            {
                return FormBosForGroupDashboard(db.TreatyPricingGroupReferralChecklists
                    .Where(q => q.InternalTeam == dept)
                .Where(q => !string.IsNullOrEmpty(q.InternalTeamPersonInCharge))
                    .Where(a => !a.TreatyPricingGroupReferralVersion.QuotationSentDate.HasValue)
                //.Where(q => q.TreatyPricingGroupReferralVersion.GroupReferralPersonInChargeId.HasValue)
                .Where(q => q.TreatyPricingGroupReferralVersion.RequestReceivedDate >= yearStart && q.TreatyPricingGroupReferralVersion.RequestReceivedDate <= yearEnd)
                .Where(q => q.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingApproval || q.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingReview)
                    .ToList(), foreign);
            }
        }

        public static IList<TreatyPricingGroupReferralChecklistBo> GetByPendingHOD(string hod, bool foreign = false)
        {
            var currentYear = DateTime.Now.Year;
            var yearStart = new DateTime(currentYear, 1, 1);
            var yearEnd = new DateTime(currentYear, 12, 31);
            var user = UserService.FindByFullName(hod.Trim());

            using (var db = new AppDbContext())
            {
                return FormBosForGroupDashboard(db.TreatyPricingGroupReferralChecklists
                    .Where(q => !string.IsNullOrEmpty(q.InternalTeamPersonInCharge) && q.InternalTeamPersonInCharge.Contains(user.UserName))
                    .Where(q => q.InternalTeam == TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamHOD)
                    .Where(a => !a.TreatyPricingGroupReferralVersion.QuotationSentDate.HasValue)
                //.Where(q => q.TreatyPricingGroupReferralVersion.GroupReferralPersonInChargeId.HasValue)
                .Where(q => q.TreatyPricingGroupReferralVersion.RequestReceivedDate >= yearStart && q.TreatyPricingGroupReferralVersion.RequestReceivedDate <= yearEnd)
                .Where(q => q.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingApproval || q.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingReview)
                    .ToList(), foreign);
            }
        }

        public static IList<TreatyPricingGroupReferralChecklistBo> GetByPendingCEO(string ceo, bool foreign = false)
        {
            var currentYear = DateTime.Now.Year;
            var yearStart = new DateTime(currentYear, 1, 1);
            var yearEnd = new DateTime(currentYear, 12, 31);
            var user = UserService.FindByFullName(ceo.Trim());

            using (var db = new AppDbContext())
            {
                return FormBosForGroupDashboard(db.TreatyPricingGroupReferralChecklists
                    .Where(q => !string.IsNullOrEmpty(q.InternalTeamPersonInCharge) && q.InternalTeamPersonInCharge.Contains(user.UserName))
                    .Where(q => q.InternalTeam == TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamCEO)
                    .Where(a => !a.TreatyPricingGroupReferralVersion.QuotationSentDate.HasValue)
                //.Where(q => q.TreatyPricingGroupReferralVersion.GroupReferralPersonInChargeId.HasValue)
                .Where(q => q.TreatyPricingGroupReferralVersion.RequestReceivedDate >= yearStart && q.TreatyPricingGroupReferralVersion.RequestReceivedDate <= yearEnd)
                .Where(q => q.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingApproval || q.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingReview)
                    .ToList(), foreign);
            }
        }



        public static IList<TreatyPricingGroupReferralChecklistBo> GetGroupOverallTatCount(int year)
        {
            var yearStart = new DateTime(year, 1, 1);
            var yearEnd = new DateTime(year, 12, 31);
            
            var checklistBos = GetForGroupDashboard(true)
                .Where(q => !string.IsNullOrEmpty(q.InternalTeamPersonInCharge))
                .Where(q => q.TreatyPricingGroupReferralVersionBo.RequestReceivedDate >= yearStart && q.TreatyPricingGroupReferralVersionBo.RequestReceivedDate <= yearEnd);

            return checklistBos.ToList();
        }
        public static IList<TreatyPricingGroupReferralChecklistBo> GetGroupOverallTatCountForDepartment(int year)
        {
            var yearStart = new DateTime(year, 1, 1);
            var yearEnd = new DateTime(year, 12, 31);

            var status = new List<int> { TreatyPricingGroupReferralChecklistBo.StatusPendingApproval, TreatyPricingGroupReferralChecklistBo.StatusPendingReview };
            var allTeams = Enumerable.Range(1, TreatyPricingGroupReferralChecklistBo.MaxDefaultInternalTeam).ToList();
            var teams = allTeams
                .Where(q => q != TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamApprover)
                .Where(q => q != TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamGroup)
                .ToList();

            var checklistBos = Get()
                .Where(q => !string.IsNullOrEmpty(q.InternalTeamPersonInCharge))
                //.Where(q => teams.Contains(q.InternalTeam))
                //.Where(q => q.Status != TreatyPricingGroupReferralChecklistBo.StatusNotRequired)
                //.Where(q => q.TreatyPricingGroupReferralVersionBo.QuotationSentDate.HasValue)
                //.Where(q => q.TreatyPricingGroupReferralVersionBo.ClientReplyDate.HasValue)
                //.Where(q => q.TreatyPricingGroupReferralVersionBo.EnquiryToClientDate.HasValue)
                //.Where(q => q.TreatyPricingGroupReferralVersionBo.GroupReferralPersonInChargeId.HasValue)
                .Where(q => q.TreatyPricingGroupReferralVersionBo.RequestReceivedDate >= yearStart && q.TreatyPricingGroupReferralVersionBo.RequestReceivedDate <= yearEnd);

            return checklistBos.ToList();
        }

        public static IList<int> GetIdsByInternalTeam(int internalTeam, string internalTeamPIC, int status, bool pendingDept = false)
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(internalTeamPIC))
                {
                    if (!pendingDept)
                    {
                        var query = (from a in db.TreatyPricingGroupReferralChecklists
                                     where a.InternalTeamPersonInCharge.Contains(internalTeamPIC) && a.InternalTeam == internalTeam && a.Status == status
                                     select a).ToList()
                                     .Where(a => a.InternalTeamPersonInCharge.Split(',').Contains(internalTeamPIC));

                        return query.Select(q => q.TreatyPricingGroupReferralVersion.TreatyPricingGroupReferralId).Distinct().ToList();
                    }
                    else
                    {
                        var query = (from a in db.TreatyPricingGroupReferralChecklists
                                     where a.InternalTeamPersonInCharge.Contains(internalTeamPIC) && a.InternalTeam == internalTeam 
                                     && a.TreatyPricingGroupReferralVersion.TreatyPricingGroupReferral.Status == TreatyPricingGroupReferralBo.StatusQuoting
                                     && a.TreatyPricingGroupReferralVersion.TreatyPricingGroupReferral.WorkflowStatus != TreatyPricingGroupReferralBo.WorkflowStatusPendingClient
                                     select a).ToList()
                                     .Where(a => a.InternalTeamPersonInCharge.Split(',').Contains(internalTeamPIC));

                        return query.Select(q => q.TreatyPricingGroupReferralVersion.TreatyPricingGroupReferralId).Distinct().ToList();
                    }
                }
                else
                {
                    return Get()
                        .Where(q => q.InternalTeam == internalTeam)
                        .Where(q => q.Status == status)
                        .Select(q => q.TreatyPricingGroupReferralVersionBo.TreatyPricingGroupReferralId)
                        .Distinct().ToList();
                }
            }
        }

        public static Result Save(ref TreatyPricingGroupReferralChecklistBo bo)
        {
            if (!TreatyPricingGroupReferralChecklist.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref TreatyPricingGroupReferralChecklistBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingGroupReferralChecklist.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingGroupReferralChecklistBo bo)
        {
            TreatyPricingGroupReferralChecklist entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingGroupReferralChecklistBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingGroupReferralChecklistBo bo)
        {
            Result result = Result();

            TreatyPricingGroupReferralChecklist entity = TreatyPricingGroupReferralChecklist.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity = FormEntity(bo);
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingGroupReferralChecklistBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingGroupReferralChecklistBo bo)
        {
            TreatyPricingGroupReferralChecklist.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingGroupReferralChecklistBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingGroupReferralChecklist.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static void Save(string json, int parentId, int authUserId, ref TrailObject trail, bool resetId = false)
        {
            List<TreatyPricingGroupReferralChecklistBo> bos = new List<TreatyPricingGroupReferralChecklistBo>();
            if (!string.IsNullOrEmpty(json))
                bos = JsonConvert.DeserializeObject<List<TreatyPricingGroupReferralChecklistBo>>(json);

            foreach (var detailBo in bos)
            {
                var bo = detailBo;
                if (resetId)
                    bo.Id = 0;

                bo.TreatyPricingGroupReferralVersionId = parentId;
                bo.UpdatedById = authUserId;

                if (bo.Id == 0)
                    bo.CreatedById = authUserId;

                Save(ref bo, ref trail);
            }
        }

        public static void Update(string json, int authUserId, ref TrailObject trail, bool resetId = false)
        {
            List<TreatyPricingGroupReferralChecklistBo> bos = new List<TreatyPricingGroupReferralChecklistBo>();
            if (!string.IsNullOrEmpty(json))
                bos = JsonConvert.DeserializeObject<List<TreatyPricingGroupReferralChecklistBo>>(json);

            foreach (var detailBo in bos)
            {
                var bo = detailBo;
                if (resetId)
                    bo.Id = 0;

                bo.UpdatedById = authUserId;

                if (bo.Id == 0)
                    bo.CreatedById = authUserId;

                Save(ref bo, ref trail);
            }
        }
    }
}
