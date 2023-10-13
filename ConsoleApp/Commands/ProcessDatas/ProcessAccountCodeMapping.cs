using BusinessObject;
using BusinessObject.Identity;
using Services;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ConsoleApp.Commands.ProcessDatas
{
    public class ProcessAccountCodeMapping : Command
    {
        public bool IsRetro { get; set; } = false;

        public int AccountCodeType { get; set; }

        public List<int> AccountCodeMappingTypes { get; set; }

        public List<Column> Columns { get; set; }

        public string FilePath { get; set; }

        public HttpPostedFileBase PostedFile { get; set; }

        public int? AuthUserId { get; set; }

        public TextFile TextFile { get; set; }

        public List<string> Errors { get; set; }

        public char[] charsToTrim = { ',', '.', ' ' };

        public ProcessAccountCodeMapping()
        {
            Title = "ProcessAccountCodeMapping";
            Description = "To read Account Code Mapping csv file and insert into database";
            Arguments = new string[]
            {
                "filePath",
            };
            Hide = true;
            Errors = new List<string> { };
            GetColumns();
        }

        public override bool Validate()
        {
            string filepath = CommandInput.Arguments[0];
            if (!File.Exists(filepath))
            {
                PrintError(string.Format(MessageBag.FileNotExists, filepath));
                return false;
            }
            else
            {
                FilePath = filepath;
            }

            return base.Validate();
        }

        public override void Run()
        {
            PrintStarting();

            Process();

            PrintEnding();
        }

        public void Process()
        {
            if (PostedFile != null)
            {
                TextFile = new TextFile(PostedFile.InputStream);
            }
            else if (File.Exists(FilePath))
            {
                TextFile = new TextFile(FilePath);
            }
            else
            {
                throw new Exception("No file can be read");
            }

            if (IsRetro)
            {
                AccountCodeType = AccountCodeBo.TypeRetro;
                AccountCodeMappingTypes = AccountCodeMappingBo.GetRetroTypes();
            }
            else
            {
                AccountCodeType = AccountCodeBo.TypeDaa;
                AccountCodeMappingTypes = AccountCodeMappingBo.GetDaaTypes();
            }

            TrailObject trail;
            Result result;
            while (TextFile.GetNextRow() != null)
            {
                if (TextFile.RowIndex == 1)
                    continue; // Skip header row

                if (PrintCommitBuffer())
                {
                }
                SetProcessCount();

                bool error = false;

                AccountCodeMappingBo acm = null;
                try
                {
                    acm = SetData();

                    if (!string.IsNullOrEmpty(acm.ReportTypeName))
                    {
                        int reportType = AccountCodeMappingBo.GetReportTypeByName(acm.ReportTypeName.Trim());
                        if (reportType == AccountCodeMappingBo.ReportTypeIfrs4 || reportType == AccountCodeMappingBo.ReportTypeIfrs17)
                        {
                            acm.ReportType = reportType;
                        }
                        else
                        {
                            SetProcessCount("Report Type not exist");
                            Errors.Add(string.Format("Report Type: {0} not exist at row {1}", acm.ReportTypeName, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        SetProcessCount("Report Type empty");
                        Errors.Add(string.Format("Please enter the Report Type at row {0}", TextFile.RowIndex));
                        error = true;
                    }


                    if (!string.IsNullOrEmpty(acm.TypeName))
                    {
                        int type = AccountCodeMappingBo.GetTypeByName(acm.TypeName);
                        if (AccountCodeMappingTypes.Contains(type))
                        {
                            acm.Type = type;
                        }
                        else
                        {
                            SetProcessCount("Type not exist");
                            Errors.Add(string.Format("Type: {0} not exist at row {1}", acm.TypeName, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        acm.Type = null;
                        SetProcessCount("Type Empty");
                        Errors.Add(string.Format("Please enter the Type at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(acm.TreatyType))
                    {
                        string[] treatyTypes = acm.TreatyType.ToArraySplitTrim();
                        foreach (string treatyTypeStr in treatyTypes)
                        {
                            PickListDetailBo pickListDetailBo = PickListDetailService.FindByPickListIdCode(PickListBo.TreatyType, treatyTypeStr);
                            if (pickListDetailBo == null)
                            {
                                SetProcessCount("Treaty Type Not Found");
                                Errors.Add(string.Format("{0} : {1} at row {2}", "Treaty Type not exist", treatyTypeStr, TextFile.RowIndex));
                                error = true;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(acm.TreatyCode))
                    {
                        TreatyCodeBo treatyCodeBo = TreatyCodeService.FindByCode(acm.TreatyCode);
                        if (treatyCodeBo == null)
                        {
                            SetProcessCount("Treaty Code Not Found");
                            Errors.Add(string.Format("The Treaty Code doesn't exists: {0} at row {1}", acm.TreatyCode, TextFile.RowIndex));
                            error = true;
                        }
                        else if (treatyCodeBo.Status == TreatyCodeBo.StatusInactive)
                        {
                            SetProcessCount("Treaty Code Inactive");
                            Errors.Add(string.Format("{0}: {1} at row {2}", MessageBag.TreatyCodeStatusInactive, acm.TreatyCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            acm.TreatyCodeId = treatyCodeBo.Id;
                            acm.TreatyCodeBo = treatyCodeBo;
                        }
                    }

                    if (!string.IsNullOrEmpty(acm.ClaimCode))
                    {
                        string[] claimCodes = acm.ClaimCode.ToArraySplitTrim();
                        foreach (string claimCodeStr in claimCodes)
                        {
                            var claimCodeBo = ClaimCodeService.FindByCode(claimCodeStr);
                            if (claimCodeBo != null)
                            {
                                if (claimCodeBo.Status == ClaimCodeBo.StatusInactive)
                                {
                                    SetProcessCount("Claim Code Inactive");
                                    Errors.Add(string.Format("{0}: {1} at row {2}", MessageBag.ClaimCodeStatusInactive, claimCodeStr, TextFile.RowIndex));
                                    error = true;
                                }
                            }
                            else
                            {
                                SetProcessCount("Claim Code Not Found");
                                Errors.Add(string.Format("{0} at row {1}", string.Format(MessageBag.ClaimCodeNotFound, claimCodeStr), TextFile.RowIndex));
                                error = true;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(acm.BusinessOrigin))
                    {
                        string[] businessOrigins = acm.BusinessOrigin.ToArraySplitTrim();
                        foreach (string businessOriginStr in businessOrigins)
                        {
                            PickListDetailBo pickListDetailBo = PickListDetailService.FindByPickListIdCode(PickListBo.BusinessOrigin, businessOriginStr);
                            if (pickListDetailBo == null)
                            {
                                SetProcessCount("Business Origin Not Found");
                                Errors.Add(string.Format("{0} : {1} at row {2}", "Business Origin not exist", businessOriginStr, TextFile.RowIndex));
                                error = true;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(acm.InvoiceField))
                    {
                        string[] invoiceFields = acm.InvoiceField.ToArraySplitTrim();
                        foreach (string invoiceFieldStr in invoiceFields)
                        {
                            PickListDetailBo pickListDetailBo = PickListDetailService.FindByPickListIdCode(PickListBo.InvoiceField, invoiceFieldStr);
                            if (pickListDetailBo == null)
                            {
                                SetProcessCount("Invoice Field Not Found");
                                Errors.Add(string.Format("{0} : {1} at row {2}", "Invoice Field not exist", invoiceFieldStr, TextFile.RowIndex));
                                error = true;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(acm.TransactionTypeCode))
                    {
                        PickListDetailBo ttc = PickListDetailService.FindByPickListIdCode(PickListBo.TransactionTypeCode, acm.TransactionTypeCode);
                        if (ttc == null)
                        {
                            SetProcessCount("Transaction Type Code Not Found");
                            Errors.Add(string.Format("The Transaction Type Code doesn't exists: {0} at row {1}", acm.TransactionTypeCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            acm.TransactionTypeCodePickListDetailId = ttc.Id;
                            acm.TransactionTypeCodePickListDetailBo = ttc;
                        }
                    }

                    if (!string.IsNullOrEmpty(acm.ModifiedContractCode))
                    {
                        int pos = acm.ModifiedContractCode.IndexOf('-');
                        var cedingCo = acm.ModifiedContractCode.Substring(0, pos);
                        var code = acm.ModifiedContractCode.Substring(pos + 1);

                        var cedingCompany = CedantService.FindByCode(cedingCo);
                        if (cedingCompany != null)
                        {
                            var mcc = Mfrs17ContractCodeService.FindByCedingCompanyAndModifiedContractCode(cedingCompany.Id, code);
                            if (mcc != null)
                            {
                                acm.ModifiedContractCodeId = mcc.Id;
                                acm.ModifiedContractCodeBo = mcc;
                            }
                            else
                            {
                                SetProcessCount("Modified Contract Code Not Found");
                                Errors.Add(string.Format("The Modified Contract Code doesn't exists: {0} at row {1}", acm.ModifiedContractCode, TextFile.RowIndex));
                                error = true;
                            }
                        }
                        else
                        {
                            SetProcessCount("Ceding Company Not Found");
                            Errors.Add(string.Format("The Ceding Company doesn't exists and could not find the Modified Contract Code: {0} at row {1}", acm.ModifiedContractCode, TextFile.RowIndex));
                            error = true;
                        }
                    } 
                    else if (string.IsNullOrEmpty(acm.ModifiedContractCode) && acm.ReportTypeName == AccountCodeMappingBo.ReportTypeIfrs17Name && (acm.TypeName != AccountCodeMappingBo.TypeDirectRetroName && acm.TypeName != AccountCodeMappingBo.TypePerLifeRetroName))
                    {
                        SetProcessCount("Modified Contract Code is required if Report Type is IFRS17");
                        Errors.Add(string.Format("Modified Contract Code is required if Report Type is IFRS17 at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                        if (!string.IsNullOrEmpty(acm.IsBalanceSheetStr))
                    {
                        if (acm.IsBalanceSheetStr != "PL" && acm.IsBalanceSheetStr != "BS")
                        {
                            SetProcessCount("PL/BS has invalid input");
                            Errors.Add(string.Format("PL/BS has invalid input at row {0}", TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            if (acm.IsBalanceSheetStr == "BS")
                                acm.IsBalanceSheet = true;
                            else
                                acm.IsBalanceSheet = false;
                        }
                    }
                    else
                    {
                        SetProcessCount("PL/BS is Empty");
                        Errors.Add(string.Format("PL/BS is required, Y = BS, N = PL at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (IsRetro)
                    {
                        if (!string.IsNullOrEmpty(acm.RetroRegisterField))
                        {
                            PickListDetailBo rgf = PickListDetailService.FindByPickListIdCode(PickListBo.RetroRegisterField, acm.RetroRegisterField);
                            if (rgf == null)
                            {
                                SetProcessCount("Retro Register Field Not Found");
                                Errors.Add(string.Format("The Retro Register Field doesn't exists: {0} at row {1}", acm.RetroRegisterField, TextFile.RowIndex));
                                error = true;
                            }
                            else
                            {
                                acm.RetroRegisterFieldPickListDetailId = rgf.Id;
                                acm.RetroRegisterFieldPickListDetailBo = rgf;
                            }
                        }

                        if (acm.ReportTypeName == AccountCodeMappingBo.ReportTypeIfrs17Name && acm.IsBalanceSheetStr == "BS" && string.IsNullOrEmpty(acm.TreatyNumber))
                        {
                            SetProcessCount("Treaty Number empty");
                            Errors.Add(string.Format("Please enter the Treaty Number at row {0}", TextFile.RowIndex));
                            error = true;
                        }
                    }

                    if (!string.IsNullOrEmpty(acm.AccountCode))
                    {
                        AccountCodeBo accountCodeBo = AccountCodeService.FindByCodeType(acm.AccountCode, AccountCodeType);
                        if (accountCodeBo == null)
                        {
                            SetProcessCount("Account Code Not Found");
                            Errors.Add(string.Format("The Account Code doesn't exists: {0} at row {1}", acm.AccountCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            acm.AccountCodeId = accountCodeBo.Id;
                            acm.AccountCodeBo = accountCodeBo;
                        }

                        if (!string.IsNullOrEmpty(acm.ReportTypeName))
                        {
                            int reportType = AccountCodeMappingBo.GetReportTypeByName(acm.ReportTypeName.Trim());
                            if (reportType == AccountCodeMappingBo.ReportTypeIfrs4 || reportType == AccountCodeMappingBo.ReportTypeIfrs17)
                            {
                                if (reportType == AccountCodeMappingBo.ReportTypeIfrs4 && accountCodeBo.ReportingType == AccountCodeBo.ReportingTypeIFRS4)
                                {
                                    acm.AccountCodeId = accountCodeBo.Id;
                                    acm.AccountCodeBo = accountCodeBo;
                                }
                                else if (reportType == AccountCodeMappingBo.ReportTypeIfrs17 && accountCodeBo.ReportingType == AccountCodeBo.ReportingTypeIFRS17)
                                {
                                    acm.AccountCodeId = accountCodeBo.Id;
                                    acm.AccountCodeBo = accountCodeBo;
                                }
                                else
                                {
                                    SetProcessCount("Reporting Type of the Account Code and Report Type must be the same");
                                    Errors.Add(string.Format("The Reporting Type of the Account Code, {0} is {1} and Report Type, {2} must be the same at row {3}", acm.AccountCode, AccountCodeBo.GetReportingTypeName(accountCodeBo.ReportingType), acm.ReportTypeName, TextFile.RowIndex));
                                    error = true;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(acm.DebitCreditIndicatorPositiveStr))
                        {
                            if (acm.DebitCreditIndicatorPositiveStr != AccountCodeMappingBo.DebitCreditIndicatorCName && acm.DebitCreditIndicatorPositiveStr != AccountCodeMappingBo.DebitCreditIndicatorDName)
                            {
                                SetProcessCount("Debit/Credit Indicator - Positive must be either 'C' or 'D'");
                                Errors.Add(string.Format("Debit/Credit Indicator - Positive has invalid value {0}, must be either 'C' or 'D' at row {0}", acm.DebitCreditIndicatorPositiveStr, TextFile.RowIndex));
                                error = true;
                            }
                            else
                            {
                                acm.DebitCreditIndicatorPositive = AccountCodeMappingBo.GetDebitCreditIndicatorByName(acm.DebitCreditIndicatorPositiveStr);
                            }
                        }

                        if (!string.IsNullOrEmpty(acm.DebitCreditIndicatorNegativeStr))
                        {
                            if (acm.DebitCreditIndicatorNegativeStr != AccountCodeMappingBo.DebitCreditIndicatorCName && acm.DebitCreditIndicatorNegativeStr != AccountCodeMappingBo.DebitCreditIndicatorDName)
                            {
                                SetProcessCount("Debit/Credit Indicator - Negative must be either 'C' or 'D'");
                                Errors.Add(string.Format("Debit/Credit Indicator - Negative has invalid value {0}, must be either 'C' or 'D' at row {0}", acm.DebitCreditIndicatorNegativeStr, TextFile.RowIndex));
                                error = true;
                            }
                            else
                            {
                                acm.DebitCreditIndicatorNegative = AccountCodeMappingBo.GetDebitCreditIndicatorByName(acm.DebitCreditIndicatorNegativeStr);
                            }
                        }
                    }
                    else
                    {
                        SetProcessCount("Account Code Empty");
                        Errors.Add(string.Format("Please enter the Account Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }
                }
                catch (Exception e)
                {
                    SetProcessCount("Error");
                    Errors.Add(string.Format("Error Triggered: {0} at row {1}", e.Message, TextFile.RowIndex));
                    error = true;
                }
                string action = IsRetro ? TextFile.GetColValue(AccountCodeMappingBo.RetroColumnAction) : TextFile.GetColValue(AccountCodeMappingBo.ColumnAction);

                if (error)
                {
                    continue;
                }

                switch (action.ToLower().Trim())
                {
                    case "u":
                    case "update":

                        AccountCodeMappingBo acmDb = AccountCodeMappingService.Find(acm.Id);
                        if (acmDb == null)
                        {
                            AddNotFoundError(acm);
                            continue;
                        }

                        var mappingResult = AccountCodeMappingService.ValidateMapping(acm);
                        if (!mappingResult.Valid)
                        {
                            foreach (var e in mappingResult.ToErrorArray())
                            {
                                Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                            }
                            error = true;
                        }
                        if (error)
                        {
                            continue;
                        }

                        UpdateData(ref acmDb, acm);

                        trail = new TrailObject();
                        result = AccountCodeMappingService.Update(ref acmDb, ref trail);

                        AccountCodeMappingService.ProcessMappingDetail(acmDb, acmDb.CreatedById); // DO NOT TRAIL
                        Trail(result, acmDb, trail, "Update");

                        break;

                    case "d":
                    case "del":
                    case "delete":

                        if (acm.Id != 0 && AccountCodeMappingService.IsExists(acm.Id))
                        {
                            trail = new TrailObject();
                            result = AccountCodeMappingService.Delete(acm, ref trail);
                            Trail(result, acm, trail, "Delete");
                        }
                        else
                        {
                            AddNotFoundError(acm);
                            continue;
                        }

                        break;

                    default:

                        if (acm.Id != 0 && AccountCodeMappingService.IsExists(acm.Id))
                        {
                            SetProcessCount("Record Found");
                            Errors.Add(string.Format("The Account Code Mapping ID exists: {0} at row {1}", acm.Id, TextFile.RowIndex));
                            continue;
                        }

                        mappingResult = AccountCodeMappingService.ValidateMapping(acm);
                        if (!mappingResult.Valid)
                        {
                            foreach (var e in mappingResult.ToErrorArray())
                            {
                                Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                            }
                            error = true;
                        }
                        if (error)
                        {
                            continue;
                        }

                        trail = new TrailObject();
                        result = AccountCodeMappingService.Create(ref acm, ref trail);

                        AccountCodeMappingService.ProcessMappingDetail(acm, acm.CreatedById); // DO NOT TRAIL
                        Trail(result, acm, trail, "Create");

                        break;
                }
            }

            PrintProcessCount();

            TextFile.Close();

            foreach (string error in Errors)
            {
                PrintError(error);
            }
        }

        public void ExportWriteLine(object line)
        {
            using (var textFile = new TextFile(FilePath, true, true))
            {
                textFile.WriteLine(line);
            }
        }

        public AccountCodeMappingBo SetData()
        {
            var acm = new AccountCodeMappingBo
            {
                Id = 0,
                CreatedById = AuthUserId != null ? AuthUserId.Value : DataAccess.Entities.Identity.User.DefaultSuperUserId,
                UpdatedById = AuthUserId != null ? AuthUserId : DataAccess.Entities.Identity.User.DefaultSuperUserId,
            };

            if (IsRetro)
            {
                acm.ReportTypeName = TextFile.GetColValue(AccountCodeMappingBo.RetroColumnReportType);
                acm.TypeName = TextFile.GetColValue(AccountCodeMappingBo.RetroColumnType);
                acm.TreatyType = TextFile.GetColValue(AccountCodeMappingBo.RetroColumnTreatyType);
                acm.TreatyNumber = TextFile.GetColValue(AccountCodeMappingBo.RetroColumnTreatyNo);
                acm.TreatyCode = TextFile.GetColValue(AccountCodeMappingBo.RetroColumnTreatyCode);
                acm.ClaimCode = TextFile.GetColValue(AccountCodeMappingBo.RetroColumnClaimCode);
                acm.BusinessOrigin = TextFile.GetColValue(AccountCodeMappingBo.RetroColumnBusinessOrigin);
                acm.TransactionTypeCode = TextFile.GetColValue(AccountCodeMappingBo.RetroColumnTransactionTypeCode);
                acm.RetroRegisterField = TextFile.GetColValue(AccountCodeMappingBo.RetroColumnRetroRegisterField);
                acm.InvoiceField = null;
                acm.ModifiedContractCode = TextFile.GetColValue(AccountCodeMappingBo.RetroColumnModifiedContractCode);
                acm.AccountCode = TextFile.GetColValue(AccountCodeMappingBo.RetroColumnAccountCode);
                acm.IsBalanceSheetStr = TextFile.GetColValue(AccountCodeMappingBo.RetroColumnIsBalanceSheet);
                acm.DebitCreditIndicatorPositiveStr = TextFile.GetColValue(AccountCodeMappingBo.RetroColumnDebitCreditIndicatorPositive);
                acm.DebitCreditIndicatorNegativeStr = TextFile.GetColValue(AccountCodeMappingBo.RetroColumnDebitCreditIndicatorNegative);
            }
            else
            {
                acm.ReportTypeName = TextFile.GetColValue(AccountCodeMappingBo.ColumnReportType);
                acm.TypeName = TextFile.GetColValue(AccountCodeMappingBo.ColumnType);
                acm.TreatyType = TextFile.GetColValue(AccountCodeMappingBo.ColumnTreatyType);
                acm.TreatyCode = TextFile.GetColValue(AccountCodeMappingBo.ColumnTreatyCode);
                acm.ClaimCode = TextFile.GetColValue(AccountCodeMappingBo.ColumnClaimCode);
                acm.BusinessOrigin = TextFile.GetColValue(AccountCodeMappingBo.ColumnBusinessOrigin);
                acm.TransactionTypeCode = TextFile.GetColValue(AccountCodeMappingBo.ColumnTransactionTypeCode);
                acm.InvoiceField = TextFile.GetColValue(AccountCodeMappingBo.ColumnInvoiceField);
                acm.ModifiedContractCode = TextFile.GetColValue(AccountCodeMappingBo.ColumnModifiedContractCode);
                acm.RetroRegisterField = null;
                acm.AccountCode = TextFile.GetColValue(AccountCodeMappingBo.ColumnAccountCode);
                acm.IsBalanceSheetStr = TextFile.GetColValue(AccountCodeMappingBo.ColumnIsBalanceSheet);
                acm.DebitCreditIndicatorPositiveStr = TextFile.GetColValue(AccountCodeMappingBo.ColumnDebitCreditIndicatorPositive);
                acm.DebitCreditIndicatorNegativeStr = TextFile.GetColValue(AccountCodeMappingBo.ColumnDebitCreditIndicatorNegative);
            }

            acm.TreatyType = acm.TreatyType?.TrimEnd(charsToTrim);
            acm.ClaimCode = acm.ClaimCode?.TrimEnd(charsToTrim);
            acm.BusinessOrigin = acm.BusinessOrigin?.TrimEnd(charsToTrim);
            acm.InvoiceField = acm.InvoiceField?.TrimEnd(charsToTrim);

            string idStr = TextFile.GetColValue(AccountCodeMappingBo.ColumnId);
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
            {
                acm.Id = id;
            }

            return acm;
        }

        public void UpdateData(ref AccountCodeMappingBo acmDb, AccountCodeMappingBo acm)
        {
            acmDb.ReportType = acm.ReportType;
            acmDb.Type = acm.Type;
            acmDb.TreatyType = acm.TreatyType;
            acmDb.TreatyNumber = acm.TreatyNumber;
            acmDb.TreatyCodeId = acm.TreatyCodeId;
            acmDb.ClaimCode = acm.ClaimCode;
            acmDb.BusinessOrigin = acm.BusinessOrigin;
            acmDb.TransactionTypeCodePickListDetailId = acm.TransactionTypeCodePickListDetailId;
            acmDb.RetroRegisterFieldPickListDetailId = acm.RetroRegisterFieldPickListDetailId;
            acmDb.InvoiceField = acm.InvoiceField;
            acmDb.ModifiedContractCodeId = acm.ModifiedContractCodeId;
            acmDb.AccountCodeId = acm.AccountCodeId;
            acmDb.IsBalanceSheet = acm.IsBalanceSheet;
            acmDb.DebitCreditIndicatorPositive = acm.DebitCreditIndicatorPositive;
            acmDb.DebitCreditIndicatorNegative = acm.DebitCreditIndicatorNegative;
            acmDb.UpdatedById = acm.UpdatedById;
        }

        public void Trail(Result result, AccountCodeMappingBo acm, TrailObject trail, string action)
        {
            if (result.Valid)
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    acm.Id,
                    string.Format("{0} Account Code Mapping", action),
                    result,
                    trail,
                    acm.UpdatedById.Value
                );
                UserTrailService.Create(ref userTrailBo);

                SetProcessCount(action);
            }
        }

        public bool ValidateDateTimeFormat(int type, ref AccountCodeMappingBo acm)
        {
            string header = Columns.Where(m => m.ColIndex == type).Select(m => m.Header).FirstOrDefault();
            string property = Columns.Where(m => m.ColIndex == type).Select(m => m.Property).FirstOrDefault();

            bool valid = true;
            string value = TextFile.GetColValue(type);
            if (!string.IsNullOrEmpty(value))
            {
                if (Util.TryParseDateTime(value, out DateTime? datetime, out string error))
                {
                    acm.SetPropertyValue(property, datetime.Value);
                }
                else
                {
                    SetProcessCount(string.Format(header, "Error"));
                    Errors.Add(string.Format("The {0} format can not be read: {1} at row {2}", header, value, TextFile.RowIndex));
                    valid = false;
                }
            }
            return valid;
        }

        public void AddNotFoundError(AccountCodeMappingBo acm)
        {
            SetProcessCount("Record Not Found");
            Errors.Add(string.Format("The Account Code Mapping ID doesn't exists: {0} at row {1}", acm.Id, TextFile.RowIndex));
        }

        public void GetColumns()
        {
            if (IsRetro)
                Columns = AccountCodeMappingBo.GetRetroColumns();
            else
                Columns = AccountCodeMappingBo.GetColumns();
        }
    }
}
