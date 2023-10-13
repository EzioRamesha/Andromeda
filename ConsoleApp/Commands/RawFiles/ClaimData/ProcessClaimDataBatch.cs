using BusinessObject;
using BusinessObject.Claims;
using BusinessObject.Identity;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Newtonsoft.Json;
using Services;
using Services.Claims;
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
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Row = Shared.ProcessFile.Row;

namespace ConsoleApp.Commands.RawFiles.ClaimData
{
    public class ProcessClaimDataBatch : Command
    {
        public bool Test { get; set; } = false;
        public int TestEndRow { get; set; } = 3;
        public int MaxEmptyRows { get; set; }
        public int Take { get; set; }
        public int DetailWidth { get; set; }
        public bool EndProcess { get; set; }
        public bool IsWithinDataRow { get; set; }

        public LogClaimDataBatch LogClaimDataBatch { get; set; }
        public LogClaimDataFile LogClaimDataFile { get; set; }
        public StatusHistoryBo ProcessingStatusHistoryBo { get; set; }

        public ClaimDataBatchBo ClaimDataBatchBo { get; set; }
        public ClaimDataFileBo ClaimDataFileBo { get; set; }
        public List<ClaimDataFileBo> ClaimDataFileBos { get; set; }

        public ClaimDataBatchStatusFileBo ClaimDataBatchStatusFileBo { get; set; }
        public ClaimDataConfigBo ClaimDataConfigBo { get; set; }
        public ClaimDataFileConfig ClaimDataFileConfigFound { get; set; }

        public List<ClaimDataMappingBo> ClaimDataMappingBos { get; set; }
        public List<ClaimDataComputationBo> ClaimDataComputationBos { get; set; }
        public List<ClaimDataValidationBo> ClaimDataPreValidationBos { get; set; }

        public CacheService CacheService { get; set; }
        public ModuleBo ModuleBo { get; set; }
        public List<Row> Rows { get; set; }
        public IProcessFile DataFile { get; set; }

        public List<ProcessRowClaimData> ProcessRowClaimData1 { get; set; }
        public List<ProcessRowClaimData> ProcessRowClaimData2 { get; set; }

        public List<ClaimDataBo> ClaimDataBo1 { get; set; }
        public List<ClaimDataBo> ClaimDataBo2 { get; set; }

        public ProcessClaimDataBatch()
        {
            Title = "ProcessClaimDataBatch";
            Description = "To process Claim Data Batch";
            Options = new string[] {
                "--t|test : Test process data",
                "--R|testMaxEndRow= : Test max end row",
            };
            DetailWidth = StandardClaimDataOutputBo.GetMaxLengthPropertyName();
        }

        public override void Initial()
        {
            base.Initial();

            Test = IsOption("test");
            TestEndRow = OptionInteger("testMaxEndRow", 3);
            MaxEmptyRows = Util.GetConfigInteger("ProcessClaimDataMaxEmptyRows", 3);
            Take = Util.GetConfigInteger("ProcessClaimDataRowRead", 50);
        }

        public override bool Validate()
        {
            try
            {
                // nothing
            }
            catch (Exception e)
            {
                PrintError(e.Message);
                return false;
            }
            return base.Validate();
        }

