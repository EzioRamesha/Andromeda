using BusinessObject;
using PagedList;
using Services;
using Shared;
using Shared.DataAccess;
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
    public class PublicHolidayController : BaseController
    {
        public const string Controller = "PublicHoliday";

        // GET: PublicHoliday
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(string Year, string SortOrder, int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Year"] = Year,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortYear = GetSortParam("Year");

            var query = _db.PublicHolidays.Select(PublicHolidayViewModel.Expression());
            if (!string.IsNullOrEmpty(Year)) query = query.Where(q => q.Year.ToString().Contains(Year));

            if (SortOrder == Html.GetSortAsc("Year")) query = query.OrderBy(q => q.Year);
            else if (SortOrder == Html.GetSortDsc("Year")) query = query.OrderByDescending(q => q.Year);
            else query = query.OrderByDescending(q => q.Year);

            ViewBag.Total = query.Count();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: PublicHoliday/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new PublicHolidayViewModel());
        }

        // POST: PublicHoliday/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, PublicHolidayViewModel model)
        {
            var publicHolidayDetailBos = model.GetPublicHolidayDetails(form);

            if (ModelState.IsValid)
            {
                Result childResult = PublicHolidayDetailService.Validate(publicHolidayDetailBos);
                if (!childResult.Valid)
                {
                    AddResult(childResult);
                    LoadPage(publicHolidayDetailBos);
                    return View(model);
                }

                var bo = model.FormBo(AuthUserId, AuthUserId);
                TrailObject trail = GetNewTrailObject();
                Result = PublicHolidayService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;
                    model.ProcessPublicHolidayDetails(publicHolidayDetailBos, AuthUserId, ref trail);

                    CreateTrail(bo.Id, "Create Public Holiday");

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }
            LoadPage(publicHolidayDetailBos);
            return View(model);
        }

        // GET: PublicHoliday/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            PublicHolidayBo publicHolidayBo = PublicHolidayService.Find(id);
            if (publicHolidayBo == null)
            {
                return RedirectToAction("Index");
            }

            var publicHolidayDetailBos = PublicHolidayDetailService.GetByPublicHolidayId(publicHolidayBo.Id);
            LoadPage(publicHolidayDetailBos);
            return View(new PublicHolidayViewModel(publicHolidayBo));
        }

        // POST: PublicHoliday/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, PublicHolidayViewModel model)
        {
            var publicHolidayDetailBos = model.GetPublicHolidayDetails(form);

            PublicHolidayBo calMLReBo = PublicHolidayService.Find(id);
            if (calMLReBo == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                Result childResult = PublicHolidayDetailService.Validate(publicHolidayDetailBos);
                if (!childResult.Valid)
                {
                    AddResult(childResult);
                    LoadPage(publicHolidayDetailBos);
                    return View(model);
                }

                var bo = model.FormBo(AuthUserId, AuthUserId);
                TrailObject trail = GetNewTrailObject();
                Result = PublicHolidayService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.ProcessPublicHolidayDetails(publicHolidayDetailBos, AuthUserId, ref trail);

                    CreateTrail(calMLReBo.Id, "Update Public Holiday");

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = calMLReBo.Id });
                }
                AddResult(Result);
            }
            LoadPage(publicHolidayDetailBos);
            return View(model);
        }

        // GET: PublicHoliday/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            PublicHolidayBo calMLReBo = PublicHolidayService.Find(id);
            if (calMLReBo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new PublicHolidayViewModel(calMLReBo));
        }

        // POST: PublicHoliday/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id)
        {
            PublicHolidayBo calMLReBo = PublicHolidayService.Find(id);
            if (calMLReBo == null)
            {
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = PublicHolidayService.Delete(calMLReBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(calMLReBo.Id, "Delete Public Holiday");

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = calMLReBo.Id });
        }

        public JsonResult CalculateDateRange(string startDateStr, string endDateStr, bool addDayAfterWorkingHour = false)
        {
            DateTime startDate;
            DateTime endDate;

            bool parseStartDate = DateTime.TryParse(startDateStr, out startDate);
            bool parseEndDate = DateTime.TryParse(endDateStr, out endDate);

            string error = null;
            if (!parseStartDate || !parseEndDate)
            {
                error = "Invalid Date Format";
            }
            else if (endDate <= startDate)
            {
                error = "End Date cannot be before Start Date";
            }

            if (error != null)
                return Json(new { error });

            DateTime startWorkingTime;
            DateTime endWorkingTime = DateTime.Parse(startDate.ToShortDateString() + " " + Util.GetConfig("EndWorkingTime"));
            if (addDayAfterWorkingHour)
            {
                startWorkingTime = DateTime.Parse(startDate.ToShortDateString() + " " + Util.GetConfig("StartWorkingTime"));

                if (startDate >= endWorkingTime)
                {
                    startDate = startDate.AddDays(1);
                    TimeSpan ts = new TimeSpan(startWorkingTime.Hour, startWorkingTime.Minute, 0);

                    startDate = startDate.Date + ts;
                }

                if (startDate < startWorkingTime)
                {
                    TimeSpan ts = new TimeSpan(startWorkingTime.Hour, startWorkingTime.Minute, 0);

                    startDate = startDate.Date + ts;
                }
            }

            TimeSpan timeRemaining;
            if (startDate.Date == endDate.Date)
            {
                timeRemaining = endDate - startDate;
            }
            else
            {
                int days = 0;
                DateTime currentDate = startDate;
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    currentDate = currentDate.AddDays(1);
                    days++;
                }
                while (currentDate.Date < endDate.Date)
                {
                    if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        currentDate = currentDate.AddDays(1);
                        continue;
                    }

                    if (PublicHolidayDetailService.IsExists(currentDate))
                    {
                        currentDate = currentDate.AddDays(1);
                        continue;
                    }

                    currentDate = currentDate.AddDays(1);
                    days++;
                }

                //TimeSpan startDateDuration = endWorkingTime - startDate;

                startWorkingTime = DateTime.Parse(endDate.ToShortDateString() + " " + Util.GetConfig("StartWorkingTime"));
                if (endDate < startWorkingTime)
                    endDate = startWorkingTime;

                //timeRemaining = endDate - startWorkingTime;

                timeRemaining = endDate.TimeOfDay - startDate.TimeOfDay;
                timeRemaining = timeRemaining.Add(new TimeSpan(days, 0, 0, 0));
            }

            int hours = (24 * timeRemaining.Days) + timeRemaining.Hours;
            int minutes = timeRemaining.Minutes;

            long turnAroundTime = (new TimeSpan(hours, minutes, 0)).Ticks;

            return Json(new { hours, minutes, turnAroundTime });
        }

        public void LoadPage(IList<PublicHolidayDetailBo> publicHolidayDetailBos = null)
        {
            if (publicHolidayDetailBos == null) ViewBag.PublicHolidayDetailBos = Array.Empty<PublicHolidayDetailBo>();
            else ViewBag.PublicHolidayDetailBos = publicHolidayDetailBos;

            SetViewBagMessage();
        }
    }
}