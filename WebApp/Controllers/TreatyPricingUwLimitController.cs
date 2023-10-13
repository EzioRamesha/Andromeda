using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using Services.Identity;
using Services.TreatyPricing;
using Shared;
using Shared.Forms.Attributes;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class TreatyPricingUwLimitController : BaseController
    {
        public const string Controller = "TreatyPricingUwLimit";

        [HttpPost]
        public JsonResult GetByTreatyPricingCedant(int? id)
        {
            IList<TreatyPricingUwLimitBo> bos = new List<TreatyPricingUwLimitBo>();
            if (id.HasValue)
            {
                bos = TreatyPricingUwLimitService.GetByTreatyPricingCedantId(id.Value);
            }
            return Json(new { bos });
        }

        [HttpPost]
        public JsonResult GetByTreatyPricingUwLimit(int? id)
        {
            IList<TreatyPricingUwLimitVersionBo> versionBos = new List<TreatyPricingUwLimitVersionBo>();
            if (id.HasValue)
            {
                versionBos = TreatyPricingUwLimitVersionService.GetByTreatyPricingUwLimitId(id.Value).ToList();
            }
            return Json(new { versionBos });
        }

//        [Auth(Controller = Controller, Power = "C")]
        [HttpPost]
        public JsonResult Add(TreatyPricingUwLimitBo uwLimitBo)
        {
            List<string> errors = new List<string>();

            if (!CheckPower(Controller, "C"))
            {
                errors.Add(string.Format(MessageBag.AccessDeniedWithActionName, "Add", "Underwriting Limit"));
                return Json(new { errors });
            }

            try
            {
                TreatyPricingUwLimitVersionBo versionBo = new TreatyPricingUwLimitVersionBo();
                if (!uwLimitBo.DuplicateFromList && string.IsNullOrEmpty(uwLimitBo.Name))
                {
                    errors.Add("Underwriting Limit Name is required");
                }

                if (uwLimitBo.IsDuplicateExisting)
                {
                    if (!uwLimitBo.DuplicateTreatyPricingUwLimitId.HasValue)
                    {
                        errors.Add("Underwriting Limit is required");
                    }

                    if (!uwLimitBo.DuplicateFromList && !uwLimitBo.DuplicateTreatyPricingUwLimitVersionId.HasValue)
                    {
                        errors.Add("Version is required");
                    }

                    if (errors.IsNullOrEmpty())
                    {
                        TreatyPricingUwLimitBo duplicate = TreatyPricingUwLimitService.Find(uwLimitBo.DuplicateTreatyPricingUwLimitId);
                        if (duplicate == null)
                        {
                            errors.Add("Underwriting Limit not found");
                        }
                        else
                        {
                            uwLimitBo.BenefitCode = duplicate.BenefitCode;

                            TrailObject trail = GetNewTrailObject();

                            TreatyPricingUwLimitVersionBo duplicateVersion = null;
                            if (uwLimitBo.DuplicateTreatyPricingUwLimitVersionId.HasValue)
                            {
                                duplicateVersion = duplicate.TreatyPricingUwLimitVersionBos.Where(q => q.Id == uwLimitBo.DuplicateTreatyPricingUwLimitVersionId.Value).FirstOrDefault();
                            }
                            else
                            {
                                duplicateVersion = duplicate.TreatyPricingUwLimitVersionBos.OrderByDescending(q => q.Version).First();
                            }

                            if (duplicateVersion == null)
                            {
                                errors.Add("Underwriting Limit Version not found");
                            }
                            else
                            {
                                versionBo = new TreatyPricingUwLimitVersionBo(duplicateVersion)
                                {
                                    Id = 0
                                };
                            }
                        }
                    }
                }

                if (errors.IsNullOrEmpty())
                {
                    uwLimitBo.Status = TreatyPricingUwLimitBo.StatusActive;
                    uwLimitBo.LimitId = TreatyPricingUwLimitService.GetNextObjectId(uwLimitBo.TreatyPricingCedantId);
                    uwLimitBo.CreatedById = AuthUserId;
                    uwLimitBo.UpdatedById = AuthUserId;

                    TrailObject trail = GetNewTrailObject();

                    Result = TreatyPricingUwLimitService.Create(ref uwLimitBo, ref trail);
                    if (Result.Valid)
                    {
                        versionBo.TreatyPricingUwLimitId = uwLimitBo.Id;
                        versionBo.Version = 1;
                        versionBo.PersonInChargeId = AuthUserId;
                        versionBo.CreatedById = AuthUserId;
                        versionBo.UpdatedById = AuthUserId;
                        TreatyPricingUwLimitVersionService.Create(ref versionBo, ref trail);

                        CreateTrail(
                            uwLimitBo.Id,
                            "Create Treaty Pricing Underwriting Limit"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(string.Format("Error occurred. Error details: {0}", ex.Message));
            }

            var uwLimitBos = TreatyPricingUwLimitService.GetByTreatyPricingCedantId(uwLimitBo.TreatyPricingCedantId);

            return Json(new { errors, uwLimitBos });
        }

        // GET: TreatyPricingUwLimit/Edit/5
        public ActionResult Edit(int id, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false, int workflowId = 0, bool isEditMode = false)
        {
            if (!CheckObjectLockReadOnly(Controller, id, isEditMode))
                return RedirectDashboard();

            var uwLimitBo = TreatyPricingUwLimitService.Find(id);
            if (uwLimitBo == null)
            {
                return RedirectToAction("Index");
            }

            var model = new TreatyPricingUwLimitViewModel(uwLimitBo);
            model.WorkflowId = workflowId;
            LoadPage(model, versionId, isCalledFromWorkflow, isQuotationWorkflow);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, TreatyPricingUwLimitViewModel model, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false, int workflowId = 0)
        {
            TreatyPricingUwLimitBo uwLimitBo = TreatyPricingUwLimitService.Find(id);
            if (uwLimitBo == null)
            {
                return RedirectToAction("Edit", "TreatyPricingCedant", new { id = model.TreatyPricingCedantId });
            }

            object routeValues = new { id = uwLimitBo.Id };
            if (isCalledFromWorkflow)
            {
                routeValues = new { id = uwLimitBo.Id, versionId, isCalledFromWorkflow, isQuotationWorkflow, workflowId };
            }

            if (!CheckObjectLock(Controller, id))
                return RedirectToAction("Edit", routeValues);

            TreatyPricingUwLimitVersionBo updatedVersionBo = null;
            uwLimitBo.SetVersionObjects(uwLimitBo.TreatyPricingUwLimitVersionBos);
            if (uwLimitBo.EditableVersion == model.CurrentVersion)
            {
                updatedVersionBo = model.GetVersionBo((TreatyPricingUwLimitVersionBo)uwLimitBo.CurrentVersionObject);
                uwLimitBo.AddVersionObject(updatedVersionBo);
            }

            if (ModelState.IsValid)
            {
                uwLimitBo = model.FormBo(uwLimitBo);
                uwLimitBo.UpdatedById = AuthUserId;

                TrailObject trail = GetNewTrailObject();
                Result = TreatyPricingUwLimitService.Update(ref uwLimitBo, ref trail);
                if (Result.Valid)
                {
                    if (updatedVersionBo != null)
                    {
                        updatedVersionBo.UpdatedById = AuthUserId;
                        TreatyPricingUwLimitVersionService.Update(ref updatedVersionBo, ref trail);
                    }

                    CreateTrail(
                        uwLimitBo.Id,
                        "Update Underwriting Limit"
                    );

                    SetUpdateSuccessMessage("Underwriting Limit", false);

                    ObjectLockController.DeleteObjectLock(Controller, id, AuthUserId);

                    return RedirectToAction("Edit", routeValues);
                }
                AddResult(Result);
            }

            model.CurrentVersion = uwLimitBo.CurrentVersion;
            model.CurrentVersionObject = uwLimitBo.CurrentVersionObject;
            model.VersionObjects = uwLimitBo.VersionObjects;
            model.TreatyPricingCedantBo = uwLimitBo.TreatyPricingCedantBo;
            model.WorkflowId = workflowId;
            LoadPage(model, versionId, isCalledFromWorkflow, isQuotationWorkflow);
            return View(model);
        }

        public void LoadPage(TreatyPricingUwLimitViewModel model, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false)
        {
            DropDownCurrencyCode(true);
            DropDownStatus();
            DropDownVersion(model);
            DropDownUser(UserBo.StatusActive, model.PersonInChargeId);
            GetBenefitCodes();

            AuthUserName();

            ViewBag.PersonInChargeId = AuthUserId;
            ViewBag.PersonInChargeName = UserService.Find(AuthUserId).FullName;
            ViewBag.IsCalledFromWorkflow = isCalledFromWorkflow;
            ViewBag.IsHideSideBar = isCalledFromWorkflow;
            ViewBag.IsQuotationWorkflow = isQuotationWorkflow;
            ViewBag.VersionId = versionId;

            if (model.Id > 0)
            {
                GetProduct(model.Id);
            }

            if (versionId > 0)
            {
                model.SetCurrentVersionObject(versionId);
            }

            // Remark & Document
            string downloadDocumentUrl = Url.Action("Download", "Document");
            GetRemarkDocument(model.ModuleId, model.TreatyPricingCedantId, downloadDocumentUrl, ModuleBo.ModuleController.TreatyPricingUwLimit.ToString(), model.Id);
            GetRemarkSubjects();

            GetObjectVersionChangelog(ModuleBo.ModuleController.TreatyPricingUwLimit.ToString(), model.Id, model);
            DropDownWorkflowObjectTypes();
            DropDownWorkflowStatus();

            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingUwLimitBo.StatusMax))
            {
                items.Add(new SelectListItem { Text = TreatyPricingUwLimitBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.StatusItems = items;
            return items;
        }

        public ActionResult CreateVersion(TreatyPricingUwLimitBo bo, bool duplicatePreviousVersion = false)
        {
            bo.SetVersionObjects(TreatyPricingUwLimitVersionService.GetByTreatyPricingUwLimitId(bo.Id));
            TreatyPricingUwLimitVersionBo nextVersionBo;
            TreatyPricingUwLimitVersionBo previousVersionBo = (TreatyPricingUwLimitVersionBo)bo.CurrentVersionObject;

            List<string> errors = new List<string>();
            errors.AddRange(ValidateVersion(previousVersionBo));
            if (!errors.IsNullOrEmpty())
            {
                return Json(new { errors });
            }

            if (duplicatePreviousVersion)
            {
                nextVersionBo = new TreatyPricingUwLimitVersionBo(previousVersionBo)
                {
                    Id = 0,
                };
            }
            else
            {
                nextVersionBo = new TreatyPricingUwLimitVersionBo()
                {
                    TreatyPricingUwLimitId = bo.Id,
                };
            }

            nextVersionBo.Version = bo.EditableVersion + 1;
            nextVersionBo.PersonInChargeId = AuthUserId;
            nextVersionBo.CreatedById = AuthUserId;
            nextVersionBo.UpdatedById = AuthUserId;

            ObjectVersionChangelog changelog = null;

            TrailObject trail = GetNewTrailObject();
            Result = TreatyPricingUwLimitVersionService.Create(ref nextVersionBo, ref trail);
            if (Result.Valid)
            {
                UserTrailBo userTrail = CreateTrail(
                    nextVersionBo.Id,
                    "Create Treaty Pricing Underwriting Limit Version"
                );

                changelog = new ObjectVersionChangelog(nextVersionBo, userTrail);
                changelog.FormatBetweenVersionTrail(previousVersionBo);
            }

            bo.AddVersionObject(nextVersionBo);
            var versions = DropDownVersion(bo);

            return Json(new { bo, versions, changelog });
        }

        public static List<string> ValidateVersion(TreatyPricingUwLimitVersionBo versionBo)
        {
            List<string> errors = new List<string>();
            TreatyPricingUwLimitViewModel model = new TreatyPricingUwLimitViewModel();

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

        [HttpPost]
        public JsonResult Search(int cedantId, string productName)
        {
            var uwLimit = TreatyPricingUwLimitService.GetCodeByTreatyPricingCedantIdProductName(cedantId, productName);
            return Json(new { UwLimit = uwLimit });
        }

        public void GetProduct(int id)
        {
            var productBos = TreatyPricingProductService.GetUwLimitProduct(id);
            ViewBag.Products = productBos;
        }
    }
}