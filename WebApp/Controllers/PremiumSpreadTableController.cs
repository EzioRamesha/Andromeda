using BusinessObject;
using BusinessObject.Identity;
using ConsoleApp.Commands.ProcessDatas;
using ConsoleApp.Commands.ProcessDatas.Exports;
using DataAccess.Entities;
using PagedList;
using Services;
using Shared;
using Shared.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
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
    public class PremiumSpreadTableController : BaseController
    {
        public const string Controller = "PremiumSpreadTable";

        // GET: PremiumSpreadTable
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string Rule,
            int? Type,
            string Description,
            string SortOrder,
            int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Rule"] = Rule,
                ["Type"] = Type,
                ["Description"] = Description,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortType = GetSortParam("Type");
            ViewBag.SortRule = GetSortParam("Rule");
            ViewBag.SortDescription = GetSortParam("Description");

            var query = _db.PremiumSpreadTables.Select(PremiumSpreadTableViewModel.Expression());

            if (Type.HasValue) query = query.Where(q => q.Type == Type);
            if (!string.IsNullOrEmpty(Rule)) query = query.Where(q => q.Rule.Contains(Rule));
            if (!string.IsNullOrEmpty(Description)) query = query.Where(q => q.Description.Contains(Description));

            if (SortOrder == Html.GetSortAsc("Rule")) query = query.OrderBy(q => q.Rule);
            else if (SortOrder == Html.GetSortDsc("Rule")) query = query.OrderByDescending(q => q.Rule);
            else if (SortOrder == Html.GetSortAsc("Type")) query = query.OrderBy(q => q.Type);
            else if (SortOrder == Html.GetSortDsc("Type")) query = query.OrderByDescending(q => q.Type);
            else if (SortOrder == Html.GetSortAsc("Description")) query = query.OrderBy(q => q.Description);
            else if (SortOrder == Html.GetSortDsc("Description")) query = query.OrderByDescending(q => q.Description);
            else query = query.OrderByDescending(q => q.Id);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: PremiumSpreadTable/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            PremiumSpreadTableViewModel model = new PremiumSpreadTableViewModel();

            LoadPage(model);
            ViewBag.Disabled = false;
            return View(model);
        }

        // POST: PremiumSpreadTable/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, PremiumSpreadTableViewModel model)
        {
            Result childResult = new Result();
            List<PremiumSpreadTableDetailBo> premiumSpreadTableDetailBos = model.GetPremiumSpreadTableDetails(form, ref childResult);

            if (ModelState.IsValid)
            {
                Result = PremiumSpreadTableService.Result();
                var bo = model.FormBo(AuthUserId, AuthUserId);

                if (childResult.Valid)
                    model.ValidateDuplicate(premiumSpreadTableDetailBos, ref childResult);

                if (childResult.Valid)
                {
                    var trail = GetNewTrailObject();
                    Result = PremiumSpreadTableService.Create(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        model.Id = bo.Id;
                        model.ProcessPremiumSpreadTableDetails(premiumSpreadTableDetailBos, AuthUserId, ref trail);

                        CreateTrail(
                            bo.Id,
                            "Create Premium Spread Table"
                        );

                        SetCreateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = bo.Id });
                    }
                }
                Result.AddErrorRange(childResult.ToErrorArray());
                AddResult(Result);
            }

            LoadPage(model, premiumSpreadTableDetailBos);
            ViewBag.Disabled = false;
            return View(model);
        }

        // GET: PremiumSpreadTable/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = PremiumSpreadTableService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            PremiumSpreadTableViewModel model = new PremiumSpreadTableViewModel(bo);

            LoadPage(model);
            ViewBag.Disabled = false;
            return View(model);
        }

        // POST: PremiumSpreadTable/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, PremiumSpreadTableViewModel model)
        {
            var dbBo = PremiumSpreadTableService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            Result childResult = new Result();
            List<PremiumSpreadTableDetailBo> premiumSpreadTableDetailBos = model.GetPremiumSpreadTableDetails(form, ref childResult);

            if (ModelState.IsValid)
            {
                Result = PremiumSpreadTableService.Result();
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();

                if (childResult.Valid)
                {
                    model.ValidateDuplicate(premiumSpreadTableDetailBos, ref childResult);
                    if (childResult.Valid)
                    {
                        model.ProcessPremiumSpreadTableDetails(premiumSpreadTableDetailBos, AuthUserId, ref trail);
                        Result = PremiumSpreadTableService.Update(ref bo, ref trail);
                        if (Result.Valid)
                        {
                            CreateTrail(
                                bo.Id,
                                "Update Premium Spread Table"
                            );

                            SetUpdateSuccessMessage(Controller);
                            return RedirectToAction("Edit", new { id = bo.Id });
                        }
                    }
                }
                Result.AddErrorRange(childResult.ToErrorArray());
                AddResult(Result);
            }

            LoadPage(model, premiumSpreadTableDetailBos);
            ViewBag.Disabled = false;
            return View(model);
        }

        // GET: PremiumSpreadTable/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            var bo = PremiumSpreadTableService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }
            PremiumSpreadTableViewModel model = new PremiumSpreadTableViewModel(bo);
            LoadPage(model);
            ViewBag.Disabled = true;
            return View(model);
        }

        // POST: PremiumSpreadTable/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, PremiumSpreadTableViewModel model)
        {
            var bo = PremiumSpreadTableService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = PremiumSpreadTableService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Premium Spread Table"
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = AccessMatrixBo.PowerUpload, ReturnController = Controller)]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var process = new ProcessPremiumSpreadTable()
                    {
                        PostedFile = upload,
                        AuthUserId = AuthUserId,
                    };
                    process.Process();

                    if (process.Errors.Count() > 0 && process.Errors != null)
                    {
                        SetErrorSessionMsgArr(process.Errors.Take(50).ToList());
                    }

                    int createParent = process.GetProcessCount("Create Parent");
                    int updateParent = process.GetProcessCount("Update Parent");
                    int deleteParent = process.GetProcessCount("Delete Parent");
                    int createChild = process.GetProcessCount("Create Child");
                    int updateChild = process.GetProcessCount("Update Child");
                    int deleteChild = process.GetProcessCount("Delete Child");

                    SetSuccessSessionMsgArr(new List<string>
                    {
                        string.Format("Process File Successful, Total Parent Created: {0}, Total Parent Updated: {1}, Total Parent Deleted: {2}", createParent, updateParent, deleteParent),
                        string.Format("Process File Successful, Total Child Created: {0}, Total Child Updated: {1}, Total Child Deleted: {2}", createChild, updateChild, deleteChild),
                    });
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Download(
            string downloadToken,
            int downloadType,
            string Rule,
            int? Type,
            string Description
        )
        {
            // type 1 = all
            // type 2 = filtered download
            // type 3 = template download

            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            _db.Database.CommandTimeout = 0;
            var query = _db.PremiumSpreadTables.LeftOuterJoin(_db.PremiumSpreadTableDetails, t => t.Id, d => d.PremiumSpreadTableId, (t, d) => new PremiumSpreadTableWithDetail { Table = t, TableDetail = d })
                .Select(PremiumSpreadTableService.ExpressionWithDetail());

            if (downloadType == 2) // filtered dowload
            {
                if (Type.HasValue) query = query.Where(q => q.Type == Type);
                if (!string.IsNullOrEmpty(Rule)) query = query.Where(q => q.Rule.Contains(Rule));
                if (!string.IsNullOrEmpty(Description)) query = query.Where(q => q.Description.Contains(Description));
            }
            if (downloadType == 3)
            {
                query = null;
            }

            var export = new ExportPremiumSpreadTable();
            export.HandleTempDirectory();

            if (query != null)
                export.SetQuery(query);

            export.Process();

            return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));
        }

        public void IndexPage()
        {
            DropDownEmpty();
            DropDownType();
            SetViewBagMessage();
        }

        public void LoadPage(PremiumSpreadTableViewModel model, List<PremiumSpreadTableDetailBo> premiumSpreadTableDetailBos = null)
        {
            DropDownBenefit();
            DropDownType();
            GetBenefitCodes();

            var entity = new PremiumSpreadTable();
            var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Rule");
            ViewBag.RuleMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 30;

            maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Description");
            ViewBag.DescriptionMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

            if (model.Id == 0)
            {
                // Create
            }
            else
            {
                if (premiumSpreadTableDetailBos == null || premiumSpreadTableDetailBos.Count == 0)
                {
                    premiumSpreadTableDetailBos = PremiumSpreadTableDetailService.GetByPremiumSpreadTableId(model.Id).ToList();
                }

            }

            ViewBag.PremiumSpreadTableDetailBos = premiumSpreadTableDetailBos;
            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownType()
        {
            var items = GetEmptyDropDownList();
            foreach(var i in Enumerable.Range(1, PremiumSpreadTableBo.TypePerLife))
            {
                items.Add(new SelectListItem { Text = PremiumSpreadTableBo.GetTypeName(i), Value = i.ToString() });
            }
            ViewBag.DropDownTypes = items;
            return items;
        }
    }
}
