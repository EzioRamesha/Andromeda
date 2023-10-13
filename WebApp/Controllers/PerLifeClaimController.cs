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
    public class PerLifeClaimController : BaseController
    {
        public const string Controller = "PerLifeClaim";
        public const string SubController = "PerLifeClaimData";

        // GET: PerLifeClaim
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string SoaQuarter,
            int? CutOffId,
            string ProcessingDate,
            int? FundsAccountingTypeCode,
            int? PersonInChargeId,
            int? Status,
            string SortOrder,
            int? Page)
        {
            var processingDate = Util.GetParseDateTime(ProcessingDate);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["SoaQuarter"] = SoaQuarter,
                ["CutOffId"] = CutOffId,
                ["ProcessingDate"] = processingDate.HasValue ? processingDate : null,
                ["FundsAccountingTypeCode"] = FundsAccountingTypeCode,
                ["PersonInChargeId"] = PersonInChargeId,
                ["Status"] = Status,
                ["SortOrder"] = SortOrder,
            };

            ViewBag.SortOrder = SortOrder;
            ViewBag.SortSoaQuarter = GetSortParam("SoaQuarter");
            ViewBag.SortCutOffId= GetSortParam("CutOffId");
            ViewBag.SortProcessingDate = GetSortParam("ProcessingDate");
            ViewBag.SortFundsAccountingTypeCode = GetSortParam("FundsAccountingTypeCode");
            ViewBag.SortPersonInChargeId = GetSortParam("PersonInChargeId");
            ViewBag.SortStatus = GetSortParam("Status");


            var query = _db.PerLifeClaims.Select(PerLifeClaimViewModel.Expression());

            if (!string.IsNullOrEmpty(SoaQuarter)) query = query.Where(q => q.SoaQuarter.Contains(SoaQuarter));
            if (CutOffId.HasValue) query = query.Where(q => q.CutOffId == CutOffId);
            if (processingDate.HasValue) query = query.Where(q => q.ProcessingDate == processingDate);
            if (FundsAccountingTypeCode.HasValue) query = query.Where(q => q.FundsAccountingTypePickListDetailId == FundsAccountingTypeCode);
            if (PersonInChargeId.HasValue) query = query.Where(q => q.PersonInCharge.Id == PersonInChargeId);
            if (Status.HasValue) query = query.Where(q => q.Status == Status);

            if (SortOrder == Html.GetSortAsc("SoaQuarter")) query = query.OrderBy(q => q.SoaQuarter);
            else if (SortOrder == Html.GetSortDsc("SoaQuarter")) query = query.OrderByDescending(q => q.SoaQuarter);
            if (SortOrder == Html.GetSortAsc("CutOffId")) query = query.OrderBy(q => q.CutOffId);
            else if (SortOrder == Html.GetSortDsc("CutOffId")) query = query.OrderByDescending(q => q.CutOffId);
            if (SortOrder == Html.GetSortAsc("ProcessingDate")) query = query.OrderBy(q => q.ProcessingDate);
            else if (SortOrder == Html.GetSortDsc("ProcessingDate")) query = query.OrderByDescending(q => q.ProcessingDate);
            else if (SortOrder == Html.GetSortAsc("FundsAccountingTypeCode")) query = query.OrderBy(q => q.FundsAccountingTypePickListDetailId);
            else if (SortOrder == Html.GetSortDsc("FundsAccountingTypeCode")) query = query.OrderByDescending(q => q.FundsAccountingTypePickListDetailId);
            else if (SortOrder == Html.GetSortAsc("PersonInChargeId")) query = query.OrderBy(q => q.PersonInChargeId);
            else if (SortOrder == Html.GetSortDsc("PersonInChargeId")) query = query.OrderByDescending(q => q.PersonInChargeId);
            else if (SortOrder == Html.GetSortAsc("Status")) query = query.OrderBy(q => q.Status);
            else if (SortOrder == Html.GetSortDsc("Status")) query = query.OrderByDescending(q => q.Status);
            else query = query.OrderBy(q => q.SoaQuarter);

            ViewBag.Total = query.Count();
            IndexPage();

            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        public void IndexPage()
        {
            DropDownCutOff();
            DropDownPerLifeClaimStatus();
            DropDownUser(UserBo.StatusActive, null, true, DepartmentBo.DepartmentRetrocession);
            DropDownFundsAccountingTypeCode();
            DropDownCutOffQuarterWithDate();
            SetViewBagMessage();
        }

        // GET: PerLifeClaim/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            var model = new PerLifeClaimViewModel();
            LoadPage(model);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, PerLifeClaimViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);
                bo.Status = PerLifeClaimBo.StatusPending;
                var trail = GetNewTrailObject();
                Result = PerLifeClaimService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;
                    //model.ProcessStatusHistory(AuthUserId, ref trail);

                    CreateTrail(
                        bo.Id,
                        "Create Per Life Claim"
                    );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage(model);
            return View(model);
        }

        #region Claim Register Data
        public ActionResult Edit(
            int id,
            bool? ClaimRegisterDataHasRedFlag,
            string ClaimRegisterDataEntryNo,
            string ClaimRegisterDataSoaQuarter,
            string ClaimRegisterDataClaimId,
            string ClaimRegisterDataClaimTransactionType,
            bool? ClaimRegisterDataIsReferralCase,
            int? ClaimRegisterDataRiDataId,
            string ClaimRegisterDataRecordType,
            string ClaimRegisterDataTreatyCode,
            string ClaimRegisterDataPolicyNumber,
            string ClaimRegisterDataCedingCompany,
            string ClaimRegisterDataInsuredName,
            string ClaimRegisterDataInsuredDateOfBirth,
            string ClaimRegisterDataLastTransaction,
            string ClaimRegisterDataDateOfReported,
            string ClaimRegisterDataCedantDateOfNotification,
            string ClaimRegisterDataDateOfRegister,
            string ClaimRegisterDataDateOfCommencement,
            string ClaimRegisterDataDateOfEvent,
            string ClaimRegisterDataSumInsured,
            string ClaimRegisterDataCauseOfEvent,
            int? ClaimRegisterDataClaimStatus,
            int? ClaimRegisterDataProvisionStatus,
            int? ClaimRegisterDataOffsetStatus,
            string ClaimRegisterDataMlreEventCode,
            string ClaimRegisterDataClaimCode,
            string ClaimRegisterDataMlreBenefitCode,
            string ClaimRegisterDataCedingPlanCode,
            string ClaimRegisterDataCedingBenefitRiskCode,
            string ClaimRegisterDataCedingBenefitTypeCode,
            string ClaimRegisterDataClaimRecoveryAmount,
            string ClaimRegisterDataMlreRetainAmount,
            string ClaimRegisterDataLateInterest,
            string ClaimRegisterDataExGratia,
            int? Page)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = PerLifeClaimService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index", "PerLifeAggregation");
            }

            var model = new PerLifeClaimViewModel(bo);

            ListClaimRegisterData(
                id,
                Page,
                ClaimRegisterDataHasRedFlag,
                ClaimRegisterDataEntryNo,
                ClaimRegisterDataSoaQuarter,
                ClaimRegisterDataClaimId,
                ClaimRegisterDataClaimTransactionType,
                ClaimRegisterDataIsReferralCase,
                ClaimRegisterDataRiDataId,
                ClaimRegisterDataRecordType,
                ClaimRegisterDataTreatyCode,
                ClaimRegisterDataPolicyNumber,
                ClaimRegisterDataCedingCompany,
                ClaimRegisterDataInsuredName,
                ClaimRegisterDataInsuredDateOfBirth,
                ClaimRegisterDataLastTransaction,
                ClaimRegisterDataDateOfReported,
                ClaimRegisterDataCedantDateOfNotification,
                ClaimRegisterDataDateOfRegister,
                ClaimRegisterDataDateOfCommencement,
                ClaimRegisterDataDateOfEvent,
                ClaimRegisterDataSumInsured,
                ClaimRegisterDataCauseOfEvent,
                ClaimRegisterDataClaimStatus,
                ClaimRegisterDataProvisionStatus,
                ClaimRegisterDataOffsetStatus,
                ClaimRegisterDataMlreEventCode,
                ClaimRegisterDataClaimCode,
                ClaimRegisterDataMlreBenefitCode,
                ClaimRegisterDataCedingPlanCode,
                ClaimRegisterDataCedingBenefitRiskCode,
                ClaimRegisterDataCedingBenefitTypeCode,
                ClaimRegisterDataClaimRecoveryAmount,
                ClaimRegisterDataMlreRetainAmount,
                ClaimRegisterDataLateInterest,
                ClaimRegisterDataExGratia
            );

            ListEmptyException();

            ListEmptyClaimRetroData();

            ListEmptySummary();

            ListEmptySummaryPendingClaims();

            ListEmptySummaryClaimsRemoved();

            model.ActiveTab = PerLifeClaimBo.ActiveTabClaimRegisterData;
            LoadPage(model);
            return View(model);
        }

        public void ListClaimRegisterData(
            int id,
            int? Page,
            bool? ClaimRegisterDataHasRedFlag = null,
            string ClaimRegisterDataEntryNo = null,
            string ClaimRegisterDataSoaQuarter = null,
            string ClaimRegisterDataClaimId = null,
            string ClaimRegisterDataClaimTransactionType = null,
            bool? ClaimRegisterDataIsReferralCase = null,
            int? ClaimRegisterDataRiDataId = null,
            string ClaimRegisterDataRecordType = null,
            string ClaimRegisterDataTreatyCode = null,
            string ClaimRegisterDataPolicyNumber = null,
            string ClaimRegisterDataCedingCompany = null,
            string ClaimRegisterDataInsuredName = null,
            string ClaimRegisterDataInsuredDateOfBirth = null,
            string ClaimRegisterDataLastTransaction = null,
            string ClaimRegisterDataDateOfReported = null,
            string ClaimRegisterDataCedantDateOfNotification = null,
            string ClaimRegisterDataDateOfRegister = null,
            string ClaimRegisterDataDateOfCommencement = null,
            string ClaimRegisterDataDateOfEvent = null,
            string ClaimRegisterDataSumInsured = null,
            string ClaimRegisterDataCauseOfEvent = null,
            int? ClaimRegisterDataClaimStatus = null,
            int? ClaimRegisterDataProvisionStatus = null,
            int? ClaimRegisterDataOffsetStatus = null,
            string ClaimRegisterDataMlreEventCode = null,
            string ClaimRegisterDataClaimCode = null,
            string ClaimRegisterDataMlreBenefitCode = null,
            string ClaimRegisterDataCedingPlanCode = null,
            string ClaimRegisterDataCedingBenefitRiskCode = null,
            string ClaimRegisterDataCedingBenefitTypeCode = null,
            string ClaimRegisterDataClaimRecoveryAmount = null,
            string ClaimRegisterDataMlreRetainAmount = null,
            string ClaimRegisterDataLateInterest = null,
            string ClaimRegisterDataExGratia = null
            )
        {
            DateTime? insuredDateOfBirth = Util.GetParseDateTime(ClaimRegisterDataInsuredDateOfBirth);
            DateTime? lastTransaction = Util.GetParseDateTime(ClaimRegisterDataLastTransaction);
            DateTime? dateOfReported = Util.GetParseDateTime(ClaimRegisterDataDateOfReported);
            DateTime? cedantDateOfNotification = Util.GetParseDateTime(ClaimRegisterDataCedantDateOfNotification);
            DateTime? dateOfRegister = Util.GetParseDateTime(ClaimRegisterDataDateOfRegister);
            DateTime? dateOfCommencement = Util.GetParseDateTime(ClaimRegisterDataDateOfCommencement);
            DateTime? dateOfEvent = Util.GetParseDateTime(ClaimRegisterDataDateOfEvent);

            double? sumInsured = Util.StringToDouble(ClaimRegisterDataSumInsured);
            double? claimRecoveryAmount = Util.StringToDouble(ClaimRegisterDataClaimRecoveryAmount);
            double? mlreRetainAmount = Util.StringToDouble(ClaimRegisterDataMlreRetainAmount);
            double? lateInterest = Util.StringToDouble(ClaimRegisterDataLateInterest);
            double? exGratia = Util.StringToDouble(ClaimRegisterDataExGratia);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["ClaimRegisterDataHasRedFlag"] = ClaimRegisterDataHasRedFlag,
                ["ClaimRegisterDataEntryNo"] = ClaimRegisterDataEntryNo,
                ["ClaimRegisterDataSoaQuarter"] = ClaimRegisterDataSoaQuarter,
                ["ClaimRegisterDataClaimId"] = ClaimRegisterDataClaimId,
                ["ClaimTransactionTypeId"] = ClaimRegisterDataClaimTransactionType,
                ["ClaimRegisterDataReferralCaseIndicator"] = ClaimRegisterDataIsReferralCase,
                ["ClaimRegisterDataRiDataId"] = ClaimRegisterDataRiDataId,
                ["ClaimRegisterDataRecordType"] = ClaimRegisterDataRecordType,
                ["ClaimRegisterDataTreatyCode"] = ClaimRegisterDataTreatyCode,
                ["ClaimRegisterDataPolicyNumber"] = ClaimRegisterDataPolicyNumber,
                ["ClaimRegisterDataCedingCompany"] = ClaimRegisterDataCedingCompany,
                ["ClaimRegisterDataInsuredName"] = ClaimRegisterDataInsuredName,
                ["ClaimRegisterDataInsuredDateOfBirth"] = insuredDateOfBirth.HasValue ? ClaimRegisterDataInsuredDateOfBirth : null,
                ["ClaimRegisterDataLastTransaction"] = lastTransaction.HasValue ? ClaimRegisterDataLastTransaction : null,
                ["ClaimRegisterDataDateOfReported"] = dateOfReported.HasValue ? ClaimRegisterDataDateOfReported : null,
                ["ClaimRegisterDataCedantDateOfNotification"] = cedantDateOfNotification.HasValue ? ClaimRegisterDataCedantDateOfNotification : null,
                ["ClaimRegisterDataDateOfRegister"] = dateOfRegister.HasValue ? ClaimRegisterDataDateOfRegister : null,
                ["ClaimRegisterDataDateOfCommencement"] = dateOfCommencement.HasValue ? ClaimRegisterDataDateOfCommencement : null,
                ["ClaimRegisterDataDateOfEvent"] = dateOfEvent.HasValue ? ClaimRegisterDataDateOfEvent : null,
                ["ClaimRegisterDataSumInsured"] = sumInsured.HasValue ? ClaimRegisterDataSumInsured : null,
                ["ClaimRegisterDataCauseOfEvent"] = ClaimRegisterDataCauseOfEvent,
                ["ClaimRegisterDataClaimStatus"] = ClaimRegisterDataClaimStatus,
                ["ClaimRegisterDataProvisionStatus"] = ClaimRegisterDataProvisionStatus,
                ["ClaimRegisterDataOffsetStatus"] = ClaimRegisterDataOffsetStatus,
                ["ClaimRegisterDataMlreEventCode"] = ClaimRegisterDataMlreEventCode,
                ["ClaimRegisterDataClaimCode"] = ClaimRegisterDataClaimCode,
                ["ClaimRegisterDataMlreBenefitCode"] = ClaimRegisterDataMlreBenefitCode,
                ["ClaimRegisterDataCedingPlanCode"] = ClaimRegisterDataCedingPlanCode,
                ["ClaimRegisterDataCedingBenefitRiskCode"] = ClaimRegisterDataCedingBenefitRiskCode,
                ["ClaimRegisterDataCedingBenefitTypeCode"] = ClaimRegisterDataCedingBenefitTypeCode,
                ["ClaimRegisterDataClaimRecoveryAmount"] = claimRecoveryAmount.HasValue ? ClaimRegisterDataClaimRecoveryAmount : null,
                ["ClaimRegisterDataMlreRetainAmount"] = mlreRetainAmount.HasValue ? ClaimRegisterDataMlreRetainAmount : null,
                ["ClaimRegisterDataLateInterest"] = lateInterest.HasValue ? ClaimRegisterDataLateInterest : null,
                ["ClaimRegisterDataExGratia"] = exGratia.HasValue ? ClaimRegisterDataExGratia : null,
            };

            _db.Database.CommandTimeout = 0;

            var query = _db.PerLifeClaimData.Where(q => q.PerLifeClaimId == id && q.IsException == false).Select(PerLifeClaimViewModel.DataExpression());

            if (ClaimRegisterDataHasRedFlag.HasValue) query = query.Where(q => q.ClaimRegisterHistory.HasRedFlag == ClaimRegisterDataHasRedFlag);
            if (!string.IsNullOrEmpty(ClaimRegisterDataEntryNo)) query = query.Where(q => q.ClaimRegisterHistory.EntryNo == ClaimRegisterDataEntryNo);
            if (!string.IsNullOrEmpty(ClaimRegisterDataSoaQuarter)) query = query.Where(q => q.ClaimRegisterHistory.SoaQuarter == ClaimRegisterDataSoaQuarter);
            if (!string.IsNullOrEmpty(ClaimRegisterDataClaimId)) query = query.Where(q => q.ClaimRegisterHistory.ClaimId == ClaimRegisterDataClaimId);
            if (!string.IsNullOrEmpty(ClaimRegisterDataClaimTransactionType)) query = query.Where(q => q.ClaimRegisterHistory.ClaimTransactionType == ClaimRegisterDataClaimTransactionType);
            if (ClaimRegisterDataIsReferralCase.HasValue) query = query.Where(q => q.ClaimRegisterHistory.IsReferralCase == ClaimRegisterDataIsReferralCase);
            if (ClaimRegisterDataRiDataId.HasValue) query = query.Where(q => q.ClaimRegisterHistory.RiDataId == ClaimRegisterDataRiDataId);
            if (!string.IsNullOrEmpty(ClaimRegisterDataRecordType)) query = query.Where(q => q.ClaimRegisterHistory.RecordType == ClaimRegisterDataRecordType);
            if (!string.IsNullOrEmpty(ClaimRegisterDataTreatyCode)) query = query.Where(q => q.ClaimRegisterHistory.TreatyCode == ClaimRegisterDataTreatyCode);
            if (!string.IsNullOrEmpty(ClaimRegisterDataPolicyNumber)) query = query.Where(q => q.ClaimRegisterHistory.PolicyNumber == ClaimRegisterDataPolicyNumber);
            if (!string.IsNullOrEmpty(ClaimRegisterDataCedingCompany)) query = query.Where(q => q.ClaimRegisterHistory.CedingCompany == ClaimRegisterDataCedingCompany);
            if (!string.IsNullOrEmpty(ClaimRegisterDataInsuredName)) query = query.Where(q => q.ClaimRegisterHistory.InsuredName == ClaimRegisterDataInsuredName);
            if (insuredDateOfBirth.HasValue) query = query.Where(q => q.ClaimRegisterHistory.InsuredDateOfBirth == insuredDateOfBirth);
            if (lastTransaction.HasValue) query = query.Where(q => q.ClaimRegisterHistory.LastTransactionDate == lastTransaction);
            if (dateOfReported.HasValue) query = query.Where(q => q.ClaimRegisterHistory.DateOfReported == dateOfReported);
            if (cedantDateOfNotification.HasValue) query = query.Where(q => q.ClaimRegisterHistory.CedantDateOfNotification == cedantDateOfNotification);
            if (dateOfRegister.HasValue) query = query.Where(q => q.ClaimRegisterHistory.DateOfRegister == dateOfRegister);
            if (dateOfCommencement.HasValue) query = query.Where(q => q.ClaimRegisterHistory.DateOfIntimation == dateOfCommencement);
            if (dateOfEvent.HasValue) query = query.Where(q => q.ClaimRegisterHistory.DateOfEvent == dateOfEvent);
            if (sumInsured.HasValue) query = query.Where(q => q.ClaimRegisterHistory.SumIns == sumInsured);
            if (!string.IsNullOrEmpty(ClaimRegisterDataCauseOfEvent)) query = query.Where(q => q.ClaimRegisterHistory.CauseOfEvent == ClaimRegisterDataCauseOfEvent);
            if (ClaimRegisterDataClaimStatus.HasValue) query = query.Where(q => q.ClaimRegisterHistory.ClaimStatus == ClaimRegisterDataClaimStatus);
            if (ClaimRegisterDataProvisionStatus.HasValue) query = query.Where(q => q.ClaimRegisterHistory.ProvisionStatus == ClaimRegisterDataProvisionStatus);
            if (ClaimRegisterDataOffsetStatus.HasValue) query = query.Where(q => q.ClaimRegisterHistory.OffsetStatus == ClaimRegisterDataOffsetStatus);
            if (!string.IsNullOrEmpty(ClaimRegisterDataMlreEventCode)) query = query.Where(q => q.ClaimRegisterHistory.MlreEventCode == ClaimRegisterDataMlreEventCode);
            if (!string.IsNullOrEmpty(ClaimRegisterDataClaimCode)) query = query.Where(q => q.ClaimRegisterHistory.ClaimCode == ClaimRegisterDataClaimCode);
            if (!string.IsNullOrEmpty(ClaimRegisterDataMlreBenefitCode)) query = query.Where(q => q.ClaimRegisterHistory.MlreBenefitCode == ClaimRegisterDataMlreBenefitCode);
            if (!string.IsNullOrEmpty(ClaimRegisterDataCedingPlanCode)) query = query.Where(q => q.ClaimRegisterHistory.CedingPlanCode == ClaimRegisterDataCedingPlanCode);
            if (!string.IsNullOrEmpty(ClaimRegisterDataCedingBenefitRiskCode)) query = query.Where(q => q.ClaimRegisterHistory.CedingBenefitRiskCode == ClaimRegisterDataCedingBenefitRiskCode);
            if (!string.IsNullOrEmpty(ClaimRegisterDataCedingBenefitTypeCode)) query = query.Where(q => q.ClaimRegisterHistory.CedingBenefitTypeCode == ClaimRegisterDataCedingBenefitTypeCode);
            if (claimRecoveryAmount.HasValue) query = query.Where(q => q.ClaimRegisterHistory.ClaimRecoveryAmt == claimRecoveryAmount);
            if (mlreRetainAmount.HasValue) query = query.Where(q => q.ClaimRegisterHistory.MlreRetainAmount == mlreRetainAmount);
            if (lateInterest.HasValue) query = query.Where(q => q.ClaimRegisterHistory.LateInterest == lateInterest);
            if (exGratia.HasValue) query = query.Where(q => q.ClaimRegisterHistory.ExGratia == exGratia);

            query = query.OrderBy(q => q.ClaimRegisterHistory.Id);

            ViewBag.ClaimRegisterDataTotal = query.Count();
            ViewBag.ClaimRegisterDataList = query.ToPagedList(Page ?? 1, PageSize);

        }


        public void ListEmptyClaimRegisterData()
        {
            ViewBag.RouteValue = new RouteValueDictionary { };
            ViewBag.ClaimRegisterDataTotal = 0;
            ViewBag.ClaimRegisterDataList = new List<PerLifeClaimViewModel>().ToPagedList(1, PageSize);
        }

        #endregion

        #region Exception
        public ActionResult Exception(
            int id,
            bool? ExceptionHasRedFlag,
            string ExceptionEntryNo,
            string ExceptionSoaQuarter,
            string ExceptionClaimId,
            string ExceptionClaimTransactionType,
            bool? ExceptionIsReferralCase,
            int? ExceptionRiDataId,
            string ExceptionRecordType,
            string ExceptionTreatyCode,
            string ExceptionPolicyNumber,
            string ExceptionCedingCompany,
            string ExceptionInsuredName,
            string ExceptionInsuredDateOfBirth,
            string ExceptionLastTransaction,
            string ExceptionDateOfReported,
            string ExceptionCedantDateOfNotification,
            string ExceptionDateOfRegister,
            string ExceptionDateOfCommencement,
            string ExceptionDateOfEvent,
            string ExceptionSumInsured,
            string ExceptionCauseOfEvent,
            int? ExceptionClaimStatus,
            int? ExceptionProvisionStatus,
            int? ExceptionOffsetStatus,
            string ExceptionMlreEventCode,
            string ExceptionClaimCode,
            string ExceptionMlreBenefitCode,
            string ExceptionCedingPlanCode,
            string ExceptionCedingBenefitRiskCode,
            string ExceptionCedingBenefitTypeCode,
            string ExceptionClaimRecoveryAmount,
            string ExceptionMlreRetainAmount,
            string ExceptionLateInterest,
            string ExceptionExGratia,
            int? Page)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = PerLifeClaimService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index", "PerLifeClaim");
            }

            var model = new PerLifeClaimViewModel(bo);

            ListEmptyClaimRegisterData();

            ListException(
                id,
                Page,
                ExceptionHasRedFlag,
                ExceptionEntryNo,
                ExceptionSoaQuarter,
                ExceptionClaimId,
                ExceptionClaimTransactionType,
                ExceptionIsReferralCase,
                ExceptionRiDataId,
                ExceptionRecordType,
                ExceptionTreatyCode,
                ExceptionPolicyNumber,
                ExceptionCedingCompany,
                ExceptionInsuredName,
                ExceptionInsuredDateOfBirth,
                ExceptionLastTransaction,
                ExceptionDateOfReported,
                ExceptionCedantDateOfNotification,
                ExceptionDateOfRegister,
                ExceptionDateOfCommencement,
                ExceptionDateOfEvent,
                ExceptionSumInsured,
                ExceptionCauseOfEvent,
                ExceptionClaimStatus,
                ExceptionProvisionStatus,
                ExceptionOffsetStatus,
                ExceptionMlreEventCode,
                ExceptionClaimCode,
                ExceptionMlreBenefitCode,
                ExceptionCedingPlanCode,
                ExceptionCedingBenefitRiskCode,
                ExceptionCedingBenefitTypeCode,
                ExceptionClaimRecoveryAmount,
                ExceptionMlreRetainAmount,
                ExceptionLateInterest,
                ExceptionExGratia
                );

            ListEmptyClaimRetroData();

            ListEmptySummary();

            ListEmptySummaryPendingClaims();

            ListEmptySummaryClaimsRemoved();

            model.ActiveTab = PerLifeClaimBo.ActiveTabException;
            LoadPage(model);
            return View("Edit", model);

        }

        public void ListException(
            int id,
            int? Page,
            bool? ExceptionHasRedFlag = null,
            string ExceptionEntryNo = null,
            string ExceptionSoaQuarter = null,
            string ExceptionClaimId = null,
            string ExceptionClaimTransactionType = null,
            bool? ExceptionIsReferralCase = null,
            int? ExceptionRiDataId = null,
            string ExceptionRecordType = null,
            string ExceptionTreatyCode = null,
            string ExceptionPolicyNumber = null,
            string ExceptionCedingCompany = null,
            string ExceptionInsuredName = null,
            string ExceptionInsuredDateOfBirth = null,
            string ExceptionLastTransaction = null,
            string ExceptionDateOfReported = null,
            string ExceptionCedantDateOfNotification = null,
            string ExceptionDateOfRegister = null,
            string ExceptionDateOfCommencement = null,
            string ExceptionDateOfEvent = null,
            string ExceptionSumInsured = null,
            string ExceptionCauseOfEvent = null,
            int? ExceptionClaimStatus = null,
            int? ExceptionProvisionStatus = null,
            int? ExceptionOffsetStatus = null,
            string ExceptionMlreEventCode = null,
            string ExceptionClaimCode = null,
            string ExceptionMlreBenefitCode = null,
            string ExceptionCedingPlanCode = null,
            string ExceptionCedingBenefitRiskCode = null,
            string ExceptionCedingBenefitTypeCode = null,
            string ExceptionClaimRecoveryAmount = null,
            string ExceptionMlreRetainAmount = null,
            string ExceptionLateInterest = null,
            string ExceptionExGratia = null
            )
        {
            DateTime? insuredDateOfBirth = Util.GetParseDateTime(ExceptionInsuredDateOfBirth);
            DateTime? lastTransaction = Util.GetParseDateTime(ExceptionLastTransaction);
            DateTime? dateOfReported = Util.GetParseDateTime(ExceptionDateOfReported);
            DateTime? cedantDateOfNotification = Util.GetParseDateTime(ExceptionCedantDateOfNotification);
            DateTime? dateOfRegister = Util.GetParseDateTime(ExceptionDateOfRegister);
            DateTime? dateOfCommencement = Util.GetParseDateTime(ExceptionDateOfCommencement);
            DateTime? dateOfEvent = Util.GetParseDateTime(ExceptionDateOfEvent);

            double? sumInsured = Util.StringToDouble(ExceptionSumInsured);
            double? claimRecoveryAmount = Util.StringToDouble(ExceptionClaimRecoveryAmount);
            double? mlreRetainAmount = Util.StringToDouble(ExceptionMlreRetainAmount);
            double? lateInterest = Util.StringToDouble(ExceptionLateInterest);
            double? exGratia = Util.StringToDouble(ExceptionExGratia);

            ViewBag.ExceptionRouteValue = new RouteValueDictionary
            {
                ["ExceptionHasRedFlag"] = ExceptionHasRedFlag,
                ["ExceptionEntryNo"] = ExceptionEntryNo,
                ["ExceptionSoaQuarter"] = ExceptionSoaQuarter,
                ["ExceptionClaimId"] = ExceptionClaimId,
                ["ExceptionClaimTransactionTypeId"] = ExceptionClaimTransactionType,
                ["ExceptionReferralCaseIndicator"] = ExceptionIsReferralCase,
                ["ExceptionRiDataId"] = ExceptionRiDataId,
                ["ExceptionRecordType"] = ExceptionRecordType,
                ["ExceptionTreatyCode"] = ExceptionTreatyCode,
                ["ExceptionPolicyNumber"] = ExceptionPolicyNumber,
                ["ExceptionCedingCompany"] = ExceptionCedingCompany,
                ["ExceptionInsuredName"] = ExceptionInsuredName,
                ["ExceptionInsuredDateOfBirth"] = insuredDateOfBirth.HasValue ? ExceptionInsuredDateOfBirth : null,
                ["ExceptionLastTransaction"] = lastTransaction.HasValue ? ExceptionLastTransaction : null,
                ["ExceptionDateOfReported"] = dateOfReported.HasValue ? ExceptionDateOfReported : null,
                ["ExceptionCedantDateOfNotification"] = cedantDateOfNotification.HasValue ? ExceptionCedantDateOfNotification : null,
                ["ExceptionDateOfRegister"] = dateOfRegister.HasValue ? ExceptionDateOfRegister : null,
                ["ExceptionDateOfCommencement"] = dateOfCommencement.HasValue ? ExceptionDateOfCommencement : null,
                ["ExceptionDateOfEvent"] = dateOfEvent.HasValue ? ExceptionDateOfEvent : null,
                ["ExceptionSumInsured"] = sumInsured.HasValue ? ExceptionSumInsured : null,
                ["ExceptionCauseOfEvent"] = ExceptionCauseOfEvent,
                ["ExceptionClaimStatus"] = ExceptionClaimStatus,
                ["ExceptionProvisionStatus"] = ExceptionProvisionStatus,
                ["ExceptionOffsetStatus"] = ExceptionOffsetStatus,
                ["ExceptionMlreEventCode"] = ExceptionMlreEventCode,
                ["ExceptionClaimCode"] = ExceptionClaimCode,
                ["ExceptionMlreBenefitCode"] = ExceptionMlreBenefitCode,
                ["ExceptionCedingPlanCode"] = ExceptionCedingPlanCode,
                ["ExceptionCedingBenefitRiskCode"] = ExceptionCedingBenefitRiskCode,
                ["ExceptionCedingBenefitTypeCode"] = ExceptionCedingBenefitTypeCode,
                ["ExceptionClaimRecoveryAmount"] = claimRecoveryAmount.HasValue ? ExceptionClaimRecoveryAmount : null,
                ["ExceptionMlreRetainAmount"] = mlreRetainAmount.HasValue ? ExceptionMlreRetainAmount : null,
                ["ExceptionLateInterest"] = lateInterest.HasValue ? ExceptionLateInterest : null,
                ["ExceptionExGratia"] = exGratia.HasValue ? ExceptionExGratia : null,
            };

            _db.Database.CommandTimeout = 0;

            var query = _db.PerLifeClaimData.Where(q => q.PerLifeClaimId == id && q.IsException == true).Select(PerLifeClaimViewModel.DataExpression());

            if (ExceptionHasRedFlag.HasValue) query = query.Where(q => q.ClaimRegisterHistory.HasRedFlag == ExceptionHasRedFlag);
            if (!string.IsNullOrEmpty(ExceptionEntryNo)) query = query.Where(q => q.ClaimRegisterHistory.EntryNo == ExceptionEntryNo);
            if (!string.IsNullOrEmpty(ExceptionSoaQuarter)) query = query.Where(q => q.ClaimRegisterHistory.SoaQuarter == ExceptionSoaQuarter);
            if (!string.IsNullOrEmpty(ExceptionClaimId)) query = query.Where(q => q.ClaimRegisterHistory.ClaimId == ExceptionClaimId);
            if (!string.IsNullOrEmpty(ExceptionClaimTransactionType)) query = query.Where(q => q.ClaimRegisterHistory.ClaimTransactionType == ExceptionClaimTransactionType);
            if (ExceptionIsReferralCase.HasValue) query = query.Where(q => q.ClaimRegisterHistory.IsReferralCase == ExceptionIsReferralCase);
            if (ExceptionRiDataId.HasValue) query = query.Where(q => q.ClaimRegisterHistory.RiDataId == ExceptionRiDataId);
            if (!string.IsNullOrEmpty(ExceptionRecordType)) query = query.Where(q => q.ClaimRegisterHistory.RecordType == ExceptionRecordType);
            if (!string.IsNullOrEmpty(ExceptionTreatyCode)) query = query.Where(q => q.ClaimRegisterHistory.TreatyCode == ExceptionTreatyCode);
            if (!string.IsNullOrEmpty(ExceptionPolicyNumber)) query = query.Where(q => q.ClaimRegisterHistory.PolicyNumber == ExceptionPolicyNumber);
            if (!string.IsNullOrEmpty(ExceptionCedingCompany)) query = query.Where(q => q.ClaimRegisterHistory.CedingCompany == ExceptionCedingCompany);
            if (!string.IsNullOrEmpty(ExceptionInsuredName)) query = query.Where(q => q.ClaimRegisterHistory.InsuredName == ExceptionInsuredName);
            if (insuredDateOfBirth.HasValue) query = query.Where(q => q.ClaimRegisterHistory.InsuredDateOfBirth == insuredDateOfBirth);
            if (lastTransaction.HasValue) query = query.Where(q => q.ClaimRegisterHistory.LastTransactionDate == lastTransaction);
            if (dateOfReported.HasValue) query = query.Where(q => q.ClaimRegisterHistory.DateOfReported == dateOfReported);
            if (cedantDateOfNotification.HasValue) query = query.Where(q => q.ClaimRegisterHistory.CedantDateOfNotification == cedantDateOfNotification);
            if (dateOfRegister.HasValue) query = query.Where(q => q.ClaimRegisterHistory.DateOfRegister == dateOfRegister);
            if (dateOfCommencement.HasValue) query = query.Where(q => q.ClaimRegisterHistory.DateOfIntimation == dateOfCommencement);
            if (dateOfEvent.HasValue) query = query.Where(q => q.ClaimRegisterHistory.DateOfEvent == dateOfEvent);
            if (sumInsured.HasValue) query = query.Where(q => q.ClaimRegisterHistory.SumIns == sumInsured);
            if (!string.IsNullOrEmpty(ExceptionCauseOfEvent)) query = query.Where(q => q.ClaimRegisterHistory.CauseOfEvent == ExceptionCauseOfEvent);
            if (ExceptionClaimStatus.HasValue) query = query.Where(q => q.ClaimRegisterHistory.ClaimStatus == ExceptionClaimStatus);
            if (ExceptionProvisionStatus.HasValue) query = query.Where(q => q.ClaimRegisterHistory.ProvisionStatus == ExceptionProvisionStatus);
            if (ExceptionOffsetStatus.HasValue) query = query.Where(q => q.ClaimRegisterHistory.OffsetStatus == ExceptionOffsetStatus);
            if (!string.IsNullOrEmpty(ExceptionMlreEventCode)) query = query.Where(q => q.ClaimRegisterHistory.MlreEventCode == ExceptionMlreEventCode);
            if (!string.IsNullOrEmpty(ExceptionClaimCode)) query = query.Where(q => q.ClaimRegisterHistory.ClaimCode == ExceptionClaimCode);
            if (!string.IsNullOrEmpty(ExceptionMlreBenefitCode)) query = query.Where(q => q.ClaimRegisterHistory.MlreBenefitCode == ExceptionMlreBenefitCode);
            if (!string.IsNullOrEmpty(ExceptionCedingPlanCode)) query = query.Where(q => q.ClaimRegisterHistory.CedingPlanCode == ExceptionCedingPlanCode);
            if (!string.IsNullOrEmpty(ExceptionCedingBenefitRiskCode)) query = query.Where(q => q.ClaimRegisterHistory.CedingBenefitRiskCode == ExceptionCedingBenefitRiskCode);
            if (!string.IsNullOrEmpty(ExceptionCedingBenefitTypeCode)) query = query.Where(q => q.ClaimRegisterHistory.CedingBenefitTypeCode == ExceptionCedingBenefitTypeCode);
            if (claimRecoveryAmount.HasValue) query = query.Where(q => q.ClaimRegisterHistory.ClaimRecoveryAmt == claimRecoveryAmount);
            if (mlreRetainAmount.HasValue) query = query.Where(q => q.ClaimRegisterHistory.MlreRetainAmount == mlreRetainAmount);
            if (lateInterest.HasValue) query = query.Where(q => q.ClaimRegisterHistory.LateInterest == lateInterest);
            if (exGratia.HasValue) query = query.Where(q => q.ClaimRegisterHistory.ExGratia == exGratia);

            query = query.OrderBy(q => q.ClaimRegisterHistory.Id);

            ViewBag.ExceptionTotal = query.Count();
            ViewBag.ExceptionList = query.ToPagedList(Page ?? 1, PageSize);

        }


        public void ListEmptyException()
        {
            ViewBag.ExceptionRouteValue = new RouteValueDictionary { };
            ViewBag.ExceptionListTotal = 0;
            ViewBag.ExceptionList = new List<PerLifeClaimViewModel>().ToPagedList(1, PageSize);
        }
        #endregion

        #region Claim Retro Data
        public ActionResult ClaimRetroData(
            int id,
            bool? ClaimRetroDataHasRedFlag,
            string ClaimRetroDataEntryNo,
            string ClaimRetroDataSoaQuarter,
            string ClaimRetroDataClaimId,
            string ClaimRetroDataClaimTransactionType,
            bool? ClaimRetroDataIsReferralCase,
            int? ClaimRetroDataRiDataId,
            string ClaimRetroDataRecordType,
            string ClaimRetroDataTreatyCode,
            string ClaimRetroDataPolicyNumber,
            string ClaimRetroDataCedingCompany,
            string ClaimRetroDataInsuredName,
            string ClaimRetroDataInsuredDateOfBirth,
            string ClaimRetroDataLastTransaction,
            string ClaimRetroDataDateOfReported,
            string ClaimRetroDataCedantDateOfNotification,
            string ClaimRetroDataDateOfRegister,
            string ClaimRetroDataDateOfCommencement,
            string ClaimRetroDataDateOfEvent,
            string ClaimRetroDataSumInsured,
            string ClaimRetroDataCauseOfEvent,
            int? ClaimRetroDataOffsetStatus,
            string ClaimRetroDataRecovered,
            int? ClaimRetroDataClaimCategory,
            string ClaimRetroDataClaimRecoveryAmount,
            string ClaimRetroDataLateInterest,
            string ClaimRetroDataExGratia,
            int? Page)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = PerLifeClaimService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index", "PerLifeClaim");
            }

            var model = new PerLifeClaimViewModel(bo);

            ListEmptyClaimRegisterData();

            ListEmptyException();

            ListClaimRetroData(
                id,
                Page,
                ClaimRetroDataHasRedFlag,
                ClaimRetroDataEntryNo,
                ClaimRetroDataSoaQuarter,
                ClaimRetroDataClaimId,
                ClaimRetroDataClaimTransactionType,
                ClaimRetroDataIsReferralCase,
                ClaimRetroDataRiDataId,
                ClaimRetroDataRecordType,
                ClaimRetroDataTreatyCode,
                ClaimRetroDataPolicyNumber,
                ClaimRetroDataCedingCompany,
                ClaimRetroDataInsuredName,
                ClaimRetroDataInsuredDateOfBirth,
                ClaimRetroDataLastTransaction,
                ClaimRetroDataDateOfReported,
                ClaimRetroDataCedantDateOfNotification,
                ClaimRetroDataDateOfRegister,
                ClaimRetroDataDateOfCommencement,
                ClaimRetroDataDateOfEvent,
                ClaimRetroDataSumInsured,
                ClaimRetroDataCauseOfEvent,
                ClaimRetroDataOffsetStatus,
                ClaimRetroDataRecovered,
                ClaimRetroDataClaimCategory,
                ClaimRetroDataClaimRecoveryAmount,
                ClaimRetroDataLateInterest,
                ClaimRetroDataExGratia
                );

            ListEmptySummary();

            ListEmptySummaryPendingClaims();

            ListEmptySummaryClaimsRemoved();

            model.ActiveTab = PerLifeClaimBo.ActiveTabClaimRetroData;
            LoadPage(model);
            return View("Edit", model);

        }

        public void ListClaimRetroData(
            int id,
            int? Page,
            bool? ClaimRetroDataHasRedFlag = null,
            string ClaimRetroDataEntryNo = null,
            string ClaimRetroDataSoaQuarter = null,
            string ClaimRetroDataClaimId = null,
            string ClaimRetroDataClaimTransactionType = null,
            bool? ClaimRetroDataIsReferralCase = null,
            int? ClaimRetroDataRiDataId = null,
            string ClaimRetroDataRecordType = null,
            string ClaimRetroDataTreatyCode = null,
            string ClaimRetroDataPolicyNumber = null,
            string ClaimRetroDataCedingCompany = null,
            string ClaimRetroDataInsuredName = null,
            string ClaimRetroDataInsuredDateOfBirth = null,
            string ClaimRetroDataLastTransaction = null,
            string ClaimRetroDataDateOfReported = null,
            string ClaimRetroDataCedantDateOfNotification = null,
            string ClaimRetroDataDateOfRegister = null,
            string ClaimRetroDataDateOfCommencement = null,
            string ClaimRetroDataDateOfEvent = null,
            string ClaimRetroDataSumInsured = null,
            string ClaimRetroDataCauseOfEvent = null,
            int? ClaimRetroDataOffsetStatus = null,
            string ClaimRetroDataRecovered = null,
            int? ClaimRetroDataClaimCategory = null,
            string ClaimRetroDataClaimRecoveryAmount = null,
            string ClaimRetroDataLateInterest = null,
            string ClaimRetroDataExGratia = null
            )
        {
            DateTime? insuredDateOfBirth = Util.GetParseDateTime(ClaimRetroDataInsuredDateOfBirth);
            DateTime? lastTransaction = Util.GetParseDateTime(ClaimRetroDataLastTransaction);
            DateTime? dateOfReported = Util.GetParseDateTime(ClaimRetroDataDateOfReported);
            DateTime? cedantDateOfNotification = Util.GetParseDateTime(ClaimRetroDataCedantDateOfNotification);
            DateTime? dateOfRegister = Util.GetParseDateTime(ClaimRetroDataDateOfRegister);
            DateTime? dateOfCommencement = Util.GetParseDateTime(ClaimRetroDataDateOfCommencement);
            DateTime? dateOfEvent = Util.GetParseDateTime(ClaimRetroDataDateOfEvent);

            double? sumInsured = Util.StringToDouble(ClaimRetroDataSumInsured);
            double? claimRecoveryAmount = Util.StringToDouble(ClaimRetroDataClaimRecoveryAmount);
            double? lateInterest = Util.StringToDouble(ClaimRetroDataLateInterest);
            double? exGratia = Util.StringToDouble(ClaimRetroDataExGratia);

            ViewBag.ClaimRetroDataRouteValue = new RouteValueDictionary
            {
                ["ClaimRetroDataHasRedFlag"] = ClaimRetroDataHasRedFlag,
                ["ClaimRetroDataEntryNo"] = ClaimRetroDataEntryNo,
                ["ClaimRetroDataSoaQuarter"] = ClaimRetroDataSoaQuarter,
                ["ClaimRetroDataClaimId"] = ClaimRetroDataClaimId,
                ["ClaimRetroDataClaimTransactionTypeId"] = ClaimRetroDataClaimTransactionType,
                ["ClaimRetroDataReferralCaseIndicator"] = ClaimRetroDataIsReferralCase,
                ["ClaimRetroDataRiDataId"] = ClaimRetroDataRiDataId,
                ["ClaimRetroDataRecordType"] = ClaimRetroDataRecordType,
                ["ClaimRetroDataTreatyCode"] = ClaimRetroDataTreatyCode,
                ["ClaimRetroDataPolicyNumber"] = ClaimRetroDataPolicyNumber,
                ["ClaimRetroDataCedingCompany"] = ClaimRetroDataCedingCompany,
                ["ClaimRetroDataInsuredName"] = ClaimRetroDataInsuredName,
                ["ClaimRetroDataInsuredDateOfBirth"] = insuredDateOfBirth.HasValue ? ClaimRetroDataInsuredDateOfBirth : null,
                ["ClaimRetroDataLastTransaction"] = lastTransaction.HasValue ? ClaimRetroDataLastTransaction : null,
                ["ClaimRetroDataDateOfReported"] = dateOfReported.HasValue ? ClaimRetroDataDateOfReported : null,
                ["ClaimRetroDataCedantDateOfNotification"] = cedantDateOfNotification.HasValue ? ClaimRetroDataCedantDateOfNotification : null,
                ["ClaimRetroDataDateOfRegister"] = dateOfRegister.HasValue ? ClaimRetroDataDateOfRegister : null,
                ["ClaimRetroDataDateOfCommencement"] = dateOfCommencement.HasValue ? ClaimRetroDataDateOfCommencement : null,
                ["ClaimRetroDataDateOfEvent"] = dateOfEvent.HasValue ? ClaimRetroDataDateOfEvent : null,
                ["ClaimRetroDataSumInsured"] = sumInsured.HasValue ? ClaimRetroDataSumInsured : null,
                ["ClaimRetroDataCauseOfEvent"] = ClaimRetroDataCauseOfEvent,
                ["ClaimRetroDataOffsetStatus"] = ClaimRetroDataOffsetStatus,
                ["ClaimRetroDataRecovered"] = ClaimRetroDataRecovered,
                ["ClaimRetroDataClaimCategory"] = ClaimRetroDataClaimCategory,
                ["ClaimRetroDataClaimRecoveryAmount"] = claimRecoveryAmount.HasValue ? ClaimRetroDataClaimRecoveryAmount : null,
                ["ClaimRetroDataLateInterest"] = lateInterest.HasValue ? ClaimRetroDataLateInterest : null,
                ["ClaimRetroDataExGratia"] = exGratia.HasValue ? ClaimRetroDataExGratia : null,
            };

            _db.Database.CommandTimeout = 0;

            var perLifeClaimDataIds = new List<int>();

            perLifeClaimDataIds = _db.PerLifeClaimData.Where(q => q.PerLifeClaimId == id).Select(q => q.Id).ToList();

            var query = _db.PerLifeClaimRetroData.Where(q => q.PerLifeClaimData.PerLifeClaimId == id).Select(PerLifeClaimViewModel.ClaimRetroDataExpression());

            if (ClaimRetroDataHasRedFlag.HasValue) query = query.Where(q => q.ClaimRegisterHistory.HasRedFlag == ClaimRetroDataHasRedFlag);
            if (!string.IsNullOrEmpty(ClaimRetroDataEntryNo)) query = query.Where(q => q.ClaimRegisterHistory.EntryNo == ClaimRetroDataEntryNo);
            if (!string.IsNullOrEmpty(ClaimRetroDataSoaQuarter)) query = query.Where(q => q.ClaimRegisterHistory.SoaQuarter == ClaimRetroDataSoaQuarter);
            if (!string.IsNullOrEmpty(ClaimRetroDataClaimId)) query = query.Where(q => q.ClaimRegisterHistory.ClaimId == ClaimRetroDataClaimId);
            if (!string.IsNullOrEmpty(ClaimRetroDataClaimTransactionType)) query = query.Where(q => q.ClaimRegisterHistory.ClaimTransactionType == ClaimRetroDataClaimTransactionType);
            if (ClaimRetroDataIsReferralCase.HasValue) query = query.Where(q => q.ClaimRegisterHistory.IsReferralCase == ClaimRetroDataIsReferralCase);
            if (ClaimRetroDataRiDataId.HasValue) query = query.Where(q => q.ClaimRegisterHistory.RiDataId == ClaimRetroDataRiDataId);
            if (!string.IsNullOrEmpty(ClaimRetroDataRecordType)) query = query.Where(q => q.ClaimRegisterHistory.RecordType == ClaimRetroDataRecordType);
            if (!string.IsNullOrEmpty(ClaimRetroDataTreatyCode)) query = query.Where(q => q.ClaimRegisterHistory.TreatyCode == ClaimRetroDataTreatyCode);
            if (!string.IsNullOrEmpty(ClaimRetroDataPolicyNumber)) query = query.Where(q => q.ClaimRegisterHistory.PolicyNumber == ClaimRetroDataPolicyNumber);
            if (!string.IsNullOrEmpty(ClaimRetroDataCedingCompany)) query = query.Where(q => q.ClaimRegisterHistory.CedingCompany == ClaimRetroDataCedingCompany);
            if (!string.IsNullOrEmpty(ClaimRetroDataInsuredName)) query = query.Where(q => q.ClaimRegisterHistory.InsuredName == ClaimRetroDataInsuredName);
            if (insuredDateOfBirth.HasValue) query = query.Where(q => q.ClaimRegisterHistory.InsuredDateOfBirth == insuredDateOfBirth);
            if (lastTransaction.HasValue) query = query.Where(q => q.ClaimRegisterHistory.LastTransactionDate == lastTransaction);
            if (dateOfReported.HasValue) query = query.Where(q => q.ClaimRegisterHistory.DateOfReported == dateOfReported);
            if (cedantDateOfNotification.HasValue) query = query.Where(q => q.ClaimRegisterHistory.CedantDateOfNotification == cedantDateOfNotification);
            if (dateOfRegister.HasValue) query = query.Where(q => q.ClaimRegisterHistory.DateOfRegister == dateOfRegister);
            if (dateOfCommencement.HasValue) query = query.Where(q => q.ClaimRegisterHistory.DateOfIntimation == dateOfCommencement);
            if (dateOfEvent.HasValue) query = query.Where(q => q.ClaimRegisterHistory.DateOfEvent == dateOfEvent);
            if (sumInsured.HasValue) query = query.Where(q => q.ClaimRegisterHistory.SumIns == sumInsured);
            if (!string.IsNullOrEmpty(ClaimRetroDataCauseOfEvent)) query = query.Where(q => q.ClaimRegisterHistory.CauseOfEvent == ClaimRetroDataCauseOfEvent);
            if (ClaimRetroDataOffsetStatus.HasValue) query = query.Where(q => q.ClaimRegisterHistory.OffsetStatus == ClaimRetroDataOffsetStatus);
            if (!string.IsNullOrEmpty(ClaimRetroDataRecovered)) query = query.Where(q => q.ClaimRetroDataRecovered == ClaimRetroDataRecovered);
            if (ClaimRetroDataClaimCategory.HasValue) query = query.Where(q => q.ClaimRetroDataCategory == ClaimRetroDataClaimCategory);
            if (claimRecoveryAmount.HasValue) query = query.Where(q => q.RetroClaimRecoveryAmount == claimRecoveryAmount);
            if (lateInterest.HasValue) query = query.Where(q => q.LateInterest == lateInterest);
            if (exGratia.HasValue) query = query.Where(q => q.ExGratia == exGratia);

            query = query.OrderBy(q => q.ClaimRegisterHistory.Id);

            ViewBag.ClaimRetroDataTotal = query.Count();
            ViewBag.ClaimRetroDataList = query.ToPagedList(Page ?? 1, PageSize);

        }

        public void ListEmptyClaimRetroData()
        {
            ViewBag.ClaimRetroDataRouteValue = new RouteValueDictionary { };
            ViewBag.ClaimRetroDataListTotal = 0;
            ViewBag.ClaimRetroDataList = new List<PerLifeClaimViewModel>().ToPagedList(1, PageSize);
        }
        #endregion

        #region Summary Paid Claims tab
        public ActionResult Summary(
            int id,
            bool? SummaryPaidClaimsHasRedFlag,
            string SummaryPaidClaimsEntryNo,
            string SummaryPaidClaimsSoaQuarter,
            string SummaryPaidClaimsClaimId,
            string SummaryPaidClaimsClaimTransactionType,
            bool? SummaryPaidClaimsIsReferralCase,
            int? SummaryPaidClaimsRiDataId,
            string SummaryPaidClaimsRecordType,
            string SummaryPaidClaimsTreatyCode,
            string SummaryPaidClaimsPolicyNumber,
            string SummaryPaidClaimsCedingCompany,
            string SummaryPaidClaimsInsuredName,
            string SummaryPaidClaimsInsuredDateOfBirth,
            string SummaryPaidClaimsLastTransaction,
            string SummaryPaidClaimsDateOfReported,
            string SummaryPaidClaimsCedantDateOfNotification,
            string SummaryPaidClaimsDateOfRegister,
            string SummaryPaidClaimsDateOfCommencement,
            string SummaryPaidClaimsDateOfEvent,
            int? SummaryPaidClaimsPolicyDuration,
            string SummaryPaidClaimsTargetDateToIssueInvoice,
            string SummaryPaidClaimsSumReinsured,
            string SummaryPaidClaimsCauseOfEvent,
            int? SummaryPaidClaimsPicClaims,
            int? SummaryPaidClaimsPicDaa,
            int? SummaryPaidClaimsStatus,
            int? SummaryPaidClaimsProvisionStatus,
            int? SummaryPaidClaimsOffsetStatus,
            int? Page)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = PerLifeClaimService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index", "PerLifeClaim");
            }

            var model = new PerLifeClaimViewModel(bo);

            ListEmptyClaimRegisterData();

            ListEmptyException();

            ListEmptyClaimRetroData();

            ListSummary(
                id,
                Page,
                SummaryPaidClaimsHasRedFlag,
                SummaryPaidClaimsEntryNo,
                SummaryPaidClaimsSoaQuarter,
                SummaryPaidClaimsClaimId,
                SummaryPaidClaimsClaimTransactionType,
                SummaryPaidClaimsIsReferralCase,
                SummaryPaidClaimsRiDataId,
                SummaryPaidClaimsRecordType,
                SummaryPaidClaimsTreatyCode,
                SummaryPaidClaimsPolicyNumber,
                SummaryPaidClaimsCedingCompany,
                SummaryPaidClaimsInsuredName,
                SummaryPaidClaimsInsuredDateOfBirth,
                SummaryPaidClaimsLastTransaction,
                SummaryPaidClaimsDateOfReported,
                SummaryPaidClaimsCedantDateOfNotification,
                SummaryPaidClaimsDateOfRegister,
                SummaryPaidClaimsDateOfCommencement,
                SummaryPaidClaimsDateOfEvent,
                SummaryPaidClaimsPolicyDuration,
                SummaryPaidClaimsTargetDateToIssueInvoice,
                SummaryPaidClaimsSumReinsured,
                SummaryPaidClaimsCauseOfEvent,
                SummaryPaidClaimsPicClaims,
                SummaryPaidClaimsPicDaa,
                SummaryPaidClaimsStatus,
                SummaryPaidClaimsProvisionStatus,
                SummaryPaidClaimsOffsetStatus
                );

            ListEmptySummaryPendingClaims();

            ListEmptySummaryClaimsRemoved();

            model.ActiveTab = PerLifeClaimBo.ActiveTabSummary;
            LoadPage(model);
            return View("Edit", model);

        }

        public void ListSummary(
            int id,
            int? Page,
            bool? SummaryPaidClaimsHasRedFlag = null,
            string SummaryPaidClaimsEntryNo = null,
            string SummaryPaidClaimsSoaQuarter = null,
            string SummaryPaidClaimsClaimId = null,
            string SummaryPaidClaimsClaimTransactionType = null,
            bool? SummaryPaidClaimsIsReferralCase = null,
            int? SummaryPaidClaimsRiDataId = null,
            string SummaryPaidClaimsRecordType = null,
            string SummaryPaidClaimsTreatyCode = null,
            string SummaryPaidClaimsPolicyNumber = null,
            string SummaryPaidClaimsCedingCompany = null,
            string SummaryPaidClaimsInsuredName = null,
            string SummaryPaidClaimsInsuredDateOfBirth = null,
            string SummaryPaidClaimsLastTransaction = null,
            string SummaryPaidClaimsDateOfReported = null,
            string SummaryPaidClaimsCedantDateOfNotification = null,
            string SummaryPaidClaimsDateOfRegister = null,
            string SummaryPaidClaimsDateOfCommencement = null,
            string SummaryPaidClaimsDateOfEvent = null,
            int? SummaryPaidClaimsPolicyDuration = null,
            string SummaryPaidClaimsTargetDateToIssueInvoice = null,
            string SummaryPaidClaimsSumReinsured = null,
            string SummaryPaidClaimsCauseOfEvent = null,
            int? SummaryPaidClaimsPicClaims = null,
            int? SummaryPaidClaimsPicDaa = null,
            int? SummaryPaidClaimsStatus = null,
            int? SummaryPaidClaimsProvisionStatus = null,
            int? SummaryPaidClaimsOffsetStatus = null
            )
        {
            DateTime? insuredDateOfBirth = Util.GetParseDateTime(SummaryPaidClaimsInsuredDateOfBirth);
            DateTime? lastTransaction = Util.GetParseDateTime(SummaryPaidClaimsLastTransaction);
            DateTime? dateOfReported = Util.GetParseDateTime(SummaryPaidClaimsDateOfReported);
            DateTime? cedantDateOfNotification = Util.GetParseDateTime(SummaryPaidClaimsCedantDateOfNotification);
            DateTime? dateOfRegister = Util.GetParseDateTime(SummaryPaidClaimsDateOfRegister);
            DateTime? dateOfCommencement = Util.GetParseDateTime(SummaryPaidClaimsDateOfCommencement);
            DateTime? dateOfEvent = Util.GetParseDateTime(SummaryPaidClaimsDateOfEvent);
            DateTime? targetDateToIssueInvoice = Util.GetParseDateTime(SummaryPaidClaimsTargetDateToIssueInvoice);

            double? sumReinsured = Util.StringToDouble(SummaryPaidClaimsSumReinsured);

            ViewBag.SummaryPaidClaimsRouteValue = new RouteValueDictionary
            {
                ["SummaryPaidClaimsHasRedFlag"] = SummaryPaidClaimsHasRedFlag,
                ["SummaryPaidClaimsEntryNo"] = SummaryPaidClaimsEntryNo,
                ["SummaryPaidClaimsSoaQuarter"] = SummaryPaidClaimsSoaQuarter,
                ["SummaryPaidClaimsClaimId"] = SummaryPaidClaimsClaimId,
                ["SummaryPaidClaimsClaimTransactionTypeId"] = SummaryPaidClaimsClaimTransactionType,
                ["SummaryPaidClaimsReferralCaseIndicator"] = SummaryPaidClaimsIsReferralCase,
                ["SummaryPaidClaimsRiDataId"] = SummaryPaidClaimsRiDataId,
                ["SummaryPaidClaimsRecordType"] = SummaryPaidClaimsRecordType,
                ["SummaryPaidClaimsTreatyCode"] = SummaryPaidClaimsTreatyCode,
                ["SummaryPaidClaimsPolicyNumber"] = SummaryPaidClaimsPolicyNumber,
                ["SummaryPaidClaimsCedingCompany"] = SummaryPaidClaimsCedingCompany,
                ["SummaryPaidClaimsInsuredName"] = SummaryPaidClaimsInsuredName,
                ["SummaryPaidClaimsInsuredDateOfBirth"] = insuredDateOfBirth.HasValue ? SummaryPaidClaimsInsuredDateOfBirth : null,
                ["SummaryPaidClaimsLastTransaction"] = lastTransaction.HasValue ? SummaryPaidClaimsLastTransaction : null,
                ["SummaryPaidClaimsDateOfReported"] = dateOfReported.HasValue ? SummaryPaidClaimsDateOfReported : null,
                ["SummaryPaidClaimsCedantDateOfNotification"] = cedantDateOfNotification.HasValue ? SummaryPaidClaimsCedantDateOfNotification : null,
                ["SummaryPaidClaimsDateOfRegister"] = dateOfRegister.HasValue ? SummaryPaidClaimsDateOfRegister : null,
                ["SummaryPaidClaimsDateOfCommencement"] = dateOfCommencement.HasValue ? SummaryPaidClaimsDateOfCommencement : null,
                ["SummaryPaidClaimsDateOfEvent"] = dateOfEvent.HasValue ? SummaryPaidClaimsDateOfEvent : null,
                ["SummaryPaidClaimsPolicyDuration"] = SummaryPaidClaimsPolicyDuration,
                ["SummaryPaidClaimsTargetDateToIssueInvoice"] = targetDateToIssueInvoice.HasValue ? SummaryPaidClaimsTargetDateToIssueInvoice : null,
                ["SummaryPaidClaimsSumReinsured"] = sumReinsured.HasValue ? SummaryPaidClaimsSumReinsured : null,
                ["SummaryPaidClaimsCauseOfEvent"] = SummaryPaidClaimsCauseOfEvent,
                ["SummaryPaidClaimsPicClaims"] = SummaryPaidClaimsPicClaims,
                ["SummaryPaidClaimsPicDaa"] = SummaryPaidClaimsPicDaa,
                ["SummaryPaidClaimsStatus"] = SummaryPaidClaimsStatus,
                ["SummaryPaidClaimsProvisionStatus"] = SummaryPaidClaimsProvisionStatus,
                ["SummaryPaidClaimsOffsetStatus"] = SummaryPaidClaimsOffsetStatus
            };

            _db.Database.CommandTimeout = 0;

            var query = _db.PerLifeClaimRetroData.Where(q => q.PerLifeClaimData.PerLifeClaimId == id && q.ClaimCategory == PerLifeClaimDataBo.ClaimCategoryPaidClaim).Select(PerLifeClaimViewModel.ClaimRetroDataExpression());

            if (SummaryPaidClaimsHasRedFlag.HasValue) query = query.Where(q => q.ClaimRegisterHistory.HasRedFlag == SummaryPaidClaimsHasRedFlag);
            if (!string.IsNullOrEmpty(SummaryPaidClaimsEntryNo)) query = query.Where(q => q.ClaimRegisterHistory.EntryNo.Contains(SummaryPaidClaimsEntryNo));
            if (!string.IsNullOrEmpty(SummaryPaidClaimsSoaQuarter)) query = query.Where(q => q.ClaimRegisterHistory.SoaQuarter.Contains(SummaryPaidClaimsSoaQuarter));
            if (!string.IsNullOrEmpty(SummaryPaidClaimsClaimId)) query = query.Where(q => q.ClaimRegisterHistory.ClaimId.Contains(SummaryPaidClaimsClaimId));
            if (!string.IsNullOrEmpty(SummaryPaidClaimsClaimTransactionType)) query = query.Where(q => q.ClaimRegisterHistory.ClaimTransactionType.Contains(SummaryPaidClaimsClaimTransactionType));
            if (SummaryPaidClaimsIsReferralCase.HasValue) query = query.Where(q => q.ClaimRegisterHistory.IsReferralCase == SummaryPaidClaimsIsReferralCase);
            if (SummaryPaidClaimsRiDataId.HasValue) query = query.Where(q => q.ClaimRegisterHistory.RiDataId == SummaryPaidClaimsRiDataId);
            if (!string.IsNullOrEmpty(SummaryPaidClaimsRecordType)) query = query.Where(q => q.ClaimRegisterHistory.RecordType.Contains(SummaryPaidClaimsRecordType));
            if (!string.IsNullOrEmpty(SummaryPaidClaimsTreatyCode)) query = query.Where(q => q.ClaimRegisterHistory.TreatyCode.Contains(SummaryPaidClaimsTreatyCode));
            if (!string.IsNullOrEmpty(SummaryPaidClaimsPolicyNumber)) query = query.Where(q => q.ClaimRegisterHistory.PolicyNumber.Contains(SummaryPaidClaimsPolicyNumber));
            if (!string.IsNullOrEmpty(SummaryPaidClaimsCedingCompany)) query = query.Where(q => q.ClaimRegisterHistory.CedingCompany.Contains(SummaryPaidClaimsCedingCompany));
            if (!string.IsNullOrEmpty(SummaryPaidClaimsInsuredName)) query = query.Where(q => q.ClaimRegisterHistory.InsuredName.Contains(SummaryPaidClaimsInsuredName));
            if (insuredDateOfBirth.HasValue) query = query.Where(q => q.ClaimRegisterHistory.InsuredDateOfBirth == insuredDateOfBirth);
            if (lastTransaction.HasValue) query = query.Where(q => q.ClaimRegisterHistory.LastTransactionDate == lastTransaction);
            if (dateOfReported.HasValue) query = query.Where(q => q.ClaimRegisterHistory.DateOfReported == dateOfReported);
            if (cedantDateOfNotification.HasValue) query = query.Where(q => q.ClaimRegisterHistory.CedantDateOfNotification == cedantDateOfNotification);
            if (dateOfRegister.HasValue) query = query.Where(q => q.ClaimRegisterHistory.DateOfRegister == dateOfRegister);
            if (dateOfCommencement.HasValue) query = query.Where(q => q.ClaimRegisterHistory.DateOfIntimation == dateOfCommencement);
            if (dateOfEvent.HasValue) query = query.Where(q => q.ClaimRegisterHistory.DateOfEvent == dateOfEvent);
            if (SummaryPaidClaimsPolicyDuration.HasValue) query = query.Where(q => q.ClaimRegisterHistory.PolicyDuration == SummaryPaidClaimsPolicyDuration);
            if (targetDateToIssueInvoice.HasValue) query = query.Where(q => q.ClaimRegisterHistory.TargetDateToIssueInvoice == targetDateToIssueInvoice);
            if (sumReinsured.HasValue) query = query.Where(q => q.ClaimRegisterHistory.SumIns == sumReinsured);
            if (!string.IsNullOrEmpty(SummaryPaidClaimsCauseOfEvent)) query = query.Where(q => q.ClaimRegisterHistory.CauseOfEvent.Contains(SummaryPaidClaimsCauseOfEvent));
            if (SummaryPaidClaimsPicClaims.HasValue) query = query.Where(q => q.ClaimRegisterHistory.ClaimRegister.PicClaimId == SummaryPaidClaimsPicClaims);
            if (SummaryPaidClaimsPicDaa.HasValue) query = query.Where(q => q.ClaimRegisterHistory.ClaimRegister.PicDaaId == SummaryPaidClaimsPicDaa);
            if (SummaryPaidClaimsProvisionStatus.HasValue) query = query.Where(q => q.ClaimRegisterHistory.ProvisionStatus == SummaryPaidClaimsProvisionStatus);
            if (SummaryPaidClaimsOffsetStatus.HasValue) query = query.Where(q => q.ClaimRegisterHistory.OffsetStatus == SummaryPaidClaimsOffsetStatus);
            if (SummaryPaidClaimsStatus.HasValue) query = query.Where(q => q.ClaimRegisterHistory.ClaimStatus == SummaryPaidClaimsStatus);
            if (SummaryPaidClaimsProvisionStatus.HasValue) query = query.Where(q => q.ClaimRegisterHistory.ProvisionStatus == SummaryPaidClaimsProvisionStatus);
            if (SummaryPaidClaimsOffsetStatus.HasValue) query = query.Where(q => q.ClaimRegisterHistory.OffsetStatus == SummaryPaidClaimsOffsetStatus);

            query = query.OrderBy(q => q.ClaimRegisterHistory.Id);

            ViewBag.SummaryPaidClaimsTotal = query.Count();
            ViewBag.SummaryPaidClaimsList = query.ToPagedList(Page ?? 1, PageSize);

        }

        public void ListEmptySummary()
        {
            ViewBag.SummaryPaidClaimsRouteValue = new RouteValueDictionary { };
            ViewBag.SummaryPaidClaimsListTotal = 0;
            ViewBag.SummaryPaidClaimsList = new List<PerLifeClaimViewModel>().ToPagedList(1, PageSize);
        }
        #endregion

        #region Summary Pending Claims tab
        public ActionResult SummaryPendingClaims(
            int id,
            bool? SummaryPendingClaimsHasRedFlag,
            string SummaryPendingClaimsEntryNo,
            string SummaryPendingClaimsSoaQuarter,
            string SummaryPendingClaimsClaimId,
            string SummaryPendingClaimsClaimTransactionType,
            bool? SummaryPendingClaimsIsReferralCase,
            int? SummaryPendingClaimsRiDataId,
            string SummaryPendingClaimsRecordType,
            string SummaryPendingClaimsTreatyCode,
            string SummaryPendingClaimsPolicyNumber,
            string SummaryPendingClaimsCedingCompany,
            string SummaryPendingClaimsInsuredName,
            string SummaryPendingClaimsInsuredDateOfBirth,
            string SummaryPendingClaimsLastTransaction,
            string SummaryPendingClaimsDateOfReported,
            string SummaryPendingClaimsCedantDateOfNotification,
            string SummaryPendingClaimsDateOfRegister,
            string SummaryPendingClaimsDateOfCommencement,
            string SummaryPendingClaimsDateOfEvent,
            int? SummaryPendingClaimsPolicyDuration,
            string SummaryPendingClaimsTargetDateToIssueInvoice,
            string SummaryPendingClaimsSumReinsured,
            string SummaryPendingClaimsCauseOfEvent,
            int? SummaryPendingClaimsPicClaims,
            int? SummaryPendingClaimsPicDaa,
            int? SummaryPendingClaimsStatus,
            int? SummaryPendingClaimsProvisionStatus,
            int? SummaryPendingClaimsOffsetStatus,
            int? Page)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = PerLifeClaimService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index", "PerLifeClaim");
            }

            var model = new PerLifeClaimViewModel(bo);

            ListEmptyClaimRegisterData();

            ListEmptyException();

            ListEmptyClaimRetroData();

            ListEmptySummary();

            ListSummaryPendingClaims(
                id,
                Page,
                SummaryPendingClaimsHasRedFlag,
                SummaryPendingClaimsEntryNo,
                SummaryPendingClaimsSoaQuarter,
                SummaryPendingClaimsClaimId,
                SummaryPendingClaimsClaimTransactionType,
                SummaryPendingClaimsIsReferralCase,
                SummaryPendingClaimsRiDataId,
                SummaryPendingClaimsRecordType,
                SummaryPendingClaimsTreatyCode,
                SummaryPendingClaimsPolicyNumber,
                SummaryPendingClaimsCedingCompany,
                SummaryPendingClaimsInsuredName,
                SummaryPendingClaimsInsuredDateOfBirth,
                SummaryPendingClaimsLastTransaction,
                SummaryPendingClaimsDateOfReported,
                SummaryPendingClaimsCedantDateOfNotification,
                SummaryPendingClaimsDateOfRegister,
                SummaryPendingClaimsDateOfCommencement,
                SummaryPendingClaimsDateOfEvent,
                SummaryPendingClaimsPolicyDuration,
                SummaryPendingClaimsTargetDateToIssueInvoice,
                SummaryPendingClaimsSumReinsured,
                SummaryPendingClaimsCauseOfEvent,
                SummaryPendingClaimsPicClaims,
                SummaryPendingClaimsPicDaa,
                SummaryPendingClaimsStatus,
                SummaryPendingClaimsProvisionStatus,
                SummaryPendingClaimsOffsetStatus
                );

            ListEmptySummaryClaimsRemoved();

            model.ActiveTab = PerLifeClaimBo.ActiveTabSummaryPendingClaims;
            LoadPage(model);
            return View("Edit", model);

        }

        public void ListSummaryPendingClaims(
            int id,
            int? Page,
            bool? SummaryPendingClaimsHasRedFlag = null,
            string SummaryPendingClaimsEntryNo = null,
            string SummaryPendingClaimsSoaQuarter = null,
            string SummaryPendingClaimsClaimId = null,
            string SummaryPendingClaimsClaimTransactionType = null,
            bool? SummaryPendingClaimsIsReferralCase = null,
            int? SummaryPendingClaimsRiDataId = null,
            string SummaryPendingClaimsRecordType = null,
            string SummaryPendingClaimsTreatyCode = null,
            string SummaryPendingClaimsPolicyNumber = null,
            string SummaryPendingClaimsCedingCompany = null,
            string SummaryPendingClaimsInsuredName = null,
            string SummaryPendingClaimsInsuredDateOfBirth = null,
            string SummaryPendingClaimsLastTransaction = null,
            string SummaryPendingClaimsDateOfReported = null,
            string SummaryPendingClaimsCedantDateOfNotification = null,
            string SummaryPendingClaimsDateOfRegister = null,
            string SummaryPendingClaimsDateOfCommencement = null,
            string SummaryPendingClaimsDateOfEvent = null,
            int? SummaryPendingClaimsPolicyDuration = null,
            string SummaryPendingClaimsTargetDateToIssueInvoice = null,
            string SummaryPendingClaimsSumReinsured = null,
            string SummaryPendingClaimsCauseOfEvent = null,
            int? SummaryPendingClaimsPicClaims = null,
            int? SummaryPendingClaimsPicDaa = null,
            int? SummaryPendingClaimsStatus = null,
            int? SummaryPendingClaimsProvisionStatus = null,
            int? SummaryPendingClaimsOffsetStatus = null
            )
        {
            DateTime? insuredDateOfBirth = Util.GetParseDateTime(SummaryPendingClaimsInsuredDateOfBirth);
            DateTime? lastTransaction = Util.GetParseDateTime(SummaryPendingClaimsLastTransaction);
            DateTime? dateOfReported = Util.GetParseDateTime(SummaryPendingClaimsDateOfReported);
            DateTime? cedantDateOfNotification = Util.GetParseDateTime(SummaryPendingClaimsCedantDateOfNotification);
            DateTime? dateOfRegister = Util.GetParseDateTime(SummaryPendingClaimsDateOfRegister);
            DateTime? dateOfCommencement = Util.GetParseDateTime(SummaryPendingClaimsDateOfCommencement);
            DateTime? dateOfEvent = Util.GetParseDateTime(SummaryPendingClaimsDateOfEvent);
            DateTime? targetDateToIssueInvoice = Util.GetParseDateTime(SummaryPendingClaimsTargetDateToIssueInvoice);

            double? sumReinsured = Util.StringToDouble(SummaryPendingClaimsSumReinsured);

            ViewBag.SummaryPendingClaimsRouteValue = new RouteValueDictionary
            {
                ["SummaryPendingClaimsHasRedFlag"] = SummaryPendingClaimsHasRedFlag,
                ["SummaryPendingClaimsEntryNo"] = SummaryPendingClaimsEntryNo,
                ["SummaryPendingClaimsSoaQuarter"] = SummaryPendingClaimsSoaQuarter,
                ["SummaryPendingClaimsClaimId"] = SummaryPendingClaimsClaimId,
                ["SummaryPendingClaimsClaimTransactionTypeId"] = SummaryPendingClaimsClaimTransactionType,
                ["SummaryPendingClaimsReferralCaseIndicator"] = SummaryPendingClaimsIsReferralCase,
                ["SummaryPendingClaimsRiDataId"] = SummaryPendingClaimsRiDataId,
                ["SummaryPendingClaimsRecordType"] = SummaryPendingClaimsRecordType,
                ["SummaryPendingClaimsTreatyCode"] = SummaryPendingClaimsTreatyCode,
                ["SummaryPendingClaimsPolicyNumber"] = SummaryPendingClaimsPolicyNumber,
                ["SummaryPendingClaimsCedingCompany"] = SummaryPendingClaimsCedingCompany,
                ["SummaryPendingClaimsInsuredName"] = SummaryPendingClaimsInsuredName,
                ["SummaryPendingClaimsInsuredDateOfBirth"] = insuredDateOfBirth.HasValue ? SummaryPendingClaimsInsuredDateOfBirth : null,
                ["SummaryPendingClaimsLastTransaction"] = lastTransaction.HasValue ? SummaryPendingClaimsLastTransaction : null,
                ["SummaryPendingClaimsDateOfReported"] = dateOfReported.HasValue ? SummaryPendingClaimsDateOfReported : null,
                ["SummaryPendingClaimsCedantDateOfNotification"] = cedantDateOfNotification.HasValue ? SummaryPendingClaimsCedantDateOfNotification : null,
                ["SummaryPendingClaimsDateOfRegister"] = dateOfRegister.HasValue ? SummaryPendingClaimsDateOfRegister : null,
                ["SummaryPendingClaimsDateOfCommencement"] = dateOfCommencement.HasValue ? SummaryPendingClaimsDateOfCommencement : null,
                ["SummaryPendingClaimsDateOfEvent"] = dateOfEvent.HasValue ? SummaryPendingClaimsDateOfEvent : null,
                ["SummaryPendingClaimsPolicyDuration"] = SummaryPendingClaimsPolicyDuration,
                ["SummaryPendingClaimsTargetDateToIssueInvoice"] = targetDateToIssueInvoice.HasValue ? SummaryPendingClaimsTargetDateToIssueInvoice : null,
                ["SummaryPendingClaimsSumReinsured"] = sumReinsured.HasValue ? SummaryPendingClaimsSumReinsured : null,
                ["SummaryPendingClaimsCauseOfEvent"] = SummaryPendingClaimsCauseOfEvent,
                ["SummaryPendingClaimsPicClaims"] = SummaryPendingClaimsPicClaims,
                ["SummaryPendingClaimsPicDaa"] = SummaryPendingClaimsPicDaa,
                ["SummaryPendingClaimsStatus"] = SummaryPendingClaimsStatus,
                ["SummaryPendingClaimsProvisionStatus"] = SummaryPendingClaimsProvisionStatus,
                ["SummaryPendingClaimsOffsetStatus"] = SummaryPendingClaimsOffsetStatus
            };

            _db.Database.CommandTimeout = 0;

            var query = _db.PerLifeClaimRetroData.Where(q => q.PerLifeClaimData.PerLifeClaimId == id && q.ClaimCategory == PerLifeClaimDataBo.ClaimCategoryPendingClaim).Select(PerLifeClaimViewModel.ClaimRetroDataExpression());

            if (SummaryPendingClaimsHasRedFlag.HasValue) query = query.Where(q => q.ClaimRegisterHistory.HasRedFlag == SummaryPendingClaimsHasRedFlag);
            if (!string.IsNullOrEmpty(SummaryPendingClaimsEntryNo)) query = query.Where(q => q.ClaimRegisterHistory.EntryNo.Contains(SummaryPendingClaimsEntryNo));
            if (!string.IsNullOrEmpty(SummaryPendingClaimsSoaQuarter)) query = query.Where(q => q.ClaimRegisterHistory.SoaQuarter.Contains(SummaryPendingClaimsSoaQuarter));
            if (!string.IsNullOrEmpty(SummaryPendingClaimsClaimId)) query = query.Where(q => q.ClaimRegisterHistory.ClaimId.Contains(SummaryPendingClaimsClaimId));
            if (!string.IsNullOrEmpty(SummaryPendingClaimsClaimTransactionType)) query = query.Where(q => q.ClaimRegisterHistory.ClaimTransactionType.Contains(SummaryPendingClaimsClaimTransactionType));
            if (SummaryPendingClaimsIsReferralCase.HasValue) query = query.Where(q => q.ClaimRegisterHistory.IsReferralCase == SummaryPendingClaimsIsReferralCase);
            if (SummaryPendingClaimsRiDataId.HasValue) query = query.Where(q => q.ClaimRegisterHistory.RiDataId == SummaryPendingClaimsRiDataId);
            if (!string.IsNullOrEmpty(SummaryPendingClaimsRecordType)) query = query.Where(q => q.ClaimRegisterHistory.RecordType.Contains(SummaryPendingClaimsRecordType));
            if (!string.IsNullOrEmpty(SummaryPendingClaimsTreatyCode)) query = query.Where(q => q.ClaimRegisterHistory.TreatyCode.Contains(SummaryPendingClaimsTreatyCode));
            if (!string.IsNullOrEmpty(SummaryPendingClaimsPolicyNumber)) query = query.Where(q => q.ClaimRegisterHistory.PolicyNumber.Contains(SummaryPendingClaimsPolicyNumber));
            if (!string.IsNullOrEmpty(SummaryPendingClaimsCedingCompany)) query = query.Where(q => q.ClaimRegisterHistory.CedingCompany.Contains(SummaryPendingClaimsCedingCompany));
            if (!string.IsNullOrEmpty(SummaryPendingClaimsInsuredName)) query = query.Where(q => q.ClaimRegisterHistory.InsuredName.Contains(SummaryPendingClaimsInsuredName));
            if (insuredDateOfBirth.HasValue) query = query.Where(q => q.ClaimRegisterHistory.InsuredDateOfBirth == insuredDateOfBirth);
            if (lastTransaction.HasValue) query = query.Where(q => q.ClaimRegisterHistory.LastTransactionDate == lastTransaction);
            if (dateOfReported.HasValue) query = query.Where(q => q.ClaimRegisterHistory.DateOfReported == dateOfReported);
            if (cedantDateOfNotification.HasValue) query = query.Where(q => q.ClaimRegisterHistory.CedantDateOfNotification == cedantDateOfNotification);
            if (dateOfRegister.HasValue) query = query.Where(q => q.ClaimRegisterHistory.DateOfRegister == dateOfRegister);
            if (dateOfCommencement.HasValue) query = query.Where(q => q.ClaimRegisterHistory.DateOfIntimation == dateOfCommencement);
            if (dateOfEvent.HasValue) query = query.Where(q => q.ClaimRegisterHistory.DateOfEvent == dateOfEvent);
            if (SummaryPendingClaimsPolicyDuration.HasValue) query = query.Where(q => q.ClaimRegisterHistory.PolicyDuration == SummaryPendingClaimsPolicyDuration);
            if (targetDateToIssueInvoice.HasValue) query = query.Where(q => q.ClaimRegisterHistory.TargetDateToIssueInvoice == targetDateToIssueInvoice);
            if (sumReinsured.HasValue) query = query.Where(q => q.ClaimRegisterHistory.SumIns == sumReinsured);
            if (!string.IsNullOrEmpty(SummaryPendingClaimsCauseOfEvent)) query = query.Where(q => q.ClaimRegisterHistory.CauseOfEvent.Contains(SummaryPendingClaimsCauseOfEvent));
            if (SummaryPendingClaimsPicClaims.HasValue) query = query.Where(q => q.ClaimRegisterHistory.ClaimRegister.PicClaimId == SummaryPendingClaimsPicClaims);
            if (SummaryPendingClaimsPicDaa.HasValue) query = query.Where(q => q.ClaimRegisterHistory.ClaimRegister.PicDaaId == SummaryPendingClaimsPicDaa);
            if (SummaryPendingClaimsProvisionStatus.HasValue) query = query.Where(q => q.ClaimRegisterHistory.ProvisionStatus == SummaryPendingClaimsProvisionStatus);
            if (SummaryPendingClaimsOffsetStatus.HasValue) query = query.Where(q => q.ClaimRegisterHistory.OffsetStatus == SummaryPendingClaimsOffsetStatus);
            if (SummaryPendingClaimsStatus.HasValue) query = query.Where(q => q.ClaimRegisterHistory.ClaimStatus == SummaryPendingClaimsStatus);
            if (SummaryPendingClaimsProvisionStatus.HasValue) query = query.Where(q => q.ClaimRegisterHistory.ProvisionStatus == SummaryPendingClaimsProvisionStatus);
            if (SummaryPendingClaimsOffsetStatus.HasValue) query = query.Where(q => q.ClaimRegisterHistory.OffsetStatus == SummaryPendingClaimsOffsetStatus);

            query = query.OrderBy(q => q.ClaimRegisterHistory.Id);

            ViewBag.SummaryPendingClaimsTotal = query.Count();
            ViewBag.SummaryPendingClaimsList = query.ToPagedList(Page ?? 1, PageSize);

        }

        public void ListEmptySummaryPendingClaims()
        {
            ViewBag.SummaryPendingClaimsRouteValue = new RouteValueDictionary { };
            ViewBag.SummaryPendingClaimsListTotal = 0;
            ViewBag.SummaryPendingClaimsList = new List<PerLifeClaimViewModel>().ToPagedList(1, PageSize);
        }
        #endregion

        #region Summary Claims Removed tab
        public ActionResult SummaryClaimsRemoved(
            int id,
            bool? SummaryClaimsRemovedHasRedFlag,
            string SummaryClaimsRemovedEntryNo,
            string SummaryClaimsRemovedSoaQuarter,
            string SummaryClaimsRemovedClaimId,
            string SummaryClaimsRemovedClaimTransactionType,
            bool? SummaryClaimsRemovedIsReferralCase,
            int? SummaryClaimsRemovedRiDataId,
            string SummaryClaimsRemovedRecordType,
            string SummaryClaimsRemovedTreatyCode,
            string SummaryClaimsRemovedPolicyNumber,
            string SummaryClaimsRemovedCedingCompany,
            string SummaryClaimsRemovedInsuredName,
            string SummaryClaimsRemovedInsuredDateOfBirth,
            string SummaryClaimsRemovedLastTransaction,
            string SummaryClaimsRemovedDateOfReported,
            string SummaryClaimsRemovedCedantDateOfNotification,
            string SummaryClaimsRemovedDateOfRegister,
            string SummaryClaimsRemovedDateOfCommencement,
            string SummaryClaimsRemovedDateOfEvent,
            int? SummaryClaimsRemovedPolicyDuration,
            string SummaryClaimsRemovedTargetDateToIssueInvoice,
            string SummaryClaimsRemovedSumReinsured,
            string SummaryClaimsRemovedCauseOfEvent,
            int? SummaryClaimsRemovedPicClaims,
            int? SummaryClaimsRemovedPicDaa,
            int? SummaryClaimsRemovedStatus,
            int? SummaryClaimsRemovedProvisionStatus,
            int? SummaryClaimsRemovedOffsetStatus,
            int? Page)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = PerLifeClaimService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index", "PerLifeClaim");
            }

            var model = new PerLifeClaimViewModel(bo);

            ListEmptyClaimRegisterData();

            ListEmptyException();

            ListEmptyClaimRetroData();

            ListEmptySummary();

            ListEmptySummaryPendingClaims();

            ListSummaryClaimsRemoved(
                id,
                Page,
                SummaryClaimsRemovedHasRedFlag,
                SummaryClaimsRemovedEntryNo,
                SummaryClaimsRemovedSoaQuarter,
                SummaryClaimsRemovedClaimId,
                SummaryClaimsRemovedClaimTransactionType,
                SummaryClaimsRemovedIsReferralCase,
                SummaryClaimsRemovedRiDataId,
                SummaryClaimsRemovedRecordType,
                SummaryClaimsRemovedTreatyCode,
                SummaryClaimsRemovedPolicyNumber,
                SummaryClaimsRemovedCedingCompany,
                SummaryClaimsRemovedInsuredName,
                SummaryClaimsRemovedInsuredDateOfBirth,
                SummaryClaimsRemovedLastTransaction,
                SummaryClaimsRemovedDateOfReported,
                SummaryClaimsRemovedCedantDateOfNotification,
                SummaryClaimsRemovedDateOfRegister,
                SummaryClaimsRemovedDateOfCommencement,
                SummaryClaimsRemovedDateOfEvent,
                SummaryClaimsRemovedPolicyDuration,
                SummaryClaimsRemovedTargetDateToIssueInvoice,
                SummaryClaimsRemovedSumReinsured,
                SummaryClaimsRemovedCauseOfEvent,
                SummaryClaimsRemovedPicClaims,
                SummaryClaimsRemovedPicDaa,
                SummaryClaimsRemovedStatus,
                SummaryClaimsRemovedProvisionStatus,
                SummaryClaimsRemovedOffsetStatus
                );

            model.ActiveTab = PerLifeClaimBo.ActiveTabSummaryClaimsRemoved;
            LoadPage(model);
            return View("Edit", model);

        }

        public void ListSummaryClaimsRemoved(
            int id,
            int? Page,
            bool? SummaryClaimsRemovedHasRedFlag = null,
            string SummaryClaimsRemovedEntryNo = null,
            string SummaryClaimsRemovedSoaQuarter = null,
            string SummaryClaimsRemovedClaimId = null,
            string SummaryClaimsRemovedClaimTransactionType = null,
            bool? SummaryClaimsRemovedIsReferralCase = null,
            int? SummaryClaimsRemovedRiDataId = null,
            string SummaryClaimsRemovedRecordType = null,
            string SummaryClaimsRemovedTreatyCode = null,
            string SummaryClaimsRemovedPolicyNumber = null,
            string SummaryClaimsRemovedCedingCompany = null,
            string SummaryClaimsRemovedInsuredName = null,
            string SummaryClaimsRemovedInsuredDateOfBirth = null,
            string SummaryClaimsRemovedLastTransaction = null,
            string SummaryClaimsRemovedDateOfReported = null,
            string SummaryClaimsRemovedCedantDateOfNotification = null,
            string SummaryClaimsRemovedDateOfRegister = null,
            string SummaryClaimsRemovedDateOfCommencement = null,
            string SummaryClaimsRemovedDateOfEvent = null,
            int? SummaryClaimsRemovedPolicyDuration = null,
            string SummaryClaimsRemovedTargetDateToIssueInvoice = null,
            string SummaryClaimsRemovedSumReinsured = null,
            string SummaryClaimsRemovedCauseOfEvent = null,
            int? SummaryClaimsRemovedPicClaims = null,
            int? SummaryClaimsRemovedPicDaa = null,
            int? SummaryClaimsRemovedStatus = null,
            int? SummaryClaimsRemovedProvisionStatus = null,
            int? SummaryClaimsRemovedOffsetStatus = null
            )
        {
            DateTime? insuredDateOfBirth = Util.GetParseDateTime(SummaryClaimsRemovedInsuredDateOfBirth);
            DateTime? lastTransaction = Util.GetParseDateTime(SummaryClaimsRemovedLastTransaction);
            DateTime? dateOfReported = Util.GetParseDateTime(SummaryClaimsRemovedDateOfReported);
            DateTime? cedantDateOfNotification = Util.GetParseDateTime(SummaryClaimsRemovedCedantDateOfNotification);
            DateTime? dateOfRegister = Util.GetParseDateTime(SummaryClaimsRemovedDateOfRegister);
            DateTime? dateOfCommencement = Util.GetParseDateTime(SummaryClaimsRemovedDateOfCommencement);
            DateTime? dateOfEvent = Util.GetParseDateTime(SummaryClaimsRemovedDateOfEvent);
            DateTime? targetDateToIssueInvoice = Util.GetParseDateTime(SummaryClaimsRemovedTargetDateToIssueInvoice);

            double? sumReinsured = Util.StringToDouble(SummaryClaimsRemovedSumReinsured);

            ViewBag.SummaryClaimsRemovedRouteValue = new RouteValueDictionary
            {
                ["SummaryClaimsRemovedHasRedFlag"] = SummaryClaimsRemovedHasRedFlag,
                ["SummaryClaimsRemovedEntryNo"] = SummaryClaimsRemovedEntryNo,
                ["SummaryClaimsRemovedSoaQuarter"] = SummaryClaimsRemovedSoaQuarter,
                ["SummaryClaimsRemovedClaimId"] = SummaryClaimsRemovedClaimId,
                ["SummaryClaimsRemovedClaimTransactionTypeId"] = SummaryClaimsRemovedClaimTransactionType,
                ["SummaryClaimsRemovedReferralCaseIndicator"] = SummaryClaimsRemovedIsReferralCase,
                ["SummaryClaimsRemovedRiDataId"] = SummaryClaimsRemovedRiDataId,
                ["SummaryClaimsRemovedRecordType"] = SummaryClaimsRemovedRecordType,
                ["SummaryClaimsRemovedTreatyCode"] = SummaryClaimsRemovedTreatyCode,
                ["SummaryClaimsRemovedPolicyNumber"] = SummaryClaimsRemovedPolicyNumber,
                ["SummaryClaimsRemovedCedingCompany"] = SummaryClaimsRemovedCedingCompany,
                ["SummaryClaimsRemovedInsuredName"] = SummaryClaimsRemovedInsuredName,
                ["SummaryClaimsRemovedInsuredDateOfBirth"] = insuredDateOfBirth.HasValue ? SummaryClaimsRemovedInsuredDateOfBirth : null,
                ["SummaryClaimsRemovedLastTransaction"] = lastTransaction.HasValue ? SummaryClaimsRemovedLastTransaction : null,
                ["SummaryClaimsRemovedDateOfReported"] = dateOfReported.HasValue ? SummaryClaimsRemovedDateOfReported : null,
                ["SummaryClaimsRemovedCedantDateOfNotification"] = cedantDateOfNotification.HasValue ? SummaryClaimsRemovedCedantDateOfNotification : null,
                ["SummaryClaimsRemovedDateOfRegister"] = dateOfRegister.HasValue ? SummaryClaimsRemovedDateOfRegister : null,
                ["SummaryClaimsRemovedDateOfCommencement"] = dateOfCommencement.HasValue ? SummaryClaimsRemovedDateOfCommencement : null,
                ["SummaryClaimsRemovedDateOfEvent"] = dateOfEvent.HasValue ? SummaryClaimsRemovedDateOfEvent : null,
                ["SummaryClaimsRemovedPolicyDuration"] = SummaryClaimsRemovedPolicyDuration,
                ["SummaryClaimsRemovedTargetDateToIssueInvoice"] = targetDateToIssueInvoice.HasValue ? SummaryClaimsRemovedTargetDateToIssueInvoice : null,
                ["SummaryClaimsRemovedSumReinsured"] = sumReinsured.HasValue ? SummaryClaimsRemovedSumReinsured : null,
                ["SummaryClaimsRemovedCauseOfEvent"] = SummaryClaimsRemovedCauseOfEvent,
                ["SummaryClaimsRemovedPicClaims"] = SummaryClaimsRemovedPicClaims,
                ["SummaryClaimsRemovedPicDaa"] = SummaryClaimsRemovedPicDaa,
                ["SummaryClaimsRemovedStatus"] = SummaryClaimsRemovedStatus,
                ["SummaryClaimsRemovedProvisionStatus"] = SummaryClaimsRemovedProvisionStatus,
                ["SummaryClaimsRemovedOffsetStatus"] = SummaryClaimsRemovedOffsetStatus
            };

            _db.Database.CommandTimeout = 0;

            var query = _db.PerLifeClaimRetroData.Where(q => q.PerLifeClaimData.PerLifeClaimId == id && (q.ClaimCategory == PerLifeClaimDataBo.ClaimCategoryReversed || q.ClaimCategory == PerLifeClaimDataBo.ClaimCategoryRetainClaim)).Select(PerLifeClaimViewModel.ClaimRetroDataExpression());

            if (SummaryClaimsRemovedHasRedFlag.HasValue) query = query.Where(q => q.ClaimRegisterHistory.HasRedFlag == SummaryClaimsRemovedHasRedFlag);
            if (!string.IsNullOrEmpty(SummaryClaimsRemovedEntryNo)) query = query.Where(q => q.ClaimRegisterHistory.EntryNo.Contains(SummaryClaimsRemovedEntryNo));
            if (!string.IsNullOrEmpty(SummaryClaimsRemovedSoaQuarter)) query = query.Where(q => q.ClaimRegisterHistory.SoaQuarter.Contains(SummaryClaimsRemovedSoaQuarter));
            if (!string.IsNullOrEmpty(SummaryClaimsRemovedClaimId)) query = query.Where(q => q.ClaimRegisterHistory.ClaimId.Contains(SummaryClaimsRemovedClaimId));
            if (!string.IsNullOrEmpty(SummaryClaimsRemovedClaimTransactionType)) query = query.Where(q => q.ClaimRegisterHistory.ClaimTransactionType.Contains(SummaryClaimsRemovedClaimTransactionType));
            if (SummaryClaimsRemovedIsReferralCase.HasValue) query = query.Where(q => q.ClaimRegisterHistory.IsReferralCase == SummaryClaimsRemovedIsReferralCase);
            if (SummaryClaimsRemovedRiDataId.HasValue) query = query.Where(q => q.ClaimRegisterHistory.RiDataId == SummaryClaimsRemovedRiDataId);
            if (!string.IsNullOrEmpty(SummaryClaimsRemovedRecordType)) query = query.Where(q => q.ClaimRegisterHistory.RecordType.Contains(SummaryClaimsRemovedRecordType));
            if (!string.IsNullOrEmpty(SummaryClaimsRemovedTreatyCode)) query = query.Where(q => q.ClaimRegisterHistory.TreatyCode.Contains(SummaryClaimsRemovedTreatyCode));
            if (!string.IsNullOrEmpty(SummaryClaimsRemovedPolicyNumber)) query = query.Where(q => q.ClaimRegisterHistory.PolicyNumber.Contains(SummaryClaimsRemovedPolicyNumber));
            if (!string.IsNullOrEmpty(SummaryClaimsRemovedCedingCompany)) query = query.Where(q => q.ClaimRegisterHistory.CedingCompany.Contains(SummaryClaimsRemovedCedingCompany));
            if (!string.IsNullOrEmpty(SummaryClaimsRemovedInsuredName)) query = query.Where(q => q.ClaimRegisterHistory.InsuredName.Contains(SummaryClaimsRemovedInsuredName));
            if (insuredDateOfBirth.HasValue) query = query.Where(q => q.ClaimRegisterHistory.InsuredDateOfBirth == insuredDateOfBirth);
            if (lastTransaction.HasValue) query = query.Where(q => q.ClaimRegisterHistory.LastTransactionDate == lastTransaction);
            if (dateOfReported.HasValue) query = query.Where(q => q.ClaimRegisterHistory.DateOfReported == dateOfReported);
            if (cedantDateOfNotification.HasValue) query = query.Where(q => q.ClaimRegisterHistory.CedantDateOfNotification == cedantDateOfNotification);
            if (dateOfRegister.HasValue) query = query.Where(q => q.ClaimRegisterHistory.DateOfRegister == dateOfRegister);
            if (dateOfCommencement.HasValue) query = query.Where(q => q.ClaimRegisterHistory.DateOfIntimation == dateOfCommencement);
            if (dateOfEvent.HasValue) query = query.Where(q => q.ClaimRegisterHistory.DateOfEvent == dateOfEvent);
            if (SummaryClaimsRemovedPolicyDuration.HasValue) query = query.Where(q => q.ClaimRegisterHistory.PolicyDuration == SummaryClaimsRemovedPolicyDuration);
            if (targetDateToIssueInvoice.HasValue) query = query.Where(q => q.ClaimRegisterHistory.TargetDateToIssueInvoice == targetDateToIssueInvoice);
            if (sumReinsured.HasValue) query = query.Where(q => q.ClaimRegisterHistory.SumIns == sumReinsured);
            if (!string.IsNullOrEmpty(SummaryClaimsRemovedCauseOfEvent)) query = query.Where(q => q.ClaimRegisterHistory.CauseOfEvent.Contains(SummaryClaimsRemovedCauseOfEvent));
            if (SummaryClaimsRemovedPicClaims.HasValue) query = query.Where(q => q.ClaimRegisterHistory.ClaimRegister.PicClaimId == SummaryClaimsRemovedPicClaims);
            if (SummaryClaimsRemovedPicDaa.HasValue) query = query.Where(q => q.ClaimRegisterHistory.ClaimRegister.PicDaaId == SummaryClaimsRemovedPicDaa);
            if (SummaryClaimsRemovedProvisionStatus.HasValue) query = query.Where(q => q.ClaimRegisterHistory.ProvisionStatus == SummaryClaimsRemovedProvisionStatus);
            if (SummaryClaimsRemovedOffsetStatus.HasValue) query = query.Where(q => q.ClaimRegisterHistory.OffsetStatus == SummaryClaimsRemovedOffsetStatus);
            if (SummaryClaimsRemovedStatus.HasValue) query = query.Where(q => q.ClaimRegisterHistory.ClaimStatus == SummaryClaimsRemovedStatus);
            if (SummaryClaimsRemovedProvisionStatus.HasValue) query = query.Where(q => q.ClaimRegisterHistory.ProvisionStatus == SummaryClaimsRemovedProvisionStatus);
            if (SummaryClaimsRemovedOffsetStatus.HasValue) query = query.Where(q => q.ClaimRegisterHistory.OffsetStatus == SummaryClaimsRemovedOffsetStatus);

            query = query.OrderBy(q => q.ClaimRegisterHistory.Id);

            ViewBag.SummaryClaimsRemovedTotal = query.Count();
            ViewBag.SummaryClaimsRemovedList = query.ToPagedList(Page ?? 1, PageSize);

        }

        public void ListEmptySummaryClaimsRemoved()
        {
            ViewBag.SummaryClaimsRemovedRouteValue = new RouteValueDictionary { };
            ViewBag.SummaryClaimsRemovedListTotal = 0;
            ViewBag.SummaryClaimsRemovedList = new List<PerLifeClaimViewModel>().ToPagedList(1, PageSize);
        }
        #endregion

        // Summary tab CSV download
        public ActionResult PaidClaimsDownload(
            string downloadToken,
            int type,
            int Id,
            bool? SummaryPaidClaimsHasRedFlag,
            string SummaryPaidClaimsEntryNo,
            string SummaryPaidClaimsSoaQuarter,
            string SummaryPaidClaimsClaimId,
            string SummaryPaidClaimsClaimTransactionType,
            bool? SummaryPaidClaimsIsReferralCase,
            int? SummaryPaidClaimsRiDataId,
            string SummaryPaidClaimsRecordType,
            string SummaryPaidClaimsTreatyCode,
            string SummaryPaidClaimsPolicyNumber,
            string SummaryPaidClaimsCedingCompany,
            string SummaryPaidClaimsInsuredName,
            string SummaryPaidClaimsInsuredDateOfBirth,
            string SummaryPaidClaimsLastTransaction,
            string SummaryPaidClaimsDateOfReported,
            string SummaryPaidClaimsCedantDateOfNotification,
            string SummaryPaidClaimsDateOfRegister,
            string SummaryPaidClaimsDateOfCommencement,
            string SummaryPaidClaimsDateOfEvent,
            int? SummaryPaidClaimsPolicyDuration,
            string SummaryPaidClaimsTargetDateToIssueInvoice,
            string SummaryPaidClaimsSumReinsured,
            string SummaryPaidClaimsCauseOfEvent,
            int? SummaryPaidClaimsPicClaims,
            int? SummaryPaidClaimsPicDaa,
            int? SummaryPaidClaimsStatus,
            int? SummaryPaidClaimsProvisionStatus,
            int? SummaryPaidClaimsOffsetStatus
        )
        {
            // type 1 = all
            // type 2 = filtered download
            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            dynamic Params = new ExpandoObject();
            Params.Id = Id;
            Params.SummaryPaidClaimsHasRedFlag = SummaryPaidClaimsHasRedFlag;
            Params.SummaryPaidClaimsEntryNo = SummaryPaidClaimsEntryNo;
            Params.SummaryPaidClaimsSoaQuarter = SummaryPaidClaimsSoaQuarter;
            Params.SummaryPaidClaimsClaimId = SummaryPaidClaimsClaimId;
            Params.SummaryPaidClaimsClaimTransactionType = SummaryPaidClaimsClaimTransactionType;
            Params.SummaryPaidClaimsIsReferralCase = SummaryPaidClaimsIsReferralCase;
            Params.SummaryPaidClaimsRiDataId = SummaryPaidClaimsRiDataId;
            Params.SummaryPaidClaimsRecordType = SummaryPaidClaimsRecordType;
            Params.SummaryPaidClaimsTreatyCode = SummaryPaidClaimsTreatyCode;
            Params.SummaryPaidClaimsPolicyNumber = SummaryPaidClaimsPolicyNumber;
            Params.SummaryPaidClaimsCedingCompany = SummaryPaidClaimsCedingCompany;
            Params.SummaryPaidClaimsInsuredName = SummaryPaidClaimsInsuredName;
            Params.SummaryPaidClaimsInsuredDateOfBirth = SummaryPaidClaimsInsuredDateOfBirth;
            Params.SummaryPaidClaimsLastTransaction = SummaryPaidClaimsLastTransaction;
            Params.SummaryPaidClaimsDateOfReported = SummaryPaidClaimsDateOfReported;
            Params.SummaryPaidClaimsCedantDateOfNotification = SummaryPaidClaimsCedantDateOfNotification;
            Params.SummaryPaidClaimsDateOfRegister = SummaryPaidClaimsDateOfRegister;
            Params.SummaryPaidClaimsDateOfCommencement = SummaryPaidClaimsDateOfCommencement;
            Params.SummaryPaidClaimsDateOfEvent = SummaryPaidClaimsDateOfEvent;
            Params.SummaryPaidClaimsPolicyDuration = SummaryPaidClaimsPolicyDuration;
            Params.SummaryPaidClaimsTargetDateToIssueInvoice = SummaryPaidClaimsTargetDateToIssueInvoice;
            Params.SummaryPaidClaimsSumReinsured = SummaryPaidClaimsSumReinsured;
            Params.SummaryPaidClaimsCauseOfEvent = SummaryPaidClaimsCauseOfEvent;
            Params.SummaryPaidClaimsPicClaims = SummaryPaidClaimsPicClaims;
            Params.SummaryPaidClaimsPicDaa = SummaryPaidClaimsPicDaa;
            Params.SummaryPaidClaimsStatus = SummaryPaidClaimsStatus;
            Params.SummaryPaidClaimsProvisionStatus = SummaryPaidClaimsProvisionStatus;
            Params.SummaryPaidClaimsOffsetStatus = SummaryPaidClaimsOffsetStatus;
            var export = new GenerateExportData();
            ExportBo exportBo = export.CreateExportPerLifeClaimSummaryPaidClaims(Id, AuthUserId, Params);

            if (exportBo.Total <= export.ExportInstantDownloadLimit)
            {
                export.Process(ref exportBo, _db);
                Session.Add("DownloadExport", true);
            }

            return RedirectToAction("Edit", "Export", new { exportBo.Id });
        }
        public ActionResult PendingClaimsDownload(
            string downloadToken,
            int type,
            int Id,
            bool? SummaryPendingClaimsHasRedFlag,
            string SummaryPendingClaimsEntryNo,
            string SummaryPendingClaimsSoaQuarter,
            string SummaryPendingClaimsClaimId,
            string SummaryPendingClaimsClaimTransactionType,
            bool? SummaryPendingClaimsIsReferralCase,
            int? SummaryPendingClaimsRiDataId,
            string SummaryPendingClaimsRecordType,
            string SummaryPendingClaimsTreatyCode,
            string SummaryPendingClaimsPolicyNumber,
            string SummaryPendingClaimsCedingCompany,
            string SummaryPendingClaimsInsuredName,
            string SummaryPendingClaimsInsuredDateOfBirth,
            string SummaryPendingClaimsLastTransaction,
            string SummaryPendingClaimsDateOfReported,
            string SummaryPendingClaimsCedantDateOfNotification,
            string SummaryPendingClaimsDateOfRegister,
            string SummaryPendingClaimsDateOfCommencement,
            string SummaryPendingClaimsDateOfEvent,
            int? SummaryPendingClaimsPolicyDuration,
            string SummaryPendingClaimsTargetDateToIssueInvoice,
            string SummaryPendingClaimsSumReinsured,
            string SummaryPendingClaimsCauseOfEvent,
            int? SummaryPendingClaimsPicClaims,
            int? SummaryPendingClaimsPicDaa,
            int? SummaryPendingClaimsStatus,
            int? SummaryPendingClaimsProvisionStatus,
            int? SummaryPendingClaimsOffsetStatus
        )
        {
            // type 1 = all
            // type 2 = filtered download
            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            dynamic Params = new ExpandoObject();
            Params.Id = Id;
            Params.SummaryPendingClaimsHasRedFlag = SummaryPendingClaimsHasRedFlag;
            Params.SummaryPendingClaimsEntryNo = SummaryPendingClaimsEntryNo;
            Params.SummaryPendingClaimsSoaQuarter = SummaryPendingClaimsSoaQuarter;
            Params.SummaryPendingClaimsClaimId = SummaryPendingClaimsClaimId;
            Params.SummaryPendingClaimsClaimTransactionType = SummaryPendingClaimsClaimTransactionType;
            Params.SummaryPendingClaimsIsReferralCase = SummaryPendingClaimsIsReferralCase;
            Params.SummaryPendingClaimsRiDataId = SummaryPendingClaimsRiDataId;
            Params.SummaryPendingClaimsRecordType = SummaryPendingClaimsRecordType;
            Params.SummaryPendingClaimsTreatyCode = SummaryPendingClaimsTreatyCode;
            Params.SummaryPendingClaimsPolicyNumber = SummaryPendingClaimsPolicyNumber;
            Params.SummaryPendingClaimsCedingCompany = SummaryPendingClaimsCedingCompany;
            Params.SummaryPendingClaimsInsuredName = SummaryPendingClaimsInsuredName;
            Params.SummaryPendingClaimsInsuredDateOfBirth = SummaryPendingClaimsInsuredDateOfBirth;
            Params.SummaryPendingClaimsLastTransaction = SummaryPendingClaimsLastTransaction;
            Params.SummaryPendingClaimsDateOfReported = SummaryPendingClaimsDateOfReported;
            Params.SummaryPendingClaimsCedantDateOfNotification = SummaryPendingClaimsCedantDateOfNotification;
            Params.SummaryPendingClaimsDateOfRegister = SummaryPendingClaimsDateOfRegister;
            Params.SummaryPendingClaimsDateOfCommencement = SummaryPendingClaimsDateOfCommencement;
            Params.SummaryPendingClaimsDateOfEvent = SummaryPendingClaimsDateOfEvent;
            Params.SummaryPendingClaimsPolicyDuration = SummaryPendingClaimsPolicyDuration;
            Params.SummaryPendingClaimsTargetDateToIssueInvoice = SummaryPendingClaimsTargetDateToIssueInvoice;
            Params.SummaryPendingClaimsSumReinsured = SummaryPendingClaimsSumReinsured;
            Params.SummaryPendingClaimsCauseOfEvent = SummaryPendingClaimsCauseOfEvent;
            Params.SummaryPendingClaimsPicClaims = SummaryPendingClaimsPicClaims;
            Params.SummaryPendingClaimsPicDaa = SummaryPendingClaimsPicDaa;
            Params.SummaryPendingClaimsStatus = SummaryPendingClaimsStatus;
            Params.SummaryPendingClaimsProvisionStatus = SummaryPendingClaimsProvisionStatus;
            Params.SummaryPendingClaimsOffsetStatus = SummaryPendingClaimsOffsetStatus;
            var export = new GenerateExportData();
            ExportBo exportBo = export.CreateExportPerLifeClaimSummaryPendingClaims(Id, AuthUserId, Params);

            if (exportBo.Total <= export.ExportInstantDownloadLimit)
            {
                export.Process(ref exportBo, _db);
                Session.Add("DownloadExport", true);
            }

            return RedirectToAction("Edit", "Export", new { exportBo.Id });
        }
        public ActionResult ClaimsRemovedDownload(
            string downloadToken,
            int type,
            int Id,
            bool? SummaryClaimsRemovedHasRedFlag,
            string SummaryClaimsRemovedEntryNo,
            string SummaryClaimsRemovedSoaQuarter,
            string SummaryClaimsRemovedClaimId,
            string SummaryClaimsRemovedClaimTransactionType,
            bool? SummaryClaimsRemovedIsReferralCase,
            int? SummaryClaimsRemovedRiDataId,
            string SummaryClaimsRemovedRecordType,
            string SummaryClaimsRemovedTreatyCode,
            string SummaryClaimsRemovedPolicyNumber,
            string SummaryClaimsRemovedCedingCompany,
            string SummaryClaimsRemovedInsuredName,
            string SummaryClaimsRemovedInsuredDateOfBirth,
            string SummaryClaimsRemovedLastTransaction,
            string SummaryClaimsRemovedDateOfReported,
            string SummaryClaimsRemovedCedantDateOfNotification,
            string SummaryClaimsRemovedDateOfRegister,
            string SummaryClaimsRemovedDateOfCommencement,
            string SummaryClaimsRemovedDateOfEvent,
            int? SummaryClaimsRemovedPolicyDuration,
            string SummaryClaimsRemovedTargetDateToIssueInvoice,
            string SummaryClaimsRemovedSumReinsured,
            string SummaryClaimsRemovedCauseOfEvent,
            int? SummaryClaimsRemovedPicClaims,
            int? SummaryClaimsRemovedPicDaa,
            int? SummaryClaimsRemovedStatus,
            int? SummaryClaimsRemovedProvisionStatus,
            int? SummaryClaimsRemovedOffsetStatus
        )
        {
            // type 1 = all
            // type 2 = filtered download
            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            dynamic Params = new ExpandoObject();
            Params.Id = Id;
            Params.SummaryClaimsRemovedHasRedFlag = SummaryClaimsRemovedHasRedFlag;
            Params.SummaryClaimsRemovedEntryNo = SummaryClaimsRemovedEntryNo;
            Params.SummaryClaimsRemovedSoaQuarter = SummaryClaimsRemovedSoaQuarter;
            Params.SummaryClaimsRemovedClaimId = SummaryClaimsRemovedClaimId;
            Params.SummaryClaimsRemovedClaimTransactionType = SummaryClaimsRemovedClaimTransactionType;
            Params.SummaryClaimsRemovedIsReferralCase = SummaryClaimsRemovedIsReferralCase;
            Params.SummaryClaimsRemovedRiDataId = SummaryClaimsRemovedRiDataId;
            Params.SummaryClaimsRemovedRecordType = SummaryClaimsRemovedRecordType;
            Params.SummaryClaimsRemovedTreatyCode = SummaryClaimsRemovedTreatyCode;
            Params.SummaryClaimsRemovedPolicyNumber = SummaryClaimsRemovedPolicyNumber;
            Params.SummaryClaimsRemovedCedingCompany = SummaryClaimsRemovedCedingCompany;
            Params.SummaryClaimsRemovedInsuredName = SummaryClaimsRemovedInsuredName;
            Params.SummaryClaimsRemovedInsuredDateOfBirth = SummaryClaimsRemovedInsuredDateOfBirth;
            Params.SummaryClaimsRemovedLastTransaction = SummaryClaimsRemovedLastTransaction;
            Params.SummaryClaimsRemovedDateOfReported = SummaryClaimsRemovedDateOfReported;
            Params.SummaryClaimsRemovedCedantDateOfNotification = SummaryClaimsRemovedCedantDateOfNotification;
            Params.SummaryClaimsRemovedDateOfRegister = SummaryClaimsRemovedDateOfRegister;
            Params.SummaryClaimsRemovedDateOfCommencement = SummaryClaimsRemovedDateOfCommencement;
            Params.SummaryClaimsRemovedDateOfEvent = SummaryClaimsRemovedDateOfEvent;
            Params.SummaryClaimsRemovedPolicyDuration = SummaryClaimsRemovedPolicyDuration;
            Params.SummaryClaimsRemovedTargetDateToIssueInvoice = SummaryClaimsRemovedTargetDateToIssueInvoice;
            Params.SummaryClaimsRemovedSumReinsured = SummaryClaimsRemovedSumReinsured;
            Params.SummaryClaimsRemovedCauseOfEvent = SummaryClaimsRemovedCauseOfEvent;
            Params.SummaryClaimsRemovedPicClaims = SummaryClaimsRemovedPicClaims;
            Params.SummaryClaimsRemovedPicDaa = SummaryClaimsRemovedPicDaa;
            Params.SummaryClaimsRemovedStatus = SummaryClaimsRemovedStatus;
            Params.SummaryClaimsRemovedProvisionStatus = SummaryClaimsRemovedProvisionStatus;
            Params.SummaryClaimsRemovedOffsetStatus = SummaryClaimsRemovedOffsetStatus;
            var export = new GenerateExportData();
            ExportBo exportBo = export.CreateExportPerLifeClaimSummaryClaimsRemoved(Id, AuthUserId, Params);

            if (exportBo.Total <= export.ExportInstantDownloadLimit)
            {
                export.Process(ref exportBo, _db);
                Session.Add("DownloadExport", true);
            }

            return RedirectToAction("Edit", "Export", new { exportBo.Id });
        }

        public void LoadPage(PerLifeClaimViewModel model)
        {
            //var departmentId = Util.GetConfigInteger("PerLifeAggregationPicDepartment", DepartmentBo.DepartmentRetrocession);
            ViewBag.DropDownRetrocessionUsers = DropDownUser(UserBo.StatusActive, null, true, DepartmentBo.DepartmentRetrocession);
            DropDownCutOffQuarterWithDate();
            DropDownFundsAccountingTypeCode();
            DropDownCutOff();
            DropDownYesNoWithSelect();
            DropDownClaimStatus();
            DropDownProvisionStatus();
            DropDownOffsetStatus();
            DropDownPicClaims();
            DropDownPicDaa();
            GetStatusHistoryList(model);

            string downloadDocumentUrl = Url.Action("Download", "Document");
            GetStatusHistories(model.ModuleId, model.Id, downloadDocumentUrl);

            ViewBag.Status = model.Status;


            SetViewBagMessage();
        }
        public void GetStatusHistoryList(PerLifeClaimViewModel model)
        {
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.PerLifeClaim.ToString());
            // StatusHistories
            IList<StatusHistoryBo> statusHistoryBos = StatusHistoryService.GetStatusHistoriesByModuleIdObjectId(moduleBo.Id, model.Id);
            ViewBag.StatusHistories = statusHistoryBos;
        }

        public List<SelectListItem> DropDownPerLifeClaimStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, PerLifeClaimBo.StatusFinalised))
            {
                items.Add(new SelectListItem { Text = PerLifeClaimBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownPerLifeClaimStatuses = items;
            return items;
        }

        public int DropDownPicClaims()
        {
            ViewBag.DropDownPicClaims = DropDownUser(UserBo.StatusActive, null, true, DepartmentBo.DepartmentClaim);
            return 1;
        }

        public int DropDownPicDaa()
        {
            ViewBag.DropDownPicDaa = DropDownUser(UserBo.StatusActive, null, true, DepartmentBo.DepartmentDataAnalyticsAdministration);
            return 1;
        }

        [HttpPost]
        public JsonResult SubmitForProcessing(int id, PerLifeClaimViewModel model, bool? confirm = false)
        {
            var bo = PerLifeClaimService.Find(id);

            if (confirm == false)
            {
                var perLifeClaimDataCount = PerLifeClaimDataService.Get().Where(a => a.PerLifeClaimId == id).Count();

                if (perLifeClaimDataCount > 0)
                {
                    return Json(new { errors = "overwrite" });
                }
            }

            bo.Status = PerLifeClaimBo.StatusSubmitForProcessing;

            var trail = GetNewTrailObject();
            Result = PerLifeClaimService.Update(ref bo);
            model.ProcessStatusHistory(AuthUserId, ref trail);


            if (Result.Valid)
            {
                return Json(new { errors = "success" });
            }
            else
            {
                return Json(new { errors = Result.MessageBag });
            }
        }

        [HttpPost]
        public JsonResult ValidateData(int id, PerLifeClaimViewModel model)
        {
            var bo = PerLifeClaimService.Find(id);

            if (bo.Status == PerLifeClaimBo.StatusProcessingSuccess)
            {
                bo.Status = PerLifeClaimBo.StatusSubmitForValidation;

                var trail = GetNewTrailObject();
                Result = PerLifeClaimService.Update(ref bo);
                model.ProcessStatusHistory(AuthUserId, ref trail);

                if (Result.Valid)
                {
                    return Json(new { errors = "success" });
                }
                else
                {
                    return Json(new { errors = Result.MessageBag });
                }

            }
            else
            {
                return Json(new { errors = "fail" });
            }
        }

        [HttpPost]
        public JsonResult ProcessClaimRecovery(int id, PerLifeClaimViewModel model)
        {
            var bo = PerLifeClaimService.Find(id);

            if (bo.Status == PerLifeClaimBo.StatusValidationSuccess)
            {

                bo.Status = PerLifeClaimBo.StatusSubmitForRetroRecovery;

                var trail = GetNewTrailObject();
                Result = PerLifeClaimService.Update(ref bo);
                model.ProcessStatusHistory(AuthUserId, ref trail);

                if (Result.Valid)
                {
                    return Json(new { errors = "success" });
                }
                else
                {
                    return Json(new { errors = Result.MessageBag });
                }
            }
            else
            {
                return Json(new { errors = "fail" });
            }
        }

        [HttpPost]
        public JsonResult ProcessFinalise(int id, PerLifeClaimViewModel model)
        {
            var bo = PerLifeClaimService.Find(id);

            if (bo.Status == PerLifeClaimBo.StatusProcessingRetroRecoverySuccess)
            {

                bo.Status = PerLifeClaimBo.StatusFinalised;

                var trail = GetNewTrailObject();
                Result = PerLifeClaimService.Update(ref bo);
                model.ProcessStatusHistory(AuthUserId, ref trail);

                if (Result.Valid)
                {
                    return Json(new { errors = "success" });
                }
                else
                {
                    return Json(new { errors = Result.MessageBag });
                }
            }
            else
            {
                return Json(new { errors = "fail" });
            }
        }

        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = PerLifeClaimService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            return View(new PerLifeClaimViewModel(bo));
        }

        // POST: PerLifeClaim/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, PerLifeClaimViewModel model)
        {
            var bo = PerLifeClaimService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = PerLifeClaimService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Per Life Claim"
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

    }
}