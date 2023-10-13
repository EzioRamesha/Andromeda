using BusinessObject;
using BusinessObject.Claims;
using BusinessObject.Identity;
using BusinessObject.RiDatas;
using BusinessObject.SoaDatas;
using ConsoleApp.Commands.ProcessDatas.Exports;
using ConsoleApp.Commands.RawFiles.SoaData.Query;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Newtonsoft.Json;
using Services;
using Services.Claims;
using Services.Identity;
using Services.RiDatas;
using Services.SoaDatas;
using Shared;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Row = Shared.ProcessFile.Row;

namespace ConsoleApp.Commands.RawFiles.SoaData
{
    public class ProcessSoaDataBatch : Command
    {
        public bool Test { get; set; }
        public bool Template { get; set; }
        public int TestEndRow { get; set; }
        public int MaxEmptyRows { get; set; }
        public string FilePath { get; set; }
        public int CedantId { get; set; }
        public int TreatyId { get; set; }
        public int Take { get; set; }
        public int DetailWidth { get; set; }

        public CedantBo CedantBo { get; set; }
        public TreatyBo TreatyBo { get; set; }
        public PickListDetailBo CurrencyCodePickListDetailBo { get; set; }
        public SoaDataBatchBo SoaDataBatchBo { get; set; }
        public SoaDataFileBo SoaDataFileBo { get; set; }
        public List<SoaDataFileBo> SoaDataFileBos { get; set; }
        public StatusHistoryBo ProcessingStatusHistoryBo { get; set; }
        public List<ProcessRowSoaData> ProcessRowSoaData1 { get; set; }
        public List<SoaDataCompiledSummaryBo> SoaDataCompiledSummaryBos { get; set; }

        public int RiDataBatchId { get; set; }
        public RiDataBatchBo RiDataBatchBo { get; set; }
        public int ClaimDataBatchId { get; set; }
        public ClaimDataBatchBo ClaimDataBatchBo { get; set; }

        public LogSoaDataBatch LogSoaDataBatch { get; set; }
        public LogSoaDataFile LogSoaDataFile { get; set; }

        public CacheService CacheService { get; set; }
        public ModuleBo ModuleBo { get; set; }
        public List<Row> Rows { get; set; }
        public List<Column> Columns { get; set; }
        public IProcessFile DataFile { get; set; }
        public bool ProcessedHeaderRow { get; set; }

        public ProcessSoaDataBatch()
        {
            Title = "ProcessSoaDataBatch";
            Description = "To process SOA Data Batch";
            Options = new string[] {
                "--t|test : Test process data",
                "--template : Generate the template file",
                "--testMaxEndRow= : Test max end row",
                "--filePath= : Enter the file path",
                "--cedantId= : Enter the Cedant ID",
                "--treatyId= : Enter the Treaty ID",
            };
            DetailWidth = StandardSoaDataOutputBo.GetMaxLengthPropertyName();
        }

        public override void Initial()
        {
            base.Initial();

            Test = IsOption("test");
            Template = IsOption("template");
            TestEndRow = OptionInteger("testMaxEndRow", 3);
            MaxEmptyRows = Util.GetConfigInteger("ProcessSoaDataMaxEmptyRows", 3);
            Take = Util.GetConfigInteger("ProcessSoaDataRowRead", 50);

            FilePath = Option("filePath");
            CedantId = OptionInteger("cedantId");
            TreatyId = OptionInteger("treatyId");

            CacheService = new CacheService();
        }

        public override bool Validate()
        {
            try
            {
                if (!string.IsNullOrEmpty(FilePath))
                    if (!File.Exists(FilePath))
                        throw new Exception(string.Format(MessageBag.FileNotExists, FilePath));

                if (CedantId != 0)
                {
                    CedantBo = CedantService.Find(CedantId);
                    if (CedantBo == null)
                        throw new Exception(string.Format(MessageBag.NoRecordFoundWithName, "Cedant"));
                }

                if (TreatyId != 0)
                {
                    TreatyBo = TreatyService.Find(TreatyId);
                    if (TreatyBo == null)
                        throw new Exception(string.Format(MessageBag.NoRecordFoundWithName, "Treaty"));
                }
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
            var failedBos = SoaDataBatchService.GetProcessingFailedByHours();
            SoaDataBatchBo failedBo;

            if (failedBos.Count > 0)
            {
                PrintStarting();
                PrintMessage("Failing ProcessSoaDataBatch stucked");
                foreach (SoaDataBatchBo eachBo in failedBos)
                {
                    PrintMessage("Failing Id: " + eachBo.Id);
                    PrintMessage();

                    eachBo.Status = SoaDataBatchBo.StatusFailed;

                    failedBo = eachBo;

                    SoaDataBatchService.Update(ref failedBo);
                }
            }
            #endregion

            if (Template)
            {
                FilePath = GenerateTemplate();
                PrintMessageOnly(string.Format("File Path: {0}", FilePath));
                return;
            }
            if (!string.IsNullOrEmpty(FilePath))
            {
                ProcessTestFile();
                return;
            }

            try
            {
                if (CutOffService.IsCutOffProcessing())
                {
                    Log = false;
                    PrintMessage(MessageBag.ProcessCannotRunDueToCutOff, true, false);
                    return;
                }
                if (SoaDataBatchService.CountByStatus(SoaDataBatchBo.StatusSubmitForProcessing) == 0)
                {
                    Log = false;
                    PrintMessage(MessageBag.NoBatchPendingProcess);
                    return;
                }
                PrintStarting();

                ModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.SoaData.ToString());

                // Checking for empty value for Match RI Data/Claim Data cause by been replaced by different Id
                var emptyBos = SoaDataBatchService.FindEmptyIdByStatusAutoCreate(SoaDataBatchBo.StatusSubmitForProcessing);
                if (!emptyBos.IsNullOrEmpty())
                {
                    //Log = false;
                    foreach (var bo in emptyBos)
                    {
                        SoaDataBatchBo = bo;
                        UpdateBatchStatus(SoaDataBatchBo.StatusFailed, MessageBag.ProcessSoaDataBatchFailed);
                        if (bo.IsRiDataAutoCreate && !bo.RiDataBatchId.HasValue)
                            PrintMessage("The Match RI Data value is null!");
                        else if (bo.IsClaimDataAutoCreate && !bo.ClaimDataBatchId.HasValue)
                            PrintMessage("The Match Claim Data value is null!");
                    }
                }

                var notExistsBos = SoaDataBatchService.FindNotExistIdByStatusAutoCreate(SoaDataBatchBo.StatusSubmitForProcessing);
                if (!notExistsBos.IsNullOrEmpty())
                {
                    //Log = false;
                    foreach (var bo in notExistsBos)
                    {
                        SoaDataBatchBo = bo;
                        // Checking for Match Id not exists in the table (status Deleted)
                        if (bo.IsRiDataAutoCreate && bo.RiDataBatchId.HasValue && bo.RiDataBatchBo == null)
                        {
                            UpdateBatchStatus(SoaDataBatchBo.StatusFailed, MessageBag.ProcessSoaDataBatchFailed);
                            PrintMessage(string.Format(MessageBag.NotExistsWithValue, "Match RI Data", bo.RiDataBatchId));
                        }
                        else if (bo.IsClaimDataAutoCreate && bo.ClaimDataBatchId.HasValue && bo.ClaimDataBatchBo == null)
                        {
                            UpdateBatchStatus(SoaDataBatchBo.StatusFailed, MessageBag.ProcessSoaDataBatchFailed);
                            PrintMessage(string.Format(MessageBag.NotExistsWithValue, "Match Claim Data", bo.ClaimDataBatchId));
                        }

                        // Checking for Match Id In SoaDataBatch different from Match Id in RiData/ClaimData
                        if (bo.IsRiDataAutoCreate && bo.RiDataBatchId.HasValue && (bo.RiDataBatchBo != null && bo.RiDataBatchBo.SoaDataBatchId != bo.Id))
                        {
                            UpdateBatchStatus(SoaDataBatchBo.StatusFailed, MessageBag.ProcessSoaDataBatchFailed);
                            PrintMessage("This Match RI Data value does not match that found in RI Data record");
                        }
                        else if (bo.IsClaimDataAutoCreate && bo.ClaimDataBatchId.HasValue && (bo.ClaimDataBatchBo != null && bo.ClaimDataBatchBo.SoaDataBatchId != bo.Id))
                        {
                            UpdateBatchStatus(SoaDataBatchBo.StatusFailed, MessageBag.ProcessSoaDataBatchFailed);
                            PrintMessage("This Match Claim Data value does not match that found in RI Data record");
                        }
                    }
                }

                while (LoadSoaDataBatchBo() != null)
                {
                    if (GetProcessCount("Batch") > 0)
                        PrintProcessCount();
                    SetProcessCount("Batch");

                    var processBatchSuccess = true;
                    CacheService.Load();
                    CacheService.LoadForSoaData(SoaDataBatchBo.CedantId);
                    LogSoaDataBatch = new LogSoaDataBatch(SoaDataBatchBo);

                    UpdateBatchStatus(SoaDataBatchBo.StatusProcessing, MessageBag.ProcessSoaDataBatchProcessing, true);
                    DeleteSoaData();
                    DeleteSoaDataCompiledSummary();

                    if (SoaDataBatchBo.IsRiDataAutoCreate == true && SoaDataFileBos.IsNullOrEmpty())
                    {
                        // Auto Created SOA: to auto generate amount for SOA Data which will be populate from RI Data
                        CreateSoaDataByRiData();
                    }
                    else if (SoaDataBatchBo.IsClaimDataAutoCreate == true && SoaDataFileBos.IsNullOrEmpty())
                    {
                        // Auto Created SOA: to auto generate amount for SOA Data which will be populate from Claim Data
                        if (GetRiDataBatchId() == 0)
                            CreateSoaDataByClaimData();
                        else
                            CreateSoaDataByRiData();
                    }
                    else
                    {
                        foreach (var file in SoaDataFileBos)
                        {
                            LoadSoaDataFile(file);

                            LogSoaDataFile.SwFile.Start();
                            if (ProcessFile() == false)
                                processBatchSuccess = false;
                            LogSoaDataFile.SwFile.Stop();

                            LogSoaDataBatch.Add(LogSoaDataFile);

                            if (Test)
                                break; // For testing only process one file
                        } // end of looping files
                    }

                    CreateCompiledSummary();
                    CreateCompiledSummaryIFRS17();
                    CreateCompiledSummaryByCellNameIFRS17();

                    if (processBatchSuccess)
                        UpdateBatchStatus(SoaDataBatchBo.StatusSuccess, MessageBag.ProcessSoaDataBatchSuccess);
                    else
                        UpdateBatchStatus(SoaDataBatchBo.StatusFailed, MessageBag.ProcessSoaDataBatchFailed);

                    if (Test)
                        break; // For testing only process one batch
                } // end of looping batches

                PrintEnding();
            }
            catch (Exception e)
            {
                PrintError(e.Message);
            }
        }

        public void ProcessTestFile()
        {
            DataFile = null;
            SoaDataBatchBo = new SoaDataBatchBo();
            SoaDataFileBo = new SoaDataFileBo();
            LogSoaDataFile = new LogSoaDataFile(SoaDataFileBo);

            CacheService.LoadForSoaData(CedantId);

            Test = true;

            ProcessFile();
        }

        public SoaDataBatchBo LoadSoaDataBatchBo()
        {
            if (CutOffService.IsCutOffProcessing())
                return null;

            SoaDataBatchBo = SoaDataBatchService.FindByStatus(SoaDataBatchBo.StatusSubmitForProcessing);            
            LoadSoaDataBatchForeignBo();
            return SoaDataBatchBo;
        }

        public void LoadSoaDataBatchForeignBo()
        {
            if (SoaDataBatchBo != null)
            {
                SoaDataFileBos = (List<SoaDataFileBo>)SoaDataFileService.GetBySoaDataBatchIdMode(SoaDataBatchBo.Id, SoaDataFileBo.ModeInclude);
                CedantId = SoaDataBatchBo.CedantId;
                CedantBo = SoaDataBatchBo.CedantBo;
                TreatyId = SoaDataBatchBo.TreatyId.Value;
                TreatyBo = SoaDataBatchBo.TreatyBo;
                if (SoaDataBatchBo.RiDataBatchId.HasValue)
                    RiDataBatchBo = RiDataBatchService.Find(SoaDataBatchBo.RiDataBatchId.Value);
                if (SoaDataBatchBo.ClaimDataBatchId.HasValue)
                    ClaimDataBatchBo = ClaimDataBatchService.Find(SoaDataBatchBo.ClaimDataBatchId.Value);
                if (SoaDataBatchBo.CurrencyCodePickListDetailId.HasValue)
                    CurrencyCodePickListDetailBo = PickListDetailService.Find(SoaDataBatchBo.CurrencyCodePickListDetailId);
            }
        }

        public void LoadSoaDataFile(SoaDataFileBo file)
        {
            DataFile = null;
            SoaDataFileBo = file;
            SoaDataFileBo.Errors = null; // clear errors
            LogSoaDataFile = new LogSoaDataFile(SoaDataFileBo);
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
                ObjectId = SoaDataBatchBo.Id,
                Status = status,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };
            StatusHistoryService.Create(ref statusBo, ref trail);

            var batch = SoaDataBatchBo;
            batch.Status = status;

            using (var db = new AppDbContext(false))
            {
                SoaDataBatchService.CountTotalFailed(ref batch, db);
            }

            var result = SoaDataBatchService.Update(ref batch, ref trail);
            var userTrailBo = new UserTrailBo(
                SoaDataBatchBo.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);

            if (setProcessingStatus)
                ProcessingStatusHistoryBo = statusBo;
        }

        public void DeleteSoaData()
        {
            if (Test)
                return;

            // DELETE ALL SOA DATA BEFORE PROCESS
            PrintMessage("Deleting SOA Data records...", true, false);
            SoaDataService.DeleteBySoaDataBatchId(SoaDataBatchBo.Id); // DO NOT TRAIL
            PrintMessage("Deleted SOA Data records", true, false);
        }

        public void DeleteSoaDataCompiledSummary()
        {
            if (Test)
                return;

            // DELETE ALL SOA DATA COMPILED SUMMARY BEFORE PROCESS
            PrintMessage("Deleting SOA Data Compiled records...", true, false);
            SoaDataCompiledSummaryService.DeleteBySoaDataBatchId(SoaDataBatchBo.Id); // DO NOT TRAIL
            PrintMessage("Deleted SOA Data Compiled records", true, false);
        }

