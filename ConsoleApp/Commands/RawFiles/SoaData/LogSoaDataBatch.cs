using BusinessObject.SoaDatas;
using Shared;
using System;
using System.Collections.Generic;

namespace ConsoleApp.Commands.RawFiles.SoaData
{
    public class LogSoaDataBatch
    {
        public SoaDataBatchBo SoaDataBatchBo { get; set; }
        public List<LogSoaDataFile> ProcessDataSoaFiles { get; set; } = new List<LogSoaDataFile> { };

        public int ProcessDataRow { get; set; } = 0;
        public int SoaDataCount { get; set; } = 0;

        public int MappingErrorCount { get; set; } = 0;

        public int PreValidationErrorCount { get; set; } = 0;
        public int PostValidationErrorCount { get; set; } = 0;

        public TimeSpan TsRead { get; set; } = new TimeSpan();
        public TimeSpan TsMapping { get; set; } = new TimeSpan();
        public TimeSpan TsProcess { get; set; } = new TimeSpan();

        public TimeSpan TsMappingDetail { get; set; } = new TimeSpan();
        public TimeSpan TsMappingDetail1 { get; set; } = new TimeSpan();
        public TimeSpan TsMappingDetail2 { get; set; } = new TimeSpan();
        public TimeSpan TsMappingDetail3 { get; set; } = new TimeSpan();
        public TimeSpan TsFixedValue { get; set; } = new TimeSpan();
        public TimeSpan TsOverrideProperties { get; set; } = new TimeSpan();
        public TimeSpan TsDataCorrection { get; set; } = new TimeSpan();
        public TimeSpan TsPreValidation { get; set; } = new TimeSpan();
        public TimeSpan TsPostValidation { get; set; } = new TimeSpan();
        public TimeSpan TsSave { get; set; } = new TimeSpan();
        public TimeSpan TsAllFiles { get; set; } = new TimeSpan();

        public LogSoaDataBatch(SoaDataBatchBo batch)
        {
            SoaDataBatchBo = batch;
        }

        public static int GetWidth(int w = 45)
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
                string.Format("{0}: {1}", "RI Data Batch ID".PadRight(w, ' '), SoaDataBatchBo.Id),
                string.Format("{0}: {1}", "Total processed row".PadRight(w, ' '), ProcessDataRow),
                string.Format("{0}: {1}", "Total number of RI Data".PadRight(w, ' '), SoaDataCount),
                string.Format("{0}: {1}", "Total Mapping error".PadRight(w, ' '), MappingErrorCount),
                string.Format("{0}: {1}", "Total Pre-Validation error".PadRight(w, ' '), PreValidationErrorCount),
                string.Format("{0}: {1}", "Total Post-Validation error".PadRight(w, ' '), PostValidationErrorCount),
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
                //string.Format("{0}: {1}", "Mapping Detail".PadRight(w, ' '), TsMappingDetail.ToString()),
                //string.Format("  {0}: {1}", "Mapping Detail 1".PadRight(w-2, ' '), TsMappingDetail1.ToString()),
                //string.Format("  {0}: {1}", "Mapping Detail 2".PadRight(w-2, ' '), TsMappingDetail2.ToString()),
                //string.Format("  {0}: {1}", "Mapping Detail 3".PadRight(w-2, ' '), TsMappingDetail3.ToString()),
                //string.Format("{0}: {1}", "Fixed Value".PadRight(w, ' '), TsFixedValue.ToString()),
                //string.Format("{0}: {1}", "Override Properties".PadRight(w, ' '), TsOverrideProperties.ToString()),
                //string.Format("{0}: {1}", "Data Correction".PadRight(w, ' '), TsDataCorrection.ToString()),
                string.Format("{0}: {1}", "Pre-Validation".PadRight(w, ' '), TsPreValidation.ToString()),
                string.Format("{0}: {1}", "Post-Validation".PadRight(w, ' '), TsPostValidation.ToString()),
                string.Format("{0}: {1}", "Save".PadRight(w, ' '), TsSave.ToString()),
                string.Format("{0}: {1}", "All Files".PadRight(w, ' '), TsAllFiles.ToString()),
            };
        }

        public void Add(LogSoaDataFile file)
        {
            ProcessDataSoaFiles.Add(file);

            ProcessDataRow += file.ProcessDataRow;
            SoaDataCount += file.SoaDataCount;
            MappingErrorCount += file.MappingErrorCount;
            PreValidationErrorCount += file.PreValidationErrorCount;
            PostValidationErrorCount += file.PostValidationErrorCount;

            TsRead = file.SwRead.Elapsed.Add(TsRead);
            TsMapping = file.SwMapping.Elapsed.Add(TsMapping);
            TsProcess = file.SwProcess.Elapsed.Add(TsProcess);

            TsMappingDetail = file.SwMappingDetail.Elapsed.Add(TsMappingDetail);
            TsMappingDetail1 = file.SwMappingDetail1.Elapsed.Add(TsMappingDetail1);
            TsMappingDetail2 = file.SwMappingDetail2.Elapsed.Add(TsMappingDetail2);
            TsMappingDetail3 = file.SwMappingDetail3.Elapsed.Add(TsMappingDetail3);
            TsFixedValue = file.SwFixedValue.Elapsed.Add(TsFixedValue);
            TsOverrideProperties = file.SwOverrideProperties.Elapsed.Add(TsOverrideProperties);
            TsDataCorrection = file.SwDataCorrection.Elapsed.Add(TsDataCorrection);
            TsPreValidation = file.SwPreValidation.Elapsed.Add(TsPreValidation);
            TsPostValidation = file.SwPostValidation.Elapsed.Add(TsPostValidation);
            TsSave = file.SwSave.Elapsed.Add(TsSave);
            TsAllFiles = file.SwFile.Elapsed.Add(TsAllFiles);
        }
    }
}
