using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Services;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ConsoleApp.Commands
{
    public class GenerateE3E4 : Command
    {
        public List<Column> Cols { get; set; }

        public Excel E3Ifrs17WM { get; set; }
        public Excel E3Ifrs17OM { get; set; }
        public Excel E3Ifrs17SF { get; set; }

        public Excel E3Ifrs4WM { get; set; }
        public Excel E3Ifrs4OM { get; set; }
        public Excel E3Ifrs4SF { get; set; }

        public Excel E4Ifrs17WM { get; set; }
        public Excel E4Ifrs17OM { get; set; }
        public Excel E4Ifrs17SF { get; set; }

        public Excel E4Ifrs4WM { get; set; }
        public Excel E4Ifrs4OM { get; set; }
        public Excel E4Ifrs4SF { get; set; }

        public ClaimRegisterBo ClaimRegisterBo { get; set; }

        public DateTime CurrentDate { get; set; } = DateTime.Today;

        public FinanceProvisioningBo FinanceProvisioningBo { get; set; }

        public List<int> FinanceClaimRegisterIds { get; set; }

        public List<int> RetroClaimRegisterIds { get; set; }

        public List<int> ClaimRegisterIds { get; set; }

        public IList<FinanceProvisioningTransactionBo> FinanceProvisioningTransactionBos { get; set; }

        public IList<DirectRetroProvisioningTransactionBo> DirectRetroProvisioningTransactionBos { get; set; }

        public List<FinanceProvisioningTransactionBo> TotalFinanceProvisioningTransactionBos { get; set; } = new List<FinanceProvisioningTransactionBo> { };

        public List<DirectRetroProvisioningTransactionBo> TotalDirectRetroProvisioningTransactionBos { get; set; } = new List<DirectRetroProvisioningTransactionBo> { };

        public int E3StartWriteRow { get; set; }

        public int E4StartWriteRow { get; set; }

        public string E3Path { get; set; }

        public string E4Path { get; set; }

        public string DateStr { get; set; }

        public double TotalE3Amount { get; set; } = 0;

        public double TotalE4Amount { get; set; } = 0;

        public bool IsReprocess { get; set; } = false;

        public bool IsForceUpdateStatus { get; set; } = false;

        public bool IsSuccess { get; set; } = true;

        public TreatyCodeBo TreatyCodeBo { get; set; }

        public const int TypeLayout = 1;
        public const int TypeJournalType = 2;
        public const int TypeAccountCode = 3;
        public const int TypeAccountDescription = 4;
        public const int TypePeriod = 5;
        public const int TypeTransactionDate = 6;
        public const int TypeAmount = 7;
        public const int TypeDebitCredit = 8;
        public const int TypeTransactionReference = 9;
        public const int TypeDescription = 10;
        public const int TypeL1 = 11;
        public const int TypeL2 = 12;
        public const int TypeL3 = 13;
        public const int TypeL4 = 14;
        public const int TypeL5 = 15;
        public const int TypeL6 = 16;
        public const int TypeL7 = 17;
        public const int TypeL8 = 18;
        public const int TypeL9 = 19;
        public const int TypeL10 = 20;
        public const int TypeAddDescription1 = 21;
        public const int TypeAddDescription2 = 22;
        public const int TypeAddDescription3 = 23;
        public const int TypeAddDescription4 = 24;
        public const int TypeAddDescription5 = 25;
        public const int TypeError = 26;

        public GenerateE3E4(FinanceProvisioningBo bo = null, bool isReprocess = false, bool isForceUpdateStatus = false)
        {
            Title = "GenerateE3E4";
            Description = "To generate E3E4 excel files";
            Options = new string[] {
                "--f|finance : To get Finance Provisioning by id",
                "--id= : Finance Provisioning Id",
            };
            E3StartWriteRow = Util.GetConfigInteger("E3StartWriteRow", 15);
            E4StartWriteRow = Util.GetConfigInteger("E4StartWriteRow", 15);
            FinanceProvisioningBo = bo;
            IsReprocess = isReprocess;
            IsForceUpdateStatus = isForceUpdateStatus;
            IsSuccess = true;
        }

        public override void Run()
        {
            PrintStarting();
            try
            {
                Process();
            }
            catch (Exception e)
            {
                PrintError(e.Message);
                PrintEnding();
                throw new Exception(e.Message);
            }
            PrintEnding();
        }

        public void Process()
        {
            try
            {
                if (FinanceProvisioningBo == null)
                    return;

                UpdateFinanceProvisioning(FinanceProvisioningBo.StatusProcessing);
                PrintMessage(string.Format(MessageBag.ProcessFinanceProvisioningProcessingWithId, FinanceProvisioningBo.Id));

                E3Path = Util.GetE3Path(FinanceProvisioningBo.Id.ToString());
                E4Path = Util.GetE4Path(FinanceProvisioningBo.Id.ToString());
                DateStr = FinanceProvisioningBo.Date.ToString("yyyyMMdd");

                // Delete previous file
                if (IsReprocess)
                {
                    if (Directory.Exists(E3Path))
                        Util.DeleteFiles(E3Path, "*");
                    if (Directory.Exists(E4Path))
                        Util.DeleteFiles(E4Path, "*");
                }

                FinanceClaimRegisterIds = FinanceProvisioningTransactionService.GetClaimRegisterIdByFinanceProvisioningId(FinanceProvisioningBo.Id);
                RetroClaimRegisterIds = DirectRetroProvisioningTransactionService.GetClaimRegisterIdByFinanceProvisioningId(FinanceProvisioningBo.Id);
                ClaimRegisterIds = FinanceClaimRegisterIds.Union(RetroClaimRegisterIds).ToList();

                FinanceProvisioningBo.ClaimsProvisionRecord = FinanceClaimRegisterIds.Count();
                FinanceProvisioningBo.DrProvisionRecord = RetroClaimRegisterIds.Count();

                if (ClaimRegisterIds == null || ClaimRegisterIds.Count == 0)
                    return;

                GetColumns();

                PrepareE3Excel();
                PrepareE4Excel();

                OpenE3Template();
                OpenE4Template();

                foreach (int id in ClaimRegisterIds)
                {
                    if (GetProcessCount("ProcessClaimRegister") > 0)
                        PrintProcessCount();
                    SetProcessCount("ProcessClaimRegister");

                    ClaimRegisterBo = ClaimRegisterService.Find(id);
                    FinanceProvisioningTransactionBos = FinanceProvisioningTransactionService.GetByClaimRegisterIdByFinanceProvisioningId(id, FinanceProvisioningBo.Id);
                    DirectRetroProvisioningTransactionBos = DirectRetroProvisioningTransactionService.GetByClaimRegisterIdByFinanceProvisioningId(id, FinanceProvisioningBo.Id);

                    ProcessE3Data();
                    ProcessE4Data();

                    if (!IsReprocess ||
                        (IsReprocess && IsForceUpdateStatus))
                    {
                        UpdateStatus();
                    }
                }

                if (GetProcessCount("ProcessClaimRegister") > 0)
                    PrintProcessCount();

                ProcessE3BalanceSheet(PickListDetailBo.BusinessOriginCodeWithinMalaysia);
                ProcessE3BalanceSheet(PickListDetailBo.BusinessOriginCodeOutsideMalaysia);
                ProcessE3BalanceSheet(PickListDetailBo.BusinessOriginCodeServiceFee);

                ProcessE4BalanceSheet(PickListDetailBo.BusinessOriginCodeWithinMalaysia);
                ProcessE4BalanceSheet(PickListDetailBo.BusinessOriginCodeOutsideMalaysia);
                ProcessE4BalanceSheet(PickListDetailBo.BusinessOriginCodeServiceFee);

                SaveE3();
                SaveE4();

                if (IsSuccess)
                {
                    UpdateFinanceProvisioning(FinanceProvisioningBo.StatusProvisioned);
                    PrintMessage(MessageBag.ProcessFinanceProvisioningSuccess);
                }
                else
                {
                    UpdateFinanceProvisioning(FinanceProvisioningBo.StatusFailed);
                    PrintMessage(MessageBag.ProcessFinanceProvisioningFailed);
                }
            }
            catch (Exception e)
            {
                PrintError(e.Message);
                UpdateFinanceProvisioning(FinanceProvisioningBo.StatusFailed);
                throw new Exception(e.Message);
            }
        }

        public void PrepareE3Excel()
        {
            var templateFilepath = Util.GetWebAppDocumentFilePath("E3_Template.xlsm");
            var filename = string.Format("E3_IFRS17_WM_{0}.xlsm", DateStr);
            var filepath = Path.Combine(E3Path, filename);
            E3Ifrs17WM = new Excel(templateFilepath, filepath, E3StartWriteRow);

            filename = string.Format("E3_IFRS17_OM_{0}.xlsm", DateStr);
            filepath = Path.Combine(E3Path, filename);
            E3Ifrs17OM = new Excel(templateFilepath, filepath, E3StartWriteRow);

            filename = string.Format("E3_IFRS17_SF_{0}.xlsm", DateStr);
            filepath = Path.Combine(E3Path, filename);
            E3Ifrs17SF = new Excel(templateFilepath, filepath, E3StartWriteRow);

            filename = string.Format("E3_IFRS4_WM_{0}.xlsm", DateStr);
            filepath = Path.Combine(E3Path, filename);
            E3Ifrs4WM = new Excel(templateFilepath, filepath, E3StartWriteRow);

            filename = string.Format("E3_IFRS4_OM_{0}.xlsm", DateStr);
            filepath = Path.Combine(E3Path, filename);
            E3Ifrs4OM = new Excel(templateFilepath, filepath, E3StartWriteRow);

            filename = string.Format("E3_IFRS4_SF_{0}.xlsm", DateStr);
            filepath = Path.Combine(E3Path, filename);
            E3Ifrs4SF = new Excel(templateFilepath, filepath, E3StartWriteRow);
        }

        public void PrepareE4Excel()
        {
            var templateFilepath = Util.GetWebAppDocumentFilePath("E4_Template.xlsm");
            var filename = string.Format("E4_IFRS17_WM_{0}.xlsm", DateStr);
            var filepath = Path.Combine(E4Path, filename);
            E4Ifrs17WM = new Excel(templateFilepath, filepath, E4StartWriteRow);

            filename = string.Format("E4_IFRS17_OM_{0}.xlsm", DateStr);
            filepath = Path.Combine(E4Path, filename);
            E4Ifrs17OM = new Excel(templateFilepath, filepath, E4StartWriteRow);

            filename = string.Format("E4_IFRS17_SF_{0}.xlsm", DateStr);
            filepath = Path.Combine(E4Path, filename);
            E4Ifrs17SF = new Excel(templateFilepath, filepath, E4StartWriteRow);

            filename = string.Format("E4_IFRS4_WM_{0}.xlsm", DateStr);
            filepath = Path.Combine(E4Path, filename);
            E4Ifrs4WM = new Excel(templateFilepath, filepath, E4StartWriteRow);

            filename = string.Format("E4_IFRS4_OM_{0}.xlsm", DateStr);
            filepath = Path.Combine(E4Path, filename);
            E4Ifrs4OM = new Excel(templateFilepath, filepath, E4StartWriteRow);

            filename = string.Format("E4_IFRS4_SF_{0}.xlsm", DateStr);
            filepath = Path.Combine(E4Path, filename);
            E4Ifrs4SF = new Excel(templateFilepath, filepath, E4StartWriteRow);
        }

        public void OpenE3Template()
        {
            E3Ifrs17WM.OpenTemplate();
            E3Ifrs17OM.OpenTemplate();
            E3Ifrs17SF.OpenTemplate();

            E3Ifrs4WM.OpenTemplate();
            E3Ifrs4OM.OpenTemplate();
            E3Ifrs4SF.OpenTemplate();
        }

        public void OpenE4Template()
        {
            E4Ifrs17WM.OpenTemplate();
            E4Ifrs17OM.OpenTemplate();
            E4Ifrs17SF.OpenTemplate();

            E4Ifrs4WM.OpenTemplate();
            E4Ifrs4OM.OpenTemplate();
            E4Ifrs4SF.OpenTemplate();
        }

        public void SaveE3()
        {
            PrintMessage("Saving E3");

            E3Ifrs17WM.SaveMacroEnabled(true, E3StartWriteRow);
            E3Ifrs17OM.SaveMacroEnabled(true, E3StartWriteRow);
            E3Ifrs17SF.SaveMacroEnabled(true, E3StartWriteRow);

            E3Ifrs4WM.SaveMacroEnabled(true, E3StartWriteRow);
            E3Ifrs4OM.SaveMacroEnabled(true, E3StartWriteRow);
            E3Ifrs4SF.SaveMacroEnabled(true, E3StartWriteRow);

            PrintMessage("Saved E3");
        }

        public void SaveE4()
        {
            PrintMessage("Saving E4");

            E4Ifrs17WM.SaveMacroEnabled(true, E4StartWriteRow);
            E4Ifrs17OM.SaveMacroEnabled(true, E4StartWriteRow);
            E4Ifrs17SF.SaveMacroEnabled(true, E4StartWriteRow);

            E4Ifrs4WM.SaveMacroEnabled(true, E4StartWriteRow);
            E4Ifrs4OM.SaveMacroEnabled(true, E4StartWriteRow);
            E4Ifrs4SF.SaveMacroEnabled(true, E4StartWriteRow);

            PrintMessage("Saved E4");
        }

        public void ProcessE3Data()
        {
            if (ClaimRegisterBo == null || (FinanceProvisioningTransactionBos == null || FinanceProvisioningTransactionBos.Count == 0))
                return;

            foreach (var transaction in FinanceProvisioningTransactionBos)
            {
                string layout;
                string journal = "GJ";

                string treatyCode = transaction.TreatyCode;

                if (TreatyCodeBo == null || TreatyCodeBo.Code != treatyCode)
                    TreatyCodeBo = TreatyCodeService.FindByCode(treatyCode);

                transaction.BusinessOrigin = TreatyCodeBo?.TreatyBo?.BusinessOriginPickListDetailBo?.Code;

                if (string.IsNullOrEmpty(transaction.BusinessOrigin))
                    IsSuccess = false;

                switch (transaction.BusinessOrigin)
                {
                    case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                        layout = E3Ifrs17OM.RowIndex == E3StartWriteRow ? "1;3;6" : "6";
                        break;
                    case PickListDetailBo.BusinessOriginCodeServiceFee:
                        layout = E3Ifrs17SF.RowIndex == E3StartWriteRow ? "1;3;6" : "6";
                        break;
                    default:
                        layout = E3Ifrs17WM.RowIndex == E3StartWriteRow ? "1;3;6" : "6";
                        break;
                }

                foreach (var col in Cols)
                {
                    if (!col.ColIndex.HasValue)
                        continue;

                    object v1 = null; // IFRS17
                    object v2 = null; // IFRS4

                    var modifiedContractCode = GetContractCode(ClaimRegisterBo.Mfrs17ContractCode);

                    var v1AccountCodeBo = AccountCodeMappingService.FindByE3E4Ifrs17Param(AccountCodeMappingBo.TypeClaimProvision, modifiedContractCode);
                    var v2AccountCodeBo = AccountCodeMappingService.FindByE3E4Ifrs4Param(AccountCodeMappingBo.TypeClaimProvision, transaction.TreatyType, transaction.ClaimCode);

                    if (v1AccountCodeBo == null ||
                        v1AccountCodeBo.AccountCodeBo == null ||
                        v2AccountCodeBo == null ||
                        v2AccountCodeBo.AccountCodeBo == null)
                    {
                        IsSuccess = false;
                    }

                    var v1AccountCode = v1AccountCodeBo?.AccountCodeBo != null ? v1AccountCodeBo?.AccountCodeBo.Code : "No Account Code mapped";
                    var v2AccountCode = v2AccountCodeBo?.AccountCodeBo != null ? v2AccountCodeBo?.AccountCodeBo.Code : "No Account Code mapped";

                    var v1DebitCreditIndicatorPositiveStr = v1AccountCodeBo != null && v1AccountCodeBo.DebitCreditIndicatorPositive.HasValue ? AccountCodeMappingBo.GetDebitCreditIndicatorName(v1AccountCodeBo.DebitCreditIndicatorPositive.Value) : "No indicator found";
                    var v1DebitCreditIndicatorNegativeStr = v1AccountCodeBo != null && v1AccountCodeBo.DebitCreditIndicatorNegative.HasValue ? AccountCodeMappingBo.GetDebitCreditIndicatorName(v1AccountCodeBo.DebitCreditIndicatorNegative.Value) : "No indicator found";

                    var v2DebitCreditIndicatorPositiveStr = v2AccountCodeBo != null && v2AccountCodeBo.DebitCreditIndicatorPositive.HasValue ? AccountCodeMappingBo.GetDebitCreditIndicatorName(v2AccountCodeBo.DebitCreditIndicatorPositive.Value) : "No indicator found";
                    var v2DebitCreditIndicatorNegativeStr = v2AccountCodeBo != null && v2AccountCodeBo.DebitCreditIndicatorNegative.HasValue ? AccountCodeMappingBo.GetDebitCreditIndicatorName(v2AccountCodeBo.DebitCreditIndicatorNegative.Value) : "No indicator found";

                    if (transaction.ClaimRecoveryAmount < 0 && string.IsNullOrEmpty(v1DebitCreditIndicatorNegativeStr))
                        IsSuccess = false;
                    if (transaction.ClaimRecoveryAmount > 0 && string.IsNullOrEmpty(v1DebitCreditIndicatorPositiveStr))
                        IsSuccess = false;

                    if (transaction.ClaimRecoveryAmount < 0 && string.IsNullOrEmpty(v2DebitCreditIndicatorNegativeStr))
                        IsSuccess = false;
                    if (transaction.ClaimRecoveryAmount > 0 && string.IsNullOrEmpty(v2DebitCreditIndicatorPositiveStr))
                        IsSuccess = false;

                    switch (col.ColIndex)
                    {
                        case TypeLayout:
                            v1 = layout;
                            v2 = layout;
                            break;
                        case TypeJournalType:
                            v1 = journal;
                            v2 = journal;
                            break;
                        case TypeAccountCode:
                            v1 = v1AccountCode;
                            v2 = v2AccountCode;
                            break;
                        case TypeAccountDescription:
                            v1 = v1AccountCodeBo?.AccountCodeBo?.Description;
                            v2 = v2AccountCodeBo?.AccountCodeBo?.Description;
                            break;
                        case TypePeriod:
                            v1 = SetAccountingPeriod(CurrentDate);
                            v2 = SetAccountingPeriod(CurrentDate);
                            break;
                        case TypeTransactionDate:
                            v1 = CurrentDate.ToString("dd/MM/yyyy");
                            v2 = CurrentDate.ToString("dd/MM/yyyy");
                            break;
                        case TypeAmount:
                            v1 = Util.RoundNullableValue(transaction.ClaimRecoveryAmount, 2);
                            v2 = Util.RoundNullableValue(transaction.ClaimRecoveryAmount, 2);
                            break;
                        case TypeDebitCredit:
                            v1 = transaction.ClaimRecoveryAmount < 0 ? v1DebitCreditIndicatorNegativeStr : v1DebitCreditIndicatorPositiveStr;
                            v2 = transaction.ClaimRecoveryAmount < 0 ? v2DebitCreditIndicatorNegativeStr : v2DebitCreditIndicatorPositiveStr;
                            break;
                        case TypeL3:
                            v1 = ClaimRegisterBo.Mfrs17AnnualCohort;
                            break;
                        case TypeL8:
                            string riskQuarter = ClaimRegisterBo.RiskQuarter?.ToLower();
                            if (!string.IsNullOrEmpty(riskQuarter) && riskQuarter.Contains('q'))
                            {
                                var riskQuarterArr = Util.ToArraySplitTrim(riskQuarter, 'q');
                                if (riskQuarterArr != null && riskQuarterArr.Count() == 2)
                                {
                                    var defaultYear = riskQuarterArr[0];
                                    var year = defaultYear.Length > 2 ? defaultYear.Substring(defaultYear.Length - 2) : defaultYear;
                                    riskQuarter = year + 'Q' + riskQuarterArr[1];
                                }
                            }

                            v1 = riskQuarter;
                            v2 = riskQuarter;
                            break;
                        case TypeL9:
                            v1 = transaction.ClaimCode;
                            break;
                        case TypeL10:
                            v1 = ClaimRegisterBo.Mfrs17ContractCode;
                            break;
                        case TypeError:
                            v1 = string.IsNullOrEmpty(transaction.BusinessOrigin) ? "Business Origin not found" : "";
                            v2 = string.IsNullOrEmpty(transaction.BusinessOrigin) ? "Business Origin not found" : "";
                            break;
                        default:
                            if (!string.IsNullOrEmpty(col.Property))
                            {
                                v1 = ClaimRegisterBo.GetPropertyValue(col.Property);
                                v2 = ClaimRegisterBo.GetPropertyValue(col.Property);
                            }
                            break;
                    }

                    switch (transaction.BusinessOrigin)
                    {
                        case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                            E3Ifrs17OM.WriteCell(E3Ifrs17OM.RowIndex, col.ColIndex.Value, v1);
                            E3Ifrs4OM.WriteCell(E3Ifrs4OM.RowIndex, col.ColIndex.Value, v2);
                            break;
                        case PickListDetailBo.BusinessOriginCodeServiceFee:
                            E3Ifrs17SF.WriteCell(E3Ifrs17SF.RowIndex, col.ColIndex.Value, v1);
                            E3Ifrs4SF.WriteCell(E3Ifrs4SF.RowIndex, col.ColIndex.Value, v2);
                            break;
                        default:
                            E3Ifrs17WM.WriteCell(E3Ifrs17WM.RowIndex, col.ColIndex.Value, v1);
                            E3Ifrs4WM.WriteCell(E3Ifrs4WM.RowIndex, col.ColIndex.Value, v2);
                            break;
                    }
                }

                switch (transaction.BusinessOrigin)
                {
                    case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                        E3Ifrs17OM.RowIndex++;
                        E3Ifrs4OM.RowIndex++;
                        break;
                    case PickListDetailBo.BusinessOriginCodeServiceFee:
                        E3Ifrs17SF.RowIndex++;
                        E3Ifrs4SF.RowIndex++;
                        break;
                    default:
                        E3Ifrs17WM.RowIndex++;
                        E3Ifrs4WM.RowIndex++;
                        break;
                }

                TotalFinanceProvisioningTransactionBos.Add(transaction);
            }
        }

        public void ProcessE3BalanceSheet(string businessOrigin)
        {
            if (businessOrigin == PickListDetailBo.BusinessOriginCodeWithinMalaysia && TotalFinanceProvisioningTransactionBos.Where(q => q.BusinessOrigin == businessOrigin || string.IsNullOrEmpty(q.BusinessOrigin)).Count() == 0)
                return;

            if (businessOrigin != PickListDetailBo.BusinessOriginCodeWithinMalaysia && TotalFinanceProvisioningTransactionBos.Where(q => q.BusinessOrigin == businessOrigin).Count() == 0)
                return;

            string journal = "GJ";
            string E3Ifrs4Layout;

            // IFRS4 Balance Sheet
            double totalClaimRecoveryAmount = 0;

            switch (businessOrigin)
            {
                case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                    E3Ifrs4Layout = E3Ifrs4OM.RowIndex == E3StartWriteRow ? "1;3;6" : "6";
                    break;
                case PickListDetailBo.BusinessOriginCodeServiceFee:
                    E3Ifrs4Layout = E3Ifrs4SF.RowIndex == E3StartWriteRow ? "1;3;6" : "6";
                    break;
                default:
                    E3Ifrs4Layout = E3Ifrs4WM.RowIndex == E3StartWriteRow ? "1;3;6" : "6";
                    break;
            }

            totalClaimRecoveryAmount = businessOrigin == PickListDetailBo.BusinessOriginCodeWithinMalaysia ? 
                TotalFinanceProvisioningTransactionBos.Where(q => q.BusinessOrigin == businessOrigin || string.IsNullOrEmpty(q.BusinessOrigin)).Sum(q => q.ClaimRecoveryAmount) :
                TotalFinanceProvisioningTransactionBos.Where(q => q.BusinessOrigin == businessOrigin).Sum(q => q.ClaimRecoveryAmount);

            foreach (var col in Cols)
            {
                if (!col.ColIndex.HasValue)
                    continue;

                object value = null;

                var accountCodeBo = AccountCodeMappingService.FindIfrs4BalanceSheet(AccountCodeMappingBo.TypeClaimProvision);

                if (accountCodeBo == null || accountCodeBo.AccountCodeBo == null)
                    IsSuccess = false;

                var accountCode = accountCodeBo?.AccountCodeBo != null ? accountCodeBo?.AccountCodeBo.Code : "No Account Code mapped";

                var debitCreditIndicatorPositiveStr = accountCodeBo != null && accountCodeBo.DebitCreditIndicatorPositive.HasValue ? AccountCodeMappingBo.GetDebitCreditIndicatorName(accountCodeBo.DebitCreditIndicatorPositive.Value) : "No indicator found";
                var debitCreditIndicatorNegativeStr = accountCodeBo != null && accountCodeBo.DebitCreditIndicatorNegative.HasValue ? AccountCodeMappingBo.GetDebitCreditIndicatorName(accountCodeBo.DebitCreditIndicatorNegative.Value) : "No indicator found";

                if (totalClaimRecoveryAmount < 0 && string.IsNullOrEmpty(debitCreditIndicatorNegativeStr))
                    IsSuccess = false;
                if (totalClaimRecoveryAmount > 0 && string.IsNullOrEmpty(debitCreditIndicatorPositiveStr))
                    IsSuccess = false;

                switch (col.ColIndex)
                {
                    case TypeLayout:
                        value = E3Ifrs4Layout;
                        break;
                    case TypeJournalType:
                        value = journal;
                        break;
                    case TypeAccountCode:
                        value = accountCode;
                        break;
                    case TypeAccountDescription:
                        value = accountCodeBo?.AccountCodeBo?.Description;
                        break;
                    case TypePeriod:
                        value = SetAccountingPeriod(CurrentDate);
                        break;
                    case TypeTransactionDate:
                        value = CurrentDate.ToString("dd/MM/yyyy");
                        break;
                    case TypeAmount:
                        value = Util.RoundNullableValue(totalClaimRecoveryAmount, 2);
                        break;
                    case TypeDebitCredit:
                        value = totalClaimRecoveryAmount < 0 ? debitCreditIndicatorNegativeStr : debitCreditIndicatorPositiveStr;
                        break;
                    case TypeTransactionReference:
                        value = string.Format("Claim reported {0}", CurrentDate.Year);
                        break;
                    case TypeDescription:
                        value = string.Format("Claim Provision {0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(CurrentDate.Month), CurrentDate.Year);
                        break;
                    default:
                        break;
                }

                switch (businessOrigin)
                {
                    case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                        E3Ifrs4OM.WriteCell(E3Ifrs4OM.RowIndex, col.ColIndex.Value, value);
                        break;
                    case PickListDetailBo.BusinessOriginCodeServiceFee:
                        E3Ifrs4SF.WriteCell(E3Ifrs4SF.RowIndex, col.ColIndex.Value, value);
                        break;
                    default:
                        E3Ifrs4WM.WriteCell(E3Ifrs4WM.RowIndex, col.ColIndex.Value, value);
                        break;
                }
            }

            switch (businessOrigin)
            {
                case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                    E3Ifrs4OM.RowIndex++;
                    break;
                case PickListDetailBo.BusinessOriginCodeServiceFee:
                    E3Ifrs4SF.RowIndex++;
                    break;
                default:
                    E3Ifrs4WM.RowIndex++;
                    break;
            }

            List<FinanceProvisioningTransactionBo> financeProvisioningTransactionBos = new List<FinanceProvisioningTransactionBo>();
            // IFRS17 Balance Sheet
            financeProvisioningTransactionBos = TotalFinanceProvisioningTransactionBos
                    .Where(q => q.BusinessOrigin == businessOrigin)
                    .GroupBy(g => new { g.ClaimRegisterBo.RiskQuarter, g.ClaimRegisterBo.Mfrs17AnnualCohort, g.ClaimRegisterBo.Mfrs17ContractCode })
                    .Select(r => new FinanceProvisioningTransactionBo
                    {
                        RiskQuarter = r.Key.RiskQuarter,
                        Mfrs17AnnualCohort = r.Key.Mfrs17AnnualCohort,
                        Mfrs17ContractCode = r.Key.Mfrs17ContractCode,
                        ClaimRecoveryAmount = r.Sum(x => x.ClaimRecoveryAmount),
                    })
                    .OrderBy(q => q.Mfrs17ContractCode)
                    .ThenBy(q => q.Mfrs17AnnualCohort)
                    .ThenBy(q => q.RiskQuarter)
                    .ToList();

            foreach (var transaction in financeProvisioningTransactionBos)
            {
                string layout;

                switch (businessOrigin)
                {
                    case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                        layout = E3Ifrs17OM.RowIndex == E3StartWriteRow ? "1;3;6" : "6";
                        break;
                    case PickListDetailBo.BusinessOriginCodeServiceFee:
                        layout = E3Ifrs17SF.RowIndex == E3StartWriteRow ? "1;3;6" : "6";
                        break;
                    default:
                        layout = E3Ifrs17WM.RowIndex == E3StartWriteRow ? "1;3;6" : "6";
                        break;
                }

                var modifiedContractCode = GetContractCode(transaction.Mfrs17ContractCode);

                foreach (var col in Cols)
                {
                    if (!col.ColIndex.HasValue)
                        continue;

                    object value = null;

                    var accountCodeBo = AccountCodeMappingService.FindIfrs17BalanceSheet(AccountCodeMappingBo.TypeClaimProvision, modifiedContractCode);

                    if (accountCodeBo == null || accountCodeBo.AccountCodeBo == null)
                        IsSuccess = false;

                    var accountCode = accountCodeBo?.AccountCodeBo != null ? accountCodeBo?.AccountCodeBo.Code : "No Account Code mapped";

                    var debitCreditIndicatorPositiveStr = accountCodeBo != null && accountCodeBo.DebitCreditIndicatorPositive.HasValue ? AccountCodeMappingBo.GetDebitCreditIndicatorName(accountCodeBo.DebitCreditIndicatorPositive.Value) : "No indicator found";
                    var debitCreditIndicatorNegativeStr = accountCodeBo != null && accountCodeBo.DebitCreditIndicatorNegative.HasValue ? AccountCodeMappingBo.GetDebitCreditIndicatorName(accountCodeBo.DebitCreditIndicatorNegative.Value) : "No indicator found";

                    if (transaction.ClaimRecoveryAmount < 0 && string.IsNullOrEmpty(debitCreditIndicatorNegativeStr))
                        IsSuccess = false;
                    if (transaction.ClaimRecoveryAmount > 0 && string.IsNullOrEmpty(debitCreditIndicatorPositiveStr))
                        IsSuccess = false;

                    switch (col.ColIndex)
                    {
                        case TypeLayout:
                            value = layout;
                            break;
                        case TypeJournalType:
                            value = journal;
                            break;
                        case TypeAccountCode:
                            value = accountCode;
                            break;
                        case TypeAccountDescription:
                            value = accountCodeBo?.AccountCodeBo?.Description;
                            break;
                        case TypePeriod:
                            value = SetAccountingPeriod(CurrentDate);
                            break;
                        case TypeTransactionDate:
                            value = CurrentDate.ToString("dd/MM/yyyy");
                            break;
                        case TypeAmount:
                            value = Util.RoundNullableValue(transaction.ClaimRecoveryAmount, 2);
                            break;
                        case TypeDebitCredit:
                            value = transaction.ClaimRecoveryAmount < 0 ? debitCreditIndicatorNegativeStr : debitCreditIndicatorPositiveStr;
                            break;
                        case TypeTransactionReference:
                            value = string.Format("Claim reported {0}", CurrentDate.Year);
                            break;
                        case TypeDescription:
                            value = string.Format("Claim Provision {0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(CurrentDate.Month), CurrentDate.Year);
                            break;
                        case TypeL3:
                            value = transaction.Mfrs17AnnualCohort;
                            break;
                        case TypeL8:
                            string riskQuarter = transaction.RiskQuarter?.ToLower();
                            if (!string.IsNullOrEmpty(riskQuarter) && riskQuarter.Contains('q'))
                            {
                                var riskQuarterArr = Util.ToArraySplitTrim(riskQuarter, 'q');
                                if (riskQuarterArr != null && riskQuarterArr.Count() == 2)
                                {
                                    var defaultYear = riskQuarterArr[0];
                                    var year = defaultYear.Length > 2 ? defaultYear.Substring(defaultYear.Length - 2) : defaultYear;
                                    riskQuarter = year + 'Q' + riskQuarterArr[1];
                                }
                            }

                            value = riskQuarter;
                            break;
                        case TypeL10:
                            value = transaction.Mfrs17ContractCode;
                            break;
                        default:
                            break;
                    }

                    switch (businessOrigin)
                    {
                        case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                            E3Ifrs17OM.WriteCell(E3Ifrs17OM.RowIndex, col.ColIndex.Value, value);
                            break;
                        case PickListDetailBo.BusinessOriginCodeServiceFee:
                            E3Ifrs17SF.WriteCell(E3Ifrs17SF.RowIndex, col.ColIndex.Value, value);
                            break;
                        default:
                            E3Ifrs17WM.WriteCell(E3Ifrs17WM.RowIndex, col.ColIndex.Value, value);
                            break;
                    }
                }

                switch (businessOrigin)
                {
                    case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                        E3Ifrs17OM.RowIndex++;
                        break;
                    case PickListDetailBo.BusinessOriginCodeServiceFee:
                        E3Ifrs17SF.RowIndex++;
                        break;
                    default:
                        E3Ifrs17WM.RowIndex++;
                        break;
                }
            }
        }

        public void ProcessE4Data()
        {
            if (ClaimRegisterBo == null || (DirectRetroProvisioningTransactionBos == null || DirectRetroProvisioningTransactionBos.Count == 0))
                return;

            foreach (var transaction in DirectRetroProvisioningTransactionBos)
            {
                string layout;
                string journal = "GJ";

                string treatyCode = transaction.TreatyCode;

                if (TreatyCodeBo == null || TreatyCodeBo.Code != treatyCode)
                    TreatyCodeBo = TreatyCodeService.FindByCode(treatyCode);

                transaction.BusinessOrigin = TreatyCodeBo?.TreatyBo?.BusinessOriginPickListDetailBo?.Code;

                if (string.IsNullOrEmpty(transaction.BusinessOrigin))
                    IsSuccess = false;

                switch (transaction.BusinessOrigin)
                {
                    case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                        layout = E4Ifrs17OM.RowIndex == E4StartWriteRow ? "1;3;6" : "6";
                        break;
                    case PickListDetailBo.BusinessOriginCodeServiceFee:
                        layout = E4Ifrs17SF.RowIndex == E4StartWriteRow ? "1;3;6" : "6";
                        break;
                    default:
                        layout = E4Ifrs17WM.RowIndex == E4StartWriteRow ? "1;3;6" : "6";
                        break;
                }

                foreach (var col in Cols)
                {
                    if (!col.ColIndex.HasValue)
                        continue;

                    object v1 = null; // IFRS17
                    object v2 = null; // IFRS4

                    var modifiedContractCode = GetContractCode(ClaimRegisterBo.Mfrs17ContractCode);

                    var v1AccountCodeBo = AccountCodeMappingService.FindByE3E4Ifrs17Param(AccountCodeMappingBo.TypeClaimRecovery, modifiedContractCode);
                    var v2AccountCodeBo = AccountCodeMappingService.FindByE3E4Ifrs4Param(AccountCodeMappingBo.TypeClaimRecovery, transaction.TreatyType, transaction.ClaimCode);

                    if (v1AccountCodeBo == null ||
                        v1AccountCodeBo.AccountCodeBo == null ||
                        v2AccountCodeBo == null ||
                        v2AccountCodeBo.AccountCodeBo == null)
                    {
                        IsSuccess = false;
                    }

                    var v1AccountCode = v1AccountCodeBo?.AccountCodeBo != null ? v1AccountCodeBo?.AccountCodeBo.Code : "No Account Code mapped";
                    var v2AccountCode = v2AccountCodeBo?.AccountCodeBo != null ? v2AccountCodeBo?.AccountCodeBo.Code : "No Account Code mapped";

                    var v1DebitCreditIndicatorPositiveStr = v1AccountCodeBo != null && v1AccountCodeBo.DebitCreditIndicatorPositive.HasValue ? AccountCodeMappingBo.GetDebitCreditIndicatorName(v1AccountCodeBo.DebitCreditIndicatorPositive.Value) : "No indicator found";
                    var v1DebitCreditIndicatorNegativeStr = v1AccountCodeBo != null && v1AccountCodeBo.DebitCreditIndicatorNegative.HasValue ? AccountCodeMappingBo.GetDebitCreditIndicatorName(v1AccountCodeBo.DebitCreditIndicatorNegative.Value) : "No indicator found";

                    var v2DebitCreditIndicatorPositiveStr = v2AccountCodeBo != null && v2AccountCodeBo.DebitCreditIndicatorPositive.HasValue ? AccountCodeMappingBo.GetDebitCreditIndicatorName(v2AccountCodeBo.DebitCreditIndicatorPositive.Value) : "No indicator found";
                    var v2DebitCreditIndicatorNegativeStr = v2AccountCodeBo != null && v2AccountCodeBo.DebitCreditIndicatorNegative.HasValue ? AccountCodeMappingBo.GetDebitCreditIndicatorName(v2AccountCodeBo.DebitCreditIndicatorNegative.Value) : "No indicator found";

                    if (transaction.RetroRecovery < 0 && string.IsNullOrEmpty(v1DebitCreditIndicatorNegativeStr))
                        IsSuccess = false;
                    if (transaction.RetroRecovery > 0 && string.IsNullOrEmpty(v1DebitCreditIndicatorPositiveStr))
                        IsSuccess = false;

                    if (transaction.RetroRecovery < 0 && string.IsNullOrEmpty(v2DebitCreditIndicatorNegativeStr))
                        IsSuccess = false;
                    if (transaction.RetroRecovery > 0 && string.IsNullOrEmpty(v2DebitCreditIndicatorPositiveStr))
                        IsSuccess = false;

                    switch (col.ColIndex)
                    {
                        case TypeLayout:
                            v1 = layout;
                            v2 = layout;
                            break;
                        case TypeJournalType:
                            v1 = journal;
                            v2 = journal;
                            break;
                        case TypeAccountCode:
                            v1 = v1AccountCode;
                            v2 = v2AccountCode;
                            break;
                        case TypeAccountDescription:
                            v1 = v1AccountCodeBo?.AccountCodeBo?.Description;
                            v2 = v2AccountCodeBo?.AccountCodeBo?.Description;
                            break;
                        case TypePeriod:
                            v1 = SetAccountingPeriod(CurrentDate);
                            v2 = SetAccountingPeriod(CurrentDate);
                            break;
                        case TypeTransactionDate:
                            v1 = CurrentDate.ToString("dd/MM/yyyy");
                            v2 = CurrentDate.ToString("dd/MM/yyyy");
                            break;
                        case TypeAmount:
                            v1 = Util.RoundNullableValue(transaction.RetroRecovery, 2);
                            v2 = Util.RoundNullableValue(transaction.RetroRecovery, 2);
                            break;
                        case TypeDebitCredit:
                            v1 = transaction.RetroRecovery < 0 ? v1DebitCreditIndicatorNegativeStr : v1DebitCreditIndicatorPositiveStr;
                            v2 = transaction.RetroRecovery < 0 ? v2DebitCreditIndicatorNegativeStr : v2DebitCreditIndicatorPositiveStr;
                            break;
                        case TypeL2:
                            v1 = transaction.RetroParty;
                            v2 = transaction.RetroParty;
                            break;
                        case TypeL3:
                            v1 = ClaimRegisterBo.Mfrs17AnnualCohort;
                            break;
                        case TypeL8:
                            string riskQuarter = ClaimRegisterBo.RiskQuarter?.ToLower();
                            if (!string.IsNullOrEmpty(riskQuarter) && riskQuarter.Contains('q'))
                            {
                                var riskQuarterArr = Util.ToArraySplitTrim(riskQuarter, 'q');
                                if (riskQuarterArr != null && riskQuarterArr.Count() == 2)
                                {
                                    var defaultYear = riskQuarterArr[0];
                                    var year = defaultYear.Length > 2 ? defaultYear.Substring(defaultYear.Length - 2) : defaultYear;
                                    riskQuarter = year + 'Q' + riskQuarterArr[1];
                                }
                            }

                            v1 = riskQuarter;
                            v2 = riskQuarter;
                            break;
                        case TypeL9:
                            v1 = transaction.ClaimCode;
                            break;
                        case TypeL10:
                            v1 = ClaimRegisterBo.Mfrs17ContractCode;
                            break;
                        case TypeError:
                            v1 = string.IsNullOrEmpty(transaction.BusinessOrigin) ? "Business Origin not found" : "";
                            v2 = string.IsNullOrEmpty(transaction.BusinessOrigin) ? "Business Origin not found" : "";
                            break;
                        default:
                            if (!string.IsNullOrEmpty(col.Property))
                            {
                                v1 = ClaimRegisterBo.GetPropertyValue(col.Property);
                                v2 = ClaimRegisterBo.GetPropertyValue(col.Property);
                            }
                            break;
                    }

                    switch (transaction.BusinessOrigin)
                    {
                        case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                            E4Ifrs17OM.WriteCell(E4Ifrs17OM.RowIndex, col.ColIndex.Value, v1);
                            E4Ifrs4OM.WriteCell(E4Ifrs4OM.RowIndex, col.ColIndex.Value, v2);
                            break;
                        case PickListDetailBo.BusinessOriginCodeServiceFee:
                            E4Ifrs17SF.WriteCell(E4Ifrs17SF.RowIndex, col.ColIndex.Value, v1);
                            E4Ifrs4SF.WriteCell(E4Ifrs4SF.RowIndex, col.ColIndex.Value, v2);
                            break;
                        default:
                            E4Ifrs17WM.WriteCell(E4Ifrs17WM.RowIndex, col.ColIndex.Value, v1);
                            E4Ifrs4WM.WriteCell(E4Ifrs4WM.RowIndex, col.ColIndex.Value, v2);
                            break;
                    }
                }

                switch (transaction.BusinessOrigin)
                {
                    case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                        E4Ifrs17OM.RowIndex++;
                        E4Ifrs4OM.RowIndex++;
                        break;
                    case PickListDetailBo.BusinessOriginCodeServiceFee:
                        E4Ifrs17SF.RowIndex++;
                        E4Ifrs4SF.RowIndex++;
                        break;
                    default:
                        E4Ifrs17WM.RowIndex++;
                        E4Ifrs4WM.RowIndex++;
                        break;
                }

                TotalDirectRetroProvisioningTransactionBos.Add(transaction);
            }
        }

        public void ProcessE4BalanceSheet(string businessOrigin)
        {
            if (businessOrigin == PickListDetailBo.BusinessOriginCodeWithinMalaysia && TotalDirectRetroProvisioningTransactionBos.Where(q => q.BusinessOrigin == businessOrigin || string.IsNullOrEmpty(q.BusinessOrigin)).Count() == 0)
                return;

            if (businessOrigin != PickListDetailBo.BusinessOriginCodeWithinMalaysia && TotalDirectRetroProvisioningTransactionBos.Where(q => q.BusinessOrigin == businessOrigin).Count() == 0)
                return;

            string journal = "GJ";
            string E4Ifrs4Layout;

            // IFRS4 Balance Sheet
            double totalRetroRecovery = 0;

            switch (businessOrigin)
            {
                case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                    E4Ifrs4Layout = E4Ifrs4OM.RowIndex == E4StartWriteRow ? "1;3;6" : "6";
                    break;
                case PickListDetailBo.BusinessOriginCodeServiceFee:
                    E4Ifrs4Layout = E4Ifrs4SF.RowIndex == E4StartWriteRow ? "1;3;6" : "6";
                    break;
                default:
                    E4Ifrs4Layout = E4Ifrs4WM.RowIndex == E4StartWriteRow ? "1;3;6" : "6";
                    break;
            }

            totalRetroRecovery = businessOrigin == PickListDetailBo.BusinessOriginCodeWithinMalaysia ? 
                TotalDirectRetroProvisioningTransactionBos.Where(q => q.BusinessOrigin == businessOrigin || string.IsNullOrEmpty(q.BusinessOrigin)).Sum(q => q.RetroRecovery) :
                TotalDirectRetroProvisioningTransactionBos.Where(q => q.BusinessOrigin == businessOrigin).Sum(q => q.RetroRecovery);

            foreach (var col in Cols)
            {
                if (!col.ColIndex.HasValue)
                    continue;

                object value = null;

                var accountCodeBo = AccountCodeMappingService.FindIfrs4BalanceSheet(AccountCodeMappingBo.TypeClaimRecovery);

                if (accountCodeBo == null ||
                    accountCodeBo.AccountCodeBo == null)
                    IsSuccess = false;

                var accountCode = accountCodeBo?.AccountCodeBo != null ? accountCodeBo?.AccountCodeBo.Code : "No Account Code mapped";

                var debitCreditIndicatorPositiveStr = accountCodeBo != null && accountCodeBo.DebitCreditIndicatorPositive.HasValue ? AccountCodeMappingBo.GetDebitCreditIndicatorName(accountCodeBo.DebitCreditIndicatorPositive.Value) : "No indicator found";
                var debitCreditIndicatorNegativeStr = accountCodeBo != null && accountCodeBo.DebitCreditIndicatorNegative.HasValue ? AccountCodeMappingBo.GetDebitCreditIndicatorName(accountCodeBo.DebitCreditIndicatorNegative.Value) : "No indicator found";

                if (totalRetroRecovery < 0 && string.IsNullOrEmpty(debitCreditIndicatorNegativeStr))
                    IsSuccess = false;
                if (totalRetroRecovery > 0 && string.IsNullOrEmpty(debitCreditIndicatorPositiveStr))
                    IsSuccess = false;

                switch (col.ColIndex)
                {
                    case TypeLayout:
                        value = E4Ifrs4Layout;
                        break;
                    case TypeJournalType:
                        value = journal;
                        break;
                    case TypeAccountCode:
                        value = accountCode;
                        break;
                    case TypeAccountDescription:
                        value = accountCodeBo?.AccountCodeBo?.Description;
                        break;
                    case TypePeriod:
                        value = SetAccountingPeriod(CurrentDate);
                        break;
                    case TypeTransactionDate:
                        value = CurrentDate.ToString("dd/MM/yyyy");
                        break;
                    case TypeAmount:
                        value = Util.RoundNullableValue(totalRetroRecovery, 2);
                        break;
                    case TypeDebitCredit:
                        value = totalRetroRecovery < 0 ? debitCreditIndicatorNegativeStr : debitCreditIndicatorPositiveStr;
                        break;
                    case TypeTransactionReference:
                        value = string.Format("Claim recoverable {0}", CurrentDate.Year);
                        break;
                    case TypeDescription:
                        value = string.Format("Provision claims recovery for month of {0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(CurrentDate.Month), CurrentDate.Year);
                        break;
                    default:
                        break;
                }

                switch (businessOrigin)
                {
                    case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                        E4Ifrs4OM.WriteCell(E4Ifrs4OM.RowIndex, col.ColIndex.Value, value);
                        break;
                    case PickListDetailBo.BusinessOriginCodeServiceFee:
                        E4Ifrs4SF.WriteCell(E4Ifrs4SF.RowIndex, col.ColIndex.Value, value);
                        break;
                    default:
                        E4Ifrs4WM.WriteCell(E4Ifrs4WM.RowIndex, col.ColIndex.Value, value);
                        break;
                }
            }

            switch (businessOrigin)
            {
                case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                    E4Ifrs4OM.RowIndex++;
                    break;
                case PickListDetailBo.BusinessOriginCodeServiceFee:
                    E4Ifrs4SF.RowIndex++;
                    break;
                default:
                    E4Ifrs4WM.RowIndex++;
                    break;
            }

            List<DirectRetroProvisioningTransactionBo> directRetroProvisioningTransactionBos = new List<DirectRetroProvisioningTransactionBo>();
            // IFRS17 Balance Sheet
            directRetroProvisioningTransactionBos = TotalDirectRetroProvisioningTransactionBos
                    .Where(q => q.BusinessOrigin == businessOrigin)
                    .GroupBy(g => new { g.RetroParty, g.ClaimRegisterBo.RiskQuarter, g.ClaimRegisterBo.Mfrs17AnnualCohort, g.ClaimRegisterBo.Mfrs17ContractCode })
                    .Select(r => new DirectRetroProvisioningTransactionBo
                    {
                        RetroParty = r.Key.RetroParty,
                        RiskQuarter = r.Key.RiskQuarter,
                        Mfrs17AnnualCohort = r.Key.Mfrs17AnnualCohort,
                        Mfrs17ContractCode = r.Key.Mfrs17ContractCode,
                        RetroRecovery = r.Sum(x => x.RetroRecovery),
                    })
                    .OrderBy(q => q.RetroParty)
                    .ThenBy(q => q.Mfrs17ContractCode)
                    .ThenBy(q => q.Mfrs17AnnualCohort)
                    .ThenBy(q => q.RiskQuarter)
                    .ToList();

            foreach (var transaction in directRetroProvisioningTransactionBos)
            {
                string layout;

                switch (businessOrigin)
                {
                    case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                        layout = E4Ifrs17OM.RowIndex == E4StartWriteRow ? "1;3;6" : "6";
                        break;
                    case PickListDetailBo.BusinessOriginCodeServiceFee:
                        layout = E4Ifrs17SF.RowIndex == E4StartWriteRow ? "1;3;6" : "6";
                        break;
                    default:
                        layout = E4Ifrs17WM.RowIndex == E4StartWriteRow ? "1;3;6" : "6";
                        break;
                }

                foreach (var col in Cols)
                {
                    if (!col.ColIndex.HasValue)
                        continue;

                    object value = null;

                    var modifiedContractCode = GetContractCode(transaction.Mfrs17ContractCode);

                    var accountCodeBo = AccountCodeMappingService.FindIfrs17BalanceSheet(AccountCodeMappingBo.TypeClaimRecovery, modifiedContractCode);

                    if (accountCodeBo == null ||
                        accountCodeBo.AccountCodeBo == null)
                        IsSuccess = false;

                    var accountCode = accountCodeBo?.AccountCodeBo != null ? accountCodeBo?.AccountCodeBo.Code : "No Account Code mapped";

                    var debitCreditIndicatorPositiveStr = accountCodeBo != null && accountCodeBo.DebitCreditIndicatorPositive.HasValue ? AccountCodeMappingBo.GetDebitCreditIndicatorName(accountCodeBo.DebitCreditIndicatorPositive.Value) : "No indicator found";
                    var debitCreditIndicatorNegativeStr = accountCodeBo != null && accountCodeBo.DebitCreditIndicatorNegative.HasValue ? AccountCodeMappingBo.GetDebitCreditIndicatorName(accountCodeBo.DebitCreditIndicatorNegative.Value) : "No indicator found";

                    if (transaction.RetroRecovery < 0 && string.IsNullOrEmpty(debitCreditIndicatorNegativeStr))
                        IsSuccess = false;
                    if (transaction.RetroRecovery > 0 && string.IsNullOrEmpty(debitCreditIndicatorPositiveStr))
                        IsSuccess = false;

                    switch (col.ColIndex)
                    {
                        case TypeLayout:
                            value = layout;
                            break;
                        case TypeJournalType:
                            value = journal;
                            break;
                        case TypeAccountCode:
                            value = accountCode;
                            break;
                        case TypeAccountDescription:
                            value = accountCodeBo?.AccountCodeBo?.Description;
                            break;
                        case TypePeriod:
                            value = SetAccountingPeriod(CurrentDate);
                            break;
                        case TypeTransactionDate:
                            value = CurrentDate.ToString("dd/MM/yyyy");
                            break;
                        case TypeAmount:
                            value = Util.RoundNullableValue(transaction.RetroRecovery, 2);
                            break;
                        case TypeDebitCredit:
                            value = transaction.RetroRecovery < 0 ? debitCreditIndicatorNegativeStr : debitCreditIndicatorPositiveStr;
                            break;
                        case TypeTransactionReference:
                            value = string.Format("Claim recoverable {0}", CurrentDate.Year);
                            break;
                        case TypeDescription:
                            value = string.Format("Provision claims recovery for month of {0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(CurrentDate.Month), CurrentDate.Year);
                            break;
                        case TypeL2:
                            value = transaction.RetroParty;
                            break;
                        case TypeL3:
                            value = transaction.Mfrs17AnnualCohort;
                            break;
                        case TypeL8:
                            string riskQuarter = transaction.RiskQuarter?.ToLower();
                            if (!string.IsNullOrEmpty(riskQuarter) && riskQuarter.Contains('q'))
                            {
                                var riskQuarterArr = Util.ToArraySplitTrim(riskQuarter, 'q');
                                if (riskQuarterArr != null && riskQuarterArr.Count() == 2)
                                {
                                    var defaultYear = riskQuarterArr[0];
                                    var year = defaultYear.Length > 2 ? defaultYear.Substring(defaultYear.Length - 2) : defaultYear;
                                    riskQuarter = year + 'Q' + riskQuarterArr[1];
                                }
                            }

                            value = riskQuarter;
                            break;
                        case TypeL10:
                            value = transaction.Mfrs17ContractCode;
                            break;
                        default:
                            break;
                    }

                    switch (businessOrigin)
                    {
                        case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                            E4Ifrs17OM.WriteCell(E4Ifrs17OM.RowIndex, col.ColIndex.Value, value);
                            break;
                        case PickListDetailBo.BusinessOriginCodeServiceFee:
                            E4Ifrs17SF.WriteCell(E4Ifrs17SF.RowIndex, col.ColIndex.Value, value);
                            break;
                        default:
                            E4Ifrs17WM.WriteCell(E4Ifrs17WM.RowIndex, col.ColIndex.Value, value);
                            break;
                    }
                }

                switch (businessOrigin)
                {
                    case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                        E4Ifrs17OM.RowIndex++;
                        break;
                    case PickListDetailBo.BusinessOriginCodeServiceFee:
                        E4Ifrs17SF.RowIndex++;
                        break;
                    default:
                        E4Ifrs17WM.RowIndex++;
                        break;
                }
            }
        }

        public string SetAccountingPeriod(DateTime currentDate)
        {
            int Year = DateTime.Parse(currentDate.ToString()).Year;
            int Month = DateTime.Parse(currentDate.ToString()).Month;

            DateTime startDate = new DateTime((Month == 1 ? Year - 1 : Year), (Month == 1 ? 12 : Month - 1), 21);
            DateTime endDate = new DateTime(Year, Month, 20);

            if ((startDate <= currentDate) && (currentDate <= endDate))
            {
                return string.Format("{0}/{1}", Year, Month.ToString("000"));
            }
            return string.Format("{0}/{1}", Year, (Month + 1).ToString("000"));
        }

        public string GetContractCode(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                if (code.Length > 3)
                    return code.Substring(0, code.Length - 3);
                else
                    return code;
            }
            return "";
        }

        public List<Column> GetColumns()
        {
            Cols = new List<Column>
            {
                new Column {
                    ColIndex = TypeLayout,
                },
                new Column {
                    ColIndex = TypeJournalType,
                },
                new Column {
                    ColIndex = TypeAccountCode,
                },
                new Column {
                    ColIndex = TypeAccountDescription,
                },
                new Column {
                    ColIndex = TypePeriod,
                },
                new Column {
                    ColIndex = TypeTransactionDate,
                },
                new Column {
                    ColIndex = TypeAmount,
                },
                new Column {
                    ColIndex = TypeDebitCredit,
                },
                new Column {
                    ColIndex = TypeTransactionReference,
                    Property = "EntryNo",
                },
                new Column {
                    ColIndex = TypeDescription,
                    Property = "ClaimId",
                },
                new Column {
                    ColIndex = TypeL1,
                },
                new Column {
                    ColIndex = TypeL2,
                },
                new Column {
                    ColIndex = TypeL3,
                },
                new Column {
                    ColIndex = TypeL4,
                },
                new Column {
                    ColIndex = TypeL5,
                },
                new Column {
                    ColIndex = TypeL6,
                },
                new Column {
                    ColIndex = TypeL7,
                },
                new Column {
                    ColIndex = TypeL8,
                },
                new Column {
                    ColIndex = TypeL9,
                },
                new Column {
                    ColIndex = TypeL10,
                },
                new Column {
                    ColIndex = TypeAddDescription1,
                },
                new Column {
                    ColIndex = TypeAddDescription2,
                },
                new Column {
                    ColIndex = TypeAddDescription3,
                },
                new Column {
                    ColIndex = TypeAddDescription4,
                },
                new Column {
                    ColIndex = TypeAddDescription5,
                },
                new Column {
                    ColIndex = TypeError,
                },
            };

            return Cols;
        }

        public void UpdateStatus()
        {
            if (ClaimRegisterBo == null)
                return;

            TrailObject trail = new TrailObject();

            var bo = ClaimRegisterBo;
            bo.ProvisionStatus = ClaimRegisterBo.ProvisionStatusProvisioned;
            Result result = ClaimRegisterService.Update(ref bo, ref trail);
            UserTrailBo userTrailBo = new UserTrailBo(
                ClaimRegisterBo.Id,
                "Update Claim Register provision status",
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }

        public void UpdateFinanceProvisioning(int status)
        {
            if (FinanceProvisioningBo == null)
                return;

            TrailObject trail = new TrailObject();

            var bo = FinanceProvisioningBo;
            bo.Status = status;
            if (!bo.ProvisionAt.HasValue)
                bo.ProvisionAt = DateTime.Now;
            Result result = FinanceProvisioningService.Update(ref bo, ref trail);
            UserTrailBo userTrailBo = new UserTrailBo(
                FinanceProvisioningBo.Id,
                "Update Finance Provisioning",
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }
    }
}
