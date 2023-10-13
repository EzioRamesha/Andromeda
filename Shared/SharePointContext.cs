using Microsoft.SharePoint.Client;
using System;
using System.Security;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Shared
{
    public class SharePointContext : IDisposable
    {
        private ClientContext _context = new ClientContext(__sharepointUrl);

        private ClientContext _contextWithSubsite = new ClientContext(__sharepointUrlWithSubsite);
        private static string __sharepointUrl => ConfigurationManager.AppSettings["SharePointURL"];
        private static string __sharepointUrlWithSubsite => __sharepointUrl + __subsite;
        private static string __subsite => ConfigurationManager.AppSettings["SharePointSubsite"];
        private static string __docRootFolder => ConfigurationManager.AppSettings["SharePointDocRootFolder"];
        private static string __docSubFolder => ConfigurationManager.AppSettings["SharePointSubFolder"];

        public SharePointContext()
        {
            this.Connect();
        }

        private void Connect()
        {
            string username = ConfigurationManager.AppSettings["SharePointUsername"];
            string password = ConfigurationManager.AppSettings["SharePointPassword"];
            var securePassword = new SecureString();
            foreach (var chr in password) securePassword.AppendChar(chr);
            _context.Credentials = new SharePointOnlineCredentials(username, securePassword);
            _contextWithSubsite.Credentials = new SharePointOnlineCredentials(username, securePassword);
        }

        public bool FileFolderExists(string FileUrl)
        {
            FileUrl = __sharepointUrlWithSubsite + __docRootFolder + __docSubFolder + "/" + FileUrl;
            Web web = _contextWithSubsite.Web;
            List list = web.Lists.GetByTitle("Documents");
            CamlQuery camlQuery = new CamlQuery();
            camlQuery.ViewXml = string.Format("" +
                "<View Scope=\"RecursiveAll\">" +
                    "<Query>" +
                        "<Where>" +
                            "<Eq>" +
                                "<FieldRef Name=\"FileRef\"/>" +
                                "<Value Type=\"Url\">" + FileUrl + "</Value>" +
                            "</Eq>" +
                        "</Where>" +
                    "</Query>" +
                "</View>");
            var items = list.GetItems(camlQuery);
            _contextWithSubsite.Load(items);
            _contextWithSubsite.ExecuteQuery();
            return items.Count > 0;
        }

        public void UploadFile(string SourcePath, string FileName, string FolderName = "")
        {
            using (FileStream fileStream =
                new FileStream(SourcePath, FileMode.Open))
                Microsoft.SharePoint.Client.File.SaveBinaryDirect(_context
                    , (FolderName == "" ? __subsite + __docRootFolder + __docSubFolder : __subsite + __docRootFolder + __docSubFolder + "/" + FolderName) + "/" + FileName
                    , fileStream
                    , true);
        }

        public string GetCopyLinkURL(string FilePath)//, string FileExtension = "")
        {
            FilePath = (__sharepointUrl + __subsite + __docRootFolder + __docSubFolder + "/" + FilePath).Replace(" ", "%20");
            ObjectSharingSettings info = Web.GetObjectSharingSettings(_context, FilePath, 0, true);
            _context.Load(info.ObjectSharingInformation);
            _context.ExecuteQuery();

            string finalUrl = "";
            //var links = info.ObjectSharingInformation.SharingLinks;
            foreach (var url in info.ObjectSharingInformation.SharingLinks)
            {
                if (!string.IsNullOrEmpty(url.Url))
                    finalUrl = url.Url;
            }

            return finalUrl;
        }

        //Old method
        //public string GetCopyLinkURL(string FilePath)//, string FileExtension = "")
        //{
        //    //if (FileExtension != "")
        //    //{
        //    FilePath = (__sharepointUrl + __subsite + __docRootFolder + __docSubFolder + "/" + FilePath).Replace(" ", "%20");
        //    ClientResult<string> clientResult;
        //    clientResult = Microsoft.SharePoint.Client.Web.CreateAnonymousLink(_context, FilePath, false);
        //    _context.ExecuteQuery();

        //    return clientResult.Value.ToString();
        //    //}
        //    //else { return ""; }
        //}

        public void AddNewFolder(string fullFolderUrl)
        {
            fullFolderUrl = __docSubFolder == "" ? fullFolderUrl : __docSubFolder + "/" + fullFolderUrl;
            var Folder = CreateFolder(_contextWithSubsite.Web, "Documents", fullFolderUrl);
        }

        private static Folder CreateFolder(Web web, string listTitle, string fullFolderUrl)
        {
            if (string.IsNullOrEmpty(fullFolderUrl))
                throw new ArgumentNullException("fullFolderUrl");
            var list = web.Lists.GetByTitle(listTitle);
            return CreateFolderInternal(web, list.RootFolder, fullFolderUrl);
        }

        private static Folder CreateFolderInternal(Web web, Folder parentFolder, string fullFolderUrl)
        {
            var folderUrls = fullFolderUrl.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            string folderUrl = folderUrls[0];
            var curFolder = parentFolder.Folders.Add(folderUrl);
            web.Context.Load(curFolder);
            web.Context.ExecuteQuery();

            if (folderUrls.Length > 1)
            {
                var subFolderUrl = string.Join("/", folderUrls, 1, folderUrls.Length - 1);
                return CreateFolderInternal(web, curFolder, subFolderUrl);
            }
            return curFolder;
        }

        #region IDisposable Support
        private bool disposedValue = false; //To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                    _contextWithSubsite.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}