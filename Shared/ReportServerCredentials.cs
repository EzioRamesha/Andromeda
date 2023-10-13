using Microsoft.Reporting.WebForms;
using System.Net;
using System.Security.Principal;

namespace Shared
{
    public class ReportServerCredentials : IReportServerCredentials
    {
        private string Username;
        private string Password;
        private string Domain;

        public ReportServerCredentials(string Username, string Password, string Domain)
        {
            this.Username = Username;
            this.Password = Password;
            this.Domain = Domain;
        }

        public WindowsIdentity ImpersonationUser
        {
            get
            {
                return null;
            }

        }
        public ICredentials NetworkCredentials
        {
            get
            {
                return new NetworkCredential(Username, Password, Domain);
            }
        }

        public bool GetFormsCredentials(out Cookie authCookie, out string userName, out string password, out string authority)
        {
            authCookie = null;
            userName = null;
            password = null;
            authority = null;

            // Not using form credentials
            return false;
        }
    }
}
