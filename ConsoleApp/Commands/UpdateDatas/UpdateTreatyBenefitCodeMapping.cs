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
    public class UpdateTreatyBenefitCodeMapping : Command
    {
        public bool IsUpdateProfitComm { get; set; }

        public UpdateTreatyBenefitCodeMapping()
        {
            Title = "UpdateTreatyBenefitCodeMapping";
            Description = "To update Treaty Benefit Code Mapping table";
            Options = new string[] {
                "--p|updateProfitComm : Update Profit Commission",
            };
            Hide = true;
        }

        public override void Initial()
        {
            base.Initial();

            IsUpdateProfitComm = IsOption("updateProfitComm");
        }

        public override void Run()
        {
            PrintStarting();

            using (var db = new AppDbContext())
            {
                int total = db.TreatyBenefitCodeMappings.Count();
                int processed = 0;

                while (processed < total)
                {
                    if (PrintCommitBuffer())
                    {
                        db.SaveChanges();
                    }
                    SetProcessCount();

                    var treatyBenefitCodeMapping = db.TreatyBenefitCodeMappings.OrderBy(q => q.Id).Skip(processed).FirstOrDefault();
                    if (treatyBenefitCodeMapping != null)
                    {
                        if (IsUpdateProfitComm)
                        {
                            var profitCommPickListDetail = db.PickListDetails.Where(q => q.PickListId == PickListBo.ProfitComm && q.Code == treatyBenefitCodeMapping.ProfitComm).FirstOrDefault();
                            if (profitCommPickListDetail != null)
                            {
                                treatyBenefitCodeMapping.ProfitComm = null;
                                treatyBenefitCodeMapping.ProfitCommPickListDetailId = profitCommPickListDetail.Id;

                                db.Entry(treatyBenefitCodeMapping).State = EntityState.Modified;

                                SetProcessCount("Updated Profit Commission");
                            }
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
