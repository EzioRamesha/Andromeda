using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BusinessObject;
using DataAccess.EntityFramework;
using PagedList;
using Services;
using Shared;
using Shared.DataAccess;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class DirectRetroConfigurationController : BaseController
    {
        public const string Controller = "DirectRetroConfiguration";

        // GET: DirectRetroConfiguration
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string Name,
            int? TreatyCodeId,
            string RetroParty,
            string SortOrder,
            int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Name"] = Name,
                ["TreatyCodeId"] = TreatyCodeId,
                ["RetroParty"] = RetroParty,
                ["SortOrder"] = SortOrder,
            };

            ViewBag.SortOrder = SortOrder;
            ViewBag.SortName = GetSortParam("Name");
            ViewBag.SortTreatyCodeId = GetSortParam("TreatyCodeId");

            var query = _db.DirectRetroConfigurations.Select(DirectRetroConfigurationViewModel.Expression());

            if (!string.IsNullOrEmpty(Name)) query = query.Where(q => q.Name.Contains(Name));
            if (TreatyCodeId.HasValue) query = query.Where(q => q.TreatyCodeId == TreatyCodeId);
            if (!string.IsNullOrEmpty(RetroParty)) query = query.Where(q => q.DirectRetroConfigurationMappings.Any(d => d.RetroParty == RetroParty));

            if (SortOrder == Html.GetSortAsc("Name")) query = query.OrderBy(q => q.Name);
            else if (SortOrder == Html.GetSortDsc("Name")) query = query.OrderByDescending(q => q.Name);
            else if (SortOrder == Html.GetSortAsc("TreatyCodeId")) query = query.OrderBy(q => q.TreatyCode.Code);
            else if (SortOrder == Html.GetSortDsc("CedanTreatyCodeIdtId")) query = query.OrderByDescending(q => q.TreatyCode.Code);
            else query = query.OrderByDescending(q => q.Id);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: DirectRetroConfiguration/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            DirectRetroConfigurationViewModel model = new DirectRetroConfigurationViewModel();
            LoadPage(model);
            ViewBag.Disabled = false;
            return View(model);
        }

        // POST: DirectRetroConfiguration/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, DirectRetroConfigurationViewModel model)
        {
            Result childResult = new Result();
            List<DirectRetroConfigurationDetailBo> directRetroConfigurationDetailBos = model.GetDirectRetroConfigurationDetails(form, ref childResult);

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);

                Result = DirectRetroConfigurationService.Result();
                var mappingResult = DirectRetroConfigurationService.ValidateMapping(bo);
                if (!mappingResult.Valid)
                {
                    Result.AddErrorRange(mappingResult.ToErrorArray());
                }

                if (!childResult.Valid)
                {
                    Result.AddErrorRange(childResult.ToErrorArray());
                }
                else
                {
                    model.ValidateDuplicate(directRetroConfigurationDetailBos, ref childResult);
                    if (!childResult.Valid)
                    {
                        Result.AddErrorRange(childResult.ToErrorArray());
                    }
                }

                if (Result.Valid)
                {
                    var trail = GetNewTrailObject();
                    Result = DirectRetroConfigurationService.Create(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        model.Id = bo.Id;
                        DirectRetroConfigurationService.ProcessMappingDetail(bo, AuthUserId); // DO NOT TRAIL
                        model.ProcessDirectRetroConfigurationDetails(directRetroConfigurationDetailBos, AuthUserId, ref trail);

                        CreateTrail(
                            bo.Id,
                            "Create Direct Retro Configuration"
                        );

                        SetCreateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = bo.Id });
                    }
                }
                AddResult(Result);
            }
            LoadPage(model, directRetroConfigurationDetailBos);
            ViewBag.Disabled = false;
            return View(model);
        }

        // GET: DirectRetroConfiguration/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = DirectRetroConfigurationService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }
            DirectRetroConfigurationViewModel model = new DirectRetroConfigurationViewModel(bo);
            LoadPage(model);
            ViewBag.Disabled = false;
            return View(model);
        }

        // POST: DirectRetroConfiguration/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, DirectRetroConfigurationViewModel model)
        {
            var dbBo = DirectRetroConfigurationService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            Result childResult = new Result();
            List<DirectRetroConfigurationDetailBo> directRetroConfigurationDetailBos = model.GetDirectRetroConfigurationDetails(form, ref childResult);

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                Result = DirectRetroConfigurationService.Result();
                var mappingResult = DirectRetroConfigurationService.ValidateMapping(bo);
                if (!mappingResult.Valid)
                {
                    Result.AddErrorRange(mappingResult.ToErrorArray());
                }

                if (!childResult.Valid)
                {
                    Result.AddErrorRange(childResult.ToErrorArray());
                }
                else
                {
                    model.ValidateDuplicate(directRetroConfigurationDetailBos, ref childResult);
                    if (!childResult.Valid)
                    {
                        Result.AddErrorRange(childResult.ToErrorArray());
                    }
                }

                if (Result.Valid)
                {
                    var trail = GetNewTrailObject();
                    Result = DirectRetroConfigurationService.Update(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        DirectRetroConfigurationService.ProcessMappingDetail(bo, AuthUserId); // DO NOT TRAIL
                        model.ProcessDirectRetroConfigurationDetails(directRetroConfigurationDetailBos, AuthUserId, ref trail);
                        CreateTrail(
                            bo.Id,
                            "Update Direct Retro Configuration"
                        );

                        SetUpdateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = bo.Id });
                    }
                }
                AddResult(Result);
            }

            LoadPage(model, directRetroConfigurationDetailBos);
            ViewBag.Disabled = false;
            return View(model);
        }

        // GET: DirectRetroConfiguration/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = DirectRetroConfigurationService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }
            DirectRetroConfigurationViewModel model = new DirectRetroConfigurationViewModel(bo);
            LoadPage(model);
            ViewBag.Disabled = true;
            return View(model);
        }

        // POST: DirectRetroConfiguration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, DirectRetroConfigurationViewModel model)
        {
            var bo = DirectRetroConfigurationService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = DirectRetroConfigurationService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Direct Retro Configuration"
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
            DropDownTreatyCode(foreign: false);
            SetViewBagMessage();
        }

        public void LoadPage(DirectRetroConfigurationViewModel model, List<DirectRetroConfigurationDetailBo> directRetroConfigurationDetailBos = null)
        {
            GetRetroPartyParties();
            DropDownYesNo();
            DropDownRetroParty();
            DropDownPremiumSpreadTable(PremiumSpreadTableBo.TypeDirectRetro);
            DropDownTreatyDiscountTable(TreatyDiscountTableBo.TypeDirectRetro);

            if (model.Id == 0)
            {
                // Create
                DropDownTreatyCode(TreatyCodeBo.StatusActive, foreign: false);
            }
            else
            {
                // Edit
                DropDownTreatyCode(TreatyCodeBo.StatusActive, model.TreatyCodeId, foreign: false);

                if (model.TreatyCodeBo != null && model.TreatyCodeBo.Status == TreatyCodeBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.TreatyCodeStatusInactive);
                }

                if (!string.IsNullOrEmpty(model.RetroParty))
                {
                    string[] retroParties = model.RetroParty.ToArraySplitTrim();
                    foreach (string retroPartyStr in retroParties)
                    {
                        var retroParty = RetroPartyService.FindByParty(retroPartyStr);
                        if (retroParty != null)
                        {
                            if (retroParty.Status == RetroPartyBo.StatusInactive)
                            {
                                AddErrorMsg(string.Format(MessageBag.RetroPartyStatusInactiveWithParty, retroPartyStr));
                            }
                        }
                        else
                        {
                            AddErrorMsg(string.Format(MessageBag.RetroPartyNotFound, retroPartyStr));
                        }
                    }
                }

                if (directRetroConfigurationDetailBos == null || directRetroConfigurationDetailBos.Count == 0)
                {
                    directRetroConfigurationDetailBos = DirectRetroConfigurationDetailService.GetByDirectRetroConfigurationId(model.Id, false).ToList();
                }
            }

            ViewBag.DirectRetroConfigurationDetailBos = directRetroConfigurationDetailBos;
            SetViewBagMessage();
        }
    }
}
