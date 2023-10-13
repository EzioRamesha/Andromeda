using PagedList;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;
using Services.TreatyPricing;
using BusinessObject.TreatyPricing;
using BusinessObject;
using Services;
using Shared.Trails;
using BusinessObject.Identity;
using Services.Identity;
using System.Dynamic;
using ConsoleApp.Commands.ProcessDatas;
using Shared.Forms.Attributes;
using System.ComponentModel;

namespace WebApp.Controllers
{
    [Auth]
    public class TreatyPricingTreatyWorkflowController : BaseController
    {
        public const string Controller = "TreatyPricingTreatyWorkflow";

        // GET: TreatyPricingTreatyWorkflow
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            int? id,
            int? LatestVersion,
            int? ReinsuranceTypePickListDetailId,
            int? CounterPartyDetailId,
            int? InwardRetroPartyDetailId,
            string DocumentId,
            int? DocumentType,
            string TypeOfBusiness,
            string Description,
            string EffectiveAtStr,
            string OrionGroupStr,
            int? PersonInChargeId,
            int? CoverageStatus,
            int? DocumentStatus,
            int? DraftingStatus,
            int? DraftingStatusCategory,
            string CountryOrigin,
            string LatestRevisionDateStr,
            string Reviewer,
            string SortOrder,
            int? Page)
        {
            var effectiveAt = Util.GetParseDateTime(EffectiveAtStr);
            var latestRevisionDate = Util.GetParseDateTime(LatestRevisionDateStr);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["ReinsuranceTypePickListDetailId"] = ReinsuranceTypePickListDetailId,
                ["CounterPartyDetailId"] = CounterPartyDetailId,
                ["InwardRetroPartyDetailId"] = InwardRetroPartyDetailId,
                ["DocumentType"] = DocumentType,
                ["DocumentId"] = DocumentId,
                ["TypeOfBusiness"] = TypeOfBusiness,
                ["Description"] = Description,
                ["EffectiveAtStr"] = effectiveAt.HasValue ? effectiveAt : null,
                ["OrionGroupStr"] = OrionGroupStr,
                ["PersonInChargeId"] = PersonInChargeId,
                ["CoverageStatus"] = CoverageStatus,
                ["DocumentStatus"] = DocumentStatus,
                ["DraftingStatus"] = DraftingStatus,
                ["DraftingStatusCategory"] = DraftingStatusCategory,
                ["CountryOrigin"] = CountryOrigin,
                ["LatestRevisionDateStr"] = latestRevisionDate.HasValue ? latestRevisionDate : null,
                ["Reviewer"] = Reviewer,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortReinsuranceTypePickListDetailId = GetSortParam("ReinsuranceTypePickListDetailId");
            ViewBag.SortCounterPartyDetailId = GetSortParam("CounterPartyDetailId");
            ViewBag.SortInwardRetroPartyDetailId = GetSortParam("InwardRetroPartyDetailId");
            ViewBag.SortDocumentType = GetSortParam("DocumentType");
            ViewBag.SortDocumentId = GetSortParam("DocumentId");
            ViewBag.SortTypeOfBusiness = GetSortParam("TypeOfBusiness");
            ViewBag.SortDescription = GetSortParam("Description");
            ViewBag.SortEffectiveAt = GetSortParam("EffectiveAtStr");
            ViewBag.SortPersonInChargeId = GetSortParam("PersonInChargeId");
            ViewBag.SortCoverageStatus = GetSortParam("CoverageStatus");
            ViewBag.SortOrionGroupStr = GetSortParam("OrionGroupStr");
            ViewBag.SortDocumentStatus = GetSortParam("DocumentStatus");
            ViewBag.SortDraftingStatus = GetSortParam("DraftingStatus");
            ViewBag.SortDraftingStatusCategory = GetSortParam("DraftingStatusCategory");
            ViewBag.SortCountryOrigin = GetSortParam("CountryOrigin");
            ViewBag.SortLatestRevisionDate = GetSortParam("LatestRevisionDateStr");

            var query = _db.TreatyPricingTreatyWorkflowVersions.OrderByDescending(q => q.Version).GroupBy(q => q.TreatyPricingTreatyWorkflowId).Select(TreatyPricingTreatyWorkflowViewModel.VersionExpression());

            if (ReinsuranceTypePickListDetailId.HasValue) query = query.Where(q => q.ReinsuranceTypePickListDetailId == ReinsuranceTypePickListDetailId);
            if (CounterPartyDetailId.HasValue) query = query.Where(q => q.CounterPartyDetailId == CounterPartyDetailId);
            if (InwardRetroPartyDetailId.HasValue) query = query.Where(q => q.InwardRetroPartyDetailId == InwardRetroPartyDetailId);
            if (DocumentType.HasValue) query = query.Where(q => q.DocumentType == DocumentType);
            if (!string.IsNullOrEmpty(DocumentId)) query = query.Where(q => q.DocumentId.Contains(DocumentId));
            if (!string.IsNullOrEmpty(TypeOfBusiness)) query = query.Where(q => q.TypeOfBusiness.Contains(TypeOfBusiness));
            if (!string.IsNullOrEmpty(Description)) query = query.Where(q => q.Description.Contains(Description));
            if (effectiveAt.HasValue) query = query.Where(q => q.EffectiveAt == effectiveAt);
            if (!string.IsNullOrEmpty(OrionGroupStr)) query = query.Where(q => q.OrionGroupStr == OrionGroupStr);
            if (PersonInChargeId.HasValue) query = query.Where(q => q.PersonInChargeId == PersonInChargeId);
            if (CoverageStatus.HasValue) query = query.Where(q => q.CoverageStatus == CoverageStatus);
            if (DocumentStatus.HasValue) query = query.Where(q => q.DocumentStatus == DocumentStatus);
            if (DraftingStatus.HasValue) query = query.Where(q => q.DraftingStatus == DraftingStatus);
            if (DraftingStatusCategory.HasValue) query = query.Where(q => q.DraftingStatusCategory == DraftingStatusCategory);
            if (!string.IsNullOrEmpty(CountryOrigin)) query = query.Where(q => q.CountryOrigin.Contains(CountryOrigin));
            if (latestRevisionDate.HasValue) query = query.Where(q => q.LatestRevisionDate == latestRevisionDate);
            if (!string.IsNullOrEmpty(Reviewer)) {
                query = query.Where(q => q.Reviewer/*.Split(',').Select(r => r.Trim()).ToArray()*/.Contains(Reviewer));
            }

            if (SortOrder == Html.GetSortAsc("ReinsuranceTypePickListDetailId")) query = query.OrderBy(q => q.ReinsuranceTypePickListDetail.Code);
            else if (SortOrder == Html.GetSortDsc("ReinsuranceTypePickListDetailId")) query = query.OrderByDescending(q => q.ReinsuranceTypePickListDetail.Code);
            else if (SortOrder == Html.GetSortAsc("CounterPartyDetailId")) query = query.OrderBy(q => q.CounterPartyDetail.Code);
            else if (SortOrder == Html.GetSortDsc("CounterPartyDetailId")) query = query.OrderByDescending(q => q.CounterPartyDetail.Code);
            else if (SortOrder == Html.GetSortAsc("InwardRetroPartyDetailId")) query = query.OrderBy(q => q.InwardRetroPartyDetail.Code);
            else if (SortOrder == Html.GetSortDsc("InwardRetroPartyDetailId")) query = query.OrderByDescending(q => q.InwardRetroPartyDetail.Code);
            else if (SortOrder == Html.GetSortAsc("DocumentType")) query = query.OrderBy(q => q.DocumentType);
            else if (SortOrder == Html.GetSortDsc("DocumentType")) query = query.OrderByDescending(q => q.DocumentType);
            else if (SortOrder == Html.GetSortAsc("DocumentId")) query = query.OrderBy(q => q.DocumentId);
            else if (SortOrder == Html.GetSortDsc("DocumentId")) query = query.OrderByDescending(q => q.DocumentId);
            else if (SortOrder == Html.GetSortAsc("TypeOfBusiness")) query = query.OrderBy(q => q.TypeOfBusiness);
            else if (SortOrder == Html.GetSortDsc("TypeOfBusiness")) query = query.OrderByDescending(q => q.TypeOfBusiness);
            else if (SortOrder == Html.GetSortAsc("Description")) query = query.OrderBy(q => q.Description);
            else if (SortOrder == Html.GetSortDsc("Description")) query = query.OrderByDescending(q => q.Description);
            else if (SortOrder == Html.GetSortAsc("EffectiveAtStr")) query = query.OrderBy(q => q.EffectiveAt);
            else if (SortOrder == Html.GetSortDsc("EffectiveAtStr")) query = query.OrderByDescending(q => q.EffectiveAt);
            else if (SortOrder == Html.GetSortAsc("PersonInChargeId")) query = query.OrderBy(q => q.PersonInChargeId);
            else if (SortOrder == Html.GetSortDsc("PersonInChargeId")) query = query.OrderByDescending(q => q.PersonInChargeId);
            else if (SortOrder == Html.GetSortAsc("CoverageStatus")) query = query.OrderByDescending(q => q.CoverageStatus);
            else if (SortOrder == Html.GetSortDsc("CoverageStatus")) query = query.OrderByDescending(q => q.CoverageStatus);
            else if (SortOrder == Html.GetSortAsc("Description")) query = query.OrderBy(q => q.Description);
            else if (SortOrder == Html.GetSortDsc("OrionGroupStr")) query = query.OrderByDescending(q => q.OrionGroupStr);
            else if (SortOrder == Html.GetSortAsc("OrionGroupStr")) query = query.OrderBy(q => q.OrionGroupStr);
            else if (SortOrder == Html.GetSortDsc("DocumentStatus")) query = query.OrderByDescending(q => q.DocumentStatus);
            else if (SortOrder == Html.GetSortAsc("DocumentStatus")) query = query.OrderBy(q => q.DocumentStatus);
            else if (SortOrder == Html.GetSortDsc("DraftingStatus")) query = query.OrderByDescending(q => q.DraftingStatus);
            else if (SortOrder == Html.GetSortAsc("DraftingStatus")) query = query.OrderBy(q => q.DraftingStatus);
            else if (SortOrder == Html.GetSortDsc("DraftingStatusCategory")) query = query.OrderByDescending(q => q.DraftingStatusCategory);
            else if (SortOrder == Html.GetSortAsc("DraftingStatusCategory")) query = query.OrderBy(q => q.DraftingStatusCategory);
            else if (SortOrder == Html.GetSortDsc("CountryOrigin")) query = query.OrderByDescending(q => q.CountryOrigin);
            else if (SortOrder == Html.GetSortAsc("CountryOrigin")) query = query.OrderBy(q => q.CountryOrigin);
            else if (SortOrder == Html.GetSortDsc("LatestRevisionDateStr")) query = query.OrderByDescending(q => q.LatestRevisionDate);
            else if (SortOrder == Html.GetSortAsc("LatestRevisionDateStr")) query = query.OrderBy(q => q.LatestRevisionDate);
            else query = query.OrderBy(q => q.Id);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        public void IndexPage()
        {
            DropDownCedant();
            DropDownTreatyWorkflowDocumentType();
            DropDownNewCedant();
            DropDownReinsuranceType();
            DropDownRetroParty();
            DropDownBusinessType();
            DropDownTreatyPricingObjectModules();


            int departmentTP = Util.GetConfigInteger("ProductPricingAndTreaty");
            DropDownUser(UserBo.StatusActive, null, false, departmentTP);

            DropDownOrionGroup();
            DropDownCoverageStatus();
            DropDownDocumentStatus();
            DropDownDraftingStatus();
            DropDownDraftingStatusCategory();

            SetViewBagMessage();
        }

        // Post: TreatyPricingTreatyWorkflow
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Add(TreatyPricingTreatyWorkflowBo bo)
        {
            List<string> errors = new List<string>();
            IList<TreatyPricingTreatyWorkflowBo> bos = null;

            try
            {
                bo.CreatedById = AuthUserId;
                bo.UpdatedById = AuthUserId;
                bo.DocumentStatus = TreatyPricingTreatyWorkflowBo.DocumentStatusUnassigned;
                bo.DraftingStatus = TreatyPricingTreatyWorkflowBo.DraftingStatusUnassigned;
                bo.DraftingStatusCategory = TreatyPricingTreatyWorkflowBo.DraftingStatusCategoryUnassigned;
                bo.LatestVersion = 1;

                if (bo.DraftingStatus > 0)
                {
                    bo.DraftingStatusCategory = TreatyPricingTreatyWorkflowService.GetDraftingStatusCategory(bo.DraftingStatus);
                }

                TrailObject trail = GetNewTrailObject();

                Result = TreatyPricingTreatyWorkflowService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    #region Create Version
                    var TreatyWorkflowVersionBo = new TreatyPricingTreatyWorkflowVersionBo()
                    {
                        TreatyPricingTreatyWorkflowId = bo.Id,
                        Version = 1,
                        PersonInChargeId = AuthUserId,
                        CreatedById = AuthUserId,
                        UpdatedById = AuthUserId,
                    };
                    TreatyPricingTreatyWorkflowVersionService.Create(ref TreatyWorkflowVersionBo, ref trail);
                    #endregion

                    #region Create Linked Objects
                    if (bo.TreatyPricingWorkflowObjectBos != null)
                        TreatyPricingWorkflowObjectController.Create(bo.TreatyPricingWorkflowObjectBos, TreatyPricingWorkflowObjectBo.TypeTreaty, bo.Id, AuthUserId, ref trail);
                    #endregion

                    CreateTrail(
                        bo.Id,
                        "Create Treaty Pricing Treaty Workflow"
                    );

                    bos = TreatyPricingTreatyWorkflowService.GetAll();
                }
            }
            catch (Exception ex)
            {
                errors.Add(string.Format("Error occurred. Error details: {0}", ex.Message));
            }

            return Json(new { errors, bos, bo });
        }