        public void UpdateFileStatus(int status, string description)
        {
            if (Test)
                return;

            var soaDataFile = SoaDataFileBo;
            var rawFile = SoaDataFileBo.RawFileBo;

            soaDataFile.Status = status;
            rawFile.Status = status;

            var trail = new TrailObject();
            var result = SoaDataFileService.Update(ref soaDataFile, ref trail);
            RawFileService.Update(ref rawFile, ref trail);

            var userTrailBo = new UserTrailBo(
                soaDataFile.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }

        public bool ProcessFile()
        {
            Columns = SoaDataBo.GetColumns();
            foreach (var column in Columns)
            {
                column.Header = column.Header.Trim().ToLower().RemoveNewLines();
            }
            if (SoaDataFileBo == null)
                return false;

            UpdateFileStatus(SoaDataFileBo.StatusProcessing, MessageBag.ProcessSoaDataFileProcessing);
            if (SoaDataFileBo.Mode == SoaDataFileBo.ModeExclude)
            {
                SetProcessCount("File Excluded");
                UpdateFileStatus(SoaDataFileBo.StatusCompleted, MessageBag.ProcessSoaDataFileCompletedFailed);
                return true;
            }
            SetProcessCount("File");

            try
            {
                var filePath = GetFilePath();
                if (!File.Exists(filePath))
                    throw new Exception(string.Format(MessageBag.FileNotExists, filePath));

                DataFile = new Excel(filePath, worksheet: 1, rowRead: Take) { };

                while (GetNextRows())
                {
                    MappingRows1(Rows); // Mapping Value
                    SaveSoaDataBo1(); // Save into database
                    //PrintSoaDataBo1(); // Print the data for testing purpose

                    PrintProcessCount();

                    //Added update for UpdatedAt for fail checking
                    var boToUpdate = SoaDataBatchBo;
                    SoaDataBatchService.Update(ref boToUpdate);
                }

                if (DataFile != null)
                    DataFile.Close();
            }
            catch (Exception e)
            {
                if (DataFile != null)
                    DataFile.Close();

                var errors = new List<string> { };
                var message = e.Message;
                if (e is DbEntityValidationException dbEx)
                    message = Util.CatchDbEntityValidationException(dbEx).ToString();

                LogSoaDataFile.FileError = message;
                errors.Add(message);

                SoaDataFileBo.Errors = JsonConvert.SerializeObject(errors);
                UpdateFileStatus(SoaDataFileBo.StatusCompletedFailed, MessageBag.ProcessSoaDataFileCompletedFailed);
                return false;
            }

            if (LogSoaDataFile.GetTotalErrorCount() > 0)
            {
                UpdateFileStatus(SoaDataFileBo.StatusCompletedFailed, MessageBag.ProcessSoaDataFileCompletedFailed);
                return false;
            }

            UpdateFileStatus(SoaDataFileBo.StatusCompleted, MessageBag.ProcessSoaDataFileCompleted);
            return true;
        }

        public List<MappingSoaData> MappingRows(List<Row> rows)
        {
            if (rows.IsNullOrEmpty())
                return null;

            var mappingDataRows = new List<MappingSoaData> { };
            int countRowEmpty = 0;
            foreach (Row row in rows)
            {
                if (row.IsEnd)
                    continue;

                SetProcessCount();

                if (row.IsEmpty)
                {
                    countRowEmpty++;
                    if (countRowEmpty >= MaxEmptyRows)
                    {
                        break;
                    }
                }
                else
                {
                    int rowIndex = row.RowIndex;
                    int rowType = MappingSoaData.ROW_TYPE_DATA;
                    if (rowIndex == 1)
                        rowType = MappingSoaData.ROW_TYPE_HEADER;

                    switch (rowType)
                    {
                        case MappingSoaData.ROW_TYPE_HEADER:
                            foreach (var column in row.Columns)
                                ProcessHeader(column);
                            break;
                        case MappingSoaData.ROW_TYPE_DATA:
                            mappingDataRows.Add(new MappingSoaData(this, row));
                            break;
                    }
                }
            }

            if (!mappingDataRows.IsNullOrEmpty())
            {
                Parallel.ForEach(mappingDataRows, mappingDataRow => mappingDataRow.MappingRow());
            }

            return mappingDataRows;
        }

        public void MappingRows1(List<Row> rows)
        {
            LogSoaDataFile.SwMapping.Start();

            ProcessRowSoaData1 = new List<ProcessRowSoaData> { };
            var mappingDataRows = MappingRows(rows);
            if (!mappingDataRows.IsNullOrEmpty())
            {
                foreach (var mappingDataRow in mappingDataRows)
                {
                    LogSoaDataFile.AddCount(mappingDataRow.LogSoaDataFile);
                    LogSoaDataFile.AddElapsedDetails(mappingDataRow.LogSoaDataFile);
                    ProcessRowSoaData1.Add(new ProcessRowSoaData(this, mappingDataRow));
                }
            }

            LogSoaDataFile.SwMapping.Stop();
        }

        public void SaveSoaDataBo1()
        {
            LogSoaDataFile.SwSave.Start();
            if (!ProcessRowSoaData1.IsNullOrEmpty())
                Parallel.ForEach(ProcessRowSoaData1, q => q.Save());
            LogSoaDataFile.SwSave.Stop();
        }

        public void CreateCompiledSummary()
        {
            if (SoaDataBatchBo == null)
                return;

            SoaDataCompiledSummaryBos = new List<SoaDataCompiledSummaryBo> { };
            var compiledSummaries = new List<SoaDataCompiledSummaryBo> { };
            using (var db = new AppDbContext(false))
            {
                var businessOriginCode = CacheService.BusinessOrigins.Where(q => q.Id == TreatyBo?.BusinessOriginPickListDetailId).Select(q => q.Code).FirstOrDefault();
                if (string.IsNullOrEmpty(businessOriginCode))
                    return;

                var riSummaryGroups = QueryRiSummaryGroupBy(db);
                var claimDataGroups = QueryClaimDataSumGroupBy(db);

                var riSummaryEndDateGroups = QueryRiSummaryGroupByEndDate(db);

                var soaDataGroups = db.SoaData
                    .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                    .Where(q => q.BusinessCode == businessOriginCode)
                    .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate })
                    .Select(q => new SoaDataGroupBy
                    {
                        TreatyCode = q.Key.TreatyCode,
                        RiskQuarter = q.Key.RiskQuarter,
                        CurrencyCode = q.Key.CurrencyCode,
                        CurrencyRate = q.Key.CurrencyRate,

                        TotalDiscount = q.Sum(d => d.TotalDiscount),
                        RiskPremium = q.Sum(d => d.RiskPremium),
                        Levy = q.Sum(d => d.Levy),
                        ProfitComm = q.Sum(d => d.ProfitComm),
                        ModcoReserveIncome = q.Sum(d => d.ModcoReserveIncome),
                        RiDeposit = q.Sum(d => d.RiDeposit),
                        AdministrationContribution = q.Sum(d => d.AdministrationContribution),
                        ShareOfRiCommissionFromCompulsoryCession = q.Sum(d => d.ShareOfRiCommissionFromCompulsoryCession),
                        RecaptureFee = q.Sum(d => d.RecaptureFee),
                        CreditCardCharges = q.Sum(d => d.CreditCardCharges),
                    })
                    .OrderBy(q => q.TreatyCode)
                    .ToList();

                // Risk Qtr shall be populate based on risk period in RI Data but when there is only Claim Data in SOA, Risk Qtr will follow SOA Qtr
                if (GetRiDataBatchId() != 0)
                {
                    foreach (var riSummaryGroup in riSummaryGroups)
                    {
                        var compiledSummary = new SoaDataCompiledSummaryBo
                        {
                            SoaDataBatchId = SoaDataBatchBo.Id,
                            ReportingType = SoaDataCompiledSummaryBo.ReportingTypeIFRS4,

                            InvoiceDate = DateTime.Now,
                            StatementReceivedDate = SoaDataBatchBo.StatementReceivedAt,

                            TreatyCode = riSummaryGroup.TreatyCode,
                            RiskQuarter = riSummaryGroup.RiskQuarter,
                            SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                            BusinessCode = businessOriginCode,
                            Frequency = riSummaryGroup.Frequency,

                            CurrencyCode = riSummaryGroup.CurrencyCode,
                            CurrencyRate = riSummaryGroup.CurrencyRate,

                            NbPremium = Util.RoundNullableValue(riSummaryGroup.NbPremium.GetValueOrDefault(), 2),
                            RnPremium = Util.RoundNullableValue(riSummaryGroup.RnPremium.GetValueOrDefault(), 2),
                            AltPremium = Util.RoundNullableValue(riSummaryGroup.AltPremium.GetValueOrDefault(), 2),

                            NbCession = riSummaryGroup.NbCession.GetValueOrDefault(),
                            RnCession = riSummaryGroup.RnCession.GetValueOrDefault(),
                            AltCession = riSummaryGroup.AltCession.GetValueOrDefault(),

                            DTH = Util.RoundNullableValue(riSummaryGroup.DTH.GetValueOrDefault(), 2),
                            TPA = Util.RoundNullableValue(riSummaryGroup.TPA.GetValueOrDefault(), 2),
                            TPS = Util.RoundNullableValue(riSummaryGroup.TPS.GetValueOrDefault(), 2),
                            PPD = Util.RoundNullableValue(riSummaryGroup.PPD.GetValueOrDefault(), 2),
                            CCA = Util.RoundNullableValue(riSummaryGroup.CCA.GetValueOrDefault(), 2),
                            CCS = Util.RoundNullableValue(riSummaryGroup.CCS.GetValueOrDefault(), 2),
                            PA = Util.RoundNullableValue(riSummaryGroup.PA.GetValueOrDefault(), 2),
                            HS = Util.RoundNullableValue(riSummaryGroup.HS.GetValueOrDefault(), 2),

                            NoClaimBonus = Util.RoundNullableValue(riSummaryGroup.NoClaimBonus.GetValueOrDefault(), 2),
                            SurrenderValue = Util.RoundNullableValue(riSummaryGroup.SurrenderValue.GetValueOrDefault(), 2),
                            DatabaseCommission = Util.RoundNullableValue(riSummaryGroup.DatabaseCommission.GetValueOrDefault(), 2),
                            BrokerageFee = Util.RoundNullableValue(riSummaryGroup.BrokerageFee.GetValueOrDefault(), 2),
                            ServiceFee = Util.RoundNullableValue(riSummaryGroup.ServiceFee.GetValueOrDefault(), 2),
                        };
                        compiledSummary.GetSstPayable();
                        compiledSummary.GetTotalAmount();
                        compiledSummary.GetTotalPremium();

                        // NB/RN/ALT SAR shall populate if it is monthly mode
                        // other than monthly mode, will take sum AAR
                        if (compiledSummary.Frequency == PickListDetailBo.PremiumFrequencyCodeMonthly)
                        {
                            if (!riSummaryEndDateGroups.IsNullOrEmpty())
                            {
                                var riSummaryEndDateGroup = riSummaryEndDateGroups?.Where(q => q.TreatyCode == riSummaryGroup.TreatyCode && q.RiskQuarter == riSummaryGroup.RiskQuarter && q.CurrencyCode == riSummaryGroup.CurrencyCode && q.CurrencyRate == riSummaryGroup.CurrencyRate).FirstOrDefault();
                                if (riSummaryEndDateGroup != null)
                                {
                                    compiledSummary.NbDiscount = Util.RoundNullableValue(riSummaryEndDateGroup.NbDiscount.GetValueOrDefault(), 2);
                                    compiledSummary.RnDiscount = Util.RoundNullableValue(riSummaryEndDateGroup.RnDiscount.GetValueOrDefault(), 2);
                                    compiledSummary.AltDiscount = Util.RoundNullableValue(riSummaryEndDateGroup.AltDiscount.GetValueOrDefault(), 2);

                                    compiledSummary.NbSar = Util.RoundNullableValue(riSummaryEndDateGroup.NbSar.GetValueOrDefault(), 2);
                                    compiledSummary.RnSar = Util.RoundNullableValue(riSummaryEndDateGroup.RnSar.GetValueOrDefault(), 2);
                                    compiledSummary.AltSar = Util.RoundNullableValue(riSummaryEndDateGroup.AltSar.GetValueOrDefault(), 2);
                                }
                            }
                        }                        
                        else
                        {
                            if (riSummaryGroup != null)
                            {
                                compiledSummary.NbDiscount = Util.RoundNullableValue(riSummaryGroup.NbDiscount.GetValueOrDefault(), 2);
                                compiledSummary.RnDiscount = Util.RoundNullableValue(riSummaryGroup.RnDiscount.GetValueOrDefault(), 2);
                                compiledSummary.AltDiscount = Util.RoundNullableValue(riSummaryGroup.AltDiscount.GetValueOrDefault(), 2);

                                compiledSummary.NbSar = Util.RoundNullableValue(riSummaryGroup.NbSar.GetValueOrDefault(), 2);
                                compiledSummary.RnSar = Util.RoundNullableValue(riSummaryGroup.RnSar.GetValueOrDefault(), 2);
                                compiledSummary.AltSar = Util.RoundNullableValue(riSummaryGroup.AltSar.GetValueOrDefault(), 2);
                            }
                        }

                        var soaDataGroup = soaDataGroups?.Where(q => q.TreatyCode == riSummaryGroup.TreatyCode && q.RiskQuarter == riSummaryGroup.RiskQuarter && q.CurrencyCode == riSummaryGroup.CurrencyCode && q.CurrencyRate == riSummaryGroup.CurrencyRate).FirstOrDefault();
                        if (soaDataGroup != null)
                        {
                            compiledSummary.TotalDiscount = Util.RoundNullableValue(soaDataGroup.TotalDiscount.GetValueOrDefault(), 2);
                            compiledSummary.RiskPremium = Util.RoundNullableValue(soaDataGroup.RiskPremium.GetValueOrDefault(), 2);
                            compiledSummary.Levy = Util.RoundNullableValue(soaDataGroup.Levy.GetValueOrDefault(), 2);
                            compiledSummary.ProfitComm = Util.RoundNullableValue(soaDataGroup.ProfitComm.GetValueOrDefault(), 2);
                            compiledSummary.ModcoReserveIncome = Util.RoundNullableValue(soaDataGroup.ModcoReserveIncome.GetValueOrDefault(), 2);
                            compiledSummary.RiDeposit = Util.RoundNullableValue(soaDataGroup.RiDeposit.GetValueOrDefault(), 2);
                            compiledSummary.AdministrationContribution = Util.RoundNullableValue(soaDataGroup.AdministrationContribution.GetValueOrDefault(), 2);
                            compiledSummary.ShareOfRiCommissionFromCompulsoryCession = Util.RoundNullableValue(soaDataGroup.ShareOfRiCommissionFromCompulsoryCession.GetValueOrDefault(), 2);
                            compiledSummary.RecaptureFee = Util.RoundNullableValue(soaDataGroup.RecaptureFee.GetValueOrDefault(), 2);
                            compiledSummary.CreditCardCharges = Util.RoundNullableValue(soaDataGroup.CreditCardCharges.GetValueOrDefault(), 2);
                            compiledSummary.CurrencyCode = soaDataGroup.CurrencyCode;
                            compiledSummary.CurrencyRate = soaDataGroup.CurrencyRate.GetValueOrDefault();
                        }                        

                        if (compiledSummary.RiskQuarter == compiledSummary.SoaQuarter)
                        {
                            var claimDataGroup = claimDataGroups?.Where(q => q.TreatyCode == riSummaryGroup.TreatyCode && q.SoaQuarter == SoaDataBatchBo.Quarter).FirstOrDefault();
                            if (claimDataGroup != null)
                            {
                                if (compiledSummary.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ClaimRecoveryAmt.GetValueOrDefault(), 2);
                                else
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ForeignClaimRecoveryAmt.GetValueOrDefault(), 2);
                            }
                        }

                        switch (businessOriginCode)
                        {
                            case PickListDetailBo.BusinessOriginCodeWithinMalaysia:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeWM;
                                break;
                            case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeOM;
                                break;
                            case PickListDetailBo.BusinessOriginCodeServiceFee:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeSFWM;
                                break;
                        }

                        compiledSummary.GetNetTotalAmount();
                        if (compiledSummary.RiskQuarter != compiledSummary.SoaQuarter)
                        {
                            switch (businessOriginCode)
                            {
                                case PickListDetailBo.BusinessOriginCodeWithinMalaysia:
                                    if (compiledSummary.NetTotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNWM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNWM;
                                    break;
                                case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                                    if (compiledSummary.NetTotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNOM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNOM;
                                    break;
                                case PickListDetailBo.BusinessOriginCodeServiceFee:
                                    if (compiledSummary.TotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNSFWM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNSFWM;
                                    break;
                            }
                        }
                        
                        if (compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeWM && compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeOM)
                            compiledSummary.Amount1 = compiledSummary.NetTotalAmount;

                        compiledSummaries.Add(compiledSummary);
                    }
                }
                else
                {
                    foreach (var soaDataGroup in soaDataGroups)
                    {
                        var compiledSummary = new SoaDataCompiledSummaryBo
                        {
                            SoaDataBatchId = SoaDataBatchBo.Id,
                            ReportingType = SoaDataCompiledSummaryBo.ReportingTypeIFRS4,

                            InvoiceDate = DateTime.Now,
                            StatementReceivedDate = SoaDataBatchBo.StatementReceivedAt,

                            TreatyCode = soaDataGroup.TreatyCode,
                            RiskQuarter = soaDataGroup.RiskQuarter,
                            SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                            BusinessCode = businessOriginCode,

                            TotalDiscount = Util.RoundNullableValue(soaDataGroup.TotalDiscount.GetValueOrDefault(), 2),
                            RiskPremium = Util.RoundNullableValue(soaDataGroup.RiskPremium.GetValueOrDefault(), 2),
                            Levy = Util.RoundNullableValue(soaDataGroup.Levy.GetValueOrDefault(), 2),
                            ProfitComm = Util.RoundNullableValue(soaDataGroup.ProfitComm.GetValueOrDefault(), 2),
                            ModcoReserveIncome = Util.RoundNullableValue(soaDataGroup.ModcoReserveIncome.GetValueOrDefault(), 2),
                            RiDeposit = Util.RoundNullableValue(soaDataGroup.RiDeposit.GetValueOrDefault(), 2),
                            AdministrationContribution = Util.RoundNullableValue(soaDataGroup.AdministrationContribution.GetValueOrDefault(), 2),
                            ShareOfRiCommissionFromCompulsoryCession = Util.RoundNullableValue(soaDataGroup.ShareOfRiCommissionFromCompulsoryCession.GetValueOrDefault(), 2),
                            RecaptureFee = Util.RoundNullableValue(soaDataGroup.RecaptureFee.GetValueOrDefault(), 2),
                            CreditCardCharges = Util.RoundNullableValue(soaDataGroup.CreditCardCharges.GetValueOrDefault(), 2),

                            CurrencyCode = soaDataGroup.CurrencyCode,
                            CurrencyRate = soaDataGroup.CurrencyRate.GetValueOrDefault(),
                        };

                        var riSummaryGroup = riSummaryGroups?.Where(q => q.TreatyCode == soaDataGroup.TreatyCode && q.RiskQuarter == compiledSummary.RiskQuarter && q.CurrencyCode == compiledSummary.CurrencyCode && q.CurrencyRate == compiledSummary.CurrencyRate && q.Frequency == compiledSummary.Frequency).FirstOrDefault();
                        if (riSummaryGroup != null)
                        {
                            compiledSummary.NbPremium = Util.RoundNullableValue(riSummaryGroup.NbPremium.GetValueOrDefault(), 2);
                            compiledSummary.RnPremium = Util.RoundNullableValue(riSummaryGroup.RnPremium.GetValueOrDefault(), 2);
                            compiledSummary.AltPremium = Util.RoundNullableValue(riSummaryGroup.AltPremium.GetValueOrDefault(), 2);

                            compiledSummary.NbDiscount = Util.RoundNullableValue(riSummaryGroup.NbDiscount.GetValueOrDefault(), 2);
                            compiledSummary.RnDiscount = Util.RoundNullableValue(riSummaryGroup.RnDiscount.GetValueOrDefault(), 2);
                            compiledSummary.AltDiscount = Util.RoundNullableValue(riSummaryGroup.AltDiscount.GetValueOrDefault(), 2);

                            compiledSummary.DTH = Util.RoundNullableValue(riSummaryGroup.DTH.GetValueOrDefault(), 2);
                            compiledSummary.TPA = Util.RoundNullableValue(riSummaryGroup.TPA.GetValueOrDefault(), 2);
                            compiledSummary.TPS = Util.RoundNullableValue(riSummaryGroup.TPS.GetValueOrDefault(), 2);
                            compiledSummary.PPD = Util.RoundNullableValue(riSummaryGroup.PPD.GetValueOrDefault(), 2);
                            compiledSummary.CCA = Util.RoundNullableValue(riSummaryGroup.CCA.GetValueOrDefault(), 2);
                            compiledSummary.CCS = Util.RoundNullableValue(riSummaryGroup.CCS.GetValueOrDefault(), 2);
                            compiledSummary.PA = Util.RoundNullableValue(riSummaryGroup.PA.GetValueOrDefault(), 2);
                            compiledSummary.HS = Util.RoundNullableValue(riSummaryGroup.HS.GetValueOrDefault(), 2);

                            compiledSummary.NoClaimBonus = Util.RoundNullableValue(riSummaryGroup.NoClaimBonus.GetValueOrDefault(), 2);
                            compiledSummary.SurrenderValue = Util.RoundNullableValue(riSummaryGroup.SurrenderValue.GetValueOrDefault(), 2);
                            compiledSummary.DatabaseCommission = Util.RoundNullableValue(riSummaryGroup.DatabaseCommission.GetValueOrDefault(), 2);
                            compiledSummary.BrokerageFee = Util.RoundNullableValue(riSummaryGroup.BrokerageFee.GetValueOrDefault(), 2);
                            compiledSummary.ServiceFee = Util.RoundNullableValue(riSummaryGroup.ServiceFee.GetValueOrDefault(), 2);

                            compiledSummary.Frequency = riSummaryGroup.Frequency;
                        }
                        compiledSummary.GetSstPayable();
                        compiledSummary.GetTotalAmount();
                        compiledSummary.GetTotalPremium();

                        // NB/RN/ALT SAR shall populate if it is monthly mode
                        // other than monthly mode, will take sum AAR
                        if (compiledSummary.Frequency == PickListDetailBo.PremiumFrequencyCodeMonthly)
                        {
                            if (!riSummaryEndDateGroups.IsNullOrEmpty())
                            {
                                var riSummaryEndDateGroup = riSummaryEndDateGroups?.Where(q => q.TreatyCode == riSummaryGroup.TreatyCode && q.RiskQuarter == riSummaryGroup.RiskQuarter && q.CurrencyCode == riSummaryGroup.CurrencyCode && q.CurrencyRate == riSummaryGroup.CurrencyRate).FirstOrDefault();
                                if (riSummaryEndDateGroup != null)
                                {
                                    compiledSummary.NbCession = riSummaryEndDateGroup.NbCession.GetValueOrDefault();
                                    compiledSummary.RnCession = riSummaryEndDateGroup.RnCession.GetValueOrDefault();
                                    compiledSummary.AltCession = riSummaryEndDateGroup.AltCession.GetValueOrDefault();

                                    compiledSummary.NbSar = Util.RoundNullableValue(riSummaryEndDateGroup.NbSar.GetValueOrDefault(), 2);
                                    compiledSummary.RnSar = Util.RoundNullableValue(riSummaryEndDateGroup.RnSar.GetValueOrDefault(), 2);
                                    compiledSummary.AltSar = Util.RoundNullableValue(riSummaryEndDateGroup.AltSar.GetValueOrDefault(), 2);
                                }

                            }
                        }                       
                        else
                        {
                            if (riSummaryGroup != null)
                            {
                                compiledSummary.NbCession = riSummaryGroup.NbCession.GetValueOrDefault();
                                compiledSummary.RnCession = riSummaryGroup.RnCession.GetValueOrDefault();
                                compiledSummary.AltCession = riSummaryGroup.AltCession.GetValueOrDefault();

                                compiledSummary.NbSar = Util.RoundNullableValue(riSummaryGroup.NbSar.GetValueOrDefault(), 2);
                                compiledSummary.RnSar = Util.RoundNullableValue(riSummaryGroup.RnSar.GetValueOrDefault(), 2);
                                compiledSummary.AltSar = Util.RoundNullableValue(riSummaryGroup.AltSar.GetValueOrDefault(), 2);
                            }
                        }

                        if (compiledSummary.RiskQuarter == compiledSummary.SoaQuarter)
                        {
                            var claimDataGroup = claimDataGroups?.Where(q => q.TreatyCode == soaDataGroup.TreatyCode && q.SoaQuarter == SoaDataBatchBo.Quarter).FirstOrDefault();
                            if (claimDataGroup != null)
                            {
                                if (compiledSummary.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ClaimRecoveryAmt.GetValueOrDefault(), 2);
                                else
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ForeignClaimRecoveryAmt.GetValueOrDefault(), 2);
                            }
                        }

                        switch (businessOriginCode)
                        {
                            case PickListDetailBo.BusinessOriginCodeWithinMalaysia:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeWM;
                                break;
                            case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeOM;
                                break;
                            case PickListDetailBo.BusinessOriginCodeServiceFee:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeSFWM;
                                break;
                        }

                        compiledSummary.GetNetTotalAmount();
                        if (compiledSummary.RiskQuarter != compiledSummary.SoaQuarter)
                        {
                            switch (businessOriginCode)
                            {
                                case PickListDetailBo.BusinessOriginCodeWithinMalaysia:
                                    if (compiledSummary.NetTotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNWM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNWM;
                                    break;
                                case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                                    if (compiledSummary.NetTotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNOM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNOM;
                                    break;
                                case PickListDetailBo.BusinessOriginCodeServiceFee:
                                    if (compiledSummary.TotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNSFWM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNSFWM;
                                    break;
                            }
                        }
                        
                        if (compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeWM && compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeOM)
                            compiledSummary.Amount1 = compiledSummary.NetTotalAmount;

                        compiledSummaries.Add(compiledSummary);
                    }
                }
                
            }

            SoaDataCompiledSummaryBos.AddRange(compiledSummaries);
            foreach (var compiledSummary in compiledSummaries)
            {
                var bo = compiledSummary;
                bo.CreatedById = SoaDataBatchBo.CreatedById;
                bo.UpdatedById = SoaDataBatchBo.UpdatedById;

                if (!Test)
                {
                    SoaDataCompiledSummaryService.Create(ref bo);
                }
                else
                {
                    //PrintCompiledSummary(bo);
                }

                //Added update for UpdatedAt for fail checking
                var boToUpdate = SoaDataBatchBo;
                SoaDataBatchService.Update(ref boToUpdate);
            }
        }

        public void CreateSoaDataByRiData()
        {
            if (SoaDataBatchBo == null)
                return;

            bool updateClaim = true;
            var soaDataBos = new List<SoaDataBo> { };
            using (var db = new AppDbContext(false))
            {
                var riDataGroups = QueryRiDataGroupBy(db);
                var claimDataGroups = QueryClaimDataSumGroupBy(db, true);

                var nbs = QueryRiDataSumByTransactionType(db, PickListDetailBo.TransactionTypeCodeNewBusiness);
                var rns = QueryRiDataSumByTransactionType(db, PickListDetailBo.TransactionTypeCodeRenewal);
                var als = QueryRiDataSumByTransactionType(db, PickListDetailBo.TransactionTypeCodeAlteration);

                if (riDataGroups.IsNullOrEmpty())
                    return;

                foreach (var riDataGroup in riDataGroups)
                {
                    var businessOriginCode = CacheService.BusinessOrigins.Where(q => q.Id == SoaDataBatchBo.TreatyBo?.BusinessOriginPickListDetailId).Select(q => q.Code).FirstOrDefault();
                    var treatyTypeCode = CacheService.TreatyCodeBos.Where(q => q.Code == riDataGroup.TreatyCode && q.TreatyId == SoaDataBatchBo.TreatyId).FirstOrDefault();
                    var treatyCode = riDataGroup.TreatyCode;
                    var riskPeriodMonth = riDataGroup.RiskPeriodMonth;
                    var riskPeriodYear = riDataGroup.RiskPeriodYear;
                    var currencyCode = riDataGroup.CurrencyCode;

                    var bo = new SoaDataBo
                    {
                        BusinessCode = businessOriginCode,
                        TreatyId = SoaDataBatchBo.TreatyBo?.TreatyIdCode,
                        TreatyCode = riDataGroup.TreatyCode,
                        RiskMonth = riDataGroup.RiskPeriodMonth,                        
                        TotalDiscount = riDataGroup.TransactionDiscount.GetValueOrDefault(),
                        NoClaimBonus = riDataGroup.NoClaimBonus.GetValueOrDefault(),
                        SurrenderValue = riDataGroup.SurrenderValue.GetValueOrDefault(),
                        Gst = riDataGroup.GstAmount.GetValueOrDefault(),
                        DatabaseCommission = riDataGroup.DatabaseCommission.GetValueOrDefault(),
                        BrokerageFee = riDataGroup.BrokerageFee.GetValueOrDefault(),
                        SoaReceivedDate = SoaDataBatchBo.StatementReceivedAt,
                        BordereauxReceivedDate = SoaDataBatchBo.RiDataBatchBo.ReceivedAt,
                        CompanyName = SoaDataBatchBo.CedantBo.Name,
                        RiskQuarter = GetQuarterInfo(riDataGroup.RiskPeriodMonth, riDataGroup.RiskPeriodYear),
                        TreatyType = riDataGroup.TreatyType,
                        TreatyMode = riDataGroup.PremiumFrequencyCode,
                        CurrencyCode = SoaDataBatchBo.CurrencyCodePickListDetailBo?.Code,
                        CurrencyRate = SoaDataBatchBo.CurrencyRate,
                    };

                    if (string.IsNullOrEmpty(riDataGroup.TreatyType))
                    {
                        if (treatyTypeCode != null)
                            bo.TreatyType = treatyTypeCode.TreatyTypePickListDetailBo?.Code;
                    }

                    var nb = nbs?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode).FirstOrDefault();
                    if (nb != null) bo.NbPremium = nb.TransactionPremium.GetValueOrDefault();

                    var rn = rns?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode).FirstOrDefault();
                    if (rn != null) bo.RnPremium = rn.TransactionPremium.GetValueOrDefault();

                    var al = als?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode).FirstOrDefault();
                    if (al != null) bo.AltPremium = al.TransactionPremium.GetValueOrDefault();

                    if (bo.RiskQuarter == FormatQuarter(SoaDataBatchBo.Quarter))
                    {
                        var claimDataGroup = claimDataGroups?.Where(q => q.TreatyCode == treatyCode && q.SoaQuarter == SoaDataBatchBo.Quarter).FirstOrDefault();
                        if (claimDataGroup != null && updateClaim)
                        {
                            if (bo.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                                bo.Claim = claimDataGroup.ClaimRecoveryAmt.GetValueOrDefault();
                            else
                                bo.Claim = claimDataGroup.ForeignClaimRecoveryAmt.GetValueOrDefault();
                            updateClaim = false;
                        }  
                    }

                    bo.GetGrossPremium();
                    bo.GetTotalCommission();
                    bo.GetNetTotalAmount();

                    soaDataBos.Add(bo);

                    if (!string.IsNullOrEmpty(bo.CurrencyCode) && bo.CurrencyCode != PickListDetailBo.CurrencyCodeMyr)
                    {
                        var MYRbo = new SoaDataBo
                        {
                            BusinessCode = businessOriginCode,
                            TreatyId = SoaDataBatchBo.TreatyBo?.TreatyIdCode,
                            TreatyCode = riDataGroup.TreatyCode,
                            RiskMonth = riDataGroup.RiskPeriodMonth,
                            SoaReceivedDate = SoaDataBatchBo.StatementReceivedAt,
                            BordereauxReceivedDate = SoaDataBatchBo.RiDataBatchBo.ReceivedAt,
                            CompanyName = SoaDataBatchBo.CedantBo.Name,
                            RiskQuarter = GetQuarterInfo(riDataGroup.RiskPeriodMonth, riDataGroup.RiskPeriodYear),
                            TreatyType = riDataGroup.TreatyType,
                            TreatyMode = riDataGroup.PremiumFrequencyCode,
                            CurrencyCode = PickListDetailBo.CurrencyCodeMyr,
                            CurrencyRate = SoaDataBatchBo.CurrencyRate,
                        };
                        MYRbo.NbPremium = bo.NbPremium * MYRbo.CurrencyRate;
                        MYRbo.RnPremium = bo.RnPremium * MYRbo.CurrencyRate;
                        MYRbo.AltPremium = bo.AltPremium * MYRbo.CurrencyRate;
                        MYRbo.TotalDiscount = bo.TotalDiscount * MYRbo.CurrencyRate;
                        MYRbo.RiskPremium = bo.RiskPremium * MYRbo.CurrencyRate;
                        MYRbo.NoClaimBonus = bo.NoClaimBonus * MYRbo.CurrencyRate;
                        MYRbo.Levy = bo.Levy * MYRbo.CurrencyRate;
                        MYRbo.ProfitComm = bo.ProfitComm * MYRbo.CurrencyRate;
                        MYRbo.SurrenderValue = bo.SurrenderValue * MYRbo.CurrencyRate;
                        MYRbo.ModcoReserveIncome = bo.ModcoReserveIncome * MYRbo.CurrencyRate;
                        MYRbo.RiDeposit = bo.RiDeposit * MYRbo.CurrencyRate;
                        MYRbo.DatabaseCommission = bo.DatabaseCommission * MYRbo.CurrencyRate;
                        MYRbo.AdministrationContribution = bo.AdministrationContribution * MYRbo.CurrencyRate;
                        MYRbo.ShareOfRiCommissionFromCompulsoryCession = bo.ShareOfRiCommissionFromCompulsoryCession * MYRbo.CurrencyRate;
                        MYRbo.RecaptureFee = bo.RecaptureFee * MYRbo.CurrencyRate;
                        MYRbo.CreditCardCharges = bo.CreditCardCharges * MYRbo.CurrencyRate;
                        MYRbo.BrokerageFee = bo.BrokerageFee * MYRbo.CurrencyRate;
                        MYRbo.Claim = bo.Claim * MYRbo.CurrencyRate;
                        MYRbo.Gst = bo.Gst * MYRbo.CurrencyRate;

                        MYRbo.GetGrossPremium();
                        MYRbo.GetTotalCommission();
                        MYRbo.GetNetTotalAmount();

                        soaDataBos.Add(MYRbo);
                    }
                }
            }

            foreach (var soaDataBo in soaDataBos)
            {
                var bo = soaDataBo;
                bo.SoaDataBatchId = SoaDataBatchBo.Id;
                bo.SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter);
                bo.CreatedById = SoaDataBatchBo.CreatedById;
                bo.UpdatedById = SoaDataBatchBo.UpdatedById;
                bo.MappingStatus = SoaDataBo.MappingStatusSuccess;
                SoaDataService.Create(ref bo);

                //Added update for UpdatedAt for fail checking
                var boToUpdate = SoaDataBatchBo;
                SoaDataBatchService.Update(ref boToUpdate);
            }
        }

