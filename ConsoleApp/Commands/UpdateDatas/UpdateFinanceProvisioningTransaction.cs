using DataAccess.Entities;
using DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.UpdateDatas
{
    public class UpdateFinanceProvisioningTransaction : Command
    {
        public UpdateFinanceProvisioningTransaction()
        {
            Title = "UpdateFinanceProvisioningTransaction";
            Description = "To update finance provisioning transactions";
            Hide = true;
        }

        public override void Run()
        {
            PrintStarting();

            using (var db = new AppDbContext())
            {
                var claimRegisters = db.FinanceProvisioningTransactions.OrderBy(q => q.CreatedAt).GroupBy(q => q.ClaimRegisterId).ToList();
                foreach (var claimRegister in claimRegisters)
                {
                    int sortIndex = 1;
                    foreach (FinanceProvisioningTransaction transaction in claimRegister.Skip(1).ToList())
                    {
                        transaction.SortIndex = sortIndex;
                        sortIndex++;

                        db.Entry(transaction).State = EntityState.Modified;

                        SetProcessCount("Updated Transaction");
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
