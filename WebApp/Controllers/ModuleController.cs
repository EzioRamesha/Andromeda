using BusinessObject;
using BusinessObject.Identity;
using PagedList;
using Services;
using Shared;
using Shared.Trails;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ModuleController : BaseController
    {
        public const string Controller = "Module";

        // GET: Module
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(string Name, string ReportPath, string SortOrder, int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Name"] = Name,
                ["ReportPath"] = ReportPath,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortName = GetSortParam("Name");
            ViewBag.SortReportPath = GetSortParam("ReportPath");

            var query = _db.Modules.Select(ModuleViewModel.Expression());
            if (!string.IsNullOrEmpty(Name)) query = query.Where(q => q.Name.Contains(Name));
            if (!string.IsNullOrEmpty(ReportPath)) query = query.Where(q => q.ReportPath.Contains(ReportPath));

            if (SortOrder == Html.GetSortAsc("Name")) query = query.OrderBy(q => q.Name);
            else if (SortOrder == Html.GetSortDsc("Name")) query = query.OrderByDescending(q => q.Name);
            else if (SortOrder == Html.GetSortAsc("ReportPath")) query = query.OrderBy(q => q.ReportPath);
            else if (SortOrder == Html.GetSortDsc("ReportPath")) query = query.OrderByDescending(q => q.ReportPath);
            else query = query.OrderBy(q => q.Name);

            ViewBag.Total = query.Count();
            SetViewBagMessage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: Module/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            SetViewBagMessage();
            ViewBag.DepartmentId = GetDepartmentList();
            return View(new ModuleViewModel());
        }

        // POST: Module/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(ModuleViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Can create report typpe only
                ModuleBo moduleBo = new ModuleBo
                {
                    DepartmentId = model.DepartmentId,
                    Type = ModuleBo.TypeReport,
                    Controller = model.Name.ToPascalCase(),
                    Name = model.Name?.Trim(),
                    Editable = true,
                    Power = "R",
                    ReportPath = model.ReportPath?.Trim(),
                    CreatedById = AuthUserId,
                    UpdatedById = AuthUserId,
                };

                TrailObject trail = GetNewTrailObject();
                Result = ModuleService.Create(ref moduleBo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        moduleBo.Id,
                        "Create Module"
                    );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = moduleBo.Id });
                }
                AddResult(Result);
            }
            ViewBag.DepartmentId = GetDepartmentList();
            return View(model);
        }

        // GET: Module/Edit/5
        public ActionResult Edit(int id)
        {
            SetViewBagMessage();
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            ModuleBo moduleBo = ModuleService.Find(id);
            if (moduleBo == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = GetDepartmentList(moduleBo.DepartmentId);
            return View(new ModuleViewModel(moduleBo));
        }

        // POST: Module/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, ModuleViewModel model)
        {
            ModuleBo moduleBo = ModuleService.Find(id);
            if (moduleBo == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                if (moduleBo.DepartmentId != model.DepartmentId)
                {
                    moduleBo.DepartmentId = model.DepartmentId;
                    moduleBo.Index = ModuleService.GetMaxIndexByDepartment(moduleBo.DepartmentId) + 1;
                }

                if (moduleBo.Editable)
                {
                    moduleBo.Controller = model.Name.ToPascalCase();
                    moduleBo.Name = model.Name?.Trim();
                }
                moduleBo.ReportPath = model.ReportPath?.Trim();
                moduleBo.UpdatedById = AuthUserId;

                TrailObject trail = GetNewTrailObject();
                Result = ModuleService.Update(ref moduleBo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        moduleBo.Id,
                        "Update Module"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = moduleBo.Id });
                }
                AddResult(Result);
            }
            ViewBag.DepartmentId = GetDepartmentList(moduleBo.DepartmentId);
            return View(model);
        }

        // GET: Module/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            ModuleBo moduleBo = ModuleService.Find(id);
            if (moduleBo == null)
            {
                return RedirectToAction("Index");
            }
            if (!moduleBo.Editable)
            {
                SetErrorSessionMsg(MessageBag.AccessDenied);
                return RedirectToAction("Index");
            }
            return View(new ModuleViewModel(moduleBo));
        }

        // POST: Module/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id)
        {
            ModuleBo moduleBo = ModuleService.Find(id);
            if (moduleBo == null)
            {
                return RedirectToAction("Index");
            }
            if (!moduleBo.Editable)
            {
                SetErrorSessionMsg(MessageBag.AccessDenied);
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = ModuleService.Delete(moduleBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    moduleBo.Id,
                    "Delete Module"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = moduleBo.Id });
        }

        public List<SelectListItem> GetDepartmentList(object selectedValue = null)
        {
            var query = _db.Departments.Select(q => new DepartmentBo { Id = q.Id, Name = q.Name, })
                .OrderBy(q => q.Name)
                .ToList();
            List<SelectListItem> list = new SelectList(query, "Id", "Name", selectedValue).ToList();
            list.Insert(0, (new SelectListItem { Text = "Please select", Value = "" }));
            return list;
        }
    }
}
