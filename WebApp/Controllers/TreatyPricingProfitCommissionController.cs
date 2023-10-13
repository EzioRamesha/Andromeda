using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using Services.Identity;
using Services.TreatyPricing;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class TreatyPricingProfitCommissionController : BaseController
    {
        public const string Controller = "TreatyPricingProfitCommission";

        [HttpPost]
        public JsonResult GetByTreatyPricingCedant(int? id)
        {
            IList<TreatyPricingProfitCommissionBo> bos = new List<TreatyPricingProfitCommissionBo>();
            if (id.HasValue)
            {
                bos = TreatyPricingProfitCommissionService.GetByTreatyPricingCedantId(id.Value);
            }
            return Json(new { bos });
        }

        [HttpPost]
        public JsonResult GetByTreatyPricingProfitCommission(int? id)
        {
            IList<TreatyPricingProfitCommissionVersionBo> versionBos = new List<TreatyPricingProfitCommissionVersionBo>();
            if (id.HasValue)
            {
                versionBos = TreatyPricingProfitCommissionVersionService.GetByTreatyPricingProfitCommissionId(id.Value).ToList();
            }
            return Json(new { versionBos });
        }

        //[Auth(Controller = Controller, Power = "C")]
        [HttpPost]
        public JsonResult Add(TreatyPricingProfitCommissionBo profitCommissionBo)
        {
            List<string> errors = new List<string>();

            if (!CheckPower(Controller, "C"))
            {
                errors.Add(string.Format(MessageBag.AccessDeniedWithActionName, "Add", "Profit Commission"));
                return Json(new { errors, profitCommissionBo });
            }

            string profitCommissionDetail = null;
            string tierProfitCommission = null;

            try
            {
                TreatyPricingProfitCommissionVersionBo versionBo = new TreatyPricingProfitCommissionVersionBo();
                //if (!profitCommissionBo.DuplicateFromList && string.IsNullOrEmpty(profitCommissionBo.Name))
                //{
                //    errors.Add("Name is required");
                //}

                if (profitCommissionBo.IsDuplicateExisting)
                {
                    if (!profitCommissionBo.DuplicateTreatyPricingProfitCommissionId.HasValue)
                    {
                        errors.Add("Profit Commission is required");
                    }

                    if (!profitCommissionBo.DuplicateFromList && !profitCommissionBo.DuplicateTreatyPricingProfitCommissionVersionId.HasValue)
                    {
                        errors.Add("Version is required");
                    }

                    if (errors.IsNullOrEmpty())
                    {
                        TreatyPricingProfitCommissionBo duplicate = TreatyPricingProfitCommissionService.Find(profitCommissionBo.DuplicateTreatyPricingProfitCommissionId);
                        if (duplicate == null)
                        {
                            errors.Add("Profit Commission not found");
                        }
                        else
                        {
                            profitCommissionBo.BenefitCode = duplicate.BenefitCode;
                            profitCommissionBo.EffectiveDate = duplicate.EffectiveDate;
                            profitCommissionBo.StartDate = duplicate.StartDate;
                            profitCommissionBo.EndDate = duplicate.EndDate;
                            profitCommissionBo.Entitlement = duplicate.Entitlement;
                            profitCommissionBo.Remark = duplicate.Remark;

                            TreatyPricingProfitCommissionVersionBo duplicateVersion = null;
                            if (profitCommissionBo.DuplicateTreatyPricingProfitCommissionVersionId.HasValue)
                            {
                                duplicateVersion = duplicate.TreatyPricingProfitCommissionVersionBos.Where(q => q.Id == profitCommissionBo.DuplicateTreatyPricingProfitCommissionVersionId.Value).FirstOrDefault();
                            }
                            else
                            {
                                duplicateVersion = duplicate.TreatyPricingProfitCommissionVersionBos.OrderByDescending(q => q.Version).First();
                            }

                            if (duplicateVersion == null)
                            {
                                errors.Add("Profit Commission Version not found");
                            }
                            else
                            {
                                versionBo = new TreatyPricingProfitCommissionVersionBo(duplicateVersion)
                                {
                                    Id = 0
                                };
                                profitCommissionDetail = TreatyPricingProfitCommissionDetailService.GetJsonByParent(duplicateVersion.Id);
                                tierProfitCommission = TreatyPricingTierProfitCommissionService.GetJsonByParent(duplicateVersion.Id);
                            }
                        }
                    }
                }

                if (errors.IsNullOrEmpty())
                {
                    profitCommissionBo.Status = TreatyPricingProfitCommissionBo.StatusActive;
                    profitCommissionBo.Code = TreatyPricingProfitCommissionService.GetNextObjectId(profitCommissionBo.TreatyPricingCedantId);
                    profitCommissionBo.CreatedById = AuthUserId;
                    profitCommissionBo.UpdatedById = AuthUserId;

                    TrailObject trail = GetNewTrailObject();

                    Result = TreatyPricingProfitCommissionService.Create(ref profitCommissionBo, ref trail);
                    if (Result.Valid)
                    {
                        versionBo.TreatyPricingProfitCommissionId = profitCommissionBo.Id;
                        versionBo.Version = 1;
                        versionBo.PersonInChargeId = AuthUserId;
                        versionBo.CreatedById = AuthUserId;
                        versionBo.UpdatedById = AuthUserId;
                        TreatyPricingProfitCommissionVersionService.Create(ref versionBo, ref trail);

                        if (string.IsNullOrEmpty(profitCommissionDetail))
                            profitCommissionDetail = TreatyPricingProfitCommissionDetailBo.GetJsonDefaultRow(versionBo.Id);
                        TreatyPricingProfitCommissionDetailController.Save(profitCommissionDetail, versionBo.Id, AuthUserId, ref trail, true);

                        if (!string.IsNullOrEmpty(tierProfitCommission))
                            TreatyPricingTierProfitCommissionController.Save(tierProfitCommission, versionBo.Id, versionBo.ProfitSharing, AuthUserId, ref trail, true);

                        CreateTrail(
                            profitCommissionBo.Id,
                            "Create Treaty Pricing Profit Commission"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(string.Format("Error occurred. Error details: {0}", ex.Message));
            }

            var profitCommissionBos = TreatyPricingProfitCommissionService.GetByTreatyPricingCedantId(profitCommissionBo.TreatyPricingCedantId);

            return Json(new { errors, profitCommissionBos });
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false, int workflowId = 0, bool isEditMode = false)
        {
            if (!CheckObjectLockReadOnly(Controller, id, isEditMode))
                return RedirectDashboard();

            var bo = TreatyPricingProfitCommissionService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var model = new TreatyPricingProfitCommissionViewModel(bo)
            {
                WorkflowId = workflowId
            };
            LoadPage(model, versionId, isCalledFromWorkflow, isQuotationWorkflow);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, TreatyPricingProfitCommissionViewModel model, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false, int workflowId = 0)
        {
            TreatyPricingProfitCommissionBo profitCommissionBo = TreatyPricingProfitCommissionService.Find(id);
            if (profitCommissionBo == null)
            {
                return RedirectToAction("Edit", "TreatyPricingCedant", new { id = model.TreatyPricingCedantId });
            }

            object routeValues = new { id = profitCommissionBo.Id };
            if (isCalledFromWorkflow)
            {
                routeValues = new { id = profitCommissionBo.Id, versionId, isCalledFromWorkflow, isQuotationWorkflow, workflowId };
            }

            if (!CheckObjectLock(Controller, id))
                return RedirectToAction("Edit", routeValues);

            TreatyPricingProfitCommissionVersionBo updatedVersionBo = null;
            profitCommissionBo.SetVersionObjects(profitCommissionBo.TreatyPricingProfitCommissionVersionBos);
            if (profitCommissionBo.EditableVersion == model.CurrentVersion)
            {
                updatedVersionBo = model.GetVersionBo((TreatyPricingProfitCommissionVersionBo)profitCommissionBo.CurrentVersionObject);
                profitCommissionBo.AddVersionObject(updatedVersionBo);
            }

            if (ModelState.IsValid)
            {
                profitCommissionBo = model.FormBo(profitCommissionBo);
                profitCommissionBo.UpdatedById = AuthUserId;

                TrailObject trail = GetNewTrailObject();
                Result = TreatyPricingProfitCommissionService.Update(ref profitCommissionBo, ref trail);
                if (Result.Valid)
                {
                    if (updatedVersionBo != null)
                    {
                        updatedVersionBo.UpdatedById = AuthUserId;
                        TreatyPricingProfitCommissionVersionService.Update(ref updatedVersionBo, ref trail);
                        TreatyPricingProfitCommissionDetailController.Save(updatedVersionBo.ProfitCommissionDetail, updatedVersionBo.Id, AuthUserId, ref trail);
                        TreatyPricingTierProfitCommissionController.Save(updatedVersionBo.TierProfitCommission, updatedVersionBo.Id, updatedVersionBo.ProfitSharing, AuthUserId, ref trail);
                    }

                    CreateTrail(
                        profitCommissionBo.Id,
                        "Update Treaty Pricing Profit Commission"
                    );

                    SetUpdateSuccessMessage("Profit Commission", false);

                    ObjectLockController.DeleteObjectLock(Controller, id, AuthUserId);

                    return RedirectToAction("Edit", routeValues);
                }
                AddResult(Result);
            }

            model.CurrentVersion = profitCommissionBo.CurrentVersion;
            model.CurrentVersionObject = profitCommissionBo.CurrentVersionObject;
            model.VersionObjects = profitCommissionBo.VersionObjects;
            model.TreatyPricingCedantBo = profitCommissionBo.TreatyPricingCedantBo;
            model.WorkflowId = workflowId;
            LoadPage(model, versionId, isCalledFromWorkflow, isQuotationWorkflow);
            return View(model);
        }

        public void LoadPage(TreatyPricingProfitCommissionViewModel model, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false)
        {
            DropDownVersion(model);
            DropDownUser(UserBo.StatusActive, model.PersonInChargeId);
            GetBenefitCodes();
            GetEntitlements();

            DropDownStatus();
            DropDownProfitSharing();
            GetItemList();
            DropDownDropDown();

            AuthUserName();

            ViewBag.IsCalledFromWorkflow = isCalledFromWorkflow;
            ViewBag.IsHideSideBar = isCalledFromWorkflow;
            ViewBag.IsQuotationWorkflow = isQuotationWorkflow;
            ViewBag.VersionId = versionId;

            // Remark & Document
            string downloadDocumentUrl = Url.Action("Download", "Document");
            GetRemarkDocument(model.ModuleId, model.TreatyPricingCedantId, downloadDocumentUrl, ModuleBo.ModuleController.TreatyPricingProfitCommission.ToString(), model.Id);
            GetRemarkSubjects();

            GetObjectVersionChangelog(ModuleBo.ModuleController.TreatyPricingProfitCommission.ToString(), model.Id, model);
            DropDownWorkflowObjectTypes();
            DropDownWorkflowStatus();

            var entity = new TreatyPricingProfitCommission();
            var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Name");
            ViewBag.NameMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

            maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Description");
            ViewBag.DescriptionMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

            var entity2 = new TreatyPricingProfitCommissionVersion();
            var maxLengthAttr2 = entity2.GetAttributeFrom<MaxLengthAttribute>("Description");
            ViewBag.ProfitDescriptionMaxLength = maxLengthAttr2 != null ? maxLengthAttr2.Length : 255;

            if (model == null)
            {

            }
            else
            {
                GetProduct(model.Id);
            }

            if (versionId > 0)
            {
                model.SetCurrentVersionObject(versionId);
            }

            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= TreatyPricingProfitCommissionBo.StatusMax; i++)
            {
                items.Add(new SelectListItem { Text = TreatyPricingProfitCommissionBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownStatuses = items;
            return items;
        }

        public List<SelectListItem> DropDownProfitSharing()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= TreatyPricingProfitCommissionVersionBo.ProfitSharingMax; i++)
            {
                items.Add(new SelectListItem { Text = TreatyPricingProfitCommissionVersionBo.GetProfitSharingName(i), Value = i.ToString() });
            }
            ViewBag.DropDownProfitSharings = items;
            return items;
        }

        public void GetItemList()
        {
            var list = new List<string>();
            for (int i = 0; i <= TreatyPricingProfitCommissionDetailBo.ItemMax; i++)
            {
                list.Add(TreatyPricingProfitCommissionDetailBo.GetItemName(i));
            }
            ViewBag.ItemList = list;
        }

        public List<SelectListItem> DropDownDropDown()
        {
            var items = GetEmptyDropDownList(false);
            for (int i = 1; i <= TreatyPricingProfitCommissionDetailBo.DropDownMax; i++)
            {
                items.Add(new SelectListItem { Text = TreatyPricingProfitCommissionDetailBo.GetDropDownName(i), Value = i.ToString() });
            }
            ViewBag.DropDownDropDowns = items;
            return items;
        }

        public void GetProduct(int id)
        {
            //var productBos = TreatyPricingProductService.GetRateTableProduct(id);
            var productBos = TreatyPricingProductService.GetProfitCommissionProduct(id);
            ViewBag.Products = productBos;
        }

        public ActionResult CreateVersion(TreatyPricingProfitCommissionBo bo, bool duplicatePreviousVersion = false)
        {
            bo.SetVersionObjects(TreatyPricingProfitCommissionVersionService.GetByTreatyPricingProfitCommissionId(bo.Id));
            TreatyPricingProfitCommissionVersionBo nextVersionBo;
            TreatyPricingProfitCommissionVersionBo previousVersionBo = (TreatyPricingProfitCommissionVersionBo)bo.CurrentVersionObject;
            if (duplicatePreviousVersion)
            {
                nextVersionBo = new TreatyPricingProfitCommissionVersionBo(previousVersionBo)
                {
                    Id = 0,
                };
            }
            else
            {
                nextVersionBo = new TreatyPricingProfitCommissionVersionBo()
                {
                    TreatyPricingProfitCommissionId = bo.Id,
                };
            }

            nextVersionBo.Version = bo.EditableVersion + 1;
            nextVersionBo.PersonInChargeId = AuthUserId;
            nextVersionBo.CreatedById = AuthUserId;
            nextVersionBo.UpdatedById = AuthUserId;

            ObjectVersionChangelog changelog = null;
            TrailObject trail = GetNewTrailObject();
            Result = TreatyPricingProfitCommissionVersionService.Create(ref nextVersionBo, ref trail);
            if (Result.Valid)
            {
                if (!duplicatePreviousVersion)
                {
                    nextVersionBo.ProfitCommissionDetail = TreatyPricingProfitCommissionDetailBo.GetJsonDefaultRow(nextVersionBo.Id);
                }
                TreatyPricingProfitCommissionDetailController.Save(nextVersionBo.ProfitCommissionDetail, nextVersionBo.Id, AuthUserId, ref trail, true);
                TreatyPricingTierProfitCommissionController.Save(nextVersionBo.TierProfitCommission, nextVersionBo.Id, nextVersionBo.ProfitSharing, AuthUserId, ref trail, true);
                nextVersionBo.ProfitCommissionDetail = TreatyPricingProfitCommissionDetailService.GetJsonByParent(nextVersionBo.Id);
                nextVersionBo.TierProfitCommission = TreatyPricingTierProfitCommissionService.GetJsonByParent(nextVersionBo.Id);

                UserTrailBo userTrail = CreateTrail(
                    nextVersionBo.Id,
                    "Create Treaty Pricing Profit Commission Version"
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
            var profitCommission = TreatyPricingProfitCommissionService.GetCodeByTreatyPricingCedantIdProductName(cedantId, productName);
            return Json(new { ProfitCommission = profitCommission });
        }
    }
}