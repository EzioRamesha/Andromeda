using DataAccess.EntityFramework;
using Newtonsoft.Json;
using Shared.Trails;
using System.Data.Entity;
using System.Linq;

namespace ConsoleApp.Commands.UpdateDatas
{
    public class UpdateUserTrail : Command
    {
        public int Limit { get; set; }

        public UpdateUserTrail()
        {
            Title = "UpdateUserTrail";
            Description = "To format trail";
            Options = new string[] {
                "--l|limit= : Enter limit of records",
            };
            Hide = true;
        }

        public override void Initial()
        {
            base.Initial();

            Limit = OptionInteger("limit", 0);
        }

        public override void Run()
        {
            PrintStarting();

            using (var db = new AppDbContext())
            {
                int total = db.UserTrails.Count();
                int processed = 0;

                while (processed < total)
                {
                    if (Limit != 0 && processed >= Limit)
                        break;

                    if (PrintCommitBuffer())
                        db.SaveChanges();
                    SetProcessCount();

                    var trail = db.UserTrails.OrderBy(q => q.Id).Skip(processed).Take(1).FirstOrDefault();
                    if (trail != null)
                    {
                        TrailObject trailObj = JsonConvert.DeserializeObject<TrailObject>(trail.Data);
                        trail.Data = trailObj.ToString();

                        db.Entry(trail).State = EntityState.Modified;
                    }

                    processed++;
                }
                db.SaveChanges();

                PrintProcessCount();
            }

            PrintEnding();
        }
    }
}
