using BusinessObject.Identity;
using Shared;
using System;
using System.IO;

namespace BusinessObject.TreatyPricing
{
    public class TemplateDetailBo
    {
        public int Id { get; set; }

        public int TemplateId { get; set; }

        public virtual TemplateBo TemplateBo { get; set; }

        public int TemplateVersion { get; set; }

        public string FileName { get; set; }

        public string HashFileName { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedAtStr { get; set; }

        public int CreatedById { get; set; }

        public string CreatedByName { get; set; }

        public int? UpdatedById { get; set; }

        public string TempFilePath { get; set; }

        public string GetDirectoryPath()
        {
            return Util.GetUploadPath("Template");
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

        //public string GetDownloadLink(string action)
        //{
        //    DownloadLink = string.Format("{0}/{1}", action, Id);
        //    return DownloadLink;
        //}

        public string FormatHashFileName()
        {
            HashFileName = Hash.HashFileName(FileName);
            return HashFileName;
        }
    }
}
