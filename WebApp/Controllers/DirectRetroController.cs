using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.SoaDatas;
using ConsoleApp.Commands;
using ConsoleApp.Commands.ProcessDatas;
using ConsoleApp.Commands.ProcessDatas.Exports;
using PagedList;
using Services;
using Services.RiDatas;
using Services.SoaDatas;
using Shared;
using System;
using System.Collections.Generic;
using System.Data;
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
    public class DirectRetroController : BaseController
    {
        public const string Controller = "DirectRetro";

        // GET: DirectRetro
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            int? CedantId,
            int? TreatyCodeId,
            string SoaQuarter,
            bool? IsProfitCommission,
            int? DaaStatus,
            int? RetroStatus,
            string SortOrder,
            int? Page
        )
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["CedantId"] = CedantId,
                ["TreatyCodeId"] = TreatyCodeId,
                ["SoaQuarter"] = SoaQuarter,
                ["DaaStatus"] = DaaStatus,
                ["IsProfitCommission"] = IsProfitCommission,
                ["RetroStatus"] = RetroStatus,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCedantId = GetSortParam("CedantId");
            ViewBag.SortTreatyCodeId = GetSortParam("TreatyCodeId");
            ViewBag.SortSoaQuarter = GetSortParam("SoaQuarter");
            ViewBag.SortIsProfitCommission = GetSortParam("IsProfitCommission");
            ViewBag.SortDaaStatus = GetSortParam("DaaStatus");
            ViewBag.SortRetroStatus = GetSortParam("RetroStatus");

            var hideDummyDirectRetro = Util.GetConfigBoolean("HideDummyDirectRetro", false);

            var query = _db.DirectRetro.Select(DirectRetroViewModel.Expression());

            if (hideDummyDirectRetro)
            {
                var dummyDirectRetro = Util.GetConfig("DummyDirectRetroIds");
                var dummyDirectRetroIds = Util.ToArraySplitTrim(dummyDirectRetro);
                query = query.Where(q => !dummyDirectRetroIds.Contains(q.Id.ToString()));
            }

            if (CedantId.HasValue) query = query.Where(q => q.CedantId == CedantId);
            if (TreatyCodeId.HasValue) query = query.Where(q => q.TreatyCodeId == TreatyCodeId);
            if (!string.IsNullOrEmpty(SoaQuarter)) query = query.Where(q => q.SoaQuarter == SoaQuarter);
            if (IsProfitCommission.HasValue) query = query.Where(q => q.SoaDataBatch.IsProfitCommissionData == IsProfitCommission);
            if (DaaStatus.HasValue) query = query.Where(q => q.SoaDataBatch.Status == DaaStatus);
            if (RetroStatus.HasValue) query = query.Where(q => q.RetroStatus == RetroStatus);

            if (SortOrder == Html.GetSortAsc("CedantId")) query = query.OrderBy(q => q.Cedant.Code);
            else if (SortOrder == Html.GetSortDsc("CedantId")) query = query.OrderByDescending(q => q.Cedant.Code);
            else if (SortOrder == Html.GetSortAsc("TreatyCodeId")) query = query.OrderBy(q => q.TreatyCode.Code);
            else if (SortOrder == Html.GetSortDsc("TreatyCodeId")) query = query.OrderByDescending(q => q.TreatyCode.Code);
            else if (SortOrder == Html.GetSortAsc("SoaQuarter")) query = query.OrderBy(q => q.SoaQuarter);
            else if (SortOrder == Html.GetSortDsc("SoaQuarter")) query = query.OrderByDescending(q => q.SoaQuarter);
            else if (SortOrder == Html.GetSortAsc("IsProfitCommission")) query = query.OrderBy(q => q.SoaDataBatch.IsProfitCommissionData);
            else if (SortOrder == Html.GetSortDsc("IsProfitCommission")) query = query.OrderByDescending(q => q.SoaDataBatch.IsProfitCommissionData);
            else if (SortOrder == Html.GetSortAsc("DaaStatus")) query = query.OrderBy(q => q.SoaDataBatch.Status);
            else if (SortOrder == Html.GetSortDsc("DaaStatus")) query = query.OrderByDescending(q => q.SoaDataBatch.Status);
            else if (SortOrder == Html.GetSortAsc("RetroStatus")) query = query.OrderBy(q => q.RetroStatus);
            else if (SortOrder == Html.GetSortDsc("RetroStatus")) query = query.OrderByDescending(q => q.RetroStatus);
            else query = query.OrderByDescending(q => q.Id);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: DirectRetro/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            ViewBag.Disabled = false;
            return View(new DirectRetroViewModel());
        }

        // POST: DirectRetro/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, DirectRetroViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);

                Result = DirectRetroService.Result();
                var duplicateResult = DirectRetroService.ValidateDuplicate(bo);

                if (!duplicateResult.Valid)
                {
                    Result.AddErrorRange(duplicateResult.ToErrorArray());
                }
                //else
                //{
                //    var mapResult = DirectRetroService.MapSoaDataBatch(ref bo);
                //    if (!mapResult.Valid)
                //        Result.AddErrorRange(mapResult.ToErrorArray());
                //}

                if (Result.Valid)
                {
                    var trail = GetNewTrailObject();
                    Result = DirectRetroService.Create(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        model.Id = bo.Id;
                        model.ProcessStatusHistory(form, AuthUserId, ref trail);

                        CreateTrail(
                            bo.Id,
                            "Create Direct Retro"
                        );

                        SetCreateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = bo.Id });
                    }
                }
                AddResult(Result);
            }

            LoadPage();
            ViewBag.Disabled = false;
            return View(model);
        }

        // GET: DirectRetro/Edit/5
        public ActionResult Edit(int id, int? RiDataPage, int? ClaimRegisterPage, int? TabIndex)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = DirectRetroService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var model = new DirectRetroViewModel(bo);

            // For RiData listing table
            ListRiData(
                bo,
                RiDataPage
            );

            // For Claim Data listing table
            ListClaimRegister(
                bo,
                ClaimRegisterPage
            );

            LoadPage(bo);
            ViewBag.RiDataPage = RiDataPage;
            ViewBag.ClaimRegisterPage = ClaimRegisterPage;
            ViewBag.ActiveTab = TabIndex;
            ViewBag.Disabled = false;
            return View(model);
        }

        // POST: DirectRetro/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, int? RiDataPage, int? ClaimRegisterPage, FormCollection form, DirectRetroViewModel model)
        {
            var dbBo = DirectRetroService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            Result = DirectRetroService.Result();
            if (ModelState.IsValid)
            {
                // Submit for approval
                if (dbBo.RetroStatus == DirectRetroBo.RetroStatusCompleted &&
                    model.RetroStatus == DirectRetroBo.RetroStatusPendingApproval && 
                    RetroStatementService.CountByDirectRetroIdByStatus(model.Id, RetroStatementBo.StatusPending) > 0)
                {
                    Result.AddError("Unable to submit for approval due to Retro Statement not finalised");
                }

                if (dbBo.RetroStatus == DirectRetroBo.RetroStatusPendingApproval && 
                    (model.RetroStatus == DirectRetroBo.RetroStatusApproved || model.RetroStatus == DirectRetroBo.RetroStatusCompleted))
                {
                    if (!CheckPower(Controller, AccessMatrixBo.PowerApprovalDirectRetro))
                        Result.AddError("You dont have access to Approve or Reject Direct Retro");
                    else if (dbBo.SoaDataBatchBo.Status != SoaDataBatchBo.StatusApproved && model.RetroStatus == DirectRetroBo.RetroStatusApproved)
                        Result.AddError("Unable to approve current Direct Retro as SOA Data has not been Approved");
                }

                if (Result.Valid)
                {
                    var bo = model.FormBo(dbBo.CreatedById, AuthUserId);

                    bo.Id = dbBo.Id;

                    var trail = GetNewTrailObject();

                    Result = DirectRetroService.Update(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        model.ProcessStatusHistory(form, AuthUserId, ref trail);
                        CreateTrail(
                            bo.Id,
                            "Update Direct Retro"
                        );

                        SetUpdateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = bo.Id });
                    }
                }
                AddResult(Result);
            }

            // For RiData listing table
            ListRiData(
                dbBo,
                RiDataPage
            );

            // For Claim Data listing table
            ListClaimRegister(
                dbBo,
                ClaimRegisterPage
            );

            LoadPage(dbBo);
            ViewBag.Disabled = false;
            return View(model);
        }

        // GET: DirectRetro/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            var bo = DirectRetroService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Disabled = true;
            LoadPage(bo);
            return View(new DirectRetroViewModel(bo));
        }

        // POST: DirectRetro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, DirectRetroViewModel model)
        {
            var bo = DirectRetroService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            if (bo.RetroStatus != DirectRetroBo.RetroStatusPending &&
                bo.RetroStatus != DirectRetroBo.RetroStatusFailed &&
                bo.RetroStatus != DirectRetroBo.RetroStatusCompleted)
            {
                SetErrorSessionMsg("Unable to delete the Direct Retro");
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = DirectRetroService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Direct Retro"
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadRetroStatement(HttpPostedFileBase upload, RetroStatementViewModel model)
        {
            var bo = model.FormBo(AuthUserId, AuthUserId);
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var process = new ProcessRetroStatement()
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

        // GET: DirectRetro/RetroStatement/5
        public ActionResult RetroStatement(int id, int directRetroId)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            RetroStatementBo bo;
            // New
            if (id == 0)
            {
                var directRetroBo = DirectRetroService.Find(directRetroId);
                bo = new RetroStatementBo
                {
                    DirectRetroId = directRetroId,
                    DirectRetroBo = DirectRetroService.Find(directRetroId),
                    CedingCompany = directRetroBo?.CedantBo?.Name,
                    TreatyCode = directRetroBo?.TreatyCodeBo?.Code,
                    TreatyType = directRetroBo?.TreatyCodeBo?.TreatyTypePickListDetailBo?.Code,
                    AccountsFor = directRetroBo?.TreatyCodeBo?.AccountFor,
                };
            }
            else
            {
                // Edit
                bo = RetroStatementService.Find(id);
                if (bo == null)
                {
                    return RedirectToAction("Edit", new { id = directRetroId });
                }
            }

            LoadRetroStatementPage(bo);
            return View(new RetroStatementViewModel(bo));
        }

        // POST: DirectRetro/RetroStatement/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult RetroStatement(RetroStatementViewModel model)
        {
            var bo = model.FormBo(AuthUserId, AuthUserId);
            bool isUpdate = false;

            if (ModelState.IsValid)
            {
                Result = RetroStatementService.Result();

                // Calculate Payment to the Reinsurer
                List<string> SwissReNames = new List<string> { "swissre", "swissresg" };
                RetroPartyBo retroPartyBo = RetroPartyService.Find(bo.RetroPartyId);
                if (retroPartyBo != null)
                {
                    string partyName = retroPartyBo.Party.Replace(" ", "").ToLower();
                    bo.CalculatePaymentToReinsuerer(SwissReNames.Contains(partyName));
                }

                if (Result.Valid)
                {
                    var trail = GetNewTrailObject();

                    if (bo.Id == 0)
                    {
                        if (bo.DirectRetroBo.RetroStatus == DirectRetroBo.RetroStatusPendingApproval ||
                            bo.DirectRetroBo.RetroStatus == DirectRetroBo.RetroStatusApproved ||
                            bo.DirectRetroBo.RetroStatus == DirectRetroBo.RetroStatusStatementIssuing || 
                            bo.DirectRetroBo.RetroStatus == DirectRetroBo.RetroStatusStatementIssued)
                        {
                            Result.AddError("Unable to add Retro Statement after \"Completed\" status");
                        }

                        if (Result.Valid)
                        {
                            Result = RetroStatementService.Create(ref bo, ref trail);
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
                        Result = RetroStatementService.Update(ref bo, ref trail);
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
                        return RedirectToAction("RetroStatement", new { id = bo.Id, directRetroId = bo.DirectRetroId });
                    }
                }
                AddResult(Result);
            }

            LoadRetroStatementPage(bo);
            return View(model);
        }

        // GET: DirectRetro/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteRetroStatement(int id, int directRetroId)
        {
            var bo = RetroStatementService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Edit", new { id = directRetroId });
            }

            ViewBag.Disabled = true;
            LoadRetroStatementPage(bo);
            return View(new RetroStatementViewModel(bo));
        }

        // POST: DirectRetro/DeleteRetroStatement/5
        [HttpPost, ActionName("DeleteRetroStatement")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteRetroStatementConfirmed(int id, RetroStatementViewModel model)
        {
            var bo = RetroStatementService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Edit", new { id = model.DirectRetroId });
            }

            var trail = GetNewTrailObject();
            Result = RetroStatementService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Retro Statement"
                );

                SetDeleteSuccessMessage("Retro Statement");
                return RedirectToAction("Edit", new { id = model.DirectRetroId });
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            LoadRetroStatementPage(bo);
            return RedirectToAction("DeleteRetroStatement", new { id = bo.Id, directRetroId = bo.DirectRetroId });
        }

        public void ListRiData(DirectRetroBo bo, int? page)
        {
            _db.Database.CommandTimeout = 0;
            List<string> transactionTypes = new List<string> { PickListDetailBo.TransactionTypeCodeNewBusiness, PickListDetailBo.TransactionTypeCodeRenewal, PickListDetailBo.TransactionTypeCodeAlteration };
            var query = _db.RiData.AsNoTracking()
                .Where(q => q.RiDataBatch.SoaDataBatchId == bo.SoaDataBatchId)
                .Where(q => q.TreatyCode == bo.TreatyCodeBo.Code)
                .Where(q => transactionTypes.Contains(q.TransactionTypeCode))
                .Select(RetroRiDataListingViewModel.Expression());

            if (bo.RetroStatus == DirectRetroBo.RetroStatusPending || 
                bo.RetroStatus == DirectRetroBo.RetroStatusSubmitForProcessing || 
                bo.RetroStatus == DirectRetroBo.RetroStatusProcessing)
            {
                query = query.Where(q => q.Id == 0);
            }

            query = query.OrderByDescending(q => q.PolicyNumber);

            ViewBag.RiDataTotal = query.Count();
            ViewBag.RiDataList = query.ToPagedList(page ?? 1, PageSize);
        }

        public void ListClaimRegister(DirectRetroBo bo, int? page)
        {
            _db.Database.CommandTimeout = 0;
            List<string> claimTransactionTypes = new List<string> { PickListDetailBo.ClaimTransactionTypeNew, PickListDetailBo.ClaimTransactionTypeAdjustment };
            var query = _db.ClaimRegister.AsNoTracking()
                .Where(q => q.SoaDataBatchId == bo.SoaDataBatchId)
                .Where(q => q.TreatyCode == bo.TreatyCodeBo.Code)
                .Where(q => claimTransactionTypes.Contains(q.ClaimTransactionType))
                .Where(q => q.TreatyType != PickListDetailBo.TreatyTypeTakaful)
                .Select(RetroClaimRegisterListingViewModel.Expression());

            if (bo.RetroStatus == DirectRetroBo.RetroStatusPending ||
                bo.RetroStatus == DirectRetroBo.RetroStatusSubmitForProcessing ||
                bo.RetroStatus == DirectRetroBo.RetroStatusProcessing)
            {
                query = query.Where(q => q.Id == 0);
            }

            query = query.OrderBy(q => q.EntryNo);

            ViewBag.ClaimRegisterTotal = query.Count();
            ViewBag.ClaimRegisterList = query.ToPagedList(page ?? 1, PageSize);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult DownloadRiData(DirectRetroViewModel model)
        {
            Response.SetCookie(new HttpCookie("downloadToken", ""));
            Session["lastActivity"] = DateTime.Now.Ticks;

            List<string> transactionTypes = new List<string> { PickListDetailBo.TransactionTypeCodeNewBusiness, PickListDetailBo.TransactionTypeCodeRenewal, PickListDetailBo.TransactionTypeCodeAlteration };

            TreatyCodeBo treatyCodeBo = TreatyCodeService.Find(model.TreatyCodeId);

            _db.Database.CommandTimeout = 0;
            var query = _db.RiData.Select(RiDataService.Expression())
                .Where(q => q.SoaDataBatchId == model.SoaDataBatchId)
                .Where(q => q.TreatyCode == treatyCodeBo.Code)
                .Where(q => transactionTypes.Contains(q.TransactionTypeCode));

            var export = new ExportDirectRetroRiData();
            export.HandleTempDirectory();
            export.SetQuery(query);
            export.Process();

            return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult DownloadClaim(DirectRetroViewModel model)
        {
            Response.SetCookie(new HttpCookie("downloadToken", ""));
            Session["lastActivity"] = DateTime.Now.Ticks;

            List<string> claimTransactionTypes = new List<string> { PickListDetailBo.ClaimTransactionTypeNew, PickListDetailBo.ClaimTransactionTypeAdjustment };

            TreatyCodeBo treatyCodeBo = TreatyCodeService.Find(model.TreatyCodeId);

            var query = _db.ClaimRegister.Select(ClaimRegisterService.Expression())
                .Where(q => q.SoaDataBatchId == model.SoaDataBatchId)
                .Where(q => q.TreatyCode == treatyCodeBo.Code)
                .Where(q => claimTransactionTypes.Contains(q.ClaimTransactionType))
                .Where(q => q.TreatyType != PickListDetailBo.TreatyTypeTakaful);

            var export = new ExportDirectRetroClaim();
            export.HandleTempDirectory();
            export.SetQuery(query);
            export.Process();

            return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult DownloadRetroSummary(DirectRetroViewModel model)
        {
            Response.SetCookie(new HttpCookie("downloadToken", ""));
            Session["lastActivity"] = DateTime.Now.Ticks;

            _db.Database.CommandTimeout = 0;
            var query = _db.RetroSummaries.Select(RetroSummaryService.Expression())
                .Where(q => q.DirectRetroId == model.Id)
                .Where(q => q.ReportingType == RetroSummaryBo.ReportingTypeIFRS4)
                .OrderBy(q => q.TreatyCode)
                .ThenByDescending(q => q.RiskQuarter)
                .ThenByDescending(q => q.Year)
                .ThenBy(q => q.Month);

            var export = new ExportRetroSummary(model.SoaQuarter);
            export.HandleTempDirectory();
            export.SetQuery(query);
            export.Process();

            return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));
        }

        public ActionResult StatusFileDownload(int directRetroId, int statusHistoryId)
        {
            DirectRetroStatusFileBo directRetroStatusFileBo = DirectRetroStatusFileService.FindByDirectRetroIdStatusHistoryId(directRetroId, statusHistoryId);
            return File(System.IO.File.ReadAllBytes(directRetroStatusFileBo.GetFilePath()), "text/plain", directRetroStatusFileBo.GetFilePath().Split('/').Last());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DownloadRetroStatement(int id)
        {
            RetroStatementBo retroStatementBo = RetroStatementService.Find(id);

            if (retroStatementBo != null && retroStatementBo.Status == RetroStatementBo.StatusFinalised)
            {
                try
                {
                    var process = new GenerateRetroStatement()
                    {
                        RetroStatementBo = retroStatementBo,
                    };
                    process.Process();

                    DownloadFile(process.FilePath, process.FileName);
                    Util.DeleteFiles(process.Directory, process.FileName);
                    return null;
                }
                catch (Exception)
                {
                    SetErrorSessionMsg("Error occured during processing Excel File");
                    return RedirectToAction("Edit", new { id = retroStatementBo.DirectRetroId });
                }
            }

            SetErrorSessionMsg("Retro Statement not found");
            return RedirectToAction("Index");
        }

        public void DownloadFile(string filePath, string fileName)
        {
            try
            {
                Response.ClearContent();
                Response.Clear();
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
        public JsonResult GetRetroConfigDetail(int directRetroId, int retroPartyId)
        {
            var configDetail = new DirectRetroConfigurationDetailBo();
            DirectRetroBo directRetroBo = DirectRetroService.Find(directRetroId);

            if (directRetroBo != null && retroPartyId != 0)
            {
                configDetail = DirectRetroConfigurationDetailService.FindByTreatyCodeIdRetroPartyId(directRetroBo.TreatyCodeId, retroPartyId);
            }

            return Json(new { ConfigDetail = configDetail });
        }

        [HttpPost]
        public JsonResult CalculateStatement(int directRetroId, int retroPartyId, string accountingPeriod1, string accountingPeriod2, string accountingPeriod3)
        {
            string error = null;
            var retroStatement = new RetroStatementBo();
            RetroPartyBo retroPartyBo = new RetroPartyBo();

            DirectRetroBo directRetroBo = DirectRetroService.Find(directRetroId);
            if (directRetroBo == null)
            {
                error = string.Format(MessageBag.NotExistsWithValue, "Direct Retro", directRetroId);
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
                IList<RetroSummaryBo> retroSummaryBos = RetroSummaryService.GetByDirectRetroIdReportingType(directRetroId, RetroSummaryBo.ReportingTypeIFRS4);
                IList<RetroSummaryBo> nbs = retroSummaryBos.Where(q => q.Type == PickListDetailBo.TransactionTypeCodeNewBusiness).ToList();
                IList<RetroSummaryBo> rns = retroSummaryBos.Where(q => q.Type == PickListDetailBo.TransactionTypeCodeRenewal).ToList();
                IList<RetroSummaryBo> als = retroSummaryBos.Where(q => q.Type == PickListDetailBo.TransactionTypeCodeAlteration).ToList();

                if (!string.IsNullOrEmpty(accountingPeriod1))
                {
                    QuarterObject qo = new QuarterObject(accountingPeriod1);
                    double claimAmount = 0;

                    if (nbs != null && nbs.Count() > 0)
                    {
                        IList<RetroSummaryBo> retro1 = nbs.Where(q => q.RetroParty1 == retroPartyBo.Party).Where(q => q.RiskQuarter == accountingPeriod1).ToList();
                        IList<RetroSummaryBo> retro2 = nbs.Where(q => q.RetroParty2 == retroPartyBo.Party).Where(q => q.RiskQuarter == accountingPeriod1).ToList();
                        IList<RetroSummaryBo> retro3 = nbs.Where(q => q.RetroParty3 == retroPartyBo.Party).Where(q => q.RiskQuarter == accountingPeriod1).ToList();

                        double riPremium = retro1.Sum(q => q.RetroRiPremium1) ?? 0;
                        riPremium += retro2.Sum(q => q.RetroRiPremium2) ?? 0;
                        riPremium += retro3.Sum(q => q.RetroRiPremium3) ?? 0;
                        retroStatement.RiPremiumNBStr = Util.DoubleToString(riPremium, 2);

                        double riDiscount = retro1.Sum(q => q.RetroDiscount1) ?? 0;
                        riDiscount += retro2.Sum(q => q.RetroDiscount2) ?? 0;
                        riDiscount += retro3.Sum(q => q.RetroDiscount3) ?? 0;
                        retroStatement.RiDiscountNBStr = Util.DoubleToString(riDiscount, 2);

                        //claimAmount += retro1.Sum(q => q.RetroClaims1) ?? 0;
                        //claimAmount += retro2.Sum(q => q.RetroClaims2) ?? 0;
                        //claimAmount += retro3.Sum(q => q.RetroClaims3) ?? 0;

                        retroStatement.TotalNoOfPolicyNB = RiDataService.CountNoOfPolicyByRetroParty(directRetroBo, PickListDetailBo.TransactionTypeCodeNewBusiness, retroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year);

                        retroStatement.TotalSumReinsuredNBStr = Util.DoubleToString(Util.RoundValue(RiDataService.CountSumRiAmountByRetroParty(directRetroBo, PickListDetailBo.TransactionTypeCodeNewBusiness, retroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year), 2), 2);
                    }

                    if (rns != null && rns.Count() > 0)
                    {
                        IList<RetroSummaryBo> retro1 = rns.Where(q => q.RetroParty1 == retroPartyBo.Party).Where(q => q.RiskQuarter == accountingPeriod1).ToList();
                        IList<RetroSummaryBo> retro2 = rns.Where(q => q.RetroParty2 == retroPartyBo.Party).Where(q => q.RiskQuarter == accountingPeriod1).ToList();
                        IList<RetroSummaryBo> retro3 = rns.Where(q => q.RetroParty3 == retroPartyBo.Party).Where(q => q.RiskQuarter == accountingPeriod1).ToList();

                        double riPremium = retro1.Sum(q => q.RetroRiPremium1) ?? 0;
                        riPremium += retro2.Sum(q => q.RetroRiPremium2) ?? 0;
                        riPremium += retro3.Sum(q => q.RetroRiPremium3) ?? 0;
                        retroStatement.RiPremiumRNStr = Util.DoubleToString(riPremium, 2);

                        double riDiscount = retro1.Sum(q => q.RetroDiscount1) ?? 0;
                        riDiscount += retro2.Sum(q => q.RetroDiscount2) ?? 0;
                        riDiscount += retro3.Sum(q => q.RetroDiscount3) ?? 0;
                        retroStatement.RiDiscountRNStr = Util.DoubleToString(riDiscount, 2);

                        //claimAmount += retro1.Sum(q => q.RetroClaims1) ?? 0;
                        //claimAmount += retro2.Sum(q => q.RetroClaims2) ?? 0;
                        //claimAmount += retro3.Sum(q => q.RetroClaims3) ?? 0;

                        retroStatement.TotalNoOfPolicyRN = RiDataService.CountNoOfPolicyByRetroParty(directRetroBo, PickListDetailBo.TransactionTypeCodeRenewal, retroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year);

                        retroStatement.TotalSumReinsuredRNStr = Util.DoubleToString(Util.RoundValue(RiDataService.CountSumRiAmountByRetroParty(directRetroBo, PickListDetailBo.TransactionTypeCodeRenewal, retroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year), 2), 2);
                    }

                    if (als != null && als.Count() > 0)
                    {
                        IList<RetroSummaryBo> retro1 = als.Where(q => q.RetroParty1 == retroPartyBo.Party).Where(q => q.RiskQuarter == accountingPeriod1).ToList();
                        IList<RetroSummaryBo> retro2 = als.Where(q => q.RetroParty2 == retroPartyBo.Party).Where(q => q.RiskQuarter == accountingPeriod1).ToList();
                        IList<RetroSummaryBo> retro3 = als.Where(q => q.RetroParty3 == retroPartyBo.Party).Where(q => q.RiskQuarter == accountingPeriod1).ToList();

                        double riPremium = retro1.Sum(q => q.RetroRiPremium1) ?? 0;
                        riPremium += retro2.Sum(q => q.RetroRiPremium2) ?? 0;
                        riPremium += retro3.Sum(q => q.RetroRiPremium3) ?? 0;
                        retroStatement.RiPremiumALTStr = Util.DoubleToString(riPremium, 2);

                        double riDiscount = retro1.Sum(q => q.RetroDiscount1) ?? 0;
                        riDiscount += retro2.Sum(q => q.RetroDiscount2) ?? 0;
                        riDiscount += retro3.Sum(q => q.RetroDiscount3) ?? 0;
                        retroStatement.RiDiscountALTStr = Util.DoubleToString(riDiscount, 2);

                        //claimAmount += retro1.Sum(q => q.RetroClaims1) ?? 0;
                        //claimAmount += retro2.Sum(q => q.RetroClaims2) ?? 0;
                        //claimAmount += retro3.Sum(q => q.RetroClaims3) ?? 0;

                        retroStatement.TotalNoOfPolicyALT = RiDataService.CountNoOfPolicyByRetroParty(directRetroBo, PickListDetailBo.TransactionTypeCodeAlteration, retroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year);

                        retroStatement.TotalSumReinsuredALTStr = Util.DoubleToString(Util.RoundValue(RiDataService.CountSumRiAmountByRetroParty(directRetroBo, PickListDetailBo.TransactionTypeCodeAlteration, retroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year), 2), 2);
                    }

                    if (directRetroBo.SoaQuarter == accountingPeriod1)
                    {
                        IList<RetroSummaryBo> claimRetro1 = retroSummaryBos.Where(q => q.RetroParty1 == retroPartyBo.Party).ToList();
                        IList<RetroSummaryBo> claimRetro2 = retroSummaryBos.Where(q => q.RetroParty2 == retroPartyBo.Party).ToList();
                        IList<RetroSummaryBo> claimRetro3 = retroSummaryBos.Where(q => q.RetroParty3 == retroPartyBo.Party).ToList();

                        claimAmount += claimRetro1.Sum(q => q.RetroClaims1) ?? 0;
                        claimAmount += claimRetro2.Sum(q => q.RetroClaims2) ?? 0;
                        claimAmount += claimRetro3.Sum(q => q.RetroClaims3) ?? 0;
                    }

                    retroStatement.ClaimsStr = Util.DoubleToString(claimAmount, 2);
                    retroStatement.NoClaimBonusStr = Util.DoubleToString(Util.RoundValue(RiDataService.CountTotalNoClaimBonus(directRetroBo, retroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year), 2), 2);
                    retroStatement.AgreedDatabaseCommStr = Util.DoubleToString(Util.RoundValue(RiDataService.CountTotalDatabaseCommission(directRetroBo, retroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year), 2), 2);
                }

                if (!string.IsNullOrEmpty(accountingPeriod2))
                {
                    QuarterObject qo = new QuarterObject(accountingPeriod2);
                    double claimAmount2 = 0;

                    if (nbs != null && nbs.Count() > 0)
                    {
                        IList<RetroSummaryBo> retro1 = nbs.Where(q => q.RetroParty1 == retroPartyBo.Party).Where(q => q.RiskQuarter == accountingPeriod2).ToList();
                        IList<RetroSummaryBo> retro2 = nbs.Where(q => q.RetroParty2 == retroPartyBo.Party).Where(q => q.RiskQuarter == accountingPeriod2).ToList();
                        IList<RetroSummaryBo> retro3 = nbs.Where(q => q.RetroParty3 == retroPartyBo.Party).Where(q => q.RiskQuarter == accountingPeriod2).ToList();

                        double riPremium2 = retro1.Sum(q => q.RetroRiPremium1) ?? 0;
                        riPremium2 += retro2.Sum(q => q.RetroRiPremium2) ?? 0;
                        riPremium2 += retro3.Sum(q => q.RetroRiPremium3) ?? 0;
                        retroStatement.RiPremiumNB2Str = Util.DoubleToString(riPremium2, 2);

                        double riDiscount2 = retro1.Sum(q => q.RetroDiscount1) ?? 0;
                        riDiscount2 += retro2.Sum(q => q.RetroDiscount2) ?? 0;
                        riDiscount2 += retro3.Sum(q => q.RetroDiscount3) ?? 0;
                        retroStatement.RiDiscountNB2Str = Util.DoubleToString(riDiscount2, 2);

                        //claimAmount2 += retro1.Sum(q => q.RetroClaims1) ?? 0;
                        //claimAmount2 += retro2.Sum(q => q.RetroClaims2) ?? 0;
                        //claimAmount2 += retro3.Sum(q => q.RetroClaims3) ?? 0;

                        retroStatement.TotalNoOfPolicyNB2 = RiDataService.CountNoOfPolicyByRetroParty(directRetroBo, PickListDetailBo.TransactionTypeCodeNewBusiness, retroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year);

                        retroStatement.TotalSumReinsuredNB2Str = Util.DoubleToString(Util.RoundValue(RiDataService.CountSumRiAmountByRetroParty(directRetroBo, PickListDetailBo.TransactionTypeCodeNewBusiness, retroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year), 2), 2);
                    }

                    if (rns != null && rns.Count() > 0)
                    {
                        IList<RetroSummaryBo> retro1 = rns.Where(q => q.RetroParty1 == retroPartyBo.Party).Where(q => q.RiskQuarter == accountingPeriod2).ToList();
                        IList<RetroSummaryBo> retro2 = rns.Where(q => q.RetroParty2 == retroPartyBo.Party).Where(q => q.RiskQuarter == accountingPeriod2).ToList();
                        IList<RetroSummaryBo> retro3 = rns.Where(q => q.RetroParty3 == retroPartyBo.Party).Where(q => q.RiskQuarter == accountingPeriod2).ToList();

                        double riPremium2 = retro1.Sum(q => q.RetroRiPremium1) ?? 0;
                        riPremium2 += retro2.Sum(q => q.RetroRiPremium2) ?? 0;
                        riPremium2 += retro3.Sum(q => q.RetroRiPremium3) ?? 0;
                        retroStatement.RiPremiumRN2Str = Util.DoubleToString(riPremium2, 2);

                        double riDiscount2 = retro1.Sum(q => q.RetroDiscount1) ?? 0;
                        riDiscount2 += retro2.Sum(q => q.RetroDiscount2) ?? 0;
                        riDiscount2 += retro3.Sum(q => q.RetroDiscount3) ?? 0;
                        retroStatement.RiDiscountRN2Str = Util.DoubleToString(riDiscount2, 2);

                        //claimAmount2 += retro1.Sum(q => q.RetroClaims1) ?? 0;
                        //claimAmount2 += retro2.Sum(q => q.RetroClaims2) ?? 0;
                        //claimAmount2 += retro3.Sum(q => q.RetroClaims3) ?? 0;

                        retroStatement.TotalNoOfPolicyRN2 = RiDataService.CountNoOfPolicyByRetroParty(directRetroBo, PickListDetailBo.TransactionTypeCodeRenewal, retroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year);

                        retroStatement.TotalSumReinsuredRN2Str = Util.DoubleToString(Util.RoundValue(RiDataService.CountSumRiAmountByRetroParty(directRetroBo, PickListDetailBo.TransactionTypeCodeRenewal, retroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year), 2), 2);
                    }

                    if (als != null && als.Count() > 0)
                    {
                        IList<RetroSummaryBo> retro1 = als.Where(q => q.RetroParty1 == retroPartyBo.Party).Where(q => q.RiskQuarter == accountingPeriod2).ToList();
                        IList<RetroSummaryBo> retro2 = als.Where(q => q.RetroParty2 == retroPartyBo.Party).Where(q => q.RiskQuarter == accountingPeriod2).ToList();
                        IList<RetroSummaryBo> retro3 = als.Where(q => q.RetroParty3 == retroPartyBo.Party).Where(q => q.RiskQuarter == accountingPeriod2).ToList();

                        double riPremium2 = retro1.Sum(q => q.RetroRiPremium1) ?? 0;
                        riPremium2 += retro2.Sum(q => q.RetroRiPremium2) ?? 0;
                        riPremium2 += retro3.Sum(q => q.RetroRiPremium3) ?? 0;
                        retroStatement.RiPremiumALT2Str = Util.DoubleToString(riPremium2, 2);

                        double riDiscount2 = retro1.Sum(q => q.RetroDiscount1) ?? 0;
                        riDiscount2 += retro2.Sum(q => q.RetroDiscount2) ?? 0;
                        riDiscount2 += retro3.Sum(q => q.RetroDiscount3) ?? 0;
                        retroStatement.RiDiscountALT2Str = Util.DoubleToString(riDiscount2, 2);

                        //claimAmount2 += retro1.Sum(q => q.RetroClaims1) ?? 0;
                        //claimAmount2 += retro2.Sum(q => q.RetroClaims2) ?? 0;
                        //claimAmount2 += retro3.Sum(q => q.RetroClaims3) ?? 0;

                        retroStatement.TotalNoOfPolicyALT2 = RiDataService.CountNoOfPolicyByRetroParty(directRetroBo, PickListDetailBo.TransactionTypeCodeAlteration, retroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year);

                        retroStatement.TotalSumReinsuredALT2Str = Util.DoubleToString(Util.RoundValue(RiDataService.CountSumRiAmountByRetroParty(directRetroBo, PickListDetailBo.TransactionTypeCodeAlteration, retroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year), 2), 2);
                    }

                    if (directRetroBo.SoaQuarter == accountingPeriod2)
                    {
                        IList<RetroSummaryBo> claimRetro1 = retroSummaryBos.Where(q => q.RetroParty1 == retroPartyBo.Party).ToList();
                        IList<RetroSummaryBo> claimRetro2 = retroSummaryBos.Where(q => q.RetroParty2 == retroPartyBo.Party).ToList();
                        IList<RetroSummaryBo> claimRetro3 = retroSummaryBos.Where(q => q.RetroParty3 == retroPartyBo.Party).ToList();

                        claimAmount2 += claimRetro1.Sum(q => q.RetroClaims1) ?? 0;
                        claimAmount2 += claimRetro2.Sum(q => q.RetroClaims2) ?? 0;
                        claimAmount2 += claimRetro3.Sum(q => q.RetroClaims3) ?? 0;
                    }

                    retroStatement.Claims2Str = Util.DoubleToString(claimAmount2, 2);
                    retroStatement.NoClaimBonus2Str = Util.DoubleToString(Util.RoundValue(RiDataService.CountTotalNoClaimBonus(directRetroBo, retroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year), 2), 2);
                    retroStatement.AgreedDatabaseComm2Str = Util.DoubleToString(Util.RoundValue(RiDataService.CountTotalDatabaseCommission(directRetroBo, retroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year), 2), 2);
                }

                if (!string.IsNullOrEmpty(accountingPeriod3))
                {
                    QuarterObject qo = new QuarterObject(accountingPeriod3);
                    double claimAmount3 = 0;

                    if (nbs != null && nbs.Count() > 0)
                    {
                        IList<RetroSummaryBo> retro1 = nbs.Where(q => q.RetroParty1 == retroPartyBo.Party).Where(q => q.RiskQuarter == accountingPeriod3).ToList();
                        IList<RetroSummaryBo> retro2 = nbs.Where(q => q.RetroParty2 == retroPartyBo.Party).Where(q => q.RiskQuarter == accountingPeriod3).ToList();
                        IList<RetroSummaryBo> retro3 = nbs.Where(q => q.RetroParty3 == retroPartyBo.Party).Where(q => q.RiskQuarter == accountingPeriod3).ToList();

                        double riPremium3 = retro1.Sum(q => q.RetroRiPremium1) ?? 0;
                        riPremium3 += retro2.Sum(q => q.RetroRiPremium2) ?? 0;
                        riPremium3 += retro3.Sum(q => q.RetroRiPremium3) ?? 0;
                        retroStatement.RiPremiumNB3Str = Util.DoubleToString(riPremium3, 2);

                        double riDiscount3 = retro1.Sum(q => q.RetroDiscount1) ?? 0;
                        riDiscount3 += retro2.Sum(q => q.RetroDiscount2) ?? 0;
                        riDiscount3 += retro3.Sum(q => q.RetroDiscount3) ?? 0;
                        retroStatement.RiDiscountNB3Str = Util.DoubleToString(riDiscount3, 2);

                        //claimAmount3 += retro1.Sum(q => q.RetroClaims1) ?? 0;
                        //claimAmount3 += retro2.Sum(q => q.RetroClaims2) ?? 0;
                        //claimAmount3 += retro3.Sum(q => q.RetroClaims3) ?? 0;

                        retroStatement.TotalNoOfPolicyNB3 = RiDataService.CountNoOfPolicyByRetroParty(directRetroBo, PickListDetailBo.TransactionTypeCodeNewBusiness, retroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year);

                        retroStatement.TotalSumReinsuredNB3Str = Util.DoubleToString(Util.RoundValue(RiDataService.CountSumRiAmountByRetroParty(directRetroBo, PickListDetailBo.TransactionTypeCodeNewBusiness, retroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year), 2), 2);
                    }

                    if (rns != null && rns.Count() > 0)
                    {
                        IList<RetroSummaryBo> retro1 = rns.Where(q => q.RetroParty1 == retroPartyBo.Party).Where(q => q.RiskQuarter == accountingPeriod3).ToList();
                        IList<RetroSummaryBo> retro2 = rns.Where(q => q.RetroParty2 == retroPartyBo.Party).Where(q => q.RiskQuarter == accountingPeriod3).ToList();
                        IList<RetroSummaryBo> retro3 = rns.Where(q => q.RetroParty3 == retroPartyBo.Party).Where(q => q.RiskQuarter == accountingPeriod3).ToList();

                        double riPremium3 = retro1.Sum(q => q.RetroRiPremium1) ?? 0;
                        riPremium3 += retro2.Sum(q => q.RetroRiPremium2) ?? 0;
                        riPremium3 += retro3.Sum(q => q.RetroRiPremium3) ?? 0;
                        retroStatement.RiPremiumRN3Str = Util.DoubleToString(riPremium3, 2);

                        double riDiscount3 = retro1.Sum(q => q.RetroDiscount1) ?? 0;
                        riDiscount3 += retro2.Sum(q => q.RetroDiscount2) ?? 0;
                        riDiscount3 += retro3.Sum(q => q.RetroDiscount3) ?? 0;
                        retroStatement.RiDiscountRN3Str = Util.DoubleToString(riDiscount3, 2);

                        //claimAmount3 += retro1.Sum(q => q.RetroClaims1) ?? 0;
                        //claimAmount3 += retro2.Sum(q => q.RetroClaims2) ?? 0;
                        //claimAmount3 += retro3.Sum(q => q.RetroClaims3) ?? 0;

                        retroStatement.TotalNoOfPolicyRN3 = RiDataService.CountNoOfPolicyByRetroParty(directRetroBo, PickListDetailBo.TransactionTypeCodeRenewal, retroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year);

                        retroStatement.TotalSumReinsuredRN3Str = Util.DoubleToString(Util.RoundValue(RiDataService.CountSumRiAmountByRetroParty(directRetroBo, PickListDetailBo.TransactionTypeCodeRenewal, retroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year), 2), 2);
                    }

                    if (als != null && als.Count() > 0)
                    {
                        IList<RetroSummaryBo> retro1 = als.Where(q => q.RetroParty1 == retroPartyBo.Party).Where(q => q.RiskQuarter == accountingPeriod3).ToList();
                        IList<RetroSummaryBo> retro2 = als.Where(q => q.RetroParty2 == retroPartyBo.Party).Where(q => q.RiskQuarter == accountingPeriod3).ToList();
                        IList<RetroSummaryBo> retro3 = als.Where(q => q.RetroParty3 == retroPartyBo.Party).Where(q => q.RiskQuarter == accountingPeriod3).ToList();

                        double riPremium3 = retro1.Sum(q => q.RetroRiPremium1) ?? 0;
                        riPremium3 += retro2.Sum(q => q.RetroRiPremium2) ?? 0;
                        riPremium3 += retro3.Sum(q => q.RetroRiPremium3) ?? 0;
                        retroStatement.RiPremiumALT3Str = Util.DoubleToString(riPremium3, 2);

                        double riDiscount3 = retro1.Sum(q => q.RetroDiscount1) ?? 0;
                        riDiscount3 += retro2.Sum(q => q.RetroDiscount2) ?? 0;
                        riDiscount3 += retro3.Sum(q => q.RetroDiscount3) ?? 0;
                        retroStatement.RiDiscountALT3Str = Util.DoubleToString(riDiscount3, 2);

                        //claimAmount3 += retro1.Sum(q => q.RetroClaims1) ?? 0;
                        //claimAmount3 += retro2.Sum(q => q.RetroClaims2) ?? 0;
                        //claimAmount3 += retro3.Sum(q => q.RetroClaims3) ?? 0;

                        retroStatement.TotalNoOfPolicyALT3 = RiDataService.CountNoOfPolicyByRetroParty(directRetroBo, PickListDetailBo.TransactionTypeCodeAlteration, retroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year);

                        retroStatement.TotalSumReinsuredALT3Str = Util.DoubleToString(Util.RoundValue(RiDataService.CountSumRiAmountByRetroParty(directRetroBo, PickListDetailBo.TransactionTypeCodeAlteration, retroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year), 2), 2);
                    }

                    if (directRetroBo.SoaQuarter == accountingPeriod3)
                    {
                        IList<RetroSummaryBo> claimRetro1 = retroSummaryBos.Where(q => q.RetroParty1 == retroPartyBo.Party).ToList();
                        IList<RetroSummaryBo> claimRetro2 = retroSummaryBos.Where(q => q.RetroParty2 == retroPartyBo.Party).ToList();
                        IList<RetroSummaryBo> claimRetro3 = retroSummaryBos.Where(q => q.RetroParty3 == retroPartyBo.Party).ToList();

                        claimAmount3 += claimRetro1.Sum(q => q.RetroClaims1) ?? 0;
                        claimAmount3 += claimRetro2.Sum(q => q.RetroClaims2) ?? 0;
                        claimAmount3 += claimRetro3.Sum(q => q.RetroClaims3) ?? 0;
                    }

                    retroStatement.Claims3Str = Util.DoubleToString(claimAmount3, 2);
                    retroStatement.NoClaimBonus3Str = Util.DoubleToString(Util.RoundValue(RiDataService.CountTotalNoClaimBonus(directRetroBo, retroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year), 2), 2);
                    retroStatement.AgreedDatabaseComm3Str = Util.DoubleToString(Util.RoundValue(RiDataService.CountTotalDatabaseCommission(directRetroBo, retroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year), 2), 2);
                }
            }

            return Json(new { RetroStatement = retroStatement, Error = error });
        }

        public ActionResult DownloadTemplate(int type)
        {
            string template;
            string filename;
            if (type == 1)
            {
                template = "RetroStatement_Default_Template.xlsx";
                filename = string.Format("Default_Template").AppendDateTimeFileName(".xlsx");
            }
            else
            {
                template = "RetroStatement_SwissRe_Template.xlsx";
                filename = string.Format("SwissRe_Template").AppendDateTimeFileName(".xlsx");
            }

            var templateFilepath = Util.GetWebAppDocumentFilePath(template);

            DownloadFile(templateFilepath, filename);
            return null;
        }

        public void IndexPage()
        {
            DropDownEmpty();
            DropDownCedant();
            DropDownDaaStatus();
            DropDownYesNoWithSelect();
            DropDownRetroStatus();
            SetViewBagMessage();
        }

        public void LoadPage(DirectRetroBo bo = null)
        {
            AuthUserName();
            DropDownEmpty();
            if (bo == null)
            {
                // Create
                DropDownCedant(CedantBo.StatusActive);
            }
            else
            {
                // Edit
                DropDownCedant(CedantBo.StatusActive, bo.CedantId);
                GetRetroSummaryList(bo);
                GetStatusHistoryList(bo);
                GetStatusFileList(bo);
                GetRetroStatamentList(bo);
                EnableSubmitForProcessing(bo);
                EnableSubmitForApproval(bo);
                EnableApproval(bo);
                EnableAddRetroStatement(bo);
                EnableDownloadData(bo);
                EnableDelete(bo);

                if (bo.CedantBo != null && bo.CedantBo.Status == CedantBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.CedantStatusInactive);
                }
                if (bo.TreatyCodeBo != null && bo.TreatyCodeBo.Status == TreatyCodeBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.TreatyCodeStatusInactive);
                }
            }
            SetViewBagMessage();
        }

        public void LoadRetroStatementPage(RetroStatementBo bo = null)
        {
            if (bo == null)
            {
                DropDownRetroPartyByRetroConfig(bo.DirectRetroBo.TreatyCodeId, bo.RetroPartyId);
            }
            else
            {
                DropDownRetroPartyByRetroConfig(bo.DirectRetroBo.TreatyCodeId, bo.RetroPartyId);
                if (bo.RetroPartyBo != null && bo.RetroPartyBo.Status == RetroPartyBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.RetroPartyStatusInactive);
                }
            }
            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownDaaStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= SoaDataBatchBo.StatusMax; i++)
            {
                items.Add(new SelectListItem { Text = SoaDataBatchBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownDaaStatuses = items;
            return items;
        }

        public List<SelectListItem> DropDownRetroStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= DirectRetroBo.RetroStatusMax; i++)
            {
                items.Add(new SelectListItem { Text = DirectRetroBo.GetRetroStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownRetroStatuses = items;
            return items;
        }

        public void GetRetroSummaryList(DirectRetroBo bo)
        {
            if (bo.RetroStatus != DirectRetroBo.RetroStatusPending &&
                bo.RetroStatus != DirectRetroBo.RetroStatusProcessing)
            {
                IList<RetroSummaryBo> retroSummaryBos = RetroSummaryService.GetByDirectRetroIdReportingType(bo.Id, RetroSummaryBo.ReportingTypeIFRS4);
                ViewBag.RetroSummaryBos = retroSummaryBos;

                RetroSummaryBo retroSummaryBo = new RetroSummaryBo
                {
                    TreatyCode = "Total",
                    NoOfPolicy = retroSummaryBos.Sum(q => q.NoOfPolicy),
                    TotalSarStr = Util.DoubleToString(retroSummaryBos.Sum(q => q.TotalSar), 2),
                    TotalDirectRetroAarStr = Util.DoubleToString(retroSummaryBos.Sum(q => q.TotalDirectRetroAar), 2),
                    TotalRiPremiumStr = Util.DoubleToString(retroSummaryBos.Sum(q => q.TotalRiPremium), 2),
                    TotalDiscountStr = Util.DoubleToString(retroSummaryBos.Sum(q => q.TotalDiscount), 2),
                    NoOfClaims = retroSummaryBos.Sum(q => q.NoOfClaims),
                    RetroRiPremium1Str = Util.DoubleToString(retroSummaryBos.Sum(q => q.RetroRiPremium1), 2),
                    RetroDiscount1Str = Util.DoubleToString(retroSummaryBos.Sum(q => q.RetroDiscount1), 2),
                    RetroClaims1Str = Util.DoubleToString(retroSummaryBos.Sum(q => q.RetroClaims1), 2),
                    RetroRiPremium2Str = Util.DoubleToString(retroSummaryBos.Sum(q => q.RetroRiPremium2), 2),
                    RetroDiscount2Str = Util.DoubleToString(retroSummaryBos.Sum(q => q.RetroDiscount2), 2),
                    RetroClaims2Str = Util.DoubleToString(retroSummaryBos.Sum(q => q.RetroClaims2), 2),
                    RetroRiPremium3Str = Util.DoubleToString(retroSummaryBos.Sum(q => q.RetroRiPremium3), 2),
                    RetroDiscount3Str = Util.DoubleToString(retroSummaryBos.Sum(q => q.RetroDiscount3), 2),
                    RetroClaims3Str = Util.DoubleToString(retroSummaryBos.Sum(q => q.RetroClaims3), 2),
                };

                ViewBag.TotalRetroSummaryBo = retroSummaryBo;
                return;
            }

            ViewBag.RetroSummaryBos = null;
        }

        public void GetStatusHistoryList(DirectRetroBo bo)
        {
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.DirectRetro.ToString());
            // StatusHistories
            IList<StatusHistoryBo> statusHistoryBos = StatusHistoryService.GetStatusHistoriesByModuleIdObjectId(moduleBo.Id, bo.Id);
            ViewBag.StatusHistories = statusHistoryBos;
        }

        public void GetStatusFileList(DirectRetroBo bo)
        {
            IList<DirectRetroStatusFileBo> statusFiles = DirectRetroStatusFileService.GetByDirectRetroId(bo.Id);
            ViewBag.StatusFiles = statusFiles;
        }

        public void GetRetroStatamentList(DirectRetroBo bo)
        {
            IList<RetroStatementBo> retroStatementBos = RetroStatementService.GetByDirectRetroId(bo.Id);
            ViewBag.RetroStatements = retroStatementBos;
        }

        public void EnableSubmitForProcessing(DirectRetroBo bo)
        {
            bool isEnable = (bo.RetroStatus == DirectRetroBo.RetroStatusPending ||
                bo.RetroStatus == DirectRetroBo.RetroStatusCompleted ||
                bo.RetroStatus == DirectRetroBo.RetroStatusFailed);

            // Requested to remove validation on SoaDataBatch Status
            //bo.SoaDataBatchBo.Status == SoaDataBatchBo.StatusApproved;

            ViewBag.EnableSubmit = isEnable;
        }

        public void EnableSubmitForApproval(DirectRetroBo bo)
        {
            bool isEnable = (bo.RetroStatus == DirectRetroBo.RetroStatusCompleted && RetroStatementService.CountByDirectRetroIdByStatus(bo.Id, RetroStatementBo.StatusPending) == 0);

            ViewBag.EnableSubmitForApproval = isEnable;
        }

        public void EnableApproval(DirectRetroBo bo)
        {
            bool isEnable = (bo.RetroStatus == DirectRetroBo.RetroStatusPendingApproval &&
                CheckPower(Controller, AccessMatrixBo.PowerApprovalDirectRetro));

            ViewBag.EnableApproval = isEnable;
        }

        public void EnableAddRetroStatement(DirectRetroBo bo)
        {
            bool isEnable = (bo.RetroStatus == DirectRetroBo.RetroStatusPending ||
                bo.RetroStatus == DirectRetroBo.RetroStatusSubmitForProcessing ||
                bo.RetroStatus == DirectRetroBo.RetroStatusCompleted ||
                bo.RetroStatus == DirectRetroBo.RetroStatusFailed);

            ViewBag.EnableAddRetroStatement = isEnable;
        }

        public void EnableDownloadData(DirectRetroBo bo)
        {
            bool isEnable = (bo.RetroStatus == DirectRetroBo.RetroStatusCompleted ||
                bo.RetroStatus == DirectRetroBo.RetroStatusFailed ||
                bo.RetroStatus == DirectRetroBo.RetroStatusPendingApproval ||
                bo.RetroStatus == DirectRetroBo.RetroStatusApproved ||
                bo.RetroStatus == DirectRetroBo.RetroStatusStatementIssuing ||
                bo.RetroStatus == DirectRetroBo.RetroStatusStatementIssued );

            ViewBag.EnableDownloadData = isEnable;
        }

        public void EnableDelete(DirectRetroBo bo)
        {
            bool isEnable = (bo.RetroStatus == DirectRetroBo.RetroStatusPending ||
                bo.RetroStatus == DirectRetroBo.RetroStatusFailed ||
                bo.RetroStatus == DirectRetroBo.RetroStatusCompleted);

            ViewBag.EnableDelete = isEnable;
        }

        [HttpPost]
        public JsonResult GetSoaDataBatch(int cedantId, int treatyCodeId, string soaQuarter)
        {
            var soaDataBatches = new List<SoaDataBatchBo>() { };
            TreatyCodeBo treatyCodeBo = TreatyCodeService.Find(treatyCodeId);

            if (treatyCodeBo == null)
                return Json(new { SoaDataBatches = soaDataBatches });

            soaDataBatches = SoaDataBatchService.GetByParam(cedantId, treatyCodeBo.TreatyId, soaQuarter)?.ToList();
            
            return Json(new { SoaDataBatches = soaDataBatches });
        }
    }
}
