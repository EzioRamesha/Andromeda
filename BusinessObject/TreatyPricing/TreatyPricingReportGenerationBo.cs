using Shared;
using System;
using System.IO;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingReportGenerationBo
    {
        public int Id { get; set; }

        public string ReportName { get; set; }

        public string ReportParams { get; set; }

        public string FileName { get; set; }

        public string HashFileName { get; set; }

        public int Status { get; set; }

        public string Errors { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string StatusName { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedAtStr { get; set; }

        public const int StatusPending = 1;
        public const int StatusProcessing = 2;
        public const int StatusCompleted = 3;
        public const int StatusFailed = 4;

        public const int StatusMax = 4;

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusPending:
                    return "Pending";
                case StatusProcessing:
                    return "Processing";
                case StatusCompleted:
                    return "Completed";
                case StatusFailed:
                    return "Failed";
                default:
                    return "";
            }
        }

        public static string GetStatusClass(int key)
        {
            switch (key)
            {
                case StatusPending:
                    return "status-pending-badge";
                case StatusProcessing:
                    return "status-processing-badge";
                case StatusCompleted:
                    return "status-success-badge";
                case StatusFailed:
                    return "status-fail-badge";
                default:
                    return "";
            }
        }

        public static string GetLocalDirectory()
        {
            return Util.GetTreatyPricingReportGenerationPath();
        }

        public string GetLocalPath()
        {
            return Path.Combine(GetLocalDirectory(), HashFileName);
        }

        public string FormatHashFileName()
        {
            HashFileName = Hash.HashFileName(FileName);
            return HashFileName;
        }
    }
}
