using BusinessObject;
using PagedList;
using Services;
using Services.Claims;
using Services.RiDatas;
using Services.Sanctions;
using Services.SoaDatas;
using System;
using System.Linq;
using System.Web.Mvc;
using WebApp.Middleware;
using WebApp.Models;
using Shared;
using BusinessObject.Sanctions;

namespace WebApp.Controllers
{
    [Auth]
    public class OperationDashboardController : BaseController
    {
        public const string Controller = "OperationDashboard";

        // GET: Dashboard
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(int? Page, int? tabIndex)
        {
            var query = _db.CutOff.Select(CutOffViewModel.Expression());

            query = query.OrderBy(q => q.Year).ThenByDescending(q => q.Month);
            ViewBag.TotalCutOff = query.Count();

            var model = new OperationDashboardViewModel();
            model.CutOff = query.ToPagedList(Page ?? 1, PageSize);

            IndexPage();
            ViewBag.ActiveTab = tabIndex;
            return View(model);
        }

        // POST: Dashboard/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id)
        {
            var bo = CutOffService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            Result = CutOffService.Result();
            // Only quarter end is allowing to manually request for cut off. 
            // If it's not quarter end, the system should give error when user click on the "cut off" button.
            var currentDate = DateTime.Now;
            var quarterObject = new QuarterObject(bo.Quarter);
            if (quarterObject.EndDate.Value.Month == currentDate.Month)
            {
                if (currentDate.Day < Util.GetConfigInteger("DayOfQuarterEndMonth"))
                    Result.AddError(string.Format("{0} is not allowed for cut off. This quarter doesn't reach the day quarter reach end of the month", bo.Quarter));
            }

            if (Result.Valid)
            {
                bo.Status = CutOffBo.StatusSubmitForProcessing;
                bo.UpdatedById = AuthUserId;

                var trail = GetNewTrailObject();
                Result = CutOffService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Update Cut Off"
                    );

                    SetUpdateSuccessMessage("Cut Off", false);
                    return RedirectToAction("Index");
                }
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);
            return RedirectToAction("Index", new { tabIndex = 3 });
        }

        public void IndexPage()
        {
            DropDownYear();

            // RiDataOverview
            var riDataOverviews = RiDataBatchService.GetTotalCaseGroupByCedantId();
            ViewBag.RiDataOverviews = riDataOverviews;

            // ClaimDataOverview
            var claimDataOverviews = ClaimDataBatchService.GetTotalCaseGroupByCedantId();
            ViewBag.ClaimDataOverviews = claimDataOverviews;

            // ClaimDataOverview
            var soaDataOverviews = SoaDataBatchService.GetTotalCaseGroupByCedantId();
            ViewBag.SoaDataOverviews = soaDataOverviews;

            var pendingClarificationClaims = ClaimRegisterService.GetByClaimStatus(ClaimRegisterBo.StatusPendingClarification);
            ViewBag.PendingClarificationClaims = pendingClarificationClaims;
            ViewBag.TotalPendingClarificationClaims = pendingClarificationClaims.Count();

            var soaTrackingOverviews = SoaDataBatchService.GetSoaTrackingOverview();
            ViewBag.SoaTrackingOverviews = soaTrackingOverviews;

            // Sanction
            GetSummary();
            GetPotentialMatchedCase();

            // OutstandingClaimCase
            GetOutstandingClaimCase();

            SetViewBagMessage();
        }

        [HttpPost]
        public JsonResult GetOverviewDetail(int cedantId, int type)
        {
            if (type == 1)
            {
                var OverviewDetail = RiDataBatchService.GetDetailByCedantIdGroupByStatusByTreatyId(cedantId);
                return Json(new { OverviewDetail });
            }
            else if (type == 2)
            {
                var OverviewDetail = ClaimDataBatchService.GetDetailByCedantIdGroupByStatusByTreatyId(cedantId);
                return Json(new { OverviewDetail });
            }
            else if (type == 3)
            {
                var OverviewDetail = SoaDataBatchService.GetDetailByCedantIdGroupByStatusByTreatyId(cedantId);
                return Json(new { OverviewDetail });
            }
            else
            {
                object OverviewDetail = null;
                return Json(new { OverviewDetail });
            }
        }

        [HttpPost]
        public JsonResult GetSoaTrackingOverviewDetail(int cedantId)
        {
            var OverviewDetail = SoaDataBatchService.GetSoaTrackingOverviewDetailByCedantId(cedantId);
            return Json(new { OverviewDetail });
        }

        [HttpPost]
        public JsonResult CheckIfReadyForCutOff(string quarter)
        {
            bool result = true;
            if (!string.IsNullOrEmpty(quarter))
            {
                int countSoaDataBatch = SoaDataBatchService.CountByQuarterByExceptStatusApproved(quarter);
                int countDirectRetro = DirectRetroService.CountByQuarterByExceptRetroStatusCompleted(quarter);
                if (countSoaDataBatch != 0 || countDirectRetro != 0)
                    result = false;
            }
            return Json(new { result });
        }

        [HttpPost]
        public JsonResult GetTotalBookedByQuarter(int year)
        {
            var totalBookedByQuarter = SoaDataBatchService.GetTotalBookedByQuarter(year);
            return Json(new { TotalBookedByQuarter = totalBookedByQuarter });
        }

        [HttpPost]
        public JsonResult GetTotalBookedByCedant(int year)
        {
            var totalBookedByCedant = SoaDataBatchService.GetTotalBookedByCedant(year);
            return Json(new { TotalBookedByCedant = totalBookedByCedant });
        }

        public void GetSummary()
        {
            var potentialMatch = SanctionVerificationDetailService.CountByIsWhitelistIsExactMatch();
            var whitelist = SanctionWhitelistService.Count();
            var exactMatch = SanctionBlacklistService.Count();

            var summary = new SanctionVerificationBo
            {
                PotentialCount = potentialMatch,
                WhitelistCount = whitelist,
                ExactMatchCount = exactMatch
            };

            ViewBag.Summary = summary;
        }

        public void GetPotentialMatchedCase()
        {
            var potentialMatchedCases = SanctionVerificationService.GetPotentialCase();
            ViewBag.PotentialMatchedCases = potentialMatchedCases;

            var riDataCount = potentialMatchedCases.Sum(q => q.RiDataCount);
            var claimRegisterCount = potentialMatchedCases.Sum(q => q.ClaimRegisterCount);
            var referralClaimCount = potentialMatchedCases.Sum(q => q.ReferralClaimCount);

            var totalPotentialMatch = new SanctionVerificationBo
            {
                RiDataCount = riDataCount,
                ClaimRegisterCount = claimRegisterCount,
                ReferralClaimCount = referralClaimCount
            };

            ViewBag.TotalPotentialMatch = totalPotentialMatch;
        }

        public void GetOutstandingClaimCase()
        {
            var outstandingClaimCases = ClaimRegisterService.GetOutstandingCase(100);
            ViewBag.OutstandingClaimCases = outstandingClaimCases;
        }
    }
}