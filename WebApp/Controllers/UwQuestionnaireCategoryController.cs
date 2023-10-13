using DataAccess.Entities.TreatyPricing;
using PagedList;
using Services.TreatyPricing;
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
    public class UwQuestionnaireCategoryController : BaseController
    {
        public const string Controller = "UwQuestionnaireCategory";

        // GET: UwQuestionnaireCategory
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

            var query = _db.UwQuestionnaireCategories.Select(UwQuestionnaireCategoryViewModel.Expression());

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

        // GET: UwQuestionnaireCategory/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new UwQuestionnaireCategoryViewModel());
        }

        // POST: UwQuestionnaireCategory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, UwQuestionnaireCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);

                var trail = GetNewTrailObject();
                Result = UwQuestionnaireCategoryService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;

                    CreateTrail(
                        bo.Id,
                        "Create UW Questionnaire Category"
                    );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage();
            return View(model);
        }

        // GET: UwQuestionnaireCategory/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = UwQuestionnaireCategoryService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            LoadPage();
            return View(new UwQuestionnaireCategoryViewModel(bo));
        }

        // POST: UwQuestionnaireCategory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, UwQuestionnaireCategoryViewModel model)
        {
            var dbBo = UwQuestionnaireCategoryService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();

                Result = UwQuestionnaireCategoryService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Update UW Questionnaire Category"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage();
            return View(model);
        }

        // GET: UwQuestionnaireCategory/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = UwQuestionnaireCategoryService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            return View(new UwQuestionnaireCategoryViewModel(bo));
        }

        // POST: UwQuestionnaireCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, UwQuestionnaireCategoryViewModel model)
        {
            var bo = UwQuestionnaireCategoryService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = UwQuestionnaireCategoryService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete UW Questionnaire Category"
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

        public void LoadPage()
        {
            var entity = new UwQuestionnaireCategory();
            var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Code");
            ViewBag.CodeMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

            maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Name");
            ViewBag.NameMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

            SetViewBagMessage();
        }
    }
}