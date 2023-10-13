using Shared;
using System;
using System.IO;

namespace BusinessObject
{
    public class DocumentBo
    {
        public int Id { get; set; }

        public int ModuleId { get; set; }

        public ModuleBo ModuleBo { get; set; }

        public int ObjectId { get; set; }

        public string SubModuleController { get; set; }

        public int? SubObjectId { get; set; }

        public int? RemarkId { get; set; }

        public int Type { get; set; }

        public string TypeName { get; set; }

        public string FileName { get; set; }

        public string HashFileName { get; set; }

        public bool FileExists { get; set; } = false;

        public string DownloadLink { get; set; }

        public string Description { get; set; }

        public string TempFilePath { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedAtStr { get; set; }
        
        public DateTime UpdatedAt { get; set; }

        public string UpdatedAtStr { get; set; }

        public int CreatedById { get; set; }

        public string CreatedByName { get; set; }

        public int? UpdatedById { get; set; }

        // Object Permission
        public bool IsPrivate { get; set; }

        public string PermissionName { get; set; }

        public ObjectPermissionBo ObjectPermissionBo { get; set; }

        // For frontend use only
        public int? RemarkIndex { get; set; }

        public const int TypeRequestForm = 1;
        public const int TypeApprovalEmail = 2;
        public const int TypeOthers = 3;
        public const int MaxType = 3;

        public static string GetTypeName(int key)
        {
            switch (key)
            {
                case TypeRequestForm:
                    return "Request Form";
                case TypeApprovalEmail:
                    return "Approval Email";
                case TypeOthers:
                    return "Others";
                default:
                    return "";
            }
        }

        public bool IsFileExists()
        {
            if (ModuleBo == null)
                return false;
            FileExists = File.Exists(GetLocalPath());
            return FileExists;
        }

        public static string GetDirectoryPathByModule(string moduleName)
        {
            return string.Format("{0}/{1}", Util.GetDocumentPath(), moduleName);
        }

        public string GetDirectoryPath()
        {
            return GetDirectoryPathByModule(ModuleBo.Controller);
        }

        public static string GetTempFolderPath(string subFolder = null)
        {
            return Util.GetTemporaryPath(subFolder);
        }

        public string GetTempPath(string subFolder = null)
        {
            return Path.Combine(GetTempFolderPath(subFolder), HashFileName);
        }

        public string GetLocalPath()
        {
            return Path.Combine(GetDirectoryPath(), HashFileName);
        }

        public string GetDownloadLink(string action)
        {
            DownloadLink = string.Format("{0}/{1}", action, Id);
            return DownloadLink;
        }

        public string FormatHashFileName()
        {
            HashFileName = Hash.HashFileName(FileName);
            return HashFileName;
        }
    }
}
