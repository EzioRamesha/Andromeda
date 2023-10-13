using BusinessObject;
using BusinessObject.RiDatas;
using Services.RiDatas;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;
using Row = Shared.ProcessFile.Row;

namespace ConsoleApp.Commands.RawFiles.RiData
{
    public class MappingRiData : Command
    {
        public ProcessRiDataBatch ProcessRiDataBatch { get; set; }
        public LogRiDataFile LogRiDataFile { get; set; }
        public List<RiDataBo> RiDataBos { get; set; }

        public Row Row { get; set; }

        public MappingRiData(ProcessRiDataBatch batch)
        {
            ProcessRiDataBatch = batch;
        }

        public MappingRiData(ProcessRiDataBatch batch, LogRiDataFile logRiDataFile, List<RiDataBo> riDataBos)
        {
            ProcessRiDataBatch = batch;
            LogRiDataFile = logRiDataFile;
            RiDataBos = riDataBos;
        }

        public MappingRiData(ProcessRiDataBatch batch, Row row) : this(batch)
        {
            Row = row;
        }

        public void MappingRow()
        {
            LogRiDataFile = new LogRiDataFile(ProcessRiDataBatch.RiDataFileBo);

            int rowIndex = Row.RowIndex;
            int rowType = ProcessRiDataBatch.RiDataFileBo.RiDataFileConfig.DefineRowType(rowIndex);
            if (rowType == 0)
                return;

            RiDataBos = new List<RiDataBo> { };
            switch (rowType)
            {
                case RiDataFileConfig.ROW_TYPE_DATA:
                    // DO NOT process data if no delimiter contain
                    if (ProcessRiDataBatch.DataFile is TextFile && ProcessRiDataBatch.RiDataFileBo.RiDataFileConfig.Delimiter != RiDataConfigBo.DelimiterFixedLength)
                        if (!Row.IsLineContainDelimiter)
                            return;

                    // Create RI Data
                    // Refer from RiDataConfigs.Configs (NOT RiDataFiles.Configs)
                    int number = ProcessRiDataBatch.RiDataConfigBo.RiDataFileConfig.IsAbleColumnToRowMapping() ? ProcessRiDataBatch.RiDataConfigBo.RiDataFileConfig.NumberOfRowMapping.Value : 1;
                    for (int i = 1; i <= number; i++)
                    {
                        var riDataBo = new RiDataBo
                        {
                            RecordType = ProcessRiDataBatch.RiDataBatchBo.RecordType
                        };

                        riDataBo.Log.SetEnabled(ProcessRiDataBatch.MaxDebugRows > 0);
                        riDataBo.Log.SetDetailWidth(ProcessRiDataBatch.DetailWidth);

                        riDataBo.Log.Row = Row.RowIndex;
                        riDataBo.Log.Index = i;
                        riDataBo.Log.RiDataBatchId = ProcessRiDataBatch.RiDataBatchBo.Id;
                        riDataBo.Log.RiDataFileId = ProcessRiDataBatch.RiDataFileBo.Id;

                        RiDataBos.Add(riDataBo);
                    }
                    break;
            }

            foreach (var column in Row.Columns)
            {
                switch (rowType)
                {
                    case RiDataFileConfig.ROW_TYPE_HEADER: // This already process at ProcessRiDataBatch
                        break;
                    case RiDataFileConfig.ROW_TYPE_DATA:
                        ProcessData(column);
                        break;
                }
            }
        }

