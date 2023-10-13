using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BusinessObject;
using DataAccess.EntityFramework;
using PagedList;
using Services;
using Shared;
using Shared.Trails;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class ItemCodeController : BaseController
    {
        public const string Controller = "ItemCode";

        // GET: ItemCode
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(string Code, int? BusinessOriginPickListDetailId, int? ReportingType, string SortOrder, int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Code"] = Code,
                ["BusinessOriginPickListDetailId"] = BusinessOriginPickListDetailId,
                ["ReportingType"] = ReportingType,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCode = GetSortParam("Code");
            ViewBag.SortBusinessOriginPickListDetailId = GetSortParam("BusinessOriginPickListDetailId");
            ViewBag.SortReportingType = GetSortParam("ReportingType");
            ViewBag.SortDesc = GetSortParam("Description");

            var query = _db.ItemCodes.Select(ItemCodeViewModel.Expression());
            if (!string.IsNullOrEmpty(Code)) query = query.Where(q => q.Code == Code);
            if (BusinessOriginPickListDetailId.HasValue) query = query.Where(q => q.BusinessOriginPickListDetailId == BusinessOriginPickListDetailId);
            if (ReportingType.HasValue) query = query.Where(q => q.ReportingType == ReportingType);

            if (SortOrder == Html.GetSortAsc("Code")) query = query.OrderBy(q => q.Code);
            else if (SortOrder == Html.GetSortDsc("Code")) query = query.OrderByDescending(q => q.Code);
            else if (SortOrder == Html.GetSortAsc("BusinessOriginPickListDetailId")) query = query.OrderBy(q => q.BusinessOriginPickListDetail.Code);
            else if (SortOrder == Html.GetSortDsc("BusinessOriginPickListDetailId")) query = query.OrderByDescending(q => q.BusinessOriginPickListDetail.Code);
            else if (SortOrder == Html.GetSortAsc("ReportingType")) query = query.OrderBy(q => q.ReportingType);
            else if (SortOrder == Html.GetSortDsc("ReportingType")) query = query.OrderByDescending(q => q.ReportingType);
            else if (SortOrder == Html.GetSortAsc("Description")) query = query.OrderBy(q => q.Description);
            else if (SortOrder == Html.GetSortDsc("Description")) query = query.OrderByDescending(q => q.Description);
            else query = query.OrderBy(q => q.Code);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: ItemCode/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            SetViewBagMessage();
            LoadPage();
            return View(new ItemCodeViewModel());
        }

        // POST: ItemCode/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(ItemCodeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var itemCodeBo = new ItemCodeBo
                {
                    Code = model.Code?.Trim(),
                    ReportingType = model.ReportingType,
                    Description = model.Description?.Trim(),
                    BusinessOriginPickListDetailId = model.BusinessOriginPickListDetailId,
                    BusinessOriginPickListDetailBo = PickListDetailService.Find(model.BusinessOriginPickListDetailId),
                    CreatedById = AuthUserId,
                    UpdatedById = AuthUserId,
                };

                TrailObject trail = GetNewTrailObject();
                Result = ItemCodeService.Create(ref itemCodeBo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        itemCodeBo.Id,
                        "Create Item Code"
                    );

                    model.Id = itemCodeBo.Id;

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = itemCodeBo.Id });
                }
                AddResult(Result);
            }
            SetViewBagMessage();
            LoadPage();
            return View(model);
        }

        // GET: ItemCode/Edit/5
        public ActionResult Edit(int id)
        {
            SetViewBagMessage();
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            ItemCodeBo itemCodeBo = ItemCodeService.Find(id);
            if (itemCodeBo == null)
            {
                return RedirectToAction("Index");
            }

            LoadPage(itemCodeBo);
            return View(new ItemCodeViewModel(itemCodeBo));
        }

        // POST: ItemCode/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, ItemCodeViewModel model)
        {
            ItemCodeBo itemCodeBo = ItemCodeService.Find(id);
            if (itemCodeBo == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                itemCodeBo.Code = model.Code.Trim();
                itemCodeBo.ReportingType = model.ReportingType;
                itemCodeBo.Description = model.Description.Trim();
                itemCodeBo.BusinessOriginPickListDetailId = model.BusinessOriginPickListDetailId;
                itemCodeBo.BusinessOriginPickListDetailBo = PickListDetailService.Find(model.BusinessOriginPickListDetailId);
                itemCodeBo.UpdatedById = AuthUserId;

                TrailObject trail = GetNewTrailObject();
                Result = ItemCodeService.Update(ref itemCodeBo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        itemCodeBo.Id,
                        "Update Item Code"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = itemCodeBo.Id });
                }
                AddResult(Result);
            }

            LoadPage(itemCodeBo);
            return View(model);
        }

        // GET: ItemCode/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            ItemCodeBo itemCodeBo = ItemCodeService.Find(id);
            if (itemCodeBo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new ItemCodeViewModel(itemCodeBo));
        }

        // POST: ItemCode/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id)
        {
            ItemCodeBo itemCodeBo = ItemCodeService.Find(id);
            if (itemCodeBo == null)
            {
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = ItemCodeService.Delete(itemCodeBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    itemCodeBo.Id,
                    "Delete Item Code"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = itemCodeBo.Id });
        }

        public void IndexPage()
        {
            DropDownEmpty();
            DropDownReportingType();
            DropDownBusinessOrigin();
            SetViewBagMessage();
        }

        public void LoadPage(ItemCodeBo itemCodeBo = null)
        {
            DropDownReportingType();
            DropDownBusinessOrigin();

            if (itemCodeBo == null)
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
            for (int i = 1; i <= ItemCodeBo.ReportingTypeMax; i++)
            {
                items.Add(new SelectListItem { Text = ItemCodeBo.GetReportingTypeName(i), Value = i.ToString() });
            }
            ViewBag.ReportingTypes = items;
            return items;
        }
    }
}
