using System;
using System.DirectoryServices.AccountManagement;

namespace Shared.DirectoryServices
{
    public class DirectoryService
    {
        public string Server { get; set; }

        public string Domain { get; set; }

        public PrincipalContext Context { get; set; }

        public DirectoryService()
        {
            string path = Util.GetConfig("ActiveDirectoryPath");
            string container = Util.GetConfig("ActiveDirectoryContainer");
            string username = Util.GetConfig("ActiveDirectoryUsername");
            string password = Util.GetConfig("ActiveDirectoryPassword");

            try
            {
                Context = new PrincipalContext(ContextType.Domain, path, container, username, password);
            } catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }

        public string Email(string address)
        {
            return string.Format("{0}@{1}", address, Util.GetConfig("ActiveDirectoryEmailDomain"));
        }
    }
}
