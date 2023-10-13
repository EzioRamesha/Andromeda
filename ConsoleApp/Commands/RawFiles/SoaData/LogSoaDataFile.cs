using BusinessObject.RiDatas;
using BusinessObject.SoaDatas;
using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConsoleApp.Commands.RawFiles.SoaData
{
    public class LogSoaDataFile
    {
        public SoaDataFileBo SoaDataFileBo { get; set; }

        public int ProcessDataRow { get; set; } = 0;
        public int SoaDataCount { get; set; } = 0;

        public int MappingErrorCount { get; set; } = 0;

        public int PreValidationErrorCount { get; set; } = 0;
        public int PostValidationErrorCount { get; set; } = 0;

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
        public Stopwatch SwDataCorrection { get; set; } = new Stopwatch();
        public Stopwatch SwPreValidation { get; set; } = new Stopwatch();
        public Stopwatch SwPostValidation { get; set; } = new Stopwatch();

        public Stopwatch SwSave { get; set; } = new Stopwatch();
        public Stopwatch SwFile { get; set; } = new Stopwatch();

        public TimeSpan TsMappingDetail { get; set; }
        public TimeSpan TsMappingDetail1 { get; set; }
        public TimeSpan TsMappingDetail2 { get; set; }
        public TimeSpan TsMappingDetail3 { get; set; }
        public TimeSpan TsFixedValue { get; set; }
        public TimeSpan TsOverrideProperties { get; set; }
        public TimeSpan TsDataCorrection { get; set; }
        public TimeSpan TsPreValidation { get; set; }
        public TimeSpan TsPostValidation { get; set; }

        public LogSoaDataFile(SoaDataFileBo file)
        {
            SoaDataFileBo = file;
        }

        public void AddCount(LogSoaDataFile file)
        {
            ProcessDataRow += file.ProcessDataRow;
            SoaDataCount += file.SoaDataCount;
            MappingErrorCount += file.MappingErrorCount;
            PreValidationErrorCount += file.PreValidationErrorCount;
            PostValidationErrorCount += file.PostValidationErrorCount;
        }

        public void AddElapsedDetails(LogSoaDataFile file)
        {
            TsMappingDetail = file.SwMappingDetail.Elapsed.Add(TsMappingDetail);
            TsMappingDetail1 = file.SwMappingDetail1.Elapsed.Add(TsMappingDetail1);
            TsMappingDetail2 = file.SwMappingDetail2.Elapsed.Add(TsMappingDetail2);
            TsMappingDetail3 = file.SwMappingDetail3.Elapsed.Add(TsMappingDetail3);
            TsFixedValue = file.SwFixedValue.Elapsed.Add(TsFixedValue);
            TsOverrideProperties = file.SwOverrideProperties.Elapsed.Add(TsOverrideProperties);
            TsDataCorrection = file.SwDataCorrection.Elapsed.Add(TsDataCorrection);
            TsPreValidation = file.SwPreValidation.Elapsed.Add(TsPreValidation);
            TsPostValidation = file.SwPostValidation.Elapsed.Add(TsPostValidation);
        }

        public int GetTotalErrorCount()
        {
            return MappingErrorCount + PreValidationErrorCount + PostValidationErrorCount;
        }

        public string GetTotalSoaData()
        {
            int w = GetWidth();
            return string.Format("{0}: {1}", "Total number of RI Data".PadRight(w, ' '), SoaDataCount);
        }

        public List<string> GetInfo()
        {
            int w = GetWidth();
            var list = new List<string>
            {
                "".PadRight(GetLineWidth(), '-'),
                string.Format("{0}: {1}", "RI Data Batch ID".PadRight(w, ' '), SoaDataFileBo.SoaDataBatchId),
                string.Format("{0}: {1}", "RI Data File ID".PadRight(w, ' '), SoaDataFileBo.Id),
                string.Format("{0}: {1}", "File Name".PadRight(w, ' '), SoaDataFileBo.RawFileBo.FileName),
                string.Format("{0}: {1}", "Excluded".PadRight(w, ' '), SoaDataFileBo.Mode == SoaDataFileBo.ModeExclude ? "YES" : "NO"),
                string.Format("{0}: {1}", "Total processed row".PadRight(w, ' '), ProcessDataRow),
                GetTotalSoaData(),
            };
            return list;
        }

        public List<string> GetCount()
        {
            int w = GetWidth();
            return new List<string>
            {
                string.Format("{0}: {1}", "Total Mapping error".PadRight(w, ' '), MappingErrorCount),
                string.Format("{0}: {1}", "Total Pre-Validation error".PadRight(w, ' '), PreValidationErrorCount),
                string.Format("{0}: {1}", "Total Post-Validation error".PadRight(w, ' '), PostValidationErrorCount),
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
                //string.Format("{0}: {1}", "Mapping Detail".PadRight(w, ' '), TsMappingDetail.ToString()),
                //string.Format("  {0}: {1}", "Mapping Detail 1".PadRight(w-2, ' '), TsMappingDetail1.ToString()),
                //string.Format("  {0}: {1}", "Mapping Detail 2".PadRight(w-2, ' '), TsMappingDetail2.ToString()),
                //string.Format("  {0}: {1}", "Mapping Detail 3".PadRight(w-2, ' '), TsMappingDetail3.ToString()),
                //string.Format("{0}: {1}", "Fixed Value".PadRight(w, ' '), TsFixedValue.ToString()),
                //string.Format("{0}: {1}", "Override Properties".PadRight(w, ' '), TsOverrideProperties.ToString()),
                //string.Format("{0}: {1}", "Data Correction".PadRight(w, ' '), TsDataCorrection.ToString()),
                string.Format("{0}: {1}", "Pre-Validation".PadRight(w, ' '), TsPreValidation.ToString()),
                string.Format("{0}: {1}", "Post-Validation".PadRight(w, ' '), TsPostValidation.ToString()),
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
            ts = ts.Add(TsDataCorrection);
            ts = ts.Add(TsPreValidation);
            ts = ts.Add(TsPostValidation);
            return ts;
        }

        public static int GetWidth(int w = 45)
        {
            return w;
        }

        public static int GetLineWidth(int w = 80)
        {
            return w;
        }
    }
}
