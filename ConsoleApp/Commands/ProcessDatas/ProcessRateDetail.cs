using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities.Identity;
using Newtonsoft.Json;
using Services;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;

namespace ConsoleApp.Commands.ProcessDatas
{
    public class ProcessRateDetail : Command
    {
        public List<Column> Columns { get; set; }

        public RateBo RateBo { get; set; }

        public RateDetailUploadBo RateDetailUploadBo { get; set; }

        public string FilePath { get; set; }

        public HttpPostedFileBase PostedFile { get; set; }

        public int? AuthUserId { get; set; }

        public TextFile TextFile { get; set; }

        public List<string> Errors { get; set; }


        public char[] charsToTrim = { ',', '.', ' ' };

        public List<int> ValuationRateFields { get; set; }

        public ProcessRateDetail()
        {
            Title = "ProcessRateDetail";
            Description = "To read Rate Detail csv file and insert into database";
            //Arguments = new string[]
            //{
            //    "filePath",
            //};
            Hide = false;
            Errors = new List<string> { };
            GetColumns();
        }

        public override bool Validate()
        {
            //string filepath = CommandInput.Arguments[0];
            //if (!File.Exists(filepath))
            //{
            //    PrintError(string.Format(MessageBag.FileNotExists, filepath));
            //    return false;
            //}
            //else
            //{
            //    FilePath = filepath;
            //}

            return base.Validate();
        }

        public override void Run()
        {
            PrintStarting();

            //Process();
            if (RateDetailUploadService.CountByStatus(RateDetailUploadBo.StatusPendingProcess) > 0)
            {
                foreach (var bo in RateDetailUploadService.GetByStatus(RateDetailUploadBo.StatusPendingProcess))
                {
                    FilePath = bo.GetLocalPath();
                    RateDetailUploadBo = bo;

                    RateBo = RateService.Find(bo.RateId);
                    ValuationRateFields = RateBo.GetFieldsByValuationRate(RateBo.ValuationRate);

                    Errors = new List<string>();
                    try
                    {
                        Process();
                    }
                    catch (Exception e)
                    {
                        if (TextFile != null)
                            TextFile.Close();

                        var message = e.Message;
                        if (e is DbEntityValidationException dbEx)
                            message = Util.CatchDbEntityValidationException(dbEx).ToString();

                        if (message.Contains("One or more errors occurred."))
                            message += "  Please refer to your system administrator for more details.";

                        Errors.Add(message);
                        foreach (string err in Errors)
                        {
                            PrintError(err);
                        }
                    }

                    if (Errors.Count > 0)
                        UpdateFileStatus(RateDetailUploadBo.StatusFailed, "Process Rate Detail File Failed");
                    else
                        UpdateFileStatus(RateDetailUploadBo.StatusSuccess, "Process Rate Detail File Success");
                }
            }

            PrintEnding();
        }

