using BusinessObject;
using DataAccess.Entities.Identity;
using Services;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;

namespace ConsoleApp.Commands
{
    public class PasswordExpiryReminder : Command
    {
        public PasswordExpiryReminder()
        {
            Title = "PasswordExpiryReminder";
            Description = "Send an email to notify users on password expiration";
        }

        public override void Run()
        {
            PrintStarting();

            int daysToExpiry = Int32.Parse(Util.GetConfig("PasswordExpiryNotice"));
            DateTime notifyDate = DateTime.Now.AddDays(daysToExpiry);

            var users = User.GetByPasswordExpiryDate(notifyDate);
            foreach (User user in users)
            {
                SendEmail(user, daysToExpiry);
            }

            PrintEnding();
        }

        protected void SendEmail(User user, int daysToExpiry)
        {
            string email = user.Email;
            string passwordExpiryDate = DateTime.Parse(user.PasswordExpiresAt.ToString()).ToShortDateString();

            if (email.IsValidEmail())
            {
                var emailBo = new EmailBo(EmailBo.TypePasswordExpiryReminder, user.Email)
                {
                    RecipientUserId = user.Id,
                    CreatedById = User.DefaultSuperUserId,
                    UpdatedById = User.DefaultSuperUserId,
                };
                emailBo.AddData(user.UserName);
                emailBo.AddData(daysToExpiry.ToString());
                emailBo.AddData(passwordExpiryDate);

                Mail mail = emailBo.GenerateMail();
                try
                {
                    mail.Send();
                    emailBo.Status = EmailBo.StatusSent;
                }
                catch (Exception ex)
                {
                    emailBo.Status = EmailBo.StatusFailed;
                    PrintError("Failed to send email: " + ex.Message);
                }

                EmailService.Create(ref emailBo);
            }
            else
            {
                PrintError(string.Format("Invalid email address format: {0}", email));
            }
        }
    }
}
