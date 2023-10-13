using Shared;
using System;
using System.IO;

namespace BusinessObject
{
    public class FinanceProvisioningBo
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int Status { get; set; }

        public int ClaimsProvisionRecord { get; set; }

        public double ClaimsProvisionAmount { get; set; }

        public int DrProvisionRecord { get; set; }

        public double DrProvisionAmount { get; set; }

        public DateTime? ProvisionAt { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public const int StatusPending = 1;
        public const int StatusProcessing = 2;
        public const int StatusProvisioned = 3;
        public const int StatusSubmitForProcessing = 4;
        public const int StatusFailed = 5;

        public const int MaxStatus = 5;

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusPending:
                    return "Pending";
                case StatusProcessing:
                    return "Processing";
                case StatusProvisioned:
                    return "Provisioned";
                case StatusSubmitForProcessing:
                    return "Submit For Processing";
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
                case StatusProvisioned:
                    return "status-success-badge";
                case StatusSubmitForProcessing:
                    return "status-submitprocess-badge";
                case StatusFailed:
                    return "status-fail-badge";
                default:
                    return "";
            }
        }

        public static bool IsE3Exist(int id)
        {
            string path = Util.GetE3Path(id.ToString());
            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(@path, "*");
                if (files != null && files.Length > 0)
                return true;
            }
            return false;
        }

        public static bool IsE4Exist(int id)
        {
            string path = Util.GetE4Path(id.ToString());
            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(@path, "*");
                if (files != null && files.Length > 0)
                return true;
            }
            return false;
        }
    }
}
