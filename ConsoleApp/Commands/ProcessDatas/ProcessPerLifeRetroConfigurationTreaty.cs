using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.Retrocession;
using DataAccess.Entities.Identity;
using Services;
using Services.Identity;
using Services.Retrocession;
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
    public class ProcessPerLifeRetroConfigurationTreaty : Command
    {
        public List<Column> Columns { get; set; }

        public string FilePath { get; set; }

        public HttpPostedFileBase PostedFile { get; set; }

        public int? AuthUserId { get; set; }

        public TextFile TextFile { get; set; }

        public List<string> Errors { get; set; }

        public ProcessPerLifeRetroConfigurationTreaty()
        {
            Title = "ProcessPerLifeRetroConfigurationTreaty";
            Description = "To read Per Life Retro Configuration Treaty csv file and insert into database";
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

                PerLifeRetroConfigurationTreatyBo plrct = null;
                try
                {
                    plrct = SetData();

                    if (!string.IsNullOrEmpty(plrct.TreatyCode))
                    {
                        var treatyCodeBo = TreatyCodeService.FindByCode(plrct.TreatyCode);
                        if (treatyCodeBo != null)
                        {
                            if (treatyCodeBo.Status == TreatyCodeBo.StatusInactive)
                            {
                                SetProcessCount("Treaty Code Inactive");
                                Errors.Add(string.Format("{0}: {1} at row {2}", MessageBag.TreatyCodeStatusInactive, plrct.TreatyCode, TextFile.RowIndex));
                                error = true;
                            }
                            else
                            {
                                plrct.TreatyCodeId = treatyCodeBo.Id;
                                plrct.TreatyCodeBo = treatyCodeBo;
                            }
                        }
                        else
                        {
                            SetProcessCount("Treaty Code Not Found");
                            Errors.Add(string.Format("{0} at row {1}", string.Format(MessageBag.TreatyCodeNotFound, plrct.TreatyCode), TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        plrct.TreatyCode = null;
                        SetProcessCount("Treaty Code Empty");
                        Errors.Add(string.Format("Please enter the Treaty Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(plrct.TreatyType))
                    {
                        PickListDetailBo tt = PickListDetailService.FindByPickListIdCode(PickListBo.TreatyType, plrct.TreatyType);
                        if (tt == null)
                        {
                            SetProcessCount("Treaty Type Not Found");
                            Errors.Add(string.Format("The Treaty Type doesn't exists: {0} at row {1}", plrct.TreatyType, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            plrct.TreatyTypePickListDetailId = tt.Id;
                            plrct.TreatyTypePickListDetailBo = tt;
                        }
                    }
                    else
                    {
                        SetProcessCount("Treaty Type Empty");
                        Errors.Add(string.Format("Please enter the Treaty Type at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(plrct.FundsAccountingType))
                    {
                        PickListDetailBo bt = PickListDetailService.FindByPickListIdCode(PickListBo.FundsAccountingType, plrct.FundsAccountingType);
                        if (bt == null)
                        {
                            SetProcessCount("Funds Accounting Type Not Found");
                            Errors.Add(string.Format("The Funds Accounting Type doesn't exists: {0} at row {1}", plrct.FundsAccountingType, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            plrct.FundsAccountingTypePickListDetailId = bt.Id;
                            plrct.FundsAccountingTypePickListDetailBo = bt;
                        }
                    }
                    else
                    {
                        SetProcessCount("Funds Accounting Type Empty");
                        Errors.Add(string.Format("Please enter the Funds Accounting Type at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(plrct.ReinsEffectiveStartDateStr))
                    {
                        if (!ValidateDateTimeFormat(PerLifeRetroConfigurationTreatyBo.ColumnReinsEffectiveStartDate, ref plrct))
                        {
                            error = true;
                        }
                    }
                    else
                    {
                        SetProcessCount("Reinsurance Effective Start Date Empty");
                        Errors.Add(string.Format("Please enter the Reinsurance Effective Start Date at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(plrct.ReinsEffectiveEndDateStr))
                    {
                        if (!ValidateDateTimeFormat(PerLifeRetroConfigurationTreatyBo.ColumnReinsEffectiveEndDate, ref plrct))
                        {
                            error = true;
                        }
                    }
                    else
                    {
                        SetProcessCount("Reinsurance Effective End Date Empty");
                        Errors.Add(string.Format("Please enter the Reinsurance Effective End Date at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(plrct.RiskQuarterStartDateStr))
                    {
                        if (!ValidateDateTimeFormat(PerLifeRetroConfigurationTreatyBo.ColumnRiskQuarterStartDate, ref plrct))
                        {
                            error = true;
                        }
                    }
                    else
                    {
                        SetProcessCount("Risk Quarter Start Date Empty");
                        Errors.Add(string.Format("Please enter the Risk Quarter Start Date at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(plrct.RiskQuarterEndDateStr))
                    {
                        if (!ValidateDateTimeFormat(PerLifeRetroConfigurationTreatyBo.ColumnRiskQuarterEndDate, ref plrct))
                        {
                            error = true;
                        }
                    }
                    else
                    {
                        SetProcessCount("Risk Quarter End Date Empty");
                        Errors.Add(string.Format("Please enter the Risk Quarter End Date at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    string isToAggregateStr = TextFile.GetColValue(PerLifeRetroConfigurationTreatyBo.ColumnIsToAggregate);
                    if (!string.IsNullOrEmpty(isToAggregateStr))
                    {
                        if (Util.StringToBool(isToAggregateStr, out bool b))
                        {
                            plrct.IsToAggregate = b;
                        }
                        else
                        {
                            SetProcessCount("To Aggregate Not valid");
                            Errors.Add(string.Format("The To Aggregate not in valid format (Y/N) at row {0}", TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        SetProcessCount("To Aggregate Empty");
                        Errors.Add(string.Format("Please enter the To Aggregate at row {0}", TextFile.RowIndex));
                        error = true;
                    }
                }
                catch (Exception e)
                {
                    SetProcessCount("Error");
                    Errors.Add(string.Format("Error Triggered: {0} at row {1}", e.Message, TextFile.RowIndex));
                    error = true;
                }
                string action = TextFile.GetColValue(PerLifeRetroConfigurationTreatyBo.ColumnAction);

                if (error)
                {
                    continue;
                }

                switch (action.ToLower().Trim())
                {
                    case "u":
                    case "update":

                        PerLifeRetroConfigurationTreatyBo plrctDb = PerLifeRetroConfigurationTreatyService.Find(plrct.Id);
                        if (plrctDb == null)
                        {
                            AddNotFoundError(plrct);
                            continue;
                        }

                        if (PerLifeRetroConfigurationTreatyService.IsDuplicate(PerLifeRetroConfigurationTreatyService.FormEntity(plrct)))
                        {
                            Errors.Add(string.Format("Existing Per Life Retro Treaty Combination found at row {0}", TextFile.RowIndex));
                            error = true;
                        }

                        if (error)
                        {
                            continue;
                        }

                        UpdateData(ref plrctDb, plrct);

                        trail = new TrailObject();
                        result = PerLifeRetroConfigurationTreatyService.Update(ref plrctDb, ref trail);
                        Trail(result, plrctDb, trail, "Update");

                        break;

                    case "d":
                    case "del":
                    case "delete":

                        if (plrct.Id != 0 && PerLifeRetroConfigurationTreatyService.IsExists(plrct.Id))
                        {
                            trail = new TrailObject();
                            result = PerLifeRetroConfigurationTreatyService.Delete(plrct, ref trail);
                            Trail(result, plrct, trail, "Delete");
                        }
                        else
                        {
                            AddNotFoundError(plrct);
                            continue;
                        }

                        break;

                    default:

                        if (plrct.Id != 0 && PerLifeRetroConfigurationTreatyService.IsExists(plrct.Id))
                        {
                            SetProcessCount("Record Found");
                            Errors.Add(string.Format("The Per Life Retro Configuration Treaty ID exists: {0} at row {1}", plrct.Id, TextFile.RowIndex));
                            continue;
                        }

                        if (PerLifeRetroConfigurationTreatyService.IsDuplicate(PerLifeRetroConfigurationTreatyService.FormEntity(plrct)))
                        {
                            Errors.Add(string.Format("Existing Per Life Retro Treaty Combination found at row {0}", TextFile.RowIndex));
                            error = true;
                        }

                        if (error)
                        {
                            continue;
                        }

                        trail = new TrailObject();
                        result = PerLifeRetroConfigurationTreatyService.Create(ref plrct, ref trail);
                        Trail(result, plrct, trail, "Create");

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

        public PerLifeRetroConfigurationTreatyBo SetData()
        {
            var plrct = new PerLifeRetroConfigurationTreatyBo
            {
                Id = 0,
                TreatyCode = TextFile.GetColValue(PerLifeRetroConfigurationTreatyBo.ColumnTreatyCode),
                TreatyType = TextFile.GetColValue(PerLifeRetroConfigurationTreatyBo.ColumnTreatyType),
                FundsAccountingType = TextFile.GetColValue(PerLifeRetroConfigurationTreatyBo.ColumnFundsAccountingType),
                ReinsEffectiveStartDateStr = TextFile.GetColValue(PerLifeRetroConfigurationTreatyBo.ColumnReinsEffectiveStartDate),
                ReinsEffectiveEndDateStr = TextFile.GetColValue(PerLifeRetroConfigurationTreatyBo.ColumnReinsEffectiveEndDate),
                RiskQuarterStartDateStr = TextFile.GetColValue(PerLifeRetroConfigurationTreatyBo.ColumnRiskQuarterStartDate),
                RiskQuarterEndDateStr = TextFile.GetColValue(PerLifeRetroConfigurationTreatyBo.ColumnRiskQuarterEndDate),
                Remark = TextFile.GetColValue(PerLifeRetroConfigurationTreatyBo.ColumnRemark),
                CreatedById = AuthUserId != null ? AuthUserId.Value : User.DefaultSuperUserId,
                UpdatedById = AuthUserId != null ? AuthUserId : User.DefaultSuperUserId,
            };

            plrct.Remark = plrct.Remark?.Trim();

            string idStr = TextFile.GetColValue(PerLifeRetroConfigurationTreatyBo.ColumnId);
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
            {
                plrct.Id = id;
            }

            return plrct;
        }

        public void UpdateData(ref PerLifeRetroConfigurationTreatyBo plrctDb, PerLifeRetroConfigurationTreatyBo plrct)
        {
            plrctDb.TreatyCodeId = plrct.TreatyCodeId;
            plrctDb.TreatyTypePickListDetailId = plrct.TreatyTypePickListDetailId;
            plrctDb.FundsAccountingTypePickListDetailId = plrct.FundsAccountingTypePickListDetailId;
            plrctDb.ReinsEffectiveStartDate = plrct.ReinsEffectiveStartDate;
            plrctDb.ReinsEffectiveEndDate = plrct.ReinsEffectiveEndDate;
            plrctDb.RiskQuarterStartDate = plrct.RiskQuarterStartDate;
            plrctDb.RiskQuarterEndDate = plrct.RiskQuarterEndDate;
            plrctDb.IsToAggregate = plrct.IsToAggregate;
            plrctDb.Remark = plrct.Remark;
            plrctDb.UpdatedById = plrct.UpdatedById;
        }

        public void Trail(Result result, PerLifeRetroConfigurationTreatyBo plrct, TrailObject trail, string action)
        {
            if (result.Valid)
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    plrct.Id,
                    string.Format("{0} Per Life Retro Configuration Treaty", action),
                    result,
                    trail,
                    plrct.UpdatedById.Value
                );
                UserTrailService.Create(ref userTrailBo);

                SetProcessCount(action);
            }
        }

        public bool ValidateDateTimeFormat(int type, ref PerLifeRetroConfigurationTreatyBo plrct)
        {
            string header = Columns.Where(m => m.ColIndex == type).Select(m => m.Header).FirstOrDefault();
            string property = Columns.Where(m => m.ColIndex == type).Select(m => m.Property).FirstOrDefault();

            bool valid = true;
            string value = TextFile.GetColValue(type);
            if (!string.IsNullOrEmpty(value))
            {
                if (Util.TryParseDateTime(value, out DateTime? datetime, out string error))
                {
                    plrct.SetPropertyValue(property, datetime.Value);
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

        public void AddNotFoundError(PerLifeRetroConfigurationTreatyBo plrct)
        {
            SetProcessCount("Record Not Found");
            Errors.Add(string.Format("The Per Life Retro Configuration Treaty ID doesn't exists: {0} at row {1}", plrct.Id, TextFile.RowIndex));
        }

        public List<Column> GetColumns()
        {
            Columns = PerLifeRetroConfigurationTreatyBo.GetColumns();
            return Columns;
        }
    }
}
