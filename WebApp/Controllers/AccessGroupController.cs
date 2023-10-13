using BusinessObject;
using BusinessObject.Identity;
using PagedList;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class AccessGroupController : BaseController
    {
        public const string Controller = "AccessGroup";

        // GET: AccessGroup
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(int? DepartmentId, string Code, string Name, string SortOrder, int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["DepartmentId"] = DepartmentId,
                ["Code"] = Code,
                ["Name"] = Name,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortDepartmentId = GetSortParam("DepartmentId");
            ViewBag.SortCode = GetSortParam("Code");
            ViewBag.SortName = GetSortParam("Name");

            var query = _db.AccessGroups.Select(AccessGroupViewModel.Expression());
            if (DepartmentId.HasValue) query = query.Where(q => q.DepartmentId == DepartmentId);
            if (!string.IsNullOrEmpty(Code)) query = query.Where(q => q.Code.Contains(Code));
            if (!string.IsNullOrEmpty(Name)) query = query.Where(q => q.Name.Contains(Name));

            if (SortOrder == Html.GetSortAsc("DepartmentId")) query = query.OrderBy(q => q.Department.Name);
            else if (SortOrder == Html.GetSortDsc("DepartmentId")) query = query.OrderByDescending(q => q.Department.Name);
            else if (SortOrder == Html.GetSortAsc("Code")) query = query.OrderBy(q => q.Code);
            else if (SortOrder == Html.GetSortDsc("Code")) query = query.OrderByDescending(q => q.Code);
            else if (SortOrder == Html.GetSortAsc("Name")) query = query.OrderBy(q => q.Name);
            else if (SortOrder == Html.GetSortDsc("Name")) query = query.OrderByDescending(q => q.Name);
            else query = query.OrderBy(q => q.Department.Name).ThenBy(q => q.Code);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: AccessGroup/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            AccessGroupViewModel model = new AccessGroupViewModel();
            LoadPage(model);
            return View(model);
        }

        // POST: AccessGroup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, AccessGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var accessGroupBo = new AccessGroupBo
                {
                    DepartmentId = model.DepartmentId,
                    Code = model.Code?.Trim(),
                    Name = model.Name?.Trim(),
                    CreatedById = AuthUserId,
                    UpdatedById = AuthUserId,
                };

                TrailObject trail = GetNewTrailObject();
                Result = AccessGroupService.Create(ref accessGroupBo, ref trail);
                if (Result.Valid)
                {
                    model.Id = accessGroupBo.Id;
                    model.ProcessMatrices(form, ref trail);
                    List<DocumentBo> documentBos = DocumentController.GetDocuments(form);
                    DocumentController.SaveDocuments(documentBos, model.ModuleId, accessGroupBo.Id, AuthUserId, ref trail);

                    CreateTrail(
                        accessGroupBo.Id,
                        "Create Access Group"
                    );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = accessGroupBo.Id });
                }
                AddResult(Result);
            }
            LoadPage(model, form);
            return View(model);
        }

        // GET: AccessGroup/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            AccessGroupBo accessGroupBo = AccessGroupService.Find(id);
            if (accessGroupBo == null)
            {
                return RedirectToAction("Index");
            }
            AccessGroupViewModel model = new AccessGroupViewModel(accessGroupBo);
            LoadPage(model);
            return View(model);
        }

        // POST: AccessGroup/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, AccessGroupViewModel model)
        {
            AccessGroupBo accessGroupBo = AccessGroupService.Find(id);
            if (accessGroupBo == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                accessGroupBo.DepartmentId = model.DepartmentId;
                accessGroupBo.Code = model.Code?.Trim();
                accessGroupBo.Name = model.Name?.Trim();
                accessGroupBo.UpdatedById = AuthUserId;

                TrailObject trail = GetNewTrailObject();
                Result = AccessGroupService.Update(ref accessGroupBo, ref trail);
                if (Result.Valid)
                {
                    model.ProcessMatrices(form, ref trail);

                    CreateTrail(
                        accessGroupBo.Id,
                        "Update Access Group"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = accessGroupBo.Id });
                }
                AddResult(Result);
            }
            LoadPage(model, form);
            return View(model);
        }

        // GET: AccessGroup/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            AccessGroupBo accessGroupBo = AccessGroupService.Find(id);
            if (accessGroupBo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new AccessGroupViewModel(accessGroupBo));
        }

        // POST: AccessGroup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, AccessGroupViewModel model)
        {
            AccessGroupBo accessGroupBo = AccessGroupService.Find(id);
            if (accessGroupBo == null)
            {
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = AccessGroupService.Delete(accessGroupBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    accessGroupBo.Id,
                    "Delete Access Group"
                );
                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = accessGroupBo.Id });
        }

        // GET: AccessGroup/RequestAccessGroup
        public ActionResult RequestAccessGroup()
        {
            if (AuthUser.DepartmentId == null)
            {
                SetErrorSessionMsg("You Do not belong to any department");
                return RedirectToAction("Index");
            }
            LoadRequestPage(AuthUser.DepartmentId);
            return View(new RequestAccessGroupViewModel());
        }

        // POST: AccessGroup/RequestAccessGroup
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestAccessGroup(FormCollection form, RequestAccessGroupViewModel model)
        {

            ModelState.Remove("Id");
            ModelState.Remove("Code");
            model.Code = null;
            if (model.Id != 0 && model.Code == null)
            {
                AccessGroupBo accessGroupBo = AccessGroupService.Find(model.Id);
                if (accessGroupBo != null)
                    model.Code = accessGroupBo.Code;
            }

            bool hasAccessMatrix = false;
            IList<AccessMatrixBo> inputAccessMatriceBos = model.GetMatrices(form);
            List<AccessMatrixBo> filteredAccessMatriceBos = new List<AccessMatrixBo>();
            foreach (AccessMatrixBo accessMatrixBo in inputAccessMatriceBos)
            {
                if (string.IsNullOrEmpty(accessMatrixBo.Power))
                    continue;

                filteredAccessMatriceBos.Add(accessMatrixBo);
                hasAccessMatrix = true;
            }

            if (!hasAccessMatrix)
            {
                ModelState.AddModelError("", "Access Group should have at least 1 access power");
            }

            if (ModelState.IsValid)
            {
                if (!model.Download)
                {
                    ViewBag.AccessMatrices = filteredAccessMatriceBos;
                    model.Download = true;
                    LoadRequestPage(AuthUser.DepartmentId);

                    ViewBag.SuccessMessageBottom = "Your request form will be downloaded shortly.";

                    return View(model);
                }

                MemoryStream s = new MemoryStream();
                Pdf document = new Pdf(Server.MapPath("~/Document/Access_Group_Request_Form_Template.pdf"));

                document.CreateRequestedDate(model.RequestDateStr);
                document.AddVerticalSpace(10);

                document.SectionHeader("EMPLOYEE DETAILS");
                document.AddTextBox("Employee Name", AuthUser.FullName);
                document.AddTextBox("Username", AuthUser.UserName);
                document.AddTextBox("Email", AuthUser.Email);
                document.AddVerticalSpace(20);

                DepartmentBo departmentBo = DepartmentService.Find(model.DepartmentId);
                string departmentName = departmentBo != null ? departmentBo.Name : "";

                document.SectionHeader("DEPARTMENT, ACCESS GROUP & REQUEST TYPE");
                document.AddTextBox("Department", departmentName);
                document.AddTextBox("Access Group", model.Name);
                document.AddTextBox("Request Type", AccessGroupBo.GetRequestTypeName(model.RequestType));
                document.AddTextBox("Requested By", AuthUser.FullName);
                document.AddVerticalSpace(20);

                document.SectionHeader("ACCESS POWER");
                document.AddText("Refer Appendix A", false);
                document.AddVerticalSpace(20);

                document.SectionHeader("APPROVAL");
                document.SignatureSection("HOD Signature", false);
                document.SignatureSection("Module Owner Signature");
                document.AddVerticalSpace(20);

                document.DrawITSection();

                document.AddPage();

                document.AddText("Appendix A: Module & Access Power", true, false, true);
                document.AddVerticalSpace(10);


                var groupedAccessMatrixBos = filteredAccessMatriceBos.GroupBy(am => am.DepartmentName);

                foreach (var accessMatrixBos in groupedAccessMatrixBos)
                {
                    document.AddText(accessMatrixBos.Key, true, false);
                    document.AddVerticalSpace(10);
                    foreach (AccessMatrixBo accessMatrixBo in accessMatrixBos)
                    {
                        document.DrawPowerTable(accessMatrixBo.ModuleBo.Name, ModuleBo.GetTypeName(accessMatrixBo.ModuleBo.Type), accessMatrixBo.GetPowersAsString());
                        document.AddVerticalSpace(10);
                    }

                    document.AddVerticalSpace(15);
                }

                document.Document.Save(s);
                var fileName = "AccessGroupApplication.pdf";
                var contentType = "application/pdf";
                return File(s, contentType, fileName);
            }

            ViewBag.AccessMatrices = filteredAccessMatriceBos;
            model.Download = false;
            LoadRequestPage(AuthUser.DepartmentId);
            return View(model);
        }

        public void IndexPage()
        {
            DropDownDepartment();
            SetViewBagMessage();
        }

        public void LoadPage(AccessGroupViewModel model, FormCollection form = null)
        {
            DropDownDepartment(false, model.DepartmentId);
            DropDownDocumentType();
            string authUserName = AuthUserName();

            string downloadDocumentUrl = Url.Action("Download", "Document");
            if (form != null)
            {
                ViewBag.DocumentBos = DocumentController.GetDocuments(form, downloadDocumentUrl);
                ViewBag.AccessMatrices = model.GetMatrices(form);
            }
            else
            {
                GetDocuments(model.ModuleId, model.Id, downloadDocumentUrl);
            }
            SetViewBagMessage();
        }

        public void LoadRequestPage(int? departmentId, FormCollection form = null)
        {
            ViewBag.AuthUserDepartment = DepartmentService.Find(departmentId);
            DropDownRequestType();
            DropDownAccessGroup(true, departmentId);
            AuthUserName();
            SetViewBagMessage();
        }

        public ActionResult GetAccessMatricesView(int? id)
        {
            AccessGroupViewModel model = new AccessGroupViewModel();
            if (id != null)
            {
                AccessGroupBo accessGroupBo = AccessGroupService.Find(id.Value);
                model = new AccessGroupViewModel(accessGroupBo);
            }

            return PartialView("_AccessMatrices", model);
        }

        [HttpPost]
        public JsonResult GetList(int? DepartmentId, int? SelectedId)
        {
            return Json(new { AccessGroupBos = AccessGroupService.GetByDepartmentId(DepartmentId) });
        }
    }
}