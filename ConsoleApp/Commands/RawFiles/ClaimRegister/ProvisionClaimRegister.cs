using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities.Identity;
using Newtonsoft.Json;
using Services;
using Services.Claims;
using Services.Identity;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;

namespace ConsoleApp.Commands.RawFiles.ClaimRegister
{
    public class ProvisionClaimRegister
    {
        //public ProvisionClaimRegisterBatch ProvisionClaimRegisterBatch { get; set; }

        public ClaimRegisterBo ClaimRegisterBo { get; set; }

        public DirectRetroConfigurationBo DirectRetroConfigurationBo { get; set; }

        public IList<DirectRetroConfigurationDetailBo> DirectRetroConfigurationDetailBos { get; set; }

        public FinanceProvisioningBo FinanceProvisioningBo { get; set; }

        public bool Success { get; set; }

        public bool DirectRetroNotFound { get; set; } = false;

        public double Amount { get; set; }

        public double DrAmount { get; set; }

        public List<string> Errors { get; set; }

        public bool ProvisionDirectRetro { get; set; }

        public int CurrentSortIndex { get; set; }

        public bool IsPendingTransaction { get; set; } = false;

        public ProvisionClaimRegister(ClaimRegisterBo claimRegisterBo, FinanceProvisioningBo financeProvisioningBo = null, bool provisionDirectRetro = true)
        {
            ClaimRegisterBo = claimRegisterBo;
            ProvisionDirectRetro = provisionDirectRetro;
            //ProvisionClaimRegisterBatch = provisionClaimRegisterBatch;
            Success = true;
            FinanceProvisioningBo = financeProvisioningBo;
            if (FinanceProvisioningBo == null)
                RetrieveFinanceProvisioning();
            CurrentSortIndex = -1;
        }

        public void Provision()
        {
            try
            {
                Validate();
                //GetDirectRetroConfiguration();
                Amount = 0;

                FinanceProvisioningTransactionBo currentTransactionBo = FinanceProvisioningTransactionService.FindLatestByClaimRegisterId(FinanceProvisioningBo.Id, ClaimRegisterBo.Id);
                if (currentTransactionBo == null)
                {
                    Amount += ReversePreviousProvision();
                }
                else
                {
                    CurrentSortIndex = currentTransactionBo.SortIndex;
                }

                UpdateReverseEntryLastTransaction();
                CreateProvisionTransaction(ClaimRegisterBo.ClaimRecoveryAmt.Value, ClaimRegisterBo.RetroRecovery1, ClaimRegisterBo.RetroRecovery2, ClaimRegisterBo.RetroRecovery3, isLatestProvision: true, transactionBo: currentTransactionBo);
                Amount += ClaimRegisterBo.ClaimRecoveryAmt.Value;

                DrAmount = 0;
                if (ProvisionDirectRetro && ClaimRegisterBo.ClaimTransactionType != PickListDetailBo.ClaimTransactionTypeBulk)
                {
                    var directRetro = new ProvisionDirectRetroClaimRegister(ClaimRegisterBo, FinanceProvisioningBo.Id);
                    directRetro.Provision();

                    Errors.AddRange(directRetro.Errors);

                    ClaimRegisterBo = directRetro.ClaimRegisterBo;
                    DrAmount += directRetro.TotalAmount;
                }
            }
            catch (Exception)
            {
                Success = false;
            }

            UpdateFinanceProvisioning();
            if (Success)
            {
                UpdateStatus(ClaimRegisterBo.ProvisionStatusProvisioning, Errors);
                //NotifyReprocessDirectRetro();
            }
            else
            {
                UpdateStatus(ClaimRegisterBo.ProvisionStatusFailed, Errors);
            }
        }

