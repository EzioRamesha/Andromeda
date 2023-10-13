using DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.UpdateDatas
{
    class UpdateRiDataFile : Command
    {
        public UpdateRiDataFile()
        {
            Title = "UpdateRiDataFile";
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
                int total = db.RiDataFiles.Count();
                int processed = 0;

                while (processed < total)
                {
                    if (PrintCommitBuffer())
                    {
                        db.SaveChanges();
                    }
                    SetProcessCount();

                    var riDataFile = db.RiDataFiles.OrderBy(q => q.Id).Skip(processed).FirstOrDefault();
                    if (riDataFile != null)
                    {
                        var treatyCode = db.TreatyCodes.Where(q => q.Id == riDataFile.TreatyCodeId).FirstOrDefault();
                        if (treatyCode != null)
                        {
                            riDataFile.TreatyId = treatyCode.TreatyId;
                            riDataFile.TreatyCodeId = null;

                            db.Entry(riDataFile).State = EntityState.Modified;

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
