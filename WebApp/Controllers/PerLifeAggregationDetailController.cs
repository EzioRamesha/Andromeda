using BusinessObject;
using BusinessObject.Retrocession;
using ConsoleApp.Commands.ProcessDatas;
using ConsoleApp.Commands.ProcessDatas.Exports;
using Ionic.Zip;
using PagedList;
using Services;
using Services.Retrocession;
using Shared;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
    public class PerLifeAggregationDetailController : BaseController
    {
        public const string Controller = "PerLifeAggregation";
        public const string SubController = "PerLifeAggregationDetail";

        // GET: PerLifeAggregationDetail
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //GET: PerLifeAggregationDetails/Edit/5
        public ActionResult Edit(
            int id,
            int? SearchRiskMonth,
            string SearchTreatyCode,
            string SearchInsuredName,
            string SearchInsuredDateOfBirth,
            string SearchPolicyNumber,
            string SearchMlreBenefitCode,
            string SearchTransactionTypeCode,
            // Filter RI Data
            string RiTreatyCode,
            string RiPolicyNumber,
            string RiReinsBasisCode,
            string RiInsuredName,
            string RiInsuredGenderCode,
            string RiInsuredDateOfBirth,
            int? RiReinsuranceIssueAge,
            string RiCedingPlanCode,
            string RiCedingBenefitTypeCode,
            string RiCedingBenefitRiskCode,
            string RiMlreBenefitCode,
            string RiAar,
            string RiGrossPremium,
            string RiTotalDiscount,
            string RiNetPremium,
            string RiBrokerageFee,
            string RiGroupPolicyNumber,
            string RiGroupPolicyName,
            string RiPolicyTerm,
            string RiPolicyExpiryDate,
            string RiCurrencyCode,
            string RiTerritoryOfIssueCode,
            string RiTransactionTypeCode,
            string RiIssueDatePol,
            string RiReinsEffDatePol,
            int? RiReportPeriodMonth,
            int? RiReportPeriodYear,
            int? RiRiskPeriodMonth,
            int? RiRiskPeriodYear,
            string RiRiskPeriodStartDate,
            string RiRiskPeriodEndDate,
            int? Page
        )
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = PerLifeAggregationDetailService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index", "PerLifeAggregation");
            }

            var model = new PerLifeAggregationDetailViewModel(bo);

            ListRiData(
                id,
                Page,
                SearchRiskMonth,
                SearchTreatyCode,
                SearchInsuredName,
                SearchInsuredDateOfBirth,
                SearchPolicyNumber,
                SearchMlreBenefitCode,
                SearchTransactionTypeCode,
                RiTreatyCode,
                RiPolicyNumber,
                RiReinsBasisCode,
                RiInsuredName,
                RiInsuredGenderCode,
                RiInsuredDateOfBirth,
                RiReinsuranceIssueAge,
                RiCedingPlanCode,
                RiCedingBenefitTypeCode,
                RiCedingBenefitRiskCode,
                RiMlreBenefitCode,
                RiAar,
                RiGrossPremium,
                RiTotalDiscount,
                RiNetPremium,
                RiBrokerageFee,
                RiGroupPolicyNumber,
                RiGroupPolicyName,
                RiPolicyTerm,
                RiPolicyExpiryDate,
                RiCurrencyCode,
                RiTerritoryOfIssueCode,
                RiTransactionTypeCode,
                RiIssueDatePol,
                RiReinsEffDatePol,
                RiReportPeriodMonth,
                RiReportPeriodYear,
                RiRiskPeriodMonth,
                RiRiskPeriodYear,
                RiRiskPeriodStartDate,
                RiRiskPeriodEndDate
            );

            ListEmptyException();

            ListEmptyRetroRiData();

            ListEmptyRetentionPremium();

            ListEmptyTreatySummary();

            model.ActiveTab = PerLifeAggregationDetailBo.ActiveTabRiData;
            LoadPage(model);
            return View(model);
        }

        //GET: PerLifeAggregationDetails/Exception/5
        public ActionResult Exception(
            int id,
            int? ExRecordType,
            string ExPolicyNumber,
            string ExIssueDatePol,
            string ExTreatyCode,
            string ExReinsEffDatePol,
            string ExInsuredName,
            string ExInsuredGenderCode,
            string ExInsuredDateOfBirth,
            string ExCedingPlanCode,
            string ExCedingBenefitTypeCode,
            string ExCedingBenefitRiskCode,
            string ExMlreBenefitCode,
            string ExAar,
            int? ExExceptionType,
            int? ExProceedStatus,
            int? Page,
            string SelectedExceptionIds
        )
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = PerLifeAggregationDetailService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index", "PerLifeAggregation");
            }

            // For Selected Exception Ids
            ViewBag.SelectedExceptionIds = SelectedExceptionIds;

            var model = new PerLifeAggregationDetailViewModel(bo);

            ListEmptyRiData();

            ListException(
                id,
                Page,
                ExRecordType,
                ExPolicyNumber,
                ExIssueDatePol,
                ExTreatyCode,
                ExReinsEffDatePol,
                ExInsuredName,
                ExInsuredGenderCode,
                ExInsuredDateOfBirth,
                ExCedingPlanCode,
                ExCedingBenefitTypeCode,
                ExCedingBenefitRiskCode,
                ExMlreBenefitCode,
                ExAar,
                ExExceptionType,
                ExProceedStatus
            );

            ListEmptyRetroRiData();

            ListEmptyRetentionPremium();

            ListEmptyTreatySummary();

            model.ActiveTab = PerLifeAggregationDetailBo.ActiveTabException;
            LoadPage(model);
            return View("Edit", model);
        }

        // Monthly Data
        //GET: PerLifeAggregationDetails/RetroRiData/5
        public ActionResult RetroRiData(
            int id,
            string ReTreatyCode,
            int? ReRiskYear,
            int? ReRiskMonth,
            string ReInsuredGenderCode,
            string ReInsuredDateOfBirth,
            string RePolicyNumber,
            string ReReinsEffDatePol,
            string ReAar,
            string ReGrossPremium,
            string ReNetPremium,
            string RePremiumFrequencyCode,
            string ReRetroPremFreq,
            string ReCedingPlanCode,
            string ReCedingBenefitTypeCode,
            string ReCedingBenefitRiskCode,
            string ReMlreBenefitCode,
            string ReRetroBenefitCode,
            int? Page
        )
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = PerLifeAggregationDetailService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index", "PerLifeAggregation");
            }

            var model = new PerLifeAggregationDetailViewModel(bo);

            ListEmptyRiData();

            ListEmptyException();

            ListRetroRiData(
                id,
                Page,
                ReTreatyCode,
                ReRiskYear,
                ReRiskMonth,
                ReInsuredGenderCode,
                ReInsuredDateOfBirth,
                RePolicyNumber,
                ReReinsEffDatePol,
                ReAar,
                ReGrossPremium,
                ReNetPremium,
                RePremiumFrequencyCode,
                ReRetroPremFreq,
                ReCedingPlanCode,
                ReCedingBenefitTypeCode,
                ReCedingBenefitRiskCode,
                ReMlreBenefitCode,
                ReRetroBenefitCode
            );

            ListEmptyRetentionPremium();

            ListEmptyTreatySummary();

            model.ActiveTab = PerLifeAggregationDetailBo.ActiveTabRetroRiData;
            LoadPage(model);
            return View("Edit", model);
        }

        //GET: PerLifeAggregationDetails/RetentionPremium/5
        public ActionResult RetentionPremium(
            int id,
            string RpUniqueKeyPerLife,
            string RpPolicyNumber,
            string RpInsuredGenderCode,
            string RpMlreBenefitCode,
            string RpRetroBenefitCode,
            string RpTerritoryOfIssueCode,
            string RpCurrencyCode,
            string RpInsuredTobaccoUse,
            int? RpReinsuranceIssueAge,
            string RpReinsEffDatePol,
            string RpUnderwriterRating,
            string RpRetroPremFreq,
            string RpSumOfNetPremium,
            string RpNetPremium,
            string RpSumOfAar,
            string RpAar,
            string RpRetentionLimit,
            string RpDistributedRetentionLimit,
            string RpRetroAmount,
            string RpDistributedRetroAmount,
            string RpAccumulativeRetainAmount,
            bool? RpErrors,
            int? Page
        )
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = PerLifeAggregationDetailService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index", "PerLifeAggregation");
            }

            var model = new PerLifeAggregationDetailViewModel(bo);

            ListEmptyRiData();

            ListEmptyException();

            ListEmptyRetroRiData();

            ListRetentionPremium(
                id,
                Page,
                RpUniqueKeyPerLife,
                RpPolicyNumber,
                RpInsuredGenderCode,
                RpMlreBenefitCode,
                RpRetroBenefitCode,
                RpTerritoryOfIssueCode,
                RpCurrencyCode,
                RpInsuredTobaccoUse,
                RpReinsuranceIssueAge,
                RpReinsEffDatePol,
                RpUnderwriterRating,
                RpRetroPremFreq,
                RpSumOfNetPremium,
                RpNetPremium,
                RpSumOfAar,
                RpAar,
                RpRetentionLimit,
                RpDistributedRetentionLimit,
                RpRetroAmount,
                RpDistributedRetroAmount,
                RpAccumulativeRetainAmount,
                RpErrors
            );

            ListEmptyTreatySummary();

            model.ActiveTab = PerLifeAggregationDetailBo.ActiveTabRetentionPremium;
            LoadPage(model);
            return View("Edit", model);
        }

        //GET: PerLifeAggregationDetails/Summary/5
        public ActionResult Summary(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = PerLifeAggregationDetailService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index", "PerLifeAggregation");
            }

            var model = new PerLifeAggregationDetailViewModel(bo);

            ListEmptyRiData();

            ListEmptyException();

            ListEmptyRetroRiData();

            ListEmptyRetentionPremium();

            ListExcludedRecordSummary(bo.Id);

            ListRetroRecordSummary(bo.Id);

            ListEmptyTreatySummary();

            model.ActiveTab = PerLifeAggregationDetailBo.ActiveTabSummary;
            LoadPage(model);
            return View("Edit", model);
        }

        //GET: PerLifeAggregationDetails/TreatySummary/5
        public ActionResult TreatySummary
        (
            int id,
            int? Page
        )
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = PerLifeAggregationDetailService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index", "PerLifeAggregation");
            }

            var model = new PerLifeAggregationDetailViewModel(bo);

            ListEmptyRiData();

            ListEmptyException();

            ListEmptyRetroRiData();

            ListEmptyRetentionPremium();

            ListTreatySummary(
                id,
                Page
            );

            model.ActiveTab = PerLifeAggregationDetailBo.ActiveTabTreatySummary;
            LoadPage(model);
            return View("Edit", model);
        }

        // POST: PerLifeAggregation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, int? Page, PerLifeAggregationDetailViewModel model)
        {
            var dbBo = PerLifeAggregationDetailService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index", "PerLifeAggregation");
            }

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();

                Result = PerLifeAggregationDetailService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Update Per Life Aggregation Detail"
                    );

                    SetUpdateSuccessMessage(SubController, false);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            ListRiData(
                id,
                Page
            );

            ListEmptyException();

            ListEmptyRetroRiData();

            ListEmptyRetentionPremium();

            model.PerLifeAggregationBo = dbBo.PerLifeAggregationBo;
            model.ActiveTab = PerLifeAggregationDetailBo.ActiveTabRiData;
            LoadPage(model);
            return View(model);
        }

        // POST: PerLifeAggregation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Exception(int id, int? Page, PerLifeAggregationDetailViewModel model)
        {
            var dbBo = PerLifeAggregationDetailService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index", "PerLifeAggregation");
            }

            ListEmptyRiData();

            ListException(
                id,
                Page
            );

            ListEmptyRetroRiData();

            ListEmptyRetentionPremium();

            model.PerLifeAggregationBo = dbBo.PerLifeAggregationBo;
            model.ActiveTab = PerLifeAggregationDetailBo.ActiveTabRiData;
            LoadPage(model);
            return View(model);
        }

        public void ListRiData(
            int id,
            int? Page,
            // Search RI Data
            int? SearchRiskMonth = null,
            string SearchTreatyCode = null,
            string SearchInsuredName = null,
            string SearchInsuredDateOfBirth = null,
            string SearchPolicyNumber = null,
            string SearchMlreBenefitCode = null,
            string SearchTransactionTypeCode = null,
            // Filter RI Data
            string RiTreatyCode = null,
            string RiPolicyNumber = null,
            string RiReinsBasisCode = null,
            string RiInsuredName = null,
            string RiInsuredGenderCode = null,
            string RiInsuredDateOfBirth = null,
            int? RiReinsuranceIssueAge = null,
            string RiCedingPlanCode = null,
            string RiCedingBenefitTypeCode = null,
            string RiCedingBenefitRiskCode = null,
            string RiMlreBenefitCode = null,
            string RiAar = null,
            string RiGrossPremium = null,
            string RiTotalDiscount = null,
            string RiNetPremium = null,
            string RiBrokerageFee = null,
            string RiGroupPolicyNumber = null,
            string RiGroupPolicyName = null,
            string RiPolicyTerm = null,
            string RiPolicyExpiryDate = null,
            string RiCurrencyCode = null,
            string RiTerritoryOfIssueCode = null,
            string RiTransactionTypeCode = null,
            string RiIssueDatePol = null,
            string RiReinsEffDatePol = null,
            int? RiReportPeriodMonth = null,
            int? RiReportPeriodYear = null,
            int? RiRiskPeriodMonth = null,
            int? RiRiskPeriodYear = null,
            string RiRiskPeriodStartDate = null,
            string RiRiskPeriodEndDate = null
        )
        {
            // Search RI Data
            DateTime? searchInsuredDateOfBirth = Util.GetParseDateTime(SearchInsuredDateOfBirth);

            // Filter RI Data
            DateTime? riInsuredDateOfBirth = Util.GetParseDateTime(RiInsuredDateOfBirth);
            DateTime? riPolicyExpiryDate = Util.GetParseDateTime(RiPolicyExpiryDate);
            DateTime? riIssueDatePol = Util.GetParseDateTime(RiIssueDatePol);
            DateTime? riReinsEffDatePol = Util.GetParseDateTime(RiReinsEffDatePol);
            DateTime? riRiskPeriodStartDate = Util.GetParseDateTime(RiRiskPeriodStartDate);
            DateTime? riRiskPeriodEndDate = Util.GetParseDateTime(RiRiskPeriodEndDate);

            double? riAar = Util.StringToDouble(RiAar);
            double? riGrossPremium = Util.StringToDouble(RiGrossPremium);
            double? riTotalDiscount = Util.StringToDouble(RiTotalDiscount);
            double? riNetPremium = Util.StringToDouble(RiNetPremium);
            double? riBrokerageFee = Util.StringToDouble(RiBrokerageFee);
            double? riPolicyTerm = Util.StringToDouble(RiPolicyTerm);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["SearchRiskMonth"] = SearchRiskMonth,
                ["SearchTreatyCode"] = SearchTreatyCode,
                ["SearchInsuredName"] = SearchInsuredName,
                ["SearchInsuredDateOfBirth"] = searchInsuredDateOfBirth.HasValue ? SearchInsuredDateOfBirth : null,
                ["SearchPolicyNumber"] = SearchPolicyNumber,
                ["SearchMlreBenefitCode"] = SearchMlreBenefitCode,
                ["SearchTransactionTypeCode"] = SearchTransactionTypeCode,
                ["RiTreatyCode"] = RiTreatyCode,
                ["RiPolicyNumber"] = RiPolicyNumber,
                ["RiReinsBasisCode"] = RiReinsBasisCode,
                ["RiInsuredName"] = RiInsuredName,
                ["RiInsuredGenderCode"] = RiInsuredGenderCode,
                ["RiInsuredDateOfBirth"] = riInsuredDateOfBirth.HasValue ? RiInsuredDateOfBirth : null,
                ["RiReinsuranceIssueAge"] = RiReinsuranceIssueAge,
                ["RiCedingPlanCode"] = RiCedingPlanCode,
                ["RiCedingBenefitTypeCode"] = RiCedingBenefitTypeCode,
                ["RiCedingBenefitRiskCode"] = RiCedingBenefitRiskCode,
                ["RiMlreBenefitCode"] = RiMlreBenefitCode,
                ["RiAar"] = riAar.HasValue ? RiAar : null,
                ["RiGrossPremium"] = riGrossPremium.HasValue ? RiGrossPremium : null,
                ["RiTotalDiscount"] = riTotalDiscount.HasValue ? RiTotalDiscount : null,
                ["RiNetPremium"] = riNetPremium.HasValue ? RiNetPremium : null,
                ["RiBrokerageFee"] = riBrokerageFee.HasValue ? RiBrokerageFee : null,
                ["RiGroupPolicyNumber"] = RiGroupPolicyNumber,
                ["RiGroupPolicyName"] = RiGroupPolicyName,
                ["RiPolicyTerm"] = riPolicyTerm.HasValue ? RiPolicyTerm : null,
                ["RiPolicyExpiryDate"] = riPolicyExpiryDate.HasValue ? RiPolicyExpiryDate : null,
                ["RiCurrencyCode"] = RiCurrencyCode,
                ["RiTerritoryOfIssueCode"] = RiTerritoryOfIssueCode,
                ["RiTransactionTypeCode"] = RiTransactionTypeCode,
                ["RiIssueDatePol"] = riIssueDatePol.HasValue ? RiIssueDatePol : null,
                ["RiReinsEffDatePol"] = riReinsEffDatePol.HasValue ? RiReinsEffDatePol : null,
                ["RiReportPeriodMonth"] = RiReportPeriodMonth,
                ["RiReportPeriodYear"] = RiReportPeriodYear,
                ["RiRiskPeriodMonth"] = RiRiskPeriodMonth,
                ["RiRiskPeriodYear"] = RiRiskPeriodYear,
                ["RiRiskPeriodStartDate"] = riRiskPeriodStartDate.HasValue ? RiRiskPeriodStartDate : null,
                ["RiRiskPeriodEndDate"] = riRiskPeriodEndDate.HasValue ? RiRiskPeriodEndDate : null,
            };

            _db.Database.CommandTimeout = 0;

            var query = _db.PerLifeAggregationDetailData.Where(q => q.PerLifeAggregationDetailTreaty.PerLifeAggregationDetailId == id).Select(PerLifeAggregationDetailDataViewModel.Expression());

            //Search Parameter
            if (SearchRiskMonth.HasValue) query = query.Where(q => q.RiskPeriodMonth == SearchRiskMonth);
            if (!string.IsNullOrEmpty(SearchTreatyCode)) query = query.Where(q => q.TreatyCode == SearchTreatyCode);
            if (!string.IsNullOrEmpty(SearchInsuredName)) query = query.Where(q => q.InsuredName == SearchInsuredName);
            if (searchInsuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == searchInsuredDateOfBirth);
            if (!string.IsNullOrEmpty(SearchPolicyNumber)) query = query.Where(q => q.PolicyNumber == SearchPolicyNumber);
            if (!string.IsNullOrEmpty(SearchMlreBenefitCode)) query = query.Where(q => q.MlreBenefitCode == SearchMlreBenefitCode);
            if (!string.IsNullOrEmpty(SearchTransactionTypeCode)) query = query.Where(q => q.TransactionTypeCode == SearchTransactionTypeCode);

            // Filter Parameters
            if (!string.IsNullOrEmpty(RiTreatyCode)) query = query.Where(q => q.TreatyCode == RiTreatyCode);
            if (!string.IsNullOrEmpty(RiPolicyNumber)) query = query.Where(q => q.PolicyNumber == RiPolicyNumber);
            if (!string.IsNullOrEmpty(RiReinsBasisCode)) query = query.Where(q => q.ReinsBasisCode == RiReinsBasisCode);
            if (!string.IsNullOrEmpty(RiInsuredName)) query = query.Where(q => q.InsuredName == RiInsuredName);
            if (!string.IsNullOrEmpty(RiInsuredGenderCode)) query = query.Where(q => q.InsuredGenderCode == RiInsuredGenderCode);
            if (riInsuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == riInsuredDateOfBirth);
            if (RiReinsuranceIssueAge.HasValue) query = query.Where(q => q.ReinsuranceIssueAge == RiReinsuranceIssueAge);
            if (!string.IsNullOrEmpty(RiCedingPlanCode)) query = query.Where(q => q.CedingPlanCode == RiCedingPlanCode);
            if (!string.IsNullOrEmpty(RiCedingBenefitTypeCode)) query = query.Where(q => q.CedingBenefitTypeCode == RiCedingBenefitTypeCode);
            if (!string.IsNullOrEmpty(RiCedingBenefitRiskCode)) query = query.Where(q => q.CedingBenefitRiskCode == RiCedingBenefitRiskCode);
            if (!string.IsNullOrEmpty(RiMlreBenefitCode)) query = query.Where(q => q.MlreBenefitCode == RiMlreBenefitCode);
            if (riAar.HasValue) query = query.Where(q => q.Aar == riAar);
            if (riGrossPremium.HasValue) query = query.Where(q => q.GrossPremium == riGrossPremium);
            if (riTotalDiscount.HasValue) query = query.Where(q => q.TotalDiscount == riTotalDiscount);
            if (riTotalDiscount.HasValue) query = query.Where(q => q.TotalDiscount == riTotalDiscount);
            if (riNetPremium.HasValue) query = query.Where(q => q.NetPremium == riNetPremium);
            if (riBrokerageFee.HasValue) query = query.Where(q => q.BrokerageFee == riBrokerageFee);
            if (!string.IsNullOrEmpty(RiGroupPolicyNumber)) query = query.Where(q => q.GroupPolicyNumber == RiGroupPolicyNumber);
            if (!string.IsNullOrEmpty(RiGroupPolicyName)) query = query.Where(q => q.GroupPolicyName == RiGroupPolicyName);
            if (riPolicyTerm.HasValue) query = query.Where(q => q.PolicyTerm == riPolicyTerm);
            if (riPolicyExpiryDate.HasValue) query = query.Where(q => q.PolicyExpiryDate == riPolicyExpiryDate);
            if (!string.IsNullOrEmpty(RiCurrencyCode)) query = query.Where(q => q.CurrencyCode == RiCurrencyCode);
            if (!string.IsNullOrEmpty(RiTerritoryOfIssueCode)) query = query.Where(q => q.TerritoryOfIssueCode == RiTerritoryOfIssueCode);
            if (!string.IsNullOrEmpty(RiTransactionTypeCode)) query = query.Where(q => q.TransactionTypeCode == RiTransactionTypeCode);
            if (riIssueDatePol.HasValue) query = query.Where(q => q.IssueDatePol == riIssueDatePol);
            if (riReinsEffDatePol.HasValue) query = query.Where(q => q.ReinsEffDatePol == riReinsEffDatePol);
            if (RiReportPeriodMonth.HasValue) query = query.Where(q => q.ReportPeriodMonth == RiReportPeriodMonth);
            if (RiReportPeriodYear.HasValue) query = query.Where(q => q.ReportPeriodYear == RiReportPeriodYear);
            if (RiRiskPeriodMonth.HasValue) query = query.Where(q => q.RiskPeriodMonth == RiRiskPeriodMonth);
            if (RiRiskPeriodYear.HasValue) query = query.Where(q => q.RiskPeriodYear == RiRiskPeriodYear);
            if (riRiskPeriodStartDate.HasValue) query = query.Where(q => q.RiskPeriodStartDate == riRiskPeriodStartDate);
            if (riRiskPeriodEndDate.HasValue) query = query.Where(q => q.RiskPeriodEndDate == riRiskPeriodEndDate);

            query = query.OrderBy(q => q.Id);

            ViewBag.RiDetailTotal = query.Count();
            ViewBag.RiDetailList = query.ToPagedList(Page ?? 1, PageSize);
        }

        public void ListEmptyRiData()
        {
            ViewBag.RouteValue = new RouteValueDictionary { };
            ViewBag.RiDetailTotal = 0;
            ViewBag.RiDetailList = new List<PerLifeAggregationDetailDataViewModel>().ToPagedList(1, PageSize);
        }

        public void ListException(
            int id,
            int? Page,
            // Filter
            int? ExRecordType = null,
            //string ExFileId = null,
            //string ExOriginalEntry = null,
            string ExPolicyNumber = null,
            string ExIssueDatePol = null,
            string ExTreatyCode = null,
            string ExReinsEffDatePol = null,
            string ExInsuredName = null,
            string ExInsuredGenderCode = null,
            string ExInsuredDateOfBirth = null,
            string ExCedingPlanCode = null,
            string ExCedingBenefitTypeCode = null,
            string ExCedingBenefitRiskCode = null,
            string ExMlreBenefitCode = null,
            string ExAar = null,
            int? ExExceptionType = null,
            int? ExProceedStatus = null
        )
        {
            DateTime? exIssueDatePol = Util.GetParseDateTime(ExIssueDatePol);
            DateTime? exReinsEffDatePol = Util.GetParseDateTime(ExReinsEffDatePol);
            DateTime? exInsuredDateOfBirth = Util.GetParseDateTime(ExInsuredDateOfBirth);

            double? exAar = Util.StringToDouble(ExAar);

            ViewBag.ExceptionRouteValue = new RouteValueDictionary
            {
                ["ExRecordType"] = ExRecordType,
                //["ExFileId"] = ExFileId,
                //["ExOriginalEntry"] = ExOriginalEntry,
                ["ExPolicyNumber"] = ExPolicyNumber,
                ["ExIssueDatePol"] = exIssueDatePol.HasValue ? ExIssueDatePol : null,
                ["ExTreatyCode"] = ExTreatyCode,
                ["ExReinsEffDatePol"] = exReinsEffDatePol.HasValue ? ExReinsEffDatePol : null,
                ["ExInsuredName"] = ExInsuredName,
                ["ExInsuredGenderCode"] = ExInsuredGenderCode,
                ["ExInsuredDateOfBirth"] = exInsuredDateOfBirth.HasValue ? ExInsuredDateOfBirth : null,
                ["ExCedingPlanCode"] = ExCedingPlanCode,
                ["ExCedingBenefitTypeCode"] = ExCedingBenefitTypeCode,
                ["ExCedingBenefitRiskCode"] = ExCedingBenefitRiskCode,
                ["ExMlreBenefitCode"] = ExMlreBenefitCode,
                ["ExAar"] = exAar.HasValue ? ExAar : null,
                ["ExExceptionType"] = ExExceptionType,
                ["ExProceedStatus"] = ExProceedStatus,
            };

            _db.Database.CommandTimeout = 0;

            var query = _db.PerLifeAggregationDetailData.Where(q => q.PerLifeAggregationDetailTreaty.PerLifeAggregationDetailId == id).Where(q => q.IsException == true).Select(PerLifeAggregationDetailDataViewModel.Expression());

            // Filter Parameters
            if (ExRecordType.HasValue) query = query.Where(q => q.RecordType == ExRecordType);
            if (!string.IsNullOrEmpty(ExPolicyNumber)) query = query.Where(q => q.PolicyNumber == ExPolicyNumber);
            if (exIssueDatePol.HasValue) query = query.Where(q => q.IssueDatePol == exIssueDatePol);
            if (!string.IsNullOrEmpty(ExTreatyCode)) query = query.Where(q => q.TreatyCode == ExTreatyCode);
            if (exReinsEffDatePol.HasValue) query = query.Where(q => q.ReinsEffDatePol == exReinsEffDatePol);
            if (!string.IsNullOrEmpty(ExInsuredName)) query = query.Where(q => q.InsuredName == ExInsuredName);
            if (!string.IsNullOrEmpty(ExInsuredGenderCode)) query = query.Where(q => q.InsuredGenderCode == ExInsuredGenderCode);
            if (exInsuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == exInsuredDateOfBirth);
            if (!string.IsNullOrEmpty(ExCedingPlanCode)) query = query.Where(q => q.CedingPlanCode == ExCedingPlanCode);
            if (!string.IsNullOrEmpty(ExCedingBenefitTypeCode)) query = query.Where(q => q.CedingBenefitTypeCode == ExCedingBenefitTypeCode);
            if (!string.IsNullOrEmpty(ExCedingBenefitRiskCode)) query = query.Where(q => q.CedingBenefitRiskCode == ExCedingBenefitRiskCode);
            if (!string.IsNullOrEmpty(ExMlreBenefitCode)) query = query.Where(q => q.MlreBenefitCode == ExMlreBenefitCode);
            if (exAar.HasValue) query = query.Where(q => q.Aar == exAar);
            if (ExExceptionType.HasValue) query = query.Where(q => q.ExceptionType == ExExceptionType);
            if (ExProceedStatus.HasValue) query = query.Where(q => q.ProceedStatus == ExProceedStatus);

            query = query.OrderBy(q => q.Id);

            ViewBag.ExceptionListTotal = query.Count();
            ViewBag.ExceptionList = query.ToPagedList(Page ?? 1, PageSize);
        }

        public void ListEmptyException()
        {
            ViewBag.ExceptionRouteValue = new RouteValueDictionary { };
            ViewBag.ExceptionListTotal = 0;
            ViewBag.ExceptionList = new List<PerLifeAggregationDetailDataViewModel>().ToPagedList(1, PageSize);
        }

        public void ListRetroRiData(
            int id,
            int? Page,
            // Filter
            string ReTreatyCode = null,
            int? ReRiskYear = null,
            int? ReRiskMonth = null,
            string ReInsuredGenderCode = null,
            string ReInsuredDateOfBirth = null,
            string RePolicyNumber = null,
            string ReReinsEffDatePol = null,
            string ReAar = null,
            string ReGrossPremium = null,
            string ReNetPremium = null,
            string RePremiumFrequencyCode = null,
            string ReRetroPremFreq = null,
            string ReCedingPlanCode = null,
            string ReCedingBenefitTypeCode = null,
            string ReCedingBenefitRiskCode = null,
            string ReMlreBenefitCode = null,
            string ReRetroBenefitCode = null
        )
        {
            DateTime? reInsuredDateOfBirth = Util.GetParseDateTime(ReInsuredDateOfBirth);
            DateTime? reReinsEffDatePol = Util.GetParseDateTime(ReReinsEffDatePol);

            double? reAar = Util.StringToDouble(ReAar);
            double? reGrossPremium = Util.StringToDouble(ReGrossPremium);
            double? reNetPremium = Util.StringToDouble(ReNetPremium);

            ViewBag.RetroRiDataRouteValue = new RouteValueDictionary
            {
                ["ReTreatyCode"] = ReTreatyCode,
                ["ReRiskYear"] = ReRiskYear,
                ["ReRiskMonth"] = ReRiskMonth,
                ["ReInsuredGenderCode"] = ReInsuredGenderCode,
                ["ReInsuredDateOfBirth"] = reInsuredDateOfBirth.HasValue ? ReInsuredDateOfBirth : null,
                ["RePolicyNumber"] = RePolicyNumber,
                ["ReReinsEffDatePol"] = reReinsEffDatePol.HasValue ? ReReinsEffDatePol : null,
                ["ReAar"] = reAar.HasValue ? ReAar : null,
                ["ReGrossPremium"] = reGrossPremium.HasValue ? ReGrossPremium : null,
                ["ReNetPremium"] = reNetPremium.HasValue ? ReNetPremium : null,
                ["RePremiumFrequencyCode"] = RePremiumFrequencyCode,
                ["ReRetroPremFreq"] = ReRetroPremFreq,
                ["ReCedingPlanCode"] = ReCedingPlanCode,
                ["ReCedingBenefitTypeCode"] = ReCedingBenefitTypeCode,
                ["ReCedingBenefitRiskCode"] = ReCedingBenefitRiskCode,
                ["ReMlreBenefitCode"] = ReMlreBenefitCode,
                ["ReRetroBenefitCode"] = ReRetroBenefitCode,
            };

            _db.Database.CommandTimeout = 0;

            var query = _db.PerLifeAggregationMonthlyData.Where(q => q.PerLifeAggregationDetailData.PerLifeAggregationDetailTreaty.PerLifeAggregationDetailId == id).Select(PerLifeAggregationMonthlyDataViewModel.Expression());

            // Filter Parameters
            if (!string.IsNullOrEmpty(ReTreatyCode)) query = query.Where(q => q.TreatyCode == ReTreatyCode);
            if (ReRiskYear.HasValue) query = query.Where(q => q.RiskYear == ReRiskYear);
            if (ReRiskMonth.HasValue) query = query.Where(q => q.RiskMonth == ReRiskMonth);
            if (!string.IsNullOrEmpty(ReInsuredGenderCode)) query = query.Where(q => q.InsuredGenderCode == ReInsuredGenderCode);
            if (reInsuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == reInsuredDateOfBirth);
            if (!string.IsNullOrEmpty(RePolicyNumber)) query = query.Where(q => q.PolicyNumber == RePolicyNumber);
            if (reReinsEffDatePol.HasValue) query = query.Where(q => q.ReinsEffDatePol == reReinsEffDatePol);
            if (reAar.HasValue) query = query.Where(q => q.Aar == reAar);
            if (reGrossPremium.HasValue) query = query.Where(q => q.GrossPremium == reGrossPremium);
            if (reNetPremium.HasValue) query = query.Where(q => q.NetPremium == reNetPremium);
            if (!string.IsNullOrEmpty(RePremiumFrequencyCode)) query = query.Where(q => q.PremiumFrequencyCode == RePremiumFrequencyCode);
            if (!string.IsNullOrEmpty(ReRetroPremFreq)) query = query.Where(q => q.RetroPremFreq == ReRetroPremFreq);
            if (!string.IsNullOrEmpty(ReCedingPlanCode)) query = query.Where(q => q.CedingPlanCode == ReCedingPlanCode);
            if (!string.IsNullOrEmpty(ReCedingBenefitTypeCode)) query = query.Where(q => q.CedingBenefitTypeCode == ReCedingBenefitTypeCode);
            if (!string.IsNullOrEmpty(ReCedingBenefitRiskCode)) query = query.Where(q => q.CedingBenefitRiskCode == ReCedingBenefitRiskCode);
            if (!string.IsNullOrEmpty(ReMlreBenefitCode)) query = query.Where(q => q.MlreBenefitCode == ReMlreBenefitCode);
            if (!string.IsNullOrEmpty(ReRetroBenefitCode)) query = query.Where(q => q.RetroBenefitCode == ReRetroBenefitCode);

            query = query.OrderBy(q => q.Id);

            ViewBag.RetroRiDataListTotal = query.Count();
            ViewBag.RetroRiDataList = query.ToPagedList(Page ?? 1, PageSize);
        }

        public void ListEmptyRetroRiData()
        {
            ViewBag.RetroRiDataRouteValue = new RouteValueDictionary { };
            ViewBag.RetroRiDataListTotal = 0;
            ViewBag.RetroRiDataList = new List<PerLifeAggregationMonthlyDataViewModel>().ToPagedList(1, PageSize);
        }

        public void ListRetentionPremium(
            int id,
            int? Page,
            // Filter
            string RpUniqueKeyPerLife = null,
            string RpPolicyNumber = null,
            string RpInsuredGenderCode = null,
            string RpMlreBenefitCode = null,
            string RpRetroBenefitCode = null,
            string RpTerritoryOfIssueCode = null,
            string RpCurrencyCode = null,
            string RpInsuredTobaccoUse = null,
            int? RpReinsuranceIssueAge = null,
            string RpReinsEffDatePol = null,
            string RpUnderwriterRating = null,
            string RpRetroPremFreq = null,
            string RpSumOfNetPremium = null,
            string RpNetPremium = null,
            string RpSumOfAar = null,
            string RpAar = null,
            string RpRetentionLimit = null,
            string RpDistributedRetentionLimit = null,
            string RpRetroAmount = null,
            string RpDistributedRetroAmount = null,
            string RpAccumulativeRetainAmount = null,
            bool? RpErrors = null
        )
        {
            DateTime? rpReinsEffDatePol = Util.GetParseDateTime(RpReinsEffDatePol);

            double? rpUnderwriterRating = Util.StringToDouble(RpUnderwriterRating);
            double? rpSumOfNetPremium = Util.StringToDouble(RpSumOfNetPremium);
            double? rpNetPremium = Util.StringToDouble(RpNetPremium);
            double? rpSumOfAar = Util.StringToDouble(RpSumOfAar);
            double? rpAar = Util.StringToDouble(RpAar);
            double? rpRetentionLimit = Util.StringToDouble(RpRetentionLimit);
            double? rpDistributedRetentionLimit = Util.StringToDouble(RpDistributedRetentionLimit);
            double? rpRetroAmount = Util.StringToDouble(RpRetroAmount);
            double? rpDistributedRetroAmount = Util.StringToDouble(RpDistributedRetroAmount);
            double? rpAccumulativeRetainAmount = Util.StringToDouble(RpAccumulativeRetainAmount);

            ViewBag.RetentionPremiumRouteValue = new RouteValueDictionary
            {
                ["RpUniqueKeyPerLife"] = RpUniqueKeyPerLife,
                ["RpPolicyNumber"] = RpPolicyNumber,
                ["RpInsuredGenderCode"] = RpInsuredGenderCode,
                ["RpMlreBenefitCode"] = RpMlreBenefitCode,
                ["RpRetroBenefitCode"] = RpRetroBenefitCode,
                ["RpTerritoryOfIssueCode"] = RpTerritoryOfIssueCode,
                ["RpCurrencyCode"] = RpCurrencyCode,
                ["RpInsuredTobaccoUse"] = RpInsuredTobaccoUse,
                ["RpReinsuranceIssueAge"] = RpReinsuranceIssueAge,
                ["RpReinsEffDatePol"] = rpReinsEffDatePol.HasValue ? RpReinsEffDatePol : null,
                ["RpUnderwriterRating"] = rpUnderwriterRating.HasValue ? RpUnderwriterRating : null,
                ["RpRetroPremFreq"] = RpRetroPremFreq,
                ["RpSumOfNetPremium"] = rpSumOfNetPremium.HasValue ? RpSumOfNetPremium : null,
                ["RpNetPremium"] = rpNetPremium.HasValue ? RpNetPremium : null,
                ["RpSumOfAar"] = rpSumOfAar.HasValue ? RpSumOfAar : null,
                ["RpAar"] = rpAar.HasValue ? RpAar : null,
                ["RpRetentionLimit"] = rpRetentionLimit.HasValue ? RpRetentionLimit : null,
                ["RpDistributedRetentionLimit"] = rpDistributedRetentionLimit.HasValue ? RpDistributedRetentionLimit : null,
                ["RpRetroAmount"] = rpRetroAmount.HasValue ? RpRetroAmount : null,
                ["RpDistributedRetroAmount"] = rpDistributedRetroAmount.HasValue ? RpDistributedRetroAmount : null,
                ["RpAccumulativeRetainAmount"] = rpAccumulativeRetainAmount.HasValue ? RpAccumulativeRetainAmount : null,
                ["RpErrors"] = RpErrors,
            };

            _db.Database.CommandTimeout = 0;

            var retroParties = PerLifeAggregationMonthlyRetroDataService.GetDistinctRetroPartyByPerLifeAggregationDetailId(id);

            var query = _db.PerLifeAggregationMonthlyData.Where(q => q.PerLifeAggregationDetailData.PerLifeAggregationDetailTreaty.PerLifeAggregationDetailId == id).Select(PerLifeAggregationMonthlyDataViewModel.Expression());

            // Filter Parameters
            if (!string.IsNullOrEmpty(RpUniqueKeyPerLife)) query = query.Where(q => RpUniqueKeyPerLife.Contains(q.UniqueKeyPerLife));
            if (!string.IsNullOrEmpty(RpPolicyNumber)) query = query.Where(q => q.PolicyNumber == RpPolicyNumber);
            if (!string.IsNullOrEmpty(RpInsuredGenderCode)) query = query.Where(q => q.InsuredGenderCode == RpInsuredGenderCode);
            if (!string.IsNullOrEmpty(RpMlreBenefitCode)) query = query.Where(q => q.MlreBenefitCode == RpMlreBenefitCode);
            if (!string.IsNullOrEmpty(RpRetroBenefitCode)) query = query.Where(q => q.RetroBenefitCode == RpRetroBenefitCode);
            if (!string.IsNullOrEmpty(RpTerritoryOfIssueCode)) query = query.Where(q => q.TerritoryOfIssueCode == RpTerritoryOfIssueCode);
            if (!string.IsNullOrEmpty(RpCurrencyCode)) query = query.Where(q => q.CurrencyCode == RpCurrencyCode);
            if (!string.IsNullOrEmpty(RpInsuredTobaccoUse)) query = query.Where(q => q.InsuredTobaccoUse == RpInsuredTobaccoUse);
            if (RpReinsuranceIssueAge.HasValue) query = query.Where(q => q.ReinsuranceIssueAge == RpReinsuranceIssueAge);
            if (rpReinsEffDatePol.HasValue) query = query.Where(q => q.ReinsEffDatePol == rpReinsEffDatePol);
            if (rpUnderwriterRating.HasValue) query = query.Where(q => q.UnderwriterRating == rpUnderwriterRating);
            if (!string.IsNullOrEmpty(RpRetroPremFreq)) query = query.Where(q => q.RetroPremFreq == RpRetroPremFreq);
            if (rpSumOfNetPremium.HasValue) query = query.Where(q => q.SumOfNetPremium == rpSumOfNetPremium);
            if (rpNetPremium.HasValue) query = query.Where(q => q.NetPremium == rpNetPremium);
            if (rpSumOfAar.HasValue) query = query.Where(q => q.SumOfAar == rpSumOfAar);
            if (rpAar.HasValue) query = query.Where(q => q.Aar == rpAar);
            if (rpRetentionLimit.HasValue) query = query.Where(q => q.RetentionLimit == rpRetentionLimit);
            if (rpDistributedRetentionLimit.HasValue) query = query.Where(q => q.DistributedRetentionLimit == rpDistributedRetentionLimit);
            if (rpRetroAmount.HasValue) query = query.Where(q => q.RetroAmount == rpRetroAmount);
            if (rpDistributedRetroAmount.HasValue) query = query.Where(q => q.DistributedRetroAmount == rpDistributedRetroAmount);
            if (rpAccumulativeRetainAmount.HasValue) query = query.Where(q => q.AccumulativeRetainAmount == rpAccumulativeRetainAmount);
            if (RpErrors.HasValue && RpErrors.Value == true) query = query.Where(q => q.Errors != null);
            if (RpErrors.HasValue && RpErrors.Value == false) query = query.Where(q => q.Errors == null);

            query = query.OrderBy(q => q.Id);

            ViewBag.RetroParties = retroParties;
            ViewBag.TotalRetroParty = retroParties.Count();

            ViewBag.RetentionPremiumListTotal = query.Count();
            ViewBag.RetentionPremiumList = query.ToPagedList(Page ?? 1, PageSize);
        }

        public void ListEmptyRetentionPremium()
        {
            ViewBag.RetroParties = new List<string> { };
            ViewBag.TotalRetroParty = 0;

            ViewBag.RetentionPremiumRouteValue = new RouteValueDictionary { };
            ViewBag.RetentionPremiumListTotal = 0;
            ViewBag.RetentionPremiumList = new List<PerLifeAggregationMonthlyDataViewModel>().ToPagedList(1, PageSize);
        }

        public void ListExcludedRecordSummary(int perLifeAggregationDetailId)
        {
            _db.Database.CommandTimeout = 0;

            var perLifeAggregationDetailTreatyBos = _db.PerLifeAggregationDetailData
                .Where(q => q.PerLifeAggregationDetailTreaty.PerLifeAggregationDetailId == perLifeAggregationDetailId)
                .Where(q => q.ProceedStatus != PerLifeAggregationDetailDataBo.ProceedStatusProceed)
                .GroupBy(
                    q => q.PerLifeAggregationDetailTreatyId,
                    (key, DetailData) => new PerLifeAggregationDetailTreatyBo
                    {
                        RiskQuarter = DetailData.Select(q => q.PerLifeAggregationDetailTreaty.PerLifeAggregationDetail.RiskQuarter).FirstOrDefault(),
                        TreatyCode = DetailData.Select(q => q.PerLifeAggregationDetailTreaty.TreatyCode).FirstOrDefault(),
                        Count = DetailData.Count(),
                        TotalAar = DetailData.Sum(q => q.RiDataWarehouseHistory.Aar) ?? 0,
                        TotalGrossPremium = DetailData.Sum(q => q.RiDataWarehouseHistory.GrossPremium) ?? 0,
                        TotalNetPremium = DetailData.Sum(q => q.RiDataWarehouseHistory.NetPremium) ?? 0,
                    }
                )
                .OrderByDescending(q => q.RiskQuarter)
                .ThenBy(q => q.TreatyCode)
                .ToList();

            foreach (var bo in perLifeAggregationDetailTreatyBos)
            {
                bo.TotalAarStr = Util.DoubleToString(bo.TotalAar);
                bo.TotalGrossPremiumStr = Util.DoubleToString(bo.TotalGrossPremium);
                bo.TotalNetPremiumStr = Util.DoubleToString(bo.TotalNetPremium);
            }

            ViewBag.Excludedrecords = perLifeAggregationDetailTreatyBos;
        }

        public void ListRetroRecordSummary(int perLifeAggregationDetailId)
        {
            _db.Database.CommandTimeout = 0;

            var perLifeAggregationDetailTreatyBos = _db.PerLifeAggregationMonthlyData
                .Where(q => q.PerLifeAggregationDetailData.PerLifeAggregationDetailTreaty.PerLifeAggregationDetailId == perLifeAggregationDetailId)
                .GroupBy(
                    q => q.PerLifeAggregationDetailData.PerLifeAggregationDetailTreatyId,
                    (key, MonthlyData) => new PerLifeAggregationDetailTreatyBo
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
                )
                .OrderByDescending(q => q.RiskQuarter)
                .ThenBy(q => q.TreatyCode)
                .ToList();

            foreach (var bo in perLifeAggregationDetailTreatyBos)
            {
                bo.TotalAarStr = Util.DoubleToString(bo.TotalAar);
                bo.TotalGrossPremiumStr = Util.DoubleToString(bo.TotalGrossPremium);
                bo.TotalNetPremiumStr = Util.DoubleToString(bo.TotalNetPremium);

                bo.TotalAarStr2 = Util.DoubleToString(bo.TotalAar2);
                bo.TotalGrossPremiumStr2 = Util.DoubleToString(bo.TotalGrossPremium2);
                bo.TotalNetPremiumStr2 = Util.DoubleToString(bo.TotalNetPremium2);

                bo.TotalRetroAmountStr3 = Util.DoubleToString(bo.TotalRetroAmount3);
                bo.TotalGrossPremiumStr3 = Util.DoubleToString(bo.TotalGrossPremium3);
                bo.TotalNetPremiumStr3 = Util.DoubleToString(bo.TotalNetPremium3);
                bo.TotalDiscountStr3 = Util.DoubleToString(bo.TotalDiscount3);
            }

            ViewBag.Retrorecords = perLifeAggregationDetailTreatyBos;
        }

        public void ListTreatySummary(
            int id,
            int? Page
        )
        {
            _db.Database.CommandTimeout = 0;

            var query = _db.PerLifeAggregationMonthlyData
                .Where(q => q.PerLifeAggregationDetailData.PerLifeAggregationDetailTreaty.PerLifeAggregationDetailId == id)
                .GroupBy(
                    q => new { 
                        q.PerLifeAggregationDetailData.PerLifeAggregationDetailTreaty.PerLifeAggregationDetail.RiskQuarter, 
                        q.RiskMonth, 
                        q.PerLifeAggregationDetailData.PerLifeAggregationDetailTreaty.TreatyCode, 
                        q.PerLifeAggregationDetailData.RiDataWarehouseHistory.TransactionTypeCode 
                    },
                    (key, MonthlyData) => new PerLifeAggregationDetailTreatyViewModel
                    {
                        RiskQuarter = MonthlyData.Select(q => q.PerLifeAggregationDetailData.PerLifeAggregationDetailTreaty.PerLifeAggregationDetail.RiskQuarter).FirstOrDefault(),
                        RiskMonth = MonthlyData.Select(q => q.RiskMonth).FirstOrDefault(),
                        TreatyCode = MonthlyData.Select(q => q.PerLifeAggregationDetailData.PerLifeAggregationDetailTreaty.TreatyCode).FirstOrDefault(),
                        TransactionTypeCode = MonthlyData.Select(q => q.PerLifeAggregationDetailData.RiDataWarehouseHistory.TransactionTypeCode).FirstOrDefault(),
                        PolicyCount = MonthlyData.Count(),
                        Aar = MonthlyData.Sum(q => q.Aar),
                        GrossPremium = MonthlyData.Sum(q => q.RetroGrossPremium) ?? 0,
                        NetPremium = MonthlyData.Sum(q => q.NetPremium),
                        StandardPremium = MonthlyData.Sum(q => q.PerLifeAggregationDetailData.RiDataWarehouseHistory.StandardPremium) ?? 0,
                        SubstandardPremium = MonthlyData.Sum(q => q.PerLifeAggregationDetailData.RiDataWarehouseHistory.SubstandardPremium) ?? 0,
                        FlatExtraPremium = MonthlyData.Sum(q => q.PerLifeAggregationDetailData.RiDataWarehouseHistory.FlatExtraPremium) ?? 0,
                        StandardDiscount = MonthlyData.Sum(q => q.PerLifeAggregationDetailData.RiDataWarehouseHistory.StandardDiscount) ?? 0,
                        SubstandardDiscount = MonthlyData.Sum(q => q.PerLifeAggregationDetailData.RiDataWarehouseHistory.SubstandardDiscount) ?? 0,
                    }
                );

            query = query
                .OrderByDescending(q => q.RiskQuarter)
                .ThenBy(q => q.RiskMonth)
                .ThenBy(q => q.TreatyCode)
                .ThenBy(q => q.TransactionTypeCode);

            ViewBag.TreatySummaryListTotal = query.Count();
            ViewBag.TreatySummaryList = query.ToPagedList(Page ?? 1, PageSize);
        }

        public void ListEmptyTreatySummary()
        {
            ViewBag.TreatySummaryListTotal = 0;
            ViewBag.TreatySummaryList = new List<PerLifeAggregationDetailTreatyViewModel>().ToPagedList(1, PageSize);
        }

        public ActionResult DownloadRiData(
            string downloadToken,
            int id,
            // Search RI Data
            int? SearchRiskMonth = null,
            string SearchTreatyCode = null,
            string SearchInsuredName = null,
            string SearchInsuredDateOfBirth = null,
            string SearchPolicyNumber = null,
            string SearchMlreBenefitCode = null,
            string SearchTransactionTypeCode = null,
            // Filter RI Data
            string RiTreatyCode = null,
            string RiCedingTreatyCode = null,
            string RiInsuredName = null,
            string RiInsuredGenderCode = null,
            string RiInsuredDateOfBirth = null,
            string RiPolicyNumber = null,
            string RiCurrencyCode = null,
            string RiTerritoryOfIssueCode = null,
            string RiCedingPlanCode = null,
            string RiCedingBenefitRiskCode = null,
            string RiCedingBenefitTypeCode = null,
            string RiStandardPremium = null,
            string RiSubstandardPremium = null,
            string RiStandardDiscount = null,
            string RiSubstandardDiscount = null,
            string RiFlatExtraPremium = null,
            string RiFlatExtraAmount = null,
            string RiBrokerageFee = null,
            string RiRiskPeriodStartDate = null,
            string RiRiskPeriodEndDate = null,
            string RiTransactionTypeCode = null,
            string RiEffectiveDate = null,
            string RiPolicyTerm = null,
            string RiPolicyExpiryDate = null
        )
        {
            // type 1 = all
            // type 2 = filtered download

            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            dynamic Params = new ExpandoObject();
            Params.Id = id;
            // Search RI Data
            Params.SearchRiskMonth = SearchRiskMonth?.ToString();
            Params.SearchTreatyCode = SearchTreatyCode;
            Params.SearchInsuredName = SearchInsuredName;
            Params.SearchInsuredDateOfBirth = SearchInsuredDateOfBirth;
            Params.SearchPolicyNumber = SearchPolicyNumber;
            Params.SearchMlreBenefitCode = SearchMlreBenefitCode;
            Params.SearchTransactionTypeCode = SearchTransactionTypeCode;
            // Filter RI Data
            Params.TreatyCode = RiTreatyCode;
            Params.CedingTreatyCode = RiCedingTreatyCode;
            Params.InsuredName = RiInsuredName;
            Params.InsuredGenderCode = RiInsuredGenderCode;
            Params.InsuredDateOfBirth = RiInsuredDateOfBirth;
            Params.PolicyNumber = RiPolicyNumber;
            Params.CurrencyCode = RiCurrencyCode;
            Params.TerritoryOfIssueCode = RiTerritoryOfIssueCode;
            Params.CedingPlanCode = RiCedingPlanCode;
            Params.CedingBenefitRiskCode = RiCedingBenefitRiskCode;
            Params.CedingBenefitTypeCode = RiCedingBenefitTypeCode;
            Params.StandardPremium = RiStandardPremium;
            Params.SubstandardPremium = RiSubstandardPremium;
            Params.StandardDiscount = RiStandardDiscount;
            Params.SubstandardDiscount = RiSubstandardDiscount;
            Params.FlatExtraPremium = RiFlatExtraPremium;
            Params.FlatExtraAmount = RiFlatExtraAmount;
            Params.BrokerageFee = RiBrokerageFee;
            Params.RiskPeriodStartDate = RiRiskPeriodStartDate;
            Params.RiskPeriodEndDate = RiRiskPeriodEndDate;
            Params.TransactionTypeCode = RiTransactionTypeCode;
            Params.EffectiveDate = RiEffectiveDate;
            Params.PolicyTerm = RiPolicyTerm;
            Params.PolicyExpiryDate = RiPolicyExpiryDate;

            var export = new GenerateExportData();
            ExportBo exportBo = export.CreateExportPerLifeAggregationRiData(id, AuthUserId, Params);

            if (exportBo.Total <= export.ExportInstantDownloadLimit)
            {
                export.Process(ref exportBo, _db);
                Session.Add("DownloadExport", true);
            }

            return RedirectToAction("Edit", "Export", new { exportBo.Id });

            /*_db.Database.CommandTimeout = 0;
            var query = _db.PerLifeAggregationDetailData.Where(q => q.PerLifeAggregationDetailTreaty.PerLifeAggregationDetailId == id).Select(PerLifeAggregationDetailDataService.Expression());

            // Search RI Data
            DateTime? searchInsuredDateOfBirth = Util.GetParseDateTime(SearchInsuredDateOfBirth);

            // Filter RI Data
            DateTime? riInsuredDateOfBirth = Util.GetParseDateTime(RiInsuredDateOfBirth);
            DateTime? riRiskPeriodStartDate = Util.GetParseDateTime(RiRiskPeriodStartDate);
            DateTime? riRiskPeriodEndDate = Util.GetParseDateTime(RiRiskPeriodEndDate);
            DateTime? riEffectiveDate = Util.GetParseDateTime(RiEffectiveDate);
            DateTime? riPolicyExpiryDate = Util.GetParseDateTime(RiPolicyExpiryDate);

            double? riStandardPremium = Util.StringToDouble(RiStandardPremium);
            double? riSubstandardPremium = Util.StringToDouble(RiSubstandardPremium);
            double? riStandardDiscount = Util.StringToDouble(RiStandardDiscount);
            double? riSubstandardDiscount = Util.StringToDouble(RiSubstandardDiscount);
            double? riFlatExtraPremium = Util.StringToDouble(RiFlatExtraPremium);
            double? riFlatExtraAmount = Util.StringToDouble(RiFlatExtraAmount);
            double? riBrokerageFee = Util.StringToDouble(RiBrokerageFee);
            double? riPolicyTerm = Util.StringToDouble(RiPolicyTerm);

            //Search Parameter
            if (SearchRiskMonth.HasValue) query = query.Where(q => q.RiskPeriodMonth == SearchRiskMonth);
            if (!string.IsNullOrEmpty(SearchTreatyCode)) query = query.Where(q => q.TreatyCode == SearchTreatyCode);
            if (!string.IsNullOrEmpty(SearchInsuredName)) query = query.Where(q => q.InsuredName == SearchInsuredName);
            if (searchInsuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == searchInsuredDateOfBirth);
            if (!string.IsNullOrEmpty(SearchPolicyNumber)) query = query.Where(q => q.PolicyNumber == SearchPolicyNumber);
            if (!string.IsNullOrEmpty(SearchMlreBenefitCode)) query = query.Where(q => q.MlreBenefitCode == SearchMlreBenefitCode);
            if (!string.IsNullOrEmpty(SearchTransactionTypeCode)) query = query.Where(q => q.TransactionTypeCode == SearchTransactionTypeCode);

            // Filter Parameters
            if (!string.IsNullOrEmpty(RiTreatyCode)) query = query.Where(q => q.TreatyCode == RiTreatyCode);
            if (!string.IsNullOrEmpty(RiCedingTreatyCode)) query = query.Where(q => q.CedingTreatyCode == RiCedingTreatyCode);
            if (!string.IsNullOrEmpty(RiInsuredName)) query = query.Where(q => q.InsuredName == RiInsuredName);
            if (!string.IsNullOrEmpty(RiInsuredGenderCode)) query = query.Where(q => q.InsuredGenderCode == RiInsuredGenderCode);
            if (riInsuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == riInsuredDateOfBirth);
            if (!string.IsNullOrEmpty(RiPolicyNumber)) query = query.Where(q => q.PolicyNumber == RiPolicyNumber);
            if (!string.IsNullOrEmpty(RiCurrencyCode)) query = query.Where(q => q.CurrencyCode == RiCurrencyCode);
            if (!string.IsNullOrEmpty(RiInsuredGenderCode)) query = query.Where(q => q.InsuredGenderCode == RiInsuredGenderCode);
            if (!string.IsNullOrEmpty(RiTerritoryOfIssueCode)) query = query.Where(q => q.TerritoryOfIssueCode == RiTerritoryOfIssueCode);
            if (!string.IsNullOrEmpty(RiCedingPlanCode)) query = query.Where(q => q.CedingPlanCode == RiCedingPlanCode);
            if (!string.IsNullOrEmpty(RiCedingBenefitRiskCode)) query = query.Where(q => q.CedingBenefitRiskCode == RiCedingBenefitRiskCode);
            if (!string.IsNullOrEmpty(RiCedingBenefitTypeCode)) query = query.Where(q => q.CedingBenefitTypeCode == RiCedingBenefitTypeCode);
            if (riStandardPremium.HasValue) query = query.Where(q => q.StandardPremium == riStandardPremium);
            if (riSubstandardPremium.HasValue) query = query.Where(q => q.SubstandardPremium == riSubstandardPremium);
            if (riStandardDiscount.HasValue) query = query.Where(q => q.StandardDiscount == riStandardDiscount);
            if (riSubstandardDiscount.HasValue) query = query.Where(q => q.SubstandardDiscount == riSubstandardDiscount);
            if (riFlatExtraPremium.HasValue) query = query.Where(q => q.FlatExtraPremium == riFlatExtraPremium);
            if (riFlatExtraAmount.HasValue) query = query.Where(q => q.FlatExtraAmount == riFlatExtraAmount);
            if (riBrokerageFee.HasValue) query = query.Where(q => q.BrokerageFee == riBrokerageFee);
            if (riRiskPeriodStartDate.HasValue) query = query.Where(q => q.RiskPeriodStartDate == riRiskPeriodStartDate);
            if (riRiskPeriodEndDate.HasValue) query = query.Where(q => q.RiskPeriodEndDate == riRiskPeriodEndDate);
            if (!string.IsNullOrEmpty(RiTransactionTypeCode)) query = query.Where(q => q.TransactionTypeCode == RiTransactionTypeCode);
            if (riEffectiveDate.HasValue) query = query.Where(q => q.EffectiveDate == riEffectiveDate);
            if (riPolicyTerm.HasValue) query = query.Where(q => q.PolicyTerm == riPolicyTerm);
            if (riPolicyExpiryDate.HasValue) query = query.Where(q => q.PolicyExpiryDate == riPolicyExpiryDate);

            var export = new ExportPerLifeAggregationDetailData()
            {
                PrefixFileName = "RiDetails"
            };
            export.HandleTempDirectory();

            if (query != null)
                export.SetQuery(query);

            export.Process();
            return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));*/
        }

        public ActionResult DownloadException(
            string downloadToken,
            int id,
            int? ExRecordType = null,
            //string ExFileId = null,
            //string ExOriginalEntry = null,
            string ExPolicyNumber = null,
            string ExIssueDatePol = null,
            string ExTreatyCode = null,
            string ExReinsEffDatePol = null,
            string ExInsuredName = null,
            string ExInsuredGenderCode = null,
            string ExInsuredDateOfBirth = null,
            string ExCedingPlanCode = null,
            string ExCedingBenefitTypeCode = null,
            string ExCedingBenefitRiskCode = null,
            string ExMlreBenefitCode = null,
            string ExAar = null,
            int? ExExceptionType = null,
            int? ExProceedStatus = null
        )
        {
            // type 1 = all
            // type 2 = filtered download

            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            dynamic Params = new ExpandoObject();
            Params.Id = id;
            Params.RecordType = ExRecordType?.ToString();
            Params.PolicyNumber = ExPolicyNumber;
            Params.IssueDatePol = ExIssueDatePol;
            Params.TreatyCode = ExTreatyCode;
            Params.ReinsEffDatePol = ExReinsEffDatePol;
            Params.InsuredName = ExInsuredName;
            Params.InsuredGenderCode = ExInsuredGenderCode;
            Params.InsuredDateOfBirth = ExInsuredDateOfBirth;
            Params.CedingPlanCode = ExCedingPlanCode;
            Params.CedingBenefitTypeCode = ExCedingBenefitTypeCode;
            Params.CedingBenefitRiskCode = ExCedingBenefitRiskCode;
            Params.MlreBenefitCode = ExMlreBenefitCode;
            Params.Aar = ExAar;
            Params.ExceptionType = ExExceptionType?.ToString();
            Params.ProceedStatus = ExProceedStatus?.ToString();

            var export = new GenerateExportData();
            ExportBo exportBo = export.CreateExportPerLifeAggregationException(id, AuthUserId, Params);

            if (exportBo.Total <= export.ExportInstantDownloadLimit)
            {
                export.Process(ref exportBo, _db);
                Session.Add("DownloadExport", true);
            }

            return RedirectToAction("Edit", "Export", new { exportBo.Id });

            //_db.Database.CommandTimeout = 0;
            //var query = _db.PerLifeAggregationDetailData.Where(q => q.PerLifeAggregationDetailTreaty.PerLifeAggregationDetailId == id).Where(q => q.IsException == true).Select(PerLifeAggregationDetailDataService.Expression());

            //if (type == 2)
            //{
            //    DateTime? exIssueDatePol = Util.GetParseDateTime(ExIssueDatePol);
            //    DateTime? exReinsEffDatePol = Util.GetParseDateTime(ExReinsEffDatePol);
            //    DateTime? exInsuredDateOfBirth = Util.GetParseDateTime(ExInsuredDateOfBirth);

            //    double? exAar = Util.StringToDouble(ExAar);

            //    if (ExRecordType.HasValue) query = query.Where(q => q.RecordType == ExRecordType);
            //    if (!string.IsNullOrEmpty(ExPolicyNumber)) query = query.Where(q => q.PolicyNumber == ExPolicyNumber);
            //    if (exIssueDatePol.HasValue) query = query.Where(q => q.IssueDatePol == exIssueDatePol);
            //    if (!string.IsNullOrEmpty(ExTreatyCode)) query = query.Where(q => q.TreatyCode == ExTreatyCode);
            //    if (exReinsEffDatePol.HasValue) query = query.Where(q => q.ReinsEffDatePol == exReinsEffDatePol);
            //    if (!string.IsNullOrEmpty(ExInsuredName)) query = query.Where(q => q.InsuredName == ExInsuredName);
            //    if (!string.IsNullOrEmpty(ExInsuredGenderCode)) query = query.Where(q => q.InsuredGenderCode == ExInsuredGenderCode);
            //    if (exInsuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == exInsuredDateOfBirth);
            //    if (!string.IsNullOrEmpty(ExCedingPlanCode)) query = query.Where(q => q.CedingPlanCode == ExCedingPlanCode);
            //    if (!string.IsNullOrEmpty(ExCedingBenefitTypeCode)) query = query.Where(q => q.CedingBenefitTypeCode == ExCedingBenefitTypeCode);
            //    if (!string.IsNullOrEmpty(ExCedingBenefitRiskCode)) query = query.Where(q => q.CedingBenefitRiskCode == ExCedingBenefitRiskCode);
            //    if (!string.IsNullOrEmpty(ExMlreBenefitCode)) query = query.Where(q => q.MlreBenefitCode == ExMlreBenefitCode);
            //    if (exAar.HasValue) query = query.Where(q => q.Aar == exAar);
            //    if (ExExceptionType.HasValue) query = query.Where(q => q.ExceptionType == ExExceptionType);
            //    if (ExProceedStatus.HasValue) query = query.Where(q => q.ProceedStatus == ExProceedStatus);
            //}

            //var export = new ExportPerLifeAggregationDetailData()
            //{
            //    PrefixFileName = "Exception",
            //    IsException = true
            //};
            //export.HandleTempDirectory();

            //if (query != null)
            //    export.SetQuery(query);

            //export.Process();
            //return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));
        }

        public ActionResult DownloadRetroRiData(
            string downloadToken,
            int id,
            string ReTreatyCode = null,
            int? ReRiskYear = null,
            int? ReRiskMonth = null,
            string ReInsuredGenderCode = null,
            string ReInsuredDateOfBirth = null,
            string RePolicyNumber = null,
            string ReReinsEffDatePol = null,
            string ReAar = null,
            string ReGrossPremium = null,
            string ReNetPremium = null,
            string RePremiumFrequencyCode = null,
            string ReRetroPremFreq = null,
            string ReCedingPlanCode = null,
            string ReCedingBenefitTypeCode = null,
            string ReCedingBenefitRiskCode = null,
            string ReMlreBenefitCode = null,
            string ReRetroBenefitCode = null
        )
        {
            // type 1 = all
            // type 2 = filtered download

            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            dynamic Params = new ExpandoObject();
            Params.Id = id;
            Params.TreatyCode = ReTreatyCode;
            Params.RiskYear = ReRiskYear?.ToString();
            Params.RiskMonth = ReRiskMonth?.ToString();
            Params.InsuredGenderCode = ReInsuredGenderCode;
            Params.InsuredDateOfBirth = ReInsuredDateOfBirth;
            Params.PolicyNumber = RePolicyNumber;
            Params.ReinsEffDatePol = ReReinsEffDatePol;
            Params.Aar = ReAar;
            Params.GrossPremium = ReGrossPremium;
            Params.NetPremium = ReNetPremium;
            Params.PremiumFrequencyCode = RePremiumFrequencyCode;
            Params.RetroPremFreq = ReRetroPremFreq;
            Params.CedingPlanCode = ReCedingPlanCode;
            Params.CedingBenefitTypeCode = ReCedingBenefitTypeCode;
            Params.CedingBenefitRiskCode = ReCedingBenefitRiskCode;
            Params.MlreBenefitCode = ReMlreBenefitCode;
            Params.RetroBenefitCode = ReRetroBenefitCode;

            var export = new GenerateExportData();
            ExportBo exportBo = export.CreateExportPerLifeAggregationRetroRiData(id, AuthUserId, Params);

            if (exportBo.Total <= export.ExportInstantDownloadLimit)
            {
                export.Process(ref exportBo, _db);
                Session.Add("DownloadExport", true);
            }

            return RedirectToAction("Edit", "Export", new { exportBo.Id });

            //_db.Database.CommandTimeout = 0;
            //var query = _db.PerLifeAggregationMonthlyData.Where(q => q.PerLifeAggregationDetailData.PerLifeAggregationDetailTreaty.PerLifeAggregationDetailId == id).Select(PerLifeAggregationMonthlyDataService.Expression());

            //if (type == 2)
            //{
            //    DateTime? reInsuredDateOfBirth = Util.GetParseDateTime(ReInsuredDateOfBirth);
            //    DateTime? reReinsEffDatePol = Util.GetParseDateTime(ReReinsEffDatePol);

            //    double? reAar = Util.StringToDouble(ReAar);
            //    double? reGrossPremium = Util.StringToDouble(ReGrossPremium);
            //    double? reNetPremium = Util.StringToDouble(ReNetPremium);

            //    if (!string.IsNullOrEmpty(ReTreatyCode)) query = query.Where(q => q.TreatyCode == ReTreatyCode);
            //    if (ReRiskYear.HasValue) query = query.Where(q => q.RiskYear == ReRiskYear);
            //    if (ReRiskMonth.HasValue) query = query.Where(q => q.RiskMonth == ReRiskMonth);
            //    if (!string.IsNullOrEmpty(ReInsuredGenderCode)) query = query.Where(q => q.InsuredGenderCode == ReInsuredGenderCode);
            //    if (reInsuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == reInsuredDateOfBirth);
            //    if (!string.IsNullOrEmpty(RePolicyNumber)) query = query.Where(q => q.PolicyNumber == RePolicyNumber);
            //    if (reReinsEffDatePol.HasValue) query = query.Where(q => q.ReinsEffDatePol == reReinsEffDatePol);
            //    if (reAar.HasValue) query = query.Where(q => q.Aar == reAar);
            //    if (reGrossPremium.HasValue) query = query.Where(q => q.GrossPremium == reGrossPremium);
            //    if (reNetPremium.HasValue) query = query.Where(q => q.NetPremium == reNetPremium);
            //    if (!string.IsNullOrEmpty(RePremiumFrequencyCode)) query = query.Where(q => q.PremiumFrequencyCode == RePremiumFrequencyCode);
            //    if (!string.IsNullOrEmpty(ReRetroPremFreq)) query = query.Where(q => q.RetroPremFreq == ReRetroPremFreq);
            //    if (!string.IsNullOrEmpty(ReCedingPlanCode)) query = query.Where(q => q.CedingPlanCode == ReCedingPlanCode);
            //    if (!string.IsNullOrEmpty(ReCedingBenefitTypeCode)) query = query.Where(q => q.CedingBenefitTypeCode == ReCedingBenefitTypeCode);
            //    if (!string.IsNullOrEmpty(ReCedingBenefitRiskCode)) query = query.Where(q => q.CedingBenefitRiskCode == ReCedingBenefitRiskCode);
            //    if (!string.IsNullOrEmpty(ReMlreBenefitCode)) query = query.Where(q => q.MlreBenefitCode == ReMlreBenefitCode);
            //    if (!string.IsNullOrEmpty(ReRetroBenefitCode)) query = query.Where(q => q.RetroBenefitCode == ReRetroBenefitCode);
            //}

            //var export = new ExportPerLifeAggregationMonthlyData()
            //{
            //    PrefixFileName = "RetroRiData",
            //    IsRetroRiData = true
            //};

            //export.HandleTempDirectory();

            //if (query != null)
            //    export.SetQuery(query);

            //export.Process();
            //return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));
        }

        public ActionResult DownloadRetentionPremium(
            string downloadToken,
            int id,
            string RpUniqueKeyPerLife = null,
            string RpPolicyNumber = null,
            string RpInsuredGenderCode = null,
            string RpMlreBenefitCode = null,
            string RpRetroBenefitCode = null,
            string RpTerritoryOfIssueCode = null,
            string RpCurrencyCode = null,
            string RpInsuredTobaccoUse = null,
            int? RpReinsuranceIssueAge = null,
            string RpReinsEffDatePol = null,
            string RpUnderwriterRating = null,
            string RpRetroPremFreq = null,
            string RpSumOfNetPremium = null,
            string RpNetPremium = null,
            string RpSumOfAar = null,
            string RpAar = null,
            string RpRetentionLimit = null,
            string RpDistributedRetentionLimit = null,
            string RpRetroAmount = null,
            string RpDistributedRetroAmount = null,
            string RpAccumulativeRetainAmount = null,
            bool? RpErrors = null
        )
        {
            // type 1 = all
            // type 2 = filtered download

            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            dynamic Params = new ExpandoObject();
            Params.Id = id;
            Params.UniqueKeyPerLife = RpUniqueKeyPerLife;
            Params.PolicyNumber = RpPolicyNumber;
            Params.InsuredGenderCode = RpInsuredGenderCode;
            Params.MlreBenefitCode = RpMlreBenefitCode;
            Params.RetroBenefitCode = RpRetroBenefitCode;
            Params.TerritoryOfIssueCode = RpTerritoryOfIssueCode;
            Params.CurrencyCode = RpCurrencyCode;
            Params.InsuredTobaccoUse = RpInsuredTobaccoUse;
            Params.ReinsuranceIssueAge = RpReinsuranceIssueAge;
            Params.ReinsEffDatePol = RpReinsEffDatePol;
            Params.UnderwriterRating = RpUnderwriterRating;
            Params.RetroPremFreq = RpRetroPremFreq;
            Params.SumOfNetPremium = RpSumOfNetPremium;
            Params.NetPremium = RpNetPremium;
            Params.SumOfAar = RpSumOfAar;
            Params.Aar = RpAar;
            Params.RetentionLimit = RpRetentionLimit;
            Params.DistributedRetentionLimit = RpDistributedRetentionLimit;
            Params.RetroAmount = RpRetroAmount;
            Params.DistributedRetroAmount = RpDistributedRetroAmount;
            Params.AccumulativeRetainAmount = RpAccumulativeRetainAmount;
            Params.Errors = RpErrors;

            var export = new GenerateExportData();
            ExportBo exportBo = export.CreateExportPerLifeAggregationRetentionPremium(id, AuthUserId, Params);

            if (exportBo.Total <= export.ExportInstantDownloadLimit)
            {
                export.Process(ref exportBo, _db);
                Session.Add("DownloadExport", true);
            }

            return RedirectToAction("Edit", "Export", new { exportBo.Id });

            //_db.Database.CommandTimeout = 0;

            //var retroParties = PerLifeAggregationMonthlyRetroDataService.GetDistinctRetroPartyByPerLifeAggregationDetailId(id);

            //var query = _db.PerLifeAggregationMonthlyData.Where(q => q.PerLifeAggregationDetailData.PerLifeAggregationDetailTreaty.PerLifeAggregationDetailId == id).Select(PerLifeAggregationMonthlyDataService.Expression());

            //if (type == 2)
            //{
            //    DateTime? rpReinsEffDatePol = Util.GetParseDateTime(RpReinsEffDatePol);

            //    double? rpUnderwriterRating = Util.StringToDouble(RpUnderwriterRating);
            //    double? rpSumOfNetPremium = Util.StringToDouble(RpSumOfNetPremium);
            //    double? rpNetPremium = Util.StringToDouble(RpNetPremium);
            //    double? rpSumOfAar = Util.StringToDouble(RpSumOfAar);
            //    double? rpAar = Util.StringToDouble(RpAar);
            //    double? rpRetentionLimit = Util.StringToDouble(RpRetentionLimit);
            //    double? rpDistributedRetentionLimit = Util.StringToDouble(RpDistributedRetentionLimit);
            //    double? rpRetroAmount = Util.StringToDouble(RpRetroAmount);
            //    double? rpDistributedRetroAmount = Util.StringToDouble(RpDistributedRetroAmount);
            //    double? rpAccumulativeRetainAmount = Util.StringToDouble(RpAccumulativeRetainAmount);

            //    if (!string.IsNullOrEmpty(RpUniqueKeyPerLife)) query = query.Where(q => RpUniqueKeyPerLife.Contains(q.UniqueKeyPerLife));
            //    if (!string.IsNullOrEmpty(RpPolicyNumber)) query = query.Where(q => q.PolicyNumber == RpPolicyNumber);
            //    if (!string.IsNullOrEmpty(RpInsuredGenderCode)) query = query.Where(q => q.InsuredGenderCode == RpInsuredGenderCode);
            //    if (!string.IsNullOrEmpty(RpMlreBenefitCode)) query = query.Where(q => q.MlreBenefitCode == RpMlreBenefitCode);
            //    if (!string.IsNullOrEmpty(RpRetroBenefitCode)) query = query.Where(q => q.RetroBenefitCode == RpRetroBenefitCode);
            //    if (!string.IsNullOrEmpty(RpTerritoryOfIssueCode)) query = query.Where(q => q.TerritoryOfIssueCode == RpTerritoryOfIssueCode);
            //    if (!string.IsNullOrEmpty(RpCurrencyCode)) query = query.Where(q => q.CurrencyCode == RpCurrencyCode);
            //    if (!string.IsNullOrEmpty(RpInsuredTobaccoUse)) query = query.Where(q => q.InsuredTobaccoUse == RpInsuredTobaccoUse);
            //    if (RpReinsuranceIssueAge.HasValue) query = query.Where(q => q.ReinsuranceIssueAge == RpReinsuranceIssueAge);
            //    if (rpReinsEffDatePol.HasValue) query = query.Where(q => q.ReinsEffDatePol == rpReinsEffDatePol);
            //    if (rpUnderwriterRating.HasValue) query = query.Where(q => q.UnderwriterRating == rpUnderwriterRating);
            //    if (!string.IsNullOrEmpty(RpRetroPremFreq)) query = query.Where(q => q.RetroPremFreq == RpRetroPremFreq);
            //    if (rpSumOfNetPremium.HasValue) query = query.Where(q => q.SumOfNetPremium == rpSumOfNetPremium);
            //    if (rpNetPremium.HasValue) query = query.Where(q => q.NetPremium == rpNetPremium);
            //    if (rpSumOfAar.HasValue) query = query.Where(q => q.SumOfAar == rpSumOfAar);
            //    if (rpAar.HasValue) query = query.Where(q => q.Aar == rpAar);
            //    if (rpRetentionLimit.HasValue) query = query.Where(q => q.RetentionLimit == rpRetentionLimit);
            //    if (rpDistributedRetentionLimit.HasValue) query = query.Where(q => q.DistributedRetentionLimit == rpDistributedRetentionLimit);
            //    if (rpRetroAmount.HasValue) query = query.Where(q => q.RetroAmount == rpRetroAmount);
            //    if (rpDistributedRetroAmount.HasValue) query = query.Where(q => q.DistributedRetroAmount == rpDistributedRetroAmount);
            //    if (rpAccumulativeRetainAmount.HasValue) query = query.Where(q => q.AccumulativeRetainAmount == rpAccumulativeRetainAmount);
            //    if (RpErrors.HasValue && RpErrors.Value == true) query = query.Where(q => q.Errors != null);
            //    if (RpErrors.HasValue && RpErrors.Value == false) query = query.Where(q => q.Errors == null);
            //}

            //var export = new ExportPerLifeAggregationMonthlyData()
            //{
            //    PrefixFileName = "RetentionPremium",
            //    IsRetentionPremium = true,
            //    RetroParties = retroParties
            //};

            //export.HandleTempDirectory();

            //if (query != null)
            //    export.SetQuery(query);

            //export.Process();
            //return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));
        }

        public ActionResult DownloadExcludedRecord(
            string downloadToken,
            int id,
            string riskQuarter
        )
        {
            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            _db.Database.CommandTimeout = 0;

            var formattedRiskQuarter = riskQuarter.Replace(" ", "");

            dynamic Params = new ExpandoObject();
            Params.RiskQuarter = formattedRiskQuarter;

            var export = new GenerateExportData();
            ExportBo exportBo = export.CreateExportPerLifeAggregationSummaryExcludedRecord(id, AuthUserId, Params);

            if (exportBo.Total <= export.ExportInstantDownloadLimit)
            {
                export.Process(ref exportBo, _db);
                Session.Add("DownloadExport", true);
            }

            return RedirectToAction("Edit", "Export", new { exportBo.Id });
        }
        
        public ActionResult DownloadRetroRecord(
            string downloadToken,
            int id,
            string riskQuarter
        )
        {
            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            _db.Database.CommandTimeout = 0;

            var formattedRiskQuarter = riskQuarter.Replace(" ", "");

            dynamic Params = new ExpandoObject();
            Params.RiskQuarter = formattedRiskQuarter;

            var export = new GenerateExportData();
            ExportBo exportBo = export.CreateExportPerLifeAggregationSummaryRetro(id, AuthUserId, Params);

            if (exportBo.Total <= export.ExportInstantDownloadLimit)
            {
                export.Process(ref exportBo, _db);
                Session.Add("DownloadExport", true);
            }

            return RedirectToAction("Edit", "Export", new { exportBo.Id });
        }

        public void LoadPage(PerLifeAggregationDetailViewModel model)
        {
            DropDownMonth();
            DropDownTreatyCode(codeAsValue: true);
            DropDownBenefit(codeAsValue: true);
            DropDownTransactionTypeCode(codeAsValue: true);
            DropDownInsuredGenderCode(codeAsValue: true);
            DropDownCurrencyCode(codeAsValue: true);
            DropDownTerritoryOfIssueCode(codeAsValue: true);
            DropDownCedingBenefitTypeCode(codeAsValue: true);
            DropDownTransactionTypeCode(codeAsValue: true);
            DropDownRiDataRecordType();
            DropDownExceptionType();
            DropDownFlagCode();
            DropDownProceedStatus();
            DropDownPremiumFrequencyCode(codeAsValue: true);
            DropDownRetroBenefitCode(codeAsValue: true);
            DropDownInsuredTobaccoUse(codeAsValue: true);
            DropDownYesNoWithSelect();
            DropDownReinsBasisCode(codeAsValue: true);

            if (model.Id == 0)
            {

            }
            else
            {
                IsEnableProcess(model);
                IsEnableValidate(model);
                IsEnableAggregate(model);
                IsEnableFinalise(model);
            }

            SetViewBagMessage();
        }

        public void IsEnableValidate(PerLifeAggregationDetailViewModel model)
        {
            var isEnableValidate = false;
            if (model.ActiveTab == PerLifeAggregationDetailBo.ActiveTabRiData &&
                model.Status != PerLifeAggregationDetailBo.StatusSubmitForValidation &&
                model.Status != PerLifeAggregationDetailBo.StatusValidating &&
                model.Status != PerLifeAggregationDetailBo.StatusSubmitForProcessing &&
                model.Status != PerLifeAggregationDetailBo.StatusProcessing &&
                model.Status != PerLifeAggregationDetailBo.StatusSubmitForAggregation &&
                model.Status != PerLifeAggregationDetailBo.StatusAggregating &&
                model.Status != PerLifeAggregationDetailBo.StatusFinalised &&
                model.PerLifeAggregationBo != null &&
                model.PerLifeAggregationBo.Status != PerLifeAggregationBo.StatusPending &&
                model.PerLifeAggregationBo.Status != PerLifeAggregationBo.StatusSubmitForProcessing &&
                //model.PerLifeAggregationBo.Status != PerLifeAggregationBo.StatusProcessing &&
                model.PerLifeAggregationBo.Status != PerLifeAggregationBo.StatusFinalised)
                isEnableValidate = true;

            ViewBag.IsEnableValidate = isEnableValidate;
        }

        public void IsEnableProcess(PerLifeAggregationDetailViewModel model)
        {
            var isEnableProcess = false;
            if (model.ActiveTab == PerLifeAggregationDetailBo.ActiveTabRiData &&
                (model.Status == PerLifeAggregationDetailBo.StatusValidationSuccess ||
                model.Status == PerLifeAggregationDetailBo.StatusValidationFailed) &&
                model.PerLifeAggregationBo != null &&
                model.PerLifeAggregationBo.Status != PerLifeAggregationBo.StatusPending &&
                model.PerLifeAggregationBo.Status != PerLifeAggregationBo.StatusSubmitForProcessing &&
                //model.PerLifeAggregationBo.Status != PerLifeAggregationBo.StatusProcessing &&
                model.PerLifeAggregationBo.Status != PerLifeAggregationBo.StatusFinalised)
                isEnableProcess = true;

            ViewBag.IsEnableProcess = isEnableProcess;
        }

        public void IsEnableAggregate(PerLifeAggregationDetailViewModel model)
        {
            var isEnableAggregate = false;
            if (model.ActiveTab == PerLifeAggregationDetailBo.ActiveTabRiData &&
                (model.Status == PerLifeAggregationDetailBo.StatusProcessSuccess ||
                model.Status == PerLifeAggregationDetailBo.StatusProcessFailed) &&
                model.PerLifeAggregationBo != null &&
                model.PerLifeAggregationBo.Status != PerLifeAggregationBo.StatusPending &&
                model.PerLifeAggregationBo.Status != PerLifeAggregationBo.StatusSubmitForProcessing &&
                //model.PerLifeAggregationBo.Status != PerLifeAggregationBo.StatusProcessing &&
                model.PerLifeAggregationBo.Status != PerLifeAggregationBo.StatusFinalised)
                isEnableAggregate = true;

            ViewBag.IsEnableAggregate = isEnableAggregate;
        }

        public void IsEnableFinalise(PerLifeAggregationDetailViewModel model)
        {
            var isEnableFinalise = false;
            //if (model.ActiveTab == PerLifeAggregationDetailBo.ActiveTabRiData &&
            //    model.Status == PerLifeAggregationDetailBo.StatusAggregationSuccess &&
            //    model.PerLifeAggregationBo != null &&
            //    model.PerLifeAggregationBo.Status == PerLifeAggregationBo.StatusSuccess)
            if (model.ActiveTab == PerLifeAggregationDetailBo.ActiveTabRiData &&
                model.Status == PerLifeAggregationDetailBo.StatusAggregationSuccess)
                isEnableFinalise = true;

            ViewBag.IsEnableFinalise = isEnableFinalise;
        }

        [Auth(Controller = Controller, Power = "U")]
        public ActionResult ReleaseException(int id, PerLifeAggregationDetailViewModel model, FormCollection form)
        {
            if (string.IsNullOrEmpty(model.SelectedExceptionIds))
            {
                SetErrorSessionMsg("No Exception Selected");
                return RedirectToAction("Exception", new { id });
            }

            int success = 0;
            int existing = 0;
            int error = 0;
            var ids = model.SelectedExceptionIds.Split(',').Select(q => int.Parse(q.Trim())).ToList();

            var trail = GetNewTrailObject();

            foreach (int i in ids)
            {
                var bo = PerLifeAggregationDetailDataService.Find(i);
                if (bo == null || bo.RiDataWarehouseHistoryBo == null || bo.PerLifeAggregationDetailTreatyBo.PerLifeAggregationDetailId != id)
                    continue;

                if (ValidDuplicationListService.IsExistsByParam(
                    bo.RiDataWarehouseHistoryBo.TreatyCode,
                    bo.RiDataWarehouseHistoryBo.CedingPlanCode,
                    bo.RiDataWarehouseHistoryBo.InsuredName,
                    bo.RiDataWarehouseHistoryBo.InsuredDateOfBirth,
                    bo.RiDataWarehouseHistoryBo.InsuredGenderCode,
                    bo.RiDataWarehouseHistoryBo.MlreBenefitCode,
                    bo.RiDataWarehouseHistoryBo.CedingBenefitRiskCode,
                    bo.RiDataWarehouseHistoryBo.CedingBenefitTypeCode,
                    bo.RiDataWarehouseHistoryBo.ReinsEffDatePol,
                    bo.RiDataWarehouseHistoryBo.FundsAccountingTypeCode,
                    bo.RiDataWarehouseHistoryBo.ReinsBasisCode,
                    bo.RiDataWarehouseHistoryBo.TransactionTypeCode
                ))
                {
                    existing++;
                    continue;
                }

                trail = GetNewTrailObject();

                var treatyCodeBo = TreatyCodeService.FindByCode(bo.RiDataWarehouseHistoryBo.TreatyCode);
                var insuredGenderCodeBo = PickListDetailService.FindByPickListIdCode(PickListBo.InsuredGenderCode, bo.RiDataWarehouseHistoryBo.InsuredGenderCode);
                var benefitCodeBo = BenefitService.FindByCode(bo.RiDataWarehouseHistoryBo.MlreBenefitCode);
                var fundsAccountingTypeCodeBo = PickListDetailService.FindByPickListIdCode(PickListBo.FundsAccountingType, bo.RiDataWarehouseHistoryBo.FundsAccountingTypeCode);
                var reinsBasisCodeBo = PickListDetailService.FindByPickListIdCode(PickListBo.ReinsBasisCode, bo.RiDataWarehouseHistoryBo.ReinsBasisCode);
                var transactionTypeCodeBo = PickListDetailService.FindByPickListIdCode(PickListBo.TransactionTypeCode, bo.RiDataWarehouseHistoryBo.TransactionTypeCode);

                if (!string.IsNullOrEmpty(bo.RiDataWarehouseHistoryBo.TreatyCode) && treatyCodeBo == null &&
                    !string.IsNullOrEmpty(bo.RiDataWarehouseHistoryBo.MlreBenefitCode) && benefitCodeBo == null &&
                    !string.IsNullOrEmpty(bo.RiDataWarehouseHistoryBo.FundsAccountingTypeCode) && fundsAccountingTypeCodeBo == null &&
                    !string.IsNullOrEmpty(bo.RiDataWarehouseHistoryBo.ReinsBasisCode) && reinsBasisCodeBo == null &&
                    !string.IsNullOrEmpty(bo.RiDataWarehouseHistoryBo.TransactionTypeCode) && transactionTypeCodeBo == null
                )
                {
                    error++;
                    continue;
                }

                var validDuplicationListBo = new ValidDuplicationListBo()
                {
                    TreatyCodeId = treatyCodeBo?.Id,
                    CedantPlanCode = bo.RiDataWarehouseHistoryBo.CedingPlanCode,
                    InsuredName = bo.RiDataWarehouseHistoryBo.InsuredName,
                    InsuredDateOfBirth = bo.RiDataWarehouseHistoryBo.InsuredDateOfBirth,
                    InsuredGenderCodePickListDetailId = insuredGenderCodeBo?.Id,
                    MLReBenefitCodeId = benefitCodeBo?.Id,
                    CedingBenefitRiskCode = bo.RiDataWarehouseHistoryBo.CedingBenefitRiskCode,
                    CedingBenefitTypeCode = bo.RiDataWarehouseHistoryBo.CedingBenefitTypeCode,
                    ReinsuranceEffectiveDate = bo.RiDataWarehouseHistoryBo.ReinsEffDatePol,
                    FundsAccountingTypePickListDetailId = fundsAccountingTypeCodeBo?.Id,
                    ReinsBasisCodePickListDetailId = reinsBasisCodeBo?.Id,
                    TransactionTypePickListDetailId = transactionTypeCodeBo?.Id,

                    CreatedById = AuthUserId,
                    UpdatedById = AuthUserId,
                };

                Result = ValidDuplicationListService.Create(ref validDuplicationListBo, ref trail);
                if (Result.Valid)
                {
                    success++;
                    CreateTrail(
                        bo.Id,
                        "Create Valid Duplication List"
                    );
                }
            }

            SetSuccessSessionMsg(string.Format("Total Records Released: {0}", success));
            SetErrorSessionMsg(string.Format("Total Existing Record: {0}, Total Record not found data in maintenance: {1}", existing, error));
            return RedirectToAction("Exception", new { id });
        }

        [Auth(Controller = Controller, Power = "U")]
        public ActionResult PutOnHoldException(int id, PerLifeAggregationDetailViewModel model, FormCollection form)
        {
            if (string.IsNullOrEmpty(model.SelectedExceptionIds))
            {
                SetErrorSessionMsg("No Exception Selected");
                return RedirectToAction("Exception", new { id });
            }

            int success = 0;
            var ids = model.SelectedExceptionIds.Split(',').Select(q => int.Parse(q.Trim())).ToList();

            var trail = GetNewTrailObject();

            foreach (int i in ids)
            {
                var bo = PerLifeAggregationDetailDataService.Find(i);
                if (bo == null || bo.PerLifeAggregationDetailTreatyBo.PerLifeAggregationDetailId != id || bo.ProceedStatus == PerLifeAggregationDetailDataBo.ProceedStatusPutOnHold)
                    continue;

                trail = GetNewTrailObject();

                bo.ProceedStatus = PerLifeAggregationDetailDataBo.ProceedStatusPutOnHold;
                Result = PerLifeAggregationDetailDataService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    success++;
                    CreateTrail(
                        bo.Id,
                        "Update Per Life Aggregation Detail Data Status"
                    );
                }
            }

            SetSuccessSessionMsg(string.Format("Total Records Put On Hold: {0}", success));
            return RedirectToAction("Exception", new { id });
        }

        public JsonResult ConflictCheckLookup(int perLifeAggregationDetailDataId)
        {
            List<PerLifeAggregationDetailDataBo> perLifeAggregationDetailDataBos = new List<PerLifeAggregationDetailDataBo> { };
            var perLifeAggregationDetailDataBo = PerLifeAggregationDetailDataService.Find(perLifeAggregationDetailDataId);

            if (perLifeAggregationDetailDataBo == null)
                return Json(new { PerLifeAggregationDetailDataBos = perLifeAggregationDetailDataBos });

            var bos = PerLifeAggregationDetailDataService.GetByConflictCheckParams
                (
                    perLifeAggregationDetailDataBo.InsuredName,
                    perLifeAggregationDetailDataBo.InsuredDateOfBirth,
                    perLifeAggregationDetailDataBo.MlreBenefitCode,
                    perLifeAggregationDetailDataBo.TransactionTypeCode,
                    perLifeAggregationDetailDataBo.InsuredGenderCode,
                    perLifeAggregationDetailDataBo.TerritoryOfIssueCode
                );

            if (!bos.IsNullOrEmpty() && bos.Count() > 0)
                perLifeAggregationDetailDataBos = bos.ToList();

            return Json(new { PerLifeAggregationDetailDataBos = perLifeAggregationDetailDataBos });
        }

        public JsonResult DuplicationCheckLookup(int perLifeAggregationDetailDataId)
        {
            List<PerLifeAggregationDetailDataBo> perLifeAggregationDetailDataBos = new List<PerLifeAggregationDetailDataBo> { };
            var perLifeAggregationDetailDataBo = PerLifeAggregationDetailDataService.Find(perLifeAggregationDetailDataId);

            if (perLifeAggregationDetailDataBo == null)
                return Json(new { PerLifeAggregationDetailDataBos = perLifeAggregationDetailDataBos });

            var bos = PerLifeAggregationDetailDataService.GetByDuplicationCheckParams
                (
                    perLifeAggregationDetailDataBo.PerLifeAggregationDetailTreatyBo.PerLifeAggregationDetailId,
                    perLifeAggregationDetailDataBo.InsuredName,
                    perLifeAggregationDetailDataBo.PolicyNumber,
                    perLifeAggregationDetailDataBo.MlreBenefitCode,
                    perLifeAggregationDetailDataBo.InsuredDateOfBirth,
                    perLifeAggregationDetailDataBo.TransactionTypeCode,
                    perLifeAggregationDetailDataBo.CedingPlanCode,
                    perLifeAggregationDetailDataBo.TreatyCode,
                    perLifeAggregationDetailDataBo.InsuredGenderCode,
                    perLifeAggregationDetailDataBo.EffectiveDate,
                    perLifeAggregationDetailDataBo.ReinsEffDatePol
                );

            if (!bos.IsNullOrEmpty() && bos.Count() > 0)
                perLifeAggregationDetailDataBos = bos.ToList();

            return Json(new { PerLifeAggregationDetailDataBos = perLifeAggregationDetailDataBos });
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

        public void DropDownFlagCode()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= PerLifeAggregationDetailDataBo.FlagCodeMax; i++)
            {
                items.Add(new SelectListItem { Text = PerLifeAggregationDetailDataBo.GetFlagCodeName(i), Value = i.ToString() });
            }
            ViewBag.DropDownFlagCodes = items;
        }

        public void DropDownProceedStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= PerLifeAggregationDetailDataBo.ProceedStatusMax; i++)
            {
                items.Add(new SelectListItem { Text = PerLifeAggregationDetailDataBo.GetProceedStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownProceedStatuses = items;
        }
    }
}