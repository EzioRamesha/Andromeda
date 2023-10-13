using BusinessObject.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Claims
{
    public class ClaimDataFileBo
    {
        public int Id { get; set; }

        public int ClaimDataBatchId { get; set; }

        public virtual ClaimDataBatchBo ClaimDataBatchBo { get; set; }

        public int RawFileId { get; set; }

        public virtual RawFileBo RawFileBo { get; set; }

        public int? TreatyId { get; set; }

        public virtual TreatyBo TreatyBo { get; set; }

        public int? ClaimDataConfigId { get; set; }

        public virtual ClaimDataConfigBo ClaimDataConfigBo { get; set; }

        public int? CurrencyCodeId { get; set; }

        public virtual PickListDetailBo CurrencyCodeBo { get; set; }

        public double? CurrencyRate { get; set; }

        public int Status { get; set; }

        public int Mode { get; set; }

        public string Configs { get; set; }

        public ClaimDataFileConfig ClaimDataFileConfig { get; set; }

        public string OverrideProperties { get; set; }

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

        public const int ModeInclude = 1;
        public const int ModeExclude = 2;
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

        public static string GetModeName(int key)
        {
            switch (key)
            {
                case ModeInclude:
                    return "Replace Data";
                case ModeExclude:
                    return "Update Data";
                default:
                    return "";
            }
        }

        public void UpdateConfigFromClaimDataConfig(ClaimDataConfigBo claimDataConfigBo)
        {
            ClaimDataFileConfig.IsColumnToRowMapping = claimDataConfigBo.ClaimDataFileConfig.IsColumnToRowMapping;
            ClaimDataFileConfig.NumberOfRowMapping = claimDataConfigBo.ClaimDataFileConfig.NumberOfRowMapping;
            ClaimDataFileConfig.IsDataCorrection = claimDataConfigBo.ClaimDataFileConfig.IsDataCorrection;
        }
    }
}
