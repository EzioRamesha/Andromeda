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
    public class EventCodeController : BaseController
    {
        public const string Controller = "EventCode";

        // GET: EventCode
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

            var query = _db.EventCodes.Select(EventCodeViewModel.Expression());
            if (!string.IsNullOrEmpty(Code)) query = query.Where(q => q.Code.Contains(Code));
            if (Status != null) query = query.Where(q => q.Status == Status);

            if (SortOrder == Html.GetSortAsc("Code")) query = query.OrderBy(q => q.Code);
            else if (SortOrder == Html.GetSortDsc("Code")) query = query.OrderByDescending(q => q.Code);
            else query = query.OrderBy(q => q.Code);

            ViewBag.Total = query.Count();
            LoadPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: EventCode/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new EventCodeViewModel());
        }

        // POST: EventCode/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(EventCodeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var eventCodeBo = new EventCodeBo
                {
                    Code = model.Code?.Trim(),
                    Status = model.Status,
                    Description = model.Description?.Trim(),
                    CreatedById = AuthUserId,
                    UpdatedById = AuthUserId,
                };

                TrailObject trail = GetNewTrailObject();
                Result = EventCodeService.Create(ref eventCodeBo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        eventCodeBo.Id,
                        "Create Event Code"
                    );

                    model.Id = eventCodeBo.Id;

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = eventCodeBo.Id });
                }
                AddResult(Result);
            }
            LoadPage();
            return View(model);
        }

        // GET: EventCode/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            EventCodeBo eventCodeBo = EventCodeService.Find(id);
            if (eventCodeBo == null)
            {
                return RedirectToAction("Index");
            }
            LoadPage(eventCodeBo);
            return View(new EventCodeViewModel(eventCodeBo));
        }

        // POST: EventCode/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, EventCodeViewModel model)
        {
            EventCodeBo eventCodeBo = EventCodeService.Find(id);
            if (eventCodeBo == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                eventCodeBo.Code = model.Code?.Trim();
                eventCodeBo.Status = model.Status;
                eventCodeBo.Description = model.Description?.Trim();
                eventCodeBo.UpdatedById = AuthUserId;

                TrailObject trail = GetNewTrailObject();
                Result = EventCodeService.Update(ref eventCodeBo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        eventCodeBo.Id,
                        "Update Event Code"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = eventCodeBo.Id });
                }
                AddResult(Result);
            }
            LoadPage(eventCodeBo);
            return View(model);
        }

        // GET: EventCode/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            EventCodeBo eventCodeBo = EventCodeService.Find(id);
            if (eventCodeBo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new EventCodeViewModel(eventCodeBo));
        }

        // POST: EventCode/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id)
        {
            EventCodeBo eventCodeBo = EventCodeService.Find(id);
            if (eventCodeBo == null)
            {
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = EventCodeService.Delete(eventCodeBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    eventCodeBo.Id,
                    "Delete Event Code"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = eventCodeBo.Id });
        }

        public void LoadPage(EventCodeBo eventCodeBo = null)
        {
            if (eventCodeBo == null) DropDownStatus();
            else DropDownStatus(eventCodeBo.Status);

            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownStatus(int? selectedId = null)
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= EventCodeBo.MaxStatus; i++)
            {
                var selected = i == selectedId;
                items.Add(new SelectListItem { Text = EventCodeBo.GetStatusName(i), Value = i.ToString(), Selected = selected });
            }
            ViewBag.StatusItems = items;
            return items;
        }
    }
}