using Shared;
using System;
using System.IO;

namespace BusinessObject
{
    public class TreatyBenefitCodeMappingUploadBo
    {
        public int Id { get; set; }

        public int Status { get; set; }

        public string FileName { get; set; }

        public string HashFileName { get; set; }

        public string Errors { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public DateTime UpdatedAt { get; set; }

        public const int StatusPendingProcess = 1;
        public const int StatusProcessing = 2;
        public const int StatusSuccess = 3;
        public const int StatusFailed = 4;
        public const int StatusMax = 4;

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusPendingProcess:
                    return "Pending Processing";
                case StatusProcessing:
                    return "Processing";
                case StatusSuccess:
                    return "Success";
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
                case StatusPendingProcess:
                    return "status-pending-badge";
                case StatusProcessing:
                    return "status-processing-badge";
                case StatusSuccess:
                    return "status-success-badge";
                case StatusFailed:
                    return "status-fail-badge";
                default:
                    return "";
            }
        }

        public string GetLocalDirectory()
        {
            return Util.GetTreatyBenefitCodeMappingPath();
        }

        public string GetLocalPath(bool useFileName = false)
        {
            if (useFileName)
                return Path.Combine(GetLocalDirectory(), FileName);
            else
                return Path.Combine(GetLocalDirectory(), HashFileName);
        }

        public string FormatHashFileName()
        {
            HashFileName = Hash.HashFileName(FileName);
            return HashFileName;
        }
    }
}
