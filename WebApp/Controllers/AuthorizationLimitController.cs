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
    public class AuthorizationLimitController : BaseController
    {
        public const string Controller = "AuthorizationLimit";

        // GET: AuthorizationLimit
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            int? AccessGroupId,
            string PositiveAmountFrom,
            string PositiveAmountTo,
            string NegativeAmountFrom,
            string NegativeAmountTo,
            string Percentage,
            string SortOrder,
            int? Page)
        {
            double? positiveAmountFrom = Util.StringToDouble(PositiveAmountFrom);
            double? positiveAmountTo = Util.StringToDouble(PositiveAmountTo);
            double? negativeAmountFrom = Util.StringToDouble(NegativeAmountFrom);
            double? negativeAmountTo = Util.StringToDouble(NegativeAmountTo);
            double? percentage = Util.StringToDouble(Percentage);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["AccessGroupId"] = AccessGroupId,
                ["PositiveAmountFrom"] = positiveAmountFrom.HasValue ? positiveAmountFrom : null,
                ["PositiveAmountTo"] = positiveAmountTo.HasValue ? positiveAmountTo : null,
                ["NegativeAmountFrom"] = negativeAmountFrom.HasValue ? negativeAmountFrom : null,
                ["NegativeAmountTo"] = negativeAmountTo.HasValue ? negativeAmountTo : null,
                ["Percentage"] = percentage.HasValue ? percentage : null,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortAccessGroupId = GetSortParam("AccessGroupId");
            ViewBag.SortPositiveAmountFrom = GetSortParam("PositiveAmountFrom");
            ViewBag.SortPositiveAmountTo = GetSortParam("PositiveAmountTo");
            ViewBag.SortNegativeAmountFrom = GetSortParam("NegativeAmountFrom");
            ViewBag.SortNegativeAmountTo = GetSortParam("NegativeAmountTo");
            ViewBag.SortPercentage = GetSortParam("Percentage");

            var query = _db.AuthorizationLimits.Select(AuthorizationLimitViewModel.Expression());
            if (AccessGroupId != null) query = query.Where(q => q.AccessGroupId == AccessGroupId);
            if (positiveAmountFrom.HasValue) query = query.Where(q => q.PositiveAmountFrom == positiveAmountFrom);
            if (positiveAmountTo.HasValue) query = query.Where(q => q.PositiveAmountTo == positiveAmountTo);
            if (negativeAmountFrom.HasValue) query = query.Where(q => q.NegativeAmountFrom == negativeAmountFrom);
            if (negativeAmountTo.HasValue) query = query.Where(q => q.NegativeAmountTo == negativeAmountTo);
            if (percentage.HasValue) query = query.Where(q => q.Percentage == percentage);

            if (SortOrder == Html.GetSortAsc("AccessGroupId")) query = query.OrderBy(q => q.AccessGroup.Name);
            else if (SortOrder == Html.GetSortDsc("AccessGroupId")) query = query.OrderByDescending(q => q.AccessGroup.Name);
            else if (SortOrder == Html.GetSortAsc("PositiveAmountFrom")) query = query.OrderBy(q => q.PositiveAmountFrom);
            else if (SortOrder == Html.GetSortDsc("PositiveAmountFrom")) query = query.OrderByDescending(q => q.PositiveAmountFrom);
            else if (SortOrder == Html.GetSortAsc("PositiveAmountTo")) query = query.OrderBy(q => q.PositiveAmountTo);
            else if (SortOrder == Html.GetSortDsc("PositiveAmountTo")) query = query.OrderByDescending(q => q.PositiveAmountTo);
            else if (SortOrder == Html.GetSortAsc("NegativeAmountFrom")) query = query.OrderBy(q => q.NegativeAmountFrom);
            else if (SortOrder == Html.GetSortDsc("NegativeAmountFrom")) query = query.OrderByDescending(q => q.NegativeAmountFrom);
            else if (SortOrder == Html.GetSortAsc("NegativeAmountTo")) query = query.OrderBy(q => q.NegativeAmountTo);
            else if (SortOrder == Html.GetSortDsc("FileNegativeAmountToType")) query = query.OrderByDescending(q => q.NegativeAmountTo);
            else if (SortOrder == Html.GetSortAsc("Percentage")) query = query.OrderBy(q => q.Percentage);
            else if (SortOrder == Html.GetSortDsc("Percentage")) query = query.OrderByDescending(q => q.Percentage);
            else query = query.OrderByDescending(q => q.Id);

            ViewBag.Total = query.Count();
            LoadPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: AuthorizationLimit/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new AuthorizationLimitViewModel());
        }

        // POST: AuthorizationLimit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(AuthorizationLimitViewModel model)
        {
            if (ModelState.IsValid)
            {
                AuthorizationLimitBo authorizationLimitBo = new AuthorizationLimitBo
                {
                    AccessGroupId = model.AccessGroupId,
                    PositiveAmountFrom = model.PositiveAmountFrom,
                    PositiveAmountTo = model.PositiveAmountTo,
                    NegativeAmountFrom = model.NegativeAmountFrom,
                    NegativeAmountTo = model.NegativeAmountTo,
                    Percentage = model.Percentage,
                    CreatedById = AuthUserId,
                    UpdatedById = AuthUserId,
                };
                TrailObject trail = GetNewTrailObject();
                Result = AuthorizationLimitService.Create(ref authorizationLimitBo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        authorizationLimitBo.Id,
                        "Create Authorization Limit"
                    );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = authorizationLimitBo.Id });
                }
                AddResult(Result);
            }
            LoadPage();
            return View(model);
        }
        
        // GET: AuthorizationLimit/Edit
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = AuthorizationLimitService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }
            LoadPage();
            return View(new AuthorizationLimitViewModel(bo));
        }

        // POST: AuthorizationLimit/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, AuthorizationLimitViewModel model)
        {
            AuthorizationLimitBo authorizationLimitBo = AuthorizationLimitService.Find(id);
            if (ModelState.IsValid)
            {
                authorizationLimitBo.AccessGroupId = model.AccessGroupId;
                authorizationLimitBo.PositiveAmountFrom = model.PositiveAmountFrom;
                authorizationLimitBo.PositiveAmountTo = model.PositiveAmountTo;
                authorizationLimitBo.NegativeAmountFrom = model.NegativeAmountFrom;
                authorizationLimitBo.NegativeAmountTo = model.NegativeAmountTo;
                authorizationLimitBo.Percentage = model.Percentage;
                authorizationLimitBo.UpdatedById = AuthUserId;

                TrailObject trail = GetNewTrailObject();
                Result = AuthorizationLimitService.Update(ref authorizationLimitBo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        authorizationLimitBo.Id,
                        "Update Authorization Limit"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = authorizationLimitBo.Id });
                }
                AddResult(Result);
            }
            LoadPage();
            return View(model);
        }

        // GET: Cedant/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            AuthorizationLimitBo authorizationLimitBo = AuthorizationLimitService.Find(id);
            if (authorizationLimitBo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new AuthorizationLimitViewModel(authorizationLimitBo));
        }

        // POST: Cedant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, CedantViewModel model)
        {
            AuthorizationLimitBo authorizationLimitBo = AuthorizationLimitService.Find(id);
            if (authorizationLimitBo == null)
            {
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = AuthorizationLimitService.Delete(authorizationLimitBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    authorizationLimitBo.Id,
                    "Delete Authorization Limit"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = authorizationLimitBo.Id });
        }

        public void LoadPage()
        {
            DropDownAccessGroup();
            SetViewBagMessage();
        }
    }
}