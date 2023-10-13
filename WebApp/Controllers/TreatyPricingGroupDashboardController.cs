using BusinessObject.TreatyPricing;
using Services.TreatyPricing;
using Services;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebApp.Middleware;
using System.Linq;
using Services.Identity;

namespace WebApp.Controllers
{
    [Auth]
    public class TreatyPricingGroupDashboardController : BaseController
    {
        public const string Controller = "TreatyPricingGroupDashboard";

        // GET: TreatyPricingGroupDashboard
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index()
        {
            LoadIndex();
            return View();
        }

        public void LoadIndex()
        {
            ViewBag.CurrentYear = DateTime.Now.Year;

            var versionBos = TreatyPricingGroupReferralVersionService.GetAllByYear(DateTime.Now.Year, true);
            GetTurnaroundTime(versionBos);
            GetActiveCaseByPic(versionBos);

            var checklistBos = TreatyPricingGroupReferralChecklistService.GetGroupOverallTatCount(DateTime.Now.Year);
            GetActiveCaseByPendingReviewer(checklistBos);
            GetActiveCaseByPendingHOD(checklistBos);
            GetActiveCaseByPendingCEO(checklistBos);
            GetActiveCaseByPendingDepartment(checklistBos);
        }

        public TreatyPricingGroupReferralBo GetTurnaroundTime(IList<TreatyPricingGroupReferralVersionBo> versions)
        {
            var bo = new TreatyPricingGroupReferralBo();

            bo.NoOfDays0Cedant = versions.Where(q => q.QuotationTAT == 0 && !q.QuotationSentDate.HasValue).Count();
            bo.NoOfDays1Cedant = versions.Where(q => q.QuotationTAT == 1 && !q.QuotationSentDate.HasValue).Count();
            bo.NoOfDays2Cedant = versions.Where(q => q.QuotationTAT == 2 && !q.QuotationSentDate.HasValue).Count();
            bo.NoOfDays3Cedant = versions.Where(q => q.QuotationTAT == 3 && !q.QuotationSentDate.HasValue).Count();
            bo.NoOfDays4Cedant = versions.Where(q => q.QuotationTAT > 3 && !q.QuotationSentDate.HasValue).Count();

            bo.NoOfDays0Internal = versions.Where(q => q.InternalTAT == 0 && !q.QuotationSentDate.HasValue).Count();
            bo.NoOfDays1Internal = versions.Where(q => q.InternalTAT == 1 && !q.QuotationSentDate.HasValue).Count();
            bo.NoOfDays2Internal = versions.Where(q => q.InternalTAT == 2 && !q.QuotationSentDate.HasValue).Count();
            bo.NoOfDays3Internal = versions.Where(q => q.InternalTAT == 3 && !q.QuotationSentDate.HasValue).Count();
            bo.NoOfDays4Internal = versions.Where(q => q.InternalTAT > 3 && !q.QuotationSentDate.HasValue).Count();

            //bo.NoOfDays0Cedant = versionBos.Where(q => q.TreatyPricingGroupReferralBo.QuotationTAT.GetValueOrDefault() == 0).Count();
            //bo.NoOfDays1Cedant = versionBos.Where(q => q.TreatyPricingGroupReferralBo.QuotationTAT.GetValueOrDefault() == 1).Count();
            //bo.NoOfDays2Cedant = versionBos.Where(q => q.TreatyPricingGroupReferralBo.QuotationTAT.GetValueOrDefault() == 2).Count();
            //bo.NoOfDays3Cedant = versionBos.Where(q => q.TreatyPricingGroupReferralBo.QuotationTAT.GetValueOrDefault() == 3).Count();
            //bo.NoOfDays4Cedant = versionBos.Where(q => q.TreatyPricingGroupReferralBo.QuotationTAT.GetValueOrDefault() > 3).Count();

            //bo.NoOfDays0Internal = versionBos.Where(q => q.TreatyPricingGroupReferralBo.InternalTAT.GetValueOrDefault() == 0).Count();
            //bo.NoOfDays1Internal = versionBos.Where(q => q.TreatyPricingGroupReferralBo.InternalTAT.GetValueOrDefault() == 1).Count();
            //bo.NoOfDays2Internal = versionBos.Where(q => q.TreatyPricingGroupReferralBo.InternalTAT.GetValueOrDefault() == 2).Count();
            //bo.NoOfDays3Internal = versionBos.Where(q => q.TreatyPricingGroupReferralBo.InternalTAT.GetValueOrDefault() == 3).Count();
            //bo.NoOfDays4Internal = versionBos.Where(q => q.TreatyPricingGroupReferralBo.InternalTAT.GetValueOrDefault() > 3).Count();

            var allGroupVersions = TreatyPricingGroupReferralVersionService.GetAllUnassignedForGroupDashboard(true);

            var unassigned = allGroupVersions.OrderByDescending(q => q.Version).Select(q => q.TreatyPricingGroupReferralId).Distinct();
            var unassignedCount = 0;

            foreach (var i in unassigned)
            {
                var verBo = TreatyPricingGroupReferralVersionService.GetLatestVersionByTreatyPricingGroupReferralIdForGroupDashboard(i);
                if (!verBo.GroupReferralPersonInChargeId.HasValue)
                    unassignedCount++;
            }

            bo.UnassignedTotal = unassignedCount;

            ViewBag.TurnaroundTime = bo;

            return bo;
        }

