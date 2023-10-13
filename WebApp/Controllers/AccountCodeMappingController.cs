using BusinessObject;
using BusinessObject.Identity;
using ConsoleApp.Commands.ProcessDatas;
using ConsoleApp.Commands.ProcessDatas.Exports;
using PagedList;
using Services;
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
    public class AccountCodeMappingController : BaseController
    {
        public const string Controller = "AccountCodeMapping";

        // GET: AccountCodeMapping
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            int? Type,
            string TreatyType,
            int? TreatyCodeId,
            string ClaimCode,
            string BusinessOrigin,
            int? TransTypeCodeId,
            int? ModifiedContractCodeId,
            string InvoiceField,
            int? ReportingType,
            int? AccountCodeId,
            bool? IsBalanceSheet,
            int? DebitCreditIndicatorPositive,
            int? DebitCreditIndicatorNegative,
            string SortOrder,
            int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Type"] = Type,
                ["TreatyType"] = TreatyType,
                ["TreatyCodeId"] = TreatyCodeId,
                ["ClaimCode"] = ClaimCode,
                ["BusinessOrigin"] = BusinessOrigin,
                ["TransTypeCodeId"] = TransTypeCodeId,
                ["ModifiedContractCodeId"] = ModifiedContractCodeId,
                ["InvoiceField"] = InvoiceField,
                ["ReportingType"] = ReportingType,
                ["AccountCodeId"] = AccountCodeId,
                ["IsBalanceSheet"] = IsBalanceSheet,
                ["DebitCreditIndicatorPositive"] = DebitCreditIndicatorPositive,
                ["DebitCreditIndicatorNegative"] = DebitCreditIndicatorNegative,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortType = GetSortParam("Type");
            ViewBag.SortTreatyCode = GetSortParam("TreatyCodeId");
            ViewBag.SortTransTypeCode = GetSortParam("TransTypeCodeId");
            ViewBag.SortModifiedContractCode = GetSortParam("ModifiedContractCodeId");
            ViewBag.SortInvoiceField = GetSortParam("InvoiceField");
            ViewBag.SortReportingType = GetSortParam("ReportingType");
            ViewBag.SortAccountCode = GetSortParam("AccountCodeId");
            ViewBag.SortIsBalanceSheet = GetSortParam("IsBalanceSheet");
            ViewBag.SortDebitCreditIndicatorPositive = GetSortParam("DebitCreditIndicatorPositive");
            ViewBag.SortDebitCreditIndicatorNegative = GetSortParam("DebitCreditIndicatorNegative");

            var query = _db.AccountCodeMappings.Select(AccountCodeMappingViewModel.Expression())
                .Where(q => q.Type == AccountCodeMappingBo.TypeClaimProvision || q.Type == AccountCodeMappingBo.TypeClaimRecovery || q.Type == AccountCodeMappingBo.TypeCedantAccountCode);

            if (Type != null) query = query.Where(q => q.Type == Type);
            if (!string.IsNullOrEmpty(TreatyType)) query = query.Where(q => q.AccountCodeMappingDetails.Any(d => d.TreatyType == TreatyType));
            if (!string.IsNullOrEmpty(ClaimCode)) query = query.Where(q => q.AccountCodeMappingDetails.Any(d => d.ClaimCode == ClaimCode));
            if (!string.IsNullOrEmpty(BusinessOrigin)) query = query.Where(q => q.AccountCodeMappingDetails.Any(d => d.BusinessOrigin == BusinessOrigin));
            if (TreatyCodeId != null) query = query.Where(q => q.TreatyCodeId == TreatyCodeId);
            if (TransTypeCodeId != null) query = query.Where(q => q.TransactionTypeCodePickListDetailId == TransTypeCodeId);
            if (ModifiedContractCodeId != null) query = query.Where(q => q.ModifiedContractCodeId == ModifiedContractCodeId);
            if (!string.IsNullOrEmpty(InvoiceField)) query = query.Where(q => q.AccountCodeMappingDetails.Any(d => d.InvoiceField == InvoiceField));
            if (ReportingType != null) query = query.Where(q => q.AccountCode.ReportingType == ReportingType);
            if (AccountCodeId != null) query = query.Where(q => q.AccountCodeId == AccountCodeId);
            if (IsBalanceSheet != null) query = query.Where(q => q.IsBalanceSheet == IsBalanceSheet);
            if (DebitCreditIndicatorPositive != null) query = query.Where(q => q.DebitCreditIndicatorPositive == DebitCreditIndicatorPositive);
            if (DebitCreditIndicatorNegative != null) query = query.Where(q => q.DebitCreditIndicatorNegative == DebitCreditIndicatorNegative);

            if (SortOrder == Html.GetSortAsc("Type")) query = query.OrderBy(q => q.Type);
            else if (SortOrder == Html.GetSortDsc("Type")) query = query.OrderByDescending(q => q.Type);
            else if (SortOrder == Html.GetSortAsc("TreatyCodeId")) query = query.OrderBy(q => q.TreatyCode.Code);
            else if (SortOrder == Html.GetSortDsc("TreatyCodeId")) query = query.OrderByDescending(q => q.TreatyCode.Code);
            else if (SortOrder == Html.GetSortAsc("TransTypeCodeId")) query = query.OrderBy(q => q.TransactionTypeCodePickListDetail.Code);
            else if (SortOrder == Html.GetSortDsc("TransTypeCodeId")) query = query.OrderByDescending(q => q.TransactionTypeCodePickListDetail.Code);
            else if (SortOrder == Html.GetSortAsc("ModifiedContractCodeId")) query = query.OrderBy(q => q.ModifiedContractCodeId);
            else if (SortOrder == Html.GetSortDsc("ModifiedContractCodeId")) query = query.OrderByDescending(q => q.ModifiedContractCodeId);
            else if (SortOrder == Html.GetSortAsc("ReportingType")) query = query.OrderBy(q => q.AccountCode.ReportingType);
            else if (SortOrder == Html.GetSortDsc("ReportingType")) query = query.OrderByDescending(q => q.AccountCode.ReportingType);
            else if (SortOrder == Html.GetSortAsc("AccountCodeId")) query = query.OrderBy(q => q.AccountCode.Code);
            else if (SortOrder == Html.GetSortDsc("AccountCodeId")) query = query.OrderByDescending(q => q.AccountCode.Code);
            else if (SortOrder == Html.GetSortAsc("IsBalanceSheetId")) query = query.OrderBy(q => q.IsBalanceSheet);
            else if (SortOrder == Html.GetSortDsc("IsBalanceSheetId")) query = query.OrderByDescending(q => q.IsBalanceSheet);
            else if (SortOrder == Html.GetSortAsc("DebitCreditIndicatorPositive")) query = query.OrderBy(q => q.DebitCreditIndicatorPositive);
            else if (SortOrder == Html.GetSortDsc("DebitCreditIndicatorPositive")) query = query.OrderByDescending(q => q.DebitCreditIndicatorPositive);
            else if (SortOrder == Html.GetSortAsc("DebitCreditIndicatorNegative")) query = query.OrderBy(q => q.DebitCreditIndicatorNegative);
            else if (SortOrder == Html.GetSortDsc("DebitCreditIndicatorNegative")) query = query.OrderByDescending(q => q.DebitCreditIndicatorNegative);
            else query = query.OrderBy(q => q.Id);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: AccountCodeMapping/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new AccountCodeMappingViewModel());
        }

        // POST: AccountCodeMapping/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(AccountCodeMappingViewModel model)
        {
            var accountCodeMappingBo = model.FormBo(AuthUserId, AuthUserId);

            if (ModelState.IsValid)
            {

                TrailObject trail = GetNewTrailObject();
                Result = AccountCodeMappingService.Result();
                var mappingResult = AccountCodeMappingService.ValidateMapping(accountCodeMappingBo);
                if (!mappingResult.Valid)
                {
                    Result.AddErrorRange(mappingResult.ToErrorArray());
                }
                else
                {
                    Result = AccountCodeMappingService.Create(ref accountCodeMappingBo, ref trail);
                    if (Result.Valid)
                    {
                        AccountCodeMappingService.ProcessMappingDetail(accountCodeMappingBo, AuthUserId); // DO NOT TRAIL
                        CreateTrail(
                            accountCodeMappingBo.Id,
                            "Create Account Code Mapping"
                        );

                        model.Id = accountCodeMappingBo.Id;

                        SetCreateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = accountCodeMappingBo.Id });
                    }
                }
                AddResult(Result);
            }
            LoadPage(accountCodeMappingBo);
            return View(model);
        }

        // GET: AccountCodeMapping/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            AccountCodeMappingBo accountCodeMappingBo = AccountCodeMappingService.Find(id);

            if (accountCodeMappingBo == null || (accountCodeMappingBo.Type != AccountCodeMappingBo.TypeClaimProvision && accountCodeMappingBo.Type != AccountCodeMappingBo.TypeClaimRecovery && accountCodeMappingBo.Type != AccountCodeMappingBo.TypeCedantAccountCode))
            {
                return RedirectToAction("Index");
            }
            LoadPage(accountCodeMappingBo);
            return View(new AccountCodeMappingViewModel(accountCodeMappingBo));
        }

        // POST: AccountCodeMapping/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, AccountCodeMappingViewModel model)
        {
            AccountCodeMappingBo accountCodeMappingBo = AccountCodeMappingService.Find(id);
            if (accountCodeMappingBo == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(accountCodeMappingBo.CreatedById, AuthUserId);
                bo.Id = accountCodeMappingBo.Id;

                TrailObject trail = GetNewTrailObject();
                Result = AccountCodeMappingService.Result();
                var mappingResult = AccountCodeMappingService.ValidateMapping(bo);
                if (!mappingResult.Valid)
                {
                    Result.AddErrorRange(mappingResult.ToErrorArray());
                }
                else
                {
                    Result = AccountCodeMappingService.Update(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        AccountCodeMappingService.ProcessMappingDetail(bo, AuthUserId); // DO NOT TRAIL
                        CreateTrail(
                            bo.Id,
                            "Update Account Code Mapping"
                        );

                        SetUpdateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = bo.Id });
                    }
                }
                AddResult(Result);
            }
            LoadPage(accountCodeMappingBo);
            return View(model);
        }

        // GET: AccountCodeMapping/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            AccountCodeMappingBo accountCodeMappingBo = AccountCodeMappingService.Find(id);
            if (accountCodeMappingBo == null || (accountCodeMappingBo.Type != AccountCodeMappingBo.TypeClaimProvision && accountCodeMappingBo.Type != AccountCodeMappingBo.TypeClaimRecovery))
            {
                return RedirectToAction("Index");
            }
            return View(new AccountCodeMappingViewModel(accountCodeMappingBo));
        }

        // POST: AccountCodeMapping/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id)
        {
            AccountCodeMappingBo accountCodeMappingBo = AccountCodeMappingService.Find(id);
            if (accountCodeMappingBo == null)
            {
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = AccountCodeMappingService.Delete(accountCodeMappingBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    accountCodeMappingBo.Id,
                    "Delete Account Code Mapping"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = accountCodeMappingBo.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = AccessMatrixBo.PowerUpload, ReturnController = Controller)]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var process = new ProcessAccountCodeMapping()
                    {
                        PostedFile = upload,
                        AuthUserId = AuthUserId,
                        IsRetro = false,
                    };
                    process.Process();

                    if (process.Errors.Count() > 0 && process.Errors != null)
                    {
                        SetErrorSessionMsgArr(process.Errors.Take(50).ToList());
                    }

                    int create = process.GetProcessCount("Create");
                    int update = process.GetProcessCount("Update");
                    int delete = process.GetProcessCount("Delete");

                    if (create != 0 || update != 0 || delete != 0)
                    {
                        SetSuccessSessionMsg(string.Format("Process File Successful, Total Create: {0}, Total Update: {1}, Total Delete: {2}", create, update, delete));
                    }
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Download(
            string downloadToken,
            int downloadType,
            int? Type,
            string TreatyType,
            int? TreatyCodeId,
            string ClaimCode,
            string BusinessOrigin,
            int? TransTypeCodeId,
            int? ModifiedContractCodeId,
            string InvoiceField,
            int? ReportingType,
            int? AccountCodeId,
            bool? IsBalanceSheet,
            int? DebitCreditIndicatorPositive,
            int? DebitCreditIndicatorNegative
        )
        {
            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            _db.Database.CommandTimeout = 0;
            var query = _db.AccountCodeMappings.Select(AccountCodeMappingViewModel.Expression())
                .Where(q => q.Type == AccountCodeMappingBo.TypeClaimProvision || q.Type == AccountCodeMappingBo.TypeClaimRecovery || q.Type == AccountCodeMappingBo.TypeCedantAccountCode);

            if (downloadType == 2)
            {
                if (Type != null) query = query.Where(q => q.Type == Type);
                if (!string.IsNullOrEmpty(TreatyType)) query = query.Where(q => q.AccountCodeMappingDetails.Any(d => d.TreatyType == TreatyType));
                if (!string.IsNullOrEmpty(ClaimCode)) query = query.Where(q => q.AccountCodeMappingDetails.Any(d => d.ClaimCode == ClaimCode));
                if (!string.IsNullOrEmpty(BusinessOrigin)) query = query.Where(q => q.AccountCodeMappingDetails.Any(d => d.BusinessOrigin == BusinessOrigin));
                if (TreatyCodeId != null) query = query.Where(q => q.TreatyCodeId == TreatyCodeId);
                if (TransTypeCodeId != null) query = query.Where(q => q.TransactionTypeCodePickListDetailId == TransTypeCodeId);
                if (!string.IsNullOrEmpty(InvoiceField)) query = query.Where(q => q.AccountCodeMappingDetails.Any(d => d.InvoiceField == InvoiceField));
                if (ModifiedContractCodeId != null) query = query.Where(q => q.ModifiedContractCodeId == ModifiedContractCodeId);
                if (ReportingType != null) query = query.Where(q => q.AccountCode.ReportingType == ReportingType);
                if (AccountCodeId != null) query = query.Where(q => q.AccountCodeId == AccountCodeId);
                if (IsBalanceSheet != null) query = query.Where(q => q.IsBalanceSheet == IsBalanceSheet);
                if (DebitCreditIndicatorPositive != null) query = query.Where(q => q.DebitCreditIndicatorPositive == DebitCreditIndicatorPositive);
                if (DebitCreditIndicatorNegative != null) query = query.Where(q => q.DebitCreditIndicatorNegative == DebitCreditIndicatorNegative);
            }
            if (downloadType == 3)
            {
                query = null;
            }

            var export = new ExportAccountCodeMapping();
            export.HandleTempDirectory();

            if (query != null)
                export.SetQuery(query.Select(x => new AccountCodeMappingBo
                {
                    Id = x.Id,
                    ReportType = x.ReportType,
                    Type = x.Type,
                    TreatyType = x.TreatyType,
                    TreatyCode = x.TreatyCode.Code,
                    ClaimCode = x.ClaimCode,
                    BusinessOrigin = x.BusinessOrigin,
                    TransactionTypeCodePickListDetailId = x.TransactionTypeCodePickListDetailId,
                    TransactionTypeCode = x.TransactionTypeCodePickListDetail.Code,
                    InvoiceField = x.InvoiceField,
                    RetroRegisterFieldPickListDetailId = x.RetroRegisterFieldPickListDetailId,
                    RetroRegisterField = x.RetroRegisterFieldPickListDetail.Code,
                    ModifiedContractCodeId = x.ModifiedContractCodeId,
                    ModifiedContractCode = !string.IsNullOrEmpty(x.ModifiedContractCode.CedingCompany.Code) && !string.IsNullOrEmpty(x.ModifiedContractCode.ModifiedContractCode) ? x.ModifiedContractCode.CedingCompany.Code + " - " + x.ModifiedContractCode.ModifiedContractCode : null,
                    AccountCodeId = x.AccountCodeId,
                    AccountCode = x.AccountCode.Code,
                    IsBalanceSheet = x.IsBalanceSheet,
                    IsBalanceSheetStr = x.IsBalanceSheet ? "BS" : "PL",
                    DebitCreditIndicatorPositive = x.DebitCreditIndicatorPositive,
                    DebitCreditIndicatorNegative = x.DebitCreditIndicatorNegative,
                }));

            export.Process();

            return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));
        }

        public void IndexPage()
        {
            SetViewBagMessage();
            DropDownType();
            DropDownAccountCode(AccountCodeBo.TypeDaa, AccountCodeBo.ReportingTypeIFRS4);
            DropDownTreatyCode(foreign: false);
            DropDownTransactionTypeCode();
            DropDownReportingType();
            DropDownMfrs17ContractCode();
            DropDownDebitCreditIndicator();
            DropDownPLBS();
        }

        public void LoadPage(AccountCodeMappingBo accountCodeMappingBo = null)
        {
            DropDownIfrs4Type();
            DropDownIfrs17Type();
            DropDownMfrs17ContractCode();
            DropDownEmpty();
            ViewBag.DropDownIfrs4AccountCodes = DropDownAccountCode(AccountCodeBo.TypeDaa, AccountCodeBo.ReportingTypeIFRS4);
            ViewBag.DropDownIfrs17AccountCodes = DropDownAccountCode(AccountCodeBo.TypeDaa, AccountCodeBo.ReportingTypeIFRS17);
            ViewBag.DropDownIfrsType = GetEmptyDropDownList();
            ViewBag.DropDownAccountCode = GetEmptyDropDownList();
            DropDownTransactionTypeCode();
            GetTreatyTypes();
            GetClaimCodes();
            GetBusinessOrigins();
            DropDownReportType();
            GetInvoiceFields();
            DropDownDebitCreditIndicator();
            DropDownPLBS();

            ViewBag.ShowModifiedContractCode = false;

            if (accountCodeMappingBo == null)
            {
                DropDownTreatyCode(TreatyCodeBo.StatusActive, foreign: false);
            }
            else
            {
                DropDownTreatyCode(TreatyCodeBo.StatusActive, accountCodeMappingBo.TreatyCodeId, foreign: false);
                if (accountCodeMappingBo.TreatyCodeBo != null && accountCodeMappingBo.TreatyCodeBo.Status == TreatyCodeBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.TreatyCodeStatusInactive);
                }

                if (!string.IsNullOrEmpty(accountCodeMappingBo.ClaimCode))
                {
                    string[] claimCodes = accountCodeMappingBo.ClaimCode.ToArraySplitTrim();
                    foreach (string claimCodeStr in claimCodes)
                    {
                        var claimCode = ClaimCodeService.FindByCode(claimCodeStr);
                        if (claimCode != null)
                        {
                            if (claimCode.Status == ClaimCodeBo.StatusInactive)
                            {
                                AddErrorMsg(string.Format(MessageBag.ClaimCodeStatusInactiveWithCode, claimCodeStr));
                            }
                        }
                        else
                        {
                            AddErrorMsg(string.Format(MessageBag.ClaimCodeNotFound, claimCodeStr));
                        }
                    }
                }

                if (accountCodeMappingBo.ReportType == AccountCodeMappingBo.ReportTypeIfrs4)
                {
                    ViewBag.DropDownIfrsType = DropDownIfrs4Type();
                    ViewBag.DropDownAccountCode = DropDownAccountCode(AccountCodeBo.TypeDaa, AccountCodeBo.ReportingTypeIFRS4);
                }
                else if (accountCodeMappingBo.ReportType == AccountCodeMappingBo.ReportTypeIfrs17)
                {
                    ViewBag.DropDownIfrsType = DropDownIfrs17Type();
                    ViewBag.DropDownAccountCode = DropDownAccountCode(AccountCodeBo.TypeDaa, AccountCodeBo.ReportingTypeIFRS17);
                }
                else
                {
                    ViewBag.DropDownIfrsType = GetEmptyDropDownList();
                    ViewBag.DropDownAccountCode = GetEmptyDropDownList();
                }
            }

            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownType()
        {
            var items = GetEmptyDropDownList();
            items.Add(new SelectListItem { Text = AccountCodeMappingBo.GetTypeName(AccountCodeMappingBo.TypeClaimProvision), Value = AccountCodeMappingBo.TypeClaimProvision.ToString() });
            items.Add(new SelectListItem { Text = AccountCodeMappingBo.GetTypeName(AccountCodeMappingBo.TypeClaimRecovery), Value = AccountCodeMappingBo.TypeClaimRecovery.ToString() });
            items.Add(new SelectListItem { Text = AccountCodeMappingBo.GetTypeName(AccountCodeMappingBo.TypeCedantAccountCode), Value = AccountCodeMappingBo.TypeCedantAccountCode.ToString() });
            ViewBag.DropDownTypes = items;
            return items;

        }

        public List<SelectListItem> DropDownIfrs4Type()
        {
            var items = GetEmptyDropDownList();
            items.Add(new SelectListItem { Text = AccountCodeMappingBo.GetTypeName(AccountCodeMappingBo.TypeClaimProvision), Value = AccountCodeMappingBo.TypeClaimProvision.ToString() });
            items.Add(new SelectListItem { Text = AccountCodeMappingBo.GetTypeName(AccountCodeMappingBo.TypeClaimRecovery), Value = AccountCodeMappingBo.TypeClaimRecovery.ToString() });

            ViewBag.DropDownIfrs4Types = items;
            return items;
        }

        public List<SelectListItem> DropDownIfrs17Type()
        {
            var items = GetEmptyDropDownList();
            items.Add(new SelectListItem { Text = AccountCodeMappingBo.GetTypeName(AccountCodeMappingBo.TypeClaimProvision), Value = AccountCodeMappingBo.TypeClaimProvision.ToString() });
            items.Add(new SelectListItem { Text = AccountCodeMappingBo.GetTypeName(AccountCodeMappingBo.TypeClaimRecovery), Value = AccountCodeMappingBo.TypeClaimRecovery.ToString() });
            items.Add(new SelectListItem { Text = AccountCodeMappingBo.GetTypeName(AccountCodeMappingBo.TypeCedantAccountCode), Value = AccountCodeMappingBo.TypeCedantAccountCode.ToString() });

            ViewBag.DropDownIfrs17Types = items;
            return items;
        }

        public List<SelectListItem> DropDownReportType()
        {
            var items = GetEmptyDropDownList();
            items.Add(new SelectListItem { Text = AccountCodeMappingBo.GetReportTypeName(AccountCodeMappingBo.ReportTypeIfrs4), Value = AccountCodeMappingBo.ReportTypeIfrs4.ToString() });
            items.Add(new SelectListItem { Text = AccountCodeMappingBo.GetReportTypeName(AccountCodeMappingBo.ReportTypeIfrs17), Value = AccountCodeMappingBo.ReportTypeIfrs17.ToString() });

            ViewBag.DropDownReportTypes = items;
            return items;
        }

        public List<SelectListItem> DropDownDebitCreditIndicator()
        {
            var items = GetEmptyDropDownList();
            items.Add(new SelectListItem { Text = AccountCodeMappingBo.GetDebitCreditIndicatorName(AccountCodeMappingBo.DebitCreditIndicatorC), Value = AccountCodeMappingBo.DebitCreditIndicatorC.ToString() });
            items.Add(new SelectListItem { Text = AccountCodeMappingBo.GetDebitCreditIndicatorName(AccountCodeMappingBo.DebitCreditIndicatorD), Value = AccountCodeMappingBo.DebitCreditIndicatorD.ToString() });

            ViewBag.DropDownDebitCreditIndicator = items;
            return items;
        }

        public List<SelectListItem> DropDownPLBS()
        {
            var items = GetEmptyDropDownList();
            items.Add(new SelectListItem { Text = "P&L", Value = false.ToString() });
            items.Add(new SelectListItem { Text = "BS", Value = true.ToString() });

            ViewBag.DropDownPLBS = items;
            return items;
        }

        public List<SelectListItem> DropDownReportingType()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= AccountCodeBo.ReportingTypeMax; i++)
            {
                items.Add(new SelectListItem { Text = AccountCodeBo.GetReportingTypeName(i), Value = i.ToString() });
            }
            ViewBag.ReportingTypes = items;
            return items;
        }
    }
}