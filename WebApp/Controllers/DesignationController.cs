using BusinessObject.TreatyPricing;
using PagedList;
using Services.TreatyPricing;
using Shared;
using Shared.Trails;
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
    public class DesignationController : BaseController
    {
        public const string Controller = "Designation";

        // GET: Designation
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(string Code, string Description, string SortOrder, int? Page)
        {
            SetViewBagMessage();

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Code"] = Code,
                ["Description"] = Description,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCode = GetSortParam("Code");
            ViewBag.SortDescription = GetSortParam("Description");

            var query = _db.Designations.Select(DesignationViewModel.Expression());
            if (!string.IsNullOrEmpty(Code)) query = query.Where(q => q.Code.Contains(Code));
            if (!string.IsNullOrEmpty(Description)) query = query.Where(q => q.Description.Contains(Description));

            if (SortOrder == Html.GetSortAsc("Code")) query = query.OrderBy(q => q.Code);
            else if (SortOrder == Html.GetSortDsc("Code")) query = query.OrderByDescending(q => q.Code);
            else if (SortOrder == Html.GetSortAsc("Description")) query = query.OrderBy(q => q.Description);
            else if (SortOrder == Html.GetSortDsc("Description")) query = query.OrderByDescending(q => q.Description);
            else query = query.OrderBy(q => q.Code);

            ViewBag.Total = query.Count();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: Designation/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            SetViewBagMessage();
            return View(new DesignationViewModel());
        }

        // POST: Designation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(DesignationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);
                TrailObject trail = GetNewTrailObject();
                Result = DesignationService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Create Designation"
                    );

                    model.Id = bo.Id;

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            SetViewBagMessage();
            return View(model);
        }

        // GET: Designation/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            DesignationBo designationBo = DesignationService.Find(id);
            if (designationBo == null)
            {
                return RedirectToAction("Index");
            }

            SetViewBagMessage();
            return View(new DesignationViewModel(designationBo));
        }

        // POST: Designation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, DesignationViewModel model)
        {
            DesignationBo designationBo = DesignationService.Find(id);
            if (designationBo == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);
                TrailObject trail = GetNewTrailObject();
                Result = DesignationService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        designationBo.Id,
                        "Update Designation"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = designationBo.Id });
                }
                AddResult(Result);
            }

            SetViewBagMessage();
            return View(model);
        }

        // GET: Designation/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            DesignationBo designationBo = DesignationService.Find(id);
            if (designationBo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new DesignationViewModel(designationBo));
        }

        // POST: Designation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id)
        {
            DesignationBo designationBo = DesignationService.Find(id);
            if (designationBo == null)
            {
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = DesignationService.Delete(designationBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    designationBo.Id,
                    "Delete Designation"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = designationBo.Id });
        }
    }
}