        [HttpPost]
        public JsonResult GetTurnaroundTimeDetail(int type)
        {
            // type 1 = Quotation TAT = 0
            // type 2 = Internal TAT = 0
            // type 3 = Quotation TAT = 1
            // type 4 = Internal TAT = 1
            // type 5 = Quotation TAT = 2
            // type 6 = Internal TAT = 2
            // type 7 = Quotation TAT = 3
            // type 8 = Internal TAT = 3
            // type 9 = Quotation TAT > 3
            // type 10 = Internal TAT > 3

            var details = new List<TreatyPricingGroupReferralBo>();
            var turnaroundTime = "";
            if (type != 0)
            {
                var picActiveCases = TreatyPricingGroupReferralVersionService.GetByTurnaroundTime(type, true);

                foreach (var i in picActiveCases)
                {
                    var bo = new TreatyPricingGroupReferralBo();
                    bo.Id = i.TreatyPricingGroupReferralBo.Id;
                    bo.CedantName = i.TreatyPricingGroupReferralBo.CedantBo.Name;
                    bo.InsuredGroupNameName = i.TreatyPricingGroupReferralBo.InsuredGroupNameBo.Name;
                    bo.Code = i.TreatyPricingGroupReferralBo.Code;
                    bo.Description = i.TreatyPricingGroupReferralBo.Description;
                    bo.Version = i.Version;
                    details.Add(bo);
                }

                if (type == 1)
                    turnaroundTime = "Quotation TAT 0 Days";
                else if (type == 2)
                    turnaroundTime = "Internal TAT 0 Days";
                else if (type == 3)
                    turnaroundTime = "Quotation TAT 1 Day";
                else if (type == 4)
                    turnaroundTime = "Internal TAT 1 Day";
                else if (type == 5)
                    turnaroundTime = "Quotation TAT 2 Days";
                else if (type == 6)
                    turnaroundTime = "Internal TAT 2 Days";
                else if (type == 7)
                    turnaroundTime = "Quotation TAT 3 Days";
                else if (type == 8)
                    turnaroundTime = "Internal TAT 3 Days";
                else if (type == 9)
                    turnaroundTime = "Quotation TAT more than 3 Days";
                else if (type == 10)
                    turnaroundTime = "Internal TAT more than 3 Days";
            }

            return Json(new { details, turnaroundTime });
        }


