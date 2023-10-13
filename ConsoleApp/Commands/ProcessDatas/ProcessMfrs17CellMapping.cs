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
    public class ProcessMfrs17CellMapping : Command
    {
        public List<Column> Columns { get; set; }

        public string FilePath { get; set; }

        public HttpPostedFileBase PostedFile { get; set; }

        public int? AuthUserId { get; set; }

        public TextFile TextFile { get; set; }

        public List<string> Errors { get; set; }

        public char[] charsToTrim = { ',', '.', ' ' };

        public ProcessMfrs17CellMapping()
        {
            Title = "ProcessMfrs17CellMapping";
            Description = "To read MFRS17 Cell Mapping csv file and insert into database";
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

                Mfrs17CellMappingBo cm = null;
                try
                {
                    cm = SetData();

                    if (!string.IsNullOrEmpty(cm.TreatyCode))
                    {
                        string[] treatyCodes = cm.TreatyCode.ToArraySplitTrim();
                        foreach (string treatyCodeStr in treatyCodes)
                        {
                            var treatyCodeBo = TreatyCodeService.FindByCode(treatyCodeStr);
                            if (treatyCodeBo != null)
                            {
                                if (treatyCodeBo.Status == TreatyCodeBo.StatusInactive)
                                {
                                    SetProcessCount("Treaty Code Inactive");
                                    Errors.Add(string.Format("{0}: {1} at row {2}", MessageBag.TreatyCodeStatusInactive, treatyCodeStr, TextFile.RowIndex));
                                    error = true;
                                }
                            }
                            else
                            {
                                SetProcessCount("Treaty Code Not Found");
                                Errors.Add(string.Format("{0} at row {1}", string.Format(MessageBag.TreatyCodeNotFound, treatyCodeStr), TextFile.RowIndex));
                                error = true;
                            }
                        }
                    }
                    else
                    {
                        cm.TreatyCode = null;
                        SetProcessCount("Treaty Code Empty");
                        Errors.Add(string.Format("Please enter the Treaty Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(cm.ReinsBasisCode))
                    {
                        PickListDetailBo rbc = PickListDetailService.FindByPickListIdCode(PickListBo.ReinsBasisCode, cm.ReinsBasisCode);
                        if (rbc == null)
                        {
                            SetProcessCount("Reinsurance Basic Code Not Found");
                            Errors.Add(string.Format("The Reinsurance Basic Code doesn't exists: {0} at row {1}", cm.ReinsBasisCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            cm.ReinsBasisCodePickListDetailId = rbc.Id;
                            cm.ReinsBasisCodePickListDetailBo = rbc;
                        }
                    }
                    else
                    {
                        SetProcessCount("Reinsurance Basic Code Empty");
                        Errors.Add(string.Format("Please enter the Reinsurance Basic Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    var reinsEffDatePolStartDate = TextFile.GetColValue(Mfrs17CellMappingBo.ColumnReinsEffDatePolStartDate);
                    if (!string.IsNullOrEmpty(reinsEffDatePolStartDate))
                    {
                        if (!ValidateDateTimeFormat(Mfrs17CellMappingBo.ColumnReinsEffDatePolStartDate, ref cm))
                        {
                            error = true;
                        }
                    }
                    else
                    {
                        cm.ReinsEffDatePolStartDate = null;
                    }

                    var reinsEffDatePolEndDate = TextFile.GetColValue(Mfrs17CellMappingBo.ColumnReinsEffDatePolEndDate);
                    if (!string.IsNullOrEmpty(reinsEffDatePolEndDate))
                    {
                        if (!ValidateDateTimeFormat(Mfrs17CellMappingBo.ColumnReinsEffDatePolEndDate, ref cm))
                        {
                            error = true;
                        }
                    }
                    else if (cm.ReinsEffDatePolStartDate != null)
                    {
                        cm.ReinsEffDatePolEndDate = DateTime.Parse(Util.GetDefaultEndDate());
                    }
                    else
                    {
                        cm.ReinsEffDatePolEndDate = null;
                    }

                    if (string.IsNullOrEmpty(cm.CedingPlanCode))
                    {
                        cm.CedingPlanCode = null;
                    }

                    if (!string.IsNullOrEmpty(cm.BenefitCode))
                    {
                        string[] benefitCodes = cm.BenefitCode.ToArraySplitTrim();
                        foreach (string benefitCodeStr in benefitCodes)
                        {
                            var benefitBo = BenefitService.FindByCode(benefitCodeStr);
                            if (benefitBo != null)
                            {
                                if (benefitBo.Status == BenefitBo.StatusInactive)
                                {
                                    SetProcessCount("Benefit Inactive");
                                    Errors.Add(string.Format("{0}: {1} at row {2}", MessageBag.BenefitStatusInactive, benefitCodeStr, TextFile.RowIndex));
                                    error = true;
                                }
                            }
                            else
                            {
                                SetProcessCount("Benefit Code Not Found");
                                Errors.Add(string.Format("{0} at row {1}", string.Format(MessageBag.BenefitCodeNotFound, benefitCodeStr), TextFile.RowIndex));
                                error = true;
                            }
                        }
                    }
                    else
                    {
                        cm.BenefitCode = null;
                    }

                    if (!string.IsNullOrEmpty(cm.ProfitComm))
                    {
                        PickListDetailBo pc = PickListDetailService.FindByPickListIdCode(PickListBo.ProfitComm, cm.ProfitComm);
                        if (pc == null)
                        {
                            SetProcessCount("Profit Commission Not Found");
                            Errors.Add(string.Format("The Profit Commission doesn't exists: {0} at row {1}", cm.ProfitComm, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            cm.ProfitCommPickListDetailId = pc.Id;
                            cm.ProfitCommPickListDetailBo = pc;
                        }
                    }
                    else
                    {
                        SetProcessCount("Profit Commission Empty");
                        Errors.Add(string.Format("Please enter the Profit Commission at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(cm.BasicRider))
                    {
                        PickListDetailBo br = PickListDetailService.FindByPickListIdCode(PickListBo.Mfrs17BasicRider, cm.BasicRider);
                        if (br == null)
                        {
                            SetProcessCount("MFRS17 Basic Ridder Not Found");
                            Errors.Add(string.Format("The MFRS17 Basic Ridder doesn't exists: {0} at row {1}", cm.BasicRider, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            cm.BasicRiderPickListDetailId = br.Id;
                            cm.BasicRiderPickListDetailBo = br;
                        }
                    }
                    else
                    {
                        SetProcessCount("MFRS17 Basic Ridder Empty");
                        Errors.Add(string.Format("Please enter the MFRS17 Basic Ridder at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (string.IsNullOrEmpty(cm.CellName))
                    {
                        SetProcessCount("MFRS17 Cell Name Empty");
                        Errors.Add(string.Format("Please enter the MFRS17 Cell Name at row {0}", TextFile.RowIndex));
                        error = true;
                    }
                    else
                    {
                        if (cm.CellName.Length > 50)
                        {
                            SetProcessCount("MFRS17 Cell Name exceeded max length");
                            Errors.Add(string.Format("MFRS17 Cell Name length can not be more than 50 characters at row {0}", TextFile.RowIndex));
                            error = true;
                        }
                    }
                    
                    if (string.IsNullOrEmpty(cm.Mfrs17ContractCode))
                    {
                        SetProcessCount("MFRS17 Contract Code Empty");
                        Errors.Add(string.Format("Please enter the MFRS17 Contract Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }
                    else
                    {
                        Mfrs17ContractCodeDetailBo cc = Mfrs17ContractCodeDetailService.FindByContractCode(cm.Mfrs17ContractCode);
                        if (cc == null)
                        {
                            SetProcessCount("MFRS17 Contract Code Not Found");
                            Errors.Add(string.Format("The MFRS17 Contract Code doesn't exists: {0} at row {1}", cm.Mfrs17ContractCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            cm.Mfrs17ContractCodeDetailId = cc.Id;
                            cm.Mfrs17ContractCodeDetailBo = cc;
                        }
                    }

                    if (!string.IsNullOrEmpty(cm.LoaCode))
                    {
                        if (cm.LoaCode.Length > 20)
                        {
                            SetProcessCount("LOA Code exceeded max length");
                            Errors.Add(string.Format("LOA Code length can not be more than 20 characters at row {0}", TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        cm.LoaCode = null;
                    }

                    if (!string.IsNullOrEmpty(cm.RateTable))
                    {
                        if (cm.RateTable.Length > 50)
                        {
                            SetProcessCount("Rate Table exceeded max length");
                            Errors.Add(string.Format("Rate Table length can not be more than 50 characters at row {0}", TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        cm.RateTable = null;
                    }
                }
                catch (Exception e)
                {
                    SetProcessCount("Error");
                    Errors.Add(string.Format("Error Triggered: {0} at row {1}", e.Message, TextFile.RowIndex));
                    error = true;
                }
                string action = TextFile.GetColValue(Mfrs17CellMappingBo.ColumnAction);

                if (error)
                {
                    continue;
                }

                switch (action.ToLower().Trim())
                {
                    case "u":
                    case "update":

                        Mfrs17CellMappingBo cmDb = Mfrs17CellMappingService.Find(cm.Id);
                        if (cmDb == null)
                        {
                            AddNotFoundError(cm);
                            continue;
                        }

                        var rangeResult = Mfrs17CellMappingService.ValidateRange(cm);
                        var mappingResult = Mfrs17CellMappingService.ValidateMapping(cm);
                        if (!rangeResult.Valid || !mappingResult.Valid)
                        {
                            foreach (var e in rangeResult.ToErrorArray())
                            {
                                Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                            }
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

                        UpdateData(ref cmDb, cm);

                        trail = new TrailObject();
                        result = Mfrs17CellMappingService.Update(ref cmDb, ref trail);

                        Mfrs17CellMappingService.ProcessMappingDetail(cmDb, cmDb.CreatedById); // DO NOT TRAIL
                        Trail(result, cmDb, trail, "Update");

                        break;

                    case "d":
                    case "del":
                    case "delete":

                        if (cm.Id != 0 && Mfrs17CellMappingService.IsExists(cm.Id))
                        {
                            trail = new TrailObject();
                            result = Mfrs17CellMappingService.Delete(cm, ref trail);
                            Trail(result, cm, trail, "Delete");
                        }
                        else
                        {
                            AddNotFoundError(cm);
                            continue;
                        }

                        break;

                    default:

                        if (cm.Id != 0 && Mfrs17CellMappingService.IsExists(cm.Id))
                        {
                            SetProcessCount("Record Found");
                            Errors.Add(string.Format("The MFRS17 Cell Mapping ID exists: {0} at row {1}", cm.Id, TextFile.RowIndex));
                            continue;
                        }

                        rangeResult = Mfrs17CellMappingService.ValidateRange(cm);
                        mappingResult = Mfrs17CellMappingService.ValidateMapping(cm);
                        if (!rangeResult.Valid || !mappingResult.Valid)
                        {
                            foreach (var e in rangeResult.ToErrorArray())
                            {
                                Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                            }
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
                        result = Mfrs17CellMappingService.Create(ref cm, ref trail);

                        Mfrs17CellMappingService.ProcessMappingDetail(cm, cm.CreatedById); // DO NOT TRAIL
                        Trail(result, cm, trail, "Create");

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

        public Mfrs17CellMappingBo SetData()
        {
            var cm = new Mfrs17CellMappingBo
            {
                Id = 0,
                TreatyCode = TextFile.GetColValue(Mfrs17CellMappingBo.ColumnTreatyCode),
                ReinsBasisCode = TextFile.GetColValue(Mfrs17CellMappingBo.ColumnReinsBasisCode),
                CedingPlanCode = TextFile.GetColValue(Mfrs17CellMappingBo.ColumnCedingPlanCode),
                BenefitCode = TextFile.GetColValue(Mfrs17CellMappingBo.ColumnBenefitCode),
                ProfitComm = TextFile.GetColValue(Mfrs17CellMappingBo.ColumnProfitComm),
                RateTable = TextFile.GetColValue(Mfrs17CellMappingBo.ColumnRateTable),
                BasicRider = TextFile.GetColValue(Mfrs17CellMappingBo.ColumnBasicRider),
                CellName = TextFile.GetColValue(Mfrs17CellMappingBo.ColumnCellName),
                Mfrs17ContractCode = TextFile.GetColValue(Mfrs17CellMappingBo.ColumnMfrs17ContractCode),
                LoaCode = TextFile.GetColValue(Mfrs17CellMappingBo.ColumnLoaCode),
                CreatedById = AuthUserId != null ? AuthUserId.Value : DataAccess.Entities.Identity.User.DefaultSuperUserId,
                UpdatedById = AuthUserId != null ? AuthUserId : DataAccess.Entities.Identity.User.DefaultSuperUserId,
            };

            cm.ReinsBasisCode = cm.ReinsBasisCode?.Trim();
            cm.ProfitComm = cm.ProfitComm?.Trim();
            cm.RateTable = cm.RateTable?.Trim();
            cm.BasicRider = cm.BasicRider?.Trim();
            cm.CellName = cm.CellName?.Trim();
            cm.Mfrs17ContractCode = cm.Mfrs17ContractCode?.Trim();
            cm.LoaCode = cm.LoaCode?.Trim();

            cm.TreatyCode = cm.TreatyCode?.TrimEnd(charsToTrim);
            cm.CedingPlanCode = cm.CedingPlanCode?.TrimEnd(charsToTrim);
            cm.BenefitCode = cm.BenefitCode?.TrimEnd(charsToTrim);

            string idStr = TextFile.GetColValue(Mfrs17CellMappingBo.ColumnId);
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
            {
                cm.Id = id;
            }

            return cm;
        }

        public void UpdateData(ref Mfrs17CellMappingBo cmDb, Mfrs17CellMappingBo cm)
        {
            cmDb.TreatyCode = cm.TreatyCode;
            cmDb.ReinsEffDatePolStartDate = cm.ReinsEffDatePolStartDate;
            cmDb.ReinsEffDatePolEndDate = cm.ReinsEffDatePolEndDate;
            cmDb.ReinsBasisCodePickListDetailId = cm.ReinsBasisCodePickListDetailId;
            cmDb.CedingPlanCode = cm.CedingPlanCode;
            cmDb.BenefitCode = cm.BenefitCode;
            cmDb.ProfitCommPickListDetailId = cm.ProfitCommPickListDetailId;
            cmDb.RateTable = cm.RateTable;
            cmDb.BasicRiderPickListDetailId = cm.BasicRiderPickListDetailId;
            cmDb.CellName = cm.CellName;
            cmDb.Mfrs17ContractCodeDetailId = cm.Mfrs17ContractCodeDetailId;
            cmDb.LoaCode = cm.LoaCode;
            cmDb.UpdatedById = cm.UpdatedById;
        }

        public void Trail(Result result, Mfrs17CellMappingBo cm, TrailObject trail, string action)
        {
            if (result.Valid)
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    cm.Id,
                    string.Format("{0} MFRS17 Cell Mapping", action),
                    result,
                    trail,
                    cm.UpdatedById.Value
                );
                UserTrailService.Create(ref userTrailBo);

                SetProcessCount(action);
            }
        }

        public bool ValidateDateTimeFormat(int type, ref Mfrs17CellMappingBo cm)
        {
            string header = Columns.Where(m => m.ColIndex == type).Select(m => m.Header).FirstOrDefault();
            string property = Columns.Where(m => m.ColIndex == type).Select(m => m.Property).FirstOrDefault();

            bool valid = true;
            string value = TextFile.GetColValue(type);
            if (!string.IsNullOrEmpty(value))
            {
                if (Util.TryParseDateTime(value, out DateTime? datetime, out string error))
                {
                    cm.SetPropertyValue(property, datetime.Value);
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

        public void AddNotFoundError(Mfrs17CellMappingBo cm)
        {
            SetProcessCount("Record Not Found");
            Errors.Add(string.Format("The MFRS17 Cell Mapping ID doesn't exists: {0} at row {1}", cm.Id, TextFile.RowIndex));
        }

        public List<Column> GetColumns()
        {
            Columns = Mfrs17CellMappingBo.GetColumns();
            return Columns;
        }
    }
}
