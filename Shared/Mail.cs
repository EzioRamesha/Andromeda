using System;
using System.Net;
using System.Net.Mail;

namespace Shared
{
    public class Mail : MailMessage
    {
        public SmtpClient Client { get; set; }

        public Mail()
        {
            Initial();
        }

        public void Initial()
        {
            From = new MailAddress(
                Util.GetConfig("EmailFrom"),
                Util.GetConfig("EmailFromDisplayName")
            );

            int port = Int32.Parse(Util.GetConfig("EmailSmtpPort"));
            Client = new SmtpClient(
                Util.GetConfig("EmailSmtpHost"),
                port
            )
            {
                Credentials = new NetworkCredential(
                    Util.GetConfig("EmailSmtpUserName"),
                    Util.GetConfig("EmailSmtpPassword")
                )
            };

            Client.EnableSsl = false;
            if (bool.TryParse(Util.GetConfig("EmailEnableSsl"), out bool enable))
            {
                Client.EnableSsl = enable;
            }
        }

        public void Send()
        {
            if (Client != null)
                Client.Send(this);
            else
                throw new Exception("Please setup SmtpClient");
        }
    }
}
