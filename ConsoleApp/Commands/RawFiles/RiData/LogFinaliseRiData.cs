using BusinessObject.RiDatas;
using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConsoleApp.Commands.RawFiles.RiData
{
    public class LogFinaliseRiData
    {
        public RiDataBatchBo RiDataBatchBo { get; set; }
        public int Total { get; set; } = 0;
        public int Ignored { get; set; } = 0;
        public int Success { get; set; } = 0;
        public int Failed { get; set; } = 0;

        public Stopwatch SwFinalise { get; set; } = new Stopwatch();

        public LogFinaliseRiData()
        {
        }

        public LogFinaliseRiData(RiDataBatchBo batch)
        {
            RiDataBatchBo = batch;
        }

        public static int GetWidth(int w = 23)
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
            var lines = new List<string> {
                "".PadLeft(line, '='),
                string.Format("{0}: {1}", "RI Data Batch ID".PadRight(w, ' '), RiDataBatchBo.Id),
                string.Format("{0}: {1}", "Total number of RI Data".PadRight(w, ' '), Total),
                string.Format("{0}: {1}", "Total number of ignored".PadRight(w, ' '), Ignored),
                string.Format("{0}: {1}", "Total number of success".PadRight(w, ' '), Success),
                string.Format("{0}: {1}", "Total number of failed".PadRight(w, ' '), Failed),
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
                string.Format("{0}: {1}", "Finalise".PadRight(w, ' '), SwFinalise.Elapsed),
            };
        }
    }
}