        public void Process()
        {
            UpdateFileStatus(RateDetailUploadBo.StatusProcessing, "Processing Rate Detail File");

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

            bool error = false;

            TrailObject trail;
            Result result;
            while (TextFile.GetNextRow() != null)
            {
                if (TextFile.RowIndex == 1)
                {
                    var textFileCols = TextFile.Columns;
                    var notMatchCols = Columns.Where(p => !textFileCols.Any(p2 => p2 == p.Header));
                    if (notMatchCols.Count() > 0)
                    {
                        foreach (var column in notMatchCols)
                        {
                            SetProcessCount("Column Not Found");
                            Errors.Add(string.Format("The Column Header doesn't exists: {0} at row {1}", column.Header, TextFile.RowIndex));
                            error = true;
                        }
                        break;
                    }
                    else
                    {
                        continue; // Skip header row
                    }
                }

                if (PrintCommitBuffer())
                {
                }
                SetProcessCount();

                RateDetailBo rd = null;
                try
                {
                    rd = SetData();

                    foreach (int field in ValuationRateFields)
                    {
                        var propertyName = Columns.Where(q => q.ColIndex == field).Select(q => q.Property).FirstOrDefault();
                        string value = TextFile.GetColValue(field);

                        switch (propertyName)
                        {
                            case "InsuredGenderCode":
                                rd.InsuredGenderCode = value;
                                if (!string.IsNullOrEmpty(rd.InsuredGenderCode))
                                {
                                    PickListDetailBo igc = PickListDetailService.FindByPickListIdCode(PickListBo.InsuredGenderCode, rd.InsuredGenderCode);
                                    if (igc == null)
                                    {
                                        SetProcessCount("Gender Code Not Found");
                                        Errors.Add(string.Format("The Gender Code doesn't exists: {0} at row {1}", rd.InsuredGenderCode, TextFile.RowIndex));
                                        error = true;
                                    }
                                    else
                                    {
                                        rd.InsuredGenderCodePickListDetailId = igc.Id;
                                        rd.InsuredGenderCodePickListDetailBo = igc;
                                    }
                                }
                                break;
                            case "CedingTobaccoUse":
                                rd.CedingTobaccoUse = value;
                                if (!string.IsNullOrEmpty(rd.CedingTobaccoUse))
                                {
                                    PickListDetailBo itu = PickListDetailService.FindByPickListIdCode(PickListBo.InsuredTobaccoUse, rd.CedingTobaccoUse);
                                    if (itu == null)
                                    {
                                        SetProcessCount("Smoker Code Not Found");
                                        Errors.Add(string.Format("The Smoker Code doesn't exists: {0} at row {1}", rd.CedingTobaccoUse, TextFile.RowIndex));
                                        error = true;
                                    }
                                    else
                                    {
                                        rd.CedingTobaccoUsePickListDetailId = itu.Id;
                                        rd.CedingTobaccoUsePickListDetailBo = itu;
                                    }
                                }
                                break;
                            case "CedingOccupationCode":
                                rd.CedingOccupationCode = value;
                                if (!string.IsNullOrEmpty(rd.CedingOccupationCode))
                                {
                                    PickListDetailBo ioc = PickListDetailService.FindByPickListIdCode(PickListBo.InsuredOccupationCode, rd.CedingOccupationCode);
                                    if (ioc == null)
                                    {
                                        SetProcessCount("Occupation Code Not Found");
                                        Errors.Add(string.Format("The Occupation Code doesn't exists: {0} at row {1}", rd.CedingOccupationCode, TextFile.RowIndex));
                                        error = true;
                                    }
                                    else
                                    {
                                        rd.CedingOccupationCodePickListDetailId = ioc.Id;
                                        rd.CedingOccupationCodePickListDetailBo = ioc;
                                    }
                                }
                                break;
                            case "AttainedAge":
                                if (!string.IsNullOrEmpty(value))
                                {
                                    if (int.TryParse(value, out int attainedAge))
                                    {
                                        rd.AttainedAge = attainedAge;
                                    }
                                    else
                                    {
                                        SetProcessCount("Attained Age Invalid");
                                        Errors.Add(string.Format("The Attained Age is not a numeric: {0} at row {1}", value, TextFile.RowIndex));
                                        error = true;
                                    }
                                }
                                else
                                {
                                    rd.AttainedAge = null;
                                }
                                break;
                            case "IssueAge":
                                if (!string.IsNullOrEmpty(value))
                                {
                                    if (int.TryParse(value, out int issueAge))
                                    {
                                        rd.IssueAge = issueAge;
                                    }
                                    else
                                    {
                                        SetProcessCount("Issue Age Invalid");
                                        Errors.Add(string.Format("The Issue Age is not a numeric: {0} at row {1}", value, TextFile.RowIndex));
                                        error = true;
                                    }
                                }
                                else
                                {
                                    rd.IssueAge = null;
                                }
                                break;
                            case "PolicyTerm":
                                if (!string.IsNullOrEmpty(value))
                                {
                                    if (int.TryParse(value, out int policyTerm))
                                    {
                                        rd.PolicyTerm = policyTerm;
                                    }
                                    else
                                    {
                                        SetProcessCount("Term Invalid");
                                        Errors.Add(string.Format("The Term is not a numeric: {0} at row {1}", value, TextFile.RowIndex));
                                        error = true;
                                    }
                                }
                                else
                                {
                                    rd.PolicyTerm = null;
                                }
                                break;
                            case "PolicyTermRemain":
                                if (!string.IsNullOrEmpty(value))
                                {
                                    if (int.TryParse(value, out int policyTermRemain))
                                    {
                                        rd.PolicyTermRemain = policyTermRemain;
                                    }
                                    else
                                    {
                                        SetProcessCount("Term Remain Invalid");
                                        Errors.Add(string.Format("The Term Remain is not a numeric: {0} at row {1}", value, TextFile.RowIndex));
                                        error = true;
                                    }
                                }
                                else
                                {
                                    rd.PolicyTermRemain = null;
                                }
                                break;
                        }
                    }

                    if (!string.IsNullOrEmpty(rd.RateValueStr))
                    {
                        if (Util.IsValidDouble(rd.RateValueStr, out double? d, out string _))
                        {
                            rd.RateValue = d.Value;
                        }
                        else
                        {
                            SetProcessCount("Rate Invalid");
                            Errors.Add(string.Format("The Rate is not valid double: {0} at row {1}", rd.RateValueStr, TextFile.RowIndex));
                            error = true;
                        }
                    }

                }
                catch (Exception e)
                {
                    SetProcessCount("Error");
                    Errors.Add(string.Format("Error Triggered: {0} at row {1}", e.Message, TextFile.RowIndex));
                    error = true;
                }

                string action = TextFile.GetColValue(RateDetailBo.TypeAction);

                if (error)
                {
                    continue;
                }

                switch (action.ToLower().Trim())
                {
                    case "u":
                    case "update":

                        RateDetailBo rdDb = RateDetailService.Find(rd.Id);
                        if (rdDb == null)
                        {
                            AddNotFoundError(rd);
                            continue;
                        }

                        var mappingResult = RateDetailService.ValidateMapping(rd, RateBo.Id);
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

                        UpdateData(ref rdDb, rd);

                        trail = new TrailObject();
                        result = RateDetailService.Update(ref rdDb, ref trail);
                        Trail(result, rdDb, trail, "Update");

                        break;

                    case "d":
                    case "del":
                    case "delete":

                        if (rd.Id != 0 && RateDetailService.IsExists(rd.Id))
                        {
                            trail = new TrailObject();
                            result = RateDetailService.Delete(rd, ref trail);
                            Trail(result, rd, trail, "Delete");
                        }
                        else
                        {
                            AddNotFoundError(rd);
                            continue;
                        }

                        break;

                    default:

                        if (rd.Id != 0 && RateDetailService.IsExists(rd.Id))
                        {
                            SetProcessCount("Record Found");
                            Errors.Add(string.Format("The Rate Detail ID exists: {0} at row {1}", rd.Id, TextFile.RowIndex));
                            continue;
                        }

                        mappingResult = RateDetailService.ValidateMapping(rd, RateBo.Id);
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

                        rd.RateId = RateBo.Id;

                        trail = new TrailObject();
                        result = RateDetailService.Create(ref rd, ref trail);
                        Trail(result, rd, trail, "Create");

                        break;
                }
            }

            PrintProcessCount();

            TextFile.Close();

            foreach (string err in Errors)
            {
                PrintError(err);
            }
        }

