using BusinessObject;
using Services;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.RawFiles.ClaimRegister
{
    class ProcessClaimRegisterBatch : Command
    {
        public bool Test { get; set; } = false;

        public int Take { get; set; } = 100;

        public IList<ClaimRegisterBo> ClaimRegisterBos { get; set; }

        public IList<ProcessClaimRegister> ProcessClaimRegisters { get; set; }

        public PickListDetailBo GroupFundsAccountingTypeBo { get; set; }

        public PickListDetailBo IndividualFundsAccountingTypeBo { get; set; }

        public ProcessClaimRegisterBatch()
        {
            Title = "ProcessClaimRegisterBatch";
            Description = "To process Claim Register";
            Options = new string[] {
                "--l|logIndex= : Log File Index",
                "--t|test : Test process data",
            };
        }

        public override void Initial()
        {
            base.Initial();

            Test = IsOption("test");
            Take = Util.GetConfigInteger("ProcessClaimRegisterItems", 100);
        }

        public override void Run()
        {
            if (CutOffService.IsCutOffProcessing())
            {
                Log = false;
                PrintMessage(MessageBag.ProcessCannotRunDueToCutOff, true, false);
                return;
            }
            if (ClaimRegisterService.CountPendingProcess() == 0)
            {
                Log = false;
                PrintMessage(MessageBag.NoClaimRegisterPendingProcess);
                return;
            }

            PrintStarting();

            GroupFundsAccountingTypeBo = PickListDetailService.FindByPickListIdCode(PickListBo.FundsAccountingType, "GROUP");
            IndividualFundsAccountingTypeBo = PickListDetailService.FindByPickListIdCode(PickListBo.FundsAccountingType, "INDIVIDUAL");

            while (GetNextBulkClaimRegister())
            {
                Parallel.ForEach(ProcessClaimRegisters, f => f.Process());

                foreach (var p in ProcessClaimRegisters)
                {
                    if (p.Success)
                    {
                        SetProcessCount("Success");
                        if (p.SuspectedDuplicate)
                        {
                            SetProcessCount("Suspected Duplicate");
                        }
                        else
                        {
                            var claimRegister = p.ClaimRegisterBo;
                            if (claimRegister.ClaimTransactionType == PickListDetailBo.ClaimTransactionTypeNew)
                            {
                                claimRegister.ClaimId = ClaimRegisterService.GetNextClaimId();
                            }

                            ClaimRegisterService.Update(ref claimRegister);
                        }
                    }
                    else
                    {
                        SetProcessCount("Failed");
                    }

                    SetProcessCount();
                }

                PrintProcessCount();

                if (Test)
                    break;
            }

            PrintEnding();
        }

        public bool GetNextBulkClaimRegister()
        {
            ClaimRegisterBos = new List<ClaimRegisterBo> { };
            ProcessClaimRegisters = new List<ProcessClaimRegister> { };

            if (CutOffService.IsCutOffProcessing())
                return false;

            int total = ClaimRegisterService.CountPendingProcess();
            if (total == 0)
                return false;

            ClaimRegisterBos = ClaimRegisterService.GetPendingProcess(0, Take);
            foreach (var claimRegisterBo in ClaimRegisterBos)
            {
                ProcessClaimRegisters.Add(new ProcessClaimRegister(this, claimRegisterBo));
            }

            return true;
        }
    }
}
