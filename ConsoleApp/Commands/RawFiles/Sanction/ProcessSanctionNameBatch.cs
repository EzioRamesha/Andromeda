using BusinessObject.Sanctions;
using Services.Sanctions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.RawFiles.Sanction
{
    public class ProcessSanctionNameBatch : Command
    {
        public int? SanctionBatchId { get; set; }

        public int? SanctionId { get; set; }

        public List<SanctionBo> SanctionBos { get; set; }

        public List<ProcessSanctionName> ProcessSanctionNames { get; set; }

        // Counts
        public int Total { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }

        // Print
        public bool PrintStartEnd { get; set; } = true;

        public string ProcessName { get; set; } = "Processed";

        public ProcessSanctionNameBatch()
        {
            Title = "ProcessSanctionNameBatch";
            Description = "To Process Sanction Name Batch";
            Options = new string[] {
                "--sanctionBatchId= : Process by Sanction Batch Id",
                "--sanctionId= : Process by Sanction Id",
            };
        }

        public override void Initial()
        {
            base.Initial();
            SanctionBatchId = OptionIntegerNullable("sanctionBatchId");
            SanctionId = OptionIntegerNullable("sanctionId");

            Take = Util.GetConfigInteger("ProcessSanctionNameRow", 1000);
        }

        public override void Run()
        {
            try
            {
                Skip = 0;
                if (PrintStartEnd)
                    PrintStarting();

                while (LoadSanctionBos())
                {
                    if (GetProcessCount(ProcessName) > 0)
                        PrintProcessCount();

                    Parallel.ForEach(ProcessSanctionNames, f => f.Process());

                    foreach (var s in ProcessSanctionNames)
                    {
                        SetProcessCount("Processed Name", s.SanctionNames.Count(), true);
                        SetProcessCount(ProcessName);
                    }
                }
                PrintProcessCount();

                if (PrintStartEnd)
                    PrintEnding();
            }
            catch (Exception e)
            {
                PrintError(e.Message);
            }
        }

        public bool LoadSanctionBos()
        {
            Total = 0;
            SanctionBos = new List<SanctionBo>();
            ProcessSanctionNames = new List<ProcessSanctionName>();
            if (SanctionBatchId.HasValue)
            {
                Total = SanctionService.CountBySanctionBatchId(SanctionBatchId.Value);

                IList<SanctionBo> sanctionBos = SanctionService.GetBySanctionBatchId(SanctionBatchId.Value, Skip, Take);
                SanctionBos.AddRange(sanctionBos);
            }
            else if (SanctionId.HasValue)
            {
                Total = 1;
                SanctionBos.Add(SanctionService.Find(SanctionId.Value));
            }
            else
            {
                Total = SanctionService.Count();
                SanctionBos.AddRange(SanctionService.Get(Skip, Take));
            }

            if (Skip > Total)
                return false;

            foreach (SanctionBo bo in SanctionBos)
            {
                ProcessSanctionNames.Add(new ProcessSanctionName(bo));
            }

            Skip += Take;
            return true;
        }
    }
}
