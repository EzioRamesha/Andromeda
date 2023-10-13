using BusinessObject;
using BusinessObject.Identity;
using ConsoleApp.Commands.ProcessDatas;
using ConsoleApp.Commands.ProcessDatas.Exports;
using DataAccess.Entities;
using PagedList;
using Services;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
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
    public class BenefitController : BaseController
    {
        public const string Controller = "Benefit";

        // GET: Benefit
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(string Code, string BenefitType, int? EventCodeId, int? ClaimCodeId, string GST, string Description, int? Status, string SortOrder, int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Code"] = Code,
                ["BenefitType"] = BenefitType,
                ["Description"] = Description,
                ["Status"] = Status,
                ["GST"] = GST,
                ["EventCodeId"] = EventCodeId,
                ["ClaimCodeId"] = ClaimCodeId,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCode = GetSortParam("Code");
            ViewBag.SortBenefitType = GetSortParam("BenefitType");
            ViewBag.SortDescription = GetSortParam("Description");
            ViewBag.SortStatus = GetSortParam("Status");

            var query = _db.Benefits.Select(BenefitViewModel.Expression());
            if (!string.IsNullOrEmpty(Code)) query = query.Where(q => q.Code.Contains(Code));
            if (!string.IsNullOrEmpty(BenefitType)) query = query.Where(q => q.Type.Contains(BenefitType));
            if (!string.IsNullOrEmpty(GST)) query = query.Where(q => q.GST.ToString().Contains(GST));
            if (!string.IsNullOrEmpty(Description)) query = query.Where(q => q.Description.Contains(Description));
            if (Status != null) query = query.Where(q => q.Status == Status);
            if (EventCodeId != null) query = query.Where(q => q.BenefitDetails.Any(d => d.EventCodeId == EventCodeId));
            if (ClaimCodeId != null) query = query.Where(q => q.BenefitDetails.Any(d => d.ClaimCodeId == ClaimCodeId));

            if (SortOrder == Html.GetSortAsc("Code")) query = query.OrderBy(q => q.Code);
            else if (SortOrder == Html.GetSortDsc("Code")) query = query.OrderByDescending(q => q.Code);
            else if (SortOrder == Html.GetSortAsc("Description")) query = query.OrderBy(q => q.Description);
            else if (SortOrder == Html.GetSortDsc("Description")) query = query.OrderByDescending(q => q.Description);
            else if (SortOrder == Html.GetSortAsc("Status")) query = query.OrderBy(q => q.Status);
            else if (SortOrder == Html.GetSortDsc("Status")) query = query.OrderByDescending(q => q.Status);
            else query = query.OrderByDescending(q => q.Code);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: Benefit/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new BenefitViewModel());
        }

        // POST: Benefit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, BenefitViewModel model)
        {
            var benefitDetailBos = model.GetBenefitDetails(form);

            if (ModelState.IsValid)
            {
                Result childResult = BenefitDetailService.Validate(benefitDetailBos);
                if (!childResult.Valid)
                {
                    AddResult(childResult);
                    LoadPage(benefitDetailBos: benefitDetailBos);
                    return View(model);
                }

                var bo = model.FormBo(AuthUserId, AuthUserId);
                TrailObject trail = GetNewTrailObject();
                Result = BenefitService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;
                    model.ProcessBenefitDetails(benefitDetailBos, AuthUserId, ref trail);

                    CreateTrail(bo.Id, "Create Benefit");

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }
            LoadPage(benefitDetailBos: benefitDetailBos);
            return View(model);
        }

        // GET: Benefit/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            BenefitBo benefitBo = BenefitService.Find(id);
            if (benefitBo == null)
            {
                return RedirectToAction("Index");
            }
           
            var benefitDetailBos = BenefitDetailService.GetByBenefitId(id);
            LoadPage(benefitBo, benefitDetailBos);
            return View(new BenefitViewModel(benefitBo));
        }

        // POST: Benefit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, BenefitViewModel model)
        {
            var benefitDetailBos = model.GetBenefitDetails(form);

            BenefitBo benefitBo = BenefitService.Find(id);
            if (benefitBo == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                Result childResult = BenefitDetailService.Validate(benefitDetailBos);
                if (!childResult.Valid)
                {
                    AddResult(childResult);
                    LoadPage(benefitBo, benefitDetailBos);
                    return View(model);
                }

                var bo = model.FormBo(benefitBo.CreatedById, AuthUserId);
                TrailObject trail = GetNewTrailObject();
                Result = BenefitService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.ProcessBenefitDetails(benefitDetailBos, AuthUserId, ref trail);

                    CreateTrail(bo.Id, "Update Benefit");

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage(benefitBo, benefitDetailBos);
            return View(model);
        }

        // GET: Benefit/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            BenefitBo benefitBo = BenefitService.Find(id);
            if (benefitBo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new BenefitViewModel(benefitBo));
        }

        // POST: Benefit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id)
        {
            BenefitBo benefitBo = BenefitService.Find(id);
            if (benefitBo == null)
            {
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = BenefitService.Delete(benefitBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    benefitBo.Id,
                    "Delete Benefit"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = benefitBo.Id });
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
                    var process = new ProcessBenefit()
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
            string Code,
            string BenefitType,
            int? EventCodeId,
            int? ClaimCodeId,
            string GST,
            string Description,
            int? Status
        )
        {
            
            // type 1 = all
            // type 2 = filtered download
            // type 3 = template download

            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            _db.Database.CommandTimeout = 0;
            var query = _db.Benefits.Select(BenefitService.Expression());
            if (type == 2) // filtered dowload 
            {
                if (!string.IsNullOrEmpty(Code)) query = query.Where(q => q.Code.Contains(Code));
                if (!string.IsNullOrEmpty(BenefitType)) query = query.Where(q => q.Type.Contains(BenefitType));
                if (!string.IsNullOrEmpty(Description)) query = query.Where(q => q.Description.Contains(Description));
                if (Status != null) query = query.Where(q => q.Status == Status);
            }
            if (type == 3)
            {
                query = null;
            }

            var export = new ExportBenefit();
            export.HandleTempDirectory();
            export.SetQuery(query);
            export.Process();

            return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));
        }

        public void LoadPage(BenefitBo benefitBo = null, IList<BenefitDetailBo> benefitDetailBos = null)
        {
            DropDownEmpty();
            DropDownStatus();
            DropDownValuationBenefitCode();
            DropDownBenefitCategory();
            DropDownClaimCode(ClaimCodeBo.StatusActive);
            DropDownEventCode(EventCodeBo.StatusActive);

            var entity = new Benefit();
            var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Code");
            ViewBag.CodeMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 10;

            maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Description");
            ViewBag.DescriptionMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 128;

            if (benefitBo == null)
            {
                if (benefitDetailBos == null) ViewBag.BenefitDetailBos = Array.Empty<BenefitDetailBo>();
                else ViewBag.BenefitDetailBos = benefitDetailBos;
            }
            else
            {
                foreach (var benefitDetailBo in benefitDetailBos)
                {
                    if (benefitDetailBo.EventCodeBo != null && benefitDetailBo.EventCodeBo.Status == EventCodeBo.StatusInactive)
                    {
                        AddWarningMsg(string.Format(MessageBag.EventCodeStatusInactiveWithCode, benefitDetailBo.EventCodeBo.Code));
                    }

                    if (benefitDetailBo.ClaimCodeBo != null && benefitDetailBo.ClaimCodeBo.Status == ClaimCodeBo.StatusInactive)
                    {
                        AddWarningMsg(string.Format(MessageBag.ClaimCodeStatusInactiveWithCode, benefitDetailBo.ClaimCodeBo.Code));
                    }
                }
                ViewBag.BenefitDetailBos = benefitDetailBos;
            }
            SetViewBagMessage();
        }

        public void IndexPage()
        {
            DropDownGST();
            DropDownStatus();
            DropDownEventCode();
            DropDownClaimCode();
            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownGST()
        {
            var items = GetEmptyDropDownList();
            items.Add(new SelectListItem { Text = "No", Value = "false" });
            items.Add(new SelectListItem { Text = "Yes", Value = "true" });
            ViewBag.GSTItems = items;
            return items;
        }

        public List<SelectListItem> DropDownStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= BenefitBo.MaxStatus; i++)
            {
                items.Add(new SelectListItem { Text = BenefitBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.StatusList = items;
            return items;
        }

        public List<SelectListItem> DropDownValuationBenefitCode()
        {
            var items = GetEmptyDropDownList();
            foreach (PickListDetailBo pickListDetailBo in PickListDetailService.GetByPickListId(PickListBo.ValuationBenefitCode))
            {
                items.Add(new SelectListItem { Text = pickListDetailBo.Code, Value = pickListDetailBo.Id.ToString() });
            }
            ViewBag.ValuationBenefitCodeItems = items;
            return items;
        }

        public List<SelectListItem> DropDownBenefitCategory()
        {
            var items = GetEmptyDropDownList();
            foreach (PickListDetailBo pickListDetailBo in PickListDetailService.GetByPickListId(PickListBo.BenefitCategory))
            {
                items.Add(new SelectListItem { Text = pickListDetailBo.Code, Value = pickListDetailBo.Id.ToString() });
            }
            ViewBag.BenefitCategoryItems = items;
            return items;
        }

        [HttpPost]
        public JsonResult GetClaimCodes(int? claimCodeId = null)
        {
            IList<SelectListItem> claimCodes = DropDownClaimCode(ClaimCodeBo.StatusActive, claimCodeId);
            return Json(new { claimCodes });
        }

        [HttpPost]
        public JsonResult GetEventCodes(int? eventCodeId = null)
        {
            IList<SelectListItem> eventCodes = DropDownEventCode(EventCodeBo.StatusActive, eventCodeId);
            return Json(new { eventCodes });
        }
    }
}
