using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.Retrocession;
using ConsoleApp.Commands.ProcessDatas;
using PagedList;
using Services;
using Services.Retrocession;
using Shared;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class PerLifeAggregationController : BaseController
    {
        public const string Controller = "PerLifeAggregation";

        // GET: PerLifeAggregation
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string SoaQuarter,
            int? CutOffId,
            string ProcessingDate,
            int? FundsAccountingTypePickListDetailId,
            int? PersonInChargeId,
            int? Status,
            string SortOrder,
            int? Page)
        {
            DateTime? processingDate = Util.GetParseDateTime(ProcessingDate);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["SoaQuarter"] = SoaQuarter,
                ["CutOffId"] = CutOffId,
                ["ProcessingDate"] = processingDate.HasValue ? ProcessingDate : null,
                ["FundsAccountingTypePickListDetailId"] = FundsAccountingTypePickListDetailId,
                ["PersonInChargeId"] = PersonInChargeId,
                ["Status"] = Status,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortSoaQuarter = GetSortParam("SoaQuarter");
            ViewBag.SortCutOffId = GetSortParam("CutOffId");
            ViewBag.SortProcessingDate = GetSortParam("ProcessingDate");
            ViewBag.SortFundsAccountingTypePickListDetailId = GetSortParam("FundsAccountingTypePickListDetailId");
            ViewBag.SortPersonInChargeId = GetSortParam("PersonInChargeId");
            ViewBag.SortDaaStatus = GetSortParam("DaaStatus");
            ViewBag.SortStatus = GetSortParam("Status");

            var query = _db.GetPerLifeAggregations().Select(PerLifeAggregationViewModel.Expression());

            if (!string.IsNullOrEmpty(SoaQuarter)) query = query.Where(q => q.SoaQuarter == SoaQuarter);
            if (CutOffId.HasValue) query = query.Where(q => q.CutOffId == CutOffId);
            if (processingDate.HasValue) query = query.Where(q => q.ProcessingDate == processingDate);
            if (FundsAccountingTypePickListDetailId.HasValue) query = query.Where(q => q.FundsAccountingTypePickListDetailId == FundsAccountingTypePickListDetailId);
            if (PersonInChargeId.HasValue) query = query.Where(q => q.PersonInChargeId == PersonInChargeId);
            if (Status.HasValue) query = query.Where(q => q.Status == Status);

            if (SortOrder == Html.GetSortAsc("SoaQuarter")) query = query.OrderBy(q => q.SoaQuarter);
            else if (SortOrder == Html.GetSortDsc("SoaQuarter")) query = query.OrderByDescending(q => q.SoaQuarter);
            else if (SortOrder == Html.GetSortAsc("CutOffId")) query = query.OrderBy(q => q.CutOff.Quarter);
            else if (SortOrder == Html.GetSortDsc("CutOffId")) query = query.OrderByDescending(q => q.CutOff.Quarter);
            else if (SortOrder == Html.GetSortAsc("ProcessingDate")) query = query.OrderBy(q => q.ProcessingDate);
            else if (SortOrder == Html.GetSortDsc("ProcessingDate")) query = query.OrderByDescending(q => q.ProcessingDate);
            else if (SortOrder == Html.GetSortAsc("FundsAccountingTypePickListDetailId")) query = query.OrderBy(q => q.FundsAccountingTypePickListDetail.Code);
            else if (SortOrder == Html.GetSortDsc("FundsAccountingTypePickListDetailId")) query = query.OrderByDescending(q => q.FundsAccountingTypePickListDetail.Code);
            else if (SortOrder == Html.GetSortAsc("PersonInChargeId")) query = query.OrderBy(q => q.PersonInCharge.FullName);
            else if (SortOrder == Html.GetSortDsc("PersonInChargeId")) query = query.OrderByDescending(q => q.PersonInCharge.FullName);
            else if (SortOrder == Html.GetSortAsc("Status")) query = query.OrderBy(q => q.Status);
            else if (SortOrder == Html.GetSortDsc("Status")) query = query.OrderByDescending(q => q.Status);
            else query = query.OrderBy(q => q.SoaQuarter);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: PerLifeAggregation/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            var model = new PerLifeAggregationViewModel();
            LoadPage(model);
            return View(model);
        }

        // POST: PerLifeAggregation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, PerLifeAggregationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);

                var trail = GetNewTrailObject();
                Result = PerLifeAggregationService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;

                    CreateTrail(
                        bo.Id,
                        "Create Per Life Aggregation"
                    );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage(model);
            return View(model);
        }

        // GET: PerLifeAggregation/Edit/5
        public ActionResult Edit(
            int id,
            string SortOrder,
            int? Page,
            string SelectedIds
        )
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = PerLifeAggregationService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            // For Detail Ids
            ViewBag.SelectedIds = SelectedIds;

            // Listing Matched Record
            ListDetail(
                id,
                Page,
                SortOrder
            );

            ListEmptyExcludedSummary();

            ListEmptyExcludedDetail();

            ListEmptyRetroSummary();

            ListEmptyRetroDetail();

            var model = new PerLifeAggregationViewModel(bo)
            {
                ActiveTab = PerLifeAggregationBo.ActiveTabRetroProcessing
            };
            LoadPage(model);
            return View(model);
        }

        // GET: PerLifeAggregation/ExcludedRecord/5
        public ActionResult ExcludedRecord(
            int id,
            string ExTreatyCode,
            string ExPolicyNumber,
            string ExPolicyStatusCode,
            string ExAar,
            string ExNetPremium,
            string ExPremiumFrequencyCode,
            int? ExRiskPeriodMonth,
            int? ExRiskPeriodYear,
            string ExLastUpdatedDate,
            string ExRiskPeriodStartDate,
            string ExRiskPeriodEndDate,
            string ExMlreBenefitCode,
            int? ExExceptionType,
            //string SortOrder,
            int? SummaryPage,
            int? Page
        )
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = PerLifeAggregationService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            ListEmptyDetail();

            // Listing Excluded Summary
            ListExcludedSummary(
                id,
                SummaryPage
            );

            // Listing Excluded Record
            ListExcludedDetail(
                id,
                Page,
                ExTreatyCode,
                ExPolicyNumber,
                ExPolicyStatusCode,
                ExAar,
                ExNetPremium,
                ExPremiumFrequencyCode,
                ExRiskPeriodMonth,
                ExRiskPeriodYear,
                ExLastUpdatedDate,
                ExRiskPeriodStartDate,
                ExRiskPeriodEndDate,
                ExMlreBenefitCode,
                ExExceptionType
            //SortOrder
            );

            ListEmptyRetroSummary();

            ListEmptyRetroDetail();

            var model = new PerLifeAggregationViewModel(bo)
            {
                ActiveTab = PerLifeAggregationBo.ActiveTabExcludedRecord
            };
            LoadPage(model);
            return View("Edit", model);
        }

        // GET: PerLifeAggregation/RetroRecord/5
        public ActionResult RetroRecord(
            int id,
            string ReTreatyCode,
            string ReReinsBasisCode,
            string ReFundsAccountingTypeCode,
            string RePremiumFrequencyCode,
            int? ReReportPeriodMonth,
            int? ReReportPeriodYear,
            string ReTransactionTypeCode,
            string RePolicyNumber,
            string ReIssueDatePol,
            string ReIssueDateBen,
            string ReReinsEffDatePol,
            string ReReinsEffDateBen,
            string ReCedingPlanCode,
            string ReCedingBenefitTypeCode,
            string ReCedingBenefitRiskCode,
            string ReMlreBenefitCode,
            int? SummaryPage,
            int? Page
        )
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = PerLifeAggregationService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            ListEmptyDetail();

            ListEmptyExcludedSummary();

            ListEmptyExcludedDetail();

            // Listing Retro Summary
            ListRetroSummary(
                id,
                SummaryPage
            );

            // Listing Retro Record
            ListRetroDetail(
                id,
                Page,
                ReTreatyCode,
                ReReinsBasisCode,
                ReFundsAccountingTypeCode,
                RePremiumFrequencyCode,
                ReReportPeriodMonth,
                ReReportPeriodYear,
                ReTransactionTypeCode,
                RePolicyNumber,
                ReIssueDatePol,
                ReIssueDateBen,
                ReReinsEffDatePol,
                ReReinsEffDateBen,
                ReCedingPlanCode,
                ReCedingBenefitTypeCode,
                ReCedingBenefitRiskCode,
                ReMlreBenefitCode
            );

            var model = new PerLifeAggregationViewModel(bo)
            {
                ActiveTab = PerLifeAggregationBo.ActiveTabRetroRecord
            };
            LoadPage(model);
            return View("Edit", model);
        }

        // POST: PerLifeAggregation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, int? Page, FormCollection form, PerLifeAggregationViewModel model)
        {
            var dbBo = PerLifeAggregationService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                Result = PerLifeAggregationService.Result();

                List<int> detailStatuses = new List<int>
                {
                    PerLifeAggregationDetailBo.StatusSubmitForValidation,
                    PerLifeAggregationDetailBo.StatusValidating,
                    PerLifeAggregationDetailBo.StatusSubmitForProcessing,
                    PerLifeAggregationDetailBo.StatusProcessing,
                    PerLifeAggregationDetailBo.StatusSubmitForAggregation,
                    PerLifeAggregationDetailBo.StatusAggregating,
                };

                if (dbBo.Status != model.Status && model.Status == PerLifeAggregationBo.StatusSubmitForProcessing && PerLifeAggregationDetailService.IsStatusesPerLifeAggregationId(detailStatuses, model.Id)) {
                    model.Status = dbBo.Status;
                    Result.AddError("Unable Submit for Processing as there are Risk Quarter pending process / processing");
                }

                if (Result.Valid)
                {
                    var trail = GetNewTrailObject();

                    Result = PerLifeAggregationService.Update(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        model.ProcessStatusHistory(form, AuthUserId, ref trail);

                        CreateTrail(
                            bo.Id,
                            "Update Per Life Aggregation"
                        );

                        SetUpdateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = bo.Id });
                    }
                }
                AddResult(Result);
            }

            ListDetail(
                id,
                Page
            );

            ListEmptyExcludedDetail();

            ListEmptyExcludedSummary();

            ListEmptyRetroSummary();

            ListEmptyRetroDetail();

            model.ActiveTab = PerLifeAggregationBo.ActiveTabRetroProcessing;
            LoadPage(model);
            return View(model);
        }

        public void ListDetail(
            int id,
            int? Page,
            string SortOrder = null
        )
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["SortOrder"] = SortOrder,
            };

            ViewBag.SortOrder = SortOrder;
            ViewBag.SortRiskQuarter = GetSortParam("RiskQuarter");
            ViewBag.SortStatus = GetSortParam("Status");
            ViewBag.SortProcessingDate = GetSortParam("ProcessingDate");

            _db.Database.CommandTimeout = 0;

            var query = _db.PerLifeAggregationDetails.AsNoTracking().Where(q => q.PerLifeAggregationId == id).Select(PerLifeAggregationDetailViewModel.Expression());

            if (SortOrder == Html.GetSortAsc("RiskQuarter")) query = query.OrderBy(q => q.RiskQuarter);
            else if (SortOrder == Html.GetSortDsc("RiskQuarter")) query = query.OrderByDescending(q => q.RiskQuarter);
            else if (SortOrder == Html.GetSortAsc("Status")) query = query.OrderBy(q => q.Status);
            else if (SortOrder == Html.GetSortDsc("Status")) query = query.OrderByDescending(q => q.Status);
            else if (SortOrder == Html.GetSortAsc("ProcessingDate")) query = query.OrderBy(q => q.ProcessingDate);
            else if (SortOrder == Html.GetSortDsc("ProcessingDate")) query = query.OrderByDescending(q => q.ProcessingDate);
            else query = query.OrderBy(q => q.RiskQuarter);

            ViewBag.DetailTotal = query.Count();
            ViewBag.DetailList = query.ToPagedList(Page ?? 1, PageSize);
        }

        public void ListEmptyDetail()
        {
            ViewBag.RouteValue = new RouteValueDictionary { };
            ViewBag.DetailTotal = 0;
            ViewBag.DetailList = new List<PerLifeAggregationDetailViewModel>().ToPagedList(1, PageSize);
        }

        public void ListExcludedSummary(
            int id,
            int? Page
        )
        {
            _db.Database.CommandTimeout = 0;

            var query = _db.PerLifeAggregationDetailData
                .Where(q => q.PerLifeAggregationDetailTreaty.PerLifeAggregationDetail.PerLifeAggregationId == id)
                .Where(q => q.ProceedStatus != PerLifeAggregationDetailDataBo.ProceedStatusProceed)
                .GroupBy(
                    q => q.PerLifeAggregationDetailTreatyId,
                    (key, DetailData) => new PerLifeAggregationDetailTreatyViewModel
                    {
                        RiskQuarter = DetailData.Select(q => q.PerLifeAggregationDetailTreaty.PerLifeAggregationDetail.RiskQuarter).FirstOrDefault(),
                        TreatyCode = DetailData.Select(q => q.PerLifeAggregationDetailTreaty.TreatyCode).FirstOrDefault(),
                        Count = DetailData.Count(),
                        TotalAar = DetailData.Sum(q => q.RiDataWarehouseHistory.Aar) ?? 0,
                        TotalGrossPremium = DetailData.Sum(q => q.RiDataWarehouseHistory.GrossPremium) ?? 0,
                        TotalNetPremium = DetailData.Sum(q => q.RiDataWarehouseHistory.NetPremium) ?? 0,
                    }
                );

            query = query.OrderByDescending(q => q.RiskQuarter).ThenBy(q => q.TreatyCode);

            ViewBag.ExcludedSummaryTotal = query.Count();
            ViewBag.ExcludedSummaryList = query.ToPagedList(Page ?? 1, SummaryPageSize);
        }

        public void ListEmptyExcludedSummary()
        {
            ViewBag.ExcludedSummaryTotal = 0;
            ViewBag.ExcludedSummaryList = new List<PerLifeAggregationDetailTreatyViewModel>().ToPagedList(1, SummaryPageSize);
        }

        public void ListExcludedDetail(
            int id,
            int? Page,
            string ExTreatyCode = null,
            string ExPolicyNumber = null,
            string ExPolicyStatusCode = null,
            string ExAar = null,
            string ExNetPremium = null,
            string ExPremiumFrequencyCode = null,
            int? ExRiskPeriodMonth = null,
            int? ExRiskPeriodYear = null,
            string ExLastUpdatedDate = null,
            string ExRiskPeriodStartDate = null,
            string ExRiskPeriodEndDate = null,
            string ExMlreBenefitCode = null,
            int? ExExceptionType = null,
            string SortOrder = null
        )
        {
            DateTime? exLastUpdatedDate = Util.GetParseDateTime(ExLastUpdatedDate);
            DateTime? exRiskPeriodStartDate = Util.GetParseDateTime(ExRiskPeriodStartDate);
            DateTime? exRiskPeriodEndDate = Util.GetParseDateTime(ExRiskPeriodEndDate);

            double? exAar = Util.StringToDouble(ExAar);
            double? exNetPremium = Util.StringToDouble(ExNetPremium);

            ViewBag.ExcludedDetailRouteValue = new RouteValueDictionary
            {
                ["ExTreatyCode"] = ExTreatyCode,
                ["ExPolicyNumber"] = ExPolicyNumber,
                ["ExPolicyStatusCode"] = ExPolicyStatusCode,
                ["ExAar"] = exAar.HasValue ? ExAar : null,
                ["ExNetPremium"] = exNetPremium.HasValue ? ExNetPremium : null,
                ["ExPremiumFrequencyCode"] = ExPremiumFrequencyCode,
                ["ExRiskPeriodMonth"] = ExRiskPeriodMonth,
                ["ExRiskPeriodYear"] = ExRiskPeriodYear,
                ["ExLastUpdatedDate"] = exLastUpdatedDate.HasValue ? ExLastUpdatedDate : null,
                ["ExRiskPeriodStartDate"] = exRiskPeriodStartDate.HasValue ? ExRiskPeriodStartDate : null,
                ["ExRiskPeriodEndDate"] = exRiskPeriodEndDate.HasValue ? ExRiskPeriodEndDate : null,
                ["ExMlreBenefitCode"] = ExMlreBenefitCode,
                ["ExExceptionType"] = ExExceptionType,
                ["SortOrder"] = SortOrder,
            };

            ViewBag.SortOrder = SortOrder;
            ViewBag.SortExTreatyCode = GetSortParam("ExTreatyCode");
            ViewBag.SortExPolicyNumber = GetSortParam("ExPolicyNumber");
            ViewBag.SortExPolicyStatusCode = GetSortParam("ExPolicyStatusCode");
            ViewBag.SortExAar = GetSortParam("ExAar");
            ViewBag.SortExNetPremium = GetSortParam("ExNetPremium");
            ViewBag.SortExPremiumFrequencyCode = GetSortParam("ExPremiumFrequencyCode");
            ViewBag.SortExRiskPeriodMonth = GetSortParam("ExRiskPeriodMonth");
            ViewBag.SortExRiskPeriodYear = GetSortParam("ExRiskPeriodYear");
            ViewBag.SortExLastUpdatedDate = GetSortParam("ExLastUpdatedDate");
            ViewBag.SortExRiskPeriodStartDate = GetSortParam("ExRiskPeriodStartDate");
            ViewBag.SortExRiskPeriodEndDate = GetSortParam("ExRiskPeriodEndDate");
            ViewBag.SortExMlreBenefitCode = GetSortParam("ExMlreBenefitCode");
            ViewBag.SortExExceptionType = GetSortParam("ExExceptionType");

            _db.Database.CommandTimeout = 0;

            var query = _db.PerLifeAggregationDetailData.AsNoTracking()
                .Where(q => q.PerLifeAggregationDetailTreaty.PerLifeAggregationDetail.PerLifeAggregationId == id)
                .Where(q => q.ProceedStatus != PerLifeAggregationDetailDataBo.ProceedStatusProceed)
                .Select(PerLifeAggregationDetailDataViewModel.Expression());

            if (!string.IsNullOrEmpty(ExTreatyCode)) query = query.Where(q => q.TreatyCode == ExTreatyCode);
            if (!string.IsNullOrEmpty(ExPolicyNumber)) query = query.Where(q => q.PolicyNumber == ExPolicyNumber);
            if (!string.IsNullOrEmpty(ExPolicyStatusCode)) query = query.Where(q => q.PolicyStatusCode == ExPolicyStatusCode);
            if (exAar.HasValue) query = query.Where(q => q.Aar == exAar);
            if (exNetPremium.HasValue) query = query.Where(q => q.NetPremium == exNetPremium);
            if (!string.IsNullOrEmpty(ExPremiumFrequencyCode)) query = query.Where(q => q.PremiumFrequencyCode == ExPremiumFrequencyCode);
            if (ExRiskPeriodMonth.HasValue) query = query.Where(q => q.RiskPeriodMonth == ExRiskPeriodMonth);
            if (ExRiskPeriodYear.HasValue) query = query.Where(q => q.RiskPeriodYear == ExRiskPeriodYear);
            if (exLastUpdatedDate.HasValue) query = query.Where(q => q.LastUpdatedDate == exLastUpdatedDate);
            if (exRiskPeriodStartDate.HasValue) query = query.Where(q => q.RiskPeriodStartDate == exRiskPeriodStartDate);
            if (exRiskPeriodEndDate.HasValue) query = query.Where(q => q.RiskPeriodEndDate == exRiskPeriodEndDate);
            if (!string.IsNullOrEmpty(ExMlreBenefitCode)) query = query.Where(q => q.MlreBenefitCode == ExMlreBenefitCode);
            if (ExExceptionType.HasValue) query = query.Where(q => q.ExceptionType == ExExceptionType);

            if (SortOrder == Html.GetSortAsc("ExTreatyCode")) query = query.OrderBy(q => q.TreatyCode);
            else if (SortOrder == Html.GetSortDsc("ExTreatyCode")) query = query.OrderByDescending(q => q.TreatyCode);
            else if (SortOrder == Html.GetSortAsc("ExPolicyNumber")) query = query.OrderBy(q => q.PolicyNumber);
            else if (SortOrder == Html.GetSortDsc("ExPolicyNumber")) query = query.OrderByDescending(q => q.PolicyNumber);
            else if (SortOrder == Html.GetSortAsc("ExPolicyStatusCode")) query = query.OrderBy(q => q.PolicyStatusCode);
            else if (SortOrder == Html.GetSortDsc("ExPolicyStatusCode")) query = query.OrderByDescending(q => q.PolicyStatusCode);
            else if (SortOrder == Html.GetSortAsc("ExAar")) query = query.OrderBy(q => q.Aar);
            else if (SortOrder == Html.GetSortDsc("ExAar")) query = query.OrderByDescending(q => q.Aar);
            else if (SortOrder == Html.GetSortAsc("ExNetPremium")) query = query.OrderBy(q => q.NetPremium);
            else if (SortOrder == Html.GetSortDsc("ExNetPremium")) query = query.OrderByDescending(q => q.NetPremium);
            else if (SortOrder == Html.GetSortAsc("ExPremiumFrequencyCode")) query = query.OrderBy(q => q.PremiumFrequencyCode);
            else if (SortOrder == Html.GetSortDsc("ExPremiumFrequencyCode")) query = query.OrderByDescending(q => q.PremiumFrequencyCode);
            else if (SortOrder == Html.GetSortAsc("ExRiskPeriodMonth")) query = query.OrderBy(q => q.RiskPeriodMonth);
            else if (SortOrder == Html.GetSortDsc("ExRiskPeriodMonth")) query = query.OrderByDescending(q => q.RiskPeriodMonth);
            else if (SortOrder == Html.GetSortAsc("ExRiskPeriodYear")) query = query.OrderBy(q => q.RiskPeriodYear);
            else if (SortOrder == Html.GetSortDsc("ExRiskPeriodYear")) query = query.OrderByDescending(q => q.RiskPeriodYear);
            else if (SortOrder == Html.GetSortAsc("ExLastUpdatedDate")) query = query.OrderBy(q => q.LastUpdatedDate);
            else if (SortOrder == Html.GetSortDsc("ExLastUpdatedDate")) query = query.OrderByDescending(q => q.LastUpdatedDate);
            else if (SortOrder == Html.GetSortAsc("ExRiskPeriodStartDate")) query = query.OrderBy(q => q.RiskPeriodStartDate);
            else if (SortOrder == Html.GetSortDsc("ExRiskPeriodStartDate")) query = query.OrderByDescending(q => q.RiskPeriodStartDate);
            else if (SortOrder == Html.GetSortAsc("ExRiskPeriodEndDate")) query = query.OrderBy(q => q.RiskPeriodEndDate);
            else if (SortOrder == Html.GetSortDsc("ExRiskPeriodEndDate")) query = query.OrderByDescending(q => q.RiskPeriodEndDate);
            else if (SortOrder == Html.GetSortAsc("ExMlreBenefitCode")) query = query.OrderBy(q => q.MlreBenefitCode);
            else if (SortOrder == Html.GetSortDsc("ExMlreBenefitCode")) query = query.OrderByDescending(q => q.MlreBenefitCode);
            else if (SortOrder == Html.GetSortAsc("ExExceptionType")) query = query.OrderBy(q => q.ExceptionType);
            else if (SortOrder == Html.GetSortDsc("ExExceptionType")) query = query.OrderByDescending(q => q.ExceptionType);
            else query = query.OrderBy(q => q.TreatyCode);

            ViewBag.ExcludedDetailTotal = query.Count();
            ViewBag.ExcludedDetailList = query.ToPagedList(Page ?? 1, PageSize);
        }

        public void ListEmptyExcludedDetail()
        {
            ViewBag.ExcludedDetailRouteValue = new RouteValueDictionary { };
            ViewBag.ExcludedDetailTotal = 0;
            ViewBag.ExcludedDetailList = new List<PerLifeAggregationDetailDataViewModel>().ToPagedList(1, PageSize);
        }

        public void ListRetroSummary(
            int id,
            int? Page
        )
        {
            _db.Database.CommandTimeout = 0;

            var query = _db.PerLifeAggregationMonthlyData
                .Where(q => q.PerLifeAggregationDetailData.PerLifeAggregationDetailTreaty.PerLifeAggregationDetail.PerLifeAggregationId == id)
                .GroupBy(
                    q => q.PerLifeAggregationDetailData.PerLifeAggregationDetailTreatyId,
                    (key, MonthlyData) => new PerLifeAggregationDetailTreatyViewModel
                    {
                        RiskQuarter = MonthlyData.Select(q => q.PerLifeAggregationDetailData.PerLifeAggregationDetailTreaty.PerLifeAggregationDetail.RiskQuarter).FirstOrDefault(),
                        TreatyCode = MonthlyData.Select(q => q.PerLifeAggregationDetailData.PerLifeAggregationDetailTreaty.TreatyCode).FirstOrDefault(),
                        Count = MonthlyData.Count(),
                        TotalAar = MonthlyData.Sum(q => q.Aar),
                        TotalGrossPremium = MonthlyData.Sum(q => q.RetroGrossPremium) ?? 0,
                        TotalNetPremium = MonthlyData.Sum(q => q.NetPremium),
                        Count2 = MonthlyData.Where(q => q.RetroIndicator == false).Count(),
                        TotalAar2 = MonthlyData.Where(q => q.RetroIndicator == false).Sum(q => q.Aar),
                        TotalGrossPremium2 = MonthlyData.Where(q => q.RetroIndicator == false).Sum(q => q.RetroGrossPremium) ?? 0,
                        TotalNetPremium2 = MonthlyData.Where(q => q.RetroIndicator == false).Sum(q => q.NetPremium),
                        Count3 = MonthlyData.Where(q => q.RetroIndicator == true).Count(),
                        TotalRetroAmount3 = MonthlyData.Where(q => q.RetroIndicator == true).Sum(q => q.RetroAmount) ?? 0,
                        TotalGrossPremium3 = MonthlyData.Where(q => q.RetroIndicator == true).Sum(q => q.PerLifeAggregationMonthlyRetroData.Sum(r => r.RetroGrossPremium)) ?? 0,
                        TotalNetPremium3 = MonthlyData.Where(q => q.RetroIndicator == true).Sum(q => q.PerLifeAggregationMonthlyRetroData.Sum(r => r.RetroNetPremium)) ?? 0,
                        TotalDiscount3 = MonthlyData.Where(q => q.RetroIndicator == true).Sum(q => q.PerLifeAggregationMonthlyRetroData.Sum(r => r.RetroDiscount)) ?? 0,
                    }
                );

            query = query.OrderByDescending(q => q.RiskQuarter).ThenBy(q => q.TreatyCode);

            ViewBag.RetroSummaryTotal = query.Count();
            ViewBag.RetroSummaryList = query.ToPagedList(Page ?? 1, SummaryPageSize);
        }

        public void ListEmptyRetroSummary()
        {
            ViewBag.RetroSummaryTotal = 0;
            ViewBag.RetroSummaryList = new List<PerLifeAggregationDetailTreatyViewModel>().ToPagedList(1, SummaryPageSize);
        }

        public void ListRetroDetail(
            int id,
            int? Page,
            string ReTreatyCode = null,
            string ReReinsBasisCode = null,
            string ReFundsAccountingTypeCode = null,
            string RePremiumFrequencyCode = null,
            int? ReReportPeriodMonth = null,
            int? ReReportPeriodYear = null,
            string ReTransactionTypeCode = null,
            string RePolicyNumber = null,
            string ReIssueDatePol = null,
            string ReIssueDateBen = null,
            string ReReinsEffDatePol = null,
            string ReReinsEffDateBen = null,
            string ReCedingPlanCode = null,
            string ReCedingBenefitTypeCode = null,
            string ReCedingBenefitRiskCode = null,
            string ReMlreBenefitCode = null
        )
        {
            DateTime? reIssueDatePol = Util.GetParseDateTime(ReIssueDatePol);
            DateTime? reIssueDateBen = Util.GetParseDateTime(ReIssueDateBen);
            DateTime? reReinsEffDatePol = Util.GetParseDateTime(ReReinsEffDatePol);
            DateTime? reReinsEffDateBen = Util.GetParseDateTime(ReReinsEffDateBen);

            ViewBag.RetroDetailRouteValue = new RouteValueDictionary
            {
                ["ReTreatyCode"] = ReTreatyCode,
                ["ReReinsBasisCode"] = ReReinsBasisCode,
                ["ReFundsAccountingTypeCode"] = ReFundsAccountingTypeCode,
                ["RePremiumFrequencyCode"] = RePremiumFrequencyCode,
                ["ReReportPeriodMonth"] = ReReportPeriodMonth,
                ["ReReportPeriodYear"] = ReReportPeriodYear,
                ["ReTransactionTypeCode"] = ReTransactionTypeCode,
                ["RePolicyNumber"] = RePolicyNumber,
                ["ReIssueDatePol"] = reIssueDatePol.HasValue ? ReIssueDatePol : null,
                ["ReIssueDateBen"] = reIssueDateBen.HasValue ? ReIssueDateBen : null,
                ["ReReinsEffDatePol"] = reReinsEffDatePol.HasValue ? ReReinsEffDatePol : null,
                ["ReReinsEffDateBen"] = reReinsEffDateBen.HasValue ? ReReinsEffDateBen : null,
                ["ReCedingPlanCode"] = ReCedingPlanCode,
                ["ReCedingBenefitTypeCode"] = ReCedingBenefitTypeCode,
                ["ReCedingBenefitRiskCode"] = ReCedingBenefitRiskCode,
                ["ReMlreBenefitCode"] = ReMlreBenefitCode,
            };

            _db.Database.CommandTimeout = 0;

            var query = _db.PerLifeAggregationMonthlyData.AsNoTracking()
                .Where(q => q.PerLifeAggregationDetailData.PerLifeAggregationDetailTreaty.PerLifeAggregationDetail.PerLifeAggregationId == id)
                .Select(PerLifeAggregationMonthlyDataViewModel.Expression());

            if (!string.IsNullOrEmpty(ReTreatyCode)) query = query.Where(q => q.TreatyCode == ReTreatyCode);
            if (!string.IsNullOrEmpty(ReReinsBasisCode)) query = query.Where(q => q.ReinsBasisCode == ReReinsBasisCode);
            if (!string.IsNullOrEmpty(ReFundsAccountingTypeCode)) query = query.Where(q => q.FundsAccountingTypeCode == ReFundsAccountingTypeCode);
            if (!string.IsNullOrEmpty(RePremiumFrequencyCode)) query = query.Where(q => q.PremiumFrequencyCode == RePremiumFrequencyCode);
            if (ReReportPeriodMonth.HasValue) query = query.Where(q => q.ReportPeriodMonth == ReReportPeriodMonth);
            if (ReReportPeriodYear.HasValue) query = query.Where(q => q.ReportPeriodYear == ReReportPeriodYear);
            if (!string.IsNullOrEmpty(ReTransactionTypeCode)) query = query.Where(q => q.TransactionTypeCode == ReTransactionTypeCode);
            if (!string.IsNullOrEmpty(RePolicyNumber)) query = query.Where(q => q.PolicyNumber == RePolicyNumber);
            if (reIssueDatePol.HasValue) query = query.Where(q => q.IssueDatePol == reIssueDatePol);
            if (reIssueDateBen.HasValue) query = query.Where(q => q.IssueDateBen == reIssueDateBen);
            if (reReinsEffDatePol.HasValue) query = query.Where(q => q.ReinsEffDatePol == reReinsEffDatePol);
            if (reReinsEffDateBen.HasValue) query = query.Where(q => q.ReinsEffDateBen == reReinsEffDateBen);
            if (!string.IsNullOrEmpty(ReCedingPlanCode)) query = query.Where(q => q.CedingPlanCode == ReCedingPlanCode);
            if (!string.IsNullOrEmpty(ReCedingBenefitTypeCode)) query = query.Where(q => q.CedingBenefitTypeCode == ReCedingBenefitTypeCode);
            if (!string.IsNullOrEmpty(ReCedingBenefitRiskCode)) query = query.Where(q => q.CedingBenefitRiskCode == ReCedingBenefitRiskCode);
            if (!string.IsNullOrEmpty(ReMlreBenefitCode)) query = query.Where(q => q.MlreBenefitCode == ReMlreBenefitCode);

            query = query.OrderBy(q => q.TreatyCode);

            ViewBag.RetroDetailTotal = query.Count();
            ViewBag.RetroDetailList = query.ToPagedList(Page ?? 1, PageSize);
        }

        public void ListEmptyRetroDetail()
        {
            ViewBag.RetroDetailRouteValue = new RouteValueDictionary { };
            ViewBag.RetroDetailTotal = 0;
            ViewBag.RetroDetailList = new List<PerLifeAggregationMonthlyDataViewModel>().ToPagedList(1, PageSize);
        }

        public ActionResult DownloadExcludedRecord(
            string downloadToken,
            int type,
            int id,
            string ExTreatyCode = null,
            string ExPolicyNumber = null,
            string ExPolicyStatusCode = null,
            string ExAar = null,
            string ExNetPremium = null,
            string ExPremiumFrequencyCode = null,
            int? ExRiskPeriodMonth = null,
            int? ExRiskPeriodYear = null,
            string ExLastUpdatedDate = null,
            string ExRiskPeriodStartDate = null,
            string ExRiskPeriodEndDate = null,
            string ExMlreBenefitCode = null,
            int? ExExceptionType = null
        )
        {
            // type 1 = all
            // type 2 = filtered download

            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            _db.Database.CommandTimeout = 0;

            dynamic Params = new ExpandoObject();
            Params.TreatyCode = ExTreatyCode;
            Params.PolicyNumber = ExPolicyNumber;
            Params.PolicyStatusCode = ExPolicyStatusCode;
            Params.Aar = ExAar;
            Params.NetPremium = ExNetPremium;
            Params.PremiumFrequencyCode = ExPremiumFrequencyCode;
            Params.RiskPeriodMonth = ExRiskPeriodMonth;
            Params.RiskPeriodYear = ExRiskPeriodYear;
            Params.LastUpdatedDate = ExLastUpdatedDate;
            Params.RiskPeriodStartDate = ExRiskPeriodStartDate;
            Params.RiskPeriodEndDate = ExRiskPeriodEndDate;
            Params.MlreBenefitCode = ExMlreBenefitCode;
            Params.ExceptionType = ExExceptionType?.ToString();
            var export = new GenerateExportData();
            ExportBo exportBo = export.CreateExportPerLifeAggregationRetroSummaryExcludedRecord(id, AuthUserId, Params);

            if (exportBo.Total <= export.ExportInstantDownloadLimit)
            {
                export.Process(ref exportBo, _db);
                Session.Add("DownloadExport", true);
            }

            return RedirectToAction("Edit", "Export", new { exportBo.Id });
        }

        public ActionResult DownloadRetroRecord(
            string downloadToken,
            int type,
            int id,
            string ReTreatyCode = null,
            string ReReinsBasisCode = null,
            string ReFundsAccountingTypeCode = null,
            string RePremiumFrequencyCode = null,
            int? ReReportPeriodMonth = null,
            int? ReReportPeriodYear = null,
            string ReTransactionTypeCode = null,
            string RePolicyNumber = null,
            string ReIssueDatePol = null,
            string ReIssueDateBen = null,
            string ReReinsEffDatePol = null,
            string ReReinsEffDateBen = null,
            string ReCedingPlanCode = null,
            string ReCedingBenefitTypeCode = null,
            string ReCedingBenefitRiskCode = null,
            string ReMlreBenefitCode = null
        )
        {
            // type 1 = all
            // type 2 = filtered download

            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            _db.Database.CommandTimeout = 0;

            dynamic Params = new ExpandoObject();
            Params.TreatyCode = ReTreatyCode;
            Params.ReinsBasisCode = ReReinsBasisCode;
            Params.FundsAccountingTypeCode = ReFundsAccountingTypeCode;
            Params.PremiumFrequencyCode = RePremiumFrequencyCode;
            Params.ReportPeriodMonth = ReReportPeriodMonth;
            Params.ReportPeriodYear = ReReportPeriodYear;
            Params.TransactionTypeCode = ReTransactionTypeCode;
            Params.PolicyNumber = RePolicyNumber;
            Params.IssueDatePol = ReIssueDatePol;
            Params.IssueDateBen = ReIssueDateBen;
            Params.ReinsEffDatePol = ReReinsEffDatePol;
            Params.ReinsEffDateBen = ReReinsEffDateBen;
            Params.CedingPlanCode = ReCedingPlanCode;
            Params.CedingBenefitTypeCode = ReCedingBenefitTypeCode;
            Params.CedingBenefitRiskCode = ReCedingBenefitRiskCode;
            Params.MlreBenefitCode = ReMlreBenefitCode;
            var export = new GenerateExportData();
            ExportBo exportBo = export.CreateExportPerLifeAggregationRetroSummaryRetro(id, AuthUserId, Params);

            if (exportBo.Total <= export.ExportInstantDownloadLimit)
            {
                export.Process(ref exportBo, _db);
                Session.Add("DownloadExport", true);
            }

            return RedirectToAction("Edit", "Export", new { exportBo.Id });
        }

        // GET: PerLifeAggregation/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = PerLifeAggregationService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            return View(new PerLifeAggregationViewModel(bo));
        }

        // POST: PerLifeAggregation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, PerLifeAggregationViewModel model)
        {
            var bo = PerLifeAggregationService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            Result = PerLifeAggregationService.Result();
            if(!ValidateDelete(model))
                Result.AddError("Unable to delete due to data still processing");

            if (PerLifeAggregationService.IsDataInUse(model.Id))
                Result.AddError(MessageBag.UnableDeletePerLifeAggregation);

            if (Result.Valid)
            {
                bo.Status = PerLifeAggregationBo.StatusPendingDelete;
                bo.UpdatedById = AuthUserId;

                var trail = GetNewTrailObject();
                Result = PerLifeAggregationService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Update Per Life Aggregation"
                    );

                    SetDeleteSuccessMessage(Controller);
                    return RedirectToAction("Index");
                }
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = bo.Id });
        }

        public void IndexPage()
        {
            var departmentId = Util.GetConfigInteger("PerLifeAggregationPicDepartment", DepartmentBo.DepartmentRetrocession);

            DropDownStatus();
            DropDownFundsAccountingTypeCode();
            DropDownCutOff();
            DropDownUser(departmentId: departmentId);
            SetViewBagMessage();
        }

        public void LoadPage(PerLifeAggregationViewModel model)
        {
            var departmentId = Util.GetConfigInteger("PerLifeAggregationPicDepartment", DepartmentBo.DepartmentRetrocession);
            DropDownCutOff();

            if (model.Id == 0)
            {
                DropDownFundsAccountingTypeCode();
                DropDownUser(UserBo.StatusActive, null, false, departmentId);
            }
            else
            {
                // Header
                ViewBag.DropDownParentFundsAccountingTypeCodes = DropDownFundsAccountingTypeCode();
                DropDownUser(UserBo.StatusActive, model.PersonInChargeId, false, departmentId: departmentId);

                IsEnableSubmitForProcessing(model);
                IsEnableFinalise(model);
                IsEnableDelete(model);
                IsEnableProcessRiskQuarter(model);

                DropDownTreatyCode(codeAsValue: true);
                DropDownPolicyStatusCode(codeAsValue: true);
                DropDownPremiumFrequencyCode(codeAsValue: true);
                DropDownMonth();
                DropDownReinsBasisCode(codeAsValue: true);
                DropDownFundsAccountingTypeCode(codeAsValue: true);
                DropDownTransactionTypeCode(codeAsValue: true);
                DropDownCedingBenefitTypeCode(codeAsValue: true);
                DropDownBenefit(codeAsValue: true);
                DropDownExceptionType();

                GetStatusHistoryList(model);
            }

            SetViewBagMessage();
        }

        public void IsEnableSubmitForProcessing(PerLifeAggregationViewModel model)
        {
            var isEnableSubmitForProcessing = false;
            List<int> detailStatuses = new List<int>
            {
                PerLifeAggregationDetailBo.StatusSubmitForValidation,
                PerLifeAggregationDetailBo.StatusValidating,
                PerLifeAggregationDetailBo.StatusSubmitForProcessing,
                PerLifeAggregationDetailBo.StatusProcessing,
                PerLifeAggregationDetailBo.StatusSubmitForAggregation,
                PerLifeAggregationDetailBo.StatusAggregating,
            };

            if ((model.Status == PerLifeAggregationBo.StatusPending ||
                model.Status == PerLifeAggregationBo.StatusPendingRiskQuarterProcessing ||
                model.Status == PerLifeAggregationBo.StatusSuccess ||
                model.Status == PerLifeAggregationBo.StatusFailed) &&
                !PerLifeAggregationDetailService.IsStatusesPerLifeAggregationId(detailStatuses, model.Id))
                isEnableSubmitForProcessing = true;

            ViewBag.IsEnableSubmitForProcessing = isEnableSubmitForProcessing;
        }

        public void IsEnableFinalise(PerLifeAggregationViewModel model)
        {
            var isEnableFinalise = false;
            var isChildNotFinalised = PerLifeAggregationDetailService.IsNotStatusByPerLifeAggregationId(PerLifeAggregationDetailBo.StatusFinalised, model.Id);
            if (model.Status == PerLifeAggregationBo.StatusSuccess && !isChildNotFinalised)
                isEnableFinalise = true;

            ViewBag.IsEnableFinalise = isEnableFinalise;
        }

        public void IsEnableProcessRiskQuarter(PerLifeAggregationViewModel model)
        {
            var isEnableProcessRiskQuarter = false;
            if ((model.ActiveTab == null || model.ActiveTab == PerLifeAggregationBo.ActiveTabRetroProcessing) &&
                model.Status != PerLifeAggregationBo.StatusPending &&
                model.Status != PerLifeAggregationBo.StatusSubmitForProcessing &&
                model.Status != PerLifeAggregationBo.StatusFinalised)
                isEnableProcessRiskQuarter = true;

            ViewBag.IsEnableProcessRiskQuarter = isEnableProcessRiskQuarter;
        }

        public void IsEnableDelete(PerLifeAggregationViewModel model)
        {
            var isEnableDelete = ValidateDelete(model);
            ViewBag.IsEnableDelete = isEnableDelete;
        }

        public bool ValidateDelete(PerLifeAggregationViewModel model)
        {
            var childProcessingStatuses = new List<int>() {
                PerLifeAggregationDetailBo.StatusSubmitForProcessing,
                PerLifeAggregationDetailBo.StatusProcessing,
                PerLifeAggregationDetailBo.StatusSubmitForValidation,
                PerLifeAggregationDetailBo.StatusValidating,
                PerLifeAggregationDetailBo.StatusSubmitForAggregation,
                PerLifeAggregationDetailBo.StatusAggregating
            };
            var isChildProcessing = PerLifeAggregationDetailService.IsStatusesPerLifeAggregationId(childProcessingStatuses, model.Id);
            if (model.Status != PerLifeAggregationBo.StatusSubmitForProcessing &&
                model.Status != PerLifeAggregationBo.StatusProcessing &&
                model.Status != PerLifeAggregationBo.StatusFinalised &&
                !isChildProcessing)
                return true;

            return false;
        }

        [Auth(Controller = Controller, Power = "U")]
        public ActionResult ProcessRiskQuarter(int id, PerLifeAggregationViewModel model, FormCollection form)
        {
            if (string.IsNullOrEmpty(model.SelectedIds))
            {
                SetErrorSessionMsg("No Risk Quarter Selected");
                return RedirectToAction("Edit", new { id });
            }

            int success = 0;
            int failed = 0;

            var ids = model.SelectedIds.Split(',').Select(q => int.Parse(q.Trim())).ToList();
            var total = ids.Count();

            var trail = GetNewTrailObject();

            foreach (int i in ids)
            {
                var bo = PerLifeAggregationDetailService.Find(i);
                if (bo == null || bo.PerLifeAggregationId != id)
                    continue;

                if (!bo.CanProcess())
                    continue;

                trail = GetNewTrailObject();

                bo.Status = PerLifeAggregationDetailBo.StatusSubmitForProcessing;
                Result = PerLifeAggregationDetailService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    success++;
                    CreateTrail(
                        bo.Id,
                        "Update Per Life Aggregation Detail Status"
                    );
                }
            }

            failed = total - success;

            SetSuccessSessionMsg(string.Format("Total Records Submitted for Processing: {0}", success));
            SetErrorSessionMsg(string.Format("Total Records not able to Submit for Processing: {0}", failed));
            return RedirectToAction("Edit", new { id });
        }

        [Auth(Controller = Controller, Power = "U")]
        public ActionResult FinaliseRiskQuarter(int id, PerLifeAggregationViewModel model, FormCollection form)
        {
            if (string.IsNullOrEmpty(model.SelectedIds))
            {
                SetErrorSessionMsg("No Risk Quarter Selected");
                return RedirectToAction("Edit", new { id });
            }

            int success = 0;
            int failed = 0;

            var ids = model.SelectedIds.Split(',').Select(q => int.Parse(q.Trim())).ToList();
            var total = ids.Count();

            var trail = GetNewTrailObject();

            foreach (int i in ids)
            {
                var bo = PerLifeAggregationDetailService.Find(i);
                if (bo == null || bo.PerLifeAggregationId != id)
                    continue;

                if (bo.Status != PerLifeAggregationDetailBo.StatusAggregationSuccess)
                    continue;

                trail = GetNewTrailObject();

                bo.Status = PerLifeAggregationDetailBo.StatusFinalised;
                Result = PerLifeAggregationDetailService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    success++;
                    CreateTrail(
                        bo.Id,
                        "Update Per Life Aggregation Detail Status"
                    );
                }
            }

            failed = total - success;

            SetSuccessSessionMsg(string.Format("Total Records Finalised: {0}", success));
            SetErrorSessionMsg(string.Format("Total Records not able to Finalise: {0}", failed));
            return RedirectToAction("Edit", new { id });
        }

        public void DropDownStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= PerLifeAggregationBo.StatusMax; i++)
            {
                items.Add(new SelectListItem { Text = PerLifeAggregationBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownStatuses = items;
        }

        public void DropDownExceptionType()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= PerLifeAggregationDetailDataBo.ExceptionTypeMax; i++)
            {
                items.Add(new SelectListItem { Text = PerLifeAggregationDetailDataBo.GetExceptionTypeName(i), Value = i.ToString() });
            }
            ViewBag.DropDownExceptionTypes = items;
        }

        public void GetStatusHistoryList(PerLifeAggregationViewModel model)
        {
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.PerLifeAggregation.ToString());
            // StatusHistories
            IList<StatusHistoryBo> statusHistoryBos = StatusHistoryService.GetStatusHistoriesByModuleIdObjectId(moduleBo.Id, model.Id);
            ViewBag.StatusHistories = statusHistoryBos;
        }
    }
}