        // GET: TreatyPricingCedant/Edit/5
        public ActionResult Edit(int id, bool isEditMode = false)
        {
            if (!CheckObjectLockReadOnly(Controller, id, isEditMode))
                return RedirectDashboard();

            var bo = TreatyPricingTreatyWorkflowService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            TreatyPricingTreatyWorkflowViewModel model = new TreatyPricingTreatyWorkflowViewModel(bo);
            LoadPage(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public virtual ActionResult Edit(int id, TreatyPricingTreatyWorkflowViewModel model)
        {
            TreatyPricingTreatyWorkflowBo bo = TreatyPricingTreatyWorkflowService.Find(id);
            if (string.IsNullOrEmpty(model.Reviewer) && !string.IsNullOrEmpty(bo.Reviewer))
                model.Reviewer = bo.Reviewer;
            
            if (bo == null)
            {
                return RedirectToAction("Index", "TreatyPricingTreatyWorkflow");
            }

            if (!CheckObjectLock(Controller, id))
                return RedirectToAction("Edit", new { id = id });

            if (model.DocumentStatus == TreatyPricingTreatyWorkflowBo.DocumentStatusUnassigned)
            {
                model.DraftingStatus = TreatyPricingTreatyWorkflowBo.DraftingStatusUnassigned;
            }

            model.DraftingStatus = bo.DraftingStatus;
            model.DraftingStatusCategory = TreatyPricingTreatyWorkflowService.GetDraftingStatusCategory(bo.DraftingStatus);


            if (model.DraftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusPendingTreatyPeerReview1st || model.DraftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusPendingGroupPricingPICReview1st || model.DraftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusPendingProductPricingPICReview1st)
            {
                model.DateSentToReviewer1stStr = DateTime.Now.ToString(Util.GetDateFormat());
            }

            if (model.DraftingStatus == TreatyPricingTreatyWorkflowBo.DraftingStatusEXTFirstDraftSent)
            {
                model.DateSentToClient1stStr = DateTime.Now.ToString(Util.GetDateFormat());
            }


            if (bo.DraftingStatus > 0)
            {
                bo.DraftingStatusCategory = TreatyPricingTreatyWorkflowService.GetDraftingStatusCategory(bo.DraftingStatus);
            }

            if (!string.IsNullOrEmpty(model.EffectiveAtStr))
            {
                if (model.DocumentStatus == TreatyPricingTreatyWorkflowBo.DocumentStatusSigned)
                {
                    model.OrionGroupStr = "";
                }
                else
                {
                    var orionGroup = TreatyPricingTreatyWorkflowService.GenerateOrionGroup(model.EffectiveAtStr);
                    if (orionGroup == 1)
                    {
                        model.OrionGroupStr = "<= 6 months";
                    }
                    else if (orionGroup == 2)
                    {
                        model.OrionGroupStr = "<= 12 months";
                    }
                    else if (orionGroup == 3)
                    {
                        model.OrionGroupStr = "> 12 months";
                    }
                }
            }

            TreatyPricingTreatyWorkflowVersionBo updatedVersionBo = null;
            var currentVersionBo = TreatyPricingTreatyWorkflowVersionService.Find(model.CurrentVersionObjectId);
            bo.SetVersionObjects(bo.TreatyPricingTreatyWorkflowVersionBos);
            if (bo.EditableVersion != model.CurrentVersion)
            {
                ModelState.AddModelError("", "You can only update details for the latest version");
            }
            else
            {
                updatedVersionBo = model.GetVersionBo((TreatyPricingTreatyWorkflowVersionBo)bo.CurrentVersionObject);
                bo.AddVersionObject(updatedVersionBo);
            }

            if (ModelState.IsValid)
            {
                bo = model.FormBo(bo);
                bo.UpdatedById = AuthUserId;

                TrailObject trail = GetNewTrailObject();
                Result = TreatyPricingTreatyWorkflowService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    if (updatedVersionBo != null)
                    {
                        updatedVersionBo.UpdatedById = AuthUserId;
                        updatedVersionBo.LatestRevisionDate = DateTime.Now; 
                        if (updatedVersionBo.PersonInChargeId.HasValue && updatedVersionBo.PersonInChargeId != currentVersionBo.PersonInChargeId)
                        {
                            NewPicAssigned(updatedVersionBo, trail);
                        }

                        TreatyPricingTreatyWorkflowVersionService.Update(ref updatedVersionBo, ref trail);
                    }

                    CreateTrail(
                        bo.Id,
                        "Update Treaty Pricing Treaty Workflow"
                    );

                    ObjectLockController.DeleteObjectLock(Controller, id, AuthUserId);

                    SetUpdateSuccessMessage("Treaty Workflow", false);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }
            else
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            }

            model.CurrentVersion = bo.CurrentVersion;
            model.CurrentVersionObject = bo.CurrentVersionObject;
            model.VersionObjects = bo.VersionObjects;
            model.TreatyPricingWorkflowObjectBos = bo.TreatyPricingWorkflowObjectBos;
            LoadPage(model);
            return View(model);
        }

        public void LoadPage(TreatyPricingTreatyWorkflowViewModel model)
        {
            DropDownCedant();
            DropDownRetroParty();
            DropDownReinsuranceType();
            DropDownBusinessOrigin();
            DropDownBusinessType();
            DropDownTreatyWorkflowDocumentType();
            DropDownDocumentStatus();
            DropDownDraftingStatus();
            DropDownDraftingStatusCategory();
            DropDownCoverageStatus();
            TargetSentDatePh();
            DropDownVersion(model);
            int departmentTP = Util.GetConfigInteger("ProductPricingAndTreaty");
            DropDownUser(UserBo.StatusActive, null, false, departmentTP);
            DropDownTreatyPricingObjectModules();
            AuthUserName();
            // Remark & Document
            string downloadDocumentUrl = Url.Action("Download", "Document");
            GetRemarkDocument(model.ModuleId, model.Id, downloadDocumentUrl);
            GetRemarkSubjects();
            ViewBag.PersonInChargeId = AuthUserId;
            ViewBag.PersonInChargeName = UserService.Find(AuthUserId).FullName;

            GetStatusHistories(model.ModuleId, model.Id, downloadDocumentUrl);
            ViewBag.RecipientEmails = UserService.GetEmailUsers();

            GetFullBaseUrl();

            SetViewBagMessage();
        }

        public ActionResult CreateVersion(TreatyPricingTreatyWorkflowBo bo, bool duplicatePreviousVersion = false)
        {
            bo.SetVersionObjects(TreatyPricingTreatyWorkflowVersionService.GetByTreatyPricingTreatyWorkflowId(bo.Id));
            TreatyPricingTreatyWorkflowVersionBo nextVersionBo;
            TreatyPricingTreatyWorkflowVersionBo previousVersionBo = (TreatyPricingTreatyWorkflowVersionBo)bo.CurrentVersionObject;

            List<string> errors = new List<string>();
            errors.AddRange(ValidateVersion(previousVersionBo));
            if (!errors.IsNullOrEmpty())
            {
                return Json(new { errors });
            }

            if (duplicatePreviousVersion)
            {
                nextVersionBo = new TreatyPricingTreatyWorkflowVersionBo(previousVersionBo)
                {
                    Id = 0,
                };
            }
            else
            {
                nextVersionBo = new TreatyPricingTreatyWorkflowVersionBo()
                {
                    TreatyPricingTreatyWorkflowId = bo.Id,
                };
            }

            nextVersionBo.Version = bo.EditableVersion + 1;
            nextVersionBo.PersonInChargeId = AuthUserId;
            nextVersionBo.CreatedById = AuthUserId;
            nextVersionBo.UpdatedById = AuthUserId;

            ObjectVersionChangelog changelog = null;

            TrailObject trail = GetNewTrailObject();
            Result = TreatyPricingTreatyWorkflowVersionService.Create(ref nextVersionBo, ref trail);
            if (Result.Valid)
            {
                UserTrailBo userTrail = CreateTrail(
                    nextVersionBo.Id,
                    "Create Treaty Pricing Treaty Workflow Version"
                );

                changelog = new ObjectVersionChangelog(nextVersionBo, userTrail);
                changelog.FormatBetweenVersionTrail(previousVersionBo);
            }

            bo.AddVersionObject(nextVersionBo);
            var versions = DropDownVersion(bo);

            return Json(new { bo, versions, changelog });
        }

        public static List<string> ValidateVersion(TreatyPricingTreatyWorkflowVersionBo versionBo)
        {
            List<string> errors = new List<string>();
            TreatyPricingTreatyWorkflowViewModel model = new TreatyPricingTreatyWorkflowViewModel();

            List<string> requiredProperties = model.GetType().GetProperties().Where(q => Attribute.IsDefined(q, typeof(RequiredVersion))).Select(q => q.Name).ToList();
            foreach (string propertyName in requiredProperties)
            {
                object value = versionBo.GetPropertyValue(propertyName);
                if (value == null || string.IsNullOrEmpty(value.ToString()))
                {
                    string displayName = model.GetAttributeFrom<DisplayNameAttribute>(propertyName).DisplayName;

                    errors.Add(string.Format(MessageBag.Required, displayName.Replace(" (Separated by comma)", string.Empty)));
                }
            }

            return errors;
        }

        public List<SelectListItem> DropDownDocumentStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingTreatyWorkflowBo.DocumentStatusUnassigned))
            {
                items.Add(new SelectListItem { Text = TreatyPricingTreatyWorkflowBo.GetDocumentStatusName(i), Value = i.ToString() });
            }
            ViewBag.DocumentStatuss = items;
            return items;
        }

        public List<SelectListItem> DropDownDraftingStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingTreatyWorkflowBo.DraftingStatusCancelled))
            {
                items.Add(new SelectListItem { Text = TreatyPricingTreatyWorkflowBo.GetDraftingStatusName(i), Value = i.ToString() });
            }
            ViewBag.DraftingStatuss = items;
            return items;
        }
        public List<SelectListItem> DropDownDraftingStatusCategory()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingTreatyWorkflowBo.DraftingStatusCategoryCancelled))
            {
                items.Add(new SelectListItem { Text = TreatyPricingTreatyWorkflowBo.GetDraftingStatusCategoryName(i), Value = i.ToString() });
            }
            ViewBag.DraftingStatusCategories = items;
            return items;
        }

        public List<SelectListItem> DropDownCoverageStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingTreatyWorkflowBo.CoverageStatusTerminated))
            {
                items.Add(new SelectListItem { Text = TreatyPricingTreatyWorkflowBo.GetCoverageStatusName(i), Value = i.ToString() });
            }
            ViewBag.CoverageStatuss = items;
            return items;
        }

        public List<SelectListItem> DropDownOrionGroup()
        {
            var items = GetEmptyDropDownList();
            items.Add(new SelectListItem { Text = "> 12 months", Value = "> 12 months" });
            items.Add(new SelectListItem { Text = "<= 12 months", Value = "<= 12 months" });
            items.Add(new SelectListItem { Text = "<= 6 months", Value = "<= 6 months" });

            ViewBag.OrionGroupSort = items;
            return items;
        }

        [HttpPost]
        public JsonResult UpdateStatus(StatusHistoryBo statusHistoryBo)
        {
            TreatyPricingTreatyWorkflowBo bo = TreatyPricingTreatyWorkflowService.Find(statusHistoryBo.ObjectId);
            if (bo == null)
                return Json(new { });

            string[] emails = null;
            if (!string.IsNullOrEmpty(statusHistoryBo.Emails))
            {
                List<string> error = new List<string>();

                emails = statusHistoryBo.Emails.Split(',').Select(e => e.Trim()).ToArray();
                foreach (string email in emails)
                {
                    if (!email.Trim().IsValidEmail())
                    {
                        error.Add(string.Format(MessageBag.InvalidEmail, email));
                    }
                }

                if (error.Count() > 0)
                    return Json(new { error });
            }

            bo.DraftingStatus = statusHistoryBo.Status;
            bo.DraftingStatusCategory = TreatyPricingTreatyWorkflowService.GetDraftingStatusCategory(statusHistoryBo.Status);
            bo.UpdatedById = AuthUserId;

            TrailObject trail = GetNewTrailObject();
            Result = TreatyPricingTreatyWorkflowService.Update(ref bo, ref trail);
            if (Result.Valid)
            {
                statusHistoryBo = StatusHistoryController.Create(statusHistoryBo, AuthUserId, AuthUser.UserName, ref trail);

                if (emails != null)
                {
                    List<string> recipientNames = new List<string>();
                    //string[] emails = statusHistoryBo.Emails.Split(',');
                    string reviewer = ""; 

                    string link = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority + Url.Action("Edit", "TreatyPricingTreatyWorkflow", new { id = bo.Id });

                    foreach (string email in emails)
                    {
                        UserBo userBo = UserService.FindByEmail(email);
                        string fullName = "";
                        if (userBo != null)
                            fullName = " " + userBo.FullName;

                        GetNewEmail(EmailBo.TypeNotifyTreatyWorkflowStatusUpdate, email, userBo?.Id);

                        EmailBo.AddData(fullName);
                        EmailBo.AddData(bo.DocumentId);
                        EmailBo.AddData(statusHistoryBo.StatusName);
                        EmailBo.AddData(link);
                        EmailBo.AddData(statusHistoryBo.RemarkBo?.Content ?? "");
                        GenerateMail(showWarning: false);

                        EmailBo.ModuleController = ModuleBo.ModuleController.StatusHistory.ToString();
                        EmailBo.ObjectId = statusHistoryBo.Id;
                        SaveEmail(ref trail);

                        recipientNames.Add(userBo?.FullName ?? email);
                        if (userBo != null)
                        {
                            if (!string.IsNullOrEmpty(reviewer))
                            {
                                reviewer = reviewer + " , " + userBo.Id.ToString();
                            }
                            else
                            {
                                reviewer = userBo.Id.ToString() + " ";
                            }
                        }
                    }

                    statusHistoryBo.RecipientNames = string.Join(", ", recipientNames);
                    bo.Reviewer = reviewer;
                    TreatyPricingTreatyWorkflowService.Update(ref bo, ref trail);
                }

                CreateTrail(
                    bo.Id,
                    "Update Treaty Pricing Treaty Workflow Status"
                );

                return Json(new { statusHistoryBo });
            }
            return Json(new { error = Result.MessageBag.Errors });
        }

        public JsonResult NewPicAssigned(TreatyPricingTreatyWorkflowVersionBo verBo, TrailObject trail, TreatyPricingTreatyWorkflowBo bo = null)
        {
            var userBo = UserService.Find(verBo.PersonInChargeId);

            GetNewEmail(EmailBo.TypeNotifyTreatyWorkflowPicUpdate, userBo.Email, userBo.Id);
            string link = "";

            EmailBo.AddData(userBo.UserName);
            if (bo != null)
            {
                EmailBo.AddData(bo.DocumentId);
                link = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority + Url.Action("Edit", "TreatyPricingTreatyWorkflow", new { id = bo.Id });
                EmailBo.AddData(link);
            }
            else
            {
                EmailBo.AddData(verBo.TreatyPricingTreatyWorkflowBo.DocumentId);
                link = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority + Url.Action("Edit", "TreatyPricingTreatyWorkflow", new { id = verBo.TreatyPricingTreatyWorkflowBo.Id });
                EmailBo.AddData(link);

            }


            GenerateMail(showWarning: false);

            EmailBo.ModuleController = ModuleBo.ModuleController.TreatyPricingTreatyWorkflowVersion.ToString();
            EmailBo.ObjectId = verBo.Id;
            SaveEmail(ref trail);

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult OrionGroup(string effectiveAt)
        {
            return Json(new { OrionGroup = TreatyPricingTreatyWorkflowService.GenerateOrionGroup(effectiveAt) });
        }

        [HttpPost]
        public string AssignPersonInCharge(int id, int personInChargeId)
        {
            var bo = TreatyPricingTreatyWorkflowService.Find(id);

            if (bo != null)
            {
                bo.UpdatedById = AuthUserId;

                if (bo.DocumentStatus == TreatyPricingTreatyWorkflowBo.DocumentStatusUnassigned)
                {
                    bo.DocumentStatus = TreatyPricingTreatyWorkflowBo.DocumentStatusDrafting;
                    bo.DraftingStatus = TreatyPricingTreatyWorkflowBo.DraftingStatusDrafting;
                    bo.DraftingStatusCategory = TreatyPricingTreatyWorkflowService.GetDraftingStatusCategory(bo.DraftingStatus);
                }

                var verBo = bo.TreatyPricingTreatyWorkflowVersionBos.OrderByDescending(a => a.Id).FirstOrDefault();
                verBo.PersonInChargeId = personInChargeId;
                verBo.LatestRevisionDate = DateTime.Now.Date;

                TrailObject trail = GetNewTrailObject();
                NewPicAssigned(verBo, trail, bo);
                Result = TreatyPricingTreatyWorkflowService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    if (verBo != null)
                    {
                        verBo.UpdatedById = AuthUserId;
                        TreatyPricingTreatyWorkflowVersionService.Update(ref verBo, ref trail);
                    }

                    CreateTrail(
                        bo.Id,
                        "Update Treaty Pricing Treaty Workflow"
                    );
                    
                }
                else
                {
                    return Result.MessageBag.Errors.ToString();
                }
            }
            else
            {
                return "Treaty Workflow not found. ";
            }
            return "";
        }
        
        [HttpPost]
        public void UpdateLatestRevisionDate(int id, int version, string date)
        {
            var bo = TreatyPricingTreatyWorkflowVersionService.FindByTreatyPricingTreatyWorkflowIdVersion(id, version);
            var latestRevisionDate = Util.GetParseDateTime(date);

            if (bo != null && latestRevisionDate.HasValue)
            {
                bo.LatestRevisionDate = latestRevisionDate.Value;

                TrailObject trail = GetNewTrailObject();
                Result = TreatyPricingTreatyWorkflowVersionService.Update(ref bo, ref trail);

                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Update Treaty Pricing Treaty Workflow Version's Latest Revision Date"
                    );
                }
            }
        }

        public List<string> TargetSentDatePh()
        {
            var publicHoliday = PublicHolidayDetailService.Get();

            var disableDates = new List<string>();

            foreach (var i in publicHoliday)
            {
                disableDates.Add(i.PublicHolidayDate.ToString("dd-MM-yyyy"));
            }

            ViewBag.TargetSentDateDisableDates = disableDates;

            return disableDates;

        }



        public ActionResult Download(
            string downloadToken,
            int type,
            string ReinsuranceTypePickListDetailId,
            string InwardRetroPartyDetailId,
            string DocumentId,
            string DocumentType,
            string TypeOfBusiness,
            string Description,
            string TreatyCode,
            string EffectiveAtStr,
            string CoverageStatus,
            string DocumentStatus,
            string DraftingStatus,
            string CountryOrigin,
            string DraftingStatusCategory
            )
        {
            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            dynamic Params = new ExpandoObject();
            Params.ReinsuranceTypePickListDetailId = ReinsuranceTypePickListDetailId;
            Params.InwardRetroPartyDetailId = InwardRetroPartyDetailId;
            Params.DocumentId = DocumentId;
            Params.DocumentType = DocumentType;
            Params.TypeOfBusiness = TypeOfBusiness;
            Params.Description = Description;
            Params.TreatyCode = TreatyCode;
            Params.EffectiveAtStr = EffectiveAtStr;
            Params.CoverageStatus = CoverageStatus;
            Params.DocumentStatus = DocumentStatus;
            Params.DraftingStatus = DraftingStatus;
            Params.CountryOrigin = CountryOrigin;
            Params.DraftingStatusCategory = DraftingStatusCategory;

            var export = new GenerateExportData();
            ExportBo exportBo = export.CreateExportTreatyWorkflow(AuthUserId, Params);

            if (exportBo.Total <= export.ExportInstantDownloadLimit)
            {
                export.Process(ref exportBo, _db);
                Session.Add("DownloadExport", true);
            }

            return RedirectToAction("Edit", "Export", new { exportBo.Id });
        }
    }
}