using System;
using System.Linq;

namespace BusinessObject
{
    public class RetroRegisterBatchBo
    {
        public int Id { get; set; }

        public int BatchNo { get; set; }

        public DateTime BatchDate { get; set; }

        public int Type { get; set; }

        public int TotalInvoice { get; set; }

        public int Status { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public const int StatusPending = 1;
        public const int StatusSubmitForProcessing = 2;
        public const int StatusProcessing = 3;
        public const int StatusSuccess = 4;
        public const int StatusFailed = 5;
        public const int StatusSubmitForGenerate = 6;
        public const int StatusGenerating = 7;
        public const int StatusGenerateComplete = 8;
        public const int StatusGenerateFailed = 9;

        public const int TypeDirectRetro = 1;
        public const int TypePerLifeRetro = 2;
        public const int TypeMax = 2;

        public const int StatusMax = 9;

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusPending:
                    return "Pending";
                case StatusSubmitForProcessing:
                    return "Submit For Processing";
                case StatusProcessing:
                    return "Processing";
                case StatusSuccess:
                    return "Success";
                case StatusFailed:
                    return "Failed";
                case StatusSubmitForGenerate:
                    return "Submit For Generate";
                case StatusGenerating:
                    return "Generating";
                case StatusGenerateComplete:
                    return "Generate Completed";
                case StatusGenerateFailed:
                    return "Generate Failed";
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
                case StatusSubmitForProcessing:
                case StatusSubmitForGenerate:
                    return "status-submitprocess-badge";
                case StatusProcessing:
                case StatusGenerating:
                    return "status-processing-badge";
                case StatusSuccess:
                case StatusGenerateComplete:
                    return "status-success-badge";
                case StatusFailed:
                case StatusGenerateFailed:
                    return "status-fail-badge";
                default:
                    return "";
            }
        }

        public static string GetTypeName(int key)
        {
            switch (key)
            {
                case TypeDirectRetro:
                    return "Direct Retro";
                case TypePerLifeRetro:
                    return "Per Life Retro";
                default:
                    return "";
            }
        }

        public static bool CanProcess(int status)
        {
            int[] statuses = { StatusPending, StatusSuccess, StatusFailed };
            return statuses.Contains(status);
        }

        public static bool CanGenerate(int status)
        {
            int[] statuses = { StatusSuccess, StatusGenerateComplete, StatusGenerateFailed };
            return statuses.Contains(status);
        }
    }    
}
