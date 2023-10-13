using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.Retrocession;
using ConsoleApp.Commands;
using ConsoleApp.Commands.ProcessDatas;
using ConsoleApp.Commands.ProcessDatas.Exports;
using Ionic.Zip;
using PagedList;
using Services;
using Services.Retrocession;
using Services.RiDatas;
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
    public class PerLifeSoaController : BaseController
    {
        public const string Controller = "PerLifeSoa";

        // GET: PerLifeSoa
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            int? RetroPartyId, 
            int? RetroTreatyId, 
            string SoaQuarter,
            string ProcessingDate,
            int? PersonInChargeId,
            int? InvoiceStatus,
            int? Status, 
            bool? HasPerLifeProfitComm,
            string SortOrder, 
            int? Page)
        {
            DateTime? processingDate = Util.GetParseDateTime(ProcessingDate);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["RetroPartyId"] = RetroPartyId,
                ["RetroTreatyId"] = RetroTreatyId,
                ["SoaQuarter"] = SoaQuarter,
                ["ProcessingDate"] = processingDate.HasValue ? ProcessingDate : null,
                ["PersonInChargeId"] = PersonInChargeId,
                ["HasPerLifeProfitComm"] = HasPerLifeProfitComm,
                ["InvoiceStatus"] = InvoiceStatus,
                ["Status"] = Status,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortRetroParty = GetSortParam("RetroPartyId");
            ViewBag.SortRetroTreaty = GetSortParam("RetroTreatyId");
            ViewBag.SortQuarter = GetSortParam("SoaQuarter");
            ViewBag.SortProcessingDate = GetSortParam("ProcessingDate");
            ViewBag.SortPersonInChargeId = GetSortParam("PersonInChargeId");
            ViewBag.SortHasPerLifeProfitComm = GetSortParam("HasPerLifeProfitComm");
            ViewBag.SortInvoiceStatus = GetSortParam("InvoiceStatus");
            ViewBag.SortStatus = GetSortParam("Status");

            var query = _db.PerLifeSoa.Select(PerLifeSoaViewModel.Expression());
            if (RetroPartyId.HasValue) query = query.Where(q => q.RetroPartyId == RetroPartyId);
            if (RetroTreatyId.HasValue) query = query.Where(q => q.RetroTreatyId == RetroTreatyId);
            if (!string.IsNullOrEmpty(SoaQuarter)) query = query.Where(q => q.SoaQuarter == SoaQuarter);
            if (processingDate.HasValue) query = query.Where(q => q.ProcessingDate == processingDate);
            if (PersonInChargeId.HasValue) query = query.Where(q => q.PersonInChargeId == PersonInChargeId);
            if (InvoiceStatus.HasValue) query = query.Where(q => q.InvoiceStatus == InvoiceStatus);
            if (Status.HasValue) query = query.Where(q => q.Status == Status);
            if (HasPerLifeProfitComm.HasValue) query = query.Where(q => q.IsProfitCommissionData == HasPerLifeProfitComm);

            if (SortOrder == Html.GetSortAsc("RetroPartyId")) query = query.OrderBy(q => q.RetroParty.Code);
            else if (SortOrder == Html.GetSortDsc("RetroPartyId")) query = query.OrderByDescending(q => q.RetroParty.Code);
            else if (SortOrder == Html.GetSortAsc("RetroTreatyId")) query = query.OrderBy(q => q.RetroTreaty.Code);
            else if (SortOrder == Html.GetSortDsc("RetroTreatyId")) query = query.OrderByDescending(q => q.RetroTreaty.Code);
            else if (SortOrder == Html.GetSortAsc("SoaQuarter")) query = query.OrderBy(q => q.SoaQuarter);
            else if (SortOrder == Html.GetSortDsc("SoaQuarter")) query = query.OrderByDescending(q => q.SoaQuarter);
            else if (SortOrder == Html.GetSortAsc("ProcessingDate")) query = query.OrderBy(q => q.ProcessingDate);
            else if (SortOrder == Html.GetSortDsc("ProcessingDate")) query = query.OrderByDescending(q => q.ProcessingDate);
            else if (SortOrder == Html.GetSortAsc("PersonInChargeId")) query = query.OrderBy(q => q.PersonInCharge.FullName);
            else if (SortOrder == Html.GetSortDsc("PersonInChargeId")) query = query.OrderByDescending(q => q.PersonInCharge.FullName);
            else if (SortOrder == Html.GetSortAsc("HasPerLifeProfitComm")) query = query.OrderBy(q => q.IsProfitCommissionData);
            else if (SortOrder == Html.GetSortDsc("HasPerLifeProfitComm")) query = query.OrderByDescending(q => q.IsProfitCommissionData);
            else if (SortOrder == Html.GetSortAsc("InvoiceStatus")) query = query.OrderBy(q => q.InvoiceStatus);
            else if (SortOrder == Html.GetSortDsc("InvoiceStatus")) query = query.OrderByDescending(q => q.InvoiceStatus);
            else if (SortOrder == Html.GetSortAsc("Status")) query = query.OrderBy(q => q.Status);
            else if (SortOrder == Html.GetSortDsc("Status")) query = query.OrderByDescending(q => q.Status);
            else query = query.OrderByDescending(q => q.RetroParty.Code);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: PerLifeSoa/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new PerLifeSoaViewModel());
        }

        // POST: PerLifeSoa/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, PerLifeSoaViewModel model)
        {
            if (ModelState.IsValid)
            {
                PerLifeSoaBo bo = new PerLifeSoaBo();
                model.Get(ref bo);
                bo.CreatedById = AuthUserId;
                bo.UpdatedById = AuthUserId;

                TrailObject trail = GetNewTrailObject();
                Result = PerLifeSoaService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;
                    model.Status = bo.Status;

                    model.ProcessStatusHistory(form, AuthUserId, ref trail);

                    CreateTrail(bo.Id, "Create Per Life Retro SOA");

                    SetCreateSuccessMessage("Per Life Retro SOA");
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage();
            return View(model);
        }

        // GET: PerLifeSoa/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            PerLifeSoaBo bo = PerLifeSoaService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            LoadPage(bo);
            return View(new PerLifeSoaViewModel(bo));
        }

        // POST: PerLifeSoa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, PerLifeSoaViewModel model)
        {
            PerLifeSoaBo bo = PerLifeSoaService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                bool statusChanged = false;
                if (model.Status != bo.Status)
                    statusChanged = true;

                model.Get(ref bo);
                bo.UpdatedById = AuthUserId;

                TrailObject trail = GetNewTrailObject();
                Result = PerLifeSoaService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;
                    model.Status = bo.Status;

                    if (statusChanged)
                        model.ProcessStatusHistory(form, AuthUserId, ref trail);

                    CreateTrail(bo.Id, "Update Per Life Retro SOA");

                    SetUpdateSuccessMessage("Per Life Retro SOA");
                    return RedirectToAction("Edit", new { id = model.Id });
                }
                AddResult(Result);
            }

            LoadPage(bo);
            return View(new PerLifeSoaViewModel(bo));
        }

        // GET: PerLifeSoa/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            PerLifeSoaBo bo = PerLifeSoaService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new PerLifeSoaViewModel(bo));
        }

        // POST: PerLifeSoa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id)
        {
            PerLifeSoaBo bo = PerLifeSoaService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = PerLifeSoaService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(bo.Id, "Delete Per Life Retro SOA");

                SetDeleteSuccessMessage("Per Life Retro SOA");
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = bo.Id });
        }

        public ActionResult RetroStatement(int id, int perLifeSoaId)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            PerLifeRetroStatementBo bo;
            // New
            if (id == 0)
            {
                var perLifeSoaBo = PerLifeSoaService.Find(perLifeSoaId);
                bo = new PerLifeRetroStatementBo
                {
                    PerLifeSoaId = perLifeSoaId,
                    PerLifeSoaBo = PerLifeSoaService.Find(perLifeSoaId),
                    //CedingCompany = perLifeSoaBo?.CedantBo?.Name,
                    //TreatyCode = perLifeSoaBo?.TreatyCodeBo?.Code,
                    //TreatyType = perLifeSoaBo?.TreatyCodeBo?.TreatyTypePickListDetailBo?.Code,
                    //AccountsFor = perLifeSoaBo?.TreatyCodeBo?.AccountFor,
                };
            }
            else
            {
                // Edit
                bo = PerLifeRetroStatementService.Find(id);
                if (bo == null)
                {
                    return RedirectToAction("Edit", new { id = perLifeSoaId });
                }
            }

            LoadRetroStatementPage(bo);
            return View(new PerLifeSoaRetroStatementViewModel(bo));
        }

        // POST: PerLifeSoa/RetroStatement/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult RetroStatement(PerLifeSoaRetroStatementViewModel model)
        {
            var bo = model.FormBo(AuthUserId, AuthUserId);
            bool isUpdate = false;

            if (ModelState.IsValid)
            {
                Result = PerLifeRetroStatementService.Result();
                if (Result.Valid)
                {
                    var trail = GetNewTrailObject();

                    if (bo.Id == 0)
                    {
                        if (bo.PerLifeSoaBo.Status == PerLifeSoaBo.StatusPendingApproval ||
                            bo.PerLifeSoaBo.Status == PerLifeSoaBo.StatusApproved ||
                            bo.PerLifeSoaBo.Status == PerLifeSoaBo.StatusStatementIssued)
                        {
                            Result.AddError("Unable to add Retro Statement after \"Completed\" status");
                        }

                        if (Result.Valid)
                        {
                            Result = PerLifeRetroStatementService.Create(ref bo, ref trail);
                            if (Result.Valid)
                            {
                                CreateTrail(
                                    bo.Id,
                                    "Create Retro Statement"
                                );
                            }
                        }
                    }
                    else
                    {
                        Result = PerLifeRetroStatementService.Update(ref bo, ref trail);
                        if (Result.Valid)
                        {
                            CreateTrail(
                                bo.Id,
                                "Update Retro Statement"
                            );
                        }
                        isUpdate = true;
                    }

                    if (Result.Valid)
                    {
                        if (isUpdate)
                        {
                            SetUpdateSuccessMessage(Controller);
                        }
                        else
                        {
                            SetCreateSuccessMessage(Controller);
                        }
                        return RedirectToAction("RetroStatement", new { id = bo.Id, perLifeSoaId = bo.PerLifeSoaId });
                    }
                }
                AddResult(Result);
            }

            LoadRetroStatementPage(bo);
            return View(model);
        }

        public ActionResult PerLifeSoaRiData(int id, int Category, int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Id"] = id,
                ["Category"] = Category,

            };

            _db.Database.CommandTimeout = 0;

            var perLifeAggregationDetailDataIds = PerLifeSoaDataService.GetAllByPerLifeSoaId(id).GroupBy(x => x.PerLifeAggregationDetailDataId).Select(x => x.Key).ToList();
            var inforcePolicyBo = PickListDetailService.FindByPickListIdCode(PickListBo.PolicyStatusCode, PickListDetailBo.PolicyStatusCodeInforce);

            var query = _db.PerLifeAggregationMonthlyData.Where(q => perLifeAggregationDetailDataIds.Contains(q.PerLifeAggregationDetailDataId)).Select(PerLifeSoaDataRetroOutputViewModel.Expression());            
            if (Category == 1)  // RetroOutput
                query = query.Where(q => q.RetroIndicator == true);
            else
            {
                if (inforcePolicyBo != null)
                    query = query.Where(q => q.EndingPolicyStatus == inforcePolicyBo.Id);
            }

            query = query.OrderBy(q => q.PolicyNumber);

            ViewBag.Total = query.Count();

            return PartialView("_PerLifeSoaDetailRetroOutput", query.ToPagedList(Page ?? 1, PageSize));
        }

        public ActionResult DownloadPerLifeSoaRiData(
            string downloadToken,
            int type,
            int id,
            int Category
        )
        {
            Response.SetCookie(new HttpCookie("downloadToken", ""));
            Session["lastActivity"] = DateTime.Now.Ticks;

            _db.Database.CommandTimeout = 0;
            var perLifeAggregationDetailDataIds = PerLifeSoaDataService.GetAllByPerLifeSoaId(id).GroupBy(x => x.PerLifeAggregationDetailDataId).Select(x => x.Key).ToList();
            var inforcePolicyBo = PickListDetailService.FindByPickListIdCode(PickListBo.PolicyStatusCode, PickListDetailBo.PolicyStatusCodeInforce);

            var query = _db.PerLifeAggregationMonthlyData.Where(q => perLifeAggregationDetailDataIds.Contains(q.PerLifeAggregationDetailDataId)).Select(PerLifeAggregationMonthlyDataService.Expression());
            if (Category == 1)  // RetroOutput
                query = query.Where(q => q.RetroIndicator == true);
            else
            {
                if (inforcePolicyBo != null)
                    query = query.Where(q => q.EndingPolicyStatus == inforcePolicyBo.Id);
            }

            if (type == 2) // filtered dowload 
            {
            }

            var export = new ExportPerLifeAggregationMonthlyData();
            export.HandleTempDirectory();
            if (query != null) export.SetQuery(query);
            export.IsRetroDetail = true;
            export.Process();

            DownloadFile(export.FilePath, Path.GetFileName(export.FilePath));
            return null;
        }

        public ActionResult PerLifeSoaClaims(
            int id,
            int ClaimCategory,            
            int? ClaimStatus,
            int? OffsetStatus,
            string ClaimTransactionType,
            string SoaQuarter,
            string EntryNo,
            string ClaimId,
            string InsuredName,
            string PolicyNumber,
            string InsuredGenderCode,
            string CedantDateOfNotification,
            string InsuredDob,
            string ReinsEffDatePol,
            string TreatyCode,
            string ReinsBasisCode,
            string ClaimCode,
            string MlreBenefitCode,
            string ClaimRecoveryAmount,
            string LateInterest,
            string ExGratia,
            string MlreRetainAmount,
            string RetroRecoveryAmount,
            string RetroLateInterest,
            string RetroExGratia,
            string DateOfEvent,
            string CauseOfEvent,
            int? Page
        )
        {
            DateTime? cedantDateOfNotification = Util.GetParseDateTime(CedantDateOfNotification);
            DateTime? insuredDob = Util.GetParseDateTime(InsuredDob);
            DateTime? reinsEffDatePol = Util.GetParseDateTime(ReinsEffDatePol);
            DateTime? dateOfEvent = Util.GetParseDateTime(DateOfEvent);
            double? claimRecoveryAmount = Util.StringToDouble(ClaimRecoveryAmount);
            double? lateInterest = Util.StringToDouble(LateInterest);
            double? exGratia = Util.StringToDouble(ExGratia);
            double? mlreRetainAmount = Util.StringToDouble(MlreRetainAmount);
            double? retroRecoveryAmount = Util.StringToDouble(RetroRecoveryAmount);
            double? retroLateInterest = Util.StringToDouble(RetroLateInterest);
            double? retroExGratia = Util.StringToDouble(RetroExGratia);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Id"] = id,
                ["ClaimCategory"] = ClaimCategory,
                ["ClaimTransactionType"] = ClaimTransactionType,
                ["ClaimStatus"] = ClaimStatus,
                ["OffsetStatus"] = OffsetStatus,
                ["SoaQuarter"] = SoaQuarter,
                ["EntryNo"] = EntryNo,
                ["ClaimId"] = ClaimId,
                ["InsuredName"] = InsuredName,
                ["PolicyNumber"] = PolicyNumber,
                ["InsuredGenderCode"] = InsuredGenderCode,
                ["CedantDateOfNotification"] = cedantDateOfNotification.HasValue ? CedantDateOfNotification : null,
                ["InsuredDob"] = insuredDob.HasValue ? InsuredDob : null,
                ["ReinsEffDatePol"] = reinsEffDatePol.HasValue ? ReinsEffDatePol : null,
                ["TreatyCode"] = TreatyCode,
                ["ReinsBasisCode"] = ReinsBasisCode,
                ["ClaimCode"] = ClaimCode,
                ["MlreBenefitCode"] = MlreBenefitCode,
                ["ClaimRecoveryAmount"] = claimRecoveryAmount.HasValue ? claimRecoveryAmount : null,
                ["LateInterest"] = lateInterest.HasValue ? lateInterest : null,
                ["ExGratia"] = exGratia.HasValue ? exGratia : null,
                ["MlreRetainAmount"] = mlreRetainAmount.HasValue ? mlreRetainAmount : null,
                ["RetroRecoveryAmount"] = retroRecoveryAmount.HasValue ? retroRecoveryAmount : null,
                ["RetroLateInterest"] = retroLateInterest.HasValue ? retroLateInterest : null,
                ["RetroExGratia"] = retroExGratia.HasValue ? retroExGratia : null,
                ["DateOfEvent"] = dateOfEvent.HasValue ? DateOfEvent : null,
                ["CauseOfEvent"] = CauseOfEvent,
            };

            _db.Database.CommandTimeout = 0;

            var perLifeClaimDataIds = PerLifeSoaDataService.GetAllByPerLifeSoaId(id).GroupBy(x => x.PerLifeClaimDataId).Select(x => x.Key).ToList();

            var query = _db.PerLifeClaimRetroData.Where(q => perLifeClaimDataIds.Contains(q.PerLifeClaimDataId) && q.ClaimCategory == ClaimCategory).Select(PerLifeSoaDataClaimViewModel.Expression());
            if (ClaimStatus.HasValue) query = query.Where(q => q.ClaimStatus == ClaimStatus);
            if (OffsetStatus.HasValue) query = query.Where(q => q.OffsetStatus == OffsetStatus);
            if (!string.IsNullOrEmpty(ClaimTransactionType)) query = query.Where(q => q.ClaimTransactionType == ClaimTransactionType);
            if (!string.IsNullOrEmpty(SoaQuarter)) query = query.Where(q => q.RetroSoaQuarter == SoaQuarter);
            if (!string.IsNullOrEmpty(EntryNo)) query = query.Where(q => !string.IsNullOrEmpty(q.EntryNo) && q.EntryNo.Contains(EntryNo));
            if (!string.IsNullOrEmpty(ClaimId)) query = query.Where(q => !string.IsNullOrEmpty(q.ClaimId) && q.ClaimId.Contains(ClaimId));
            if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => !string.IsNullOrEmpty(q.InsuredName) && q.InsuredName.Contains(InsuredName));
            if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => !string.IsNullOrEmpty(q.PolicyNumber) && q.PolicyNumber.Contains(PolicyNumber));
            if (!string.IsNullOrEmpty(InsuredGenderCode)) query = query.Where(q => q.InsuredGenderCode == InsuredGenderCode);
            if (cedantDateOfNotification.HasValue) query = query.Where(q => q.CedantDateOfNotification == cedantDateOfNotification);
            if (insuredDob.HasValue) query = query.Where(q => q.InsuredDateOfBirth == insuredDob);
            if (reinsEffDatePol.HasValue) query = query.Where(q => q.ReinsEffDatePol == reinsEffDatePol);
            if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => q.TreatyCode == TreatyCode);
            if (!string.IsNullOrEmpty(ReinsBasisCode)) query = query.Where(q => q.ReinsBasisCode == ReinsBasisCode);
            if (!string.IsNullOrEmpty(ClaimCode)) query = query.Where(q => q.ClaimCode == ClaimCode);
            if (!string.IsNullOrEmpty(MlreBenefitCode)) query = query.Where(q => q.MlreBenefitCode == MlreBenefitCode);
            if (claimRecoveryAmount.HasValue) query = query.Where(q => q.ClaimRecoveryAmt == claimRecoveryAmount);
            if (lateInterest.HasValue) query = query.Where(q => q.LateInterest == lateInterest);
            if (exGratia.HasValue) query = query.Where(q => q.ExGratia == exGratia);
            if (mlreRetainAmount.HasValue) query = query.Where(q => q.MlreRetainAmount == mlreRetainAmount);
            if (retroRecoveryAmount.HasValue) query = query.Where(q => q.RetroRecoveryAmount == retroRecoveryAmount);
            if (retroLateInterest.HasValue) query = query.Where(q => q.RetroLateInterest == retroLateInterest);
            if (retroExGratia.HasValue) query = query.Where(q => q.RetroExGratia == retroExGratia);
            if (dateOfEvent.HasValue) query = query.Where(q => q.DateOfEvent == dateOfEvent);
            if (!string.IsNullOrEmpty(CauseOfEvent)) query = query.Where(q => !string.IsNullOrEmpty(q.CauseOfEvent) && q.CauseOfEvent.Contains(CauseOfEvent));

            query = query.OrderBy(q => q.RetroSoaQuarter);

            ViewBag.Total = query.Count();
            LoadPerLifeSoaDetailClaimPage();

            return PartialView("_PerLifeSoaDetailClaims", query.ToPagedList(Page ?? 1, PageSize));
        }

        public ActionResult DownloadPerLifeSoaClaims(
            string downloadToken,
            int type,
            int id,
            int ClaimCategory,
            int? ClaimStatus,
            int? OffsetStatus,
            string ClaimTransactionType,
            string SoaQuarter,
            string EntryNo,
            string ClaimId,
            string InsuredName,
            string PolicyNumber,
            string InsuredGenderCode,
            string CedantDateOfNotification,
            string InsuredDob,
            string ReinsEffDatePol,
            string TreatyCode,
            string ReinsBasisCode,
            string ClaimCode,
            string MlreBenefitCode,
            string ClaimRecoveryAmount,
            string LateInterest,
            string ExGratia,
            string MlreRetainAmount,
            string RetroRecoveryAmount,
            string RetroLateInterest,
            string RetroExGratia,
            string DateOfEvent,
            string CauseOfEvent
        )
        {
            Response.SetCookie(new HttpCookie("downloadToken", ""));
            Session["lastActivity"] = DateTime.Now.Ticks;

            _db.Database.CommandTimeout = 0;
            var perLifeClaimDataIds = PerLifeSoaDataService.GetAllByPerLifeSoaId(id).GroupBy(x => x.PerLifeClaimDataId).Select(x => x.Key).ToList();

            var query = _db.PerLifeClaimRetroData.Where(q => perLifeClaimDataIds.Contains(q.PerLifeClaimDataId) && q.ClaimCategory == ClaimCategory).Select(PerLifeSoaDataService.ClaimExpression());
            if (type == 2) // filtered dowload 
            {
                DateTime? cedantDateOfNotification = Util.GetParseDateTime(CedantDateOfNotification);
                DateTime? insuredDob = Util.GetParseDateTime(InsuredDob);
                DateTime? reinsEffDatePol = Util.GetParseDateTime(ReinsEffDatePol);
                DateTime? dateOfEvent = Util.GetParseDateTime(DateOfEvent);
                double? claimRecoveryAmount = Util.StringToDouble(ClaimRecoveryAmount);
                double? lateInterest = Util.StringToDouble(LateInterest);
                double? exGratia = Util.StringToDouble(ExGratia);
                double? mlreRetainAmount = Util.StringToDouble(MlreRetainAmount);
                double? retroRecoveryAmount = Util.StringToDouble(RetroRecoveryAmount);
                double? retroLateInterest = Util.StringToDouble(RetroLateInterest);
                double? retroExGratia = Util.StringToDouble(RetroExGratia);

                if (ClaimStatus.HasValue) query = query.Where(q => q.ClaimStatus == ClaimStatus);
                if (OffsetStatus.HasValue) query = query.Where(q => q.OffsetStatus == OffsetStatus);
                if (!string.IsNullOrEmpty(ClaimTransactionType)) query = query.Where(q => q.ClaimTransactionType == ClaimTransactionType);
                if (!string.IsNullOrEmpty(SoaQuarter)) query = query.Where(q => q.RetroSoaQuarter == SoaQuarter);
                if (!string.IsNullOrEmpty(EntryNo)) query = query.Where(q => !string.IsNullOrEmpty(q.EntryNo) && q.EntryNo.Contains(EntryNo));
                if (!string.IsNullOrEmpty(ClaimId)) query = query.Where(q => !string.IsNullOrEmpty(q.ClaimId) && q.ClaimId.Contains(ClaimId));
                if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => !string.IsNullOrEmpty(q.InsuredName) && q.InsuredName.Contains(InsuredName));
                if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => !string.IsNullOrEmpty(q.PolicyNumber) && q.PolicyNumber.Contains(PolicyNumber));
                if (!string.IsNullOrEmpty(InsuredGenderCode)) query = query.Where(q => q.InsuredGenderCode == InsuredGenderCode);
                if (cedantDateOfNotification.HasValue) query = query.Where(q => q.CedantDateOfNotification == cedantDateOfNotification);
                if (insuredDob.HasValue) query = query.Where(q => q.InsuredDateOfBirth == insuredDob);
                if (reinsEffDatePol.HasValue) query = query.Where(q => q.ReinsEffDatePol == reinsEffDatePol);
                if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => q.TreatyCode == TreatyCode);
                if (!string.IsNullOrEmpty(ReinsBasisCode)) query = query.Where(q => q.ReinsBasisCode == ReinsBasisCode);
                if (!string.IsNullOrEmpty(ClaimCode)) query = query.Where(q => q.ClaimCode == ClaimCode);
                if (!string.IsNullOrEmpty(MlreBenefitCode)) query = query.Where(q => q.MlreBenefitCode == MlreBenefitCode);
                if (claimRecoveryAmount.HasValue) query = query.Where(q => q.ClaimRecoveryAmt == claimRecoveryAmount);
                if (lateInterest.HasValue) query = query.Where(q => q.LateInterest == lateInterest);
                if (exGratia.HasValue) query = query.Where(q => q.ExGratia == exGratia);
                if (mlreRetainAmount.HasValue) query = query.Where(q => q.MlreRetainAmount == mlreRetainAmount);
                if (retroRecoveryAmount.HasValue) query = query.Where(q => q.RetroRecoveryAmount == retroRecoveryAmount);
                if (retroLateInterest.HasValue) query = query.Where(q => q.RetroLateInterest == retroLateInterest);
                if (retroExGratia.HasValue) query = query.Where(q => q.RetroExGratia == retroExGratia);
                if (dateOfEvent.HasValue) query = query.Where(q => q.DateOfEvent == dateOfEvent);
                if (!string.IsNullOrEmpty(CauseOfEvent)) query = query.Where(q => !string.IsNullOrEmpty(q.CauseOfEvent) && q.CauseOfEvent.Contains(CauseOfEvent));
            }

            var export = new ExportPerLifeSoaDataClaims();
            export.HandleTempDirectory();
            if (query != null) export.SetQuery(query);
            export.CategoryPendingClaim = (ClaimCategory == PerLifeClaimDataBo.ClaimCategoryPendingClaim ? true : false);
            export.Process();

            DownloadFile(export.FilePath, Path.GetFileName(export.FilePath));
            return null;
        }

        public void IndexPage()
        {
            SetViewBagMessage();

            var departmentId = Util.GetConfigInteger("PerLifeAggregationPicDepartment", DepartmentBo.DepartmentRetrocession);
            DropDownEmpty();
            DropDownRetroParty();
            DropDownRetroTreaty();
            DropDownStatus();
            DropDownUser(departmentId: departmentId);
            DropDownInvoiceStatus();
            DropDownYesNoWithSelect();
        }

        public void LoadPage(PerLifeSoaBo perLifeSoaBo = null)
        {
            SetViewBagMessage();

            var departmentId = Util.GetConfigInteger("PerLifeAggregationPicDepartment", DepartmentBo.DepartmentRetrocession);
            DropDownEmpty();
            if (perLifeSoaBo != null)
            {
                DropDownRetroParty(RetroPartyBo.StatusActive, perLifeSoaBo.RetroPartyId);
                DropDownRetroTreaty(RetroTreatyBo.StatusActive, perLifeSoaBo.RetroTreatyId);
                DropDownUser(UserBo.StatusActive, perLifeSoaBo.PersonInChargeId, false, departmentId: departmentId);
                DropDownDocumentType();
                DropDownTreatyCode(TreatyCodeBo.StatusActive, codeAsValue: true);

                ModuleBo moduleBo = GetModuleByController(ModuleBo.ModuleController.PerLifeSoa.ToString());
                ViewBag.StatusHistoryBos = StatusHistoryService.GetStatusHistoriesByModuleIdObjectId(moduleBo.Id, perLifeSoaBo.Id);
                string downloadDocumentUrl = Url.Action("Download", "Document");
                var documentBos = GetDocuments(moduleBo.Id, perLifeSoaBo.Id, downloadDocumentUrl, true, AuthUser.DepartmentId);
                var remarkBos = RemarkService.GetByModuleIdObjectId(moduleBo.Id, perLifeSoaBo.Id, true, AuthUser.DepartmentId);
                if (documentBos != null && documentBos.Count != 0)
                {
                    foreach (RemarkBo remarkBo in remarkBos)
                    {
                        remarkBo.DocumentBos = documentBos.Where(q => q.RemarkId.HasValue && q.RemarkId == remarkBo.Id).ToList();
                    }
                }
                ViewBag.RemarkBos = remarkBos;
                ViewBag.CanApproveSoa = CheckPower(Controller, AccessMatrixBo.PowerApprovalRetroSOA);

                ViewBag.ShowSubmitForProcessing = PerLifeSoaBo.CanSubmitForProcessing(perLifeSoaBo.Status);
                ViewBag.ShowSubmitForApproval = PerLifeSoaBo.CanSubmitForApproval(perLifeSoaBo.Status);
                ViewBag.ShowApprove = PerLifeSoaBo.CanApprove(perLifeSoaBo.Status);
                ViewBag.ShowSave = PerLifeSoaBo.CanSave(perLifeSoaBo.Status);

                ViewBag.RetroStatements = PerLifeRetroStatementService.GetByPerLifeSoaId(perLifeSoaBo.Id);
                ViewBag.SoaSummaries = PerLifeSoaSummariesSoaService.GetByPerLifeSoaId(perLifeSoaBo.Id);
                ViewBag.SoaWMOMs = PerLifeSoaSummariesService.GetByPerLifeSoaId(perLifeSoaBo.Id);
                ViewBag.SoaSummaryByTreaties = PerLifeSoaSummariesByTreatyService.GetByPerLifeSoaId(perLifeSoaBo.Id);
            }
            else
            {
                DropDownRetroParty(RetroPartyBo.StatusActive);
                DropDownRetroTreaty(RetroTreatyBo.StatusActive);
                DropDownUser(UserBo.StatusActive, null, false, departmentId);
            }            
        }

        public void LoadRetroStatementPage(PerLifeRetroStatementBo bo = null)
        {
            if (bo == null)
            {
                DropDownRetroParty(RetroPartyBo.StatusActive);
            }
            else
            {
                DropDownRetroParty(RetroPartyBo.StatusActive, bo.RetroPartyId);
                if (bo.RetroPartyBo != null && bo.RetroPartyBo.Status == RetroPartyBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.RetroPartyStatusInactive);
                }
            }
            SetViewBagMessage();
        }

        public void LoadPerLifeSoaDetailClaimPage()
        {           
            DropDownClaimStatus();
            DropDownOffsetStatus();

            DropDownTreatyCode(codeAsValue: true);
            DropDownClaimCode(codeAsValue: true);
            DropDownBenefit(codeAsValue: true);
            DropDownClaimTransactionType(codeAsValue: true);
            DropDownInsuredGenderCode(codeAsValue: true);
            DropDownReinsBasisCode(codeAsValue: true);
        }

        public List<SelectListItem> DropDownStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= PerLifeSoaBo.StatusMax; i++)
            {
                items.Add(new SelectListItem { Text = PerLifeSoaBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.StatusList = items;
            return items;
        }

        public List<SelectListItem> DropDownInvoiceStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= PerLifeSoaBo.InvoiceStatusMax; i++)
            {
                items.Add(new SelectListItem { Text = PerLifeSoaBo.GetInvoiceStatusName(i), Value = i.ToString() });
            }
            ViewBag.InvoiceStatuses = items;
            return items;
        }

        // Ajax Functions called by other modules
        [HttpPost]
        public JsonResult GetRetroTreatyByRetroParty(int retroPartyId)
        {
            var retroTreatyBos = RetroTreatyService.GetByRetroParty(retroPartyId);
            return Json(new { retroTreatyBos });
        }

        [HttpPost]
        public JsonResult SearchSummaryByTreaty(int id, string treatyCode, string riskYear)
        {
            var bos = PerLifeSoaSummariesByTreatyService.GetByPerLifeSoaId(id);
            if(bos != null)
            {
                if (!string.IsNullOrEmpty(treatyCode))
                    bos = bos.Where(q => q.TreatyCode == treatyCode).ToList();

                if (!string.IsNullOrEmpty(riskYear))
                    bos = bos.Where(q => q.ProcessingPeriodYear == riskYear).ToList();
            }
            return Json(new { PremiumSummaryByTreatyBos = bos });
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

        [HttpPost]
        public JsonResult ApproveSoa(int id)
        {
            var bo = PerLifeSoaService.Find(id);

            if (bo.Status == PerLifeSoaBo.StatusProcessingSuccess)
            {
                bo.Status = PerLifeSoaBo.StatusPendingApproval;
            }
            else if (bo.Status == PerLifeSoaBo.StatusPendingApproval && ViewBag.CanApproveSoa == true)
            {
                bo.Status = PerLifeSoaBo.StatusApproved;
            }

            Result = PerLifeSoaService.Update(ref bo);

            if (Result.Valid)
            {
                return Json(new { errors = "" });
            }
            else
            {
                return Json(new { errors = Result.MessageBag });
            }
        }

        public ActionResult DownloadRetroStatement(int id)
        {
            PerLifeRetroStatementBo retroStatementBo = PerLifeRetroStatementService.Find(id);

            if (retroStatementBo != null || retroStatementBo.Status == PerLifeRetroStatementBo.StatusFinalised)
            {
                try
                {
                    var process = new GeneratePerLifeRetroStatement()
                    {
                        PerLifeRetroStatementBo = retroStatementBo,
                    };
                    process.Process();

                    DownloadFile(process.FilePath, process.FileName);
                    Util.DeleteFiles(process.Directory, process.FileName);
                    return null;
                }
                catch (Exception ex)
                {
                    SetErrorSessionMsg("Error occured during processing Excel File");
                    return RedirectToAction("Edit", new { id = retroStatementBo.PerLifeSoaId });
                }
            }

            SetErrorSessionMsg("Retro Statement not found");
            return RedirectToAction("Index");
        }

        public ActionResult DownloadAllRetroStatement(int perLifeSoaId)
        {
            Dictionary<string, string> files = new Dictionary<string, string>() { };

            var retroStatementBos = PerLifeRetroStatementService.GetByPerLifeSoaId(perLifeSoaId);
            if (retroStatementBos != null)
            {
                foreach (var retroStatementBo in retroStatementBos)
                {
                    try
                    {
                        var process = new GeneratePerLifeRetroStatement()
                        {
                            PerLifeRetroStatementBo = retroStatementBo,
                        };
                        process.Process();

                        files.Add(process.FilePath, process.FileName);

                        //DownloadFile(process.FilePath, process.FileName);
                        //Util.DeleteFiles(process.Directory, process.FileName);


                    }
                    catch (Exception ex)
                    {
                        SetErrorSessionMsg("Error occured during processing Excel File");
                        return RedirectToAction("Edit", new { id = perLifeSoaId });
                    }
                }

                if (files.Any())
                {
                    try
                    {
                        string zipFileName = string.Format("RetroStatements").AppendDateTimeFileName(".zip");

                        Response.ClearContent();
                        Response.Clear();
                        Response.ContentType = "application/zip";
                        Response.AddHeader("content-disposition", "attachment; filename=" + zipFileName);

                        using (ZipFile zip = new ZipFile())
                        {
                            foreach(var file in files)
                                zip.AddSelectedFiles(file.Value, Util.GetRetroStatementDownloadPath(), "", false);
                            zip.Save(Response.OutputStream);
                        }

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
                    return null;
                }
                
            }

            SetErrorSessionMsg("Retro Statement not found");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult CalculateStatement(int perLifeSoaId, int retroPartyId, string accountingPeriod1, string accountingPeriod2, string accountingPeriod3)
        {
            string error = null;
            var retroStatement = new PerLifeRetroStatementBo();
            RetroPartyBo retroPartyBo = new RetroPartyBo();

            PerLifeSoaBo perLifeSoaBo = PerLifeSoaService.Find(perLifeSoaId);
            if (perLifeSoaBo == null)
            {
                error = string.Format(MessageBag.NotExistsWithValue, "Per Life Retro SOA", perLifeSoaId);
            }

            if (string.IsNullOrEmpty(error))
            {
                retroPartyBo = RetroPartyService.Find(retroPartyId);
                if (retroPartyBo == null)
                {
                    error = string.Format(MessageBag.NotExistsWithValue, "Retro Party", retroPartyId);
                }

                if (!string.IsNullOrEmpty(accountingPeriod1) && !Util.ValidateQuarter(accountingPeriod1))
                    error = string.Format("Invalid Accounting Period 1 format");

                if (!string.IsNullOrEmpty(accountingPeriod2) && !Util.ValidateQuarter(accountingPeriod2))
                    error = string.Format("Invalid Accounting Period 2 format");

                if (!string.IsNullOrEmpty(accountingPeriod3) && !Util.ValidateQuarter(accountingPeriod3))
                    error = string.Format("Invalid Accounting Period 3 format");
            }

            if (string.IsNullOrEmpty(error))
            {
                _db.Database.CommandTimeout = 0;

                var perLifeClaimDataIds = PerLifeSoaDataService.GetAllByPerLifeSoaId(perLifeSoaId).GroupBy(x => x.PerLifeClaimDataId).Select(x => x.Key).ToList();
                var retroClaimRecoveryAmount = _db.PerLifeClaimRetroData.Where(q => perLifeClaimDataIds.Contains(q.PerLifeClaimDataId)).Sum(q => q.RetroClaimRecoveryAmount) ?? 0;

                var perLifeAggregationDetailDataIds = PerLifeSoaDataService.GetAllByPerLifeSoaId(perLifeSoaId).GroupBy(x => x.PerLifeAggregationDetailDataId).Select(x => x.Key).ToList();
                var perLifeAggregationMonthlyDataBos = _db.PerLifeAggregationMonthlyData.Where(q => perLifeAggregationDetailDataIds.Contains(q.PerLifeAggregationDetailDataId) && q.RetroIndicator == true).Select(PerLifeAggregationMonthlyDataService.Expression());
                var nbs = perLifeAggregationMonthlyDataBos.Where(q => q.TransactionTypeCode == PickListDetailBo.TransactionTypeCodeNewBusiness).ToList();
                var rns = perLifeAggregationMonthlyDataBos.Where(q => q.TransactionTypeCode == PickListDetailBo.TransactionTypeCodeRenewal).ToList();
                var als = perLifeAggregationMonthlyDataBos.Where(q => q.TransactionTypeCode == PickListDetailBo.TransactionTypeCodeAlteration).ToList();

                if (!string.IsNullOrEmpty(accountingPeriod1))
                {
                    QuarterObject qo = new QuarterObject(accountingPeriod1);
                    double claimAmount = 0;

                    if (nbs != null && nbs.Count() > 0)
                    {
                        var retro1 = nbs.Where(q => q.RiskPeriodYear == qo.Year).Where(q => q.RiskPeriodMonth >= qo.MonthStart).Where(q => q.RiskPeriodMonth <= qo.MonthEnd).ToList();

                        double riPremium = retro1.Sum(q => q.RetroAmount) ?? 0;
                        retroStatement.RiPremiumNBStr = Util.DoubleToString(riPremium, 2);

                        double riDiscount = retro1.Sum(q => q.RetroDiscount) ?? 0;
                        retroStatement.RiDiscountNBStr = Util.DoubleToString(riDiscount, 2);
                        
                        int policy = retro1.Count();
                        retroStatement.TotalNoOfPolicyNB = policy;

                        double retroAar = retro1.Where(q => q.RetroParty1 == retroPartyBo.Party).Sum(q => q.RetroAar1) ?? 0;
                        retroAar += retro1.Where(q => q.RetroParty2 == retroPartyBo.Party).Sum(q => q.RetroAar2) ?? 0;
                        retroAar += retro1.Where(q => q.RetroParty3 == retroPartyBo.Party).Sum(q => q.RetroAar3) ?? 0;
                        retroStatement.TotalSumReinsuredNBStr = Util.DoubleToString(Util.RoundValue(retroAar, 2), 2);
                    }

                    if (rns != null && rns.Count() > 0)
                    {
                        var retro1 = rns.Where(q => q.RiskPeriodYear == qo.Year).Where(q => q.RiskPeriodMonth >= qo.MonthStart).Where(q => q.RiskPeriodMonth <= qo.MonthEnd).ToList();

                        double riPremium = retro1.Sum(q => q.RetroAmount) ?? 0;
                        retroStatement.RiPremiumRNStr = Util.DoubleToString(riPremium, 2);

                        double riDiscount = retro1.Sum(q => q.RetroDiscount) ?? 0;
                        retroStatement.RiDiscountRNStr = Util.DoubleToString(riDiscount, 2);

                        int policy = retro1.Count();
                        retroStatement.TotalNoOfPolicyRN = policy;

                        double retroAar = retro1.Where(q => q.RetroParty1 == retroPartyBo.Party).Sum(q => q.RetroAar1) ?? 0;
                        retroAar += retro1.Where(q => q.RetroParty2 == retroPartyBo.Party).Sum(q => q.RetroAar2) ?? 0;
                        retroAar += retro1.Where(q => q.RetroParty3 == retroPartyBo.Party).Sum(q => q.RetroAar3) ?? 0;
                        retroStatement.TotalSumReinsuredRNStr = Util.DoubleToString(Util.RoundValue(retroAar, 2), 2);
                    }

                    if (als != null && als.Count() > 0)
                    {
                        var retro1 = als.Where(q => q.RiskPeriodYear == qo.Year).Where(q => q.RiskPeriodMonth >= qo.MonthStart).Where(q => q.RiskPeriodMonth <= qo.MonthEnd).ToList();

                        double riPremium = retro1.Sum(q => q.RetroAmount) ?? 0;
                        retroStatement.RiPremiumALTStr = Util.DoubleToString(riPremium, 2);

                        double riDiscount = retro1.Sum(q => q.RetroDiscount) ?? 0;
                        retroStatement.RiDiscountALTStr = Util.DoubleToString(riDiscount, 2);

                        int policy = retro1.Count();
                        retroStatement.TotalNoOfPolicyALT = policy;

                        double retroAar = retro1.Where(q => q.RetroParty1 == retroPartyBo.Party).Sum(q => q.RetroAar1) ?? 0;
                        retroAar += retro1.Where(q => q.RetroParty2 == retroPartyBo.Party).Sum(q => q.RetroAar2) ?? 0;
                        retroAar += retro1.Where(q => q.RetroParty3 == retroPartyBo.Party).Sum(q => q.RetroAar3) ?? 0;
                        retroStatement.TotalSumReinsuredALTStr = Util.DoubleToString(Util.RoundValue(retroAar, 2), 2);
                    }

                    if (perLifeSoaBo.SoaQuarter == accountingPeriod1)
                        claimAmount = retroClaimRecoveryAmount;
                    retroStatement.ClaimsStr = Util.DoubleToString(claimAmount, 2);

                    var totalClaim = perLifeAggregationMonthlyDataBos.Where(q => q.RiskPeriodYear == qo.Year).Where(q => q.RiskPeriodMonth >= qo.MonthStart).Where(q => q.RiskPeriodMonth <= qo.MonthEnd).ToList();

                    var noClaimBonus = totalClaim.Where(q => q.RetroParty1 == retroPartyBo.Party).Sum(q => q.NoClaimBonus) ?? 0;
                    noClaimBonus += totalClaim.Where(q => q.RetroParty2 == retroPartyBo.Party).Sum(q => q.NoClaimBonus) ?? 0;
                    noClaimBonus += totalClaim.Where(q => q.RetroParty3 == retroPartyBo.Party).Sum(q => q.NoClaimBonus) ?? 0;
                    retroStatement.NoClaimBonusStr = Util.DoubleToString(Util.RoundValue(noClaimBonus, 2), 2);

                    var databaseCommission = totalClaim.Where(q => q.RetroParty1 == retroPartyBo.Party).Sum(q => q.DatabaseCommision) ?? 0;
                    databaseCommission += totalClaim.Where(q => q.RetroParty2 == retroPartyBo.Party).Sum(q => q.DatabaseCommision) ?? 0;
                    databaseCommission += totalClaim.Where(q => q.RetroParty3 == retroPartyBo.Party).Sum(q => q.DatabaseCommision) ?? 0;
                    retroStatement.AgreedDatabaseCommStr = Util.DoubleToString(Util.RoundValue(databaseCommission, 2), 2);
                }

                if (!string.IsNullOrEmpty(accountingPeriod2))
                {
                    QuarterObject qo = new QuarterObject(accountingPeriod2);
                    double claimAmount2 = 0;

                    if (nbs != null && nbs.Count() > 0)
                    {
                        var retro2 = nbs.Where(q => q.RiskPeriodYear == qo.Year).Where(q => q.RiskPeriodMonth >= qo.MonthStart).Where(q => q.RiskPeriodMonth <= qo.MonthEnd).ToList();

                        double riPremium2 = retro2.Sum(q => q.RetroAmount) ?? 0;
                        retroStatement.RiPremiumNB2Str = Util.DoubleToString(riPremium2, 2);

                        double riDiscount2 = retro2.Sum(q => q.RetroDiscount) ?? 0;
                        retroStatement.RiDiscountNB2Str = Util.DoubleToString(riDiscount2, 2);

                        int policy2 = retro2.Count();
                        retroStatement.TotalNoOfPolicyNB2 = policy2;

                        double retroAar2 = retro2.Where(q => q.RetroParty1 == retroPartyBo.Party).Sum(q => q.RetroAar1) ?? 0;
                        retroAar2 += retro2.Where(q => q.RetroParty2 == retroPartyBo.Party).Sum(q => q.RetroAar2) ?? 0;
                        retroAar2 += retro2.Where(q => q.RetroParty3 == retroPartyBo.Party).Sum(q => q.RetroAar3) ?? 0;
                        retroStatement.TotalSumReinsuredNB2Str = Util.DoubleToString(Util.RoundValue(retroAar2, 2), 2);
                    }

                    if (rns != null && rns.Count() > 0)
                    {
                        var retro2 = rns.Where(q => q.RiskPeriodYear == qo.Year).Where(q => q.RiskPeriodMonth >= qo.MonthStart).Where(q => q.RiskPeriodMonth <= qo.MonthEnd).ToList();

                        double riPremium2 = retro2.Sum(q => q.RetroAmount) ?? 0;
                        retroStatement.RiPremiumRN2Str = Util.DoubleToString(riPremium2, 2);

                        double riDiscount2 = retro2.Sum(q => q.RetroDiscount) ?? 0;
                        retroStatement.RiDiscountRN2Str = Util.DoubleToString(riDiscount2, 2);

                        int policy2 = retro2.Count();
                        retroStatement.TotalNoOfPolicyRN2 = policy2;

                        double retroAar2 = retro2.Where(q => q.RetroParty1 == retroPartyBo.Party).Sum(q => q.RetroAar1) ?? 0;
                        retroAar2 += retro2.Where(q => q.RetroParty2 == retroPartyBo.Party).Sum(q => q.RetroAar2) ?? 0;
                        retroAar2 += retro2.Where(q => q.RetroParty3 == retroPartyBo.Party).Sum(q => q.RetroAar3) ?? 0;
                        retroStatement.TotalSumReinsuredRN2Str = Util.DoubleToString(Util.RoundValue(retroAar2, 2), 2);
                    }

                    if (als != null && als.Count() > 0)
                    {
                        var retro2 = als.Where(q => q.RiskPeriodYear == qo.Year).Where(q => q.RiskPeriodMonth >= qo.MonthStart).Where(q => q.RiskPeriodMonth <= qo.MonthEnd).ToList();

                        double riPremium2 = retro2.Sum(q => q.RetroAmount) ?? 0;
                        retroStatement.RiPremiumALT2Str = Util.DoubleToString(riPremium2, 2);

                        double riDiscount2 = retro2.Sum(q => q.RetroDiscount) ?? 0;
                        retroStatement.RiDiscountALT2Str = Util.DoubleToString(riDiscount2, 2);

                        int policy2 = retro2.Count();
                        retroStatement.TotalNoOfPolicyALT2 = policy2;

                        double retroAar2 = retro2.Where(q => q.RetroParty1 == retroPartyBo.Party).Sum(q => q.RetroAar1) ?? 0;
                        retroAar2 += retro2.Where(q => q.RetroParty2 == retroPartyBo.Party).Sum(q => q.RetroAar2) ?? 0;
                        retroAar2 += retro2.Where(q => q.RetroParty3 == retroPartyBo.Party).Sum(q => q.RetroAar3) ?? 0;
                        retroStatement.TotalSumReinsuredALT2Str = Util.DoubleToString(Util.RoundValue(retroAar2, 2), 2);
                    }

                    if (perLifeSoaBo.SoaQuarter == accountingPeriod2)
                        claimAmount2 = retroClaimRecoveryAmount;
                    retroStatement.Claims2Str = Util.DoubleToString(claimAmount2, 2);

                    var totalClaim2 = perLifeAggregationMonthlyDataBos.Where(q => q.RiskPeriodYear == qo.Year).Where(q => q.RiskPeriodMonth >= qo.MonthStart).Where(q => q.RiskPeriodMonth <= qo.MonthEnd).ToList();

                    var noClaimBonus2 = totalClaim2.Where(q => q.RetroParty1 == retroPartyBo.Party).Sum(q => q.NoClaimBonus) ?? 0;
                    noClaimBonus2 += totalClaim2.Where(q => q.RetroParty2 == retroPartyBo.Party).Sum(q => q.NoClaimBonus) ?? 0;
                    noClaimBonus2 += totalClaim2.Where(q => q.RetroParty3 == retroPartyBo.Party).Sum(q => q.NoClaimBonus) ?? 0;
                    retroStatement.NoClaimBonus2Str = Util.DoubleToString(Util.RoundValue(noClaimBonus2, 2), 2);

                    var databaseCommission2 = totalClaim2.Where(q => q.RetroParty1 == retroPartyBo.Party).Sum(q => q.DatabaseCommision) ?? 0;
                    databaseCommission2 += totalClaim2.Where(q => q.RetroParty2 == retroPartyBo.Party).Sum(q => q.DatabaseCommision) ?? 0;
                    databaseCommission2 += totalClaim2.Where(q => q.RetroParty3 == retroPartyBo.Party).Sum(q => q.DatabaseCommision) ?? 0;
                    retroStatement.AgreedDatabaseComm2Str = Util.DoubleToString(Util.RoundValue(databaseCommission2, 2), 2);
                }

                if (!string.IsNullOrEmpty(accountingPeriod3))
                {
                    QuarterObject qo = new QuarterObject(accountingPeriod3);
                    double claimAmount3 = 0;

                    if (nbs != null && nbs.Count() > 0)
                    {
                        var retro3 = nbs.Where(q => q.RiskPeriodYear == qo.Year).Where(q => q.RiskPeriodMonth >= qo.MonthStart).Where(q => q.RiskPeriodMonth <= qo.MonthEnd).ToList();

                        double riPremium3 = retro3.Sum(q => q.RetroAmount) ?? 0;
                        retroStatement.RiPremiumNB3Str = Util.DoubleToString(riPremium3, 2);

                        double riDiscount3 = retro3.Sum(q => q.RetroDiscount) ?? 0;
                        retroStatement.RiDiscountNB3Str = Util.DoubleToString(riDiscount3, 2);

                        int policy3 = retro3.Count();
                        retroStatement.TotalNoOfPolicyNB3 = policy3;

                        double retroAar3 = retro3.Where(q => q.RetroParty1 == retroPartyBo.Party).Sum(q => q.RetroAar1) ?? 0;
                        retroAar3 += retro3.Where(q => q.RetroParty2 == retroPartyBo.Party).Sum(q => q.RetroAar2) ?? 0;
                        retroAar3 += retro3.Where(q => q.RetroParty3 == retroPartyBo.Party).Sum(q => q.RetroAar3) ?? 0;
                        retroStatement.TotalSumReinsuredNB3Str = Util.DoubleToString(Util.RoundValue(retroAar3, 2), 2);
                    }

                    if (rns != null && rns.Count() > 0)
                    {
                        var retro3 = rns.Where(q => q.RiskPeriodYear == qo.Year).Where(q => q.RiskPeriodMonth >= qo.MonthStart).Where(q => q.RiskPeriodMonth <= qo.MonthEnd).ToList();

                        double riPremium3 = retro3.Sum(q => q.RetroAmount) ?? 0;
                        retroStatement.RiPremiumRN3Str = Util.DoubleToString(riPremium3, 2);

                        double riDiscount3 = retro3.Sum(q => q.RetroDiscount) ?? 0;
                        retroStatement.RiDiscountRN3Str = Util.DoubleToString(riDiscount3, 2);

                        int policy3 = retro3.Count();
                        retroStatement.TotalNoOfPolicyRN3 = policy3;

                        double retroAar3 = retro3.Where(q => q.RetroParty1 == retroPartyBo.Party).Sum(q => q.RetroAar1) ?? 0;
                        retroAar3 += retro3.Where(q => q.RetroParty2 == retroPartyBo.Party).Sum(q => q.RetroAar2) ?? 0;
                        retroAar3 += retro3.Where(q => q.RetroParty3 == retroPartyBo.Party).Sum(q => q.RetroAar3) ?? 0;
                        retroStatement.TotalSumReinsuredRN3Str = Util.DoubleToString(Util.RoundValue(retroAar3, 2), 2);
                    }

                    if (als != null && als.Count() > 0)
                    {
                        var retro3 = als.Where(q => q.RiskPeriodYear == qo.Year).Where(q => q.RiskPeriodMonth >= qo.MonthStart).Where(q => q.RiskPeriodMonth <= qo.MonthEnd).ToList();

                        double riPremium3 = retro3.Sum(q => q.RetroAmount) ?? 0;
                        retroStatement.RiPremiumALT3Str = Util.DoubleToString(riPremium3, 2);

                        double riDiscount3 = retro3.Sum(q => q.RetroDiscount) ?? 0;
                        retroStatement.RiDiscountALT3Str = Util.DoubleToString(riDiscount3, 2);

                        int policy3 = retro3.Count();
                        retroStatement.TotalNoOfPolicyALT3 = policy3;

                        double retroAar3 = retro3.Where(q => q.RetroParty1 == retroPartyBo.Party).Sum(q => q.RetroAar1) ?? 0;
                        retroAar3 += retro3.Where(q => q.RetroParty2 == retroPartyBo.Party).Sum(q => q.RetroAar2) ?? 0;
                        retroAar3 += retro3.Where(q => q.RetroParty3 == retroPartyBo.Party).Sum(q => q.RetroAar3) ?? 0;
                        retroStatement.TotalSumReinsuredALT3Str = Util.DoubleToString(Util.RoundValue(retroAar3, 2), 2);
                    }

                    if (perLifeSoaBo.SoaQuarter == accountingPeriod3)
                        claimAmount3 = retroClaimRecoveryAmount;
                    retroStatement.Claims3Str = Util.DoubleToString(claimAmount3, 2);

                    var totalClaim3 = perLifeAggregationMonthlyDataBos.Where(q => q.RiskPeriodYear == qo.Year).Where(q => q.RiskPeriodMonth >= qo.MonthStart).Where(q => q.RiskPeriodMonth <= qo.MonthEnd).ToList();

                    var noClaimBonus3 = totalClaim3.Where(q => q.RetroParty1 == retroPartyBo.Party).Sum(q => q.NoClaimBonus) ?? 0;
                    noClaimBonus3 += totalClaim3.Where(q => q.RetroParty2 == retroPartyBo.Party).Sum(q => q.NoClaimBonus) ?? 0;
                    noClaimBonus3 += totalClaim3.Where(q => q.RetroParty3 == retroPartyBo.Party).Sum(q => q.NoClaimBonus) ?? 0;
                    retroStatement.NoClaimBonus3Str = Util.DoubleToString(Util.RoundValue(noClaimBonus3, 2), 2);

                    var databaseCommission3 = totalClaim3.Where(q => q.RetroParty1 == retroPartyBo.Party).Sum(q => q.DatabaseCommision) ?? 0;
                    databaseCommission3 += totalClaim3.Where(q => q.RetroParty2 == retroPartyBo.Party).Sum(q => q.DatabaseCommision) ?? 0;
                    databaseCommission3 += totalClaim3.Where(q => q.RetroParty3 == retroPartyBo.Party).Sum(q => q.DatabaseCommision) ?? 0;
                    retroStatement.AgreedDatabaseComm3Str = Util.DoubleToString(Util.RoundValue(databaseCommission3, 2), 2);
                }
            }

            return Json(new { RetroStatement = retroStatement, Error = error });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadRetroStatement(HttpPostedFileBase upload, PerLifeSoaRetroStatementViewModel model)
        {
            var bo = model.FormBo(AuthUserId, AuthUserId);
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var process = new ProcessPerLifeRetroStatement()
                    {
                        RetroStatementBo = bo,
                        PostedFile = upload,
                    };
                    bo = process.Process();

                    if (process.Errors.Count() > 0 && process.Errors != null)
                    {
                        SetErrorSessionMsgArr(process.Errors.ToList());
                    }

                    SetSuccessSessionMsg("Successfully Processed Quarterly Report File");
                }
                ModelState.Clear();
            }
            LoadRetroStatementPage(bo);
            model.Set(bo);
            return View("RetroStatement", model);
        }
    }
}