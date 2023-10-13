using BusinessObject.Claims;
using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConsoleApp.Commands.RawFiles.ClaimData
{
    public class LogClaimDataFile
    {
        public ClaimDataFileBo ClaimDataFileBo { get; set; }

        public int ProcessDataRow { get; set; } = 0;
        public int ClaimDataCount { get; set; } = 0;

        public int MappingErrorCount { get; set; } = 0;
        public int FormulaErrorCount { get; set; } = 0;
        public int TreatyCodeMappingErrorCount { get; set; } = 0;
        public int PreComputationErrorCount { get; set; } = 0;
        public int PreValidationErrorCount { get; set; } = 0;

        public string FileError { get; set; }

        public Stopwatch SwRead { get; set; } = new Stopwatch();
        public Stopwatch SwMapping { get; set; } = new Stopwatch();
        public Stopwatch SwProcess { get; set; } = new Stopwatch();

        public Stopwatch SwMappingDetail { get; set; } = new Stopwatch();
        public Stopwatch SwMappingDetail1 { get; set; } = new Stopwatch();
        public Stopwatch SwMappingDetail2 { get; set; } = new Stopwatch();
        public Stopwatch SwMappingDetail3 { get; set; } = new Stopwatch();
        public Stopwatch SwFixedValue { get; set; } = new Stopwatch();
        public Stopwatch SwOverrideProperties { get; set; } = new Stopwatch();
        public Stopwatch SwPreComputation { get; set; } = new Stopwatch();
        public Stopwatch SwTreatyMapping { get; set; } = new Stopwatch();
        public Stopwatch SwPreValidation { get; set; } = new Stopwatch();

        public Stopwatch SwSave { get; set; } = new Stopwatch();
        public Stopwatch SwFile { get; set; } = new Stopwatch();

        public TimeSpan TsMappingDetail { get; set; }
        public TimeSpan TsMappingDetail1 { get; set; }
        public TimeSpan TsMappingDetail2 { get; set; }
        public TimeSpan TsMappingDetail3 { get; set; }
        public TimeSpan TsFixedValue { get; set; }
        public TimeSpan TsOverrideProperties { get; set; }
        public TimeSpan TsPreComputation { get; set; }
        public TimeSpan TsTreatyMapping { get; set; }
        public TimeSpan TsPreValidation { get; set; }

        public LogClaimDataFile(ClaimDataFileBo file)
        {
            ClaimDataFileBo = file;
        }

        public void AddCount(LogClaimDataFile file)
        {
            ProcessDataRow += file.ProcessDataRow;
            ClaimDataCount += file.ClaimDataCount;

            MappingErrorCount += file.MappingErrorCount;
            PreComputationErrorCount += file.PreComputationErrorCount;
            PreValidationErrorCount += file.PreValidationErrorCount;
        }

        public void AddElapsedDetails(LogClaimDataFile file)
        {
            TsMappingDetail = file.SwMappingDetail.Elapsed.Add(TsMappingDetail);
            TsMappingDetail1 = file.SwMappingDetail1.Elapsed.Add(TsMappingDetail1);
            TsMappingDetail2 = file.SwMappingDetail2.Elapsed.Add(TsMappingDetail2);
            TsMappingDetail3 = file.SwMappingDetail3.Elapsed.Add(TsMappingDetail3);
            TsFixedValue = file.SwFixedValue.Elapsed.Add(TsFixedValue);
            TsOverrideProperties = file.SwOverrideProperties.Elapsed.Add(TsOverrideProperties);
            TsPreComputation = file.SwPreValidation.Elapsed.Add(TsPreComputation);
            TsTreatyMapping = file.SwTreatyMapping.Elapsed.Add(TsTreatyMapping);
            TsPreValidation = file.SwPreValidation.Elapsed.Add(TsPreValidation);
        }

        public int GetTotalErrorCount()
        {
            return MappingErrorCount + PreComputationErrorCount + PreValidationErrorCount;
        }

        public string GetTotalClaimData()
        {
            int w = GetWidth();
            return string.Format("{0}: {1}", "Total Number Of Claim Data".PadRight(w, ' '), ClaimDataCount);
        }

        public List<string> GetInfo()
        {
            int w = GetWidth();
            var list = new List<string>
            {
                "".PadRight(GetLineWidth(), '-'),
                string.Format("{0}: {1}", "Claim Data Batch ID".PadRight(w, ' '), ClaimDataFileBo.ClaimDataBatchId),
                string.Format("{0}: {1}", "Claim Data File ID".PadRight(w, ' '), ClaimDataFileBo.Id),
                string.Format("{0}: {1}", "File Name".PadRight(w, ' '), ClaimDataFileBo.RawFileBo.FileName),
                string.Format("{0}: {1}", "Excluded".PadRight(w, ' '), ClaimDataFileBo.Mode == ClaimDataFileBo.ModeExclude ? "YES" : "NO"),
                string.Format("{0}: {1}", "Total processed row".PadRight(w, ' '), ProcessDataRow),
                GetTotalClaimData(),
            };
            return list;
        }

        public List<string> GetCount()
        {
            int w = GetWidth();
            return new List<string>
            {
                string.Format("{0}: {1}", "Total Mapping Error".PadRight(w, ' '), MappingErrorCount),
                string.Format("{0}: {1}", "Total Pre-Computation Error".PadRight(w, ' '), PreComputationErrorCount),
                string.Format("{0}: {1}", "Total Pre-Validation Error".PadRight(w, ' '), PreValidationErrorCount),
                "",
                string.Format("{0}: {1}", "  Total Formula error".PadRight(w, ' '), FormulaErrorCount),
                string.Format("{0}: {1}", "  Total Treaty Code mapping error".PadRight(w, ' '), TreatyCodeMappingErrorCount),
            };
        }

        public List<string> GetDetails()
        {
            int w = GetWidth();
            var list = new List<string> { };

            list.AddRange(GetInfo());
            list.AddRange(GetCount());
            list.AddRange(GetElapsedTime());
            list.AddRange(GetElapsedTimeDetails());

            if (!string.IsNullOrEmpty(FileError))
            {
                list.Add(string.Format("{0}: {1}", "File Error".PadRight(w, ' '), FileError));
            }

            return list;
        }

        public List<string> GetElapsedTime()
        {
            int w = GetWidth();
            return new List<string>
            {
                "Elapsed Time".PadBoth(GetLineWidth(), '*'),
                string.Format("{0}: {1}", "Read Lines".PadRight(w, ' '), SwRead.Elapsed.ToString()),
                string.Format("{0}: {1}", "Mapping Lines".PadRight(w, ' '), SwMapping.Elapsed.ToString()),
                string.Format("{0}: {1}", "Process Lines".PadRight(w, ' '), SwProcess.Elapsed.ToString()),
                string.Format("{0}: {1}", "Save".PadRight(w, ' '), SwSave.Elapsed.ToString()),
                string.Format("{0}: {1}", "File".PadRight(w, ' '), SwFile.Elapsed.ToString()),
            };
        }

        public List<string> GetElapsedTimeDetails()
        {
            int w = GetWidth();
            return new List<string>
            {
                "Elapsed Time In Details".PadBoth(GetLineWidth(), '*'),
                //string.Format("{0}: {1}", "Mapping Detail".PadRight(w, ' '), TsMapping.ToString()),
                //string.Format("  {0}: {1}", "Mapping Detail 1".PadRight(w-2, ' '), TsMapping1.ToString()),
                //string.Format("  {0}: {1}", "Mapping Detail 2".PadRight(w-2, ' '), TsMapping2.ToString()),
                //string.Format("  {0}: {1}", "Mapping Detail 3".PadRight(w-2, ' '), TsMapping3.ToString()),
                string.Format("{0}: {1}", "Fixed Value".PadRight(w, ' '), TsFixedValue.ToString()),
                string.Format("{0}: {1}", "Override Properties".PadRight(w, ' '), TsOverrideProperties.ToString()),
                string.Format("{0}: {1}", "Pre-Computation".PadRight(w, ' '), TsPreComputation.ToString()),
                string.Format("{0}: {1}", "Pre-Validation".PadRight(w, ' '), TsPreValidation.ToString()),
                string.Format("{0}: {1}", "Total".PadRight(w, ' '), GetTotalElapsedTimeDetails().ToString()),
            };
        }

        public TimeSpan GetTotalElapsedTimeDetails()
        {
            var ts = TsMappingDetail;
            ts = ts.Add(TsMappingDetail1);
            ts = ts.Add(TsMappingDetail2);
            ts = ts.Add(TsMappingDetail3);
            ts = ts.Add(TsFixedValue);
            ts = ts.Add(TsOverrideProperties);
            ts = ts.Add(TsPreComputation);
            ts = ts.Add(TsPreValidation);
            return ts;
        }

        public static int GetWidth(int w = 36)
        {
            return w;
        }

        public static int GetLineWidth(int w = 80)
        {
            return w;
        }
    }
}
