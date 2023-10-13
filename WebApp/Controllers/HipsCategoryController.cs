using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using PagedList;
using Services.TreatyPricing;
using Shared;
using Shared.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class HipsCategoryController : BaseController
    {
        public const string Controller = "HipsCategory";

        // GET: HipsCategory
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string Code,
            string Name,
            string SortOrder,
            int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Code"] = Code,
                ["Name"] = Name,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCode = GetSortParam("Code");
            ViewBag.SortName = GetSortParam("Name");

            var query = _db.HipsCategories.Select(HipsCategoryViewModel.Expression());

            if (!string.IsNullOrEmpty(Code)) query = query.Where(q => q.Code.Contains(Code));
            if (!string.IsNullOrEmpty(Name)) query = query.Where(q => q.Name.Contains(Name));

            if (SortOrder == Html.GetSortAsc("Code")) query = query.OrderBy(q => q.Code);
            else if (SortOrder == Html.GetSortDsc("Code")) query = query.OrderByDescending(q => q.Code);
            else if (SortOrder == Html.GetSortAsc("Name")) query = query.OrderBy(q => q.Name);
            else if (SortOrder == Html.GetSortDsc("Name")) query = query.OrderByDescending(q => q.Name);
            else query = query.OrderBy(q => q.Code);

            ViewBag.Total = query.Count();
            SetViewBagMessage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: HipsCategory/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            HipsCategoryViewModel model = new HipsCategoryViewModel();

            LoadPage(model);
            ViewBag.Disabled = false;
            return View(model);
        }

        // POST: HipsCategory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, HipsCategoryViewModel model)
        {
            Result childResult = new Result();
            List<HipsCategoryDetailBo> hipsCategoryDetailBos = model.GetHipsCategoryDetails(form, ref childResult);

            if (ModelState.IsValid)
            {
                Result = HipsCategoryService.Result();
                var bo = model.FormBo(AuthUserId, AuthUserId);

                if (childResult.Valid)
                    model.ValidateDuplicate(hipsCategoryDetailBos, ref childResult);

                if (childResult.Valid)
                {
                    var trail = GetNewTrailObject();
                    Result = HipsCategoryService.Create(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        model.Id = bo.Id;
                        model.ProcessHipsCategoryDetails(hipsCategoryDetailBos, AuthUserId, ref trail);

                        CreateTrail(
                            bo.Id,
                            "Create HIPS Category"
                        );

                        SetCreateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = bo.Id });
                    }
                }
                Result.AddErrorRange(childResult.ToErrorArray());
                AddResult(Result);
            }

            LoadPage(model, hipsCategoryDetailBos);
            ViewBag.Disabled = false;
            return View(model);
        }

        // GET: HipsCategory/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = HipsCategoryService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            HipsCategoryViewModel model = new HipsCategoryViewModel(bo);

            LoadPage(model);
            ViewBag.Disabled = false;
            return View(model);
        }

        // POST: HipsCategory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, HipsCategoryViewModel model)
        {
            var dbBo = HipsCategoryService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            Result childResult = new Result();
            List<HipsCategoryDetailBo> hipsCategoryDetailBos = model.GetHipsCategoryDetails(form, ref childResult);

            if (ModelState.IsValid)
            {
                Result = HipsCategoryService.Result();
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();

                if (childResult.Valid)
                {
                    model.ValidateDuplicate(hipsCategoryDetailBos, ref childResult);
                    if (childResult.Valid)
                    {
                        model.ProcessHipsCategoryDetails(hipsCategoryDetailBos, AuthUserId, ref trail);
                        Result = HipsCategoryService.Update(ref bo, ref trail);
                        if (Result.Valid)
                        {
                            CreateTrail(
                                bo.Id,
                                "Update HIPS Category"
                            );

                            SetUpdateSuccessMessage(Controller);
                            return RedirectToAction("Edit", new { id = bo.Id });
                        }
                    }
                }
                Result.AddErrorRange(childResult.ToErrorArray());
                AddResult(Result);
            }

            LoadPage(model, hipsCategoryDetailBos);
            ViewBag.Disabled = false;
            return View(model);
        }

        // GET: HipsCategory/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = HipsCategoryService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            HipsCategoryViewModel model = new HipsCategoryViewModel(bo);
            LoadPage(model);
            ViewBag.Disabled = true;
            return View(model);
        }

        // POST: HipsCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, HipsCategoryViewModel model)
        {
            var bo = HipsCategoryService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = HipsCategoryService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete HIPS Category"
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

        public void LoadPage(HipsCategoryViewModel model, List<HipsCategoryDetailBo> hipsCategoryDetailBos = null)
        {
            DropDownItemType();

            var entity = new HipsCategory();
            var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Code");
            ViewBag.CodeMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

            maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Name");
            ViewBag.NameMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

            var detailEntity = new HipsCategoryDetail();
            maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Subcategory");
            ViewBag.SubcategoryMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 20;

            maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Description");
            ViewBag.DescriptionMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

            if (model.Id == 0)
            {
                // Create
            }
            else
            {
                if (hipsCategoryDetailBos == null || hipsCategoryDetailBos.Count == 0)
                {
                    hipsCategoryDetailBos = HipsCategoryDetailService.GetByHipsCategoryId(model.Id).ToList();
                }

            }

            ViewBag.HipsCategoryDetailBos = hipsCategoryDetailBos;

            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownItemType()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= HipsCategoryDetailBo.ItemTypeMax; i++)
            {
                items.Add(new SelectListItem { Text = HipsCategoryDetailBo.GetItemTypeName(i), Value = i.ToString() });
            }
            ViewBag.DropDownItemTypes = items;
            return items;
        }

        [HttpPost]
        public JsonResult ValidateHipsSubCategoryDelete(int hipsSubCategoryId)
        {
            bool valid = true;

            if (TreatyPricingGroupReferralHipsTableService.CountByHipsSubCategoryId(hipsSubCategoryId) > 0)
                valid = false;

            return Json(new { valid });
        }
    }
}