using BusinessObject;
using BusinessObject.Identity;
using PagedList;
using Services;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class CedantWorkgroupController : BaseController
    {
        public const string Controller = "CedantWorkgroup";

        // GET: CedantWorkgroup
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string Code,
            string Description,
            string SortOrder,
            int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Code"] = Code,
                ["Description"] = Description,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCode = GetSortParam("Code");
            ViewBag.SortDescription = GetSortParam("Description");

            var query = _db.CedantWorkgroups.Select(CedantWorkgroupViewModel.Expression());
            if (!string.IsNullOrEmpty(Code)) query = query.Where(q => q.Code.Contains(Code));
            if (!string.IsNullOrEmpty(Description)) query = query.Where(q => q.Description.Contains(Description));

            if (SortOrder == Html.GetSortAsc("Code")) query = query.OrderBy(q => q.Code);
            else if (SortOrder == Html.GetSortDsc("Code")) query = query.OrderByDescending(q => q.Code);
            else if (SortOrder == Html.GetSortAsc("Description")) query = query.OrderBy(q => q.Description);
            else if (SortOrder == Html.GetSortDsc("Description")) query = query.OrderByDescending(q => q.Description);
            else query = query.OrderByDescending(q => q.Code);

            ViewBag.Total = query.Count();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: CedantWorkgroup/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            CedantWorkgroupViewModel model = new CedantWorkgroupViewModel();
            LoadPage(model);
            return View(model);
        }

        // POST: CedantWorkgroup/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, CedantWorkgroupViewModel model)
        {
            model.GetCedants(form);
            model.GetUsers(form);
            if (ModelState.IsValid)
            {
                var cedantWorkgroupBo = new CedantWorkgroupBo();
                model.Get(ref cedantWorkgroupBo);
                cedantWorkgroupBo.CreatedById = AuthUserId;
                cedantWorkgroupBo.UpdatedById = AuthUserId;

                TrailObject trail = GetNewTrailObject();
                Result = CedantWorkgroupService.Create(ref cedantWorkgroupBo, ref trail);
                if (Result.Valid)
                {
                    model.Id = cedantWorkgroupBo.Id;
                    model.ProcessCedants(ref trail);
                    model.ProcessUsers(ref trail);

                    CreateTrail(
                        cedantWorkgroupBo.Id,
                        "Create Cedant Workgroup"
                    );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { cedantWorkgroupBo.Id });
                }

                AddResult(Result);
            }

            LoadPage(model, true);
            return View(model);
        }

        // GET: CedantWorkgroup/Edit
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            CedantWorkgroupBo cedantWorkgroupBo = CedantWorkgroupService.Find(id);
            if (cedantWorkgroupBo == null)
            {
                return RedirectToAction("Index");
            }
            CedantWorkgroupViewModel model = new CedantWorkgroupViewModel(cedantWorkgroupBo);
            LoadPage(model);
            return View(model);
        }

        // POST: CedantWorkgroup/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, CedantWorkgroupViewModel model)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            CedantWorkgroupBo cedantWorkgroupBo = CedantWorkgroupService.Find(id);
            if (cedantWorkgroupBo == null)
            {
                return RedirectToAction("Index");
            }

            model.GetCedants(form);
            model.GetUsers(form);
            if (ModelState.IsValid)
            {
                model.Get(ref cedantWorkgroupBo);
                cedantWorkgroupBo.UpdatedById = AuthUserId;

                TrailObject trail = GetNewTrailObject();
                Result = CedantWorkgroupService.Update(ref cedantWorkgroupBo, ref trail);
                if (Result.Valid)
                {
                    model.ProcessCedants(ref trail);
                    model.ProcessUsers(ref trail);

                    CreateTrail(
                        cedantWorkgroupBo.Id,
                        "Create Cedant Workgroup"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { Id = id });
                }

                AddResult(Result);
            }
            LoadPage(model, true);
            return View(model);
        }

        // GET: CedantWorkgroup/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            CedantWorkgroupBo cedantWorkgroupBo = CedantWorkgroupService.Find(id);
            if (cedantWorkgroupBo == null)
            {
                return RedirectToAction("Index");
            }
            CedantWorkgroupViewModel model = new CedantWorkgroupViewModel(cedantWorkgroupBo);
            LoadPage(model);
            return View(model);
        }

        // POST: CedantWorkgroup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, CedantWorkgroupViewModel model)
        {
            CedantWorkgroupBo cedantWorkgroupBo = CedantWorkgroupService.Find(id);
            if (cedantWorkgroupBo == null)
            {
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = CedantWorkgroupService.Delete(cedantWorkgroupBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    cedantWorkgroupBo.Id,
                    "Delete Cedant Workgroup"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = cedantWorkgroupBo.Id });
        }

        public void LoadPage(CedantWorkgroupViewModel model, bool getChild = false)
        {
            ViewBag.CedantBos = CedantService.GetByStatus(CedantBo.StatusActive);
            List<int> selectedUserIds = null;

            IList<CedantWorkgroupCedantBo> cedantWorkgroupCedantBos = new List<CedantWorkgroupCedantBo>();
            IList<CedantWorkgroupUserBo> cedantWorkgroupUserBos = new List<CedantWorkgroupUserBo>();
            if (getChild)
            {
                cedantWorkgroupCedantBos = (IList<CedantWorkgroupCedantBo>)model.GetChildBos("CedantWorkgroupCedant");
                cedantWorkgroupUserBos = (IList<CedantWorkgroupUserBo>)model.GetChildBos("CedantWorkgroupUser");
            }
            else if (model.Id != 0)
            {
                cedantWorkgroupCedantBos = CedantWorkgroupCedantService.GetByCedantWorkgroupId(model.Id);
                cedantWorkgroupUserBos = CedantWorkgroupUserService.GetByCedantWorkgroupId(model.Id);
                selectedUserIds = cedantWorkgroupUserBos.Select(u => u.UserId).ToList();
            }

            ViewBag.UserBos = UserService.GetByStatus(UserBo.StatusActive, selectedIds: selectedUserIds, departmentId: DepartmentBo.DepartmentDataAnalyticsAdministration);
            ViewBag.CedantWorkgroupCedantBos = cedantWorkgroupCedantBos;
            ViewBag.CedantWorkgroupUserBos = cedantWorkgroupUserBos;

            SetViewBagMessage();
        }
    }
}