        public double ReversePreviousProvision()
        {
            double totalReversedProvisionAmount = 0;
            IList<FinanceProvisioningTransactionBo> transactionBos = FinanceProvisioningTransactionService.GetByClaimRegisterId(ClaimRegisterBo.Id);
            foreach (FinanceProvisioningTransactionBo transactionBo in transactionBos)
            {
                CurrentSortIndex = transactionBo.SortIndex;
                FinanceProvisioningTransactionBo bo = transactionBo;
                bo.IsLatestProvision = false;
                FinanceProvisioningTransactionService.Update(ref bo);

                double reverseProvisionAmount = bo.ClaimRecoveryAmount * -1;
                double? reverseRetroRecovery1 = bo.RetroRecovery1 * -1;
                double? reverseRetroRecovery2 = bo.RetroRecovery2 * -1;
                double? reverseRetroRecovery3 = bo.RetroRecovery3 * -1;
                totalReversedProvisionAmount += reverseProvisionAmount;
                CreateProvisionTransaction(reverseProvisionAmount, reverseRetroRecovery1, reverseRetroRecovery2, reverseRetroRecovery3, historyTransactionBo: bo);
            }
            return totalReversedProvisionAmount;
        }

        public void UpdateReverseEntryLastTransaction()
        {
            var bo = FinanceProvisioningTransactionService.FindNotLatestByClaimRegisterId(FinanceProvisioningBo.Id, ClaimRegisterBo.Id);
            if (bo != null)
            {
                bo.LastTransactionDate = ClaimRegisterBo.LastTransactionDate;
                bo.LastTransactionQuarter = ClaimRegisterBo.LastTransactionQuarter;
                FinanceProvisioningTransactionService.Update(ref bo);
            }
        }

        public void CreateProvisionTransaction(double amount, double? retroRecovery1, double? retroRecovery2, double? retroRecovery3, bool isLatestProvision = false, FinanceProvisioningTransactionBo transactionBo = null, FinanceProvisioningTransactionBo historyTransactionBo = null)
        {
            if (amount == 0)
                return;

            if (transactionBo == null)
            {
                CurrentSortIndex++;
                transactionBo = new FinanceProvisioningTransactionBo()
                {
                    ClaimRegisterId = ClaimRegisterBo.Id,
                    FinanceProvisioningId = FinanceProvisioningBo?.Id,
                    SortIndex = CurrentSortIndex,
                    CreatedById = User.DefaultSuperUserId,
                };
            }

            transactionBo.IsLatestProvision = isLatestProvision;
            transactionBo.ClaimId = historyTransactionBo != null ? historyTransactionBo.ClaimId : ClaimRegisterBo.ClaimId;
            transactionBo.PolicyNumber = historyTransactionBo != null ? historyTransactionBo.PolicyNumber : ClaimRegisterBo.PolicyNumber;
            transactionBo.CedingCompany = historyTransactionBo != null ? historyTransactionBo.CedingCompany : ClaimRegisterBo.CedingCompany;
            transactionBo.EntryNo = historyTransactionBo != null ? historyTransactionBo.EntryNo : ClaimRegisterBo.EntryNo;
            transactionBo.Quarter = historyTransactionBo != null ? historyTransactionBo.Quarter : ClaimRegisterBo.SoaQuarter;
            transactionBo.SumReinsured = historyTransactionBo != null ? historyTransactionBo.SumReinsured : (ClaimRegisterBo.AarPayable.HasValue ? ClaimRegisterBo.AarPayable.Value : 0);
            transactionBo.ClaimRecoveryAmount = amount;
            transactionBo.TreatyCode = historyTransactionBo != null ? historyTransactionBo.TreatyCode : ClaimRegisterBo.TreatyCode;
            transactionBo.TreatyType = historyTransactionBo != null ? historyTransactionBo.TreatyType : ClaimRegisterBo.TreatyType;
            transactionBo.ClaimCode = historyTransactionBo != null ? historyTransactionBo.ClaimCode : ClaimRegisterBo.ClaimCode;
            transactionBo.LastTransactionDate = ClaimRegisterBo.LastTransactionDate; // to update all the time
            transactionBo.LastTransactionQuarter = ClaimRegisterBo.LastTransactionQuarter; // to update all the time
            transactionBo.DateOfEvent = historyTransactionBo != null ? historyTransactionBo.DateOfEvent : ClaimRegisterBo.DateOfEvent;
            transactionBo.RiskQuarter = historyTransactionBo != null ? historyTransactionBo.RiskQuarter : ClaimRegisterBo.RiskQuarter;
            transactionBo.RiskPeriodYear = historyTransactionBo != null ? historyTransactionBo.RiskPeriodYear : ClaimRegisterBo.RiskPeriodYear;
            transactionBo.RiskPeriodMonth = historyTransactionBo != null ? historyTransactionBo.RiskPeriodMonth : ClaimRegisterBo.RiskPeriodMonth;
            transactionBo.FundsAccountingTypeCode = historyTransactionBo != null ? historyTransactionBo.FundsAccountingTypeCode : ClaimRegisterBo.FundsAccountingTypeCode;
            transactionBo.MlreBenefitCode = historyTransactionBo != null ? historyTransactionBo.MlreBenefitCode : ClaimRegisterBo.MlreBenefitCode;
            transactionBo.RetroParty1 = historyTransactionBo != null ? historyTransactionBo.RetroParty1 : ClaimRegisterBo.RetroParty1;
            transactionBo.RetroParty2 = historyTransactionBo != null ? historyTransactionBo.RetroParty2 : ClaimRegisterBo.RetroParty2;
            transactionBo.RetroParty3 = historyTransactionBo != null ? historyTransactionBo.RetroParty3 : ClaimRegisterBo.RetroParty3;
            transactionBo.RetroRecovery1 = retroRecovery1;
            transactionBo.RetroRecovery2 = retroRecovery2;
            transactionBo.RetroRecovery3 = retroRecovery3;
            transactionBo.ReinsEffDatePol = historyTransactionBo != null ? historyTransactionBo.ReinsEffDatePol : ClaimRegisterBo.ReinsEffDatePol;
            transactionBo.ReinsBasisCode = historyTransactionBo != null ? historyTransactionBo.ReinsBasisCode : ClaimRegisterBo.ReinsBasisCode;
            transactionBo.Mfrs17ContractCode = historyTransactionBo != null ? historyTransactionBo.Mfrs17ContractCode : ClaimRegisterBo.Mfrs17ContractCode;
            transactionBo.Mfrs17AnnualCohort = historyTransactionBo != null ? historyTransactionBo.Mfrs17AnnualCohort : ClaimRegisterBo.Mfrs17AnnualCohort;
            transactionBo.UpdatedById = User.DefaultSuperUserId;

            FinanceProvisioningTransactionService.Save(ref transactionBo);
        }

