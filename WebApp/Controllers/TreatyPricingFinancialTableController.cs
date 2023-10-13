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
    public class TreatyPricingFinancialTableController : BaseController
    {
        public const string Controller = "TreatyPricingFinancialTable";

        [HttpPost]
        public JsonResult GetByTreatyPricingCedant(int? id)
        {
            IList<TreatyPricingFinancialTableBo> bos = new List<TreatyPricingFinancialTableBo>();
            if (id.HasValue)
            {
                bos = TreatyPricingFinancialTableService.GetByTreatyPricingCedantId(id.Value);
            }
            return Json(new { bos });
        }

        [HttpPost]
        public JsonResult GetByTreatyPricingFinancialTable(int? id)
        {
            IList<TreatyPricingFinancialTableVersionBo> versionBos = new List<TreatyPricingFinancialTableVersionBo>();
            if (id.HasValue)
            {
                versionBos = TreatyPricingFinancialTableVersionService.GetByTreatyPricingFinancialTableId(id.Value).ToList();
            }
            return Json(new { versionBos });
        }

        //[Auth(Controller = Controller, Power = "C")]
        [HttpPost]
        public JsonResult Add(TreatyPricingFinancialTableBo financialTableBo)
        {
            List<string> errors = new List<string>();
            
            if (!CheckPower(Controller, "C"))
            {
                errors.Add(string.Format(MessageBag.AccessDeniedWithActionName, "Add", "Financial Table"));
                return Json(new { errors });
            }

            try
            {
                TreatyPricingFinancialTableVersionBo versionBo = new TreatyPricingFinancialTableVersionBo();
                if (!financialTableBo.DuplicateFromList && string.IsNullOrEmpty(financialTableBo.Name))
                {
                    errors.Add("Financial Table Name is required");
                }

                if (financialTableBo.IsDuplicateExisting)
                {
                    if (!financialTableBo.DuplicateTreatyPricingFinancialTableId.HasValue)
                    {
                        errors.Add("No Financial Table Selected");
                    }

                    if (!financialTableBo.DuplicateFromList && !financialTableBo.DuplicateTreatyPricingFinancialTableVersionId.HasValue)
                    {
                        errors.Add("Version is required");
                    }

                    if (errors.IsNullOrEmpty())
                    {
                        TreatyPricingFinancialTableBo duplicate = TreatyPricingFinancialTableService.Find(financialTableBo.DuplicateTreatyPricingFinancialTableId);

                        if (duplicate == null)
                        {
                            errors.Add("Financial Table not found");
                        }
                        else
                        {
                            financialTableBo.BenefitCode = duplicate.BenefitCode;
                            financialTableBo.DistributionChannel = duplicate.DistributionChannel;
                            financialTableBo.CurrencyCode = duplicate.CurrencyCode;

                            TreatyPricingFinancialTableVersionBo duplicateVersion = null;
                            if (financialTableBo.DuplicateTreatyPricingFinancialTableVersionId.HasValue)
                            {
                                duplicateVersion = duplicate.TreatyPricingFinancialTableVersionBos.Where(q => q.Id == financialTableBo.DuplicateTreatyPricingFinancialTableVersionId.Value).FirstOrDefault();
                            }
                            else
                            {
                                duplicateVersion = duplicate.TreatyPricingFinancialTableVersionBos.OrderByDescending(q => q.Version).First();
                            }

                            if (duplicateVersion == null)
                            {
                                errors.Add("Product Version not found");
                            }
                            else
                            {
                                versionBo = new TreatyPricingFinancialTableVersionBo(duplicateVersion)
                                {
                                    Id = 0
                                };
                            }
                        }
                    }
                } 

                if (errors.IsNullOrEmpty())
                {
                    financialTableBo.FinancialTableId = TreatyPricingFinancialTableService.GetNextObjectId(financialTableBo.TreatyPricingCedantId);
                    financialTableBo.Status = TreatyPricingFinancialTableBo.StatusActive;
                    financialTableBo.CreatedById = AuthUserId;
                    financialTableBo.UpdatedById = AuthUserId;

                    TrailObject trail = GetNewTrailObject();

                    Result = TreatyPricingFinancialTableService.Create(ref financialTableBo, ref trail);
                    if (Result.Valid)
                    {
                        versionBo.TreatyPricingFinancialTableId = financialTableBo.Id;
                        versionBo.Version = 1;
                        versionBo.PersonInChargeId = AuthUserId;
                        versionBo.CreatedById = AuthUserId;
                        versionBo.UpdatedById = AuthUserId;
                        TreatyPricingFinancialTableVersionService.Create(ref versionBo, ref trail);

                        CreateTrail(
                            financialTableBo.Id,
                            "Create Treaty Pricing Financial Table"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(string.Format("Error occurred. Error details: {0}", ex.Message));
            }

            var financialTableBos = TreatyPricingFinancialTableService.GetByTreatyPricingCedantId(financialTableBo.TreatyPricingCedantId);

            return Json(new { errors, financialTableBos });
        }

        // GET: TreatyPricingFinancialTable/Edit/5
        public ActionResult Edit(int id, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false, int workflowId = 0, bool isEditMode = false)
        {
            if (!CheckObjectLockReadOnly(Controller, id, isEditMode))
                return RedirectDashboard();

            var FinancialTableBo = TreatyPricingFinancialTableService.Find(id);
            if (FinancialTableBo == null)
            {
                return RedirectToAction("Index");
            }

            var model = new TreatyPricingFinancialTableViewModel(FinancialTableBo);
            model.WorkflowId = workflowId;
            LoadPage(model, versionId, isCalledFromWorkflow, isQuotationWorkflow);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, TreatyPricingFinancialTableViewModel model, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false, int workflowId = 0)
        {
            TreatyPricingFinancialTableBo FinancialTableBo = TreatyPricingFinancialTableService.Find(id);
            if (FinancialTableBo == null)
            {
                return RedirectToAction("Edit", "TreatyPricingCedant", new { id = model.TreatyPricingCedantId });
            }

            object routeValues = new { id = FinancialTableBo.Id };
            if (isCalledFromWorkflow)
            {
                routeValues = new { id = FinancialTableBo.Id, versionId, isCalledFromWorkflow, isQuotationWorkflow, workflowId };
            }

            if (!CheckObjectLock(Controller, id))
                return RedirectToAction("Edit", routeValues);

            TreatyPricingFinancialTableVersionBo updatedVersionBo = null;
            FinancialTableBo.SetVersionObjects(FinancialTableBo.TreatyPricingFinancialTableVersionBos);
            if (FinancialTableBo.EditableVersion == model.CurrentVersion)
            {
                updatedVersionBo = model.GetVersionBo((TreatyPricingFinancialTableVersionBo)FinancialTableBo.CurrentVersionObject);
                FinancialTableBo.AddVersionObject(updatedVersionBo);
            }

            if (ModelState.IsValid)
            {
                FinancialTableBo = model.FormBo(FinancialTableBo);
                FinancialTableBo.UpdatedById = AuthUserId;

                TrailObject trail = GetNewTrailObject();
                Result = TreatyPricingFinancialTableService.Update(ref FinancialTableBo, ref trail);
                if (Result.Valid)
                {
                    if (updatedVersionBo != null)
                    {
                        updatedVersionBo.UpdatedById = AuthUserId;
                        TreatyPricingFinancialTableVersionService.Update(ref updatedVersionBo, ref trail);
                    }

                    CreateTrail(
                        FinancialTableBo.Id,
                        "Update Financial Table"
                    );

                    SetUpdateSuccessMessage("Financial Table", false);

                    ObjectLockController.DeleteObjectLock(Controller, id, AuthUserId);

                    return RedirectToAction("Edit", routeValues);
                }
                AddResult(Result);
            }

            model.CurrentVersion = FinancialTableBo.CurrentVersion;
            model.CurrentVersionObject = FinancialTableBo.CurrentVersionObject;
            model.VersionObjects = FinancialTableBo.VersionObjects;
            model.TreatyPricingCedantBo = FinancialTableBo.TreatyPricingCedantBo;
            model.WorkflowId = workflowId;
            LoadPage(model, versionId, isCalledFromWorkflow, isQuotationWorkflow);
            return View(model);
        }

        public void LoadPage(TreatyPricingFinancialTableViewModel model, int versionId = 0, bool isCalledFromWorkflow = false, bool isQuotationWorkflow = false)
        {
            DropDownCurrencyCode(true);
            DropDownStatus();
            DropDownDistributionTier();
            DropDownVersion(model);
            DropDownUser(UserBo.StatusActive, model.PersonInChargeId);
            GetBenefitCodes();
            GetDistributionChannels();

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
            GetRemarkDocument(model.ModuleId, model.TreatyPricingCedantId, downloadDocumentUrl, ModuleBo.ModuleController.TreatyPricingFinancialTable.ToString(), model.Id);
            GetRemarkSubjects();

            GetObjectVersionChangelog(ModuleBo.ModuleController.TreatyPricingFinancialTable.ToString(), model.Id, model);
            DropDownWorkflowObjectTypes();
            DropDownWorkflowStatus();

            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingFinancialTableBo.StatusMax))
            {
                items.Add(new SelectListItem { Text = TreatyPricingFinancialTableBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.StatusItems = items;
            return items;
        }

        public ActionResult CreateVersion(TreatyPricingFinancialTableBo bo, bool duplicatePreviousVersion = false)
        {
            bo.SetVersionObjects(TreatyPricingFinancialTableVersionService.GetByTreatyPricingFinancialTableId(bo.Id));
            TreatyPricingFinancialTableVersionBo nextVersionBo;
            TreatyPricingFinancialTableVersionBo previousVersionBo = (TreatyPricingFinancialTableVersionBo)bo.CurrentVersionObject;

            if (duplicatePreviousVersion)
            {
                nextVersionBo = new TreatyPricingFinancialTableVersionBo(previousVersionBo)
                {
                    Id = 0,
                };
            }
            else
            {
                nextVersionBo = new TreatyPricingFinancialTableVersionBo()
                {
                    TreatyPricingFinancialTableId = bo.Id,
                };
            }

            nextVersionBo.Version = bo.EditableVersion + 1;
            nextVersionBo.PersonInChargeId = AuthUserId;
            nextVersionBo.CreatedById = AuthUserId;
            nextVersionBo.UpdatedById = AuthUserId;

            ObjectVersionChangelog changelog = null;

            TrailObject trail = GetNewTrailObject();
            Result = TreatyPricingFinancialTableVersionService.Create(ref nextVersionBo, ref trail);
            if (Result.Valid)
            {
                UserTrailBo userTrail = CreateTrail(
                    nextVersionBo.Id,
                    "Create Treaty Pricing Financial Table Version"
                );

                changelog = new ObjectVersionChangelog(nextVersionBo, userTrail);
                changelog.FormatBetweenVersionTrail(previousVersionBo);
            }

            bo.AddVersionObject(nextVersionBo);
            var versions = DropDownVersion(bo);

            return Json(new { bo, versions, changelog });
        }

        [HttpPost]
        public ActionResult GetVersionDetails(int financialTableId, int financialTableVersion)
        {
            //(int treatyPricingFinancialTableVersionId)
            int treatyPricingFinancialTableVersionId = TreatyPricingFinancialTableVersionService.GetVersionId(financialTableId, financialTableVersion);

            var financialTableBos = TreatyPricingFinancialTableVersionDetailService.GetByTreatyPricingFinancialTableVersionId(treatyPricingFinancialTableVersionId);
            var fileUploadBos = TreatyPricingFinancialTableVersionFileService.GetByTreatyPricingFinancialTableVersionId(treatyPricingFinancialTableVersionId);

            return Json(new { financialTableBos, fileUploadBos });
        }

        [HttpPost]
        public ActionResult GetUploadedData(int treatyPricingFinancialTableVersionDetailId)
        {
            var legendBos = TreatyPricingFinancialTableUploadLegendService.GetByTreatyPricingFinancialTableVersionDetailId(treatyPricingFinancialTableVersionDetailId);
            var cellBos = TreatyPricingFinancialTableUploadService.GetByTreatyPricingFinancialTableVersionDetailId(treatyPricingFinancialTableVersionDetailId);

            return Json(new { legendBos, cellBos });
        }

        [HttpPost]
        public ActionResult UploadFinancialTable()
        {
            List<string> errors = new List<string>();
            TreatyPricingFinancialTableVersionFileBo financialTableVersionFileBo = new TreatyPricingFinancialTableVersionFileBo();

            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    //string treatyPricingFinancialTableVersionId = Request["treatyPricingFinancialTableVersionId"];
                    int financialTableId = int.Parse(Request["financialTableId"]);
                    int financialTableVersion = int.Parse(Request["financialTableVersion"]);
                    int treatyPricingFinancialTableVersionId = TreatyPricingFinancialTableVersionService.GetVersionId(financialTableId, financialTableVersion);

                    int uploadDistributionTier = int.Parse(Request["uploadDistributionTier"]);
                    string uploadDescription = Request["uploadDescription"];

                    HttpPostedFileBase file = files[0];
                    string fileName = Path.GetFileName(file.FileName);

                    Result = TreatyPricingFinancialTableVersionFileService.Result();

                    int fileSize = file.ContentLength / 1024 / 1024 / 1024;
                    if (fileSize >= 2)
                        Result.AddError("Uploaded file size exceeded 2 GB");

                    if (Result.Valid)
                    {
                        financialTableVersionFileBo = new TreatyPricingFinancialTableVersionFileBo
                        {
                            //TreatyPricingFinancialTableVersionId = int.Parse(treatyPricingFinancialTableVersionId),
                            TreatyPricingFinancialTableVersionId = treatyPricingFinancialTableVersionId,
                            DistributionTierPickListDetailId = uploadDistributionTier,
                            Description = uploadDescription,
                            FileName = fileName,
                            Status = TreatyPricingFinancialTableVersionFileBo.StatusSubmitForProcessing,
                            CreatedById = AuthUserId,
                            UpdatedById = AuthUserId,
                        };

                        financialTableVersionFileBo.FormatHashFileName();
                        string path = financialTableVersionFileBo.GetLocalPath();
                        Util.MakeDir(path);

                        TrailObject trail = GetNewTrailObject();
                        Result = TreatyPricingFinancialTableVersionFileService.Create(ref financialTableVersionFileBo, ref trail);
                        if (Result.Valid)
                        {
                            CreateTrail(
                                financialTableVersionFileBo.Id,
                                "Create Treaty Pricing Financial Table Version File"
                            );
                            file.SaveAs(path);

                            //Start processing Excel file
                            var process = new ProcessFinancialTable()
                            {
                                FilePath = path,
                                FinancialTableFileBo = financialTableVersionFileBo,
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

            return Json(new { errors, financialTableVersionFileBo });
        }

        [HttpPost]
        public JsonResult Search(int cedantId, string productName)
        {
            var financialTable = TreatyPricingFinancialTableService.GetCodeByTreatyPricingCedantIdProductName(cedantId, productName);
            return Json(new { FinancialTable = financialTable });
        }

        public void GetProduct(int id)
        {
            var productBos = TreatyPricingProductService.GetFinancialTableProduct(id);
            ViewBag.Products = productBos;
        }

        public ActionResult DownloadTemplate()
        {
            string template = "TreatyPricingFinancialTableTemplate.xlsx";
            string filename = string.Format("TreatyPricingFinancialTableTemplate").AppendDateTimeFileName(".xlsx");
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