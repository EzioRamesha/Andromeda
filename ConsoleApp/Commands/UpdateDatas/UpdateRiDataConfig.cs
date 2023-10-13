using DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.UpdateDatas
{
    class UpdateRiDataConfig : Command
    {
        public UpdateRiDataConfig()
        {
            Title = "UpdateRiDataConfig";
            Description = "To update treaty code id to treaty id";
            Options = new string[] {
            };
            Hide = true;
        }

        public override void Run()
        {
            PrintStarting();

            using (var db = new AppDbContext())
            {
                int total = db.RiDataConfigs.Count();
                int processed = 0;

                while (processed < total)
                {
                    if (PrintCommitBuffer())
                    {
                        db.SaveChanges();
                    }
                    SetProcessCount();

                    var riDataConfig = db.RiDataConfigs.OrderBy(q => q.Id).Skip(processed).FirstOrDefault();
                    if (riDataConfig != null)
                    {
                        var treatyCode = db.TreatyCodes.Where(q => q.Id == riDataConfig.TreatyCodeId).FirstOrDefault();
                        if (treatyCode != null)
                        {
                            riDataConfig.TreatyId = treatyCode.TreatyId;
                            riDataConfig.TreatyCodeId = null;

                            db.Entry(riDataConfig).State = EntityState.Modified;

                            SetProcessCount("Updated");
                        }
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
