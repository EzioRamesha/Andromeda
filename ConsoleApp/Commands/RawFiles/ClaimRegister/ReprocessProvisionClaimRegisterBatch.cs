using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities.Identity;
using Services;
using Services.Identity;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.RawFiles.ClaimRegister
{
    public class ReprocessProvisionClaimRegisterBatch : Command
    {
        public IList<ClaimRegisterBo> ClaimRegisterBos { get; set; }

        public IList<ProvisionClaimRegister> ProvisionClaimRegisters { get; set; }

        public FinanceProvisioningBo FinanceProvisioningBo { get; set; }

        public bool Test { get; set; } = false;

        public int Take { get; set; } = 100;

        public ReprocessProvisionClaimRegisterBatch()
        {
            Title = "ReprocessProvisionClaimRegisterBatch";
            Description = "To reprocess failed provision Claim Register";
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
            if (ClaimRegisterService.CountByProvisionStatus(ClaimRegisterBo.ProvisionStatusPendingReprocess) == 0)
            {
                Log = false;
                PrintMessage(MessageBag.NoClaimRegisterPendingProvisionReprocess);
                return;
            }
            PrintStarting();

            RetrieveFinanceProvisioning();

            while (GetNextBulkClaimRegister())
            {
                Parallel.ForEach(ProvisionClaimRegisters, p => p.Provision());

                foreach (var p in ProvisionClaimRegisters)
                {
                    if (p.Success)
                    {
                        FinanceProvisioningBo.ClaimsProvisionAmount += p.Amount;
                        FinanceProvisioningBo.DrProvisionAmount += p.DrAmount;
                        SetProcessCount("Success");
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

            UpdateFinanceProvisioning();

            PrintEnding();
        }

        public bool GetNextBulkClaimRegister()
        {
            ClaimRegisterBos = new List<ClaimRegisterBo> { };
            ProvisionClaimRegisters = new List<ProvisionClaimRegister> { };
            int total = ClaimRegisterService.CountByProvisionStatus(ClaimRegisterBo.ProvisionStatusPendingReprocess);
            if (total == 0)
                return false;

            ClaimRegisterBos = ClaimRegisterService.GetByProvisionStatus(ClaimRegisterBo.ProvisionStatusPendingReprocess, 0, Take);
            foreach (var bo in ClaimRegisterBos)
            {
                ProvisionClaimRegisters.Add(new ProvisionClaimRegister(bo, FinanceProvisioningBo));
            }

            return true;
        }

        public void RetrieveFinanceProvisioning()
        {
            FinanceProvisioningBo = FinanceProvisioningService.FindByStatus(FinanceProvisioningBo.StatusPending);

            if (FinanceProvisioningBo != null)
                return;

            FinanceProvisioningBo = new FinanceProvisioningBo
            {
                Date = DateTime.Today,
                Status = FinanceProvisioningBo.StatusPending,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };
            var trail = new TrailObject();
            var financeProvisioningBo = FinanceProvisioningBo;
            var result = FinanceProvisioningService.Create(ref financeProvisioningBo, ref trail);

            var userTrailBo = new UserTrailBo(
                FinanceProvisioningBo.Id,
                "Create Finance Provisioning",
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }

        public void UpdateFinanceProvisioning()
        {
            var trail = new TrailObject();
            var financeProvisioningBo = FinanceProvisioningBo;

            var result = FinanceProvisioningService.Update(ref financeProvisioningBo, ref trail);
            var userTrailBo = new UserTrailBo(
                FinanceProvisioningBo.Id,
                "Update Finance Provisioning",
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }
    }
}
