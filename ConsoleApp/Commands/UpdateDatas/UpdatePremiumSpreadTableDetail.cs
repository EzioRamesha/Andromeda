using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using System.Data.Entity;
using System.Linq;


namespace ConsoleApp.Commands.UpdateDatas
{
    public class UpdatePremiumSpreadTableDetail : Command
    {
        public UpdatePremiumSpreadTableDetail()
        {
            Title = "UpdatePremiumSpreadTableDetail";
            Description = "To update Benefit Code from previous Benefit Code ID";
            Options = new string[] {
            };
            Hide = true;
        }

        public override void Run()
        {
            PrintStarting();

            using (var db = new AppDbContext())
            {
                int total = db.PremiumSpreadTableDetails.Count();
                int processed = 0;

                while (processed < total)
                {
                    if (PrintCommitBuffer())
                    {
                        db.SaveChanges();
                    }
                    SetProcessCount();

                    var premiumSpreadTableDetail = db.PremiumSpreadTableDetails.OrderBy(q => q.Id).Skip(processed).FirstOrDefault();

                    if (premiumSpreadTableDetail != null)
                    {
                        if (premiumSpreadTableDetail.BenefitId.HasValue)
                        {
                            var benefit = Benefit.Find(premiumSpreadTableDetail.BenefitId.Value);
                            premiumSpreadTableDetail.BenefitCode = benefit?.Code;
                        }
                        db.Entry(premiumSpreadTableDetail).State = EntityState.Modified;
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
