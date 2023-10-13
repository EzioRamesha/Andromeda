using BusinessObject;
using BusinessObject.Claims;
using DataAccess.Entities.Claims;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Claims
{
    public class ClaimDataService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ClaimData)),
                Controller = ModuleBo.ModuleController.ClaimData.ToString(),
            };
        }

        public static Expression<Func<ClaimData, ClaimDataBo>> Expression()
        {
            return entity => new ClaimDataBo
            {
                Id = entity.Id,
                ClaimDataBatchId = entity.ClaimDataBatchId,
                ClaimDataFileId = entity.ClaimDataFileId,
                ClaimId = entity.ClaimId,
                ClaimCode = entity.ClaimCode,
                CopyAndOverwriteData = entity.CopyAndOverwriteData,

                MappingStatus = entity.MappingStatus,
                PreComputationStatus = entity.PreComputationStatus,
                PreValidationStatus = entity.PreValidationStatus,
                ReportingStatus = entity.ReportingStatus,

                Errors = entity.Errors,
                CustomField = entity.CustomField,

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
                IssueDatePol = entity.IssueDatePol,
                PolicyExpiryDate = entity.PolicyExpiryDate,
                DateOfReported = entity.DateOfReported,
                CedingTreatyCode = entity.CedingTreatyCode,
                CampaignCode = entity.CampaignCode,
                DateOfIntimation = entity.DateOfIntimation,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static ClaimDataBo FormBo(ClaimData entity = null)
        {
            if (entity == null)
                return null;
            return new ClaimDataBo
            {
                Id = entity.Id,
                ClaimDataBatchId = entity.ClaimDataBatchId,
                ClaimDataFileId = entity.ClaimDataFileId,
                ClaimId = entity.ClaimId,
                ClaimCode = entity.ClaimCode,
                CopyAndOverwriteData = entity.CopyAndOverwriteData,

                MappingStatus = entity.MappingStatus,
                PreComputationStatus = entity.PreComputationStatus,
                PreValidationStatus = entity.PreValidationStatus,
                ReportingStatus = entity.ReportingStatus,

                Errors = entity.Errors,
                CustomField = entity.CustomField,

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
                IssueDatePol = entity.IssueDatePol,
                PolicyExpiryDate = entity.PolicyExpiryDate,
                DateOfReported = entity.DateOfReported,
                CedingTreatyCode = entity.CedingTreatyCode,
                CampaignCode = entity.CampaignCode,
                DateOfIntimation = entity.DateOfIntimation,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static ClaimData FormEntity(ClaimDataBo bo = null)
        {
            if (bo == null)
                return null;
            return new ClaimData
            {
                Id = bo.Id,
                ClaimDataBatchId = bo.ClaimDataBatchId,
                ClaimDataFileId = bo.ClaimDataFileId,
                ClaimId = bo.ClaimId,
                ClaimCode = bo.ClaimCode,
                CopyAndOverwriteData = bo.CopyAndOverwriteData,

                MappingStatus = bo.MappingStatus,
                PreComputationStatus = bo.PreComputationStatus,
                PreValidationStatus = bo.PreValidationStatus,
                ReportingStatus = bo.ReportingStatus,

                Errors = bo.Errors,
                CustomField = bo.CustomField,

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
                IssueDatePol = bo.IssueDatePol,
                PolicyExpiryDate = bo.PolicyExpiryDate,
                DateOfReported = bo.DateOfReported,
                CedingTreatyCode = bo.CedingTreatyCode,
                CampaignCode = bo.CampaignCode,
                DateOfIntimation = bo.DateOfIntimation,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static IList<ClaimDataBo> FormBos(IList<ClaimData> entities = null)
        {
            if (entities == null)
                return null;
            IList<ClaimDataBo> bos = new List<ClaimDataBo>() { };
            foreach (ClaimData entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static bool IsExists(int id)
        {
            return ClaimData.IsExists(id);
        }

        public static ClaimDataBo Find(int id)
        {
            return FormBo(ClaimData.Find(id));
        }

        public static int CountByClaimDataBatchId(int claimDataBatchId)
        {
            return ClaimData.CountByClaimDataBatchId(claimDataBatchId);
        }

        public static int CountByClaimDataFileIdMappingStatus(int claimDataFileId, int mappingStatus)
        {
            return ClaimData.CountByClaimDataFileIdMappingStatus(claimDataFileId, mappingStatus);
        }

        public static int CountByClaimDataFileIdComputationStatus(int claimDataFileId, int preComputationStatus)
        {
            using (var db = new AppDbContext(false))
            {
                return db.ClaimData
                    .Where(q => q.ClaimDataFileId == claimDataFileId)
                    .Where(q => q.PreComputationStatus == preComputationStatus)
                    .Count();
            }
        }

        public static int CountByClaimDataFileIdPreValidationStatus(int claimDataFileId, int preValidationStatus)
        {
            using (var db = new AppDbContext(false))
            {
                return db.ClaimData
                    .Where(q => q.ClaimDataFileId == claimDataFileId)
                    .Where(q => q.PreValidationStatus == preValidationStatus)
                    .Count();
            }
        }

        public static int CountByClaimDataBatchIdMappingStatusFailed(int claimDataBatchId, AppDbContext db)
        {
            return db.ClaimData
                .Where(q => q.ClaimDataBatchId == claimDataBatchId)
                .Where(q => q.MappingStatus == ClaimDataBo.MappingStatusFailed)
                .Count();
        }

        public static int CountByClaimDataBatchIdPreComputationStatusFailed(int claimDataBatchId, AppDbContext db)
        {
            return db.ClaimData
                .Where(q => q.ClaimDataBatchId == claimDataBatchId)
                .Where(q => q.PreComputationStatus == ClaimDataBo.PreComputationStatusFailed)
                .Count();
        }

        public static int CountByClaimDataBatchIdPreValidationStatusFailed(int claimDataBatchId, AppDbContext db)
        {
            return db.ClaimData
                .Where(q => q.ClaimDataBatchId == claimDataBatchId)
                .Where(q => q.PreValidationStatus == ClaimDataBo.PreValidationStatusFailed)
                .Count();
        }

        public static IList<ClaimDataBo> GetByClaimDataBatchId(int claimDataBatchId, int skip, int take)
        {
            return FormBos(ClaimData.GetByClaimDataBatchId(claimDataBatchId, skip, take));
        }

        public static IList<ClaimDataBo> GetByClaimDataBatchIdClaimDataFileId(int claimDataBatchId, int claimDataFileId)
        {
            return FormBos(ClaimData.GetByClaimDataBatchIdClaimDataFileId(claimDataBatchId, claimDataFileId));
        }

        public static Result Save(ref ClaimDataBo bo)
        {
            if (!ClaimData.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref ClaimDataBo bo, ref TrailObject trail)
        {
            if (!ClaimData.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref ClaimDataBo bo)
        {
            ClaimData entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static void Create(ref ClaimDataBo bo, AppDbContext db)
        {
            ClaimData entity = FormEntity(bo);
            entity.Create(db);
            bo.Id = entity.Id;
        }

        public static Result Create(ref ClaimDataBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ClaimDataBo bo)
        {
            Result result = Result();

            ClaimData entity = ClaimData.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (!result.Valid)
                return result;

            entity.ClaimDataBatchId = bo.ClaimDataBatchId;
            entity.ClaimDataFileId = bo.ClaimDataFileId;
            entity.ClaimId = bo.ClaimId;
            entity.ClaimCode = bo.ClaimCode;
            entity.CopyAndOverwriteData = bo.CopyAndOverwriteData;

            entity.MappingStatus = bo.MappingStatus;
            entity.PreComputationStatus = bo.PreComputationStatus;
            entity.PreValidationStatus = bo.PreValidationStatus;
            entity.ReportingStatus = bo.ReportingStatus;

            entity.Errors = bo.Errors;
            entity.CustomField = bo.CustomField;

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
            entity.EntryNo = bo.EntryNo;
            entity.ExGratia = bo.ExGratia;
            entity.ForeignClaimRecoveryAmt = bo.ForeignClaimRecoveryAmt;
            entity.FundsAccountingTypeCode = bo.FundsAccountingTypeCode;
            entity.InsuredDateOfBirth = bo.InsuredDateOfBirth;
            entity.InsuredGenderCode = bo.InsuredGenderCode;
            entity.InsuredName = bo.InsuredName;
            entity.InsuredTobaccoUse = bo.InsuredTobaccoUse;
            entity.LastTransactionDate = bo.LastTransactionDate;
            entity.LastTransactionQuarter = bo.LastTransactionQuarter;
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
            entity.PendingProvisionDay = bo.PendingProvisionDay;
            entity.PolicyDuration = bo.PolicyDuration;
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
            entity.IssueDatePol = bo.IssueDatePol;
            entity.PolicyExpiryDate = bo.PolicyExpiryDate;
            entity.DateOfReported = bo.DateOfReported;
            entity.CedingTreatyCode = bo.CedingTreatyCode;
            entity.CampaignCode = bo.CampaignCode;
            entity.DateOfIntimation = bo.DateOfIntimation;
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

            result.DataTrail = entity.Update();
            return result;
        }

        public static Result Update(ref ClaimDataBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ClaimDataBo bo)
        {
            ClaimData.Delete(bo.Id);
        }

        public static Result Delete(ClaimDataBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = ClaimData.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static List<DataTrail> DeleteByClaimDataBatchId(int claimDataBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                List<DataTrail> trails = new List<DataTrail>();

                // DO NOT TRAIL since this is mass data
                /*
                var query = db.ClaimData.Where(q => q.ClaimDataBatchId == claimDataBatchId);
                foreach (ClaimData claimData in query.ToList())
                {
                    DataTrail trail = new DataTrail(claimData, true);
                    trails.Add(trail);

                    db.Entry(claimData).State = EntityState.Deleted;
                    db.ClaimData.Remove(claimData);
                }
                db.ClaimData.RemoveRange(query);
                */

                db.Database.ExecuteSqlCommand("DELETE FROM [ClaimData] WHERE [ClaimDataBatchId] = {0}", claimDataBatchId);
                db.SaveChanges();

                return trails;
            }
        }

        public static List<string> ValidateDropDownCodes(string title, List<int> types, ClaimDataBo claimData)
        {
            var errors = new List<string> { };
            foreach (var type in types)
            {
                string property = StandardClaimDataOutputBo.GetPropertyNameByType(type);
                string code = (string)claimData.GetPropertyValue(property);

                if (string.IsNullOrEmpty(code))
                    continue;

                if (PickListDetailService.CountByStandardClaimDataOutputIdCode(type, code.ToString()) == 0)
                {
                    errors.Add(claimData.FormatDropDownError(title, type, string.Format("Record not found in Pick List: {0}", code)));
                }
            }
            return errors;
        }
    }
}
