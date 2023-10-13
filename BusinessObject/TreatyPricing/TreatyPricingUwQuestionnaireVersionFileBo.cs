using Shared;
using System.IO;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingUwQuestionnaireVersionFileBo
    {
        public int Id { get; set; }

        public int TreatyPricingUwQuestionnaireVersionId { get; set; }

        public virtual TreatyPricingUwQuestionnaireVersionBo TreatyPricingUwQuestionnaireVersionBo { get; set; }

        public string FileName { get; set; }

        public string HashFileName { get; set; }

        public int Status { get; set; }

        public string Errors { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string StatusName { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedAtStr { get; set; }

        public const int StatusPending = 1;
        public const int StatusSubmitForProcessing = 2;
        public const int StatusProcessing = 3;
        public const int StatusSuccess = 4;
        public const int StatusFailed = 5;

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
            return Util.GetTreatyPricingUwQuestionnaireUploadPath();
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
