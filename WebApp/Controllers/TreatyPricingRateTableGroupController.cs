using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using Newtonsoft.Json;
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
    public class TreatyPricingRateTableGroupController : BaseController
    {
        public const string Controller = "TreatyPricingRateTableGroup";

        //[Auth(Controller = Controller, Power = "C")]
        [HttpPost]
        public JsonResult AddRateTableGroup()
        {
            List<string> errors = new List<string>();
            TreatyPricingRateTableGroupBo rateTableGroupBo = new TreatyPricingRateTableGroupBo();

            if (!CheckPower(Controller, "C"))
            {
                errors.Add(string.Format(MessageBag.AccessDeniedWithActionName, "Add", "Rate Table Group"));
                return Json(new { errors });
            }

            if (Request.Files.Count == 0)
            {
                errors.Add("No files selected.");
                return Json(new { errors });
            }

            try
            {
                HttpFileCollectionBase files = Request.Files;
                string treatyPricingCedantIdStr = Request["treatyPricingCedantId"];
                string name = Request["name"];
                string description = Request["description"];

                HttpPostedFileBase file = files[0];
                string fileName = Path.GetFileName(file.FileName);
                string extension = Path.GetExtension(file.FileName);

                if (extension.ToLower() != ".xls" && extension.ToLower() != ".xlsx")
                    errors.Add("Only .xls or .xlsx file format is accepted");

                int fileSize = file.ContentLength / 1024 / 1024 / 1024;
                if (fileSize >= 2)
                    errors.Add("Uploaded file's size exceeded 2 GB");

                if (errors.IsNullOrEmpty())
                {
                    int treatyPricingCedantId = int.Parse(treatyPricingCedantIdStr);
                    rateTableGroupBo = new TreatyPricingRateTableGroupBo
                    {
                        TreatyPricingCedantId = treatyPricingCedantId,
                        Code = TreatyPricingRateTableGroupService.GetNextObjectId(treatyPricingCedantId),
                        Name = name,
                        Description = description,
                        FileName = fileName,
                        Status = TreatyPricingRateTableGroupBo.StatusPending,
                        UploadedAt = DateTime.Now,
                        UploadedById = AuthUserId,
                        CreatedById = AuthUserId,
                        UpdatedById = AuthUserId,
                    };

                    rateTableGroupBo.FormatHashFileName();
                    string path = rateTableGroupBo.GetLocalPath();
                    Util.MakeDir(path);

                    TrailObject trail = GetNewTrailObject();
                    Result = TreatyPricingRateTableGroupService.Create(ref rateTableGroupBo, ref trail);
                    if (Result.Valid)
                    {
                        CreateTrail(
                            rateTableGroupBo.Id,
                            "Create Treaty Pricing Rate Table Group"
                        );

                        file.SaveAs(path);
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(string.Format("Error occurred. Error details: ", ex.Message));
            }

            var rateTableGroupBos = TreatyPricingRateTableGroupService.GetByTreatyPricingCedantId(rateTableGroupBo.TreatyPricingCedantId);

            return Json(new { errors, rateTableGroupBos });
        }

        // GET: TreatyPricingRateTableGroup/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = TreatyPricingRateTableGroupService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var model = new TreatyPricingRateTableGroupViewModel(bo);
            LoadPage(model);
            return View(model);
        }

        // POST: TreatyPricingRateTableGroup/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, TreatyPricingRateTableGroupViewModel model)
        {
            var dbBo = TreatyPricingRateTableGroupService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            Result = TreatyPricingRateTableGroupService.Result();
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                bool isFileUploaded = model.Upload != null && model.Upload[0] != null;
                string fileName = null;
                HttpPostedFileBase uploadItem = null;
                if ((bo.Status != TreatyPricingRateTableGroupBo.StatusSuccess && bo.Status != TreatyPricingRateTableGroupBo.StatusFailed) && isFileUploaded)
                {
                    Result.AddError("File only able to be uploaded when status is \"Sucess\" or \"Failed\"");
                }

                if (Result.Valid && isFileUploaded)
                {
                    uploadItem = model.Upload[0];
                    fileName = Path.GetFileName(uploadItem.FileName);
                    string extension = Path.GetExtension(uploadItem.FileName);
                    if (extension.ToLower() != ".xls" && extension.ToLower() != ".xlsx")
                        Result.AddError("Only .xls or .xlsx file format is accepted");

                    int fileSize = uploadItem.ContentLength / 1024 / 1024 / 1024;
                    if (fileSize >= 2)
                        Result.AddError("Uploaded file's size exceeded 2 GB");
                }

                if (Result.Valid)
                {
                    var trail = GetNewTrailObject();

                    if (isFileUploaded)
                    {
                        bo.Status = TreatyPricingRateTableGroupBo.StatusPending;
                        bo.FileName = fileName;
                        bo.FormatHashFileName();
                        bo.Errors = null;
                        bo.UploadedAt = DateTime.Now;
                        bo.UploadedById = AuthUserId;
                    }

                    Result = TreatyPricingRateTableGroupService.Update(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        if (isFileUploaded)
                        {
                            // Delete old file
                            Util.DeleteFiles(bo.GetLocalDirectory(), model.HashFileName);
                            // Create new file
                            string path = bo.GetLocalPath();
                            Util.MakeDir(path);
                            uploadItem.SaveAs(path);
                        }

                        CreateTrail(
                            bo.Id,
                            "Update Treaty Pricing Rate Table Group"
                        );

                        SetUpdateSuccessMessage("Rate Table Group", false);
                        return RedirectToAction("Edit", new { id = bo.Id });
                    }
                }
                AddResult(Result);
            }

            model.Upload = null;
            model.UploadedByBo = UserService.Find(model.UploadedById);
            LoadPage(model);
            return View(model);
        }

        public void LoadPage(TreatyPricingRateTableGroupViewModel model)
        {
            AuthUserName();

            var entity = new TreatyPricingRateTableGroup();
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
                GetRateTable(model.Id);

                if (!string.IsNullOrEmpty(model.Errors))
                {
                    var errors = JsonConvert.DeserializeObject<List<string>>(model.Errors);
                    ViewBag.Errors = string.Join("\n", errors.ToArray());
                }
                else
                {
                    ViewBag.Errors = "";
                }
            }

            SetViewBagMessage();
        }

        public void GetRateTable(int treatyPricingRateTableGroupId)
        {
            var treatyPricingRateTables = TreatyPricingRateTableService.GetByTreatyPricingRateTableGroupId(treatyPricingRateTableGroupId);
            ViewBag.TreatyPricingRateTables = treatyPricingRateTables;
        }

        public ActionResult FileDownload(int treatyPricingRateTableGroupId)
        {
            TreatyPricingRateTableGroupBo bo = TreatyPricingRateTableGroupService.Find(treatyPricingRateTableGroupId);
            DownloadFile(bo.GetLocalPath(), bo.FileName);
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

        public ActionResult DownloadTemplate()
        {
            string template = "TreatyPricingRateTableGroupTemplate.xlsx";
            string filename = string.Format("TreatyPricingRateTableGroupTemplate").AppendDateTimeFileName(".xlsx");
            var templateFilepath = Util.GetWebAppDocumentFilePath(template);

            DownloadFile(templateFilepath, filename);
            return null;
        }

        [HttpPost]
        public JsonResult Search(int cedantId, string productName)
        {
            var rateTableGroup = TreatyPricingRateTableGroupService.GetCodeByTreatyPricingCedantIdProductName(cedantId, productName);
            return Json(new { RateTableGroup = rateTableGroup });
        }
    }
}