        public void CreateSoaDataByClaimData()
        {
            if (SoaDataBatchBo == null)
                return;

            bool updateClaim = true;
            var soaDataBos = new List<SoaDataBo> { };
            using (var db = new AppDbContext(false))
            {
                var claimDataGroups = QueryClaimDataGroupBy(db);
                var claimDataSumGroups = QueryClaimDataSumGroupBy(db, true);

                if (claimDataGroups.IsNullOrEmpty())
                    return;

                foreach (var claimDataGroup in claimDataGroups)
                {
                    var businessOriginCode = CacheService.BusinessOrigins.Where(q => q.Id == SoaDataBatchBo.TreatyBo?.BusinessOriginPickListDetailId).Select(q => q.Code).FirstOrDefault();
                    var treatyTypeCode = CacheService.TreatyCodeBos.Where(q => q.Code == claimDataGroup.TreatyCode && q.TreatyId == SoaDataBatchBo.TreatyId).FirstOrDefault();
                    var treatyCode = claimDataGroup.TreatyCode;

                    if (claimDataGroup.RiskQuarter == SoaDataBatchBo.Quarter)
                    {
                        var bo = new SoaDataBo
                        {
                            BusinessCode = businessOriginCode,
                            TreatyId = SoaDataBatchBo.TreatyBo?.TreatyIdCode,
                            TreatyCode = claimDataGroup.TreatyCode,
                            TreatyType = claimDataGroup.TreatyType,
                            SoaReceivedDate = SoaDataBatchBo.StatementReceivedAt,
                            CompanyName = SoaDataBatchBo.CedantBo.Name,
                            RiskQuarter = claimDataGroup.RiskQuarter,
                            CurrencyCode = SoaDataBatchBo.CurrencyCodePickListDetailBo?.Code,
                            CurrencyRate = SoaDataBatchBo.CurrencyRate,
                        };
                        bo.GetGrossPremium();
                        bo.GetTotalCommission();
                        bo.GetNetTotalAmount();

                        if (string.IsNullOrEmpty(claimDataGroup.TreatyType))
                        {
                            if (treatyTypeCode != null)
                                bo.TreatyType = treatyTypeCode.TreatyTypePickListDetailBo?.Code;
                        }

                        var claimDataSumGroup = claimDataSumGroups?.Where(q => q.TreatyCode == treatyCode && q.SoaQuarter == SoaDataBatchBo.Quarter).FirstOrDefault();
                        if (claimDataSumGroup != null && updateClaim)
                        {
                            if (bo.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                                bo.Claim = claimDataSumGroup.ClaimRecoveryAmt.GetValueOrDefault();
                            else
                                bo.Claim = claimDataSumGroup.ForeignClaimRecoveryAmt.GetValueOrDefault();
                            updateClaim = false;
                        }

                        soaDataBos.Add(bo);

                        if (!string.IsNullOrEmpty(bo.CurrencyCode) && bo.CurrencyCode != PickListDetailBo.CurrencyCodeMyr)
                        {
                            var MYRbo = new SoaDataBo
                            {
                                BusinessCode = businessOriginCode,
                                TreatyId = SoaDataBatchBo.TreatyBo?.TreatyIdCode,
                                TreatyCode = claimDataGroup.TreatyCode,
                                SoaReceivedDate = SoaDataBatchBo.StatementReceivedAt,
                                BordereauxReceivedDate = SoaDataBatchBo.RiDataBatchBo.ReceivedAt,
                                CompanyName = SoaDataBatchBo.CedantBo.Name,
                                RiskQuarter = claimDataGroup.RiskQuarter,
                                TreatyType = claimDataGroup.TreatyType,
                                CurrencyCode = PickListDetailBo.CurrencyCodeMyr,
                                CurrencyRate = SoaDataBatchBo.CurrencyRate,
                            };
                            MYRbo.TreatyType = bo.TreatyType;
                            MYRbo.Claim = bo.Claim * MYRbo.CurrencyRate;

                            MYRbo.GetGrossPremium();
                            MYRbo.GetTotalCommission();
                            MYRbo.GetNetTotalAmount();

                            soaDataBos.Add(MYRbo);
                        }
                    }
                    
                }
            }

            foreach (var soaDataBo in soaDataBos)
            {
                var bo = soaDataBo;
                bo.SoaDataBatchId = SoaDataBatchBo.Id;
                bo.SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter);
                bo.CreatedById = SoaDataBatchBo.CreatedById;
                bo.UpdatedById = SoaDataBatchBo.UpdatedById;
                bo.MappingStatus = SoaDataBo.MappingStatusSuccess;
                SoaDataService.Create(ref bo);

                //Added update for UpdatedAt for fail checking
                var boToUpdate = SoaDataBatchBo;
                SoaDataBatchService.Update(ref boToUpdate);
            }
        }

