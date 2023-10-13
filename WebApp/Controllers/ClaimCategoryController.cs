using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using PagedList;
using Services;
using Shared;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class ClaimCategoryController : BaseController
    {
        public const string Controller = "ClaimCategory";

        // GET: ClaimCategory
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string Category,
            string Remark,
            string SortOrder,
            int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Category"] = Category,
                ["Remark"] = Remark,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCategory = GetSortParam("Category");

            var query = _db.ClaimCategories.Select(ClaimCategoryViewModel.Expression());

            if (!string.IsNullOrEmpty(Category)) query = query.Where(q => q.Category.Contains(Category));
            if (!string.IsNullOrEmpty(Remark)) query = query.Where(q => q.Remark.Contains(Remark));

            if (SortOrder == Html.GetSortAsc("Category")) query = query.OrderBy(q => q.Category);
            else if (SortOrder == Html.GetSortDsc("Category")) query = query.OrderByDescending(q => q.Category);
            else query = query.OrderBy(q => q.Category);

            ViewBag.Total = query.Count();
            SetViewBagMessage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: ClaimCategory/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new ClaimCategoryViewModel());
        }

        // POST: ClaimCategory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, ClaimCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);

                var trail = GetNewTrailObject();
                Result = ClaimCategoryService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;

                    CreateTrail(
                        bo.Id,
                        "Create Claim Category"
                    );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage();
            return View(model);
        }

        // GET: ClaimCategory/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = ClaimCategoryService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            LoadPage();
            return View(new ClaimCategoryViewModel(bo));
        }

        // POST: ClaimCategory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, ClaimCategoryViewModel model)
        {
            var dbBo = ClaimCategoryService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();

                Result = ClaimCategoryService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Update Claim Category"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage();
            return View(model);
        }

        // GET: ClaimCategory/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = ClaimCategoryService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            return View(new ClaimCategoryViewModel(bo));
        }

        // POST: ClaimCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, ClaimCategoryViewModel model)
        {
            var bo = ClaimCategoryService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = ClaimCategoryService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Claim Category"
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

        public void LoadPage()
        {
            var entity = new ClaimCategory();
            var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Category");
            ViewBag.CategoryMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

            maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Remark");
            ViewBag.RemarkMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

            SetViewBagMessage();
        }
    }
}
