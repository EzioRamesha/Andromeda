using DataAccess.EntityFramework;
using Services.Sanctions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.UpdateDatas
{
    public class UpdateSanctionVerification : Command
    {
        public UpdateSanctionVerification()
        {
            Title = "UpdateSanctionVerification";
            Description = "To update sanction verifications";
            Hide = true;
        }

        public override void Run()
        {
            PrintStarting();

            using (var db = new AppDbContext())
            {
                int total = db.SanctionVerifications.Count();
                int processed = 0;

                while (processed < total)
                {
                    if (PrintCommitBuffer())
                    {
                        db.SaveChanges();
                    }
                    SetProcessCount();

                    var verification = db.SanctionVerifications.OrderBy(q => q.Id).Skip(processed).FirstOrDefault();
                    if (verification != null)
                    {
                        verification.Record = SanctionVerificationDetailService.CountByParentId(verification.Id, db);
                        verification.UnprocessedRecords = SanctionVerificationDetailService.CountUnprocessedByParentId(verification.Id, db);
                        db.Entry(verification).State = EntityState.Modified;
                    }

                    processed++;
                }
                db.SaveChanges();

                if (processed > 0)
                    PrintProcessCount();
            }
        }
    }
}
