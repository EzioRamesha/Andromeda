using BusinessObject.Identity;
using BusinessObject.RiDatas;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Services.Identity;
using Services.RiDatas;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Linq;

namespace ConsoleApp.Commands.UpdateDatas
{
    public class UpdateRiDataBatchFailedNumber : Command
    {
        public int? RiDataBatchId { get; set; }

        public bool Test { get; set; }

        public int? Limit { get; set; }

        public bool Trail { get; set; }

        public UpdateRiDataBatchFailedNumber()
        {
            Title = "UpdateRiDataBatchFailedNumber";
            Description = "To calculate the total number of failed status in RI Data";
            Options = new string[] {
                "--i|riDataBatchId= : Enter the RiDataBatchId",
                "--t|test : Test to count total number of failed status",
                "--l|limit= : Enter limit of records",
                "--trail : Save the UserTrail record",
            };
            Log = false; // do not log just print message
            Hide = true;
        }

        public override void Initial()
        {
            base.Initial();

            RiDataBatchId = OptionIntegerNullable("riDataBatchId");
            Test = IsOption("test");
            Limit = OptionIntegerNullable("limit");
            Trail = IsOption("trail");
        }

        public override bool Validate()
        {
            try
            {
                // nothing
            }
            catch (Exception e)
            {
                PrintError(e.Message);
                return false;
            }
            return base.Validate();
        }

        public override void Run()
        {
            PrintStarting();

            try
            {
                if (RiDataBatchId != null)
                {
                    var batch = RiDataBatchService.Find(RiDataBatchId.Value);
                    if (batch == null)
                    {
                        throw new Exception($"The RiDataBatchId does not exists! RiDataBatchId: {RiDataBatchId}");
                    }

                    using (var db = new AppDbContext(false))
                    {
                        RiDataBatchService.CountTotalFailed(ref batch, db);
                    }

                    if (Test)
                    {
                        PrintTotal(batch);
                    }
                    else
                    {
                        Save(ref batch);
                    }
                }
                else
                {
                    using (var db = new AppDbContext(false))
                    {
                        int total = db.GetRiDataBatches().Count();

                        while (GetProcessCount() < total)
                        {
                            if (Limit != 0 && GetProcessCount() >= Limit)
                                break;

                            PrintCommitBuffer();

                            var entity = db.GetRiDataBatches().OrderBy(q => q.Id).Skip(GetProcessCount()).Take(1).FirstOrDefault();
                            if (entity == null)
                            {
                                break;
                            }

                            var batch = RiDataBatchService.FormBo(entity, false);
                            RiDataBatchService.CountTotalFailed(ref batch, db);

                            if (Test)
                            {
                                PrintTotal(batch);
                            }
                            else
                            {
                                Save(ref batch);
                            }

                            SetProcessCount();
                        }

                        PrintProcessCount();
                    }
                }
            }
            catch (Exception e)
            {
                PrintError(e.Message);
            }

            PrintEnding();
        }

        public void Save(ref RiDataBatchBo batch)
        {
            TrailObject trail = new TrailObject();
            Result result = RiDataBatchService.Update(ref batch, ref trail);
            if (Trail)
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    batch.Id,
                    "Update total number of failed status",
                    result,
                    trail,
                    User.DefaultSuperUserId
                );
                UserTrailService.Create(ref userTrailBo);
            }
        }

        public void PrintTotal(RiDataBatchBo batch)
        {
            PrintDetail("RiDataBatchId", batch.Id);
            PrintDetail("TotalMappingFailedStatus", batch.TotalMappingFailedStatus);
            PrintDetail("TotalPreComputation1FailedStatus", batch.TotalPreComputation1FailedStatus);
            PrintDetail("TotalPreComputation2FailedStatus", batch.TotalPreComputation2FailedStatus);
            PrintDetail("TotalPreValidationFailedStatus", batch.TotalPreValidationFailedStatus);
            PrintDetail("TotalConflict", batch.TotalConflict);
            PrintDetail("TotalPostComputationFailedStatus", batch.TotalPostComputationFailedStatus);
            PrintDetail("TotalPostValidationFailedStatus", batch.TotalPostValidationFailedStatus);
            PrintDetail("TotalFinaliseFailedStatus", batch.TotalFinaliseFailedStatus);
            PrintMessage();
        }
    }
}
