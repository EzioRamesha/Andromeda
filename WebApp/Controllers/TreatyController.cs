using BusinessObject;
using PagedList;
using Services;
using Services.RiDatas;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class TreatyController : BaseController
    {
        public const string Controller = "Treaty";

        [Auth(Controller = Controller, Power = "R")]
        // GET: Treaty
        public ActionResult Index(string TreatyIdCode, int? CedantId, string Description, string SortOrder, int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["TreatyIdCode"] = TreatyIdCode,
                ["CedantId"] = CedantId,
                ["Description"] = Description,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortTreatyIdCode = GetSortParam("TreatyIdCode");
            ViewBag.SortCedantId = GetSortParam("CedantId");

            var query = _db.Treaties.Select(TreatyViewModel.Expression());
            if (!string.IsNullOrEmpty(TreatyIdCode)) query = query.Where(q => q.TreatyIdCode.Contains(TreatyIdCode));
            if (CedantId != null) query = query.Where(q => q.CedantId == CedantId);
            if (!string.IsNullOrEmpty(Description)) query = query.Where(q => q.Description.Contains(Description));

            if (SortOrder == Html.GetSortAsc("TreatyIdCode")) query = query.OrderBy(q => q.TreatyIdCode);
            else if (SortOrder == Html.GetSortDsc("TreatyIdCode")) query = query.OrderByDescending(q => q.TreatyIdCode);
            else if (SortOrder == Html.GetSortAsc("CedantId")) query = query.OrderBy(q => q.Cedant.Code);
            else if (SortOrder == Html.GetSortDsc("CedantId")) query = query.OrderByDescending(q => q.Cedant.Code);
            else query = query.OrderBy(q => q.TreatyIdCode);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: Treaty/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            ViewBag.TreatyCodeBos = Array.Empty<TreatyCodeBo>();
            LoadPage();         
            return View(new TreatyViewModel());
        }

        // POST: Treaty/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, TreatyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var treatyBo = model.FormBo(AuthUserId, AuthUserId);

                Result childResult = new Result();
                List<TreatyCodeBo> treatyCodeBos = model.GetTreatyCodes(form, ref childResult);
                if (!childResult.Valid)
                {
                    AddResult(childResult);
                    ViewBag.TreatyCodeBos = treatyCodeBos;
                    LoadPage();
                    return View(model);
                }

                TrailObject trail = GetNewTrailObject();
                Result = TreatyService.Create(ref treatyBo, ref trail);
                if (Result.Valid)
                {
                    model.Id = treatyBo.Id;
                    model.ProcessTreatyCodes(form, AuthUserId, ref trail);

                    CreateTrail(
                        treatyBo.Id,
                        "Create Treaty"
                    );

                    model.Id = treatyBo.Id;
                    
                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = treatyBo.Id });
                }
                AddResult(Result);
            }

            ViewBag.TreatyCodeBos = TreatyCodeService.GetByTreatyId(model.Id);
            LoadPage();
            return View(model);
        }

        // GET: Treaty/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            TreatyBo treatyBo = TreatyService.Find(id);
            if (treatyBo == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.TreatyCodeBos = TreatyCodeService.GetByTreatyId(treatyBo.Id);
            LoadPage(treatyBo);
            return View(new TreatyViewModel(treatyBo));
        }

        // POST: Treaty/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, TreatyViewModel model)
        {
            TreatyBo treatyBo = TreatyService.Find(id);
            if (treatyBo == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                treatyBo = model.FormBo(treatyBo.CreatedById, AuthUserId);

                Result childResult = new Result();
                List<TreatyCodeBo> treatyCodeBos = model.GetTreatyCodes(form, ref childResult);
                if (!childResult.Valid)
                {
                    AddResult(childResult);
                    ViewBag.TreatyCodeBos = treatyCodeBos;
                    LoadPage(treatyBo);
                    return View(model);
                }

                TrailObject trail = GetNewTrailObject();
                Result = TreatyService.Update(ref treatyBo, ref trail);
                if (Result.Valid)
                {
                    model.ProcessTreatyCodes(form, AuthUserId, ref trail);

                    CreateTrail(
                        treatyBo.Id,
                        "Update Treaty"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = treatyBo.Id });
                }
                AddResult(Result);
            }

            ViewBag.TreatyCodeBos = TreatyCodeService.GetByTreatyId(treatyBo.Id);
            LoadPage(treatyBo);
            return View(model);
        }

        // GET: Treaty/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            TreatyBo treatyBo = TreatyService.Find(id);
            if (treatyBo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new TreatyViewModel(treatyBo));
        }

        // POST: Treaty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, TreatyViewModel model)
        {
            TreatyBo treatyBo = TreatyService.Find(id);
            if (treatyBo == null)
            {
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = TreatyService.Delete(treatyBo, ref trail);
            if (Result.Valid)
            {        
                CreateTrail(
                    treatyBo.Id,
                    "Delete Treaty"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = treatyBo.Id });
        }

        public void IndexPage()
        {
            DropDownEmpty();
            DropDownCedant();
            SetViewBagMessage();
        }

        public void LoadPage(TreatyBo treatyBo = null)
        {
            DropDownEmpty();
            DropDownStatus();
            DropDownBusinessOrigin();
            DropDownTreatyStatus();
            DropDownTreatyType();

            ViewBag.DropDownLineOfBusiness = GetPickListDetailIdDropDown(StandardOutputBo.TypeLineOfBusiness);

            if (treatyBo == null)
            {
                DropDownCedant(CedantBo.StatusActive);
                GetTreatyCodeList();
            }
            else
            {
                DropDownCedant(CedantBo.StatusActive, treatyBo.CedantId);
                if (treatyBo.CedantBo.Status == CedantBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.CedantStatusInactive);
                }

                GetTreatyCodeList(treatyBo.CedantId);
            }
            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= TreatyCodeBo.StatusInactive; i++)
            {
                items.Add(new SelectListItem { Text = TreatyCodeBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownStatus = items;
            return items;
        }

        public List<SelectListItem> DropDownTreatyStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (PickListDetailBo pickListDetailBo in PickListDetailService.GetByPickListId(PickListBo.TreatyStatus))
            {
                items.Add(new SelectListItem { Text = pickListDetailBo.Code, Value = pickListDetailBo.Id.ToString() });
            }
            ViewBag.DropDownTreatyStatus = items;
            return items;
        }

        public void GetTreatyCodeList(int cedantId = 0)
        {
            ViewBag.TreatyCodeItems = TreatyCodeService.GetByCedantId(cedantId);
        }

        [HttpPost]
        public JsonResult GetTreatyByCedant(int cedantId)
        {
            IList<TreatyBo> treatyBos = TreatyService.GetByCedantId(cedantId);
            return Json(new { treatyBos });
        }

        [HttpPost]
        public JsonResult GetTreatyCodeByCedant(int cedantId)
        {
            IList<TreatyCodeBo> treatyCodeBos = TreatyCodeService.GetByCedantId(cedantId);
            return Json(new { treatyCodeBos });
        }

        [HttpPost]
        public JsonResult GetTreatyOldCodeByTreatyCode(int treatyCodeId)
        {
            IList<TreatyOldCodeBo> treatyOldCodeBos = TreatyOldCodeService.GetByTreatyCodeId(treatyCodeId);
            int[] treatyOldCodeIds = treatyOldCodeBos.Select(q => q.OldTreatyCodeId).ToArray();
            return Json(new { treatyOldCodeIds });
        }

        [HttpPost]
        public JsonResult ValidateTreatyCodeDelete(int treatyCodeId)
        {
            bool valid = true;

            if (
                RiDataCorrectionService.CountByTreatyCodeId(treatyCodeId) > 0 ||
                TreatyBenefitCodeMappingService.CountByTreatyCodeId(treatyCodeId) > 0 ||
                Mfrs17CellMappingDetailService.CountByTreatyCodeId(treatyCodeId) > 0 ||
                Mfrs17ReportingDetailService.CountByTreatyCodeId(treatyCodeId) > 0 ||
                TreatyOldCodeService.CountByTreatyCodeId(treatyCodeId) > 0 ||
                DirectRetroService.CountByTreatyCodeId(treatyCodeId) > 0 ||
                ItemCodeMappingDetailService.CountByTreatyCodeId(treatyCodeId) > 0 ||
                RiDataService.IsExistByTreatyCode(treatyCodeId)
            )
            {
                valid = false;
            }

            return Json(new { valid });
        }
    }
}