using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.Sanctions;
using Services;
using Services.Identity;
using Services.Sanctions;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApp.Middleware;

namespace WebApp.Controllers
{
    [Auth]
    public class ClaimDashboardController : BaseController
    {
        public const string Controller = "ClaimDashboard";

        // GET: ClaimDashboard
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index()
        {
            if (UserService.CheckPowerFlag(AuthUser, Controller, AccessMatrixBo.PowerManagerClaimDashboard))
            {
                LoadManager();
                return View("Manager");
            }
            else if (UserService.CheckPowerFlag(AuthUser, Controller, AccessMatrixBo.PowerIndividualClaimDashboard))
            {
                LoadIndividual();
                return View("Individual");
            }

            SetErrorSessionMsg("No Power Granted to View Operational Dashboard");
            return RedirectDashboard();
        }

        public void LoadManager()
        {
            ViewBag.AuthUserId = AuthUserId;
            var individualAssignedRiDataClaims = ClaimRegisterService.GetAssignedCaseByPicClaimId(AuthUserId);
            int totalIndividualRiDataClaims = individualAssignedRiDataClaims.Sum(q => q.NoOfCase);
            ViewBag.IndividualAssignedRiDataClaims = individualAssignedRiDataClaims;
            ViewBag.TotalIndividualRiDataClaims = totalIndividualRiDataClaims;

            var individualAssignedReferralClaims = ReferralClaimService.GetAssignedCaseByPicId(AuthUserId);
            int totalIndividualReferralClaims = individualAssignedReferralClaims.Sum(q => q.NoOfCase);
            ViewBag.IndividualAssignedReferralClaims = individualAssignedReferralClaims;
            ViewBag.TotalIndividualReferralClaims = totalIndividualReferralClaims;

            int totalIndividualAssignedCase = totalIndividualRiDataClaims + totalIndividualReferralClaims;
            ViewBag.TotalIndividualAssignedCase = totalIndividualAssignedCase;

            // RI Data Claim (AssignedOverview)
            var riAssignedCaseOverviews = ClaimRegisterService.GetAssignedCaseOverview();
            int totalRiAssignedCaseOverviews = riAssignedCaseOverviews != null ?riAssignedCaseOverviews.Sum(q => q.NoOfCase) : 0;
            ViewBag.TotalRiAssignedCaseOverviews = totalRiAssignedCaseOverviews;
            ViewBag.RiAssignedCaseOverviews = riAssignedCaseOverviews;

            // Referral Claim (AssignedOverview)
            var referralAssignedCaseOverviews = ReferralClaimService.GetAssignedCaseOverview();
            int totalReferralAssignedCaseOverviews = referralAssignedCaseOverviews != null ? referralAssignedCaseOverviews.Sum(q => q.NoOfCase) : 0;
            ViewBag.TotalReferralAssignedCaseOverviews = totalReferralAssignedCaseOverviews;
            ViewBag.ReferralAssignedCaseOverviews = referralAssignedCaseOverviews;

            // Total (Assigned Overview)
            ViewBag.TotalAssignedCaseOverviews = totalRiAssignedCaseOverviews + totalReferralAssignedCaseOverviews;

            // Turnaround Time
            GetTurnaroundTime();

            // Claims Operational Dashboard
            GetClaimsOperationalDashboard();

            // Pending Follow Up
            GetPendingFollowUp();

            // Sanction
            GetSummary();
            GetPotentialMatchedCase();

            // Referral Claim Pending Registration
            GetReferralClaimPendingRegistration();

            // StatusApprovalByLimit
            ViewBag.StatusApprovalByLimit = ClaimRegisterBo.StatusApprovalByLimit;

            // OutstandingClaimCase
            GetOutstandingClaimCase();
        }

        public void LoadIndividual()
        {
            ViewBag.AuthUserId = AuthUserId;
            var individualAssignedRiDataClaims = ClaimRegisterService.GetAssignedCaseByPicClaimId(AuthUserId);
            int totalIndividualRiDataClaims = individualAssignedRiDataClaims.Sum(q => q.NoOfCase);
            ViewBag.IndividualAssignedRiDataClaims = individualAssignedRiDataClaims;
            ViewBag.TotalIndividualRiDataClaims = totalIndividualRiDataClaims;

            var individualAssignedReferralClaims = ReferralClaimService.GetAssignedCaseByPicId(AuthUserId);
            int totalIndividualReferralClaims = individualAssignedReferralClaims.Sum(q => q.NoOfCase);
            ViewBag.IndividualAssignedReferralClaims = individualAssignedReferralClaims;
            ViewBag.TotalIndividualReferralClaims = totalIndividualReferralClaims;

            int totalIndividualAssignedCase = totalIndividualRiDataClaims + totalIndividualReferralClaims;
            ViewBag.TotalIndividualAssignedCase = totalIndividualAssignedCase;

            // Turnaround Time
            GetTurnaroundTime();

            // Claims Operational Dashboard
            GetClaimsOperationalDashboard();

            // Pending Follow Up
            GetPendingFollowUp(AuthUserId);

            // Sanction
            GetSummary();
            GetPotentialMatchedCase();

            // Referral Claim Pending Registration
            GetReferralClaimPendingRegistration(AuthUserId);

            // StatusApprovalByLimit
            ViewBag.StatusApprovalByLimit = ClaimRegisterBo.StatusApprovalByLimit;

            // OutstandingClaimCase
            GetOutstandingClaimCase();
        }

