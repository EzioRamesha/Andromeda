using BusinessObject.Sanctions;
using Services.Sanctions;
using System.Linq;
using System.Web.Mvc;
using WebApp.Middleware;

namespace WebApp.Controllers
{
    [Auth]
    public class ComplianceRiskDashboardController : BaseController
    {
        public const string Controller = "ComplianceRiskDashboard";

        // GET: ComplianceRiskDashboard
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index()
        {
            IndexPage();
            return View();
        }

        public void IndexPage()
        {
            GetSummary();
            //GetExactMatchedCase();
            GetPotentialMatchedCase();
        }

        //public void GetExactMatchedCase()
        //{
        //    var exactMatchedCases = SanctionVerificationService.GetMatchedCase(true);
        //    ViewBag.ExactMatchedCases = exactMatchedCases;
        //}

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
    }
}