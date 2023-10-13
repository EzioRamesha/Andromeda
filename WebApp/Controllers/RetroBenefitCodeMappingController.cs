using BusinessObject;
using BusinessObject.Retrocession;
using PagedList;
using Services;
using Services.Retrocession;
using Shared;
using Shared.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class RetroBenefitCodeMappingController : BaseController
    {
        public const string Controller = "RetroBenefitCodeMapping";

        // GET: RetroBenefitCodeMapping
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            int? BenefitId,
            bool? IsPerAnnum,
            int? RetroBenefitCodeId,
            string SortOrder,
            int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["BenefitId"] = BenefitId,
                ["IsPerAnnum"] = IsPerAnnum,
                ["RetroBenefitCodeId"] = RetroBenefitCodeId,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortBenefitId = GetSortParam("BenefitId");
            ViewBag.SortIsPerAnnum = GetSortParam("IsPerAnnum");

            var query = _db.RetroBenefitCodeMappings.Select(RetroBenefitCodeMappingViewModel.Expression());

            if (BenefitId.HasValue) query = query.Where(q => q.BenefitId == BenefitId);
            if (IsPerAnnum.HasValue) query = query.Where(q => q.IsPerAnnum == IsPerAnnum);
            if (RetroBenefitCodeId.HasValue) query = query.Where(q => q.RetroBenefitCodeMappingDetails.Any(r => r.RetroBenefitCodeId == RetroBenefitCodeId));

            if (SortOrder == Html.GetSortAsc("BenefitId")) query = query.OrderBy(q => q.Benefit.Code);
            else if (SortOrder == Html.GetSortDsc("BenefitId")) query = query.OrderByDescending(q => q.Benefit.Code);
            else if (SortOrder == Html.GetSortAsc("IsPerAnnum")) query = query.OrderBy(q => q.IsPerAnnum);
            else if (SortOrder == Html.GetSortDsc("IsPerAnnum")) query = query.OrderByDescending(q => q.IsPerAnnum);
            else query = query.OrderBy(q => q.Benefit.Code);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: RetroBenefitCodeMapping/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            RetroBenefitCodeMappingViewModel model = new RetroBenefitCodeMappingViewModel();

            LoadPage(model);
            ViewBag.Disabled = false;
            return View(model);
        }

        // POST: RetroBenefitCodeMapping/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, RetroBenefitCodeMappingViewModel model)
        {
            Result childResult = new Result();
            List<RetroBenefitCodeMappingDetailBo> retroBenefitCodeMappingDetailBos = model.GetRetroBenefitCodeMappingDetails(form, ref childResult);

            if (ModelState.IsValid)
            {
                Result = RetroBenefitCodeMappingService.Result();
                var bo = model.FormBo(AuthUserId, AuthUserId);

                if (childResult.Valid)
                    model.ValidateDuplicate(retroBenefitCodeMappingDetailBos, ref childResult);

                if (childResult.Valid)
                {
                    var trail = GetNewTrailObject();
                    Result = RetroBenefitCodeMappingService.Create(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        model.Id = bo.Id;
                        model.ProcessRetroBenefitCodeMappingTreaties(AuthUserId, ref trail);
                        model.ProcessRetroBenefitCodeMappingDetails(retroBenefitCodeMappingDetailBos, AuthUserId, ref trail);

                        CreateTrail(
                            bo.Id,
                            "Create Retro Benefit Code Mapping"
                        );

                        SetCreateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = bo.Id });
                    }
                }
                Result.AddErrorRange(childResult.ToErrorArray());
                AddResult(Result);
            }

            LoadPage(model, retroBenefitCodeMappingDetailBos);
            ViewBag.Disabled = false;
            return View(model);
        }

        // GET: RetroBenefitCodeMapping/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = RetroBenefitCodeMappingService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            RetroBenefitCodeMappingViewModel model = new RetroBenefitCodeMappingViewModel(bo);

            LoadPage(model);
            ViewBag.Disabled = false;
            return View(model);
        }

        // POST: RetroBenefitCodeMapping/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, RetroBenefitCodeMappingViewModel model)
        {
            var dbBo = RetroBenefitCodeMappingService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            Result childResult = new Result();
            List<RetroBenefitCodeMappingDetailBo> retroBenefitCodeMappingDetailBos = model.GetRetroBenefitCodeMappingDetails(form, ref childResult);

            if (ModelState.IsValid)
            {
                Result = RetroBenefitCodeMappingService.Result();
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();

                if (childResult.Valid)
                {
                    model.ValidateDuplicate(retroBenefitCodeMappingDetailBos, ref childResult);
                    if (childResult.Valid)
                    {
                        model.ProcessRetroBenefitCodeMappingTreaties(AuthUserId, ref trail);
                        model.ProcessRetroBenefitCodeMappingDetails(retroBenefitCodeMappingDetailBos, AuthUserId, ref trail);
                        Result = RetroBenefitCodeMappingService.Update(ref bo, ref trail);
                        if (Result.Valid)
                        {
                            CreateTrail(
                                bo.Id,
                                "Update Retro Benefit Code Mapping"
                            );

                            SetUpdateSuccessMessage(Controller);
                            return RedirectToAction("Edit", new { id = bo.Id });
                        }
                    }
                }
                Result.AddErrorRange(childResult.ToErrorArray());
                AddResult(Result);
            }

            model.BenefitBo = BenefitService.Find(model.BenefitId);
            LoadPage(model, retroBenefitCodeMappingDetailBos, false);
            ViewBag.Disabled = false;
            return View(model);
        }

        // GET: RetroBenefitCodeMapping/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = RetroBenefitCodeMappingService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            RetroBenefitCodeMappingViewModel model = new RetroBenefitCodeMappingViewModel(bo);
            LoadPage(model);
            ViewBag.Disabled = true;
            return View(model);
        }

        // POST: RetroBenefitCodeMapping/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, RetroBenefitCodeMappingViewModel model)
        {
            var bo = RetroBenefitCodeMappingService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = RetroBenefitCodeMappingService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Retro Benefit Code Mapping"
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
            DropDownBenefit();
            DropDownRetroBenefitCode();
            DropDownYesNoWithSelect();
            SetViewBagMessage();
        }

        public void LoadPage(RetroBenefitCodeMappingViewModel model, List<RetroBenefitCodeMappingDetailBo> retroBenefitCodeMappingDetailBos = null, bool searchChild = true)
        {
            DropDownYesNo();
            GetTreatyCodes();

            if (model.Id == 0)
            {
                // Create
                DropDownBenefit(BenefitBo.StatusActive);
                DropDownRetroBenefitCode(RetroBenefitCodeBo.StatusActive);
            }
            else
            {
                if (retroBenefitCodeMappingDetailBos.IsNullOrEmpty() && searchChild)
                {
                    retroBenefitCodeMappingDetailBos = RetroBenefitCodeMappingDetailService.GetByRetroBenefitCodeMappingId(model.Id).ToList();
                }

                DropDownBenefit(BenefitBo.StatusActive, model.BenefitId);
                if (model.BenefitBo != null && model.BenefitBo.Status == BenefitBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.BenefitStatusInactive);
                }

                List<int> ids = RetroBenefitCodeMappingDetailService.GetRetroBenefitCodeIdByRetroBenefitCodeMappingId(model.Id);
                DropDownRetroBenefitCode(RetroBenefitCodeBo.StatusActive, isDetail: true, selectedIds: ids);
            }

            ViewBag.RetroBenefitCodeMappingDetailBos = retroBenefitCodeMappingDetailBos;

            SetViewBagMessage();
        }
    }
}