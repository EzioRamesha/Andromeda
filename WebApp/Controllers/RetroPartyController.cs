using BusinessObject;
using DataAccess.Entities;
using PagedList;
using Services;
using Shared;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class RetroPartyController : BaseController
    {
        public const string Controller = "RetroParty";

        // GET: RetroParty
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string Party,
            string Code,
            string Name,
            bool? IsDirectRetro,
            bool? IsPerLifeRetro,
            int? CountryCodePickListDetailId,
            string Description,
            int? Status,
            string AccountCode,
            string AccountCodeDescription,
            string SortOrder,
            int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Party"] = Party,
                ["Code"] = Code,
                ["Name"] = Name,
                ["IsDirectRetro"] = IsDirectRetro,
                ["IsPerLifeRetro"] = IsPerLifeRetro,
                ["CountryCodePickListDetailId"] = CountryCodePickListDetailId,
                ["Description"] = Description,
                ["Status"] = Status,
                ["AccountCode"] = AccountCode,
                ["AccountCodeDescription"] = AccountCodeDescription,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortParty = GetSortParam("Party");
            ViewBag.SortCode = GetSortParam("Code");
            ViewBag.SortName = GetSortParam("Name");
            ViewBag.SortIsDirectRetro = GetSortParam("IsDirectRetro");
            ViewBag.SortIsPerLifeRetro = GetSortParam("IsPerLifeRetro");
            ViewBag.SortCountryCodePickListDetailId = GetSortParam("CountryCodePickListDetailId");
            ViewBag.SortStatus = GetSortParam("Status");
            ViewBag.SortAccountCode = GetSortParam("AccountCode");

            var query = _db.RetroParties.Select(RetroPartyViewModel.Expression());

            if (!string.IsNullOrEmpty(Party)) query = query.Where(q => q.Party.Contains(Party));
            if (!string.IsNullOrEmpty(Code)) query = query.Where(q => q.Code.Contains(Code));
            if (!string.IsNullOrEmpty(Name)) query = query.Where(q => q.Name.Contains(Name));
            if (IsDirectRetro.HasValue) query = query.Where(q => q.IsDirectRetro == IsDirectRetro);
            if (IsPerLifeRetro.HasValue) query = query.Where(q => q.IsPerLifeRetro == IsPerLifeRetro);
            if (CountryCodePickListDetailId.HasValue) query = query.Where(q => q.CountryCodePickListDetailId == CountryCodePickListDetailId);
            if (!string.IsNullOrEmpty(Description)) query = query.Where(q => q.Description.Contains(Description));
            if (Status.HasValue) query = query.Where(q => q.Status == Status);
            if (!string.IsNullOrEmpty(AccountCode)) query = query.Where(q => q.AccountCode.Contains(AccountCode));
            if (!string.IsNullOrEmpty(AccountCodeDescription)) query = query.Where(q => q.AccountCodeDescription.Contains(AccountCodeDescription));

            if (SortOrder == Html.GetSortAsc("Party")) query = query.OrderBy(q => q.Party);
            else if (SortOrder == Html.GetSortDsc("Party")) query = query.OrderByDescending(q => q.Party);
            else if (SortOrder == Html.GetSortAsc("Code")) query = query.OrderBy(q => q.Code);
            else if (SortOrder == Html.GetSortDsc("Code")) query = query.OrderByDescending(q => q.Code);
            else if (SortOrder == Html.GetSortAsc("Name")) query = query.OrderBy(q => q.Name);
            else if (SortOrder == Html.GetSortDsc("Name")) query = query.OrderByDescending(q => q.Name);
            else if (SortOrder == Html.GetSortAsc("IsDirectRetro")) query = query.OrderBy(q => q.IsDirectRetro);
            else if (SortOrder == Html.GetSortDsc("IsDirectRetro")) query = query.OrderByDescending(q => q.IsDirectRetro);
            else if (SortOrder == Html.GetSortAsc("IsPerLifeRetro")) query = query.OrderBy(q => q.IsPerLifeRetro);
            else if (SortOrder == Html.GetSortDsc("IsPerLifeRetro")) query = query.OrderByDescending(q => q.IsPerLifeRetro);
            else if (SortOrder == Html.GetSortAsc("CountryCodePickListDetailId")) query = query.OrderBy(q => q.CountryCodePickListDetailId);
            else if (SortOrder == Html.GetSortDsc("CountryCodePickListDetailId")) query = query.OrderByDescending(q => q.CountryCodePickListDetailId);
            else if (SortOrder == Html.GetSortAsc("Status")) query = query.OrderBy(q => q.Status);
            else if (SortOrder == Html.GetSortDsc("Status")) query = query.OrderByDescending(q => q.Status);
            else if (SortOrder == Html.GetSortAsc("AccountCode")) query = query.OrderBy(q => q.AccountCode);
            else if (SortOrder == Html.GetSortDsc("AccountCode")) query = query.OrderByDescending(q => q.AccountCode);
            else query = query.OrderByDescending(q => q.Id);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: RetroParty/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new RetroPartyViewModel());
        }

        // POST: RetroParty/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, RetroPartyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);

                var trail = GetNewTrailObject();
                Result = RetroPartyService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;

                    CreateTrail(
                        bo.Id,
                        "Create Retro Party"
                    );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage();
            return View(model);
        }

        // GET: RetroParty/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = RetroPartyService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            LoadPage(bo);
            return View(new RetroPartyViewModel(bo));
        }

        // POST: RetroParty/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, RetroPartyViewModel model)
        {
            var dbBo = RetroPartyService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();

                Result = RetroPartyService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Update Retro Party"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage(dbBo);
            return View(model);
        }

        // GET: RetroParty/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = RetroPartyService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            return View(new RetroPartyViewModel(bo));
        }

        // POST: RetroParty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, RetroPartyViewModel model)
        {
            var bo = RetroPartyService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = RetroPartyService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Retro Party"
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
            DropDownEmpty();
            DropDownStatus();
            DropDownCountryCode();
            DropDownYesNoWithSelect();
            SetViewBagMessage();
        }

        public void LoadPage(RetroPartyBo bo = null)
        {
            DropDownStatus();
            DropDownCountryCode();

            var entity = new RetroParty();
            var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Party");
            ViewBag.PartyMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 50;

            maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Code");
            ViewBag.CodeMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 50;

            maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Name");
            ViewBag.NameMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

            maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Description");
            ViewBag.DescriptionMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

            maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("AccountCode");
            ViewBag.AccountCodeMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 10;

            maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("AccountCodeDescription");
            ViewBag.AccountCodeDescriptionMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

            if (bo == null)
            {
                // Create
            }
            else
            {
                // Edit
            }
            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= RetroPartyBo.MaxStatus; i++)
            {
                items.Add(new SelectListItem { Text = RetroPartyBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownStatuses = items;
            return items;
        }
    }
}
