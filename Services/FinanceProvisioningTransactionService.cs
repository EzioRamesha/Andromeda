using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services
{
    public class FinanceProvisioningTransactionService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(FinanceProvisioningTransaction)),
                Controller = ModuleBo.ModuleController.FinanceProvisioningTransaction.ToString(),
            };
        }



        public static Expression<Func<FinanceProvisioningTransaction, ClaimRegisterBo>> ClaimRegisterExpression()
        {
            return entity => new ClaimRegisterBo
            {
                Id = entity.ClaimRegister.Id,
                ClaimDataBatchId = entity.ClaimRegister.ClaimDataBatchId,
                ClaimDataId = entity.ClaimRegister.ClaimDataId,
                ClaimDataConfigId = entity.ClaimRegister.ClaimDataConfigId,
                SoaDataBatchId = entity.ClaimRegister.SoaDataBatchId,
                RiDataWarehouseId = entity.ClaimRegister.RiDataWarehouseId,
                ReferralRiDataId = entity.ClaimRegister.ReferralRiDataId,
                ClaimStatus = entity.ClaimRegister.ClaimStatus,
                ClaimDecisionStatus = entity.ClaimRegister.ClaimDecisionStatus,
                ReferralClaimId = entity.ClaimRegister.ReferralClaimId,
                OriginalClaimRegisterId = entity.ClaimRegister.OriginalClaimRegisterId,
                ClaimReasonId = entity.ClaimRegister.ClaimReasonId,
                PicDaaId = entity.ClaimRegister.PicDaaId,
                PicClaimId = entity.ClaimRegister.PicClaimId,
                ProvisionStatus = entity.FinanceProvisioning.ProvisionAt.HasValue ? ClaimRegisterBo.ProvisionStatusProvisioned : entity.ClaimRegister.ProvisionStatus,
                DrProvisionStatus = entity.ClaimRegister.DrProvisionStatus,
                TargetDateToIssueInvoice = entity.ClaimRegister.TargetDateToIssueInvoice,
                IsReferralCase = entity.ClaimRegister.IsReferralCase,
                OffsetStatus = entity.ClaimRegister.OffsetStatus,
                ClaimId = entity.ClaimId,
                ClaimCode = entity.ClaimCode,
                MappingStatus = entity.ClaimRegister.MappingStatus,
                ProcessingStatus = entity.ClaimRegister.ProcessingStatus,
                DuplicationCheckStatus = entity.ClaimRegister.DuplicationCheckStatus,
                PostComputationStatus = entity.ClaimRegister.PostComputationStatus,
                PostValidationStatus = entity.ClaimRegister.PostValidationStatus,
                Errors = entity.ClaimRegister.Errors,
                ProvisionErrors = entity.ClaimRegister.ProvisionErrors,
                RedFlagWarnings = entity.ClaimRegister.RedFlagWarnings,
                RequestUnderwriterReview = entity.ClaimRegister.RequestUnderwriterReview,
                UnderwriterFeedback = entity.ClaimRegister.UnderwriterFeedback,
                HasRedFlag = entity.ClaimRegister.HasRedFlag,

                PolicyNumber = entity.PolicyNumber,
                PolicyTerm = entity.ClaimRegister.PolicyTerm,
                ClaimRecoveryAmt = entity.ClaimRecoveryAmount,
                ClaimTransactionType = entity.SortIndex == 0 ? entity.ClaimRegister.ClaimTransactionType : "ADJ",
                TreatyCode = entity.TreatyCode,
                TreatyType = entity.TreatyType,
                AarPayable = entity.SumReinsured,
                AnnualRiPrem = entity.ClaimRegister.AnnualRiPrem,
                CauseOfEvent = entity.ClaimRegister.CauseOfEvent,
                CedantClaimEventCode = entity.ClaimRegister.CedantClaimEventCode,
                CedantClaimType = entity.ClaimRegister.CedantClaimType,
                CedantDateOfNotification = entity.ClaimRegister.CedantDateOfNotification,
                CedingBenefitRiskCode = entity.ClaimRegister.CedingBenefitRiskCode,
                CedingBenefitTypeCode = entity.ClaimRegister.CedingBenefitTypeCode,
                CedingClaimType = entity.ClaimRegister.CedingClaimType,
                CedingCompany = entity.CedingCompany,
                CedingEventCode = entity.ClaimRegister.CedingEventCode,
                CedingPlanCode = entity.ClaimRegister.CedingPlanCode,
                CurrencyRate = entity.ClaimRegister.CurrencyRate,
                CurrencyCode = entity.ClaimRegister.CurrencyCode,
                DateApproved = entity.ClaimRegister.DateApproved,
                DateOfEvent = entity.DateOfEvent,
                DateOfRegister = entity.ClaimRegister.DateOfRegister,
                DateOfReported = entity.ClaimRegister.DateOfReported,
                EntryNo = entity.SortIndex == 0 ? entity.EntryNo : entity.EntryNo + " - " + entity.SortIndex,
                ExGratia = entity.ClaimRegister.ExGratia,
                ForeignClaimRecoveryAmt = entity.ClaimRegister.ForeignClaimRecoveryAmt,
                FundsAccountingTypeCode = entity.FundsAccountingTypeCode,
                InsuredDateOfBirth = entity.ClaimRegister.InsuredDateOfBirth,
                InsuredGenderCode = entity.ClaimRegister.InsuredGenderCode,
                InsuredName = entity.ClaimRegister.InsuredName,
                InsuredTobaccoUse = entity.ClaimRegister.InsuredTobaccoUse,
                LastTransactionDate = entity.LastTransactionDate,
                LastTransactionQuarter = entity.LastTransactionQuarter,
                LateInterest = entity.ClaimRegister.LateInterest,
                Layer1SumRein = entity.ClaimRegister.Layer1SumRein,
                Mfrs17AnnualCohort = entity.Mfrs17AnnualCohort,
                Mfrs17ContractCode = entity.Mfrs17ContractCode,
                MlreBenefitCode = entity.MlreBenefitCode,
                MlreEventCode = entity.ClaimRegister.MlreEventCode,
                MlreInvoiceDate = entity.ClaimRegister.MlreInvoiceDate,
                MlreInvoiceNumber = entity.ClaimRegister.MlreInvoiceNumber,
                MlreRetainAmount = entity.ClaimRegister.MlreRetainAmount,
                MlreShare = entity.ClaimRegister.MlreShare,
                PendingProvisionDay = entity.ClaimRegister.PendingProvisionDay,
                PolicyDuration = entity.ClaimRegister.PolicyDuration,
                RecordType = entity.ClaimRegister.RecordType,
                ReinsBasisCode = entity.ReinsBasisCode,
                ReinsEffDatePol = entity.ReinsEffDatePol,
                RetroParty1 = entity.RetroParty1,
                RetroParty2 = entity.RetroParty2,
                RetroParty3 = entity.RetroParty3,
                RetroRecovery1 = entity.RetroRecovery1,
                RetroRecovery2 = entity.RetroRecovery2,
                RetroRecovery3 = entity.RetroRecovery3,
                RetroStatementDate1 = entity.ClaimRegister.RetroStatementDate1,
                RetroStatementDate2 = entity.ClaimRegister.RetroStatementDate2,
                RetroStatementDate3 = entity.ClaimRegister.RetroStatementDate3,
                RetroStatementId1 = entity.ClaimRegister.RetroStatementId1,
                RetroStatementId2 = entity.ClaimRegister.RetroStatementId2,
                RetroStatementId3 = entity.ClaimRegister.RetroStatementId3,
                RetroShare1 = entity.ClaimRegister.RetroShare1,
                RetroShare2 = entity.ClaimRegister.RetroShare2,
                RetroShare3 = entity.ClaimRegister.RetroShare3,
                RiskPeriodMonth = entity.RiskPeriodMonth,
                RiskPeriodYear = entity.RiskPeriodYear,
                RiskQuarter = entity.RiskQuarter,
                SaFactor = entity.ClaimRegister.SaFactor,
                SoaQuarter = entity.Quarter,
                SumIns = entity.ClaimRegister.SumIns,
                TempA1 = entity.ClaimRegister.TempA1,
                TempA2 = entity.ClaimRegister.TempA2,
                TempD1 = entity.ClaimRegister.TempD1,
                TempD2 = entity.ClaimRegister.TempD2,
                TempI1 = entity.ClaimRegister.TempI1,
                TempI2 = entity.ClaimRegister.TempI2,
                TempS1 = entity.ClaimRegister.TempS1,
                TempS2 = entity.ClaimRegister.TempS2,
                TransactionDateWop = entity.ClaimRegister.TransactionDateWop,
                MlreReferenceNo = entity.ClaimRegister.MlreReferenceNo,
                AddInfo = entity.ClaimRegister.AddInfo,
                Remark1 = entity.ClaimRegister.Remark1,
                Remark2 = entity.ClaimRegister.Remark2,
                IssueDatePol = entity.ClaimRegister.IssueDatePol,
                PolicyExpiryDate = entity.ClaimRegister.PolicyExpiryDate,
                ClaimAssessorId = entity.ClaimRegister.ClaimAssessorId,
                Comment = entity.ClaimRegister.Comment,
                SignOffById = entity.ClaimRegister.SignOffById,
                SignOffDate = entity.ClaimRegister.SignOffDate,
                CedingTreatyCode = entity.ClaimRegister.CedingTreatyCode,
                CampaignCode = entity.ClaimRegister.CampaignCode,
                DateOfIntimation = entity.ClaimRegister.DateOfIntimation,

                // Ex Gratia
                EventChronologyComment = entity.ClaimRegister.EventChronologyComment,
                ClaimAssessorRecommendation = entity.ClaimRegister.ClaimAssessorRecommendation,
                ClaimCommitteeComment1 = entity.ClaimRegister.ClaimCommitteeComment1,
                ClaimCommitteeComment2 = entity.ClaimRegister.ClaimCommitteeComment2,
                ClaimCommitteeUser1Id = entity.ClaimRegister.ClaimCommitteeUser1Id,
                ClaimCommitteeUser1Name = entity.ClaimRegister.ClaimCommitteeUser1Name,
                ClaimCommitteeUser2Id = entity.ClaimRegister.ClaimCommitteeUser2Id,
                ClaimCommitteeUser2Name = entity.ClaimRegister.ClaimCommitteeUser2Name,
                ClaimCommitteeDateCommented1 = entity.ClaimRegister.ClaimCommitteeDateCommented1,
                ClaimCommitteeDateCommented2 = entity.ClaimRegister.ClaimCommitteeDateCommented2,
                CeoClaimReasonId = entity.ClaimRegister.CeoClaimReasonId,
                CeoComment = entity.ClaimRegister.CeoComment,
                UpdatedOnBehalfById = entity.ClaimRegister.UpdatedOnBehalfById,
                UpdatedOnBehalfAt = entity.ClaimRegister.UpdatedOnBehalfAt,

                // Checklist
                Checklist = entity.ClaimRegister.Checklist,

                CreatedById = entity.ClaimRegister.CreatedById,
                UpdatedById = entity.ClaimRegister.UpdatedById,

                SortIndex = entity.SortIndex,
                ProvisionAt = entity.FinanceProvisioning.ProvisionAt ?? DateTime.Today
            };
        }

        public static Expression<Func<ClaimRegisterHistoryTransaction, ClaimRegisterHistoryBo>> ClaimRegisterHistoryExpression()
        {
            return entity => new ClaimRegisterHistoryBo
            {
                Id = entity.History.Id,
                CutOffId = entity.History.CutOffId,
                ClaimRegisterId = entity.History.ClaimRegisterId,
                ClaimDataBatchId = entity.History.ClaimDataBatchId,
                ClaimDataId = entity.History.ClaimDataId,
                ClaimDataConfigId = entity.History.ClaimDataConfigId,
                SoaDataBatchId = entity.History.SoaDataBatchId,
                RiDataWarehouseId = entity.History.RiDataWarehouseId,
                ReferralRiDataId = entity.History.ReferralRiDataId,
                ClaimStatus = entity.History.ClaimStatus,
                ClaimDecisionStatus = entity.History.ClaimDecisionStatus,
                ReferralClaimId = entity.History.ReferralClaimId,
                OriginalClaimRegisterId = entity.History.OriginalClaimRegisterId,
                ClaimReasonId = entity.History.ClaimReasonId,
                PicDaaId = entity.History.PicDaaId,
                PicClaimId = entity.History.PicClaimId,
                ProvisionStatus = entity.Transaction.FinanceProvisioning.ProvisionAt.HasValue ? ClaimRegisterBo.ProvisionStatusProvisioned : entity.History.ProvisionStatus,
                DrProvisionStatus = entity.History.DrProvisionStatus,
                TargetDateToIssueInvoice = entity.History.TargetDateToIssueInvoice,
                IsReferralCase = entity.History.IsReferralCase,
                OffsetStatus = entity.History.OffsetStatus,
                ClaimId = entity.Transaction.ClaimId,
                ClaimCode = entity.Transaction.ClaimCode,
                MappingStatus = entity.History.MappingStatus,
                ProcessingStatus = entity.History.ProcessingStatus,
                DuplicationCheckStatus = entity.History.DuplicationCheckStatus,
                PostComputationStatus = entity.History.PostComputationStatus,
                PostValidationStatus = entity.History.PostValidationStatus,
                Errors = entity.History.Errors,
                ProvisionErrors = entity.History.ProvisionErrors,
                RedFlagWarnings = entity.History.RedFlagWarnings,
                RequestUnderwriterReview = entity.History.RequestUnderwriterReview,
                UnderwriterFeedback = entity.History.UnderwriterFeedback,
                HasRedFlag = entity.History.HasRedFlag,

                PolicyNumber = entity.Transaction.PolicyNumber,
                PolicyTerm = entity.History.PolicyTerm,
                ClaimRecoveryAmt = entity.Transaction.ClaimRecoveryAmount,
                ClaimTransactionType = entity.History.ClaimTransactionType,
                TreatyCode = entity.Transaction.TreatyCode,
                TreatyType = entity.Transaction.TreatyType,
                AarPayable = entity.Transaction.SumReinsured,
                AnnualRiPrem = entity.History.AnnualRiPrem,
                CauseOfEvent = entity.History.CauseOfEvent,
                CedantClaimEventCode = entity.History.CedantClaimEventCode,
                CedantClaimType = entity.History.CedantClaimType,
                CedantDateOfNotification = entity.History.CedantDateOfNotification,
                CedingBenefitRiskCode = entity.History.CedingBenefitRiskCode,
                CedingBenefitTypeCode = entity.History.CedingBenefitTypeCode,
                CedingClaimType = entity.History.CedingClaimType,
                CedingCompany = entity.Transaction.CedingCompany,
                CedingEventCode = entity.History.CedingEventCode,
                CedingPlanCode = entity.History.CedingPlanCode,
                CurrencyRate = entity.History.CurrencyRate,
                CurrencyCode = entity.History.CurrencyCode,
                DateApproved = entity.History.DateApproved,
                DateOfEvent = entity.Transaction.DateOfEvent,
                DateOfRegister = entity.History.DateOfRegister,
                DateOfReported = entity.History.DateOfReported,
                EntryNo = entity.Transaction.SortIndex == 0 ? entity.Transaction.EntryNo : entity.Transaction.EntryNo + " - " + entity.Transaction.SortIndex,
                ExGratia = entity.History.ExGratia,
                ForeignClaimRecoveryAmt = entity.History.ForeignClaimRecoveryAmt,
                FundsAccountingTypeCode = entity.Transaction.FundsAccountingTypeCode,
                InsuredDateOfBirth = entity.History.InsuredDateOfBirth,
                InsuredGenderCode = entity.History.InsuredGenderCode,
                InsuredName = entity.History.InsuredName,
                InsuredTobaccoUse = entity.History.InsuredTobaccoUse,
                LastTransactionDate = entity.Transaction.LastTransactionDate,
                LastTransactionQuarter = entity.Transaction.LastTransactionQuarter,
                LateInterest = entity.History.LateInterest,
                Layer1SumRein = entity.History.Layer1SumRein,
                Mfrs17AnnualCohort = entity.Transaction.Mfrs17AnnualCohort,
                Mfrs17ContractCode = entity.Transaction.Mfrs17ContractCode,
                MlreBenefitCode = entity.Transaction.MlreBenefitCode,
                MlreEventCode = entity.History.MlreEventCode,
                MlreInvoiceDate = entity.History.MlreInvoiceDate,
                MlreInvoiceNumber = entity.History.MlreInvoiceNumber,
                MlreRetainAmount = entity.History.MlreRetainAmount,
                MlreShare = entity.History.MlreShare,
                PendingProvisionDay = entity.History.PendingProvisionDay,
                PolicyDuration = entity.History.PolicyDuration,
                RecordType = entity.History.RecordType,
                ReinsBasisCode = entity.Transaction.ReinsBasisCode,
                ReinsEffDatePol = entity.Transaction.ReinsEffDatePol,
                RetroParty1 = entity.Transaction.RetroParty1,
                RetroParty2 = entity.Transaction.RetroParty2,
                RetroParty3 = entity.Transaction.RetroParty3,
                RetroRecovery1 = entity.Transaction.RetroRecovery1,
                RetroRecovery2 = entity.Transaction.RetroRecovery2,
                RetroRecovery3 = entity.Transaction.RetroRecovery3,
                RetroStatementDate1 = entity.History.RetroStatementDate1,
                RetroStatementDate2 = entity.History.RetroStatementDate2,
                RetroStatementDate3 = entity.History.RetroStatementDate3,
                RetroStatementId1 = entity.History.RetroStatementId1,
                RetroStatementId2 = entity.History.RetroStatementId2,
                RetroStatementId3 = entity.History.RetroStatementId3,
                RetroShare1 = entity.History.RetroShare1,
                RetroShare2 = entity.History.RetroShare2,
                RetroShare3 = entity.History.RetroShare3,
                RiskPeriodMonth = entity.Transaction.RiskPeriodMonth,
                RiskPeriodYear = entity.Transaction.RiskPeriodYear,
                RiskQuarter = entity.Transaction.RiskQuarter,
                SaFactor = entity.History.SaFactor,
                SoaQuarter = entity.Transaction.Quarter,
                SumIns = entity.History.SumIns,
                TempA1 = entity.History.TempA1,
                TempA2 = entity.History.TempA2,
                TempD1 = entity.History.TempD1,
                TempD2 = entity.History.TempD2,
                TempI1 = entity.History.TempI1,
                TempI2 = entity.History.TempI2,
                TempS1 = entity.History.TempS1,
                TempS2 = entity.History.TempS2,
                TransactionDateWop = entity.History.TransactionDateWop,
                MlreReferenceNo = entity.History.MlreReferenceNo,
                AddInfo = entity.History.AddInfo,
                Remark1 = entity.History.Remark1,
                Remark2 = entity.History.Remark2,
                IssueDatePol = entity.History.IssueDatePol,
                PolicyExpiryDate = entity.History.PolicyExpiryDate,
                ClaimAssessorId = entity.History.ClaimAssessorId,
                Comment = entity.History.Comment,
                SignOffById = entity.History.SignOffById,
                SignOffDate = entity.History.SignOffDate,
                CedingTreatyCode = entity.History.CedingTreatyCode,
                CampaignCode = entity.History.CampaignCode,
                DateOfIntimation = entity.History.DateOfIntimation,

                // Ex Gratia
                EventChronologyComment = entity.History.EventChronologyComment,
                ClaimAssessorRecommendation = entity.History.ClaimAssessorRecommendation,
                ClaimCommitteeComment1 = entity.History.ClaimCommitteeComment1,
                ClaimCommitteeComment2 = entity.History.ClaimCommitteeComment2,
                ClaimCommitteeUser1Id = entity.History.ClaimCommitteeUser1Id,
                ClaimCommitteeUser1Name = entity.History.ClaimCommitteeUser1Name,
                ClaimCommitteeUser2Id = entity.History.ClaimCommitteeUser2Id,
                ClaimCommitteeUser2Name = entity.History.ClaimCommitteeUser2Name,
                ClaimCommitteeDateCommented1 = entity.History.ClaimCommitteeDateCommented1,
                ClaimCommitteeDateCommented2 = entity.History.ClaimCommitteeDateCommented2,
                CeoClaimReasonId = entity.History.CeoClaimReasonId,
                CeoComment = entity.History.CeoComment,
                UpdatedOnBehalfById = entity.History.UpdatedOnBehalfById,
                UpdatedOnBehalfAt = entity.History.UpdatedOnBehalfAt,

                // Checklist
                Checklist = entity.History.Checklist,

                CreatedById = entity.History.CreatedById,
                UpdatedById = entity.History.UpdatedById,

                FinanceProvisioningStatus = entity.Transaction.FinanceProvisioning != null ? entity.Transaction.FinanceProvisioning.Status : FinanceProvisioningBo.StatusPending,
                SortIndex = entity.Transaction.SortIndex,
                ProvisionAt = entity.Transaction.FinanceProvisioning.ProvisionAt ?? DateTime.Today,
                CutOffAt = entity.History.CutOff.CutOffDateTime
            };
        }

        public static FinanceProvisioningTransactionBo FormBo(FinanceProvisioningTransaction entity = null)
        {
            if (entity == null)
                return null;

            return new FinanceProvisioningTransactionBo
            {
                Id = entity.Id,
                ClaimRegisterId = entity.ClaimRegisterId,
                ClaimRegisterBo = ClaimRegisterService.Find(entity.ClaimRegisterId),
                FinanceProvisioningId = entity.FinanceProvisioningId,
                SortIndex = entity.SortIndex,
                IsLatestProvision = entity.IsLatestProvision,
                ClaimId = entity.ClaimId,
                PolicyNumber = entity.PolicyNumber,
                CedingCompany = entity.CedingCompany,
                EntryNo = entity.EntryNo,
                Quarter = entity.Quarter,
                SumReinsured = entity.SumReinsured,
                SumReinsuredStr = Util.DoubleToString(entity.SumReinsured),
                ClaimRecoveryAmount = entity.ClaimRecoveryAmount,
                ClaimRecoveryAmountStr = Util.DoubleToString(entity.ClaimRecoveryAmount),

                TreatyCode = entity.TreatyCode,
                TreatyType = entity.TreatyType,
                ClaimCode = entity.ClaimCode,
                LastTransactionDate = entity.LastTransactionDate,
                LastTransactionQuarter = entity.LastTransactionQuarter,
                DateOfEvent = entity.DateOfEvent,
                RiskQuarter = entity.RiskQuarter,
                RiskPeriodYear = entity.RiskPeriodYear,
                RiskPeriodMonth = entity.RiskPeriodMonth,
                FundsAccountingTypeCode = entity.FundsAccountingTypeCode,
                MlreBenefitCode = entity.MlreBenefitCode,
                RetroParty1 = entity.RetroParty1,
                RetroParty2 = entity.RetroParty2,
                RetroParty3 = entity.RetroParty3,
                RetroRecovery1 = entity.RetroRecovery1,
                RetroRecovery2 = entity.RetroRecovery2,
                RetroRecovery3 = entity.RetroRecovery3,
                ReinsEffDatePol = entity.ReinsEffDatePol,
                ReinsBasisCode = entity.ReinsBasisCode,
                Mfrs17ContractCode = entity.Mfrs17ContractCode,
                Mfrs17AnnualCohort = entity.Mfrs17AnnualCohort,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
                CreatedAt = entity.CreatedAt,
                CreatedAtStr = entity.CreatedAt.ToString(Util.GetDateFormat()),
            };
        }

        public static FinanceProvisioningTransaction FormEntity(FinanceProvisioningTransactionBo bo = null)
        {
            if (bo == null)
                return null;
            return new FinanceProvisioningTransaction
            {
                Id = bo.Id,
                ClaimRegisterId = bo.ClaimRegisterId,
                FinanceProvisioningId = bo.FinanceProvisioningId,
                SortIndex = bo.SortIndex,
                IsLatestProvision = bo.IsLatestProvision,
                ClaimId = bo.ClaimId,
                PolicyNumber = bo.PolicyNumber,
                CedingCompany = bo.CedingCompany,
                EntryNo = bo.EntryNo,
                Quarter = bo.Quarter,
                SumReinsured = bo.SumReinsured,
                ClaimRecoveryAmount = bo.ClaimRecoveryAmount,

                TreatyCode = bo.TreatyCode,
                TreatyType = bo.TreatyType,
                ClaimCode = bo.ClaimCode,
                LastTransactionDate = bo.LastTransactionDate,
                LastTransactionQuarter = bo.LastTransactionQuarter,
                DateOfEvent = bo.DateOfEvent,
                RiskQuarter = bo.RiskQuarter,
                RiskPeriodYear = bo.RiskPeriodYear,
                RiskPeriodMonth = bo.RiskPeriodMonth,
                FundsAccountingTypeCode = bo.FundsAccountingTypeCode,
                MlreBenefitCode = bo.MlreBenefitCode,
                RetroParty1 = bo.RetroParty1,
                RetroParty2 = bo.RetroParty2,
                RetroParty3 = bo.RetroParty3,
                RetroRecovery1 = bo.RetroRecovery1,
                RetroRecovery2 = bo.RetroRecovery2,
                RetroRecovery3 = bo.RetroRecovery3,
                ReinsEffDatePol = bo.ReinsEffDatePol,
                ReinsBasisCode = bo.ReinsBasisCode,
                Mfrs17ContractCode = bo.Mfrs17ContractCode,
                Mfrs17AnnualCohort = bo.Mfrs17AnnualCohort,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static IList<FinanceProvisioningTransactionBo> FormBos(IList<FinanceProvisioningTransaction> entities = null)
        {
            if (entities == null)
                return null;
            IList<FinanceProvisioningTransactionBo> bos = new List<FinanceProvisioningTransactionBo>() { };
            foreach (FinanceProvisioningTransaction entity in entities)
            {
                bos.Add(FormBo(entity));
            }

            return bos;
        }

        public static bool IsExists(int id)
        {
            return FinanceProvisioningTransaction.IsExists(id);
        }

        public static FinanceProvisioningTransactionBo Find(int? id)
        {
            if (!id.HasValue)
                return null;
            return FormBo(FinanceProvisioningTransaction.Find(id.Value));
        }

        public static FinanceProvisioningTransactionBo FindLatestByClaimRegisterId(int financeProvisioningId, int claimRegisterId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.FinanceProvisioningTransactions
                    .Where(q => q.FinanceProvisioningId == financeProvisioningId)
                    .Where(q => q.ClaimRegisterId == claimRegisterId)
                    .Where(q => q.IsLatestProvision == true);

                return FormBo(query.FirstOrDefault());
            }
        }

        public static FinanceProvisioningTransactionBo FindNotLatestByClaimRegisterId(int financeProvisioningId, int claimRegisterId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.FinanceProvisioningTransactions
                    .Where(q => q.FinanceProvisioningId == financeProvisioningId)
                    .Where(q => q.ClaimRegisterId == claimRegisterId)
                    .Where(q => q.IsLatestProvision == false);

                return FormBo(query.FirstOrDefault());
            }
        }

        public static IList<FinanceProvisioningTransactionBo> GetByClaimRegisterId(int claimRegisterId, bool latestProvisionOnly = true)
        {
            using (var db = new AppDbContext())
            {
                var query = db.FinanceProvisioningTransactions.Where(q => q.ClaimRegisterId == claimRegisterId);

                if (latestProvisionOnly)
                    query = query.Where(q => q.IsLatestProvision == true);
                else
                    query = query.Where(q => q.FinanceProvisioning.Status != FinanceProvisioningBo.StatusPending);

                return FormBos(query.ToList());
            }
        }

        public static IList<FinanceProvisioningTransactionBo> GetByClaimRegisterIdByFinanceProvisioningId(int claimRegisterId, int financeProvisioningId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.FinanceProvisioningTransactions
                    .Where(q => q.ClaimRegisterId == claimRegisterId)
                    .Where(q => q.FinanceProvisioningId == financeProvisioningId)
                    .ToList());
            }
        }

        public static List<int> GetClaimRegisterIdByFinanceProvisioningId(int financeProvisioningId)
        {
            using (var db = new AppDbContext())
            {
                return db.FinanceProvisioningTransactions
                    .Where(q => q.FinanceProvisioningId == financeProvisioningId)
                    .GroupBy(q => q.ClaimRegisterId)
                    .Select(q => q.FirstOrDefault())
                    .Select(q => q.ClaimRegisterId)
                    .ToList();
            }
        }

        public static Result Save(ref FinanceProvisioningTransactionBo bo)
        {
            if (!FinanceProvisioningTransaction.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref FinanceProvisioningTransactionBo bo, ref TrailObject trail)
        {
            if (!FinanceProvisioningTransaction.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref FinanceProvisioningTransactionBo bo)
        {
            FinanceProvisioningTransaction entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref FinanceProvisioningTransactionBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref FinanceProvisioningTransactionBo bo)
        {
            Result result = Result();

            FinanceProvisioningTransaction entity = FinanceProvisioningTransaction.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                bo.UpdatedAt = DateTime.Now;
                entity = FormEntity(bo);
                result.DataTrail = entity.Update();
            }

            return result;
        }

        public static Result Update(ref FinanceProvisioningTransactionBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static bool ToProvision(ClaimRegisterBo bo)
        {
            var dbBo = ClaimRegisterService.Find(bo.Id);

            return bo.ClaimRecoveryAmt != dbBo.ClaimRecoveryAmt ||
                bo.ClaimCode != dbBo.ClaimCode ||
                bo.TreatyCode != dbBo.TreatyCode ||
                bo.TreatyType != dbBo.TreatyType ||
                bo.DateOfEvent != dbBo.DateOfEvent ||
                bo.FundsAccountingTypeCode != dbBo.FundsAccountingTypeCode ||
                bo.MlreBenefitCode != dbBo.MlreBenefitCode ||
                bo.PolicyNumber != dbBo.PolicyNumber ||
                bo.RetroParty1 != dbBo.RetroParty1 ||
                bo.RetroParty2 != dbBo.RetroParty2 ||
                bo.RetroParty3 != dbBo.RetroParty3 ||
                bo.RetroRecovery1 != dbBo.RetroRecovery1 ||
                bo.RetroRecovery2 != dbBo.RetroRecovery2 ||
                bo.RetroRecovery3 != dbBo.RetroRecovery3 ||
                bo.ReinsEffDatePol != dbBo.ReinsEffDatePol ||
                bo.ReinsBasisCode != dbBo.ReinsBasisCode ||
                bo.Mfrs17ContractCode != dbBo.Mfrs17ContractCode ||
                bo.Mfrs17AnnualCohort != dbBo.Mfrs17AnnualCohort;
        }
    }

    public class ClaimRegisterHistoryTransaction
    {
        public FinanceProvisioningTransaction Transaction { get; set; }

        public ClaimRegisterHistory History { get; set; }
    }
}
