using BusinessObject;
using PagedList;
using Services;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class ClaimAuthorityLimitCedantController : BaseController
    {
        public const string Controller = "ClaimAuthorityLimitCedant";

        // GET: ClaimAuthorityLimitCedant
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(int? CedantId, string Remarks, string SortOrder, int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["CedantId"] = CedantId,
                ["Remarks"] = Remarks,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCedantId = GetSortParam("CedantId");

            var query = _db.ClaimAuthorityLimitCedants.Select(ClaimAuthorityLimitCedantViewModel.Expression());
            if (!string.IsNullOrEmpty(Remarks)) query = query.Where(q => q.Remarks.Contains(Remarks));
            if (CedantId != null) query = query.Where(q => q.CedantId == CedantId);

            if (SortOrder == Html.GetSortAsc("CedantId")) query = query.OrderBy(q => q.Cedant.Code).ThenBy(q => q.Cedant.Name);
            else if (SortOrder == Html.GetSortDsc("CedantId")) query = query.OrderByDescending(q => q.Cedant.Code).ThenByDescending(q => q.Cedant.Name);
            else query = query.OrderBy(q => q.Cedant.Code).ThenBy(q => q.Cedant.Name);

            ViewBag.Total = query.Count();
            ViewBag.DistinctClaimCode = ClaimAuthorityLimitCedantDetailService.GetDistinctClaimCodeForClaimAuthorityLimitCedant();
            DropDownCedant();
            SetViewBagMessage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: ClaimAuthorityLimitCedant/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new ClaimAuthorityLimitCedantViewModel());
        }

        // POST: ClaimAuthorityLimitCedant/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, ClaimAuthorityLimitCedantViewModel model)
        {
            var calCedantDetailBos = model.GetClaimAuthorityLimitCedantDetails(form);

            if (ModelState.IsValid)
            {
                Result childResult = ClaimAuthorityLimitCedantDetailService.Validate(calCedantDetailBos);
                if (!childResult.Valid)
                {
                    AddResult(childResult);
                    LoadPage(calDetailBos: calCedantDetailBos);
                    return View(model);
                }

                var bo = model.FormBo(AuthUserId, AuthUserId);
                TrailObject trail = GetNewTrailObject();
                Result = ClaimAuthorityLimitCedantService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;
                    model.ProcessClaimAuthorityLimitCedantDetails(calCedantDetailBos, AuthUserId, ref trail);

                    CreateTrail(bo.Id, "Create Claim Authority Limit - Cedant");

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }
            LoadPage(calDetailBos: calCedantDetailBos);
            return View(model);
        }

        // GET: ClaimAuthorityLimitCedant/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            ClaimAuthorityLimitCedantBo calCedantBo = ClaimAuthorityLimitCedantService.Find(id);
            if (calCedantBo == null)
            {
                return RedirectToAction("Index");
            }

            var calCedantDetailBos = ClaimAuthorityLimitCedantDetailService.GetByClaimAuthorityLimitCedantId(calCedantBo.Id);
            LoadPage(calCedantBo, calCedantDetailBos);
            return View(new ClaimAuthorityLimitCedantViewModel(calCedantBo));
        }

        // POST: ClaimAuthorityLimitCedant/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, ClaimAuthorityLimitCedantViewModel model)
        {
            var calCedantDetailBos = model.GetClaimAuthorityLimitCedantDetails(form);

            ClaimAuthorityLimitCedantBo calCedantBo = ClaimAuthorityLimitCedantService.Find(id);
            if (calCedantBo == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                Result childResult = ClaimAuthorityLimitCedantDetailService.Validate(calCedantDetailBos);
                if (!childResult.Valid)
                {
                    AddResult(childResult);
                    LoadPage(calCedantBo, calCedantDetailBos);
                    return View(model);
                }

                var bo = model.FormBo(AuthUserId, AuthUserId);
                TrailObject trail = GetNewTrailObject();
                Result = ClaimAuthorityLimitCedantService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.ProcessClaimAuthorityLimitCedantDetails(calCedantDetailBos, AuthUserId, ref trail);

                    CreateTrail(calCedantBo.Id, "Update Claim Authority Limit - Cedant");

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = calCedantBo.Id });
                }
                AddResult(Result);
            }
            LoadPage(calCedantBo, calCedantDetailBos);
            return View(model);
        }

        // GET: ClaimAuthorityLimitCedant/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            ClaimAuthorityLimitCedantBo calCedantBo = ClaimAuthorityLimitCedantService.Find(id);
            if (calCedantBo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new ClaimAuthorityLimitCedantViewModel(calCedantBo));
        }

        // POST: ClaimAuthorityLimitCedant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id)
        {
            ClaimAuthorityLimitCedantBo calCedantBo = ClaimAuthorityLimitCedantService.Find(id);
            if (calCedantBo == null)
            {
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = ClaimAuthorityLimitCedantService.Delete(calCedantBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(calCedantBo.Id, "Delete Claim Authority Limit - Cedant");

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = calCedantBo.Id });
        }

        public void LoadPage(ClaimAuthorityLimitCedantBo calCedantBo = null, IList<ClaimAuthorityLimitCedantDetailBo> calDetailBos = null)
        {
            DropDownClaimCode(ClaimCodeBo.StatusActive);
            DropDownCalCedantType();
            DropDownFundsAccountingTypeCode();
            if (calCedantBo == null)
            {
                DropDownCedant(CedantBo.StatusActive);
                if (calDetailBos == null) ViewBag.CALCedantDetailBos = Array.Empty<ClaimAuthorityLimitCedantDetailBo>(); 
                else ViewBag.CALCedantDetailBos = calDetailBos;
            }
            else 
            { 
                DropDownCedant(CedantBo.StatusActive, calCedantBo.CedantId);
                if (calCedantBo.CedantBo.Status == CedantBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.CedantStatusInactive);
                }

                foreach (var calDetailBo in calDetailBos)
                {
                    if (calDetailBo.ClaimCodeBo != null && calDetailBo.ClaimCodeBo.Status == ClaimCodeBo.StatusInactive)
                    {
                        AddWarningMsg(string.Format(MessageBag.ClaimCodeStatusInactiveWithCode, calDetailBo.ClaimCodeBo.Code));
                    }
                }
                ViewBag.CALCedantDetailBos = calDetailBos;
            }
            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownCalCedantType()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= ClaimAuthorityLimitCedantDetailBo.MaxType; i++)
            {
                items.Add(new SelectListItem { Text = ClaimAuthorityLimitCedantDetailBo.GetTypeName(i), Value = i.ToString() });
            }
            ViewBag.DropDownCalCedantTypes = items;
            return items;
        }

        [HttpPost]
        public JsonResult GetClaimCodes(int? claimCodeId = null)
        {
            IList<SelectListItem> claimCodes = DropDownClaimCode(ClaimCodeBo.StatusActive, claimCodeId);
            return Json(new { claimCodes });
        }
    }
}