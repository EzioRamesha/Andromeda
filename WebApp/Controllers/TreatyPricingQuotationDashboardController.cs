using BusinessObject;
using Services;
using Services.TreatyPricing;
using System.Linq;
using System.Web.Mvc;
using WebApp.Middleware;

namespace WebApp.Controllers
{
    [Auth]
    public class TreatyPricingQuotationDashboardController : BaseController
    {
        public const string Controller = "TreatyPricingQuotationDashboard";

        // GET: TreatyPricingQuotationDashboard
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index()
        {
            LoadIndex();
            return View();
        }

        public void LoadIndex()
        {
            var quotationCaseByReinsuranceTypes = TreatyPricingQuotationWorkflowService.GetQuotationCaseByReinsuranceType();
            ViewBag.QuotationCaseByReinsuranceTypes = quotationCaseByReinsuranceTypes;
            ViewBag.TotalQuotationCaseByReinsuranceTypes = quotationCaseByReinsuranceTypes.Sum(q => q.NoOfCase);

            var quotationCaseByPics = TreatyPricingQuotationWorkflowService.GetQuotationCaseByPic();
            ViewBag.QuotationCaseByPics = quotationCaseByPics;

            var departmentQuotingCases = TreatyPricingQuotationWorkflowService.GetQuotationCaseByDepartment();
            ViewBag.DepartmentQuotingCases = departmentQuotingCases;

            var companyActiveCases = TreatyPricingQuotationWorkflowService.GetCompanyActiveCases();
            ViewBag.CompanyActiveCases = companyActiveCases;

            ViewBag.Statuses = TreatyPricingQuotationWorkflowVersionQuotationChecklistService.GetChecklistStatusBosForDashboard();
        }
    }
}