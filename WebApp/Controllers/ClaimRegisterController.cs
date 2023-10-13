using BusinessObject;
using BusinessObject.Claims;
using BusinessObject.Identity;
using BusinessObject.RiDatas;
using ConsoleApp.Commands.ProcessDatas;
using ConsoleApp.Commands.RawFiles.ClaimRegister;
using Newtonsoft.Json;
using PagedList;
using Services;
using Services.Claims;
using Services.Identity;
using Services.RiDatas;
using Shared;
using System;
using System.Collections;
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
    public class ClaimRegisterController : BaseController
    {
        public const string Controller = "ClaimRegister";

        // GET: ClaimRegister
        [Auth(Controller = Controller, Power = "R")]
        public virtual ActionResult Index(
            string EntryNo,
            string SoaQuarter,
            string ClaimId,
            string ClaimTransactionType,
            string IsReferralCase,
            int? RiDataWarehouseId,
            string RecordType,
            int? TreatyCodeId,
            string PolicyNumber,
            int? CedantId,
            string InsuredName,
            string InsuredDateOfBirth,
            string LastTransactionDate,
            string DateOfReported,
            string CedantDateOfNotification,
            string DateOfRegister,
            string DateOfCommencement,
            string DateOfEvent,
            int? PolicyDuration,
            string TargetDateToIssueInvoice,
            string ClaimRecoveryAmt,
            string CauseOfEvent,
            int? PicClaimId,
            int? PicDaaId,
            int? ClaimStatus,
            int? DuplicationCheckStatus,
            int? ProvisionStatus,
            int? OffsetStatus,
            string SortOrder,
            int? Page,
            bool BatchSelection = false,
            string SelectedIds = null)
        {
            DateTime? insuredDateOfBirth = Util.GetParseDateTime(InsuredDateOfBirth);
            DateTime? lastTransactionDate = Util.GetParseDateTime(LastTransactionDate);
            DateTime? dateOfReported = Util.GetParseDateTime(DateOfReported);
            DateTime? cedantDateOfNotification = Util.GetParseDateTime(CedantDateOfNotification);
            DateTime? dateOfRegister = Util.GetParseDateTime(DateOfRegister);
            DateTime? dateOfCommencement = Util.GetParseDateTime(DateOfCommencement);
            DateTime? dateOfEvent = Util.GetParseDateTime(DateOfEvent);
            DateTime? targetDateToIssueInvoice = Util.GetParseDateTime(TargetDateToIssueInvoice);

            double? claimRecoveryAmt = Util.StringToDouble(ClaimRecoveryAmt);

            bool? isReferralCase = null;
            if (!string.IsNullOrEmpty(IsReferralCase))
            {
                if (Util.StringToBool(IsReferralCase, out bool b))
                {
                    isReferralCase = b;
                }
            }

            ViewBag.BatchSelection = BatchSelection;
            ViewBag.SelectedIds = SelectedIds;

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["EntryNo"] = EntryNo,
                ["SoaQuarter"] = SoaQuarter,
                ["ClaimId"] = ClaimId,
                ["ClaimTransactionType"] = ClaimTransactionType,
                ["IsReferralCase"] = IsReferralCase,
                ["RiDataWarehouseId"] = RiDataWarehouseId,
                ["RecordType"] = RecordType,
                ["TreatyCodeId"] = TreatyCodeId,
                ["PolicyNumber"] = PolicyNumber,
                ["CedantId"] = CedantId,
                ["InsuredName"] = InsuredName,
                ["InsuredDateOfBirth"] = insuredDateOfBirth.HasValue ? InsuredDateOfBirth : null,
                ["LastTransactionDate"] = lastTransactionDate.HasValue ? LastTransactionDate : null,
                ["DateOfReported"] = dateOfReported.HasValue ? DateOfReported : null,
                ["CedantDateOfNotification"] = cedantDateOfNotification.HasValue ? CedantDateOfNotification : null,
                ["DateOfRegister"] = dateOfRegister.HasValue ? DateOfRegister : null,
                ["DateOfCommencement"] = dateOfCommencement.HasValue ? DateOfCommencement : null,
                ["DateOfEvent"] = dateOfEvent.HasValue ? DateOfEvent : null,
                ["PolicyDuration"] = PolicyDuration,
                ["TargetDateToIssueInvoice"] = targetDateToIssueInvoice.HasValue ? TargetDateToIssueInvoice : null,
                ["SumIns"] = claimRecoveryAmt.HasValue ? ClaimRecoveryAmt : null,
                ["CauseOfEvent"] = CauseOfEvent,
                ["PicClaimId"] = PicClaimId,
                ["PicDaaId"] = PicDaaId,
                ["ClaimStatus"] = ClaimStatus,
                ["DuplicationCheckStatus"] = DuplicationCheckStatus,
                ["ProvisionStatus"] = ProvisionStatus,
                ["OffsetStatus"] = OffsetStatus,
                ["SortOrder"] = SortOrder,
            };

            ViewBag.SortOrder = SortOrder;
            ViewBag.SortEntryNo = GetSortParam("EntryNo");
            ViewBag.SortSoaQuarter = GetSortParam("SoaQuarter");
            ViewBag.SortClaimId = GetSortParam("ClaimId");
            ViewBag.SortClaimTransactionType = GetSortParam("ClaimTransactionType");
            ViewBag.SortIsReferralCase = GetSortParam("IsReferralCase");
            ViewBag.SortRiDataWarehouseId = GetSortParam("RiDataWarehouseId");
            ViewBag.SortRecordType = GetSortParam("RecordType");
            ViewBag.SortTreatyCodeId = GetSortParam("TreatyCodeId");
            ViewBag.SortPolicyNumber = GetSortParam("PolicyNumber");
            ViewBag.SortCedantId = GetSortParam("CedantId");
            ViewBag.SortInsuredName = GetSortParam("InsuredName");
            ViewBag.SortInsuredDateOfBirth = GetSortParam("InsuredDateOfBirth");
            ViewBag.SortLastTransactionDate = GetSortParam("LastTransactionDate");
            ViewBag.SortDateOfReported = GetSortParam("DateOfReported");
            ViewBag.SortCedantDateOfNotification = GetSortParam("CedantDateOfNotification");
            ViewBag.SortDateOfRegister = GetSortParam("DateOfRegister");
            ViewBag.SortDateOfCommencement = GetSortParam("DateOfCommencement"); // Date of Commencement
            ViewBag.SortDateOfEvent = GetSortParam("DateOfEvent");
            ViewBag.SortPolicyDuration = GetSortParam("PolicyDuration");
            ViewBag.SortTargetDateToIssueInvoice = GetSortParam("TargetDateToIssueInvoice");
            ViewBag.SortClaimRecoveryAmt = GetSortParam("ClaimRecoveryAmt");
            ViewBag.SortCauseOfEvent = GetSortParam("CauseOfEvent");
            ViewBag.SortPicClaimId = GetSortParam("PicClaimId");
            ViewBag.SortPicDaaId = GetSortParam("PicDaaId");
            ViewBag.SortClaimStatus = GetSortParam("ClaimStatus");
            ViewBag.SortDuplicationCheckStatus = GetSortParam("DuplicationCheckStatus");
            ViewBag.SortProvisionStatus = GetSortParam("ProvisionStatus");
            ViewBag.SortOffsetStatus = GetSortParam("OffsetStatus");

            var query = GetQuery();
            if (!string.IsNullOrEmpty(EntryNo)) query = query.Where(q => q.EntryNo.Contains(EntryNo));
            if (!string.IsNullOrEmpty(SoaQuarter)) query = query.Where(q => q.SoaQuarter == SoaQuarter);
            if (!string.IsNullOrEmpty(ClaimId)) query = query.Where(q => q.ClaimId.Contains(ClaimId));
            if (!string.IsNullOrEmpty(ClaimTransactionType)) query = query.Where(q => q.ClaimTransactionType == ClaimTransactionType);
            if (isReferralCase.HasValue) query = query.Where(q => q.IsReferralCase == isReferralCase);
            if (RiDataWarehouseId.HasValue) query = query.Where(q => q.RiDataWarehouseId == RiDataWarehouseId);
            if (!string.IsNullOrEmpty(RecordType)) query = query.Where(q => q.RecordType.Contains(RecordType));
            if (TreatyCodeId.HasValue)
            {
                string treatyCode = TreatyCodeService.Find(TreatyCodeId)?.Code;
                query = query.Where(q => q.TreatyCode == treatyCode);
            }
            if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber.Contains(PolicyNumber));
            if (CedantId.HasValue)
            {
                string cedingCompany = CedantService.Find(CedantId)?.Code;
                query = query.Where(q => q.CedingCompany == cedingCompany);
            }
            if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => q.InsuredName.Contains(InsuredName));
            if (insuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == insuredDateOfBirth);
            if (lastTransactionDate.HasValue) query = query.Where(q => q.LastTransactionDate == lastTransactionDate);
            if (dateOfReported.HasValue) query = query.Where(q => q.DateOfReported == dateOfReported);
            if (cedantDateOfNotification.HasValue) query = query.Where(q => q.CedantDateOfNotification == cedantDateOfNotification);
            if (dateOfRegister.HasValue) query = query.Where(q => q.DateOfRegister == dateOfRegister);
            if (dateOfCommencement.HasValue) query = query.Where(q => q.ReinsEffDatePol == dateOfCommencement);
            if (dateOfEvent.HasValue) query = query.Where(q => q.DateOfEvent == dateOfEvent);
            if (PolicyDuration.HasValue) query = query.Where(q => q.PolicyDuration == PolicyDuration);
            if (targetDateToIssueInvoice.HasValue) query = query.Where(q => q.TargetDateToIssueInvoice == targetDateToIssueInvoice);
            if (claimRecoveryAmt.HasValue) query = query.Where(q => q.ClaimRecoveryAmt == claimRecoveryAmt);
            if (!string.IsNullOrEmpty(CauseOfEvent)) query = query.Where(q => q.CauseOfEvent.Contains(CauseOfEvent));
            if (PicClaimId.HasValue)
            {
                if (PicClaimId.Value == 0)
                {
                    query = query.Where(q => !q.PicClaimId.HasValue);
                }
                else
                {
                    query = query.Where(q => q.PicClaimId == PicClaimId);
                }
            }
            else if (BatchSelection)
            {
                if (!UserService.CheckPowerFlag(AuthUser, ModuleBo.ModuleController.ClaimRegisterClaim.ToString(), AccessMatrixBo.PowerAssignClaim))
                {
                    query = query.Where(q => !q.PicClaimId.HasValue || q.PicClaimId == AuthUserId);
                }
            }
            if (PicDaaId.HasValue) query = query.Where(q => q.PicDaaId == PicDaaId);
            if (ClaimStatus.HasValue) query = query.Where(q => q.ClaimStatus == ClaimStatus);
            if (DuplicationCheckStatus.HasValue) query = query.Where(q => q.DuplicationCheckStatus == DuplicationCheckStatus);
            if (ProvisionStatus.HasValue) query = query.Where(q => q.ProvisionStatus == ProvisionStatus);
            if (OffsetStatus.HasValue) query = query.Where(q => q.OffsetStatus == OffsetStatus);

            if (SortOrder == Html.GetSortAsc("EntryNo")) query = query.OrderBy(q => q.EntryNo);
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
            else if (SortOrder == Html.GetSortAsc("TreatyCodeId")) query = query.OrderBy(q => q.TreatyCode);
            else if (SortOrder == Html.GetSortDsc("TreatyCodeId")) query = query.OrderByDescending(q => q.TreatyCode);
            else if (SortOrder == Html.GetSortAsc("PolicyNumber")) query = query.OrderBy(q => q.PolicyNumber);
            else if (SortOrder == Html.GetSortDsc("PolicyNumber")) query = query.OrderByDescending(q => q.PolicyNumber);
            else if (SortOrder == Html.GetSortAsc("CedantId")) query = query.OrderBy(q => q.CedingCompany);
            else if (SortOrder == Html.GetSortDsc("CedantId")) query = query.OrderByDescending(q => q.CedingCompany);
            else if (SortOrder == Html.GetSortAsc("InsuredName")) query = query.OrderBy(q => q.InsuredName);
            else if (SortOrder == Html.GetSortDsc("InsuredName")) query = query.OrderByDescending(q => q.InsuredName);
            else if (SortOrder == Html.GetSortAsc("InsuredDateOfBirth")) query = query.OrderBy(q => q.InsuredDateOfBirth);
            else if (SortOrder == Html.GetSortDsc("InsuredDateOfBirth")) query = query.OrderByDescending(q => q.InsuredDateOfBirth);
            else if (SortOrder == Html.GetSortAsc("LastTransactionDate")) query = query.OrderBy(q => q.LastTransactionDate);
            else if (SortOrder == Html.GetSortDsc("LastTransactionDate")) query = query.OrderByDescending(q => q.LastTransactionDate);
            else if (SortOrder == Html.GetSortAsc("DateOfReported")) query = query.OrderBy(q => q.DateOfReported);
            else if (SortOrder == Html.GetSortDsc("DateOfReported")) query = query.OrderByDescending(q => q.DateOfReported);
            else if (SortOrder == Html.GetSortAsc("CedantDateOfNotification")) query = query.OrderBy(q => q.CedantDateOfNotification);
            else if (SortOrder == Html.GetSortDsc("CedantDateOfNotification")) query = query.OrderByDescending(q => q.CedantDateOfNotification);
            else if (SortOrder == Html.GetSortAsc("DateOfRegister")) query = query.OrderBy(q => q.DateOfRegister);
            else if (SortOrder == Html.GetSortDsc("DateOfRegister")) query = query.OrderByDescending(q => q.DateOfRegister);
            else if (SortOrder == Html.GetSortAsc("DateOfCommencement")) query = query.OrderBy(q => q.ReinsEffDatePol);
            else if (SortOrder == Html.GetSortDsc("DateOfCommencement")) query = query.OrderByDescending(q => q.ReinsEffDatePol);
            else if (SortOrder == Html.GetSortAsc("DateOfEvent")) query = query.OrderBy(q => q.DateOfEvent);
            else if (SortOrder == Html.GetSortDsc("DateOfEvent")) query = query.OrderByDescending(q => q.DateOfEvent);
            else if (SortOrder == Html.GetSortAsc("PolicyDuration")) query = query.OrderBy(q => q.PolicyDuration);
            else if (SortOrder == Html.GetSortDsc("PolicyDuration")) query = query.OrderByDescending(q => q.PolicyDuration);
            else if (SortOrder == Html.GetSortAsc("TargetDateToIssueInvoice")) query = query.OrderBy(q => q.TargetDateToIssueInvoice);
            else if (SortOrder == Html.GetSortDsc("TargetDateToIssueInvoice")) query = query.OrderByDescending(q => q.TargetDateToIssueInvoice);
            else if (SortOrder == Html.GetSortAsc("ClaimRecoveryAmt")) query = query.OrderBy(q => q.ClaimRecoveryAmt);
            else if (SortOrder == Html.GetSortDsc("ClaimRecoveryAmt")) query = query.OrderByDescending(q => q.ClaimRecoveryAmt);
            else if (SortOrder == Html.GetSortAsc("CauseOfEvent")) query = query.OrderBy(q => q.CauseOfEvent);
            else if (SortOrder == Html.GetSortDsc("CauseOfEvent")) query = query.OrderByDescending(q => q.CauseOfEvent);
            else if (SortOrder == Html.GetSortAsc("PicClaimId")) query = query.OrderBy(q => q.PicClaim.FullName);
            else if (SortOrder == Html.GetSortDsc("PicClaimId")) query = query.OrderByDescending(q => q.PicClaim.FullName);
            else if (SortOrder == Html.GetSortAsc("PicDaaId")) query = query.OrderBy(q => q.PicDaa.FullName);
            else if (SortOrder == Html.GetSortDsc("PicDaaId")) query = query.OrderByDescending(q => q.PicDaa.FullName);
            else if (SortOrder == Html.GetSortAsc("ClaimStatus")) query = query.OrderBy(q => q.ClaimStatus);
            else if (SortOrder == Html.GetSortDsc("ClaimStatus")) query = query.OrderByDescending(q => q.ClaimStatus);
            else if (SortOrder == Html.GetSortAsc("DuplicationCheckStatus")) query = query.OrderBy(q => q.DuplicationCheckStatus);
            else if (SortOrder == Html.GetSortDsc("DuplicationCheckStatus")) query = query.OrderByDescending(q => q.DuplicationCheckStatus);
            else if (SortOrder == Html.GetSortAsc("ProvisionStatus")) query = query.OrderBy(q => q.ProvisionStatus);
            else if (SortOrder == Html.GetSortDsc("ProvisionStatus")) query = query.OrderByDescending(q => q.ProvisionStatus);
            else if (SortOrder == Html.GetSortAsc("OffsetStatus")) query = query.OrderBy(q => q.OffsetStatus);
            else if (SortOrder == Html.GetSortDsc("OffsetStatus")) query = query.OrderByDescending(q => q.OffsetStatus);
            else query = query.OrderByDescending(q => q.Id);

            ViewBag.Total = query.Count();
            IndexPage();
            return View("Index", query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: ClaimRegister/Create
        [Auth(Controller = Controller, Power = "C")]
        public virtual ActionResult Create(int? id = null)
        {
            if (CheckCutOffReadOnly(Controller))
                return RedirectToAction("Index");

            var model = new ClaimRegisterViewModel();
            if (id.HasValue)
            {
                var bo = ClaimRegisterService.Find(id);
                if (bo == null)
                {
                    return RedirectToAction("Index");
                }

                bo.ClaimRecoveryAmt *= -1;
                model = new ClaimRegisterViewModel(bo);
                model.Id = 0;
                model.EntryNo = null;
                model.SoaQuarter = null;
                model.ClaimTransactionType = PickListDetailBo.ClaimTransactionTypeAdjustment;
                model.OriginalClaimRegisterId = bo.Id;
                model.DateOfReported = DateTime.Today;
            }

            LoadPage(ref model);
            return View(model);
        }

        // GET: ClaimRegister/Create
        [Auth(Controller = Controller, Power = "C")]
        public virtual ActionResult CreateAdjustment(int id)
        {
            if (CheckCutOffReadOnly(Controller))
                return RedirectToAction("Index");

            var bo = ClaimRegisterService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var model = new ClaimRegisterViewModel(bo);
            model.Id = 0;
            model.EntryNo = null;
            model.SoaQuarter = null;
            model.ClaimTransactionType = PickListDetailBo.ClaimTransactionTypeAdjustment;
            model.OriginalClaimRegisterId = bo.Id;

            LoadPage(ref model);
            return View("Create", model);
        }

        // POST: ClaimRegister/Create
        [HttpPost]
        public virtual ActionResult Create(FormCollection form, ClaimRegisterViewModel model)
        {
            if (ModelState.IsValid && !CheckCutOffReadOnly(Controller))
            {
                var bo = model.FormBo(AuthUserId);

                if (!string.IsNullOrEmpty(bo.ClaimCode))
                    bo.Checklist = ClaimChecklistDetailService.GetJsonByClaimCode(bo.ClaimCode);

                bo.EntryNo = ClaimRegisterService.GetNextEntryNo();
                if (!string.IsNullOrEmpty(bo.TreatyCode) && string.IsNullOrEmpty(bo.TreatyType))
                {
                    var treatyCodeBo = TreatyCodeService.FindByCode(bo.TreatyCode);
                    bo.TreatyType = treatyCodeBo.TreatyStatusPickListDetailBo?.Code;
                }

                if (bo.OriginalClaimRegisterId.HasValue && bo.ClaimTransactionType == PickListDetailBo.ClaimTransactionTypeAdjustment)
                {
                    var originalClaimRegisterBo = ClaimRegisterService.Find(bo.OriginalClaimRegisterId);
                    bo.ClaimId = originalClaimRegisterBo?.ClaimId;
                }

                var trail = GetNewTrailObject();
                Result = ClaimRegisterService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;

                    if (bo.OriginalClaimRegisterId.HasValue && bo.ClaimTransactionType == PickListDetailBo.ClaimTransactionTypeAdjustment)
                    {
                        var originalClaimRegisterBo = ClaimRegisterService.Find(bo.OriginalClaimRegisterId);
                        originalClaimRegisterBo.RemoveRedFlagWarning(ClaimRegisterBo.RedFlagCreateAdjustment.ToString());
                        ClaimRegisterService.Update(ref originalClaimRegisterBo, ref trail);
                    }

                    ProvisionClaimRegister provisionClaimRegister = new ProvisionClaimRegister(bo, provisionDirectRetro: false);
                    provisionClaimRegister.Provision();

                    StatusHistoryController.Add(model.ModuleId, bo.Id, bo.ClaimStatus, AuthUserId, ref trail);

                    List<RemarkBo> remarkBos = RemarkController.GetRemarks(form, AuthUser.UserName);
                    RemarkController.SaveRemarks(remarkBos, model.ModuleId, model.Id, AuthUserId, ref trail, AuthUser.DepartmentId);

                    CreateTrail(
                        bo.Id,
                        "Create Claim Register"
                    );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage(ref model);
            return View(model);
        }

        // GET: ClaimRegister/Edit/5
        public virtual ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = ClaimRegisterService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            ClaimRegisterViewModel model = new ClaimRegisterViewModel(bo);
            LoadPage(ref model, bo);
            return View(model);
        }

        // POST: ClaimRegister/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public virtual ActionResult Edit(int id, FormCollection form, ClaimRegisterViewModel model)
        {
            var dbBo = ClaimRegisterService.Find(id);
            if (dbBo == null)
                return RedirectToAction("Index");

            if (dbBo.ClaimStatus == ClaimRegisterBo.StatusReported)
                ModelState.Remove("OriginalClaimRegisterId");
            if (ModelState.IsValid && !CheckCutOffReadOnly(Controller, id))
            {
                var bo = model.FormBo(AuthUserId, dbBo);

                //if (originalStatus == ClaimRegisterBo.StatusPendingDecline && bo.ClaimStatus == ClaimRegisterBo.StatusDeclined)
                //{
                //    if (bo.OffsetStatus == ClaimRegisterBo.OffsetStatusPendingInvoicing || bo.OffsetStatus == ClaimRegisterBo.OffsetStatusOffset)
                //        bo.AddRedFlagWarning(ClaimRegisterBo.RedFlagCreateAdjustment.ToString());
                //    else
                //        bo.ReverseClaim();
                //}
                //else 

                if (UpdateClaim(bo, model.ModuleId, dbBo))
                {
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
            }

            LoadPage(ref model, dbBo);
            return View("Edit", model);
        }

        public virtual ActionResult UpdateStatus(int id, int status, FormCollection form, ClaimRegisterViewModel model)
        {
            var dbBo = ClaimRegisterService.Find(id);
            if (dbBo == null)
                return RedirectToAction("Index");

            Dictionary<string, int> statuses = new Dictionary<string, int>();
            VerifyStatusUpdate(dbBo, status, ref statuses);

            if (status == ClaimRegisterBo.StatusReported || (status == ClaimRegisterBo.StatusSuspectedDuplication && dbBo.ClaimStatus == ClaimRegisterBo.StatusFailed))
                ModelState.Remove("OriginalClaimRegisterId");
            if (ModelState.IsValid && !CheckCutOffReadOnly(Controller, id))
            {
                var bo = model.FormBo(AuthUserId, dbBo);

                int originalStatus = dbBo.ClaimStatus;
                if (originalStatus == ClaimRegisterBo.StatusPendingClarification)
                {
                    switch (status)
                    {
                        case ClaimRegisterBo.StatusRegistered:
                            if (string.IsNullOrEmpty(bo.ClaimId))
                            {
                                bo.SetRegisteredValues();
                                if (bo.ClaimTransactionType == PickListDetailBo.ClaimTransactionTypeNew)
                                    bo.ClaimId = ClaimRegisterService.GetNextClaimId();
                            }
                            break;
                        case ClaimRegisterBo.StatusClosed:
                            bo.ReverseClaim();
                            break;
                        default:
                            break;
                    }
                }

                bo.ClaimStatus = status;
                if (UpdateClaim(bo, model.ModuleId, dbBo))
                {
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
            }

            LoadPage(ref model, dbBo);
            return View("Edit", model);
        }

        public bool VerifyStatusUpdate(ClaimRegisterBo bo, int nextStatus, ref Dictionary<string, int> statuses)
        {
            statuses = GetPossibleNextStatus(bo.ClaimStatus, bo.DuplicationCheckStatus, bo.RecordType, bo.IsReferralCase, bo.ClaimCode, bo.ClaimRecoveryAmt);
            if (!statuses.Values.Contains(nextStatus))
            {
                //SetErrorSessionMsg("Invalid Action");
                //return RedirectToAction("Edit", new { id });
                ModelState.AddModelError("", "Invalid Action");
                return false;
            }
            return true;
        }

        public ActionResult ReprocessProvisionClaim(int id, FormCollection form, ClaimRegisterViewModel model)
        {
            var dbBo = ClaimRegisterService.Find(id);
            if (dbBo == null)
                return RedirectToAction("Index");

            if (ModelState.IsValid && !CheckCutOffReadOnly(Controller, id))
            {
                var bo = model.FormBo(AuthUserId, dbBo);
                bo.ProvisionStatus = ClaimRegisterBo.ProvisionStatusPendingReprocess;
                if (UpdateClaim(bo, model.ModuleId, dbBo))
                {
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
            }

            LoadPage(ref model, dbBo);
            return View("Edit", model);
        }

        public ActionResult ProvisionDirectRetro(int id, FormCollection form, ClaimRegisterViewModel model)
        {
            var dbBo = ClaimRegisterService.Find(id);
            if (dbBo == null)
                return RedirectToAction("Index");

            if (ModelState.IsValid && !CheckCutOffReadOnly(Controller, id))
            {
                var bo = model.FormBo(AuthUserId, dbBo);
                bo.DrProvisionStatus = ClaimRegisterBo.DrProvisionStatusPending;
                if (UpdateClaim(bo, model.ModuleId, dbBo))
                {
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
            }

            LoadPage(ref model, dbBo);
            return View("Edit", model);
        }

        public ActionResult UnbulkClaim(int id, FormCollection form, ClaimRegisterViewModel model)
        {
            var dbBo = ClaimRegisterService.Find(id);
            if (dbBo == null)
                return RedirectToAction("Index");

            if (dbBo.ClaimTransactionType != PickListDetailBo.ClaimTransactionTypeBulk)
                ModelState.AddModelError("", "Invalid Action");

            if (ModelState.IsValid && !CheckCutOffReadOnly(Controller, id))
            {
                var bo = model.FormBo(AuthUserId, dbBo);

                bo.ClaimStatus = ClaimRegisterBo.StatusClosed;
                bo.ReverseClaim();

                if (UpdateClaim(bo, model.ModuleId, dbBo))
                {
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
            }

            LoadPage(ref model, dbBo);
            return View("Edit", model);
        }

        public ActionResult ZeroriseClaim(int id, FormCollection form, ClaimRegisterViewModel model)
        {
            var dbBo = ClaimRegisterService.Find(id);
            if (dbBo == null)
                return RedirectToAction("Index");

            if (dbBo.ClaimStatus != ClaimRegisterBo.StatusDeclined || dbBo.OffsetStatus != ClaimRegisterBo.OffsetStatusPending)
                ModelState.AddModelError("", "Invalid Action");

            if (ModelState.IsValid && !CheckCutOffReadOnly(Controller, id))
            {
                var bo = model.FormBo(AuthUserId, dbBo);

                //bo.ClaimStatus = ClaimRegisterBo.StatusClosed;
                //bo.SoaDataBatchId = null;
                bo.SoaQuarter = null;
                bo.OffsetStatus = ClaimRegisterBo.OffsetStatusNotRequired;
                bo.ReverseClaim();

                if (UpdateClaim(bo, model.ModuleId, dbBo))
                {
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
            }

            LoadPage(ref model, dbBo);
            return View("Edit", model);
        }

        public bool UpdateClaim(ClaimRegisterBo bo, int moduleId, ClaimRegisterBo dbBo = null, bool preventResetStatus = false)
        {
            if (dbBo == null)
                dbBo = ClaimRegisterService.Find(bo.Id);

            if (bo.RiDataWarehouseId.HasValue && bo.ReferralRiDataId.HasValue)
                bo.ReferralRiDataId = null;

            if (bo.LastTransactionDate.HasValue)
            {
                bo.LastTransactionQuarter = Util.GetQuarterFromDate(bo.LastTransactionDate.Value);
            }

            bool provision = FinanceProvisioningTransactionService.ToProvision(bo);
            if (bo.ProvisionStatus == ClaimRegisterBo.ProvisionStatusProvisioned || bo.ProvisionStatus == ClaimRegisterBo.ProvisionStatusProvisioning)
            {
                if (provision)
                {
                    bo.ProvisionStatus = ClaimRegisterBo.ProvisionStatusPending;
                }
                else if (!provision)
                {
                    if (FinanceProvisioningTransactionService.ToProvision(bo))
                    {
                        provision = true;
                        bo.ProvisionStatus = ClaimRegisterBo.ProvisionStatusPending;
                    }
                }

                if (bo.RiDataWarehouseId != dbBo.RiDataWarehouseId && ClaimRegisterBo.GetClaimDepartmentStatus(false).Contains(bo.ClaimStatus) && !preventResetStatus)
                {
                    bo.ClaimStatus = ClaimRegisterBo.StatusRegistered;
                }
            }
            else if (ClaimRegisterBo.GetClaimDepartmentStatus(false).Contains(bo.ClaimStatus) && bo.SoaQuarter == dbBo.SoaQuarter && !preventResetStatus)
            {
                bo.ClaimStatus = ClaimRegisterBo.StatusRegistered;
            }

            if (provision)
            {
                if (bo.DrProvisionStatus != ClaimRegisterBo.DrProvisionStatusPending)
                    bo.DrProvisionStatus = ClaimRegisterBo.DrProvisionStatusPending;

                if (bo.ReinsEffDatePol.HasValue)
                    bo.Mfrs17AnnualCohort = bo.ReinsEffDatePol.Value.Year;

                if (bo.DateOfEvent.HasValue)
                {
                    bo.RiskQuarter = Util.GetQuarterFromDate(bo.DateOfEvent.Value);
                    bo.RiskPeriodMonth = bo.DateOfEvent.Value.Month;
                    bo.RiskPeriodYear = bo.DateOfEvent.Value.Year;
                }

                //if (bo.ClaimTransactionType != ClaimRegisterBo.ClaimTransactionTypeAdjName && bo.ClaimTransactionType == ClaimRegisterBo.ClaimTransactionTypeNewName)
                //    bo.ClaimTransactionType = ClaimRegisterBo.ClaimTransactionTypeAdjName;

                // MLRe Retain Amount computation
                var directRetroTreaty = DirectRetroConfigurationService.FindByTreatyCode(bo.TreatyCode, false);

                var claimRecoveryAmt = bo.ClaimRecoveryAmt.HasValue ? bo.ClaimRecoveryAmt : 0;
                var retroRecovery1 = bo.RetroRecovery1.HasValue ? bo.RetroRecovery1 : 0;
                var retroRecovery2 = bo.RetroRecovery2.HasValue ? bo.RetroRecovery2 : 0;
                var retroRecovery3 = bo.RetroRecovery3.HasValue ? bo.RetroRecovery3 : 0;

                if (directRetroTreaty != null)
                {
                    bo.MlreRetainAmount = claimRecoveryAmt - retroRecovery1 - retroRecovery2 - retroRecovery3;
                }
                else
                {
                    bo.MlreRetainAmount = claimRecoveryAmt;
                }

                bo.LastTransactionDate = DateTime.Now;
                bo.LastTransactionQuarter = Util.GetCurrentQuarter();
            }

            bo.ReValidateRedFlagWarnings();
            if (dbBo.ClaimStatus != ClaimRegisterBo.StatusReported && bo.DuplicateParamsChanged(dbBo))
            {
                bo.DuplicationCheckStatus = ClaimRegisterService.HasDuplicate(bo) ? ClaimRegisterBo.DuplicationCheckStatusHasDuplicate : ClaimRegisterBo.DuplicationCheckStatusNoDuplicate;
            }

            ClaimRegisterBo originalClaimRegisterBo = null;
            if (bo.OriginalClaimRegisterId.HasValue && bo.ClaimTransactionType == PickListDetailBo.ClaimTransactionTypeAdjustment && (bo.OriginalClaimRegisterId != dbBo.OriginalClaimRegisterId || string.IsNullOrEmpty(bo.ClaimId)))
            {
                originalClaimRegisterBo = ClaimRegisterService.Find(bo.OriginalClaimRegisterId);
                bo.ClaimId = originalClaimRegisterBo.ClaimId;
            }

            var trail = GetNewTrailObject();
            Result = ClaimRegisterService.Update(ref bo, ref trail);
            if (Result.Valid)
            {
                if (originalClaimRegisterBo != null)
                {
                    originalClaimRegisterBo.RemoveRedFlagWarning(ClaimRegisterBo.RedFlagCreateAdjustment.ToString());
                    ClaimRegisterService.Update(ref originalClaimRegisterBo, ref trail);
                }

                if (provision)
                {
                    ProvisionClaimRegister provisionClaimRegister = new ProvisionClaimRegister(bo, provisionDirectRetro: false);
                    provisionClaimRegister.Provision();
                }

                StatusHistoryController.Add(moduleId, bo.Id, bo.ClaimStatus, AuthUserId, ref trail);

                CreateTrail(
                    bo.Id,
                    "Update Claim Register"
                );

                SetUpdateSuccessMessage(Controller);
                return true;
            }
            AddResult(Result);
            return false;
        }

        // GET: ClaimRegister/Delete/5
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = ClaimRegisterService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            return View(new ClaimRegisterViewModel(bo));
        }

        // POST: ClaimRegister/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id, ClaimRegisterViewModel model)
        {
            var bo = ClaimRegisterService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = ClaimRegisterService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Claim Register"
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

        public ActionResult Download(
            string downloadToken,
            int type,
            string EntryNo,
            string SoaQuarter,
            string ClaimId,
            int? ClaimTransactionType,
            bool? IsReferralCase,
            int? RiDataWarehouseId,
            string RecordType,
            int? TreatyCodeId,
            string PolicyNumber,
            int? CedantId,
            string InsuredName,
            string InsuredDateOfBirth,
            string LastTransactionDate,
            string DateOfReported,
            string CedantDateOfNotification,
            string DateOfRegister,
            string DateOfCommencement,
            string DateOfEvent,
            int? PolicyDuration,
            string TargetDateToIssueInvoice,
            string ClaimRecoveryAmt,
            string CauseOfEvent,
            int? PicClaimId,
            int? PicDaaId,
            int? ClaimStatus,
            int? DuplicationCheckStatus,
            int? ProvisionStatus,
            int? OffsetStatus)
        {
            // type 1 = all
            // type 2 = filtered download

            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            dynamic Params = new ExpandoObject();
            Params.EntryNo = EntryNo;
            Params.SoaQuarter = SoaQuarter;
            Params.ClaimId = ClaimId;
            Params.ClaimTransactionType = ClaimTransactionType;
            Params.IsReferralCase = IsReferralCase;
            Params.RiDataWarehouseId = RiDataWarehouseId;
            Params.RecordType = RecordType;
            if (TreatyCodeId != null)
                Params.TreatyCode = TreatyCodeService.Find(TreatyCodeId)?.Code;
            Params.PolicyNumber = PolicyNumber;
            if (CedantId != null)
                Params.CedingCompany = CedantService.Find(CedantId)?.Code;
            Params.InsuredName = InsuredName;
            Params.InsuredDateOfBirth = InsuredDateOfBirth;
            Params.LastTransactionDate = LastTransactionDate;
            Params.DateOfReported = DateOfReported;
            Params.CedantDateOfNotification = CedantDateOfNotification;
            Params.DateOfRegister = DateOfRegister;
            Params.DateOfCommencement = DateOfCommencement;
            Params.DateOfEvent = DateOfEvent;
            Params.PolicyDuration = PolicyDuration;
            Params.TargetDateToIssueInvoice = TargetDateToIssueInvoice;
            Params.ClaimRecoveryAmt = ClaimRecoveryAmt;
            Params.CauseOfEvent = CauseOfEvent;
            Params.PicClaimId = PicClaimId;
            Params.PicDaaId = PicDaaId;
            Params.ClaimStatus = ClaimStatus;
            Params.DuplicationCheckStatus = DuplicationCheckStatus;
            Params.ProvisionStatus = ProvisionStatus;
            Params.OffsetStatus = OffsetStatus;
            Params.IsClaimOnly = IsClaimOnly();
            var export = new GenerateExportData();
            ExportBo exportBo = export.CreateExportClaimRegister(AuthUserId, Params);

            if (exportBo.Total <= export.ExportInstantDownloadLimit)
            {
                export.Process(ref exportBo, _db);
                Session.Add("DownloadExport", true);
            }

            return RedirectToAction("Edit", "Export", new { exportBo.Id });
        }

        [Auth(Controller = Controller, Power = AccessMatrixBo.PowerUpload, ReturnController = Controller)]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            //if (!CheckPower(Controller, AccessMatrixBo.PowerApprovalDirectRetro))
            //    Result.AddError("You dont have access to Approve or Reject Direct Retro");

            if (ModelState.IsValid && !CheckCutOffReadOnly(Controller))
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var process = new ProcessClaimRegister()
                    {
                        PostedFile = upload,
                        AuthUserId = AuthUserId,
                    };
                    process.Process();

                    if (process.Errors.Count() > 0 && process.Errors != null)
                    {
                        SetErrorSessionMsgArr(process.Errors.Take(50).ToList());
                    }

                    int update = process.GetProcessCount("Update");

                    SetSuccessSessionMsg(string.Format("Process File Successful, Total Update: {0}", update));
                }
            }

            return RedirectToAction("Index");
        }

        // Load Data
        public virtual void IndexPage()
        {
            CheckCutOffReadOnly(Controller);

            DropDownEmpty();
            ViewBag.TreatyCodes = TreatyCodeService.Get();
            DropDownCedant();
            DropDownClaimTransactionType(true);
            DropDownYesNo(true);
            ViewBag.DropDownAssignPic = DropDownPicClaim();
            DropDownPicClaim(hasUnassignedOption: true);
            DropDownPicDaa(hasUnassignedOption: true);
            DropDownClaimStatus();
            DropDownDuplicationCheckStatus();
            DropDownProvisionStatus();
            DropDownOffsetStatus();
            SetViewBagMessage();
        }

        public void LoadPage(ref ClaimRegisterViewModel model, ClaimRegisterBo bo = null)
        {
            LoadShared(ref model, true, bo);

            DropDownReinsBasisCode(true);
            if (bo == null)
            {
                DropDownClaimTransactionType(true);
            }
            else if (bo.ClaimTransactionType != PickListDetailBo.ClaimTransactionTypeBulk)
            {
                DropDownClaimTransactionType(true, removeBulk: true, displaySelect: false);
            }
            DropDownTreatyType(true);
            DropDownTreatyCode(codeAsValue: true);
            DropDownCedingBenefitTypeCode(true);
            DropDownFundsAccountingTypeCode(true);
            DropDownCurrencyCode(true);
            DropDownCedantCode(CedantBo.StatusActive, model.CedingCompany, true);
            DropDownEventCode(EventCodeBo.StatusActive, selectedCode: model.MlreEventCode, codeAsValue: true);
            DropDownBenefit(BenefitBo.StatusActive, selectedCode: model.MlreBenefitCode, codeAsValue: true);
            DropDownPicDaa(UserBo.StatusActive, model.PicDaaId);
            DropDownBusinessOrigin(true);

            // To be Moved
            //ViewBag.DropDownClaimUsers = DropDownPicClaim(UserBo.StatusActive);
            //DropDownClaimAssessor(UserBo.StatusActive, model.ClaimAssessorId);
            //DropDownClaimReason(ClaimReasonBo.TypeClaimDeclinePending);
            //DropDownPicClaim(UserBo.StatusActive, model.PicClaimId);

            if (model.Id != 0)
            {
                if (model.PicDaaBo != null && model.PicDaaBo.Status == UserBo.StatusSuspend)
                {
                    AddWarningMsg("Person In-Charge (DA&A) is suspended");
                }

                if ((model.ClaimStatus == ClaimRegisterBo.StatusReported || model.ClaimStatus == ClaimRegisterBo.StatusFailed) && model.ClaimDataConfigBo != null)
                {
                    if (model.ClaimDataConfigBo.Status != ClaimDataConfigBo.StatusApproved)
                    {
                        AddWarningMsg(string.Format(MessageBag.ClaimDataConfigStatusInactive, ClaimDataConfigBo.GetStatusName(model.ClaimDataConfigBo.Status)));
                    }
                }

                if (ViewBag.ReadOnly == null || !ViewBag.ReadOnly)
                {
                    ViewBag.ReadOnly = false;
                    if ((model.OffsetStatus != ClaimRegisterBo.OffsetStatusPending) && (bo.RiDataWarehouseId.HasValue && bo.RiDataWarehouseId != 0))
                    {
                        ViewBag.ReadOnly = true;
                    }
                }

                if (!bo.RiDataWarehouseId.HasValue)
                {
                    ViewBag.EnableSaveOnly = true;

                    if (model.OffsetStatus == ClaimRegisterBo.OffsetStatusPending)
                        ViewBag.EnableSaveOnly = false;
                }

                if (!string.IsNullOrEmpty(model.Errors))
                {
                    var errors = JsonConvert.DeserializeObject<Dictionary<string, object>>(model.Errors);
                    foreach (var error in errors)
                    {
                        if (error.Value is string @string)
                        {
                            model.SetPropertyValue(error.Key, @string);
                        }
                        else if (error.Value is IEnumerable errorArr)
                        {
                            List<string> s = new List<string> { };
                            foreach (var e in errorArr)
                            {
                                if (e is string @str)
                                    s.Add(@str);
                                else if (e is Newtonsoft.Json.Linq.JValue @jvalue)
                                    s.Add(@jvalue.ToString());
                            }
                            model.SetPropertyValue(error.Key, string.Join("\n", s.ToArray()));
                        }
                    }
                }
            }

            SetViewBagMessage();
        }

        public void LoadShared(ref ClaimRegisterViewModel model, bool genActions = true, ClaimRegisterBo bo = null)
        {
            AuthUserName();
            int moduleId = model.ModuleId;
            int id = model.Id;
            int? departmentId = AuthUser.DepartmentId;

            ViewBag.StandardOutputList = StandardOutputService.Get();
            if (id != 0)
            {
                if (model.ClaimAssessorBo != null && model.ClaimAssessorBo.Status == UserBo.StatusSuspend)
                {
                    AddWarningMsg("Claim Assessor suspended");
                }

                if (bo != null && model.DuplicationCheckStatus == ClaimRegisterBo.DuplicationCheckStatusHasDuplicate)
                {
                    ViewBag.DuplicateClaimRegisterBos = ClaimRegisterService.GetDuplicate(bo);
                }
            }

            ViewBag.CanEdit = CheckCutOffReadOnly(Controller, bo?.Id);

            // Shared Drop Downs
            DropDownEmpty();
            DropDownMonth();
            DropDownClaimCode(ClaimCodeBo.StatusActive, selectedCode: model.ClaimCode, codeAsValue: true);
            DropDownInsuredGenderCode(true);
            DropDownInsuredTobaccoUse(true);
            ViewBag.DropDownRemarkStatus = DropDownClaimStatus();

            // Documents
            string downloadDocumentUrl = Url.Action("Download", "Document");
            IList<DocumentBo> documentBos = GetDocuments(moduleId, id, downloadDocumentUrl, true, departmentId);

            // Remarks
            IList<RemarkBo> remarkBos = RemarkService.GetByModuleIdObjectId(moduleId, id, true, departmentId);
            if (documentBos != null && documentBos.Count != 0)
            {
                foreach (RemarkBo remarkBo in remarkBos)
                {
                    remarkBo.DocumentBos = documentBos.Where(q => q.RemarkId.HasValue && q.RemarkId == remarkBo.Id).ToList();
                }
            }
            ViewBag.Remarks = remarkBos;

            // StatusHistories
            IList<StatusHistoryBo> statusHistoryBos = StatusHistoryService.GetStatusHistoriesByModuleIdObjectId(moduleId, id);
            ViewBag.StatusHistories = statusHistoryBos;

            // Changelog
            IList<UserTrailBo> userTrailBos = UserTrailService.GetByControllerObject(Controller, id, true);
            ViewBag.UserTrails = userTrailBos;

            // Finance Provisioning
            IList<FinanceProvisioningTransactionBo> financeProvisioningTransactionBos = FinanceProvisioningTransactionService.GetByClaimRegisterId(id, false);
            ViewBag.FinanceProvisioningTransactions = financeProvisioningTransactionBos;
            ViewBag.IsProvisioned = financeProvisioningTransactionBos.Count > 0;

            ViewBag.DirectRetroProvisioningTransactions = DirectRetroProvisioningTransactionService.GetByClaimRegisterId(id, false, 2);

            if (!string.IsNullOrEmpty(model.ProvisionErrors))
            {
                ViewBag.ProvisionErrors = JsonConvert.DeserializeObject<List<string>>(model.ProvisionErrors);
            }

            // Red Flag Warnings
            if (!string.IsNullOrEmpty(model.RedFlagWarnings))
            {
                List<string> redFlags = JsonConvert.DeserializeObject<List<string>>(model.RedFlagWarnings);
                foreach (string redFlag in redFlags)
                {
                    string warning = ClaimRegisterBo.ParseRedFlagWarning(redFlag);
                    if (!string.IsNullOrEmpty(warning))
                        AddWarningMsg(warning);
                }
            }

            // Action Urls
            if (genActions)
                LoadActionUrls(model);
        }

        public List<SelectListItem> DropDownDuplicationCheckStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= ClaimRegisterBo.DuplicationCheckStatusMax; i++)
            {
                items.Add(new SelectListItem { Text = ClaimRegisterBo.GetDuplicationCheckStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownDuplicationCheckStatuses = items;
            return items;
        }

        public void LoadActionUrls(ClaimRegisterViewModel model)
        {
            Dictionary<string, object> urls = new Dictionary<string, object>();

            Dictionary<string, int> statuses = GetPossibleNextStatus(model.ClaimStatus, model.DuplicationCheckStatus, model.RecordType, model.IsReferralCase, model.ClaimCode, model.ClaimRecoveryAmt);
            foreach (var status in statuses)
            {
                switch (status.Key)
                {
                    case "DUPLICATE CLAIM":
                        break;
                    default:
                        string url = Url.Action("UpdateStatus", new { id = model.Id, status = status.Value });
                        urls.Add(status.Key, url);
                        break;
                }
            }

            if (!model.IsClaim)
            {
                if (model.ClaimTransactionType == PickListDetailBo.ClaimTransactionTypeBulk && model.ClaimStatus == ClaimRegisterBo.StatusReported)
                    urls.Add("Unbulk Claim", Url.Action("UnbulkClaim", new { id = model.Id }));

                if (model.ClaimStatus == ClaimRegisterBo.StatusDeclined)
                {
                    if (model.OffsetStatus == ClaimRegisterBo.OffsetStatusPending)
                        urls.Add("Zerorise Claim", Url.Action("ZeroriseClaim", new { id = model.Id }));
                    else if (model.OffsetStatus == ClaimRegisterBo.OffsetStatusOffset)
                        urls.Add("Create Adjustment", Url.Action("Create", new { id = model.Id }));
                }

                if (model.ProvisionStatus == ClaimRegisterBo.ProvisionStatusFailed)
                    urls.Add("Reprocess Provision", Url.Action("ReprocessProvisionClaim", new { id = model.Id }));

                if (model.DrProvisionStatus == ClaimRegisterBo.DrProvisionStatusFailed)
                    urls.Add("Provision Direct Retro", Url.Action("ProvisionDirectRetro", new { id = model.Id }));
            }

            ViewBag.ActionUrls = urls;
        }

        // Overwrite Functions
        public virtual IQueryable<ClaimRegisterViewModel> GetQuery()
        {
            return _db.GetClaimRegister().Select(ClaimRegisterViewModel.Expression());
        }

        public virtual bool IsClaimOnly()
        {
            return false;
        }

        public virtual Dictionary<string, int> GetPossibleNextStatus(int currentStatus, int duplicationCheckStatus, string recordType, bool isReferralCase = false, string claimCode = null, double? claimRecoveryAmt = null)
        {
            Dictionary<string, int> statuses = new Dictionary<string, int>();

            switch (currentStatus)
            {
                case ClaimRegisterBo.StatusFailed:
                    if (duplicationCheckStatus == ClaimRegisterBo.DuplicationCheckStatusHasDuplicate)
                        statuses.Add("SUSPECTED DUPLICATE", ClaimRegisterBo.StatusSuspectedDuplication);
                    else
                        statuses.Add("SUBMIT FOR CLARIFICATION", ClaimRegisterBo.StatusPendingClarification);
                    statuses.Add("PROCESS DATA", ClaimRegisterBo.StatusReported);
                    break;
                case ClaimRegisterBo.StatusPendingClarification:
                    statuses.Add("CLOSE CLAIM", ClaimRegisterBo.StatusClosed);
                    statuses.Add("REGISTER CLAIM", ClaimRegisterBo.StatusRegistered);
                    break;
            }

            return statuses;
        }

        // Ajax Functions
        [HttpPost]
        public JsonResult GetClaimDataConfigByCedant(string cedantCode)
        {
            IList<ClaimDataConfigBo> claimDataConfigBos = ClaimDataConfigService.GetByCedantCodeStatus(cedantCode, ClaimDataConfigBo.StatusApproved);
            return Json(new { claimDataConfigBos });
        }

        [HttpPost]
        public JsonResult SearchOriginalClaimRegister(string policyNumber, string cedingPlanCode, string insuredName, string claimCode, string dateOfBirthStr, string soaQuarter = null, string riskQuarter = null, string dateOfEventStr = null, string treatyCode = null, string cedingCompany = null, int claimRegisterId = 0)
        {
            DateTime? dateOfEvent = Util.GetParseDateTime(dateOfEventStr);
            DateTime? dateOfBirth = Util.GetParseDateTime(dateOfBirthStr);

            IList<ClaimRegisterBo> claimRegisterBos = ClaimRegisterService.GetByMatchParam(policyNumber, cedingPlanCode, insuredName, soaQuarter, riskQuarter, dateOfEvent, claimCode, treatyCode, dateOfBirth, cedingCompany, claimRegisterId);

            return Json(new { claimRegisterBos });
        }

        [HttpPost]
        public JsonResult FindClaimRegister(int id)
        {
            ClaimRegisterBo bo = ClaimRegisterService.Find(id, false, true);
            return Json(new { bo });
        }
    }
}
