using BusinessObject.Retrocession;
using DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.UpdateDatas
{
    public class UpdatePerLifeAggregation : Command
    {

        public UpdatePerLifeAggregation()
        {
            Title = "UpdatePerLifeAggregation";
            Description = "To update per life aggregation records";
            Hide = true;
        }

        public override void Run()
        {
            PrintStarting();

            using (var db = new AppDbContext())
            {
                int total = db.PerLifeAggregations.Count();
                int processed = 0;

                while (processed < total)
                {
                    if (PrintCommitBuffer())
                    {
                        db.SaveChanges();
                    }
                    SetProcessCount();

                    var perLifeAggregation = db.PerLifeAggregations.OrderBy(q => q.Id).Skip(processed).FirstOrDefault();
                    if (perLifeAggregation != null && perLifeAggregation.Status > PerLifeAggregationBo.StatusProcessing)
                    {
                        perLifeAggregation.Status += 1;

                        db.Entry(perLifeAggregation).State = EntityState.Modified;

                        SetProcessCount("Updated");
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
