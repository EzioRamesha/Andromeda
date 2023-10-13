using BusinessObject;
using ConsoleApp.Commands.ProcessDatas;
using DataAccess.Entities;
using Newtonsoft.Json;
using PagedList;
using Services;
using Shared;
using System;
using System.Collections.Generic;
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
    public class ClaimRegisterSearchController : BaseController
    {
        public const string Controller = "ClaimRegisterSearch";

        // GET: ClaimRegisterSearch
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            int? CutOffId,
            string ClaimId,
            string InsuredName,
            string InsuredDateOfBirth,
            string PolicyNumber,
            string TreatyCode,
            bool? IsWithAdjustmentDetail, // Filter claim transaction type
            bool? HasRedFlag,
            string EntryNo,
            string SoaQuarter,
            string ClaimIdFilter,
            string ClaimTransactionType,
            string ClaimRecoveryAmount,
            bool? IsReferralCase,
            int? RiDataWarehouseId,
            string RecordType,
            string TreatyCodeFilter,
            string PolicyNumberFilter,
            string CedingCompany,
            string ProvisionAt,
            string SortOrder,
            int? Page,
            bool IsSnapshotVersion = false
        )
        {
            DateTime? insuredDateOfBirth = Util.GetParseDateTime(InsuredDateOfBirth);
            DateTime? provisionAt = Util.GetParseDateTime(ProvisionAt);
            double? claimRecoveryAmount = Util.StringToDouble(ClaimRecoveryAmount);

            string[] treatyCodes = Util.ToArraySplitTrim(TreatyCode);

            List<string> errors = new List<string> { };

            if (!string.IsNullOrEmpty(InsuredDateOfBirth) && !insuredDateOfBirth.HasValue)
                errors.Add(string.Format(MessageBag.InvalidDateFormatWithName, "Insured Date of Birth"));

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["IsSnapshotVersion"] = IsSnapshotVersion,
                ["CutOffId"] = CutOffId,
                ["ClaimId"] = ClaimId,
                ["InsuredName"] = InsuredName,
                ["InsuredDateOfBirth"] = InsuredDateOfBirth,
                ["PolicyNumber"] = PolicyNumber,
                ["TreatyCode"] = TreatyCode,
                ["IsWithAdjustmentDetail"] = IsWithAdjustmentDetail,
                ["HasRedFlag"] = HasRedFlag,
                ["EntryNo"] = EntryNo,
                ["SoaQuarter"] = SoaQuarter,
                ["ClaimIdFilter"] = ClaimIdFilter,
                ["ClaimTransactionType"] = ClaimTransactionType,
                ["ClaimRecoveryAmount"] = ClaimRecoveryAmount,
                ["IsReferralCase"] = IsReferralCase,
                ["RiDataWarehouseId"] = RiDataWarehouseId,
                ["RecordType"] = RecordType,
                ["TreatyCodeFilter"] = TreatyCodeFilter,
                ["PolicyNumberFilter"] = PolicyNumberFilter,
                ["CedingCompany"] = CedingCompany,
                ["ProvisionAt"] = ProvisionAt,
                ["SortOrder"] = SortOrder,
            };

            ViewBag.SortOrder = SortOrder;
            ViewBag.SortEntryNo = GetSortParam("EntryNo");
            ViewBag.SortSoaQuarter = GetSortParam("SoaQuarter");
            ViewBag.SortClaimIdFilter = GetSortParam("ClaimIdFilter");
            ViewBag.SortClaimTransactionType = GetSortParam("ClaimTransactionType");
            ViewBag.SortClaimRecoveryAmount = GetSortParam("ClaimRecoveryAmount");
            ViewBag.SortHasRedFlag = GetSortParam("HasRedFlag");
            ViewBag.SortIsReferralCase = GetSortParam("IsReferralCase");
            ViewBag.SortRiDataWarehouseId = GetSortParam("RiDataWarehouseId");
            ViewBag.SortRecordType = GetSortParam("RecordType");
            ViewBag.SortTreatyCodeFilter = GetSortParam("TreatyCodeFilter");
            ViewBag.SortPolicyNumberFilter = GetSortParam("PolicyNumberFilter");
            ViewBag.SortCedingCompany = GetSortParam("CedingCompany");
            ViewBag.SortProvisionAt = GetSortParam("ProvisionAt");

            _db.Database.CommandTimeout = 0;

            IQueryable<ClaimRegisterSearchViewModel> query;

            bool isWithAdjustmentDetail = IsWithAdjustmentDetail.HasValue && IsWithAdjustmentDetail.Value;
            if (IsSnapshotVersion)
            {
                if (isWithAdjustmentDetail)
                    query = _db.FinanceProvisioningTransactions.Join(_db.ClaimRegisterHistories, t => t.ClaimRegisterId, h => h.ClaimRegisterId, (t, h) => new ClaimRegisterHistoryTransaction { Transaction = t, History = h })
                        .Select(ClaimRegisterSearchViewModel.HistoryAdjustmentExpression());
                else
                    query = _db.ClaimRegisterHistories.AsNoTracking().Select(ClaimRegisterSearchViewModel.HistoryExpression());
            }
            else
            {
                if (isWithAdjustmentDetail)
                    query = _db.FinanceProvisioningTransactions.AsNoTracking().Select(ClaimRegisterSearchViewModel.AdjustmentExpression());
                else
                    query = _db.ClaimRegister.AsNoTracking().Select(ClaimRegisterSearchViewModel.Expression());
            }

            ViewBag.ShowProvisionDate = isWithAdjustmentDetail;
            if (errors.Count() > 0)
            {
                query = query.Where(q => q.Id == 0);

                if (errors.Count() > 0)
                    SetErrorSessionMsgArr(errors);

                LoadPage(CutOffId);
                ViewBag.DisableDownload = true;
                ViewBag.Total = query.Count();
                return View(query.ToPagedList(Page ?? 1, PageSize));
            }
            else
            {
                query = query.Where(q => q.ClaimTransactionType != PickListDetailBo.ClaimTransactionTypeBulk);
                if (IsSnapshotVersion)
                {
                    var cutOffId = CutOffId ?? 0;
                    query = query.Where(q => q.CutOffId == cutOffId);

                    if (isWithAdjustmentDetail)
                    {
                        query = query.Where(q => q.FinanceProvisioningStatus != FinanceProvisioningBo.StatusPending).Where(q => DbFunctions.TruncateTime(q.ProvisionAt) <= DbFunctions.TruncateTime(q.CutOffAt));
                    }
                }
                if (!string.IsNullOrEmpty(ClaimId)) query = query.Where(q => q.ClaimId == ClaimId);
                if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => q.InsuredName == InsuredName);
                if (insuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == insuredDateOfBirth);
                if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber == PolicyNumber);
                if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => treatyCodes.Contains(q.TreatyCode));
                if (HasRedFlag.HasValue) query = query.Where(q => q.HasRedFlag == HasRedFlag);
                if (!string.IsNullOrEmpty(EntryNo)) query = query.Where(q => q.EntryNo == EntryNo);
                if (!string.IsNullOrEmpty(SoaQuarter)) query = query.Where(q => q.SoaQuarter == SoaQuarter);
                if (!string.IsNullOrEmpty(ClaimIdFilter)) query = query.Where(q => q.ClaimId == ClaimIdFilter);
                if (!string.IsNullOrEmpty(ClaimTransactionType)) query = query.Where(q => q.ClaimTransactionType == ClaimTransactionType);
                if (claimRecoveryAmount.HasValue) query = query.Where(q => q.ClaimRecoveryAmount == claimRecoveryAmount);
                if (IsReferralCase.HasValue) query = query.Where(q => q.IsReferralCase == IsReferralCase);
                if (RiDataWarehouseId.HasValue) query = query.Where(q => q.RiDataWarehouseId == RiDataWarehouseId);
                if (!string.IsNullOrEmpty(RecordType)) query = query.Where(q => q.RecordType == RecordType);
                if (!string.IsNullOrEmpty(TreatyCodeFilter)) query = query.Where(q => q.TreatyCode == TreatyCodeFilter);
                if (!string.IsNullOrEmpty(PolicyNumberFilter)) query = query.Where(q => q.PolicyNumber == PolicyNumberFilter);
                if (!string.IsNullOrEmpty(CedingCompany)) query = query.Where(q => q.CedingCompany == CedingCompany);
                if (provisionAt.HasValue) query = query.Where(q => DbFunctions.TruncateTime(q.ProvisionAt) == DbFunctions.TruncateTime(provisionAt));

                if (SortOrder == Html.GetSortAsc("EntryNo")) query = query.OrderBy(q => q.EntryNo);
                else if (SortOrder == Html.GetSortDsc("EntryNo")) query = query.OrderByDescending(q => q.EntryNo);
                else if (SortOrder == Html.GetSortAsc("SoaQuarter")) query = query.OrderBy(q => q.SoaQuarter);
                else if (SortOrder == Html.GetSortDsc("SoaQuarter")) query = query.OrderByDescending(q => q.SoaQuarter);
                else if (SortOrder == Html.GetSortAsc("ClaimId")) query = query.OrderBy(q => q.ClaimId);
                else if (SortOrder == Html.GetSortDsc("ClaimId")) query = query.OrderByDescending(q => q.ClaimId);
                else if (SortOrder == Html.GetSortAsc("ClaimTransactionType")) query = query.OrderBy(q => q.ClaimTransactionType);
                else if (SortOrder == Html.GetSortDsc("ClaimTransactionType")) query = query.OrderByDescending(q => q.ClaimTransactionType);
                else if (SortOrder == Html.GetSortAsc("ClaimRecoveryAmount")) query = query.OrderBy(q => q.ClaimRecoveryAmount);
                else if (SortOrder == Html.GetSortDsc("ClaimRecoveryAmount")) query = query.OrderByDescending(q => q.ClaimRecoveryAmount);
                else if (SortOrder == Html.GetSortAsc("HasRedFlag")) query = query.OrderBy(q => q.HasRedFlag);
                else if (SortOrder == Html.GetSortDsc("HasRedFlag")) query = query.OrderByDescending(q => q.HasRedFlag);
                else if (SortOrder == Html.GetSortAsc("IsReferralCase")) query = query.OrderBy(q => q.IsReferralCase);
                else if (SortOrder == Html.GetSortDsc("IsReferralCase")) query = query.OrderByDescending(q => q.IsReferralCase);
                else if (SortOrder == Html.GetSortAsc("RiDataWarehouseId")) query = query.OrderBy(q => q.RiDataWarehouseId);
                else if (SortOrder == Html.GetSortDsc("RiDataWarehouseId")) query = query.OrderByDescending(q => q.RiDataWarehouseId);
                else if (SortOrder == Html.GetSortAsc("RecordType")) query = query.OrderBy(q => q.RecordType);
                else if (SortOrder == Html.GetSortDsc("RecordType")) query = query.OrderByDescending(q => q.RecordType);
                else if (SortOrder == Html.GetSortAsc("TreatyCodeFilter")) query = query.OrderBy(q => q.TreatyCode);
                else if (SortOrder == Html.GetSortDsc("TreatyCodeFilter")) query = query.OrderByDescending(q => q.TreatyCode);
                else if (SortOrder == Html.GetSortAsc("PolicyNumberFilter")) query = query.OrderBy(q => q.PolicyNumber);
                else if (SortOrder == Html.GetSortDsc("PolicyNumberFilter")) query = query.OrderByDescending(q => q.PolicyNumber);
                else if (SortOrder == Html.GetSortAsc("CedingCompany")) query = query.OrderBy(q => q.CedingCompany);
                else if (SortOrder == Html.GetSortDsc("CedingCompany")) query = query.OrderByDescending(q => q.CedingCompany);
                else if (SortOrder == Html.GetSortAsc("ProvisionAt")) query = query.OrderBy(q => q.ProvisionAt);
                else if (SortOrder == Html.GetSortDsc("ProvisionAt")) query = query.OrderByDescending(q => q.ProvisionAt);
                else query = query.OrderBy(q => q.ClaimRegisterId).ThenBy(q => q.SortIndex);

                //IOrderedQueryable<ClaimRegisterSearchViewModel> orderedQuery = (IOrderedQueryable<ClaimRegisterSearchViewModel>)query;

                LoadPage(CutOffId);
                ViewBag.Total = query.Count();
                return View(query.ToPagedList(Page ?? 1, PageSize));
            }
        }

        // GET: RiDataSearch/Details
        public ActionResult Details(
            int? id,
            bool IsSnapshotVersion,
            int? CutOffId,
            string ClaimId,
            string InsuredName,
            string InsuredDateOfBirth,
            string PolicyNumber,
            string TreatyCode,
            bool? IsWithAdjustmentDetail, // Ignore when filter
            bool? HasRedFlag,
            string EntryNo,
            string SoaQuarter,
            string ClaimIdFilter,
            string ClaimTransactionType,
            string ClaimRecoveryAmount,
            bool? IsReferralCase,
            int? RiDataWarehouseId,
            string RecordType,
            string TreatyCodeFilter,
            string PolicyNumberFilter,
            string CedingCompany,
            string ProvisionAt
        )
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["IsSnapshotVersion"] = IsSnapshotVersion,
                ["CutOffId"] = CutOffId,
                ["ClaimId"] = ClaimId,
                ["InsuredName"] = InsuredName,
                ["InsuredDateOfBirth"] = InsuredDateOfBirth,
                ["PolicyNumber"] = PolicyNumber,
                ["TreatyCode"] = TreatyCode,
                ["IsWithAdjustmentDetail"] = IsWithAdjustmentDetail,
                ["HasRedFlag"] = HasRedFlag,
                ["EntryNo"] = EntryNo,
                ["SoaQuarter"] = SoaQuarter,
                ["ClaimIdFilter"] = ClaimIdFilter,
                ["ClaimTransactionType"] = ClaimTransactionType,
                ["ClaimRecoveryAmount"] = ClaimRecoveryAmount,
                ["IsReferralCase"] = IsReferralCase,
                ["RiDataWarehouseId"] = RiDataWarehouseId,
                ["RecordType"] = RecordType,
                ["TreatyCodeFilter"] = TreatyCodeFilter,
                ["PolicyNumberFilter"] = PolicyNumberFilter,
                ["CedingCompany"] = CedingCompany,
                ["ProvisionAt"] = ProvisionAt,
            };

            bool isWithAdjustmentDetail = IsWithAdjustmentDetail.HasValue && IsWithAdjustmentDetail.Value;
            if (IsSnapshotVersion)
            {
                ClaimRegisterHistoryBo claimRegisterHistoryBo = new ClaimRegisterHistoryBo();
                if (isWithAdjustmentDetail)
                {
                    FinanceProvisioningTransactionBo bo = FinanceProvisioningTransactionService.Find(id);
                    claimRegisterHistoryBo = bo != null ? ClaimRegisterHistoryService.Find(CutOffId.Value, bo.ClaimRegisterId) : null;
                    if (claimRegisterHistoryBo != null)
                    {
                        if (bo.SortIndex != 0)
                            claimRegisterHistoryBo.EntryNo = string.Format("{0} - {1}", claimRegisterHistoryBo.EntryNo, bo.SortIndex);
                        if (bo.Status == FinanceProvisioningTransactionBo.StatusProvisioned)
                        {
                            claimRegisterHistoryBo.ProvisionStatus = ClaimRegisterBo.ProvisionStatusProvisioned;
                        }
                        claimRegisterHistoryBo.ClaimId = bo.ClaimId;
                        claimRegisterHistoryBo.PolicyNumber = bo.PolicyNumber;
                        claimRegisterHistoryBo.CedingCompany = bo.CedingCompany;
                        claimRegisterHistoryBo.SoaQuarter = bo.Quarter;
                        claimRegisterHistoryBo.AarPayable = bo.SumReinsured;
                        claimRegisterHistoryBo.AarPayableStr = bo.SumReinsuredStr;
                        claimRegisterHistoryBo.ClaimRecoveryAmt = bo.ClaimRecoveryAmount;
                        claimRegisterHistoryBo.ClaimRecoveryAmtStr = bo.ClaimRecoveryAmountStr;
                        claimRegisterHistoryBo.TreatyCode = bo.TreatyCode;
                        claimRegisterHistoryBo.TreatyType = bo.TreatyType;
                        claimRegisterHistoryBo.ClaimCode = bo.ClaimCode;
                    }
                }
                else
                {
                    if (id.HasValue)
                        claimRegisterHistoryBo = ClaimRegisterHistoryService.Find(id.Value);
                }
                ViewBag.ClaimRegisterBo = claimRegisterHistoryBo;

                if (claimRegisterHistoryBo != null && !string.IsNullOrEmpty(claimRegisterHistoryBo.Errors))
                {
                    var Errors = JsonConvert.DeserializeObject<Dictionary<string, object>>(claimRegisterHistoryBo.Errors);
                    ViewBag.Errors = Errors;
                }
            }
            else
            {
                ClaimRegisterBo claimRegisterBo = new ClaimRegisterBo();
                if (isWithAdjustmentDetail)
                {
                    FinanceProvisioningTransactionBo bo = FinanceProvisioningTransactionService.Find(id);
                    claimRegisterBo = bo != null ? ClaimRegisterService.Find(bo.ClaimRegisterId, formatOutput: true, precision: null) : null;
                    if (claimRegisterBo != null)
                    {
                        if (bo.SortIndex != 0)
                            claimRegisterBo.EntryNo = string.Format("{0} - {1}", claimRegisterBo.EntryNo, bo.SortIndex);
                        if (bo.Status == FinanceProvisioningTransactionBo.StatusProvisioned)
                        {
                            claimRegisterBo.ProvisionStatus = ClaimRegisterBo.ProvisionStatusProvisioned;
                        }
                        claimRegisterBo.ClaimId = bo.ClaimId;
                        claimRegisterBo.PolicyNumber = bo.PolicyNumber;
                        claimRegisterBo.CedingCompany = bo.CedingCompany;
                        claimRegisterBo.SoaQuarter = bo.Quarter;
                        claimRegisterBo.AarPayable = bo.SumReinsured;
                        claimRegisterBo.AarPayableStr = bo.SumReinsuredStr;
                        claimRegisterBo.ClaimRecoveryAmt = bo.ClaimRecoveryAmount;
                        claimRegisterBo.ClaimRecoveryAmtStr = bo.ClaimRecoveryAmountStr;
                        claimRegisterBo.TreatyCode = bo.TreatyCode;
                        claimRegisterBo.TreatyType = bo.TreatyType;
                        claimRegisterBo.ClaimCode = bo.ClaimCode;
                    }
                }
                else
                {
                    if (id.HasValue)
                        claimRegisterBo = ClaimRegisterService.Find(id, formatOutput: true, precision: null);
                }
                ViewBag.ClaimRegisterBo = claimRegisterBo;

                if (claimRegisterBo != null && !string.IsNullOrEmpty(claimRegisterBo.Errors))
                {
                    var Errors = JsonConvert.DeserializeObject<Dictionary<string, object>>(claimRegisterBo.Errors);
                    ViewBag.Errors = Errors;
                }
            }

            LoadPage();
            return View(new ClaimRegisterSearchViewModel());
        }

        public ActionResult Download(
            string downloadToken,
            bool? WriteHeader,
            bool? IsSnapshotVersion,
            int? CutOffId,
            string ClaimId,
            string InsuredName,
            string InsuredDateOfBirth,
            string PolicyNumber,
            string TreatyCode,
            bool? IsWithAdjustmentDetail, // Ignore when filter
            bool? HasRedFlag,
            string EntryNo,
            string SoaQuarter,
            string ClaimIdFilter,
            string ClaimTransactionType,
            string ClaimRecoveryAmount,
            bool? IsReferralCase,
            int? RiDataWarehouseId,
            string RecordType,
            string TreatyCodeFilter,
            string PolicyNumberFilter,
            string CedingCompany,
            string ProvisionAt
        )
        {
            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            dynamic Params = new ExpandoObject();
            Params.WriteHeader = WriteHeader;
            Params.IsSnapshotVersion = IsSnapshotVersion ?? false;
            Params.CutOffId = IsSnapshotVersion.HasValue && IsSnapshotVersion.Value ? CutOffId : null;
            Params.ClaimId = ClaimId;
            Params.InsuredName = InsuredName;
            Params.InsuredDateOfBirth = InsuredDateOfBirth;
            Params.PolicyNumber = PolicyNumber;
            Params.TreatyCode = TreatyCode;
            Params.IsWithAdjustmentDetail = IsWithAdjustmentDetail ?? false;
            Params.HasRedFlag = HasRedFlag;
            Params.EntryNo = EntryNo;
            Params.SoaQuarter = SoaQuarter;
            Params.ClaimIdFilter = ClaimIdFilter;
            Params.ClaimTransactionType = ClaimTransactionType;
            Params.ClaimRecoveryAmount = ClaimRecoveryAmount;
            Params.IsReferralCase = IsReferralCase;
            Params.RiDataWarehouseId = RiDataWarehouseId;
            Params.RecordType = RecordType;
            Params.TreatyCodeFilter = TreatyCodeFilter;
            Params.PolicyNumberFilter = PolicyNumberFilter;
            Params.CedingCompany = CedingCompany;
            Params.ProvisionAt = ProvisionAt;
            var export = new GenerateExportData();
            ExportBo exportBo = IsSnapshotVersion.HasValue && IsSnapshotVersion.Value ? export.CreateExportClaimRegisterHistorySearch(AuthUserId, Params) : export.CreateExportClaimRegisterSearch(AuthUserId, Params);

            if (exportBo.Total <= export.ExportInstantDownloadLimit)
            {
                export.Process(ref exportBo, _db);
                Session.Add("DownloadExport", true);
            }

            return RedirectToAction("Edit", "Export", new { exportBo.Id });
        }

        public void LoadPage(int? cutOffId = null)
        {
            DropDownYesNo();
            DropDownYesNoWithSelect();
            DropDownCedantCode();
            DropDownCutOff(cutOffId);
            DropDownClaimTransactionType(true, false, true);

            GetTreatyCodes(foreign: false);

            ViewBag.StandardClaimDataOutputList = StandardClaimDataOutputService.Get();

            SetViewBagMessage();
        }
    }
}