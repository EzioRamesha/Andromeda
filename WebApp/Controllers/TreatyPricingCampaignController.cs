using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using Services.TreatyPricing;
using Shared;
using Shared.DataAccess;
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
    public class TreatyPricingCampaignController : BaseController
    {
        public const string Controller = "TreatyPricingCampaign";

        // GET: TreatyPricingCampaign
        public ActionResult Index()
        {
            return View();
        }

        // POST: TreatyPricingCampaign/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(TreatyPricingCampaignBo campaignBo)
        {
            List<string> errors = new List<string>();

            if (!CheckPower(Controller, "C"))
            {
                errors.Add(string.Format(MessageBag.AccessDeniedWithActionName, "Add", "Campaign"));
                return Json(new { errors });
            }

            try
            {
                TreatyPricingCampaignVersionBo versionBo = new TreatyPricingCampaignVersionBo();
                if (!campaignBo.DuplicateFromList && string.IsNullOrEmpty(campaignBo.Name))
                {
                    errors.Add("Name is required");
                }

                if (campaignBo.IsDuplicateExisting)
                {
                    if (!campaignBo.DuplicateTreatyPricingCampaignId.HasValue)
                    {
                        errors.Add("Campaign is required");
                    }

                    if (!campaignBo.DuplicateFromList && !campaignBo.DuplicateTreatyPricingCampaignVersionId.HasValue)
                    {
                        errors.Add("Version is required");
                    }

                    if (errors.IsNullOrEmpty())
                    {
                        TreatyPricingCampaignBo duplicate = TreatyPricingCampaignService.Find(campaignBo.DuplicateTreatyPricingCampaignId);
                        if (duplicate == null)
                        {
                            errors.Add("Campaign not found");
                        }
                        else
                        {
                            campaignBo.Type = duplicate.Type;
                            campaignBo.Purpose = duplicate.Purpose;
                            campaignBo.PeriodStartDate = duplicate.PeriodStartDate;
                            campaignBo.PeriodEndDate = duplicate.PeriodEndDate;
                            campaignBo.Period = duplicate.Period;
                            campaignBo.Duration = duplicate.Duration;
                            campaignBo.TargetTakeUpRate = duplicate.TargetTakeUpRate;
                            campaignBo.AverageSumAssured = duplicate.AverageSumAssured;
                            campaignBo.RiPremiumReceivable = duplicate.RiPremiumReceivable;
                            campaignBo.NoOfPolicy = duplicate.NoOfPolicy;
                            campaignBo.Remarks = duplicate.Remarks;

                            TreatyPricingCampaignVersionBo duplicateVersion = null;
                            if (campaignBo.DuplicateTreatyPricingCampaignVersionId.HasValue)
                            {
                                duplicateVersion = duplicate.TreatyPricingCampaignVersionBos.Where(q => q.Id == campaignBo.DuplicateTreatyPricingCampaignVersionId.Value).FirstOrDefault();
                            }
                            else
                            {
                                duplicateVersion = duplicate.TreatyPricingCampaignVersionBos.OrderByDescending(q => q.Version).First();
                            }

                            if (duplicateVersion == null)
                            {
                                errors.Add("Campaign Version not found");
                            }
                            else
                            {
                                versionBo = new TreatyPricingCampaignVersionBo(duplicateVersion)
                                {
                                    Id = 0
                                };
                            }
                        }
                    }
                }

                if (errors.IsNullOrEmpty())
                {
                    campaignBo.Status = TreatyPricingCampaignBo.StatusActive;
                    campaignBo.Code = TreatyPricingCampaignService.GetNextObjectId(campaignBo.TreatyPricingCedantId);
                    campaignBo.CreatedById = AuthUserId;
                    campaignBo.UpdatedById = AuthUserId;

                    TrailObject trail = GetNewTrailObject();
                    Result = TreatyPricingCampaignService.Create(ref campaignBo, ref trail);
                    if (Result.Valid)
                    {
                        versionBo.TreatyPricingCampaignId = campaignBo.Id;
                        versionBo.Version = 1;
                        versionBo.PersonInChargeId = AuthUserId;
                        versionBo.CreatedById = AuthUserId;
                        versionBo.UpdatedById = AuthUserId;
                        TreatyPricingCampaignVersionService.Create(ref versionBo, ref trail);

                        //versionBo.SetSelectValues();

                        CreateTrail(
                            campaignBo.Id,
                            "Create Treaty Pricing Campaign"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(string.Format("Error occurred. Error details: ", ex.Message));
            }

            var campaignBos = TreatyPricingCampaignService.GetByTreatyPricingCedantId(campaignBo.TreatyPricingCedantId);

            return Json(new { errors, campaignBos });
        }

        // GET: TreatyPricingCampaign/Edit/5
        public ActionResult Edit(int id, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false, int workflowId = 0, bool isEditMode = false)
        {
            if (!CheckObjectLockReadOnly(Controller, id, isEditMode))
                return RedirectDashboard();

            TreatyPricingCampaignBo bo = TreatyPricingCampaignService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var model = new TreatyPricingCampaignViewModel(bo);
            model.WorkflowId = workflowId;
            LoadPage(model, versionId, isCalledFromWorkflow, isQuotationWorkflow);
            return View(model);
        }

        // POST: TreatyPricingCampaign/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, TreatyPricingCampaignViewModel model, FormCollection form, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false, int workflowId = 0)
        {
            var dbBo = TreatyPricingCampaignService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            object routeValues = new { id = dbBo.Id };
            if (isCalledFromWorkflow)
            {
                routeValues = new { id = dbBo.Id, versionId, isCalledFromWorkflow, isQuotationWorkflow, workflowId };
            }

            if (!CheckObjectLock(Controller, id))
                return RedirectToAction("Edit", routeValues);

            TreatyPricingCampaignVersionBo updatedVersionBo = null;
            dbBo.SetVersionObjects(dbBo.TreatyPricingCampaignVersionBos);
            if (dbBo.EditableVersion == model.CurrentVersion)
            {
                updatedVersionBo = model.GetVersionBo((TreatyPricingCampaignVersionBo)dbBo.CurrentVersionObject);
                dbBo.AddVersionObject(updatedVersionBo);
            }

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();
                Result = TreatyPricingCampaignService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    if (updatedVersionBo != null)
                    {
                        updatedVersionBo.UpdatedById = AuthUserId;
                        updatedVersionBo.SetSelectValues();
                        TreatyPricingCampaignVersionService.Update(ref updatedVersionBo, ref trail);
                    }
                    //model.ProcessProducts(form, ref trail);

                    CreateTrail(
                        bo.Id,
                        "Update Treaty Pricing Campaign"
                    );

                    SetUpdateSuccessMessage(Controller);

                    ObjectLockController.DeleteObjectLock(Controller, id, AuthUserId);

                    return RedirectToAction("Edit", routeValues);
                }
                AddResult(Result);
            }

            model.CurrentVersion = dbBo.CurrentVersion;
            model.CurrentVersionObject = dbBo.CurrentVersionObject;
            model.VersionObjects = dbBo.VersionObjects;
            model.TreatyPricingCedantBo = dbBo.TreatyPricingCedantBo;
            model.WorkflowId = workflowId;
            LoadPage(model, versionId, isCalledFromWorkflow, isQuotationWorkflow);
            return View(model);
        }

        public void LoadPage(TreatyPricingCampaignViewModel model, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false)
        {
            AuthUserName();
            DropDownVersion(model);
            DropDownUser(UserBo.StatusActive, model.PersonInChargeId);
            DropDownCedant(CedantBo.StatusActive, model.TreatyPricingCedantBo.CedantId);
            DropDownStatus();
            DropDownAgeBasis();
            DropDownProductType();
            DropDownProduct(model.TreatyPricingCedantId);
            DropDownProductQuotation(model.TreatyPricingCedantId);
            GetCampaignTypes();

            ViewBag.CampaignTypeCodes = GetPickListDetailCodes(PickListBo.CampaignType);
            ViewBag.TargetSegmentCodes = GetPickListDetailCodeDescription(PickListBo.TargetSegment);
            ViewBag.DistributionChannelCodes = GetPickListDetailCodeDescription(PickListBo.DistributionChannel);
            ViewBag.UnderwritingMethodCodes = GetPickListDetailCodeDescription(PickListBo.UnderwritingMethod);

            DropDownTreatyPricingMedicalTable(model.TreatyPricingCedantId);
            DropDownTreatyPricingFinancialTable(model.TreatyPricingCedantId);
            DropDownTreatyPricingUwQuestionnaire(model.TreatyPricingCedantId);
            DropDownTreatyPricingAdvantageProgram(model.TreatyPricingCedantId);
            DropDownTreatyPricingProfitCommission(model.TreatyPricingCedantId);
            DropDownTreatyPricingRateTable(model.TreatyPricingCedantId);

            ViewBag.CampaignProducts = TreatyPricingCampaignProductService.GetByTreatyPricingCampaignId(model.Id);
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
            GetRemarkDocument(model.ModuleId, model.TreatyPricingCedantId, downloadDocumentUrl, ModuleBo.ModuleController.TreatyPricingCampaign.ToString(), model.Id);
            GetRemarkSubjects();

            GetObjectVersionChangelog(ModuleBo.ModuleController.TreatyPricingCampaign.ToString(), model.Id, model);
            DropDownWorkflowObjectTypes();
            DropDownWorkflowStatus();

            var entity = new TreatyPricingCampaign();
            var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Remarks");
            ViewBag.RemarkMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingCampaignBo.StatusMax))
            {
                items.Add(new SelectListItem { Text = TreatyPricingCampaignBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownStatus = items;
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

        public ActionResult CreateVersion(TreatyPricingCampaignBo bo, bool duplicatePreviousVersion = false)
        {
            bo.SetVersionObjects(TreatyPricingCampaignVersionService.GetByTreatyPricingCampaignId(bo.Id));
            TreatyPricingCampaignVersionBo nextVersionBo;
            TreatyPricingCampaignVersionBo previousVersionBo = (TreatyPricingCampaignVersionBo)bo.CurrentVersionObject;

            if (duplicatePreviousVersion)
            {
                nextVersionBo = new TreatyPricingCampaignVersionBo(previousVersionBo)
                {
                    Id = 0,
                };
            }
            else
            {
                nextVersionBo = new TreatyPricingCampaignVersionBo()
                {
                    TreatyPricingCampaignId = bo.Id,
                };
            }

            nextVersionBo.Version = bo.EditableVersion + 1;
            nextVersionBo.PersonInChargeId = AuthUserId;
            nextVersionBo.CreatedById = AuthUserId;
            nextVersionBo.UpdatedById = AuthUserId;

            ObjectVersionChangelog changelog = null;

            TrailObject trail = GetNewTrailObject();
            nextVersionBo.SetSelectValues();
            Result = TreatyPricingCampaignVersionService.Create(ref nextVersionBo, ref trail);
            if (Result.Valid)
            {
                UserTrailBo userTrail = CreateTrail(
                    nextVersionBo.Id,
                    "Create Treaty Pricing UW Questionnaire Version"
                );

                changelog = new ObjectVersionChangelog(nextVersionBo, userTrail);
                changelog.FormatBetweenVersionTrail(previousVersionBo);
            }

            bo.AddVersionObject(nextVersionBo);
            var versions = DropDownVersion(bo);

            return Json(new { bo, versions });
        }

        [HttpPost]
        public JsonResult GetProductData(int campaignId, int? cedantId, int? treatyPricingCedantId, string quotationName, string underwritingMethods, string distributionChannels, string targetSegments, int? productType)
        {
            var queryProductBos = TreatyPricingProductVersionService.GetBySearchParams(cedantId, treatyPricingCedantId, quotationName, underwritingMethods, distributionChannels, targetSegments, productType);
            var productBos = new List<TreatyPricingProductBo>();

            var existingProduct = TreatyPricingCampaignProductService.GetByTreatyPricingCampaignId(campaignId).Select(q => q.TreatyPricingProductId).ToList();
            if (existingProduct.Count > 0)
                queryProductBos = queryProductBos.Where(q => !existingProduct.Contains(q.TreatyPricingProductId)).ToList();

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
            var campaign = TreatyPricingCampaignProductService.GetCodeByProductName(productName);
            return Json(new { Campaign = campaign });
        }

        private List<string> GetCampaignTypes()
        {
            var items = new List<string>();

            foreach (var i in Enumerable.Range(1, TreatyPricingCampaignBo.TypeUnderwritingProgram))
            {
                items.Add(TreatyPricingCampaignBo.GetTypeName(i));
            }
            ViewBag.CampaignTypes = items;
            return items;
        }

        [HttpPost]
        public JsonResult GetByTreatyPricingCedant(int? id)
        {
            IList<TreatyPricingCampaignBo> bos = new List<TreatyPricingCampaignBo>();
            if (id.HasValue)
            {
                bos = TreatyPricingCampaignService.GetByTreatyPricingCedantId(id.Value);
            }
            return Json(new { bos });
        }

        [HttpPost]
        public JsonResult GetByTreatyPricingCampaign(int? id)
        {
            IList<TreatyPricingCampaignVersionBo> versionBos = new List<TreatyPricingCampaignVersionBo>();
            if (id.HasValue)
            {
                versionBos = TreatyPricingCampaignVersionService.GetByTreatyPricingCampaignId(id.Value).ToList();
            }
            return Json(new { versionBos });
        }
    }
}