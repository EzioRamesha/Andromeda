using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using Services;
using Services.Identity;
using Services.TreatyPricing;
using Shared;
using Shared.DataAccess;
using Shared.Forms.Attributes;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class TreatyPricingDefinitionAndExclusionController : BaseController
    {
        public const string Controller = "TreatyPricingDefinitionAndExclusion";

        //[Auth(Controller = Controller, Power = "C")]
        [HttpPost]
        public JsonResult Add(TreatyPricingDefinitionAndExclusionBo definitionAndExclusionBo)
        {
            List<string> errors = new List<string>();

            if (!CheckPower(Controller, "C"))
            {
                errors.Add(string.Format(MessageBag.AccessDeniedWithActionName, "Add", "Definitions & Exclusions"));
                return Json(new { errors });
            }

            try
            {
                TreatyPricingDefinitionAndExclusionVersionBo versionBo = new TreatyPricingDefinitionAndExclusionVersionBo();
                if (!definitionAndExclusionBo.DuplicateFromList && string.IsNullOrEmpty(definitionAndExclusionBo.Name))
                {
                    errors.Add("Name is required");
                }

                if (definitionAndExclusionBo.IsDuplicateExisting)
                {
                    if (!definitionAndExclusionBo.DuplicateTreatyPricingDefinitionAndExclusionId.HasValue)
                    {
                        errors.Add("Definition and Exclusion is required");
                    }

                    if (!definitionAndExclusionBo.DuplicateFromList && !definitionAndExclusionBo.DuplicateTreatyPricingDefinitionAndExclusionVersionId.HasValue)
                    {
                        errors.Add("Version is required");
                    }

                    if (errors.IsNullOrEmpty())
                    {
                        TreatyPricingDefinitionAndExclusionBo duplicate = TreatyPricingDefinitionAndExclusionService.Find(definitionAndExclusionBo.DuplicateTreatyPricingDefinitionAndExclusionId);
                        if (duplicate == null)
                        {
                            errors.Add("Definition and Exclusion not found");
                        }
                        else
                        {
                            definitionAndExclusionBo.BenefitCode = duplicate.BenefitCode;
                            definitionAndExclusionBo.AdditionalRemarks = duplicate.AdditionalRemarks;

                            TreatyPricingDefinitionAndExclusionVersionBo duplicateVersion = null;
                            if (definitionAndExclusionBo.DuplicateTreatyPricingDefinitionAndExclusionVersionId.HasValue)
                            {
                                duplicateVersion = duplicate.TreatyPricingDefinitionAndExclusionVersionBos.Where(q => q.Id == definitionAndExclusionBo.DuplicateTreatyPricingDefinitionAndExclusionVersionId.Value).FirstOrDefault();
                            }
                            else
                            {
                                duplicateVersion = duplicate.TreatyPricingDefinitionAndExclusionVersionBos.OrderByDescending(q => q.Version).First();
                            }

                            if (duplicateVersion == null)
                            {
                                errors.Add("Definition and Exclusion Version not found");
                            }
                            else
                            {
                                versionBo = new TreatyPricingDefinitionAndExclusionVersionBo(duplicateVersion)
                                {
                                    Id = 0
                                };
                            }
                        }

                    }
                }

                if (errors.IsNullOrEmpty())
                {
                    definitionAndExclusionBo.Status = TreatyPricingDefinitionAndExclusionBo.StatusActive;
                    definitionAndExclusionBo.Code = TreatyPricingDefinitionAndExclusionService.GetNextObjectId(definitionAndExclusionBo.TreatyPricingCedantId);
                    definitionAndExclusionBo.CreatedById = AuthUserId;
                    definitionAndExclusionBo.UpdatedById = AuthUserId;

                    TrailObject trail = GetNewTrailObject();

                    Result = TreatyPricingDefinitionAndExclusionService.Create(ref definitionAndExclusionBo, ref trail);
                    if (Result.Valid)
                    {
                        versionBo.TreatyPricingDefinitionAndExclusionId = definitionAndExclusionBo.Id;
                        versionBo.Version = 1;
                        versionBo.PersonInChargeId = AuthUserId;
                        versionBo.CreatedById = AuthUserId;
                        versionBo.UpdatedById = AuthUserId;

                        TreatyPricingDefinitionAndExclusionVersionService.Create(ref versionBo, ref trail);
                        CreateTrail(
                            definitionAndExclusionBo.Id,
                            "Create Treaty Pricing Definition And Exclusion"
                        );

                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(string.Format("Error occurred. Error details: {0}", ex.Message));
            }

            var definitionAndExclusionBos = TreatyPricingDefinitionAndExclusionService.GetByTreatyPricingCedantId(definitionAndExclusionBo.TreatyPricingCedantId);

            return Json(new { errors, definitionAndExclusionBos });
        }

        // GET: TreatyPricingDefinitionAndExclusion
        public ActionResult Edit(int id, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false, int workflowId = 0, bool isEditMode = false)
        {
            if (!CheckObjectLockReadOnly(Controller, id, isEditMode))
                return RedirectDashboard();

            var bo = TreatyPricingDefinitionAndExclusionService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var model = new TreatyPricingDefinitionAndExclusionViewModel(bo);
            model.WorkflowId = workflowId;
            LoadPage(model, versionId, isCalledFromWorkflow, isQuotationWorkflow);
            return View(model);
        }

        // POST: TreatyPricingDefinitionAndExclusion/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, TreatyPricingDefinitionAndExclusionViewModel model, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false, int workflowId = 0)
        {
            TreatyPricingDefinitionAndExclusionBo definitionAndExclusionBo = TreatyPricingDefinitionAndExclusionService.Find(id);
            if (definitionAndExclusionBo == null)
            {
                return RedirectToAction("Edit", "TreatyPricingCedant", new { id = model.TreatyPricingCedantId });
            }

            object routeValues = new { id = definitionAndExclusionBo.Id };
            if (isCalledFromWorkflow)
            {
                routeValues = new { id = definitionAndExclusionBo.Id, versionId, isCalledFromWorkflow, isQuotationWorkflow, workflowId };
            }

            if (!CheckObjectLock(Controller, id))
                return RedirectToAction("Edit", routeValues);

            TreatyPricingDefinitionAndExclusionVersionBo updatedVersionBo = null;
            definitionAndExclusionBo.SetVersionObjects(definitionAndExclusionBo.TreatyPricingDefinitionAndExclusionVersionBos);
            if (definitionAndExclusionBo.EditableVersion == model.CurrentVersion)
            {
                updatedVersionBo = model.GetVersionBo((TreatyPricingDefinitionAndExclusionVersionBo)definitionAndExclusionBo.CurrentVersionObject);
                definitionAndExclusionBo.AddVersionObject(updatedVersionBo);
            }

            if (ModelState.IsValid)
            {
                definitionAndExclusionBo = model.FormBo(definitionAndExclusionBo);
                definitionAndExclusionBo.UpdatedById = AuthUserId;

                TrailObject trail = GetNewTrailObject();
                Result = TreatyPricingDefinitionAndExclusionService.Update(ref definitionAndExclusionBo, ref trail);
                if (Result.Valid)
                {
                    if (updatedVersionBo != null)
                    {
                        updatedVersionBo.UpdatedById = AuthUserId;
                        TreatyPricingDefinitionAndExclusionVersionService.Update(ref updatedVersionBo, ref trail);
                    }

                    CreateTrail(
                        definitionAndExclusionBo.Id,
                        "Update Definition And Exclusion"
                    );

                    SetUpdateSuccessMessage("Definition And Exclusion", false);

                    ObjectLockController.DeleteObjectLock(Controller, id, AuthUserId);

                    return RedirectToAction("Edit", routeValues);
                }
                AddResult(Result);
            }

            model.CurrentVersion = definitionAndExclusionBo.CurrentVersion;
            model.CurrentVersionObject = definitionAndExclusionBo.CurrentVersionObject;
            model.VersionObjects = definitionAndExclusionBo.VersionObjects;
            model.TreatyPricingCedantBo = definitionAndExclusionBo.TreatyPricingCedantBo;
            model.WorkflowId = workflowId;
            LoadPage(model, versionId, isCalledFromWorkflow, isQuotationWorkflow);
            return View(model);
        }
        public void LoadPage(TreatyPricingDefinitionAndExclusionViewModel model, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false)
        {
            AuthUserName();
            ViewBag.PersonInChargeId = AuthUserId;
            ViewBag.PersonInChargeName = UserService.Find(AuthUserId).FullName;

            DropDownVersion(model);
            DropDownUser(UserBo.StatusActive);
            GetBenefitCodes();

            var entity = new TreatyPricingDefinitionAndExclusion();
            var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Description");
            ViewBag.DescriptionMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

            // Workflow 
            ViewBag.PersonInChargeId = AuthUserId;
            ViewBag.PersonInChargeName = AuthUser.FullName;
            ViewBag.IsCalledFromWorkflow = isCalledFromWorkflow;
            ViewBag.IsHideSideBar = isCalledFromWorkflow;
            ViewBag.IsQuotationWorkflow = isQuotationWorkflow;
            ViewBag.VersionId = versionId;

            if (versionId > 0)
            {
                model.SetCurrentVersionObject(versionId);
            }

            // Remark & Document
            string downloadDocumentUrl = Url.Action("Download", "Document");
            GetRemarkDocument(model.ModuleId, model.TreatyPricingCedantId, downloadDocumentUrl, ModuleBo.ModuleController.TreatyPricingDefinitionAndExclusion.ToString(), model.Id);
            GetRemarkSubjects();

            if (model.Id == 0)
            {
                // Create
            }
            else
            {
                //Edit
                if (!string.IsNullOrEmpty(model.BenefitCode))
                {
                    string[] benefitCodes = model.BenefitCode.ToArraySplitTrim();
                    foreach (string benefitCodeStr in benefitCodes)
                    {
                        var benefitCode = BenefitService.FindByCode(benefitCodeStr);
                        if (benefitCode != null)
                        {
                            if (benefitCode.Status == BenefitBo.StatusInactive)
                            {
                                AddErrorMsg(string.Format(MessageBag.BenefitStatusInactiveWithCode, benefitCodeStr));
                            }
                        }
                        else
                        {
                            AddErrorMsg(string.Format(MessageBag.BenefitCodeNotFound, benefitCodeStr));
                        }
                    }
                }

                GetProduct(model.Id);
            }

            GetObjectVersionChangelog(ModuleBo.ModuleController.TreatyPricingDefinitionAndExclusion.ToString(), model.Id, model);
            DropDownWorkflowObjectTypes();
            DropDownWorkflowStatus();

            SetViewBagMessage();
        }

        public void GetProduct(int id)
        {
            var productBos = TreatyPricingProductService.GetDefinitionAndExclusionProduct(id);
            ViewBag.Products = productBos;
        }

        public ActionResult CreateVersion(TreatyPricingDefinitionAndExclusionBo bo, bool duplicatePreviousVersion = false)
        {
            bo.SetVersionObjects(TreatyPricingDefinitionAndExclusionVersionService.GetByTreatyPricingDefinitionAndExclusionId(bo.Id));
            TreatyPricingDefinitionAndExclusionVersionBo nextVersionBo;
            TreatyPricingDefinitionAndExclusionVersionBo previousVersionBo = (TreatyPricingDefinitionAndExclusionVersionBo)bo.CurrentVersionObject;
            List<string> errors = new List<string>();
            errors.AddRange(ValidateVersion(previousVersionBo));
            if (!errors.IsNullOrEmpty())
            {
                return Json(new { errors });
            }

            if (duplicatePreviousVersion)
            {
                nextVersionBo = new TreatyPricingDefinitionAndExclusionVersionBo(previousVersionBo)
                {
                    Id = 0,
                };
            }
            else
            {
                nextVersionBo = new TreatyPricingDefinitionAndExclusionVersionBo()
                {
                    TreatyPricingDefinitionAndExclusionId = bo.Id,
                };
            }

            nextVersionBo.Version = bo.EditableVersion + 1;
            nextVersionBo.PersonInChargeId = AuthUserId;
            nextVersionBo.CreatedById = AuthUserId;
            nextVersionBo.UpdatedById = AuthUserId;

            ObjectVersionChangelog changelog = null;
            TrailObject trail = GetNewTrailObject();
            Result = TreatyPricingDefinitionAndExclusionVersionService.Create(ref nextVersionBo, ref trail);
            if (Result.Valid)
            {
                UserTrailBo userTrail = CreateTrail(
                     nextVersionBo.Id,
                     "Create Treaty Pricing Definition And Exclusion Version"
                 );

                userTrail.CreatedByBo = UserService.Find(userTrail.CreatedById);
                userTrail.CreatedAtStr = userTrail.CreatedAt.ToString(Util.GetDateTimeFormat());

                changelog = new ObjectVersionChangelog(nextVersionBo, userTrail);
                changelog.FormatBetweenVersionTrail(previousVersionBo);
            }

            bo.AddVersionObject(nextVersionBo);
            var versions = DropDownVersion(bo);

            return Json(new { bo, versions, changelog });
        }

        [HttpPost]
        public JsonResult Search(int cedantId, string productName)
        {
            var DefinitionAndExclusion = TreatyPricingDefinitionAndExclusionService.GetCodeByTreatyPricingCedantIdProductName(cedantId, productName);
            return Json(new { DefinitionAndExclusion = DefinitionAndExclusion });
        }

        public static List<string> ValidateVersion(TreatyPricingDefinitionAndExclusionVersionBo versionBo)
        {
            List<string> errors = new List<string>();
            TreatyPricingDefinitionAndExclusionViewModel model = new TreatyPricingDefinitionAndExclusionViewModel();

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
        public JsonResult GetByTreatyPricingCedant(int? id)
        {
            IList<TreatyPricingDefinitionAndExclusionBo> bos = new List<TreatyPricingDefinitionAndExclusionBo>();
            if (id.HasValue)
            {
                bos = TreatyPricingDefinitionAndExclusionService.GetByTreatyPricingCedantId(id.Value);
            }
            return Json(new { bos });
        }

        [HttpPost]
        public JsonResult GetByTreatyPricingDefinitionAndExclusion(int? id)
        {
            IList<TreatyPricingDefinitionAndExclusionVersionBo> versionBos = new List<TreatyPricingDefinitionAndExclusionVersionBo>();
            if (id.HasValue)
            {
                versionBos = TreatyPricingDefinitionAndExclusionVersionService.GetByTreatyPricingDefinitionAndExclusionId(id.Value).ToList();
            }
            return Json(new { versionBos });
        }
    }
}