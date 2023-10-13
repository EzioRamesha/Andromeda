using BusinessObject;
using BusinessObject.Retrocession;
using DataAccess.Entities.Retrocession;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.Retrocession
{
    public class PerLifeAggregatedDataService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PerLifeAggregatedData)),
                Controller = ModuleBo.ModuleController.PerLifeAggregatedData.ToString()
            };
        }

        public static Expression<Func<PerLifeAggregatedData, PerLifeAggregatedDataBo>> Expression()
        {
            return entity => new PerLifeAggregatedDataBo
            {
                Id = entity.Id,
                PerLifeAggregationDetailId = entity.PerLifeAggregationDetailId,
                TreatyCode = entity.TreatyCode,
                ReinsBasisCode = entity.ReinsBasisCode,
                FundsAccountingTypeCode = entity.FundsAccountingTypeCode,
                PremiumFrequencyCode = entity.PremiumFrequencyCode,
                ReportPeriodMonth = entity.ReportPeriodMonth,
                ReportPeriodYear = entity.ReportPeriodYear,
                RiskPeriodMonth = entity.RiskPeriodMonth,
                RiskPeriodYear = entity.RiskPeriodYear,
                TransactionTypeCode = entity.TransactionTypeCode,
                PolicyNumber = entity.PolicyNumber,
                IssueDatePol = entity.IssueDatePol,
                IssueDateBen = entity.IssueDateBen,
                ReinsEffDatePol = entity.ReinsEffDatePol,
                ReinsEffDateBen = entity.ReinsEffDateBen,
                CedingPlanCode = entity.CedingPlanCode,
                CedingBenefitTypeCode = entity.CedingBenefitTypeCode,
                CedingBenefitRiskCode = entity.CedingBenefitRiskCode,
                MlreBenefitCode = entity.MlreBenefitCode,
                OriSumAssured = entity.OriSumAssured,
                CurrSumAssured = entity.CurrSumAssured,
                AmountCededB4MlreShare = entity.AmountCededB4MlreShare,
                AarOri = entity.AarOri,
                Aar = entity.Aar,
                InsuredName = entity.InsuredName,
                InsuredGenderCode = entity.InsuredGenderCode,
                InsuredTobaccoUse = entity.InsuredTobaccoUse,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                InsuredOccupationCode = entity.InsuredOccupationCode,
                InsuredRegisterNo = entity.InsuredRegisterNo,
                InsuredAttainedAge = entity.InsuredAttainedAge,
                InsuredNewIcNumber = entity.InsuredNewIcNumber,
                InsuredOldIcNumber = entity.InsuredOldIcNumber,
                ReinsuranceIssueAge = entity.ReinsuranceIssueAge,
                PolicyTerm = entity.PolicyTerm,
                PolicyExpiryDate = entity.PolicyExpiryDate,
                LoadingType = entity.LoadingType,
                UnderwriterRating = entity.UnderwriterRating,
                FlatExtraAmount = entity.FlatExtraAmount,
                StandardPremium = entity.StandardPremium,
                SubstandardPremium = entity.SubstandardPremium,
                FlatExtraPremium = entity.FlatExtraPremium,
                GrossPremium = entity.GrossPremium,
                StandardDiscount = entity.StandardDiscount,
                SubstandardDiscount = entity.SubstandardDiscount,
                NetPremium = entity.NetPremium,
                PolicyNumberOld = entity.PolicyNumberOld,
                PolicyLifeNumber = entity.PolicyLifeNumber,
                FundCode = entity.FundCode,
                RiderNumber = entity.RiderNumber,
                CampaignCode = entity.CampaignCode,
                Nationality = entity.Nationality,
                TerritoryOfIssueCode = entity.TerritoryOfIssueCode,
                CurrencyCode = entity.CurrencyCode,
                StaffPlanIndicator = entity.StaffPlanIndicator,
                CedingPlanCodeOld = entity.CedingPlanCodeOld,
                CedingBasicPlanCode = entity.CedingBasicPlanCode,
                GroupPolicyNumber = entity.GroupPolicyNumber,
                GroupPolicyName = entity.GroupPolicyName,
                GroupSubsidiaryName = entity.GroupSubsidiaryName,
                GroupSubsidiaryNo = entity.GroupSubsidiaryNo,
                CedingPlanCode2 = entity.CedingPlanCode2,
                DependantIndicator = entity.DependantIndicator,
                Mfrs17BasicRider = entity.Mfrs17BasicRider,
                Mfrs17CellName = entity.Mfrs17CellName,
                Mfrs17ContractCode = entity.Mfrs17ContractCode,
                LoaCode = entity.LoaCode,
                RiskPeriodStartDate = entity.RiskPeriodStartDate,
                RiskPeriodEndDate = entity.RiskPeriodEndDate,
                Mfrs17AnnualCohort = entity.Mfrs17AnnualCohort,
                BrokerageFee = entity.BrokerageFee,
                ApLoading = entity.ApLoading,
                EffectiveDate = entity.EffectiveDate,
                AnnuityFactor = entity.AnnuityFactor,
                EndingPolicyStatus = entity.EndingPolicyStatus,
                LastUpdatedDate = entity.LastUpdatedDate,
                TreatyType = entity.TreatyType,
                TreatyNumber = entity.TreatyNumber,
                RetroPremFreq = entity.RetroPremFreq,
                LifeBenefitFlag = entity.LifeBenefitFlag,
                RiskQuarter = entity.RiskQuarter,
                ProcessingDate = entity.ProcessingDate,
                UniqueKeyPerLife = entity.UniqueKeyPerLife,
                RetroBenefitCode = entity.RetroBenefitCode,
                RetroRatio = entity.RetroRatio,
                AccumulativeRetainAmount = entity.AccumulativeRetainAmount,
                RetroRetainAmount = entity.RetroRetainAmount,
                RetroAmount = entity.RetroAmount,
                RetroGrossPremium = entity.RetroGrossPremium,
                RetroNetPremium = entity.RetroNetPremium,
                RetroDiscount = entity.RetroDiscount,
                RetroExtraPremium = entity.RetroExtraPremium,
                RetroExtraComm = entity.RetroExtraComm,
                RetroGst = entity.RetroGst,
                RetroTreaty = entity.RetroTreaty,
                RetroClaimId = entity.RetroClaimId,
                Soa = entity.Soa,
                RetroIndicator = entity.RetroIndicator,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static PerLifeAggregatedDataBo FormBo(PerLifeAggregatedData entity = null)
        {
            if (entity == null)
                return null;
            return new PerLifeAggregatedDataBo
            {
                Id = entity.Id,
                PerLifeAggregationDetailId = entity.PerLifeAggregationDetailId,
                PerLifeAggregationDetailBo = PerLifeAggregationDetailService.Find(entity.PerLifeAggregationDetailId),
                TreatyCode = entity.TreatyCode,
                ReinsBasisCode = entity.ReinsBasisCode,
                FundsAccountingTypeCode = entity.FundsAccountingTypeCode,
                PremiumFrequencyCode = entity.PremiumFrequencyCode,
                ReportPeriodMonth = entity.ReportPeriodMonth,
                ReportPeriodYear = entity.ReportPeriodYear,
                RiskPeriodMonth = entity.RiskPeriodMonth,
                RiskPeriodYear = entity.RiskPeriodYear,
                TransactionTypeCode = entity.TransactionTypeCode,
                PolicyNumber = entity.PolicyNumber,
                IssueDatePol = entity.IssueDatePol,
                IssueDateBen = entity.IssueDateBen,
                ReinsEffDatePol = entity.ReinsEffDatePol,
                ReinsEffDateBen = entity.ReinsEffDateBen,
                CedingPlanCode = entity.CedingPlanCode,
                CedingBenefitTypeCode = entity.CedingBenefitTypeCode,
                CedingBenefitRiskCode = entity.CedingBenefitRiskCode,
                MlreBenefitCode = entity.MlreBenefitCode,
                OriSumAssured = entity.OriSumAssured,
                CurrSumAssured = entity.CurrSumAssured,
                AmountCededB4MlreShare = entity.AmountCededB4MlreShare,
                AarOri = entity.AarOri,
                Aar = entity.Aar,
                InsuredName = entity.InsuredName,
                InsuredGenderCode = entity.InsuredGenderCode,
                InsuredTobaccoUse = entity.InsuredTobaccoUse,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                InsuredOccupationCode = entity.InsuredOccupationCode,
                InsuredRegisterNo = entity.InsuredRegisterNo,
                InsuredAttainedAge = entity.InsuredAttainedAge,
                InsuredNewIcNumber = entity.InsuredNewIcNumber,
                InsuredOldIcNumber = entity.InsuredOldIcNumber,
                ReinsuranceIssueAge = entity.ReinsuranceIssueAge,
                PolicyTerm = entity.PolicyTerm,
                PolicyExpiryDate = entity.PolicyExpiryDate,
                LoadingType = entity.LoadingType,
                UnderwriterRating = entity.UnderwriterRating,
                FlatExtraAmount = entity.FlatExtraAmount,
                StandardPremium = entity.StandardPremium,
                SubstandardPremium = entity.SubstandardPremium,
                FlatExtraPremium = entity.FlatExtraPremium,
                GrossPremium = entity.GrossPremium,
                StandardDiscount = entity.StandardDiscount,
                SubstandardDiscount = entity.SubstandardDiscount,
                NetPremium = entity.NetPremium,
                PolicyNumberOld = entity.PolicyNumberOld,
                PolicyLifeNumber = entity.PolicyLifeNumber,
                FundCode = entity.FundCode,
                RiderNumber = entity.RiderNumber,
                CampaignCode = entity.CampaignCode,
                Nationality = entity.Nationality,
                TerritoryOfIssueCode = entity.TerritoryOfIssueCode,
                CurrencyCode = entity.CurrencyCode,
                StaffPlanIndicator = entity.StaffPlanIndicator,
                CedingPlanCodeOld = entity.CedingPlanCodeOld,
                CedingBasicPlanCode = entity.CedingBasicPlanCode,
                GroupPolicyNumber = entity.GroupPolicyNumber,
                GroupPolicyName = entity.GroupPolicyName,
                GroupSubsidiaryName = entity.GroupSubsidiaryName,
                GroupSubsidiaryNo = entity.GroupSubsidiaryNo,
                CedingPlanCode2 = entity.CedingPlanCode2,
                DependantIndicator = entity.DependantIndicator,
                Mfrs17BasicRider = entity.Mfrs17BasicRider,
                Mfrs17CellName = entity.Mfrs17CellName,
                Mfrs17ContractCode = entity.Mfrs17ContractCode,
                LoaCode = entity.LoaCode,
                RiskPeriodStartDate = entity.RiskPeriodStartDate,
                RiskPeriodEndDate = entity.RiskPeriodEndDate,
                Mfrs17AnnualCohort = entity.Mfrs17AnnualCohort,
                BrokerageFee = entity.BrokerageFee,
                ApLoading = entity.ApLoading,
                EffectiveDate = entity.EffectiveDate,
                AnnuityFactor = entity.AnnuityFactor,
                EndingPolicyStatus = entity.EndingPolicyStatus,
                LastUpdatedDate = entity.LastUpdatedDate,
                TreatyType = entity.TreatyType,
                TreatyNumber = entity.TreatyNumber,
                RetroPremFreq = entity.RetroPremFreq,
                LifeBenefitFlag = entity.LifeBenefitFlag,
                RiskQuarter = entity.RiskQuarter,
                ProcessingDate = entity.ProcessingDate,
                UniqueKeyPerLife = entity.UniqueKeyPerLife,
                RetroBenefitCode = entity.RetroBenefitCode,
                RetroRatio = entity.RetroRatio,
                AccumulativeRetainAmount = entity.AccumulativeRetainAmount,
                RetroRetainAmount = entity.RetroRetainAmount,
                RetroAmount = entity.RetroAmount,
                RetroGrossPremium = entity.RetroGrossPremium,
                RetroNetPremium = entity.RetroNetPremium,
                RetroDiscount = entity.RetroDiscount,
                RetroExtraPremium = entity.RetroExtraPremium,
                RetroExtraComm = entity.RetroExtraComm,
                RetroGst = entity.RetroGst,
                RetroTreaty = entity.RetroTreaty,
                RetroClaimId = entity.RetroClaimId,
                Soa = entity.Soa,
                RetroIndicator = entity.RetroIndicator,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<PerLifeAggregatedDataBo> FormBos(IList<PerLifeAggregatedData> entities = null)
        {
            if (entities == null)
                return null;
            IList<PerLifeAggregatedDataBo> bos = new List<PerLifeAggregatedDataBo>() { };
            foreach (PerLifeAggregatedData entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static PerLifeAggregatedData FormEntity(PerLifeAggregatedDataBo bo = null)
        {
            if (bo == null)
                return null;
            return new PerLifeAggregatedData
            {
                Id = bo.Id,
                PerLifeAggregationDetailId = bo.PerLifeAggregationDetailId,
                TreatyCode = bo.TreatyCode,
                ReinsBasisCode = bo.ReinsBasisCode,
                FundsAccountingTypeCode = bo.FundsAccountingTypeCode,
                PremiumFrequencyCode = bo.PremiumFrequencyCode,
                ReportPeriodMonth = bo.ReportPeriodMonth,
                ReportPeriodYear = bo.ReportPeriodYear,
                RiskPeriodMonth = bo.RiskPeriodMonth,
                RiskPeriodYear = bo.RiskPeriodYear,
                TransactionTypeCode = bo.TransactionTypeCode,
                PolicyNumber = bo.PolicyNumber,
                IssueDatePol = bo.IssueDatePol,
                IssueDateBen = bo.IssueDateBen,
                ReinsEffDatePol = bo.ReinsEffDatePol,
                ReinsEffDateBen = bo.ReinsEffDateBen,
                CedingPlanCode = bo.CedingPlanCode,
                CedingBenefitTypeCode = bo.CedingBenefitTypeCode,
                CedingBenefitRiskCode = bo.CedingBenefitRiskCode,
                MlreBenefitCode = bo.MlreBenefitCode,
                OriSumAssured = bo.OriSumAssured,
                CurrSumAssured = bo.CurrSumAssured,
                AmountCededB4MlreShare = bo.AmountCededB4MlreShare,
                AarOri = bo.AarOri,
                Aar = bo.Aar,
                InsuredName = bo.InsuredName,
                InsuredGenderCode = bo.InsuredGenderCode,
                InsuredTobaccoUse = bo.InsuredTobaccoUse,
                InsuredDateOfBirth = bo.InsuredDateOfBirth,
                InsuredOccupationCode = bo.InsuredOccupationCode,
                InsuredRegisterNo = bo.InsuredRegisterNo,
                InsuredAttainedAge = bo.InsuredAttainedAge,
                InsuredNewIcNumber = bo.InsuredNewIcNumber,
                InsuredOldIcNumber = bo.InsuredOldIcNumber,
                ReinsuranceIssueAge = bo.ReinsuranceIssueAge,
                PolicyTerm = bo.PolicyTerm,
                PolicyExpiryDate = bo.PolicyExpiryDate,
                LoadingType = bo.LoadingType,
                UnderwriterRating = bo.UnderwriterRating,
                FlatExtraAmount = bo.FlatExtraAmount,
                StandardPremium = bo.StandardPremium,
                SubstandardPremium = bo.SubstandardPremium,
                FlatExtraPremium = bo.FlatExtraPremium,
                GrossPremium = bo.GrossPremium,
                StandardDiscount = bo.StandardDiscount,
                SubstandardDiscount = bo.SubstandardDiscount,
                NetPremium = bo.NetPremium,
                PolicyNumberOld = bo.PolicyNumberOld,
                PolicyLifeNumber = bo.PolicyLifeNumber,
                FundCode = bo.FundCode,
                RiderNumber = bo.RiderNumber,
                CampaignCode = bo.CampaignCode,
                Nationality = bo.Nationality,
                TerritoryOfIssueCode = bo.TerritoryOfIssueCode,
                CurrencyCode = bo.CurrencyCode,
                StaffPlanIndicator = bo.StaffPlanIndicator,
                CedingPlanCodeOld = bo.CedingPlanCodeOld,
                CedingBasicPlanCode = bo.CedingBasicPlanCode,
                GroupPolicyNumber = bo.GroupPolicyNumber,
                GroupPolicyName = bo.GroupPolicyName,
                GroupSubsidiaryName = bo.GroupSubsidiaryName,
                GroupSubsidiaryNo = bo.GroupSubsidiaryNo,
                CedingPlanCode2 = bo.CedingPlanCode2,
                DependantIndicator = bo.DependantIndicator,
                Mfrs17BasicRider = bo.Mfrs17BasicRider,
                Mfrs17CellName = bo.Mfrs17CellName,
                Mfrs17ContractCode = bo.Mfrs17ContractCode,
                LoaCode = bo.LoaCode,
                RiskPeriodStartDate = bo.RiskPeriodStartDate,
                RiskPeriodEndDate = bo.RiskPeriodEndDate,
                Mfrs17AnnualCohort = bo.Mfrs17AnnualCohort,
                BrokerageFee = bo.BrokerageFee,
                ApLoading = bo.ApLoading,
                EffectiveDate = bo.EffectiveDate,
                AnnuityFactor = bo.AnnuityFactor,
                EndingPolicyStatus = bo.EndingPolicyStatus,
                LastUpdatedDate = bo.LastUpdatedDate,
                TreatyType = bo.TreatyType,
                TreatyNumber = bo.TreatyNumber,
                RetroPremFreq = bo.RetroPremFreq,
                LifeBenefitFlag = bo.LifeBenefitFlag,
                RiskQuarter = bo.RiskQuarter,
                ProcessingDate = bo.ProcessingDate,
                UniqueKeyPerLife = bo.UniqueKeyPerLife,
                RetroBenefitCode = bo.RetroBenefitCode,
                RetroRatio = bo.RetroRatio,
                AccumulativeRetainAmount = bo.AccumulativeRetainAmount,
                RetroRetainAmount = bo.RetroRetainAmount,
                RetroAmount = bo.RetroAmount,
                RetroGrossPremium = bo.RetroGrossPremium,
                RetroNetPremium = bo.RetroNetPremium,
                RetroDiscount = bo.RetroDiscount,
                RetroExtraPremium = bo.RetroExtraPremium,
                RetroExtraComm = bo.RetroExtraComm,
                RetroGst = bo.RetroGst,
                RetroTreaty = bo.RetroTreaty,
                RetroClaimId = bo.RetroClaimId,
                Soa = bo.Soa,
                RetroIndicator = bo.RetroIndicator,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return PerLifeAggregatedData.IsExists(id);
        }

        public static PerLifeAggregatedDataBo Find(int? id)
        {
            return FormBo(PerLifeAggregatedData.Find(id));
        }

        public static IList<PerLifeAggregatedDataBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeAggregatedData.ToList());
            }
        }

        public static IList<PerLifeAggregatedDataBo> GetByPerLifeAggregationDetailId(int perLifeAggregationDetailId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeAggregatedData
                    .Where(q => q.PerLifeAggregationDetailId == perLifeAggregationDetailId)
                    .ToList());
            }
        }

        public static Result Save(ref PerLifeAggregatedDataBo bo)
        {
            if (!PerLifeAggregatedData.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref PerLifeAggregatedDataBo bo, ref TrailObject trail)
        {
            if (!PerLifeAggregatedData.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref PerLifeAggregatedDataBo bo)
        {
            PerLifeAggregatedData entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref PerLifeAggregatedDataBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref PerLifeAggregatedDataBo bo)
        {
            Result result = Result();

            PerLifeAggregatedData entity = PerLifeAggregatedData.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.PerLifeAggregationDetailId = bo.PerLifeAggregationDetailId;
                entity.TreatyCode = bo.TreatyCode;
                entity.ReinsBasisCode = bo.ReinsBasisCode;
                entity.FundsAccountingTypeCode = bo.FundsAccountingTypeCode;
                entity.PremiumFrequencyCode = bo.PremiumFrequencyCode;
                entity.ReportPeriodMonth = bo.ReportPeriodMonth;
                entity.ReportPeriodYear = bo.ReportPeriodYear;
                entity.RiskPeriodMonth = bo.RiskPeriodMonth;
                entity.RiskPeriodYear = bo.RiskPeriodYear;
                entity.TransactionTypeCode = bo.TransactionTypeCode;
                entity.PolicyNumber = bo.PolicyNumber;
                entity.IssueDatePol = bo.IssueDatePol;
                entity.IssueDateBen = bo.IssueDateBen;
                entity.ReinsEffDatePol = bo.ReinsEffDatePol;
                entity.ReinsEffDateBen = bo.ReinsEffDateBen;
                entity.CedingPlanCode = bo.CedingPlanCode;
                entity.CedingBenefitTypeCode = bo.CedingBenefitTypeCode;
                entity.CedingBenefitRiskCode = bo.CedingBenefitRiskCode;
                entity.MlreBenefitCode = bo.MlreBenefitCode;
                entity.OriSumAssured = bo.OriSumAssured;
                entity.CurrSumAssured = bo.CurrSumAssured;
                entity.AmountCededB4MlreShare = bo.AmountCededB4MlreShare;
                entity.AarOri = bo.AarOri;
                entity.Aar = bo.Aar;
                entity.InsuredName = bo.InsuredName;
                entity.InsuredGenderCode = bo.InsuredGenderCode;
                entity.InsuredTobaccoUse = bo.InsuredTobaccoUse;
                entity.InsuredDateOfBirth = bo.InsuredDateOfBirth;
                entity.InsuredOccupationCode = bo.InsuredOccupationCode;
                entity.InsuredRegisterNo = bo.InsuredRegisterNo;
                entity.InsuredAttainedAge = bo.InsuredAttainedAge;
                entity.InsuredNewIcNumber = bo.InsuredNewIcNumber;
                entity.InsuredOldIcNumber = bo.InsuredOldIcNumber;
                entity.ReinsuranceIssueAge = bo.ReinsuranceIssueAge;
                entity.PolicyTerm = bo.PolicyTerm;
                entity.PolicyExpiryDate = bo.PolicyExpiryDate;
                entity.LoadingType = bo.LoadingType;
                entity.UnderwriterRating = bo.UnderwriterRating;
                entity.FlatExtraAmount = bo.FlatExtraAmount;
                entity.StandardPremium = bo.StandardPremium;
                entity.SubstandardPremium = bo.SubstandardPremium;
                entity.FlatExtraPremium = bo.FlatExtraPremium;
                entity.GrossPremium = bo.GrossPremium;
                entity.StandardDiscount = bo.StandardDiscount;
                entity.SubstandardDiscount = bo.SubstandardDiscount;
                entity.NetPremium = bo.NetPremium;
                entity.PolicyNumberOld = bo.PolicyNumberOld;
                entity.PolicyLifeNumber = bo.PolicyLifeNumber;
                entity.FundCode = bo.FundCode;
                entity.RiderNumber = bo.RiderNumber;
                entity.CampaignCode = bo.CampaignCode;
                entity.Nationality = bo.Nationality;
                entity.TerritoryOfIssueCode = bo.TerritoryOfIssueCode;
                entity.CurrencyCode = bo.CurrencyCode;
                entity.StaffPlanIndicator = bo.StaffPlanIndicator;
                entity.CedingPlanCodeOld = bo.CedingPlanCodeOld;
                entity.CedingBasicPlanCode = bo.CedingBasicPlanCode;
                entity.GroupPolicyNumber = bo.GroupPolicyNumber;
                entity.GroupPolicyName = bo.GroupPolicyName;
                entity.GroupSubsidiaryName = bo.GroupSubsidiaryName;
                entity.GroupSubsidiaryNo = bo.GroupSubsidiaryNo;
                entity.CedingPlanCode2 = bo.CedingPlanCode2;
                entity.DependantIndicator = bo.DependantIndicator;
                entity.Mfrs17BasicRider = bo.Mfrs17BasicRider;
                entity.Mfrs17CellName = bo.Mfrs17CellName;
                entity.Mfrs17ContractCode = bo.Mfrs17ContractCode;
                entity.LoaCode = bo.LoaCode;
                entity.RiskPeriodStartDate = bo.RiskPeriodStartDate;
                entity.RiskPeriodEndDate = bo.RiskPeriodEndDate;
                entity.Mfrs17AnnualCohort = bo.Mfrs17AnnualCohort;
                entity.BrokerageFee = bo.BrokerageFee;
                entity.ApLoading = bo.ApLoading;
                entity.EffectiveDate = bo.EffectiveDate;
                entity.AnnuityFactor = bo.AnnuityFactor;
                entity.EndingPolicyStatus = bo.EndingPolicyStatus;
                entity.LastUpdatedDate = bo.LastUpdatedDate;
                entity.TreatyType = bo.TreatyType;
                entity.TreatyNumber = bo.TreatyNumber;
                entity.RetroPremFreq = bo.RetroPremFreq;
                entity.LifeBenefitFlag = bo.LifeBenefitFlag;
                entity.RiskQuarter = bo.RiskQuarter;
                entity.ProcessingDate = bo.ProcessingDate;
                entity.UniqueKeyPerLife = bo.UniqueKeyPerLife;
                entity.RetroBenefitCode = bo.RetroBenefitCode;
                entity.RetroRatio = bo.RetroRatio;
                entity.AccumulativeRetainAmount = bo.AccumulativeRetainAmount;
                entity.RetroRetainAmount = bo.RetroRetainAmount;
                entity.RetroAmount = bo.RetroAmount;
                entity.RetroGrossPremium = bo.RetroGrossPremium;
                entity.RetroNetPremium = bo.RetroNetPremium;
                entity.RetroDiscount = bo.RetroDiscount;
                entity.RetroExtraPremium = bo.RetroExtraPremium;
                entity.RetroExtraComm = bo.RetroExtraComm;
                entity.RetroGst = bo.RetroGst;
                entity.RetroTreaty = bo.RetroTreaty;
                entity.RetroClaimId = bo.RetroClaimId;
                entity.Soa = bo.Soa;
                entity.RetroIndicator = bo.RetroIndicator;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref PerLifeAggregatedDataBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(PerLifeAggregatedDataBo bo)
        {
            PerLifeAggregatedData.Delete(bo.Id);
        }

        public static Result Delete(PerLifeAggregatedDataBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: Add validation

            if (result.Valid)
            {
                DataTrail dataTrail = PerLifeAggregatedData.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteByPerLifeAggregationDetailId(int perLifeAggregationDetailId)
        {
            return PerLifeAggregatedData.DeleteByPerLifeAggregationDetailId(perLifeAggregationDetailId);
        }

        public static void DeleteByPerLifeAggregationDetailId(int perLifeAggregationDetailId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteByPerLifeAggregationDetailId(perLifeAggregationDetailId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(PerLifeAggregatedData)));
                }
            }
        }
    }
}