        public List<TreatyPricingGroupReferralBo> GetActiveCaseByPic(IList<TreatyPricingGroupReferralVersionBo> versionBos)
        {
            var activeCasesByPic = new List<TreatyPricingGroupReferralBo>();

            var PICs = versionBos.Where(q => q.GroupReferralPersonInChargeId.HasValue).Select(q => q.GroupReferralPersonInChargeId).Distinct().ToList();
            foreach (var picId in PICs)
            {
                if (picId != null)
                {
                    var picVerBo = versionBos.Where(a => a.GroupReferralPersonInChargeId == picId);
                    //var activeCaseBo = picVerBo.Where(q => q.TreatyPricingGroupReferralBo.WorkflowStatus != TreatyPricingGroupReferralBo.WorkflowStatusQuotationSent && q.TreatyPricingGroupReferralBo.WorkflowStatus != TreatyPricingGroupReferralBo.WorkflowStatusPrepareRIGroupSlip && q.TreatyPricingGroupReferralBo.WorkflowStatus != TreatyPricingGroupReferralBo.WorkflowStatusCompleted);
                    var activeCaseBo = picVerBo.Where(q => !q.QuotationSentDate.HasValue);
                    var activeInternalCaseBo = picVerBo.Where(q => !q.QuotationSentDate.HasValue &&
                    q.TreatyPricingGroupReferralBo.WorkflowStatus != TreatyPricingGroupReferralBo.WorkflowStatusPendingClient);

                    var picBo = new TreatyPricingGroupReferralBo();
                    picBo.PersonInChargeName = UserService.Find(picId).FullName;
                    picBo.PersonInChargeId = picId.Value;
                    picBo.NoOfCasesByPic = picVerBo.Count();
                    picBo.NoOfActiveCasesByPic = activeCaseBo.Count();

                    var caseCount = 0;

                    foreach (var ver in picVerBo)
                    {
                        var checklistBo = TreatyPricingGroupReferralChecklistService.GetByTreatyPricingGroupReferralVersionIdForGroupDashboard(ver.Id);
                        var internalCase = checklistBo.Where(q => q.Status != TreatyPricingGroupReferralChecklistBo.StatusNotRequired);

                        caseCount = caseCount + internalCase.Count();
                    }


                    //picBo.ActiveCaseWithNoOfDays4 = picVerBo.Where(a => a.TreatyPricingGroupReferralBo.InternalTAT.GetValueOrDefault() > 3).Count();
                    picBo.ActiveCaseWithNoOfDays4 = activeInternalCaseBo.Count();
                    var sumScore = picVerBo.Where(q => q.Score.HasValue).Select(a => a.Score.Value).Sum();
                    picBo.AverageScore = Math.Round((double)sumScore / picBo.NoOfCasesByPic, 2);

                    activeCasesByPic.Add(picBo);
                }
            }

            ViewBag.ActiveCasesByPic = activeCasesByPic;
            ViewBag.TotalNoOfCasesByPic = activeCasesByPic.Select(a => a.NoOfCasesByPic).Sum();
            ViewBag.TotalNoOfActiveCasesByPic = activeCasesByPic.Select(a => a.NoOfActiveCasesByPic).Sum();
            ViewBag.TotalActiveCaseWithNoOfDays4 = activeCasesByPic.Select(a => a.ActiveCaseWithNoOfDays4).Sum();
            ViewBag.TotalAverageScore = activeCasesByPic.Select(a => a.AverageScore).Sum();

            return activeCasesByPic;
        }

        [HttpPost]
        public JsonResult GetActiveCaseByPicDetail(int picId)
        {
            var details = new List<TreatyPricingGroupReferralBo>();

            var pic = UserService.Find(picId).FullName;

            if (picId != 0)
            {
                var picActiveCases = TreatyPricingGroupReferralVersionService.GetByPendingPIC(picId, true);

                foreach (var i in picActiveCases)
                {
                    var bo = new TreatyPricingGroupReferralBo();
                    bo.Id = i.TreatyPricingGroupReferralBo.Id;
                    bo.CedantName = i.TreatyPricingGroupReferralBo.CedantBo.Name;
                    bo.InsuredGroupNameName = i.TreatyPricingGroupReferralBo.InsuredGroupNameBo.Name;
                    bo.Code = i.TreatyPricingGroupReferralBo.Code;
                    bo.Description = i.TreatyPricingGroupReferralBo.Description;
                    bo.Version = i.Version;
                    details.Add(bo);
                }
            }

            return Json(new { details, pic });
        }

