using BusinessObject;
using BusinessObject.TreatyPricing;
using Services;
using Services.Identity;
using Services.TreatyPricing;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApp.Middleware;

namespace WebApp.Controllers
{
    [Auth]
    public class TreatyPricingInternalDashboardController : BaseController
    {
        public const string Controller = "TreatyPricingInternalDashboard";

        // GET: TreatyPricingInternalDashboard
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index()
        {
            LoadIndex();
            return View();
        }

        public void LoadIndex()
        {
            GetPendingQuotationCase();
            GetPendingGroupReferralCase();
            GetPendingTreatyCase();
        }

        public List<TreatyPricingQuotationWorkflowBo> GetPendingQuotationCase()
        {
            var quotationChecklist = new List<int>();
            var user = UserService.Find(AuthUserId);

            //int deptCEO = Util.GetConfigInteger("CEO");
            //int deptUw = Util.GetConfigInteger("UW");
            //int deptHealth = Util.GetConfigInteger("Health");
            //int deptClaims = Util.GetConfigInteger("Claims");
            //int deptBD = Util.GetConfigInteger("BDG");

            //if (user.DepartmentBo.Id == deptCEO || user.DepartmentBo.Id == deptUw || user.DepartmentBo.Id == deptHealth || user.DepartmentBo.Id == deptClaims || user.DepartmentBo.Id == deptBD)
            //{
            quotationChecklist = TreatyPricingQuotationWorkflowVersionQuotationChecklistService.GetPendingInternalDashboard(user);
            //}
            //else
            //{
            //    quotationChecklist = TreatyPricingQuotationWorkflowVersionQuotationChecklistService.GetAll()
            //    .Where(a => a.Status == TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.StatusPendingSignOff)
            //    .Select(a => a.TreatyPricingQuotationWorkflowVersionId).Distinct().ToList();
            //}

            var pendingQuotationCases = new List<TreatyPricingQuotationWorkflowBo>();

            int moduleId = ModuleService.FindByController(ModuleBo.ModuleController.TreatyPricingQuotationWorkflow.ToString()).Id;

            foreach (var ver in quotationChecklist)
            {
                var verBo = TreatyPricingQuotationWorkflowVersionService.Find(ver);
                var bo = TreatyPricingQuotationWorkflowService.Find(verBo.TreatyPricingQuotationWorkflowId);
                var checklistHistoryTriggerTime = GetStatusHistories(moduleId, bo.Id, null, ModuleBo.ModuleController.TreatyPricingQuotationWorkflowVersionQuotationChecklist.ToString())
                    .OrderByDescending(q => q.Id)
                    .Where(q => q.Department.Contains(user.DepartmentBo.Name) || user.DepartmentBo.Name.Contains(q.Department))
                    .Select(q => q.CreatedAtStr)
                    .FirstOrDefault();

                string triggerDate = "";

                if (!string.IsNullOrEmpty(checklistHistoryTriggerTime) && Util.GetParseDateTime(checklistHistoryTriggerTime).HasValue)
                {
                    triggerDate = Util.GetParseDateTime(checklistHistoryTriggerTime).Value.ToString("dd MMM yyyy");
                }
                else
                {
                    var checklistHistoryTriggerTime2 = GetStatusHistories(moduleId, bo.Id, null, ModuleBo.ModuleController.TreatyPricingQuotationWorkflowVersionQuotationChecklist.ToString())
                    .OrderByDescending(q => q.Id)
                    .Select(q => q.CreatedAtStr)
                    .FirstOrDefault();

                    if (!string.IsNullOrEmpty(checklistHistoryTriggerTime2) && Util.GetParseDateTime(checklistHistoryTriggerTime2).HasValue)
                    {
                        triggerDate = Util.GetParseDateTime(checklistHistoryTriggerTime2).Value.ToString("dd MMM yyyy");
                    }
                }

                bo.TriggerDateStr = triggerDate;

                pendingQuotationCases.Add(bo);
            }

            ViewBag.PendingQuotationCases = pendingQuotationCases.Distinct();
            ViewBag.PendingQuotationCasesLength = pendingQuotationCases.Distinct().Count();

            return pendingQuotationCases;
        }

