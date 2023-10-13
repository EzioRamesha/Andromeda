using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using System.Data.Entity;
using System.Linq;


namespace ConsoleApp.Commands.UpdateDatas
{
    public class UpdateTreatyDiscountTable : Command
    {
        public UpdateTreatyDiscountTable()
        {
            Title = "UpdateTreatyDiscountTable";
            Description = "To set all previous Treaty Discount Table to Direct Retro";
            Options = new string[] {
            };
            Hide = true;
        }

        public override void Run()
        {
            PrintStarting();

            using (var db = new AppDbContext())
            {
                int total = db.TreatyDiscountTables.Count();
                int processed = 0;

                while (processed < total)
                {
                    if (PrintCommitBuffer())
                    {
                        db.SaveChanges();
                    }
                    SetProcessCount();

                    var treatyDiscountTable = db.TreatyDiscountTables.OrderBy(q => q.Id).Skip(processed).FirstOrDefault();

                    if (treatyDiscountTable != null)
                    {
                        treatyDiscountTable.Type = TreatyDiscountTableBo.TypeDirectRetro;
                        db.Entry(treatyDiscountTable).State = EntityState.Modified;
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