        public void CreateCompiledSummaryIFRS17()
        {
            if (SoaDataBatchBo == null)
                return;

            var compiledSummaries = new List<SoaDataCompiledSummaryBo> { };
            using (var db = new AppDbContext(false))
            {
                var businessOriginCode = CacheService.BusinessOrigins.Where(q => q.Id == TreatyBo?.BusinessOriginPickListDetailId).Select(q => q.Code).FirstOrDefault();
                if (string.IsNullOrEmpty(businessOriginCode))
                    return;

                double? soaDataRiskPremium = 0;
                var soaDataGroups = db.SoaData
                    .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                    .Where(q => q.BusinessCode == businessOriginCode)
                    .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate })
                    .Select(q => new SoaDataGroupBy
                    {
                        TreatyCode = q.Key.TreatyCode,
                        RiskQuarter = q.Key.RiskQuarter,
                        CurrencyCode = q.Key.CurrencyCode,
                        CurrencyRate = q.Key.CurrencyRate,

                        TotalDiscount = q.Sum(d => d.TotalDiscount),
                        RiskPremium = q.Sum(d => d.RiskPremium),
                        Levy = q.Sum(d => d.Levy),
                        ProfitComm = q.Sum(d => d.ProfitComm),
                        ModcoReserveIncome = q.Sum(d => d.ModcoReserveIncome),
                        RiDeposit = q.Sum(d => d.RiDeposit),
                        AdministrationContribution = q.Sum(d => d.AdministrationContribution),
                        ShareOfRiCommissionFromCompulsoryCession = q.Sum(d => d.ShareOfRiCommissionFromCompulsoryCession),
                        RecaptureFee = q.Sum(d => d.RecaptureFee),
                        CreditCardCharges = q.Sum(d => d.CreditCardCharges),
                    })
                    .OrderBy(q => q.TreatyCode)
                    .ToList();

