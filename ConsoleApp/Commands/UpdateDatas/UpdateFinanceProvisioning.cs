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
    public class UpdateFinanceProvisioning : Command
    {
        public UpdateFinanceProvisioning()
        {
            Title = "UpdateFinanceProvisioning";
            Description = "To update finance provisioning";
            Hide = true;
        }

        public override void Run()
        {
            PrintStarting();

            using (var db = new AppDbContext())
            {
                var financeProvisionings = db.FinanceProvisionings.ToList();
                foreach (var financeProvisioning in financeProvisionings)
                {
                    if (financeProvisioning.Status != FinanceProvisioningBo.StatusPending)
                    {
                        financeProvisioning.ProvisionAt = financeProvisioning.UpdatedAt;
                        db.Entry(financeProvisioning).State = EntityState.Modified;
                        SetProcessCount("Updated");
                    }

                    SetProcessCount();
                }
                db.SaveChanges();
            }

            PrintProcessCount();
            PrintEnding();
        }
    }
}
