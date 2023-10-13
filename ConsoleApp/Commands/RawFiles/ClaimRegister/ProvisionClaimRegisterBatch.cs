using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities.Identity;
using Newtonsoft.Json;
using Services;
using Services.Identity;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.RawFiles.ClaimRegister
{
    public class ProvisionClaimRegisterBatch : Command
    {
        public IList<ClaimRegisterBo> ClaimRegisterBos { get; set; }

        public IList<ProvisionClaimRegister> ProvisionClaimRegisters { get; set; }

        public FinanceProvisioningBo FinanceProvisioningBo { get; set; }

        public int? ClaimDataBatchId { get; set; }

        public bool IsGenerateE3E4 { get; set; } = true;

        public bool Test { get; set; } = false;

        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 100;

        public ProvisionClaimRegisterBatch()
        {
            Title = "ProvisionClaimRegisterBatch";
            Description = "To provision Claim Register";
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
            if (!ClaimDataBatchId.HasValue && ClaimRegisterService.CountPendingProvision() == 0)
            {
                Log = false;
                PrintMessage(MessageBag.NoClaimRegisterPendingProvision);

                if (IsPendingFinanceProvisioning())
                {
                    GenerateE3E4 genE3E4 = new GenerateE3E4(FinanceProvisioningBo);
                    genE3E4.Run();
                }

                return;
            }

            if (Title == "ProvisionClaimRegisterBatch")
                PrintStarting();
            else
            {
                PrintMessage();
                PrintMessage("Claim Provisioning");
            }

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

            if (IsGenerateE3E4 && FinanceProvisioningBo != null)
            {
                GenerateE3E4 generateE3E4 = new GenerateE3E4(FinanceProvisioningBo);
                generateE3E4.Run();
            }

            if (Title != "ProvisionClaimRegisterBatch")
                PrintEnding();
        }

        public bool GetNextBulkClaimRegister()
        {
            ClaimRegisterBos = new List<ClaimRegisterBo> { };
            ProvisionClaimRegisters = new List<ProvisionClaimRegister> { };

            int total;
            if (ClaimDataBatchId.HasValue)
            {
                ClaimRegisterBos = ClaimRegisterService.GetByClaimDataBatchId(ClaimDataBatchId.Value, Skip, Take);
            }
            else
            {
                total = ClaimRegisterService.CountPendingProvision();
                if (total == 0)
                    return false;

                ClaimRegisterBos = ClaimRegisterService.GetPendingProvision(0, Take);
            }

            int count = ClaimRegisterBos.Count;
            if (count == 0)
                return false;

            bool isPendingTransaction = ClaimDataBatchId.HasValue;
            foreach (var bo in ClaimRegisterBos)
            {
                //bool reprocessDirectRetro = bo.ProvisionStatus == ClaimRegisterBo.ProvisionStatusPendingReprovision;
                ProvisionClaimRegisters.Add(new ProvisionClaimRegister(bo, FinanceProvisioningBo) { IsPendingTransaction = isPendingTransaction  });
            }

            Skip += count;
            return true;
        }

        public bool IsPendingFinanceProvisioning()
        {
            FinanceProvisioningBo = FinanceProvisioningService.FindByStatus(FinanceProvisioningBo.StatusPending);

            if (FinanceProvisioningBo != null)
                return true;

            return false;
        }

        public void RetrieveFinanceProvisioning()
        {
            if (IsPendingFinanceProvisioning())
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