        public List<TreatyPricingGroupReferralBo> GetActiveCaseByPendingReviewer(IList<TreatyPricingGroupReferralChecklistBo> checklistBos)
        {
            var activeCasesPendingReviewer = new List<TreatyPricingGroupReferralBo>();
            var checklistReviewers = checklistBos.Where(a => a.InternalTeam == TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamReviewer);

            var reviewerPICs = new List<string>();
            foreach (var checklistReviewer in checklistReviewers)
            {
                if (!string.IsNullOrEmpty(checklistReviewer.InternalTeamPersonInCharge))
                {
                    if (checklistReviewer.InternalTeamPersonInCharge.Contains(','))
                        reviewerPICs.AddRange(checklistReviewer.InternalTeamPersonInCharge.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList());
                    else
                        reviewerPICs.Add(checklistReviewer.InternalTeamPersonInCharge);
                }
            }

            foreach (var reviewer in reviewerPICs.Distinct())
            {
                if (!string.IsNullOrEmpty(reviewer))
                {
                    var user = UserService.FindByUsername(reviewer.Trim());

                    var checklistReviewer = checklistReviewers.Where(q => !string.IsNullOrEmpty(q.InternalTeamPersonInCharge) && q.InternalTeamPersonInCharge.Contains(reviewer.Trim()));

                    var bo = new TreatyPricingGroupReferralBo();
                    bo.PersonInChargeName = user.FullName;
                    bo.NoOfCasesByPic = checklistReviewer.Count();
                    bo.NoOfActiveCasesByPic = checklistReviewer
                        .Where(a => !a.TreatyPricingGroupReferralVersionBo.QuotationSentDate.HasValue)
                        .Where(a => a.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingApproval || a.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingReview).Count();

                    bo.ActiveCaseWithNoOfDays4 = checklistReviewer.Where(q => !q.TreatyPricingGroupReferralVersionBo.QuotationSentDate.HasValue &&
                    q.TreatyPricingGroupReferralVersionBo.TreatyPricingGroupReferralBo.WorkflowStatus != TreatyPricingGroupReferralBo.WorkflowStatusPendingClient &&
                    (q.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingApproval || q.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingReview)).Count();

                    activeCasesPendingReviewer.Add(bo);
                }
            }

            ViewBag.ActiveCasesPendingReviewer = activeCasesPendingReviewer;
            ViewBag.TotalNoOfCasesPendingReviewer = activeCasesPendingReviewer.Select(a => a.NoOfCasesByPic).Sum();
            ViewBag.TotalNoOfActiveCasesPendingReviewer = activeCasesPendingReviewer.Select(a => a.NoOfActiveCasesByPic).Sum();
            ViewBag.TotalActiveCasePendingReviewerWithNoOfDays4 = activeCasesPendingReviewer.Select(a => a.ActiveCaseWithNoOfDays4).Sum();

            return activeCasesPendingReviewer;
        }

