using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using ConsoleApp.Commands;
using ConsoleApp.Commands.ProcessDatas;
using Newtonsoft.Json;
using PagedList;
using Services;
using Services.Identity;
using Services.TreatyPricing;
using Shared;
using Shared.Trails;
using System;
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
    public class TreatyPricingQuotationWorkflowController : BaseController
    {
        public const string Controller = "TreatyPricingQuotationWorkflow";

        // GET: TreatyPricingQuotationWorkflow
        [Auth(Controller = Controller, Power = "R")]
        public virtual ActionResult Index(
            string QuotationId,
            string CreatedAt,
            int? CedantId,
            int? ReinsuranceTypePickListDetailId,
            string Name,
            string Description,
            int? LatestVersion,
            int? Status,
            int? PricingStatus,
            int? PricingTeamPickListDetailId,
            int? BDPersonInChargeId,
            int? PersonInChargeId,
            int? InternalTeam,
            string IdList,
            string SortOrder,
            int? FromQuotationDashboard,
            int? Page)
        {
            DateTime? createdAt = Util.GetParseDateTime(CreatedAt);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["QuotationId"] = QuotationId,
                ["CreatedAt"] = createdAt.HasValue ? CreatedAt : null,
                ["CedantId"] = CedantId,
                ["ReinsuranceTypePickListDetailId"] = ReinsuranceTypePickListDetailId,
                ["Name"] = Name,
                ["Description"] = Description,
                ["LatestVersion"] = LatestVersion,
                ["Status"] = Status,
                ["PricingStatus"] = PricingStatus,
                ["PricingTeamPickListDetailId"] = PricingTeamPickListDetailId,
                ["BDPersonInChargeId"] = BDPersonInChargeId,
                ["PersonInChargeId"] = PersonInChargeId,
                ["InternalTeam"] = InternalTeam,
                ["IdList"] = IdList,
                ["FromQuotationDashboard"] = FromQuotationDashboard,
                ["SortOrder"] = SortOrder,
            };

            ViewBag.SortOrder = SortOrder;
            ViewBag.SortQuotationId = GetSortParam("QuotationId");
            ViewBag.SortCreatedAt = GetSortParam("CreatedAt");
            ViewBag.SortCedantId = GetSortParam("CedantId");
            ViewBag.SortReinsuranceTypePickListDetailId = GetSortParam("ReinsuranceTypePickListDetailId");
            ViewBag.SortName = GetSortParam("Name");
            ViewBag.SortDescription = GetSortParam("Description");
            ViewBag.SortLatestVersion = GetSortParam("LatestVersion");
            ViewBag.SortStatus = GetSortParam("Status");
            ViewBag.SortPricingStatus = GetSortParam("PricingStatus");
            ViewBag.SortPricingTeamPickListDetailId = GetSortParam("PricingTeamPickListDetailId");
            ViewBag.SortBDPersonInChargeId = GetSortParam("BDPersonInChargeId");
            ViewBag.SortPersonInChargeId = GetSortParam("PersonInChargeId");

            var query = _db.TreatyPricingQuotationWorkflows.Select(TreatyPricingQuotationWorkflowViewModel.Expression());
            if (!string.IsNullOrEmpty(QuotationId)) query = query.Where(q => q.QuotationId.Contains(QuotationId));

            if (createdAt.HasValue) query = query.Where(q => q.CreatedAt == createdAt);
            if (CedantId.HasValue) query = query.Where(q => q.CedantId == CedantId);
            if (ReinsuranceTypePickListDetailId.HasValue) query = query.Where(q => q.ReinsuranceTypePickListDetailId == ReinsuranceTypePickListDetailId);
            if (!string.IsNullOrEmpty(Name)) query = query.Where(q => q.Name == Name);
            if (!string.IsNullOrEmpty(Description)) query = query.Where(q => q.Description == Description);
            if (LatestVersion.HasValue) query = query.Where(q => q.LatestVersion == LatestVersion);
            if (Status.HasValue) query = query.Where(q => q.Status == Status);
            if (PricingStatus.HasValue) query = query.Where(q => q.PricingStatus == PricingStatus);
            if (PricingTeamPickListDetailId.HasValue) query = query.Where(q => q.PricingTeamPickListDetailId == PricingTeamPickListDetailId);
            if (BDPersonInChargeId.HasValue) {
                if (BDPersonInChargeId.Value == 0)
                {
                    query = query.Where(q => q.BDPersonInChargeId == null);
                }
                else
                {
                    query = query.Where(q => q.BDPersonInChargeId == BDPersonInChargeId);
                }
            }
            if (PersonInChargeId.HasValue)
            {
                if (PersonInChargeId.Value == 0)
                {
                    query = query.Where(q => q.PersonInChargeId == null);
                }
                else
                {
                    query = query.Where(q => q.PersonInChargeId == PersonInChargeId);
                }
            }

            if (!string.IsNullOrEmpty(IdList))
            {
                var idList = Array.ConvertAll(Util.ToArraySplitTrim(IdList), int.Parse);
                query = query.Where(q => idList.Contains(q.Id));
            }

            int statusApproved = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.StatusApproved;

            ViewBag.StatusApproved = statusApproved;
            ViewBag.FromQuotationDashboard = FromQuotationDashboard;

            if (FromQuotationDashboard == 1)
            {
                if (InternalTeam.HasValue && InternalTeam.Value == TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamCEO) query = query.Where(q => q.CEOPending > 1);
                if (InternalTeam.HasValue && InternalTeam.Value == TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamPricing) query = query.Where(q => q.PricingPending > 1);
                if (InternalTeam.HasValue && InternalTeam.Value == TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamUnderwriting) query = query.Where(q => q.UnderwritingPending > 1);
                if (InternalTeam.HasValue && InternalTeam.Value == TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamHealth) query = query.Where(q => q.HealthPending > 1);
                if (InternalTeam.HasValue && InternalTeam.Value == TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamClaims) query = query.Where(q => q.ClaimsPending > 1);
                if (InternalTeam.HasValue && InternalTeam.Value == TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamBD) query = query.Where(q => q.BDPending > 1);
                if (InternalTeam.HasValue && InternalTeam.Value == TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamCR) query = query.Where(q => q.TGPending > 1);
            }
            else
            {
                if (InternalTeam.HasValue && InternalTeam.Value == TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamCEO) query = query.Where(q => q.CEOPending > 1 && q.CEOPending != statusApproved);
                if (InternalTeam.HasValue && InternalTeam.Value == TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamPricing) query = query.Where(q => q.PricingPending > 1 && q.PricingPending != statusApproved);
                if (InternalTeam.HasValue && InternalTeam.Value == TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamUnderwriting) query = query.Where(q => q.UnderwritingPending > 1 && q.UnderwritingPending != statusApproved);
                if (InternalTeam.HasValue && InternalTeam.Value == TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamHealth) query = query.Where(q => q.HealthPending > 1 && q.HealthPending != statusApproved);
                if (InternalTeam.HasValue && InternalTeam.Value == TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamClaims) query = query.Where(q => q.ClaimsPending > 1 && q.ClaimsPending != statusApproved);
                if (InternalTeam.HasValue && InternalTeam.Value == TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamBD) query = query.Where(q => q.BDPending > 1 && q.BDPending != statusApproved);
                if (InternalTeam.HasValue && InternalTeam.Value == TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamCR) query = query.Where(q => q.TGPending > 1 && q.TGPending != statusApproved);
            }

            if (SortOrder == Html.GetSortAsc("QuotationId")) query = query.OrderBy(q => q.QuotationId);
            else if (SortOrder == Html.GetSortDsc("QuotationId")) query = query.OrderByDescending(q => q.QuotationId);
            else if (SortOrder == Html.GetSortAsc("CreatedAt")) query = query.OrderBy(q => q.CreatedAt);
            else if (SortOrder == Html.GetSortDsc("CreatedAt")) query = query.OrderByDescending(q => q.CreatedAt);
            else if (SortOrder == Html.GetSortAsc("CedantId")) query = query.OrderBy(q => q.Cedant.Code);
            else if (SortOrder == Html.GetSortDsc("CedantId")) query = query.OrderByDescending(q => q.Cedant.Code);
            else if (SortOrder == Html.GetSortAsc("ReinsuranceTypePickListDetailId")) query = query.OrderBy(q => q.ReinsuranceTypePickListDetail.Code);
            else if (SortOrder == Html.GetSortDsc("ReinsuranceTypePickListDetailId")) query = query.OrderByDescending(q => q.ReinsuranceTypePickListDetail.Code);
            else if (SortOrder == Html.GetSortAsc("Name")) query = query.OrderBy(q => q.Name);
            else if (SortOrder == Html.GetSortDsc("Name")) query = query.OrderByDescending(q => q.Name);
            else if (SortOrder == Html.GetSortAsc("Description")) query = query.OrderBy(q => q.Description);
            else if (SortOrder == Html.GetSortDsc("Description")) query = query.OrderByDescending(q => q.Description);
            else if (SortOrder == Html.GetSortAsc("LatestVersion")) query = query.OrderBy(q => q.LatestVersion);
            else if (SortOrder == Html.GetSortDsc("LatestVersion")) query = query.OrderByDescending(q => q.LatestVersion);
            else if (SortOrder == Html.GetSortAsc("Status")) query = query.OrderBy(q => q.Status);
            else if (SortOrder == Html.GetSortDsc("Status")) query = query.OrderByDescending(q => q.Status);
            else if (SortOrder == Html.GetSortAsc("PricingStatus")) query = query.OrderBy(q => q.PricingStatus);
            else if (SortOrder == Html.GetSortDsc("PricingStatus")) query = query.OrderByDescending(q => q.PricingStatus);
            else if (SortOrder == Html.GetSortAsc("PricingTeamPickListDetailId")) query = query.OrderBy(q => q.PricingTeamPickListDetailId);
            else if (SortOrder == Html.GetSortDsc("PricingTeamPickListDetailId")) query = query.OrderByDescending(q => q.PricingTeamPickListDetailId);
            else if (SortOrder == Html.GetSortAsc("BDPersonInChargeId")) query = query.OrderBy(q => q.BDPersonInChargeId);
            else if (SortOrder == Html.GetSortDsc("BDPersonInChargeId")) query = query.OrderByDescending(q => q.BDPersonInChargeId);
            else if (SortOrder == Html.GetSortAsc("PersonInChargeId")) query = query.OrderBy(q => q.PersonInChargeId);
            else if (SortOrder == Html.GetSortDsc("PersonInChargeId")) query = query.OrderByDescending(q => q.PersonInChargeId);
            else query = query.OrderByDescending(q => q.Id);

            ViewBag.Total = query.Count();

            IndexPage();
            return View("Index", query.ToPagedList(Page ?? 1, PageSize));
        }

        [Auth(Controller = Controller, Power = "C")]
        [HttpPost]
        public JsonResult Add(TreatyPricingQuotationWorkflowBo quotationWorkflowBo)
        {
            List<string> errors = new List<string>();
            try
            {
                quotationWorkflowBo.CreatedById = AuthUserId;
                quotationWorkflowBo.UpdatedById = AuthUserId;
                quotationWorkflowBo.Status = TreatyPricingQuotationWorkflowBo.StatusQuoting;
                quotationWorkflowBo.PricingStatus = TreatyPricingQuotationWorkflowBo.PricingStatusUnassigned;
                quotationWorkflowBo.LatestVersion = 1;

                quotationWorkflowBo.CedantBo = CedantService.Find(quotationWorkflowBo.CedantId);
                quotationWorkflowBo.CedantId = quotationWorkflowBo.CedantBo.Id;

                #region Set Quotation Id
                DateTime today = DateTime.Today;
                string prefix = string.Format("{0}_{1}_", quotationWorkflowBo.CedantBo.Code, today.Year);
                int currentIdCount = TreatyPricingQuotationWorkflowService.GetCurrentQuotationIdCount(prefix) + 1;

                string countStr = currentIdCount.ToString().PadLeft(3, '0');
                quotationWorkflowBo.QuotationId = prefix + countStr;
                #endregion

                TrailObject trail = GetNewTrailObject();

                Result = TreatyPricingQuotationWorkflowService.Create(ref quotationWorkflowBo, ref trail);
                if (Result.Valid)
                {
                    #region Create Version
                    string sharePointPath = TreatyPricingQuotationWorkflowService.GetSharePointPath(quotationWorkflowBo);

                    var QuotationWorkflowVersionBo = new TreatyPricingQuotationWorkflowVersionBo()
                    {
                        TreatyPricingQuotationWorkflowId = quotationWorkflowBo.Id,
                        Version = 1,
                        QuoteSpecSharePointFolderPath = sharePointPath,
                        RateTableSharePointFolderPath = sharePointPath,
                        PendingOn = "Pricing Senior Manager/HOD",
                        CreatedById = AuthUserId,
                        UpdatedById = AuthUserId,
                    };
                    TreatyPricingQuotationWorkflowVersionService.Create(ref QuotationWorkflowVersionBo, ref trail);

                    //Create new set of checklist items
                    for (int i = 0; i < 7; i++)
                    {
                        var checklistBo = new TreatyPricingQuotationWorkflowVersionQuotationChecklistBo()
                        {
                            TreatyPricingQuotationWorkflowVersionId = QuotationWorkflowVersionBo.Id,
                            InternalTeam = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamName(i + 1),
                            Status = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.StatusNotRequired,
                            CreatedById = AuthUserId,
                            UpdatedById = AuthUserId,
                        };

                        TreatyPricingQuotationWorkflowVersionQuotationChecklistService.Create(ref checklistBo, ref trail);
                    }
                    #endregion

                    #region Create Linked Objects
                    TreatyPricingWorkflowObjectController.Create(quotationWorkflowBo.TreatyPricingWorkflowObjectBos, TreatyPricingWorkflowObjectBo.TypeQuotation, quotationWorkflowBo.Id, AuthUserId, ref trail);
                    #endregion

                    CreateTrail(
                        quotationWorkflowBo.Id,
                        "Create Treaty Pricing Quotation Workflow"
                    );

                    //QuotationWorkflowBos = TreatyPricingQuotationWorkflowService.GetAll();
                }
            }
            catch (Exception ex)
            {
                errors.Add(string.Format("Error occurred. Error details: {0}", ex.Message));
            }

            return Json(new { errors, quotationWorkflowBo });
        }

        // GET: TreatyPricingQuotationWorkflow/Edit/5
        public virtual ActionResult Edit(int id, bool isEditMode = false)
        {
            if (!CheckObjectLockReadOnly(Controller, id, isEditMode) && !CheckPower(Controller, AccessMatrixBo.PowerCompleteChecklist))
                return RedirectDashboard();

            var QuotationWorkflowBo = TreatyPricingQuotationWorkflowService.Find(id);
            if (QuotationWorkflowBo == null)
            {
                return RedirectToAction("Index");
            }

            var model = new TreatyPricingQuotationWorkflowViewModel(QuotationWorkflowBo);
            LoadPage(model, QuotationWorkflowBo.CedantId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public virtual ActionResult Edit(int id, TreatyPricingQuotationWorkflowViewModel model)
        {
            TreatyPricingQuotationWorkflowBo QuotationWorkflowBo = TreatyPricingQuotationWorkflowService.Find(id);
            if (QuotationWorkflowBo == null)
            {
                return RedirectToAction("Index", "TreatyPricingQuotationWorkflow");
            }

            model.Status = QuotationWorkflowBo.Status;
            //model.StatusRemarks = QuotationWorkflowBo.StatusRemarks;
            model.PricingStatus = QuotationWorkflowBo.PricingStatus;

            if (!CheckObjectLock(Controller, id))
                return RedirectToAction("Edit", new { id = id });

            bool isPricingPic = QuotationWorkflowBo.PricingStatus == TreatyPricingQuotationWorkflowBo.PricingStatusUnassigned &&
                ((!QuotationWorkflowBo.PersonInChargeId.HasValue && model.PersonInChargeId.HasValue) || 
                (QuotationWorkflowBo.PersonInChargeId != model.PersonInChargeId));

            TreatyPricingQuotationWorkflowVersionBo updatedVersionBo = null;
            QuotationWorkflowBo.SetVersionObjects(QuotationWorkflowBo.TreatyPricingQuotationWorkflowVersionBos);
            if (QuotationWorkflowBo.EditableVersion != model.CurrentVersion)
            {
                ModelState.AddModelError("", "You can only update details for the latest version");
            }
            else
            {
                updatedVersionBo = model.GetVersionBo((TreatyPricingQuotationWorkflowVersionBo)QuotationWorkflowBo.CurrentVersionObject);
                var currentVersionBo = TreatyPricingQuotationWorkflowVersionService.Find(updatedVersionBo.Id);

                updatedVersionBo.QuoteSpecSharePointLink = currentVersionBo.QuoteSpecSharePointLink;
                updatedVersionBo.RateTableSharePointLink = currentVersionBo.RateTableSharePointLink;

                QuotationWorkflowBo.AddVersionObject(updatedVersionBo);
            }

            if (ModelState.IsValid)
            {
                QuotationWorkflowBo = model.FormBo(QuotationWorkflowBo);
                QuotationWorkflowBo.UpdatedById = AuthUserId;

                if (isPricingPic)
                {
                    QuotationWorkflowBo.PricingStatus = TreatyPricingQuotationWorkflowBo.PricingStatusAssessmentInProgress;
                }

                TrailObject trail = GetNewTrailObject();
                Result = TreatyPricingQuotationWorkflowService.Update(ref QuotationWorkflowBo, ref trail);
                if (Result.Valid)
                {
                    if (isPricingPic)
                    {
                        StatusHistoryBo statusHistoryBo = new StatusHistoryBo
                        {
                            ModuleId = model.ModuleId,
                            ObjectId = QuotationWorkflowBo.Id,
                            Version = QuotationWorkflowBo.LatestVersion,
                            SubModuleController = ModuleBo.ModuleController.TreatyPricingQuotationWorkflowPricing.ToString(),
                            Status = QuotationWorkflowBo.PricingStatus ?? 0
                        };

                        UpdateStatus(statusHistoryBo);
                    }

                    if (updatedVersionBo != null)
                    {
                        updatedVersionBo.UpdatedById = AuthUserId;
                        int statusNotRequired = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.StatusNotRequired;

                        TreatyPricingQuotationWorkflowVersionService.Update(ref updatedVersionBo, ref trail);
                        int CEOPending = statusNotRequired;
                        int PricingPending = statusNotRequired;
                        int UnderwritingPending = statusNotRequired;
                        int HealthPending = statusNotRequired;
                        int ClaimsPending = statusNotRequired;
                        int BDPending = statusNotRequired;
                        int TGPending = statusNotRequired;
                        //int statusRequested = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.StatusRequested;
                        //int statusPendingSignOff = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.StatusPendingSignOff;

                        //Process quotation checklist saving
                        string checklists = model.QuotationChecklists;
                        if (!string.IsNullOrEmpty(checklists))
                        {
                            List<TreatyPricingQuotationWorkflowVersionQuotationChecklistBo> checklistBos = JsonConvert.DeserializeObject<List<TreatyPricingQuotationWorkflowVersionQuotationChecklistBo>>(checklists);

                            foreach (var checklistBo in checklistBos)
                            {
                                checklistBo.UpdatedById = AuthUserId;

                                #region Set Quotation Workflow Pending fields
                                string nameCeo = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamName(TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamCEO);
                                string namePricing = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamName(TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamPricing);
                                string nameUnderwriting = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamName(TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamUnderwriting);
                                string nameHealth = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamName(TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamHealth);
                                string nameClaims = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamName(TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamClaims);
                                string nameBD = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamName(TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamBD);
                                string nameCR = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamName(TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamCR);

                                if (checklistBo.InternalTeam == nameCeo)
                                    CEOPending = checklistBo.Status;
                                if (checklistBo.InternalTeam == namePricing)
                                    PricingPending = checklistBo.Status;
                                if (checklistBo.InternalTeam == nameUnderwriting)
                                    UnderwritingPending = checklistBo.Status;
                                if (checklistBo.InternalTeam == nameHealth)
                                    HealthPending = checklistBo.Status;
                                if (checklistBo.InternalTeam == nameClaims)
                                    ClaimsPending = checklistBo.Status;
                                if (checklistBo.InternalTeam == nameBD)
                                    BDPending = checklistBo.Status;
                                if (checklistBo.InternalTeam == nameCR)
                                    TGPending = checklistBo.Status;
                                #endregion

                                var checklistUpdateBo = checklistBo;
                                TreatyPricingQuotationWorkflowVersionQuotationChecklistService.Update(ref checklistUpdateBo, ref trail);
                            }
                        }

                        #region Update Quotation Workflow table fields for Quotation & Pricing dashboard
                        QuotationWorkflowBo.BDPersonInChargeId = updatedVersionBo.BDPersonInChargeId;
                        QuotationWorkflowBo.PersonInChargeId = updatedVersionBo.PersonInChargeId;
                        QuotationWorkflowBo.CEOPending = CEOPending;
                        QuotationWorkflowBo.PricingPending = PricingPending;
                        QuotationWorkflowBo.UnderwritingPending = UnderwritingPending;
                        QuotationWorkflowBo.HealthPending = HealthPending;
                        QuotationWorkflowBo.ClaimsPending = ClaimsPending;
                        QuotationWorkflowBo.BDPending = BDPending;
                        QuotationWorkflowBo.TGPending = TGPending;

                        QuotationWorkflowBo.PricingDueDate = updatedVersionBo.RevisedPricingDueDate.HasValue ? updatedVersionBo.RevisedPricingDueDate : (updatedVersionBo.TargetPricingDueDate.HasValue ? updatedVersionBo.TargetPricingDueDate : DateTime.Now);

                        Result = TreatyPricingQuotationWorkflowService.Update(ref QuotationWorkflowBo, ref trail);
                        #endregion
                    }

                    CreateTrail(
                        QuotationWorkflowBo.Id,
                        "Update Quotation Workflow"
                    );

                    ObjectLockController.DeleteObjectLock(Controller, id, AuthUserId);

                    SetUpdateSuccessMessage("Quotation Workflow", false);
                    return RedirectToAction("Edit", new { id = QuotationWorkflowBo.Id });
                }
                AddResult(Result);
            }

            model.CurrentVersion = QuotationWorkflowBo.CurrentVersion;
            model.CurrentVersionObject = QuotationWorkflowBo.CurrentVersionObject;
            model.VersionObjects = QuotationWorkflowBo.VersionObjects;
            model.CedantBo = QuotationWorkflowBo.CedantBo;
            model.TreatyPricingWorkflowObjectBos = QuotationWorkflowBo.TreatyPricingWorkflowObjectBos;
            LoadPage(model, QuotationWorkflowBo.CedantId);
            return View(model);
        }

        public void LoadPage(TreatyPricingQuotationWorkflowViewModel model, int cedantId)
        {
            DropDownCedant(CedantBo.StatusActive);
            DropDownReinsuranceType();
            DropDownPricingTeam();
            DropDownStatus();
            DropDownPricingStatus();
            DropDownVersion(model);
            DropDownUser(UserBo.StatusActive, model.PersonInChargeId);
            DropDownQuoteSpecTemplate(cedantId);
            //DropDownRateTableTemplate(cedantId);
            DropDownRateTableTemplate();

            DropDownTreatyPricingObjectModules();

            // Emails
            ViewBag.RecipientEmails = UserService.GetEmailUsers();

            AuthUserName();
            ViewBag.PersonInChargeId = AuthUserId;
            ViewBag.PersonInChargeName = UserService.Find(AuthUserId).FullName;
            ViewBag.PersonInChargeDepartment = UserService.Find(AuthUserId).DepartmentId;

            ViewBag.IsHideSideBar = false;

            GetFullBaseUrl();

            // Checklist user list
            int departmentCEO = Util.GetConfigInteger("CEO");
            int departmentPPT = Util.GetConfigInteger("ProductPricingAndTreaty");
            int departmentUW = Util.GetConfigInteger("Underwriting");
            int departmentHealth = Util.GetConfigInteger("Health");
            int departmentClaims = Util.GetConfigInteger("Claims");
            int departmentBD = Util.GetConfigInteger("BDAndGroup");
            int departmentCR = Util.GetConfigInteger("ComplianceAndRisk");

            ViewBag.PersonInChargeBD = DropDownUser(UserBo.StatusActive, null, false, departmentBD);
            ViewBag.PersonInChargePPT = DropDownUser(UserBo.StatusActive, null, false, departmentPPT);
            ViewBag.PersonInChargeGroupPricing = DropDownUserByAccessPower(model.ModuleId, AccessMatrixBo.PowerGroupPricing);

            ViewBag.UsersCEO = GetUsers(UserBo.StatusActive, departmentCEO);
            ViewBag.UsersPricing = GetUsers(UserBo.StatusActive, departmentPPT);
            ViewBag.UsersUnderwriting = GetUsers(UserBo.StatusActive, departmentUW);
            ViewBag.UsersHealth = GetUsers(UserBo.StatusActive, departmentHealth);
            ViewBag.UsersClaims = GetUsers(UserBo.StatusActive, departmentClaims);
            ViewBag.UsersBD = GetUsers(UserBo.StatusActive, departmentBD);
            ViewBag.UsersTG = GetUsers(UserBo.StatusActive, departmentCR);

            // Remark & Document
            string downloadDocumentUrl = Url.Action("Download", "Document");
            ViewBag.QuotationRemarkBos = GetRemarkDocument(model.ModuleId, model.Id, downloadDocumentUrl, ModuleBo.ModuleController.TreatyPricingQuotationWorkflowQuotation.ToString());
            ViewBag.PricingRemarkBos = GetRemarkDocument(model.ModuleId, model.Id, downloadDocumentUrl, ModuleBo.ModuleController.TreatyPricingQuotationWorkflowPricing.ToString());
            GetRemarkSubjects();

            // Status Histories
            ViewBag.ChecklistStatusHistoryBos = GetStatusHistories(model.ModuleId, model.Id, downloadDocumentUrl, ModuleBo.ModuleController.TreatyPricingQuotationWorkflowVersionQuotationChecklist.ToString());
            ViewBag.PricingStatusHistoryBos = GetStatusHistories(model.ModuleId, model.Id, downloadDocumentUrl, ModuleBo.ModuleController.TreatyPricingQuotationWorkflowPricing.ToString());
            ViewBag.StatusHistoryBos = GetStatusHistories(model.ModuleId, model.Id, downloadDocumentUrl);

            SetViewBagMessage();
        }

        public void IndexPage()
        {
            DropDownCedant(CedantBo.StatusActive);
            DropDownReinsuranceType();
            DropDownPricingTeam();
            DropDownStatus();
            DropDownPricingStatus();
            DropDownUser(UserBo.StatusActive);

            DropDownTreatyPricingObjectModules();
            DropDownPendingChecklist();

            SetViewBagMessage();

            int departmentPPT = Util.GetConfigInteger("ProductPricingAndTreaty");
            int departmentBD = Util.GetConfigInteger("BDAndGroup");
            ViewBag.UsersPricing = DropDownUser(UserBo.StatusActive, null, false, departmentPPT);
            ViewBag.UsersBD = DropDownUser(UserBo.StatusActive, null, false, departmentBD);
        }

        public List<SelectListItem> DropDownStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingQuotationWorkflowBo.StatusMax))
            {
                items.Add(new SelectListItem { Text = TreatyPricingQuotationWorkflowBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.StatusItems = items;
            return items;
        }

        public List<SelectListItem> DropDownPricingStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingQuotationWorkflowBo.PricingStatusMax))
            {
                items.Add(new SelectListItem { Text = TreatyPricingQuotationWorkflowBo.GetPricingStatusName(i), Value = i.ToString() });
            }
            ViewBag.PricingStatusItems = items;
            return items;
        }

        public ActionResult CreateVersion(TreatyPricingQuotationWorkflowBo bo, bool duplicatePreviousVersion = false)
        {
            bo.SetVersionObjects(TreatyPricingQuotationWorkflowVersionService.GetByTreatyPricingQuotationWorkflowId(bo.Id));
            TreatyPricingQuotationWorkflowVersionBo nextVersionBo;
            if (duplicatePreviousVersion)
            {
                TreatyPricingQuotationWorkflowVersionBo previousVersionBo = (TreatyPricingQuotationWorkflowVersionBo)bo.CurrentVersionObject;
                nextVersionBo = new TreatyPricingQuotationWorkflowVersionBo(previousVersionBo)
                {
                    Id = 0,
                };
            }
            else
            {
                nextVersionBo = new TreatyPricingQuotationWorkflowVersionBo()
                {
                    TreatyPricingQuotationWorkflowId = bo.Id,
                };
            }

            string sharePointPath = TreatyPricingQuotationWorkflowService.GetSharePointPath(bo);

            nextVersionBo.Version = bo.EditableVersion + 1;
            nextVersionBo.QuoteSpecSharePointFolderPath = sharePointPath;
            nextVersionBo.RateTableSharePointFolderPath = sharePointPath;
            nextVersionBo.PersonInChargeId = AuthUserId;
            nextVersionBo.CreatedById = AuthUserId;
            nextVersionBo.UpdatedById = AuthUserId;

            TrailObject trail = GetNewTrailObject();
            Result = TreatyPricingQuotationWorkflowVersionService.Create(ref nextVersionBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    nextVersionBo.Id,
                    "Create Treaty Pricing Quotation Workflow Version"
                );

                //Create new set of checklist items
                for (int i = 0; i < 7; i++)
                {
                    var checklistBo = new TreatyPricingQuotationWorkflowVersionQuotationChecklistBo()
                    {
                        TreatyPricingQuotationWorkflowVersionId = nextVersionBo.Id,
                        InternalTeam = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamName(i + 1),
                        Status = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.StatusNotRequired,
                        CreatedById = AuthUserId,
                        UpdatedById = AuthUserId,
                    };

                    TreatyPricingQuotationWorkflowVersionQuotationChecklistService.Create(ref checklistBo, ref trail);
                }
            }

            bo.AddVersionObject(nextVersionBo);
            var versions = DropDownVersion(bo);

            return Json(new { bo, versions });
        }

        [HttpPost]
        public ActionResult GetVersionDetails(int quotationWorkflowId, int quotationWorkflowVersion, bool isEditMode)
        {
            int treatyPricingQuotationWorkflowVersionId = TreatyPricingQuotationWorkflowVersionService.GetVersionId(quotationWorkflowId, quotationWorkflowVersion);
            var latestVersionBo = TreatyPricingQuotationWorkflowVersionService.GetLatestVersionByTreatyPricingQuotationWorkflowId(quotationWorkflowId);

            var quotationChecklistBos = TreatyPricingQuotationWorkflowVersionQuotationChecklistService.GetByTreatyPricingQuotationWorkflowVersionId(treatyPricingQuotationWorkflowVersionId);

            //Checklist permissions
            string username = AuthUser.UserName;
            int? departmentId = AuthUser.DepartmentId;
            bool isLatestVersion = latestVersionBo?.Version == quotationWorkflowVersion;

            bool enableCompleteChecklist = CheckPower(Controller, AccessMatrixBo.PowerCompleteChecklist);
            bool enableEdit = CheckPower(Controller, AccessMatrixBo.AccessMatrixCRUD.U.ToString()) && isEditMode;

            #region Internal department
            int departmentCEO = Util.GetConfigInteger("CEO");
            int departmentPPT = Util.GetConfigInteger("ProductPricingAndTreaty");
            int departmentUW = Util.GetConfigInteger("Underwriting");
            int departmentHealth = Util.GetConfigInteger("Health");
            int departmentClaims = Util.GetConfigInteger("Claims");
            int departmentBD = Util.GetConfigInteger("BDAndGroup");
            int departmentCR = Util.GetConfigInteger("ComplianceAndRisk");

            string nameCeo = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamName(TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamCEO);
            string namePricing = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamName(TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamPricing);
            string nameUnderwriting = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamName(TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamUnderwriting);
            string nameHealth = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamName(TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamHealth);
            string nameClaims = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamName(TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamClaims);
            string nameBD = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamName(TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamBD);
            string nameCR = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamName(TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamCR);
            #endregion

            if (departmentId != null)
            {
                foreach (var bo in quotationChecklistBos)
                {
                    bo.DisableRequest = !enableEdit;

                    bo.DisableButtons = true;
                    bo.DisablePersonInCharge = true;

                    if (enableEdit)
                    {
                        bo.DisableButtons = false;
                        bo.DisablePersonInCharge = false;
                    }
                    else
                    {
                        if (bo.InternalTeam == nameCeo && isLatestVersion && departmentId == departmentCEO && enableCompleteChecklist)
                            bo.DisableButtons = false;
                        if (bo.InternalTeam == namePricing && isLatestVersion && departmentId == departmentPPT && enableCompleteChecklist)
                            bo.DisableButtons = false;
                        if (bo.InternalTeam == nameUnderwriting && isLatestVersion && departmentId == departmentUW && enableCompleteChecklist)
                            bo.DisableButtons = false;
                        if (bo.InternalTeam == nameHealth && isLatestVersion && departmentId == departmentHealth && enableCompleteChecklist)
                            bo.DisableButtons = false;
                        if (bo.InternalTeam == nameClaims && isLatestVersion && departmentId == departmentClaims && enableCompleteChecklist)
                            bo.DisableButtons = false;
                        if (bo.InternalTeam == nameBD && isLatestVersion && departmentId == departmentBD && enableCompleteChecklist)
                            bo.DisableButtons = false;
                        if (bo.InternalTeam == nameCR && isLatestVersion && departmentId == departmentCR && enableCompleteChecklist)
                            bo.DisableButtons = false;
                    }

                    if (bo.DisableButtons == false)
                    {
                        bo.DisableButtons = true;

                        if (!string.IsNullOrEmpty(bo.InternalTeamPersonInCharge))
                        {
                            var internalTeamPersonInCharge = bo.InternalTeamPersonInCharge.Split(',').ToList();

                            foreach (string personInCharge in internalTeamPersonInCharge)
                            {
                                if (personInCharge.Trim() == username)
                                    bo.DisableButtons = false;
                            }
                        }
                    }
                }
            }

            return Json(new { quotationChecklistBos });
        }

        public JsonResult GetLatestVersion(int id)
        {
            int version = TreatyPricingQuotationWorkflowService.GetLatestVersion(id);

            var bo = TreatyPricingQuotationWorkflowService.Find(id);
            bo.LatestVersion = version;
            TreatyPricingQuotationWorkflowService.Update(ref bo);

            return Json(new { version });
        }

        [HttpPost]
        public void UpdateVersionChecklistFinalised(int versionId, bool isFinalise = true)
        {
            var bo = TreatyPricingQuotationWorkflowVersionService.Find(versionId);

            if (bo != null)
            {
                TrailObject trail = GetNewTrailObject();

                bo.ChecklistFinalised = isFinalise;
                Result = TreatyPricingQuotationWorkflowVersionService.Update(ref bo, ref trail);

                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Update Treaty Pricing Quotation Workflow Version"
                    );
                }
            }
        }

        public ActionResult Download(
            string downloadToken,
            int type,
            string QuotationId,
            DateTime? CreatedAt,
            int? CedantId,
            int? ReinsuranceTypePickListDetailId,
            string Name,
            string Description,
            int? LatestVersion,
            int? Status,
            int? PricingStatus,
            int? PricingTeamPickListDetailId,
            int? BDPersonInChargeId,
            int? PersonInChargeId,
            int? InternalTeam)
        {
            // type 1 = all
            // type 2 = filtered download

            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            dynamic Params = new ExpandoObject();
            Params.QuotationId = QuotationId;
            Params.CreatedAt = CreatedAt;
            Params.CedantId = CedantId;
            Params.ReinsuranceTypePickListDetailId = ReinsuranceTypePickListDetailId;
            Params.Name = Name;
            Params.Description = Description;
            Params.LatestVersion = LatestVersion;
            Params.Status = Status;
            Params.PricingStatus = PricingStatus;
            Params.PricingTeamPickListDetailId = PricingTeamPickListDetailId;
            Params.BDPersonInChargeId = BDPersonInChargeId;
            Params.PersonInChargeId = PersonInChargeId;
            Params.InternalTeam = InternalTeam;
            var export = new GenerateExportData();
            ExportBo exportBo = export.CreateExportQuotationWorkflow(AuthUserId, Params);

            if (exportBo.Total <= export.ExportInstantDownloadLimit)
            {
                export.Process(ref exportBo, _db);
                Session.Add("DownloadExport", true);
            }

            return RedirectToAction("Edit", "Export", new { exportBo.Id });
        }

        #region SharePoint
        [HttpPost]
        public JsonResult GenerateSharePointFile(int versionId, string type, string typeFull, string templateCode, string sharePointFolderPath, string sharePointLink)
        {
            List<string> errors = new List<string>();
            List<string> confirmations = new List<string>();
            bool callGenerate = false;
            bool isCampaignSpec = false;
            string fileName = "";
            string path = "";
            string editLink = "";

            try
            {
                TreatyPricingQuotationWorkflowVersionBo versionBo = TreatyPricingQuotationWorkflowVersionService.Find(versionId);
                TreatyPricingQuotationWorkflowBo bo = TreatyPricingQuotationWorkflowService.Find(versionBo.TreatyPricingQuotationWorkflowId);
                IList<TreatyPricingWorkflowObjectBo> objectBos = TreatyPricingWorkflowObjectService.GetByTypeWorkflowId(TreatyPricingWorkflowObjectBo.TypeQuotation, bo.Id);

                if (type == "QuoteSpec")
                {
                    TemplateBo templateBo = TemplateService.FindByCode(templateCode);

                    //Used to determine if spec is campaign or quote
                    isCampaignSpec = templateBo.DocumentTypeId == "Quotation Workflow - Campaign Spec" ? true : false;

                    if (templateBo != null)
                    {
                        TemplateDetailBo templateDetailBo = TemplateDetailService.GetLatestByTemplateId(templateBo.Id);

                        if (templateDetailBo != null)
                        {
                            path = Util.GetUploadPath("Template") + @"\" + templateDetailBo.HashFileName;
                            //fileName = templateCode + Path.GetExtension(templateDetailBo.HashFileName);

                            fileName = versionBo.Version.ToString() + " Quotation " + bo.CedantBo.Code + " " + bo.Name;
                            //fileName += type == "QuoteSpec" ? "" : " - Rates Only";
                            fileName += " " + DateTime.Now.ToString("yyyyMMdd") + Path.GetExtension(templateDetailBo.HashFileName);

                            if (sharePointLink != null && sharePointLink != "")
                            {
                                confirmations.Add(typeFull + " file has been created for this version.");
                            }

                            using (var sp = new SharePointContext())
                            {
                                if (!sp.FileFolderExists(sharePointFolderPath))
                                {
                                    confirmations.Add("This folder doesn't exist in SharePoint.");
                                }
                                else if (sp.FileFolderExists(sharePointFolderPath + "/" + fileName))
                                {
                                    confirmations.Add("File already exist in SharePoint.");
                                }
                                else
                                {
                                    callGenerate = true;
                                }
                            }

                            if (callGenerate)
                            {
                                GenerateFileInSharePoint(versionId, type, path, fileName, sharePointFolderPath, isCampaignSpec, ref errors, ref editLink);
                            }
                        }
                        else
                        {
                            errors.Add("Template details not found.");
                        }
                    }
                    else
                    {
                        errors.Add("Template not found.");
                    }
                }
                else
                {
                    path = templateCode;// Util.GetUploadPath("Template") + @"\" + templateDetailBo.HashFileName;
                    //fileName = templateCode + Path.GetExtension(templateDetailBo.HashFileName);

                    fileName = versionBo.Version.ToString() + " Quotation " + bo.CedantBo.Code + " " + bo.Name;
                    fileName += " - Rates Only " + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";

                    if (sharePointLink != null && sharePointLink != "")
                    {
                        confirmations.Add(typeFull + " file has been created for this version.");
                    }

                    using (var sp = new SharePointContext())
                    {
                        if (!sp.FileFolderExists(sharePointFolderPath))
                        {
                            confirmations.Add("This folder doesn't exist in SharePoint.");
                        }
                        else if (sp.FileFolderExists(sharePointFolderPath + "/" + fileName))
                        {
                            confirmations.Add("File already exist in SharePoint.");
                        }
                        else
                        {
                            callGenerate = true;
                        }
                    }

                    if (callGenerate)
                    {
                        GenerateFileInSharePoint(versionId, type, path, fileName, sharePointFolderPath, isCampaignSpec, ref errors, ref editLink);
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(string.Format("Error occurred. Error details: {0}", ex.Message));
            }

            return Json(new { errors, confirmations, fileName, path, editLink, isCampaignSpec });
        }

        [HttpPost]
        public JsonResult GenerateSharePointFileConfirmed(int versionId, string type, string newFileName, string sharePointFolderPath, string localPath, bool isCampaignSpec)
        {
            List<string> errors = new List<string>();
            string editLink = "";

            GenerateFileInSharePoint(versionId, type, localPath, newFileName, sharePointFolderPath, isCampaignSpec, ref errors, ref editLink);

            return Json(new { errors, editLink });
        }

        public void GenerateFileInSharePoint(int versionId, string type, string localPath, string newFileName, string sharePointFolderPath, bool isCampaignSpec, ref List<string> errors, ref string editLink)
        {
            try
            {
                TreatyPricingQuotationWorkflowVersionBo versionBo = TreatyPricingQuotationWorkflowVersionService.Find(versionId);
                TreatyPricingQuotationWorkflowBo bo = TreatyPricingQuotationWorkflowService.Find(versionBo.TreatyPricingQuotationWorkflowId);
                IList<TreatyPricingWorkflowObjectBo> objectBos = TreatyPricingWorkflowObjectService.GetByTypeWorkflowId(TreatyPricingWorkflowObjectBo.TypeQuotation, bo.Id);

                if (type == "QuoteSpec")
                {
                    //Copy file
                    string folderPath = Util.GetTreatyPricingQuotationWorkflowUploadPath(type) + "_Copied";
                    string sourceFile = localPath;
                    string destinationFile = Path.Combine(folderPath, Path.GetFileName(localPath));
                    Util.MakeDir(destinationFile);
                    System.IO.File.Copy(sourceFile, destinationFile, true);

                    //Start processing file
                    if (isCampaignSpec)
                    {
                        var process = new ProcessQuotationWorkflowCampaignSpec()
                        {
                            FilePath = destinationFile,
                            WorkflowBo = bo,
                            WorkflowVersionBo = versionBo,
                            WorkflowObjectBos = objectBos,
                        };
                        process.Process();
                    }
                    else
                    {
                        var process = new ProcessQuotationWorkflowQuoteSpec()
                        {
                            FilePath = destinationFile,
                            WorkflowBo = bo,
                            WorkflowVersionBo = versionBo,
                            WorkflowObjectBos = objectBos,
                        };
                        process.Process();
                    }

                    //Upload to SharePoint
                    using (var sp = new SharePointContext())
                    {
                        string fullSharePointPath = sharePointFolderPath + "/" + newFileName;

                        sp.AddNewFolder(sharePointFolderPath);
                        sp.UploadFile(destinationFile
                            , newFileName
                            , (sharePointFolderPath == null ? "" : sharePointFolderPath));

                        editLink = sp.GetCopyLinkURL(fullSharePointPath);

                        versionBo.QuoteSpecSharePointLink = editLink;

                        TreatyPricingQuotationWorkflowVersionService.Update(ref versionBo);
                    }
                }
                else
                {
                    //Copy file
                    string folderPath = Util.GetTreatyPricingQuotationWorkflowUploadPath(type);
                    string templateType = localPath;
                    string destinationFile = Path.Combine(folderPath, newFileName);
                    Util.MakeDir(destinationFile);

                    //Start processing file
                    var process = new ProcessQuotationWorkflowRateTable()
                    {
                        TemplateType = templateType,
                        FilePath = destinationFile,
                        WorkflowBo = bo,
                        WorkflowVersionBo = versionBo,
                        WorkflowObjectBos = objectBos,
                    };
                    process.Process();

                    //Upload to SharePoint
                    using (var sp = new SharePointContext())
                    {
                        string fullSharePointPath = sharePointFolderPath + "/" + newFileName;

                        sp.AddNewFolder(sharePointFolderPath);
                        sp.UploadFile(destinationFile
                            , newFileName
                            , (sharePointFolderPath == null ? "" : sharePointFolderPath));

                        editLink = sp.GetCopyLinkURL(fullSharePointPath);

                        versionBo.RateTableSharePointLink = editLink;

                        TreatyPricingQuotationWorkflowVersionService.Update(ref versionBo);

                        if (System.IO.File.Exists(destinationFile))
                            System.IO.File.Delete(destinationFile);
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(string.Format("Error occurred. Error details: {0}", ex.Message));
            }
        }
        #endregion

        #region Final file upload
        [HttpPost]
        public ActionResult UploadFinalFile()
        {
            List<string> errors = new List<string>();
            string finalHashFileName = "";

            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    int versionId = int.Parse(Request["versionId"]);
                    string type = Request["type"];

                    HttpPostedFileBase file = files[0];
                    string fileName = Path.GetFileName(file.FileName);

                    Result = TreatyPricingQuotationWorkflowVersionService.Result();

                    int fileSize = file.ContentLength / 1024 / 1024 / 1024;
                    if (fileSize >= 2)
                        Result.AddError("Uploaded file size exceeded 2 GB");

                    if (Result.Valid)
                    {
                        TreatyPricingQuotationWorkflowVersionBo bo = TreatyPricingQuotationWorkflowVersionService.Find(versionId);

                        string hashFileName = Hash.HashFileName(fileName);
                        string folderPath = Util.GetTreatyPricingQuotationWorkflowUploadPath(type);
                        string path = Path.Combine(folderPath, hashFileName);
                        Util.MakeDir(path);

                        finalHashFileName = hashFileName;

                        if (type == "QuoteSpec")
                        {
                            bo.FinalQuoteSpecFileName = fileName;
                            bo.FinalQuoteSpecHashFileName = hashFileName;
                        }
                        else
                        {
                            bo.FinalRateTableFileName = fileName;
                            bo.FinalRateTableHashFileName = hashFileName;
                        }

                        TrailObject trail = GetNewTrailObject();
                        Result = TreatyPricingQuotationWorkflowVersionService.Update(ref bo, ref trail);
                        if (Result.Valid)
                        {
                            CreateTrail(
                                bo.Id,
                                "Upload Treaty Pricing Quotation Workflow Final File"
                            );
                            file.SaveAs(path);
                        }
                    }
                    errors = Result.ToErrorArray().OfType<string>().ToList();
                }
                catch (Exception ex)
                {
                    errors.Add(string.Format("Error occurred. Error details: ", ex.Message));
                }
            }
            else
            {
                errors.Add("No files selected.");
            }

            return Json(new { errors, finalHashFileName });
        }

        public ActionResult DownloadFinalFile(int id, string type)
        {
            TreatyPricingQuotationWorkflowVersionBo bo = TreatyPricingQuotationWorkflowVersionService.Find(id);
            if (bo == null)
                return null;

            string path = bo.GetLocalPath(type);
            if (System.IO.File.Exists(path) && path != "")
            {
                return File(
                    System.IO.File.ReadAllBytes(path),
                    System.Net.Mime.MediaTypeNames.Application.Octet,
                    type == "QuoteSpec" ? bo.FinalQuoteSpecFileName : bo.FinalRateTableFileName
                );
            }
            return null;
        }
        #endregion

        #region Quotation checklist
        public JsonResult GetChecklistStatusName(int status)
        {
            string statusName = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetStatusName(status);

            return Json(new { statusName });
        }

        public JsonResult NotifyQuotationChecklist(string quotationId, string internalTeam, int? status = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.StatusRequested)
        {
            List<int?> userIds = new List<int?>();
            List<string> usernames = internalTeam.Split(',').ToList();

            int id = TreatyPricingQuotationWorkflowService.FindByQuotationId(quotationId).Id;
            string link = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority + Url.Action("Edit", "TreatyPricingQuotationWorkflow", new { id = id });

            foreach (string username in usernames)
            {
                int userId = UserService.FindByUsername(username.Trim()).Id;
                userIds.Add(userId);
            }

            List<string> errors = new List<string>();

            for (int i = 0; i < userIds.Count; i++)
            {
                int? userId = userIds[i];
                if (!userId.HasValue)
                {
                    errors.Insert(i, "User not selected");
                    continue;
                }

                UserBo userBo = UserService.Find(userId);
                if (userBo == null)
                {
                    errors.Insert(i, "User not found");
                    continue;
                }

                string pendingType = "sign off";

                if (status == TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.StatusRequested)
                    pendingType = "your review";

                GetNewEmail(EmailBo.TypeNotifyQuotationWorkflowChecklist, userBo.Email, userBo.Id);
                EmailBo.AddData(userBo.FullName);
                EmailBo.AddData(pendingType);
                EmailBo.AddData(quotationId);
                EmailBo.AddData(link);

                if (!GenerateMail(showWarning: false))
                    errors.Insert(i, MessageBag.EmailError);

                EmailBo bo = EmailBo;
                Services.EmailService.Create(ref bo);
            }

            return Json(new { errors });
        }
        #endregion

        #region Status History
        [HttpPost]
        public JsonResult UpdateStatus(StatusHistoryBo statusHistoryBo)
        {
            TreatyPricingQuotationWorkflowBo bo = TreatyPricingQuotationWorkflowService.Find(statusHistoryBo.ObjectId);
            TreatyPricingQuotationWorkflowVersionBo versionBo = TreatyPricingQuotationWorkflowVersionService.GetLatestVersionByTreatyPricingQuotationWorkflowId(statusHistoryBo.ObjectId);

            string quotationId = bo.QuotationId;
            string statusRemark = statusHistoryBo?.RemarkBo?.Content;
            int emailType;
            TrailObject trail = GetNewTrailObject();

            if (bo == null)
                return Json(new { });

            List<dynamic> recipients = new List<dynamic>();
            if (!string.IsNullOrEmpty(statusHistoryBo.Emails))
            {
                List<string> error = new List<string>();

                List<string> emails = statusHistoryBo.Emails.Split(',').Select(e => e.Trim()).ToList();
                foreach (string email in emails)
                {
                    if (!email.Trim().IsValidEmail())
                    {
                        error.Add(string.Format(MessageBag.InvalidEmail, email));
                    }
                }

                if (error.Count() > 0)
                    return Json(new { error });

                recipients.AddRange(emails);
            }

            if (statusHistoryBo.SubModuleController == ModuleBo.ModuleController.TreatyPricingQuotationWorkflowPricing.ToString())
            {
                bo.PricingStatus = statusHistoryBo.Status;

                #region Update Pending On
                versionBo.PendingOn = TreatyPricingQuotationWorkflowBo.GetPendingOn(statusHistoryBo.Status, versionBo);

                TreatyPricingQuotationWorkflowVersionService.Update(ref versionBo, ref trail);
                #endregion

                emailType = EmailBo.TypeNotifyQuotationWorkflowPricingStatusUpdate;
                UserBo picRecipient = null;
                switch (bo.PricingStatus)
                {
                    case TreatyPricingQuotationWorkflowBo.PricingStatusPendingTechReview:
                        picRecipient = UserService.Find(versionBo.PersonInChargeTechReviewerId);
                        break;
                    case TreatyPricingQuotationWorkflowBo.PricingStatusPendingPeerReview:
                        picRecipient = UserService.Find(versionBo.PersonInChargePeerReviewerId);
                        break;
                    case TreatyPricingQuotationWorkflowBo.PricingStatusPendingPricingAuthorityReview:
                        picRecipient = UserService.Find(versionBo.PersonInChargePricingAuthorityReviewerId);
                        break;
                    default:
                        break;
                }

                if (picRecipient != null)
                {
                    recipients.Insert(0, picRecipient);
                }
            }
            else
            {
                bo.Status = statusHistoryBo.Status;
                emailType = EmailBo.TypeNotifyQuotationWorkflowStatusUpdate;
            }

            bo.UpdatedById = AuthUserId;
            trail = GetNewTrailObject();

            Result = TreatyPricingQuotationWorkflowService.Update(ref bo, ref trail);
            if (Result.Valid)
            {
                statusHistoryBo = StatusHistoryController.Create(statusHistoryBo, AuthUserId, AuthUser.UserName, ref trail);
                if (statusHistoryBo.SubModuleController == ModuleBo.ModuleController.TreatyPricingQuotationWorkflowPricing.ToString())
                    statusHistoryBo.StatusName = TreatyPricingQuotationWorkflowBo.GetPricingStatusName(statusHistoryBo.Status);

                if (recipients.Count() > 0)
                {
                    List<string> recipientNames = new List<string>();
                    foreach (dynamic recipient in recipients)
                    {
                        UserBo userBo = null;
                        string email = null;
                        string fullName = "";
                        if (recipient is UserBo)
                        {
                            userBo = recipient;
                            email = userBo.Email;
                        }
                        else if (recipient is string)
                        {
                            email = recipient;
                            userBo = UserService.FindByEmail(email);
                        }
                        else
                        {
                            continue;
                        }

                        if (userBo != null)
                            fullName = " " + userBo.FullName;

                        int id = TreatyPricingQuotationWorkflowService.FindByQuotationId(quotationId).Id;
                        string link = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority + Url.Action("Edit", "TreatyPricingQuotationWorkflow", new { id = id });

                        GetNewEmail(emailType, email, userBo?.Id);

                        EmailBo.AddData(fullName);
                        EmailBo.AddData(quotationId);
                        EmailBo.AddData(statusHistoryBo.StatusName);
                        EmailBo.AddData(statusRemark);
                        EmailBo.AddData(link);

                        GenerateMail(showWarning: false);

                        EmailBo.ModuleController = ModuleBo.ModuleController.StatusHistory.ToString();
                        EmailBo.ObjectId = statusHistoryBo.Id;
                        SaveEmail(ref trail);

                        recipientNames.Add(userBo?.FullName ?? email);
                    }

                    statusHistoryBo.RecipientNames = string.Join(", ", recipientNames);
                }

                CreateTrail(
                    bo.Id,
                    "Update Treaty Pricing Quotation Workflow Status"
                );

                return Json(new { statusHistoryBo });
            }
            return Json(new { error = Result.MessageBag.Errors });
        }

        [HttpPost]
        public JsonResult UpdateChecklistStatus(StatusHistoryBo statusHistoryBo)
        {
            int subObjectId = statusHistoryBo.SubObjectId ?? default(int);
            TreatyPricingQuotationWorkflowVersionQuotationChecklistBo bo = TreatyPricingQuotationWorkflowVersionQuotationChecklistService.Find(subObjectId);
            if (bo == null)
                return Json(new { });

            bo.Status = statusHistoryBo.Status;
            bo.InternalTeamPersonInCharge = statusHistoryBo.PersonInCharge;
            bo.UpdatedById = AuthUserId;

            #region Set Quotation Workflow Pending field for Quotation & Pricing dashboard
            var quotationWorkflowBo = bo.TreatyPricingQuotationWorkflowVersionBo.TreatyPricingQuotationWorkflowBo;
            //int statusRequested = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.StatusRequested;
            //int statusPendingSignOff = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.StatusPendingSignOff;
            string nameCeo = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamName(TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamCEO);
            string namePricing = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamName(TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamPricing);
            string nameUnderwriting = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamName(TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamUnderwriting);
            string nameHealth = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamName(TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamHealth);
            string nameClaims = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamName(TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamClaims);
            string nameBD = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamName(TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamBD);
            string nameCR = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamName(TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamCR);

            if (bo.InternalTeam == nameCeo)
                quotationWorkflowBo.CEOPending = bo.Status;
            if (bo.InternalTeam == namePricing)
                quotationWorkflowBo.PricingPending = bo.Status;
            if (bo.InternalTeam == nameUnderwriting)
                quotationWorkflowBo.UnderwritingPending = bo.Status;
            if (bo.InternalTeam == nameHealth)
                quotationWorkflowBo.HealthPending = bo.Status;
            if (bo.InternalTeam == nameClaims)
                quotationWorkflowBo.ClaimsPending = bo.Status;
            if (bo.InternalTeam == nameBD)
                quotationWorkflowBo.BDPending = bo.Status;
            if (bo.InternalTeam == nameCR)
                quotationWorkflowBo.TGPending = bo.Status;

            TreatyPricingQuotationWorkflowService.Update(ref quotationWorkflowBo);
            #endregion

            TrailObject trail = GetNewTrailObject();
            Result = TreatyPricingQuotationWorkflowVersionQuotationChecklistService.Update(ref bo, ref trail);

            if (Result.Valid)
            {
                statusHistoryBo = StatusHistoryController.Create(statusHistoryBo, AuthUserId, AuthUser.UserName, ref trail);

                CreateTrail(
                    bo.Id,
                    "Update Treaty Pricing Quotation Workflow Checklist Status"
                );

                // Replace status name get from parent
                statusHistoryBo.Department = bo.InternalTeam;
                statusHistoryBo.StatusName = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetStatusName(statusHistoryBo.Status);

                return Json(new { statusHistoryBo });
            }
            return Json(new { error = Result.MessageBag.Errors });
        }

        [HttpPost]
        public JsonResult GetStatusName(string type, int status, int id, string remarks)
        {
            string statusText;
            string pendingOn = "";

            if (type == "Pricing")
            {
                var versionBo = TreatyPricingQuotationWorkflowVersionService.GetLatestVersionByTreatyPricingQuotationWorkflowId(id);
                statusText = TreatyPricingQuotationWorkflowBo.GetPricingStatusName(status);
                pendingOn = TreatyPricingQuotationWorkflowBo.GetPendingOn(status, versionBo);
            }
            else
            {
                statusText = TreatyPricingQuotationWorkflowBo.GetStatusName(status);

                //Update status remarks
                TreatyPricingQuotationWorkflowBo bo = TreatyPricingQuotationWorkflowService.Find(id);
                bo.StatusRemarks = remarks;
                TreatyPricingQuotationWorkflowService.Update(ref bo);
            }

            return Json(new { statusText, pendingOn });
        }
        #endregion

        [HttpPost]
        public string AssignPricingPersonInCharge(int id, int personInChargeId)
        {
            var bo = TreatyPricingQuotationWorkflowService.Find(id);

            if (bo != null)
            {
                var versionBo = TreatyPricingQuotationWorkflowVersionService.GetLatestVersionByTreatyPricingQuotationWorkflowId(bo.Id);

                TrailObject trail = GetNewTrailObject();
                if (versionBo != null)
                {
                    versionBo.PersonInChargeId = personInChargeId;
                    versionBo.UpdatedById = AuthUserId;
                    Result = TreatyPricingQuotationWorkflowVersionService.Update(ref versionBo, ref trail);

                    if (Result.Valid)
                    {
                        CreateTrail(
                            bo.Id,
                            "Update Treaty Pricing Quotation Workflow Version"
                        );

                        trail = GetNewTrailObject();
                        bo.PricingStatus = TreatyPricingQuotationWorkflowBo.PricingStatusAssessmentInProgress;
                        bo.UpdatedById = AuthUserId;

                        Result = TreatyPricingQuotationWorkflowService.Update(ref bo, ref trail);

                        if (Result.Valid)
                        {
                            CreateTrail(
                                bo.Id,
                                "Update Treaty Pricing Quotation Workflow"
                            );

                            var moduleBo = ModuleService.FindByController(Controller);

                            StatusHistoryBo statusHistoryBo = new StatusHistoryBo
                            {
                                ModuleId = moduleBo.Id,
                                ObjectId = bo.Id,
                                Version = bo.LatestVersion,
                                SubModuleController = ModuleBo.ModuleController.TreatyPricingQuotationWorkflowPricing.ToString(),
                                Status = bo.PricingStatus ?? 0
                            };

                            UpdateStatus(statusHistoryBo);
                        }
                    }

                    if (!Result.Valid)
                        return Result.MessageBag.Errors.ToString();
                } else
                {
                    return "Quotation Workflow Version not found. ";
                }
            }
            else
            {
                return "Quotation Workflow not found. ";
            }
            return null;
        }

        public JsonResult GetPickListDetailCode(int pricingTeamPickListDetailId)
        {
            string code = PickListDetailService.Find(pricingTeamPickListDetailId).Code;

            return Json(new { code });
        }

        public JsonResult GetTreatyPricingCedantId(int cedantId, int reinsuranceTypePickListDetailId)
        {
            int treatyPricingCedantId = TreatyPricingCedantService.FindByCedantIdReinsuranceType(cedantId, reinsuranceTypePickListDetailId).Id;

            return Json(new { treatyPricingCedantId });
        }

        public List<SelectListItem> DropDownPendingChecklist()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamCEO, TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.DefaultInternalTeamCR))
            {
                items.Add(new SelectListItem { Text = string.Format("Pending {0}", TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamName(i)), Value = i.ToString() });
            }
            ViewBag.DropDownQuotationPendingChecklists = items;
            return items;
        }
    }
}