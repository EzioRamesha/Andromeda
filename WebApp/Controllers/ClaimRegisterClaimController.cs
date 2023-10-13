using BusinessObject;
using BusinessObject.Identity;
using ConsoleApp.Commands.RawFiles.ClaimRegister;
using Newtonsoft.Json;
using PagedList;
using Services;
using Services.Identity;
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
    public class ClaimRegisterClaimController : ClaimRegisterController
    {
        [Auth(Controller = "ClaimRegisterClaim", Power = "R")]
        public override ActionResult Index(
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
            return base.Index(
                EntryNo,
                SoaQuarter,
                ClaimId,
                ClaimTransactionType,
                IsReferralCase,
                RiDataWarehouseId,
                RecordType,
                TreatyCodeId,
                PolicyNumber,
                CedantId,
                InsuredName,
                InsuredDateOfBirth,
                LastTransactionDate,
                DateOfReported,
                CedantDateOfNotification,
                DateOfRegister,
                DateOfCommencement,
                DateOfEvent,
                PolicyDuration,
                TargetDateToIssueInvoice,
                ClaimRecoveryAmt,
                CauseOfEvent,
                PicClaimId,
                PicDaaId,
                ClaimStatus,
                DuplicationCheckStatus,
                ProvisionStatus,
                OffsetStatus,
                SortOrder,
                Page,
                BatchSelection,
                SelectedIds);
        }



        [Auth(Controller = "ClaimRegisterClaim", Power = "C")]
        public override ActionResult Create(int? id = null)
        {
            if (CheckCutOffReadOnly(Controller))
                return RedirectToAction("Index");

            var model = new ClaimRegisterViewModel(true);
            LoadClaimPage(ref model);
            DropDownClaimIds();
            return View(model);
        }

        [HttpPost]
        [Auth(Controller = "ClaimRegisterClaim", Power = "C")]
        public override ActionResult Create(FormCollection form, ClaimRegisterViewModel model)
        {
            if (ModelState.IsValid && !CheckCutOffReadOnly(Controller))
            {
                var originalBo = ClaimRegisterService.Find(model.OriginalClaimRegisterId);
                originalBo.Id = 0;
                originalBo.ClaimStatus = ClaimRegisterBo.StatusRegistered;
                originalBo.MappingStatus = ClaimRegisterBo.MappingStatusPending;
                originalBo.ProcessingStatus = ClaimRegisterBo.ProcessingStatusPending;
                originalBo.DuplicationCheckStatus = ClaimRegisterBo.DuplicationCheckStatusPending;
                originalBo.PostComputationStatus = ClaimRegisterBo.PostComputationStatusPending;
                originalBo.PostValidationStatus = ClaimRegisterBo.PostValidationStatusPending;
                originalBo.ClaimTransactionType = PickListDetailBo.ClaimTransactionTypeAdjustment;
                originalBo.ProvisionStatus = ClaimRegisterBo.ProvisionStatusPending;
                originalBo.OffsetStatus = ClaimRegisterBo.OffsetStatusPending;
                originalBo.RequestUnderwriterReview = false;
                originalBo.UnderwriterFeedback = null;
                originalBo.CreatedById = AuthUserId;

                var bo = model.FormBo(AuthUserId, originalBo);
                var originalClaimRegisterBo = ClaimRegisterService.Find(bo.OriginalClaimRegisterId);
                bo.ClaimId = originalClaimRegisterBo?.ClaimId;

                originalBo.PicClaimId = AuthUserId;

                if (!string.IsNullOrEmpty(bo.ClaimCode))
                    bo.Checklist = ClaimChecklistDetailService.GetJsonByClaimCode(bo.ClaimCode);

                //bo.ClaimId = ClaimRegisterService.GetNextClaimId();
                bo.EntryNo = ClaimRegisterService.GetNextEntryNo();
                if (!string.IsNullOrEmpty(bo.TreatyCode) && string.IsNullOrEmpty(bo.TreatyType))
                {
                    var treatyCodeBo = TreatyCodeService.FindByCode(bo.TreatyCode);
                    bo.TreatyType = treatyCodeBo.TreatyStatusPickListDetailBo?.Code;
                }

                bo.DateOfReported = DateTime.Today;
                bo.SetRegisteredValues();

                var trail = GetNewTrailObject();
                Result = ClaimRegisterService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;

                    ProvisionClaimRegister provisionClaimRegister = new ProvisionClaimRegister(bo, provisionDirectRetro: false);
                    provisionClaimRegister.Provision();

                    StatusHistoryController.Add(model.ModuleId, bo.Id, bo.ClaimStatus, AuthUserId, ref trail);

                    List<RemarkBo> remarkBos = RemarkController.GetRemarks(form);
                    RemarkController.SaveRemarks(remarkBos, model.ModuleId, model.Id, AuthUserId, ref trail, AuthUser.DepartmentId);

                    List<RemarkBo> underWritingRemarkBos = RemarkController.GetRemarks(form, prefix: "ur");
                    underWritingRemarkBos = underWritingRemarkBos.Select(u => { u.SubModuleController = ClaimRegisterBo.SubModuleUnderwriting; return u; }).ToList();
                    RemarkController.SaveRemarks(underWritingRemarkBos, model.ModuleId, model.Id, AuthUserId, ref trail, AuthUser.DepartmentId);

                    CreateTrail(
                        bo.Id,
                        "Create Claim Register"
                    );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadClaimPage(ref model);
            DropDownClaimIds();
            return View(model);
        }

        // GET: ClaimRegister/Edit/5
        public override ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly("ClaimRegisterClaim"))
                return RedirectDashboard();

            var bo = ClaimRegisterService.Find(id, true);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            ClaimRegisterViewModel model = new ClaimRegisterViewModel(bo, true);
            CheckCutOffReadOnly(Controller, id);
            LoadClaimPage(ref model, bo, SetPermissions(bo));
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = "ClaimRegisterClaim", Power = "U")]
        public override ActionResult Edit(int id, FormCollection form, ClaimRegisterViewModel model)
        {
            var dbBo = ClaimRegisterService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            if (!SetPermissions(dbBo))
            {
                SetErrorSessionMsg(string.Format(MessageBag.AccessDeniedWithName, "update"));
                return RedirectToAction("Edit", new { id = dbBo.Id });
            }

            if (ModelState.IsValid && !CheckCutOffReadOnly(Controller, id))
            {
                var bo = model.FormBo(AuthUserId, dbBo);

                var checklists = JsonConvert.DeserializeObject<Dictionary<string, string>>(bo.Checklist ?? "");
                if (checklists != null)
                {
                    var updatedChecklists = new Dictionary<string, string>(checklists);
                    foreach (KeyValuePair<string, string> checklist in checklists)
                    {
                        string valueStr = form.Get(checklist.Key);
                        if (string.IsNullOrEmpty(valueStr) && checklist.Key != ClaimChecklistDetailBo.RemarkCode)
                            continue;

                        updatedChecklists[checklist.Key] = valueStr;
                    }
                    bo.Checklist = JsonConvert.SerializeObject(updatedChecklists);
                }
                else
                {
                    bo.Checklist = null;
                }

                if (UpdateClaim(bo, model.ModuleId, dbBo))
                {
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
            }

            LoadClaimPage(ref model, dbBo, SetPermissions(dbBo));
            return View("Edit", model);
        }

        public ActionResult AssignCase(int id, int? userId)
        {
            if (CheckCutOffReadOnly(Controller, id))
            {
                return RedirectToAction("Edit", new { id });
            }

            bool hasAssignClaimPower = UserService.CheckPowerFlag(AuthUser, ModuleBo.ModuleController.ClaimRegisterClaim.ToString(), AccessMatrixBo.PowerAssignClaim);

            ClaimRegisterBo bo = ClaimRegisterService.Find(id);
            if (!hasAssignClaimPower && bo.PicClaimId.HasValue && bo.PicClaimId != AuthUserId)
            {
                SetErrorSessionMsg(string.Format(MessageBag.AccessDeniedWithActionName, "assign", "claim"));
                return RedirectToAction("Edit", new { id });
            }

            var trail = GetNewTrailObject();

            bo.PicClaimId = userId;
            bo.UpdatedById = AuthUserId;
            Result = ClaimRegisterService.Update(ref bo, ref trail);

            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Update Claim Register"
                );

                SetSuccessSessionMsg("Claim has been assigned successfully");
            }

            return RedirectToAction("Edit", new { id });
        }

        public override ActionResult UpdateStatus(int id, int status, FormCollection form, ClaimRegisterViewModel model)
        {
            var dbBo = ClaimRegisterService.Find(id);
            if (dbBo == null)
                return RedirectToAction("Index");

            if (CheckCutOffReadOnly(Controller, id))
                return RedirectToAction("Edit", new { id });

            bool isOverwrite = false;
            bool isValid = VerifyClaimStatusUpdate(dbBo, status, model, ref isOverwrite);
            bool hasUpdatePower = CheckPower("ClaimRegisterClaim", "U");
            if (ModelState.IsValid || (!hasUpdatePower && isValid))
            {
                bool hasCeoApprovalPower = CheckPower("ClaimRegisterClaim", AccessMatrixBo.PowerCeoApproval);
                bool hasApprovalOnBehalfCeoPower = CheckPower("ClaimRegisterClaim", AccessMatrixBo.PowerApprovalOnBehalfCeo);

                ClaimRegisterBo bo = hasUpdatePower ? model.FormBo(AuthUserId, dbBo) : dbBo;

                switch (status)
                {
                    case ClaimRegisterBo.StatusRegistered:
                        if (bo.ClaimStatus == ClaimRegisterBo.StatusPostUnderwritingReview && !hasUpdatePower)
                            bo.UnderwriterFeedback = model.UnderwriterFeedback;
                        if (bo.ClaimStatus == ClaimRegisterBo.StatusSuspectedDuplication)
                        {
                            if (string.IsNullOrEmpty(bo.ClaimId) && bo.ClaimTransactionType == PickListDetailBo.ClaimTransactionTypeNew)
                                bo.ClaimId = ClaimRegisterService.GetNextClaimId();
                            if (!bo.DateOfRegister.HasValue)
                                bo.DateOfRegister = DateTime.Now;
                        }
                        break;
                    case ClaimRegisterBo.StatusPostUnderwritingReview:
                        bo.RequestUnderwriterReview = true;
                        break;
                    case ClaimRegisterBo.StatusPendingCeoApproval:
                        if (Util.GetConfigBoolean("ExGratiaNotifyCeo"))
                        {
                            UserBo exGratiaCeoBo = UserService.FindByFullName(Util.GetConfig("ExGratiaCeoName"));
                            if (exGratiaCeoBo != null)
                            {
                                GetNewEmail(EmailBo.TypeClaimRegisterPendingApproval, exGratiaCeoBo.Email, exGratiaCeoBo.Id);
                                EmailBo.AddData(bo.ClaimId);
                                bool success = GenerateMail();

                                EmailBo emailBo = EmailBo;
                                Services.EmailService.Create(ref emailBo);
                            }
                            else
                                SetWarningSessionMsg("Ceo User not found in the system");
                        }
                        break;
                    case ClaimRegisterBo.StatusApprovedByCeo:
                        if (!hasCeoApprovalPower && hasApprovalOnBehalfCeoPower)
                        {
                            bo.UpdatedOnBehalfById = AuthUserId;
                            bo.UpdatedOnBehalfAt = DateTime.Now;
                        }
                        break;
                    case ClaimRegisterBo.StatusApprovalByLimit:
                        bo.PicClaimId = model.NextPicClaimId;
                        break;
                    case ClaimRegisterBo.StatusApproved:
                    case ClaimRegisterBo.StatusApprovedReferralClaim:
                        //bo.SignOff(true);
                        bo.ClearRedFlagWarning();
                        bo.DateApproved = DateTime.Today;
                        bo.ClaimDecisionStatus = isOverwrite ? ClaimRegisterBo.ClaimDecisionStatusApprovedOverwrite : ClaimRegisterBo.ClaimDecisionStatusApproved;
                        break;
                    case ClaimRegisterBo.StatusDeclinedReferralClaim:
                    case ClaimRegisterBo.StatusDeclinedByClaim:
                        bo.ClaimDecisionStatus = ClaimRegisterBo.ClaimDecisionStatusDeclined;
                        break;
                    case ClaimRegisterBo.StatusDeclined:
                        if (bo.ClaimStatus == ClaimRegisterBo.StatusRegistered)
                        {
                            bo.ClaimReasonId = ClaimReasonService.FindByReason(ClaimReasonBo.TypeClaimDeclinePending, ClaimReasonBo.StaticReasonDuplicateClaim)?.Id;
                            bo.ClaimDecisionStatus = ClaimRegisterBo.ClaimDecisionStatusDeclined;
                        }
                        if (bo.OffsetStatus != ClaimRegisterBo.OffsetStatusPending)
                            bo.AddRedFlagWarning(ClaimRegisterBo.RedFlagCreateAdjustment.ToString());
                        //bo.SignOff();
                        break;
                    case ClaimRegisterBo.StatusClosedReferralClaim:
                        bo.ReverseClaim();
                        break;
                    default:
                        break;

                }

                bo.ClaimStatus = status;
                if (UpdateClaim(bo, model.ModuleId, dbBo, true))
                {
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
            }

            LoadClaimPage(ref model, dbBo, SetPermissions(dbBo));
            return View("Edit", model);
        }

        public ActionResult LinkReferral(int id, FormCollection form, ClaimRegisterViewModel model)
        {
            var dbBo = ClaimRegisterService.Find(id);
            if (dbBo == null)
                return RedirectToAction("Index");

            if (CheckCutOffReadOnly(Controller, id))
                return RedirectToAction("Edit", new { id });

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, dbBo);

                ReferralClaimBo referralClaimBo = ReferralClaimService.FindForClaimRegisterLink(model.TreatyCode, model.InsuredName, model.ClaimCode);
                if (referralClaimBo != null)
                {
                    bo.ReferralClaimId = referralClaimBo.Id;
                }
                else
                {
                    SetWarningSessionMsg("No Referral Claim found");
                }

                if (UpdateClaim(bo, model.ModuleId, dbBo, true))
                {
                    if (referralClaimBo != null)
                    {
                        referralClaimBo.TreatyShare = bo.MlreShare;
                        referralClaimBo.UpdatedById = AuthUserId;

                        var trail = GetNewTrailObject();
                        Result = ReferralClaimService.Update(ref referralClaimBo, ref trail);
                        if (Result.Valid)
                        {
                            CreateTrail(
                                bo.Id,
                                "Update Referral Claim"
                            );
                        }
                    }

                    return RedirectToAction("Edit", new { id = bo.Id });
                }
            }

            LoadClaimPage(ref model, dbBo, SetPermissions(dbBo));
            return View("Edit", model);
        }

        public bool VerifyClaimStatusUpdate(ClaimRegisterBo bo, int nextStatus, ClaimRegisterViewModel model, ref bool isOverwrite)
        {
            Dictionary<string, int> statuses = new Dictionary<string, int>();
            if (!VerifyStatusUpdate(bo, nextStatus, ref statuses))
                return false;

            string error = null;
            switch (nextStatus)
            {
                case ClaimRegisterBo.StatusRegistered:
                    if (bo.ClaimStatus == ClaimRegisterBo.StatusPostUnderwritingReview && !model.UnderwriterFeedback.HasValue)
                        error = "Please choose an option before completing";
                    break;
                case ClaimRegisterBo.StatusApprovalByLimit:
                    if (!model.NextPicClaimId.HasValue)
                        error = "You are required to assign this case to another person in-charge for approval";
                    break;
                case ClaimRegisterBo.StatusApproved:
                case ClaimRegisterBo.StatusApprovedReferralClaim:
                    isOverwrite = statuses.FirstOrDefault(x => x.Value == nextStatus).Key == "Overwrite";
                    break;
                default:
                    break;
            }

            if (string.IsNullOrEmpty(error))
                return true;

            ModelState.AddModelError("", error);
            return false;
        }

        public ActionResult DownloadExGratia(
            string CedingCompany,
            string InsuredName,
            string ClaimId,
            string InsuredDateOfBirthStr,
            string InsuredGenderCode,
            string ClaimTransactionType,
            string PolicyNumber,
            string ReinsEffDatePolStr,
            string DateOfEventStr,
            string MlreBenefitCode,
            string TreatyCode,
            string CauseOfEvent,
            string ClaimRecoveryAmtStr,
            string ClaimAssessorRecommendation,
            string EventChronologyComment,
            string ClaimCommitteeComment1,
            string ClaimCommitteeComment2,
            string ClaimCommitteeUser1,
            string ClaimCommitteeUser2,
            string CeoComment)
        {
            MemoryStream s = new MemoryStream();
            Pdf document = new Pdf(Server.MapPath("~/Document/ExGratia_Template.pdf"));
            document.TextBoxHeight = 15;

            document.CreateRequestedDate(DateTime.Today.ToString(Util.GetDateFormat()), false);
            document.AddVerticalSpace(5);

            DateTime? insuredDateOfBirth = Util.GetParseDateTime(InsuredDateOfBirthStr);
            string ageNextBirthStr = null;
            if (insuredDateOfBirth.HasValue)
            {
                //DateTime yearBirthday = new DateTime(DateTime.Today.Year, insuredDateOfBirth.Value.Month, insuredDateOfBirth.Value.Day);
                DateTime today = DateTime.Today;
                int age = today.Year - insuredDateOfBirth.Value.Year;
                if (insuredDateOfBirth.Value.Month < today.Month || (insuredDateOfBirth.Value.Month == today.Month && insuredDateOfBirth.Value.Day < today.Day))
                {
                    age++;
                }
                ageNextBirthStr = age.ToString();
            }

            document.SectionHeader("DETAILS");
            document.AddTextBox("Ceding Company", CedingCompany == "null" ? null : CedingCompany, 130);
            document.AddTextBox("Insured Name", InsuredName, 130);
            document.AddTextBox("Claim ID", ClaimId, 130, inputLength: 150, newLine: false);
            document.AddTextBox("Claim Type", ClaimTransactionType == "null" ? null : ClaimTransactionType, labelLength: 80, inputLength: 150);
            document.AddTextBox("Date of Birth", InsuredDateOfBirthStr, 130, inputLength: 150, newLine: false);
            document.AddTextBox("Age Next Birth", ageNextBirthStr, labelLength: 80, inputLength: 150);
            document.AddTextBox("Gender", InsuredGenderCode == "null" ? null : InsuredGenderCode, 130, inputLength: 150, newLine: false);
            document.AddTextBox("Policy No", PolicyNumber, labelLength: 80, inputLength: 150);
            document.AddTextBox("Date of Commencement", ReinsEffDatePolStr, 130, inputLength: 150, newLine: false);
            document.AddTextBox("Date of Event", DateOfEventStr, labelLength: 80, inputLength: 150);
            document.AddTextBox("Benefit Code", MlreBenefitCode == "null" ? null : MlreBenefitCode, 130, inputLength: 150, newLine: false);
            document.AddTextBox("TreatyCode", TreatyCode == "null" ? null : TreatyCode, labelLength: 80, inputLength: 150);
            document.AddTextBox("Cause of Event / Diagnosis", CauseOfEvent, 130);
            document.AddTextBox("Claim Amount (RM)", ClaimRecoveryAmtStr, 130);
            document.AddVerticalSpace(5);

            document.SectionHeader("COMMENTS & RECOMMENDATION");
            document.AddText("Comments on Chronology of Events", false);
            document.AddVerticalSpace(5);
            document.AddTextBox(input: EventChronologyComment, inputLength: 510, rows: 2);
            document.AddText("Recommendation by Claim Assessor", false);
            document.AddVerticalSpace(5);
            document.AddTextBox(input: ClaimAssessorRecommendation, inputLength: 510, rows: 2);
            document.AddVerticalSpace(5);

            document.SectionHeader("APPROVAL BY CLAIMS COMMITTEE");
            document.AddText("Comments", false);
            document.AddVerticalSpace(5);
            document.AddTextBox(input: ClaimCommitteeComment1, inputLength: 300, rows: 4, newLine: false);
            document.SignatureSection(labelLength: 80, fontSize: 8, textHeight: 20, dateFieldName: "Date Sign-Off", name: ClaimCommitteeUser1 == "null" ? null : ClaimCommitteeUser1, height: 20);
            document.AddVerticalSpace(5);
            document.DrawLine();
            document.AddText("Comments", false);
            document.AddVerticalSpace(5);
            document.AddTextBox(input: ClaimCommitteeComment2, inputLength: 300, rows: 4, newLine: false);
            document.SignatureSection(labelLength: 80, fontSize: 8, textHeight: 20, dateFieldName: "Date Sign-Off", name: ClaimCommitteeUser2 == "null" ? null : ClaimCommitteeUser2, height: 20);
            document.AddVerticalSpace(5);
            document.DrawLine();
            document.AddText("Comments", false);
            document.AddVerticalSpace(5);
            document.AddTextBox(input: CeoComment, inputLength: 300, rows: 4, newLine: false);
            document.SignatureSection(labelLength: 80, fontSize: 8, textHeight: 20, dateFieldName: "Date Sign-Off", name: Util.GetConfig("ExGratiaCeoName"), height: 20);

            document.Document.Save(s);


            var fileName = "Ex Gratia.pdf";
            var contentType = "application/pdf";

            return File(s, contentType, fileName);
        }

        // Overwrite Functions
        public override IQueryable<ClaimRegisterViewModel> GetQuery()
        {
            var query = _db.GetClaimRegister(true).Select(ClaimRegisterViewModel.Expression());

            if (AuthUser.DepartmentId == DepartmentBo.DepartmentUnderwriting)
                query = query.Where(q => q.ClaimStatus == ClaimRegisterBo.StatusPostUnderwritingReview);

            return query;
        }

        public override bool IsClaimOnly()
        {
            return true;
        }

        public override void IndexPage()
        {
            base.IndexPage();
            DropDownClaimStatus(true);

            ViewBag.DropDownNextAction = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Submit for Approval Limit", Value = "approval", Selected = true },
                new SelectListItem() { Text = "Overwrite Approval", Value = "overwrite" }
            };
        }

        // Load Data
        public void LoadClaimPage(ref ClaimRegisterViewModel model, ClaimRegisterBo bo = null, bool genActions = true)
        {
            LoadShared(ref model, genActions, bo);

            DropDownChecklistStatus();
            DropDownRecordType(true);
            DropDownAuthorityLimitClaimUser(model.ClaimCode, model.ClaimRecoveryAmt);
            ViewBag.DropDownClaimUsers = DropDownPicClaim(UserBo.StatusActive);
            ViewBag.DropDownSignOffUsers = DropDownUserByDept(UserBo.StatusActive, model.SignOffById, new List<int> { DepartmentBo.DepartmentClaim, DepartmentBo.DepartmentCEO });
            DropDownClaimAssessor(UserBo.StatusActive, model.ClaimAssessorId);
            DropDownClaimReason(ClaimReasonBo.TypeClaimDeclinePending);
            DropDownPicClaim(UserBo.StatusActive, model.PicClaimId);

            // Related Claims
            LoadRelatedClaims(model);

            // Underwriting
            ViewBag.UnderwritingRemarks = RemarkService.GetBySubModule(model.ModuleId, model.Id, ClaimRegisterBo.SubModuleUnderwriting);

            // Checklist
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
                    if (!checklistValue.HasValue && checklistValue.Value != ClaimRegisterBo.ChecklistStatusPending)
                        continue;

                    checklistErrors.Add(checklist.Key);
                }
                ViewBag.ChecklistErrors = checklistErrors;
            }
            ViewBag.Checklists = checklists;

            SetViewBagMessage();
        }

        public void DropDownAuthorityLimitClaimUser(string claimCode, double? claimRecoveryAmt)
        {
            var items = GetEmptyDropDownList();
            foreach (var user in UserService.GetByAuthorityLimit(claimCode, claimRecoveryAmt))
            {
                items.Add(new SelectListItem { Text = user.FullName, Value = user.Id.ToString() });
            }

            ViewBag.HasAuthorityLimitClaimUsers = items.Count > 1 ? true : false;
            ViewBag.DropDownAuthorityLimitClaimUsers = items;
        }

        public void DropDownClaimIds()
        {
            var items = GetEmptyDropDownList();
            Dictionary<int, string> claimIds = ClaimRegisterService.GetClaimIds();
            foreach (var claimId in claimIds.GroupBy(c => c.Value).Select(c => c.First()).ToDictionary(c => c.Key, c => c.Value))
            {
                items.Add(new SelectListItem { Value = claimId.Key.ToString(), Text = claimId.Value });
            }

            ViewBag.DropDownClaimIds = items;
        }

        public void LoadRelatedClaims(ClaimRegisterViewModel model)
        {
            List<ClaimRegisterBo> relatedClaimBos = new List<ClaimRegisterBo>();

            if (model.Id != 0)
            {
                relatedClaimBos = ClaimRegisterService.GetRelatedClaims(model.Id).ToList();
                if (model.ReferralClaimId.HasValue)
                {
                    relatedClaimBos.Add(ReferralClaimService.FindAsClaimRegister(model.ReferralClaimId.Value));
                }
            }

            ViewBag.RelatedClaims = relatedClaimBos;
        }

        public override Dictionary<string, int> GetPossibleNextStatus(int currentStatus, int duplicationCheckStatus, string recordType, bool isReferralCase = false, string claimCode = null, double? claimRecoveryAmt = null)
        {
            ViewBag.CanUpdateUnderwritingFeedback = false;
            Dictionary<string, int> statuses = new Dictionary<string, int>();

            bool isSkipApprovalByLimit = false;
            bool isAllowOverwriteApproval = false;
            if (!string.IsNullOrEmpty(claimCode) && claimRecoveryAmt.HasValue)
            {
                ClaimAuthorityLimitMLReBo authorityLimitBo = ClaimAuthorityLimitMLReService.FindByUserId(AuthUserId);
                if (authorityLimitBo != null)
                {
                    isAllowOverwriteApproval = authorityLimitBo.IsAllowOverwriteApproval;
                    ClaimAuthorityLimitMLReDetailBo authorityLimitDetailBo = ClaimAuthorityLimitMLReDetailService.FindByClaimAuthorityLimitMLReIdClaimCode(authorityLimitBo.Id, claimCode);
                    if (authorityLimitDetailBo != null)
                    {
                        isSkipApprovalByLimit = claimRecoveryAmt <= authorityLimitDetailBo.ClaimAuthorityLimitValue;
                    }
                }
            }

            bool hasCeoApprovalPower = CheckPower("ClaimRegisterClaim", AccessMatrixBo.PowerCeoApproval);
            bool hasApprovalOnBehalfCeoPower = CheckPower("ClaimRegisterClaim", AccessMatrixBo.PowerApprovalOnBehalfCeo);

            switch (currentStatus)
            {
                case ClaimRegisterBo.StatusSuspectedDuplication:
                    statuses.Add("SUBMIT FOR CLARIFICATION", ClaimRegisterBo.StatusPendingClarification);
                    statuses.Add("SUBMIT FOR ASSESSMENT", ClaimRegisterBo.StatusRegistered);
                    break;
                case ClaimRegisterBo.StatusRegistered:
                    statuses.Add("DUPLICATE CLAIM", ClaimRegisterBo.StatusDeclined);
                    statuses.Add("Submit for Post Underwriting Review", ClaimRegisterBo.StatusPostUnderwritingReview);
                    if (recordType == PickListDetailBo.RecordTypeExGratia)
                        statuses.Add("Submit for CEO Approval", ClaimRegisterBo.StatusPendingCeoApproval);
                    if (!isSkipApprovalByLimit)
                        statuses.Add("Submit for Approval by Limit", ClaimRegisterBo.StatusApprovalByLimit);
                    if (isSkipApprovalByLimit || isAllowOverwriteApproval)
                    {
                        string approveText = (!isSkipApprovalByLimit && isAllowOverwriteApproval) ? "Overwrite" : "Approve";
                        ClaimRegisterBo.GetApproveRejectUrls(ref statuses, isReferralCase, approveText, "Reject");
                    }
                    break;
                case ClaimRegisterBo.StatusPendingCeoApproval:
                    if (hasCeoApprovalPower)
                        statuses.Add("APPROVE", ClaimRegisterBo.StatusApprovedByCeo);
                    else if (hasApprovalOnBehalfCeoPower)
                        statuses.Add("APPROVE ON BEHALF OF CEO", ClaimRegisterBo.StatusApprovedByCeo);
                    break;
                case ClaimRegisterBo.StatusApprovedByCeo:
                    if (hasCeoApprovalPower || hasApprovalOnBehalfCeoPower)
                    {
                        if (!isSkipApprovalByLimit)
                            statuses.Add("APPROVE", ClaimRegisterBo.StatusApprovalByLimit);
                        else
                            ClaimRegisterBo.GetApproveRejectUrls(ref statuses, isReferralCase);
                    }
                    break;
                case ClaimRegisterBo.StatusApprovalByLimit:
                    statuses.Add("SUBMIT FOR ASSESSMENT", ClaimRegisterBo.StatusRegistered);
                    if (isSkipApprovalByLimit || isAllowOverwriteApproval)
                    {
                        string approveText = (!isSkipApprovalByLimit && isAllowOverwriteApproval) ? "Overwrite" : "Approve";
                        ClaimRegisterBo.GetApproveRejectUrls(ref statuses, isReferralCase, approveText, "Reject");
                    }
                    break;
                case ClaimRegisterBo.StatusApprovedReferralClaim:
                case ClaimRegisterBo.StatusDeclinedReferralClaim:
                    statuses.Add("CLOSE CLAIM", ClaimRegisterBo.StatusClosedReferralClaim);
                    break;
                case ClaimRegisterBo.StatusDeclinedByClaim:
                    statuses.Add("SUBMIT FOR CEO SIGN OFF", ClaimRegisterBo.StatusPendingCeoSignOff);
                    statuses.Add("SUBMIT FOR ASSESSMENT", ClaimRegisterBo.StatusRegistered);
                    break;
                case ClaimRegisterBo.StatusPendingCeoSignOff:
                    if (hasCeoApprovalPower)
                        statuses.Add("SIGN OFF", ClaimRegisterBo.StatusDeclined);
                    else if (hasApprovalOnBehalfCeoPower)
                        statuses.Add("SIGN OFF ON BEHALF OF CEO", ClaimRegisterBo.StatusDeclined);
                    break;
                case ClaimRegisterBo.StatusPostUnderwritingReview:
                    if (CheckPower("ClaimRegisterClaim", AccessMatrixBo.PowerUnderwritingFeedback))
                    {
                        ViewBag.CanUpdateUnderwritingFeedback = true;
                        statuses.Add("COMPLETE", ClaimRegisterBo.StatusRegistered);
                    }
                    break;
                default:
                    break;
            }

            return statuses;
        }

        public bool SetPermissions(ClaimRegisterBo bo)
        {
            bool canAssign = false;
            int authUserId = AuthUserId;

            bool hasAssignClaimPower = UserService.CheckPowerFlag(AuthUser, ModuleBo.ModuleController.ClaimRegisterClaim.ToString(), AccessMatrixBo.PowerAssignClaim);
            bool canEdit = CheckPower(ModuleBo.ModuleController.ClaimRegisterClaim.ToString(), "U");
            bool isPicClaimUser = bo.PicClaimBo != null && bo.PicClaimBo.DepartmentId == DepartmentBo.DepartmentClaim;

            if (!bo.PicClaimId.HasValue || !isPicClaimUser || bo.PicClaimId == authUserId || hasAssignClaimPower)
            {
                canAssign = true;
            }

            if (ViewBag.ReadOnly == false && !canEdit)
                ViewBag.ReadOnly = true;

            ViewBag.CanAssign = canAssign;

            return canEdit || bo.ClaimStatus == ClaimRegisterBo.StatusPostUnderwritingReview;
        }

        // Batch Functions
        public JsonResult ValidateUpdateBatchStatus(string SelectedIds, int Status)
        {
            bool success = true;
            bool requireNextPic = false;
            string error = null;

            if (string.IsNullOrEmpty(SelectedIds))
            {
                success = false;
                error = "No Claims Selected";
                return Json(new { success, error });
            }

            List<int> statuses = new List<int>() { ClaimRegisterBo.StatusRegistered, ClaimRegisterBo.StatusApprovedByCeo, ClaimRegisterBo.StatusApprovalByLimit };

            ClaimAuthorityLimitMLReBo authorityLimitBo = ClaimAuthorityLimitMLReService.FindByUserId(AuthUserId);
            bool isAllowOverwriteApproval = authorityLimitBo.IsAllowOverwriteApproval;
            Dictionary<string, double> authorityLimits = new Dictionary<string, double>();
            if (authorityLimitBo != null)
            {
                authorityLimits = ClaimAuthorityLimitMLReDetailService.GetLimitsByClaimAuthorityLimitMlreId(authorityLimitBo.Id);
            }

            bool hasCeoApprovalPower = CheckPower("ClaimRegisterClaim", AccessMatrixBo.PowerCeoApproval);


            Dictionary<string, double> limits = new Dictionary<string, double>();

            List<int> ids = SelectedIds.Split(',').Select(int.Parse).ToList();
            var bos = ClaimRegisterService.GetByIds(ids);
            foreach (ClaimRegisterBo bo in bos)
            {
                success = bo.ValidateApproveReject(AuthUserId, hasCeoApprovalPower);
                if (!success)
                    continue;

                if (Status == ClaimRegisterBo.StatusDeclinedByClaim)
                    continue;

                List<string> emptyValues = new List<string>();
                if (string.IsNullOrEmpty(bo.ClaimCode))
                    emptyValues.Add("Claim Code");
                if (!bo.ClaimRecoveryAmt.HasValue)
                    emptyValues.Add("Claim Amount");

                if (!emptyValues.IsNullOrEmpty())
                {
                    success = false;
                    bo.BatchSelectionError = string.Format("{0} is empty", string.Join(" And ", emptyValues));
                }

                bool hasApprovalLimit = false;
                if (authorityLimits.ContainsKey(bo.ClaimCode))
                    hasApprovalLimit = bo.ClaimRecoveryAmt <= authorityLimits[bo.ClaimCode];

                if (!hasApprovalLimit)
                {
                    if (bo.ClaimRecoveryAmt.HasValue && (!limits.ContainsKey(bo.ClaimCode) || limits[bo.ClaimCode] < bo.ClaimRecoveryAmt.Value))
                    {
                        limits[bo.ClaimCode] = bo.ClaimRecoveryAmt.Value;
                    }
                    requireNextPic = true;
                    bo.BatchSelectionError = "Claim Amount exceeds your authorised limit for this claim code";
                }
            }

            List<UserBo> nextPicUsers = new List<UserBo>();
            if (requireNextPic)
            {
                List<int> nextPicUserIds = ClaimAuthorityLimitMLReDetailService.GetAuthorisedUserByMultipleLimits(limits);
                nextPicUsers = UserService.GetByIds(nextPicUserIds).ToList();
            }

            return Json(new { success, error, bos, requireNextPic, nextPicUsers, isAllowOverwriteApproval });
        }

        [Auth(Controller = "ClaimRegisterClaim", Power = "U")]
        public ActionResult UpdateBatch(string SelectedIds, int Status, bool IsAllowOverwriteApproval, int? AuthorisedPicClaimId = null)
        {
            if (CheckCutOffReadOnly(Controller))
                return Index(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 1, true);

            List<int> ids = null;
            if (!string.IsNullOrEmpty(SelectedIds))
                ids = SelectedIds.Split(',').Select(Int32.Parse).ToList();

            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.ClaimRegister.ToString());

            int success = 0;
            List<int> statuses = new List<int>() { ClaimRegisterBo.StatusRegistered, ClaimRegisterBo.StatusApprovedByCeo, ClaimRegisterBo.StatusApprovalByLimit };
            foreach (int id in ids)
            {
                ClaimRegisterBo bo = ClaimRegisterService.Find(id);
                if (bo == null)
                    continue;

                if (!statuses.Contains(bo.ClaimStatus))
                    continue;

                int status = Status;
                bool isSkipApprovalByLimit = false;
                if (!string.IsNullOrEmpty(bo.ClaimCode) && bo.ClaimRecoveryAmt.HasValue)
                {
                    ClaimAuthorityLimitMLReBo authorityLimitBo = ClaimAuthorityLimitMLReService.FindByUserId(AuthUserId);
                    if (authorityLimitBo != null)
                    {
                        ClaimAuthorityLimitMLReDetailBo authorityLimitDetailBo = ClaimAuthorityLimitMLReDetailService.FindByClaimAuthorityLimitMLReIdClaimCode(authorityLimitBo.Id, bo.ClaimCode);
                        if (authorityLimitDetailBo != null)
                        {
                            isSkipApprovalByLimit = bo.ClaimRecoveryAmt <= authorityLimitDetailBo.ClaimAuthorityLimitValue;
                        }
                    }
                }

                bool isReferralCase = bo.IsReferralCase;
                bool isExGratia = bo.RecordType == PickListDetailBo.RecordTypeExGratia;

                if (status == ClaimRegisterBo.StatusApproved)
                {
                    if (bo.ClaimStatus == ClaimRegisterBo.StatusRegistered && isExGratia)
                    {
                        status = ClaimRegisterBo.StatusApprovedByCeo;
                    }
                    if (!isSkipApprovalByLimit && !IsAllowOverwriteApproval)
                    {
                        if (AuthorisedPicClaimId.HasValue)
                            bo.PicClaimId = AuthorisedPicClaimId;
                        else
                            continue;
                        status = ClaimRegisterBo.StatusApprovalByLimit;
                    }
                    else
                    {
                        bo.ClearRedFlagWarning();
                        bo.SignOffById = AuthUserId;
                        bo.SignOffDate = DateTime.Today;
                        bo.DateApproved = DateTime.Today;
                        bo.ClaimDecisionStatus = (!isSkipApprovalByLimit && IsAllowOverwriteApproval) ? ClaimRegisterBo.ClaimDecisionStatusApprovedOverwrite : ClaimRegisterBo.ClaimDecisionStatusApproved;
                        status = isReferralCase ? ClaimRegisterBo.StatusApprovedReferralClaim : ClaimRegisterBo.StatusApproved;
                    }
                }
                else if (status == ClaimRegisterBo.StatusDeclinedByClaim)
                {
                    bo.ClaimDecisionStatus = ClaimRegisterBo.ClaimDecisionStatusDeclined;
                    if (isReferralCase)
                    {
                        status = ClaimRegisterBo.StatusDeclinedReferralClaim;
                    }
                    else
                    {
                        status = ClaimRegisterBo.StatusDeclinedByClaim;
                    }
                }
                else
                {
                    continue;
                }

                var trail = GetNewTrailObject();
                bo.ClaimStatus = status;
                Result = ClaimRegisterService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    StatusHistoryController.Add(moduleBo.Id, id, status, AuthUserId, ref trail);

                    CreateTrail(
                        bo.Id,
                        "Update Claim Register Status"
                    );

                    success++;
                }
            }

            SetSuccessSessionMsg(string.Format("Total Claims Updated: {0}", success));

            return Index(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 1, true);
        }

        public JsonResult ValidateBatchAssign(string SelectedIds)
        {
            bool success = true;
            string error = null;

            if (string.IsNullOrEmpty(SelectedIds))
            {
                success = false;
                error = "No Claims Selected";
                return Json(new { success, error });
            }

            List<int> ids = SelectedIds.Split(',').Select(int.Parse).ToList();
            var bos = ClaimRegisterService.GetByIds(ids);
            if (!UserService.CheckPowerFlag(AuthUser, ModuleBo.ModuleController.ClaimRegisterClaim.ToString(), AccessMatrixBo.PowerAssignClaim))
            {
                foreach (ClaimRegisterBo bo in bos)
                {
                    if (bo.PicClaimId.HasValue && bo.PicClaimId != AuthUserId)
                    {
                        success = false;
                        bo.BatchSelectionError = "You are not authorised to assign this claim";
                    }
                }
            }


            return Json(new { success, error, bos });
        }

        [Auth(Controller = "ClaimRegisterClaim", Power = "U")]
        public ActionResult AssignBatch(string SelectedIds, int NextPicClaimId)
        {
            if (CheckCutOffReadOnly(Controller))
                return Index(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 1, true);

            List<int> ids = new List<int>();
            if (!string.IsNullOrEmpty(SelectedIds))
                ids = SelectedIds.Split(',').Select(Int32.Parse).ToList();

            var bos = ClaimRegisterService.GetByIds(ids);

            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.ClaimRegister.ToString());

            bool hasAssignClaimPower = UserService.CheckPowerFlag(AuthUser, ModuleBo.ModuleController.ClaimRegisterClaim.ToString(), AccessMatrixBo.PowerAssignClaim);

            int success = 0;
            foreach (var bo in bos)
            {
                if (bo.PicClaimId.HasValue)
                {
                    if (bo.PicClaimId == NextPicClaimId)
                        continue;

                    if (!hasAssignClaimPower && bo.PicClaimId != AuthUserId)
                        continue;
                }

                var claimRegisterBo = bo;
                var trail = GetNewTrailObject();

                claimRegisterBo.PicClaimId = NextPicClaimId;
                Result = ClaimRegisterService.Update(ref claimRegisterBo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Update Claim Register"
                    );

                    success++;
                }
            }

            SetSuccessSessionMsg(string.Format("Total Claims Assigned: {0}", success));

            return Index(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 1, true);
        }

        // Load Data
        public JsonResult NotifyClaimCommittee(string claimId, int? userId1 = null, int? userId2 = null)
        {
            List<int?> userIds = new List<int?>() { userId1, userId2 };
            List<string> messages = new List<string>();

            for (int i = 0; i < 2; i++)
            {
                int? userId = userIds[i];
                if (!userId.HasValue)
                {
                    //messages.Insert(i, "User not selected");
                    continue;
                }

                UserBo userBo = UserService.Find(userId);
                if (userBo == null)
                {
                    messages.Insert(i, "User not found");
                    continue;
                }

                GetNewEmail(EmailBo.TypeClaimRegisterPendingComment, userBo.Email, userBo.Id);
                EmailBo.AddData(claimId);

                if (GenerateMail(showWarning: false))
                    messages.Insert(i, string.Format("Notification Successfully Sent To: {0}", userBo.UserName));
                else
                    messages.Insert(i, MessageBag.EmailError);

                EmailBo bo = EmailBo;
                Services.EmailService.Create(ref bo);
            }

            return Json(new { messages = messages.ToList() });
        }
    }
}