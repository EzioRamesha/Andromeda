using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using Services.TreatyPricing;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class TreatyPricingRateTableController : BaseController
    {
        public const string Controller = "TreatyPricingRateTableGroup";

        // GET: TreatyPricingRateTable/Edit/5
        public ActionResult Edit(int id, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false, int workflowId = 0, bool isEditMode = false)
        {
            if (!CheckObjectLockReadOnly(Controller, id, isEditMode))
                return RedirectDashboard();

            var bo = TreatyPricingRateTableService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var model = new TreatyPricingRateTableViewModel(bo)
            {
                WorkflowId = workflowId
            };
            LoadPage(model, versionId, isCalledFromWorkflow, isQuotationWorkflow);
            return View(model);
        }

        // POST: TreatyPricingRateTable/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, TreatyPricingRateTableViewModel model, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false, int workflowId = 0)
        {
            var rateTableBo = TreatyPricingRateTableService.Find(id);
            if (rateTableBo == null)
            {
                return RedirectToAction("Edit", "TreatyPricingRateTableGroup", new { id = model.TreatyPricingRateTableGroupId });
            }

            object routeValues = new { id = rateTableBo.Id };
            if (isCalledFromWorkflow)
            {
                routeValues = new { id = rateTableBo.Id, versionId, isCalledFromWorkflow, isQuotationWorkflow, workflowId };
            }

            if (!CheckObjectLock(Controller, id))
                return RedirectToAction("Edit", routeValues);

            TreatyPricingRateTableVersionBo updatedVersionBo = null;
            rateTableBo.SetVersionObjects(rateTableBo.TreatyPricingRateTableVersionBos);
            if (rateTableBo.EditableVersion == model.CurrentVersion)
            {
                updatedVersionBo = model.GetVersionBo((TreatyPricingRateTableVersionBo)rateTableBo.CurrentVersionObject);
                rateTableBo.AddVersionObject(updatedVersionBo);
            }

            Result childResult = new Result();

            Result = TreatyPricingRateTableService.Result();
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(rateTableBo.CreatedById, AuthUserId);
                bo.Id = rateTableBo.Id;

                var trail = GetNewTrailObject();

                if (childResult.Valid)
                {
                    Result = TreatyPricingRateTableService.Update(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        if (updatedVersionBo != null)
                        {
                            updatedVersionBo.UpdatedById = AuthUserId;
                            TreatyPricingRateTableVersionService.Update(ref updatedVersionBo, ref trail);
                            TreatyPricingRateTableDetailController.Save(updatedVersionBo.LargeSizeDiscount, updatedVersionBo.Id, TreatyPricingRateTableDetailBo.TypeLargeSizeDiscount, AuthUserId, ref trail);
                            TreatyPricingRateTableDetailController.Save(updatedVersionBo.JuvenileLien, updatedVersionBo.Id, TreatyPricingRateTableDetailBo.TypeJuvenileLien, AuthUserId, ref trail);
                            TreatyPricingRateTableDetailController.Save(updatedVersionBo.SpecialLien, updatedVersionBo.Id, TreatyPricingRateTableDetailBo.TypeSpecialLien, AuthUserId, ref trail);
                        }

                        CreateTrail(
                            bo.Id,
                            "Update Treaty Pricing Rate Table"
                        );

                        SetUpdateSuccessMessage("Rate Table", false);

                        ObjectLockController.DeleteObjectLock(Controller, id, AuthUserId);

                        return RedirectToAction("Edit", routeValues);
                    }
                }
                Result.AddErrorRange(childResult.ToErrorArray());
                AddResult(Result);
            }

            model.CurrentVersion = rateTableBo.CurrentVersion;
            model.CurrentVersionObjectId = rateTableBo.CurrentVersionObjectId;
            model.CurrentVersionObject = rateTableBo.CurrentVersionObject;
            model.VersionObjects = rateTableBo.VersionObjects;
            model.TreatyPricingRateTableGroupBo = rateTableBo.TreatyPricingRateTableGroupBo;
            model.BenefitBo = rateTableBo.BenefitBo;
            model.WorkflowId = workflowId;
            LoadPage(model, versionId, isCalledFromWorkflow, isQuotationWorkflow);
            return View(model);
        }

        public void LoadPage(TreatyPricingRateTableViewModel model, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false)
        {
            AuthUserName();
            DropDownStatus();
            DropDownVersion(model);
            DropDownUser(UserBo.StatusActive, model.PersonInChargeId);
            DropDownAgeBasis();
            DropDownRateGuarantee();

            ViewBag.IsCalledFromWorkflow = isCalledFromWorkflow;
            ViewBag.IsHideSideBar = isCalledFromWorkflow;
            ViewBag.IsQuotationWorkflow = isQuotationWorkflow;
            ViewBag.VersionId = versionId;

            // Remark & Document
            string downloadDocumentUrl = Url.Action("Download", "Document");
            GetRemarkDocument(model.ModuleId, model.TreatyPricingRateTableGroupBo.TreatyPricingCedantId, downloadDocumentUrl, ModuleBo.ModuleController.TreatyPricingRateTable.ToString(), model.Id);
            GetRemarkSubjects();

            GetObjectVersionChangelog(ModuleBo.ModuleController.TreatyPricingRateTable.ToString(), model.Id, model);
            DropDownWorkflowObjectTypes();
            DropDownWorkflowStatus();

            var entity = new TreatyPricingRateTable();
            var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Name");
            ViewBag.NameMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

            maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Description");
            ViewBag.DescriptionMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

            if (model.Id == 0)
            {
                // Create
            }
            else
            {
                //Edit
                DropDownBenefit(BenefitBo.StatusActive, model.BenefitId);

                if (model.BenefitBo.Status == BenefitBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.BenefitStatusInactive);
                }

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
            for (int i = 1; i <= TreatyPricingRateTableBo.StatusMax; i++)
            {
                items.Add(new SelectListItem { Text = TreatyPricingRateTableBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownStatuses = items;
            return items;
        }

        public void GetProduct(int id)
        {
            var productBos = TreatyPricingProductService.GetRateTableProduct(id);
            ViewBag.Products = productBos;
        }

        public ActionResult CreateVersion(TreatyPricingRateTableBo bo, bool duplicatePreviousVersion = false)
        {
            bo.SetVersionObjects(TreatyPricingRateTableVersionService.GetByTreatyPricingRateTableId(bo.Id));
            TreatyPricingRateTableVersionBo nextVersionBo;

            TreatyPricingRateTableVersionBo previousVersionBo = (TreatyPricingRateTableVersionBo)bo.CurrentVersionObject;
            nextVersionBo = new TreatyPricingRateTableVersionBo(previousVersionBo)
            {
                Id = 0,
            };

            ObjectVersionChangelog changelog = null;
            nextVersionBo.Version = bo.EditableVersion + 1;
            nextVersionBo.PersonInChargeId = AuthUserId;
            nextVersionBo.CreatedById = AuthUserId;
            nextVersionBo.UpdatedById = AuthUserId;

            TrailObject trail = GetNewTrailObject();
            Result = TreatyPricingRateTableVersionService.Create(ref nextVersionBo, ref trail);
            if (Result.Valid)
            {
                // Copy Date From another Tables
                TreatyPricingRateTableDetailService.CopyBulk(nextVersionBo.Id, previousVersionBo.Id, AuthUserId);
                TreatyPricingRateTableRateService.CopyBulk(nextVersionBo.Id, previousVersionBo.Id, AuthUserId);
                TreatyPricingRateTableOriginalRateService.CopyBulk(nextVersionBo.Id, previousVersionBo.Id, AuthUserId);

                var userTrail = CreateTrail(
                    nextVersionBo.Id,
                    "Create Treaty Pricing Rate Table Version"
                );

                changelog = new ObjectVersionChangelog(nextVersionBo, userTrail);
                changelog.FormatBetweenVersionTrail(previousVersionBo);
            }

            bo.AddVersionObject(nextVersionBo);
            var versions = DropDownVersion(bo);

            return Json(new { bo, versions, changelog });
        }

        //[HttpPost]
        //public JsonResult RefreshData(int id)
        //{
        //    IList<TreatyPricingRateTableRateBo> treatyPricingRateTableRateBos = TreatyPricingRateTableRateService.GetByTreatyPricingRateTableVersionId(id);

        //    return Json(new { 
        //        TreatyPricingRateTableRateBos = treatyPricingRateTableRateBos,
        //    });
        //}
    }
}