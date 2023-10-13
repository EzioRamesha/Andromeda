using BusinessObject;
using BusinessObject.Retrocession;
using ConsoleApp.Commands.ProcessDatas.Exports;
using DataAccess.Entities.Retrocession;
using PagedList;
using Services;
using Services.Retrocession;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class PerLifeDuplicationCheckController : BaseController
    {
        public const string Controller = "PerLifeDuplicationCheck";

        // GET: PerLifeDuplicationCheck
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string ConfigurationCode,
            string Description,
            string ReinsuranceEffectiveStartDate,
            string ReinsuranceEffectiveEndDate,
            int? NoOfTreatyCode,
            string TreatyCode,
            string SortOrder,
            int? Page)
        {
            DateTime? reinsuranceEffectiveStartDate = Util.GetParseDateTime(ReinsuranceEffectiveStartDate);
            DateTime? reinsuranceEffectiveEndDate = Util.GetParseDateTime(ReinsuranceEffectiveEndDate);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["ConfigurationCode"] = ConfigurationCode,
                ["Description"] = Description,
                ["ReinsuranceEffectiveStartDate"] = reinsuranceEffectiveStartDate.HasValue ? ReinsuranceEffectiveStartDate : null,
                ["ReinsuranceEffectiveEndDate"] = reinsuranceEffectiveEndDate.HasValue ? ReinsuranceEffectiveEndDate : null,
                ["NoOfTreatyCode"] = NoOfTreatyCode,
                ["TreatyCode"] = TreatyCode,
                ["SortOrder"] = SortOrder,
            };

            ViewBag.SortOrder = SortOrder;
            ViewBag.SortConfigurationCode = GetSortParam("ConfigurationCode");
            ViewBag.SortDescritpion = GetSortParam("Description");
            ViewBag.SortReinsuranceEffectiveStartDate = GetSortParam("ReinsuranceEffectiveStartDate");
            ViewBag.SortReinsuranceEffectiveEndDate = GetSortParam("ReinsuranceEffectiveEndDate");
            ViewBag.SortNoOfTreatyCode = GetSortParam("NoOfTreatyCode");

            var query = _db.PerLifeDuplicationChecks.Select(PerLifeDuplicationCheckViewModel.Expression());

            if (!string.IsNullOrEmpty(ConfigurationCode)) query = query.Where(q => q.ConfigurationCode.Contains(ConfigurationCode));
            if (!string.IsNullOrEmpty(Description)) query = query.Where(q => q.Description.Contains(Description));
            if (reinsuranceEffectiveStartDate.HasValue) query = query.Where(q => q.ReinsuranceEffectiveStartDate >= reinsuranceEffectiveStartDate);
            if (reinsuranceEffectiveEndDate.HasValue) query = query.Where(q => q.ReinsuranceEffectiveEndDate <= reinsuranceEffectiveEndDate);
            if (NoOfTreatyCode.HasValue) query = query.Where(q => q.NoOfTreatyCode == NoOfTreatyCode);
            if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => q.PerLifeDuplicationCheckDetails.Any(d => d.TreatyCode == TreatyCode));

            if (SortOrder == Html.GetSortAsc("ConfigurationCode")) query = query.OrderBy(q => q.ConfigurationCode);
            else if (SortOrder == Html.GetSortDsc("ConfigurationCode")) query = query.OrderByDescending(q => q.ConfigurationCode);
            else if (SortOrder == Html.GetSortAsc("Description")) query = query.OrderBy(q => q.Description);
            else if (SortOrder == Html.GetSortDsc("Description")) query = query.OrderByDescending(q => q.Description);
            else if (SortOrder == Html.GetSortAsc("ReinsuranceEffectiveStartDate")) query = query.OrderBy(q => q.ReinsuranceEffectiveStartDate);
            else if (SortOrder == Html.GetSortDsc("ReinsuranceEffectiveStartDate")) query = query.OrderByDescending(q => q.ReinsuranceEffectiveStartDate);
            else if (SortOrder == Html.GetSortAsc("ReinsuranceEffectiveEndDate")) query = query.OrderBy(q => q.ReinsuranceEffectiveEndDate);
            else if (SortOrder == Html.GetSortDsc("ReinsuranceEffectiveEndDate")) query = query.OrderByDescending(q => q.ReinsuranceEffectiveEndDate);
            else if (SortOrder == Html.GetSortAsc("NoOfTreatyCode")) query = query.OrderBy(q => q.NoOfTreatyCode);
            else if (SortOrder == Html.GetSortDsc("NoOfTreatyCode")) query = query.OrderByDescending(q => q.NoOfTreatyCode);
            else query = query.OrderBy(q => q.ConfigurationCode);

            ViewBag.Total = query.Count();
            IndexPage();

            return View(query.ToPagedList(Page ?? 1, PageSize)); 
        }

        public void IndexPage()
        {
            SetViewBagMessage();
        }

        // GET: PerLifeDuplicationCheck/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new PerLifeDuplicationCheckViewModel());
        }

        // POST: PerLifeDuplicationCheck/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, PerLifeDuplicationCheckViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);
                bo.NoOfTreatyCode = bo.TreatyCode.Split(',').Length;
                var trail = GetNewTrailObject();
                var treatyCodeResult = PerLifeDuplicationCheckService.ValidateTreatyCode(bo);
                Result = new Shared.DataAccess.Result();
                if (!treatyCodeResult.Valid)
                {
                    Result.AddErrorRange(treatyCodeResult.ToErrorArray());
                }
                else 
                {

                    Result = PerLifeDuplicationCheckService.Create(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        PerLifeDuplicationCheckService.ProcessDuplicationCheckDetail(bo, AuthUserId);
                        model.Id = bo.Id;

                        CreateTrail(
                            bo.Id,
                            "Create Per Life Duplication Check"
                            );

                        SetCreateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = bo.Id });
                    }
                }
                AddResult(Result);
            }
            else
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            }
            LoadPage();
            return View(model);
        }

        // GET:PerLifeDuplicationCheck/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = PerLifeDuplicationCheckService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }
            var model = new PerLifeDuplicationCheckViewModel(bo);
            LoadPage();
            return View(new PerLifeDuplicationCheckViewModel(bo));
        }

        // POST: PerLifeRetroCountry/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, PerLifeDuplicationCheckViewModel model)
        {
            var dbBo = PerLifeDuplicationCheckService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.NoOfTreatyCode = bo.TreatyCode.Split(',').Length;
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();
                Result = new Shared.DataAccess.Result();
                var treatyCodeResult = PerLifeDuplicationCheckService.ValidateTreatyCode(bo);
                if (!treatyCodeResult.Valid)
                {
                    Result.AddErrorRange(treatyCodeResult.ToErrorArray());
                }
                else
                {
                    Result = PerLifeDuplicationCheckService.Update(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        PerLifeDuplicationCheckService.ProcessDuplicationCheckDetail(bo, AuthUserId);
                        CreateTrail(
                            bo.Id,
                            "Update Per Life Duplication Check"
                            );

                        SetUpdateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = bo.Id });
                    }
                }
                AddResult(Result);
            }
            else
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            }

            LoadPage();
            return View(model);
        }

        // GET: PerLifeDuplicationCheck/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = PerLifeDuplicationCheckService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            return View(new PerLifeDuplicationCheckViewModel(bo));
        }

        // POST: PerLifeDuplicationCheck/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, PerLifeDuplicationCheckViewModel model)
        {
            var bo = PerLifeDuplicationCheckService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = PerLifeDuplicationCheckService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Per Life Duplication Check"
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

        public ActionResult Download(
            string downloadToken,
            int type,
            string ConfigurationCode,
            string Description,
            string ReinsuranceEffectiveStartDate,
            string ReinsuranceEffectiveEndDate,
            int? NoOfTreatyCode,
            string TreatyCode
        )
        {
            // type 1 = all
            // type 2 = filtered download

            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            _db.Database.CommandTimeout = 0;
            var query = _db.PerLifeDuplicationChecks.Select(PerLifeDuplicationCheckViewModel.Expression());

            if (type == 2) // filtered dowload
            {

                DateTime? reinsuranceEffectiveStartDate = Util.GetParseDateTime(ReinsuranceEffectiveStartDate);
                DateTime? reinsuranceEffectiveEndDate = Util.GetParseDateTime(ReinsuranceEffectiveEndDate);

                if (!string.IsNullOrEmpty(ConfigurationCode)) query = query.Where(q => q.ConfigurationCode.Contains(ConfigurationCode));
                if (!string.IsNullOrEmpty(Description)) query = query.Where(q => q.Description.Contains(Description));
                if (reinsuranceEffectiveStartDate.HasValue) query = query.Where(q => q.ReinsuranceEffectiveStartDate >= reinsuranceEffectiveStartDate);
                if (reinsuranceEffectiveEndDate.HasValue) query = query.Where(q => q.ReinsuranceEffectiveEndDate <= reinsuranceEffectiveEndDate);
                if (NoOfTreatyCode.HasValue) query = query.Where(q => q.NoOfTreatyCode == NoOfTreatyCode);
                if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => q.PerLifeDuplicationCheckDetails.Any(d => d.TreatyCode == TreatyCode));

            }

            var export = new ExportPerLifeDuplicationCheck();
            export.HandleTempDirectory();

            if (query != null)
                export.SetQuery(query.Select(x => new PerLifeDuplicationCheckBo
                {
                    Id = x.Id,
                    ConfigurationCode = x.ConfigurationCode,
                    Description = x.Description,
                    Inclusion = x.Inclusion,
                    ReinsuranceEffectiveStartDate = x.ReinsuranceEffectiveStartDate,
                    ReinsuranceEffectiveEndDate = x.ReinsuranceEffectiveEndDate,
                    TreatyCode = x.TreatyCode,
                    NoOfTreatyCode = x.NoOfTreatyCode,
                    EnableReinsuranceBasisCodeCheck = x.EnableReinsuranceBasisCodeCheck,
                }));

            export.Process();

            return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));
        }

        public void LoadPage()
        {
            var entity = new PerLifeDuplicationCheck();
            var configurationCodeMaxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("ConfigurationCode");
            ViewBag.ConfigurationCodeMaxLength = configurationCodeMaxLengthAttr != null ? configurationCodeMaxLengthAttr.Length : 30;
            var descriptionMaxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Description");
            ViewBag.DescriptionMaxLength = descriptionMaxLengthAttr != null ? descriptionMaxLengthAttr.Length : 255;
            GetTreatyCodes();
           
            SetViewBagMessage();
        }

    }
}