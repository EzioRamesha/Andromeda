using BusinessObject;
using PagedList;
using Services;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
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
    public class ClaimCodeController : BaseController
    {
        public const string Controller = "ClaimCode";

        // GET: ClaimCode
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(string Code, int? Status, string SortOrder, int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Code"] = Code,
                ["Status"] = Status,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCode = GetSortParam("Code");

            var query = _db.ClaimCodes.Select(ClaimCodeViewModel.Expression());
            if (!string.IsNullOrEmpty(Code)) query = query.Where(q => q.Code.Contains(Code));
            if (Status != null) query = query.Where(q => q.Status == Status);

            if (SortOrder == Html.GetSortAsc("Code")) query = query.OrderBy(q => q.Code);
            else if (SortOrder == Html.GetSortDsc("Code")) query = query.OrderByDescending(q => q.Code);
            else query = query.OrderBy(q => q.Code);

            ViewBag.Total = query.Count();
            LoadPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: ClaimCode/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new ClaimCodeViewModel());
        }

        // POST: ClaimCode/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(ClaimCodeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var claimCodeBo = new ClaimCodeBo
                {
                    Code = model.Code?.Trim(),
                    Status = model.Status,
                    Description = model.Description?.Trim(),
                    CreatedById = AuthUserId,
                    UpdatedById = AuthUserId,
                };

                TrailObject trail = GetNewTrailObject();
                Result = ClaimCodeService.Create(ref claimCodeBo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        claimCodeBo.Id,
                        "Create Claim Code"
                    );

                    model.Id = claimCodeBo.Id;

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = claimCodeBo.Id });
                }
                AddResult(Result);
            }
            LoadPage();
            return View(model);
        }

        // GET: ClaimCode/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            ClaimCodeBo claimCodeBo = ClaimCodeService.Find(id);
            if (claimCodeBo == null)
            {
                return RedirectToAction("Index");
            }
            LoadPage(claimCodeBo);
            return View(new ClaimCodeViewModel(claimCodeBo));
        }

        // POST: ClaimCode/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, ClaimCodeViewModel model)
        {
            ClaimCodeBo claimCodeBo = ClaimCodeService.Find(id);
            if (claimCodeBo == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                claimCodeBo.Code = model.Code?.Trim();
                claimCodeBo.Status = model.Status;
                claimCodeBo.Description = model.Description?.Trim();
                claimCodeBo.UpdatedById = AuthUserId;

                TrailObject trail = GetNewTrailObject();
                Result = ClaimCodeService.Update(ref claimCodeBo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        claimCodeBo.Id,
                        "Update Claim Code"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = claimCodeBo.Id });
                }
                AddResult(Result);
            }
            LoadPage(claimCodeBo);
            return View(model);
        }

        // GET: ClaimCode/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            ClaimCodeBo claimCodeBo = ClaimCodeService.Find(id);
            if (claimCodeBo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new ClaimCodeViewModel(claimCodeBo));
        }

        // POST: ClaimCode/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, ClaimCodeViewModel model)
        {
            ClaimCodeBo claimCodeBo = ClaimCodeService.Find(id);
            if (claimCodeBo == null)
            {
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = ClaimCodeService.Delete(claimCodeBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    claimCodeBo.Id,
                    "Delete Claim Code"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = claimCodeBo.Id });
        }

        public void LoadPage(ClaimCodeBo claimCodeBo = null)
        {
            if (claimCodeBo == null)
            {
                DropDownStatus();
            }
            else
            {
                DropDownStatus(claimCodeBo.Status);
            }
            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownStatus(int? selectedId = null)
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= ClaimCodeBo.MaxStatus; i++)
            {
                var selected = i == selectedId;
                items.Add(new SelectListItem { Text = ClaimCodeBo.GetStatusName(i), Value = i.ToString(), Selected = selected });
            }
            ViewBag.StatusItems = items;
            return items;
        }
    }
}
