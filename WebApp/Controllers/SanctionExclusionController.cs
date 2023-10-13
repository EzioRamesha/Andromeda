using BusinessObject.Sanctions;
using DataAccess.Entities.Sanctions;
using PagedList;
using Services.Sanctions;
using Shared;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class SanctionExclusionController : BaseController
    {
        public const string Controller = "SanctionExclusion";

        // GET: SanctionExclusion
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string Keyword,
            string SortOrder,
            int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Keyword"] = Keyword,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortKeyword = GetSortParam("Keyword");

            var query = _db.SanctionExclusions.Select(SanctionExclusionViewModel.Expression());

            if (!string.IsNullOrEmpty(Keyword)) query = query.Where(q => q.Keyword.Contains(Keyword));

            if (SortOrder == Html.GetSortAsc("Keyword")) query = query.OrderBy(q => q.Keyword);
            else if (SortOrder == Html.GetSortDsc("Keyword")) query = query.OrderByDescending(q => q.Keyword);
            else query = query.OrderByDescending(q => q.Id);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: SanctionExclusion/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new SanctionExclusionViewModel());
        }

        // POST: SanctionExclusion/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(SanctionExclusionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);

                var trail = GetNewTrailObject();
                Result = SanctionExclusionService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;

                    CreateTrail(
                        bo.Id,
                        "Create Sanction Exclusion"
                    );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }
            LoadPage();
            return View(model);
        }

        // GET: SanctionExclusion/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = SanctionExclusionService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            LoadPage(bo);
            return View(new SanctionExclusionViewModel(bo));
        }

        // POST: SanctionExclusion/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, SanctionExclusionViewModel model)
        {
            var dbBo = SanctionExclusionService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();

                Result = SanctionExclusionService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Update Sanction Exclusion"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage(dbBo);
            return View(model);
        }

        // GET: SanctionExclusion/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = SanctionExclusionService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            return View(new SanctionExclusionViewModel(bo));
        }

        // POST: SanctionExclusion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, SanctionExclusionViewModel model)
        {
            var bo = SanctionExclusionService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = SanctionExclusionService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Sanction Exclusion"
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
            SetViewBagMessage();
        }

        public void LoadPage(SanctionExclusionBo bo = null)
        {
            var entity = new SanctionExclusion();
            var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Keyword");
            ViewBag.KeywordMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

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
    }
}