        public override void Run()
        {
            #region Checking for jobs with status 'Processing'
            var failedBos = ClaimDataBatchService.GetProcessingFailedByHours();
            ClaimDataBatchBo failedBo;

            if (failedBos.Count > 0)
            {
                PrintStarting();
                PrintMessage("Failing ProcessRiDataBatch stucked");
                foreach (ClaimDataBatchBo eachBo in failedBos)
                {
                    PrintMessage("Failing Id: " + eachBo.Id);
                    PrintMessage();

                    eachBo.Status = ClaimDataBatchBo.StatusFailed;

                    failedBo = eachBo;

                    ClaimDataBatchService.Update(ref failedBo);
                }
            }
            #endregion
            try
            {
                if (ClaimDataBatchService.CountByStatus(ClaimDataBatchBo.StatusSubmitForProcessing) == 0)
                {
                    Log = false;
                    PrintMessage(MessageBag.NoBatchPendingProcess);
                    return;
                }
                PrintStarting();

                CacheService = new CacheService();
                ModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.ClaimData.ToString());

                while (LoadClaimDataBatchBo() != null)
                {
                    if (GetProcessCount("Batch") > 0)
                        PrintProcessCount();
                    SetProcessCount("Batch");

                    CacheService.Load();

                    // Reset
                    SetProcessCount("Processed", 0);
                    SetProcessCount("Saved", 0);
                    SetProcessCount("File", 0);

                    PrintOutputTitle(string.Format("Process Batch Id: {0}", ClaimDataBatchBo.Id));

                    var processBatchSuccess = true;
                    LogClaimDataBatch = new LogClaimDataBatch(ClaimDataBatchBo);

                    UpdateBatchStatus(ClaimDataBatchBo.StatusProcessing, MessageBag.ProcessClaimDataBatchProcessing, true);
                    CreateStatusLogFile();
                    DeleteClaimData();

                    foreach (var file in ClaimDataFileBos)
                    {
                        LoadClaimDataFile(file);

                        LogClaimDataFile.SwFile.Start();
                        if (ProcessFile() == false)
                            processBatchSuccess = false;
                        LogClaimDataFile.SwFile.Stop();

                        LogClaimDataBatch.Add(LogClaimDataFile);
                        WriteProcessFileSummary(LogClaimDataFile);

                        if (Test)
                            break; // For testing only process one file
                    } // end of looping files

                    WriteProcessBatchSummary(LogClaimDataBatch);

                    if (processBatchSuccess)
                        UpdateBatchStatus(ClaimDataBatchBo.StatusSuccess, MessageBag.ProcessClaimDataBatchSuccess);
                    else
                        UpdateBatchStatus(ClaimDataBatchBo.StatusFailed, MessageBag.ProcessClaimDataBatchFailed);

                    if (Test)
                        break; // For testing only process one batch
                }

                PrintEnding();
            }
            catch (Exception e)
            {
                PrintError(e.Message);
            }
        }

        public ClaimDataBatchBo LoadClaimDataBatchBo()
        {
            ClaimDataBatchBo = ClaimDataBatchService.FindByStatus(ClaimDataBatchBo.StatusSubmitForProcessing);
            if (ClaimDataBatchBo != null)
            {
                ClaimDataFileBos = (List<ClaimDataFileBo>)ClaimDataFileService.GetByClaimDataBatchIdMode(ClaimDataBatchBo.Id, ClaimDataFileBo.ModeInclude);
            }
            return ClaimDataBatchBo;
        }

        public void LoadClaimDataFile(ClaimDataFileBo file)
        {
            DataFile = null;
            ClaimDataFileBo = file;
            ClaimDataFileBo.Errors = null; // clear errors
            ClaimDataFileConfigFound = ClaimDataFileBo.ClaimDataFileConfig;
            LoadClaimDataConfigDetails(ClaimDataFileBo.ClaimDataConfigId);
            LogClaimDataFile = new LogClaimDataFile(ClaimDataFileBo);
        }

        public void LoadClaimDataConfigDetails(int? claimDataConfigId)
        {
            ClaimDataConfigBo = ClaimDataConfigService.Find(claimDataConfigId);
            if (ClaimDataConfigBo != null)
            {
                ClaimDataMappingBos = (List<ClaimDataMappingBo>)ClaimDataMappingService.GetByClaimDataConfigId(ClaimDataConfigBo.Id);
                UpdateMappingBos();
                ClaimDataComputationBos = (List<ClaimDataComputationBo>)ClaimDataComputationService.GetByClaimDataConfigId(ClaimDataConfigBo.Id, ClaimDataComputationBo.StepPreComputation);
                ClaimDataPreValidationBos = (List<ClaimDataValidationBo>)ClaimDataValidationService.GetByClaimDataConfigId(ClaimDataConfigBo.Id, ClaimDataValidationBo.StepPreValidation);
            }
        }