                var soaDataCompiledSummaries = SoaDataCompiledSummaryBos
                    .Where(q => q.ReportingType == SoaDataCompiledSummaryBo.ReportingTypeIFRS4)
                    .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate })
                    .Select(q => new SoaDataCompiledSummaryBo
                    {
                        TreatyCode = q.Key.TreatyCode,
                        RiskQuarter = q.Key.RiskQuarter,

                        TotalPremium = q.Sum(d => d.TotalPremium),

                        CurrencyCode = q.Key.CurrencyCode,
                        CurrencyRate = q.Key.CurrencyRate,
                    })
                    .OrderBy(q => q.TreatyCode)
                    .ToList();

                // Risk Qtr shall be populate based on risk period in RI Data but when there is only Claim Data in SOA, Risk Qtr will follow SOA Qtr
                if (GetRiDataBatchId() != 0)
                {
                    var riSummaryGroups = QueryRiSummaryIfrs17GroupBy(db, true);
                    var riSummaryEndDateGroups = QueryRiSummaryIfrs17GroupByEndDate(db, true);

                    var claimDataGroups = QueryClaimDataSumGroupBy(db, false, true);

                    foreach (var riSummaryGroup in riSummaryGroups)
                    {
                        var compiledSummary = new SoaDataCompiledSummaryBo
                        {
                            SoaDataBatchId = SoaDataBatchBo.Id,
                            ReportingType = SoaDataCompiledSummaryBo.ReportingTypeIFRS17,

                            InvoiceDate = DateTime.Now,
                            StatementReceivedDate = SoaDataBatchBo.StatementReceivedAt,

                            TreatyCode = riSummaryGroup.TreatyCode,
                            RiskQuarter = riSummaryGroup.RiskQuarter,
                            SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                            BusinessCode = businessOriginCode,
                            Frequency = riSummaryGroup.Frequency,
                            ContractCode = riSummaryGroup.ContractCode,
                            AnnualCohort = riSummaryGroup.AnnualCohort,

                            CurrencyCode = riSummaryGroup.CurrencyCode,
                            CurrencyRate = riSummaryGroup.CurrencyRate,

                            NbPremium = Util.RoundNullableValue(riSummaryGroup.NbPremium.GetValueOrDefault(), 2),
                            RnPremium = Util.RoundNullableValue(riSummaryGroup.RnPremium.GetValueOrDefault(), 2),
                            AltPremium = Util.RoundNullableValue(riSummaryGroup.AltPremium.GetValueOrDefault(), 2),

                            NbDiscount = Util.RoundNullableValue(riSummaryGroup.NbDiscount.GetValueOrDefault(), 2),
                            RnDiscount = Util.RoundNullableValue(riSummaryGroup.RnDiscount.GetValueOrDefault(), 2),
                            AltDiscount = Util.RoundNullableValue(riSummaryGroup.AltDiscount.GetValueOrDefault(), 2),

                            DTH = Util.RoundNullableValue(riSummaryGroup.DTH.GetValueOrDefault(), 2),
                            TPA = Util.RoundNullableValue(riSummaryGroup.TPA.GetValueOrDefault(), 2),
                            TPS = Util.RoundNullableValue(riSummaryGroup.TPS.GetValueOrDefault(), 2),
                            PPD = Util.RoundNullableValue(riSummaryGroup.PPD.GetValueOrDefault(), 2),
                            CCA = Util.RoundNullableValue(riSummaryGroup.CCA.GetValueOrDefault(), 2),
                            CCS = Util.RoundNullableValue(riSummaryGroup.CCS.GetValueOrDefault(), 2),
                            PA = Util.RoundNullableValue(riSummaryGroup.PA.GetValueOrDefault(), 2),
                            HS = Util.RoundNullableValue(riSummaryGroup.HS.GetValueOrDefault(), 2),
                            TPD = Util.RoundNullableValue(riSummaryGroup.TPD.GetValueOrDefault(), 2),
                            CI = Util.RoundNullableValue(riSummaryGroup.CI.GetValueOrDefault(), 2),

                            NoClaimBonus = Util.RoundNullableValue(riSummaryGroup.NoClaimBonus.GetValueOrDefault(), 2),
                            SurrenderValue = Util.RoundNullableValue(riSummaryGroup.SurrenderValue.GetValueOrDefault(), 2),
                            DatabaseCommission = Util.RoundNullableValue(riSummaryGroup.DatabaseCommission.GetValueOrDefault(), 2),
                            BrokerageFee = Util.RoundNullableValue(riSummaryGroup.BrokerageFee.GetValueOrDefault(), 2),
                            ServiceFee = Util.RoundNullableValue(riSummaryGroup.ServiceFee.GetValueOrDefault(), 2),
                        };
                        compiledSummary.GetSstPayable();
                        compiledSummary.GetTotalAmount();
                        compiledSummary.GetTotalPremium();

                        // NB/RN/ALT SAR shall populate if it is monthly mode
                        // other than monthly mode, will take sum AAR
                        if (compiledSummary.Frequency == PickListDetailBo.PremiumFrequencyCodeMonthly)
                        {
                            if (!riSummaryEndDateGroups.IsNullOrEmpty())
                            {
                                var riSummaryEndDateGroup = riSummaryEndDateGroups?.Where(q => q.TreatyCode == riSummaryGroup.TreatyCode && q.RiskQuarter == riSummaryGroup.RiskQuarter && q.CurrencyCode == riSummaryGroup.CurrencyCode
                                        && q.CurrencyRate == riSummaryGroup.CurrencyRate && q.Frequency == riSummaryGroup.Frequency && q.ContractCode == riSummaryGroup.ContractCode && q.AnnualCohort == riSummaryGroup.AnnualCohort).FirstOrDefault();
                                if (riSummaryEndDateGroup != null)
                                {
                                    compiledSummary.NbCession = riSummaryGroup.NbCession.GetValueOrDefault();
                                    compiledSummary.RnCession = riSummaryGroup.RnCession.GetValueOrDefault();
                                    compiledSummary.AltCession = riSummaryGroup.AltCession.GetValueOrDefault();

                                    compiledSummary.NbSar = Util.RoundNullableValue(riSummaryEndDateGroup.NbSar.GetValueOrDefault(), 2);
                                    compiledSummary.RnSar = Util.RoundNullableValue(riSummaryEndDateGroup.RnSar.GetValueOrDefault(), 2);
                                    compiledSummary.AltSar = Util.RoundNullableValue(riSummaryEndDateGroup.AltSar.GetValueOrDefault(), 2);
                                }
                            }
                        }                        
                        else
                        {
                            if (riSummaryGroup != null)
                            {
                                compiledSummary.NbCession = riSummaryGroup.NbCession.GetValueOrDefault();
                                compiledSummary.RnCession = riSummaryGroup.RnCession.GetValueOrDefault();
                                compiledSummary.AltCession = riSummaryGroup.AltCession.GetValueOrDefault();

                                compiledSummary.NbSar = Util.RoundNullableValue(riSummaryGroup.NbSar.GetValueOrDefault(), 2);
                                compiledSummary.RnSar = Util.RoundNullableValue(riSummaryGroup.RnSar.GetValueOrDefault(), 2);
                                compiledSummary.AltSar = Util.RoundNullableValue(riSummaryGroup.AltSar.GetValueOrDefault(), 2);
                            }
                        }

                        var soaDataGroup = soaDataGroups?.Where(q => q.TreatyCode == riSummaryGroup.TreatyCode && q.RiskQuarter == riSummaryGroup.RiskQuarter && q.CurrencyCode == riSummaryGroup.CurrencyCode && q.CurrencyRate == riSummaryGroup.CurrencyRate).FirstOrDefault();
                        if (soaDataGroup != null)
                        {
                            compiledSummary.TotalDiscount = Util.RoundNullableValue(soaDataGroup.TotalDiscount.GetValueOrDefault(), 2);
                            compiledSummary.Levy = Util.RoundNullableValue(soaDataGroup.Levy.GetValueOrDefault(), 2);
                            compiledSummary.Gst = Util.RoundNullableValue(soaDataGroup.Gst.GetValueOrDefault(), 2);
                            compiledSummary.ProfitComm = Util.RoundNullableValue(soaDataGroup.ProfitComm.GetValueOrDefault(), 2);
                            compiledSummary.ModcoReserveIncome = Util.RoundNullableValue(soaDataGroup.ModcoReserveIncome.GetValueOrDefault(), 2);
                            compiledSummary.RiDeposit = Util.RoundNullableValue(soaDataGroup.RiDeposit.GetValueOrDefault(), 2);
                            compiledSummary.AdministrationContribution = Util.RoundNullableValue(soaDataGroup.AdministrationContribution.GetValueOrDefault(), 2);
                            compiledSummary.ShareOfRiCommissionFromCompulsoryCession = Util.RoundNullableValue(soaDataGroup.ShareOfRiCommissionFromCompulsoryCession.GetValueOrDefault(), 2);
                            compiledSummary.RecaptureFee = Util.RoundNullableValue(soaDataGroup.RecaptureFee.GetValueOrDefault(), 2);
                            compiledSummary.CreditCardCharges = Util.RoundNullableValue(soaDataGroup.CreditCardCharges.GetValueOrDefault(), 2);
                            compiledSummary.CurrencyCode = soaDataGroup.CurrencyCode;
                            compiledSummary.CurrencyRate = soaDataGroup.CurrencyRate;

                            soaDataRiskPremium = soaDataGroup.RiskPremium.GetValueOrDefault();
                        }

                        if (compiledSummary.RiskQuarter == compiledSummary.SoaQuarter)
                        {
                            var claimDataGroup = claimDataGroups?.Where(q => q.TreatyCode == riSummaryGroup.TreatyCode && q.SoaQuarter == SoaDataBatchBo.Quarter && q.ContractCode == compiledSummary.ContractCode && q.AnnualCohort == compiledSummary.AnnualCohort).FirstOrDefault();
                            if (claimDataGroup != null)
                            {
                                if (compiledSummary.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ClaimRecoveryAmt.GetValueOrDefault(), 2);
                                else
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ForeignClaimRecoveryAmt.GetValueOrDefault(), 2);
                            }
                        }

                        switch (businessOriginCode)
                        {
                            case PickListDetailBo.BusinessOriginCodeWithinMalaysia:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeWM;
                                break;
                            case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeOM;
                                break;
                            case PickListDetailBo.BusinessOriginCodeServiceFee:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeSFWM;
                                break;
                        }

                        compiledSummary.GetNetTotalAmount();
                        if (compiledSummary.RiskQuarter != compiledSummary.SoaQuarter)
                        {
                            switch (businessOriginCode)
                            {
                                case PickListDetailBo.BusinessOriginCodeWithinMalaysia:
                                    if (compiledSummary.NetTotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNWM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNWM;
                                    break;
                                case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                                    if (compiledSummary.NetTotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNOM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNOM;
                                    break;
                                case PickListDetailBo.BusinessOriginCodeServiceFee:
                                    if (compiledSummary.TotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNSFWM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNSFWM;
                                    break;
                            }
                        }
                        
                        if (compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeWM && compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeOM)
                            compiledSummary.Amount1 = compiledSummary.NetTotalAmount;

                        // Risk Premium update by multiply with premium ratio
                        var soaDataCompiledSummaryBo = soaDataCompiledSummaries.Where(q => q.TreatyCode == compiledSummary.TreatyCode && q.RiskQuarter == compiledSummary.RiskQuarter && q.CurrencyCode == compiledSummary.CurrencyCode && q.CurrencyRate == compiledSummary.CurrencyRate).FirstOrDefault();
                        if (soaDataCompiledSummaryBo != null)
                            compiledSummary.GetRiskPremium(soaDataCompiledSummaryBo.TotalPremium.GetValueOrDefault(), soaDataRiskPremium.GetValueOrDefault());

                        compiledSummaries.Add(compiledSummary);
                    }

                    if (!compiledSummaries.IsNullOrEmpty())
                    {
                        // Claim amount in IFRS17 compiled summary also will be updated under invoice Risk Qtr=SOA Qtr. Claim for annual cohort 2010 is also under SOA Qtr 16Q1 so the amount shall be updated under WM invoice 16Q1
                        // Hence, the claim amount for annual cohort 2010 shall be updated under new row with Risk Qtr 16Q1 with its respective annual cohort & contract code
                        var existAnnualCohorts = compiledSummaries.Where(o => o.ReportingType == SoaDataCompiledSummaryBo.ReportingTypeIFRS17 && o.RiskQuarter == FormatQuarter(SoaDataBatchBo.Quarter) && o.SoaQuarter == FormatQuarter(SoaDataBatchBo.Quarter)).Select(o => o.AnnualCohort).ToArray();
                        if (!claimDataGroups.IsNullOrEmpty())
                        {
                            foreach (var claimDataGroup in claimDataGroups.Where(q => !existAnnualCohorts.Contains(q.AnnualCohort)))
                            {
                                var compiledSummary = new SoaDataCompiledSummaryBo
                                {
                                    SoaDataBatchId = SoaDataBatchBo.Id,
                                    ReportingType = SoaDataCompiledSummaryBo.ReportingTypeIFRS17,

                                    InvoiceDate = DateTime.Now,
                                    StatementReceivedDate = SoaDataBatchBo.StatementReceivedAt,

                                    TreatyCode = claimDataGroup.TreatyCode,
                                    RiskQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                                    SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                                    BusinessCode = businessOriginCode,
                                    ContractCode = claimDataGroup.ContractCode,
                                    AnnualCohort = claimDataGroup.AnnualCohort,
                                };

                                var soaDataCS = compiledSummaries.Where(x => x.ReportingType == SoaDataCompiledSummaryBo.ReportingTypeIFRS17 && x.TreatyCode == claimDataGroup.TreatyCode && x.SoaQuarter == FormatQuarter(SoaDataBatchBo.Quarter) && x.AnnualCohort == claimDataGroup.AnnualCohort && x.ContractCode == claimDataGroup.ContractCode).FirstOrDefault();
                                if (soaDataCS != null)
                                {
                                    compiledSummary.CurrencyCode = soaDataCS.CurrencyCode;
                                    compiledSummary.CurrencyRate = soaDataCS.CurrencyRate;
                                    compiledSummary.Frequency = soaDataCS.Frequency;
                                }
                                else
                                {
                                    compiledSummary.CurrencyCode = SoaDataBatchBo.CurrencyCodePickListDetailBo?.Code;
                                    compiledSummary.CurrencyRate = SoaDataBatchBo.CurrencyRate;
                                    compiledSummary.Frequency = SoaDataCompiledSummaryBos?.Where(x => x.ReportingType == SoaDataCompiledSummaryBo.ReportingTypeIFRS4 && x.TreatyCode == claimDataGroup.TreatyCode && x.SoaQuarter == FormatQuarter(SoaDataBatchBo.Quarter) && x.RiskQuarter == FormatQuarter(SoaDataBatchBo.Quarter)).Select(q => q.Frequency).FirstOrDefault();
                                }

                                if (compiledSummary.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ClaimRecoveryAmt.GetValueOrDefault(), 2);
                                else
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ForeignClaimRecoveryAmt.GetValueOrDefault(), 2);

                                switch (businessOriginCode)
                                {
                                    case PickListDetailBo.BusinessOriginCodeWithinMalaysia:
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeWM;
                                        break;
                                    case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeOM;
                                        break;
                                    case PickListDetailBo.BusinessOriginCodeServiceFee:
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeSFWM;
                                        break;
                                }

                                compiledSummary.GetNetTotalAmount();
                                if (compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeWM && compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeOM)
                                    compiledSummary.Amount1 = compiledSummary.NetTotalAmount;

                                compiledSummaries.Add(compiledSummary);
                            }
                        }
                        
                    }

                }
                else
                {
                    var riSummaryGroups = QueryRiSummaryIfrs17GroupBy(db);
                    var riSummaryEndDateGroups = QueryRiSummaryIfrs17GroupByEndDate(db);

                    var claimDataGroups = QueryClaimDataSumGroupBy(db);

                    foreach (var soaDataGroup in soaDataGroups)
                    {
                        var compiledSummary = new SoaDataCompiledSummaryBo
                        {
                            SoaDataBatchId = SoaDataBatchBo.Id,
                            ReportingType = SoaDataCompiledSummaryBo.ReportingTypeIFRS17,

                            InvoiceDate = DateTime.Now,
                            StatementReceivedDate = SoaDataBatchBo.StatementReceivedAt,

                            TreatyCode = soaDataGroup.TreatyCode,
                            RiskQuarter = soaDataGroup.RiskQuarter,
                            SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                            BusinessCode = businessOriginCode,

                            TotalDiscount = Util.RoundNullableValue(soaDataGroup.TotalDiscount.GetValueOrDefault(), 2),
                            Levy = Util.RoundNullableValue(soaDataGroup.Levy.GetValueOrDefault(), 2),
                            Gst = Util.RoundNullableValue(soaDataGroup.Gst.GetValueOrDefault(), 2),
                            ProfitComm = Util.RoundNullableValue(soaDataGroup.ProfitComm.GetValueOrDefault(), 2),
                            ModcoReserveIncome = Util.RoundNullableValue(soaDataGroup.ModcoReserveIncome.GetValueOrDefault(), 2),
                            RiDeposit = Util.RoundNullableValue(soaDataGroup.RiDeposit.GetValueOrDefault(), 2),
                            AdministrationContribution = Util.RoundNullableValue(soaDataGroup.AdministrationContribution.GetValueOrDefault(), 2),
                            ShareOfRiCommissionFromCompulsoryCession = Util.RoundNullableValue(soaDataGroup.ShareOfRiCommissionFromCompulsoryCession.GetValueOrDefault(), 2),
                            RecaptureFee = Util.RoundNullableValue(soaDataGroup.RecaptureFee.GetValueOrDefault(), 2),
                            CreditCardCharges = Util.RoundNullableValue(soaDataGroup.CreditCardCharges.GetValueOrDefault(), 2),

                            CurrencyCode = soaDataGroup.CurrencyCode,
                            CurrencyRate = soaDataGroup.CurrencyRate.GetValueOrDefault(),
                        };

                        var riSummaryGroup = riSummaryGroups?.Where(q => q.TreatyCode == soaDataGroup.TreatyCode && q.RiskQuarter == compiledSummary.RiskQuarter && q.CurrencyCode == compiledSummary.CurrencyCode && q.CurrencyRate == compiledSummary.CurrencyRate).FirstOrDefault();
                        if (riSummaryGroup != null)
                        {
                            compiledSummary.NbPremium = Util.RoundNullableValue(riSummaryGroup.NbPremium.GetValueOrDefault(), 2);
                            compiledSummary.RnPremium = Util.RoundNullableValue(riSummaryGroup.RnPremium.GetValueOrDefault(), 2);
                            compiledSummary.AltPremium = Util.RoundNullableValue(riSummaryGroup.AltPremium.GetValueOrDefault(), 2);

                            compiledSummary.NbDiscount = Util.RoundNullableValue(riSummaryGroup.NbDiscount.GetValueOrDefault(), 2);
                            compiledSummary.RnDiscount = Util.RoundNullableValue(riSummaryGroup.RnDiscount.GetValueOrDefault(), 2);
                            compiledSummary.AltDiscount = Util.RoundNullableValue(riSummaryGroup.AltDiscount.GetValueOrDefault(), 2);

                            compiledSummary.DTH = Util.RoundNullableValue(riSummaryGroup.DTH.GetValueOrDefault(), 2);
                            compiledSummary.TPA = Util.RoundNullableValue(riSummaryGroup.TPA.GetValueOrDefault(), 2);
                            compiledSummary.TPS = Util.RoundNullableValue(riSummaryGroup.TPS.GetValueOrDefault(), 2);
                            compiledSummary.PPD = Util.RoundNullableValue(riSummaryGroup.PPD.GetValueOrDefault(), 2);
                            compiledSummary.CCA = Util.RoundNullableValue(riSummaryGroup.CCA.GetValueOrDefault(), 2);
                            compiledSummary.CCS = Util.RoundNullableValue(riSummaryGroup.CCS.GetValueOrDefault(), 2);
                            compiledSummary.PA = Util.RoundNullableValue(riSummaryGroup.PA.GetValueOrDefault(), 2);
                            compiledSummary.HS = Util.RoundNullableValue(riSummaryGroup.HS.GetValueOrDefault(), 2);
                            compiledSummary.TPD = Util.RoundNullableValue(riSummaryGroup.TPD.GetValueOrDefault(), 2);
                            compiledSummary.CI = Util.RoundNullableValue(riSummaryGroup.CI.GetValueOrDefault(), 2);

                            compiledSummary.NoClaimBonus = Util.RoundNullableValue(riSummaryGroup.NoClaimBonus.GetValueOrDefault(), 2);
                            compiledSummary.SurrenderValue = Util.RoundNullableValue(riSummaryGroup.SurrenderValue.GetValueOrDefault(), 2);
                            compiledSummary.DatabaseCommission = Util.RoundNullableValue(riSummaryGroup.DatabaseCommission.GetValueOrDefault(), 2);
                            compiledSummary.BrokerageFee = Util.RoundNullableValue(riSummaryGroup.BrokerageFee.GetValueOrDefault(), 2);
                            compiledSummary.ServiceFee = Util.RoundNullableValue(riSummaryGroup.ServiceFee.GetValueOrDefault(), 2);

                            compiledSummary.Frequency = riSummaryGroup.Frequency;
                            compiledSummary.ContractCode = riSummaryGroup.ContractCode;
                            compiledSummary.AnnualCohort = riSummaryGroup.AnnualCohort;
                        }
                        compiledSummary.GetSstPayable();
                        compiledSummary.GetTotalAmount();
                        compiledSummary.GetTotalPremium();

                        // NB/RN/ALT SAR shall populate if it is monthly mode
                        // other than monthly mode, will take sum AAR
                        if (compiledSummary.Frequency == PickListDetailBo.PremiumFrequencyCodeMonthly)
                        {
                            if (!riSummaryEndDateGroups.IsNullOrEmpty())
                            {
                                var riSummaryEndDateGroup = riSummaryEndDateGroups?.Where(q => q.TreatyCode == soaDataGroup.TreatyCode && q.RiskQuarter == compiledSummary.RiskQuarter && q.CurrencyCode == compiledSummary.CurrencyCode && q.CurrencyRate == riSummaryGroup.CurrencyRate && q.Frequency == compiledSummary.Frequency).FirstOrDefault();
                                if (riSummaryEndDateGroup != null)
                                {
                                    compiledSummary.NbCession = riSummaryEndDateGroup.NbCession.GetValueOrDefault();
                                    compiledSummary.RnCession = riSummaryEndDateGroup.RnCession.GetValueOrDefault();
                                    compiledSummary.AltCession = riSummaryEndDateGroup.AltCession.GetValueOrDefault();

                                    compiledSummary.NbSar = Util.RoundNullableValue(riSummaryEndDateGroup.NbSar.GetValueOrDefault(), 2);
                                    compiledSummary.RnSar = Util.RoundNullableValue(riSummaryEndDateGroup.RnSar.GetValueOrDefault(), 2);
                                    compiledSummary.AltSar = Util.RoundNullableValue(riSummaryEndDateGroup.AltSar.GetValueOrDefault(), 2);
                                }
                            }
                        }                        
                        else
                        {
                            if (riSummaryGroup != null)
                            {
                                compiledSummary.NbCession = riSummaryGroup.NbCession.GetValueOrDefault();
                                compiledSummary.RnCession = riSummaryGroup.RnCession.GetValueOrDefault();
                                compiledSummary.AltCession = riSummaryGroup.AltCession.GetValueOrDefault();

                                compiledSummary.NbSar = Util.RoundNullableValue(riSummaryGroup.NbSar.GetValueOrDefault(), 2);
                                compiledSummary.RnSar = Util.RoundNullableValue(riSummaryGroup.RnSar.GetValueOrDefault(), 2);
                                compiledSummary.AltSar = Util.RoundNullableValue(riSummaryGroup.AltSar.GetValueOrDefault(), 2);
                            }
                        }

                        if (compiledSummary.RiskQuarter == compiledSummary.SoaQuarter)
                        {
                            var claimDataGroup = claimDataGroups?.Where(q => q.TreatyCode == soaDataGroup.TreatyCode && q.SoaQuarter == SoaDataBatchBo.Quarter).FirstOrDefault();
                            if (claimDataGroup != null)
                            {
                                if (compiledSummary.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ClaimRecoveryAmt.GetValueOrDefault(), 2);
                                else
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ForeignClaimRecoveryAmt.GetValueOrDefault(), 2);
                            }
                        }

                        switch (businessOriginCode)
                        {
                            case PickListDetailBo.BusinessOriginCodeWithinMalaysia:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeWM;
                                break;
                            case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeOM;
                                break;
                            case PickListDetailBo.BusinessOriginCodeServiceFee:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeSFWM;
                                break;
                        }

                        compiledSummary.GetNetTotalAmount();
                        if (compiledSummary.RiskQuarter != compiledSummary.SoaQuarter)
                        {
                            switch (businessOriginCode)
                            {
                                case PickListDetailBo.BusinessOriginCodeWithinMalaysia:
                                    if (compiledSummary.NetTotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNWM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNWM;
                                    break;
                                case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                                    if (compiledSummary.NetTotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNOM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNOM;
                                    break;
                                case PickListDetailBo.BusinessOriginCodeServiceFee:
                                    if (compiledSummary.TotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNSFWM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNSFWM;
                                    break;
                            }
                        }
                        
                        if (compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeWM && compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeOM)
                            compiledSummary.Amount1 = compiledSummary.NetTotalAmount;

                        // Risk Premium update by multiply with premium ratio
                        var soaDataCompiledSummaryBo = soaDataCompiledSummaries.Where(q => q.TreatyCode == compiledSummary.TreatyCode && q.RiskQuarter == compiledSummary.RiskQuarter && q.CurrencyCode == compiledSummary.CurrencyCode && q.CurrencyRate == compiledSummary.CurrencyRate).FirstOrDefault();
                        if (soaDataCompiledSummaryBo != null)
                            compiledSummary.GetRiskPremium(soaDataCompiledSummaryBo.TotalPremium.GetValueOrDefault(), soaDataGroup.RiskPremium.GetValueOrDefault());

                        compiledSummaries.Add(compiledSummary);
                    }
                }

                foreach (var compiledSummary in compiledSummaries)
                {
                    var bo = compiledSummary;
                    bo.CreatedById = SoaDataBatchBo.CreatedById;
                    bo.UpdatedById = SoaDataBatchBo.UpdatedById;
                    SoaDataCompiledSummaryService.Create(ref bo);

                    //Added update for UpdatedAt for fail checking
                    var boToUpdate = SoaDataBatchBo;
                    SoaDataBatchService.Update(ref boToUpdate);
                }

            }
        }

        public void CreateCompiledSummaryByCellNameIFRS17()
        {
            if (SoaDataBatchBo == null)
                return;

            var compiledSummaries = new List<SoaDataCompiledSummaryBo> { };
            using (var db = new AppDbContext(false))
            {
                var businessOriginCode = CacheService.BusinessOrigins.Where(q => q.Id == TreatyBo?.BusinessOriginPickListDetailId).Select(q => q.Code).FirstOrDefault();
                if (string.IsNullOrEmpty(businessOriginCode))
                    return;

                double? soaDataRiskPremium = 0;
                var soaDataGroups = db.SoaData
                    .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                    .Where(q => q.BusinessCode == businessOriginCode)
                    .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate })
                    .Select(q => new SoaDataGroupBy
                    {
                        TreatyCode = q.Key.TreatyCode,
                        RiskQuarter = q.Key.RiskQuarter,
                        CurrencyCode = q.Key.CurrencyCode,
                        CurrencyRate = q.Key.CurrencyRate,

                        TotalDiscount = q.Sum(d => d.TotalDiscount),
                        RiskPremium = q.Sum(d => d.RiskPremium),
                        Levy = q.Sum(d => d.Levy),
                        ProfitComm = q.Sum(d => d.ProfitComm),
                        ModcoReserveIncome = q.Sum(d => d.ModcoReserveIncome),
                        RiDeposit = q.Sum(d => d.RiDeposit),
                        AdministrationContribution = q.Sum(d => d.AdministrationContribution),
                        ShareOfRiCommissionFromCompulsoryCession = q.Sum(d => d.ShareOfRiCommissionFromCompulsoryCession),
                        RecaptureFee = q.Sum(d => d.RecaptureFee),
                        CreditCardCharges = q.Sum(d => d.CreditCardCharges),
                    })
                    .OrderBy(q => q.TreatyCode)
                    .ToList();

                var soaDataCompiledSummaries = SoaDataCompiledSummaryBos
                    .Where(q => q.ReportingType == SoaDataCompiledSummaryBo.ReportingTypeIFRS4)
                    .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate })
                    .Select(q => new SoaDataCompiledSummaryBo
                    {
                        TreatyCode = q.Key.TreatyCode,
                        RiskQuarter = q.Key.RiskQuarter,

                        TotalPremium = q.Sum(d => d.TotalPremium),

                        CurrencyCode = q.Key.CurrencyCode,
                        CurrencyRate = q.Key.CurrencyRate,
                    })
                    .OrderBy(q => q.TreatyCode)
                    .ToList();

                // Risk Qtr shall be populate based on risk period in RI Data but when there is only Claim Data in SOA, Risk Qtr will follow SOA Qtr
                if (GetRiDataBatchId() != 0)
                {
                    var riSummaryGroups = QueryRiSummaryIfrs17GroupByCellName(db, true);
                    var riSummaryEndDateGroups = QueryRiSummaryIfrs17GroupByEndDateCellName(db, true);

                    var claimDataGroups = QueryClaimDataSumGroupBy(db, false, true);

                    foreach (var riSummaryGroup in riSummaryGroups)
                    {
                        var compiledSummary = new SoaDataCompiledSummaryBo
                        {
                            SoaDataBatchId = SoaDataBatchBo.Id,
                            ReportingType = SoaDataCompiledSummaryBo.ReportingTypeCNIFRS17,

                            InvoiceDate = DateTime.Now,
                            StatementReceivedDate = SoaDataBatchBo.StatementReceivedAt,

                            TreatyCode = riSummaryGroup.TreatyCode,
                            RiskQuarter = riSummaryGroup.RiskQuarter,
                            SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                            BusinessCode = businessOriginCode,
                            Frequency = riSummaryGroup.Frequency,
                            ContractCode = riSummaryGroup.ContractCode,
                            AnnualCohort = riSummaryGroup.AnnualCohort,
                            Mfrs17CellName = riSummaryGroup.Mfrs17CellName,

                            CurrencyCode = riSummaryGroup.CurrencyCode,
                            CurrencyRate = riSummaryGroup.CurrencyRate,

                            NbPremium = Util.RoundNullableValue(riSummaryGroup.NbPremium.GetValueOrDefault(), 2),
                            RnPremium = Util.RoundNullableValue(riSummaryGroup.RnPremium.GetValueOrDefault(), 2),
                            AltPremium = Util.RoundNullableValue(riSummaryGroup.AltPremium.GetValueOrDefault(), 2),

                            NbDiscount = Util.RoundNullableValue(riSummaryGroup.NbDiscount.GetValueOrDefault(), 2),
                            RnDiscount = Util.RoundNullableValue(riSummaryGroup.RnDiscount.GetValueOrDefault(), 2),
                            AltDiscount = Util.RoundNullableValue(riSummaryGroup.AltDiscount.GetValueOrDefault(), 2),

                            DTH = Util.RoundNullableValue(riSummaryGroup.DTH.GetValueOrDefault(), 2),
                            TPA = Util.RoundNullableValue(riSummaryGroup.TPA.GetValueOrDefault(), 2),
                            TPS = Util.RoundNullableValue(riSummaryGroup.TPS.GetValueOrDefault(), 2),
                            PPD = Util.RoundNullableValue(riSummaryGroup.PPD.GetValueOrDefault(), 2),
                            CCA = Util.RoundNullableValue(riSummaryGroup.CCA.GetValueOrDefault(), 2),
                            CCS = Util.RoundNullableValue(riSummaryGroup.CCS.GetValueOrDefault(), 2),
                            PA = Util.RoundNullableValue(riSummaryGroup.PA.GetValueOrDefault(), 2),
                            HS = Util.RoundNullableValue(riSummaryGroup.HS.GetValueOrDefault(), 2),
                            TPD = Util.RoundNullableValue(riSummaryGroup.TPD.GetValueOrDefault(), 2),
                            CI = Util.RoundNullableValue(riSummaryGroup.CI.GetValueOrDefault(), 2),

                            NoClaimBonus = Util.RoundNullableValue(riSummaryGroup.NoClaimBonus.GetValueOrDefault(), 2),
                            SurrenderValue = Util.RoundNullableValue(riSummaryGroup.SurrenderValue.GetValueOrDefault(), 2),
                            DatabaseCommission = Util.RoundNullableValue(riSummaryGroup.DatabaseCommission.GetValueOrDefault(), 2),
                            BrokerageFee = Util.RoundNullableValue(riSummaryGroup.BrokerageFee.GetValueOrDefault(), 2),
                            ServiceFee = Util.RoundNullableValue(riSummaryGroup.ServiceFee.GetValueOrDefault(), 2),
                        };
                        compiledSummary.GetSstPayable();
                        compiledSummary.GetTotalAmount();
                        compiledSummary.GetTotalPremium();

                        // NB/RN/ALT SAR shall populate if it is monthly mode
                        // other than monthly mode, will take sum AAR
                        if (compiledSummary.Frequency == PickListDetailBo.PremiumFrequencyCodeMonthly)
                        {
                            if (!riSummaryEndDateGroups.IsNullOrEmpty())
                            {
                                var riSummaryEndDateGroup = riSummaryEndDateGroups?.Where(q => q.TreatyCode == riSummaryGroup.TreatyCode && q.RiskQuarter == riSummaryGroup.RiskQuarter && q.CurrencyCode == riSummaryGroup.CurrencyCode
                                        && q.CurrencyRate == riSummaryGroup.CurrencyRate && q.Frequency == riSummaryGroup.Frequency && q.ContractCode == riSummaryGroup.ContractCode && q.AnnualCohort == riSummaryGroup.AnnualCohort 
                                        && q.Mfrs17CellName == riSummaryGroup.Mfrs17CellName).FirstOrDefault();
                                if (riSummaryEndDateGroup != null)
                                {
                                    compiledSummary.NbCession = riSummaryGroup.NbCession.GetValueOrDefault();
                                    compiledSummary.RnCession = riSummaryGroup.RnCession.GetValueOrDefault();
                                    compiledSummary.AltCession = riSummaryGroup.AltCession.GetValueOrDefault();

                                    compiledSummary.NbSar = Util.RoundNullableValue(riSummaryEndDateGroup.NbSar.GetValueOrDefault(), 2);
                                    compiledSummary.RnSar = Util.RoundNullableValue(riSummaryEndDateGroup.RnSar.GetValueOrDefault(), 2);
                                    compiledSummary.AltSar = Util.RoundNullableValue(riSummaryEndDateGroup.AltSar.GetValueOrDefault(), 2);
                                }
                            }
                        }
                        else
                        {
                            if (riSummaryGroup != null)
                            {
                                compiledSummary.NbCession = riSummaryGroup.NbCession.GetValueOrDefault();
                                compiledSummary.RnCession = riSummaryGroup.RnCession.GetValueOrDefault();
                                compiledSummary.AltCession = riSummaryGroup.AltCession.GetValueOrDefault();

                                compiledSummary.NbSar = Util.RoundNullableValue(riSummaryGroup.NbSar.GetValueOrDefault(), 2);
                                compiledSummary.RnSar = Util.RoundNullableValue(riSummaryGroup.RnSar.GetValueOrDefault(), 2);
                                compiledSummary.AltSar = Util.RoundNullableValue(riSummaryGroup.AltSar.GetValueOrDefault(), 2);
                            }
                        }

                        var soaDataGroup = soaDataGroups?.Where(q => q.TreatyCode == riSummaryGroup.TreatyCode && q.RiskQuarter == riSummaryGroup.RiskQuarter && q.CurrencyCode == riSummaryGroup.CurrencyCode && q.CurrencyRate == riSummaryGroup.CurrencyRate).FirstOrDefault();
                        if (soaDataGroup != null)
                        {
                            compiledSummary.TotalDiscount = Util.RoundNullableValue(soaDataGroup.TotalDiscount.GetValueOrDefault(), 2);
                            compiledSummary.Levy = Util.RoundNullableValue(soaDataGroup.Levy.GetValueOrDefault(), 2);
                            compiledSummary.Gst = Util.RoundNullableValue(soaDataGroup.Gst.GetValueOrDefault(), 2);
                            compiledSummary.ProfitComm = Util.RoundNullableValue(soaDataGroup.ProfitComm.GetValueOrDefault(), 2);
                            compiledSummary.ModcoReserveIncome = Util.RoundNullableValue(soaDataGroup.ModcoReserveIncome.GetValueOrDefault(), 2);
                            compiledSummary.RiDeposit = Util.RoundNullableValue(soaDataGroup.RiDeposit.GetValueOrDefault(), 2);
                            compiledSummary.AdministrationContribution = Util.RoundNullableValue(soaDataGroup.AdministrationContribution.GetValueOrDefault(), 2);
                            compiledSummary.ShareOfRiCommissionFromCompulsoryCession = Util.RoundNullableValue(soaDataGroup.ShareOfRiCommissionFromCompulsoryCession.GetValueOrDefault(), 2);
                            compiledSummary.RecaptureFee = Util.RoundNullableValue(soaDataGroup.RecaptureFee.GetValueOrDefault(), 2);
                            compiledSummary.CreditCardCharges = Util.RoundNullableValue(soaDataGroup.CreditCardCharges.GetValueOrDefault(), 2);
                            compiledSummary.CurrencyCode = soaDataGroup.CurrencyCode;
                            compiledSummary.CurrencyRate = soaDataGroup.CurrencyRate;

                            soaDataRiskPremium = soaDataGroup.RiskPremium.GetValueOrDefault();
                        }

                        if (compiledSummary.RiskQuarter == compiledSummary.SoaQuarter)
                        {
                            var claimDataGroup = claimDataGroups?.Where(q => q.TreatyCode == riSummaryGroup.TreatyCode && q.SoaQuarter == SoaDataBatchBo.Quarter && q.ContractCode == compiledSummary.ContractCode && q.AnnualCohort == compiledSummary.AnnualCohort).FirstOrDefault();
                            if (claimDataGroup != null)
                            {
                                if (compiledSummary.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ClaimRecoveryAmt.GetValueOrDefault(), 2);
                                else
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ForeignClaimRecoveryAmt.GetValueOrDefault(), 2);
                            }
                        }

                        switch (businessOriginCode)
                        {
                            case PickListDetailBo.BusinessOriginCodeWithinMalaysia:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeWM;
                                break;
                            case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeOM;
                                break;
                            case PickListDetailBo.BusinessOriginCodeServiceFee:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeSFWM;
                                break;
                        }

                        compiledSummary.GetNetTotalAmount();
                        if (compiledSummary.RiskQuarter != compiledSummary.SoaQuarter)
                        {
                            switch (businessOriginCode)
                            {
                                case PickListDetailBo.BusinessOriginCodeWithinMalaysia:
                                    if (compiledSummary.NetTotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNWM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNWM;
                                    break;
                                case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                                    if (compiledSummary.NetTotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNOM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNOM;
                                    break;
                                case PickListDetailBo.BusinessOriginCodeServiceFee:
                                    if (compiledSummary.TotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNSFWM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNSFWM;
                                    break;
                            }
                        }
                        
                        if (compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeWM && compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeOM)
                            compiledSummary.Amount1 = compiledSummary.NetTotalAmount;

                        // Risk Premium update by multiply with premium ratio
                        var soaDataCompiledSummaryBo = soaDataCompiledSummaries.Where(q => q.TreatyCode == compiledSummary.TreatyCode && q.RiskQuarter == compiledSummary.RiskQuarter && q.CurrencyCode == compiledSummary.CurrencyCode && q.CurrencyRate == compiledSummary.CurrencyRate).FirstOrDefault();
                        if (soaDataCompiledSummaryBo != null)
                            compiledSummary.GetRiskPremium(soaDataCompiledSummaryBo.TotalPremium.GetValueOrDefault(), soaDataRiskPremium.GetValueOrDefault());

                        compiledSummaries.Add(compiledSummary);
                    }
                }
                else
                {
                    var riSummaryGroups = QueryRiSummaryIfrs17GroupByCellName(db);
                    var riSummaryEndDateGroups = QueryRiSummaryIfrs17GroupByEndDateCellName(db);

                    var claimDataGroups = QueryClaimDataSumGroupBy(db);

                    foreach (var soaDataGroup in soaDataGroups)
                    {
                        var compiledSummary = new SoaDataCompiledSummaryBo
                        {
                            SoaDataBatchId = SoaDataBatchBo.Id,
                            ReportingType = SoaDataCompiledSummaryBo.ReportingTypeCNIFRS17,

                            InvoiceDate = DateTime.Now,
                            StatementReceivedDate = SoaDataBatchBo.StatementReceivedAt,

                            TreatyCode = soaDataGroup.TreatyCode,
                            RiskQuarter = soaDataGroup.RiskQuarter,
                            SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                            BusinessCode = businessOriginCode,

                            TotalDiscount = Util.RoundNullableValue(soaDataGroup.TotalDiscount.GetValueOrDefault(), 2),
                            Levy = Util.RoundNullableValue(soaDataGroup.Levy.GetValueOrDefault(), 2),
                            Gst = Util.RoundNullableValue(soaDataGroup.Gst.GetValueOrDefault(), 2),
                            ProfitComm = Util.RoundNullableValue(soaDataGroup.ProfitComm.GetValueOrDefault(), 2),
                            ModcoReserveIncome = Util.RoundNullableValue(soaDataGroup.ModcoReserveIncome.GetValueOrDefault(), 2),
                            RiDeposit = Util.RoundNullableValue(soaDataGroup.RiDeposit.GetValueOrDefault(), 2),
                            AdministrationContribution = Util.RoundNullableValue(soaDataGroup.AdministrationContribution.GetValueOrDefault(), 2),
                            ShareOfRiCommissionFromCompulsoryCession = Util.RoundNullableValue(soaDataGroup.ShareOfRiCommissionFromCompulsoryCession.GetValueOrDefault(), 2),
                            RecaptureFee = Util.RoundNullableValue(soaDataGroup.RecaptureFee.GetValueOrDefault(), 2),
                            CreditCardCharges = Util.RoundNullableValue(soaDataGroup.CreditCardCharges.GetValueOrDefault(), 2),

                            CurrencyCode = soaDataGroup.CurrencyCode,
                            CurrencyRate = soaDataGroup.CurrencyRate.GetValueOrDefault(),
                        };

                        var riSummaryGroup = riSummaryGroups?.Where(q => q.TreatyCode == soaDataGroup.TreatyCode && q.RiskQuarter == compiledSummary.RiskQuarter && q.CurrencyCode == compiledSummary.CurrencyCode && q.CurrencyRate == compiledSummary.CurrencyRate).FirstOrDefault();
                        if (riSummaryGroup != null)
                        {
                            compiledSummary.NbPremium = Util.RoundNullableValue(riSummaryGroup.NbPremium.GetValueOrDefault(), 2);
                            compiledSummary.RnPremium = Util.RoundNullableValue(riSummaryGroup.RnPremium.GetValueOrDefault(), 2);
                            compiledSummary.AltPremium = Util.RoundNullableValue(riSummaryGroup.AltPremium.GetValueOrDefault(), 2);

                            compiledSummary.NbDiscount = Util.RoundNullableValue(riSummaryGroup.NbDiscount.GetValueOrDefault(), 2);
                            compiledSummary.RnDiscount = Util.RoundNullableValue(riSummaryGroup.RnDiscount.GetValueOrDefault(), 2);
                            compiledSummary.AltDiscount = Util.RoundNullableValue(riSummaryGroup.AltDiscount.GetValueOrDefault(), 2);

                            compiledSummary.DTH = Util.RoundNullableValue(riSummaryGroup.DTH.GetValueOrDefault(), 2);
                            compiledSummary.TPA = Util.RoundNullableValue(riSummaryGroup.TPA.GetValueOrDefault(), 2);
                            compiledSummary.TPS = Util.RoundNullableValue(riSummaryGroup.TPS.GetValueOrDefault(), 2);
                            compiledSummary.PPD = Util.RoundNullableValue(riSummaryGroup.PPD.GetValueOrDefault(), 2);
                            compiledSummary.CCA = Util.RoundNullableValue(riSummaryGroup.CCA.GetValueOrDefault(), 2);
                            compiledSummary.CCS = Util.RoundNullableValue(riSummaryGroup.CCS.GetValueOrDefault(), 2);
                            compiledSummary.PA = Util.RoundNullableValue(riSummaryGroup.PA.GetValueOrDefault(), 2);
                            compiledSummary.HS = Util.RoundNullableValue(riSummaryGroup.HS.GetValueOrDefault(), 2);
                            compiledSummary.TPD = Util.RoundNullableValue(riSummaryGroup.TPD.GetValueOrDefault(), 2);
                            compiledSummary.CI = Util.RoundNullableValue(riSummaryGroup.CI.GetValueOrDefault(), 2);

                            compiledSummary.NoClaimBonus = Util.RoundNullableValue(riSummaryGroup.NoClaimBonus.GetValueOrDefault(), 2);
                            compiledSummary.SurrenderValue = Util.RoundNullableValue(riSummaryGroup.SurrenderValue.GetValueOrDefault(), 2);
                            compiledSummary.DatabaseCommission = Util.RoundNullableValue(riSummaryGroup.DatabaseCommission.GetValueOrDefault(), 2);
                            compiledSummary.BrokerageFee = Util.RoundNullableValue(riSummaryGroup.BrokerageFee.GetValueOrDefault(), 2);
                            compiledSummary.ServiceFee = Util.RoundNullableValue(riSummaryGroup.ServiceFee.GetValueOrDefault(), 2);

                            compiledSummary.Frequency = riSummaryGroup.Frequency;
                            compiledSummary.ContractCode = riSummaryGroup.ContractCode;
                            compiledSummary.AnnualCohort = riSummaryGroup.AnnualCohort;
                            compiledSummary.Mfrs17CellName = riSummaryGroup.Mfrs17CellName;
                        }
                        compiledSummary.GetSstPayable();
                        compiledSummary.GetTotalAmount();
                        compiledSummary.GetTotalPremium();

                        // NB/RN/ALT SAR shall populate if it is monthly mode
                        // other than monthly mode, will take sum AAR
                        if (compiledSummary.Frequency == PickListDetailBo.PremiumFrequencyCodeMonthly)
                        {
                            if (!riSummaryEndDateGroups.IsNullOrEmpty())
                            {
                                var riSummaryEndDateGroup = riSummaryEndDateGroups?.Where(q => q.TreatyCode == soaDataGroup.TreatyCode && q.RiskQuarter == compiledSummary.RiskQuarter && q.CurrencyCode == compiledSummary.CurrencyCode && q.CurrencyRate == riSummaryGroup.CurrencyRate && q.Frequency == compiledSummary.Frequency).FirstOrDefault();
                                if (riSummaryEndDateGroup != null)
                                {
                                    compiledSummary.NbCession = riSummaryEndDateGroup.NbCession.GetValueOrDefault();
                                    compiledSummary.RnCession = riSummaryEndDateGroup.RnCession.GetValueOrDefault();
                                    compiledSummary.AltCession = riSummaryEndDateGroup.AltCession.GetValueOrDefault();

                                    compiledSummary.NbSar = Util.RoundNullableValue(riSummaryEndDateGroup.NbSar.GetValueOrDefault(), 2);
                                    compiledSummary.RnSar = Util.RoundNullableValue(riSummaryEndDateGroup.RnSar.GetValueOrDefault(), 2);
                                    compiledSummary.AltSar = Util.RoundNullableValue(riSummaryEndDateGroup.AltSar.GetValueOrDefault(), 2);
                                }
                            }
                        }
                        else
                        {
                            if (riSummaryGroup != null)
                            {
                                compiledSummary.NbCession = riSummaryGroup.NbCession.GetValueOrDefault();
                                compiledSummary.RnCession = riSummaryGroup.RnCession.GetValueOrDefault();
                                compiledSummary.AltCession = riSummaryGroup.AltCession.GetValueOrDefault();

                                compiledSummary.NbSar = Util.RoundNullableValue(riSummaryGroup.NbSar.GetValueOrDefault(), 2);
                                compiledSummary.RnSar = Util.RoundNullableValue(riSummaryGroup.RnSar.GetValueOrDefault(), 2);
                                compiledSummary.AltSar = Util.RoundNullableValue(riSummaryGroup.AltSar.GetValueOrDefault(), 2);
                            }
                        }

                        if (compiledSummary.RiskQuarter == compiledSummary.SoaQuarter)
                        {
                            var claimDataGroup = claimDataGroups?.Where(q => q.TreatyCode == soaDataGroup.TreatyCode && q.SoaQuarter == SoaDataBatchBo.Quarter).FirstOrDefault();
                            if (claimDataGroup != null)
                            {
                                if (compiledSummary.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ClaimRecoveryAmt.GetValueOrDefault(), 2);
                                else
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ForeignClaimRecoveryAmt.GetValueOrDefault(), 2);
                            }
                        }

                        switch (businessOriginCode)
                        {
                            case PickListDetailBo.BusinessOriginCodeWithinMalaysia:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeWM;
                                break;
                            case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeOM;
                                break;
                            case PickListDetailBo.BusinessOriginCodeServiceFee:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeSFWM;
                                break;
                        }

                        compiledSummary.GetNetTotalAmount();
                        if (compiledSummary.RiskQuarter != compiledSummary.SoaQuarter)
                        {
                            switch (businessOriginCode)
                            {
                                case PickListDetailBo.BusinessOriginCodeWithinMalaysia:
                                    if (compiledSummary.NetTotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNWM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNWM;
                                    break;
                                case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                                    if (compiledSummary.NetTotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNOM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNOM;
                                    break;
                                case PickListDetailBo.BusinessOriginCodeServiceFee:
                                    if (compiledSummary.TotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNSFWM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNSFWM;
                                    break;
                            }
                        }
                        
                        if (compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeWM && compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeOM)
                            compiledSummary.Amount1 = compiledSummary.NetTotalAmount;

                        // Risk Premium update by multiply with premium ratio
                        var soaDataCompiledSummaryBo = soaDataCompiledSummaries.Where(q => q.TreatyCode == compiledSummary.TreatyCode && q.RiskQuarter == compiledSummary.RiskQuarter && q.CurrencyCode == compiledSummary.CurrencyCode && q.CurrencyRate == compiledSummary.CurrencyRate).FirstOrDefault();
                        if (soaDataCompiledSummaryBo != null)
                            compiledSummary.GetRiskPremium(soaDataCompiledSummaryBo.TotalPremium.GetValueOrDefault(), soaDataGroup.RiskPremium.GetValueOrDefault());

                        compiledSummaries.Add(compiledSummary);
                    }
                }

                foreach (var compiledSummary in compiledSummaries)
                {
                    var bo = compiledSummary;
                    bo.CreatedById = SoaDataBatchBo.CreatedById;
                    bo.UpdatedById = SoaDataBatchBo.UpdatedById;
                    SoaDataCompiledSummaryService.Create(ref bo);

                    //Added update for UpdatedAt for fail checking
                    var boToUpdate = SoaDataBatchBo;
                    SoaDataBatchService.Update(ref boToUpdate);
                }

            }
        }

        public void PrintSoaDataBo1()
        {
            if (ProcessRowSoaData1.IsNullOrEmpty())
                return;

            foreach (var p in ProcessRowSoaData1)
            {
                foreach (var soaData in p.SoaDataBos)
                {
                    PrintSoaData(soaData);
                }
            }
        }

        public bool GetNextRows()
        {
            LogSoaDataFile.SwRead.Start();
            Rows = DataFile.GetNextRows(Take);
            LogSoaDataFile.SwRead.Stop();
            return !Rows.IsNullOrEmpty();
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
                var columns = Columns.Where(q => q.Header == header).ToList();
                if (columns != null && columns.Count > 0)
                {
                    foreach (var column in columns)
                    {
                        column.ColIndex = colIndex;
                    }
                }
            }
        }

        public string GetFilePath()
        {
            if (SoaDataFileBo != null && SoaDataFileBo.RawFileBo != null)
                return SoaDataFileBo.RawFileBo.GetLocalPath();
            if (!string.IsNullOrEmpty(FilePath))
                return FilePath;
            return null;
        }

        public string GenerateTemplate()
        {
            var export = new ExportSoaData();
            export.HandleTempDirectory();
            export.GetColumns();

            export.OpenExcel();

            export.WriteHeaderLine();
            export.WriteTemplateDataLineFormat();

            export.CloseExcel();

            return export.FilePath;
        }

        public string FormatSoaDataByType(int type, SoaDataBo soaDataBo)
        {
            string property = StandardSoaDataOutputBo.GetPropertyNameByType(type);
            return Util.FormatDetail(property, soaDataBo.GetPropertyValue(property));
        }

        public void PrintSoaData(SoaDataBo soaDataBo)
        {
            PrintDetail("Errors", soaDataBo.Errors);

            PrintMessage();

            PrintSoaDataByTypes(new List<int>
            {
                StandardSoaDataOutputBo.TypeCompanyName,
                StandardSoaDataOutputBo.TypeBusinessCode,
                StandardSoaDataOutputBo.TypeTreatyId,
                StandardSoaDataOutputBo.TypeTreatyCode,
                StandardSoaDataOutputBo.TypeTreatyMode,
                StandardSoaDataOutputBo.TypeTreatyType,
                StandardSoaDataOutputBo.TypePlanBlock,
                StandardSoaDataOutputBo.TypeRiskMonth,
                StandardSoaDataOutputBo.TypeSoaQuarter,
                StandardSoaDataOutputBo.TypeRiskQuarter,
                StandardSoaDataOutputBo.TypeRiskPremium,
                StandardSoaDataOutputBo.TypeStatementStatus,
                StandardSoaDataOutputBo.TypeCurrencyCode,
            }, soaDataBo);

            PrintMessage();
        }

        public void PrintSoaDataByType(int type, SoaDataBo soaDataBo)
        {
            PrintMessage(FormatSoaDataByType(type, soaDataBo));
        }

        public void PrintSoaDataByTypes(List<int> types, SoaDataBo soaDataBo)
        {
            foreach (int type in types)
            {
                PrintSoaDataByType(type, soaDataBo);
            }
        }

        public void PrintCompiledSummary(SoaDataCompiledSummaryBo bo)
        {
            PrintMessage();
            PrintDetail("TreatyCode", bo.TreatyCode);
            PrintDetail("RiskQuarter", bo.RiskQuarter);
            PrintDetail("NbPremium", bo.NbPremium);
            PrintDetail("RnPremium", bo.RnPremium);
            PrintDetail("AltPremium", bo.AltPremium);
            PrintDetail("NetTotalAmount", bo.NetTotalAmount);
            PrintDetail("InvoiceType", SoaDataCompiledSummaryBo.GetInvoiceTypeName(bo.InvoiceType));
            PrintMessage();
        }

        public List<RiSummaryGroupBy> QueryRiSummaryGroupBy(AppDbContext db)
        {
            return db.SoaDataRiDataSummaries
                .Where(q => q.Type == SoaDataRiDataSummaryBo.TypeRiDataSummaryIfrs4)
                .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate, q.Frequency })
                .Select(q => new RiSummaryGroupBy
                {
                    TreatyCode = q.Key.TreatyCode,
                    RiskQuarter = q.Key.RiskQuarter,
                    NbPremium = q.Sum(d => d.NbPremium),
                    RnPremium = q.Sum(d => d.RnPremium),
                    AltPremium = q.Sum(d => d.AltPremium),
                    NbDiscount = q.Sum(d => d.NbDiscount),
                    RnDiscount = q.Sum(d => d.RnDiscount),
                    AltDiscount = q.Sum(d => d.AltDiscount),
                    NbCession = q.Sum(d => d.NbCession),
                    NbSar = q.Sum(d => d.NbSar),
                    RnCession = q.Sum(d => d.RnCession),
                    RnSar = q.Sum(d => d.RnSar),
                    AltCession = q.Sum(d => d.AltCession),
                    AltSar = q.Sum(d => d.AltSar),
                    DTH = q.Sum(d => d.DTH),
                    TPA = q.Sum(d => d.TPA),
                    TPS = q.Sum(d => d.TPS),
                    PPD = q.Sum(d => d.PPD),
                    CCA = q.Sum(d => d.CCA),
                    CCS = q.Sum(d => d.CCS),
                    PA = q.Sum(d => d.PA),
                    HS = q.Sum(d => d.HS),

                    NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                    SurrenderValue = q.Sum(d => d.SurrenderValue),
                    BrokerageFee = q.Sum(d => d.BrokerageFee),
                    DatabaseCommission = q.Sum(d => d.DatabaseCommission),
                    ServiceFee = q.Sum(d => d.ServiceFee),

                    CurrencyCode = q.Key.CurrencyCode,
                    CurrencyRate = q.Key.CurrencyRate,
                    Frequency = q.Key.Frequency,
                })
                .OrderBy(q => q.TreatyCode)
                .ThenBy(q => q.RiskQuarter)
                .ToList();
        }

        public List<RiSummaryGroupBy> QueryRiSummaryGroupByEndDate(AppDbContext db)
        {
            List<int> quarterEndMonth = new List<int> { 3, 6, 9, 12 };
            return db.SoaDataRiDataSummaries
                .Where(q => q.Type == SoaDataRiDataSummaryBo.TypeRiDataSummaryIfrs4 && (q.RiskMonth.HasValue && quarterEndMonth.Contains(q.RiskMonth.Value)))
                .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate, q.Frequency })
                .Select(q => new RiSummaryGroupBy
                {
                    TreatyCode = q.Key.TreatyCode,
                    RiskQuarter = q.Key.RiskQuarter,
                    NbPremium = q.Sum(d => d.NbPremium),
                    RnPremium = q.Sum(d => d.RnPremium),
                    AltPremium = q.Sum(d => d.AltPremium),
                    NbDiscount = q.Sum(d => d.NbDiscount),
                    RnDiscount = q.Sum(d => d.RnDiscount),
                    AltDiscount = q.Sum(d => d.AltDiscount),
                    NbCession = q.Sum(d => d.NbCession),
                    NbSar = q.Sum(d => d.NbSar),
                    RnCession = q.Sum(d => d.RnCession),
                    RnSar = q.Sum(d => d.RnSar),
                    AltCession = q.Sum(d => d.AltCession),
                    AltSar = q.Sum(d => d.AltSar),
                    DTH = q.Sum(d => d.DTH),
                    TPA = q.Sum(d => d.TPA),
                    TPS = q.Sum(d => d.TPS),
                    PPD = q.Sum(d => d.PPD),
                    CCA = q.Sum(d => d.CCA),
                    CCS = q.Sum(d => d.CCS),
                    PA = q.Sum(d => d.PA),
                    HS = q.Sum(d => d.HS),

                    NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                    SurrenderValue = q.Sum(d => d.SurrenderValue),
                    BrokerageFee = q.Sum(d => d.BrokerageFee),
                    DatabaseCommission = q.Sum(d => d.DatabaseCommission),
                    ServiceFee = q.Sum(d => d.ServiceFee),

                    CurrencyCode = q.Key.CurrencyCode,
                    CurrencyRate = q.Key.CurrencyRate,
                    Frequency = q.Key.Frequency,
                })
                .OrderBy(q => q.TreatyCode)
                .ThenBy(q => q.RiskQuarter)
                .ToList();
        }

        public List<RiSummaryGroupBy> QueryRiSummaryIfrs17GroupBy(AppDbContext db, bool groupBySp = false)
        {
            if (!groupBySp)
                return db.SoaDataRiDataSummaries
                    .Where(q => q.Type == SoaDataRiDataSummaryBo.TypeRiDataSummaryIfrs17)
                    .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                    .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate, q.Frequency, q.ContractCode })
                    .Select(q => new RiSummaryGroupBy
                    {
                        TreatyCode = q.Key.TreatyCode,
                        RiskQuarter = q.Key.RiskQuarter,
                        CurrencyCode = q.Key.CurrencyCode,
                        CurrencyRate = q.Key.CurrencyRate,
                        Frequency = q.Key.Frequency,
                        ContractCode = q.Key.ContractCode,

                        NbPremium = q.Sum(d => d.NbPremium),
                        RnPremium = q.Sum(d => d.RnPremium),
                        AltPremium = q.Sum(d => d.AltPremium),

                        NbDiscount = q.Sum(d => d.NbDiscount),
                        RnDiscount = q.Sum(d => d.RnDiscount),
                        AltDiscount = q.Sum(d => d.AltDiscount),

                        NbCession = q.Sum(d => d.NbCession),
                        RnCession = q.Sum(d => d.RnCession),
                        AltCession = q.Sum(d => d.AltCession),

                        NbSar = q.Sum(d => d.NbSar),
                        RnSar = q.Sum(d => d.RnSar),
                        AltSar = q.Sum(d => d.AltSar),

                        DTH = q.Sum(d => d.DTH),
                        TPA = q.Sum(d => d.TPA),
                        TPS = q.Sum(d => d.TPS),
                        PPD = q.Sum(d => d.PPD),
                        CCA = q.Sum(d => d.CCA),
                        CCS = q.Sum(d => d.CCS),
                        PA = q.Sum(d => d.PA),
                        HS = q.Sum(d => d.HS),
                        TPD = q.Sum(d => d.TPD),
                        CI = q.Sum(d => d.CI),

                        NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                        SurrenderValue = q.Sum(d => d.SurrenderValue),
                        BrokerageFee = q.Sum(d => d.BrokerageFee),
                        DatabaseCommission = q.Sum(d => d.DatabaseCommission),
                        ServiceFee = q.Sum(d => d.ServiceFee),
                    })
                    .OrderBy(q => q.TreatyCode)
                    .ThenBy(q => q.RiskQuarter)
                    .ToList();
            else
                return db.SoaDataRiDataSummaries
                   .Where(q => q.Type == SoaDataRiDataSummaryBo.TypeRiDataSummaryIfrs17)
                   .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                   .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate, q.Frequency, q.ContractCode, q.AnnualCohort })
                   .Select(q => new RiSummaryGroupBy
                   {
                       TreatyCode = q.Key.TreatyCode,
                       RiskQuarter = q.Key.RiskQuarter,
                       CurrencyCode = q.Key.CurrencyCode,
                       CurrencyRate = q.Key.CurrencyRate,
                       Frequency = q.Key.Frequency,
                       ContractCode = q.Key.ContractCode,
                       AnnualCohort = q.Key.AnnualCohort,

                       NbPremium = q.Sum(d => d.NbPremium),
                       RnPremium = q.Sum(d => d.RnPremium),
                       AltPremium = q.Sum(d => d.AltPremium),

                       NbDiscount = q.Sum(d => d.NbDiscount),
                       RnDiscount = q.Sum(d => d.RnDiscount),
                       AltDiscount = q.Sum(d => d.AltDiscount),

                       NbCession = q.Sum(d => d.NbCession),
                       RnCession = q.Sum(d => d.RnCession),
                       AltCession = q.Sum(d => d.AltCession),

                       NbSar = q.Sum(d => d.NbSar),
                       RnSar = q.Sum(d => d.RnSar),
                       AltSar = q.Sum(d => d.AltSar),

                       DTH = q.Sum(d => d.DTH),
                       TPA = q.Sum(d => d.TPA),
                       TPS = q.Sum(d => d.TPS),
                       PPD = q.Sum(d => d.PPD),
                       CCA = q.Sum(d => d.CCA),
                       CCS = q.Sum(d => d.CCS),
                       PA = q.Sum(d => d.PA),
                       HS = q.Sum(d => d.HS),
                       TPD = q.Sum(d => d.TPD),
                       CI = q.Sum(d => d.CI),

                       NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                       SurrenderValue = q.Sum(d => d.SurrenderValue),
                       BrokerageFee = q.Sum(d => d.BrokerageFee),
                       DatabaseCommission = q.Sum(d => d.DatabaseCommission),
                       ServiceFee = q.Sum(d => d.ServiceFee),
                   })
                   .OrderBy(q => q.TreatyCode)
                   .ThenBy(q => q.RiskQuarter)
                   .ToList();
        }

        public List<RiSummaryGroupBy> QueryRiSummaryIfrs17GroupByEndDate(AppDbContext db, bool groupBySp = false)
        {
            List<int> quarterEndMonth = new List<int> { 3, 6, 9, 12 };
            if (!groupBySp)
                return db.SoaDataRiDataSummaries
                    .Where(q => q.Type == SoaDataRiDataSummaryBo.TypeRiDataSummaryIfrs17 && (q.RiskMonth.HasValue && quarterEndMonth.Contains(q.RiskMonth.Value)))
                    .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                    .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate, q.Frequency, q.ContractCode })
                    .Select(q => new RiSummaryGroupBy
                    {
                        TreatyCode = q.Key.TreatyCode,
                        RiskQuarter = q.Key.RiskQuarter,
                        CurrencyCode = q.Key.CurrencyCode,
                        CurrencyRate = q.Key.CurrencyRate,
                        Frequency = q.Key.Frequency,
                        ContractCode = q.Key.ContractCode,

                        NbPremium = q.Sum(d => d.NbPremium),
                        RnPremium = q.Sum(d => d.RnPremium),
                        AltPremium = q.Sum(d => d.AltPremium),

                        NbDiscount = q.Sum(d => d.NbDiscount),
                        RnDiscount = q.Sum(d => d.RnDiscount),
                        AltDiscount = q.Sum(d => d.AltDiscount),

                        NbCession = q.Sum(d => d.NbCession),
                        RnCession = q.Sum(d => d.RnCession),
                        AltCession = q.Sum(d => d.AltCession),

                        NbSar = q.Sum(d => d.NbSar),
                        RnSar = q.Sum(d => d.RnSar),
                        AltSar = q.Sum(d => d.AltSar),

                        DTH = q.Sum(d => d.DTH),
                        TPA = q.Sum(d => d.TPA),
                        TPS = q.Sum(d => d.TPS),
                        PPD = q.Sum(d => d.PPD),
                        CCA = q.Sum(d => d.CCA),
                        CCS = q.Sum(d => d.CCS),
                        PA = q.Sum(d => d.PA),
                        HS = q.Sum(d => d.HS),
                        TPD = q.Sum(d => d.TPD),
                        CI = q.Sum(d => d.CI),

                        NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                        SurrenderValue = q.Sum(d => d.SurrenderValue),
                        BrokerageFee = q.Sum(d => d.BrokerageFee),
                        DatabaseCommission = q.Sum(d => d.DatabaseCommission),
                        ServiceFee = q.Sum(d => d.ServiceFee),
                    })
                    .OrderBy(q => q.TreatyCode)
                    .ThenBy(q => q.RiskQuarter)
                    .ToList();
            else
                return db.SoaDataRiDataSummaries
               .Where(q => q.Type == SoaDataRiDataSummaryBo.TypeRiDataSummaryIfrs17 && (q.RiskMonth.HasValue && quarterEndMonth.Contains(q.RiskMonth.Value)))
               .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
               .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate, q.Frequency, q.ContractCode, q.AnnualCohort })
               .Select(q => new RiSummaryGroupBy
               {
                   TreatyCode = q.Key.TreatyCode,
                   RiskQuarter = q.Key.RiskQuarter,
                   CurrencyCode = q.Key.CurrencyCode,
                   CurrencyRate = q.Key.CurrencyRate,
                   Frequency = q.Key.Frequency,
                   ContractCode = q.Key.ContractCode,
                   AnnualCohort = q.Key.AnnualCohort,

                   NbPremium = q.Sum(d => d.NbPremium),
                   RnPremium = q.Sum(d => d.RnPremium),
                   AltPremium = q.Sum(d => d.AltPremium),

                   NbDiscount = q.Sum(d => d.NbDiscount),
                   RnDiscount = q.Sum(d => d.RnDiscount),
                   AltDiscount = q.Sum(d => d.AltDiscount),

                   NbCession = q.Sum(d => d.NbCession),
                   RnCession = q.Sum(d => d.RnCession),
                   AltCession = q.Sum(d => d.AltCession),

                   NbSar = q.Sum(d => d.NbSar),
                   RnSar = q.Sum(d => d.RnSar),
                   AltSar = q.Sum(d => d.AltSar),

                   DTH = q.Sum(d => d.DTH),
                   TPA = q.Sum(d => d.TPA),
                   TPS = q.Sum(d => d.TPS),
                   PPD = q.Sum(d => d.PPD),
                   CCA = q.Sum(d => d.CCA),
                   CCS = q.Sum(d => d.CCS),
                   PA = q.Sum(d => d.PA),
                   HS = q.Sum(d => d.HS),
                   TPD = q.Sum(d => d.TPD),
                   CI = q.Sum(d => d.CI),

                   NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                   SurrenderValue = q.Sum(d => d.SurrenderValue),
                   BrokerageFee = q.Sum(d => d.BrokerageFee),
                   DatabaseCommission = q.Sum(d => d.DatabaseCommission),
                   ServiceFee = q.Sum(d => d.ServiceFee),
               })
               .OrderBy(q => q.TreatyCode)
               .ThenBy(q => q.RiskQuarter)
               .ToList();
        }

        public List<RiSummaryGroupBy> QueryRiSummaryIfrs17GroupByCellName(AppDbContext db, bool groupBySp = false)
        {
            if (!groupBySp)
                return db.SoaDataRiDataSummaries
                    .Where(q => q.Type == SoaDataRiDataSummaryBo.TypeRiDataSummaryCNIfrs17)
                    .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                    .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate, q.Frequency, q.Mfrs17CellName, q.ContractCode })
                    .Select(q => new RiSummaryGroupBy
                    {
                        TreatyCode = q.Key.TreatyCode,
                        RiskQuarter = q.Key.RiskQuarter,
                        CurrencyCode = q.Key.CurrencyCode,
                        CurrencyRate = q.Key.CurrencyRate,
                        Frequency = q.Key.Frequency,
                        ContractCode = q.Key.ContractCode,
                        Mfrs17CellName = q.Key.Mfrs17CellName,

                        NbPremium = q.Sum(d => d.NbPremium),
                        RnPremium = q.Sum(d => d.RnPremium),
                        AltPremium = q.Sum(d => d.AltPremium),

                        NbDiscount = q.Sum(d => d.NbDiscount),
                        RnDiscount = q.Sum(d => d.RnDiscount),
                        AltDiscount = q.Sum(d => d.AltDiscount),

                        NbCession = q.Sum(d => d.NbCession),
                        RnCession = q.Sum(d => d.RnCession),
                        AltCession = q.Sum(d => d.AltCession),

                        NbSar = q.Sum(d => d.NbSar),
                        RnSar = q.Sum(d => d.RnSar),
                        AltSar = q.Sum(d => d.AltSar),

                        DTH = q.Sum(d => d.DTH),
                        TPA = q.Sum(d => d.TPA),
                        TPS = q.Sum(d => d.TPS),
                        PPD = q.Sum(d => d.PPD),
                        CCA = q.Sum(d => d.CCA),
                        CCS = q.Sum(d => d.CCS),
                        PA = q.Sum(d => d.PA),
                        HS = q.Sum(d => d.HS),
                        TPD = q.Sum(d => d.TPD),
                        CI = q.Sum(d => d.CI),

                        NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                        SurrenderValue = q.Sum(d => d.SurrenderValue),
                        BrokerageFee = q.Sum(d => d.BrokerageFee),
                        DatabaseCommission = q.Sum(d => d.DatabaseCommission),
                        ServiceFee = q.Sum(d => d.ServiceFee),
                    })
                    .OrderBy(q => q.TreatyCode)
                    .ThenBy(q => q.RiskQuarter)
                    .ToList();
            else
                return db.SoaDataRiDataSummaries
                   .Where(q => q.Type == SoaDataRiDataSummaryBo.TypeRiDataSummaryCNIfrs17)
                   .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                   .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate, q.Frequency, q.Mfrs17CellName, q.ContractCode, q.AnnualCohort })
                   .Select(q => new RiSummaryGroupBy
                   {
                       TreatyCode = q.Key.TreatyCode,
                       RiskQuarter = q.Key.RiskQuarter,
                       CurrencyCode = q.Key.CurrencyCode,
                       CurrencyRate = q.Key.CurrencyRate,
                       Frequency = q.Key.Frequency,
                       ContractCode = q.Key.ContractCode,
                       AnnualCohort = q.Key.AnnualCohort,
                       Mfrs17CellName = q.Key.Mfrs17CellName,

                       NbPremium = q.Sum(d => d.NbPremium),
                       RnPremium = q.Sum(d => d.RnPremium),
                       AltPremium = q.Sum(d => d.AltPremium),

                       NbDiscount = q.Sum(d => d.NbDiscount),
                       RnDiscount = q.Sum(d => d.RnDiscount),
                       AltDiscount = q.Sum(d => d.AltDiscount),

                       NbCession = q.Sum(d => d.NbCession),
                       RnCession = q.Sum(d => d.RnCession),
                       AltCession = q.Sum(d => d.AltCession),

                       NbSar = q.Sum(d => d.NbSar),
                       RnSar = q.Sum(d => d.RnSar),
                       AltSar = q.Sum(d => d.AltSar),

                       DTH = q.Sum(d => d.DTH),
                       TPA = q.Sum(d => d.TPA),
                       TPS = q.Sum(d => d.TPS),
                       PPD = q.Sum(d => d.PPD),
                       CCA = q.Sum(d => d.CCA),
                       CCS = q.Sum(d => d.CCS),
                       PA = q.Sum(d => d.PA),
                       HS = q.Sum(d => d.HS),
                       TPD = q.Sum(d => d.TPD),
                       CI = q.Sum(d => d.CI),

                       NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                       SurrenderValue = q.Sum(d => d.SurrenderValue),
                       BrokerageFee = q.Sum(d => d.BrokerageFee),
                       DatabaseCommission = q.Sum(d => d.DatabaseCommission),
                       ServiceFee = q.Sum(d => d.ServiceFee),
                   })
                   .OrderBy(q => q.TreatyCode)
                   .ThenBy(q => q.RiskQuarter)
                   .ToList();
        }

        public List<RiSummaryGroupBy> QueryRiSummaryIfrs17GroupByEndDateCellName(AppDbContext db, bool groupBySp = false)
        {
            List<int> quarterEndMonth = new List<int> { 3, 6, 9, 12 };
            if (!groupBySp)
                return db.SoaDataRiDataSummaries
                    .Where(q => q.Type == SoaDataRiDataSummaryBo.TypeRiDataSummaryCNIfrs17 && (q.RiskMonth.HasValue && quarterEndMonth.Contains(q.RiskMonth.Value)))
                    .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                    .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate, q.Frequency, q.Mfrs17CellName, q.ContractCode })
                    .Select(q => new RiSummaryGroupBy
                    {
                        TreatyCode = q.Key.TreatyCode,
                        RiskQuarter = q.Key.RiskQuarter,
                        CurrencyCode = q.Key.CurrencyCode,
                        CurrencyRate = q.Key.CurrencyRate,
                        Frequency = q.Key.Frequency,
                        ContractCode = q.Key.ContractCode,
                        Mfrs17CellName = q.Key.Mfrs17CellName,

                        NbPremium = q.Sum(d => d.NbPremium),
                        RnPremium = q.Sum(d => d.RnPremium),
                        AltPremium = q.Sum(d => d.AltPremium),

                        NbDiscount = q.Sum(d => d.NbDiscount),
                        RnDiscount = q.Sum(d => d.RnDiscount),
                        AltDiscount = q.Sum(d => d.AltDiscount),

                        NbCession = q.Sum(d => d.NbCession),
                        RnCession = q.Sum(d => d.RnCession),
                        AltCession = q.Sum(d => d.AltCession),

                        NbSar = q.Sum(d => d.NbSar),
                        RnSar = q.Sum(d => d.RnSar),
                        AltSar = q.Sum(d => d.AltSar),

                        DTH = q.Sum(d => d.DTH),
                        TPA = q.Sum(d => d.TPA),
                        TPS = q.Sum(d => d.TPS),
                        PPD = q.Sum(d => d.PPD),
                        CCA = q.Sum(d => d.CCA),
                        CCS = q.Sum(d => d.CCS),
                        PA = q.Sum(d => d.PA),
                        HS = q.Sum(d => d.HS),
                        TPD = q.Sum(d => d.TPD),
                        CI = q.Sum(d => d.CI),

                        NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                        SurrenderValue = q.Sum(d => d.SurrenderValue),
                        BrokerageFee = q.Sum(d => d.BrokerageFee),
                        DatabaseCommission = q.Sum(d => d.DatabaseCommission),
                        ServiceFee = q.Sum(d => d.ServiceFee),
                    })
                    .OrderBy(q => q.TreatyCode)
                    .ThenBy(q => q.RiskQuarter)
                    .ToList();
            else
                return db.SoaDataRiDataSummaries
               .Where(q => q.Type == SoaDataRiDataSummaryBo.TypeRiDataSummaryCNIfrs17 && (q.RiskMonth.HasValue && quarterEndMonth.Contains(q.RiskMonth.Value)))
               .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
               .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate, q.Frequency, q.Mfrs17CellName, q.ContractCode, q.AnnualCohort })
               .Select(q => new RiSummaryGroupBy
               {
                   TreatyCode = q.Key.TreatyCode,
                   RiskQuarter = q.Key.RiskQuarter,
                   CurrencyCode = q.Key.CurrencyCode,
                   CurrencyRate = q.Key.CurrencyRate,
                   Frequency = q.Key.Frequency,
                   ContractCode = q.Key.ContractCode,
                   AnnualCohort = q.Key.AnnualCohort,
                   Mfrs17CellName = q.Key.Mfrs17CellName,

                   NbPremium = q.Sum(d => d.NbPremium),
                   RnPremium = q.Sum(d => d.RnPremium),
                   AltPremium = q.Sum(d => d.AltPremium),

                   NbDiscount = q.Sum(d => d.NbDiscount),
                   RnDiscount = q.Sum(d => d.RnDiscount),
                   AltDiscount = q.Sum(d => d.AltDiscount),

                   NbCession = q.Sum(d => d.NbCession),
                   RnCession = q.Sum(d => d.RnCession),
                   AltCession = q.Sum(d => d.AltCession),

                   NbSar = q.Sum(d => d.NbSar),
                   RnSar = q.Sum(d => d.RnSar),
                   AltSar = q.Sum(d => d.AltSar),

                   DTH = q.Sum(d => d.DTH),
                   TPA = q.Sum(d => d.TPA),
                   TPS = q.Sum(d => d.TPS),
                   PPD = q.Sum(d => d.PPD),
                   CCA = q.Sum(d => d.CCA),
                   CCS = q.Sum(d => d.CCS),
                   PA = q.Sum(d => d.PA),
                   HS = q.Sum(d => d.HS),
                   TPD = q.Sum(d => d.TPD),
                   CI = q.Sum(d => d.CI),

                   NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                   SurrenderValue = q.Sum(d => d.SurrenderValue),
                   BrokerageFee = q.Sum(d => d.BrokerageFee),
                   DatabaseCommission = q.Sum(d => d.DatabaseCommission),
                   ServiceFee = q.Sum(d => d.ServiceFee),
               })
               .OrderBy(q => q.TreatyCode)
               .ThenBy(q => q.RiskQuarter)
               .ToList();
        }

        public int GetRiDataBatchId()
        {
            if (RiDataBatchBo == null)
                return 0;
            int riDataBatchId = RiDataBatchBo.Id;
            return riDataBatchId;
        }

        public int GetClaimDataBatchId()
        {
            if (ClaimDataBatchBo == null)
                return 0;
            int claimDataBatchId = ClaimDataBatchBo.Id;
            return claimDataBatchId;
        }

        public List<RiDataGroupBy> QueryRiDataGroupBy(AppDbContext db)
        {
            int riDataBatchId = GetRiDataBatchId();
            if (riDataBatchId == 0)
                return null;
            return db.RiData
                .Where(q => q.RiDataBatchId == riDataBatchId)
                .GroupBy(q => new { q.TreatyCode, q.RiskPeriodMonth, q.RiskPeriodYear, q.PremiumFrequencyCode, q.TreatyType, q.CurrencyCode })
                .Select(q => new RiDataGroupBy
                {
                    TreatyCode = q.Key.TreatyCode,
                    RiskPeriodMonth = q.Key.RiskPeriodMonth,
                    RiskPeriodYear = q.Key.RiskPeriodYear,
                    CurrencyCode = q.Key.CurrencyCode,

                    TransactionDiscount = q.Sum(d => d.TransactionDiscount),
                    NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                    SurrenderValue = q.Sum(d => d.SurrenderValue),
                    GstAmount = q.Sum(d => d.GstAmount),
                    DatabaseCommission = q.Sum(d => d.DatabaseCommision),
                    BrokerageFee = q.Sum(d => d.BrokerageFee),

                    PremiumFrequencyCode = q.Key.PremiumFrequencyCode,
                    TreatyType = q.Key.TreatyType,
                })
                .OrderBy(q => q.TreatyCode)
                .ThenBy(q => q.RiskPeriodMonth)
                .ToList();
        }

        public List<RiDataGroupByTransactionType> QueryRiDataSumByTransactionType(AppDbContext db, string transactionType)
        {
            int riDataBatchId = GetRiDataBatchId();
            if (riDataBatchId == 0)
                return null;
            return db.RiData
                .Where(q => q.RiDataBatchId == riDataBatchId)
                .Where(q => q.TransactionTypeCode == transactionType)
                .GroupBy(q => new { q.TreatyCode, q.RiskPeriodMonth, q.RiskPeriodYear, q.CurrencyCode, q.PremiumFrequencyCode })
                .Select(q => new RiDataGroupByTransactionType
                {
                    TreatyCode = q.Key.TreatyCode,
                    RiskPeriodMonth = q.Key.RiskPeriodMonth,
                    RiskPeriodYear = q.Key.RiskPeriodYear,
                    CurrencyCode = q.Key.CurrencyCode,
                    PremiumFrequencyCode = q.Key.PremiumFrequencyCode,

                    StandardPremium = q.Sum(d => d.StandardPremium),
                    SubstandardPremium = q.Sum(d => d.SubstandardPremium),
                    FlatExtraPremium = q.Sum(d => d.FlatExtraPremium),

                    StandardDiscount = q.Sum(d => d.StandardDiscount),
                    SubstandardDiscount = q.Sum(d => d.SubstandardDiscount),
                    TotalDiscount = q.Sum(d => d.TotalDiscount),

                    Aar = q.Sum(d => d.Aar),

                    TransactionPremium = q.Sum(d => d.TransactionPremium),
                    TransactionDiscount = q.Sum(d => d.TransactionDiscount),
                })
                .OrderBy(q => q.TreatyCode)
                .ThenBy(q => q.RiskPeriodMonth)
                .ToList();
        }

        // Create Soa Data from Claim Data
        public List<ClaimRegisterGroupBy> QueryClaimDataGroupBy(AppDbContext db)
        {
            int claimDataBatchId = GetClaimDataBatchId();
            if (claimDataBatchId == 0)
                return null;
            if (ClaimDataBatchBo.Status == ClaimDataBatchBo.StatusReportedClaim)
            {
                // Auto Create - if claim being reported, claim amount from Claim Register regardless of its claim status
                return db.ClaimRegister
                     .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                     .GroupBy(q => new { q.TreatyCode, q.TreatyType, q.RiskQuarter })
                     .Select(q => new ClaimRegisterGroupBy
                     {
                         TreatyCode = q.Key.TreatyCode,
                         TreatyType = q.Key.TreatyType,
                         RiskQuarter = q.Key.RiskQuarter,

                         ClaimRecoveryAmt = q.Sum(d => d.ClaimRecoveryAmt),
                         ForeignClaimRecoveryAmt = q.Sum(d => d.ForeignClaimRecoveryAmt),
                     })
                     .OrderBy(q => q.TreatyCode)
                     .ThenBy(q => q.RiskQuarter)
                     .ToList();
            }
            else
            {
                // Auto Create - if the claim success in process claim amount from Claim Data.
                return db.ClaimData
                    .Where(q => q.ClaimDataBatchId == claimDataBatchId)
                    .Where(q => q.ClaimDataBatch.Status == ClaimDataBatchBo.StatusSuccess)
                    .GroupBy(q => new { q.TreatyCode, q.TreatyType, q.RiskQuarter })
                     .Select(q => new ClaimRegisterGroupBy
                     {
                         TreatyCode = q.Key.TreatyCode,
                         TreatyType = q.Key.TreatyType,
                         RiskQuarter = q.Key.RiskQuarter,

                         ClaimRecoveryAmt = q.Sum(d => d.ClaimRecoveryAmt),
                         ForeignClaimRecoveryAmt = q.Sum(d => d.ForeignClaimRecoveryAmt),
                     })
                     .OrderBy(q => q.TreatyCode)
                     .ThenBy(q => q.RiskQuarter)
                     .ToList();
            }
        }

        public List<ClaimRegisterGroupBy> QueryClaimDataSumGroupBy(AppDbContext db, bool autoCreate = false, bool ifrs17 = false)
        {
            int claimDataBatchId = GetClaimDataBatchId();
            if (claimDataBatchId == 0)
                return null;
            if (autoCreate == true)
            {
                if (ClaimDataBatchBo.Status == ClaimDataBatchBo.StatusReportedClaim)
                {
                    // Auto Create - if claim being reported, claim amount from Claim Register regardless of its claim status
                    return db.ClaimRegister
                         .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                         .GroupBy(q => new { q.TreatyCode, q.SoaQuarter })
                         .Select(q => new ClaimRegisterGroupBy
                         {
                             TreatyCode = q.Key.TreatyCode,
                             SoaQuarter = q.Key.SoaQuarter,

                             ClaimRecoveryAmt = q.Sum(d => d.ClaimRecoveryAmt),
                             ForeignClaimRecoveryAmt = q.Sum(d => d.ForeignClaimRecoveryAmt),
                         })
                         .OrderBy(q => q.TreatyCode)
                         .ThenBy(q => q.SoaQuarter)
                         .ToList();
                }
                else
                {
                    // Auto Create - if the claim success in process claim amount from Claim Data.
                    return db.ClaimData
                        .Where(q => q.ClaimDataBatchId == claimDataBatchId)
                        .Where(q => q.ClaimDataBatch.Status == ClaimDataBatchBo.StatusSuccess)
                        .GroupBy(q => new { q.TreatyCode, q.SoaQuarter })
                        .Select(q => new ClaimRegisterGroupBy
                        {
                            TreatyCode = q.Key.TreatyCode,
                            SoaQuarter = q.Key.SoaQuarter,

                            ClaimRecoveryAmt = q.Sum(d => d.ClaimRecoveryAmt),
                            ForeignClaimRecoveryAmt = q.Sum(d => d.ForeignClaimRecoveryAmt),
                        })
                        .OrderBy(q => q.TreatyCode)
                        .ThenBy(q => q.SoaQuarter)
                        .ToList();
                }
            }
            else
            {
                // Compiled Summary - claim amount from Claim Register regardless of its claim status
                if (!ifrs17)
                    return db.ClaimRegister
                          .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                          .GroupBy(q => new { q.TreatyCode, q.SoaQuarter })
                          .Select(q => new ClaimRegisterGroupBy
                          {
                              TreatyCode = q.Key.TreatyCode,
                              SoaQuarter = q.Key.SoaQuarter,

                              ClaimRecoveryAmt = q.Sum(d => d.ClaimRecoveryAmt),
                              ForeignClaimRecoveryAmt = q.Sum(d => d.ForeignClaimRecoveryAmt),
                          })
                          .OrderBy(q => q.TreatyCode)
                          .ThenBy(q => q.SoaQuarter)
                          .ToList();
                else
                    return db.ClaimRegister
                      .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                      .GroupBy(q => new { q.TreatyCode, q.SoaQuarter, q.Mfrs17ContractCode, AnnualCohort = q.Mfrs17AnnualCohort })
                      .Select(q => new ClaimRegisterGroupBy
                      {
                          TreatyCode = q.Key.TreatyCode,
                          SoaQuarter = q.Key.SoaQuarter,
                          ContractCode = q.Key.Mfrs17ContractCode,
                          AnnualCohort = q.Key.AnnualCohort,

                          ClaimRecoveryAmt = q.Sum(d => d.ClaimRecoveryAmt),
                          ForeignClaimRecoveryAmt = q.Sum(d => d.ForeignClaimRecoveryAmt),
                      })
                      .OrderBy(q => q.TreatyCode)
                      .ThenBy(q => q.SoaQuarter)
                      .ToList();
            }
        }

        public string GetQuarterInfo(int? month, int? year)
        {
            string qtrString = "";

            if (month.HasValue && year.HasValue)
            {
                string quarter = "";
                if (month <= 3)
                    quarter = "Q1";
                else if (month > 3 && month <= 6)
                    quarter = "Q2";
                else if (month > 6 && month <= 9)
                    quarter = "Q3";
                else if (month > 9 && month <= 12)
                    quarter = "Q4";

                qtrString = string.Format("{0}{1}", (year % 100), quarter);
            }
            return qtrString;
        }

        public string FormatQuarter(string quarter)
        {
            string qtrString = "";
            if (!string.IsNullOrEmpty(quarter))
            {
                string[] q = quarter.Split(' ');
                qtrString = string.Format("{0}{1}", (int.Parse(q[0]) % 100), q[1]);
            }
            return qtrString;
        }
    }
}
