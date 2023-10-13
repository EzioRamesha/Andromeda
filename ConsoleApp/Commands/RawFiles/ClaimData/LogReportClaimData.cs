using BusinessObject.Claims;
using Shared;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConsoleApp.Commands.RawFiles.ClaimData
{
    public class LogReportClaimData
    {
        public ClaimDataBatchBo ClaimDataBatchBo { get; set; }
        public int Total { get; set; } = 0;
        public Stopwatch SwReport { get; set; } = new Stopwatch();

        public LogReportClaimData()
        {
        }

        public LogReportClaimData(ClaimDataBatchBo batch)
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
            var lines = new List<string> {
                "".PadLeft(line, '='),
                string.Format("{0}: {1}", "Claim Data Batch ID".PadRight(w, ' '), ClaimDataBatchBo.Id),
                string.Format("{0}: {1}", "Total Number Of Claim Data".PadRight(w, ' '), Total),
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
                string.Format("{0}: {1}", "Report".PadRight(w, ' '), SwReport.Elapsed),
            };
        }
    }
}
