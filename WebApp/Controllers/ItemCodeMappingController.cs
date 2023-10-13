using BusinessObject;
using BusinessObject.Identity;
using ConsoleApp.Commands.ProcessDatas;
using ConsoleApp.Commands.ProcessDatas.Exports;
using PagedList;
using Services;
using Shared;
using Shared.DataAccess;
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
    public class ItemCodeMappingController : BaseController
    {
        public const string Controller = "ItemCodeMapping";

        // GET: ItemCodeMapping
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            int? InvoiceFieldPickListDetailId,
            string TreatyType, 
            string TreatyCode,
            int? BusinessOriginPickListDetailId,
            int? ItemCodeId,
            int? ReportingType,
            string SortOrder, 
            int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["InvoiceFieldPickListDetailId"] = InvoiceFieldPickListDetailId,
                ["TreatyType"] = TreatyType,
                ["TreatyCode"] = TreatyCode,
                ["BusinessOriginPickListDetailId"] = BusinessOriginPickListDetailId,
                ["ItemCodeId"] = ItemCodeId,
                ["ReportingType"] = ReportingType,
                ["SortOrder"] = SortOrder,
            };

            ViewBag.SortOrder = SortOrder;
            ViewBag.SortInvoiceFieldPickListDetailId = GetSortParam("InvoiceFieldPickListDetailId");
            ViewBag.SortBusinessOriginPickListDetailId = GetSortParam("BusinessOriginPickListDetailId");
            ViewBag.SortItemCodeId = GetSortParam("ItemCodeId");
            ViewBag.SortReportingType = GetSortParam("ReportingType");

            var query = _db.ItemCodeMappings.Select(ItemCodeMappingViewModel.Expression());
            if (InvoiceFieldPickListDetailId.HasValue) query = query.Where(q => q.InvoiceFieldPickListDetailId == InvoiceFieldPickListDetailId);
            if (!string.IsNullOrEmpty(TreatyType)) query = query.Where(q => q.ItemCodeMappingDetails.Any(d => d.TreatyType == TreatyType));
            if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => q.ItemCodeMappingDetails.Any(d => d.TreatyCode == TreatyCode));
            if (BusinessOriginPickListDetailId.HasValue) query = query.Where(q => q.BusinessOriginPickListDetailId == BusinessOriginPickListDetailId);
            if (ItemCodeId.HasValue) query = query.Where(q => q.ItemCodeId == ItemCodeId);
            if (ReportingType.HasValue) query = query.Where(q => q.ReportingType == ReportingType);

            if (SortOrder == Html.GetSortAsc("InvoiceFieldPickListDetailId")) query = query.OrderBy(q => q.InvoiceFieldPickListDetail.Code);
            else if (SortOrder == Html.GetSortDsc("InvoiceFieldPickListDetailId")) query = query.OrderByDescending(q => q.InvoiceFieldPickListDetail.Code);
            else if (SortOrder == Html.GetSortAsc("BusinessOriginPickListDetailId")) query = query.OrderBy(q => q.BusinessOriginPickListDetail.Code);
            else if (SortOrder == Html.GetSortDsc("BusinessOriginPickListDetailId")) query = query.OrderByDescending(q => q.BusinessOriginPickListDetail.Code);
            else if (SortOrder == Html.GetSortAsc("ItemCodeId")) query = query.OrderBy(q => q.ItemCode.Code);
            else if (SortOrder == Html.GetSortDsc("ItemCodeId")) query = query.OrderByDescending(q => q.ItemCode.Code);
            else if (SortOrder == Html.GetSortAsc("ReportingType")) query = query.OrderBy(q => q.ItemCode.ReportingType);
            else if (SortOrder == Html.GetSortDsc("ReportingType")) query = query.OrderByDescending(q => q.ItemCode.ReportingType);
            else query = query.OrderBy(q => q.InvoiceFieldPickListDetail.Code);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: ItemCodeMapping/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new ItemCodeMappingViewModel());
        }

        // POST: ItemCodeMapping/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(ItemCodeMappingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var itemCodeMappingBo = model.FormBo(AuthUserId, AuthUserId);

                TrailObject trail = GetNewTrailObject();
                Result = ItemCodeMappingService.Result();
                var mappingResult = ItemCodeMappingService.ValidateMapping(itemCodeMappingBo);
                if (!mappingResult.Valid)
                {
                    Result.AddErrorRange(mappingResult.ToErrorArray());
                }
                else
                {
                    Result = ItemCodeMappingService.Create(ref itemCodeMappingBo, ref trail);
                    if (Result.Valid)
                    {
                        ItemCodeMappingService.ProcessMappingDetail(itemCodeMappingBo, AuthUserId); // DO NOT TRAIL
                        CreateTrail(
                            itemCodeMappingBo.Id,
                            "Create Item Code Mapping"
                        );

                        model.Id = itemCodeMappingBo.Id;

                        SetCreateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = itemCodeMappingBo.Id });
                    }
                }
                AddResult(Result);
            }
            LoadPage();
            return View(model);
        }

        // GET: ItemCodeMapping/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            ItemCodeMappingBo itemCodeMappingBo = ItemCodeMappingService.Find(id);
            if (itemCodeMappingBo == null)
            {
                return RedirectToAction("Index");
            }
            LoadPage(itemCodeMappingBo);
            return View(new ItemCodeMappingViewModel(itemCodeMappingBo));
        }

        // POST: ItemCodeMapping/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, ItemCodeMappingViewModel model)
        {
            ItemCodeMappingBo itemCodeMappingBo = ItemCodeMappingService.Find(id);
            if (itemCodeMappingBo == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(itemCodeMappingBo.CreatedById, AuthUserId);
                bo.Id = itemCodeMappingBo.Id;

                TrailObject trail = GetNewTrailObject();
                Result = ItemCodeMappingService.Result();
                var mappingResult = ItemCodeMappingService.ValidateMapping(bo);
                if (!mappingResult.Valid)
                {
                    Result.AddErrorRange(mappingResult.ToErrorArray());
                }
                else
                {
                    Result = ItemCodeMappingService.Update(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        ItemCodeMappingService.ProcessMappingDetail(bo, AuthUserId); // DO NOT TRAIL
                        CreateTrail(
                            bo.Id,
                            "Update Item Code Mapping"
                        );

                        SetUpdateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = bo.Id });
                    }
                }
                AddResult(Result);
            }
            LoadPage(itemCodeMappingBo);
            return View(model);
        }

        // GET: ItemCodeMapping/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            ItemCodeMappingBo itemCodeMappingBo = ItemCodeMappingService.Find(id);
            if (itemCodeMappingBo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new ItemCodeMappingViewModel(itemCodeMappingBo));
        }

        // POST: ItemCodeMapping/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id)
        {
            ItemCodeMappingBo itemCodeMappingBo = ItemCodeMappingService.Find(id);
            if (itemCodeMappingBo == null)
            {
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = ItemCodeMappingService.Delete(itemCodeMappingBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    itemCodeMappingBo.Id,
                    "Delete Item Code Mapping"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = itemCodeMappingBo.Id });
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
                    var process = new ProcessItemCodeMapping()
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
            string downloadToken,
            int type, 
            int? InvoiceFieldPickListDetailId, 
            string TreatyType, 
            string TreatyCode, 
            int? ItemCodeId,
            int? ReportingType)
        {
            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            _db.Database.CommandTimeout = 0;
            var query = _db.ItemCodeMappings.Select(ItemCodeMappingViewModel.Expression());

            if (type == 2)
            {
                if (InvoiceFieldPickListDetailId.HasValue) query = query.Where(q => q.InvoiceFieldPickListDetailId == InvoiceFieldPickListDetailId);
                if (!string.IsNullOrEmpty(TreatyType)) query = query.Where(q => q.ItemCodeMappingDetails.Any(d => d.TreatyType == TreatyType));
                if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => q.ItemCodeMappingDetails.Any(d => d.TreatyCode == TreatyCode));
                if (ItemCodeId.HasValue) query = query.Where(q => q.ItemCodeId == ItemCodeId);
                if (ReportingType.HasValue) query = query.Where(q => q.ReportingType == ReportingType);
            }
            if (type == 3)
            {
                query = null;
            }

            var export = new ExportItemCodeMapping();
            export.HandleTempDirectory();

            if (query != null)
                export.SetQuery(query.Select(x => new ItemCodeMappingBo
                {
                    Id = x.Id,
                    InvoiceFieldPickListDetailId = x.InvoiceFieldPickListDetailId,
                    InvoiceField = x.InvoiceFieldPickListDetail.Code,
                    TreatyType = x.TreatyType,
                    TreatyCode = x.TreatyCode,
                    BusinessOriginPickListDetailId = x.BusinessOriginPickListDetailId,
                    BusinessOrigin = x.BusinessOriginPickListDetail.Code,
                    ItemCodeId = x.ItemCodeId,
                    ItemCode = x.ItemCode.Code,
                    ReportingType = x.ItemCode.ReportingType,
                }));

            export.Process();

            return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));
        }

        public void IndexPage()
        {
            DropDownItemCode();
            DropDownInvoiceField();
            DropDownBusinessOrigin();
            DropDownReportingType();
            SetViewBagMessage();
        }

        public void LoadPage(ItemCodeMappingBo itemCodeMappingBo = null)
        {
            DropDownItemCode();
            DropDownInvoiceField();
            DropDownBusinessOrigin();
            GetTreatyTypes();
            GetTreatyCodes();

            if (itemCodeMappingBo == null)
            {
                DropDownTreatyCode(TreatyCodeBo.StatusActive);
            }
            else
            {
                if (!string.IsNullOrEmpty(itemCodeMappingBo.TreatyCode))
                {
                    string[] treatyCodes = itemCodeMappingBo.TreatyCode.ToArraySplitTrim();
                    foreach (string treatyCodeStr in treatyCodes)
                    {
                        var treatyCode = TreatyCodeService.FindByCode(treatyCodeStr);
                        if (treatyCode != null)
                        {
                            if (treatyCode.Status == TreatyCodeBo.StatusInactive)
                            {
                                AddErrorMsg(string.Format(MessageBag.TreatyCodeStatusInactiveWithCode, treatyCodeStr));
                            }
                        }
                        else
                        {
                            AddErrorMsg(string.Format(MessageBag.TreatyCodeNotFound, treatyCodeStr));
                        }
                    }
                }
            }

            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownItemCode()
        {
            var items = GetEmptyDropDownList();
            foreach (ItemCodeBo itemCodeBo in ItemCodeService.Get())
            {
                items.Add(new SelectListItem { Text = itemCodeBo.ToString(), Value = itemCodeBo.Id.ToString() });
            }
            ViewBag.ItemCodes = items;
            return items;
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