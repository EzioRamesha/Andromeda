using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.Sanctions;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Newtonsoft.Json;
using Services;
using Services.Identity;
using Services.Sanctions;
using Shared;
using Shared.DataAccess;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Row = Shared.ProcessFile.Row;

namespace ConsoleApp.Commands.RawFiles.Sanction
{
    public class ProcessSanctionBatch : Command
    {
        public int? SanctionBatchId { get; set; }

        public SanctionBatchBo SanctionBatchBo { get; set; }

        public List<ProcessRowSanction> ProcessRowSanctions { get; set; } = new List<ProcessRowSanction> { };

        public List<ProcessRowSanction> ProcessRowSanction1 { get; set; } = new List<ProcessRowSanction> { };

        public List<SanctionBo> SanctionBos1 { get; set; } = new List<SanctionBo> { };

        public List<Row> Rows { get; set; }

        public List<Column> Columns { get; set; }

        public ModuleBo ModuleBo { get; set; }

        public IProcessFile DataFile { get; set; }

        public int Record { get; set; }

        public int Take { get; set; }

        public int Saved { get; set; }

        public int MaxEmptyRows { get; set; }

        public int TotalDetail { get; set; } = 0;

        public List<string> Errors { get; set; }

        public ProcessSanctionBatch()
        {
            Title = "ProcessSanctionBatch";
            Description = "To process Sanction Upload Batch";
            Options = new string[] {
                "--sanctionBatchId= : Process by Id",
            };
        }

        public override void Initial()
        {
            base.Initial();
            SanctionBatchId = OptionIntegerNullable("sanctionBatchId");
            MaxEmptyRows = Util.GetConfigInteger("ProcessSanctionMaxEmptyRows", 3);
            Take = Util.GetConfigInteger("ProcessSanctionRow", 100);
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
            try
            {
                if (SanctionBatchId.HasValue)
                {
                    SanctionBatchBo = SanctionBatchService.Find(SanctionBatchId.Value);
                    if (SanctionBatchBo != null && SanctionBatchBo.Status != SanctionBatchBo.StatusPending)
                    {
                        Log = false;
                        PrintMessage(MessageBag.NoSanctionBatchPendingProcess);
                        return;
                    }
                }
                else if (SanctionBatchService.CountByStatus(SanctionBatchBo.StatusPending) == 0)
                {
                    Log = false;
                    PrintMessage(MessageBag.NoSanctionBatchPendingProcess);
                    return;
                }
                PrintStarting();

                ModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.SanctionBatch.ToString());

                while (LoadSanctionBatchBo() != null)
                {
                    SetProcessCount("Batch");

                    SetProcessCount("Processed", 0);
                    SetProcessCount("Saved", 0);
                    SetProcessCount("Processed Sanction", 0);
                    SetProcessCount("Processed Name", 0);

                    PrintOutputTitle(string.Format("Process Sanction Batch Id: {0}", SanctionBatchBo.Id));

                    UpdateSanctionBatchStatus(SanctionBatchBo.StatusProcessing, MessageBag.ProcessSanctionBatchProcessing);

                    DeleteSanctionBatch();

                    bool success = ProcessFile();

                    if (success)
                    {
                        UpdateSanctionBatchStatus(SanctionBatchBo.StatusSuccess, MessageBag.ProcessSanctionBatchSuccess);

                        PrintMessage();
                        PrintOutputTitle("Process Sanction Name");
                        ProcessSanctionNameBatch process = new ProcessSanctionNameBatch()
                        {
                            Title = Title,
                            SanctionBatchId = SanctionBatchBo.Id,
                            Take = Util.GetConfigInteger("ProcessSanctionNameRow", 1000),
                            PrintStartEnd = false,
                            ProcessName = "Processed Sanction",
                            LogIndex = 0
                        };
                        process.Run();
                    }
                    else
                    {
                        UpdateSanctionBatchStatus(SanctionBatchBo.StatusFailed, MessageBag.ProcessSanctionBatchFailed);
                    }
                }

                PrintEnding();
            }
            catch (Exception e)
            {
                PrintError(e.Message);
            }
        }

