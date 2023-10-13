using BusinessObject;
using BusinessObject.Claims;
using BusinessObject.Identity;
using BusinessObject.Retrocession;
using BusinessObject.RiDatas;
using BusinessObject.SoaDatas;
using DataAccess.Entities;
using DataAccess.Entities.Claims;
using DataAccess.Entities.Identity;
using DataAccess.Entities.InvoiceRegisters;
using DataAccess.Entities.Retrocession;
using DataAccess.Entities.RiDatas;
using DataAccess.Entities.Sanctions;
using DataAccess.Entities.SoaDatas;
using DataAccess.Entities.TreatyPricing;
using DataAccess.Entities.Views;
using Microsoft.AspNet.Identity.EntityFramework;
using Shared;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;

namespace DataAccess.EntityFramework
{
    public class AppDbContext : IdentityDbContext<User, Role, int, UserLogin, UserRole, UserClaim>
    {
        public AppDbContext() : this("Default")
        {
        }

        public AppDbContext(bool timeout = true) : this("Default", timeout)
        {
        }

        public AppDbContext(string connection, bool timeout = true) : base(connection)
        {
            if (!timeout)
                Database.CommandTimeout = 0; // no time out
            Configuration.AutoDetectChangesEnabled = false;
            if (bool.TryParse(Util.GetConfig("AutoDetectChangesEnabled"), out bool result))
            {
                Configuration.AutoDetectChangesEnabled = result;
            }
        }

        public static AppDbContext Create()
        {
            return new AppDbContext();
        }

        public IQueryable<User> GetUsers(bool deleted = false)
        {
            if (!deleted)
                return Users.Where(q => q.Status != UserBo.StatusDelete);
            return Users.AsQueryable();
        }

        public IQueryable<RiDataBatch> GetRiDataBatches(bool deleted = false)
        {
            if (!deleted)
                return RiDataBatches.Where(q => q.Status != RiDataBatchBo.StatusPendingDelete);
            return RiDataBatches.AsQueryable();
        }

        public IQueryable<ClaimDataBatch> GetClaimDataBatches(bool deleted = false)
        {
            if (!deleted)
                return ClaimDataBatches.Where(q => q.Status != ClaimDataBatchBo.StatusPendingDelete);
            return ClaimDataBatches.AsQueryable();
        }

        public IQueryable<ClaimRegister> GetClaimRegister(bool isClaimOnly = false)
        {
            if (isClaimOnly)
            {
                var statuses = ClaimRegisterBo.GetClaimDepartmentStatus();
                return ClaimRegister.Where(q => statuses.Contains(q.ClaimStatus)).AsQueryable();
            }
            return ClaimRegister.AsQueryable();
        }

        public IQueryable<SoaDataBatch> GetSoaDataBatches(bool deleted = false)
        {
            if (!deleted)
                return SoaDataBatches.Where(q => q.Status != SoaDataBatchBo.StatusPendingDelete);
            return SoaDataBatches.AsQueryable();
        }

        public IQueryable<PerLifeAggregation> GetPerLifeAggregations(bool deleted = false)
        {
            if (!deleted)
                return PerLifeAggregations.Where(q => q.Status != PerLifeAggregationBo.StatusPendingDelete);
            return PerLifeAggregations.AsQueryable();
        }

        public DbSet<Module> Modules { get; set; }
        public DbSet<AccessGroup> AccessGroups { get; set; }
        public DbSet<AccessMatrix> AccessMatrices { get; set; }
        public DbSet<Benefit> Benefits { get; set; }