        public void UpdateBatchStatus(int status, string description, bool setProcessingStatus = false)
        {
            ProcessingStatusHistoryBo = null;
            if (Test)
                return;

            var trail = new TrailObject();
            var statusBo = new StatusHistoryBo
            {
                ModuleId = ModuleBo.Id,
                ObjectId = ClaimDataBatchBo.Id,
                Status = status,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };
            StatusHistoryService.Create(ref statusBo, ref trail);

            var batch = ClaimDataBatchBo;
            batch.Status = status;

            using (var db = new AppDbContext(false))
            {
                ClaimDataBatchService.CountTotalFailed(ref batch, db);
            }

            var result = ClaimDataBatchService.Update(ref batch, ref trail);
            var userTrailBo = new UserTrailBo(
                ClaimDataBatchBo.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);

            if (setProcessingStatus)
                ProcessingStatusHistoryBo = statusBo;
        }

        public void CreateStatusLogFile()
        {
            if (Test)
                return;
            if (ClaimDataBatchBo == null)
                return;
            if (ProcessingStatusHistoryBo == null)
                return;

            var trail = new TrailObject();
            ClaimDataBatchStatusFileBo = new ClaimDataBatchStatusFileBo
            {
                ClaimDataBatchId = ClaimDataBatchBo.Id,
                StatusHistoryId = ProcessingStatusHistoryBo.Id,
                StatusHistoryBo = ProcessingStatusHistoryBo,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };
            var fileBo = ClaimDataBatchStatusFileBo;
            var result = ClaimDataBatchStatusFileService.Create(ref fileBo, ref trail);

            var userTrailBo = new UserTrailBo(
                fileBo.Id,
                "Create Claim Data Batch Status File",
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);

            var path = fileBo.GetFilePath();
            Util.MakeDir(path);

            if (File.Exists(path))
                File.Delete(path);
        }

        public void DeleteClaimData()
        {
            if (Test)
                return;

            // DELETE ALL CLAIM DATA BEFORE PROCESS
            PrintMessage("Deleting Claim Data records...", true, false);
            ClaimDataService.DeleteByClaimDataBatchId(ClaimDataBatchBo.Id); // DO NOT TRAIL
            PrintMessage("Deleted Claim Data records", true, false);
        }

