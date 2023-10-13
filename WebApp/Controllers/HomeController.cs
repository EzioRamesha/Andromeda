using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using PagedList;
using Services;
using Services.TreatyPricing;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        public ActionResult Dashboard(
            int? ExportType,
            int? ExportStatus,
            string ExportSortOrder,
            string ReportCreatedAt,
            string ReportName,
            string ReportFileName,
            int? ReportSubmittedBy,
            int? ReportStatus,
            string ReportSortOrder,
            int? ExportQueueType,
            string ExportQueueSortOrder,
            int? ExportPage,
            int? ReportPage,
            int? ExportQueuePage,
            int? ActiveTab
        )
        {
            ViewBag.ExportRouteValue = new RouteValueDictionary
            {    
                ["ExportType"] = ExportType,
                ["ExportStatus"] = ExportStatus,
                ["ExportSortOrder"] = ExportSortOrder,

                ["CreatedAt"] = ReportCreatedAt,
                ["ReportName"] = ReportName,
                ["FileName"] = ReportFileName,
                ["CreatedById"] = ReportSubmittedBy,
                ["Status"] = ReportStatus,
                ["ReportSortOrder"] = ReportSortOrder,

                ["ExportQueueType"] = ExportQueueType,
                ["ExportQueueSortOrder"] = ExportQueueSortOrder,
                ["ActiveTab"] = ActiveTab,
            };
            ViewBag.SortOrder = ActiveTab == 3 ? ReportSortOrder : ActiveTab == 2 ? ExportSortOrder : ExportQueueSortOrder;

            ViewBag.ExportSortOrder = ExportSortOrder;
            ViewBag.ExportTotal = GetSortParam("ExportTotal");
            ViewBag.ExportProcessed = GetSortParam("ExportProcessed");
            ViewBag.ExportGenerateStartAt = GetSortParam("ExportGenerateStartAt");
            ViewBag.ExportGenerateEndAt = GetSortParam("ExportGenerateEndAt");

            ViewBag.ReportSortOrder = ReportSortOrder;
            ViewBag.CreatedAt = GetSortParam("CreatedAt");
            ViewBag.ReportName = GetSortParam("ReportName");
            ViewBag.FileName = GetSortParam("FileName");
            ViewBag.CreatedById = GetSortParam("CreatedById");
            ViewBag.Status = GetSortParam("Status");

            ViewBag.ExportQueueSortOrder = ExportQueueSortOrder;
            ViewBag.ExportQueueGenerateStartAt = GetSortParam("ExportQueueGenerateStartAt");
            ViewBag.ExportQueueRequestedAt = GetSortParam("ExportQueueRequestedAt");

            var query = _db.Exports.Select(ExportViewModel.Expression());
            query = query.Where(q => q.CreatedById == AuthUserId);
            if (ExportType != null)
            {
                var exportTypes = new List<int>();
                exportTypes.Add(ExportType.Value);
                if (ExportType == ExportBo.TypeRiDataWarehouse)
                    exportTypes.Add(ExportBo.TypeRiDataWarehouseHistory);

                if (ExportType == ExportBo.TypeClaimRegisterSearch)
                    exportTypes.Add(ExportBo.TypeClaimRegisterHistorySearch);

                query = query.Where(q => exportTypes.Contains(q.Type));
            }
            if (ExportStatus != null) query = query.Where(q => q.Status == ExportStatus);

            if (ExportSortOrder == Html.GetSortAsc("ExportTotal")) query = query.OrderBy(q => q.Total);
            else if (ExportSortOrder == Html.GetSortDsc("ExportTotal")) query = query.OrderByDescending(q => q.Total);
            else if (ExportSortOrder == Html.GetSortAsc("ExportProcessed")) query = query.OrderBy(q => q.Processed);
            else if (ExportSortOrder == Html.GetSortDsc("ExportProcessed")) query = query.OrderByDescending(q => q.Processed);
            else if (ExportSortOrder == Html.GetSortAsc("ExportGenerateStartAt")) query = query.OrderBy(q => q.GenerateStartAt);
            else if (ExportSortOrder == Html.GetSortDsc("ExportGenerateStartAt")) query = query.OrderByDescending(q => q.GenerateStartAt);
            else if (ExportSortOrder == Html.GetSortAsc("ExportGenerateEndAt")) query = query.OrderBy(q => q.GenerateEndAt);
            else if (ExportSortOrder == Html.GetSortDsc("ExportGenerateEndAt")) query = query.OrderByDescending(q => q.GenerateEndAt);
            else query = query.OrderByDescending(q => q.CreatedAt);

            ViewBag.ExportTotal = query.Count();
            ViewBag.ExportList = query.ToPagedList(ExportPage ?? 1, PageSize);

            DropDownExportType();
            DropDownExportStatus();
            DropDownTreatyPricingReportGenerationReportType();
            DropDownReportStatus();
            DropDownUser(UserBo.StatusActive);
            AuthUserName();
            SetViewBagMessage();

            ViewBag.ActiveTab = ActiveTab;

            //Report Generation tab
            GetReportData(ReportPage, ReportCreatedAt, ReportName, ReportFileName, ReportSubmittedBy, ReportStatus, ReportSortOrder);
            //Export Queue tab
            GetExportQueueData(ExportQueuePage, ExportQueueType, ExportQueueSortOrder);

            return View();
        }

        private void GetReportData(int? ReportPage,
            string ReportCreatedAt,
            string ReportName,
            string ReportFileName,
            int? ReportSubmittedBy,
            int? ReportStatus,
            string ReportSortOrder)
        {
            DateTime? createdAtFrom = Util.GetParseDateTime(ReportCreatedAt);
            DateTime? createdAtTo = null;

            if (createdAtFrom.HasValue)
                createdAtTo = createdAtFrom.Value.AddDays(1);

            var query = _db.TreatyPricingReportGenerations.Select(TreatyPricingReportGenerationViewModel.Expression());
            if (createdAtFrom.HasValue) query = query.Where(q => q.CreatedAt >= createdAtFrom && q.CreatedAt < createdAtTo);
            if (!string.IsNullOrEmpty(ReportName)) query = query.Where(q => q.ReportName == ReportName);
            if (!string.IsNullOrEmpty(ReportFileName)) query = query.Where(q => q.FileName == ReportFileName);
            if (ReportSubmittedBy.HasValue) query = query.Where(q => q.CreatedById == ReportSubmittedBy);
            if (ReportStatus.HasValue) query = query.Where(q => q.Status == ReportStatus);

            if (ReportSortOrder == Html.GetSortAsc("CreatedAt")) query = query.OrderBy(q => q.CreatedAt);
            else if (ReportSortOrder == Html.GetSortDsc("CreatedAt")) query = query.OrderByDescending(q => q.CreatedAt);
            else if (ReportSortOrder == Html.GetSortAsc("ReportName")) query = query.OrderBy(q => q.ReportName);
            else if (ReportSortOrder == Html.GetSortDsc("ReportName")) query = query.OrderByDescending(q => q.ReportName);
            else if (ReportSortOrder == Html.GetSortAsc("FileName")) query = query.OrderBy(q => q.FileName);
            else if (ReportSortOrder == Html.GetSortDsc("FileName")) query = query.OrderByDescending(q => q.FileName);
            else if (ReportSortOrder == Html.GetSortAsc("CreatedById")) query = query.OrderBy(q => q.CreatedById);
            else if (ReportSortOrder == Html.GetSortDsc("CreatedById")) query = query.OrderByDescending(q => q.CreatedById);
            else if (ReportSortOrder == Html.GetSortAsc("Status")) query = query.OrderBy(q => q.Status);
            else if (ReportSortOrder == Html.GetSortDsc("Status")) query = query.OrderByDescending(q => q.Status);
            else query = query.OrderByDescending(q => q.Id);

            ViewBag.TreatyPricingReportTotal = query.Count();
            ViewBag.TreatyPricingReportList = query.ToPagedList(ReportPage ?? 1, PageSize);
        }

        private void GetExportQueueData(int? ExportQueuePage,
            int? ExportQueueType,
            string ExportQueueSortOrder)
        {
            var exportStatus = new List<int>();
            exportStatus.Add(ExportBo.StatusPending);
            exportStatus.Add(ExportBo.StatusGenerating);

            var query = _db.Exports.Select(ExportViewModel.Expression())
                .Where(q => exportStatus.Contains(q.Status));
            if (ExportQueueType != null)
            {
                var exportTypes = new List<int>();
                exportTypes.Add(ExportQueueType.Value);
                if (ExportQueueType == ExportBo.TypeRiDataWarehouse)
                    exportTypes.Add(ExportBo.TypeRiDataWarehouseHistory);

                if (ExportQueueType == ExportBo.TypeClaimRegisterSearch)
                    exportTypes.Add(ExportBo.TypeClaimRegisterHistorySearch);

                query = query.Where(q => exportTypes.Contains(q.Type));
            }

            if (ExportQueueSortOrder == Html.GetSortAsc("ExportQueueGenerateStartAt")) query = query.OrderBy(q => q.GenerateStartAt);
            else if (ExportQueueSortOrder == Html.GetSortDsc("ExportQueueGenerateStartAt")) query = query.OrderByDescending(q => q.GenerateStartAt);
            else if (ExportQueueSortOrder == Html.GetSortAsc("ExportQueueRequestedAt")) query = query.OrderBy(q => q.CreatedAt);
            else if (ExportQueueSortOrder == Html.GetSortDsc("ExportQueueRequestedAt")) query = query.OrderByDescending(q => q.CreatedAt);
            else query = query.OrderBy(q => q.CreatedAt);

            ViewBag.ExportQueueTotal = query.Count();
            ViewBag.ExportQueueList = query.ToPagedList(ExportQueuePage ?? 1, PageSize);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AllowAnonymous]
        public JsonResult IpAddress()
        {
            return Json(new { IpAddress = GetIpAddress() }, JsonRequestBehavior.AllowGet);
        }

        public List<SelectListItem> DropDownExportType(int? selectedId = null)
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= ExportBo.MaxType; i++)
            {
                if (i == ExportBo.TypeRiDataWarehouseHistory || i == ExportBo.TypeClaimRegisterHistorySearch)
                    continue;

                var selected = i == selectedId;
                items.Add(new SelectListItem { Text = ExportBo.GetTypeName(i), Value = i.ToString(), Selected = selected });
            }
            ViewBag.ExportTypeItems = items;
            return items;
        }

        public List<SelectListItem> DropDownExportStatus(int? selectedId = null)
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= ExportBo.MaxStatus; i++)
            {
                var selected = i == selectedId;
                items.Add(new SelectListItem { Text = ExportBo.GetStatusName(i), Value = i.ToString(), Selected = selected });
            }
            ViewBag.ExportStatusItems = items;
            return items;
        }

        public List<SelectListItem> DropDownReportStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingReportGenerationBo.StatusMax))
            {
                items.Add(new SelectListItem { Text = TreatyPricingReportGenerationBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.ReportStatusItems = items;
            return items;
        }

        public ActionResult DownloadReportGenerationFile(int id)
        {
            TreatyPricingReportGenerationBo bo = TreatyPricingReportGenerationService.Find(id);
            if (bo == null)
                return null;

            string path = bo.GetLocalPath();
            if (System.IO.File.Exists(path) && path != "")
            {
                return File(
                    System.IO.File.ReadAllBytes(path),
                    System.Net.Mime.MediaTypeNames.Application.Octet,
                    bo.FileName
                );
            }
            return null;
        }
    }
}