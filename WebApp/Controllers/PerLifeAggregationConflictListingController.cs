using BusinessObject;
using BusinessObject.Retrocession;
using ConsoleApp.Commands.ProcessDatas;
using DataAccess.Entities.Retrocession;
using PagedList;
using Services.Retrocession;
using Shared;
using System;
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
    public class PerLifeAggregationConflictListingController : BaseController
    {
        public const string Controller = "PerLifeAggregationConflictListing";

        // GET: PerLifeAggregationConflictListing
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string TreatyCode,
            int? RiskYear,
            int? RiskMonth,
            string InsuredName,
            string InsuredGender,
            string InsuredDateOfBirth,
            string PolicyNumber,
            string ReinsEffDatePol,
            string AAR,
            string GrossPremium,
            string NetPremium,
            string PremiumFrequencyMode,
            string RetroPremiumFrequencyMode,
            string CedingPlanCode,
            string CedingBenefitTypeCode,
            string CedingBenefitRiskCode,
            string MLReBenefitCode,
            string TerritoryOfIssueCode,
            string SortOrder,
            int? Page)
        {

            var insuredDateOfBirth = Util.GetParseDateTime(InsuredDateOfBirth);
            var reinsEffDatePol = Util.GetParseDateTime(ReinsEffDatePol);

            var aar = Util.StringToDouble(AAR);
            var grossPremium = Util.StringToDouble(GrossPremium);
            var netPremium = Util.StringToDouble(NetPremium);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["TreatyCode"] = TreatyCode,
                ["RiskYear"] = RiskYear,
                ["RiskMonth"] = RiskMonth,
                ["InsuredName"] = InsuredName,
                ["InsuredGender"] = InsuredGender,
                ["InsuredDateOfBirth"] = insuredDateOfBirth.HasValue ? InsuredDateOfBirth : null,
                ["PolicyNumber"] = PolicyNumber,
                ["ReinsEffDatePol"] = reinsEffDatePol.HasValue ? ReinsEffDatePol : null,
                ["AAR"] = aar.HasValue ? AAR : null,
                ["GrossPremium"] = grossPremium.HasValue ? GrossPremium : null,
                ["NetPremium"] = netPremium.HasValue ? NetPremium : null,
                ["PremiumFrequencyMode"] = PremiumFrequencyMode,
                ["RetroPremiumFrequencyMode"] = RetroPremiumFrequencyMode,
                ["CedingPlanCode"] = CedingPlanCode,
                ["CedingBenefitTypeCode"] = CedingBenefitTypeCode,
                ["CedingBenefitRiskCode"] = CedingBenefitRiskCode,
                ["MLReBenefitCode"] = MLReBenefitCode,
                ["TerritoryOfIssueCode"] = TerritoryOfIssueCode,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortTreatyCode = GetSortParam("TreatyCode");
            ViewBag.SortRiskYear = GetSortParam("RiskYear");
            ViewBag.SortRiskMonth = GetSortParam("RiskMonth");
            ViewBag.SortInsuredName = GetSortParam("InsuredName");
            ViewBag.SortInsuredGender = GetSortParam("InsuredGender");
            ViewBag.SortInsuredDateOfBirth = GetSortParam("InsuredDateOfBirth");
            ViewBag.SortPolicyNumber = GetSortParam("PolicyNumber");
            ViewBag.SortReinsEffDatePol = GetSortParam("ReinsEffDatePol");
            ViewBag.SortAAR = GetSortParam("AAR");
            ViewBag.SortGrossPremium = GetSortParam("GrossPremium");
            ViewBag.SortNetPremium = GetSortParam("NetPremium");
            ViewBag.SortPremiumFrequencyMode = GetSortParam("PremiumFrequencyMode");
            ViewBag.SortRetroPremiumFrequencyMode = GetSortParam("RetroPremiumFrequencyMode");
            ViewBag.SortCedingPlanCode = GetSortParam("CedingPlanCode");
            ViewBag.SortCedingBenefitTypeCode = GetSortParam("CedingBenefitTypeCode");
            ViewBag.SortCedingBenefitRiskCode = GetSortParam("CedingBenefitRiskCode");
            ViewBag.SortMLReBenefitCode = GetSortParam("MLReBenefitCode");
            ViewBag.SortTerritoryOfIssueCode = GetSortParam("TerritoryOfIssueCode");
            ViewBag.SortExceptionStatus = GetSortParam("ExceptionStatus");
            ViewBag.SortRemarks = GetSortParam("Remarks");

            var query = _db.PerLifeAggregationDetailData
                .Where(q => q.IsException == true)
                .Where(q => q.ExceptionType == PerLifeAggregationDetailDataBo.ExceptionTypeConflictCheck)
                .Select(PerLifeAggregationDetailDataViewModel.Expression());

            if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => q.TreatyCode == TreatyCode);
            if (RiskYear.HasValue) query = query.Where(q => q.RiskPeriodYear == RiskYear);
            if (RiskMonth.HasValue) query = query.Where(q => q.RiskPeriodMonth == RiskMonth);
            if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => q.InsuredName.Contains(InsuredName));
            if (!string.IsNullOrEmpty(InsuredGender)) query = query.Where(q => q.InsuredGenderCode == InsuredGender);
            if (insuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == insuredDateOfBirth);
            if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber.Contains(PolicyNumber));
            if (reinsEffDatePol.HasValue) query = query.Where(q => q.ReinsEffDatePol == reinsEffDatePol);
            if (aar.HasValue) query = query.Where(q => q.Aar == aar);
            if (grossPremium.HasValue) query = query.Where(q => q.GrossPremium == grossPremium);
            if (netPremium.HasValue) query = query.Where(q => q.NetPremium == netPremium);
            if (!string.IsNullOrEmpty(PremiumFrequencyMode)) query = query.Where(q => q.PremiumFrequencyCode == PremiumFrequencyMode);
            //if (RetroPremiumFrequencyMode.HasValue) query = query.Where(q => q.PremiumFrequencyCode == RetroPremiumFrequencyMode);
            if (!string.IsNullOrEmpty(CedingPlanCode)) query = query.Where(q => q.CedingPlanCode.Contains(CedingPlanCode));
            if (!string.IsNullOrEmpty(CedingBenefitTypeCode)) query = query.Where(q => q.CedingBenefitTypeCode == CedingBenefitTypeCode);
            if (!string.IsNullOrEmpty(CedingBenefitRiskCode)) query = query.Where(q => q.CedingBenefitRiskCode.Contains(CedingBenefitRiskCode));
            if (!string.IsNullOrEmpty(MLReBenefitCode)) query = query.Where(q => q.MlreBenefitCode == MLReBenefitCode);
            if (!string.IsNullOrEmpty(TerritoryOfIssueCode)) query = query.Where(q => q.TerritoryOfIssueCode == TerritoryOfIssueCode);

            if (SortOrder == Html.GetSortAsc("TreatyCode")) query = query.OrderBy(q => q.TreatyCode);
            else if (SortOrder == Html.GetSortDsc("TreatyCode")) query = query.OrderByDescending(q => q.TreatyCode);
            else if (SortOrder == Html.GetSortAsc("RiskYear")) query = query.OrderBy(q => q.RiskPeriodYear);
            else if (SortOrder == Html.GetSortDsc("RiskYear")) query = query.OrderByDescending(q => q.RiskPeriodYear);
            else if (SortOrder == Html.GetSortAsc("RiskMonth")) query = query.OrderBy(q => q.RiskPeriodMonth);
            else if (SortOrder == Html.GetSortDsc("RiskYRiskMonthear")) query = query.OrderByDescending(q => q.RiskPeriodMonth);
            else if (SortOrder == Html.GetSortAsc("InsuredName")) query = query.OrderBy(q => q.InsuredName);
            else if (SortOrder == Html.GetSortDsc("InsuredName")) query = query.OrderByDescending(q => q.InsuredName);
            else if (SortOrder == Html.GetSortAsc("InsuredGender")) query = query.OrderBy(q => q.InsuredGenderCode);
            else if (SortOrder == Html.GetSortDsc("InsuredGender")) query = query.OrderByDescending(q => q.InsuredGenderCode);
            else if (SortOrder == Html.GetSortAsc("InsuredDateOfBirth")) query = query.OrderBy(q => q.InsuredDateOfBirth);
            else if (SortOrder == Html.GetSortDsc("InsuredDateOfBirth")) query = query.OrderByDescending(q => q.InsuredDateOfBirth);
            else if (SortOrder == Html.GetSortAsc("PolicyNumber")) query = query.OrderBy(q => q.PolicyNumber);
            else if (SortOrder == Html.GetSortDsc("PolicyNumber")) query = query.OrderByDescending(q => q.PolicyNumber);
            else if (SortOrder == Html.GetSortAsc("ReinsEffDatePol")) query = query.OrderBy(q => q.ReinsEffDatePol);
            else if (SortOrder == Html.GetSortDsc("ReinsEffDatePol")) query = query.OrderByDescending(q => q.ReinsEffDatePol);
            else if (SortOrder == Html.GetSortAsc("AAR")) query = query.OrderBy(q => q.Aar);
            else if (SortOrder == Html.GetSortDsc("AAR")) query = query.OrderByDescending(q => q.Aar);
            else if (SortOrder == Html.GetSortAsc("GrossPremium")) query = query.OrderBy(q => q.GrossPremium);
            else if (SortOrder == Html.GetSortDsc("GrossPremium")) query = query.OrderByDescending(q => q.GrossPremium);
            else if (SortOrder == Html.GetSortAsc("NetPremium")) query = query.OrderBy(q => q.NetPremium);
            else if (SortOrder == Html.GetSortDsc("NetPremium")) query = query.OrderByDescending(q => q.NetPremium);
            else if (SortOrder == Html.GetSortAsc("PremiumFrequencyMode")) query = query.OrderBy(q => q.PremiumFrequencyCode);
            else if (SortOrder == Html.GetSortDsc("PremiumFrequencyMode")) query = query.OrderByDescending(q => q.PremiumFrequencyCode);
            //else if (SortOrder == Html.GetSortAsc("RetroPremiumFrequencyMode")) query = query.OrderBy(q => q.PremiumFrequencyCode);
            //else if (SortOrder == Html.GetSortDsc("RetroPremiumFrequencyMode")) query = query.OrderByDescending(q => q.PremiumFrequencyCode);
            else if (SortOrder == Html.GetSortAsc("CedingPlanCode")) query = query.OrderBy(q => q.CedingPlanCode);
            else if (SortOrder == Html.GetSortDsc("CedingPlanCode")) query = query.OrderByDescending(q => q.CedingPlanCode);
            else if (SortOrder == Html.GetSortAsc("CedingBenefitTypeCode")) query = query.OrderBy(q => q.CedingBenefitTypeCode);
            else if (SortOrder == Html.GetSortDsc("CedingBenefitTypeCode")) query = query.OrderByDescending(q => q.CedingBenefitTypeCode);
            else if (SortOrder == Html.GetSortAsc("CedingBenefitRiskCode")) query = query.OrderBy(q => q.CedingBenefitRiskCode);
            else if (SortOrder == Html.GetSortDsc("CedingBenefitRiskCode")) query = query.OrderByDescending(q => q.CedingBenefitRiskCode);
            else if (SortOrder == Html.GetSortAsc("MLReBenefitCode")) query = query.OrderBy(q => q.MlreBenefitCode);
            else if (SortOrder == Html.GetSortDsc("MLReBenefitCode")) query = query.OrderByDescending(q => q.MlreBenefitCode);
            else if (SortOrder == Html.GetSortAsc("TerritoryOfIssueCode")) query = query.OrderBy(q => q.TerritoryOfIssueCode);
            else if (SortOrder == Html.GetSortDsc("TerritoryOfIssueCode")) query = query.OrderByDescending(q => q.TerritoryOfIssueCode);
            else query = query.OrderBy(q => q.Id);

            ViewBag.Total = query.Count();
            IndexPage();

            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        public ActionResult Download(
            string downloadToken,
            string TreatyCode,
            int? RiskYear,
            int? RiskMonth,
            string InsuredName,
            string InsuredGender,
            string InsuredDateOfBirth,
            string PolicyNumber,
            string ReinsEffDatePol,
            string AAR,
            string GrossPremium,
            string NetPremium,
            string PremiumFrequencyMode,
            string RetroPremiumFrequencyMode,
            string CedingPlanCode,
            string CedingBenefitTypeCode,
            string CedingBenefitRiskCode,
            string MLReBenefitCode,
            string TerritoryOfIssueCode)
        {
            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            dynamic Params = new ExpandoObject();
            Params.TreatyCode = TreatyCode;
            Params.RiskYear = RiskYear;
            Params.RiskMonth = RiskMonth;
            Params.InsuredName = InsuredName;
            Params.InsuredGender = InsuredGender;
            Params.InsuredDateOfBirth = InsuredDateOfBirth;
            Params.PolicyNumber = PolicyNumber;
            Params.ReinsEffDatePol = ReinsEffDatePol;
            Params.Aar = AAR;
            Params.GrossPremium = GrossPremium;
            Params.NetPremium = NetPremium;
            Params.PremiumFrequencyMode = PremiumFrequencyMode;
            Params.RetroPremiumFrequencyMode = RetroPremiumFrequencyMode;
            Params.CedingPlanCode = CedingPlanCode;
            Params.CedingBenefitTypeCode = CedingBenefitTypeCode;
            Params.CedingBenefitRiskCode = CedingBenefitRiskCode;
            Params.MLReBenefitCode = MLReBenefitCode;
            Params.TerritoryOfIssueCode = TerritoryOfIssueCode;

            var export = new GenerateExportData();
            ExportBo exportBo = export.CreateExportPerLifeAggregationConflictListing(AuthUserId, Params);

            if (exportBo.Total <= export.ExportInstantDownloadLimit)
            {
                export.Process(ref exportBo, _db);
                Session.Add("DownloadExport", true);
            }

            return RedirectToAction("Edit", "Export", new { exportBo.Id });
        }

        public void IndexPage()
        {
            DropDownInsuredGenderCode(codeAsValue: true);
            DropDownPremiumFrequencyCode(codeAsValue: true);
            DropDownRetroPremiumFrequencyMode(codeAsValue: true);
            DropDownTerritoryOfIssueCode(codeAsValue: true);
            DropDownCedingBenefitTypeCode(codeAsValue: true);
            DropDownBenefit(codeAsValue: true);
            DropDownMonth();
            SetViewBagMessage();
        }

        //// GET: PerLifeAggregationConflictListing/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            //LoadPage();
            //return View(new PerLifeAggregationConflictListingViewModel());
            return RedirectToAction("Index");
        }

        // Post: PerLifeAggregationConflictListing/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, PerLifeAggregationConflictListingViewModel model)
        {
            //if (ModelState.IsValid)
            //{
            //    var bo = model.FormBo(AuthUserId, AuthUserId);
            //    var trail = GetNewTrailObject();
            //    Result = new Shared.DataAccess.Result();
            //    var treatyCodeBo = TreatyCodeService.FindByCode(bo.TreatyCodeStr);
            //    if (treatyCodeBo != null)
            //    {
            //        bo.TreatyCodeId = treatyCodeBo.Id;
            //    }
            //    else
            //    {
            //        Result.AddError(string.Format(MessageBag.TreatyCodeNotFound, bo.TreatyCodeStr));
            //        Result.Valid = false;
            //    }
            //    var benefitCodeBo = BenefitService.FindByCode(bo.MLReBenefitCodeStr);
            //    if (benefitCodeBo != null)
            //    {
            //        bo.MLReBenefitCodeId = benefitCodeBo.Id;
            //    }
            //    else
            //    {
            //        Result.AddError(string.Format(MessageBag.BenefitCodeNotFound, bo.MLReBenefitCodeStr));
            //        Result.Valid = false;
            //    }
            //    if (Result.Valid)
            //    {
            //        model.Id = bo.Id;
            //        Result = PerLifeAggregationConflictListingService.Create(ref bo, ref trail);
            //        if (Result.Valid)
            //        {
            //            CreateTrail(
            //                bo.Id,
            //                "Create Per Life Aggregation Conflict Listing"
            //                );

            //            SetCreateSuccessMessage(Controller);
            //            return RedirectToAction("Edit", new { id = bo.Id });
            //        }
            //    }
            //    AddResult(Result);
            //}
            //LoadPage();
            //return View(model);
            return RedirectToAction("Index");
        }

        // GET:PerLifeAggregationConflictListing/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = PerLifeAggregationDetailDataService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            //LoadPage();
            return View(new PerLifeAggregationDetailDataViewModel(bo));
        }

        // POST: PerLifeAggregationConflictListing/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, PerLifeAggregationConflictListingViewModel model)
        {
            //var dbBo = PerLifeAggregationConflictListingService.Find(id);
            //if (dbBo == null)
            //{
            //    return RedirectToAction("Index");
            //}

            //if (ModelState.IsValid)
            //{
            //    var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
            //    bo.Id = dbBo.Id;

            //    var trail = GetNewTrailObject();
            //    Result = new Shared.DataAccess.Result();

            //    var treatyCodeBo = TreatyCodeService.FindByCode(bo.TreatyCodeStr);
            //    if (treatyCodeBo != null)
            //    {
            //        bo.TreatyCodeId = treatyCodeBo.Id;
            //    }
            //    else
            //    {
            //        Result.AddError(string.Format(MessageBag.TreatyCodeNotFound, bo.TreatyCodeStr));
            //        Result.Valid = false;
            //    }
            //    var benefitCodeBo = BenefitService.FindByCode(bo.MLReBenefitCodeStr);
            //    if (benefitCodeBo != null)
            //    {
            //        bo.MLReBenefitCodeId = benefitCodeBo.Id;
            //    }
            //    else
            //    {
            //        Result.AddError(string.Format(MessageBag.BenefitCodeNotFound, bo.MLReBenefitCodeStr));
            //        Result.Valid = false;
            //    }

            //    if (Result.Valid)
            //    {
            //        Result = PerLifeAggregationConflictListingService.Update(ref bo, ref trail);
            //        if (Result.Valid)
            //        {
            //            CreateTrail(
            //                bo.Id,
            //                "Update Per Life Aggregation Conflict Listing"
            //                );

            //            SetUpdateSuccessMessage(Controller);
            //            return RedirectToAction("Edit", new { id = bo.Id });
            //        }
            //    }
            //    AddResult(Result);
            //}

            //LoadPage();
            //return View(model);
            return RedirectToAction("Index");
        }

        // GET: PerLifeAggregationConflictListing/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            //SetViewBagMessage();
            //var bo = PerLifeAggregationConflictListingService.Find(id);
            //if (bo == null)
            //{
            //    return RedirectToAction("Index");
            //}

            //return View(new PerLifeAggregationConflictListingViewModel(bo));
            return RedirectToAction("Index");
        }

        // POST: PerLifeAggregationConflictListing/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, PerLifeAggregationConflictListingViewModel model)
        {
            //var bo = PerLifeAggregationConflictListingService.Find(id);
            //if (bo == null)
            //{
            //    return RedirectToAction("Index");
            //}

            //var trail = GetNewTrailObject();
            //Result = PerLifeAggregationConflictListingService.Delete(bo, ref trail);
            //if (Result.Valid)
            //{
            //    CreateTrail(
            //        bo.Id,
            //        "Delete Per Life Aggregation Conflict Listing"
            //    );

            //    SetDeleteSuccessMessage(Controller);
            //    return RedirectToAction("Index");
            //}

            //if (Result.MessageBag.Errors.Count > 1)
            //    SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            //else
            //    SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            //return RedirectToAction("Delete", new { id = bo.Id });
            return RedirectToAction("Index");
        }

        public void LoadPage()
        {
            var entity = new PerLifeAggregationConflictListing();
            var cedantPlanCodeCodeMaxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("CedantPlanCode");
            ViewBag.CedantPlanCodeMaxLength = cedantPlanCodeCodeMaxLengthAttr != null ? cedantPlanCodeCodeMaxLengthAttr.Length : 255;
            var insuredNameMaxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("InsuredName");
            ViewBag.InsuredNameMaxLength = insuredNameMaxLengthAttr != null ? insuredNameMaxLengthAttr.Length : 100;
            var policyNumberMaxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("PolicyNumber");
            ViewBag.PolicyNumberMaxLength = policyNumberMaxLengthAttr != null ? policyNumberMaxLengthAttr.Length : 50;
            var cedingBenefitRiskCodeMaxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("CedingBenefitRiskCode");
            ViewBag.PolicyNumberMaxLength = cedingBenefitRiskCodeMaxLengthAttr != null ? cedingBenefitRiskCodeMaxLengthAttr.Length : 50;
            var cedingBenefitTypeCodeMaxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("CedingBenefitTypeCode");
            ViewBag.PolicyNumberMaxLength = cedingBenefitTypeCodeMaxLengthAttr != null ? cedingBenefitTypeCodeMaxLengthAttr.Length : 50;

            DropDownTreatyCode();
            DropDownInsuredGenderCode();
            DropDownPremiumFrequencyCode();
            DropDownRetroPremiumFrequencyMode();
            DropDownTerritoryOfIssueCode();
            DropDownMonth();

            SetViewBagMessage();
        }

        //public List<SelectListItem> DropDownRiskMonth()
        //{
        //    var items = GetEmptyDropDownList();
        //    foreach (var i in Enumerable.Range(1, PerLifeAggregationConflictListingBo.RiskMonthDecember))
        //    {
        //        items.Add(new SelectListItem { Text = PerLifeAggregationConflictListingBo.GetRiskMonthName(i), Value = i.ToString() });
        //    }
        //    ViewBag.DropDownRiskMonths = items;
        //    return items;
        //}
    }
}