        [HttpPost]
        public JsonResult GetActiveCasePendingReviewerDetail(string reviewer)
        {
            var details = new List<TreatyPricingGroupReferralBo>();

            if (!string.IsNullOrEmpty(reviewer))
            {
                var checklistBos = TreatyPricingGroupReferralChecklistService.GetByPendingReviewer(reviewer, true);
                foreach (var checklistBo in checklistBos)
                {
                    var versionBo = TreatyPricingGroupReferralVersionService.FindForGroupDashboardDetail(checklistBo.TreatyPricingGroupReferralVersionId, true);

                    var bo = new TreatyPricingGroupReferralBo();
                    bo.Id = versionBo.TreatyPricingGroupReferralBo.Id;
                    bo.CedantName = versionBo.TreatyPricingGroupReferralBo.CedantBo.Name;
                    bo.InsuredGroupNameName = versionBo.TreatyPricingGroupReferralBo.InsuredGroupNameBo.Name;
                    bo.Code = versionBo.TreatyPricingGroupReferralBo.Code;
                    bo.Description = versionBo.TreatyPricingGroupReferralBo.Description;
                    bo.Version = versionBo.Version;
                    details.Add(bo);
                }
            }

            return Json(new { details, reviewer });
        }

        public List<TreatyPricingGroupReferralBo> GetActiveCaseByPendingHOD(IList<TreatyPricingGroupReferralChecklistBo> checklistBos)
        {
            var activeCasesPendingHOD = new List<TreatyPricingGroupReferralBo>();
            var checklistHODs = checklistBos.Where(a => a.InternalTeam == TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamHOD);

            var hodPICs = new List<string>();
            foreach (var checklistHOD in checklistHODs)
            {
                if (!string.IsNullOrEmpty(checklistHOD.InternalTeamPersonInCharge))
                {
                    if (checklistHOD.InternalTeamPersonInCharge.Contains(','))
                        hodPICs.AddRange(checklistHOD.InternalTeamPersonInCharge.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList());
                    else
                        hodPICs.Add(checklistHOD.InternalTeamPersonInCharge);
                }
            }

            foreach (var hod in hodPICs.Distinct())
            {
                if (!string.IsNullOrEmpty(hod))
                {
                    var user = UserService.FindByUsername(hod);

                    var checklistHOD = checklistHODs.Where(q => !string.IsNullOrEmpty(q.InternalTeamPersonInCharge) && q.InternalTeamPersonInCharge.Contains(hod));

                    var bo = new TreatyPricingGroupReferralBo();
                    bo.PersonInChargeName = user.FullName;
                    bo.NoOfCasesByPic = checklistHOD.Count();
                    bo.NoOfActiveCasesByPic = checklistHOD.Where(q => !q.TreatyPricingGroupReferralVersionBo.QuotationSentDate.HasValue &&
                    (q.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingApproval || q.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingReview)).Count();

                    //var ids = TreatyPricingGroupReferralChecklistService.GetIdsByInternalTeam(TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamHOD, hod, TreatyPricingGroupReferralChecklistBo.StatusPendingApproval);
                    //if (ids != null) count = TreatyPricingGroupReferralService.CountByIds(ids);
                    bo.ActiveCaseWithNoOfDays4 = checklistHOD.Where(q => !q.TreatyPricingGroupReferralVersionBo.QuotationSentDate.HasValue &&
                    q.TreatyPricingGroupReferralVersionBo.TreatyPricingGroupReferralBo.WorkflowStatus != TreatyPricingGroupReferralBo.WorkflowStatusPendingClient &&
                    (q.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingApproval || q.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingReview)).Count();

                    activeCasesPendingHOD.Add(bo);
                }
            }
            ViewBag.ActiveCasesPendingHOD = activeCasesPendingHOD;

            return activeCasesPendingHOD;
        }