        public void GetTurnaroundTime()
        {
            int tat1Day = ReferralClaimService.CountByFilterTatDay(ReferralClaimBo.FilterTat1Day);
            int tat2Day = ReferralClaimService.CountByFilterTatDay(ReferralClaimBo.FilterTat2Day);
            int tatMoreThan2Day = ReferralClaimService.CountByFilterTatDay(ReferralClaimBo.FilterTatMoreThan2Day);
            ViewBag.Tat1Day = tat1Day;
            ViewBag.Tat2Day = tat2Day;
            ViewBag.TatMoreThan2Day = tatMoreThan2Day;
        }

        public void GetClaimsOperationalDashboard()
        {
            // RI Data Claim
            DropDownClaimTransactionType();
            var odRiDataClaims = ClaimRegisterService.GetOperationalDashboard();
            ViewBag.OdRiDataClaims = odRiDataClaims;

            // Referral Claim
            var odReferralDataClaims = ReferralClaimService.GetOperationalDashboard();
            ViewBag.OdReferralDataClaims = odReferralDataClaims;
        }

        public void GetPendingFollowUp(int? personInChargeId = null)
        {
            // RI Data Claim
            var pfuRiDataClaims = ClaimRegisterService.GetPendingFollowUp(personInChargeId);
            int totalpfuRiDataClaims = pfuRiDataClaims.Count();
            ViewBag.TotalPfuRiDataClaims = totalpfuRiDataClaims;
            ViewBag.PfuRiDataClaims = pfuRiDataClaims;

            // Referral Claim
            var pfuReferralClaims = ReferralClaimService.GetPendingFollowUp(personInChargeId);
            int totalpfuReferralClaims = pfuReferralClaims.Count();
            ViewBag.TotalPfuReferralClaims = totalpfuReferralClaims;
            ViewBag.PfuReferralClaims = pfuReferralClaims;

            // Total
            ViewBag.TotalPendingFollowUP = totalpfuRiDataClaims + totalpfuReferralClaims;
        }

        [HttpPost]
        public JsonResult GetRiAssignedCaseDetail(int picClaimId)
        {
            var riAssignedCaseDetails = ClaimRegisterService.GetAssignedCaseByPicClaimId(picClaimId);
            return Json(new { riAssignedCaseDetails });
        }

        [HttpPost]
        public JsonResult GetReferralAssignedCaseDetail(int personInChargeId)
        {
            var referralAssignedCaseDetails = ReferralClaimService.GetAssignedCaseByPicId(personInChargeId);
            return Json(new { referralAssignedCaseDetails });
        }

        [HttpPost]
        public JsonResult RefreshOdRiDataClaim(string claimTransactionType = null)
        {
            var odRiDataClaims = ClaimRegisterService.GetOperationalDashboard(claimTransactionType);
            return Json(new { odRiDataClaims });
        }

        [HttpPost]
        public JsonResult RefreshOdReferralClaim(string claimTransactionType = null)
        {
            var odRiDataClaims = ClaimRegisterService.GetOperationalDashboard(claimTransactionType);
            return Json(new { odRiDataClaims });
        }

        public List<SelectListItem> DropDownClaimTransactionType()
        {
            var items = GetEmptyDropDownList();
            items.Add(new SelectListItem { Text = PickListDetailBo.ClaimTransactionTypeNew, Value = PickListDetailBo.ClaimTransactionTypeNew });
            items.Add(new SelectListItem { Text = PickListDetailBo.ClaimTransactionTypeAdjustment, Value = PickListDetailBo.ClaimTransactionTypeAdjustment });
            ViewBag.DropDownClaimTransactionTypes = items;
            return items;
        }

        public void GetReferralClaimPendingRegistration(int? personInChargeId = null)
        {
            // 5 Days
            var pending5DaysReferralClaims = ReferralClaimService.GetPendingRegistrationByDays(5, false, personInChargeId, 20);
            int totalPending5DaysReferralClaims = ReferralClaimService.CountPendingRegistrationByDays(5, false, personInChargeId);
            ViewBag.TotalPending5DaysReferralClaims = totalPending5DaysReferralClaims;
            ViewBag.Pending5DaysReferralClaims = pending5DaysReferralClaims;

            // 6 Days
            var pending6DaysReferralClaims = ReferralClaimService.GetPendingRegistrationByDays(6, false, personInChargeId, 20);
            int totalPending6DaysReferralClaims = ReferralClaimService.CountPendingRegistrationByDays(6, false, personInChargeId);
            ViewBag.TotalPending6DaysReferralClaims = totalPending6DaysReferralClaims;
            ViewBag.Pending6DaysReferralClaims = pending6DaysReferralClaims;

            // 7 Days
            var pending7DaysReferralClaims = ReferralClaimService.GetPendingRegistrationByDays(7, false, personInChargeId, 20);
            int totalPending7DaysReferralClaims = ReferralClaimService.CountPendingRegistrationByDays(7, false, personInChargeId);
            ViewBag.TotalPending7DaysReferralClaims = totalPending7DaysReferralClaims;
            ViewBag.Pending7DaysReferralClaims = pending7DaysReferralClaims;

            // 7 Days onwards
            var pending7DaysOwReferralClaims = ReferralClaimService.GetPendingRegistrationByDays(7, true, personInChargeId, 20);
            int totalPending7DaysOwReferralClaims = ReferralClaimService.CountPendingRegistrationByDays(7, true, personInChargeId);
            ViewBag.TotalPending7DaysOwReferralClaims = totalPending7DaysOwReferralClaims;
            ViewBag.Pending7DaysOwReferralClaims = pending7DaysOwReferralClaims;

            // Total
            ViewBag.TotalPendingRegistrationReferralClaims = totalPending5DaysReferralClaims + totalPending6DaysReferralClaims + totalPending7DaysReferralClaims + totalPending7DaysOwReferralClaims;
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