        public SanctionBatchBo LoadSanctionBatchBo()
        {
            SanctionBatchBo = null;
            Errors = new List<string>();
            if (SanctionBatchId.HasValue)
            {
                SanctionBatchBo = SanctionBatchService.Find(SanctionBatchId.Value);
                if (SanctionBatchBo != null && SanctionBatchBo.Status != SanctionBatchBo.StatusPending)
                    SanctionBatchBo = null;
            }
            else
                SanctionBatchBo = SanctionBatchService.FindByStatus(SanctionBatchBo.StatusPending);

            return SanctionBatchBo;
        }

        public void UpdateSanctionBatchStatus(int status, string description)
        {
            var trail = new TrailObject();

            var sanctionBatch = SanctionBatchBo;
            sanctionBatch.Status = status;
            sanctionBatch.Record = Record;

            var result = SanctionBatchService.Update(ref sanctionBatch, ref trail);
            var userTrailBo = new UserTrailBo(
                SanctionBatchBo.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }

        public void DeleteSanctionBatch()
        {
            if (SanctionBatchBo == null || SanctionBatchBo.Method == SanctionBatchBo.MethodAddOn)
                return;

            if (SanctionBatchBo.Method == SanctionBatchBo.MethodReplacement)
            {
                var trail = new TrailObject();

                foreach (var sanctionBatchBo in SanctionBatchService.GetBySourceByStatuses(SanctionBatchBo.SourceId, new List<int> { SanctionBatchBo.StatusSuccess, SanctionBatchBo.StatusFailed }))
                {
                    var bo = sanctionBatchBo;
                    bo.Status = SanctionBatchBo.StatusReplaced;
                    bo.UpdatedById = User.DefaultSuperUserId;
                    Result result = SanctionBatchService.Replace(ref bo, ref trail, Take);

                    if (result.Valid)
                    {
                        StatusHistoryBo statusHistoryBo = new StatusHistoryBo
                        {
                            ModuleId = ModuleBo.Id,
                            ObjectId = bo.Id,
                            Status = bo.Status,
                            CreatedById = User.DefaultSuperUserId,
                            UpdatedById = User.DefaultSuperUserId
                        };
                        StatusHistoryService.Save(ref statusHistoryBo, ref trail);

                        var userTrailBo = new UserTrailBo(
                            bo.Id,
                            "Replace Sanction Upload",
                            result,
                            trail,
                            User.DefaultSuperUserId
                        );
                        UserTrailService.Create(ref userTrailBo);
                    }

                }
            }
        }

        public string GetFilePath()
        {
            if (SanctionBatchBo != null)
                return SanctionBatchBo.GetLocalPath();
            return null;
        }

        public bool GetNextRows()
        {
            Rows = DataFile.GetNextRows(Take);
            return !Rows.IsNullOrEmpty();
        }

        public bool GetNextRecords()
        {
            ProcessRowSanction1 = ProcessRowSanctions.Skip(Record).Take(Take).ToList();
            return !ProcessRowSanction1.IsNullOrEmpty();
        }

        public bool ProcessFile()
        {
            Record = 0;
            Columns = SanctionBatchBo.GetColumns();
            foreach (var column in Columns)
            {
                column.Header = column.Header.Trim().ToLower().RemoveNewLines();
            }

            if (SanctionBatchBo == null)
                return false;

            try
            {
                var filePath = GetFilePath();
                if (!File.Exists(filePath))
                    throw new Exception(string.Format(MessageBag.FileNotExists, filePath));

                DataFile = new TextFile(GetFilePath(), ',')
                {
                    HandleNewLine = true
                };

                if (DataFile == null)
                    throw new Exception(string.Format(MessageBag.FileNotSupport, filePath));

                PrintMessage(string.Format("Sanction Filename: {0}", SanctionBatchBo.FileName));

                ProcessRowSanctions = new List<ProcessRowSanction> { };
                ProcessRowSanction1 = new List<ProcessRowSanction> { };
                while (GetNextRows())
                {
                    MappingRows1(Rows);
                    PrintProcessCount();
                }

                if (DataFile != null)
                    DataFile.Close();

                if (Errors.IsNullOrEmpty())
                {
                    while (GetNextRecords())
                    {
                        SaveSanctionBo1();
                        PrintProcessCount();
                    }
                }
                else
                {
                    SanctionBatchBo.Errors = JsonConvert.SerializeObject(Errors);
                    return false;
                }
            }
            catch (Exception e)
            {
                if (DataFile != null)
                    DataFile.Close();

                var errors = new List<string> { };
                var message = e.Message;
                if (e is DbEntityValidationException dbEx)
                    message = Util.CatchDbEntityValidationException(dbEx).ToString();

                errors.Add(message);

                SanctionBatchBo.Errors = JsonConvert.SerializeObject(errors);
                return false;
            }

            return true;
        }

        public List<MappingSanction> MappingRows(List<Row> rows)
        {
            if (rows.IsNullOrEmpty())
                return null;

            var mappingDataRows = new List<MappingSanction> { };
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
                    int rowType = MappingSanction.ROW_TYPE_DATA;
                    if (rowIndex == 1)
                        rowType = MappingSanction.ROW_TYPE_HEADER;

                    switch (rowType)
                    {
                        case MappingSanction.ROW_TYPE_HEADER:
                            ProcessHeader(row.Columns);

                            break;
                        case MappingSanction.ROW_TYPE_DATA:
                            mappingDataRows.Add(new MappingSanction(this, row));
                            break;
                    }
                }
            }