        public List<TreatyPricingGroupReferralBo> GetPendingGroupReferralCase()
        {
            //var dep = 0;
            var user = UserService.Find(AuthUserId);

            var pendingGroupReferralCases = new List<TreatyPricingGroupReferralBo>();
            var groupReferralChecklist = TreatyPricingGroupReferralChecklistService.GetPendingInternalDashboard(user);

            int moduleId = ModuleService.FindByController(ModuleBo.ModuleController.TreatyPricingGroupReferral.ToString()).Id;
            foreach (var rCases in groupReferralChecklist)
            {
                var verBo = TreatyPricingGroupReferralVersionService.Find(rCases);
                var bo = TreatyPricingGroupReferralService.Find(verBo.TreatyPricingGroupReferralId);
                var checklistHistoryTriggerTime = GetStatusHistories(moduleId, bo.Id, null, ModuleBo.ModuleController.TreatyPricingGroupReferralChecklist.ToString())
                    .OrderByDescending(q => q.Id)
                    .Where(q => q.Department.Contains(user.DepartmentBo.Name) || user.DepartmentBo.Name.Contains(q.Department))
                    .Select(q => q.CreatedAtStr)
                    .FirstOrDefault();

                string triggerDate = "";

                if (!string.IsNullOrEmpty(checklistHistoryTriggerTime) && Util.GetParseDateTime(checklistHistoryTriggerTime).HasValue)
                {
                    triggerDate = Util.GetParseDateTime(checklistHistoryTriggerTime).Value.ToString("dd MMM yyyy");
                }
                else
                {
                    var checklistHistoryTriggerTime2 = GetStatusHistories(moduleId, bo.Id, null, ModuleBo.ModuleController.TreatyPricingGroupReferralChecklist.ToString())
                        .OrderByDescending(q => q.Id)
                        .Select(q => q.CreatedAtStr)
                        .FirstOrDefault();
                    if (!string.IsNullOrEmpty(checklistHistoryTriggerTime2) && Util.GetParseDateTime(checklistHistoryTriggerTime2).HasValue)
                    {
                        triggerDate = Util.GetParseDateTime(checklistHistoryTriggerTime2).Value.ToString("dd MMM yyyy");
                    }
                }

                bo.TriggerDateStr = triggerDate;

                var pendingItemList = new List<string>();

                var pendingItems = TreatyPricingGroupReferralChecklistDetailService.GetByTreatyPricingGroupReferralVersionId(rCases)
                   .Select(a => a.InternalItem).ToList();

                foreach (var pendingItem in pendingItems)
                {
                    pendingItemList.Add(pendingItem + " - " + TreatyPricingGroupReferralChecklistDetailBo.GetItemPovName(pendingItem));
                }

                bo.PendingItems = TreatyPricingGroupReferralChecklistService.GetPendingInternalDashboardItem(rCases, user);
                bo.LatestTreatyPricingGroupReferralVersionBo = bo.TreatyPricingGroupReferralVersionBos.OrderByDescending(a => a.Version).FirstOrDefault();
                pendingGroupReferralCases.Add(bo);
            }

            ViewBag.PendingGroupReferralCases = pendingGroupReferralCases;
            ViewBag.PendingGroupReferralCasesLength = pendingGroupReferralCases.Count();

            return pendingGroupReferralCases;
        }

        public List<TreatyPricingTreatyWorkflowVersionBo> GetPendingTreatyCase()
        {
            var user = UserService.Find(AuthUserId);

            var pendingTreatyWorkflowCases = TreatyPricingTreatyWorkflowVersionService.GetPendingInternalDashboard(user);
            pendingTreatyWorkflowCases = pendingTreatyWorkflowCases.Where(q => !string.IsNullOrEmpty(q.TreatyPricingTreatyWorkflowBo.Reviewer) && q.TreatyPricingTreatyWorkflowBo.Reviewer.Split(',').Select(r => r.Trim()).ToArray().Contains(user.Id.ToString())).ToList();

            var pendingCases = new List<TreatyPricingTreatyWorkflowVersionBo>();

            int moduleId = ModuleService.FindByController(ModuleBo.ModuleController.TreatyPricingTreatyWorkflow.ToString()).Id;
            foreach (var pendingItem in pendingTreatyWorkflowCases)
            {
                var checklistHistoryTriggerTime = GetStatusHistories(moduleId, pendingItem.TreatyPricingTreatyWorkflowId, null)
                    .OrderByDescending(q => q.Id)
                    .Where(q => q.RecipientNames.Contains(user.FullName))
                    .Select(q => q.CreatedAtStr)
                    .FirstOrDefault();


                string triggerDate = "";

                if (!string.IsNullOrEmpty(checklistHistoryTriggerTime) && Util.GetParseDateTime(checklistHistoryTriggerTime).HasValue)
                {
                        triggerDate = Util.GetParseDateTime(checklistHistoryTriggerTime).Value.ToString("dd MMM yyyy");
                    
                }
                else
                {
                    var checklistHistoryTriggerTime2 = GetStatusHistories(moduleId, pendingItem.TreatyPricingTreatyWorkflowId, null)
                         .OrderByDescending(q => q.Id)
                         .Select(q => q.CreatedAtStr)
                         .FirstOrDefault();

                    if (!string.IsNullOrEmpty(checklistHistoryTriggerTime2) && Util.GetParseDateTime(checklistHistoryTriggerTime2).HasValue)
                    {
                        triggerDate = Util.GetParseDateTime(checklistHistoryTriggerTime2).Value.ToString("dd MMM yyyy");
                    }
                }

                pendingItem.TriggerDateStr = triggerDate;
                pendingCases.Add(pendingItem);
            }

            ViewBag.PendingTreatyCases = pendingCases.Distinct();
            ViewBag.PendingTreatyCasesLength = pendingCases.Distinct().Count();

            return pendingCases;
        }
    }
}