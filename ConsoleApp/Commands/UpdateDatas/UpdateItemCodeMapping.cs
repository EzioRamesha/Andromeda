using DataAccess.EntityFramework;
using System.Data.Entity;
using System.Linq;

namespace ConsoleApp.Commands.UpdateDatas
{
    public class UpdateItemCodeMapping : Command
    {
        public bool IsUpdateProfitComm { get; set; }

        public UpdateItemCodeMapping()
        {
            Title = "UpdateItemCodeMapping";
            Description = "To update Item Code Mapping table";
            Hide = true;
        }

        public override void Initial()
        {
            base.Initial();
        }

        public override void Run()
        {
            PrintStarting();

            using (var db = new AppDbContext())
            {
                int total = db.ItemCodeMappings.Count();
                int processed = 0;

                while (processed < total)
                {
                    if (PrintCommitBuffer())
                    {
                        db.SaveChanges();
                    }
                    SetProcessCount();

                    var itemCodeMapping = db.ItemCodeMappings.OrderBy(q => q.Id).Skip(processed).FirstOrDefault();
                    if (itemCodeMapping != null)
                    {
                        var businessOriginId = itemCodeMapping.ItemCode?.BusinessOriginPickListDetailId;

                        itemCodeMapping.BusinessOriginPickListDetailId = businessOriginId;

                        db.Entry(itemCodeMapping).State = EntityState.Modified;

                        SetProcessCount("Updated Business Origin");
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
