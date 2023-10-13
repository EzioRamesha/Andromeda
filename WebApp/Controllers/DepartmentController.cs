using BusinessObject.Identity;
using PagedList;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class DepartmentController : BaseController
    {
        public const string Controller = "Department";

        // GET: Department
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(string Code, string Name, string HodUser, string SortOrder, int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Code"] = Code,
                ["Name"] = Name,
                ["HodUser"] = HodUser,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCode = GetSortParam("Code");
            ViewBag.SortName = GetSortParam("Name");

            var query = _db.Departments.Select(DepartmentViewModel.Expression());
            if (!string.IsNullOrEmpty(Code)) query = query.Where(q => q.Code.Contains(Code));
            if (!string.IsNullOrEmpty(Name)) query = query.Where(q => q.Name.Contains(Name));
            if (!string.IsNullOrEmpty(HodUser)) query = query.Where(q => q.HodUser.FullName.Contains(HodUser));

            if (SortOrder == Html.GetSortAsc("Code")) query = query.OrderBy(q => q.Code);
            else if (SortOrder == Html.GetSortDsc("Code")) query = query.OrderByDescending(q => q.Code);
            else if (SortOrder == Html.GetSortAsc("Name")) query = query.OrderBy(q => q.Name);
            else if (SortOrder == Html.GetSortDsc("Name")) query = query.OrderByDescending(q => q.Name);
            else query = query.OrderBy(q => q.Code);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: Department/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            return RedirectToAction("Index");
            //return View(new DepartmentViewModel());
        }

        // POST: Department/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, DepartmentViewModel model)
        {
            /*
            if (ModelState.IsValid)
            {
                var departmentBo = new DepartmentBo
                {
                    Code = model.Code,
                    Name = model.Name,
                    HodUserId = model.HodUserId,
                    CreatedById = AuthUserId,
                    UpdatedById = AuthUserId,
                };
            
                TrailObject trail = GetNewTrailObject();
                Result = DepartmentService.Create(ref departmentBo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        departmentBo.Id,
                        "Create Department"
                    );

                    model.Id = departmentBo.Id;

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = departmentBo.Id });
                }
                AddResult(Result);
            }
            return View(model);
            */
            return RedirectToAction("Index");
        }

        // GET: Department/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            DepartmentBo departmentBo = DepartmentService.Find(id);
            if (departmentBo == null)
            {
                return RedirectToAction("Index");
            }
            PageLoad(departmentBo);
            return View(new DepartmentViewModel(departmentBo));
        }

        // POST: Department/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, DepartmentViewModel model)
        {
            DepartmentBo departmentBo = DepartmentService.Find(id);
            if (departmentBo == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                departmentBo.Code = model.Code?.Trim();
                departmentBo.Name = model.Name?.Trim();
                departmentBo.HodUserId = model.HodUserId;
                departmentBo.UpdatedById = AuthUserId;

                TrailObject trail = GetNewTrailObject();
                Result = DepartmentService.Update(ref departmentBo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        departmentBo.Id,
                        "Update Department"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = departmentBo.Id });
                }
                AddResult(Result);
            }
            PageLoad(departmentBo);
            return View(model);
        }

        // GET: Department/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            return RedirectToAction("Index");
            /* SetViewBagMessage();
            DepartmentBo departmentBo = DepartmentService.Find(id);
            if (departmentBo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new DepartmentViewModel(departmentBo));*/
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, DepartmentViewModel model)
        {
            /*
            DepartmentBo departmentBo = DepartmentService.Find(id);
            if (departmentBo == null)
            {
                return RedirectToAction("Index");
            }
            
            TrailObject trail = GetNewTrailObject();
            Result = DepartmentService.Delete(departmentBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    departmentBo.Id,
                    "Delete Department"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }
            AddResult(Result);
            return RedirectToAction("Delete", new { id = departmentBo.Id });
            */
            return RedirectToAction("Index");
        }

        public void IndexPage()
        {
            //DropDownUser(exceptSuper: false);
            SetViewBagMessage();
        }

        public void PageLoad(DepartmentBo bo)
        {
            if (bo != null)
            {
                DropDownUser(departmentId: bo.Id);
            }
            SetViewBagMessage();
        }
    }
}
