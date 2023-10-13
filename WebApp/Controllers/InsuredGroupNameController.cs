using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using PagedList;
using Services.TreatyPricing;
using Shared;
using Shared.Trails;
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
    public class InsuredGroupNameController : BaseController
    {
        public const string Controller = "InsuredGroupName";

        // GET: InsuredGroupName
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(string Name, string Description, int? Status, string SortOrder, int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Name"] = Name,
                ["Description"] = Description,
                ["Status"] = Status,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortName = GetSortParam("Name");
            ViewBag.SortDescription = GetSortParam("Description");
            ViewBag.SortDesc = GetSortParam("Description");

            var query = _db.InsuredGroupNames.Select(InsuredGroupNameViewModel.Expression());
            if (!string.IsNullOrEmpty(Name)) query = query.Where(q => q.Name.Contains(Name));
            if (!string.IsNullOrEmpty(Description)) query = query.Where(q => q.Description.Contains(Description));
            if (Status.HasValue) query = query.Where(q => q.Status == Status);

            if (SortOrder == Html.GetSortAsc("Name")) query = query.OrderBy(q => q.Name);
            else if (SortOrder == Html.GetSortDsc("Name")) query = query.OrderByDescending(q => q.Name);
            else if (SortOrder == Html.GetSortAsc("Description")) query = query.OrderBy(q => q.Description);
            else if (SortOrder == Html.GetSortDsc("Description")) query = query.OrderByDescending(q => q.Description);
            else query = query.OrderBy(q => q.Name);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: InsuredGroupName/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new InsuredGroupNameViewModel());
        }

        // POST: InsuredGroupName/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(InsuredGroupNameViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);
                TrailObject trail = GetNewTrailObject();
                Result = InsuredGroupNameService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Create Insured Group Name"
                    );
                    model.Id = bo.Id;

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }
            LoadPage();
            return View(model);
        }

        // GET: InsuredGroupName/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            InsuredGroupNameBo insuredGroupNameBo = InsuredGroupNameService.Find(id);
            if (insuredGroupNameBo == null)
            {
                return RedirectToAction("Index");
            }

            LoadPage(insuredGroupNameBo);
            return View(new InsuredGroupNameViewModel(insuredGroupNameBo));
        }

        // POST: InsuredGroupName/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, InsuredGroupNameViewModel model)
        {
            InsuredGroupNameBo insuredGroupNameBo = InsuredGroupNameService.Find(id);
            if (insuredGroupNameBo == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);
                TrailObject trail = GetNewTrailObject();
                Result = InsuredGroupNameService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        insuredGroupNameBo.Id,
                        "Update Insured Group Name"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = insuredGroupNameBo.Id });
                }
                AddResult(Result);
            }
            LoadPage(insuredGroupNameBo);
            return View(model);
        }

        // GET: InsuredGroupName/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            InsuredGroupNameBo insuredGroupNameBo = InsuredGroupNameService.Find(id);
            if (insuredGroupNameBo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new InsuredGroupNameViewModel(insuredGroupNameBo));
        }

        // POST: InsuredGroupName/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id)
        {
            InsuredGroupNameBo insuredGroupNameBo = InsuredGroupNameService.Find(id);
            if (insuredGroupNameBo == null)
            {
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = InsuredGroupNameService.Delete(insuredGroupNameBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    insuredGroupNameBo.Id,
                    "Delete Insured Group Name"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = insuredGroupNameBo.Id });
        }

        public void IndexPage()
        {
            DropDownStatus();
            SetViewBagMessage();
        }

        public void LoadPage(InsuredGroupNameBo insuredGroupNameBo = null)
        {
            var entity = new InsuredGroupName();
            var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Description");
            ViewBag.DescriptionMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

            maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Name");
            ViewBag.NameMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

            if (insuredGroupNameBo == null) 
                DropDownStatus();
            else 
                DropDownStatus(insuredGroupNameBo.Status);

            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownStatus(int? selectedId = null)
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= InsuredGroupNameBo.StatusInactive; i++)
            {
                var selected = i == selectedId;
                items.Add(new SelectListItem { Text = InsuredGroupNameBo.GetStatusName(i), Value = i.ToString(), Selected = selected });
            }
            ViewBag.StatusItems = items;
            return items;
        }
    }
}