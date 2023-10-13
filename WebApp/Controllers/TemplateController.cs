using BusinessObject;
using BusinessObject.TreatyPricing;
using PagedList;
using Services.TreatyPricing;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class TemplateController : BaseController
    {
        public const string Controller = "Template";

        // GET: Template
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(string Code, int? CedantId, string DocumentTypeId, string Description, string SortOrder, int? Page)
        {
            SetViewBagMessage();

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Code"] = Code,
                ["CedantId"] = CedantId,
                ["DocumentTypeId"] = DocumentTypeId,
                ["Description"] = Description,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCode = GetSortParam("Code");
            ViewBag.SortCedantId = GetSortParam("CedantId");
            ViewBag.SortDocumentTypeId = GetSortParam("DocumentTypeId");
            ViewBag.SortDescription = GetSortParam("Description");

            var query = _db.Templates.Select(TemplateViewModel.Expression());
            if (!string.IsNullOrEmpty(Code)) query = query.Where(q => q.Code.Contains(Code));
            if (CedantId != null) query = query.Where(q => q.CedantId == CedantId);
            if (!string.IsNullOrEmpty(DocumentTypeId)) query = query.Where(q => q.DocumentTypeId.Contains(DocumentTypeId));
            if (!string.IsNullOrEmpty(Description)) query = query.Where(q => q.Description.Contains(Description));

            if (SortOrder == Html.GetSortAsc("Code")) query = query.OrderBy(q => q.Code);
            else if (SortOrder == Html.GetSortDsc("Code")) query = query.OrderByDescending(q => q.Code);
            else if (SortOrder == Html.GetSortAsc("CedantId")) query = query.OrderBy(q => q.Cedant.Code).ThenBy(q => q.Cedant.Name);
            else if (SortOrder == Html.GetSortDsc("CedantId")) query = query.OrderByDescending(q => q.Cedant.Code).ThenByDescending(q => q.Cedant.Name);
            else if (SortOrder == Html.GetSortAsc("DocumentTypeId")) query = query.OrderBy(q => q.DocumentTypeId);
            else if (SortOrder == Html.GetSortDsc("DocumentTypeId")) query = query.OrderByDescending(q => q.DocumentTypeId);
            else if (SortOrder == Html.GetSortAsc("Description")) query = query.OrderBy(q => q.Description);
            else if (SortOrder == Html.GetSortDsc("Description")) query = query.OrderByDescending(q => q.Description);
            else query = query.OrderBy(q => q.Code);

            DropDownCedant();
            DropDownTemplateDocumentTypes();

            ViewBag.Total = query.Count();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: Template/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new TemplateViewModel());
        }

        // POST: Template/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, TemplateViewModel model)
        {
            var templateDetailBos = model.GetTemplateDetails(form);

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);
                TrailObject trail = GetNewTrailObject();
                Result = TemplateService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;
                    model.ProcessTemplateDetails(templateDetailBos, AuthUserId, ref trail);

                    CreateTrail(bo.Id, "Create Template");

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage(templateDetailBos: templateDetailBos);
            return View(model);
        }

        // GET: Template/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            TemplateBo templateBo = TemplateService.Find(id);
            if (templateBo == null)
            {
                return RedirectToAction("Index");
            }

            var templateDetailBos = TemplateDetailService.GetByTemplateId(templateBo.Id);
            LoadPage(templateBo, templateDetailBos);
            return View(new TemplateViewModel(templateBo));
        }

        // POST: Template/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, TemplateViewModel model)
        {
            TemplateBo templateBo = TemplateService.Find(id);
            if (templateBo == null)
            {
                return RedirectToAction("Index");
            }

            var templateDetailBos = model.GetTemplateDetails(form);

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);
                TrailObject trail = GetNewTrailObject();
                Result = TemplateService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.ProcessTemplateDetails(templateDetailBos, AuthUserId, ref trail);

                    CreateTrail(templateBo.Id, "Update Template");

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = templateBo.Id });
                }
                AddResult(Result);
            }

            LoadPage(templateBo, templateDetailBos);
            return View(model);
        }

        // GET: Template/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();

            TemplateBo templateBo = TemplateService.Find(id);
            if (templateBo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new TemplateViewModel(templateBo));
        }

        // POST: Template/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id)
        {
            TemplateBo templateBo = TemplateService.Find(id);
            if (templateBo == null)
            {
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = TemplateService.Delete(templateBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(templateBo.Id, "Delete Template");

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = templateBo.Id });
        }

        public void LoadPage(TemplateBo templateBo = null, IList<TemplateDetailBo> templateDetailBos = null)
        {
            if (templateBo == null)
            {
                DropDownTemplateDocumentTypes();
                DropDownCedant(CedantBo.StatusActive);
                if (templateDetailBos == null)
                {
                    ViewBag.TemplateDetailBos = Array.Empty<TemplateDetailBo>();
                }
                else
                {
                    ViewBag.TemplateDetailBos = templateDetailBos;
                }
            }
            else
            {
                DropDownTemplateDocumentTypes();
                DropDownCedant(CedantBo.StatusActive, templateBo.CedantId);
                if (templateBo.CedantBo.Status == CedantBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.CedantStatusInactive);
                }

                ViewBag.TemplateDetailBos = templateDetailBos;
            }
            SetViewBagMessage();
        }

        [HttpPost]
        public JsonResult Upload()
        {
            string error = null;
            var bo = new TemplateDetailBo();

            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;

                    string path = TemplateDetailBo.GetTempFolderPath("Uploads");
                    string fileName = Path.GetFileName(files[0].FileName);
                    string hashFileName = Hash.HashFileName(fileName);
                    string tempFilePath = string.Format("{0}/{1}", path, hashFileName);

                    Util.MakeDir(tempFilePath);
                    HttpPostedFileBase file = files[0];
                    file.SaveAs(tempFilePath);

                    bo.FileName = fileName;
                    bo.HashFileName = hashFileName;
                    bo.TempFilePath = tempFilePath;
                    bo.CreatedAt = DateTime.Now;
                    bo.CreatedAtStr = DateTime.Now.ToString(Util.GetDateTimeFormat());
                    bo.CreatedById = AuthUserId;
                    bo.CreatedByName = AuthUser.FullName;    
                    
                }
                catch (Exception ex)
                {
                    error = string.Format("Error occurred. Error details: ", ex.Message);
                }
            }
            else
            {
                error = "No files selected.";
            }

            return Json(new { error, TemplateUploadBo = bo });
        }

        public ActionResult Download(int id)
        {
            TemplateDetailBo bo = TemplateDetailService.Find(id);
            if (bo == null)
                return null;

            string path = bo.GetLocalPath();
            if (System.IO.File.Exists(path))
            {
                return File(
                    System.IO.File.ReadAllBytes(bo.GetLocalPath()),
                    System.Net.Mime.MediaTypeNames.Application.Octet,
                    bo.FileName
                );
            }
            return null;
        }
    }
}