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
    public class ProvisionDirectRetroClaimRegisterBatch : Command
    {
        public int? ClaimDataBatchId { get; set; }

        public int? SoaDataBatchId { get; set; }

        public string TreatyCode { get; set; }

        public Dictionary<string, DirectRetroConfigurationBo> DirectRetroConfigurationBos { get; set; }

        public IList<ClaimRegisterBo> ClaimRegisterBos { get; set; }

        public IList<ProvisionDirectRetroClaimRegister> ProvisionDirectRetroClaimRegisters { get; set; }

        public FinanceProvisioningBo FinanceProvisioningBo { get; set; }

        // Statistics
        public int Total { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }

        public ProvisionDirectRetroClaimRegisterBatch()
        {
            Title = "ProvisionDirectRetroClaimRegisterBatch";
            Description = "To provision direct retro for claim register";
            LogIndex = 0;
            Skip = 0;
            Take = Util.GetConfigInteger("ProvisionDirectRetroClaimRegisterRow", 1000);

            DirectRetroConfigurationBos = new Dictionary<string, DirectRetroConfigurationBo>();
        }

        public override void Run()
        {
            if (Title == "ProvisionDirectRetroClaimRegisterBatch")
                PrintStarting();
            else
            {
                PrintMessage();
                PrintMessage("Direct Retro Provisioning");
            }

            RetrieveFinanceProvisioning();
            while (LoadClaimRegister())
            {
                Parallel.ForEach(ProvisionDirectRetroClaimRegisters, p => p.Provision());

                foreach (var p in ProvisionDirectRetroClaimRegisters)
                {
                    if (p.IsSuccess)
                    {
                        FinanceProvisioningBo.DrProvisionAmount += p.TotalAmount;
                        SetProcessCount("DR Provision Success");
                    }
                    else
                    {
                        SetProcessCount("DR Provision Failed");
                    }
                    SetProcessCount();
                }

                PrintProcessCount();
            }
            UpdateFinanceProvisioning();

            if (Title == "ProvisionDirectRetroClaimRegisterBatch")
                PrintEnding();
        }

        public bool LoadClaimRegister()
        {
            ClaimRegisterBos = new List<ClaimRegisterBo>();
            ProvisionDirectRetroClaimRegisters = new List<ProvisionDirectRetroClaimRegister>();

            if (ClaimDataBatchId.HasValue)
            {
                // Called from Reporting Claim
                Total = ClaimRegisterService.CountByClaimDataBatchId(ClaimDataBatchId.Value);
                ClaimRegisterBos = ClaimRegisterService.GetByClaimDataBatchId(ClaimDataBatchId.Value, Skip, Take);
            }
            else if (SoaDataBatchId.HasValue && !string.IsNullOrEmpty(TreatyCode))
            {
                // Called from Direct Retro Processing
                Total = ClaimRegisterService.CountPendingDirectRetro(SoaDataBatchId.Value, TreatyCode);
                ClaimRegisterBos = ClaimRegisterService.GetPendingDirectRetro(SoaDataBatchId.Value, TreatyCode, Skip, Take);
            }
            else
            {
                // Process Pending Direct Retro Provisioning
                Total = ClaimRegisterService.CountPendingDirectRetro(ClaimRegisterBo.DrProvisionStatusPending);
                ClaimRegisterBos = ClaimRegisterService.GetPendingDirectRetro(ClaimRegisterBo.DrProvisionStatusPending, Skip, Take);
            }

            foreach (var claimRegisterBo in ClaimRegisterBos)
            {
                ProvisionDirectRetroClaimRegisters.Add(new ProvisionDirectRetroClaimRegister(this, claimRegisterBo, FinanceProvisioningBo.Id));
                GetDirectRetroConfiguration(claimRegisterBo.TreatyCode);
            }

            int count = ClaimRegisterBos.Count();

            Skip += count;
            return count > 0;
        }

        public void SetDirectRetroConfiguration(DirectRetroConfigurationBo bo)
        {
            if (string.IsNullOrEmpty(TreatyCode))
                return;

            DirectRetroConfigurationBos[TreatyCode] = bo;
        }

        public DirectRetroConfigurationBo GetDirectRetroConfiguration(string treatyCode = null)
        {
            if (string.IsNullOrEmpty(treatyCode))
                treatyCode = TreatyCode;

            if (!DirectRetroConfigurationBos.ContainsKey(treatyCode))
            {
                DirectRetroConfigurationBos.Add(treatyCode, DirectRetroConfigurationService.FindByTreatyCode(treatyCode));
            }

            return DirectRetroConfigurationBos[treatyCode];
        }

        public bool IsPendingFinanceProvisioning()
        {
            FinanceProvisioningBo = FinanceProvisioningService.FindByStatus(FinanceProvisioningBo.StatusPending);

            return FinanceProvisioningBo != null;
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