        public void UpdateFileStatus(int status, string description)
        {
            if (Test)
                return;

            var claimDataFile = ClaimDataFileBo;
            var rawFile = ClaimDataFileBo.RawFileBo;

            claimDataFile.Status = status;
            rawFile.Status = status;

            // Update configs from ClaimDataConfig
            claimDataFile.UpdateConfigFromClaimDataConfig(ClaimDataConfigBo);

            var trail = new TrailObject();
            var result = ClaimDataFileService.Update(ref claimDataFile, ref trail);
            RawFileService.Update(ref rawFile, ref trail);

            var userTrailBo = new UserTrailBo(
                claimDataFile.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }

        public void UpdateMappingBos()
        {
            if (ClaimDataMappingBos == null)
                return;
            if (ClaimDataMappingBos.Count == 0)
                return;

            foreach (var mapping in ClaimDataMappingBos)
            {
                if (string.IsNullOrEmpty(mapping.RawColumnName))
                    continue;

                if (int.TryParse(mapping.RawColumnName, out int col))
                {
                    mapping.Col = col;
                }
                else
                {
                    mapping.RawColumnName = mapping.RawColumnName.RemoveNewLines(); // remove new lines
                    mapping.RawColumnName = mapping.RawColumnName.Trim().ToLower(); // trim and to lower case
                }
            }
        }

        public bool ProcessFile()
        {
            if (ClaimDataFileBo == null)
                return false;

            EndProcess = false;
            IsWithinDataRow = false;

            PrintOutputTitle(string.Format("Process File Id: {0}", ClaimDataFileBo.Id));

            UpdateFileStatus(ClaimDataFileBo.StatusProcessing, MessageBag.ProcessClaimDataFileProcessing);
            if (ClaimDataFileBo.Mode == ClaimDataFileBo.ModeExclude)
            {
                SetProcessCount("File Excluded");
                UpdateFileStatus(ClaimDataFileBo.StatusCompleted, MessageBag.ProcessClaimDataFileCompletedFailed);
                return true;
            }
            SetProcessCount("File");

            try
            {
                var filePath = GetFilePath();
                if (!File.Exists(filePath))
                    throw new Exception(string.Format(MessageBag.FileNotExists, filePath));

                var maxRow = Test ? TestEndRow : ClaimDataFileConfigFound.EndRow;
                var maxCol = ClaimDataFileConfigFound.EndColumn;
                switch (ClaimDataConfigBo.FileType)
                {
                    case ClaimDataConfigBo.FileTypeExcel:
                        DataFile = new Excel(GetFilePath(), worksheet: ClaimDataFileConfigFound.Worksheet, rowRead: Take)
                        {
                            MaxRow = maxRow,
                            MaxCol = maxCol,
                        };
                        break;
                    case ClaimDataConfigBo.FileTypePlainText:
                        DataFile = new TextFile(GetFilePath(), ClaimDataConfigBo.GetDelimiterChar(ClaimDataFileConfigFound.Delimiter))
                        {
                            MaxRow = maxRow,
                            MaxCol = maxCol,
                        };

                        if (ClaimDataFileConfigFound.Delimiter == ClaimDataConfigBo.DelimiterFixedLength)
                        {
                            DataFile.ResetColLengths();
                            foreach (ClaimDataMappingBo mapping in ClaimDataMappingBos)
                            {
                                if (mapping.Length == null)
                                    throw new Exception(string.Format(MessageBag.DelimiterFixedLengthIsNull, mapping.StandardClaimDataOutputBo.TypeName));
                                if (mapping.Length == 0)
                                    throw new Exception(string.Format(MessageBag.DelimiterFixedLengthIsZero, mapping.StandardClaimDataOutputBo.TypeName));
                                DataFile.AddColLengths(mapping.Length.Value);
                            }
                        }
                        break;
                }

                if (DataFile == null)
                    throw new Exception(string.Format(MessageBag.FileNotSupport, filePath));

                while (GetNextRows())
                {
                    MappingRows1(Rows); // Mapping Value
                    ProcessRows1(); // Remove Salutation, Pre Computation, Pre Validation, Save
                    AddClaimData1(); // Add the count and elapsed time
                    //SaveClaimData1(); // already saved commit in ProcessRows1()

                    PrintProcessCount();
                }

                if (DataFile != null)
                    DataFile.Close();

                // if no ClaimData created
                if (LogClaimDataFile.ClaimDataCount == 0)
                    throw new Exception(ClaimDataFileConfigFound.GetNoDataMessage());
            }
            catch (Exception e)
            {
                if (DataFile != null)
                    DataFile.Close();

                var errors = new List<string> { };
                var message = e.Message;
                if (e is DbEntityValidationException dbEx)
                    message = Util.CatchDbEntityValidationException(dbEx).ToString();
                else if (e is COMException)
                {
                    message = "Excel Data Processing Error: " + message;
                    PrintError(e.ToString());
                }

                if (e.StackTrace.Length > 0)
                {
                    PrintError(message);
                    if (e.InnerException != null)
                    {
                        if (e.InnerException is DbEntityValidationException idbEx)
                            PrintError(Util.CatchDbEntityValidationException(idbEx).ToString());
                        PrintError(e.InnerException.Message);
                    }
                    PrintError(e.StackTrace);
                }

                if (message == "One or more errors occurred.")
                    message += "  Please refer to your system administrator for more details.";

                LogClaimDataFile.FileError = message;
                errors.Add(message);

                ClaimDataFileBo.Errors = JsonConvert.SerializeObject(errors);
                UpdateFileStatus(ClaimDataFileBo.StatusCompletedFailed, MessageBag.ProcessClaimDataFileCompletedFailed);
                return false;
            }

            if (LogClaimDataFile.GetTotalErrorCount() > 0)
            {
                UpdateFileStatus(ClaimDataFileBo.StatusCompletedFailed, MessageBag.ProcessClaimDataFileCompletedFailed);
                return false;
            }

            UpdateFileStatus(ClaimDataFileBo.StatusCompleted, MessageBag.ProcessClaimDataFileCompleted);
            return true;
        }

        public bool GetNextRows()
        {
            if (EndProcess)
                return false;

            LogClaimDataFile.SwRead.Start();
            Rows = DataFile.GetNextRows(Take);
            LogClaimDataFile.SwRead.Stop();

            if (Rows.IsNullOrEmpty())
                return false;

            if (Rows.Where(q => !q.IsEmpty).Count() > 0)
                return true;

            return false;
        }

        public void ProcessHeader(Column col)
        {
            int? colIndex = col.ColIndex;
            object value = col.Value;
            //object value2 = col.Value2; // not using for now

            if (value != null)
            {
                // The RawColumnName already removed new lines, trim, and convert to lower case (refer to UpdateMappingBos() method in this class)
                // Thus, the value from cell or text file should remove the new lines, trim, and convert to lower case
                var header = value.ToString().Trim().ToLower().RemoveNewLines();

                // It is allow to map single column for multiple mapping fields
                // Thus, we should get all mappings using ToList() instead of one mapping using FirstOrDefault()
                var mappings = ClaimDataMappingBos.Where(q => q.RawColumnName == header).ToList();
                if (mappings != null && mappings.Count > 0)
                {
                    foreach (var mapping in mappings)
                    {
                        mapping.Col = colIndex;
                    }
                }
                else
                {
                    // Add custom field header
                    ClaimDataMappingBos.Add(ClaimDataMappingService.GetCustomFieldMapping(value.ToString(), colIndex));
                }
            }
        }

        public List<MappingClaimData> MappingRows(List<Row> rows)
        {
            if (rows.IsNullOrEmpty())
                return null;

            var mappingDataRows = new List<MappingClaimData> { };
            int countRowEmpty = 0;
            foreach (Row row in rows)
            {
                if (row.IsEnd)
                    continue;

                SetProcessCount();

                if (row.IsEmpty)
                {
                    if (IsWithinDataRow)
                    {
                        countRowEmpty++;
                        if (countRowEmpty >= MaxEmptyRows)
                        {
                            EndProcess = true;
                            break;
                        }
                    }
                }
                else
                {
                    countRowEmpty = 0;

                    int rowIndex = row.RowIndex;
                    int rowType = ClaimDataFileBo.ClaimDataFileConfig.DefineRowType(rowIndex);

                    switch (rowType)
                    {
                        case ClaimDataFileConfig.ROW_TYPE_HEADER:
                            foreach (var column in row.Columns)
                                ProcessHeader(column);
                            break;
                        case ClaimDataFileConfig.ROW_TYPE_DATA:
                            IsWithinDataRow = true;
                            mappingDataRows.Add(new MappingClaimData(this, row));
                            break;
                    }
                }
            }

            if (mappingDataRows.Count > 0)
            {
                Parallel.ForEach(mappingDataRows, mappingDataRow => mappingDataRow.MappingRow());
            }

            return mappingDataRows;
        }

        public void MappingRows1(List<Row> rows)
        {
            LogClaimDataFile.SwMapping.Start();

            ProcessRowClaimData1 = new List<ProcessRowClaimData> { };
            var mappingDataRows = MappingRows(rows);
            if (!mappingDataRows.IsNullOrEmpty())
            {
                foreach (var mappingDataRow in mappingDataRows)
                {
                    LogClaimDataFile.AddCount(mappingDataRow.LogClaimDataFile);
                    LogClaimDataFile.AddElapsedDetails(mappingDataRow.LogClaimDataFile);
                    ProcessRowClaimData1.Add(new ProcessRowClaimData(this, mappingDataRow));
                }
            }

            LogClaimDataFile.SwMapping.Stop();
        }

        public void MappingRows2(List<Row> rows)
        {
            LogClaimDataFile.SwMapping.Start();

            ProcessRowClaimData2 = new List<ProcessRowClaimData> { };
            var mappingDataRows = MappingRows(rows);
            if (!mappingDataRows.IsNullOrEmpty())
            {
                foreach (var mappingDataRow in mappingDataRows)
                {
                    LogClaimDataFile.AddCount(mappingDataRow.LogClaimDataFile);
                    LogClaimDataFile.AddElapsedDetails(mappingDataRow.LogClaimDataFile);
                    ProcessRowClaimData2.Add(new ProcessRowClaimData(this, mappingDataRow));
                }
            }

            LogClaimDataFile.SwMapping.Stop();
        }

        public void ProcessRows1()
        {
            LogClaimDataFile.SwProcess.Start();
            Parallel.ForEach(ProcessRowClaimData1, row => row.Process());
            LogClaimDataFile.SwProcess.Stop();
        }

        public void ProcessRows2()
        {
            LogClaimDataFile.SwProcess.Start();
            Parallel.ForEach(ProcessRowClaimData2, row => row.Process());
            LogClaimDataFile.SwProcess.Stop();
        }

        public List<ClaimDataBo> AddClaimData(List<ProcessRowClaimData> processRowClaimDatas)
        {
            if (processRowClaimDatas.IsNullOrEmpty())
                return null;

            var claimDataBos = new List<ClaimDataBo> { };
            foreach (var processedRow in processRowClaimDatas)
            {
                if (processedRow.ClaimDataBos.IsNullOrEmpty())
                    continue;

                // already save committed in parallel process
                //claimDataBos.AddRange(processedRow.ClaimDataBos);

                SetProcessCount("Saved", processedRow.ClaimDataBos.Count, true);

                if (!processedRow.OutputLines.IsNullOrEmpty())
                {
                    foreach (var lines in processedRow.OutputLines)
                    {
                        PrintMessageOnly(lines);
                    }
                }

                if (processedRow.LogClaimDataFile != null)
                {
                    LogClaimDataFile.AddCount(processedRow.LogClaimDataFile);
                    LogClaimDataFile.AddElapsedDetails(processedRow.LogClaimDataFile);
                }
            }

            return claimDataBos;
        }

        public void AddClaimData1()
        {
            var claimDataBos = AddClaimData(ProcessRowClaimData1);
            if (!claimDataBos.IsNullOrEmpty())
                ClaimDataBo1.AddRange(claimDataBos);
        }

        public void AddClaimData2()
        {
            var claimDataBos = AddClaimData(ProcessRowClaimData2);
            if (!claimDataBos.IsNullOrEmpty())
                ClaimDataBo2.AddRange(claimDataBos);
        }

        public void SaveClaimData(List<ClaimDataBo> claimDataBos)
        {
            if (claimDataBos.IsNullOrEmpty())
                return;

            using (var db = new AppDbContext())
            {
                var transaction = db.Database.BeginTransaction();
                //var trail = new TrailObject();
                //var result = null;
                foreach (var claimDataBo in claimDataBos)
                {
                    if (IsCommitBuffer("Saved"))
                    {
                        db.SaveChanges();
                        transaction.Commit();

                        transaction = db.Database.BeginTransaction();
                    }

                    var claimData = claimDataBo;
                    claimData.ClaimDataBatchId = ClaimDataFileBo.ClaimDataBatchId;
                    claimData.ClaimDataFileId = ClaimDataFileBo.Id;
                    claimData.CreatedById = ClaimDataFileBo.CreatedById;
                    claimData.UpdatedById = ClaimDataFileBo.UpdatedById;
                    ClaimDataService.Create(ref claimData, db);
                    //result = ClaimDataService.Create(ref claimData, ref trail);

                    SetProcessCount("Saved");
                }
                db.SaveChanges();
                transaction.Commit();

                transaction.Dispose();

                /*
                UserTrailBo userTrailBo = new UserTrailBo(
                    ClaimDataFileBo.Id,
                    "Create Claim Data",
                    result,
                    trail,
                    ClaimDataFileBo.CreatedById,
                    ignoreNull: true // ClaimData too many columns so json format should ignore the null columns
                );
                UserTrailService.Create(ref userTrailBo);
                */
            }
        }

        public void SaveClaimData1()
        {
            if (Test)
                return;
            if (ClaimDataBo1.IsNullOrEmpty())
                return;

            LogClaimDataFile.SwSave.Start();

            SaveClaimData(ClaimDataBo1);

            ClaimDataBo1 = new List<ClaimDataBo> { }; // reset for next bulk records
            LogClaimDataFile.SwSave.Stop();
        }

        public void SaveClaimData2()
        {
            if (Test)
                return;
            if (ClaimDataBo2.IsNullOrEmpty())
                return;

            LogClaimDataFile.SwSave.Start();

            SaveClaimData(ClaimDataBo2);

            ClaimDataBo2 = new List<ClaimDataBo> { }; // reset for next bulk records
            LogClaimDataFile.SwSave.Stop();
        }

        public string GetFilePath()
        {
            if (ClaimDataFileBo != null && ClaimDataFileBo.RawFileBo != null)
                return ClaimDataFileBo.RawFileBo.GetLocalPath();
            return null;
        }

        public void WriteProcessBatchSummary(LogClaimDataBatch summary, bool print = false)
        {
            if (summary == null)
                return;
            if (print)
            {
                foreach (var line in summary.GetDetails())
                    PrintMessage(line);
                PrintMessage();
            }
            if (ClaimDataBatchStatusFileBo == null)
                return;

            string path = ClaimDataBatchStatusFileBo.GetFilePath();
            if (string.IsNullOrEmpty(path))
                return;

            if (summary != null)
            {
                using (var summaryLogFile = new TextFile(path, true, true))
                {
                    foreach (var line in summary.GetDetails())
                        summaryLogFile.WriteLine(line);
                    summaryLogFile.WriteLine("");
                }
            }
        }

        public void WriteProcessFileSummary(LogClaimDataFile summary, bool print = false)
        {
            if (summary == null)
                return;
            if (print)
            {
                foreach (var line in summary.GetDetails())
                    PrintMessage(line);
                PrintMessage();
            }
            if (ClaimDataBatchStatusFileBo == null)
                return;

            string path = ClaimDataBatchStatusFileBo.GetFilePath();
            if (string.IsNullOrEmpty(path))
                return;

            if (summary != null)
            {
                using (var summaryLogFile = new TextFile(path, true, true))
                {
                    foreach (var line in summary.GetDetails())
                        summaryLogFile.WriteLine(line);
                    summaryLogFile.WriteLine("");
                }
            }
        }

        public string FormatClaimDataByType(int type, ClaimDataBo claimDataBo)
        {
            string property = StandardClaimDataOutputBo.GetPropertyNameByType(type);
            return Util.FormatDetail(property, claimDataBo.GetPropertyValue(property));
        }

        public void PrintClaimData(ClaimDataBo claimDataBo, int index = 1)
        {
            PrintDetail("Errors", claimDataBo.Errors);
            PrintDetail("CustomField", claimDataBo.CustomField);

            PrintMessage();

            PrintClaimDataByTypes(new List<int>
            {
                StandardClaimDataOutputBo.TypeTreatyCode,
                StandardClaimDataOutputBo.TypePolicyNumber,
                StandardClaimDataOutputBo.TypeInsuredName,
                StandardClaimDataOutputBo.TypeInsuredGenderCode,
                StandardClaimDataOutputBo.TypeInsuredDateOfBirth,
                StandardClaimDataOutputBo.TypeMlreBenefitCode,
                StandardClaimDataOutputBo.TypeReinsEffDatePol,
                StandardClaimDataOutputBo.TypeReinsBasisCode,
                StandardClaimDataOutputBo.TypeCedantDateOfNotification,
                StandardClaimDataOutputBo.TypeCurrencyCode,
                StandardClaimDataOutputBo.TypeCurrencyRate,
            }, claimDataBo);

            PrintMessage();
        }

        public void PrintClaimDataByType(int type, ClaimDataBo claimDataBo)
        {
            PrintMessage(FormatClaimDataByType(type, claimDataBo));
        }

        public void PrintClaimDataByTypes(List<int> types, ClaimDataBo claimDataBo)
        {
            foreach (int type in types)
            {
                PrintClaimDataByType(type, claimDataBo);
            }
        }
    }
}