using Newtonsoft.Json;
using System;

namespace BusinessObject.RiDatas
{
    public class RiDataFileBo
    {
        public int Id { get; set; }

        public int RiDataBatchId { get; set; }

        public virtual RiDataBatchBo RiDataBatchBo { get; set; }

        public int RawFileId { get; set; }

        public virtual RawFileBo RawFileBo { get; set; }

        public int? TreatyId { get; set; }

        public virtual TreatyBo TreatyBo { get; set; }

        public int? RiDataConfigId { get; set; }

        public virtual RiDataConfigBo RiDataConfigBo { get; set; }

        public string Configs { get; set; }

        public RiDataFileConfig RiDataFileConfig { get; set; }

        public string OverrideProperties { get; set; }

        public int Mode { get; set; }

        public int Status { get; set; }

        public string Errors { get; set; }

        public int RecordType { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedAtStr { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public const int StatusPending = 1;
        public const int StatusProcessing = 2;
        public const int StatusCompleted = 3;
        public const int StatusCompletedFailed = 4;

        public const int ModeInclude = 1;
        public const int ModeExclude = 2;

        public const int RecordTypeNew = 1;
        public const int RecordTypeAdjustment = 2;
        public const int RecordTypeMax = 2;

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
                    return "Completed - Failed";
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

        public static string GetRecordTypeName(int key)
        {
            switch (key)
            {
                case RecordTypeNew:
                    return "New";
                case RecordTypeAdjustment:
                    return "Adjustment";
                default:
                    return "";
            }
        }

        public void UpdateConfigFromRiDataConfig(RiDataConfigBo riDataConfigBo)
        {
            RiDataFileConfig.IsColumnToRowMapping = riDataConfigBo.RiDataFileConfig.IsColumnToRowMapping;
            RiDataFileConfig.NumberOfRowMapping = riDataConfigBo.RiDataFileConfig.NumberOfRowMapping;
            RiDataFileConfig.IsDataCorrection = riDataConfigBo.RiDataFileConfig.IsDataCorrection;
        }
    }
}
