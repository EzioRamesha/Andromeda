using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BusinessObject;
using DataAccess.EntityFramework;
using PagedList;
using Services;
using Shared;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class ClaimReasonController : BaseController
    {
        public const string Controller = "ClaimReason";

        // GET: ClaimReason
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            int? Type,
            string Reason,
            string Remark,
            string SortOrder,
            int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Type"] = Type,
                ["Reason"] = Reason,
                ["Remark"] = Remark,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortType = GetSortParam("Type");
            ViewBag.SortReason = GetSortParam("Reason");

            var query = _db.ClaimReasons.Select(ClaimReasonViewModel.Expression());

            if (Type.HasValue) query = query.Where(q => q.Type == Type);
            if (!string.IsNullOrEmpty(Reason)) query = query.Where(q => q.Reason.Contains(Reason));
            if (!string.IsNullOrEmpty(Remark)) query = query.Where(q => q.Remark.Contains(Remark));

            if (SortOrder == Html.GetSortAsc("Type")) query = query.OrderBy(q => q.Type);
            else if (SortOrder == Html.GetSortDsc("Type")) query = query.OrderByDescending(q => q.Type);
            else if (SortOrder == Html.GetSortAsc("Reason")) query = query.OrderBy(q => q.Reason);
            else if (SortOrder == Html.GetSortDsc("Reason")) query = query.OrderByDescending(q => q.Reason);
            else query = query.OrderBy(q => q.Type).ThenBy(q => q.Reason);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: ClaimReason/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new ClaimReasonViewModel());
        }

        // POST: ClaimReason/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, ClaimReasonViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);

                var trail = GetNewTrailObject();
                Result = ClaimReasonService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;

                    CreateTrail(
                        bo.Id,
                        "Create Claim Reason"
                    );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage();
            return View(model);
        }

        // GET: ClaimReason/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = ClaimReasonService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            LoadPage(bo);
            return View(new ClaimReasonViewModel(bo));
        }

        // POST: ClaimReason/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, ClaimReasonViewModel model)
        {
            var dbBo = ClaimReasonService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();

                Result = ClaimReasonService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Update Claim Reason"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage(dbBo);
            return View(model);
        }

        // GET: ClaimReason/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = ClaimReasonService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            return View(new ClaimReasonViewModel(bo));
        }

        // POST: ClaimReason/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, ClaimReasonViewModel model)
        {
            var bo = ClaimReasonService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = ClaimReasonService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Claim Reason"
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

        public void IndexPage()
        {
            DropDownEmpty();
            DropDownType();
            SetViewBagMessage();
        }

        public void LoadPage(ClaimReasonBo bo = null)
        {
            DropDownType();
            if (bo == null)
            {
                // Create
            }
            else
            {
                // Edit
            }
            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownType()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= ClaimReasonBo.MaxType; i++)
            {
                items.Add(new SelectListItem { Text = ClaimReasonBo.GetTypeName(i), Value = i.ToString() });
            }
            ViewBag.DropDownTypes = items;
            return items;
        }
    }
}
