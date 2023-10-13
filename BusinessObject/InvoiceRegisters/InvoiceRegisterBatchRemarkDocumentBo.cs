using Shared;
using System;
using System.IO;

namespace BusinessObject.InvoiceRegisters
{
    public class InvoiceRegisterBatchRemarkDocumentBo
    {
        public int Id { get; set; }

        public int InvoiceRegisterBatchRemarkId { get; set; }

        public InvoiceRegisterBatchRemarkBo InvoiceRegisterBatchRemarkBo { get; set; }

        public string FileName { get; set; }

        public string HashFileName { get; set; }

        public bool FileExists { get; set; } = false;

        public string DownloadLink { get; set; }

        public string TempFilePath { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedAtStr { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedAtStr { get; set; }

        public int CreatedById { get; set; }

        public string CreatedByName { get; set; }

        public int? UpdatedById { get; set; }


        public bool IsFileExists()
        {
            FileExists = File.Exists(GetLocalPath());
            return FileExists;
        }

        public string GetDirectoryPath()
        {
            return string.Format("{0}/{1}", Util.GetDocumentPath(), "InvoiceRegister");
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
