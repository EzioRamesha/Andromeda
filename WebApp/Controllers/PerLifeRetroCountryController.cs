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
    public class PerLifeRetroCountryController : BaseController
    {
        public const string Controller = "PerLifeRetroCountry";

        // GET: PerLifeRetroCountry
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            int? TerritoryOfIssueCodePickListDetailId,
            string EffectiveStartDate,
            string EffectiveEndDate,
            string SortOrder,
            int? Page)
        {
            DateTime? effectiveStartDate = Util.GetParseDateTime(EffectiveStartDate);
            DateTime? effectiveEndDate = Util.GetParseDateTime(EffectiveEndDate);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["TerritoryOfIssueCodePickListDetailId"] = TerritoryOfIssueCodePickListDetailId,
                ["EffectiveStartDate"] = effectiveStartDate.HasValue ? EffectiveStartDate : null,
                ["EffectiveEndDate"] = effectiveEndDate.HasValue ? EffectiveEndDate : null,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortTerritoryOfIssueCodePickListDetailId = GetSortParam("TerritoryOfIssueCodePickListDetailId");
            ViewBag.SortEffectiveStartDate = GetSortParam("EffectiveStartDate");
            ViewBag.SortEffectiveEndDate = GetSortParam("EffectiveEndDate");

            var query = _db.PerLifeRetroCountries.Select(PerLifeRetroCountryViewModel.Expression());

            if (TerritoryOfIssueCodePickListDetailId.HasValue) query = query.Where(q => q.TerritoryOfIssueCodePickListDetailId == TerritoryOfIssueCodePickListDetailId);
            if (effectiveStartDate.HasValue) query = query.Where(q => q.EffectiveStartDate >= effectiveStartDate);
            if (effectiveEndDate.HasValue) query = query.Where(q => q.EffectiveEndDate <= effectiveEndDate);

            if (SortOrder == Html.GetSortAsc("TerritoryOfIssueCodePickListDetailId")) query = query.OrderBy(q => q.TerritoryOfIssueCodePickListDetail.Code);
            else if (SortOrder == Html.GetSortDsc("TerritoryOfIssueCodePickListDetailId")) query = query.OrderByDescending(q => q.TerritoryOfIssueCodePickListDetail.Code);
            else if (SortOrder == Html.GetSortAsc("EffectiveStartDate")) query = query.OrderBy(q => q.EffectiveStartDate);
            else if (SortOrder == Html.GetSortDsc("EffectiveStartDate")) query = query.OrderByDescending(q => q.EffectiveStartDate);
            else if (SortOrder == Html.GetSortAsc("EffectiveEndDate")) query = query.OrderBy(q => q.EffectiveEndDate);
            else if (SortOrder == Html.GetSortDsc("EffectiveEndDate")) query = query.OrderByDescending(q => q.EffectiveEndDate);
            else query = query.OrderBy(q => q.TerritoryOfIssueCodePickListDetail.Code);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: PerLifeRetroCountry/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new PerLifeRetroCountryViewModel());
        }

        // POST: PerLifeRetroCountry/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, PerLifeRetroCountryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);

                var trail = GetNewTrailObject();
                Result = PerLifeRetroCountryService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;

                    CreateTrail(
                        bo.Id,
                        "Create Per Life Retro Country"
                    );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage();
            return View(model);
        }

        // GET: PerLifeRetroCountry/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = PerLifeRetroCountryService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            LoadPage();
            return View(new PerLifeRetroCountryViewModel(bo));
        }

        // POST: PerLifeRetroCountry/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, PerLifeRetroCountryViewModel model)
        {
            var dbBo = PerLifeRetroCountryService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();

                Result = PerLifeRetroCountryService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Update Per Life Retro Country"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage();
            return View(model);
        }

        // GET: PerLifeRetroCountry/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = PerLifeRetroCountryService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            return View(new PerLifeRetroCountryViewModel(bo));
        }

        // POST: PerLifeRetroCountry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, PerLifeRetroCountryViewModel model)
        {
            var bo = PerLifeRetroCountryService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = PerLifeRetroCountryService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Per Life Retro Country"
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
            DropDownTerritoryOfIssueCode();

            SetViewBagMessage();
        }
        public void LoadPage()
        {
            //var entity = new PerLifeRetroCountry();
            //var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Country");
            //ViewBag.CountryMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 15;

            DropDownTerritoryOfIssueCode();

            SetViewBagMessage();
        }
    }
}