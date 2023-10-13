using BusinessObject.Claims;
using Shared;
using System;
using System.Collections.Generic;

namespace ConsoleApp.Commands.RawFiles.ClaimData
{
    public class LogClaimDataBatch
    {
        public ClaimDataBatchBo ClaimDataBatchBo { get; set; }

        public List<LogClaimDataFile> LogClaimDataFiles { get; set; } = new List<LogClaimDataFile> { };

        public int ProcessDataRow { get; set; } = 0;
        public int ClaimDataCount { get; set; } = 0;

        public int MappingErrorCount { get; set; } = 0;
        public int PreComputationErrorCount { get; set; } = 0;
        public int PreValidationErrorCount { get; set; } = 0;
        public int PostComputationErrorCount { get; set; } = 0;
        public int PostValidationErrorCount { get; set; } = 0;

        public TimeSpan TsRead { get; set; } = new TimeSpan();
        public TimeSpan TsMapping { get; set; } = new TimeSpan();
        public TimeSpan TsProcess { get; set; } = new TimeSpan();

        public TimeSpan TsMappingDetail { get; set; } = new TimeSpan();
        public TimeSpan TsMappingDetail1 { get; set; } = new TimeSpan();
        public TimeSpan TsMappingDetail2 { get; set; } = new TimeSpan();
        public TimeSpan TsMappingDetail3 { get; set; } = new TimeSpan();
        public TimeSpan TsOverrideProperties { get; set; } = new TimeSpan();
        public TimeSpan TsPreComputation { get; set; } = new TimeSpan();
        public TimeSpan TsPreValidation { get; set; } = new TimeSpan();
        public TimeSpan TsSave { get; set; } = new TimeSpan();
        public TimeSpan TsAllFiles { get; set; } = new TimeSpan();

        public LogClaimDataBatch(ClaimDataBatchBo batch)
        {
            ClaimDataBatchBo = batch;
        }

        public static int GetWidth(int w = 36)
        {
            return w;
        }

        public static int GetLineWidth(int w = 80)
        {
            return w;
        }

        public List<string> GetDetails()
        {
            int w = GetWidth();
            int line = GetLineWidth();
            var lines = new List<string>
            {
                "".PadLeft(line, '='),
                "SUMMARY",
                "",
                string.Format("{0}: {1}", "Claim Data Batch ID".PadRight(w, ' '), ClaimDataBatchBo.Id),
                string.Format("{0}: {1}", "Total Processed Row".PadRight(w, ' '), ProcessDataRow),
                string.Format("{0}: {1}", "Total Number Of Claim Data".PadRight(w, ' '), ClaimDataCount),
                string.Format("{0}: {1}", "Total Mapping Error".PadRight(w, ' '), MappingErrorCount),
                string.Format("{0}: {1}", "Total Pre-Computaion Error".PadRight(w, ' '), PreComputationErrorCount),
                string.Format("{0}: {1}", "Total Pre-Validation Error".PadRight(w, ' '), PreValidationErrorCount),
            };

            lines.AddRange(GetElapsedTime());

            return lines;
        }

        public List<string> GetElapsedTime()
        {
            int w = GetWidth();
            int line = GetLineWidth();
            return new List<string>
            {
                "Elapsed Time".PadBoth(line, '*'),
                string.Format("{0}: {1}", "Read Lines".PadRight(w, ' '), TsRead.ToString()),
                string.Format("{0}: {1}", "Mapping Lines".PadRight(w, ' '), TsMapping.ToString()),
                string.Format("{0}: {1}", "Process Lines".PadRight(w, ' '), TsProcess.ToString()),
                //string.Format("{0}: {1}", "Mapping Detail".PadRight(w, ' '), TsMapping.ToString()),
                //string.Format("  {0}: {1}", "Mapping Detail 1".PadRight(w-2, ' '), TsMapping1.ToString()),
                //string.Format("  {0}: {1}", "Mapping Detail 2".PadRight(w-2, ' '), TsMapping2.ToString()),
                //string.Format("  {0}: {1}", "Mapping Detail 3".PadRight(w-2, ' '), TsMapping3.ToString()),
                //string.Format("{0}: {1}", "Override Properties".PadRight(w, ' '), TsOverrideProperties.ToString()),
                //string.Format("{0}: {1}", "Pre-Computaion".PadRight(w, ' '), TsPreComputaion.ToString()),
                //string.Format("{0}: {1}", "Pre-Validation".PadRight(w, ' '), TsPreValidation.ToString()),
                //string.Format("{0}: {1}", "Post-Computaion".PadRight(w, ' '), TsPostComputaion.ToString()),
                //string.Format("{0}: {1}", "Post-Validation".PadRight(w, ' '), TsPostValidation.ToString()),
                string.Format("{0}: {1}", "Save".PadRight(w, ' '), TsSave.ToString()),
                string.Format("{0}: {1}", "All Files".PadRight(w, ' '), TsAllFiles.ToString()),
            };
        }

        public void Add(LogClaimDataFile file)
        {
            LogClaimDataFiles.Add(file);

            ProcessDataRow += file.ProcessDataRow;
            ClaimDataCount += file.ClaimDataCount;
            MappingErrorCount += file.MappingErrorCount;
            PreComputationErrorCount += file.PreComputationErrorCount;
            PreValidationErrorCount += file.PreValidationErrorCount;

            TsRead = file.SwRead.Elapsed.Add(TsRead);
            TsMapping = file.SwMapping.Elapsed.Add(TsMapping);
            TsProcess = file.SwProcess.Elapsed.Add(TsProcess);

            TsMappingDetail = file.SwMappingDetail.Elapsed.Add(TsMappingDetail);
            TsMappingDetail1 = file.SwMappingDetail1.Elapsed.Add(TsMappingDetail1);
            TsMappingDetail2 = file.SwMappingDetail2.Elapsed.Add(TsMappingDetail2);
            TsMappingDetail3 = file.SwMappingDetail3.Elapsed.Add(TsMappingDetail3);
            TsOverrideProperties = file.SwOverrideProperties.Elapsed.Add(TsOverrideProperties);
            TsPreComputation = file.SwPreComputation.Elapsed.Add(TsPreComputation);
            TsPreValidation = file.SwPreValidation.Elapsed.Add(TsPreValidation);
            TsSave = file.SwSave.Elapsed.Add(TsSave);
            TsAllFiles = file.SwFile.Elapsed.Add(TsAllFiles);
        }
    }
}
