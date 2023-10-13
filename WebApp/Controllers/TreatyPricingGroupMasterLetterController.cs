using BusinessObject;
using BusinessObject.TreatyPricing;
using ConsoleApp.Commands;
using PagedList;
using Services;
using Services.TreatyPricing;
using Shared;
using Shared.DataAccess;
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
    public class TreatyPricingGroupMasterLetterController : BaseController
    {
        public const string Controller = "TreatyPricingGroupMasterLetter";

        // GET: TreatyPricingGroupMasterLetter
        public ActionResult Index(
           string Code,
           int? CedantId,
           string SortOrder,
           int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Code"] = Code,
                ["CedantId"] = CedantId,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCode = GetSortParam("Code");
            ViewBag.SortCedantId = GetSortParam("CedantId");
            ViewBag.SortTotalRiGroupSlip = GetSortParam("TotalRiGroupSlip");

            var query = _db.TreatyPricingGroupMasterLetters.Select(TreatyPricingGroupMasterLetterViewModel.Expression());

            if (!string.IsNullOrEmpty(Code)) query = query.Where(q => q.Code.Contains(Code));
            if (CedantId.HasValue) query = query.Where(q => q.CedantId == CedantId);

            if (SortOrder == Html.GetSortAsc("Code")) query = query.OrderBy(q => q.Code);
            else if (SortOrder == Html.GetSortDsc("Code")) query = query.OrderByDescending(q => q.Code);
            else if (SortOrder == Html.GetSortAsc("CedantId")) query = query.OrderBy(q => q.Cedant.Code).ThenBy(q => q.Cedant.Name);
            else if (SortOrder == Html.GetSortDsc("CedantId")) query = query.OrderByDescending(q => q.Cedant.Code).ThenByDescending(q => q.Cedant.Name);
            else if (SortOrder == Html.GetSortAsc("TotalRiGroupSlip")) query = query.OrderBy(q => q.NoOfRiGroupSlip);
            else if (SortOrder == Html.GetSortDsc("TotalRiGroupSlip")) query = query.OrderByDescending(q => q.NoOfRiGroupSlip);
            else query = query.OrderBy(q => q.Code);

            IndexPage();

            ViewBag.Total = query.Count();            
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: TreatyPricingGroupMasterLetter/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            TreatyPricingGroupMasterLetterViewModel model = new TreatyPricingGroupMasterLetterViewModel();

            LoadPage(model);
            ViewBag.Disabled = false;
            return View(model);
        }

        // POST: TreatyPricingGroupMasterLetter/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, TreatyPricingGroupMasterLetterViewModel model)
        {
            List<TreatyPricingGroupMasterLetterGroupReferralBo> groupMasterLetterDetailBos = model.GetGroupMasterLetterDetails(form);

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);
                bo.CedantBo = CedantService.Find(bo.CedantId);
                bo.Code = TreatyPricingGroupMasterLetterService.GetNextCodeNo(DateTime.Now.Year, bo.CedantBo.Code);

                var trail = GetNewTrailObject();
                Result = TreatyPricingGroupMasterLetterService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;
                    model.ProcessGroupMasterLetterDetails(groupMasterLetterDetailBos, AuthUserId, ref trail);

                    CreateTrail(
                        bo.Id,
                        "Create Group Master Letter"
                    );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage(model, groupMasterLetterDetailBos);
            ViewBag.Disabled = false;
            return View(model);
        }

        // GET: TreatyPricingGroupMasterLetter/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = TreatyPricingGroupMasterLetterService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            TreatyPricingGroupMasterLetterViewModel model = new TreatyPricingGroupMasterLetterViewModel(bo);

            LoadPage(model);
            ViewBag.Disabled = false;
            return View(model);
        }

        // POST: TreatyPricingGroupMasterLetter/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, TreatyPricingGroupMasterLetterViewModel model)
        {
            var dbBo = TreatyPricingGroupMasterLetterService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            List<TreatyPricingGroupMasterLetterGroupReferralBo> groupMasterLetterDetailBos = model.GetGroupMasterLetterDetails(form);

            if (ModelState.IsValid)
            {
                Result = TreatyPricingGroupMasterLetterService.Result();
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();
                Result = TreatyPricingGroupMasterLetterService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;
                    model.ProcessGroupMasterLetterDetails(groupMasterLetterDetailBos, AuthUserId, ref trail);

                    CreateTrail(
                        bo.Id,
                        "Update Group Master Letter"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage(model, groupMasterLetterDetailBos);
            ViewBag.Disabled = false;
            return View(model);
        }

        // GET: TreatyPricingGroupMasterLetter/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();

            var bo = TreatyPricingGroupMasterLetterService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            TreatyPricingGroupMasterLetterViewModel model = new TreatyPricingGroupMasterLetterViewModel(bo);
            LoadPage(model);
            ViewBag.Disabled = true;
            return View(model);
        }

        // POST: TreatyPricingGroupMasterLetter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id)
        {
            var bo = TreatyPricingGroupMasterLetterService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = TreatyPricingGroupMasterLetterService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Group Master Letter"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = bo.Id });
        }

        public void IndexPage()
        {
            DropDownCedant();
            SetViewBagMessage();
        }

        public void LoadPage(TreatyPricingGroupMasterLetterViewModel model, List<TreatyPricingGroupMasterLetterGroupReferralBo> GroupMasterLetterDetailBos = null)
        {
            if (model.Id == 0)
            {
                DropDownCedant(CedantBo.StatusActive);
            }
            else
            {
                DropDownCedant(CedantBo.StatusActive, model.CedantId);

                if (GroupMasterLetterDetailBos == null || GroupMasterLetterDetailBos.Count == 0)
                {
                    GroupMasterLetterDetailBos = TreatyPricingGroupMasterLetterGroupReferralService.GetByGroupMasterLetterId(model.Id).ToList();
                }
            }
            ViewBag.GroupMasterLetterDetailBos = GroupMasterLetterDetailBos;

            SetViewBagMessage();
        }

        [HttpPost]
        public JsonResult GetByCedantIdHasRiGroupSlip(int cedantId)
        {
            var bos = TreatyPricingGroupReferralService.GetByCedantIdHasRiGroupSlipId(cedantId);
            return Json(new { GroupReferrals = bos });
        }

        public ActionResult Download(int id)
        {
            if (id != 0)
            {
                var process = new GenerateGroupMasterLetter()
                {
                    GroupMasterLetterId = id
                };
                process.Process();

                return File(process.FilePath, "text/csv", Path.GetFileName(process.FilePath));
            }
            else
            {
                return null;
            }
        }
    }
}