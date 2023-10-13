using BusinessObject;
using BusinessObject.Identity;
using ConsoleApp.Commands;
using ConsoleApp.Commands.ProcessDatas;
using PagedList;
using Services;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class RetroRegisterController : BaseController
    {
        public const string Controller = "RetroRegister";

        // GET: RetroRegister
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            RetroRegisterViewModel model,
            int? Type,
            string InvoiceNo,
            string InvoiceDate,
            string ReportCompletedDate,
            string RetroConfirmationDate,
            int? CedantId,
            string RiskQuarter,
            int? RetroPartyId,
            int? TreatyCodeId,
            string AccountFor,
            string TotalPaid,
            string Year1st,
            string Renewal,
            string Gross1st,
            string GrossRenewal,
            int? PreparedById,
            int? ApprovalStatus,
            int? RetroStatus,
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
            DateTime? reportCompletedDate = Util.GetParseDateTime(ReportCompletedDate);
            DateTime? retroConfirmationDate = Util.GetParseDateTime(RetroConfirmationDate);
            double? totalPaid = Util.StringToDouble(TotalPaid);
            double? year1st = Util.StringToDouble(Year1st);
            double? renewal = Util.StringToDouble(Renewal);
            double? gross1st = Util.StringToDouble(Gross1st);
            double? grossRenewal = Util.StringToDouble(GrossRenewal);
            DateTime? batchDate = Util.GetParseDateTime(BatchDate);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["InvoiceNo"] = InvoiceNo,
                ["InvoiceDate"] = invoiceDate.HasValue ? InvoiceDate : null,
                ["ReportCompletedDate"] = reportCompletedDate.HasValue ? ReportCompletedDate : null,
                ["RetroConfirmationDate"] = retroConfirmationDate.HasValue ? RetroConfirmationDate : null,
                ["CedantId"] = CedantId,
                ["RiskQuarter"] = RiskQuarter,
                ["RetroCode"] = RetroPartyId,
                ["TreatyCodeId"] = TreatyCodeId,
                ["AccountFor"] = AccountFor,
                ["TotalPaid"] = totalPaid.HasValue ? TotalPaid : null,
                ["Year1st"] = year1st.HasValue ? Year1st : null,
                ["Renewal"] = renewal.HasValue ? Renewal : null,
                ["Gross1st"] = gross1st.HasValue ? Gross1st : null,
                ["GrossRen"] = grossRenewal.HasValue ? GrossRenewal : null,
                ["PreparedById"] = PreparedById,
                ["ApprovalStatus"] = ApprovalStatus.HasValue ? ApprovalStatus : null,
                ["RetroStatus"] = RetroStatus.HasValue ? RetroStatus : null,
                ["Type"] = Type,

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
            ViewBag.SortReportCompletedDate = GetSortParam("ReportCompletedDate");
            ViewBag.SortRetroConfirmationDate = GetSortParam("RetroConfirmationDate");
            ViewBag.SortCedantId = GetSortParam("CedantId");
            ViewBag.SortRiskQuarter = GetSortParam("RiskQuarter");
            ViewBag.SortRetroId = GetSortParam("RetroCode");
            ViewBag.SortTreatyCodeId = GetSortParam("TreatyCodeId");
            ViewBag.SortAccountFor = GetSortParam("AccountFor");
            ViewBag.SortTotalPaid = GetSortParam("TotalPaid");
            ViewBag.Sort1stYear = GetSortParam("Year1st");
            ViewBag.SortRenewal = GetSortParam("Renewal");
            ViewBag.SortGross1st = GetSortParam("Gross1st");
            ViewBag.SortGrossRen = GetSortParam("GrossRenewal");
            ViewBag.SortPreparedBy = GetSortParam("PreparedById");
            ViewBag.SortType = GetSortParam("Type");

            ViewBag.SortBatchNo = GetSortParam("BatchNo");
            ViewBag.SortBatchType = GetSortParam("BatchType");
            ViewBag.SortBatchDate = GetSortParam("BatchDate");
            ViewBag.SortTotalInvoices = GetSortParam("NoOfInvoices");
            ViewBag.SortPersonInCharge = GetSortParam("BatchPersonInChargeId");

            // PagedList - Retro Register
            var query = _db.RetroRegisters
                .Where(q => q.ReportingType != RetroRegisterBo.ReportingTypeIFRS17)
                .Where(q => q.Type == RetroRegisterBo.TypeDirectRetro || (q.Type == RetroRegisterBo.TypePerLifeRetro && q.TreatyType == "L-YRT"))
                .Select(RetroRegisterListingViewModel.Expression());

            if (Type.HasValue) query = query.Where(q => q.Type == Type);
            if (!string.IsNullOrEmpty(InvoiceNo)) query = query.Where(q => q.InvoiceNo == InvoiceNo);
            if (invoiceDate.HasValue) query = query.Where(q => q.InvoiceDate == invoiceDate);
            if (reportCompletedDate.HasValue) query = query.Where(q => q.ReportCompletedDate == reportCompletedDate);
            if (retroConfirmationDate.HasValue) query = query.Where(q => q.RetroConfirmationDate == retroConfirmationDate);
            if (CedantId.HasValue) query = query.Where(q => q.CedantId == CedantId);
            if (RetroPartyId.HasValue) query = query.Where(q => q.RetoPartyId == RetroPartyId);
            if (TreatyCodeId.HasValue) query = query.Where(q => (q.TreatyCode != null) && q.TreatyCode.Id == TreatyCodeId);
            if (!string.IsNullOrEmpty(RiskQuarter)) query = query.Where(q => q.RiskQuarter == RiskQuarter);
            if (!string.IsNullOrEmpty(AccountFor)) query = query.Where(q => q.AccountsFor == AccountFor);
            if (totalPaid.HasValue) query = query.Where(q => q.TotalPaid == totalPaid);
            if (year1st.HasValue) query = query.Where(q => q.Year1st == year1st);
            if (renewal.HasValue) query = query.Where(q => q.Renewal == renewal);
            if (gross1st.HasValue) query = query.Where(q => q.Gross1st == gross1st);
            if (grossRenewal.HasValue) query = query.Where(q => q.GrossRenewal == grossRenewal);
            if (PreparedById.HasValue) query = query.Where(q => q.PreparedById == PreparedById);
            if (ApprovalStatus.HasValue) query = query.Where(q => q.Status == ApprovalStatus);
            if (RetroStatus.HasValue) query = query.Where(q => (q.DirectRetro != null) && q.DirectRetro.RetroStatus == RetroStatus);

            if (Sort1Order == Html.GetSortAsc("InvoiceNo")) query = query.OrderBy(q => q.InvoiceNo);
            else if (Sort1Order == Html.GetSortDsc("InvoiceNo")) query = query.OrderByDescending(q => q.InvoiceNo);
            else if (Sort1Order == Html.GetSortAsc("InvoiceDate")) query = query.OrderBy(q => q.InvoiceDate);
            else if (Sort1Order == Html.GetSortDsc("InvoiceDate")) query = query.OrderByDescending(q => q.InvoiceDate);
            else if (Sort1Order == Html.GetSortAsc("ReportCompletedDate")) query = query.OrderBy(q => q.ReportCompletedDate);
            else if (Sort1Order == Html.GetSortDsc("ReportCompletedDate")) query = query.OrderByDescending(q => q.ReportCompletedDate);
            else if (Sort1Order == Html.GetSortAsc("RetroConfirmationDate")) query = query.OrderBy(q => q.RetroConfirmationDate);
            else if (Sort1Order == Html.GetSortDsc("RetroConfirmationDate")) query = query.OrderByDescending(q => q.RetroConfirmationDate);
            else if (Sort1Order == Html.GetSortAsc("CedantId")) query = query.OrderBy(q => q.ClientName);
            else if (Sort1Order == Html.GetSortDsc("CedantId")) query = query.OrderByDescending(q => q.ClientName);
            else if (Sort1Order == Html.GetSortAsc("RiskQuarter")) query = query.OrderBy(q => q.RiskQuarter);
            else if (Sort1Order == Html.GetSortDsc("RiskQuarter")) query = query.OrderByDescending(q => q.RiskQuarter);
            else if (Sort1Order == Html.GetSortAsc("RetroCode")) query = query.OrderBy(q => q.RetroPartyName);
            else if (Sort1Order == Html.GetSortDsc("RetroCode")) query = query.OrderByDescending(q => q.RetroPartyName);
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
            else if (Sort1Order == Html.GetSortAsc("Type")) query = query.OrderBy(q => q.Type);
            else if (Sort1Order == Html.GetSortDsc("Type")) query = query.OrderByDescending(q => q.Type);
            else query = query.OrderBy(q => q.InvoiceNo);

            // PagedList - Retro Register Batch
            var queryB = _db.RetroRegisterBatches.Select(RetroRegisterBatchListingViewModel.Expression());
            if (model.Batch != null)
            {
                if (model.Batch.BatchNo.HasValue) queryB = queryB.Where(q => q.BatchNo == model.Batch.BatchNo);
                if (model.Batch.BatchType != 0) queryB = queryB.Where(q => q.BatchType == model.Batch.BatchType);
                if (model.Batch.TotalInvoice.HasValue) queryB = queryB.Where(q => q.TotalInvoice == model.Batch.TotalInvoice);
                if (model.Batch.PersonInChargeId.HasValue) queryB = queryB.Where(q => q.PersonInChargeId == model.Batch.PersonInChargeId);
            }

            if (Sort2Order == Html.GetSortAsc("BatchNo")) queryB = queryB.OrderBy(q => q.BatchNo);
            else if (Sort2Order == Html.GetSortDsc("BatchNo")) queryB = queryB.OrderByDescending(q => q.BatchNo);
            else if (Sort2Order == Html.GetSortAsc("BatchType")) queryB = queryB.OrderBy(q => q.BatchType);
            else if (Sort2Order == Html.GetSortDsc("BatchType")) queryB = queryB.OrderByDescending(q => q.BatchType);
            else if (Sort2Order == Html.GetSortAsc("BatchDate")) queryB = queryB.OrderBy(q => q.BatchDate);
            else if (Sort2Order == Html.GetSortDsc("BatchDate")) queryB = queryB.OrderByDescending(q => q.BatchDate);
            else if (Sort2Order == Html.GetSortAsc("NoOfInvoices")) queryB = queryB.OrderBy(q => q.TotalInvoice);
            else if (Sort2Order == Html.GetSortDsc("NoOfInvoices")) queryB = queryB.OrderByDescending(q => q.TotalInvoice);
            else if (Sort2Order == Html.GetSortAsc("BatchPersonInChargeId")) queryB = queryB.OrderBy(q => q.PersonInChargeName);
            else if (Sort2Order == Html.GetSortDsc("BatchPersonInChargeId")) queryB = queryB.OrderByDescending(q => q.PersonInChargeName);
            else queryB = queryB.OrderBy(q => q.BatchNo);

            ViewBag.TotalInvoices = query.Count();
            ViewBag.TotalBatches = queryB.Count();

            model.RetroRegisters = query.ToPagedList(model.SearchResults1Page ?? 1, PageSize);
            model.RetroRegisterBatches = queryB.ToPagedList(model.SearchResults2Page ?? 1, PageSize);

            IndexPage();
            return View(model);
        }

        // GET: RetroRegister/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new RetroRegisterBatchViewModel());
        }

        // POST: RetroRegister/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, RetroRegisterBatchViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);
                bo.BatchNo = (RetroRegisterBatchService.GetMaxId() + 1);

                TrailObject trail = GetNewTrailObject();
                Result = RetroRegisterBatchService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;
                    model.Status = bo.Status;

                    model.ProcessDirectRetroDetails(form, AuthUserId);
                    model.ProcessStatusHistory(AuthUserId, ref trail);

                    List<RemarkBo> remarkBos = RemarkController.GetRemarks(form);
                    RemarkController.SaveRemarks(remarkBos, model.ModuleId, model.Id, AuthUserId, ref trail, AuthUser.DepartmentId);

                    CreateTrail(bo.Id, "Create Retro Register Batch");

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }
            LoadPage();
            return View(model);
        }

        // GET: RetroRegister/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            RetroRegisterBatchBo batchBo = RetroRegisterBatchService.Find(id);
            if (batchBo == null)
            {
                return RedirectToAction("Index");
            }

            RetroRegisterBatchViewModel model = new RetroRegisterBatchViewModel(batchBo);
            LoadPage(model.ModuleId, batchBo);

            return View(model);
        }

        // POST: RetroRegister/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, RetroRegisterBatchViewModel model)
        {
            RetroRegisterBatchBo batchBo = RetroRegisterBatchService.Find(id);
            if (batchBo == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid && !CheckCutOffReadOnly(Controller, id))
            {
                bool statusChanged = false;
                if (model.Status != batchBo.Status)
                    statusChanged = true;

                var bo = model.FormBo(AuthUserId, AuthUserId);
                if (model.BatchNo.HasValue)
                    bo.BatchNo = model.BatchNo.Value;

                TrailObject trail = GetNewTrailObject();
                Result = RetroRegisterBatchService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    if (RetroRegisterBatchBo.CanProcess(bo.Status))
                        model.ProcessDirectRetroDetails(form, AuthUserId);
                    if (statusChanged)
                        model.ProcessStatusHistory(AuthUserId, ref trail);

                    CreateTrail(batchBo.Id, "Update Retro Register Batch");

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = batchBo.Id });
                }
                AddResult(Result);
            }

            LoadPage(model.ModuleId, batchBo);
            return View(model);
        }

        // GET: RetroRegister/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = RetroRegisterBatchService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            return View(new RetroRegisterBatchViewModel(bo));
        }

        // POST: RetroRegister/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id)
        {
            var bo = RetroRegisterBatchService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = RetroRegisterBatchService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Retro Register Batch"
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
            RetroRegisterDetailViewModel model = new RetroRegisterDetailViewModel();
            if (id != 0)
            {
                if (!CheckEditPageReadOnly(Controller))
                    return RedirectDashboard();

                var bo = RetroRegisterService.Find(id);
                if (bo == null)
                    return RedirectToAction("Index");

                model = new RetroRegisterDetailViewModel(bo);
            }
            else
            {
                if (CheckCutOffReadOnly(Controller))
                    return RedirectToAction("Index");

                model.Type = RetroRegisterBo.TypePerLifeRetro;
                model.PreparedById = AuthUserId;
            }

            DetailPage(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int id, RetroRegisterDetailViewModel model)
        {
            RetroRegisterBo bo = new RetroRegisterBo();
            TrailObject trail = GetNewTrailObject();

            if (CheckCutOffReadOnly(Controller))
            {
                DetailPage(model);
                return View(model);
            }

            if (id != 0)
            {
                bo = RetroRegisterService.Find(id);
                if (bo == null)
                    return RedirectToAction("Index");

                if (bo.Status == RetroRegisterBo.StatusPendingApproval && !CheckPower(Controller, AccessMatrixBo.PowerApprovalRetroRegister))
                {
                    Result.AddError("You dont have access to Approve or Reject Retro Register");
                }

                if (ModelState.IsValid)
                {
                    model.Get(ref bo);
                    bo.UpdatedById = AuthUserId;

                    Result = RetroRegisterService.Update(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        CreateTrail(bo.Id, "Update Retro Register");

                        SetUpdateSuccessMessage(Controller);
                        return RedirectToAction("Details", new { id = bo.Id });
                    }
                    AddResult(Result);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    model.Get(ref bo);
                    bo.CreatedById = AuthUserId;
                    bo.UpdatedById = AuthUserId;

                    bo.RetroStatementNo = RetroRegisterService.GetNextStatementNo(DateTime.Now.Year, RetroRegisterBo.GetRetroTypeName(bo.RetroStatementType));

                    Result = RetroRegisterService.Create(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        CreateTrail(
                            bo.Id,
                            "Create Retro Register"
                        );

                        SetCreateSuccessMessage(Controller);
                        return RedirectToAction("Details", new { id = bo.Id });
                    }
                    AddResult(Result);
                }
            }

            DetailPage(model);
            return View(model);
        }

        public ActionResult Download(
            string downloadToken,
            int type,
            int? RetroType,
            string InvoiceNo,
            string InvoiceDate,
            string ReportCompletedDate,
            string RetroConfirmationDate,
            int? CedantId,
            string RiskQuarter,
            int? RetroPartyId,
            int? TreatyCodeId,
            string AccountFor,
            string TotalPaid,
            string Year1st,
            string Renewal,
            string Gross1st,
            string GrossRenewal,
            int? PreparedById,
            int? ApprovalStatus,
            int? RetroStatus)
        {
            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            dynamic Params = new ExpandoObject();
            Params.Type = RetroType;
            Params.TreatyCodeId = TreatyCodeId;
            Params.InvoiceNumber = InvoiceNo;
            Params.InvoiceDate = InvoiceDate;
            Params.ReportCompletedDate = ReportCompletedDate;
            Params.RetroConfirmationDate = RetroConfirmationDate;
            Params.CedantId = CedantId;
            Params.RetroPartyId = RetroPartyId;
            Params.RiskQuarter = RiskQuarter;
            Params.AccountFor = AccountFor;
            Params.Year1st = Year1st;
            Params.TotalPaid = TotalPaid;
            Params.Renewal = Renewal;
            Params.Gross1st = Gross1st;
            Params.GrossRen = GrossRenewal;
            Params.PreparedById = PreparedById;
            Params.ApprovalStatus = ApprovalStatus;
            Params.RetroStatus = RetroStatus;

            var export = new GenerateExportData();
            ExportBo exportBo = export.CreateExportRetroRegister(AuthUserId, Params);

            if (exportBo.Total <= export.ExportInstantDownloadLimit)
            {
                export.Process(ref exportBo, _db);
                Session.Add("DownloadExport", true);
            }

            return RedirectToAction("Edit", "Export", new { exportBo.Id });
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
                    var process = new ProcessRetroRegister()
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

        public void IndexPage()
        {
            CheckCutOffReadOnly(Controller);

            DropDownUser();
            DropDownBatchStatus();
            DropDownType();

            // Retro Register tab
            DropDownEmpty();
            DropDownCedant();
            DropDownTreatyCode(foreign: false);
            DropDownRetroParty();
            DropDownApprovalStatus();
            DropDownRetroStatus();
            SetViewBagMessage();
        }

        public void LoadPage(int moduleId = 0, RetroRegisterBatchBo bo = null)
        {
            DropDownEmpty();
            DropDownCedant(CedantBo.StatusActive);
            DropDownUser();
            DropDownType();
            DropDownBatchStatus();

            ViewBag.AuthUserName = AuthUserName();

            // Batch Status
            List<string> allStatusItems = new List<string>();
            for (int i = 0; i <= RetroRegisterBatchBo.StatusMax; i++)
            {
                allStatusItems.Add(RetroRegisterBatchBo.GetStatusName(i));
            }
            ViewBag.StatusHistoryStatusList = allStatusItems;

            // Direct Retro Status
            allStatusItems = new List<string>();
            for (int i = 0; i <= DirectRetroBo.RetroStatusMax; i++)
            {
                allStatusItems.Add(DirectRetroBo.GetRetroStatusName(i));
            }
            ViewBag.DetailStatusList = allStatusItems;

            if (bo != null)
            {
                // Quarterly Report
                List<int> ids = RetroRegisterBatchDirectRetroService.GetIdsByRetroRegisterBatchId(bo.Id);
                ViewBag.DirectRetroDetails = DirectRetroService.GetByIds(ids);

                // SUNGL Files
                ViewBag.SunglFiles = RetroRegisterBatchFileService.GetByRetroRegisterBatchId(bo.Id);

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
                ViewBag.StatusFiles = RetroRegisterBatchStatusFileService.GetByRetroRegisterBatchId(bo.Id);

                ViewBag.CanEdit = CheckCutOffReadOnly(Controller, bo.Id);
                ViewBag.ShowSubmitForProcessing = RetroRegisterBatchBo.CanProcess(bo.Status);
                ViewBag.ShowSubmitForGenerate = RetroRegisterBatchBo.CanGenerate(bo.Status);
            }

            SetViewBagMessage();
        }

        public void DetailPage(RetroRegisterDetailViewModel model = null)
        {
            DropDownEmpty();
            DropDownUser();
            DropDownRetroType();
            DropDownType();

            if (model != null)
            {
                DropDownCedant(CedantBo.StatusActive, model.CedantId);
                DropDownTreatyCode(TreatyCodeBo.StatusActive, model.TreatyCodeId, foreign: false);
                DropDownRetroParty(RetroPartyBo.StatusActive, model.RetroPartyId);

                // SUNGL Files
                ViewBag.SunglFiles = RetroRegisterFileService.GetByRetroRegisterId(model.Id);

            }
            else
            {
                DropDownCedant(CedantBo.StatusActive);
                DropDownTreatyCode(TreatyCodeBo.StatusActive, foreign: false);
                DropDownRetroParty(RetroPartyBo.StatusActive);
            }

            EnableSubmitForApproval(model);
            EnableApproval(model);
            EnableGenerateIFRS(model);
            EnabledEdit(model);

            Dictionary<string, string> values = new Dictionary<string, string>();
            foreach (var property in model.GetType().GetProperties())
            {
                if (property.Name == "Id")
                    continue;

                string propertyName = property.Name.ToProperCase(true);

                var value = property.GetValue(model);
                if (value == null || (value is string @s && string.IsNullOrEmpty(@s)))
                {
                    values[propertyName] = null;
                    continue;
                }
                values[propertyName] = value.ToString();
            }
            ViewBag.Values = values;

            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownBatchStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= RetroRegisterBatchBo.StatusMax; i++)
            {
                items.Add(new SelectListItem { Text = RetroRegisterBatchBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.StatusItems = items;
            return items;
        }

        public List<SelectListItem> DropDownApprovalStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= RetroRegisterBo.StatusMax; i++)
            {
                items.Add(new SelectListItem { Text = RetroRegisterBo.GetStatusApprovalName(i), Value = i.ToString() });
            }
            ViewBag.ApprovalStatusItems = items;
            return items;
        }

        public List<SelectListItem> DropDownRetroStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= DirectRetroBo.RetroStatusMax; i++)
            {
                items.Add(new SelectListItem { Text = DirectRetroBo.GetRetroStatusName(i), Value = i.ToString() });
            }
            ViewBag.RetroStatusItems = items;
            return items;
        }

        public List<SelectListItem> DropDownRetroType()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= RetroRegisterBo.RetroTypeMax; i++)
            {
                items.Add(new SelectListItem { Text = RetroRegisterBo.GetRetroTypeName(i), Value = i.ToString() });
            }
            ViewBag.DropDownRetroTypes = items;
            return items;
        }

        public List<SelectListItem> DropDownType()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= RetroRegisterBo.TypeMax; i++)
            {
                items.Add(new SelectListItem { Text = RetroRegisterBo.GetTypeName(i), Value = i.ToString() });
            }
            ViewBag.DropDownTypes = items;
            return items;
        }

        public List<SelectListItem> DropDownAccountFor(int? treatyCodeId = null)
        {
            var items = GetEmptyDropDownList();
            if (treatyCodeId.HasValue)
            {
                TreatyCodeBo treatyCodeBo = TreatyCodeService.Find(treatyCodeId);
                if (!string.IsNullOrEmpty(treatyCodeBo.AccountFor))
                {
                    string[] accounts = treatyCodeBo.AccountFor.Split(',').ToArray();
                    foreach (string code in accounts)
                    {
                        items.Add(new SelectListItem { Text = code, Value = code });
                    }
                }
            }
            ViewBag.DropDownAccountFor = items;
            return items;
        }

        [HttpPost]
        public ActionResult GetTreatyCodeByCedant(int cedantId)
        {
            var treatyCodeBos = TreatyCodeService.GetByCedantId(cedantId);
            return Json(new { treatyCodeBos });
        }

        [HttpPost]
        public JsonResult GetTreatyCodeDetails(int treatyCodeId)
        {
            TreatyCodeBo treatyCodeBo = TreatyCodeService.Find(treatyCodeId);

            string treatyNo = treatyCodeBo.TreatyNo;
            string treatyType = treatyCodeBo.TreatyTypePickListDetailBo?.Code;
            string lob = treatyCodeBo.LineOfBusinessPickListDetailBo?.Code;

            return Json(new { treatyNo, treatyType, lob });
        }

        [HttpPost]
        public JsonResult GetRetroPartyDetails(int retroPartyId)
        {
            RetroPartyBo retroPartyBo = RetroPartyService.Find(retroPartyId);

            string retroName = retroPartyBo.Name;
            string partyCode = retroPartyBo.Party;

            return Json(new { retroName, partyCode });
        }

        [HttpPost]
        public JsonResult GetRetroData(int? cedantId, int? treatyCodeId, string soaQuarter)
        {
            var directRetroBos = DirectRetroService.FindByCedantIdTreatyCodeIdQuarter(cedantId, treatyCodeId, soaQuarter);
            return Json(new { directRetroBos });
        }

        public ActionResult FileDownload(int id)
        {
            RetroRegisterBatchFileBo bo = RetroRegisterBatchFileService.Find(id);
            DownloadFile(bo.GetLocalPathE2(), bo.FileName);
            return null;
        }

        public ActionResult RetroRegisterFileDownload(int id)
        {
            RetroRegisterFileBo bo = RetroRegisterFileService.Find(id);
            DownloadFile(bo.GetLocalPathE2(), bo.FileName);
            return null;
        }

        public ActionResult StatusFileDownload(int id, int statusHistoryId)
        {
            RetroRegisterBatchStatusFileBo retroRegisterBatchStatusFileBo = RetroRegisterBatchStatusFileService.FindByRetroRegisterBatchIdStatusHistoryId(id, statusHistoryId);
            return File(System.IO.File.ReadAllBytes(retroRegisterBatchStatusFileBo.GetFilePath()), "text/plain", retroRegisterBatchStatusFileBo.GetFilePath().Split('/').Last());
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

        public ActionResult Generate(int id, int type, string downloadToken)
        {
            // type 1 = IFRS4
            // type 2 = IFRS17

            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            var bo = RetroRegisterService.Find(id);
            if (bo == null)
                return RedirectToAction("Index");

            var retroE2 = new GenerateE2();
            if (type == 1)
            {
                retroE2.RetroRegisterBo = bo;
                retroE2.GenerateIFRS4 = true;
                retroE2.GenerateE2IFRS4();
            }

            if (type == 2)
            {
                var bos = RetroRegisterService.FindByInvoiceReferenceIfrs17(bo.RetroStatementNo);

                retroE2.RetroRegisterBo = bo;
                retroE2.RetroRegisterBos = bos;
                retroE2.GenerateIFRS17 = true;
                retroE2.GenerateE2IFRS17();
            }

            var retroRegisterFileBo = new RetroRegisterFileBo()
            {
                FileName = retroE2.FileName,
                RetroRegisterId = id,
                CreatedById = AuthUserId,
            };
            retroRegisterFileBo.FormatHashFileName();
            string path = retroRegisterFileBo.GetLocalPathE2();

            RetroRegisterFileService.Create(ref retroRegisterFileBo);

            return File(retroE2.FilePath, MimeMapping.GetMimeMapping(retroE2.FileName), retroE2.FileName);
        }

        public void EnableSubmitForApproval(RetroRegisterDetailViewModel bo)
        {
            bool isEnable = (bo.Type != RetroRegisterBo.TypeDirectRetro && (!bo.Status.HasValue || bo.Status == RetroRegisterBo.StatusRejected));
            ViewBag.EnableSubmitForApproval = isEnable;
        }

        public void EnableApproval(RetroRegisterDetailViewModel bo)
        {
            bool isEnable = (bo.Status == RetroRegisterBo.StatusPendingApproval && CheckPower(Controller, AccessMatrixBo.PowerApprovalRetroRegister));
            ViewBag.EnableApproval = isEnable;
        }

        public void EnableGenerateIFRS(RetroRegisterDetailViewModel bo)
        {
            bool isEnable = (bo.Status == RetroRegisterBo.StatusApproved || (bo.Type == RetroRegisterBo.TypeDirectRetro && bo.Id != 0));
            ViewBag.EnableGenerateIFRS = isEnable;
        }

        public void EnabledEdit(RetroRegisterDetailViewModel bo)
        {
            bool isEnable = false;

            if (CheckCutOffReadOnly(Controller, bo.Id))
            {
                ViewBag.EnableSubmitForApproval = false;
                ViewBag.EnabledEdit = isEnable;
                return;
            }

            if (bo.Type == RetroRegisterBo.TypePerLifeRetro)
                isEnable = (bo.Status != RetroRegisterBo.StatusPendingApproval && bo.Status != RetroRegisterBo.StatusApproved);
            else
                isEnable = (bo.DirectRetroBo?.RetroStatus != DirectRetroBo.RetroStatusStatementIssued);

            ViewBag.EnabledEdit = isEnable;
        }
    }
}