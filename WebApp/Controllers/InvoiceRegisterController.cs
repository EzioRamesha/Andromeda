using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.InvoiceRegisters;
using BusinessObject.SoaDatas;
using ConsoleApp.Commands.ProcessDatas;
using PagedList;
using Services;
using Services.InvoiceRegisters;
using Services.SoaDatas;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class InvoiceRegisterController : BaseController
    {
        public const string Controller = "InvoiceRegister";

        // GET: InvoiceRegister
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            InvoiceRegisterViewModel model,
            int? InvoiceType,
            string InvoiceNo,
            string InvoiceDate,
            string StatementReceivedDate,
            int? CedantId,
            string RiskQuarter,
            int? TreatyCodeId,
            string AccountFor,
            string TotalPaid,
            string Year1st,
            string Renewal,
            string Gross1st,
            string GrossRenewal,
            int? PreparedById,
            int? InvoiceStatus,
            int? BatchNo,
            string BatchDate,
            int? NoOfInvoices,
            int? BatchPersonInChargeId,
            int? BatchStatus,
            string Sort1Order,
            string Sort2Order,
            int? Results1Page,
            int? Results2Page,
            int? TabIndex)
        {
            if (Results1Page.HasValue) model.SearchResults1Page = Results1Page;
            if (Results2Page.HasValue) model.SearchResults2Page = Results2Page;
            if (TabIndex.HasValue) model.ActiveTab = TabIndex;

            DateTime? invoiceDate = Util.GetParseDateTime(InvoiceDate);
            DateTime? statementReceivedDate = Util.GetParseDateTime(StatementReceivedDate);
            double? totalPaid = Util.StringToDouble(TotalPaid);
            double? year1st = Util.StringToDouble(Year1st);
            double? renewal = Util.StringToDouble(Renewal);
            double? gross1st = Util.StringToDouble(Gross1st);
            double? grossRenewal = Util.StringToDouble(GrossRenewal);
            DateTime? batchDate = Util.GetParseDateTime(BatchDate);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["InvoiceType"] = InvoiceType,
                ["TreatyCodeId"] = TreatyCodeId,
                ["InvoiceNo"] = InvoiceNo,
                ["InvoiceDate"] = invoiceDate.HasValue ? InvoiceDate : null,
                ["StatementReceivedDate"] = statementReceivedDate.HasValue ? StatementReceivedDate : null,
                ["CedantId"] = CedantId,
                ["RiskQuarter"] = RiskQuarter,
                ["AccountFor"] = AccountFor,
                ["TotalPaid"] = totalPaid.HasValue ? TotalPaid : null,
                ["Year1st"] = year1st.HasValue ? Year1st : null,
                ["Renewal"] = renewal.HasValue ? Renewal : null,
                ["Gross1st"] = gross1st.HasValue ? Gross1st : null,
                ["GrossRenewal"] = grossRenewal.HasValue ? GrossRenewal : null,
                ["PreparedById"] = PreparedById,
                ["InvoiceStatus"] = InvoiceStatus,

                ["BatchNo"] = BatchNo,
                ["BatchDate"] = batchDate.HasValue ? BatchDate : null,
                ["NoOfInvoices"] = NoOfInvoices,
                ["BatchPersonInChargeId"] = BatchPersonInChargeId,
                ["BatchStatus"] = BatchStatus,

                ["Sort1Order"] = Sort1Order,
                ["Sort2Order"] = Sort2Order,
                ["ActiveTab"] = TabIndex,
                ["Results1Page"] = Results1Page,
                ["Results2Page"] = Results2Page,
            };
            ViewBag.Sort1Order = Sort1Order;
            ViewBag.Sort2Order = Sort2Order;
            ViewBag.Results1Page = Results1Page;
            ViewBag.Results2Page = Results2Page;

            ViewBag.SortOrder = TabIndex == 1 ? Sort1Order : Sort2Order;

            ViewBag.SortInvoiceNo = GetSortParam("InvoiceNo");
            ViewBag.SortInvoiceDate = GetSortParam("InvoiceDate");
            ViewBag.SortSttReceivedDate = GetSortParam("StatementReceivedDate");
            ViewBag.SortCedantId = GetSortParam("CedantId");
            ViewBag.SortRiskQuarter = GetSortParam("RiskQuarter");
            ViewBag.SortTreatyCodeId = GetSortParam("TreatyCodeId");
            ViewBag.SortAccountFor = GetSortParam("AccountFor");
            ViewBag.SortTotalPaid = GetSortParam("TotalPaid");
            ViewBag.Sort1stYear = GetSortParam("Year1st");
            ViewBag.SortRenewal = GetSortParam("Renewal");
            ViewBag.SortGross1st = GetSortParam("Gross1st");
            ViewBag.SortGrossRen = GetSortParam("GrossRenewal");
            ViewBag.SortPreparedBy = GetSortParam("PreparedById");

            ViewBag.SortBatchNo = GetSortParam("BatchNo");
            ViewBag.SortBatchDate = GetSortParam("BatchDate");
            ViewBag.SortTotalInvoices = GetSortParam("NoOfInvoices");
            ViewBag.SortPersonInCharge = GetSortParam("BatchPersonInChargeId");

            // PagedList - Invoice Register
            var query = _db.InvoiceRegisters.Select(InvoiceRegisterListingViewModel.Expression())
                .Where(q => q.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                .Where(q => q.ReportingType == InvoiceRegisterBo.ReportingTypeIFRS4);

            if (InvoiceType.HasValue) query = query.Where(q => q.InvoiceType == InvoiceType);
            if (!string.IsNullOrEmpty(InvoiceNo)) query = query.Where(q => q.InvoiceNo == InvoiceNo);
            if (invoiceDate.HasValue) query = query.Where(q => q.InvoiceDate >= invoiceDate && q.InvoiceDate <= invoiceDate);
            if (statementReceivedDate.HasValue) query = query.Where(q => q.StatementReceivedDate == statementReceivedDate);
            if (CedantId.HasValue) query = query.Where(q => q.Cedant.Id == CedantId);
            if (!string.IsNullOrEmpty(RiskQuarter)) query = query.Where(q => q.RiskQuarter == RiskQuarter);
            if (TreatyCodeId.HasValue) query = query.Where(q => (q.TreatyCode != null) && q.TreatyCode.Id == TreatyCodeId);
            if (!string.IsNullOrEmpty(AccountFor)) query = query.Where(q => q.AccountsFor == AccountFor);
            if (totalPaid.HasValue) query = query.Where(q => q.TotalPaid == totalPaid);
            if (year1st.HasValue) query = query.Where(q => q.Year1st == year1st);
            if (renewal.HasValue) query = query.Where(q => q.Renewal == renewal);
            if (gross1st.HasValue) query = query.Where(q => q.Gross1st == gross1st);
            if (grossRenewal.HasValue) query = query.Where(q => q.GrossRenewal == grossRenewal);
            if (PreparedById.HasValue) query = query.Where(q => q.PreparedById == PreparedById);
            if (InvoiceStatus.HasValue) query = query.Where(q => q.Status == InvoiceStatus);

            if (Sort1Order == Html.GetSortAsc("InvoiceNo")) query = query.OrderBy(q => q.InvoiceNo);
            else if (Sort1Order == Html.GetSortDsc("InvoiceNo")) query = query.OrderByDescending(q => q.InvoiceNo);
            else if (Sort1Order == Html.GetSortAsc("InvoiceDate")) query = query.OrderBy(q => q.InvoiceDate);
            else if (Sort1Order == Html.GetSortDsc("InvoiceDate")) query = query.OrderByDescending(q => q.InvoiceDate);
            else if (Sort1Order == Html.GetSortAsc("StatementReceivedDate")) query = query.OrderBy(q => q.StatementReceivedDate);
            else if (Sort1Order == Html.GetSortDsc("StatementReceivedDate")) query = query.OrderByDescending(q => q.StatementReceivedDate);
            else if (Sort1Order == Html.GetSortAsc("CedantId")) query = query.OrderBy(q => q.ClientName);
            else if (Sort1Order == Html.GetSortDsc("CedantId")) query = query.OrderByDescending(q => q.ClientName);
            else if (Sort1Order == Html.GetSortAsc("RiskQuarter")) query = query.OrderBy(q => q.RiskQuarter);
            else if (Sort1Order == Html.GetSortDsc("RiskQuarter")) query = query.OrderByDescending(q => q.RiskQuarter);
            else if (Sort1Order == Html.GetSortAsc("TreatyCodeId")) query = query.OrderBy(q => q.TreatyCode.Code);
            else if (Sort1Order == Html.GetSortDsc("TreatyCodeId")) query = query.OrderByDescending(q => q.TreatyCode.Code);
            else if (Sort1Order == Html.GetSortAsc("AccountFor")) query = query.OrderBy(q => q.AccountsFor);
            else if (Sort1Order == Html.GetSortDsc("AccountFor")) query = query.OrderByDescending(q => q.AccountsFor);
            else if (Sort1Order == Html.GetSortAsc("TotalPaid")) query = query.OrderBy(q => q.TotalPaid);
            else if (Sort1Order == Html.GetSortDsc("TotalPaid")) query = query.OrderByDescending(q => q.TotalPaid);
            else if (Sort1Order == Html.GetSortAsc("Year1st")) query = query.OrderBy(q => q.Year1st);
            else if (Sort1Order == Html.GetSortDsc("Year1st")) query = query.OrderByDescending(q => q.Year1st);
            else if (Sort1Order == Html.GetSortAsc("Renewal")) query = query.OrderBy(q => q.Renewal);
            else if (Sort1Order == Html.GetSortDsc("Renewal")) query = query.OrderByDescending(q => q.Renewal);
            else if (Sort1Order == Html.GetSortAsc("Gross1st")) query = query.OrderBy(q => q.Gross1st);
            else if (Sort1Order == Html.GetSortDsc("Gross1st")) query = query.OrderByDescending(q => q.Gross1st);
            else if (Sort1Order == Html.GetSortAsc("GrossRenewal")) query = query.OrderBy(q => q.GrossRenewal);
            else if (Sort1Order == Html.GetSortDsc("GrossRenewal")) query = query.OrderByDescending(q => q.GrossRenewal);
            else if (Sort1Order == Html.GetSortAsc("PreparedById")) query = query.OrderBy(q => q.PreparedByName);
            else if (Sort1Order == Html.GetSortDsc("PreparedById")) query = query.OrderByDescending(q => q.PreparedByName);
            else query = query.OrderBy(q => q.InvoiceNo);


            // PagedList - Invoice Register Batch
            var queryB = _db.InvoiceRegisterBatches.Select(InvoiceRegisterBatchListingViewModel.Expression());
            if (BatchNo.HasValue) queryB = queryB.Where(q => q.BatchNo == BatchNo);
            if (batchDate.HasValue) queryB = queryB.Where(q => q.BatchDate == batchDate);
            if (NoOfInvoices.HasValue) queryB = queryB.Where(q => q.TotalInvoice == NoOfInvoices);
            if (BatchPersonInChargeId.HasValue) queryB = queryB.Where(q => q.PersonInChargeId == BatchPersonInChargeId);
            if (BatchStatus.HasValue) queryB = queryB.Where(q => q.Status == BatchStatus);

            if (Sort2Order == Html.GetSortAsc("BatchNo")) queryB = queryB.OrderBy(q => q.BatchNo);
            else if (Sort2Order == Html.GetSortDsc("BatchNo")) queryB = queryB.OrderByDescending(q => q.BatchNo);
            else if (Sort2Order == Html.GetSortAsc("BatchDate")) queryB = queryB.OrderBy(q => q.BatchDate);
            else if (Sort2Order == Html.GetSortDsc("BatchDate")) queryB = queryB.OrderByDescending(q => q.BatchDate);
            else if (Sort2Order == Html.GetSortAsc("NoOfInvoices")) queryB = queryB.OrderBy(q => q.TotalInvoice);
            else if (Sort2Order == Html.GetSortDsc("NoOfInvoices")) queryB = queryB.OrderByDescending(q => q.TotalInvoice);
            else if (Sort2Order == Html.GetSortAsc("BatchPersonInChargeId")) queryB = queryB.OrderBy(q => q.PersonInChargeName);
            else if (Sort2Order == Html.GetSortDsc("BatchPersonInChargeId")) queryB = queryB.OrderByDescending(q => q.PersonInChargeName);
            else queryB = queryB.OrderBy(q => q.BatchNo);

            ViewBag.TotalInvoices = query.Count();
            ViewBag.TotalBatches = queryB.Count();

            model.InvoiceRegisters = query.ToPagedList(model.SearchResults1Page ?? 1, PageSize);
            model.InvoiceRegisterBatches = queryB.ToPagedList(Results2Page ?? 1, PageSize);

            IndexPage();
            return View(model);
        }

        // GET: InvoiceRegister/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new InvoiceRegisterBatchViewModel());
        }

        // POST: InvoiceRegister/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, InvoiceRegisterBatchViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);
                bo.BatchNo = (InvoiceRegisterBatchService.GetMaxId() + 1);

                TrailObject trail = GetNewTrailObject();
                Result = InvoiceRegisterBatchService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;
                    model.Status = bo.Status;

                    model.ProcessSoaDetails(form, AuthUserId);
                    model.ProcessStatusHistory(AuthUserId, ref trail);

                    List<RemarkBo> remarkBos = RemarkController.GetRemarks(form);
                    RemarkController.SaveRemarks(remarkBos, model.ModuleId, model.Id, AuthUserId, ref trail, AuthUser.DepartmentId);

                    CreateTrail(bo.Id, "Create Invoice Register Batch");

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }
            LoadPage();
            return View(model);
        }

        // GET: InvoiceRegister/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            InvoiceRegisterBatchBo irBatchBo = InvoiceRegisterBatchService.Find(id);
            if (irBatchBo == null)
            {
                return RedirectToAction("Index");
            }

            InvoiceRegisterBatchViewModel model = new InvoiceRegisterBatchViewModel(irBatchBo);
            LoadPage(model.ModuleId, irBatchBo);

            return View(model);
        }

        // POST: InvoiceRegister/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, InvoiceRegisterBatchViewModel model)
        {
            InvoiceRegisterBatchBo irBatchBo = InvoiceRegisterBatchService.Find(id);
            if (irBatchBo == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid && !CheckCutOffReadOnly(Controller))
            {
                bool statusChanged = false;
                if (model.Status != irBatchBo.Status)
                    statusChanged = true;

                var bo = model.FormBo(AuthUserId, AuthUserId);
                if (model.BatchNo.HasValue)
                    bo.BatchNo = model.BatchNo.Value;

                TrailObject trail = GetNewTrailObject();
                Result = InvoiceRegisterBatchService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.ProcessSoaDetails(form, AuthUserId);
                    if (statusChanged)
                        model.ProcessStatusHistory(AuthUserId, ref trail);
                    model.ProcessUploadSunglFile(form, AuthUserId, ref trail);

                    CreateTrail(irBatchBo.Id, "Update Invoice Register Batch");

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = irBatchBo.Id });
                }
                AddResult(Result);
            }

            LoadPage(model.ModuleId, irBatchBo);
            return View(model);
        }

        // GET: InvoiceRegister/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = InvoiceRegisterBatchService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            return View(new InvoiceRegisterBatchViewModel(bo));
        }

        // POST: InvoiceRegister/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id)
        {
            var bo = InvoiceRegisterBatchService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = InvoiceRegisterBatchService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Invoice Register Batch"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = bo.Id });
        }

        public ActionResult Details(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = InvoiceRegisterService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            InvoiceRegisterDetailViewModel model = new InvoiceRegisterDetailViewModel(bo);
            Dictionary<string, string> values = new Dictionary<string, string>();
            foreach (var property in model.GetType().GetProperties())
            {
                if (property.Name == "Id" || property.Name == "Status")
                    continue;

                string propertyName = "";
                if (property.Name == "DTH" || property.Name == "TPD" || property.Name == "CI" || property.Name == "PA" || property.Name == "HS")
                    propertyName = property.Name;
                else
                    propertyName = property.Name.ToProperCase(true);

                var value = property.GetValue(model);
                if (value == null || (value is string @s && string.IsNullOrEmpty(@s)))
                {
                    values[propertyName] = null;
                    continue;
                }
                values[propertyName] = value.ToString();
            }

            ViewBag.Values = values;

            return View(model);
        }

        public ActionResult Download(
            string downloadToken,
            int type,
            int? InvoiceType,
            int? TreatyCodeId,
            string InvoiceNo,
            string InvoiceDate,
            string SttReceivedDate,
            int? CedantId,
            string RiskQuarter,
            string AccountFor,
            string TotalPaid,
            string Year1st,
            string Renewal,
            string Gross1st,
            string GrossRen,
            int? PreparedById,
            int? Status)
        {
            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            dynamic Params = new ExpandoObject();
            Params.InvoiceType = InvoiceType;
            Params.TreatyCodeId = TreatyCodeId;
            Params.InvoiceNumber = InvoiceNo;
            Params.InvoiceDate = InvoiceDate;
            Params.SttReceivedDate = SttReceivedDate;
            Params.CedantId = CedantId;
            Params.RiskQuarter = RiskQuarter;
            Params.AccountFor = AccountFor;
            Params.Year1st = Year1st;
            Params.TotalPaid = TotalPaid;
            Params.Renewal = Renewal;
            Params.Gross1st = Gross1st;
            Params.GrossRen = GrossRen;
            Params.PreparedById = PreparedById;
            Params.Status = Status;

            var export = new GenerateExportData();
            ExportBo exportBo = export.CreateExportInvoiceRegister(AuthUserId, Params);

            if (exportBo.Total <= export.ExportInstantDownloadLimit)
            {
                export.Process(ref exportBo, _db);
                Session.Add("DownloadExport", true);
            }

            return RedirectToAction("Edit", "Export", new { exportBo.Id });
        }

        public void IndexPage()
        {
            CheckCutOffReadOnly(Controller);

            DropDownUser();
            DropDownBatchStatus();

            // Invoice Register tab
            DropDownEmpty();
            DropDownCedant();
            DropDownTreatyType();
            DropDownTreatyCode(foreign: false);
            DropDownInvoiceType();
            DropDownInvoiceStatus();
            ViewBag.DropDownLineOfBusiness = GetPickListDetailIdDropDown(StandardOutputBo.TypeLineOfBusiness);

            SetViewBagMessage();
        }

        public void LoadPage(int moduleId = 0, InvoiceRegisterBatchBo bo = null)
        {
            DropDownEmpty();
            DropDownCedant(CedantBo.StatusActive);
            DropDownUser();
            DropDownType();
            DropDownReportingType();
            DropDownBatchStatus();

            ViewBag.AuthUserName = AuthUserName();

            // Batch Status
            List<string> allStatusItems = new List<string>();
            for (int i = 0; i <= InvoiceRegisterBatchBo.StatusMax; i++)
            {
                allStatusItems.Add(InvoiceRegisterBatchBo.GetStatusName(i));
            }
            ViewBag.StatusHistoryStatusList = allStatusItems;

            // Soa Detail Status
            allStatusItems = new List<string>();
            for (int i = 0; i <= SoaDataBatchBo.StatusMax; i++)
            {
                allStatusItems.Add(SoaDataBatchBo.GetStatusName(i));
            }
            ViewBag.DetailStatusList = allStatusItems;

            allStatusItems = new List<string>();
            for (int i = 0; i <= SoaDataCompiledSummaryBo.InvoiceTypeMax; i++)
            {
                allStatusItems.Add(SoaDataCompiledSummaryBo.GetInvoiceTypeName(i));
            }
            ViewBag.InvoiceTypeItems = allStatusItems;

            allStatusItems = new List<string>();
            for (int i = 0; i <= InvoiceRegisterBatchFileBo.TypeMax; i++)
            {
                allStatusItems.Add(InvoiceRegisterBatchFileBo.GetTypeName(i));
            }
            ViewBag.UploadTypeItems = allStatusItems;

            if (bo != null)
            {
                // SoaDataDetail
                List<int> ids = InvoiceRegisterBatchSoaDataService.GetIdsByInvoiceRegisterBatchId(bo.Id);
                ViewBag.SoaDataDetails = SoaDataBatchService.GetByIds(ids);

                // SUNGL Files
                ViewBag.SunglFiles = InvoiceRegisterBatchFileService.GetByInvoiceRegisterBatchId(bo.Id, false);
                ViewBag.UploadFiles = InvoiceRegisterBatchFileService.GetByInvoiceRegisterBatchId(bo.Id, true);

                // Documents
                string downloadDocumentUrl = Url.Action("Download", "Document");
                IList<DocumentBo> documentBos = GetDocuments(moduleId, bo.Id, downloadDocumentUrl, true, AuthUser.DepartmentId);

                // Remarks
                IList<RemarkBo> remarkBos = RemarkService.GetByModuleIdObjectId(moduleId, bo.Id, true, AuthUser.DepartmentId);
                if (documentBos != null && documentBos.Count != 0)
                {
                    foreach (RemarkBo remarkBo in remarkBos)
                    {
                        remarkBo.DocumentBos = documentBos.Where(q => q.RemarkId.HasValue && q.RemarkId == remarkBo.Id).ToList();
                    }
                }
                ViewBag.Remarks = remarkBos;

                // StatusHistories
                ViewBag.StatusHistoryBos = StatusHistoryService.GetStatusHistoriesByModuleIdObjectId(moduleId, bo.Id);
                ViewBag.StatusFiles = InvoiceRegisterBatchStatusFileService.GetByInvoiceRegisterBatchId(bo.Id);

                ViewBag.CanEdit = CheckCutOffReadOnly(Controller);
                ViewBag.ShowSubmitForProcessing = InvoiceRegisterBatchBo.CanProcess(bo.Status);
                ViewBag.ShowSubmitForGenerate = InvoiceRegisterBatchBo.CanGenerate(bo.Status);
            }

            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownBatchStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= InvoiceRegisterBatchBo.StatusMax; i++)
            {
                items.Add(new SelectListItem { Text = InvoiceRegisterBatchBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.StatusItems = items;
            return items;
        }

        public List<SelectListItem> DropDownType()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, InvoiceRegisterBatchFileBo.TypeMax))
            {
                items.Add(new SelectListItem { Text = InvoiceRegisterBatchFileBo.GetTypeName(i), Value = i.ToString() });
            }
            ViewBag.TypeItems = items;
            return items;
        }

        public List<SelectListItem> DropDownInvoiceType()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, InvoiceRegisterBo.InvoiceTypeMax))
            {
                items.Add(new SelectListItem { Text = InvoiceRegisterBo.GetInvoiceTypeName(i), Value = i.ToString() });
            }
            ViewBag.InvoiceTypeItems = items;
            return items;
        }

        public List<SelectListItem> DropDownInvoiceStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, SoaDataBatchBo.InvoiceStatusMax))
            {
                items.Add(new SelectListItem { Text = SoaDataBatchBo.GetInvoiceStatusName(i), Value = i.ToString() });
            }
            ViewBag.InvoiceStatusItems = items;
            return items;
        }

        public List<SelectListItem> DropDownReportingType()
        {
            var items = GetEmptyDropDownList(false);
            foreach (var i in Enumerable.Range(1, SoaDataCompiledSummaryBo.ReportingTypeMax))
            {
                items.Add(new SelectListItem { Text = SoaDataCompiledSummaryBo.GetReportingTypeName(i), Value = i.ToString() });
            }
            ViewBag.ReportingTypeItems = items;
            return items;
        }

        [HttpPost]
        public ActionResult GetTreatyByCedant(int cedantId)
        {
            var treatyBos = TreatyService.GetByCedantId(cedantId);
            return Json(new { treatyBos });
        }

        [HttpPost]
        public JsonResult GetSoaData(int? cedantId, int? treatyId, string soaQuarter)
        {
            var soaDataBatchBos = SoaDataBatchService.FindByCedantIdTreatyIdQuarter(cedantId, treatyId, soaQuarter);
            return Json(new { soaDataBatchBos });
        }

        //[HttpPost]
        //public JsonResult Upload()
        //{
        //    string error = null;
        //    string tempFilePath = null;
        //    string hashFileName = null;

        //    if (Request.Files.Count > 0)
        //    {
        //        try
        //        {
        //            HttpFileCollectionBase files = Request.Files;

        //            string path = InvoiceRegisterBatchFileBo.GetTempFolderPath("Uploads");
        //            string fileName = Path.GetFileName(files[0].FileName);
        //            hashFileName = Hash.HashFileName(fileName);
        //            tempFilePath = string.Format("{0}/{1}", path, hashFileName);

        //            Util.MakeDir(tempFilePath);
        //            HttpPostedFileBase file = files[0];
        //            file.SaveAs(tempFilePath);
        //        }
        //        catch (Exception ex)
        //        {
        //            error = string.Format("Error occurred. Error details: ", ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        error = "No files selected.";
        //    }

        //    return Json(new { error, tempFilePath, hashFileName });
        //}

        public ActionResult FileDownload(int id)
        {
            InvoiceRegisterBatchFileBo bo = InvoiceRegisterBatchFileService.Find(id);
            DownloadFile(bo.GetLocalPathE1(), bo.FileName);
            return null;
        }

        public ActionResult StatusFileDownload(int id, int statusHistoryId)
        {
            InvoiceRegisterBatchStatusFileBo invoiceRegisterBatchStatusFileBo = InvoiceRegisterBatchStatusFileService.FindByInvoiceRegisterBatchIdStatusHistoryId(id, statusHistoryId);
            return File(System.IO.File.ReadAllBytes(invoiceRegisterBatchStatusFileBo.GetFilePath()), "text/plain", invoiceRegisterBatchStatusFileBo.GetFilePath().Split('/').Last());
        }

        public void DownloadFile(string filePath, string fileName)
        {
            try
            {
                Response.ClearContent();
                Response.Clear();
                Response.ContentType = MimeMapping.GetMimeMapping(fileName);
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                Response.TransmitFile(filePath);
                Response.Flush();
                Response.Close();
                Response.End();
            }
            catch
            {
                Response.Flush();
                Response.Close();
                Response.End();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U", ReturnController = Controller)]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            if (ModelState.IsValid && !CheckCutOffReadOnly(Controller))
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var process = new ProcessInvoiceRegister()
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

                    if (create != 0 || update != 0 || delete != 0)
                    {
                        SetSuccessSessionMsg(string.Format("Process File Successful, Total Create: {0}, Total Update: {1}, Total Delete: {2}", create, update, delete));
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetTreatyCodeByCedant(int cedantId)
        {
            IList<TreatyCodeBo> treatyCodeBos = TreatyCodeService.GetByCedantId(cedantId);
            return Json(new { treatyCodeBos });
        }

        [HttpPost]
        public JsonResult GetCompiledSummaryByReportingType(int InvoiceRegisterBatchId, int ReportingTypeId)
        {
            List<int> ids = InvoiceRegisterBatchSoaDataService.GetIdsByInvoiceRegisterBatchId(InvoiceRegisterBatchId);

            var WMCompiledSummaryMYRBos = SoaDataCompiledSummaryService.GetBySoaDataBatchIdsCode(ids, ReportingTypeId, false);
            var WMCompiledSummaryOCBos = SoaDataCompiledSummaryService.GetBySoaDataBatchIdsCode(ids, ReportingTypeId, false, true);
            var SFCompiledSummaryBos = SoaDataCompiledSummaryService.GetBySoaDataBatchIdsCode(ids, ReportingTypeId, true);

            return Json(new { WMCompiledSummaryMYRBos, WMCompiledSummaryOCBos, SFCompiledSummaryBos });
        }
    }
}