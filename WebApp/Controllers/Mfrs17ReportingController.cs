using BusinessObject;
using ConsoleApp.Commands;
using Ionic.Zip;
using PagedList;
using Services;
using Services.RiDatas;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data;
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
    public class Mfrs17ReportingController : BaseController
    {
        public const string Controller = "Mfrs17Reporting";

        // GET: Mfrs17Reporting
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(string Quarter, int? CutOffId, int? TotalRecord, int? Status, string SortOrder, int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Quarter"] = Quarter,
                ["CutOffId"] = CutOffId,
                ["TotalRecord"] = TotalRecord,
                ["Status"] = Status,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortQuarter = GetSortParam("Quarter");
            ViewBag.SortCutOffQuarter = GetSortParam("CutOffQuarter");
            ViewBag.SortTotalRecord = GetSortParam("TotalRecord");

            var query = _db.Mfrs17Reportings.Select(Mfrs17ReportingViewModel.Expression());

            if (!string.IsNullOrEmpty(Quarter)) query = query.Where(q => q.Quarter == Quarter);
            if (CutOffId.HasValue) query = query.Where(q => q.CutOffId == CutOffId);
            if (TotalRecord.HasValue) query = query.Where(q => q.TotalRecord == TotalRecord);
            if (Status.HasValue) query = query.Where(q => q.Status == Status);

            if (SortOrder == Html.GetSortAsc("Quarter")) query = query.OrderBy(q => q.Quarter);
            else if (SortOrder == Html.GetSortDsc("Quarter")) query = query.OrderByDescending(q => q.Quarter);
            else if (SortOrder == Html.GetSortAsc("CutOffId")) query = query.OrderBy(q => q.CutOffId);
            else if (SortOrder == Html.GetSortDsc("CutOffId")) query = query.OrderByDescending(q => q.CutOffId);
            else if (SortOrder == Html.GetSortAsc("TotalRecord")) query = query.OrderBy(q => q.TotalRecord);
            else if (SortOrder == Html.GetSortDsc("TotalRecord")) query = query.OrderByDescending(q => q.TotalRecord);
            else query = query.OrderByDescending(q => q.Quarter);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: Mfrs17Reporting/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            PageLoad();
            return View(new Mfrs17ReportingViewModel());
        }

        // POST: Mfrs17Reporting/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, Mfrs17ReportingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var mfrs17ReportingBo = model.FormBo(AuthUserId, AuthUserId);
                TrailObject trail = GetNewTrailObject();
                Result = Mfrs17ReportingService.Create(ref mfrs17ReportingBo, ref trail);
                if (Result.Valid)
                {
                    model.Id = mfrs17ReportingBo.Id;
                    RemarkController.SaveRemarks(RemarkController.GetRemarks(form), model.ModuleId, mfrs17ReportingBo.Id, AuthUserId, ref trail);
                    model.ProcessStatusHistory(form, AuthUserId, ref trail);

                    CreateTrail(
                        mfrs17ReportingBo.Id,
                        "Create MFRS17 Reporting"
                    );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = mfrs17ReportingBo.Id });
                }
                AddResult(Result);
            }
            PageLoad();
            return View(model);
        }

        // GET: Mfrs17Reporting/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            Mfrs17ReportingBo mfrs17ReportingBo = Mfrs17ReportingService.Find(id);
            if (mfrs17ReportingBo == null)
            {
                return RedirectToAction("Index");
            }
            Mfrs17ReportingViewModel model = new Mfrs17ReportingViewModel(mfrs17ReportingBo);
            PageLoad(model.ModuleId, mfrs17ReportingBo);

            return View(model);
        }

        // POST: Mfrs17Reporting/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, Mfrs17ReportingViewModel model)
        {
            Mfrs17ReportingBo mfrs17ReportingBo = Mfrs17ReportingService.Find(id);
            int currentStatus = mfrs17ReportingBo.Status;

            if (mfrs17ReportingBo == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                mfrs17ReportingBo.Status = model.Status;
                if (mfrs17ReportingBo.Status == Mfrs17ReportingBo.StatusPendingGenerate)
                {
                    mfrs17ReportingBo.GenerateType = model.GenerateType;

                    mfrs17ReportingBo.GenerateModifiedOnly = false;
                    if (model.GenerateType == Mfrs17ReportingBo.GenerateTypeMultiple)
                    {
                        mfrs17ReportingBo.GenerateModifiedOnly = model.GenerateModifiedOnly;
                    }
                }
                mfrs17ReportingBo.UpdatedById = AuthUserId;
                mfrs17ReportingBo.IsResume = model.IsResume;

                Result validateResult = Mfrs17ReportingService.ValidateGenerateType(mfrs17ReportingBo);

                if (validateResult.Valid)
                {
                    TrailObject trail = GetNewTrailObject();
                    Result = Mfrs17ReportingService.Update(ref mfrs17ReportingBo, ref trail);
                    if (Result.Valid)
                    {
                        if (mfrs17ReportingBo.Status == Mfrs17ReportingBo.StatusPendingUpdate)
                        {
                            model.ProcessCedantDetails(form, AuthUserId, ref trail);
                        }
                        if (!mfrs17ReportingBo.IsResume.Value && mfrs17ReportingBo.GenerateType == Mfrs17ReportingBo.GenerateTypeMultiple)
                        {
                            // to set all generate status to pending if not resume & generate multiple
                            model.UpdateGenerateStatus(AuthUserId);
                        }
                        model.ProcessStatusHistory(form, AuthUserId, ref trail);
                        CreateTrail(
                            mfrs17ReportingBo.Id,
                            "Update MFRS17 Reporting"
                        );

                        SetUpdateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = mfrs17ReportingBo.Id });
                    }
                    AddResult(Result);
                }
                AddResult(validateResult);
            }

            model.Status = currentStatus;
            PageLoad(model.ModuleId, mfrs17ReportingBo);

            return View(model);
        }

        // GET: Mfrs17Reporting/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            return RedirectToAction("Index");

            /*Mfrs17ReportingBo mfrs17ReportingBo = Mfrs17ReportingService.Find(id);
            if (mfrs17ReportingBo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new Mfrs17ReportingViewModel(mfrs17ReportingBo));*/
        }

        // POST: Mfrs17Reporting/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, Mfrs17ReportingViewModel model)
        {
            return RedirectToAction("Index");

            /*Mfrs17ReportingBo mfrs17ReportingBo = Mfrs17ReportingService.Find(id);
            if (mfrs17ReportingBo == null)
            {
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = Mfrs17ReportingService.Delete(mfrs17ReportingBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    mfrs17ReportingBo.Id,
                    "Delete MFRS17 Reporting"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = mfrs17ReportingBo.Id });*/
        }

        public void IndexPage()
        {
            DropDownCutOffQuarter();
            // Status
            List<SelectListItem> statusItems = new List<SelectListItem> { };
            statusItems.Add(new SelectListItem { Text = "Please select", Value = "" });
            for (int i = 1; i <= Mfrs17ReportingBo.StatusMax; i++)
            {
                statusItems.Add(new SelectListItem { Text = Mfrs17ReportingBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownStatuses = statusItems;
            SetViewBagMessage();
        }

        public void PageLoad(int moduleId = 0, Mfrs17ReportingBo bo = null)
        {
            // AuthUserName
            AuthUserName();
            DropDownEmpty();
            DropDownCedant();
            DropDownPremiumFrequencyCode();
            DropDownCutOffQuarter();
            ViewBag.PremiumFrequencyCodes = GetPickListDetailCode(StandardOutputBo.TypePremiumFrequencyCode, true);

            // Status
            List<SelectListItem> statusItems = new List<SelectListItem> { };
            statusItems.Add(new SelectListItem { Text = "Please select", Value = "" });
            for (int i = 1; i <= Mfrs17ReportingBo.StatusMax; i++)
            {
                statusItems.Add(new SelectListItem { Text = Mfrs17ReportingBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownStatuses = statusItems;

            // Reporting Status
            List<string> allStatusItems = new List<string>();
            for (int i = 0; i <= Mfrs17ReportingBo.StatusMax; i++)
            {
                allStatusItems.Add(Mfrs17ReportingBo.GetStatusName(i));
            }
            ViewBag.StatusHistoryStatusList = allStatusItems;

            // Reporting Detail Status
            allStatusItems = new List<string>();
            for (int i = 0; i <= Mfrs17ReportingDetailBo.StatusMax; i++)
            {
                allStatusItems.Add(Mfrs17ReportingDetailBo.GetStatusName(i));
            }
            ViewBag.DetailStatusList = allStatusItems;

            if (bo == null)
            {

            }
            else
            {
                GetList(moduleId, bo);
            }
            SetViewBagMessage();
        }

        public void GetList(int moduleId, Mfrs17ReportingBo bo)
        {
            DropDownGenerateType();
            RetrieveFile(bo);

            // CedantDetail
            IList<Mfrs17ReportingDetailBo> cedantDetailBos = Mfrs17ReportingDetailService.GetDataByMfrs17ReportingId(bo.Id);
            ViewBag.CedantDetails = cedantDetailBos;

            // Mfrs17TreatyCodeDetail
            IList<Mfrs17ReportingDetailBo> mfrs17TreatyCodeDetailBos = Mfrs17ReportingDetailService.GetDataByMfrs17ReportingId(bo.Id, false);
            ViewBag.Mfrs17TreatyCodeDetails = mfrs17TreatyCodeDetailBos;

            // Remarks
            IList<RemarkBo> remarkBos = RemarkService.GetByModuleIdObjectId(moduleId, bo.Id);
            ViewBag.Remarks = remarkBos;

            // StatusHistories
            IList<StatusHistoryBo> statusHistoryBos = StatusHistoryService.GetStatusHistoriesByModuleIdObjectId(moduleId, bo.Id);
            ViewBag.StatusHistories = statusHistoryBos;


            ViewBag.HasBeenGenerated = false;
            if (statusHistoryBos.Where(q => q.Status == Mfrs17ReportingBo.StatusPendingGenerate).Count() > 0 && bo.Status != Mfrs17ReportingBo.StatusPendingGenerate)
                ViewBag.HasBeenGenerated = true;

            ViewBag.ToResume = false;
            if (bo.Status == Mfrs17ReportingBo.StatusFailedOnGenerate && bo.GenerateType == Mfrs17ReportingBo.GenerateTypeMultiple)
                ViewBag.ToResume = true;
        }

        protected List<SelectListItem> DropDownGenerateType()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= Mfrs17ReportingBo.GenerateTypeMax; i++)
            {
                items.Add(new SelectListItem { Text = Mfrs17ReportingBo.GetGenerateTypeName(i), Value = i.ToString() });
            }
            ViewBag.DropDownGenerateTypes = items;
            return items;
        }

        protected void RetrieveFile(Mfrs17ReportingBo bo)
        {
            string qtr = bo.Quarter.Replace(" ", string.Empty);
            string singlePath = Util.GetMfrs17ReportingPath(qtr + "/Single");
            string multiplePath = Util.GetMfrs17ReportingPath(qtr + "/Multiple");
            List<Mfrs17ReportingFile> mfrs17ReportingFiles = new List<Mfrs17ReportingFile> { };
            bool isMultipleFileExist = false;

            if (Directory.Exists(singlePath))
            {
                foreach (string filePath in Directory.GetFiles(@singlePath, "*.txt"))
                {
                    mfrs17ReportingFiles.Add(new Mfrs17ReportingFile
                    {
                        FileName = Path.GetFileNameWithoutExtension(filePath),
                        SubFolder = "Single",
                        CreatedAtStr = System.IO.File.GetCreationTime(filePath).ToString(Util.GetDateTimeFormat()),
                        ModifiedAtStr = System.IO.File.GetLastWriteTime(filePath).ToString(Util.GetDateTimeFormat()),
                    });
                }
            }
            if (Directory.Exists(multiplePath))
            {
                if (Directory.GetFiles(@multiplePath, "*.txt").Count() > 0)
                {
                    isMultipleFileExist = true;
                }
                foreach (string filePath in Directory.GetFiles(@multiplePath, "*.txt"))
                {
                    mfrs17ReportingFiles.Add(new Mfrs17ReportingFile
                    {
                        FileName = Path.GetFileNameWithoutExtension(filePath),
                        SubFolder = "Multiple",
                        CreatedAtStr = System.IO.File.GetCreationTime(filePath).ToString(Util.GetDateTimeFormat()),
                        ModifiedAtStr = System.IO.File.GetLastWriteTime(filePath).ToString(Util.GetDateTimeFormat()),
                    });
                }
            }

            ViewBag.IsMultipleFileExist = isMultipleFileExist;
            ViewBag.Mfrs17ReportingFiles = mfrs17ReportingFiles;
        }

        public ActionResult FileDownload(string quarter, string type, string fileName)
        {
            try
            {
                string fileNameExt = fileName + ".txt";
                string qtr = quarter.Replace(" ", string.Empty);
                string path = Util.GetMfrs17ReportingPath(qtr + "/" + type);
                string filePath = string.Format("{0}/{1}", path, fileNameExt);

                Response.ClearContent();
                Response.Clear();
                Response.ContentType = MimeMapping.GetMimeMapping(fileNameExt);
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileNameExt);
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
            return null;
        }

        [HttpPost]
        public ActionResult DownloadMultiple(string quarter)
        {
            try
            {
                string qtr = quarter.Replace(" ", string.Empty);
                string multiplePath = Util.GetMfrs17ReportingPath(qtr + "/Multiple");
                string zipFileName = string.Format("{0} Multiple Files.zip", qtr);

                Response.ClearContent();
                Response.Clear();
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipFileName);

                using (ZipFile zip = new ZipFile())
                {
                    zip.AddSelectedFiles("*.txt", multiplePath, "", false);
                    zip.Save(Response.OutputStream);
                }

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

            return null;
        }

        [HttpPost]
        public ActionResult GenerateMfrs17SummaryReport(Mfrs17ReportingViewModel model, bool isDefault = true)
        {
            string quarter = model.Quarter;
            int mfrs17ReportingId = model.Id;
            string qtr = quarter.Replace(" ", string.Empty);

            var process = new GenerateMfrs17SummaryReport()
            {
                Mfrs17ReportingId = mfrs17ReportingId,
                Quarter = qtr,
                IsDefault = isDefault,
            };
            process.Process();

            return File(process.FilePath, "text/csv", Path.GetFileName(process.FilePath));
        }

        [HttpPost]
        public JsonResult RefreshDropDownTreatyCode(int? cedantId)
        {
            if (cedantId.HasValue && cedantId != 0)
            {
                return Json(new { TreatyCodes = DropDownTreatyCode(cedantId: cedantId) });
            }
            return Json(new { TreatyCodes = GetEmptyDropDownList() });
        }

        [HttpPost]
        public JsonResult RefreshDropDownRiskQuarterMonth(string riskQuarter)
        {
            var quarterObject = new QuarterObject(riskQuarter);
            var items = new List<SelectListItem> { };

            for (int i = quarterObject.MonthStart; i <= quarterObject.MonthEnd; i++)
            {
                var selected = i == quarterObject.MonthEnd;
                items.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString(), Selected = selected });
            }
            return Json(new { RiskQuarterMonths = items });
        }
        
        [HttpPost]
        public JsonResult CountCedantData(int cedantId, int treatyCodeId, int paymentMode, string riskQuarter, string cedingPlanCode, int cutOffId, int? riskQuarterMonth = null)
        {
            var quarterObject = new QuarterObject(riskQuarter);
            var quarterEndDate = quarterObject.EndDate;
            var treatyCodeBo = TreatyCodeService.Find(treatyCodeId);
            var treatyCode = treatyCodeBo.Code;
            var premiumFrequencyCodeBo = PickListDetailService.Find(paymentMode);

            RiskQuarterDate riskQuarterDate = RiskQuarterDate.GetRiskQuarterDate(quarterEndDate.Value, RiskQuarterDate.TypeQuarterly);
            if (premiumFrequencyCodeBo.Code == "M")
            {
                if (riskQuarterMonth.HasValue && riskQuarterMonth >= quarterObject.MonthStart && riskQuarterMonth <= quarterObject.MonthEnd)
                {
                    var maxDay = DateTime.DaysInMonth(quarterEndDate.Value.Year, riskQuarterMonth.Value);
                    quarterEndDate = new DateTime(quarterEndDate.Value.Year, riskQuarterMonth.Value, maxDay);
                }
                riskQuarterDate = RiskQuarterDate.GetRiskQuarterDate(quarterEndDate.Value, RiskQuarterDate.TypeMonthly);
            }

            //Mfrs17ReportingDetailBo mfrs17ReportingDetailBo = null;
            List<Mfrs17ReportingDetailBo> mfrs17ReportingDetailBos = new List<Mfrs17ReportingDetailBo>();
            var counts = RiDataWarehouseHistoryService.CountForMfrs17Reporting(
                cutOffId,
                treatyCodeBo.Code,
                premiumFrequencyCodeBo.Code,
                cedingPlanCode,
                riskQuarterDate.EndDate.Year,
                riskQuarterDate.StartDate.Month,
                riskQuarterDate.EndDate.Month
            );
            if (counts != null)
            {
                foreach (var count in counts)
                {
                    mfrs17ReportingDetailBos.Add(new Mfrs17ReportingDetailBo
                    {
                        CedantId = cedantId,
                        CedantBo = CedantService.Find(cedantId),
                        TreatyCode = treatyCode,
                        PremiumFrequencyCodePickListDetailId = paymentMode,
                        PremiumFrequencyCodePickListDetailBo = premiumFrequencyCodeBo,
                        CedingPlanCode = count.Key,
                        RiskQuarter = riskQuarter,
                        LatestDataStartDateStr = riskQuarterDate.DataStartDate.ToString(Util.GetDateFormat()),
                        LatestDataEndDateStr = riskQuarterDate.DataEndDate.ToString(Util.GetDateFormat()),
                        Record = count.Value,
                        Status = Mfrs17ReportingDetailBo.StatusPending,
                    });
                }
            }
            //if (count != 0)
            //{
            //    mfrs17ReportingDetailBo = new Mfrs17ReportingDetailBo
            //    {
            //        CedantId = cedantId,
            //        CedantBo = CedantService.Find(cedantId),
            //        TreatyCode = treatyCode,
            //        PremiumFrequencyCodePickListDetailId = paymentMode,
            //        PremiumFrequencyCodePickListDetailBo = premiumFrequencyCodeBo,
            //        CedingPlanCode = "",
            //        RiskQuarter = riskQuarter,
            //        LatestDataStartDateStr = riskQuarterDate.DataStartDate.ToString(Util.GetDateFormat()),
            //        LatestDataEndDateStr = riskQuarterDate.DataEndDate.ToString(Util.GetDateFormat()),
            //        Record = count,
            //        Status = Mfrs17ReportingDetailBo.StatusPending,
            //    };
            //}

            return Json(new { mfrs17ReportingDetailBos });
        }

        [HttpPost]
        public JsonResult RefreshTokenfieldCedingPlanCode(int cedantId, int treatyCodeId, int cutOffId)
        {
            var cedingPlanCodes = RiDataWarehouseHistoryService.GetDistinctCedingPlanCodes(cutOffId, cedantId, treatyCodeId);
            return Json(new { CedingPlanCodes = cedingPlanCodes });
        }
    }
}