        //public void ReversePreviousDirectRetroProvision()
        //{
        //    IList<DirectRetroProvisioningTransactionBo> transactionBos = DirectRetroProvisioningTransactionService.GetByClaimRegisterId(ClaimRegisterBo.Id);
        //    foreach (DirectRetroProvisioningTransactionBo transactionBo in transactionBos)
        //    {
        //        DirectRetroProvisioningTransactionBo bo = transactionBo;
        //        bo.IsLatestProvision = false;
        //        DirectRetroProvisioningTransactionService.Update(ref bo);

        //        double reverseProvisionAmount = bo.RetroRecovery * -1;
        //        CreateDirectRetroProvisionTransaction(bo.RetroParty, reverseProvisionAmount);
        //    }
        //}

        //public void CreateDirectRetroProvisionTransaction(string retroParty, double amount, bool isLatestProvision = false)
        //{
        //DirectRetroProvisioningTransactionBo transactionBo = new DirectRetroProvisioningTransactionBo()
        //{
        //    ClaimRegisterId = ClaimRegisterBo.Id,
        //    FinanceProvisioningId = FinanceProvisioningBo?.Id,
        //    IsLatestProvision = isLatestProvision,
        //    ClaimId = ClaimRegisterBo.ClaimId,
        //    CedingCompany = ClaimRegisterBo.CedingCompany,
        //    EntryNo = ClaimRegisterBo.EntryNo,
        //    Quarter = ClaimRegisterBo.SoaQuarter,
        //    RetroParty = retroParty,
        //    RetroRecovery = amount,
        //    TreatyCode = ClaimRegisterBo.TreatyCode,
        //    TreatyType = ClaimRegisterBo.TreatyType,
        //    ClaimCode = ClaimRegisterBo.ClaimCode,
        //    CreatedById = User.DefaultSuperUserId,
        //    UpdatedById = User.DefaultSuperUserId,
        //};

        //    DirectRetroProvisioningTransactionService.Create(ref transactionBo);
        //}

