using BusinessObject.Sanctions;
using DataAccess.Entities.Sanctions;
using Newtonsoft.Json;
using PagedList;
using Services.Sanctions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class SanctionBatchController : BaseController
    {
        public const string Controller = "SanctionBatch";

        // GET: SanctionUpload
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string FileName,
            int? SourceId,
            //string UploadedAt,
            string CreatedBy,
            int? Method,
            int? Status,
            string SortOrder,
            int? Page
        )
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["FileName"] = FileName,
                ["SourceId"] = SourceId,
                //["UploadedAt"] = UploadedAt,
                ["CreatedBy"] = CreatedBy,
                ["Method"] = Method,
                ["Status"] = Status,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortFileName = GetSortParam("FileName");
            ViewBag.SortFileType = GetSortParam("FileType");
            ViewBag.SortSourceId = GetSortParam("SourceId");
            ViewBag.SortUploadedAt = GetSortParam("UploadedAt");
            ViewBag.SortCreatedBy = GetSortParam("CreatedBy");
            ViewBag.SortMethod = GetSortParam("Method");
            ViewBag.SortStatus = GetSortParam("Status");

            var query = _db.SanctionBatches.Select(SanctionBatchViewModel.Expression());

            if (!string.IsNullOrEmpty(FileName)) query = query.Where(q => q.FileName.Contains(FileName));
            if (SourceId.HasValue) query = query.Where(q => q.SourceId == SourceId);
            if (!string.IsNullOrEmpty(CreatedBy)) query = query.Where(q => q.CreatedBy.FullName.Contains(CreatedBy));
            if (Method.HasValue) query = query.Where(q => q.Method == Method);
            if (Status.HasValue) query = query.Where(q => q.Status == Status);

            if (SortOrder == Html.GetSortAsc("FileName")) query = query.OrderBy(q => q.FileName);
            else if (SortOrder == Html.GetSortDsc("FileName")) query = query.OrderByDescending(q => q.FileName);
            else if (SortOrder == Html.GetSortAsc("SourceId")) query = query.OrderBy(q => q.Source.Name);
            else if (SortOrder == Html.GetSortDsc("SourceId")) query = query.OrderByDescending(q => q.Source.Name);
            else if (SortOrder == Html.GetSortAsc("UploadedAt")) query = query.OrderBy(q => q.UploadedAt);
            else if (SortOrder == Html.GetSortDsc("UploadedAt")) query = query.OrderByDescending(q => q.UploadedAt);
            else if (SortOrder == Html.GetSortAsc("CreatedBy")) query = query.OrderBy(q => q.CreatedBy.FullName);
            else if (SortOrder == Html.GetSortDsc("CreatedBy")) query = query.OrderByDescending(q => q.CreatedBy.FullName);
            else if (SortOrder == Html.GetSortAsc("Method")) query = query.OrderBy(q => q.Method);
            else if (SortOrder == Html.GetSortDsc("Method")) query = query.OrderByDescending(q => q.Method);
            else if (SortOrder == Html.GetSortAsc("Status")) query = query.OrderBy(q => q.Status);
            else if (SortOrder == Html.GetSortDsc("Status")) query = query.OrderByDescending(q => q.Status);
            else query = query.OrderByDescending(q => q.Id);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // Post: SanctionUpload
        [Auth(Controller = Controller, Power = "C", ReturnController = Controller)]
        public ActionResult UploadFile(
            int? UploadUpdateMethod,
            int? UploadSourceId,
            HttpPostedFileBase[] Upload = null
        )
        {
            Result = SanctionBatchService.Result();
            if (!UploadUpdateMethod.HasValue)
                Result.AddError("Update Method is Required.");
            if (!UploadSourceId.HasValue)
                Result.AddError("Source is Required.");
            if (Upload == null)
                Result.AddError("File Upload is Required.");

            if (Upload != null && Upload[0] != null)
            {
                var uploadItem = Upload[0];
                string extension = System.IO.Path.GetExtension(uploadItem.FileName);

                if (extension.ToLower() != ".csv")
                    Result.AddError("Only .csv file format is accepted");

                int fileSize = uploadItem.ContentLength / 1024 / 1024 / 1024;
                if (fileSize >= 2)
                    Result.AddError("Uploaded file's size exceeded 2 GB");
            }

            if (Result.Valid)
            {
                var uploadItem = Upload[0];

                SanctionBatchBo bo = new SanctionBatchBo
                {
                    Method = UploadUpdateMethod.Value,
                    SourceId = UploadSourceId.Value,
                    FileName = uploadItem.FileName,
                    Status = SanctionBatchBo.StatusPending,
                    UploadedAt = DateTime.Now,
                    CreatedById = AuthUserId,
                    UpdatedById = AuthUserId,
                };

                bo.FormatHashFileName();

                string path = bo.GetLocalPath();
                Util.MakeDir(path);
                uploadItem.SaveAs(path);

                var trail = GetNewTrailObject();
                Result = SanctionBatchService.Create(ref bo, ref trail);

                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Create Sanction Upload"
                    );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Index");
                }
            }

            SetErrorSessionMsgArr(Result.ToErrorArray().OfType<string>().ToList());
            return RedirectToAction("Index");
        }

        public void Download(int id)
        {
            try
            {
                SanctionBatchBo bo = SanctionBatchService.Find(id);
                string filePath = bo.GetLocalPath();
                string fileName = bo.FileName;

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

        // GET: SanctionUpload/Edit/5
        public ActionResult Edit(int id, int? Page)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = SanctionBatchService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            if (!string.IsNullOrEmpty(bo.Errors))
            {
                var errors = JsonConvert.DeserializeObject<List<string>>(bo.Errors);
                ViewBag.Errors = string.Join("\n", errors.ToArray());
            }
            else
            {
                ViewBag.Errors = "";
            }

            var model = new SanctionBatchViewModel(bo);

            // For Sanction Upload Detail listing table
            ListDetail(id, Page);

            LoadPage();
            return View(model);
        }

        public void ListDetail(int id, int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary { };

            _db.Database.CommandTimeout = 0;

            var query = _db.Sanctions.Where(q => q.SanctionBatchId == id).Select(SanctionViewModel.Expression());

            int maxNameCount = SanctionNameService.CountMaxRowBySanctionBatchId(id);
            int maxIdentityCount = SanctionIdentityService.CountMaxRowBySanctionBatchId(id);

            ViewBag.MaxNameCount = maxNameCount;
            ViewBag.MaxIdentityCount = maxIdentityCount;
            ViewBag.DetailTotal = query.Count();
            ViewBag.DetailList = query.OrderBy(q => q.Id).ToPagedList(Page ?? 1, PageSize);
        }

        public void IndexPage()
        {
            DropDownSource();
            DropDownStatus();
            DropDownMethod();
            SetViewBagMessage();
        }

        public void LoadPage()
        {
            DropDownSource();
            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= SanctionBatchBo.StatusMax; i++)
            {
                items.Add(new SelectListItem { Text = SanctionBatchBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownStatuses = items;
            return items;
        }

        public List<SelectListItem> DropDownMethod()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= SanctionBatchBo.MethodMax; i++)
            {
                items.Add(new SelectListItem { Text = SanctionBatchBo.GetMethodName(i), Value = i.ToString() });
            }
            ViewBag.DropDownMethods = items;
            return items;
        }
    }
}