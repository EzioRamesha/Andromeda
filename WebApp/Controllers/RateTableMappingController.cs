using BusinessObject;
using BusinessObject.Identity;
using ConsoleApp.Commands.ProcessDatas;
using ConsoleApp.Commands.ProcessDatas.Exports;
using PagedList;
using Services;
using Shared;
using Shared.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class RateTableMappingController : BaseController
    {
        public const string Controller = "RateTableMapping";

        public const string ClearSessionValue = "NULL";
        public const string SessionCedantIdName = "RateTableMapping.CedantId";

        // GET: RateTable
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string CedantId,
            string TreatyCode,
            string CedingTreatyCode,
            string CedingPlanCode,
            string CedingPlanCode2,
            string CedingBenefitTypeCode,
            string CedingBenefitRiskCode,
            string PolicyTermFrom,
            string PolicyTermTo,
            string PolicyDurationFrom,
            string PolicyDurationTo,
            string GroupPolicyNumber,
            int? BenefitId,
            string ReinsEffDatePolStartDate,
            string ReinsEffDatePolEndDate,
            string ReportingStartDate,
            string ReportingEndDate,
            int? AttainedAgeFrom,
            int? AttainedAgeTo,
            int? PremiumFrequencyCodePickListDetailId,
            string PolicyAmountFrom,
            string PolicyAmountTo,
            string AarFrom,
            string AarTo,
            int? ReinsBasisCodePickListDetailId,
            int? RateId,
            string RiDiscountCode,
            string LargeDiscountCode,
            string GroupDiscountCode,
            string SortOrder,
            string UploadCreatedAt,
            string UploadFileName,
            int? UploadSubmittedBy,
            int? UploadStatus,
            string UploadSortOrder,
            int? UploadPage,
            int? Page,
            int? ActiveTab)
        {
            if (!string.IsNullOrEmpty(CedantId))
            {
                if (CedantId == ClearSessionValue)
                {
                    CedantId = null;
                    Session.Remove(SessionCedantIdName);
                }
                else
                {
                    Session[SessionCedantIdName] = CedantId;
                }
            }
            var sessionCedantId = (string)Session[SessionCedantIdName];
            if (!string.IsNullOrEmpty(sessionCedantId))
            {
                CedantId = sessionCedantId;
            }

            int? cid = null;
            if (int.TryParse(CedantId, out int outId))
                cid = outId;

            DateTime? start = Util.GetParseDateTime(ReinsEffDatePolStartDate);
            DateTime? end = Util.GetParseDateTime(ReinsEffDatePolEndDate);

            DateTime? reportingStartDate = Util.GetParseDateTime(ReportingStartDate);
            DateTime? reportingEndDate = Util.GetParseDateTime(ReportingEndDate);

            double? policyTermFrom = Util.StringToDouble(PolicyTermFrom);
            double? policyTermTo = Util.StringToDouble(PolicyTermTo);

            double? policyAmountFrom = Util.StringToDouble(PolicyAmountFrom);
            double? policyAmountTo = Util.StringToDouble(PolicyAmountTo);

            double? aarFrom = Util.StringToDouble(AarFrom);
            double? aarTo = Util.StringToDouble(AarTo);

            double? policyDurationFrom = Util.StringToDouble(PolicyDurationFrom);
            double? policyDurationTo = Util.StringToDouble(PolicyDurationTo);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["CedantId"] = cid.HasValue ? CedantId : null,
                ["TreatyCode"] = TreatyCode,
                ["CedingTreatyCode"] = CedingTreatyCode,
                ["CedingPlanCode"] = CedingPlanCode,
                ["CedingPlanCode2"] = CedingPlanCode2,
                ["CedingBenefitTypeCode"] = CedingBenefitTypeCode,
                ["CedingBenefitRiskCode"] = CedingBenefitRiskCode,
                ["PolicyTermFrom"] = policyTermFrom.HasValue ? PolicyTermFrom : null,
                ["PolicyTermTo"] = policyTermTo.HasValue ? PolicyTermTo : null,
                ["PolicyDurationFrom"] = policyDurationFrom.HasValue ? PolicyDurationFrom : null,
                ["PolicyDurationTo"] = policyDurationTo.HasValue ? PolicyDurationTo : null,
                ["GroupPolicyNumber"] = GroupPolicyNumber,
                ["BenefitId"] = BenefitId,
                ["ReinsEffDatePolStartDate"] = start.HasValue ? ReinsEffDatePolStartDate : null,
                ["ReinsEffDatePolEndDate"] = end.HasValue ? ReinsEffDatePolEndDate : null,
                ["ReportingStartDate"] = reportingStartDate.HasValue ? ReportingStartDate : null,
                ["ReportingEndDate"] = reportingEndDate.HasValue ? ReportingEndDate : null,
                ["AttainedAgeFrom"] = AttainedAgeFrom,
                ["AttainedAgeTo"] = AttainedAgeTo,
                ["PremiumFrequencyCodePickListDetailId"] = PremiumFrequencyCodePickListDetailId,
                ["PolicyAmountFrom"] = policyAmountFrom.HasValue ? PolicyAmountFrom : null,
                ["PolicyAmountTo"] = policyAmountTo.HasValue ? PolicyAmountTo : null,
                ["AarFrom"] = aarFrom.HasValue ? AarFrom : null,
                ["AarTo"] = aarTo.HasValue ? AarTo : null,
                ["ReinsBasisCodePickListDetailId"] = ReinsBasisCodePickListDetailId,
                ["RateId"] = RateId,
                ["RiDiscountCode"] = RiDiscountCode,
                ["LargeDiscountCode"] = LargeDiscountCode,
                ["GroupDiscountCode"] = GroupDiscountCode,
                ["SortOrder"] = SortOrder,

                ["UploadCreatedAt"] = UploadCreatedAt,
                ["UploadFileName"] = UploadFileName,
                ["UploadCreatedById"] = UploadSubmittedBy,
                ["UploadStatus"] = UploadStatus,
                ["UploadSortOrder"] = UploadSortOrder,
                ["ActiveTab"] = ActiveTab,
            };
            ViewBag.SortOrder = ActiveTab == 2 ? UploadSortOrder : SortOrder;
            ViewBag.SortRateId = GetSortParam("RateId");
            ViewBag.SortRiDiscountCode = GetSortParam("RiDiscountCode");
            ViewBag.SortLargeDiscountCode = GetSortParam("LargeDiscountCode");
            ViewBag.SortGroupDiscountCode = GetSortParam("GroupDiscountCode");

            ViewBag.UploadSortOrder = UploadSortOrder;
            ViewBag.SortUploadCreatedAt = GetSortParam("UploadCreatedAt");
            ViewBag.SortUploadFileName = GetSortParam("UploadFileName");
            ViewBag.SortUploadCreatedById = GetSortParam("UploadCreatedById");
            ViewBag.SortUploadStatus = GetSortParam("UploadStatus");

            var query = _db.RateTables.Select(RateTableViewModel.Expression());

            if (cid.HasValue) query = query.Where(q => q.CedantId == cid);
            if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => q.RateTableDetails.Any(d => d.TreatyCode == TreatyCode));
            if (!string.IsNullOrEmpty(CedingTreatyCode)) query = query.Where(q => q.RateTableDetails.Any(d => d.CedingTreatyCode == CedingTreatyCode));
            if (!string.IsNullOrEmpty(CedingPlanCode)) query = query.Where(q => q.RateTableDetails.Any(d => d.CedingPlanCode == CedingPlanCode));
            if (!string.IsNullOrEmpty(CedingPlanCode2)) query = query.Where(q => q.RateTableDetails.Any(d => d.CedingPlanCode2 == CedingPlanCode2));
            if (!string.IsNullOrEmpty(CedingBenefitTypeCode)) query = query.Where(q => q.RateTableDetails.Any(d => d.CedingBenefitTypeCode == CedingBenefitTypeCode));
            if (!string.IsNullOrEmpty(CedingBenefitRiskCode)) query = query.Where(q => q.RateTableDetails.Any(d => d.CedingBenefitRiskCode == CedingBenefitRiskCode));
            if (!string.IsNullOrEmpty(GroupPolicyNumber)) query = query.Where(q => q.RateTableDetails.Any(d => d.GroupPolicyNumber == GroupPolicyNumber));

            if (BenefitId.HasValue) query = query.Where(q => q.BenefitId == BenefitId);

            if (start.HasValue) query = query.Where(q => q.ReinsEffDatePolStartDate >= start);
            if (end.HasValue) query = query.Where(q => q.ReinsEffDatePolEndDate <= end);

            if (reportingStartDate.HasValue) query = query.Where(q => q.ReportingStartDate >= reportingStartDate);
            if (reportingEndDate.HasValue) query = query.Where(q => q.ReportingEndDate <= reportingEndDate);

            if (AttainedAgeFrom.HasValue) query = query.Where(q => q.AttainedAgeFrom >= AttainedAgeFrom);
            if (AttainedAgeTo.HasValue) query = query.Where(q => q.AttainedAgeTo <= AttainedAgeTo);

            if (policyTermFrom.HasValue) query = query.Where(q => q.PolicyTermFrom >= policyTermFrom);
            if (policyTermTo.HasValue) query = query.Where(q => q.PolicyTermTo <= policyTermTo);

            if (policyDurationFrom.HasValue) query = query.Where(q => q.PolicyDurationFrom >= policyDurationFrom);
            if (policyDurationTo.HasValue) query = query.Where(q => q.PolicyDurationTo <= policyDurationTo);

            if (PremiumFrequencyCodePickListDetailId != null) query = query.Where(q => q.PremiumFrequencyCodePickListDetailId == PremiumFrequencyCodePickListDetailId);

            if (policyAmountFrom.HasValue) query = query.Where(q => q.PolicyAmountFrom >= policyAmountFrom);
            if (policyAmountTo.HasValue) query = query.Where(q => q.PolicyAmountTo <= policyAmountTo);
            if (aarFrom.HasValue) query = query.Where(q => q.AarFrom >= aarFrom);
            if (aarTo.HasValue) query = query.Where(q => q.AarTo <= aarTo);

            if (ReinsBasisCodePickListDetailId.HasValue) query = query.Where(q => q.ReinsBasisCodePickListDetailId == ReinsBasisCodePickListDetailId);

            if (RateId.HasValue) query = query.Where(q => q.RateId == RateId);
            if (!string.IsNullOrEmpty(RiDiscountCode)) query = query.Where(q => q.RiDiscountCode.Contains(RiDiscountCode));
            if (!string.IsNullOrEmpty(LargeDiscountCode)) query = query.Where(q => q.LargeDiscountCode.Contains(LargeDiscountCode));
            if (!string.IsNullOrEmpty(GroupDiscountCode)) query = query.Where(q => q.GroupDiscountCode.Contains(GroupDiscountCode));

            if (SortOrder == Html.GetSortAsc("RateId")) query = query.OrderBy(q => q.Rate.Code);
            else if (SortOrder == Html.GetSortDsc("RateId")) query = query.OrderByDescending(q => q.Rate.Code);
            else if (SortOrder == Html.GetSortAsc("RiDiscountCode")) query = query.OrderBy(q => q.RiDiscountCode);
            else if (SortOrder == Html.GetSortDsc("RiDiscountCode")) query = query.OrderByDescending(q => q.RiDiscountCode);
            else if (SortOrder == Html.GetSortAsc("LargeDiscountCode")) query = query.OrderBy(q => q.LargeDiscountCode);
            else if (SortOrder == Html.GetSortDsc("LargeDiscountCode")) query = query.OrderByDescending(q => q.LargeDiscountCode);
            else if (SortOrder == Html.GetSortAsc("GroupDiscountCode")) query = query.OrderBy(q => q.GroupDiscountCode);
            else if (SortOrder == Html.GetSortDsc("GroupDiscountCode")) query = query.OrderByDescending(q => q.GroupDiscountCode);
            query = query.OrderByDescending(q => q.Id);

            ViewBag.ListingTotal = query.Count();
            ViewBag.ListingList = query.ToPagedList(Page ?? 1, PageSize);
            IndexPage(cid);

            DropDownUploadStatus();
            DropDownUser(UserBo.StatusActive);

            ViewBag.ActiveTab = ActiveTab;

            //Upload tab
            GetUploadData(UploadPage, UploadCreatedAt, UploadFileName, UploadSubmittedBy, UploadStatus, UploadSortOrder);

            return View();
        }

        private void GetUploadData(int? UploadPage,
            string UploadCreatedAt,
            string UploadFileName,
            int? UploadSubmittedBy,
            int? UploadStatus,
            string UploadSortOrder)
        {
            DateTime? createdAtFrom = Util.GetParseDateTime(UploadCreatedAt);
            DateTime? createdAtTo = null;

            if (createdAtFrom.HasValue)
                createdAtTo = createdAtFrom.Value.AddDays(1);

            var query = _db.RateTableMappingUpload.Select(RateTableMappingUploadViewModel.Expression());
            if (createdAtFrom.HasValue) query = query.Where(q => q.CreatedAt >= createdAtFrom && q.CreatedAt < createdAtTo);
            if (!string.IsNullOrEmpty(UploadFileName)) query = query.Where(q => q.FileName == UploadFileName);
            if (UploadSubmittedBy.HasValue) query = query.Where(q => q.CreatedById == UploadSubmittedBy);
            if (UploadStatus.HasValue) query = query.Where(q => q.Status == UploadStatus);

            if (UploadSortOrder == Html.GetSortAsc("CreatedAt")) query = query.OrderBy(q => q.CreatedAt);
            else if (UploadSortOrder == Html.GetSortDsc("CreatedAt")) query = query.OrderByDescending(q => q.CreatedAt);
            else if (UploadSortOrder == Html.GetSortAsc("FileName")) query = query.OrderBy(q => q.FileName);
            else if (UploadSortOrder == Html.GetSortDsc("FileName")) query = query.OrderByDescending(q => q.FileName);
            else if (UploadSortOrder == Html.GetSortAsc("CreatedById")) query = query.OrderBy(q => q.CreatedById);
            else if (UploadSortOrder == Html.GetSortDsc("CreatedById")) query = query.OrderByDescending(q => q.CreatedById);
            else if (UploadSortOrder == Html.GetSortAsc("Status")) query = query.OrderBy(q => q.Status);
            else if (UploadSortOrder == Html.GetSortDsc("Status")) query = query.OrderByDescending(q => q.Status);
            else query = query.OrderByDescending(q => q.Id);

            ViewBag.UploadTotal = query.Count();
            ViewBag.UploadList = query.ToPagedList(UploadPage ?? 1, PageSize);
        }

        public List<SelectListItem> DropDownUploadStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, RateTableMappingUploadBo.StatusMax))
            {
                items.Add(new SelectListItem { Text = RateTableMappingUploadBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.UploadStatusItems = items;
            return items;
        }

        // GET: RateTable/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            var model = new RateTableViewModel();
            var sessionCedantId = (string)Session[SessionCedantIdName];
            if (!string.IsNullOrEmpty(sessionCedantId))
            {
                if (int.TryParse(sessionCedantId, out int cid))
                    model.CedantId = cid;
            }
            LoadPage();
            return View(model);
        }

        // POST: RateTable/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(RateTableViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);
                var trail = GetNewTrailObject();
                Result = RateTableService.Result();
                var mappingResult = RateTableService.ValidateMapping(bo);
                if (!mappingResult.Valid)
                {
                    Result.AddErrorRange(mappingResult.ToErrorArray());
                }
                else
                {
                    Result = RateTableService.Create(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        model.Id = bo.Id;
                        RateTableService.ProcessMappingDetail(bo, AuthUserId); // DO NOT TRAIL
                        CreateTrail(
                            bo.Id,
                            "Create Rate Table Mapping"
                        );

                        SetCreateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = bo.Id });
                    }
                }
                AddResult(Result);
            }
            LoadPage();
            return View(model);
        }

        // GET: RateTable/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = RateTableService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }
            LoadPage(bo);
            return View(new RateTableViewModel(bo));
        }

        // POST: RateTable/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, RateTableViewModel model)
        {
            var dbBo = RateTableService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();
                Result = RateTableService.Result();
                var mappingResult = RateTableService.ValidateMapping(bo);
                if (!mappingResult.Valid)
                {
                    Result.AddErrorRange(mappingResult.ToErrorArray());
                }
                else
                {
                    Result = RateTableService.Update(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        RateTableService.ProcessMappingDetail(bo, AuthUserId); // DO NOT TRAIL
                        CreateTrail(
                            bo.Id,
                            "Update Rate Table Mapping"
                        );

                        SetUpdateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = bo.Id });
                    }
                }
                AddResult(Result);
            }
            LoadPage(dbBo);
            return View(model);
        }

        // GET: RateTable/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = RateTableService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new RateTableViewModel(bo));
        }

        // POST: RateTable/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, RateTableViewModel model)
        {
            var bo = RateTableService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = RateTableService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Rate Table Mapping"
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = AccessMatrixBo.PowerUpload, ReturnController = Controller)]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    string fileExtension = Path.GetExtension(upload.FileName);
                    if (fileExtension != ".csv")
                    {
                        SetErrorSessionMsg("Allowed file of type: .csv");
                        return RedirectToAction("Index");
                    }

                    var trail = GetNewTrailObject();
                    RateTableMappingUploadBo rateTableMappingUploadBo = new RateTableMappingUploadBo
                    {
                        Status = RateTableMappingUploadBo.StatusPendingProcess,
                        FileName = upload.FileName,
                        CreatedById = AuthUserId,
                        UpdatedById = AuthUserId,
                    };

                    rateTableMappingUploadBo.FormatHashFileName();

                    string path = rateTableMappingUploadBo.GetLocalPath();
                    Util.MakeDir(path);
                    upload.SaveAs(path);

                    RateTableMappingUploadService.Create(ref rateTableMappingUploadBo, ref trail);

                    SetSuccessSessionMsg("File uploaded and pending processing.");
                    //var process = new ProcessRateTableMapping()
                    //{
                    //    PostedFile = upload,
                    //    AuthUserId = AuthUserId,
                    //};
                    //process.Process();

                    //if (process.Errors.Count() > 0 && process.Errors != null)
                    //{
                    //    SetErrorSessionMsgArr(process.Errors.Take(50).ToList());
                    //}

                    //int create = process.GetProcessCount("Create");
                    //int update = process.GetProcessCount("Update");
                    //int delete = process.GetProcessCount("Delete");

                    //SetSuccessSessionMsg(string.Format("Process File Successful, Total Create: {0}, Total Update: {1}, Total Delete: {2}", create, update, delete));
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Download(
            string downloadToken,
            int type,
            int? CedantId,
            string TreatyCode,
            string CedingTreatyCode,
            string CedingPlanCode,
            string CedingPlanCode2,
            string CedingBenefitTypeCode,
            string CedingBenefitRiskCode,
            double? PolicyTermFrom,
            double? PolicyTermTo,
            string PolicyDurationFrom,
            string PolicyDurationTo,
            string GroupPolicyNumber,
            int? BenefitId,
            string ReinsEffDatePolStartDate,
            string ReinsEffDatePolEndDate,
            string ReportingStartDate,
            string ReportingEndDate,
            int? AttainedAgeFrom,
            int? AttainedAgeTo,
            int? PremiumFrequencyCodePickListDetailId,
            string PolicyAmountFrom,
            string PolicyAmountTo,
            string AarFrom,
            string AarTo,
            int? ReinsBasisCodePickListDetailId,
            int? RateId,
            string RiDiscountCode,
            string LargeDiscountCode,
            string GroupDiscountCode
        )
        {
            // type 1 = all
            // type 2 = filtered download
            // type 3 = template download

            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            _db.Database.CommandTimeout = 0;
            var query = _db.RateTables.Select(RateTableViewModel.Expression());

            if (type == 2) // filtered dowload
            {
                DateTime? start = Util.GetParseDateTime(ReinsEffDatePolStartDate);
                DateTime? end = Util.GetParseDateTime(ReinsEffDatePolEndDate);

                DateTime? reportingStartDate = Util.GetParseDateTime(ReportingStartDate);
                DateTime? reportingEndDate = Util.GetParseDateTime(ReportingEndDate);

                double? policyTermFrom = Util.StringToDouble(PolicyTermFrom);
                double? policyTermTo = Util.StringToDouble(PolicyTermTo);

                double? policyAmountFrom = Util.StringToDouble(PolicyAmountFrom);
                double? policyAmountTo = Util.StringToDouble(PolicyAmountTo);

                double? aarFrom = Util.StringToDouble(AarFrom);
                double? aarTo = Util.StringToDouble(AarTo);

                double? policyDurationFrom = Util.StringToDouble(PolicyDurationFrom);
                double? policyDurationTo = Util.StringToDouble(PolicyDurationTo);

                if (CedantId.HasValue) query = query.Where(q => q.CedantId == CedantId);
                if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => q.RateTableDetails.Any(d => d.TreatyCode == TreatyCode));
                if (!string.IsNullOrEmpty(CedingTreatyCode)) query = query.Where(q => q.RateTableDetails.Any(d => d.CedingTreatyCode == CedingTreatyCode));
                if (!string.IsNullOrEmpty(CedingPlanCode)) query = query.Where(q => q.RateTableDetails.Any(d => d.CedingPlanCode == CedingPlanCode));
                if (!string.IsNullOrEmpty(CedingPlanCode2)) query = query.Where(q => q.RateTableDetails.Any(d => d.CedingPlanCode2 == CedingPlanCode2));
                if (!string.IsNullOrEmpty(CedingBenefitTypeCode)) query = query.Where(q => q.RateTableDetails.Any(d => d.CedingBenefitTypeCode == CedingBenefitTypeCode));
                if (!string.IsNullOrEmpty(CedingBenefitRiskCode)) query = query.Where(q => q.RateTableDetails.Any(d => d.CedingBenefitRiskCode == CedingBenefitRiskCode));
                if (!string.IsNullOrEmpty(GroupPolicyNumber)) query = query.Where(q => q.RateTableDetails.Any(d => d.GroupPolicyNumber == GroupPolicyNumber));

                if (BenefitId.HasValue) query = query.Where(q => q.BenefitId == BenefitId);

                if (start.HasValue) query = query.Where(q => q.ReinsEffDatePolStartDate >= start);
                if (end.HasValue) query = query.Where(q => q.ReinsEffDatePolEndDate <= end);

                if (reportingStartDate.HasValue) query = query.Where(q => q.ReportingStartDate >= reportingStartDate);
                if (reportingEndDate.HasValue) query = query.Where(q => q.ReportingEndDate <= reportingEndDate);

                if (AttainedAgeFrom.HasValue) query = query.Where(q => q.AttainedAgeFrom >= AttainedAgeFrom);
                if (AttainedAgeTo.HasValue) query = query.Where(q => q.AttainedAgeTo <= AttainedAgeTo);

                if (PolicyTermFrom.HasValue) query = query.Where(q => q.PolicyTermFrom >= PolicyTermFrom);
                if (PolicyTermTo.HasValue) query = query.Where(q => q.PolicyTermTo <= PolicyTermTo);

                if (policyDurationFrom.HasValue) query = query.Where(q => q.PolicyDurationFrom >= policyDurationFrom);
                if (policyDurationTo.HasValue) query = query.Where(q => q.PolicyDurationTo <= policyDurationTo);

                if (PremiumFrequencyCodePickListDetailId != null) query = query.Where(q => q.PremiumFrequencyCodePickListDetailId == PremiumFrequencyCodePickListDetailId);

                if (policyAmountFrom.HasValue) query = query.Where(q => q.PolicyAmountFrom >= policyAmountFrom);
                if (policyAmountTo.HasValue) query = query.Where(q => q.PolicyAmountTo <= policyAmountTo);
                if (aarFrom.HasValue) query = query.Where(q => q.AarFrom >= aarFrom);
                if (aarTo.HasValue) query = query.Where(q => q.AarTo <= aarTo);

                if (ReinsBasisCodePickListDetailId.HasValue) query = query.Where(q => q.ReinsBasisCodePickListDetailId == ReinsBasisCodePickListDetailId);

                if (RateId.HasValue) query = query.Where(q => q.RateId == RateId);
                if (!string.IsNullOrEmpty(RiDiscountCode)) query = query.Where(q => q.RiDiscountCode.Contains(RiDiscountCode));
                if (!string.IsNullOrEmpty(LargeDiscountCode)) query = query.Where(q => q.LargeDiscountCode.Contains(LargeDiscountCode));
                if (!string.IsNullOrEmpty(GroupDiscountCode)) query = query.Where(q => q.GroupDiscountCode.Contains(GroupDiscountCode));
            }
            if (type == 3)
            {
                query = null;
            }

            var export = new ExportRateTable();
            export.HandleTempDirectory();

            if (query != null)
                export.SetQuery(query.Select(x => new RateTableBo
                {
                    Id = x.Id,
                    //CedantId = x.CedantId,
                    CedantCode = x.Cedant.Code,
                    TreatyCode = x.TreatyCode,
                    CedingTreatyCode = x.CedingTreatyCode,

                    //BenefitId = x.BenefitId,
                    BenefitCode = x.Benefit.Code,
                    CedingPlanCode = x.CedingPlanCode,
                    CedingPlanCode2 = x.CedingPlanCode2,
                    CedingBenefitTypeCode = x.CedingBenefitTypeCode,
                    CedingBenefitRiskCode = x.CedingBenefitRiskCode,
                    GroupPolicyNumber = x.GroupPolicyNumber,

                    //PremiumFrequencyCodePickListDetailId = x.PremiumFrequencyCodePickListDetailId,
                    PremiumFrequencyCode = x.PremiumFrequencyCodePickListDetail.Code,

                    //ReinsBasisCodePickListDetailId = x.ReinsBasisCodePickListDetailId,
                    ReinsBasisCode = x.ReinsBasisCodePickListDetail.Code,

                    PolicyAmountFrom = x.PolicyAmountFrom,
                    PolicyAmountTo = x.PolicyAmountTo,
                    AttainedAgeFrom = x.AttainedAgeFrom,
                    AttainedAgeTo = x.AttainedAgeTo,
                    AarFrom = x.AarFrom,
                    AarTo = x.AarTo,
                    ReinsEffDatePolStartDate = x.ReinsEffDatePolStartDate,
                    ReinsEffDatePolEndDate = x.ReinsEffDatePolEndDate,

                    // Phase 2
                    PolicyTermFrom = x.PolicyTermFrom,
                    PolicyTermTo = x.PolicyTermTo,
                    PolicyDurationFrom = x.PolicyDurationFrom,
                    PolicyDurationTo = x.PolicyDurationTo,
                    //RateId = x.RateId,
                    RateCode = x.Rate.Code,
                    RiDiscountCode = x.RiDiscountCode,
                    LargeDiscountCode = x.LargeDiscountCode,
                    GroupDiscountCode = x.GroupDiscountCode,

                    ReportingStartDate = x.ReportingStartDate,
                    ReportingEndDate = x.ReportingEndDate,
                }).AsQueryable());

            export.Process();

            return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));
        }

        public string ListToCSV<T>(IEnumerable<T> list)
        {
            var getHeaderMapping = new ProcessRateTableMapping();
            StringBuilder sList = new StringBuilder();

            Type type = typeof(T);
            var props = type.GetProperties();

            sList.Append(string.Join(",", getHeaderMapping.GetColumns().Select(p => p.Header)));
            sList.Append(Environment.NewLine);

            if (list != null)
            {
                foreach (var element in list)
                {
                    List<string> values = new List<string> { };
                    foreach (var p in props)
                    {
                        object v = element.GetType().GetProperty(p.Name).GetValue(element, null);
                        string output = "";
                        switch (p.Name)
                        {
                            case "PolicyAmountFrom":
                            case "PolicyAmountTo":
                            case "AarFrom":
                            case "AarTo":
                            case "PolicyDurationFrom":
                            case "PolicyDurationTo":
                                output = Util.DoubleToString(v);
                                break;
                            default:
                                output = v.ToString();
                                break;
                        }
                        values.Add(string.Format("\"{0}\"", output));
                    }

                    sList.Append(string.Join(",", values.ToArray()));
                    sList.Append(Environment.NewLine);
                }
            }

            return sList.ToString();
        }

        public void IndexPage(int? cedantId = null)
        {
            DropDownEmpty();
            DropDownCedant(selectedId: cedantId, emptyValue: ClearSessionValue);
            DropDownInsuredGenderCode();
            DropDownInsuredTobaccoUse();
            DropDownPremiumFrequencyCode();
            DropDownInsuredOccupationCode();
            DropDownReinsBasisCode();
            DropDownBenefit();
            DropDownRate();
            SetViewBagMessage();
        }

        public void LoadPage(RateTableBo rateTableBo = null)
        {
            DropDownEmpty();
            DropDownInsuredGenderCode();
            DropDownInsuredTobaccoUse();
            DropDownPremiumFrequencyCode();
            DropDownInsuredOccupationCode();
            DropDownReinsBasisCode();
            GetCedingBenefitTypeCode();
            DropDownRate();

            if (rateTableBo == null)
            {
                // Create
                var sessionCedantId = (string)Session[SessionCedantIdName];
                if (int.TryParse(sessionCedantId, out int cid))
                {
                    DropDownCedant(CedantBo.StatusActive, cid);
                    //GetTreatyCodes(cid, true);

                }
                else
                {
                    DropDownCedant(CedantBo.StatusActive);
                    //GetTreatyCodes(isDependent: true);
                }
                DropDownBenefit(BenefitBo.StatusActive);
            }
            else
            {
                // Edit
                DropDownCedant(CedantBo.StatusActive, rateTableBo.CedantId);
                if (rateTableBo.CedantBo != null && rateTableBo.CedantBo.Status == CedantBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.CedantStatusInactive);
                }


                DropDownBenefit(BenefitBo.StatusActive, rateTableBo.BenefitId);
                if (rateTableBo.BenefitBo != null && rateTableBo.BenefitBo.Status == BenefitBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.BenefitStatusInactive);
                }

                //GetTreatyCodes(rateTableBo.CedantId, true);
                string[] treatyCodes = rateTableBo.TreatyCode.ToArraySplitTrim();
                foreach (string treatyCodeStr in treatyCodes)
                {
                    var treatyCode = TreatyCodeService.FindByCode(treatyCodeStr, false);
                    if (treatyCode != null)
                    {
                        if (treatyCode.Status == TreatyCodeBo.StatusInactive)
                        {
                            AddErrorMsg(string.Format(MessageBag.TreatyCodeStatusInactiveWithCode, treatyCodeStr));
                        }
                    }
                    else
                    {
                        AddErrorMsg(string.Format(MessageBag.TreatyCodeNotFound, treatyCodeStr));
                    }
                }
            }
            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownRate()
        {
            var items = GetEmptyDropDownList();
            foreach (RateBo rateBo in RateService.Get())
            {
                items.Add(new SelectListItem { Text = rateBo.Code, Value = rateBo.Id.ToString() });
            }
            ViewBag.DropDownRates = items;
            return items;
        }

        public void DownloadError(int id)
        {
            try
            {
                var rtmuBo = RateTableMappingUploadService.Find(id);
                MemoryStream ms = new MemoryStream();
                TextWriter tw = new StreamWriter(ms);
                tw.WriteLine(rtmuBo.Errors.Replace(",", Environment.NewLine));
                tw.Flush();
                byte[] bytes = ms.ToArray();
                ms.Close();

                Response.ClearContent();
                Response.Clear();
                //Response.Buffer = true;                                                                                                                   
                //Response.BufferOutput = false;
                Response.ContentType = MimeMapping.GetMimeMapping("RateTableMappingUploadError");
                Response.AddHeader("Content-Disposition", "attachment; filename=RateTableMappingUploadError.txt");
                Response.BinaryWrite(bytes);
                //Response.TransmitFile(filePath);
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
    }
}
