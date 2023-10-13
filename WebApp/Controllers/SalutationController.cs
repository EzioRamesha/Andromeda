using BusinessObject;
using PagedList;
using Services;
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
    public class SalutationController : BaseController
    {
        public const string Controller = "Salutation";

        // GET: Salutation
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(string Name, string SortOrder, int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Name"] = Name,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortName = GetSortParam("Name");

            var query = _db.Salutations.Select(SalutationViewModel.Expression());
            if (!string.IsNullOrEmpty(Name)) query = query.Where(q => q.Name == Name);

            if (SortOrder == Html.GetSortAsc("Name")) query = query.OrderBy(q => q.Name);
            else if (SortOrder == Html.GetSortDsc("Name")) query = query.OrderByDescending(q => q.Name);
            else query = query.OrderBy(q => q.Name);

            ViewBag.Total = query.Count();
            SetViewBagMessage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: Salutation/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            SetViewBagMessage();
            return View(new SalutationViewModel());
        }

        // POST: Salutation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(SalutationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var salutationBo = new SalutationBo
                {
                    Name = model.Name?.Trim(),
                    CreatedById = AuthUserId,
                    UpdatedById = AuthUserId,
                };

                TrailObject trail = GetNewTrailObject();
                Result = SalutationService.Create(ref salutationBo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        salutationBo.Id,
                        "Create Salutation"
                    );

                    model.Id = salutationBo.Id;

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = salutationBo.Id });
                }
                AddResult(Result);
            }
            SetViewBagMessage();
            return View(model);
        }

        // GET: Salutation/Edit/5
        public ActionResult Edit(int id)
        {
            SetViewBagMessage();
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            SalutationBo salutationBo = SalutationService.Find(id);
            if (salutationBo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new SalutationViewModel(salutationBo));
        }

        // POST: Salutation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, SalutationViewModel model)
        {
            SalutationBo salutationBo = SalutationService.Find(id);
            if (salutationBo == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                salutationBo.Name = model.Name?.Trim();
                salutationBo.UpdatedById = AuthUserId;

                TrailObject trail = GetNewTrailObject();
                Result = SalutationService.Update(ref salutationBo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        salutationBo.Id,
                        "Update Salutation"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = salutationBo.Id });
                }
                AddResult(Result);
            }
            SetViewBagMessage();
            return View(model);
        }

        // GET: Salutation/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            SalutationBo salutationBo = SalutationService.Find(id);
            if (salutationBo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new SalutationViewModel(salutationBo));
        }

        // POST: Salutation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id)
        {
            SalutationBo salutationBo = SalutationService.Find(id);
            if (salutationBo == null)
            {
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = SalutationService.Delete(salutationBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    salutationBo.Id,
                    "Delete Salutation"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = salutationBo.Id });
        }
    }
}