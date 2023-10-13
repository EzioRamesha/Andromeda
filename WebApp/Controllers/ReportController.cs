using BusinessObject;
using BusinessObject.TreatyPricing;
using ConsoleApp.Commands;
using BusinessObject.InvoiceRegisters;
using Microsoft.Reporting.WebForms;
using Services;
using Services.SoaDatas;
using Services.TreatyPricing;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using BusinessObject.SoaDatas;
using Services.InvoiceRegisters;

namespace WebApp.Controllers
{
    public class ReportController : BaseController
    {
        // GET: Report
        [Obsolete]
        public ActionResult Index(int moduleId = 0)
        {
            ModuleBo moduleBo = ModuleService.Find(moduleId);
            if (moduleBo == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ModuleBo = moduleBo;

            // SSRS
            if (!string.IsNullOrEmpty(moduleBo.ReportPath))
            {
                if (moduleBo.HideParameters)
                {
                    LoadSSRSPage(moduleBo.Controller);
                    return View(moduleBo.Controller);
                }
                else
                {
                    if (moduleBo.ReportPath.Contains("KPIMonitoringReport") || moduleBo.ReportPath.Contains("GroupAuthorityLimitListing") || moduleBo.ReportPath.Contains("TreatyWeeklyMonthlyQuarterlyReport"))
                    {
                        Report(string.Format("/{0}", moduleBo.ReportPath), false, null, true);
                    }
                    else
                    {
                        Report(string.Format("/{0}", moduleBo.ReportPath));
                    }

                    return View();
                }
            }
            // Non-SSRS
            else
            {
                LoadPage(moduleBo.Controller);
                return View(moduleBo.Controller);
            }
        }

        public void LoadSSRSPage(string controller)
        {
            switch (controller)
            {
                case "PremiumInfoSoaDataRiSummaryReport":
                    LoadPremiumInfoSoaDataRiSummaryReportParameters();
                    break;
                case "PremiumInfoSoaDataRiSummarySnapshotReport":
                    LoadPremiumInfoSoaDataRiSummarySnapshotReportParameters();
                    break;
                case "PremiumInfoInvoiceRegisterReport":
                    LoadPremiumInfoInvoiceRegisterReportParameters();
                    break;
                case "PremiumInfoInvoiceRegisterSnapshotReport":
                    LoadPremiumInfoInvoiceRegisterSnapshotReportParameters();
                    break;
                case "PremiumInfoRetroRegisterReport":
                    LoadPremiumInfoRetroRegisterReportParameters();
                    break;
                case "PremiumInfoRetroRegisterSnapshotReport":
                    LoadPremiumInfoRetroRegisterSnapshotReportParameters();
                    break;
                case "PremiumProjectionReport":
                    LoadPremiumProjectionReportParameters();
                    break;
                case "PremiumProjectionSnapshotReport":
                    LoadPremiumProjectionSnapshotReportParameters();
                    break;
                case "PremiumInfoByMfrs17CellNameReport":
                    LoadPremiumInfoByMfrs17CellNameReportParameters();
                    break;
                case "PremiumInfoByMfrs17CellNameSnapshotReport":
                    LoadPremiumInfoByMfrs17CellNameSnapshotReportParameters();
                    break;
                default:
                    break;
            }
        }

        #region Valuation Report

        #region Premium Info (SOA Data - RI Summary) Report
        [HttpPost, Obsolete]
        public ActionResult GeneratePremiumInfoSoaDataRiSummaryReport(
            int typeOfReport,
            int? typeOfField,
            int? sumOf,
            string businessOrigin,
            string cedant,
            string treatyCode,
            string treatyType,
            string treatyMode,
            string riskQuarter,
            string soaQuarter,
            string planBlock
        )
        {
            ReportParameter[] param = new ReportParameter[11];
            param[0] = new ReportParameter("TypeId", typeOfReport.ToString());
            param[1] = new ReportParameter("FieldType", typeOfField.HasValue ? typeOfField.ToString() : null);
            param[2] = new ReportParameter("SumOf", sumOf.HasValue ? sumOf.ToString() : null);
            param[3] = new ReportParameter("BusinessOrigin", businessOrigin.Split(',').Select(q => q.Trim()).ToArray());
            param[4] = new ReportParameter("Cedant", cedant.Split(',').Select(q => q.Trim()).ToArray());
            param[5] = new ReportParameter("TreatyCode", treatyCode.Split(',').Select(q => q.Trim()).ToArray());
            param[6] = new ReportParameter("TreatyType", treatyType.Split(',').Select(q => q.Trim()).ToArray());
            param[7] = new ReportParameter("TreatyMode", treatyMode.Split(',').Select(q => q.Trim()).ToArray());
            param[8] = new ReportParameter("RiskQuarter", riskQuarter.Split(',').Select(q => q.Trim()).ToArray());
            param[9] = new ReportParameter("SoaQuarter", soaQuarter.Split(',').Select(q => q.Trim()).ToArray());
            param[10] = new ReportParameter("PlanBlock", planBlock.Split(',').Select(q => q.Trim()).ToArray());

            Report(string.Format("/{0}", ModuleBo.ModuleController.PremiumInfoSoaDataRiSummaryReport.ToString()), true, param);

            return PartialView("_SSRSWithoutParameters");
        }

        public void LoadPremiumInfoSoaDataRiSummaryReportParameters()
        {
            var bos = SoaDataService.SoaDataReportParams();

            DropdownSoaDataBatchCedant(bos);
            DropdownSoaDataTreatyCode(bos);
            DropdownSoaDataTreatyType(bos);
            DropdownSoaDataRiskQuarter(bos);
            DropdownSoaDataSoaQuarter(bos);
            DropdownSoaDataTreatyMode(bos);
            DropdownSoaDataPlan(bos);

            DropdownSoaDataIndividualSumOf();
            DropdownSoaDataGroupSumOf();
        }

        public IList<SelectListItem> DropdownSoaDataIndividualSumOf()
        {
            var items = GetEmptyDropDownList(false);
            items.Add(new SelectListItem { Text = "NB Premium", Value = "1" });
            items.Add(new SelectListItem { Text = "RN Premium", Value = "2" });
            items.Add(new SelectListItem { Text = "ALT Premium", Value = "3" });
            items.Add(new SelectListItem { Text = "Risk Premium", Value = "4" });
            items.Add(new SelectListItem { Text = "Total Discount", Value = "5" });
            items.Add(new SelectListItem { Text = "Total Commision", Value = "6" });
            items.Add(new SelectListItem { Text = "Surrender Value", Value = "7" });
            ViewBag.DropdownIndividualSumOfs = items;
            return items;
        }

        public IList<SelectListItem> DropdownSoaDataGroupSumOf()
        {
            var items = GetEmptyDropDownList(false);
            items.Add(new SelectListItem { Text = "Reinsurance Premium", Value = "8" });
            items.Add(new SelectListItem { Text = "Risk Premium", Value = "4" });
            items.Add(new SelectListItem { Text = "Total Discount", Value = "5" });
            items.Add(new SelectListItem { Text = "Total Commision", Value = "6" });
            items.Add(new SelectListItem { Text = "Surrender Value", Value = "7" });
            ViewBag.DropdownGroupSumOfs = items;
            return items;
        }

        public IList<SelectListItem> DropdownSoaDataBatchCedant(IList<SoaDataBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.SoaDataBatchBo.CedantId, q.SoaDataBatchBo.CedantBo.Name }))
            {
                items.Add(new SelectListItem { Text = bo.Key.Name, Value = bo.Key.CedantId.ToString() });
            }
            ViewBag.DropdownSoaDataBatchCedants = items;
            return items;
        }

        public IList<SelectListItem> DropdownSoaDataTreatyCode(IList<SoaDataBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.TreatyCode }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.TreatyCode) ? "(Blank)" : bo.Key.TreatyCode), Value = (string.IsNullOrEmpty(bo.Key.TreatyCode) ? "NULL" : bo.Key.TreatyCode) });
            }
            ViewBag.DropdownSoaDataTreatyCodes = items;
            return items;
        }

        public IList<SelectListItem> DropdownSoaDataTreatyType(IList<SoaDataBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.TreatyType }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.TreatyType) ? "(Blank)" : bo.Key.TreatyType), Value = (string.IsNullOrEmpty(bo.Key.TreatyType) ? "NULL" : bo.Key.TreatyType) });
            }
            ViewBag.DropdownSoaDataTreatyTypes = items;
            return items;
        }

        public IList<SelectListItem> DropdownSoaDataRiskQuarter(IList<SoaDataBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.RiskQuarter }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.RiskQuarter) ? "(Blank)" : bo.Key.RiskQuarter), Value = (string.IsNullOrEmpty(bo.Key.RiskQuarter) ? "NULL" : bo.Key.RiskQuarter) });
            }
            ViewBag.DropdownSoaDataRiskQuarters = items;
            return items;
        }

        public IList<SelectListItem> DropdownSoaDataSoaQuarter(IList<SoaDataBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.SoaQuarter }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.SoaQuarter) ? "(Blank)" : bo.Key.SoaQuarter), Value = (string.IsNullOrEmpty(bo.Key.SoaQuarter) ? "NULL" : bo.Key.SoaQuarter) });
            }
            ViewBag.DropdownSoaDataSoaQuarters = items;
            return items;
        }

        public IList<SelectListItem> DropdownSoaDataTreatyMode(IList<SoaDataBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.TreatyMode }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.TreatyMode) ? "(Blank)" : bo.Key.TreatyMode), Value = (string.IsNullOrEmpty(bo.Key.TreatyMode) ? "NULL" : bo.Key.TreatyMode) });
            }
            ViewBag.DropdownSoaDataTreatyModes = items;
            return items;
        }

        public IList<SelectListItem> DropdownSoaDataPlan(IList<SoaDataBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.PlanBlock }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.PlanBlock) ? "(Blank)" : bo.Key.PlanBlock), Value = (string.IsNullOrEmpty(bo.Key.PlanBlock) ? "NULL" : bo.Key.PlanBlock) });
            }
            ViewBag.DropdownSoaDataPlans = items;
            return items;
        }
        #endregion

        #region Premium Info (SOA Data - RI Summary) Report (Snapshot)
        [HttpPost, Obsolete]
        public ActionResult GeneratePremiumInfoSoaDataRiSummarySnapshotReport(
            int typeOfReport,
            int? typeOfField,
            int? sumOf,
            string businessOrigin,
            string cedant,
            string treatyCode,
            string treatyType,
            string treatyMode,
            string riskQuarter,
            string soaQuarter,
            string planBlock,
            int? cutOff
        )
        {
            ReportParameter[] param = new ReportParameter[12];
            param[0] = new ReportParameter("TypeId", typeOfReport.ToString());
            param[1] = new ReportParameter("FieldType", typeOfField.HasValue ? typeOfField.ToString() : null);
            param[2] = new ReportParameter("SumOf", sumOf.HasValue ? sumOf.ToString() : null);
            param[3] = new ReportParameter("BusinessOrigin", businessOrigin.Split(',').Select(q => q.Trim()).ToArray());
            param[4] = new ReportParameter("Cedant", cedant.Split(',').Select(q => q.Trim()).ToArray());
            param[5] = new ReportParameter("TreatyCode", treatyCode.Split(',').Select(q => q.Trim()).ToArray());
            param[6] = new ReportParameter("TreatyType", treatyType.Split(',').Select(q => q.Trim()).ToArray());
            param[7] = new ReportParameter("TreatyMode", treatyMode.Split(',').Select(q => q.Trim()).ToArray());
            param[8] = new ReportParameter("RiskQuarter", riskQuarter.Split(',').Select(q => q.Trim()).ToArray());
            param[9] = new ReportParameter("SoaQuarter", soaQuarter.Split(',').Select(q => q.Trim()).ToArray());
            param[10] = new ReportParameter("PlanBlock", planBlock.Split(',').Select(q => q.Trim()).ToArray());
            param[11] = new ReportParameter("ReportingQuarter", cutOff.HasValue ? cutOff.ToString() : null);

            Report(string.Format("/{0}", ModuleBo.ModuleController.PremiumInfoSoaDataRiSummarySnapshotReport.ToString()), true, param);

            return PartialView("_SSRSWithoutParameters");
        }

        public void LoadPremiumInfoSoaDataRiSummarySnapshotReportParameters()
        {
            var bos = SoaDataHistoryService.SoaDataHistoryReportParams();

            DropDownCutOffQuarter();
            DropdownSoaDataBatchSnapshotCedant(bos);
            DropdownSoaDataSnapshotTreatyCode(bos);
            DropdownSoaDataSnapshotTreatyType(bos);
            DropdownSoaDataSnapshotRiskQuarter(bos);
            DropdownSoaDataSnapshotSoaQuarter(bos);
            DropdownSoaDataSnapshotTreatyMode(bos);
            DropdownSoaDataSnapshotPlan(bos);

            DropdownSoaDataSnapshotIndividualSumOf();
            DropdownSoaDataSnapshotGroupSumOf();
        }

        public IList<SelectListItem> DropdownSoaDataSnapshotIndividualSumOf()
        {
            var items = GetEmptyDropDownList(false);
            items.Add(new SelectListItem { Text = "NB Premium", Value = "1" });
            items.Add(new SelectListItem { Text = "RN Premium", Value = "2" });
            items.Add(new SelectListItem { Text = "ALT Premium", Value = "3" });
            items.Add(new SelectListItem { Text = "Risk Premium", Value = "4" });
            items.Add(new SelectListItem { Text = "Total Discount", Value = "5" });
            items.Add(new SelectListItem { Text = "Total Commision", Value = "6" });
            items.Add(new SelectListItem { Text = "Surrender Value", Value = "7" });
            ViewBag.DropdownIndividualSumOfs = items;
            return items;
        }

        public IList<SelectListItem> DropdownSoaDataSnapshotGroupSumOf()
        {
            var items = GetEmptyDropDownList(false);
            items.Add(new SelectListItem { Text = "Reinsurance Premium", Value = "8" });
            items.Add(new SelectListItem { Text = "Risk Premium", Value = "4" });
            items.Add(new SelectListItem { Text = "Total Discount", Value = "5" });
            items.Add(new SelectListItem { Text = "Total Commision", Value = "6" });
            items.Add(new SelectListItem { Text = "Surrender Value", Value = "7" });
            ViewBag.DropdownGroupSumOfs = items;
            return items;
        }

        public IList<SelectListItem> DropdownSoaDataBatchSnapshotCedant(IList<SoaDataHistoryBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.SoaDataBatchHistoryBo.CedantId, q.SoaDataBatchHistoryBo.CedantBo.Name }))
            {
                items.Add(new SelectListItem { Text = bo.Key.Name, Value = bo.Key.CedantId.ToString() });
            }
            ViewBag.DropdownSoaDataBatchSnapshotCedants = items;
            return items;
        }

        public IList<SelectListItem> DropdownSoaDataSnapshotTreatyCode(IList<SoaDataHistoryBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.TreatyCode }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.TreatyCode) ? "(Blank)" : bo.Key.TreatyCode), Value = (string.IsNullOrEmpty(bo.Key.TreatyCode) ? "NULL" : bo.Key.TreatyCode) });
            }
            ViewBag.DropdownSoaDataSnapshotTreatyCodes = items;
            return items;
        }

        public IList<SelectListItem> DropdownSoaDataSnapshotTreatyType(IList<SoaDataHistoryBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.TreatyType }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.TreatyType) ? "(Blank)" : bo.Key.TreatyType), Value = (string.IsNullOrEmpty(bo.Key.TreatyType) ? "NULL" : bo.Key.TreatyType) });
            }
            ViewBag.DropdownSoaDataSnapshotTreatyTypes = items;
            return items;
        }

        public IList<SelectListItem> DropdownSoaDataSnapshotRiskQuarter(IList<SoaDataHistoryBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.RiskQuarter }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.RiskQuarter) ? "(Blank)" : bo.Key.RiskQuarter), Value = (string.IsNullOrEmpty(bo.Key.RiskQuarter) ? "NULL" : bo.Key.RiskQuarter) });
            }
            ViewBag.DropdownSoaDataSnapshotRiskQuarters = items;
            return items;
        }

        public IList<SelectListItem> DropdownSoaDataSnapshotSoaQuarter(IList<SoaDataHistoryBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.SoaQuarter }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.SoaQuarter) ? "(Blank)" : bo.Key.SoaQuarter), Value = (string.IsNullOrEmpty(bo.Key.SoaQuarter) ? "NULL" : bo.Key.SoaQuarter) });
            }
            ViewBag.DropdownSoaDataSnapshotSoaQuarters = items;
            return items;
        }

        public IList<SelectListItem> DropdownSoaDataSnapshotTreatyMode(IList<SoaDataHistoryBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.TreatyMode }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.TreatyMode) ? "(Blank)" : bo.Key.TreatyMode), Value = (string.IsNullOrEmpty(bo.Key.TreatyMode) ? "NULL" : bo.Key.TreatyMode) });
            }
            ViewBag.DropdownSoaDataSnapshotTreatyModes = items;
            return items;
        }

        public IList<SelectListItem> DropdownSoaDataSnapshotPlan(IList<SoaDataHistoryBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.PlanBlock }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.PlanBlock) ? "(Blank)" : bo.Key.PlanBlock), Value = (string.IsNullOrEmpty(bo.Key.PlanBlock) ? "NULL" : bo.Key.PlanBlock) });
            }
            ViewBag.DropdownSoaDataSnapshotPlans = items;
            return items;
        }
        #endregion

        #region Premium Info (Invoice Register) Report
        [HttpPost, Obsolete]
        public ActionResult GeneratePremiumInfoInvoiceRegisterReport(
            int typeOfReport,
            int? typeOfField,
            int? sumOf,
            string invoiceType,
            string invoiceStartDate,
            string invoiceEndDate,
            string cedant,
            string treatyCode,
            string treatyType,
            string riskQuarter,
            string soaQuarter,
            string frequency
        )
        {
            DateTime? InvoiceStartDate = Util.GetParseDateTime(invoiceStartDate);
            DateTime? InvoiceEndDate = Util.GetParseDateTime(invoiceEndDate);

            ReportParameter[] param = new ReportParameter[12];
            param[0] = new ReportParameter("TypeId", typeOfReport.ToString());
            param[1] = new ReportParameter("FieldType", typeOfField.HasValue ? typeOfField.ToString() : null);
            param[2] = new ReportParameter("SumOf", sumOf.HasValue ? sumOf.ToString() : null);
            param[3] = new ReportParameter("InvoiceType", invoiceType.Split(',').Select(q => q.Trim()).ToArray());
            param[4] = new ReportParameter("InvoiceStartDate", InvoiceStartDate?.ToString());
            param[5] = new ReportParameter("InvoiceEndDate", InvoiceEndDate?.ToString());
            param[6] = new ReportParameter("CedantId", cedant.Split(',').Select(q => q.Trim()).ToArray());
            param[7] = new ReportParameter("TreatyCodeId", treatyCode.Split(',').Select(q => q.Trim()).ToArray());
            param[8] = new ReportParameter("TreatyTypeId", treatyType.Split(',').Select(q => q.Trim()).ToArray());
            param[9] = new ReportParameter("RiskQuarter", riskQuarter.Split(',').Select(q => q.Trim()).ToArray());
            param[10] = new ReportParameter("SoaQuarter", soaQuarter.Split(',').Select(q => q.Trim()).ToArray());
            param[11] = new ReportParameter("Frequency", frequency.Split(',').Select(q => q.Trim()).ToArray());

            Report(string.Format("/{0}", ModuleBo.ModuleController.PremiumInfoInvoiceRegisterReport.ToString()), true, param);

            return PartialView("_SSRSWithoutParameters");
        }

        public void LoadPremiumInfoInvoiceRegisterReportParameters()
        {
            var bos = InvoiceRegisterService.InvoiceRegisterReportParams(InvoiceRegisterBo.ReportingTypeIFRS4);

            DropdownInvoiceRegisterInvoiceType();
            DropdownInvoiceRegisterCedant(bos);
            DropdownInvoiceRegisterTreatyCode(bos);
            DropdownInvoiceRegisterTreatyType();
            DropdownInvoiceRegisterRiskQuarter(bos);
            DropdownInvoiceRegisterSoaQuarter(bos);
            DropdownInvoiceRegisterFrequency(bos);

            DropdownInvoiceRegisterIndividualSumOf();
            DropdownInvoiceRegisterGroupSumOf();
        }

        public IList<SelectListItem> DropdownInvoiceRegisterIndividualSumOf()
        {
            var items = GetEmptyDropDownList(false);
            items.Add(new SelectListItem { Text = "Valuation - Gross-1st", Value = "1" });
            items.Add(new SelectListItem { Text = "Valuation - Gross-Ren", Value = "2" });
            items.Add(new SelectListItem { Text = "Valuation - Risk Premium", Value = "3" });
            items.Add(new SelectListItem { Text = "Valuation - Discount-1st", Value = "4" });
            items.Add(new SelectListItem { Text = "Valuation - Discount-Ren", Value = "5" });
            items.Add(new SelectListItem { Text = "Valuation - Com-1st", Value = "6" });
            items.Add(new SelectListItem { Text = "Valuation - Com-Ren", Value = "7" });
            items.Add(new SelectListItem { Text = "Valuation - Profit Commission", Value = "8" });
            items.Add(new SelectListItem { Text = "Valuation - Claims", Value = "9" });
            items.Add(new SelectListItem { Text = "Surrender Value", Value = "10" });
            ViewBag.DropdownIndividualSumOfs = items;
            return items;
        }

        public IList<SelectListItem> DropdownInvoiceRegisterGroupSumOf()
        {
            var items = GetEmptyDropDownList(false);
            items.Add(new SelectListItem { Text = "Reinsurance Premium", Value = "11" });
            items.Add(new SelectListItem { Text = "Valuation - Risk Premium", Value = "3" });
            items.Add(new SelectListItem { Text = "Reinsurance Discount", Value = "12" });
            items.Add(new SelectListItem { Text = "Reinsurance Commission", Value = "13" });
            items.Add(new SelectListItem { Text = "Valuation - Profit Commission", Value = "8" });
            items.Add(new SelectListItem { Text = "Valuation - Claims", Value = "9" });
            items.Add(new SelectListItem { Text = "Surrender Value", Value = "10" });
            ViewBag.DropdownGroupSumOfs = items;
            return items;
        }

        public IList<SelectListItem> DropdownInvoiceRegisterInvoiceType()
        {
            var items = GetEmptyDropDownList(false);
            foreach (var i in Enumerable.Range(1, InvoiceRegisterBo.InvoiceTypeMax))
            {
                items.Add(new SelectListItem { Text = InvoiceRegisterBo.GetInvoiceTypeName(i), Value = i.ToString() });
            }
            ViewBag.DropdownInvoiceRegisterInvoiceTypes = items;
            return items;
        }

        public IList<SelectListItem> DropdownInvoiceRegisterCedant(IList<InvoiceRegisterBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.CedantId, q.CedantBo.Name }))
            {
                items.Add(new SelectListItem { Text = bo.Key.Name, Value = bo.Key.CedantId.ToString() });
            }
            ViewBag.DropdownInvoiceRegisterCedants = items;
            return items;
        }

        public IList<SelectListItem> DropdownInvoiceRegisterTreatyCode(IList<InvoiceRegisterBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.TreatyCodeId, q.TreatyCodeBo.Code }))
            {
                items.Add(new SelectListItem { Text = bo.Key.Code, Value = bo.Key.TreatyCodeId.ToString() });
            }
            ViewBag.DropdownInvoiceRegisterTreatyCodes = items;
            return items;
        }

        public IList<SelectListItem> DropdownInvoiceRegisterTreatyType()
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in PickListDetailService.GetByPickListId(PickListBo.TreatyType))
            {
                items.Add(new SelectListItem { Text = bo.Code, Value = bo.Id.ToString() });
            }
            ViewBag.DropdownInvoiceRegisterTreatyTypes = items;
            return items;
        }

        public IList<SelectListItem> DropdownInvoiceRegisterRiskQuarter(IList<InvoiceRegisterBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.RiskQuarter }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.RiskQuarter) ? "(Blank)" : bo.Key.RiskQuarter), Value = (string.IsNullOrEmpty(bo.Key.RiskQuarter) ? "NULL" : bo.Key.RiskQuarter) });
            }
            ViewBag.DropdownInvoiceRegisterRiskQuarters = items;
            return items;
        }

        public IList<SelectListItem> DropdownInvoiceRegisterSoaQuarter(IList<InvoiceRegisterBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.SoaQuarter }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.SoaQuarter) ? "(Blank)" : bo.Key.SoaQuarter), Value = (string.IsNullOrEmpty(bo.Key.SoaQuarter) ? "NULL" : bo.Key.SoaQuarter) });
            }
            ViewBag.DropdownInvoiceRegisterSoaQuarters = items;
            return items;
        }

        public IList<SelectListItem> DropdownInvoiceRegisterFrequency(IList<InvoiceRegisterBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.Frequency }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.Frequency) ? "(Blank)" : bo.Key.Frequency), Value = (string.IsNullOrEmpty(bo.Key.Frequency) ? "NULL" : bo.Key.Frequency) });
            }
            ViewBag.DropdownInvoiceRegisterFrequencys = items;
            return items;
        }
        #endregion

        #region Premium Info (Invoice Register) Report (Snapshot)
        [HttpPost, Obsolete]
        public ActionResult GeneratePremiumInfoInvoiceRegisterSnapshotReport(
            int typeOfReport,
            int? typeOfField,
            int? sumOf,
            string invoiceType,
            string invoiceStartDate,
            string invoiceEndDate,
            string cedant,
            string treatyCode,
            string treatyType,
            string riskQuarter,
            string soaQuarter,
            string frequency,
            int? cutOff
        )
        {
            DateTime? InvoiceStartDate = Util.GetParseDateTime(invoiceStartDate);
            DateTime? InvoiceEndDate = Util.GetParseDateTime(invoiceEndDate);

            ReportParameter[] param = new ReportParameter[13];
            param[0] = new ReportParameter("TypeId", typeOfReport.ToString());
            param[1] = new ReportParameter("FieldType", typeOfField.HasValue ? typeOfField.ToString() : null);
            param[2] = new ReportParameter("SumOf", sumOf.HasValue ? sumOf.ToString() : null);
            param[3] = new ReportParameter("InvoiceType", invoiceType.Split(',').Select(q => q.Trim()).ToArray());
            param[4] = new ReportParameter("InvoiceStartDate", InvoiceStartDate?.ToString());
            param[5] = new ReportParameter("InvoiceEndDate", InvoiceEndDate?.ToString());
            param[6] = new ReportParameter("CedantId", cedant.Split(',').Select(q => q.Trim()).ToArray());
            param[7] = new ReportParameter("TreatyCodeId", treatyCode.Split(',').Select(q => q.Trim()).ToArray());
            param[8] = new ReportParameter("TreatyTypeId", treatyType.Split(',').Select(q => q.Trim()).ToArray());
            param[9] = new ReportParameter("RiskQuarter", riskQuarter.Split(',').Select(q => q.Trim()).ToArray());
            param[10] = new ReportParameter("SoaQuarter", soaQuarter.Split(',').Select(q => q.Trim()).ToArray());
            param[11] = new ReportParameter("Frequency", frequency.Split(',').Select(q => q.Trim()).ToArray());
            param[12] = new ReportParameter("ReportingQuarter", cutOff.HasValue ? cutOff.ToString() : null);

            Report(string.Format("/{0}", ModuleBo.ModuleController.PremiumInfoInvoiceRegisterSnapshotReport.ToString()), true, param);

            return PartialView("_SSRSWithoutParameters");
        }

        public void LoadPremiumInfoInvoiceRegisterSnapshotReportParameters()
        {
            var bos = InvoiceRegisterHistoryService.InvoiceRegisterHistoryReportParams(InvoiceRegisterHistoryBo.ReportingTypeIFRS17);

            DropDownCutOffQuarter();
            DropdownInvoiceRegisterSnapshotInvoiceType();
            DropdownInvoiceRegisterSnapshotCedant(bos);
            DropdownInvoiceRegisterSnapshotTreatyCode(bos);
            DropdownInvoiceRegisterSnapshotTreatyType();
            DropdownInvoiceRegisterSnapshotRiskQuarter(bos);
            DropdownInvoiceRegisterSnapshotSoaQuarter(bos);
            DropdownInvoiceRegisterSnapshotFrequency(bos);

            DropdownInvoiceRegisterSnapshotIndividualSumOf();
            DropdownInvoiceRegisterSnapshotGroupSumOf();
        }

        public IList<SelectListItem> DropdownInvoiceRegisterSnapshotIndividualSumOf()
        {
            var items = GetEmptyDropDownList(false);
            items.Add(new SelectListItem { Text = "Valuation - Gross-1st", Value = "1" });
            items.Add(new SelectListItem { Text = "Valuation - Gross-Ren", Value = "2" });
            items.Add(new SelectListItem { Text = "Valuation - Risk Premium", Value = "3" });
            items.Add(new SelectListItem { Text = "Valuation - Discount-1st", Value = "4" });
            items.Add(new SelectListItem { Text = "Valuation - Discount-Ren", Value = "5" });
            items.Add(new SelectListItem { Text = "Valuation - Com-1st", Value = "6" });
            items.Add(new SelectListItem { Text = "Valuation - Com-Ren", Value = "7" });
            items.Add(new SelectListItem { Text = "Valuation - Profit Commission", Value = "8" });
            items.Add(new SelectListItem { Text = "Valuation - Claims", Value = "9" });
            items.Add(new SelectListItem { Text = "Surrender Value", Value = "10" });
            ViewBag.DropdownIndividualSumOfs = items;
            return items;
        }

        public IList<SelectListItem> DropdownInvoiceRegisterSnapshotGroupSumOf()
        {
            var items = GetEmptyDropDownList(false);
            items.Add(new SelectListItem { Text = "Reinsurance Premium", Value = "11" });
            items.Add(new SelectListItem { Text = "Valuation - Risk Premium", Value = "3" });
            items.Add(new SelectListItem { Text = "Reinsurance Discount", Value = "12" });
            items.Add(new SelectListItem { Text = "Reinsurance Commission", Value = "13" });
            items.Add(new SelectListItem { Text = "Valuation - Profit Commission", Value = "8" });
            items.Add(new SelectListItem { Text = "Valuation - Claims", Value = "9" });
            items.Add(new SelectListItem { Text = "Surrender Value", Value = "10" });
            ViewBag.DropdownGroupSumOfs = items;
            return items;
        }

        public IList<SelectListItem> DropdownInvoiceRegisterSnapshotInvoiceType()
        {
            var items = GetEmptyDropDownList(false);
            foreach (var i in Enumerable.Range(1, InvoiceRegisterBo.InvoiceTypeMax))
            {
                items.Add(new SelectListItem { Text = InvoiceRegisterBo.GetInvoiceTypeName(i), Value = i.ToString() });
            }
            ViewBag.DropdownInvoiceRegisterSnapshotInvoiceTypes = items;
            return items;
        }

        public IList<SelectListItem> DropdownInvoiceRegisterSnapshotCedant(IList<InvoiceRegisterHistoryBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.CedantId, q.CedantBo.Name }))
            {
                items.Add(new SelectListItem { Text = bo.Key.Name, Value = bo.Key.CedantId.ToString() });
            }
            ViewBag.DropdownInvoiceRegisterSnapshotCedants = items;
            return items;
        }

        public IList<SelectListItem> DropdownInvoiceRegisterSnapshotTreatyCode(IList<InvoiceRegisterHistoryBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.TreatyCodeId, q.TreatyCodeBo.Code }))
            {
                items.Add(new SelectListItem { Text = bo.Key.Code, Value = bo.Key.TreatyCodeId.ToString() });
            }
            ViewBag.DropdownInvoiceRegisterSnapshotTreatyCodes = items;
            return items;
        }

        public IList<SelectListItem> DropdownInvoiceRegisterSnapshotTreatyType()
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in PickListDetailService.GetByPickListId(PickListBo.TreatyType))
            {
                items.Add(new SelectListItem { Text = bo.Code, Value = bo.Id.ToString() });
            }
            ViewBag.DropdownInvoiceRegisterSnapshotTreatyTypes = items;
            return items;
        }

        public IList<SelectListItem> DropdownInvoiceRegisterSnapshotRiskQuarter(IList<InvoiceRegisterHistoryBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.RiskQuarter }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.RiskQuarter) ? "(Blank)" : bo.Key.RiskQuarter), Value = (string.IsNullOrEmpty(bo.Key.RiskQuarter) ? "NULL" : bo.Key.RiskQuarter) });
            }
            ViewBag.DropdownInvoiceRegisterSnapshotRiskQuarters = items;
            return items;
        }

        public IList<SelectListItem> DropdownInvoiceRegisterSnapshotSoaQuarter(IList<InvoiceRegisterHistoryBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.SoaQuarter }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.SoaQuarter) ? "(Blank)" : bo.Key.SoaQuarter), Value = (string.IsNullOrEmpty(bo.Key.SoaQuarter) ? "NULL" : bo.Key.SoaQuarter) });
            }
            ViewBag.DropdownInvoiceRegisterSnapshotSoaQuarters = items;
            return items;
        }

        public IList<SelectListItem> DropdownInvoiceRegisterSnapshotFrequency(IList<InvoiceRegisterHistoryBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.Frequency }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.Frequency) ? "(Blank)" : bo.Key.Frequency), Value = (string.IsNullOrEmpty(bo.Key.Frequency) ? "NULL" : bo.Key.Frequency) });
            }
            ViewBag.DropdownInvoiceRegisterSnapshotFrequencys = items;
            return items;
        }
        #endregion

        #region Premium Info (Retro Register) Report
        [HttpPost, Obsolete]
        public ActionResult GeneratePremiumInfoRetroRegisterReport(
            int typeOfReport,
            int? typeOfField,
            int? sumOf,
            string invoiceType,
            string invoiceStartDate,
            string invoiceEndDate,
            string cedant,
            string treatyCode,
            string treatyType,
            string riskQuarter,
            string soaQuarter,
            string frequency,
            string retroParty
        )
        {
            DateTime? InvoiceStartDate = Util.GetParseDateTime(invoiceStartDate);
            DateTime? InvoiceEndDate = Util.GetParseDateTime(invoiceEndDate);

            ReportParameter[] param = new ReportParameter[13];
            param[0] = new ReportParameter("TypeId", typeOfReport.ToString());
            param[1] = new ReportParameter("FieldType", typeOfField.HasValue ? typeOfField.ToString() : null);
            param[2] = new ReportParameter("SumOf", sumOf.HasValue ? sumOf.ToString() : null);
            param[3] = new ReportParameter("InvoiceType", invoiceType.Split(',').Select(q => q.Trim()).ToArray());
            param[4] = new ReportParameter("InvoiceStartDate", InvoiceStartDate?.ToString());
            param[5] = new ReportParameter("InvoiceEndDate", InvoiceEndDate?.ToString());
            param[6] = new ReportParameter("CedantId", cedant.Split(',').Select(q => q.Trim()).ToArray());
            param[7] = new ReportParameter("TreatyCodeId", treatyCode.Split(',').Select(q => q.Trim()).ToArray());
            param[8] = new ReportParameter("TreatyType", treatyType.Split(',').Select(q => q.Trim()).ToArray());
            param[9] = new ReportParameter("RiskQuarter", riskQuarter.Split(',').Select(q => q.Trim()).ToArray());
            param[10] = new ReportParameter("SoaQuarter", soaQuarter.Split(',').Select(q => q.Trim()).ToArray());
            param[11] = new ReportParameter("Frequency", frequency.Split(',').Select(q => q.Trim()).ToArray());
            param[12] = new ReportParameter("RetroPartyId", retroParty.Split(',').Select(q => q.Trim()).ToArray());

            Report(string.Format("/{0}", ModuleBo.ModuleController.PremiumInfoRetroRegisterReport.ToString()), true, param);

            return PartialView("_SSRSWithoutParameters");
        }

        public void LoadPremiumInfoRetroRegisterReportParameters()
        {
            var bos = RetroRegisterService.RetroRegisterReportParams();

            DropdownRetroRegisterCedant(bos);
            DropdownRetroRegisterTreatyCode(bos);
            DropdownRetroRegisterTreatyType(bos);
            DropdownRetroRegisterRiskQuarter(bos);
            DropdownRetroRegisterSoaQuarter(bos);
            DropdownRetroRegisterFrequency(bos);
            DropdownRetroRegisterRetroParty(bos);

            DropdownRetroRegisterIndividualSumOf();
            DropdownRetroRegisterGroupSumOf();
        }

        public IList<SelectListItem> DropdownRetroRegisterIndividualSumOf()
        {
            var items = GetEmptyDropDownList(false);
            items.Add(new SelectListItem { Text = "Valuation - Gross-1st", Value = "1" });
            items.Add(new SelectListItem { Text = "Valuation - Gross-Ren", Value = "2" });
            items.Add(new SelectListItem { Text = "Valuation - Risk Premium", Value = "3" });
            items.Add(new SelectListItem { Text = "Valuation - Discount-1st", Value = "4" });
            items.Add(new SelectListItem { Text = "Valuation - Discount-Ren", Value = "5" });
            items.Add(new SelectListItem { Text = "Valuation - Com-1st", Value = "6" });
            items.Add(new SelectListItem { Text = "Valuation - Com-Ren", Value = "7" });
            items.Add(new SelectListItem { Text = "Valuation - Profit Commission", Value = "8" });
            items.Add(new SelectListItem { Text = "Valuation - Claims", Value = "9" });
            items.Add(new SelectListItem { Text = "Surrender Value", Value = "10" });
            ViewBag.DropdownIndividualSumOfs = items;
            return items;
        }

        public IList<SelectListItem> DropdownRetroRegisterGroupSumOf()
        {
            var items = GetEmptyDropDownList(false);
            items.Add(new SelectListItem { Text = "Reinsurance Premium", Value = "11" });
            items.Add(new SelectListItem { Text = "Valuation - Risk Premium", Value = "3" });
            items.Add(new SelectListItem { Text = "Reinsurance Discount", Value = "12" });
            items.Add(new SelectListItem { Text = "Reinsurance Commission", Value = "13" });
            items.Add(new SelectListItem { Text = "Valuation - Profit Commission", Value = "8" });
            items.Add(new SelectListItem { Text = "Valuation - Claims", Value = "9" });
            items.Add(new SelectListItem { Text = "Surrender Value", Value = "10" });
            ViewBag.DropdownGroupSumOfs = items;
            return items;
        }

        public IList<SelectListItem> DropdownRetroRegisterCedant(IList<RetroRegisterBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.CedantId, q.CedantBo?.Name }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.Name) ? "(Blank)" : bo.Key.Name), Value = (!bo.Key.CedantId.HasValue ? "0" : bo.Key.CedantId.ToString()) });
            }
            ViewBag.DropdownRetroRegisterCedants = items;
            return items;
        }

        public IList<SelectListItem> DropdownRetroRegisterTreatyCode(IList<RetroRegisterBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.TreatyCodeId, q.TreatyCodeBo?.Code }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.Code) ? "(Blank)" : bo.Key.Code), Value = (!bo.Key.TreatyCodeId.HasValue ? "0" : bo.Key.TreatyCodeId.ToString()) });
            }
            ViewBag.DropdownRetroRegisterTreatyCodes = items;
            return items;
        }

        public IList<SelectListItem> DropdownRetroRegisterTreatyType(IList<RetroRegisterBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.TreatyType }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.TreatyType) ? "(Blank)" : bo.Key.TreatyType), Value = (string.IsNullOrEmpty(bo.Key.TreatyType) ? "NULL" : bo.Key.TreatyType) });
            }
            ViewBag.DropdownRetroRegisterTreatyTypes = items;
            return items;
        }

        public IList<SelectListItem> DropdownRetroRegisterRiskQuarter(IList<RetroRegisterBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.RiskQuarter }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.RiskQuarter) ? "(Blank)" : bo.Key.RiskQuarter), Value = (string.IsNullOrEmpty(bo.Key.RiskQuarter) ? "NULL" : bo.Key.RiskQuarter) });
            }
            ViewBag.DropdownRetroRegisterRiskQuarters = items;
            return items;
        }

        public IList<SelectListItem> DropdownRetroRegisterSoaQuarter(IList<RetroRegisterBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.OriginalSoaQuarter }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.OriginalSoaQuarter) ? "(Blank)" : bo.Key.OriginalSoaQuarter), Value = (string.IsNullOrEmpty(bo.Key.OriginalSoaQuarter) ? "NULL" : bo.Key.OriginalSoaQuarter) });
            }
            ViewBag.DropdownRetroRegisterSoaQuarters = items;
            return items;
        }

        public IList<SelectListItem> DropdownRetroRegisterFrequency(IList<RetroRegisterBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.Frequency }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.Frequency) ? "(Blank)" : bo.Key.Frequency), Value = (string.IsNullOrEmpty(bo.Key.Frequency) ? "NULL" : bo.Key.Frequency) });
            }
            ViewBag.DropdownRetroRegisterFrequencys = items;
            return items;
        }

        public IList<SelectListItem> DropdownRetroRegisterRetroParty(IList<RetroRegisterBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.RetroPartyId, q.RetroPartyBo?.Name }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.Name) ? "(Blank)" : bo.Key.Name), Value = (!bo.Key.RetroPartyId.HasValue ? "0" : bo.Key.RetroPartyId.ToString()) });
            }
            ViewBag.DropdownRetroRegisterRetroPartys = items;
            return items;
        }
        #endregion

        #region Premium Info (Retro Register) Report (Snapshot)
        [HttpPost, Obsolete]
        public ActionResult GeneratePremiumInfoRetroRegisterSnapshotReport(
            int typeOfReport,
            int? typeOfField,
            int? sumOf,
            string invoiceType,
            string invoiceStartDate,
            string invoiceEndDate,
            string cedant,
            string treatyCode,
            string treatyType,
            string riskQuarter,
            string soaQuarter,
            string frequency,
            string retroParty,
            int? cutOff
        )
        {
            DateTime? InvoiceStartDate = Util.GetParseDateTime(invoiceStartDate);
            DateTime? InvoiceEndDate = Util.GetParseDateTime(invoiceEndDate);

            ReportParameter[] param = new ReportParameter[14];
            param[0] = new ReportParameter("TypeId", typeOfReport.ToString());
            param[1] = new ReportParameter("FieldType", typeOfField.HasValue ? typeOfField.ToString() : null);
            param[2] = new ReportParameter("SumOf", sumOf.HasValue ? sumOf.ToString() : null);
            param[3] = new ReportParameter("InvoiceType", invoiceType.Split(',').Select(q => q.Trim()).ToArray());
            param[4] = new ReportParameter("InvoiceStartDate", InvoiceStartDate?.ToString());
            param[5] = new ReportParameter("InvoiceEndDate", InvoiceEndDate?.ToString());
            param[6] = new ReportParameter("CedantId", cedant.Split(',').Select(q => q.Trim()).ToArray());
            param[7] = new ReportParameter("TreatyCodeId", treatyCode.Split(',').Select(q => q.Trim()).ToArray());
            param[8] = new ReportParameter("TreatyType", treatyType.Split(',').Select(q => q.Trim()).ToArray());
            param[9] = new ReportParameter("RiskQuarter", riskQuarter.Split(',').Select(q => q.Trim()).ToArray());
            param[10] = new ReportParameter("SoaQuarter", soaQuarter.Split(',').Select(q => q.Trim()).ToArray());
            param[11] = new ReportParameter("Frequency", frequency.Split(',').Select(q => q.Trim()).ToArray());
            param[12] = new ReportParameter("RetroPartyId", retroParty.Split(',').Select(q => q.Trim()).ToArray());
            param[13] = new ReportParameter("ReportingQuarter", cutOff.HasValue ? cutOff.ToString() : null);

            Report(string.Format("/{0}", ModuleBo.ModuleController.PremiumInfoRetroRegisterSnapshotReport.ToString()), true, param);

            return PartialView("_SSRSWithoutParameters");
        }

        public void LoadPremiumInfoRetroRegisterSnapshotReportParameters()
        {
            var bos = RetroRegisterHistoryService.RetroRegisterHistoryReportParams();

            DropDownCutOffQuarter();
            DropdownRetroRegisterSnapshotCedant(bos);
            DropdownRetroRegisterSnapshotTreatyCode(bos);
            DropdownRetroRegisterSnapshotTreatyType(bos);
            DropdownRetroRegisterSnapshotRiskQuarter(bos);
            DropdownRetroRegisterSnapshotSoaQuarter(bos);
            DropdownRetroRegisterSnapshotFrequency(bos);
            DropdownRetroRegisterSnapshotRetroParty(bos);

            DropdownRetroRegisterSnapshotIndividualSumOf();
            DropdownRetroRegisterSnapshotGroupSumOf();
        }

        public IList<SelectListItem> DropdownRetroRegisterSnapshotIndividualSumOf()
        {
            var items = GetEmptyDropDownList(false);
            items.Add(new SelectListItem { Text = "Valuation - Gross-1st", Value = "1" });
            items.Add(new SelectListItem { Text = "Valuation - Gross-Ren", Value = "2" });
            items.Add(new SelectListItem { Text = "Valuation - Risk Premium", Value = "3" });
            items.Add(new SelectListItem { Text = "Valuation - Discount-1st", Value = "4" });
            items.Add(new SelectListItem { Text = "Valuation - Discount-Ren", Value = "5" });
            items.Add(new SelectListItem { Text = "Valuation - Com-1st", Value = "6" });
            items.Add(new SelectListItem { Text = "Valuation - Com-Ren", Value = "7" });
            items.Add(new SelectListItem { Text = "Valuation - Profit Commission", Value = "8" });
            items.Add(new SelectListItem { Text = "Valuation - Claims", Value = "9" });
            items.Add(new SelectListItem { Text = "Surrender Value", Value = "10" });
            ViewBag.DropdownIndividualSumOfs = items;
            return items;
        }

        public IList<SelectListItem> DropdownRetroRegisterSnapshotGroupSumOf()
        {
            var items = GetEmptyDropDownList(false);
            items.Add(new SelectListItem { Text = "Reinsurance Premium", Value = "11" });
            items.Add(new SelectListItem { Text = "Valuation - Risk Premium", Value = "3" });
            items.Add(new SelectListItem { Text = "Reinsurance Discount", Value = "12" });
            items.Add(new SelectListItem { Text = "Reinsurance Commission", Value = "13" });
            items.Add(new SelectListItem { Text = "Valuation - Profit Commission", Value = "8" });
            items.Add(new SelectListItem { Text = "Valuation - Claims", Value = "9" });
            items.Add(new SelectListItem { Text = "Surrender Value", Value = "10" });
            ViewBag.DropdownGroupSumOfs = items;
            return items;
        }

        public IList<SelectListItem> DropdownRetroRegisterSnapshotCedant(IList<RetroRegisterHistoryBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.CedantId, q.CedantBo?.Name }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.Name) ? "(Blank)" : bo.Key.Name), Value = (!bo.Key.CedantId.HasValue ? "0" : bo.Key.CedantId.ToString()) });
            }
            ViewBag.DropdownRetroRegisterCedants = items;
            return items;
        }

        public IList<SelectListItem> DropdownRetroRegisterSnapshotTreatyCode(IList<RetroRegisterHistoryBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.TreatyCodeId, q.TreatyCodeBo?.Code }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.Code) ? "(Blank)" : bo.Key.Code), Value = (!bo.Key.TreatyCodeId.HasValue ? "0" : bo.Key.TreatyCodeId.ToString()) });
            }
            ViewBag.DropdownRetroRegisterTreatyCodes = items;
            return items;
        }

        public IList<SelectListItem> DropdownRetroRegisterSnapshotTreatyType(IList<RetroRegisterHistoryBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.TreatyType }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.TreatyType) ? "(Blank)" : bo.Key.TreatyType), Value = (string.IsNullOrEmpty(bo.Key.TreatyType) ? "NULL" : bo.Key.TreatyType) });
            }
            ViewBag.DropdownRetroRegisterTreatyTypes = items;
            return items;
        }

        public IList<SelectListItem> DropdownRetroRegisterSnapshotRiskQuarter(IList<RetroRegisterHistoryBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.RiskQuarter }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.RiskQuarter) ? "(Blank)" : bo.Key.RiskQuarter), Value = (string.IsNullOrEmpty(bo.Key.RiskQuarter) ? "NULL" : bo.Key.RiskQuarter) });
            }
            ViewBag.DropdownRetroRegisterRiskQuarters = items;
            return items;
        }

        public IList<SelectListItem> DropdownRetroRegisterSnapshotSoaQuarter(IList<RetroRegisterHistoryBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.OriginalSoaQuarter }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.OriginalSoaQuarter) ? "(Blank)" : bo.Key.OriginalSoaQuarter), Value = (string.IsNullOrEmpty(bo.Key.OriginalSoaQuarter) ? "NULL" : bo.Key.OriginalSoaQuarter) });
            }
            ViewBag.DropdownRetroRegisterSoaQuarters = items;
            return items;
        }

        public IList<SelectListItem> DropdownRetroRegisterSnapshotFrequency(IList<RetroRegisterHistoryBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.Frequency }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.Frequency) ? "(Blank)" : bo.Key.Frequency), Value = (string.IsNullOrEmpty(bo.Key.Frequency) ? "NULL" : bo.Key.Frequency) });
            }
            ViewBag.DropdownRetroRegisterFrequencys = items;
            return items;
        }

        public IList<SelectListItem> DropdownRetroRegisterSnapshotRetroParty(IList<RetroRegisterHistoryBo> bos)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in bos.GroupBy(q => new { q.RetroPartyId, q.RetroPartyBo?.Name }))
            {
                items.Add(new SelectListItem { Text = (string.IsNullOrEmpty(bo.Key.Name) ? "(Blank)" : bo.Key.Name), Value = (!bo.Key.RetroPartyId.HasValue ? "0" : bo.Key.RetroPartyId.ToString()) });
            }
            ViewBag.DropdownRetroRegisterRetroPartys = items;
            return items;
        }
        #endregion

        #region Premium Projection Report
        [HttpPost, Obsolete]
        public ActionResult GeneratePremiumProjectionReport(
            int typeOfReport,
            string treatyType
        )
        {
            ReportParameter[] param = new ReportParameter[2];
            param[0] = new ReportParameter("TypeId", typeOfReport.ToString());
            param[1] = new ReportParameter("TreatyType", treatyType.Split(',').Select(q => q.Trim()).ToArray());

            Report(string.Format("/{0}", ModuleBo.ModuleController.PremiumProjectionReport.ToString()), true, param);

            return PartialView("_SSRSWithoutParameters");
        }

        public void LoadPremiumProjectionReportParameters()
        {
            var soaDataBos = SoaDataService.SoaDataReportParams();
            DropdownSoaDataTreatyType(soaDataBos);

            DropdownInvoiceRegisterGPTreatyType();
        }

        public IList<SelectListItem> DropdownInvoiceRegisterGPTreatyType()
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in PickListDetailService.GetByPickListId(PickListBo.TreatyType))
            {
                items.Add(new SelectListItem { Text = bo.Code, Value = bo.Code });
            }
            ViewBag.DropdownInvoiceRegisterGPTreatyTypes = items;
            return items;
        }
        #endregion

        #region Premium Projection Report (Snapshot)
        [HttpPost, Obsolete]
        public ActionResult GeneratePremiumProjectionSnapshotReport(
            int typeOfReport,
            string treatyType,
            int? cutOff
        )
        {
            ReportParameter[] param = new ReportParameter[3];
            param[0] = new ReportParameter("TypeId", typeOfReport.ToString());
            param[1] = new ReportParameter("TreatyType", treatyType.Split(',').Select(q => q.Trim()).ToArray());
            param[2] = new ReportParameter("ReportingQuarter", cutOff.HasValue ? cutOff.ToString() : null);

            Report(string.Format("/{0}", ModuleBo.ModuleController.PremiumProjectionSnapshotReport.ToString()), true, param);

            return PartialView("_SSRSWithoutParameters");
        }

        public void LoadPremiumProjectionSnapshotReportParameters()
        {
            var soaDataHistoryBos = SoaDataHistoryService.SoaDataHistoryReportParams();
            DropdownSoaDataSnapshotTreatyType(soaDataHistoryBos);

            DropdownInvoiceRegisterSnapshotGPTreatyType();
            DropDownCutOffQuarter();
        }

        public IList<SelectListItem> DropdownInvoiceRegisterSnapshotGPTreatyType()
        {
            var items = GetEmptyDropDownList(false);
            foreach (var bo in PickListDetailService.GetByPickListId(PickListBo.TreatyType))
            {
                items.Add(new SelectListItem { Text = bo.Code, Value = bo.Code });
            }
            ViewBag.DropdownInvoiceRegisterSnapshotGPTreatyTypes = items;
            return items;
        }
        #endregion

        #region Premium Info by MFRS17 Cell Name Report
        [HttpPost, Obsolete]
        public ActionResult GeneratePremiumInfoByMfrs17CellNameReport(string invoiceType)
        {
            ReportParameter[] param = new ReportParameter[1];
            param[0] = new ReportParameter("InvoiceType", invoiceType.Split(',').Select(q => q.Trim()).ToArray());

            Report(string.Format("/{0}", ModuleBo.ModuleController.PremiumInfoByMfrs17CellNameReport.ToString()), true, param);

            return PartialView("_SSRSWithoutParameters");
        }

        public void LoadPremiumInfoByMfrs17CellNameReportParameters()
        {
            DropdownInvoiceRegisterInvoiceType();
        }
        #endregion

        #region Premium Info by MFRS17 Cell Name Report (Snapshot)
        [HttpPost, Obsolete]
        public ActionResult GeneratePremiumInfoByMfrs17CellNameSnapshotReport(
            string invoiceType,
            int? cutOff
        )
        {
            ReportParameter[] param = new ReportParameter[2];
            param[0] = new ReportParameter("InvoiceType", invoiceType.Split(',').Select(q => q.Trim()).ToArray());
            param[1] = new ReportParameter("ReportingQuarter", cutOff.HasValue ? cutOff.ToString() : null);

            Report(string.Format("/{0}", ModuleBo.ModuleController.PremiumInfoByMfrs17CellNameSnapshotReport.ToString()), true, param);

            return PartialView("_SSRSWithoutParameters");
        }

        public void LoadPremiumInfoByMfrs17CellNameSnapshotReportParameters()
        {
            DropDownCutOffQuarter();
            DropdownInvoiceRegisterSnapshotInvoiceType();
        }
        #endregion

        #endregion

        public void LoadPage(string controller)
        {
            switch (controller)
            {
                case "RateComparisonReport":
                case "RateComparisonPaReport":
                    DropDownTreatyPricingCedant();
                    DropDownEmpty();
                    break;
                case "UwLimitComparisonReport":
                case "AdvantageProgramComparisonReport":
                    GetTreatyPricingCedant();
                    break;
                case "DefinitionAndExclusionComparisonReport":
                    DropDownTreatyPricingCedant();
                    DropDownEmpty();
                    break;
                case "MedicalTableComparisonReport":
                    DropDownTreatyPricingCedant();
                    DropDownEmpty();
                    break;
                case "UwQuestionnaireComparisonReport":
                    DropDownTreatyPricingCedant();
                    DropDownEmpty();
                    break;
                case "NonMedicalTableComparisonReport":
                    DropDownTreatyPricingCedant();
                    DropDownEmpty();
                    break;
                case "FinancialTableComparisonReport":
                    DropDownTreatyPricingCedant();
                    DropDownEmpty();
                    break;
                case "ProductComparisonReport":
                    DropDownTreatyPricingCedant();
                    DropDownEmpty();
                    break;
                case "ProductAndBenefitDetailsReport":
                    DropDownTreatyPricingCedant();
                    DropDownCedant();
                    DropDownEmpty();
                    break;
                case "HipsComparisonReport":
                    DropDownTreatyPricingCedant();
                    DropDownCedant();
                    DropDownEmpty();
                    break;
                case "GtlRatesByUnitRateReport":
                    DropDownTreatyPricingCedant();
                    DropDownCedant();
                    DropDownEmpty();
                    break;
                case "GtlRatesByAgeBandedReport":
                    DropDownTreatyPricingCedant();
                    DropDownCedant();
                    DropDownEmpty();
                    break;
                case "GTLBasisOfSA":
                    DropDownTreatyPricingCedant();
                    DropDownCedant();
                    DropDownEmpty();
                    break;
                case "CampaignComparisonReport":
                    DropDownTreatyPricingCedant();
                    DropDownEmpty();
                    break;
                case "TargetPlanningStatementTrackingReport":
                    DropDownQuarter();
                    break;
                case "TargetPlanningPCStatementTrackingReport":
                    DropDownYear();
                    break;
                default:
                    break;
            }
        }

        #region Rate Table Comparison
        [HttpPost]
        public JsonResult UpdateData(
            int? treatyPricingCedantId,
            string underwritingMethod = null,
            string productName = null,
            int? productType = null,
            string targetSegment = null,
            string distributionChannel = null,
            int? benefitCode = null,
            string benefitName = null,
            string effectiveDate = null,
            int? rateTableId = null
        )
        {
            var underwritingMethodCodes = new List<string> { };
            var productNameDropDowns = GetEmptyDropDownList();
            var productTypeDropDowns = GetEmptyDropDownList();
            var targetSegmentCodes = new List<string> { };
            var distributionChannelCodes = new List<string> { };
            var benefitCodeDropDowns = GetEmptyDropDownList();
            var benefitNameDropDowns = GetEmptyDropDownList();
            var effectiveDateDropDowns = GetEmptyDropDownList();
            var rateTableIdDropDowns = GetEmptyDropDownList();
            var versionDropDowns = GetEmptyDropDownList();
            var productVersionIds = new List<int> { };

            if (treatyPricingCedantId.HasValue)
            {
                var productBos = TreatyPricingProductService.GetByTreatyPricingCedantId(treatyPricingCedantId.Value);
                var productIds = productBos.Select(q => q.Id).ToList();
                var rateTableVersionIds = new List<int> { };

                bool isProductVersion = false;

                bool isProductType = productType.HasValue;
                bool isTargetSegment = !string.IsNullOrEmpty(targetSegment);
                bool isDistributionChannel = !string.IsNullOrEmpty(distributionChannel);
                bool isBenefitCode = benefitCode.HasValue;
                bool isBenefitName = !string.IsNullOrEmpty(benefitName);
                bool isEffectiveDate = !string.IsNullOrEmpty(effectiveDate);
                bool isRateTableId = rateTableId.HasValue;

                if (!string.IsNullOrEmpty(underwritingMethod))
                {
                    var underwritingMethods = Util.ToArraySplitTrim(underwritingMethod).ToList();
                    productIds = TreatyPricingPickListDetailService.GetProductIdByProductIdsUnderwritingMethods(productIds, underwritingMethods);
                }

                //if (!string.IsNullOrEmpty(productName))
                //{
                //    productIds = TreatyPricingProductService.GetProductIdByProductIdsProductName(productIds, productName);
                //}

                if (isProductType)
                {
                    productVersionIds = TreatyPricingProductVersionService.GetIdByProductIdsProductType(productIds, productType.Value);
                    isProductVersion = true;
                }

                if (isTargetSegment)
                {
                    var targetSegments = Util.ToArraySplitTrim(targetSegment).ToList();
                    if (isProductVersion && productVersionIds.Count() > 0)
                        productVersionIds = TreatyPricingPickListDetailService.GetProductVersionIdByProductVersionIdsPickListIdCodes(productVersionIds, PickListBo.TargetSegment, targetSegments);
                    else if (!isProductVersion)
                        productVersionIds = TreatyPricingPickListDetailService.GetProductVersionIdByProductIdsPickListIdCodes(productIds, PickListBo.TargetSegment, targetSegments);
                    isProductVersion = true;
                }

                if (isDistributionChannel)
                {
                    var distributionChannels = Util.ToArraySplitTrim(distributionChannel).ToList();
                    if (isProductVersion && productVersionIds.Count() > 0)
                        productVersionIds = TreatyPricingPickListDetailService.GetProductVersionIdByProductVersionIdsPickListIdCodes(productVersionIds, PickListBo.DistributionChannel, distributionChannels);
                    else if (!isProductVersion)
                        productVersionIds = TreatyPricingPickListDetailService.GetProductVersionIdByProductIdsPickListIdCodes(productIds, PickListBo.DistributionChannel, distributionChannels);
                    isProductVersion = true;
                }

                if (!string.IsNullOrEmpty(productName))
                {
                    if (isProductVersion && productVersionIds.Count() > 0)
                        productVersionIds = TreatyPricingProductVersionService.GetIdByIdsProductName(productVersionIds, productName);
                    else if (!isProductVersion)
                        productVersionIds = TreatyPricingProductVersionService.GetIdByProductIdsProductName(productIds, productName);
                    isProductVersion = true;

                    //productIds = TreatyPricingProductService.GetProductIdByProductIdsProductName(productIds, productName);
                }

                if (isProductVersion && productVersionIds.Count() > 0)
                    rateTableVersionIds = TreatyPricingProductBenefitService.GetRateTableVersionIdByProductVersionIds(productVersionIds);
                else if (!isProductVersion)
                    rateTableVersionIds = TreatyPricingProductBenefitService.GetRateTableVersionIdByProductIds(productIds);

                if (isBenefitCode)
                {
                    rateTableVersionIds = TreatyPricingRateTableVersionService.GetIdByRateTableVersionIdsBenefitId(rateTableVersionIds, benefitCode.Value);
                }

                if (isBenefitName)
                {
                    rateTableVersionIds = TreatyPricingRateTableVersionService.GetIdByRateTableVersionIdsBenefitName(rateTableVersionIds, benefitName);
                }

                if (isEffectiveDate)
                {
                    DateTime effDate = DateTime.Parse(effectiveDate);
                    rateTableVersionIds = TreatyPricingRateTableVersionService.GetIdByRateTableVersionIdsEffectiveDate(rateTableVersionIds, effDate);
                }

                if (isRateTableId)
                {
                    foreach (var v in TreatyPricingRateTableVersionService.GetVersionByRateTableVersionIdsRateTableId(rateTableVersionIds, rateTableId.Value))
                    {
                        versionDropDowns.Add(new SelectListItem { Text = string.Format("{0}.0", v), Value = v.ToString() });
                    }
                }

                if (isProductVersion)
                {
                    underwritingMethodCodes = GetUnderwritingMethodCodes(productIds);
                    productTypeDropDowns.AddRange(GetProductTypeDropDowns(productIds));

                    targetSegmentCodes = GetTargetSegmentCodesByProductVersionIds(productVersionIds);
                    distributionChannelCodes = GetDistributionChannelCodesByProductVersionIds(productVersionIds);
                    productNameDropDowns.AddRange(GetProductNameDropDownsByProductVersionIds(productVersionIds));
                    benefitCodeDropDowns.AddRange(GetBenefitCodeDropDownsByProductVersionIds(productVersionIds));

                    benefitNameDropDowns.AddRange(GetBenefitNameDropDowns(rateTableVersionIds));
                    effectiveDateDropDowns.AddRange(GetEffectiveDateDropDowns(rateTableVersionIds));
                    rateTableIdDropDowns.AddRange(GetRateTableIdDropDowns(rateTableVersionIds));
                }
                else
                {
                    underwritingMethodCodes = GetUnderwritingMethodCodes(productIds);
                    productTypeDropDowns.AddRange(GetProductTypeDropDowns(productIds));
                    targetSegmentCodes = GetTargetSegmentCodesByProductIds(productIds);
                    distributionChannelCodes = GetDistributionChannelCodesByProductIds(productIds);
                    productNameDropDowns.AddRange(GetProductNameDropDownsByProductIds(productIds));
                    benefitCodeDropDowns.AddRange(GetBenefitCodeDropDownsByProductIds(productIds));

                    benefitNameDropDowns.AddRange(GetBenefitNameDropDowns(rateTableVersionIds));
                    effectiveDateDropDowns.AddRange(GetEffectiveDateDropDowns(rateTableVersionIds));
                    rateTableIdDropDowns.AddRange(GetRateTableIdDropDowns(rateTableVersionIds));
                }
            }

            return Json(new { underwritingMethodCodes, productNameDropDowns, productTypeDropDowns, productVersionIds, targetSegmentCodes, distributionChannelCodes, benefitCodeDropDowns, benefitNameDropDowns, effectiveDateDropDowns, rateTableIdDropDowns, versionDropDowns });
        }

        [HttpPost]
        public JsonResult GetRateTableVersionId(int rateTableId, int? version)
        {
            int rateTableVersionId = 0;
            if (version.HasValue)
            {
                var bo = TreatyPricingRateTableVersionService.FindByParentIdVersion(rateTableId, version.Value);
                rateTableVersionId = bo != null ? bo.Id : 0;
            }

            return Json(new { rateTableVersionId });
        }

        [HttpPost]
        public JsonResult GenerateSaRateComparison(List<int> rateTableVersionIds, List<string> productVersionIds, List<RateComparisonAgeRange> ageRanges)
        {
            List<TreatyPricingRateTableVersionBo> treatyPricingRateTableVersionBos = new List<TreatyPricingRateTableVersionBo> { };
            IList<TreatyPricingRateTableRateBo> treatyPricingRateTableRateBos = null;
            int ageCount = 0;
            foreach (var (rateTableVersionId, index) in rateTableVersionIds.Select((v, i) => (v, i)))
            {
                TreatyPricingRateTableVersionBo bo = null;
                if (rateTableVersionId != 0)
                {
                    bo = TreatyPricingRateTableVersionService.Find(rateTableVersionId);
                    if (bo != null)
                    {
                        bo.TreatyPricingRateTableBo = TreatyPricingRateTableService.Find(bo.TreatyPricingRateTableId);
                        bo.TreatyPricingRateTableRateBos = TreatyPricingRateTableRateService.GetByTreatyPricingRateTableVersionIdSa(bo.Id);
                        bo.RateDifferencePercentages = new List<RateDifferencePercentage> { };
                    }
                }
                if (bo == null)
                {
                    bo = new TreatyPricingRateTableVersionBo
                    {
                        RateDifferencePercentages = new List<RateDifferencePercentage> { }
                    };
                }

                // Product Detail
                List<int> productIds = new List<int> { };
                List<string> underwritingMethods = new List<string> { };
                List<string> reinsuranceShares = new List<string> { };
                List<string> cedantRetentions = new List<string> { };
                List<string> recaptureClauses = new List<string> { };
                bool isProduct = false;
                string productVersion = productVersionIds.ElementAt(index);
                var arrProductVersionId = !string.IsNullOrEmpty(productVersion) ? productVersion.Split(',').Select(q => q.Trim()).Select(Int32.Parse).ToList() : null;
                if (arrProductVersionId == null)
                {
                    arrProductVersionId = TreatyPricingProductBenefitService.GetDistinctProductVersionIdByRateTableVersionId(rateTableVersionId);
                }
                if (arrProductVersionId != null)
                {
                    var productBenefitBos = TreatyPricingProductBenefitService.GetByProductVersionIdsRateTableVersionId(arrProductVersionId, rateTableVersionId);

                    foreach (var productBenefitBo in productBenefitBos)
                    {
                        if (!reinsuranceShares.Contains(productBenefitBo.ReinsuranceShare))
                            reinsuranceShares.Add(productBenefitBo.ReinsuranceShare);
                        if (!cedantRetentions.Contains(productBenefitBo.CedantRetention))
                            cedantRetentions.Add(productBenefitBo.CedantRetention);

                        var productVersionBo = TreatyPricingProductVersionService.Find(productBenefitBo.TreatyPricingProductVersionId);
                        if (productVersionBo != null)
                        {
                            if (!productIds.Contains(productVersionBo.TreatyPricingProductId))
                            {
                                productIds.Add(productVersionBo.TreatyPricingProductId);
                                var productBo = TreatyPricingProductService.Find(productVersionBo.TreatyPricingProductId);
                                if (productBo != null)
                                {
                                    if (!isProduct)
                                    {
                                        bo.CedantName = productBo.TreatyPricingCedantBo?.Code;
                                        bo.ProductName = productBo.Name;
                                        isProduct = true;
                                    }
                                    foreach (var uwm in productBo.UnderwritingMethod.ToArraySplitTrim())
                                    {
                                        if (!underwritingMethods.Contains(uwm))
                                            underwritingMethods.Add(uwm);
                                    }
                                }
                            }
                            if (!recaptureClauses.Contains(productVersionBo.RecaptureClause))
                                recaptureClauses.Add(productVersionBo.RecaptureClause);
                        }
                    }
                }
                bo.UnderwritingMethod = string.Join(", ", underwritingMethods);
                bo.ReinsuranceShare = string.Join(", ", reinsuranceShares);
                bo.CedantRetention = string.Join(", ", cedantRetentions);
                bo.RecaptureClause = string.Join(", ", recaptureClauses);

                // Rate Differences
                if (index == 0)
                {
                    treatyPricingRateTableRateBos = bo.TreatyPricingRateTableRateBos;
                    ageCount = !bo.TreatyPricingRateTableRateBos.IsNullOrEmpty() ? bo.TreatyPricingRateTableRateBos.Max(q => q.Age) + 1 : 0;
                }
                else
                {
                    int tempMaxAge = 0;
                    tempMaxAge = !bo.TreatyPricingRateTableRateBos.IsNullOrEmpty() ? bo.TreatyPricingRateTableRateBos.Max(q => q.Age) + 1 : 0;
                    ageCount = ageCount < tempMaxAge ? tempMaxAge : ageCount;
                }

                foreach (var ageRange in ageRanges)
                {
                    if (!ageRange.Minimum.HasValue ||
                        !ageRange.Maximum.HasValue ||
                        treatyPricingRateTableRateBos.IsNullOrEmpty() ||
                        bo.TreatyPricingRateTableRateBos.IsNullOrEmpty())
                    {
                        bo.RateDifferencePercentages.Add(new RateDifferencePercentage());
                    }
                    else
                    {
                        var ageRangeBasisBos = treatyPricingRateTableRateBos
                            .Where(q => q.Age >= ageRange.Minimum.Value)
                            .Where(q => q.Age <= ageRange.Maximum.Value)
                            .ToList();

                        //var basisTotalMale = ageRangeBasisBos.Sum(q => q.MaleNonSmoker) +
                        //    ageRangeBasisBos.Sum(q => q.MaleSmoker) +
                        //    ageRangeBasisBos.Sum(q => q.Male);

                        var basisTotalMale = ageRangeBasisBos.Sum(q => q.Male);

                        //var basisTotalFemale = ageRangeBasisBos.Sum(q => q.FemaleNonSmoker) +
                        //    ageRangeBasisBos.Sum(q => q.FemaleSmoker) +
                        //    ageRangeBasisBos.Sum(q => q.Female);

                        var basisTotalFemale = ageRangeBasisBos.Sum(q => q.Female);

                        var basisTotalUnisex = ageRangeBasisBos.Sum(q => q.Unisex);

                        var ageRangeNextBos = bo.TreatyPricingRateTableRateBos
                            .Where(q => q.Age >= ageRange.Minimum.Value)
                            .Where(q => q.Age <= ageRange.Maximum.Value)
                            .ToList();

                        //var nextTotalMale = ageRangeNextBos.Sum(q => q.MaleNonSmoker) +
                        //    ageRangeNextBos.Sum(q => q.MaleSmoker) +
                        //    ageRangeNextBos.Sum(q => q.Male);

                        var nextTotalMale = ageRangeNextBos.Sum(q => q.Male);

                        //var nextTotalFemale = ageRangeNextBos.Sum(q => q.FemaleNonSmoker) +
                        //    ageRangeNextBos.Sum(q => q.FemaleSmoker) +
                        //    ageRangeNextBos.Sum(q => q.Female);

                        var nextTotalFemale = ageRangeNextBos.Sum(q => q.Female);

                        var nextTotalUnisex = ageRangeNextBos.Sum(q => q.Unisex);

                        double? malePercent = 0;
                        double? femalePercent = 0;
                        double? unisexPercent = 0;

                        if (basisTotalMale.HasValue && basisTotalMale != 0 && nextTotalMale.HasValue && nextTotalMale != 0)
                            malePercent = Util.RoundValue((nextTotalMale.Value / basisTotalMale.Value) * 100, 2);

                        if (basisTotalFemale.HasValue && basisTotalFemale != 0 && nextTotalFemale.HasValue && nextTotalFemale != 0)
                            femalePercent = Util.RoundValue((nextTotalFemale.Value / basisTotalFemale.Value) * 100, 2);

                        if (basisTotalUnisex.HasValue && basisTotalUnisex != 0 && nextTotalUnisex.HasValue && nextTotalUnisex != 0)
                            unisexPercent = Util.RoundValue((nextTotalUnisex.Value / basisTotalUnisex.Value) * 100, 2);

                        bo.RateDifferencePercentages.Add(new RateDifferencePercentage
                        {
                            MalePercentStr = malePercent.HasValue && malePercent != 0 ? string.Format("{0}{1}", malePercent, "%") : "",
                            FemalePercentStr = femalePercent.HasValue && femalePercent != 0 ? string.Format("{0}{1}", femalePercent, "%") : "",
                            UnisexPercentStr = unisexPercent.HasValue && unisexPercent != 0 ? string.Format("{0}{1}", unisexPercent, "%") : "",
                        });
                    }
                }
                treatyPricingRateTableVersionBos.Add(bo);
            }

            //var ageCount = treatyPricingRateTableVersionBos.Where(q => !q.TreatyPricingRateTableRateBos.IsNullOrEmpty()).Select(q => q.TreatyPricingRateTableRateBos).Where(q => q.)
            return Json(new { rateTables = treatyPricingRateTableVersionBos, ageCount });
        }

        [HttpPost]
        public JsonResult GeneratePaRateComparison(List<int> rateTableVersionIds, List<string> productVersionIds)
        {
            List<TreatyPricingRateTableVersionBo> treatyPricingRateTableVersionBos = new List<TreatyPricingRateTableVersionBo> { };
            IList<TreatyPricingRateTableRateBo> treatyPricingRateTableRateBos = null;
            int ageCount = 0;
            foreach (var (rateTableVersionId, index) in rateTableVersionIds.Select((v, i) => (v, i)))
            {
                TreatyPricingRateTableVersionBo bo = null;
                if (rateTableVersionId != 0)
                {
                    bo = TreatyPricingRateTableVersionService.Find(rateTableVersionId);
                    if (bo != null)
                    {
                        bo.TreatyPricingRateTableBo = TreatyPricingRateTableService.Find(bo.TreatyPricingRateTableId);
                        bo.TreatyPricingRateTableRateBos = TreatyPricingRateTableRateService.GetByTreatyPricingRateTableVersionIdPa(bo.Id);
                        bo.RateDifferencePercentages = new List<RateDifferencePercentage> { };
                    }
                }
                if (bo == null)
                {
                    bo = new TreatyPricingRateTableVersionBo
                    {
                        RateDifferencePercentages = new List<RateDifferencePercentage> { }
                    };
                }

                // Product Detail
                List<int> productIds = new List<int> { };
                List<string> underwritingMethods = new List<string> { };
                List<string> reinsuranceShares = new List<string> { };
                List<string> cedantRetentions = new List<string> { };
                List<string> maxExpiryAges = new List<string> { };
                bool isProduct = false;
                string productVersion = productVersionIds.ElementAt(index);
                var arrProductVersionId = !string.IsNullOrEmpty(productVersion) ? productVersion.Split(',').Select(q => q.Trim()).Select(Int32.Parse).ToList() : null;
                if (arrProductVersionId == null)
                {
                    arrProductVersionId = TreatyPricingProductBenefitService.GetDistinctProductVersionIdByRateTableVersionId(rateTableVersionId);
                }
                if (arrProductVersionId != null)
                {
                    var productBenefitBos = TreatyPricingProductBenefitService.GetByProductVersionIdsRateTableVersionId(arrProductVersionId, rateTableVersionId);

                    foreach (var productBenefitBo in productBenefitBos)
                    {
                        if (!reinsuranceShares.Contains(productBenefitBo.ReinsuranceShare))
                            reinsuranceShares.Add(productBenefitBo.ReinsuranceShare);
                        if (!cedantRetentions.Contains(productBenefitBo.CedantRetention))
                            cedantRetentions.Add(productBenefitBo.CedantRetention);
                        if (!maxExpiryAges.Contains(productBenefitBo.MaximumExpiryAge))
                            maxExpiryAges.Add(productBenefitBo.MaximumExpiryAge);

                        var productVersionBo = TreatyPricingProductVersionService.Find(productBenefitBo.TreatyPricingProductVersionId);
                        if (productVersionBo != null)
                        {
                            if (!productIds.Contains(productVersionBo.TreatyPricingProductId))
                            {
                                productIds.Add(productVersionBo.TreatyPricingProductId);
                                var productBo = TreatyPricingProductService.Find(productVersionBo.TreatyPricingProductId);
                                if (productBo != null)
                                {
                                    if (!isProduct)
                                    {
                                        bo.CedantName = productBo.TreatyPricingCedantBo?.Code;
                                        bo.ProductName = productBo.Name;
                                        isProduct = true;
                                    }
                                    foreach (var uwm in productBo.UnderwritingMethod.ToArraySplitTrim())
                                    {
                                        if (!underwritingMethods.Contains(uwm))
                                            underwritingMethods.Add(uwm);
                                    }
                                }
                            }
                        }
                    }
                }
                bo.UnderwritingMethod = string.Join(", ", underwritingMethods);
                bo.ReinsuranceShare = string.Join(", ", reinsuranceShares);
                bo.CedantRetention = string.Join(", ", cedantRetentions);
                bo.MaxExpiryAge = string.Join(", ", maxExpiryAges);

                // Rate Differences
                if (index == 0)
                {
                    treatyPricingRateTableRateBos = bo.TreatyPricingRateTableRateBos;
                    ageCount = !bo.TreatyPricingRateTableRateBos.IsNullOrEmpty() ? bo.TreatyPricingRateTableRateBos.Max(q => q.Age) + 1 : 0;
                }
                else
                {
                    int tempMaxAge = 0;
                    tempMaxAge = !bo.TreatyPricingRateTableRateBos.IsNullOrEmpty() ? bo.TreatyPricingRateTableRateBos.Max(q => q.Age) + 1 : 0;
                    ageCount = ageCount < tempMaxAge ? tempMaxAge : ageCount;
                }

                if (!treatyPricingRateTableRateBos.IsNullOrEmpty())
                {
                    double? basisTotalUnitRate = 0;
                    double? nextTotalUnitRate = 0;
                    foreach (var rateBo in treatyPricingRateTableRateBos)
                    {
                        if (bo.TreatyPricingRateTableRateBos.IsNullOrEmpty())
                        {
                            bo.RateDifferencePercentages.Add(
                                new RateDifferencePercentage
                                {
                                    Age = rateBo.Age,
                                });
                        }
                        else
                        {
                            var ageBasisBos = treatyPricingRateTableRateBos
                                .Where(q => q.Age == rateBo.Age)
                                .ToList();

                            basisTotalUnitRate += ageBasisBos.Sum(q => q.UnitRate);
                            var basisTotalOccupationClass = ageBasisBos.Sum(q => q.OccupationClass);

                            var ageNextBos = bo.TreatyPricingRateTableRateBos
                                .Where(q => q.Age == rateBo.Age)
                                .ToList();

                            nextTotalUnitRate += ageNextBos.Sum(q => q.UnitRate);
                            var nextTotalOccupationClass = ageNextBos.Sum(q => q.OccupationClass);

                            double? occupationClassPercent = 0;

                            if (basisTotalOccupationClass.HasValue && basisTotalOccupationClass != 0 && nextTotalOccupationClass.HasValue && nextTotalOccupationClass != 0)
                                occupationClassPercent = Util.RoundValue((basisTotalOccupationClass.Value / nextTotalOccupationClass.Value) * 100, 2);

                            bo.RateDifferencePercentages.Add(new RateDifferencePercentage
                            {
                                OccupationClassPercentStr = occupationClassPercent.HasValue && occupationClassPercent != 0 ? string.Format("{0}{1}", occupationClassPercent, "%") : "",
                                Age = rateBo.Age,
                            });
                        }
                    }
                    double? unitRatePercent = 0;
                    if (basisTotalUnitRate.HasValue && basisTotalUnitRate != 0 && nextTotalUnitRate.HasValue && nextTotalUnitRate != 0)
                        unitRatePercent = Util.RoundValue((basisTotalUnitRate.Value / nextTotalUnitRate.Value) * 100, 2);
                    bo.UnitRatePercentStr = unitRatePercent.HasValue && unitRatePercent != 0 ? string.Format("{0}{1}", unitRatePercent, "%") : "";
                }

                treatyPricingRateTableVersionBos.Add(bo);
            }

            //var ageCount = treatyPricingRateTableVersionBos.Where(q => !q.TreatyPricingRateTableRateBos.IsNullOrEmpty()).Max(q => q.TreatyPricingRateTableRateBos.Count());
            return Json(new { rateTables = treatyPricingRateTableVersionBos, ageCount });
        }


        // Get Distinct UnderWrtitingCode Name By Product Ids
        public List<string> GetUnderwritingMethodCodes(List<int> productIds)
        {
            return TreatyPricingPickListDetailService.GetDistinctCodeByObjectProductObjectIdsPickList(productIds, PickListBo.UnderwritingMethod);
        }

        // Get Distinct Product Name By Product Ids
        public List<SelectListItem> GetProductNameDropDownsByProductIds(List<int> productIds)
        {
            var productNameDropDowns = new List<SelectListItem> { };
            foreach (var productName in TreatyPricingProductService.GetDistinctNameByProductIds(productIds))
            {
                productNameDropDowns.Add(new SelectListItem { Text = productName, Value = productName });
            }
            return productNameDropDowns;
        }

        // Get Distinct Product Type By Product Ids
        public List<SelectListItem> GetProductTypeDropDowns(List<int> productIds)
        {
            var productTypeDropDowns = new List<SelectListItem> { };
            foreach (var productType in TreatyPricingProductVersionService.GetDistinctProductTypeByIds(productIds))
            {
                productTypeDropDowns.Add(new SelectListItem { Text = productType.ToString(), Value = productType.Id.ToString() });
            }
            return productTypeDropDowns;
        }

        // Get Distinct Target Segment By Product Ids
        public List<string> GetTargetSegmentCodesByProductIds(List<int> productIds)
        {
            return TreatyPricingPickListDetailService.GetDistinctCodeByObjectProductVersionProductIdsPickList(productIds, PickListBo.TargetSegment);
        }

        // Get Distinct Distribution Channel By Product Ids
        public List<string> GetDistributionChannelCodesByProductIds(List<int> productIds)
        {
            return TreatyPricingPickListDetailService.GetDistinctCodeByObjectProductVersionProductIdsPickList(productIds, PickListBo.DistributionChannel);
        }

        // Get Distinct Benefit Code By Product Ids
        public List<SelectListItem> GetBenefitCodeDropDownsByProductIds(List<int> productIds)
        {
            var benefitCodeDropDowns = new List<SelectListItem> { };
            foreach (var benefitBo in TreatyPricingRateTableService.GetDistinctBenefitCodeByProductIds(productIds))
            {
                benefitCodeDropDowns.Add(new SelectListItem { Text = benefitBo.ToString(), Value = benefitBo.Id.ToString() });
            }
            return benefitCodeDropDowns;
        }

        // Get Distinct Target Segment By Product Version Ids
        public List<string> GetTargetSegmentCodesByProductVersionIds(List<int> productVersionIds)
        {
            return TreatyPricingPickListDetailService.GetDistinctCodeByObjectProductVersionProductVersionIdsPickList(productVersionIds, PickListBo.TargetSegment);
        }

        // Get Distinct Distribution Channel By Product Version Ids
        public List<string> GetDistributionChannelCodesByProductVersionIds(List<int> productVersionIds)
        {
            return TreatyPricingPickListDetailService.GetDistinctCodeByObjectProductVersionProductVersionIdsPickList(productVersionIds, PickListBo.DistributionChannel);
        }

        // Get Distinct Product Name By Product Version Ids
        public List<SelectListItem> GetProductNameDropDownsByProductVersionIds(List<int> productVersionIds)
        {
            var productNameDropDowns = new List<SelectListItem> { };
            foreach (var productName in TreatyPricingProductVersionService.GetDistinctProductNameByIds(productVersionIds))
            {
                productNameDropDowns.Add(new SelectListItem { Text = productName, Value = productName });
            }
            return productNameDropDowns;
        }

        // Get Distinct Benefit Code By Product Version Ids
        public List<SelectListItem> GetBenefitCodeDropDownsByProductVersionIds(List<int> productVersionIds)
        {
            var benefitCodeDropDowns = new List<SelectListItem> { };
            foreach (var benefitBo in TreatyPricingRateTableService.GetDistinctBenefitCodeByProductVersionIds(productVersionIds))
            {
                benefitCodeDropDowns.Add(new SelectListItem { Text = benefitBo.ToString(), Value = benefitBo.Id.ToString() });
            }
            return benefitCodeDropDowns;
        }

        // Get Distinct Benefit Name
        public List<SelectListItem> GetBenefitNameDropDowns(List<int> rateTableVersionIds)
        {
            var benefitNameDropDowns = new List<SelectListItem> { };
            foreach (var benefitName in TreatyPricingRateTableVersionService.GetDistinctBenefitNameByRateTableVersionIds(rateTableVersionIds))
            {
                benefitNameDropDowns.Add(new SelectListItem { Text = benefitName, Value = benefitName });
            }
            return benefitNameDropDowns;
        }

        // Get Distinct Effective Date
        public List<SelectListItem> GetEffectiveDateDropDowns(List<int> rateTableVersionIds)
        {
            var effectiveDateDropDowns = new List<SelectListItem> { };
            foreach (var effectiveDate in TreatyPricingRateTableVersionService.GetDistinctEffectiveDateByRateTableVersionIds(rateTableVersionIds))
            {
                effectiveDateDropDowns.Add(new SelectListItem { Text = effectiveDate, Value = effectiveDate });
            }
            return effectiveDateDropDowns;
        }

        // Get Distinct Rate Table ID
        public List<SelectListItem> GetRateTableIdDropDowns(List<int> rateTableVersionIds)
        {
            var rateTableIdDropDowns = new List<SelectListItem> { };
            foreach (var rateTableBo in TreatyPricingRateTableVersionService.GetRateTableByRateTableVersionIds(rateTableVersionIds))
            {
                rateTableIdDropDowns.Add(new SelectListItem { Text = rateTableBo.Code, Value = rateTableBo.Id.ToString() });
            }
            return rateTableIdDropDowns;
        }
        #endregion

        #region Definition & Exclusion Comparison
        [HttpPost]
        public JsonResult GetDefinitionAndExclusionVersionId(string definitionAndExclusionCode, int? version)
        {
            int definitionAndExclusionVersionId = 0;
            var definitionAndExclusionId = TreatyPricingDefinitionAndExclusionService.FindByDefinitionAndExclusionCode(definitionAndExclusionCode).Id;
            if (version.HasValue)
            {
                var bo = TreatyPricingDefinitionAndExclusionVersionService.FindByParentIdVersion(definitionAndExclusionId, version.Value);
                definitionAndExclusionVersionId = bo != null ? bo.Id : 0;
            }
            return Json(new { definitionAndExclusionVersionId });
        }

        [HttpPost]
        public JsonResult GenerateDefinitionAndExclusionComparison(List<int> definitionAndExclusionVersionIds)
        {
            List<TreatyPricingDefinitionAndExclusionVersionBo> treatyPricingDefinitionAndExclusionVersionBos = new List<TreatyPricingDefinitionAndExclusionVersionBo> { };

            foreach (var (definitionAndExclusionVersionId, index) in definitionAndExclusionVersionIds.Select((v, i) => (v, i)))
            {
                TreatyPricingDefinitionAndExclusionVersionBo bo = null;
                if (definitionAndExclusionVersionId != 0)
                {
                    bo = TreatyPricingDefinitionAndExclusionVersionService.Find(definitionAndExclusionVersionId);
                    if (bo != null)
                    {
                        bo.TreatyPricingDefinitionAndExclusionBo = TreatyPricingDefinitionAndExclusionService.Find(bo.TreatyPricingDefinitionAndExclusionId);
                        bo.VersionStr = bo.Version.ToString() + ".0";
                    }
                }
                else
                {
                    bo = new TreatyPricingDefinitionAndExclusionVersionBo();
                }
                treatyPricingDefinitionAndExclusionVersionBos.Add(bo);
            }
            return Json(new { definitionAndExclusions = treatyPricingDefinitionAndExclusionVersionBos });
        }

        [HttpPost]
        public JsonResult UpdateDefinitionAndExclusionData(
            int? treatyPricingCedantId,
            string definitionAndExclusionCode,
            int? status,
            string benefitCode,
            int? version,
            int? personInCharge
            )
        {
            var definitionAndExclusionCodeDropDowns = GetEmptyDropDownList();
            var statusDropDowns = GetEmptyDropDownList();
            var benefitCodes = new List<string> { };
            var versionDropDowns = GetEmptyDropDownList();
            var personInChargeDropDowns = GetEmptyDropDownList();

            if (treatyPricingCedantId.HasValue)
            {
                var definitionAndExclusionCodes = TreatyPricingDefinitionAndExclusionService.GetByTreatyPricingCedantId(treatyPricingCedantId).Select(q => q.Code).ToList();

                foreach (var i in definitionAndExclusionCodes)
                {
                    definitionAndExclusionCodeDropDowns.Add(new SelectListItem { Text = i, Value = i });
                }

                if (!string.IsNullOrEmpty(definitionAndExclusionCode))
                {
                    var definitionAndExclusionBo = TreatyPricingDefinitionAndExclusionService.FindByDefinitionAndExclusionCode(definitionAndExclusionCode);

                    statusDropDowns.Add(new SelectListItem { Text = definitionAndExclusionBo.StatusName, Value = definitionAndExclusionBo.Status.ToString() });

                    if (!string.IsNullOrEmpty(definitionAndExclusionBo.BenefitCode))
                        benefitCodes.AddRange(definitionAndExclusionBo.BenefitCode.Split(','));


                    foreach (var v in TreatyPricingDefinitionAndExclusionService.GetVersionByDefinitionAndExclusionId(definitionAndExclusionBo.Id))
                    {
                        versionDropDowns.Add(new SelectListItem { Text = string.Format("{0}.0", v), Value = v.ToString() });
                    }

                    if (version.HasValue)
                    {
                        var versionBo = TreatyPricingDefinitionAndExclusionVersionService.GetByTreatyPricingDefinitionAndExclusionId(definitionAndExclusionBo.Id).Where(a => a.Version == version).FirstOrDefault();

                        personInChargeDropDowns.Add(new SelectListItem { Text = versionBo.PersonInChargeName, Value = versionBo.PersonInChargeId.ToString() });
                    }
                }


            }
            return Json(new { definitionAndExclusionCodeDropDowns, statusDropDowns, benefitCodes, versionDropDowns, personInChargeDropDowns });
        }
        #endregion

        #region Campaign Comparison
        [HttpPost]
        public JsonResult UpdateCampaignComparisonData(
            int? treatyPricingCedantId,
            int? campaignId,
            string campaignType,
            int? status,
            string products,
            string benefitCode,
            string benefitRemarks,
            string distributionChannel,
            string ifOthers,
            int? version)
        {
            var campaignIdDropDowns = GetEmptyDropDownList();
            var campaignTypes = new List<string>();
            var statusDropDowns = GetEmptyDropDownList();
            var campaignProducts = new List<string>();
            var distributionChannels = new List<string>();
            var versionDropDowns = GetEmptyDropDownList();

            if (treatyPricingCedantId.HasValue)
            {
                var campaigns = TreatyPricingCampaignService.GetByTreatyPricingCedantId(treatyPricingCedantId.Value).ToList();

                foreach (var i in campaigns)
                {
                    campaignIdDropDowns.Add(new SelectListItem { Text = i.Code, Value = i.Id.ToString() });
                }

                if (campaignId.HasValue)
                {
                    var campaignBo = TreatyPricingCampaignService.Find(campaignId.Value);

                    if (campaignBo != null)
                    {
                        if (campaignBo.Type != null)
                        {
                            if (campaignBo.Type.Contains(','))
                            {
                                foreach (var type in campaignBo.Type.Split(','))
                                {
                                    campaignTypes.Add(type.Trim());
                                }
                            }
                            else
                            {
                                campaignTypes.Add(campaignBo.Type);
                            }
                        }
                    }

                    statusDropDowns.Add(new SelectListItem { Text = TreatyPricingCampaignBo.GetStatusName(TreatyPricingCampaignBo.StatusActive), Value = TreatyPricingCampaignBo.StatusActive.ToString() });
                    statusDropDowns.Add(new SelectListItem { Text = TreatyPricingCampaignBo.GetStatusName(TreatyPricingCampaignBo.StatusInactive), Value = TreatyPricingCampaignBo.StatusInactive.ToString() });

                    campaignProducts = TreatyPricingCampaignProductService.GetByTreatyPricingCampaignId(campaignId.Value).Select(a => a.TreatyPricingProductBo.Code).ToList();

                    var distributionCh = new List<string>();

                    var campaignVers = TreatyPricingCampaignVersionService.GetByTreatyPricingCampaignId(campaignId);
                    foreach (var ver in campaignVers)
                    {
                        if (!string.IsNullOrEmpty(ver.DistributionChannel))
                        {
                            if (ver.DistributionChannel.Contains(','))
                            {
                                foreach (var disCh in ver.DistributionChannel.Split(',').ToList())
                                {
                                    distributionCh.Add(disCh.Trim());
                                }
                            }
                            else
                            {
                                distributionCh.Add(ver.DistributionChannel);
                            }
                        }
                    }

                    distributionChannels = distributionCh.Distinct().ToList();
                    var disChs = new List<string>();

                    if (!string.IsNullOrEmpty(distributionChannel))
                    {
                        if (distributionChannel.Contains(','))
                        {
                            foreach (var dis in distributionChannel.Split(','))
                            {
                                disChs.Add(dis.Trim());
                            }
                        }
                        else
                        {
                            disChs.Add(distributionChannel);
                        }

                        foreach (var campVer in campaignVers)
                        {
                            foreach (var dCH in disChs)
                            {
                                if (campVer.DistributionChannel != null && campVer.DistributionChannel.Contains(dCH))
                                {
                                    versionDropDowns.Add(new SelectListItem { Text = string.Format("{0}.0", campVer.Version), Value = campVer.Version.ToString() });
                                }
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(ifOthers))
                    {
                        foreach (var campVer in campaignVers.Where(a => a.IsPerDistributionChannel == bool.Parse(ifOthers)))
                        {
                            versionDropDowns.Add(new SelectListItem { Text = string.Format("{0}.0", campVer.Version), Value = campVer.Version.ToString() });
                        }
                    }
                    else
                    {
                        foreach (var campVer in campaignVers)
                        {
                            versionDropDowns.Add(new SelectListItem { Text = string.Format("{0}.0", campVer.Version), Value = campVer.Version.ToString() });
                        }
                    }

                }

            }

            return Json(new { campaignIdDropDowns, campaignTypes, statusDropDowns, campaignProducts, distributionChannels, versionDropDowns });

        }

        [HttpPost]
        public JsonResult GetCampaignVersionId(int campaignId, int? version)
        {
            int campaignVersionId = 0;
            if (version.HasValue)
            {
                var bo = TreatyPricingCampaignVersionService.FindByParentIdVersion(campaignId, version.Value);
                campaignVersionId = bo != null ? bo.Id : 0;
            }
            return Json(new { campaignVersionId });
        }

        [HttpPost]
        public JsonResult GenerateCampaignComparison(List<int> campaignVersionIds)
        {
            List<TreatyPricingCampaignVersionBo> treatyPricingCampaignVersionBos = new List<TreatyPricingCampaignVersionBo>();

            foreach (var (campaignVersionId, index) in campaignVersionIds.Select((v, i) => (v, i)))
            {
                TreatyPricingCampaignVersionBo bo = null;
                if (campaignVersionId != 0)
                {
                    bo = TreatyPricingCampaignVersionService.Find(campaignVersionId, true);
                    if (bo != null)
                    {
                        bo.TreatyPricingCampaignBo = TreatyPricingCampaignService.Find(bo.TreatyPricingCampaignId);
                        bo.VersionStr = bo.Version.ToString() + ".0";

                        if (bo.IsPerBenefit)
                            bo.IsPerBenefitStr = "As Per Existing";
                        else
                            bo.IsPerBenefitStr = "Others";

                        if (bo.IsPerCedantRetention)
                            bo.IsPerCedantRetentionStr = "As Per Existing";
                        else
                            bo.IsPerBenefitStr = "Others";

                        if (bo.IsPerMlreShare)
                            bo.IsPerMlreShareStr = "As Per Existing";
                        else
                            bo.IsPerMlreShareStr = "Others";

                        if (bo.IsPerDistributionChannel)
                            bo.IsPerDistributionChannelStr = "As Per Existing";
                        else
                            bo.IsPerDistributionChannelStr = "Others";

                        if (bo.IsPerAgeBasis)
                            bo.IsPerAgeBasisStr = "As Per Existing";
                        else
                            bo.IsPerAgeBasisStr = "Others";

                        if (bo.IsPerMinEntryAge)
                            bo.IsPerMinEntryAgeStr = "As Per Existing";
                        else
                            bo.IsPerMinEntryAgeStr = "Others";

                        if (bo.IsPerMaxEntryAge)
                            bo.IsPerMaxEntryAgeStr = "As Per Existing";
                        else
                            bo.IsPerMaxEntryAgeStr = "Others";

                        if (bo.IsPerMaxExpiryAge)
                            bo.IsPerMaxExpiryAgeStr = "As Per Existing";
                        else
                            bo.IsPerMaxExpiryAgeStr = "Others";

                        if (bo.IsPerMinSumAssured)
                            bo.IsPerMinSumAssuredStr = "As Per Existing";
                        else
                            bo.IsPerMinSumAssuredStr = "Others";

                        if (bo.IsPerMaxSumAssured)
                            bo.IsPerMaxSumAssuredStr = "As Per Existing";
                        else
                            bo.IsPerMaxSumAssuredStr = "Others";

                        if (bo.IsPerReinsuranceRate)
                            bo.IsPerReinsuranceRateStr = "As Per Existing";
                        else
                            bo.IsPerReinsuranceRateStr = "Others";

                        if (bo.IsPerReinsuranceDiscount)
                            bo.IsPerReinsuranceDiscountStr = "As Per Existing";
                        else
                            bo.IsPerReinsuranceDiscountStr = "Others";

                        if (bo.IsPerProfitComm)
                            bo.IsPerProfitCommStr = "As Per Existing";
                        else
                            bo.IsPerProfitCommStr = "Others";

                        if (bo.IsPerUnderwritingMethod)
                            bo.IsPerUnderwritingMethodStr = "As Per Existing";
                        else
                            bo.IsPerUnderwritingMethodStr = "Others";

                        if (bo.IsPerUnderwritingQuestion)
                            bo.IsPerUnderwritingQuestionStr = "As Per Existing";
                        else
                            bo.IsPerUnderwritingQuestionStr = "Others";

                        if (bo.IsPerMedicalTable)
                            bo.IsPerMedicalTableStr = "As Per Existing";
                        else
                            bo.IsPerMedicalTableStr = "Others";

                        if (bo.IsPerFinancialTable)
                            bo.IsPerFinancialTableStr = "As Per Existing";
                        else
                            bo.IsPerFinancialTableStr = "Others";

                        if (bo.IsPerAggregationNotes)
                            bo.IsPerAggregationNotesStr = "As Per Existing";
                        else
                            bo.IsPerAggregationNotesStr = "Others";

                        if (bo.IsPerAdvantageProgram)
                            bo.IsPerAdvantageProgramStr = "As Per Existing";
                        else
                            bo.IsPerAdvantageProgramStr = "Others";

                        if (bo.IsPerWaitingPeriod)
                            bo.IsPerWaitingPeriodStr = "As Per Existing";
                        else
                            bo.IsPerWaitingPeriodStr = "Others";

                        if (bo.IsPerSurvivalPeriod)
                            bo.IsPerSurvivalPeriodStr = "As Per Existing";
                        else
                            bo.IsPerSurvivalPeriodStr = "Others";


                        bo.TreatyPricingCampaignBo.TreatyPricingCampaignProductBo = TreatyPricingCampaignProductService.GetByTreatyPricingCampaignId(bo.TreatyPricingCampaignBo.Id).ToList();
                        var productStr = "";
                        foreach (var product in bo.TreatyPricingCampaignBo.TreatyPricingCampaignProductBo)
                        {
                            if (product != null)
                                if (bo.TreatyPricingCampaignBo.TreatyPricingCampaignProductBo.Count > 1)
                                    productStr = product.TreatyPricingProductBo.Code + " - " + product.TreatyPricingProductBo.Name + ", <br />" + productStr;
                                else
                                    productStr = product.TreatyPricingProductBo.Code + " - " + product.TreatyPricingProductBo.Name;
                        }
                        bo.TreatyPricingCampaignBo.TreatyPricingCampaignProduct = productStr;
                        if (!string.IsNullOrEmpty(bo.TreatyPricingCampaignBo.PeriodStartDateStr) && !string.IsNullOrEmpty(bo.TreatyPricingCampaignBo.PeriodEndDateStr))
                            bo.TreatyPricingCampaignBo.Period = bo.TreatyPricingCampaignBo.PeriodStartDateStr + " - " + bo.TreatyPricingCampaignBo.PeriodEndDateStr;
                        bo.AgeBasisPickListDetailBo = PickListDetailService.Find(bo.AgeBasisPickListDetailId);
                    }
                }
                else
                {
                    bo = new TreatyPricingCampaignVersionBo();
                }
                treatyPricingCampaignVersionBos.Add(bo);
            }
            return Json(new { campaigns = treatyPricingCampaignVersionBos });
        }

        #endregion

        #region Underwriting Limit Comparison
        public JsonResult GenerateUwLimitComparison(string treatyPricingCedantId, string transposeExcel)
        {
            List<string> errors = new List<string>();

            try
            {
                TrailObject trail = GetNewTrailObject();

                var bo = new TreatyPricingReportGenerationBo()
                {
                    ReportName = "Underwriting Limit Comparison Report",
                    ReportParams = treatyPricingCedantId + "|" + transposeExcel,
                    Status = TreatyPricingReportGenerationBo.StatusPending,
                    CreatedById = AuthUserId,
                    UpdatedById = AuthUserId,
                };
                Result = TreatyPricingReportGenerationService.Create(ref bo, ref trail);

                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Create Treaty Pricing Report Generation"
                    );
                }
            }
            catch (Exception ex)
            {
                errors.Add(string.Format("Error occurred. Error details: {0}", ex.Message));
            }

            return Json(new { errors });
        }
        #endregion

        #region Medical & Non-Medical Table Comparison
        [HttpPost]
        public JsonResult UpdateMedicalTableData(
            int? treatyPricingCedantId,
            int? benefitCode = null,
            int? distributionChannel = null,
            int? status = null,
            int? medicalTableId = null
        )
        {
            var benefitCodeDropDowns = GetEmptyDropDownList();
            var distributionChannelDropDowns = GetEmptyDropDownList();
            var statusDropdowns = GetEmptyDropDownList();
            var medicalTableIdDropDowns = GetEmptyDropDownList();
            var versionDropDowns = GetEmptyDropDownList();
            var distributionTierDropDowns = GetEmptyDropDownList();

            if (treatyPricingCedantId.HasValue)
            {
                var medicalTableIds = TreatyPricingMedicalTableService.GetIdByMedicalTableIdsTreatyPricingCedantId(treatyPricingCedantId.Value);

                bool isBenefitCode = benefitCode.HasValue;
                bool isDistributionChannel = distributionChannel.HasValue;
                bool isStatus = status.HasValue;
                bool isMedicalTableId = medicalTableId.HasValue;

                if (isBenefitCode)
                {
                    medicalTableIds = TreatyPricingMedicalTableService.GetIdByMedicalTableIdsBenefitCode(medicalTableIds, benefitCode.Value);
                    distributionChannelDropDowns.AddRange(GetMedicalDistributionChannelDropDownsByBenefitCode(medicalTableIds, benefitCode.Value));
                }

                if (isDistributionChannel)
                {
                    medicalTableIds = TreatyPricingMedicalTableService.GetIdByMedicalTableIdsDistributionChannel(medicalTableIds, distributionChannel.Value);
                    statusDropdowns.AddRange(GetMedicalStatusDropDownsByDistributionChannel(medicalTableIds, distributionChannel.Value));
                }

                if (isStatus)
                {
                    medicalTableIds = TreatyPricingMedicalTableService.GetIdByMedicalTableIdsStatus(medicalTableIds, status.Value);
                }

                if (isMedicalTableId)
                {
                    foreach (var versionBo in TreatyPricingMedicalTableVersionService.GetVersionByMedicalTableVersionIdsMedicalTableId(medicalTableId.Value))
                    {
                        versionDropDowns.Add(new SelectListItem { Text = string.Format("{0}.0", versionBo.Version.ToString()), Value = versionBo.Id.ToString() });
                    }
                }

                benefitCodeDropDowns.AddRange(GetMedicalBenefitCodeDropDownsByTreatyPricingCedantId(treatyPricingCedantId.Value));
                medicalTableIdDropDowns.AddRange(GetMedicalTableDropDownsByMedicalTableIds(medicalTableIds));
            }

            return Json(new { benefitCodeDropDowns, distributionChannelDropDowns, statusDropdowns, medicalTableIdDropDowns, versionDropDowns, distributionTierDropDowns });
        }

        // Get Distinct Medical Table Benefit Code By TreatyPricingCedantId
        public List<SelectListItem> GetMedicalBenefitCodeDropDownsByTreatyPricingCedantId(int cedantId)
        {
            var benefitCodeDropDowns = new List<SelectListItem> { };
            foreach (var benefitBo in TreatyPricingMedicalTableService.GetDistinctBenefitCodeByCedantId(cedantId))
            {
                benefitCodeDropDowns.Add(new SelectListItem { Text = benefitBo.ToString(), Value = benefitBo.Id.ToString() });
            }
            return benefitCodeDropDowns;
        }

        // Get Distinct Medical Table Distribution Channel By Benefit Code
        public List<SelectListItem> GetMedicalDistributionChannelDropDownsByBenefitCode(List<int> medicalTableIds, int benefitCode)
        {
            var distributionChannelDropDowns = new List<SelectListItem> { };
            foreach (var distributionChannelBo in TreatyPricingMedicalTableService.GetDistinctDistributionChannelByBenefitCode(medicalTableIds, benefitCode))
            {
                distributionChannelDropDowns.Add(new SelectListItem { Text = distributionChannelBo.ToString(), Value = distributionChannelBo.Id.ToString() });
            }
            return distributionChannelDropDowns;
        }

        // Get Distinct Medical Table Status By Distribution Channel
        public List<SelectListItem> GetMedicalStatusDropDownsByDistributionChannel(List<int> medicalTableIds, int distributionChannel)
        {
            var statusDropdowns = new List<SelectListItem> { };
            foreach (var statusBo in TreatyPricingMedicalTableService.GetDistinctStatusByDistributionChannel(medicalTableIds, distributionChannel))
            {
                statusDropdowns.Add(new SelectListItem { Text = statusBo.StatusName, Value = statusBo.Status.ToString() });
            }
            return statusDropdowns;
        }

        // Get Distinct Medical Table By Medical Table Ids
        public List<SelectListItem> GetMedicalTableDropDownsByMedicalTableIds(List<int> medicalTableIds)
        {
            var medicalTableIdDropDowns = new List<SelectListItem> { };
            foreach (var medicalTableBo in TreatyPricingMedicalTableService.GetMedicalTablesByMedicalTableIds(medicalTableIds))
            {
                medicalTableIdDropDowns.Add(new SelectListItem { Text = (medicalTableBo.MedicalTableId + " - " + medicalTableBo.Name), Value = medicalTableBo.Id.ToString() });
            }
            return medicalTableIdDropDowns;
        }

        [HttpPost]
        public JsonResult GetMedicalTableComparisonDistributionTiers(int? versionId)
        {
            var distributionTierDropDowns = GetEmptyDropDownList();

            if (versionId.HasValue)
            {
                foreach (var detailBo in TreatyPricingMedicalTableVersionDetailService.GetByTreatyPricingMedicalTableVersionId(versionId.Value))
                {
                    distributionTierDropDowns.Add(new SelectListItem { Text = string.Format("{0}.0", detailBo.DistributionTierPickListDetailBo.Code.ToString()) + " - " + detailBo.Description, Value = detailBo.DistributionTierPickListDetailId.ToString() });
                }
            }

            return Json(new { distributionTierDropDowns });
        }

        public class SumAssuredInterval
        {
            public string Start { get; set; }

            public string End { get; set; }

            public SumAssuredInterval(string start, string end)
            {
                Start = start;
                End = end;
            }
        }

        public class SumAssuredIntervalUnformatted
        {
            public int Start { get; set; }

            public int End { get; set; }

            public SumAssuredIntervalUnformatted(int start, int end)
            {
                Start = start;
                End = end;
            }
        }

        [HttpPost]
        public JsonResult GenerateMedicalTableComparison(List<int> versionIds, List<int> distributionTierIds, List<RateComparisonAgeRange> ageRanges)
        {
            List<SumAssuredInterval> sumAssuredIntervals = new List<SumAssuredInterval>();
            List<SumAssuredIntervalUnformatted> sumAssuredIntervalUnformattedList = new List<SumAssuredIntervalUnformatted>();
            List<TreatyPricingMedicalTableBo> medicalTableBos = new List<TreatyPricingMedicalTableBo>();
            List<TreatyPricingMedicalTableVersionBo> versionBos = new List<TreatyPricingMedicalTableVersionBo>();
            List<TreatyPricingMedicalTableVersionDetailBo> detailBos = new List<TreatyPricingMedicalTableVersionDetailBo>();
            List<TreatyPricingMedicalTableUploadRowBo> rowBos = new List<TreatyPricingMedicalTableUploadRowBo>();
            List<int> sumAssured = new List<int>();
            List<int> sumAssuredDistinct = new List<int>();

            #region Get Sum Assured Intervals
            for (int i = 0; i < versionIds.Count; i++)
            {
                if (versionIds[i] > 0 && distributionTierIds[i] > 0)
                {
                    int detailId = TreatyPricingMedicalTableVersionDetailService.GetByVersionIdDistributionTierPickListDetailId(versionIds[i], distributionTierIds[i]).Id;
                    detailBos.Add(TreatyPricingMedicalTableVersionDetailService.Find(detailId));
                    rowBos.AddRange(TreatyPricingMedicalTableUploadRowService.GetByTreatyPricingMedicalTableVersionDetailId(detailId));
                }
            }

            if (rowBos.Count > 0)
            {
                foreach (var rowBo in rowBos)
                {
                    sumAssured.Add(rowBo.MinimumSumAssured - 1);
                    sumAssured.Add(rowBo.MaximumSumAssured);
                }
            }

            sumAssuredDistinct.AddRange(sumAssured.Distinct());
            sumAssuredDistinct = sumAssuredDistinct.OrderBy(o => o).ToList();

            for (int i = 0; i < sumAssuredDistinct.Count; i++)
            {
                if (i != sumAssuredDistinct.Count - 1)
                {
                    string minimum = String.Format("{0:n0}", sumAssuredDistinct[i] + 1);
                    string maximum = sumAssuredDistinct[i + 1] >= 2000000000 ? "Max" : String.Format("{0:n0}", sumAssuredDistinct[i + 1]);
                    sumAssuredIntervals.Add(new SumAssuredInterval(minimum, maximum));
                    sumAssuredIntervalUnformattedList.Add(new SumAssuredIntervalUnformatted(sumAssuredDistinct[i] + 1, sumAssuredDistinct[i + 1]));
                }
            }
            #endregion

            #region Get Medical Table information
            foreach (var detailBo in detailBos)
            {
                #region Comparison header information
                var versionBo = TreatyPricingMedicalTableVersionService.Find(detailBo.TreatyPricingMedicalTableVersionId);
                versionBos.Add(versionBo);

                var medicalTableBo = TreatyPricingMedicalTableService.Find(versionBo.TreatyPricingMedicalTableId);
                medicalTableBo.VersionBo = versionBo;
                medicalTableBo.DetailBo = detailBo;
                medicalTableBo.CedantInfo = medicalTableBo.TreatyPricingCedantBo.CedantBo.Code + " - " + medicalTableBo.TreatyPricingCedantBo.CedantBo.Name;

                //Get Linked Products
                string linkedProducts = "";
                List<string> linkedProductList = new List<string>();
                var productVersionBos = TreatyPricingProductVersionService.GetByTreatyPricingMedicalTableVersionId(versionBo.Id);

                if (productVersionBos.Count > 0)
                {
                    foreach (var productVersionBo in productVersionBos)
                    {
                        linkedProductList.Add(productVersionBo.TreatyPricingProductBo.Name);
                    }

                    linkedProducts = String.Join(", ", linkedProductList.ToArray());
                }

                medicalTableBo.LinkedProducts = linkedProducts;
                #endregion

                #region Comparison detail information
                List<MedicalTableDetailComparison> medicalTableDetailComparisons = new List<MedicalTableDetailComparison>();

                foreach (var ageRange in ageRanges)
                {
                    List<string> items = new List<string>();

                    if (ageRange.Minimum.HasValue && ageRange.Maximum.HasValue)
                    {
                        int minimumAge = ageRange.Minimum.Value;
                        int maximumAge = ageRange.Maximum.Value;

                        foreach (var sumAssuredInterval in sumAssuredIntervalUnformattedList)
                        {
                            int minimumSumAssured = sumAssuredInterval.Start;
                            int maximumSumAssured = sumAssuredInterval.End;
                            string item = "";

                            var medicalTableRowBo = TreatyPricingMedicalTableUploadRowService.GetByTreatyPricingMedicalTableVersionDetailIdForComparison(detailBo.Id, minimumSumAssured, maximumSumAssured);

                            if (medicalTableRowBo != null)
                            {
                                int rowId = medicalTableRowBo.Id;
                                var medicalTableColumnBos = TreatyPricingMedicalTableUploadColumnService.GetByTreatyPricingMedicalTableVersionDetailId(detailBo.Id);

                                foreach (var columnBo in medicalTableColumnBos)
                                {
                                    if ((columnBo.MinimumAge >= minimumAge && columnBo.MinimumAge <= maximumAge) || (columnBo.MaximumAge >= minimumAge && columnBo.MaximumAge <= maximumAge) || (columnBo.MinimumAge <= minimumAge && columnBo.MaximumAge >= maximumAge))

                                    {
                                        var cellBo = TreatyPricingMedicalTableUploadCellService.GetByTreatyPricingMedicalTableUploadRowColumnId(rowId, columnBo.Id);

                                        if (cellBo != null)
                                        {
                                            string cellBoItem = cellBo.Code;

                                            if (item == "")
                                            {
                                                item += (columnBo.MinimumAge == minimumAge && columnBo.MaximumAge == maximumAge ? cellBoItem : cellBoItem + "(" + columnBo.MinimumAge + "-" + columnBo.MaximumAge + ")");
                                            }
                                            else
                                            {
                                                item += ", " + (columnBo.MinimumAge == minimumAge && columnBo.MaximumAge == maximumAge ? cellBoItem : cellBoItem + "(" + columnBo.MinimumAge + "-" + columnBo.MaximumAge + ")");
                                            }
                                        }
                                    }
                                }
                            }

                            items.Add(item);
                        }

                        MedicalTableDetailComparison medicalTableDetailComparison = new MedicalTableDetailComparison(items);
                        medicalTableDetailComparisons.Add(medicalTableDetailComparison);
                    }
                }

                medicalTableBo.MedicalTableDetailComparisons = medicalTableDetailComparisons;
                #endregion

                #region Comparison detail legends information
                string legends = "";
                var legendBos = TreatyPricingMedicalTableUploadLegendService.GetByTreatyPricingMedicalTableVersionDetailId(detailBo.Id);

                foreach (var legendBo in legendBos)
                {
                    string legend = legendBo.Code + " - " + legendBo.Description;

                    legends += (legends == "" ? legend : "\n\n" + legend);
                }

                medicalTableBo.Legends = legends;
                #endregion

                medicalTableBos.Add(medicalTableBo);
            }
            #endregion

            return Json(new { medicalTables = medicalTableBos, sumAssuredIntervals });
        }

        [HttpPost]
        public JsonResult GenerateNonMedicalTableComparison(List<int> versionIds, List<int> distributionTierIds, List<RateComparisonAgeRange> ageRanges)
        {
            string nonMedicalTableCode = Util.GetConfig("NonMedicalTableCode");
            List<TreatyPricingMedicalTableBo> medicalTableBos = new List<TreatyPricingMedicalTableBo>();
            List<TreatyPricingMedicalTableVersionBo> versionBos = new List<TreatyPricingMedicalTableVersionBo>();
            List<TreatyPricingMedicalTableVersionDetailBo> detailBos = new List<TreatyPricingMedicalTableVersionDetailBo>();

            for (int i = 0; i < versionIds.Count; i++)
            {
                if (versionIds[i] > 0 && distributionTierIds[i] > 0)
                {
                    int detailId = TreatyPricingMedicalTableVersionDetailService.GetByVersionIdDistributionTierPickListDetailId(versionIds[i], distributionTierIds[i]).Id;
                    detailBos.Add(TreatyPricingMedicalTableVersionDetailService.Find(detailId));
                }
            }

            #region Get Medical Table information
            foreach (var detailBo in detailBos)
            {
                #region Comparison header information
                var versionBo = TreatyPricingMedicalTableVersionService.Find(detailBo.TreatyPricingMedicalTableVersionId);
                versionBos.Add(versionBo);

                var medicalTableBo = TreatyPricingMedicalTableService.Find(versionBo.TreatyPricingMedicalTableId);
                medicalTableBo.VersionBo = versionBo;
                medicalTableBo.DetailBo = detailBo;
                medicalTableBo.CedantInfo = medicalTableBo.TreatyPricingCedantBo.CedantBo.Code + " - " + medicalTableBo.TreatyPricingCedantBo.CedantBo.Name;

                //Get Linked Products
                string linkedProducts = "";
                List<string> linkedProductList = new List<string>();
                var productVersionBos = TreatyPricingProductVersionService.GetByTreatyPricingMedicalTableVersionId(versionBo.Id);

                if (productVersionBos.Count > 0)
                {
                    foreach (var productVersionBo in productVersionBos)
                    {
                        linkedProductList.Add(productVersionBo.TreatyPricingProductBo.Name);
                    }

                    linkedProducts = String.Join(", ", linkedProductList.ToArray());
                }

                medicalTableBo.LinkedProducts = linkedProducts;
                #endregion

                #region Comparison detail information
                List<MedicalTableDetailComparison> medicalTableDetailComparisons = new List<MedicalTableDetailComparison>();
                var medicalTableColumnBos = TreatyPricingMedicalTableUploadColumnService.GetByTreatyPricingMedicalTableVersionDetailId(detailBo.Id);
                var medicalTableRowBos = TreatyPricingMedicalTableUploadRowService.GetByTreatyPricingMedicalTableVersionDetailId(detailBo.Id);

                foreach (var ageRange in ageRanges)
                {
                    List<string> items = new List<string>();

                    if (ageRange.Minimum.HasValue && ageRange.Maximum.HasValue)
                    {
                        int minimumAge = ageRange.Minimum.Value;
                        int maximumAge = ageRange.Maximum.Value;
                        string itemFull = "";

                        foreach (var columnBo in medicalTableColumnBos)
                        {
                            if ((columnBo.MinimumAge >= minimumAge && columnBo.MinimumAge <= maximumAge) || (columnBo.MaximumAge >= minimumAge && columnBo.MaximumAge <= maximumAge) || (columnBo.MinimumAge <= minimumAge && columnBo.MaximumAge >= maximumAge))
                            {
                                string item = "";

                                foreach (var rowBo in medicalTableRowBos)
                                {
                                    var cellBo = TreatyPricingMedicalTableUploadCellService.GetByTreatyPricingMedicalTableUploadRowColumnId(rowBo.Id, columnBo.Id);

                                    if (cellBo != null)
                                    {
                                        List<string> cellBoCodes = cellBo.Code.Split(',').ToList();

                                        foreach (string cellBoCode in cellBoCodes)
                                        {
                                            if (cellBoCode == nonMedicalTableCode)
                                            {
                                                string maxSumAssured = "";

                                                #region Handle Maximum Sum Assured string suffix
                                                if (rowBo.MaximumSumAssured >= 2000000000)
                                                {
                                                    maxSumAssured = "Max";
                                                }
                                                else if (rowBo.MaximumSumAssured >= 1000000)
                                                {
                                                    maxSumAssured = (rowBo.MaximumSumAssured / 1000000).ToString() + "M";
                                                }
                                                else if (rowBo.MaximumSumAssured >= 1000)
                                                {
                                                    maxSumAssured = (rowBo.MaximumSumAssured / 1000).ToString() + "K";
                                                }
                                                else
                                                {
                                                    maxSumAssured = rowBo.MaximumSumAssured.ToString();
                                                }
                                                #endregion

                                                //if (item == "")
                                                //{
                                                //    item += (columnBo.MinimumAge == minimumAge && columnBo.MaximumAge == maximumAge ? maxSumAssured : maxSumAssured + "(" + columnBo.MinimumAge + "-" + columnBo.MaximumAge + ")");
                                                //}
                                                //else
                                                //{
                                                //    item += ", " + (columnBo.MinimumAge == minimumAge && columnBo.MaximumAge == maximumAge ? maxSumAssured : maxSumAssured + "(" + columnBo.MinimumAge + "-" + columnBo.MaximumAge + ")");
                                                //}
                                                item = (columnBo.MinimumAge == minimumAge && columnBo.MaximumAge == maximumAge ? maxSumAssured : maxSumAssured + "(" + columnBo.MinimumAge + "-" + columnBo.MaximumAge + ")");
                                            }
                                        }
                                    }
                                }

                                if (item != "")
                                {
                                    if (itemFull == "")
                                        itemFull += item;
                                    else
                                        itemFull += ", " + item;
                                }
                            }
                        }

                        items.Add(itemFull);

                        MedicalTableDetailComparison medicalTableDetailComparison = new MedicalTableDetailComparison(items);
                        medicalTableDetailComparisons.Add(medicalTableDetailComparison);
                    }
                }

                medicalTableBo.MedicalTableDetailComparisons = medicalTableDetailComparisons;
                #endregion

                medicalTableBos.Add(medicalTableBo);
            }
            #endregion

            return Json(new { medicalTables = medicalTableBos });
        }
        #endregion

        #region Financial Table Comparison
        [HttpPost]
        public JsonResult UpdateFinancialTableData(
            int? treatyPricingCedantId,
            int? benefitCode = null,
            int? distributionChannel = null,
            int? status = null,
            int? financialTableId = null
        )
        {
            var benefitCodeDropDowns = GetEmptyDropDownList();
            var distributionChannelDropDowns = GetEmptyDropDownList();
            var statusDropdowns = GetEmptyDropDownList();
            var financialTableIdDropDowns = GetEmptyDropDownList();
            var versionDropDowns = GetEmptyDropDownList();
            var distributionTierDropDowns = GetEmptyDropDownList();

            if (treatyPricingCedantId.HasValue)
            {
                var financialTableIds = TreatyPricingFinancialTableService.GetIdByFinancialTableIdsTreatyPricingCedantId(treatyPricingCedantId.Value);

                bool isBenefitCode = benefitCode.HasValue;
                bool isDistributionChannel = distributionChannel.HasValue;
                bool isStatus = status.HasValue;
                bool isFinancialTableId = financialTableId.HasValue;

                if (isBenefitCode)
                {
                    financialTableIds = TreatyPricingFinancialTableService.GetIdByFinancialTableIdsBenefitCode(financialTableIds, benefitCode.Value);
                    distributionChannelDropDowns.AddRange(GetFinancialDistributionChannelDropDownsByBenefitCode(financialTableIds, benefitCode.Value));
                }

                if (isDistributionChannel)
                {
                    financialTableIds = TreatyPricingFinancialTableService.GetIdByFinancialTableIdsDistributionChannel(financialTableIds, distributionChannel.Value);
                    statusDropdowns.AddRange(GetFinancialStatusDropDownsByDistributionChannel(financialTableIds, distributionChannel.Value));
                }

                if (isStatus)
                {
                    financialTableIds = TreatyPricingFinancialTableService.GetIdByFinancialTableIdsStatus(financialTableIds, status.Value);
                }

                if (isFinancialTableId)
                {
                    foreach (var versionBo in TreatyPricingFinancialTableVersionService.GetVersionByFinancialTableVersionIdsFinancialTableId(financialTableId.Value))
                    {
                        versionDropDowns.Add(new SelectListItem { Text = string.Format("{0}.0", versionBo.Version.ToString()), Value = versionBo.Id.ToString() });
                    }
                }

                benefitCodeDropDowns.AddRange(GetFinancialBenefitCodeDropDownsByTreatyPricingCedantId(treatyPricingCedantId.Value));
                financialTableIdDropDowns.AddRange(GetFinancialTableDropDownsByFinancialTableIds(financialTableIds));
            }

            return Json(new { benefitCodeDropDowns, distributionChannelDropDowns, statusDropdowns, financialTableIdDropDowns, versionDropDowns, distributionTierDropDowns });
        }

        // Get Distinct Financial Table Benefit Code By TreatyPricingCedantId
        public List<SelectListItem> GetFinancialBenefitCodeDropDownsByTreatyPricingCedantId(int cedantId)
        {
            var benefitCodeDropDowns = new List<SelectListItem> { };
            foreach (var benefitBo in TreatyPricingFinancialTableService.GetDistinctBenefitCodeByCedantId(cedantId))
            {
                benefitCodeDropDowns.Add(new SelectListItem { Text = benefitBo.ToString(), Value = benefitBo.Id.ToString() });
            }
            return benefitCodeDropDowns;
        }

        // Get Distinct Financial Table Distribution Channel By Benefit Code
        public List<SelectListItem> GetFinancialDistributionChannelDropDownsByBenefitCode(List<int> financialTableIds, int benefitCode)
        {
            var distributionChannelDropDowns = new List<SelectListItem> { };
            foreach (var distributionChannelBo in TreatyPricingFinancialTableService.GetDistinctDistributionChannelByBenefitCode(financialTableIds, benefitCode))
            {
                distributionChannelDropDowns.Add(new SelectListItem { Text = distributionChannelBo.ToString(), Value = distributionChannelBo.Id.ToString() });
            }
            return distributionChannelDropDowns;
        }

        // Get Distinct Financial Table Status By Distribution Channel
        public List<SelectListItem> GetFinancialStatusDropDownsByDistributionChannel(List<int> financialTableIds, int distributionChannel)
        {
            var statusDropdowns = new List<SelectListItem> { };
            foreach (var statusBo in TreatyPricingFinancialTableService.GetDistinctStatusByDistributionChannel(financialTableIds, distributionChannel))
            {
                statusDropdowns.Add(new SelectListItem { Text = statusBo.StatusName, Value = statusBo.Status.ToString() });
            }
            return statusDropdowns;
        }

        // Get Distinct Financial Table By Financial Table Ids
        public List<SelectListItem> GetFinancialTableDropDownsByFinancialTableIds(List<int> financialTableIds)
        {
            var financialTableIdDropDowns = new List<SelectListItem> { };
            foreach (var financialTableBo in TreatyPricingFinancialTableService.GetFinancialTablesByFinancialTableIds(financialTableIds))
            {
                financialTableIdDropDowns.Add(new SelectListItem { Text = (financialTableBo.FinancialTableId + " - " + financialTableBo.Name), Value = financialTableBo.Id.ToString() });
            }
            return financialTableIdDropDowns;
        }

        [HttpPost]
        public JsonResult GetFinancialTableComparisonDistributionTiers(int? versionId)
        {
            var distributionTierDropDowns = GetEmptyDropDownList();

            if (versionId.HasValue)
            {
                foreach (var detailBo in TreatyPricingFinancialTableVersionDetailService.GetByTreatyPricingFinancialTableVersionId(versionId.Value))
                {
                    distributionTierDropDowns.Add(new SelectListItem { Text = string.Format("{0}.0", detailBo.DistributionTierPickListDetailBo.Code.ToString()) + " - " + detailBo.Description, Value = detailBo.DistributionTierPickListDetailId.ToString() });
                }
            }

            return Json(new { distributionTierDropDowns });
        }

        [HttpPost]
        public JsonResult GenerateFinancialTableComparison(List<int> versionIds, List<int> distributionTierIds)
        {
            List<SumAssuredInterval> sumAssuredIntervals = new List<SumAssuredInterval>();
            List<SumAssuredIntervalUnformatted> sumAssuredIntervalUnformattedList = new List<SumAssuredIntervalUnformatted>();
            List<TreatyPricingFinancialTableBo> financialTableBos = new List<TreatyPricingFinancialTableBo>();
            List<TreatyPricingFinancialTableVersionBo> versionBos = new List<TreatyPricingFinancialTableVersionBo>();
            List<TreatyPricingFinancialTableVersionDetailBo> detailBos = new List<TreatyPricingFinancialTableVersionDetailBo>();
            List<TreatyPricingFinancialTableUploadBo> rowBos = new List<TreatyPricingFinancialTableUploadBo>();
            List<int> sumAssured = new List<int>();
            List<int> sumAssuredDistinct = new List<int>();

            #region Get Sum Assured Intervals
            for (int i = 0; i < versionIds.Count; i++)
            {
                if (versionIds[i] > 0 && distributionTierIds[i] > 0)
                {
                    int detailId = TreatyPricingFinancialTableVersionDetailService.GetByVersionIdDistributionTierPickListDetailId(versionIds[i], distributionTierIds[i]).Id;
                    detailBos.Add(TreatyPricingFinancialTableVersionDetailService.Find(detailId));
                    rowBos.AddRange(TreatyPricingFinancialTableUploadService.GetByTreatyPricingFinancialTableVersionDetailId(detailId));
                }
            }

            if (rowBos.Count > 0)
            {
                foreach (var rowBo in rowBos)
                {
                    sumAssured.Add(rowBo.MinimumSumAssured - 1);
                    sumAssured.Add(rowBo.MaximumSumAssured);
                }
            }

            sumAssuredDistinct.AddRange(sumAssured.Distinct());
            sumAssuredDistinct = sumAssuredDistinct.OrderBy(o => o).ToList();

            for (int i = 0; i < sumAssuredDistinct.Count; i++)
            {
                if (i != sumAssuredDistinct.Count - 1)
                {
                    string minimum = String.Format("{0:n0}", sumAssuredDistinct[i] + 1);
                    string maximum = sumAssuredDistinct[i + 1] >= 2000000000 ? "Max" : String.Format("{0:n0}", sumAssuredDistinct[i + 1]);
                    sumAssuredIntervals.Add(new SumAssuredInterval(minimum, maximum));
                    sumAssuredIntervalUnformattedList.Add(new SumAssuredIntervalUnformatted(sumAssuredDistinct[i] + 1, sumAssuredDistinct[i + 1]));
                }
            }
            #endregion

            #region Get Financial Table information
            foreach (var detailBo in detailBos)
            {
                #region Comparison header information
                var versionBo = TreatyPricingFinancialTableVersionService.Find(detailBo.TreatyPricingFinancialTableVersionId);
                versionBos.Add(versionBo);

                var financialTableBo = TreatyPricingFinancialTableService.Find(versionBo.TreatyPricingFinancialTableId);
                financialTableBo.VersionBo = versionBo;
                financialTableBo.DetailBo = detailBo;
                financialTableBo.CedantInfo = financialTableBo.TreatyPricingCedantBo.CedantBo.Code + " - " + financialTableBo.TreatyPricingCedantBo.CedantBo.Name;

                //Get Linked Products
                string linkedProducts = "";
                List<string> linkedProductList = new List<string>();
                var productVersionBos = TreatyPricingProductVersionService.GetByTreatyPricingFinancialTableVersionId(versionBo.Id);

                if (productVersionBos.Count > 0)
                {
                    foreach (var productVersionBo in productVersionBos)
                    {
                        linkedProductList.Add(productVersionBo.TreatyPricingProductBo.Name);
                    }

                    linkedProducts = String.Join(", ", linkedProductList.ToArray());
                }

                financialTableBo.LinkedProducts = linkedProducts;
                #endregion

                #region Comparison detail legends information
                string legends = "";
                var legendBos = TreatyPricingFinancialTableUploadLegendService.GetByTreatyPricingFinancialTableVersionDetailId(detailBo.Id);

                foreach (var legendBo in legendBos)
                {
                    string legend = legendBo.Code + " - " + legendBo.Description;

                    legends += (legends == "" ? legend : "\n\n" + legend);
                }

                financialTableBo.Legends = legends;
                #endregion

                #region Comparison detail information
                List<string> financialTableDetailComparisons = new List<string>();
                List<string> items = new List<string>();

                foreach (var sumAssuredInterval in sumAssuredIntervalUnformattedList)
                {
                    int minimumSumAssured = sumAssuredInterval.Start;
                    int maximumSumAssured = sumAssuredInterval.End;
                    string item = "";

                    var financialTableRowBo = TreatyPricingFinancialTableUploadService.GetByTreatyPricingFinancialTableVersionDetailIdForComparison(detailBo.Id, minimumSumAssured, maximumSumAssured);

                    if (financialTableRowBo != null)
                    {
                        if (item == "")
                        {
                            item += financialTableRowBo.Code;
                        }
                        else
                        {
                            item += ", " + financialTableRowBo.Code;
                        }
                    }

                    items.Add(item);
                }

                financialTableBo.FinancialTableDetailComparisons = items;
                #endregion

                financialTableBos.Add(financialTableBo);
            }
            #endregion

            return Json(new { financialTables = financialTableBos, sumAssuredIntervals });
        }
        #endregion

        #region Treaty Workflow Report
        [HttpPost]
        public JsonResult GenerateDraftStatusOverviewByRetroParty(string effStartDate, string effEndDate)
        {
            var effectiveStartDate = DateTime.Parse(effStartDate);
            var effectiveEndDate = DateTime.Parse(effEndDate);

            if (effectiveStartDate > effectiveEndDate)
            {
                return Json(new { error = "Effective Start Date must be earlier than Effective End Date" });
            }

            List<TreatyPricingTreatyWorkflowBo> treatyPricingTreatyWorkflowBos = new List<TreatyPricingTreatyWorkflowBo> { };

            var retroPartiesDistinct = TreatyPricingTreatyWorkflowService.GetAll().Select(a => a.InwardRetroPartyDetailId).Distinct().ToList();

            foreach (var retroParty in retroPartiesDistinct)
            {
                TreatyPricingTreatyWorkflowBo bo = new TreatyPricingTreatyWorkflowBo();
                bo.InwardRetroPartyName = RetroPartyService.Find(retroParty).Code;
                bo.SignedCountInTreaty = TreatyPricingTreatyWorkflowService.GetSignedCountByInwardRetroPartyDetaiIdInTreaty(retroParty, effectiveStartDate, effectiveEndDate);
                bo.LessThan6MonthCountInTreaty = TreatyPricingTreatyWorkflowService.GetLessThan6MonthsCountByInwardRetroPartyDetailIdInTreaty(retroParty, effectiveStartDate, effectiveEndDate);
                bo.LessThan12MonthCountInTreaty = TreatyPricingTreatyWorkflowService.GetLessThan12MonthsCountByInwardRetroPartyDetailIdInTreaty(retroParty, effectiveStartDate, effectiveEndDate);
                bo.MoreThan12MonthCountInTreaty = TreatyPricingTreatyWorkflowService.GetMoreThan12MonthsCountByInwardRetroPartyDetailIdInTreaty(retroParty, effectiveStartDate, effectiveEndDate);
                bo.TotalCountInTreaty = bo.LessThan6MonthCountInTreaty + bo.LessThan12MonthCountInTreaty + bo.MoreThan12MonthCountInTreaty;
                bo.SignedCountInAddendum = TreatyPricingTreatyWorkflowService.GetSignedCountByInwardRetroPartyDetaiIdInAddendum(retroParty, effectiveStartDate, effectiveEndDate);
                bo.LessThan6MonthCountInAddendum = TreatyPricingTreatyWorkflowService.GetLessThan6MonthsCountByInwardRetroPartyDetailIdInAddendum(retroParty, effectiveStartDate, effectiveEndDate);
                bo.LessThan12MonthCountInAddendum = TreatyPricingTreatyWorkflowService.GetLessThan12MonthsCountByInwardRetroPartyDetailIdInAddendum(retroParty, effectiveStartDate, effectiveEndDate);
                bo.MoreThan12MonthCountInAddendum = TreatyPricingTreatyWorkflowService.GetMoreThan12MonthsCountByInwardRetroPartyDetailIdInAddendum(retroParty, effectiveStartDate, effectiveEndDate);
                bo.TotalCountInAddendum = bo.LessThan6MonthCountInAddendum + bo.LessThan12MonthCountInAddendum + bo.MoreThan12MonthCountInAddendum;

                treatyPricingTreatyWorkflowBos.Add(bo);
            }

            return Json(new { treatyWorkflowDraftStatuses = treatyPricingTreatyWorkflowBos });
        }

        [HttpPost]
        public JsonResult GenerateDraftStatusOverviewByBusinessOrigin(string effStartDate, string effEndDate)
        {
            var effectiveStartDate = DateTime.Parse(effStartDate);
            var effectiveEndDate = DateTime.Parse(effEndDate);

            if (effectiveStartDate > effectiveEndDate)
            {
                return Json(new { error = "Effective Start Date must be earlier than Effective End Date" });
            }

            List<TreatyPricingTreatyWorkflowBo> treatyPricingTreatyWorkflowBosWM = new List<TreatyPricingTreatyWorkflowBo> { };
            List<TreatyPricingTreatyWorkflowBo> treatyPricingTreatyWorkflowBosOM = new List<TreatyPricingTreatyWorkflowBo> { };
            TreatyPricingTreatyWorkflowBo workflowBo = new TreatyPricingTreatyWorkflowBo();

            var retroPartiesDistinctWM = TreatyPricingTreatyWorkflowService.GetAll().Where(a => a.BusinessOriginPickListDetailBo?.Code == "WM").Select(a => a.InwardRetroPartyDetailId).Distinct().ToList();
            var retroPartiesDistinctOM = TreatyPricingTreatyWorkflowService.GetAll().Where(a => a.BusinessOriginPickListDetailBo?.Code == "OM").Select(a => a.InwardRetroPartyDetailId).Distinct().ToList();

            if (retroPartiesDistinctWM != null)
            {
                foreach (var retroParty in retroPartiesDistinctWM)
                {
                    TreatyPricingTreatyWorkflowBo boWM = new TreatyPricingTreatyWorkflowBo();
                    boWM.InwardRetroPartyName = RetroPartyService.Find(retroParty).Code;

                    boWM.SignedCountInTreaty = TreatyPricingTreatyWorkflowService.GetSignedCountByInwardRetroPartyDetaiIdInTreatyWM(retroParty, effectiveStartDate, effectiveEndDate);
                    boWM.LessThan6MonthCountInTreaty = TreatyPricingTreatyWorkflowService.GetLessThan6MonthsCountByInwardRetroPartyDetailIdInTreatyWM(retroParty, effectiveStartDate, effectiveEndDate);
                    boWM.LessThan12MonthCountInTreaty = TreatyPricingTreatyWorkflowService.GetLessThan12MonthsCountByInwardRetroPartyDetailIdInTreatyWM(retroParty, effectiveStartDate, effectiveEndDate);
                    boWM.MoreThan12MonthCountInTreaty = TreatyPricingTreatyWorkflowService.GetMoreThan12MonthsCountByInwardRetroPartyDetailIdInTreatyWM(retroParty, effectiveStartDate, effectiveEndDate);
                    boWM.TotalCountInTreaty = boWM.LessThan6MonthCountInTreaty + boWM.LessThan12MonthCountInTreaty + boWM.MoreThan12MonthCountInTreaty;
                    boWM.SignedCountInAddendum = TreatyPricingTreatyWorkflowService.GetSignedCountByInwardRetroPartyDetaiIdInAddendumWM(retroParty, effectiveStartDate, effectiveEndDate);
                    boWM.LessThan6MonthCountInAddendum = TreatyPricingTreatyWorkflowService.GetLessThan6MonthsCountByInwardRetroPartyDetailIdInAddendumWM(retroParty, effectiveStartDate, effectiveEndDate);
                    boWM.LessThan12MonthCountInAddendum = TreatyPricingTreatyWorkflowService.GetLessThan12MonthsCountByInwardRetroPartyDetailIdInAddendumWM(retroParty, effectiveStartDate, effectiveEndDate);
                    boWM.MoreThan12MonthCountInAddendum = TreatyPricingTreatyWorkflowService.GetMoreThan12MonthsCountByInwardRetroPartyDetailIdInAddendumWM(retroParty, effectiveStartDate, effectiveEndDate);
                    boWM.TotalCountInAddendum = boWM.LessThan6MonthCountInAddendum + boWM.LessThan12MonthCountInAddendum + boWM.MoreThan12MonthCountInAddendum;

                    treatyPricingTreatyWorkflowBosWM.Add(boWM);
                }
            }

            if (retroPartiesDistinctOM != null)
            {
                foreach (var retroParty in retroPartiesDistinctOM)
                {
                    TreatyPricingTreatyWorkflowBo boOM = new TreatyPricingTreatyWorkflowBo();
                    boOM.InwardRetroPartyName = RetroPartyService.Find(retroParty).Code;

                    boOM.SignedCountInTreaty = TreatyPricingTreatyWorkflowService.GetSignedCountByInwardRetroPartyDetaiIdInTreatyOM(retroParty, effectiveStartDate, effectiveEndDate);
                    boOM.LessThan6MonthCountInTreaty = TreatyPricingTreatyWorkflowService.GetLessThan6MonthsCountByInwardRetroPartyDetailIdInTreatyOM(retroParty, effectiveStartDate, effectiveEndDate);
                    boOM.LessThan12MonthCountInTreaty = TreatyPricingTreatyWorkflowService.GetLessThan12MonthsCountByInwardRetroPartyDetailIdInTreatyOM(retroParty, effectiveStartDate, effectiveEndDate);
                    boOM.MoreThan12MonthCountInTreaty = TreatyPricingTreatyWorkflowService.GetMoreThan12MonthsCountByInwardRetroPartyDetailIdInTreatyOM(retroParty, effectiveStartDate, effectiveEndDate);
                    boOM.TotalCountInTreaty = boOM.LessThan6MonthCountInTreaty + boOM.LessThan12MonthCountInTreaty + boOM.MoreThan12MonthCountInTreaty;
                    boOM.SignedCountInAddendum = TreatyPricingTreatyWorkflowService.GetSignedCountByInwardRetroPartyDetaiIdInAddendumOM(retroParty, effectiveStartDate, effectiveEndDate);
                    boOM.LessThan6MonthCountInAddendum = TreatyPricingTreatyWorkflowService.GetLessThan6MonthsCountByInwardRetroPartyDetailIdInAddendumOM(retroParty, effectiveStartDate, effectiveEndDate);
                    boOM.LessThan12MonthCountInAddendum = TreatyPricingTreatyWorkflowService.GetLessThan12MonthsCountByInwardRetroPartyDetailIdInAddendumOM(retroParty, effectiveStartDate, effectiveEndDate);
                    boOM.MoreThan12MonthCountInAddendum = TreatyPricingTreatyWorkflowService.GetMoreThan12MonthsCountByInwardRetroPartyDetailIdInAddendumOM(retroParty, effectiveStartDate, effectiveEndDate);
                    boOM.TotalCountInAddendum = boOM.LessThan6MonthCountInAddendum + boOM.LessThan12MonthCountInAddendum + boOM.MoreThan12MonthCountInAddendum;

                    treatyPricingTreatyWorkflowBosOM.Add(boOM);
                }
            }

            workflowBo.TotalSignedInTreatyWM = treatyPricingTreatyWorkflowBosWM.Select(a => a.SignedCountInTreaty).ToList().Sum();
            workflowBo.TotalLessThan6MonthCountInTreatyWM = treatyPricingTreatyWorkflowBosWM.Select(a => a.LessThan6MonthCountInTreaty).ToList().Sum();
            workflowBo.TotalLessThan12MonthCountInTreatyWM = treatyPricingTreatyWorkflowBosWM.Select(a => a.LessThan12MonthCountInTreaty).ToList().Sum();
            workflowBo.TotalMoreThan12MonthCountInTreatyWM = treatyPricingTreatyWorkflowBosWM.Select(a => a.MoreThan12MonthCountInTreaty).ToList().Sum();
            workflowBo.TotalInTreatyWM = workflowBo.TotalLessThan6MonthCountInTreatyWM + workflowBo.TotalLessThan12MonthCountInTreatyWM + workflowBo.TotalMoreThan12MonthCountInTreatyWM;
            workflowBo.TotalSignedInAddendumWM = treatyPricingTreatyWorkflowBosWM.Select(a => a.SignedCountInAddendum).ToList().Sum();
            workflowBo.TotalLessThan6MonthCountInAddendumWM = treatyPricingTreatyWorkflowBosWM.Select(a => a.LessThan6MonthCountInTreaty).ToList().Sum();
            workflowBo.TotalLessThan12MonthCountInAddendumWM = treatyPricingTreatyWorkflowBosWM.Select(a => a.LessThan12MonthCountInAddendum).ToList().Sum();
            workflowBo.TotalMoreThan12MonthCountInAddendumWM = treatyPricingTreatyWorkflowBosWM.Select(a => a.MoreThan12MonthCountInAddendum).ToList().Sum();
            workflowBo.TotalInAddendumWM = workflowBo.TotalLessThan6MonthCountInAddendumWM + workflowBo.TotalLessThan12MonthCountInAddendumWM + workflowBo.TotalMoreThan12MonthCountInAddendumWM;

            workflowBo.TotalSignedInTreatyOM = treatyPricingTreatyWorkflowBosOM.Select(a => a.SignedCountInTreaty).ToList().Sum();
            workflowBo.TotalLessThan6MonthCountInTreatyOM = treatyPricingTreatyWorkflowBosOM.Select(a => a.LessThan6MonthCountInTreaty).ToList().Sum();
            workflowBo.TotalLessThan12MonthCountInTreatyOM = treatyPricingTreatyWorkflowBosOM.Select(a => a.LessThan12MonthCountInTreaty).ToList().Sum();
            workflowBo.TotalMoreThan12MonthCountInTreatyOM = treatyPricingTreatyWorkflowBosOM.Select(a => a.MoreThan12MonthCountInTreaty).ToList().Sum();
            workflowBo.TotalInTreatyOM = workflowBo.TotalLessThan6MonthCountInTreatyOM + workflowBo.TotalLessThan12MonthCountInTreatyOM + workflowBo.TotalMoreThan12MonthCountInTreatyOM;
            workflowBo.TotalSignedInAddendumOM = treatyPricingTreatyWorkflowBosOM.Select(a => a.SignedCountInAddendum).ToList().Sum();
            workflowBo.TotalLessThan6MonthCountInAddendumOM = treatyPricingTreatyWorkflowBosOM.Select(a => a.LessThan6MonthCountInAddendum).ToList().Sum();
            workflowBo.TotalLessThan12MonthCountInAddendumOM = treatyPricingTreatyWorkflowBosOM.Select(a => a.LessThan12MonthCountInAddendum).ToList().Sum();
            workflowBo.TotalMoreThan12MonthCountInAddendumOM = treatyPricingTreatyWorkflowBosOM.Select(a => a.MoreThan12MonthCountInAddendum).ToList().Sum();
            workflowBo.TotalInAddendumOM = workflowBo.TotalLessThan6MonthCountInTreatyOM + workflowBo.TotalLessThan12MonthCountInTreatyOM + workflowBo.TotalMoreThan12MonthCountInAddendumOM;

            workflowBo.TotalSignedInTreatyWMandOM = workflowBo.TotalSignedInTreatyWM + workflowBo.TotalSignedInTreatyOM;
            workflowBo.TotalLessThan6MonthCountInTreatyWMandOM = workflowBo.TotalLessThan6MonthCountInTreatyWM + workflowBo.TotalLessThan6MonthCountInTreatyOM;
            workflowBo.TotalLessThan12MonthCountInTreatyWMandOM = workflowBo.TotalLessThan12MonthCountInTreatyWM + workflowBo.TotalLessThan12MonthCountInTreatyOM;
            workflowBo.TotalMoreThan12MonthCountInTreatyWMandOM = workflowBo.TotalMoreThan12MonthCountInTreatyWM + workflowBo.TotalMoreThan12MonthCountInTreatyOM;
            workflowBo.TotalInTreatyWMandOM = workflowBo.TotalLessThan6MonthCountInTreatyWMandOM + workflowBo.TotalLessThan12MonthCountInTreatyWMandOM + workflowBo.TotalMoreThan12MonthCountInTreatyWMandOM;
            workflowBo.TotalSignedInAddendumWMandOM = workflowBo.TotalSignedInAddendumWM + workflowBo.TotalSignedInAddendumOM;
            workflowBo.TotalLessThan6MonthCountInAddendumWMandOM = workflowBo.TotalLessThan6MonthCountInAddendumWM + workflowBo.TotalLessThan6MonthCountInAddendumOM;
            workflowBo.TotalLessThan12MonthCountInAddendumWMandOM = workflowBo.TotalLessThan12MonthCountInAddendumWM + workflowBo.TotalLessThan12MonthCountInAddendumOM;
            workflowBo.TotalMoreThan12MonthCountInAddendumWMandOM = workflowBo.TotalMoreThan12MonthCountInAddendumWM + workflowBo.TotalMoreThan12MonthCountInAddendumOM;
            workflowBo.TotalInAddendumWMandOM = workflowBo.TotalLessThan6MonthCountInAddendumWMandOM + workflowBo.TotalLessThan12MonthCountInAddendumWMandOM + workflowBo.TotalMoreThan12MonthCountInAddendumWMandOM;

            return Json(new { treatyWorkflowDraftStatusesWM = treatyPricingTreatyWorkflowBosWM, treatyWorkflowDraftStatusesOM = treatyPricingTreatyWorkflowBosOM, treatyWorkflowDraftStatusesTotal = workflowBo });
        }

        [HttpPost]
        public JsonResult GenerateTreatyWeeklyMonthlyQuarterlyReport(int type, string startDate, string endDate)
        {
            // type 1 = weekly
            // type 2 = monthly
            // type 3 = quarterly

            var startDateF = DateTime.Parse(startDate);
            var endDateF = DateTime.Parse(endDate);

            if (startDateF > endDateF)
            {
                return Json(new { error = "Start Date Sent to Client (1st) must be earlier than End Date Sent to Client (1st)" });
            }

            List<TreatyPricingTreatyWorkflowBo> treatyPricingTreatyWorkflowBos = new List<TreatyPricingTreatyWorkflowBo> { };

            if (type == 1)
            {
                var filterWorkflowVersion = TreatyPricingTreatyWorkflowVersionService.GetTreatyWorkflowLatestVersionBetweenDates(startDateF, endDateF);
                var workflowVersionWithoutDateSentToClient1st = TreatyPricingTreatyWorkflowVersionService.GetTreatyWorkflowLatestVersionWithoutDateSentToClient1st();

                foreach (var workflowVer in filterWorkflowVersion)
                {
                    var bo = TreatyPricingTreatyWorkflowService.Find(workflowVer.TreatyPricingTreatyWorkflowId);
                    bo.ReportedDateStr = workflowVer.ReportedDate?.ToString(Util.GetDateFormat());
                    bo.DateSentToClient1st = workflowVer.DateSentToClient1st?.ToString(Util.GetDateFormat());
                    bo.ReportingStatus = TreatyPricingTreatyWorkflowBo.GetReportingStatusName(TreatyPricingTreatyWorkflowService.GetReportingStatus(workflowVer.ReportedDate, workflowVer.DateSentToClient1st, startDateF, endDateF));
                    treatyPricingTreatyWorkflowBos.Add(bo);
                }

                foreach (var workflowVer in workflowVersionWithoutDateSentToClient1st)
                {
                    var bo = TreatyPricingTreatyWorkflowService.Find(workflowVer.TreatyPricingTreatyWorkflowId);
                    bo.ReportedDateStr = workflowVer.ReportedDate?.ToString(Util.GetDateFormat());
                    bo.DateSentToClient1st = workflowVer.DateSentToClient1st?.ToString(Util.GetDateFormat());
                    bo.ReportingStatus = TreatyPricingTreatyWorkflowBo.GetReportingStatusName(TreatyPricingTreatyWorkflowBo.ReportingStatusDoing);
                    treatyPricingTreatyWorkflowBos.Add(bo);
                }
            }

            if (type == 2 || type == 3)
            {
                var filterWorkflowVersion = TreatyPricingTreatyWorkflowVersionService.GetTreatyWorkflowLatestVersionBetweenDates(startDateF, endDateF);

                foreach (var workflowVer in filterWorkflowVersion)
                {
                    var bo = TreatyPricingTreatyWorkflowService.Find(workflowVer.TreatyPricingTreatyWorkflowId);
                    bo.ReportedDateStr = workflowVer.ReportedDate?.ToString(Util.GetDateFormat());
                    bo.DateSentToClient1st = workflowVer.DateSentToClient1st?.ToString(Util.GetDateFormat());
                    bo.ReportingStatus = TreatyPricingTreatyWorkflowBo.GetReportingStatusName(TreatyPricingTreatyWorkflowService.GetReportingStatus(workflowVer.ReportedDate, workflowVer.DateSentToClient1st, startDateF, endDateF));
                    treatyPricingTreatyWorkflowBos.Add(bo);
                }
            }

            return Json(new { treatyPricingTreatyWorkflowBos });
        }

        [HttpPost]
        public JsonResult GenerateKPIMonitoringReport(string effStartDate, string effEndDate)
        {
            var effectiveStartDate = DateTime.Parse(effStartDate);
            var effectiveEndDate = DateTime.Parse(effEndDate);

            if (effectiveStartDate > effectiveEndDate)
            {
                return Json(new { error = "Effective Start Date must be earlier than Effective End Date" });
            }

            List<TreatyPricingTreatyWorkflowBo> treatyPricingTreatyWorkflowBos = new List<TreatyPricingTreatyWorkflowBo> { };

            var treatyWorkflowInBetweenDates = TreatyPricingTreatyWorkflowService.GetAll()
                .Where(a => a.EffectiveAt > effectiveStartDate && a.EffectiveAt < effectiveEndDate)
                .ToList();

            foreach (var bo in treatyWorkflowInBetweenDates)
            {
                var firstVerBo = TreatyPricingTreatyWorkflowVersionService.GetByTreatyPricingTreatyWorkflowId(bo.Id).OrderBy(q => q.Version).FirstOrDefault();
                var latestVerBo = TreatyPricingTreatyWorkflowVersionService.GetByTreatyPricingTreatyWorkflowId(bo.Id).OrderByDescending(q => q.Version).FirstOrDefault();


                if (firstVerBo.DateSentToReviewer1st.HasValue && firstVerBo.DateSentToClient1st.HasValue && latestVerBo.SignedDate.HasValue && latestVerBo.LatestRevisionDate.HasValue && latestVerBo.RequestDate.HasValue)
                {
                    var workflowBo = TreatyPricingTreatyWorkflowService.Find(bo.Id);

                    workflowBo.RequestDate = latestVerBo.RequestDate?.ToString(Util.GetDateFormat());
                    workflowBo.DateSentToReviewer1st = firstVerBo.DateSentToReviewer1st?.ToString(Util.GetDateFormat());
                    workflowBo.DateSentToClient1st = firstVerBo.DateSentToClient1st?.ToString(Util.GetDateFormat());
                    workflowBo.LatestRevisionDate = latestVerBo.LatestRevisionDate?.ToString(Util.GetDateFormat());
                    workflowBo.SignedDate = latestVerBo.SignedDate?.ToString(Util.GetDateFormat());

                    workflowBo.Days1stDraftToReviewer = CalculateDaysWithHolidays(latestVerBo.RequestDate.Value, firstVerBo.DateSentToReviewer1st.Value);
                    workflowBo.Days1stDraftToCedant = CalculateDaysWithHolidays(latestVerBo.RequestDate.Value, firstVerBo.DateSentToClient1st.Value);
                    workflowBo.DaysSigned = CalculateDaysWithHolidays(latestVerBo.RequestDate.Value, latestVerBo.SignedDate.Value);
                    workflowBo.FollowUpFrequency = Convert.ToDouble((latestVerBo.LatestRevisionDate.Value - firstVerBo.DateSentToClient1st.Value).Days) / Convert.ToDouble(latestVerBo.Version);

                    treatyPricingTreatyWorkflowBos.Add(workflowBo);
                }
            }

            return Json(new { treatyPricingTreatyWorkflowBos });
        }

        private int CalculateDaysWithHolidays(DateTime startDate, DateTime endDate)
        {
            int days = 0;
            DateTime currentDate = startDate;

            while (currentDate.Date < endDate.Date)
            {
                if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    currentDate = currentDate.AddDays(1);
                    continue;
                }

                if (PublicHolidayDetailService.IsExists(currentDate))
                {
                    currentDate = currentDate.AddDays(1);
                    continue;
                }

                currentDate = currentDate.AddDays(1);
                days++;
            }

            return days;
        }

        #endregion

        #region UW Questionnaire Comparison Report
        [HttpPost]
        public JsonResult UpdateUwQuestionnaireData(
            int? treatyPricingCedantId,
            string benefitCode,
            string distributionChannel,
            int? status = null,
            int? uwQuestionnaireId = null,
            int? uwQuestionnaireVersionId = null
        )
        {
            var uwQuestionnaireDropDowns = GetEmptyDropDownList();
            var benefitCodeDropDowns = GetEmptyDropDownList();
            var distributionChannelDropDowns = GetEmptyDropDownList();
            var statusDropDowns = GetEmptyDropDownList();
            var versionDropDowns = GetEmptyDropDownList();
            var questionnaireTypeDropDowns = GetEmptyDropDownList();

            if (treatyPricingCedantId.HasValue)
            {
                bool isBenefitCode = !string.IsNullOrEmpty(benefitCode);
                bool isDistributionChannel = !string.IsNullOrEmpty(distributionChannel);
                bool isStatus = status.HasValue;
                bool isUwQuestionnaireId = uwQuestionnaireId.HasValue;
                bool isUwQuestionnaireVersionId = uwQuestionnaireVersionId.HasValue;

                if (isBenefitCode && isDistributionChannel && isStatus)
                {
                    var bos = TreatyPricingUwQuestionnaireService.GetByParams(treatyPricingCedantId, benefitCode, distributionChannel, status);
                    foreach (var bo in bos)
                        uwQuestionnaireDropDowns.Add(new SelectListItem { Text = string.Format("{0} - {1}", bo.Code, bo.Name), Value = bo.Id.ToString() });

                    if (isUwQuestionnaireId)
                    {
                        var versionsBo = bos.Where(q => q.Id == uwQuestionnaireId).FirstOrDefault();

                        foreach (var versionBo in versionsBo.TreatyPricingUwQuestionnaireVersionBos)
                            versionDropDowns.Add(new SelectListItem { Text = string.Format("{0}.0", versionBo.Version), Value = versionBo.Id.ToString() });
                    }
                }
                else
                {
                    var bos = TreatyPricingUwQuestionnaireService.GetByTreatyPricingCedantIdWithForeign(treatyPricingCedantId.Value);
                    foreach (var bo in bos)
                        uwQuestionnaireDropDowns.Add(new SelectListItem { Text = string.Format("{0} - {1}", bo.Code, bo.Name), Value = bo.Id.ToString() });

                    if (isUwQuestionnaireId)
                    {
                        var versionsBo = bos.Where(q => q.Id == uwQuestionnaireId).FirstOrDefault();

                        foreach (var versionBo in versionsBo.TreatyPricingUwQuestionnaireVersionBos)
                            versionDropDowns.Add(new SelectListItem { Text = string.Format("{0}.0", versionBo.Version), Value = versionBo.Id.ToString() });
                    }
                }

                if (isUwQuestionnaireVersionId)
                {
                    var bo = TreatyPricingUwQuestionnaireVersionService.Find(uwQuestionnaireVersionId);
                    questionnaireTypeDropDowns.Add(new SelectListItem { Text = TreatyPricingUwQuestionnaireVersionBo.GetTypeName(bo.Type), Value = bo.Type.ToString() });
                }

                benefitCodeDropDowns.AddRange(DropDownBenefitCode(treatyPricingCedantId.Value));
                distributionChannelDropDowns.AddRange(DropDownDistributionChannel(treatyPricingCedantId.Value));
                statusDropDowns.AddRange(DropDownStatus());
            }

            return Json(new { uwQuestionnaireDropDowns, benefitCodeDropDowns, distributionChannelDropDowns, statusDropDowns, versionDropDowns, questionnaireTypeDropDowns });
        }

        public List<SelectListItem> GetUwQuestionnaireDropDownsByTreatyPricingCedantId(int cedantId)
        {
            var uwQuestionnaireDropDowns = new List<SelectListItem> { };
            foreach (var uw in TreatyPricingUwQuestionnaireService.GetByTreatyPricingCedantId(cedantId))
            {
                uwQuestionnaireDropDowns.Add(new SelectListItem { Text = uw.Name, Value = uw.Id.ToString() });
            }
            return uwQuestionnaireDropDowns;
        }

        public List<SelectListItem> DropDownBenefitCode(int cedantId)
        {
            var items = new List<SelectListItem> { };
            foreach (var benefitCode in TreatyPricingUwQuestionnaireService.GetDistinctBenefitCodeByCedantId(cedantId))
            {
                items.Add(new SelectListItem { Text = benefitCode, Value = benefitCode });
            }
            return items;
        }

        public List<SelectListItem> DropDownDistributionChannel(int cedantId)
        {
            var items = new List<SelectListItem> { };
            foreach (var distributionChannel in TreatyPricingUwQuestionnaireService.GetDistinctDistributionChannelByCedantId(cedantId))
            {
                items.Add(new SelectListItem { Text = distributionChannel, Value = distributionChannel });
            }
            return items;
        }

        public List<SelectListItem> DropDownStatus()
        {
            var items = new List<SelectListItem> { };
            foreach (var i in Enumerable.Range(1, TreatyPricingUwQuestionnaireBo.StatusMax))
            {
                items.Add(new SelectListItem { Text = TreatyPricingUwQuestionnaireBo.GetStatusName(i), Value = i.ToString() });
            }
            return items;
        }

        public List<SelectListItem> DropDownUwQuestionnaire(int cedantId, string benefitCode, string distributionCode, int status)
        {
            var items = new List<SelectListItem> { };
            foreach (var uw in TreatyPricingUwQuestionnaireService.GetByParams(cedantId, benefitCode, distributionCode, status))
            {
                items.Add(new SelectListItem { Text = string.Format("{0} - {1}", uw.Code, uw.Name), Value = uw.Id.ToString() });
            }
            return items;
        }

        [HttpPost]
        public JsonResult GenerateUnderwritingQuestionnaireComparison(List<int> versionIds)
        {
            List<TreatyPricingUwQuestionnaireVersionBo> uwQuestionnaireVersionBos = new List<TreatyPricingUwQuestionnaireVersionBo>();
            foreach (int versionId in versionIds)
            {
                var versionBo = TreatyPricingUwQuestionnaireVersionService.Find(versionId);
                if (versionBo != null)
                {
                    //Get Linked Products
                    string linkedProducts = "";
                    List<string> linkedProductList = new List<string>();
                    var productBos = TreatyPricingProductService.GetUwQuestionnaireProduct(versionBo.TreatyPricingUwQuestionnaireId);

                    if (productBos.Count > 0)
                    {
                        foreach (var productBo in productBos)
                        {
                            linkedProductList.Add(string.Format("{0} - {1}", productBo.Code, productBo.Name));
                        }

                        linkedProducts = string.Join(", ", linkedProductList.ToArray());
                    }
                    versionBo.LinkedProducts = linkedProducts;
                    versionBo.VersionStr = string.Format("{0}.0", versionBo.Version);

                    var uwQuestionnaireBo = TreatyPricingUwQuestionnaireService.Find(versionBo.TreatyPricingUwQuestionnaireId);
                    versionBo.UwQuestionnaireId = uwQuestionnaireBo.Code;
                    versionBo.UwQuestionnaireName = uwQuestionnaireBo.Name;
                    versionBo.StatusName = uwQuestionnaireBo.StatusName;
                    versionBo.Description = uwQuestionnaireBo.Description;
                    versionBo.BenefitCode = uwQuestionnaireBo.BenefitCode;
                    versionBo.DistributionChannel = uwQuestionnaireBo.DistributionChannel;
                    versionBo.CedantName = string.Format("{0} - {1}", uwQuestionnaireBo.TreatyPricingCedantBo.CedantBo.Code, uwQuestionnaireBo.TreatyPricingCedantBo.CedantBo.Name);

                    var uwQuestionnaireDetailBos = TreatyPricingUwQuestionnaireVersionDetailService.GetByTreatyPricingUwQuestionnaireVersionId(versionId);
                    versionBo.TreatyPricingUwQuestionnaireVersionDetailBos = uwQuestionnaireDetailBos;
                }
                uwQuestionnaireVersionBos.Add(versionBo);
            }
            var uwQuestionnaireCategoryBos = UwQuestionnaireCategoryService.Get();

            return Json(new { uwQuestionnaireVersionBos, uwQuestionnaireCategoryBos });
        }

        #endregion

        #region Advantage Program Comparison
        public JsonResult GenerateAdvantageProgramComparison(string treatyPricingCedantId, string transposeExcel)
        {
            List<string> errors = new List<string>();

            try
            {
                TrailObject trail = GetNewTrailObject();

                var bo = new TreatyPricingReportGenerationBo()
                {
                    ReportName = "Advantage Program Comparison Report",
                    ReportParams = treatyPricingCedantId + "|" + transposeExcel,
                    Status = TreatyPricingReportGenerationBo.StatusPending,
                    CreatedById = AuthUserId,
                    UpdatedById = AuthUserId,
                };
                Result = TreatyPricingReportGenerationService.Create(ref bo, ref trail);

                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Create Treaty Pricing Report Generation"
                    );
                }
            }
            catch (Exception ex)
            {
                errors.Add(string.Format("Error occurred. Error details: {0}", ex.Message));
            }

            return Json(new { errors });
        }
        #endregion

        #region Product & Benefit Comparison
        [HttpPost]
        public JsonResult UpdateProductComparisonData(
            int? treatyPricingCedantId,
            string underwritingMethod = null,
            string quotationName = null,
            int? productId = null,
            string targetSegment = null,
            int? productType = null,
            string distributionChannel = null
        )
        {
            var underwritingMethodCodes = new List<string> { };
            var quotationNameDropDowns = GetEmptyDropDownList();
            var productNameDropDowns = GetEmptyDropDownList();
            var targetSegmentCodes = new List<string> { };
            var productTypeDropdowns = GetEmptyDropDownList();
            var distributionChannelCodes = new List<string> { };
            var versionDropDowns = GetEmptyDropDownList();
            var benefitCodes = new List<string> { };
            var productVersionIds = new List<int> { };

            if (treatyPricingCedantId.HasValue)
            {
                var productIds = TreatyPricingProductService.GetIdByProductIdsTreatyPricingCedantId(treatyPricingCedantId.Value);

                bool isUnderwritingMethod = !string.IsNullOrEmpty(underwritingMethod);
                bool isQuotationName = !string.IsNullOrEmpty(quotationName);
                bool isProduct = productId.HasValue;
                bool isTargetSegment = !string.IsNullOrEmpty(targetSegment);
                bool isProductType = productType.HasValue;
                bool isDistributionChannel = !string.IsNullOrEmpty(distributionChannel);

                if (isUnderwritingMethod)
                {
                    var underwritingMethods = Util.ToArraySplitTrim(underwritingMethod).ToList();
                    productIds = TreatyPricingPickListDetailService.GetProductIdByProductIdsUnderwritingMethods(productIds, underwritingMethods);
                    quotationNameDropDowns.AddRange(GetQuotationNameDropDowns(productIds));
                }

                if (isQuotationName)
                {
                    productIds = TreatyPricingProductService.GetIdByProductIdsQuotationName(productIds, quotationName);
                }

                if (isProduct)
                {
                    //productIds = TreatyPricingProductService.GetProductIdByProductIdsProductName(productIds, productId);
                    targetSegmentCodes = GetTargetSegmentCodesByProductId(productId.Value);
                    productVersionIds = TreatyPricingProductService.GetVersionIdsByProductId(productId.Value);
                }

                if (isTargetSegment)
                {
                    var targetSegments = Util.ToArraySplitTrim(targetSegment).ToList();
                    productVersionIds = TreatyPricingPickListDetailService.GetProductVersionIdByProductIdPickListIdCodes(productId.Value, PickListBo.TargetSegment, targetSegments);
                    productTypeDropdowns.AddRange(GetProductTypeDropDowns(productIds));
                }

                if (isProductType)
                {
                    productVersionIds = TreatyPricingProductVersionService.GetIdByProductVersionIdsProductType(productVersionIds, productType.Value);
                    distributionChannelCodes = GetDistributionChannelCodesByProductVersionIds(productVersionIds);
                }

                if (isDistributionChannel)
                {
                    var distributionChannels = Util.ToArraySplitTrim(distributionChannel).ToList();
                    productVersionIds = TreatyPricingPickListDetailService.GetProductVersionIdByProductVersionIdsPickListIdCodes(productVersionIds, PickListBo.DistributionChannel, distributionChannels);
                }

                underwritingMethodCodes = GetUnderwritingMethodCodes(productIds);
                productNameDropDowns.AddRange(GetProductDropDownsByProductIds(productIds));

                if (productVersionIds.Count() > 0)
                {
                    foreach (var versionBo in TreatyPricingProductVersionService.GetByVersionIds(productVersionIds))
                    {
                        versionDropDowns.Add(new SelectListItem { Text = string.Format("{0}.0", versionBo.Version), Value = versionBo.Id.ToString() });
                    }
                }
            }

            return Json(new
            {
                underwritingMethodCodes,
                quotationNameDropDowns,
                productNameDropDowns,
                targetSegmentCodes,
                productTypeDropdowns,
                distributionChannelCodes,
                versionDropDowns,
                benefitCodes
            });
        }

        // Get Distinct Quotation Name By Product Ids
        public List<SelectListItem> GetQuotationNameDropDowns(List<int> productIds)
        {
            var quotationNameDropDowns = new List<SelectListItem> { };
            foreach (var quotationName in TreatyPricingProductService.GetDistinctQuotationNameByProductIds(productIds))
            {
                quotationNameDropDowns.Add(new SelectListItem { Text = quotationName, Value = quotationName });
            }
            return quotationNameDropDowns;
        }

        // Get Distinct Product Ids and Name
        public List<SelectListItem> GetProductDropDownsByProductIds(List<int> productIds)
        {
            var productNameDropDowns = new List<SelectListItem> { };
            foreach (var product in TreatyPricingProductService.GetProductsByProductIds(productIds))
            {
                productNameDropDowns.Add(new SelectListItem { Text = product.Code + " - " + product.Name, Value = product.Id.ToString() });
            }
            return productNameDropDowns;
        }

        // Get Distinct Target Segment By Product Ids
        public List<string> GetTargetSegmentCodesByProductId(int productId)
        {
            return TreatyPricingPickListDetailService.GetDistinctCodeByObjectProductVersionProductIdPickList(productId, PickListBo.TargetSegment);
        }

        [HttpPost]
        public JsonResult GetProductComparisonBenefitCodes(int? versionId)
        {
            var benefitCodesBeforeDistinct = new List<string> { };
            var benefitCodes = new List<string> { };

            if (versionId.HasValue)
            {
                foreach (var benefitBo in TreatyPricingProductBenefitService.GetByVersionId(versionId.Value))
                {
                    benefitCodesBeforeDistinct.Add(benefitBo.BenefitCode);
                }
            }

            benefitCodes = benefitCodesBeforeDistinct.Distinct().ToList();

            return Json(new { benefitCodes });
        }

        [HttpPost]
        public JsonResult GenerateProductBenefitComparison(List<int?> versionIds, List<string> benefitCodes)
        {
            List<TreatyPricingProductBo> products = new List<TreatyPricingProductBo>();
            int i = 0;

            foreach (int? versionId in versionIds)
            {
                if (versionId.HasValue && versionId.Value > 0)
                {
                    var productVersionBo = TreatyPricingProductVersionService.FindForProductComparisonReport(versionId.Value);

                    if (productVersionBo != null)
                    {
                        var productBo = TreatyPricingProductService.FindForProductComparisonReport(productVersionBo.TreatyPricingProductId);
                        List<TreatyPricingProductBenefitBo> productBenefitBos = new List<TreatyPricingProductBenefitBo>();

                        string benefitCode = benefitCodes[i];
                        List<string> benefitCodeList = benefitCode.Split(',').ToList();
                        List<int> benefitIdList = new List<int>();

                        foreach (string benefit in benefitCodeList)
                        {
                            int id = 0;
                            var benefitBo = BenefitService.FindByCode(benefit);
                            if (benefitBo != null)
                            {
                                id = benefitBo.Id;
                            }
                            benefitIdList.Add(id);
                        }
                        IList<TreatyPricingProductBenefitBo> comparisonTreatyPricingProductBenefitBos = TreatyPricingProductBenefitService.GetByVersionIdBenefits(productVersionBo.Id, benefitIdList);

                        #region Product information processing
                        //Set Underwriting Method string
                        productBo.UnderwritingMethodStr = String.Join(",", TreatyPricingPickListDetailService.GetByObjectPickList(TreatyPricingCedantBo.ObjectProduct, productBo.Id, PickListBo.UnderwritingMethod)
                            .Select(q => q.PickListDetailCode).ToList());

                        //Set Final Documents
                        var finalDocumentsBo = DocumentService.GetLatestByModuleIdObjectId(
                            ModuleService.FindByController(ModuleBo.ModuleController.TreatyPricingCedant.ToString()).Id,
                            productBo.TreatyPricingCedantId,
                            ModuleBo.ModuleController.TreatyPricingProduct.ToString(),
                            productBo.Id);

                        productBo.FinalDocuments = finalDocumentsBo != null ? finalDocumentsBo.Description : "";

                        //Set Version BO and Version string
                        productBo.ComparisonTreatyPricingProductVersionBo = productVersionBo;

                        //Set Quotation Workflow BO and BD Person In-Charge
                        var workflowObjectBo = TreatyPricingWorkflowObjectService.GetByTypeObjectTypeObjectIdObjectVersionId(
                            TreatyPricingWorkflowObjectBo.TypeQuotation, TreatyPricingWorkflowObjectBo.ObjectTypeProduct, productBo.Id, productVersionBo.Id);

                        if (workflowObjectBo != null)
                        {
                            int? quotationWorkflowId = workflowObjectBo.WorkflowId;

                            var comparisonTreatyPricingQuotationWorkflowVersionBo = TreatyPricingQuotationWorkflowVersionService.GetLatestVersionByTreatyPricingQuotationWorkflowId(quotationWorkflowId.Value);
                            productBo.ComparisonTreatyPricingQuotationWorkflowVersionBo = comparisonTreatyPricingQuotationWorkflowVersionBo;
                            productBo.BDPersonInChargeId = comparisonTreatyPricingQuotationWorkflowVersionBo.BDPersonInChargeId;
                            productBo.BDPersonInChargeName = comparisonTreatyPricingQuotationWorkflowVersionBo.BDPersonInChargeName;
                        }
                        else
                        {
                            productBo.BDPersonInChargeName = "";
                        }

                        //Set Country of Residence
                        if (productVersionBo.TerritoryOfIssueCodePickListDetailId.HasValue)
                        {
                            productBo.ResidenceCountry = PickListDetailService.Find(productVersionBo.TerritoryOfIssueCodePickListDetailId).Description;
                        }
                        //if (!String.IsNullOrEmpty(productBo.PerLifeRetroTreatyCode))
                        //{
                        //    var perLifeRetroTreatyCodes = productBo.PerLifeRetroTreatyCode.Split(',').ToList().Select(s => s.Trim());
                        //    List<string> residenceCountries = new List<string>();

                        //    foreach (string perLifeRetroTreatyCode in perLifeRetroTreatyCodes)
                        //    {
                        //        int perLifeRetroTreatyId = TreatyPricingPerLifeRetroService.FindByCode(perLifeRetroTreatyCode.Trim()).Id;
                        //        string residenceCountry = TreatyPricingPerLifeRetroVersionService.FindLatestByTreatyPricingPerLifeRetroId(perLifeRetroTreatyId).ResidenceCountry;

                        //        if (!String.IsNullOrEmpty(residenceCountry))
                        //            residenceCountries.Add(residenceCountry);
                        //    }

                        //    productBo.ResidenceCountry = string.Join(",", residenceCountries);
                        //}
                        #endregion

                        #region Product Details information processing
                        productBo.ProductType = productBo.ComparisonTreatyPricingProductVersionBo.ProductTypeStr;
                        productBo.BusinessOrigin = productBo.ComparisonTreatyPricingProductVersionBo.BusinessOriginStr;
                        productBo.BusinessType = productBo.ComparisonTreatyPricingProductVersionBo.BusinessTypeStr;
                        productBo.ReinsuranceArrangement = productBo.ComparisonTreatyPricingProductVersionBo.ReinsuranceArrangementStr;
                        productBo.ReinsurancePremiumPayment = productBo.ComparisonTreatyPricingProductVersionBo.ReinsurancePremiumPaymentStr;
                        productBo.UnearnedPremiumRefund = productBo.ComparisonTreatyPricingProductVersionBo.UnearnedPremiumRefundStr;

                        //Repo objects
                        string medicalTableInfo = "";
                        string financialTableInfo = "";
                        string uwQuestionnaireInfo = "";
                        string advantageProgramInfo = "";
                        string profitCommInfo = "";

                        if (productBo.ComparisonTreatyPricingProductVersionBo.TreatyPricingMedicalTableVersionId.HasValue)
                        {
                            var medicalTableVersionBo = TreatyPricingMedicalTableVersionService.Find(productBo.ComparisonTreatyPricingProductVersionBo.TreatyPricingMedicalTableVersionId.Value, true);
                            medicalTableInfo = medicalTableVersionBo.TreatyPricingMedicalTableBo.MedicalTableId;
                            medicalTableInfo += " - V" + medicalTableVersionBo.Version.ToString() + ".0";
                        }

                        if (productBo.ComparisonTreatyPricingProductVersionBo.TreatyPricingFinancialTableVersionId.HasValue)
                        {
                            var financialTableVersionBo = TreatyPricingFinancialTableVersionService.Find(productBo.ComparisonTreatyPricingProductVersionBo.TreatyPricingFinancialTableVersionId.Value, true);
                            financialTableInfo = financialTableVersionBo.TreatyPricingFinancialTableBo.FinancialTableId;
                            financialTableInfo += " - V" + financialTableVersionBo.Version.ToString() + ".0";
                        }

                        if (productBo.ComparisonTreatyPricingProductVersionBo.TreatyPricingUwQuestionnaireVersionId.HasValue)
                        {
                            var uwQuestionnaireVersionBo = TreatyPricingUwQuestionnaireVersionService.Find(productBo.ComparisonTreatyPricingProductVersionBo.TreatyPricingUwQuestionnaireVersionId.Value, true);
                            uwQuestionnaireInfo = uwQuestionnaireVersionBo.TreatyPricingUwQuestionnaireBo.Code;
                            uwQuestionnaireInfo += " - V" + uwQuestionnaireVersionBo.Version.ToString() + ".0";
                        }

                        if (productBo.ComparisonTreatyPricingProductVersionBo.TreatyPricingAdvantageProgramVersionId.HasValue)
                        {
                            var advantageProgramVersionBo = TreatyPricingAdvantageProgramVersionService.Find(productBo.ComparisonTreatyPricingProductVersionBo.TreatyPricingAdvantageProgramVersionId.Value, true);
                            advantageProgramInfo = advantageProgramVersionBo.TreatyPricingAdvantageProgramBo.Code;
                            advantageProgramInfo += " - V" + advantageProgramVersionBo.Version.ToString() + ".0";
                        }

                        if (productBo.ComparisonTreatyPricingProductVersionBo.TreatyPricingProfitCommissionVersionId.HasValue)
                        {
                            var profitCommissionVersionBo = TreatyPricingProfitCommissionVersionService.Find(productBo.ComparisonTreatyPricingProductVersionBo.TreatyPricingProfitCommissionVersionId.Value, true);
                            profitCommInfo = profitCommissionVersionBo.TreatyPricingProfitCommissionBo.Code;
                            profitCommInfo += " - V" + profitCommissionVersionBo.Version.ToString() + ".0";
                        }

                        productBo.MedicalTableInfo = medicalTableInfo;
                        productBo.FinancialTableInfo = financialTableInfo;
                        productBo.UwQuestionnaireInfo = uwQuestionnaireInfo;
                        productBo.AdvantageProgramInfo = advantageProgramInfo;
                        productBo.ProfitCommInfo = profitCommInfo;
                        #endregion

                        #region Benefit information processing
                        foreach (var comparisonTreatyPricingProductBenefitBo in comparisonTreatyPricingProductBenefitBos)
                        {
                            string uwLimitInfo = "";
                            string claimApprovalLimitInfo = "";
                            string definitionExclusionInfo = "";
                            string rateTableInfo = "";

                            if (comparisonTreatyPricingProductBenefitBo.TreatyPricingUwLimitVersionBo != null)
                            {
                                uwLimitInfo = comparisonTreatyPricingProductBenefitBo.TreatyPricingUwLimitBo.LimitId;
                                uwLimitInfo += " - V" + comparisonTreatyPricingProductBenefitBo.TreatyPricingUwLimitVersionBo.Version.ToString() + ".0";
                            }

                            if (comparisonTreatyPricingProductBenefitBo.TreatyPricingClaimApprovalLimitVersionBo != null)
                            {
                                claimApprovalLimitInfo = comparisonTreatyPricingProductBenefitBo.TreatyPricingClaimApprovalLimitBo.Code;
                                claimApprovalLimitInfo += " - V" + comparisonTreatyPricingProductBenefitBo.TreatyPricingClaimApprovalLimitVersionBo.Version.ToString() + ".0";
                            }

                            if (comparisonTreatyPricingProductBenefitBo.TreatyPricingDefinitionAndExclusionVersionBo != null)
                            {
                                definitionExclusionInfo = comparisonTreatyPricingProductBenefitBo.TreatyPricingDefinitionAndExclusionBo.Code;
                                definitionExclusionInfo += " - V" + comparisonTreatyPricingProductBenefitBo.TreatyPricingDefinitionAndExclusionVersionBo.Version.ToString() + ".0";
                            }

                            if (comparisonTreatyPricingProductBenefitBo.TreatyPricingRateTableVersionBo != null)
                            {
                                rateTableInfo = comparisonTreatyPricingProductBenefitBo.TreatyPricingRateTableBo.Code;
                                rateTableInfo += " - V" + comparisonTreatyPricingProductBenefitBo.TreatyPricingRateTableVersionBo.Version.ToString() + ".0";
                            }

                            comparisonTreatyPricingProductBenefitBo.UwLimitInfo = uwLimitInfo;
                            comparisonTreatyPricingProductBenefitBo.ClaimApprovalLimitInfo = claimApprovalLimitInfo;
                            comparisonTreatyPricingProductBenefitBo.DefinitionExclusionInfo = definitionExclusionInfo;
                            comparisonTreatyPricingProductBenefitBo.RateTableInfo = rateTableInfo;

                            //Direct retro processing
                            TreatyPricingProductBenefitDirectRetroBo directRetroBo = TreatyPricingProductBenefitDirectRetroService.GetLatestByBenefitId(comparisonTreatyPricingProductBenefitBo.Id);

                            if (directRetroBo == null)
                            {
                                directRetroBo = new TreatyPricingProductBenefitDirectRetroBo
                                {
                                    RetroPartyCode = "",
                                    ArrangementRetrocessionType = "",
                                    MlreRetention = "",
                                    RetrocessionShare = "",
                                    IsRetrocessionProfitCommissionStr = "",
                                    IsRetrocessionAdvantageProgramStr = "",
                                    RetrocessionRateTable = "",
                                    NewBusinessRateGuarantee = "",
                                    RenewalBusinessRateGuarantee = "",
                                    RetrocessionDiscount = "",
                                    AdditionalDiscount = "",
                                    AdditionalLoading = "",
                                };
                            }

                            comparisonTreatyPricingProductBenefitBo.DirectRetroBo = directRetroBo;
                        }

                        productBo.ComparisonTreatyPricingProductBenefitBos = comparisonTreatyPricingProductBenefitBos;
                        #endregion

                        products.Add(productBo);
                    }
                }

                i++;
            }

            return Json(new { products });
        }
        #endregion

        #region Group report
        [HttpPost]
        public JsonResult GroupOverAllTatReport(string reqRecYear)
        {
            if (int.Parse(reqRecYear) <= 1900 && int.Parse(reqRecYear) >= 2100)
            {
                return Json(new { error = "Please select a valid year" });
            }

            var versionBos = TreatyPricingGroupReferralVersionService.GetGroupOverallTatCount(int.Parse(reqRecYear));

            var cedantCaseCount = new List<int>();
            foreach (var verBo in versionBos.Where(a => a.QuotationSentDate.HasValue && a.RequestReceivedDate.HasValue))
            {
                var caseCount = (verBo.QuotationSentDate.Value - verBo.RequestReceivedDate.Value).Days - CalculateDaysWithHolidays(verBo.RequestReceivedDate.Value, verBo.QuotationSentDate.Value);
                cedantCaseCount.Add(caseCount);
            }

            var internalCaseCount = new List<int>();
            foreach (var verBo in versionBos.Where(a => a.QuotationSentDate.HasValue && a.ClientReplyDate.HasValue && a.EnquiryToClientDate.HasValue))
            {
                var caseCount = (verBo.QuotationSentDate.Value - verBo.ClientReplyDate.Value).Days - CalculateDaysWithHolidays(verBo.ClientReplyDate.Value, verBo.QuotationSentDate.Value) + (verBo.EnquiryToClientDate.Value - verBo.RequestReceivedDate.Value).Days - CalculateDaysWithHolidays(verBo.RequestReceivedDate.Value, verBo.EnquiryToClientDate.Value);
                internalCaseCount.Add(caseCount);
            }

            var bo = new TreatyPricingGroupReferralBo();

            bo.NoOfDays0Cedant = cedantCaseCount.Where(q => q == 0).Count();
            bo.NoOfDays1Cedant = cedantCaseCount.Where(q => q == 1).Count();
            bo.NoOfDays2Cedant = cedantCaseCount.Where(q => q == 2).Count();
            bo.NoOfDays3Cedant = cedantCaseCount.Where(q => q == 3).Count();
            bo.NoOfDays4Cedant = cedantCaseCount.Where(q => q != 0 && q != 1 && q != 2 && q != 3).Count();

            bo.NoOfDays0Internal = internalCaseCount.Where(q => q == 0).Count();
            bo.NoOfDays1Internal = internalCaseCount.Where(q => q == 1).Count();
            bo.NoOfDays2Internal = internalCaseCount.Where(q => q == 2).Count();
            bo.NoOfDays3Internal = internalCaseCount.Where(q => q == 3).Count();
            bo.NoOfDays4Internal = internalCaseCount.Where(q => q != 0 && q != 1 && q != 2 && q != 3).Count();

            return Json(new { bo });
        }

        #region used for GTL Rates by Unit Rate, GTL Rates by Age Banded, GTL Basis of SA, Product & Benefit Details, HIPS
        [HttpPost]
        public JsonResult UpdateGroupReferralData(
            int? treatyPricingCedantId,
            int? insuredGroupNameId,
            string coverageStartDate,
            string description,
            int? version,
            bool hips = false)
        {
            var insuredGroupNameDropDowns = GetEmptyDropDownList();
            var coverageStartDateDropDowns = GetEmptyDropDownList();
            var descriptionDropDowns = GetEmptyDropDownList();
            var groupReferralVersions = GetEmptyDropDownList();
            var hipsCategories = new List<string>();
            var groupReferralVersionId = 0;
            var groupReferralId = 0;

            if (treatyPricingCedantId.HasValue)
            {
                var distinctInsuredGroupNames = TreatyPricingGroupReferralService.GetByCedantIdForReport(treatyPricingCedantId.Value).Select(q => q.InsuredGroupNameId).Distinct().ToList();

                if (hips)
                    hipsCategories = HipsCategoryService.Get().Select(q => q.Name).ToList();

                foreach (var insGroupNameId in distinctInsuredGroupNames)
                {
                    insuredGroupNameDropDowns.Add(new SelectListItem { Text = InsuredGroupNameService.Find(insGroupNameId).Name, Value = insGroupNameId.ToString() });
                }

                if (insuredGroupNameId.HasValue)
                {
                    var distinctCoverageStartDate = TreatyPricingGroupReferralService.GetByCedantIdForReport(treatyPricingCedantId.Value)
                        .Where(q => q.InsuredGroupNameId == insuredGroupNameId)
                        .Select(q => q.CoverageStartDateStr).Distinct().ToList();

                    foreach (var distinctCSDate in distinctCoverageStartDate)
                    {
                        coverageStartDateDropDowns.Add(new SelectListItem { Text = distinctCSDate, Value = distinctCSDate });
                    }

                    if (!string.IsNullOrEmpty(coverageStartDate))
                    {
                        var covStartDate = DateTime.Parse(coverageStartDate).Date;

                        var descriptions = TreatyPricingGroupReferralService.GetByCedantIdForReport(treatyPricingCedantId.Value)
                            .Where(q => q.InsuredGroupNameId == insuredGroupNameId.Value)
                            .Where(q => q.CoverageStartDate == covStartDate)
                            .Select(q => q.Description)
                            .ToList();

                        foreach (var groupDescription in descriptions)
                        {
                            descriptionDropDowns.Add(new SelectListItem { Text = groupDescription, Value = groupDescription });
                        }

                        if (!string.IsNullOrEmpty(description))
                        {
                            var groupReferral = TreatyPricingGroupReferralService.GetByCedantIdForReport(treatyPricingCedantId.Value)
                            .Where(q => q.InsuredGroupNameId == insuredGroupNameId.Value)
                            .Where(q => q.CoverageStartDate == covStartDate)
                            .Where(q => q.Description == description)
                            .FirstOrDefault();

                            var verGroupReferrals = TreatyPricingGroupReferralVersionService.GetByTreatyPricingGroupReferralIdForReport(groupReferral.Id).Select(q => q.Version).ToList();

                            foreach (var ver in verGroupReferrals)
                            {
                                groupReferralVersions.Add(new SelectListItem { Text = string.Format("{0}.0", ver), Value = ver.ToString() });
                            }

                            if (version.HasValue)
                            {
                                groupReferralVersionId = TreatyPricingGroupReferralVersionService.FindByIdAndVersion(groupReferral.Id, version.Value).Id;
                                groupReferralId = groupReferral.Id;
                            }

                        }
                    }
                }
            }
            return Json(new { insuredGroupNameDropDowns, coverageStartDateDropDowns, descriptionDropDowns, groupReferralVersions, groupReferralVersionId, groupReferralId, hipsCategories });
        }

        #endregion

        #region HIPS comparison
        [HttpPost]
        public JsonResult GenerateHipsComparisonReport(List<int> groupReferralVersionIds, List<string> hipsCategories)
        {
            List<TreatyPricingGroupReferralBo> groupReferralBos = new List<TreatyPricingGroupReferralBo>();
            List<TreatyPricingGroupReferralHipsTableBo> groupReferralHipsTableBos = new List<TreatyPricingGroupReferralHipsTableBo>();
            var distinctHipsSubCategory = TreatyPricingGroupReferralHipsTableService.GetDistinctSubCategory();

            var groupReferralHipsCategories = new List<HipsCategoryDetailBo>();


            foreach (var (groupReferralVersionId, index) in groupReferralVersionIds.Select((v, i) => (v, i)))
            {
                if (groupReferralVersionId != 0)
                {
                    var groupReferral = TreatyPricingGroupReferralVersionService.FindForHipsComparison(groupReferralVersionId);
                    var hipsCategory = hipsCategories[index].Split(',');
                    var hipsCategoryIds = new List<int>();

                    foreach (var i in hipsCategory)
                    {
                        if (!string.IsNullOrEmpty(i))
                            hipsCategoryIds.Add(HipsCategoryService.FindByName(i.Trim()).Id);
                    }

                    var hipsTableIds = TreatyPricingGroupReferralHipsTableService.GetHipsSubCategoryIdByTreatyPricingGroupReferralIdForHipsReport(groupReferral.TreatyPricingGroupReferralId, hipsCategoryIds);

                    if (groupReferral.TreatyPricingGroupReferralBo != null)
                    {
                        groupReferralBos.Add(groupReferral.TreatyPricingGroupReferralBo);


                        foreach (var hipsTableId in hipsTableIds)
                        {
                            var hipsTable = TreatyPricingGroupReferralHipsTableService.GetByTreatyPricingGroupReferralIdHipsSubCategory(groupReferral.TreatyPricingGroupReferralId, hipsTableId);
                            var hipsTableBo = HipsCategoryDetailService.Find(hipsTable.HipsSubCategoryId);
                            if (hipsTableBo != null)
                            {
                                foreach (var distinctHips in distinctHipsSubCategory)
                                {
                                    if (hipsTable.HipsSubCategoryId == distinctHips)
                                    {
                                        var grHipsTableBo = new TreatyPricingGroupReferralHipsTableBo();
                                        var hipsCategoryDetailBo = new HipsCategoryDetailBo();
                                        groupReferralHipsTableBos.Add(hipsTable);
                                        groupReferralHipsCategories.Add(hipsTableBo);
                                    }
                                    else if (hipsTable.HipsSubCategoryId != distinctHips && hipsTableIds.Where(q => q == distinctHips).ToList().Count() == 0)
                                    {
                                        var emptyBo = new TreatyPricingGroupReferralHipsTableBo();
                                        emptyBo.HipsSubCategoryId = distinctHips;
                                        if (groupReferralHipsTableBos.Count > 0 && groupReferralHipsTableBos.Where(q => q.HipsSubCategoryId == emptyBo.HipsSubCategoryId && q.PlanA == null).Count() == 0)
                                            groupReferralHipsTableBos.Add(emptyBo);
                                    }
                                }
                            }
                        }

                        if (hipsTableIds.Count() == 0)
                        {
                            foreach (var i in distinctHipsSubCategory)
                            {
                                var emptyBo = new TreatyPricingGroupReferralHipsTableBo();
                                emptyBo.HipsSubCategoryId = i;
                                groupReferralHipsTableBos.Add(emptyBo);
                            }
                        }
                    }
                    else
                    {
                        groupReferralBos.Add(new TreatyPricingGroupReferralBo());
                        foreach (var i in distinctHipsSubCategory)
                        {
                            var emptyBo = new TreatyPricingGroupReferralHipsTableBo();
                            emptyBo.HipsSubCategoryId = i;
                            groupReferralHipsTableBos.Add(emptyBo);
                        }
                    }
                }
                else
                {
                    groupReferralBos.Add(new TreatyPricingGroupReferralBo());
                    foreach (var i in distinctHipsSubCategory)
                    {
                        var emptyBo = new TreatyPricingGroupReferralHipsTableBo();
                        emptyBo.HipsSubCategoryId = i;
                        groupReferralHipsTableBos.Add(emptyBo);
                    }
                }
            }

            var grHipsCategoriesList = new List<HipsCategoryDetailBo>();

            foreach (var i in groupReferralHipsCategories.Select(q => q.Id).Distinct())
            {
                grHipsCategoriesList.Add(HipsCategoryDetailService.Find(i));
            }

            return Json(new { groupReferrals = groupReferralBos, groupReferralHipsTables = groupReferralHipsTableBos, hipsCategories = grHipsCategoriesList.OrderBy(q => q.Subcategory).ToList() });
        }

        #endregion

        #region Product and Benefit Details Report
        [HttpPost]
        public JsonResult GenerateProductAndBenefitDetails(List<int> groupReferralVersionIds)
        {
            List<TreatyPricingGroupReferralVersionBo> treatyPricingGroupReferralVersionBos = new List<TreatyPricingGroupReferralVersionBo> { };
            List<TreatyPricingGroupReferralVersionBenefitBo> treatyPricingGroupReferralVersionBenefitBos = new List<TreatyPricingGroupReferralVersionBenefitBo> { };
            var distinctBenefitIds = TreatyPricingGroupReferralVersionBenefitService.GetDistinctBenefits().OrderBy(q => q);

            var groupReferralVersionBenefitNames = new List<string>();

            foreach (var (groupReferralVersionId, index) in groupReferralVersionIds.Select((v, i) => (v, i)))
            {
                TreatyPricingGroupReferralVersionBo bo = null;
                if (groupReferralVersionId != 0)
                {
                    bo = TreatyPricingGroupReferralVersionService.FindForProductAndBenefitDetailComparison(groupReferralVersionId);
                    if (bo != null)
                    {
                        bo.TreatyPricingGroupReferralBo = TreatyPricingGroupReferralService.FindForProductAndBenefitDetailsComparison(bo.TreatyPricingGroupReferralId);
                        bo.VersionStr = bo.Version.ToString() + ".0";
                        bo.CompulsoryOrVoluntary = TreatyPricingGroupReferralVersionBo.GetCompulsoryOrVoluntaryName(bo.IsCompulsoryOrVoluntary);

                        //bo.TreatyPricingGroupReferralVersionBenefitBos = TreatyPricingGroupReferralVersionBenefitService.GetByTreatyPricingGroupReferralVersionId(groupReferralVersionId);

                        if (bo.TreatyPricingGroupReferralBo.ReferredTypePickListDetailId.HasValue)
                        {
                            bo.ReferredTypePickListDetailBo = PickListDetailService.Find(bo.TreatyPricingGroupReferralBo.ReferredTypePickListDetailId.Value);
                        }

                        if (bo.RequestTypePickListDetailId.HasValue)
                        {
                            bo.RequestTypePickListDetailBo = PickListDetailService.Find(bo.RequestTypePickListDetailId.Value);
                        }

                        if (bo.PremiumTypePickListDetailId.HasValue)
                        {
                            bo.PremiumTypePickListDetailBo = PickListDetailService.Find(bo.PremiumTypePickListDetailId.Value);
                        }


                        var groupReferralVersionBenefits = TreatyPricingGroupReferralVersionBenefitService.GetByTreatyPricingGroupReferralVersionIdForProductAndBenefitDetailsComparison(groupReferralVersionId).OrderBy(a => a.BenefitId);
                        //var versionBenefit = TreatyPricingGroupReferralVersionBenefitService.GetByTreatyPricingGroupReferralVersionId(groupReferralVersionId);

                        foreach (var groupReferralVersionBenefit in groupReferralVersionBenefits)
                        {
                            groupReferralVersionBenefitNames.Add(!string.IsNullOrEmpty(groupReferralVersionBenefit.BenefitBo.Description) ? groupReferralVersionBenefit.BenefitBo.Description : "");

                            foreach (var distinctBen in distinctBenefitIds)
                            {
                                if (groupReferralVersionBenefit.BenefitId == distinctBen)
                                {
                                    if (groupReferralVersionBenefit.TreatyPricingUwLimitId.HasValue)
                                    {
                                        var uwLimit = TreatyPricingUwLimitVersionService.FindForProductAndBenefitDetailComparison(groupReferralVersionBenefit.TreatyPricingUwLimitVersionId);

                                        groupReferralVersionBenefit.AutoBindingLimit = uwLimit.AblSumAssured;
                                    }

                                    if (groupReferralVersionBenefit.ReinsuranceArrangementPickListDetailId.HasValue)
                                    {
                                        groupReferralVersionBenefit.ReinsuranceArrangementPickListDetailBo = PickListDetailService.Find(groupReferralVersionBenefit.ReinsuranceArrangementPickListDetailId.Value);
                                    }

                                    if (groupReferralVersionBenefit.AgeBasisPickListDetailId.HasValue)
                                    {
                                        groupReferralVersionBenefit.AgeBasisPickListDetailBo = PickListDetailService.Find(groupReferralVersionBenefit.AgeBasisPickListDetailId);
                                    }

                                    if (!string.IsNullOrEmpty(groupReferralVersionBenefit.IsOverwriteGroupProfitCommissionStr))
                                    {
                                        if (groupReferralVersionBenefit.IsOverwriteGroupProfitCommissionStr.ToLower() == "true")
                                            groupReferralVersionBenefit.IsOverwriteGroupProfitCommissionStr = "Yes";
                                        else
                                            groupReferralVersionBenefit.IsOverwriteGroupProfitCommissionStr = "No";
                                    }

                                    if (groupReferralVersionBenefit.HasGroupProfitCommission == true)
                                        groupReferralVersionBenefit.HasGroupProfitCommissionStr = "Yes";
                                    else
                                        groupReferralVersionBenefit.HasGroupProfitCommissionStr = "No";

                                    if (groupReferralVersionBenefit.BenefitBo.Code == "CCA" || groupReferralVersionBenefit.BenefitBo.Code == "CCS")
                                    {
                                        groupReferralVersionBenefit.RequestedFreeCoverLimitNonCI = groupReferralVersionBenefit.RequestedFreeCoverLimitCI;
                                        groupReferralVersionBenefit.GroupFreeCoverLimitAgeNonCI = groupReferralVersionBenefit.GroupFreeCoverLimitAgeCI;
                                    }


                                    treatyPricingGroupReferralVersionBenefitBos.Add(groupReferralVersionBenefit);
                                }
                                else if (groupReferralVersionBenefit.BenefitId != distinctBen && groupReferralVersionBenefits.Where(q => q.BenefitId == distinctBen).ToList().Count() == 0)
                                {
                                    var benefitBo = BenefitService.Find(distinctBen);
                                    var emptyBenefits = new TreatyPricingGroupReferralVersionBenefitBo();
                                    var emptyBenefitBo = new BenefitBo();
                                    emptyBenefitBo.Description = benefitBo.Description;
                                    emptyBenefits.BenefitBo = emptyBenefitBo;
                                    emptyBenefits.IsOverwriteGroupProfitCommissionStr = null;
                                    if (treatyPricingGroupReferralVersionBenefitBos.Count() > 0 && treatyPricingGroupReferralVersionBenefitBos.Where(q => q.BenefitBo.Description == emptyBenefitBo.Description && q.ReinsuranceArrangementPickListDetailId == null && q.OtherSpecialReinsuranceArrangement == null && q.ProfitMargin == null && q.ExpenseMargin == null && q.CommissionMargin == null).Count() == 0)
                                    {
                                        treatyPricingGroupReferralVersionBenefitBos.Add(emptyBenefits);
                                    }
                                }
                            }
                        }

                        if (groupReferralVersionBenefits.Count() == 0)
                        {

                            foreach (var i in groupReferralVersionBenefitNames.Distinct().ToList())
                            {
                                var emptyBenefits = new TreatyPricingGroupReferralVersionBenefitBo();
                                emptyBenefits.BenefitBo.Description = i;
                                treatyPricingGroupReferralVersionBenefitBos.Add(emptyBenefits);
                            }
                        }
                    }
                    else
                    {
                        bo = new TreatyPricingGroupReferralVersionBo();
                        treatyPricingGroupReferralVersionBenefitBos.Add(new TreatyPricingGroupReferralVersionBenefitBo());
                    }
                    treatyPricingGroupReferralVersionBos.Add(bo);
                }
                else
                {
                    bo = new TreatyPricingGroupReferralVersionBo();
                    treatyPricingGroupReferralVersionBos.Add(bo);

                    foreach (var i in groupReferralVersionBenefitNames.Distinct().ToList())
                    {
                        var emptyBenefits = new TreatyPricingGroupReferralVersionBenefitBo();
                        var emptyBenefitBo = new BenefitBo();
                        emptyBenefitBo.Description = i;
                        emptyBenefits.BenefitBo = emptyBenefitBo;
                        emptyBenefits.IsOverwriteGroupProfitCommissionStr = null;
                        treatyPricingGroupReferralVersionBenefitBos.Add(emptyBenefits);
                    }
                }
            }
            return Json(new { groupReferralVersions = treatyPricingGroupReferralVersionBos, groupReferralVersionBenefits = treatyPricingGroupReferralVersionBenefitBos, benefitNames = groupReferralVersionBenefitNames.Distinct().ToList() });
        }
        #endregion

        #region GTL Rates by Unit Rate
        [HttpPost]
        public JsonResult GenerateGtlRatesbyUnitRate(List<int> groupReferralVersionIds)
        {
            List<TreatyPricingGroupReferralGtlTableBo> treatyPricingGroupReferralGtlTableBos = new List<TreatyPricingGroupReferralGtlTableBo>();
            List<TreatyPricingGroupReferralBo> groupReferralBo = new List<TreatyPricingGroupReferralBo>();
            List<TreatyPricingGroupReferralVersionBenefitBo> treatyPricingGroupReferralVersionBenefitBos = new List<TreatyPricingGroupReferralVersionBenefitBo>();
            var gtlBenefitNames = new List<string>();

            var distinctBenefitCodes = TreatyPricingGroupReferralGtlTableService.GetDistinctBenefitCodesWithType(TreatyPricingGroupReferralGtlTableBo.TypeGtlUnitRates);

            foreach (var (groupReferralVersionId, index) in groupReferralVersionIds.Select((v, i) => (v, i)))
            {
                if (groupReferralVersionId != 0)
                {
                    TreatyPricingGroupReferralVersionBo bo = null;
                    bo = TreatyPricingGroupReferralVersionService.Find(groupReferralVersionId);

                    if (bo != null)
                    {
                        //bo.TreatyPricingGroupReferralBo = TreatyPricingGroupReferralService.Find(bo.TreatyPricingGroupReferralId, false);
                        var grBo = TreatyPricingGroupReferralService.FindForReport(bo.TreatyPricingGroupReferralId);
                        groupReferralBo.Add(grBo);
                        if (grBo != null)
                        {
                            var gtlTables = TreatyPricingGroupReferralGtlTableService.GetByTreatyPricingGroupReferralIdTypeForReport(grBo.Id, TreatyPricingGroupReferralGtlTableBo.TypeGtlUnitRates);

                            if (gtlTables.Count() != 0)
                            {
                                foreach (var gtlTable in gtlTables)
                                {
                                    foreach (var distinctBen in distinctBenefitCodes)
                                    {
                                        if (gtlTable != null && gtlTable.BenefitCode == distinctBen)
                                        {
                                            //gtlTable.TreatyPricingGroupReferralBo = TreatyPricingGroupReferralService.Find(gtlTable.TreatyPricingGroupReferralId);
                                            treatyPricingGroupReferralGtlTableBos.Add(gtlTable);
                                            gtlBenefitNames.Add(gtlTable.BenefitCode);
                                        }
                                        else if (gtlTable.BenefitCode != distinctBen && gtlTables.Where(q => q.BenefitCode == distinctBen).ToList().Count() == 0)
                                        {
                                            var emptyBo = new TreatyPricingGroupReferralGtlTableBo
                                            {
                                                BenefitCode = distinctBen
                                            };
                                            if (treatyPricingGroupReferralGtlTableBos.Count > 0 && treatyPricingGroupReferralGtlTableBos.Where(q => q.BenefitCode == emptyBo.BenefitCode && q.GrossRate == null && q.RiskRate == null).Count() == 0)
                                                treatyPricingGroupReferralGtlTableBos.Add(emptyBo);
                                        }
                                    }
                                }
                            }
                            else
                            {

                                foreach (var i in gtlBenefitNames.Distinct().ToList())
                                {
                                    var emptyBenefits = new TreatyPricingGroupReferralVersionBenefitBo();
                                    emptyBenefits.BenefitBo.Description = i;
                                    treatyPricingGroupReferralVersionBenefitBos.Add(emptyBenefits);
                                }
                            }
                        }
                        else
                        {
                            groupReferralBo.Add(new TreatyPricingGroupReferralBo());
                            foreach (var distinctBen in distinctBenefitCodes)
                            {
                                var emptyBo = new TreatyPricingGroupReferralGtlTableBo
                                {
                                    BenefitCode = distinctBen
                                };
                                treatyPricingGroupReferralGtlTableBos.Add(emptyBo);
                            }
                        }
                    }
                }
                else
                {
                    groupReferralBo.Add(new TreatyPricingGroupReferralBo());
                    foreach (var i in gtlBenefitNames.Distinct())
                    {
                        var emptyGtl = new TreatyPricingGroupReferralGtlTableBo();
                        emptyGtl.BenefitCode = i;
                        treatyPricingGroupReferralGtlTableBos.Add(emptyGtl);
                    }
                }
            }

            return Json(new { groupReferral = groupReferralBo, groupReferralGtls = treatyPricingGroupReferralGtlTableBos, benefitNames = gtlBenefitNames.Distinct().ToList() });
        }
        #endregion

        #region GTL Rates by Age Banded
        [HttpPost]
        public JsonResult GenerateGtlRatesByAgeBanded(List<int> groupReferralVersionIds)
        {
            List<TreatyPricingGroupReferralBo> groupReferralBo = new List<TreatyPricingGroupReferralBo>();
            List<TreatyPricingGroupReferralGtlTableBo> treatyPricingGroupReferralGtlTableBos = new List<TreatyPricingGroupReferralGtlTableBo>();
            List<TreatyPricingGroupReferralVersionBenefitBo> treatyPricingGroupReferralVersionBenefitBos = new List<TreatyPricingGroupReferralVersionBenefitBo>();
            var gtlBenefitNames = new List<string>();
            var gtlAgeBands = new List<string>();

            var distinctBenefitCodes = TreatyPricingGroupReferralGtlTableService.GetDistinctBenefitCodesWithType(TreatyPricingGroupReferralGtlTableBo.TypeGtlAgeBanded);
            var distinctAgeBandRanges = TreatyPricingGroupReferralGtlTableService.GetDistinctAgeBandRange();

            foreach (var (groupReferralVersionId, index) in groupReferralVersionIds.Select((v, i) => (v, i)))
            {
                if (groupReferralVersionId != 0)
                {
                    TreatyPricingGroupReferralVersionBo bo = null;
                    bo = TreatyPricingGroupReferralVersionService.Find(groupReferralVersionId, true);
                    if (bo != null)
                    {
                        //bo.TreatyPricingGroupReferralBo = TreatyPricingGroupReferralService.Find(bo.TreatyPricingGroupReferralId, false);
                        var grBo = TreatyPricingGroupReferralService.Find(bo.TreatyPricingGroupReferralId, false);
                        grBo.CedantBo = CedantService.Find(grBo.CedantId);
                        groupReferralBo.Add(grBo);

                        if (grBo != null)
                        {
                            var gtlTables = TreatyPricingGroupReferralGtlTableService.GetByTreatyPricingGroupReferralIdType(grBo.Id, TreatyPricingGroupReferralGtlTableBo.TypeGtlAgeBanded).Where(q => !string.IsNullOrEmpty(q.BenefitCode) && !string.IsNullOrEmpty(q.AgeBandRange)).OrderBy(q => q.AgeBandRange).Distinct();

                            foreach (var gtlTable in gtlTables)
                            {
                                foreach (var distinctBen in distinctBenefitCodes)
                                {
                                    foreach (var distinctAgeBandRange in distinctAgeBandRanges)
                                    {
                                        if (gtlTable != null && (!string.IsNullOrEmpty(gtlTable.AgeBandRange) && gtlTable.AgeBandRange == distinctAgeBandRange) && (!string.IsNullOrEmpty(gtlTable.BenefitCode) && gtlTable.BenefitCode == distinctBen))
                                        {
                                            //gtlTable.TreatyPricingGroupReferralBo = TreatyPricingGroupReferralService.Find(gtlTable.TreatyPricingGroupReferralId);
                                            var gtlBo = new TreatyPricingGroupReferralGtlTableBo
                                            {
                                                AgeBandRange = distinctAgeBandRange,
                                                BenefitCode = distinctBen,
                                                RiskRate = gtlTable.RiskRate,
                                                GrossRate = gtlTable.GrossRate
                                            };
                                            treatyPricingGroupReferralGtlTableBos.Add(gtlBo);
                                            gtlBenefitNames.Add(gtlTable.BenefitCode);
                                            gtlAgeBands.Add(gtlTable.AgeBandRange);
                                        }
                                        else if ((gtlTable.BenefitCode != distinctBen && gtlTables.Where(q => q.BenefitCode == distinctBen).ToList().Count() == 0) || (gtlTable.AgeBandRange != distinctAgeBandRange && gtlTables.Where(q => q.AgeBandRange == distinctAgeBandRange).ToList().Count() == 0))
                                        {
                                            var emptyBo = new TreatyPricingGroupReferralGtlTableBo
                                            {
                                                BenefitCode = distinctBen,
                                                AgeBandRange = distinctAgeBandRange
                                            };
                                            if (treatyPricingGroupReferralGtlTableBos.Count > 0 && treatyPricingGroupReferralGtlTableBos.Where(q => q.BenefitCode == emptyBo.BenefitCode && q.AgeBandRange == emptyBo.AgeBandRange && q.GrossRate == null && q.RiskRate == null).Count() == 0)
                                            {
                                                treatyPricingGroupReferralGtlTableBos.Add(emptyBo);
                                            }
                                        }
                                    }
                                }
                            }

                            if (gtlTables.Count() == 0)
                            {
                                foreach (var i in gtlBenefitNames.Distinct().ToList())
                                {
                                    foreach (var u in gtlAgeBands.Distinct().ToList())
                                    {
                                        var emptyBo = new TreatyPricingGroupReferralGtlTableBo();
                                        emptyBo.AgeBandRange = u;
                                        emptyBo.BenefitCode = i;
                                        treatyPricingGroupReferralGtlTableBos.Add(emptyBo);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        groupReferralBo.Add(new TreatyPricingGroupReferralBo());
                        foreach (var i in gtlBenefitNames.Distinct().ToList())
                        {
                            foreach (var u in gtlAgeBands.Distinct().ToList())
                            {
                                var emptyBo = new TreatyPricingGroupReferralGtlTableBo();
                                emptyBo.AgeBandRange = u;
                                emptyBo.BenefitCode = i;
                                treatyPricingGroupReferralGtlTableBos.Add(emptyBo);
                            }

                        }
                    }
                }
                else
                {
                    groupReferralBo.Add(new TreatyPricingGroupReferralBo());
                    foreach (var i in gtlBenefitNames.Distinct().ToList())
                    {
                        foreach (var u in gtlAgeBands.Distinct().ToList())
                        {
                            var emptyBo = new TreatyPricingGroupReferralGtlTableBo();
                            emptyBo.AgeBandRange = u;
                            emptyBo.BenefitCode = i;
                            treatyPricingGroupReferralGtlTableBos.Add(emptyBo);
                        }

                    }
                }

            }

            return Json(new { groupReferral = groupReferralBo, groupReferralGtls = treatyPricingGroupReferralGtlTableBos, benefitNames = gtlBenefitNames.Distinct().ToList(), gtlAgeBandRange = gtlAgeBands.Distinct().ToList() });
        }


        #endregion

        #region GTL Basis of SA
        [HttpPost]
        public JsonResult GenerateGtlBasisOfSa(List<int> groupReferralVersionIds)
        {
            List<TreatyPricingGroupReferralBo> groupReferralBo = new List<TreatyPricingGroupReferralBo>();
            List<TreatyPricingGroupReferralGtlTableBo> treatyPricingGroupReferralGtlTableBos = new List<TreatyPricingGroupReferralGtlTableBo>();
            List<TreatyPricingGroupReferralVersionBenefitBo> treatyPricingGroupReferralVersionBenefitBos = new List<TreatyPricingGroupReferralVersionBenefitBo>();
            var gtlBenefitNames = new List<string>();
            var gtlDesignations = new List<string>();

            var distinctBenefitCodes = TreatyPricingGroupReferralGtlTableService.GetDistinctBenefitCodesWithType(TreatyPricingGroupReferralGtlTableBo.TypeGtlBasisOfSa);
            var distinctDesignations = TreatyPricingGroupReferralGtlTableService.GetDistinctDesignation();

            foreach (var (groupReferralVersionId, index) in groupReferralVersionIds.Select((v, i) => (v, i)))
            {
                if (groupReferralVersionId != 0)
                {
                    TreatyPricingGroupReferralVersionBo bo = null;
                    bo = TreatyPricingGroupReferralVersionService.Find(groupReferralVersionId, false);
                    if (bo != null)
                    {
                        //bo.TreatyPricingGroupReferralBo = TreatyPricingGroupReferralService.Find(bo.TreatyPricingGroupReferralId, false);
                        var grBo = TreatyPricingGroupReferralService.Find(bo.TreatyPricingGroupReferralId, false);
                        grBo.CedantBo = CedantService.Find(grBo.CedantId);
                        groupReferralBo.Add(grBo);

                        if (grBo != null)
                        {
                            var gtlTables = TreatyPricingGroupReferralGtlTableService.GetByTreatyPricingGroupReferralIdType(grBo.Id, TreatyPricingGroupReferralGtlTableBo.TypeGtlBasisOfSa).Where(q => !string.IsNullOrEmpty(q.BenefitCode) && !string.IsNullOrEmpty(q.Designation)).OrderBy(q => q.Designation).Distinct();

                            foreach (var gtlTable in gtlTables)
                            {
                                foreach (var distinctBen in distinctBenefitCodes)
                                {
                                    foreach (var distinctDesignation in distinctDesignations)
                                    {
                                        if (gtlTable != null && (!string.IsNullOrEmpty(gtlTable.Designation) && gtlTable.Designation == distinctDesignation) && (!string.IsNullOrEmpty(gtlTable.BenefitCode) && gtlTable.BenefitCode == distinctBen))
                                        {
                                            //treatyPricingGroupReferralGtlTableBos.Add(gtlTable);
                                            var gtlBo = new TreatyPricingGroupReferralGtlTableBo
                                            {
                                                Designation = distinctDesignation,
                                                BenefitCode = distinctBen,
                                                BasisOfSA = gtlTable.BasisOfSA
                                            };
                                            treatyPricingGroupReferralGtlTableBos.Add(gtlBo);
                                            gtlBenefitNames.Add(gtlTable.BenefitCode);
                                            gtlDesignations.Add(gtlTable.Designation);
                                        }
                                        else if ((gtlTable.BenefitCode != distinctBen && gtlTables.Where(q => q.BenefitCode == distinctBen).ToList().Count() == 0) || (gtlTable.Designation != distinctDesignation && gtlTables.Where(q => q.Designation == distinctDesignation).ToList().Count() == 0))
                                        {
                                            var emptyBo = new TreatyPricingGroupReferralGtlTableBo
                                            {
                                                BenefitCode = distinctBen,
                                                Designation = distinctDesignation
                                            };
                                            if (treatyPricingGroupReferralGtlTableBos.Count > 0 && treatyPricingGroupReferralGtlTableBos.Where(q => q.BenefitCode == emptyBo.BenefitCode && q.Designation == emptyBo.Designation && q.BasisOfSA == null).Count() == 0)
                                            {
                                                treatyPricingGroupReferralGtlTableBos.Add(emptyBo);
                                            }
                                        }
                                    }
                                }
                            }
                            if (gtlTables.Count() == 0)
                            {
                                foreach (var i in gtlBenefitNames.Distinct().ToList())
                                {
                                    foreach (var u in gtlDesignations.Distinct().ToList())
                                    {
                                        var emptyBo = new TreatyPricingGroupReferralGtlTableBo();
                                        emptyBo.Designation = u;
                                        emptyBo.BenefitCode = i;
                                        treatyPricingGroupReferralGtlTableBos.Add(emptyBo);
                                    }

                                }
                            }
                        }
                        else
                        {
                            groupReferralBo.Add(new TreatyPricingGroupReferralBo());
                            foreach (var i in gtlBenefitNames.Distinct().ToList())
                            {
                                foreach (var u in gtlDesignations.Distinct().ToList())
                                {
                                    var emptyBo = new TreatyPricingGroupReferralGtlTableBo();
                                    emptyBo.Designation = u;
                                    emptyBo.BenefitCode = i;
                                    treatyPricingGroupReferralGtlTableBos.Add(emptyBo);
                                }

                            }
                        }
                    }
                }
                else
                {
                    groupReferralBo.Add(new TreatyPricingGroupReferralBo());
                    foreach (var i in gtlBenefitNames.Distinct().ToList())
                    {
                        foreach (var u in gtlDesignations.Distinct().ToList())
                        {
                            var emptyBo = new TreatyPricingGroupReferralGtlTableBo();
                            emptyBo.Designation = u;
                            emptyBo.BenefitCode = i;
                            treatyPricingGroupReferralGtlTableBos.Add(emptyBo);
                        }

                    }
                }
            }

            return Json(new { groupReferrals = groupReferralBo, groupReferralGtls = treatyPricingGroupReferralGtlTableBos.OrderBy(q => q.Designation), benefitNames = gtlBenefitNames.Distinct().ToList(), gtlDesignation = gtlDesignations.Distinct().ToList(), designations = gtlDesignations.Distinct() });
        }


        #endregion

        #region Group Referral Report
        [HttpPost, Obsolete]
        public ActionResult GenerateGroupReferralReport(int typeOfReport, int? firstQuotationSentWeek, int? firstQuotationSentMonth, int? firstQuotationSentQuarter, int firstQuotationSentYear)
        {
            ReportParameter[] param = new ReportParameter[5];
            param[0] = new ReportParameter("TypeOfReport", typeOfReport.ToString());
            param[1] = new ReportParameter("FirstQuotationSentWeek", firstQuotationSentWeek.HasValue ? firstQuotationSentWeek.ToString() : "1");
            param[2] = new ReportParameter("FirstQuotationSentMonth", firstQuotationSentMonth.HasValue ? firstQuotationSentMonth.ToString() : "1");
            param[3] = new ReportParameter("FirstQuotationSentQuarter", firstQuotationSentQuarter.HasValue ? firstQuotationSentQuarter.ToString() : "1");
            param[4] = new ReportParameter("FirstQuotationSentYear", firstQuotationSentYear.ToString());

            Report(string.Format("/{0}", ModuleBo.ModuleController.GroupReferralReport.ToString()), true, param);

            return PartialView("_SSRSWithoutParameters");
        }
        #endregion


        #endregion

        #region Target Planning Report
        [HttpPost]
        public JsonResult GetTargetPlanningReportOutput(string extractionDateStr, string extractionFrom, string extractionTo, bool isProfitComm)
        {
            List<string> errors = new List<string>();
            List<string> columnHeaders = new List<string>();
            List<TargetPlanningReportOutput> output = new List<TargetPlanningReportOutput>();

            if (!isProfitComm)
            {
                columnHeaders = GetTargetPlanningStatementTrackingColumnHeaders(extractionFrom, extractionTo);

                GenerateTargetPlanningStatementTrackingReport(extractionDateStr, extractionFrom, extractionTo, columnHeaders, ref errors, ref output);
            }
            else
            {
                int extractionFromInt = int.Parse(extractionFrom);
                int extractionToInt = int.Parse(extractionTo);

                var columnHeadersInt = GetTargetPlanningPCStatementTrackingColumnHeaders(extractionFromInt, extractionToInt);
                columnHeaders = columnHeadersInt.Select(q => q.ToString()).OrderBy(q => q).ToList();

                GenerateTargetPlanningPCStatementTrackingReport(extractionDateStr, extractionFromInt, extractionToInt, columnHeadersInt, ref errors, ref output);
            }

            return Json(new { errors, columnHeaders, output });
        }

        [HttpGet]
        public ActionResult DownloadTargetPlanningOutput(string extractionDateStr, string extractionFrom, string extractionTo, bool isProfitComm)
        {
            List<string> errors = new List<string>();
            List<string> columnHeaders = new List<string>();
            List<TargetPlanningReportOutput> output = new List<TargetPlanningReportOutput>();

            if (!isProfitComm)
            {
                columnHeaders = GetTargetPlanningStatementTrackingColumnHeaders(extractionFrom, extractionTo);

                GenerateTargetPlanningStatementTrackingReport(extractionDateStr, extractionFrom, extractionTo, columnHeaders, ref errors, ref output);
            }
            else
            {
                int extractionFromInt = int.Parse(extractionFrom);
                int extractionToInt = int.Parse(extractionTo);

                var columnHeadersInt = GetTargetPlanningPCStatementTrackingColumnHeaders(extractionFromInt, extractionToInt);
                columnHeaders = columnHeadersInt.Select(q => q.ToString()).OrderBy(q => q).ToList();

                GenerateTargetPlanningPCStatementTrackingReport(extractionDateStr, extractionFromInt, extractionToInt, columnHeadersInt, ref errors, ref output);
            }

            string filename = "";
            var process = new GenerateTargetPlanningReport()
            {
                ColumnHeaders = columnHeaders,
                Output = output
            };
            process.Process(ref filename);

            string path = Path.Combine(Util.GetTemporaryPath(), filename);
            if (System.IO.File.Exists(path) && path != "")
            {
                return File(
                    System.IO.File.ReadAllBytes(path),
                    System.Net.Mime.MediaTypeNames.Application.Octet,
                    filename
                );
            }
            return null;
        }

        public void GenerateTargetPlanningStatementTrackingReport(string extractionDateStr, string extractionFrom, string extractionTo, List<string> columnHeaders, ref List<string> errors, ref List<TargetPlanningReportOutput> output)
        {
            try
            {
                DateTime extractionDate = DateTime.Parse(extractionDateStr);

                List<string> distinctList = SoaDataService.GetDistinctForTargetPlanning();

                foreach (string distinctItem in distinctList)
                {
                    string[] items = distinctItem.Split('|');

                    List<string> outputData = new List<string>();

                    foreach (string columnHeader in columnHeaders)
                    {
                        string value = SoaDataService.GetTargetPlanningStatementTrackingValue(items[6], columnHeader);

                        outputData.Add(value);
                    }

                    output.Add(new TargetPlanningReportOutput
                    {
                        CedingCompany = items[3],
                        TreatyId = items[4],
                        PersonInCharge = items[5],
                        TreatyCode = items[6],
                        OutputData = outputData,
                    });
                }
            }
            catch (Exception ex)
            {
                errors.Add(string.Format("Error occurred. Error details: {0}", ex.Message));
            }
        }

        public void GenerateTargetPlanningPCStatementTrackingReport(string extractionDateStr, int extractionFrom, int extractionTo, List<int> columnHeaders, ref List<string> errors, ref List<TargetPlanningReportOutput> output)
        {
            try
            {
                DateTime extractionDate = DateTime.Parse(extractionDateStr);

                List<string> distinctList = SoaDataService.GetDistinctForTargetPlanning();

                foreach (string distinctItem in distinctList)
                {
                    string[] items = distinctItem.Split('|');

                    List<string> outputData = new List<string>();

                    foreach (int columnHeader in columnHeaders)
                    {
                        string value = SoaDataService.GetTargetPlanningPCStatementTrackingValue(items[6], columnHeader);

                        outputData.Add(value);
                    }

                    output.Add(new TargetPlanningReportOutput
                    {
                        CedingCompany = items[3],
                        TreatyId = items[4],
                        PersonInCharge = items[5],
                        TreatyCode = items[6],
                        OutputData = outputData,
                    });
                }
            }
            catch (Exception ex)
            {
                errors.Add(string.Format("Error occurred. Error details: {0}", ex.Message));
            }
        }

        List<string> GetTargetPlanningStatementTrackingColumnHeaders(string extractionFrom, string extractionTo)
        {
            List<string> columnHeaders = new List<string>();

            int startYear = int.Parse(extractionFrom.Substring(0, 4));
            int startQuarter = int.Parse(extractionFrom.Substring(5));
            int endYear = int.Parse(extractionTo.Substring(0, 4));
            int endQuarter = int.Parse(extractionTo.Substring(5));

            #region Process all entries in start year
            columnHeaders.Add(extractionFrom.Substring(2));

            if (endYear > startYear)
            {
                if (startQuarter < 4)
                {
                    for (int i = startQuarter + 1; i <= 4; i++)
                    {
                        columnHeaders.Add(startYear.ToString().Substring(2) + "Q" + i.ToString());
                    }
                }
            }
            else if (endQuarter > startQuarter)
            {
                for (int i = startQuarter + 1; i <= endQuarter; i++)
                {
                    columnHeaders.Add(startYear.ToString().Substring(2) + "Q" + i.ToString());
                }
            }
            #endregion

            if (endYear > startYear)
            {
                for (int i = startYear + 1; i < endYear; i++)
                {
                    for (int j = 1; j <= 4; j++)
                    {
                        columnHeaders.Add(i.ToString().Substring(2) + "Q" + j.ToString());
                    }
                }
            }

            #region Process all entries in end year
            if (endYear > startYear) // Ignore if start and end year are the same
            {
                if (endQuarter > 1)
                {
                    for (int i = 1; i < endQuarter; i++)
                    {
                        columnHeaders.Add(endYear.ToString().Substring(2) + "Q" + i.ToString());
                    }
                }

                columnHeaders.Add(extractionTo.Substring(2));
            }
            #endregion

            return columnHeaders;
        }

        List<int> GetTargetPlanningPCStatementTrackingColumnHeaders(int extractionFrom, int extractionTo)
        {
            List<int> columnHeaders = new List<int>();

            columnHeaders.Add(extractionFrom);

            if (extractionTo > extractionFrom)
            {
                for (int i = extractionFrom + 1; i < extractionTo; i++)
                {
                    columnHeaders.Add(i);
                }
            }

            if (extractionTo > extractionFrom)
                columnHeaders.Add(extractionTo);

            return columnHeaders;
        }

        #endregion

        public JsonResult ProcessComparisonHtmlTableReport(string type, List<string> rows)
        {
            List<string> errors = new List<string>();
            string fileName = type + "ComparisonReport" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";
            string path = Path.Combine(Util.GetTreatyPricingReportGenerationPath(), fileName);

            try
            {
                //Start processing file
                var process = new ProcessTreatyPricingComparisonReportHtml()
                {
                    ReportType = type,
                    FilePath = path,
                    StringRows = rows,
                };
                process.Process();
            }
            catch (Exception e)
            {
                errors.Add(e.Message);
            }

            return Json(new { errors, fileName, path });
        }

        public JsonResult ProcessProductComparisonHtmlTableReport(List<string> rows, List<int> benefitCount)
        {
            List<string> errors = new List<string>();
            string fileName = "ProductBenefitComparisonReport" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";
            string path = Path.Combine(Util.GetTreatyPricingReportGenerationPath(), fileName);

            try
            {
                //Start processing file
                var process = new ProcessTreatyPricingProductComparisonReportHtml()
                {
                    FilePath = path,
                    StringRows = rows,
                    BenefitCount = benefitCount,
                };
                process.Process();
            }
            catch (Exception e)
            {
                errors.Add(e.Message);
            }

            return Json(new { errors, fileName, path });
        }

        public JsonResult ProcessRateComparisonHtmlTableReport(string type, List<string> mergingRows, List<string> rows, List<string> rows2)
        {
            List<string> errors = new List<string>();
            string fileName = type + "ComparisonReport" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";
            string path = Path.Combine(Util.GetTreatyPricingReportGenerationPath(), fileName);

            try
            {
                //Start processing file
                var process = new ProcessTreatyPricingRateComparisonReportHtml()
                {
                    ReportType = type,
                    FilePath = path,
                    StringMergingRows = mergingRows,
                    StringRows = rows,
                    StringRows2 = rows2,
                };
                process.Process();
            }
            catch (Exception e)
            {
                errors.Add(e.Message);
            }

            return Json(new { errors, fileName, path });
        }

        public ActionResult DownloadComparisonHtmlTableReport(string fileName)
        {
            string path = Path.Combine(Util.GetTreatyPricingReportGenerationPath(), fileName);

            if (System.IO.File.Exists(path) && path != "")
            {
                return File(
                    System.IO.File.ReadAllBytes(path),
                    System.Net.Mime.MediaTypeNames.Application.Octet,
                    fileName
                );
            }
            return null;
        }
    }
}