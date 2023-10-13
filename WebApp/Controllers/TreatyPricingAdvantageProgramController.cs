using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using Services;
using Services.Identity;
using Services.TreatyPricing;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
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
    public class TreatyPricingAdvantageProgramController : BaseController
    {
        public const string Controller = "TreatyPricingAdvantageProgram";

        // GET: TreatyPricingAdvantageProgram
        public ActionResult Index()
        {
            return View();
        }

        // POST: TreatyPricingAdvantageProgram/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(TreatyPricingAdvantageProgramBo advantageProgramBo)
        {
            List<string> errors = new List<string>();

            if (!CheckPower(Controller, "C"))
            {
                errors.Add(string.Format(MessageBag.AccessDeniedWithActionName, "Add", "Advantage Program"));
                return Json(new { errors });
            }

            try
            {
                TreatyPricingAdvantageProgramVersionBo versionBo = new TreatyPricingAdvantageProgramVersionBo();
                List<TreatyPricingAdvantageProgramVersionBenefitBo> benefitBos = new List<TreatyPricingAdvantageProgramVersionBenefitBo>();
                if (!advantageProgramBo.DuplicateFromList && string.IsNullOrEmpty(advantageProgramBo.Name))
                {
                    errors.Add("Advantage Program Name is required");
                }

                if (advantageProgramBo.IsDuplicateExisting)
                {
                    if (!advantageProgramBo.DuplicateTreatyPricingAdvantageProgramId.HasValue)
                    {
                        errors.Add("Advantage Program is required");
                    }

                    if (!advantageProgramBo.DuplicateFromList && !advantageProgramBo.DuplicateTreatyPricingAdvantageProgramVersionId.HasValue)
                    {
                        errors.Add("Version is required");
                    }

                    if (errors.IsNullOrEmpty())
                    {
                        TreatyPricingAdvantageProgramBo duplicate = TreatyPricingAdvantageProgramService.Find(advantageProgramBo.DuplicateTreatyPricingAdvantageProgramId);
                        if (duplicate == null)
                        {
                            errors.Add("Advantage Program not found");
                        }
                        else
                        {
                            TreatyPricingAdvantageProgramVersionBo duplicateVersion = null;
                            if (advantageProgramBo.DuplicateTreatyPricingAdvantageProgramVersionId.HasValue)
                            {
                                duplicateVersion = duplicate.TreatyPricingAdvantageProgramVersionBos.Where(q => q.Id == advantageProgramBo.DuplicateTreatyPricingAdvantageProgramVersionId.Value).FirstOrDefault();
                            }
                            else
                            {
                                duplicateVersion = duplicate.TreatyPricingAdvantageProgramVersionBos.OrderByDescending(q => q.Version).First();
                            }

                            if (duplicateVersion == null)
                            {
                                errors.Add("Advantage Program Version not found");
                            }
                            else
                            {
                                versionBo = new TreatyPricingAdvantageProgramVersionBo(duplicateVersion)
                                {
                                    Id = 0
                                };
                                benefitBos = TreatyPricingAdvantageProgramVersionBenefitService.GetByTreatyPricingAdvantageProgramVersionId(duplicateVersion.Id).ToList();
                            }
                        }
                    }
                }

                if (errors.IsNullOrEmpty())
                {
                    advantageProgramBo.Status = TreatyPricingAdvantageProgramBo.StatusActive;
                    advantageProgramBo.Code = TreatyPricingAdvantageProgramService.GetNextObjectId(advantageProgramBo.TreatyPricingCedantId);
                    advantageProgramBo.CreatedById = AuthUserId;
                    advantageProgramBo.UpdatedById = AuthUserId;

                    TrailObject trail = GetNewTrailObject();
                    Result = TreatyPricingAdvantageProgramService.Create(ref advantageProgramBo, ref trail);
                    if (Result.Valid)
                    {
                        versionBo.TreatyPricingAdvantageProgramId = advantageProgramBo.Id;
                        versionBo.Version = 1;
                        versionBo.PersonInChargeId = AuthUserId;
                        versionBo.CreatedById = AuthUserId;
                        versionBo.UpdatedById = AuthUserId;

                        TreatyPricingAdvantageProgramVersionService.Create(ref versionBo, ref trail);

                        foreach (var advantageProgramVersionBenefitBo in benefitBos)
                        {
                            var bo = advantageProgramVersionBenefitBo;
                            bo.Id = 0;
                            bo.TreatyPricingAdvantageProgramVersionId = versionBo.Id;
                            bo.CreatedById = AuthUserId;
                            bo.UpdatedById = AuthUserId;

                            TreatyPricingAdvantageProgramVersionBenefitService.Create(ref bo, ref trail);
                        }

                        CreateTrail(
                            advantageProgramBo.Id,
                            "Create Treaty Pricing Advantage Program"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(string.Format("Error occurred. Error details: ", ex.Message));
            }

            var advantageProgramBos = TreatyPricingAdvantageProgramService.GetByTreatyPricingCedantId(advantageProgramBo.TreatyPricingCedantId);

            return Json(new { errors, advantageProgramBos });
        }

        // GET: TreatyPricingAdvantageProgram/Edit/5
        public ActionResult Edit(int id, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false, int workflowId = 0, bool isEditMode = false)
        {
            if (!CheckObjectLockReadOnly(Controller, id, isEditMode))
                return RedirectDashboard();

            var bo = TreatyPricingAdvantageProgramService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var model = new TreatyPricingAdvantageProgramViewModel(bo);
            ViewBag.VersionBenefits = TreatyPricingAdvantageProgramVersionBenefitService.GetByTreatyPricingAdvantageProgramVersionId(model.CurrentVersionObjectId);
            model.WorkflowId = workflowId;
            LoadPage(model, null, versionId, isCalledFromWorkflow, isQuotationWorkflow);
            return View(model);
        }

        // POST: TreatyPricingAdvantageProgram/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, TreatyPricingAdvantageProgramViewModel model, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false, int workflowId = 0)
        {
            var dbBo = TreatyPricingAdvantageProgramService.Find(id);
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

            TreatyPricingAdvantageProgramVersionBo updatedVersionBo = null;
            dbBo.SetVersionObjects(dbBo.TreatyPricingAdvantageProgramVersionBos);
            if (dbBo.EditableVersion == model.CurrentVersion)
            {
                updatedVersionBo = model.GetVersionBo((TreatyPricingAdvantageProgramVersionBo)dbBo.CurrentVersionObject);
                dbBo.AddVersionObject(updatedVersionBo);
            }


            Result childResult = new Result();
            List<TreatyPricingAdvantageProgramVersionBenefitBo> versionBenefitBo = model.GetBenefits(form, ref childResult, dbBo.CurrentVersionObjectId);
            if (!childResult.Valid)
            {
                AddResult(childResult);
                model.CurrentVersion = dbBo.CurrentVersion;
                model.CurrentVersionObject = dbBo.CurrentVersionObject;
                model.VersionObjects = dbBo.VersionObjects;
                ViewBag.VersionBenefits = versionBenefitBo;
                LoadPage(model, versionBenefitBo);
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();

                Result = TreatyPricingAdvantageProgramService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    if (updatedVersionBo != null)
                    {
                        updatedVersionBo.UpdatedById = AuthUserId;
                        TreatyPricingAdvantageProgramVersionService.Update(ref updatedVersionBo, ref trail);
                    }

                    TreatyPricingAdvantageProgramVersionBenefitService.DeleteAllByTreatyPricingAdvantageProgramVersionId(dbBo.CurrentVersionObjectId);
                    model.ProcessBenefits(versionBenefitBo, AuthUserId, ref trail, dbBo.CurrentVersionObjectId);

                    CreateTrail(
                        bo.Id,
                        "Update Treaty Pricing Advantage Program"
                    );

                    SetUpdateSuccessMessage("Advantage Program", false);

                    ObjectLockController.DeleteObjectLock(Controller, id, AuthUserId);

                    return RedirectToAction("Edit", routeValues);
                }
                AddResult(Result);
            }

            model.CurrentVersion = dbBo.CurrentVersion;
            model.CurrentVersionObject = dbBo.CurrentVersionObject;
            model.VersionObjects = dbBo.VersionObjects;
            ViewBag.VersionBenefits = TreatyPricingAdvantageProgramVersionBenefitService.GetByTreatyPricingAdvantageProgramVersionId(dbBo.CurrentVersionObjectId);
            model.WorkflowId = workflowId;
            LoadPage(model, null, versionId, isCalledFromWorkflow, isQuotationWorkflow);
            return View(model);
        }

        public void LoadPage(TreatyPricingAdvantageProgramViewModel model, List<TreatyPricingAdvantageProgramVersionBenefitBo> benefitBos = null, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false)
        {
            AuthUserName();
            DropDownStatus();
            DropDownVersion(model);
            DropDownUser(UserBo.StatusActive, model.PersonInChargeId);

            var entity = new TreatyPricingAdvantageProgram();
            var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Description");
            ViewBag.DescriptionMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;
            ViewBag.IsCalledFromWorkflow = isCalledFromWorkflow;
            ViewBag.IsHideSideBar = isCalledFromWorkflow;
            ViewBag.IsQuotationWorkflow = isQuotationWorkflow;
            ViewBag.VersionId = versionId;

            GetBenefits();
            GetVersionDetails(model.CurrentVersionObjectId, benefitBos);
            GetProduct(model.Id);

            if (versionId > 0)
            {
                model.SetCurrentVersionObject(versionId);
            }

            // Remark & Document
            string downloadDocumentUrl = Url.Action("Download", "Document");
            GetRemarkDocument(model.ModuleId, model.TreatyPricingCedantId, downloadDocumentUrl, ModuleBo.ModuleController.TreatyPricingAdvantageProgram.ToString(), model.Id);
            GetRemarkSubjects();

            GetObjectVersionChangelog(ModuleBo.ModuleController.TreatyPricingAdvantageProgram.ToString(), model.Id, model);
            DropDownWorkflowObjectTypes();
            DropDownWorkflowStatus();

            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingAdvantageProgramBo.StatusMax))
            {
                items.Add(new SelectListItem { Text = TreatyPricingAdvantageProgramBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.StatusItems = items;
            return items;
        }

        public Dictionary<string, string> GetListBenefitCodes()
        {
            var items = new Dictionary<string, string>();
            foreach (BenefitBo bo in BenefitService.Get())
            {
                items.Add(bo.Id.ToString(), bo.Code);
            }
            ViewBag.BenefitItems = items;
            return items;
        }

        public void GetProduct(int id)
        {
            var productBos = TreatyPricingProductService.GetAdvantageProgramProduct(id);
            ViewBag.Products = productBos;
        }

        public ActionResult CreateVersion(TreatyPricingAdvantageProgramBo bo, bool duplicatePreviousVersion = false)
        {
            bo.SetVersionObjects(TreatyPricingAdvantageProgramVersionService.GetByTreatyPricingAdvantageProgramId(bo.Id));
            TreatyPricingAdvantageProgramVersionBo nextVersionBo;
            TreatyPricingAdvantageProgramVersionBo previousVersionBo = (TreatyPricingAdvantageProgramVersionBo)bo.CurrentVersionObject;

            if (duplicatePreviousVersion)
            {
                nextVersionBo = new TreatyPricingAdvantageProgramVersionBo(previousVersionBo)
                {
                    Id = 0,
                };
            }
            else
            {
                nextVersionBo = new TreatyPricingAdvantageProgramVersionBo()
                {
                    TreatyPricingAdvantageProgramId = bo.Id,
                };
            }

            nextVersionBo.Version = bo.EditableVersion + 1;
            nextVersionBo.PersonInChargeId = AuthUserId;
            nextVersionBo.CreatedById = AuthUserId;
            nextVersionBo.UpdatedById = AuthUserId;

            ObjectVersionChangelog changelog = null;

            TrailObject trail = GetNewTrailObject();
            Result = TreatyPricingAdvantageProgramVersionService.Create(ref nextVersionBo, ref trail);
            if (Result.Valid)
            {
                TreatyPricingAdvantageProgramVersionBenefitService.Save(nextVersionBo.Id, nextVersionBo.TreatyPricingAdvantageProgramVersionBenefit, AuthUserId, ref trail, true);
                nextVersionBo.TreatyPricingAdvantageProgramVersionBenefit = TreatyPricingAdvantageProgramVersionBenefitService.GetJsonByVersionId(nextVersionBo.Id);

                UserTrailBo userTrail = CreateTrail(
                    nextVersionBo.Id,
                    "Create Treaty Pricing Advantage Program Version"
                );

                changelog = new ObjectVersionChangelog(nextVersionBo, userTrail);
                changelog.FormatBetweenVersionTrail(previousVersionBo);
            }

            bo.AddVersionObject(nextVersionBo);
            var versions = DropDownVersion(bo);

            return Json(new { bo, versions });
        }

        [HttpPost]
        public ActionResult GetVersionDetails(int treatyPricingAdvantageProgramVersionId, List<TreatyPricingAdvantageProgramVersionBenefitBo> verBenefitBos)
        {
            var benefitBos = TreatyPricingAdvantageProgramVersionBenefitService.GetByTreatyPricingAdvantageProgramVersionId(treatyPricingAdvantageProgramVersionId);
            if (verBenefitBos != null && verBenefitBos.Count > 0)
            {
                benefitBos = verBenefitBos;
            }
            ViewBag.AdvantageProgramBenefit = benefitBos;
            return Json(new { benefitBos });
        }

        [HttpPost]
        public JsonResult Search(int cedantId, string productName)
        {
            var advantageProgram = TreatyPricingAdvantageProgramService.GetCodeByTreatyPricingCedantIdProductName(cedantId, productName);
            return Json(new { AdvantageProgram = advantageProgram });
        }

        [HttpPost]
        public JsonResult GetByTreatyPricingCedant(int? id)
        {
            IList<TreatyPricingAdvantageProgramBo> bos = new List<TreatyPricingAdvantageProgramBo>();
            if (id.HasValue)
            {
                bos = TreatyPricingAdvantageProgramService.GetByTreatyPricingCedantId(id.Value);
            }
            return Json(new { bos });
        }

        [HttpPost]
        public JsonResult GetByTreatyPricingAdvantageProgram(int? id)
        {
            IList<TreatyPricingAdvantageProgramVersionBo> versionBos = new List<TreatyPricingAdvantageProgramVersionBo>();
            if (id.HasValue)
            {
                versionBos = TreatyPricingAdvantageProgramVersionService.GetByTreatyPricingAdvantageProgramId(id.Value).ToList();
            }
            return Json(new { versionBos });
        }
    }
}