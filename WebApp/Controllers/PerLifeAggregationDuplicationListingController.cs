using BusinessObject;
using BusinessObject.Retrocession;
using ConsoleApp.Commands.ProcessDatas;
using PagedList;
using Services.Retrocession;
using Shared;
using System;
using System.Data.Entity;
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
    public class PerLifeAggregationDuplicationListingController : BaseController
    {
        public const string Controller = "PerLifeAggregationDuplicationListing";

        // GET: PerLifeAggregationDuplicationListing
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string TreatyCode,
            string InsuredName,
            string InsuredGender,
            string InsuredDateOfBirth,
            string PolicyNumber,
            string ReinsuranceEffectiveDate,
            string FundsAccountingType,
            string ReinsuranceRiskBasis,
            string CedingPlanCode,
            string MLReBenefitCode,
            string CedingBenefitRiskCode,
            string CedingBenefitTypeCode,
            string TransactionType,
            int? ProceedToAggregate,
            string DateUpdated,
            int? ExceptionStatus,
            string Remarks,
            string SortOrder,
            int? Page)
        {

            var insuredDateOfBirth = Util.GetParseDateTime(InsuredDateOfBirth);
            var reinsuranceEffectiveDate = Util.GetParseDateTime(ReinsuranceEffectiveDate);
            var dateUpdated = Util.GetParseDateTime(DateUpdated);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["TreatyCode"] = TreatyCode,
                ["InsuredName"] = InsuredName,
                ["InsuredGender"] = InsuredGender,
                ["InsuredDateOfBirth"] = insuredDateOfBirth.HasValue ? InsuredDateOfBirth : null,
                ["PolicyNumber"] = PolicyNumber,
                ["ReinsuranceEffectiveDate"] = reinsuranceEffectiveDate.HasValue ? ReinsuranceEffectiveDate : null,
                ["FundsAccountingType"] = FundsAccountingType,
                ["ReinsuranceRiskBasis"] = ReinsuranceRiskBasis,
                ["CedingPlanCode"] = CedingPlanCode,
                ["MLReBenefitCode"] = MLReBenefitCode,
                ["CedingBenefitRiskCode"] = CedingBenefitRiskCode,
                ["CedingBenefitTypeCode"] = CedingBenefitTypeCode,
                ["TransactionType"] = TransactionType,
                ["ProceedToAggregate"] = ProceedToAggregate,
                ["DateUpdated"] = dateUpdated.HasValue ? DateUpdated : null,
                ["ExceptionStatus"] = ExceptionStatus,
                ["Remarks"] = Remarks,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortTreatyCode = GetSortParam("TreatyCode");
            ViewBag.SortInsuredName = GetSortParam("InsuredName");
            ViewBag.SortInsuredGender = GetSortParam("InsuredGender");
            ViewBag.SortInsuredDateOfBirth = GetSortParam("InsuredDateOfBirth");
            ViewBag.SortPolicyNumber = GetSortParam("PolicyNumber");
            ViewBag.SortReinsuranceEffectiveDate = GetSortParam("ReinsuranceEffectiveDate");
            ViewBag.SortFundsAccountingType = GetSortParam("FundsAccountingType");
            ViewBag.SortReinsuranceRiskBasis = GetSortParam("ReinsuranceRiskBasis");
            ViewBag.SortCedingPlanCode = GetSortParam("CedingPlanCode");
            ViewBag.SortMLReBenefitCode = GetSortParam("MLReBenefitCode");
            ViewBag.SortCedingBenefitRiskCode = GetSortParam("CedingBenefitRiskCode");
            ViewBag.SortCedingBenefitTypeCode = GetSortParam("CedingBenefitTypeCode");
            ViewBag.SortTransactionType = GetSortParam("TransactionType");
            ViewBag.SortDateUpdated = GetSortParam("DateUpdated");
            ViewBag.SortExceptionStatus = GetSortParam("ExceptionStatus");
            //ViewBag.SortRemarks = GetSortParam("Remarks");

            var query = _db.PerLifeAggregationDetailData
                .Where(q => q.IsException == true)
                .Where(q => q.ExceptionType == PerLifeAggregationDetailDataBo.ExceptionTypeDuplicationCheck)
                .Select(PerLifeAggregationDetailDataViewModel.Expression());

            if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => q.TreatyCode == TreatyCode);
            if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => q.InsuredName.Contains(InsuredName));
            if (!string.IsNullOrEmpty(InsuredGender)) query = query.Where(q => q.InsuredGenderCode == InsuredGender);
            if (insuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == insuredDateOfBirth);
            if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber.Contains(PolicyNumber));
            if (reinsuranceEffectiveDate.HasValue) query = query.Where(q => q.ReinsEffDatePol == reinsuranceEffectiveDate);
            if (!string.IsNullOrEmpty(FundsAccountingType)) query = query.Where(q => q.FundsAccountingTypeCode == FundsAccountingType);
            if (!string.IsNullOrEmpty(ReinsuranceRiskBasis)) query = query.Where(q => q.ReinsBasisCode == ReinsuranceRiskBasis);
            if (!string.IsNullOrEmpty(CedingPlanCode)) query = query.Where(q => q.CedingPlanCode.Contains(CedingPlanCode));
            if (!string.IsNullOrEmpty(MLReBenefitCode)) query = query.Where(q => q.MlreBenefitCode == MLReBenefitCode);
            if (!string.IsNullOrEmpty(CedingBenefitRiskCode)) query = query.Where(q => q.CedingBenefitRiskCode.Contains(CedingBenefitRiskCode));
            if (!string.IsNullOrEmpty(CedingBenefitTypeCode)) query = query.Where(q => q.CedingBenefitTypeCode == CedingBenefitTypeCode);
            if (!string.IsNullOrEmpty(TransactionType)) query = query.Where(q => q.TransactionTypeCode == TransactionType);
            if (ProceedToAggregate.HasValue) query = query.Where(q => q.ProceedStatus == ProceedToAggregate);
            if (dateUpdated.HasValue) query = query.Where(q => DbFunctions.TruncateTime(q.UpdatedAt) == DbFunctions.TruncateTime(dateUpdated));
            //if (ExceptionStatus.HasValue) query = query.Where(q => q.ExceptionType == ExceptionStatus);
            if (!string.IsNullOrEmpty(Remarks)) query = query.Where(q => q.Remarks.Contains(Remarks));

            if (SortOrder == Html.GetSortAsc("TreatyCodeId")) query = query.OrderBy(q => q.TreatyCode);
            else if (SortOrder == Html.GetSortDsc("TreatyCodeId")) query = query.OrderByDescending(q => q.TreatyCode);
            else if (SortOrder == Html.GetSortAsc("InsuredName")) query = query.OrderBy(q => q.InsuredName);
            else if (SortOrder == Html.GetSortDsc("InsuredName")) query = query.OrderByDescending(q => q.InsuredName);
            else if (SortOrder == Html.GetSortAsc("InsuredGender")) query = query.OrderBy(q => q.InsuredGenderCode);
            else if (SortOrder == Html.GetSortDsc("InsuredGender")) query = query.OrderByDescending(q => q.InsuredGenderCode);
            else if (SortOrder == Html.GetSortAsc("InsuredDateOfBirth")) query = query.OrderBy(q => q.InsuredDateOfBirth);
            else if (SortOrder == Html.GetSortDsc("InsuredDateOfBirth")) query = query.OrderByDescending(q => q.InsuredDateOfBirth);
            else if (SortOrder == Html.GetSortAsc("PolicyNumber")) query = query.OrderBy(q => q.PolicyNumber);
            else if (SortOrder == Html.GetSortDsc("PolicyNumber")) query = query.OrderByDescending(q => q.PolicyNumber);
            else if (SortOrder == Html.GetSortAsc("ReinsuranceEffectiveDate")) query = query.OrderBy(q => q.ReinsEffDatePol);
            else if (SortOrder == Html.GetSortDsc("ReinsuranceEffectiveDate")) query = query.OrderByDescending(q => q.ReinsEffDatePol);
            else if (SortOrder == Html.GetSortAsc("FundsAccountingType")) query = query.OrderBy(q => q.FundsAccountingTypeCode);
            else if (SortOrder == Html.GetSortDsc("FundsAccountingType")) query = query.OrderByDescending(q => q.FundsAccountingTypeCode);
            else if (SortOrder == Html.GetSortAsc("CedantPlanCode")) query = query.OrderBy(q => q.CedingPlanCode);
            else if (SortOrder == Html.GetSortDsc("CedantPlanCode")) query = query.OrderByDescending(q => q.CedingPlanCode);
            else if (SortOrder == Html.GetSortAsc("MLReBenefitCode")) query = query.OrderBy(q => q.MlreBenefitCode);
            else if (SortOrder == Html.GetSortDsc("MLReBenefitCode")) query = query.OrderByDescending(q => q.MlreBenefitCode);
            else if (SortOrder == Html.GetSortAsc("CedingBenefitRiskCode")) query = query.OrderBy(q => q.CedingBenefitRiskCode);
            else if (SortOrder == Html.GetSortDsc("CedingBenefitRiskCode")) query = query.OrderByDescending(q => q.CedingBenefitRiskCode);
            else if (SortOrder == Html.GetSortAsc("CedingBenefitTypeCode")) query = query.OrderBy(q => q.CedingBenefitTypeCode);
            else if (SortOrder == Html.GetSortDsc("CedingBenefitTypeCode")) query = query.OrderByDescending(q => q.CedingBenefitTypeCode);
            else if (SortOrder == Html.GetSortAsc("TransactionType")) query = query.OrderBy(q => q.TransactionTypeCode);
            else if (SortOrder == Html.GetSortDsc("TransactionType")) query = query.OrderByDescending(q => q.TransactionTypeCode);
            else if (SortOrder == Html.GetSortAsc("DateUpdated")) query = query.OrderBy(q => q.UpdatedAt);
            else if (SortOrder == Html.GetSortDsc("DateUpdated")) query = query.OrderByDescending(q => q.UpdatedAt);
            //else if (SortOrder == Html.GetSortAsc("ExceptionStatus")) query = query.OrderBy(q => q.ExceptionType);
            //else if (SortOrder == Html.GetSortDsc("ExceptionStatus")) query = query.OrderByDescending(q => q.ExceptionType);
            //else if (SortOrder == Html.GetSortAsc("Remarks")) query = query.OrderBy(q => q.Remarks);
            //else if (SortOrder == Html.GetSortDsc("Remarks")) query = query.OrderByDescending(q => q.Remarks);
            else query = query.OrderBy(q => q.Id);

            ViewBag.Total = query.Count();
            IndexPage();

            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        public ActionResult Download(
            string downloadToken,
            string TreatyCode,
            string InsuredName,
            string InsuredGender,
            string InsuredDateOfBirth,
            string PolicyNumber,
            string ReinsuranceEffectiveDate,
            string FundsAccountingType,
            string ReinsuranceRiskBasis,
            string CedingPlanCode,
            string MLReBenefitCode,
            string CedingBenefitRiskCode,
            string CedingBenefitTypeCode,
            string TransactionType,
            int? ProceedToAggregate,
            string DateUpdated,
            int? ExceptionStatus,
            string Remarks)
        {
            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            dynamic Params = new ExpandoObject();
            Params.TreatyCode = TreatyCode;
            Params.InsuredName = InsuredName;
            Params.InsuredGender = InsuredGender;
            Params.InsuredDateOfBirth = InsuredDateOfBirth;
            Params.PolicyNumber = PolicyNumber;
            Params.ReinsuranceEffectiveDate = ReinsuranceEffectiveDate;
            Params.FundsAccountingType = FundsAccountingType;
            Params.ReinsuranceRiskBasis = ReinsuranceRiskBasis;
            Params.CedingPlanCode = CedingPlanCode;
            Params.MLReBenefitCode = MLReBenefitCode;
            Params.CedingBenefitRiskCode = CedingBenefitRiskCode;
            Params.CedingBenefitTypeCode = CedingBenefitTypeCode;
            Params.TransactionType = TransactionType;
            Params.ProceedToAggregate = ProceedToAggregate;
            Params.DateUpdated = DateUpdated;
            Params.ExceptionStatus = ExceptionStatus;

            var export = new GenerateExportData();
            ExportBo exportBo = export.CreateExportPerLifeAggregationDuplicationListing(AuthUserId, Params);

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
            DropDownFundsAccountingTypeCode(codeAsValue: true);
            DropDownReinsBasisCode(codeAsValue: true);
            DropDownTransactionTypeCode(codeAsValue: true);
            DropDownCedingBenefitTypeCode(codeAsValue: true);
            DropDownBenefit(codeAsValue: true);
            DropDownProceedStatus(true);
            //DropDownExceptionStatus();
            SetViewBagMessage();
        }

        //// GET: PerLifeAggregationDuplicationListing/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            //LoadPage();
            //return View(new PerLifeAggregationDuplicationListingViewModel());
            return RedirectToAction("Index");
        }

        // Post: PerLifeAggregationDuplicationListing/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, PerLifeAggregationDuplicationListingViewModel model)
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
            //        Result = PerLifeAggregationDuplicationListingService.Create(ref bo, ref trail);
            //        if (Result.Valid)
            //        {
            //            CreateTrail(
            //                bo.Id,
            //                "Create Per Life Aggregation Duplication Listing"
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

        // GET:PerLifeAggregationDuplicationListing/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = PerLifeAggregationDetailDataService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            LoadPage();
            return View(new PerLifeAggregationDetailDataViewModel(bo));
        }

        // POST: PerLifeAggregationDuplicationListing/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, PerLifeAggregationDetailDataViewModel model)
        {
            var dbBo = PerLifeAggregationDetailDataService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();
                Result = PerLifeAggregationDetailDataService.Update(ref bo, ref trail);

                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Update Per Life Aggregation Detail Data"
                        );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage();
            return View(model);
        }

        // GET: PerLifeAggregationDuplicationListing/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            //SetViewBagMessage();
            //var bo = PerLifeAggregationDuplicationListingService.Find(id);
            //if (bo == null)
            //{
            //    return RedirectToAction("Index");
            //}

            //return View(new PerLifeAggregationDuplicationListingViewModel(bo));
            return RedirectToAction("Index");
        }

        // POST: PerLifeAggregationDuplicationListing/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, PerLifeAggregationDuplicationListingViewModel model)
        {
            //var bo = PerLifeAggregationDuplicationListingService.Find(id);
            //if (bo == null)
            //{
            //    return RedirectToAction("Index");
            //}

            //var trail = GetNewTrailObject();
            //Result = PerLifeAggregationDuplicationListingService.Delete(bo, ref trail);
            //if (Result.Valid)
            //{
            //    CreateTrail(
            //        bo.Id,
            //        "Delete Per Life Aggregation Duplication Listing"
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
            //var entity = new PerLifeAggregationDuplicationListing();
            //var cedantPlanCodeCodeMaxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("CedantPlanCode");
            //ViewBag.CedantPlanCodeMaxLength = cedantPlanCodeCodeMaxLengthAttr != null ? cedantPlanCodeCodeMaxLengthAttr.Length : 255;
            //var insuredNameMaxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("InsuredName");
            //ViewBag.InsuredNameMaxLength = insuredNameMaxLengthAttr != null ? insuredNameMaxLengthAttr.Length : 100;
            //var policyNumberMaxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("PolicyNumber");
            //ViewBag.PolicyNumberMaxLength = policyNumberMaxLengthAttr != null ? policyNumberMaxLengthAttr.Length : 50;
            //var cedingBenefitRiskCodeMaxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("CedingBenefitRiskCode");
            //ViewBag.PolicyNumberMaxLength = cedingBenefitRiskCodeMaxLengthAttr != null ? cedingBenefitRiskCodeMaxLengthAttr.Length : 50;
            //var cedingBenefitTypeCodeMaxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("CedingBenefitTypeCode");
            //ViewBag.PolicyNumberMaxLength = cedingBenefitTypeCodeMaxLengthAttr != null ? cedingBenefitTypeCodeMaxLengthAttr.Length : 50;

            //DropDownTreatyCode();
            //DropDownInsuredGenderCode();
            //DropDownBenefit();
            //DropDownFundsAccountingTypeCode();
            //DropDownReinsBasisCode();
            //DropDownTransactionTypeCode();
            //DropDownExceptionStatus();

            DropDownProceedStatus();

            SetViewBagMessage();
        }

        public void DropDownProceedStatus(bool withSelect = false)
        {
            var items = GetEmptyDropDownList(withSelect);
            for (int i = 1; i <= PerLifeAggregationDetailDataBo.ProceedStatusMax; i++)
            {
                items.Add(new SelectListItem { Text = PerLifeAggregationDetailDataBo.GetProceedStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownProceedStatuses = items;
        }

    }
}