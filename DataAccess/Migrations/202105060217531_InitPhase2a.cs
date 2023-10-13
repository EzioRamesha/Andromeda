namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class InitPhase2a : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Mfrs17ContractCodeDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Mfrs17ContractCodeDetails", "Mfrs17ContractCodeId", "dbo.Mfrs17ContractCodes");
            CreateTable(
                "dbo.Designations",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Code = c.String(nullable: false, maxLength: 255),
                    Description = c.String(nullable: false, maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Code)
                .Index(t => t.Description)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.GstMaintenances",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    EffectiveStartDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    EffectiveEndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    RiskEffectiveStartDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    RiskEffectiveEndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    Rate = c.Double(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.EffectiveStartDate)
                .Index(t => t.EffectiveEndDate)
                .Index(t => t.RiskEffectiveStartDate)
                .Index(t => t.RiskEffectiveEndDate)
                .Index(t => t.Rate)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.GtlBenefitCategories",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Category = c.String(nullable: false, maxLength: 255),
                    Benefit = c.String(nullable: false, maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Category)
                .Index(t => t.Benefit)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.HipsCategories",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Code = c.String(nullable: false, maxLength: 255),
                    Name = c.String(nullable: false, maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Code)
                .Index(t => t.Name)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.HipsCategoryDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    HipsCategoryId = c.Int(nullable: false),
                    Subcategory = c.String(nullable: false, maxLength: 20),
                    Description = c.String(nullable: false, maxLength: 255),
                    ItemType = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.HipsCategories", t => t.HipsCategoryId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.HipsCategoryId)
                .Index(t => t.Subcategory)
                .Index(t => t.Description)
                .Index(t => t.ItemType)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.InsuredGroupNames",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 255),
                    Description = c.String(maxLength: 255),
                    Status = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Name)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.PerLifeAggregationConflictListings",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyCodeId = c.Int(),
                    RiskYear = c.Int(),
                    RiskMonth = c.Int(),
                    InsuredName = c.String(maxLength: 255),
                    InsuredGenderCodePickListDetailId = c.Int(),
                    InsuredDateOfBirth = c.DateTime(),
                    PolicyNumber = c.String(maxLength: 50),
                    ReinsEffectiveDatePol = c.DateTime(),
                    AAR = c.Double(),
                    GrossPremium = c.Double(),
                    NetPremium = c.Double(),
                    PremiumFrequencyModePickListDetailId = c.Int(),
                    RetroPremiumFrequencyModePickListDetailId = c.Int(),
                    CedantPlanCode = c.String(maxLength: 255),
                    CedingBenefitRiskCode = c.String(maxLength: 30),
                    CedingBenefitTypeCode = c.String(maxLength: 30),
                    MLReBenefitCodeId = c.Int(),
                    TerritoryOfIssueCodePickListDetailId = c.Int(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickListDetails", t => t.InsuredGenderCodePickListDetailId)
                .ForeignKey("dbo.Benefits", t => t.MLReBenefitCodeId)
                .ForeignKey("dbo.PickListDetails", t => t.PremiumFrequencyModePickListDetailId)
                .ForeignKey("dbo.PickListDetails", t => t.RetroPremiumFrequencyModePickListDetailId)
                .ForeignKey("dbo.PickListDetails", t => t.TerritoryOfIssueCodePickListDetailId)
                .ForeignKey("dbo.TreatyCodes", t => t.TreatyCodeId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyCodeId)
                .Index(t => t.RiskYear)
                .Index(t => t.RiskMonth)
                .Index(t => t.InsuredName)
                .Index(t => t.InsuredGenderCodePickListDetailId)
                .Index(t => t.InsuredDateOfBirth)
                .Index(t => t.PolicyNumber)
                .Index(t => t.ReinsEffectiveDatePol)
                .Index(t => t.AAR)
                .Index(t => t.GrossPremium)
                .Index(t => t.NetPremium)
                .Index(t => t.PremiumFrequencyModePickListDetailId)
                .Index(t => t.RetroPremiumFrequencyModePickListDetailId)
                .Index(t => t.CedantPlanCode)
                .Index(t => t.CedingBenefitRiskCode)
                .Index(t => t.CedingBenefitTypeCode)
                .Index(t => t.MLReBenefitCodeId)
                .Index(t => t.TerritoryOfIssueCodePickListDetailId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.PerLifeAggregationDetailData",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PerLifeAggregationDetailTreatyId = c.Int(nullable: false),
                    RiDataWarehouseHistoryId = c.Long(nullable: false),
                    ExpectedGenderCode = c.String(maxLength: 15),
                    RetroBenefitCode = c.String(maxLength: 30),
                    ExpectedTerritoryOfIssueCode = c.String(maxLength: 50),
                    FlagCode = c.Int(),
                    ExceptionType = c.Int(),
                    IsException = c.Boolean(nullable: false),
                    Errors = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PerLifeAggregationDetailTreaties", t => t.PerLifeAggregationDetailTreatyId)
                .ForeignKey("dbo.RiDataWarehouseHistories", t => t.RiDataWarehouseHistoryId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.PerLifeAggregationDetailTreatyId)
                .Index(t => t.RiDataWarehouseHistoryId)
                .Index(t => t.IsException)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.PerLifeAggregationDetailTreaties",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PerLifeAggregationDetailId = c.Int(nullable: false),
                    TreatyCode = c.String(nullable: false, maxLength: 35),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PerLifeAggregationDetails", t => t.PerLifeAggregationDetailId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.PerLifeAggregationDetailId)
                .Index(t => t.TreatyCode)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.PerLifeAggregationDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PerLifeAggregationId = c.Int(nullable: false),
                    RiskQuarter = c.String(maxLength: 64),
                    ProcessingDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    Status = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PerLifeAggregations", t => t.PerLifeAggregationId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.PerLifeAggregationId)
                .Index(t => t.RiskQuarter)
                .Index(t => t.ProcessingDate)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.PerLifeAggregations",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FundsAccountingTypePickListDetailId = c.Int(nullable: false),
                    CutOffId = c.Int(nullable: false),
                    SoaQuarter = c.String(nullable: false, maxLength: 64),
                    ProcessingDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    Status = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.CutOff", t => t.CutOffId)
                .ForeignKey("dbo.PickListDetails", t => t.FundsAccountingTypePickListDetailId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.FundsAccountingTypePickListDetailId)
                .Index(t => t.CutOffId)
                .Index(t => t.SoaQuarter)
                .Index(t => t.ProcessingDate)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.PerLifeAggregationDuplicationListings",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyCodeId = c.Int(),
                    InsuredName = c.String(maxLength: 100),
                    InsuredGenderCodePickListDetailId = c.Int(),
                    InsuredDateOfBirth = c.DateTime(),
                    PolicyNumber = c.String(maxLength: 50),
                    ReinsuranceEffectiveDate = c.DateTime(),
                    FundsAccountingTypePickListDetailId = c.Int(),
                    ReinsBasisCodePickListDetailId = c.Int(),
                    CedantPlanCode = c.String(maxLength: 255),
                    MLReBenefitCodeId = c.Int(),
                    CedingBenefitRiskCode = c.String(maxLength: 30),
                    CedingBenefitTypeCode = c.String(maxLength: 30),
                    TransactionTypePickListDetailId = c.Int(),
                    ProceedToAggregate = c.Int(),
                    DateUpdated = c.DateTime(),
                    ExceptionStatusPickListDetailId = c.Int(),
                    Remarks = c.String(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickListDetails", t => t.ExceptionStatusPickListDetailId)
                .ForeignKey("dbo.PickListDetails", t => t.FundsAccountingTypePickListDetailId)
                .ForeignKey("dbo.PickListDetails", t => t.InsuredGenderCodePickListDetailId)
                .ForeignKey("dbo.Benefits", t => t.MLReBenefitCodeId)
                .ForeignKey("dbo.PickListDetails", t => t.ReinsBasisCodePickListDetailId)
                .ForeignKey("dbo.PickListDetails", t => t.TransactionTypePickListDetailId)
                .ForeignKey("dbo.TreatyCodes", t => t.TreatyCodeId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyCodeId)
                .Index(t => t.InsuredName)
                .Index(t => t.InsuredGenderCodePickListDetailId)
                .Index(t => t.InsuredDateOfBirth)
                .Index(t => t.PolicyNumber)
                .Index(t => t.ReinsuranceEffectiveDate)
                .Index(t => t.FundsAccountingTypePickListDetailId)
                .Index(t => t.ReinsBasisCodePickListDetailId)
                .Index(t => t.CedantPlanCode)
                .Index(t => t.MLReBenefitCodeId)
                .Index(t => t.CedingBenefitRiskCode)
                .Index(t => t.CedingBenefitTypeCode)
                .Index(t => t.TransactionTypePickListDetailId)
                .Index(t => t.ProceedToAggregate)
                .Index(t => t.DateUpdated)
                .Index(t => t.ExceptionStatusPickListDetailId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.PerLifeDataCorrections",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyCodeId = c.Int(nullable: false),
                    InsuredName = c.String(nullable: false, maxLength: 128),
                    InsuredDateOfBirth = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    PolicyNumber = c.String(nullable: false, maxLength: 150),
                    InsuredGenderCodePickListDetailId = c.Int(nullable: false),
                    TerritoryOfIssueCodePickListDetailId = c.Int(nullable: false),
                    PerLifeRetroGenderId = c.Int(nullable: false),
                    PerLifeRetroCountryId = c.Int(nullable: false),
                    DateOfPolicyExist = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    IsProceedToAggregate = c.Boolean(nullable: false),
                    DateOfExceptionDetected = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ExceptionStatusPickListDetailId = c.Int(nullable: false),
                    Remark = c.String(storeType: "ntext"),
                    DateUpdated = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickListDetails", t => t.ExceptionStatusPickListDetailId)
                .ForeignKey("dbo.PickListDetails", t => t.InsuredGenderCodePickListDetailId)
                .ForeignKey("dbo.PerLifeRetroCountries", t => t.PerLifeRetroCountryId)
                .ForeignKey("dbo.PerLifeRetroGenders", t => t.PerLifeRetroGenderId)
                .ForeignKey("dbo.PickListDetails", t => t.TerritoryOfIssueCodePickListDetailId)
                .ForeignKey("dbo.TreatyCodes", t => t.TreatyCodeId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyCodeId)
                .Index(t => t.InsuredName)
                .Index(t => t.InsuredDateOfBirth)
                .Index(t => t.PolicyNumber)
                .Index(t => t.InsuredGenderCodePickListDetailId)
                .Index(t => t.TerritoryOfIssueCodePickListDetailId)
                .Index(t => t.PerLifeRetroGenderId)
                .Index(t => t.PerLifeRetroCountryId)
                .Index(t => t.DateOfPolicyExist)
                .Index(t => t.IsProceedToAggregate)
                .Index(t => t.DateOfExceptionDetected)
                .Index(t => t.ExceptionStatusPickListDetailId)
                .Index(t => t.DateUpdated)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.PerLifeRetroCountries",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Country = c.String(nullable: false, maxLength: 50),
                    EffectiveStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    EffectiveEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Country)
                .Index(t => t.EffectiveStartDate)
                .Index(t => t.EffectiveEndDate)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.PerLifeRetroGenders",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Gender = c.String(nullable: false, maxLength: 15),
                    EffectiveStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    EffectiveEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Gender)
                .Index(t => t.EffectiveStartDate)
                .Index(t => t.EffectiveEndDate)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.PerLifeDuplicationCheckDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PerLifeDuplicationCheckId = c.Int(nullable: false),
                    TreatyCode = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PerLifeDuplicationChecks", t => t.PerLifeDuplicationCheckId)
                .Index(t => t.PerLifeDuplicationCheckId)
                .Index(t => t.TreatyCode)
                .Index(t => t.CreatedById);

            CreateTable(
                "dbo.PerLifeDuplicationChecks",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ConfigurationCode = c.String(nullable: false, maxLength: 30),
                    Description = c.String(maxLength: 255),
                    Inclusion = c.Boolean(nullable: false),
                    ReinsuranceEffectiveStartDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    ReinsuranceEffectiveEndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    TreatyCode = c.String(),
                    NoOfTreatyCode = c.Int(nullable: false),
                    EnableReinsuranceBasisCodeCheck = c.Boolean(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.ConfigurationCode)
                .Index(t => t.Description)
                .Index(t => t.Inclusion)
                .Index(t => t.ReinsuranceEffectiveStartDate)
                .Index(t => t.ReinsuranceEffectiveEndDate)
                .Index(t => t.EnableReinsuranceBasisCodeCheck)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.PerLifeRetroConfigurationRatios",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyCodeId = c.Int(nullable: false),
                    RetroRatio = c.Double(nullable: false),
                    MlreRetainRatio = c.Double(nullable: false),
                    ReinsEffectiveStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ReinsEffectiveEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    RiskQuarterStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    RiskQuarterEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    RuleEffectiveDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    RuleCeaseDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    RuleValue = c.Double(nullable: false),
                    Description = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyCodes", t => t.TreatyCodeId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyCodeId)
                .Index(t => t.RetroRatio)
                .Index(t => t.MlreRetainRatio)
                .Index(t => t.ReinsEffectiveStartDate)
                .Index(t => t.ReinsEffectiveEndDate)
                .Index(t => t.RiskQuarterStartDate)
                .Index(t => t.RiskQuarterEndDate)
                .Index(t => t.RuleEffectiveDate)
                .Index(t => t.RuleCeaseDate)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.PerLifeRetroConfigurationTreaties",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyCodeId = c.Int(nullable: false),
                    TreatyTypePickListDetailId = c.Int(nullable: false),
                    BusinessTypePickListDetailId = c.Int(nullable: false),
                    ReinsEffectiveStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ReinsEffectiveEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    RiskQuarterStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    RiskQuarterEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    IsToAggregate = c.Boolean(nullable: false),
                    Remark = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PickListDetails", t => t.BusinessTypePickListDetailId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyCodes", t => t.TreatyCodeId)
                .ForeignKey("dbo.PickListDetails", t => t.TreatyTypePickListDetailId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyCodeId)
                .Index(t => t.TreatyTypePickListDetailId)
                .Index(t => t.BusinessTypePickListDetailId)
                .Index(t => t.ReinsEffectiveStartDate)
                .Index(t => t.ReinsEffectiveEndDate)
                .Index(t => t.RiskQuarterStartDate)
                .Index(t => t.RiskQuarterEndDate)
                .Index(t => t.IsToAggregate)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RetroBenefitCodeMappingDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    RetroBenefitCodeMappingId = c.Int(nullable: false),
                    RetroBenefitCodeId = c.Int(nullable: false),
                    IsComputePremium = c.Boolean(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.RetroBenefitCodes", t => t.RetroBenefitCodeId)
                .ForeignKey("dbo.RetroBenefitCodeMappings", t => t.RetroBenefitCodeMappingId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.RetroBenefitCodeMappingId)
                .Index(t => t.RetroBenefitCodeId)
                .Index(t => t.IsComputePremium)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RetroBenefitCodes",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Code = c.String(nullable: false, maxLength: 30),
                    Description = c.String(nullable: false, maxLength: 255),
                    EffectiveDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CeaseDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    Status = c.Int(nullable: false),
                    Remarks = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Code)
                .Index(t => t.Description)
                .Index(t => t.EffectiveDate)
                .Index(t => t.CeaseDate)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RetroBenefitCodeMappings",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    BenefitId = c.Int(nullable: false),
                    IsPerAnnum = c.Boolean(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Benefits", t => t.BenefitId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.BenefitId)
                .Index(t => t.IsPerAnnum)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RetroBenefitRetentionLimitDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    RetroBenefitRetentionLimitId = c.Int(nullable: false),
                    MinIssueAge = c.Int(nullable: false),
                    MaxIssueAge = c.Int(nullable: false),
                    MortalityLimitFrom = c.Double(nullable: false),
                    MortalityLimitTo = c.Double(nullable: false),
                    ReinsEffStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ReinsEffEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    MlreRetentionAmount = c.Double(nullable: false),
                    MinReinsAmount = c.Double(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.RetroBenefitRetentionLimits", t => t.RetroBenefitRetentionLimitId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.RetroBenefitRetentionLimitId)
                .Index(t => t.MinIssueAge)
                .Index(t => t.MaxIssueAge)
                .Index(t => t.MortalityLimitFrom)
                .Index(t => t.MortalityLimitTo)
                .Index(t => t.ReinsEffStartDate)
                .Index(t => t.ReinsEffEndDate)
                .Index(t => t.MlreRetentionAmount)
                .Index(t => t.MinReinsAmount)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RetroBenefitRetentionLimits",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    RetroBenefitCodeId = c.Int(nullable: false),
                    Type = c.Int(nullable: false),
                    Description = c.String(nullable: false, maxLength: 255),
                    EffectiveStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    EffectiveEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    MinRetentionLimit = c.Double(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.RetroBenefitCodes", t => t.RetroBenefitCodeId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.RetroBenefitCodeId)
                .Index(t => t.Type)
                .Index(t => t.Description)
                .Index(t => t.MinRetentionLimit)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RetroRegisterFiles",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    RetroRegisterId = c.Long(nullable: false),
                    FileName = c.String(maxLength: 255),
                    HashFileName = c.String(maxLength: 255),
                    Status = c.Int(nullable: false),
                    Errors = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.RetroRegister", t => t.RetroRegisterId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.RetroRegisterId)
                .Index(t => t.FileName)
                .Index(t => t.HashFileName)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RetroTreaties",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Status = c.Int(nullable: false),
                    Party = c.String(nullable: false, maxLength: 50),
                    Code = c.String(nullable: false, maxLength: 50),
                    TreatyTypePickListDetailId = c.Int(nullable: false),
                    IsLobAutomatic = c.Boolean(nullable: false),
                    IsLobFacultative = c.Boolean(nullable: false),
                    IsLobAdvantageProgram = c.Boolean(nullable: false),
                    RetroShare = c.Double(),
                    TreatyDiscountTableId = c.Int(),
                    EffectiveStartDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    EffectiveEndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyDiscountTables", t => t.TreatyDiscountTableId)
                .ForeignKey("dbo.PickListDetails", t => t.TreatyTypePickListDetailId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Status)
                .Index(t => t.Party)
                .Index(t => t.Code)
                .Index(t => t.TreatyTypePickListDetailId)
                .Index(t => t.IsLobAutomatic)
                .Index(t => t.IsLobFacultative)
                .Index(t => t.IsLobAdvantageProgram)
                .Index(t => t.RetroShare)
                .Index(t => t.TreatyDiscountTableId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RetroTreatyDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    RetroTreatyId = c.Int(nullable: false),
                    PerLifeRetroConfigurationTreatyId = c.Int(nullable: false),
                    PremiumSpreadTableId = c.Int(),
                    TreatyDiscountTableId = c.Int(),
                    MlreShare = c.Double(nullable: false),
                    GrossRetroPremium = c.String(storeType: "ntext"),
                    TreatyDiscount = c.String(storeType: "ntext"),
                    NetRetroPremium = c.String(storeType: "ntext"),
                    Remark = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PerLifeRetroConfigurationTreaties", t => t.PerLifeRetroConfigurationTreatyId)
                .ForeignKey("dbo.PremiumSpreadTables", t => t.PremiumSpreadTableId)
                .ForeignKey("dbo.RetroTreaties", t => t.RetroTreatyId)
                .ForeignKey("dbo.TreatyDiscountTables", t => t.TreatyDiscountTableId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.RetroTreatyId)
                .Index(t => t.PerLifeRetroConfigurationTreatyId)
                .Index(t => t.PremiumSpreadTableId)
                .Index(t => t.TreatyDiscountTableId)
                .Index(t => t.MlreShare)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TemplateDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TemplateId = c.Int(nullable: false),
                    TemplateVersion = c.Int(nullable: false),
                    FileName = c.String(maxLength: 255),
                    HashFileName = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Templates", t => t.TemplateId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TemplateId)
                .Index(t => t.FileName)
                .Index(t => t.HashFileName)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.Templates",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Code = c.String(nullable: false, maxLength: 255),
                    CedantId = c.Int(nullable: false),
                    DocumentTypeId = c.String(nullable: false, maxLength: 255),
                    Description = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cedants", t => t.CedantId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Code)
                .Index(t => t.CedantId)
                .Index(t => t.DocumentTypeId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingAdvantagePrograms",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingCedantId = c.Int(nullable: false),
                    Code = c.String(nullable: false, maxLength: 30),
                    Name = c.String(maxLength: 255),
                    Description = c.String(maxLength: 255),
                    Status = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingCedants", t => t.TreatyPricingCedantId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingCedantId)
                .Index(t => t.Code)
                .Index(t => t.Name)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingCedants",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CedantId = c.Int(nullable: false),
                    ReinsuranceTypePickListDetailId = c.Int(nullable: false),
                    Code = c.String(nullable: false, maxLength: 30),
                    NoOfProduct = c.Int(nullable: false),
                    NoOfDocument = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cedants", t => t.CedantId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickListDetails", t => t.ReinsuranceTypePickListDetailId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.CedantId)
                .Index(t => t.ReinsuranceTypePickListDetailId)
                .Index(t => t.Code)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingAdvantageProgramVersionBenefits",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingAdvantageProgramVersionId = c.Int(nullable: false),
                    BenefitId = c.Int(nullable: false),
                    ExtraMortality = c.Double(),
                    SumAssured = c.Double(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Benefits", t => t.BenefitId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingAdvantageProgramVersions", t => t.TreatyPricingAdvantageProgramVersionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingAdvantageProgramVersionId)
                .Index(t => t.BenefitId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingAdvantageProgramVersions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingAdvantageProgramId = c.Int(nullable: false),
                    Version = c.Int(nullable: false),
                    PersonInChargeId = c.Int(nullable: false),
                    EffectiveAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    Retention = c.String(),
                    MlreShare = c.String(),
                    Remarks = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.PersonInChargeId)
                .ForeignKey("dbo.TreatyPricingAdvantagePrograms", t => t.TreatyPricingAdvantageProgramId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingAdvantageProgramId)
                .Index(t => t.Version)
                .Index(t => t.PersonInChargeId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingCampaignProducts",
                c => new
                {
                    TreatyPricingCampaignId = c.Int(nullable: false),
                    TreatyPricingProductId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.TreatyPricingCampaignId, t.TreatyPricingProductId })
                .ForeignKey("dbo.TreatyPricingCampaigns", t => t.TreatyPricingCampaignId)
                .ForeignKey("dbo.TreatyPricingProducts", t => t.TreatyPricingProductId)
                .Index(t => t.TreatyPricingCampaignId)
                .Index(t => t.TreatyPricingProductId);

            CreateTable(
                "dbo.TreatyPricingCampaigns",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingCedantId = c.Int(nullable: false),
                    Code = c.String(nullable: false, maxLength: 30),
                    Name = c.String(maxLength: 255),
                    Status = c.Int(nullable: false),
                    Type = c.String(storeType: "ntext"),
                    Purpose = c.String(),
                    PeriodStartDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    PeriodEndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    Duration = c.String(),
                    TargetTakeUpRate = c.String(maxLength: 255),
                    AverageSumAssured = c.String(),
                    RiPremiumReceivable = c.String(),
                    NoOfPolicy = c.String(),
                    Remarks = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingCedants", t => t.TreatyPricingCedantId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingCedantId)
                .Index(t => t.Code)
                .Index(t => t.Name)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingProducts",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingCedantId = c.Int(nullable: false),
                    Code = c.String(nullable: false, maxLength: 30),
                    Name = c.String(maxLength: 255),
                    EffectiveDate = c.DateTime(),
                    Summary = c.String(storeType: "ntext"),
                    QuotationName = c.String(maxLength: 255),
                    HasPerLifeRetro = c.Boolean(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingCedants", t => t.TreatyPricingCedantId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingCedantId)
                .Index(t => t.Code)
                .Index(t => t.Name)
                .Index(t => t.EffectiveDate)
                .Index(t => t.QuotationName)
                .Index(t => t.HasPerLifeRetro)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingCampaignVersions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingCampaignId = c.Int(nullable: false),
                    Version = c.Int(nullable: false),
                    PersonInChargeId = c.Int(nullable: false),
                    IsPerBenefit = c.Boolean(nullable: false),
                    BenefitRemark = c.String(maxLength: 128),
                    IsPerCedantRetention = c.Boolean(nullable: false),
                    CedantRetention = c.String(maxLength: 128),
                    IsPerMlreShare = c.Boolean(nullable: false),
                    MlreShare = c.String(maxLength: 128),
                    IsPerDistributionChannel = c.Boolean(nullable: false),
                    DistributionChannel = c.String(storeType: "ntext"),
                    IsPerAgeBasis = c.Boolean(nullable: false),
                    AgeBasisPickListDetailId = c.Int(),
                    IsPerMinEntryAge = c.Boolean(nullable: false),
                    MinEntryAge = c.String(),
                    IsPerMaxEntryAge = c.Boolean(nullable: false),
                    MaxEntryAge = c.String(),
                    IsPerMaxExpiryAge = c.Boolean(nullable: false),
                    MaxExpiryAge = c.String(),
                    IsPerMinSumAssured = c.Boolean(nullable: false),
                    MinSumAssured = c.String(),
                    IsPerMaxSumAssured = c.Boolean(nullable: false),
                    MaxSumAssured = c.String(),
                    IsPerReinsuranceRate = c.Boolean(nullable: false),
                    ReinsRateTreatyPricingRateTableId = c.Int(),
                    ReinsRateTreatyPricingRateTableVersionId = c.Int(),
                    ReinsRateNote = c.String(maxLength: 128),
                    IsPerReinsuranceDiscount = c.Boolean(nullable: false),
                    ReinsDiscountTreatyPricingRateTableId = c.Int(),
                    ReinsDiscountTreatyPricingRateTableVersionId = c.Int(),
                    ReinsDiscountNote = c.String(maxLength: 128),
                    IsPerProfitComm = c.Boolean(nullable: false),
                    TreatyPricingProfitCommissionId = c.Int(),
                    TreatyPricingProfitCommissionVersionId = c.Int(),
                    ProfitCommNote = c.String(maxLength: 128),
                    CampaignFundByMlre = c.String(maxLength: 128),
                    ComplimentarySumAssured = c.String(maxLength: 128),
                    IsPerUnderwritingMethod = c.Boolean(nullable: false),
                    UnderwritingMethod = c.String(storeType: "ntext"),
                    IsPerUnderwritingQuestion = c.Boolean(nullable: false),
                    TreatyPricingUwQuestionnaireId = c.Int(),
                    TreatyPricingUwQuestionnaireVersionId = c.Int(),
                    UnderwritingQuestionNote = c.String(maxLength: 128),
                    IsPerMedicalTable = c.Boolean(nullable: false),
                    TreatyPricingMedicalTableId = c.Int(),
                    TreatyPricingMedicalTableVersionId = c.Int(),
                    MedicalTableNote = c.String(maxLength: 128),
                    IsPerFinancialTable = c.Boolean(nullable: false),
                    TreatyPricingFinancialTableId = c.Int(),
                    TreatyPricingFinancialTableVersionId = c.Int(),
                    FinancialTableNote = c.String(maxLength: 128),
                    IsPerAggregationNotes = c.Boolean(nullable: false),
                    AggregationNotes = c.String(maxLength: 128),
                    IsPerAdvantageProgram = c.Boolean(nullable: false),
                    TreatyPricingAdvantageProgramId = c.Int(),
                    TreatyPricingAdvantageProgramVersionId = c.Int(),
                    AdvantageProgramNote = c.String(maxLength: 128),
                    IsPerWaitingPeriod = c.Boolean(nullable: false),
                    WaitingPeriod = c.String(maxLength: 128),
                    IsPerSurvivalPeriod = c.Boolean(nullable: false),
                    SurvivalPeriod = c.String(maxLength: 128),
                    OtherCampaignCriteria = c.String(maxLength: 128),
                    AdditionalRemark = c.String(maxLength: 128),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PickListDetails", t => t.AgeBasisPickListDetailId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.PersonInChargeId)
                .ForeignKey("dbo.TreatyPricingRateTables", t => t.ReinsDiscountTreatyPricingRateTableId)
                .ForeignKey("dbo.TreatyPricingRateTableVersions", t => t.ReinsDiscountTreatyPricingRateTableVersionId)
                .ForeignKey("dbo.TreatyPricingRateTables", t => t.ReinsRateTreatyPricingRateTableId)
                .ForeignKey("dbo.TreatyPricingRateTableVersions", t => t.ReinsRateTreatyPricingRateTableVersionId)
                .ForeignKey("dbo.TreatyPricingAdvantagePrograms", t => t.TreatyPricingAdvantageProgramId)
                .ForeignKey("dbo.TreatyPricingAdvantageProgramVersions", t => t.TreatyPricingAdvantageProgramVersionId)
                .ForeignKey("dbo.TreatyPricingCampaigns", t => t.TreatyPricingCampaignId)
                .ForeignKey("dbo.TreatyPricingFinancialTables", t => t.TreatyPricingFinancialTableId)
                .ForeignKey("dbo.TreatyPricingFinancialTableVersions", t => t.TreatyPricingFinancialTableVersionId)
                .ForeignKey("dbo.TreatyPricingMedicalTables", t => t.TreatyPricingMedicalTableId)
                .ForeignKey("dbo.TreatyPricingMedicalTableVersions", t => t.TreatyPricingMedicalTableVersionId)
                .ForeignKey("dbo.TreatyPricingProfitCommissions", t => t.TreatyPricingProfitCommissionId)
                .ForeignKey("dbo.TreatyPricingProfitCommissionVersions", t => t.TreatyPricingProfitCommissionVersionId)
                .ForeignKey("dbo.TreatyPricingUwQuestionnaires", t => t.TreatyPricingUwQuestionnaireId)
                .ForeignKey("dbo.TreatyPricingUwQuestionnaireVersions", t => t.TreatyPricingUwQuestionnaireVersionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingCampaignId)
                .Index(t => t.Version)
                .Index(t => t.PersonInChargeId)
                .Index(t => t.IsPerBenefit)
                .Index(t => t.AgeBasisPickListDetailId)
                .Index(t => t.ReinsRateTreatyPricingRateTableId)
                .Index(t => t.ReinsRateTreatyPricingRateTableVersionId)
                .Index(t => t.ReinsDiscountTreatyPricingRateTableId)
                .Index(t => t.ReinsDiscountTreatyPricingRateTableVersionId)
                .Index(t => t.TreatyPricingProfitCommissionId)
                .Index(t => t.TreatyPricingProfitCommissionVersionId)
                .Index(t => t.TreatyPricingUwQuestionnaireId)
                .Index(t => t.TreatyPricingUwQuestionnaireVersionId)
                .Index(t => t.TreatyPricingMedicalTableId)
                .Index(t => t.TreatyPricingMedicalTableVersionId)
                .Index(t => t.TreatyPricingFinancialTableId)
                .Index(t => t.TreatyPricingFinancialTableVersionId)
                .Index(t => t.TreatyPricingAdvantageProgramId)
                .Index(t => t.TreatyPricingAdvantageProgramVersionId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingRateTables",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingRateTableGroupId = c.Int(nullable: false),
                    Code = c.String(nullable: false, maxLength: 255),
                    Name = c.String(maxLength: 255),
                    BenefitId = c.Int(nullable: false),
                    Description = c.String(maxLength: 255),
                    Status = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Benefits", t => t.BenefitId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingRateTableGroups", t => t.TreatyPricingRateTableGroupId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingRateTableGroupId)
                .Index(t => t.Code)
                .Index(t => t.Name)
                .Index(t => t.BenefitId)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingRateTableGroups",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingCedantId = c.Int(nullable: false),
                    Code = c.String(nullable: false, maxLength: 255),
                    Name = c.String(maxLength: 255),
                    Description = c.String(maxLength: 255),
                    FileName = c.String(maxLength: 255),
                    HashFileName = c.String(maxLength: 255),
                    NoOfRateTable = c.Int(nullable: false),
                    Status = c.Int(nullable: false),
                    Errors = c.String(storeType: "ntext"),
                    UploadedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UploadedById = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingCedants", t => t.TreatyPricingCedantId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .ForeignKey("dbo.Users", t => t.UploadedById)
                .Index(t => t.TreatyPricingCedantId)
                .Index(t => t.Code)
                .Index(t => t.Name)
                .Index(t => t.Description)
                .Index(t => t.FileName)
                .Index(t => t.HashFileName)
                .Index(t => t.NoOfRateTable)
                .Index(t => t.Status)
                .Index(t => t.UploadedById)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingRateTableVersions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingRateTableId = c.Int(nullable: false),
                    Version = c.Int(nullable: false),
                    PersonInChargeId = c.Int(nullable: false),
                    BenefitName = c.String(maxLength: 255),
                    EffectiveDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    AgeBasisPickListDetailId = c.Int(),
                    RiDiscount = c.String(maxLength: 255),
                    CoinsuranceRiDiscount = c.String(maxLength: 255),
                    RateGuaranteePickListDetailId = c.Int(),
                    RateGuaranteeForNewBusiness = c.String(maxLength: 255),
                    RateGuaranteeForRenewalBusiness = c.String(maxLength: 255),
                    AdvantageProgram = c.String(maxLength: 255),
                    ProfitCommission = c.String(maxLength: 255),
                    AdditionalRemark = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PickListDetails", t => t.AgeBasisPickListDetailId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.PersonInChargeId)
                .ForeignKey("dbo.PickListDetails", t => t.RateGuaranteePickListDetailId)
                .ForeignKey("dbo.TreatyPricingRateTables", t => t.TreatyPricingRateTableId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingRateTableId)
                .Index(t => t.Version)
                .Index(t => t.PersonInChargeId)
                .Index(t => t.EffectiveDate)
                .Index(t => t.AgeBasisPickListDetailId)
                .Index(t => t.RateGuaranteePickListDetailId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingFinancialTables",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingCedantId = c.Int(nullable: false),
                    FinancialTableId = c.String(maxLength: 30),
                    Status = c.Int(nullable: false),
                    Name = c.String(maxLength: 255),
                    Description = c.String(maxLength: 255),
                    BenefitCode = c.String(storeType: "ntext"),
                    DistributionChannel = c.String(storeType: "ntext"),
                    CurrencyCode = c.String(maxLength: 3),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingCedants", t => t.TreatyPricingCedantId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingCedantId)
                .Index(t => t.FinancialTableId)
                .Index(t => t.Status)
                .Index(t => t.Name)
                .Index(t => t.Description)
                .Index(t => t.CurrencyCode)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingFinancialTableVersions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingFinancialTableId = c.Int(nullable: false),
                    Version = c.Int(nullable: false),
                    PersonInChargeId = c.Int(nullable: false),
                    EffectiveAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    Remarks = c.String(maxLength: 255),
                    AggregationNote = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.PersonInChargeId)
                .ForeignKey("dbo.TreatyPricingFinancialTables", t => t.TreatyPricingFinancialTableId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingFinancialTableId)
                .Index(t => t.Version)
                .Index(t => t.PersonInChargeId)
                .Index(t => t.EffectiveAt)
                .Index(t => t.Remarks)
                .Index(t => t.AggregationNote)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingMedicalTables",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingCedantId = c.Int(nullable: false),
                    MedicalTableId = c.String(maxLength: 30),
                    Status = c.Int(nullable: false),
                    Name = c.String(maxLength: 255),
                    Description = c.String(maxLength: 255),
                    BenefitCode = c.String(storeType: "ntext"),
                    DistributionChannel = c.String(storeType: "ntext"),
                    CurrencyCode = c.String(maxLength: 3),
                    AgeDefinitionPickListDetailId = c.Int(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PickListDetails", t => t.AgeDefinitionPickListDetailId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingCedants", t => t.TreatyPricingCedantId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingCedantId)
                .Index(t => t.MedicalTableId)
                .Index(t => t.Status)
                .Index(t => t.Name)
                .Index(t => t.Description)
                .Index(t => t.CurrencyCode)
                .Index(t => t.AgeDefinitionPickListDetailId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingMedicalTableVersions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingMedicalTableId = c.Int(nullable: false),
                    Version = c.Int(nullable: false),
                    PersonInChargeId = c.Int(nullable: false),
                    EffectiveAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    Remarks = c.String(maxLength: 255),
                    AggregationNote = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.PersonInChargeId)
                .ForeignKey("dbo.TreatyPricingMedicalTables", t => t.TreatyPricingMedicalTableId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingMedicalTableId)
                .Index(t => t.Version)
                .Index(t => t.PersonInChargeId)
                .Index(t => t.EffectiveAt)
                .Index(t => t.Remarks)
                .Index(t => t.AggregationNote)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingProfitCommissions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingCedantId = c.Int(nullable: false),
                    Code = c.String(nullable: false, maxLength: 255),
                    Name = c.String(maxLength: 255),
                    BenefitCode = c.String(storeType: "ntext"),
                    Description = c.String(maxLength: 255),
                    Status = c.Int(nullable: false),
                    EffectiveDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    StartDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    EndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    Entitlement = c.String(storeType: "ntext"),
                    Remark = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingCedants", t => t.TreatyPricingCedantId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingCedantId)
                .Index(t => t.Code)
                .Index(t => t.Name)
                .Index(t => t.Description)
                .Index(t => t.Status)
                .Index(t => t.EffectiveDate)
                .Index(t => t.StartDate)
                .Index(t => t.EndDate)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingProfitCommissionVersions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingProfitCommissionId = c.Int(nullable: false),
                    Version = c.Int(nullable: false),
                    PersonInChargeId = c.Int(nullable: false),
                    ProfitSharing = c.Int(),
                    ProfitDescription = c.String(maxLength: 255),
                    NetProfitPercentage = c.Double(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.PersonInChargeId, cascadeDelete: true)
                .ForeignKey("dbo.TreatyPricingProfitCommissions", t => t.TreatyPricingProfitCommissionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingProfitCommissionId)
                .Index(t => t.Version)
                .Index(t => t.PersonInChargeId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingUwQuestionnaires",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingCedantId = c.Int(nullable: false),
                    Code = c.String(nullable: false, maxLength: 255),
                    Name = c.String(maxLength: 255),
                    Description = c.String(maxLength: 255),
                    Status = c.Int(nullable: false),
                    BenefitCode = c.String(storeType: "ntext"),
                    DistributionChannel = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingCedants", t => t.TreatyPricingCedantId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingCedantId)
                .Index(t => t.Code)
                .Index(t => t.Name)
                .Index(t => t.Description)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingUwQuestionnaireVersions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingUwQuestionnaireId = c.Int(nullable: false),
                    Version = c.Int(nullable: false),
                    PersonInChargeId = c.Int(nullable: false),
                    EffectiveAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    Type = c.Int(nullable: false),
                    Remarks = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.PersonInChargeId)
                .ForeignKey("dbo.TreatyPricingUwQuestionnaires", t => t.TreatyPricingUwQuestionnaireId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingUwQuestionnaireId)
                .Index(t => t.Version)
                .Index(t => t.PersonInChargeId)
                .Index(t => t.Type)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingClaimApprovalLimits",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingCedantId = c.Int(nullable: false),
                    Code = c.String(nullable: false, maxLength: 255),
                    Name = c.String(maxLength: 255),
                    Description = c.String(),
                    Status = c.Int(nullable: false),
                    BenefitCode = c.String(storeType: "ntext"),
                    Remarks = c.String(),
                    Errors = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingCedants", t => t.TreatyPricingCedantId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingCedantId)
                .Index(t => t.Code)
                .Index(t => t.Name)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingClaimApprovalLimitVersions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingClaimApprovalLimitId = c.Int(nullable: false),
                    Version = c.Int(nullable: false),
                    PersonInChargeId = c.Int(nullable: false),
                    EffectiveAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    Amount = c.String(maxLength: 50),
                    AdditionalRemarks = c.String(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.PersonInChargeId)
                .ForeignKey("dbo.TreatyPricingClaimApprovalLimits", t => t.TreatyPricingClaimApprovalLimitId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingClaimApprovalLimitId)
                .Index(t => t.Version)
                .Index(t => t.PersonInChargeId)
                .Index(t => t.Amount)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingCustomOtherProducts",
                c => new
                {
                    TreatyPricingCustomOtherId = c.Int(nullable: false),
                    TreatyPricingProductId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.TreatyPricingCustomOtherId, t.TreatyPricingProductId })
                .ForeignKey("dbo.TreatyPricingCustomerOthers", t => t.TreatyPricingCustomOtherId)
                .ForeignKey("dbo.TreatyPricingProducts", t => t.TreatyPricingProductId)
                .Index(t => t.TreatyPricingCustomOtherId)
                .Index(t => t.TreatyPricingProductId);

            CreateTable(
                "dbo.TreatyPricingCustomerOthers",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingCedantId = c.Int(nullable: false),
                    Code = c.String(nullable: false, maxLength: 255),
                    Status = c.Int(nullable: false),
                    Name = c.String(maxLength: 255),
                    Description = c.String(),
                    Errors = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingCedants", t => t.TreatyPricingCedantId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingCedantId)
                .Index(t => t.Code)
                .Index(t => t.Status)
                .Index(t => t.Name)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingCustomOtherVersions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingCustomOtherId = c.Int(nullable: false),
                    Version = c.Int(nullable: false),
                    PersonInChargeId = c.Int(nullable: false),
                    EffectiveAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    FileName = c.String(maxLength: 255),
                    HashFileName = c.String(maxLength: 255),
                    AdditionalRemarks = c.String(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.PersonInChargeId)
                .ForeignKey("dbo.TreatyPricingCustomerOthers", t => t.TreatyPricingCustomOtherId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingCustomOtherId)
                .Index(t => t.Version)
                .Index(t => t.PersonInChargeId)
                .Index(t => t.FileName)
                .Index(t => t.HashFileName)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingDefinitionAndExclusion",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingCedantId = c.Int(nullable: false),
                    Code = c.String(nullable: false, maxLength: 255),
                    Status = c.Int(nullable: false),
                    Name = c.String(maxLength: 255),
                    Description = c.String(),
                    BenefitCode = c.String(storeType: "ntext"),
                    AdditionalRemarks = c.String(),
                    Errors = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingCedants", t => t.TreatyPricingCedantId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingCedantId)
                .Index(t => t.Code)
                .Index(t => t.Status)
                .Index(t => t.Name)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingDefinitionAndExclusionVersions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingDefinitionAndExclusionId = c.Int(nullable: false),
                    Version = c.Int(nullable: false),
                    PersonInChargeId = c.Int(nullable: false),
                    EffectiveAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    Definitions = c.String(),
                    Exclusions = c.String(),
                    DeclinedRisk = c.String(),
                    ReferredRisk = c.String(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.PersonInChargeId)
                .ForeignKey("dbo.TreatyPricingDefinitionAndExclusion", t => t.TreatyPricingDefinitionAndExclusionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingDefinitionAndExclusionId)
                .Index(t => t.Version)
                .Index(t => t.PersonInChargeId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingFinancialTableUploadLegends",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingFinancialTableVersionDetailId = c.Int(nullable: false),
                    Code = c.String(maxLength: 30),
                    Description = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingFinancialTableVersionDetails", t => t.TreatyPricingFinancialTableVersionDetailId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingFinancialTableVersionDetailId)
                .Index(t => t.Code)
                .Index(t => t.Description)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingFinancialTableVersionDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingFinancialTableVersionId = c.Int(nullable: false),
                    DistributionTierPickListDetailId = c.Int(nullable: false),
                    Description = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickListDetails", t => t.DistributionTierPickListDetailId)
                .ForeignKey("dbo.TreatyPricingFinancialTableVersions", t => t.TreatyPricingFinancialTableVersionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingFinancialTableVersionId)
                .Index(t => t.DistributionTierPickListDetailId)
                .Index(t => t.Description)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingFinancialTableUploads",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingFinancialTableVersionDetailId = c.Int(nullable: false),
                    MinimumSumAssured = c.Int(nullable: false),
                    MaximumSumAssured = c.Int(nullable: false),
                    Code = c.String(maxLength: 30),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingFinancialTableVersionDetails", t => t.TreatyPricingFinancialTableVersionDetailId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingFinancialTableVersionDetailId)
                .Index(t => t.MinimumSumAssured)
                .Index(t => t.MaximumSumAssured)
                .Index(t => t.Code)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingFinancialTableVersionFiles",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingFinancialTableVersionId = c.Int(nullable: false),
                    DistributionTierPickListDetailId = c.Int(nullable: false),
                    Description = c.String(maxLength: 255),
                    FileName = c.String(maxLength: 255),
                    HashFileName = c.String(maxLength: 255),
                    Status = c.Int(nullable: false),
                    Errors = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickListDetails", t => t.DistributionTierPickListDetailId)
                .ForeignKey("dbo.TreatyPricingFinancialTableVersions", t => t.TreatyPricingFinancialTableVersionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingFinancialTableVersionId)
                .Index(t => t.DistributionTierPickListDetailId)
                .Index(t => t.Description)
                .Index(t => t.FileName)
                .Index(t => t.HashFileName)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingGroupMasterLetterGroupReferrals",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingGroupMasterLetterId = c.Int(nullable: false),
                    TreatyPricingGroupReferralId = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingGroupMasterLetters", t => t.TreatyPricingGroupMasterLetterId)
                .ForeignKey("dbo.TreatyPricingGroupReferrals", t => t.TreatyPricingGroupReferralId)
                .Index(t => t.TreatyPricingGroupMasterLetterId)
                .Index(t => t.TreatyPricingGroupReferralId)
                .Index(t => t.CreatedById);

            CreateTable(
                "dbo.TreatyPricingGroupMasterLetters",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CedantId = c.Int(nullable: false),
                    Code = c.String(nullable: false, maxLength: 30),
                    NoOfRiGroupSlip = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cedants", t => t.CedantId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.CedantId)
                .Index(t => t.Code)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingGroupReferrals",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CedantId = c.Int(nullable: false),
                    Code = c.String(nullable: false, maxLength: 30),
                    Description = c.String(nullable: false, maxLength: 255),
                    RiArrangementPickListDetailId = c.Int(),
                    InsuredGroupNameId = c.Int(nullable: false),
                    Status = c.Int(nullable: false),
                    WorkflowStatus = c.Int(),
                    PrimaryTreatyPricingProductId = c.Int(nullable: false),
                    PrimaryTreatyPricingProductVersionId = c.Int(nullable: false),
                    SecondaryTreatyPricingProductId = c.Int(),
                    SecondaryTreatyPricingProductVersionId = c.Int(),
                    FirstRequestDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    FirstReferralDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    CoverageType = c.Int(),
                    CoverageStartDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    CoverageEndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    IndustryNamePickListDetailId = c.Int(),
                    ReferredTypePickListDetailId = c.Int(),
                    QuotationTAT = c.Int(),
                    InternalTAT = c.Int(),
                    QuotationValidityDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    QuotationValidityDay = c.String(),
                    FirstQuotationSentWeek = c.Int(),
                    FirstQuotationSentMonth = c.Int(),
                    FirstQuotationSentQuarter = c.String(maxLength: 10),
                    FirstQuotationSentYear = c.Int(),
                    WonVersion = c.String(maxLength: 128),
                    HasRiGroupSlip = c.Boolean(nullable: false),
                    RiGroupSlipCode = c.String(maxLength: 30),
                    RiGroupSlipStatus = c.Int(),
                    RiGroupSlipPersonInChargeId = c.Int(),
                    RiGroupSlipConfirmationDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    RiGroupSlipVersionId = c.Int(),
                    RiGroupSlipTemplateId = c.Int(),
                    RiGroupSlipSharePointLink = c.String(storeType: "ntext"),
                    RiGroupSlipSharePointFolderPath = c.String(storeType: "ntext"),
                    QuotationPath = c.String(maxLength: 128),
                    ReplyVersionId = c.Int(),
                    ReplyTemplateId = c.Int(),
                    ReplySharePointLink = c.String(storeType: "ntext"),
                    ReplySharePointFolderPath = c.String(storeType: "ntext"),
                    TreatyPricingGroupMasterLetterId = c.Int(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cedants", t => t.CedantId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickListDetails", t => t.IndustryNamePickListDetailId)
                .ForeignKey("dbo.InsuredGroupNames", t => t.InsuredGroupNameId)
                .ForeignKey("dbo.TreatyPricingProducts", t => t.PrimaryTreatyPricingProductId)
                .ForeignKey("dbo.TreatyPricingProductVersions", t => t.PrimaryTreatyPricingProductVersionId)
                .ForeignKey("dbo.PickListDetails", t => t.ReferredTypePickListDetailId)
                .ForeignKey("dbo.Templates", t => t.ReplyTemplateId)
                .ForeignKey("dbo.TreatyPricingGroupReferralVersions", t => t.ReplyVersionId)
                .ForeignKey("dbo.PickListDetails", t => t.RiArrangementPickListDetailId)
                .ForeignKey("dbo.Users", t => t.RiGroupSlipPersonInChargeId)
                .ForeignKey("dbo.Templates", t => t.RiGroupSlipTemplateId)
                .ForeignKey("dbo.TreatyPricingGroupReferralVersions", t => t.RiGroupSlipVersionId)
                .ForeignKey("dbo.TreatyPricingProducts", t => t.SecondaryTreatyPricingProductId)
                .ForeignKey("dbo.TreatyPricingProductVersions", t => t.SecondaryTreatyPricingProductVersionId)
                .ForeignKey("dbo.TreatyPricingGroupMasterLetters", t => t.TreatyPricingGroupMasterLetterId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.CedantId)
                .Index(t => t.Code)
                .Index(t => t.RiArrangementPickListDetailId)
                .Index(t => t.InsuredGroupNameId)
                .Index(t => t.Status)
                .Index(t => t.PrimaryTreatyPricingProductId)
                .Index(t => t.PrimaryTreatyPricingProductVersionId)
                .Index(t => t.SecondaryTreatyPricingProductId)
                .Index(t => t.SecondaryTreatyPricingProductVersionId)
                .Index(t => t.IndustryNamePickListDetailId)
                .Index(t => t.ReferredTypePickListDetailId)
                .Index(t => t.RiGroupSlipCode)
                .Index(t => t.RiGroupSlipPersonInChargeId)
                .Index(t => t.RiGroupSlipVersionId)
                .Index(t => t.RiGroupSlipTemplateId)
                .Index(t => t.ReplyVersionId)
                .Index(t => t.ReplyTemplateId)
                .Index(t => t.TreatyPricingGroupMasterLetterId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingProductVersions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingProductId = c.Int(nullable: false),
                    Version = c.Int(nullable: false),
                    PersonInChargeId = c.Int(nullable: false),
                    ProductTypePickListDetailId = c.Int(),
                    BusinessOriginPickListDetailId = c.Int(),
                    BusinessTypePickListDetailId = c.Int(),
                    ReinsuranceArrangementPickListDetailId = c.Int(),
                    ExpectedAverageSumAssured = c.String(maxLength: 128),
                    ExpectedRiPremium = c.String(maxLength: 128),
                    TreatyPricingMedicalTableId = c.Int(),
                    TreatyPricingMedicalTableVersionId = c.Int(),
                    TreatyPricingFinancialTableId = c.Int(),
                    TreatyPricingFinancialTableVersionId = c.Int(),
                    TreatyPricingUwQuestionnaireId = c.Int(),
                    TreatyPricingUwQuestionnaireVersionId = c.Int(),
                    TreatyPricingAdvantageProgramId = c.Int(),
                    TreatyPricingAdvantageProgramVersionId = c.Int(),
                    ExpectedPolicyNo = c.String(maxLength: 128),
                    JumboLimit = c.String(maxLength: 128),
                    UnderwritingAdditionalRemark = c.String(storeType: "ntext"),
                    WaitingPeriod = c.String(maxLength: 128),
                    SurvivalPeriod = c.String(maxLength: 128),
                    TreatyPricingProfitCommissionId = c.Int(),
                    TreatyPricingProfitCommissionVersionId = c.Int(),
                    ReinsurancePremiumPaymentPickListDetailId = c.Int(),
                    UnearnedPremiumRefundPickListDetailId = c.Int(),
                    TerminationClause = c.String(maxLength: 128),
                    RecaptureClause = c.String(maxLength: 128),
                    ResidenceCountry = c.String(maxLength: 128),
                    QuarterlyRiskPremium = c.String(maxLength: 128),
                    GroupFreeCoverLimitNonCi = c.String(storeType: "ntext"),
                    GroupFreeCoverLimitAgeNonCi = c.String(maxLength: 128),
                    GroupFreeCoverLimitCi = c.String(storeType: "ntext"),
                    GroupFreeCoverLimitAgeCi = c.String(maxLength: 128),
                    GroupProfitCommission = c.String(storeType: "ntext"),
                    IsDirectRetro = c.Boolean(nullable: false),
                    DirectRetroProfitCommission = c.String(maxLength: 128),
                    DirectRetroTerminationClause = c.String(maxLength: 128),
                    DirectRetroRecaptureClause = c.String(maxLength: 128),
                    DirectRetroQuarterlyRiskPremium = c.String(maxLength: 128),
                    IsInwardRetro = c.Boolean(nullable: false),
                    InwardRetroProfitCommission = c.String(maxLength: 128),
                    InwardRetroTerminationClause = c.String(maxLength: 128),
                    InwardRetroRecaptureClause = c.String(maxLength: 128),
                    InwardRetroQuarterlyRiskPremium = c.String(maxLength: 128),
                    IsRetakafulService = c.Boolean(nullable: false),
                    InvestmentProfitSharing = c.String(maxLength: 128),
                    RetakafulModel = c.String(maxLength: 128),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PickListDetails", t => t.BusinessOriginPickListDetailId)
                .ForeignKey("dbo.PickListDetails", t => t.BusinessTypePickListDetailId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.PersonInChargeId)
                .ForeignKey("dbo.PickListDetails", t => t.ProductTypePickListDetailId)
                .ForeignKey("dbo.PickListDetails", t => t.ReinsuranceArrangementPickListDetailId)
                .ForeignKey("dbo.PickListDetails", t => t.ReinsurancePremiumPaymentPickListDetailId)
                .ForeignKey("dbo.TreatyPricingAdvantagePrograms", t => t.TreatyPricingAdvantageProgramId)
                .ForeignKey("dbo.TreatyPricingAdvantageProgramVersions", t => t.TreatyPricingAdvantageProgramVersionId)
                .ForeignKey("dbo.TreatyPricingFinancialTables", t => t.TreatyPricingFinancialTableId)
                .ForeignKey("dbo.TreatyPricingFinancialTableVersions", t => t.TreatyPricingFinancialTableVersionId)
                .ForeignKey("dbo.TreatyPricingMedicalTables", t => t.TreatyPricingMedicalTableId)
                .ForeignKey("dbo.TreatyPricingMedicalTableVersions", t => t.TreatyPricingMedicalTableVersionId)
                .ForeignKey("dbo.TreatyPricingProducts", t => t.TreatyPricingProductId)
                .ForeignKey("dbo.TreatyPricingProfitCommissions", t => t.TreatyPricingProfitCommissionId)
                .ForeignKey("dbo.TreatyPricingProfitCommissionVersions", t => t.TreatyPricingProfitCommissionVersionId)
                .ForeignKey("dbo.TreatyPricingUwQuestionnaires", t => t.TreatyPricingUwQuestionnaireId)
                .ForeignKey("dbo.TreatyPricingUwQuestionnaireVersions", t => t.TreatyPricingUwQuestionnaireVersionId)
                .ForeignKey("dbo.PickListDetails", t => t.UnearnedPremiumRefundPickListDetailId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingProductId)
                .Index(t => t.Version)
                .Index(t => t.PersonInChargeId)
                .Index(t => t.ProductTypePickListDetailId)
                .Index(t => t.BusinessOriginPickListDetailId)
                .Index(t => t.BusinessTypePickListDetailId)
                .Index(t => t.ReinsuranceArrangementPickListDetailId)
                .Index(t => t.ExpectedAverageSumAssured)
                .Index(t => t.ExpectedRiPremium)
                .Index(t => t.TreatyPricingMedicalTableId)
                .Index(t => t.TreatyPricingMedicalTableVersionId)
                .Index(t => t.TreatyPricingFinancialTableId)
                .Index(t => t.TreatyPricingFinancialTableVersionId)
                .Index(t => t.TreatyPricingUwQuestionnaireId)
                .Index(t => t.TreatyPricingUwQuestionnaireVersionId)
                .Index(t => t.TreatyPricingAdvantageProgramId)
                .Index(t => t.TreatyPricingAdvantageProgramVersionId)
                .Index(t => t.ExpectedPolicyNo)
                .Index(t => t.JumboLimit)
                .Index(t => t.WaitingPeriod)
                .Index(t => t.SurvivalPeriod)
                .Index(t => t.TreatyPricingProfitCommissionId)
                .Index(t => t.TreatyPricingProfitCommissionVersionId)
                .Index(t => t.ReinsurancePremiumPaymentPickListDetailId)
                .Index(t => t.UnearnedPremiumRefundPickListDetailId)
                .Index(t => t.TerminationClause)
                .Index(t => t.RecaptureClause)
                .Index(t => t.ResidenceCountry)
                .Index(t => t.QuarterlyRiskPremium)
                .Index(t => t.GroupFreeCoverLimitAgeNonCi)
                .Index(t => t.GroupFreeCoverLimitAgeCi)
                .Index(t => t.IsDirectRetro)
                .Index(t => t.DirectRetroProfitCommission)
                .Index(t => t.DirectRetroTerminationClause)
                .Index(t => t.DirectRetroRecaptureClause)
                .Index(t => t.DirectRetroQuarterlyRiskPremium)
                .Index(t => t.IsInwardRetro)
                .Index(t => t.InwardRetroProfitCommission)
                .Index(t => t.InwardRetroTerminationClause)
                .Index(t => t.InwardRetroRecaptureClause)
                .Index(t => t.InwardRetroQuarterlyRiskPremium)
                .Index(t => t.IsRetakafulService)
                .Index(t => t.InvestmentProfitSharing)
                .Index(t => t.RetakafulModel)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingGroupReferralVersions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingGroupReferralId = c.Int(nullable: false),
                    Version = c.Int(nullable: false),
                    GroupReferralPersonInChargeId = c.Int(),
                    CedantPersonInCharge = c.String(maxLength: 128),
                    RequestTypePickListDetailId = c.Int(),
                    PremiumTypePickListDetailId = c.Int(),
                    GrossRiskPremium = c.String(),
                    ReinsurancePremium = c.String(),
                    GrossRiskPremiumGTL = c.String(),
                    ReinsurancePremiumGTL = c.String(),
                    GrossRiskPremiumGHS = c.String(),
                    ReinsurancePremiumGHS = c.String(),
                    AverageSumAssured = c.String(),
                    GroupSize = c.String(),
                    IsCompulsoryOrVoluntary = c.Boolean(nullable: false),
                    UnderwritingMethodPickListDetailId = c.Int(),
                    Remarks = c.String(maxLength: 255),
                    RequestReceivedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    EnquiryToClientDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    ClientReplyDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    QuotationSentDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    Score = c.Int(),
                    DeclinedRisk = c.String(maxLength: 128),
                    ReferredRisk = c.String(maxLength: 128),
                    HasPerLifeRetro = c.Boolean(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickListDetails", t => t.GroupReferralPersonInChargeId)
                .ForeignKey("dbo.PickListDetails", t => t.PremiumTypePickListDetailId)
                .ForeignKey("dbo.PickListDetails", t => t.RequestTypePickListDetailId)
                .ForeignKey("dbo.TreatyPricingGroupReferrals", t => t.TreatyPricingGroupReferralId)
                .ForeignKey("dbo.PickListDetails", t => t.UnderwritingMethodPickListDetailId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingGroupReferralId)
                .Index(t => t.Version)
                .Index(t => t.GroupReferralPersonInChargeId)
                .Index(t => t.RequestTypePickListDetailId)
                .Index(t => t.PremiumTypePickListDetailId)
                .Index(t => t.IsCompulsoryOrVoluntary)
                .Index(t => t.UnderwritingMethodPickListDetailId)
                .Index(t => t.HasPerLifeRetro)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingGroupReferralChecklistDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingGroupReferralVersionId = c.Int(nullable: false),
                    InternalItem = c.Int(nullable: false),
                    Underwriting = c.Boolean(nullable: false),
                    Health = c.Boolean(nullable: false),
                    Claim = c.Boolean(nullable: false),
                    BD = c.Boolean(nullable: false),
                    CnR = c.Boolean(nullable: false),
                    UltimateApprover = c.Int(),
                    GroupTeamApprover = c.Boolean(nullable: false),
                    ReviewerApprover = c.Boolean(nullable: false),
                    HODApprover = c.Boolean(nullable: false),
                    CEOApprover = c.Boolean(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingGroupReferralVersions", t => t.TreatyPricingGroupReferralVersionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingGroupReferralVersionId)
                .Index(t => t.InternalItem)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingGroupReferralChecklists",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingGroupReferralVersionId = c.Int(nullable: false),
                    InternalTeam = c.Int(nullable: false),
                    InternalTeamPersonInCharge = c.String(maxLength: 255),
                    Status = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingGroupReferralVersions", t => t.TreatyPricingGroupReferralVersionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingGroupReferralVersionId)
                .Index(t => t.InternalTeam)
                .Index(t => t.InternalTeamPersonInCharge)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingGroupReferralFiles",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingGroupReferralId = c.Int(),
                    TableTypePickListDetailId = c.Int(),
                    FileName = c.String(maxLength: 255),
                    HashFileName = c.String(maxLength: 255),
                    UploadedType = c.Int(nullable: false),
                    Status = c.Int(nullable: false),
                    Errors = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickListDetails", t => t.TableTypePickListDetailId)
                .ForeignKey("dbo.TreatyPricingGroupReferrals", t => t.TreatyPricingGroupReferralId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingGroupReferralId)
                .Index(t => t.TableTypePickListDetailId)
                .Index(t => t.FileName)
                .Index(t => t.HashFileName)
                .Index(t => t.UploadedType)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingGroupReferralGhsTables",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingGroupReferralId = c.Int(nullable: false),
                    TreatyPricingGroupReferralFileId = c.Int(),
                    CoverageStartDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    EventDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    ClaimListDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    ClaimantsName = c.String(maxLength: 255),
                    CauseOfClaim = c.String(maxLength: 128),
                    RbCovered = c.String(),
                    AolCovered = c.String(),
                    HospitalCovered = c.String(maxLength: 255),
                    GrossClaimIncurred = c.String(),
                    GrossClaimPaid = c.String(),
                    GrossClaimPaidIbnr = c.String(),
                    RiClaimPaid = c.String(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingGroupReferrals", t => t.TreatyPricingGroupReferralId)
                .ForeignKey("dbo.TreatyPricingGroupReferralFiles", t => t.TreatyPricingGroupReferralFileId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingGroupReferralId)
                .Index(t => t.TreatyPricingGroupReferralFileId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingGroupReferralGtlTables",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingGroupReferralId = c.Int(nullable: false),
                    TreatyPricingGroupReferralFileId = c.Int(),
                    Type = c.Int(nullable: false),
                    GtlBenefitCategoryId = c.Int(),
                    DesignationId = c.Int(),
                    CoverageStartDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    CoverageEndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    EventDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    ClaimListDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    ClaimantsName = c.String(maxLength: 255),
                    CauseOfClaim = c.String(maxLength: 128),
                    ClaimType = c.String(maxLength: 128),
                    AgeBandRange = c.String(maxLength: 128),
                    BasisOfSA = c.String(maxLength: 128),
                    GrossClaim = c.String(),
                    RiClaim = c.String(),
                    RiskRate = c.String(),
                    GrossRate = c.String(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Designations", t => t.DesignationId)
                .ForeignKey("dbo.GtlBenefitCategories", t => t.GtlBenefitCategoryId)
                .ForeignKey("dbo.TreatyPricingGroupReferrals", t => t.TreatyPricingGroupReferralId)
                .ForeignKey("dbo.TreatyPricingGroupReferralFiles", t => t.TreatyPricingGroupReferralFileId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingGroupReferralId)
                .Index(t => t.TreatyPricingGroupReferralFileId)
                .Index(t => t.Type)
                .Index(t => t.GtlBenefitCategoryId)
                .Index(t => t.DesignationId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingGroupReferralHipsTables",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingGroupReferralId = c.Int(nullable: false),
                    TreatyPricingGroupReferralFileId = c.Int(),
                    HipsCategoryId = c.Int(),
                    HipsSubCategoryId = c.Int(),
                    CoverageStartDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    PlanA = c.String(),
                    PlanB = c.String(),
                    PlanC = c.String(),
                    PlanD = c.String(),
                    PlanE = c.String(),
                    PlanF = c.String(),
                    PlanG = c.String(),
                    PlanH = c.String(),
                    PlanI = c.String(),
                    PlanJ = c.String(),
                    PlanK = c.String(),
                    PlanL = c.String(),
                    PlanM = c.String(),
                    PlanN = c.String(),
                    PlanO = c.String(),
                    PlanP = c.String(),
                    PlanQ = c.String(),
                    PlanR = c.String(),
                    PlanS = c.String(),
                    PlanT = c.String(),
                    PlanU = c.String(),
                    PlanV = c.String(),
                    PlanW = c.String(),
                    PlanX = c.String(),
                    PlanY = c.String(),
                    PlanZ = c.String(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.HipsCategories", t => t.HipsCategoryId)
                .ForeignKey("dbo.HipsCategoryDetails", t => t.HipsSubCategoryId)
                .ForeignKey("dbo.TreatyPricingGroupReferrals", t => t.TreatyPricingGroupReferralId)
                .ForeignKey("dbo.TreatyPricingGroupReferralFiles", t => t.TreatyPricingGroupReferralFileId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingGroupReferralId)
                .Index(t => t.TreatyPricingGroupReferralFileId)
                .Index(t => t.HipsCategoryId)
                .Index(t => t.HipsSubCategoryId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingGroupReferralVersionBenefits",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingGroupReferralVersionId = c.Int(nullable: false),
                    BenefitId = c.Int(nullable: false),
                    ReinsuranceArrangementPickListDetailId = c.Int(),
                    AgeBasisPickListDetailId = c.Int(),
                    MinimumEntryAge = c.String(),
                    MaximumEntryAge = c.String(),
                    MaximumExpiryAge = c.String(),
                    TreatyPricingUwLimitId = c.Int(),
                    TreatyPricingUwLimitVersionId = c.Int(),
                    GroupFreeCoverLimitNonCI = c.String(maxLength: 128),
                    RequestedFreeCoverLimitNonCI = c.String(maxLength: 128),
                    GroupFreeCoverLimitCI = c.String(maxLength: 128),
                    RequestedFreeCoverLimitCI = c.String(maxLength: 128),
                    GroupFreeCoverLimitAgeNonCI = c.String(maxLength: 128),
                    GroupFreeCoverLimitAgeCI = c.String(maxLength: 128),
                    OtherSpecialReinsuranceArrangement = c.Int(),
                    OtherSpecialTerms = c.String(maxLength: 255),
                    ProfitMargin = c.String(),
                    ExpenseMargin = c.String(),
                    CommissionMargin = c.String(),
                    ProfitCommissionLoading = c.String(),
                    AdditionalLoading = c.String(),
                    CoinsuranceRiDiscount = c.String(),
                    CoinsuranceCedantRetention = c.String(),
                    CoinsuranceReinsurerShare = c.String(),
                    HasProfitCommission = c.Boolean(nullable: false),
                    HasGroupProfitCommission = c.Boolean(nullable: false),
                    IsOverwriteGroupProfitCommission = c.Boolean(nullable: false),
                    OverwriteGroupProfitCommissionRemarks = c.String(),
                    GroupProfitCommissionId = c.Int(),
                    AdditionalLoadingYRTLayer = c.String(),
                    RiDiscount = c.String(),
                    CedantRetention = c.String(),
                    ReinsuranceShare = c.String(maxLength: 128),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PickListDetails", t => t.AgeBasisPickListDetailId)
                .ForeignKey("dbo.Benefits", t => t.BenefitId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingProfitCommissions", t => t.GroupProfitCommissionId)
                .ForeignKey("dbo.PickListDetails", t => t.ReinsuranceArrangementPickListDetailId)
                .ForeignKey("dbo.TreatyPricingGroupReferralVersions", t => t.TreatyPricingGroupReferralVersionId)
                .ForeignKey("dbo.TreatyPricingUwLimits", t => t.TreatyPricingUwLimitId)
                .ForeignKey("dbo.TreatyPricingUwLimitVersions", t => t.TreatyPricingUwLimitVersionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingGroupReferralVersionId)
                .Index(t => t.BenefitId)
                .Index(t => t.ReinsuranceArrangementPickListDetailId)
                .Index(t => t.AgeBasisPickListDetailId)
                .Index(t => t.TreatyPricingUwLimitId)
                .Index(t => t.TreatyPricingUwLimitVersionId)
                .Index(t => t.OtherSpecialReinsuranceArrangement)
                .Index(t => t.GroupProfitCommissionId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingUwLimits",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingCedantId = c.Int(nullable: false),
                    LimitId = c.String(maxLength: 30),
                    Name = c.String(maxLength: 255),
                    Description = c.String(maxLength: 255),
                    Status = c.Int(nullable: false),
                    BenefitCode = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingCedants", t => t.TreatyPricingCedantId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingCedantId)
                .Index(t => t.LimitId)
                .Index(t => t.Name)
                .Index(t => t.Description)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingUwLimitVersions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingUwLimitId = c.Int(nullable: false),
                    Version = c.Int(nullable: false),
                    PersonInChargeId = c.Int(nullable: false),
                    EffectiveAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    CurrencyCode = c.String(maxLength: 3),
                    UwLimit = c.String(maxLength: 255),
                    Remarks1 = c.String(maxLength: 255),
                    AblSumAssured = c.String(maxLength: 255),
                    Remarks2 = c.String(maxLength: 255),
                    AblMaxUwRating = c.String(maxLength: 255),
                    Remarks3 = c.String(maxLength: 255),
                    MaxSumAssured = c.String(maxLength: 255),
                    PerLifePerIndustry = c.Boolean(nullable: false),
                    IssuePayoutLimit = c.Boolean(nullable: false),
                    Remarks4 = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.PersonInChargeId)
                .ForeignKey("dbo.TreatyPricingUwLimits", t => t.TreatyPricingUwLimitId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingUwLimitId)
                .Index(t => t.Version)
                .Index(t => t.PersonInChargeId)
                .Index(t => t.EffectiveAt)
                .Index(t => t.CurrencyCode)
                .Index(t => t.UwLimit)
                .Index(t => t.Remarks1)
                .Index(t => t.AblSumAssured)
                .Index(t => t.Remarks2)
                .Index(t => t.AblMaxUwRating)
                .Index(t => t.Remarks3)
                .Index(t => t.MaxSumAssured)
                .Index(t => t.PerLifePerIndustry)
                .Index(t => t.IssuePayoutLimit)
                .Index(t => t.Remarks4)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingMedicalTableUploadCells",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingMedicalTableUploadColumnId = c.Int(nullable: false),
                    TreatyPricingMedicalTableUploadRowId = c.Int(nullable: false),
                    Code = c.String(maxLength: 30),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingMedicalTableUploadColumns", t => t.TreatyPricingMedicalTableUploadColumnId)
                .ForeignKey("dbo.TreatyPricingMedicalTableUploadRows", t => t.TreatyPricingMedicalTableUploadRowId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingMedicalTableUploadColumnId)
                .Index(t => t.TreatyPricingMedicalTableUploadRowId)
                .Index(t => t.Code)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingMedicalTableUploadColumns",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingMedicalTableVersionDetailId = c.Int(nullable: false),
                    MinimumAge = c.Int(nullable: false),
                    MaximumAge = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingMedicalTableVersionDetails", t => t.TreatyPricingMedicalTableVersionDetailId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingMedicalTableVersionDetailId)
                .Index(t => t.MinimumAge)
                .Index(t => t.MaximumAge)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingMedicalTableVersionDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingMedicalTableVersionId = c.Int(nullable: false),
                    DistributionTierPickListDetailId = c.Int(nullable: false),
                    Description = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickListDetails", t => t.DistributionTierPickListDetailId)
                .ForeignKey("dbo.TreatyPricingMedicalTableVersions", t => t.TreatyPricingMedicalTableVersionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingMedicalTableVersionId)
                .Index(t => t.DistributionTierPickListDetailId)
                .Index(t => t.Description)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingMedicalTableUploadRows",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingMedicalTableVersionDetailId = c.Int(nullable: false),
                    MinimumSumAssured = c.Int(nullable: false),
                    MaximumSumAssured = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingMedicalTableVersionDetails", t => t.TreatyPricingMedicalTableVersionDetailId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingMedicalTableVersionDetailId)
                .Index(t => t.MinimumSumAssured)
                .Index(t => t.MaximumSumAssured)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingMedicalTableUploadLegends",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingMedicalTableVersionDetailId = c.Int(nullable: false),
                    Code = c.String(maxLength: 30),
                    Description = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingMedicalTableVersionDetails", t => t.TreatyPricingMedicalTableVersionDetailId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingMedicalTableVersionDetailId)
                .Index(t => t.Code)
                .Index(t => t.Description)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingMedicalTableVersionFiles",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingMedicalTableVersionId = c.Int(nullable: false),
                    DistributionTierPickListDetailId = c.Int(nullable: false),
                    Description = c.String(maxLength: 255),
                    FileName = c.String(maxLength: 255),
                    HashFileName = c.String(maxLength: 255),
                    Status = c.Int(nullable: false),
                    Errors = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickListDetails", t => t.DistributionTierPickListDetailId)
                .ForeignKey("dbo.TreatyPricingMedicalTableVersions", t => t.TreatyPricingMedicalTableVersionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingMedicalTableVersionId)
                .Index(t => t.DistributionTierPickListDetailId)
                .Index(t => t.Description)
                .Index(t => t.FileName)
                .Index(t => t.HashFileName)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingPerLifeRetro",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Code = c.String(nullable: false, maxLength: 30),
                    RetroPartyId = c.Int(),
                    Type = c.Int(nullable: false),
                    RetrocessionaireShare = c.Double(),
                    Description = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.RetroParties", t => t.RetroPartyId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Code)
                .Index(t => t.RetroPartyId)
                .Index(t => t.Type)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingPerLifeRetroProducts",
                c => new
                {
                    TreatyPricingPerLifeRetroId = c.Int(nullable: false),
                    TreatyPricingProductId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.TreatyPricingPerLifeRetroId, t.TreatyPricingProductId })
                .ForeignKey("dbo.TreatyPricingPerLifeRetro", t => t.TreatyPricingPerLifeRetroId)
                .ForeignKey("dbo.TreatyPricingProducts", t => t.TreatyPricingProductId)
                .Index(t => t.TreatyPricingPerLifeRetroId)
                .Index(t => t.TreatyPricingProductId);

            CreateTable(
                "dbo.TreatyPricingPerLifeRetroVersionBenefits",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingPerLifeRetroVersionId = c.Int(nullable: false),
                    BenefitId = c.Int(nullable: false),
                    ArrangementRetrocessionnaireTypePickListDetailId = c.Int(),
                    TotalMortality = c.String(),
                    MlreRetention = c.Double(),
                    RetrocessionnaireShare = c.Double(),
                    AgeBasisPickListDetailId = c.Int(),
                    MinIssueAge = c.Int(),
                    MaxIssueAge = c.Int(),
                    MaxExpiryAge = c.Int(),
                    RetrocessionaireDiscount = c.String(),
                    RateTablePercentage = c.Double(),
                    ClaimApprovalLimit = c.Double(),
                    AutoBindingLimit = c.Double(),
                    IsProfitCommission = c.Boolean(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PickListDetails", t => t.AgeBasisPickListDetailId)
                .ForeignKey("dbo.PickListDetails", t => t.ArrangementRetrocessionnaireTypePickListDetailId)
                .ForeignKey("dbo.Benefits", t => t.BenefitId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingPerLifeRetroVersions", t => t.TreatyPricingPerLifeRetroVersionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingPerLifeRetroVersionId)
                .Index(t => t.BenefitId)
                .Index(t => t.ArrangementRetrocessionnaireTypePickListDetailId)
                .Index(t => t.AgeBasisPickListDetailId)
                .Index(t => t.IsProfitCommission)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingPerLifeRetroVersions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingPerLifeRetroId = c.Int(nullable: false),
                    Version = c.Int(nullable: false),
                    PersonInChargeId = c.Int(nullable: false),
                    RetrocessionaireRetroPartyId = c.Int(),
                    RefundofUnearnedPremium = c.String(maxLength: 128),
                    TerminationPeriod = c.Int(),
                    ResidenceCountry = c.String(maxLength: 128),
                    PaymentRetrocessionairePremiumPickListDetailId = c.Int(),
                    EffectiveDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    JumboLimit = c.Double(),
                    Remarks = c.String(maxLength: 255),
                    ProfitSharing = c.Int(),
                    ProfitDescription = c.String(maxLength: 255),
                    NetProfitPercentage = c.Double(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickListDetails", t => t.PaymentRetrocessionairePremiumPickListDetailId)
                .ForeignKey("dbo.Users", t => t.PersonInChargeId)
                .ForeignKey("dbo.RetroParties", t => t.RetrocessionaireRetroPartyId)
                .ForeignKey("dbo.TreatyPricingPerLifeRetro", t => t.TreatyPricingPerLifeRetroId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingPerLifeRetroId)
                .Index(t => t.Version)
                .Index(t => t.PersonInChargeId)
                .Index(t => t.RetrocessionaireRetroPartyId)
                .Index(t => t.PaymentRetrocessionairePremiumPickListDetailId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingPerLifeRetroVersionDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingPerLifeRetroVersionId = c.Int(nullable: false),
                    SortIndex = c.Int(nullable: false),
                    Item = c.Int(nullable: false),
                    Component = c.String(maxLength: 255),
                    IsComponentEditable = c.Boolean(nullable: false),
                    ComponentDescription = c.String(maxLength: 255),
                    IsComponentDescriptionEditable = c.Boolean(nullable: false),
                    IsDropDown = c.Boolean(nullable: false),
                    DropDownValue = c.Int(),
                    IsEnabled = c.Boolean(),
                    IsNetGrossRequired = c.Boolean(nullable: false),
                    IsNetGross = c.Boolean(),
                    Value = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingPerLifeRetroVersions", t => t.TreatyPricingPerLifeRetroVersionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingPerLifeRetroVersionId)
                .Index(t => t.SortIndex)
                .Index(t => t.Item)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingPerLifeRetroVersionTiers",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingPerLifeRetroVersionId = c.Int(nullable: false),
                    Col1 = c.String(maxLength: 255),
                    Col2 = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingPerLifeRetroVersions", t => t.TreatyPricingPerLifeRetroVersionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingPerLifeRetroVersionId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingPickListDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PickListId = c.Int(nullable: false),
                    ObjectType = c.Int(nullable: false),
                    ObjectId = c.Int(nullable: false),
                    PickListDetailCode = c.String(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickLists", t => t.PickListId)
                .Index(t => t.PickListId)
                .Index(t => t.ObjectType)
                .Index(t => t.ObjectId)
                .Index(t => t.CreatedById);

            CreateTable(
                "dbo.TreatyPricingProductBenefitDirectRetros",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingProductBenefitId = c.Int(nullable: false),
                    RetroPartyId = c.Int(nullable: false),
                    ArrangementRetrocessionTypePickListDetailId = c.Int(),
                    MlreRetention = c.String(maxLength: 128),
                    RetrocessionShare = c.String(maxLength: 128),
                    IsRetrocessionProfitCommission = c.Boolean(nullable: false),
                    IsRetrocessionAdvantageProgram = c.Boolean(nullable: false),
                    RetrocessionRateTable = c.String(maxLength: 128),
                    NewBusinessRateGuarantee = c.String(maxLength: 128),
                    RenewalBusinessRateGuarantee = c.String(maxLength: 128),
                    RetrocessionDiscount = c.String(maxLength: 128),
                    AdditionalDiscount = c.String(maxLength: 128),
                    AdditionalLoading = c.String(maxLength: 128),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PickListDetails", t => t.ArrangementRetrocessionTypePickListDetailId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.RetroParties", t => t.RetroPartyId)
                .ForeignKey("dbo.TreatyPricingProductBenefits", t => t.TreatyPricingProductBenefitId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingProductBenefitId)
                .Index(t => t.RetroPartyId)
                .Index(t => t.ArrangementRetrocessionTypePickListDetailId)
                .Index(t => t.MlreRetention)
                .Index(t => t.RetrocessionShare)
                .Index(t => t.IsRetrocessionProfitCommission)
                .Index(t => t.IsRetrocessionAdvantageProgram)
                .Index(t => t.RetrocessionRateTable)
                .Index(t => t.NewBusinessRateGuarantee)
                .Index(t => t.RenewalBusinessRateGuarantee)
                .Index(t => t.RetrocessionDiscount)
                .Index(t => t.AdditionalDiscount)
                .Index(t => t.AdditionalLoading)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingProductBenefits",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingProductVersionId = c.Int(nullable: false),
                    BenefitId = c.Int(nullable: false),
                    Name = c.String(maxLength: 255),
                    BasicRiderPickListDetailId = c.Int(),
                    PayoutTypePickListDetailId = c.Int(),
                    RiderAttachmentRatio = c.String(maxLength: 128),
                    AgeBasisPickListDetailId = c.Int(),
                    MinimumEntryAge = c.String(maxLength: 128),
                    MaximumEntryAge = c.String(maxLength: 128),
                    MaximumExpiryAge = c.String(maxLength: 128),
                    MinimumPolicyTerm = c.String(maxLength: 128),
                    MaximumPolicyTerm = c.String(maxLength: 128),
                    PremiumPayingTerm = c.String(maxLength: 128),
                    MinimumSumAssured = c.String(maxLength: 128),
                    MaximumSumAssured = c.String(maxLength: 128),
                    TreatyPricingUwLimitId = c.Int(),
                    TreatyPricingUwLimitVersionId = c.Int(),
                    Others = c.String(maxLength: 128),
                    TreatyPricingClaimApprovalLimitId = c.Int(),
                    TreatyPricingClaimApprovalLimitVersionId = c.Int(),
                    IfOthers1 = c.String(maxLength: 128),
                    TreatyPricingDefinitionAndExclusionId = c.Int(),
                    TreatyPricingDefinitionAndExclusionVersionId = c.Int(),
                    IfOthers2 = c.String(maxLength: 128),
                    RefundOfPremium = c.String(maxLength: 128),
                    PreExistingCondition = c.String(maxLength: 128),
                    PricingArrangementReinsuranceTypePickListDetailId = c.Int(),
                    BenefitPayout = c.String(maxLength: 128),
                    CedantRetention = c.String(maxLength: 128),
                    ReinsuranceShare = c.String(maxLength: 128),
                    CoinsuranceCedantRetention = c.String(maxLength: 128),
                    CoinsuranceReinsuranceShare = c.String(maxLength: 128),
                    RiskPatternSumPickListDetailId = c.Int(),
                    PricingUploadFileName = c.String(maxLength: 255),
                    PricingUploadHashFileName = c.String(maxLength: 255),
                    IsProfitCommission = c.Boolean(nullable: false),
                    IsAdvantageProgram = c.Boolean(nullable: false),
                    TreatyPricingRateTableId = c.Int(),
                    TreatyPricingRateTableVersionId = c.Int(),
                    RequestedRateGuarantee = c.String(maxLength: 128),
                    RequestedReinsuranceDiscount = c.String(maxLength: 128),
                    RequestedCoinsuranceRiDiscount = c.String(maxLength: 128),
                    InwardRetroParty = c.String(maxLength: 128),
                    InwardRetroArrangementReinsuranceTypePickListDetailId = c.Int(),
                    InwardRetroRetention = c.String(maxLength: 128),
                    MlreShare = c.String(maxLength: 128),
                    IsRetrocessionProfitCommission = c.Boolean(nullable: false),
                    IsRetrocessionAdvantageProgram = c.Boolean(nullable: false),
                    RetrocessionRateTable = c.String(maxLength: 128),
                    NewBusinessRateGuarantee = c.String(maxLength: 128),
                    RenewalBusinessRateGuarantee = c.String(maxLength: 128),
                    RetrocessionDiscount = c.String(maxLength: 128),
                    AdditionalDiscount = c.String(maxLength: 128),
                    AdditionalLoading = c.String(maxLength: 128),
                    WakalahFee = c.String(maxLength: 128),
                    MlreServiceFee = c.String(maxLength: 128),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PickListDetails", t => t.AgeBasisPickListDetailId)
                .ForeignKey("dbo.PickListDetails", t => t.BasicRiderPickListDetailId)
                .ForeignKey("dbo.Benefits", t => t.BenefitId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickListDetails", t => t.InwardRetroArrangementReinsuranceTypePickListDetailId)
                .ForeignKey("dbo.PickListDetails", t => t.PayoutTypePickListDetailId)
                .ForeignKey("dbo.PickListDetails", t => t.PricingArrangementReinsuranceTypePickListDetailId)
                .ForeignKey("dbo.PickListDetails", t => t.RiskPatternSumPickListDetailId)
                .ForeignKey("dbo.TreatyPricingClaimApprovalLimits", t => t.TreatyPricingClaimApprovalLimitId)
                .ForeignKey("dbo.TreatyPricingClaimApprovalLimitVersions", t => t.TreatyPricingClaimApprovalLimitVersionId)
                .ForeignKey("dbo.TreatyPricingDefinitionAndExclusion", t => t.TreatyPricingDefinitionAndExclusionId)
                .ForeignKey("dbo.TreatyPricingDefinitionAndExclusionVersions", t => t.TreatyPricingDefinitionAndExclusionVersionId)
                .ForeignKey("dbo.TreatyPricingProductVersions", t => t.TreatyPricingProductVersionId)
                .ForeignKey("dbo.TreatyPricingRateTables", t => t.TreatyPricingRateTableId)
                .ForeignKey("dbo.TreatyPricingRateTableVersions", t => t.TreatyPricingRateTableVersionId)
                .ForeignKey("dbo.TreatyPricingUwLimits", t => t.TreatyPricingUwLimitId)
                .ForeignKey("dbo.TreatyPricingUwLimitVersions", t => t.TreatyPricingUwLimitVersionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingProductVersionId)
                .Index(t => t.BenefitId)
                .Index(t => t.Name)
                .Index(t => t.BasicRiderPickListDetailId)
                .Index(t => t.PayoutTypePickListDetailId)
                .Index(t => t.RiderAttachmentRatio)
                .Index(t => t.AgeBasisPickListDetailId)
                .Index(t => t.MinimumEntryAge)
                .Index(t => t.MaximumEntryAge)
                .Index(t => t.MaximumExpiryAge)
                .Index(t => t.MinimumPolicyTerm)
                .Index(t => t.MaximumPolicyTerm)
                .Index(t => t.PremiumPayingTerm)
                .Index(t => t.MinimumSumAssured)
                .Index(t => t.MaximumSumAssured)
                .Index(t => t.TreatyPricingUwLimitId)
                .Index(t => t.TreatyPricingUwLimitVersionId)
                .Index(t => t.Others)
                .Index(t => t.TreatyPricingClaimApprovalLimitId)
                .Index(t => t.TreatyPricingClaimApprovalLimitVersionId)
                .Index(t => t.IfOthers1)
                .Index(t => t.TreatyPricingDefinitionAndExclusionId)
                .Index(t => t.TreatyPricingDefinitionAndExclusionVersionId)
                .Index(t => t.IfOthers2)
                .Index(t => t.RefundOfPremium)
                .Index(t => t.PreExistingCondition)
                .Index(t => t.PricingArrangementReinsuranceTypePickListDetailId)
                .Index(t => t.BenefitPayout)
                .Index(t => t.CedantRetention)
                .Index(t => t.ReinsuranceShare)
                .Index(t => t.CoinsuranceCedantRetention)
                .Index(t => t.CoinsuranceReinsuranceShare)
                .Index(t => t.RiskPatternSumPickListDetailId)
                .Index(t => t.PricingUploadFileName)
                .Index(t => t.PricingUploadHashFileName)
                .Index(t => t.IsProfitCommission)
                .Index(t => t.IsAdvantageProgram)
                .Index(t => t.TreatyPricingRateTableId)
                .Index(t => t.TreatyPricingRateTableVersionId)
                .Index(t => t.RequestedRateGuarantee)
                .Index(t => t.RequestedReinsuranceDiscount)
                .Index(t => t.RequestedCoinsuranceRiDiscount)
                .Index(t => t.InwardRetroParty)
                .Index(t => t.InwardRetroArrangementReinsuranceTypePickListDetailId)
                .Index(t => t.InwardRetroRetention)
                .Index(t => t.MlreShare)
                .Index(t => t.IsRetrocessionProfitCommission)
                .Index(t => t.IsRetrocessionAdvantageProgram)
                .Index(t => t.RetrocessionRateTable)
                .Index(t => t.NewBusinessRateGuarantee)
                .Index(t => t.RenewalBusinessRateGuarantee)
                .Index(t => t.RetrocessionDiscount)
                .Index(t => t.AdditionalDiscount)
                .Index(t => t.AdditionalLoading)
                .Index(t => t.WakalahFee)
                .Index(t => t.MlreServiceFee)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingProductDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingProductVersionId = c.Int(nullable: false),
                    Type = c.Int(nullable: false),
                    Col1 = c.String(nullable: false, maxLength: 255),
                    Col2 = c.String(nullable: false, maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingProductVersions", t => t.TreatyPricingProductVersionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingProductVersionId)
                .Index(t => t.Type)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingProductPerLifeRetros",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingProductId = c.Int(nullable: false),
                    TreatyPricingPerLifeRetroId = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingPerLifeRetro", t => t.TreatyPricingPerLifeRetroId)
                .ForeignKey("dbo.TreatyPricingProducts", t => t.TreatyPricingProductId)
                .Index(t => t.TreatyPricingProductId)
                .Index(t => t.TreatyPricingPerLifeRetroId)
                .Index(t => t.CreatedById);

            CreateTable(
                "dbo.TreatyPricingProfitCommissionDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingProfitCommissionVersionId = c.Int(nullable: false),
                    SortIndex = c.Int(nullable: false),
                    Item = c.Int(nullable: false),
                    Component = c.String(maxLength: 255),
                    IsComponentEditable = c.Boolean(nullable: false),
                    ComponentDescription = c.String(maxLength: 255),
                    IsComponentDescriptionEditable = c.Boolean(nullable: false),
                    IsDropDown = c.Boolean(nullable: false),
                    DropDownValue = c.Int(),
                    IsEnabled = c.Boolean(),
                    IsNetGrossRequired = c.Boolean(nullable: false),
                    IsNetGross = c.Boolean(),
                    Value = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingProfitCommissionVersions", t => t.TreatyPricingProfitCommissionVersionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingProfitCommissionVersionId)
                .Index(t => t.SortIndex)
                .Index(t => t.Item)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingQuotationWorkflows",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    QuotationId = c.String(maxLength: 30),
                    CedantId = c.Int(nullable: false),
                    ReinsuranceTypePickListDetailId = c.Int(),
                    Name = c.String(maxLength: 255),
                    Summary = c.String(storeType: "ntext"),
                    Status = c.Int(nullable: false),
                    StatusRemarks = c.String(storeType: "ntext"),
                    TargetSendDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    LatestRevisionDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    PricingTeamPickListDetailId = c.Int(),
                    PricingStatus = c.Int(),
                    TargetClientReleaseDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    TargetRateCompletionDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    FinaliseDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    Description = c.String(maxLength: 255),
                    LatestVersion = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                    BDPersonInChargeId = c.Int(),
                    PersonInChargeId = c.Int(),
                    CEOPending = c.Boolean(nullable: false),
                    PricingPending = c.Boolean(nullable: false),
                    UnderwritingPending = c.Boolean(nullable: false),
                    HealthPending = c.Boolean(nullable: false),
                    ClaimsPending = c.Boolean(nullable: false),
                    BDPending = c.Boolean(nullable: false),
                    TGPending = c.Boolean(nullable: false),
                    PricingDueDate = c.DateTime(precision: 7, storeType: "datetime2"),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cedants", t => t.CedantId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickListDetails", t => t.PricingTeamPickListDetailId)
                .ForeignKey("dbo.PickListDetails", t => t.ReinsuranceTypePickListDetailId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.QuotationId)
                .Index(t => t.CedantId)
                .Index(t => t.ReinsuranceTypePickListDetailId)
                .Index(t => t.Name)
                .Index(t => t.Status)
                .Index(t => t.TargetSendDate)
                .Index(t => t.LatestRevisionDate)
                .Index(t => t.PricingTeamPickListDetailId)
                .Index(t => t.PricingStatus)
                .Index(t => t.TargetClientReleaseDate)
                .Index(t => t.TargetRateCompletionDate)
                .Index(t => t.FinaliseDate)
                .Index(t => t.Description)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById)
                .Index(t => t.PricingDueDate);

            CreateTable(
                "dbo.TreatyPricingQuotationWorkflowVersionQuotationChecklists",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingQuotationWorkflowVersionId = c.Int(nullable: false),
                    InternalTeam = c.String(maxLength: 255),
                    InternalTeamPersonInCharge = c.String(maxLength: 255),
                    Status = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingQuotationWorkflowVersions", t => t.TreatyPricingQuotationWorkflowVersionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingQuotationWorkflowVersionId)
                .Index(t => t.InternalTeam)
                .Index(t => t.InternalTeamPersonInCharge)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingQuotationWorkflowVersions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingQuotationWorkflowId = c.Int(nullable: false),
                    Version = c.Int(nullable: false),
                    BDPersonInChargeId = c.Int(),
                    QuoteValidityDay = c.Int(),
                    QuoteSpecTemplate = c.String(storeType: "ntext"),
                    RateTableTemplate = c.String(storeType: "ntext"),
                    QuoteSpecSharePointLink = c.String(storeType: "ntext"),
                    QuoteSpecSharePointFolderPath = c.String(storeType: "ntext"),
                    RateTableSharePointLink = c.String(storeType: "ntext"),
                    RateTableSharePointFolderPath = c.String(storeType: "ntext"),
                    FinalQuoteSpecFileName = c.String(maxLength: 255),
                    FinalQuoteSpecHashFileName = c.String(maxLength: 255),
                    FinalRateTableFileName = c.String(maxLength: 255),
                    FinalRateTableHashFileName = c.String(maxLength: 255),
                    ChecklistFinalised = c.Boolean(nullable: false),
                    PersonInChargeId = c.Int(),
                    PersonInChargeTechReviewerId = c.Int(),
                    PersonInChargePeerReviewerId = c.Int(),
                    PersonInChargePricingAuthorityReviewerId = c.Int(),
                    PendingOnDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    RequestDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    TargetPricingDueDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    RevisedPricingDueDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    PricingCompletedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    ProfitMargin = c.Double(),
                    FirstYearPremium = c.Double(),
                    PVProfit = c.Double(),
                    ROE = c.Double(),
                    FileLocationPricingMemo = c.String(storeType: "ntext"),
                    FileLocationNBChecklist = c.String(storeType: "ntext"),
                    FileLocationTechnicalChecklist = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.BDPersonInChargeId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.PersonInChargeId)
                .ForeignKey("dbo.Users", t => t.PersonInChargePeerReviewerId)
                .ForeignKey("dbo.Users", t => t.PersonInChargePricingAuthorityReviewerId)
                .ForeignKey("dbo.Users", t => t.PersonInChargeTechReviewerId)
                .ForeignKey("dbo.TreatyPricingQuotationWorkflows", t => t.TreatyPricingQuotationWorkflowId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingQuotationWorkflowId)
                .Index(t => t.Version)
                .Index(t => t.BDPersonInChargeId)
                .Index(t => t.QuoteValidityDay)
                .Index(t => t.FinalQuoteSpecFileName)
                .Index(t => t.FinalQuoteSpecHashFileName)
                .Index(t => t.FinalRateTableFileName)
                .Index(t => t.FinalRateTableHashFileName)
                .Index(t => t.ChecklistFinalised)
                .Index(t => t.PersonInChargeId)
                .Index(t => t.PersonInChargeTechReviewerId)
                .Index(t => t.PersonInChargePeerReviewerId)
                .Index(t => t.PersonInChargePricingAuthorityReviewerId)
                .Index(t => t.PendingOnDate)
                .Index(t => t.RequestDate)
                .Index(t => t.TargetPricingDueDate)
                .Index(t => t.RevisedPricingDueDate)
                .Index(t => t.PricingCompletedDate)
                .Index(t => t.ProfitMargin)
                .Index(t => t.FirstYearPremium)
                .Index(t => t.PVProfit)
                .Index(t => t.ROE)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingRateTableDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingRateTableVersionId = c.Int(nullable: false),
                    Type = c.Int(nullable: false),
                    Col1 = c.String(nullable: false, maxLength: 255),
                    Col2 = c.String(nullable: false, maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingRateTableVersions", t => t.TreatyPricingRateTableVersionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingRateTableVersionId)
                .Index(t => t.Type)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingRateTableRates",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingRateTableVersionId = c.Int(nullable: false),
                    Age = c.Int(nullable: false),
                    MaleNonSmoker = c.Double(),
                    MaleSmoker = c.Double(),
                    FemaleNonSmoker = c.Double(),
                    FemaleSmoker = c.Double(),
                    Male = c.Double(),
                    Female = c.Double(),
                    Unisex = c.Double(),
                    UnitRate = c.Double(),
                    OccupationClass = c.Double(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingRateTableVersions", t => t.TreatyPricingRateTableVersionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingRateTableVersionId)
                .Index(t => t.Age)
                .Index(t => t.MaleNonSmoker)
                .Index(t => t.MaleSmoker)
                .Index(t => t.FemaleNonSmoker)
                .Index(t => t.FemaleSmoker)
                .Index(t => t.Male)
                .Index(t => t.Female)
                .Index(t => t.Unisex)
                .Index(t => t.UnitRate)
                .Index(t => t.OccupationClass)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingReportGenerations",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ReportName = c.String(maxLength: 255),
                    ReportParams = c.String(storeType: "ntext"),
                    FileName = c.String(maxLength: 255),
                    HashFileName = c.String(maxLength: 255),
                    Status = c.Int(nullable: false),
                    Errors = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.ReportName)
                .Index(t => t.FileName)
                .Index(t => t.HashFileName)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingTierProfitCommissions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingProfitCommissionVersionId = c.Int(nullable: false),
                    Col1 = c.String(maxLength: 255),
                    Col2 = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingProfitCommissionVersions", t => t.TreatyPricingProfitCommissionVersionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingProfitCommissionVersionId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingTreatyWorkflows",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DocumentType = c.Int(nullable: false),
                    ReinsuranceTypePickListDetailId = c.Int(nullable: false),
                    CounterPartyDetailId = c.Int(nullable: false),
                    InwardRetroPartyDetailId = c.Int(),
                    BusinessOriginPickListDetailId = c.Int(),
                    BusinessTypePickListDetailId = c.Int(),
                    DocumentId = c.String(maxLength: 255),
                    TreatyCode = c.String(maxLength: 255),
                    CoverageStatus = c.Int(),
                    DocumentStatus = c.Int(),
                    DraftingStatus = c.Int(),
                    DraftingStatusCategory = c.Int(),
                    EffectiveAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    OrionGroup = c.Double(),
                    OrionGroupStr = c.String(),
                    Description = c.String(),
                    SharepointLink = c.String(),
                    Reviewer = c.String(),
                    LatestVersion = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PickListDetails", t => t.BusinessOriginPickListDetailId)
                .ForeignKey("dbo.PickListDetails", t => t.BusinessTypePickListDetailId)
                .ForeignKey("dbo.Cedants", t => t.CounterPartyDetailId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.RetroParties", t => t.InwardRetroPartyDetailId)
                .ForeignKey("dbo.PickListDetails", t => t.ReinsuranceTypePickListDetailId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.DocumentType)
                .Index(t => t.ReinsuranceTypePickListDetailId)
                .Index(t => t.CounterPartyDetailId)
                .Index(t => t.InwardRetroPartyDetailId)
                .Index(t => t.BusinessOriginPickListDetailId)
                .Index(t => t.BusinessTypePickListDetailId)
                .Index(t => t.DocumentId)
                .Index(t => t.TreatyCode)
                .Index(t => t.CoverageStatus)
                .Index(t => t.DocumentStatus)
                .Index(t => t.DraftingStatus)
                .Index(t => t.DraftingStatusCategory)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingTreatyWorkflowVersions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingTreatyWorkflowId = c.Int(nullable: false),
                    Version = c.Int(nullable: false),
                    RequestDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    TargetSentDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    DateSentToReviewer1st = c.DateTime(precision: 7, storeType: "datetime2"),
                    DateSentToClient1st = c.DateTime(precision: 7, storeType: "datetime2"),
                    LatestRevisionDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    SignedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    ReportedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    PersonInChargeId = c.Int(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.PersonInChargeId)
                .ForeignKey("dbo.TreatyPricingTreatyWorkflows", t => t.TreatyPricingTreatyWorkflowId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingTreatyWorkflowId)
                .Index(t => t.Version)
                .Index(t => t.PersonInChargeId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingUwQuestionnaireVersionDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingUwQuestionnaireVersionId = c.Int(nullable: false),
                    UwQuestionnaireCategoryId = c.Int(nullable: false),
                    Question = c.String(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingUwQuestionnaireVersions", t => t.TreatyPricingUwQuestionnaireVersionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .ForeignKey("dbo.UwQuestionnaireCategories", t => t.UwQuestionnaireCategoryId)
                .Index(t => t.TreatyPricingUwQuestionnaireVersionId)
                .Index(t => t.UwQuestionnaireCategoryId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.UwQuestionnaireCategories",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Code = c.String(nullable: false, maxLength: 255),
                    Name = c.String(nullable: false, maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Code)
                .Index(t => t.Name)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingUwQuestionnaireVersionFiles",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyPricingUwQuestionnaireVersionId = c.Int(nullable: false),
                    FileName = c.String(maxLength: 255),
                    HashFileName = c.String(maxLength: 255),
                    Status = c.Int(nullable: false),
                    Errors = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingUwQuestionnaireVersions", t => t.TreatyPricingUwQuestionnaireVersionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingUwQuestionnaireVersionId)
                .Index(t => t.FileName)
                .Index(t => t.HashFileName)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyPricingWorkflowObjects",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Type = c.Int(nullable: false),
                    WorkflowId = c.Int(nullable: false),
                    ObjectType = c.Int(nullable: false),
                    ObjectId = c.Int(nullable: false),
                    ObjectVersionId = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Type)
                .Index(t => t.WorkflowId)
                .Index(t => t.ObjectType)
                .Index(t => t.ObjectId)
                .Index(t => t.ObjectVersionId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.ValidDuplicationLists",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyCodeId = c.Int(),
                    CedantPlanCode = c.String(maxLength: 255),
                    InsuredName = c.String(maxLength: 100),
                    InsuredDateOfBirth = c.DateTime(),
                    PolicyNumber = c.String(maxLength: 50),
                    InsuredGenderCodePickListDetailId = c.Int(),
                    MLReBenefitCodeId = c.Int(),
                    CedingBenefitRiskCode = c.String(maxLength: 30),
                    CedingBenefitTypeCode = c.String(maxLength: 30),
                    ReinsuranceEffectiveDate = c.DateTime(),
                    FundsAccountingTypePickListDetailId = c.Int(),
                    ReinsBasisCodePickListDetailId = c.Int(),
                    TransactionTypePickListDetailId = c.Int(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickListDetails", t => t.FundsAccountingTypePickListDetailId)
                .ForeignKey("dbo.PickListDetails", t => t.InsuredGenderCodePickListDetailId)
                .ForeignKey("dbo.Benefits", t => t.MLReBenefitCodeId)
                .ForeignKey("dbo.PickListDetails", t => t.ReinsBasisCodePickListDetailId)
                .ForeignKey("dbo.PickListDetails", t => t.TransactionTypePickListDetailId)
                .ForeignKey("dbo.TreatyCodes", t => t.TreatyCodeId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyCodeId)
                .Index(t => t.CedantPlanCode)
                .Index(t => t.InsuredName)
                .Index(t => t.InsuredDateOfBirth)
                .Index(t => t.PolicyNumber)
                .Index(t => t.InsuredGenderCodePickListDetailId)
                .Index(t => t.MLReBenefitCodeId)
                .Index(t => t.CedingBenefitRiskCode)
                .Index(t => t.CedingBenefitTypeCode)
                .Index(t => t.ReinsuranceEffectiveDate)
                .Index(t => t.FundsAccountingTypePickListDetailId)
                .Index(t => t.ReinsBasisCodePickListDetailId)
                .Index(t => t.TransactionTypePickListDetailId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            AddColumn("dbo.StatusHistories", "SubModuleController", c => c.String(maxLength: 64));
            AddColumn("dbo.StatusHistories", "SubObjectId", c => c.Int());
            AddColumn("dbo.StatusHistories", "Version", c => c.Int());
            AddColumn("dbo.PremiumSpreadTables", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.TreatyDiscountTables", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.Remarks", "StatusHistoryId", c => c.Long());
            AddColumn("dbo.Remarks", "Subject", c => c.String(maxLength: 128));
            AddColumn("dbo.Remarks", "Version", c => c.Int());
            AddColumn("dbo.RetroRegisterBatches", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.TreatyDiscountTableDetails", "AARFrom", c => c.Int());
            AddColumn("dbo.TreatyDiscountTableDetails", "AARTo", c => c.Int());
            CreateIndex("dbo.StatusHistories", "SubModuleController");
            CreateIndex("dbo.StatusHistories", "SubObjectId");
            CreateIndex("dbo.StatusHistories", "Version");
            CreateIndex("dbo.Remarks", "StatusHistoryId");
            CreateIndex("dbo.Remarks", "Subject");
            CreateIndex("dbo.Remarks", "Version");
            CreateIndex("dbo.TreatyDiscountTableDetails", "AARFrom");
            CreateIndex("dbo.TreatyDiscountTableDetails", "AARTo");
            AddForeignKey("dbo.Remarks", "StatusHistoryId", "dbo.StatusHistories", "Id");
            AddForeignKey("dbo.Mfrs17ContractCodeDetails", "CreatedById", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Mfrs17ContractCodeDetails", "Mfrs17ContractCodeId", "dbo.Mfrs17ContractCodes", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Mfrs17ContractCodeDetails", "Mfrs17ContractCodeId", "dbo.Mfrs17ContractCodes");
            DropForeignKey("dbo.Mfrs17ContractCodeDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ValidDuplicationLists", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.ValidDuplicationLists", "TreatyCodeId", "dbo.TreatyCodes");
            DropForeignKey("dbo.ValidDuplicationLists", "TransactionTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.ValidDuplicationLists", "ReinsBasisCodePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.ValidDuplicationLists", "MLReBenefitCodeId", "dbo.Benefits");
            DropForeignKey("dbo.ValidDuplicationLists", "InsuredGenderCodePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.ValidDuplicationLists", "FundsAccountingTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.ValidDuplicationLists", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingWorkflowObjects", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingWorkflowObjects", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingUwQuestionnaireVersionFiles", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingUwQuestionnaireVersionFiles", "TreatyPricingUwQuestionnaireVersionId", "dbo.TreatyPricingUwQuestionnaireVersions");
            DropForeignKey("dbo.TreatyPricingUwQuestionnaireVersionFiles", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingUwQuestionnaireVersionDetails", "UwQuestionnaireCategoryId", "dbo.UwQuestionnaireCategories");
            DropForeignKey("dbo.UwQuestionnaireCategories", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.UwQuestionnaireCategories", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingUwQuestionnaireVersionDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingUwQuestionnaireVersionDetails", "TreatyPricingUwQuestionnaireVersionId", "dbo.TreatyPricingUwQuestionnaireVersions");
            DropForeignKey("dbo.TreatyPricingUwQuestionnaireVersionDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingTreatyWorkflowVersions", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingTreatyWorkflowVersions", "TreatyPricingTreatyWorkflowId", "dbo.TreatyPricingTreatyWorkflows");
            DropForeignKey("dbo.TreatyPricingTreatyWorkflowVersions", "PersonInChargeId", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingTreatyWorkflowVersions", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingTreatyWorkflows", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingTreatyWorkflows", "ReinsuranceTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingTreatyWorkflows", "InwardRetroPartyDetailId", "dbo.RetroParties");
            DropForeignKey("dbo.TreatyPricingTreatyWorkflows", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingTreatyWorkflows", "CounterPartyDetailId", "dbo.Cedants");
            DropForeignKey("dbo.TreatyPricingTreatyWorkflows", "BusinessTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingTreatyWorkflows", "BusinessOriginPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingTierProfitCommissions", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingTierProfitCommissions", "TreatyPricingProfitCommissionVersionId", "dbo.TreatyPricingProfitCommissionVersions");
            DropForeignKey("dbo.TreatyPricingTierProfitCommissions", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingReportGenerations", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingReportGenerations", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingRateTableRates", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingRateTableRates", "TreatyPricingRateTableVersionId", "dbo.TreatyPricingRateTableVersions");
            DropForeignKey("dbo.TreatyPricingRateTableRates", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingRateTableDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingRateTableDetails", "TreatyPricingRateTableVersionId", "dbo.TreatyPricingRateTableVersions");
            DropForeignKey("dbo.TreatyPricingRateTableDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingQuotationWorkflowVersionQuotationChecklists", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingQuotationWorkflowVersionQuotationChecklists", "TreatyPricingQuotationWorkflowVersionId", "dbo.TreatyPricingQuotationWorkflowVersions");
            DropForeignKey("dbo.TreatyPricingQuotationWorkflowVersions", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingQuotationWorkflowVersions", "TreatyPricingQuotationWorkflowId", "dbo.TreatyPricingQuotationWorkflows");
            DropForeignKey("dbo.TreatyPricingQuotationWorkflowVersions", "PersonInChargeTechReviewerId", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingQuotationWorkflowVersions", "PersonInChargePricingAuthorityReviewerId", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingQuotationWorkflowVersions", "PersonInChargePeerReviewerId", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingQuotationWorkflowVersions", "PersonInChargeId", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingQuotationWorkflowVersions", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingQuotationWorkflowVersions", "BDPersonInChargeId", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingQuotationWorkflowVersionQuotationChecklists", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingQuotationWorkflows", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingQuotationWorkflows", "ReinsuranceTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingQuotationWorkflows", "PricingTeamPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingQuotationWorkflows", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingQuotationWorkflows", "CedantId", "dbo.Cedants");
            DropForeignKey("dbo.TreatyPricingProfitCommissionDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingProfitCommissionDetails", "TreatyPricingProfitCommissionVersionId", "dbo.TreatyPricingProfitCommissionVersions");
            DropForeignKey("dbo.TreatyPricingProfitCommissionDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingProductPerLifeRetros", "TreatyPricingProductId", "dbo.TreatyPricingProducts");
            DropForeignKey("dbo.TreatyPricingProductPerLifeRetros", "TreatyPricingPerLifeRetroId", "dbo.TreatyPricingPerLifeRetro");
            DropForeignKey("dbo.TreatyPricingProductPerLifeRetros", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingProductDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingProductDetails", "TreatyPricingProductVersionId", "dbo.TreatyPricingProductVersions");
            DropForeignKey("dbo.TreatyPricingProductDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingProductBenefitDirectRetros", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingProductBenefitDirectRetros", "TreatyPricingProductBenefitId", "dbo.TreatyPricingProductBenefits");
            DropForeignKey("dbo.TreatyPricingProductBenefits", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingProductBenefits", "TreatyPricingUwLimitVersionId", "dbo.TreatyPricingUwLimitVersions");
            DropForeignKey("dbo.TreatyPricingProductBenefits", "TreatyPricingUwLimitId", "dbo.TreatyPricingUwLimits");
            DropForeignKey("dbo.TreatyPricingProductBenefits", "TreatyPricingRateTableVersionId", "dbo.TreatyPricingRateTableVersions");
            DropForeignKey("dbo.TreatyPricingProductBenefits", "TreatyPricingRateTableId", "dbo.TreatyPricingRateTables");
            DropForeignKey("dbo.TreatyPricingProductBenefits", "TreatyPricingProductVersionId", "dbo.TreatyPricingProductVersions");
            DropForeignKey("dbo.TreatyPricingProductBenefits", "TreatyPricingDefinitionAndExclusionVersionId", "dbo.TreatyPricingDefinitionAndExclusionVersions");
            DropForeignKey("dbo.TreatyPricingProductBenefits", "TreatyPricingDefinitionAndExclusionId", "dbo.TreatyPricingDefinitionAndExclusion");
            DropForeignKey("dbo.TreatyPricingProductBenefits", "TreatyPricingClaimApprovalLimitVersionId", "dbo.TreatyPricingClaimApprovalLimitVersions");
            DropForeignKey("dbo.TreatyPricingProductBenefits", "TreatyPricingClaimApprovalLimitId", "dbo.TreatyPricingClaimApprovalLimits");
            DropForeignKey("dbo.TreatyPricingProductBenefits", "RiskPatternSumPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingProductBenefits", "PricingArrangementReinsuranceTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingProductBenefits", "PayoutTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingProductBenefits", "InwardRetroArrangementReinsuranceTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingProductBenefits", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingProductBenefits", "BenefitId", "dbo.Benefits");
            DropForeignKey("dbo.TreatyPricingProductBenefits", "BasicRiderPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingProductBenefits", "AgeBasisPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingProductBenefitDirectRetros", "RetroPartyId", "dbo.RetroParties");
            DropForeignKey("dbo.TreatyPricingProductBenefitDirectRetros", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingProductBenefitDirectRetros", "ArrangementRetrocessionTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingPickListDetails", "PickListId", "dbo.PickLists");
            DropForeignKey("dbo.TreatyPricingPickListDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingPerLifeRetroVersionTiers", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingPerLifeRetroVersionTiers", "TreatyPricingPerLifeRetroVersionId", "dbo.TreatyPricingPerLifeRetroVersions");
            DropForeignKey("dbo.TreatyPricingPerLifeRetroVersionTiers", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingPerLifeRetroVersionDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingPerLifeRetroVersionDetails", "TreatyPricingPerLifeRetroVersionId", "dbo.TreatyPricingPerLifeRetroVersions");
            DropForeignKey("dbo.TreatyPricingPerLifeRetroVersionDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingPerLifeRetroVersionBenefits", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingPerLifeRetroVersionBenefits", "TreatyPricingPerLifeRetroVersionId", "dbo.TreatyPricingPerLifeRetroVersions");
            DropForeignKey("dbo.TreatyPricingPerLifeRetroVersions", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingPerLifeRetroVersions", "TreatyPricingPerLifeRetroId", "dbo.TreatyPricingPerLifeRetro");
            DropForeignKey("dbo.TreatyPricingPerLifeRetroVersions", "RetrocessionaireRetroPartyId", "dbo.RetroParties");
            DropForeignKey("dbo.TreatyPricingPerLifeRetroVersions", "PersonInChargeId", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingPerLifeRetroVersions", "PaymentRetrocessionairePremiumPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingPerLifeRetroVersions", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingPerLifeRetroVersionBenefits", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingPerLifeRetroVersionBenefits", "BenefitId", "dbo.Benefits");
            DropForeignKey("dbo.TreatyPricingPerLifeRetroVersionBenefits", "ArrangementRetrocessionnaireTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingPerLifeRetroVersionBenefits", "AgeBasisPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingPerLifeRetroProducts", "TreatyPricingProductId", "dbo.TreatyPricingProducts");
            DropForeignKey("dbo.TreatyPricingPerLifeRetroProducts", "TreatyPricingPerLifeRetroId", "dbo.TreatyPricingPerLifeRetro");
            DropForeignKey("dbo.TreatyPricingPerLifeRetro", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingPerLifeRetro", "RetroPartyId", "dbo.RetroParties");
            DropForeignKey("dbo.TreatyPricingPerLifeRetro", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingMedicalTableVersionFiles", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingMedicalTableVersionFiles", "TreatyPricingMedicalTableVersionId", "dbo.TreatyPricingMedicalTableVersions");
            DropForeignKey("dbo.TreatyPricingMedicalTableVersionFiles", "DistributionTierPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingMedicalTableVersionFiles", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingMedicalTableUploadLegends", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingMedicalTableUploadLegends", "TreatyPricingMedicalTableVersionDetailId", "dbo.TreatyPricingMedicalTableVersionDetails");
            DropForeignKey("dbo.TreatyPricingMedicalTableUploadLegends", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingMedicalTableUploadCells", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingMedicalTableUploadCells", "TreatyPricingMedicalTableUploadRowId", "dbo.TreatyPricingMedicalTableUploadRows");
            DropForeignKey("dbo.TreatyPricingMedicalTableUploadRows", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingMedicalTableUploadRows", "TreatyPricingMedicalTableVersionDetailId", "dbo.TreatyPricingMedicalTableVersionDetails");
            DropForeignKey("dbo.TreatyPricingMedicalTableUploadRows", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingMedicalTableUploadCells", "TreatyPricingMedicalTableUploadColumnId", "dbo.TreatyPricingMedicalTableUploadColumns");
            DropForeignKey("dbo.TreatyPricingMedicalTableUploadColumns", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingMedicalTableUploadColumns", "TreatyPricingMedicalTableVersionDetailId", "dbo.TreatyPricingMedicalTableVersionDetails");
            DropForeignKey("dbo.TreatyPricingMedicalTableVersionDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingMedicalTableVersionDetails", "TreatyPricingMedicalTableVersionId", "dbo.TreatyPricingMedicalTableVersions");
            DropForeignKey("dbo.TreatyPricingMedicalTableVersionDetails", "DistributionTierPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingMedicalTableVersionDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingMedicalTableUploadColumns", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingMedicalTableUploadCells", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingGroupReferralVersionBenefits", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingGroupReferralVersionBenefits", "TreatyPricingUwLimitVersionId", "dbo.TreatyPricingUwLimitVersions");
            DropForeignKey("dbo.TreatyPricingUwLimitVersions", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingUwLimitVersions", "TreatyPricingUwLimitId", "dbo.TreatyPricingUwLimits");
            DropForeignKey("dbo.TreatyPricingUwLimitVersions", "PersonInChargeId", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingUwLimitVersions", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingGroupReferralVersionBenefits", "TreatyPricingUwLimitId", "dbo.TreatyPricingUwLimits");
            DropForeignKey("dbo.TreatyPricingUwLimits", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingUwLimits", "TreatyPricingCedantId", "dbo.TreatyPricingCedants");
            DropForeignKey("dbo.TreatyPricingUwLimits", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingGroupReferralVersionBenefits", "TreatyPricingGroupReferralVersionId", "dbo.TreatyPricingGroupReferralVersions");
            DropForeignKey("dbo.TreatyPricingGroupReferralVersionBenefits", "ReinsuranceArrangementPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingGroupReferralVersionBenefits", "GroupProfitCommissionId", "dbo.TreatyPricingProfitCommissions");
            DropForeignKey("dbo.TreatyPricingGroupReferralVersionBenefits", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingGroupReferralVersionBenefits", "BenefitId", "dbo.Benefits");
            DropForeignKey("dbo.TreatyPricingGroupReferralVersionBenefits", "AgeBasisPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingGroupReferralHipsTables", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingGroupReferralHipsTables", "TreatyPricingGroupReferralFileId", "dbo.TreatyPricingGroupReferralFiles");
            DropForeignKey("dbo.TreatyPricingGroupReferralHipsTables", "TreatyPricingGroupReferralId", "dbo.TreatyPricingGroupReferrals");
            DropForeignKey("dbo.TreatyPricingGroupReferralHipsTables", "HipsSubCategoryId", "dbo.HipsCategoryDetails");
            DropForeignKey("dbo.TreatyPricingGroupReferralHipsTables", "HipsCategoryId", "dbo.HipsCategories");
            DropForeignKey("dbo.TreatyPricingGroupReferralHipsTables", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingGroupReferralGtlTables", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingGroupReferralGtlTables", "TreatyPricingGroupReferralFileId", "dbo.TreatyPricingGroupReferralFiles");
            DropForeignKey("dbo.TreatyPricingGroupReferralGtlTables", "TreatyPricingGroupReferralId", "dbo.TreatyPricingGroupReferrals");
            DropForeignKey("dbo.TreatyPricingGroupReferralGtlTables", "GtlBenefitCategoryId", "dbo.GtlBenefitCategories");
            DropForeignKey("dbo.TreatyPricingGroupReferralGtlTables", "DesignationId", "dbo.Designations");
            DropForeignKey("dbo.TreatyPricingGroupReferralGtlTables", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingGroupReferralGhsTables", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingGroupReferralGhsTables", "TreatyPricingGroupReferralFileId", "dbo.TreatyPricingGroupReferralFiles");
            DropForeignKey("dbo.TreatyPricingGroupReferralGhsTables", "TreatyPricingGroupReferralId", "dbo.TreatyPricingGroupReferrals");
            DropForeignKey("dbo.TreatyPricingGroupReferralGhsTables", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingGroupReferralFiles", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingGroupReferralFiles", "TreatyPricingGroupReferralId", "dbo.TreatyPricingGroupReferrals");
            DropForeignKey("dbo.TreatyPricingGroupReferralFiles", "TableTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingGroupReferralFiles", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingGroupReferralChecklists", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingGroupReferralChecklists", "TreatyPricingGroupReferralVersionId", "dbo.TreatyPricingGroupReferralVersions");
            DropForeignKey("dbo.TreatyPricingGroupReferralChecklists", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingGroupReferralChecklistDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingGroupReferralChecklistDetails", "TreatyPricingGroupReferralVersionId", "dbo.TreatyPricingGroupReferralVersions");
            DropForeignKey("dbo.TreatyPricingGroupReferralChecklistDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingGroupMasterLetterGroupReferrals", "TreatyPricingGroupReferralId", "dbo.TreatyPricingGroupReferrals");
            DropForeignKey("dbo.TreatyPricingGroupReferrals", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingGroupReferrals", "TreatyPricingGroupMasterLetterId", "dbo.TreatyPricingGroupMasterLetters");
            DropForeignKey("dbo.TreatyPricingGroupReferrals", "SecondaryTreatyPricingProductVersionId", "dbo.TreatyPricingProductVersions");
            DropForeignKey("dbo.TreatyPricingGroupReferrals", "SecondaryTreatyPricingProductId", "dbo.TreatyPricingProducts");
            DropForeignKey("dbo.TreatyPricingGroupReferrals", "RiGroupSlipVersionId", "dbo.TreatyPricingGroupReferralVersions");
            DropForeignKey("dbo.TreatyPricingGroupReferrals", "RiGroupSlipTemplateId", "dbo.Templates");
            DropForeignKey("dbo.TreatyPricingGroupReferrals", "RiGroupSlipPersonInChargeId", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingGroupReferrals", "RiArrangementPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingGroupReferrals", "ReplyVersionId", "dbo.TreatyPricingGroupReferralVersions");
            DropForeignKey("dbo.TreatyPricingGroupReferralVersions", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingGroupReferralVersions", "UnderwritingMethodPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingGroupReferralVersions", "TreatyPricingGroupReferralId", "dbo.TreatyPricingGroupReferrals");
            DropForeignKey("dbo.TreatyPricingGroupReferralVersions", "RequestTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingGroupReferralVersions", "PremiumTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingGroupReferralVersions", "GroupReferralPersonInChargeId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingGroupReferralVersions", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingGroupReferrals", "ReplyTemplateId", "dbo.Templates");
            DropForeignKey("dbo.TreatyPricingGroupReferrals", "ReferredTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingGroupReferrals", "PrimaryTreatyPricingProductVersionId", "dbo.TreatyPricingProductVersions");
            DropForeignKey("dbo.TreatyPricingProductVersions", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingProductVersions", "UnearnedPremiumRefundPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingProductVersions", "TreatyPricingUwQuestionnaireVersionId", "dbo.TreatyPricingUwQuestionnaireVersions");
            DropForeignKey("dbo.TreatyPricingProductVersions", "TreatyPricingUwQuestionnaireId", "dbo.TreatyPricingUwQuestionnaires");
            DropForeignKey("dbo.TreatyPricingProductVersions", "TreatyPricingProfitCommissionVersionId", "dbo.TreatyPricingProfitCommissionVersions");
            DropForeignKey("dbo.TreatyPricingProductVersions", "TreatyPricingProfitCommissionId", "dbo.TreatyPricingProfitCommissions");
            DropForeignKey("dbo.TreatyPricingProductVersions", "TreatyPricingProductId", "dbo.TreatyPricingProducts");
            DropForeignKey("dbo.TreatyPricingProductVersions", "TreatyPricingMedicalTableVersionId", "dbo.TreatyPricingMedicalTableVersions");
            DropForeignKey("dbo.TreatyPricingProductVersions", "TreatyPricingMedicalTableId", "dbo.TreatyPricingMedicalTables");
            DropForeignKey("dbo.TreatyPricingProductVersions", "TreatyPricingFinancialTableVersionId", "dbo.TreatyPricingFinancialTableVersions");
            DropForeignKey("dbo.TreatyPricingProductVersions", "TreatyPricingFinancialTableId", "dbo.TreatyPricingFinancialTables");
            DropForeignKey("dbo.TreatyPricingProductVersions", "TreatyPricingAdvantageProgramVersionId", "dbo.TreatyPricingAdvantageProgramVersions");
            DropForeignKey("dbo.TreatyPricingProductVersions", "TreatyPricingAdvantageProgramId", "dbo.TreatyPricingAdvantagePrograms");
            DropForeignKey("dbo.TreatyPricingProductVersions", "ReinsurancePremiumPaymentPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingProductVersions", "ReinsuranceArrangementPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingProductVersions", "ProductTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingProductVersions", "PersonInChargeId", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingProductVersions", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingProductVersions", "BusinessTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingProductVersions", "BusinessOriginPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingGroupReferrals", "PrimaryTreatyPricingProductId", "dbo.TreatyPricingProducts");
            DropForeignKey("dbo.TreatyPricingGroupReferrals", "InsuredGroupNameId", "dbo.InsuredGroupNames");
            DropForeignKey("dbo.TreatyPricingGroupReferrals", "IndustryNamePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingGroupReferrals", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingGroupReferrals", "CedantId", "dbo.Cedants");
            DropForeignKey("dbo.TreatyPricingGroupMasterLetterGroupReferrals", "TreatyPricingGroupMasterLetterId", "dbo.TreatyPricingGroupMasterLetters");
            DropForeignKey("dbo.TreatyPricingGroupMasterLetters", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingGroupMasterLetters", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingGroupMasterLetters", "CedantId", "dbo.Cedants");
            DropForeignKey("dbo.TreatyPricingGroupMasterLetterGroupReferrals", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingFinancialTableVersionFiles", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingFinancialTableVersionFiles", "TreatyPricingFinancialTableVersionId", "dbo.TreatyPricingFinancialTableVersions");
            DropForeignKey("dbo.TreatyPricingFinancialTableVersionFiles", "DistributionTierPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingFinancialTableVersionFiles", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingFinancialTableUploads", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingFinancialTableUploads", "TreatyPricingFinancialTableVersionDetailId", "dbo.TreatyPricingFinancialTableVersionDetails");
            DropForeignKey("dbo.TreatyPricingFinancialTableUploads", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingFinancialTableUploadLegends", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingFinancialTableUploadLegends", "TreatyPricingFinancialTableVersionDetailId", "dbo.TreatyPricingFinancialTableVersionDetails");
            DropForeignKey("dbo.TreatyPricingFinancialTableVersionDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingFinancialTableVersionDetails", "TreatyPricingFinancialTableVersionId", "dbo.TreatyPricingFinancialTableVersions");
            DropForeignKey("dbo.TreatyPricingFinancialTableVersionDetails", "DistributionTierPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingFinancialTableVersionDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingFinancialTableUploadLegends", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingDefinitionAndExclusionVersions", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingDefinitionAndExclusionVersions", "TreatyPricingDefinitionAndExclusionId", "dbo.TreatyPricingDefinitionAndExclusion");
            DropForeignKey("dbo.TreatyPricingDefinitionAndExclusionVersions", "PersonInChargeId", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingDefinitionAndExclusionVersions", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingDefinitionAndExclusion", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingDefinitionAndExclusion", "TreatyPricingCedantId", "dbo.TreatyPricingCedants");
            DropForeignKey("dbo.TreatyPricingDefinitionAndExclusion", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingCustomOtherVersions", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingCustomOtherVersions", "TreatyPricingCustomOtherId", "dbo.TreatyPricingCustomerOthers");
            DropForeignKey("dbo.TreatyPricingCustomOtherVersions", "PersonInChargeId", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingCustomOtherVersions", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingCustomOtherProducts", "TreatyPricingProductId", "dbo.TreatyPricingProducts");
            DropForeignKey("dbo.TreatyPricingCustomOtherProducts", "TreatyPricingCustomOtherId", "dbo.TreatyPricingCustomerOthers");
            DropForeignKey("dbo.TreatyPricingCustomerOthers", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingCustomerOthers", "TreatyPricingCedantId", "dbo.TreatyPricingCedants");
            DropForeignKey("dbo.TreatyPricingCustomerOthers", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingClaimApprovalLimitVersions", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingClaimApprovalLimitVersions", "TreatyPricingClaimApprovalLimitId", "dbo.TreatyPricingClaimApprovalLimits");
            DropForeignKey("dbo.TreatyPricingClaimApprovalLimitVersions", "PersonInChargeId", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingClaimApprovalLimitVersions", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingClaimApprovalLimits", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingClaimApprovalLimits", "TreatyPricingCedantId", "dbo.TreatyPricingCedants");
            DropForeignKey("dbo.TreatyPricingClaimApprovalLimits", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingCampaignVersions", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingCampaignVersions", "TreatyPricingUwQuestionnaireVersionId", "dbo.TreatyPricingUwQuestionnaireVersions");
            DropForeignKey("dbo.TreatyPricingUwQuestionnaireVersions", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingUwQuestionnaireVersions", "TreatyPricingUwQuestionnaireId", "dbo.TreatyPricingUwQuestionnaires");
            DropForeignKey("dbo.TreatyPricingUwQuestionnaireVersions", "PersonInChargeId", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingUwQuestionnaireVersions", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingCampaignVersions", "TreatyPricingUwQuestionnaireId", "dbo.TreatyPricingUwQuestionnaires");
            DropForeignKey("dbo.TreatyPricingUwQuestionnaires", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingUwQuestionnaires", "TreatyPricingCedantId", "dbo.TreatyPricingCedants");
            DropForeignKey("dbo.TreatyPricingUwQuestionnaires", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingCampaignVersions", "TreatyPricingProfitCommissionVersionId", "dbo.TreatyPricingProfitCommissionVersions");
            DropForeignKey("dbo.TreatyPricingProfitCommissionVersions", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingProfitCommissionVersions", "TreatyPricingProfitCommissionId", "dbo.TreatyPricingProfitCommissions");
            DropForeignKey("dbo.TreatyPricingProfitCommissionVersions", "PersonInChargeId", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingProfitCommissionVersions", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingCampaignVersions", "TreatyPricingProfitCommissionId", "dbo.TreatyPricingProfitCommissions");
            DropForeignKey("dbo.TreatyPricingProfitCommissions", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingProfitCommissions", "TreatyPricingCedantId", "dbo.TreatyPricingCedants");
            DropForeignKey("dbo.TreatyPricingProfitCommissions", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingCampaignVersions", "TreatyPricingMedicalTableVersionId", "dbo.TreatyPricingMedicalTableVersions");
            DropForeignKey("dbo.TreatyPricingMedicalTableVersions", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingMedicalTableVersions", "TreatyPricingMedicalTableId", "dbo.TreatyPricingMedicalTables");
            DropForeignKey("dbo.TreatyPricingMedicalTableVersions", "PersonInChargeId", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingMedicalTableVersions", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingCampaignVersions", "TreatyPricingMedicalTableId", "dbo.TreatyPricingMedicalTables");
            DropForeignKey("dbo.TreatyPricingMedicalTables", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingMedicalTables", "TreatyPricingCedantId", "dbo.TreatyPricingCedants");
            DropForeignKey("dbo.TreatyPricingMedicalTables", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingMedicalTables", "AgeDefinitionPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingCampaignVersions", "TreatyPricingFinancialTableVersionId", "dbo.TreatyPricingFinancialTableVersions");
            DropForeignKey("dbo.TreatyPricingFinancialTableVersions", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingFinancialTableVersions", "TreatyPricingFinancialTableId", "dbo.TreatyPricingFinancialTables");
            DropForeignKey("dbo.TreatyPricingFinancialTableVersions", "PersonInChargeId", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingFinancialTableVersions", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingCampaignVersions", "TreatyPricingFinancialTableId", "dbo.TreatyPricingFinancialTables");
            DropForeignKey("dbo.TreatyPricingFinancialTables", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingFinancialTables", "TreatyPricingCedantId", "dbo.TreatyPricingCedants");
            DropForeignKey("dbo.TreatyPricingFinancialTables", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingCampaignVersions", "TreatyPricingCampaignId", "dbo.TreatyPricingCampaigns");
            DropForeignKey("dbo.TreatyPricingCampaignVersions", "TreatyPricingAdvantageProgramVersionId", "dbo.TreatyPricingAdvantageProgramVersions");
            DropForeignKey("dbo.TreatyPricingCampaignVersions", "TreatyPricingAdvantageProgramId", "dbo.TreatyPricingAdvantagePrograms");
            DropForeignKey("dbo.TreatyPricingCampaignVersions", "ReinsRateTreatyPricingRateTableVersionId", "dbo.TreatyPricingRateTableVersions");
            DropForeignKey("dbo.TreatyPricingCampaignVersions", "ReinsRateTreatyPricingRateTableId", "dbo.TreatyPricingRateTables");
            DropForeignKey("dbo.TreatyPricingCampaignVersions", "ReinsDiscountTreatyPricingRateTableVersionId", "dbo.TreatyPricingRateTableVersions");
            DropForeignKey("dbo.TreatyPricingRateTableVersions", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingRateTableVersions", "TreatyPricingRateTableId", "dbo.TreatyPricingRateTables");
            DropForeignKey("dbo.TreatyPricingRateTableVersions", "RateGuaranteePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingRateTableVersions", "PersonInChargeId", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingRateTableVersions", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingRateTableVersions", "AgeBasisPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingCampaignVersions", "ReinsDiscountTreatyPricingRateTableId", "dbo.TreatyPricingRateTables");
            DropForeignKey("dbo.TreatyPricingRateTables", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingRateTables", "TreatyPricingRateTableGroupId", "dbo.TreatyPricingRateTableGroups");
            DropForeignKey("dbo.TreatyPricingRateTableGroups", "UploadedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingRateTableGroups", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingRateTableGroups", "TreatyPricingCedantId", "dbo.TreatyPricingCedants");
            DropForeignKey("dbo.TreatyPricingRateTableGroups", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingRateTables", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingRateTables", "BenefitId", "dbo.Benefits");
            DropForeignKey("dbo.TreatyPricingCampaignVersions", "PersonInChargeId", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingCampaignVersions", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingCampaignVersions", "AgeBasisPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingCampaignProducts", "TreatyPricingProductId", "dbo.TreatyPricingProducts");
            DropForeignKey("dbo.TreatyPricingProducts", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingProducts", "TreatyPricingCedantId", "dbo.TreatyPricingCedants");
            DropForeignKey("dbo.TreatyPricingProducts", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingCampaignProducts", "TreatyPricingCampaignId", "dbo.TreatyPricingCampaigns");
            DropForeignKey("dbo.TreatyPricingCampaigns", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingCampaigns", "TreatyPricingCedantId", "dbo.TreatyPricingCedants");
            DropForeignKey("dbo.TreatyPricingCampaigns", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingAdvantageProgramVersionBenefits", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingAdvantageProgramVersionBenefits", "TreatyPricingAdvantageProgramVersionId", "dbo.TreatyPricingAdvantageProgramVersions");
            DropForeignKey("dbo.TreatyPricingAdvantageProgramVersions", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingAdvantageProgramVersions", "TreatyPricingAdvantageProgramId", "dbo.TreatyPricingAdvantagePrograms");
            DropForeignKey("dbo.TreatyPricingAdvantageProgramVersions", "PersonInChargeId", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingAdvantageProgramVersions", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingAdvantageProgramVersionBenefits", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingAdvantageProgramVersionBenefits", "BenefitId", "dbo.Benefits");
            DropForeignKey("dbo.TreatyPricingAdvantagePrograms", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingAdvantagePrograms", "TreatyPricingCedantId", "dbo.TreatyPricingCedants");
            DropForeignKey("dbo.TreatyPricingCedants", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingCedants", "ReinsuranceTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyPricingCedants", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingCedants", "CedantId", "dbo.Cedants");
            DropForeignKey("dbo.TreatyPricingAdvantagePrograms", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TemplateDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TemplateDetails", "TemplateId", "dbo.Templates");
            DropForeignKey("dbo.Templates", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.Templates", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Templates", "CedantId", "dbo.Cedants");
            DropForeignKey("dbo.TemplateDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RetroTreatyDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RetroTreatyDetails", "TreatyDiscountTableId", "dbo.TreatyDiscountTables");
            DropForeignKey("dbo.RetroTreatyDetails", "RetroTreatyId", "dbo.RetroTreaties");
            DropForeignKey("dbo.RetroTreatyDetails", "PremiumSpreadTableId", "dbo.PremiumSpreadTables");
            DropForeignKey("dbo.RetroTreatyDetails", "PerLifeRetroConfigurationTreatyId", "dbo.PerLifeRetroConfigurationTreaties");
            DropForeignKey("dbo.RetroTreatyDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RetroTreaties", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RetroTreaties", "TreatyTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.RetroTreaties", "TreatyDiscountTableId", "dbo.TreatyDiscountTables");
            DropForeignKey("dbo.RetroTreaties", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RetroRegisterFiles", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RetroRegisterFiles", "RetroRegisterId", "dbo.RetroRegister");
            DropForeignKey("dbo.RetroRegisterFiles", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RetroBenefitRetentionLimitDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RetroBenefitRetentionLimitDetails", "RetroBenefitRetentionLimitId", "dbo.RetroBenefitRetentionLimits");
            DropForeignKey("dbo.RetroBenefitRetentionLimits", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RetroBenefitRetentionLimits", "RetroBenefitCodeId", "dbo.RetroBenefitCodes");
            DropForeignKey("dbo.RetroBenefitRetentionLimits", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RetroBenefitRetentionLimitDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RetroBenefitCodeMappingDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RetroBenefitCodeMappingDetails", "RetroBenefitCodeMappingId", "dbo.RetroBenefitCodeMappings");
            DropForeignKey("dbo.RetroBenefitCodeMappings", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RetroBenefitCodeMappings", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RetroBenefitCodeMappings", "BenefitId", "dbo.Benefits");
            DropForeignKey("dbo.RetroBenefitCodeMappingDetails", "RetroBenefitCodeId", "dbo.RetroBenefitCodes");
            DropForeignKey("dbo.RetroBenefitCodes", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RetroBenefitCodes", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RetroBenefitCodeMappingDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeRetroConfigurationTreaties", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeRetroConfigurationTreaties", "TreatyTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.PerLifeRetroConfigurationTreaties", "TreatyCodeId", "dbo.TreatyCodes");
            DropForeignKey("dbo.PerLifeRetroConfigurationTreaties", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeRetroConfigurationTreaties", "BusinessTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.PerLifeRetroConfigurationRatios", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeRetroConfigurationRatios", "TreatyCodeId", "dbo.TreatyCodes");
            DropForeignKey("dbo.PerLifeRetroConfigurationRatios", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeDuplicationCheckDetails", "PerLifeDuplicationCheckId", "dbo.PerLifeDuplicationChecks");
            DropForeignKey("dbo.PerLifeDuplicationChecks", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeDuplicationChecks", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeDuplicationCheckDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeDataCorrections", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeDataCorrections", "TreatyCodeId", "dbo.TreatyCodes");
            DropForeignKey("dbo.PerLifeDataCorrections", "TerritoryOfIssueCodePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.PerLifeDataCorrections", "PerLifeRetroGenderId", "dbo.PerLifeRetroGenders");
            DropForeignKey("dbo.PerLifeRetroGenders", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeRetroGenders", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeDataCorrections", "PerLifeRetroCountryId", "dbo.PerLifeRetroCountries");
            DropForeignKey("dbo.PerLifeRetroCountries", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeRetroCountries", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeDataCorrections", "InsuredGenderCodePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.PerLifeDataCorrections", "ExceptionStatusPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.PerLifeDataCorrections", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeAggregationDuplicationListings", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeAggregationDuplicationListings", "TreatyCodeId", "dbo.TreatyCodes");
            DropForeignKey("dbo.PerLifeAggregationDuplicationListings", "TransactionTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.PerLifeAggregationDuplicationListings", "ReinsBasisCodePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.PerLifeAggregationDuplicationListings", "MLReBenefitCodeId", "dbo.Benefits");
            DropForeignKey("dbo.PerLifeAggregationDuplicationListings", "InsuredGenderCodePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.PerLifeAggregationDuplicationListings", "FundsAccountingTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.PerLifeAggregationDuplicationListings", "ExceptionStatusPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.PerLifeAggregationDuplicationListings", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeAggregationDetailData", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeAggregationDetailData", "RiDataWarehouseHistoryId", "dbo.RiDataWarehouseHistories");
            DropForeignKey("dbo.PerLifeAggregationDetailData", "PerLifeAggregationDetailTreatyId", "dbo.PerLifeAggregationDetailTreaties");
            DropForeignKey("dbo.PerLifeAggregationDetailTreaties", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeAggregationDetailTreaties", "PerLifeAggregationDetailId", "dbo.PerLifeAggregationDetails");
            DropForeignKey("dbo.PerLifeAggregationDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeAggregationDetails", "PerLifeAggregationId", "dbo.PerLifeAggregations");
            DropForeignKey("dbo.PerLifeAggregations", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeAggregations", "FundsAccountingTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.PerLifeAggregations", "CutOffId", "dbo.CutOff");
            DropForeignKey("dbo.PerLifeAggregations", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeAggregationDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeAggregationDetailTreaties", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeAggregationDetailData", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeAggregationConflictListings", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeAggregationConflictListings", "TreatyCodeId", "dbo.TreatyCodes");
            DropForeignKey("dbo.PerLifeAggregationConflictListings", "TerritoryOfIssueCodePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.PerLifeAggregationConflictListings", "RetroPremiumFrequencyModePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.PerLifeAggregationConflictListings", "PremiumFrequencyModePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.PerLifeAggregationConflictListings", "MLReBenefitCodeId", "dbo.Benefits");
            DropForeignKey("dbo.PerLifeAggregationConflictListings", "InsuredGenderCodePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.PerLifeAggregationConflictListings", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.InsuredGroupNames", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.InsuredGroupNames", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.HipsCategoryDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.HipsCategoryDetails", "HipsCategoryId", "dbo.HipsCategories");
            DropForeignKey("dbo.HipsCategoryDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.HipsCategories", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.HipsCategories", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.GtlBenefitCategories", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.GtlBenefitCategories", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.GstMaintenances", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.GstMaintenances", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Remarks", "StatusHistoryId", "dbo.StatusHistories");
            DropForeignKey("dbo.Designations", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.Designations", "CreatedById", "dbo.Users");
            DropIndex("dbo.ValidDuplicationLists", new[] { "UpdatedById" });
            DropIndex("dbo.ValidDuplicationLists", new[] { "CreatedById" });
            DropIndex("dbo.ValidDuplicationLists", new[] { "TransactionTypePickListDetailId" });
            DropIndex("dbo.ValidDuplicationLists", new[] { "ReinsBasisCodePickListDetailId" });
            DropIndex("dbo.ValidDuplicationLists", new[] { "FundsAccountingTypePickListDetailId" });
            DropIndex("dbo.ValidDuplicationLists", new[] { "ReinsuranceEffectiveDate" });
            DropIndex("dbo.ValidDuplicationLists", new[] { "CedingBenefitTypeCode" });
            DropIndex("dbo.ValidDuplicationLists", new[] { "CedingBenefitRiskCode" });
            DropIndex("dbo.ValidDuplicationLists", new[] { "MLReBenefitCodeId" });
            DropIndex("dbo.ValidDuplicationLists", new[] { "InsuredGenderCodePickListDetailId" });
            DropIndex("dbo.ValidDuplicationLists", new[] { "PolicyNumber" });
            DropIndex("dbo.ValidDuplicationLists", new[] { "InsuredDateOfBirth" });
            DropIndex("dbo.ValidDuplicationLists", new[] { "InsuredName" });
            DropIndex("dbo.ValidDuplicationLists", new[] { "CedantPlanCode" });
            DropIndex("dbo.ValidDuplicationLists", new[] { "TreatyCodeId" });
            DropIndex("dbo.TreatyPricingWorkflowObjects", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingWorkflowObjects", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingWorkflowObjects", new[] { "ObjectVersionId" });
            DropIndex("dbo.TreatyPricingWorkflowObjects", new[] { "ObjectId" });
            DropIndex("dbo.TreatyPricingWorkflowObjects", new[] { "ObjectType" });
            DropIndex("dbo.TreatyPricingWorkflowObjects", new[] { "WorkflowId" });
            DropIndex("dbo.TreatyPricingWorkflowObjects", new[] { "Type" });
            DropIndex("dbo.TreatyPricingUwQuestionnaireVersionFiles", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingUwQuestionnaireVersionFiles", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingUwQuestionnaireVersionFiles", new[] { "Status" });
            DropIndex("dbo.TreatyPricingUwQuestionnaireVersionFiles", new[] { "HashFileName" });
            DropIndex("dbo.TreatyPricingUwQuestionnaireVersionFiles", new[] { "FileName" });
            DropIndex("dbo.TreatyPricingUwQuestionnaireVersionFiles", new[] { "TreatyPricingUwQuestionnaireVersionId" });
            DropIndex("dbo.UwQuestionnaireCategories", new[] { "UpdatedById" });
            DropIndex("dbo.UwQuestionnaireCategories", new[] { "CreatedById" });
            DropIndex("dbo.UwQuestionnaireCategories", new[] { "Name" });
            DropIndex("dbo.UwQuestionnaireCategories", new[] { "Code" });
            DropIndex("dbo.TreatyPricingUwQuestionnaireVersionDetails", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingUwQuestionnaireVersionDetails", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingUwQuestionnaireVersionDetails", new[] { "UwQuestionnaireCategoryId" });
            DropIndex("dbo.TreatyPricingUwQuestionnaireVersionDetails", new[] { "TreatyPricingUwQuestionnaireVersionId" });
            DropIndex("dbo.TreatyPricingTreatyWorkflowVersions", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingTreatyWorkflowVersions", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingTreatyWorkflowVersions", new[] { "PersonInChargeId" });
            DropIndex("dbo.TreatyPricingTreatyWorkflowVersions", new[] { "Version" });
            DropIndex("dbo.TreatyPricingTreatyWorkflowVersions", new[] { "TreatyPricingTreatyWorkflowId" });
            DropIndex("dbo.TreatyPricingTreatyWorkflows", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingTreatyWorkflows", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingTreatyWorkflows", new[] { "DraftingStatusCategory" });
            DropIndex("dbo.TreatyPricingTreatyWorkflows", new[] { "DraftingStatus" });
            DropIndex("dbo.TreatyPricingTreatyWorkflows", new[] { "DocumentStatus" });
            DropIndex("dbo.TreatyPricingTreatyWorkflows", new[] { "CoverageStatus" });
            DropIndex("dbo.TreatyPricingTreatyWorkflows", new[] { "TreatyCode" });
            DropIndex("dbo.TreatyPricingTreatyWorkflows", new[] { "DocumentId" });
            DropIndex("dbo.TreatyPricingTreatyWorkflows", new[] { "BusinessTypePickListDetailId" });
            DropIndex("dbo.TreatyPricingTreatyWorkflows", new[] { "BusinessOriginPickListDetailId" });
            DropIndex("dbo.TreatyPricingTreatyWorkflows", new[] { "InwardRetroPartyDetailId" });
            DropIndex("dbo.TreatyPricingTreatyWorkflows", new[] { "CounterPartyDetailId" });
            DropIndex("dbo.TreatyPricingTreatyWorkflows", new[] { "ReinsuranceTypePickListDetailId" });
            DropIndex("dbo.TreatyPricingTreatyWorkflows", new[] { "DocumentType" });
            DropIndex("dbo.TreatyPricingTierProfitCommissions", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingTierProfitCommissions", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingTierProfitCommissions", new[] { "TreatyPricingProfitCommissionVersionId" });
            DropIndex("dbo.TreatyPricingReportGenerations", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingReportGenerations", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingReportGenerations", new[] { "Status" });
            DropIndex("dbo.TreatyPricingReportGenerations", new[] { "HashFileName" });
            DropIndex("dbo.TreatyPricingReportGenerations", new[] { "FileName" });
            DropIndex("dbo.TreatyPricingReportGenerations", new[] { "ReportName" });
            DropIndex("dbo.TreatyPricingRateTableRates", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingRateTableRates", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingRateTableRates", new[] { "OccupationClass" });
            DropIndex("dbo.TreatyPricingRateTableRates", new[] { "UnitRate" });
            DropIndex("dbo.TreatyPricingRateTableRates", new[] { "Unisex" });
            DropIndex("dbo.TreatyPricingRateTableRates", new[] { "Female" });
            DropIndex("dbo.TreatyPricingRateTableRates", new[] { "Male" });
            DropIndex("dbo.TreatyPricingRateTableRates", new[] { "FemaleSmoker" });
            DropIndex("dbo.TreatyPricingRateTableRates", new[] { "FemaleNonSmoker" });
            DropIndex("dbo.TreatyPricingRateTableRates", new[] { "MaleSmoker" });
            DropIndex("dbo.TreatyPricingRateTableRates", new[] { "MaleNonSmoker" });
            DropIndex("dbo.TreatyPricingRateTableRates", new[] { "Age" });
            DropIndex("dbo.TreatyPricingRateTableRates", new[] { "TreatyPricingRateTableVersionId" });
            DropIndex("dbo.TreatyPricingRateTableDetails", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingRateTableDetails", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingRateTableDetails", new[] { "Type" });
            DropIndex("dbo.TreatyPricingRateTableDetails", new[] { "TreatyPricingRateTableVersionId" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersions", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersions", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersions", new[] { "ROE" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersions", new[] { "PVProfit" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersions", new[] { "FirstYearPremium" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersions", new[] { "ProfitMargin" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersions", new[] { "PricingCompletedDate" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersions", new[] { "RevisedPricingDueDate" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersions", new[] { "TargetPricingDueDate" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersions", new[] { "RequestDate" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersions", new[] { "PendingOnDate" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersions", new[] { "PersonInChargePricingAuthorityReviewerId" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersions", new[] { "PersonInChargePeerReviewerId" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersions", new[] { "PersonInChargeTechReviewerId" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersions", new[] { "PersonInChargeId" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersions", new[] { "ChecklistFinalised" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersions", new[] { "FinalRateTableHashFileName" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersions", new[] { "FinalRateTableFileName" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersions", new[] { "FinalQuoteSpecHashFileName" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersions", new[] { "FinalQuoteSpecFileName" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersions", new[] { "QuoteValidityDay" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersions", new[] { "BDPersonInChargeId" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersions", new[] { "Version" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersions", new[] { "TreatyPricingQuotationWorkflowId" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersionQuotationChecklists", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersionQuotationChecklists", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersionQuotationChecklists", new[] { "Status" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersionQuotationChecklists", new[] { "InternalTeamPersonInCharge" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersionQuotationChecklists", new[] { "InternalTeam" });
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersionQuotationChecklists", new[] { "TreatyPricingQuotationWorkflowVersionId" });
            DropIndex("dbo.TreatyPricingQuotationWorkflows", new[] { "PricingDueDate" });
            DropIndex("dbo.TreatyPricingQuotationWorkflows", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingQuotationWorkflows", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingQuotationWorkflows", new[] { "Description" });
            DropIndex("dbo.TreatyPricingQuotationWorkflows", new[] { "FinaliseDate" });
            DropIndex("dbo.TreatyPricingQuotationWorkflows", new[] { "TargetRateCompletionDate" });
            DropIndex("dbo.TreatyPricingQuotationWorkflows", new[] { "TargetClientReleaseDate" });
            DropIndex("dbo.TreatyPricingQuotationWorkflows", new[] { "PricingStatus" });
            DropIndex("dbo.TreatyPricingQuotationWorkflows", new[] { "PricingTeamPickListDetailId" });
            DropIndex("dbo.TreatyPricingQuotationWorkflows", new[] { "LatestRevisionDate" });
            DropIndex("dbo.TreatyPricingQuotationWorkflows", new[] { "TargetSendDate" });
            DropIndex("dbo.TreatyPricingQuotationWorkflows", new[] { "Status" });
            DropIndex("dbo.TreatyPricingQuotationWorkflows", new[] { "Name" });
            DropIndex("dbo.TreatyPricingQuotationWorkflows", new[] { "ReinsuranceTypePickListDetailId" });
            DropIndex("dbo.TreatyPricingQuotationWorkflows", new[] { "CedantId" });
            DropIndex("dbo.TreatyPricingQuotationWorkflows", new[] { "QuotationId" });
            DropIndex("dbo.TreatyPricingProfitCommissionDetails", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingProfitCommissionDetails", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingProfitCommissionDetails", new[] { "Item" });
            DropIndex("dbo.TreatyPricingProfitCommissionDetails", new[] { "SortIndex" });
            DropIndex("dbo.TreatyPricingProfitCommissionDetails", new[] { "TreatyPricingProfitCommissionVersionId" });
            DropIndex("dbo.TreatyPricingProductPerLifeRetros", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingProductPerLifeRetros", new[] { "TreatyPricingPerLifeRetroId" });
            DropIndex("dbo.TreatyPricingProductPerLifeRetros", new[] { "TreatyPricingProductId" });
            DropIndex("dbo.TreatyPricingProductDetails", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingProductDetails", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingProductDetails", new[] { "Type" });
            DropIndex("dbo.TreatyPricingProductDetails", new[] { "TreatyPricingProductVersionId" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "MlreServiceFee" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "WakalahFee" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "AdditionalLoading" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "AdditionalDiscount" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "RetrocessionDiscount" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "RenewalBusinessRateGuarantee" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "NewBusinessRateGuarantee" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "RetrocessionRateTable" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "IsRetrocessionAdvantageProgram" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "IsRetrocessionProfitCommission" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "MlreShare" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "InwardRetroRetention" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "InwardRetroArrangementReinsuranceTypePickListDetailId" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "InwardRetroParty" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "RequestedCoinsuranceRiDiscount" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "RequestedReinsuranceDiscount" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "RequestedRateGuarantee" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "TreatyPricingRateTableVersionId" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "TreatyPricingRateTableId" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "IsAdvantageProgram" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "IsProfitCommission" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "PricingUploadHashFileName" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "PricingUploadFileName" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "RiskPatternSumPickListDetailId" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "CoinsuranceReinsuranceShare" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "CoinsuranceCedantRetention" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "ReinsuranceShare" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "CedantRetention" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "BenefitPayout" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "PricingArrangementReinsuranceTypePickListDetailId" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "PreExistingCondition" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "RefundOfPremium" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "IfOthers2" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "TreatyPricingDefinitionAndExclusionVersionId" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "TreatyPricingDefinitionAndExclusionId" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "IfOthers1" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "TreatyPricingClaimApprovalLimitVersionId" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "TreatyPricingClaimApprovalLimitId" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "Others" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "TreatyPricingUwLimitVersionId" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "TreatyPricingUwLimitId" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "MaximumSumAssured" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "MinimumSumAssured" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "PremiumPayingTerm" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "MaximumPolicyTerm" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "MinimumPolicyTerm" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "MaximumExpiryAge" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "MaximumEntryAge" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "MinimumEntryAge" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "AgeBasisPickListDetailId" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "RiderAttachmentRatio" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "PayoutTypePickListDetailId" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "BasicRiderPickListDetailId" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "Name" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "BenefitId" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "TreatyPricingProductVersionId" });
            DropIndex("dbo.TreatyPricingProductBenefitDirectRetros", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingProductBenefitDirectRetros", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingProductBenefitDirectRetros", new[] { "AdditionalLoading" });
            DropIndex("dbo.TreatyPricingProductBenefitDirectRetros", new[] { "AdditionalDiscount" });
            DropIndex("dbo.TreatyPricingProductBenefitDirectRetros", new[] { "RetrocessionDiscount" });
            DropIndex("dbo.TreatyPricingProductBenefitDirectRetros", new[] { "RenewalBusinessRateGuarantee" });
            DropIndex("dbo.TreatyPricingProductBenefitDirectRetros", new[] { "NewBusinessRateGuarantee" });
            DropIndex("dbo.TreatyPricingProductBenefitDirectRetros", new[] { "RetrocessionRateTable" });
            DropIndex("dbo.TreatyPricingProductBenefitDirectRetros", new[] { "IsRetrocessionAdvantageProgram" });
            DropIndex("dbo.TreatyPricingProductBenefitDirectRetros", new[] { "IsRetrocessionProfitCommission" });
            DropIndex("dbo.TreatyPricingProductBenefitDirectRetros", new[] { "RetrocessionShare" });
            DropIndex("dbo.TreatyPricingProductBenefitDirectRetros", new[] { "MlreRetention" });
            DropIndex("dbo.TreatyPricingProductBenefitDirectRetros", new[] { "ArrangementRetrocessionTypePickListDetailId" });
            DropIndex("dbo.TreatyPricingProductBenefitDirectRetros", new[] { "RetroPartyId" });
            DropIndex("dbo.TreatyPricingProductBenefitDirectRetros", new[] { "TreatyPricingProductBenefitId" });
            DropIndex("dbo.TreatyPricingPickListDetails", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingPickListDetails", new[] { "ObjectId" });
            DropIndex("dbo.TreatyPricingPickListDetails", new[] { "ObjectType" });
            DropIndex("dbo.TreatyPricingPickListDetails", new[] { "PickListId" });
            DropIndex("dbo.TreatyPricingPerLifeRetroVersionTiers", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingPerLifeRetroVersionTiers", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingPerLifeRetroVersionTiers", new[] { "TreatyPricingPerLifeRetroVersionId" });
            DropIndex("dbo.TreatyPricingPerLifeRetroVersionDetails", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingPerLifeRetroVersionDetails", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingPerLifeRetroVersionDetails", new[] { "Item" });
            DropIndex("dbo.TreatyPricingPerLifeRetroVersionDetails", new[] { "SortIndex" });
            DropIndex("dbo.TreatyPricingPerLifeRetroVersionDetails", new[] { "TreatyPricingPerLifeRetroVersionId" });
            DropIndex("dbo.TreatyPricingPerLifeRetroVersions", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingPerLifeRetroVersions", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingPerLifeRetroVersions", new[] { "PaymentRetrocessionairePremiumPickListDetailId" });
            DropIndex("dbo.TreatyPricingPerLifeRetroVersions", new[] { "RetrocessionaireRetroPartyId" });
            DropIndex("dbo.TreatyPricingPerLifeRetroVersions", new[] { "PersonInChargeId" });
            DropIndex("dbo.TreatyPricingPerLifeRetroVersions", new[] { "Version" });
            DropIndex("dbo.TreatyPricingPerLifeRetroVersions", new[] { "TreatyPricingPerLifeRetroId" });
            DropIndex("dbo.TreatyPricingPerLifeRetroVersionBenefits", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingPerLifeRetroVersionBenefits", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingPerLifeRetroVersionBenefits", new[] { "IsProfitCommission" });
            DropIndex("dbo.TreatyPricingPerLifeRetroVersionBenefits", new[] { "AgeBasisPickListDetailId" });
            DropIndex("dbo.TreatyPricingPerLifeRetroVersionBenefits", new[] { "ArrangementRetrocessionnaireTypePickListDetailId" });
            DropIndex("dbo.TreatyPricingPerLifeRetroVersionBenefits", new[] { "BenefitId" });
            DropIndex("dbo.TreatyPricingPerLifeRetroVersionBenefits", new[] { "TreatyPricingPerLifeRetroVersionId" });
            DropIndex("dbo.TreatyPricingPerLifeRetroProducts", new[] { "TreatyPricingProductId" });
            DropIndex("dbo.TreatyPricingPerLifeRetroProducts", new[] { "TreatyPricingPerLifeRetroId" });
            DropIndex("dbo.TreatyPricingPerLifeRetro", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingPerLifeRetro", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingPerLifeRetro", new[] { "Type" });
            DropIndex("dbo.TreatyPricingPerLifeRetro", new[] { "RetroPartyId" });
            DropIndex("dbo.TreatyPricingPerLifeRetro", new[] { "Code" });
            DropIndex("dbo.TreatyPricingMedicalTableVersionFiles", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingMedicalTableVersionFiles", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingMedicalTableVersionFiles", new[] { "Status" });
            DropIndex("dbo.TreatyPricingMedicalTableVersionFiles", new[] { "HashFileName" });
            DropIndex("dbo.TreatyPricingMedicalTableVersionFiles", new[] { "FileName" });
            DropIndex("dbo.TreatyPricingMedicalTableVersionFiles", new[] { "Description" });
            DropIndex("dbo.TreatyPricingMedicalTableVersionFiles", new[] { "DistributionTierPickListDetailId" });
            DropIndex("dbo.TreatyPricingMedicalTableVersionFiles", new[] { "TreatyPricingMedicalTableVersionId" });
            DropIndex("dbo.TreatyPricingMedicalTableUploadLegends", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingMedicalTableUploadLegends", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingMedicalTableUploadLegends", new[] { "Description" });
            DropIndex("dbo.TreatyPricingMedicalTableUploadLegends", new[] { "Code" });
            DropIndex("dbo.TreatyPricingMedicalTableUploadLegends", new[] { "TreatyPricingMedicalTableVersionDetailId" });
            DropIndex("dbo.TreatyPricingMedicalTableUploadRows", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingMedicalTableUploadRows", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingMedicalTableUploadRows", new[] { "MaximumSumAssured" });
            DropIndex("dbo.TreatyPricingMedicalTableUploadRows", new[] { "MinimumSumAssured" });
            DropIndex("dbo.TreatyPricingMedicalTableUploadRows", new[] { "TreatyPricingMedicalTableVersionDetailId" });
            DropIndex("dbo.TreatyPricingMedicalTableVersionDetails", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingMedicalTableVersionDetails", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingMedicalTableVersionDetails", new[] { "Description" });
            DropIndex("dbo.TreatyPricingMedicalTableVersionDetails", new[] { "DistributionTierPickListDetailId" });
            DropIndex("dbo.TreatyPricingMedicalTableVersionDetails", new[] { "TreatyPricingMedicalTableVersionId" });
            DropIndex("dbo.TreatyPricingMedicalTableUploadColumns", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingMedicalTableUploadColumns", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingMedicalTableUploadColumns", new[] { "MaximumAge" });
            DropIndex("dbo.TreatyPricingMedicalTableUploadColumns", new[] { "MinimumAge" });
            DropIndex("dbo.TreatyPricingMedicalTableUploadColumns", new[] { "TreatyPricingMedicalTableVersionDetailId" });
            DropIndex("dbo.TreatyPricingMedicalTableUploadCells", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingMedicalTableUploadCells", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingMedicalTableUploadCells", new[] { "Code" });
            DropIndex("dbo.TreatyPricingMedicalTableUploadCells", new[] { "TreatyPricingMedicalTableUploadRowId" });
            DropIndex("dbo.TreatyPricingMedicalTableUploadCells", new[] { "TreatyPricingMedicalTableUploadColumnId" });
            DropIndex("dbo.TreatyPricingUwLimitVersions", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingUwLimitVersions", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingUwLimitVersions", new[] { "Remarks4" });
            DropIndex("dbo.TreatyPricingUwLimitVersions", new[] { "IssuePayoutLimit" });
            DropIndex("dbo.TreatyPricingUwLimitVersions", new[] { "PerLifePerIndustry" });
            DropIndex("dbo.TreatyPricingUwLimitVersions", new[] { "MaxSumAssured" });
            DropIndex("dbo.TreatyPricingUwLimitVersions", new[] { "Remarks3" });
            DropIndex("dbo.TreatyPricingUwLimitVersions", new[] { "AblMaxUwRating" });
            DropIndex("dbo.TreatyPricingUwLimitVersions", new[] { "Remarks2" });
            DropIndex("dbo.TreatyPricingUwLimitVersions", new[] { "AblSumAssured" });
            DropIndex("dbo.TreatyPricingUwLimitVersions", new[] { "Remarks1" });
            DropIndex("dbo.TreatyPricingUwLimitVersions", new[] { "UwLimit" });
            DropIndex("dbo.TreatyPricingUwLimitVersions", new[] { "CurrencyCode" });
            DropIndex("dbo.TreatyPricingUwLimitVersions", new[] { "EffectiveAt" });
            DropIndex("dbo.TreatyPricingUwLimitVersions", new[] { "PersonInChargeId" });
            DropIndex("dbo.TreatyPricingUwLimitVersions", new[] { "Version" });
            DropIndex("dbo.TreatyPricingUwLimitVersions", new[] { "TreatyPricingUwLimitId" });
            DropIndex("dbo.TreatyPricingUwLimits", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingUwLimits", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingUwLimits", new[] { "Status" });
            DropIndex("dbo.TreatyPricingUwLimits", new[] { "Description" });
            DropIndex("dbo.TreatyPricingUwLimits", new[] { "Name" });
            DropIndex("dbo.TreatyPricingUwLimits", new[] { "LimitId" });
            DropIndex("dbo.TreatyPricingUwLimits", new[] { "TreatyPricingCedantId" });
            DropIndex("dbo.TreatyPricingGroupReferralVersionBenefits", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingGroupReferralVersionBenefits", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingGroupReferralVersionBenefits", new[] { "GroupProfitCommissionId" });
            DropIndex("dbo.TreatyPricingGroupReferralVersionBenefits", new[] { "OtherSpecialReinsuranceArrangement" });
            DropIndex("dbo.TreatyPricingGroupReferralVersionBenefits", new[] { "TreatyPricingUwLimitVersionId" });
            DropIndex("dbo.TreatyPricingGroupReferralVersionBenefits", new[] { "TreatyPricingUwLimitId" });
            DropIndex("dbo.TreatyPricingGroupReferralVersionBenefits", new[] { "AgeBasisPickListDetailId" });
            DropIndex("dbo.TreatyPricingGroupReferralVersionBenefits", new[] { "ReinsuranceArrangementPickListDetailId" });
            DropIndex("dbo.TreatyPricingGroupReferralVersionBenefits", new[] { "BenefitId" });
            DropIndex("dbo.TreatyPricingGroupReferralVersionBenefits", new[] { "TreatyPricingGroupReferralVersionId" });
            DropIndex("dbo.TreatyPricingGroupReferralHipsTables", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingGroupReferralHipsTables", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingGroupReferralHipsTables", new[] { "HipsSubCategoryId" });
            DropIndex("dbo.TreatyPricingGroupReferralHipsTables", new[] { "HipsCategoryId" });
            DropIndex("dbo.TreatyPricingGroupReferralHipsTables", new[] { "TreatyPricingGroupReferralFileId" });
            DropIndex("dbo.TreatyPricingGroupReferralHipsTables", new[] { "TreatyPricingGroupReferralId" });
            DropIndex("dbo.TreatyPricingGroupReferralGtlTables", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingGroupReferralGtlTables", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingGroupReferralGtlTables", new[] { "DesignationId" });
            DropIndex("dbo.TreatyPricingGroupReferralGtlTables", new[] { "GtlBenefitCategoryId" });
            DropIndex("dbo.TreatyPricingGroupReferralGtlTables", new[] { "Type" });
            DropIndex("dbo.TreatyPricingGroupReferralGtlTables", new[] { "TreatyPricingGroupReferralFileId" });
            DropIndex("dbo.TreatyPricingGroupReferralGtlTables", new[] { "TreatyPricingGroupReferralId" });
            DropIndex("dbo.TreatyPricingGroupReferralGhsTables", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingGroupReferralGhsTables", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingGroupReferralGhsTables", new[] { "TreatyPricingGroupReferralFileId" });
            DropIndex("dbo.TreatyPricingGroupReferralGhsTables", new[] { "TreatyPricingGroupReferralId" });
            DropIndex("dbo.TreatyPricingGroupReferralFiles", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingGroupReferralFiles", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingGroupReferralFiles", new[] { "Status" });
            DropIndex("dbo.TreatyPricingGroupReferralFiles", new[] { "UploadedType" });
            DropIndex("dbo.TreatyPricingGroupReferralFiles", new[] { "HashFileName" });
            DropIndex("dbo.TreatyPricingGroupReferralFiles", new[] { "FileName" });
            DropIndex("dbo.TreatyPricingGroupReferralFiles", new[] { "TableTypePickListDetailId" });
            DropIndex("dbo.TreatyPricingGroupReferralFiles", new[] { "TreatyPricingGroupReferralId" });
            DropIndex("dbo.TreatyPricingGroupReferralChecklists", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingGroupReferralChecklists", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingGroupReferralChecklists", new[] { "Status" });
            DropIndex("dbo.TreatyPricingGroupReferralChecklists", new[] { "InternalTeamPersonInCharge" });
            DropIndex("dbo.TreatyPricingGroupReferralChecklists", new[] { "InternalTeam" });
            DropIndex("dbo.TreatyPricingGroupReferralChecklists", new[] { "TreatyPricingGroupReferralVersionId" });
            DropIndex("dbo.TreatyPricingGroupReferralChecklistDetails", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingGroupReferralChecklistDetails", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingGroupReferralChecklistDetails", new[] { "InternalItem" });
            DropIndex("dbo.TreatyPricingGroupReferralChecklistDetails", new[] { "TreatyPricingGroupReferralVersionId" });
            DropIndex("dbo.TreatyPricingGroupReferralVersions", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingGroupReferralVersions", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingGroupReferralVersions", new[] { "HasPerLifeRetro" });
            DropIndex("dbo.TreatyPricingGroupReferralVersions", new[] { "UnderwritingMethodPickListDetailId" });
            DropIndex("dbo.TreatyPricingGroupReferralVersions", new[] { "IsCompulsoryOrVoluntary" });
            DropIndex("dbo.TreatyPricingGroupReferralVersions", new[] { "PremiumTypePickListDetailId" });
            DropIndex("dbo.TreatyPricingGroupReferralVersions", new[] { "RequestTypePickListDetailId" });
            DropIndex("dbo.TreatyPricingGroupReferralVersions", new[] { "GroupReferralPersonInChargeId" });
            DropIndex("dbo.TreatyPricingGroupReferralVersions", new[] { "Version" });
            DropIndex("dbo.TreatyPricingGroupReferralVersions", new[] { "TreatyPricingGroupReferralId" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "RetakafulModel" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "InvestmentProfitSharing" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "IsRetakafulService" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "InwardRetroQuarterlyRiskPremium" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "InwardRetroRecaptureClause" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "InwardRetroTerminationClause" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "InwardRetroProfitCommission" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "IsInwardRetro" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "DirectRetroQuarterlyRiskPremium" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "DirectRetroRecaptureClause" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "DirectRetroTerminationClause" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "DirectRetroProfitCommission" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "IsDirectRetro" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "GroupFreeCoverLimitAgeCi" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "GroupFreeCoverLimitAgeNonCi" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "QuarterlyRiskPremium" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "ResidenceCountry" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "RecaptureClause" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "TerminationClause" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "UnearnedPremiumRefundPickListDetailId" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "ReinsurancePremiumPaymentPickListDetailId" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "TreatyPricingProfitCommissionVersionId" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "TreatyPricingProfitCommissionId" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "SurvivalPeriod" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "WaitingPeriod" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "JumboLimit" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "ExpectedPolicyNo" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "TreatyPricingAdvantageProgramVersionId" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "TreatyPricingAdvantageProgramId" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "TreatyPricingUwQuestionnaireVersionId" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "TreatyPricingUwQuestionnaireId" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "TreatyPricingFinancialTableVersionId" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "TreatyPricingFinancialTableId" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "TreatyPricingMedicalTableVersionId" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "TreatyPricingMedicalTableId" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "ExpectedRiPremium" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "ExpectedAverageSumAssured" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "ReinsuranceArrangementPickListDetailId" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "BusinessTypePickListDetailId" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "BusinessOriginPickListDetailId" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "ProductTypePickListDetailId" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "PersonInChargeId" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "Version" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "TreatyPricingProductId" });
            DropIndex("dbo.TreatyPricingGroupReferrals", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingGroupReferrals", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingGroupReferrals", new[] { "TreatyPricingGroupMasterLetterId" });
            DropIndex("dbo.TreatyPricingGroupReferrals", new[] { "ReplyTemplateId" });
            DropIndex("dbo.TreatyPricingGroupReferrals", new[] { "ReplyVersionId" });
            DropIndex("dbo.TreatyPricingGroupReferrals", new[] { "RiGroupSlipTemplateId" });
            DropIndex("dbo.TreatyPricingGroupReferrals", new[] { "RiGroupSlipVersionId" });
            DropIndex("dbo.TreatyPricingGroupReferrals", new[] { "RiGroupSlipPersonInChargeId" });
            DropIndex("dbo.TreatyPricingGroupReferrals", new[] { "RiGroupSlipCode" });
            DropIndex("dbo.TreatyPricingGroupReferrals", new[] { "ReferredTypePickListDetailId" });
            DropIndex("dbo.TreatyPricingGroupReferrals", new[] { "IndustryNamePickListDetailId" });
            DropIndex("dbo.TreatyPricingGroupReferrals", new[] { "SecondaryTreatyPricingProductVersionId" });
            DropIndex("dbo.TreatyPricingGroupReferrals", new[] { "SecondaryTreatyPricingProductId" });
            DropIndex("dbo.TreatyPricingGroupReferrals", new[] { "PrimaryTreatyPricingProductVersionId" });
            DropIndex("dbo.TreatyPricingGroupReferrals", new[] { "PrimaryTreatyPricingProductId" });
            DropIndex("dbo.TreatyPricingGroupReferrals", new[] { "Status" });
            DropIndex("dbo.TreatyPricingGroupReferrals", new[] { "InsuredGroupNameId" });
            DropIndex("dbo.TreatyPricingGroupReferrals", new[] { "RiArrangementPickListDetailId" });
            DropIndex("dbo.TreatyPricingGroupReferrals", new[] { "Code" });
            DropIndex("dbo.TreatyPricingGroupReferrals", new[] { "CedantId" });
            DropIndex("dbo.TreatyPricingGroupMasterLetters", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingGroupMasterLetters", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingGroupMasterLetters", new[] { "Code" });
            DropIndex("dbo.TreatyPricingGroupMasterLetters", new[] { "CedantId" });
            DropIndex("dbo.TreatyPricingGroupMasterLetterGroupReferrals", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingGroupMasterLetterGroupReferrals", new[] { "TreatyPricingGroupReferralId" });
            DropIndex("dbo.TreatyPricingGroupMasterLetterGroupReferrals", new[] { "TreatyPricingGroupMasterLetterId" });
            DropIndex("dbo.TreatyPricingFinancialTableVersionFiles", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingFinancialTableVersionFiles", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingFinancialTableVersionFiles", new[] { "Status" });
            DropIndex("dbo.TreatyPricingFinancialTableVersionFiles", new[] { "HashFileName" });
            DropIndex("dbo.TreatyPricingFinancialTableVersionFiles", new[] { "FileName" });
            DropIndex("dbo.TreatyPricingFinancialTableVersionFiles", new[] { "Description" });
            DropIndex("dbo.TreatyPricingFinancialTableVersionFiles", new[] { "DistributionTierPickListDetailId" });
            DropIndex("dbo.TreatyPricingFinancialTableVersionFiles", new[] { "TreatyPricingFinancialTableVersionId" });
            DropIndex("dbo.TreatyPricingFinancialTableUploads", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingFinancialTableUploads", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingFinancialTableUploads", new[] { "Code" });
            DropIndex("dbo.TreatyPricingFinancialTableUploads", new[] { "MaximumSumAssured" });
            DropIndex("dbo.TreatyPricingFinancialTableUploads", new[] { "MinimumSumAssured" });
            DropIndex("dbo.TreatyPricingFinancialTableUploads", new[] { "TreatyPricingFinancialTableVersionDetailId" });
            DropIndex("dbo.TreatyPricingFinancialTableVersionDetails", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingFinancialTableVersionDetails", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingFinancialTableVersionDetails", new[] { "Description" });
            DropIndex("dbo.TreatyPricingFinancialTableVersionDetails", new[] { "DistributionTierPickListDetailId" });
            DropIndex("dbo.TreatyPricingFinancialTableVersionDetails", new[] { "TreatyPricingFinancialTableVersionId" });
            DropIndex("dbo.TreatyPricingFinancialTableUploadLegends", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingFinancialTableUploadLegends", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingFinancialTableUploadLegends", new[] { "Description" });
            DropIndex("dbo.TreatyPricingFinancialTableUploadLegends", new[] { "Code" });
            DropIndex("dbo.TreatyPricingFinancialTableUploadLegends", new[] { "TreatyPricingFinancialTableVersionDetailId" });
            DropIndex("dbo.TreatyPricingDefinitionAndExclusionVersions", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingDefinitionAndExclusionVersions", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingDefinitionAndExclusionVersions", new[] { "PersonInChargeId" });
            DropIndex("dbo.TreatyPricingDefinitionAndExclusionVersions", new[] { "Version" });
            DropIndex("dbo.TreatyPricingDefinitionAndExclusionVersions", new[] { "TreatyPricingDefinitionAndExclusionId" });
            DropIndex("dbo.TreatyPricingDefinitionAndExclusion", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingDefinitionAndExclusion", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingDefinitionAndExclusion", new[] { "Name" });
            DropIndex("dbo.TreatyPricingDefinitionAndExclusion", new[] { "Status" });
            DropIndex("dbo.TreatyPricingDefinitionAndExclusion", new[] { "Code" });
            DropIndex("dbo.TreatyPricingDefinitionAndExclusion", new[] { "TreatyPricingCedantId" });
            DropIndex("dbo.TreatyPricingCustomOtherVersions", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingCustomOtherVersions", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingCustomOtherVersions", new[] { "HashFileName" });
            DropIndex("dbo.TreatyPricingCustomOtherVersions", new[] { "FileName" });
            DropIndex("dbo.TreatyPricingCustomOtherVersions", new[] { "PersonInChargeId" });
            DropIndex("dbo.TreatyPricingCustomOtherVersions", new[] { "Version" });
            DropIndex("dbo.TreatyPricingCustomOtherVersions", new[] { "TreatyPricingCustomOtherId" });
            DropIndex("dbo.TreatyPricingCustomerOthers", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingCustomerOthers", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingCustomerOthers", new[] { "Name" });
            DropIndex("dbo.TreatyPricingCustomerOthers", new[] { "Status" });
            DropIndex("dbo.TreatyPricingCustomerOthers", new[] { "Code" });
            DropIndex("dbo.TreatyPricingCustomerOthers", new[] { "TreatyPricingCedantId" });
            DropIndex("dbo.TreatyPricingCustomOtherProducts", new[] { "TreatyPricingProductId" });
            DropIndex("dbo.TreatyPricingCustomOtherProducts", new[] { "TreatyPricingCustomOtherId" });
            DropIndex("dbo.TreatyPricingClaimApprovalLimitVersions", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingClaimApprovalLimitVersions", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingClaimApprovalLimitVersions", new[] { "Amount" });
            DropIndex("dbo.TreatyPricingClaimApprovalLimitVersions", new[] { "PersonInChargeId" });
            DropIndex("dbo.TreatyPricingClaimApprovalLimitVersions", new[] { "Version" });
            DropIndex("dbo.TreatyPricingClaimApprovalLimitVersions", new[] { "TreatyPricingClaimApprovalLimitId" });
            DropIndex("dbo.TreatyPricingClaimApprovalLimits", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingClaimApprovalLimits", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingClaimApprovalLimits", new[] { "Status" });
            DropIndex("dbo.TreatyPricingClaimApprovalLimits", new[] { "Name" });
            DropIndex("dbo.TreatyPricingClaimApprovalLimits", new[] { "Code" });
            DropIndex("dbo.TreatyPricingClaimApprovalLimits", new[] { "TreatyPricingCedantId" });
            DropIndex("dbo.TreatyPricingUwQuestionnaireVersions", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingUwQuestionnaireVersions", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingUwQuestionnaireVersions", new[] { "Type" });
            DropIndex("dbo.TreatyPricingUwQuestionnaireVersions", new[] { "PersonInChargeId" });
            DropIndex("dbo.TreatyPricingUwQuestionnaireVersions", new[] { "Version" });
            DropIndex("dbo.TreatyPricingUwQuestionnaireVersions", new[] { "TreatyPricingUwQuestionnaireId" });
            DropIndex("dbo.TreatyPricingUwQuestionnaires", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingUwQuestionnaires", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingUwQuestionnaires", new[] { "Status" });
            DropIndex("dbo.TreatyPricingUwQuestionnaires", new[] { "Description" });
            DropIndex("dbo.TreatyPricingUwQuestionnaires", new[] { "Name" });
            DropIndex("dbo.TreatyPricingUwQuestionnaires", new[] { "Code" });
            DropIndex("dbo.TreatyPricingUwQuestionnaires", new[] { "TreatyPricingCedantId" });
            DropIndex("dbo.TreatyPricingProfitCommissionVersions", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingProfitCommissionVersions", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingProfitCommissionVersions", new[] { "PersonInChargeId" });
            DropIndex("dbo.TreatyPricingProfitCommissionVersions", new[] { "Version" });
            DropIndex("dbo.TreatyPricingProfitCommissionVersions", new[] { "TreatyPricingProfitCommissionId" });
            DropIndex("dbo.TreatyPricingProfitCommissions", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingProfitCommissions", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingProfitCommissions", new[] { "EndDate" });
            DropIndex("dbo.TreatyPricingProfitCommissions", new[] { "StartDate" });
            DropIndex("dbo.TreatyPricingProfitCommissions", new[] { "EffectiveDate" });
            DropIndex("dbo.TreatyPricingProfitCommissions", new[] { "Status" });
            DropIndex("dbo.TreatyPricingProfitCommissions", new[] { "Description" });
            DropIndex("dbo.TreatyPricingProfitCommissions", new[] { "Name" });
            DropIndex("dbo.TreatyPricingProfitCommissions", new[] { "Code" });
            DropIndex("dbo.TreatyPricingProfitCommissions", new[] { "TreatyPricingCedantId" });
            DropIndex("dbo.TreatyPricingMedicalTableVersions", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingMedicalTableVersions", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingMedicalTableVersions", new[] { "AggregationNote" });
            DropIndex("dbo.TreatyPricingMedicalTableVersions", new[] { "Remarks" });
            DropIndex("dbo.TreatyPricingMedicalTableVersions", new[] { "EffectiveAt" });
            DropIndex("dbo.TreatyPricingMedicalTableVersions", new[] { "PersonInChargeId" });
            DropIndex("dbo.TreatyPricingMedicalTableVersions", new[] { "Version" });
            DropIndex("dbo.TreatyPricingMedicalTableVersions", new[] { "TreatyPricingMedicalTableId" });
            DropIndex("dbo.TreatyPricingMedicalTables", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingMedicalTables", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingMedicalTables", new[] { "AgeDefinitionPickListDetailId" });
            DropIndex("dbo.TreatyPricingMedicalTables", new[] { "CurrencyCode" });
            DropIndex("dbo.TreatyPricingMedicalTables", new[] { "Description" });
            DropIndex("dbo.TreatyPricingMedicalTables", new[] { "Name" });
            DropIndex("dbo.TreatyPricingMedicalTables", new[] { "Status" });
            DropIndex("dbo.TreatyPricingMedicalTables", new[] { "MedicalTableId" });
            DropIndex("dbo.TreatyPricingMedicalTables", new[] { "TreatyPricingCedantId" });
            DropIndex("dbo.TreatyPricingFinancialTableVersions", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingFinancialTableVersions", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingFinancialTableVersions", new[] { "AggregationNote" });
            DropIndex("dbo.TreatyPricingFinancialTableVersions", new[] { "Remarks" });
            DropIndex("dbo.TreatyPricingFinancialTableVersions", new[] { "EffectiveAt" });
            DropIndex("dbo.TreatyPricingFinancialTableVersions", new[] { "PersonInChargeId" });
            DropIndex("dbo.TreatyPricingFinancialTableVersions", new[] { "Version" });
            DropIndex("dbo.TreatyPricingFinancialTableVersions", new[] { "TreatyPricingFinancialTableId" });
            DropIndex("dbo.TreatyPricingFinancialTables", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingFinancialTables", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingFinancialTables", new[] { "CurrencyCode" });
            DropIndex("dbo.TreatyPricingFinancialTables", new[] { "Description" });
            DropIndex("dbo.TreatyPricingFinancialTables", new[] { "Name" });
            DropIndex("dbo.TreatyPricingFinancialTables", new[] { "Status" });
            DropIndex("dbo.TreatyPricingFinancialTables", new[] { "FinancialTableId" });
            DropIndex("dbo.TreatyPricingFinancialTables", new[] { "TreatyPricingCedantId" });
            DropIndex("dbo.TreatyPricingRateTableVersions", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingRateTableVersions", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingRateTableVersions", new[] { "RateGuaranteePickListDetailId" });
            DropIndex("dbo.TreatyPricingRateTableVersions", new[] { "AgeBasisPickListDetailId" });
            DropIndex("dbo.TreatyPricingRateTableVersions", new[] { "EffectiveDate" });
            DropIndex("dbo.TreatyPricingRateTableVersions", new[] { "PersonInChargeId" });
            DropIndex("dbo.TreatyPricingRateTableVersions", new[] { "Version" });
            DropIndex("dbo.TreatyPricingRateTableVersions", new[] { "TreatyPricingRateTableId" });
            DropIndex("dbo.TreatyPricingRateTableGroups", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingRateTableGroups", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingRateTableGroups", new[] { "UploadedById" });
            DropIndex("dbo.TreatyPricingRateTableGroups", new[] { "Status" });
            DropIndex("dbo.TreatyPricingRateTableGroups", new[] { "NoOfRateTable" });
            DropIndex("dbo.TreatyPricingRateTableGroups", new[] { "HashFileName" });
            DropIndex("dbo.TreatyPricingRateTableGroups", new[] { "FileName" });
            DropIndex("dbo.TreatyPricingRateTableGroups", new[] { "Description" });
            DropIndex("dbo.TreatyPricingRateTableGroups", new[] { "Name" });
            DropIndex("dbo.TreatyPricingRateTableGroups", new[] { "Code" });
            DropIndex("dbo.TreatyPricingRateTableGroups", new[] { "TreatyPricingCedantId" });
            DropIndex("dbo.TreatyPricingRateTables", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingRateTables", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingRateTables", new[] { "Status" });
            DropIndex("dbo.TreatyPricingRateTables", new[] { "BenefitId" });
            DropIndex("dbo.TreatyPricingRateTables", new[] { "Name" });
            DropIndex("dbo.TreatyPricingRateTables", new[] { "Code" });
            DropIndex("dbo.TreatyPricingRateTables", new[] { "TreatyPricingRateTableGroupId" });
            DropIndex("dbo.TreatyPricingCampaignVersions", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingCampaignVersions", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingCampaignVersions", new[] { "TreatyPricingAdvantageProgramVersionId" });
            DropIndex("dbo.TreatyPricingCampaignVersions", new[] { "TreatyPricingAdvantageProgramId" });
            DropIndex("dbo.TreatyPricingCampaignVersions", new[] { "TreatyPricingFinancialTableVersionId" });
            DropIndex("dbo.TreatyPricingCampaignVersions", new[] { "TreatyPricingFinancialTableId" });
            DropIndex("dbo.TreatyPricingCampaignVersions", new[] { "TreatyPricingMedicalTableVersionId" });
            DropIndex("dbo.TreatyPricingCampaignVersions", new[] { "TreatyPricingMedicalTableId" });
            DropIndex("dbo.TreatyPricingCampaignVersions", new[] { "TreatyPricingUwQuestionnaireVersionId" });
            DropIndex("dbo.TreatyPricingCampaignVersions", new[] { "TreatyPricingUwQuestionnaireId" });
            DropIndex("dbo.TreatyPricingCampaignVersions", new[] { "TreatyPricingProfitCommissionVersionId" });
            DropIndex("dbo.TreatyPricingCampaignVersions", new[] { "TreatyPricingProfitCommissionId" });
            DropIndex("dbo.TreatyPricingCampaignVersions", new[] { "ReinsDiscountTreatyPricingRateTableVersionId" });
            DropIndex("dbo.TreatyPricingCampaignVersions", new[] { "ReinsDiscountTreatyPricingRateTableId" });
            DropIndex("dbo.TreatyPricingCampaignVersions", new[] { "ReinsRateTreatyPricingRateTableVersionId" });
            DropIndex("dbo.TreatyPricingCampaignVersions", new[] { "ReinsRateTreatyPricingRateTableId" });
            DropIndex("dbo.TreatyPricingCampaignVersions", new[] { "AgeBasisPickListDetailId" });
            DropIndex("dbo.TreatyPricingCampaignVersions", new[] { "IsPerBenefit" });
            DropIndex("dbo.TreatyPricingCampaignVersions", new[] { "PersonInChargeId" });
            DropIndex("dbo.TreatyPricingCampaignVersions", new[] { "Version" });
            DropIndex("dbo.TreatyPricingCampaignVersions", new[] { "TreatyPricingCampaignId" });
            DropIndex("dbo.TreatyPricingProducts", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingProducts", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingProducts", new[] { "HasPerLifeRetro" });
            DropIndex("dbo.TreatyPricingProducts", new[] { "QuotationName" });
            DropIndex("dbo.TreatyPricingProducts", new[] { "EffectiveDate" });
            DropIndex("dbo.TreatyPricingProducts", new[] { "Name" });
            DropIndex("dbo.TreatyPricingProducts", new[] { "Code" });
            DropIndex("dbo.TreatyPricingProducts", new[] { "TreatyPricingCedantId" });
            DropIndex("dbo.TreatyPricingCampaigns", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingCampaigns", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingCampaigns", new[] { "Name" });
            DropIndex("dbo.TreatyPricingCampaigns", new[] { "Code" });
            DropIndex("dbo.TreatyPricingCampaigns", new[] { "TreatyPricingCedantId" });
            DropIndex("dbo.TreatyPricingCampaignProducts", new[] { "TreatyPricingProductId" });
            DropIndex("dbo.TreatyPricingCampaignProducts", new[] { "TreatyPricingCampaignId" });
            DropIndex("dbo.TreatyPricingAdvantageProgramVersions", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingAdvantageProgramVersions", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingAdvantageProgramVersions", new[] { "PersonInChargeId" });
            DropIndex("dbo.TreatyPricingAdvantageProgramVersions", new[] { "Version" });
            DropIndex("dbo.TreatyPricingAdvantageProgramVersions", new[] { "TreatyPricingAdvantageProgramId" });
            DropIndex("dbo.TreatyPricingAdvantageProgramVersionBenefits", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingAdvantageProgramVersionBenefits", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingAdvantageProgramVersionBenefits", new[] { "BenefitId" });
            DropIndex("dbo.TreatyPricingAdvantageProgramVersionBenefits", new[] { "TreatyPricingAdvantageProgramVersionId" });
            DropIndex("dbo.TreatyPricingCedants", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingCedants", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingCedants", new[] { "Code" });
            DropIndex("dbo.TreatyPricingCedants", new[] { "ReinsuranceTypePickListDetailId" });
            DropIndex("dbo.TreatyPricingCedants", new[] { "CedantId" });
            DropIndex("dbo.TreatyPricingAdvantagePrograms", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingAdvantagePrograms", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingAdvantagePrograms", new[] { "Name" });
            DropIndex("dbo.TreatyPricingAdvantagePrograms", new[] { "Code" });
            DropIndex("dbo.TreatyPricingAdvantagePrograms", new[] { "TreatyPricingCedantId" });
            DropIndex("dbo.TreatyDiscountTableDetails", new[] { "AARTo" });
            DropIndex("dbo.TreatyDiscountTableDetails", new[] { "AARFrom" });
            DropIndex("dbo.Templates", new[] { "UpdatedById" });
            DropIndex("dbo.Templates", new[] { "CreatedById" });
            DropIndex("dbo.Templates", new[] { "DocumentTypeId" });
            DropIndex("dbo.Templates", new[] { "CedantId" });
            DropIndex("dbo.Templates", new[] { "Code" });
            DropIndex("dbo.TemplateDetails", new[] { "UpdatedById" });
            DropIndex("dbo.TemplateDetails", new[] { "CreatedById" });
            DropIndex("dbo.TemplateDetails", new[] { "HashFileName" });
            DropIndex("dbo.TemplateDetails", new[] { "FileName" });
            DropIndex("dbo.TemplateDetails", new[] { "TemplateId" });
            DropIndex("dbo.RetroTreatyDetails", new[] { "UpdatedById" });
            DropIndex("dbo.RetroTreatyDetails", new[] { "CreatedById" });
            DropIndex("dbo.RetroTreatyDetails", new[] { "MlreShare" });
            DropIndex("dbo.RetroTreatyDetails", new[] { "TreatyDiscountTableId" });
            DropIndex("dbo.RetroTreatyDetails", new[] { "PremiumSpreadTableId" });
            DropIndex("dbo.RetroTreatyDetails", new[] { "PerLifeRetroConfigurationTreatyId" });
            DropIndex("dbo.RetroTreatyDetails", new[] { "RetroTreatyId" });
            DropIndex("dbo.RetroTreaties", new[] { "UpdatedById" });
            DropIndex("dbo.RetroTreaties", new[] { "CreatedById" });
            DropIndex("dbo.RetroTreaties", new[] { "TreatyDiscountTableId" });
            DropIndex("dbo.RetroTreaties", new[] { "RetroShare" });
            DropIndex("dbo.RetroTreaties", new[] { "IsLobAdvantageProgram" });
            DropIndex("dbo.RetroTreaties", new[] { "IsLobFacultative" });
            DropIndex("dbo.RetroTreaties", new[] { "IsLobAutomatic" });
            DropIndex("dbo.RetroTreaties", new[] { "TreatyTypePickListDetailId" });
            DropIndex("dbo.RetroTreaties", new[] { "Code" });
            DropIndex("dbo.RetroTreaties", new[] { "Party" });
            DropIndex("dbo.RetroTreaties", new[] { "Status" });
            DropIndex("dbo.RetroRegisterFiles", new[] { "UpdatedById" });
            DropIndex("dbo.RetroRegisterFiles", new[] { "CreatedById" });
            DropIndex("dbo.RetroRegisterFiles", new[] { "Status" });
            DropIndex("dbo.RetroRegisterFiles", new[] { "HashFileName" });
            DropIndex("dbo.RetroRegisterFiles", new[] { "FileName" });
            DropIndex("dbo.RetroRegisterFiles", new[] { "RetroRegisterId" });
            DropIndex("dbo.RetroBenefitRetentionLimits", new[] { "UpdatedById" });
            DropIndex("dbo.RetroBenefitRetentionLimits", new[] { "CreatedById" });
            DropIndex("dbo.RetroBenefitRetentionLimits", new[] { "MinRetentionLimit" });
            DropIndex("dbo.RetroBenefitRetentionLimits", new[] { "Description" });
            DropIndex("dbo.RetroBenefitRetentionLimits", new[] { "Type" });
            DropIndex("dbo.RetroBenefitRetentionLimits", new[] { "RetroBenefitCodeId" });
            DropIndex("dbo.RetroBenefitRetentionLimitDetails", new[] { "UpdatedById" });
            DropIndex("dbo.RetroBenefitRetentionLimitDetails", new[] { "CreatedById" });
            DropIndex("dbo.RetroBenefitRetentionLimitDetails", new[] { "MinReinsAmount" });
            DropIndex("dbo.RetroBenefitRetentionLimitDetails", new[] { "MlreRetentionAmount" });
            DropIndex("dbo.RetroBenefitRetentionLimitDetails", new[] { "ReinsEffEndDate" });
            DropIndex("dbo.RetroBenefitRetentionLimitDetails", new[] { "ReinsEffStartDate" });
            DropIndex("dbo.RetroBenefitRetentionLimitDetails", new[] { "MortalityLimitTo" });
            DropIndex("dbo.RetroBenefitRetentionLimitDetails", new[] { "MortalityLimitFrom" });
            DropIndex("dbo.RetroBenefitRetentionLimitDetails", new[] { "MaxIssueAge" });
            DropIndex("dbo.RetroBenefitRetentionLimitDetails", new[] { "MinIssueAge" });
            DropIndex("dbo.RetroBenefitRetentionLimitDetails", new[] { "RetroBenefitRetentionLimitId" });
            DropIndex("dbo.RetroBenefitCodeMappings", new[] { "UpdatedById" });
            DropIndex("dbo.RetroBenefitCodeMappings", new[] { "CreatedById" });
            DropIndex("dbo.RetroBenefitCodeMappings", new[] { "IsPerAnnum" });
            DropIndex("dbo.RetroBenefitCodeMappings", new[] { "BenefitId" });
            DropIndex("dbo.RetroBenefitCodes", new[] { "UpdatedById" });
            DropIndex("dbo.RetroBenefitCodes", new[] { "CreatedById" });
            DropIndex("dbo.RetroBenefitCodes", new[] { "Status" });
            DropIndex("dbo.RetroBenefitCodes", new[] { "CeaseDate" });
            DropIndex("dbo.RetroBenefitCodes", new[] { "EffectiveDate" });
            DropIndex("dbo.RetroBenefitCodes", new[] { "Description" });
            DropIndex("dbo.RetroBenefitCodes", new[] { "Code" });
            DropIndex("dbo.RetroBenefitCodeMappingDetails", new[] { "UpdatedById" });
            DropIndex("dbo.RetroBenefitCodeMappingDetails", new[] { "CreatedById" });
            DropIndex("dbo.RetroBenefitCodeMappingDetails", new[] { "IsComputePremium" });
            DropIndex("dbo.RetroBenefitCodeMappingDetails", new[] { "RetroBenefitCodeId" });
            DropIndex("dbo.RetroBenefitCodeMappingDetails", new[] { "RetroBenefitCodeMappingId" });
            DropIndex("dbo.PerLifeRetroConfigurationTreaties", new[] { "UpdatedById" });
            DropIndex("dbo.PerLifeRetroConfigurationTreaties", new[] { "CreatedById" });
            DropIndex("dbo.PerLifeRetroConfigurationTreaties", new[] { "IsToAggregate" });
            DropIndex("dbo.PerLifeRetroConfigurationTreaties", new[] { "RiskQuarterEndDate" });
            DropIndex("dbo.PerLifeRetroConfigurationTreaties", new[] { "RiskQuarterStartDate" });
            DropIndex("dbo.PerLifeRetroConfigurationTreaties", new[] { "ReinsEffectiveEndDate" });
            DropIndex("dbo.PerLifeRetroConfigurationTreaties", new[] { "ReinsEffectiveStartDate" });
            DropIndex("dbo.PerLifeRetroConfigurationTreaties", new[] { "BusinessTypePickListDetailId" });
            DropIndex("dbo.PerLifeRetroConfigurationTreaties", new[] { "TreatyTypePickListDetailId" });
            DropIndex("dbo.PerLifeRetroConfigurationTreaties", new[] { "TreatyCodeId" });
            DropIndex("dbo.PerLifeRetroConfigurationRatios", new[] { "UpdatedById" });
            DropIndex("dbo.PerLifeRetroConfigurationRatios", new[] { "CreatedById" });
            DropIndex("dbo.PerLifeRetroConfigurationRatios", new[] { "RuleCeaseDate" });
            DropIndex("dbo.PerLifeRetroConfigurationRatios", new[] { "RuleEffectiveDate" });
            DropIndex("dbo.PerLifeRetroConfigurationRatios", new[] { "RiskQuarterEndDate" });
            DropIndex("dbo.PerLifeRetroConfigurationRatios", new[] { "RiskQuarterStartDate" });
            DropIndex("dbo.PerLifeRetroConfigurationRatios", new[] { "ReinsEffectiveEndDate" });
            DropIndex("dbo.PerLifeRetroConfigurationRatios", new[] { "ReinsEffectiveStartDate" });
            DropIndex("dbo.PerLifeRetroConfigurationRatios", new[] { "MlreRetainRatio" });
            DropIndex("dbo.PerLifeRetroConfigurationRatios", new[] { "RetroRatio" });
            DropIndex("dbo.PerLifeRetroConfigurationRatios", new[] { "TreatyCodeId" });
            DropIndex("dbo.PerLifeDuplicationChecks", new[] { "UpdatedById" });
            DropIndex("dbo.PerLifeDuplicationChecks", new[] { "CreatedById" });
            DropIndex("dbo.PerLifeDuplicationChecks", new[] { "EnableReinsuranceBasisCodeCheck" });
            DropIndex("dbo.PerLifeDuplicationChecks", new[] { "ReinsuranceEffectiveEndDate" });
            DropIndex("dbo.PerLifeDuplicationChecks", new[] { "ReinsuranceEffectiveStartDate" });
            DropIndex("dbo.PerLifeDuplicationChecks", new[] { "Inclusion" });
            DropIndex("dbo.PerLifeDuplicationChecks", new[] { "Description" });
            DropIndex("dbo.PerLifeDuplicationChecks", new[] { "ConfigurationCode" });
            DropIndex("dbo.PerLifeDuplicationCheckDetails", new[] { "CreatedById" });
            DropIndex("dbo.PerLifeDuplicationCheckDetails", new[] { "TreatyCode" });
            DropIndex("dbo.PerLifeDuplicationCheckDetails", new[] { "PerLifeDuplicationCheckId" });
            DropIndex("dbo.PerLifeRetroGenders", new[] { "UpdatedById" });
            DropIndex("dbo.PerLifeRetroGenders", new[] { "CreatedById" });
            DropIndex("dbo.PerLifeRetroGenders", new[] { "EffectiveEndDate" });
            DropIndex("dbo.PerLifeRetroGenders", new[] { "EffectiveStartDate" });
            DropIndex("dbo.PerLifeRetroGenders", new[] { "Gender" });
            DropIndex("dbo.PerLifeRetroCountries", new[] { "UpdatedById" });
            DropIndex("dbo.PerLifeRetroCountries", new[] { "CreatedById" });
            DropIndex("dbo.PerLifeRetroCountries", new[] { "EffectiveEndDate" });
            DropIndex("dbo.PerLifeRetroCountries", new[] { "EffectiveStartDate" });
            DropIndex("dbo.PerLifeRetroCountries", new[] { "Country" });
            DropIndex("dbo.PerLifeDataCorrections", new[] { "UpdatedById" });
            DropIndex("dbo.PerLifeDataCorrections", new[] { "CreatedById" });
            DropIndex("dbo.PerLifeDataCorrections", new[] { "DateUpdated" });
            DropIndex("dbo.PerLifeDataCorrections", new[] { "ExceptionStatusPickListDetailId" });
            DropIndex("dbo.PerLifeDataCorrections", new[] { "DateOfExceptionDetected" });
            DropIndex("dbo.PerLifeDataCorrections", new[] { "IsProceedToAggregate" });
            DropIndex("dbo.PerLifeDataCorrections", new[] { "DateOfPolicyExist" });
            DropIndex("dbo.PerLifeDataCorrections", new[] { "PerLifeRetroCountryId" });
            DropIndex("dbo.PerLifeDataCorrections", new[] { "PerLifeRetroGenderId" });
            DropIndex("dbo.PerLifeDataCorrections", new[] { "TerritoryOfIssueCodePickListDetailId" });
            DropIndex("dbo.PerLifeDataCorrections", new[] { "InsuredGenderCodePickListDetailId" });
            DropIndex("dbo.PerLifeDataCorrections", new[] { "PolicyNumber" });
            DropIndex("dbo.PerLifeDataCorrections", new[] { "InsuredDateOfBirth" });
            DropIndex("dbo.PerLifeDataCorrections", new[] { "InsuredName" });
            DropIndex("dbo.PerLifeDataCorrections", new[] { "TreatyCodeId" });
            DropIndex("dbo.PerLifeAggregationDuplicationListings", new[] { "UpdatedById" });
            DropIndex("dbo.PerLifeAggregationDuplicationListings", new[] { "CreatedById" });
            DropIndex("dbo.PerLifeAggregationDuplicationListings", new[] { "ExceptionStatusPickListDetailId" });
            DropIndex("dbo.PerLifeAggregationDuplicationListings", new[] { "DateUpdated" });
            DropIndex("dbo.PerLifeAggregationDuplicationListings", new[] { "ProceedToAggregate" });
            DropIndex("dbo.PerLifeAggregationDuplicationListings", new[] { "TransactionTypePickListDetailId" });
            DropIndex("dbo.PerLifeAggregationDuplicationListings", new[] { "CedingBenefitTypeCode" });
            DropIndex("dbo.PerLifeAggregationDuplicationListings", new[] { "CedingBenefitRiskCode" });
            DropIndex("dbo.PerLifeAggregationDuplicationListings", new[] { "MLReBenefitCodeId" });
            DropIndex("dbo.PerLifeAggregationDuplicationListings", new[] { "CedantPlanCode" });
            DropIndex("dbo.PerLifeAggregationDuplicationListings", new[] { "ReinsBasisCodePickListDetailId" });
            DropIndex("dbo.PerLifeAggregationDuplicationListings", new[] { "FundsAccountingTypePickListDetailId" });
            DropIndex("dbo.PerLifeAggregationDuplicationListings", new[] { "ReinsuranceEffectiveDate" });
            DropIndex("dbo.PerLifeAggregationDuplicationListings", new[] { "PolicyNumber" });
            DropIndex("dbo.PerLifeAggregationDuplicationListings", new[] { "InsuredDateOfBirth" });
            DropIndex("dbo.PerLifeAggregationDuplicationListings", new[] { "InsuredGenderCodePickListDetailId" });
            DropIndex("dbo.PerLifeAggregationDuplicationListings", new[] { "InsuredName" });
            DropIndex("dbo.PerLifeAggregationDuplicationListings", new[] { "TreatyCodeId" });
            DropIndex("dbo.PerLifeAggregations", new[] { "UpdatedById" });
            DropIndex("dbo.PerLifeAggregations", new[] { "CreatedById" });
            DropIndex("dbo.PerLifeAggregations", new[] { "Status" });
            DropIndex("dbo.PerLifeAggregations", new[] { "ProcessingDate" });
            DropIndex("dbo.PerLifeAggregations", new[] { "SoaQuarter" });
            DropIndex("dbo.PerLifeAggregations", new[] { "CutOffId" });
            DropIndex("dbo.PerLifeAggregations", new[] { "FundsAccountingTypePickListDetailId" });
            DropIndex("dbo.PerLifeAggregationDetails", new[] { "UpdatedById" });
            DropIndex("dbo.PerLifeAggregationDetails", new[] { "CreatedById" });
            DropIndex("dbo.PerLifeAggregationDetails", new[] { "Status" });
            DropIndex("dbo.PerLifeAggregationDetails", new[] { "ProcessingDate" });
            DropIndex("dbo.PerLifeAggregationDetails", new[] { "RiskQuarter" });
            DropIndex("dbo.PerLifeAggregationDetails", new[] { "PerLifeAggregationId" });
            DropIndex("dbo.PerLifeAggregationDetailTreaties", new[] { "UpdatedById" });
            DropIndex("dbo.PerLifeAggregationDetailTreaties", new[] { "CreatedById" });
            DropIndex("dbo.PerLifeAggregationDetailTreaties", new[] { "TreatyCode" });
            DropIndex("dbo.PerLifeAggregationDetailTreaties", new[] { "PerLifeAggregationDetailId" });
            DropIndex("dbo.PerLifeAggregationDetailData", new[] { "UpdatedById" });
            DropIndex("dbo.PerLifeAggregationDetailData", new[] { "CreatedById" });
            DropIndex("dbo.PerLifeAggregationDetailData", new[] { "IsException" });
            DropIndex("dbo.PerLifeAggregationDetailData", new[] { "RiDataWarehouseHistoryId" });
            DropIndex("dbo.PerLifeAggregationDetailData", new[] { "PerLifeAggregationDetailTreatyId" });
            DropIndex("dbo.PerLifeAggregationConflictListings", new[] { "UpdatedById" });
            DropIndex("dbo.PerLifeAggregationConflictListings", new[] { "CreatedById" });
            DropIndex("dbo.PerLifeAggregationConflictListings", new[] { "TerritoryOfIssueCodePickListDetailId" });
            DropIndex("dbo.PerLifeAggregationConflictListings", new[] { "MLReBenefitCodeId" });
            DropIndex("dbo.PerLifeAggregationConflictListings", new[] { "CedingBenefitTypeCode" });
            DropIndex("dbo.PerLifeAggregationConflictListings", new[] { "CedingBenefitRiskCode" });
            DropIndex("dbo.PerLifeAggregationConflictListings", new[] { "CedantPlanCode" });
            DropIndex("dbo.PerLifeAggregationConflictListings", new[] { "RetroPremiumFrequencyModePickListDetailId" });
            DropIndex("dbo.PerLifeAggregationConflictListings", new[] { "PremiumFrequencyModePickListDetailId" });
            DropIndex("dbo.PerLifeAggregationConflictListings", new[] { "NetPremium" });
            DropIndex("dbo.PerLifeAggregationConflictListings", new[] { "GrossPremium" });
            DropIndex("dbo.PerLifeAggregationConflictListings", new[] { "AAR" });
            DropIndex("dbo.PerLifeAggregationConflictListings", new[] { "ReinsEffectiveDatePol" });
            DropIndex("dbo.PerLifeAggregationConflictListings", new[] { "PolicyNumber" });
            DropIndex("dbo.PerLifeAggregationConflictListings", new[] { "InsuredDateOfBirth" });
            DropIndex("dbo.PerLifeAggregationConflictListings", new[] { "InsuredGenderCodePickListDetailId" });
            DropIndex("dbo.PerLifeAggregationConflictListings", new[] { "InsuredName" });
            DropIndex("dbo.PerLifeAggregationConflictListings", new[] { "RiskMonth" });
            DropIndex("dbo.PerLifeAggregationConflictListings", new[] { "RiskYear" });
            DropIndex("dbo.PerLifeAggregationConflictListings", new[] { "TreatyCodeId" });
            DropIndex("dbo.InsuredGroupNames", new[] { "UpdatedById" });
            DropIndex("dbo.InsuredGroupNames", new[] { "CreatedById" });
            DropIndex("dbo.InsuredGroupNames", new[] { "Name" });
            DropIndex("dbo.HipsCategoryDetails", new[] { "UpdatedById" });
            DropIndex("dbo.HipsCategoryDetails", new[] { "CreatedById" });
            DropIndex("dbo.HipsCategoryDetails", new[] { "ItemType" });
            DropIndex("dbo.HipsCategoryDetails", new[] { "Description" });
            DropIndex("dbo.HipsCategoryDetails", new[] { "Subcategory" });
            DropIndex("dbo.HipsCategoryDetails", new[] { "HipsCategoryId" });
            DropIndex("dbo.HipsCategories", new[] { "UpdatedById" });
            DropIndex("dbo.HipsCategories", new[] { "CreatedById" });
            DropIndex("dbo.HipsCategories", new[] { "Name" });
            DropIndex("dbo.HipsCategories", new[] { "Code" });
            DropIndex("dbo.GtlBenefitCategories", new[] { "UpdatedById" });
            DropIndex("dbo.GtlBenefitCategories", new[] { "CreatedById" });
            DropIndex("dbo.GtlBenefitCategories", new[] { "Benefit" });
            DropIndex("dbo.GtlBenefitCategories", new[] { "Category" });
            DropIndex("dbo.GstMaintenances", new[] { "UpdatedById" });
            DropIndex("dbo.GstMaintenances", new[] { "CreatedById" });
            DropIndex("dbo.GstMaintenances", new[] { "Rate" });
            DropIndex("dbo.GstMaintenances", new[] { "RiskEffectiveEndDate" });
            DropIndex("dbo.GstMaintenances", new[] { "RiskEffectiveStartDate" });
            DropIndex("dbo.GstMaintenances", new[] { "EffectiveEndDate" });
            DropIndex("dbo.GstMaintenances", new[] { "EffectiveStartDate" });
            DropIndex("dbo.Remarks", new[] { "Version" });
            DropIndex("dbo.Remarks", new[] { "Subject" });
            DropIndex("dbo.Remarks", new[] { "StatusHistoryId" });
            DropIndex("dbo.Designations", new[] { "UpdatedById" });
            DropIndex("dbo.Designations", new[] { "CreatedById" });
            DropIndex("dbo.Designations", new[] { "Description" });
            DropIndex("dbo.Designations", new[] { "Code" });
            DropIndex("dbo.StatusHistories", new[] { "Version" });
            DropIndex("dbo.StatusHistories", new[] { "SubObjectId" });
            DropIndex("dbo.StatusHistories", new[] { "SubModuleController" });
            DropColumn("dbo.TreatyDiscountTableDetails", "AARTo");
            DropColumn("dbo.TreatyDiscountTableDetails", "AARFrom");
            DropColumn("dbo.RetroRegisterBatches", "Type");
            DropColumn("dbo.Remarks", "Version");
            DropColumn("dbo.Remarks", "Subject");
            DropColumn("dbo.Remarks", "StatusHistoryId");
            DropColumn("dbo.TreatyDiscountTables", "Type");
            DropColumn("dbo.PremiumSpreadTables", "Type");
            DropColumn("dbo.StatusHistories", "Version");
            DropColumn("dbo.StatusHistories", "SubObjectId");
            DropColumn("dbo.StatusHistories", "SubModuleController");
            DropTable("dbo.ValidDuplicationLists");
            DropTable("dbo.TreatyPricingWorkflowObjects");
            DropTable("dbo.TreatyPricingUwQuestionnaireVersionFiles");
            DropTable("dbo.UwQuestionnaireCategories");
            DropTable("dbo.TreatyPricingUwQuestionnaireVersionDetails");
            DropTable("dbo.TreatyPricingTreatyWorkflowVersions");
            DropTable("dbo.TreatyPricingTreatyWorkflows");
            DropTable("dbo.TreatyPricingTierProfitCommissions");
            DropTable("dbo.TreatyPricingReportGenerations");
            DropTable("dbo.TreatyPricingRateTableRates");
            DropTable("dbo.TreatyPricingRateTableDetails");
            DropTable("dbo.TreatyPricingQuotationWorkflowVersions");
            DropTable("dbo.TreatyPricingQuotationWorkflowVersionQuotationChecklists");
            DropTable("dbo.TreatyPricingQuotationWorkflows");
            DropTable("dbo.TreatyPricingProfitCommissionDetails");
            DropTable("dbo.TreatyPricingProductPerLifeRetros");
            DropTable("dbo.TreatyPricingProductDetails");
            DropTable("dbo.TreatyPricingProductBenefits");
            DropTable("dbo.TreatyPricingProductBenefitDirectRetros");
            DropTable("dbo.TreatyPricingPickListDetails");
            DropTable("dbo.TreatyPricingPerLifeRetroVersionTiers");
            DropTable("dbo.TreatyPricingPerLifeRetroVersionDetails");
            DropTable("dbo.TreatyPricingPerLifeRetroVersions");
            DropTable("dbo.TreatyPricingPerLifeRetroVersionBenefits");
            DropTable("dbo.TreatyPricingPerLifeRetroProducts");
            DropTable("dbo.TreatyPricingPerLifeRetro");
            DropTable("dbo.TreatyPricingMedicalTableVersionFiles");
            DropTable("dbo.TreatyPricingMedicalTableUploadLegends");
            DropTable("dbo.TreatyPricingMedicalTableUploadRows");
            DropTable("dbo.TreatyPricingMedicalTableVersionDetails");
            DropTable("dbo.TreatyPricingMedicalTableUploadColumns");
            DropTable("dbo.TreatyPricingMedicalTableUploadCells");
            DropTable("dbo.TreatyPricingUwLimitVersions");
            DropTable("dbo.TreatyPricingUwLimits");
            DropTable("dbo.TreatyPricingGroupReferralVersionBenefits");
            DropTable("dbo.TreatyPricingGroupReferralHipsTables");
            DropTable("dbo.TreatyPricingGroupReferralGtlTables");
            DropTable("dbo.TreatyPricingGroupReferralGhsTables");
            DropTable("dbo.TreatyPricingGroupReferralFiles");
            DropTable("dbo.TreatyPricingGroupReferralChecklists");
            DropTable("dbo.TreatyPricingGroupReferralChecklistDetails");
            DropTable("dbo.TreatyPricingGroupReferralVersions");
            DropTable("dbo.TreatyPricingProductVersions");
            DropTable("dbo.TreatyPricingGroupReferrals");
            DropTable("dbo.TreatyPricingGroupMasterLetters");
            DropTable("dbo.TreatyPricingGroupMasterLetterGroupReferrals");
            DropTable("dbo.TreatyPricingFinancialTableVersionFiles");
            DropTable("dbo.TreatyPricingFinancialTableUploads");
            DropTable("dbo.TreatyPricingFinancialTableVersionDetails");
            DropTable("dbo.TreatyPricingFinancialTableUploadLegends");
            DropTable("dbo.TreatyPricingDefinitionAndExclusionVersions");
            DropTable("dbo.TreatyPricingDefinitionAndExclusion");
            DropTable("dbo.TreatyPricingCustomOtherVersions");
            DropTable("dbo.TreatyPricingCustomerOthers");
            DropTable("dbo.TreatyPricingCustomOtherProducts");
            DropTable("dbo.TreatyPricingClaimApprovalLimitVersions");
            DropTable("dbo.TreatyPricingClaimApprovalLimits");
            DropTable("dbo.TreatyPricingUwQuestionnaireVersions");
            DropTable("dbo.TreatyPricingUwQuestionnaires");
            DropTable("dbo.TreatyPricingProfitCommissionVersions");
            DropTable("dbo.TreatyPricingProfitCommissions");
            DropTable("dbo.TreatyPricingMedicalTableVersions");
            DropTable("dbo.TreatyPricingMedicalTables");
            DropTable("dbo.TreatyPricingFinancialTableVersions");
            DropTable("dbo.TreatyPricingFinancialTables");
            DropTable("dbo.TreatyPricingRateTableVersions");
            DropTable("dbo.TreatyPricingRateTableGroups");
            DropTable("dbo.TreatyPricingRateTables");
            DropTable("dbo.TreatyPricingCampaignVersions");
            DropTable("dbo.TreatyPricingProducts");
            DropTable("dbo.TreatyPricingCampaigns");
            DropTable("dbo.TreatyPricingCampaignProducts");
            DropTable("dbo.TreatyPricingAdvantageProgramVersions");
            DropTable("dbo.TreatyPricingAdvantageProgramVersionBenefits");
            DropTable("dbo.TreatyPricingCedants");
            DropTable("dbo.TreatyPricingAdvantagePrograms");
            DropTable("dbo.Templates");
            DropTable("dbo.TemplateDetails");
            DropTable("dbo.RetroTreatyDetails");
            DropTable("dbo.RetroTreaties");
            DropTable("dbo.RetroRegisterFiles");
            DropTable("dbo.RetroBenefitRetentionLimits");
            DropTable("dbo.RetroBenefitRetentionLimitDetails");
            DropTable("dbo.RetroBenefitCodeMappings");
            DropTable("dbo.RetroBenefitCodes");
            DropTable("dbo.RetroBenefitCodeMappingDetails");
            DropTable("dbo.PerLifeRetroConfigurationTreaties");
            DropTable("dbo.PerLifeRetroConfigurationRatios");
            DropTable("dbo.PerLifeDuplicationChecks");
            DropTable("dbo.PerLifeDuplicationCheckDetails");
            DropTable("dbo.PerLifeRetroGenders");
            DropTable("dbo.PerLifeRetroCountries");
            DropTable("dbo.PerLifeDataCorrections");
            DropTable("dbo.PerLifeAggregationDuplicationListings");
            DropTable("dbo.PerLifeAggregations");
            DropTable("dbo.PerLifeAggregationDetails");
            DropTable("dbo.PerLifeAggregationDetailTreaties");
            DropTable("dbo.PerLifeAggregationDetailData");
            DropTable("dbo.PerLifeAggregationConflictListings");
            DropTable("dbo.InsuredGroupNames");
            DropTable("dbo.HipsCategoryDetails");
            DropTable("dbo.HipsCategories");
            DropTable("dbo.GtlBenefitCategories");
            DropTable("dbo.GstMaintenances");
            DropTable("dbo.Designations");
            AddForeignKey("dbo.Mfrs17ContractCodeDetails", "Mfrs17ContractCodeId", "dbo.Mfrs17ContractCodes", "Id");
            AddForeignKey("dbo.Mfrs17ContractCodeDetails", "CreatedById", "dbo.Users", "Id");
        }
    }
}
