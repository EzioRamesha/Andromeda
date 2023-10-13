using BusinessObject.Claims;
using BusinessObject.RiDatas;
using Services.Claims;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;
using Row = Shared.ProcessFile.Row;

namespace ConsoleApp.Commands.RawFiles.ClaimData
{
    public class MappingClaimData : Command
    {
        public ProcessClaimDataBatch ProcessClaimDataBatch { get; set; }
        public LogClaimDataFile LogClaimDataFile { get; set; }
        public List<ClaimDataBo> ClaimDataBos { get; set; }
        public Row Row { get; set; }

        public MappingClaimData(ProcessClaimDataBatch batch, Row row)
        {
            ProcessClaimDataBatch = batch;
            Row = row;
        }

        public void MappingRow()
        {
            LogClaimDataFile = new LogClaimDataFile(ProcessClaimDataBatch.ClaimDataFileBo);

            int rowIndex = Row.RowIndex;
            int rowType = ProcessClaimDataBatch.ClaimDataFileConfigFound.DefineRowType(rowIndex);
            if (rowType == 0)
                return;

            ClaimDataBos = new List<ClaimDataBo> { };
            switch (rowType)
            {
                case RiDataFileConfig.ROW_TYPE_DATA:
                    if (ProcessClaimDataBatch.DataFile is TextFile && ProcessClaimDataBatch.ClaimDataFileConfigFound.Delimiter != RiDataConfigBo.DelimiterFixedLength)
                    {
                        // DO NOT process data if no delimiter contain
                        if (!Row.IsLineContainDelimiter)
                            return;
                    }
                    // pre create ri data
                    int number = ProcessClaimDataBatch.ClaimDataFileConfigFound.IsAbleColumnToRowMapping() ? ProcessClaimDataBatch.ClaimDataFileConfigFound.NumberOfRowMapping.Value : 1;
                    for (int i = 0; i < number; i++)
                    {
                        ClaimDataBos.Add(new ClaimDataBo() { });
                    }
                    break;
            }

            foreach (var col in Row.Columns)
            {
                switch (rowType)
                {
                    case RiDataFileConfig.ROW_TYPE_HEADER:
                        break;
                    case RiDataFileConfig.ROW_TYPE_DATA:
                        ProcessData(col);
                        break;
                }
            }
        }

        public bool ProcessData(Column col)
        {
            int? colIndex = col.ColIndex;
            object value = col.Value;
            object value2 = col.Value2;

            //LogRiDataFile.SwMappingDetail.Start();

            var mappings = ProcessClaimDataBatch.ClaimDataMappingBos.Where(q => q.Col == colIndex).ToList();
            if (mappings != null && mappings.Count > 0)
            {
                foreach (var mapping in mappings)
                {
                    if (mapping.Row == 0 || mapping.Row == null)
                    {
                        //LogRiDataFile.SwMappingDetail1.Start();

                        // All rows
                        foreach (ClaimDataBo claimData in ClaimDataBos)
                        {
                            bool success = claimData.ProcessClaimData(mapping, value);
                            if (!success)
                            {
                                LogClaimDataFile.MappingErrorCount++;
                            }
                        }

                        //LogRiDataFile.SwMappingDetail1.Stop();
                    }
                    else if (ClaimDataBos.Count > mapping.Row.Value - 1)
                    {
                        //LogRiDataFile.SwMappingDetail2.Start();

                        bool success = ClaimDataBos[mapping.Row.Value - 1].ProcessClaimData(mapping, value);
                        if (!success)
                        {
                            LogClaimDataFile.MappingErrorCount++;
                        }

                        //LogRiDataFile.SwMappingDetail2.Stop();
                    }
                }
            }
            else
            {
                if (value == null || value is string @string && string.IsNullOrEmpty(@string))
                    return false;

                //LogRiDataFile.SwMappingDetail3.Start();

                // Insert into Custom Field
                foreach (ClaimDataBo claimData in ClaimDataBos)
                {
                    bool success = claimData.ProcessClaimData(ClaimDataMappingService.GetCustomFieldMapping(colIndex.ToString(), colIndex), value);
                    if (!success)
                    {
                        LogClaimDataFile.MappingErrorCount++;
                    }
                }

                //LogRiDataFile.SwMappingDetail3.Stop();
            }

            //LogRiDataFile.SwMappingDetail.Stop();
            return true;
        }
    }
}
