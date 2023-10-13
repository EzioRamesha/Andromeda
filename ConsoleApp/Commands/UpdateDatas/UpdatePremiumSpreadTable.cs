using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using System.Data.Entity;
using System.Linq;


namespace ConsoleApp.Commands.UpdateDatas
{
    public class UpdatePremiumSpreadTable : Command
    {
        public UpdatePremiumSpreadTable()
        {
            Title = "UpdatePremiumSpreadTable";
            Description = "To set all previous Premium Spread Table to Direct Retro";
            Options = new string[] {
            };
            Hide = true;
        }

        public override void Run()
        {
            PrintStarting();

            using (var db = new AppDbContext())
            {
                int total = db.PremiumSpreadTables.Count();
                int processed = 0;

                while (processed < total)
                {
                    if (PrintCommitBuffer())
                    {
                        db.SaveChanges();
                    }
                    SetProcessCount();

                    var premiumSpreadTable = db.PremiumSpreadTables.OrderBy(q => q.Id).Skip(processed).FirstOrDefault();

                    if (premiumSpreadTable != null)
                    {
                        premiumSpreadTable.Type = PremiumSpreadTableBo.TypeDirectRetro;
                        db.Entry(premiumSpreadTable).State = EntityState.Modified;
                    }
                    processed++;
                }
                db.SaveChanges();

                if (processed > 0)
                    PrintProcessCount();
            }

            PrintEnding();
        }
    }
}
