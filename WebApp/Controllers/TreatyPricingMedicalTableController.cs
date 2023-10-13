using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using ConsoleApp.Commands;
using Services.Identity;
using Services.TreatyPricing;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class TreatyPricingMedicalTableController : BaseController
    {
        public const string Controller = "TreatyPricingMedicalTable";

        [HttpPost]
        public JsonResult GetByTreatyPricingCedant(int? id)
        {
            IList<TreatyPricingMedicalTableBo> bos = new List<TreatyPricingMedicalTableBo>();
            if (id.HasValue)
            {
                bos = TreatyPricingMedicalTableService.GetByTreatyPricingCedantId(id.Value);
            }
            return Json(new { bos });
        }

        [HttpPost]
        public JsonResult GetByTreatyPricingMedicalTable(int? id)
        {
            IList<TreatyPricingMedicalTableVersionBo> versionBos = new List<TreatyPricingMedicalTableVersionBo>();
            if (id.HasValue)
            {
                versionBos = TreatyPricingMedicalTableVersionService.GetByTreatyPricingMedicalTableId(id.Value).ToList();
            }
            return Json(new { versionBos });
        }

        //[Auth(Controller = Controller, Power = "C")]
        [HttpPost]
        public JsonResult Add(TreatyPricingMedicalTableBo medicalTableBo)
        {
            List<string> errors = new List<string>();

            if (!CheckPower(Controller, "C"))
            {
                errors.Add(string.Format(MessageBag.AccessDeniedWithActionName, "Add", "Medical Table"));
                return Json(new { errors });
            }

            try
            {
                TreatyPricingMedicalTableVersionBo versionBo = new TreatyPricingMedicalTableVersionBo();
                if (!medicalTableBo.DuplicateFromList && string.IsNullOrEmpty(medicalTableBo.Name))
                {
                    errors.Add("Medical Table Name is required");
                }

                if (medicalTableBo.IsDuplicateExisting)
                {
                    if (!medicalTableBo.DuplicateTreatyPricingMedicalTableId.HasValue)
                    {
                        errors.Add("Medical Table is required");
                    }

                    if (!medicalTableBo.DuplicateFromList && !medicalTableBo.DuplicateTreatyPricingMedicalTableVersionId.HasValue)
                    {
                        errors.Add("Version is required");
                    }

                    if (errors.IsNullOrEmpty())
                    {
                        TreatyPricingMedicalTableBo duplicate = TreatyPricingMedicalTableService.Find(medicalTableBo.DuplicateTreatyPricingMedicalTableId);
                        if (duplicate == null)
                        {
                            errors.Add("Medical Table not found");
                        }
                        else
                        {
                            medicalTableBo.BenefitCode = duplicate.BenefitCode;
                            medicalTableBo.DistributionChannel = duplicate.DistributionChannel;
                            medicalTableBo.CurrencyCode = duplicate.CurrencyCode;
                            medicalTableBo.AgeDefinitionPickListDetailId = duplicate.AgeDefinitionPickListDetailId;

                            TreatyPricingMedicalTableVersionBo duplicateVersion = null;
                            if (medicalTableBo.DuplicateTreatyPricingMedicalTableVersionId.HasValue)
                            {
                                duplicateVersion = duplicate.TreatyPricingMedicalTableVersionBos.Where(q => q.Id == medicalTableBo.DuplicateTreatyPricingMedicalTableVersionId.Value).FirstOrDefault();
                            }
                            else
                            {
                                duplicateVersion = duplicate.TreatyPricingMedicalTableVersionBos.OrderByDescending(q => q.Version).First();
                            }

                            if (duplicateVersion == null)
                            {
                                errors.Add("Medical Table Version not found");
                            }
                            else
                            {
                                versionBo = new TreatyPricingMedicalTableVersionBo(duplicateVersion)
                                {
                                    Id = 0
                                };
                            }
                        }
                    }
                }

                if (errors.IsNullOrEmpty())
                {
                    medicalTableBo.MedicalTableId = TreatyPricingMedicalTableService.GetNextObjectId(medicalTableBo.TreatyPricingCedantId);
                    medicalTableBo.Status = TreatyPricingMedicalTableBo.StatusActive;
                    medicalTableBo.CreatedById = AuthUserId;
                    medicalTableBo.UpdatedById = AuthUserId;

                    TrailObject trail = GetNewTrailObject();

                    Result = TreatyPricingMedicalTableService.Create(ref medicalTableBo, ref trail);
                    if (Result.Valid)
                    {
                        versionBo.TreatyPricingMedicalTableId = medicalTableBo.Id;
                        versionBo.Version = 1;
                        versionBo.PersonInChargeId = AuthUserId;
                        versionBo.CreatedById = AuthUserId;
                        versionBo.UpdatedById = AuthUserId;
                        TreatyPricingMedicalTableVersionService.Create(ref versionBo, ref trail);

                        CreateTrail(
                            medicalTableBo.Id,
                            "Create Treaty Pricing Medical Table"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(string.Format("Error occurred. Error details: {0}", ex.Message));
            }

            var medicalTableBos = TreatyPricingMedicalTableService.GetByTreatyPricingCedantId(medicalTableBo.TreatyPricingCedantId);

            return Json(new { errors, medicalTableBos });
        }

        // GET: TreatyPricingMedicalTable/Edit/5
        public ActionResult Edit(int id, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false, int workflowId = 0, bool isEditMode = false)
        {
            if (!CheckObjectLockReadOnly(Controller, id, isEditMode))
                return RedirectDashboard();

            var MedicalTableBo = TreatyPricingMedicalTableService.Find(id);
            if (MedicalTableBo == null)
            {
                return RedirectToAction("Index");
            }

            var model = new TreatyPricingMedicalTableViewModel(MedicalTableBo);
            model.WorkflowId = workflowId;
            LoadPage(model, versionId, isCalledFromWorkflow, isQuotationWorkflow);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, TreatyPricingMedicalTableViewModel model, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false, int workflowId = 0)
        {
            TreatyPricingMedicalTableBo MedicalTableBo = TreatyPricingMedicalTableService.Find(id);
            if (MedicalTableBo == null)
            {
                return RedirectToAction("Edit", "TreatyPricingCedant", new { id = model.TreatyPricingCedantId });
            }

            object routeValues = new { id = MedicalTableBo.Id };
            if (isCalledFromWorkflow)
            {
                routeValues = new { id = MedicalTableBo.Id, versionId, isCalledFromWorkflow, isQuotationWorkflow, workflowId };
            }

            if (!CheckObjectLock(Controller, id))
                return RedirectToAction("Edit", routeValues);

            TreatyPricingMedicalTableVersionBo updatedVersionBo = null;
            MedicalTableBo.SetVersionObjects(MedicalTableBo.TreatyPricingMedicalTableVersionBos);
            if (MedicalTableBo.EditableVersion == model.CurrentVersion)
            {
                updatedVersionBo = model.GetVersionBo((TreatyPricingMedicalTableVersionBo)MedicalTableBo.CurrentVersionObject);
                MedicalTableBo.AddVersionObject(updatedVersionBo);
            }

            if (ModelState.IsValid)
            {
                MedicalTableBo = model.FormBo(MedicalTableBo);
                MedicalTableBo.UpdatedById = AuthUserId;

                TrailObject trail = GetNewTrailObject();
                Result = TreatyPricingMedicalTableService.Update(ref MedicalTableBo, ref trail);
                if (Result.Valid)
                {
                    if (updatedVersionBo != null)
                    {
                        updatedVersionBo.UpdatedById = AuthUserId;
                        TreatyPricingMedicalTableVersionService.Update(ref updatedVersionBo, ref trail);
                    }

                    CreateTrail(
                        MedicalTableBo.Id,
                        "Update Medical Table"
                    );

                    SetUpdateSuccessMessage("Medical Table", false);

                    ObjectLockController.DeleteObjectLock(Controller, id, AuthUserId);

                    return RedirectToAction("Edit", routeValues);
                }
                AddResult(Result);
            }

            model.CurrentVersion = MedicalTableBo.CurrentVersion;
            model.CurrentVersionObject = MedicalTableBo.CurrentVersionObject;
            model.VersionObjects = MedicalTableBo.VersionObjects;
            model.TreatyPricingCedantBo = MedicalTableBo.TreatyPricingCedantBo;
            model.WorkflowId = workflowId;
            LoadPage(model, versionId, isCalledFromWorkflow, isQuotationWorkflow);
            return View(model);
        }

        public void LoadPage(TreatyPricingMedicalTableViewModel model, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false)
        {
            DropDownCurrencyCode(true);
            DropDownStatus();
            DropDownAgeDefinition();
            DropDownDistributionTier();
            DropDownVersion(model);
            DropDownUser(UserBo.StatusActive, model.PersonInChargeId);
            GetBenefitCodes();
            GetDistributionChannels();
            ViewBag.DistributionChannelCodes = GetPickListDetailCodes(PickListBo.DistributionChannel);

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
            GetRemarkDocument(model.ModuleId, model.TreatyPricingCedantId, downloadDocumentUrl, ModuleBo.ModuleController.TreatyPricingMedicalTable.ToString(), model.Id);
            GetRemarkSubjects();

            GetObjectVersionChangelog(ModuleBo.ModuleController.TreatyPricingMedicalTable.ToString(), model.Id, model);
            DropDownWorkflowObjectTypes();
            DropDownWorkflowStatus();

            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingMedicalTableBo.StatusMax))
            {
                items.Add(new SelectListItem { Text = TreatyPricingMedicalTableBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.StatusItems = items;
            return items;
        }

        public ActionResult CreateVersion(TreatyPricingMedicalTableBo bo, bool duplicatePreviousVersion = false)
        {
            bo.SetVersionObjects(TreatyPricingMedicalTableVersionService.GetByTreatyPricingMedicalTableId(bo.Id));
            TreatyPricingMedicalTableVersionBo nextVersionBo;
            TreatyPricingMedicalTableVersionBo previousVersionBo = (TreatyPricingMedicalTableVersionBo)bo.CurrentVersionObject;

            if (duplicatePreviousVersion)
            {
                nextVersionBo = new TreatyPricingMedicalTableVersionBo(previousVersionBo)
                {
                    Id = 0,
                };
            }
            else
            {
                nextVersionBo = new TreatyPricingMedicalTableVersionBo()
                {
                    TreatyPricingMedicalTableId = bo.Id,
                };
            }

            nextVersionBo.Version = bo.EditableVersion + 1;
            nextVersionBo.PersonInChargeId = AuthUserId;
            nextVersionBo.CreatedById = AuthUserId;
            nextVersionBo.UpdatedById = AuthUserId;

            ObjectVersionChangelog changelog = null;

            TrailObject trail = GetNewTrailObject();
            Result = TreatyPricingMedicalTableVersionService.Create(ref nextVersionBo, ref trail);
            if (Result.Valid)
            {
                UserTrailBo userTrail = CreateTrail(
                    nextVersionBo.Id,
                    "Create Treaty Pricing Medical Table Version"
                );

                changelog = new ObjectVersionChangelog(nextVersionBo, userTrail);
                changelog.FormatBetweenVersionTrail(previousVersionBo);
            }

            bo.AddVersionObject(nextVersionBo);
            var versions = DropDownVersion(bo);

            return Json(new { bo, versions, changelog });
        }

        [HttpPost]
        public ActionResult GetVersionDetails(int medicalTableId, int medicalTableVersion)
        {
            //(int treatyPricingMedicalTableVersionId)
            int treatyPricingMedicalTableVersionId = TreatyPricingMedicalTableVersionService.GetVersionId(medicalTableId, medicalTableVersion);

            var medicalTableBos = TreatyPricingMedicalTableVersionDetailService.GetByTreatyPricingMedicalTableVersionId(treatyPricingMedicalTableVersionId);
            var fileUploadBos = TreatyPricingMedicalTableVersionFileService.GetByTreatyPricingMedicalTableVersionId(treatyPricingMedicalTableVersionId);

            return Json(new { medicalTableBos, fileUploadBos });
        }

        [HttpPost]
        public ActionResult GetUploadedData(int treatyPricingMedicalTableVersionDetailId)
        {
            var legendBos = TreatyPricingMedicalTableUploadLegendService.GetByTreatyPricingMedicalTableVersionDetailId(treatyPricingMedicalTableVersionDetailId);
            var columnBos = TreatyPricingMedicalTableUploadColumnService.GetByTreatyPricingMedicalTableVersionDetailId(treatyPricingMedicalTableVersionDetailId);
            var rowBos = TreatyPricingMedicalTableUploadRowService.GetByTreatyPricingMedicalTableVersionDetailId(treatyPricingMedicalTableVersionDetailId);

            List<TreatyPricingMedicalTableUploadCellBo> cellBos = new List<TreatyPricingMedicalTableUploadCellBo>();

            foreach (TreatyPricingMedicalTableUploadColumnBo bo in columnBos)
            {
                var cellBosByColumn = TreatyPricingMedicalTableUploadCellService.GetByTreatyPricingMedicalTableUploadColumnId(bo.Id);

                foreach (TreatyPricingMedicalTableUploadCellBo cellBo in cellBosByColumn)
                {
                    cellBos.Add(cellBo);
                }
            }

            return Json(new { legendBos, columnBos, rowBos, cellBos });
        }

        [HttpPost]
        public ActionResult UploadMedicalTable()
        {
            List<string> errors = new List<string>();
            TreatyPricingMedicalTableVersionFileBo medicalTableVersionFileBo = new TreatyPricingMedicalTableVersionFileBo();

            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    //string treatyPricingMedicalTableVersionId = Request["treatyPricingMedicalTableVersionId"];
                    int medicalTableId = int.Parse(Request["medicalTableId"]);
                    int medicalTableVersion = int.Parse(Request["medicalTableVersion"]);
                    int treatyPricingMedicalTableVersionId = TreatyPricingMedicalTableVersionService.GetVersionId(medicalTableId, medicalTableVersion);

                    int uploadDistributionTier = int.Parse(Request["uploadDistributionTier"]);
                    string uploadDescription = Request["uploadDescription"];

                    HttpPostedFileBase file = files[0];
                    string fileName = Path.GetFileName(file.FileName);

                    Result = TreatyPricingMedicalTableVersionFileService.Result();

                    int fileSize = file.ContentLength / 1024 / 1024 / 1024;
                    if (fileSize >= 2)
                        Result.AddError("Uploaded file size exceeded 2 GB");

                    if (Result.Valid)
                    {
                        medicalTableVersionFileBo = new TreatyPricingMedicalTableVersionFileBo
                        {
                            //TreatyPricingMedicalTableVersionId = int.Parse(treatyPricingMedicalTableVersionId),
                            TreatyPricingMedicalTableVersionId = treatyPricingMedicalTableVersionId,
                            DistributionTierPickListDetailId = uploadDistributionTier,
                            Description = uploadDescription,
                            FileName = fileName,
                            Status = TreatyPricingMedicalTableVersionFileBo.StatusSubmitForProcessing,
                            CreatedById = AuthUserId,
                            UpdatedById = AuthUserId,
                        };

                        medicalTableVersionFileBo.FormatHashFileName();
                        string path = medicalTableVersionFileBo.GetLocalPath();
                        Util.MakeDir(path);

                        TrailObject trail = GetNewTrailObject();
                        Result = TreatyPricingMedicalTableVersionFileService.Create(ref medicalTableVersionFileBo, ref trail);
                        if (Result.Valid)
                        {
                            CreateTrail(
                                medicalTableVersionFileBo.Id,
                                "Create Treaty Pricing Medical Table Version File"
                            );
                            file.SaveAs(path);

                            //Start processing Excel file
                            var process = new ProcessMedicalTable()
                            {
                                FilePath = path,
                                MedicalTableFileBo = medicalTableVersionFileBo,
                                UserId = AuthUserId,
                            };
                            process.Process();
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

            return Json(new { errors, medicalTableVersionFileBo });
        }

        [HttpPost]
        public JsonResult Search(int cedantId, string productName)
        {
            var medicalTable = TreatyPricingMedicalTableService.GetCodeByTreatyPricingCedantIdProductName(cedantId, productName);
            return Json(new { MedicalTable = medicalTable });
        }

        public void GetProduct(int id)
        {
            var productBos = TreatyPricingProductService.GetMedicalTableProduct(id);
            ViewBag.Products = productBos;
        }

        public ActionResult DownloadTemplate()
        {
            string template = "TreatyPricingMedicalTableTemplate.xlsx";
            string filename = string.Format("TreatyPricingMedicalTableTemplate").AppendDateTimeFileName(".xlsx");
            var templateFilepath = Util.GetWebAppDocumentFilePath(template);

            DownloadFile(templateFilepath, filename);
            return null;
        }

        public void DownloadFile(string filePath, string fileName)
        {
            try
            {
                Response.ClearContent();
                Response.Clear();
                //Response.Buffer = true;                                                                                                                   
                //Response.BufferOutput = false;
                Response.ContentType = MimeMapping.GetMimeMapping(fileName);
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                Response.TransmitFile(filePath);
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
    }
}