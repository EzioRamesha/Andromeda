using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.Retrocession;
using DataAccess.Entities.Retrocession;
using Services;
using Services.Identity;
using Services.Retrocession;
using Shared;
using Shared.DataAccess;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace ConsoleApp.Commands.ProcessDatas
{
    public class ProcessPerLifeDataCorrection : Command
    {
        public List<Column> Columns { get; set; }

        public string FilePath { get; set; }

        public HttpPostedFileBase PostedFile { get; set; }

        public int? AuthUserId { get; set; }

        public TextFile TextFile { get; set; }

        public List<string> Errors { get; set; }

        public char[] charsToTrim = { ',', '.', ' ' };

        public ProcessPerLifeDataCorrection()
        {
            Title = "ProcessPerLifeDataCorrection";
            Description = "To read Per Life Data Correction csv file and insert into database";
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

                PerLifeDataCorrectionBo pldc = null;
                try
                {
                    var entity = new PerLifeDataCorrection();
                    var insuredNameMaxLength = entity.GetAttributeFrom<MaxLengthAttribute>("InsuredName").Length;
                    var policyNumberMaxLength = entity.GetAttributeFrom<MaxLengthAttribute>("PolicyNumber").Length;

                    pldc = SetData();

                    if (!string.IsNullOrEmpty(pldc.TreatyCode))
                    {
                        TreatyCodeBo treatyCodeBo = TreatyCodeService.FindByCode(pldc.TreatyCode);
                        if (treatyCodeBo == null)
                        {
                            SetProcessCount("Treaty Code Not Found");
                            Errors.Add(string.Format("The Treaty Code doesn't exists: {0} at row {1}", pldc.TreatyCode, TextFile.RowIndex));
                            error = true;
                        }
                        else if (treatyCodeBo.Status == TreatyCodeBo.StatusInactive)
                        {
                            SetProcessCount("Treaty Code Inactive");
                            Errors.Add(string.Format("{0}: {1} at row {2}", MessageBag.TreatyCodeStatusInactive, pldc.TreatyCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            pldc.TreatyCodeId = treatyCodeBo.Id;
                            pldc.TreatyCodeBo = treatyCodeBo;
                        }
                    }
                    else
                    {
                        SetProcessCount("Treaty Code Empty");
                        Errors.Add(string.Format("Please enter the Treaty Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (string.IsNullOrEmpty(pldc.InsuredName))
                    {
                        SetProcessCount("Insured Name Empty");
                        Errors.Add(string.Format("Please enter the Insured Name at row {0}", TextFile.RowIndex));
                        error = true;
                    }
                    else
                    {
                        if (pldc.InsuredName.Length > insuredNameMaxLength)
                        {
                            SetProcessCount("Insured Name exceeded max length");
                            Errors.Add(string.Format("Insured Name length can not be more than {0} characters at row {1}", insuredNameMaxLength, TextFile.RowIndex));
                            error = true;
                        }
                    }

                    var insuredDateOfBirth = TextFile.GetColValue(PerLifeDataCorrectionBo.ColumnInsuredDateOfBirth);
                    if (!string.IsNullOrEmpty(insuredDateOfBirth))
                    {
                        if (!ValidateDateTimeFormat(PerLifeDataCorrectionBo.ColumnInsuredDateOfBirth, ref pldc))
                        {
                            error = true;
                        }
                    }
                    else
                    {
                        SetProcessCount("Insured Date of Birth Empty");
                        Errors.Add(string.Format("Please enter the Insured Date of Birth at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (string.IsNullOrEmpty(pldc.PolicyNumber))
                    {
                        SetProcessCount("Policy Number Empty");
                        Errors.Add(string.Format("Please enter the Policy Number at row {0}", TextFile.RowIndex));
                        error = true;
                    }
                    else
                    {
                        if (pldc.PolicyNumber.Length > policyNumberMaxLength)
                        {
                            SetProcessCount("Policy Number exceeded max length");
                            Errors.Add(string.Format("Policy Number length can not be more than {0} characters at row {1}", policyNumberMaxLength, TextFile.RowIndex));
                            error = true;
                        }
                    }

                    if (!string.IsNullOrEmpty(pldc.InsuredGenderCode))
                    {
                        PickListDetailBo igc = PickListDetailService.FindByPickListIdCode(PickListBo.InsuredGenderCode, pldc.InsuredGenderCode);
                        if (igc == null)
                        {
                            SetProcessCount("Org Gender Code Not Found");
                            Errors.Add(string.Format("The Org Gender Code doesn't exists: {0} at row {1}", pldc.InsuredGenderCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            pldc.InsuredGenderCodePickListDetailId = igc.Id;
                            pldc.InsuredGenderCodePickListDetailBo = igc;
                        }
                    }
                    else
                    {
                        SetProcessCount("Org Gender Code Empty");
                        Errors.Add(string.Format("Please enter the Org Gender Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(pldc.TerritoryOfIssueCode))
                    {
                        PickListDetailBo rbc = PickListDetailService.FindByPickListIdCode(PickListBo.TerritoryOfIssueCode, pldc.TerritoryOfIssueCode);
                        if (rbc == null)
                        {
                            SetProcessCount("Org Territory Of Issue ID Not Found");
                            Errors.Add(string.Format("The Org Territory of Issue ID doesn't exists: {0} at row {1}", pldc.TerritoryOfIssueCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            pldc.TerritoryOfIssueCodePickListDetailId = rbc.Id;
                            pldc.TerritoryOfIssueCodePickListDetailBo = rbc;
                        }
                    }
                    else
                    {
                        SetProcessCount("Org Territory of Issue ID Empty");
                        Errors.Add(string.Format("Please enter the Org Territory of Issue ID at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(pldc.PerLifeRetroGenderStr))
                    {
                        PerLifeRetroGenderBo plg = PerLifeRetroGenderService.FindByInsuredGenderCode(pldc.PerLifeRetroGenderStr);
                        if (plg == null)
                        {
                            SetProcessCount("Expected Gender Code Not Found");
                            Errors.Add(string.Format("The Expected Gender Code doesn't exists: {0} at row {1}", pldc.PerLifeRetroGenderStr, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            pldc.PerLifeRetroGenderId = plg.Id;
                            pldc.PerLifeRetroGenderBo = plg;
                        }
                    }
                    else
                    {
                        SetProcessCount("Expected Gender Code Empty");
                        Errors.Add(string.Format("Please enter the Expected Gender Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(pldc.PerLifeRetroCountryStr))
                    {
                        PerLifeRetroCountryBo plc = PerLifeRetroCountryService.FindByTerritoryOfIssueCode(pldc.PerLifeRetroCountryStr);
                        if (plc == null)
                        {
                            SetProcessCount("Expected Territory of Issue ID Not Found");
                            Errors.Add(string.Format("The Expected Territory of Issue ID doesn't exists: {0} at row {1}", pldc.PerLifeRetroCountryStr, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            pldc.PerLifeRetroCountryId = plc.Id;
                            pldc.PerLifeRetroCountryBo = plc;
                        }
                    }
                    else
                    {
                        SetProcessCount("Expected Territory of Issue ID Empty");
                        Errors.Add(string.Format("Please enter the Expected Territory of Issue ID at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    var dateOfExceptionDetected = TextFile.GetColValue(PerLifeDataCorrectionBo.ColumnDateOfExceptionDetected);
                    if (!string.IsNullOrEmpty(dateOfExceptionDetected))
                    {
                        if (!ValidateDateTimeFormat(PerLifeDataCorrectionBo.ColumnDateOfExceptionDetected, ref pldc))
                        {
                            error = true;
                        }
                    }
                    else
                    {
                        SetProcessCount("Date of Exception Detected Empty");
                        Errors.Add(string.Format("Please enter the Date of Exception Detected at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    var dateOfPolicyExist = TextFile.GetColValue(PerLifeDataCorrectionBo.ColumnDateOfPolicyExist);
                    if (!string.IsNullOrEmpty(dateOfPolicyExist))
                    {
                        if (!ValidateDateTimeFormat(PerLifeDataCorrectionBo.ColumnDateOfPolicyExist, ref pldc))
                        {
                            error = true;
                        }
                    }
                    else
                    {
                        SetProcessCount("1st Date of The Policy Exist In The System Empty");
                        Errors.Add(string.Format("Please enter the 1st Date of The Policy Exist In The System at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    string isProceedToAggregateStr = TextFile.GetColValue(PerLifeDataCorrectionBo.ColumnIsProceedToAggregate);
                    if (!string.IsNullOrEmpty(isProceedToAggregateStr))
                    {
                        if (Util.StringToBool(isProceedToAggregateStr, out bool b))
                        {
                            pldc.IsProceedToAggregate = b;
                        }
                        else
                        {
                            SetProcessCount("Proceed to Aggregate Invalid");
                            Errors.Add(string.Format("The Proceed to Aggregate is invalid at row {0}", TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        SetProcessCount("Proceed to Aggregate Empty");
                        Errors.Add(string.Format("Please enter the Proceed to Aggregate at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    var dateUpdated = TextFile.GetColValue(PerLifeDataCorrectionBo.ColumnDateUpdated);
                    if (!string.IsNullOrEmpty(dateUpdated))
                    {
                        if (!ValidateDateTimeFormat(PerLifeDataCorrectionBo.ColumnDateUpdated, ref pldc))
                        {
                            error = true;
                        }
                    }
                    else
                    {
                        SetProcessCount("Date Updated Empty");
                        Errors.Add(string.Format("Please enter the Date Updated In The System at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(pldc.ExceptionStatus))
                    {
                        PickListDetailBo rbc = PickListDetailService.FindByPickListIdCode(PickListBo.ExceptionStatus, pldc.ExceptionStatus);
                        if (rbc == null)
                        {
                            SetProcessCount("Exception Status Not Found");
                            Errors.Add(string.Format("The Exception Status doesn't exists: {0} at row {1}", pldc.ExceptionStatus, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            pldc.ExceptionStatusPickListDetailId = rbc.Id;
                            pldc.ExceptionStatusPickListDetailBo = rbc;
                        }
                    }
                    else
                    {
                        SetProcessCount("Exception Status Empty");
                        Errors.Add(string.Format("Please enter the Exception Status at row {0}", TextFile.RowIndex));
                        error = true;
                    }
                }
                catch (Exception e)
                {
                    SetProcessCount("Error");
                    Errors.Add(string.Format("Error Triggered: {0} at row {1}", e.Message, TextFile.RowIndex));
                    error = true;
                }
                string action = TextFile.GetColValue(PerLifeDataCorrectionBo.ColumnAction);

                if (error)
                {
                    continue;
                }

                switch (action.ToLower().Trim())
                {
                    case "u":
                    case "update":

                        PerLifeDataCorrectionBo pldcDb = PerLifeDataCorrectionService.Find(pldc.Id);
                        if (pldcDb == null)
                        {
                            AddNotFoundError(pldc);
                            continue;
                        }

                        UpdateData(ref pldcDb, pldc);

                        trail = new TrailObject();
                        result = PerLifeDataCorrectionService.Update(ref pldcDb, ref trail);

                        if (!result.Valid)
                        {
                            foreach (var e in result.ToErrorArray())
                            {
                                Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                            }
                            error = true;
                        }

                        Trail(result, pldcDb, trail, "Update");

                        break;

                    case "d":
                    case "del":
                    case "delete":

                        if (pldc.Id != 0 && PerLifeDataCorrectionService.IsExists(pldc.Id))
                        {
                            trail = new TrailObject();
                            result = PerLifeDataCorrectionService.Delete(pldc, ref trail);
                            Trail(result, pldc, trail, "Delete");
                        }
                        else
                        {
                            AddNotFoundError(pldc);
                            continue;
                        }

                        break;

                    default:

                        if (pldc.Id != 0 && PerLifeDataCorrectionService.IsExists(pldc.Id))
                        {
                            SetProcessCount("Record Found");
                            Errors.Add(string.Format("The MFRS17 Cell Mapping ID exists: {0} at row {1}", pldc.Id, TextFile.RowIndex));
                            continue;
                        }

                        trail = new TrailObject();
                        result = PerLifeDataCorrectionService.Create(ref pldc, ref trail);

                        if (!result.Valid)
                        {
                            foreach (var e in result.ToErrorArray())
                            {
                                Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                            }
                            error = true;
                        }

                        Trail(result, pldc, trail, "Create");

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

        public PerLifeDataCorrectionBo SetData()
        {
            var cm = new PerLifeDataCorrectionBo
            {
                Id = 0,
                TreatyCode = TextFile.GetColValue(PerLifeDataCorrectionBo.ColumnTreatyCode),
                InsuredName = TextFile.GetColValue(PerLifeDataCorrectionBo.ColumnInsuredName),
                PolicyNumber = TextFile.GetColValue(PerLifeDataCorrectionBo.ColumnPolicyNumber),
                InsuredGenderCode = TextFile.GetColValue(PerLifeDataCorrectionBo.ColumnInsuredGenderCode),
                TerritoryOfIssueCode = TextFile.GetColValue(PerLifeDataCorrectionBo.ColumnTerritoryOfIssueCode),
                PerLifeRetroGenderStr = TextFile.GetColValue(PerLifeDataCorrectionBo.ColumnPerLifeRetroGender),
                PerLifeRetroCountryStr = TextFile.GetColValue(PerLifeDataCorrectionBo.ColumnPerLifeRetroCountry),
                ExceptionStatus = TextFile.GetColValue(PerLifeDataCorrectionBo.ColumnExceptionStatus),
                Remark = TextFile.GetColValue(PerLifeDataCorrectionBo.ColumnRemark),

                CreatedById = AuthUserId != null ? AuthUserId.Value : DataAccess.Entities.Identity.User.DefaultSuperUserId,
                UpdatedById = AuthUserId != null ? AuthUserId : DataAccess.Entities.Identity.User.DefaultSuperUserId,
            };

            string idStr = TextFile.GetColValue(PerLifeDataCorrectionBo.ColumnId);
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
            {
                cm.Id = id;
            }

            return cm;
        }

        public void UpdateData(ref PerLifeDataCorrectionBo pldcDb, PerLifeDataCorrectionBo pldc)
        {
            pldcDb.TreatyCodeId = pldc.TreatyCodeId;
            pldcDb.InsuredName = pldc.InsuredName;
            pldcDb.InsuredDateOfBirth = pldc.InsuredDateOfBirth;
            pldcDb.PolicyNumber = pldc.PolicyNumber;
            pldcDb.InsuredGenderCodePickListDetailId = pldc.InsuredGenderCodePickListDetailId;
            pldcDb.TerritoryOfIssueCodePickListDetailId = pldc.TerritoryOfIssueCodePickListDetailId;
            pldcDb.PerLifeRetroGenderId = pldc.PerLifeRetroGenderId;
            pldcDb.PerLifeRetroCountryId = pldc.PerLifeRetroCountryId;
            pldcDb.DateOfExceptionDetected = pldc.DateOfExceptionDetected;
            pldcDb.DateOfPolicyExist = pldc.DateOfPolicyExist;
            pldcDb.IsProceedToAggregate = pldc.IsProceedToAggregate;
            pldcDb.DateUpdated = pldc.DateUpdated;
            pldcDb.ExceptionStatusPickListDetailId = pldc.ExceptionStatusPickListDetailId;
            pldcDb.Remark = pldc.Remark;
            pldcDb.UpdatedById = pldc.UpdatedById;
        }

        public void Trail(Result result, PerLifeDataCorrectionBo cm, TrailObject trail, string action)
        {
            if (result.Valid)
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    cm.Id,
                    string.Format("{0} Per Life Data Correction", action),
                    result,
                    trail,
                    cm.UpdatedById.Value
                );
                UserTrailService.Create(ref userTrailBo);

                SetProcessCount(action);
            }
        }

        public bool ValidateDateTimeFormat(int type, ref PerLifeDataCorrectionBo pldc)
        {
            string header = Columns.Where(m => m.ColIndex == type).Select(m => m.Header).FirstOrDefault();
            string property = Columns.Where(m => m.ColIndex == type).Select(m => m.Property).FirstOrDefault();

            bool valid = true;
            string value = TextFile.GetColValue(type);
            if (!string.IsNullOrEmpty(value))
            {
                if (Util.TryParseDateTime(value, out DateTime? datetime, out string error))
                {
                    pldc.SetPropertyValue(property, datetime.Value);
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

        public void AddNotFoundError(PerLifeDataCorrectionBo pldc)
        {
            SetProcessCount("Record Not Found");
            Errors.Add(string.Format("The Per Life Data Correction ID doesn't exists: {0} at row {1}", pldc.Id, TextFile.RowIndex));
        }

        public List<Column> GetColumns()
        {
            Columns = PerLifeDataCorrectionBo.GetColumns();
            return Columns;
        }
    }
}