        [HttpPost]
        public JsonResult GetActiveCasePendingHODDetail(string reviewer)
        {
            var details = new List<TreatyPricingGroupReferralBo>();

            if (!string.IsNullOrEmpty(reviewer))
            {
                var checklistBos = TreatyPricingGroupReferralChecklistService.GetByPendingHOD(reviewer, true);
                foreach (var checklistBo in checklistBos)
                {
                    var versionBo = TreatyPricingGroupReferralVersionService.FindForGroupDashboardDetail(checklistBo.TreatyPricingGroupReferralVersionId, true);

                    var bo = new TreatyPricingGroupReferralBo();
                    bo.Id = versionBo.TreatyPricingGroupReferralBo.Id;
                    bo.CedantName = versionBo.TreatyPricingGroupReferralBo.CedantBo.Name;
                    bo.InsuredGroupNameName = versionBo.TreatyPricingGroupReferralBo.InsuredGroupNameBo.Name;
                    bo.Code = versionBo.TreatyPricingGroupReferralBo.Code;
                    bo.Description = versionBo.TreatyPricingGroupReferralBo.Description;
                    bo.Version = versionBo.Version;
                    details.Add(bo);
                }
            }


            return Json(new { details, reviewer });
        }

        public List<TreatyPricingGroupReferralBo> GetActiveCaseByPendingCEO(IList<TreatyPricingGroupReferralChecklistBo> checklistBos)
        {
            var activeCasesPendingCEO = new List<TreatyPricingGroupReferralBo>();
            var checklistCEOs = checklistBos.Where(a => a.InternalTeam == TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamCEO);

            var ceoPICs = new List<string>();
            foreach (var checklistCEO in checklistCEOs)
            {
                if (!string.IsNullOrEmpty(checklistCEO.InternalTeamPersonInCharge))
                {
                    if (checklistCEO.InternalTeamPersonInCharge.Contains(','))
                        ceoPICs.AddRange(checklistCEO.InternalTeamPersonInCharge.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList());
                    else
                        ceoPICs.Add(checklistCEO.InternalTeamPersonInCharge);
                }
            }

            foreach (var ceo in ceoPICs.Distinct())
            {
                if (!string.IsNullOrEmpty(ceo))
                {
                    var user = UserService.FindByUsername(ceo);

                    var checklistCEO = checklistCEOs.Where(q => !string.IsNullOrEmpty(q.InternalTeamPersonInCharge) && q.InternalTeamPersonInCharge.Contains(ceo));

                    var bo = new TreatyPricingGroupReferralBo();
                    bo.PersonInChargeName = user.FullName;
                    bo.NoOfCasesByPic = checklistCEO.Count();
                    bo.NoOfActiveCasesByPic = checklistCEO.Where(q => !q.TreatyPricingGroupReferralVersionBo.QuotationSentDate.HasValue &&
                    (q.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingApproval || q.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingReview)).Count();

                    //var ids = TreatyPricingGroupReferralChecklistService.GetIdsByInternalTeam(TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamCEO, ceo, TreatyPricingGroupReferralChecklistBo.StatusPendingApproval);
                    //if (ids != null) count = TreatyPricingGroupReferralService.CountByIds(ids);
                    bo.ActiveCaseWithNoOfDays4 = checklistCEO.Where(q => !q.TreatyPricingGroupReferralVersionBo.QuotationSentDate.HasValue &&
                    q.TreatyPricingGroupReferralVersionBo.TreatyPricingGroupReferralBo.WorkflowStatus != TreatyPricingGroupReferralBo.WorkflowStatusPendingClient &&
                    (q.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingApproval || q.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingReview)).Count();

                    activeCasesPendingCEO.Add(bo);
                }
            }
            ViewBag.ActiveCasesPendingCEO = activeCasesPendingCEO;

            return activeCasesPendingCEO;
        }

        [HttpPost]
        public JsonResult GetActiveCasePendingCEODetail(string reviewer)
        {
            var details = new List<TreatyPricingGroupReferralBo>();

            if (!string.IsNullOrEmpty(reviewer))
            {
                var checklistBos = TreatyPricingGroupReferralChecklistService.GetByPendingCEO(reviewer, true);
                foreach (var checklistBo in checklistBos)
                {
                    var bo = new TreatyPricingGroupReferralBo();
                    var versionBo = TreatyPricingGroupReferralVersionService.FindForGroupDashboardDetail(checklistBo.TreatyPricingGroupReferralVersionId, true);

                    bo.Id = versionBo.TreatyPricingGroupReferralBo.Id;
                    bo.CedantName = versionBo.TreatyPricingGroupReferralBo.CedantBo.Name;
                    bo.InsuredGroupNameName = versionBo.TreatyPricingGroupReferralBo.InsuredGroupNameBo.Name;
                    bo.Code = versionBo.TreatyPricingGroupReferralBo.Code;
                    bo.Description = versionBo.TreatyPricingGroupReferralBo.Description;
                    bo.Version = versionBo.Version;
                    details.Add(bo);
                }
            }


            return Json(new { details, reviewer });
        }

