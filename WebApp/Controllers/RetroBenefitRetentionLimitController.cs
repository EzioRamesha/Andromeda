using BusinessObject.Retrocession;
using ConsoleApp.Commands.ProcessDatas.Exports;
using DataAccess.Entities.Retrocession;
using PagedList;
using Services.Retrocession;
using Shared;
using Shared.DataAccess;
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
    public class RetroBenefitRetentionLimitController : BaseController
    {
        public const string Controller = "RetroBenefitRetentionLimit";

        // GET: RetroBenefitRetentionLimit
        public ActionResult Index(
            int? IndRetroBenefitCodeId,
            string IndDescription,
            string IndEffectiveStartDate,
            string IndEffectiveEndDate,
            string IndMinRetentionLimit,
            string IndReinsEffStartDate,
            string IndReinsEffEndDate,
            int? IndMinIssueAge,
            int? IndMaxIssueAge,
            string IndMortalityLimitFrom,
            string IndMortalityLimitTo,
            string IndMlreRetentionAmount,
            string IndMinReinsAmount,
            string SortOrder,
            int? Page
        )
        {
            DateTime? indEffectiveStartDate = Util.GetParseDateTime(IndEffectiveStartDate);
            DateTime? indEffectiveEndDate = Util.GetParseDateTime(IndEffectiveEndDate);
            DateTime? indReinsEffStartDate = Util.GetParseDateTime(IndReinsEffStartDate);
            DateTime? indReinsEffEndDate = Util.GetParseDateTime(IndReinsEffEndDate);

            double? indMinRetentionLimit = Util.StringToDouble(IndMinRetentionLimit);
            double? indMortalityLimitFrom = Util.StringToDouble(IndMortalityLimitFrom);
            double? indMortalityLimitTo = Util.StringToDouble(IndMortalityLimitTo);
            double? indMlreRetentionAmount = Util.StringToDouble(IndMlreRetentionAmount);
            double? indMinReinsAmount = Util.StringToDouble(IndMinReinsAmount);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["IndRetroBenefitCodeId"] = IndRetroBenefitCodeId,
                ["IndDescription"] = IndDescription,
                ["IndEffectiveStartDate"] = indEffectiveStartDate.HasValue ? IndEffectiveStartDate : null,
                ["IndEffectiveEndDate"] = indEffectiveEndDate.HasValue ? IndEffectiveEndDate : null,
                ["IndMinRetentionLimit"] = indMinRetentionLimit.HasValue ? IndMinRetentionLimit : null,
                ["IndReinsEffStartDate"] = indReinsEffStartDate.HasValue ? IndReinsEffStartDate : null,
                ["IndReinsEffEndDate"] = indReinsEffEndDate.HasValue ? IndReinsEffEndDate : null,
                ["IndMinIssueAge"] = IndMinIssueAge,
                ["IndMaxIssueAge"] = IndMaxIssueAge,
                ["IndMortalityLimitFrom"] = indMortalityLimitFrom.HasValue ? IndMortalityLimitFrom : null,
                ["IndMortalityLimitTo"] = indMortalityLimitTo.HasValue ? IndMortalityLimitTo : null,
                ["IndMlreRetentionAmount"] = indMlreRetentionAmount.HasValue ? IndMlreRetentionAmount : null,
                ["IndMinReinsAmount"] = indMinReinsAmount.HasValue ? IndMinReinsAmount : null,
                ["SortOrder"] = SortOrder,
            };

            ViewBag.SortOrder = SortOrder;

            ViewBag.SortIndRetroBenefitCodeId = GetSortParam("IndRetroBenefitCodeId");
            ViewBag.SortIndDescription = GetSortParam("IndDescription");
            ViewBag.SortIndEffectiveStartDate = GetSortParam("IndEffectiveStartDate");
            ViewBag.SortIndEffectiveEndDate = GetSortParam("IndEffectiveEndDate");
            ViewBag.SortIndMinRetentionLimit = GetSortParam("IndMinRetentionLimit");
            ViewBag.SortIndReinsEffStartDate = GetSortParam("IndReinsEffStartDate");
            ViewBag.SortIndReinsEffEndDate = GetSortParam("IndReinsEffEndDate");
            ViewBag.SortIndMinIssueAge = GetSortParam("IndMinIssueAge");
            ViewBag.SortIndMaxIssueAge = GetSortParam("IndMaxIssueAge");
            ViewBag.SortIndMortalityLimitFrom = GetSortParam("IndMortalityLimitFrom");
            ViewBag.SortIndMortalityLimitTo = GetSortParam("IndMortalityLimitTo");
            ViewBag.SortIndMlreRetentionAmount = GetSortParam("IndMlreRetentionAmount");
            ViewBag.SortIndMinReinsAmount = GetSortParam("IndMinReinsAmount");

            var query = _db.RetroBenefitRetentionLimits
                .Where(q => q.Type == RetroBenefitRetentionLimitBo.TypeIndividual)
                .LeftOuterJoin(_db.RetroBenefitRetentionLimitDetails, r => r.Id, d => d.RetroBenefitRetentionLimitId, (r, d) => new RetroBenefitRetentionLimitWithDetail { RetroBenefitRetentionLimit = r, RetroBenefitRetentionLimitDetail = d })
                .Select(RetroBenefitRetentionLimitViewModel.ExpressionWithDetail());

            if (IndRetroBenefitCodeId.HasValue) query = query.Where(q => q.RetroBenefitCodeId == IndRetroBenefitCodeId);
            if (!string.IsNullOrEmpty(IndDescription)) query = query.Where(q => q.Description.Contains(IndDescription));
            if (indEffectiveStartDate.HasValue) query = query.Where(q => q.EffectiveStartDate >= indEffectiveStartDate);
            if (indEffectiveEndDate.HasValue) query = query.Where(q => q.EffectiveEndDate <= indEffectiveEndDate);
            if (indMinRetentionLimit.HasValue) query = query.Where(q => q.MinRetentionLimit == indMinRetentionLimit);
            if (indReinsEffStartDate.HasValue) query = query.Where(q => q.ReinsEffStartDate >= indReinsEffStartDate);
            if (indReinsEffEndDate.HasValue) query = query.Where(q => q.ReinsEffEndDate <= indReinsEffEndDate);
            if (IndMinIssueAge.HasValue) query = query.Where(q => q.MinIssueAge == IndMinIssueAge);
            if (IndMaxIssueAge.HasValue) query = query.Where(q => q.MaxIssueAge == IndMaxIssueAge);
            if (indMortalityLimitFrom.HasValue) query = query.Where(q => q.MortalityLimitFrom == indMortalityLimitFrom);
            if (indMortalityLimitTo.HasValue) query = query.Where(q => q.MortalityLimitTo == indMortalityLimitTo);
            if (indMlreRetentionAmount.HasValue) query = query.Where(q => q.MlreRetentionAmount == indMlreRetentionAmount);
            if (indMinReinsAmount.HasValue) query = query.Where(q => q.MlreRetentionAmount == indMinReinsAmount);

            if (SortOrder == Html.GetSortAsc("IndRetroBenefitCodeId")) query = query.OrderBy(q => q.RetroBenefitCode.Code);
            else if (SortOrder == Html.GetSortDsc("IndRetroBenefitCodeId")) query = query.OrderByDescending(q => q.RetroBenefitCode.Code);
            else if (SortOrder == Html.GetSortAsc("IndDescription")) query = query.OrderBy(q => q.Description);
            else if (SortOrder == Html.GetSortDsc("IndDescription")) query = query.OrderByDescending(q => q.Description);
            else if (SortOrder == Html.GetSortAsc("IndEffectiveStartDate")) query = query.OrderBy(q => q.EffectiveStartDate);
            else if (SortOrder == Html.GetSortDsc("IndEffectiveStartDate")) query = query.OrderByDescending(q => q.EffectiveStartDate);
            else if (SortOrder == Html.GetSortAsc("IndEffectiveEndDate")) query = query.OrderBy(q => q.EffectiveEndDate);
            else if (SortOrder == Html.GetSortDsc("IndEffectiveEndDate")) query = query.OrderByDescending(q => q.EffectiveEndDate);
            else if (SortOrder == Html.GetSortAsc("IndMinRetentionLimit")) query = query.OrderBy(q => q.MinRetentionLimit);
            else if (SortOrder == Html.GetSortDsc("IndMinRetentionLimit")) query = query.OrderByDescending(q => q.MinRetentionLimit);
            else if (SortOrder == Html.GetSortAsc("IndReinsEffStartDate")) query = query.OrderBy(q => q.ReinsEffStartDate);
            else if (SortOrder == Html.GetSortDsc("IndReinsEffStartDate")) query = query.OrderByDescending(q => q.ReinsEffStartDate);
            else if (SortOrder == Html.GetSortAsc("IndReinsEffEndDate")) query = query.OrderBy(q => q.ReinsEffEndDate);
            else if (SortOrder == Html.GetSortDsc("IndReinsEffEndDate")) query = query.OrderByDescending(q => q.ReinsEffEndDate);
            else if (SortOrder == Html.GetSortAsc("IndMinIssueAge")) query = query.OrderBy(q => q.MinIssueAge);
            else if (SortOrder == Html.GetSortDsc("IndMinIssueAge")) query = query.OrderByDescending(q => q.MinIssueAge);
            else if (SortOrder == Html.GetSortAsc("IndMaxIssueAge")) query = query.OrderBy(q => q.MaxIssueAge);
            else if (SortOrder == Html.GetSortDsc("IndMaxIssueAge")) query = query.OrderByDescending(q => q.MaxIssueAge);
            else if (SortOrder == Html.GetSortAsc("IndMortalityLimitFrom")) query = query.OrderBy(q => q.MortalityLimitFrom);
            else if (SortOrder == Html.GetSortDsc("IndMortalityLimitFrom")) query = query.OrderByDescending(q => q.MortalityLimitFrom);
            else if (SortOrder == Html.GetSortAsc("IndMortalityLimitTo")) query = query.OrderBy(q => q.MortalityLimitTo);
            else if (SortOrder == Html.GetSortDsc("IndMortalityLimitTo")) query = query.OrderByDescending(q => q.MortalityLimitTo);
            else if (SortOrder == Html.GetSortAsc("IndMlreRetentionAmount")) query = query.OrderBy(q => q.MlreRetentionAmount);
            else if (SortOrder == Html.GetSortDsc("IndMlreRetentionAmount")) query = query.OrderByDescending(q => q.MlreRetentionAmount);
            else if (SortOrder == Html.GetSortAsc("IndMinReinsAmount")) query = query.OrderBy(q => q.MinReinsAmount);
            else if (SortOrder == Html.GetSortDsc("IndMinReinsAmount")) query = query.OrderByDescending(q => q.MinReinsAmount);

            else query = query.OrderBy(q => q.RetroBenefitCode.Code);

            ViewBag.ActiveTab = 1;

            ViewBag.IndividualTotal = query.Count();
            ViewBag.IndividualList = query.ToPagedList(Page ?? 1, PageSize);

            ViewBag.GroupRouteValue = new RouteValueDictionary { };
            ViewBag.GroupTotal = 0;
            ViewBag.GroupList = new List<RetroBenefitRetentionLimitViewModel>().ToPagedList(1, PageSize);

            IndexPage();
            return View();
        }

        // GET: RetroBenefitRetentionLimit/Group
        public ActionResult Group(
            int? GrpRetroBenefitCodeId,
            string GrpDescription,
            string GrpEffectiveStartDate,
            string GrpEffectiveEndDate,
            string GrpMinRetentionLimit,
            string GrpReinsEffStartDate,
            string GrpReinsEffEndDate,
            int? GrpMinIssueAge,
            int? GrpMaxIssueAge,
            string GrpMortalityLimitFrom,
            string GrpMortalityLimitTo,
            string GrpMlreRetentionAmount,
            string GrpMinReinsAmount,
            string SortOrder,
            int? Page
        )
        {
            DateTime? grpEffectiveStartDate = Util.GetParseDateTime(GrpEffectiveStartDate);
            DateTime? grpEffectiveEndDate = Util.GetParseDateTime(GrpEffectiveEndDate);
            DateTime? grpReinsEffStartDate = Util.GetParseDateTime(GrpReinsEffStartDate);
            DateTime? grpReinsEffEndDate = Util.GetParseDateTime(GrpReinsEffEndDate);

            double? grpMinRetentionLimit = Util.StringToDouble(GrpMinRetentionLimit);
            double? grpMortalityLimitFrom = Util.StringToDouble(GrpMortalityLimitFrom);
            double? grpMortalityLimitTo = Util.StringToDouble(GrpMortalityLimitTo);
            double? grpMlreRetentionAmount = Util.StringToDouble(GrpMlreRetentionAmount);
            double? grpMinReinsAmount = Util.StringToDouble(GrpMinReinsAmount);

            ViewBag.GroupRouteValue = new RouteValueDictionary
            {
                ["IndRetroBenefitCodeId"] = GrpRetroBenefitCodeId,
                ["IndDescription"] = GrpDescription,
                ["IndEffectiveStartDate"] = grpEffectiveStartDate.HasValue ? GrpEffectiveStartDate : null,
                ["IndEffectiveEndDate"] = grpEffectiveEndDate.HasValue ? GrpEffectiveEndDate : null,
                ["IndMinRetentionLimit"] = grpMinRetentionLimit.HasValue ? GrpMinRetentionLimit : null,
                ["IndReinsEffStartDate"] = grpReinsEffStartDate.HasValue ? GrpReinsEffStartDate : null,
                ["IndReinsEffEndDate"] = grpReinsEffEndDate.HasValue ? GrpReinsEffEndDate : null,
                ["IndMinIssueAge"] = GrpMinIssueAge,
                ["IndMaxIssueAge"] = GrpMaxIssueAge,
                ["IndMortalityLimitFrom"] = grpMortalityLimitFrom.HasValue ? GrpMortalityLimitFrom : null,
                ["IndMortalityLimitTo"] = grpMortalityLimitTo.HasValue ? GrpMortalityLimitTo : null,
                ["IndMlreRetentionAmount"] = grpMlreRetentionAmount.HasValue ? GrpMlreRetentionAmount : null,
                ["IndMinReinsAmount"] = grpMinReinsAmount.HasValue ? GrpMinReinsAmount : null,
                ["SortOrder"] = SortOrder,
            };

            ViewBag.SortOrder = SortOrder;

            ViewBag.SortGrpRetroBenefitCodeId = GetSortParam("GrpRetroBenefitCodeId");
            ViewBag.SortGrpDescription = GetSortParam("GrpDescription");
            ViewBag.SortGrpEffectiveStartDate = GetSortParam("GrpEffectiveStartDate");
            ViewBag.SortGrpEffectiveEndDate = GetSortParam("GrpEffectiveEndDate");
            ViewBag.SortGrpMinRetentionLimit = GetSortParam("GrpMinRetentionLimit");
            ViewBag.SortGrpReinsEffStartDate = GetSortParam("GrpReinsEffStartDate");
            ViewBag.SortGrpReinsEffEndDate = GetSortParam("GrpReinsEffEndDate");
            ViewBag.SortGrpMinIssueAge = GetSortParam("GrpMinIssueAge");
            ViewBag.SortGrpMaxIssueAge = GetSortParam("GrpMaxIssueAge");
            ViewBag.SortGrpMortalityLimitFrom = GetSortParam("GrpMortalityLimitFrom");
            ViewBag.SortGrpMortalityLimitTo = GetSortParam("GrpMortalityLimitTo");
            ViewBag.SortGrpMlreRetentionAmount = GetSortParam("GrpMlreRetentionAmount");
            ViewBag.SortGrpMinReinsAmount = GetSortParam("GrpMinReinsAmount");

            var query = _db.RetroBenefitRetentionLimits
                .Where(q => q.Type == RetroBenefitRetentionLimitBo.TypeGroup)
                .LeftOuterJoin(_db.RetroBenefitRetentionLimitDetails, r => r.Id, d => d.RetroBenefitRetentionLimitId, (r, d) => new RetroBenefitRetentionLimitWithDetail { RetroBenefitRetentionLimit = r, RetroBenefitRetentionLimitDetail = d })
                .Select(RetroBenefitRetentionLimitViewModel.ExpressionWithDetail());

            if (GrpRetroBenefitCodeId.HasValue) query = query.Where(q => q.RetroBenefitCodeId == GrpRetroBenefitCodeId);
            if (!string.IsNullOrEmpty(GrpDescription)) query = query.Where(q => q.Description.Contains(GrpDescription));
            if (grpEffectiveStartDate.HasValue) query = query.Where(q => q.EffectiveStartDate >= grpEffectiveStartDate);
            if (grpEffectiveEndDate.HasValue) query = query.Where(q => q.EffectiveEndDate <= grpEffectiveEndDate);
            if (grpMinRetentionLimit.HasValue) query = query.Where(q => q.MinRetentionLimit == grpMinRetentionLimit);
            if (grpReinsEffStartDate.HasValue) query = query.Where(q => q.ReinsEffStartDate >= grpReinsEffStartDate);
            if (grpReinsEffEndDate.HasValue) query = query.Where(q => q.ReinsEffEndDate <= grpReinsEffEndDate);
            if (GrpMinIssueAge.HasValue) query = query.Where(q => q.MinIssueAge == GrpMinIssueAge);
            if (GrpMaxIssueAge.HasValue) query = query.Where(q => q.MaxIssueAge == GrpMaxIssueAge);
            if (grpMortalityLimitFrom.HasValue) query = query.Where(q => q.MortalityLimitFrom == grpMortalityLimitFrom);
            if (grpMortalityLimitTo.HasValue) query = query.Where(q => q.MortalityLimitTo == grpMortalityLimitTo);
            if (grpMlreRetentionAmount.HasValue) query = query.Where(q => q.MlreRetentionAmount == grpMlreRetentionAmount);
            if (grpMinReinsAmount.HasValue) query = query.Where(q => q.MlreRetentionAmount == grpMinReinsAmount);

            if (SortOrder == Html.GetSortAsc("grpRetroBenefitCodeId")) query = query.OrderBy(q => q.RetroBenefitCode.Code);
            else if (SortOrder == Html.GetSortDsc("grpRetroBenefitCodeId")) query = query.OrderByDescending(q => q.RetroBenefitCode.Code);
            else if (SortOrder == Html.GetSortAsc("grpDescription")) query = query.OrderBy(q => q.Description);
            else if (SortOrder == Html.GetSortDsc("grpDescription")) query = query.OrderByDescending(q => q.Description);
            else if (SortOrder == Html.GetSortAsc("grpEffectiveStartDate")) query = query.OrderBy(q => q.EffectiveStartDate);
            else if (SortOrder == Html.GetSortDsc("grpEffectiveStartDate")) query = query.OrderByDescending(q => q.EffectiveStartDate);
            else if (SortOrder == Html.GetSortAsc("grpEffectiveEndDate")) query = query.OrderBy(q => q.EffectiveEndDate);
            else if (SortOrder == Html.GetSortDsc("grpEffectiveEndDate")) query = query.OrderByDescending(q => q.EffectiveEndDate);
            else if (SortOrder == Html.GetSortAsc("grpMinRetentionLimit")) query = query.OrderBy(q => q.MinRetentionLimit);
            else if (SortOrder == Html.GetSortDsc("grpMinRetentionLimit")) query = query.OrderByDescending(q => q.MinRetentionLimit);
            else if (SortOrder == Html.GetSortAsc("grpReinsEffStartDate")) query = query.OrderBy(q => q.ReinsEffStartDate);
            else if (SortOrder == Html.GetSortDsc("grpReinsEffStartDate")) query = query.OrderByDescending(q => q.ReinsEffStartDate);
            else if (SortOrder == Html.GetSortAsc("grpReinsEffEndDate")) query = query.OrderBy(q => q.ReinsEffEndDate);
            else if (SortOrder == Html.GetSortDsc("grpReinsEffEndDate")) query = query.OrderByDescending(q => q.ReinsEffEndDate);
            else if (SortOrder == Html.GetSortAsc("grpMinIssueAge")) query = query.OrderBy(q => q.MinIssueAge);
            else if (SortOrder == Html.GetSortDsc("grpMinIssueAge")) query = query.OrderByDescending(q => q.MinIssueAge);
            else if (SortOrder == Html.GetSortAsc("grpMaxIssueAge")) query = query.OrderBy(q => q.MaxIssueAge);
            else if (SortOrder == Html.GetSortDsc("grpMaxIssueAge")) query = query.OrderByDescending(q => q.MaxIssueAge);
            else if (SortOrder == Html.GetSortAsc("grpMortalityLimitFrom")) query = query.OrderBy(q => q.MortalityLimitFrom);
            else if (SortOrder == Html.GetSortDsc("grpMortalityLimitFrom")) query = query.OrderByDescending(q => q.MortalityLimitFrom);
            else if (SortOrder == Html.GetSortAsc("grpMortalityLimitTo")) query = query.OrderBy(q => q.MortalityLimitTo);
            else if (SortOrder == Html.GetSortDsc("grpMortalityLimitTo")) query = query.OrderByDescending(q => q.MortalityLimitTo);
            else if (SortOrder == Html.GetSortAsc("grpMlreRetentionAmount")) query = query.OrderBy(q => q.MlreRetentionAmount);
            else if (SortOrder == Html.GetSortDsc("grpMlreRetentionAmount")) query = query.OrderByDescending(q => q.MlreRetentionAmount);
            else if (SortOrder == Html.GetSortAsc("grpMinReinsAmount")) query = query.OrderBy(q => q.MinReinsAmount);
            else if (SortOrder == Html.GetSortDsc("grpMinReinsAmount")) query = query.OrderByDescending(q => q.MinReinsAmount);

            else query = query.OrderBy(q => q.RetroBenefitCode.Code);

            ViewBag.ActiveTab = 2;

            ViewBag.GroupTotal = query.Count();
            ViewBag.GroupList = query.ToPagedList(Page ?? 1, PageSize);

            ViewBag.RouteValue = new RouteValueDictionary { };
            ViewBag.IndividualTotal = 0;
            ViewBag.IndividualList = new List<RetroBenefitRetentionLimitViewModel>().ToPagedList(1, PageSize);

            IndexPage();
            return View("Index");
        }

        // GET: RetroBenefitRetentionLimit/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(int? Type)
        {
            RetroBenefitRetentionLimitViewModel model = new RetroBenefitRetentionLimitViewModel();

            int defaultType = Type ?? RetroBenefitRetentionLimitBo.TypeIndividual;

            model.Type = defaultType;
            LoadPage(model);
            ViewBag.Disabled = false;
            return View(model);
        }

        // POST: RetroBenefitRetentionLimit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, RetroBenefitRetentionLimitViewModel model)
        {
            Result childResult = new Result();
            List<RetroBenefitRetentionLimitDetailBo> retroBenefitRetentionLimitDetailBos = model.GetRetroBenefitRetentionLimitDetails(form, ref childResult);

            if (ModelState.IsValid)
            {
                Result = RetroBenefitRetentionLimitService.Result();
                var bo = model.FormBo(AuthUserId, AuthUserId);

                if (childResult.Valid)
                    model.ValidateDuplicate(retroBenefitRetentionLimitDetailBos, ref childResult);

                if (childResult.Valid)
                {
                    var trail = GetNewTrailObject();
                    Result = RetroBenefitRetentionLimitService.Create(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        model.Id = bo.Id;
                        model.ProcessRetroBenefitRetentionLimitDetails(retroBenefitRetentionLimitDetailBos, AuthUserId, ref trail);

                        CreateTrail(
                            bo.Id,
                            "Create Retro Benefit Retention Limit"
                        );

                        SetCreateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = bo.Id });
                    }
                }
                Result.AddErrorRange(childResult.ToErrorArray());
                AddResult(Result);
            }

            LoadPage(model, retroBenefitRetentionLimitDetailBos);
            ViewBag.Disabled = false;
            return View(model);
        }

        // GET: RetroBenefitRetentionLimit/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = RetroBenefitRetentionLimitService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            RetroBenefitRetentionLimitViewModel model = new RetroBenefitRetentionLimitViewModel(bo);

            LoadPage(model);
            ViewBag.Disabled = false;
            return View(model);
        }

        // POST: HipsCategory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, RetroBenefitRetentionLimitViewModel model)
        {
            var dbBo = RetroBenefitRetentionLimitService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            Result childResult = new Result();
            List<RetroBenefitRetentionLimitDetailBo> retroBenefitRetentionLimitDetailBos = model.GetRetroBenefitRetentionLimitDetails(form, ref childResult);

            if (ModelState.IsValid)
            {
                Result = RetroBenefitRetentionLimitService.Result();
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();

                if (childResult.Valid)
                {
                    model.ValidateDuplicate(retroBenefitRetentionLimitDetailBos, ref childResult);
                    if (childResult.Valid)
                    {
                        model.ProcessRetroBenefitRetentionLimitDetails(retroBenefitRetentionLimitDetailBos, AuthUserId, ref trail);
                        Result = RetroBenefitRetentionLimitService.Update(ref bo, ref trail);
                        if (Result.Valid)
                        {
                            CreateTrail(
                                bo.Id,
                                "Update Retro Benefit Retention Limit"
                            );

                            SetUpdateSuccessMessage(Controller);
                            return RedirectToAction("Edit", new { id = bo.Id });
                        }
                    }
                }
                Result.AddErrorRange(childResult.ToErrorArray());
                AddResult(Result);
            }

            model.RetroBenefitCodeBo = RetroBenefitCodeService.Find(model.RetroBenefitCodeId);
            LoadPage(model, retroBenefitRetentionLimitDetailBos);
            ViewBag.Disabled = false;
            return View(model);
        }

        // GET: HipsCategory/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = RetroBenefitRetentionLimitService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            RetroBenefitRetentionLimitViewModel model = new RetroBenefitRetentionLimitViewModel(bo);
            LoadPage(model);
            ViewBag.Disabled = true;
            return View(model);
        }

        // POST: HipsCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, RetroBenefitRetentionLimitViewModel model)
        {
            var bo = RetroBenefitRetentionLimitService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var type = bo.Type;
            var trail = GetNewTrailObject();
            Result = RetroBenefitRetentionLimitService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Retro Benefit Retention Limit"
                );

                SetDeleteSuccessMessage(Controller);
                if (type == RetroBenefitRetentionLimitBo.TypeIndividual)
                    return RedirectToAction("Index");
                else
                    return RedirectToAction("Group");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = bo.Id });
        }

        public ActionResult DownloadIndividual(
            string downloadToken,
            int type,
            int? IndRetroBenefitCodeId,
            string IndDescription,
            string IndEffectiveStartDate,
            string IndEffectiveEndDate,
            string IndMinRetentionLimit,
            string IndReinsEffStartDate,
            string IndReinsEffEndDate,
            int? IndMinIssueAge,
            int? IndMaxIssueAge,
            string IndMortalityLimitFrom,
            string IndMortalityLimitTo,
            string IndMlreRetentionAmount,
            string IndMinReinsAmount
        )
        {
            // type 1 = all
            // type 2 = filtered download

            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            _db.Database.CommandTimeout = 0;
            var query = _db.RetroBenefitRetentionLimits
                .Where(q => q.Type == RetroBenefitRetentionLimitBo.TypeIndividual)
                .LeftOuterJoin(_db.RetroBenefitRetentionLimitDetails, r => r.Id, d => d.RetroBenefitRetentionLimitId, (r, d) => new RetroBenefitRetentionLimitWithDetail { RetroBenefitRetentionLimit = r, RetroBenefitRetentionLimitDetail = d })
                .Select(RetroBenefitRetentionLimitService.ExpressionWithDetail());

            if (type == 2) // filtered dowload
            {
                DateTime? indEffectiveStartDate = Util.GetParseDateTime(IndEffectiveStartDate);
                DateTime? indEffectiveEndDate = Util.GetParseDateTime(IndEffectiveEndDate);
                DateTime? indReinsEffStartDate = Util.GetParseDateTime(IndReinsEffStartDate);
                DateTime? indReinsEffEndDate = Util.GetParseDateTime(IndReinsEffEndDate);

                double? indMinRetentionLimit = Util.StringToDouble(IndMinRetentionLimit);
                double? indMortalityLimitFrom = Util.StringToDouble(IndMortalityLimitFrom);
                double? indMortalityLimitTo = Util.StringToDouble(IndMortalityLimitTo);
                double? indMlreRetentionAmount = Util.StringToDouble(IndMlreRetentionAmount);
                double? indMinReinsAmount = Util.StringToDouble(IndMinReinsAmount);

                if (IndRetroBenefitCodeId.HasValue) query = query.Where(q => q.RetroBenefitCodeId == IndRetroBenefitCodeId);
                if (!string.IsNullOrEmpty(IndDescription)) query = query.Where(q => q.Description.Contains(IndDescription));
                if (indEffectiveStartDate.HasValue) query = query.Where(q => q.EffectiveStartDate >= indEffectiveStartDate);
                if (indEffectiveEndDate.HasValue) query = query.Where(q => q.EffectiveEndDate <= indEffectiveEndDate);
                if (indMinRetentionLimit.HasValue) query = query.Where(q => q.MinRetentionLimit == indMinRetentionLimit);
                if (indReinsEffStartDate.HasValue) query = query.Where(q => q.ReinsEffStartDate >= indReinsEffStartDate);
                if (indReinsEffEndDate.HasValue) query = query.Where(q => q.ReinsEffEndDate <= indReinsEffEndDate);
                if (IndMinIssueAge.HasValue) query = query.Where(q => q.MinIssueAge == IndMinIssueAge);
                if (IndMaxIssueAge.HasValue) query = query.Where(q => q.MaxIssueAge == IndMaxIssueAge);
                if (indMortalityLimitFrom.HasValue) query = query.Where(q => q.MortalityLimitFrom == indMortalityLimitFrom);
                if (indMortalityLimitTo.HasValue) query = query.Where(q => q.MortalityLimitTo == indMortalityLimitTo);
                if (indMlreRetentionAmount.HasValue) query = query.Where(q => q.MlreRetentionAmount == indMlreRetentionAmount);
                if (indMinReinsAmount.HasValue) query = query.Where(q => q.MlreRetentionAmount == indMinReinsAmount);
            }

            var export = new ExportRetroBenefitRetentionLimit() {
                PrefixFileName = "RetroBenefitRetentionLimit_Individual",
                IsWithDetail = true,
            };
            export.HandleTempDirectory();

            if (query != null)
                export.SetQuery(query);

            export.Process();

            return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));
        }

        public ActionResult DownloadGroup(
            string downloadToken,
            int type,
            int? GrpRetroBenefitCodeId,
            string GrpDescription,
            string GrpEffectiveStartDate,
            string GrpEffectiveEndDate,
            string GrpMinRetentionLimit,
            string GrpReinsEffStartDate,
            string GrpReinsEffEndDate,
            int? GrpMinIssueAge,
            int? GrpMaxIssueAge,
            string GrpMortalityLimitFrom,
            string GrpMortalityLimitTo,
            string GrpMlreRetentionAmount,
            string GrpMinReinsAmount
        )
        {
            // type 1 = all
            // type 2 = filtered download

            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            _db.Database.CommandTimeout = 0;
            var query = _db.RetroBenefitRetentionLimits
                .Where(q => q.Type == RetroBenefitRetentionLimitBo.TypeGroup)
                .LeftOuterJoin(_db.RetroBenefitRetentionLimitDetails, r => r.Id, d => d.RetroBenefitRetentionLimitId, (r, d) => new RetroBenefitRetentionLimitWithDetail { RetroBenefitRetentionLimit = r, RetroBenefitRetentionLimitDetail = d })
                .Select(RetroBenefitRetentionLimitService.ExpressionWithDetail());

            if (type == 2) // filtered dowload
            {
                DateTime? grpEffectiveStartDate = Util.GetParseDateTime(GrpEffectiveStartDate);
                DateTime? grpEffectiveEndDate = Util.GetParseDateTime(GrpEffectiveEndDate);
                DateTime? grpReinsEffStartDate = Util.GetParseDateTime(GrpReinsEffStartDate);
                DateTime? grpReinsEffEndDate = Util.GetParseDateTime(GrpReinsEffEndDate);

                double? grpMinRetentionLimit = Util.StringToDouble(GrpMinRetentionLimit);
                double? grpMortalityLimitFrom = Util.StringToDouble(GrpMortalityLimitFrom);
                double? grpMortalityLimitTo = Util.StringToDouble(GrpMortalityLimitTo);
                double? grpMlreRetentionAmount = Util.StringToDouble(GrpMlreRetentionAmount);
                double? grpMinReinsAmount = Util.StringToDouble(GrpMinReinsAmount);

                if (GrpRetroBenefitCodeId.HasValue) query = query.Where(q => q.RetroBenefitCodeId == GrpRetroBenefitCodeId);
                if (!string.IsNullOrEmpty(GrpDescription)) query = query.Where(q => q.Description.Contains(GrpDescription));
                if (grpEffectiveStartDate.HasValue) query = query.Where(q => q.EffectiveStartDate >= grpEffectiveStartDate);
                if (grpEffectiveEndDate.HasValue) query = query.Where(q => q.EffectiveEndDate <= grpEffectiveEndDate);
                if (grpMinRetentionLimit.HasValue) query = query.Where(q => q.MinRetentionLimit == grpMinRetentionLimit);
                if (grpReinsEffStartDate.HasValue) query = query.Where(q => q.ReinsEffStartDate >= grpReinsEffStartDate);
                if (grpReinsEffEndDate.HasValue) query = query.Where(q => q.ReinsEffEndDate <= grpReinsEffEndDate);
                if (GrpMinIssueAge.HasValue) query = query.Where(q => q.MinIssueAge == GrpMinIssueAge);
                if (GrpMaxIssueAge.HasValue) query = query.Where(q => q.MaxIssueAge == GrpMaxIssueAge);
                if (grpMortalityLimitFrom.HasValue) query = query.Where(q => q.MortalityLimitFrom == grpMortalityLimitFrom);
                if (grpMortalityLimitTo.HasValue) query = query.Where(q => q.MortalityLimitTo == grpMortalityLimitTo);
                if (grpMlreRetentionAmount.HasValue) query = query.Where(q => q.MlreRetentionAmount == grpMlreRetentionAmount);
                if (grpMinReinsAmount.HasValue) query = query.Where(q => q.MlreRetentionAmount == grpMinReinsAmount);
            }

            var export = new ExportRetroBenefitRetentionLimit() {
                PrefixFileName = "RetroBenefitRetentionLimit_Group",
                IsWithDetail = true,
            };
            export.HandleTempDirectory();

            if (query != null)
                export.SetQuery(query);

            export.Process();

            return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));
        }

        public void IndexPage()
        {
            DropDownRetroBenefitCode();
            SetViewBagMessage();
        }

        public void LoadPage(RetroBenefitRetentionLimitViewModel model, List<RetroBenefitRetentionLimitDetailBo> retroBenefitRetentionLimitDetailBos = null)
        {
            DropDownType();

            var entity = new RetroBenefitRetentionLimit();
            var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Description");
            ViewBag.DescriptionMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

            if (model.Id == 0)
            {
                // Create
                DropDownRetroBenefitCode(RetroBenefitCodeBo.StatusActive);
            }
            else
            {
                if (retroBenefitRetentionLimitDetailBos == null || retroBenefitRetentionLimitDetailBos.Count == 0)
                {
                    retroBenefitRetentionLimitDetailBos = RetroBenefitRetentionLimitDetailService.GetByRetroBenefitRetentionLimitId(model.Id).ToList();
                }

                DropDownRetroBenefitCode(RetroBenefitCodeBo.StatusActive, model.RetroBenefitCodeId);
                if (model.RetroBenefitCodeBo != null && model.RetroBenefitCodeBo.Status == RetroBenefitCodeBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.RetroBenefitCodeStatusInactive);
                }
            }

            ViewBag.RetroBenefitRetentionLimitDetailBos = retroBenefitRetentionLimitDetailBos;
            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownType()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= RetroBenefitRetentionLimitBo.TypeMax; i++)
            {
                items.Add(new SelectListItem { Text = RetroBenefitRetentionLimitBo.GetTypeName(i), Value = i.ToString() });
            }
            ViewBag.DropDownTypes = items;
            return items;
        }
    }
}