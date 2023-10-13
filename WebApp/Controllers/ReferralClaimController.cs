using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.RiDatas;
using BusinessObject.Sanctions;
using ConsoleApp.Commands.ProcessDatas;
using ConsoleApp.Commands.ProcessDatas.Exports;
using ConsoleApp.Commands.RawFiles.Sanction;
using Newtonsoft.Json;
using PagedList;
using Services;
using Services.Identity;
using Services.RiDatas;
using Services.Sanctions;
using Shared;
using Shared.DataAccess;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
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
    public class ReferralClaimController : BaseController
    {
        public const string Controller = "ReferralClaim";

        // GET: ReferralClaim
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string ReferralId,
            int? TurnAroundTime,
            string PolicyNumber,
            string ReceivedAt,
            string RespondedAt,
            string DateReceivedFullDocuments,
            string DateOfCommencement,
            string DateOfEvent,
            string TreatyCode,
            string RecordType,
            string InsuredName,
            string CedingCompany,
            string ClaimRecoveryAmount,
            int? PersonInChargeId,
            int? Status,
            int? ClaimsDecision,
            string SortOrder,
            int? Page,
            int? ClaimRegisterFilePage)
        {
            DateTime? receivedAt = Util.GetParseDateTime(ReceivedAt);
            DateTime? respondedAt = Util.GetParseDateTime(RespondedAt);
            DateTime? dateReceivedFullDocuments = Util.GetParseDateTime(DateReceivedFullDocuments);
            DateTime? dateOfCommencement = Util.GetParseDateTime(DateOfCommencement);
            DateTime? dateOfEvent = Util.GetParseDateTime(DateOfEvent);

            double? claimRecoveryAmount = Util.StringToDouble(ClaimRecoveryAmount);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["ReferralId"] = ReferralId,
                ["TurnAroundTime"] = TurnAroundTime,
                ["PolicyNumber"] = PolicyNumber,
                ["ReceivedAt"] = receivedAt.HasValue ? ReceivedAt : null,
                ["RespondedAt"] = respondedAt.HasValue ? RespondedAt : null,
                ["DateReceivedFullDocuments"] = dateReceivedFullDocuments.HasValue ? DateReceivedFullDocuments : null,
                ["DateOfCommencement"] = dateOfCommencement.HasValue ? DateOfCommencement : null,
                ["DateOfEvent"] = dateOfEvent.HasValue ? DateOfEvent : null,
                ["TreatyCode"] = TreatyCode,
                ["RecordType"] = RecordType,
                ["InsuredName"] = InsuredName,
                ["CedingCompany"] = CedingCompany,
                ["ClaimRecoveryAmount"] = claimRecoveryAmount.HasValue ? ClaimRecoveryAmount : null,
                ["PersonInChargeId"] = PersonInChargeId,
                ["Status"] = Status,
                ["ClaimsDecision"] = ClaimsDecision,
                ["SortOrder"] = SortOrder,
            };

            ViewBag.SortOrder = SortOrder;
            ViewBag.SortReferralId = GetSortParam("ReferralId");
            ViewBag.SortPolicyNumber = GetSortParam("PolicyNumber");
            ViewBag.SortReceivedAt = GetSortParam("ReceivedAt");
            ViewBag.SortRespondedAt = GetSortParam("RespondedAt");
            ViewBag.SortDateReceivedFullDocuments = GetSortParam("DateReceivedFullDocuments");
            ViewBag.SortDateOfCommencement = GetSortParam("DateOfCommencement");
            ViewBag.SortDateOfEvent = GetSortParam("DateOfEvent");
            ViewBag.SortTreatyCode = GetSortParam("TreatyCode");
            ViewBag.SortRecordType = GetSortParam("RecordType");
            ViewBag.SortInsuredName = GetSortParam("InsuredName");
            ViewBag.SortCedingCompany = GetSortParam("CedingCompany");
            ViewBag.SortClaimRecoveryAmount = GetSortParam("ClaimRecoveryAmount");
            ViewBag.SortPersonInChargeId = GetSortParam("PersonInChargeId");
            ViewBag.SortStatus = GetSortParam("Status");
            ViewBag.SortClaimsDecision = GetSortParam("ClaimsDecision");

            var query = _db.ReferralClaims.Select(ReferralClaimViewModel.Expression());
            if (!string.IsNullOrEmpty(ReferralId)) query = query.Where(q => q.ReferralId.Contains(ReferralId));
            if (TurnAroundTime.HasValue)
            {
                long ticks;
                switch (TurnAroundTime)
                {
                    case ReferralClaimBo.FilterTat1Day:
                        ticks = (new TimeSpan(1, 0, 0, 0)).Ticks;
                        query = query.Where(q => q.TurnAroundTime <= ticks);
                        break;
                    case ReferralClaimBo.FilterTat2Day:
                        ticks = (new TimeSpan(2, 0, 0, 0)).Ticks;
                        query = query.Where(q => q.TurnAroundTime <= ticks);
                        break;
                    case ReferralClaimBo.FilterTatMoreThan2Day:
                        ticks = (new TimeSpan(2, 0, 0, 0)).Ticks;
                        query = query.Where(q => q.TurnAroundTime > ticks);
                        break;
                    default:
                        break;
                }
            }
            if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber.Contains(PolicyNumber));
            if (receivedAt.HasValue) query = query.Where(q => q.ReceivedAt == receivedAt);
            if (respondedAt.HasValue) query = query.Where(q => q.RespondedAt == respondedAt);
            if (dateReceivedFullDocuments.HasValue) query = query.Where(q => q.DateReceivedFullDocuments == dateReceivedFullDocuments);
            if (dateOfCommencement.HasValue) query = query.Where(q => q.DateOfCommencement == dateOfCommencement);
            if (dateOfEvent.HasValue) query = query.Where(q => q.DateOfEvent == dateOfEvent);
            if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => q.TreatyCode == TreatyCode);
            if (!string.IsNullOrEmpty(RecordType)) query = query.Where(q => q.RecordType == RecordType);
            if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => q.InsuredName.Contains(InsuredName));
            if (!string.IsNullOrEmpty(CedingCompany)) query = query.Where(q => q.CedingCompany == CedingCompany);
            if (claimRecoveryAmount.HasValue) query = query.Where(q => q.ClaimRecoveryAmount == claimRecoveryAmount);
            if (PersonInChargeId.HasValue) query = query.Where(q => q.PersonInChargeId == PersonInChargeId);
            if (Status.HasValue) query = query.Where(q => q.Status == Status);
            if (ClaimsDecision.HasValue) query = query.Where(q => q.ClaimsDecision == ClaimsDecision);

            if (SortOrder == Html.GetSortAsc("ReferralId")) query = query.OrderBy(q => q.ReferralId);
            else if (SortOrder == Html.GetSortDsc("ReferralId")) query = query.OrderByDescending(q => q.ReferralId);
            if (SortOrder == Html.GetSortAsc("TurnAroundTime")) query = query.OrderBy(q => q.TurnAroundTime);
            else if (SortOrder == Html.GetSortDsc("TurnAroundTime")) query = query.OrderByDescending(q => q.TurnAroundTime);
            else if (SortOrder == Html.GetSortAsc("PolicyNumber")) query = query.OrderBy(q => q.PolicyNumber);
            else if (SortOrder == Html.GetSortDsc("PolicyNumber")) query = query.OrderByDescending(q => q.PolicyNumber);
            else if (SortOrder == Html.GetSortAsc("ReceivedAt")) query = query.OrderBy(q => q.ReceivedAt);
            else if (SortOrder == Html.GetSortDsc("ReceivedAt")) query = query.OrderByDescending(q => q.ReceivedAt);
            else if (SortOrder == Html.GetSortAsc("RespondedAt")) query = query.OrderBy(q => q.RespondedAt);
            else if (SortOrder == Html.GetSortDsc("RespondedAt")) query = query.OrderByDescending(q => q.RespondedAt);
            else if (SortOrder == Html.GetSortAsc("DateReceivedFullDocuments")) query = query.OrderBy(q => q.DateReceivedFullDocuments);
            else if (SortOrder == Html.GetSortDsc("DateReceivedFullDocuments")) query = query.OrderByDescending(q => q.DateReceivedFullDocuments);
            else if (SortOrder == Html.GetSortAsc("DateOfCommencement")) query = query.OrderBy(q => q.DateOfCommencement);
            else if (SortOrder == Html.GetSortDsc("DateOfCommencement")) query = query.OrderByDescending(q => q.DateOfCommencement);
            else if (SortOrder == Html.GetSortAsc("DateOfEvent")) query = query.OrderBy(q => q.DateOfEvent);
            else if (SortOrder == Html.GetSortDsc("DateOfEvent")) query = query.OrderByDescending(q => q.DateOfEvent);
            else if (SortOrder == Html.GetSortAsc("TreatyCode")) query = query.OrderBy(q => q.TreatyCode);
            else if (SortOrder == Html.GetSortDsc("TreatyCode")) query = query.OrderByDescending(q => q.TreatyCode);
            else if (SortOrder == Html.GetSortAsc("RecordType")) query = query.OrderBy(q => q.RecordType);
            else if (SortOrder == Html.GetSortDsc("RecordType")) query = query.OrderByDescending(q => q.RecordType);
            else if (SortOrder == Html.GetSortAsc("InsuredName")) query = query.OrderBy(q => q.InsuredName);
            else if (SortOrder == Html.GetSortDsc("InsuredName")) query = query.OrderByDescending(q => q.InsuredName);
            else if (SortOrder == Html.GetSortAsc("CedingCompany")) query = query.OrderBy(q => q.CedingCompany);
            else if (SortOrder == Html.GetSortDsc("CedingCompany")) query = query.OrderByDescending(q => q.CedingCompany);
            else if (SortOrder == Html.GetSortAsc("ClaimRecoveryAmount")) query = query.OrderBy(q => q.ClaimRecoveryAmount);
            else if (SortOrder == Html.GetSortDsc("ClaimRecoveryAmount")) query = query.OrderByDescending(q => q.ClaimRecoveryAmount);
            else if (SortOrder == Html.GetSortAsc("PersonInChargeId")) query = query.OrderBy(q => q.PersonInCharge.FullName);
            else if (SortOrder == Html.GetSortDsc("PersonInChargeId")) query = query.OrderByDescending(q => q.PersonInCharge.FullName);
            else if (SortOrder == Html.GetSortAsc("Status")) query = query.OrderBy(q => q.Status);
            else if (SortOrder == Html.GetSortDsc("Status")) query = query.OrderByDescending(q => q.Status);
            else if (SortOrder == Html.GetSortAsc("ClaimsDecision")) query = query.OrderBy(q => q.ClaimsDecision);
            else if (SortOrder == Html.GetSortDsc("ClaimsDecision")) query = query.OrderByDescending(q => q.ClaimsDecision);
            else query = query.OrderByDescending(q => q.Id);

            ViewBag.Total = query.Count();

            IndexPage();
            LoadClaimRegisterFiles(ClaimRegisterFilePage);
            return View("Index", query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: ReferralClaim/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            var model = new ReferralClaimViewModel();
            LoadPage(model);
            return View(model);
        }

        // POST: ReferralClaim/Create
        [HttpPost]
        public virtual ActionResult Create(FormCollection form, ReferralClaimViewModel model)
        {
            if (ModelState.IsValid)
            {
                LogHelper("Referral Claim Create() " + model.Id + " Start Create Page");
                var bo = model.FormBo(AuthUserId);

                LogHelper("Referral Claim Create() " + model.Id + " Start Checklist");
                // Set Checklist
                if (!string.IsNullOrEmpty(bo.ClaimCode))
                    bo.Checklist = ClaimChecklistDetailService.GetJsonByClaimCode(bo.ClaimCode);

                LogHelper("Referral Claim Create() " + model.Id + " End Checklist");

                LogHelper("Referral Claim Create() " + model.Id + " Start Set Referral Id");
                // Set Referral Id
                DateTime today = DateTime.Today;
                string month = today.Month.ToString().PadLeft(2, '0');
                string prefix = string.Format("RC/{0}/{1}/", today.Year, month);
                int currentIdCount = ReferralClaimService.GetCurrentReferralIdCount(DateTime.Today, prefix) + 1;

                string countStr = currentIdCount.ToString().PadLeft(4, '0');
                bo.ReferralId = prefix + countStr;
                LogHelper("Referral Claim Create() " + model.Id + " End Set Referral Id");

                var trail = GetNewTrailObject();
                LogHelper("Referral Claim Create() " + model.Id + " Start Create Result");
                Result = ReferralClaimService.Create(ref bo, ref trail);
                LogHelper("Referral Claim Create() " + model.Id + " End Create Result");
                if (Result.Valid)
                {
                    model.Id = bo.Id;

                    LogHelper("Referral Claim Create() " + model.Id + " Start Add Status History");
                    StatusHistoryController.Add(model.ModuleId, bo.Id, bo.Status, AuthUserId, ref trail);

                    RemarkController.SaveRemarks(RemarkController.GetRemarks(form), model.ModuleId, model.Id, AuthUserId, ref trail, AuthUser.DepartmentId);
                    LogHelper("Referral Claim Create() " + model.Id + " End Add Status History");

                    LogHelper("Referral Claim Create() " + model.Id + " Start Create Trail");
                    CreateTrail(
                        bo.Id,
                        "Create Referral Claim"
                    );
                    LogHelper("Referral Claim Create() " + model.Id + " End Create Trail");

                    SetCreateSuccessMessage(Controller);
                    RedirectToRouteResult result = RedirectToAction("Edit", new { id = bo.Id });
                    LogHelper("Referral Claim Edit() " + model.Id + " End RedirectToAction() Create");
                    return result;
                }
                AddResult(Result);
            }

            LoadPage(model);
            LogHelper("Referral Claim Create " + model.Id + " End Load Page");
            return View(model);
        }

        // GET: ClaimRegister/Edit/5
        public virtual ActionResult Edit(int id)
        {
            LogHelper("Referral Claim Edit() " + id + " Start Opening Edit Page");
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            LogHelper("Referral Claim Edit() " + id + " Start Find Referral Claim Id");
            var bo = ReferralClaimService.Find(id);
            if (bo == null)
            {
                LogHelper("Referral Claim Edit() " + id + " End Find Referral Claim Id");
                return RedirectToAction("Index");
            }

            LogHelper("Referral Claim Edit() " + id + " Start model from bo");
            ReferralClaimViewModel model = new ReferralClaimViewModel(bo);
            LogHelper("Referral Claim Edit() " + id + " End model from bo");
            //FormatErrors(ref model, bo);
            LoadPage(model);
            LogHelper("Referral Claim Edit() " + id + " End Opening Edit Page");

            LogHelper("Referral Claim Edit() " + id + " End View() Edit Page");
            ViewResult view = View(model);
            LogHelper("Referral Claim Edit() " + id + " End View() Edit Page");
            return View(model);
        }

        // POST: ClaimRegister/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public virtual ActionResult Edit(int id, FormCollection form, ReferralClaimViewModel model)
        {
            LogHelper("Referral Claim Edit() " + model.Id + " Start Saving Edit");
            var dbBo = ReferralClaimService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            int originalStatus = dbBo.Status;

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, dbBo);

                LogHelper("Referral Claim Edit() " + model.Id + " Start Updating Edit");
                if (UpdateReferralClaim(bo, model.ModuleId, form))
                {
                    LogHelper("Referral Claim Edit() " + model.Id + " End Updating Edit");
                    RedirectToRouteResult result = RedirectToAction("Edit", new { id = bo.Id });
                    LogHelper("Referral Claim Edit() " + model.Id + " End RedirectToAction() Edit");
                    return result;
                }
            }

            model.Status = originalStatus;
            LogHelper("Referral Claim Edit() " + model.Id + " Start LoadPage()");
            LoadPage(model);
            LogHelper("Referral Claim Edit() " + model.Id + " Start View() Edit Page");
            ViewResult view = View(model);
            LogHelper("Referral Claim Edit() " + model.Id + " End View() Edit Page");
            return view;
        }

        // POST: ClaimRegister/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public virtual ActionResult CloseRegister(int id, FormCollection form, ReferralClaimViewModel model)
        {
            LogHelper("Referral Claim CloseRegister() " + model.Id + " Start Close Register");
            LogHelper("Referral Claim CloseRegister() " + model.Id + " Start Find bo");
            var dbBo = ReferralClaimService.Find(id);
            LogHelper("Referral Claim CloseRegister() " + model.Id + " End Find bo");
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            LogHelper("Referral Claim CloseRegister() " + model.Id + " Start Get Required Error");
            if (!model.RiDataWarehouseId.HasValue && !model.ReferralRiDataId.HasValue)
            {
                ModelState.AddModelError("RiDataWarehouseId", string.Format(MessageBag.Required, "RI Data"));
            }

            if (ModelState.IsValid)
            {
                LogHelper("Referral Claim CloseRegister() " + model.Id + " Start Form Bo");
                var bo = model.FormBo(AuthUserId, dbBo);
                bo.Status = ReferralClaimBo.StatusClosedRegistered;
                LogHelper("Referral Claim CloseRegister() " + model.Id + " End Form Bo");

                LogHelper("Referral Claim CloseRegister() " + model.Id + " Start Update Referral Claim if statement");
                if (UpdateReferralClaim(bo, model.ModuleId, form))
                {
                    LogHelper("Referral Claim CloseRegister() " + model.Id + " Start Find RiDataWarehouseBo");
                    RiDataWarehouseBo riDataBo = RiDataWarehouseService.Find(bo.RiDataWarehouseId, bo.ReferralRiDataId);
                    LogHelper("Referral Claim CloseRegister() " + model.Id + " End Find RiDataWarehouseBo");
                    LogHelper("Referral Claim CloseRegister() " + model.Id + " Start new Trail Object");
                    var registerTrail = new TrailObject();
                    int registerModuleId = ModuleService.FindByController(ModuleBo.ModuleController.ClaimRegister.ToString()).Id;
                    LogHelper("Referral Claim CloseRegister() " + model.Id + " End new Trail Object");

                    LogHelper("Referral Claim CloseRegister() " + model.Id + " Start creating claimRegisterBo");
                    var claimRegisterBo = new ClaimRegisterBo(true)
                    {
                        ClaimId = ClaimRegisterService.GetNextClaimId(),
                        EntryNo = ClaimRegisterService.GetNextEntryNo(),
                        ReferralClaimId = bo.Id,
                        RiDataWarehouseId = bo.RiDataWarehouseId,
                        ReferralRiDataId = bo.ReferralRiDataId,
                        PicClaimId = bo.PersonInChargeId,
                        IsReferralCase = true,
                        ClaimTransactionType = PickListDetailBo.ClaimTransactionTypeNew,
                        ClaimCode = bo.ClaimCode,
                        PolicyNumber = bo.PolicyNumber,
                        PolicyTerm = riDataBo.PolicyTerm,
                        ClaimRecoveryAmt = bo.ClaimRecoveryAmount,
                        TreatyCode = bo.TreatyCode,
                        TreatyType = bo.TreatyType,
                        AnnualRiPrem = riDataBo.AnnualRiPrem,
                        CauseOfEvent = bo.CauseOfEvent,
                        CedingCompany = bo.CedingCompany,
                        CedingPlanCode = bo.CedingPlanCode,
                        DateOfEvent = bo.DateOfEvent,
                        InsuredDateOfBirth = bo.InsuredDateOfBirth,
                        InsuredGenderCode = bo.InsuredGenderCode,
                        InsuredName = bo.InsuredName,
                        InsuredTobaccoUse = riDataBo.InsuredTobaccoUse,
                        Layer1SumRein = bo.SumReinsured,
                        Mfrs17AnnualCohort = riDataBo.Mfrs17AnnualCohort,
                        MlreBenefitCode = bo.MlreBenefitCode,
                        RecordType = bo.RecordType,
                        ReinsBasisCode = bo.ReinsBasisCode,
                        ReinsEffDatePol = bo.DateOfCommencement,
                        RiskQuarter = bo.RiskQuarter,
                        SumIns = bo.SumInsured,
                        IssueDatePol = riDataBo.IssueDatePol,
                        PolicyExpiryDate = riDataBo.PolicyExpiryDate,
                        ClaimAssessorId = bo.AssessedById,
                        Checklist = bo.Checklist,
                        DateOfReported = DateTime.Today,
                        CreatedById = AuthUserId
                    };
                    LogHelper("Referral Claim CloseRegister() " + model.Id + " End creating claimRegisterBo");
                    LogHelper("Referral Claim CloseRegister() " + model.Id + " Start claimRegisterBo.SetRegisteredValues()");
                    claimRegisterBo.SetRegisteredValues();
                    LogHelper("Referral Claim CloseRegister()" + model.Id + " End claimRegisterBo.SetRegisteredValues()");

                    LogHelper("Referral Claim " + model.Id + " Start Create claim Register");
                    Result registerResult = ClaimRegisterService.Create(ref claimRegisterBo, ref registerTrail);
                    LogHelper("Referral Claim CloseRegister()" + model.Id + " End Create claim Register");
                    LogHelper("Referral Claim CloseRegister()" + model.Id + " Start Add Status History");
                    StatusHistoryController.Add(registerModuleId, claimRegisterBo.Id, claimRegisterBo.ClaimStatus, AuthUserId, ref registerTrail);
                    LogHelper("Referral Claim CloseRegister()" + model.Id + " End Add Status History");

                    LogHelper("Referral Claim CloseRegister()" + model.Id + " Start new UserTrailBo");
                    UserTrailBo userTrailBo = new UserTrailBo(
                        claimRegisterBo.Id,
                        "Create Claim Register",
                        registerResult,
                        registerTrail,
                        AuthUserId
                    );
                    LogHelper("Referral Claim CloseRegister() " + model.Id + " End new UserTrailBo");
                    LogHelper("Referral Claim CloseRegister() " + model.Id + " Start Create UserTrail");
                    UserTrailService.Create(ref userTrailBo);

                    LogHelper("Referral Claim CloseRegister() " + model.Id + " End Create UserTrail");
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
            }

            LoadPage(model);
            LogHelper("Referral Claim CloseRegister() " + model.Id + " End Close Register");
            LogHelper("Referral Claim Edit() " + model.Id + " Start View() Edit Page");
            ViewResult view = View(model);
            LogHelper("Referral Claim Edit() " + model.Id + " End View() Edit Page");
            return view;
        }

        public bool UpdateReferralClaim(ReferralClaimBo bo, int moduleId, FormCollection form)
        {
            LogHelper("Referral Claim UpdateReferralClaim() " + bo.Id + " Start UpdateReferralClaim");
            ReferralClaimBo dbBo = ReferralClaimService.Find(bo.Id);

            LogHelper("Referral Claim UpdateReferralClaim() " + bo.Id + " Start Deserialize checklist Object");
            var checklists = JsonConvert.DeserializeObject<Dictionary<string, string>>(bo.Checklist ?? "");
            LogHelper("Referral Claim UpdateReferralClaim() " + bo.Id + " End Deserialize checklist Object");
            if (checklists != null)
            {
                LogHelper("Referral Claim UpdateReferralClaim() " + bo.Id + " Start Update Checklist");
                var updatedChecklists = new Dictionary<string, string>(checklists);
                foreach (KeyValuePair<string, string> checklist in checklists)
                {
                    string valueStr = form.Get(checklist.Key);
                    if (string.IsNullOrEmpty(valueStr) && checklist.Key != ClaimChecklistDetailBo.RemarkCode)
                        continue;

                    updatedChecklists[checklist.Key] = valueStr;
                }
                LogHelper("Referral Claim UpdateReferralClaim() " + bo.Id + " End Update Checklist with " + checklists.Count() + " checklists");
                LogHelper("Referral Claim UpdateReferralClaim() " + bo.Id + " Start Serialise Checklist");
                bo.Checklist = JsonConvert.SerializeObject(updatedChecklists);
                LogHelper("Referral Claim UpdateReferralClaim() " + bo.Id + " End Serialise Checklist");
            }
            else
            {
                bo.Checklist = null;
            }

            var trail = GetNewTrailObject();
            LogHelper("Referral Claim UpdateReferralClaim()" + bo.Id + " Start Updating");
            Result = ReferralClaimService.Update(ref bo, ref trail);
            LogHelper("Referral Claim UpdateReferralClaim()" + bo.Id + " End Updating");
            if (Result.Valid)
            {
                LogHelper("Referral Claim UpdateReferralClaim()" + bo.Id + " Start Adding Status History");
                StatusHistoryController.Add(moduleId, bo.Id, bo.Status, AuthUserId, ref trail);
                LogHelper("Referral Claim UpdateReferralClaim()" + bo.Id + " End Adding Status History");

                LogHelper("Referral Claim UpdateReferralClaim()" + bo.Id + " Start CreateTrail");
                CreateTrail(
                    bo.Id,
                    "Update Referral Claim"
                );
                LogHelper("Referral Claim UpdateReferralClaim()" + bo.Id + " End CreateTrail");

                SetUpdateSuccessMessage(Controller);
                LogHelper("Referral Claim UpdateReferralClaim() " + bo.Id + " End true UpdateReferralClaim");
                return true;
            }
            AddResult(Result);
            LogHelper("Referral Claim UpdateReferralClaim() " + bo.Id + " End false UpdateReferralClaim");
            return false;
        }

        [Auth(Controller = Controller, Power = AccessMatrixBo.PowerUpload, ReturnController = Controller)]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            LogHelper("Referral Claim Upload() " + "Start");
            if (upload != null && upload.ContentLength > 0)
            {
                string fileExtension = Path.GetExtension(upload.FileName);
                if (fileExtension != ".csv" && fileExtension != ".xlsx")
                {
                    SetErrorSessionMsg("Allowed file of type: .csv, .xlsx");
                    return RedirectToAction("Index");
                }

                var trail = GetNewTrailObject();
                RawFileBo rawFileBo = new RawFileBo
                {
                    Type = RawFileBo.TypeClaimData,
                    Status = RawFileBo.StatusPending,
                    FileName = upload.FileName,
                    CreatedById = AuthUserId,
                    UpdatedById = AuthUserId,
                };

                LogHelper("Referral Claim Upload() " + "Start FormatHashFileName()");
                rawFileBo.FormatHashFileName();
                LogHelper("Referral Claim Upload() " + "End FormatHashFileName()");

                LogHelper("Referral Claim Upload() " + "Start LocalPath");
                string path = rawFileBo.GetLocalPath();
                Util.MakeDir(path);
                upload.SaveAs(path);
                LogHelper("Referral Claim Upload() " + "End LocalPath");

                LogHelper("Referral Claim Upload() " + "Start Create RawFile");
                RawFileService.Create(ref rawFileBo, ref trail);
                LogHelper("Referral Claim Upload() " + "End Create RawFile");

                ReferralRiDataFileBo referralRiDataFileBo = new ReferralRiDataFileBo
                {
                    RawFileId = rawFileBo.Id,
                    CreatedById = AuthUserId,
                    UpdatedById = AuthUserId,
                };

                LogHelper("Referral Claim Upload() " + "Start Create ReferrailRiDataFile");
                Result = ReferralRiDataFileService.Create(ref referralRiDataFileBo, ref trail);
                LogHelper("Referral Claim Upload() " + "Start End ReferrailRiDataFile");

                LogHelper("Referral Claim Upload() " + "Start Create ReferrailRiDataFile Trail");
                CreateTrail(
                    referralRiDataFileBo.Id,
                    "Create Referral RI Data File"
                );
                LogHelper("Referral Claim Upload() " + "Start Create ReferrailRiDataFile End");
            }
            else
            {
                SetErrorSessionMsg("No file was uploaded");
            }

            LogHelper("Referral Claim Upload() " + "End");
            return RedirectToAction("Index");
        }

        public ActionResult Download(
            string downloadToken,
            int type,
            string ReferralId,
            int? TurnAroundTime,
            string PolicyNumber,
            string ReceivedAt,
            string RespondedAt,
            string DateReceivedFullDocuments,
            string DateOfCommencement,
            string DateOfEvent,
            string TreatyCode,
            string RecordType,
            string InsuredName,
            string CedingCompany,
            string ClaimRecoveryAmount,
            int? PersonInChargeId,
            int? Status)
        {
            // type 1 = all
            // type 2 = filtered download

            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            dynamic Params = new ExpandoObject();
            Params.ReferralId = ReferralId;
            Params.TurnAroundTime = TurnAroundTime;
            Params.PolicyNumber = PolicyNumber;
            Params.ReceivedAt = ReceivedAt;
            Params.RespondedAt = RespondedAt;
            Params.DateReceivedFullDocuments = DateReceivedFullDocuments;
            Params.DateOfCommencement = DateOfCommencement;
            Params.DateOfEvent = DateOfEvent;
            Params.TreatyCode = TreatyCode;
            Params.RecordType = RecordType;
            Params.InsuredName = InsuredName;
            Params.CedingCompany = CedingCompany;
            Params.ClaimRecoveryAmount = ClaimRecoveryAmount;
            Params.PersonInChargeId = PersonInChargeId;
            Params.Status = Status;
            var export = new GenerateExportData();
            ExportBo exportBo = export.CreateExportReferralClaim(AuthUserId, Params);

            if (exportBo.Total <= export.ExportInstantDownloadLimit)
            {
                export.Process(ref exportBo, _db);
                Session.Add("DownloadExport", true);
            }

            return RedirectToAction("Edit", "Export", new { exportBo.Id });
        }

        public void DownloadRawFile(int rawFileId)
        {
            try
            {
                RawFileBo rawFileBo = RawFileService.Find(rawFileId);

                Response.ClearContent();
                Response.Clear();
                //Response.Buffer = true;                                                                                                                   
                //Response.BufferOutput = false;
                Response.ContentType = MimeMapping.GetMimeMapping(rawFileBo.FileName);
                Response.AddHeader("Content-Disposition", "attachment; filename=" + rawFileBo.FileName);
                Response.TransmitFile(rawFileBo.GetLocalPath());
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

        public ActionResult DownloadRiDataTemplate()
        {
            var export = new ExportRiDataWarehouse();
            export.LinkReferral = true;
            export.PrefixFileName = "RIDataUploadTemplate";
            export.HandleTempDirectory();
            export.Process();

            return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));
        }

        public void LoadPage(ReferralClaimViewModel model)
        {
            LogHelper("Referral Claim LoadPage() " + model.Id + " Start LoadPage");
            LogHelper("Referral Claim LoadPage() " + model.Id + " Start Get TurnaroundTimeHours and TrnaroundTimeMinutes");
            if (model.TurnAroundTime.HasValue)
            {
                TimeSpan ts = new TimeSpan(model.TurnAroundTime.Value);
                model.TurnAroundTimeHours = ts.Hours + (ts.Days * 24);
                model.TurnAroundTimeMinutes = ts.Minutes;
            }

            if (model.DocTurnAroundTime.HasValue)
            {
                TimeSpan ts = new TimeSpan(model.DocTurnAroundTime.Value);
                model.DocTurnAroundTimeHours = ts.Hours + (ts.Days * 24);
                model.DocTurnAroundTimeMinutes = ts.Minutes;
            }
            LogHelper("Referral Claim LoadPage() " + model.Id + " End Get TurnaroundTimeHours and TrnaroundTimeMinutes");

            //if (model.ClaimRegisterId.HasValue)
            //{
            //    model.ClaimRegisterBo = ClaimRegisterService.Find(model.ClaimRegisterId, false, true);
            //}

            LogHelper("Referral Claim LoadPage() " + model.Id + " Start Find RiDataWarehouseBo");
            if (model.RiDataWarehouseId.HasValue)
            {
                model.RiDataWarehouseBo = RiDataWarehouseService.Find(model.RiDataWarehouseId, true);
            }
            else if (model.ReferralRiDataId.HasValue)
            {
                model.RiDataWarehouseBo = ReferralRiDataService.FindToWarehouse(model.ReferralRiDataId);
            }
            LogHelper("Referral Claim LoadPage() " + model.Id + " End Find RiDataWarehouseBo");


            LogHelper("Referral Claim LoadPage() " + model.Id + " Start Get Dropdowns");
            AuthUserName();
            LogHelper("Referral Claim LoadPage() " + model.Id + " Start DropDownTreatyCode()");
            DropDownTreatyCode(codeAsValue: true, foreign: false);
            LogHelper("Referral Claim LoadPage() " + model.Id + " Start DropDownCedantCode()");
            DropDownCedantCode();
            LogHelper("Referral Claim LoadPage() " + model.Id + " Start DropDownRecordType()");
            DropDownRecordType(true);
            LogHelper("Referral Claim LoadPage() " + model.Id + " Start DropDownGenderCode()");
            DropDownInsuredGenderCode(true);
            LogHelper("Referral Claim LoadPage() " + model.Id + " Start DropDownTobaccoUse()");
            DropDownInsuredTobaccoUse(true);
            LogHelper("Referral Claim LoadPage() " + model.Id + " Start DropDownClaimCode()");
            DropDownClaimCode(ClaimCodeBo.StatusActive, codeAsValue: true);
            LogHelper("Referral Claim LoadPage() " + model.Id + " Start DropDownClaimCategory()");
            DropDownClaimCategory();
            LogHelper("Referral Claim LoadPage() " + model.Id + " Start DropDownReinsBasisCode()");
            DropDownReinsBasisCode(true);
            LogHelper("Referral Claim LoadPage() " + model.Id + " Start DropDownTreatyType()");
            DropDownTreatyType(true);
            LogHelper("Referral Claim LoadPage() " + model.Id + " Start DropDownChecklistStatus()");
            DropDownChecklistStatus();
            LogHelper("Referral Claim LoadPage() " + model.Id + " Start DropDownBenefit()");
            DropDownBenefit(BenefitBo.StatusActive, codeAsValue: true);
            LogHelper("Referral Claim LoadPage() " + model.Id + " Start DropDownPicClaim()");
            DropDownPicClaim(UserBo.StatusActive, model.PersonInChargeId);
            LogHelper("Referral Claim LoadPage() " + model.Id + " Start DropDownUser()");
            DropDownUser(UserBo.StatusActive, null, false, DepartmentBo.DepartmentClaim);
            LogHelper("Referral Claim LoadPage() " + model.Id + " Start DropDownClaimsDecision()");
            DropDownClaimsDecision();
            LoadRelatedClaimRegisters(model);
            LogHelper("Referral Claim LoadPage() " + model.Id + " Start DropDownStatus()");
            DropDownStatus();
            LogHelper("Referral Claim LoadPage() " + model.Id + " End Get Dropdowns");


            LogHelper("Referral Claim LoadPage() " + model.Id + " Start Get List");
            //ViewBag.TreatyCodeList = TreatyCodeService.GetForList(TreatyCodeBo.StatusActive); -- used in treatyCodeModal, current is not used
            ViewBag.TreatyCodeList = Array.Empty<TreatyCodeBo>();
            ViewBag.StandardOutputList = StandardOutputService.Get();
            ViewBag.IsClosedRegistered = model.Status == ReferralClaimBo.StatusClosedRegistered;
            LogHelper("Referral Claim LoadPage() " + model.Id + " End Get List");


            LogHelper("Referral Claim LoadPage() " + model.Id + " Start Get Check List");
            var checklists = JsonConvert.DeserializeObject<Dictionary<string, string>>(model.Checklist ?? "");
            if (string.IsNullOrEmpty(model.ClaimCode))
            {
                ViewBag.ChecklistError = "Claim Code is not set";
            }
            else if (string.IsNullOrEmpty(model.Checklist))
            {
                ViewBag.ChecklistError = "No checklist found";
            }
            else
            {
                List<string> checklistErrors = new List<string>();
                foreach (KeyValuePair<string, string> checklist in checklists)
                {
                    if (checklist.Key == ClaimChecklistDetailBo.RemarkCode)
                        continue;

                    int? checklistValue = Util.GetParseInt(checklist.Value);
                    if (!checklistValue.HasValue || checklistValue.Value != ClaimRegisterBo.ChecklistStatusPending)
                        continue;

                    checklistErrors.Add(checklist.Key);
                }
                ViewBag.ChecklistErrors = checklistErrors;
            }
            ViewBag.Checklists = checklists;
            LogHelper("Referral Claim LoadPage() " + model.Id + " End Get Check List");

            LogHelper("Referral Claim LoadPage() " + model.Id + " Start Get Referral Reason Drop Down");
            ViewBag.DropDownReasonCedantReferral = DropDownClaimReason(ClaimReasonBo.TypeCedantReferral);
            ViewBag.DropDownReasonReferralDelay = DropDownClaimReason(ClaimReasonBo.TypeReferralDelay);
            ViewBag.DropDownReasonRetroReferral = DropDownClaimReason(ClaimReasonBo.TypeRetroReferral);
            LogHelper("Referral Claim LoadPage() " + model.Id + " End Get Referral Reason Drop Down");

            if (!string.IsNullOrEmpty(model.Error))
            {
                LogHelper("Referral Claim LoadPage() " + model.Id + " Start Get Error");
                List<string> warnings = JsonConvert.DeserializeObject<List<string>>(model.Error);
                if (!warnings.IsNullOrEmpty())
                    SetWarningSessionMsgArr(warnings);
                LogHelper("Referral Claim LoadPage() " + model.Id + " End Get Error");
            }

            if (model.Id != 0)
            {
                LogHelper("Referral Claim LoadPage() " + model.Id + " Start Edit Load Page");
                int? departmentId = AuthUser.DepartmentId;
                string authUserName = AuthUserName();

                LogHelper("Referral Claim LoadPage() " + model.Id + " Start Get Document");
                // Documents
                string downloadDocumentUrl = Url.Action("Download", "Document");
                IList<DocumentBo> documentBos = GetDocuments(model.ModuleId, model.Id, downloadDocumentUrl, true, departmentId);
                LogHelper("Referral Claim LoadPage() " + model.Id + " End Get Document");

                LogHelper("Referral Claim LoadPage() " + model.Id + " Start Get Remarks");
                // Remarks
                IList<RemarkBo> remarkBos = RemarkService.GetByModuleIdObjectId(model.ModuleId, model.Id, true, departmentId);
                if (documentBos != null && documentBos.Count != 0)
                {
                    foreach (RemarkBo remarkBo in remarkBos)
                    {
                        remarkBo.DocumentBos = documentBos.Where(q => q.RemarkId.HasValue && q.RemarkId == remarkBo.Id).ToList();
                    }
                }
                ViewBag.Remarks = remarkBos;
                LogHelper("Referral Claim LoadPage() " + model.Id + " End Get Remarks");

                LogHelper("Referral Claim LoadPage() " + model.Id + " Start Get Status Histories");
                // StatusHistories
                IList<StatusHistoryBo> statusHistoryBos = StatusHistoryService.GetStatusHistoriesByModuleIdObjectId(model.ModuleId, model.Id);
                ViewBag.StatusHistories = statusHistoryBos;
                LogHelper("Referral Claim LoadPage() " + model.Id + " End Get Status Histories");

                LogHelper("Referral Claim LoadPage() " + model.Id + " Start Get Change log");
                // Changelog
                IList<UserTrailBo> userTrailBos = UserTrailService.GetByControllerObject(Controller, model.Id, true);
                ViewBag.UserTrails = userTrailBos;
                LogHelper("Referral Claim LoadPage() " + model.Id + " End Get Change log");
            }


            LogHelper("Referral Claim LoadPage() " + model.Id + " End Load Page (Start SetViewBagMessage())");
            SetViewBagMessage();
            LogHelper("Referral Claim LoadPage() " + model.Id + " End Load Page (End SetViewBagMessage())");
        }

        public void IndexPage()
        {
            DropDownTreatyCode(codeAsValue: true, foreign: false);
            DropDownCedantCode();
            DropDownRecordType(true);
            DropDownStatus();
            DropDownTurnAroundTime();
            DropDownPicClaim();
            DropDownClaimsDecision();

            SetViewBagMessage();
        }

        public void LoadClaimRegisterFiles(int? page)
        {
            var query = _db.ReferralRiDataFiles.Select(ReferralRiDataFileViewModel.Expression()).OrderByDescending(q => q.CreatedAt);

            ViewBag.ReferralRiDataFilesTotal = query.Count();
            ViewBag.ReferralRiDataFiles = query.ToPagedList(page ?? 1, PageSize);
        }

        public List<SelectListItem> DropDownStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= ReferralClaimBo.StatusMax; i++)
            {
                items.Add(new SelectListItem { Text = ReferralClaimBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownStatuses = items;
            return items;
        }

        public List<SelectListItem> DropDownClaimsDecision()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= ReferralClaimBo.ClaimsDecisionMax; i++)
            {
                items.Add(new SelectListItem { Text = ReferralClaimBo.GetClaimsDecisionName(i), Value = i.ToString() });
            }
            ViewBag.DropDownClaimsDecisions = items;
            return items;
        }

        public List<SelectListItem> DropDownTurnAroundTime()
        {
            var items = GetEmptyDropDownList();

            for (int i = 1; i <= ReferralClaimBo.FilterTatMax; i++)
            {
                items.Add(new SelectListItem { Text = ReferralClaimBo.GetFilterTatName(i), Value = i.ToString() });
            }
            ViewBag.DropDownTurnAroundTime = items;
            return items;
        }

        [HttpPost]
        public JsonResult GetDuplicate(ReferralClaimBo bo)
        {
            LogHelper("Referral Claim GetDuplicate() " + "Start");
            bo.InsuredDateOfBirth = Util.GetParseDateTime(bo.InsuredDateOfBirthStr);
            bo.DateOfEvent = Util.GetParseDateTime(bo.DateOfEventStr);
            List<ReferralClaimBo> bos = ReferralClaimService.GetDuplicate(bo, false, true).ToList();

            bos.AddRange(ClaimRegisterService.GetReferralClaimDuplicate(bo));

            LogHelper("Referral Claim GetDuplicate() " + "End");
            return Json(new { bos });
        }

        [HttpPost]
        public JsonResult MatchSanction(ReferralClaimBo bo)
        {
            LogHelper("Referral Claim MatchSanction() " + "Start");
            SanctionVerificationChecking checking = new SanctionVerificationChecking()
            {
                ModuleBo = GetModuleByController(Controller),
                ObjectId = bo.Id,
                IsReferralClaim = true,
                InsuredName = bo.InsuredName,
                InsuredDateOfBirth = Util.GetParseDateTime(bo.InsuredDateOfBirthStr),
                InsuredIcNumber = bo.InsuredIcNumber,
                CedingCompany = bo.CedingCompany,
                TreatyCode = bo.TreatyCode,
                CedingPlanCode = bo.CedingPlanCode,
                PolicyNumber = bo.PolicyNumber,
                SumReins = Util.StringToDouble(bo.SumReinsuredStr),
                ClaimAmount = Util.StringToDouble(bo.ClaimRecoveryAmount),
                PolicyCommencementDate = Util.GetParseDateTime(bo.DateOfCommencementStr),
                Validate = true
            };
            checking.Check();
            LogHelper("Referral Claim MatchSanction() " + "End checking" + bo.Id);

            IList<SanctionBo> bos = new List<SanctionBo>();
            List<int> detailIds = new List<int>();
            if (checking.IsFound && !checking.SanctionBos.IsNullOrEmpty())
            {
                checking.Save();
                bos = checking.SanctionBos;
                detailIds = checking.SanctionVerificationDetailIds;
            }

            LogHelper("Referral Claim MatchSanction() " + "End");
            return Json(new { bos, detailIds });
        }

        [HttpPost]
        public JsonResult UpdateSanctionVerificationDetail(List<int> ids, bool isWhitelist, bool isExactMatch)
        {
            LogHelper("Referral Claim UpdateSanctionVerificationDetail() " + "Start");
            bool success = true;
            string action = isWhitelist ? "Whitelist" : "Exact Matched";

            foreach (int id in ids)
            {
                LogHelper("Referral Claim UpdateSanctionVerificationDetail() " + "Start SanctionVerificationDetailService.Find()");
                var bo = SanctionVerificationDetailService.Find(id);
                LogHelper("Referral Claim UpdateSanctionVerificationDetail() " + "End SanctionVerificationDetailService.Find()");
                if (bo.IsExactMatch == isExactMatch && bo.IsWhitelist == isWhitelist)
                    continue;

                LogHelper("Referral Claim UpdateSanctionVerificationDetail() " + "Start trail");
                var trail = GetNewTrailObject();
                if (isWhitelist)
                {
                    LogHelper("Referral Claim UpdateSanctionVerificationDetail() " + "Start trail isWhiteList");
                    Result = (new SanctionVerificationController()).WhitelistSanction(bo, ref trail, false, authUserId: AuthUserId);
                }
                else if (isExactMatch)
                {
                    LogHelper("Referral Claim UpdateSanctionVerificationDetail() " + "Start trail isExactMatch");
                    Result = (new SanctionVerificationController()).ExactMatchSanction(bo, ref trail, 0, ModuleBo.Id, AuthUserId);
                }
                LogHelper("Referral Claim UpdateSanctionVerificationDetail() " + "End trail");

                CreateTrail(
                    bo.Id,
                    string.Format("Update Sanction Verification Detail {0}", action)
                );
                LogHelper("Referral Claim UpdateSanctionVerificationDetail() " + "End trail");
            }

            string message = "An error occured";
            if (success)
                message = string.Format("Sanction Successfully {0}", isWhitelist ? "Whitelisted" : "Matched");

            return Json(new { success, message });
        }

        [HttpPost]
        public JsonResult FindReferralClaim(string treatyCode, string insuredName, string claimCode, int claimRegisterId, string treatyShareStr)
        {
            LogHelper("Referral Claim FindReferralClaim() " + "Start");
            ClaimRegisterBo claimRegisterBo = null;

            LogHelper("Referral Claim FindReferralClaim() " + "Start FindForClaimRegisterLink() with " + treatyCode + " " + insuredName + " " + claimCode);
            ReferralClaimBo bo = ReferralClaimService.FindForClaimRegisterLink(treatyCode, insuredName, claimCode);
            LogHelper("Referral Claim FindReferralClaim() " + "End FindForClaimRegisterLink() with " + treatyCode + " " + insuredName + " " + claimCode);

            bool success = true;
            string message = "";

            LogHelper("Referral Claim FindReferralClaim() " + "Start if Bo != null");
            if (bo != null)
            {
                LogHelper("Referral Claim FindReferralClaim() " + "Start bo != null");
                //bo.ClaimRegisterId = claimRegisterId;
                bo.TreatyShare = Util.StringToDouble(treatyShareStr);
                bo.UpdatedById = AuthUserId;

                var trail = GetNewTrailObject();
                LogHelper("Referral Claim FindReferralClaim() " + "Start Update() bo");
                Result = ReferralClaimService.Update(ref bo, ref trail);
                LogHelper("Referral Claim FindReferralClaim() " + "End Update() bo");
                if (Result.Valid)
                {
                    LogHelper("Referral Claim FindReferralClaim() " + bo.Id + "Start CreateTrail");
                    CreateTrail(
                        bo.Id,
                        "Update Referral Claim"
                    );
                    LogHelper("Referral Claim FindReferralClaim() " + bo.Id + "End CreateTrail");

                    LogHelper("Referral Claim FindReferralClaim() " + bo.Id + "Start form bo");
                    claimRegisterBo = new ClaimRegisterBo()
                    {
                        Id = bo.Id,
                        PolicyNumber = bo.PolicyNumber,
                        ReferralId = bo.ReferralId,
                        CedingCompany = bo.CedingCompany,
                        SumInsStr = Util.DoubleToString(bo.SumInsured, 2),
                        CauseOfEvent = bo.CauseOfEvent,
                        ClaimRecoveryAmtStr = Util.DoubleToString(bo.ClaimRecoveryAmount, 2),
                        PicClaimBo = UserService.Find(bo.PersonInChargeId),
                        StatusName = ReferralClaimBo.GetStatusName(bo.Status),
                    };
                    LogHelper("Referral Claim FindReferralClaim() " + bo.Id + "End form bo");
                }
                else
                {
                    LogHelper("Referral Claim FindReferralClaim() " + "Start Result not Valid");
                    success = false;
                    message = Result.MessageBag.Errors[0];
                    LogHelper("Referral Claim FindReferralClaim() " + "End Result not Valid");
                }
            }
            else
            {
                LogHelper("Referral Claim FindReferralClaim() " + "Start bo = null");
                success = false;
                message = "No Referral Claim found";
                LogHelper("Referral Claim FindReferralClaim() " + "End bo = null");
            }
            LogHelper("Referral Claim FindReferralClaim() " + "End if Bo != null");

            return Json(new { success, message, claimRegisterBo });
        }

        [HttpPost]
        public JsonResult FindForClaimRegister(string refId)
        {
            LogHelper("Referral Claim FindForClaimRegister() " + refId + " Start");
            bool success = true;
            string message = "";
            LogHelper("Referral Claim FindForClaimRegister() " + refId + " Start FindByReferralId");
            var referralClaimBo = ReferralClaimService.FindByReferralId(refId);
            LogHelper("Referral Claim FindForClaimRegister() " + refId + " End FindByReferralId");
            ClaimRegisterBo claimRegisterBo = null;

            LogHelper("Referral Claim FindForClaimRegister() " + refId + " Start If ClaimRegisterId != null");
            if (referralClaimBo.ClaimRegisterId != null)
            {
                LogHelper("Referral Claim FindForClaimRegister() " + refId + " Start FindForClaimRegister");
                var bo = ClaimRegisterService.FindForClaimRegister(referralClaimBo.ClaimRegisterId);
                LogHelper("Referral Claim FindForClaimRegister() " + refId + " End FindForClaimRegister");
                var totalRetroAmount = bo.RetroRecovery1 + bo.RetroRecovery2 + bo.RetroRecovery3;
                if (bo != null)
                {
                    LogHelper("Referral Claim FindForClaimRegister() " + refId + " Start form bo");
                    claimRegisterBo = new ClaimRegisterBo()
                    {
                        Id = bo.Id,
                        PolicyNumber = bo.PolicyNumber,
                        ClaimId = bo.ClaimId,
                        CedingCompany = bo.CedingCompany,
                        SumInsStr = Util.DoubleToString(bo.SumIns, 2),
                        CauseOfEvent = bo.CauseOfEvent,
                        TotalRetroAmountStr = Util.DoubleToString(totalRetroAmount, 2),
                        ClaimRecoveryAmtStr = Util.DoubleToString(bo.ClaimRecoveryAmt, 2),
                        PicClaimBo = UserService.Find(bo.PicClaimId),
                        StatusName = ClaimRegisterBo.GetStatusName(bo.ClaimStatus),
                    };
                    LogHelper("Referral Claim FindForClaimRegister() " + refId + " End form bo");
                }
                else
                {
                    LogHelper("Referral Claim FindForClaimRegister() " + refId + " Start Error Message");
                    success = false;
                    message = Result.MessageBag.Errors[0];
                    LogHelper("Referral Claim FindForClaimRegister() " + refId + " End Error Message");
                }
            }
            else
            {
                LogHelper("Referral Claim FindForClaimRegister() " + refId + " Start If ClaimRegisterId = null");
                success = false;
                message = "No Claim Register found";
                LogHelper("Referral Claim FindForClaimRegister() " + refId + " End If ClaimRegisterId = null");
            }
            LogHelper("Referral Claim FindForClaimRegister() " + refId + " End If ClaimRegisterId != null");

            return Json(new { success, message, claimRegisterBo });
        }

        public void LoadRelatedClaimRegisters(ReferralClaimViewModel model)
        {
            LogHelper("Referral Claim LoadRelatedClaimRegisters() " + model.Id + " Start");
            ViewBag.RelatedClaimRegisters = ReferralClaimService.GetRelatedClaimRegisters(model.Id);
            LogHelper("Referral Claim LoadRelatedClaimRegisters() " + model.Id + " End");
        }

        public static void LogHelper(string message)
        {
            bool lockObj = Convert.ToBoolean(Util.GetConfig("Logging"));
            string filePath = string.Format("{0}/ReferralClaimVerySummary".AppendDateFileName(".txt"), Util.GetLogPath("ReferralClaimView"));

            if (lockObj)
            {
                Util.MakeDir(filePath);

                try
                {
                    using (var textFile = new TextFile(filePath, true, true))
                    {
                        textFile.WriteLine(string.Format("{0}   {1}   {2}", DateTime.Now.ToString("dd MMM yyyy HH:mm:ss:ffff"), System.Web.HttpContext.Current.Session.SessionID, message));
                        textFile.WriteLine();
                        textFile.WriteLine();
                    }
                }
                catch { }
            }
        }
    }
}