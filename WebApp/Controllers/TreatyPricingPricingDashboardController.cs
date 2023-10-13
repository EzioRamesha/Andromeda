using BusinessObject;
using Services;
using Services.TreatyPricing;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApp.Middleware;

namespace WebApp.Controllers
{
    [Auth]
    public class TreatyPricingPricingDashboardController : BaseController
    {
        public const string Controller = "TreatyPricingPricingDashboard";

        // GET: TreatyPricingPricingDashboard
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index()
        {
            LoadIndex();
            return View();
        }

        public void LoadIndex()
        {

        }

        [HttpPost]
        public JsonResult GetDetails(string pricingTeam)
        {
            int pricingTeamPickListDetailId = PickListDetailService.FindByPickListIdCode(PickListBo.PricingTeam, pricingTeam).Id;

            //Outstanding Pricing Overview
            var quotationCaseByPricingStatus = TreatyPricingQuotationWorkflowService.GetQuotationCaseByPricingStatus(pricingTeamPickListDetailId);
            int totalQuotationCaseByPricingStatus = quotationCaseByPricingStatus.Sum(q => q.NoOfCase);

            var quotationCaseByPics = TreatyPricingQuotationWorkflowService.GetQuotationCaseByPricingPic(pricingTeamPickListDetailId);

            //Due Date Overview
            var quotationCaseByDueDateOverview = TreatyPricingQuotationWorkflowService.GetQuotationCaseForDueDateOverview(pricingTeamPickListDetailId);
            var quotationCaseByDueDateOverviewPIC = TreatyPricingQuotationWorkflowService.GetQuotationCaseByPricingPicForDueDateOverview(pricingTeamPickListDetailId);
            int totalQuotationCaseByDueDateOverview = quotationCaseByDueDateOverview.Sum(q => q.NoOfCase);

            //Quoting Case
            var quotationCaseByStatusQuoting = TreatyPricingQuotationWorkflowService.GetPricingCompanyActiveCases(pricingTeamPickListDetailId);
            int totalQuotationCaseByStatusQuoting = quotationCaseByStatusQuoting.Sum(q => q.TotalCase);

            return Json(new { pricingTeamPickListDetailId
                , totalQuotationCaseByPricingStatus
                , totalQuotationCaseByDueDateOverview
                , totalQuotationCaseByStatusQuoting
                , quotationCaseByPricingStatus
                , quotationCaseByPics
                , quotationCaseByDueDateOverview
                , quotationCaseByDueDateOverviewPIC
                , quotationCaseByStatusQuoting
            });
        }

        [HttpPost]
        public JsonResult GetDueDateFilteredIds(int pricingTeamPickListDetailId, int dueDateOverviewType)
        {
            List<int> idList = TreatyPricingQuotationWorkflowService.GetIdsFilteredByDueDateOverview(pricingTeamPickListDetailId, dueDateOverviewType);

            return Json(new { idList });
        }
    }
}