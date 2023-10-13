using DataAccess.Entities.Retrocession;
using PagedList;
using Services.Retrocession;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class GstMaintenanceController : BaseController
    {
        public const string Controller = "GstMaintenance";

        // GET: GstMaintenance
        public ActionResult Index(
            string EffectiveStartDate,
            string EffectiveEndDate,
            string RiskEffectiveStartDate,
            string RiskEffectiveEndDate,
            double? Rate,
            string SortOrder,
            int? Page)
        {
            DateTime? effectiveStartDate = Util.GetParseDateTime(EffectiveStartDate);
            DateTime? effectiveEndDate = Util.GetParseDateTime(EffectiveEndDate);
            DateTime? riskEffectiveStartDate = Util.GetParseDateTime(RiskEffectiveStartDate);
            DateTime? riskEffectiveEndDate = Util.GetParseDateTime(RiskEffectiveEndDate);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["EffectiveStartDate"] = EffectiveStartDate,
                ["EffectiveEndDate"] = EffectiveEndDate,
                ["RiskEffectiveStartDate"] = RiskEffectiveStartDate,
                ["RiskEffectiveEndDate"] = RiskEffectiveEndDate,
                ["Rate"] = Rate,
                ["SortOrder"] = SortOrder
            };

            ViewBag.SortOrder = SortOrder;
            ViewBag.SortEffectiveStartDate = GetSortParam("EffectiveStartDate");
            ViewBag.SortEffectiveEndDate = GetSortParam("EffectiveEndDate");
            ViewBag.SortRiskEffectiveStartDate = GetSortParam("RiskEffectiveStartDate");
            ViewBag.SortRiskEffectiveEndDate = GetSortParam("RiskEffectiveEndDate");

            var query = _db.GstMaintenances.Select(GstMaintenanceViewModel.Expression());

            if (effectiveStartDate.HasValue) query = query.Where(q => q.EffectiveStartDate <= effectiveStartDate);
            if (effectiveEndDate.HasValue) query = query.Where(q => q.EffectiveEndDate >= effectiveEndDate);
            if (riskEffectiveStartDate.HasValue) query = query.Where(q => q.RiskEffectiveStartDate <= riskEffectiveStartDate);
            if (riskEffectiveEndDate.HasValue) query = query.Where(q => q.RiskEffectiveEndDate >= riskEffectiveEndDate);
            if (Rate.HasValue) query = query.Where(q => q.Rate == Rate);

            if (SortOrder == Html.GetSortAsc("EffectiveStartDate")) query = query.OrderBy(q => q.EffectiveStartDate);
            else if (SortOrder == Html.GetSortDsc("EffectiveStartDate")) query = query.OrderByDescending(q => q.EffectiveStartDate);
            else if (SortOrder == Html.GetSortAsc("EffectiveEndDate")) query = query.OrderBy(q => q.EffectiveEndDate);
            else if (SortOrder == Html.GetSortDsc("EffectiveEndDate")) query = query.OrderByDescending(q => q.EffectiveEndDate);
            else if (SortOrder == Html.GetSortAsc("RiskEffectiveStartDate")) query = query.OrderBy(q => q.RiskEffectiveStartDate);
            else if (SortOrder == Html.GetSortDsc("RiskEffectiveStartDate")) query = query.OrderByDescending(q => q.RiskEffectiveStartDate);
            else if (SortOrder == Html.GetSortAsc("RiskEffectiveEndDate")) query = query.OrderBy(q => q.RiskEffectiveEndDate);
            else if (SortOrder == Html.GetSortDsc("RiskEffectiveEndDate")) query = query.OrderByDescending(q => q.RiskEffectiveEndDate);
            else if (SortOrder == Html.GetSortAsc("Rate")) query = query.OrderBy(q => q.Rate);
            else if (SortOrder == Html.GetSortDsc("Rate")) query = query.OrderByDescending(q => q.Rate);
            else query = query.OrderBy(q => q.Id);

            ViewBag.Total = query.Count();
            IndexPage();

            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        public void IndexPage()
        {
            SetViewBagMessage();
        }

        // GET: GstMaintenance/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new GstMaintenanceViewModel());
        }

        // POST: GstMaintenance/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, GstMaintenanceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);
                var trail = GetNewTrailObject();
                Result = GstMaintenanceService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;

                    CreateTrail(
                        bo.Id,
                        "Create GST Maintenance"
                        );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }
            LoadPage();
            return View(model);
        }

        // GET: GstMaintenance/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = GstMaintenanceService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            LoadPage();
            return View(new GstMaintenanceViewModel(bo));
        }

        // POST: GstMaintenance/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, GstMaintenanceViewModel model)
        {
            var dbBo = GstMaintenanceService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();

                Result = GstMaintenanceService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Update GST Maintenance"
                        );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage();
            return View(model);
        }

        // GET: GstMaintenance/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = GstMaintenanceService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            return View(new GstMaintenanceViewModel(bo));
        }

        // POST: GstMaintenance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, GstMaintenanceViewModel model)
        {
            var bo = GstMaintenanceService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = GstMaintenanceService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete GST Maintenance"
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
            SetViewBagMessage();
        }
    }
}