using DataAccess.Entities;
using DataAccess.EntityFramework;
using System.Data.Entity;
using System.Linq;

namespace ConsoleApp.Commands.UpdateDatas
{
    public class UpdateTreatyOldCode : Command
    {
        public UpdateTreatyOldCode()
        {
            Title = "UpdateTreatyOldCode";
            Description = "To link treaty old code";
            Options = new string[] {
                "--t|truncate : Truncate records",
            };
            Hide = true;
        }

        public override void Run()
        {
            PrintStarting();

            using (var db = new AppDbContext())
            {
                if (IsOption("truncate"))
                {
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [TreatyOldCodes]");
                    PrintMessage("Truncated [TreatyOldCodes] table");
                    db.SaveChanges();
                }

                var query = db.TreatyCodes.Where(q => q.OldTreatyCodeId != 0 && q.OldTreatyCodeId != null);
                int count = query.Count();
                int processed = 0;

                while (processed < count)
                {
                    if (PrintCommitBuffer())
                    {
                        db.SaveChanges();
                    }
                    SetProcessCount();

                    var code = query.OrderBy(q => q.Id).Skip(processed).Take(1).FirstOrDefault();
                    if (code != null)
                    {
                        var oldCode = new TreatyOldCode
                        {
                            TreatyCodeId = code.Id,
                            OldTreatyCodeId = code.OldTreatyCodeId.Value,
                        };

                        db.TreatyOldCodes.Add(oldCode);
                    }

                    count = query.Count();
                    processed++;
                }
                db.SaveChanges();

                if (processed > 0)
                    PrintProcessCount();

                var zeroCode = GetZeroOldCode(db);
                while (zeroCode != null)
                {
                    if (PrintCommitBuffer("Updated"))
                    {
                    }

                    zeroCode.OldTreatyCodeId = null;
                    db.Entry(zeroCode).State = EntityState.Modified;
                    db.SaveChanges();
                    SetProcessCount("Updated");

                    zeroCode = GetZeroOldCode(db);
                }
                db.SaveChanges();

                if (GetProcessCount("Updated") > 0)
                    PrintProcessCount();
            }

            PrintEnding();
        }

        public TreatyCode GetZeroOldCode(AppDbContext db)
        {
            return db.TreatyCodes.Where(q => q.OldTreatyCodeId == 0).OrderBy(q => q.Id).FirstOrDefault();
        }
    }
}
