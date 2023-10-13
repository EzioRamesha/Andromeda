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
    public class CedantController : BaseController
    {
        public const string Controller = "Cedant";

        // GET: Cedant
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(string Code, string Name, int? Type, string PartyCode, int? Status, string SortOrder, int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Code"] = Code,
                ["Name"] = Name,
                ["Type"] = Type,
                ["PartyCode"] = PartyCode,
                ["Status"] = Status,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCode = GetSortParam("Code");
            ViewBag.SortName = GetSortParam("Name");
            ViewBag.SortPartyCode = GetSortParam("PartyCode");

            var query = _db.Cedants.Select(CedantViewModel.Expression());
            if (!string.IsNullOrEmpty(Code)) query = query.Where(q => q.Code.Contains(Code));
            if (!string.IsNullOrEmpty(Name)) query = query.Where(q => q.Name.Contains(Name));
            if (!string.IsNullOrEmpty(PartyCode)) query = query.Where(q => q.PartyCode.Contains(PartyCode));
            if (Type != null) query = query.Where(q => q.CedingCompanyTypePickListDetailId == Type);
            if (Status != null) query = query.Where(q => q.Status == Status);

            if (SortOrder == Html.GetSortAsc("Code")) query = query.OrderBy(q => q.Code);
            else if (SortOrder == Html.GetSortDsc("Code")) query = query.OrderByDescending(q => q.Code);
            else if (SortOrder == Html.GetSortAsc("Name")) query = query.OrderBy(q => q.Name);
            else if (SortOrder == Html.GetSortDsc("Name")) query = query.OrderByDescending(q => q.Name);
            else if (SortOrder == Html.GetSortAsc("PartyCode")) query = query.OrderBy(q => q.PartyCode);
            else if (SortOrder == Html.GetSortDsc("PartyCode")) query = query.OrderByDescending(q => q.PartyCode);
            else query = query.OrderBy(q => q.Code);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: Cedant/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new CedantViewModel());
        }

        // POST: Cedant/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(CedantViewModel model)
        {
            if (ModelState.IsValid)
            {
                var cedantBo = model.FormBo(AuthUserId, AuthUserId);

                TrailObject trail = GetNewTrailObject();
                Result = CedantService.Create(ref cedantBo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        cedantBo.Id,
                        "Create Cedant"
                    );

                    model.Id = cedantBo.Id;

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = cedantBo.Id });
                }
                AddResult(Result);
            }
            LoadPage();
            return View(model);
        }

        // GET: Cedant/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            CedantBo cedantBo = CedantService.Find(id);
            if (cedantBo == null)
            {
                return RedirectToAction("Index");
            }
            LoadPage(cedantBo);
            return View(new CedantViewModel(cedantBo));
        }

        // POST: Cedant/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, CedantViewModel model)
        {
            CedantBo cedantBo = CedantService.Find(id);
            if (cedantBo == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                cedantBo = model.FormBo(cedantBo.CreatedById, AuthUserId);

                TrailObject trail = GetNewTrailObject();
                Result = CedantService.Update(ref cedantBo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        cedantBo.Id,
                        "Update Cedant"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = cedantBo.Id });
                }
                AddResult(Result);
            }
            LoadPage(cedantBo);
            return View(model);
        }

        // GET: Cedant/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            CedantBo cedantBo = CedantService.Find(id);
            if (cedantBo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new CedantViewModel(cedantBo));
        }

        // POST: Cedant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id)
        {
            CedantBo cedantBo = CedantService.Find(id);
            if (cedantBo == null)
            {
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = CedantService.Delete(cedantBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    cedantBo.Id,
                    "Delete Cedant"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = cedantBo.Id });
        }

        public void IndexPage()
        {
            DropDownStatus();
            DropDownType();
            DropDownCedantName();
            SetViewBagMessage();
        }

        public void LoadPage(CedantBo cedantBo = null)
        {
            if (cedantBo == null)
            {
                DropDownStatus();
                DropDownType();
            }
            else
            {
                DropDownStatus(cedantBo.Status);
                DropDownType(cedantBo.CedingCompanyTypePickListDetailId);
            }
            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownStatus(int? selectedId = null)
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= CedantBo.StatusInactive; i++)
            {
                var selected = i == selectedId;
                items.Add(new SelectListItem { Text = CedantBo.GetStatusName(i), Value = i.ToString(), Selected = selected });
            }
            ViewBag.StatusItems = items;
            return items;
        }

        public List<SelectListItem> DropDownType(int? selectedId = null)
        {
            var items = GetEmptyDropDownList();
            foreach (PickListDetailBo pickListDetail in PickListDetailService.GetByPickListId(PickListBo.CedingCompanyType))
            {
                var selected = pickListDetail.Id == selectedId;
                items.Add(new SelectListItem { Text = pickListDetail.Code, Value = pickListDetail.Id.ToString(), Selected = selected });
            }
            ViewBag.TypeItems = items;
            return items;
        }

        protected List<SelectListItem> DropDownCedantName()
        {
            var items = GetEmptyDropDownList();
            IList<CedantBo> bos = CedantService.Get();

            foreach (var cedant in bos)
            {
                items.Add(new SelectListItem { Text = cedant.Name, Value = cedant.Id.ToString() });
            }
            ViewBag.DropDownCedantsName = items;
            return items;
        }
    }
}
