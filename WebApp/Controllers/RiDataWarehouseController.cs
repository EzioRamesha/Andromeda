using BusinessObject;
using BusinessObject.RiDatas;
using ConsoleApp.Commands.ProcessDatas;
using DataAccess.Entities;
using PagedList;
using Services;
using Services.RiDatas;
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
    public class RiDataWarehouseController : BaseController
    {
        public const string Controller = "RiDataWarehouse";

        // GET: RiDataWarehouse
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            int? CutOffId,
            int? CedantId,
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
            string BenefitCodeFilter,
            string ReinsBasisCode,
            string FundsAccountingTypeCode,
            string PremiumFrequencyCode,
            int? ReportPeriodMonth,
            int? ReportPeriodYear,
            string TransactionTypeCode,
            string PolicyNumberFilter,
            string IssueDatePol,
            string IssueDateBen,
            string ReinsEffDatePol,
            string SortOrder,
            int? Page,
            bool IsSnapshotVersion = false)
        {
            List<string> errors = new List<string> { };

            DateTime? riskPeriodStartDate = null;
            if (!string.IsNullOrEmpty(RiskPeriodStartDate) && !Util.TryParseDateTime(RiskPeriodStartDate, out riskPeriodStartDate, out _))
                errors.Add(string.Format(MessageBag.InvalidDateFormatWithName, "Risk Period Start Date"));

            DateTime? riskPeriodEndDate = null;
            if (!string.IsNullOrEmpty(RiskPeriodEndDate) && !Util.TryParseDateTime(RiskPeriodEndDate, out riskPeriodEndDate, out _))
                errors.Add(string.Format(MessageBag.InvalidDateFormatWithName, "Risk Period End Date"));

            if (riskPeriodStartDate.HasValue && riskPeriodEndDate.HasValue && riskPeriodEndDate < riskPeriodStartDate)
                errors.Add(string.Format(MessageBag.GreaterThan, "Risk Period End Date", "Risk Period Start Date"));

            DateTime? insuredDateOfBirth = null;
            if (!string.IsNullOrEmpty(InsuredDateOfBirth) && !Util.TryParseDateTime(InsuredDateOfBirth, out insuredDateOfBirth, out _))
                errors.Add(string.Format(MessageBag.InvalidDateFormatWithName, "Insured Date of Birth"));

            DateTime? issueDatePol = null;
            if (!string.IsNullOrEmpty(IssueDatePol) && !Util.TryParseDateTime(IssueDatePol, out issueDatePol, out _))
                errors.Add(string.Format(MessageBag.InvalidDateFormatWithName, "Issue Date Policy"));

            DateTime? issueDateBen = null;
            if (!string.IsNullOrEmpty(IssueDateBen) && !Util.TryParseDateTime(IssueDateBen, out issueDateBen, out _))
                errors.Add(string.Format(MessageBag.InvalidDateFormatWithName, "Issue Date Benefit"));

            DateTime? reinsEffDatePol = null;
            if (!string.IsNullOrEmpty(ReinsEffDatePol) && !Util.TryParseDateTime(ReinsEffDatePol, out reinsEffDatePol, out _))
                errors.Add(string.Format(MessageBag.InvalidDateFormatWithName, "Issue Date Benefit"));

            string[] treatyCodes = (string.IsNullOrEmpty(TreatyCode)) ? new string[] { } : Util.ToArraySplitTrim(TreatyCode, emptyString: false);
            string[] benefitCodes = Util.ToArraySplitTrim(BenefitCode);

            if (CedantId.HasValue && string.IsNullOrEmpty(TreatyCode))
            {
                var treatyCodeBos = TreatyCodeService.GetByCedantId(CedantId.Value);
                treatyCodes = !treatyCodeBos.IsNullOrEmpty() ? treatyCodeBos.Select(q => q.Code).ToArray() : new string[] { };
            }
            else if (CedantId.HasValue && !string.IsNullOrEmpty(TreatyCode))
            {
                var treatyCodeBos = TreatyCodeService.GetByCedantId(CedantId.Value);
                var treatyCodeList = treatyCodeBos.Select(q => q.Code).ToList();
                if (!treatyCodeList.IsNullOrEmpty() && treatyCodes.Except(treatyCodeList).Any())
                    errors.Add("Treaty Code entered not belongs to selected Ceding Company");
            }

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["IsSnapshotVersion"] = IsSnapshotVersion,
                ["CutOffId"] = CutOffId,
                ["CedantId"] = CedantId,
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
                ["BenefitCodeFilter"] = BenefitCodeFilter,
                ["ReinsBasisCode"] = ReinsBasisCode,
                ["FundsAccountingTypeCode"] = FundsAccountingTypeCode,
                ["PremiumFrequencyCode"] = PremiumFrequencyCode,
                ["ReportPeriodMonth"] = ReportPeriodMonth,
                ["ReportPeriodYear"] = ReportPeriodYear,
                ["TransactionTypeCode"] = TransactionTypeCode,
                ["PolicyNumberFilter"] = PolicyNumberFilter,
                ["IssueDatePol"] = IssueDatePol,
                ["IssueDateBen"] = IssueDateBen,
                ["ReinsEffDatePol"] = ReinsEffDatePol,
                ["SortOrder"] = SortOrder,
            };

            ViewBag.SortOrder = SortOrder;
            ViewBag.SortTreatyCodeFilter = GetSortParam("TreatyCodeFilter");
            ViewBag.SortBenefitCodeFilter = GetSortParam("BenefitCodeFilter");
            ViewBag.SortReinsBasisCode = GetSortParam("ReinsBasisCode");
            ViewBag.SortFundsAccountingTypeCode = GetSortParam("FundsAccountingTypeCode");
            ViewBag.SortPremiumFrequencyCode = GetSortParam("PremiumFrequencyCode");
            ViewBag.SortReportPeriodMonth = GetSortParam("ReportPeriodMonth");
            ViewBag.SortReportPeriodYear = GetSortParam("ReportPeriodYear");
            ViewBag.SortTransactionTypeCode = GetSortParam("TransactionTypeCode");
            ViewBag.SortPolicyNumberFilter = GetSortParam("PolicyNumberFilter");
            ViewBag.SortIssueDatePol = GetSortParam("IssueDatePol");
            ViewBag.SortIssueDateBen = GetSortParam("IssueDateBen");
            ViewBag.SortReinsEffDatePol = GetSortParam("ReinsEffDatePol");

            _db.Database.CommandTimeout = 0;

            IQueryable<RiDataWarehouseViewModel> query;

            if (IsSnapshotVersion)
            {
                query = _db.RiDataWarehouseHistories.Select(RiDataWarehouseViewModel.HistoryExpression());
            }
            else
            {
                query = _db.RiDataWarehouse.Select(RiDataWarehouseViewModel.Expression());
            }

            if ((!IsSnapshotVersion &&
                !CedantId.HasValue &&
                string.IsNullOrEmpty(TreatyCode) &&
                string.IsNullOrEmpty(SoaQuarter) &&
                !riskPeriodStartDate.HasValue &&
                !riskPeriodEndDate.HasValue &&
                string.IsNullOrEmpty(InsuredName) &&
                string.IsNullOrEmpty(PolicyNumber) &&
                !insuredDateOfBirth.HasValue &&
                string.IsNullOrEmpty(CedingPlanCode) &&
                string.IsNullOrEmpty(BenefitCode)) ||
                errors.Count() > 0)
            {
                query = query.Where(q => q.Id == 0);

                if (errors.Count() > 0)
                    SetErrorSessionMsgArr(errors);

                ViewBag.DisableDownload = true;
            }
            else
            {
                if (IsSnapshotVersion)
                {
                    var cutOffId = CutOffId ?? 0;
                    query = query.Where(q => q.CutOffId == cutOffId);
                }
                if (!treatyCodes.IsNullOrEmpty()) query = query.Where(q => treatyCodes.Contains(q.TreatyCode));
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
                if (!string.IsNullOrEmpty(BenefitCode)) query = query.Where(q => benefitCodes.Contains(q.MlreBenefitCode));
                if (!string.IsNullOrEmpty(TreatyCodeFilter)) query = query.Where(q => TreatyCodeFilter == q.TreatyCode);
                if (!string.IsNullOrEmpty(BenefitCodeFilter)) query = query.Where(q => BenefitCodeFilter == q.MlreBenefitCode);
                if (!string.IsNullOrEmpty(ReinsBasisCode)) query = query.Where(q => q.ReinsBasisCode == ReinsBasisCode);
                if (!string.IsNullOrEmpty(FundsAccountingTypeCode)) query = query.Where(q => q.FundsAccountingTypeCode == FundsAccountingTypeCode);
                if (!string.IsNullOrEmpty(PremiumFrequencyCode)) query = query.Where(q => q.PremiumFrequencyCode == PremiumFrequencyCode);
                if (ReportPeriodMonth.HasValue) query = query.Where(q => q.ReportPeriodMonth == ReportPeriodMonth);
                if (ReportPeriodYear.HasValue) query = query.Where(q => q.ReportPeriodYear == ReportPeriodYear);
                if (!string.IsNullOrEmpty(TransactionTypeCode)) query = query.Where(q => q.TransactionTypeCode == TransactionTypeCode);
                if (!string.IsNullOrEmpty(PolicyNumberFilter)) query = query.Where(q => q.PolicyNumber == PolicyNumberFilter);
                if (issueDatePol.HasValue) query = query.Where(q => q.IssueDatePol == issueDatePol);
                if (issueDateBen.HasValue) query = query.Where(q => q.IssueDateBen == issueDateBen);
                if (reinsEffDatePol.HasValue) query = query.Where(q => q.ReinsEffDatePol == reinsEffDatePol);

                if (SortOrder == Html.GetSortAsc("TreatyCodeFilter")) query = query.OrderBy(q => q.TreatyCode);
                else if (SortOrder == Html.GetSortDsc("TreatyCodeFilter")) query = query.OrderByDescending(q => q.TreatyCode);
                else if (SortOrder == Html.GetSortAsc("BenefitCodeFilter")) query = query.OrderBy(q => q.MlreBenefitCode);
                else if (SortOrder == Html.GetSortDsc("BenefitCodeFilter")) query = query.OrderByDescending(q => q.MlreBenefitCode);
                else if (SortOrder == Html.GetSortAsc("ReinsBasisCode")) query = query.OrderBy(q => q.ReinsBasisCode);
                else if (SortOrder == Html.GetSortDsc("ReinsBasisCode")) query = query.OrderByDescending(q => q.ReinsBasisCode);
                else if (SortOrder == Html.GetSortAsc("FundsAccountingTypeCode")) query = query.OrderBy(q => q.FundsAccountingTypeCode);
                else if (SortOrder == Html.GetSortDsc("FundsAccountingTypeCode")) query = query.OrderByDescending(q => q.FundsAccountingTypeCode);
                else if (SortOrder == Html.GetSortAsc("PremiumFrequencyCode")) query = query.OrderBy(q => q.PremiumFrequencyCode);
                else if (SortOrder == Html.GetSortDsc("PremiumFrequencyCode")) query = query.OrderByDescending(q => q.PremiumFrequencyCode);
                else if (SortOrder == Html.GetSortAsc("ReportPeriodMonth")) query = query.OrderBy(q => q.ReportPeriodMonth);
                else if (SortOrder == Html.GetSortDsc("ReportPeriodMonth")) query = query.OrderByDescending(q => q.ReportPeriodMonth);
                else if (SortOrder == Html.GetSortAsc("ReportPeriodYear")) query = query.OrderBy(q => q.ReportPeriodYear);
                else if (SortOrder == Html.GetSortDsc("ReportPeriodYear")) query = query.OrderByDescending(q => q.ReportPeriodYear);
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
                else query = query.OrderByDescending(q => q.Id);
            }

            IndexPage();
            ViewBag.Total = query.Count();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        public ActionResult Edit(
            int? id,
            bool? IsSnapshotVersion,
            int? CutOffId,
            int? CedantId,
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
            string BenefitCodeFilter,
            string ReinsBasisCode,
            string FundsAccountingTypeCode,
            string PremiumFrequencyCode,
            int? ReportPeriodMonth,
            int? ReportPeriodYear,
            string TransactionTypeCode,
            string PolicyNumberFilter,
            string IssueDatePol,
            string IssueDateBen,
            string ReinsEffDatePol)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["IsSnapshotVersion"] = IsSnapshotVersion,
                ["CutOffId"] = CutOffId,
                ["CedantId"] = CedantId,
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
                ["BenefitCodeFilter"] = BenefitCodeFilter,
                ["ReinsBasisCode"] = ReinsBasisCode,
                ["FundsAccountingTypeCode"] = FundsAccountingTypeCode,
                ["PremiumFrequencyCode"] = PremiumFrequencyCode,
                ["ReportPeriodMonth"] = ReportPeriodMonth,
                ["ReportPeriodYear"] = ReportPeriodYear,
                ["TransactionTypeCode"] = TransactionTypeCode,
                ["PolicyNumberFilter"] = PolicyNumberFilter,
                ["IssueDatePol"] = IssueDatePol,
                ["IssueDateBen"] = IssueDateBen,
                ["ReinsEffDatePol"] = ReinsEffDatePol,
            };

            RiDataWarehouseViewModel model = null;

            if (IsSnapshotVersion.HasValue && IsSnapshotVersion.Value)
            {
                var bo = RiDataWarehouseHistoryService.Find(id, CutOffId);
                if (bo != null)
                    model = new RiDataWarehouseViewModel(bo);
            }
            else
            {
                var bo = RiDataWarehouseService.Find(id);
                if (bo != null)
                    model = new RiDataWarehouseViewModel(bo);
            }

            if (model == null)
            {
                return RedirectToAction("Index");
            }

            Dictionary<string, string> values = new Dictionary<string, string>();
            foreach (var property in model.GetType().GetProperties())
            {
                if (property.Name == "Id" || property.Name == "IsSnapShotVersion" || property.Name == "CutOffId")
                    continue;

                string propertyName = "";
                if (property.Name.StartsWith("Mfrs17"))
                {
                    propertyName = "Mfrs17" + property.Name.Substring(6).ToProperCase(true);
                }
                else
                {
                    propertyName = property.Name.ToProperCase(true);
                }

                if (property.Name == "EndingPolicyStatus")
                {
                    values[propertyName] = PickListDetailService.Find(model.EndingPolicyStatus)?.ToString();
                    continue;
                }

                if (property.Name == "RecordType")
                {
                    values[propertyName] = RiDataBatchBo.GetRecordTypeName(model.RecordType);
                    continue;
                }

                if (property.Name == "LastUpdatedDate")
                {
                    values[propertyName] = model.LastUpdatedDate.ToString(Util.GetDateFormat());
                    continue;
                }

                var value = property.GetValue(model);
                if (value == null || (value is string @s && string.IsNullOrEmpty(@s)))
                {
                    values[propertyName] = null;
                    continue;
                }

                int type = StandardOutputBo.GetTypeByPropertyName(property.Name);
                int dataType = StandardOutputBo.GetDataTypeByType(type);
                switch (dataType)
                {
                    case StandardOutputBo.DataTypeDate:
                        values[propertyName] = ((DateTime)value).ToString(Util.GetDateFormat());
                        break;
                    case StandardOutputBo.DataTypeAmount:
                    case StandardOutputBo.DataTypePercentage:
                        values[propertyName] = Util.DoubleToString(value.ToString());
                        break;
                    case StandardOutputBo.DataTypeString:
                    case StandardOutputBo.DataTypeInteger:
                    case StandardOutputBo.DataTypeDropDown:
                        values[propertyName] = value.ToString();
                        break;
                    default:
                        break;
                }
            }

            ViewBag.Values = values;

            //LoadPage(model);
            return View(model);
        }

        public ActionResult Download(
           string downloadToken,
            bool? WriteHeader,
            bool? IsSnapshotVersion,
            int? CutOffId,
            int? CedantId,
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
            string BenefitCodeFilter,
            string ReinsBasisCode,
            string FundsAccountingTypeCode,
            string PremiumFrequencyCode,
            int? ReportPeriodMonth,
            int? ReportPeriodYear,
            string TransactionTypeCode,
            string PolicyNumberFilter,
            string IssueDatePol,
            string IssueDateBen,
            string ReinsEffDatePol)
        {
            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            dynamic Params = new ExpandoObject();
            Params.WriteHeader = WriteHeader ?? false;
            Params.IsSnapshotVersion = IsSnapshotVersion ?? false;
            Params.CutOffId = IsSnapshotVersion.HasValue && IsSnapshotVersion.Value ? CutOffId : null;
            Params.CedantId = CedantId;
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
            Params.BenefitCodeFilter = BenefitCodeFilter;
            Params.ReinsBasisCode = ReinsBasisCode;
            Params.FundsAccountingTypeCode = FundsAccountingTypeCode;
            Params.PremiumFrequencyCode = PremiumFrequencyCode;
            Params.ReportPeriodMonth = ReportPeriodMonth;
            Params.ReportPeriodYear = ReportPeriodYear;
            Params.TransactionTypeCode = TransactionTypeCode;
            Params.PolicyNumberFilter = PolicyNumberFilter;
            Params.IssueDatePol = IssueDatePol;
            Params.IssueDateBen = IssueDateBen;
            Params.ReinsEffDatePol = ReinsEffDatePol;
            var export = new GenerateExportData();
            ExportBo exportBo = IsSnapshotVersion.HasValue && IsSnapshotVersion.Value ?
                export.CreateExportRiDataWarehouseHistory(AuthUserId, Params) : export.CreateExportRiDataWarehouse(AuthUserId, Params);

            if (exportBo.Total <= export.ExportInstantDownloadLimit)
            {
                export.Process(ref exportBo, _db);
                Session.Add("DownloadExport", true);
            }

            return RedirectToAction("Edit", "Export", new { exportBo.Id });
        }

        public void IndexPage()
        {
            //SetViewBagMessage();
            //DropDownPolicyStatus();
            //DropDownPremiumFrequencyCode();
            //DropDownMonth();
            //DropDownTransactionTypeCode(true);
            //DropDownRecordType();
            //DropDownPolicyStatusCode();

            DropDownTreatyCode(codeAsValue: true, isUniqueCode: true, foreign: false);
            DropDownBenefit(codeAsValue: true);
            DropDownReinsBasisCode(codeAsValue: true);
            DropDownFundsAccountingTypeCode(codeAsValue: true);
            DropDownPremiumFrequencyCode(codeAsValue: true);
            DropDownTransactionTypeCode(codeAsValue: true);
            DropDownYesNo();
            DropDownCutOff();
            DropDownMonth(false);

            DropDownCedant();

            GetTreatyCodes(true, false);
            GetBenefitCodes();

            ViewBag.StandardOutputList = StandardOutputService.Get();

            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownRecordType()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= RiDataBatchBo.RecordTypeMax; i++)
            {
                items.Add(new SelectListItem { Text = RiDataBatchBo.GetRecordTypeName(i), Value = i.ToString() });
            }
            ViewBag.DropDownRecordTypes = items;
            return items;
        }

        public List<SelectListItem> DropDownPolicyStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= RiDataWarehouseBo.PolicyStatusMax; i++)
            {
                items.Add(new SelectListItem { Text = RiDataWarehouseBo.GetPolicyStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownPolicyStatus = items;
            return items;
        }


        [HttpPost]
        public JsonResult SearchFromReferralClaim(string insuredName, string policyNumber, string treatyCode = null, string cedingPlanCode = null, string dateOfBirthStr = null)
        {
            DateTime? dateOfBirth = Util.GetParseDateTime(dateOfBirthStr);

            IList<RiDataWarehouseBo> riDataBos = RiDataWarehouseService.GetByReferralClaimParam(insuredName, policyNumber, treatyCode, cedingPlanCode, dateOfBirth);

            return Json(new { riDataBos });
        }

        [HttpPost]
        public JsonResult SearchFromClaimRegister(
            string policyNumber,
            string cedingPlanCode,
            int? riskYear,
            int? riskMonth,
            string soaQuarter,
            string mlreBenefitCode,
            string cedingBenefitRiskCode,
            string treatyCode,
            string dateOfEventStr
        )
        {
            DateTime? dateOfEvent = Util.GetParseDateTime(dateOfEventStr);

            IList<RiDataWarehouseBo> riDataWarehouseBos = RiDataWarehouseService.GetByClaimRegisterParam(policyNumber, cedingPlanCode, riskYear, riskMonth, soaQuarter, mlreBenefitCode, cedingBenefitRiskCode, treatyCode, dateOfEvent);
            if (riDataWarehouseBos.IsNullOrEmpty())
            {
                // Search by old treaty code
                var oldTreatyCode = TreatyOldCodeService.GetByTreatyCode(treatyCode);
                if (!string.IsNullOrEmpty(oldTreatyCode))
                {
                    riDataWarehouseBos = RiDataWarehouseService.GetByClaimRegisterParam(policyNumber, cedingPlanCode, riskYear, riskMonth, soaQuarter, mlreBenefitCode, cedingBenefitRiskCode, oldTreatyCode, dateOfEvent);
                }
            }

            return Json(new { riDataWarehouseBos });
        }
    }
}