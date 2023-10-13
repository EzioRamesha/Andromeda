using DataAccess.Entities.Retrocession;
using PagedList;
using Services.Retrocession;
using Shared;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class PerLifeRetroGenderController : BaseController
    {
        public const string Controller = "PerLifeRetroGender";

        // GET: PerLifeRetroGender
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            int? InsuredGenderCodePickListDetailId,
            string EffectiveStartDate,
            string EffectiveEndDate,
            string SortOrder,
            int? Page)
        {
            DateTime? effectiveStartDate = Util.GetParseDateTime(EffectiveStartDate);
            DateTime? effectiveEndDate = Util.GetParseDateTime(EffectiveEndDate);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["InsuredGenderCodePickListDetailId"] = InsuredGenderCodePickListDetailId,
                ["EffectiveStartDate"] = effectiveStartDate.HasValue ? EffectiveStartDate : null,
                ["EffectiveEndDate"] = effectiveEndDate.HasValue ? EffectiveEndDate : null,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortGender = GetSortParam("Gender");
            ViewBag.SortEffectiveStartDate = GetSortParam("EffectiveStartDate");
            ViewBag.SortEffectiveEndDate = GetSortParam("EffectiveEndDate");

            var query = _db.PerLifeRetroGenders.Select(PerLifeRetroGenderViewModel.Expression());

            if (InsuredGenderCodePickListDetailId.HasValue) query = query.Where(q => q.InsuredGenderCodePickListDetailId == InsuredGenderCodePickListDetailId);
            if (effectiveStartDate.HasValue) query = query.Where(q => q.EffectiveStartDate >= effectiveStartDate);
            if (effectiveEndDate.HasValue) query = query.Where(q => q.EffectiveEndDate <= effectiveEndDate);

            if (SortOrder == Html.GetSortAsc("InsuredGenderCodePickListDetailId")) query = query.OrderBy(q => q.InsuredGenderCodePickListDetail.Code);
            else if (SortOrder == Html.GetSortDsc("InsuredGenderCodePickListDetailId")) query = query.OrderByDescending(q => q.InsuredGenderCodePickListDetail.Code);
            else if (SortOrder == Html.GetSortAsc("EffectiveStartDate")) query = query.OrderBy(q => q.EffectiveStartDate);
            else if (SortOrder == Html.GetSortDsc("EffectiveStartDate")) query = query.OrderByDescending(q => q.EffectiveStartDate);
            else if (SortOrder == Html.GetSortAsc("EffectiveEndDate")) query = query.OrderBy(q => q.EffectiveEndDate);
            else if (SortOrder == Html.GetSortDsc("EffectiveEndDate")) query = query.OrderByDescending(q => q.EffectiveEndDate);
            else query = query.OrderBy(q => q.InsuredGenderCodePickListDetail.Code);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: PerLifeRetroGender/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new PerLifeRetroGenderViewModel());
        }

        // POST: PerLifeRetroGender/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, PerLifeRetroGenderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);

                var trail = GetNewTrailObject();
                Result = PerLifeRetroGenderService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;

                    CreateTrail(
                        bo.Id,
                        "Create Per Life Retro Gender"
                    );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage();
            return View(model);
        }

        // GET: PerLifeRetroGender/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = PerLifeRetroGenderService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            LoadPage();
            return View(new PerLifeRetroGenderViewModel(bo));
        }

        // POST: PerLifeRetroGender/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, PerLifeRetroGenderViewModel model)
        {
            var dbBo = PerLifeRetroGenderService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();

                Result = PerLifeRetroGenderService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Update Per Life Retro Gender"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage();
            return View(model);
        }

        // GET: PerLifeRetroGender/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = PerLifeRetroGenderService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            return View(new PerLifeRetroGenderViewModel(bo));
        }

        // POST: PerLifeRetroGender/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, PerLifeRetroGenderViewModel model)
        {
            var bo = PerLifeRetroGenderService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = PerLifeRetroGenderService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Per Life Retro Gender"
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
            DropDownInsuredGenderCode();
            SetViewBagMessage();
        }

        public void LoadPage()
        {
            var entity = new PerLifeRetroGender();
            //var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Gender");
            //ViewBag.GenderMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 15;

            DropDownInsuredGenderCode();

            SetViewBagMessage();
        }
    }
}