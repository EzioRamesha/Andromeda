using BusinessObject.Identity;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessObject
{
    public class ModuleBo
    {
        public int Id { get; set; }

        public int DepartmentId { get; set; }

        public int Type { get; set; }

        public string Controller { get; set; }

        public string Power { get; set; }

        public string PowerAdditional { get; set; }
        public List<string> PowerAdditionals { get; set; }

        public bool Editable { get; set; }

        public string Name { get; set; }

        public string ReportPath { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public int Index { get; set; }

        public AccessMatrixBo AccessMatrixBo { get; set; }

        public DepartmentBo DepartmentBo { get; set; }

        public bool HideParameters { get; set; }

        public enum ModuleController
        {
            AccessGroup,
            AccessMatrix,
            AuditTrailReport,
            Benefit,
            Cedant,
            Department,
            Document,
            Export,
            Mfrs17CellMapping,
            Mfrs17CellMappingDetail,
            Mfrs17Reporting,
            Mfrs17ReportingDetail,
            Mfrs17ReportingDetailRiData,
            Module,
            PickList,
            PickListDetail,
            RateTableMapping,
            RateTableDetail,
            RawFile,
            Remark,
            RiData,
            RiDataBatch,
            RiDataBatchStatusFile,
            RiDataComputation,
            RiDataConfig,
            RiDataCorrection,
            RiDataFile,
            RiDataMapping,
            RiDataMappingDetail,
            RiDataPreValidation,
            RiDataPostValidation,
            RiDataPostComputation,
            RiDataReport,
            Roles,
            StandardOutput,
            StatusHistory,
            Treaty,
            ProductFeatureMapping,
            TreatyBenefitCodeMappingDetail,
            TreatyCode,
            UserAccessGroup,
            UserClaim,
            UserLogin,
            UserPassword,
            UserRoles,
            User,
            UserReport,
            ClaimCode,
            ClaimCodeMapping,
            ClaimDataConfig,
            EventClaimCodeMapping,
            AccountCode,
            AccountCodeMapping,
            AccountCodeMappingDetail,
            ClaimDataMapping,
            ClaimDataMappingDetail,
            ClaimDataComputation,
            ClaimDataPreValidation,
            ClaimDataPostComputation,
            ClaimDataPostValidation,
            Salutation,
            RateTable,
            RateDetail,
            ItemCode,
            ItemCodeMapping,
            ItemCodeMappingDetail,
            AuthorizationLimit,
            DiscountTable,
            RiDiscount,
            LargeDiscount,
            GroupDiscount,
            ClaimData,
            ClaimDataBatch,
            ClaimDataFile,
            ClaimDataBatchStatusFile,
            AnnuityFactor,
            AnnuityFactorDetail,
            AnnuityFactorMapping,
            EventCode,
            FacMasterListing,
            FacMasterListingDetail,
            CedantWorkgroup,
            CedantWorkgroupCedant,
            CedantWorkgroupUser,
            ClaimAuthorityLimitCedant,
            ClaimAuthorityLimitMLRe,
            ClaimChecklist,
            ClaimChecklistDetail,
            ClaimCategory,
            StandardClaimDataOutput,
            ClaimReason,
            PublicHoliday,
            ClaimRegister,
            SoaData,
            SoaDataBatch,
            SoaDataBatchStatusFile,
            SoaDataFile,
            StandardSoaDataOutput,
            RetroParty,
            RiDataWarehouse,
            PremiumSpreadTable,
            PremiumSpreadTableDetail,
            TreatyDiscountTable,
            TreatyDiscountTableDetail,
            DirectRetroConfiguration,
            DirectRetroConfigurationMapping,
            DirectRetroConfigurationDetail,
            InvoiceRegister,
            InvoiceRegisterValuation,
            InvoiceRegisterBatch,
            InvoiceRegisterBatchSoaData,
            InvoiceRegisterBatchRemark,
            InvoiceRegisterBatchRemarkDocument,
            RiDataSearch,
            ClaimRegisterSearch,
            DirectRetro,
            RetroSummary,
            ClaimRegisterClaim,
            DirectRetroStatusFile,
            RetroStatement,
            ReferralClaim,
            RetroRegister,
            RetroRegisterBatch,
            ClaimDashboard,
            CutOff,
            OperationDashboard,
            FinanceProvisioningTransaction,
            DirectRetroProvisioningTransaction,
            UnderwritingRemark,
            FinanceProvisioning,
            ClaimRegisterFile,
            ReferralRiData,
            SanctionKeyword,
            SanctionKeywordDetail,
            SanctionExclusion,
            Source,
            SanctionBatch,
            Sanction,
            SanctionName,
            SanctionVerification,
            SanctionAddress,
            SanctionBirthDate,
            SanctionComment,
            SanctionCountry,
            SanctionIdentity,
            SanctionVerificationDetail,
            SanctionFormatName,
            SanctionEnquiry,
            ClaimsAbove1MilReport,
            ClaimsAmountCountReport,
            ClaimsApprovedNotOffsetReport,
            ClaimsChangelogReport,
            ClaimsDeclineReport,
            ClaimsPendingReport,
            ClaimsQARReport,
            ClaimsReport,
            ClaimsReferralReport,
            ClaimsReversalReport,
            ClaimsTrendReport,
            ReferralReasonsTATReport,
            ExactMatchReport,
            MarketingAllowanceProvisionsReport,
            WOPClaimsReport,
            APPolicyCountTrackingReport,
            InvoiceRegisterForSSTSubmissionOnServiceFeeReport,
            InvoiceRegisterReport,
            RetroRegisterReport,
            RetroPremiumProvisionReport,
            TotalBookedPremiumAsAtQuarterClosingReport,
            TotalInHandBalanceAsAtQuarterClosingReport,
            ExperienceSummaryReport,
            PremiumProjectionReport,
            PremiumInfoInvoiceRegisterReport,
            PremiumInfoSoaDataRiSummaryReport,
            PremiumInfoRetroRegisterReport,
            PoliciesInfoReport,
            WeeklyReport,
            ComplianceRiskDashboard,
            ObjectPermission,
            RemarkFollowUp,
            RetroAccountCode,
            RetroAccountCodeMapping,
            Email,
            SanctionWhitelist,
            SanctionBlacklist,
            PremiumProjectionSnapshotReport,
            PremiumInfoInvoiceRegisterSnapshotReport,
            PremiumInfoSoaDataRiSummarySnapshotReport,
            PremiumInfoRetroRegisterSnapshotReport,
            PoliciesInfoTerminatedPoliciesOnDeathClaimsReport,
            PoliciesInfoSurrenderedPoliciesReport,
            PoliciesInfoNetPremiumByFundCodeReport,
            InvoiceRegisterBatchStatusFile,
            RetroRegisterBatchStatusFile,
            PoliciesInfoNetPremiumByFundCodeSoaDataReport,
            Mfrs17ContractCode,
            Mfrs17ContractCodeDetail,
            UwQuestionnaireCategory,
            HipsCategory,
            PremiumInfoByMfrs17CellNameReport,
            PremiumInfoByMfrs17CellNameSnapshotReport,
            HipsCategoryDetail,
            Designation,
            InsuredGroupName,
            Template,
            TemplateDetail,
            TreatyPricingCedant,
            TreatyPricingRateTableGroup,
            TreatyPricingRateTable,
            TreatyPricingClaimApprovalLimit,
            TreatyPricingClaimApprovalLimitVersion,
            TreatyPricingUwQuestionnaire,
            TreatyPricingUwQuestionnaireVersion,
            TreatyPricingUwQuestionnaireVersionDetail,
            TreatyPricingUwQuestionnaireVersionFile,
            TreatyPricingProduct,
            TreatyPricingRateTableVersion,
            TreatyPricingRateTableDetail,
            TreatyPricingRateTableRate,
            TreatyPricingRateTableOriginalRate,
            TreatyPricingUwLimit,
            TreatyPricingUwLimitVersion,
            TreatyPricingProductVersion,
            TreatyPricingDefinitionAndExclusion,
            TreatyPricingDefinitionAndExclusionVersion,
            TreatyPricingCustomOther,
            TreatyPricingCustomOtherVersion,
            TreatyPricingAdvantageProgram,
            TreatyPricingAdvantageProgramVersion,
            TreatyPricingAdvantageProgramVersionBenefit,
            TreatyPricingMedicalTable,
            TreatyPricingMedicalTableVersion,
            TreatyPricingMedicalTableVersionDetail,
            TreatyPricingMedicalTableVersionFile,
            TreatyPricingFinancialTable,
            TreatyPricingFinancialTableVersion,
            TreatyPricingFinancialTableVersionDetail,
            TreatyPricingFinancialTableVersionFile,
            TreatyPricingProductDetail,
            TreatyPricingProfitCommission,
            TreatyPricingProfitCommissionVersion,
            TreatyPricingTierProfitCommission,
            TreatyPricingProfitCommissionDetail,
            TreatyPricingMedicalTableUploadCell,
            TreatyPricingMedicalTableUploadColumn,
            TreatyPricingMedicalTableUploadLegend,
            TreatyPricingMedicalTableUploadRow,
            TreatyPricingFinancialTableUpload,
            TreatyPricingFinancialTableUploadLegend,
            TreatyPricingProductBenefit,
            TreatyPricingCampaign,
            TreatyPricingCampaignVersion,
            TreatyPricingQuotationWorkflow,
            TreatyPricingQuotationWorkflowVersion,
            TreatyPricingWorkflowObject,
            TreatyPricingQuotationWorkflowVersionQuotationChecklist,
            TreatyPricingQuotationWorkflowQuotation,
            TreatyPricingQuotationWorkflowPricing,
            TreatyPricingTreatyWorkflow,
            TreatyPricingTreatyWorkflowVersion,
            TreatyPricingPerLifeRetro,
            TreatyPricingPerLifeRetroVersion,
            TreatyPricingPerLifeRetroVersionBenefit,
            TreatyPricingPerLifeRetroVersionDetail,
            TreatyPricingPerLifeRetroVersionTier,
            TreatyPricingProductBenefitDirectRetro,
            TreatyPricingQuotationDashboard,
            TreatyPricingPricingDashboard,
            TreatyPricingTreatyDashboard,
            TreatyPricingGroupDashboard,
            TreatyPricingInternalDashboard,
            TreatyPricingGroupReferral,
            TreatyPricingGroupReferralVersion,
            TreatyPricingGroupReferralVersionBenefit,
            TreatyPricingProductPerLifeRetro,
            RateComparisonReport,
            RateComparisonPaReport,
            RetroBenefitCode,
            RetroBenefitRetentionLimit,
            RetroBenefitRetentionLimitDetail,
            TreatyPricingGroupReferralFile,
            TreatyPricingReportGeneration,
            DefinitionAndExclusionComparisonReport,
            UwLimitComparisonReport,
            RetroBenefitCodeMapping,
            RetroBenefitCodeMappingDetail,
            TreatyPricingGroupReferralChecklist,
            PerLifeDuplicationCheck,
            PerLifeRetroGender,
            PerLifeRetroCountry,
            GstMaintenance,
            MedicalTableComparisonReport,
            NonMedicalTableComparisonReport,
            FinancialTableComparisonReport,
            RetroTreaty,
            PerLifeRetroConfiguration,
            ValidDuplicationList,
            PerLifeRetroConfigurationTreaty,
            PerLifeRetroConfigurationRatio,
            RetroTreatyDetail,
            QuotationReport,
            QuotationSummaryReport,
            BDWeeklyReport,
            TreatyPricingGroupMasterLetter,
            TreatyPricingGroupMasterLetterGroupReferral,
            DraftStatusOverviewByBusinessOriginReport,
            DraftStatusOverviewByRetroPartyReport,
            TreatyWeeklyMonthlyQuarterlyReport,
            KPIMonitoringReport,
            KPIMonitoringForPricingReport,
            UwQuestionnaireComparisonReport,
            GroupOverallTatReport,
            ProductAndBenefitDetailsReport,
            ProductComparisonReport,
            AdvantageProgramComparisonReport,
            HipsComparisonReport,
            GtlRatesByUnitRateReport,
            CampaignComparisonReport,
            GtlRatesByAgeBandedReport,
            PerLifeAggregation,
            PerLifeRetroReport,
            GroupReferralReport,
            ProfitCommissionReport,
            PerLifeDataCorrection,
            GHSClaimExperienceReport,
            GTLClaimExperienceReport,
            PerLifeAggregationDetail,
            PerLifeAggregationDetailData,
            PerLifeAggregationDetailTreaty,
            PerLifeAggregationDuplicationListing,
            PerLifeAggregationConflictListing,
            TargetPlanningStatementTrackingReport,
            TargetPlanningPCStatementTrackingReport,
            TreatyPricingGroupReferralChecklistDetail,
            PerLifeDuplicationCheckDetail,
            GroupAuthorityLimitReport,
            GroupAuthorityLimitListing,
            StandardRetroOutput,
            PerLifeAggregationMonthlyData,
            PerLifeAggregatedData,
            PerLifeAggregationMonthlyRetroData,
            RetroBenefitCodeMappingTreaty,
            PerLifeSoa,
            PerLifeClaim,
            PerLifeClaimData,
            PerLifeClaimRetroData,
            PerLifeSoaData,
            PendingClaimsReport,
            PaidClaimsReport,
            SummaryOfExclusionReport,
            SummaryRetroPremiumByTreatyReport,
            RetrocessionDashboard,
            PerLifeRetroStatement,
            OverallRetroPremiumAndClaimsSummaryReport,
            RetroProcessedDataReport,
            PerLifeSoaSummaries,
            PerLifeSoaSummariesByTreaty,
            SummaryValuationReport,
            PerLifeSoaSummariesSoa,
            ObjectLock,
            GTLBasisOfSA,
            TreatyBenefitCodeMappingUpload,
            RateTableMappingUpload,
            FacMasterListingUpload,
            RateDetailUpload,
        }

        public const int TypeAction = 1;
        public const int TypeReport = 2;
        public const int TypeMaintenance = 3;
        public const int TypeGroupMaintenance = 4;
        public const int TypeRepository = 5;
        public const int TypeWorkflow = 6;
        public const int TypeDashboard = 7;
        public const int TypeGroupBusiness = 8;
        public const int TypeGroupReport = 9;
        public const int TypePerLifeRetro = 10;
        public const int TypePerLifeAggregation = 11;
        public const int TypeTargetPlanningReport = 12;
        public const int TypeSubModule = 13;

        public const int TypeMax = 13;

        public static char Delimiter = ',';
        public static string DefaultDelimiter = string.Format("{0}", Delimiter);
        public static string DefaultPower = string.Join(DefaultDelimiter, AccessMatrixBo.DefaultPowers);

        public ModuleBo()
        {
            Type = TypeAction;
            Editable = false;
        }

        public ModuleBo(string controller) : this()
        {
            Controller = controller;
            Name = controller.ToProperCase();
        }

        public ModuleBo(string controller, int type) : this(controller)
        {
            Type = type;

            Power = GetPowerByType(type);
        }

        public ModuleBo(string controller, int type, int departmentId) : this(controller, type)
        {
            DepartmentId = departmentId;
        }

        public static IList<ModuleBo> GetActionModules()
        {
            return new List<ModuleBo>
            {
                new ModuleBo(ModuleController.Mfrs17Reporting.ToString(), TypeAction, DepartmentBo.DepartmentValuation)
                {
                    Name = "MFRS17 Reporting", Index = 1
                },


                (new ModuleBo(ModuleController.ClaimDashboard.ToString(), TypeAction, DepartmentBo.DepartmentClaim)
                {
                    Name = "Operational Dashboard",
                    Power = AccessMatrixBo.AccessMatrixCRUD.R.ToString(),
                    PowerAdditionals = new List<string>
                    {
                        AccessMatrixBo.PowerManagerClaimDashboard,
                        AccessMatrixBo.PowerIndividualClaimDashboard,
                    },
                    Index = 1
                }).SetPowerAdditionals(),
                (new ModuleBo(ModuleController.ClaimRegisterClaim.ToString(), TypeAction, DepartmentBo.DepartmentClaim)
                {
                    Name = "Claim Register", Index = 2,
                    PowerAdditionals = new List<string>
                    {
                        AccessMatrixBo.PowerCeoApproval,
                        AccessMatrixBo.PowerApprovalOnBehalfCeo,
                        AccessMatrixBo.PowerAssignClaim,
                        AccessMatrixBo.PowerUnderwritingFeedback,
                    },
                }).SetPowerAdditionals(),
                (new ModuleBo(ModuleController.ReferralClaim.ToString(), TypeAction, DepartmentBo.DepartmentClaim)
                {
                    PowerAdditionals = new List<string>
                    {
                        AccessMatrixBo.PowerUpload,
                    },
                    Index = 3
                }).SetPowerAdditionals(),
                new ModuleBo(ModuleController.OperationDashboard.ToString(), TypeAction, DepartmentBo.DepartmentDataAnalyticsAdministration)
                {
                    Name = "Operational Dashboard",
                    Power = string.Format("{0},{1}", AccessMatrixBo.AccessMatrixCRUD.R.ToString(), AccessMatrixBo.AccessMatrixCRUD.U.ToString()),
                    Index = 1,
                },
                new ModuleBo(ModuleController.RiData.ToString(), TypeAction, DepartmentBo.DepartmentDataAnalyticsAdministration)
                {
                    Name = "RI Data", Index = 2
                },
                new ModuleBo(ModuleController.ClaimData.ToString(), TypeAction, DepartmentBo.DepartmentDataAnalyticsAdministration) { Index = 3 },
                new ModuleBo(ModuleController.SoaData.ToString(), TypeAction, DepartmentBo.DepartmentDataAnalyticsAdministration)
                {
                    Name = "SOA Data", Index = 4
                },
                new ModuleBo(ModuleController.RiDataWarehouse.ToString(), TypeAction, DepartmentBo.DepartmentDataAnalyticsAdministration)
                {
                    Name = "RI Data Warehouse", Index = 5
                },
                (new ModuleBo(ModuleController.ClaimRegister.ToString(), TypeAction, DepartmentBo.DepartmentDataAnalyticsAdministration)
                {
                    PowerAdditionals = new List<string>
                    {
                        AccessMatrixBo.PowerUpload,
                    },
                    Index = 6
                }).SetPowerAdditionals(),
                new ModuleBo(ModuleController.InvoiceRegister.ToString(), TypeAction, DepartmentBo.DepartmentDataAnalyticsAdministration) { Index = 7 },
                (new ModuleBo(ModuleController.FacMasterListing.ToString(), TypeAction, DepartmentBo.DepartmentDataAnalyticsAdministration)
                {
                    PowerAdditionals = new List<string>
                    {
                        AccessMatrixBo.PowerUpload,
                    },
                    Name = "FAC Master Listing", Index = 8
                }).SetPowerAdditionals(),
                new ModuleBo(ModuleController.RiDataSearch.ToString(), TypeAction, DepartmentBo.DepartmentDataAnalyticsAdministration)
                {
                    Name = "RI Data Search",
                    Power = AccessMatrixBo.AccessMatrixCRUD.R.ToString(),
                    Index = 9
                },
                new ModuleBo(ModuleController.ClaimRegisterSearch.ToString(), TypeAction, DepartmentBo.DepartmentDataAnalyticsAdministration)
                {
                    Power = AccessMatrixBo.AccessMatrixCRUD.R.ToString(),
                    Index = 10
                },

                new ModuleBo(ModuleController.RetrocessionDashboard.ToString(), TypeAction, DepartmentBo.DepartmentRetrocession)
                {
                    Name = "Operational Dashboard", Index = 1,
                },
                (new ModuleBo(ModuleController.DirectRetro.ToString(), TypeAction, DepartmentBo.DepartmentRetrocession)
                {
                    PowerAdditionals = new List<string>
                    {
                        AccessMatrixBo.PowerApprovalDirectRetro,
                    },
                    Index = 2
                }).SetPowerAdditionals(),
                (new ModuleBo(ModuleController.RetroRegister.ToString(), TypeAction, DepartmentBo.DepartmentRetrocession)
                {
                    PowerAdditionals = new List<string>
                    {
                        AccessMatrixBo.PowerApprovalRetroRegister,
                    },
                    Index = 3
                }).SetPowerAdditionals(),

                new ModuleBo(ModuleController.FinanceProvisioning.ToString(), TypeAction, DepartmentBo.DepartmentFinance) { Index = 1 },

                new ModuleBo(ModuleController.ComplianceRiskDashboard.ToString(), TypeAction, DepartmentBo.DepartmentComplianceRisk)
                {
                    Name = "Dashboard",
                    Power = AccessMatrixBo.AccessMatrixCRUD.R.ToString(),
                    Index = 1
                },
                new ModuleBo(ModuleController.SanctionBatch.ToString(), TypeAction, DepartmentBo.DepartmentComplianceRisk)
                {
                    Name = "Sanction Upload", Index = 2
                },
                new ModuleBo(ModuleController.SanctionVerification.ToString(), TypeAction, DepartmentBo.DepartmentComplianceRisk) { Index = 3 },
                new ModuleBo(ModuleController.SanctionEnquiry.ToString(), TypeAction, DepartmentBo.DepartmentComplianceRisk) { Index = 4 },
            };
        }

        public static IList<ModuleBo> GetMaintenanceModules()
        {
            return new List<ModuleBo>
            {
                new ModuleBo(ModuleController.AccessGroup.ToString(), TypeMaintenance, DepartmentBo.DepartmentIT) { Index = 1 },
                new ModuleBo(ModuleController.Department.ToString(), TypeMaintenance, DepartmentBo.DepartmentIT) { Index = 2 },
                new ModuleBo(ModuleController.Module.ToString(), TypeMaintenance, DepartmentBo.DepartmentIT) { Index = 3 },
                new ModuleBo(ModuleController.User.ToString(), TypeMaintenance, DepartmentBo.DepartmentIT)
                {
                    PowerAdditional = AccessMatrixBo.PowerInactiveUserReport, Index = 4
                },
                //new ModuleBo(ModuleController.StandardOutput.ToString(), TypeMaintenance, DepartmentBo.DepartmentIT),
                new ModuleBo(ModuleController.PublicHoliday.ToString(), TypeMaintenance, DepartmentBo.DepartmentIT) { Index = 5 },


                new ModuleBo(ModuleController.Cedant.ToString(), TypeMaintenance, DepartmentBo.DepartmentShared) { Index = 1 },
                new ModuleBo(ModuleController.Treaty.ToString(), TypeMaintenance, DepartmentBo.DepartmentShared) { Index = 2 },
                new ModuleBo(ModuleController.PickList.ToString(), TypeMaintenance, DepartmentBo.DepartmentShared)
                {
                    Power = string.Format("{0},{1}", AccessMatrixBo.AccessMatrixCRUD.R.ToString(), AccessMatrixBo.AccessMatrixCRUD.U.ToString()),
                    //PowerAdditional = AccessMatrixBo.PowerAddDepartment, Index = 3
                },


                new ModuleBo(ModuleController.ClaimAuthorityLimitCedant.ToString(), TypeMaintenance, DepartmentBo.DepartmentClaim)
                {
                    Name = "Claim Authority Limit - Cedant", Index = 1
                },
                new ModuleBo(ModuleController.ClaimAuthorityLimitMLRe.ToString(), TypeMaintenance, DepartmentBo.DepartmentClaim)
                {
                    Name = "Claim Authority Limit - MLRe", Index = 2
                },
                new ModuleBo(ModuleController.ClaimChecklist.ToString(), TypeMaintenance, DepartmentBo.DepartmentClaim) { Index = 3 },
                new ModuleBo(ModuleController.ClaimCategory.ToString(), TypeMaintenance, DepartmentBo.DepartmentClaim) { Index = 4 },
                new ModuleBo(ModuleController.ClaimReason.ToString(), TypeMaintenance, DepartmentBo.DepartmentClaim) { Index = 5 },


                new ModuleBo(ModuleController.PerLifeDuplicationCheck.ToString(), TypeMaintenance, DepartmentBo.DepartmentRetrocession) { Index = 1 },
                new ModuleBo(ModuleController.ValidDuplicationList.ToString(), TypeMaintenance, DepartmentBo.DepartmentRetrocession) { Index = 2 },
                new ModuleBo(ModuleController.PerLifeDataCorrection.ToString(), TypeMaintenance, DepartmentBo.DepartmentRetrocession) { Index = 3 },
                new ModuleBo(ModuleController.RetroParty.ToString(), TypeMaintenance, DepartmentBo.DepartmentRetrocession) { Index = 4 },
                (new ModuleBo(ModuleController.RetroTreaty.ToString(), TypeMaintenance, DepartmentBo.DepartmentRetrocession)
                {
                    PowerAdditionals = new List<string>
                    {
                        AccessMatrixBo.PowerUpload,
                    },
                    Index = 5
                }).SetPowerAdditionals(),
                new ModuleBo(ModuleController.DirectRetroConfiguration.ToString(), TypeMaintenance, DepartmentBo.DepartmentRetrocession) { Index = 6 },
                new ModuleBo(ModuleController.PerLifeRetroConfiguration.ToString(), TypeMaintenance, DepartmentBo.DepartmentRetrocession) { Index = 7 },
                new ModuleBo(ModuleController.RetroBenefitRetentionLimit.ToString(), TypeMaintenance, DepartmentBo.DepartmentRetrocession) {
                    Name = "Retention Limit by Retro Benefit", Index = 8
                },
                new ModuleBo(ModuleController.RetroBenefitCodeMapping.ToString(), TypeMaintenance, DepartmentBo.DepartmentRetrocession) { Index = 9 },
                new ModuleBo(ModuleController.RetroBenefitCode.ToString(), TypeMaintenance, DepartmentBo.DepartmentRetrocession) { Index = 10 },
                (new ModuleBo(ModuleController.PremiumSpreadTable.ToString(), TypeMaintenance, DepartmentBo.DepartmentRetrocession)
                {
                    PowerAdditionals = new List<string>
                    {
                        AccessMatrixBo.PowerUpload,
                    },
                    Index = 11
                }).SetPowerAdditionals(),
                (new ModuleBo(ModuleController.TreatyDiscountTable.ToString(), TypeMaintenance, DepartmentBo.DepartmentRetrocession)
                {
                    PowerAdditionals = new List<string>
                    {
                        AccessMatrixBo.PowerUpload,
                    },
                    Index = 12
                }).SetPowerAdditionals(),
                new ModuleBo(ModuleController.GstMaintenance.ToString(), TypeMaintenance, DepartmentBo.DepartmentRetrocession)
                {
                    Name = "GST Maintenance",
                    Index = 13
                },
                new ModuleBo(ModuleController.PerLifeRetroGender.ToString(), TypeMaintenance, DepartmentBo.DepartmentRetrocession) { Index = 14 },
                new ModuleBo(ModuleController.PerLifeRetroCountry.ToString(), TypeMaintenance, DepartmentBo.DepartmentRetrocession) { Index = 15 },

                new ModuleBo(ModuleController.RiDataConfig.ToString(), TypeMaintenance, DepartmentBo.DepartmentDataAnalyticsAdministration)
                {
                    Name = "RI Data Config", PowerAdditional = AccessMatrixBo.PowerApproval, Index = 1
                },
                new ModuleBo(ModuleController.ClaimDataConfig.ToString(), TypeMaintenance, DepartmentBo.DepartmentDataAnalyticsAdministration)
                {
                    PowerAdditional = AccessMatrixBo.PowerApproval, Index = 2
                },
                new ModuleBo(ModuleController.ClaimCode.ToString(), TypeMaintenance, DepartmentBo.DepartmentDataAnalyticsAdministration) { Index = 3 },
                //new ModuleBo(ModuleController.ClaimCodeMapping.ToString(), TypeMaintenance, DepartmentBo.DepartmentDataAnalyticsAdministration), //Remove from Menu
                new ModuleBo(ModuleController.EventCode.ToString(), TypeMaintenance, DepartmentBo.DepartmentDataAnalyticsAdministration)
                {
                    Name = "MLRe Event Code", Index = 4
                },
                (new ModuleBo(ModuleController.EventClaimCodeMapping.ToString(), TypeMaintenance, DepartmentBo.DepartmentDataAnalyticsAdministration)
                {
                    PowerAdditionals = new List<string>
                    {
                        AccessMatrixBo.PowerUpload,
                    },
                    Name = "MLRe Event Code Mapping", Index = 5
                }).SetPowerAdditionals(),
                (new ModuleBo(ModuleController.Benefit.ToString(), TypeMaintenance, DepartmentBo.DepartmentDataAnalyticsAdministration)
                {
                     PowerAdditionals = new List<string>
                    {
                        AccessMatrixBo.PowerUpload,
                    },
                    Name = "MLRe Benefit Code", Index = 6
                }).SetPowerAdditionals(),
                (new ModuleBo(ModuleController.ProductFeatureMapping.ToString(), TypeMaintenance, DepartmentBo.DepartmentDataAnalyticsAdministration)
                {
                    PowerAdditionals = new List<string>
                    {
                        AccessMatrixBo.PowerUpload,
                    },
                    Index = 7
                }).SetPowerAdditionals(),
                new ModuleBo(ModuleController.DiscountTable.ToString(), TypeMaintenance, DepartmentBo.DepartmentDataAnalyticsAdministration) { Index = 8 },
                new ModuleBo(ModuleController.AnnuityFactor.ToString(), TypeMaintenance, DepartmentBo.DepartmentDataAnalyticsAdministration) { Index = 9 },
                new ModuleBo(ModuleController.RateTable.ToString(), TypeMaintenance, DepartmentBo.DepartmentDataAnalyticsAdministration) { Index = 10 },
                (new ModuleBo(ModuleController.RateTableMapping.ToString(), TypeMaintenance, DepartmentBo.DepartmentDataAnalyticsAdministration)
                {
                    PowerAdditionals = new List<string>
                    {
                        AccessMatrixBo.PowerUpload,
                    },
                    Index = 11
                }).SetPowerAdditionals(),
                new ModuleBo(ModuleController.Salutation.ToString(), TypeMaintenance, DepartmentBo.DepartmentDataAnalyticsAdministration) { Index = 12 },
                (new ModuleBo(ModuleController.RiDataCorrection.ToString(), TypeMaintenance, DepartmentBo.DepartmentDataAnalyticsAdministration)
                {
                    PowerAdditionals = new List<string>
                    {
                        AccessMatrixBo.PowerUpload,
                    },
                    Name = "RI Data Correction", Index = 13
                }).SetPowerAdditionals(),
                new ModuleBo(ModuleController.AuthorizationLimit.ToString(), TypeMaintenance, DepartmentBo.DepartmentDataAnalyticsAdministration) { Index = 14 },
                new ModuleBo(ModuleController.CedantWorkgroup.ToString(), TypeMaintenance, DepartmentBo.DepartmentDataAnalyticsAdministration) { Index = 15 },


                (new ModuleBo(ModuleController.Mfrs17CellMapping.ToString(), TypeMaintenance, DepartmentBo.DepartmentValuation)
                {
                    PowerAdditionals = new List<string>
                    {
                        AccessMatrixBo.PowerUpload,
                    },
                    Name = "MFRS17 Cell Mapping", Index = 1
                }).SetPowerAdditionals(),
                (new ModuleBo(ModuleController.Mfrs17ContractCode.ToString(), TypeMaintenance,DepartmentBo.DepartmentValuation)
                {
                    PowerAdditionals = new List<string>
                    {
                        AccessMatrixBo.PowerUpload,
                    },
                    Name = "MFRS17 Contract Code",
                    Index = 2
                }).SetPowerAdditionals(),

                new ModuleBo(ModuleController.SanctionKeyword.ToString(), TypeMaintenance, DepartmentBo.DepartmentComplianceRisk) { Index = 1 },
                new ModuleBo(ModuleController.SanctionExclusion.ToString(), TypeMaintenance, DepartmentBo.DepartmentComplianceRisk) { Index = 2 },
                new ModuleBo(ModuleController.Source.ToString(), TypeMaintenance, DepartmentBo.DepartmentComplianceRisk) { Index = 3 },

                new ModuleBo(ModuleController.ItemCode.ToString(), TypeMaintenance, DepartmentBo.DepartmentFinance) { Index = 1 },
                (new ModuleBo(ModuleController.ItemCodeMapping.ToString(), TypeMaintenance, DepartmentBo.DepartmentFinance)
                {
                    PowerAdditionals = new List<string>
                    {
                        AccessMatrixBo.PowerUpload,
                    },
                    Index = 2
                }).SetPowerAdditionals(),
                new ModuleBo(ModuleController.AccountCode.ToString(), TypeMaintenance, DepartmentBo.DepartmentFinance)
                {
                    Name = "Account Code - Invoice, Provision & Recovery", Index = 3
                },
                new ModuleBo(ModuleController.RetroAccountCode.ToString(), TypeMaintenance, DepartmentBo.DepartmentFinance)
                {
                    Name = "Account Code - Retro", Index = 4
                },
                (new ModuleBo(ModuleController.AccountCodeMapping.ToString(), TypeMaintenance, DepartmentBo.DepartmentFinance)
                {
                    PowerAdditionals = new List<string>
                    {
                        AccessMatrixBo.PowerUpload,
                    },
                    Name = "Account Code Mapping - Invoice, Provision & Recovery", Index = 5
                }).SetPowerAdditionals(),
                (new ModuleBo(ModuleController.RetroAccountCodeMapping.ToString(), TypeMaintenance, DepartmentBo.DepartmentFinance)
                {
                    PowerAdditionals = new List<string>
                    {
                        AccessMatrixBo.PowerUpload,
                    },
                    Name = "Account Code Mapping - Retro", Index = 6
                }).SetPowerAdditionals(),
                new ModuleBo(ModuleController.Template.ToString(), TypeMaintenance, DepartmentBo.DepartmentTreatyPricing) { Index = 2 },
                new ModuleBo(ModuleController.UwQuestionnaireCategory.ToString(), TypeMaintenance, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "UW Questionnaire Category",
                    Index = 3
                },
            };
        }

        public static IList<ModuleBo> GetGroupMaintenanceModules()
        {
            return new List<ModuleBo>
            {
                new ModuleBo(ModuleController.HipsCategory.ToString(), TypeGroupMaintenance, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "HIPS Category",
                    Index = 1
                },
                new ModuleBo(ModuleController.InsuredGroupName.ToString(), TypeGroupMaintenance, DepartmentBo.DepartmentTreatyPricing) { Index = 3 },
            };
        }

        public static IList<ModuleBo> GetReportModules()
        {
            return new List<ModuleBo>
            {
                #region IT
                new ModuleBo(ModuleController.UserReport.ToString(), TypeReport, DepartmentBo.DepartmentIT) {
                    Editable = true, ReportPath = ModuleController.UserReport.ToString()
                },
                new ModuleBo(ModuleController.AuditTrailReport.ToString(), TypeReport, DepartmentBo.DepartmentIT) {
                    Editable = true, ReportPath = ModuleController.AuditTrailReport.ToString()
                },
                #endregion

                #region DA&A
                new ModuleBo(ModuleController.RiDataReport.ToString(), TypeReport, DepartmentBo.DepartmentDataAnalyticsAdministration) {
                    Name = "RI Data Report", Editable = true, ReportPath = ModuleController.RiDataReport.ToString(), Index = 1
                },
                 new ModuleBo(ModuleController.MarketingAllowanceProvisionsReport.ToString(), TypeReport, DepartmentBo.DepartmentDataAnalyticsAdministration) {
                    Editable = true, ReportPath = ModuleController.MarketingAllowanceProvisionsReport.ToString(), Index = 2
                },
                new ModuleBo(ModuleController.WOPClaimsReport.ToString(), TypeReport, DepartmentBo.DepartmentDataAnalyticsAdministration) {
                    Name = "Waiver of Premium (WOP) Claims Report", Editable = true, ReportPath = ModuleController.WOPClaimsReport.ToString(), Index = 3
                },
                new ModuleBo(ModuleController.APPolicyCountTrackingReport.ToString(), TypeReport, DepartmentBo.DepartmentDataAnalyticsAdministration) {
                    Name = "Advantage Program (AP) Policy Count Tracking Report", Editable = true, ReportPath = ModuleController.APPolicyCountTrackingReport.ToString(), Index = 4
                },
                new ModuleBo(ModuleController.InvoiceRegisterReport.ToString(), TypeReport, DepartmentBo.DepartmentDataAnalyticsAdministration) {
                    Editable = true, ReportPath = ModuleController.InvoiceRegisterReport.ToString(), Index = 5
                },
                new ModuleBo(ModuleController.RetroRegisterReport.ToString(), TypeReport, DepartmentBo.DepartmentDataAnalyticsAdministration) {
                    Editable = true, ReportPath = ModuleController.RetroRegisterReport.ToString(), Index = 6
                },
                new ModuleBo(ModuleController.InvoiceRegisterForSSTSubmissionOnServiceFeeReport.ToString(), TypeReport, DepartmentBo.DepartmentDataAnalyticsAdministration) {
                    Name = "Invoice Register for SST Submission on Service Fee Report", Editable = true, ReportPath = ModuleController.InvoiceRegisterForSSTSubmissionOnServiceFeeReport.ToString(), Index = 7
                },
                new ModuleBo(ModuleController.RetroPremiumProvisionReport.ToString(), TypeReport, DepartmentBo.DepartmentDataAnalyticsAdministration) {
                    Editable = true, ReportPath = ModuleController.RetroPremiumProvisionReport.ToString(), Index = 8
                },
                new ModuleBo(ModuleController.TotalBookedPremiumAsAtQuarterClosingReport.ToString(), TypeReport, DepartmentBo.DepartmentDataAnalyticsAdministration) {
                    Name = "Total Booked in Premium as at Quarter Closing Report", Editable = true, ReportPath = ModuleController.TotalBookedPremiumAsAtQuarterClosingReport.ToString(), Index = 9
                },
                new ModuleBo(ModuleController.TotalInHandBalanceAsAtQuarterClosingReport.ToString(), TypeReport, DepartmentBo.DepartmentDataAnalyticsAdministration) {
                    Name = "Total In-Hand Balance Report", Editable = true, ReportPath = ModuleController.TotalInHandBalanceAsAtQuarterClosingReport.ToString(), Index = 10
                },
                new ModuleBo(ModuleController.ExperienceSummaryReport.ToString(), TypeReport, DepartmentBo.DepartmentDataAnalyticsAdministration) {
                    Editable = true, ReportPath = ModuleController.ExperienceSummaryReport.ToString(), Index = 11
                },
                new ModuleBo(ModuleController.WeeklyReport.ToString(), TypeReport, DepartmentBo.DepartmentDataAnalyticsAdministration) {
                    Editable = true, ReportPath = ModuleController.WeeklyReport.ToString(), Index = 12
                },
                new ModuleBo(ModuleController.ProfitCommissionReport.ToString(), TypeReport, DepartmentBo.DepartmentDataAnalyticsAdministration) {
                    Name = "Profit Commission", Editable = true, ReportPath = ModuleController.ProfitCommissionReport.ToString(), Index = 13
                },
                new ModuleBo(ModuleController.TargetPlanningStatementTrackingReport.ToString(), TypeReport, DepartmentBo.DepartmentDataAnalyticsAdministration) {
                    Name = "Statement Tracking Report", Index = 14
                },
                new ModuleBo(ModuleController.TargetPlanningPCStatementTrackingReport.ToString(), TypeReport, DepartmentBo.DepartmentDataAnalyticsAdministration) {
                    Name = "Profit Commission Statement Tracking Report", Index = 15
                },
                #endregion

                #region Valuation
                new ModuleBo(ModuleController.PremiumProjectionReport.ToString(), TypeReport, DepartmentBo.DepartmentValuation) {
                    Editable = true, ReportPath = ModuleController.PremiumProjectionReport.ToString(), HideParameters = true, Index = 1
                },
                new ModuleBo(ModuleController.PremiumProjectionSnapshotReport.ToString(), TypeReport, DepartmentBo.DepartmentValuation) {
                    Name = "Premium Projection Report (Snapshot)", Editable = true, ReportPath = ModuleController.PremiumProjectionSnapshotReport.ToString(), HideParameters = true, Index = 2
                },
                new ModuleBo(ModuleController.PremiumInfoInvoiceRegisterReport.ToString(), TypeReport, DepartmentBo.DepartmentValuation) {
                    Name = "Premium Info (Invoice Register) Report", Editable = true, ReportPath = ModuleController.PremiumInfoInvoiceRegisterReport.ToString(), HideParameters = true, Index = 3
                },
                new ModuleBo(ModuleController.PremiumInfoInvoiceRegisterSnapshotReport.ToString(), TypeReport, DepartmentBo.DepartmentValuation) {
                    Name = "Premium Info (Invoice Register) Report (Snapshot)", Editable = true, ReportPath = ModuleController.PremiumInfoInvoiceRegisterSnapshotReport.ToString(), HideParameters = true, Index = 4
                },
                new ModuleBo(ModuleController.PremiumInfoSoaDataRiSummaryReport.ToString(), TypeReport, DepartmentBo.DepartmentValuation) {
                    Name = "Premium Info (SOA Data - RI Summary) Report", Editable = true, ReportPath = ModuleController.PremiumInfoSoaDataRiSummaryReport.ToString(), HideParameters = true, Index = 5
                },
                new ModuleBo(ModuleController.PremiumInfoSoaDataRiSummarySnapshotReport.ToString(), TypeReport, DepartmentBo.DepartmentValuation) {
                    Name = "Premium Info (SOA Data - RI Summary) Report (Snapshot)", Editable = true, ReportPath = ModuleController.PremiumInfoSoaDataRiSummarySnapshotReport.ToString(), HideParameters = true, Index = 6
                },
                new ModuleBo(ModuleController.PremiumInfoRetroRegisterReport.ToString(), TypeReport, DepartmentBo.DepartmentValuation) {
                    Name = "Premium Info (Retro Register) Report", Editable = true, ReportPath = ModuleController.PremiumInfoRetroRegisterReport.ToString(), HideParameters = true, Index = 7
                },
                new ModuleBo(ModuleController.PremiumInfoRetroRegisterSnapshotReport.ToString(), TypeReport, DepartmentBo.DepartmentValuation) {
                    Name = "Premium Info (Retro Register) Report (Snapshot)", Editable = true, ReportPath = ModuleController.PremiumInfoRetroRegisterSnapshotReport.ToString(), HideParameters = true, Index = 8
                },
                new ModuleBo(ModuleController.PoliciesInfoReport.ToString(), TypeReport, DepartmentBo.DepartmentValuation) {
                    Name = "Policies Info (In Force Policies Count & SA Covered) Report", Editable = true, ReportPath = ModuleController.PoliciesInfoReport.ToString(), Index = 9
                },
                new ModuleBo(ModuleController.PoliciesInfoTerminatedPoliciesOnDeathClaimsReport.ToString(), TypeReport, DepartmentBo.DepartmentValuation) {
                    Name = "Policies Info (Terminated Policies On Death Claims) Report", Editable = true, ReportPath = ModuleController.PoliciesInfoTerminatedPoliciesOnDeathClaimsReport.ToString(), Index = 10
                },
                new ModuleBo(ModuleController.PoliciesInfoSurrenderedPoliciesReport.ToString(), TypeReport, DepartmentBo.DepartmentValuation) {
                    Name = "Policies Info (Surrendered Policies) Report", Editable = true, ReportPath = ModuleController.PoliciesInfoSurrenderedPoliciesReport.ToString(), Index = 11
                },
                new ModuleBo(ModuleController.PoliciesInfoNetPremiumByFundCodeReport.ToString(), TypeReport, DepartmentBo.DepartmentValuation) {
                    Name = "Policies Info (RI Warehouse Net Premium By Fund Code) Report", Editable = true, ReportPath = ModuleController.PoliciesInfoNetPremiumByFundCodeReport.ToString(), Index = 12
                },
                new ModuleBo(ModuleController.PoliciesInfoNetPremiumByFundCodeSoaDataReport.ToString(), TypeReport, DepartmentBo.DepartmentValuation) {
                    Name = "Policies Info (SOA Data Net Premium By Fund Code) Report", Editable = true, ReportPath = ModuleController.PoliciesInfoNetPremiumByFundCodeSoaDataReport.ToString(), Index = 13
                },
                new ModuleBo(ModuleController.PremiumInfoByMfrs17CellNameReport.ToString(), TypeReport, DepartmentBo.DepartmentValuation) {
                    Name = "Premium Info by MFRS17 Cell Name Report", Editable = true, ReportPath = ModuleController.PremiumInfoByMfrs17CellNameReport.ToString(), HideParameters = true, Index = 14
                },
                new ModuleBo(ModuleController.PremiumInfoByMfrs17CellNameSnapshotReport.ToString(), TypeReport, DepartmentBo.DepartmentValuation) {
                    Name = "Premium Info by MFRS17 Cell Name Report (Snapshot)", Editable = true, ReportPath = ModuleController.PremiumInfoByMfrs17CellNameSnapshotReport.ToString(), HideParameters = true, Index = 15
                },
                #endregion

                #region Claims
                new ModuleBo(ModuleController.ClaimsAbove1MilReport.ToString(), TypeReport, DepartmentBo.DepartmentClaim) {
                    Name = "Claims Above RM1mil per Life Report", Editable = true, ReportPath = ModuleController.ClaimsAbove1MilReport.ToString(), Index = 1
                },
                new ModuleBo(ModuleController.ClaimsAmountCountReport.ToString(), TypeReport, DepartmentBo.DepartmentClaim) {
                    Name = "Claims Amount & Count Report", Editable = true, ReportPath = ModuleController.ClaimsAmountCountReport.ToString(), Index = 2
                },
                new ModuleBo(ModuleController.ClaimsApprovedNotOffsetReport.ToString(), TypeReport, DepartmentBo.DepartmentClaim) {
                    Name = "Claims Approved Not Offset Report", Editable = true, ReportPath = ModuleController.ClaimsApprovedNotOffsetReport.ToString(), Index = 3
                },
                new ModuleBo(ModuleController.ClaimsChangelogReport.ToString(), TypeReport, DepartmentBo.DepartmentClaim) {
                    Name = "Claims Changelog Report", Editable = true, ReportPath = ModuleController.ClaimsChangelogReport.ToString(), Index = 4
                },
                new ModuleBo(ModuleController.ClaimsDeclineReport.ToString(), TypeReport, DepartmentBo.DepartmentClaim) {
                    Name = "Claims Decline Report", Editable = true, ReportPath = ModuleController.ClaimsDeclineReport.ToString(), Index = 5
                },
                new ModuleBo(ModuleController.ClaimsPendingReport.ToString(), TypeReport, DepartmentBo.DepartmentClaim) {
                    Name = "Claims Pending Report", Editable = true, ReportPath = ModuleController.ClaimsPendingReport.ToString(), Index = 6
                },
                new ModuleBo(ModuleController.ClaimsQARReport.ToString(), TypeReport, DepartmentBo.DepartmentClaim) {
                    Name = "Claims QAR Report", Editable = true, ReportPath = ModuleController.ClaimsQARReport.ToString(), Index = 7
                },
                new ModuleBo(ModuleController.ClaimsReport.ToString(), TypeReport, DepartmentBo.DepartmentClaim) {
                    Name = "Claims Report", Editable = true, ReportPath = ModuleController.ClaimsReport.ToString(), Index = 8
                },
                new ModuleBo(ModuleController.ClaimsReferralReport.ToString(), TypeReport, DepartmentBo.DepartmentClaim) {
                    Name = "Claims Referral Report", Editable = true, ReportPath = ModuleController.ClaimsReferralReport.ToString(), Index = 9
                },
                new ModuleBo(ModuleController.ClaimsReversalReport.ToString(), TypeReport, DepartmentBo.DepartmentClaim) {
                    Name = "Claims for Reversal Report", Editable = true, ReportPath = ModuleController.ClaimsReversalReport.ToString(), Index = 10
                },
                new ModuleBo(ModuleController.ClaimsTrendReport.ToString(), TypeReport, DepartmentBo.DepartmentClaim) {
                    Name = "Claims Trend Report", Editable = true, ReportPath = ModuleController.ClaimsTrendReport.ToString(), Index = 11
                },
                new ModuleBo(ModuleController.ReferralReasonsTATReport.ToString(), TypeReport, DepartmentBo.DepartmentClaim) {
                    Name = "Referral Reasons & TAT Report", Editable = true, ReportPath = ModuleController.ReferralReasonsTATReport.ToString(), Index = 12
                },
                #endregion

                #region C&R
                new ModuleBo(ModuleController.ExactMatchReport.ToString(), TypeReport, DepartmentBo.DepartmentComplianceRisk) {
                    Name = "Exact Match Report", Editable = true, ReportPath = ModuleController.ExactMatchReport.ToString(), Index = 1
                },
                #endregion

                #region T&P
                new ModuleBo(ModuleController.UwLimitComparisonReport.ToString(), TypeReport, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Underwriting Limit Comparison", Index = 1
                },
                new ModuleBo(ModuleController.UwQuestionnaireComparisonReport.ToString(), TypeReport, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Underwriting Questionnaire Comparison", Index = 2
                },
                new ModuleBo(ModuleController.MedicalTableComparisonReport.ToString(), TypeReport, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Underwriting Medical Table Comparison", Index = 3
                },
                new ModuleBo(ModuleController.NonMedicalTableComparisonReport.ToString(), TypeReport, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Underwriting Non-Medical Limit Table Comparison", Index = 4
                },
                new ModuleBo(ModuleController.FinancialTableComparisonReport.ToString(), TypeReport, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Underwriting Financial Table Comparison", Index = 5
                },
                new ModuleBo(ModuleController.AdvantageProgramComparisonReport.ToString(), TypeReport, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Advantage Program Comparison", Index = 6
                },
                new ModuleBo(ModuleController.TreatyWeeklyMonthlyQuarterlyReport.ToString(), TypeReport, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Treaty Weekly/Monthly/Quarterly Report", Editable = true, ReportPath = ModuleController.TreatyWeeklyMonthlyQuarterlyReport.ToString(), Index = 7
                },
                new ModuleBo(ModuleController.KPIMonitoringReport.ToString(), TypeReport, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "KPI Monitoring Report", Editable = true, ReportPath = ModuleController.KPIMonitoringReport.ToString(), Index = 8
                },
                new ModuleBo(ModuleController.DraftStatusOverviewByBusinessOriginReport.ToString(), TypeReport, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Draft Status Overview (by Business Origin)", Editable = true, ReportPath = ModuleController.DraftStatusOverviewByBusinessOriginReport.ToString(), Index = 9
                },
                new ModuleBo(ModuleController.DraftStatusOverviewByRetroPartyReport.ToString(), TypeReport, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Draft Status Overview (by Retro Party)", Editable = true, ReportPath = ModuleController.DraftStatusOverviewByRetroPartyReport.ToString(), Index = 10
                },
                new ModuleBo(ModuleController.PerLifeRetroReport.ToString(), TypeReport, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Per Life Retro Party", Editable = true, ReportPath = ModuleController.PerLifeRetroReport.ToString(), Index = 11
                },
                new ModuleBo(ModuleController.KPIMonitoringForPricingReport.ToString(), TypeReport, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "KPI Monitoring For Pricing Report", Editable = true, ReportPath = ModuleController.KPIMonitoringForPricingReport.ToString(), Index = 12
                },
                new ModuleBo(ModuleController.QuotationReport.ToString(), TypeReport, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Quotation Report", Editable = true, ReportPath = ModuleController.QuotationReport.ToString(), Index = 13
                },
                new ModuleBo(ModuleController.BDWeeklyReport.ToString(), TypeReport, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "BD Weekly Report", Editable = true, ReportPath = ModuleController.BDWeeklyReport.ToString(), Index = 14
                },
                new ModuleBo(ModuleController.QuotationSummaryReport.ToString(), TypeReport, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Quotation Summary Report", Editable = true, ReportPath = ModuleController.QuotationSummaryReport.ToString(), Index = 15
                },
                new ModuleBo(ModuleController.RateComparisonReport.ToString(), TypeReport, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Rates Comparison - SA", Index = 16
                },
                new ModuleBo(ModuleController.RateComparisonPaReport.ToString(), TypeReport, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Rates Comparison - PA", Index = 17
                },
                new ModuleBo(ModuleController.ProductComparisonReport.ToString(), TypeReport, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Products & Benefits Comparison", Index = 18
                },
                new ModuleBo(ModuleController.DefinitionAndExclusionComparisonReport.ToString(), TypeReport, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Definitions & Exclusions Comparison", Index = 19
                },
                new ModuleBo(ModuleController.CampaignComparisonReport.ToString(), TypeReport, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Campaign Comparison", Index = 20
                },
                #endregion

                #region Retrocession
                new ModuleBo(ModuleController.SummaryOfExclusionReport.ToString(), TypeReport, DepartmentBo.DepartmentRetrocession) {
                    Name = "Summary of Exclusion Report", Editable = true, ReportPath = ModuleController.SummaryOfExclusionReport.ToString(), Index = 1
                },
                new ModuleBo(ModuleController.SummaryValuationReport.ToString(), TypeReport, DepartmentBo.DepartmentRetrocession) {
                    Name = "Summary Valuation", Editable = true, ReportPath = ModuleController.SummaryValuationReport.ToString(), Index = 2
                },
                new ModuleBo(ModuleController.PendingClaimsReport.ToString(), TypeReport, DepartmentBo.DepartmentRetrocession) {
                    Name = "Pending Claims", Editable = true, ReportPath = ModuleController.PendingClaimsReport.ToString(), Index = 3
                },
                new ModuleBo(ModuleController.PaidClaimsReport.ToString(), TypeReport, DepartmentBo.DepartmentRetrocession) {
                    Name = "Paid Claims", Editable = true, ReportPath = ModuleController.PaidClaimsReport.ToString(), Index = 4
                },
                new ModuleBo(ModuleController.SummaryRetroPremiumByTreatyReport.ToString(), TypeReport, DepartmentBo.DepartmentRetrocession) {
                    Name = "Summary Retro Premium by Treaty", Editable = true, ReportPath = ModuleController.SummaryRetroPremiumByTreatyReport.ToString(), Index = 5
                },
                new ModuleBo(ModuleController.RetroProcessedDataReport.ToString(), TypeReport, DepartmentBo.DepartmentRetrocession) {
                    Name = "Retro Processed Data", Editable = true, ReportPath = ModuleController.RetroProcessedDataReport.ToString(), Index = 6
                },
                new ModuleBo(ModuleController.OverallRetroPremiumAndClaimsSummaryReport.ToString(), TypeReport, DepartmentBo.DepartmentRetrocession) {
                    Name = "Overall Retro Premium and Claims Summary", Editable = true, ReportPath = ModuleController.OverallRetroPremiumAndClaimsSummaryReport.ToString(), Index = 7
                },
                #endregion
            };
        }

        public static IList<ModuleBo> GetRepositoryModules()
        {
            return new List<ModuleBo>
            {
                new ModuleBo(ModuleController.TreatyPricingCedant.ToString(), TypeRepository, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Cedant",
                    Index = 1
                },
                new ModuleBo(ModuleController.TreatyPricingPerLifeRetro.ToString(), TypeRepository, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Per Life Retro",
                    Index = 2
                },
            };
        }

        public static IList<ModuleBo> GetWorkflowModules()
        {
            return new List<ModuleBo>
            {
                (new ModuleBo(ModuleController.TreatyPricingQuotationWorkflow.ToString(), TypeWorkflow, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Quotation Workflow",
                    PowerAdditionals = new List<string>
                    {
                        AccessMatrixBo.PowerCompleteChecklist,
                        AccessMatrixBo.PowerGroupPricing,
                    },
                    Index = 1
                }).SetPowerAdditionals(),
                new ModuleBo(ModuleController.TreatyPricingTreatyWorkflow.ToString(), TypeWorkflow, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Treaty Workflow",
                    Index = 2
                },
            };
        }

        public static IList<ModuleBo> GetDashboardModules()
        {
            return new List<ModuleBo>
            {
                new ModuleBo(ModuleController.TreatyPricingQuotationDashboard.ToString(), TypeDashboard, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Quotation",
                    Index = 1
                },
                new ModuleBo(ModuleController.TreatyPricingPricingDashboard.ToString(), TypeDashboard, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Pricing",
                    Index = 2
                },
                new ModuleBo(ModuleController.TreatyPricingTreatyDashboard.ToString(), TypeDashboard, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Treaty",
                    Index = 3
                },
                new ModuleBo(ModuleController.TreatyPricingGroupDashboard.ToString(), TypeDashboard, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Group",
                    Index = 4
                },
                new ModuleBo(ModuleController.TreatyPricingInternalDashboard.ToString(), TypeDashboard, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Internal",
                    Index = 5
                },
            };
        }

        public static IList<ModuleBo> GetGroupBusinessModules()
        {
            return new List<ModuleBo>
            {
                (new ModuleBo(ModuleController.TreatyPricingGroupReferral.ToString(), TypeGroupBusiness, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Group Referral",
                    PowerAdditionals = new List<string>
                    {
                        AccessMatrixBo.PowerCompleteChecklist,
                        AccessMatrixBo.PowerUltimaApproverGroup,
                        AccessMatrixBo.PowerUltimaApproverReviewer,
                        AccessMatrixBo.PowerUltimaApproverHod,
                        AccessMatrixBo.PowerUltimaApproverCeo,
                    },
                    Index = 1
                }).SetPowerAdditionals(),
                new ModuleBo(ModuleController.TreatyPricingGroupMasterLetter.ToString(), TypeGroupBusiness, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Group Master Letter",
                    Index = 2
                },
            };
        }

        public static IList<ModuleBo> GetGroupReportModules()
        {
            return new List<ModuleBo>
            {
                new ModuleBo(ModuleController.HipsComparisonReport.ToString(), TypeGroupReport, DepartmentBo.DepartmentTreatyPricing)
                {
                    Name = "HIPS",
                    Index = 1
                },
                new ModuleBo(ModuleController.GHSClaimExperienceReport.ToString(), TypeGroupReport, DepartmentBo.DepartmentTreatyPricing)
                {
                    Name = "GHS Claim Experience",
                    Editable = true, ReportPath = ModuleController.GHSClaimExperienceReport.ToString(),
                    Index = 2
                },
                new ModuleBo(ModuleController.GTLClaimExperienceReport.ToString(), TypeGroupReport, DepartmentBo.DepartmentTreatyPricing)
                {
                    Name = "GTL Claim Experience",
                    Editable = true, ReportPath = ModuleController.GTLClaimExperienceReport.ToString(),
                    Index = 3
                },
                new ModuleBo(ModuleController.GtlRatesByUnitRateReport.ToString(), TypeGroupReport, DepartmentBo.DepartmentTreatyPricing)
                {
                    Name = "GTL Rates by Unit Rate",
                    Index = 4
                },
                new ModuleBo(ModuleController.GtlRatesByAgeBandedReport.ToString(), TypeGroupReport, DepartmentBo.DepartmentTreatyPricing)
                {
                    Name = "GTL Rates by Age Banded",
                    Index = 5
                },
                new ModuleBo(ModuleController.GTLBasisOfSA.ToString(), TypeGroupReport, DepartmentBo.DepartmentTreatyPricing)
                {
                    Name = "GTL Basis of SA",
                    Index = 5
                },
                new ModuleBo(ModuleController.ProductAndBenefitDetailsReport.ToString(), TypeGroupReport, DepartmentBo.DepartmentTreatyPricing)
                {
                    Name = "Product & Benefit Details",
                    Index = 7
                },
                new ModuleBo(ModuleController.GroupReferralReport.ToString(), TypeGroupReport, DepartmentBo.DepartmentTreatyPricing)
                {
                    Name = "Group Referral Report",
                    Editable = true,
                    ReportPath = ModuleController.GroupReferralReport.ToString(),
                    HideParameters = true,
                    Index = 10
                },
                new ModuleBo(ModuleController.GroupOverallTatReport.ToString(), TypeGroupReport, DepartmentBo.DepartmentTreatyPricing)
                {
                    Name = "Group Overall TAT",
                    Editable = true, ReportPath = ModuleController.GroupOverallTatReport.ToString(),
                    Index = 11
                },
                new ModuleBo(ModuleController.GroupAuthorityLimitReport.ToString(), TypeGroupReport, DepartmentBo.DepartmentTreatyPricing)
                {
                    Name = "Group Authority Limit by Reporting",
                    Editable = true, ReportPath = ModuleController.GroupAuthorityLimitReport.ToString(),
                    Index = 9
                },
                new ModuleBo(ModuleController.GroupAuthorityLimitListing.ToString(), TypeGroupReport, DepartmentBo.DepartmentTreatyPricing)
                {
                    Name = "Group Authority Limit by Listing",
                    Editable = true, ReportPath = ModuleController.GroupAuthorityLimitListing.ToString(),
                    Index = 8
                },

            };
        }

        public static IList<ModuleBo> GetPerLifeRetroModules()
        {
            return new List<ModuleBo>
            {
                new ModuleBo(ModuleController.PerLifeClaim.ToString(), TypePerLifeRetro, DepartmentBo.DepartmentRetrocession)
                {
                    Name = "Per Life Claims",
                    Index = 1
                },
                new ModuleBo(ModuleController.PerLifeSoa.ToString(), TypePerLifeRetro, DepartmentBo.DepartmentRetrocession)
                {
                    Name = "Per Life SOA",
                    PowerAdditional = AccessMatrixBo.PowerApprovalRetroSOA,
                    Index = 1
                },
            };
        }

        public static IList<ModuleBo> GetPerLifeAggregationModules()
        {
            return new List<ModuleBo>
            {
                new ModuleBo(ModuleController.PerLifeAggregation.ToString(), TypePerLifeAggregation, DepartmentBo.DepartmentRetrocession)
                {
                    Name = "Aggregation",
                    Index = 1
                },
                new ModuleBo(ModuleController.PerLifeAggregationDuplicationListing.ToString(), TypePerLifeAggregation, DepartmentBo.DepartmentRetrocession)
                {
                    Name = "Duplication Listing",
                    Index = 2
                },
                new ModuleBo(ModuleController.PerLifeAggregationConflictListing.ToString(), TypePerLifeAggregation, DepartmentBo.DepartmentRetrocession)
                {
                    Name = "Conflict Listing",
                    Index = 3
                },
            };
        }

        //public static IList<ModuleBo> GetTargetPlanningReportModules()
        //{
        //    return new List<ModuleBo>
        //    {
        //        new ModuleBo(ModuleController.TargetPlanningStatementTrackingReport.ToString(), TypeTargetPlanningReport, DepartmentBo.DepartmentDataAnalyticsAdministration) {
        //            Name = "Statement Tracking Report", Index = 1
        //        },
        //        new ModuleBo(ModuleController.TargetPlanningPCStatementTrackingReport.ToString(), TypeTargetPlanningReport, DepartmentBo.DepartmentDataAnalyticsAdministration) {
        //            Name = "Profit Commission Statement Tracking Report", Index = 2
        //        },
        //    };
        //}

        public static IList<ModuleBo> GetSubModules()
        {
            return new List<ModuleBo>
            {
                new ModuleBo(ModuleController.TreatyPricingProduct.ToString(), TypeSubModule, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Product",
                    Index = 1
                },
                new ModuleBo(ModuleController.TreatyPricingRateTableGroup.ToString(), TypeSubModule, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Rate Table Group",
                    Index = 2
                },
                new ModuleBo(ModuleController.TreatyPricingUwLimit.ToString(), TypeSubModule, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Underwriting Limit",
                    Index = 3
                },
                new ModuleBo(ModuleController.TreatyPricingMedicalTable.ToString(), TypeSubModule, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Medical Table",
                    Index = 4
                },
                new ModuleBo(ModuleController.TreatyPricingFinancialTable.ToString(), TypeSubModule, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Financial Table",
                    Index = 5
                },
                new ModuleBo(ModuleController.TreatyPricingUwQuestionnaire.ToString(), TypeSubModule, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Underwriting Questionnaire",
                    Index = 6
                },
                new ModuleBo(ModuleController.TreatyPricingAdvantageProgram.ToString(), TypeSubModule, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Advantage Program",
                    Index = 7
                },
                new ModuleBo(ModuleController.TreatyPricingClaimApprovalLimit.ToString(), TypeSubModule, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Claim Approval Limit",
                    Index = 8
                },
                new ModuleBo(ModuleController.TreatyPricingDefinitionAndExclusion.ToString(), TypeSubModule, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Definitions & Exclusions",
                    Index = 9
                },
                new ModuleBo(ModuleController.TreatyPricingCampaign.ToString(), TypeSubModule, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Campaign",
                    Index = 10
                },
                new ModuleBo(ModuleController.TreatyPricingProfitCommission.ToString(), TypeSubModule, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Profit Commission",
                    Index = 11
                },
                new ModuleBo(ModuleController.TreatyPricingCustomOther.ToString(), TypeSubModule, DepartmentBo.DepartmentTreatyPricing) {
                    Name = "Custom / Other",
                    Index = 12
                },
            };
        }

        public static IList<ModuleBo> GetModules()
        {
            var l = new List<ModuleBo>() { };
            l.AddRange(GetActionModules());
            l.AddRange(GetMaintenanceModules());
            l.AddRange(GetGroupMaintenanceModules());
            l.AddRange(GetReportModules());
            l.AddRange(GetRepositoryModules());
            l.AddRange(GetWorkflowModules());
            l.AddRange(GetDashboardModules());
            l.AddRange(GetGroupBusinessModules());
            l.AddRange(GetGroupReportModules());
            l.AddRange(GetPerLifeRetroModules());
            l.AddRange(GetPerLifeAggregationModules());
            //l.AddRange(GetTargetPlanningReportModules());
            l.AddRange(GetSubModules());
            return l;
        }

        public static string GetPowerByType(int type)
        {
            switch (type)
            {
                case TypeAction:
                case TypeMaintenance:
                case TypeGroupMaintenance:
                case TypeRepository:
                case TypeWorkflow:
                case TypeGroupBusiness:
                case TypePerLifeRetro:
                case TypePerLifeAggregation:
                case TypeSubModule:
                    return AccessMatrixBo.DefaultPower;
                case TypeReport:
                case TypeGroupReport:
                case TypeTargetPlanningReport:
                case TypeDashboard:
                    return AccessMatrixBo.AccessMatrixCRUD.R.ToString();
                default:
                    return "";
            }
        }

        public static string GetTypeName(int key)
        {
            switch (key)
            {
                case TypeAction:
                    return "Action";
                case TypeReport:
                    return "Report";
                case TypeMaintenance:
                    return "Maintenance";
                case TypeGroupMaintenance:
                    return "Group Maintenance";
                case TypeRepository:
                    return "Repository";
                case TypeWorkflow:
                    return "Workflow";
                case TypeDashboard:
                    return "Dashboard";
                case TypeGroupBusiness:
                    return "Group Business";
                case TypeGroupReport:
                    return "Group Report";
                case TypePerLifeRetro:
                    return "Per Life Retro";
                case TypePerLifeAggregation:
                    return "Per Life Aggregation";
                case TypeTargetPlanningReport:
                    return "Target Planning";
                case TypeSubModule:
                    return "Sub Module";
                default:
                    return "";
            }
        }

        public static string GetTypeIcon(int key)
        {
            switch (key)
            {
                case TypeAction:
                    return "fas fa-list";
                case TypeReport:
                case TypeGroupReport:
                    return "fas fa-table";
                case TypeMaintenance:
                    return "fas fa-cog";
                case TypeRepository:
                    return "fas fa-folder";
                case TypeWorkflow:
                    return "fas fa-folder";
                case TypeGroupBusiness:
                    return "fas fa-folder";
                case TypePerLifeRetro:
                    return "fas fa-folder";
                case TypePerLifeAggregation:
                    return "fas fa-folder";
                case TypeSubModule:
                    return "fas fa-list";
                default:
                    return "";
            }
        }

        public static string FormatCheckBoxName(int moduleId, string power)
        {
            return string.Format(AccessMatrixBo.CheckBoxNameFormat, moduleId, power);
        }

        public string GetFormattedPower()
        {
            if (string.IsNullOrEmpty(PowerAdditional))
                return Power;
            return string.Format("{0},{1}", Power, PowerAdditional);
        }

        public string GetCheckBoxName(string power)
        {
            if (IsPowerExists(power))
            {
                return FormatCheckBoxName(Id, power);
            }
            return null;
        }

        public string GetAdditionalCheckBoxName(string power)
        {
            return FormatCheckBoxName(Id, power);
        }

        public string GetCreateName()
        {
            return GetCheckBoxName(AccessMatrixBo.AccessMatrixCRUD.C.ToString());
        }

        public string GetReadName()
        {
            return GetCheckBoxName(AccessMatrixBo.AccessMatrixCRUD.R.ToString());
        }

        public string GetUpdateName()
        {
            return GetCheckBoxName(AccessMatrixBo.AccessMatrixCRUD.U.ToString());
        }

        public string GetDeleteName()
        {
            return GetCheckBoxName(AccessMatrixBo.AccessMatrixCRUD.D.ToString());
        }

        public string[] GetPowers()
        {
            return Power.Split(Delimiter);
        }

        public List<string> GetPowerAdditionals()
        {
            if (string.IsNullOrEmpty(PowerAdditional))
                return new List<string> { };
            PowerAdditionals = PowerAdditional.Split(Delimiter).ToList();
            return PowerAdditionals;
        }

        /// <summary>
        /// This is to string.Join the List<string> to PowerAdditional
        /// </summary>
        /// <returns></returns>
        public ModuleBo SetPowerAdditionals()
        {
            PowerAdditional = null;
            if (PowerAdditionals.IsNullOrEmpty())
                return this;
            PowerAdditional = string.Join(",", PowerAdditionals);
            return this;
        }

        public bool IsChecked(string p)
        {
            if (AccessMatrixBo == null)
                return false;
            return AccessMatrixBo.IsPowerExists(p);
        }

        public bool IsCreateChecked()
        {
            return IsChecked(AccessMatrixBo.AccessMatrixCRUD.C.ToString());
        }

        public bool IsReadChecked()
        {
            return IsChecked(AccessMatrixBo.AccessMatrixCRUD.R.ToString());
        }

        public bool IsUpdateChecked()
        {
            return IsChecked(AccessMatrixBo.AccessMatrixCRUD.U.ToString());
        }

        public bool IsDeleteChecked()
        {
            return IsChecked(AccessMatrixBo.AccessMatrixCRUD.D.ToString());
        }

        public bool IsPowerExists(string power)
        {
            if (GetPowers().Contains<String>(power))
            {
                return true;
            }
            return false;
        }
    }
}
