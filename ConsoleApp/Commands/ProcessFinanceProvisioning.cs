using BusinessObject;
using Services;
using Shared;
using System;

namespace ConsoleApp.Commands
{
    public class ProcessFinanceProvisioning : Command
    {
        public FinanceProvisioningBo FinanceProvisioningBo { get; set; }

        public int? FinanceProvisioningId { get; set; }

        public bool IsReprocess { get; set; } = true;

        public bool IsForceUpdateStatus { get; set; } = false;

        public ProcessFinanceProvisioning()
        {
            Title = "ProcessFinanceProvisioning";
            Description = "To process Finance Provisioning";
            Options = new string[] {
                "--id= : Finance Provisioning Id",
                "--r|reprocess : Reprocess Finance Provisioning",
                "--f|forceUpdateStatus : Force Update ",
            };
        }

        public override void Initial()
        {
            base.Initial();

            FinanceProvisioningId = OptionIntegerNullable("id");
            bool? isReprocess = IsOptionNullable("reprocess");
            bool? isForceUpdateStatus = IsOptionNullable("forceUpdateStatus");

            IsReprocess = isReprocess ?? false;
            IsForceUpdateStatus = isForceUpdateStatus ?? false;
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
            try
            {
                if (FinanceProvisioningId.HasValue)
                {
                    FinanceProvisioningBo = FinanceProvisioningService.Find(FinanceProvisioningId.Value);
                    if (FinanceProvisioningBo != null && FinanceProvisioningBo.Status != FinanceProvisioningBo.StatusSubmitForProcessing)
                    {
                        Log = false;
                        PrintMessage(MessageBag.NoBatchPendingProcess);
                        return;
                    }
                }
                else if (FinanceProvisioningService.CountByStatus(FinanceProvisioningBo.StatusSubmitForProcessing) == 0)
                {
                    PrintMessage(MessageBag.NoFinanceProvisioningPendingProcess);
                    return;
                }

                PrintStarting();

                while (LoadFinanceProvisioningBo() != null)
                {
                    try
                    {
                        if (GetProcessCount("Process") > 0)
                            PrintProcessCount();
                        SetProcessCount("Process");

                        var generateE3E4 = new GenerateE3E4(FinanceProvisioningBo, IsReprocess, IsForceUpdateStatus);
                        generateE3E4.Run();
                    }
                    catch (Exception e)
                    {
                        PrintError(e.Message);
                    }
                }

                if (GetProcessCount("Process") > 0)
                    PrintProcessCount();

                PrintEnding();
            }
            catch (Exception e)
            {
                PrintError(e.Message);
            }
        }

        public FinanceProvisioningBo LoadFinanceProvisioningBo()
        {
            FinanceProvisioningBo = null;

            if (FinanceProvisioningId.HasValue)
            {
                FinanceProvisioningBo = FinanceProvisioningService.Find(FinanceProvisioningId.Value);
                if (FinanceProvisioningBo != null && FinanceProvisioningBo.Status != FinanceProvisioningBo.StatusSubmitForProcessing)
                    FinanceProvisioningBo = null;
            }
            else
            {
                FinanceProvisioningBo = FinanceProvisioningService.FindByStatus(FinanceProvisioningBo.StatusSubmitForProcessing);
            }

            return FinanceProvisioningBo;
        }
    }
}
