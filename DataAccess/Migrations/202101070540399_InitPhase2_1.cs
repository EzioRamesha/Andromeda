namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class InitPhase2_1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FinanceProvisionings", "CutOffId", "dbo.CutOff");
            DropIndex("dbo.FinanceProvisionings", new[] { "CutOffId" });
            DropIndex("dbo.FinanceProvisionings", new[] { "Record" });
            DropIndex("dbo.FinanceProvisionings", new[] { "Amount" });
            CreateTable(
                "dbo.SanctionAddresses",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    SanctionId = c.Int(nullable: false),
                    Address = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Sanctions", t => t.SanctionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.SanctionId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.Sanctions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    SanctionBatchId = c.Int(nullable: false),
                    PublicationInformation = c.String(maxLength: 255),
                    Category = c.String(maxLength: 128),
                    Name = c.String(maxLength: 128),
                    RefNumber = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.SanctionBatches", t => t.SanctionBatchId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.SanctionBatchId)
                .Index(t => t.PublicationInformation)
                .Index(t => t.Category)
                .Index(t => t.Name)
                .Index(t => t.RefNumber)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.SanctionBatches",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    SourceId = c.Int(nullable: false),
                    FileName = c.String(maxLength: 255),
                    HashFileName = c.String(maxLength: 255),
                    Method = c.Int(nullable: false),
                    Status = c.Int(nullable: false),
                    Record = c.Int(nullable: false),
                    UploadedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    Errors = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Sources", t => t.SourceId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.SourceId)
                .Index(t => t.FileName)
                .Index(t => t.HashFileName)
                .Index(t => t.Method)
                .Index(t => t.Status)
                .Index(t => t.Record)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.Sources",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 128),
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
                "dbo.SanctionBirthDates",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    SanctionId = c.Int(nullable: false),
                    DateOfBirth = c.DateTime(precision: 7, storeType: "datetime2"),
                    YearOfBirth = c.Int(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Sanctions", t => t.SanctionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.SanctionId)
                .Index(t => t.DateOfBirth)
                .Index(t => t.YearOfBirth)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.SanctionComments",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    SanctionId = c.Int(nullable: false),
                    Comment = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Sanctions", t => t.SanctionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.SanctionId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.SanctionCountries",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    SanctionId = c.Int(nullable: false),
                    Country = c.String(nullable: false, maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Sanctions", t => t.SanctionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.SanctionId)
                .Index(t => t.Country)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.SanctionFormatNames",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    SanctionId = c.Int(nullable: false),
                    SanctionNameId = c.Int(nullable: false),
                    Type = c.Int(nullable: false),
                    Name = c.String(nullable: false, maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Sanctions", t => t.SanctionId)
                .ForeignKey("dbo.SanctionNames", t => t.SanctionNameId, cascadeDelete: true)
                .Index(t => t.SanctionId)
                .Index(t => t.SanctionNameId)
                .Index(t => t.Type)
                .Index(t => t.Name)
                .Index(t => t.CreatedById);

            CreateTable(
                "dbo.SanctionNames",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    SanctionId = c.Int(nullable: false),
                    Name = c.String(nullable: false, maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Sanctions", t => t.SanctionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.SanctionId)
                .Index(t => t.Name)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.SanctionIdentities",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    SanctionId = c.Int(nullable: false),
                    IdType = c.String(nullable: false, maxLength: 30),
                    IdNumber = c.String(nullable: false, maxLength: 30),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Sanctions", t => t.SanctionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.SanctionId)
                .Index(t => t.IdType)
                .Index(t => t.IdNumber)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.SanctionExclusions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Keyword = c.String(nullable: false, maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Keyword)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.SanctionKeywordDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    SanctionKeywordId = c.Int(nullable: false),
                    Keyword = c.String(nullable: false, maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.SanctionKeywords", t => t.SanctionKeywordId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.SanctionKeywordId)
                .Index(t => t.Keyword)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.SanctionKeywords",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Code = c.String(nullable: false, maxLength: 10),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Code)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.SanctionVerificationDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    SanctionVerificationId = c.Int(nullable: false),
                    ModuleId = c.Int(nullable: false),
                    ObjectId = c.Int(nullable: false),
                    BatchId = c.Int(),
                    UploadDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    CedingCompany = c.String(maxLength: 30),
                    TreatyCode = c.String(maxLength: 35),
                    CedingPlanCode = c.String(maxLength: 30),
                    PolicyNumber = c.String(maxLength: 150),
                    InsuredName = c.String(maxLength: 128),
                    InsuredDateOfBirth = c.DateTime(precision: 7, storeType: "datetime2"),
                    InsuredIcNumber = c.String(maxLength: 15),
                    SoaQuarter = c.String(maxLength: 30),
                    SumReins = c.Double(),
                    ClaimAmount = c.Double(),
                    SanctionRefNumber = c.String(storeType: "ntext"),
                    SanctionIdNumber = c.String(storeType: "ntext"),
                    SanctionAddress = c.String(storeType: "ntext"),
                    LineOfBusiness = c.String(maxLength: 5),
                    PolicyCommencementDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    PolicyStatusCode = c.String(maxLength: 20),
                    RiskCoverageEndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    GrossPremium = c.Double(),
                    IsWhitelist = c.Boolean(nullable: false),
                    IsExactMatch = c.Boolean(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Modules", t => t.ModuleId, cascadeDelete: true)
                .ForeignKey("dbo.SanctionVerifications", t => t.SanctionVerificationId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.SanctionVerificationId)
                .Index(t => t.ModuleId)
                .Index(t => t.ObjectId)
                .Index(t => t.BatchId)
                .Index(t => t.UploadDate)
                .Index(t => t.CedingCompany)
                .Index(t => t.TreatyCode)
                .Index(t => t.CedingPlanCode)
                .Index(t => t.PolicyNumber)
                .Index(t => t.InsuredName)
                .Index(t => t.InsuredDateOfBirth)
                .Index(t => t.InsuredIcNumber)
                .Index(t => t.SoaQuarter)
                .Index(t => t.SumReins)
                .Index(t => t.ClaimAmount)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.SanctionVerifications",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    SourceId = c.Int(nullable: false),
                    IsRiData = c.Boolean(nullable: false),
                    IsClaimRegister = c.Boolean(nullable: false),
                    IsReferralClaim = c.Boolean(nullable: false),
                    Type = c.Int(nullable: false),
                    BatchId = c.Int(),
                    Status = c.Int(nullable: false),
                    Record = c.Int(nullable: false),
                    ProcessStartAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    ProcessEndAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Sources", t => t.SourceId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.SourceId)
                .Index(t => t.Type)
                .Index(t => t.BatchId)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyMarketingAllowanceProvisions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyCodeId = c.Int(nullable: false),
                    Code = c.String(nullable: false, maxLength: 50),
                    Name = c.String(nullable: false, maxLength: 255),
                    LaunchAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    Year1stPremiumRate = c.Double(),
                    RenewalPremiumRate = c.Double(),
                    Type = c.Int(),
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
                .Index(t => t.Code)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            AddColumn("dbo.FinanceProvisionings", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.FinanceProvisionings", "ClaimsProvisionRecord", c => c.Int(nullable: false));
            AddColumn("dbo.FinanceProvisionings", "ClaimsProvisionAmount", c => c.Double(nullable: false));
            AddColumn("dbo.FinanceProvisionings", "DrProvisionRecord", c => c.Int(nullable: false));
            AddColumn("dbo.FinanceProvisionings", "DrProvisionAmount", c => c.Double(nullable: false));
            CreateIndex("dbo.FinanceProvisionings", "Status");
            CreateIndex("dbo.FinanceProvisionings", "ClaimsProvisionRecord");
            CreateIndex("dbo.FinanceProvisionings", "ClaimsProvisionAmount");
            CreateIndex("dbo.FinanceProvisionings", "DrProvisionRecord");
            CreateIndex("dbo.FinanceProvisionings", "DrProvisionAmount");
            DropColumn("dbo.FinanceProvisionings", "CutOffId");
            DropColumn("dbo.FinanceProvisionings", "Record");
            DropColumn("dbo.FinanceProvisionings", "Amount");
        }

        public override void Down()
        {
            AddColumn("dbo.FinanceProvisionings", "Amount", c => c.Double(nullable: false));
            AddColumn("dbo.FinanceProvisionings", "Record", c => c.Int(nullable: false));
            AddColumn("dbo.FinanceProvisionings", "CutOffId", c => c.Int(nullable: false));
            DropForeignKey("dbo.TreatyMarketingAllowanceProvisions", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyMarketingAllowanceProvisions", "TreatyCodeId", "dbo.TreatyCodes");
            DropForeignKey("dbo.TreatyMarketingAllowanceProvisions", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionVerificationDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionVerificationDetails", "SanctionVerificationId", "dbo.SanctionVerifications");
            DropForeignKey("dbo.SanctionVerifications", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionVerifications", "SourceId", "dbo.Sources");
            DropForeignKey("dbo.SanctionVerifications", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionVerificationDetails", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.SanctionVerificationDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionKeywordDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionKeywordDetails", "SanctionKeywordId", "dbo.SanctionKeywords");
            DropForeignKey("dbo.SanctionKeywords", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionKeywords", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionKeywordDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionExclusions", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionExclusions", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionAddresses", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionAddresses", "SanctionId", "dbo.Sanctions");
            DropForeignKey("dbo.Sanctions", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionIdentities", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionIdentities", "SanctionId", "dbo.Sanctions");
            DropForeignKey("dbo.SanctionIdentities", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionFormatNames", "SanctionNameId", "dbo.SanctionNames");
            DropForeignKey("dbo.SanctionNames", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionNames", "SanctionId", "dbo.Sanctions");
            DropForeignKey("dbo.SanctionNames", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionFormatNames", "SanctionId", "dbo.Sanctions");
            DropForeignKey("dbo.SanctionFormatNames", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionCountries", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionCountries", "SanctionId", "dbo.Sanctions");
            DropForeignKey("dbo.SanctionCountries", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionComments", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionComments", "SanctionId", "dbo.Sanctions");
            DropForeignKey("dbo.SanctionComments", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionBirthDates", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionBirthDates", "SanctionId", "dbo.Sanctions");
            DropForeignKey("dbo.SanctionBirthDates", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Sanctions", "SanctionBatchId", "dbo.SanctionBatches");
            DropForeignKey("dbo.SanctionBatches", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionBatches", "SourceId", "dbo.Sources");
            DropForeignKey("dbo.Sources", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.Sources", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionBatches", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Sanctions", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionAddresses", "CreatedById", "dbo.Users");
            DropIndex("dbo.TreatyMarketingAllowanceProvisions", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyMarketingAllowanceProvisions", new[] { "CreatedById" });
            DropIndex("dbo.TreatyMarketingAllowanceProvisions", new[] { "Code" });
            DropIndex("dbo.TreatyMarketingAllowanceProvisions", new[] { "TreatyCodeId" });
            DropIndex("dbo.SanctionVerifications", new[] { "UpdatedById" });
            DropIndex("dbo.SanctionVerifications", new[] { "CreatedById" });
            DropIndex("dbo.SanctionVerifications", new[] { "Status" });
            DropIndex("dbo.SanctionVerifications", new[] { "BatchId" });
            DropIndex("dbo.SanctionVerifications", new[] { "Type" });
            DropIndex("dbo.SanctionVerifications", new[] { "SourceId" });
            DropIndex("dbo.SanctionVerificationDetails", new[] { "UpdatedById" });
            DropIndex("dbo.SanctionVerificationDetails", new[] { "CreatedById" });
            DropIndex("dbo.SanctionVerificationDetails", new[] { "ClaimAmount" });
            DropIndex("dbo.SanctionVerificationDetails", new[] { "SumReins" });
            DropIndex("dbo.SanctionVerificationDetails", new[] { "SoaQuarter" });
            DropIndex("dbo.SanctionVerificationDetails", new[] { "InsuredIcNumber" });
            DropIndex("dbo.SanctionVerificationDetails", new[] { "InsuredDateOfBirth" });
            DropIndex("dbo.SanctionVerificationDetails", new[] { "InsuredName" });
            DropIndex("dbo.SanctionVerificationDetails", new[] { "PolicyNumber" });
            DropIndex("dbo.SanctionVerificationDetails", new[] { "CedingPlanCode" });
            DropIndex("dbo.SanctionVerificationDetails", new[] { "TreatyCode" });
            DropIndex("dbo.SanctionVerificationDetails", new[] { "CedingCompany" });
            DropIndex("dbo.SanctionVerificationDetails", new[] { "UploadDate" });
            DropIndex("dbo.SanctionVerificationDetails", new[] { "BatchId" });
            DropIndex("dbo.SanctionVerificationDetails", new[] { "ObjectId" });
            DropIndex("dbo.SanctionVerificationDetails", new[] { "ModuleId" });
            DropIndex("dbo.SanctionVerificationDetails", new[] { "SanctionVerificationId" });
            DropIndex("dbo.SanctionKeywords", new[] { "UpdatedById" });
            DropIndex("dbo.SanctionKeywords", new[] { "CreatedById" });
            DropIndex("dbo.SanctionKeywords", new[] { "Code" });
            DropIndex("dbo.SanctionKeywordDetails", new[] { "UpdatedById" });
            DropIndex("dbo.SanctionKeywordDetails", new[] { "CreatedById" });
            DropIndex("dbo.SanctionKeywordDetails", new[] { "Keyword" });
            DropIndex("dbo.SanctionKeywordDetails", new[] { "SanctionKeywordId" });
            DropIndex("dbo.SanctionExclusions", new[] { "UpdatedById" });
            DropIndex("dbo.SanctionExclusions", new[] { "CreatedById" });
            DropIndex("dbo.SanctionExclusions", new[] { "Keyword" });
            DropIndex("dbo.SanctionIdentities", new[] { "UpdatedById" });
            DropIndex("dbo.SanctionIdentities", new[] { "CreatedById" });
            DropIndex("dbo.SanctionIdentities", new[] { "IdNumber" });
            DropIndex("dbo.SanctionIdentities", new[] { "IdType" });
            DropIndex("dbo.SanctionIdentities", new[] { "SanctionId" });
            DropIndex("dbo.SanctionNames", new[] { "UpdatedById" });
            DropIndex("dbo.SanctionNames", new[] { "CreatedById" });
            DropIndex("dbo.SanctionNames", new[] { "Name" });
            DropIndex("dbo.SanctionNames", new[] { "SanctionId" });
            DropIndex("dbo.SanctionFormatNames", new[] { "CreatedById" });
            DropIndex("dbo.SanctionFormatNames", new[] { "Name" });
            DropIndex("dbo.SanctionFormatNames", new[] { "Type" });
            DropIndex("dbo.SanctionFormatNames", new[] { "SanctionNameId" });
            DropIndex("dbo.SanctionFormatNames", new[] { "SanctionId" });
            DropIndex("dbo.SanctionCountries", new[] { "UpdatedById" });
            DropIndex("dbo.SanctionCountries", new[] { "CreatedById" });
            DropIndex("dbo.SanctionCountries", new[] { "Country" });
            DropIndex("dbo.SanctionCountries", new[] { "SanctionId" });
            DropIndex("dbo.SanctionComments", new[] { "UpdatedById" });
            DropIndex("dbo.SanctionComments", new[] { "CreatedById" });
            DropIndex("dbo.SanctionComments", new[] { "SanctionId" });
            DropIndex("dbo.SanctionBirthDates", new[] { "UpdatedById" });
            DropIndex("dbo.SanctionBirthDates", new[] { "CreatedById" });
            DropIndex("dbo.SanctionBirthDates", new[] { "YearOfBirth" });
            DropIndex("dbo.SanctionBirthDates", new[] { "DateOfBirth" });
            DropIndex("dbo.SanctionBirthDates", new[] { "SanctionId" });
            DropIndex("dbo.Sources", new[] { "UpdatedById" });
            DropIndex("dbo.Sources", new[] { "CreatedById" });
            DropIndex("dbo.Sources", new[] { "Name" });
            DropIndex("dbo.SanctionBatches", new[] { "UpdatedById" });
            DropIndex("dbo.SanctionBatches", new[] { "CreatedById" });
            DropIndex("dbo.SanctionBatches", new[] { "Record" });
            DropIndex("dbo.SanctionBatches", new[] { "Status" });
            DropIndex("dbo.SanctionBatches", new[] { "Method" });
            DropIndex("dbo.SanctionBatches", new[] { "HashFileName" });
            DropIndex("dbo.SanctionBatches", new[] { "FileName" });
            DropIndex("dbo.SanctionBatches", new[] { "SourceId" });
            DropIndex("dbo.Sanctions", new[] { "UpdatedById" });
            DropIndex("dbo.Sanctions", new[] { "CreatedById" });
            DropIndex("dbo.Sanctions", new[] { "RefNumber" });
            DropIndex("dbo.Sanctions", new[] { "Name" });
            DropIndex("dbo.Sanctions", new[] { "Category" });
            DropIndex("dbo.Sanctions", new[] { "PublicationInformation" });
            DropIndex("dbo.Sanctions", new[] { "SanctionBatchId" });
            DropIndex("dbo.SanctionAddresses", new[] { "UpdatedById" });
            DropIndex("dbo.SanctionAddresses", new[] { "CreatedById" });
            DropIndex("dbo.SanctionAddresses", new[] { "SanctionId" });
            DropIndex("dbo.FinanceProvisionings", new[] { "DrProvisionAmount" });
            DropIndex("dbo.FinanceProvisionings", new[] { "DrProvisionRecord" });
            DropIndex("dbo.FinanceProvisionings", new[] { "ClaimsProvisionAmount" });
            DropIndex("dbo.FinanceProvisionings", new[] { "ClaimsProvisionRecord" });
            DropIndex("dbo.FinanceProvisionings", new[] { "Status" });
            DropColumn("dbo.FinanceProvisionings", "DrProvisionAmount");
            DropColumn("dbo.FinanceProvisionings", "DrProvisionRecord");
            DropColumn("dbo.FinanceProvisionings", "ClaimsProvisionAmount");
            DropColumn("dbo.FinanceProvisionings", "ClaimsProvisionRecord");
            DropColumn("dbo.FinanceProvisionings", "Status");
            DropTable("dbo.TreatyMarketingAllowanceProvisions");
            DropTable("dbo.SanctionVerifications");
            DropTable("dbo.SanctionVerificationDetails");
            DropTable("dbo.SanctionKeywords");
            DropTable("dbo.SanctionKeywordDetails");
            DropTable("dbo.SanctionExclusions");
            DropTable("dbo.SanctionIdentities");
            DropTable("dbo.SanctionNames");
            DropTable("dbo.SanctionFormatNames");
            DropTable("dbo.SanctionCountries");
            DropTable("dbo.SanctionComments");
            DropTable("dbo.SanctionBirthDates");
            DropTable("dbo.Sources");
            DropTable("dbo.SanctionBatches");
            DropTable("dbo.Sanctions");
            DropTable("dbo.SanctionAddresses");
            CreateIndex("dbo.FinanceProvisionings", "Amount");
            CreateIndex("dbo.FinanceProvisionings", "Record");
            CreateIndex("dbo.FinanceProvisionings", "CutOffId");
            AddForeignKey("dbo.FinanceProvisionings", "CutOffId", "dbo.CutOff", "Id");
        }
    }
}
