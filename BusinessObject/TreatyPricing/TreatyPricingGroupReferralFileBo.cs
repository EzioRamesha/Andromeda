using BusinessObject.Identity;
using Shared;
using System;
using System.IO;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingGroupReferralFileBo
    {
        public int Id { get; set; }

        public int? TreatyPricingGroupReferralId { get; set; }
        public virtual TreatyPricingGroupReferralBo TreatyPricingGroupReferralBo { get; set; }

        public int? TableTypePickListDetailId { get; set; }
        public virtual PickListDetailBo TableTypePickListDetailBo { get; set; }

        public string FileName { get; set; }

        public string HashFileName { get; set; }

        public int UploadedType { get; set; }

        public int Status { get; set; }

        public string StatusName { get; set; }

        public string Errors { get; set; }

        public string FormattedErrors { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedAtStr { get; set; }

        public int CreatedById { get; set; }

        public string CreatedBy { get; set; }

        public UserBo CreatedByBo { get; set; }

        public int? UpdatedById { get; set; }

        public const int StatusPending = 1;
        public const int StatusProcessing = 2;
        public const int StatusSuccess = 3;
        public const int StatusFailed = 4;

        public const int UploadedTypeFile = 1;
        public const int UploadedTypeTable = 2;
        public const int UploadedTypeMax = 2;

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

        public string GetLocalDirectory()
        {
            return Util.GetTreatyPricingGroupReferralUploadPath();
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
