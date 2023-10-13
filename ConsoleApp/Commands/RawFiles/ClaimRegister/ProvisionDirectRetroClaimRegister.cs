using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities.Identity;
using Newtonsoft.Json;
using Services;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.RawFiles.ClaimRegister
{
    public class ProvisionDirectRetroClaimRegister
    {
        public ProvisionDirectRetroClaimRegisterBatch ProvisionDirectRetroClaimRegisterBatch { get; set; }

        public ClaimRegisterBo ClaimRegisterBo { get; set; }

        public DirectRetroConfigurationBo DirectRetroConfigurationBo { get; set; }

        public FinanceProvisioningTransactionBo FinanceProvisioningTransactionBo { get; set; }

        public IList<DirectRetroProvisioningTransactionBo> Transactions { get; set; }

        public IList<string> Errors { get; set; }

        public int FinanceProvisioningId { get; set; }

        public double TotalAmount { get; set; }

        public bool IsSuccess { get; set; }

        public bool Update { get; set; }

        public ProvisionDirectRetroClaimRegister(ProvisionDirectRetroClaimRegisterBatch batch, ClaimRegisterBo claimRegister, int financeProvisioningId)
        {
            ProvisionDirectRetroClaimRegisterBatch = batch;
            ClaimRegisterBo = claimRegister;
            FinanceProvisioningId = financeProvisioningId;
            Update = true;
        }

        public ProvisionDirectRetroClaimRegister(ClaimRegisterBo claimRegister, int financeProvisioningId)
        {
            ClaimRegisterBo = claimRegister;
            FinanceProvisioningId = financeProvisioningId;
            Update = false;
        }

        public void Provision()
        {
            IsSuccess = true;
            TotalAmount = 0;

            if (!Validate())
                return;

            if (!LoadDirectRetroConfiguration())
                return;

            Transactions = new List<DirectRetroProvisioningTransactionBo>();

            var detailBos = DirectRetroConfigurationDetailService.GetByClaimProvisioningParam(DirectRetroConfigurationBo.Id, ClaimRegisterBo.DateOfEvent, ClaimRegisterBo.IssueDatePol, ClaimRegisterBo.ReinsEffDatePol, false);
            if (detailBos.Count == 0)
            {
                detailBos = DirectRetroConfigurationDetailService.GetDefaultByParentId(DirectRetroConfigurationBo.Id, false);
            }

            FinanceProvisioningTransactionBo = FinanceProvisioningTransactionService.FindLatestByClaimRegisterId(FinanceProvisioningId, ClaimRegisterBo.Id);

            if (detailBos.Count == 0)
            {
                Errors.Add(string.Format("There are no Retro Parties found with the params"));
            }
            else if (detailBos.Count > 3)
            {
                Errors.Add(string.Format("There are more than 3 Retro Parties found with the params"));
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    int index = i + 1;
                    string retroPartyField = string.Format("RetroParty{0}", index);
                    string retroRecoveryField = string.Format("RetroRecovery{0}", index);
                    string retroShareField = string.Format("RetroShare{0}", index);

                    string partyCode = null;
                    double? share = null;
                    double? claimAmount = null;
                    if (detailBos.Count > i)
                    {
                        var detailBo = detailBos[i];
                        share = detailBo.Share;
                        partyCode = detailBo.RetroPartyBo.Party;
                        double computeShare = share.Value / 100;
                        claimAmount = Util.RoundValue(computeShare * ClaimRegisterBo.ClaimRecoveryAmt.Value, 2);
                    }

                    ClaimRegisterBo.SetPropertyValue(retroPartyField, partyCode);
                    ClaimRegisterBo.SetPropertyValue(retroShareField, share);
                    ClaimRegisterBo.SetPropertyValue(retroRecoveryField, claimAmount);

                    FinanceProvisioningTransactionBo.SetPropertyValue(retroPartyField, partyCode);
                    FinanceProvisioningTransactionBo.SetPropertyValue(retroShareField, share);
                    FinanceProvisioningTransactionBo.SetPropertyValue(retroRecoveryField, claimAmount);

                    CreateDirectRetroProvisionTransaction(partyCode, claimAmount ?? 0, true);
                }

                ReversePreviousDirectRetroProvision();
                SaveDirectRetroProvisionTransaction();
            }
            UpdateClaimRegister();
        }


        protected bool LoadDirectRetroConfiguration()
        {
            if (ProvisionDirectRetroClaimRegisterBatch != null)
                DirectRetroConfigurationBo = ProvisionDirectRetroClaimRegisterBatch.GetDirectRetroConfiguration(ClaimRegisterBo.TreatyCode);
            else
                DirectRetroConfigurationBo = DirectRetroConfigurationService.FindByTreatyCode(ClaimRegisterBo.TreatyCode);

            if (DirectRetroConfigurationBo == null)
                UpdateClaimRegister();

            return DirectRetroConfigurationBo != null;
        }

        public bool Validate()
        {
            ClaimRegisterBo bo = ClaimRegisterBo;
            Errors = new List<string>();

            List<int> required = new List<int>
            {
                StandardClaimDataOutputBo.TypeTreatyCode,
                StandardClaimDataOutputBo.TypeClaimRecoveryAmt,
            };

            foreach (int type in required)
            {
                string property = StandardClaimDataOutputBo.GetPropertyNameByType(type);
                object value = bo.GetPropertyValue(property);
                if (value == null)
                {
                    Errors.Add(string.Format("{0} is empty", property));
                }
                else if (value is string @string && string.IsNullOrEmpty(@string))
                {
                    Errors.Add(string.Format("{0} is empty", property));
                }
            }

            if (Errors.Count > 0)
                UpdateClaimRegister();

            return Errors.Count == 0;
        }

        public void ReversePreviousDirectRetroProvision()
        {
            IList<DirectRetroProvisioningTransactionBo> transactionBos = DirectRetroProvisioningTransactionService.GetByClaimRegisterId(ClaimRegisterBo.Id).OrderByDescending(q => q.Id).ToList();
            foreach (DirectRetroProvisioningTransactionBo directRetroProvisioningTransactionBo in transactionBos)
            {
                DirectRetroProvisioningTransactionBo transactionBo = directRetroProvisioningTransactionBo;

                var unchangedTransaction = Transactions.Where(q => q.RetroParty == transactionBo.RetroParty).Where(q => q.RetroRecovery == transactionBo.RetroRecovery).FirstOrDefault();
                if (unchangedTransaction != null)
                {
                    if (transactionBo.FinanceProvisioningStatus == FinanceProvisioningBo.StatusPending)
                    {
                        transactionBo.ClaimId = ClaimRegisterBo.ClaimId;
                        transactionBo.CedingCompany = ClaimRegisterBo.CedingCompany;
                        transactionBo.EntryNo = ClaimRegisterBo.EntryNo;
                        transactionBo.Quarter = ClaimRegisterBo.SoaQuarter;
                        transactionBo.TreatyCode = ClaimRegisterBo.TreatyCode;
                        transactionBo.TreatyType = ClaimRegisterBo.TreatyType;
                        transactionBo.ClaimCode = ClaimRegisterBo.ClaimCode;
                        DirectRetroProvisioningTransactionService.Update(ref transactionBo);
                    }
                    Transactions.Remove(unchangedTransaction);
                    continue;
                }

                transactionBo.IsLatestProvision = false;
                DirectRetroProvisioningTransactionService.Update(ref transactionBo);

                double reverseProvisionAmount = transactionBo.RetroRecovery * -1;
                CreateDirectRetroProvisionTransaction(transactionBo.RetroParty, reverseProvisionAmount, historyTransactionBo: transactionBo);
            }
        }

        public void CreateDirectRetroProvisionTransaction(string retroParty, double amount, bool isLatestProvision = false, DirectRetroProvisioningTransactionBo historyTransactionBo = null)
        {
            if (amount == 0)
                return;

            DirectRetroProvisioningTransactionBo transactionBo = new DirectRetroProvisioningTransactionBo()
            {
                ClaimRegisterId = ClaimRegisterBo.Id,
                FinanceProvisioningId = FinanceProvisioningId,
                IsLatestProvision = isLatestProvision,
                ClaimId = historyTransactionBo != null ? historyTransactionBo.ClaimId : ClaimRegisterBo.ClaimId,
                CedingCompany = historyTransactionBo != null ? historyTransactionBo.CedingCompany : ClaimRegisterBo.CedingCompany,
                EntryNo = historyTransactionBo != null ? historyTransactionBo.EntryNo : ClaimRegisterBo.EntryNo,
                Quarter = historyTransactionBo != null ? historyTransactionBo.Quarter : ClaimRegisterBo.SoaQuarter,
                RetroParty = retroParty,
                RetroRecovery = amount,
                TreatyCode = historyTransactionBo != null ? historyTransactionBo.TreatyCode : ClaimRegisterBo.TreatyCode,
                TreatyType = historyTransactionBo != null ? historyTransactionBo.TreatyType : ClaimRegisterBo.TreatyType,
                ClaimCode = historyTransactionBo != null ? historyTransactionBo.ClaimCode : ClaimRegisterBo.ClaimCode,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };

            if (isLatestProvision)
                Transactions.Add(transactionBo);
            else
                Transactions.Insert(0, transactionBo);
        }

        public void SaveDirectRetroProvisionTransaction()
        {
            foreach (var bo in Transactions)
            {
                var transactionBo = bo;

                TotalAmount += transactionBo.RetroRecovery;

                DirectRetroProvisioningTransactionService.Create(ref transactionBo);
            }
        }

        public void UpdateClaimRegister()
        {
            var claimRegisterBo = ClaimRegisterBo;
            var financeProvisioningTransactionBo = FinanceProvisioningTransactionBo;
            if (Errors.IsNullOrEmpty())
            {
                claimRegisterBo.DrProvisionStatus = ClaimRegisterBo.DrProvisionStatusSuccess;
                if (!string.IsNullOrEmpty(claimRegisterBo.ProvisionErrors))
                {
                    claimRegisterBo.ProvisionErrors = null;
                }

                // MLRe Retain Amount computation
                var directRetroTreaty = DirectRetroConfigurationService.FindByTreatyCode(claimRegisterBo.TreatyCode, false);

                var claimRecoveryAmt = claimRegisterBo.ClaimRecoveryAmt.HasValue ? claimRegisterBo.ClaimRecoveryAmt : 0;
                var retroRecovery1 = claimRegisterBo.RetroRecovery1.HasValue ? claimRegisterBo.RetroRecovery1 : 0;
                var retroRecovery2 = claimRegisterBo.RetroRecovery2.HasValue ? claimRegisterBo.RetroRecovery2 : 0;
                var retroRecovery3 = claimRegisterBo.RetroRecovery3.HasValue ? claimRegisterBo.RetroRecovery3 : 0;

                if (directRetroTreaty != null)
                {
                    claimRegisterBo.MlreRetainAmount = claimRecoveryAmt - retroRecovery1 - retroRecovery2 - retroRecovery3;
                }
                else
                {
                    claimRegisterBo.MlreRetainAmount = claimRecoveryAmt;
                }
            }
            else
            {
                IsSuccess = false;
                claimRegisterBo.DrProvisionStatus = ClaimRegisterBo.DrProvisionStatusFailed;
                claimRegisterBo.ProvisionErrors = JsonConvert.SerializeObject(Errors);
            }

            if (!Update)
                return;

            TrailObject trail = new TrailObject();

            Result result = ClaimRegisterService.Update(ref claimRegisterBo, ref trail);
            if (result.Valid)
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    claimRegisterBo.Id,
                    "Claim Provision Direct Retro",
                    result,
                    trail,
                    User.DefaultSuperUserId
                );
                UserTrailService.Create(ref userTrailBo);
            }

            Result result2 = FinanceProvisioningTransactionService.Update(ref financeProvisioningTransactionBo, ref trail);
            if (result2.Valid)
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    claimRegisterBo.Id,
                    "Finance Provisioning Transaction",
                    result2,
                    trail,
                    User.DefaultSuperUserId
                );
                UserTrailService.Create(ref userTrailBo);
            }
        }
    }
}
