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
    public class SourceController : BaseController
    {
        public const string Controller = "Source";

        // GET: Source
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string Name,
            string SortOrder,
            int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Name"] = Name,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortName = GetSortParam("Name");

            var query = _db.Sources.Select(SourceViewModel.Expression());

            if (!string.IsNullOrEmpty(Name)) query = query.Where(q => q.Name.Contains(Name));

            if (SortOrder == Html.GetSortAsc("Name")) query = query.OrderBy(q => q.Name);
            else if (SortOrder == Html.GetSortDsc("Name")) query = query.OrderByDescending(q => q.Name);
            else query = query.OrderByDescending(q => q.Id);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: Source/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new SourceViewModel());
        }

        // POST: Source/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(SourceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);

                var trail = GetNewTrailObject();
                Result = SourceService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;

                    CreateTrail(
                        bo.Id,
                        "Create Source"
                    );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }
            LoadPage();
            return View(model);
        }

        // GET: Source/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = SourceService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            LoadPage(bo);
            return View(new SourceViewModel(bo));
        }

        // POST: Source/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, SourceViewModel model)
        {
            var dbBo = SourceService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();

                Result = SourceService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Update Source"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage(dbBo);
            return View(model);
        }

        // GET: Source/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = SourceService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            return View(new SourceViewModel(bo));
        }

        // POST: Source/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, SourceViewModel model)
        {
            var bo = SourceService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = SourceService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Source"
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

        public void LoadPage(SourceBo bo = null)
        {
            var entity = new Source();
            var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Name");
            ViewBag.NameMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 128;

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