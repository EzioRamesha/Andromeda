using BusinessObject.Sanctions;
using DataAccess.Entities.Sanctions;
using PagedList;
using Services.Sanctions;
using Shared;
using Shared.DataAccess;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class SanctionKeywordController : BaseController
    {
        public const string Controller = "SanctionKeyword";

        // GET: SanctionKeyword
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string Code,
            string Keyword,
            string SortOrder,
            int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Code"] = Code,
                ["Keyword"] = Keyword,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCode = GetSortParam("Code");

            var query = _db.SanctionKeywords.Select(SanctionKeywordViewModel.Expression());

            if (!string.IsNullOrEmpty(Code)) query = query.Where(q => q.Code.Contains(Code));
            if (!string.IsNullOrEmpty(Keyword)) query = query.Where(q => q.SanctionKeywordDetails.Any(d => d.Keyword.Contains(Keyword)));

            if (SortOrder == Html.GetSortAsc("Code")) query = query.OrderBy(q => q.Code);
            else if (SortOrder == Html.GetSortDsc("Code")) query = query.OrderByDescending(q => q.Code);
            else query = query.OrderByDescending(q => q.Id);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: SanctionKeyword/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            SanctionKeywordViewModel model = new SanctionKeywordViewModel();

            LoadPage(model);
            ViewBag.Disabled = false;
            return View(model);
        }

        // POST: SanctionKeyword/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, SanctionKeywordViewModel model)
        {
            Result childResult = new Result();
            List<SanctionKeywordDetailBo> sanctionKeywordDetailBos = model.GetSanctionKeywordDetails(form, ref childResult);

            if (ModelState.IsValid)
            {
                Result = SanctionKeywordDetailService.Result();
                var bo = model.FormBo(AuthUserId, AuthUserId);

                if (childResult.Valid)
                    model.ValidateDuplicate(sanctionKeywordDetailBos, ref childResult);

                if (childResult.Valid)
                {
                    var trail = GetNewTrailObject();
                    Result = SanctionKeywordService.Create(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        model.Id = bo.Id;
                        model.ProcessSanctionKeywordDetails(sanctionKeywordDetailBos, AuthUserId, ref trail);

                        CreateTrail(
                            bo.Id,
                            "Create Sanction Keyword"
                        );

                        SetCreateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = bo.Id });
                    }
                }
                Result.AddErrorRange(childResult.ToErrorArray());
                AddResult(Result);
            }

            LoadPage(model, sanctionKeywordDetailBos);
            ViewBag.Disabled = false;
            return View(model);
        }

        // GET: SanctionKeyword/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = SanctionKeywordService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            SanctionKeywordViewModel model = new SanctionKeywordViewModel(bo);

            LoadPage(model);
            ViewBag.Disabled = false;
            return View(model);
        }

        // POST: SanctionKeyword/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, SanctionKeywordViewModel model)
        {
            var dbBo = SanctionKeywordService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            Result childResult = new Result();
            List<SanctionKeywordDetailBo> sanctionKeywordDetailBos = model.GetSanctionKeywordDetails(form, ref childResult);

            if (ModelState.IsValid)
            {
                Result = SanctionKeywordDetailService.Result();
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();

                if (childResult.Valid)
                {
                    model.ValidateDuplicate(sanctionKeywordDetailBos, ref childResult);
                    if (childResult.Valid)
                    {
                        model.ProcessSanctionKeywordDetails(sanctionKeywordDetailBos, AuthUserId, ref trail);
                        Result = SanctionKeywordService.Update(ref bo, ref trail);
                        if (Result.Valid)
                        {
                            CreateTrail(
                                bo.Id,
                                "Update Sanction Keyword"
                            );

                            SetUpdateSuccessMessage(Controller);
                            return RedirectToAction("Edit", new { id = bo.Id });
                        }
                    }
                }
                Result.AddErrorRange(childResult.ToErrorArray());
                AddResult(Result);
            }

            LoadPage(model, sanctionKeywordDetailBos, false);
            ViewBag.Disabled = false;
            return View(model);
        }

        // GET: SanctionKeyword/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            var bo = SanctionKeywordService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }
            SanctionKeywordViewModel model = new SanctionKeywordViewModel(bo);
            LoadPage(model);
            ViewBag.Disabled = true;
            return View(model);
        }

        // POST: SanctionKeyword/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, SanctionKeywordViewModel model)
        {
            var bo = SanctionKeywordService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = SanctionKeywordService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Sanction Keyword"
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

        public void LoadPage(SanctionKeywordViewModel model, List<SanctionKeywordDetailBo> sanctionKeywordDetailBos = null, bool SearchKeyword = true)
        {
            var entity = new SanctionKeyword();
            var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Code");
            ViewBag.CodeMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 10;

            var detailEntity = new SanctionKeywordDetail();
            maxLengthAttr = detailEntity.GetAttributeFrom<MaxLengthAttribute>("Keyword");
            ViewBag.KeywordMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

            if (model.Id == 0)
            {
                // Create
            }
            else
            {
                if (sanctionKeywordDetailBos.IsNullOrEmpty() && SearchKeyword)
                {
                    sanctionKeywordDetailBos = SanctionKeywordDetailService.GetBySanctionKeywordId(model.Id).ToList();
                }
            }

            ViewBag.SanctionKeywordDetailBos = sanctionKeywordDetailBos;
            SetViewBagMessage();
        }
    }
}