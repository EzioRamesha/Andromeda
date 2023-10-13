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
    public class TreatyPricingCustomOtherController : BaseController
    {
        public const string Controller = "TreatyPricingCustomOther";

        //[Auth(Controller = Controller, Power = "C")]
        [HttpPost]
        public JsonResult Add(TreatyPricingCustomOtherBo customOtherBo)
        {
            List<string> errors = new List<string>();

            if (!CheckPower(Controller, "C"))
            {
                errors.Add(string.Format(MessageBag.AccessDeniedWithActionName, "Add", "Custom / Other"));
                return Json(new { errors });
            }

            try
            {
                TreatyPricingCustomOtherVersionBo versionBo = new TreatyPricingCustomOtherVersionBo();
                if (!customOtherBo.DuplicateFromList && string.IsNullOrEmpty(customOtherBo.Name))
                {
                    errors.Add("Name is required");
                }

                if (customOtherBo.IsDuplicateExisting)
                {
                    if (!customOtherBo.DuplicateTreatyPricingCustomOtherId.HasValue)
                    {
                        errors.Add("Custom / Other is required");
                    }

                    if (!customOtherBo.DuplicateFromList && !customOtherBo.DuplicateTreatyPricingCustomOtherVersionId.HasValue)
                    {
                        errors.Add("Version is required");
                    }

                    if (errors.IsNullOrEmpty())
                    {
                        TreatyPricingCustomOtherBo duplicate = TreatyPricingCustomOtherService.Find(customOtherBo.DuplicateTreatyPricingCustomOtherId);
                        if (duplicate == null)
                        {
                            errors.Add("Custom / Other not found");
                        }
                        else
                        {
                            TreatyPricingCustomOtherVersionBo duplicateVersion = null;
                            if (customOtherBo.DuplicateTreatyPricingCustomOtherVersionId.HasValue)
                            {
                                duplicateVersion = duplicate.TreatyPricingCustomOtherVersionBos.Where(q => q.Id == customOtherBo.DuplicateTreatyPricingCustomOtherVersionId.Value).FirstOrDefault();
                            }
                            else
                            {
                                duplicateVersion = duplicate.TreatyPricingCustomOtherVersionBos.OrderByDescending(q => q.Version).First();
                            }

                            if (duplicateVersion == null)
                            {
                                errors.Add("Product Version not found");
                            }
                            else
                            {
                                versionBo = new TreatyPricingCustomOtherVersionBo(duplicateVersion)
                                {
                                    Id = 0
                                };
                            }
                        }
                    }
                }

                if (errors.IsNullOrEmpty())
                {
                    customOtherBo.Status = TreatyPricingCustomOtherBo.StatusActive;
                    customOtherBo.Code = TreatyPricingCustomOtherService.GetNextObjectId(customOtherBo.TreatyPricingCedantId);
                    customOtherBo.CreatedById = AuthUserId;
                    customOtherBo.UpdatedById = AuthUserId;

                    TrailObject trail = GetNewTrailObject();

                    Result = TreatyPricingCustomOtherService.Create(ref customOtherBo, ref trail);
                    if (Result.Valid)
                    {
                        versionBo.TreatyPricingCustomOtherId = customOtherBo.Id;
                        versionBo.Version = 1;
                        versionBo.PersonInChargeId = AuthUserId;
                        versionBo.CreatedById = AuthUserId;
                        versionBo.UpdatedById = AuthUserId;

                        TreatyPricingCustomOtherVersionService.Create(ref versionBo, ref trail);
                        CreateTrail(
                            customOtherBo.Id,
                            "Create Treaty Pricing Custom Other"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(string.Format("Error occurred. Error details: {0}", ex.Message));
            }

            var customOtherBos = TreatyPricingCustomOtherService.GetByTreatyPricingCedantId(customOtherBo.TreatyPricingCedantId);

            return Json(new { errors, customOtherBos });
        }

        // GET: TreatyPricingCustomOther
        public ActionResult Edit(int id, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false, int workflowId = 0, bool isEditMode = false)
        {
            if (!CheckObjectLockReadOnly(Controller, id, isEditMode))
                return RedirectDashboard();

            var bo = TreatyPricingCustomOtherService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var model = new TreatyPricingCustomOtherViewModel(bo);
            model.WorkflowId = workflowId;
            LoadPage(model, versionId, isCalledFromWorkflow, isQuotationWorkflow);
            return View(model);
        }

        // POST: TreatyPricingCustomOther/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, TreatyPricingCustomOtherViewModel model, FormCollection form, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false, int workflowId = 0)
        {
            var dbBo = TreatyPricingCustomOtherService.Find(id);
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

            TreatyPricingCustomOtherVersionBo updatedVersionBo = null;
            string fileName = null;
            HttpPostedFileBase uploadItem = null;
            bool isFileUploaded = false;
            string oldFilePath = null;

            dbBo.SetVersionObjects(dbBo.TreatyPricingCustomOtherVersionBos);
            if (dbBo.EditableVersion == model.CurrentVersion)
            {
                TreatyPricingCustomOtherVersionBo oldVersionBo = (TreatyPricingCustomOtherVersionBo)dbBo.CurrentVersionObject;
                oldFilePath = oldVersionBo.GetLocalPath();
                updatedVersionBo = model.GetVersionBo(oldVersionBo);

                isFileUploaded = model.Upload != null && model.Upload[0] != null;
                if (isFileUploaded)
                {
                    uploadItem = model.Upload[0];
                    fileName = Path.GetFileName(uploadItem.FileName);

                    int fileSize = uploadItem.ContentLength / 1024 / 1024 / 1024;
                    if (fileSize >= 2)
                        Result.AddError("Uploaded file's size exceeded 2 GB");
                }

                dbBo.AddVersionObject(updatedVersionBo);
            }

            if (ModelState.IsValid)
            {
                dbBo = model.FormBo(dbBo);
                dbBo.UpdatedById = AuthUserId;

                var trail = GetNewTrailObject();

                Result = TreatyPricingCustomOtherService.Update(ref dbBo, ref trail);
                if (Result.Valid)
                {
                    if (updatedVersionBo != null)
                    {
                        if (isFileUploaded)
                        {
                            updatedVersionBo.FileName = fileName;
                            updatedVersionBo.FormatHashFileName();
                            string path = updatedVersionBo.GetLocalPath();
                            if (!string.IsNullOrEmpty(oldFilePath) && System.IO.File.Exists(oldFilePath))
                            {
                                // Delete old file
                                System.IO.File.Delete(oldFilePath);
                            }

                            // Create new file
                            Util.MakeDir(path);
                            uploadItem.SaveAs(path);
                        }

                        updatedVersionBo.UpdatedById = AuthUserId;
                        TreatyPricingCustomOtherVersionService.Update(ref updatedVersionBo, ref trail);
                        
                    }

                    //model.ProcessProducts(form, ref trail);

                    CreateTrail(
                        dbBo.Id,
                        "Update Treaty Pricing Custom Other"
                    );

                    SetUpdateSuccessMessage("Custom Other", false);

                    ObjectLockController.DeleteObjectLock(Controller, id, AuthUserId);

                    return RedirectToAction("Edit", routeValues);
                }
                AddResult(Result);
            }
            else
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            }

            model.Upload = null;
            model.UploadedByBo = UserService.Find(model.UploadedById);
            model.CurrentVersion = dbBo.CurrentVersion;
            model.CurrentVersionObject = dbBo.CurrentVersionObject;
            model.VersionObjects = dbBo.VersionObjects;
            model.TreatyPricingCedantBo = dbBo.TreatyPricingCedantBo;

            model.WorkflowId = workflowId;
            LoadPage(model, versionId, isCalledFromWorkflow, isQuotationWorkflow);
            return View(model);
        }

        public void LoadPage(TreatyPricingCustomOtherViewModel model, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false)
        {
            AuthUserName();
            ViewBag.PersonInChargeId = AuthUserId;
            ViewBag.PersonInChargeName = UserService.Find(AuthUserId).FullName;
            DropDownProductType();
            DropDownProduct(model.TreatyPricingCedantId);
            DropDownProductQuotation(model.TreatyPricingCedantId);

            ViewBag.CustomOtherProducts = TreatyPricingCustomOtherProductService.GetByTreatyPricingCustomOtherId(model.Id);

            DropDownCedant(CedantBo.StatusActive, model.TreatyPricingCedantBo.CedantId);
            ViewBag.CustomOtherTypeCodes = GetPickListDetailCodes(PickListBo.ProductType);
            ViewBag.TargetSegmentCodes = GetPickListDetailCodeDescription(PickListBo.TargetSegment);
            ViewBag.DistributionChannelCodes = GetPickListDetailCodeDescription(PickListBo.DistributionChannel);
            ViewBag.UnderwritingMethodCodes = GetPickListDetailCodeDescription(PickListBo.UnderwritingMethod);

            ViewBag.CustomOtherProducts = TreatyPricingCustomOtherProductService.GetByTreatyPricingCustomOtherId(model.Id);

            DropDownVersion(model);

            if (versionId > 0)
            {
                model.SetCurrentVersionObject(versionId);
            }

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

            DropDownUser(UserBo.StatusActive);
            GetBenefitCodes();

            // Remark & Document
            string downloadDocumentUrl = Url.Action("Download", "Document");
            GetRemarkDocument(model.ModuleId, model.TreatyPricingCedantId, downloadDocumentUrl, ModuleBo.ModuleController.TreatyPricingCustomOther.ToString(), model.Id);
            GetRemarkSubjects();

            var entity = new TreatyPricingCustomOther();
            var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Description");
            ViewBag.DescriptionMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

            if (model.Id == 0)
            {
                // Create
            }
            else
            {
                //Edit
            }


            GetObjectVersionChangelog(ModuleBo.ModuleController.TreatyPricingCustomOther.ToString(), model.Id, model);
            DropDownWorkflowObjectTypes();
            DropDownWorkflowStatus();

            SetViewBagMessage();
        }

        public ActionResult CreateVersion(TreatyPricingCustomOtherBo bo, bool duplicatePreviousVersion = false)
        {
            bo.SetVersionObjects(TreatyPricingCustomOtherVersionService.GetByTreatyPricingCustomOtherId(bo.Id));
            TreatyPricingCustomOtherVersionBo nextVersionBo;
            TreatyPricingCustomOtherVersionBo previousVersionBo = (TreatyPricingCustomOtherVersionBo)bo.CurrentVersionObject;

            List<string> errors = new List<string>();
            errors.AddRange(ValidateVersion(previousVersionBo));
            if (!errors.IsNullOrEmpty())
            {
                return Json(new { errors });
            }

            if (duplicatePreviousVersion)
            {
                nextVersionBo = new TreatyPricingCustomOtherVersionBo(previousVersionBo)
                {
                    Id = 0,
                };
            }
            else
            {
                nextVersionBo = new TreatyPricingCustomOtherVersionBo()
                {
                    TreatyPricingCustomOtherId = bo.Id,
                };
            }

            nextVersionBo.Version = bo.EditableVersion + 1;
            nextVersionBo.PersonInChargeId = AuthUserId;
            nextVersionBo.CreatedById = AuthUserId;
            nextVersionBo.UpdatedById = AuthUserId;

            ObjectVersionChangelog changelog = null;
            TrailObject trail = GetNewTrailObject();
            Result = TreatyPricingCustomOtherVersionService.Create(ref nextVersionBo, ref trail);
            if (Result.Valid)
            {
                UserTrailBo userTrail = CreateTrail(
                    nextVersionBo.Id,
                    "Create Treaty Pricing Custom Other Version"
                );

                changelog = new ObjectVersionChangelog(nextVersionBo, userTrail);
                changelog.FormatBetweenVersionTrail(previousVersionBo);
            }

            bo.AddVersionObject(nextVersionBo);
            var versions = DropDownVersion(bo);

            return Json(new { bo, versions, changelog });
        }

        public void DownloadFile(int versionId)
        {
            try
            {
                TreatyPricingCustomOtherVersionBo bo = TreatyPricingCustomOtherVersionService.Find(versionId);

                Response.ClearContent();
                Response.Clear();
                //Response.Buffer = true;                                                                                                                   
                //Response.BufferOutput = false;
                Response.ContentType = MimeMapping.GetMimeMapping(bo.FileName);
                Response.AddHeader("Content-Disposition", "attachment; filename=" + bo.FileName);
                Response.TransmitFile(bo.GetLocalPath());
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

        [HttpPost]
        public JsonResult GetProductData(int customOtherId, int? cedantId, int? treatyPricingCedantId, string quotationName, string underwritingMethods, string distributionChannels, string targetSegments, int? productType)
        {
            var queryProductBos = TreatyPricingProductVersionService.GetBySearchParams(cedantId, treatyPricingCedantId, quotationName, underwritingMethods, distributionChannels, targetSegments, productType);
            var productBos = new List<TreatyPricingProductBo>();

            var existingProduct = TreatyPricingCustomOtherProductService.GetByTreatyPricingCustomOtherId(customOtherId).Select(q => q.TreatyPricingProductId).ToList();
            if (existingProduct.Count > 0)
                queryProductBos = queryProductBos.Where(q => !existingProduct.Contains(q.Id)).ToList();

            foreach (var productVer in queryProductBos)
            {
                var productBo = TreatyPricingProductService.Find(productVer.TreatyPricingProductId);
                productBo.TreatyPricingProductVersionBos = null;
                productBo.LatestTreatyPricingProductVersionBo = productVer;
                productBos.Add(productBo);
            }

            return Json(new { productBos });
        }

        [HttpPost]
        public JsonResult Search(string productName)
        {
            var customOther = TreatyPricingCustomOtherProductService.GetCodeByProductName(productName);
            return Json(new { CustomOther = customOther });
        }

        public List<SelectListItem> DropDownProductQuotation(int treatyPricingCedantId)
        {
            var items = GetEmptyDropDownList();
            foreach (var productBo in TreatyPricingProductService.GetByTreatyPricingCedantId(treatyPricingCedantId))
            {
                items.Add(new SelectListItem { Text = productBo.QuotationName, Value = productBo.QuotationName });
            }
            ViewBag.DropDownProductQuotations = items;
            return items;
        }

        public List<SelectListItem> DropDownProduct(int treatyPricingCedantId)
        {
            var items = GetEmptyDropDownList();
            foreach (var productBo in TreatyPricingProductService.GetByTreatyPricingCedantId(treatyPricingCedantId))
            {
                items.Add(new SelectListItem { Text = string.Format("{0} - {1}", productBo.Code, productBo.Name), Value = productBo.Id.ToString() });
            }
            ViewBag.DropDownProducts = items;
            return items;
        }

        public static List<string> ValidateVersion(TreatyPricingCustomOtherVersionBo versionBo)
        {
            List<string> errors = new List<string>();
            TreatyPricingCustomOtherViewModel model = new TreatyPricingCustomOtherViewModel();

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
            IList<TreatyPricingCustomOtherBo> bos = new List<TreatyPricingCustomOtherBo>();
            if (id.HasValue)
            {
                bos = TreatyPricingCustomOtherService.GetByTreatyPricingCedantId(id.Value);
            }
            return Json(new { bos });
        }

        [HttpPost]
        public JsonResult GetByTreatyPricingCustomOther(int? id)
        {
            IList<TreatyPricingCustomOtherVersionBo> versionBos = new List<TreatyPricingCustomOtherVersionBo>();
            if (id.HasValue)
            {
                versionBos = TreatyPricingCustomOtherVersionService.GetByTreatyPricingCustomOtherId(id.Value).ToList();
            }
            return Json(new { versionBos });
        }
    }
}