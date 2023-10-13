using BusinessObject;
using BusinessObject.Identity;
using ConsoleApp.Commands.ProcessDatas.Exports;
using Ionic.Zip;
using PagedList;
using Services;
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
    public class FinanceProvisioningController : BaseController
    {
        public const string Controller = "FinanceProvisioning";

        // GET: FinanceProvisioning
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string Date,
            int? Status,
            int? ClaimsProvisionRecord,
            string ClaimsProvisionAmount,
            int? DrProvisionRecord,
            string DrProvisionAmount,
            string SortOrder,
            int? Page
        )
        {
            DateTime? date = Util.GetParseDateTime(Date);
            double? claimProvisionAmount = Util.StringToDouble(ClaimsProvisionAmount);
            double? drProvisionAmount = Util.StringToDouble(DrProvisionAmount);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Date"] = date.HasValue ? Date : null,
                ["Status"] = Status,
                ["ClaimsProvisionRecord"] = ClaimsProvisionRecord,
                ["ClaimsProvisionAmount"] = claimProvisionAmount.HasValue ? ClaimsProvisionAmount : null,
                ["DrProvisionRecord"] = DrProvisionRecord,
                ["DrProvisionAmount"] = drProvisionAmount.HasValue ? DrProvisionAmount : null,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortDate = GetSortParam("Date");
            ViewBag.SortStatus = GetSortParam("Status");
            ViewBag.SortClaimsProvisionRecord = GetSortParam("ClaimsProvisionRecord");
            ViewBag.SortClaimsProvisionAmount = GetSortParam("ClaimsProvisionAmount");
            ViewBag.SortDrProvisionRecord = GetSortParam("DrProvisionRecord");
            ViewBag.SortDrProvisionAmount = GetSortParam("DrProvisionAmount");

            var query = _db.FinanceProvisionings.Select(FinanceProvisioningViewModel.Expression());

            if (date.HasValue) query = query.Where(q => q.Date == date);
            if (Status.HasValue) query = query.Where(q => q.Status == Status);
            if (ClaimsProvisionRecord.HasValue) query = query.Where(q => q.ClaimsProvisionRecord == ClaimsProvisionRecord);
            if (claimProvisionAmount.HasValue) query = query.Where(q => q.ClaimsProvisionAmount == claimProvisionAmount);
            if (DrProvisionRecord.HasValue) query = query.Where(q => q.DrProvisionRecord == DrProvisionRecord);
            if (drProvisionAmount.HasValue) query = query.Where(q => q.DrProvisionAmount == drProvisionAmount);

            if (SortOrder == Html.GetSortAsc("Date")) query = query.OrderBy(q => q.Date);
            else if (SortOrder == Html.GetSortDsc("Date")) query = query.OrderByDescending(q => q.Date);
            else if (SortOrder == Html.GetSortAsc("Status")) query = query.OrderBy(q => q.Status);
            else if (SortOrder == Html.GetSortDsc("Status")) query = query.OrderByDescending(q => q.Status);
            else if (SortOrder == Html.GetSortAsc("ClaimsProvisionRecord")) query = query.OrderBy(q => q.ClaimsProvisionRecord);
            else if (SortOrder == Html.GetSortDsc("ClaimsProvisionRecord")) query = query.OrderByDescending(q => q.ClaimsProvisionRecord);
            else if (SortOrder == Html.GetSortAsc("ClaimsProvisionAmount")) query = query.OrderBy(q => q.ClaimsProvisionAmount);
            else if (SortOrder == Html.GetSortDsc("ClaimsProvisionAmount")) query = query.OrderByDescending(q => q.ClaimsProvisionAmount);
            else if (SortOrder == Html.GetSortAsc("DrProvisionRecord")) query = query.OrderBy(q => q.DrProvisionRecord);
            else if (SortOrder == Html.GetSortDsc("DrProvisionRecord")) query = query.OrderByDescending(q => q.DrProvisionRecord);
            else if (SortOrder == Html.GetSortAsc("DrProvisionAmount")) query = query.OrderBy(q => q.DrProvisionAmount);
            else if (SortOrder == Html.GetSortDsc("DrProvisionAmount")) query = query.OrderByDescending(q => q.DrProvisionAmount);
            else query = query.OrderByDescending(q => q.Id);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: FinanceProvisioning/Edit/5
        public ActionResult Edit(
            int id,
            bool? HasRedFlag,
            string EntryNo,
            string SoaQuarter,
            string ClaimId,
            string ClaimTransactionType,
            bool? IsReferralCase,
            int? RiDataWarehouseId,
            string RecordType,
            string TreatyCode,
            string PolicyNumber,
            string CedingCompany,
            string InsuredName,
            string InsuredDateOfBirth,
            string LastTransactionDate,
            string DateOfReported,
            string CedantDateOfNotification,
            string DateOfRegister,
            string ReinsEffDatePol,
            string DateOfEvent,
            int? PolicyDuration,
            string TargetDateToIssueInvoice,
            string SumIns,
            string CauseOfEvent,
            int? PicClaimId,
            int? PicDaaId,
            int? ClaimStatus,
            int? ProvisionStatus,
            int? OffsetStatus,
            string SortOrder,
            int? Page
        )
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = FinanceProvisioningService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var model = new FinanceProvisioningViewModel(bo);

            // For Claim Data listing table
            ListClaimRegister(
                id,
                Page,
                HasRedFlag,
                EntryNo,
                SoaQuarter,
                ClaimId,
                ClaimTransactionType,
                IsReferralCase,
                RiDataWarehouseId,
                RecordType,
                TreatyCode,
                PolicyNumber,
                CedingCompany,
                InsuredName,
                InsuredDateOfBirth,
                LastTransactionDate,
                DateOfReported,
                CedantDateOfNotification,
                DateOfRegister,
                ReinsEffDatePol,
                DateOfEvent,
                PolicyDuration,
                TargetDateToIssueInvoice,
                SumIns,
                CauseOfEvent,
                PicClaimId,
                PicDaaId,
                ClaimStatus,
                ProvisionStatus,
                OffsetStatus,
                SortOrder
            );

            LoadPage(model);
            return View(model);
        }

        // POST: DirectRetro/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, int? Page, FormCollection form, FinanceProvisioningViewModel model)
        {
            var dbBo = FinanceProvisioningService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            Result = FinanceProvisioningService.Result();
            if (ModelState.IsValid)
            {
                if (dbBo.Status != FinanceProvisioningBo.StatusFailed &&
                    model.Status == FinanceProvisioningBo.StatusSubmitForProcessing)
                {
                    Result.AddError("Only \"Failed\" Finance Provisioning able to submit for processing");
                }

                if (Result.Valid)
                {
                    var bo = model.FormBo(dbBo.CreatedById, AuthUserId);

                    bo.Id = dbBo.Id;

                    var trail = GetNewTrailObject();

                    Result = FinanceProvisioningService.Update(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        CreateTrail(
                            bo.Id,
                            "Update Finance Provisioning"
                        );

                        SetUpdateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = bo.Id });
                    }
                }
                AddResult(Result);
            }

            ListClaimRegister(dbBo.Id, Page);

            LoadPage(model);
            return View(model);
        }

        public void ListClaimRegister(
            int id,
            int? Page,
            bool? HasRedFlag = null,
            string EntryNo = null,
            string SoaQuarter = null,
            string ClaimId = null,
            string ClaimTransactionType = null,
            bool? IsReferralCase = null,
            int? RiDataWarehouseId = null,
            string RecordType = null,
            string TreatyCode = null,
            string PolicyNumber = null,
            string CedingCompany = null,
            string InsuredName = null,
            string InsuredDateOfBirth = null,
            string LastTransactionDate = null,
            string DateOfReported = null,
            string CedantDateOfNotification = null,
            string DateOfRegister = null,
            string ReinsEffDatePol = null,
            string DateOfEvent = null,
            int? PolicyDuration = null,
            string TargetDateToIssueInvoice = null,
            string SumIns = null,
            string CauseOfEvent = null,
            int? PicClaimId = null,
            int? PicDaaId = null,
            int? ClaimStatus = null,
            int? ProvisionStatus = null,
            int? OffsetStatus = null,
            string SortOrder = null
        )
        {
            DateTime? insuredDateOfBirth = Util.GetParseDateTime(InsuredDateOfBirth);
            DateTime? lastTransactionDate = Util.GetParseDateTime(LastTransactionDate);
            DateTime? dateOfReported = Util.GetParseDateTime(DateOfReported);
            DateTime? cedantDateOfNotification = Util.GetParseDateTime(CedantDateOfNotification);
            DateTime? dateOfRegister = Util.GetParseDateTime(DateOfRegister);
            DateTime? reinsEffDatePol = Util.GetParseDateTime(ReinsEffDatePol);
            DateTime? dateOfEvent = Util.GetParseDateTime(DateOfEvent);
            DateTime? targetDateToIssueInvoice = Util.GetParseDateTime(TargetDateToIssueInvoice);

            double? sumIns = Util.StringToDouble(SumIns);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["HasRedFlag"] = HasRedFlag,
                ["EntryNo"] = EntryNo,
                ["SoaQuarter"] = SoaQuarter,
                ["ClaimId"] = ClaimId,
                ["ClaimTransactionType"] = ClaimTransactionType,
                ["IsReferralCase"] = IsReferralCase,
                ["RiDataWarehouseId"] = RiDataWarehouseId,
                ["RecordType"] = RecordType,
                ["TreatyCode"] = TreatyCode,
                ["PolicyNumber"] = PolicyNumber,
                ["CedingCompany"] = CedingCompany,
                ["InsuredName"] = InsuredName,
                ["InsuredDateOfBirth"] = insuredDateOfBirth.HasValue ? InsuredDateOfBirth : null,
                ["LastTransactionDate"] = lastTransactionDate.HasValue ? LastTransactionDate : null,
                ["DateOfReported"] = dateOfReported.HasValue ? DateOfReported : null,
                ["CedantDateOfNotification"] = cedantDateOfNotification.HasValue ? CedantDateOfNotification : null,
                ["DateOfRegister"] = dateOfRegister.HasValue ? DateOfRegister : null,
                ["ReinsEffDatePol"] = reinsEffDatePol.HasValue ? ReinsEffDatePol : null,
                ["DateOfEvent"] = dateOfEvent.HasValue ? DateOfEvent : null,
                ["PolicyDuration"] = PolicyDuration,
                ["TargetDateToIssueInvoice"] = targetDateToIssueInvoice.HasValue ? TargetDateToIssueInvoice : null,
                ["SumIns"] = sumIns.HasValue ? SumIns : null,
                ["CauseOfEvent"] = CauseOfEvent,
                ["PicClaimId"] = PicClaimId,
                ["PicDaaId"] = PicDaaId,
                ["ClaimStatus"] = ClaimStatus,
                ["ProvisionStatus"] = ProvisionStatus,
                ["OffsetStatus"] = OffsetStatus,
                ["SortOrder"] = SortOrder,
            };

            ViewBag.SortOrder = SortOrder;
            ViewBag.SortHasRedFlag = GetSortParam("HasRedFlag");
            ViewBag.SortEntryNo = GetSortParam("EntryNo");
            ViewBag.SortSoaQuarter = GetSortParam("SoaQuarter");
            ViewBag.SortClaimId = GetSortParam("ClaimId");
            ViewBag.SortClaimTransactionType = GetSortParam("ClaimTransactionType");
            ViewBag.SortIsReferralCase = GetSortParam("IsReferralCase");
            ViewBag.SortRiDataWarehouseId = GetSortParam("RiDataWarehouseId");
            ViewBag.SortRecordType = GetSortParam("RecordType");
            ViewBag.SortTreatyCode = GetSortParam("TreatyCode");
            ViewBag.SortPolicyNumber = GetSortParam("PolicyNumber");
            ViewBag.SortCedingCompany = GetSortParam("CedingCompany");
            ViewBag.SortInsuredName = GetSortParam("InsuredName");
            ViewBag.SortInsuredDateOfBirth = GetSortParam("InsuredDateOfBirth");
            ViewBag.SortLastTransactionDate = GetSortParam("LastTransactionDate");
            ViewBag.SortDateOfReported = GetSortParam("DateOfReported");
            ViewBag.SortCedantDateOfNotification = GetSortParam("CedantDateOfNotification");
            ViewBag.SortDateOfRegister = GetSortParam("DateOfRegister");
            ViewBag.SortReinsEffDatePol = GetSortParam("ReinsEffDatePol");
            ViewBag.SortDateOfEvent = GetSortParam("DateOfEvent");
            ViewBag.SortPolicyDuration = GetSortParam("PolicyDuration");
            ViewBag.SortTargetDateToIssueInvoice = GetSortParam("TargetDateToIssueInvoice");
            ViewBag.SortSumIns = GetSortParam("SumIns");
            ViewBag.SortCauseOfEvent = GetSortParam("CauseOfEvent");
            ViewBag.SortPicClaimId = GetSortParam("PicClaimId");
            ViewBag.SortPicDaaId = GetSortParam("PicDaaId");
            ViewBag.SortClaimStatus = GetSortParam("ClaimStatus");
            ViewBag.SortProvisionStatus = GetSortParam("ProvisionStatus");
            ViewBag.SortOffsetStatus = GetSortParam("OffsetStatus");

            _db.Database.CommandTimeout = 0;

            List<int> financeClaimRegisterIds = FinanceProvisioningTransactionService.GetClaimRegisterIdByFinanceProvisioningId(id);
            List<int> retroClaimRegisterIds = DirectRetroProvisioningTransactionService.GetClaimRegisterIdByFinanceProvisioningId(id);
            List<int> claimRegisterIds = financeClaimRegisterIds.Union(retroClaimRegisterIds).ToList();

            var query = _db.ClaimRegister.AsNoTracking().Where(q => claimRegisterIds.Contains(q.Id)).Select(ProvisioningClaimRegisterListingViewModel.Expression());

            if (HasRedFlag.HasValue) query = query.Where(q => q.HasRedFlag == HasRedFlag);
            if (!string.IsNullOrEmpty(EntryNo)) query = query.Where(q => q.EntryNo.Contains(EntryNo));
            if (!string.IsNullOrEmpty(SoaQuarter)) query = query.Where(q => q.SoaQuarter == SoaQuarter);
            if (!string.IsNullOrEmpty(ClaimId)) query = query.Where(q => q.ClaimId.Contains(ClaimId));
            if (!string.IsNullOrEmpty(ClaimTransactionType)) query = query.Where(q => q.ClaimTransactionType == ClaimTransactionType);
            if (IsReferralCase.HasValue) query = query.Where(q => q.IsReferralCase == IsReferralCase);
            if (RiDataWarehouseId.HasValue) query = query.Where(q => q.RiDataWarehouseId == RiDataWarehouseId);
            if (!string.IsNullOrEmpty(RecordType)) query = query.Where(q => q.RecordType == RecordType);
            if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => q.TreatyCode == TreatyCode);
            if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber.Contains(PolicyNumber));
            if (!string.IsNullOrEmpty(CedingCompany)) query = query.Where(q => q.CedingCompany == CedingCompany);
            if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => q.InsuredName.Contains(InsuredName));
            if (insuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == insuredDateOfBirth);
            if (lastTransactionDate.HasValue) query = query.Where(q => q.LastTransactionDate == lastTransactionDate);
            if (dateOfReported.HasValue) query = query.Where(q => q.DateOfReported == dateOfReported);
            if (cedantDateOfNotification.HasValue) query = query.Where(q => q.CedantDateOfNotification == cedantDateOfNotification);
            if (dateOfRegister.HasValue) query = query.Where(q => q.DateOfRegister == dateOfRegister);
            if (reinsEffDatePol.HasValue) query = query.Where(q => q.ReinsEffDatePol == reinsEffDatePol);
            if (dateOfEvent.HasValue) query = query.Where(q => q.DateOfEvent == dateOfEvent);
            if (PolicyDuration.HasValue) query = query.Where(q => q.PolicyDuration == PolicyDuration);
            if (targetDateToIssueInvoice.HasValue) query = query.Where(q => q.TargetDateToIssueInvoice == targetDateToIssueInvoice);
            if (sumIns.HasValue) query = query.Where(q => q.SumIns == sumIns);
            if (!string.IsNullOrEmpty(CauseOfEvent)) query = query.Where(q => q.CauseOfEvent.Contains(CauseOfEvent));
            if (PicClaimId.HasValue) query = query.Where(q => q.PicClaimId == PicClaimId);
            if (PicDaaId.HasValue) query = query.Where(q => q.PicDaaId == PicDaaId);
            if (ClaimStatus.HasValue) query = query.Where(q => q.ClaimStatus == ClaimStatus);
            if (ProvisionStatus.HasValue) query = query.Where(q => q.ProvisionStatus == ProvisionStatus);
            if (OffsetStatus.HasValue) query = query.Where(q => q.OffsetStatus == OffsetStatus);

            if (SortOrder == Html.GetSortAsc("HasRedFlag")) query = query.OrderBy(q => q.HasRedFlag);
            else if (SortOrder == Html.GetSortDsc("HasRedFlag")) query = query.OrderByDescending(q => q.HasRedFlag);
            else if (SortOrder == Html.GetSortAsc("EntryNo")) query = query.OrderBy(q => q.EntryNo);
            else if (SortOrder == Html.GetSortDsc("EntryNo")) query = query.OrderByDescending(q => q.EntryNo);
            else if (SortOrder == Html.GetSortAsc("SoaQuarter")) query = query.OrderBy(q => q.SoaQuarter);
            else if (SortOrder == Html.GetSortDsc("SoaQuarter")) query = query.OrderByDescending(q => q.SoaQuarter);
            else if (SortOrder == Html.GetSortAsc("ClaimId")) query = query.OrderBy(q => q.ClaimId);
            else if (SortOrder == Html.GetSortDsc("ClaimId")) query = query.OrderByDescending(q => q.ClaimId);
            else if (SortOrder == Html.GetSortAsc("ClaimTransactionType")) query = query.OrderBy(q => q.ClaimTransactionType);
            else if (SortOrder == Html.GetSortDsc("ClaimTransactionType")) query = query.OrderByDescending(q => q.ClaimTransactionType);
            else if (SortOrder == Html.GetSortAsc("IsReferralCase")) query = query.OrderBy(q => q.IsReferralCase);
            else if (SortOrder == Html.GetSortDsc("IsReferralCase")) query = query.OrderByDescending(q => q.IsReferralCase);
            else if (SortOrder == Html.GetSortAsc("RiDataWarehouseId")) query = query.OrderBy(q => q.RiDataWarehouseId);
            else if (SortOrder == Html.GetSortDsc("RiDataWarehouseId")) query = query.OrderByDescending(q => q.RiDataWarehouseId);
            else if (SortOrder == Html.GetSortAsc("RecordType")) query = query.OrderBy(q => q.RecordType);
            else if (SortOrder == Html.GetSortDsc("RecordType")) query = query.OrderByDescending(q => q.RecordType);
            else if (SortOrder == Html.GetSortAsc("TreatyCode")) query = query.OrderBy(q => q.TreatyCode);
            else if (SortOrder == Html.GetSortDsc("TreatyCode")) query = query.OrderByDescending(q => q.TreatyCode);
            else if (SortOrder == Html.GetSortAsc("PolicyNumber")) query = query.OrderBy(q => q.PolicyNumber);
            else if (SortOrder == Html.GetSortDsc("PolicyNumber")) query = query.OrderByDescending(q => q.PolicyNumber);
            else if (SortOrder == Html.GetSortAsc("CedingCompany")) query = query.OrderBy(q => q.CedingCompany);
            else if (SortOrder == Html.GetSortDsc("CedingCompany")) query = query.OrderByDescending(q => q.CedingCompany);
            else if (SortOrder == Html.GetSortAsc("InsuredName")) query = query.OrderBy(q => q.InsuredName);
            else if (SortOrder == Html.GetSortDsc("InsuredName")) query = query.OrderByDescending(q => q.InsuredName);
            else if (SortOrder == Html.GetSortAsc("InsuredDateOfBirth")) query = query.OrderBy(q => q.InsuredDateOfBirth);
            else if (SortOrder == Html.GetSortDsc("InsuredDateOfBirth")) query = query.OrderByDescending(q => q.InsuredDateOfBirth);
            else if (SortOrder == Html.GetSortAsc("LastTransactionDate")) query = query.OrderBy(q => q.LastTransactionDate);
            else if (SortOrder == Html.GetSortDsc("LastTransactionDate")) query = query.OrderByDescending(q => q.LastTransactionDate);
            else if (SortOrder == Html.GetSortAsc("DateOfReported")) query = query.OrderBy(q => q.DateOfReported);
            else if (SortOrder == Html.GetSortDsc("DateOfReported")) query = query.OrderByDescending(q => q.DateOfReported);
            else if (SortOrder == Html.GetSortAsc("ReinsEffDatePol")) query = query.OrderBy(q => q.ReinsEffDatePol);
            else if (SortOrder == Html.GetSortDsc("ReinsEffDatePol")) query = query.OrderByDescending(q => q.ReinsEffDatePol);
            else if (SortOrder == Html.GetSortAsc("DateOfEvent")) query = query.OrderBy(q => q.DateOfEvent);
            else if (SortOrder == Html.GetSortDsc("DateOfEvent")) query = query.OrderByDescending(q => q.DateOfEvent);
            else if (SortOrder == Html.GetSortAsc("PolicyDuration")) query = query.OrderBy(q => q.PolicyDuration);
            else if (SortOrder == Html.GetSortDsc("PolicyDuration")) query = query.OrderByDescending(q => q.PolicyDuration);
            else if (SortOrder == Html.GetSortAsc("TargetDateToIssueInvoice")) query = query.OrderBy(q => q.TargetDateToIssueInvoice);
            else if (SortOrder == Html.GetSortDsc("TargetDateToIssueInvoice")) query = query.OrderByDescending(q => q.TargetDateToIssueInvoice);
            else if (SortOrder == Html.GetSortAsc("SumIns")) query = query.OrderBy(q => q.SumIns);
            else if (SortOrder == Html.GetSortDsc("SumIns")) query = query.OrderByDescending(q => q.SumIns);
            else if (SortOrder == Html.GetSortAsc("CauseOfEvent")) query = query.OrderBy(q => q.CauseOfEvent);
            else if (SortOrder == Html.GetSortDsc("CauseOfEvent")) query = query.OrderByDescending(q => q.CauseOfEvent);
            else if (SortOrder == Html.GetSortAsc("PicClaimId")) query = query.OrderBy(q => q.PicClaim.FullName);
            else if (SortOrder == Html.GetSortDsc("PicClaimId")) query = query.OrderByDescending(q => q.PicClaim.FullName);
            else if (SortOrder == Html.GetSortAsc("PicDaaId")) query = query.OrderBy(q => q.PicDaa.FullName);
            else if (SortOrder == Html.GetSortDsc("PicDaaId")) query = query.OrderByDescending(q => q.PicDaa.FullName);
            else if (SortOrder == Html.GetSortAsc("ClaimStatus")) query = query.OrderBy(q => q.ClaimStatus);
            else if (SortOrder == Html.GetSortDsc("ClaimStatus")) query = query.OrderByDescending(q => q.ClaimStatus);
            else if (SortOrder == Html.GetSortAsc("ProvisionStatus")) query = query.OrderBy(q => q.ProvisionStatus);
            else if (SortOrder == Html.GetSortDsc("ProvisionStatus")) query = query.OrderByDescending(q => q.ProvisionStatus);
            else if (SortOrder == Html.GetSortAsc("OffsetStatus")) query = query.OrderBy(q => q.OffsetStatus);
            else if (SortOrder == Html.GetSortDsc("OffsetStatus")) query = query.OrderByDescending(q => q.OffsetStatus);
            else query = query.OrderBy(q => q.EntryNo);

            ViewBag.ClaimRegisterTotal = query.Count();
            ViewBag.ClaimRegisterList = query.ToPagedList(Page ?? 1, PageSize);
        }

        [Auth(Controller = Controller, Power = "R")]
        public ActionResult DownloadClaimRegister(
            string downloadToken,
            int type,
            int id,
            bool? HasRedFlag,
            string EntryNo,
            string SoaQuarter,
            string ClaimId,
            string ClaimTransactionType,
            bool? IsReferralCase,
            int? RiDataWarehouseId,
            string RecordType,
            string TreatyCode,
            string PolicyNumber,
            string CedingCompany,
            string InsuredName,
            string InsuredDateOfBirth,
            string LastTransactionDate,
            string DateOfReported,
            string CedantDateOfNotification,
            string DateOfRegister,
            string ReinsEffDatePol,
            string DateOfEvent,
            int? PolicyDuration,
            string TargetDateToIssueInvoice,
            string SumIns,
            string CauseOfEvent,
            int? PicClaimId,
            int? PicDaaId,
            int? ClaimStatus,
            int? ProvisionStatus,
            int? OffsetStatus
        )
        {
            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            _db.Database.CommandTimeout = 0;

            List<int> financeClaimRegisterIds = FinanceProvisioningTransactionService.GetClaimRegisterIdByFinanceProvisioningId(id);
            List<int> retroClaimRegisterIds = DirectRetroProvisioningTransactionService.GetClaimRegisterIdByFinanceProvisioningId(id);
            List<int> claimRegisterIds = financeClaimRegisterIds.Union(retroClaimRegisterIds).ToList();

            var query = _db.ClaimRegister.Where(q => claimRegisterIds.Contains(q.Id)).Select(ClaimRegisterService.Expression());

            if (type == 2)
            {
                DateTime? insuredDateOfBirth = Util.GetParseDateTime(InsuredDateOfBirth);
                DateTime? lastTransactionDate = Util.GetParseDateTime(LastTransactionDate);
                DateTime? dateOfReported = Util.GetParseDateTime(DateOfReported);
                DateTime? cedantDateOfNotification = Util.GetParseDateTime(CedantDateOfNotification);
                DateTime? dateOfRegister = Util.GetParseDateTime(DateOfRegister);
                DateTime? reinsEffDatePol = Util.GetParseDateTime(ReinsEffDatePol);
                DateTime? dateOfEvent = Util.GetParseDateTime(DateOfEvent);
                DateTime? targetDateToIssueInvoice = Util.GetParseDateTime(TargetDateToIssueInvoice);

                double? sumIns = Util.StringToDouble(SumIns);

                if (HasRedFlag.HasValue) query = query.Where(q => q.HasRedFlag == HasRedFlag);
                if (!string.IsNullOrEmpty(EntryNo)) query = query.Where(q => q.EntryNo.Contains(EntryNo));
                if (!string.IsNullOrEmpty(SoaQuarter)) query = query.Where(q => q.SoaQuarter == SoaQuarter);
                if (!string.IsNullOrEmpty(ClaimId)) query = query.Where(q => q.ClaimId.Contains(ClaimId));
                if (!string.IsNullOrEmpty(ClaimTransactionType)) query = query.Where(q => q.ClaimTransactionType == ClaimTransactionType);
                if (IsReferralCase.HasValue) query = query.Where(q => q.IsReferralCase == IsReferralCase);
                if (RiDataWarehouseId.HasValue) query = query.Where(q => q.RiDataWarehouseId == RiDataWarehouseId);
                if (!string.IsNullOrEmpty(RecordType)) query = query.Where(q => q.RecordType == RecordType);
                if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => q.TreatyCode == TreatyCode);
                if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber.Contains(PolicyNumber));
                if (!string.IsNullOrEmpty(CedingCompany)) query = query.Where(q => q.CedingCompany == CedingCompany);
                if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => q.InsuredName.Contains(InsuredName));
                if (insuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == insuredDateOfBirth);
                if (lastTransactionDate.HasValue) query = query.Where(q => q.LastTransactionDate == lastTransactionDate);
                if (dateOfReported.HasValue) query = query.Where(q => q.DateOfReported == dateOfReported);
                if (cedantDateOfNotification.HasValue) query = query.Where(q => q.CedantDateOfNotification == cedantDateOfNotification);
                if (dateOfRegister.HasValue) query = query.Where(q => q.DateOfRegister == dateOfRegister);
                if (reinsEffDatePol.HasValue) query = query.Where(q => q.ReinsEffDatePol == reinsEffDatePol);
                if (dateOfEvent.HasValue) query = query.Where(q => q.DateOfEvent == dateOfEvent);
                if (PolicyDuration.HasValue) query = query.Where(q => q.PolicyDuration == PolicyDuration);
                if (targetDateToIssueInvoice.HasValue) query = query.Where(q => q.TargetDateToIssueInvoice == targetDateToIssueInvoice);
                if (sumIns.HasValue) query = query.Where(q => q.SumIns == sumIns);
                if (!string.IsNullOrEmpty(CauseOfEvent)) query = query.Where(q => q.CauseOfEvent.Contains(CauseOfEvent));
                if (PicClaimId.HasValue) query = query.Where(q => q.PicClaimId == PicClaimId);
                if (PicDaaId.HasValue) query = query.Where(q => q.PicDaaId == PicDaaId);
                if (ClaimStatus.HasValue) query = query.Where(q => q.ClaimStatus == ClaimStatus);
                if (ProvisionStatus.HasValue) query = query.Where(q => q.ProvisionStatus == ProvisionStatus);
                if (OffsetStatus.HasValue) query = query.Where(q => q.OffsetStatus == OffsetStatus);
            }

            var export = new ExportFinanceProvisioningClaim();
            export.HandleTempDirectory();
            export.SetQuery(query);
            export.Process();

            return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));
        }

        public void IndexPage()
        {
            DropDownStatus();
            SetViewBagMessage();
        }

        public void LoadPage(FinanceProvisioningViewModel model)
        {
            DropDownCedantCode();
            DropDownPicClaim();
            DropDownPicDaa();
            DropDownClaimTransactionType(true, false, true);
            DropDownYesNoWithSelect();
            DropDownClaimStatus();
            DropDownProvisionStatus();
            DropDownOffsetStatus();

            if (model.Id != 0)
            {
                IsEnableSubmitForProcessing(model);
            }

            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= FinanceProvisioningBo.MaxStatus; i++)
            {
                items.Add(new SelectListItem { Text = FinanceProvisioningBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.StatusList = items;
            return items;
        }

        public ActionResult DownloadE3(int id, DateTime date)
        {
            string path = Util.GetE3Path(id.ToString());
            string dateStr = date.ToString("yyyyMMdd");
            if (Directory.Exists(path))
            {
                try
                {
                    string zipFileName = string.Format("E3_{0}.zip", dateStr);

                    Response.ClearContent();
                    Response.Clear();
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipFileName);

                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AddSelectedFiles("*", path, "", false);
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
            }
            return null;
        }

        public ActionResult DownloadE4(int id, DateTime date)
        {
            string path = Util.GetE4Path(id.ToString());
            string dateStr = date.ToString("yyyyMMdd");
            if (Directory.Exists(path))
            {
                try
                {
                    string zipFileName = string.Format("E4_{0}.zip", dateStr);

                    Response.ClearContent();
                    Response.Clear();
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipFileName);

                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AddSelectedFiles("*", path, "", false);
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
            }
            return null;
        }

        public void IsEnableSubmitForProcessing(FinanceProvisioningViewModel model)
        {
            var isEnable = false;
            if (model.Status == FinanceProvisioningBo.StatusFailed)
            {
                isEnable = true;
            }

            ViewBag.IsEnableSubmitForProcessing = isEnable;
        }
    }
}