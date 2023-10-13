using BusinessObject;
using DataAccess.EntityFramework;
using System.Data.Entity;
using System.Linq;

namespace ConsoleApp.Commands.UpdateDatas
{
    public class UpdateCellMapping : Command
    {
        public bool IsUpdateTreatyCode { get; set; }

        public bool IsUpdateProfitComm { get; set; }

        public UpdateCellMapping()
        {
            Title = "UpdateCellMapping";
            Description = "To update Mfrs17 Cell Mapping table";
            Options = new string[] {
                "--t|updateTreatyCode : Update Treaty Code",
                "--p|updateProfitComm : Update Profit Commission",
            };
            Hide = true;
        }

        public override void Initial()
        {
            base.Initial();

            IsUpdateTreatyCode = IsOption("updateTreatyCode");
            IsUpdateProfitComm = IsOption("updateProfitComm");
        }

        public override void Run()
        {
            PrintStarting();

            using (var db = new AppDbContext())
            {
                int total = db.Mfrs17CellMappings.Count();
                int processed = 0;

                while (processed < total)
                {
                    if (PrintCommitBuffer())
                    {
                        db.SaveChanges();
                    }
                    SetProcessCount();

                    var cellMapping = db.Mfrs17CellMappings.OrderBy(q => q.Id).Skip(processed).FirstOrDefault();
                    if (cellMapping != null)
                    {
                        if (IsUpdateTreatyCode)
                        {
                            var treatyCode = db.TreatyCodes.Where(q => q.Id == cellMapping.TreatyCodeId).FirstOrDefault();
                            if (treatyCode != null)
                            {
                                cellMapping.TreatyCode = treatyCode.Code;
                                cellMapping.TreatyCodeId = null;

                                db.Entry(cellMapping).State = EntityState.Modified;

                                SetProcessCount("Updated Treaty Code");
                            }
                        }

                        if (IsUpdateProfitComm)
                        {
                            var profitCommPickListDetail = db.PickListDetails.Where(q => q.PickListId == PickListBo.ProfitComm && q.Code == cellMapping.ProfitComm).FirstOrDefault();
                            if (profitCommPickListDetail != null)
                            {
                                cellMapping.ProfitComm = null;
                                cellMapping.ProfitCommPickListDetailId = profitCommPickListDetail.Id;

                                db.Entry(cellMapping).State = EntityState.Modified;

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
