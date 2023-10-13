using BusinessObject.Identity;
using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class RetroRegisterFileBo
    {
        public int Id { get; set; }

        public int RetroRegisterId { get; set; }

        public virtual RetroRegisterBo RetroRegisterBo { get; set; }

        public string FileName { get; set; }

        public string HashFileName { get; set; }

        public int Status { get; set; }

        public string Errors { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedAtStr { get; set; }

        public int CreatedById { get; set; }

        public virtual UserBo CreatedByBo { get; set; }

        public int? UpdatedById { get; set; }

        public const int StatusPending = 1;
        public const int StatusProcessing = 2;
        public const int StatusCompleted = 3;
        public const int StatusCompletedFailed = 4;

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
                case StatusCompletedFailed:
                    return "Completed- Failed";
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
                case StatusCompletedFailed:
                    return "status-fail-badge";
                default:
                    return "";
            }
        }

        public string GetLocalPathE2()
        {
            return Path.Combine(Util.GetE2Path(), HashFileName);
        }

        public string FormatHashFileName()
        {
            HashFileName = Hash.HashFileName(FileName);
            return HashFileName;
        }
    }
}