        //public void GetDirectRetroConfiguration()
        //{
        //    DirectRetroConfigurationDetailBos = new List<DirectRetroConfigurationDetailBo>();
        //    DirectRetroConfigurationBo = DirectRetroConfigurationService.FindByTreatyCode(ClaimRegisterBo.TreatyCode);
        //    if (DirectRetroConfigurationBo == null)
        //    {
        //        Errors.Add(string.Format("Direct Retro Not Applicable"));
        //        DirectRetroNotFound = true;

        //        return;
        //    }

        //    DirectRetroConfigurationDetailBos = DirectRetroConfigurationDetailService.GetByClaimProvisioningParam(DirectRetroConfigurationBo.Id, ClaimRegisterBo.RiskPeriodMonth.Value, ClaimRegisterBo.RiskPeriodYear.Value, ClaimRegisterBo.IssueDatePol.Value, ClaimRegisterBo.ReinsEffDatePol.Value, false);
        //    if (DirectRetroConfigurationDetailBos.Count == 0)
        //    {
        //        Errors.Add(string.Format("There are no Retro Parties found with the params"));
        //        throw new Exception();
        //    }
        //    else if (DirectRetroConfigurationDetailBos.Count > 3)
        //    {
        //        Errors.Add(string.Format("There are more than 3 Retro Parties found with the params"));
        //        throw new Exception();
        //    }
        //}

        public void Validate()
        {
            ClaimRegisterBo bo = ClaimRegisterBo;
            Errors = new List<string>();

            List<int> required = new List<int>
            {
                //StandardClaimDataOutputBo.TypeRiskPeriodMonth,
                //StandardClaimDataOutputBo.TypeRiskPeriodYear,
                //StandardClaimDataOutputBo.TypeReinsEffDatePol,
                //StandardClaimDataOutputBo.TypeIssueDatePol,
                StandardClaimDataOutputBo.TypeClaimRecoveryAmt,
            };

            if (bo.ClaimTransactionType != PickListDetailBo.ClaimTransactionTypeBulk)
            {
                required.Add(StandardClaimDataOutputBo.TypeTreatyCode);
            }

            foreach (int type in required)
            {
                string property = StandardClaimDataOutputBo.GetPropertyNameByType(type);
                string name = StandardClaimDataOutputBo.GetTypeName(type);
                object value = bo.GetPropertyValue(property);
                if (value == null)
                {
                    Errors.Add(string.Format("{0} is empty", name));
                }
                else if (value is string @string && string.IsNullOrEmpty(@string))
                {
                    Errors.Add(string.Format("{0} is empty", name));
                }
            }

            if (Errors.Count > 0)
                throw new Exception();
        }

        public void UpdateStatus(int status, List<string> errors)
        {
            if (IsPendingTransaction)
                return;

            ClaimRegisterBo bo = ClaimRegisterBo;
            bo.ProvisionStatus = status;

            bo.ProvisionErrors = JsonConvert.SerializeObject(errors);

            ClaimRegisterService.Update(ref bo);
        }

        public void NotifyReprocessDirectRetro()
        {
            //if (!ReprocessDirectRetro || ClaimRegisterBo.ClaimTransactionType == PickListDetailBo.ClaimTransactionTypeBulk)
            //    return;

            if (ClaimRegisterBo.ClaimDataBatchId.HasValue)
            {
                var batchBo = ClaimDataBatchService.Find(ClaimRegisterBo.ClaimDataBatchId.Value);
                if (batchBo != null)
                {
                    if (batchBo.SoaDataBatchId.HasValue)
                    {
                        var directRetroBo = DirectRetroService.FindByClaimRegisterParam(batchBo.SoaDataBatchId.Value, ClaimRegisterBo.TreatyCode);
                        if (directRetroBo != null && directRetroBo.RetroStatus == DirectRetroBo.RetroStatusCompleted)
                        {
                            directRetroBo.RetroStatus = DirectRetroBo.RetroStatusSubmitForProcessing;
                            directRetroBo.UpdatedById = User.DefaultSuperUserId;
                            DirectRetroService.Update(ref directRetroBo);
                        }
                    }
                }
            }
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
            IsPendingTransaction = true;
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
            if (!IsPendingTransaction)
                return;

            var trail = new TrailObject();
            var financeProvisioningBo = FinanceProvisioningBo;
            FinanceProvisioningService.SumAmount(ref financeProvisioningBo);

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
