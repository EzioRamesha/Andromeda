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
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class TreatyPricingUwQuestionnaireController : BaseController
    {
        public const string Controller = "TreatyPricingUwQuestionnaire";

        // GET: TreatyPricingUwQuestionnaire
        public ActionResult Index()
        {
            return View();
        }

        // POST: TreatyPricingUwQuestionnaire/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(TreatyPricingUwQuestionnaireBo uwQuestionnaireBo)
        {
            List<string> errors = new List<string>();

            if (!CheckPower(Controller, "C"))
            {
                errors.Add(string.Format(MessageBag.AccessDeniedWithActionName, "Add", "Underwriting Questionnaire"));
                return Json(new { errors });
            }

            try
            {
                TreatyPricingUwQuestionnaireVersionBo versionBo = new TreatyPricingUwQuestionnaireVersionBo();
                if (!uwQuestionnaireBo.DuplicateFromList && string.IsNullOrEmpty(uwQuestionnaireBo.Name))
                {
                    errors.Add("Underwriting Questionnaire Name is required");
                }

                if (uwQuestionnaireBo.IsDuplicateExisting)
                {
                    if (!uwQuestionnaireBo.DuplicateTreatyPricingUwQuestionnaireId.HasValue)
                    {
                        errors.Add("Underwriting Questionnaire is required");
                    }

                    if (!uwQuestionnaireBo.DuplicateFromList && !uwQuestionnaireBo.DuplicateTreatyPricingUwQuestionnaireVersionId.HasValue)
                    {
                        errors.Add("Version is required");
                    }

                    if (errors.IsNullOrEmpty())
                    {
                        TreatyPricingUwQuestionnaireBo duplicate = TreatyPricingUwQuestionnaireService.Find(uwQuestionnaireBo.DuplicateTreatyPricingUwQuestionnaireId);
                        if (duplicate == null)
                        {
                            errors.Add("Underwriting Questionnaire not found");
                        }
                        else
                        {
                            uwQuestionnaireBo.BenefitCode = duplicate.BenefitCode;
                            uwQuestionnaireBo.DistributionChannel = duplicate.DistributionChannel;

                            TreatyPricingUwQuestionnaireVersionBo duplicateVersion = null;
                            if (uwQuestionnaireBo.DuplicateTreatyPricingUwQuestionnaireVersionId.HasValue)
                            {
                                duplicateVersion = duplicate.TreatyPricingUwQuestionnaireVersionBos.Where(q => q.Id == uwQuestionnaireBo.DuplicateTreatyPricingUwQuestionnaireVersionId.Value).FirstOrDefault();
                            }
                            else
                            {
                                duplicateVersion = duplicate.TreatyPricingUwQuestionnaireVersionBos.OrderByDescending(q => q.Version).First();
                            }

                            if (duplicateVersion == null)
                            {
                                errors.Add("Underwriting Questionnaire Version not found");
                            }
                            else
                            {
                                versionBo = new TreatyPricingUwQuestionnaireVersionBo(duplicateVersion)
                                {
                                    Id = 0
                                };
                            }
                        }
                    }
                }

                if (errors.IsNullOrEmpty())
                {
                    uwQuestionnaireBo.Status = TreatyPricingUwQuestionnaireBo.StatusActive;
                    uwQuestionnaireBo.Code = TreatyPricingUwQuestionnaireService.GetNextObjectId(uwQuestionnaireBo.TreatyPricingCedantId);
                    uwQuestionnaireBo.CreatedById = AuthUserId;
                    uwQuestionnaireBo.UpdatedById = AuthUserId;

                    TrailObject trail = GetNewTrailObject();
                    Result = TreatyPricingUwQuestionnaireService.Create(ref uwQuestionnaireBo, ref trail);
                    if (Result.Valid)
                    {
                        versionBo.TreatyPricingUwQuestionnaireId = uwQuestionnaireBo.Id;
                        versionBo.Version = 1;
                        versionBo.PersonInChargeId = AuthUserId;
                        versionBo.CreatedById = AuthUserId;
                        versionBo.UpdatedById = AuthUserId;
                        TreatyPricingUwQuestionnaireVersionService.Create(ref versionBo, ref trail);

                        CreateTrail(
                            uwQuestionnaireBo.Id,
                            "Create Underwriting Questionnaire"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(string.Format("Error occurred. Error details: ", ex.Message));
            }

            var uwQuestionnaireBos = TreatyPricingUwQuestionnaireService.GetByTreatyPricingCedantId(uwQuestionnaireBo.TreatyPricingCedantId);

            return Json(new { errors, uwQuestionnaireBos });
        }

        // GET: TreatyPricingUwQuestionnaire/Edit/5
        public ActionResult Edit(int id, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false, int workflowId = 0, bool isEditMode = false)
        {
            if (!CheckObjectLockReadOnly(Controller, id, isEditMode))
                return RedirectDashboard();

            TreatyPricingUwQuestionnaireBo bo = TreatyPricingUwQuestionnaireService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var model = new TreatyPricingUwQuestionnaireViewModel(bo);
            model.WorkflowId = workflowId;
            LoadPage(model, versionId, isCalledFromWorkflow, isQuotationWorkflow);
            return View(model);
        }

        // POST: TreatyPricingUwQuestionnaire/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, TreatyPricingUwQuestionnaireViewModel model, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false, int workflowId = 0)
        {
            var dbBo = TreatyPricingUwQuestionnaireService.Find(id);
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

            TreatyPricingUwQuestionnaireVersionBo updatedVersionBo = null;
            dbBo.SetVersionObjects(dbBo.TreatyPricingUwQuestionnaireVersionBos);
            if (dbBo.EditableVersion == model.CurrentVersion)
            {
                updatedVersionBo = model.GetVersionBo((TreatyPricingUwQuestionnaireVersionBo)dbBo.CurrentVersionObject);
                dbBo.AddVersionObject(updatedVersionBo);
            }

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();
                Result = TreatyPricingUwQuestionnaireService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    if (updatedVersionBo != null)
                    {
                        updatedVersionBo.UpdatedById = AuthUserId;
                        TreatyPricingUwQuestionnaireVersionService.Update(ref updatedVersionBo, ref trail);
                    }
                    CreateTrail(
                        bo.Id,
                        "Update Treaty Pricing Underwriting Questionnaire"
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
            model.WorkflowId = workflowId;
            LoadPage(model, versionId, isCalledFromWorkflow, isQuotationWorkflow);
            return View(model);
        }

        public void LoadPage(TreatyPricingUwQuestionnaireViewModel model, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false)
        {
            AuthUserName();
            DropDownVersion(model);
            DropDownUser(UserBo.StatusActive, model.PersonInChargeId);
            DropDownStatus();
            DropDownQuestionnaireType();
            GetBenefitCodes();
            GetDistributionChannels();
            GetProduct(model.Id);

            var entity = new TreatyPricingUwQuestionnaire();
            var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Description");
            ViewBag.DescriptionMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

            ViewBag.DistributionChannelCodes = GetPickListDetailCodes(PickListBo.DistributionChannel);
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
            GetRemarkDocument(model.ModuleId, model.TreatyPricingCedantId, downloadDocumentUrl, ModuleBo.ModuleController.TreatyPricingUwQuestionnaire.ToString(), model.Id);
            GetRemarkSubjects();

            GetObjectVersionChangelog(ModuleBo.ModuleController.TreatyPricingUwQuestionnaire.ToString(), model.Id, model);
            DropDownWorkflowObjectTypes();
            DropDownWorkflowStatus();

            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingUwQuestionnaireBo.StatusMax))
            {
                items.Add(new SelectListItem { Text = TreatyPricingUwQuestionnaireBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.StatusItems = items;
            return items;
        }

        public List<SelectListItem> DropDownQuestionnaireType()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingUwQuestionnaireVersionBo.TypeMax))
            {
                items.Add(new SelectListItem { Text = TreatyPricingUwQuestionnaireVersionBo.GetTypeName(i), Value = i.ToString() });
            }
            ViewBag.QuestionnaireTypeItems = items;
            return items;
        }

        public void GetProduct(int id)
        {
            var productBos = TreatyPricingProductService.GetUwQuestionnaireProduct(id);
            ViewBag.Products = productBos;
        }

        public ActionResult CreateVersion(TreatyPricingUwQuestionnaireBo bo, bool duplicatePreviousVersion = false)
        {
            bo.SetVersionObjects(TreatyPricingUwQuestionnaireVersionService.GetByTreatyPricingUwQuestionnaireId(bo.Id));
            TreatyPricingUwQuestionnaireVersionBo nextVersionBo;
            TreatyPricingUwQuestionnaireVersionBo previousVersionBo = (TreatyPricingUwQuestionnaireVersionBo)bo.CurrentVersionObject;

            if (duplicatePreviousVersion)
            {                
                nextVersionBo = new TreatyPricingUwQuestionnaireVersionBo(previousVersionBo)
                {
                    Id = 0,
                };
            }
            else
            {
                nextVersionBo = new TreatyPricingUwQuestionnaireVersionBo()
                {
                    TreatyPricingUwQuestionnaireId = bo.Id,
                };
            }

            nextVersionBo.Version = bo.EditableVersion + 1;
            nextVersionBo.PersonInChargeId = AuthUserId;
            nextVersionBo.CreatedById = AuthUserId;
            nextVersionBo.UpdatedById = AuthUserId;

            ObjectVersionChangelog changelog = null;

            TrailObject trail = GetNewTrailObject();
            Result = TreatyPricingUwQuestionnaireVersionService.Create(ref nextVersionBo, ref trail);
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

            return Json(new { bo, versions, changelog });
        }

        [HttpPost]
        public ActionResult GetVersionDetails(int uwQuestionnaireId, int uwQuestionnaireVersion)
        {
            int treatyPricingUwQuestionnaireVersionId = TreatyPricingUwQuestionnaireVersionService.GetVersionId(uwQuestionnaireId, uwQuestionnaireVersion);

            var questionnaireBos = TreatyPricingUwQuestionnaireVersionDetailService.GetByTreatyPricingUwQuestionnaireVersionId(treatyPricingUwQuestionnaireVersionId);
            var fileUploadBos = TreatyPricingUwQuestionnaireVersionFileService.GetByTreatyPricingUwQuestionnaireVersionId(treatyPricingUwQuestionnaireVersionId);

            return Json(new { questionnaireBos, fileUploadBos });
        }

        [HttpPost]
        public ActionResult UploadQuestionnaire()
        {
            List<string> errors = new List<string>();
            TreatyPricingUwQuestionnaireVersionFileBo questionnaireVersionFileBo = new TreatyPricingUwQuestionnaireVersionFileBo();

            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    int uwQuestionnaireId = int.Parse(Request["uwQuestionnaireId"]);
                    int uwQuestionnaireVersion = int.Parse(Request["uwQuestionnaireVersion"]);

                    HttpPostedFileBase file = files[0];
                    string fileName = Path.GetFileName(file.FileName);

                    Result = TreatyPricingUwQuestionnaireVersionFileService.Result();

                    int fileSize = file.ContentLength / 1024 / 1024 / 1024;
                    if (fileSize >= 2)
                        Result.AddError("Uploaded file's size exceeded 2 GB");

                    if (Result.Valid)
                    {
                        int treatyPricingUwQuestionnaireVersionId = TreatyPricingUwQuestionnaireVersionService.GetVersionId(uwQuestionnaireId, uwQuestionnaireVersion);

                        questionnaireVersionFileBo = new TreatyPricingUwQuestionnaireVersionFileBo
                        {
                            TreatyPricingUwQuestionnaireVersionId = treatyPricingUwQuestionnaireVersionId,
                            FileName = fileName,
                            Status = TreatyPricingUwQuestionnaireVersionFileBo.StatusSubmitForProcessing,
                            CreatedById = AuthUserId,
                            UpdatedById = AuthUserId,
                        };

                        questionnaireVersionFileBo.FormatHashFileName();
                        string path = questionnaireVersionFileBo.GetLocalPath();
                        Util.MakeDir(path);

                        TrailObject trail = GetNewTrailObject();
                        Result = TreatyPricingUwQuestionnaireVersionFileService.Create(ref questionnaireVersionFileBo, ref trail);
                        if (Result.Valid)
                        {
                            CreateTrail(
                                questionnaireVersionFileBo.Id,
                                "Create Treaty Pricing UW Questionnaire Version File"
                            );
                            file.SaveAs(path);
                        }
                    }
                    errors = Result.ToErrorArray().OfType<string>().ToList();
                }
                catch (Exception ex)
                {
                    errors.Add(string.Format("Error occurred. Error details: ", ex.Message));
                }
            }
            else
            {
                errors.Add("No files selected.");
            }

            return Json(new { errors, questionnaireVersionFileBo });
        }

        [HttpPost]
        public JsonResult Search(int cedantId, string productName)
        {
            var uwQuestionnaire = TreatyPricingUwQuestionnaireService.GetCodeByTreatyPricingCedantIdProductName(cedantId, productName);
            return Json(new { UWQuestionnaire = uwQuestionnaire });
        }

        [HttpPost]
        public JsonResult GetByTreatyPricingCedant(int? id)
        {
            IList<TreatyPricingUwQuestionnaireBo> bos = new List<TreatyPricingUwQuestionnaireBo>();
            if (id.HasValue)
            {
                bos = TreatyPricingUwQuestionnaireService.GetByTreatyPricingCedantId(id.Value);
            }
            return Json(new { bos });
        }

        [HttpPost]
        public JsonResult GetByTreatyPricingUwQuestionnaire(int? id)
        {
            IList<TreatyPricingUwQuestionnaireVersionBo> versionBos = new List<TreatyPricingUwQuestionnaireVersionBo>();
            if (id.HasValue)
            {
                versionBos = TreatyPricingUwQuestionnaireVersionService.GetByTreatyPricingUwQuestionnaireId(id.Value).ToList();
            }
            return Json(new { versionBos });
        }
    }
}