        public void ProcessData(Column column)
        {
            var colIndex = column.ColIndex;
            var value = column.Value;
            var value2 = column.Value2;

            //LogRiDataFile.SwMappingDetail.Start();

            var mappings = ProcessRiDataBatch.RiDataMappingBos.Where(q => q.Col == colIndex).ToList();
            if (mappings != null && mappings.Count > 0)
            {
                foreach (var mapping in mappings)
                {
                    if (mapping.Row.HasValue && mapping.Row > 0)
                    {
                        //LogRiDataFile.SwMappingDetail1.Start();

                        // Specific row
                        var riDataBo = RiDataBos[mapping.Row.Value - 1];
                        bool success = riDataBo.ProcessRiData(mapping, value, out StandardOutputBo so, out object before, out object after, out string error);
                        riDataBo.Log.Mapping.LineDelimiter();
                        riDataBo.Log.Mapping.Property(so.Property);
                        riDataBo.Log.Mapping.Detail("SortIndex", mapping.SortIndex);
                        riDataBo.Log.Mapping.Detail("RawColumnName", mapping.RawColumnName);
                        riDataBo.Log.Mapping.Detail("Length", mapping.Length);
                        riDataBo.Log.Mapping.Detail("TransformFormulaName", mapping.TransformFormulaName);

                        if (value == null || value is string @string && string.IsNullOrEmpty(@string))
                        {
                            riDataBo.Log.Mapping.ValueIsNull();
                            continue;
                        }

                        if (success)
                        {
                            riDataBo.Log.Mapping.Before(before);
                            riDataBo.Log.Mapping.After(after);
                        }
                        else
                        {
                            riDataBo.Log.Mapping.Error(error);
                            LogRiDataFile.MappingErrorCount++;
                        }

                        //LogRiDataFile.SwMappingDetail1.Stop();
                    }
                    else if (!RiDataBos.IsNullOrEmpty() && RiDataBos.Count > 0)
                    {
                        //LogRiDataFile.SwMappingDetail2.Start();

                        // All RI Data
                        foreach (var riDataBo in RiDataBos)
                        {
                            bool success = riDataBo.ProcessRiData(mapping, value, out StandardOutputBo so, out object before, out object after, out string error);
                            riDataBo.Log.Mapping.LineDelimiter();
                            riDataBo.Log.Mapping.Property(so.Property);
                            riDataBo.Log.Mapping.Detail("SortIndex", mapping.SortIndex);
                            riDataBo.Log.Mapping.Detail("RawColumnName", mapping.RawColumnName);
                            riDataBo.Log.Mapping.Detail("Length", mapping.Length);
                            riDataBo.Log.Mapping.Detail("TransformFormulaName", mapping.TransformFormulaName);

                            if (value == null || value is string @string && string.IsNullOrEmpty(@string))
                            {
                                riDataBo.Log.Mapping.ValueIsNull();
                                continue;
                            }

                            if (success)
                            {
                                riDataBo.Log.Mapping.Before(before);
                                riDataBo.Log.Mapping.After(after);
                            }
                            else
                            {
                                riDataBo.Log.Mapping.Error(error);
                                LogRiDataFile.MappingErrorCount++;
                            }
                        }

                        //LogRiDataFile.SwMappingDetail2.Stop();
                    }
                }
            }
            else
            {
                //LogRiDataFile.SwMappingDetail3.Start();

                // All RI Data Insert into Custom Field
                var mapping = RiDataMappingService.GetCustomFieldMapping(colIndex.ToString(), colIndex);
                foreach (var riDataBo in RiDataBos)
                {
                    bool success = riDataBo.ProcessRiData(mapping, value, out StandardOutputBo so, out object before, out object after, out string error);
                    riDataBo.Log.MappingCustomField.LineDelimiter();
                    riDataBo.Log.MappingCustomField.Property(mapping.RawColumnName);

                    if (value == null || value is string @string && string.IsNullOrEmpty(@string))
                    {
                        riDataBo.Log.MappingCustomField.ValueIsNull();
                        continue;
                    }

                    if (success)
                    {
                        riDataBo.Log.MappingCustomField.Detail("Value", after);
                    }
                    else
                    {
                        riDataBo.Log.MappingCustomField.Error(error);
                        LogRiDataFile.MappingErrorCount++;
                    }
                }

                //LogRiDataFile.SwMappingDetail3.Stop();
            }

            //LogRiDataFile.SwMappingDetail.Stop();
        }
    }
}
