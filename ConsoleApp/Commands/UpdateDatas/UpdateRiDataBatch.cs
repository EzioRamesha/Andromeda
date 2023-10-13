using BusinessObject.RiDatas;
using DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.UpdateDatas
{
    class UpdateRiDataBatch : Command
    {
        public bool IsUpdateTreatyCode { get; set; }

        public bool IsUpdateWarehouseStatus { get; set; }

        public UpdateRiDataBatch()
        {
            Title = "UpdateRiDataBatch";
            Description = "To update treaty code id to treaty id";
            Options = new string[] {
                "--t|updateTreatyCode : Update Treaty Code Id to Treaty Id",
                "--w|updateWarehouseStatus : Update Compile Status",
            };
            Hide = true;
        }

        public override void Initial()
        {
            base.Initial();

            IsUpdateTreatyCode = IsOption("updateTreatyCode");
            IsUpdateWarehouseStatus = IsOption("updateWarehouseStatus");
        }

        public override void Run()
        {
            PrintStarting();

            using (var db = new AppDbContext())
            {
                int total = db.RiDataBatches.Count();
                int processed = 0;

                while (processed < total)
                {
                    if (PrintCommitBuffer())
                    {
                        db.SaveChanges();
                    }
                    SetProcessCount();

                    var riDataBatch = db.RiDataBatches.OrderBy(q => q.Id).Skip(processed).FirstOrDefault();
                    if (riDataBatch != null)
                    {
                        if (IsUpdateTreatyCode)
                        {
                            var treatyCode = db.TreatyCodes.Where(q => q.Id == riDataBatch.TreatyCodeId).FirstOrDefault();
                            if (treatyCode != null)
                            {
                                riDataBatch.TreatyId = treatyCode.TreatyId;
                                riDataBatch.TreatyCodeId = null;

                                db.Entry(riDataBatch).State = EntityState.Modified;

                                SetProcessCount("Updated Treaty Code");
                            }
                        }
                        
                        if (IsUpdateWarehouseStatus)
                        {
                            bool isUpdated = false;
                            if (riDataBatch.ProcessWarehouseStatus == 0)
                            {
                                riDataBatch.ProcessWarehouseStatus = riDataBatch.Status == RiDataBatchBo.StatusFinalised ? RiDataBatchBo.ProcessWarehouseStatusPending : RiDataBatchBo.ProcessWarehouseStatusNotApplicable;
                                isUpdated = true;
                            }
                            if (!riDataBatch.FinalisedAt.HasValue && riDataBatch.Status == RiDataBatchBo.StatusFinalised)
                            {
                                riDataBatch.FinalisedAt = riDataBatch.UpdatedAt;
                                isUpdated = true;
                            }

                            if (isUpdated)
                            {
                                db.Entry(riDataBatch).State = EntityState.Modified;

                                SetProcessCount("Updated Process Warehouse Status");
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
