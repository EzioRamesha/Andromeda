using BusinessObject;
using BusinessObject.Claims;
using BusinessObject.RiDatas;
using BusinessObject.SoaDatas;
using ConsoleApp.Commands;
using PagedList;
using Services;
using Services.Claims;
using Services.Identity;
using Services.RiDatas;
using Services.SoaDatas;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
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
    public class SoaDataController : BaseController
    {
        public const string Controller = "SoaData";

        // GET: SoaData
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            int? StatusId,
            int? CedantId,
            int? TreatyId,
            int? DirectRetroStatusId,
            int? InvoiceStatusId,
            int? DataUpdateStatusId,
            string UploadDate,
            string Quarter,
            string PersonInCharge,
            string SortOrder,
            int? Page
        )
        {
            IndexPage();

            DateTime? uploadDate = Util.GetParseDateTime(UploadDate);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["StatusId"] = StatusId,
                ["CedantId"] = CedantId,
                ["TreatyId"] = TreatyId,
                ["DirectRetroStatusId"] = DirectRetroStatusId,
                ["InvoiceStatusId"] = InvoiceStatusId,
                ["DataUpdateStatusId"] = DataUpdateStatusId,
                ["UploadDate"] = uploadDate.HasValue ? UploadDate : null,
                ["Quarter"] = Quarter,
                ["PersonInCharge"] = PersonInCharge,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCedantId = GetSortParam("CedantId");
            ViewBag.SortUploadDate = GetSortParam("UploadDate");
            ViewBag.SortTreatyId = GetSortParam("TreatyId");
            ViewBag.SortQuarter = GetSortParam("Quarter");
            ViewBag.SortPersonInCharge = GetSortParam("PersonInCharge");

            var hideDummySoaDataBatch = Util.GetConfigBoolean("HideDummySoaDataBatch", false);
            var query = _db.GetSoaDataBatches().Select(SoaDataBatchViewModel.Expression());
            if (hideDummySoaDataBatch)
            {
                var dummySoaDataBatch = Util.GetConfig("DummySoaDataBatchIds");
                var dummySoaDataBatchIds = Util.ToArraySplitTrim(dummySoaDataBatch);
                query = query.Where(q => !dummySoaDataBatchIds.Contains(q.Id.ToString()));
            }
            if (StatusId != null) query = query.Where(q => q.Status == StatusId);
            if (CedantId != null) query = query.Where(q => q.CedantId == CedantId);
            if (TreatyId != null) query = query.Where(q => q.TreatyId == TreatyId);
            if (!string.IsNullOrEmpty(Quarter)) query = query.Where(q => q.Quarter == Quarter);
            if (uploadDate.HasValue) query = query.Where(q => q.UploadDate == uploadDate);
            if (!string.IsNullOrEmpty(PersonInCharge)) query = query.Where(q => q.PersonInCharge.FullName.Contains(PersonInCharge));
            if (DirectRetroStatusId != null) query = query.Where(q => q.DirectRetroStatus == DirectRetroStatusId);
            if (InvoiceStatusId != null) query = query.Where(q => q.InvoiceStatus == InvoiceStatusId);
            if (DataUpdateStatusId != null) query = query.Where(q => q.DataUpdateStatus == DataUpdateStatusId);

            if (SortOrder == Html.GetSortAsc("CedantId")) query = query.OrderBy(q => q.Cedant.Code);
            else if (SortOrder == Html.GetSortDsc("CedantId")) query = query.OrderByDescending(q => q.Cedant.Code);
            else if (SortOrder == Html.GetSortAsc("UploadDate")) query = query.OrderBy(q => q.UploadDate);
            else if (SortOrder == Html.GetSortDsc("UploadDate")) query = query.OrderByDescending(q => q.UploadDate);
            else if (SortOrder == Html.GetSortAsc("TreatyId")) query = query.OrderBy(q => q.Treaty.TreatyIdCode);
            else if (SortOrder == Html.GetSortDsc("TreatyId")) query = query.OrderByDescending(q => q.Treaty.TreatyIdCode);
            else if (SortOrder == Html.GetSortAsc("Quarter")) query = query.OrderBy(q => q.Quarter);
            else if (SortOrder == Html.GetSortDsc("Quarter")) query = query.OrderByDescending(q => q.Quarter);
            else if (SortOrder == Html.GetSortAsc("PersonInCharge")) query = query.OrderBy(q => q.PersonInCharge.FullName);
            else if (SortOrder == Html.GetSortDsc("PersonInCharge")) query = query.OrderByDescending(q => q.PersonInCharge.FullName);
            else query = query.OrderBy(q => q.Quarter);

            ViewBag.Total = query.Count();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: SoaData/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            if (CheckCutOffReadOnly(Controller))
                return RedirectToAction("Index");

            SoaDataBatchViewModel model = new SoaDataBatchViewModel();
            model.PersonInChargeId = AuthUserId;
            model.PersonInChargeBo = UserService.Find(AuthUserId);

            LoadPage();
            return View(model);
        }

        // POST: SoaData/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, SoaDataBatchViewModel model)
        {
            if (ModelState.IsValid && !CheckCutOffReadOnly(Controller))
            {
                SoaDataBatchBo soaDataBatchBo = new SoaDataBatchBo();
                model.Get(ref soaDataBatchBo);                
                soaDataBatchBo.DataUpdateStatus = SoaDataBatchBo.DataUpdateStatusPending;
                soaDataBatchBo.CreatedById = AuthUserId;
                soaDataBatchBo.UpdatedById = AuthUserId;
                if (DirectRetroConfigurationService.CountByTreatyId(model.TreatyId.Value) > 0)
                    soaDataBatchBo.DirectStatus = SoaDataBatchBo.DirectStatusIncomplete;

                TrailObject trail = GetNewTrailObject();
                Result = SoaDataBatchService.Create(ref soaDataBatchBo, ref trail);
                if (Result.Valid)
                {
                    model.Id = soaDataBatchBo.Id;
                    model.Status = soaDataBatchBo.Status;

                    model.ProcessStatusHistory(form, AuthUserId, ref trail);
                    model.ProcessFileUpload(AuthUserId, ref trail);

                    if (model.RiDataBatchId.HasValue)
                    {
                        RiDataBatchBo riDataBatchBo = RiDataBatchService.Find(model.RiDataBatchId.Value);
                        if (riDataBatchBo != null)
                        {
                            riDataBatchBo.SoaDataBatchId = model.Id;
                            RiDataBatchService.Update(ref riDataBatchBo, ref trail);
                        }
                    }

                    if (model.ClaimDataBatchId.HasValue)
                    {
                        ClaimDataBatchBo claimDataBatchBo = ClaimDataBatchService.Find(model.ClaimDataBatchId.Value);
                        if (claimDataBatchBo != null)
                        {
                            claimDataBatchBo.SoaDataBatchId = model.Id;
                            ClaimDataBatchService.Update(ref claimDataBatchBo, ref trail);
                        }
                    }

                    CreateTrail(soaDataBatchBo.Id, "Create SOA Data Batch");

                    SetCreateSuccessMessage("SOA Data Batch");
                    return RedirectToAction("Edit", new { id = model.Id });
                }
                AddResult(Result);
            }

            model.Upload = null;
            model.PersonInChargeBo = UserService.Find(AuthUserId);
            LoadPage();            
            return View(model);
        }

        // GET: SoaData/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var hideDummySoaDataBatch = Util.GetConfigBoolean("HideDummySoaDataBatch", false);
            if (hideDummySoaDataBatch)
            {
                var dummySoaDataBatch = Util.GetConfig("DummySoaDataBatchIds");
                var dummySoaDataBatchIds = Util.ToArraySplitTrim(dummySoaDataBatch);
                if (dummySoaDataBatchIds.Contains(id.ToString()))
                    return RedirectToAction("Index");
            }

            SoaDataBatchBo soaDataBatchBo = SoaDataBatchService.Find(id);
            if (soaDataBatchBo == null)
            {
                return RedirectToAction("Index");
            }

            LoadPage(soaDataBatchBo);
            return View(new SoaDataBatchViewModel(soaDataBatchBo));
        }

        // POST: SoaData/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, SoaDataBatchViewModel model)
        {
            SoaDataBatchBo soaDataBatchBo = SoaDataBatchService.Find(id);
            if (soaDataBatchBo == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid && !CheckCutOffReadOnly(Controller, id))
            {
                bool statusChanged = false;
                if (model.Status != soaDataBatchBo.Status)
                    statusChanged = true;

                bool update = false;
                if (SoaDataBatchBo.CanUpdate(soaDataBatchBo.DataUpdateStatus))
                    update = true;

                bool canUpload = false;
                if (SoaDataBatchBo.CanUpload(soaDataBatchBo.Status))
                    canUpload = true;

                int? oldRiDataBatchId = soaDataBatchBo.RiDataBatchId;
                int? oldClaimDataBatchId = soaDataBatchBo.ClaimDataBatchId;

                model.Get(ref soaDataBatchBo);
                soaDataBatchBo.UpdatedById = AuthUserId;
                bool dataStatusChanged = false;
                if (update)
                {
                    if (model.DataUpdateStatus != soaDataBatchBo.DataUpdateStatus)
                        dataStatusChanged = true;
                    soaDataBatchBo.DataUpdateStatus = model.DataUpdateStatus;
                }
                if (soaDataBatchBo.DirectStatus == 0 && DirectRetroConfigurationService.CountByTreatyId(model.TreatyId.Value) > 0)
                    soaDataBatchBo.DirectStatus = SoaDataBatchBo.DirectStatusIncomplete;

                TrailObject trail = GetNewTrailObject();
                Result = SoaDataBatchService.Update(ref soaDataBatchBo, ref trail);
                if (Result.Valid)
                {
                    model.Id = soaDataBatchBo.Id;
                    model.Status = soaDataBatchBo.Status;

                    if (statusChanged)
                        model.ProcessStatusHistory(form, AuthUserId, ref trail);
                    if (dataStatusChanged)
                        model.ProcessStatusHistory(form, AuthUserId, ref trail, true);
                    if (canUpload && model.Upload != null)
                        model.ProcessFileUpload(AuthUserId, ref trail);
                    model.ProcessExistingFile(model.Mode, AuthUserId, ref trail);
                    model.UpdateCompiledSummary(form, AuthUserId, ref trail);
                    model.UpdatePostValidationDifferenceSummary(form, AuthUserId, ref trail);
                    if (model.Status == SoaDataBatchBo.StatusApproved)
                        model.UpdateClaimOffsetStatus(AuthUserId);

                    // If Ri Data Batch changed
                    if (oldRiDataBatchId != soaDataBatchBo.RiDataBatchId)
                    {
                        if (oldRiDataBatchId.HasValue)
                        {
                            RiDataBatchBo oldRiDataBatchBo = RiDataBatchService.Find(oldRiDataBatchId.Value);
                            if (oldRiDataBatchBo != null && oldRiDataBatchBo.SoaDataBatchId == model.Id)
                            {
                                oldRiDataBatchBo.SoaDataBatchId = null;
                                RiDataBatchService.Update(ref oldRiDataBatchBo, ref trail);
                            }
                        }

                        if (model.RiDataBatchId.HasValue)
                        {
                            RiDataBatchBo riDataBatchBo = RiDataBatchService.Find(model.RiDataBatchId.Value);
                            if (riDataBatchBo != null)
                            {
                                riDataBatchBo.SoaDataBatchId = model.Id;
                                RiDataBatchService.Update(ref riDataBatchBo, ref trail);
                            }
                        }
                    }

                    // If Claim Data Batch changed
                    if (oldClaimDataBatchId != soaDataBatchBo.ClaimDataBatchId)
                    {
                        if (oldClaimDataBatchId.HasValue)
                        {
                            ClaimDataBatchBo oldClaimDataBatchBo = ClaimDataBatchService.Find(oldClaimDataBatchId.Value);
                            if (oldClaimDataBatchBo != null && oldClaimDataBatchBo.SoaDataBatchId == model.Id)
                            {
                                oldClaimDataBatchBo.SoaDataBatchId = null;
                                ClaimDataBatchService.Update(ref oldClaimDataBatchBo, ref trail);
                            }
                        }

                        if (model.ClaimDataBatchId.HasValue)
                        {
                            ClaimDataBatchBo claimDataBatchBo = ClaimDataBatchService.Find(model.ClaimDataBatchId.Value);
                            if (claimDataBatchBo != null)
                            {
                                claimDataBatchBo.SoaDataBatchId = model.Id;
                                ClaimDataBatchService.Update(ref claimDataBatchBo, ref trail);
                            }
                        }
                    }

                    CreateTrail(soaDataBatchBo.Id, "Update SOA Data Batch");

                    SetUpdateSuccessMessage("SOA Data Batch");
                    return RedirectToAction("Edit", new { id = model.Id });
                }
                AddResult(Result);
            }

            model.Upload = null;

            soaDataBatchBo = SoaDataBatchService.Find(id);
            model.SetBos(soaDataBatchBo);
            LoadPage(soaDataBatchBo);
            return View(new SoaDataBatchViewModel(soaDataBatchBo));
        }

        // GET: SoaData/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            SoaDataBatchBo soaDataBatchBo = SoaDataBatchService.Find(id);
            if (soaDataBatchBo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new SoaDataBatchViewModel(soaDataBatchBo));
        }

        // POST: SoaData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id)
        {
            SoaDataBatchBo soaDataBatchBo = SoaDataBatchService.Find(id);
            if (soaDataBatchBo == null)
            {
                return RedirectToAction("Index");
            }

            int? riDataBatchId = soaDataBatchBo.RiDataBatchId;
            int? claimDataBatchId = soaDataBatchBo.ClaimDataBatchId;

            soaDataBatchBo.Status = SoaDataBatchBo.StatusPendingDelete;
            soaDataBatchBo.RiDataBatchId = null;
            soaDataBatchBo.ClaimDataBatchId = null;
            soaDataBatchBo.UpdatedById = AuthUserId;

            TrailObject trail = GetNewTrailObject();
            Result = SoaDataBatchService.Update(ref soaDataBatchBo, ref trail);
            if (Result.Valid)
            {
                if (riDataBatchId.HasValue)
                {
                    RiDataBatchBo riDataBatchBo = RiDataBatchService.Find(riDataBatchId.Value);
                    if (riDataBatchBo != null && riDataBatchBo.SoaDataBatchId.HasValue)
                    {
                        riDataBatchBo.SoaDataBatchId = null;
                        RiDataBatchService.Update(ref riDataBatchBo, ref trail);
                    }
                }

                if (claimDataBatchId.HasValue)
                {
                    ClaimDataBatchBo claimDataBatchBo = ClaimDataBatchService.Find(claimDataBatchId.Value);
                    if (claimDataBatchBo != null && claimDataBatchBo.SoaDataBatchId.HasValue)
                    {
                        claimDataBatchBo.SoaDataBatchId = null;
                        ClaimDataBatchService.Update(ref claimDataBatchBo, ref trail);
                    }
                }

                CreateTrail(soaDataBatchBo.Id, "Update SOA Data Batch");

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = soaDataBatchBo.Id });
        }

        public ActionResult SoaDataReinsurance(int id, bool originalCurrency, int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary { ["Id"] = id, ["OriginalCurrency"] = originalCurrency };

            if (!originalCurrency)
            {
                _db.Database.CommandTimeout = 0;
                var query = _db.SoaData.AsNoTracking().Where(q => q.SoaDataBatchId == id && q.BusinessCode != PickListDetailBo.BusinessOriginCodeServiceFee && (string.IsNullOrEmpty(q.CurrencyCode) || q.CurrencyCode == PickListDetailBo.CurrencyCodeMyr))
                        .Select(SoaDataReinsuranceViewModel.Expression())
                        .OrderBy(q => q.CompanyName);
                ViewBag.Total = query.Count();

                return PartialView("_SoaDataDetailsReinsurance", query.ToPagedList(Page ?? 1, PageSize));
            }
            else
            {
                _db.Database.CommandTimeout = 0;
                var query = _db.SoaData.AsNoTracking().Where(q => q.SoaDataBatchId == id && q.BusinessCode != PickListDetailBo.BusinessOriginCodeServiceFee && (!string.IsNullOrEmpty(q.CurrencyCode) && q.CurrencyCode != PickListDetailBo.CurrencyCodeMyr))
                        .Select(SoaDataReinsuranceViewModel.Expression())
                        .OrderBy(q => q.CompanyName);
                ViewBag.Total = query.Count();

                return PartialView("_SoaDataDetailsReinsurance", query.ToPagedList(Page ?? 1, PageSize));
            }
        }

        public ActionResult SoaDataRetakaful(int id, int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary { ["Id"] = id };
             
            _db.Database.CommandTimeout = 0;
            var query = _db.SoaData.AsNoTracking().Where(q => q.SoaDataBatchId == id && q.BusinessCode == PickListDetailBo.BusinessOriginCodeServiceFee)
                    .Select(SoaDataRetakafulViewModel.Expression())
                    .OrderBy(q => q.CompanyName);
            ViewBag.Total = query.Count();

            return PartialView("_SoaDataDetailsRetakaful", query.ToPagedList(Page ?? 1, PageSize));
        }

        public ActionResult SoaDataValidation(int id, int type, bool originalCurrency)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Id"] = id,
                ["Type"] = type,
                ["OriginalCurrency"] = originalCurrency,
            };

            IList<SoaDataRiDataSummaryBo> SoaDataBos = null;
            IList<SoaDataRiDataSummaryBo> RiSummaryBos = null;
            IList<SoaDataRiDataSummaryBo> DifferenceBos = null;

            SoaDataBos = SoaDataRiDataSummaryService.GetBySoaDataBatchIdBusinessCode(id, SoaDataRiDataSummaryBo.TypeSoaData, type, originalCurrency);
            RiSummaryBos = SoaDataRiDataSummaryService.GetBySoaDataBatchIdBusinessCode(id, SoaDataRiDataSummaryBo.TypeRiDataSummary, type, originalCurrency);
            DifferenceBos = SoaDataRiDataSummaryService.GetBySoaDataBatchIdBusinessCode(id, SoaDataRiDataSummaryBo.TypeDifferences, type, originalCurrency);

            var model = new SoaDataValidationListingViewModel();
            model.SoaDatas = SoaDataBos.AsQueryable().Select(SoaDataValidationViewModel.ExpressionRi()).ToList();
            model.RiSummarys = RiSummaryBos.AsQueryable().Select(SoaDataValidationViewModel.ExpressionRi()).ToList();
            model.Differences = DifferenceBos.AsQueryable().Select(SoaDataValidationViewModel.ExpressionRi()).ToList();

            return PartialView("_SoaDataDetailsValidation", model);
        }

        public ActionResult SoaDataPostValidationSummary(int id, int type, bool originalCurrency)
        {
            IList<SoaDataPostValidationBo> MLReCheckingBos = null;
            IList<SoaDataPostValidationBo> CedantAmountBos = null;
            IList<SoaDataPostValidationBo> DiscrepancyChecksBos = null;
            IList<SoaDataPostValidationDifferenceBo> DifferenceBos = null;
            IList<SoaDataDiscrepancyBo> DiscrepancyBos = null;
            int? index = null;

            switch (type)
            {
                case 1:
                    MLReCheckingBos = SoaDataPostValidationService.GetBySoaDataBatchIdTypeCurrencyCode(id, SoaDataPostValidationBo.TypeMlreShareMlreChecking, originalCurrency);
                    CedantAmountBos = SoaDataPostValidationService.GetBySoaDataBatchIdTypeCurrencyCode(id, SoaDataPostValidationBo.TypeMlreShareCedantAmount, originalCurrency);
                    DiscrepancyChecksBos = SoaDataPostValidationService.GetBySoaDataBatchIdTypeCurrencyCode(id, SoaDataPostValidationBo.TypeMlreShareDiscrepancyCheck, originalCurrency);
                    DifferenceBos = SoaDataPostValidationDifferenceService.GetBySoaDataBatchIdType(id, SoaDataPostValidationDifferenceBo.TypeMlreShare, originalCurrency);
                    DiscrepancyBos = SoaDataDiscrepancyService.GetBySoaDataBatchIdType(id, SoaDataDiscrepancyBo.TypeMlreShare, originalCurrency);

                    int totalCount = SoaDataPostValidationDifferenceService.CountBySoaDataBatchIdType(id, SoaDataPostValidationBo.TypeMlreShareMlreChecking, true);
                    if (totalCount > 0 && originalCurrency) index = totalCount;
                    else index = 0;
                    break;
                case 2:
                    MLReCheckingBos = SoaDataPostValidationService.GetBySoaDataBatchIdTypeCurrencyCode(id, SoaDataPostValidationBo.TypeLayer1ShareMlreChecking, originalCurrency);
                    CedantAmountBos = SoaDataPostValidationService.GetBySoaDataBatchIdTypeCurrencyCode(id, SoaDataPostValidationBo.TypeLayer1ShareCedantAmount, originalCurrency);
                    DiscrepancyChecksBos = SoaDataPostValidationService.GetBySoaDataBatchIdTypeCurrencyCode(id, SoaDataPostValidationBo.TypeLayer1ShareDiscrepancyCheck, originalCurrency);
                    DifferenceBos = SoaDataPostValidationDifferenceService.GetBySoaDataBatchIdType(id, SoaDataPostValidationDifferenceBo.TypeLayer1Share, originalCurrency);
                    DiscrepancyBos = SoaDataDiscrepancyService.GetBySoaDataBatchIdType(id, SoaDataDiscrepancyBo.TypeLayer1Share, originalCurrency);

                    totalCount = SoaDataPostValidationDifferenceService.CountBySoaDataBatchIdType(id, SoaDataPostValidationBo.TypeMlreShareMlreChecking);
                    if (totalCount > 0) index = totalCount;
                    else index = 0;
                    break;
                case 3:
                    MLReCheckingBos = SoaDataPostValidationService.GetBySoaDataBatchIdTypeCurrencyCode(id, SoaDataPostValidationBo.TypeRetakafulShareMlreChecking, originalCurrency);
                    CedantAmountBos = SoaDataPostValidationService.GetBySoaDataBatchIdTypeCurrencyCode(id, SoaDataPostValidationBo.TypeRetakafulShareCedantAmount, originalCurrency);
                    DiscrepancyChecksBos = SoaDataPostValidationService.GetBySoaDataBatchIdTypeCurrencyCode(id, SoaDataPostValidationBo.TypeRetakafulShareDiscrepancyCheck, originalCurrency);
                    DifferenceBos = SoaDataPostValidationDifferenceService.GetBySoaDataBatchIdType(id, SoaDataPostValidationDifferenceBo.TypeRetakaful, originalCurrency);
                    DiscrepancyBos = SoaDataDiscrepancyService.GetBySoaDataBatchIdType(id, SoaDataDiscrepancyBo.TypeRetakaful, originalCurrency);

                    int totalCountMlre = SoaDataPostValidationDifferenceService.CountBySoaDataBatchIdType(id, SoaDataPostValidationBo.TypeMlreShareMlreChecking);
                    int totalCountLayer1 = SoaDataPostValidationDifferenceService.CountBySoaDataBatchIdType(id, SoaDataPostValidationBo.TypeMlreShareMlreChecking);
                    if (totalCountMlre > 0 && totalCountLayer1 > 0) index = (totalCountMlre + totalCountLayer1);
                    else if (totalCountMlre > 0 && totalCountLayer1 == 0) index = totalCountMlre;
                    else index = 0;
                    break;
            }

            var model = new SoaDataPostValidationListingViewModel();
            model.MLReCheckings = MLReCheckingBos.AsQueryable().Select(SoaDataPostValidationViewModel.Expression()).ToList();
            model.CedantAmounts = CedantAmountBos.AsQueryable().Select(SoaDataPostValidationViewModel.Expression()).ToList();        
            model.DiscrepancyChecks = DiscrepancyChecksBos.AsQueryable().Select(SoaDataPostValidationViewModel.Expression()).ToList();
            model.Differences = DifferenceBos.AsQueryable().Select(SoaDataPostValidationDifferencesViewModel.Expression()).ToList();
            model.Discrepancies = DiscrepancyBos.AsQueryable().Select(SoaDataPostValidationDiscrepancyViewModel.Expression()).ToList();

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Id"] = id,
                ["Type"] = type,
                ["Index"] = index,
                ["OriginalCurrency"] = originalCurrency,
            };

            return PartialView("_SoaDataDetailsPostValidationSummary", model);
        }

        public void IndexPage()
        {
            CheckCutOffReadOnly(Controller);

            DropDownEmpty();
            DropDownCedant();
            DropDownBatchStatus();
            DropDownDirectRetroStatus();
            DropDownInvoiceStatus();
            DropDownDataUpdateStatus();
            SetViewBagMessage();
        }

        public void LoadPage(SoaDataBatchBo soaDataBatchBo = null)
        {
            int? selectedCedantId = soaDataBatchBo == null ? (int?)null : soaDataBatchBo.CedantId;

            DropDownEmpty();
            DropDownCurrencyCode();
            DropDownType();
            DropDownCedant(CedantBo.StatusActive, selectedCedantId);
            DropDownReportingType();
            DropDownBatchStatus();

            if (soaDataBatchBo != null)
            {
                if (soaDataBatchBo.CedantBo != null && soaDataBatchBo.CedantBo.Status == CedantBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.CedantStatusInactive);
                }

                ModuleBo moduleBo = GetModuleByController(ModuleBo.ModuleController.SoaData.ToString());                
                ViewBag.SoaDataBatchStatusFileBos = SoaDataBatchStatusFileService.GetSoaDataBatchStatusBySoaDataBatchId(soaDataBatchBo.Id);
                ViewBag.StatusHistoryBos = StatusHistoryService.GetStatusHistoriesByModuleIdObjectId(moduleBo.Id, soaDataBatchBo.Id);
                ViewBag.RemarkBos = RemarkService.GetByModuleIdObjectId(moduleBo.Id, soaDataBatchBo.Id);

                IList<SoaDataFileBo> soaFileHistoryBos = SoaDataFileService.GetBySoaDataBatchId(soaDataBatchBo.Id);
                ViewBag.FileHistoryBos = soaFileHistoryBos;
                ViewBag.CheckedExcludes = soaFileHistoryBos.Where(q => q.Mode == SoaDataFileBo.ModeExclude).Select(q => q.Id).ToArray();

                //ViewBag.WMCompiledSummaryMYRBos = SoaDataCompiledSummaryService.GetBySoaDataBatchIdCode(soaDataBatchBo.Id, false);
                //ViewBag.WMCompiledSummaryOCBos = SoaDataCompiledSummaryService.GetBySoaDataBatchIdCode(soaDataBatchBo.Id, false, true);
                //ViewBag.SFCompiledSummaryBos = SoaDataCompiledSummaryService.GetBySoaDataBatchIdCode(soaDataBatchBo.Id, true);
                AccountForDropDownItems(soaDataBatchBo);

                bool cutOffReadOnly = CheckCutOffReadOnly(Controller, soaDataBatchBo?.Id);

                ViewBag.ShowSubmitForProcessing = SoaDataBatchBo.CanSubmitForProcessing(soaDataBatchBo.Status);
                ViewBag.ShowSubmitForDataUpdate = SoaDataBatchBo.CanSubmitForDataUpdate(soaDataBatchBo.Status);
                ViewBag.ShowSubmitForApproval = SoaDataBatchBo.CanSubmitForApproval(soaDataBatchBo.Status);
                ViewBag.ShowApproveReject = SoaDataBatchBo.CanApproveReject(soaDataBatchBo.Status);
                ViewBag.ShowSave = SoaDataBatchBo.CanSave(soaDataBatchBo.Status);

                ViewBag.DisabledDataUpdate = cutOffReadOnly ? true : !SoaDataBatchBo.CanUpdate(soaDataBatchBo.DataUpdateStatus);
                ViewBag.DisableUpload = !SoaDataBatchBo.CanUpload(soaDataBatchBo.Status);
                ViewBag.DisableSave = cutOffReadOnly;
                bool disabled = !SoaDataBatchBo.CanProcess(soaDataBatchBo.Status);
                if (!disabled)
                {
                    disabled = cutOffReadOnly;
                }
                ViewBag.Disabled = disabled;

                // disable approve/reject button
                var userBo = UserService.Find(AuthUserId);

                int userAccessGroupId = 0;
                if (userBo.UserAccessGroupBos != null && userBo.UserAccessGroupBos.Count > 0)
                    userAccessGroupId = userBo.UserAccessGroupBos[0].AccessGroupId;

                var disabledApproveReject = false;
                if (AuthorizationLimitService.CountByAccessGroupId(userAccessGroupId) == 0)
                {
                    disabledApproveReject = true;
                }
                ViewBag.DisabledApproveReject = disabledApproveReject;

                if (soaDataBatchBo.Status == SoaDataBatchBo.StatusConditionalApproval 
                        || soaDataBatchBo.Status == SoaDataBatchBo.StatusProvisionalApproval)
                {
                    if (soaDataBatchBo.RiDataBatchId.HasValue && soaDataBatchBo.RiDataBatchBo?.Status != RiDataBatchBo.StatusFinalised)
                        AddWarningMsg(string.Format("The RI Data batch status is {0}. Please submit for finalise the RI data batch to proceed with invoicing.", RiDataBatchBo.GetStatusName((int)(soaDataBatchBo.RiDataBatchBo?.Status))));
                }                    
            }
            ViewBag.AuthUserName = AuthUser.FullName;

            List<string> allStatusItems = new List<string>();
            for (int i = 0; i <= SoaDataCompiledSummaryBo.InvoiceTypeMax; i++)
            {
                allStatusItems.Add(SoaDataCompiledSummaryBo.GetInvoiceTypeName(i));
            }
            ViewBag.InvoiceTypeItems = allStatusItems;

            SetViewBagMessage();
        }
        
        public List<SelectListItem> DropDownBatchStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, SoaDataBatchBo.StatusMax)) 
            {
                items.Add(new SelectListItem { Text = SoaDataBatchBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.StatusItems = items;
            return items;
        }

        public List<SelectListItem> DropDownType()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, SoaDataBatchBo.TypeMax))
            {
                items.Add(new SelectListItem { Text = SoaDataBatchBo.GetTypeName(i), Value = i.ToString() });
            }
            ViewBag.TypeItems = items;
            return items;
        }

        public List<SelectListItem> DropDownDirectRetroStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, SoaDataBatchBo.DirectStatusMax))
            {
                items.Add(new SelectListItem { Text = SoaDataBatchBo.GetDirectStatusName(i), Value = i.ToString() });
            }
            ViewBag.DirectRetroStatusItems = items;
            return items;
        }

        public List<SelectListItem> DropDownInvoiceStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, SoaDataBatchBo.InvoiceStatusMax))
            {
                items.Add(new SelectListItem { Text = SoaDataBatchBo.GetInvoiceStatusName(i), Value = i.ToString() });
            }
            ViewBag.InvoiceStatusItems = items;
            return items;
        }

        public List<SelectListItem> DropDownDataUpdateStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, SoaDataBatchBo.DataUpdateStatusMax))
            {
                int dataUpdateKey = (100 + i);
                items.Add(new SelectListItem { Text = SoaDataBatchBo.GetStatusName(dataUpdateKey), Value = dataUpdateKey.ToString() });
            }
            ViewBag.DataUpdateStatusItems = items;
            return items;
        }

        public List<SelectListItem> DropDownReportingType()
        {
            var items = GetEmptyDropDownList(false);
            foreach (var i in Enumerable.Range(1, SoaDataCompiledSummaryBo.ReportingTypeMax))
            {
                items.Add(new SelectListItem { Text = SoaDataCompiledSummaryBo.GetReportingTypeName(i), Value = i.ToString() });
            }
            ViewBag.ReportingTypeItems = items;
            return items;
        }

        [HttpPost]
        public JsonResult GetCompiledSummaryByReportingType(int SoaDataBatchId, int ReportingTypeId)
        {
            var WMCompiledSummaryMYRBos = SoaDataCompiledSummaryService.GetBySoaDataBatchIdCode(SoaDataBatchId, ReportingTypeId, false);
            var WMCompiledSummaryOCBos = SoaDataCompiledSummaryService.GetBySoaDataBatchIdCode(SoaDataBatchId, ReportingTypeId, false, true);
            var SFCompiledSummaryBos = SoaDataCompiledSummaryService.GetBySoaDataBatchIdCode(SoaDataBatchId, ReportingTypeId, true);

            return Json(new { WMCompiledSummaryMYRBos, WMCompiledSummaryOCBos, SFCompiledSummaryBos });
        }

        [HttpPost]
        public JsonResult GetTreatyByCedant(int cedantId)
        {
            var treatyBos = TreatyService.GetByCedantId(cedantId);
            if (treatyBos != null)
                treatyBos = treatyBos.Where(q => q.BusinessOriginPickListDetailId.HasValue).ToList();
            return Json(new { treatyBos });
        }

        [HttpPost]
        public JsonResult GetBusinessOriginType(int treatyId)
        {
            var treatyBos = TreatyService.Find(treatyId);
            var typeKey = SoaDataBatchBo.GetType((treatyBos.BusinessOriginPickListDetailBo == null ? "" : treatyBos.BusinessOriginPickListDetailBo.Code));
            var typeName = SoaDataBatchBo.GetTypeName(typeKey);
            return Json(new { typeKey, typeName });
        }

        [HttpPost]
        public JsonResult SearchRiDataBatch(int? CedantId, int? TreatyId, string Quarter)
        {
            var riDataBatchBos = RiDataBatchService.GetByParam(CedantId, TreatyId, Quarter);
            return Json(new { riDataBatchBos });
        }

        [HttpPost]
        public JsonResult SearchClaimDataBatch(int? CedantId, int? TreatyId, string Quarter)
        {
            var claimDataBatchBos = ClaimDataBatchService.GetByParam(CedantId, TreatyId, Quarter);
            return Json(new { claimDataBatchBos });
        }

        public ActionResult DownloadSoaDataFile(int rawFileId)
        {
            RawFileBo rawFileBo = RawFileService.Find(rawFileId);
            DownloadFile(rawFileBo.GetLocalPath(), rawFileBo.FileName);
            return null;
        }

        public ActionResult DownloadSoaDataExcelFile()
        {
            var filepath = Util.GetWebAppDocumentFilePath("SoaData_Template.xlsx");
            return File(filepath, MimeMapping.GetMimeMapping(filepath), Path.GetFileName(filepath));
        }

        public ActionResult DownloadSoaDataValidation(int soaDataBatchId, int type, bool originalCurrency = false)
        {
            Response.SetCookie(new HttpCookie("downloadToken", ""));
            Session["lastActivity"] = DateTime.Now.Ticks;

            var process = new GenerateSoaDataValidation();
            process.SoaDataBatchId = soaDataBatchId;
            process.Type = type;
            process.OriginalCurrency = originalCurrency;

            process.TypeRetakaful = type == 2 ? true : false;

            process.Process();

            DownloadFile(process.FilePath, Path.GetFileName(process.FilePath));
            return null;
        }

        public ActionResult DownloadSoaDataPostValidation(int soaDataBatchId, int type, bool originalCurrency = false)
        {
            Response.SetCookie(new HttpCookie("downloadToken", ""));
            Session["lastActivity"] = DateTime.Now.Ticks;

            var process = new GenerateSoaDataPostValidation();
            process.SoaDataBatchId = soaDataBatchId;
            process.Type = type;
            process.OriginalCurrency = originalCurrency;

            process.Process();

            DownloadFile(process.FilePath, Path.GetFileName(process.FilePath));
            return null;
        }

        public ActionResult DownloadSoaDataCompiledSummary(int soaDataBatchId)
        {
            var process = new GenerateSoaDataCompiledSummary();
            process.SoaDataBatchId = soaDataBatchId;

            process.Process();

            DownloadFile(process.FilePath, Path.GetFileName(process.FilePath));
            return null;
        }

        public void DownloadFile(string filePath, string fileName)
        {
            // For download big file
            try
            {
                Response.ClearContent();
                Response.Clear();
                //Response.Buffer = true;                                                                                                                   
                //Response.BufferOutput = false;
                Response.ContentType = MimeMapping.GetMimeMapping(fileName);
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                Response.TransmitFile(filePath);
                Response.Flush();
                Response.Close();
                Response.End();
            }
            catch
            {
                Response.Flush();
                Response.Close();
                Response.End();
            }
        }

        public void AccountForDropDownItems(SoaDataBatchBo bo)
        {
            Dictionary<string, List<SelectListItem>> items = new Dictionary<string, List<SelectListItem>>();
            var tc = SoaDataCompiledSummaryService.GetTreatyCodeBySoaDataBatchId(bo.Id);
            if (tc != null)
            {
                if (bo.TreatyId.HasValue)
                {
                    foreach (var b in tc)
                    {
                        List<SelectListItem> selectList = GetEmptyDropDownList();
                        if (!string.IsNullOrEmpty(b))
                        {
                            var treatyCodeBo = TreatyCodeService.FindByTreatyIdCode(bo.TreatyId.Value, b);
                            if (treatyCodeBo != null)
                            {
                                if (!string.IsNullOrEmpty(treatyCodeBo.AccountFor))
                                {
                                    string[] accounts = treatyCodeBo.AccountFor.Split(',').ToArray();
                                    foreach (string code in accounts)
                                    {
                                        selectList.Add(new SelectListItem { Text = code, Value = code });
                                    }
                                }
                            }
                        }
                        items.Add(!string.IsNullOrEmpty(b) ? b : "NULL", selectList);
                    }
                }

            }
            ViewBag.AccountDropDownItems = items;
        }


        // Ajax Functions called by other modules
        [HttpPost]
        public JsonResult GetSoaDataBatch(int? CedantId, int? TreatyId, string Quarter)
        {
            IList<SoaDataBatchBo> soaDataBatchBos = SoaDataBatchService.GetNotSOAApproved(CedantId, TreatyId, Quarter);
            return Json(new { soaDataBatchBos });
        }

        [HttpPost]
        public JsonResult GetSoaDataBatchByStr(string CedingCompany, string Quarter, string TreatyCode)
        {
            int? cedantId = CedantService.FindByCode(CedingCompany)?.Id;
            int? treatyId = TreatyCodeService.FindByCode(TreatyCode)?.TreatyId;

            return GetSoaDataBatch(cedantId, treatyId, Quarter);
        }

        [HttpPost]
        public JsonResult CreateSoaDataBatchByStr(string CedingCompany, string Quarter, string TreatyCode)
        {
            int? cedantId = CedantService.FindByCode(CedingCompany)?.Id;
            int? treatyId = TreatyCodeService.FindByCode(TreatyCode)?.TreatyId;

            return CreateSoaDataBatch(cedantId, treatyId, Quarter);
        }

        [HttpPost]
        public JsonResult CreateSoaDataBatch(int? CedantId, int? TreatyId, string Quarter, int? ClaimDataBatchId = null, int? RiDataBatchId = null)
        {
            bool success = true;
            int? resultId = null;
            string message = "";
            try
            {
                var treatyBo = TreatyService.Find(TreatyId);
                if (treatyBo.BusinessOriginPickListDetailId == null)
                {
                    throw new Exception("Business Origin in Treaty Id is empty");
                }

                ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.SoaData.ToString());
                PickListDetailBo pickListDetailBo = PickListDetailService.FindByPickListIdCode(PickListBo.CurrencyCode, PickListDetailBo.CurrencyCodeMyr);

                SoaDataBatchBo bo = new SoaDataBatchBo()
                {
                    CedantId = CedantId.Value,
                    TreatyId = TreatyId.Value,
                    Quarter = Quarter,
                    Status = SoaDataBatchBo.StatusSubmitForProcessing,
                    DataUpdateStatus = SoaDataBatchBo.DataUpdateStatusPending,
                    StatementReceivedAt = DateTime.Parse(DateTime.Now.ToString(Util.GetDateFormat())),
                    ClaimDataBatchId = ClaimDataBatchId != 0 ? ClaimDataBatchId : null,
                    RiDataBatchId = RiDataBatchId != 0 ? RiDataBatchId : null,
                    CreatedById = AuthUserId,
                    UpdatedById = AuthUserId,
                };
                bo.CurrencyCodePickListDetailId = pickListDetailBo?.Id;
                bo.CurrencyRate = 1;

                if (DirectRetroConfigurationService.CountByTreatyId(TreatyId.Value) > 0)
                    bo.DirectStatus = SoaDataBatchBo.DirectStatusIncomplete;

                if (RiDataBatchId.HasValue || RiDataBatchId == 0)
                    bo.IsRiDataAutoCreate = true;

                bo.IsClaimDataAutoCreate = true;

                TrailObject trail = GetNewTrailObject();
                Result = SoaDataBatchService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    StatusHistoryBo statusHistoryBo = new StatusHistoryBo
                    {
                        ModuleId = moduleBo.Id,
                        ObjectId = bo.Id,
                        Status = bo.Status,
                        CreatedById = AuthUserId,
                        UpdatedById = AuthUserId,
                    };
                    StatusHistoryService.Save(ref statusHistoryBo, ref trail);

                    resultId = bo.Id;
                    CreateTrail(bo.Id, "Create SOA Data Batch");
                }
                else
                {
                    success = false;
                    message = Result.MessageBag.Errors[0];
                }
            }
            catch (Exception ex)
            {
                success = false;
                message = ex.ToString();
            }

            return Json(new { success, resultId, message });
        }

        public ActionResult DownloadHtmlTable(int id, int type, string rows)
        {
            List<string> errors = new List<string>();

            string fileName = string.Format("{0}_{1}", SoaDataBatchBo.GetTabName(type), id).AppendDateTimeFileName(".xlsx");
            var directory = Util.GetSoaDataExportGenerationPath();
            string path = Path.Combine(directory, fileName);

            var arrRows = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(rows);

            try
            {
                switch (type)
                {
                    case SoaDataBatchBo.TabSoaValidationReinsuranceMyr:
                    case SoaDataBatchBo.TabSoaValidationReinsuranceOri:
                    case SoaDataBatchBo.TabSoaValidationRetakaful:
                        var processValidation = new GenerateSoaDataValidation()
                        {
                            SoaDataBatchId = id,
                            FileName = fileName,
                            FilePath = path,
                            StringRows = arrRows,
                            Directory = directory
                        };
                        processValidation.Process();
                        break;
                    case SoaDataBatchBo.TabSoaPostValidationMLReShareOri:
                    case SoaDataBatchBo.TabSoaPostValidationMLReShareMyr:
                    case SoaDataBatchBo.TabSoaPostValidationLayerShare:
                    case SoaDataBatchBo.TabSoaPostValidationRetakaful:
                        var processPostValidation = new GenerateSoaDataPostValidation()
                        {
                            SoaDataBatchId = id,
                            FileName = fileName,
                            FilePath = path,
                            StringRows = arrRows,
                            Directory = directory
                        };
                        processPostValidation.Process();
                        break;
                    case SoaDataBatchBo.TabSoaCompiledSummaryIfrs4:
                    case SoaDataBatchBo.TabSoaCompiledSummaryIfrs17:
                        var processCompiledSummary = new GenerateSoaDataCompiledSummary()
                        {
                            SoaDataBatchId = id,
                            FileName = fileName,
                            FilePath = path,
                            StringRows = arrRows,
                            Directory = directory
                        };
                        processCompiledSummary.Process();
                        break;
                }

            }
            catch (Exception ex)
            {

                errors.Add(ex.Message);
            }

            return Json(new { errors, fileName, path });
        }

        public ActionResult DownloadSoaDataHtmlTable(string fileName)
        {
            string path = Path.Combine(Util.GetSoaDataExportGenerationPath(), fileName);

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