using Shared;
using Shared.Argument;
using System;

namespace ConsoleApp.Commands
{
    public class SendTestEmail : Command
    {
        public SendTestEmail()
        {
            Title = "SendTestEmail";
            Description = "To send a test email";
            Arguments = new string[]
            {
                "email_address",
            };
        }

        public override bool Validate()
        {
            string email = CommandInput.Arguments[0];
            if (!email.IsValidEmail())
            {
                PrintError(string.Format("This is wrong emaill address format: {0}", email));
                return false;
            }
            return true;
        }

        public override void Run()
        {
            PrintStarting();

            string email = CommandInput.Arguments[0];
            if (email.IsValidEmail())
            {
                PrintMessage("Sending now...");

                try
                {
                    Mail mail = new Mail();

                    mail.To.Add(email);
                    mail.Subject = "Test Mail";
                    mail.Body = "This is for testing mail";
                    mail.Send();

                    PrintMessage("Successfully sent a test email");
                }
                catch (Exception ex)
                {
                    PrintError("Failed to send a test email: " + ex.Message);
                }
            }

            PrintEnding();
        }
    }
}
