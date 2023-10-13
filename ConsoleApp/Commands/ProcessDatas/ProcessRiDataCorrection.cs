using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.RiDatas;
using Services;
using Services.Identity;
using Services.RiDatas;
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
    public class ProcessRiDataCorrection : Command
    {
        public List<Column> Columns { get; set; }

        public string FilePath { get; set; }

        public HttpPostedFileBase PostedFile { get; set; }

        public int? AuthUserId { get; set; }

        public TextFile TextFile { get; set; }

        public List<string> Errors { get; set; }

        public ProcessRiDataCorrection()
        {
            Title = "ProcessRiDataCorrection";
            Description = "To read RI Data Correction csv file and insert into database";
            Arguments = new string[]
            {
                "filePath",
            };
            Hide = true;
            Errors = new List<string> { };
            GetMappings();
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

                RiDataCorrectionBo rdc = null;
                try
                {
                    rdc = SetData();

                    if (!string.IsNullOrEmpty(rdc.CedantCode))
                    {
                        CedantBo cedant = CedantService.FindByCode(rdc.CedantCode);
                        if (cedant == null)
                        {
                            SetProcessCount("Cedant Code Not Found");
                            Errors.Add(string.Format("The Cedant Code doesn't exists: {0} at row {1}", rdc.CedantCode, TextFile.RowIndex));
                            error = true;
                        }
                        else if (cedant.Status == CedantBo.StatusInactive)
                        {
                            SetProcessCount("Cedant Code Inactive");
                            Errors.Add(string.Format("{0}: {1} at row {2}", MessageBag.CedantStatusInactive, rdc.CedantCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            rdc.CedantId = cedant.Id;
                            rdc.CedantBo = cedant;
                        }
                    }
                    else
                    {
                        SetProcessCount("Cedant Code Empty");
                        Errors.Add(string.Format("Please enter the Cedant Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(rdc.TreatyCode))
                    {
                        TreatyCodeBo treatyCode = TreatyCodeService.FindByCode(rdc.TreatyCode);
                        if (treatyCode == null)
                        {
                            SetProcessCount("Treaty Code Not Found");
                            Errors.Add(string.Format("The Treaty Code doesn't exists: {0} at row {1}", rdc.TreatyCode, TextFile.RowIndex));
                            error = true;
                        }
                        else if (treatyCode.Status == TreatyCodeBo.StatusInactive)
                        {
                            SetProcessCount("Treaty Code Inactive");
                            Errors.Add(string.Format("{0}: {1} at row {2}", MessageBag.TreatyCodeStatusInactive, rdc.TreatyCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            rdc.TreatyCodeId = treatyCode.Id;
                            rdc.TreatyCodeBo = treatyCode;
                        }
                    }
                    else
                    {
                        rdc.TreatyCodeId = null;
                    }

                    if (string.IsNullOrEmpty(rdc.PolicyNumber))
                    {
                        SetProcessCount("Policy Number Empty");
                        Errors.Add(string.Format("Please enter the Policy Number at row {0}", TextFile.RowIndex));
                        error = true;
                    }
                    else if (rdc.PolicyNumber.Length > 150)
                    {
                        SetProcessCount("Policy Number Exceed Max Length");
                        Errors.Add(string.Format("The Policy Number exceed Max Length (150) at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (string.IsNullOrEmpty(rdc.InsuredRegisterNo))
                    {
                        rdc.InsuredRegisterNo = null;
                    }
                    else if (rdc.InsuredRegisterNo.Length > 30)
                    {
                        SetProcessCount("Insured Register Number Exceed Max Length");
                        Errors.Add(string.Format("The Insured Register Number exceed Max Length (30) at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(rdc.InsuredGenderCode))
                    {
                        PickListDetailBo pld = PickListDetailService.FindByPickListIdCode(PickListBo.InsuredGenderCode, rdc.InsuredGenderCode);
                        if (pld == null)
                        {
                            SetProcessCount("Insured Gender Code Not Found");
                            Errors.Add(string.Format("The Insured Gender Code doesn't exists: {0} at row {1}", rdc.InsuredGenderCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            rdc.InsuredGenderCodePickListDetailId = pld.Id;
                            rdc.InsuredGenderCodePickListDetailBo = pld;
                        }
                    }

                    if (!string.IsNullOrEmpty(TextFile.GetColValue(RiDataCorrectionBo.ColumnInsuredDateOfBirth)))
                    {
                        if (!ValidateDateTimeFormat(RiDataCorrectionBo.ColumnInsuredDateOfBirth, ref rdc, true))
                        {
                            error = true;
                        }
                    }

                    if (string.IsNullOrEmpty(rdc.InsuredName))
                    {
                        rdc.InsuredName = null;
                    }
                    else if (rdc.InsuredName.Length > 128)
                    {
                        SetProcessCount("Insured Name Exceed Max Length");
                        Errors.Add(string.Format("The Insured Name exceed Max Length (128) at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (string.IsNullOrEmpty(rdc.CampaignCode))
                    {
                        rdc.CampaignCode = null;
                    }
                    else if (rdc.CampaignCode.Length > 10)
                    {
                        SetProcessCount("Campaign Code Exceed Max Length");
                        Errors.Add(string.Format("The Campaign Code exceed Max Length (10) at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(rdc.ReinsBasisCode))
                    {
                        PickListDetailBo rbc = PickListDetailService.FindByPickListIdCode(PickListBo.ReinsBasisCode, rdc.ReinsBasisCode);
                        if (rbc == null)
                        {
                            SetProcessCount("Reins Basis Code Not Found");
                            Errors.Add(string.Format("The Reins Basis Code doesn't exists: {0} at row {1}", rdc.ReinsBasisCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            rdc.ReinsBasisCodePickListDetailId = rbc.Id;
                            rdc.ReinsBasisCodePickListDetailBo = rbc;
                        }
                    }
                    else
                    {
                        rdc.ReinsBasisCodePickListDetailId = null;
                    }

                    var apLoadingStr = TextFile.GetColValue(RiDataCorrectionBo.ColumnApLoading);
                    if (!string.IsNullOrEmpty(apLoadingStr))
                    {
                        if (Util.IsValidDouble(apLoadingStr, out double? output, out string _))
                        {
                            rdc.ApLoading = output.Value;
                        }
                        else
                        {
                            SetProcessCount("AP Loading Invalid");
                            Errors.Add(string.Format("The AP Loading is invalid: {0} at row {1}", apLoadingStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        rdc.ApLoading = null;
                    }
                }
                catch (Exception e)
                {
                    SetProcessCount("Error");
                    Errors.Add(string.Format("Error Triggered: {0} at row {1}", e.Message, TextFile.RowIndex));
                    error = true;
                }

                if (error)
                {
                    continue;
                }

                string action = TextFile.GetColValue(RiDataCorrectionBo.ColumnAction);
                if (action == null)
                    action = "";

                switch (action.ToLower().Trim())
                {
                    case "u":
                    case "update":

                        RiDataCorrectionBo rdcDb = RiDataCorrectionService.Find(rdc.Id);
                        if (rdcDb == null)
                        {
                            AddNotFoundError(rdc);
                            continue;
                        }

                        var validateResult = RiDataCorrectionService.ValidateMappedResult(rdc);
                        if (!validateResult.Valid)
                        {
                            foreach (var e in validateResult.ToErrorArray())
                            {
                                Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                            }
                            error = true;
                        }
                        if (error)
                        {
                            continue;
                        }

                        UpdateData(ref rdcDb, rdc);

                        trail = new TrailObject();
                        result = RiDataCorrectionService.Update(ref rdcDb, ref trail);
                        Trail(result, rdcDb, trail, "Update");

                        break;

                    case "d":
                    case "del":
                    case "delete":

                        if (rdc.Id != 0 && RiDataCorrectionService.IsExists(rdc.Id))
                        {
                            trail = new TrailObject();
                            result = RiDataCorrectionService.Delete(rdc, ref trail);
                            Trail(result, rdc, trail, "Delete");
                        }
                        else
                        {
                            AddNotFoundError(rdc);
                            continue;
                        }

                        break;

                    default:

                        if (rdc.Id != 0 && RiDataCorrectionService.IsExists(rdc.Id))
                        {
                            SetProcessCount("Record Found");
                            Errors.Add(string.Format("The RI Data Correction ID exists: {0} at row {1}", rdc.Id, TextFile.RowIndex));
                            continue;
                        }

                        var rdcFound = RiDataCorrectionService.FindByCedantIdTreatyCodeIdPolicyRegNo(rdc.CedantId, rdc.PolicyNumber, rdc.InsuredRegisterNo, rdc.TreatyCodeId);
                        if (rdcFound != null)
                        {
                            SetProcessCount("Record Found");
                            Errors.Add(string.Format(
                                "The RI Data Correction found by Cedant Code: {0}, Tready Code: {1}, Policy No: {2}, Insured Reg. No.: {3} at row {4}",
                                rdc.CedantCode,
                                rdc.TreatyCode,
                                rdc.PolicyNumber,
                                rdc.InsuredRegisterNo,
                                TextFile.RowIndex
                            ));
                            continue;
                        }

                        validateResult = RiDataCorrectionService.ValidateMappedResult(rdc);
                        if (!validateResult.Valid)
                        {
                            foreach (var e in validateResult.ToErrorArray())
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
                        result = RiDataCorrectionService.Create(ref rdc, ref trail);
                        Trail(result, rdc, trail, "Create");

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

        public RiDataCorrectionBo SetData()
        {
            var rdc = new RiDataCorrectionBo
            {
                Id = 0,
                CedantCode = TextFile.GetColValue(RiDataCorrectionBo.ColumnCedantCode),
                TreatyCode = TextFile.GetColValue(RiDataCorrectionBo.ColumnTreatyCode),
                PolicyNumber = TextFile.GetColValue(RiDataCorrectionBo.ColumnPolicyNumber),
                InsuredRegisterNo = TextFile.GetColValue(RiDataCorrectionBo.ColumnInsuredRegisterNo),
                InsuredGenderCode = TextFile.GetColValue(RiDataCorrectionBo.ColumnInsuredGenderCode),
                InsuredName = TextFile.GetColValue(RiDataCorrectionBo.ColumnInsuredName),
                CampaignCode = TextFile.GetColValue(RiDataCorrectionBo.ColumnCampaignCode),
                ReinsBasisCode = TextFile.GetColValue(RiDataCorrectionBo.ColumnReinsBasisCode),
                CreatedById = AuthUserId != null ? AuthUserId.Value : DataAccess.Entities.Identity.User.DefaultSuperUserId,
                UpdatedById = AuthUserId != null ? AuthUserId : DataAccess.Entities.Identity.User.DefaultSuperUserId,
            };

            rdc.CedantCode = rdc.CedantCode?.Trim();
            rdc.TreatyCode = rdc.TreatyCode?.Trim();
            rdc.PolicyNumber = rdc.PolicyNumber?.Trim();
            rdc.InsuredRegisterNo = rdc.InsuredRegisterNo?.Trim();
            rdc.InsuredGenderCode = rdc.InsuredGenderCode?.Trim();
            rdc.InsuredName = rdc.InsuredName?.Trim();
            rdc.CampaignCode = rdc.CampaignCode?.Trim();
            rdc.ReinsBasisCode = rdc.ReinsBasisCode?.Trim();

            string idStr = TextFile.GetColValue(RiDataCorrectionBo.ColumnId);
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
            {
                rdc.Id = id;
            }

            return rdc;
        }

        public void UpdateData(ref RiDataCorrectionBo rdcDb, RiDataCorrectionBo rdc)
        {
            rdcDb.CedantId = rdc.CedantId;
            rdcDb.TreatyCodeId = rdc.TreatyCodeId;
            rdcDb.PolicyNumber = rdc.PolicyNumber;
            rdcDb.InsuredRegisterNo = rdc.InsuredRegisterNo;
            rdcDb.InsuredGenderCodePickListDetailId = rdc.InsuredGenderCodePickListDetailId;
            rdcDb.InsuredDateOfBirth = rdc.InsuredDateOfBirth;
            rdcDb.InsuredName = rdc.InsuredName;
            rdcDb.CampaignCode = rdc.CampaignCode;
            rdcDb.ReinsBasisCodePickListDetailId = rdc.ReinsBasisCodePickListDetailId;
            rdcDb.ApLoading = rdc.ApLoading;
            rdcDb.UpdatedById = rdc.UpdatedById;
        }

        public void Trail(Result result, RiDataCorrectionBo rdc, TrailObject trail, string action)
        {
            if (result.Valid)
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    rdc.Id,
                    string.Format("{0} RI Data Correction", action),
                    result,
                    trail,
                    rdc.UpdatedById.Value
                );
                UserTrailService.Create(ref userTrailBo);

                SetProcessCount(action);
            }
        }

        public bool ValidateDateTimeFormat(int type, ref RiDataCorrectionBo rdc, bool required = false)
        {
            string header = GetHeader(type);
            string property = GetProperty(type);

            bool valid = true;
            string value = TextFile.GetColValue(type);
            if (!string.IsNullOrEmpty(value))
            {
                if (Util.TryParseDateTime(value, out DateTime? datetime, out string error))
                {
                    rdc.SetPropertyValue(property, datetime.Value);
                }
                else
                {
                    SetProcessCount(string.Format("{0} Error", header));
                    Errors.Add(string.Format("The {0} format can not be read: {1} at row {2}", header, value, TextFile.RowIndex));
                    valid = false;
                }
            }
            else if (required)
            {
                SetProcessCount(string.Format("{0} Empty", header));
                Errors.Add(string.Format("Please enter the {0} at row {1}", header, TextFile.RowIndex));
                valid = false;
            }
            return valid;
        }

        public void AddNotFoundError(RiDataCorrectionBo rdc)
        {
            SetProcessCount("Record Not Found");
            Errors.Add(string.Format("The RI Data Correction ID doesn't exists: {0} at row {1}", rdc.Id, TextFile.RowIndex));
        }

        public string GetHeader(int col)
        {
            return Columns.Where(m => m.ColIndex == col).Select(m => m.Header).FirstOrDefault();
        }

        public string GetProperty(int col)
        {
            return Columns.Where(m => m.ColIndex == col).Select(m => m.Property).FirstOrDefault();
        }

        public List<Column> GetMappings()
        {
            Columns = RiDataCorrectionBo.GetColumns();
            return Columns;
        }
    }
}

