using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.Sanctions;
using ConsoleApp.Commands.ProcessDatas.Exports;
using PagedList;
using Services;
using Services.Sanctions;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public class SanctionVerificationController : BaseController
    {
        public const string Controller = "SanctionVerification";

        // GET: SanctionVerification
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string CreatedAt,
            int? SourceId,
            bool? IsRiData,
            bool? IsClaimRegister,
            bool? IsReferralClaim,
            int? Type,
            int? BatchId,
            string CreatedBy,
            string ProcessStartAt,
            string ProcessEndAt,
            bool? UnprocessedRecords,
            int? Status,
            string SortOrder,
            int? Page
        )
        {
            DateTime? createdAt = Util.GetParseDateTime(CreatedAt);
            DateTime? processStartAt = Util.GetParseDateTime(ProcessStartAt);
            DateTime? processEndAt = Util.GetParseDateTime(ProcessEndAt);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["CreatedAt"] = createdAt.HasValue ? CreatedAt : null,
                ["SourceId"] = SourceId,
                ["IsRiData"] = IsRiData,
                ["IsClaimRegister"] = IsClaimRegister,
                ["IsReferralClaim"] = IsReferralClaim,
                ["Type"] = Type,
                ["BatchId"] = BatchId,
                ["CreatedBy"] = CreatedBy,
                ["ProcessStartAt"] = processStartAt.HasValue ? ProcessStartAt : null,
                ["ProcessEndAt"] = processEndAt.HasValue ? ProcessEndAt : null,
                ["UnprocessedRecords"] = UnprocessedRecords,
                ["Status"] = Status,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCreatedAt = GetSortParam("CreatedAt");
            ViewBag.SortSourceId = GetSortParam("SourceId");
            ViewBag.SortIsRiData = GetSortParam("IsRiData");
            ViewBag.SortIsClaimRegister = GetSortParam("IsClaimRegister");
            ViewBag.SortIsReferralClaim = GetSortParam("IsReferralClaim");
            ViewBag.SortType = GetSortParam("Type");
            ViewBag.SortBatchId = GetSortParam("BatchId");
            ViewBag.SortCreatedBy = GetSortParam("CreatedBy");
            ViewBag.SortProcessStartAt = GetSortParam("ProcessStartAt");
            ViewBag.SortProcessEndAt = GetSortParam("ProcessEndAt");
            ViewBag.SortStatus = GetSortParam("Status");

            var query = _db.SanctionVerifications.Select(SanctionVerificationViewModel.Expression());

            if (createdAt.HasValue) query = query.Where(q => q.CreatedAt.Date == createdAt.Value.Date);
            if (SourceId.HasValue) query = query.Where(q => q.SourceId == SourceId);
            if (IsRiData.HasValue) query = query.Where(q => q.IsRiData == IsRiData);
            if (IsClaimRegister.HasValue) query = query.Where(q => q.IsClaimRegister == IsClaimRegister);
            if (IsReferralClaim.HasValue) query = query.Where(q => q.IsReferralClaim == IsReferralClaim);
            if (Type.HasValue) query = query.Where(q => q.Type == Type);
            if (BatchId.HasValue) query = query.Where(q => q.BatchId == BatchId);
            if (!string.IsNullOrEmpty(CreatedBy)) query = query.Where(q => q.CreatedBy.FullName.Contains(CreatedBy));
            if (processStartAt.HasValue) query = query.Where(q => q.ProcessStartAt.HasValue && DbFunctions.TruncateTime(q.ProcessStartAt.Value) == DbFunctions.TruncateTime(processStartAt.Value));
            if (processEndAt.HasValue) query = query.Where(q => q.ProcessEndAt.HasValue && DbFunctions.TruncateTime(q.ProcessEndAt) == DbFunctions.TruncateTime(processEndAt.Value));
            if (UnprocessedRecords.HasValue && UnprocessedRecords.Value) query = query.Where(q => q.UnprocessedRecords > 0);
            if (Status.HasValue) query = query.Where(q => q.Status == Status);

            if (SortOrder == Html.GetSortAsc("CreatedAt")) query = query.OrderBy(q => q.CreatedAt);
            else if (SortOrder == Html.GetSortDsc("CreatedAt")) query = query.OrderByDescending(q => q.CreatedAt);
            else if (SortOrder == Html.GetSortAsc("SourceId")) query = query.OrderBy(q => q.Source.Name);
            else if (SortOrder == Html.GetSortDsc("SourceId")) query = query.OrderByDescending(q => q.Source.Name);
            else if (SortOrder == Html.GetSortAsc("IsRiData")) query = query.OrderBy(q => q.IsRiData);
            else if (SortOrder == Html.GetSortDsc("IsRiData")) query = query.OrderByDescending(q => q.IsRiData);
            else if (SortOrder == Html.GetSortAsc("IsClaimRegister")) query = query.OrderBy(q => q.IsClaimRegister);
            else if (SortOrder == Html.GetSortDsc("IsClaimRegister")) query = query.OrderByDescending(q => q.IsClaimRegister);
            else if (SortOrder == Html.GetSortAsc("IsReferralClaim")) query = query.OrderBy(q => q.IsReferralClaim);
            else if (SortOrder == Html.GetSortDsc("IsReferralClaim")) query = query.OrderByDescending(q => q.IsReferralClaim);
            else if (SortOrder == Html.GetSortAsc("Type")) query = query.OrderBy(q => q.Type);
            else if (SortOrder == Html.GetSortDsc("Type")) query = query.OrderByDescending(q => q.Type);
            else if (SortOrder == Html.GetSortAsc("BatchId")) query = query.OrderBy(q => q.BatchId);
            else if (SortOrder == Html.GetSortDsc("BatchId")) query = query.OrderByDescending(q => q.BatchId);
            else if (SortOrder == Html.GetSortAsc("CreatedBy")) query = query.OrderBy(q => q.CreatedBy.FullName);
            else if (SortOrder == Html.GetSortDsc("CreatedBy")) query = query.OrderByDescending(q => q.CreatedBy.FullName);
            else if (SortOrder == Html.GetSortAsc("ProcessStartAt")) query = query.OrderBy(q => q.ProcessStartAt);
            else if (SortOrder == Html.GetSortDsc("ProcessStartAt")) query = query.OrderByDescending(q => q.ProcessStartAt);
            else if (SortOrder == Html.GetSortAsc("ProcessEndAt")) query = query.OrderBy(q => q.ProcessEndAt);
            else if (SortOrder == Html.GetSortDsc("ProcessEndAt")) query = query.OrderByDescending(q => q.ProcessEndAt);
            else if (SortOrder == Html.GetSortAsc("Status")) query = query.OrderBy(q => q.Status);
            else if (SortOrder == Html.GetSortDsc("Status")) query = query.OrderByDescending(q => q.Status);
            else query = query.OrderByDescending(q => q.Id);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: SanctionVerification/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new SanctionVerificationViewModel());
        }

        // POST: SanctionVerification/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(SanctionVerificationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);

                var trail = GetNewTrailObject();
                Result = SanctionVerificationService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;

                    CreateTrail(
                        bo.Id,
                        "Create Sanction Verification"
                    );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }
            LoadPage();
            return View(model);
        }

        // GET: SanctionVerification/Edit/5
        public ActionResult Edit(
            int id,
            int? ModuleId,
            string UploadDate,
            string Category,
            string CedingCompany,
            string TreatyCode,
            string CedingPlanCode,
            string PolicyNumber,
            string InsuredName,
            string InsuredDateOfBirth,
            string InsuredIcNumber,
            string SoaQuarter,
            string SumReins,
            string ClaimAmount,
            bool? IsWhiteList,
            string SortOrder,
            int? Page,
            string SelectedIds
        )
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = SanctionVerificationService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            // For Detail Ids
            ViewBag.SelectedIds = SelectedIds;

            // Listing Matched Record
            ListDetail(
                id,
                Page,
                ModuleId,
                UploadDate,
                Category,
                CedingCompany,
                TreatyCode,
                CedingPlanCode,
                PolicyNumber,
                InsuredName,
                InsuredDateOfBirth,
                InsuredIcNumber,
                SoaQuarter,
                SumReins,
                ClaimAmount,
                IsWhiteList,
                SortOrder
            );

            LoadPage(bo);
            return View(new SanctionVerificationViewModel(bo));
        }

        // POST: SanctionVerification/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, int? Page, FormCollection form, SanctionVerificationViewModel model)
        {
            var dbBo = SanctionVerificationService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                SanctionVerificationService.CountTotalUnprocessed(ref bo);

                var trail = GetNewTrailObject();

                Result = SanctionVerificationService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Update Sanction Verification"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            ListDetail(
                id,
                Page
            );

            LoadPage(dbBo);
            return View(model);
        }

        public void ListDetail(
            int id,
            int? Page,
            int? ModuleId = null,
            string UploadDate = null,
            string Category = null,
            string CedingCompany = null,
            string TreatyCode = null,
            string CedingPlanCode = null,
            string PolicyNumber = null,
            string InsuredName = null,
            string InsuredDateOfBirth = null,
            string InsuredIcNumber = null,
            string SoaQuarter = null,
            string SumReins = null,
            string ClaimAmount = null,
            bool? IsWhitelist = null,
            string SortOrder = null
        )
        {
            DateTime? uploadDate = Util.GetParseDateTime(UploadDate);
            DateTime? insuredDateOfBirth = Util.GetParseDateTime(InsuredDateOfBirth);

            double? sumReins = Util.StringToDouble(SumReins);
            double? claimAmount = Util.StringToDouble(ClaimAmount);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["ModuleId"] = ModuleId,
                ["UploadDate"] = uploadDate.HasValue ? UploadDate : null,
                ["Category"] = Category,
                ["CedingCompany"] = CedingCompany,
                ["TreatyCode"] = TreatyCode,
                ["CedingPlanCode"] = CedingPlanCode,
                ["PolicyNumber"] = PolicyNumber,
                ["InsuredName"] = InsuredName,
                ["InsuredDateOfBirth"] = insuredDateOfBirth.HasValue ? InsuredDateOfBirth : null,
                ["InsuredIcNumber"] = InsuredIcNumber,
                ["SoaQuarter"] = SoaQuarter,
                ["SumReins"] = sumReins.HasValue ? SumReins : null,
                ["ClaimAmount"] = claimAmount.HasValue ? ClaimAmount : null,
                ["IsWhitelist"] = IsWhitelist,
                ["SortOrder"] = SortOrder,
            };

            ViewBag.SortOrder = SortOrder;
            ViewBag.SortModuleId = GetSortParam("ModuleId");
            ViewBag.SortUploadDate = GetSortParam("UploadDate");
            ViewBag.SortCategory = GetSortParam("Category");
            ViewBag.SortCedingCompany = GetSortParam("CedingCompany");
            ViewBag.SortTreatyCode = GetSortParam("TreatyCode");
            ViewBag.SortCedingPlanCode = GetSortParam("CedingPlanCode");
            ViewBag.SortPolicyNumber = GetSortParam("PolicyNumber");
            ViewBag.SortInsuredName = GetSortParam("InsuredName");
            ViewBag.SortInsuredDateOfBirth = GetSortParam("InsuredDateOfBirth");
            ViewBag.SortInsuredIcNumber = GetSortParam("InsuredIcNumber");
            ViewBag.SortSoaQuarter = GetSortParam("SoaQuarter");
            ViewBag.SortSumReins = GetSortParam("SumReins");
            ViewBag.SortClaimAmount = GetSortParam("ClaimAmount");
            ViewBag.SortIsWhiteList = GetSortParam("IsWhiteList");

            _db.Database.CommandTimeout = 0;

            var query = _db.SanctionVerificationDetails.AsNoTracking().Where(q => q.SanctionVerificationId == id).Where(q => q.IsExactMatch == false).Select(SanctionVerificationDetailListingViewModel.Expression());

            if (ModuleId.HasValue) query = query.Where(q => q.ModuleId == ModuleId);
            if (uploadDate.HasValue) query = query.Where(q => q.UploadDate == uploadDate);
            if (!string.IsNullOrEmpty(Category)) query = query.Where(q => q.Category == Category);
            if (!string.IsNullOrEmpty(CedingCompany)) query = query.Where(q => q.CedingCompany.Contains(CedingCompany));
            if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => q.TreatyCode.Contains(TreatyCode));
            if (!string.IsNullOrEmpty(CedingPlanCode)) query = query.Where(q => q.CedingPlanCode.Contains(CedingPlanCode));
            if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber.Contains(PolicyNumber));
            if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => q.InsuredName.Contains(InsuredName));
            if (insuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == insuredDateOfBirth);
            if (!string.IsNullOrEmpty(InsuredIcNumber)) query = query.Where(q => q.InsuredIcNumber.Contains(InsuredIcNumber));
            if (!string.IsNullOrEmpty(SoaQuarter)) query = query.Where(q => q.SoaQuarter == SoaQuarter);
            if (sumReins.HasValue) query = query.Where(q => q.SumReins == sumReins);
            if (claimAmount.HasValue) query = query.Where(q => q.ClaimAmount == claimAmount);
            if (IsWhitelist.HasValue) query = query.Where(q => q.IsWhitelist == IsWhitelist);

            if (SortOrder == Html.GetSortAsc("ModuleId")) query = query.OrderBy(q => q.Module.Name);
            else if (SortOrder == Html.GetSortDsc("ModuleId")) query = query.OrderByDescending(q => q.Module.Name);
            else if (SortOrder == Html.GetSortAsc("UploadDate")) query = query.OrderBy(q => q.UploadDate);
            else if (SortOrder == Html.GetSortDsc("UploadDate")) query = query.OrderByDescending(q => q.UploadDate);
            else if (SortOrder == Html.GetSortAsc("Category")) query = query.OrderBy(q => q.Category);
            else if (SortOrder == Html.GetSortDsc("Category")) query = query.OrderByDescending(q => q.Category);
            else if (SortOrder == Html.GetSortAsc("CedingCompany")) query = query.OrderBy(q => q.CedingCompany);
            else if (SortOrder == Html.GetSortDsc("CedingCompany")) query = query.OrderByDescending(q => q.CedingCompany);
            else if (SortOrder == Html.GetSortAsc("TreatyCode")) query = query.OrderBy(q => q.TreatyCode);
            else if (SortOrder == Html.GetSortDsc("TreatyCode")) query = query.OrderByDescending(q => q.TreatyCode);
            else if (SortOrder == Html.GetSortAsc("CedingPlanCode")) query = query.OrderBy(q => q.CedingPlanCode);
            else if (SortOrder == Html.GetSortDsc("CedingPlanCode")) query = query.OrderByDescending(q => q.CedingPlanCode);
            else if (SortOrder == Html.GetSortAsc("PolicyNumber")) query = query.OrderBy(q => q.PolicyNumber);
            else if (SortOrder == Html.GetSortDsc("PolicyNumber")) query = query.OrderByDescending(q => q.PolicyNumber);
            else if (SortOrder == Html.GetSortAsc("InsuredName")) query = query.OrderBy(q => q.InsuredName);
            else if (SortOrder == Html.GetSortDsc("InsuredName")) query = query.OrderByDescending(q => q.InsuredName);
            else if (SortOrder == Html.GetSortAsc("InsuredDateOfBirth")) query = query.OrderBy(q => q.InsuredDateOfBirth);
            else if (SortOrder == Html.GetSortDsc("InsuredDateOfBirth")) query = query.OrderByDescending(q => q.InsuredDateOfBirth);
            else if (SortOrder == Html.GetSortAsc("InsuredIcNumber")) query = query.OrderBy(q => q.InsuredIcNumber);
            else if (SortOrder == Html.GetSortDsc("InsuredIcNumber")) query = query.OrderByDescending(q => q.InsuredIcNumber);
            else if (SortOrder == Html.GetSortAsc("SoaQuarter")) query = query.OrderBy(q => q.SoaQuarter);
            else if (SortOrder == Html.GetSortDsc("SoaQuarter")) query = query.OrderByDescending(q => q.SoaQuarter);
            else if (SortOrder == Html.GetSortAsc("SumReins")) query = query.OrderBy(q => q.SumReins);
            else if (SortOrder == Html.GetSortDsc("SumReins")) query = query.OrderByDescending(q => q.SumReins);
            else if (SortOrder == Html.GetSortAsc("ClaimAmount")) query = query.OrderBy(q => q.ClaimAmount);
            else if (SortOrder == Html.GetSortDsc("ClaimAmount")) query = query.OrderByDescending(q => q.ClaimAmount);
            else if (SortOrder == Html.GetSortAsc("IsWhitelist")) query = query.OrderBy(q => q.IsWhitelist);
            else if (SortOrder == Html.GetSortDsc("IsWhitelist")) query = query.OrderByDescending(q => q.IsWhitelist);
            else query = query.OrderBy(q => q.Id);

            ViewBag.DetailTotal = query.Count();
            ViewBag.DetailList = query.ToPagedList(Page ?? 1, PageSize);
        }

        [Auth(Controller = Controller, Power = "R")]
        public ActionResult DownloadDetail(
            string downloadToken,
            int type,
            int id,
            int? ModuleId,
            string UploadDate,
            string Category,
            string CedingCompany,
            string TreatyCode,
            string CedingPlanCode,
            string PolicyNumber,
            string InsuredName,
            string InsuredDateOfBirth,
            string InsuredIcNumber,
            string SoaQuarter,
            string SumReins,
            string ClaimAmount,
            bool? IsWhitelist
        )
        {
            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            _db.Database.CommandTimeout = 0;
            var query = _db.SanctionVerificationDetails.Where(q => q.SanctionVerificationId == id).Where(q => q.IsExactMatch == false).Select(SanctionVerificationDetailService.Expression());

            if (type == 2)
            {
                DateTime? uploadDate = Util.GetParseDateTime(UploadDate);
                DateTime? insuredDateOfBirth = Util.GetParseDateTime(InsuredDateOfBirth);

                double? sumReins = Util.StringToDouble(SumReins);
                double? claimAmount = Util.StringToDouble(ClaimAmount);

                if (ModuleId.HasValue) query = query.Where(q => q.ModuleId == ModuleId);
                if (uploadDate.HasValue) query = query.Where(q => q.UploadDate == uploadDate);
                if (!string.IsNullOrEmpty(Category)) query = query.Where(q => q.Category == Category);
                if (!string.IsNullOrEmpty(CedingCompany)) query = query.Where(q => q.CedingCompany.Contains(CedingCompany));
                if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => q.TreatyCode.Contains(TreatyCode));
                if (!string.IsNullOrEmpty(CedingPlanCode)) query = query.Where(q => q.CedingPlanCode.Contains(CedingPlanCode));
                if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber.Contains(PolicyNumber));
                if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => q.InsuredName.Contains(InsuredName));
                if (insuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == insuredDateOfBirth);
                if (!string.IsNullOrEmpty(InsuredIcNumber)) query = query.Where(q => q.InsuredIcNumber.Contains(InsuredIcNumber));
                if (!string.IsNullOrEmpty(SoaQuarter)) query = query.Where(q => q.SoaQuarter == SoaQuarter);
                if (sumReins.HasValue) query = query.Where(q => q.SumReins == sumReins);
                if (claimAmount.HasValue) query = query.Where(q => q.ClaimAmount == claimAmount);
                if (IsWhitelist.HasValue) query = query.Where(q => q.IsWhitelist == IsWhitelist);
            }

            var export = new ExportSanctionVerificationDetail();
            export.HandleTempDirectory();
            export.SetQuery(query);
            export.Process();

            return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));
        }

        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Whitelist(int id, SanctionVerificationViewModel model, FormCollection form)
        {
            if (string.IsNullOrEmpty(model.SelectedIds))
            {
                SetErrorSessionMsg("No Records Selected");
                return RedirectToAction("Edit", new { id });
            }

            int success = 0;
            var ids = model.SelectedIds.Split(',').Select(q => int.Parse(q.Trim())).ToList();
            var trail = GetNewTrailObject();
            foreach (int i in ids)
            {
                var bo = SanctionVerificationDetailService.Find(i);
                if (bo == null || bo.SanctionVerificationId != id)
                    continue;

                trail = GetNewTrailObject();
                Result = WhitelistSanction(bo, ref trail, false, form.Get("Remark"));
                if (Result.Valid)
                {
                    success++;
                    CreateTrail(
                        bo.Id,
                        "Update Sanction Verification Detail Whitelist"
                    );
                }
            }

            // Update total unprocessed
            trail = GetNewTrailObject();
            Result = SanctionVerificationService.UpdateTotalUnprocessed(model.Id, AuthUserId, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    model.Id,
                    "Update Sanction Verification"
                );
            }

            SetSuccessSessionMsg(string.Format("Total Records Whitelisted: {0}", success));
            return RedirectToAction("Edit", new { id });
        }

        [Auth(Controller = Controller, Power = "U")]
        public ActionResult ExactMatch(int id, SanctionVerificationViewModel model, FormCollection form)
        {
            if (string.IsNullOrEmpty(model.SelectedIds))
            {
                SetErrorSessionMsg("No Records Selected");
                return RedirectToAction("Edit", new { id });
            }

            int success = 0;
            var ids = model.SelectedIds.Split(',').Select(q => int.Parse(q.Trim())).ToList();

            var claimRegisterModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.ClaimRegister.ToString());
            var referralClaimModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.ReferralClaim.ToString());

            var trail = GetNewTrailObject();

            foreach (int i in ids)
            {
                var bo = SanctionVerificationDetailService.Find(i);
                if (bo == null || bo.SanctionVerificationId != id)
                    continue;


                trail = GetNewTrailObject();
                Result = ExactMatchSanction(bo, ref trail, claimRegisterModuleBo.Id, referralClaimModuleBo.Id);
                if (Result.Valid)
                {
                    success++;
                    CreateTrail(
                        bo.Id,
                        "Update Sanction Verification Detail Exact Matched"
                    );
                }
            }

            // Update total unprocessed
            trail = GetNewTrailObject();
            Result = SanctionVerificationService.UpdateTotalUnprocessed(model.Id, AuthUserId, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    model.Id,
                    "Update Sanction Verification"
                );
            }

            SetSuccessSessionMsg(string.Format("Total Records Exact Matched: {0}", success));
            return RedirectToAction("Edit", new { id });
        }

        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Remain(int id, SanctionVerificationViewModel model, FormCollection form)
        {
            if (string.IsNullOrEmpty(model.SelectedIds))
            {
                SetErrorSessionMsg("No Records Selected");
                return RedirectToAction("Edit", new { id });
            }

            int whitelisted = 0;
            int exactMatched = 0;
            var ids = model.SelectedIds.Split(',').Select(q => int.Parse(q.Trim())).ToList();

            var claimRegisterModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.ClaimRegister.ToString());
            var referralClaimModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.ReferralClaim.ToString());

            var trail = GetNewTrailObject();
            foreach (int i in ids)
            {
                var bo = SanctionVerificationDetailService.Find(i);
                if (bo == null || bo.SanctionVerificationId != id)
                    continue;

                Result = null;
                string action = "";

                trail = GetNewTrailObject();
                switch (bo.PreviousDecision)
                {
                    case SanctionVerificationDetailBo.PreviousDecisionWhitelist:
                        Result = WhitelistSanction(bo, ref trail, true);
                        action = "Whitelist";
                        if (Result.Valid)
                            whitelisted++;
                        break;
                    case SanctionVerificationDetailBo.PreviousDecisionExactMatch:
                        action = "Exact Matched";
                        Result = ExactMatchSanction(bo, ref trail, claimRegisterModuleBo.Id, referralClaimModuleBo.Id);
                        if (Result.Valid)
                            exactMatched++;
                        break;
                    default:
                        break;
                }

                if (Result != null && Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        string.Format("Update Sanction Verification Detail {0}", action)
                    );
                }
            }

            // Update total unprocessed
            trail = GetNewTrailObject();
            Result = SanctionVerificationService.UpdateTotalUnprocessed(model.Id, AuthUserId, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    model.Id,
                    "Update Sanction Verification"
                );
            }

            SetSuccessSessionMsgArr(
                new List<string>()
                {
                    string.Format("Total Records Whitelisted: {0}", whitelisted),
                    string.Format("Total Records Exact Matched: {0}", exactMatched)
                }
            );
            return RedirectToAction("Edit", new { id });
        }

        public Result WhitelistSanction(SanctionVerificationDetailBo detailBo, ref TrailObject trail, bool remain = false, string remark = "", int authUserId = 0)
        {
            if (authUserId == 0)
                authUserId = AuthUserId;

            detailBo.IsWhitelist = true;
            detailBo.Remark = remain ? detailBo.PreviousDecisionRemark : remark;
            detailBo.UpdatedById = authUserId;

            Result = SanctionVerificationDetailService.Update(ref detailBo, ref trail);
            if (Result.Valid)
            {
                SanctionWhitelistBo whitelistBo = SanctionWhitelistService.Find(detailBo.PolicyNumber, detailBo.InsuredName);
                if (whitelistBo == null)
                {
                    whitelistBo = new SanctionWhitelistBo()
                    {
                        PolicyNumber = detailBo.PolicyNumber,
                        InsuredName = detailBo.InsuredName,
                        CreatedById = authUserId
                    };
                }

                if (whitelistBo.Id == 0 || (!remain && whitelistBo.Reason != detailBo.Remark))
                {
                    whitelistBo.Reason = detailBo.Remark;
                    whitelistBo.UpdatedById = authUserId;
                    SanctionWhitelistService.Save(ref whitelistBo, ref trail);
                }

                var sanctionBlacklistBo = SanctionBlacklistService.Find(detailBo.PolicyNumber, detailBo.InsuredName);
                if (sanctionBlacklistBo != null)
                {
                    SanctionBlacklistService.Delete(sanctionBlacklistBo, ref trail);
                }
            }
            return Result;
        }

        public Result ExactMatchSanction(SanctionVerificationDetailBo detailBo, ref TrailObject trail, int claimRegisterModuleId, int referralClaimModuleId, int authUserId = 0)
        {
            if (authUserId == 0)
                authUserId = AuthUserId;

            detailBo.IsExactMatch = true;
            detailBo.UpdatedById = authUserId;

            if (detailBo.ModuleId == claimRegisterModuleId)
            {
                var claimregisterBo = ClaimRegisterService.Find(detailBo.ObjectId);
                if (claimregisterBo != null)
                {
                    detailBo.PolicyStatusCode = claimregisterBo.ClaimStatus == ClaimRegisterBo.StatusApproved ? "Closed" : "Active";
                    detailBo.RiskCoverageEndDate = claimregisterBo.DateApproved;
                }
            }
            else if (detailBo.ModuleId == referralClaimModuleId)
            {
                var referralClaimBo = ReferralClaimService.Find(detailBo.ObjectId);
                if (referralClaimBo != null)
                {
                    detailBo.PolicyStatusCode = referralClaimBo.ClaimsDecision == ReferralClaimBo.ClaimsDecisionApproved ? "Closed" : "Active";
                    detailBo.RiskCoverageEndDate = referralClaimBo.ClaimsDecisionDate;
                }
            }

            Result = SanctionVerificationDetailService.Update(ref detailBo, ref trail);
            if (Result.Valid)
            {
                if (!SanctionBlacklistService.IsExists(detailBo.PolicyNumber, detailBo.InsuredName))
                {
                    SanctionBlacklistBo blacklistBo = new SanctionBlacklistBo()
                    {
                        PolicyNumber = detailBo.PolicyNumber,
                        InsuredName = detailBo.InsuredName,
                        CreatedById = authUserId,
                        UpdatedById = authUserId
                    };
                    SanctionBlacklistService.Save(ref blacklistBo, ref trail);
                }

                var sanctionWhitelistBo = SanctionWhitelistService.Find(detailBo.PolicyNumber, detailBo.InsuredName);
                if (sanctionWhitelistBo != null)
                {
                    SanctionWhitelistService.Delete(sanctionWhitelistBo, ref trail);
                }
            }
            return Result;
        }

        public void IndexPage()
        {
            DropDownSource();
            DropDownType();
            DropDownStatus();
            DropDownYesNoWithSelect();
            SetViewBagMessage();
        }

        public void LoadPage(SanctionVerificationBo bo = null)
        {
            AuthUserName();
            ViewBag.IsClaimUser = AuthUser.DepartmentId == DepartmentBo.DepartmentClaim;
            DropDownSource();
            DropDownType();
            DropDownYesNoWithSelect();
            DropDownModule(bo);
            DropDownCategory();
            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= SanctionVerificationBo.StatusMax; i++)
            {
                items.Add(new SelectListItem { Text = SanctionVerificationBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownStatuses = items;
            return items;
        }

        public List<SelectListItem> DropDownCategory()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= SanctionBo.CategoryMax; i++)
            {
                items.Add(new SelectListItem { Text = SanctionBo.GetCategoryName(i), Value = SanctionBo.GetCategoryName(i) });
            }
            ViewBag.DropDownCategories = items;
            return items;
        }

        public List<SelectListItem> DropDownType()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= SanctionVerificationBo.TypeMax; i++)
            {
                items.Add(new SelectListItem { Text = SanctionVerificationBo.GetTypeName(i), Value = i.ToString() });
            }
            ViewBag.DropDownTypes = items;
            return items;
        }

        public List<SelectListItem> DropDownModule(SanctionVerificationBo bo)
        {
            var items = GetEmptyDropDownList();

            if (bo != null)
            {
                if (bo.IsRiData)
                {
                    var moduleBo = ModuleService.FindByController("RiData");
                    if (moduleBo != null)
                        items.Add(new SelectListItem { Text = moduleBo.Name, Value = moduleBo.Id.ToString() });
                }

                if (bo.IsClaimRegister)
                {
                    var moduleBo = ModuleService.FindByController("ClaimRegister");
                    if (moduleBo != null)
                        items.Add(new SelectListItem { Text = moduleBo.Name, Value = moduleBo.Id.ToString() });
                }

                if (bo.IsReferralClaim)
                {
                    var moduleBo = ModuleService.FindByController("ReferralClaim");
                    if (moduleBo != null)
                        items.Add(new SelectListItem { Text = moduleBo.Name, Value = moduleBo.Id.ToString() });
                }
            }
            else
            {
                List<string> controllers = new List<string> { "RiData", "ClaimRegister", "ReferralClaim" };
                foreach (string controller in controllers)
                {
                    var moduleBo = ModuleService.FindByController(controller);
                    if (moduleBo != null)
                        items.Add(new SelectListItem { Text = moduleBo.Name, Value = moduleBo.Id.ToString() });
                }
            }

            ViewBag.DropDownModuleIds = items;
            return items;
        }
    }
}