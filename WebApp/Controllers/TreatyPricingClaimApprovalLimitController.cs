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
    public class TreatyPricingClaimApprovalLimitController : BaseController
    {
        public const string Controller = "TreatyPricingClaimApprovalLimit";

        //[Auth(Controller = Controller, Power = "C")]
        [HttpPost]
        public JsonResult Add(TreatyPricingClaimApprovalLimitBo claimApprovalLimitBo)
        {
            List<string> errors = new List<string>();

            if (!CheckPower(Controller, "C"))
            {
                errors.Add(string.Format(MessageBag.AccessDeniedWithActionName, "Add", "Claim Approval Limit"));
                return Json(new { errors });
            }

            try
            {
                TreatyPricingClaimApprovalLimitVersionBo versionBo = new TreatyPricingClaimApprovalLimitVersionBo();
                if (!claimApprovalLimitBo.DuplicateFromList && string.IsNullOrEmpty(claimApprovalLimitBo.Name))
                {
                    errors.Add("Name is required");
                }

                if (claimApprovalLimitBo.IsDuplicateExisting)
                {
                    if (!claimApprovalLimitBo.DuplicateTreatyPricingClaimApprovalLimitId.HasValue)
                    {
                        errors.Add("Claim Approval Limit is required");
                    }

                    
                    if (!claimApprovalLimitBo.DuplicateFromList && !claimApprovalLimitBo.DuplicateTreatyPricingClaimApprovalLimitVersionId.HasValue)
                    {
                        errors.Add("Version is required");
                    }

                    if (errors.IsNullOrEmpty())
                    {
                        TreatyPricingClaimApprovalLimitBo duplicate = TreatyPricingClaimApprovalLimitService.Find(claimApprovalLimitBo.DuplicateTreatyPricingClaimApprovalLimitId);
                        if (duplicate == null)
                        {
                            errors.Add("Claim Approval Limit not found");
                        }
                        else
                        {
                            claimApprovalLimitBo.BenefitCode = duplicate.BenefitCode;
                            claimApprovalLimitBo.Remarks = duplicate.Remarks;

                            TreatyPricingClaimApprovalLimitVersionBo duplicateVersion = null;
                            if (claimApprovalLimitBo.DuplicateTreatyPricingClaimApprovalLimitVersionId.HasValue)
                            {
                                duplicateVersion = duplicate.TreatyPricingClaimApprovalLimitVersionBos.Where(q => q.Id == claimApprovalLimitBo.DuplicateTreatyPricingClaimApprovalLimitVersionId.Value).FirstOrDefault();
                            }
                            else
                            {
                                duplicateVersion = duplicate.TreatyPricingClaimApprovalLimitVersionBos.OrderByDescending(q => q.Version).First();
                            }

                            if (duplicateVersion == null)
                            {
                                errors.Add("Claim Approval Limit Version not found");
                            }
                            else
                            {
                                versionBo = new TreatyPricingClaimApprovalLimitVersionBo(duplicateVersion)
                                {
                                    Id = 0
                                };
                            }
                        }
                    }
                }

                if (errors.IsNullOrEmpty())
                {
                    claimApprovalLimitBo.Status = TreatyPricingClaimApprovalLimitBo.StatusActive;
                    claimApprovalLimitBo.Code = TreatyPricingClaimApprovalLimitService.GetNextObjectId(claimApprovalLimitBo.TreatyPricingCedantId);
                    claimApprovalLimitBo.CreatedById = AuthUserId;
                    claimApprovalLimitBo.UpdatedById = AuthUserId;

                    TrailObject trail = GetNewTrailObject();

                    Result = TreatyPricingClaimApprovalLimitService.Create(ref claimApprovalLimitBo, ref trail);
                    if (Result.Valid)
                    {
                        versionBo.TreatyPricingClaimApprovalLimitId = claimApprovalLimitBo.Id;
                        versionBo.Version = 1;
                        versionBo.PersonInChargeId = AuthUserId;
                        versionBo.CreatedById = AuthUserId;
                        versionBo.UpdatedById = AuthUserId;
                        TreatyPricingClaimApprovalLimitVersionService.Create(ref versionBo, ref trail);

                        CreateTrail(
                            claimApprovalLimitBo.Id,
                            "Create Treaty Pricing Claim Approval Limit"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(string.Format("Error occurred. Error details: {0}", ex.Message));
            }

            var claimApprovalLimitBos = TreatyPricingClaimApprovalLimitService.GetByTreatyPricingCedantId(claimApprovalLimitBo.TreatyPricingCedantId);

            return Json(new { errors, claimApprovalLimitBos });
        }

        // GET: TreatyPricingClaimApprovalLimit
        public ActionResult Edit(int id, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false, int workflowId = 0, bool isEditMode = false)
        {
            if (!CheckObjectLockReadOnly(Controller, id, isEditMode))
                return RedirectDashboard();

            var bo = TreatyPricingClaimApprovalLimitService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var model = new TreatyPricingClaimApprovalLimitViewModel(bo);
            model.WorkflowId = workflowId;
            LoadPage(model, versionId, isCalledFromWorkflow, isQuotationWorkflow);
            return View(model);
        }

        // POST: TreatyPricingClaimApprovalLimit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, TreatyPricingClaimApprovalLimitViewModel model, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false, int workflowId = 0)
        {
            var dbBo = TreatyPricingClaimApprovalLimitService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Edit", "TreatyPricingCedant", new { id = model.TreatyPricingCedantId });
            }

            object routeValues = new { id = dbBo.Id };
            if (isCalledFromWorkflow)
            {
                routeValues = new { id = dbBo.Id, versionId, isCalledFromWorkflow, isQuotationWorkflow, workflowId };
            }

            if (!CheckObjectLock(Controller, id))
                return RedirectToAction("Edit", routeValues);

            TreatyPricingClaimApprovalLimitVersionBo updatedVersionBo = null;
            dbBo.SetVersionObjects(dbBo.TreatyPricingClaimApprovalLimitVersionBos);
            if (dbBo.EditableVersion == model.CurrentVersion)
            {
                updatedVersionBo = model.GetVersionBo((TreatyPricingClaimApprovalLimitVersionBo)dbBo.CurrentVersionObject);
                dbBo.AddVersionObject(updatedVersionBo);
            }

            if (ModelState.IsValid)
            {
                dbBo = model.FormBo(dbBo);
                dbBo.UpdatedById = AuthUserId;

                var trail = GetNewTrailObject();

                Result = TreatyPricingClaimApprovalLimitService.Update(ref dbBo, ref trail);
                if (Result.Valid)
                {
                    if (updatedVersionBo != null)
                    {
                        updatedVersionBo.UpdatedById = AuthUserId;
                        TreatyPricingClaimApprovalLimitVersionService.Update(ref updatedVersionBo, ref trail);
                    }

                    CreateTrail(
                        dbBo.Id,
                        "Update Treaty Pricing Claim Approval Limit"
                    );

                    SetUpdateSuccessMessage("Claim Approval Limit", false);

                    ObjectLockController.DeleteObjectLock(Controller, id, AuthUserId);

                    return RedirectToAction("Edit", routeValues);
                }
                AddResult(Result);
            }
            else
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            }


            model.CurrentVersion = dbBo.CurrentVersion;
            model.CurrentVersionObject = dbBo.CurrentVersionObject;
            model.VersionObjects = dbBo.VersionObjects;
            model.TreatyPricingCedantBo = dbBo.TreatyPricingCedantBo;
            model.WorkflowId = workflowId;
            LoadPage(model, versionId, isCalledFromWorkflow, isQuotationWorkflow);
            return View(model);
        }

        public void LoadPage(TreatyPricingClaimApprovalLimitViewModel model, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false)
        {
            AuthUserName();
            ViewBag.PersonInChargeId = AuthUserId;
            ViewBag.PersonInChargeName = UserService.Find(AuthUserId).FullName;

            DropDownVersion(model);
            DropDownUser(UserBo.StatusActive);
            GetBenefitCodes();

            var entity = new TreatyPricingClaimApprovalLimit();
            var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Description");
            ViewBag.DescriptionMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

            var versionEntity = new TreatyPricingClaimApprovalLimitVersion();
            maxLengthAttr = versionEntity.GetAttributeFrom<MaxLengthAttribute>("Amount");
            ViewBag.AmountMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

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
            GetRemarkDocument(model.ModuleId, model.TreatyPricingCedantId, downloadDocumentUrl, ModuleBo.ModuleController.TreatyPricingClaimApprovalLimit.ToString(), model.Id);
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

            GetObjectVersionChangelog(ModuleBo.ModuleController.TreatyPricingClaimApprovalLimit.ToString(), model.Id, model);
            DropDownWorkflowObjectTypes();
            DropDownWorkflowStatus();

            SetViewBagMessage();
        }

        public void GetProduct(int id)
        {
            var productBos = TreatyPricingProductService.GetClaimApprovalLimitProduct(id);
            ViewBag.Products = productBos;
        }

        public ActionResult CreateVersion(TreatyPricingClaimApprovalLimitBo bo, bool duplicatePreviousVersion = false)
        {
            bo.SetVersionObjects(TreatyPricingClaimApprovalLimitVersionService.GetByTreatyPricingClaimApprovalLimitId(bo.Id));
            TreatyPricingClaimApprovalLimitVersionBo nextVersionBo;
            TreatyPricingClaimApprovalLimitVersionBo previousVersionBo = (TreatyPricingClaimApprovalLimitVersionBo)bo.CurrentVersionObject;

            List<string> errors = new List<string>();
            errors.AddRange(ValidateVersion(previousVersionBo));
            if (!errors.IsNullOrEmpty())
            {
                return Json(new { errors });
            }

            if (duplicatePreviousVersion)
            {
                nextVersionBo = new TreatyPricingClaimApprovalLimitVersionBo(previousVersionBo)
                {
                    Id = 0,
                };
            }
            else
            {
                nextVersionBo = new TreatyPricingClaimApprovalLimitVersionBo()
                {
                    TreatyPricingClaimApprovalLimitId = bo.Id,
                };
            }

            nextVersionBo.Version = bo.EditableVersion + 1;
            nextVersionBo.PersonInChargeId = AuthUserId;
            nextVersionBo.CreatedById = AuthUserId;
            nextVersionBo.UpdatedById = AuthUserId;

            ObjectVersionChangelog changelog = null;

            TrailObject trail = GetNewTrailObject();
            Result = TreatyPricingClaimApprovalLimitVersionService.Create(ref nextVersionBo, ref trail);
            if (Result.Valid)
            {
                UserTrailBo userTrail = CreateTrail(
                    nextVersionBo.Id,
                    "Create Treaty Pricing Claim Approval Limit Version"
                );

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
            var claimApprovalLimit = TreatyPricingClaimApprovalLimitService.GetCodeByTreatyPricingCedantIdProductName(cedantId, productName);
            return Json(new { ClaimApprovalLimit = claimApprovalLimit });
        }

        public static List<string> ValidateVersion(TreatyPricingClaimApprovalLimitVersionBo versionBo)
        {
            List<string> errors = new List<string>();
            TreatyPricingClaimApprovalLimitViewModel model = new TreatyPricingClaimApprovalLimitViewModel();

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
            IList<TreatyPricingClaimApprovalLimitBo> bos = new List<TreatyPricingClaimApprovalLimitBo>();
            if (id.HasValue)
            {
                bos = TreatyPricingClaimApprovalLimitService.GetByTreatyPricingCedantId(id.Value);
            }
            return Json(new { bos });
        }

        [HttpPost]
        public JsonResult GetByTreatyPricingClaimApprovalLimit(int? id)
        {
            IList<TreatyPricingClaimApprovalLimitVersionBo> versionBos = new List<TreatyPricingClaimApprovalLimitVersionBo>();
            if (id.HasValue)
            {
                versionBos = TreatyPricingClaimApprovalLimitVersionService.GetByTreatyPricingClaimApprovalLimitId(id.Value).ToList();
            }
            return Json(new { versionBos });
        }
    }


}