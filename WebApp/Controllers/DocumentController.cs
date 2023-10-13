using BusinessObject;
using Newtonsoft.Json;
using Services;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class DocumentController : BaseController
    {
        // GET: Document
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveTemp()
        {
            string error = null;

            DocumentBo documentBo = null;
            if (Request.Files.Count > 0)
            {
                try
                {
                    documentBo = JsonConvert.DeserializeObject<DocumentBo>(Request["documentBo"]);
                    HttpFileCollectionBase files = Request.Files;

                    string path = DocumentBo.GetTempFolderPath("Uploads");
                    string fileName = Path.GetFileName(files[0].FileName);

                    documentBo.HashFileName = Hash.HashFileName(fileName);
                    documentBo.TempFilePath = string.Format("{0}/{1}", path, documentBo.HashFileName);
                    documentBo.TypeName = DocumentBo.GetTypeName(documentBo.Type);
                    documentBo.PermissionName = ObjectPermissionBo.GetPermissionName(documentBo.IsPrivate);

                    int fileSize = files[0].ContentLength / 1024 / 1024;
                    if (fileSize >= 10)
                        return Json(new { error = "Uploaded file's size exceeded 10 MB", documentBo });

                    Util.MakeDir(documentBo.TempFilePath);
                    HttpPostedFileBase file = files[0];
                    file.SaveAs(documentBo.TempFilePath);
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

            return Json(new { error, documentBo });
        }

        [HttpPost]
        public JsonResult Save()
        {
            string error = null;

            DocumentBo documentBo = null;
            if (Request.Files.Count > 0)
            {
                try
                {
                    documentBo = JsonConvert.DeserializeObject<DocumentBo>(Request["documentBo"]); 

                    HttpFileCollectionBase files = Request.Files;

                    string fileName = Path.GetFileName(files[0].FileName);
                    if (documentBo.Type == 0)
                        documentBo.Type = DocumentBo.TypeOthers;
                    documentBo.TypeName = DocumentBo.GetTypeName(documentBo.Type);
                    documentBo.HashFileName = Hash.HashFileName(fileName); 
                    documentBo.ModuleBo = ModuleService.Find(documentBo.ModuleId);
                    documentBo.PermissionName = ObjectPermissionBo.GetPermissionName(documentBo.IsPrivate);
                    documentBo.CreatedByName = AuthUser.FullName;
                    documentBo.CreatedById = AuthUserId;
                    documentBo.UpdatedById = AuthUserId;

                    int fileSize = files[0].ContentLength / 1024 / 1024;
                    if (fileSize >= 10)
                        return Json(new { error = "Uploaded file's size exceeded 10 MB", documentBo });

                    string path = documentBo.GetLocalPath();
                    Util.MakeDir(path);
                    HttpPostedFileBase file = files[0];
                    file.SaveAs(path);

                    TrailObject trail = GetNewTrailObject();
                    Result = DocumentService.Create(ref documentBo, ref trail);
                    if (Result.Valid)
                    {
                        documentBo.CreatedAtStr = documentBo.CreatedAt.ToString("dd MMM yyyy hh:mm:ss tt");
                        if (documentBo.IsPrivate && AuthUser.DepartmentId.HasValue)
                        {
                            var permission = new ObjectPermissionBo()
                            {
                                ObjectId = documentBo.Id,
                                Type = ObjectPermissionBo.TypeDocument,
                                DepartmentId = AuthUser.DepartmentId.Value,
                                CreatedById = AuthUserId
                            };
                            ObjectPermissionService.Create(ref permission, ref trail);
                        }

                        if (documentBo.IsFileExists())
                        {
                            documentBo.GetDownloadLink(Url.Action("Download", "Document"));
                        }

                        CreateTrail(
                            documentBo.Id,
                            "Create Document"
                        );
                    }
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

            return Json(new { error, documentBo });
        }

        [HttpPost]
        public JsonResult Delete(DocumentBo documentBo)
        {
            bool success = false;
            if (documentBo.Id != 0)
            {
                TrailObject trail = GetNewTrailObject();
                documentBo = DocumentService.Find(documentBo.Id);

                Result = DocumentService.Delete(documentBo, ref trail);
                if (Result.Valid)
                {
                    success = true;
                    CreateTrail(
                        documentBo.Id,
                        "Delete Document"
                    );
                }
            }
            else
            {
                Util.DeleteFiles(DocumentBo.GetTempFolderPath("Uploads"), documentBo.HashFileName);
                success = true;
            }

            return Json(new { success });
        }

        public ActionResult Download(int id)
        {
            DocumentBo documentBo = DocumentService.Find(id);
            if (documentBo == null)
                return null;

            documentBo.ModuleBo = ModuleService.Find(documentBo.ModuleId);
            string path = documentBo.GetLocalPath();
            if (System.IO.File.Exists(path))
            {
                return File(
                    System.IO.File.ReadAllBytes(documentBo.GetLocalPath()),
                    System.Net.Mime.MediaTypeNames.Application.Octet,
                    documentBo.FileName
                );
            }
            return null;
        }

        public static List<DocumentBo> GetDocuments(FormCollection form, string downloadUrl = "")
        {
            string maxIndexStr = form.Get("document.MaxIndex");
            int maxIndex = string.IsNullOrEmpty(maxIndexStr) ? 0 : int.Parse(maxIndexStr);
            List<DocumentBo> documentBos = new List<DocumentBo>();
            for (int i = 0; i <= maxIndex; i++)
            {
                DocumentBo documentBo = GetDocument(form, i);

                if (documentBo == null)
                    continue;

                if (documentBo.IsFileExists())
                {
                    documentBo.GetDownloadLink(downloadUrl);
                }

                documentBos.Add(documentBo);
            }

            return documentBos;
        }
        
        public static DocumentBo GetDocument(FormCollection form, int index)
        {
            string idStr = form.Get(string.Format("document.Id[{0}]", index));
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
            {
                var bo = DocumentService.Find(id);
                if (bo != null)
                    return bo;
            }

            string tempFilePath = form.Get(string.Format("document.TempFilePath[{0}]", index));
            if (string.IsNullOrEmpty(tempFilePath))
                return null;

            string typeStr = form.Get(string.Format("document.Type[{0}]", index));
            string fileName = form.Get(string.Format("document.Filename[{0}]", index));
            string hashFileName = form.Get(string.Format("document.HashFileName[{0}]", index));
            string description = form.Get(string.Format("document.Description[{0}]", index));
            string createdAtStr = form.Get(string.Format("document.CreatedAtStr[{0}]", index));
            string createdByName = form.Get(string.Format("document.CreatedByName[{0}]", index));
            string isPrivateStr = form.Get(string.Format("document.IsPrivate[{0}]", index));
            string remarkIndexStr = form.Get(string.Format("document.RemarkIndex[{0}]", index));

            int type = string.IsNullOrEmpty(typeStr) ? DocumentBo.TypeOthers : int.Parse(typeStr);
            bool isPrivate = string.IsNullOrEmpty(isPrivateStr) ? false : bool.Parse(isPrivateStr);
            DateTime? createdAt = Util.GetParseDateTime(createdAtStr);

            int? remarkIndex = null;
            if (!string.IsNullOrEmpty(remarkIndexStr))
            {
                remarkIndex = Util.GetParseInt(remarkIndexStr);
            }

            DocumentBo documentBo = new DocumentBo
            {
                Type = type,
                TypeName = DocumentBo.GetTypeName(type),
                IsPrivate = isPrivate,
                PermissionName = ObjectPermissionBo.GetPermissionName(isPrivate),
                FileName = fileName,
                HashFileName = hashFileName,
                TempFilePath = tempFilePath,
                Description = description,
                CreatedAtStr = createdAtStr,
                CreatedByName = createdByName,
                RemarkIndex = remarkIndex,
                CreatedAt = createdAt ?? DateTime.Now,
                UpdatedAt = createdAt ?? DateTime.Now,
            };

            return documentBo;
        }

        public static void SaveDocuments(IList<DocumentBo> documentBos, int moduleId, int objectId, int authUserId, ref TrailObject trail, int? remarkId = null, int? departmentId = null)
        {
            ModuleBo moduleBo = ModuleService.Find(moduleId);
            foreach (DocumentBo bo in documentBos)
            {
                DocumentBo documentBo = bo;
                documentBo.ModuleId = moduleId;
                documentBo.ModuleBo = moduleBo;
                documentBo.ObjectId = objectId;
                documentBo.RemarkId = remarkId;
                documentBo.CreatedById = authUserId;
                documentBo.UpdatedById = authUserId;

                string path = documentBo.GetLocalPath();
                string tempPath = documentBo.GetTempPath("Uploads");
                if (System.IO.File.Exists(tempPath))
                {
                    Util.MakeDir(path);
                    System.IO.File.Move(tempPath, path);
                }

                DocumentService.Save(ref documentBo, ref trail);
                if (documentBo.IsPrivate && departmentId.HasValue)
                {
                    var permission = new ObjectPermissionBo()
                    {
                        ObjectId = documentBo.Id,
                        Type = ObjectPermissionBo.TypeRemark,
                        DepartmentId = departmentId.Value,
                        CreatedById = authUserId
                    };
                    ObjectPermissionService.Create(ref permission, ref trail);
                }
            }
        }
    }
}