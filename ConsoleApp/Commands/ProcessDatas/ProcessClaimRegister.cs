using BusinessObject;
using BusinessObject.Identity;
using ConsoleApp.Commands.RawFiles.ClaimRegister;
using Services;
using Services.Identity;
using Services.SoaDatas;
using Shared;
using Shared.DataAccess;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace ConsoleApp.Commands.ProcessDatas
{
    public class ProcessClaimRegister : Command
    {
        public List<Column> Columns { get; set; }

        public List<Column> EditableColumns { get; set; }

        public int? IdColIndex { get; set; }

        public string FilePath { get; set; }

        public HttpPostedFileBase PostedFile { get; set; }

        public int AuthUserId { get; set; }

        public TextFile TextFile { get; set; }

        public List<string> Errors { get; set; }

        public Regex QuarterRegex { get; set; }

        public const string QuarterRegexPattern = @"^[0-9]{4}\s[Q]{1}[1-4]{1}$";

        public ProcessClaimRegister()
        {
            Title = "ProcessClaimRegister";
            Description = "To read Process Claim Register csv file and insert into database";
            Arguments = new string[]
            {
                "filePath",
            };
            Hide = true;
            Errors = new List<string> { };
            QuarterRegex = new Regex(QuarterRegexPattern);
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

            //TrailObject trail;
            //Result result;

            while (TextFile.GetNextRow() != null)
            {
                if (TextFile.RowIndex == 1)
                {
                    SetHeader();
                    if (IsSuccess)
                    {
                        GetEditableColumns();
                        continue;
                    }
                    else
                        break;
                }

                if (PrintCommitBuffer())
                {
                }
                SetProcessCount();

                try
                {
                    ClaimRegisterBo bo = null;
                    string idStr = TextFile.GetColValue(IdColIndex.Value);
                    if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
                    {
                        bo = ClaimRegisterService.Find(id);
                        if (bo == null)
                        {
                            AddNotFoundError(id);
                            continue;
                        }
                    }
                    else
                    {
                        Errors.Add(string.Format("The Id format cannot be read: {0} at row {1}", idStr, TextFile.RowIndex));
                        continue;
                    }

                    double? oriClaimAmount = bo.ClaimRecoveryAmt;
                    string oriClaimCode = bo.ClaimCode;
                    string oriTreatyType = bo.TreatyType;

                    bool valid = true;
                    foreach (var column in EditableColumns)
                    {
                        int colIndex = column.ColIndex.Value;
                        string value = TextFile.GetColValue(colIndex);
                        string property = column.Property;

                        if (string.IsNullOrEmpty(value))
                        {
                            bo.SetPropertyValue(property, null);
                            continue;
                        }

                        if (property == "TargetDateToIssueInvoice")
                        {
                            valid = valid ? SetDate(ref bo, column, value) : valid;
                            continue;
                        }

                        if (property == "SoaDataBatchId")
                        {
                            if (SetInteger(ref bo, column, value) && bo.SoaDataBatchId.HasValue)
                            {
                                if (!SoaDataBatchService.IsExists(bo.SoaDataBatchId.Value))
                                {
                                    SetProcessCount(string.Format("SOA Data Not Found"));
                                    Errors.Add(string.Format("The SOA Data cannot be found: {0} at row {1}", value, TextFile.RowIndex));
                                    bo.SoaDataBatchId = null;
                                    valid = false;
                                }
                            }
                            else
                            {
                                valid = false;
                            }
                            continue;
                        }

                        // Check if Column Type has value > 0. Due to DATE_OF_COMMENCEMENT no match with StandardClaimDataOutputBo (REINS_EFF_DATE_POL)
                        int type = column.Type != 0 ? column.Type : StandardClaimDataOutputBo.GetTypeByCode(column.Header);
                        int dataType = StandardClaimDataOutputBo.GetDataTypeByType(type);
                        switch (dataType)
                        {
                            case StandardOutputBo.DataTypeDate:
                                valid = valid ? SetDate(ref bo, column, value) : valid;
                                break;
                            case StandardOutputBo.DataTypeString:
                                valid = valid ? SetString(ref bo, column, type, value) : valid;
                                break;
                            case StandardOutputBo.DataTypeAmount:
                            case StandardOutputBo.DataTypePercentage:
                                valid = valid ? SetDouble(ref bo, column, value) : valid;
                                break;
                            case StandardOutputBo.DataTypeInteger:
                                valid = valid ? SetInteger(ref bo, column, value) : valid;
                                break;
                            case StandardOutputBo.DataTypeDropDown:
                                valid = valid ? SetDropDown(ref bo, column, type, value) : valid;
                                break;
                            default:
                                bo.SetPropertyValue(column.Property, value);
                                break;
                        }
                    }

                    if (valid)
                    {
                        bool provision = false;
                        if (bo.ClaimRecoveryAmt != oriClaimAmount || bo.ClaimCode != oriClaimCode || bo.TreatyType != oriTreatyType)
                        {
                            if (bo.ProvisionStatus == ClaimRegisterBo.ProvisionStatusProvisioned || bo.ProvisionStatus == ClaimRegisterBo.ProvisionStatusProvisioning)
                            {
                                bo.ProvisionStatus = ClaimRegisterBo.ProvisionStatusPending;
                            }
                            provision = true;
                        }

                        Update(bo);
                        if (provision)
                        {
                            ProvisionClaimRegister provisionClaimRegister = new ProvisionClaimRegister(bo, provisionDirectRetro: false);
                            provisionClaimRegister.Provision();
                        }
                    }
                }
                catch (Exception e)
                {
                    SetProcessCount("Error");
                    Errors.Add(string.Format("Error Triggered: {0} at row {1}", e.Message, TextFile.RowIndex));
                }
            }

            PrintProcessCount();

            TextFile.Close();

            foreach (string error in Errors)
            {
                PrintError(error);
            }
        }

        public void Update(ClaimRegisterBo bo)
        {
            TrailObject trail = new TrailObject();
            Result result = ClaimRegisterService.Update(ref bo, ref trail);
            if (result.Valid)
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    bo.Id,
                    string.Format("Update Claim Register"),
                    result,
                    trail,
                    AuthUserId
                );
                UserTrailService.Create(ref userTrailBo);

                SetProcessCount("Update");
            }
        }

        public bool SetDate(ref ClaimRegisterBo bo, Column column, string value)
        {
            bool valid = true;
            if (Util.TryParseDateTime(value, out DateTime? datetime, out string _))
            {
                bo.SetPropertyValue(column.Property, datetime.Value);
            }
            else
            {
                SetProcessCount(string.Format("{0}{1}", column.Header, "Error"));
                Errors.Add(string.Format("The {0} format can not be read: {1} at row {2}", column.Header, value, TextFile.RowIndex));
                valid = false;
            }

            return valid;
        }

        public bool SetDouble(ref ClaimRegisterBo bo, Column column, string value)
        {
            bool valid = true;
            if (double.TryParse(value, out double r))
            {
                bo.SetPropertyValue(column.Property, r);
            }
            else
            {
                SetProcessCount(string.Format("{0}{1}", column.Header, "Error"));
                Errors.Add(string.Format("The {0} format can not be read: {1} at row {2}", column.Header, value, TextFile.RowIndex));
                valid = false;
            }

            return valid;
        }

        public bool SetInteger(ref ClaimRegisterBo bo, Column column, string value)
        {
            bool valid = true;
            if (int.TryParse(value, out int r))
            {
                bo.SetPropertyValue(column.Property, r);
            }
            else
            {
                SetProcessCount(string.Format("{0}{1}", column.Header, "Error"));
                Errors.Add(string.Format("The {0} format can not be read: {1} at row {2}", column.Header, value, TextFile.RowIndex));
                valid = false;
            }

            return valid;
        }

        public bool SetDropDown(ref ClaimRegisterBo bo, Column column, int type, string value)
        {
            bool valid = true;

            bo.SetPropertyValue(column.Property, value);
            PickListDetailBo pickListDetailBo = PickListDetailService.FindByStandardClaimDataOutputIdCode(type, value);
            if (pickListDetailBo == null)
            {
                SetProcessCount(string.Format("{0} Not Found", column.Header));
                Errors.Add(string.Format("The {0} doesn't exists: {1} at row {2}", column.Property, value, TextFile.RowIndex));
                valid = false;
            }

            return valid;
        }

        public bool SetString(ref ClaimRegisterBo bo, Column column, int type, string value)
        {
            string error = null;
            bo.SetPropertyValue(column.Property, value);
            switch (type)
            {
                case StandardClaimDataOutputBo.TypeTreatyCode:
                    TreatyCodeBo treatyCodeBo = TreatyCodeService.FindByCode(value);
                    if (treatyCodeBo == null)
                    {
                        SetProcessCount("Treaty Code Not Found");
                        error = string.Format("{0} at row {1}", string.Format(MessageBag.TreatyCodeNotFound, value), TextFile.RowIndex);
                    }
                    else if (treatyCodeBo.Status == TreatyCodeBo.StatusInactive)
                    {
                        SetProcessCount("Treaty Code Inactive");
                        error = string.Format("{0}: {1} at row {2}", MessageBag.TreatyCodeStatusInactive, value, TextFile.RowIndex);
                    }
                    break;
                case StandardClaimDataOutputBo.TypeCedingCompany:
                    CedantBo cedantBo = CedantService.FindByCode(value);
                    if (cedantBo == null)
                    {
                        SetProcessCount("Ceding Company Not Found");
                        error = string.Format("{0} at row {1}", string.Format(MessageBag.CedantNotFound, value), TextFile.RowIndex);
                    }
                    else if (cedantBo.Status == CedantBo.StatusInactive)
                    {
                        SetProcessCount("Ceding Company Inactive");
                        error = string.Format("{0}: {1} at row {2}", MessageBag.CedantStatusInactive, value, TextFile.RowIndex);
                    }
                    break;
                case StandardClaimDataOutputBo.TypeMlreBenefitCode:
                    BenefitBo benefitBo = BenefitService.FindByCode(value);
                    if (benefitBo == null)
                    {
                        SetProcessCount("MLRe Benefit Code Not Found");
                        error = string.Format("{0} at row {1}", string.Format(MessageBag.BenefitCodeNotFound, value), TextFile.RowIndex);
                    }
                    else if (benefitBo.Status == BenefitBo.StatusInactive)
                    {
                        SetProcessCount("Benefit Inactive");
                        error = string.Format("{0}: {1} at row {2}", MessageBag.BenefitStatusInactive, value, TextFile.RowIndex);
                    }
                    break;
                case StandardClaimDataOutputBo.TypeMlreEventCode:
                    EventCodeBo eventCodeBo = EventCodeService.FindByCode(value);
                    if (eventCodeBo == null)
                    {
                        SetProcessCount("MLRe Event Code Not Found");
                        error = string.Format("{0} at row {1}", string.Format(MessageBag.EventCodeNotFound, value), TextFile.RowIndex);
                    }
                    else if (eventCodeBo.Status == EventCodeBo.StatusInactive)
                    {
                        SetProcessCount("Event Code Inactive");
                        error = string.Format("{0}: {1} at row {2}", MessageBag.EventCodeStatusInactive, value, TextFile.RowIndex);
                    }
                    break;
                case StandardClaimDataOutputBo.TypeClaimCode:
                    ClaimCodeBo claimCodeBo = ClaimCodeService.FindByCode(value);
                    if (claimCodeBo == null)
                    {
                        SetProcessCount("Claim Code Not Found");
                        error = string.Format("{0} at row {1}", string.Format(MessageBag.ClaimCodeNotFound, value), TextFile.RowIndex);
                    }
                    else if (claimCodeBo.Status == ClaimCodeBo.StatusInactive)
                    {
                        SetProcessCount("Claim Code Inactive");
                        error = string.Format("{0}: {1} at row {2}", MessageBag.ClaimCodeStatusInactive, value, TextFile.RowIndex);
                    }
                    break;
                case StandardClaimDataOutputBo.TypeLastTransactionQuarter:
                case StandardClaimDataOutputBo.TypeRiskQuarter:
                case StandardClaimDataOutputBo.TypeSoaQuarter:
                    if (!QuarterRegex.IsMatch(value))
                    {
                        SetProcessCount(string.Format("{0}{1}", column.Header, "Error"));
                        error = string.Format("The {0} format is incorrect: {1} at row {2}", column.Header, value, TextFile.RowIndex);
                    }
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(error))
            {
                Errors.Add(error);
                return false;
            }

            return true;
        }

        public void SetHeader()
        {
            while (TextFile.GetNextCol() != null)
            {
                var value = TextFile.GetValue();
                var column = Columns.Where(c => c.Header == value).FirstOrDefault();

                if (column == null)
                    Errors.Add(string.Format("Error Triggered: Column in header row does not exist {0}", value));
                else
                {
                    column.ColIndex = TextFile.ColIndex;
                    if (column.Property == "Id")
                    {
                        IdColIndex = TextFile.ColIndex;
                    }
                }
            }

            if (IdColIndex == null)
            {
                Errors.Add(string.Format("Error Triggered: Id Column is required"));
            }
        }

        public void AddNotFoundError(int id)
        {
            SetProcessCount("Record Not Found");
            Errors.Add(string.Format("The Claim Register ID doesn't exists: {0} at row {1}", id, TextFile.RowIndex));
        }

        public bool IsSuccess
        {
            get
            {
                return Errors.Count == 0;
            }
        }

        public List<Column> GetColumns()
        {
            Columns = ClaimRegisterBo.GetColumns();
            return Columns;
        }

        public List<Column> GetEditableColumns()
        {
            EditableColumns = Columns.Where(c => c.Editable == true).ToList();
            return EditableColumns;
        }
    }
}
