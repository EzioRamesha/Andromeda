using BusinessObject;
using DataAccess.Entities;
using DataAccess.Entities.Claims;
using DataAccess.EntityFramework;
using Services.Claims;
using Services.Identity;
using Services.RiDatas;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ClaimRegisterService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ClaimRegister)),
                Controller = ModuleBo.ModuleController.ClaimRegister.ToString(),
            };
        }

        public static Expression<Func<ClaimRegister, ClaimRegisterBo>> Expression()
        {
            return entity => new ClaimRegisterBo
            {
                Id = entity.Id,
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

        public static ClaimRegisterBo FormBo(ClaimRegister entity = null, bool foreign = true, bool formatOutput = false, int? precision = 2)
        {
            if (entity == null)
                return null;
            ClaimRegisterBo claimRegisterBo = new ClaimRegisterBo
            {
                Id = entity.Id,
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
                    claimRegisterBo.ClaimDataBatchBo = ClaimDataBatchService.Find(entity.ClaimDataBatchId.Value);
                if (entity.ClaimDataId.HasValue)
                    claimRegisterBo.ClaimDataBo = ClaimDataService.Find(entity.ClaimDataId.Value);
                claimRegisterBo.ClaimDataConfigBo = ClaimDataConfigService.Find(entity.ClaimDataConfigId);
                claimRegisterBo.RiDataWarehouseBo = RiDataWarehouseService.Find(entity.RiDataWarehouseId, entity.ReferralRiDataId);
                claimRegisterBo.ReferralClaimBo = ReferralClaimService.Find(entity.ReferralClaimId);
                claimRegisterBo.OriginalClaimRegisterBo = Find(entity.OriginalClaimRegisterId);
                claimRegisterBo.ClaimReasonBo = ClaimReasonService.Find(entity.ClaimReasonId);
                claimRegisterBo.PicDaaBo = UserService.Find(entity.PicDaaId);
                claimRegisterBo.PicClaimBo = UserService.Find(entity.PicClaimId);
                claimRegisterBo.ClaimAssessorBo = UserService.Find(entity.ClaimAssessorId);
                claimRegisterBo.SignOffByBo = UserService.Find(entity.SignOffById);

                claimRegisterBo.TreatyCodeBo = TreatyCodeService.FindByCode(entity.TreatyCode);
                if (claimRegisterBo.TreatyCodeBo != null)
                {
                    var treatyBo = TreatyService.Find(claimRegisterBo.TreatyCodeBo.TreatyId);
                    if (treatyBo != null && treatyBo.BusinessOriginPickListDetailId.HasValue)
                    {
                        claimRegisterBo.BusinessOrigin = PickListDetailService.Find(treatyBo.BusinessOriginPickListDetailId)?.ToString();
                    }
                }
                claimRegisterBo.ClaimCodeBo = ClaimCodeService.FindByCode(entity.ClaimCode);
                claimRegisterBo.TreatyTypePickListDetailBo = PickListDetailService.FindByPickListIdCode(PickListBo.TreatyType, entity.TreatyType);
                claimRegisterBo.ClaimCommitteeUser1Bo = UserService.Find(entity.ClaimCommitteeUser1Id);
                claimRegisterBo.ClaimCommitteeUser2Bo = UserService.Find(entity.ClaimCommitteeUser2Id);
                claimRegisterBo.CeoClaimReasonBo = ClaimReasonService.Find(entity.CeoClaimReasonId);
                claimRegisterBo.UpdatedOnBehalfByBo = UserService.Find(entity.UpdatedOnBehalfById);
            }

            if (formatOutput)
            {
                // Double
                claimRegisterBo.ClaimRecoveryAmtStr = Util.DoubleToString(entity.ClaimRecoveryAmt, 2);
                claimRegisterBo.AarPayableStr = Util.DoubleToString(entity.AarPayable, precision);
                claimRegisterBo.AnnualRiPremStr = Util.DoubleToString(entity.AnnualRiPrem, precision);
                claimRegisterBo.CurrencyRateStr = Util.DoubleToString(entity.CurrencyRate);
                claimRegisterBo.ExGratiaStr = Util.DoubleToString(entity.ExGratia, precision);
                claimRegisterBo.ForeignClaimRecoveryAmtStr = Util.DoubleToString(entity.ForeignClaimRecoveryAmt, precision);
                claimRegisterBo.LateInterestStr = Util.DoubleToString(entity.LateInterest, precision);
                claimRegisterBo.Layer1SumReinStr = Util.DoubleToString(entity.Layer1SumRein, precision);
                claimRegisterBo.MlreRetainAmountStr = Util.DoubleToString(entity.MlreRetainAmount, precision);
                claimRegisterBo.MlreShareStr = Util.DoubleToString(entity.MlreShare, precision);
                claimRegisterBo.RetroRecovery1Str = Util.DoubleToString(entity.RetroRecovery1, 2);
                claimRegisterBo.RetroRecovery2Str = Util.DoubleToString(entity.RetroRecovery2, 2);
                claimRegisterBo.RetroRecovery3Str = Util.DoubleToString(entity.RetroRecovery3, 2);
                claimRegisterBo.RetroShare1Str = Util.DoubleToString(entity.RetroShare1, precision);
                claimRegisterBo.RetroShare2Str = Util.DoubleToString(entity.RetroShare2, precision);
                claimRegisterBo.RetroShare3Str = Util.DoubleToString(entity.RetroShare3, precision);
                claimRegisterBo.SaFactorStr = Util.DoubleToString(entity.SaFactor, precision);
                claimRegisterBo.SumInsStr = Util.DoubleToString(entity.SumIns, precision);
                claimRegisterBo.TempA1Str = Util.DoubleToString(entity.TempA1, precision);
                claimRegisterBo.TempA2Str = Util.DoubleToString(entity.TempA2, precision);

                // Date
                string dateFormat = Util.GetDateFormat();
                claimRegisterBo.TargetDateToIssueInvoiceStr = entity.TargetDateToIssueInvoice?.ToString(dateFormat);
                claimRegisterBo.CedantDateOfNotificationStr = entity.CedantDateOfNotification?.ToString(dateFormat);
                claimRegisterBo.DateApprovedStr = entity.DateApproved?.ToString(dateFormat);
                claimRegisterBo.DateOfEventStr = entity.DateOfEvent?.ToString(dateFormat);
                claimRegisterBo.DateOfRegisterStr = entity.DateOfRegister?.ToString(dateFormat);
                claimRegisterBo.DateOfReportedStr = entity.DateOfReported?.ToString(dateFormat);
                claimRegisterBo.InsuredDateOfBirthStr = entity.InsuredDateOfBirth?.ToString(dateFormat);
                claimRegisterBo.LastTransactionDateStr = entity.LastTransactionDate?.ToString(dateFormat);
                claimRegisterBo.MlreInvoiceDateStr = entity.MlreInvoiceDate?.ToString(dateFormat);
                claimRegisterBo.ReinsEffDatePolStr = entity.ReinsEffDatePol?.ToString(dateFormat);
                claimRegisterBo.RetroStatementDate1Str = entity.RetroStatementDate1?.ToString(dateFormat);
                claimRegisterBo.RetroStatementDate2Str = entity.RetroStatementDate2?.ToString(dateFormat);
                claimRegisterBo.RetroStatementDate3Str = entity.RetroStatementDate3?.ToString(dateFormat);
                claimRegisterBo.TempD1Str = entity.TempD1?.ToString(dateFormat);
                claimRegisterBo.TempD2Str = entity.TempD2?.ToString(dateFormat);
                claimRegisterBo.TransactionDateWopStr = entity.TransactionDateWop?.ToString(dateFormat);
                claimRegisterBo.IssueDatePolStr = entity.IssueDatePol?.ToString(dateFormat);
                claimRegisterBo.PolicyExpiryDateStr = entity.PolicyExpiryDate?.ToString(dateFormat);
                claimRegisterBo.SignOffDateStr = entity.SignOffDate?.ToString(dateFormat);
                claimRegisterBo.ClaimCommitteeDateCommented1Str = entity.ClaimCommitteeDateCommented1?.ToString(dateFormat);
                claimRegisterBo.ClaimCommitteeDateCommented2Str = entity.ClaimCommitteeDateCommented2?.ToString(dateFormat);
                claimRegisterBo.UpdatedOnBehalfAtStr = entity.UpdatedOnBehalfAt?.ToString(dateFormat);
                claimRegisterBo.DateOfIntimationStr = entity.DateOfIntimation?.ToString(dateFormat);

                // Constants
                claimRegisterBo.PostComputationStatusStr = ClaimRegisterBo.GetPostComputationStatusName(entity.PostComputationStatus);
                claimRegisterBo.PostValidationStatusStr = ClaimRegisterBo.GetPostValidationStatusName(entity.PostValidationStatus);
                claimRegisterBo.StatusName = ClaimRegisterBo.GetStatusName(entity.ClaimStatus);
            }

            return claimRegisterBo;
        }

        public static ClaimRegister FormEntity(ClaimRegisterBo bo = null)
        {
            if (bo == null)
                return null;
            return new ClaimRegister
            {
                Id = bo.Id,
                ClaimDataBatchId = bo.ClaimDataBatchId,
                ClaimDataId = bo.ClaimDataId,
                ClaimDataConfigId = bo.ClaimDataConfigId,
                SoaDataBatchId = bo.SoaDataBatchId,
                RiDataWarehouseId = bo.RiDataWarehouseId,
                ReferralRiDataId = bo.ReferralRiDataId,
                ClaimStatus = bo.ClaimStatus,
                ClaimDecisionStatus = bo.ClaimDecisionStatus,
                ReferralClaimId = bo.ReferralClaimId,
                OriginalClaimRegisterId = bo.OriginalClaimRegisterId,
                ClaimReasonId = bo.ClaimReasonId,
                ClaimCode = bo.ClaimCode,
                PicDaaId = bo.PicDaaId,
                PicClaimId = bo.PicClaimId,
                ProvisionStatus = bo.ProvisionStatus,
                DrProvisionStatus = bo.DrProvisionStatus,
                TargetDateToIssueInvoice = bo.TargetDateToIssueInvoice,
                IsReferralCase = bo.IsReferralCase,
                ClaimId = bo.ClaimId,
                MappingStatus = bo.MappingStatus,
                ProcessingStatus = bo.ProcessingStatus,
                DuplicationCheckStatus = bo.DuplicationCheckStatus,
                PostComputationStatus = bo.PostComputationStatus,
                PostValidationStatus = bo.PostValidationStatus,
                OffsetStatus = bo.OffsetStatus,
                Errors = bo.Errors,
                ProvisionErrors = bo.ProvisionErrors,
                RedFlagWarnings = bo.RedFlagWarnings,
                RequestUnderwriterReview = bo.RequestUnderwriterReview,
                UnderwriterFeedback = bo.UnderwriterFeedback,
                HasRedFlag = bo.HasRedFlag,

                PolicyNumber = bo.PolicyNumber,
                PolicyTerm = bo.PolicyTerm,
                ClaimRecoveryAmt = bo.ClaimRecoveryAmt,
                ClaimTransactionType = bo.ClaimTransactionType,
                TreatyCode = bo.TreatyCode,
                TreatyType = bo.TreatyType,
                AarPayable = bo.AarPayable,
                AnnualRiPrem = bo.AnnualRiPrem,
                CauseOfEvent = bo.CauseOfEvent,
                CedantClaimEventCode = bo.CedantClaimEventCode,
                CedantClaimType = bo.CedantClaimType,
                CedantDateOfNotification = bo.CedantDateOfNotification,
                CedingBenefitRiskCode = bo.CedingBenefitRiskCode,
                CedingBenefitTypeCode = bo.CedingBenefitTypeCode,
                CedingClaimType = bo.CedingClaimType,
                CedingCompany = bo.CedingCompany,
                CedingEventCode = bo.CedingEventCode,
                CedingPlanCode = bo.CedingPlanCode,
                CurrencyRate = bo.CurrencyRate,
                CurrencyCode = bo.CurrencyCode,
                DateApproved = bo.DateApproved,
                DateOfEvent = bo.DateOfEvent,
                DateOfRegister = bo.DateOfRegister,
                DateOfReported = bo.DateOfReported,
                EntryNo = bo.EntryNo,
                ExGratia = bo.ExGratia,
                ForeignClaimRecoveryAmt = bo.ForeignClaimRecoveryAmt,
                FundsAccountingTypeCode = bo.FundsAccountingTypeCode,
                InsuredDateOfBirth = bo.InsuredDateOfBirth,
                InsuredGenderCode = bo.InsuredGenderCode,
                InsuredName = bo.InsuredName,
                InsuredTobaccoUse = bo.InsuredTobaccoUse,
                LastTransactionDate = bo.LastTransactionDate,
                LastTransactionQuarter = bo.LastTransactionQuarter,
                LateInterest = bo.LateInterest,
                Layer1SumRein = bo.Layer1SumRein,
                Mfrs17AnnualCohort = bo.Mfrs17AnnualCohort,
                Mfrs17ContractCode = bo.Mfrs17ContractCode,
                MlreBenefitCode = bo.MlreBenefitCode,
                MlreEventCode = bo.MlreEventCode,
                MlreInvoiceDate = bo.MlreInvoiceDate,
                MlreInvoiceNumber = bo.MlreInvoiceNumber,
                MlreRetainAmount = bo.MlreRetainAmount,
                MlreShare = bo.MlreShare,
                PendingProvisionDay = bo.PendingProvisionDay,
                PolicyDuration = bo.PolicyDuration,
                RecordType = bo.RecordType,
                ReinsBasisCode = bo.ReinsBasisCode,
                ReinsEffDatePol = bo.ReinsEffDatePol,
                RetroParty1 = bo.RetroParty1,
                RetroParty2 = bo.RetroParty2,
                RetroParty3 = bo.RetroParty3,
                RetroRecovery1 = bo.RetroRecovery1,
                RetroRecovery2 = bo.RetroRecovery2,
                RetroRecovery3 = bo.RetroRecovery3,
                RetroStatementDate1 = bo.RetroStatementDate1,
                RetroStatementDate2 = bo.RetroStatementDate2,
                RetroStatementDate3 = bo.RetroStatementDate3,
                RetroStatementId1 = bo.RetroStatementId1,
                RetroStatementId2 = bo.RetroStatementId2,
                RetroStatementId3 = bo.RetroStatementId3,
                RetroShare1 = bo.RetroShare1,
                RetroShare2 = bo.RetroShare2,
                RetroShare3 = bo.RetroShare3,
                RiskPeriodMonth = bo.RiskPeriodMonth,
                RiskPeriodYear = bo.RiskPeriodYear,
                RiskQuarter = bo.RiskQuarter,
                SaFactor = bo.SaFactor,
                SoaQuarter = bo.SoaQuarter,
                SumIns = bo.SumIns,
                TempA1 = bo.TempA1,
                TempA2 = bo.TempA2,
                TempD1 = bo.TempD1,
                TempD2 = bo.TempD2,
                TempI1 = bo.TempI1,
                TempI2 = bo.TempI2,
                TempS1 = bo.TempS1,
                TempS2 = bo.TempS2,
                TransactionDateWop = bo.TransactionDateWop,
                MlreReferenceNo = bo.MlreReferenceNo,
                AddInfo = bo.AddInfo,
                Remark1 = bo.Remark1,
                Remark2 = bo.Remark2,
                IssueDatePol = bo.IssueDatePol,
                PolicyExpiryDate = bo.PolicyExpiryDate,
                ClaimAssessorId = bo.ClaimAssessorId,
                Comment = bo.Comment,
                SignOffById = bo.SignOffById,
                SignOffDate = bo.SignOffDate,
                CedingTreatyCode = bo.CedingTreatyCode,
                CampaignCode = bo.CampaignCode,
                DateOfIntimation = bo.DateOfIntimation,

                // Ex Gratia
                EventChronologyComment = bo.EventChronologyComment,
                ClaimAssessorRecommendation = bo.ClaimAssessorRecommendation,
                ClaimCommitteeComment1 = bo.ClaimCommitteeComment1,
                ClaimCommitteeComment2 = bo.ClaimCommitteeComment2,
                ClaimCommitteeUser1Id = bo.ClaimCommitteeUser1Id,
                ClaimCommitteeUser1Name = bo.ClaimCommitteeUser1Name,
                ClaimCommitteeUser2Id = bo.ClaimCommitteeUser2Id,
                ClaimCommitteeUser2Name = bo.ClaimCommitteeUser2Name,
                ClaimCommitteeDateCommented1 = bo.ClaimCommitteeDateCommented1,
                ClaimCommitteeDateCommented2 = bo.ClaimCommitteeDateCommented2,
                CeoClaimReasonId = bo.CeoClaimReasonId,
                CeoComment = bo.CeoComment,
                UpdatedOnBehalfById = bo.UpdatedOnBehalfById,
                UpdatedOnBehalfAt = bo.UpdatedOnBehalfAt,

                // Checklist
                Checklist = bo.Checklist,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static IList<ClaimRegisterBo> FormBos(IList<ClaimRegister> entities = null, bool foreign = true, bool formatOutput = false)
        {
            if (entities == null)
                return null;
            IList<ClaimRegisterBo> bos = new List<ClaimRegisterBo>() { };
            foreach (ClaimRegister entity in entities)
            {
                bos.Add(FormBo(entity, foreign, formatOutput));
            }
            return bos;
        }

        // Parsing Referral Claims to Claim Register
        public static ClaimRegisterBo FormBo(ReferralClaim entity)
        {
            return new ClaimRegisterBo()
            {
                Id = entity.Id,
                PolicyNumber = entity.PolicyNumber,
                ReferralId = entity.ReferralId,
                CedingCompany = entity.CedingCompany,
                SumInsStr = Util.DoubleToString(entity.SumInsured, 2),
                CauseOfEvent = entity.CauseOfEvent,
                ClaimRecoveryAmtStr = Util.DoubleToString(entity.ClaimRecoveryAmount, 2),
                PicClaimBo = UserService.Find(entity.PersonInChargeId),
                StatusName = ReferralClaimBo.GetStatusName(entity.Status),
            };
        }

        public static IList<ClaimRegisterBo> FormBos(IList<ReferralClaim> entities = null, bool foreign = true, bool formatOutput = false)
        {
            if (entities == null)
                return null;
            IList<ClaimRegisterBo> bos = new List<ClaimRegisterBo>() { };
            foreach (ReferralClaim entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        // General Functions
        public static bool IsExists(int id)
        {
            return ClaimRegister.IsExists(id);
        }

        public static ClaimRegisterBo Find(int id, bool isClaimOnly = false)
        {
            return FormBo(ClaimRegister.Find(id, isClaimOnly));
        }

        public static ClaimRegisterBo Find(int? id, bool foreign = true, bool formatOutput = false, int? precision = 2)
        {
            if (id.HasValue)
                return FormBo(ClaimRegister.Find(id.Value), foreign, formatOutput, precision);
            return null;
        }

        public static Result Save(ref ClaimRegisterBo bo)
        {
            if (!ClaimRegister.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref ClaimRegisterBo bo, ref TrailObject trail)
        {
            if (!ClaimRegister.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref ClaimRegisterBo bo)
        {
            ClaimRegister entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static void Create(ref ClaimRegisterBo bo, AppDbContext db)
        {
            ClaimRegister entity = FormEntity(bo);
            entity.Create(db);
            bo.Id = entity.Id;
        }

        public static Result Create(ref ClaimRegisterBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ClaimRegisterBo bo, bool toUpdateLastTransactionDate = false)
        {
            Result result = Result();

            ClaimRegister entity = ClaimRegister.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (!result.Valid)
                return result;

            entity.ClaimDataBatchId = bo.ClaimDataBatchId;
            entity.ClaimDataId = bo.ClaimDataId;
            entity.ClaimDataConfigId = bo.ClaimDataConfigId;
            entity.SoaDataBatchId = bo.SoaDataBatchId;
            entity.RiDataWarehouseId = bo.RiDataWarehouseId;
            entity.ReferralRiDataId = bo.ReferralRiDataId;
            entity.ClaimStatus = bo.ClaimStatus;
            entity.ClaimDecisionStatus = bo.ClaimDecisionStatus;
            entity.ReferralClaimId = bo.ReferralClaimId;
            entity.OriginalClaimRegisterId = bo.OriginalClaimRegisterId;
            entity.ClaimReasonId = bo.ClaimReasonId;
            entity.PicDaaId = bo.PicDaaId;
            entity.PicClaimId = bo.PicClaimId;
            entity.ProvisionStatus = bo.ProvisionStatus;
            entity.DrProvisionStatus = bo.DrProvisionStatus;
            entity.TargetDateToIssueInvoice = bo.TargetDateToIssueInvoice;
            entity.IsReferralCase = bo.IsReferralCase;
            entity.ClaimId = bo.ClaimId;
            entity.ClaimCode = bo.ClaimCode;
            entity.MappingStatus = bo.MappingStatus;
            entity.ProcessingStatus = bo.ProcessingStatus;
            entity.DuplicationCheckStatus = bo.DuplicationCheckStatus;
            entity.PostComputationStatus = bo.PostComputationStatus;
            entity.PostValidationStatus = bo.PostValidationStatus;
            entity.Errors = bo.Errors;
            entity.ProvisionErrors = bo.ProvisionErrors;
            entity.RedFlagWarnings = bo.RedFlagWarnings;
            entity.RequestUnderwriterReview = bo.RequestUnderwriterReview;
            entity.UnderwriterFeedback = bo.UnderwriterFeedback;
            entity.HasRedFlag = bo.HasRedFlag;

            entity.PolicyNumber = bo.PolicyNumber;
            entity.PolicyTerm = bo.PolicyTerm;
            entity.ClaimRecoveryAmt = bo.ClaimRecoveryAmt;
            entity.ClaimTransactionType = bo.ClaimTransactionType;
            entity.TreatyCode = bo.TreatyCode;
            entity.TreatyType = bo.TreatyType;
            entity.AarPayable = bo.AarPayable;
            entity.AnnualRiPrem = bo.AnnualRiPrem;
            entity.CauseOfEvent = bo.CauseOfEvent;
            entity.CedantClaimEventCode = bo.CedantClaimEventCode;
            entity.CedantClaimType = bo.CedantClaimType;
            entity.CedantDateOfNotification = bo.CedantDateOfNotification;
            entity.CedingBenefitRiskCode = bo.CedingBenefitRiskCode;
            entity.CedingBenefitTypeCode = bo.CedingBenefitTypeCode;
            entity.CedingClaimType = bo.CedingClaimType;
            entity.CedingCompany = bo.CedingCompany;
            entity.CedingEventCode = bo.CedingEventCode;
            entity.CedingPlanCode = bo.CedingPlanCode;
            entity.CurrencyRate = bo.CurrencyRate;
            entity.CurrencyCode = bo.CurrencyCode;
            entity.DateApproved = bo.DateApproved;
            entity.DateOfEvent = bo.DateOfEvent;
            entity.DateOfRegister = bo.DateOfRegister;
            entity.DateOfReported = bo.DateOfReported;
            entity.EntryNo = bo.EntryNo;
            entity.ExGratia = bo.ExGratia;
            entity.ForeignClaimRecoveryAmt = bo.ForeignClaimRecoveryAmt;
            entity.FundsAccountingTypeCode = bo.FundsAccountingTypeCode;
            entity.InsuredDateOfBirth = bo.InsuredDateOfBirth;
            entity.InsuredGenderCode = bo.InsuredGenderCode;
            entity.InsuredName = bo.InsuredName;
            entity.InsuredTobaccoUse = bo.InsuredTobaccoUse;
            entity.LastTransactionDate = toUpdateLastTransactionDate ? DateTime.Today : bo.LastTransactionDate;
            entity.LastTransactionQuarter = toUpdateLastTransactionDate ? Util.GetCurrentQuarter() : bo.LastTransactionQuarter;
            entity.LateInterest = bo.LateInterest;
            entity.Layer1SumRein = bo.Layer1SumRein;
            entity.Mfrs17AnnualCohort = bo.Mfrs17AnnualCohort;
            entity.Mfrs17ContractCode = bo.Mfrs17ContractCode;
            entity.MlreBenefitCode = bo.MlreBenefitCode;
            entity.MlreEventCode = bo.MlreEventCode;
            entity.MlreInvoiceDate = bo.MlreInvoiceDate;
            entity.MlreInvoiceNumber = bo.MlreInvoiceNumber;
            entity.MlreRetainAmount = bo.MlreRetainAmount;
            entity.MlreShare = bo.MlreShare;
            entity.OffsetStatus = bo.OffsetStatus;
            entity.PendingProvisionDay = bo.PendingProvisionDay;
            entity.PolicyDuration = bo.PolicyDuration;
            entity.RecordType = bo.RecordType;
            entity.ReinsBasisCode = bo.ReinsBasisCode;
            entity.ReinsEffDatePol = bo.ReinsEffDatePol;
            entity.RetroParty1 = bo.RetroParty1;
            entity.RetroParty2 = bo.RetroParty2;
            entity.RetroParty3 = bo.RetroParty3;
            entity.RetroRecovery1 = bo.RetroRecovery1;
            entity.RetroRecovery2 = bo.RetroRecovery2;
            entity.RetroRecovery3 = bo.RetroRecovery3;
            entity.RetroStatementDate1 = bo.RetroStatementDate1;
            entity.RetroStatementDate2 = bo.RetroStatementDate2;
            entity.RetroStatementDate3 = bo.RetroStatementDate3;
            entity.RetroStatementId1 = bo.RetroStatementId1;
            entity.RetroStatementId2 = bo.RetroStatementId2;
            entity.RetroStatementId3 = bo.RetroStatementId3;
            entity.RetroShare1 = bo.RetroShare1;
            entity.RetroShare2 = bo.RetroShare2;
            entity.RetroShare3 = bo.RetroShare3;
            entity.RiskPeriodMonth = bo.RiskPeriodMonth;
            entity.RiskPeriodYear = bo.RiskPeriodYear;
            entity.RiskQuarter = bo.RiskQuarter;
            entity.SaFactor = bo.SaFactor;
            entity.SoaQuarter = bo.SoaQuarter;
            entity.SumIns = bo.SumIns;
            entity.TempA1 = bo.TempA1;
            entity.TempA2 = bo.TempA2;
            entity.TempD1 = bo.TempD1;
            entity.TempD2 = bo.TempD2;
            entity.TempI1 = bo.TempI1;
            entity.TempI2 = bo.TempI2;
            entity.TempS1 = bo.TempS1;
            entity.TempS2 = bo.TempS2;
            entity.TransactionDateWop = bo.TransactionDateWop;
            entity.MlreReferenceNo = bo.MlreReferenceNo;
            entity.AddInfo = bo.AddInfo;
            entity.Remark1 = bo.Remark1;
            entity.Remark2 = bo.Remark2;
            entity.IssueDatePol = bo.IssueDatePol;
            entity.PolicyExpiryDate = bo.PolicyExpiryDate;
            entity.ClaimAssessorId = bo.ClaimAssessorId;
            entity.Comment = bo.Comment;
            entity.SignOffById = bo.SignOffById;
            entity.SignOffDate = bo.SignOffDate;
            entity.CedingTreatyCode = bo.CedingTreatyCode;
            entity.CampaignCode = bo.CampaignCode;
            entity.DateOfIntimation = bo.DateOfIntimation;

            // Ex Gratia
            entity.EventChronologyComment = bo.EventChronologyComment;
            entity.ClaimAssessorRecommendation = bo.ClaimAssessorRecommendation;
            entity.ClaimCommitteeComment1 = bo.ClaimCommitteeComment1;
            entity.ClaimCommitteeComment2 = bo.ClaimCommitteeComment2;
            entity.ClaimCommitteeUser1Id = bo.ClaimCommitteeUser1Id;
            entity.ClaimCommitteeUser1Name = bo.ClaimCommitteeUser1Name;
            entity.ClaimCommitteeUser2Id = bo.ClaimCommitteeUser2Id;
            entity.ClaimCommitteeUser2Name = bo.ClaimCommitteeUser2Name;
            entity.ClaimCommitteeDateCommented1 = bo.ClaimCommitteeDateCommented1;
            entity.ClaimCommitteeDateCommented2 = bo.ClaimCommitteeDateCommented2;
            entity.CeoClaimReasonId = bo.CeoClaimReasonId;
            entity.CeoComment = bo.CeoComment;
            entity.UpdatedOnBehalfById = bo.UpdatedOnBehalfById;
            entity.UpdatedOnBehalfAt = bo.UpdatedOnBehalfAt;

            // Checklist
            entity.Checklist = bo.Checklist;

            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

            result.DataTrail = entity.Update();
            return result;
        }

        public static Result Update(ref ClaimRegisterBo bo, ref TrailObject trail, bool toUpdateLastTransactionDate = false)
        {
            Result result = Update(ref bo, toUpdateLastTransactionDate);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ClaimRegisterBo bo)
        {
            ClaimRegister.Delete(bo.Id);
        }

        public static Result Delete(ClaimRegisterBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = ClaimRegister.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }


        // Shared Functions
        public static int CountByClaimDataIds(List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimRegister.Where(q => q.ClaimDataId != null && ids.Contains(q.ClaimDataId.Value)).Count();
            }
        }

        public static int CountByClaimDataConfigIdStatus(int claimDataConfigId, int status)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimRegister.Where(q => q.ClaimDataConfigId == claimDataConfigId && q.ClaimStatus == status).Count();
            }
        }

        public static IList<ClaimRegisterBo> GetByIds(List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.ClaimRegister.Where(q => ids.Contains(q.Id)).ToList(), false, true);
            }
        }

        public static Dictionary<int, string> GetClaimIds()
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimRegister.Where(q => !string.IsNullOrEmpty(q.ClaimId)).OrderBy(q => q.ClaimId).ToDictionary(q => q.Id, q => q.ClaimId);
            }
        }

        public static ClaimRegisterBo FindByClaimStatus(int claimStatus)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.ClaimRegister.Where(q => q.ClaimStatus == claimStatus).FirstOrDefault());
            }
        }

        public static IList<ClaimRegisterBo> GetByOldClaimRegisterId(AppDbContext db, int claimRegisterId)
        {
            return FormBos(db.ClaimRegister.Where(q => q.OriginalClaimRegisterId == claimRegisterId).ToList(), true, true);
        }

        public static int CountByClaimDataId(int claimDataId)
        {
            return ClaimRegister.CountByClaimDataId(claimDataId);
        }

        public static int CountByRiDataId(int riDataId)
        {
            return ClaimRegister.CountByRiDataId(riDataId);
        }

        public static int CountByRiDataWarehouseId(int riDataWarehouseId)
        {
            return ClaimRegister.CountByRiDataWarehouseId(riDataWarehouseId);
        }

        public static int CountByOldClaimRegisterId(int oldClaimDataId)
        {
            return ClaimRegister.CountByOriginalClaimRegisterId(oldClaimDataId);
        }

        public static int CountByClaimReasonId(int claimReasonId)
        {
            return ClaimRegister.CountByClaimReasonId(claimReasonId);
        }

        public static int CountByClaimCode(string claimCode)
        {
            return ClaimRegister.CountByClaimCode(claimCode);
        }

        public static int CountByPicClaimId(int picClaimId)
        {
            return ClaimRegister.CountByPicClaimId(picClaimId);
        }

        public static int CountByPicDaaId(int picDaaId)
        {
            return ClaimRegister.CountByPicDaaId(picDaaId);
        }

        public static int CountByProvisionStatus(int provisionStatus)
        {
            using (var db = new AppDbContext(false))
            {
                return db.ClaimRegister.Where(q => q.ProvisionStatus == provisionStatus).Count();
            }
        }

        public static IList<ClaimRegisterBo> GetByClaimStatus(int claimStatus, bool foreign = true, bool formatOutput = false)
        {
            using (var db = new AppDbContext(false))
            {
                var bos = db.ClaimRegister.Where(q => q.ClaimStatus == claimStatus).ToList();

                return FormBos(bos, foreign, formatOutput);
            }
        }

        public static IList<ClaimRegisterBo> GetByClaimDataBatchId(int claimDataBatchId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.ClaimRegister.Where(q => q.ClaimDataBatchId == claimDataBatchId).ToList());
            }
        }

        public static int CountByClaimDataBatchId(int claimDataBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimRegister.Where(q => q.ClaimDataBatchId == claimDataBatchId).Count();
            }
        }

        public static IList<ClaimRegisterBo> GetByClaimDataBatchId(int claimDataBatchId, int skip, int take)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimRegister.Where(q => q.ClaimDataBatchId == claimDataBatchId).OrderBy(q => q.Id).Skip(skip).Take(take);

                return FormBos(query.ToList());
            }
        }

        public static int CountByDrProvisionStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimRegister
                    .Where(q => q.DrProvisionStatus == status)
                    .Count();
            }
        }

        public static IList<ClaimRegisterBo> GetByDrProvisionStatus(int status, int skip, int take)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimRegister.Where(q => q.DrProvisionStatus == status).OrderBy(q => q.Id).Skip(skip).Take(take);

                return FormBos(query.ToList());
            }
        }

        // Related Claims
        public static IList<ClaimRegisterBo> GetRelatedClaims(int id)
        {
            using (var db = new AppDbContext())
            {
                ClaimRegisterBo bo = GetMainClaimRegister(db, id);
                IList<ClaimRegisterBo> bos = GetLinkedClaimRegisters(db, bo).Where(q => q.Id != id).ToList();
                return bos;
            }
        }

        public static double SumRelatedClaimsClaimAmount(int id, int? excludeId = null)
        {
            using (var db = new AppDbContext())
            {
                ClaimRegisterBo bo = GetMainClaimRegister(db, id);
                IEnumerable<ClaimRegisterBo> query = GetLinkedClaimRegisters(db, bo);
                if (excludeId.HasValue)
                    query = query.Where(q => q.Id != excludeId);

                if (bo.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                    return query.Sum(q => q.ClaimRecoveryAmt ?? 0);

                return query.Sum(q => q.ForeignClaimRecoveryAmt ?? 0);
            }
        }

        public static ClaimRegisterBo FindForClaimRegister(int? claimRegisterId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimRegister.Where(q => q.Id == claimRegisterId);
                return FormBo(query.FirstOrDefault());
            }
        }

        public static ClaimRegisterBo GetMainClaimRegister(AppDbContext db, int id)
        {
            ClaimRegisterBo claimRegisterBo = Find(id);

            if (!claimRegisterBo.OriginalClaimRegisterId.HasValue)
            {
                return claimRegisterBo;
            }

            return GetMainClaimRegister(db, claimRegisterBo.OriginalClaimRegisterId.Value);
        }

        public static List<ClaimRegisterBo> GetLinkedClaimRegisters(AppDbContext db, ClaimRegisterBo bo)
        {
            List<ClaimRegisterBo> bos = new List<ClaimRegisterBo>();

            bos.Add(bo);
            foreach (ClaimRegisterBo findBo in GetByOldClaimRegisterId(db, bo.Id))
            {
                bos.AddRange(GetLinkedClaimRegisters(db, findBo));
            }

            return bos;
        }

        public static List<int> GetLinkedClaimRegisterIds(AppDbContext db, int id)
        {
            ClaimRegisterBo claimRegisterBo = Find(id);
            List<int> ids = new List<int>();

            ids.Add(id);
            foreach (ClaimRegisterBo bo in GetByOldClaimRegisterId(db, claimRegisterBo.Id))
            {
                ids.AddRange(GetLinkedClaimRegisterIds(db, bo.Id));
            }

            return ids;
        }

        // Get By Referral Claim Params
        public static IList<ClaimRegisterBo> GetByReferralClaim(ReferralClaimBo bo)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimRegister
                    .Where(q => q.MlreBenefitCode == BenefitBo.CodeHTH)
                    .Where(q => q.InsuredName == bo.InsuredName)
                    .Where(q => q.DateOfEvent == bo.DateOfEvent)
                    .Where(q => q.ClaimRecoveryAmt.HasValue)
                    .Where(q => !string.IsNullOrEmpty(q.ClaimCode));

                return FormBos(query.ToList(), false);
            }
        }

        // Duplicate
        public static IQueryable<ClaimRegister> DuplicateQuery(AppDbContext db, ClaimRegisterBo bo)
        {
            ClaimRegisterBo mainBo = GetMainClaimRegister(db, bo.Id);
            List<int> ids = GetLinkedClaimRegisterIds(db, mainBo.Id);

            bool isMedical = ClaimCodeBo.GetMedicalClaimCodes().Contains(bo.ClaimCode);
            var query = db.ClaimRegister
                    .Where(q => q.PolicyNumber == bo.PolicyNumber)
                    .Where(q => q.InsuredName == bo.InsuredName)
                    .Where(q => q.CedingPlanCode == bo.CedingPlanCode)
                    .Where(q => !q.IsReferralCase)
                    .Where(q => q.Id != bo.Id)
                    .Where(q => !ids.Contains(q.Id));

            if (isMedical)
            {
                query = query.Where(q => q.ClaimRecoveryAmt == bo.ClaimRecoveryAmt)
                    .Where(q => q.DateOfEvent == bo.DateOfEvent)
                    .Where(q => q.SoaQuarter != bo.SoaQuarter);
            }
            else
            {
                query = query.Where(q => q.InsuredDateOfBirth == bo.InsuredDateOfBirth)
                    .Where(q => q.TreatyCode == bo.TreatyCode)
                    .Where(q => q.ClaimCode == bo.ClaimCode)
                    .Where(q => q.CauseOfEvent == bo.CauseOfEvent);

                if (bo.TransactionDateWop != null)
                {
                    query = query.Where(q => q.TransactionDateWop == bo.TransactionDateWop);
                }
            }

            return query;
        }

        public static IList<ClaimRegisterBo> GetDuplicate(ClaimRegisterBo bo)
        {
            using (var db = new AppDbContext())
            {
                var query = DuplicateQuery(db, bo);
                return FormBos(query.ToList(), false, true);
            }
        }

        public static bool HasDuplicate(ClaimRegisterBo bo)
        {
            using (var db = new AppDbContext())
            {
                var query = DuplicateQuery(db, bo);
                return query.Any();
            }
        }

        public static bool HasSuspectedDuplicate(ClaimRegisterBo bo)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimRegister
                    .Where(q => q.IsReferralCase)
                    .Where(q => q.PolicyNumber == bo.PolicyNumber)
                    .Where(q => q.InsuredName == bo.InsuredName)
                    .Where(q => q.InsuredDateOfBirth == bo.InsuredDateOfBirth)
                    .Where(q => q.TreatyCode == bo.TreatyCode)
                    .Where(q => q.ClaimCode == bo.ClaimCode)
                    .Where(q => q.CedingPlanCode == bo.CedingPlanCode)
                    .Any();
            }
        }

        public static ClaimRegisterBo FindOriginal(ClaimRegisterBo bo)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(OriginalQuery(db, bo).FirstOrDefault());
            }
        }

        public static int CountOriginal(ClaimRegisterBo bo)
        {
            using (var db = new AppDbContext())
            {
                return OriginalQuery(db, bo).Count();
            }
        }

        public static IQueryable<ClaimRegister> OriginalQuery(AppDbContext db, ClaimRegisterBo bo)
        {
            return db.ClaimRegister
                .Where(q => q.Id != bo.Id)
                .Where(q => !q.IsReferralCase)
                .Where(q => q.ClaimTransactionType == PickListDetailBo.ClaimTransactionTypeNew)
                .Where(q => q.PolicyNumber == bo.PolicyNumber)
                .Where(q => q.InsuredName == bo.InsuredName)
                .Where(q => q.InsuredGenderCode == bo.InsuredGenderCode)
                .Where(q => q.InsuredDateOfBirth == bo.InsuredDateOfBirth)
                .Where(q => q.TreatyCode == bo.TreatyCode)
                .Where(q => q.CedingCompany == bo.CedingCompany);
        }

        // Red Flag
        public static int? HasRedFlagDuplicate(ClaimRegisterBo claimRegisterBo)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimRegister
                    .Where(q => q.Id != claimRegisterBo.Id)
                    .Where(q => q.InsuredName == claimRegisterBo.InsuredName)
                    .Where(q => q.DateOfEvent == claimRegisterBo.DateOfEvent);

                if (query.Where(q => !string.IsNullOrEmpty(q.ClaimCode) && (q.ClaimCode != claimRegisterBo.ClaimCode)).Any())
                    return ClaimRegisterBo.RedFlagDuplicateDiffClaimCode;

                if (query.Where(q => (q.ClaimCode == claimRegisterBo.ClaimCode && q.CauseOfEvent != claimRegisterBo.CauseOfEvent)).Any())
                    return ClaimRegisterBo.RedFlagDuplicateDiffCOE;

                return null;
            }
        }

        // Duplicate (Referral Claim)
        public static IList<ReferralClaimBo> GetReferralClaimDuplicate(ReferralClaimBo referralClaimBo)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimRegister
                    .Where(q => q.ClaimStatus >= ClaimRegisterBo.StatusRegistered)
                    .Where(q => q.PolicyNumber == referralClaimBo.PolicyNumber)
                    .Where(q => q.InsuredName == referralClaimBo.InsuredName)
                    .Where(q => q.DateOfEvent == referralClaimBo.DateOfEvent);

                if (ClaimCodeBo.GetMedicalClaimCodes().Contains(referralClaimBo.ClaimCode) && (referralClaimBo.TreatyType == PickListDetailBo.TreatyTypeGroup || referralClaimBo.TreatyType == PickListDetailBo.TreatyTypeIndividual))
                {
                    query = query.Where(q => q.ClaimRecoveryAmt == referralClaimBo.ClaimRecoveryAmount)
                        .Where(q => q.CedingPlanCode == referralClaimBo.CedingPlanCode);
                }
                else
                {
                    query = query.Where(q => q.InsuredDateOfBirth == referralClaimBo.InsuredDateOfBirth)
                        .Where(q => q.TreatyCode == referralClaimBo.TreatyCode)
                        .Where(q => (!string.IsNullOrEmpty(q.ClaimCode) && q.ClaimCode == referralClaimBo.ClaimCode) || (!string.IsNullOrEmpty(q.CedingPlanCode) && q.CedingPlanCode == referralClaimBo.CedingPlanCode))
                        .Where(q => q.CauseOfEvent == referralClaimBo.CauseOfEvent);
                }

                return ReferralClaimService.FormBos(query.ToList());
            }
        }

        public static bool HasReferralClaimDuplicate(ReferralClaimBo bo)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimRegister
                    .Where(q => q.InsuredName == bo.InsuredName)
                    .Where(q => q.DateOfEvent == bo.DateOfEvent)
                    .Where(q => q.ClaimCode == bo.ClaimCode)
                    .Where(q => q.CauseOfEvent != bo.CauseOfEvent)
                    .Any();
            }
        }

        // Dashboard
        public static int GetCurrentClaimIdCount(DateTime today, string prefix)
        {
            using (var db = new AppDbContext())
            {
                ClaimRegister claimRegister = db.ClaimRegister.Where(q => !string.IsNullOrEmpty(q.ClaimId) && q.ClaimId.StartsWith(prefix)).OrderByDescending(q => q.ClaimId).FirstOrDefault();

                int count = 0;
                if (claimRegister != null)
                {
                    string claimId = claimRegister.ClaimId;
                    string[] claimIdInfo = claimId.Split('/');

                    string countStr;
                    if (claimIdInfo.Length == 4)
                    {
                        countStr = claimIdInfo[3];
                        int.TryParse(countStr, out count);
                    }
                }

                return count;
            }
        }

        public static double SumClaimByClaimRegister(ClaimRegisterBo bo)
        {
            using (var db = new AppDbContext())
            {
                var claimRegisters = db.ClaimRegister
                    .Where(q => q.MlreBenefitCode == BenefitBo.CodeHTH)
                    .Where(q => q.InsuredName == bo.InsuredName)
                    .Where(q => q.DateOfEvent == bo.DateOfEvent)
                    .Where(q => q.ClaimRecoveryAmt.HasValue)
                    .Where(q => !string.IsNullOrEmpty(q.ClaimCode))
                    .ToList();

                double totalClaim = 0;

                List<string> claimCodes = new List<string>();
                foreach (var claimRegister in claimRegisters)
                {
                    if (claimCodes.Contains(claimRegister.ClaimCode))
                        continue;

                    totalClaim += claimRegister.ClaimRecoveryAmt.Value;
                    claimCodes.Add(claimRegister.ClaimCode);
                }

                return totalClaim;
            }
        }

        // Process Claim Register Batch
        public static IQueryable<ClaimRegister> PendingProcessQuery(AppDbContext db)
        {
            return db.ClaimRegister.Where(q => q.ClaimStatus == ClaimRegisterBo.StatusReported && q.ClaimTransactionType != PickListDetailBo.ClaimTransactionTypeBulk);
        }

        public static int CountPendingProcess()
        {
            using (var db = new AppDbContext())
            {
                return PendingProcessQuery(db).Count();
            }
        }

        public static IList<ClaimRegisterBo> GetPendingProcess(int skip, int take, bool foreign = false)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(PendingProcessQuery(db).OrderBy(q => q.Id).Skip(skip).Take(take).ToList(), foreign);
            }
        }

        // Provisioning
        public static IQueryable<ClaimRegister> PendingProvisionQuery(AppDbContext db)
        {
            List<int> pendingProvisionStatus = new List<int>() { ClaimRegisterBo.ProvisionStatusPending, ClaimRegisterBo.ProvisionStatusPendingReprovision };
            return db.ClaimRegister
                .Where(q => pendingProvisionStatus.Contains(q.ProvisionStatus))
                .Where(q => !string.IsNullOrEmpty(q.TreatyType) && q.TreatyType != PickListDetailBo.TreatyTypeTakaful);
        }

        public static int CountPendingProvision()
        {
            using (var db = new AppDbContext())
            {
                return PendingProvisionQuery(db).Count();
            }
        }

        public static IList<ClaimRegisterBo> GetPendingProvision(int skip = 0, int take = 50)
        {
            using (var db = new AppDbContext())
            {
                var query = PendingProvisionQuery(db).OrderBy(q => q.Id).Skip(skip).Take(take);

                return FormBos(query.ToList(), false);
            }
        }

        public static IList<ClaimRegisterBo> GetByProvisionStatus(int provisionStatus, int skip = 0, int take = 50)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBos(db.ClaimRegister.Where(q => q.ProvisionStatus == provisionStatus).OrderBy(q => q.Id).Skip(skip).Take(take).ToList());
            }
        }

        // Match Old Claim Register 
        public static IList<ClaimRegisterBo> GetByMatchParam(string policyNumber, string cedingPlanCode, string insuredName, string soaQuarter, string riskQuarter, DateTime? dateOfEvent, string claimCode, string treatyCode, DateTime? dateOfBirth, string cedingCompany, int claimRegisterId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimRegister
                    .Where(q => q.InsuredName == insuredName);

                if (claimRegisterId != 0)
                    query = query.Where(q => q.Id != claimRegisterId);

                if (!string.IsNullOrEmpty(treatyCode))
                    query = query.Where(q => q.TreatyCode == treatyCode);

                if (!string.IsNullOrEmpty(policyNumber))
                    query = query.Where(q => q.PolicyNumber == policyNumber);

                if (!string.IsNullOrEmpty(cedingPlanCode))
                    query = query.Where(q => q.CedingPlanCode == cedingPlanCode);

                if (!string.IsNullOrEmpty(soaQuarter))
                    query = query.Where(q => q.SoaQuarter == soaQuarter);

                if (!string.IsNullOrEmpty(riskQuarter))
                    query = query.Where(q => q.RiskQuarter == riskQuarter);

                if (!string.IsNullOrEmpty(claimCode))
                    query = query.Where(q => q.ClaimCode == claimCode);

                if (dateOfBirth.HasValue)
                    query = query.Where(q => q.InsuredDateOfBirth == dateOfBirth);

                if (dateOfEvent.HasValue)
                    query = query.Where(q => q.DateOfEvent == dateOfEvent);

                if (!string.IsNullOrEmpty(cedingCompany))
                    query = query.Where(q => q.CedingCompany == cedingCompany);

                return FormBos(query.ToList(), false, true);
            }
        }

        // Get Running Number Fields
        public static string GetNextClaimId()
        {
            using (var db = new AppDbContext())
            {
                DateTime today = DateTime.Today;
                string month = today.Month.ToString().PadLeft(2, '0');

                string prefix = string.Format("CL/{0}/{1}/", today.Year, month);
                ClaimRegister claimRegister = db.ClaimRegister.Where(q => !string.IsNullOrEmpty(q.ClaimId) && q.ClaimId.StartsWith(prefix)).OrderByDescending(q => q.ClaimId).FirstOrDefault();

                int count = 0;
                if (claimRegister != null)
                {
                    string claimId = claimRegister.ClaimId;
                    string[] claimIdInfo = claimId.Split('/');

                    if (claimIdInfo.Length == 4)
                    {
                        string countStr = claimIdInfo[3];
                        int.TryParse(countStr, out count);
                    }
                }
                count++;
                string nextCountStr = count.ToString().PadLeft(7, '0');

                return prefix + nextCountStr;
            }
        }

        public static string GetNextEntryNo()
        {
            using (var db = new AppDbContext())
            {
                DateTime today = DateTime.Today;
                string month = today.Month.ToString().PadLeft(2, '0');

                string prefix = string.Format("{0}/", today.Year);
                ClaimRegister claimRegister = db.ClaimRegister.Where(q => !string.IsNullOrEmpty(q.EntryNo) && q.EntryNo.StartsWith(prefix)).OrderByDescending(q => q.EntryNo).FirstOrDefault();

                int count = 0;
                if (claimRegister != null)
                {
                    string entryNo = claimRegister.EntryNo;
                    string[] entryNoInfo = entryNo.Split('/');

                    if (entryNoInfo.Length == 2)
                    {
                        string countStr = entryNoInfo[1];
                        int.TryParse(countStr, out count);
                    }
                }
                count++;
                string nextCountStr = count.ToString().PadLeft(7, '0');

                return prefix + nextCountStr;
            }
        }

        // Direct Retro
        public static IQueryable<ClaimRegister> PendingDirectRetroQuery(AppDbContext db, int? soaDataBatchId = null, string treatyCode = null, int? status = null)
        {
            var query = db.ClaimRegister
                    .Where(q => q.ClaimTransactionType != PickListDetailBo.ClaimTransactionTypeBulk)
                    .Where(q => q.TreatyType != PickListDetailBo.TreatyTypeTakaful);

            if (soaDataBatchId.HasValue && !string.IsNullOrEmpty(treatyCode))
            {
                query = query.Where(q => q.SoaDataBatchId == soaDataBatchId)
                    .Where(q => q.TreatyCode == treatyCode);
            }

            if (status.HasValue)
            {
                query = query.Where(q => q.DrProvisionStatus == status);
            }

            return query;
        }

        public static int CountPendingDirectRetro(int soaDataBatchId, string treatyCode)
        {
            using (var db = new AppDbContext(false))
            {
                return PendingDirectRetroQuery(db, soaDataBatchId, treatyCode).Count();
            }
        }

        public static IList<ClaimRegisterBo> GetPendingDirectRetro(int soaDataBatchId, string treatyCode, int skip, int take)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBos(PendingDirectRetroQuery(db, soaDataBatchId, treatyCode).OrderBy(q => q.Id).Skip(skip).Take(take).ToList());
            }
        }

        public static int CountPendingDirectRetro(int drProvisionStatus)
        {
            using (var db = new AppDbContext(false))
            {
                return PendingDirectRetroQuery(db, status: drProvisionStatus).Count();
            }
        }

        public static IList<ClaimRegisterBo> GetPendingDirectRetro(int drProvisionStatus, int skip, int take)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBos(PendingDirectRetroQuery(db, status: drProvisionStatus).OrderBy(q => q.Id).Skip(skip).Take(take).ToList());
            }
        }

        public static List<string> GetDistinctClaimCodes(string treatyCode, int? soaDataBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                List<string> claimTransactionTypes = new List<string> { PickListDetailBo.ClaimTransactionTypeNew, PickListDetailBo.ClaimTransactionTypeAdjustment };

                var query = db.ClaimRegister.Where(q => q.TreatyCode == treatyCode)
                    .Where(q => q.SoaDataBatchId == soaDataBatchId)
                    .Where(q => claimTransactionTypes.Contains(q.ClaimTransactionType))
                    .Where(q => !string.IsNullOrEmpty(q.ClaimCode));

                return query.GroupBy(q => q.ClaimCode).Select(q => q.FirstOrDefault().ClaimCode).ToList();
            }
        }

        public static List<string> GetDistinctClaimIds(int? soaDataBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                return db.ClaimRegister
                    .Where(q => q.SoaDataBatchId == soaDataBatchId)
                    .GroupBy(q => q.ClaimCode)
                    .Select(q => q.FirstOrDefault().ClaimId)
                    .ToList();
            }
        }

        // Claims Operational Dashboard
        public static List<ClaimRegisterBo> GetOperationalDashboard(string claimTransactionType = null)
        {
            List<int> statuses = ClaimRegisterBo.GetOperationalDashboardDisplayStatus();
            List<string> claimTransactionTypes = ClaimRegisterBo.GetTransactionTypeList(ClaimRegisterBo.ClaimTransactionTypeBulk);
            List<ClaimRegisterBo> bos = new List<ClaimRegisterBo>();
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.ClaimRegister.ToString());

            using (var db = new AppDbContext(false))
            {
                foreach (int status in statuses)
                {
                    var query = db.ClaimRegister.Where(q => q.ClaimStatus == status);

                    if (!string.IsNullOrEmpty(claimTransactionType))
                    {
                        query = query.Where(q => q.ClaimTransactionType == claimTransactionType);
                    }
                    else
                    {
                        query = query.Where(q => claimTransactionTypes.Contains(q.ClaimTransactionType));
                    }

                    ClaimRegisterBo bo = new ClaimRegisterBo
                    {
                        ClaimStatus = status,
                        StatusName = ClaimRegisterBo.GetStatusName(status),
                        NoOfCase = query.Count()
                    };

                    if (bo.NoOfCase > 0)
                    {
                        var histories = db.StatusHistories
                            .Where(q => q.ModuleId == moduleBo.Id)
                            .Where(q => q.Status == status)
                            .Where(q => query.Select(s => s.Id).Contains(q.ObjectId))
                            .GroupBy(q => q.ObjectId)
                            .Select(g => g.OrderByDescending(q => q.Id).FirstOrDefault())
                            .ToList();

                        if (histories != null && histories.Count > 0)
                        {
                            int statusSlaDay = ClaimRegisterBo.GetStatusSlaDay(status);
                            bo.OverdueCase = histories.Where(q => (DateTime.Now.Date - q.CreatedAt.Date).Days > statusSlaDay).Count();
                            bo.MaxDay = histories.Max(q => (DateTime.Now.Date - q.CreatedAt.Date).Days);
                        }
                    }
                    bo.UnassignedCase = query.Where(q => q.PicClaimId == null).Count();

                    bos.Add(bo);
                }
            }

            return bos;
        }

        // Assigned Case Overview
        public static IList<ClaimRegisterBo> GetAssignedCaseOverview()
        {
            List<int> statuses = ClaimRegisterBo.GetOperationalDashboardDisplayStatus();
            List<string> claimTransactionTypes = ClaimRegisterBo.GetTransactionTypeList(ClaimRegisterBo.ClaimTransactionTypeBulk);

            using (var db = new AppDbContext(false))
            {
                var bos = db.ClaimRegister
                    .Where(q => q.PicClaimId != null)
                    .Where(q => claimTransactionTypes.Contains(q.ClaimTransactionType))
                    .Where(q => statuses.Contains(q.ClaimStatus))
                    .GroupBy(g => new { g.PicClaimId })
                    .Select(r => new ClaimRegisterBo
                    {
                        PicClaimId = r.Key.PicClaimId,
                        NoOfCase = r.Count(),
                    })
                    .ToList();

                foreach (ClaimRegisterBo bo in bos)
                {
                    bo.PicClaimBo = UserService.Find(bo.PicClaimId);
                }

                return bos;
            }
        }

        public static IList<ClaimRegisterBo> GetAssignedCaseByPicClaimId(int picClaimId)
        {
            var statuses = ClaimRegisterBo.GetOperationalDashboardDisplayStatus();
            var claimTransactionTypes = ClaimRegisterBo.GetTransactionTypeList(ClaimRegisterBo.ClaimTransactionTypeBulk);

            using (var db = new AppDbContext(false))
            {
                var bos = db.ClaimRegister
                    .Where(q => q.PicClaimId == picClaimId)
                    .Where(q => claimTransactionTypes.Contains(q.ClaimTransactionType))
                    .Where(q => statuses.Contains(q.ClaimStatus))
                    .GroupBy(g => new { g.ClaimStatus })
                    .Select(r => new ClaimRegisterBo
                    {
                        ClaimStatus = r.Key.ClaimStatus,
                        NoOfCase = r.Count(),
                    })
                    .OrderBy(q => q.ClaimStatus)
                    .ToList();

                foreach (var bo in bos)
                {
                    bo.StatusName = ClaimRegisterBo.GetStatusName(bo.ClaimStatus);
                }

                return bos;
            }
        }

        public static int CountTotalAssignedCaseOverview()
        {
            List<int> statuses = ClaimRegisterBo.GetOperationalDashboardDisplayStatus();
            List<string> claimTransactionTypes = ClaimRegisterBo.GetTransactionTypeList(ClaimRegisterBo.ClaimTransactionTypeBulk);

            using (var db = new AppDbContext(false))
            {
                return db.ClaimRegister
                    .Where(q => q.PicClaimId != null)
                    .Where(q => claimTransactionTypes.Contains(q.ClaimTransactionType))
                    .Where(q => statuses.Contains(q.ClaimStatus))
                    .Count();
            }
        }

        // Pending Follow Up
        public static IList<ClaimRegisterBo> GetPendingFollowUp(int? personInChargeId = null)
        {
            List<string> claimTransactionTypes = ClaimRegisterBo.GetTransactionTypeList(ClaimRegisterBo.ClaimTransactionTypeBulk);
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.ClaimRegister.ToString());
            List<ClaimRegisterBo> bos = new List<ClaimRegisterBo>();

            using (var db = new AppDbContext(false))
            {
                var remarkFollowUps = db.RemarkFollowUps
                    .Where(q => q.Remark.ModuleId == moduleBo.Id)
                    .Where(q => q.Status == RemarkFollowUpBo.StatusPending)
                    .GroupBy(q => q.Remark.ObjectId)
                    .Select(g => g.OrderByDescending(q => q.CreatedAt).FirstOrDefault())
                    .ToList();

                foreach (var remarkFollowUp in remarkFollowUps)
                {
                    var query = db.ClaimRegister
                        .Where(q => q.Id == remarkFollowUp.Remark.ObjectId)
                        .Where(q => claimTransactionTypes.Contains(q.ClaimTransactionType));

                    if (personInChargeId.HasValue)
                        query = query.Where(q => q.PicClaimId == personInChargeId);

                    var claimRegisterBo = FormBo(query.FirstOrDefault());

                    if (claimRegisterBo != null)
                    {
                        claimRegisterBo.FollowUpWith = remarkFollowUp.FollowUpUser?.FullName;
                        claimRegisterBo.FollowUpDateStr = remarkFollowUp.FollowUpAt?.ToString(Util.GetDateFormat());
                        claimRegisterBo.Remark = Util.GetTruncatedValue(remarkFollowUp.Remark.Content, 60);
                        bos.Add(claimRegisterBo);
                    }
                }

                return bos;
            }
        }

        // Outstanding case
        public static IList<ClaimRegisterBo> GetOutstandingCase(int? take = null)
        {
            List<int> statuses = ClaimRegisterBo.GetOutstandingCaseStatus();

            using (var db = new AppDbContext(false))
            {
                var query = db.ClaimRegister
                    .Where(q => q.IsReferralCase == false)
                    .GroupBy(g => new { g.TargetDateToIssueInvoice, g.TreatyCode, g.SoaQuarter })
                    .Select(r => new ClaimRegisterBo
                    {
                        TargetDateToIssueInvoice = r.Key.TargetDateToIssueInvoice,
                        TreatyCode = r.Key.TreatyCode,
                        SoaQuarter = r.Key.SoaQuarter,
                        NoOfCase = r.Count(),
                        PendingCase = r.Where(q => statuses.Contains(q.ClaimStatus)).Count(),
                    }).Where(q => q.PendingCase > 0);

                if (take.HasValue)
                {
                    query = query.Take(take.Value);
                }

                var bos = query
                    .OrderBy(q => q.TargetDateToIssueInvoice)
                    .ThenBy(q => q.TreatyCode)
                    .ThenBy(q => q.SoaQuarter)
                    .ToList();

                foreach (var bo in bos)
                {
                    if (bo.TargetDateToIssueInvoice.HasValue)
                        bo.TargetDateToIssueInvoiceStr = bo.TargetDateToIssueInvoice.Value.ToString(Util.GetDateFormat());
                }

                return bos;
            }
        }

        public static bool GetNotApproveClaimRegister(int claimDataBatchId)
        {
            using (var db = new AppDbContext())
            {
                List<int> claimStatus = new List<int> { ClaimRegisterBo.StatusApproved, ClaimRegisterBo.StatusDeclined };

                var query = db.ClaimRegister.Where(q => q.ClaimDataBatchId == claimDataBatchId)
                    .Where(q => !claimStatus.Contains(q.ClaimStatus));
                return query.Count() > 0;
            }
        }
    }
}
