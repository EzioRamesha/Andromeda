using BusinessObject;
using BusinessObject.Identity;
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
    public class ClaimAuthorityLimitMLReController : BaseController
    {
        public const string Controller = "ClaimAuthorityLimitMLRe";

        // GET: ClaimAuthorityLimitMLRe
        public ActionResult Index(int? UserId, string Position, string SortOrder, int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["UserId"] = UserId,
                ["Position"] = Position,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortUserId = GetSortParam("UserId");
            ViewBag.SortPosition = GetSortParam("Position");

            var query = _db.ClaimAuthorityLimitMLRe.Select(ClaimAuthorityLimitMLReViewModel.Expression());
            if (!string.IsNullOrEmpty(Position)) query = query.Where(q => q.Position.Contains(Position));
            if (UserId != null) query = query.Where(q => q.UserId == UserId);

            if (SortOrder == Html.GetSortAsc("UserId")) query = query.OrderBy(q => q.User.FullName);
            else if (SortOrder == Html.GetSortDsc("UserId")) query = query.OrderByDescending(q => q.User.FullName);
            else if (SortOrder == Html.GetSortAsc("Position")) query = query.OrderBy(q => q.Position);
            else if (SortOrder == Html.GetSortDsc("Position")) query = query.OrderByDescending(q => q.Position);
            else query = query.OrderBy(q => q.User.FullName);

            ViewBag.Total = query.Count();
            ViewBag.DistinctClaimCode = ClaimAuthorityLimitMLReDetailService.GetDistinctClaimCodeForClaimAuthorityLimitMLRe();
            DropDownUser();
            SetViewBagMessage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: ClaimAuthorityLimitMLRe/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new ClaimAuthorityLimitMLReViewModel());
        }

        // POST: ClaimAuthorityLimitMLRe/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, ClaimAuthorityLimitMLReViewModel model)
        {
            var calMLReDetailBos = model.GetClaimAuthorityLimitMLReDetails(form);

            if (ModelState.IsValid)
            {
                Result childResult = ClaimAuthorityLimitMLReDetailService.Validate(calMLReDetailBos);
                if (!childResult.Valid)
                {
                    AddResult(childResult);
                    LoadPage(calDetailBos: calMLReDetailBos);
                    return View(model);
                }

                var bo = model.FormBo(AuthUserId, AuthUserId);
                TrailObject trail = GetNewTrailObject();
                Result = ClaimAuthorityLimitMLReService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;
                    model.ProcessClaimAuthorityLimitMLReDetails(calMLReDetailBos, AuthUserId, ref trail);

                    CreateTrail(bo.Id, "Create Claim Authority Limit - MLRe");

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }
            LoadPage(calDetailBos: calMLReDetailBos);
            return View(model);
        }

        // GET: ClaimAuthorityLimitMLRe/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            ClaimAuthorityLimitMLReBo calMLReBo = ClaimAuthorityLimitMLReService.Find(id);
            if (calMLReBo == null)
            {
                return RedirectToAction("Index");
            }

            var calMLReDetailBos = ClaimAuthorityLimitMLReDetailService.GetByClaimAuthorityLimitMLReId(calMLReBo.Id);
            LoadPage(calMLReBo, calMLReDetailBos);
            return View(new ClaimAuthorityLimitMLReViewModel(calMLReBo));
        }

        // POST: ClaimAuthorityLimitMLRe/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, ClaimAuthorityLimitMLReViewModel model)
        {
            var calMLReDetailBos = model.GetClaimAuthorityLimitMLReDetails(form);

            ClaimAuthorityLimitMLReBo calMLReBo = ClaimAuthorityLimitMLReService.Find(id);
            if (calMLReBo == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                Result childResult = ClaimAuthorityLimitMLReDetailService.Validate(calMLReDetailBos);
                if (!childResult.Valid)
                {
                    AddResult(childResult);
                    LoadPage(calMLReBo, calMLReDetailBos);
                    return View(model);
                }

                var bo = model.FormBo(AuthUserId, AuthUserId);
                TrailObject trail = GetNewTrailObject();
                Result = ClaimAuthorityLimitMLReService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.ProcessClaimAuthorityLimitMLReDetails(calMLReDetailBos, AuthUserId, ref trail);

                    CreateTrail(calMLReBo.Id, "Update Claim Authority Limit - MLRe");

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = calMLReBo.Id });
                }
                AddResult(Result);
            }
            LoadPage(calMLReBo, calMLReDetailBos);
            return View(model);
        }

        // GET: ClaimAuthorityLimitMLRe/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            ClaimAuthorityLimitMLReBo calMLReBo = ClaimAuthorityLimitMLReService.Find(id);
            if (calMLReBo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new ClaimAuthorityLimitMLReViewModel(calMLReBo));
        }

        // POST: ClaimAuthorityLimitMLRe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id)
        {
            ClaimAuthorityLimitMLReBo calMLReBo = ClaimAuthorityLimitMLReService.Find(id);
            if (calMLReBo == null)
            {
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = ClaimAuthorityLimitMLReService.Delete(calMLReBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(calMLReBo.Id, "Delete Claim Authority Limit - MLRe");

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = calMLReBo.Id });
        }

        public void LoadPage(ClaimAuthorityLimitMLReBo calMLReBo = null, IList<ClaimAuthorityLimitMLReDetailBo> calDetailBos = null)
        {
            DropDownEmpty();
            DropDownDepartment();
            DropDownClaimCode(ClaimCodeBo.StatusActive);
            if (calMLReBo == null)
            {
                //DropDownUser(UserBo.StatusActive);
                if (calDetailBos == null) ViewBag.CALCedantDetailBos = Array.Empty<ClaimAuthorityLimitMLReDetailBo>();
                else ViewBag.CALCedantDetailBos = calDetailBos;
            }
            else
            {
                //DropDownUser(UserBo.StatusActive, calMLReBo.UserId);
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

        [HttpPost]
        public JsonResult GetUserByDepartment(int departmentId)
        {
            IList<SelectListItem> users = DropDownUser(UserBo.StatusActive, departmentId: departmentId);
            return Json(new { users });
        }

        [HttpPost]
        public JsonResult GetClaimCodes(int? claimCodeId = null)
        {
            IList<SelectListItem> claimCodes = DropDownClaimCode(ClaimCodeBo.StatusActive, claimCodeId);
            return Json(new { claimCodes });
        }
    }
}