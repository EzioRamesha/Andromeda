using BusinessObject;
using BusinessObject.RiDatas;
using ConsoleApp.Commands.ProcessDatas;
using DataAccess.Entities.RiDatas;
using Newtonsoft.Json;
using PagedList;
using Services;
using Services.RiDatas;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class RiDataSearchController : BaseController
    {
        public const string Controller = "RiDataSearch";

        // GET: RiDataSearch
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string TreatyCode,
            string SoaQuarter,
            string RiskPeriodStartDate,
            string RiskPeriodEndDate,
            string InsuredName,
            string PolicyNumber,
            string InsuredDateOfBirth,
            string CedingPlanCode,
            string BenefitCode,
            string TreatyCodeFilter,
            string ReinsBasisCode,
            string FundsAccountingTypeCode,
            string PremiumFrequencyCode,
            int? ReportPeriodMonth,
            int? ReportPeriodYear,
            int? RiskPeriodMonth,
            int? RiskPeriodYear,
            string TransactionTypeCode,
            string PolicyNumberFilter,
            string IssueDatePol,
            string IssueDateBen,
            string ReinsEffDatePol,
            string ReinsEffDateBen,
            string CedingPlanCodeFilter,
            string CedingBenefitTypeCode,
            string CedingBenefitRiskCode,
            string BenefitCodeFilter,
            string OriSumAssured,
            string CurrSumAssured,
            string AmountCededB4MlreShare,
            string RetentionAmount,
            string AarOri,
            string Aar,
            string InsuredNameFilter,
            string InsuredGenderCode,
            string InsuredTobaccoUse,
            string InsuredDateOfBirthFilter,
            string InsuredOccupationCode,
            int? InsuredAttainedAge,
            string SortOrder,
            int? Page
        )
        {
            DateTime? riskPeriodStartDate = Util.GetParseDateTime(RiskPeriodStartDate);
            DateTime? riskPeriodEndDate = Util.GetParseDateTime(RiskPeriodEndDate);
            DateTime? insuredDateOfBirth = Util.GetParseDateTime(InsuredDateOfBirth);
            DateTime? issueDatePol = Util.GetParseDateTime(IssueDatePol);
            DateTime? issueDateBen = Util.GetParseDateTime(IssueDateBen);
            DateTime? reinsEffDatePol = Util.GetParseDateTime(ReinsEffDatePol);
            DateTime? reinsEffDateBen = Util.GetParseDateTime(ReinsEffDateBen);
            DateTime? insuredDateOfBirthFilter = Util.GetParseDateTime(InsuredDateOfBirthFilter);

            double? oriSumAssured = Util.StringToDouble(OriSumAssured);
            double? currSumAssured = Util.StringToDouble(CurrSumAssured);
            double? amountCededB4MlreShare = Util.StringToDouble(AmountCededB4MlreShare);
            double? retentionAmount = Util.StringToDouble(RetentionAmount);
            double? aarOri = Util.StringToDouble(AarOri);
            double? aar = Util.StringToDouble(Aar);

            string[] treatyCodes = Util.ToArraySplitTrim(TreatyCode);
            string[] benefitCodes = Util.ToArraySplitTrim(BenefitCode);

            List<string> errors = new List<string> { };

            if (!string.IsNullOrEmpty(RiskPeriodStartDate) && !riskPeriodStartDate.HasValue)
                errors.Add(string.Format(MessageBag.InvalidDateFormatWithName, "Risk Period Start Date"));

            if (!string.IsNullOrEmpty(RiskPeriodEndDate) && !riskPeriodEndDate.HasValue)
                errors.Add(string.Format(MessageBag.InvalidDateFormatWithName, "Risk Period End Date"));

            if (riskPeriodStartDate.HasValue && riskPeriodEndDate.HasValue && riskPeriodEndDate < riskPeriodStartDate)
                errors.Add(string.Format(MessageBag.GreaterThan, "Risk Period End Date", "Risk Period Start Date"));

            if (!string.IsNullOrEmpty(InsuredDateOfBirth) && !insuredDateOfBirth.HasValue)
                errors.Add(string.Format(MessageBag.InvalidDateFormatWithName, "Insured Date of Birth"));

            if (!string.IsNullOrEmpty(IssueDatePol) && !issueDatePol.HasValue)
                errors.Add(string.Format(MessageBag.InvalidDateFormatWithName, "Issue Date Policy"));

            if (!string.IsNullOrEmpty(IssueDateBen) && !issueDateBen.HasValue)
                errors.Add(string.Format(MessageBag.InvalidDateFormatWithName, "Issue Date Benefit"));

            if (!string.IsNullOrEmpty(ReinsEffDatePol) && !reinsEffDatePol.HasValue)
                errors.Add(string.Format(MessageBag.InvalidDateFormatWithName, "Reinsurance Effective Date Policy"));

            if (!string.IsNullOrEmpty(ReinsEffDateBen) && !reinsEffDateBen.HasValue)
                errors.Add(string.Format(MessageBag.InvalidDateFormatWithName, "Reinsurance Effective Date Benefit"));

            if (!string.IsNullOrEmpty(InsuredDateOfBirthFilter) && !insuredDateOfBirthFilter.HasValue)
                errors.Add(string.Format(MessageBag.InvalidDateFormatWithName, "Insured Date of Birth at filter"));

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["TreatyCode"] = TreatyCode,
                ["SoaQuarter"] = SoaQuarter,
                ["RiskPeriodStartDate"] = RiskPeriodStartDate,
                ["RiskPeriodEndDate"] = RiskPeriodEndDate,
                ["InsuredName"] = InsuredName,
                ["PolicyNumber"] = PolicyNumber,
                ["InsuredDateOfBirth"] = InsuredDateOfBirth,
                ["CedingPlanCode"] = CedingPlanCode,
                ["BenefitCode"] = BenefitCode,
                ["TreatyCodeFilter"] = TreatyCodeFilter,
                ["ReinsBasisCode"] = ReinsBasisCode,
                ["FundsAccountingTypeCode"] = FundsAccountingTypeCode,
                ["PremiumFrequencyCode"] = PremiumFrequencyCode,
                ["ReportPeriodMonth"] = ReportPeriodMonth,
                ["ReportPeriodYear"] = ReportPeriodYear,
                ["RiskPeriodMonth"] = RiskPeriodMonth,
                ["RiskPeriodYear"] = RiskPeriodYear,
                ["TransactionTypeCode"] = TransactionTypeCode,
                ["PolicyNumberFilter"] = PolicyNumberFilter,
                ["IssueDatePol"] = IssueDatePol,
                ["IssueDateBen"] = IssueDateBen,
                ["ReinsEffDatePol"] = ReinsEffDatePol,
                ["ReinsEffDateBen"] = ReinsEffDateBen,
                ["CedingPlanCodeFilter"] = CedingPlanCodeFilter,
                ["CedingBenefitTypeCode"] = CedingBenefitTypeCode,
                ["CedingBenefitRiskCode"] = CedingBenefitRiskCode,
                ["BenefitCodeFilter"] = BenefitCodeFilter,
                ["OriSumAssured"] = OriSumAssured,
                ["CurrSumAssured"] = CurrSumAssured,
                ["AmountCededB4MlreShare"] = AmountCededB4MlreShare,
                ["RetentionAmount"] = RetentionAmount,
                ["AarOri"] = AarOri,
                ["Aar"] = Aar,
                ["InsuredNameFilter"] = InsuredNameFilter,
                ["InsuredGenderCode"] = InsuredGenderCode,
                ["InsuredTobaccoUse"] = InsuredTobaccoUse,
                ["InsuredDateOfBirthFilter"] = InsuredDateOfBirthFilter,
                ["InsuredOccupationCode"] = InsuredOccupationCode,
                ["InsuredAttainedAge"] = InsuredAttainedAge,
                ["SortOrder"] = SortOrder,
            };

            ViewBag.SortOrder = SortOrder;
            ViewBag.SortTreatyCodeFilter = GetSortParam("TreatyCodeFilter");
            ViewBag.SortReinsBasisCode = GetSortParam("ReinsBasisCode");
            ViewBag.SortFundsAccountingTypeCode = GetSortParam("FundsAccountingTypeCode");
            ViewBag.SortPremiumFrequencyCode = GetSortParam("PremiumFrequencyCode");
            ViewBag.SortReportPeriodMonth = GetSortParam("ReportPeriodMonth");
            ViewBag.SortReportPeriodYear = GetSortParam("ReportPeriodYear");
            ViewBag.SortRiskPeriodMonth = GetSortParam("RiskPeriodMonth");
            ViewBag.SortRiskPeriodYear = GetSortParam("RiskPeriodYear");
            ViewBag.SortTransactionTypeCode = GetSortParam("TransactionTypeCode");
            ViewBag.SortPolicyNumberFilter = GetSortParam("PolicyNumberFilter");
            ViewBag.SortIssueDatePol = GetSortParam("IssueDatePol");
            ViewBag.SortIssueDateBen = GetSortParam("IssueDateBen");
            ViewBag.SortReinsEffDatePol = GetSortParam("ReinsEffDatePol");
            ViewBag.SortReinsEffDateBen = GetSortParam("ReinsEffDateBen");
            ViewBag.SortCedingPlanCodeFilter = GetSortParam("CedingPlanCodeFilter");
            ViewBag.SortCedingBenefitTypeCode = GetSortParam("CedingBenefitTypeCode");
            ViewBag.SortCedingBenefitRiskCode = GetSortParam("CedingBenefitRiskCode");
            ViewBag.SortBenefitCodeFilter = GetSortParam("BenefitCodeFilter");
            ViewBag.SortOriSumAssured = GetSortParam("OriSumAssured");
            ViewBag.SortCurrSumAssured = GetSortParam("CurrSumAssured");
            ViewBag.SortAmountCededB4MlreShare = GetSortParam("AmountCededB4MlreShare");
            ViewBag.SortRetentionAmount = GetSortParam("RetentionAmount");
            ViewBag.SortAarOri = GetSortParam("AarOri");
            ViewBag.SortAar = GetSortParam("Aar");
            ViewBag.SortInsuredNameFilter = GetSortParam("InsuredNameFilter");
            ViewBag.SortInsuredGenderCode = GetSortParam("InsuredGenderCode");
            ViewBag.SortInsuredTobaccoUse = GetSortParam("InsuredTobaccoUse");
            ViewBag.SortInsuredDateOfBirthFilter = GetSortParam("InsuredDateOfBirthFilter");
            ViewBag.SortInsuredOccupationCode = GetSortParam("InsuredOccupationCode");
            ViewBag.SortInsuredAttainedAge = GetSortParam("InsuredAttainedAge");

            _db.Database.CommandTimeout = 0;

            if (string.IsNullOrEmpty(TreatyCode) &&
                string.IsNullOrEmpty(SoaQuarter) &&
                string.IsNullOrEmpty(RiskPeriodStartDate) &&
                string.IsNullOrEmpty(RiskPeriodEndDate) &&
                string.IsNullOrEmpty(InsuredName) &&
                string.IsNullOrEmpty(PolicyNumber) &&
                string.IsNullOrEmpty(InsuredDateOfBirth) &&
                string.IsNullOrEmpty(CedingPlanCode) &&
                string.IsNullOrEmpty(BenefitCode) ||
                errors.Count() > 0)
            {
                var query = _db.RiData.AsNoTracking().Where(q => q.Id == 0).Select(RiDataSearchViewModel.Expression());

                if (errors.Count() > 0)
                    SetErrorSessionMsgArr(errors);

                LoadPage();
                ViewBag.DisableDownload = true;
                ViewBag.Total = query.Count();
                return View(query.ToPagedList(Page ?? 1, PageSize));
            }
            else
            {
                var query = _db.RiData.AsNoTracking().Where(q => q.FinaliseStatus == RiDataBo.FinaliseStatusSuccess).Select(RiDataSearchViewModel.Expression());
                if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => treatyCodes.Contains(q.TreatyCode));
                if (!string.IsNullOrEmpty(SoaQuarter))
                {
                    string[] quarterStr = SoaQuarter.Split(' ');
                    List<int> months = new List<int> { };

                    switch (quarterStr[1])
                    {
                        case "Q1": months = new List<int> { 1, 2, 3 }; break;
                        case "Q2": months = new List<int> { 4, 5, 6 }; break;
                        case "Q3": months = new List<int> { 7, 8, 9 }; break;
                        case "Q4": months = new List<int> { 10, 11, 12 }; break;
                    }

                    int quarterYear = Convert.ToInt32(quarterStr[0]);
                    query = query.Where(q => months.Contains(q.ReportPeriodMonth.Value) && q.ReportPeriodYear == quarterYear);
                }
                if (riskPeriodStartDate.HasValue) query = query.Where(q => q.RiskPeriodStartDate >= riskPeriodStartDate);
                if (riskPeriodEndDate.HasValue) query = query.Where(q => q.RiskPeriodEndDate <= riskPeriodEndDate);
                if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => q.InsuredName == InsuredName);
                if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber == PolicyNumber);
                if (insuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == insuredDateOfBirth);
                if (!string.IsNullOrEmpty(CedingPlanCode)) query = query.Where(q => q.CedingPlanCode == CedingPlanCode);
                if (!string.IsNullOrEmpty(BenefitCode)) query = query.Where(q => benefitCodes.Contains(q.BenefitCode));
                if (!string.IsNullOrEmpty(TreatyCodeFilter)) query = query.Where(q => q.TreatyCode == TreatyCodeFilter);
                if (!string.IsNullOrEmpty(ReinsBasisCode)) query = query.Where(q => q.ReinsBasisCode == ReinsBasisCode);
                if (!string.IsNullOrEmpty(FundsAccountingTypeCode)) query = query.Where(q => q.FundsAccountingTypeCode == FundsAccountingTypeCode);
                if (!string.IsNullOrEmpty(PremiumFrequencyCode)) query = query.Where(q => q.PremiumFrequencyCode == PremiumFrequencyCode);
                if (ReportPeriodMonth.HasValue) query = query.Where(q => q.ReportPeriodMonth == ReportPeriodMonth);
                if (ReportPeriodYear.HasValue) query = query.Where(q => q.ReportPeriodYear == ReportPeriodYear);
                if (RiskPeriodMonth.HasValue) query = query.Where(q => q.RiskPeriodMonth == RiskPeriodMonth);
                if (RiskPeriodYear.HasValue) query = query.Where(q => q.RiskPeriodYear == RiskPeriodYear);
                if (!string.IsNullOrEmpty(TransactionTypeCode)) query = query.Where(q => q.TransactionTypeCode == TransactionTypeCode);
                if (!string.IsNullOrEmpty(PolicyNumberFilter)) query = query.Where(q => q.PolicyNumber == PolicyNumberFilter);
                if (issueDatePol.HasValue) query = query.Where(q => q.IssueDatePol == issueDatePol);
                if (issueDateBen.HasValue) query = query.Where(q => q.IssueDateBen == issueDateBen);
                if (reinsEffDatePol.HasValue) query = query.Where(q => q.ReinsEffDatePol == reinsEffDatePol);
                if (reinsEffDateBen.HasValue) query = query.Where(q => q.ReinsEffDateBen == reinsEffDateBen);
                if (!string.IsNullOrEmpty(CedingPlanCodeFilter)) query = query.Where(q => q.CedingPlanCode == CedingPlanCodeFilter);
                if (!string.IsNullOrEmpty(CedingBenefitTypeCode)) query = query.Where(q => q.CedingBenefitTypeCode == CedingBenefitTypeCode);
                if (!string.IsNullOrEmpty(CedingBenefitRiskCode)) query = query.Where(q => q.CedingBenefitRiskCode == CedingBenefitRiskCode);
                if (!string.IsNullOrEmpty(BenefitCodeFilter)) query = query.Where(q => q.BenefitCode == BenefitCodeFilter);
                if (oriSumAssured.HasValue) query = query.Where(q => q.OriSumAssured == oriSumAssured);
                if (currSumAssured.HasValue) query = query.Where(q => q.CurrSumAssured == currSumAssured);
                if (amountCededB4MlreShare.HasValue) query = query.Where(q => q.AmountCededB4MlreShare == amountCededB4MlreShare);
                if (retentionAmount.HasValue) query = query.Where(q => q.RetentionAmount == retentionAmount);
                if (aarOri.HasValue) query = query.Where(q => q.AarOri == aarOri);
                if (aar.HasValue) query = query.Where(q => q.Aar == aar);
                if (!string.IsNullOrEmpty(InsuredNameFilter)) query = query.Where(q => q.InsuredName == InsuredNameFilter);
                if (!string.IsNullOrEmpty(InsuredGenderCode)) query = query.Where(q => q.InsuredGenderCode == InsuredGenderCode);
                if (!string.IsNullOrEmpty(InsuredTobaccoUse)) query = query.Where(q => q.InsuredTobaccoUse == InsuredTobaccoUse);
                if (insuredDateOfBirthFilter.HasValue) query = query.Where(q => q.InsuredDateOfBirth == insuredDateOfBirthFilter);
                if (!string.IsNullOrEmpty(InsuredOccupationCode)) query = query.Where(q => q.InsuredOccupationCode == InsuredOccupationCode);
                if (InsuredAttainedAge.HasValue) query = query.Where(q => q.InsuredAttainedAge == InsuredAttainedAge);

                if (SortOrder == Html.GetSortAsc("ReinsBasisCode")) query = query.OrderBy(q => q.ReinsBasisCode);
                else if (SortOrder == Html.GetSortDsc("ReinsBasisCode")) query = query.OrderByDescending(q => q.ReinsBasisCode);
                else if (SortOrder == Html.GetSortAsc("TreatyCodeFilter")) query = query.OrderBy(q => q.TreatyCode);
                else if (SortOrder == Html.GetSortDsc("TreatyCodeFilter")) query = query.OrderByDescending(q => q.TreatyCode);
                else if (SortOrder == Html.GetSortAsc("FundsAccountingTypeCode")) query = query.OrderBy(q => q.FundsAccountingTypeCode);
                else if (SortOrder == Html.GetSortDsc("FundsAccountingTypeCode")) query = query.OrderByDescending(q => q.FundsAccountingTypeCode);
                else if (SortOrder == Html.GetSortAsc("PremiumFrequencyCode")) query = query.OrderBy(q => q.PremiumFrequencyCode);
                else if (SortOrder == Html.GetSortDsc("PremiumFrequencyCode")) query = query.OrderByDescending(q => q.PremiumFrequencyCode);
                else if (SortOrder == Html.GetSortAsc("ReportPeriodMonth")) query = query.OrderBy(q => q.ReportPeriodMonth);
                else if (SortOrder == Html.GetSortDsc("ReportPeriodMonth")) query = query.OrderByDescending(q => q.ReportPeriodMonth);
                else if (SortOrder == Html.GetSortAsc("ReportPeriodYear")) query = query.OrderBy(q => q.ReportPeriodYear);
                else if (SortOrder == Html.GetSortDsc("ReportPeriodYear")) query = query.OrderByDescending(q => q.ReportPeriodYear);
                else if (SortOrder == Html.GetSortAsc("RiskPeriodMonth")) query = query.OrderBy(q => q.RiskPeriodMonth);
                else if (SortOrder == Html.GetSortDsc("RiskPeriodMonth")) query = query.OrderByDescending(q => q.RiskPeriodMonth);
                else if (SortOrder == Html.GetSortAsc("RiskPeriodYear")) query = query.OrderBy(q => q.RiskPeriodYear);
                else if (SortOrder == Html.GetSortDsc("RiskPeriodYear")) query = query.OrderByDescending(q => q.RiskPeriodYear);
                else if (SortOrder == Html.GetSortAsc("TransactionTypeCode")) query = query.OrderBy(q => q.TransactionTypeCode);
                else if (SortOrder == Html.GetSortDsc("TransactionTypeCode")) query = query.OrderByDescending(q => q.TransactionTypeCode);
                else if (SortOrder == Html.GetSortAsc("PolicyNumberFilter")) query = query.OrderBy(q => q.PolicyNumber);
                else if (SortOrder == Html.GetSortDsc("PolicyNumberFilter")) query = query.OrderByDescending(q => q.PolicyNumber);
                else if (SortOrder == Html.GetSortAsc("IssueDatePol")) query = query.OrderBy(q => q.IssueDatePol);
                else if (SortOrder == Html.GetSortDsc("IssueDatePol")) query = query.OrderByDescending(q => q.IssueDatePol);
                else if (SortOrder == Html.GetSortAsc("IssueDateBen")) query = query.OrderBy(q => q.IssueDateBen);
                else if (SortOrder == Html.GetSortDsc("IssueDateBen")) query = query.OrderByDescending(q => q.IssueDateBen);
                else if (SortOrder == Html.GetSortAsc("ReinsEffDatePol")) query = query.OrderBy(q => q.ReinsEffDatePol);
                else if (SortOrder == Html.GetSortDsc("ReinsEffDatePol")) query = query.OrderByDescending(q => q.ReinsEffDatePol);
                else if (SortOrder == Html.GetSortAsc("ReinsEffDateBen")) query = query.OrderBy(q => q.ReinsEffDateBen);
                else if (SortOrder == Html.GetSortDsc("ReinsEffDateBen")) query = query.OrderByDescending(q => q.ReinsEffDateBen);
                else if (SortOrder == Html.GetSortAsc("CedingPlanCodeFilter")) query = query.OrderBy(q => q.CedingPlanCode);
                else if (SortOrder == Html.GetSortDsc("CedingPlanCodeFilter")) query = query.OrderByDescending(q => q.CedingPlanCode);
                else if (SortOrder == Html.GetSortAsc("CedingBenefitTypeCode")) query = query.OrderBy(q => q.CedingBenefitTypeCode);
                else if (SortOrder == Html.GetSortDsc("CedingBenefitTypeCode")) query = query.OrderByDescending(q => q.CedingBenefitTypeCode);
                else if (SortOrder == Html.GetSortAsc("CedingBenefitRiskCode")) query = query.OrderBy(q => q.CedingBenefitRiskCode);
                else if (SortOrder == Html.GetSortDsc("CedingBenefitRiskCode")) query = query.OrderByDescending(q => q.CedingBenefitRiskCode);
                else if (SortOrder == Html.GetSortAsc("BenefitCodeFilter")) query = query.OrderBy(q => q.BenefitCode);
                else if (SortOrder == Html.GetSortDsc("BenefitCodeFilter")) query = query.OrderByDescending(q => q.BenefitCode);
                else if (SortOrder == Html.GetSortAsc("OriSumAssured")) query = query.OrderBy(q => q.OriSumAssured);
                else if (SortOrder == Html.GetSortDsc("OriSumAssured")) query = query.OrderByDescending(q => q.OriSumAssured);
                else if (SortOrder == Html.GetSortAsc("CurrSumAssured")) query = query.OrderBy(q => q.CurrSumAssured);
                else if (SortOrder == Html.GetSortDsc("CurrSumAssured")) query = query.OrderByDescending(q => q.CurrSumAssured);
                else if (SortOrder == Html.GetSortAsc("AmountCededB4MlreShare")) query = query.OrderBy(q => q.AmountCededB4MlreShare);
                else if (SortOrder == Html.GetSortDsc("AmountCededB4MlreShare")) query = query.OrderByDescending(q => q.AmountCededB4MlreShare);
                else if (SortOrder == Html.GetSortAsc("RetentionAmount")) query = query.OrderBy(q => q.RetentionAmount);
                else if (SortOrder == Html.GetSortDsc("RetentionAmount")) query = query.OrderByDescending(q => q.RetentionAmount);
                else if (SortOrder == Html.GetSortAsc("AarOri")) query = query.OrderBy(q => q.AarOri);
                else if (SortOrder == Html.GetSortDsc("AarOri")) query = query.OrderByDescending(q => q.AarOri);
                else if (SortOrder == Html.GetSortAsc("Aar")) query = query.OrderBy(q => q.Aar);
                else if (SortOrder == Html.GetSortDsc("Aar")) query = query.OrderByDescending(q => q.Aar);
                else if (SortOrder == Html.GetSortAsc("InsuredNameFilter")) query = query.OrderBy(q => q.InsuredName);
                else if (SortOrder == Html.GetSortDsc("InsuredNameFilter")) query = query.OrderByDescending(q => q.InsuredName);
                else if (SortOrder == Html.GetSortAsc("InsuredGenderCode")) query = query.OrderBy(q => q.InsuredGenderCode);
                else if (SortOrder == Html.GetSortDsc("InsuredGenderCode")) query = query.OrderByDescending(q => q.InsuredGenderCode);
                else if (SortOrder == Html.GetSortAsc("InsuredTobaccoUse")) query = query.OrderBy(q => q.InsuredTobaccoUse);
                else if (SortOrder == Html.GetSortDsc("InsuredTobaccoUse")) query = query.OrderByDescending(q => q.InsuredTobaccoUse);
                else if (SortOrder == Html.GetSortAsc("InsuredDateOfBirthFilter")) query = query.OrderBy(q => q.InsuredDateOfBirth);
                else if (SortOrder == Html.GetSortDsc("InsuredDateOfBirthFilter")) query = query.OrderByDescending(q => q.InsuredDateOfBirth);
                else if (SortOrder == Html.GetSortAsc("InsuredOccupationCode")) query = query.OrderBy(q => q.InsuredOccupationCode);
                else if (SortOrder == Html.GetSortDsc("InsuredOccupationCode")) query = query.OrderByDescending(q => q.InsuredOccupationCode);
                else if (SortOrder == Html.GetSortAsc("InsuredAttainedAge")) query = query.OrderBy(q => q.InsuredAttainedAge);
                else if (SortOrder == Html.GetSortDsc("InsuredAttainedAge")) query = query.OrderByDescending(q => q.InsuredAttainedAge);
                else query = query.OrderBy(q => q.Id);

                LoadPage();
                var ids = query.Select(q => q.Id).ToArray();
                ViewBag.Total = ids.Length;

                var list = query.Skip(((Page ?? 1) - 1) * PageSize).Take(PageSize);
                return View(new StaticPagedList<RiDataSearchViewModel>(list, Page ?? 1, PageSize, ViewBag.Total));
            }
        
        
        
        }

        // GET: RiDataSearch/Details
        public ActionResult Details(
            int? id,
            string TreatyCode,
            string SoaQuarter,
            string RiskPeriodStartDate,
            string RiskPeriodEndDate,
            string InsuredName,
            string PolicyNumber,
            string InsuredDateOfBirth,
            string CedingPlanCode,
            string BenefitCode,
            string TreatyCodeFilter,
            string ReinsBasisCode,
            string FundsAccountingTypeCode,
            string PremiumFrequencyCode,
            int? ReportPeriodMonth,
            int? ReportPeriodYear,
            int? RiskPeriodMonth,
            int? RiskPeriodYear,
            string TransactionTypeCode,
            string PolicyNumberFilter,
            string IssueDatePol,
            string IssueDateBen,
            string ReinsEffDatePol,
            string ReinsEffDateBen,
            string CedingPlanCodeFilter,
            string CedingBenefitTypeCode,
            string CedingBenefitRiskCode,
            string BenefitCodeFilter,
            string OriSumAssured,
            string CurrSumAssured,
            string AmountCededB4MlreShare,
            string RetentionAmount,
            string AarOri,
            string Aar,
            string InsuredNameFilter,
            string InsuredGenderCode,
            string InsuredTobaccoUse,
            string InsuredDateOfBirthFilter,
            string InsuredOccupationCode,
            int? InsuredAttainedAge
        )
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["TreatyCode"] = TreatyCode,
                ["SoaQuarter"] = SoaQuarter,
                ["RiskPeriodStartDate"] = RiskPeriodStartDate,
                ["RiskPeriodEndDate"] = RiskPeriodEndDate,
                ["InsuredName"] = InsuredName,
                ["PolicyNumber"] = PolicyNumber,
                ["InsuredDateOfBirth"] = InsuredDateOfBirth,
                ["CedingPlanCode"] = CedingPlanCode,
                ["BenefitCode"] = BenefitCode,
                ["TreatyCodeFilter"] = TreatyCodeFilter,
                ["ReinsBasisCode"] = ReinsBasisCode,
                ["FundsAccountingTypeCode"] = FundsAccountingTypeCode,
                ["PremiumFrequencyCode"] = PremiumFrequencyCode,
                ["ReportPeriodMonth"] = ReportPeriodMonth,
                ["ReportPeriodYear"] = ReportPeriodYear,
                ["RiskPeriodMonth"] = RiskPeriodMonth,
                ["RiskPeriodYear"] = RiskPeriodYear,
                ["TransactionTypeCode"] = TransactionTypeCode,
                ["PolicyNumberFilter"] = PolicyNumberFilter,
                ["IssueDatePol"] = IssueDatePol,
                ["IssueDateBen"] = IssueDateBen,
                ["ReinsEffDatePol"] = ReinsEffDatePol,
                ["ReinsEffDateBen"] = ReinsEffDateBen,
                ["CedingPlanCodeFilter"] = CedingPlanCodeFilter,
                ["CedingBenefitTypeCode"] = CedingBenefitTypeCode,
                ["CedingBenefitRiskCode"] = CedingBenefitRiskCode,
                ["BenefitCodeFilter"] = BenefitCodeFilter,
                ["OriSumAssured"] = OriSumAssured,
                ["CurrSumAssured"] = CurrSumAssured,
                ["AmountCededB4MlreShare"] = AmountCededB4MlreShare,
                ["RetentionAmount"] = RetentionAmount,
                ["AarOri"] = AarOri,
                ["Aar"] = Aar,
                ["InsuredNameFilter"] = InsuredNameFilter,
                ["InsuredGenderCode"] = InsuredGenderCode,
                ["InsuredTobaccoUse"] = InsuredTobaccoUse,
                ["InsuredDateOfBirthFilter"] = InsuredDateOfBirthFilter,
                ["InsuredOccupationCode"] = InsuredOccupationCode,
                ["InsuredAttainedAge"] = InsuredAttainedAge,
            };

            RiDataBo riDataBo = new RiDataBo();
            if (id.HasValue)
                riDataBo = RiDataService.FindWithFormattedOutput(id.Value);

            if (riDataBo != null && !string.IsNullOrEmpty(riDataBo.Errors))
            {
                var Errors = JsonConvert.DeserializeObject<Dictionary<string, object>>(riDataBo.Errors);
                ViewBag.Errors = Errors;
            }

            ViewBag.RiDataBo = riDataBo;
            LoadPage();
            return View(new RiDataSearchViewModel());
        }

        public ActionResult Download(
            string downloadToken,
            bool? WriteHeader,
            string TreatyCode,
            string SoaQuarter,
            string RiskPeriodStartDate,
            string RiskPeriodEndDate,
            string InsuredName,
            string PolicyNumber,
            string InsuredDateOfBirth,
            string CedingPlanCode,
            string BenefitCode,
            string TreatyCodeFilter,
            string ReinsBasisCode,
            string FundsAccountingTypeCode,
            string PremiumFrequencyCode,
            int? ReportPeriodMonth,
            int? ReportPeriodYear,
            int? RiskPeriodMonth,
            int? RiskPeriodYear,
            string TransactionTypeCode,
            string PolicyNumberFilter,
            string IssueDatePol,
            string IssueDateBen,
            string ReinsEffDatePol,
            string ReinsEffDateBen,
            string CedingPlanCodeFilter,
            string CedingBenefitTypeCode,
            string CedingBenefitRiskCode,
            string BenefitCodeFilter,
            string OriSumAssured,
            string CurrSumAssured,
            string AmountCededB4MlreShare,
            string RetentionAmount,
            string AarOri,
            string Aar,
            string InsuredNameFilter,
            string InsuredGenderCode,
            string InsuredTobaccoUse,
            string InsuredDateOfBirthFilter,
            string InsuredOccupationCode,
            int? InsuredAttainedAge
        )
        {
            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            dynamic Params = new ExpandoObject();
            Params.WriteHeader = WriteHeader ?? false;
            Params.TreatyCode = TreatyCode;
            Params.SoaQuarter = SoaQuarter;
            Params.RiskPeriodStartDate = RiskPeriodStartDate;
            Params.RiskPeriodEndDate = RiskPeriodEndDate;
            Params.InsuredName = InsuredName;
            Params.PolicyNumber = PolicyNumber;
            Params.InsuredDateOfBirth = InsuredDateOfBirth;
            Params.CedingPlanCode = CedingPlanCode;
            Params.BenefitCode = BenefitCode;
            Params.TreatyCodeFilter = TreatyCodeFilter;
            Params.ReinsBasisCode = ReinsBasisCode;
            Params.FundsAccountingTypeCode = FundsAccountingTypeCode;
            Params.PremiumFrequencyCode = PremiumFrequencyCode;
            Params.ReportPeriodMonth = ReportPeriodMonth;
            Params.ReportPeriodYear = ReportPeriodYear;
            Params.RiskPeriodMonth = RiskPeriodMonth;
            Params.RiskPeriodYear = RiskPeriodYear;
            Params.TransactionTypeCode = TransactionTypeCode;
            Params.PolicyNumberFilter = PolicyNumberFilter;
            Params.IssueDatePol = IssueDatePol;
            Params.IssueDateBen = IssueDateBen;
            Params.ReinsEffDatePol = ReinsEffDatePol;
            Params.ReinsEffDateBen = ReinsEffDateBen;
            Params.CedingPlanCodeFilter = CedingPlanCodeFilter;
            Params.CedingBenefitTypeCode = CedingBenefitTypeCode;
            Params.CedingBenefitRiskCode = CedingBenefitRiskCode;
            Params.BenefitCodeFilter = BenefitCodeFilter;
            Params.OriSumAssured = OriSumAssured;
            Params.CurrSumAssured = CurrSumAssured;
            Params.AmountCededB4MlreShare = AmountCededB4MlreShare;
            Params.RetentionAmount = RetentionAmount;
            Params.AarOri = AarOri;
            Params.Aar = Aar;
            Params.InsuredNameFilter = InsuredNameFilter;
            Params.InsuredGenderCode = InsuredGenderCode;
            Params.InsuredTobaccoUse = InsuredTobaccoUse;
            Params.InsuredDateOfBirthFilter = InsuredDateOfBirthFilter;
            Params.InsuredOccupationCode = InsuredOccupationCode;
            Params.InsuredAttainedAge = InsuredAttainedAge;
            var export = new GenerateExportData();
            ExportBo exportBo = export.CreateExportRiDataSearch(AuthUserId, Params);

            if (exportBo.Total <= export.ExportInstantDownloadLimit)
            {
                export.Process(ref exportBo, _db);
                Session.Add("DownloadExport", true);
            }

            return RedirectToAction("Edit", "Export", new { exportBo.Id });
        }

        public void LoadPage()
        {
            DropDownTreatyCode(codeAsValue: true, foreign: false);
            DropDownBenefit(codeAsValue: true);
            DropDownReinsBasisCode(codeAsValue: true);
            DropDownFundsAccountingTypeCode(codeAsValue: true);
            DropDownPremiumFrequencyCode(codeAsValue: true);
            DropDownTransactionTypeCode(codeAsValue: true);
            DropDownYesNo();
            DropDownMonth(false);
            DropDownInsuredGenderCode(true);
            DropDownInsuredTobaccoUse(true);
            DropDownInsuredOccupationCode(true);

            GetTreatyCodes(foreign: false);
            GetBenefitCodes();

            ViewBag.StandardOutputList = StandardOutputService.Get();

            var entity = new RiData();
            var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("CedingPlanCode");
            ViewBag.CedingPlanCodeMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 30;

            SetViewBagMessage();
        }
    }
}