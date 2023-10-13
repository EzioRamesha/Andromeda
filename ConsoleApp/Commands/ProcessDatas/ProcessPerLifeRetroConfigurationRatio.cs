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
    public class ProcessPerLifeRetroConfigurationRatio : Command
    {
        public List<Column> Columns { get; set; }

        public string FilePath { get; set; }

        public HttpPostedFileBase PostedFile { get; set; }

        public int? AuthUserId { get; set; }

        public TextFile TextFile { get; set; }

        public List<string> Errors { get; set; }

        public ProcessPerLifeRetroConfigurationRatio()
        {
            Title = "ProcessPerLifeRetroConfigurationRatio";
            Description = "To read Per Life Retro Configuration Ratio csv file and insert into database";
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

                PerLifeRetroConfigurationRatioBo plrcr = null;
                try
                {
                    plrcr = SetData();

                    if (!string.IsNullOrEmpty(plrcr.TreatyCode))
                    {
                        var treatyCodeBo = TreatyCodeService.FindByCode(plrcr.TreatyCode);
                        if (treatyCodeBo != null)
                        {
                            if (treatyCodeBo.Status == TreatyCodeBo.StatusInactive)
                            {
                                SetProcessCount("Treaty Code Inactive");
                                Errors.Add(string.Format("{0}: {1} at row {2}", MessageBag.TreatyCodeStatusInactive, plrcr.TreatyCode, TextFile.RowIndex));
                                error = true;
                            }
                            else
                            {
                                plrcr.TreatyCodeId = treatyCodeBo.Id;
                                plrcr.TreatyCodeBo = treatyCodeBo;
                            }
                        }
                        else
                        {
                            SetProcessCount("Treaty Code Not Found");
                            Errors.Add(string.Format("{0} at row {1}", string.Format(MessageBag.TreatyCodeNotFound, plrcr.TreatyCode), TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        plrcr.TreatyCode = null;
                        SetProcessCount("Treaty Code Empty");
                        Errors.Add(string.Format("Please enter the Treaty Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    var retroRatioStr = TextFile.GetColValue(PerLifeRetroConfigurationRatioBo.ColumnRetroRatio);
                    if (!string.IsNullOrEmpty(retroRatioStr))
                    {
                        if (Util.IsValidDouble(retroRatioStr, out double? output, out string _))
                        {
                            plrcr.RetroRatio = output.Value;
                        }
                        else
                        {
                            SetProcessCount("Retro Ratio Invalid");
                            Errors.Add(string.Format("The Retro Ratio is invalid: {0} at row {1}", retroRatioStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        SetProcessCount("Retro Ratio Empty");
                        Errors.Add(string.Format("Please enter the Retro Ratio at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    var mlreRetainRatioStr = TextFile.GetColValue(PerLifeRetroConfigurationRatioBo.ColumnMlreRetainRatio);
                    if (!string.IsNullOrEmpty(mlreRetainRatioStr))
                    {
                        if (Util.IsValidDouble(mlreRetainRatioStr, out double? output, out string _))
                        {
                            plrcr.MlreRetainRatio = output.Value;
                        }
                        else
                        {
                            SetProcessCount("MLRe Retain Ratio Invalid");
                            Errors.Add(string.Format("The MLRe Retain Ratio is invalid: {0} at row {1}", mlreRetainRatioStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        SetProcessCount("MLRe Retain Ratio Empty");
                        Errors.Add(string.Format("Please enter the MLRe Retain Ratio at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    var ruleValueStr = TextFile.GetColValue(PerLifeRetroConfigurationRatioBo.ColumnRuleValue);
                    if (!string.IsNullOrEmpty(ruleValueStr))
                    {
                        if (Util.IsValidDouble(ruleValueStr, out double? output, out string _))
                        {
                            plrcr.RuleValue = output.Value;
                        }
                        else
                        {
                            SetProcessCount("Rule Value Invalid");
                            Errors.Add(string.Format("The Rule Value is invalid: {0} at row {1}", ruleValueStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        SetProcessCount("Rule Value Empty");
                        Errors.Add(string.Format("Please enter the Rule Value at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(plrcr.ReinsEffectiveStartDateStr))
                    {
                        if (!ValidateDateTimeFormat(PerLifeRetroConfigurationRatioBo.ColumnReinsEffectiveStartDate, ref plrcr))
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

                    if (!string.IsNullOrEmpty(plrcr.ReinsEffectiveEndDateStr))
                    {
                        if (!ValidateDateTimeFormat(PerLifeRetroConfigurationRatioBo.ColumnReinsEffectiveEndDate, ref plrcr))
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

                    if (!string.IsNullOrEmpty(plrcr.RiskQuarterStartDateStr))
                    {
                        if (!ValidateDateTimeFormat(PerLifeRetroConfigurationRatioBo.ColumnRiskQuarterStartDate, ref plrcr))
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

                    if (!string.IsNullOrEmpty(plrcr.RiskQuarterEndDateStr))
                    {
                        if (!ValidateDateTimeFormat(PerLifeRetroConfigurationRatioBo.ColumnRiskQuarterEndDate, ref plrcr))
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

                    if (!string.IsNullOrEmpty(plrcr.RuleEffectiveDateStr))
                    {
                        if (!ValidateDateTimeFormat(PerLifeRetroConfigurationRatioBo.ColumnRuleEffectiveDate, ref plrcr))
                        {
                            error = true;
                        }
                    }
                    else
                    {
                        SetProcessCount("Rule Effective Date Empty");
                        Errors.Add(string.Format("Please enter the Rule Effective Date at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(plrcr.RuleCeaseDateStr))
                    {
                        if (!ValidateDateTimeFormat(PerLifeRetroConfigurationRatioBo.ColumnRuleCeaseDate, ref plrcr))
                        {
                            error = true;
                        }
                    }
                    else
                    {
                        SetProcessCount("Rule Cease Date Empty");
                        Errors.Add(string.Format("Please enter the Rule Cease Date at row {0}", TextFile.RowIndex));
                        error = true;
                    }
                }
                catch (Exception e)
                {
                    SetProcessCount("Error");
                    Errors.Add(string.Format("Error Triggered: {0} at row {1}", e.Message, TextFile.RowIndex));
                    error = true;
                }
                string action = TextFile.GetColValue(PerLifeRetroConfigurationRatioBo.ColumnAction);

                if (error)
                {
                    continue;
                }

                switch (action.ToLower().Trim())
                {
                    case "u":
                    case "update":

                        PerLifeRetroConfigurationRatioBo plrcrDb = PerLifeRetroConfigurationRatioService.Find(plrcr.Id);
                        if (plrcrDb == null)
                        {
                            AddNotFoundError(plrcr);
                            continue;
                        }

                        if (PerLifeRetroConfigurationRatioService.IsDuplicate(PerLifeRetroConfigurationRatioService.FormEntity(plrcr)))
                        {
                            Errors.Add(string.Format("Existing Per Life Retro Ratio Combination found at row {0}", TextFile.RowIndex));
                            error = true;
                        }

                        if (error)
                        {
                            continue;
                        }

                        UpdateData(ref plrcrDb, plrcr);

                        trail = new TrailObject();
                        result = PerLifeRetroConfigurationRatioService.Update(ref plrcrDb, ref trail);
                        Trail(result, plrcrDb, trail, "Update");

                        break;

                    case "d":
                    case "del":
                    case "delete":

                        if (plrcr.Id != 0 && PerLifeRetroConfigurationRatioService.IsExists(plrcr.Id))
                        {
                            trail = new TrailObject();
                            result = PerLifeRetroConfigurationRatioService.Delete(plrcr, ref trail);
                            Trail(result, plrcr, trail, "Delete");
                        }
                        else
                        {
                            AddNotFoundError(plrcr);
                            continue;
                        }

                        break;

                    default:

                        if (plrcr.Id != 0 && PerLifeRetroConfigurationRatioService.IsExists(plrcr.Id))
                        {
                            SetProcessCount("Record Found");
                            Errors.Add(string.Format("The Per Life Retro Configuration Ratio ID exists: {0} at row {1}", plrcr.Id, TextFile.RowIndex));
                            continue;
                        }

                        if (PerLifeRetroConfigurationRatioService.IsDuplicate(PerLifeRetroConfigurationRatioService.FormEntity(plrcr)))
                        {
                            Errors.Add(string.Format("Existing Per Life Retro Ratio Combination found at row {0}", TextFile.RowIndex));
                            error = true;
                        }

                        if (error)
                        {
                            continue;
                        }

                        trail = new TrailObject();
                        result = PerLifeRetroConfigurationRatioService.Create(ref plrcr, ref trail);
                        Trail(result, plrcr, trail, "Create");

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

        public PerLifeRetroConfigurationRatioBo SetData()
        {
            var plrcr = new PerLifeRetroConfigurationRatioBo
            {
                Id = 0,
                TreatyCode = TextFile.GetColValue(PerLifeRetroConfigurationRatioBo.ColumnTreatyCode),
                ReinsEffectiveStartDateStr = TextFile.GetColValue(PerLifeRetroConfigurationRatioBo.ColumnReinsEffectiveStartDate),
                ReinsEffectiveEndDateStr = TextFile.GetColValue(PerLifeRetroConfigurationRatioBo.ColumnReinsEffectiveEndDate),
                RiskQuarterStartDateStr = TextFile.GetColValue(PerLifeRetroConfigurationRatioBo.ColumnRiskQuarterStartDate),
                RiskQuarterEndDateStr = TextFile.GetColValue(PerLifeRetroConfigurationRatioBo.ColumnRiskQuarterEndDate),
                RuleEffectiveDateStr = TextFile.GetColValue(PerLifeRetroConfigurationRatioBo.ColumnRuleEffectiveDate),
                RuleCeaseDateStr = TextFile.GetColValue(PerLifeRetroConfigurationRatioBo.ColumnRuleCeaseDate),
                Description = TextFile.GetColValue(PerLifeRetroConfigurationRatioBo.ColumnDescription),
                CreatedById = AuthUserId != null ? AuthUserId.Value : User.DefaultSuperUserId,
                UpdatedById = AuthUserId != null ? AuthUserId : User.DefaultSuperUserId,
            };

            plrcr.Description = plrcr.Description?.Trim();

            string idStr = TextFile.GetColValue(PerLifeRetroConfigurationRatioBo.ColumnId);
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
            {
                plrcr.Id = id;
            }

            return plrcr;
        }

        public void UpdateData(ref PerLifeRetroConfigurationRatioBo plrcrDb, PerLifeRetroConfigurationRatioBo plrcr)
        {
            plrcrDb.TreatyCodeId = plrcr.TreatyCodeId;
            plrcrDb.RetroRatio = plrcr.RetroRatio;
            plrcrDb.MlreRetainRatio = plrcr.MlreRetainRatio;
            plrcrDb.RuleValue = plrcr.RuleValue;
            plrcrDb.ReinsEffectiveStartDate = plrcr.ReinsEffectiveStartDate;
            plrcrDb.ReinsEffectiveEndDate = plrcr.ReinsEffectiveEndDate;
            plrcrDb.RiskQuarterStartDate = plrcr.RiskQuarterStartDate;
            plrcrDb.RiskQuarterEndDate = plrcr.RiskQuarterEndDate;
            plrcrDb.RuleEffectiveDate = plrcr.RuleEffectiveDate;
            plrcrDb.RuleCeaseDate = plrcr.RuleCeaseDate;
            plrcrDb.Description = plrcr.Description;
            plrcrDb.UpdatedById = plrcr.UpdatedById;
        }

        public void Trail(Result result, PerLifeRetroConfigurationRatioBo plrcr, TrailObject trail, string action)
        {
            if (result.Valid)
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    plrcr.Id,
                    string.Format("{0} Per Life Retro Configuration Ratio", action),
                    result,
                    trail,
                    plrcr.UpdatedById.Value
                );
                UserTrailService.Create(ref userTrailBo);

                SetProcessCount(action);
            }
        }

        public bool ValidateDateTimeFormat(int type, ref PerLifeRetroConfigurationRatioBo plrcr)
        {
            string header = Columns.Where(m => m.ColIndex == type).Select(m => m.Header).FirstOrDefault();
            string property = Columns.Where(m => m.ColIndex == type).Select(m => m.Property).FirstOrDefault();

            bool valid = true;
            string value = TextFile.GetColValue(type);
            if (!string.IsNullOrEmpty(value))
            {
                if (Util.TryParseDateTime(value, out DateTime? datetime, out string error))
                {
                    plrcr.SetPropertyValue(property, datetime.Value);
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

        public void AddNotFoundError(PerLifeRetroConfigurationRatioBo plrcr)
        {
            SetProcessCount("Record Not Found");
            Errors.Add(string.Format("The Per Life Retro Configuration Ratio ID doesn't exists: {0} at row {1}", plrcr.Id, TextFile.RowIndex));
        }

        public List<Column> GetColumns()
        {
            Columns = PerLifeRetroConfigurationRatioBo.GetColumns();
            return Columns;
        }
    }
}
