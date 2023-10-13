using BusinessObject;
using PagedList;
using Services;
using Shared;
using Shared.Trails;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class RetroAccountCodeController : BaseController
    {
        public const string Controller = "RetroAccountCode";

        // GET: AccountCode
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(string Code, int? ReportingType, string SortOrder, int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Code"] = Code,
                ["ReportingType"] = ReportingType,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortReportingType = GetSortParam("ReportingType");
            ViewBag.SortCode = GetSortParam("Code");
            ViewBag.SortDesc = GetSortParam("Description");

            var query = _db.AccountCodes.Select(AccountCodeViewModel.Expression())
                .Where(q => q.Type == AccountCodeBo.TypeRetro);

            if (!string.IsNullOrEmpty(Code)) query = query.Where(q => q.Code.Contains(Code));
            if (ReportingType.HasValue) query = query.Where(q => q.ReportingType == ReportingType);

            if (SortOrder == Html.GetSortAsc("Code")) query = query.OrderBy(q => q.Code);
            else if (SortOrder == Html.GetSortDsc("Code")) query = query.OrderByDescending(q => q.Code);
            else if (SortOrder == Html.GetSortAsc("ReportingType")) query = query.OrderBy(q => q.ReportingType);
            else if (SortOrder == Html.GetSortDsc("ReportingType")) query = query.OrderByDescending(q => q.ReportingType);
            else if (SortOrder == Html.GetSortAsc("Description")) query = query.OrderBy(q => q.Description);
            else if (SortOrder == Html.GetSortDsc("Description")) query = query.OrderByDescending(q => q.Description);
            else query = query.OrderBy(q => q.Code);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: AccountCode/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            SetViewBagMessage();
            LoadPage();
            return View(new AccountCodeViewModel());
        }

        // POST: AccountCode/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(AccountCodeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var accountCodeBo = new AccountCodeBo
                {
                    Code = model.Code?.Trim(),
                    ReportingType = model.ReportingType,
                    Description = model.Description?.Trim(),
                    Type = AccountCodeBo.TypeRetro,
                    CreatedById = AuthUserId,
                    UpdatedById = AuthUserId,
                };

                TrailObject trail = GetNewTrailObject();
                Result = AccountCodeService.Create(ref accountCodeBo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        accountCodeBo.Id,
                        "Create Account Code"
                    );

                    model.Id = accountCodeBo.Id;

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = accountCodeBo.Id });
                }
                AddResult(Result);
            }
            SetViewBagMessage();
            LoadPage();
            return View(model);
        }

        // GET: AccountCode/Edit/5
        public ActionResult Edit(int id)
        {
            SetViewBagMessage();
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            AccountCodeBo accountCodeBo = AccountCodeService.Find(id);
            if (accountCodeBo == null || accountCodeBo.Type != AccountCodeBo.TypeRetro)
            {
                return RedirectToAction("Index");
            }

            LoadPage(accountCodeBo);
            return View(new AccountCodeViewModel(accountCodeBo));
        }

        // POST: AccountCode/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, AccountCodeViewModel model)
        {
            AccountCodeBo accountCodeBo = AccountCodeService.Find(id);
            if (accountCodeBo == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                accountCodeBo.Code = model.Code?.Trim();
                accountCodeBo.ReportingType = model.ReportingType;
                accountCodeBo.Description = model.Description?.Trim();
                accountCodeBo.Type = model.Type;
                accountCodeBo.UpdatedById = AuthUserId;

                TrailObject trail = GetNewTrailObject();
                Result = AccountCodeService.Update(ref accountCodeBo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        accountCodeBo.Id,
                        "Update Account Code"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = accountCodeBo.Id });
                }
                AddResult(Result);
            }

            LoadPage(accountCodeBo);
            return View(model);
        }

        // GET: AccountCode/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            AccountCodeBo accountCodeBo = AccountCodeService.Find(id);
            if (accountCodeBo == null || accountCodeBo.Type != AccountCodeBo.TypeRetro)
            {
                return RedirectToAction("Index");
            }
            return View(new AccountCodeViewModel(accountCodeBo));
        }

        // POST: AccountCode/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id)
        {
            AccountCodeBo accountCodeBo = AccountCodeService.Find(id);
            if (accountCodeBo == null)
            {
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = AccountCodeService.Delete(accountCodeBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    accountCodeBo.Id,
                    "Delete Account Code"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = accountCodeBo.Id });
        }

        public void IndexPage()
        {
            DropDownEmpty();
            DropDownReportingType();
            SetViewBagMessage();
        }

        public void LoadPage(AccountCodeBo accCodeBo = null)
        {
            DropDownReportingType();

            if (accCodeBo == null)
            {
                // Create
            }
            else
            {

            }
            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownReportingType()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= AccountCodeBo.ReportingTypeMax; i++)
            {
                items.Add(new SelectListItem { Text = AccountCodeBo.GetReportingTypeName(i), Value = i.ToString() });
            }
            ViewBag.ReportingTypes = items;
            return items;
        }
    }
}