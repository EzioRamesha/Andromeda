using Shared;
using System;
using System.IO;

namespace BusinessObject
{
    public class RawFileBo
    {
        public int Id { get; set; }

        public int Type { get; set; }

        public int Status { get; set; }

        public string FileName { get; set; }

        public string HashFileName { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public const int StatusPending = 1;
        public const int StatusProcessing = 2;
        public const int StatusCompleted = 3;
        public const int StatusCompletedFailed = 4;

        public const int TypeRiData = 1;
        public const int TypeClaimData = 2;
        public const int TypeSoaData = 3;

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

        public static string GetTypeName(int key)
        {
            switch (key)
            {
                case TypeRiData:
                    return "RiData";
                case TypeClaimData:
                    return "ClaimData";
                case TypeSoaData:
                    return "SoaData";
                default:
                    return "";
            }
        }

        public string GetLocalDirectory()
        {
            return Util.GetRawFilePath(GetTypeName(Type));
        }

        public string GetLocalPath()
        {
            return Path.Combine(GetLocalDirectory(), HashFileName);
        }

        public string GetTemporaryDirectory()
        {
            return Util.GetTemporaryPath(GetTypeName(Type));
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
    }
}
