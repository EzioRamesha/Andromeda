using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Services.Claims;
using Services.Identity;
using Services.RiDatas;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ClaimRegisterHistoryService
    {
        public static ClaimRegisterHistoryBo FormBo(ClaimRegisterHistory entity = null, bool foreign = true, bool formatOutput = false)
        {
            if (entity == null)
                return null;
            ClaimRegisterHistoryBo claimRegisterHistoryBo = new ClaimRegisterHistoryBo
            {
                Id = entity.Id,
                CutOffId = entity.CutOffId,
                ClaimDataBatchId = entity.ClaimDataBatchId,
                ClaimDataId = entity.ClaimDataId,
                ClaimDataConfigId = entity.ClaimDataConfigId,
                SoaDataBatchId = entity.SoaDataBatchId,
                RiDataWarehouseId = entity.RiDataWarehouseId,
                ReferralRiDataId = entity.ReferralRiDataId,
                ClaimStatus = entity.ClaimStatus,
                ClaimDecisionStatus = entity.ClaimDecisionStatus,
                ClaimDecisionStatusName = ClaimRegisterBo.GetClaimDecisionStatusName(entity.ClaimDecisionStatus),
                ReferralClaimId = entity.ReferralClaimId,
                OriginalClaimRegisterId = entity.OriginalClaimRegisterId,
                ClaimRegisterId = entity.ClaimRegisterId,
                ClaimReasonId = entity.ClaimReasonId,
                PicDaaId = entity.PicDaaId,
                PicClaimId = entity.PicClaimId,
                ProvisionStatus = entity.ProvisionStatus,
                DrProvisionStatus = entity.DrProvisionStatus,
                TargetDateToIssueInvoice = entity.TargetDateToIssueInvoice,
                IsReferralCase = entity.IsReferralCase,
                OffsetStatus = entity.OffsetStatus,
                ClaimId = entity.ClaimId,
                ClaimCode = entity.ClaimCode,
                MappingStatus = entity.MappingStatus,
                ProcessingStatus = entity.ProcessingStatus,
                DuplicationCheckStatus = entity.DuplicationCheckStatus,
                PostComputationStatus = entity.PostComputationStatus,
                PostValidationStatus = entity.PostValidationStatus,
                Errors = entity.Errors,
                ProvisionErrors = entity.ProvisionErrors,
                RedFlagWarnings = entity.RedFlagWarnings,
                RequestUnderwriterReview = entity.RequestUnderwriterReview,
                UnderwriterFeedback = entity.UnderwriterFeedback,
                HasRedFlag = entity.HasRedFlag,

                PolicyNumber = entity.PolicyNumber,
                PolicyTerm = entity.PolicyTerm,
                ClaimRecoveryAmt = entity.ClaimRecoveryAmt,
                ClaimTransactionType = entity.ClaimTransactionType,
                TreatyCode = entity.TreatyCode,
                TreatyType = entity.TreatyType,
                AarPayable = entity.AarPayable,
                AnnualRiPrem = entity.AnnualRiPrem,
                CauseOfEvent = entity.CauseOfEvent,
                CedantClaimEventCode = entity.CedantClaimEventCode,
                CedantClaimType = entity.CedantClaimType,
                CedantDateOfNotification = entity.CedantDateOfNotification,
                CedingBenefitRiskCode = entity.CedingBenefitRiskCode,
                CedingBenefitTypeCode = entity.CedingBenefitTypeCode,
                CedingClaimType = entity.CedingClaimType,
                CedingCompany = entity.CedingCompany,
                CedingEventCode = entity.CedingEventCode,
                CedingPlanCode = entity.CedingPlanCode,
                CurrencyRate = entity.CurrencyRate,
                CurrencyCode = entity.CurrencyCode,
                DateApproved = entity.DateApproved,
                DateOfEvent = entity.DateOfEvent,
                DateOfRegister = entity.DateOfRegister,
                DateOfReported = entity.DateOfReported,
                EntryNo = entity.EntryNo,
                ExGratia = entity.ExGratia,
                ForeignClaimRecoveryAmt = entity.ForeignClaimRecoveryAmt,
                FundsAccountingTypeCode = entity.FundsAccountingTypeCode,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                InsuredGenderCode = entity.InsuredGenderCode,
                InsuredName = entity.InsuredName,
                InsuredTobaccoUse = entity.InsuredTobaccoUse,
                LastTransactionDate = entity.LastTransactionDate,
                LastTransactionQuarter = entity.LastTransactionQuarter,
                LateInterest = entity.LateInterest,
                Layer1SumRein = entity.Layer1SumRein,
                Mfrs17AnnualCohort = entity.Mfrs17AnnualCohort,
                Mfrs17ContractCode = entity.Mfrs17ContractCode,
                MlreBenefitCode = entity.MlreBenefitCode,
                MlreEventCode = entity.MlreEventCode,
                MlreInvoiceDate = entity.MlreInvoiceDate,
                MlreInvoiceNumber = entity.MlreInvoiceNumber,
                MlreRetainAmount = entity.MlreRetainAmount,
                MlreShare = entity.MlreShare,
                PendingProvisionDay = entity.PendingProvisionDay,
                PolicyDuration = entity.PolicyDuration,
                RecordType = entity.RecordType,
                ReinsBasisCode = entity.ReinsBasisCode,
                ReinsEffDatePol = entity.ReinsEffDatePol,
                RetroParty1 = entity.RetroParty1,
                RetroParty2 = entity.RetroParty2,
                RetroParty3 = entity.RetroParty3,
                RetroRecovery1 = entity.RetroRecovery1,
                RetroRecovery2 = entity.RetroRecovery2,
                RetroRecovery3 = entity.RetroRecovery3,
                RetroStatementDate1 = entity.RetroStatementDate1,
                RetroStatementDate2 = entity.RetroStatementDate2,
                RetroStatementDate3 = entity.RetroStatementDate3,
                RetroShare1 = entity.RetroShare1,
                RetroShare2 = entity.RetroShare2,
                RetroShare3 = entity.RetroShare3,
                RetroStatementId1 = entity.RetroStatementId1,
                RetroStatementId2 = entity.RetroStatementId2,
                RetroStatementId3 = entity.RetroStatementId3,
                RiskPeriodMonth = entity.RiskPeriodMonth,
                RiskPeriodYear = entity.RiskPeriodYear,
                RiskQuarter = entity.RiskQuarter,
                SaFactor = entity.SaFactor,
                SoaQuarter = entity.SoaQuarter,
                SumIns = entity.SumIns,
                TempA1 = entity.TempA1,
                TempA2 = entity.TempA2,
                TempD1 = entity.TempD1,
                TempD2 = entity.TempD2,
                TempI1 = entity.TempI1,
                TempI2 = entity.TempI2,
                TempS1 = entity.TempS1,
                TempS2 = entity.TempS2,
                TransactionDateWop = entity.TransactionDateWop,
                MlreReferenceNo = entity.MlreReferenceNo,
                AddInfo = entity.AddInfo,
                Remark1 = entity.Remark1,
                Remark2 = entity.Remark2,
                IssueDatePol = entity.IssueDatePol,
                PolicyExpiryDate = entity.PolicyExpiryDate,
                ClaimAssessorId = entity.ClaimAssessorId,
                Comment = entity.Comment,
                SignOffById = entity.SignOffById,
                SignOffDate = entity.SignOffDate,
                CedingTreatyCode = entity.CedingTreatyCode,
                CampaignCode = entity.CampaignCode,
                DateOfIntimation = entity.DateOfIntimation,

                // Ex Gratia
                EventChronologyComment = entity.EventChronologyComment,
                ClaimAssessorRecommendation = entity.ClaimAssessorRecommendation,
                ClaimCommitteeComment1 = entity.ClaimCommitteeComment1,
                ClaimCommitteeComment2 = entity.ClaimCommitteeComment2,
                ClaimCommitteeUser1Id = entity.ClaimCommitteeUser1Id,
                ClaimCommitteeUser1Name = entity.ClaimCommitteeUser1Name,
                ClaimCommitteeUser2Id = entity.ClaimCommitteeUser2Id,
                ClaimCommitteeUser2Name = entity.ClaimCommitteeUser2Name,
                ClaimCommitteeDateCommented1 = entity.ClaimCommitteeDateCommented1,
                ClaimCommitteeDateCommented2 = entity.ClaimCommitteeDateCommented2,
                CeoClaimReasonId = entity.CeoClaimReasonId,
                CeoComment = entity.CeoComment,
                UpdatedOnBehalfById = entity.UpdatedOnBehalfById,
                UpdatedOnBehalfAt = entity.UpdatedOnBehalfAt,

                // Checklist
                Checklist = entity.Checklist,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };

            if (foreign)
            {
                if (entity.ClaimDataBatchId.HasValue)
                    claimRegisterHistoryBo.ClaimDataBatchBo = ClaimDataBatchService.Find(entity.ClaimDataBatchId.Value);
                if (entity.ClaimDataId.HasValue)
                    claimRegisterHistoryBo.ClaimDataBo = ClaimDataService.Find(entity.ClaimDataId.Value);
                claimRegisterHistoryBo.RiDataWarehouseBo = RiDataWarehouseService.Find(entity.RiDataWarehouseId, true);
                claimRegisterHistoryBo.OriginalClaimRegisterBo = ClaimRegisterService.Find(entity.OriginalClaimRegisterId);
                claimRegisterHistoryBo.ClaimReasonBo = ClaimReasonService.Find(entity.ClaimReasonId);
                claimRegisterHistoryBo.PicDaaBo = UserService.Find(entity.PicDaaId);
                claimRegisterHistoryBo.PicClaimBo = UserService.Find(entity.PicClaimId);
                claimRegisterHistoryBo.ClaimAssessorBo = UserService.Find(entity.ClaimAssessorId);
                claimRegisterHistoryBo.SignOffByBo = UserService.Find(entity.SignOffById);

                claimRegisterHistoryBo.TreatyCodeBo = TreatyCodeService.FindByCode(entity.TreatyCode);
                claimRegisterHistoryBo.ClaimCodeBo = ClaimCodeService.FindByCode(entity.ClaimCode);
                claimRegisterHistoryBo.TreatyTypePickListDetailBo = PickListDetailService.FindByPickListIdCode(PickListBo.TreatyType, entity.TreatyType);
                claimRegisterHistoryBo.ClaimCommitteeUser1Bo = UserService.Find(entity.ClaimCommitteeUser1Id);
                claimRegisterHistoryBo.ClaimCommitteeUser2Bo = UserService.Find(entity.ClaimCommitteeUser2Id);
                claimRegisterHistoryBo.CeoClaimReasonBo = ClaimReasonService.Find(entity.CeoClaimReasonId);
                claimRegisterHistoryBo.UpdatedOnBehalfByBo = UserService.Find(entity.UpdatedOnBehalfById);
            }

            if (formatOutput)
            {
                // Double
                claimRegisterHistoryBo.ClaimRecoveryAmtStr = Util.DoubleToString(entity.ClaimRecoveryAmt);
                claimRegisterHistoryBo.AarPayableStr = Util.DoubleToString(entity.AarPayable);
                claimRegisterHistoryBo.AnnualRiPremStr = Util.DoubleToString(entity.AnnualRiPrem);
                claimRegisterHistoryBo.CurrencyRateStr = Util.DoubleToString(entity.CurrencyRate);
                claimRegisterHistoryBo.ExGratiaStr = Util.DoubleToString(entity.ExGratia);
                claimRegisterHistoryBo.ForeignClaimRecoveryAmtStr = Util.DoubleToString(entity.ForeignClaimRecoveryAmt);
                claimRegisterHistoryBo.LateInterestStr = Util.DoubleToString(entity.LateInterest);
                claimRegisterHistoryBo.Layer1SumReinStr = Util.DoubleToString(entity.Layer1SumRein);
                claimRegisterHistoryBo.MlreRetainAmountStr = Util.DoubleToString(entity.MlreRetainAmount);
                claimRegisterHistoryBo.MlreShareStr = Util.DoubleToString(entity.MlreShare);
                claimRegisterHistoryBo.RetroRecovery1Str = Util.DoubleToString(entity.RetroRecovery1);
                claimRegisterHistoryBo.RetroRecovery2Str = Util.DoubleToString(entity.RetroRecovery2);
                claimRegisterHistoryBo.RetroRecovery3Str = Util.DoubleToString(entity.RetroRecovery3);
                claimRegisterHistoryBo.RetroShare1Str = Util.DoubleToString(entity.RetroShare1);
                claimRegisterHistoryBo.RetroShare2Str = Util.DoubleToString(entity.RetroShare2);
                claimRegisterHistoryBo.RetroShare3Str = Util.DoubleToString(entity.RetroShare3);
                claimRegisterHistoryBo.SaFactorStr = Util.DoubleToString(entity.SaFactor);
                claimRegisterHistoryBo.SumInsStr = Util.DoubleToString(entity.SumIns);
                claimRegisterHistoryBo.TempA1Str = Util.DoubleToString(entity.TempA1);
                claimRegisterHistoryBo.TempA2Str = Util.DoubleToString(entity.TempA2);

                // Date
                string dateFormat = Util.GetDateFormat();
                claimRegisterHistoryBo.TargetDateToIssueInvoiceStr = entity.TargetDateToIssueInvoice?.ToString(dateFormat);
                claimRegisterHistoryBo.CedantDateOfNotificationStr = entity.CedantDateOfNotification?.ToString(dateFormat);
                claimRegisterHistoryBo.DateApprovedStr = entity.DateApproved?.ToString(dateFormat);
                claimRegisterHistoryBo.DateOfEventStr = entity.DateOfEvent?.ToString(dateFormat);
                claimRegisterHistoryBo.DateOfRegisterStr = entity.DateOfRegister?.ToString(dateFormat);
                claimRegisterHistoryBo.DateOfReportedStr = entity.DateOfReported?.ToString(dateFormat);
                claimRegisterHistoryBo.InsuredDateOfBirthStr = entity.InsuredDateOfBirth?.ToString(dateFormat);
                claimRegisterHistoryBo.LastTransactionDateStr = entity.LastTransactionDate?.ToString(dateFormat);
                claimRegisterHistoryBo.MlreInvoiceDateStr = entity.MlreInvoiceDate?.ToString(dateFormat);
                claimRegisterHistoryBo.ReinsEffDatePolStr = entity.ReinsEffDatePol?.ToString(dateFormat);
                claimRegisterHistoryBo.RetroStatementDate1Str = entity.RetroStatementDate1?.ToString(dateFormat);
                claimRegisterHistoryBo.RetroStatementDate2Str = entity.RetroStatementDate2?.ToString(dateFormat);
                claimRegisterHistoryBo.RetroStatementDate3Str = entity.RetroStatementDate3?.ToString(dateFormat);
                claimRegisterHistoryBo.TempD1Str = entity.TempD1?.ToString(dateFormat);
                claimRegisterHistoryBo.TempD2Str = entity.TempD2?.ToString(dateFormat);
                claimRegisterHistoryBo.TransactionDateWopStr = entity.TransactionDateWop?.ToString(dateFormat);
                claimRegisterHistoryBo.IssueDatePolStr = entity.IssueDatePol?.ToString(dateFormat);
                claimRegisterHistoryBo.PolicyExpiryDateStr = entity.PolicyExpiryDate?.ToString(dateFormat);
                claimRegisterHistoryBo.SignOffDateStr = entity.SignOffDate?.ToString(dateFormat);
                claimRegisterHistoryBo.ClaimCommitteeDateCommented1Str = entity.ClaimCommitteeDateCommented1?.ToString(dateFormat);
                claimRegisterHistoryBo.ClaimCommitteeDateCommented2Str = entity.ClaimCommitteeDateCommented2?.ToString(dateFormat);
                claimRegisterHistoryBo.UpdatedOnBehalfAtStr = entity.UpdatedOnBehalfAt?.ToString(dateFormat);
                claimRegisterHistoryBo.DateOfIntimationStr = entity.DateOfIntimation?.ToString(dateFormat);

                // Constants
                claimRegisterHistoryBo.PostComputationStatusStr = ClaimRegisterBo.GetPostComputationStatusName(entity.PostComputationStatus);
                claimRegisterHistoryBo.PostValidationStatusStr = ClaimRegisterBo.GetPostValidationStatusName(entity.PostValidationStatus);
                claimRegisterHistoryBo.StatusName = ClaimRegisterBo.GetStatusName(entity.ClaimStatus);
            }

            return claimRegisterHistoryBo;
        }

        public static ClaimRegisterHistoryBo Find(int id)
        {
            return FormBo(ClaimRegisterHistory.Find(id));
        }

        public static ClaimRegisterHistoryBo Find(int cutOffId, int claimRegisterId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.ClaimRegisterHistories.Where(q => q.CutOffId == cutOffId).Where(q => q.ClaimRegisterId == claimRegisterId).FirstOrDefault());
            }
        }

        public static Expression<Func<ClaimRegisterHistory, ClaimRegisterHistoryBo>> Expression()
        {
            return entity => new ClaimRegisterHistoryBo
            {
                Id = entity.Id,
                CutOffId = entity.CutOffId,
                ClaimRegisterId = entity.ClaimRegisterId,
                ClaimDataBatchId = entity.ClaimDataBatchId,
                ClaimDataId = entity.ClaimDataId,
                ClaimDataConfigId = entity.ClaimDataConfigId,
                SoaDataBatchId = entity.SoaDataBatchId,
                RiDataWarehouseId = entity.RiDataWarehouseId,
                ReferralRiDataId = entity.ReferralRiDataId,
                ClaimStatus = entity.ClaimStatus,
                ClaimDecisionStatus = entity.ClaimDecisionStatus,
                ReferralClaimId = entity.ReferralClaimId,
                OriginalClaimRegisterId = entity.OriginalClaimRegisterId,
                ClaimReasonId = entity.ClaimReasonId,
                PicDaaId = entity.PicDaaId,
                PicClaimId = entity.PicClaimId,
                ProvisionStatus = entity.ProvisionStatus,
                DrProvisionStatus = entity.DrProvisionStatus,
                TargetDateToIssueInvoice = entity.TargetDateToIssueInvoice,
                IsReferralCase = entity.IsReferralCase,
                OffsetStatus = entity.OffsetStatus,
                ClaimId = entity.ClaimId,
                ClaimCode = entity.ClaimCode,
                MappingStatus = entity.MappingStatus,
                ProcessingStatus = entity.ProcessingStatus,
                DuplicationCheckStatus = entity.DuplicationCheckStatus,
                PostComputationStatus = entity.PostComputationStatus,
                PostValidationStatus = entity.PostValidationStatus,
                Errors = entity.Errors,
                ProvisionErrors = entity.ProvisionErrors,
                RedFlagWarnings = entity.RedFlagWarnings,
                RequestUnderwriterReview = entity.RequestUnderwriterReview,
                UnderwriterFeedback = entity.UnderwriterFeedback,
                HasRedFlag = entity.HasRedFlag,

                PolicyNumber = entity.PolicyNumber,
                PolicyTerm = entity.PolicyTerm,
                ClaimRecoveryAmt = entity.ClaimRecoveryAmt,
                ClaimTransactionType = entity.ClaimTransactionType,
                TreatyCode = entity.TreatyCode,
                TreatyType = entity.TreatyType,
                AarPayable = entity.AarPayable,
                AnnualRiPrem = entity.AnnualRiPrem,
                CauseOfEvent = entity.CauseOfEvent,
                CedantClaimEventCode = entity.CedantClaimEventCode,
                CedantClaimType = entity.CedantClaimType,
                CedantDateOfNotification = entity.CedantDateOfNotification,
                CedingBenefitRiskCode = entity.CedingBenefitRiskCode,
                CedingBenefitTypeCode = entity.CedingBenefitTypeCode,
                CedingClaimType = entity.CedingClaimType,
                CedingCompany = entity.CedingCompany,
                CedingEventCode = entity.CedingEventCode,
                CedingPlanCode = entity.CedingPlanCode,
                CurrencyRate = entity.CurrencyRate,
                CurrencyCode = entity.CurrencyCode,
                DateApproved = entity.DateApproved,
                DateOfEvent = entity.DateOfEvent,
                DateOfRegister = entity.DateOfRegister,
                DateOfReported = entity.DateOfReported,
                EntryNo = entity.EntryNo,
                ExGratia = entity.ExGratia,
                ForeignClaimRecoveryAmt = entity.ForeignClaimRecoveryAmt,
                FundsAccountingTypeCode = entity.FundsAccountingTypeCode,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                InsuredGenderCode = entity.InsuredGenderCode,
                InsuredName = entity.InsuredName,
                InsuredTobaccoUse = entity.InsuredTobaccoUse,
                LastTransactionDate = entity.LastTransactionDate,
                LastTransactionQuarter = entity.LastTransactionQuarter,
                LateInterest = entity.LateInterest,
                Layer1SumRein = entity.Layer1SumRein,
                Mfrs17AnnualCohort = entity.Mfrs17AnnualCohort,
                Mfrs17ContractCode = entity.Mfrs17ContractCode,
                MlreBenefitCode = entity.MlreBenefitCode,
                MlreEventCode = entity.MlreEventCode,
                MlreInvoiceDate = entity.MlreInvoiceDate,
                MlreInvoiceNumber = entity.MlreInvoiceNumber,
                MlreRetainAmount = entity.MlreRetainAmount,
                MlreShare = entity.MlreShare,
                PendingProvisionDay = entity.PendingProvisionDay,
                PolicyDuration = entity.PolicyDuration,
                RecordType = entity.RecordType,
                ReinsBasisCode = entity.ReinsBasisCode,
                ReinsEffDatePol = entity.ReinsEffDatePol,
                RetroParty1 = entity.RetroParty1,
                RetroParty2 = entity.RetroParty2,
                RetroParty3 = entity.RetroParty3,
                RetroRecovery1 = entity.RetroRecovery1,
                RetroRecovery2 = entity.RetroRecovery2,
                RetroRecovery3 = entity.RetroRecovery3,
                RetroStatementDate1 = entity.RetroStatementDate1,
                RetroStatementDate2 = entity.RetroStatementDate2,
                RetroStatementDate3 = entity.RetroStatementDate3,
                RetroStatementId1 = entity.RetroStatementId1,
                RetroStatementId2 = entity.RetroStatementId2,
                RetroStatementId3 = entity.RetroStatementId3,
                RetroShare1 = entity.RetroShare1,
                RetroShare2 = entity.RetroShare2,
                RetroShare3 = entity.RetroShare3,
                RiskPeriodMonth = entity.RiskPeriodMonth,
                RiskPeriodYear = entity.RiskPeriodYear,
                RiskQuarter = entity.RiskQuarter,
                SaFactor = entity.SaFactor,
                SoaQuarter = entity.SoaQuarter,
                SumIns = entity.SumIns,
                TempA1 = entity.TempA1,
                TempA2 = entity.TempA2,
                TempD1 = entity.TempD1,
                TempD2 = entity.TempD2,
                TempI1 = entity.TempI1,
                TempI2 = entity.TempI2,
                TempS1 = entity.TempS1,
                TempS2 = entity.TempS2,
                TransactionDateWop = entity.TransactionDateWop,
                MlreReferenceNo = entity.MlreReferenceNo,
                AddInfo = entity.AddInfo,
                Remark1 = entity.Remark1,
                Remark2 = entity.Remark2,
                IssueDatePol = entity.IssueDatePol,
                PolicyExpiryDate = entity.PolicyExpiryDate,
                ClaimAssessorId = entity.ClaimAssessorId,
                Comment = entity.Comment,
                SignOffById = entity.SignOffById,
                SignOffDate = entity.SignOffDate,
                CedingTreatyCode = entity.CedingTreatyCode,
                CampaignCode = entity.CampaignCode,
                DateOfIntimation = entity.DateOfIntimation,

                // Ex Gratia
                EventChronologyComment = entity.EventChronologyComment,
                ClaimAssessorRecommendation = entity.ClaimAssessorRecommendation,
                ClaimCommitteeComment1 = entity.ClaimCommitteeComment1,
                ClaimCommitteeComment2 = entity.ClaimCommitteeComment2,
                ClaimCommitteeUser1Id = entity.ClaimCommitteeUser1Id,
                ClaimCommitteeUser1Name = entity.ClaimCommitteeUser1Name,
                ClaimCommitteeUser2Id = entity.ClaimCommitteeUser2Id,
                ClaimCommitteeUser2Name = entity.ClaimCommitteeUser2Name,
                ClaimCommitteeDateCommented1 = entity.ClaimCommitteeDateCommented1,
                ClaimCommitteeDateCommented2 = entity.ClaimCommitteeDateCommented2,
                CeoClaimReasonId = entity.CeoClaimReasonId,
                CeoComment = entity.CeoComment,
                UpdatedOnBehalfById = entity.UpdatedOnBehalfById,
                UpdatedOnBehalfAt = entity.UpdatedOnBehalfAt,

                // Checklist
                Checklist = entity.Checklist,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                SortIndex = 0
            };
        }
    }
}
