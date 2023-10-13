using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Services;
using Services.Identity;
using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands
{
    public class SendInactiveUserReport : Command
    {
        public int DaysInactive { get; set; } = 90;
        public int DaysInactiveReport { get; set; } = 30;
        public int DiffDaysInactive { get; set; } = 60;
        public int Total { get; set; } = 0;
        public int Take { get; set; } = 50;
        public int Skip { get; set; } = 0;
        public IQueryable<User> Query { get; set; }
        public List<User> Users { get; set; }
        public Pdf Document { get; set; }
        public Table Table { get; set; }

        public SendInactiveUserReport()
        {
            Title = "SendInactiveUserReport";
            Description = "To send Inactive User Report to System Administrator";

            DaysInactive = Util.GetConfigInteger("DaysBeforeInactiveUserSuspension");
            DaysInactiveReport = Util.GetConfigInteger("DaysBeforeInactiveUserReport");
            DiffDaysInactive = DaysInactive - DaysInactiveReport;
        }

        public override void Run()
        {
            using (var db = new AppDbContext(false))
            {
                Query = User.QueryInactiveUser(db, DaysInactive);
                Total = Query.Count();
                if (Total == 0)
                {
                    Log = false;
                    PrintMessage(MessageBag.NoInactiveUser);
                    return;
                }

                PrintStarting();
                Document = new Pdf();
                Document.CenterTitle("Inactive User Report");
                Document.AddVerticalSpace(20);

                Table = new Table(Document.GetPageWidth());
                Table.AddHeader(new List<Cell>
                {
                    new Cell("User Name", 100),
                    new Cell("Employee Name"),
                    new Cell("Last Login Date & Time", 140),
                    new Cell("Expiry Date", 110),
                });

                while (GetNextBulkUser(db))
                {
                    foreach (var u in Users)
                    {
                        var lastLoginAt = u.LastLoginAt;
                        var lastActivityAt = lastLoginAt != null ? lastLoginAt.Value : u.CreatedAt;
                        var expiryDate = lastActivityAt.AddDays(DaysInactive);
                        var lastActivity = lastLoginAt != null ? lastLoginAt.Value.ToString(Util.GetDateTimeFormat()) : "";
                        Table.AddRow(new Row(new List<Cell>
                        {
                            new Cell(u.UserName),
                            new Cell(u.FullName),
                            new Cell(lastActivity),
                            new Cell(expiryDate.ToString(Util.GetDateFormat())),
                        }));
                    }
                }

                Document.PrintTable(Table);

                var stream = new MemoryStream();
                Document.Document.Save(stream);

                var attachment = new Attachment(stream, "Inactive User Report.pdf", "application/pdf");

                var moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.User.ToString());
                var adminUserBos = UserService.GetByModulePower(moduleBo.Id, AccessMatrixBo.PowerInactiveUserReport);
                foreach (var adminUserBo in adminUserBos)
                {
                    var emailBo = new EmailBo(EmailBo.TypeInactiveUserReport, adminUserBo.Email) 
                    {
                        RecipientUserId = adminUserBo.Id,
                        CreatedById = User.DefaultSuperUserId,
                        UpdatedById = User.DefaultSuperUserId,
                    };

                    Mail mail = emailBo.GenerateMail();
                    try
                    {
                        mail.Attachments.Add(attachment);
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

                stream.Close();
                PrintEnd();
            }
        }

        public bool GetNextBulkUser(AppDbContext db)
        {
            Users = new List<User> { };
            Query = User.QueryInactiveUser(db, DiffDaysInactive);
            Total = Query.Count();
            if (Skip >= Total)
                return false;

            Users = Query.Skip(Skip).Take(Take).ToList();
            Skip += Take;
            return true;
        }
    }
}
