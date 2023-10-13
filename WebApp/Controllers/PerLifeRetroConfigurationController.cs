using ConsoleApp.Commands.ProcessDatas;
using ConsoleApp.Commands.ProcessDatas.Exports;
using PagedList;
using Services.Retrocession;
using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class PerLifeRetroConfigurationController : BaseController
    {
        public const string Controller = "PerLifeRetroConfiguration";

        // GET: PerLifeRetroConfiguration
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string TcTreatyCodeId,
            int? TcTreatyTypePickListDetailId,
            int? TcFundsAccountingTypePickListDetailId,
            string TcReinsEffectiveStartDate,
            string TcReinsEffectiveEndDate,
            string TcRiskQuarterStartDate,
            string TcRiskQuarterEndDate,
            bool? TcIsToAggregate,
            string TcRemark,
            string SortOrder,
            int? Page
        )
        {
            DateTime? tcReinsEffectiveStartDate = Util.GetParseDateTime(TcReinsEffectiveStartDate);
            DateTime? tcReinsEffectiveEndDate = Util.GetParseDateTime(TcReinsEffectiveEndDate);
            DateTime? tcRiskQuarterStartDate = Util.GetParseDateTime(TcRiskQuarterStartDate);
            DateTime? tcRiskQuarterEndDate = Util.GetParseDateTime(TcRiskQuarterEndDate);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["TcTreatyCodeId"] = TcTreatyCodeId,
                ["TcTreatyTypePickListDetailId"] = TcTreatyTypePickListDetailId,
                ["TcFundsAccountingTypePickListDetailId"] = TcFundsAccountingTypePickListDetailId,
                ["TcReinsEffectiveStartDate"] = tcReinsEffectiveStartDate.HasValue ? TcReinsEffectiveStartDate : null,
                ["TcReinsEffectiveEndDate"] = tcReinsEffectiveEndDate.HasValue ? TcReinsEffectiveEndDate : null,
                ["TcRiskQuarterStartDate"] = tcRiskQuarterStartDate.HasValue ? TcRiskQuarterStartDate : null,
                ["TcRiskQuarterEndDate"] = tcRiskQuarterEndDate.HasValue ? TcRiskQuarterEndDate : null,
                ["TcIsToAggregate"] = TcIsToAggregate,
                ["TcRemark"] = TcRemark,
                ["SortOrder"] = SortOrder,
            };

            ViewBag.SortOrder = SortOrder;

            ViewBag.SortTcTreatyCodeId = GetSortParam("TcTreatyCodeId");
            ViewBag.SortTcTreatyTypePickListDetailId = GetSortParam("TcTreatyTypePickListDetailId");
            ViewBag.SortTcFundsAccountingTypePickListDetailId = GetSortParam("TcFundsAccountingTypePickListDetailId");
            ViewBag.SortTcReinsEffectiveStartDate = GetSortParam("TcReinsEffectiveStartDate");
            ViewBag.SortTcReinsEffectiveEndDate = GetSortParam("TcReinsEffectiveEndDate");
            ViewBag.SortTcRiskQuarterStartDate = GetSortParam("TcRiskQuarterStartDate");
            ViewBag.SortTcRiskQuarterEndDate = GetSortParam("TcRiskQuarterEndDate");
            ViewBag.SortTcIsToAggregate = GetSortParam("TcIsToAggregate");

            var query = _db.PerLifeRetroConfigurationTreaties
                .Select(PerLifeRetroConfigurationTreatyViewModel.Expression());

            if (!string.IsNullOrEmpty(TcTreatyCodeId)) query = query.Where(q => q.TreatyCode.Code == TcTreatyCodeId);
            if (TcTreatyTypePickListDetailId.HasValue) query = query.Where(q => q.TreatyTypePickListDetailId == TcTreatyTypePickListDetailId);
            if (TcFundsAccountingTypePickListDetailId.HasValue) query = query.Where(q => q.FundsAccountingTypePickListDetailId == TcFundsAccountingTypePickListDetailId);
            if (tcReinsEffectiveStartDate.HasValue) query = query.Where(q => q.ReinsEffectiveStartDate >= tcReinsEffectiveStartDate);
            if (tcReinsEffectiveEndDate.HasValue) query = query.Where(q => q.ReinsEffectiveEndDate <= tcReinsEffectiveEndDate);
            if (tcRiskQuarterStartDate.HasValue) query = query.Where(q => q.RiskQuarterStartDate >= tcRiskQuarterStartDate);
            if (tcRiskQuarterEndDate.HasValue) query = query.Where(q => q.RiskQuarterEndDate <= tcRiskQuarterStartDate);
            if (TcIsToAggregate.HasValue) query = query.Where(q => q.IsToAggregate == TcIsToAggregate);
            if (!string.IsNullOrEmpty(TcRemark)) query = query.Where(q => q.Remark.Contains(TcRemark));

            if (SortOrder == Html.GetSortAsc("TcTreatyCodeId")) query = query.OrderBy(q => q.TreatyCode.Code);
            else if (SortOrder == Html.GetSortDsc("TcTreatyCodeId")) query = query.OrderByDescending(q => q.TreatyCode.Code);
            else if (SortOrder == Html.GetSortAsc("TcTreatyTypePickListDetailId")) query = query.OrderBy(q => q.TreatyTypePickListDetail.Code);
            else if (SortOrder == Html.GetSortDsc("TcTreatyTypePickListDetailId")) query = query.OrderByDescending(q => q.TreatyTypePickListDetail.Code);
            else if (SortOrder == Html.GetSortAsc("TcFundsAccountingTypePickListDetailId")) query = query.OrderBy(q => q.FundsAccountingTypePickListDetail.Code);
            else if (SortOrder == Html.GetSortDsc("TcFundsAccountingTypePickListDetailId")) query = query.OrderByDescending(q => q.FundsAccountingTypePickListDetail.Code);
            else if (SortOrder == Html.GetSortAsc("TcReinsEffectiveStartDate")) query = query.OrderBy(q => q.ReinsEffectiveStartDate);
            else if (SortOrder == Html.GetSortDsc("TcReinsEffectiveStartDate")) query = query.OrderByDescending(q => q.ReinsEffectiveStartDate);
            else if (SortOrder == Html.GetSortAsc("TcReinsEffectiveEndDate")) query = query.OrderBy(q => q.ReinsEffectiveEndDate);
            else if (SortOrder == Html.GetSortDsc("TcReinsEffectiveEndDate")) query = query.OrderByDescending(q => q.ReinsEffectiveEndDate);
            else if (SortOrder == Html.GetSortAsc("TcRiskQuarterStartDate")) query = query.OrderBy(q => q.RiskQuarterStartDate);
            else if (SortOrder == Html.GetSortDsc("TcRiskQuarterStartDate")) query = query.OrderByDescending(q => q.RiskQuarterStartDate);
            else if (SortOrder == Html.GetSortAsc("TcRiskQuarterEndDate")) query = query.OrderBy(q => q.RiskQuarterEndDate);
            else if (SortOrder == Html.GetSortDsc("TcRiskQuarterEndDate")) query = query.OrderByDescending(q => q.RiskQuarterEndDate);
            else query = query.OrderBy(q => q.TreatyCode.Code);

            ViewBag.ActiveTab = 1;

            ViewBag.TreatyTotal = query.Count();
            ViewBag.TreatyList = query.ToPagedList(Page ?? 1, PageSize);

            ViewBag.RaRouteValue = new RouteValueDictionary { };
            ViewBag.RatioTotal = 0;
            ViewBag.RatioList = new List<PerLifeRetroConfigurationRatioViewModel>().ToPagedList(1, PageSize);

            IndexPage();
            return View();
        }

        // GET: PerLifeRetroConfiguration/Ratio
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Ratio(
            string RaTreatyCodeId,
            string RaRetroRatio,
            string RaRetainRatio,
            string RaRuleValue,
            string RaReinsEffectiveStartDate,
            string RaReinsEffectiveEndDate,
            string RaRiskQuarterStartDate,
            string RaRiskQuarterEndDate,
            string RaRuleEffectiveDate,
            string RaRuleCeaseDate,
            string RaDescription,
            string SortOrder,
            int? Page
        )
        {
            double? raRetroRatio = Util.StringToDouble(RaRetroRatio);
            double? raRetainRatio = Util.StringToDouble(RaRetainRatio);
            double? raRuleValue = Util.StringToDouble(RaRuleValue);

            DateTime? raReinsEffectiveStartDate = Util.GetParseDateTime(RaReinsEffectiveStartDate);
            DateTime? raReinsEffectiveEndDate = Util.GetParseDateTime(RaReinsEffectiveEndDate);
            DateTime? raRiskQuarterStartDate = Util.GetParseDateTime(RaRiskQuarterStartDate);
            DateTime? raRiskQuarterEndDate = Util.GetParseDateTime(RaRiskQuarterEndDate);
            DateTime? raRuleEffectiveDate = Util.GetParseDateTime(RaRuleEffectiveDate);
            DateTime? raRuleCeaseDate = Util.GetParseDateTime(RaRuleCeaseDate);

            ViewBag.RaRouteValue = new RouteValueDictionary
            {
                ["RaTreatyCodeId"] = RaTreatyCodeId,
                ["RaRetroRatio"] = raRetroRatio.HasValue ? RaRetroRatio : null,
                ["RaRetainRatio"] = raRetainRatio.HasValue ? RaRetainRatio : null,
                ["RaRuleValue"] = raRuleValue.HasValue ? RaRuleValue : null,
                ["RaReinsEffectiveStartDate"] = raReinsEffectiveStartDate.HasValue ? RaReinsEffectiveStartDate : null,
                ["RaReinsEffectiveEndDate"] = raReinsEffectiveEndDate.HasValue ? RaReinsEffectiveEndDate : null,
                ["RaRiskQuarterStartDate"] = raRiskQuarterStartDate.HasValue ? RaRiskQuarterStartDate : null,
                ["RaRiskQuarterEndDate"] = raRiskQuarterEndDate.HasValue ? RaRiskQuarterEndDate : null,
                ["RaRuleEffectiveDate"] = raRuleEffectiveDate.HasValue ? RaRuleEffectiveDate : null,
                ["RaRuleCeaseDate"] = raRuleEffectiveDate.HasValue ? RaRuleCeaseDate : null,
                ["RaDescription"] = RaDescription,
                ["SortOrder"] = SortOrder,
            };

            ViewBag.SortOrder = SortOrder;

            ViewBag.SortRaTreatyCodeId = GetSortParam("RaTreatyCodeId");
            ViewBag.SortRaRetroRatio = GetSortParam("RaRetroRatio");
            ViewBag.SortRaRetainRatio = GetSortParam("RaRetainRatio");
            ViewBag.SortRaRuleValue = GetSortParam("RaRuleValue");
            ViewBag.SortRaReinsEffectiveStartDate = GetSortParam("RaReinsEffectiveStartDate");
            ViewBag.SortRaReinsEffectiveEndDate = GetSortParam("RaReinsEffectiveEndDate");
            ViewBag.SortRaRiskQuarterStartDate = GetSortParam("RaRiskQuarterStartDate");
            ViewBag.SortRaRiskQuarterEndDate = GetSortParam("RaRiskQuarterEndDate");
            ViewBag.SortRaRuleEffectiveDate = GetSortParam("RaRuleEffectiveDate");
            ViewBag.SortRaRuleCeaseDate = GetSortParam("RaRuleCeaseDate");

            var query = _db.PerLifeRetroConfigurationRatios
                .Select(PerLifeRetroConfigurationRatioViewModel.Expression());

            if (!string.IsNullOrEmpty(RaTreatyCodeId)) query = query.Where(q => q.TreatyCode.Code == RaTreatyCodeId);
            if (raRetroRatio.HasValue) query = query.Where(q => q.RetroRatio == raRetroRatio);
            if (raRetainRatio.HasValue) query = query.Where(q => q.MlreRetainRatio == raRetainRatio);
            if (raRuleValue.HasValue) query = query.Where(q => q.RuleValue == raRuleValue);
            if (raReinsEffectiveStartDate.HasValue) query = query.Where(q => q.ReinsEffectiveStartDate >= raReinsEffectiveStartDate);
            if (raReinsEffectiveEndDate.HasValue) query = query.Where(q => q.ReinsEffectiveEndDate <= raReinsEffectiveEndDate);
            if (raRiskQuarterStartDate.HasValue) query = query.Where(q => q.RiskQuarterStartDate >= raRiskQuarterStartDate);
            if (raRiskQuarterEndDate.HasValue) query = query.Where(q => q.RiskQuarterEndDate <= raRiskQuarterEndDate);
            if (raRuleEffectiveDate.HasValue) query = query.Where(q => q.RuleEffectiveDate >= raRuleEffectiveDate);
            if (raRuleCeaseDate.HasValue) query = query.Where(q => q.RuleCeaseDate <= raRuleCeaseDate);
            if (!string.IsNullOrEmpty(RaDescription)) query = query.Where(q => q.Description.Contains(RaDescription));

            if (SortOrder == Html.GetSortAsc("RaTreatyCodeId")) query = query.OrderBy(q => q.TreatyCode.Code);
            else if (SortOrder == Html.GetSortDsc("RaTreatyCodeId")) query = query.OrderByDescending(q => q.TreatyCode.Code);
            else if (SortOrder == Html.GetSortAsc("RaRetroRatio")) query = query.OrderBy(q => q.RetroRatio);
            else if (SortOrder == Html.GetSortDsc("RaRetroRatio")) query = query.OrderByDescending(q => q.RetroRatio);
            else if (SortOrder == Html.GetSortAsc("RaMlreRetainRatio")) query = query.OrderBy(q => q.MlreRetainRatio);
            else if (SortOrder == Html.GetSortDsc("RaMlreRetainRatio")) query = query.OrderByDescending(q => q.MlreRetainRatio);
            else if (SortOrder == Html.GetSortAsc("RaRuleValue")) query = query.OrderBy(q => q.RuleValue);
            else if (SortOrder == Html.GetSortDsc("RaRuleValue")) query = query.OrderByDescending(q => q.RuleValue);
            else if (SortOrder == Html.GetSortAsc("RaReinsEffectiveStartDate")) query = query.OrderBy(q => q.ReinsEffectiveStartDate);
            else if (SortOrder == Html.GetSortDsc("RaReinsEffectiveStartDate")) query = query.OrderByDescending(q => q.ReinsEffectiveStartDate);
            else if (SortOrder == Html.GetSortAsc("RaReinsEffectiveEndDate")) query = query.OrderBy(q => q.ReinsEffectiveEndDate);
            else if (SortOrder == Html.GetSortDsc("RaReinsEffectiveEndDate")) query = query.OrderByDescending(q => q.ReinsEffectiveEndDate);
            else if (SortOrder == Html.GetSortAsc("RaRiskQuarterStartDate")) query = query.OrderBy(q => q.RiskQuarterStartDate);
            else if (SortOrder == Html.GetSortDsc("RaRiskQuarterStartDate")) query = query.OrderByDescending(q => q.RiskQuarterStartDate);
            else if (SortOrder == Html.GetSortAsc("RaRiskQuarterEndDate")) query = query.OrderBy(q => q.RiskQuarterEndDate);
            else if (SortOrder == Html.GetSortDsc("RaRiskQuarterEndDate")) query = query.OrderByDescending(q => q.RiskQuarterEndDate);
            else if (SortOrder == Html.GetSortAsc("RaRuleEffectiveDate")) query = query.OrderBy(q => q.RiskQuarterStartDate);
            else if (SortOrder == Html.GetSortDsc("RaRuleEffectiveDate")) query = query.OrderByDescending(q => q.RiskQuarterStartDate);
            else if (SortOrder == Html.GetSortAsc("RaRuleCeaseDate")) query = query.OrderBy(q => q.RuleCeaseDate);
            else if (SortOrder == Html.GetSortDsc("RaRuleCeaseDate")) query = query.OrderByDescending(q => q.RuleCeaseDate);
            else query = query.OrderBy(q => q.TreatyCode.Code);

            ViewBag.ActiveTab = 2;

            ViewBag.RatioTotal = query.Count();
            ViewBag.RatioList = query.ToPagedList(Page ?? 1, PageSize);

            ViewBag.RouteValue = new RouteValueDictionary { };
            ViewBag.TreatyTotal = 0;
            ViewBag.TreatyList = new List<PerLifeRetroConfigurationTreatyViewModel>().ToPagedList(1, PageSize);

            IndexPage();
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadTreaty(HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var process = new ProcessPerLifeRetroConfigurationTreaty()
                    {
                        PostedFile = upload,
                        AuthUserId = AuthUserId,
                    };
                    process.Process();

                    if (process.Errors.Count() > 0 && process.Errors != null)
                    {
                        SetErrorSessionMsgArr(process.Errors.Take(50).ToList());
                    }

                    int create = process.GetProcessCount("Create");
                    int update = process.GetProcessCount("Update");
                    int delete = process.GetProcessCount("Delete");

                    SetSuccessSessionMsg(string.Format("Process File Successful, Total Create: {0}, Total Update: {1}, Total Delete: {2}", create, update, delete));
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult DownloadTreaty(
            string downloadToken,
            int type,
            string TcTreatyCodeId,
            int? TcTreatyTypePickListDetailId,
            int? TcFundsAccountingTypePickListDetailId,
            string TcReinsEffectiveStartDate,
            string TcReinsEffectiveEndDate,
            string TcRiskQuarterStartDate,
            string TcRiskQuarterEndDate,
            bool? TcIsToAggregate,
            string TcRemark
        )
        {
            // type 1 = all
            // type 2 = filtered download
            // type 3 = template download

            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            _db.Database.CommandTimeout = 0;
            var query = _db.PerLifeRetroConfigurationTreaties.Select(PerLifeRetroConfigurationTreatyService.Expression());
            if (type == 2) // filtered dowload
            {
                DateTime? tcReinsEffectiveStartDate = Util.GetParseDateTime(TcReinsEffectiveStartDate);
                DateTime? tcReinsEffectiveEndDate = Util.GetParseDateTime(TcReinsEffectiveEndDate);
                DateTime? tcRiskQuarterStartDate = Util.GetParseDateTime(TcRiskQuarterStartDate);
                DateTime? tcRiskQuarterEndDate = Util.GetParseDateTime(TcRiskQuarterEndDate);

                if (!string.IsNullOrEmpty(TcTreatyCodeId)) query = query.Where(q => q.TreatyCode == TcTreatyCodeId);
                if (TcTreatyTypePickListDetailId.HasValue) query = query.Where(q => q.TreatyTypePickListDetailId == TcTreatyTypePickListDetailId);
                if (TcFundsAccountingTypePickListDetailId.HasValue) query = query.Where(q => q.FundsAccountingTypePickListDetailId == TcFundsAccountingTypePickListDetailId);
                if (tcReinsEffectiveStartDate.HasValue) query = query.Where(q => q.ReinsEffectiveStartDate >= tcReinsEffectiveStartDate);
                if (tcReinsEffectiveEndDate.HasValue) query = query.Where(q => q.ReinsEffectiveEndDate <= tcReinsEffectiveEndDate);
                if (tcRiskQuarterStartDate.HasValue) query = query.Where(q => q.RiskQuarterStartDate >= tcRiskQuarterStartDate);
                if (tcRiskQuarterEndDate.HasValue) query = query.Where(q => q.RiskQuarterEndDate <= tcRiskQuarterStartDate);
                if (TcIsToAggregate.HasValue) query = query.Where(q => q.IsToAggregate == TcIsToAggregate);
                if (!string.IsNullOrEmpty(TcRemark)) query = query.Where(q => q.Remark.Contains(TcRemark));
            }

            if (type == 3)
            {
                query = null;
            }

            var export = new ExportPerLifeRetroConfigurationTreaty();
            export.HandleTempDirectory();

            if (query != null)
                export.SetQuery(query);

            export.Process();

            return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadRatio(HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var process = new ProcessPerLifeRetroConfigurationRatio()
                    {
                        PostedFile = upload,
                        AuthUserId = AuthUserId,
                    };
                    process.Process();

                    if (process.Errors.Count() > 0 && process.Errors != null)
                    {
                        SetErrorSessionMsgArr(process.Errors.Take(50).ToList());
                    }

                    int create = process.GetProcessCount("Create");
                    int update = process.GetProcessCount("Update");
                    int delete = process.GetProcessCount("Delete");

                    SetSuccessSessionMsg(string.Format("Process File Successful, Total Create: {0}, Total Update: {1}, Total Delete: {2}", create, update, delete));
                }
            }

            return RedirectToAction("Ratio");
        }

        public ActionResult DownloadRatio(
            string downloadToken,
            int type,
            string RaTreatyCodeId,
            string RaRetroRatio,
            string RaRetainRatio,
            string RaRuleValue,
            string RaReinsEffectiveStartDate,
            string RaReinsEffectiveEndDate,
            string RaRiskQuarterStartDate,
            string RaRiskQuarterEndDate,
            string RaRuleEffectiveDate,
            string RaRuleCeaseDate,
            string RaDescription
        )
        {
            // type 1 = all
            // type 2 = filtered download
            // type 3 = template download

            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            _db.Database.CommandTimeout = 0;
            var query = _db.PerLifeRetroConfigurationRatios.Select(PerLifeRetroConfigurationRatioService.Expression());
            if (type == 2) // filtered dowload
            {
                double? raRetroRatio = Util.StringToDouble(RaRetroRatio);
                double? raRetainRatio = Util.StringToDouble(RaRetainRatio);
                double? raRuleValue = Util.StringToDouble(RaRuleValue);

                DateTime? raReinsEffectiveStartDate = Util.GetParseDateTime(RaReinsEffectiveStartDate);
                DateTime? raReinsEffectiveEndDate = Util.GetParseDateTime(RaReinsEffectiveEndDate);
                DateTime? raRiskQuarterStartDate = Util.GetParseDateTime(RaRiskQuarterStartDate);
                DateTime? raRiskQuarterEndDate = Util.GetParseDateTime(RaRiskQuarterEndDate);
                DateTime? raRuleEffectiveDate = Util.GetParseDateTime(RaRuleEffectiveDate);
                DateTime? raRuleCeaseDate = Util.GetParseDateTime(RaRuleCeaseDate);

                if (!string.IsNullOrEmpty(RaTreatyCodeId)) query = query.Where(q => q.TreatyCode == RaTreatyCodeId);
                if (raRetroRatio.HasValue) query = query.Where(q => q.RetroRatio == raRetroRatio);
                if (raRetainRatio.HasValue) query = query.Where(q => q.MlreRetainRatio == raRetainRatio);
                if (raRuleValue.HasValue) query = query.Where(q => q.RuleValue == raRuleValue);
                if (raReinsEffectiveStartDate.HasValue) query = query.Where(q => q.ReinsEffectiveStartDate >= raReinsEffectiveStartDate);
                if (raReinsEffectiveEndDate.HasValue) query = query.Where(q => q.ReinsEffectiveEndDate <= raReinsEffectiveEndDate);
                if (raRiskQuarterStartDate.HasValue) query = query.Where(q => q.RiskQuarterStartDate >= raRiskQuarterStartDate);
                if (raRiskQuarterEndDate.HasValue) query = query.Where(q => q.RiskQuarterEndDate <= raRiskQuarterEndDate);
                if (raRuleEffectiveDate.HasValue) query = query.Where(q => q.RuleEffectiveDate >= raRuleEffectiveDate);
                if (raRuleCeaseDate.HasValue) query = query.Where(q => q.RuleCeaseDate <= raRuleCeaseDate);
                if (!string.IsNullOrEmpty(RaDescription)) query = query.Where(q => q.Description.Contains(RaDescription));
            }

            if (type == 3)
            {
                query = null;
            }

            var export = new ExportPerLifeRetroConfigurationRatio();
            export.HandleTempDirectory();

            if (query != null)
                export.SetQuery(query);

            export.Process();

            return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));
        }

        public void IndexPage()
        {
            DropDownTreatyCode(codeAsValue: true, isUniqueCode: true, foreign: false);
            DropDownTreatyType();
            DropDownFundsAccountingTypeCode();
            DropDownYesNoWithSelect();

            SetViewBagMessage();
        }
    }
}