        public RateDetailBo SetData()
        {
            var rd = new RateDetailBo
            {
                Id = 0,
                InsuredGenderCode = null,
                CedingTobaccoUse = null,
                CedingOccupationCode = null,
                AttainedAge = null,
                IssueAge = null,
                PolicyTerm = null,
                PolicyTermRemain = null,
                RateValueStr = TextFile.GetColValue(RateDetailBo.TypeRateValue),
                CreatedById = AuthUserId != null ? AuthUserId.Value : DataAccess.Entities.Identity.User.DefaultSuperUserId,
                UpdatedById = AuthUserId != null ? AuthUserId : DataAccess.Entities.Identity.User.DefaultSuperUserId,
            };

            string idStr = TextFile.GetColValue(RateDetailBo.TypeRateDetailId);
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
            {
                rd.Id = id;
            }

            return rd;
        }

        public void UpdateData(ref RateDetailBo rdDb, RateDetailBo rd)
        {
            rdDb.InsuredGenderCodePickListDetailId = rd.InsuredGenderCodePickListDetailId;
            rdDb.CedingTobaccoUsePickListDetailId = rd.CedingTobaccoUsePickListDetailId;
            rdDb.CedingOccupationCodePickListDetailId = rd.CedingOccupationCodePickListDetailId;
            rdDb.AttainedAge = rd.AttainedAge;
            rdDb.IssueAge = rd.IssueAge;
            rdDb.PolicyTerm = rd.PolicyTerm;
            rdDb.PolicyTermRemain = rd.PolicyTermRemain;
            rdDb.RateValue = rd.RateValue;
        }

        public void Trail(Result result, RateDetailBo rt, TrailObject trail, string action)
        {
            if (result.Valid)
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    rt.Id,
                    string.Format("{0} Rate Detail", action),
                    result,
                    trail,
                    rt.UpdatedById.Value
                );
                UserTrailService.Create(ref userTrailBo);

                SetProcessCount(action);
            }
        }

        public void AddNotFoundError(RateDetailBo cm)
        {
            SetProcessCount("Record Not Found");
            Errors.Add(string.Format("The Rate Detail ID doesn't exists: {0} at row {1}", cm.Id, TextFile.RowIndex));
        }

        public List<Column> GetColumns()
        {
            Columns = RateDetailBo.GetColumns();
            return Columns;
        }

        public void UpdateFileStatus(int status, string description)
        {
            var rateDetailUploadBo = RateDetailUploadBo;
            rateDetailUploadBo.Status = status;

            if (Errors.Count > 0)
            {
                rateDetailUploadBo.Errors = JsonConvert.SerializeObject(Errors);
            }

            var trail = new TrailObject();
            var result = RateDetailUploadService.Update(ref rateDetailUploadBo, ref trail);

            var userTrailBo = new UserTrailBo(
                rateDetailUploadBo.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }
    }
}
