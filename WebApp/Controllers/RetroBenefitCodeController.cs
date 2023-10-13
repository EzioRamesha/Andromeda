using BusinessObject.Retrocession;
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
    public class RetroBenefitCodeController : BaseController
    {
        public const string Controller = "RetroBenefitCode";

        // GET: RetroBenefitCode
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string Code,
            string Description,
            string EffectiveDate,
            string CeaseDate,
            int? Status,
            string Remarks,
            string SortOrder,
            int? Page)
        {
            DateTime? effectiveDate = Util.GetParseDateTime(EffectiveDate);
            DateTime? ceaseDate = Util.GetParseDateTime(CeaseDate);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Code"] = Code,
                ["Description"] = Description,
                ["EffectiveDate"] = effectiveDate.HasValue ? EffectiveDate : null,
                ["CeaseDate"] = ceaseDate.HasValue ? ceaseDate : null,
                ["Status"] = Status,
                ["Remarks"] = Remarks,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCode = GetSortParam("Code");
            ViewBag.SortDescription = GetSortParam("Description");
            ViewBag.SortEffectiveDate = GetSortParam("EffectiveDate");
            ViewBag.SortCeaseDate = GetSortParam("CeaseDate");
            ViewBag.SortStatus = GetSortParam("Status");
            ViewBag.SortRemarks = GetSortParam("Remarks");

            var query = _db.RetroBenefitCodes.Select(RetroBenefitCodeViewModel.Expression());

            if (!string.IsNullOrEmpty(Code)) query = query.Where(q => q.Code.Contains(Code));
            if (!string.IsNullOrEmpty(Description)) query = query.Where(q => q.Description.Contains(Description));
            if (effectiveDate.HasValue) query = query.Where(q => q.EffectiveDate >= effectiveDate);
            if (ceaseDate.HasValue) query = query.Where(q => q.CeaseDate <= ceaseDate);
            if (Status.HasValue) query = query.Where(q => q.Status == Status);
            if (!string.IsNullOrEmpty(Remarks)) query = query.Where(q => q.Remarks.Contains(Remarks));

            if (SortOrder == Html.GetSortAsc("Code")) query = query.OrderBy(q => q.Code);
            else if (SortOrder == Html.GetSortDsc("Code")) query = query.OrderByDescending(q => q.Code);
            else if (SortOrder == Html.GetSortAsc("Description")) query = query.OrderBy(q => q.Description);
            else if (SortOrder == Html.GetSortDsc("Description")) query = query.OrderByDescending(q => q.Description);
            else if (SortOrder == Html.GetSortAsc("EffectiveDate")) query = query.OrderBy(q => q.EffectiveDate);
            else if (SortOrder == Html.GetSortDsc("EffectiveDate")) query = query.OrderByDescending(q => q.EffectiveDate);
            else if (SortOrder == Html.GetSortAsc("CeaseDate")) query = query.OrderBy(q => q.CeaseDate);
            else if (SortOrder == Html.GetSortDsc("CeaseDate")) query = query.OrderByDescending(q => q.CeaseDate);
            else if (SortOrder == Html.GetSortAsc("Status")) query = query.OrderBy(q => q.Status);
            else if (SortOrder == Html.GetSortDsc("Status")) query = query.OrderByDescending(q => q.Status);
            else if (SortOrder == Html.GetSortAsc("Remarks")) query = query.OrderBy(q => q.Remarks);
            else if (SortOrder == Html.GetSortDsc("Remarks")) query = query.OrderByDescending(q => q.Remarks);
            else query = query.OrderBy(q => q.Code);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: RetroBenefitCode/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new RetroBenefitCodeViewModel());
        }

        // POST: RetroBenefitCode/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, RetroBenefitCodeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);

                var trail = GetNewTrailObject();
                Result = RetroBenefitCodeService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;

                    CreateTrail(
                        bo.Id,
                        "Create Retro Benefit Code"
                    );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage();
            return View(model);
        }

        // GET: RetroBenefitCode/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = RetroBenefitCodeService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            LoadPage();
            return View(new RetroBenefitCodeViewModel(bo));
        }

        // POST: RetroBenefitCode/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, RetroBenefitCodeViewModel model)
        {
            var dbBo = RetroBenefitCodeService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();

                Result = RetroBenefitCodeService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Update Retro Benefit Code"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage();
            return View(model);
        }

        // GET: RetroBenefitCode/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = RetroBenefitCodeService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            return View(new RetroBenefitCodeViewModel(bo));
        }

        // POST: RetroBenefitCode/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, RetroBenefitCodeViewModel model)
        {
            var bo = RetroBenefitCodeService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = RetroBenefitCodeService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Retro Benefit Code"
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
            DropDownStatus();

            SetViewBagMessage();
        }
        public void LoadPage()
        {
            DropDownStatus();

            var entity = new RetroBenefitCode();
            var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Code");
            ViewBag.CodeMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 30;

            maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Description");
            ViewBag.DescriptionMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

            maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Remarks");
            ViewBag.RemarksMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

            SetViewBagMessage();
        }

        public void DropDownStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= RetroBenefitCodeBo.StatusMax; i++)
            {
                items.Add(new SelectListItem { Text = RetroBenefitCodeBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownStatuses = items;
        }
    }
}