        public DbSet<BenefitDetail> BenefitDetails { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Cedant> Cedants { get; set; }
        public DbSet<CedantWorkgroup> CedantWorkgroups { get; set; }
        public DbSet<CedantWorkgroupCedant> CedantWorkgroupCedants { get; set; }
        public DbSet<CedantWorkgroupUser> CedantWorkgroupUsers { get; set; }
        public DbSet<RiDataConfig> RiDataConfigs { get; set; }
        public DbSet<PickList> PickLists { get; set; }
        public DbSet<PickListDetail> PickListDetails { get; set; }
        public DbSet<RawFile> RawFiles { get; set; }
        public DbSet<RiDataBatch> RiDataBatches { get; set; }
        public DbSet<RiDataBatchStatusFile> RiDataBatchStatusFiles { get; set; }
        public DbSet<RiDataFile> RiDataFiles { get; set; }
        public DbSet<RiData> RiData { get; set; }
        public DbSet<RiDataMapping> RiDataMappings { get; set; }
        public DbSet<RiDataMappingDetail> RiDataMappingDetails { get; set; }
        public DbSet<RiDataComputation> RiDataComputations { get; set; }
        public DbSet<RiDataPreValidation> RiDataPreValidations { get; set; }
        public DbSet<Remark> Remarks { get; set; }
        public DbSet<RiDataCorrection> RiDataCorrections { get; set; }
        public DbSet<RateTable> RateTables { get; set; }
        public DbSet<RateTableDetail> RateTableDetails { get; set; }
        public DbSet<Mfrs17CellMapping> Mfrs17CellMappings { get; set; }
        public DbSet<Mfrs17CellMappingDetail> Mfrs17CellMappingDetails { get; set; }
        public DbSet<Mfrs17Reporting> Mfrs17Reportings { get; set; }
        public DbSet<Mfrs17ReportingDetail> Mfrs17ReportingDetails { get; set; }
        public DbSet<Mfrs17ReportingDetailRiData> Mfrs17ReportingDetailRiDatas { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<StandardOutput> StandardOutputs { get; set; }
        public DbSet<StatusHistory> StatusHistories { get; set; }
        public DbSet<Treaty> Treaties { get; set; }
        public DbSet<TreatyBenefitCodeMapping> TreatyBenefitCodeMappings { get; set; }
        public DbSet<TreatyBenefitCodeMappingDetail> TreatyBenefitCodeMappingDetails { get; set; }
        public DbSet<TreatyCode> TreatyCodes { get; set; }
        public DbSet<TreatyOldCode> TreatyOldCodes { get; set; }
        public DbSet<UserAccessGroup> UserAccessGroups { get; set; }
        public DbSet<UserPassword> UserPasswords { get; set; }
        public DbSet<UserTrail> UserTrails { get; set; }
        public DbSet<Export> Exports { get; set; }
        public DbSet<ClaimCode> ClaimCodes { get; set; }
        public DbSet<ClaimDataConfig> ClaimDataConfigs { get; set; }
        public DbSet<EventClaimCodeMapping> EventClaimCodeMappings { get; set; }
        public DbSet<EventClaimCodeMappingDetail> EventClaimCodeMappingDetails { get; set; }
        public DbSet<AccountCode> AccountCodes { get; set; }
        public DbSet<AccountCodeMapping> AccountCodeMappings { get; set; }
        public DbSet<AccountCodeMappingDetail> AccountCodeMappingDetails { get; set; }
        public DbSet<ClaimDataMapping> ClaimDataMappings { get; set; }
        public DbSet<ClaimDataMappingDetail> ClaimDataMappingDetails { get; set; }
        public DbSet<ClaimDataComputation> ClaimDataComputations { get; set; }
        public DbSet<ClaimDataValidation> ClaimDataValidations { get; set; }
        public DbSet<Salutation> Salutations { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<RateDetail> RateDetails { get; set; }
        public DbSet<ItemCodeMapping> ItemCodeMappings { get; set; }
        public DbSet<ItemCodeMappingDetail> ItemCodeMappingDetails { get; set; }
        public DbSet<AuthorizationLimit> AuthorizationLimits { get; set; }
        public DbSet<DiscountTable> DiscountTables { get; set; }
        public DbSet<RiDiscount> RiDiscounts { get; set; }
        public DbSet<LargeDiscount> LargeDiscounts { get; set; }
        public DbSet<GroupDiscount> GroupDiscounts { get; set; }
        public DbSet<ItemCode> ItemCodes { get; set; }
        public DbSet<ClaimCodeMapping> ClaimCodeMappings { get; set; }
        public DbSet<ClaimCodeMappingDetail> ClaimCodeMappingDetails { get; set; }
        public DbSet<ClaimDataBatch> ClaimDataBatches { get; set; }
        public DbSet<ClaimDataFile> ClaimDataFiles { get; set; }
        public DbSet<ClaimDataBatchStatusFile> ClaimDataBatchStatusFiles { get; set; }
        public DbSet<AnnuityFactor> AnnuityFactors { get; set; }
        public DbSet<AnnuityFactorDetail> AnnuityFactorDetails { get; set; }
        public DbSet<AnnuityFactorMapping> AnnuityFactorMappings { get; set; }
        public DbSet<EventCode> EventCodes { get; set; }
        public DbSet<FacMasterListing> FacMasterListings { get; set; }
        public DbSet<FacMasterListingDetail> FacMasterListingDetails { get; set; }
        public DbSet<ClaimAuthorityLimitCedant> ClaimAuthorityLimitCedants { get; set; }
        public DbSet<ClaimAuthorityLimitCedantDetail> ClaimAuthorityLimitCedantDetails { get; set; }
        public DbSet<ClaimChecklist> ClaimChecklists { get; set; }
        public DbSet<ClaimChecklistDetail> ClaimChecklistDetails { get; set; }
        public DbSet<ClaimAuthorityLimitMLRe> ClaimAuthorityLimitMLRe { get; set; }
        public DbSet<ClaimAuthorityLimitMLReDetail> ClaimAuthorityLimitMLReDetails { get; set; }
        public DbSet<ClaimCategory> ClaimCategories { get; set; }
        public DbSet<ClaimReason> ClaimReasons { get; set; }
        public DbSet<StandardClaimDataOutput> StandardClaimDataOutputs { get; set; }
        public DbSet<ClaimData> ClaimData { get; set; }
        public DbSet<PublicHoliday> PublicHolidays { get; set; }
        public DbSet<PublicHolidayDetail> PublicHolidayDetails { get; set; }
        public DbSet<ClaimRegister> ClaimRegister { get; set; }
        public DbSet<ClaimRegisterHistory> ClaimRegisterHistories { get; set; }
        public DbSet<SoaDataBatch> SoaDataBatches { get; set; }
        public DbSet<SoaDataBatchStatusFile> SoaDataBatchStatusFiles { get; set; }
        public DbSet<SoaDataFile> SoaDataFiles { get; set; }
        public DbSet<SoaData> SoaData { get; set; }
        public DbSet<StandardSoaDataOutput> StandardSoaDataOutputs { get; set; }
        public DbSet<RetroParty> RetroParties { get; set; }
        public DbSet<SoaDataRiDataSummary> SoaDataRiDataSummaries { get; set; }
        public DbSet<SoaDataPostValidation> SoaDataPostValidations { get; set; }
        public DbSet<SoaDataPostValidationDifference> SoaDataPostValidationDifferences { get; set; }
        public DbSet<SoaDataCompiledSummary> SoaDataCompiledSummaries { get; set; }
        public DbSet<SoaDataDiscrepancy> SoaDataDiscrepancies { get; set; }
        public DbSet<RiDataWarehouse> RiDataWarehouse { get; set; }
        public DbSet<PremiumSpreadTable> PremiumSpreadTables { get; set; }
        public DbSet<PremiumSpreadTableDetail> PremiumSpreadTableDetails { get; set; }
        public DbSet<TreatyDiscountTable> TreatyDiscountTables { get; set; }
        public DbSet<TreatyDiscountTableDetail> TreatyDiscountTableDetails { get; set; }
        public DbSet<DirectRetroConfiguration> DirectRetroConfigurations { get; set; }
        public DbSet<DirectRetroConfigurationMapping> DirectRetroConfigurationMappings { get; set; }
        public DbSet<DirectRetroConfigurationDetail> DirectRetroConfigurationDetails { get; set; }
        public DbSet<InvoiceRegisterBatch> InvoiceRegisterBatches { get; set; }
        public DbSet<InvoiceRegisterBatchFile> InvoiceRegisterBatchFiles { get; set; }
        public DbSet<InvoiceRegisterBatchSoaData> InvoiceRegisterBatchSoaDatas { get; set; }
        public DbSet<InvoiceRegisterBatchRemark> InvoiceRegisterBatchRemarks { get; set; }
        public DbSet<InvoiceRegisterBatchRemarkDocument> InvoiceRegisterBatchRemarkDocuments { get; set; }
        public DbSet<InvoiceRegister> InvoiceRegisters { get; set; }
        public DbSet<InvoiceRegisterHistory> InvoiceRegisterHistories { get; set; }
        public DbSet<InvoiceRegisterValuation> InvoiceRegisterValuations { get; set; }
        public DbSet<DirectRetro> DirectRetro { get; set; }
        public DbSet<RetroSummary> RetroSummaries { get; set; }
        public DbSet<DirectRetroStatusFile> DirectRetroStatusFiles { get; set; }
        public DbSet<RetroStatement> RetroStatements { get; set; }
        public DbSet<ReferralClaim> ReferralClaims { get; set; }
        public DbSet<RetroRegisterBatch> RetroRegisterBatches { get; set; }
        public DbSet<RetroRegisterBatchDirectRetro> RetroRegisterBatchDirectRetros { get; set; }
        public DbSet<RetroRegisterBatchFile> RetroRegisterBatchFiles { get; set; }
        public DbSet<RetroRegister> RetroRegisters { get; set; }
        public DbSet<RetroRegisterHistory> RetroRegisterHistories { get; set; }
        public DbSet<CutOff> CutOff { get; set; }
        public DbSet<RiDataWarehouseHistory> RiDataWarehouseHistories { get; set; }
        public DbSet<FinanceProvisioningTransaction> FinanceProvisioningTransactions { get; set; }
        public DbSet<DirectRetroProvisioningTransaction> DirectRetroProvisioningTransactions { get; set; }
        public DbSet<FinanceProvisioning> FinanceProvisionings { get; set; }
        public DbSet<ReferralRiDataFile> ReferralRiDataFiles { get; set; }
        public DbSet<ReferralRiData> ReferralRiData { get; set; }
        public DbSet<SanctionKeyword> SanctionKeywords { get; set; }
        public DbSet<SanctionKeywordDetail> SanctionKeywordDetails { get; set; }
        public DbSet<SanctionExclusion> SanctionExclusions { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<SanctionBatch> SanctionBatches { get; set; }
        public DbSet<Sanction> Sanctions { get; set; }
        public DbSet<SanctionName> SanctionNames { get; set; }
        public DbSet<SanctionVerification> SanctionVerifications { get; set; }
        public DbSet<SanctionAddress> SanctionAddresses { get; set; }
        public DbSet<SanctionBirthDate> SanctionBirthDates { get; set; }
        public DbSet<SanctionComment> SanctionComments { get; set; }
        public DbSet<SanctionCountry> SanctionCountries { get; set; }
        public DbSet<SanctionIdentity> SanctionIdentities { get; set; }
        public DbSet<SanctionVerificationDetail> SanctionVerificationDetails { get; set; }
        public DbSet<SanctionFormatName> SanctionFormatNames { get; set; }
        public DbSet<TreatyMarketingAllowanceProvision> TreatyMarketingAllowanceProvisions { get; set; }
        public DbSet<ObjectPermission> ObjectPermissions { get; set; }
        public DbSet<RemarkFollowUp> RemarkFollowUps { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<SoaDataHistory> SoaDataHistories { get; set; }
        public DbSet<SoaDataCompiledSummaryHistory> SoaDataCompiledSummaryHistories { get; set; }
        public DbSet<SanctionWhitelist> SanctionWhitelists { get; set; }
        public DbSet<PickListDepartment> PickListDepartments { get; set; }
        public DbSet<SanctionBlacklist> SanctionBlacklists { get; set; }
        public DbSet<InvoiceRegisterBatchStatusFile> InvoiceRegisterBatchStatusFiles { get; set; }
        public DbSet<RetroRegisterBatchStatusFile> RetroRegisterBatchStatusFiles { get; set; }
        public DbSet<SoaDataBatchHistory> SoaDataBatchHistories { get; set; }
        public DbSet<Mfrs17ContractCode> Mfrs17ContractCodes { get; set; }
        public DbSet<Mfrs17ContractCodeDetail> Mfrs17ContractCodeDetails { get; set; }
        public DbSet<UwQuestionnaireCategory> UwQuestionnaireCategories { get; set; }
        public DbSet<HipsCategory> HipsCategories { get; set; }
        public DbSet<HipsCategoryDetail> HipsCategoryDetails { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<InsuredGroupName> InsuredGroupNames { get; set; }
        public DbSet<TreatyPricingCedant> TreatyPricingCedants { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<TemplateDetail> TemplateDetails { get; set; }
        public DbSet<TreatyPricingRateTableGroup> TreatyPricingRateTableGroups { get; set; }
        public DbSet<TreatyPricingRateTable> TreatyPricingRateTables { get; set; }
        public DbSet<TreatyPricingClaimApprovalLimit> TreatyPricingClaimApprovalLimits { get; set; }
        public DbSet<TreatyPricingClaimApprovalLimitVersion> TreatyPricingClaimApprovalLimitVersions { get; set; }
        public DbSet<TreatyPricingUwQuestionnaire> TreatyPricingUwQuestionnaires { get; set; }
        public DbSet<TreatyPricingUwQuestionnaireVersion> TreatyPricingUwQuestionnaireVersions { get; set; }
        public DbSet<TreatyPricingUwQuestionnaireVersionDetail> TreatyPricingUwQuestionnaireVersionDetails { get; set; }
        public DbSet<TreatyPricingUwQuestionnaireVersionFile> TreatyPricingUwQuestionnaireVersionFiles { get; set; }
        public DbSet<TreatyPricingProduct> TreatyPricingProducts { get; set; }
        public DbSet<TreatyPricingRateTableVersion> TreatyPricingRateTableVersions { get; set; }
        public DbSet<TreatyPricingRateTableDetail> TreatyPricingRateTableDetails { get; set; }
        public DbSet<TreatyPricingRateTableRate> TreatyPricingRateTableRates { get; set; }
        public DbSet<TreatyPricingRateTableOriginalRate> TreatyPricingRateTableOriginalRates { get; set; }
        public DbSet<TreatyPricingUwLimit> TreatyPricingUwLimits { get; set; }
        public DbSet<TreatyPricingUwLimitVersion> TreatyPricingUwLimitVersions { get; set; }
        public DbSet<TreatyPricingProductVersion> TreatyPricingProductVersions { get; set; }
        public DbSet<TreatyPricingDefinitionAndExclusion> TreatyPricingDefinitionAndExclusions { get; set; }
        public DbSet<TreatyPricingDefinitionAndExclusionVersion> TreatyPricingDefinitionAndExclusionVersions { get; set; }
        public DbSet<TreatyPricingCustomOther> TreatyPricingCustomOthers { get; set; }
        public DbSet<TreatyPricingCustomOtherVersion> TreatyPricingCustomOtherVersions { get; set; }
        public DbSet<TreatyPricingCustomOtherProduct> TreatyPricingCustomOtherProducts { get; set; }
        public DbSet<TreatyPricingMedicalTable> TreatyPricingMedicalTables { get; set; }
        public DbSet<TreatyPricingMedicalTableVersion> TreatyPricingMedicalTableVersions { get; set; }
        public DbSet<TreatyPricingMedicalTableVersionFile> TreatyPricingMedicalTableVersionFiles { get; set; }
        public DbSet<TreatyPricingMedicalTableVersionDetail> TreatyPricingMedicalTableVersionDetails { get; set; }
        public DbSet<TreatyPricingAdvantageProgram> TreatyPricingAdvantagePrograms { get; set; }
        public DbSet<TreatyPricingAdvantageProgramVersion> TreatyPricingAdvantageProgramVersions { get; set; }
        public DbSet<TreatyPricingAdvantageProgramVersionBenefit> TreatyPricingAdvantageProgramVersionBenefits { get; set; }
        public DbSet<TreatyPricingFinancialTable> TreatyPricingFinancialTables { get; set; }
        public DbSet<TreatyPricingFinancialTableVersion> TreatyPricingFinancialTableVersions { get; set; }
        public DbSet<TreatyPricingFinancialTableVersionFile> TreatyPricingFinancialTableVersionFiles { get; set; }
        public DbSet<TreatyPricingFinancialTableVersionDetail> TreatyPricingFinancialTableVersionDetails { get; set; }
        public DbSet<TreatyPricingMedicalTableUploadCell> TreatyPricingMedicalTableUploadCells { get; set; }
        public DbSet<TreatyPricingMedicalTableUploadColumn> TreatyPricingMedicalTableUploadColumns { get; set; }
        public DbSet<TreatyPricingMedicalTableUploadLegend> TreatyPricingMedicalTableUploadLegends { get; set; }
        public DbSet<TreatyPricingMedicalTableUploadRow> TreatyPricingMedicalTableUploadRows { get; set; }
        public DbSet<TreatyPricingFinancialTableUpload> TreatyPricingFinancialTableUploads { get; set; }
        public DbSet<TreatyPricingFinancialTableUploadLegend> TreatyPricingFinancialTableUploadLegends { get; set; }
        public DbSet<TreatyPricingProductDetail> TreatyPricingProductDetails { get; set; }
        public DbSet<TreatyPricingPickListDetail> TreatyPricingPickListDetails { get; set; }
        public DbSet<TreatyPricingProfitCommission> TreatyPricingProfitCommissions { get; set; }
        public DbSet<TreatyPricingProfitCommissionVersion> TreatyPricingProfitCommissionVersions { get; set; }
        public DbSet<TreatyPricingTierProfitCommission> TreatyPricingTierProfitCommissions { get; set; }
        public DbSet<TreatyPricingProfitCommissionDetail> TreatyPricingProfitCommissionDetails { get; set; }
        public DbSet<TreatyPricingProductBenefit> TreatyPricingProductBenefits { get; set; }
        public DbSet<TreatyPricingCampaign> TreatyPricingCampaigns { get; set; }
        public DbSet<TreatyPricingCampaignVersion> TreatyPricingCampaignVersions { get; set; }
        public DbSet<TreatyPricingCampaignProduct> TreatyPricingCampaignProducts { get; set; }
        public DbSet<TreatyPricingQuotationWorkflow> TreatyPricingQuotationWorkflows { get; set; }
        public DbSet<TreatyPricingQuotationWorkflowVersion> TreatyPricingQuotationWorkflowVersions { get; set; }
        public DbSet<TreatyPricingWorkflowObject> TreatyPricingWorkflowObjects { get; set; }
        public DbSet<TreatyPricingQuotationWorkflowVersionQuotationChecklist> TreatyPricingQuotationWorkflowVersionQuotationChecklists { get; set; }
        public DbSet<TreatyPricingTreatyWorkflow> TreatyPricingTreatyWorkflows { get; set; }
        public DbSet<TreatyPricingTreatyWorkflowVersion> TreatyPricingTreatyWorkflowVersions { get; set; }
        public DbSet<TreatyPricingPerLifeRetro> TreatyPricingPerLifeRetro { get; set; }
        public DbSet<TreatyPricingPerLifeRetroVersion> TreatyPricingPerLifeRetroVersions { get; set; }
        public DbSet<TreatyPricingPerLifeRetroVersionDetail> TreatyPricingPerLifeRetroVersionDetails { get; set; }
        public DbSet<TreatyPricingPerLifeRetroVersionTier> TreatyPricingPerLifeRetroVersionTiers { get; set; }
        public DbSet<TreatyPricingPerLifeRetroVersionBenefit> TreatyPricingPerLifeRetroVersionBenefits { get; set; }
        public DbSet<TreatyPricingPerLifeRetroProduct> TreatyPricingPerLifeRetroProducts { get; set; }
        public DbSet<TreatyPricingProductBenefitDirectRetro> TreatyPricingProductBenefitDirectRetros { get; set; }
        public DbSet<TreatyPricingGroupReferral> TreatyPricingGroupReferrals { get; set; }
        public DbSet<TreatyPricingGroupReferralVersion> TreatyPricingGroupReferralVersions { get; set; }
        public DbSet<TreatyPricingGroupReferralVersionBenefit> TreatyPricingGroupReferralVersionBenefits { get; set; }
        public DbSet<TreatyPricingProductPerLifeRetro> TreatyPricingProductPerLifeRetros { get; set; }
        public DbSet<TreatyPricingGroupReferralFile> TreatyPricingGroupReferralFiles { get; set; }
        public DbSet<TreatyPricingGroupReferralHipsTable> TreatyPricingGroupReferralHipsTables { get; set; }
        public DbSet<TreatyPricingGroupReferralGtlTable> TreatyPricingGroupReferralGtlTables { get; set; }
        public DbSet<TreatyPricingGroupReferralGhsTable> TreatyPricingGroupReferralGhsTables { get; set; }
        public DbSet<RetroBenefitCode> RetroBenefitCodes { get; set; }
        public DbSet<RetroBenefitRetentionLimit> RetroBenefitRetentionLimits { get; set; }
        public DbSet<RetroBenefitRetentionLimitDetail> RetroBenefitRetentionLimitDetails { get; set; }
        public DbSet<TreatyPricingReportGeneration> TreatyPricingReportGenerations { get; set; }
        public DbSet<RetroBenefitCodeMapping> RetroBenefitCodeMappings { get; set; }
        public DbSet<RetroBenefitCodeMappingDetail> RetroBenefitCodeMappingDetails { get; set; }
        public DbSet<TreatyPricingGroupReferralChecklist> TreatyPricingGroupReferralChecklists { get; set; }
        public DbSet<PerLifeDuplicationCheck> PerLifeDuplicationChecks { get; set; }
        public DbSet<PerLifeRetroGender> PerLifeRetroGenders { get; set; }
        public DbSet<PerLifeRetroCountry> PerLifeRetroCountries { get; set; }
        public DbSet<GstMaintenance> GstMaintenances { get; set; }
        public DbSet<RetroTreaty> RetroTreaties { get; set; }
        public DbSet<PerLifeRetroConfigurationTreaty> PerLifeRetroConfigurationTreaties { get; set; }
        public DbSet<ValidDuplicationList> ValidDuplicationLists { get; set; }
        public DbSet<PerLifeRetroConfigurationRatio> PerLifeRetroConfigurationRatios { get; set; }
        public DbSet<TreatyPricingGroupMasterLetter> TreatyPricingGroupMasterLetters { get; set; }
        public DbSet<TreatyPricingGroupMasterLetterGroupReferral> TreatyPricingGroupMasterLetterGroupReferrals { get; set; }
        public DbSet<RetroTreatyDetail> RetroTreatyDetails { get; set; }
        public DbSet<PerLifeAggregation> PerLifeAggregations { get; set; }
        public DbSet<PerLifeDataCorrection> PerLifeDataCorrections { get; set; }
        public DbSet<PerLifeAggregationDetail> PerLifeAggregationDetails { get; set; }
        public DbSet<RetroRegisterFile> RetroRegisterFiles { get; set; }
        public DbSet<PerLifeAggregationDetailData> PerLifeAggregationDetailData { get; set; }
        public DbSet<PerLifeAggregationDuplicationListing> PerLifeAggregationDuplicationListings { get; set; }
        public DbSet<PerLifeAggregationDetailTreaty> PerLifeAggregationDetailTreaties { get; set; }
        public DbSet<PerLifeAggregationConflictListing> PerLifeAggregationConflictListings { get; set; }
        public DbSet<TreatyPricingGroupReferralChecklistDetail> TreatyPricingGroupReferralChecklistDetails { get; set; }
        public DbSet<PerLifeDuplicationCheckDetail> PerLifeDuplicationCheckDetails { get; set; }
        public DbSet<StandardRetroOutput> StandardRetroOutputs { get; set; }
        public DbSet<PerLifeAggregationMonthlyData> PerLifeAggregationMonthlyData { get; set; }
        public DbSet<PerLifeAggregatedData> PerLifeAggregatedData { get; set; }
        public DbSet<PerLifeAggregationMonthlyRetroData> PerLifeAggregationMonthlyRetroData { get; set; }
        public DbSet<PerLifeClaim> PerLifeClaims { get; set; }
        public DbSet<PerLifeClaimData> PerLifeClaimData { get; set; }
        public DbSet<PerLifeClaimRetroData> PerLifeClaimRetroData { get; set; }
        public DbSet<PerLifeSoa> PerLifeSoa { get; set; }
        public DbSet<PerLifeSoaData> PerLifeSoaData { get; set; }
        public DbSet<RetroBenefitCodeMappingTreaty> RetroBenefitCodeMappingTreaties { get; set; }
        public DbSet<PerLifeRetroStatement> PerLifeRetroStatements { get; set; }
        public DbSet<PerLifeSoaSummaries> PerLifeSoaSummaries { get; set; }
        public DbSet<PerLifeSoaSummariesByTreaty> PerLifeSoaSummariesByTreaty { get; set; }
        public DbSet<PerLifeSoaSummariesSoa> PerLifeSoaSummariesSoa { get; set; }
        public DbSet<ObjectLock> ObjectLocks { get; set; }
        public DbSet<QuotationDashboardActiveCasesByCompany> QuotationDashboardActiveCasesByCompany { get; set; }
        public DbSet<PricingDashboardDueDateOverview> PricingDashboardDueDateOverview { get; set; }
        public DbSet<PricingDashboardDueDateOverviewIdList> PricingDashboardDueDateOverviewIdList { get; set; }
        public DbSet<PricingDashboardDueDateOverviewPIC> PricingDashboardDueDateOverviewPIC { get; set; }
        public DbSet<PricingDashboardOutstandingPricingOverviewPIC> PricingDashboardOutstandingPricingOverviewPIC { get; set; }
        public DbSet<PricingDashboardQuotingCasesByCompany> PricingDashboardQuotingCasesByCompany { get; set; }
        public DbSet<TreatyBenefitCodeMappingUpload> TreatyBenefitCodeMappingUpload { get; set; }
        public DbSet<RateTableMappingUpload> RateTableMappingUpload { get; set; }
        public DbSet<FacMasterListingUpload> FacMasterListingUpload { get; set; }
        public DbSet<RateDetailUpload> RateDetailUpload { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            EntityTypeConfiguration<Role> roles = modelBuilder.Entity<Role>();
            roles.ToTable("Roles");

            EntityTypeConfiguration<UserClaim> userClaims = modelBuilder.Entity<UserClaim>();
            userClaims.ToTable("UserClaims").HasKey(q => q.Id);

            EntityTypeConfiguration<UserLogin> userLogins = modelBuilder.Entity<UserLogin>();
            userLogins.ToTable("UserLogins");

            EntityTypeConfiguration<UserPassword> userPasswords = modelBuilder.Entity<UserPassword>();
            userPasswords.ToTable("UserPasswords").HasKey(q => q.Id);
            userPasswords.HasRequired(q => q.User).WithMany().HasForeignKey(q => q.UserId).WillCascadeOnDelete(false);
            // DO NOT FOREIGN
            // userPasswords.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<UserRole> userRoles = modelBuilder.Entity<UserRole>();
            userRoles.ToTable("UserRoles");

            EntityTypeConfiguration<User> users = modelBuilder.Entity<User>();
            users.ToTable("Users");
            users.HasOptional(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            // DO NOT FOREIGN
            //users.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);
            users.Property(u => u.UserName).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UserNameIndex")));
            users.Ignore(q => q.PhoneNumber);
            users.Ignore(q => q.PhoneNumberConfirmed);
            users.Ignore(q => q.TwoFactorEnabled);

            EntityTypeConfiguration<UserTrail> userTrails = modelBuilder.Entity<UserTrail>();
            // DO NOT FOREIGN
            //userTrails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<Module> modules = modelBuilder.Entity<Module>();
            modules.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            modules.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<AccessGroup> accessGroups = modelBuilder.Entity<AccessGroup>();
            accessGroups.HasRequired(q => q.Department).WithMany().HasForeignKey(q => q.DepartmentId);
            accessGroups.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            accessGroups.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<AccessMatrix> accessMatrices = modelBuilder.Entity<AccessMatrix>();
            accessMatrices.HasRequired(q => q.AccessGroup).WithMany().HasForeignKey(q => q.AccessGroupId).WillCascadeOnDelete(false);
            accessMatrices.HasRequired(q => q.Module).WithMany().HasForeignKey(q => q.ModuleId).WillCascadeOnDelete(false);

            EntityTypeConfiguration<Benefit> benefits = modelBuilder.Entity<Benefit>();
            benefits.HasOptional(q => q.ValuationBenefitCodePickListDetail).WithMany().HasForeignKey(q => q.ValuationBenefitCodePickListDetailId).WillCascadeOnDelete(false);
            benefits.HasOptional(q => q.BenefitCategoryPickListDetail).WithMany().HasForeignKey(q => q.BenefitCategoryPickListDetailId).WillCascadeOnDelete(false);
            benefits.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            benefits.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<BenefitDetail> benefitDetails = modelBuilder.Entity<BenefitDetail>();
            benefitDetails.HasRequired(q => q.Benefit).WithMany(q => q.BenefitDetails).HasForeignKey(q => q.BenefitId).WillCascadeOnDelete(false);
            benefitDetails.HasRequired(q => q.ClaimCode).WithMany().HasForeignKey(q => q.ClaimCodeId).WillCascadeOnDelete(false);
            benefitDetails.HasRequired(q => q.EventCode).WithMany().HasForeignKey(q => q.EventCodeId).WillCascadeOnDelete(false);
            benefitDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            benefitDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<Cedant> cedants = modelBuilder.Entity<Cedant>();
            cedants.HasOptional(q => q.CedingCompanyTypePickListDetail).WithMany().HasForeignKey(q => q.CedingCompanyTypePickListDetailId).WillCascadeOnDelete(false);
            cedants.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            cedants.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<CedantWorkgroup> cedantWorkgroups = modelBuilder.Entity<CedantWorkgroup>();
            cedantWorkgroups.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            cedantWorkgroups.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<CedantWorkgroupCedant> cedantWorkgroupCedants = modelBuilder.Entity<CedantWorkgroupCedant>();
            cedantWorkgroupCedants.HasRequired(q => q.CedantWorkgroup).WithMany().HasForeignKey(q => q.CedantWorkgroupId).WillCascadeOnDelete(false);
            cedantWorkgroupCedants.HasRequired(q => q.Cedant).WithMany().HasForeignKey(q => q.CedantId).WillCascadeOnDelete(false);

            EntityTypeConfiguration<CedantWorkgroupUser> cedantWorkgroupUsers = modelBuilder.Entity<CedantWorkgroupUser>();
            cedantWorkgroupUsers.HasRequired(q => q.CedantWorkgroup).WithMany().HasForeignKey(q => q.CedantWorkgroupId).WillCascadeOnDelete(false);
            cedantWorkgroupUsers.HasRequired(q => q.User).WithMany().HasForeignKey(q => q.UserId).WillCascadeOnDelete(false);

            EntityTypeConfiguration<Department> departments = modelBuilder.Entity<Department>();
            departments.HasOptional(q => q.HodUser).WithMany().HasForeignKey(q => q.HodUserId).WillCascadeOnDelete(false);
            departments.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            departments.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RiDataConfig> riDataConfigs = modelBuilder.Entity<RiDataConfig>();
            riDataConfigs.HasRequired(q => q.Cedant).WithMany().HasForeignKey(q => q.CedantId).WillCascadeOnDelete(false);
            riDataConfigs.HasOptional(q => q.Treaty).WithMany().HasForeignKey(q => q.TreatyId).WillCascadeOnDelete(false);
            riDataConfigs.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            riDataConfigs.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RiDataMapping> riDataMappings = modelBuilder.Entity<RiDataMapping>();
            riDataMappings.HasRequired(q => q.RiDataConfig).WithMany().HasForeignKey(q => q.RiDataConfigId).WillCascadeOnDelete(false);
            riDataMappings.HasRequired(q => q.StandardOutput).WithMany().HasForeignKey(q => q.StandardOutputId).WillCascadeOnDelete(false);
            riDataMappings.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            riDataMappings.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RiDataMappingDetail> riDataMappingDetails = modelBuilder.Entity<RiDataMappingDetail>();
            riDataMappingDetails.HasRequired(q => q.RiDataMapping).WithMany().HasForeignKey(q => q.RiDataMappingId).WillCascadeOnDelete(false);
            riDataMappingDetails.HasOptional(q => q.PickListDetail).WithMany().HasForeignKey(q => q.PickListDetailId).WillCascadeOnDelete(false);
            riDataMappingDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            riDataMappingDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PickList> pickLists = modelBuilder.Entity<PickList>();
            pickLists.HasRequired(q => q.Department).WithMany().HasForeignKey(q => q.DepartmentId).WillCascadeOnDelete(false);
            pickLists.HasOptional(q => q.StandardOutput).WithMany().HasForeignKey(q => q.StandardOutputId).WillCascadeOnDelete(false);
            pickLists.HasOptional(q => q.StandardClaimDataOutput).WithMany().HasForeignKey(q => q.StandardClaimDataOutputId).WillCascadeOnDelete(false);
            pickLists.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            pickLists.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PickListDetail> pickListDetails = modelBuilder.Entity<PickListDetail>();
            pickListDetails.HasRequired(q => q.PickList).WithMany().HasForeignKey(q => q.PickListId).WillCascadeOnDelete(false);
            pickListDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            pickListDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RawFile> rawFiles = modelBuilder.Entity<RawFile>();
            rawFiles.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            rawFiles.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RiDataBatch> riDataBatches = modelBuilder.Entity<RiDataBatch>();
            riDataBatches.HasRequired(q => q.Cedant).WithMany().HasForeignKey(q => q.CedantId).WillCascadeOnDelete(false);
            riDataBatches.HasOptional(q => q.Treaty).WithMany().HasForeignKey(q => q.TreatyId).WillCascadeOnDelete(false);
            riDataBatches.HasRequired(q => q.RiDataConfig).WithMany().HasForeignKey(q => q.RiDataConfigId).WillCascadeOnDelete(false);
            riDataBatches.HasOptional(q => q.SoaDataBatch).WithMany().HasForeignKey(q => q.SoaDataBatchId).WillCascadeOnDelete(false);
            riDataBatches.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            riDataBatches.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RiDataBatchStatusFile> riDataBatchStatusFiles = modelBuilder.Entity<RiDataBatchStatusFile>();
            riDataBatchStatusFiles.HasRequired(q => q.RiDataBatch).WithMany().HasForeignKey(q => q.RiDataBatchId).WillCascadeOnDelete(false);
            riDataBatchStatusFiles.HasRequired(q => q.StatusHistory).WithMany().HasForeignKey(q => q.StatusHistoryId).WillCascadeOnDelete(false);
            riDataBatchStatusFiles.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            riDataBatchStatusFiles.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RiDataFile> riDataFiles = modelBuilder.Entity<RiDataFile>();
            riDataFiles.HasRequired(q => q.RiDataBatch).WithMany().HasForeignKey(q => q.RiDataBatchId).WillCascadeOnDelete(false);
            riDataFiles.HasRequired(q => q.RawFile).WithMany().HasForeignKey(q => q.RawFileId).WillCascadeOnDelete(false);
            riDataFiles.HasOptional(q => q.Treaty).WithMany().HasForeignKey(q => q.TreatyId).WillCascadeOnDelete(false);
            riDataFiles.HasOptional(q => q.RiDataConfig).WithMany().HasForeignKey(q => q.RiDataConfigId).WillCascadeOnDelete(false);
            riDataFiles.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            riDataFiles.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RiData> riData = modelBuilder.Entity<RiData>();
            riData.HasOptional(q => q.RiDataBatch).WithMany().HasForeignKey(q => q.RiDataBatchId).WillCascadeOnDelete(false);
            riData.HasOptional(q => q.RiDataFile).WithMany().HasForeignKey(q => q.RiDataFileId).WillCascadeOnDelete(false);
            riData.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            riData.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);
            riData.HasIndex(q => new { q.TreatyCode, q.RiskPeriodMonth, q.RiskPeriodYear, q.TransactionTypeCode, q.PolicyNumber, q.CedingBasicPlanCode, q.CedingBenefitTypeCode, q.CedingBenefitRiskCode, q.InsuredName, q.NetPremium }).HasName("IX_Duplicate");
            riData.HasIndex(q => new { q.PolicyNumber, q.CedingPlanCode, q.MlreBenefitCode, q.TreatyCode, q.RiskPeriodMonth, q.RiskPeriodYear, q.RiderNumber }).HasName("IX_RiDataLookup");

            EntityTypeConfiguration<RiDataComputation> riDataComputations = modelBuilder.Entity<RiDataComputation>();
            riDataComputations.HasRequired(q => q.RiDataConfig).WithMany().HasForeignKey(q => q.RiDataConfigId).WillCascadeOnDelete(false);
            riDataComputations.HasOptional(q => q.StandardOutput).WithMany().HasForeignKey(q => q.StandardOutputId).WillCascadeOnDelete(false);
            riDataComputations.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            riDataComputations.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RiDataPreValidation> riDataPreValidations = modelBuilder.Entity<RiDataPreValidation>();
            riDataPreValidations.HasRequired(q => q.RiDataConfig).WithMany().HasForeignKey(q => q.RiDataConfigId).WillCascadeOnDelete(false);
            riDataPreValidations.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            riDataPreValidations.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<Remark> remarks = modelBuilder.Entity<Remark>();
            remarks.HasRequired(q => q.Module).WithMany().HasForeignKey(q => q.ModuleId).WillCascadeOnDelete(false);
            remarks.HasOptional(q => q.StatusHistory).WithMany().HasForeignKey(q => q.StatusHistoryId).WillCascadeOnDelete(false);
            remarks.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            remarks.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RiDataCorrection> riDataCorrections = modelBuilder.Entity<RiDataCorrection>();
            riDataCorrections.HasRequired(q => q.Cedant).WithMany().HasForeignKey(q => q.CedantId).WillCascadeOnDelete(false);
            riDataCorrections.HasOptional(q => q.TreatyCode).WithMany().HasForeignKey(q => q.TreatyCodeId).WillCascadeOnDelete(false);
            riDataCorrections.HasOptional(q => q.InsuredGenderCodePickListDetail).WithMany().HasForeignKey(q => q.InsuredGenderCodePickListDetailId).WillCascadeOnDelete(false);
            riDataCorrections.HasOptional(q => q.ReinsBasisCodePickListDetail).WithMany().HasForeignKey(q => q.ReinsBasisCodePickListDetailId).WillCascadeOnDelete(false);
            riDataCorrections.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            riDataCorrections.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);
            riDataCorrections.HasIndex(q => new { q.CedantId, q.PolicyNumber, q.InsuredRegisterNo, q.TreatyCodeId }).HasName("IX_DataCorrection");

            EntityTypeConfiguration<RateTable> rateTables = modelBuilder.Entity<RateTable>();
            rateTables.HasOptional(q => q.Benefit).WithMany().HasForeignKey(q => q.BenefitId).WillCascadeOnDelete(false);
            rateTables.HasOptional(q => q.PremiumFrequencyCodePickListDetail).WithMany().HasForeignKey(q => q.PremiumFrequencyCodePickListDetailId).WillCascadeOnDelete(false);
            rateTables.HasOptional(q => q.ReinsBasisCodePickListDetail).WithMany().HasForeignKey(q => q.ReinsBasisCodePickListDetailId).WillCascadeOnDelete(false);
            rateTables.HasOptional(q => q.Rate).WithMany().HasForeignKey(q => q.RateId).WillCascadeOnDelete(false);
            rateTables.HasOptional(q => q.Cedant).WithMany().HasForeignKey(q => q.CedantId).WillCascadeOnDelete(false);
            rateTables.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            rateTables.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);
            rateTables.HasOptional(q => q.RateTableMappingUpload).WithMany().HasForeignKey(q => q.RateTableMappingUploadId).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RateTableDetail> rateTableDetails = modelBuilder.Entity<RateTableDetail>();
            rateTableDetails.HasRequired(q => q.RateTable).WithMany(q => q.RateTableDetails).HasForeignKey(q => q.RateTableId).WillCascadeOnDelete(false);
            rateTableDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            rateTableDetails.HasIndex(q => new { q.TreatyCode, q.CedingPlanCode, q.CedingTreatyCode, q.CedingPlanCode2, q.CedingBenefitTypeCode, q.CedingBenefitRiskCode, q.GroupPolicyNumber, q.RateTableId }).HasName("IX_RateTableMapping");

            EntityTypeConfiguration<Mfrs17CellMapping> mfrs17CellMappings = modelBuilder.Entity<Mfrs17CellMapping>();
            mfrs17CellMappings.HasRequired(q => q.ReinsBasisCodePickListDetail).WithMany().HasForeignKey(q => q.ReinsBasisCodePickListDetailId).WillCascadeOnDelete(false);
            mfrs17CellMappings.HasRequired(q => q.BasicRiderPickListDetail).WithMany().HasForeignKey(q => q.BasicRiderPickListDetailId).WillCascadeOnDelete(false);
            mfrs17CellMappings.HasOptional(q => q.ProfitCommPickListDetail).WithMany().HasForeignKey(q => q.ProfitCommPickListDetailId).WillCascadeOnDelete(false);
            mfrs17CellMappings.HasOptional(q => q.Mfrs17ContractCodeDetail).WithMany().HasForeignKey(q => q.Mfrs17ContractCodeDetailId).WillCascadeOnDelete(false);
            mfrs17CellMappings.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            mfrs17CellMappings.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<Mfrs17CellMappingDetail> mfrs17CellMappingDetails = modelBuilder.Entity<Mfrs17CellMappingDetail>();
            mfrs17CellMappingDetails.HasRequired(q => q.Mfrs17CellMapping).WithMany(q => q.Mfrs17CellMappingDetails).HasForeignKey(q => q.Mfrs17CellMappingId).WillCascadeOnDelete(false);
            mfrs17CellMappingDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            mfrs17CellMappingDetails.HasIndex(q => new { q.CedingPlanCode, q.BenefitCode, q.TreatyCode, q.Mfrs17CellMappingId }).HasName("IX_Mfrs17CellMapping");

            EntityTypeConfiguration<Mfrs17Reporting> mfrs17Reportings = modelBuilder.Entity<Mfrs17Reporting>();
            mfrs17Reportings.HasRequired(q => q.CutOff).WithMany().HasForeignKey(q => q.CutOffId).WillCascadeOnDelete(false);
            mfrs17Reportings.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            mfrs17Reportings.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<Mfrs17ReportingDetail> mfrs17ReportingDetails = modelBuilder.Entity<Mfrs17ReportingDetail>();
            mfrs17ReportingDetails.HasRequired(q => q.Mfrs17Reporting).WithMany().HasForeignKey(q => q.Mfrs17ReportingId).WillCascadeOnDelete(false);
            mfrs17ReportingDetails.HasRequired(q => q.Cedant).WithMany().HasForeignKey(q => q.CedantId).WillCascadeOnDelete(false);
            mfrs17ReportingDetails.HasRequired(q => q.PremiumFrequencyCodePickListDetail).WithMany().HasForeignKey(q => q.PremiumFrequencyCodePickListDetailId).WillCascadeOnDelete(false);
            mfrs17ReportingDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            mfrs17ReportingDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<Mfrs17ReportingDetailRiData> mfrs17ReportingDetailRiDatas = modelBuilder.Entity<Mfrs17ReportingDetailRiData>();
            mfrs17ReportingDetailRiDatas.HasRequired(q => q.Mfrs17ReportingDetail).WithMany().HasForeignKey(q => q.Mfrs17ReportingDetailId).WillCascadeOnDelete(false);
            //mfrs17ReportingDetailRiDatas.HasRequired(q => q.RiDataWarehouseHistory).WithMany().HasForeignKey(q => q.RiDataWarehouseHistoryId).WillCascadeOnDelete(false);

            EntityTypeConfiguration<Document> documents = modelBuilder.Entity<Document>();
            documents.HasRequired(q => q.Module).WithMany().HasForeignKey(q => q.ModuleId).WillCascadeOnDelete(false);
            documents.HasOptional(q => q.Remark).WithMany().HasForeignKey(q => q.RemarkId).WillCascadeOnDelete(false);
            documents.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            documents.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<StandardOutput> standardOutputs = modelBuilder.Entity<StandardOutput>();
            standardOutputs.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            standardOutputs.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<StatusHistory> statusHistories = modelBuilder.Entity<StatusHistory>();
            statusHistories.HasRequired(q => q.Module).WithMany().HasForeignKey(q => q.ModuleId).WillCascadeOnDelete(false);
            statusHistories.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            statusHistories.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<Treaty> treaties = modelBuilder.Entity<Treaty>();
            treaties.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treaties.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);
            treaties.HasRequired(q => q.Cedant).WithMany().HasForeignKey(q => q.CedantId).WillCascadeOnDelete(false);
            treaties.HasOptional(q => q.BusinessOriginPickListDetail).WithMany().HasForeignKey(q => q.BusinessOriginPickListDetailId).WillCascadeOnDelete(false);
            treaties.HasOptional(q => q.LineOfBusinessPickListDetail).WithMany().HasForeignKey(q => q.LineOfBusinessPickListDetailId).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyBenefitCodeMapping> treatyBenefitCodeMappings = modelBuilder.Entity<TreatyBenefitCodeMapping>();
            treatyBenefitCodeMappings.HasRequired(q => q.Cedant).WithMany().HasForeignKey(q => q.CedantId).WillCascadeOnDelete(false);
            treatyBenefitCodeMappings.HasRequired(q => q.TreatyCode).WithMany().HasForeignKey(q => q.TreatyCodeId).WillCascadeOnDelete(false);
            treatyBenefitCodeMappings.HasOptional(q => q.ReinsBasisCodePickListDetail).WithMany().HasForeignKey(q => q.ReinsBasisCodePickListDetailId).WillCascadeOnDelete(false);
            treatyBenefitCodeMappings.HasOptional(q => q.ProfitCommPickListDetail).WithMany().HasForeignKey(q => q.ProfitCommPickListDetailId).WillCascadeOnDelete(false);
            treatyBenefitCodeMappings.HasRequired(q => q.Benefit).WithMany().HasForeignKey(q => q.BenefitId).WillCascadeOnDelete(false);
            treatyBenefitCodeMappings.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyBenefitCodeMappings.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);
            treatyBenefitCodeMappings.HasOptional(q => q.TreatyBenefitCodeMappingUpload).WithMany().HasForeignKey(q => q.TreatyBenefitCodeMappingUploadId).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyBenefitCodeMappingDetail> treatyBenefitCodeMappingDetails = modelBuilder.Entity<TreatyBenefitCodeMappingDetail>();
            treatyBenefitCodeMappingDetails.HasRequired(q => q.TreatyBenefitCodeMapping).WithMany(q => q.TreatyBenefitCodeMappingDetails).HasForeignKey(q => q.TreatyBenefitCodeMappingId).WillCascadeOnDelete(false);
            treatyBenefitCodeMappingDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyBenefitCodeMappingDetails.HasIndex(q => new { q.CedingPlanCode, q.CedingBenefitTypeCode, q.CedingBenefitRiskCode, q.CedingTreatyCode, q.CampaignCode, q.TreatyBenefitCodeMappingId }).HasName("IX_ProductFeatureMapping");

            EntityTypeConfiguration<TreatyCode> treatyCodes = modelBuilder.Entity<TreatyCode>();
            treatyCodes.HasOptional(q => q.TreatyTypePickListDetail).WithMany().HasForeignKey(q => q.TreatyTypePickListDetailId).WillCascadeOnDelete(false);
            treatyCodes.HasOptional(q => q.TreatyStatusPickListDetail).WithMany().HasForeignKey(q => q.TreatyStatusPickListDetailId).WillCascadeOnDelete(false);
            treatyCodes.HasOptional(q => q.LineOfBusinessPickListDetail).WithMany().HasForeignKey(q => q.LineOfBusinessPickListDetailId).WillCascadeOnDelete(false);
            treatyCodes.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyCodes.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);
            treatyCodes.HasRequired(q => q.Treaty).WithMany().HasForeignKey(q => q.TreatyId).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyOldCode> treatyOldCodes = modelBuilder.Entity<TreatyOldCode>();
            treatyOldCodes.HasRequired(q => q.TreatyCode).WithMany().HasForeignKey(q => q.TreatyCodeId).WillCascadeOnDelete(false);
            treatyOldCodes.HasRequired(q => q.OldTreatyCode).WithMany().HasForeignKey(q => q.OldTreatyCodeId).WillCascadeOnDelete(false);

            EntityTypeConfiguration<Export> export = modelBuilder.Entity<Export>();
            export.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            export.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ClaimCode> claimCodes = modelBuilder.Entity<ClaimCode>();
            claimCodes.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            claimCodes.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ClaimDataConfig> claimDataConfigs = modelBuilder.Entity<ClaimDataConfig>();
            claimDataConfigs.HasRequired(q => q.Cedant).WithMany().HasForeignKey(q => q.CedantId).WillCascadeOnDelete(false);
            claimDataConfigs.HasOptional(q => q.Treaty).WithMany().HasForeignKey(q => q.TreatyId).WillCascadeOnDelete(false);
            claimDataConfigs.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            claimDataConfigs.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<EventClaimCodeMapping> eventClaimCodeMappings = modelBuilder.Entity<EventClaimCodeMapping>();
            eventClaimCodeMappings.HasRequired(q => q.Cedant).WithMany().HasForeignKey(q => q.CedantId).WillCascadeOnDelete(false);
            eventClaimCodeMappings.HasRequired(q => q.EventCode).WithMany().HasForeignKey(q => q.EventCodeId).WillCascadeOnDelete(false);
            eventClaimCodeMappings.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            eventClaimCodeMappings.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<EventClaimCodeMappingDetail> eventClaimCodeMappingDetails = modelBuilder.Entity<EventClaimCodeMappingDetail>();
            eventClaimCodeMappingDetails.HasRequired(q => q.EventClaimCodeMapping).WithMany(q => q.EventClaimCodeMappingDetails).HasForeignKey(q => q.EventClaimCodeMappingId).WillCascadeOnDelete(false);
            eventClaimCodeMappingDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<AccountCode> accountCodes = modelBuilder.Entity<AccountCode>();
            accountCodes.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            accountCodes.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<AccountCodeMapping> accountCodeMappings = modelBuilder.Entity<AccountCodeMapping>();
            accountCodeMappings.HasRequired(q => q.AccountCode).WithMany().HasForeignKey(q => q.AccountCodeId).WillCascadeOnDelete(false);
            accountCodeMappings.HasOptional(q => q.TransactionTypeCodePickListDetail).WithMany().HasForeignKey(q => q.TransactionTypeCodePickListDetailId).WillCascadeOnDelete(false);
            accountCodeMappings.HasOptional(q => q.RetroRegisterFieldPickListDetail).WithMany().HasForeignKey(q => q.RetroRegisterFieldPickListDetailId).WillCascadeOnDelete(false);
            accountCodeMappings.HasOptional(q => q.ModifiedContractCode).WithMany().HasForeignKey(q => q.ModifiedContractCodeId).WillCascadeOnDelete(false);
            accountCodeMappings.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            accountCodeMappings.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<AccountCodeMappingDetail> accountCodeMappingDetails = modelBuilder.Entity<AccountCodeMappingDetail>();
            accountCodeMappingDetails.HasRequired(q => q.AccountCodeMapping).WithMany(q => q.AccountCodeMappingDetails).HasForeignKey(q => q.AccountCodeMappingId).WillCascadeOnDelete(false);
            accountCodeMappingDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ClaimDataMapping> claimDataMappings = modelBuilder.Entity<ClaimDataMapping>();
            claimDataMappings.HasRequired(q => q.ClaimDataConfig).WithMany().HasForeignKey(q => q.ClaimDataConfigId).WillCascadeOnDelete(false);
            claimDataMappings.HasRequired(q => q.StandardClaimDataOutput).WithMany().HasForeignKey(q => q.StandardClaimDataOutputId).WillCascadeOnDelete(false);
            claimDataMappings.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            claimDataMappings.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ClaimDataMappingDetail> claimDataMappingDetails = modelBuilder.Entity<ClaimDataMappingDetail>();
            claimDataMappingDetails.HasRequired(q => q.ClaimDataMapping).WithMany().HasForeignKey(q => q.ClaimDataMappingId).WillCascadeOnDelete(false);
            claimDataMappingDetails.HasOptional(q => q.PickListDetail).WithMany().HasForeignKey(q => q.PickListDetailId).WillCascadeOnDelete(false);
            claimDataMappingDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            claimDataMappingDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ClaimDataComputation> claimDataComputations = modelBuilder.Entity<ClaimDataComputation>();
            claimDataComputations.HasRequired(q => q.ClaimDataConfig).WithMany().HasForeignKey(q => q.ClaimDataConfigId).WillCascadeOnDelete(false);
            claimDataComputations.HasRequired(q => q.StandardClaimDataOutput).WithMany().HasForeignKey(q => q.StandardClaimDataOutputId).WillCascadeOnDelete(false);
            claimDataComputations.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            claimDataComputations.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ClaimDataValidation> claimDataValidations = modelBuilder.Entity<ClaimDataValidation>();
            claimDataValidations.HasRequired(q => q.ClaimDataConfig).WithMany().HasForeignKey(q => q.ClaimDataConfigId).WillCascadeOnDelete(false);
            claimDataValidations.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            claimDataValidations.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<Salutation> salutations = modelBuilder.Entity<Salutation>();
            salutations.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            salutations.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<Rate> rates = modelBuilder.Entity<Rate>();
            rates.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            rates.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RateDetail> rateDetails = modelBuilder.Entity<RateDetail>();
            rateDetails.HasRequired(q => q.Rate).WithMany().HasForeignKey(q => q.RateId).WillCascadeOnDelete(false);
            rateDetails.HasOptional(q => q.InsuredGenderCodePickListDetail).WithMany().HasForeignKey(q => q.InsuredGenderCodePickListDetailId).WillCascadeOnDelete(false);
            rateDetails.HasOptional(q => q.CedingTobaccoUsePickListDetail).WithMany().HasForeignKey(q => q.CedingTobaccoUsePickListDetailId).WillCascadeOnDelete(false);
            rateDetails.HasOptional(q => q.CedingOccupationCodePickListDetail).WithMany().HasForeignKey(q => q.CedingOccupationCodePickListDetailId).WillCascadeOnDelete(false);
            rateDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            rateDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);
            rateDetails.HasIndex(q => new { q.InsuredGenderCodePickListDetailId, q.CedingTobaccoUsePickListDetailId, q.CedingOccupationCodePickListDetailId, q.AttainedAge, q.IssueAge, q.PolicyTerm, q.PolicyTermRemain, q.RateId }).HasName("IX_RateMapping");

            EntityTypeConfiguration<ItemCodeMapping> itemCodeMappings = modelBuilder.Entity<ItemCodeMapping>();
            itemCodeMappings.HasRequired(q => q.ItemCode).WithMany().HasForeignKey(q => q.ItemCodeId).WillCascadeOnDelete(false);
            itemCodeMappings.HasOptional(q => q.InvoiceFieldPickListDetail).WithMany().HasForeignKey(q => q.InvoiceFieldPickListDetailId).WillCascadeOnDelete(false);
            itemCodeMappings.HasOptional(q => q.BusinessOriginPickListDetail).WithMany().HasForeignKey(q => q.BusinessOriginPickListDetailId).WillCascadeOnDelete(false);
            itemCodeMappings.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            itemCodeMappings.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ItemCodeMappingDetail> itemCodeMappingDetails = modelBuilder.Entity<ItemCodeMappingDetail>();
            itemCodeMappingDetails.HasRequired(q => q.ItemCodeMapping).WithMany(q => q.ItemCodeMappingDetails).HasForeignKey(q => q.ItemCodeMappingId).WillCascadeOnDelete(false);
            itemCodeMappingDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<AuthorizationLimit> authorizationLimits = modelBuilder.Entity<AuthorizationLimit>();
            authorizationLimits.HasRequired(q => q.AccessGroup).WithMany().HasForeignKey(q => q.AccessGroupId).WillCascadeOnDelete(false);
            authorizationLimits.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            authorizationLimits.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<DiscountTable> discountTables = modelBuilder.Entity<DiscountTable>();
            discountTables.HasRequired(q => q.Cedant).WithMany().HasForeignKey(q => q.CedantId).WillCascadeOnDelete(false);
            discountTables.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            discountTables.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RiDiscount> riDiscounts = modelBuilder.Entity<RiDiscount>();
            riDiscounts.HasRequired(q => q.DiscountTable).WithMany().HasForeignKey(q => q.DiscountTableId).WillCascadeOnDelete(false);
            riDiscounts.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            riDiscounts.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<LargeDiscount> largeDiscounts = modelBuilder.Entity<LargeDiscount>();
            largeDiscounts.HasRequired(q => q.DiscountTable).WithMany().HasForeignKey(q => q.DiscountTableId).WillCascadeOnDelete(false);
            largeDiscounts.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            largeDiscounts.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<GroupDiscount> groupDiscounts = modelBuilder.Entity<GroupDiscount>();
            groupDiscounts.HasRequired(q => q.DiscountTable).WithMany().HasForeignKey(q => q.DiscountTableId).WillCascadeOnDelete(false);
            groupDiscounts.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            groupDiscounts.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ItemCode> itemCodes = modelBuilder.Entity<ItemCode>();
            itemCodes.HasOptional(q => q.BusinessOriginPickListDetail).WithMany().HasForeignKey(q => q.BusinessOriginPickListDetailId).WillCascadeOnDelete(false);
            itemCodes.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            itemCodes.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ClaimCodeMapping> claimCodeMappings = modelBuilder.Entity<ClaimCodeMapping>();
            claimCodeMappings.HasRequired(q => q.ClaimCode).WithMany().HasForeignKey(q => q.ClaimCodeId).WillCascadeOnDelete(false);
            claimCodeMappings.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            claimCodeMappings.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ClaimCodeMappingDetail> claimCodeMappingDetails = modelBuilder.Entity<ClaimCodeMappingDetail>();
            claimCodeMappingDetails.HasRequired(q => q.ClaimCodeMapping).WithMany(q => q.ClaimCodeMappingDetails).HasForeignKey(q => q.ClaimCodeMappingId).WillCascadeOnDelete(false);
            claimCodeMappingDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ClaimDataBatch> claimDataBatches = modelBuilder.Entity<ClaimDataBatch>();
            claimDataBatches.HasRequired(q => q.Cedant).WithMany().HasForeignKey(q => q.CedantId).WillCascadeOnDelete(false);
            claimDataBatches.HasOptional(q => q.Treaty).WithMany().HasForeignKey(q => q.TreatyId).WillCascadeOnDelete(false);
            claimDataBatches.HasRequired(q => q.ClaimDataConfig).WithMany().HasForeignKey(q => q.ClaimDataConfigId).WillCascadeOnDelete(false);
            claimDataBatches.HasOptional(q => q.SoaDataBatch).WithMany().HasForeignKey(q => q.SoaDataBatchId).WillCascadeOnDelete(false);
            claimDataBatches.HasOptional(q => q.ClaimTransactionTypePickListDetail).WithMany().HasForeignKey(q => q.ClaimTransactionTypePickListDetailId).WillCascadeOnDelete(false);
            claimDataBatches.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            claimDataBatches.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ClaimDataFile> claimDataFiles = modelBuilder.Entity<ClaimDataFile>();
            claimDataFiles.HasRequired(q => q.ClaimDataBatch).WithMany().HasForeignKey(q => q.ClaimDataBatchId).WillCascadeOnDelete(false);
            claimDataFiles.HasRequired(q => q.RawFile).WithMany().HasForeignKey(q => q.RawFileId).WillCascadeOnDelete(false);
            claimDataFiles.HasOptional(q => q.Treaty).WithMany().HasForeignKey(q => q.TreatyId).WillCascadeOnDelete(false);
            claimDataFiles.HasOptional(q => q.ClaimDataConfig).WithMany().HasForeignKey(q => q.ClaimDataConfigId).WillCascadeOnDelete(false);
            claimDataFiles.HasOptional(q => q.CurrencyCode).WithMany().HasForeignKey(q => q.CurrencyCodeId).WillCascadeOnDelete(false);
            claimDataFiles.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            claimDataFiles.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ClaimDataBatchStatusFile> claimDataBatchStatusFiles = modelBuilder.Entity<ClaimDataBatchStatusFile>();
            claimDataBatchStatusFiles.HasRequired(q => q.ClaimDataBatch).WithMany().HasForeignKey(q => q.ClaimDataBatchId).WillCascadeOnDelete(false);
            claimDataBatchStatusFiles.HasRequired(q => q.StatusHistory).WithMany().HasForeignKey(q => q.StatusHistoryId).WillCascadeOnDelete(false);
            claimDataBatchStatusFiles.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            claimDataBatchStatusFiles.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<AnnuityFactor> annuityFactors = modelBuilder.Entity<AnnuityFactor>();
            annuityFactors.HasRequired(q => q.Cedant).WithMany().HasForeignKey(q => q.CedantId).WillCascadeOnDelete(false);
            annuityFactors.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            annuityFactors.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<AnnuityFactorDetail> annuityFactorDetails = modelBuilder.Entity<AnnuityFactorDetail>();
            annuityFactorDetails.HasRequired(q => q.AnnuityFactor).WithMany().HasForeignKey(q => q.AnnuityFactorId).WillCascadeOnDelete(false);
            annuityFactorDetails.HasOptional(q => q.InsuredGenderCodePickListDetail).WithMany().HasForeignKey(q => q.InsuredGenderCodePickListDetailId).WillCascadeOnDelete(false);
            annuityFactorDetails.HasOptional(q => q.InsuredTobaccoUsePickListDetail).WithMany().HasForeignKey(q => q.InsuredTobaccoUsePickListDetailId).WillCascadeOnDelete(false);
            annuityFactorDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            annuityFactorDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);
            annuityFactorDetails.HasIndex(q => new { q.PolicyTermRemain, q.InsuredGenderCodePickListDetailId, q.InsuredTobaccoUsePickListDetailId, q.InsuredAttainedAge, q.PolicyTerm, q.AnnuityFactorId }).HasName("IX_AnnuityMapping");

            EntityTypeConfiguration<AnnuityFactorMapping> annuityFactorMappings = modelBuilder.Entity<AnnuityFactorMapping>();
            annuityFactorMappings.HasRequired(q => q.AnnuityFactor).WithMany(q => q.AnnuityFactorMappings).HasForeignKey(q => q.AnnuityFactorId).WillCascadeOnDelete(false);
            annuityFactorMappings.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            annuityFactorMappings.HasIndex(q => new { q.CedingPlanCode, q.AnnuityFactorId }).HasName("IX_AnnuityFactorMapping");

            EntityTypeConfiguration<EventCode> eventCodes = modelBuilder.Entity<EventCode>();
            eventCodes.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            eventCodes.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<FacMasterListing> facMasterListings = modelBuilder.Entity<FacMasterListing>();
            facMasterListings.HasOptional(q => q.InsuredGenderCodePickListDetail).WithMany().HasForeignKey(q => q.InsuredGenderCodePickListDetailId).WillCascadeOnDelete(false);
            facMasterListings.HasOptional(q => q.Cedant).WithMany().HasForeignKey(q => q.CedantId).WillCascadeOnDelete(false);
            facMasterListings.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            facMasterListings.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<FacMasterListingDetail> facMasterListingDetails = modelBuilder.Entity<FacMasterListingDetail>();
            facMasterListingDetails.HasRequired(q => q.FacMasterListing).WithMany(q => q.FacMasterListingDetails).HasForeignKey(q => q.FacMasterListingId).WillCascadeOnDelete(false);
            facMasterListingDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            facMasterListingDetails.HasIndex(q => new { q.PolicyNumber, q.BenefitCode, q.FacMasterListingId }).HasName("IX_FacMasterListingMapping");

            EntityTypeConfiguration<ClaimAuthorityLimitCedant> claimAuthorityLimitCedants = modelBuilder.Entity<ClaimAuthorityLimitCedant>();
            claimAuthorityLimitCedants.HasRequired(q => q.Cedant).WithMany().HasForeignKey(q => q.CedantId).WillCascadeOnDelete(false);
            claimAuthorityLimitCedants.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            claimAuthorityLimitCedants.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ClaimAuthorityLimitCedantDetail> claimAuthorityLimitCedantDetails = modelBuilder.Entity<ClaimAuthorityLimitCedantDetail>();
            claimAuthorityLimitCedantDetails.HasRequired(q => q.ClaimAuthorityLimitCedant).WithMany(q => q.ClaimAuthorityLimitCedantDetails).HasForeignKey(q => q.ClaimAuthorityLimitCedantId).WillCascadeOnDelete(false);
            claimAuthorityLimitCedantDetails.HasRequired(q => q.ClaimCode).WithMany().HasForeignKey(q => q.ClaimCodeId).WillCascadeOnDelete(false);
            claimAuthorityLimitCedantDetails.HasRequired(q => q.FundsAccountingTypePickListDetail).WithMany().HasForeignKey(q => q.FundsAccountingTypePickListDetailId).WillCascadeOnDelete(false);
            claimAuthorityLimitCedantDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ClaimChecklist> claimChecklists = modelBuilder.Entity<ClaimChecklist>();
            claimChecklists.HasRequired(q => q.ClaimCode).WithMany().HasForeignKey(q => q.ClaimCodeId).WillCascadeOnDelete(false);
            claimChecklists.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            claimChecklists.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ClaimChecklistDetail> claimChecklistDetails = modelBuilder.Entity<ClaimChecklistDetail>();
            claimChecklistDetails.HasRequired(q => q.ClaimChecklist).WithMany().HasForeignKey(q => q.ClaimChecklistId).WillCascadeOnDelete(false);
            claimChecklistDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            claimChecklistDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ClaimAuthorityLimitMLRe> claimAuthorityLimitMLRe = modelBuilder.Entity<ClaimAuthorityLimitMLRe>();
            claimAuthorityLimitMLRe.HasRequired(q => q.Department).WithMany().HasForeignKey(q => q.DepartmentId).WillCascadeOnDelete(false);
            claimAuthorityLimitMLRe.HasRequired(q => q.User).WithMany().HasForeignKey(q => q.UserId).WillCascadeOnDelete(false);
            claimAuthorityLimitMLRe.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            claimAuthorityLimitMLRe.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ClaimAuthorityLimitMLReDetail> claimAuthorityLimitMLReDetails = modelBuilder.Entity<ClaimAuthorityLimitMLReDetail>();
            claimAuthorityLimitMLReDetails.HasRequired(q => q.ClaimAuthorityLimitMLRe).WithMany(q => q.ClaimAuthorityLimitMLReDetails).HasForeignKey(q => q.ClaimAuthorityLimitMLReId).WillCascadeOnDelete(false);
            claimAuthorityLimitMLReDetails.HasRequired(q => q.ClaimCode).WithMany().HasForeignKey(q => q.ClaimCodeId).WillCascadeOnDelete(false);
            claimAuthorityLimitMLReDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ClaimCategory> claimCategories = modelBuilder.Entity<ClaimCategory>();
            claimCategories.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            claimCategories.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ClaimReason> claimReasons = modelBuilder.Entity<ClaimReason>();
            claimReasons.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            claimReasons.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<StandardClaimDataOutput> standardClaimDataOutputs = modelBuilder.Entity<StandardClaimDataOutput>();
            standardClaimDataOutputs.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            standardClaimDataOutputs.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ClaimData> claimData = modelBuilder.Entity<ClaimData>();
            claimData.HasRequired(q => q.ClaimDataBatch).WithMany().HasForeignKey(q => q.ClaimDataBatchId).WillCascadeOnDelete(false);
            claimData.HasOptional(q => q.ClaimDataFile).WithMany().HasForeignKey(q => q.ClaimDataFileId).WillCascadeOnDelete(false);
            claimData.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            claimData.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PublicHoliday> publicHolidays = modelBuilder.Entity<PublicHoliday>();
            publicHolidays.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            publicHolidays.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PublicHolidayDetail> publicHolidayDetails = modelBuilder.Entity<PublicHolidayDetail>();
            publicHolidayDetails.HasRequired(q => q.PublicHoliday).WithMany().HasForeignKey(q => q.PublicHolidayId).WillCascadeOnDelete(false);
            publicHolidayDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            publicHolidayDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ClaimRegister> claimRegister = modelBuilder.Entity<ClaimRegister>();
            claimRegister.HasOptional(q => q.ClaimDataBatch).WithMany().HasForeignKey(q => q.ClaimDataBatchId).WillCascadeOnDelete(false);
            claimRegister.HasOptional(q => q.ClaimData).WithMany().HasForeignKey(q => q.ClaimDataId).WillCascadeOnDelete(false);
            claimRegister.HasOptional(q => q.ClaimDataConfig).WithMany().HasForeignKey(q => q.ClaimDataConfigId).WillCascadeOnDelete(false);
            claimRegister.HasOptional(q => q.SoaDataBatch).WithMany().HasForeignKey(q => q.SoaDataBatchId).WillCascadeOnDelete(false);
            claimRegister.HasOptional(q => q.RiDataWarehouse).WithMany().HasForeignKey(q => q.RiDataWarehouseId).WillCascadeOnDelete(false);
            claimRegister.HasOptional(q => q.ReferralRiData).WithMany().HasForeignKey(q => q.ReferralRiDataId).WillCascadeOnDelete(false);
            claimRegister.HasOptional(q => q.ReferralClaim).WithMany().HasForeignKey(q => q.ReferralClaimId).WillCascadeOnDelete(false);
            claimRegister.HasOptional(q => q.OriginalClaimRegister).WithMany().HasForeignKey(q => q.OriginalClaimRegisterId).WillCascadeOnDelete(false);
            claimRegister.HasOptional(q => q.ClaimReason).WithMany().HasForeignKey(q => q.ClaimReasonId).WillCascadeOnDelete(false);
            claimRegister.HasOptional(q => q.PicClaim).WithMany().HasForeignKey(q => q.PicClaimId).WillCascadeOnDelete(false);
            claimRegister.HasOptional(q => q.PicDaa).WithMany().HasForeignKey(q => q.PicDaaId).WillCascadeOnDelete(false);
            claimRegister.HasOptional(q => q.ClaimAssessor).WithMany().HasForeignKey(q => q.ClaimAssessorId).WillCascadeOnDelete(false);
            claimRegister.HasOptional(q => q.CeoClaimReason).WithMany().HasForeignKey(q => q.CeoClaimReasonId).WillCascadeOnDelete(false);
            claimRegister.HasOptional(q => q.SignOffBy).WithMany().HasForeignKey(q => q.SignOffById).WillCascadeOnDelete(false);
            claimRegister.HasOptional(q => q.ClaimCommitteeUser1).WithMany().HasForeignKey(q => q.ClaimCommitteeUser1Id).WillCascadeOnDelete(false);
            claimRegister.HasOptional(q => q.UpdatedOnBehalfBy).WithMany().HasForeignKey(q => q.UpdatedOnBehalfById).WillCascadeOnDelete(false);
            claimRegister.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            claimRegister.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ClaimRegisterHistory> claimRegisterHistories = modelBuilder.Entity<ClaimRegisterHistory>();
            claimRegisterHistories.HasRequired(q => q.CutOff).WithMany().HasForeignKey(q => q.CutOffId).WillCascadeOnDelete(false);
            claimRegisterHistories.HasRequired(q => q.ClaimRegister).WithMany().HasForeignKey(q => q.ClaimRegisterId).WillCascadeOnDelete(false);
            claimRegisterHistories.HasOptional(q => q.ClaimDataBatch).WithMany().HasForeignKey(q => q.ClaimDataBatchId).WillCascadeOnDelete(false);
            claimRegisterHistories.HasOptional(q => q.ClaimData).WithMany().HasForeignKey(q => q.ClaimDataId).WillCascadeOnDelete(false);
            claimRegisterHistories.HasOptional(q => q.ClaimDataConfig).WithMany().HasForeignKey(q => q.ClaimDataConfigId).WillCascadeOnDelete(false);
            claimRegisterHistories.HasOptional(q => q.SoaDataBatch).WithMany().HasForeignKey(q => q.SoaDataBatchId).WillCascadeOnDelete(false);
            claimRegisterHistories.HasOptional(q => q.RiDataWarehouse).WithMany().HasForeignKey(q => q.RiDataWarehouseId).WillCascadeOnDelete(false);
            claimRegisterHistories.HasOptional(q => q.ReferralRiData).WithMany().HasForeignKey(q => q.ReferralRiDataId).WillCascadeOnDelete(false);
            claimRegisterHistories.HasOptional(q => q.ReferralClaim).WithMany().HasForeignKey(q => q.ReferralClaimId).WillCascadeOnDelete(false);
            claimRegisterHistories.HasOptional(q => q.OriginalClaimRegister).WithMany().HasForeignKey(q => q.OriginalClaimRegisterId).WillCascadeOnDelete(false);
            claimRegisterHistories.HasOptional(q => q.ClaimReason).WithMany().HasForeignKey(q => q.ClaimReasonId).WillCascadeOnDelete(false);
            claimRegisterHistories.HasOptional(q => q.PicClaim).WithMany().HasForeignKey(q => q.PicClaimId).WillCascadeOnDelete(false);
            claimRegisterHistories.HasOptional(q => q.PicDaa).WithMany().HasForeignKey(q => q.PicDaaId).WillCascadeOnDelete(false);
            claimRegisterHistories.HasOptional(q => q.ClaimAssessor).WithMany().HasForeignKey(q => q.ClaimAssessorId).WillCascadeOnDelete(false);
            claimRegisterHistories.HasOptional(q => q.CeoClaimReason).WithMany().HasForeignKey(q => q.CeoClaimReasonId).WillCascadeOnDelete(false);
            claimRegisterHistories.HasOptional(q => q.SignOffBy).WithMany().HasForeignKey(q => q.SignOffById).WillCascadeOnDelete(false);
            claimRegisterHistories.HasOptional(q => q.ClaimCommitteeUser1).WithMany().HasForeignKey(q => q.ClaimCommitteeUser1Id).WillCascadeOnDelete(false);
            claimRegisterHistories.HasOptional(q => q.UpdatedOnBehalfBy).WithMany().HasForeignKey(q => q.UpdatedOnBehalfById).WillCascadeOnDelete(false);
            claimRegisterHistories.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            claimRegisterHistories.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<SoaDataBatch> soaDataBatches = modelBuilder.Entity<SoaDataBatch>();
            soaDataBatches.HasRequired(q => q.Cedant).WithMany().HasForeignKey(q => q.CedantId).WillCascadeOnDelete(false);
            soaDataBatches.HasOptional(q => q.Treaty).WithMany().HasForeignKey(q => q.TreatyId).WillCascadeOnDelete(false);
            soaDataBatches.HasOptional(q => q.CurrencyCodePickListDetail).WithMany().HasForeignKey(q => q.CurrencyCodePickListDetailId).WillCascadeOnDelete(false);
            soaDataBatches.HasOptional(q => q.RiDataBatch).WithMany().HasForeignKey(q => q.RiDataBatchId).WillCascadeOnDelete(false);
            soaDataBatches.HasOptional(q => q.ClaimDataBatch).WithMany().HasForeignKey(q => q.ClaimDataBatchId).WillCascadeOnDelete(false);
            soaDataBatches.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            soaDataBatches.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<SoaDataBatchStatusFile> soaDataBatchStatusFiles = modelBuilder.Entity<SoaDataBatchStatusFile>();
            soaDataBatchStatusFiles.HasRequired(q => q.SoaDataBatch).WithMany().HasForeignKey(q => q.SoaDataBatchId).WillCascadeOnDelete(false);
            soaDataBatchStatusFiles.HasRequired(q => q.StatusHistory).WithMany().HasForeignKey(q => q.StatusHistoryId).WillCascadeOnDelete(false);
            soaDataBatchStatusFiles.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            soaDataBatchStatusFiles.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<SoaDataFile> soaDataFiles = modelBuilder.Entity<SoaDataFile>();
            soaDataFiles.HasRequired(q => q.SoaDataBatch).WithMany().HasForeignKey(q => q.SoaDataBatchId).WillCascadeOnDelete(false);
            soaDataFiles.HasRequired(q => q.RawFile).WithMany().HasForeignKey(q => q.RawFileId).WillCascadeOnDelete(false);
            soaDataFiles.HasOptional(q => q.Treaty).WithMany().HasForeignKey(q => q.TreatyId).WillCascadeOnDelete(false);
            soaDataFiles.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            soaDataFiles.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<SoaData> soaData = modelBuilder.Entity<SoaData>();
            soaData.HasRequired(q => q.SoaDataBatch).WithMany().HasForeignKey(q => q.SoaDataBatchId).WillCascadeOnDelete(false);
            soaData.HasOptional(q => q.SoaDataFile).WithMany().HasForeignKey(q => q.SoaDataFileId).WillCascadeOnDelete(false);
            soaData.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            soaData.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<SoaDataRiDataSummary> soaDataRiDataSummaries = modelBuilder.Entity<SoaDataRiDataSummary>();
            soaDataRiDataSummaries.HasRequired(q => q.SoaDataBatch).WithMany().HasForeignKey(q => q.SoaDataBatchId).WillCascadeOnDelete(false);
            soaDataRiDataSummaries.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            soaDataRiDataSummaries.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<SoaDataPostValidation> soaDataPostValidations = modelBuilder.Entity<SoaDataPostValidation>();
            soaDataPostValidations.HasRequired(q => q.SoaDataBatch).WithMany().HasForeignKey(q => q.SoaDataBatchId).WillCascadeOnDelete(false);
            soaDataPostValidations.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            soaDataPostValidations.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<SoaDataPostValidationDifference> SoaDataPostValidationDifferences = modelBuilder.Entity<SoaDataPostValidationDifference>();
            SoaDataPostValidationDifferences.HasRequired(q => q.SoaDataBatch).WithMany().HasForeignKey(q => q.SoaDataBatchId).WillCascadeOnDelete(false);
            SoaDataPostValidationDifferences.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            SoaDataPostValidationDifferences.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<SoaDataCompiledSummary> soaDataCompiledSummaries = modelBuilder.Entity<SoaDataCompiledSummary>();
            soaDataCompiledSummaries.HasRequired(q => q.SoaDataBatch).WithMany().HasForeignKey(q => q.SoaDataBatchId).WillCascadeOnDelete(false);
            soaDataCompiledSummaries.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            soaDataCompiledSummaries.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<SoaDataDiscrepancy> soaDataDiscrepancies = modelBuilder.Entity<SoaDataDiscrepancy>();
            soaDataDiscrepancies.HasRequired(q => q.SoaDataBatch).WithMany().HasForeignKey(q => q.SoaDataBatchId).WillCascadeOnDelete(false);
            soaDataDiscrepancies.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            soaDataDiscrepancies.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<StandardSoaDataOutput> standardSoaDataOutputs = modelBuilder.Entity<StandardSoaDataOutput>();
            standardSoaDataOutputs.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            standardSoaDataOutputs.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RetroParty> retroParties = modelBuilder.Entity<RetroParty>();
            retroParties.HasRequired(q => q.CountryCodePickListDetail).WithMany().HasForeignKey(q => q.CountryCodePickListDetailId).WillCascadeOnDelete(false);
            retroParties.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            retroParties.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RiDataWarehouse> riDataWarehouse = modelBuilder.Entity<RiDataWarehouse>();
            riDataWarehouse.HasOptional(q => q.EndingPolicyStatusPickListDetail).WithMany().HasForeignKey(q => q.EndingPolicyStatus).WillCascadeOnDelete(false);
            riDataWarehouse.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            riDataWarehouse.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PremiumSpreadTable> premiumSpreadTables = modelBuilder.Entity<PremiumSpreadTable>();
            premiumSpreadTables.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            premiumSpreadTables.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PremiumSpreadTableDetail> premiumSpreadTableDetails = modelBuilder.Entity<PremiumSpreadTableDetail>();
            premiumSpreadTableDetails.HasRequired(q => q.PremiumSpreadTable).WithMany().HasForeignKey(q => q.PremiumSpreadTableId).WillCascadeOnDelete(false);
            premiumSpreadTableDetails.HasOptional(q => q.Benefit).WithMany().HasForeignKey(q => q.BenefitId).WillCascadeOnDelete(false);
            premiumSpreadTableDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            premiumSpreadTableDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyDiscountTable> treatyDiscountTables = modelBuilder.Entity<TreatyDiscountTable>();
            treatyDiscountTables.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyDiscountTables.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyDiscountTableDetail> treatyDiscountTableDetails = modelBuilder.Entity<TreatyDiscountTableDetail>();
            treatyDiscountTableDetails.HasRequired(q => q.TreatyDiscountTable).WithMany().HasForeignKey(q => q.TreatyDiscountTableId).WillCascadeOnDelete(false);
            treatyDiscountTableDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyDiscountTableDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<DirectRetroConfiguration> directRetroConfigurations = modelBuilder.Entity<DirectRetroConfiguration>();
            directRetroConfigurations.HasRequired(q => q.TreatyCode).WithMany().HasForeignKey(q => q.TreatyCodeId).WillCascadeOnDelete(false);
            directRetroConfigurations.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            directRetroConfigurations.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<DirectRetroConfigurationMapping> directRetroConfigurationMappings = modelBuilder.Entity<DirectRetroConfigurationMapping>();
            directRetroConfigurationMappings.HasRequired(q => q.DirectRetroConfiguration).WithMany(q => q.DirectRetroConfigurationMappings).HasForeignKey(q => q.DirectRetroConfigurationId).WillCascadeOnDelete(false);
            directRetroConfigurationMappings.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<DirectRetroConfigurationDetail> directRetroConfigurationDetails = modelBuilder.Entity<DirectRetroConfigurationDetail>();
            directRetroConfigurationDetails.HasRequired(q => q.DirectRetroConfiguration).WithMany().HasForeignKey(q => q.DirectRetroConfigurationId).WillCascadeOnDelete(false);
            directRetroConfigurationDetails.HasRequired(q => q.RetroParty).WithMany().HasForeignKey(q => q.RetroPartyId).WillCascadeOnDelete(false);
            directRetroConfigurationDetails.HasOptional(q => q.PremiumSpreadTable).WithMany().HasForeignKey(q => q.PremiumSpreadTableId).WillCascadeOnDelete(false);
            directRetroConfigurationDetails.HasOptional(q => q.TreatyDiscountTable).WithMany().HasForeignKey(q => q.TreatyDiscountTableId).WillCascadeOnDelete(false);
            directRetroConfigurationDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<InvoiceRegisterBatch> invoiceRegisterBatches = modelBuilder.Entity<InvoiceRegisterBatch>();
            invoiceRegisterBatches.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            invoiceRegisterBatches.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<InvoiceRegisterBatchFile> invoiceRegisterBatchFiles = modelBuilder.Entity<InvoiceRegisterBatchFile>();
            invoiceRegisterBatchFiles.HasRequired(q => q.InvoiceRegisterBatch).WithMany().HasForeignKey(q => q.InvoiceRegisterBatchId).WillCascadeOnDelete(false);
            invoiceRegisterBatchFiles.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            invoiceRegisterBatchFiles.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<InvoiceRegisterBatchSoaData> invoiceRegisterBatchSoaDatas = modelBuilder.Entity<InvoiceRegisterBatchSoaData>();
            invoiceRegisterBatchSoaDatas.HasRequired(q => q.InvoiceRegisterBatch).WithMany().HasForeignKey(q => q.InvoiceRegisterBatchId).WillCascadeOnDelete(false);
            invoiceRegisterBatchSoaDatas.HasRequired(q => q.SoaDataBatch).WithMany().HasForeignKey(q => q.SoaDataBatchId).WillCascadeOnDelete(false);

            EntityTypeConfiguration<InvoiceRegisterBatchRemark> invoiceRegisterBatchRemarks = modelBuilder.Entity<InvoiceRegisterBatchRemark>();
            invoiceRegisterBatchRemarks.HasRequired(q => q.InvoiceRegisterBatch).WithMany().HasForeignKey(q => q.InvoiceRegisterBatchId).WillCascadeOnDelete(false);
            invoiceRegisterBatchRemarks.HasOptional(q => q.FollowUpUser).WithMany().HasForeignKey(q => q.FollowUpUserId).WillCascadeOnDelete(false);
            invoiceRegisterBatchRemarks.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            invoiceRegisterBatchRemarks.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<InvoiceRegisterBatchRemarkDocument> invoiceRegisterBatchRemarkDocuments = modelBuilder.Entity<InvoiceRegisterBatchRemarkDocument>();
            invoiceRegisterBatchRemarkDocuments.HasRequired(q => q.InvoiceRegisterBatchRemark).WithMany(q => q.InvoiceRegisterBatchRemarkDocuments).HasForeignKey(q => q.InvoiceRegisterBatchRemarkId).WillCascadeOnDelete(false);
            invoiceRegisterBatchRemarkDocuments.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            invoiceRegisterBatchRemarkDocuments.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<InvoiceRegister> invoiceRegisters = modelBuilder.Entity<InvoiceRegister>();
            invoiceRegisters.HasRequired(q => q.InvoiceRegisterBatch).WithMany().HasForeignKey(q => q.InvoiceRegisterBatchId).WillCascadeOnDelete(false);
            invoiceRegisters.HasOptional(q => q.Cedant).WithMany().HasForeignKey(q => q.CedantId).WillCascadeOnDelete(false);
            invoiceRegisters.HasOptional(q => q.TreatyCode).WithMany().HasForeignKey(q => q.TreatyCodeId).WillCascadeOnDelete(false);
            invoiceRegisters.HasOptional(q => q.SoaDataBatch).WithMany().HasForeignKey(q => q.SoaDataBatchId).WillCascadeOnDelete(false);
            invoiceRegisters.HasOptional(q => q.PreparedBy).WithMany().HasForeignKey(q => q.PreparedById).WillCascadeOnDelete(false);
            invoiceRegisters.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            invoiceRegisters.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<InvoiceRegisterHistory> invoiceRegisterHistories = modelBuilder.Entity<InvoiceRegisterHistory>();
            invoiceRegisterHistories.HasRequired(q => q.CutOff).WithMany().HasForeignKey(q => q.CutOffId).WillCascadeOnDelete(false);
            invoiceRegisterHistories.HasOptional(q => q.Cedant).WithMany().HasForeignKey(q => q.CedantId).WillCascadeOnDelete(false);
            invoiceRegisterHistories.HasOptional(q => q.TreatyCode).WithMany().HasForeignKey(q => q.TreatyCodeId).WillCascadeOnDelete(false);
            invoiceRegisterHistories.HasOptional(q => q.PreparedBy).WithMany().HasForeignKey(q => q.PreparedById).WillCascadeOnDelete(false);
            invoiceRegisterHistories.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            invoiceRegisterHistories.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<InvoiceRegisterValuation> invoiceRegisterValuations = modelBuilder.Entity<InvoiceRegisterValuation>();
            invoiceRegisterValuations.HasRequired(q => q.InvoiceRegister).WithMany().HasForeignKey(q => q.InvoiceRegisterId).WillCascadeOnDelete(false);
            invoiceRegisterValuations.HasRequired(q => q.ValuationBenefitCode).WithMany().HasForeignKey(q => q.ValuationBenefitCodeId).WillCascadeOnDelete(false);
            invoiceRegisterValuations.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            invoiceRegisterValuations.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<DirectRetro> directRetro = modelBuilder.Entity<DirectRetro>();
            directRetro.HasRequired(q => q.Cedant).WithMany().HasForeignKey(q => q.CedantId).WillCascadeOnDelete(false);
            directRetro.HasRequired(q => q.TreatyCode).WithMany().HasForeignKey(q => q.TreatyCodeId).WillCascadeOnDelete(false);
            directRetro.HasRequired(q => q.SoaDataBatch).WithMany().HasForeignKey(q => q.SoaDataBatchId).WillCascadeOnDelete(false);
            directRetro.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            directRetro.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RetroSummary> retroSummaries = modelBuilder.Entity<RetroSummary>();
            retroSummaries.HasRequired(q => q.DirectRetro).WithMany().HasForeignKey(q => q.DirectRetroId).WillCascadeOnDelete(false);
            retroSummaries.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<DirectRetroStatusFile> directRetroStatusFiles = modelBuilder.Entity<DirectRetroStatusFile>();
            directRetroStatusFiles.HasRequired(q => q.DirectRetro).WithMany().HasForeignKey(q => q.DirectRetroId).WillCascadeOnDelete(false);
            directRetroStatusFiles.HasRequired(q => q.StatusHistory).WithMany().HasForeignKey(q => q.StatusHistoryId).WillCascadeOnDelete(false);
            directRetroStatusFiles.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            directRetroStatusFiles.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RetroStatement> retroStatements = modelBuilder.Entity<RetroStatement>();
            retroStatements.HasRequired(q => q.DirectRetro).WithMany().HasForeignKey(q => q.DirectRetroId).WillCascadeOnDelete(false);
            retroStatements.HasRequired(q => q.RetroParty).WithMany().HasForeignKey(q => q.RetroPartyId).WillCascadeOnDelete(false);
            retroStatements.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            retroStatements.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ReferralClaim> referralClaims = modelBuilder.Entity<ReferralClaim>();
            referralClaims.HasOptional(q => q.ClaimRegister).WithMany().HasForeignKey(q => q.ClaimRegisterId).WillCascadeOnDelete(false);
            referralClaims.HasOptional(q => q.RiDataWarehouse).WithMany().HasForeignKey(q => q.RiDataWarehouseId).WillCascadeOnDelete(false);
            referralClaims.HasOptional(q => q.ReferralRiData).WithMany().HasForeignKey(q => q.ReferralRiDataId).WillCascadeOnDelete(false);
            referralClaims.HasOptional(q => q.ReferralReason).WithMany().HasForeignKey(q => q.ReferralReasonId).WillCascadeOnDelete(false);
            referralClaims.HasOptional(q => q.ClaimCategory).WithMany().HasForeignKey(q => q.ClaimCategoryId).WillCascadeOnDelete(false);
            referralClaims.HasOptional(q => q.DelayReason).WithMany().HasForeignKey(q => q.DelayReasonId).WillCascadeOnDelete(false);
            referralClaims.HasOptional(q => q.RetroReferralReason).WithMany().HasForeignKey(q => q.RetroReferralReasonId).WillCascadeOnDelete(false);
            referralClaims.HasOptional(q => q.MlreReferralReason).WithMany().HasForeignKey(q => q.MlreReferralReasonId).WillCascadeOnDelete(false);
            referralClaims.HasOptional(q => q.AssessedBy).WithMany().HasForeignKey(q => q.AssessedById).WillCascadeOnDelete(false);
            referralClaims.HasOptional(q => q.ReviewedBy).WithMany().HasForeignKey(q => q.ReviewedById).WillCascadeOnDelete(false);
            referralClaims.HasOptional(q => q.AssignedBy).WithMany().HasForeignKey(q => q.AssignedById).WillCascadeOnDelete(false);
            referralClaims.HasOptional(q => q.PersonInCharge).WithMany().HasForeignKey(q => q.PersonInChargeId).WillCascadeOnDelete(false);
            referralClaims.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            referralClaims.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RetroRegisterBatch> retroRegisterBatches = modelBuilder.Entity<RetroRegisterBatch>();
            retroRegisterBatches.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            retroRegisterBatches.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RetroRegisterBatchDirectRetro> retroRegisterBatchDirectRetro = modelBuilder.Entity<RetroRegisterBatchDirectRetro>();
            retroRegisterBatchDirectRetro.HasRequired(q => q.RetroRegisterBatch).WithMany().HasForeignKey(q => q.RetroRegisterBatchId).WillCascadeOnDelete(false);
            retroRegisterBatchDirectRetro.HasRequired(q => q.DirectRetro).WithMany().HasForeignKey(q => q.DirectRetroId).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RetroRegisterBatchFile> retroRegisterBatchFiles = modelBuilder.Entity<RetroRegisterBatchFile>();
            retroRegisterBatchFiles.HasRequired(q => q.RetroRegisterBatch).WithMany().HasForeignKey(q => q.RetroRegisterBatchId).WillCascadeOnDelete(false);
            retroRegisterBatchFiles.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            retroRegisterBatchFiles.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RetroRegister> retroRegisters = modelBuilder.Entity<RetroRegister>();
            retroRegisters.HasOptional(q => q.RetroRegisterBatch).WithMany().HasForeignKey(q => q.RetroRegisterBatchId).WillCascadeOnDelete(false);
            retroRegisters.HasOptional(q => q.RetroParty).WithMany().HasForeignKey(q => q.RetroPartyId).WillCascadeOnDelete(false);
            retroRegisters.HasOptional(q => q.Cedant).WithMany().HasForeignKey(q => q.CedantId).WillCascadeOnDelete(false);
            retroRegisters.HasOptional(q => q.TreatyCode).WithMany().HasForeignKey(q => q.TreatyCodeId).WillCascadeOnDelete(false);
            retroRegisters.HasOptional(q => q.DirectRetro).WithMany().HasForeignKey(q => q.DirectRetroId).WillCascadeOnDelete(false);
            retroRegisters.HasOptional(q => q.PreparedBy).WithMany().HasForeignKey(q => q.PreparedById).WillCascadeOnDelete(false);
            retroRegisters.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            retroRegisters.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RetroRegisterHistory> retroRegisterHistories = modelBuilder.Entity<RetroRegisterHistory>();
            retroRegisterHistories.HasRequired(q => q.CutOff).WithMany().HasForeignKey(q => q.CutOffId).WillCascadeOnDelete(false);
            retroRegisterHistories.HasOptional(q => q.RetroParty).WithMany().HasForeignKey(q => q.RetroPartyId).WillCascadeOnDelete(false);
            retroRegisterHistories.HasOptional(q => q.Cedant).WithMany().HasForeignKey(q => q.CedantId).WillCascadeOnDelete(false);
            retroRegisterHistories.HasOptional(q => q.TreatyCode).WithMany().HasForeignKey(q => q.TreatyCodeId).WillCascadeOnDelete(false);
            retroRegisterHistories.HasOptional(q => q.PreparedBy).WithMany().HasForeignKey(q => q.PreparedById).WillCascadeOnDelete(false);
            retroRegisterHistories.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            retroRegisterHistories.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<CutOff> cutOff = modelBuilder.Entity<CutOff>();
            cutOff.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            cutOff.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            //EntityTypeConfiguration<RiDataWarehouseHistory> riDataWarehouseHistories = modelBuilder.Entity<RiDataWarehouseHistory>();
            //riDataWarehouseHistories.HasRequired(q => q.CutOff).WithMany().HasForeignKey(q => q.CutOffId).WillCascadeOnDelete(false);
            //riDataWarehouseHistories.HasRequired(q => q.RiDataWarehouse).WithMany().HasForeignKey(q => q.RiDataWarehouseId).WillCascadeOnDelete(false);
            //riDataWarehouseHistories.HasOptional(q => q.EndingPolicyStatusPickListDetail).WithMany().HasForeignKey(q => q.EndingPolicyStatus).WillCascadeOnDelete(false);
            //riDataWarehouseHistories.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            //riDataWarehouseHistories.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<FinanceProvisioningTransaction> financeProvisioningTransactions = modelBuilder.Entity<FinanceProvisioningTransaction>();
            financeProvisioningTransactions.HasRequired(q => q.ClaimRegister).WithMany().HasForeignKey(q => q.ClaimRegisterId).WillCascadeOnDelete(false);
            financeProvisioningTransactions.HasOptional(q => q.FinanceProvisioning).WithMany().HasForeignKey(q => q.FinanceProvisioningId).WillCascadeOnDelete(false);
            financeProvisioningTransactions.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            financeProvisioningTransactions.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<DirectRetroProvisioningTransaction> directRetroProvisioningTransactions = modelBuilder.Entity<DirectRetroProvisioningTransaction>();
            directRetroProvisioningTransactions.HasRequired(q => q.ClaimRegister).WithMany().HasForeignKey(q => q.ClaimRegisterId).WillCascadeOnDelete(false);
            directRetroProvisioningTransactions.HasOptional(q => q.FinanceProvisioning).WithMany().HasForeignKey(q => q.FinanceProvisioningId).WillCascadeOnDelete(false);
            directRetroProvisioningTransactions.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            directRetroProvisioningTransactions.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<FinanceProvisioning> financeProvisionings = modelBuilder.Entity<FinanceProvisioning>();
            financeProvisionings.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            financeProvisionings.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ReferralRiDataFile> referralRiDataFiles = modelBuilder.Entity<ReferralRiDataFile>();
            referralRiDataFiles.HasRequired(q => q.RawFile).WithMany().HasForeignKey(q => q.RawFileId).WillCascadeOnDelete(false);
            referralRiDataFiles.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            referralRiDataFiles.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ReferralRiData> referralRiData = modelBuilder.Entity<ReferralRiData>();
            referralRiData.HasRequired(q => q.ReferralRiDataFile).WithMany().HasForeignKey(q => q.ReferralRiDataFileId).WillCascadeOnDelete(false);
            referralRiData.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            referralRiData.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<SanctionKeyword> sanctionKeywords = modelBuilder.Entity<SanctionKeyword>();
            sanctionKeywords.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            sanctionKeywords.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<SanctionKeywordDetail> sanctionKeywordDetails = modelBuilder.Entity<SanctionKeywordDetail>();
            sanctionKeywordDetails.HasRequired(q => q.SanctionKeyword).WithMany(q => q.SanctionKeywordDetails).HasForeignKey(q => q.SanctionKeywordId).WillCascadeOnDelete(false);
            sanctionKeywordDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            sanctionKeywordDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<SanctionExclusion> sanctionExclusions = modelBuilder.Entity<SanctionExclusion>();
            sanctionExclusions.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            sanctionExclusions.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<Source> sources = modelBuilder.Entity<Source>();
            sources.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            sources.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<SanctionBatch> sanctionBatches = modelBuilder.Entity<SanctionBatch>();
            sanctionBatches.HasRequired(q => q.Source).WithMany().HasForeignKey(q => q.SourceId).WillCascadeOnDelete(false);
            sanctionBatches.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            sanctionBatches.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<Sanction> sanctions = modelBuilder.Entity<Sanction>();
            sanctions.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            sanctions.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<SanctionName> sanctionNames = modelBuilder.Entity<SanctionName>();
            sanctionNames.HasRequired(q => q.Sanction).WithMany(q => q.SanctionNames).HasForeignKey(q => q.SanctionId).WillCascadeOnDelete(false);
            sanctionNames.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            sanctionNames.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<SanctionVerification> sanctionVerifications = modelBuilder.Entity<SanctionVerification>();
            sanctionVerifications.HasRequired(q => q.Source).WithMany().HasForeignKey(q => q.SourceId).WillCascadeOnDelete(false);
            sanctionVerifications.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            sanctionVerifications.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<SanctionAddress> sanctionAddresses = modelBuilder.Entity<SanctionAddress>();
            sanctionAddresses.HasRequired(q => q.Sanction).WithMany(q => q.SanctionAddresses).HasForeignKey(q => q.SanctionId).WillCascadeOnDelete(false);
            sanctionAddresses.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            sanctionAddresses.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<SanctionBirthDate> sanctionBirthDates = modelBuilder.Entity<SanctionBirthDate>();
            sanctionBirthDates.HasRequired(q => q.Sanction).WithMany(q => q.SanctionBirthDates).HasForeignKey(q => q.SanctionId).WillCascadeOnDelete(false);
            sanctionBirthDates.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            sanctionBirthDates.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<SanctionComment> sanctionComments = modelBuilder.Entity<SanctionComment>();
            sanctionComments.HasRequired(q => q.Sanction).WithMany(q => q.SanctionComments).HasForeignKey(q => q.SanctionId).WillCascadeOnDelete(false);
            sanctionComments.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            sanctionComments.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<SanctionCountry> sanctionCountries = modelBuilder.Entity<SanctionCountry>();
            sanctionCountries.HasRequired(q => q.Sanction).WithMany(q => q.SanctionCountries).HasForeignKey(q => q.SanctionId).WillCascadeOnDelete(false);
            sanctionCountries.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            sanctionCountries.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<SanctionIdentity> sanctionIdentities = modelBuilder.Entity<SanctionIdentity>();
            sanctionIdentities.HasRequired(q => q.Sanction).WithMany(q => q.SanctionIdentities).HasForeignKey(q => q.SanctionId).WillCascadeOnDelete(false);
            sanctionIdentities.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            sanctionIdentities.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<SanctionVerificationDetail> sanctionVerificationDetails = modelBuilder.Entity<SanctionVerificationDetail>();
            sanctionVerificationDetails.HasRequired(q => q.SanctionVerification).WithMany().HasForeignKey(q => q.SanctionVerificationId).WillCascadeOnDelete(false);
            sanctionVerificationDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            sanctionVerificationDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<SanctionFormatName> sanctionFormatNames = modelBuilder.Entity<SanctionFormatName>();
            sanctionFormatNames.HasRequired(q => q.Sanction).WithMany(q => q.SanctionFormatNames).HasForeignKey(q => q.SanctionId).WillCascadeOnDelete(false);
            sanctionFormatNames.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyMarketingAllowanceProvision> treatyMarketingAllowanceProvisions = modelBuilder.Entity<TreatyMarketingAllowanceProvision>();
            treatyMarketingAllowanceProvisions.HasRequired(q => q.TreatyCode).WithMany().HasForeignKey(q => q.TreatyCodeId).WillCascadeOnDelete(false);
            treatyMarketingAllowanceProvisions.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyMarketingAllowanceProvisions.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ObjectPermission> objectPermissions = modelBuilder.Entity<ObjectPermission>();
            objectPermissions.HasRequired(q => q.Department).WithMany().HasForeignKey(q => q.DepartmentId).WillCascadeOnDelete(false);
            objectPermissions.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RemarkFollowUp> remarkFollowUps = modelBuilder.Entity<RemarkFollowUp>();
            remarkFollowUps.HasRequired(q => q.Remark).WithMany().HasForeignKey(q => q.RemarkId).WillCascadeOnDelete(false);
            remarkFollowUps.HasRequired(q => q.FollowUpUser).WithMany().HasForeignKey(q => q.FollowUpUserId).WillCascadeOnDelete(false);
            remarkFollowUps.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            remarkFollowUps.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<Email> emails = modelBuilder.Entity<Email>();
            emails.HasOptional(q => q.RecipientUser).WithMany().HasForeignKey(q => q.RecipientUserId).WillCascadeOnDelete(false);
            emails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            emails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<SoaDataHistory> soaDataHistories = modelBuilder.Entity<SoaDataHistory>();
            soaDataHistories.HasRequired(q => q.CutOff).WithMany().HasForeignKey(q => q.CutOffId).WillCascadeOnDelete(false);

            EntityTypeConfiguration<SoaDataCompiledSummaryHistory> soaDataCompiledSummaryHistories = modelBuilder.Entity<SoaDataCompiledSummaryHistory>();
            soaDataCompiledSummaryHistories.HasRequired(q => q.CutOff).WithMany().HasForeignKey(q => q.CutOffId).WillCascadeOnDelete(false);

            EntityTypeConfiguration<SanctionWhitelist> sanctionWhitelists = modelBuilder.Entity<SanctionWhitelist>();
            sanctionWhitelists.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            sanctionWhitelists.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<SanctionBlacklist> sanctionBlacklists = modelBuilder.Entity<SanctionBlacklist>();
            sanctionBlacklists.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            sanctionBlacklists.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PickListDepartment> pickListDepartments = modelBuilder.Entity<PickListDepartment>();
            pickListDepartments.HasRequired(q => q.PickList).WithMany(q => q.PickListDepartments).HasForeignKey(q => q.PickListId).WillCascadeOnDelete(false);
            pickListDepartments.HasRequired(q => q.Department).WithMany().HasForeignKey(q => q.DepartmentId).WillCascadeOnDelete(false);
            pickListDepartments.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            pickListDepartments.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<InvoiceRegisterBatchStatusFile> invoiceRegisterBatchStatusFile = modelBuilder.Entity<InvoiceRegisterBatchStatusFile>();
            invoiceRegisterBatchStatusFile.HasRequired(q => q.InvoiceRegisterBatch).WithMany().HasForeignKey(q => q.InvoiceRegisterBatchId).WillCascadeOnDelete(false);
            invoiceRegisterBatchStatusFile.HasRequired(q => q.StatusHistory).WithMany().HasForeignKey(q => q.StatusHistoryId).WillCascadeOnDelete(false);
            invoiceRegisterBatchStatusFile.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            invoiceRegisterBatchStatusFile.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RetroRegisterBatchStatusFile> retroRegisterBatchStatusFile = modelBuilder.Entity<RetroRegisterBatchStatusFile>();
            retroRegisterBatchStatusFile.HasRequired(q => q.RetroRegisterBatch).WithMany().HasForeignKey(q => q.RetroRegisterBatchId).WillCascadeOnDelete(false);
            retroRegisterBatchStatusFile.HasRequired(q => q.StatusHistory).WithMany().HasForeignKey(q => q.StatusHistoryId).WillCascadeOnDelete(false);
            retroRegisterBatchStatusFile.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            retroRegisterBatchStatusFile.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<SoaDataBatchHistory> soaDataBatchHistories = modelBuilder.Entity<SoaDataBatchHistory>();
            soaDataBatchHistories.HasRequired(q => q.CutOff).WithMany().HasForeignKey(q => q.CutOffId).WillCascadeOnDelete(false);

            EntityTypeConfiguration<Mfrs17ContractCode> mfrs17ContractCodes = modelBuilder.Entity<Mfrs17ContractCode>();
            mfrs17ContractCodes.HasRequired(q => q.CedingCompany).WithMany().HasForeignKey(q => q.CedingCompanyId).WillCascadeOnDelete(false);
            mfrs17ContractCodes.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            mfrs17ContractCodes.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<UwQuestionnaireCategory> uwQuestionnaireCategories = modelBuilder.Entity<UwQuestionnaireCategory>();
            uwQuestionnaireCategories.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            uwQuestionnaireCategories.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<HipsCategory> hipsCategories = modelBuilder.Entity<HipsCategory>();
            hipsCategories.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            hipsCategories.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<HipsCategoryDetail> hipsCategoryDetails = modelBuilder.Entity<HipsCategoryDetail>();
            hipsCategoryDetails.HasRequired(q => q.HipsCategory).WithMany().HasForeignKey(q => q.HipsCategoryId).WillCascadeOnDelete(false);
            hipsCategoryDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            hipsCategoryDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<Designation> designations = modelBuilder.Entity<Designation>();
            designations.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            designations.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<InsuredGroupName> insuredGroupNames = modelBuilder.Entity<InsuredGroupName>();
            insuredGroupNames.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            insuredGroupNames.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<Template> templates = modelBuilder.Entity<Template>();
            templates.HasRequired(q => q.Cedant).WithMany().HasForeignKey(q => q.CedantId).WillCascadeOnDelete(false);
            templates.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            templates.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TemplateDetail> templateDetails = modelBuilder.Entity<TemplateDetail>();
            templateDetails.HasRequired(q => q.Template).WithMany().HasForeignKey(q => q.TemplateId).WillCascadeOnDelete(false);
            templateDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            templateDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingCedant> treatyPricingCedants = modelBuilder.Entity<TreatyPricingCedant>();
            treatyPricingCedants.HasRequired(q => q.Cedant).WithMany().HasForeignKey(q => q.CedantId).WillCascadeOnDelete(false);
            treatyPricingCedants.HasRequired(q => q.ReinsuranceTypePickListDetail).WithMany().HasForeignKey(q => q.ReinsuranceTypePickListDetailId).WillCascadeOnDelete(false);
            treatyPricingCedants.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingCedants.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingRateTableGroup> treatyPricingRateTableGroups = modelBuilder.Entity<TreatyPricingRateTableGroup>();
            treatyPricingRateTableGroups.HasRequired(q => q.TreatyPricingCedant).WithMany().HasForeignKey(q => q.TreatyPricingCedantId).WillCascadeOnDelete(false);
            treatyPricingRateTableGroups.HasRequired(q => q.UploadedBy).WithMany().HasForeignKey(q => q.UploadedById).WillCascadeOnDelete(false);
            treatyPricingRateTableGroups.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingRateTableGroups.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingRateTable> treatyPricingRateTables = modelBuilder.Entity<TreatyPricingRateTable>();
            treatyPricingRateTables.HasRequired(q => q.TreatyPricingRateTableGroup).WithMany().HasForeignKey(q => q.TreatyPricingRateTableGroupId).WillCascadeOnDelete(false);
            treatyPricingRateTables.HasRequired(q => q.Benefit).WithMany().HasForeignKey(q => q.BenefitId).WillCascadeOnDelete(false);
            treatyPricingRateTables.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingRateTables.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingUwQuestionnaire> treatyPricingUwQuestionnaires = modelBuilder.Entity<TreatyPricingUwQuestionnaire>();
            treatyPricingUwQuestionnaires.HasRequired(q => q.TreatyPricingCedant).WithMany().HasForeignKey(q => q.TreatyPricingCedantId).WillCascadeOnDelete(false);
            treatyPricingUwQuestionnaires.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingUwQuestionnaires.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingUwQuestionnaireVersion> treatyPricingUwQuestionnaireVersions = modelBuilder.Entity<TreatyPricingUwQuestionnaireVersion>();
            treatyPricingUwQuestionnaireVersions.HasRequired(q => q.TreatyPricingUwQuestionnaire).WithMany().HasForeignKey(q => q.TreatyPricingUwQuestionnaireId).WillCascadeOnDelete(false);
            treatyPricingUwQuestionnaireVersions.HasRequired(q => q.PersonInCharge).WithMany().HasForeignKey(q => q.PersonInChargeId).WillCascadeOnDelete(false);
            treatyPricingUwQuestionnaireVersions.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingUwQuestionnaireVersions.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingUwQuestionnaireVersionDetail> treatyPricingUwQuestionnaireVersionDetails = modelBuilder.Entity<TreatyPricingUwQuestionnaireVersionDetail>();
            treatyPricingUwQuestionnaireVersionDetails.HasRequired(q => q.TreatyPricingUwQuestionnaireVersion).WithMany().HasForeignKey(q => q.TreatyPricingUwQuestionnaireVersionId).WillCascadeOnDelete(false);
            treatyPricingUwQuestionnaireVersionDetails.HasRequired(q => q.UwQuestionnaireCategory).WithMany().HasForeignKey(q => q.UwQuestionnaireCategoryId).WillCascadeOnDelete(false);
            treatyPricingUwQuestionnaireVersionDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingUwQuestionnaireVersionDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingUwQuestionnaireVersionFile> treatyPricingUwQuestionnaireVersionFiles = modelBuilder.Entity<TreatyPricingUwQuestionnaireVersionFile>();
            treatyPricingUwQuestionnaireVersionFiles.HasRequired(q => q.TreatyPricingUwQuestionnaireVersion).WithMany().HasForeignKey(q => q.TreatyPricingUwQuestionnaireVersionId).WillCascadeOnDelete(false);
            treatyPricingUwQuestionnaireVersionFiles.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingUwQuestionnaireVersionFiles.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingProduct> treatyPricingProducts = modelBuilder.Entity<TreatyPricingProduct>();
            treatyPricingProducts.HasRequired(q => q.TreatyPricingCedant).WithMany().HasForeignKey(q => q.TreatyPricingCedantId).WillCascadeOnDelete(false);
            treatyPricingProducts.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingProducts.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingRateTableVersion> treatyPricingRateTableVersions = modelBuilder.Entity<TreatyPricingRateTableVersion>();
            treatyPricingRateTableVersions.HasRequired(q => q.TreatyPricingRateTable).WithMany().HasForeignKey(q => q.TreatyPricingRateTableId).WillCascadeOnDelete(false);
            treatyPricingRateTableVersions.HasRequired(q => q.PersonInCharge).WithMany().HasForeignKey(q => q.PersonInChargeId).WillCascadeOnDelete(false);
            treatyPricingRateTableVersions.HasOptional(q => q.AgeBasisPickListDetail).WithMany().HasForeignKey(q => q.AgeBasisPickListDetailId).WillCascadeOnDelete(false);
            treatyPricingRateTableVersions.HasOptional(q => q.RateGuaranteePickListDetail).WithMany().HasForeignKey(q => q.RateGuaranteePickListDetailId).WillCascadeOnDelete(false);
            treatyPricingRateTableVersions.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingRateTableVersions.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingRateTableDetail> treatyPricingRateTableDetails = modelBuilder.Entity<TreatyPricingRateTableDetail>();
            treatyPricingRateTableDetails.HasRequired(q => q.TreatyPricingRateTableVersion).WithMany().HasForeignKey(q => q.TreatyPricingRateTableVersionId).WillCascadeOnDelete(false);
            treatyPricingRateTableDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingRateTableDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingRateTableRate> treatyPricingRateTableRates = modelBuilder.Entity<TreatyPricingRateTableRate>();
            treatyPricingRateTableRates.HasRequired(q => q.TreatyPricingRateTableVersion).WithMany().HasForeignKey(q => q.TreatyPricingRateTableVersionId).WillCascadeOnDelete(false);
            treatyPricingRateTableRates.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingRateTableRates.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingRateTableOriginalRate> treatyPricingRateTableOriginalRates = modelBuilder.Entity<TreatyPricingRateTableOriginalRate>();
            treatyPricingRateTableOriginalRates.HasRequired(q => q.TreatyPricingRateTableVersion).WithMany().HasForeignKey(q => q.TreatyPricingRateTableVersionId).WillCascadeOnDelete(false);
            treatyPricingRateTableOriginalRates.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingRateTableOriginalRates.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingUwLimit> treatyPricingUwLimits = modelBuilder.Entity<TreatyPricingUwLimit>();
            treatyPricingUwLimits.HasRequired(q => q.TreatyPricingCedant).WithMany().HasForeignKey(q => q.TreatyPricingCedantId).WillCascadeOnDelete(false);
            treatyPricingUwLimits.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingUwLimits.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingUwLimitVersion> treatyPricingUwLimitVersions = modelBuilder.Entity<TreatyPricingUwLimitVersion>();
            treatyPricingUwLimitVersions.HasRequired(q => q.TreatyPricingUwLimit).WithMany().HasForeignKey(q => q.TreatyPricingUwLimitId).WillCascadeOnDelete(false);
            treatyPricingUwLimitVersions.HasRequired(q => q.PersonInCharge).WithMany().HasForeignKey(q => q.PersonInChargeId).WillCascadeOnDelete(false);
            treatyPricingUwLimitVersions.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingUwLimitVersions.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingClaimApprovalLimit> treatyPricingClaimApprovalLimits = modelBuilder.Entity<TreatyPricingClaimApprovalLimit>();
            treatyPricingClaimApprovalLimits.HasRequired(q => q.TreatyPricingCedant).WithMany().HasForeignKey(q => q.TreatyPricingCedantId).WillCascadeOnDelete(false);
            treatyPricingClaimApprovalLimits.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingClaimApprovalLimits.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingClaimApprovalLimitVersion> treatyPricingClaimApprovalLimitVersions = modelBuilder.Entity<TreatyPricingClaimApprovalLimitVersion>();
            treatyPricingClaimApprovalLimitVersions.HasRequired(q => q.TreatyPricingClaimApprovalLimit).WithMany().HasForeignKey(q => q.TreatyPricingClaimApprovalLimitId).WillCascadeOnDelete(false);
            treatyPricingClaimApprovalLimitVersions.HasRequired(q => q.PersonInCharge).WithMany().HasForeignKey(q => q.PersonInChargeId).WillCascadeOnDelete(false);
            treatyPricingClaimApprovalLimitVersions.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingClaimApprovalLimitVersions.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingDefinitionAndExclusion> treatyPricingDefinitionAndExclusions = modelBuilder.Entity<TreatyPricingDefinitionAndExclusion>();
            treatyPricingDefinitionAndExclusions.HasRequired(q => q.TreatyPricingCedant).WithMany().HasForeignKey(q => q.TreatyPricingCedantId).WillCascadeOnDelete(false);
            treatyPricingDefinitionAndExclusions.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingDefinitionAndExclusions.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingDefinitionAndExclusionVersion> treatyPricingDefinitionAndExclusionVersions = modelBuilder.Entity<TreatyPricingDefinitionAndExclusionVersion>();
            treatyPricingDefinitionAndExclusionVersions.HasRequired(q => q.TreatyPricingDefinitionAndExclusion).WithMany().HasForeignKey(q => q.TreatyPricingDefinitionAndExclusionId).WillCascadeOnDelete(false);
            treatyPricingDefinitionAndExclusionVersions.HasRequired(q => q.PersonInCharge).WithMany().HasForeignKey(q => q.PersonInChargeId).WillCascadeOnDelete(false);
            treatyPricingDefinitionAndExclusionVersions.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingDefinitionAndExclusionVersions.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingCustomOther> treatyPricingCustomOthers = modelBuilder.Entity<TreatyPricingCustomOther>();
            treatyPricingCustomOthers.HasRequired(q => q.TreatyPricingCedant).WithMany().HasForeignKey(q => q.TreatyPricingCedantId).WillCascadeOnDelete(false);
            treatyPricingCustomOthers.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingCustomOthers.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingCustomOtherVersion> treatyPricingCustomOtherVersions = modelBuilder.Entity<TreatyPricingCustomOtherVersion>();
            treatyPricingCustomOtherVersions.HasRequired(q => q.TreatyPricingCustomOther).WithMany().HasForeignKey(q => q.TreatyPricingCustomOtherId).WillCascadeOnDelete(false);
            treatyPricingCustomOtherVersions.HasRequired(q => q.PersonInCharge).WithMany().HasForeignKey(q => q.PersonInChargeId).WillCascadeOnDelete(false);
            treatyPricingCustomOtherVersions.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingCustomOtherVersions.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingCustomOtherProduct> treatyPricingCustomOtherProducts = modelBuilder.Entity<TreatyPricingCustomOtherProduct>();
            treatyPricingCustomOtherProducts.HasRequired(q => q.TreatyPricingCustomOther).WithMany().HasForeignKey(q => q.TreatyPricingCustomOtherId).WillCascadeOnDelete(false);
            treatyPricingCustomOtherProducts.HasRequired(q => q.TreatyPricingProduct).WithMany().HasForeignKey(q => q.TreatyPricingProductId).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingProductVersion> treatyPricingProductVersions = modelBuilder.Entity<TreatyPricingProductVersion>();
            treatyPricingProductVersions.HasRequired(q => q.TreatyPricingProduct).WithMany().HasForeignKey(q => q.TreatyPricingProductId).WillCascadeOnDelete(false);
            treatyPricingProductVersions.HasRequired(q => q.PersonInCharge).WithMany().HasForeignKey(q => q.PersonInChargeId).WillCascadeOnDelete(false);
            treatyPricingProductVersions.HasOptional(q => q.ProductTypePickListDetail).WithMany().HasForeignKey(q => q.ProductTypePickListDetailId).WillCascadeOnDelete(false);
            treatyPricingProductVersions.HasOptional(q => q.BusinessOriginPickListDetail).WithMany().HasForeignKey(q => q.BusinessOriginPickListDetailId).WillCascadeOnDelete(false);
            treatyPricingProductVersions.HasOptional(q => q.BusinessTypePickListDetail).WithMany().HasForeignKey(q => q.BusinessTypePickListDetailId).WillCascadeOnDelete(false);
            treatyPricingProductVersions.HasOptional(q => q.ReinsuranceArrangementPickListDetail).WithMany().HasForeignKey(q => q.ReinsuranceArrangementPickListDetailId).WillCascadeOnDelete(false);
            treatyPricingProductVersions.HasOptional(q => q.TreatyPricingMedicalTable).WithMany().HasForeignKey(q => q.TreatyPricingMedicalTableId).WillCascadeOnDelete(false);
            treatyPricingProductVersions.HasOptional(q => q.TreatyPricingMedicalTableVersion).WithMany().HasForeignKey(q => q.TreatyPricingMedicalTableVersionId).WillCascadeOnDelete(false);
            treatyPricingProductVersions.HasOptional(q => q.TreatyPricingFinancialTable).WithMany().HasForeignKey(q => q.TreatyPricingFinancialTableId).WillCascadeOnDelete(false);
            treatyPricingProductVersions.HasOptional(q => q.TreatyPricingFinancialTableVersion).WithMany().HasForeignKey(q => q.TreatyPricingFinancialTableVersionId).WillCascadeOnDelete(false);
            treatyPricingProductVersions.HasOptional(q => q.TreatyPricingUwQuestionnaire).WithMany().HasForeignKey(q => q.TreatyPricingUwQuestionnaireId).WillCascadeOnDelete(false);
            treatyPricingProductVersions.HasOptional(q => q.TreatyPricingUwQuestionnaireVersion).WithMany().HasForeignKey(q => q.TreatyPricingUwQuestionnaireVersionId).WillCascadeOnDelete(false);
            treatyPricingProductVersions.HasOptional(q => q.TreatyPricingAdvantageProgram).WithMany().HasForeignKey(q => q.TreatyPricingAdvantageProgramId).WillCascadeOnDelete(false);
            treatyPricingProductVersions.HasOptional(q => q.TreatyPricingAdvantageProgramVersion).WithMany().HasForeignKey(q => q.TreatyPricingAdvantageProgramVersionId).WillCascadeOnDelete(false);
            treatyPricingProductVersions.HasOptional(q => q.TreatyPricingProfitCommission).WithMany().HasForeignKey(q => q.TreatyPricingProfitCommissionId).WillCascadeOnDelete(false);
            treatyPricingProductVersions.HasOptional(q => q.TreatyPricingProfitCommissionVersion).WithMany().HasForeignKey(q => q.TreatyPricingProfitCommissionVersionId).WillCascadeOnDelete(false);
            treatyPricingProductVersions.HasOptional(q => q.ReinsurancePremiumPaymentPickListDetail).WithMany().HasForeignKey(q => q.ReinsurancePremiumPaymentPickListDetailId).WillCascadeOnDelete(false);
            treatyPricingProductVersions.HasOptional(q => q.UnearnedPremiumRefundPickListDetail).WithMany().HasForeignKey(q => q.UnearnedPremiumRefundPickListDetailId).WillCascadeOnDelete(false);
            treatyPricingProductVersions.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingProductVersions.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingMedicalTable> treatyPricingMedicalTables = modelBuilder.Entity<TreatyPricingMedicalTable>();
            treatyPricingMedicalTables.HasRequired(q => q.TreatyPricingCedant).WithMany().HasForeignKey(q => q.TreatyPricingCedantId).WillCascadeOnDelete(false);
            treatyPricingMedicalTables.HasOptional(q => q.AgeDefinitionPickListDetail).WithMany().HasForeignKey(q => q.AgeDefinitionPickListDetailId).WillCascadeOnDelete(false);
            treatyPricingMedicalTables.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingMedicalTables.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingMedicalTableVersion> treatyPricingMedicalTableVersions = modelBuilder.Entity<TreatyPricingMedicalTableVersion>();
            treatyPricingMedicalTableVersions.HasRequired(q => q.TreatyPricingMedicalTable).WithMany().HasForeignKey(q => q.TreatyPricingMedicalTableId).WillCascadeOnDelete(false);
            treatyPricingMedicalTableVersions.HasRequired(q => q.PersonInCharge).WithMany().HasForeignKey(q => q.PersonInChargeId).WillCascadeOnDelete(false);
            treatyPricingMedicalTableVersions.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingMedicalTableVersions.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingMedicalTableVersionFile> treatyPricingMedicalTableVersionFiles = modelBuilder.Entity<TreatyPricingMedicalTableVersionFile>();
            treatyPricingMedicalTableVersionFiles.HasRequired(q => q.TreatyPricingMedicalTableVersion).WithMany().HasForeignKey(q => q.TreatyPricingMedicalTableVersionId).WillCascadeOnDelete(false);
            treatyPricingMedicalTableVersionFiles.HasRequired(q => q.DistributionTierPickListDetail).WithMany().HasForeignKey(q => q.DistributionTierPickListDetailId).WillCascadeOnDelete(false);
            treatyPricingMedicalTableVersionFiles.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingMedicalTableVersionFiles.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingMedicalTableVersionDetail> treatyPricingMedicalTableVersionDetails = modelBuilder.Entity<TreatyPricingMedicalTableVersionDetail>();
            treatyPricingMedicalTableVersionDetails.HasRequired(q => q.TreatyPricingMedicalTableVersion).WithMany().HasForeignKey(q => q.TreatyPricingMedicalTableVersionId).WillCascadeOnDelete(false);
            treatyPricingMedicalTableVersionDetails.HasRequired(q => q.DistributionTierPickListDetail).WithMany().HasForeignKey(q => q.DistributionTierPickListDetailId).WillCascadeOnDelete(false);
            treatyPricingMedicalTableVersionDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingMedicalTableVersionDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingMedicalTableUploadCell> treatyPricingMedicalTableUploadCells = modelBuilder.Entity<TreatyPricingMedicalTableUploadCell>();
            treatyPricingMedicalTableUploadCells.HasRequired(q => q.TreatyPricingMedicalTableUploadColumn).WithMany().HasForeignKey(q => q.TreatyPricingMedicalTableUploadColumnId).WillCascadeOnDelete(false);
            treatyPricingMedicalTableUploadCells.HasRequired(q => q.TreatyPricingMedicalTableUploadRow).WithMany().HasForeignKey(q => q.TreatyPricingMedicalTableUploadRowId).WillCascadeOnDelete(false);
            treatyPricingMedicalTableUploadCells.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingMedicalTableUploadCells.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingMedicalTableUploadColumn> treatyPricingMedicalTableUploadColumns = modelBuilder.Entity<TreatyPricingMedicalTableUploadColumn>();
            treatyPricingMedicalTableUploadColumns.HasRequired(q => q.TreatyPricingMedicalTableVersionDetail).WithMany().HasForeignKey(q => q.TreatyPricingMedicalTableVersionDetailId).WillCascadeOnDelete(false);
            treatyPricingMedicalTableUploadColumns.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingMedicalTableUploadColumns.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingMedicalTableUploadLegend> treatyPricingMedicalTableUploadLegends = modelBuilder.Entity<TreatyPricingMedicalTableUploadLegend>();
            treatyPricingMedicalTableUploadLegends.HasRequired(q => q.TreatyPricingMedicalTableVersionDetail).WithMany().HasForeignKey(q => q.TreatyPricingMedicalTableVersionDetailId).WillCascadeOnDelete(false);
            treatyPricingMedicalTableUploadLegends.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingMedicalTableUploadLegends.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingMedicalTableUploadRow> treatyPricingMedicalTableUploadRows = modelBuilder.Entity<TreatyPricingMedicalTableUploadRow>();
            treatyPricingMedicalTableUploadRows.HasRequired(q => q.TreatyPricingMedicalTableVersionDetail).WithMany().HasForeignKey(q => q.TreatyPricingMedicalTableVersionDetailId).WillCascadeOnDelete(false);
            treatyPricingMedicalTableUploadRows.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingMedicalTableUploadRows.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingAdvantageProgram> treatyPricingAdvantagePrograms = modelBuilder.Entity<TreatyPricingAdvantageProgram>();
            treatyPricingAdvantagePrograms.HasRequired(q => q.TreatyPricingCedant).WithMany().HasForeignKey(q => q.TreatyPricingCedantId).WillCascadeOnDelete(false);
            treatyPricingAdvantagePrograms.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingAdvantagePrograms.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingAdvantageProgramVersion> treatyPricingAdvantageProgramVersions = modelBuilder.Entity<TreatyPricingAdvantageProgramVersion>();
            treatyPricingAdvantageProgramVersions.HasRequired(q => q.TreatyPricingAdvantageProgram).WithMany().HasForeignKey(q => q.TreatyPricingAdvantageProgramId).WillCascadeOnDelete(false);
            treatyPricingAdvantageProgramVersions.HasRequired(q => q.PersonInCharge).WithMany().HasForeignKey(q => q.PersonInChargeId).WillCascadeOnDelete(false);
            treatyPricingAdvantageProgramVersions.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingAdvantageProgramVersions.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingAdvantageProgramVersionBenefit> treatyPricingAdvantageProgramVersionBenefits = modelBuilder.Entity<TreatyPricingAdvantageProgramVersionBenefit>();
            treatyPricingAdvantageProgramVersionBenefits.HasRequired(q => q.TreatyPricingAdvantageProgramVersion).WithMany().HasForeignKey(q => q.TreatyPricingAdvantageProgramVersionId).WillCascadeOnDelete(false);
            treatyPricingAdvantageProgramVersionBenefits.HasRequired(q => q.Benefit).WithMany().HasForeignKey(q => q.BenefitId).WillCascadeOnDelete(false);
            treatyPricingAdvantageProgramVersionBenefits.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingAdvantageProgramVersionBenefits.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingFinancialTable> treatyPricingFinancialTables = modelBuilder.Entity<TreatyPricingFinancialTable>();
            treatyPricingFinancialTables.HasRequired(q => q.TreatyPricingCedant).WithMany().HasForeignKey(q => q.TreatyPricingCedantId).WillCascadeOnDelete(false);
            treatyPricingFinancialTables.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingFinancialTables.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingFinancialTableVersion> treatyPricingFinancialTableVersions = modelBuilder.Entity<TreatyPricingFinancialTableVersion>();
            treatyPricingFinancialTableVersions.HasRequired(q => q.TreatyPricingFinancialTable).WithMany().HasForeignKey(q => q.TreatyPricingFinancialTableId).WillCascadeOnDelete(false);
            treatyPricingFinancialTableVersions.HasRequired(q => q.PersonInCharge).WithMany().HasForeignKey(q => q.PersonInChargeId).WillCascadeOnDelete(false);
            treatyPricingFinancialTableVersions.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingFinancialTableVersions.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingFinancialTableVersionFile> treatyPricingFinancialTableVersionFiles = modelBuilder.Entity<TreatyPricingFinancialTableVersionFile>();
            treatyPricingFinancialTableVersionFiles.HasRequired(q => q.TreatyPricingFinancialTableVersion).WithMany().HasForeignKey(q => q.TreatyPricingFinancialTableVersionId).WillCascadeOnDelete(false);
            treatyPricingFinancialTableVersionFiles.HasRequired(q => q.DistributionTierPickListDetail).WithMany().HasForeignKey(q => q.DistributionTierPickListDetailId).WillCascadeOnDelete(false);
            treatyPricingFinancialTableVersionFiles.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingFinancialTableVersionFiles.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingFinancialTableVersionDetail> treatyPricingFinancialTableVersionDetails = modelBuilder.Entity<TreatyPricingFinancialTableVersionDetail>();
            treatyPricingFinancialTableVersionDetails.HasRequired(q => q.TreatyPricingFinancialTableVersion).WithMany().HasForeignKey(q => q.TreatyPricingFinancialTableVersionId).WillCascadeOnDelete(false);
            treatyPricingFinancialTableVersionDetails.HasRequired(q => q.DistributionTierPickListDetail).WithMany().HasForeignKey(q => q.DistributionTierPickListDetailId).WillCascadeOnDelete(false);
            treatyPricingFinancialTableVersionDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingFinancialTableVersionDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingFinancialTableUpload> treatyPricingFinancialTableUploads = modelBuilder.Entity<TreatyPricingFinancialTableUpload>();
            treatyPricingFinancialTableUploads.HasRequired(q => q.TreatyPricingFinancialTableVersionDetail).WithMany().HasForeignKey(q => q.TreatyPricingFinancialTableVersionDetailId).WillCascadeOnDelete(false);
            treatyPricingFinancialTableUploads.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingFinancialTableUploads.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingFinancialTableUploadLegend> treatyPricingFinancialTableUploadLegends = modelBuilder.Entity<TreatyPricingFinancialTableUploadLegend>();
            treatyPricingFinancialTableUploadLegends.HasRequired(q => q.TreatyPricingFinancialTableVersionDetail).WithMany().HasForeignKey(q => q.TreatyPricingFinancialTableVersionDetailId).WillCascadeOnDelete(false);
            treatyPricingFinancialTableUploadLegends.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingFinancialTableUploadLegends.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingProductDetail> treatyPricingProductDetails = modelBuilder.Entity<TreatyPricingProductDetail>();
            treatyPricingProductDetails.HasRequired(q => q.TreatyPricingProductVersion).WithMany().HasForeignKey(q => q.TreatyPricingProductVersionId).WillCascadeOnDelete(false);
            treatyPricingProductDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingProductDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingPickListDetail> treatyPricingPickListDetails = modelBuilder.Entity<TreatyPricingPickListDetail>();
            treatyPricingPickListDetails.HasRequired(q => q.PickList).WithMany().HasForeignKey(q => q.PickListId).WillCascadeOnDelete(false);
            treatyPricingPickListDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingProfitCommission> treatyPricingProfitCommissions = modelBuilder.Entity<TreatyPricingProfitCommission>();
            treatyPricingProfitCommissions.HasRequired(q => q.TreatyPricingCedant).WithMany().HasForeignKey(q => q.TreatyPricingCedantId).WillCascadeOnDelete(false);
            treatyPricingProfitCommissions.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingProfitCommissions.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingProfitCommissionVersion> treatyPricingProfitCommissionVersions = modelBuilder.Entity<TreatyPricingProfitCommissionVersion>();
            treatyPricingProfitCommissionVersions.HasRequired(q => q.TreatyPricingProfitCommission).WithMany().HasForeignKey(q => q.TreatyPricingProfitCommissionId).WillCascadeOnDelete(false);
            treatyPricingProfitCommissionVersions.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingProfitCommissionVersions.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingTierProfitCommission> treatyPricingTierProfitCommissions = modelBuilder.Entity<TreatyPricingTierProfitCommission>();
            treatyPricingTierProfitCommissions.HasRequired(q => q.TreatyPricingProfitCommissionVersion).WithMany().HasForeignKey(q => q.TreatyPricingProfitCommissionVersionId).WillCascadeOnDelete(false);
            treatyPricingTierProfitCommissions.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingTierProfitCommissions.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingProfitCommissionDetail> treatyPricingProfitCommissionDetails = modelBuilder.Entity<TreatyPricingProfitCommissionDetail>();
            treatyPricingProfitCommissionDetails.HasRequired(q => q.TreatyPricingProfitCommissionVersion).WithMany().HasForeignKey(q => q.TreatyPricingProfitCommissionVersionId).WillCascadeOnDelete(false);
            treatyPricingProfitCommissionDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingProfitCommissionDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingProductBenefit> treatyPricingProductBenefits = modelBuilder.Entity<TreatyPricingProductBenefit>();
            treatyPricingProductBenefits.HasRequired(q => q.TreatyPricingProductVersion).WithMany().HasForeignKey(q => q.TreatyPricingProductVersionId).WillCascadeOnDelete(false);
            treatyPricingProductBenefits.HasRequired(q => q.Benefit).WithMany().HasForeignKey(q => q.BenefitId).WillCascadeOnDelete(false);
            treatyPricingProductBenefits.HasOptional(q => q.BasicRiderPickListDetail).WithMany().HasForeignKey(q => q.BasicRiderPickListDetailId).WillCascadeOnDelete(false);
            treatyPricingProductBenefits.HasOptional(q => q.PayoutTypePickListDetail).WithMany().HasForeignKey(q => q.PayoutTypePickListDetailId).WillCascadeOnDelete(false);
            treatyPricingProductBenefits.HasOptional(q => q.AgeBasisPickListDetail).WithMany().HasForeignKey(q => q.AgeBasisPickListDetailId).WillCascadeOnDelete(false);
            treatyPricingProductBenefits.HasOptional(q => q.TreatyPricingUwLimit).WithMany().HasForeignKey(q => q.TreatyPricingUwLimitId).WillCascadeOnDelete(false);
            treatyPricingProductBenefits.HasOptional(q => q.TreatyPricingUwLimitVersion).WithMany().HasForeignKey(q => q.TreatyPricingUwLimitVersionId).WillCascadeOnDelete(false);
            treatyPricingProductBenefits.HasOptional(q => q.TreatyPricingClaimApprovalLimit).WithMany().HasForeignKey(q => q.TreatyPricingClaimApprovalLimitId).WillCascadeOnDelete(false);
            treatyPricingProductBenefits.HasOptional(q => q.TreatyPricingClaimApprovalLimitVersion).WithMany().HasForeignKey(q => q.TreatyPricingClaimApprovalLimitVersionId).WillCascadeOnDelete(false);
            treatyPricingProductBenefits.HasOptional(q => q.TreatyPricingDefinitionAndExclusion).WithMany().HasForeignKey(q => q.TreatyPricingDefinitionAndExclusionId).WillCascadeOnDelete(false);
            treatyPricingProductBenefits.HasOptional(q => q.TreatyPricingDefinitionAndExclusionVersion).WithMany().HasForeignKey(q => q.TreatyPricingDefinitionAndExclusionVersionId).WillCascadeOnDelete(false);
            treatyPricingProductBenefits.HasOptional(q => q.PricingArrangementReinsuranceTypePickListDetail).WithMany().HasForeignKey(q => q.PricingArrangementReinsuranceTypePickListDetailId).WillCascadeOnDelete(false);
            treatyPricingProductBenefits.HasOptional(q => q.RiskPatternSumPickListDetail).WithMany().HasForeignKey(q => q.RiskPatternSumPickListDetailId).WillCascadeOnDelete(false);
            treatyPricingProductBenefits.HasOptional(q => q.TreatyPricingRateTable).WithMany().HasForeignKey(q => q.TreatyPricingRateTableId).WillCascadeOnDelete(false);
            treatyPricingProductBenefits.HasOptional(q => q.TreatyPricingRateTableVersion).WithMany().HasForeignKey(q => q.TreatyPricingRateTableVersionId).WillCascadeOnDelete(false);
            treatyPricingProductBenefits.HasOptional(q => q.InwardRetroArrangementReinsuranceTypePickListDetail).WithMany().HasForeignKey(q => q.InwardRetroArrangementReinsuranceTypePickListDetailId).WillCascadeOnDelete(false);
            treatyPricingProductBenefits.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingProductBenefits.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingCampaign> treatyPricingCampaigns = modelBuilder.Entity<TreatyPricingCampaign>();
            treatyPricingCampaigns.HasRequired(q => q.TreatyPricingCedant).WithMany().HasForeignKey(q => q.TreatyPricingCedantId).WillCascadeOnDelete(false);
            treatyPricingCampaigns.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingCampaigns.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingCampaignVersion> treatyPricingCampaignVersions = modelBuilder.Entity<TreatyPricingCampaignVersion>();
            treatyPricingCampaignVersions.HasRequired(q => q.TreatyPricingCampaign).WithMany().HasForeignKey(q => q.TreatyPricingCampaignId).WillCascadeOnDelete(false);
            treatyPricingCampaignVersions.HasRequired(q => q.PersonInCharge).WithMany().HasForeignKey(q => q.PersonInChargeId).WillCascadeOnDelete(false);
            treatyPricingCampaignVersions.HasOptional(q => q.AgeBasisPickListDetail).WithMany().HasForeignKey(q => q.AgeBasisPickListDetailId).WillCascadeOnDelete(false);
            treatyPricingCampaignVersions.HasOptional(q => q.ReinsRateTreatyPricingRateTable).WithMany().HasForeignKey(q => q.ReinsRateTreatyPricingRateTableId).WillCascadeOnDelete(false);
            treatyPricingCampaignVersions.HasOptional(q => q.ReinsRateTreatyPricingRateTableVersion).WithMany().HasForeignKey(q => q.ReinsRateTreatyPricingRateTableVersionId).WillCascadeOnDelete(false);
            treatyPricingCampaignVersions.HasOptional(q => q.ReinsDiscountTreatyPricingRateTable).WithMany().HasForeignKey(q => q.ReinsDiscountTreatyPricingRateTableId).WillCascadeOnDelete(false);
            treatyPricingCampaignVersions.HasOptional(q => q.ReinsDiscountTreatyPricingRateTableVersion).WithMany().HasForeignKey(q => q.ReinsDiscountTreatyPricingRateTableVersionId).WillCascadeOnDelete(false);
            treatyPricingCampaignVersions.HasOptional(q => q.TreatyPricingProfitCommission).WithMany().HasForeignKey(q => q.TreatyPricingProfitCommissionId).WillCascadeOnDelete(false);
            treatyPricingCampaignVersions.HasOptional(q => q.TreatyPricingProfitCommissionVersion).WithMany().HasForeignKey(q => q.TreatyPricingProfitCommissionVersionId).WillCascadeOnDelete(false);
            treatyPricingCampaignVersions.HasOptional(q => q.TreatyPricingUwQuestionnaire).WithMany().HasForeignKey(q => q.TreatyPricingUwQuestionnaireId).WillCascadeOnDelete(false);
            treatyPricingCampaignVersions.HasOptional(q => q.TreatyPricingUwQuestionnaireVersion).WithMany().HasForeignKey(q => q.TreatyPricingUwQuestionnaireVersionId).WillCascadeOnDelete(false);
            treatyPricingCampaignVersions.HasOptional(q => q.TreatyPricingMedicalTable).WithMany().HasForeignKey(q => q.TreatyPricingMedicalTableId).WillCascadeOnDelete(false);
            treatyPricingCampaignVersions.HasOptional(q => q.TreatyPricingMedicalTableVersion).WithMany().HasForeignKey(q => q.TreatyPricingMedicalTableVersionId).WillCascadeOnDelete(false);
            treatyPricingCampaignVersions.HasOptional(q => q.TreatyPricingFinancialTable).WithMany().HasForeignKey(q => q.TreatyPricingFinancialTableId).WillCascadeOnDelete(false);
            treatyPricingCampaignVersions.HasOptional(q => q.TreatyPricingFinancialTableVersion).WithMany().HasForeignKey(q => q.TreatyPricingFinancialTableVersionId).WillCascadeOnDelete(false);
            treatyPricingCampaignVersions.HasOptional(q => q.TreatyPricingAdvantageProgram).WithMany().HasForeignKey(q => q.TreatyPricingAdvantageProgramId).WillCascadeOnDelete(false);
            treatyPricingCampaignVersions.HasOptional(q => q.TreatyPricingAdvantageProgramVersion).WithMany().HasForeignKey(q => q.TreatyPricingAdvantageProgramVersionId).WillCascadeOnDelete(false);
            treatyPricingCampaignVersions.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingCampaignVersions.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingCampaignProduct> treatyPricingCampaignProducts = modelBuilder.Entity<TreatyPricingCampaignProduct>();
            treatyPricingCampaignProducts.HasRequired(q => q.TreatyPricingCampaign).WithMany().HasForeignKey(q => q.TreatyPricingCampaignId).WillCascadeOnDelete(false);
            treatyPricingCampaignProducts.HasRequired(q => q.TreatyPricingProduct).WithMany().HasForeignKey(q => q.TreatyPricingProductId).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingQuotationWorkflow> treatyPricingQuotationWorkflows = modelBuilder.Entity<TreatyPricingQuotationWorkflow>();
            treatyPricingQuotationWorkflows.HasRequired(q => q.Cedant).WithMany().HasForeignKey(q => q.CedantId).WillCascadeOnDelete(false);
            treatyPricingQuotationWorkflows.HasOptional(q => q.ReinsuranceTypePickListDetail).WithMany().HasForeignKey(q => q.ReinsuranceTypePickListDetailId).WillCascadeOnDelete(false);
            treatyPricingQuotationWorkflows.HasOptional(q => q.PricingTeamPickListDetail).WithMany().HasForeignKey(q => q.PricingTeamPickListDetailId).WillCascadeOnDelete(false);
            treatyPricingQuotationWorkflows.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingQuotationWorkflows.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);
            treatyPricingQuotationWorkflows.HasOptional(q => q.BDPersonInCharge).WithMany().HasForeignKey(q => q.BDPersonInChargeId).WillCascadeOnDelete(false);
            treatyPricingQuotationWorkflows.HasOptional(q => q.PersonInCharge).WithMany().HasForeignKey(q => q.PersonInChargeId).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingQuotationWorkflowVersion> treatyPricingQuotationWorkflowVersions = modelBuilder.Entity<TreatyPricingQuotationWorkflowVersion>();
            treatyPricingQuotationWorkflowVersions.HasRequired(q => q.TreatyPricingQuotationWorkflow).WithMany().HasForeignKey(q => q.TreatyPricingQuotationWorkflowId).WillCascadeOnDelete(false);
            treatyPricingQuotationWorkflowVersions.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingQuotationWorkflowVersions.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);
            treatyPricingQuotationWorkflowVersions.HasOptional(q => q.PersonInCharge).WithMany().HasForeignKey(q => q.PersonInChargeId).WillCascadeOnDelete(false);
            treatyPricingQuotationWorkflowVersions.HasOptional(q => q.PersonInChargeTechReviewer).WithMany().HasForeignKey(q => q.PersonInChargeTechReviewerId).WillCascadeOnDelete(false);
            treatyPricingQuotationWorkflowVersions.HasOptional(q => q.PersonInChargePeerReviewer).WithMany().HasForeignKey(q => q.PersonInChargePeerReviewerId).WillCascadeOnDelete(false);
            treatyPricingQuotationWorkflowVersions.HasOptional(q => q.PersonInChargePricingAuthorityReviewer).WithMany().HasForeignKey(q => q.PersonInChargePricingAuthorityReviewerId).WillCascadeOnDelete(false);
            treatyPricingQuotationWorkflowVersions.HasOptional(q => q.BDPersonInCharge).WithMany().HasForeignKey(q => q.BDPersonInChargeId).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingWorkflowObject> treatyPricingWorkflowObjects = modelBuilder.Entity<TreatyPricingWorkflowObject>();
            treatyPricingWorkflowObjects.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingWorkflowObjects.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingQuotationWorkflowVersionQuotationChecklist> treatyPricingQuotationWorkflowVersionQuotationChecklists = modelBuilder.Entity<TreatyPricingQuotationWorkflowVersionQuotationChecklist>();
            treatyPricingQuotationWorkflowVersionQuotationChecklists.HasRequired(q => q.TreatyPricingQuotationWorkflowVersion).WithMany().HasForeignKey(q => q.TreatyPricingQuotationWorkflowVersionId).WillCascadeOnDelete(false);
            treatyPricingQuotationWorkflowVersionQuotationChecklists.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingQuotationWorkflowVersionQuotationChecklists.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingTreatyWorkflow> treatyPricingTreatyWorkflows = modelBuilder.Entity<TreatyPricingTreatyWorkflow>();
            treatyPricingTreatyWorkflows.HasRequired(q => q.ReinsuranceTypePickListDetail).WithMany().HasForeignKey(q => q.ReinsuranceTypePickListDetailId).WillCascadeOnDelete(false);
            treatyPricingTreatyWorkflows.HasOptional(q => q.BusinessOriginPickListDetail).WithMany().HasForeignKey(q => q.BusinessOriginPickListDetailId).WillCascadeOnDelete(false);
            treatyPricingTreatyWorkflows.HasRequired(q => q.CounterPartyDetail).WithMany().HasForeignKey(q => q.CounterPartyDetailId).WillCascadeOnDelete(false);
            treatyPricingTreatyWorkflows.HasOptional(q => q.InwardRetroPartyDetail).WithMany().HasForeignKey(q => q.InwardRetroPartyDetailId).WillCascadeOnDelete(false);
            treatyPricingTreatyWorkflows.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingTreatyWorkflows.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingTreatyWorkflowVersion> treatyPricingTreatyWorkflowVersions = modelBuilder.Entity<TreatyPricingTreatyWorkflowVersion>();
            treatyPricingTreatyWorkflowVersions.HasRequired(q => q.TreatyPricingTreatyWorkflow).WithMany().HasForeignKey(q => q.TreatyPricingTreatyWorkflowId).WillCascadeOnDelete(false);
            treatyPricingTreatyWorkflowVersions.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingTreatyWorkflowVersions.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);
            treatyPricingTreatyWorkflowVersions.HasOptional(q => q.PersonInCharge).WithMany().HasForeignKey(q => q.PersonInChargeId).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingPerLifeRetro> treatyPricingPerLifeRetro = modelBuilder.Entity<TreatyPricingPerLifeRetro>();
            treatyPricingPerLifeRetro.HasOptional(q => q.RetroParty).WithMany().HasForeignKey(q => q.RetroPartyId).WillCascadeOnDelete(false);
            treatyPricingPerLifeRetro.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingPerLifeRetro.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingPerLifeRetroVersion> treatyPricingPerLifeRetroVersions = modelBuilder.Entity<TreatyPricingPerLifeRetroVersion>();
            treatyPricingPerLifeRetroVersions.HasRequired(q => q.TreatyPricingPerLifeRetro).WithMany().HasForeignKey(q => q.TreatyPricingPerLifeRetroId).WillCascadeOnDelete(false);
            treatyPricingPerLifeRetroVersions.HasRequired(q => q.PersonInCharge).WithMany().HasForeignKey(q => q.PersonInChargeId).WillCascadeOnDelete(false);
            treatyPricingPerLifeRetroVersions.HasOptional(q => q.RetrocessionaireRetroParty).WithMany().HasForeignKey(q => q.RetrocessionaireRetroPartyId).WillCascadeOnDelete(false);
            treatyPricingPerLifeRetroVersions.HasOptional(q => q.PaymentRetrocessionairePremiumPickListDetail).WithMany().HasForeignKey(q => q.PaymentRetrocessionairePremiumPickListDetailId).WillCascadeOnDelete(false);
            treatyPricingPerLifeRetroVersions.HasOptional(q => q.JumboLimitCurrencyCodePickListDetail).WithMany().HasForeignKey(q => q.JumboLimitCurrencyCodePickListDetailId).WillCascadeOnDelete(false);
            treatyPricingPerLifeRetroVersions.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingPerLifeRetroVersions.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingPerLifeRetroVersionDetail> treatyPricingPerLifeRetroVersionDetails = modelBuilder.Entity<TreatyPricingPerLifeRetroVersionDetail>();
            treatyPricingPerLifeRetroVersionDetails.HasRequired(q => q.TreatyPricingPerLifeRetroVersion).WithMany().HasForeignKey(q => q.TreatyPricingPerLifeRetroVersionId).WillCascadeOnDelete(false);
            treatyPricingPerLifeRetroVersionDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingPerLifeRetroVersionDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingPerLifeRetroVersionTier> treatyPricingPerLifeRetroVersionTiers = modelBuilder.Entity<TreatyPricingPerLifeRetroVersionTier>();
            treatyPricingPerLifeRetroVersionTiers.HasRequired(q => q.TreatyPricingPerLifeRetroVersion).WithMany().HasForeignKey(q => q.TreatyPricingPerLifeRetroVersionId).WillCascadeOnDelete(false);
            treatyPricingPerLifeRetroVersionTiers.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingPerLifeRetroVersionTiers.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingPerLifeRetroVersionBenefit> treatyPricingPerLifeRetroVersionBenefits = modelBuilder.Entity<TreatyPricingPerLifeRetroVersionBenefit>();
            treatyPricingPerLifeRetroVersionBenefits.HasRequired(q => q.TreatyPricingPerLifeRetroVersion).WithMany().HasForeignKey(q => q.TreatyPricingPerLifeRetroVersionId).WillCascadeOnDelete(false);
            treatyPricingPerLifeRetroVersionBenefits.HasRequired(q => q.Benefit).WithMany().HasForeignKey(q => q.BenefitId).WillCascadeOnDelete(false);
            treatyPricingPerLifeRetroVersionBenefits.HasOptional(q => q.ArrangementRetrocessionnaireTypePickListDetail).WithMany().HasForeignKey(q => q.ArrangementRetrocessionnaireTypePickListDetailId).WillCascadeOnDelete(false);
            treatyPricingPerLifeRetroVersionBenefits.HasOptional(q => q.AgeBasisPickListDetail).WithMany().HasForeignKey(q => q.AgeBasisPickListDetailId).WillCascadeOnDelete(false);
            treatyPricingPerLifeRetroVersionBenefits.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingPerLifeRetroVersionBenefits.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingPerLifeRetroProduct> treatyPricingPerLifeRetroProducts = modelBuilder.Entity<TreatyPricingPerLifeRetroProduct>();
            treatyPricingPerLifeRetroProducts.HasRequired(q => q.TreatyPricingPerLifeRetro).WithMany().HasForeignKey(q => q.TreatyPricingPerLifeRetroId).WillCascadeOnDelete(false);
            treatyPricingPerLifeRetroProducts.HasRequired(q => q.TreatyPricingProduct).WithMany().HasForeignKey(q => q.TreatyPricingProductId).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingProductBenefitDirectRetro> treatyPricingProductBenefitDirectRetros = modelBuilder.Entity<TreatyPricingProductBenefitDirectRetro>();
            treatyPricingProductBenefitDirectRetros.HasRequired(q => q.TreatyPricingProductBenefit).WithMany().HasForeignKey(q => q.TreatyPricingProductBenefitId).WillCascadeOnDelete(false);
            treatyPricingProductBenefitDirectRetros.HasRequired(q => q.RetroParty).WithMany().HasForeignKey(q => q.RetroPartyId).WillCascadeOnDelete(false);
            treatyPricingProductBenefitDirectRetros.HasOptional(q => q.ArrangementRetrocessionTypePickListDetail).WithMany().HasForeignKey(q => q.ArrangementRetrocessionTypePickListDetailId).WillCascadeOnDelete(false);
            treatyPricingProductBenefitDirectRetros.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingProductBenefitDirectRetros.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingGroupReferral> treatyPricingGroupReferrals = modelBuilder.Entity<TreatyPricingGroupReferral>();
            treatyPricingGroupReferrals.HasRequired(q => q.Cedant).WithMany().HasForeignKey(q => q.CedantId).WillCascadeOnDelete(false);
            treatyPricingGroupReferrals.HasOptional(q => q.RiArrangementPickListDetail).WithMany().HasForeignKey(q => q.RiArrangementPickListDetailId).WillCascadeOnDelete(false);
            treatyPricingGroupReferrals.HasRequired(q => q.InsuredGroupName).WithMany().HasForeignKey(q => q.InsuredGroupNameId).WillCascadeOnDelete(false);
            treatyPricingGroupReferrals.HasRequired(q => q.PrimaryTreatyPricingProduct).WithMany().HasForeignKey(q => q.PrimaryTreatyPricingProductId).WillCascadeOnDelete(false);
            treatyPricingGroupReferrals.HasRequired(q => q.PrimaryTreatyPricingProductVersion).WithMany().HasForeignKey(q => q.PrimaryTreatyPricingProductVersionId).WillCascadeOnDelete(false);
            treatyPricingGroupReferrals.HasOptional(q => q.SecondaryTreatyPricingProduct).WithMany().HasForeignKey(q => q.SecondaryTreatyPricingProductId).WillCascadeOnDelete(false);
            treatyPricingGroupReferrals.HasOptional(q => q.SecondaryTreatyPricingProductVersion).WithMany().HasForeignKey(q => q.SecondaryTreatyPricingProductVersionId).WillCascadeOnDelete(false);
            treatyPricingGroupReferrals.HasOptional(q => q.IndustryNamePickListDetail).WithMany().HasForeignKey(q => q.IndustryNamePickListDetailId).WillCascadeOnDelete(false);
            treatyPricingGroupReferrals.HasOptional(q => q.ReferredTypePickListDetail).WithMany().HasForeignKey(q => q.ReferredTypePickListDetailId).WillCascadeOnDelete(false);
            treatyPricingGroupReferrals.HasOptional(q => q.RiGroupSlipPersonInCharge).WithMany().HasForeignKey(q => q.RiGroupSlipPersonInChargeId).WillCascadeOnDelete(false);
            treatyPricingGroupReferrals.HasOptional(q => q.RiGroupSlipVersion).WithMany().HasForeignKey(q => q.RiGroupSlipVersionId).WillCascadeOnDelete(false);
            treatyPricingGroupReferrals.HasOptional(q => q.RiGroupSlipTemplate).WithMany().HasForeignKey(q => q.RiGroupSlipTemplateId).WillCascadeOnDelete(false);
            treatyPricingGroupReferrals.HasOptional(q => q.ReplyVersion).WithMany().HasForeignKey(q => q.ReplyVersionId).WillCascadeOnDelete(false);
            treatyPricingGroupReferrals.HasOptional(q => q.ReplyTemplate).WithMany().HasForeignKey(q => q.ReplyTemplateId).WillCascadeOnDelete(false);
            treatyPricingGroupReferrals.HasOptional(q => q.TreatyPricingGroupMasterLetter).WithMany().HasForeignKey(q => q.TreatyPricingGroupMasterLetterId).WillCascadeOnDelete(false);
            treatyPricingGroupReferrals.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingGroupReferrals.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingGroupReferralVersion> treatyPricingGroupReferralVersions = modelBuilder.Entity<TreatyPricingGroupReferralVersion>();
            treatyPricingGroupReferralVersions.HasRequired(q => q.TreatyPricingGroupReferral).WithMany().HasForeignKey(q => q.TreatyPricingGroupReferralId).WillCascadeOnDelete(false);
            treatyPricingGroupReferralVersions.HasOptional(q => q.GroupReferralPIC).WithMany().HasForeignKey(q => q.GroupReferralPersonInChargeId).WillCascadeOnDelete(false);
            treatyPricingGroupReferralVersions.HasOptional(q => q.RequestTypePickListDetail).WithMany().HasForeignKey(q => q.RequestTypePickListDetailId).WillCascadeOnDelete(false);
            treatyPricingGroupReferralVersions.HasOptional(q => q.PremiumTypePickListDetail).WithMany().HasForeignKey(q => q.PremiumTypePickListDetailId).WillCascadeOnDelete(false);
            treatyPricingGroupReferralVersions.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingGroupReferralVersions.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingGroupReferralVersionBenefit> treatyPricingGroupReferralVersionBenefits = modelBuilder.Entity<TreatyPricingGroupReferralVersionBenefit>();
            treatyPricingGroupReferralVersionBenefits.HasRequired(q => q.TreatyPricingGroupReferralVersion).WithMany().HasForeignKey(q => q.TreatyPricingGroupReferralVersionId).WillCascadeOnDelete(false);
            treatyPricingGroupReferralVersionBenefits.HasRequired(q => q.Benefit).WithMany().HasForeignKey(q => q.BenefitId).WillCascadeOnDelete(false);
            treatyPricingGroupReferralVersionBenefits.HasOptional(q => q.ReinsuranceArrangementPickListDetail).WithMany().HasForeignKey(q => q.ReinsuranceArrangementPickListDetailId).WillCascadeOnDelete(false);
            treatyPricingGroupReferralVersionBenefits.HasOptional(q => q.AgeBasisPickListDetail).WithMany().HasForeignKey(q => q.AgeBasisPickListDetailId).WillCascadeOnDelete(false);
            treatyPricingGroupReferralVersionBenefits.HasOptional(q => q.TreatyPricingUwLimit).WithMany().HasForeignKey(q => q.TreatyPricingUwLimitId).WillCascadeOnDelete(false);
            treatyPricingGroupReferralVersionBenefits.HasOptional(q => q.TreatyPricingUwLimitVersion).WithMany().HasForeignKey(q => q.TreatyPricingUwLimitVersionId).WillCascadeOnDelete(false);
            treatyPricingGroupReferralVersionBenefits.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingGroupReferralVersionBenefits.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingProductPerLifeRetro> treatyPricingProductPerLifeRetros = modelBuilder.Entity<TreatyPricingProductPerLifeRetro>();
            treatyPricingProductPerLifeRetros.HasRequired(q => q.TreatyPricingProduct).WithMany().HasForeignKey(q => q.TreatyPricingProductId).WillCascadeOnDelete(false);
            treatyPricingProductPerLifeRetros.HasRequired(q => q.TreatyPricingPerLifeRetro).WithMany().HasForeignKey(q => q.TreatyPricingPerLifeRetroId).WillCascadeOnDelete(false);
            treatyPricingProductPerLifeRetros.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingGroupReferralFile> treatyPricingGroupReferralFiles = modelBuilder.Entity<TreatyPricingGroupReferralFile>();
            treatyPricingGroupReferralFiles.HasOptional(q => q.TreatyPricingGroupReferral).WithMany().HasForeignKey(q => q.TreatyPricingGroupReferralId).WillCascadeOnDelete(false);
            treatyPricingGroupReferralFiles.HasOptional(q => q.TableTypePickListDetail).WithMany().HasForeignKey(q => q.TableTypePickListDetailId).WillCascadeOnDelete(false);
            treatyPricingGroupReferralFiles.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingGroupReferralFiles.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingGroupReferralHipsTable> treatyPricingGroupReferralHipsTables = modelBuilder.Entity<TreatyPricingGroupReferralHipsTable>();
            treatyPricingGroupReferralHipsTables.HasRequired(q => q.TreatyPricingGroupReferral).WithMany().HasForeignKey(q => q.TreatyPricingGroupReferralId).WillCascadeOnDelete(false);
            treatyPricingGroupReferralHipsTables.HasOptional(q => q.TreatyPricingGroupReferralFile).WithMany().HasForeignKey(q => q.TreatyPricingGroupReferralFileId).WillCascadeOnDelete(false);
            treatyPricingGroupReferralHipsTables.HasOptional(q => q.HipsCategory).WithMany().HasForeignKey(q => q.HipsCategoryId).WillCascadeOnDelete(false);
            treatyPricingGroupReferralHipsTables.HasOptional(q => q.HipsSubCategory).WithMany().HasForeignKey(q => q.HipsSubCategoryId).WillCascadeOnDelete(false);
            treatyPricingGroupReferralHipsTables.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingGroupReferralHipsTables.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingGroupReferralGtlTable> treatyPricingGroupReferralGtlTables = modelBuilder.Entity<TreatyPricingGroupReferralGtlTable>();
            treatyPricingGroupReferralGtlTables.HasRequired(q => q.TreatyPricingGroupReferral).WithMany().HasForeignKey(q => q.TreatyPricingGroupReferralId).WillCascadeOnDelete(false);
            treatyPricingGroupReferralGtlTables.HasOptional(q => q.TreatyPricingGroupReferralFile).WithMany().HasForeignKey(q => q.TreatyPricingGroupReferralFileId).WillCascadeOnDelete(false);
            treatyPricingGroupReferralGtlTables.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingGroupReferralGtlTables.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingGroupReferralGhsTable> treatyPricingGroupReferralGhsTables = modelBuilder.Entity<TreatyPricingGroupReferralGhsTable>();
            treatyPricingGroupReferralGhsTables.HasRequired(q => q.TreatyPricingGroupReferral).WithMany().HasForeignKey(q => q.TreatyPricingGroupReferralId).WillCascadeOnDelete(false);
            treatyPricingGroupReferralGhsTables.HasOptional(q => q.TreatyPricingGroupReferralFile).WithMany().HasForeignKey(q => q.TreatyPricingGroupReferralFileId).WillCascadeOnDelete(false);
            treatyPricingGroupReferralGhsTables.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingGroupReferralGhsTables.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RetroBenefitCode> retroBenefitCodes = modelBuilder.Entity<RetroBenefitCode>();
            retroBenefitCodes.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            retroBenefitCodes.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RetroBenefitRetentionLimit> retroBenefitRetentionLimits = modelBuilder.Entity<RetroBenefitRetentionLimit>();
            retroBenefitRetentionLimits.HasRequired(q => q.RetroBenefitCode).WithMany().HasForeignKey(q => q.RetroBenefitCodeId).WillCascadeOnDelete(false);
            retroBenefitRetentionLimits.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            retroBenefitRetentionLimits.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RetroBenefitRetentionLimitDetail> retroBenefitRetentionLimitDetails = modelBuilder.Entity<RetroBenefitRetentionLimitDetail>();
            retroBenefitRetentionLimitDetails.HasRequired(q => q.RetroBenefitRetentionLimit).WithMany().HasForeignKey(q => q.RetroBenefitRetentionLimitId).WillCascadeOnDelete(false);
            retroBenefitRetentionLimitDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            retroBenefitRetentionLimitDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingReportGeneration> treatyPricingReportGenerations = modelBuilder.Entity<TreatyPricingReportGeneration>();
            treatyPricingReportGenerations.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingReportGenerations.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RetroBenefitCodeMapping> retroBenefitCodeMappings = modelBuilder.Entity<RetroBenefitCodeMapping>();
            retroBenefitCodeMappings.HasRequired(q => q.Benefit).WithMany().HasForeignKey(q => q.BenefitId).WillCascadeOnDelete(false);
            retroBenefitCodeMappings.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            retroBenefitCodeMappings.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RetroBenefitCodeMappingDetail> retroBenefitCodeMappingDetails = modelBuilder.Entity<RetroBenefitCodeMappingDetail>();
            retroBenefitCodeMappingDetails.HasRequired(q => q.RetroBenefitCodeMapping).WithMany(q => q.RetroBenefitCodeMappingDetails).HasForeignKey(q => q.RetroBenefitCodeMappingId).WillCascadeOnDelete(false);
            retroBenefitCodeMappingDetails.HasRequired(q => q.RetroBenefitCode).WithMany().HasForeignKey(q => q.RetroBenefitCodeId).WillCascadeOnDelete(false);
            retroBenefitCodeMappingDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            retroBenefitCodeMappingDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingGroupReferralChecklist> treatyPricingGroupReferralChecklists = modelBuilder.Entity<TreatyPricingGroupReferralChecklist>();
            treatyPricingGroupReferralChecklists.HasRequired(q => q.TreatyPricingGroupReferralVersion).WithMany().HasForeignKey(q => q.TreatyPricingGroupReferralVersionId).WillCascadeOnDelete(false);
            treatyPricingGroupReferralChecklists.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PerLifeDuplicationCheck> perLifeDuplicationCheck = modelBuilder.Entity<PerLifeDuplicationCheck>();
            perLifeDuplicationCheck.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            perLifeDuplicationCheck.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);
            treatyPricingGroupReferralChecklists.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PerLifeRetroGender> perLifeRetroGenders = modelBuilder.Entity<PerLifeRetroGender>();
            perLifeRetroGenders.HasOptional(q => q.InsuredGenderCodePickListDetail).WithMany().HasForeignKey(q => q.InsuredGenderCodePickListDetailId).WillCascadeOnDelete(false);
            perLifeRetroGenders.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            perLifeRetroGenders.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PerLifeRetroCountry> perLifeRetroCountries = modelBuilder.Entity<PerLifeRetroCountry>();
            perLifeRetroCountries.HasOptional(q => q.TerritoryOfIssueCodePickListDetail).WithMany().HasForeignKey(q => q.TerritoryOfIssueCodePickListDetailId).WillCascadeOnDelete(false);
            perLifeRetroCountries.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            perLifeRetroCountries.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<GstMaintenance> gstMaintenance = modelBuilder.Entity<GstMaintenance>();
            gstMaintenance.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            gstMaintenance.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RetroTreaty> retroTreaties = modelBuilder.Entity<RetroTreaty>();
            retroTreaties.HasOptional(q => q.RetroParty).WithMany().HasForeignKey(q => q.RetroPartyId).WillCascadeOnDelete(false);
            retroTreaties.HasRequired(q => q.TreatyTypePickListDetail).WithMany().HasForeignKey(q => q.TreatyTypePickListDetailId).WillCascadeOnDelete(false);
            retroTreaties.HasOptional(q => q.TreatyDiscountTable).WithMany().HasForeignKey(q => q.TreatyDiscountTableId).WillCascadeOnDelete(false);
            retroTreaties.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            retroTreaties.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ValidDuplicationList> validDuplicationList = modelBuilder.Entity<ValidDuplicationList>();
            validDuplicationList.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            validDuplicationList.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);
            validDuplicationList.HasOptional(q => q.InsuredGenderCodePickListDetail).WithMany().HasForeignKey(q => q.InsuredGenderCodePickListDetailId).WillCascadeOnDelete(false);
            validDuplicationList.HasOptional(q => q.MLReBenefitCode).WithMany().HasForeignKey(q => q.MLReBenefitCodeId).WillCascadeOnDelete(false);
            validDuplicationList.HasOptional(q => q.FundsAccountingTypePickListDetail).WithMany().HasForeignKey(q => q.FundsAccountingTypePickListDetailId).WillCascadeOnDelete(false);
            validDuplicationList.HasOptional(q => q.ReinsBasisCodePickListDetail).WithMany().HasForeignKey(q => q.ReinsBasisCodePickListDetailId).WillCascadeOnDelete(false);
            validDuplicationList.HasOptional(q => q.TransactionTypePickListDetail).WithMany().HasForeignKey(q => q.TransactionTypePickListDetailId).WillCascadeOnDelete(false);


            EntityTypeConfiguration<PerLifeRetroConfigurationTreaty> perLifeRetroConfigurationTreaties = modelBuilder.Entity<PerLifeRetroConfigurationTreaty>();
            perLifeRetroConfigurationTreaties.HasRequired(q => q.TreatyCode).WithMany().HasForeignKey(q => q.TreatyCodeId).WillCascadeOnDelete(false);
            perLifeRetroConfigurationTreaties.HasRequired(q => q.TreatyTypePickListDetail).WithMany().HasForeignKey(q => q.TreatyTypePickListDetailId).WillCascadeOnDelete(false);
            perLifeRetroConfigurationTreaties.HasRequired(q => q.FundsAccountingTypePickListDetail).WithMany().HasForeignKey(q => q.FundsAccountingTypePickListDetailId).WillCascadeOnDelete(false);
            perLifeRetroConfigurationTreaties.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            perLifeRetroConfigurationTreaties.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PerLifeRetroConfigurationRatio> perLifeRetroConfigurationRatios = modelBuilder.Entity<PerLifeRetroConfigurationRatio>();
            perLifeRetroConfigurationRatios.HasRequired(q => q.TreatyCode).WithMany().HasForeignKey(q => q.TreatyCodeId).WillCascadeOnDelete(false);
            perLifeRetroConfigurationRatios.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            perLifeRetroConfigurationRatios.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingGroupMasterLetter> treatyPricingGroupMasterLetters = modelBuilder.Entity<TreatyPricingGroupMasterLetter>();
            treatyPricingGroupMasterLetters.HasRequired(q => q.Cedant).WithMany().HasForeignKey(q => q.CedantId).WillCascadeOnDelete(false);
            treatyPricingGroupMasterLetters.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingGroupMasterLetters.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingGroupMasterLetterGroupReferral> treatyPricingGroupMasterLetterGroupReferrals = modelBuilder.Entity<TreatyPricingGroupMasterLetterGroupReferral>();
            treatyPricingGroupMasterLetterGroupReferrals.HasRequired(q => q.TreatyPricingGroupMasterLetter).WithMany().HasForeignKey(q => q.TreatyPricingGroupMasterLetterId).WillCascadeOnDelete(false);
            treatyPricingGroupMasterLetterGroupReferrals.HasRequired(q => q.TreatyPricingGroupReferral).WithMany().HasForeignKey(q => q.TreatyPricingGroupReferralId).WillCascadeOnDelete(false);
            treatyPricingGroupMasterLetterGroupReferrals.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RetroTreatyDetail> retroTreatyDetails = modelBuilder.Entity<RetroTreatyDetail>();
            retroTreatyDetails.HasRequired(q => q.RetroTreaty).WithMany().HasForeignKey(q => q.RetroTreatyId).WillCascadeOnDelete(false);
            retroTreatyDetails.HasRequired(q => q.PerLifeRetroConfigurationTreaty).WithMany().HasForeignKey(q => q.PerLifeRetroConfigurationTreatyId).WillCascadeOnDelete(false);
            retroTreatyDetails.HasOptional(q => q.PremiumSpreadTable).WithMany().HasForeignKey(q => q.PremiumSpreadTableId).WillCascadeOnDelete(false);
            retroTreatyDetails.HasOptional(q => q.TreatyDiscountTable).WithMany().HasForeignKey(q => q.TreatyDiscountTableId).WillCascadeOnDelete(false);
            retroTreatyDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            retroTreatyDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PerLifeAggregation> perLifeAggregations = modelBuilder.Entity<PerLifeAggregation>();
            perLifeAggregations.HasRequired(q => q.FundsAccountingTypePickListDetail).WithMany().HasForeignKey(q => q.FundsAccountingTypePickListDetailId).WillCascadeOnDelete(false);
            perLifeAggregations.HasRequired(q => q.CutOff).WithMany().HasForeignKey(q => q.CutOffId).WillCascadeOnDelete(false);
            perLifeAggregations.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            perLifeAggregations.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PerLifeDataCorrection> perLifeDataCorrections = modelBuilder.Entity<PerLifeDataCorrection>();
            perLifeDataCorrections.HasRequired(q => q.TreatyCode).WithMany().HasForeignKey(q => q.TreatyCodeId).WillCascadeOnDelete(false);
            perLifeDataCorrections.HasRequired(q => q.InsuredGenderCodePickListDetail).WithMany().HasForeignKey(q => q.InsuredGenderCodePickListDetailId).WillCascadeOnDelete(false);
            perLifeDataCorrections.HasRequired(q => q.TerritoryOfIssueCodePickListDetail).WithMany().HasForeignKey(q => q.TerritoryOfIssueCodePickListDetailId).WillCascadeOnDelete(false);
            perLifeDataCorrections.HasRequired(q => q.PerLifeRetroGender).WithMany().HasForeignKey(q => q.PerLifeRetroGenderId).WillCascadeOnDelete(false);
            perLifeDataCorrections.HasRequired(q => q.PerLifeRetroCountry).WithMany().HasForeignKey(q => q.PerLifeRetroCountryId).WillCascadeOnDelete(false);
            perLifeDataCorrections.HasRequired(q => q.ExceptionStatusPickListDetail).WithMany().HasForeignKey(q => q.ExceptionStatusPickListDetailId).WillCascadeOnDelete(false);
            perLifeDataCorrections.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            perLifeDataCorrections.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PerLifeAggregationDetail> perLifeAggregationDetails = modelBuilder.Entity<PerLifeAggregationDetail>();
            perLifeAggregationDetails.HasRequired(q => q.PerLifeAggregation).WithMany().HasForeignKey(q => q.PerLifeAggregationId).WillCascadeOnDelete(false);
            perLifeAggregationDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            perLifeAggregationDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RetroRegisterFile> retroRegisterFiles = modelBuilder.Entity<RetroRegisterFile>();
            retroRegisterFiles.HasRequired(q => q.RetroRegister).WithMany().HasForeignKey(q => q.RetroRegisterId).WillCascadeOnDelete(false);
            retroRegisterFiles.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            retroRegisterFiles.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PerLifeAggregationDetailData> perLifeAggregationDetailData = modelBuilder.Entity<PerLifeAggregationDetailData>();
            perLifeAggregationDetailData.HasRequired(q => q.PerLifeAggregationDetailTreaty).WithMany().HasForeignKey(q => q.PerLifeAggregationDetailTreatyId).WillCascadeOnDelete(false);
            //perLifeAggregationDetailData.HasRequired(q => q.RiDataWarehouseHistory).WithMany().HasForeignKey(q => q.RiDataWarehouseHistoryId).WillCascadeOnDelete(false);
            perLifeAggregationDetailData.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            perLifeAggregationDetailData.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PerLifeAggregationDuplicationListing> perLifeAggregationDuplicationListing = modelBuilder.Entity<PerLifeAggregationDuplicationListing>();
            perLifeAggregationDuplicationListing.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            perLifeAggregationDuplicationListing.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);
            perLifeAggregationDuplicationListing.HasOptional(q => q.InsuredGenderCodePickListDetail).WithMany().HasForeignKey(q => q.InsuredGenderCodePickListDetailId).WillCascadeOnDelete(false);
            perLifeAggregationDuplicationListing.HasOptional(q => q.MLReBenefitCode).WithMany().HasForeignKey(q => q.MLReBenefitCodeId).WillCascadeOnDelete(false);
            perLifeAggregationDuplicationListing.HasOptional(q => q.FundsAccountingTypePickListDetail).WithMany().HasForeignKey(q => q.FundsAccountingTypePickListDetailId).WillCascadeOnDelete(false);
            perLifeAggregationDuplicationListing.HasOptional(q => q.ReinsBasisCodePickListDetail).WithMany().HasForeignKey(q => q.ReinsBasisCodePickListDetailId).WillCascadeOnDelete(false);
            perLifeAggregationDuplicationListing.HasOptional(q => q.TransactionTypePickListDetail).WithMany().HasForeignKey(q => q.TransactionTypePickListDetailId).WillCascadeOnDelete(false);
            perLifeAggregationDuplicationListing.HasOptional(q => q.ExceptionStatusPickListDetail).WithMany().HasForeignKey(q => q.ExceptionStatusPickListDetailId).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PerLifeAggregationDetailTreaty> perLifeAggregationDetailTreaties = modelBuilder.Entity<PerLifeAggregationDetailTreaty>();
            perLifeAggregationDetailTreaties.HasRequired(q => q.PerLifeAggregationDetail).WithMany().HasForeignKey(q => q.PerLifeAggregationDetailId).WillCascadeOnDelete(false);
            perLifeAggregationDetailTreaties.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            perLifeAggregationDetailTreaties.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PerLifeAggregationConflictListing> perLifeAggregationConflictListing = modelBuilder.Entity<PerLifeAggregationConflictListing>();
            perLifeAggregationConflictListing.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            perLifeAggregationConflictListing.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);
            perLifeAggregationConflictListing.HasOptional(q => q.InsuredGenderCodePickListDetail).WithMany().HasForeignKey(q => q.InsuredGenderCodePickListDetailId).WillCascadeOnDelete(false);
            perLifeAggregationConflictListing.HasOptional(q => q.PremiumFrequencyModePickListDetail).WithMany().HasForeignKey(q => q.PremiumFrequencyModePickListDetailId).WillCascadeOnDelete(false);
            perLifeAggregationConflictListing.HasOptional(q => q.RetroPremiumFrequencyModePickListDetail).WithMany().HasForeignKey(q => q.RetroPremiumFrequencyModePickListDetailId).WillCascadeOnDelete(false);
            perLifeAggregationConflictListing.HasOptional(q => q.TerritoryOfIssueCodePickListDetail).WithMany().HasForeignKey(q => q.TerritoryOfIssueCodePickListDetailId).WillCascadeOnDelete(false);
            perLifeAggregationConflictListing.HasOptional(q => q.MLReBenefitCode).WithMany().HasForeignKey(q => q.MLReBenefitCodeId).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyPricingGroupReferralChecklistDetail> treatyPricingGroupReferralChecklistDetails = modelBuilder.Entity<TreatyPricingGroupReferralChecklistDetail>();
            treatyPricingGroupReferralChecklistDetails.HasRequired(q => q.TreatyPricingGroupReferralVersion).WithMany().HasForeignKey(q => q.TreatyPricingGroupReferralVersionId).WillCascadeOnDelete(false);
            treatyPricingGroupReferralChecklistDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyPricingGroupReferralChecklistDetails.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PerLifeDuplicationCheckDetail> perLifeDuplicationCheckDetails = modelBuilder.Entity<PerLifeDuplicationCheckDetail>();
            perLifeDuplicationCheckDetails.HasRequired(q => q.PerLifeDuplicationCheck).WithMany(q => q.PerLifeDuplicationCheckDetails).HasForeignKey(q => q.PerLifeDuplicationCheckId).WillCascadeOnDelete(false);
            perLifeDuplicationCheckDetails.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<StandardRetroOutput> standardRetroOutputs = modelBuilder.Entity<StandardRetroOutput>();
            standardRetroOutputs.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            standardRetroOutputs.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PerLifeAggregationMonthlyData> perLifeAggregationMonthlyData = modelBuilder.Entity<PerLifeAggregationMonthlyData>();
            perLifeAggregationMonthlyData.HasRequired(q => q.PerLifeAggregationDetailData).WithMany().HasForeignKey(q => q.PerLifeAggregationDetailDataId).WillCascadeOnDelete(false);
            perLifeAggregationMonthlyData.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            perLifeAggregationMonthlyData.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PerLifeAggregatedData> perLifeAggregatedData = modelBuilder.Entity<PerLifeAggregatedData>();
            perLifeAggregatedData.HasRequired(q => q.PerLifeAggregationDetail).WithMany().HasForeignKey(q => q.PerLifeAggregationDetailId).WillCascadeOnDelete(false);
            perLifeAggregatedData.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            perLifeAggregatedData.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PerLifeAggregationMonthlyRetroData> perLifeAggregationMonthlyRetroData = modelBuilder.Entity<PerLifeAggregationMonthlyRetroData>();
            perLifeAggregationMonthlyRetroData.HasRequired(q => q.PerLifeAggregationMonthlyData).WithMany(q => q.PerLifeAggregationMonthlyRetroData).HasForeignKey(q => q.PerLifeAggregationMonthlyDataId).WillCascadeOnDelete(false);
            perLifeAggregationMonthlyRetroData.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            perLifeAggregationMonthlyRetroData.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PerLifeClaim> perLifeClaims = modelBuilder.Entity<PerLifeClaim>();
            perLifeClaims.HasRequired(q => q.FundsAccountingTypePickListDetail).WithMany().HasForeignKey(q => q.FundsAccountingTypePickListDetailId).WillCascadeOnDelete(false);
            perLifeClaims.HasRequired(q => q.CutOff).WithMany().HasForeignKey(q => q.CutOffId).WillCascadeOnDelete(false);
            perLifeClaims.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            perLifeClaims.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PerLifeClaimData> perLifeClaimData = modelBuilder.Entity<PerLifeClaimData>();
            perLifeClaimData.HasRequired(q => q.PerLifeClaim).WithMany().HasForeignKey(q => q.PerLifeClaimId).WillCascadeOnDelete(false);
            perLifeClaimData.HasRequired(q => q.ClaimRegisterHistory).WithMany().HasForeignKey(q => q.ClaimRegisterHistoryId).WillCascadeOnDelete(false);
            perLifeClaimData.HasOptional(q => q.PerLifeAggregationDetailData).WithMany().HasForeignKey(q => q.PerLifeAggregationDetailDataId).WillCascadeOnDelete(false);
            perLifeClaimData.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            perLifeClaimData.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PerLifeClaimRetroData> perLifeClaimRetroData = modelBuilder.Entity<PerLifeClaimRetroData>();
            perLifeClaimRetroData.HasRequired(q => q.PerLifeClaimData).WithMany().HasForeignKey(q => q.PerLifeClaimDataId).WillCascadeOnDelete(false);
            perLifeClaimRetroData.HasOptional(q => q.RetroTreaty).WithMany().HasForeignKey(q => q.RetroTreatyId).WillCascadeOnDelete(false);
            perLifeClaimRetroData.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            perLifeClaimRetroData.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PerLifeSoa> perLifeSoa = modelBuilder.Entity<PerLifeSoa>();
            perLifeSoa.HasRequired(q => q.RetroParty).WithMany().HasForeignKey(q => q.RetroPartyId).WillCascadeOnDelete(false);
            perLifeSoa.HasRequired(q => q.RetroTreaty).WithMany().HasForeignKey(q => q.RetroTreatyId).WillCascadeOnDelete(false);
            perLifeSoa.HasOptional(q => q.PersonInCharge).WithMany().HasForeignKey(q => q.PersonInChargeId).WillCascadeOnDelete(false);
            perLifeSoa.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            perLifeSoa.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);
            perLifeSoa.HasOptional(q => q.PerLifeAggregation).WithMany().HasForeignKey(q => q.PerLifeAggregationId).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PerLifeSoaData> perLifeSoaData = modelBuilder.Entity<PerLifeSoaData>();
            perLifeSoaData.HasRequired(q => q.PerLifeSoa).WithMany().HasForeignKey(q => q.PerLifeSoaId).WillCascadeOnDelete(false);
            perLifeSoaData.HasOptional(q => q.PerLifeAggregationDetailData).WithMany().HasForeignKey(q => q.PerLifeAggregationDetailDataId).WillCascadeOnDelete(false);
            perLifeSoaData.HasOptional(q => q.PerLifeClaimData).WithMany().HasForeignKey(q => q.PerLifeClaimDataId).WillCascadeOnDelete(false);
            perLifeSoaData.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            perLifeSoaData.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RetroBenefitCodeMappingTreaty> retroBenefitCodeMappingTreaty = modelBuilder.Entity<RetroBenefitCodeMappingTreaty>();
            retroBenefitCodeMappingTreaty.HasRequired(q => q.RetroBenefitCodeMapping).WithMany(q => q.RetroBenefitCodeMappingTreaties).HasForeignKey(q => q.RetroBenefitCodeMappingId).WillCascadeOnDelete(false);
            retroBenefitCodeMappingTreaty.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            retroBenefitCodeMappingTreaty.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PerLifeRetroStatement> perLifeRetroStatements = modelBuilder.Entity<PerLifeRetroStatement>();
            perLifeRetroStatements.HasRequired(q => q.PerLifeSoa).WithMany().HasForeignKey(q => q.PerLifeSoaId).WillCascadeOnDelete(false);
            perLifeRetroStatements.HasRequired(q => q.RetroParty).WithMany().HasForeignKey(q => q.RetroPartyId).WillCascadeOnDelete(false);
            perLifeRetroStatements.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            perLifeRetroStatements.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PerLifeSoaSummaries> perLifeSoaSummaries = modelBuilder.Entity<PerLifeSoaSummaries>();
            perLifeSoaSummaries.HasRequired(q => q.PerLifeSoa).WithMany().HasForeignKey(q => q.PerLifeSoaId).WillCascadeOnDelete(false);
            perLifeSoaSummaries.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            perLifeSoaSummaries.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PerLifeSoaSummariesByTreaty> perLifeSoaSummariesByTreaty = modelBuilder.Entity<PerLifeSoaSummariesByTreaty>();
            perLifeSoaSummariesByTreaty.HasRequired(q => q.PerLifeSoa).WithMany().HasForeignKey(q => q.PerLifeSoaId).WillCascadeOnDelete(false);
            perLifeSoaSummariesByTreaty.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            perLifeSoaSummariesByTreaty.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<PerLifeSoaSummariesSoa> perLifeSoaSummariesSoa = modelBuilder.Entity<PerLifeSoaSummariesSoa>();
            perLifeSoaSummariesSoa.HasRequired(q => q.PerLifeSoa).WithMany().HasForeignKey(q => q.PerLifeSoaId).WillCascadeOnDelete(false);
            perLifeSoaSummariesSoa.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            perLifeSoaSummariesSoa.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<ObjectLock> objectLocks = modelBuilder.Entity<ObjectLock>();
            objectLocks.HasRequired(q => q.Module).WithMany().HasForeignKey(q => q.ModuleId).WillCascadeOnDelete(false);
            objectLocks.HasRequired(q => q.LockedBy).WithMany().HasForeignKey(q => q.LockedById).WillCascadeOnDelete(false);
            objectLocks.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            objectLocks.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<TreatyBenefitCodeMappingUpload> treatyBenefitCodeMappingUpload = modelBuilder.Entity<TreatyBenefitCodeMappingUpload>();
            treatyBenefitCodeMappingUpload.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            treatyBenefitCodeMappingUpload.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RateTableMappingUpload> rateTableMappingUpload = modelBuilder.Entity<RateTableMappingUpload>();
            rateTableMappingUpload.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            rateTableMappingUpload.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<FacMasterListingUpload> facMasterListingUpload = modelBuilder.Entity<FacMasterListingUpload>();
            facMasterListingUpload.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            facMasterListingUpload.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);

            EntityTypeConfiguration<RateDetailUpload> rateDetailUpload = modelBuilder.Entity<RateDetailUpload>();
            rateDetailUpload.HasRequired(q => q.Rate).WithMany().HasForeignKey(q => q.RateId).WillCascadeOnDelete(false);
            rateDetailUpload.HasRequired(q => q.CreatedBy).WithMany().HasForeignKey(q => q.CreatedById).WillCascadeOnDelete(false);
            rateDetailUpload.HasOptional(q => q.UpdatedBy).WithMany().HasForeignKey(q => q.UpdatedById).WillCascadeOnDelete(false);
        }

        /// <summary>
        ///     Validates that UserNames are unique and case insenstive
        /// </summary>
        /// <param name="entityEntry"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            if (entityEntry != null && entityEntry.State == EntityState.Added)
            {
                var errors = new List<DbValidationError>();
                //check for uniqueness of user name and email
                if (entityEntry.Entity is User user)
                {
                    if (Users.Any(u => string.Equals(u.UserName, user.UserName) && u.Status != UserBo.StatusDelete))
                    {
                        errors.Add(new DbValidationError("User",
                            string.Format(CultureInfo.CurrentCulture, MessageBag.DuplicateUsername, user.UserName)));
                    }
                    if (RequireUniqueEmail && Users.Any(u => string.Equals(u.Email, user.Email) && u.Status != UserBo.StatusDelete))
                    {
                        errors.Add(new DbValidationError("User",
                            string.Format(CultureInfo.CurrentCulture, MessageBag.DuplicateEmail, user.Email)));
                    }
                    if (errors.Any())
                    {
                        return new DbEntityValidationResult(entityEntry, errors);
                    }
                    return new DbEntityValidationResult(entityEntry, Enumerable.Empty<DbValidationError>());
                }
            }
            return base.ValidateEntity(entityEntry, items);
        }
    }
}
