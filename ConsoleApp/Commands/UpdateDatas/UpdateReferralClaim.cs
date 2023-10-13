using BusinessObject;
using DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.UpdateDatas
{
    public class UpdateReferralClaim : Command
    {
        public UpdateReferralClaim()
        {
            Title = "UpdateReferralClaim";
            Description = "To update referral claims";
            Hide = true;
        }

        public override void Run()
        {
            PrintStarting();

            using (var db = new AppDbContext())
            {
                int total = db.ReferralClaims.Count();
                int processed = 0;

                while (processed < total)
                {
                    if (PrintCommitBuffer())
                    {
                        db.SaveChanges();
                    }
                    SetProcessCount();

                    var referralClaim = db.ReferralClaims.OrderBy(q => q.Id).Skip(processed).FirstOrDefault();
                    if (referralClaim != null)
                    {
                        if (!referralClaim.ClaimRegisterId.HasValue)
                        {
                            processed++;
                            continue;
                        }

                        var claimRegister = db.ClaimRegister.Where(q => q.Id == referralClaim.ClaimRegisterId).FirstOrDefault();
                        if (claimRegister != null)
                        {
                            claimRegister.ReferralClaimId = referralClaim.Id;
                            db.Entry(claimRegister).State = EntityState.Modified;
                            SetProcessCount("Updated Claim Register");
                        }

                        //referralClaim = db.ReferralClaims.Where(q => q.Id == referralClaim.Id).FirstOrDefault();
                        //referralClaim.ClaimRegisterId = null;
                        //db.Entry(referralClaim).State = EntityState.Modified;
                        //SetProcessCount("Cleared Claim Register Id");
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
