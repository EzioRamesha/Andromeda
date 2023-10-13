using BusinessObject.Identity;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Row = Shared.ProcessFile.Row;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingRateTableGroupBo
    {
        public int Id { get; set; }

        public int TreatyPricingCedantId { get; set; }

        public TreatyPricingCedantBo TreatyPricingCedantBo { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string FileName { get; set; }

        public string HashFileName { get; set; }

        public int NoOfRateTable { get; set; }

        public int Status { get; set; }

        public string StatusName { get; set; }

        public string Errors { get; set; }

        public string FormattedErrors { get; set; }

        public DateTime UploadedAt { get; set; }

        public string UploadedAtStr { get; set; }

        public int UploadedById { get; set; }

        public UserBo UploadedByBo { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public const int StatusPending = 1; 
        public const int StatusProcessing = 2; 
        public const int StatusSuccess = 3; 
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
                case StatusPending:
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
            return Util.GetTreatyPricingRateTableGroupUploadPath();
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