        public List<TreatyPricingGroupReferralBo> GetActiveCaseByPendingDepartment(IList<TreatyPricingGroupReferralChecklistBo> checklistBos)
        {
            var activeCasesPendingDepartment = new List<TreatyPricingGroupReferralBo>();

            foreach (var department in Enumerable.Range(1, TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamCR))
            {
                var checklistDepartment = checklistBos.Where(q => q.InternalTeam == department);
                var bo = new TreatyPricingGroupReferralBo();
                bo.DepartmentId = department;
                bo.DepartmentName = TreatyPricingGroupReferralChecklistBo.GetDefaultInternalTeamName(department);
                bo.NoOfCasesByPic = checklistDepartment.Count();
                bo.NoOfActiveCasesByPic = checklistDepartment.Where(q => !q.TreatyPricingGroupReferralVersionBo.QuotationSentDate.HasValue &&
                (q.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingApproval || q.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingReview)).Count();

                bo.ActiveCaseWithNoOfDays4 = checklistDepartment.Where(q => !q.TreatyPricingGroupReferralVersionBo.QuotationSentDate.HasValue &&
                q.TreatyPricingGroupReferralVersionBo.TreatyPricingGroupReferralBo.WorkflowStatus != TreatyPricingGroupReferralBo.WorkflowStatusPendingClient &&
                (q.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingApproval || q.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingReview)).Count();



                activeCasesPendingDepartment.Add(bo);
            }

            ViewBag.ActiveCasesPendingDepartment = activeCasesPendingDepartment;
            ViewBag.TotalNoOfCasesPendingDepartment = activeCasesPendingDepartment.Select(q => q.NoOfCasesByPic).Sum();
            ViewBag.TotalNoOfActiveCasesPendingDepartment = activeCasesPendingDepartment.Select(q => q.NoOfActiveCasesByPic).Sum();
            ViewBag.TotalActiveCasePendingDepartmentWithNoOfDays4 = activeCasesPendingDepartment.Select(q => q.ActiveCaseWithNoOfDays4).Sum();

            return activeCasesPendingDepartment;
        }

        [HttpPost]
        public JsonResult GetActiveCasePendingDepartmentDetail(int dept)
        {
            var details = new List<TreatyPricingGroupReferralBo>();

            var deptName = TreatyPricingGroupReferralChecklistBo.GetDefaultInternalTeamName(dept);

            var verIds = TreatyPricingGroupReferralChecklistService.GetByPendingDepartment(dept, true).Select(q => q.TreatyPricingGroupReferralVersionId).Distinct();
            foreach (var verId in verIds)
            {
                var versionBo = TreatyPricingGroupReferralVersionService.FindForGroupDashboardDetail(verId, true);

                var bo = new TreatyPricingGroupReferralBo();
                bo.Id = versionBo.TreatyPricingGroupReferralBo.Id;
                bo.CedantName = versionBo.TreatyPricingGroupReferralBo.CedantBo.Name;
                bo.InsuredGroupNameName = versionBo.TreatyPricingGroupReferralBo.InsuredGroupNameBo.Name;
                bo.Code = versionBo.TreatyPricingGroupReferralBo.Code;
                bo.Description = versionBo.TreatyPricingGroupReferralBo.Description;
                bo.Version = versionBo.Version;
                details.Add(bo);
            }


            return Json(new { details, deptName });
        }
    }
}