using Newtonsoft.Json;
using System.Collections.Generic;

namespace BusinessObject.RiDatas
{
    public class RiDataConfigBo
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public int CedantId { get; set; }

        public string CedantName { get; set; }

        [JsonIgnore]
        public CedantBo CedantBo { get; set; }

        [JsonIgnore]
        public int? TreatyId { get; set; }

        public string TreatyIdCode { get; set; }

        [JsonIgnore]
        public TreatyBo TreatyBo { get; set; }

        [JsonIgnore]
        public int Status { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int FileType { get; set; }

        public string FileTypeName { get; set; }

        [JsonIgnore]
        public string Configs { get; set; }

        public RiDataFileConfig RiDataFileConfig { get; set; }

        [JsonIgnore]
        public int CreatedById { get; set; }

        [JsonIgnore]
        public int? UpdatedById { get; set; }

        public IList<RiDataMappingBo> RiDataMappingBos { get; set; }

        public IList<RiDataComputationBo> RiDataComputationBos { get; set; }

        public IList<RiDataPreValidationBo> RiDataPreValidationBos { get; set; }

        public const int FileTypeExcel = 1;
        public const int FileTypePlainText = 2;
        public const int MaxFileType = 2;

        public const int DelimiterComma = 1;
        public const int DelimiterPipe = 2;
        public const int DelimiterFixedLength = 3;
        public const int DelimiterTab = 4;
        public const int MaxDelimiter = 4;

        public const int StatusDraft = 1;
        public const int StatusPending = 2;
        public const int StatusApproved = 3;
        public const int StatusRejected = 4;
        public const int StatusDisabled = 5;

        public const int MaxStatus = 5;


        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusDraft:
                    return "Draft";
                case StatusPending:
                    return "Pending";
                case StatusApproved:
                    return "Approved";
                case StatusRejected:
                    return "Rejected";
                case StatusDisabled:
                    return "Disabled";
                default:
                    return "";
            }
        }

        public static string GetFileTypeName(int key)
        {
            switch (key)
            {
                case FileTypeExcel:
                    return "Excel (.xls .xlsx)";
                case FileTypePlainText:
                    return "Plain Text (.txt .csv)";
                default:
                    return "";
            }
        }

        public static string GetDelimiterName(int? key)
        {
            switch (key)
            {
                case DelimiterComma:
                    return "Comma";
                case DelimiterPipe:
                    return "Pipe";
                case DelimiterFixedLength:
                    return "Fixed Length";
                case DelimiterTab:
                    return "Tab Space";
                default:
                    return "";
            }
        }

        public static char GetDelimiterChar(int? key)
        {
            switch (key)
            {
                case DelimiterComma:
                    return ',';
                case DelimiterPipe:
                    return '|';
                case DelimiterFixedLength:
                    return ' ';
                case DelimiterTab:
                    return '\t';
                default:
                    return ' ';
            }
        }

        public static string GetStatusClass(int key)
        {
            switch (key)
            {
                case StatusDraft:
                    return "status-submitprocess-badge";
                case StatusPending:
                    return "status-processing-badge";
                case StatusApproved:
                    return "status-finalize-badge";
                case StatusRejected:
                    return "status-fail-badge";
                case StatusDisabled:
                    return "status-submitfinalise-badge";
                default:
                    return "";
            }
        }
    }
}