            //if (!mappingDataRows.IsNullOrEmpty())
            //{
            //    Parallel.ForEach(mappingDataRows, mappingDataRow => mappingDataRow.MappingRow());
            //}

            return mappingDataRows;
        }

        public void MappingRows1(List<Row> rows)
        {
            var mappingDataRows = MappingRows(rows);
            if (!mappingDataRows.IsNullOrEmpty())
            {
                foreach (var mappingDataRow in mappingDataRows)
                {
                    mappingDataRow.MappingRow();
                    if (!mappingDataRow.IsSuccess)
                    {
                        Errors.AddRange(mappingDataRow.Errors);
                    }

                    ProcessRowSanctions.Add(new ProcessRowSanction(this, mappingDataRow));
                }
            }
        }

        public void SaveSanctionBo1()
        {
            //if (!ProcessRowSanction1.IsNullOrEmpty())
            //    Parallel.ForEach(ProcessRowSanction1, q => q.Save());

            foreach (var sanction in ProcessRowSanction1)
            {
                sanction.Save();
                SetProcessCount("Saved");
                Record++;
            }
        }

        public void ProcessHeader(List<Column> columns)   
        {
            foreach (var col in columns)
            {
                int? colIndex = col.ColIndex;
                object value = col.Value;
                //object value2 = col.Value2; // not using for now

                if (value != null)
                {
                    // The RawColumnName already removed new lines, trim, and convert to lower case (refer to UpdateMappingBos() method in this class)
                    // Thus, the value from cell or text file should remove the new lines, trim, and convert to lower case
                    var header = value.ToString().Trim().ToLower().RemoveNewLines();

                    var column = Columns.Where(q => q.Header == header).FirstOrDefault();
                    if (column != null)
                    {
                        column.ColIndex = colIndex;
                    }
                }
            }

            if (Columns.Any(q => !q.ColIndex.HasValue))
            {
                throw new Exception("Incorrect headers in uploaded file");
            }
        }
    }
}
