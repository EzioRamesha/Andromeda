using BusinessObject;
using BusinessObject.Identity;
using ConsoleApp.Commands.ProcessDatas;
using PagedList;
using Services;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class EventClaimCodeMappingController : BaseController
    {
        public const string Controller = "EventClaimCodeMapping";

        // GET: EventClaimCodeMapping
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string CedantId, 
            string CedingEventCode, 
            string CedingClaimType, 
            int? EventCodeId, 
            string SortOrder, 
            int? Page
        )
        {
            int? cid = null;
            if (int.TryParse(CedantId, out int outId))
                cid = outId;

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["CedantId"] = cid.HasValue ? CedantId : null,
                ["CedingEventCode"] = CedingEventCode,
                ["CedingClaimType"] = CedingClaimType,
                ["EventCodeId"] = EventCodeId,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCedantId = GetSortParam("CedantId");
            ViewBag.SortEventCodeId = GetSortParam("EventCodeId");

            var query = _db.EventClaimCodeMappings.Select(EventClaimCodeMappingViewModel.Expression());
            if (cid.HasValue) query = query.Where(q => q.CedantId == cid);
            if (!string.IsNullOrEmpty(CedingEventCode)) query = query.Where(q => q.EventClaimCodeMappingDetails.Any(d => d.CedingEventCode == CedingEventCode));
            if (!string.IsNullOrEmpty(CedingClaimType)) query = query.Where(q => q.EventClaimCodeMappingDetails.Any(d => d.CedingClaimType == CedingClaimType));
            if (EventCodeId != null) query = query.Where(q => q.EventCodeId == EventCodeId);

            if (SortOrder == Html.GetSortAsc("CedantId")) query = query.OrderBy(q => q.Cedant.Code);
            else if (SortOrder == Html.GetSortDsc("CedantId")) query = query.OrderByDescending(q => q.Cedant.Code);
            else if (SortOrder == Html.GetSortAsc("EventCodeId")) query = query.OrderBy(q => q.EventCode.Code);
            else if (SortOrder == Html.GetSortDsc("EventCodeId")) query = query.OrderByDescending(q => q.EventCode.Code);
            else query = query.OrderBy(q => q.Cedant.Code);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: EventClaimCodeMapping/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new EventClaimCodeMappingViewModel());
        }

        // POST: EventClaimCodeMapping/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(EventClaimCodeMappingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);
                var trail = GetNewTrailObject();
                Result = EventClaimCodeMappingService.Result();
                var mappingResult = EventClaimCodeMappingService.ValidateMapping(bo);
                if (!mappingResult.Valid)
                {
                    Result.AddErrorRange(mappingResult.ToErrorArray());
                }
                else
                {
                    Result = EventClaimCodeMappingService.Create(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        model.Id = bo.Id;
                        EventClaimCodeMappingService.ProcessMappingDetail(bo, AuthUserId); // DO NOT TRAIL
                        CreateTrail(
                            bo.Id,
                            "Create Event & Claim Code Mapping"
                        );

                        SetCreateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = bo.Id });
                    }
                }
                AddResult(Result);
            }
            LoadPage();
            return View(model);
        }

        // GET: EventClaimCodeMapping/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            EventClaimCodeMappingBo eventClaimCodeMappingBo = EventClaimCodeMappingService.Find(id);
            if (eventClaimCodeMappingBo == null)
            {
                return RedirectToAction("Index");
            }
            LoadPage(eventClaimCodeMappingBo);
            return View(new EventClaimCodeMappingViewModel(eventClaimCodeMappingBo));
        }

        // POST: EventClaimCodeMapping/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, EventClaimCodeMappingViewModel model)
        {
            var dbBo = EventClaimCodeMappingService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                var trail = GetNewTrailObject();
                Result = EventClaimCodeMappingService.Result();
                var mappingResult = EventClaimCodeMappingService.ValidateMapping(bo);
                if (!mappingResult.Valid)
                {
                    Result.AddErrorRange(mappingResult.ToErrorArray());
                }
                else
                {
                    Result = EventClaimCodeMappingService.Update(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        EventClaimCodeMappingService.ProcessMappingDetail(bo, AuthUserId); // DO NOT TRAIL
                        CreateTrail(
                            dbBo.Id,
                            "Update Event & Claim Code Mapping"
                        );

                        SetUpdateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = bo.Id });
                    }
                }
                AddResult(Result);
            }
            LoadPage(dbBo);
            return View(model);
        }

        // GET: EventClaimCodeMapping/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            EventClaimCodeMappingBo eventClaimCodeMappingBo = EventClaimCodeMappingService.Find(id);
            if (eventClaimCodeMappingBo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new EventClaimCodeMappingViewModel(eventClaimCodeMappingBo));
        }

        // POST: EventClaimCodeMapping/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id)
        {
            EventClaimCodeMappingBo eventClaimCodeMappingBo = EventClaimCodeMappingService.Find(id);
            if (eventClaimCodeMappingBo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = EventClaimCodeMappingService.Delete(eventClaimCodeMappingBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    eventClaimCodeMappingBo.Id,
                    "Delete Event & Claim Code Mapping"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = eventClaimCodeMappingBo.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = AccessMatrixBo.PowerUpload, ReturnController = Controller)]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var process = new ProcessEventClaimCodeMapping()
                    {
                        PostedFile = upload,
                        AuthUserId = AuthUserId,
                    };
                    process.Process();

                    if (process.Errors.Count() > 0 && process.Errors != null)
                    {
                        SetErrorSessionMsgArr(process.Errors.Take(50).ToList());
                    }

                    int create = process.GetProcessCount("Create");
                    int update = process.GetProcessCount("Update");
                    int delete = process.GetProcessCount("Delete");

                    if (create != 0 || update != 0 || delete != 0)
                    {
                        SetSuccessSessionMsg(string.Format("Process File Successful, Total Create: {0}, Total Update: {1}, Total Delete: {2}", create, update, delete));
                    }
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Download(
            int type,
            int? CedantId,
            string CedingEventCode,
            string CedingClaimType,
            int? EventCodeId
        )
        {
            var process = new ProcessEventClaimCodeMapping();
            IEnumerable<EventClaimCodeMappingBo> template = null;

            if (type != 3)
            {
                var query = _db.EventClaimCodeMappings.Select(EventClaimCodeMappingService.Expression());

                if (type == 2) // filtered dowload
                {
                    if (CedantId.HasValue) query = query.Where(q => q.CedantId == CedantId);
                    if (!string.IsNullOrEmpty(CedingEventCode)) query = query.Where(q => q.CedingEventCode.Contains(CedingEventCode));
                    if (!string.IsNullOrEmpty(CedingClaimType)) query = query.Where(q => q.CedingClaimType.Contains(CedingClaimType));
                    if (EventCodeId.HasValue) query = query.Where(q => q.EventCodeId == EventCodeId);
                }
                process.ExportToCsv(query);
            }
            else
            {
                process.ExportToCsv(template);
            }

            return File(process.FilePath, "text/csv", Path.GetFileName(process.FilePath));
        }

        public void IndexPage()
        {
            DropDownCedant();
            DropDownEventCode();
            SetViewBagMessage();
        }
        public void LoadPage(EventClaimCodeMappingBo eventClaimCodeMappingBo = null)
        {
            if (eventClaimCodeMappingBo == null)
            {
                DropDownEventCode(EventCodeBo.StatusActive);
                DropDownCedant(CedantBo.StatusActive);
            }
            else
            {
                DropDownEventCode(EventCodeBo.StatusActive, eventClaimCodeMappingBo.EventCodeId);
                if (eventClaimCodeMappingBo.EventCodeBo.Status == EventCodeBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.EventCodeStatusInactive);
                }

                DropDownCedant(CedantBo.StatusActive, eventClaimCodeMappingBo.CedantId);
                if (eventClaimCodeMappingBo.CedantBo.Status == CedantBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.CedantStatusInactive);
                }
            }
            SetViewBagMessage();
        }
    }
}