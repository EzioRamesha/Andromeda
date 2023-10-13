using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using Services;
using Services.Identity;
using Services.TreatyPricing;
using Shared;
using Shared.Forms.Attributes;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class TreatyPricingProductController : BaseController
    {
        public const string Controller = "TreatyPricingProduct";

        [HttpPost]
        public JsonResult GetByTreatyPricingCedant(int? id)
        {
            IList<TreatyPricingProductBo> bos = new List<TreatyPricingProductBo>();
            if (id.HasValue)
            {
                bos = TreatyPricingProductService.GetByTreatyPricingCedantId(id.Value);
            }
            return Json(new { bos });
        }

        [HttpPost]
        public JsonResult GetWorkflowObjects(int? id)
        {
            IList<TreatyPricingWorkflowObjectBo> bos = new List<TreatyPricingWorkflowObjectBo>();
            if (id.HasValue)
            {
                bos = TreatyPricingWorkflowObjectService.GetByObjectTypeObjectId(TreatyPricingWorkflowObjectBo.ObjectTypeProduct, id.Value, true);
            }
            return Json(new { bos });
        }

        [HttpPost]
        public JsonResult GetByTreatyPricingProduct(int? id)
        {
            IList<TreatyPricingProductVersionBo> versionBos = new List<TreatyPricingProductVersionBo>();
            if (id.HasValue)
            {
                versionBos = TreatyPricingProductVersionService.GetByTreatyPricingProductId(id.Value).ToList();
            }
            return Json(new { versionBos });
        }

        //[Auth(Controller = Controller, Power = "C")]
        [HttpPost]
        public JsonResult Add(TreatyPricingProductBo productBo)
        {
            List<string> errors = new List<string>();

            if (!CheckPower(Controller, "C"))
            {
                errors.Add(string.Format(MessageBag.AccessDeniedWithActionName, "Add", "Product"));
                return Json(new { errors });
            }

            try
            {
                TreatyPricingProductVersionBo versionBo = new TreatyPricingProductVersionBo();
                if (!productBo.DuplicateFromList && string.IsNullOrEmpty(productBo.QuotationName))
                {
                    errors.Add("Quotation Name is required");
                }

                if (productBo.IsDuplicateExisting)
                {
                    if (!productBo.DuplicateTreatyPricingProductId.HasValue)
                    {
                        errors.Add("Product is required");
                    }

                    if (!productBo.DuplicateFromList && !productBo.DuplicateTreatyPricingProductVersionId.HasValue)
                    {
                        errors.Add("Version is required");
                    }

                    if (errors.IsNullOrEmpty())
                    {
                        TreatyPricingProductBo duplicate = TreatyPricingProductService.Find(productBo.DuplicateTreatyPricingProductId);
                        if (duplicate == null)
                        {
                            errors.Add("Product not found");
                        }
                        else
                        {
                            productBo.EffectiveDate = duplicate.EffectiveDate;
                            productBo.Summary = duplicate.Summary;
                            productBo.HasPerLifeRetro = duplicate.HasPerLifeRetro;
                            productBo.UnderwritingMethod = duplicate.UnderwritingMethod;

                            TreatyPricingProductVersionBo duplicateVersion = null;
                            if (productBo.DuplicateTreatyPricingProductVersionId.HasValue)
                            {
                                duplicateVersion = duplicate.TreatyPricingProductVersionBos.Where(q => q.Id == productBo.DuplicateTreatyPricingProductVersionId.Value).FirstOrDefault();
                            }
                            else
                            {
                                duplicateVersion = duplicate.TreatyPricingProductVersionBos.OrderByDescending(q => q.Version).First();
                            }

                            if (duplicateVersion == null)
                            {
                                errors.Add("Product Version not found");
                            }
                            else
                            {
                                versionBo = new TreatyPricingProductVersionBo(duplicateVersion)
                                {
                                    Id = 0
                                };
                            }
                        }
                    }
                }

                if (errors.IsNullOrEmpty())
                {
                    productBo.Code = TreatyPricingProductService.GetNextObjectId(productBo.TreatyPricingCedantId);
                    productBo.CreatedById = AuthUserId;
                    productBo.UpdatedById = AuthUserId;

                    TrailObject trail = GetNewTrailObject();

                    Result = TreatyPricingProductService.Create(ref productBo, ref trail);
                    if (Result.Valid)
                    {
                        TreatyPricingPickListDetailController.Save(TreatyPricingCedantBo.ObjectProduct, productBo.Id, PickListBo.UnderwritingMethod, productBo.UnderwritingMethod, AuthUserId, ref trail);

                        var cedantBo = TreatyPricingCedantService.Find(productBo.TreatyPricingCedantId);
                        cedantBo.NoOfProduct = TreatyPricingProductService.CountByTreatyPricingCedantId(productBo.TreatyPricingCedantId);
                        cedantBo.UpdatedById = AuthUserId;
                        TreatyPricingCedantService.Update(ref cedantBo, ref trail);

                        versionBo.TreatyPricingProductId = productBo.Id;
                        versionBo.Version = 1;
                        versionBo.PersonInChargeId = AuthUserId;
                        versionBo.CreatedById = AuthUserId;
                        versionBo.UpdatedById = AuthUserId;
                        TreatyPricingProductVersionService.Create(ref versionBo, ref trail);

                        versionBo.SetSelectValues();
                        TreatyPricingProductDetailController.Save(versionBo.JuvenileLien, versionBo.Id, TreatyPricingProductDetailBo.TypeJuvenileLien, AuthUserId, ref trail);
                        TreatyPricingProductDetailController.Save(versionBo.SpecialLien, versionBo.Id, TreatyPricingProductDetailBo.TypeSpecialLien, AuthUserId, ref trail);

                        int objectType = TreatyPricingCedantBo.ObjectProductVersion;
                        TreatyPricingPickListDetailController.Save(objectType, versionBo.Id, PickListBo.TargetSegment, versionBo.TargetSegment, AuthUserId, ref trail);
                        TreatyPricingPickListDetailController.Save(objectType, versionBo.Id, PickListBo.DistributionChannel, versionBo.DistributionChannel, AuthUserId, ref trail);
                        TreatyPricingPickListDetailController.Save(objectType, versionBo.Id, PickListBo.CessionType, versionBo.CessionType, AuthUserId, ref trail);
                        TreatyPricingPickListDetailController.Save(objectType, versionBo.Id, PickListBo.ProductLine, versionBo.ProductLine, AuthUserId, ref trail);

                        TreatyPricingProductBenefitController.Save(versionBo.Id, versionBo.TreatyPricingProductBenefit, AuthUserId, ref trail, true);

                        CreateTrail(
                            productBo.Id,
                            "Create Treaty Pricing Product"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(string.Format("Error occurred. Error details: {0}", ex.Message));
            }

            var productBos = TreatyPricingProductService.GetByTreatyPricingCedantId(productBo.TreatyPricingCedantId);

            return Json(new { errors, productBos });
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false, int workflowId = 0, bool isEditMode = false)
        {
            if (!CheckObjectLockReadOnly(Controller, id, isEditMode))
                return RedirectDashboard();

            var bo = TreatyPricingProductService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var model = new TreatyPricingProductViewModel(bo);
            model.WorkflowId = workflowId;
            LoadPage(model, versionId, isCalledFromWorkflow, isQuotationWorkflow);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, TreatyPricingProductViewModel model, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false, int workflowId = 0)
        {
            TreatyPricingProductBo productBo = TreatyPricingProductService.Find(id);
            if (productBo == null)
            {
                return RedirectToAction("Edit", "TreatyPricingCedant", new { id = model.TreatyPricingCedantId });
            }

            object routeValues = new { id = productBo.Id };
            if (isCalledFromWorkflow)
            {
                routeValues = new { id = productBo.Id, versionId, isCalledFromWorkflow, isQuotationWorkflow, workflowId };
            }

            if (!CheckObjectLock(Controller, id))
                return RedirectToAction("Edit", routeValues);

            TreatyPricingProductVersionBo updatedVersionBo = null;
            productBo.SetVersionObjects(productBo.TreatyPricingProductVersionBos);
            if (productBo.EditableVersion == model.CurrentVersion)
            {
                updatedVersionBo = model.GetVersionBo((TreatyPricingProductVersionBo)productBo.CurrentVersionObject);
                productBo.AddVersionObject(updatedVersionBo);

                foreach (string error in TreatyPricingProductBenefitController.Validate(updatedVersionBo.TreatyPricingProductBenefit))
                {
                    ModelState.AddModelError("", error);
                }

                if (model.PersonInChargeId == 0)
                {
                    ModelState.AddModelError("PersonInChargeId", string.Format(MessageBag.Required, "Person In-Charge (Business Development)"));
                }
            }

            if (ModelState.IsValid)
            {
                productBo = model.FormBo(productBo);
                productBo.UpdatedById = AuthUserId;

                TrailObject trail = GetNewTrailObject();
                Result = TreatyPricingProductService.Update(ref productBo, ref trail);
                if (Result.Valid)
                {
                    TreatyPricingProductPerLifeRetroController.Save(productBo.HasPerLifeRetro, productBo.Id, productBo.PerLifeRetroTreatyCode, AuthUserId, ref trail);
                    TreatyPricingPickListDetailController.Save(TreatyPricingCedantBo.ObjectProduct, productBo.Id, PickListBo.UnderwritingMethod, productBo.UnderwritingMethod, AuthUserId, ref trail);
                    if (updatedVersionBo != null)
                    {
                        updatedVersionBo.UpdatedById = AuthUserId;
                        updatedVersionBo.SetSelectValues();
                        TreatyPricingProductVersionService.Update(ref updatedVersionBo, ref trail);
                        TreatyPricingProductDetailController.Save(updatedVersionBo.JuvenileLien, updatedVersionBo.Id, TreatyPricingProductDetailBo.TypeJuvenileLien, AuthUserId, ref trail);
                        TreatyPricingProductDetailController.Save(updatedVersionBo.SpecialLien, updatedVersionBo.Id, TreatyPricingProductDetailBo.TypeSpecialLien, AuthUserId, ref trail);

                        int objectType = TreatyPricingCedantBo.ObjectProductVersion;
                        TreatyPricingPickListDetailController.Save(objectType, updatedVersionBo.Id, PickListBo.TargetSegment, updatedVersionBo.TargetSegment, AuthUserId, ref trail);
                        TreatyPricingPickListDetailController.Save(objectType, updatedVersionBo.Id, PickListBo.DistributionChannel, updatedVersionBo.DistributionChannel, AuthUserId, ref trail);
                        TreatyPricingPickListDetailController.Save(objectType, updatedVersionBo.Id, PickListBo.CessionType, updatedVersionBo.CessionType, AuthUserId, ref trail);
                        TreatyPricingPickListDetailController.Save(objectType, updatedVersionBo.Id, PickListBo.ProductLine, updatedVersionBo.ProductLine, AuthUserId, ref trail);

                        TreatyPricingProductBenefitController.Save(updatedVersionBo.Id, updatedVersionBo.TreatyPricingProductBenefit, AuthUserId, ref trail);
                    }

                    CreateTrail(
                        productBo.Id,
                        "Update Treaty Pricing Product"
                    );

                    SetUpdateSuccessMessage("Product", false);

                    ObjectLockController.DeleteObjectLock(Controller, id, AuthUserId);

                    return RedirectToAction("Edit", routeValues);
                }
                AddResult(Result);
            }

            model.CurrentVersion = productBo.CurrentVersion;
            model.CurrentVersionObject = productBo.CurrentVersionObject;
            model.VersionObjects = productBo.VersionObjects;
            model.TreatyPricingCedantBo = productBo.TreatyPricingCedantBo;
            model.WorkflowId = workflowId;
            LoadPage(model, versionId, isCalledFromWorkflow, isQuotationWorkflow);
            return View(model);
        }

        public void LoadPage(TreatyPricingProductViewModel model, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false)
        {
            DropDownVersion(model);

            var departmentId = Util.GetConfigInteger("TreatyPricingProductPicDepartment", DepartmentBo.DepartmentBD);
            List<SelectListItem> userList = DropDownUser(UserBo.StatusActive, departmentId: departmentId, selectedId: model.PersonInChargeId);

            //if (userList.Where(q => q.Value == model.PersonInChargeId.ToString()).Count() > 1)
            //    model.PersonInChargeId = 0;

            ViewBag.PerLifeRetroCodes = GetTreatyPricingPerLifeRetroCodes();
            ViewBag.UnderwritingMethodCodes = GetPickListDetailCodeDescription(PickListBo.UnderwritingMethod);
            ViewBag.TargetSegmentCodes = GetPickListDetailCodeDescription(PickListBo.TargetSegment);
            ViewBag.DistributionChannelCodes = GetPickListDetailCodeDescription(PickListBo.DistributionChannel);
            ViewBag.CessionTypeCodes = GetPickListDetailCodeDescription(PickListBo.CessionType);
            ViewBag.ProductLineCodes = GetPickListDetailCodeDescription(PickListBo.ProductLine);

            GetBenefits(false);
            ViewBag.RetroPartyBos = RetroPartyService.GetDirectRetro();

            ViewBag.TreatyPricingCampaignBos = TreatyPricingCampaignProductService.GetByTreatyPricingProductId(model.Id);
            ViewBag.TreatyPricingGroupReferralBos = TreatyPricingGroupReferralService.GetByTreatyPricingProductId(model.Id);

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

            DropDownProductType();
            DropDownBusinessOrigin();
            DropDownBusinessType();
            DropDownRiArrangement();
            DropDownPremiumFrequencyCode();
            DropDownUnearnedPremiumRefund();
            DropDownMfrs17BasicRider();
            DropDownPayoutType();
            DropDownAgeBasis();
            DropDownArrangementReinsuranceType();
            DropDownRiskPatternSum();
            DropDownTerritoryOfIssueCode();

            DropDownTreatyPricingMedicalTable(model.TreatyPricingCedantId);
            DropDownTreatyPricingFinancialTable(model.TreatyPricingCedantId);
            DropDownTreatyPricingUwQuestionnaire(model.TreatyPricingCedantId);
            DropDownTreatyPricingAdvantageProgram(model.TreatyPricingCedantId);
            DropDownTreatyPricingProfitCommission(model.TreatyPricingCedantId);

            DropDownTreatyPricingUnderwritingLimit(model.TreatyPricingCedantId);
            DropDownTreatyPricingClaimApprovalLimit(model.TreatyPricingCedantId);
            DropDownTreatyPricingRateTable(model.TreatyPricingCedantId);
            DropDownTreatyPricingDefinitionExclusion(model.TreatyPricingCedantId);

            // Remark & Document
            string downloadDocumentUrl = Url.Action("Download", "Document");
            GetRemarkDocument(model.ModuleId, model.TreatyPricingCedantId, downloadDocumentUrl, ModuleBo.ModuleController.TreatyPricingProduct.ToString(), model.Id);
            GetRemarkSubjects();

            var finalDocuments = DocumentService.GetBySubModule(model.ModuleId, model.TreatyPricingCedantId, ModuleBo.ModuleController.TreatyPricingProduct.ToString(), model.Id, true);
            foreach (var document in finalDocuments)
            {
                document.IsFileExists();
                document.GetDownloadLink(downloadDocumentUrl);
            }
            ViewBag.FinalDocuments = finalDocuments;

            //GetObjectVersionChangelog(ModuleBo.ModuleController.TreatyPricingProduct.ToString(), model.Id, model);

            DropDownWorkflowObjectTypes();
            DropDownWorkflowStatus();

            #region Get latest workflow object information
            var changelogItems = GetObjectVersionChangelog(ModuleBo.ModuleController.TreatyPricingProduct.ToString(), model.Id, model);
            var workflowObjectBo = changelogItems
                .Where(q => q.WorkflowObjectBo != null)
                .OrderByDescending(q => q.Version)
                .Select(q => q.WorkflowObjectBo)
                .FirstOrDefault();

            if (workflowObjectBo != null)
            {
                if (workflowObjectBo.Type == TreatyPricingWorkflowObjectBo.TypeQuotation)
                {
                    var bo = TreatyPricingQuotationWorkflowService.Find(workflowObjectBo.WorkflowId);

                    if (bo != null)
                    {
                        model.TargetSendDate = bo.TargetSendDate.HasValue ? bo.TargetSendDate.Value.ToString(Util.GetDateFormat()) : "";
                        model.LatestRevisionDate = bo.LatestRevisionDate.HasValue ? bo.LatestRevisionDate.Value.ToString(Util.GetDateFormat()) : "";
                        model.QuotationStatus = TreatyPricingQuotationWorkflowBo.GetStatusName(bo.Status);
                        model.QuotationStatusRemark = bo.StatusRemarks;
                    }
                }
                else
                {
                    var bo = TreatyPricingTreatyWorkflowService.Find(workflowObjectBo.WorkflowId);
                    var versionBo = TreatyPricingTreatyWorkflowVersionService.GetLatestVersionByTreatyPricingTreatyWorkflowId(workflowObjectBo.WorkflowId);

                    if (bo != null && versionBo != null)
                    {
                        model.TargetSendDate = versionBo.TargetSentDate.HasValue ? versionBo.TargetSentDate.Value.ToString(Util.GetDateFormat()) : "";
                        model.LatestRevisionDate = versionBo.LatestRevisionDate.HasValue ? versionBo.LatestRevisionDate.Value.ToString(Util.GetDateFormat()) : "";
                        model.QuotationStatus = TreatyPricingTreatyWorkflowBo.GetDocumentStatusName(bo.DocumentStatus);
                        model.QuotationStatusRemark = "";
                    }
                }
            }

            #endregion

            AuthUserName();

            SetViewBagMessage();
        }

        public ActionResult CreateVersion(TreatyPricingProductBo bo, bool duplicatePreviousVersion = false)
        {
            bo.SetVersionObjects(TreatyPricingProductVersionService.GetByTreatyPricingProductId(bo.Id));
            TreatyPricingProductVersionBo nextVersionBo;
            TreatyPricingProductVersionBo previousVersionBo = (TreatyPricingProductVersionBo)bo.CurrentVersionObject;

            List<string> errors = new List<string>();
            errors.AddRange(ValidateVersion(previousVersionBo));
            errors.AddRange(TreatyPricingProductBenefitController.Validate(previousVersionBo.TreatyPricingProductBenefit));
            if (!errors.IsNullOrEmpty())
            {
                return Json(new { errors });
            }

            if (duplicatePreviousVersion)
            {
                nextVersionBo = new TreatyPricingProductVersionBo(previousVersionBo)
                {
                    Id = 0,
                };
            }
            else
            {
                nextVersionBo = new TreatyPricingProductVersionBo()
                {
                    TreatyPricingProductId = bo.Id,
                };
            }

            nextVersionBo.Version = bo.EditableVersion + 1;
            nextVersionBo.PersonInChargeId = AuthUserId;
            nextVersionBo.CreatedById = AuthUserId;
            nextVersionBo.UpdatedById = AuthUserId;

            ObjectVersionChangelog changelog = null;
            TrailObject trail = GetNewTrailObject();
            nextVersionBo.SetSelectValues();
            Result = TreatyPricingProductVersionService.Create(ref nextVersionBo, ref trail);
            if (Result.Valid)
            {
                TreatyPricingProductDetailController.Save(nextVersionBo.JuvenileLien, nextVersionBo.Id, TreatyPricingProductDetailBo.TypeJuvenileLien, AuthUserId, ref trail, true);
                TreatyPricingProductDetailController.Save(nextVersionBo.SpecialLien, nextVersionBo.Id, TreatyPricingProductDetailBo.TypeSpecialLien, AuthUserId, ref trail, true);
                nextVersionBo.JuvenileLien = TreatyPricingProductDetailService.GetJsonByParentType(nextVersionBo.Id, TreatyPricingProductDetailBo.TypeJuvenileLien);
                nextVersionBo.SpecialLien = TreatyPricingProductDetailService.GetJsonByParentType(nextVersionBo.Id, TreatyPricingProductDetailBo.TypeSpecialLien);

                int objectType = TreatyPricingCedantBo.ObjectProductVersion;
                TreatyPricingPickListDetailController.Save(objectType, nextVersionBo.Id, PickListBo.TargetSegment, nextVersionBo.TargetSegment, AuthUserId, ref trail);
                TreatyPricingPickListDetailController.Save(objectType, nextVersionBo.Id, PickListBo.DistributionChannel, nextVersionBo.DistributionChannel, AuthUserId, ref trail);
                TreatyPricingPickListDetailController.Save(objectType, nextVersionBo.Id, PickListBo.CessionType, nextVersionBo.CessionType, AuthUserId, ref trail);
                TreatyPricingPickListDetailController.Save(objectType, nextVersionBo.Id, PickListBo.ProductLine, nextVersionBo.ProductLine, AuthUserId, ref trail);

                TreatyPricingProductBenefitController.Save(nextVersionBo.Id, nextVersionBo.TreatyPricingProductBenefit, AuthUserId, ref trail, true);
                nextVersionBo.TreatyPricingProductBenefit = TreatyPricingProductBenefitService.GetJsonByVersionId(nextVersionBo.Id);

                UserTrailBo userTrail = CreateTrail(
                    nextVersionBo.Id,
                    "Create Treaty Pricing Product Version"
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

        public static List<string> ValidateVersion(TreatyPricingProductVersionBo versionBo)
        {
            List<string> errors = new List<string>();
            TreatyPricingProductViewModel model = new TreatyPricingProductViewModel();

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
        public JsonResult GetWorkflowData(TreatyPricingWorkflowObjectBo workflowObjectBo)
        {
            string targetSendDate = "", latestRevisionDate = "", quotationStatus = "", quotationStatusRemark = "";

            var bos = TreatyPricingWorkflowObjectService.GetByObjectTypeObjectId(workflowObjectBo.ObjectType, workflowObjectBo.ObjectId);
            var bo = bos.OrderByDescending(q => q.ObjectVersionId).FirstOrDefault();

            if (bo != null)
            {
                if (bo.Type == TreatyPricingWorkflowObjectBo.TypeQuotation)
                {
                    var workflowBo = TreatyPricingQuotationWorkflowService.Find(bo.WorkflowId);

                    if (workflowBo != null)
                    {
                        targetSendDate = workflowBo.TargetSendDate.HasValue ? workflowBo.TargetSendDate.Value.ToString(Util.GetDateFormat()) : "";
                        latestRevisionDate = workflowBo.LatestRevisionDate.HasValue ? workflowBo.LatestRevisionDate.Value.ToString(Util.GetDateFormat()) : "";
                        quotationStatus = TreatyPricingQuotationWorkflowBo.GetStatusName(workflowBo.Status);
                        quotationStatusRemark = workflowBo.StatusRemarks;
                    }
                }
                else
                {
                    var workflowBo = TreatyPricingTreatyWorkflowService.Find(bo.WorkflowId);
                    var versionBo = TreatyPricingTreatyWorkflowVersionService.GetLatestVersionByTreatyPricingTreatyWorkflowId(bo.WorkflowId);

                    if (workflowBo != null && versionBo != null)
                    {
                        targetSendDate = versionBo.TargetSentDate.HasValue ? versionBo.TargetSentDate.Value.ToString(Util.GetDateFormat()) : "";
                        latestRevisionDate = versionBo.LatestRevisionDate.HasValue ? versionBo.LatestRevisionDate.Value.ToString(Util.GetDateFormat()) : "";
                        quotationStatus = TreatyPricingTreatyWorkflowBo.GetDocumentStatusName(workflowBo.DocumentStatus);
                        quotationStatusRemark = "";
                    }
                }
            }

            return Json(new { targetSendDate, latestRevisionDate, quotationStatus, quotationStatusRemark });
        }
    }
}