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
using DataAccess.Entities;
using DataAccess.EntityFramework;
using PagedList;
using Services;
using Shared;
using Shared.DataAccess;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class ClaimChecklistController : BaseController
    {
        public const string Controller = "ClaimChecklist";

        // GET: ClaimChecklist
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            int? ClaimCodeId,
            string SortOrder,
            int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["ClaimCodeId"] = ClaimCodeId,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortClaimCodeId = GetSortParam("ClaimCodeId");

            var query = _db.ClaimChecklists.Select(ClaimChecklistViewModel.Expression());

            if (ClaimCodeId.HasValue) query = query.Where(q => q.ClaimCodeId == ClaimCodeId);

            if (SortOrder == Html.GetSortAsc("ClaimCodeId")) query = query.OrderBy(q => q.ClaimCode.Code);
            else if (SortOrder == Html.GetSortDsc("ClaimCodeId")) query = query.OrderByDescending(q => q.ClaimCode.Code);
            else query = query.OrderByDescending(q => q.Id);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: ClaimChecklist/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            ClaimChecklistViewModel model = new ClaimChecklistViewModel();

            LoadPage(model);
            ViewBag.Disabled = false;
            return View(model);
        }

        // POST: ClaimChecklist/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, ClaimChecklistViewModel model)
        {
            Result = ClaimChecklistService.Result();
            Result childResult = new Result();
            List<ClaimChecklistDetailBo> claimChecklistDetailBos = model.GetClaimChecklistDetails(form, ref childResult);

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);

                if (childResult.Valid)
                {
                    model.ValidateDuplicate(claimChecklistDetailBos, ref childResult);

                    if (childResult.Valid)
                    {
                        var trail = GetNewTrailObject();
                        Result = ClaimChecklistService.Create(ref bo, ref trail);
                        if (Result.Valid)
                        {
                            model.Id = bo.Id;
                            model.ProcessClaimChecklistDetails(claimChecklistDetailBos, AuthUserId, ref trail);

                            CreateTrail(
                                bo.Id,
                                "Create Claim Checklist"
                            );

                            SetCreateSuccessMessage(Controller);
                            return RedirectToAction("Edit", new { id = bo.Id });
                        }
                    }
                }
                if (!childResult.Valid)
                    Result.AddErrorRange(childResult.ToErrorArray());
                AddResult(Result);
            }

            LoadPage(model, claimChecklistDetailBos);
            ViewBag.Disabled = false;
            return View(model);
        }

        // GET: ClaimChecklist/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = ClaimChecklistService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            ClaimChecklistViewModel model = new ClaimChecklistViewModel(bo);

            LoadPage(model);
            ViewBag.Disabled = false;
            return View(model);
        }

        // POST: ClaimChecklist/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, ClaimChecklistViewModel model)
        {
            var dbBo = ClaimChecklistService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            Result = ClaimChecklistService.Result();
            Result childResult = new Result();
            List<ClaimChecklistDetailBo> claimChecklistDetailBos = model.GetClaimChecklistDetails(form, ref childResult);

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();

                if (childResult.Valid)
                {
                    model.ValidateDuplicate(claimChecklistDetailBos, ref childResult);
                    if (childResult.Valid)
                    {
                        model.ProcessClaimChecklistDetails(claimChecklistDetailBos, AuthUserId, ref trail);
                        Result = ClaimChecklistService.Update(ref bo, ref trail);
                        if (Result.Valid)
                        {
                            CreateTrail(
                                bo.Id,
                                "Update Claim Checklist"
                            );

                            SetUpdateSuccessMessage(Controller);
                            return RedirectToAction("Edit", new { id = bo.Id });
                        }
                    }
                }
                if (!childResult.Valid)
                    Result.AddErrorRange(childResult.ToErrorArray());
                AddResult(Result);
            }

            LoadPage(model, claimChecklistDetailBos);
            ViewBag.Disabled = false;
            return View(model);
        }

        // GET: ClaimChecklist/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = ClaimChecklistService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }
            ClaimChecklistViewModel model = new ClaimChecklistViewModel(bo);
            LoadPage(model);
            ViewBag.Disabled = true;
            return View(new ClaimChecklistViewModel(bo));
        }

        // POST: ClaimChecklist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, ClaimChecklistViewModel model)
        {
            var bo = ClaimChecklistService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = ClaimChecklistService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Claim Checklist"
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
            DropDownClaimCode();
            SetViewBagMessage();
        }

        public void LoadPage(ClaimChecklistViewModel model, List<ClaimChecklistDetailBo> claimChecklistDetailBos = null)
        {
            if (model.Id == 0)
            {
                // Create
                DropDownClaimCode(ClaimCodeBo.StatusActive);
            }
            else
            {
                DropDownClaimCode(ClaimCodeBo.StatusActive, model.ClaimCodeId);

                if (model.ClaimCodeBo != null && model.ClaimCodeBo.Status == ClaimCodeBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.ClaimCodeStatusInactive);
                }

                if (claimChecklistDetailBos == null || claimChecklistDetailBos.Count == 0)
                {
                    claimChecklistDetailBos = ClaimChecklistDetailService.GetByClaimChecklistId(model.Id).ToList();
                }
            }

            ViewBag.ClaimChecklistDetailBos = claimChecklistDetailBos;
            SetViewBagMessage();
        }
    }
}
