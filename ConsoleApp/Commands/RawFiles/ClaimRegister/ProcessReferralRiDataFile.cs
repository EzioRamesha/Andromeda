using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.RiDatas;
using DataAccess.Entities.Identity;
using Newtonsoft.Json;
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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.RawFiles.ClaimRegister
{
    public class ProcessReferralRiDataFile : Command
    {
        public int? ReferralRiDataFileId { get; set; }

        public ReferralRiDataFileBo ReferralRiDataFileBo { get; set; }

        public ReferralRiDataBo ReferralRiDataBo { get; set; }

        public ReferralClaimBo ReferralClaimBo { get; set; }

        public int Records { get; set; }

        public int UpdatedRecords { get; set; }

        public bool Test { get; set; } = false;

        public IProcessFile DataFile { get; set; }

        List<Column> Columns { get; set; }

        public List<Column> EditableColumns { get; set; }

        public int? ReferralIdColIndex { get; set; }

        public List<string> Errors { get; set; }

        public ModuleBo ModuleBo { get; set; }

        public Regex QuarterRegex { get; set; }

        public const string QuarterRegexPattern = @"^[0-9]{4}\s[Q]{1}[1-4]{1}$";

        public bool Success { get; set; }

        public ProcessReferralRiDataFile()
        {
            Title = "ProcessReferralRiDataFile";
            Description = "To process Claim Register File";
            Options = new string[] {
                "--t|test : Test process data",
                "--riDataBatchId= : Process by Batch Id",
            };
        }

        public override void Initial()
        {
            base.Initial();

            Test = IsOption("test");
            ReferralRiDataFileId = OptionIntegerNullable("referralRiDataFileId");
        }

        public override void Run()
        {
            if (ReferralRiDataFileId.HasValue)
            {
                ReferralRiDataFileBo = ReferralRiDataFileService.Find(ReferralRiDataFileId.Value);
                if (ReferralRiDataFileBo != null && ReferralRiDataFileBo.RawFileBo.Status != RawFileBo.StatusPending)
                {
                    Log = false;
                    PrintMessage(MessageBag.NoBatchPendingProcess);
                    return;
                }
            }
            else if (ReferralRiDataFileService.CountByStatus(RawFileBo.StatusPending) == 0)
            {
                Log = false;
                PrintMessage(MessageBag.NoBatchPendingProcess);
                return;
            }
            PrintStarting();

            ModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.ReferralClaim.ToString());
            QuarterRegex = new Regex(QuarterRegexPattern);
            GetColumns();

            while (LoadReferralRiDataFile())
            {
                if (GetProcessCount("File") > 0)
                    PrintProcessCount();
                SetProcessCount("File");
                if (!Test)
                {
                    UpdateFileStatus(RawFileBo.StatusProcessing);
                }

                string filePath = GetFilePath();
                string fileExtension = Path.GetExtension(filePath);

                switch (fileExtension)
                {
                    case ".csv":
                        DataFile = new TextFile(filePath);
                        break;
                    case ".xlsx":
                        DataFile = new Excel(filePath);
                        break;
                    default:
                        Success = false;
                        Errors.Add("Invalid File Format!");
                        break;
                }

                if (Errors.IsNullOrEmpty())
                    ProcessFile();

                if (!Test)
                {
                    ReferralRiDataFileBo.Records = Records;
                    ReferralRiDataFileBo.UpdatedRecords = UpdatedRecords;
                    if (Success)
                    {
                        SetProcessCount("Success");
                        UpdateFileStatus(RawFileBo.StatusCompleted);
                    }
                    else
                    {
                        SetProcessCount("Failed");
                        UpdateFileStatus(RawFileBo.StatusCompletedFailed);
                    }
                }
            }

            PrintProcessCount();
        }

        public void ProcessFile()
        {
            while (DataFile.GetNextRow() != null)
            {
                SetProcessCount();
                if (DataFile.GetRowIndex() == 1)
                {
                    SetHeader();
                    if (IsSuccess)
                    {
                        continue;
                    }
                    else
                    {
                        Success = false;
                        break;
                    }
                }

                Records++;
                bool hasValue = false;
                try
                {
                    ReferralRiDataBo = null;

                    ReferralRiDataBo bo = new ReferralRiDataBo()
                    {
                        ReferralRiDataFileId = ReferralRiDataFileBo.Id,
                        CreatedById = User.DefaultSuperUserId,
                        UpdatedById = User.DefaultSuperUserId,
                    };

                    bool valid = true;
                    while (DataFile.GetNextCol() != null)
                    {
                        int colIndex = DataFile.GetColIndex().Value;
                        Column column = EditableColumns.Where(q => q.ColIndex == colIndex).FirstOrDefault();

                        object rawValue = DataFile.GetValue();
                        string value = rawValue != null ? rawValue.ToString() : null;
                        string property = column.Property;

                        if (string.IsNullOrEmpty(value))
                        {
                            bo.SetPropertyValue(property, null);
                            continue;
                        }

                        hasValue = true;
                        if (colIndex == ReferralIdColIndex)
                        {
                            ReferralClaimBo = ReferralClaimService.FindByReferralId(value);

                            if (ReferralClaimBo == null)
                            {
                                Errors.Add(string.Format("Referral Claim not found with Referral ID: {0} at row {1}", value, DataFile.GetRowIndex()));
                                valid = false;
                                break;
                            }
                        }

                        int type = StandardOutputBo.GetTypeByCode(column.Header);
                        int dataType = StandardOutputBo.GetDataTypeByType(type);
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
                            case StandardOutputBo.DataTypeBoolean:
                                valid = valid ? SetBoolean(ref bo, column, value) : valid;
                                break;
                            default:
                                bo.SetPropertyValue(column.Property, value);
                                break;
                        }
                    }

                    if (!hasValue)
                        break;
                    else if (valid && ReferralClaimBo == null)
                    {
                        Errors.Add(string.Format("Referral ID is empty at row {0}", DataFile.GetRowIndex()));
                        valid = false;
                    }

                    if (!Test)
                    {
                        if (valid)
                        {
                            Create(bo);
                        }
                        else
                        {
                            Success = false;
                        }
                    }
                }
                catch (Exception e)
                {
                    Success = false;
                    Errors.Add(string.Format("{0} at row {1}", e.Message, DataFile.GetRowIndex()));
                }
            }
        }

        public void Create(ReferralRiDataBo bo)
        {
            TrailObject trail = new TrailObject();
            Result result = ReferralRiDataService.Create(ref bo, ref trail);
            if (result.Valid)
            {
                var referralClaimBo = ReferralClaimBo;
                referralClaimBo.ReferralRiDataId = bo.Id;
                referralClaimBo.RiDataWarehouseId = null;
                referralClaimBo.SumReinsured = bo.Aar;
                referralClaimBo.ClaimRecoveryAmount = bo.Aar;
                referralClaimBo.TreatyCode = bo.TreatyCode;
                referralClaimBo.ReinsBasisCode = bo.ReinsBasisCode;
                referralClaimBo.TreatyType = bo.TreatyType;
                if (bo.Aar.HasValue && bo.CurrSumAssured.HasValue)
                {
                    referralClaimBo.TreatyShare = bo.Aar.Value / bo.CurrSumAssured.Value;
                }
                //if (referralClaimBo.Status != ReferralClaimBo.StatusClosedReported)
                //{
                //    referralClaimBo.Status = ReferralClaimBo.StatusClosedReported;

                //    StatusHistoryBo statusHistoryBo = new StatusHistoryBo
                //    {
                //        ModuleId = ModuleBo.Id,
                //        ObjectId = referralClaimBo.Id,
                //        Status = referralClaimBo.Status,
                //        CreatedById = User.DefaultSuperUserId,
                //        UpdatedById = User.DefaultSuperUserId,
                //    };
                //    StatusHistoryService.Save(ref statusHistoryBo, ref trail);
                //}
                ReferralClaimService.Update(ref referralClaimBo, ref trail);

                UserTrailBo userTrailBo = new UserTrailBo(
                    bo.Id,
                    string.Format("Create Referral RI Data"),
                    result,
                    trail,
                    User.DefaultSuperUserId
                );
                UserTrailService.Create(ref userTrailBo);

                UpdatedRecords++;
            }
        }

        public void UpdateFileStatus(int status)
        {
            var rawFileBo = ReferralRiDataFileBo.RawFileBo;
            rawFileBo.Status = status;

            if (status != RawFileBo.StatusProcessing)
            {
                var referralRiDataFileBo = ReferralRiDataFileBo;
                referralRiDataFileBo.Error = Errors.IsNullOrEmpty() ? null : JsonConvert.SerializeObject(Errors);

                ReferralRiDataFileService.Update(ref referralRiDataFileBo);
            }

            RawFileService.Update(ref rawFileBo);
        }

        public bool LoadReferralRiDataFile()
        {
            Errors = new List<string>();
            Success = true;
            ReferralRiDataFileBo = null;
            Records = 0;
            UpdatedRecords = 0;
            if (ReferralRiDataFileId.HasValue)
            {
                ReferralRiDataFileBo = ReferralRiDataFileService.Find(ReferralRiDataFileId.Value);
            }
            else
            {
                ReferralRiDataFileBo = ReferralRiDataFileService.FindByStatus(RawFileBo.StatusPending);
            }

            if (ReferralRiDataFileBo == null || (ReferralRiDataFileBo != null && ReferralRiDataFileBo.RawFileBo.Status != RawFileBo.StatusPending))
            {
                return false;
            }
            return true;
        }

        public string GetFilePath()
        {
            if (ReferralRiDataFileBo != null && ReferralRiDataFileBo.RawFileBo != null)
                return ReferralRiDataFileBo.RawFileBo.GetLocalPath();
            return null;
        }

        public void SetHeader()
        {
            while (DataFile.GetNextCol() != null)
            {
                var value = DataFile.GetValue();
                if (value == null)
                    continue;

                var column = Columns.Where(c => c.Header == value).FirstOrDefault();

                if (column == null)
                    Errors.Add(string.Format("Column in header row does not exist {0}", value));
                else
                {
                    column.ColIndex = DataFile.GetColIndex();
                    if (column.Property == "ReferralId")
                    {
                        ReferralIdColIndex = DataFile.GetColIndex();
                    }
                }
            }

            if (ReferralIdColIndex == null)
            {
                Errors.Add(string.Format("Referral ID Column is required"));
            }

            EditableColumns = Columns.Where(q => q.ColIndex.HasValue).ToList();
        }

        public List<Column> GetColumns()
        {
            Columns = RiDataWarehouseBo.GetColumns(true);
            return Columns;
        }

        public bool SetDate(ref ReferralRiDataBo bo, Column column, string value)
        {
            bool valid = true;
            if (Util.TryParseDateTime(value, out DateTime? datetime, out string _))
            {
                bo.SetPropertyValue(column.Property, datetime.Value);
            }
            else
            {
                SetProcessCount(string.Format("{0}{1}", column.Header, "Error"));
                Errors.Add(string.Format("The {0} format can not be read: {1} at row {2}", column.Header, value, DataFile.GetRowIndex()));
                valid = false;
            }

            return valid;
        }

        public bool SetDouble(ref ReferralRiDataBo bo, Column column, string value)
        {
            bool valid = true;
            if (double.TryParse(value, out double r))
            {
                bo.SetPropertyValue(column.Property, r);
            }
            else
            {
                SetProcessCount(string.Format("{0}{1}", column.Header, "Error"));
                Errors.Add(string.Format("The {0} format can not be read: {1} at row {2}", column.Header, value, DataFile.GetRowIndex()));
                valid = false;
            }

            return valid;
        }

        public bool SetInteger(ref ReferralRiDataBo bo, Column column, string value)
        {
            bool valid = true;
            if (int.TryParse(value, out int r))
            {
                bo.SetPropertyValue(column.Property, r);
            }
            else
            {
                SetProcessCount(string.Format("{0}{1}", column.Header, "Error"));
                Errors.Add(string.Format("The {0} format can not be read: {1} at row {2}", column.Header, value, DataFile.GetRowIndex()));
                valid = false;
            }

            return valid;
        }

        public bool SetDropDown(ref ReferralRiDataBo bo, Column column, int type, string value)
        {
            bool valid = true;

            bo.SetPropertyValue(column.Property, value);
            PickListDetailBo pickListDetailBo = PickListDetailService.FindByStandardOutputIdCode(type, value);
            if (pickListDetailBo == null)
            {
                SetProcessCount(string.Format("{0} Not Found", column.Header));
                Errors.Add(string.Format("The {0} doesn't exists: {1} at row {2}", column.Property, value, DataFile.GetRowIndex()));
                valid = false;
            }

            return valid;
        }

        public bool SetString(ref ReferralRiDataBo bo, Column column, int type, string value)
        {
            string error = null;
            bo.SetPropertyValue(column.Property, value);
            switch (type)
            {
                case StandardOutputBo.TypeTreatyCode:
                    TreatyCodeBo treatyCodeBo = TreatyCodeService.FindByCode(value);
                    if (treatyCodeBo == null)
                    {
                        SetProcessCount("Treaty Code Not Found");
                        error = string.Format("{0} at row {1}", string.Format(MessageBag.TreatyCodeNotFound, value), DataFile.GetRowIndex());
                    }
                    else if (treatyCodeBo.Status == TreatyCodeBo.StatusInactive)
                    {
                        SetProcessCount("Treaty Code Inactive");
                        error = string.Format("{0}: {1} at row {2}", MessageBag.TreatyCodeStatusInactive, value, DataFile.GetRowIndex());
                    }
                    break;
                case StandardOutputBo.TypeMlreBenefitCode:
                    BenefitBo benefitBo = BenefitService.FindByCode(value);
                    if (benefitBo == null)
                    {
                        SetProcessCount("MLRe Benefit Code Not Found");
                        error = string.Format("{0} at row {1}", string.Format(MessageBag.BenefitCodeNotFound, value), DataFile.GetRowIndex());
                    }
                    else if (benefitBo.Status == BenefitBo.StatusInactive)
                    {
                        SetProcessCount("Benefit Inactive");
                        error = string.Format("{0}: {1} at row {2}", MessageBag.BenefitStatusInactive, value, DataFile.GetRowIndex());
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

        public bool SetBoolean(ref ReferralRiDataBo bo, Column column, string value)
        {
            bool valid = true;
            if (bool.TryParse(value, out bool b))
            {
                bo.SetPropertyValue(column.Property, b);
            }
            else
            {
                SetProcessCount(string.Format("{0}{1}", column.Header, "Error"));
                Errors.Add(string.Format("The {0} format can not be read: {1} at row {2}", column.Header, value, DataFile.GetRowIndex()));
                valid = false;
            }

            return valid;
        }



        public bool IsSuccess
        {
            get
            {
                return Errors.Count == 0;
            }
        }
    }
}
