using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.IO;

namespace BusinessObject.Sanctions
{
    public class SanctionBatchBo
    {
        public int Id { get; set; }

        public int SourceId { get; set; }

        public SourceBo SourceBo { get; set; }

        public string FileName { get; set; }

        public string HashFileName { get; set; }

        public int Method { get; set; }

        public int Status { get; set; }

        public int Record { get; set; }

        public DateTime UploadedAt { get; set; }

        public string Errors { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public const int StatusPending = 1;
        public const int StatusProcessing = 2;
        public const int StatusSuccess = 3;
        public const int StatusFailed = 4;
        public const int StatusReplaced = 5;

        public const int StatusMax = 5;

        public const int MethodAddOn = 1;
        public const int MethodReplacement = 2;

        public const int MethodMax = 2;

        public const int TypePublicationInformation = 1;
        public const int TypeName = 2;
        public const int TypeAlias = 3;
        public const int TypeCategory = 4;
        public const int TypeIdType = 5;
        public const int TypeIdNumber = 6;
        public const int TypeDateOfBirth = 7;
        public const int TypeCountry = 8;
        public const int TypeRefNumber = 9;
        public const int TypeAddress = 10;
        public const int TypeComment = 11;

        public const int TypeMax = 9;

        public SanctionBatchBo()
        {
            Record = 0;
        }

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
                case StatusReplaced:
                    return "Replaced";
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
                case StatusReplaced:
                    return "status-fail-badge";
                default:
                    return "";
            }
        }

        public static string GetMethodName(int key)
        {
            switch (key)
            {
                case MethodAddOn:
                    return "Add On";
                case MethodReplacement:
                    return "Replacement";
                default:
                    return "";
            }
        }

        public string GetLocalDirectory()
        {
            return Util.GetSanctionUploadPath();
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

        public static List<Column> GetColumns()
        {
            return new List<Column>
            {
                new Column()
                {
                    Type = TypePublicationInformation,
                    Header = "Publication Information",
                    Property = "PublicationInformation"
                },
                new Column()
                {
                    Type = TypeName,
                    Header = "Name",
                    Property = "Name"
                },
                new Column()
                {
                    Type = TypeAlias,
                    Header = "Alias Name",
                },
                new Column()
                {
                    Type = TypeCategory,
                    Header = "Category",
                    Property = "Category"
                },
                new Column()
                {
                    Type = TypeIdType,
                    Header = "Document Type",
                },
                new Column()
                {
                    Type = TypeIdNumber,
                    Header = "Document Number",
                },
                new Column()
                {
                    Type = TypeDateOfBirth,
                    Header = "Date Of Birth",
                },
                new Column()
                {
                    Type = TypeCountry,
                    Header = "Country",
                },
                new Column()
                {
                    Type = TypeRefNumber,
                    Header = "Reference Number",
                    Property = "RefNumber"
                },
                new Column()
                {
                    Type = TypeAddress,
                    Header = "Address",
                },
                new Column()
                {
                    Type = TypeComment,
                    Header = "Comment",
                }
            };
        }
    }
}
