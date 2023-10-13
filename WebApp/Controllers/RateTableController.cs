using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BusinessObject;
using ConsoleApp.Commands.ProcessDatas;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Newtonsoft.Json;
using PagedList;
using Services;
using Shared;
using Shared.DataAccess;
using Shared.ProcessFile;
using Shared.Trails;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class RateTableController : BaseController
    {
        public const string Controller = "RateTable";

        // GET: Rate
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string Code,
            string SortOrder,
            int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Code"] = Code,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCode = GetSortParam("Code");

            var query = _db.Rates.Select(RateViewModel.Expression());

            if (!string.IsNullOrEmpty(Code)) query = query.Where(q => q.Code.Contains(Code));

            if (SortOrder == Html.GetSortAsc("Code")) query = query.OrderBy(q => q.Code);
            else if (SortOrder == Html.GetSortDsc("Code")) query = query.OrderByDescending(q => q.Code);
            else query = query.OrderByDescending(q => q.Id);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: Rate/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            RateViewModel model = new RateViewModel();

            LoadPage(model);
            ViewBag.Disabled = false;
            return View(model);
        }

        // POST: Rate/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, RateViewModel model)
        {
            Result childResult = new Result();
            List<RateDetailBo> rateDetailBos = model.GetRateDetails(form, ref childResult);
            List<RateDetailUploadBo> rateDetailUploadBos = model.GetRateDetailUploads(form);

            if (ModelState.IsValid)
            {
                Result = RateDetailService.Result();
                var bo = model.FormBo(AuthUserId, AuthUserId);

                if (childResult.Valid)
                    model.ValidateDuplicate(rateDetailBos, ref childResult);

                if (childResult.Valid)
                {
                    var trail = GetNewTrailObject();
                    Result = RateService.Create(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        model.Id = bo.Id;
                        model.ProcessRateDetails(rateDetailBos, AuthUserId, ref trail);
                        model.SaveRateDetailUploads(rateDetailUploadBos, AuthUserId);

                        CreateTrail(
                            bo.Id,
                            "Create Rate Table"
                        );

                        SetCreateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = bo.Id });
                    }
                }
                Result.AddErrorRange(childResult.ToErrorArray());
                AddResult(Result);
            }

            LoadPage(model, rateDetailBos, rateDetailUploadBos);
            ViewBag.Disabled = false;
            return View(model);
        }

        // GET: Rate/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = RateService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            RateViewModel model = new RateViewModel(bo);

            LoadPage(model);
            ViewBag.Disabled = false;
            return View(model);
        }

        // POST: Rate/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, RateViewModel model)
        {
            var dbBo = RateService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            Result childResult = new Result();
            List<RateDetailUploadBo> rateDetailUploadBos = RateDetailUploadService.GetByRateId(id).ToList();
            List<RateDetailBo> rateDetailBos = model.UpdateRateDetailFields(RateDetailService.GetByRateId(id, false).ToList());

            if (ModelState.IsValid)
            {
                bool valuationRateChanged = false;
                if (model.ValuationRate != dbBo.ValuationRate)
                    valuationRateChanged = true;

                Result = RateDetailService.Result();
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                if (childResult.Valid && valuationRateChanged)
                    model.ValidateDuplicate(rateDetailBos, ref childResult);

                if (childResult.Valid)
                {
                    var trail = GetNewTrailObject();
                    Result = RateService.Update(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        if (valuationRateChanged) // need to update 
                            model.ProcessRateDetails(rateDetailBos, AuthUserId, ref trail);
                        model.SaveRateDetailUploads(rateDetailUploadBos, AuthUserId);

                        CreateTrail(
                            bo.Id,
                            "Update Rate Table"
                        );

                        SetUpdateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = bo.Id });
                    }
                }
                Result.AddErrorRange(childResult.ToErrorArray());
                AddResult(Result);
            }

            LoadPage(model);
            ViewBag.Disabled = false;
            return View(model);
        }

        // GET: Rate/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            var bo = RateService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }
            RateViewModel model = new RateViewModel(bo);
            LoadPage(model);
            ViewBag.Disabled = true;
            return View(model);
        }

        // POST: Rate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, RateViewModel model)
        {
            var bo = RateService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = RateService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Rate Table"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = bo.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadDetail(FormCollection form, RateViewModel model)
        {
            ModelState.Clear();
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            List<RateDetailBo> rateDetailBos = null;
            if (model.Id == 0)
            {
                Result childResult = new Result();
                rateDetailBos = model.GetRateDetails(form, ref childResult);
            }

            HttpPostedFileBase postedFile = Request.Files["RateTableFile"];

            IList<RateDetailUploadBo> uploadBos = model.GetRateDetailUploads(form); ;
            if (Request.Files.Count > 0)
            {
                try
                {
                    int fileSize = postedFile.ContentLength / 1024 / 1024;
                    if (fileSize >= 10)
                    {
                        ModelState.AddModelError("", "Uploaded file's size exceeded 10 MB");
                    }
                    else
                    {
                        string fileName = Path.GetFileName(postedFile.FileName);

                        RateDetailUploadBo uploadBo = new RateDetailUploadBo
                        {
                            FileName = fileName,
                            HashFileName = Hash.HashFileName(fileName),
                        };

                        if (model.Id == 0)
                        {
                            string path = RateDetailUploadBo.GetTempFolderPath("Uploads");
                            string tempFilePath = string.Format("{0}/{1}", path, uploadBo.HashFileName);

                            Util.MakeDir(tempFilePath);
                            HttpPostedFileBase file = postedFile;
                            file.SaveAs(tempFilePath);

                            uploadBos.Add(uploadBo);
                        }
                        else
                        {
                            uploadBo.RateId = model.Id;
                            uploadBo.Status = 0;
                            uploadBo.CreatedById = AuthUserId;

                            string path = uploadBo.GetLocalPath();
                            Util.MakeDir(path);
                            HttpPostedFileBase file = postedFile;
                            file.SaveAs(path);

                            TrailObject trail = GetNewTrailObject();
                            Result = RateDetailUploadService.Create(ref uploadBo, ref trail);
                            if (Result.Valid)
                            {
                                uploadBos = RateDetailUploadService.GetByRateId(model.Id);
                                CreateTrail(uploadBo.Id, "Create Rate Detail Upload");
                            }
                        }
                    }

                    SetSuccessSessionMsg("File uploaded.");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("", "No files selected.");
            }

            string view = "Create";
            if (model.Id != 0)
            {
                view = "Edit";
                CheckEditPageReadOnly(Controller);
            }
            else
            {
                ViewBag.ReadOnly = false;
            }

            ViewBag.Disabled = false;
            LoadPage(model, rateDetailBos, (List<RateDetailUploadBo>)uploadBos);
            return View(view, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult DownloadDetail(RateViewModel model, string downloadToken, int type)
        {
            if (type == 2) // template download
            {
                List<Column> cols = RateDetailBo.GetColumns();

                string filename = "RateTable".AppendDateTimeFileName(".csv");
                string path = Util.GetTemporaryPath();

                string filePath = string.Format("{0}/{1}", path, filename);
                Util.MakeDir(filePath);

                // Delete all previous files
                Util.DeleteFiles(path, @"RateTable*");

                // Header
                ExportWriteLine(filePath, string.Join(",", cols.Select(p => p.Header)));

                return File(filePath, "text/csv", Path.GetFileName(filePath));
            }
            else
            {
                Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
                Session["lastActivity"] = DateTime.Now.Ticks;

                dynamic Params = new ExpandoObject();
                Params.RateId = model.Id;

                var export = new GenerateExportData();
                ExportBo exportBo = export.CreateExportRateDetail(AuthUserId, Params);

                if (exportBo.Total <= export.ExportInstantDownloadLimit)
                {
                    export.Process(ref exportBo, _db);
                    Session.Add("DownloadExport", true);
                }

                return RedirectToAction("Edit", "Export", new { exportBo.Id });
            }
        }

        public void ExportWriteLine(string filePath, object line)
        {
            using (var textFile = new TextFile(filePath, true, true))
            {
                textFile.WriteLine(line);
            }
        }

        public void IndexPage()
        {
            DropDownEmpty();
            SetViewBagMessage();
        }

        public void LoadPage(RateViewModel model, List<RateDetailBo> rateDetailBos = null, List<RateDetailUploadBo> rateDetailUploadBos = null)
        {
            DropDownInsuredGenderCode();
            DropDownInsuredTobaccoUse();
            DropDownInsuredOccupationCode();
            DropDownValuationRate();

            var entity = new Rate();
            var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Code");
            ViewBag.CodeMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 50;

            if (model.Id == 0)
            {
                // Create
                ViewBag.RateDetailBos = rateDetailBos;
                ViewBag.RateDetailUploadBos = rateDetailUploadBos;
            }
            else
            {
                ViewBag.RateDetailUploadBos = RateDetailUploadService.GetByRateId(model.Id);
            }

            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownValuationRate()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= RateBo.MaxValuationRate; i++)
            {
                items.Add(new SelectListItem { Text = RateBo.GetValuationRateName(i), Value = i.ToString() });
            }
            ViewBag.DropDownValuationRates = items;
            return items;
        }

        [HttpPost]
        public JsonResult ActionDetail(RateDetailBo rateDetailBo, string action)
        {
            var trail = GetNewTrailObject();
            var error = "";
            RateDetailBo bo = null;

            switch (action)
            {
                case "d": // delete
                    bo = RateDetailService.Find(rateDetailBo.Id);
                    if (bo != null)
                        RateDetailService.Delete(bo, ref trail);
                    break;
                case "u": // update
                case "c": // create
                    // to check duplicate
                    bool isFound = false;
                    var bos = RateDetailService.GetByRateId(rateDetailBo.RateId, false);

                    RateViewModel model = new RateViewModel();
                    var rdBos = model.UpdateRateDetailFields(bos.ToList());
                    if (rdBos.Any())
                    {
                        var exist = bos.Where(q => q.InsuredGenderCodePickListDetailId == rateDetailBo.InsuredGenderCodePickListDetailId)
                            .Where(q => q.CedingTobaccoUsePickListDetailId == rateDetailBo.CedingTobaccoUsePickListDetailId)
                            .Where(q => q.CedingOccupationCodePickListDetailId == rateDetailBo.CedingOccupationCodePickListDetailId)
                            .Where(q => q.AttainedAge == rateDetailBo.AttainedAge)
                            .Where(q => q.IssueAge == rateDetailBo.IssueAge)
                            .Where(q => q.PolicyTerm == rateDetailBo.PolicyTerm)
                            .Where(q => q.PolicyTermRemain == rateDetailBo.PolicyTermRemain)
                            .Where(q => q.Id != rateDetailBo.Id);
                        if (exist.Any())
                        {
                            isFound = true;
                            error = "Duplicate Data Found";
                        }
                    }

                    if (!isFound)
                    {
                        bo = rateDetailBo;
                        if (!String.IsNullOrEmpty(bo.RateValueStr))
                            bo.RateValue = Util.StringToDouble(bo.RateValueStr).Value;
                        bo.CreatedById = AuthUserId;
                        bo.UpdatedById = AuthUserId;

                        RateDetailService.Save(ref bo, ref trail);
                    }
                    break;
            }

            return Json(new { error });
        }

        [HttpPost]
        public JsonResult GetDetail(int id, int pageNumber)
        {
            IList<RateDetailBo> bos = null;
            int bosTotal = 0;
            if (pageNumber > 0)
            {
                bos = RateDetailService.GetNextByRateId(id, (pageNumber == 1 ? 0 : PageSize * (pageNumber - 1)), PageSize);
                bosTotal = RateDetailService.CountByRateId(id);
            }

            return Json(new { bos, bosTotal });
        }

        public void DownloadError(int id)
        {
            try
            {
                var rduBo = RateDetailUploadService.Find(id);
                MemoryStream ms = new MemoryStream();
                TextWriter tw = new StreamWriter(ms);
                tw.WriteLine(rduBo.Errors.Replace(",", Environment.NewLine));
                tw.Flush();
                byte[] bytes = ms.ToArray();
                ms.Close();

                Response.ClearContent();
                Response.Clear();
                //Response.Buffer = true;                                                                                                                   
                //Response.BufferOutput = false;
                Response.ContentType = MimeMapping.GetMimeMapping("RateDetailUploadError");
                Response.AddHeader("Content-Disposition", "attachment; filename=RateDetailUploadError.txt");
                Response.BinaryWrite(bytes);
                //Response.TransmitFile(filePath);
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
