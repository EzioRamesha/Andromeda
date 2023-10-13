using BusinessObject.Identity;
using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.InvoiceRegisters
{
    public class InvoiceRegisterBatchFileBo
    {
        public int Id { get; set; }

        public int InvoiceRegisterBatchId { get; set; }

        public virtual InvoiceRegisterBatchBo InvoiceRegisterBatchBo { get; set; }

        public string FileName { get; set; }

        public string HashFileName { get; set; }

        public int Type { get; set; }

        public int Status { get; set; }

        public string Errors { get; set; }

        public bool DataUpdate { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedAtStr { get; set; }

        public int CreatedById { get; set; }

        public virtual UserBo CreatedByBo { get; set; }

        public int? UpdatedById { get; set; }

        public const int TypeWM = 1;
        public const int TypeCNWM = 2;
        public const int TypeDNWM = 3;
        public const int TypeOM = 4;
        public const int TypeCNOM = 5;
        public const int TypeDNOM = 6;
        public const int TypeSFWM = 7;
        public const int TypeCNSFWM = 8;
        public const int TypeDNSFWM = 9;
        public const int TypeMax = 9;

        public const int StatusPending = 1;
        public const int StatusProcessing = 2;
        public const int StatusCompleted = 3;
        public const int StatusCompletedFailed = 4;

        public static string GetTypeName(int key)
        {
            switch (key)
            {
                case TypeWM:
                    return "WM";
                case TypeCNWM:
                    return "CNWM";
                case TypeDNWM:
                    return "DNWM";
                case TypeOM:
                    return "OM";
                case TypeCNOM:
                    return "CNOM";
                case TypeDNOM:
                    return "DNOM";
                case TypeSFWM:
                    return "SFWM";
                case TypeCNSFWM:
                    return "CNSFWM";
                case TypeDNSFWM:
                    return "DNSFWM";
                default:
                    return "";
            }
        }

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

        public string GetLocalDirectory()
        {
            return Util.GetRawFilePath("InvoiceRegister");
        }

        public string GetLocalPathE1()
        {
            return Path.Combine(Util.GetE1Path(), HashFileName);
        }

        public string GetLocalPath()
        {
            return Path.Combine(GetLocalDirectory(), HashFileName);
        }

        public string GetTemporaryDirectory()
        {
            return Util.GetTemporaryPath("InvoiceRegister");
        }

        public string GetTemporaryPath()
        {
            return Path.Combine(GetTemporaryDirectory(), HashFileName);
        }

        public string FormatHashFileName()
        {
            HashFileName = Hash.HashFileName(FileName);
            return HashFileName;
        }

        public static string GetTempFolderPath(string subFolder = null)
        {
            return Util.GetTemporaryPath(subFolder);
        }

        public string GetTempPath(string subFolder = null)
        {
            return Path.Combine(GetTempFolderPath(subFolder), HashFileName);
        }
    }
}
