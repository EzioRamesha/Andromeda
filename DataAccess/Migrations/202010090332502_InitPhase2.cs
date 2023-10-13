namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class InitPhase2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Mfrs17ReportingDetailRiDatas", "RiDataId", "dbo.RiData");
            DropForeignKey("dbo.RateTables", "CedingOccupationCodePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.RateTables", "CedingTobaccoUsePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.RateTables", "InsuredGenderCodePickListDetailId", "dbo.PickListDetails");
            DropIndex("dbo.Benefits", new[] { "Type" });
            DropIndex("dbo.Benefits", new[] { "Description" });
            DropIndex("dbo.Mfrs17ReportingDetailRiDatas", new[] { "RiDataId" });
            DropIndex("dbo.RiData", new[] { "RiDataFileId" });
            DropIndex("dbo.RiData", new[] { "ComputationStatus" });
            DropIndex("dbo.RateTables", new[] { "RateTableCode" });
            DropIndex("dbo.RateTables", new[] { "CedingOccupationCodePickListDetailId" });
            DropIndex("dbo.RateTables", new[] { "InsuredGenderCodePickListDetailId" });
            DropIndex("dbo.RateTables", new[] { "CedingTobaccoUsePickListDetailId" });
            DropIndex("dbo.RiDataComputations", new[] { "StandardOutputId" });
            DropPrimaryKey("dbo.Mfrs17ReportingDetailRiDatas");
            CreateTable(
                "dbo.AccountCodeMappings",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyTypePickListDetailId = c.Int(nullable: false),
                    ClaimCodeId = c.Int(),
                    AccountCodeId = c.Int(nullable: false),
                    TransactionTypeCodePickListDetailId = c.Int(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccountCodes", t => t.AccountCodeId)
                .ForeignKey("dbo.ClaimCodes", t => t.ClaimCodeId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickListDetails", t => t.TransactionTypeCodePickListDetailId)
                .ForeignKey("dbo.PickListDetails", t => t.TreatyTypePickListDetailId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyTypePickListDetailId)
                .Index(t => t.ClaimCodeId)
                .Index(t => t.AccountCodeId)
                .Index(t => t.TransactionTypeCodePickListDetailId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.AccountCodes",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Code = c.String(nullable: false, maxLength: 30),
                    ReportingType = c.Int(nullable: false),
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
                .Index(t => t.ReportingType)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.ClaimCodes",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Code = c.String(nullable: false, maxLength: 30),
                    Description = c.String(nullable: false, maxLength: 255),
                    Status = c.Int(nullable: false),
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
                "dbo.StandardClaimDataOutputs",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Type = c.Int(nullable: false),
                    DataType = c.Int(nullable: false),
                    Code = c.String(maxLength: 128),
                    Name = c.String(maxLength: 128),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.StandardSoaDataOutputs",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Type = c.Int(nullable: false),
                    DataType = c.Int(nullable: false),
                    Code = c.String(maxLength: 128),
                    Name = c.String(maxLength: 128),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.AnnuityFactorDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    AnnuityFactorId = c.Int(nullable: false),
                    PolicyTermRemain = c.Int(nullable: false),
                    AnnuityFactorValue = c.Double(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AnnuityFactors", t => t.AnnuityFactorId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.AnnuityFactorId)
                .Index(t => t.PolicyTermRemain)
                .Index(t => t.AnnuityFactorValue)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.AnnuityFactors",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CedantId = c.Int(nullable: false),
                    CedingPlanCode = c.String(storeType: "ntext"),
                    ReinsEffDatePolStartDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    ReinsEffDatePolEndDate = c.DateTime(precision: 7, storeType: "datetime2"),
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
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.AnnuityFactorMappings",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    AnnuityFactorId = c.Int(nullable: false),
                    Combination = c.String(nullable: false, maxLength: 255),
                    CedingPlanCode = c.String(nullable: false, maxLength: 30),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AnnuityFactors", t => t.AnnuityFactorId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .Index(t => t.AnnuityFactorId)
                .Index(t => t.Combination)
                .Index(t => t.CedingPlanCode)
                .Index(t => t.CreatedById);

            CreateTable(
                "dbo.AuthorizationLimits",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    AccessGroupId = c.Int(nullable: false),
                    PositiveAmountFrom = c.Double(),
                    PositiveAmountTo = c.Double(),
                    NegativeAmountFrom = c.Double(),
                    NegativeAmountTo = c.Double(),
                    Percentage = c.Double(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccessGroups", t => t.AccessGroupId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.AccessGroupId)
                .Index(t => t.PositiveAmountFrom)
                .Index(t => t.PositiveAmountTo)
                .Index(t => t.NegativeAmountFrom)
                .Index(t => t.NegativeAmountTo)
                .Index(t => t.Percentage)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.BenefitDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    BenefitId = c.Int(nullable: false),
                    ClaimCodeId = c.Int(nullable: false),
                    EventCodeId = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Benefits", t => t.BenefitId)
                .ForeignKey("dbo.ClaimCodes", t => t.ClaimCodeId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.EventCodes", t => t.EventCodeId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.BenefitId)
                .Index(t => t.ClaimCodeId)
                .Index(t => t.EventCodeId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.EventCodes",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Code = c.String(nullable: false, maxLength: 30),
                    Description = c.String(nullable: false, maxLength: 255),
                    Status = c.Int(nullable: false),
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
                "dbo.CedantWorkgroupCedants",
                c => new
                {
                    CedantWorkgroupId = c.Int(nullable: false),
                    CedantId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.CedantWorkgroupId, t.CedantId })
                .ForeignKey("dbo.Cedants", t => t.CedantId)
                .ForeignKey("dbo.CedantWorkgroups", t => t.CedantWorkgroupId)
                .Index(t => t.CedantWorkgroupId)
                .Index(t => t.CedantId);

            CreateTable(
                "dbo.CedantWorkgroups",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Code = c.String(nullable: false, maxLength: 20),
                    Description = c.String(nullable: false),
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
                "dbo.CedantWorkgroupUsers",
                c => new
                {
                    CedantWorkgroupId = c.Int(nullable: false),
                    UserId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.CedantWorkgroupId, t.UserId })
                .ForeignKey("dbo.CedantWorkgroups", t => t.CedantWorkgroupId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.CedantWorkgroupId)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.ClaimAuthorityLimitCedantDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ClaimAuthorityLimitCedantId = c.Int(nullable: false),
                    ClaimCodeId = c.Int(nullable: false),
                    ClaimAuthorityLimitValue = c.Double(nullable: false),
                    FundAccountingCode = c.Int(nullable: false),
                    EffectiveDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClaimAuthorityLimitCedants", t => t.ClaimAuthorityLimitCedantId)
                .ForeignKey("dbo.ClaimCodes", t => t.ClaimCodeId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .Index(t => t.ClaimAuthorityLimitCedantId)
                .Index(t => t.ClaimCodeId)
                .Index(t => t.CreatedById);

            CreateTable(
                "dbo.ClaimAuthorityLimitCedants",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CedantId = c.Int(nullable: false),
                    Remarks = c.String(maxLength: 255),
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
                .Index(t => t.Remarks)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.ClaimAuthorityLimitMLRe",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DepartmentId = c.Int(nullable: false),
                    UserId = c.Int(nullable: false),
                    Position = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.DepartmentId)
                .Index(t => t.UserId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.ClaimAuthorityLimitMLReDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ClaimAuthorityLimitMLReId = c.Int(nullable: false),
                    ClaimCodeId = c.Int(nullable: false),
                    ClaimAuthorityLimitValue = c.Double(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClaimAuthorityLimitMLRe", t => t.ClaimAuthorityLimitMLReId)
                .ForeignKey("dbo.ClaimCodes", t => t.ClaimCodeId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .Index(t => t.ClaimAuthorityLimitMLReId)
                .Index(t => t.ClaimCodeId)
                .Index(t => t.CreatedById);

            CreateTable(
                "dbo.ClaimCategories",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Category = c.String(nullable: false, maxLength: 255),
                    Remark = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Category)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.ClaimChecklistDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ClaimChecklistId = c.Int(nullable: false),
                    Name = c.String(maxLength: 255),
                    Remark = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClaimChecklists", t => t.ClaimChecklistId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.ClaimChecklistId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.ClaimChecklists",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ClaimCodeId = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClaimCodes", t => t.ClaimCodeId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.ClaimCodeId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.ClaimCodeMappingDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ClaimCodeMappingId = c.Int(nullable: false),
                    Combination = c.String(nullable: false, maxLength: 255),
                    MlreEventCode = c.String(maxLength: 10),
                    MlreBenefitCode = c.String(maxLength: 10),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClaimCodeMappings", t => t.ClaimCodeMappingId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .Index(t => t.ClaimCodeMappingId)
                .Index(t => t.Combination)
                .Index(t => t.MlreEventCode)
                .Index(t => t.MlreBenefitCode)
                .Index(t => t.CreatedById);

            CreateTable(
                "dbo.ClaimCodeMappings",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    MlreEventCode = c.String(storeType: "ntext"),
                    MlreBenefitCode = c.String(storeType: "ntext"),
                    ClaimCodeId = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClaimCodes", t => t.ClaimCodeId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.ClaimCodeId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.ClaimData",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ClaimDataBatchId = c.Int(nullable: false),
                    ClaimDataFileId = c.Int(),
                    ClaimId = c.String(maxLength: 30),
                    ClaimCode = c.String(maxLength: 30),
                    CopyAndOverwriteData = c.Boolean(nullable: false),
                    MappingStatus = c.Int(nullable: false),
                    PreComputationStatus = c.Int(nullable: false),
                    PreValidationStatus = c.Int(nullable: false),
                    Errors = c.String(storeType: "ntext"),
                    CustomField = c.String(storeType: "ntext"),
                    PolicyNumber = c.String(maxLength: 150),
                    PolicyTerm = c.Int(),
                    ClaimRecoveryAmt = c.Double(),
                    ClaimTransactionType = c.String(maxLength: 30),
                    TreatyCode = c.String(maxLength: 35),
                    TreatyType = c.String(maxLength: 35),
                    AarPayable = c.Double(),
                    AnnualRiPrem = c.Double(),
                    CauseOfEvent = c.String(maxLength: 255),
                    CedantClaimEventCode = c.String(maxLength: 30),
                    CedantClaimType = c.String(maxLength: 30),
                    CedantDateOfNotification = c.DateTime(),
                    CedingBenefitRiskCode = c.String(maxLength: 30),
                    CedingBenefitTypeCode = c.String(maxLength: 30),
                    CedingClaimType = c.String(maxLength: 30),
                    CedingCompany = c.String(maxLength: 30),
                    CedingEventCode = c.String(maxLength: 30),
                    CedingPlanCode = c.String(maxLength: 30),
                    CurrencyRate = c.Double(),
                    CurrencyCode = c.String(maxLength: 3),
                    DateApproved = c.DateTime(),
                    DateOfEvent = c.DateTime(),
                    EntryNo = c.String(maxLength: 30),
                    ExGratia = c.Double(),
                    ForeignClaimRecoveryAmt = c.Double(),
                    FundsAccountingTypeCode = c.String(maxLength: 30),
                    InsuredDateOfBirth = c.DateTime(),
                    InsuredGenderCode = c.String(maxLength: 1),
                    InsuredName = c.String(maxLength: 128),
                    InsuredTobaccoUse = c.String(maxLength: 1),
                    LastTransactionDate = c.DateTime(),
                    LastTransactionQuarter = c.String(maxLength: 30),
                    LateInterest = c.Double(),
                    Layer1SumRein = c.Double(),
                    Mfrs17AnnualCohort = c.Int(),
                    Mfrs17ContractCode = c.String(maxLength: 30),
                    MlreBenefitCode = c.String(maxLength: 30),
                    MlreEventCode = c.String(maxLength: 30),
                    MlreInvoiceDate = c.DateTime(),
                    MlreInvoiceNumber = c.String(maxLength: 30),
                    MlreRetainAmount = c.Double(),
                    MlreShare = c.Double(),
                    PendingProvisionDay = c.Int(),
                    PolicyDuration = c.Int(),
                    ReinsBasisCode = c.String(maxLength: 30),
                    ReinsEffDatePol = c.DateTime(),
                    RetroParty1 = c.String(maxLength: 128),
                    RetroParty2 = c.String(maxLength: 128),
                    RetroParty3 = c.String(maxLength: 128),
                    RetroRecovery1 = c.Double(),
                    RetroRecovery2 = c.Double(),
                    RetroRecovery3 = c.Double(),
                    RetroStatementDate1 = c.DateTime(),
                    RetroStatementDate2 = c.DateTime(),
                    RetroStatementDate3 = c.DateTime(),
                    RetroStatementId1 = c.String(maxLength: 30),
                    RetroStatementId2 = c.String(maxLength: 30),
                    RetroStatementId3 = c.String(maxLength: 30),
                    RiskPeriodMonth = c.Int(),
                    RiskPeriodYear = c.Int(),
                    RiskQuarter = c.String(maxLength: 30),
                    SaFactor = c.Double(),
                    SoaQuarter = c.String(maxLength: 30),
                    SumIns = c.Double(),
                    TempA1 = c.Double(),
                    TempA2 = c.Double(),
                    TempD1 = c.DateTime(),
                    TempD2 = c.DateTime(),
                    TempI1 = c.Int(),
                    TempI2 = c.Int(),
                    TempS1 = c.String(maxLength: 50),
                    TempS2 = c.String(maxLength: 50),
                    TransactionDateWop = c.DateTime(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                    IssueDatePol = c.DateTime(precision: 7, storeType: "datetime2"),
                    DateOfReported = c.DateTime(),
                    CedingTreatyCode = c.String(maxLength: 30),
                    CampaignCode = c.String(maxLength: 10),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClaimDataBatches", t => t.ClaimDataBatchId)
                .ForeignKey("dbo.ClaimDataFiles", t => t.ClaimDataFileId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.ClaimDataBatchId)
                .Index(t => t.ClaimDataFileId)
                .Index(t => t.ClaimId)
                .Index(t => t.ClaimCode)
                .Index(t => t.MappingStatus)
                .Index(t => t.PreComputationStatus)
                .Index(t => t.PreValidationStatus)
                .Index(t => t.PolicyNumber)
                .Index(t => t.PolicyTerm)
                .Index(t => t.ClaimRecoveryAmt)
                .Index(t => t.ClaimTransactionType)
                .Index(t => t.TreatyCode)
                .Index(t => t.TreatyType)
                .Index(t => t.AarPayable)
                .Index(t => t.AnnualRiPrem)
                .Index(t => t.CauseOfEvent)
                .Index(t => t.CedantClaimEventCode)
                .Index(t => t.CedantClaimType)
                .Index(t => t.CedantDateOfNotification)
                .Index(t => t.CedingBenefitRiskCode)
                .Index(t => t.CedingBenefitTypeCode)
                .Index(t => t.CedingClaimType)
                .Index(t => t.CedingCompany)
                .Index(t => t.CedingEventCode)
                .Index(t => t.CedingPlanCode)
                .Index(t => t.CurrencyRate)
                .Index(t => t.CurrencyCode)
                .Index(t => t.DateApproved)
                .Index(t => t.DateOfEvent)
                .Index(t => t.EntryNo)
                .Index(t => t.ExGratia)
                .Index(t => t.ForeignClaimRecoveryAmt)
                .Index(t => t.FundsAccountingTypeCode)
                .Index(t => t.InsuredDateOfBirth)
                .Index(t => t.InsuredGenderCode)
                .Index(t => t.InsuredName)
                .Index(t => t.InsuredTobaccoUse)
                .Index(t => t.LastTransactionDate)
                .Index(t => t.LastTransactionQuarter)
                .Index(t => t.LateInterest)
                .Index(t => t.Layer1SumRein)
                .Index(t => t.Mfrs17AnnualCohort)
                .Index(t => t.Mfrs17ContractCode)
                .Index(t => t.MlreBenefitCode)
                .Index(t => t.MlreEventCode)
                .Index(t => t.MlreInvoiceDate)
                .Index(t => t.MlreInvoiceNumber)
                .Index(t => t.MlreRetainAmount)
                .Index(t => t.MlreShare)
                .Index(t => t.PendingProvisionDay)
                .Index(t => t.PolicyDuration)
                .Index(t => t.ReinsBasisCode)
                .Index(t => t.ReinsEffDatePol)
                .Index(t => t.RetroParty1)
                .Index(t => t.RetroParty2)
                .Index(t => t.RetroParty3)
                .Index(t => t.RetroRecovery1)
                .Index(t => t.RetroRecovery2)
                .Index(t => t.RetroRecovery3)
                .Index(t => t.RetroStatementDate1)
                .Index(t => t.RetroStatementDate2)
                .Index(t => t.RetroStatementDate3)
                .Index(t => t.RetroStatementId1)
                .Index(t => t.RetroStatementId2)
                .Index(t => t.RetroStatementId3)
                .Index(t => t.RiskPeriodMonth)
                .Index(t => t.RiskPeriodYear)
                .Index(t => t.RiskQuarter)
                .Index(t => t.SaFactor)
                .Index(t => t.SoaQuarter)
                .Index(t => t.SumIns)
                .Index(t => t.TempA1)
                .Index(t => t.TempA2)
                .Index(t => t.TempD1)
                .Index(t => t.TempD2)
                .Index(t => t.TempI1)
                .Index(t => t.TempI2)
                .Index(t => t.TempS1)
                .Index(t => t.TempS2)
                .Index(t => t.TransactionDateWop)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById)
                .Index(t => t.DateOfReported)
                .Index(t => t.CedingTreatyCode)
                .Index(t => t.CampaignCode);

            CreateTable(
                "dbo.ClaimDataBatches",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Status = c.Int(nullable: false),
                    CedantId = c.Int(nullable: false),
                    TreatyId = c.Int(),
                    ClaimDataConfigId = c.Int(nullable: false),
                    SoaDataBatchId = c.Int(),
                    Configs = c.String(storeType: "ntext"),
                    OverrideProperties = c.String(storeType: "ntext"),
                    Quarter = c.String(maxLength: 64),
                    TotalMappingFailedStatus = c.Int(nullable: false),
                    TotalPreComputationFailedStatus = c.Int(nullable: false),
                    TotalPreValidationFailedStatus = c.Int(nullable: false),
                    ReceivedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cedants", t => t.CedantId)
                .ForeignKey("dbo.ClaimDataConfigs", t => t.ClaimDataConfigId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.SoaDataBatches", t => t.SoaDataBatchId)
                .ForeignKey("dbo.Treaties", t => t.TreatyId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Status)
                .Index(t => t.CedantId)
                .Index(t => t.TreatyId)
                .Index(t => t.ClaimDataConfigId)
                .Index(t => t.SoaDataBatchId)
                .Index(t => t.Quarter)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.ClaimDataConfigs",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CedantId = c.Int(nullable: false),
                    TreatyCodeId = c.Int(),
                    TreatyId = c.Int(),
                    Status = c.Int(nullable: false),
                    Code = c.String(nullable: false, maxLength: 64),
                    Name = c.String(nullable: false, maxLength: 255),
                    FileType = c.Int(nullable: false),
                    Configs = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cedants", t => t.CedantId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Treaties", t => t.TreatyId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.CedantId)
                .Index(t => t.TreatyId)
                .Index(t => t.Code)
                .Index(t => t.Name)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.SoaDataBatches",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CedantId = c.Int(nullable: false),
                    TreatyId = c.Int(),
                    Quarter = c.String(maxLength: 64),
                    CurrencyCodePickListDetailId = c.Int(),
                    CurrencyRate = c.Double(),
                    Type = c.Int(nullable: false),
                    Status = c.Int(nullable: false),
                    DataUpdateStatus = c.Int(nullable: false),
                    StatementReceivedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    RiDataBatchId = c.Int(),
                    ClaimDataBatchId = c.Int(),
                    DirectStatus = c.Int(nullable: false),
                    InvoiceStatus = c.Int(nullable: false),
                    TotalRecords = c.Int(nullable: false),
                    TotalMappingFailedStatus = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cedants", t => t.CedantId)
                .ForeignKey("dbo.ClaimDataBatches", t => t.ClaimDataBatchId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickListDetails", t => t.CurrencyCodePickListDetailId)
                .ForeignKey("dbo.RiDataBatches", t => t.RiDataBatchId)
                .ForeignKey("dbo.Treaties", t => t.TreatyId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.CedantId)
                .Index(t => t.TreatyId)
                .Index(t => t.Quarter)
                .Index(t => t.CurrencyCodePickListDetailId)
                .Index(t => t.Type)
                .Index(t => t.Status)
                .Index(t => t.DataUpdateStatus)
                .Index(t => t.RiDataBatchId)
                .Index(t => t.ClaimDataBatchId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.ClaimDataFiles",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ClaimDataBatchId = c.Int(nullable: false),
                    RawFileId = c.Int(nullable: false),
                    TreatyId = c.Int(),
                    ClaimDataConfigId = c.Int(),
                    CurrencyCodeId = c.Int(),
                    CurrencyRate = c.Double(),
                    Status = c.Int(nullable: false),
                    Mode = c.Int(nullable: false),
                    Configs = c.String(storeType: "ntext"),
                    OverrideProperties = c.String(storeType: "ntext"),
                    Errors = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClaimDataBatches", t => t.ClaimDataBatchId)
                .ForeignKey("dbo.ClaimDataConfigs", t => t.ClaimDataConfigId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickListDetails", t => t.CurrencyCodeId)
                .ForeignKey("dbo.RawFiles", t => t.RawFileId)
                .ForeignKey("dbo.Treaties", t => t.TreatyId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.ClaimDataBatchId)
                .Index(t => t.RawFileId)
                .Index(t => t.TreatyId)
                .Index(t => t.ClaimDataConfigId)
                .Index(t => t.CurrencyCodeId)
                .Index(t => t.CurrencyRate)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.ClaimDataBatchStatusFiles",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ClaimDataBatchId = c.Int(nullable: false),
                    StatusHistoryId = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClaimDataBatches", t => t.ClaimDataBatchId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.StatusHistories", t => t.StatusHistoryId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.ClaimDataBatchId)
                .Index(t => t.StatusHistoryId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.ClaimDataComputations",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ClaimDataConfigId = c.Int(nullable: false),
                    Step = c.Int(nullable: false),
                    SortIndex = c.Int(nullable: false),
                    Description = c.String(nullable: false, maxLength: 128),
                    Condition = c.String(nullable: false, unicode: false, storeType: "text"),
                    StandardClaimDataOutputId = c.Int(nullable: false),
                    Mode = c.Int(nullable: false),
                    CalculationFormula = c.String(nullable: false, maxLength: 128),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClaimDataConfigs", t => t.ClaimDataConfigId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.StandardClaimDataOutputs", t => t.StandardClaimDataOutputId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.ClaimDataConfigId)
                .Index(t => t.Description)
                .Index(t => t.StandardClaimDataOutputId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.ClaimDataMappingDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ClaimDataMappingId = c.Int(nullable: false),
                    PickListDetailId = c.Int(),
                    RawValue = c.String(maxLength: 128),
                    IsPickDetailIdEmpty = c.Boolean(nullable: false),
                    IsRawValueEmpty = c.Boolean(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClaimDataMappings", t => t.ClaimDataMappingId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickListDetails", t => t.PickListDetailId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.ClaimDataMappingId)
                .Index(t => t.PickListDetailId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.ClaimDataMappings",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ClaimDataConfigId = c.Int(nullable: false),
                    StandardClaimDataOutputId = c.Int(nullable: false),
                    SortIndex = c.Int(nullable: false),
                    Row = c.Int(nullable: false),
                    RawColumnName = c.String(maxLength: 128),
                    Length = c.Int(),
                    TransformFormula = c.Int(nullable: false),
                    DefaultValue = c.String(maxLength: 128),
                    DefaultObjectId = c.Int(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClaimDataConfigs", t => t.ClaimDataConfigId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.StandardClaimDataOutputs", t => t.StandardClaimDataOutputId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.ClaimDataConfigId)
                .Index(t => t.StandardClaimDataOutputId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.ClaimDataValidations",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ClaimDataConfigId = c.Int(nullable: false),
                    Step = c.Int(nullable: false),
                    SortIndex = c.Int(nullable: false),
                    Description = c.String(nullable: false, maxLength: 255),
                    Condition = c.String(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClaimDataConfigs", t => t.ClaimDataConfigId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.ClaimDataConfigId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.ClaimReasons",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Type = c.Int(nullable: false),
                    Reason = c.String(maxLength: 255),
                    Remark = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Type)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.ClaimRegister",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ClaimDataBatchId = c.Int(),
                    ClaimDataId = c.Int(),
                    ClaimDataConfigId = c.Int(),
                    RiDataId = c.Int(),
                    RiDataWarehouseId = c.Int(),
                    ReferralRiDataId = c.Int(),
                    OriginalClaimRegisterId = c.Int(),
                    ClaimReasonId = c.Int(),
                    PicClaimId = c.Int(),
                    PicDaaId = c.Int(),
                    ClaimStatus = c.Int(nullable: false),
                    ProvisionStatus = c.Int(nullable: false),
                    OffsetStatus = c.Int(nullable: false),
                    MappingStatus = c.Int(nullable: false),
                    ProcessingStatus = c.Int(nullable: false),
                    DuplicationCheckStatus = c.Int(nullable: false),
                    PostComputationStatus = c.Int(nullable: false),
                    PostValidationStatus = c.Int(nullable: false),
                    Errors = c.String(storeType: "ntext"),
                    ProvisionErrors = c.String(storeType: "ntext"),
                    RedFlagWarnings = c.String(storeType: "ntext"),
                    IsReferralCase = c.Boolean(nullable: false),
                    HasRedFlag = c.Boolean(nullable: false),
                    TargetDateToIssueInvoice = c.DateTime(),
                    ClaimId = c.String(maxLength: 30),
                    ClaimCode = c.String(maxLength: 30),
                    PolicyNumber = c.String(maxLength: 150),
                    PolicyTerm = c.Int(),
                    ClaimRecoveryAmt = c.Double(),
                    ClaimTransactionType = c.String(maxLength: 30),
                    TreatyCode = c.String(maxLength: 35),
                    TreatyType = c.String(maxLength: 35),
                    AarPayable = c.Double(),
                    AnnualRiPrem = c.Double(),
                    CauseOfEvent = c.String(maxLength: 255),
                    CedantClaimEventCode = c.String(maxLength: 30),
                    CedantClaimType = c.String(maxLength: 30),
                    CedantDateOfNotification = c.DateTime(),
                    CedingBenefitRiskCode = c.String(maxLength: 30),
                    CedingBenefitTypeCode = c.String(maxLength: 30),
                    CedingClaimType = c.String(maxLength: 30),
                    CedingCompany = c.String(maxLength: 30),
                    CedingEventCode = c.String(maxLength: 30),
                    CedingPlanCode = c.String(maxLength: 30),
                    CurrencyRate = c.Double(),
                    CurrencyCode = c.String(maxLength: 3),
                    DateApproved = c.DateTime(),
                    DateOfEvent = c.DateTime(),
                    DateOfRegister = c.DateTime(),
                    DateOfReported = c.DateTime(),
                    EntryNo = c.String(maxLength: 30),
                    ExGratia = c.Double(),
                    ForeignClaimRecoveryAmt = c.Double(),
                    FundsAccountingTypeCode = c.String(maxLength: 30),
                    InsuredDateOfBirth = c.DateTime(),
                    InsuredGenderCode = c.String(maxLength: 1),
                    InsuredName = c.String(maxLength: 128),
                    InsuredTobaccoUse = c.String(maxLength: 1),
                    LastTransactionDate = c.DateTime(),
                    LastTransactionQuarter = c.String(maxLength: 30),
                    LateInterest = c.Double(),
                    Layer1SumRein = c.Double(),
                    Mfrs17AnnualCohort = c.Int(),
                    Mfrs17ContractCode = c.String(maxLength: 30),
                    MlreBenefitCode = c.String(maxLength: 30),
                    MlreEventCode = c.String(maxLength: 30),
                    MlreInvoiceDate = c.DateTime(),
                    MlreInvoiceNumber = c.String(maxLength: 30),
                    MlreRetainAmount = c.Double(),
                    MlreShare = c.Double(),
                    PendingProvisionDay = c.Int(),
                    PolicyDuration = c.Int(),
                    RecordType = c.String(maxLength: 30),
                    ReinsBasisCode = c.String(maxLength: 30),
                    ReinsEffDatePol = c.DateTime(),
                    RetroParty1 = c.String(maxLength: 128),
                    RetroParty2 = c.String(maxLength: 128),
                    RetroParty3 = c.String(maxLength: 128),
                    RetroRecovery1 = c.Double(),
                    RetroRecovery2 = c.Double(),
                    RetroRecovery3 = c.Double(),
                    RetroStatementDate1 = c.DateTime(),
                    RetroStatementDate2 = c.DateTime(),
                    RetroStatementDate3 = c.DateTime(),
                    RetroShare1 = c.Double(),
                    RetroShare2 = c.Double(),
                    RetroShare3 = c.Double(),
                    RetroStatementId1 = c.String(maxLength: 30),
                    RetroStatementId2 = c.String(maxLength: 30),
                    RetroStatementId3 = c.String(maxLength: 30),
                    RiskPeriodMonth = c.Int(),
                    RiskPeriodYear = c.Int(),
                    RiskQuarter = c.String(maxLength: 30),
                    SaFactor = c.Double(),
                    SoaQuarter = c.String(maxLength: 30),
                    SumIns = c.Double(),
                    TempA1 = c.Double(),
                    TempA2 = c.Double(),
                    TempD1 = c.DateTime(),
                    TempD2 = c.DateTime(),
                    TempI1 = c.Int(),
                    TempI2 = c.Int(),
                    TempS1 = c.String(maxLength: 50),
                    TempS2 = c.String(maxLength: 50),
                    TransactionDateWop = c.DateTime(),
                    MlreReferenceNo = c.String(maxLength: 30),
                    AddInfo = c.String(maxLength: 30),
                    Remark1 = c.String(maxLength: 128),
                    Remark2 = c.String(maxLength: 128),
                    IssueDatePol = c.DateTime(precision: 7, storeType: "datetime2"),
                    PolicyExpiryDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    ClaimAssessorId = c.Int(),
                    Comment = c.String(maxLength: 128),
                    RequestUnderwriterReview = c.Boolean(nullable: false),
                    UnderwriterFeedback = c.Int(),
                    EventChronologyComment = c.String(),
                    ClaimAssessorRecommendation = c.String(),
                    ClaimCommitteeComment1 = c.String(),
                    ClaimCommitteeComment2 = c.String(),
                    ClaimCommitteeUser1Id = c.Int(),
                    ClaimCommitteeUser2Id = c.Int(),
                    ClaimCommitteeDateCommented1 = c.DateTime(),
                    ClaimCommitteeDateCommented2 = c.DateTime(),
                    CeoClaimReasonId = c.Int(),
                    CeoComment = c.String(),
                    UpdatedOnBehalfById = c.Int(),
                    UpdatedOnBehalfAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    Checklist = c.String(storeType: "ntext"),
                    SignOffById = c.Int(),
                    SignOffDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    CedingTreatyCode = c.String(maxLength: 30),
                    CampaignCode = c.String(maxLength: 10),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClaimReasons", t => t.CeoClaimReasonId)
                .ForeignKey("dbo.Users", t => t.ClaimAssessorId)
                .ForeignKey("dbo.Users", t => t.ClaimCommitteeUser1Id)
                .ForeignKey("dbo.Users", t => t.ClaimCommitteeUser2Id)
                .ForeignKey("dbo.ClaimData", t => t.ClaimDataId)
                .ForeignKey("dbo.ClaimDataBatches", t => t.ClaimDataBatchId)
                .ForeignKey("dbo.ClaimDataConfigs", t => t.ClaimDataConfigId)
                .ForeignKey("dbo.ClaimReasons", t => t.ClaimReasonId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.ClaimRegister", t => t.OriginalClaimRegisterId)
                .ForeignKey("dbo.Users", t => t.PicClaimId)
                .ForeignKey("dbo.Users", t => t.PicDaaId)
                .ForeignKey("dbo.ReferralRiData", t => t.ReferralRiDataId)
                .ForeignKey("dbo.RiDataWarehouse", t => t.RiDataWarehouseId)
                .ForeignKey("dbo.Users", t => t.SignOffById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedOnBehalfById)
                .Index(t => t.ClaimDataBatchId)
                .Index(t => t.ClaimDataId)
                .Index(t => t.ClaimDataConfigId)
                .Index(t => t.RiDataWarehouseId)
                .Index(t => t.ReferralRiDataId)
                .Index(t => t.OriginalClaimRegisterId)
                .Index(t => t.ClaimReasonId)
                .Index(t => t.PicClaimId)
                .Index(t => t.PicDaaId)
                .Index(t => t.ClaimStatus)
                .Index(t => t.ProvisionStatus)
                .Index(t => t.OffsetStatus)
                .Index(t => t.MappingStatus)
                .Index(t => t.ProcessingStatus)
                .Index(t => t.DuplicationCheckStatus)
                .Index(t => t.PostComputationStatus)
                .Index(t => t.PostValidationStatus)
                .Index(t => t.IsReferralCase)
                .Index(t => t.HasRedFlag)
                .Index(t => t.TargetDateToIssueInvoice)
                .Index(t => t.ClaimId)
                .Index(t => t.ClaimCode)
                .Index(t => t.PolicyNumber)
                .Index(t => t.PolicyTerm)
                .Index(t => t.ClaimRecoveryAmt)
                .Index(t => t.ClaimTransactionType)
                .Index(t => t.TreatyCode)
                .Index(t => t.TreatyType)
                .Index(t => t.AarPayable)
                .Index(t => t.AnnualRiPrem)
                .Index(t => t.CauseOfEvent)
                .Index(t => t.CedantClaimEventCode)
                .Index(t => t.CedantClaimType)
                .Index(t => t.CedantDateOfNotification)
                .Index(t => t.CedingBenefitRiskCode)
                .Index(t => t.CedingBenefitTypeCode)
                .Index(t => t.CedingClaimType)
                .Index(t => t.CedingCompany)
                .Index(t => t.CedingEventCode)
                .Index(t => t.CedingPlanCode)
                .Index(t => t.CurrencyRate)
                .Index(t => t.CurrencyCode)
                .Index(t => t.DateApproved)
                .Index(t => t.DateOfEvent)
                .Index(t => t.DateOfRegister)
                .Index(t => t.DateOfReported)
                .Index(t => t.EntryNo)
                .Index(t => t.ExGratia)
                .Index(t => t.ForeignClaimRecoveryAmt)
                .Index(t => t.FundsAccountingTypeCode)
                .Index(t => t.InsuredDateOfBirth)
                .Index(t => t.InsuredGenderCode)
                .Index(t => t.InsuredName)
                .Index(t => t.InsuredTobaccoUse)
                .Index(t => t.LastTransactionDate)
                .Index(t => t.LastTransactionQuarter)
                .Index(t => t.LateInterest)
                .Index(t => t.Layer1SumRein)
                .Index(t => t.Mfrs17AnnualCohort)
                .Index(t => t.Mfrs17ContractCode)
                .Index(t => t.MlreBenefitCode)
                .Index(t => t.MlreEventCode)
                .Index(t => t.MlreInvoiceDate)
                .Index(t => t.MlreInvoiceNumber)
                .Index(t => t.MlreRetainAmount)
                .Index(t => t.MlreShare)
                .Index(t => t.PendingProvisionDay)
                .Index(t => t.PolicyDuration)
                .Index(t => t.RecordType)
                .Index(t => t.ReinsBasisCode)
                .Index(t => t.ReinsEffDatePol)
                .Index(t => t.RetroParty1)
                .Index(t => t.RetroParty2)
                .Index(t => t.RetroParty3)
                .Index(t => t.RetroRecovery1)
                .Index(t => t.RetroRecovery2)
                .Index(t => t.RetroRecovery3)
                .Index(t => t.RetroStatementDate1)
                .Index(t => t.RetroStatementDate2)
                .Index(t => t.RetroStatementDate3)
                .Index(t => t.RetroShare1)
                .Index(t => t.RetroShare2)
                .Index(t => t.RetroShare3)
                .Index(t => t.RetroStatementId1)
                .Index(t => t.RetroStatementId2)
                .Index(t => t.RetroStatementId3)
                .Index(t => t.RiskPeriodMonth)
                .Index(t => t.RiskPeriodYear)
                .Index(t => t.RiskQuarter)
                .Index(t => t.SaFactor)
                .Index(t => t.SoaQuarter)
                .Index(t => t.SumIns)
                .Index(t => t.TempA1)
                .Index(t => t.TempA2)
                .Index(t => t.TempD1)
                .Index(t => t.TempD2)
                .Index(t => t.TempI1)
                .Index(t => t.TempI2)
                .Index(t => t.TempS1)
                .Index(t => t.TempS2)
                .Index(t => t.TransactionDateWop)
                .Index(t => t.ClaimAssessorId)
                .Index(t => t.ClaimCommitteeUser1Id)
                .Index(t => t.ClaimCommitteeUser2Id)
                .Index(t => t.ClaimCommitteeDateCommented1)
                .Index(t => t.ClaimCommitteeDateCommented2)
                .Index(t => t.CeoClaimReasonId)
                .Index(t => t.UpdatedOnBehalfById)
                .Index(t => t.SignOffById)
                .Index(t => t.CedingTreatyCode)
                .Index(t => t.CampaignCode)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.ReferralRiData",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ReferralRiDataFileId = c.Int(nullable: false),
                    TreatyCode = c.String(maxLength: 35),
                    ReinsBasisCode = c.String(maxLength: 30),
                    FundsAccountingTypeCode = c.String(maxLength: 30),
                    PremiumFrequencyCode = c.String(maxLength: 10),
                    ReportPeriodMonth = c.Int(),
                    ReportPeriodYear = c.Int(),
                    RiskPeriodMonth = c.Int(),
                    RiskPeriodYear = c.Int(),
                    TransactionTypeCode = c.String(maxLength: 2),
                    PolicyNumber = c.String(maxLength: 150),
                    IssueDatePol = c.DateTime(precision: 7, storeType: "datetime2"),
                    IssueDateBen = c.DateTime(precision: 7, storeType: "datetime2"),
                    ReinsEffDatePol = c.DateTime(precision: 7, storeType: "datetime2"),
                    ReinsEffDateBen = c.DateTime(precision: 7, storeType: "datetime2"),
                    CedingPlanCode = c.String(maxLength: 30),
                    CedingBenefitTypeCode = c.String(maxLength: 30),
                    CedingBenefitRiskCode = c.String(maxLength: 50),
                    MlreBenefitCode = c.String(maxLength: 10),
                    OriSumAssured = c.Double(),
                    CurrSumAssured = c.Double(),
                    AmountCededB4MlreShare = c.Double(),
                    RetentionAmount = c.Double(),
                    AarOri = c.Double(),
                    Aar = c.Double(),
                    AarSpecial1 = c.Double(),
                    AarSpecial2 = c.Double(),
                    AarSpecial3 = c.Double(),
                    InsuredName = c.String(maxLength: 128),
                    InsuredGenderCode = c.String(maxLength: 1),
                    InsuredTobaccoUse = c.String(maxLength: 1),
                    InsuredDateOfBirth = c.DateTime(precision: 7, storeType: "datetime2"),
                    InsuredOccupationCode = c.String(maxLength: 5),
                    InsuredRegisterNo = c.String(maxLength: 30),
                    InsuredAttainedAge = c.Int(),
                    InsuredNewIcNumber = c.String(maxLength: 15),
                    InsuredOldIcNumber = c.String(maxLength: 15),
                    InsuredName2nd = c.String(maxLength: 128),
                    InsuredGenderCode2nd = c.String(maxLength: 1),
                    InsuredTobaccoUse2nd = c.String(maxLength: 1),
                    InsuredDateOfBirth2nd = c.DateTime(precision: 7, storeType: "datetime2"),
                    InsuredAttainedAge2nd = c.Int(),
                    InsuredNewIcNumber2nd = c.String(maxLength: 15),
                    InsuredOldIcNumber2nd = c.String(maxLength: 15),
                    ReinsuranceIssueAge = c.Int(),
                    ReinsuranceIssueAge2nd = c.Int(),
                    PolicyTerm = c.Int(),
                    PolicyExpiryDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    DurationYear = c.Double(),
                    DurationDay = c.Double(),
                    DurationMonth = c.Double(),
                    PremiumCalType = c.String(maxLength: 5),
                    CedantRiRate = c.Double(),
                    RateTable = c.String(maxLength: 50),
                    AgeRatedUp = c.Int(),
                    DiscountRate = c.Double(),
                    LoadingType = c.String(maxLength: 15),
                    UnderwriterRating = c.Double(),
                    UnderwriterRatingUnit = c.Double(),
                    UnderwriterRatingTerm = c.Int(),
                    UnderwriterRating2 = c.Double(),
                    UnderwriterRatingUnit2 = c.Double(),
                    UnderwriterRatingTerm2 = c.Int(),
                    UnderwriterRating3 = c.Double(),
                    UnderwriterRatingUnit3 = c.Double(),
                    UnderwriterRatingTerm3 = c.Int(),
                    FlatExtraAmount = c.Double(),
                    FlatExtraUnit = c.Double(),
                    FlatExtraTerm = c.Int(),
                    FlatExtraAmount2 = c.Double(),
                    FlatExtraTerm2 = c.Int(),
                    StandardPremium = c.Double(),
                    SubstandardPremium = c.Double(),
                    FlatExtraPremium = c.Double(),
                    GrossPremium = c.Double(),
                    StandardDiscount = c.Double(),
                    SubstandardDiscount = c.Double(),
                    VitalityDiscount = c.Double(),
                    TotalDiscount = c.Double(),
                    NetPremium = c.Double(),
                    AnnualRiPrem = c.Double(),
                    RiCovPeriod = c.Double(),
                    AdjBeginDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    AdjEndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    PolicyNumberOld = c.String(maxLength: 150),
                    PolicyStatusCode = c.String(maxLength: 20),
                    PolicyGrossPremium = c.Double(),
                    PolicyStandardPremium = c.Double(),
                    PolicySubstandardPremium = c.Double(),
                    PolicyTermRemain = c.Int(),
                    PolicyAmountDeath = c.Double(),
                    PolicyReserve = c.Double(),
                    PolicyPaymentMethod = c.String(maxLength: 10),
                    PolicyLifeNumber = c.Int(),
                    FundCode = c.String(maxLength: 25),
                    LineOfBusiness = c.String(maxLength: 5),
                    ApLoading = c.Double(),
                    LoanInterestRate = c.Double(),
                    DefermentPeriod = c.Int(),
                    RiderNumber = c.Int(),
                    CampaignCode = c.String(maxLength: 10),
                    Nationality = c.String(maxLength: 20),
                    TerritoryOfIssueCode = c.String(maxLength: 20),
                    CurrencyCode = c.String(maxLength: 3),
                    StaffPlanIndicator = c.String(maxLength: 1),
                    CedingTreatyCode = c.String(maxLength: 30),
                    CedingPlanCodeOld = c.String(maxLength: 30),
                    CedingBasicPlanCode = c.String(maxLength: 30),
                    CedantSar = c.Double(),
                    CedantReinsurerCode = c.String(maxLength: 10),
                    AmountCededB4MlreShare2 = c.Double(),
                    CessionCode = c.String(maxLength: 10),
                    CedantRemark = c.String(maxLength: 255),
                    GroupPolicyNumber = c.String(maxLength: 30),
                    GroupPolicyName = c.String(maxLength: 128),
                    NoOfEmployee = c.Int(),
                    PolicyTotalLive = c.Int(),
                    GroupSubsidiaryName = c.String(maxLength: 128),
                    GroupSubsidiaryNo = c.String(maxLength: 30),
                    GroupEmployeeBasicSalary = c.Double(),
                    GroupEmployeeJobType = c.String(maxLength: 10),
                    GroupEmployeeJobCode = c.String(maxLength: 10),
                    GroupEmployeeBasicSalaryRevise = c.Double(),
                    GroupEmployeeBasicSalaryMultiplier = c.Double(),
                    CedingPlanCode2 = c.String(maxLength: 30),
                    DependantIndicator = c.String(maxLength: 2),
                    GhsRoomBoard = c.Int(),
                    PolicyAmountSubstandard = c.Double(),
                    Layer1RiShare = c.Double(),
                    Layer1InsuredAttainedAge = c.Int(),
                    Layer1InsuredAttainedAge2nd = c.Int(),
                    Layer1StandardPremium = c.Double(),
                    Layer1SubstandardPremium = c.Double(),
                    Layer1GrossPremium = c.Double(),
                    Layer1StandardDiscount = c.Double(),
                    Layer1SubstandardDiscount = c.Double(),
                    Layer1TotalDiscount = c.Double(),
                    Layer1NetPremium = c.Double(),
                    Layer1GrossPremiumAlt = c.Double(),
                    Layer1TotalDiscountAlt = c.Double(),
                    Layer1NetPremiumAlt = c.Double(),
                    SpecialIndicator1 = c.String(),
                    SpecialIndicator2 = c.String(),
                    SpecialIndicator3 = c.String(),
                    IndicatorJointLife = c.String(maxLength: 1),
                    TaxAmount = c.Double(),
                    GstIndicator = c.String(maxLength: 3),
                    GstGrossPremium = c.Double(),
                    GstTotalDiscount = c.Double(),
                    GstVitality = c.Double(),
                    GstAmount = c.Double(),
                    Mfrs17BasicRider = c.String(maxLength: 5),
                    Mfrs17CellName = c.String(maxLength: 50),
                    Mfrs17TreatyCode = c.String(maxLength: 25),
                    LoaCode = c.String(maxLength: 10),
                    CurrencyRate = c.Double(),
                    NoClaimBonus = c.Double(),
                    SurrenderValue = c.Double(),
                    DatabaseCommision = c.Double(),
                    GrossPremiumAlt = c.Double(),
                    NetPremiumAlt = c.Double(),
                    Layer1FlatExtraPremium = c.Double(),
                    TransactionPremium = c.Double(),
                    OriginalPremium = c.Double(),
                    TransactionDiscount = c.Double(),
                    OriginalDiscount = c.Double(),
                    BrokerageFee = c.Double(),
                    MaxUwRating = c.Double(),
                    RetentionCap = c.Double(),
                    AarCap = c.Double(),
                    RiRate = c.Double(),
                    RiRate2 = c.Double(),
                    AnnuityFactor = c.Double(),
                    SumAssuredOffered = c.Double(),
                    UwRatingOffered = c.Double(),
                    FlatExtraAmountOffered = c.Double(),
                    FlatExtraDuration = c.Double(),
                    EffectiveDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    OfferLetterSentDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    RiskPeriodStartDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    RiskPeriodEndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    Mfrs17AnnualCohort = c.Int(),
                    MaxExpiryAge = c.Int(),
                    MinIssueAge = c.Int(),
                    MaxIssueAge = c.Int(),
                    MinAar = c.Double(),
                    MaxAar = c.Double(),
                    CorridorLimit = c.Double(),
                    Abl = c.Double(),
                    RatePerBasisUnit = c.Int(),
                    RiDiscountRate = c.Double(),
                    LargeSaDiscount = c.Double(),
                    GroupSizeDiscount = c.Double(),
                    EwarpNumber = c.Int(),
                    EwarpActionCode = c.String(maxLength: 10),
                    RetentionShare = c.Double(),
                    AarShare = c.Double(),
                    ProfitComm = c.String(maxLength: 1),
                    TotalDirectRetroAar = c.Double(),
                    TotalDirectRetroGrossPremium = c.Double(),
                    TotalDirectRetroDiscount = c.Double(),
                    TotalDirectRetroNetPremium = c.Double(),
                    TreatyType = c.String(maxLength: 20),
                    LastUpdatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    AarShare2 = c.Double(),
                    AarCap2 = c.Double(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.ReferralRiDataFiles", t => t.ReferralRiDataFileId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.ReferralRiDataFileId)
                .Index(t => t.TreatyCode)
                .Index(t => t.ReinsBasisCode)
                .Index(t => t.FundsAccountingTypeCode)
                .Index(t => t.PremiumFrequencyCode)
                .Index(t => t.ReportPeriodMonth)
                .Index(t => t.ReportPeriodYear)
                .Index(t => t.RiskPeriodMonth)
                .Index(t => t.RiskPeriodYear)
                .Index(t => t.TransactionTypeCode)
                .Index(t => t.PolicyNumber)
                .Index(t => t.IssueDatePol)
                .Index(t => t.IssueDateBen)
                .Index(t => t.ReinsEffDatePol)
                .Index(t => t.ReinsEffDateBen)
                .Index(t => t.CedingPlanCode)
                .Index(t => t.CedingBenefitTypeCode)
                .Index(t => t.CedingBenefitRiskCode)
                .Index(t => t.MlreBenefitCode)
                .Index(t => t.OriSumAssured)
                .Index(t => t.CurrSumAssured)
                .Index(t => t.AmountCededB4MlreShare)
                .Index(t => t.RetentionAmount)
                .Index(t => t.AarOri)
                .Index(t => t.Aar)
                .Index(t => t.AarSpecial1)
                .Index(t => t.AarSpecial2)
                .Index(t => t.AarSpecial3)
                .Index(t => t.InsuredName)
                .Index(t => t.InsuredGenderCode)
                .Index(t => t.InsuredTobaccoUse)
                .Index(t => t.InsuredDateOfBirth)
                .Index(t => t.InsuredOccupationCode)
                .Index(t => t.InsuredRegisterNo)
                .Index(t => t.InsuredAttainedAge)
                .Index(t => t.InsuredNewIcNumber)
                .Index(t => t.InsuredOldIcNumber)
                .Index(t => t.InsuredName2nd)
                .Index(t => t.InsuredGenderCode2nd)
                .Index(t => t.InsuredTobaccoUse2nd)
                .Index(t => t.InsuredDateOfBirth2nd)
                .Index(t => t.InsuredAttainedAge2nd)
                .Index(t => t.InsuredNewIcNumber2nd)
                .Index(t => t.InsuredOldIcNumber2nd)
                .Index(t => t.ReinsuranceIssueAge)
                .Index(t => t.ReinsuranceIssueAge2nd)
                .Index(t => t.PolicyTerm)
                .Index(t => t.PolicyExpiryDate)
                .Index(t => t.DurationYear)
                .Index(t => t.DurationDay)
                .Index(t => t.DurationMonth)
                .Index(t => t.PremiumCalType)
                .Index(t => t.CedantRiRate)
                .Index(t => t.RateTable)
                .Index(t => t.AgeRatedUp)
                .Index(t => t.DiscountRate)
                .Index(t => t.LoadingType)
                .Index(t => t.UnderwriterRating)
                .Index(t => t.UnderwriterRatingUnit)
                .Index(t => t.UnderwriterRatingTerm)
                .Index(t => t.UnderwriterRating2)
                .Index(t => t.UnderwriterRatingUnit2)
                .Index(t => t.UnderwriterRatingTerm2)
                .Index(t => t.UnderwriterRating3)
                .Index(t => t.UnderwriterRatingUnit3)
                .Index(t => t.UnderwriterRatingTerm3)
                .Index(t => t.FlatExtraAmount)
                .Index(t => t.FlatExtraUnit)
                .Index(t => t.FlatExtraTerm)
                .Index(t => t.FlatExtraAmount2)
                .Index(t => t.FlatExtraTerm2)
                .Index(t => t.StandardPremium)
                .Index(t => t.SubstandardPremium)
                .Index(t => t.FlatExtraPremium)
                .Index(t => t.GrossPremium)
                .Index(t => t.StandardDiscount)
                .Index(t => t.SubstandardDiscount)
                .Index(t => t.VitalityDiscount)
                .Index(t => t.TotalDiscount)
                .Index(t => t.NetPremium)
                .Index(t => t.AnnualRiPrem)
                .Index(t => t.RiCovPeriod)
                .Index(t => t.AdjBeginDate)
                .Index(t => t.AdjEndDate)
                .Index(t => t.PolicyNumberOld)
                .Index(t => t.PolicyStatusCode)
                .Index(t => t.PolicyGrossPremium)
                .Index(t => t.PolicyStandardPremium)
                .Index(t => t.PolicySubstandardPremium)
                .Index(t => t.PolicyTermRemain)
                .Index(t => t.PolicyAmountDeath)
                .Index(t => t.PolicyReserve)
                .Index(t => t.PolicyPaymentMethod)
                .Index(t => t.PolicyLifeNumber)
                .Index(t => t.FundCode)
                .Index(t => t.LineOfBusiness)
                .Index(t => t.ApLoading)
                .Index(t => t.LoanInterestRate)
                .Index(t => t.DefermentPeriod)
                .Index(t => t.RiderNumber)
                .Index(t => t.CampaignCode)
                .Index(t => t.Nationality)
                .Index(t => t.TerritoryOfIssueCode)
                .Index(t => t.CurrencyCode)
                .Index(t => t.StaffPlanIndicator)
                .Index(t => t.CedingTreatyCode)
                .Index(t => t.CedingPlanCodeOld)
                .Index(t => t.CedingBasicPlanCode)
                .Index(t => t.CedantSar)
                .Index(t => t.CedantReinsurerCode)
                .Index(t => t.AmountCededB4MlreShare2)
                .Index(t => t.CessionCode)
                .Index(t => t.GroupPolicyNumber)
                .Index(t => t.GroupPolicyName)
                .Index(t => t.NoOfEmployee)
                .Index(t => t.PolicyTotalLive)
                .Index(t => t.GroupSubsidiaryName)
                .Index(t => t.GroupSubsidiaryNo)
                .Index(t => t.GroupEmployeeBasicSalary)
                .Index(t => t.GroupEmployeeJobType)
                .Index(t => t.GroupEmployeeJobCode)
                .Index(t => t.GroupEmployeeBasicSalaryRevise)
                .Index(t => t.GroupEmployeeBasicSalaryMultiplier)
                .Index(t => t.CedingPlanCode2)
                .Index(t => t.DependantIndicator)
                .Index(t => t.GhsRoomBoard)
                .Index(t => t.PolicyAmountSubstandard)
                .Index(t => t.Layer1RiShare)
                .Index(t => t.Layer1InsuredAttainedAge)
                .Index(t => t.Layer1InsuredAttainedAge2nd)
                .Index(t => t.Layer1StandardPremium)
                .Index(t => t.Layer1SubstandardPremium)
                .Index(t => t.Layer1GrossPremium)
                .Index(t => t.Layer1StandardDiscount)
                .Index(t => t.Layer1SubstandardDiscount)
                .Index(t => t.Layer1TotalDiscount)
                .Index(t => t.Layer1NetPremium)
                .Index(t => t.Layer1GrossPremiumAlt)
                .Index(t => t.Layer1TotalDiscountAlt)
                .Index(t => t.Layer1NetPremiumAlt)
                .Index(t => t.IndicatorJointLife)
                .Index(t => t.TaxAmount)
                .Index(t => t.GstIndicator)
                .Index(t => t.GstGrossPremium)
                .Index(t => t.GstTotalDiscount)
                .Index(t => t.GstVitality)
                .Index(t => t.GstAmount)
                .Index(t => t.Mfrs17BasicRider)
                .Index(t => t.Mfrs17CellName)
                .Index(t => t.Mfrs17TreatyCode)
                .Index(t => t.LoaCode)
                .Index(t => t.CurrencyRate)
                .Index(t => t.NoClaimBonus)
                .Index(t => t.SurrenderValue)
                .Index(t => t.DatabaseCommision)
                .Index(t => t.GrossPremiumAlt)
                .Index(t => t.NetPremiumAlt)
                .Index(t => t.Layer1FlatExtraPremium)
                .Index(t => t.TransactionPremium)
                .Index(t => t.OriginalPremium)
                .Index(t => t.TransactionDiscount)
                .Index(t => t.OriginalDiscount)
                .Index(t => t.BrokerageFee)
                .Index(t => t.MaxUwRating)
                .Index(t => t.RetentionCap)
                .Index(t => t.AarCap)
                .Index(t => t.RiRate)
                .Index(t => t.RiRate2)
                .Index(t => t.AnnuityFactor)
                .Index(t => t.SumAssuredOffered)
                .Index(t => t.UwRatingOffered)
                .Index(t => t.FlatExtraAmountOffered)
                .Index(t => t.FlatExtraDuration)
                .Index(t => t.EffectiveDate)
                .Index(t => t.OfferLetterSentDate)
                .Index(t => t.RiskPeriodStartDate)
                .Index(t => t.RiskPeriodEndDate)
                .Index(t => t.Mfrs17AnnualCohort)
                .Index(t => t.MaxExpiryAge)
                .Index(t => t.MinIssueAge)
                .Index(t => t.MaxIssueAge)
                .Index(t => t.MinAar)
                .Index(t => t.MaxAar)
                .Index(t => t.CorridorLimit)
                .Index(t => t.Abl)
                .Index(t => t.RatePerBasisUnit)
                .Index(t => t.RiDiscountRate)
                .Index(t => t.LargeSaDiscount)
                .Index(t => t.GroupSizeDiscount)
                .Index(t => t.EwarpNumber)
                .Index(t => t.EwarpActionCode)
                .Index(t => t.RetentionShare)
                .Index(t => t.AarShare)
                .Index(t => t.ProfitComm)
                .Index(t => t.TotalDirectRetroAar)
                .Index(t => t.TotalDirectRetroGrossPremium)
                .Index(t => t.TotalDirectRetroDiscount)
                .Index(t => t.TotalDirectRetroNetPremium)
                .Index(t => t.TreatyType)
                .Index(t => t.LastUpdatedDate)
                .Index(t => t.AarShare2)
                .Index(t => t.AarCap2)
                .Index(t => t.CreatedAt)
                .Index(t => t.UpdatedAt)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.ReferralRiDataFiles",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    RawFileId = c.Int(nullable: false),
                    Records = c.Int(nullable: false),
                    UpdatedRecords = c.Int(nullable: false),
                    Error = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.RawFiles", t => t.RawFileId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.RawFileId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RiDataWarehouse",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    EndingPolicyStatus = c.Int(nullable: false),
                    RecordType = c.Int(nullable: false),
                    Quarter = c.String(maxLength: 64),
                    TreatyCode = c.String(maxLength: 35),
                    ReinsBasisCode = c.String(maxLength: 30),
                    FundsAccountingTypeCode = c.String(maxLength: 30),
                    PremiumFrequencyCode = c.String(maxLength: 10),
                    ReportPeriodMonth = c.Int(),
                    ReportPeriodYear = c.Int(),
                    RiskPeriodMonth = c.Int(),
                    RiskPeriodYear = c.Int(),
                    TransactionTypeCode = c.String(maxLength: 2),
                    PolicyNumber = c.String(maxLength: 150),
                    IssueDatePol = c.DateTime(precision: 7, storeType: "datetime2"),
                    IssueDateBen = c.DateTime(precision: 7, storeType: "datetime2"),
                    ReinsEffDatePol = c.DateTime(precision: 7, storeType: "datetime2"),
                    ReinsEffDateBen = c.DateTime(precision: 7, storeType: "datetime2"),
                    CedingPlanCode = c.String(maxLength: 30),
                    CedingBenefitTypeCode = c.String(maxLength: 30),
                    CedingBenefitRiskCode = c.String(maxLength: 50),
                    MlreBenefitCode = c.String(maxLength: 10),
                    OriSumAssured = c.Double(),
                    CurrSumAssured = c.Double(),
                    AmountCededB4MlreShare = c.Double(),
                    RetentionAmount = c.Double(),
                    AarOri = c.Double(),
                    Aar = c.Double(),
                    AarSpecial1 = c.Double(),
                    AarSpecial2 = c.Double(),
                    AarSpecial3 = c.Double(),
                    InsuredName = c.String(maxLength: 128),
                    InsuredGenderCode = c.String(maxLength: 1),
                    InsuredTobaccoUse = c.String(maxLength: 1),
                    InsuredDateOfBirth = c.DateTime(precision: 7, storeType: "datetime2"),
                    InsuredOccupationCode = c.String(maxLength: 5),
                    InsuredRegisterNo = c.String(maxLength: 30),
                    InsuredAttainedAge = c.Int(),
                    InsuredNewIcNumber = c.String(maxLength: 15),
                    InsuredOldIcNumber = c.String(maxLength: 15),
                    InsuredName2nd = c.String(maxLength: 128),
                    InsuredGenderCode2nd = c.String(maxLength: 1),
                    InsuredTobaccoUse2nd = c.String(maxLength: 1),
                    InsuredDateOfBirth2nd = c.DateTime(precision: 7, storeType: "datetime2"),
                    InsuredAttainedAge2nd = c.Int(),
                    InsuredNewIcNumber2nd = c.String(maxLength: 15),
                    InsuredOldIcNumber2nd = c.String(maxLength: 15),
                    ReinsuranceIssueAge = c.Int(),
                    ReinsuranceIssueAge2nd = c.Int(),
                    PolicyTerm = c.Int(),
                    PolicyExpiryDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    DurationYear = c.Double(),
                    DurationDay = c.Double(),
                    DurationMonth = c.Double(),
                    PremiumCalType = c.String(maxLength: 5),
                    CedantRiRate = c.Double(),
                    RateTable = c.String(maxLength: 50),
                    AgeRatedUp = c.Int(),
                    DiscountRate = c.Double(),
                    LoadingType = c.String(maxLength: 15),
                    UnderwriterRating = c.Double(),
                    UnderwriterRatingUnit = c.Double(),
                    UnderwriterRatingTerm = c.Int(),
                    UnderwriterRating2 = c.Double(),
                    UnderwriterRatingUnit2 = c.Double(),
                    UnderwriterRatingTerm2 = c.Int(),
                    UnderwriterRating3 = c.Double(),
                    UnderwriterRatingUnit3 = c.Double(),
                    UnderwriterRatingTerm3 = c.Int(),
                    FlatExtraAmount = c.Double(),
                    FlatExtraUnit = c.Double(),
                    FlatExtraTerm = c.Int(),
                    FlatExtraAmount2 = c.Double(),
                    FlatExtraTerm2 = c.Int(),
                    StandardPremium = c.Double(),
                    SubstandardPremium = c.Double(),
                    FlatExtraPremium = c.Double(),
                    GrossPremium = c.Double(),
                    StandardDiscount = c.Double(),
                    SubstandardDiscount = c.Double(),
                    VitalityDiscount = c.Double(),
                    TotalDiscount = c.Double(),
                    NetPremium = c.Double(),
                    AnnualRiPrem = c.Double(),
                    RiCovPeriod = c.Double(),
                    AdjBeginDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    AdjEndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    PolicyNumberOld = c.String(maxLength: 150),
                    PolicyStatusCode = c.String(maxLength: 20),
                    PolicyGrossPremium = c.Double(),
                    PolicyStandardPremium = c.Double(),
                    PolicySubstandardPremium = c.Double(),
                    PolicyTermRemain = c.Int(),
                    PolicyAmountDeath = c.Double(),
                    PolicyReserve = c.Double(),
                    PolicyPaymentMethod = c.String(maxLength: 10),
                    PolicyLifeNumber = c.Int(),
                    FundCode = c.String(maxLength: 25),
                    LineOfBusiness = c.String(maxLength: 5),
                    ApLoading = c.Double(),
                    LoanInterestRate = c.Double(),
                    DefermentPeriod = c.Int(),
                    RiderNumber = c.Int(),
                    CampaignCode = c.String(maxLength: 10),
                    Nationality = c.String(maxLength: 20),
                    TerritoryOfIssueCode = c.String(maxLength: 20),
                    CurrencyCode = c.String(maxLength: 3),
                    StaffPlanIndicator = c.String(maxLength: 1),
                    CedingTreatyCode = c.String(maxLength: 30),
                    CedingPlanCodeOld = c.String(maxLength: 30),
                    CedingBasicPlanCode = c.String(maxLength: 30),
                    CedantSar = c.Double(),
                    CedantReinsurerCode = c.String(maxLength: 10),
                    AmountCededB4MlreShare2 = c.Double(),
                    CessionCode = c.String(maxLength: 10),
                    CedantRemark = c.String(maxLength: 255),
                    GroupPolicyNumber = c.String(maxLength: 30),
                    GroupPolicyName = c.String(maxLength: 128),
                    NoOfEmployee = c.Int(),
                    PolicyTotalLive = c.Int(),
                    GroupSubsidiaryName = c.String(maxLength: 128),
                    GroupSubsidiaryNo = c.String(maxLength: 30),
                    GroupEmployeeBasicSalary = c.Double(),
                    GroupEmployeeJobType = c.String(maxLength: 10),
                    GroupEmployeeJobCode = c.String(maxLength: 10),
                    GroupEmployeeBasicSalaryRevise = c.Double(),
                    GroupEmployeeBasicSalaryMultiplier = c.Double(),
                    CedingPlanCode2 = c.String(maxLength: 30),
                    DependantIndicator = c.String(maxLength: 2),
                    GhsRoomBoard = c.Int(),
                    PolicyAmountSubstandard = c.Double(),
                    Layer1RiShare = c.Double(),
                    Layer1InsuredAttainedAge = c.Int(),
                    Layer1InsuredAttainedAge2nd = c.Int(),
                    Layer1StandardPremium = c.Double(),
                    Layer1SubstandardPremium = c.Double(),
                    Layer1GrossPremium = c.Double(),
                    Layer1StandardDiscount = c.Double(),
                    Layer1SubstandardDiscount = c.Double(),
                    Layer1TotalDiscount = c.Double(),
                    Layer1NetPremium = c.Double(),
                    Layer1GrossPremiumAlt = c.Double(),
                    Layer1TotalDiscountAlt = c.Double(),
                    Layer1NetPremiumAlt = c.Double(),
                    SpecialIndicator1 = c.String(),
                    SpecialIndicator2 = c.String(),
                    SpecialIndicator3 = c.String(),
                    IndicatorJointLife = c.String(maxLength: 1),
                    TaxAmount = c.Double(),
                    GstIndicator = c.String(maxLength: 3),
                    GstGrossPremium = c.Double(),
                    GstTotalDiscount = c.Double(),
                    GstVitality = c.Double(),
                    GstAmount = c.Double(),
                    Mfrs17BasicRider = c.String(maxLength: 5),
                    Mfrs17CellName = c.String(maxLength: 50),
                    Mfrs17TreatyCode = c.String(maxLength: 25),
                    LoaCode = c.String(maxLength: 10),
                    CurrencyRate = c.Double(),
                    NoClaimBonus = c.Double(),
                    SurrenderValue = c.Double(),
                    DatabaseCommision = c.Double(),
                    GrossPremiumAlt = c.Double(),
                    NetPremiumAlt = c.Double(),
                    Layer1FlatExtraPremium = c.Double(),
                    TransactionPremium = c.Double(),
                    OriginalPremium = c.Double(),
                    TransactionDiscount = c.Double(),
                    OriginalDiscount = c.Double(),
                    BrokerageFee = c.Double(),
                    MaxUwRating = c.Double(),
                    RetentionCap = c.Double(),
                    AarCap = c.Double(),
                    RiRate = c.Double(),
                    RiRate2 = c.Double(),
                    AnnuityFactor = c.Double(),
                    SumAssuredOffered = c.Double(),
                    UwRatingOffered = c.Double(),
                    FlatExtraAmountOffered = c.Double(),
                    FlatExtraDuration = c.Double(),
                    EffectiveDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    OfferLetterSentDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    RiskPeriodStartDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    RiskPeriodEndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    Mfrs17AnnualCohort = c.Int(),
                    MaxExpiryAge = c.Int(),
                    MinIssueAge = c.Int(),
                    MaxIssueAge = c.Int(),
                    MinAar = c.Double(),
                    MaxAar = c.Double(),
                    CorridorLimit = c.Double(),
                    Abl = c.Double(),
                    RatePerBasisUnit = c.Int(),
                    RiDiscountRate = c.Double(),
                    LargeSaDiscount = c.Double(),
                    GroupSizeDiscount = c.Double(),
                    EwarpNumber = c.Int(),
                    EwarpActionCode = c.String(maxLength: 10),
                    RetentionShare = c.Double(),
                    AarShare = c.Double(),
                    ProfitComm = c.String(maxLength: 1),
                    TotalDirectRetroAar = c.Double(),
                    TotalDirectRetroGrossPremium = c.Double(),
                    TotalDirectRetroDiscount = c.Double(),
                    TotalDirectRetroNetPremium = c.Double(),
                    TreatyType = c.String(maxLength: 20),
                    LastUpdatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                    RetroParty1 = c.String(maxLength: 128),
                    RetroParty2 = c.String(maxLength: 128),
                    RetroParty3 = c.String(maxLength: 128),
                    RetroShare1 = c.Double(),
                    RetroShare2 = c.Double(),
                    RetroShare3 = c.Double(),
                    RetroAar1 = c.Double(),
                    RetroAar2 = c.Double(),
                    RetroAar3 = c.Double(),
                    RetroReinsurancePremium1 = c.Double(),
                    RetroReinsurancePremium2 = c.Double(),
                    RetroReinsurancePremium3 = c.Double(),
                    RetroDiscount1 = c.Double(),
                    RetroDiscount2 = c.Double(),
                    RetroDiscount3 = c.Double(),
                    RetroNetPremium1 = c.Double(),
                    RetroNetPremium2 = c.Double(),
                    RetroNetPremium3 = c.Double(),
                    AarShare2 = c.Double(),
                    AarCap2 = c.Double(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.EndingPolicyStatus)
                .Index(t => t.RecordType)
                .Index(t => t.Quarter)
                .Index(t => t.TreatyCode)
                .Index(t => t.PremiumFrequencyCode)
                .Index(t => t.RiskPeriodMonth)
                .Index(t => t.RiskPeriodYear)
                .Index(t => t.TransactionTypeCode)
                .Index(t => t.PolicyNumber)
                .Index(t => t.CedingPlanCode)
                .Index(t => t.CedingBenefitTypeCode)
                .Index(t => t.CedingBenefitRiskCode)
                .Index(t => t.InsuredName)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById)
                .Index(t => t.RetroParty1)
                .Index(t => t.RetroParty2)
                .Index(t => t.RetroParty3)
                .Index(t => t.RetroShare1)
                .Index(t => t.RetroShare2)
                .Index(t => t.RetroShare3)
                .Index(t => t.RetroAar1)
                .Index(t => t.RetroAar2)
                .Index(t => t.RetroAar3)
                .Index(t => t.RetroReinsurancePremium1)
                .Index(t => t.RetroReinsurancePremium2)
                .Index(t => t.RetroReinsurancePremium3)
                .Index(t => t.RetroDiscount1)
                .Index(t => t.RetroDiscount2)
                .Index(t => t.RetroDiscount3)
                .Index(t => t.RetroNetPremium1)
                .Index(t => t.RetroNetPremium2)
                .Index(t => t.RetroNetPremium3)
                .Index(t => t.AarShare2)
                .Index(t => t.AarCap2);

            CreateTable(
                "dbo.ClaimRegisterHistories",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CutOffId = c.Int(nullable: false),
                    ClaimRegisterId = c.Int(nullable: false),
                    ClaimDataBatchId = c.Int(),
                    ClaimDataId = c.Int(),
                    ClaimDataConfigId = c.Int(),
                    RiDataId = c.Int(),
                    RiDataWarehouseId = c.Int(),
                    OriginalClaimRegisterId = c.Int(),
                    ClaimReasonId = c.Int(),
                    PicClaimId = c.Int(),
                    PicDaaId = c.Int(),
                    ClaimStatus = c.Int(nullable: false),
                    ProvisionStatus = c.Int(nullable: false),
                    OffsetStatus = c.Int(nullable: false),
                    MappingStatus = c.Int(nullable: false),
                    ProcessingStatus = c.Int(nullable: false),
                    DuplicationCheckStatus = c.Int(nullable: false),
                    PostComputationStatus = c.Int(nullable: false),
                    PostValidationStatus = c.Int(nullable: false),
                    Errors = c.String(storeType: "ntext"),
                    ProvisionErrors = c.String(storeType: "ntext"),
                    RedFlagWarnings = c.String(storeType: "ntext"),
                    RefferalCaseIndicator = c.Boolean(nullable: false),
                    HasRedFlag = c.Boolean(nullable: false),
                    TargetDateToIssueInvoice = c.DateTime(),
                    ClaimId = c.String(maxLength: 30),
                    ClaimCode = c.String(maxLength: 30),
                    PolicyNumber = c.String(maxLength: 150),
                    PolicyTerm = c.Int(),
                    ClaimRecoveryAmt = c.Double(),
                    ClaimTransactionType = c.String(maxLength: 30),
                    TreatyCode = c.String(maxLength: 35),
                    TreatyType = c.String(maxLength: 35),
                    AarPayable = c.Double(),
                    AnnualRiPrem = c.Double(),
                    CauseOfEvent = c.String(maxLength: 255),
                    CedantClaimEventCode = c.String(maxLength: 30),
                    CedantClaimType = c.String(maxLength: 30),
                    CedantDateOfNotification = c.DateTime(),
                    CedingBenefitRiskCode = c.String(maxLength: 30),
                    CedingBenefitTypeCode = c.String(maxLength: 30),
                    CedingClaimType = c.String(maxLength: 30),
                    CedingCompany = c.String(maxLength: 30),
                    CedingEventCode = c.String(maxLength: 30),
                    CedingPlanCode = c.String(maxLength: 30),
                    CurrencyRate = c.Double(),
                    CurrencyCode = c.String(maxLength: 3),
                    DateApproved = c.DateTime(),
                    DateOfEvent = c.DateTime(),
                    DateOfRegister = c.DateTime(),
                    DateOfReported = c.DateTime(),
                    EntryNo = c.String(maxLength: 30),
                    ExGratia = c.Double(),
                    ForeignClaimRecoveryAmt = c.Double(),
                    FundsAccountingTypeCode = c.String(maxLength: 30),
                    InsuredDateOfBirth = c.DateTime(),
                    InsuredGenderCode = c.String(maxLength: 1),
                    InsuredName = c.String(maxLength: 128),
                    InsuredTobaccoUse = c.String(maxLength: 1),
                    LastTransactionDate = c.DateTime(),
                    LastTransactionQuarter = c.String(maxLength: 30),
                    LateInterest = c.Double(),
                    Layer1SumRein = c.Double(),
                    Mfrs17AnnualCohort = c.Int(),
                    Mfrs17ContractCode = c.String(maxLength: 30),
                    MlreBenefitCode = c.String(maxLength: 30),
                    MlreEventCode = c.String(maxLength: 30),
                    MlreInvoiceDate = c.DateTime(),
                    MlreInvoiceNumber = c.String(maxLength: 30),
                    MlreRetainAmount = c.Double(),
                    MlreShare = c.Double(),
                    PendingProvisionDay = c.Int(),
                    PolicyDuration = c.Int(),
                    RecordType = c.String(maxLength: 30),
                    ReinsBasisCode = c.String(maxLength: 30),
                    ReinsEffDatePol = c.DateTime(),
                    RetroParty1 = c.String(maxLength: 128),
                    RetroParty2 = c.String(maxLength: 128),
                    RetroParty3 = c.String(maxLength: 128),
                    RetroRecovery1 = c.Double(),
                    RetroRecovery2 = c.Double(),
                    RetroRecovery3 = c.Double(),
                    RetroStatementDate1 = c.DateTime(),
                    RetroStatementDate2 = c.DateTime(),
                    RetroStatementDate3 = c.DateTime(),
                    RetroShare1 = c.Double(),
                    RetroShare2 = c.Double(),
                    RetroShare3 = c.Double(),
                    RetroStatementId1 = c.String(maxLength: 30),
                    RetroStatementId2 = c.String(maxLength: 30),
                    RetroStatementId3 = c.String(maxLength: 30),
                    RiskPeriodMonth = c.Int(),
                    RiskPeriodYear = c.Int(),
                    RiskQuarter = c.String(maxLength: 30),
                    SaFactor = c.Double(),
                    SoaQuarter = c.String(maxLength: 30),
                    SumIns = c.Double(),
                    TempA1 = c.Double(),
                    TempA2 = c.Double(),
                    TempD1 = c.DateTime(),
                    TempD2 = c.DateTime(),
                    TempI1 = c.Int(),
                    TempI2 = c.Int(),
                    TempS1 = c.String(maxLength: 50),
                    TempS2 = c.String(maxLength: 50),
                    TransactionDateWop = c.DateTime(),
                    MlreReferenceNo = c.String(maxLength: 30),
                    AddInfo = c.String(maxLength: 30),
                    Remark1 = c.String(maxLength: 128),
                    Remark2 = c.String(maxLength: 128),
                    IssueDatePol = c.DateTime(precision: 7, storeType: "datetime2"),
                    PolicyExpiryDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    ClaimAssessorId = c.Int(),
                    Comment = c.String(maxLength: 128),
                    RequestUnderwriterReview = c.Boolean(nullable: false),
                    UnderwriterFeedback = c.Int(),
                    EventChronologyComment = c.String(),
                    ClaimAssessorRecommendation = c.String(),
                    ClaimCommitteeComment1 = c.String(),
                    ClaimCommitteeComment2 = c.String(),
                    ClaimCommitteeUser1Id = c.Int(),
                    ClaimCommitteeUser2Id = c.Int(),
                    ClaimCommitteeDateCommented1 = c.DateTime(),
                    ClaimCommitteeDateCommented2 = c.DateTime(),
                    CeoClaimReasonId = c.Int(),
                    CeoComment = c.String(),
                    UpdatedOnBehalfById = c.Int(),
                    UpdatedOnBehalfAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    Checklist = c.String(storeType: "ntext"),
                    SignOffById = c.Int(),
                    SignOffDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    CedingTreatyCode = c.String(maxLength: 30),
                    CampaignCode = c.String(maxLength: 10),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClaimReasons", t => t.CeoClaimReasonId)
                .ForeignKey("dbo.Users", t => t.ClaimAssessorId)
                .ForeignKey("dbo.Users", t => t.ClaimCommitteeUser1Id)
                .ForeignKey("dbo.Users", t => t.ClaimCommitteeUser2Id)
                .ForeignKey("dbo.ClaimData", t => t.ClaimDataId)
                .ForeignKey("dbo.ClaimDataBatches", t => t.ClaimDataBatchId)
                .ForeignKey("dbo.ClaimDataConfigs", t => t.ClaimDataConfigId)
                .ForeignKey("dbo.ClaimReasons", t => t.ClaimReasonId)
                .ForeignKey("dbo.ClaimRegister", t => t.ClaimRegisterId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.CutOff", t => t.CutOffId)
                .ForeignKey("dbo.ClaimRegister", t => t.OriginalClaimRegisterId)
                .ForeignKey("dbo.Users", t => t.PicClaimId)
                .ForeignKey("dbo.Users", t => t.PicDaaId)
                .ForeignKey("dbo.RiDataWarehouse", t => t.RiDataWarehouseId)
                .ForeignKey("dbo.Users", t => t.SignOffById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedOnBehalfById)
                .Index(t => t.CutOffId)
                .Index(t => t.ClaimRegisterId)
                .Index(t => t.ClaimDataBatchId)
                .Index(t => t.ClaimDataId)
                .Index(t => t.ClaimDataConfigId)
                .Index(t => t.RiDataWarehouseId)
                .Index(t => t.OriginalClaimRegisterId)
                .Index(t => t.ClaimReasonId)
                .Index(t => t.PicClaimId)
                .Index(t => t.PicDaaId)
                .Index(t => t.ClaimStatus)
                .Index(t => t.ProvisionStatus)
                .Index(t => t.OffsetStatus)
                .Index(t => t.MappingStatus)
                .Index(t => t.ProcessingStatus)
                .Index(t => t.DuplicationCheckStatus)
                .Index(t => t.PostComputationStatus)
                .Index(t => t.PostValidationStatus)
                .Index(t => t.RefferalCaseIndicator)
                .Index(t => t.HasRedFlag)
                .Index(t => t.TargetDateToIssueInvoice)
                .Index(t => t.ClaimId)
                .Index(t => t.ClaimCode)
                .Index(t => t.PolicyNumber)
                .Index(t => t.PolicyTerm)
                .Index(t => t.ClaimRecoveryAmt)
                .Index(t => t.ClaimTransactionType)
                .Index(t => t.TreatyCode)
                .Index(t => t.TreatyType)
                .Index(t => t.AarPayable)
                .Index(t => t.AnnualRiPrem)
                .Index(t => t.CauseOfEvent)
                .Index(t => t.CedantClaimEventCode)
                .Index(t => t.CedantClaimType)
                .Index(t => t.CedantDateOfNotification)
                .Index(t => t.CedingBenefitRiskCode)
                .Index(t => t.CedingBenefitTypeCode)
                .Index(t => t.CedingClaimType)
                .Index(t => t.CedingCompany)
                .Index(t => t.CedingEventCode)
                .Index(t => t.CedingPlanCode)
                .Index(t => t.CurrencyRate)
                .Index(t => t.CurrencyCode)
                .Index(t => t.DateApproved)
                .Index(t => t.DateOfEvent)
                .Index(t => t.DateOfRegister)
                .Index(t => t.DateOfReported)
                .Index(t => t.EntryNo)
                .Index(t => t.ExGratia)
                .Index(t => t.ForeignClaimRecoveryAmt)
                .Index(t => t.FundsAccountingTypeCode)
                .Index(t => t.InsuredDateOfBirth)
                .Index(t => t.InsuredGenderCode)
                .Index(t => t.InsuredName)
                .Index(t => t.InsuredTobaccoUse)
                .Index(t => t.LastTransactionDate)
                .Index(t => t.LastTransactionQuarter)
                .Index(t => t.LateInterest)
                .Index(t => t.Layer1SumRein)
                .Index(t => t.Mfrs17AnnualCohort)
                .Index(t => t.Mfrs17ContractCode)
                .Index(t => t.MlreBenefitCode)
                .Index(t => t.MlreEventCode)
                .Index(t => t.MlreInvoiceDate)
                .Index(t => t.MlreInvoiceNumber)
                .Index(t => t.MlreRetainAmount)
                .Index(t => t.MlreShare)
                .Index(t => t.PendingProvisionDay)
                .Index(t => t.PolicyDuration)
                .Index(t => t.RecordType)
                .Index(t => t.ReinsBasisCode)
                .Index(t => t.ReinsEffDatePol)
                .Index(t => t.RetroParty1)
                .Index(t => t.RetroParty2)
                .Index(t => t.RetroParty3)
                .Index(t => t.RetroRecovery1)
                .Index(t => t.RetroRecovery2)
                .Index(t => t.RetroRecovery3)
                .Index(t => t.RetroStatementDate1)
                .Index(t => t.RetroStatementDate2)
                .Index(t => t.RetroStatementDate3)
                .Index(t => t.RetroShare1)
                .Index(t => t.RetroShare2)
                .Index(t => t.RetroShare3)
                .Index(t => t.RetroStatementId1)
                .Index(t => t.RetroStatementId2)
                .Index(t => t.RetroStatementId3)
                .Index(t => t.RiskPeriodMonth)
                .Index(t => t.RiskPeriodYear)
                .Index(t => t.RiskQuarter)
                .Index(t => t.SaFactor)
                .Index(t => t.SoaQuarter)
                .Index(t => t.SumIns)
                .Index(t => t.TempA1)
                .Index(t => t.TempA2)
                .Index(t => t.TempD1)
                .Index(t => t.TempD2)
                .Index(t => t.TempI1)
                .Index(t => t.TempI2)
                .Index(t => t.TempS1)
                .Index(t => t.TempS2)
                .Index(t => t.TransactionDateWop)
                .Index(t => t.ClaimAssessorId)
                .Index(t => t.ClaimCommitteeUser1Id)
                .Index(t => t.ClaimCommitteeUser2Id)
                .Index(t => t.ClaimCommitteeDateCommented1)
                .Index(t => t.ClaimCommitteeDateCommented2)
                .Index(t => t.CeoClaimReasonId)
                .Index(t => t.UpdatedOnBehalfById)
                .Index(t => t.SignOffById)
                .Index(t => t.CedingTreatyCode)
                .Index(t => t.CampaignCode)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.CutOff",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Status = c.Int(nullable: false),
                    Month = c.Int(nullable: false),
                    Year = c.Int(nullable: false),
                    Quarter = c.String(maxLength: 32),
                    CutOffDateTime = c.DateTime(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Status)
                .Index(t => t.Quarter)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.DirectRetro",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CedantId = c.Int(nullable: false),
                    TreatyCodeId = c.Int(nullable: false),
                    SoaQuarter = c.String(nullable: false, maxLength: 10),
                    SoaDataBatchId = c.Int(nullable: false),
                    RetroStatus = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cedants", t => t.CedantId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.SoaDataBatches", t => t.SoaDataBatchId)
                .ForeignKey("dbo.TreatyCodes", t => t.TreatyCodeId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.CedantId)
                .Index(t => t.TreatyCodeId)
                .Index(t => t.SoaQuarter)
                .Index(t => t.SoaDataBatchId)
                .Index(t => t.RetroStatus)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.DirectRetroConfigurationDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DirectRetroConfigurationId = c.Int(nullable: false),
                    RiskPeriodStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    RiskPeriodEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    IssueDatePolStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    IssueDatePolEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ReinsEffDatePolStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ReinsEffDatePolEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    IsDefault = c.Boolean(nullable: false),
                    RetroPartyId = c.Int(nullable: false),
                    Share = c.Double(nullable: false),
                    PremiumSpreadTableId = c.Int(nullable: false),
                    TreatyDiscountTableId = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById, cascadeDelete: true)
                .ForeignKey("dbo.DirectRetroConfigurations", t => t.DirectRetroConfigurationId)
                .ForeignKey("dbo.PremiumSpreadTables", t => t.PremiumSpreadTableId)
                .ForeignKey("dbo.RetroParties", t => t.RetroPartyId)
                .ForeignKey("dbo.TreatyDiscountTables", t => t.TreatyDiscountTableId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.DirectRetroConfigurationId)
                .Index(t => t.RiskPeriodStartDate)
                .Index(t => t.RiskPeriodEndDate)
                .Index(t => t.IssueDatePolStartDate)
                .Index(t => t.IssueDatePolEndDate)
                .Index(t => t.ReinsEffDatePolStartDate)
                .Index(t => t.ReinsEffDatePolEndDate)
                .Index(t => t.IsDefault)
                .Index(t => t.RetroPartyId)
                .Index(t => t.Share)
                .Index(t => t.PremiumSpreadTableId)
                .Index(t => t.TreatyDiscountTableId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.DirectRetroConfigurations",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 255),
                    TreatyCodeId = c.Int(nullable: false),
                    RetroParty = c.String(nullable: false, storeType: "ntext"),
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
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.DirectRetroConfigurationMappings",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DirectRetroConfigurationId = c.Int(nullable: false),
                    Combination = c.String(maxLength: 255),
                    RetroParty = c.String(maxLength: 50),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.DirectRetroConfigurations", t => t.DirectRetroConfigurationId)
                .Index(t => t.DirectRetroConfigurationId)
                .Index(t => t.Combination)
                .Index(t => t.RetroParty)
                .Index(t => t.CreatedById);

            CreateTable(
                "dbo.PremiumSpreadTables",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Rule = c.String(nullable: false, maxLength: 30),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Rule)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RetroParties",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Party = c.String(nullable: false, maxLength: 50),
                    Code = c.String(nullable: false, maxLength: 50),
                    Name = c.String(nullable: false, maxLength: 255),
                    Type = c.Int(nullable: false),
                    CountryCodePickListDetailId = c.Int(nullable: false),
                    Description = c.String(maxLength: 255),
                    Status = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PickListDetails", t => t.CountryCodePickListDetailId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Party)
                .Index(t => t.Code)
                .Index(t => t.Type)
                .Index(t => t.CountryCodePickListDetailId)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyDiscountTables",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Rule = c.String(nullable: false, maxLength: 30),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Rule)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.DirectRetroProvisioningTransactions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ClaimRegisterId = c.Int(nullable: false),
                    FinanceProvisioningId = c.Int(),
                    IsLatestProvision = c.Boolean(nullable: false),
                    ClaimId = c.String(maxLength: 30),
                    CedingCompany = c.String(maxLength: 30),
                    EntryNo = c.String(maxLength: 30),
                    Quarter = c.String(maxLength: 30),
                    RetroParty = c.String(maxLength: 128),
                    RetroRecovery = c.Double(nullable: false),
                    RetroStatementId = c.String(maxLength: 30),
                    RetroStatementDate = c.DateTime(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClaimRegister", t => t.ClaimRegisterId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.FinanceProvisionings", t => t.FinanceProvisioningId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.ClaimRegisterId)
                .Index(t => t.FinanceProvisioningId)
                .Index(t => t.IsLatestProvision)
                .Index(t => t.ClaimId)
                .Index(t => t.CedingCompany)
                .Index(t => t.EntryNo)
                .Index(t => t.Quarter)
                .Index(t => t.RetroParty)
                .Index(t => t.RetroRecovery)
                .Index(t => t.RetroStatementId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.FinanceProvisionings",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CutOffId = c.Int(nullable: false),
                    Date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    Record = c.Int(nullable: false),
                    Amount = c.Double(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.CutOff", t => t.CutOffId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.CutOffId)
                .Index(t => t.Record)
                .Index(t => t.Amount)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.DirectRetroStatusFiles",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DirectRetroId = c.Int(nullable: false),
                    StatusHistoryId = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.DirectRetro", t => t.DirectRetroId)
                .ForeignKey("dbo.StatusHistories", t => t.StatusHistoryId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.DirectRetroId)
                .Index(t => t.StatusHistoryId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.DiscountTables",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CedantId = c.Int(nullable: false),
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
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.EventClaimCodeMappingDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    EventClaimCodeMappingId = c.Int(nullable: false),
                    Combination = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    CedingEventCode = c.String(maxLength: 30),
                    CedingClaimType = c.String(maxLength: 30),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.EventClaimCodeMappings", t => t.EventClaimCodeMappingId)
                .Index(t => t.EventClaimCodeMappingId)
                .Index(t => t.Combination)
                .Index(t => t.CreatedById)
                .Index(t => t.CedingEventCode)
                .Index(t => t.CedingClaimType);

            CreateTable(
                "dbo.EventClaimCodeMappings",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CedantId = c.Int(nullable: false),
                    EventCodeId = c.Int(nullable: false),
                    CedingEventCode = c.String(storeType: "ntext"),
                    CedingClaimType = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cedants", t => t.CedantId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.EventCodes", t => t.EventCodeId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.CedantId)
                .Index(t => t.EventCodeId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.FacMasterListingDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FacMasterListingId = c.Int(nullable: false),
                    PolicyNumber = c.String(maxLength: 150),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.FacMasterListings", t => t.FacMasterListingId)
                .Index(t => t.FacMasterListingId)
                .Index(t => t.PolicyNumber)
                .Index(t => t.CreatedById);

            CreateTable(
                "dbo.FacMasterListings",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UniqueId = c.String(maxLength: 255),
                    EwarpNumber = c.Int(),
                    InsuredName = c.String(maxLength: 128),
                    InsuredDateOfBirth = c.DateTime(precision: 7, storeType: "datetime2"),
                    InsuredGenderCodePickListDetailId = c.Int(),
                    CedantId = c.Int(),
                    PolicyNumber = c.String(storeType: "ntext"),
                    FlatExtraAmountOffered = c.Double(),
                    FlatExtraDuration = c.Int(),
                    BenefitId = c.Int(),
                    SumAssuredOffered = c.Double(),
                    EwarpActionCode = c.String(maxLength: 10),
                    UwRatingOffered = c.Double(),
                    OfferLetterSentDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    UwOpinion = c.String(),
                    Remark = c.String(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Benefits", t => t.BenefitId)
                .ForeignKey("dbo.Cedants", t => t.CedantId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickListDetails", t => t.InsuredGenderCodePickListDetailId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.UniqueId)
                .Index(t => t.EwarpNumber)
                .Index(t => t.InsuredGenderCodePickListDetailId)
                .Index(t => t.CedantId)
                .Index(t => t.FlatExtraAmountOffered)
                .Index(t => t.FlatExtraDuration)
                .Index(t => t.BenefitId)
                .Index(t => t.SumAssuredOffered)
                .Index(t => t.EwarpActionCode)
                .Index(t => t.UwRatingOffered)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.FinanceProvisioningTransactions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ClaimRegisterId = c.Int(nullable: false),
                    FinanceProvisioningId = c.Int(),
                    IsLatestProvision = c.Boolean(nullable: false),
                    ClaimId = c.String(),
                    PolicyNumber = c.String(),
                    CedingCompany = c.String(),
                    EntryNo = c.String(),
                    Quarter = c.String(),
                    SumReinsured = c.Double(nullable: false),
                    ClaimRecoveryAmount = c.Double(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClaimRegister", t => t.ClaimRegisterId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.FinanceProvisionings", t => t.FinanceProvisioningId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.ClaimRegisterId)
                .Index(t => t.FinanceProvisioningId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.GroupDiscounts",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DiscountTableId = c.Int(nullable: false),
                    DiscountCode = c.String(nullable: false, maxLength: 30),
                    EffectiveStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    EffectiveEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    GroupSizeFrom = c.Int(nullable: false),
                    GroupSizeTo = c.Int(nullable: false),
                    Discount = c.Double(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.DiscountTables", t => t.DiscountTableId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.DiscountTableId)
                .Index(t => t.DiscountCode)
                .Index(t => t.GroupSizeFrom)
                .Index(t => t.GroupSizeTo)
                .Index(t => t.Discount)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.InvoiceRegisterBatches",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    BatchNo = c.Int(nullable: false),
                    BatchDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    TotalInvoice = c.Int(nullable: false),
                    Status = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.BatchNo)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.InvoiceRegisterBatchFiles",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    InvoiceRegisterBatchId = c.Int(nullable: false),
                    FileName = c.String(maxLength: 255),
                    HashFileName = c.String(maxLength: 255),
                    Type = c.Int(nullable: false),
                    Status = c.Int(nullable: false),
                    Errors = c.String(storeType: "ntext"),
                    DataUpdate = c.Boolean(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.InvoiceRegisterBatches", t => t.InvoiceRegisterBatchId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.InvoiceRegisterBatchId)
                .Index(t => t.FileName)
                .Index(t => t.HashFileName)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.InvoiceRegisterBatchRemarkDocuments",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    InvoiceRegisterBatchRemarkId = c.Int(nullable: false),
                    FileName = c.String(maxLength: 255),
                    HashFileName = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.InvoiceRegisterBatchRemarks", t => t.InvoiceRegisterBatchRemarkId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.InvoiceRegisterBatchRemarkId)
                .Index(t => t.FileName)
                .Index(t => t.HashFileName)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.InvoiceRegisterBatchRemarks",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    InvoiceRegisterBatchId = c.Int(nullable: false),
                    Status = c.Int(nullable: false),
                    RemarkPermission = c.Boolean(nullable: false),
                    Content = c.String(storeType: "ntext"),
                    FilePermission = c.Boolean(nullable: false),
                    FollowUp = c.Boolean(nullable: false),
                    FollowUpStatus = c.Int(),
                    FollowUpDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    FollowUpUserId = c.Int(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.FollowUpUserId)
                .ForeignKey("dbo.InvoiceRegisterBatches", t => t.InvoiceRegisterBatchId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.InvoiceRegisterBatchId)
                .Index(t => t.FollowUpUserId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.InvoiceRegisterBatchSoaDatas",
                c => new
                {
                    InvoiceRegisterBatchId = c.Int(nullable: false),
                    SoaDataBatchId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.InvoiceRegisterBatchId, t.SoaDataBatchId })
                .ForeignKey("dbo.InvoiceRegisterBatches", t => t.InvoiceRegisterBatchId)
                .ForeignKey("dbo.SoaDataBatches", t => t.SoaDataBatchId)
                .Index(t => t.InvoiceRegisterBatchId)
                .Index(t => t.SoaDataBatchId);

            CreateTable(
                "dbo.InvoiceRegisterHistories",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CutOffId = c.Int(nullable: false),
                    InvoiceRegisterId = c.Int(nullable: false),
                    InvoiceRegisterBatchId = c.Int(nullable: false),
                    SoaDataBatchId = c.Int(),
                    InvoiceType = c.Int(nullable: false),
                    InvoiceReference = c.String(nullable: false, maxLength: 30),
                    InvoiceNumber = c.String(maxLength: 30),
                    InvoiceDate = c.DateTime(nullable: false),
                    StatementReceivedDate = c.DateTime(),
                    CedantId = c.Int(),
                    RiskQuarter = c.String(maxLength: 32),
                    TreatyCodeId = c.Int(),
                    AccountDescription = c.String(maxLength: 128),
                    TotalPaid = c.Double(),
                    PaymentReference = c.String(maxLength: 50),
                    PaymentAmount = c.Double(),
                    PaymentReceivedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    Year1st = c.Double(),
                    Renewal = c.Double(),
                    Gross1st = c.Double(),
                    GrossRenewal = c.Double(),
                    AltPremium = c.Double(),
                    Discount1st = c.Double(),
                    DiscountRen = c.Double(),
                    DiscountAlt = c.Double(),
                    RiskPremium = c.Double(),
                    Claim = c.Double(),
                    ProfitComm = c.Double(),
                    Levy = c.Double(),
                    SurrenderValue = c.Double(),
                    ModcoReserveIncome = c.Double(),
                    ReinsDeposit = c.Double(),
                    NoClaimBonus = c.Double(),
                    DatabaseCommission = c.Double(),
                    AdministrationContribution = c.Double(),
                    ShareOfRiCommissionFromCompulsoryCession = c.Double(),
                    RecaptureFee = c.Double(),
                    CreditCardCharges = c.Double(),
                    BrokerageFee = c.Double(),
                    NBCession = c.Int(),
                    NBSumReins = c.Double(),
                    RNCession = c.Int(),
                    RNSumReins = c.Double(),
                    AltCession = c.Int(),
                    AltSumReins = c.Double(),
                    Frequency = c.Int(),
                    PreparedById = c.Int(),
                    PlanName = c.String(),
                    ValuationGross1st = c.Double(),
                    ValuationGrossRen = c.Double(),
                    ValuationDiscount1st = c.Double(),
                    ValuationDiscountRen = c.Double(),
                    ValuationCom1st = c.Double(),
                    ValuationComRen = c.Double(),
                    ValuationClaims = c.Double(),
                    ValuationProfitCom = c.Double(),
                    ValuationMode = c.Int(),
                    ValuationRiskPremium = c.Double(),
                    Remark = c.String(storeType: "ntext"),
                    CurrencyCode = c.String(maxLength: 32),
                    CurrencyRate = c.Double(),
                    SoaQuarter = c.String(maxLength: 32),
                    ReasonOfAdjustment1 = c.String(maxLength: 128),
                    ReasonOfAdjustment2 = c.String(maxLength: 128),
                    InvoiceNumber1 = c.String(maxLength: 64),
                    InvoiceDate1 = c.DateTime(),
                    Amount1 = c.Double(),
                    InvoiceNumber2 = c.String(maxLength: 64),
                    InvoiceDate2 = c.DateTime(),
                    Amount2 = c.Double(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cedants", t => t.CedantId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.CutOff", t => t.CutOffId)
                .ForeignKey("dbo.InvoiceRegister", t => t.InvoiceRegisterId)
                .ForeignKey("dbo.InvoiceRegisterBatches", t => t.InvoiceRegisterBatchId)
                .ForeignKey("dbo.Users", t => t.PreparedById)
                .ForeignKey("dbo.SoaDataBatches", t => t.SoaDataBatchId)
                .ForeignKey("dbo.TreatyCodes", t => t.TreatyCodeId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.CutOffId)
                .Index(t => t.InvoiceRegisterId)
                .Index(t => t.InvoiceRegisterBatchId)
                .Index(t => t.SoaDataBatchId)
                .Index(t => t.InvoiceType)
                .Index(t => t.InvoiceReference)
                .Index(t => t.InvoiceNumber)
                .Index(t => t.CedantId)
                .Index(t => t.RiskQuarter)
                .Index(t => t.TreatyCodeId)
                .Index(t => t.PreparedById)
                .Index(t => t.SoaQuarter)
                .Index(t => t.InvoiceNumber1)
                .Index(t => t.InvoiceNumber2)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.InvoiceRegister",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    InvoiceRegisterBatchId = c.Int(nullable: false),
                    SoaDataBatchId = c.Int(),
                    InvoiceType = c.Int(nullable: false),
                    InvoiceReference = c.String(maxLength: 30),
                    InvoiceNumber = c.String(maxLength: 30),
                    InvoiceDate = c.DateTime(nullable: false),
                    StatementReceivedDate = c.DateTime(),
                    CedantId = c.Int(),
                    RiskQuarter = c.String(maxLength: 32),
                    TreatyCodeId = c.Int(),
                    AccountDescription = c.String(maxLength: 128),
                    TotalPaid = c.Double(),
                    PaymentReference = c.String(maxLength: 50),
                    PaymentAmount = c.Double(),
                    PaymentReceivedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    Year1st = c.Double(),
                    Renewal = c.Double(),
                    Gross1st = c.Double(),
                    GrossRenewal = c.Double(),
                    AltPremium = c.Double(),
                    Discount1st = c.Double(),
                    DiscountRen = c.Double(),
                    DiscountAlt = c.Double(),
                    RiskPremium = c.Double(),
                    Claim = c.Double(),
                    ProfitComm = c.Double(),
                    Levy = c.Double(),
                    SurrenderValue = c.Double(),
                    ModcoReserveIncome = c.Double(),
                    ReinsDeposit = c.Double(),
                    NoClaimBonus = c.Double(),
                    DatabaseCommission = c.Double(),
                    AdministrationContribution = c.Double(),
                    ShareOfRiCommissionFromCompulsoryCession = c.Double(),
                    RecaptureFee = c.Double(),
                    CreditCardCharges = c.Double(),
                    BrokerageFee = c.Double(),
                    NBCession = c.Int(),
                    NBSumReins = c.Double(),
                    RNCession = c.Int(),
                    RNSumReins = c.Double(),
                    AltCession = c.Int(),
                    AltSumReins = c.Double(),
                    Frequency = c.Int(),
                    PreparedById = c.Int(),
                    PlanName = c.String(),
                    ValuationGross1st = c.Double(),
                    ValuationGrossRen = c.Double(),
                    ValuationDiscount1st = c.Double(),
                    ValuationDiscountRen = c.Double(),
                    ValuationCom1st = c.Double(),
                    ValuationComRen = c.Double(),
                    ValuationClaims = c.Double(),
                    ValuationProfitCom = c.Double(),
                    ValuationMode = c.Int(),
                    ValuationRiskPremium = c.Double(),
                    Remark = c.String(storeType: "ntext"),
                    CurrencyCode = c.String(maxLength: 32),
                    CurrencyRate = c.Double(),
                    SoaQuarter = c.String(maxLength: 32),
                    ReasonOfAdjustment1 = c.String(maxLength: 128),
                    ReasonOfAdjustment2 = c.String(maxLength: 128),
                    InvoiceNumber1 = c.String(maxLength: 64),
                    InvoiceDate1 = c.DateTime(),
                    Amount1 = c.Double(),
                    InvoiceNumber2 = c.String(maxLength: 64),
                    InvoiceDate2 = c.DateTime(),
                    Amount2 = c.Double(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cedants", t => t.CedantId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.InvoiceRegisterBatches", t => t.InvoiceRegisterBatchId)
                .ForeignKey("dbo.Users", t => t.PreparedById)
                .ForeignKey("dbo.SoaDataBatches", t => t.SoaDataBatchId)
                .ForeignKey("dbo.TreatyCodes", t => t.TreatyCodeId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.InvoiceRegisterBatchId)
                .Index(t => t.SoaDataBatchId)
                .Index(t => t.InvoiceType)
                .Index(t => t.InvoiceReference)
                .Index(t => t.InvoiceNumber)
                .Index(t => t.CedantId)
                .Index(t => t.RiskQuarter)
                .Index(t => t.TreatyCodeId)
                .Index(t => t.PreparedById)
                .Index(t => t.SoaQuarter)
                .Index(t => t.InvoiceNumber1)
                .Index(t => t.InvoiceNumber2)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.InvoiceRegisterValuations",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    InvoiceRegisterId = c.Int(nullable: false),
                    ValuationBenefitCodeId = c.Int(nullable: false),
                    Amount = c.Double(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.InvoiceRegister", t => t.InvoiceRegisterId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .ForeignKey("dbo.Benefits", t => t.ValuationBenefitCodeId)
                .Index(t => t.InvoiceRegisterId)
                .Index(t => t.ValuationBenefitCodeId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.ItemCodeMappings",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ItemCodeId = c.Int(nullable: false),
                    TreatyTypePickListDetailId = c.Int(nullable: false),
                    InvoiceFieldPickListDetailId = c.Int(),
                    StandardSoaDataOutputId = c.Int(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickListDetails", t => t.InvoiceFieldPickListDetailId)
                .ForeignKey("dbo.ItemCodes", t => t.ItemCodeId)
                .ForeignKey("dbo.StandardSoaDataOutputs", t => t.StandardSoaDataOutputId)
                .ForeignKey("dbo.PickListDetails", t => t.TreatyTypePickListDetailId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.ItemCodeId)
                .Index(t => t.TreatyTypePickListDetailId)
                .Index(t => t.InvoiceFieldPickListDetailId)
                .Index(t => t.StandardSoaDataOutputId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.ItemCodes",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Code = c.String(nullable: false, maxLength: 10),
                    ReportingType = c.Int(nullable: false),
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
                .Index(t => t.ReportingType)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.LargeDiscounts",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DiscountTableId = c.Int(nullable: false),
                    DiscountCode = c.String(nullable: false, maxLength: 30),
                    EffectiveStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    EffectiveEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    AarFrom = c.Double(nullable: false),
                    AarTo = c.Double(nullable: false),
                    Discount = c.Double(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.DiscountTables", t => t.DiscountTableId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.DiscountTableId)
                .Index(t => t.DiscountCode)
                .Index(t => t.AarFrom)
                .Index(t => t.AarTo)
                .Index(t => t.Discount)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RiDataWarehouseHistories",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CutOffId = c.Int(nullable: false),
                    RiDataWarehouseId = c.Int(nullable: false),
                    EndingPolicyStatus = c.Int(nullable: false),
                    RecordType = c.Int(nullable: false),
                    Quarter = c.String(maxLength: 64),
                    TreatyCode = c.String(maxLength: 35),
                    ReinsBasisCode = c.String(maxLength: 30),
                    FundsAccountingTypeCode = c.String(maxLength: 30),
                    PremiumFrequencyCode = c.String(maxLength: 10),
                    ReportPeriodMonth = c.Int(),
                    ReportPeriodYear = c.Int(),
                    RiskPeriodMonth = c.Int(),
                    RiskPeriodYear = c.Int(),
                    TransactionTypeCode = c.String(maxLength: 2),
                    PolicyNumber = c.String(maxLength: 150),
                    IssueDatePol = c.DateTime(precision: 7, storeType: "datetime2"),
                    IssueDateBen = c.DateTime(precision: 7, storeType: "datetime2"),
                    ReinsEffDatePol = c.DateTime(precision: 7, storeType: "datetime2"),
                    ReinsEffDateBen = c.DateTime(precision: 7, storeType: "datetime2"),
                    CedingPlanCode = c.String(maxLength: 30),
                    CedingBenefitTypeCode = c.String(maxLength: 30),
                    CedingBenefitRiskCode = c.String(maxLength: 50),
                    MlreBenefitCode = c.String(maxLength: 10),
                    OriSumAssured = c.Double(),
                    CurrSumAssured = c.Double(),
                    AmountCededB4MlreShare = c.Double(),
                    RetentionAmount = c.Double(),
                    AarOri = c.Double(),
                    Aar = c.Double(),
                    AarSpecial1 = c.Double(),
                    AarSpecial2 = c.Double(),
                    AarSpecial3 = c.Double(),
                    InsuredName = c.String(maxLength: 128),
                    InsuredGenderCode = c.String(maxLength: 1),
                    InsuredTobaccoUse = c.String(maxLength: 1),
                    InsuredDateOfBirth = c.DateTime(precision: 7, storeType: "datetime2"),
                    InsuredOccupationCode = c.String(maxLength: 5),
                    InsuredRegisterNo = c.String(maxLength: 30),
                    InsuredAttainedAge = c.Int(),
                    InsuredNewIcNumber = c.String(maxLength: 15),
                    InsuredOldIcNumber = c.String(maxLength: 15),
                    InsuredName2nd = c.String(maxLength: 128),
                    InsuredGenderCode2nd = c.String(maxLength: 1),
                    InsuredTobaccoUse2nd = c.String(maxLength: 1),
                    InsuredDateOfBirth2nd = c.DateTime(precision: 7, storeType: "datetime2"),
                    InsuredAttainedAge2nd = c.Int(),
                    InsuredNewIcNumber2nd = c.String(maxLength: 15),
                    InsuredOldIcNumber2nd = c.String(maxLength: 15),
                    ReinsuranceIssueAge = c.Int(),
                    ReinsuranceIssueAge2nd = c.Int(),
                    PolicyTerm = c.Int(),
                    PolicyExpiryDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    DurationYear = c.Double(),
                    DurationDay = c.Double(),
                    DurationMonth = c.Double(),
                    PremiumCalType = c.String(maxLength: 5),
                    CedantRiRate = c.Double(),
                    RateTable = c.String(maxLength: 50),
                    AgeRatedUp = c.Int(),
                    DiscountRate = c.Double(),
                    LoadingType = c.String(maxLength: 15),
                    UnderwriterRating = c.Double(),
                    UnderwriterRatingUnit = c.Double(),
                    UnderwriterRatingTerm = c.Int(),
                    UnderwriterRating2 = c.Double(),
                    UnderwriterRatingUnit2 = c.Double(),
                    UnderwriterRatingTerm2 = c.Int(),
                    UnderwriterRating3 = c.Double(),
                    UnderwriterRatingUnit3 = c.Double(),
                    UnderwriterRatingTerm3 = c.Int(),
                    FlatExtraAmount = c.Double(),
                    FlatExtraUnit = c.Double(),
                    FlatExtraTerm = c.Int(),
                    FlatExtraAmount2 = c.Double(),
                    FlatExtraTerm2 = c.Int(),
                    StandardPremium = c.Double(),
                    SubstandardPremium = c.Double(),
                    FlatExtraPremium = c.Double(),
                    GrossPremium = c.Double(),
                    StandardDiscount = c.Double(),
                    SubstandardDiscount = c.Double(),
                    VitalityDiscount = c.Double(),
                    TotalDiscount = c.Double(),
                    NetPremium = c.Double(),
                    AnnualRiPrem = c.Double(),
                    RiCovPeriod = c.Double(),
                    AdjBeginDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    AdjEndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    PolicyNumberOld = c.String(maxLength: 150),
                    PolicyStatusCode = c.String(maxLength: 20),
                    PolicyGrossPremium = c.Double(),
                    PolicyStandardPremium = c.Double(),
                    PolicySubstandardPremium = c.Double(),
                    PolicyTermRemain = c.Int(),
                    PolicyAmountDeath = c.Double(),
                    PolicyReserve = c.Double(),
                    PolicyPaymentMethod = c.String(maxLength: 10),
                    PolicyLifeNumber = c.Int(),
                    FundCode = c.String(maxLength: 25),
                    LineOfBusiness = c.String(maxLength: 5),
                    ApLoading = c.Double(),
                    LoanInterestRate = c.Double(),
                    DefermentPeriod = c.Int(),
                    RiderNumber = c.Int(),
                    CampaignCode = c.String(maxLength: 10),
                    Nationality = c.String(maxLength: 20),
                    TerritoryOfIssueCode = c.String(maxLength: 20),
                    CurrencyCode = c.String(maxLength: 3),
                    StaffPlanIndicator = c.String(maxLength: 1),
                    CedingTreatyCode = c.String(maxLength: 30),
                    CedingPlanCodeOld = c.String(maxLength: 30),
                    CedingBasicPlanCode = c.String(maxLength: 30),
                    CedantSar = c.Double(),
                    CedantReinsurerCode = c.String(maxLength: 10),
                    AmountCededB4MlreShare2 = c.Double(),
                    CessionCode = c.String(maxLength: 10),
                    CedantRemark = c.String(maxLength: 255),
                    GroupPolicyNumber = c.String(maxLength: 30),
                    GroupPolicyName = c.String(maxLength: 128),
                    NoOfEmployee = c.Int(),
                    PolicyTotalLive = c.Int(),
                    GroupSubsidiaryName = c.String(maxLength: 128),
                    GroupSubsidiaryNo = c.String(maxLength: 30),
                    GroupEmployeeBasicSalary = c.Double(),
                    GroupEmployeeJobType = c.String(maxLength: 10),
                    GroupEmployeeJobCode = c.String(maxLength: 10),
                    GroupEmployeeBasicSalaryRevise = c.Double(),
                    GroupEmployeeBasicSalaryMultiplier = c.Double(),
                    CedingPlanCode2 = c.String(maxLength: 30),
                    DependantIndicator = c.String(maxLength: 2),
                    GhsRoomBoard = c.Int(),
                    PolicyAmountSubstandard = c.Double(),
                    Layer1RiShare = c.Double(),
                    Layer1InsuredAttainedAge = c.Int(),
                    Layer1InsuredAttainedAge2nd = c.Int(),
                    Layer1StandardPremium = c.Double(),
                    Layer1SubstandardPremium = c.Double(),
                    Layer1GrossPremium = c.Double(),
                    Layer1StandardDiscount = c.Double(),
                    Layer1SubstandardDiscount = c.Double(),
                    Layer1TotalDiscount = c.Double(),
                    Layer1NetPremium = c.Double(),
                    Layer1GrossPremiumAlt = c.Double(),
                    Layer1TotalDiscountAlt = c.Double(),
                    Layer1NetPremiumAlt = c.Double(),
                    SpecialIndicator1 = c.String(),
                    SpecialIndicator2 = c.String(),
                    SpecialIndicator3 = c.String(),
                    IndicatorJointLife = c.String(maxLength: 1),
                    TaxAmount = c.Double(),
                    GstIndicator = c.String(maxLength: 3),
                    GstGrossPremium = c.Double(),
                    GstTotalDiscount = c.Double(),
                    GstVitality = c.Double(),
                    GstAmount = c.Double(),
                    Mfrs17BasicRider = c.String(maxLength: 5),
                    Mfrs17CellName = c.String(maxLength: 50),
                    Mfrs17TreatyCode = c.String(maxLength: 25),
                    LoaCode = c.String(maxLength: 10),
                    CurrencyRate = c.Double(),
                    NoClaimBonus = c.Double(),
                    SurrenderValue = c.Double(),
                    DatabaseCommision = c.Double(),
                    GrossPremiumAlt = c.Double(),
                    NetPremiumAlt = c.Double(),
                    Layer1FlatExtraPremium = c.Double(),
                    TransactionPremium = c.Double(),
                    OriginalPremium = c.Double(),
                    TransactionDiscount = c.Double(),
                    OriginalDiscount = c.Double(),
                    BrokerageFee = c.Double(),
                    MaxUwRating = c.Double(),
                    RetentionCap = c.Double(),
                    AarCap = c.Double(),
                    RiRate = c.Double(),
                    RiRate2 = c.Double(),
                    AnnuityFactor = c.Double(),
                    SumAssuredOffered = c.Double(),
                    UwRatingOffered = c.Double(),
                    FlatExtraAmountOffered = c.Double(),
                    FlatExtraDuration = c.Double(),
                    EffectiveDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    OfferLetterSentDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    RiskPeriodStartDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    RiskPeriodEndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    Mfrs17AnnualCohort = c.Int(),
                    MaxExpiryAge = c.Int(),
                    MinIssueAge = c.Int(),
                    MaxIssueAge = c.Int(),
                    MinAar = c.Double(),
                    MaxAar = c.Double(),
                    CorridorLimit = c.Double(),
                    Abl = c.Double(),
                    RatePerBasisUnit = c.Int(),
                    RiDiscountRate = c.Double(),
                    LargeSaDiscount = c.Double(),
                    GroupSizeDiscount = c.Double(),
                    EwarpNumber = c.Int(),
                    EwarpActionCode = c.String(maxLength: 10),
                    RetentionShare = c.Double(),
                    AarShare = c.Double(),
                    ProfitComm = c.String(maxLength: 1),
                    TotalDirectRetroAar = c.Double(),
                    TotalDirectRetroGrossPremium = c.Double(),
                    TotalDirectRetroDiscount = c.Double(),
                    TotalDirectRetroNetPremium = c.Double(),
                    TreatyType = c.String(maxLength: 20),
                    LastUpdatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    RetroParty1 = c.String(maxLength: 128),
                    RetroParty2 = c.String(maxLength: 128),
                    RetroParty3 = c.String(maxLength: 128),
                    RetroShare1 = c.Double(),
                    RetroShare2 = c.Double(),
                    RetroShare3 = c.Double(),
                    RetroAar1 = c.Double(),
                    RetroAar2 = c.Double(),
                    RetroAar3 = c.Double(),
                    RetroReinsurancePremium1 = c.Double(),
                    RetroReinsurancePremium2 = c.Double(),
                    RetroReinsurancePremium3 = c.Double(),
                    RetroDiscount1 = c.Double(),
                    RetroDiscount2 = c.Double(),
                    RetroDiscount3 = c.Double(),
                    RetroNetPremium1 = c.Double(),
                    RetroNetPremium2 = c.Double(),
                    RetroNetPremium3 = c.Double(),
                    AarShare2 = c.Double(),
                    AarCap2 = c.Double(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.CutOff", t => t.CutOffId)
                .ForeignKey("dbo.RiDataWarehouse", t => t.RiDataWarehouseId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.CutOffId)
                .Index(t => t.RiDataWarehouseId)
                .Index(t => t.EndingPolicyStatus)
                .Index(t => t.RecordType)
                .Index(t => t.Quarter)
                .Index(t => t.TreatyCode)
                .Index(t => t.PremiumFrequencyCode)
                .Index(t => t.RiskPeriodMonth)
                .Index(t => t.RiskPeriodYear)
                .Index(t => t.TransactionTypeCode)
                .Index(t => t.PolicyNumber)
                .Index(t => t.CedingPlanCode)
                .Index(t => t.CedingBenefitTypeCode)
                .Index(t => t.CedingBenefitRiskCode)
                .Index(t => t.InsuredName)
                .Index(t => t.RetroParty1)
                .Index(t => t.RetroParty2)
                .Index(t => t.RetroParty3)
                .Index(t => t.RetroShare1)
                .Index(t => t.RetroShare2)
                .Index(t => t.RetroShare3)
                .Index(t => t.RetroAar1)
                .Index(t => t.RetroAar2)
                .Index(t => t.RetroAar3)
                .Index(t => t.RetroReinsurancePremium1)
                .Index(t => t.RetroReinsurancePremium2)
                .Index(t => t.RetroReinsurancePremium3)
                .Index(t => t.RetroDiscount1)
                .Index(t => t.RetroDiscount2)
                .Index(t => t.RetroDiscount3)
                .Index(t => t.RetroNetPremium1)
                .Index(t => t.RetroNetPremium2)
                .Index(t => t.RetroNetPremium3)
                .Index(t => t.AarShare2)
                .Index(t => t.AarCap2)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.PremiumSpreadTableDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PremiumSpreadTableId = c.Int(nullable: false),
                    CedingPlanCode = c.String(nullable: false, maxLength: 30),
                    BenefitId = c.Int(nullable: false),
                    AgeFrom = c.Int(nullable: false),
                    AgeTo = c.Int(nullable: false),
                    PremiumSpread = c.Double(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Benefits", t => t.BenefitId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PremiumSpreadTables", t => t.PremiumSpreadTableId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.PremiumSpreadTableId)
                .Index(t => t.BenefitId)
                .Index(t => t.AgeFrom)
                .Index(t => t.AgeTo)
                .Index(t => t.PremiumSpread)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.PublicHolidayDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PublicHolidayId = c.Int(nullable: false),
                    PublicHolidayDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    Description = c.String(nullable: false, maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PublicHolidays", t => t.PublicHolidayId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.PublicHolidayId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.PublicHolidays",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Year = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Year)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RateDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    RateId = c.Int(nullable: false),
                    InsuredGenderCodePickListDetailId = c.Int(),
                    CedingTobaccoUsePickListDetailId = c.Int(),
                    CedingOccupationCodePickListDetailId = c.Int(),
                    AttainedAge = c.Int(),
                    IssueAge = c.Int(),
                    PolicyTerm = c.Int(),
                    PolicyTermRemain = c.Int(),
                    RateValue = c.Double(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PickListDetails", t => t.CedingOccupationCodePickListDetailId)
                .ForeignKey("dbo.PickListDetails", t => t.CedingTobaccoUsePickListDetailId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickListDetails", t => t.InsuredGenderCodePickListDetailId)
                .ForeignKey("dbo.Rates", t => t.RateId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.RateId)
                .Index(t => t.InsuredGenderCodePickListDetailId)
                .Index(t => t.CedingTobaccoUsePickListDetailId)
                .Index(t => t.CedingOccupationCodePickListDetailId)
                .Index(t => t.AttainedAge)
                .Index(t => t.IssueAge)
                .Index(t => t.PolicyTerm)
                .Index(t => t.PolicyTermRemain)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.Rates",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Code = c.String(nullable: false, maxLength: 50),
                    ValuationRate = c.Int(nullable: false),
                    RatePerBasis = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Code)
                .Index(t => t.ValuationRate)
                .Index(t => t.RatePerBasis)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RiDiscounts",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DiscountTableId = c.Int(nullable: false),
                    DiscountCode = c.String(nullable: false, maxLength: 30),
                    EffectiveStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    EffectiveEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    DurationFrom = c.Int(nullable: false),
                    DurationTo = c.Int(nullable: false),
                    Discount = c.Double(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.DiscountTables", t => t.DiscountTableId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.DiscountTableId)
                .Index(t => t.DiscountCode)
                .Index(t => t.DurationFrom)
                .Index(t => t.DurationTo)
                .Index(t => t.Discount)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.ReferralClaims",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ClaimRegisterId = c.Int(),
                    RiDataWarehouseId = c.Int(),
                    ReferralRiDataId = c.Int(),
                    Status = c.Int(nullable: false),
                    ReferralId = c.String(),
                    RecordType = c.String(maxLength: 30),
                    InsuredName = c.String(),
                    PolicyNumber = c.String(),
                    InsuredGenderCode = c.String(maxLength: 1),
                    InsuredTobaccoUsage = c.String(maxLength: 1),
                    ReferralReasonId = c.Int(),
                    GroupName = c.String(),
                    DateReceivedFullDocuments = c.DateTime(precision: 7, storeType: "datetime2"),
                    InsuredDateOfBirth = c.DateTime(precision: 7, storeType: "datetime2"),
                    InsuredIcNumber = c.String(maxLength: 15),
                    DateOfCommencement = c.DateTime(precision: 7, storeType: "datetime2"),
                    CedingCompany = c.String(maxLength: 30),
                    ClaimCode = c.String(maxLength: 30),
                    CedingPlanCode = c.String(maxLength: 30),
                    SumInsured = c.Double(),
                    SumReinsured = c.Double(),
                    BenefitSubCode = c.String(),
                    DateOfEvent = c.DateTime(precision: 7, storeType: "datetime2"),
                    RiskQuarter = c.String(maxLength: 30),
                    CauseOfEvent = c.String(maxLength: 255),
                    MlreBenefitCode = c.String(maxLength: 30),
                    ClaimRecoveryAmount = c.Double(),
                    ReinsBasisCode = c.String(maxLength: 30),
                    ClaimCategoryId = c.Int(),
                    IsRgalRetakaful = c.Boolean(nullable: false),
                    ReceivedAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    RespondedAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    TurnAroundTime = c.Long(),
                    DelayReasonId = c.Int(),
                    IsRetro = c.Boolean(nullable: false),
                    RetrocessionaireName = c.String(),
                    RetrocessionaireShare = c.Double(),
                    RetroReferralReasonId = c.Int(),
                    MlreReferralReasonId = c.Int(),
                    RetroReviewedById = c.Int(),
                    RetroReviewedAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    IsValueAddedService = c.Boolean(nullable: false),
                    ValueAddedServiceDetails = c.String(),
                    IsClaimCaseStudy = c.Boolean(nullable: false),
                    CompletedCaseStudyMaterialAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    AssessedById = c.Int(),
                    AssessedAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    AssessorComments = c.String(),
                    ReviewedById = c.Int(),
                    ReviewedAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    ReviewerComments = c.String(),
                    AssignedById = c.Int(),
                    AssignedAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    TreatyCode = c.String(maxLength: 35),
                    TreatyType = c.String(maxLength: 35),
                    TreatyShare = c.Double(),
                    Checklist = c.String(storeType: "ntext"),
                    Error = c.String(storeType: "ntext"),
                    PersonInChargeId = c.Int(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AssessedById)
                .ForeignKey("dbo.Users", t => t.AssignedById)
                .ForeignKey("dbo.ClaimCategories", t => t.ClaimCategoryId)
                .ForeignKey("dbo.ClaimRegister", t => t.ClaimRegisterId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.ClaimReasons", t => t.DelayReasonId)
                .ForeignKey("dbo.ClaimReasons", t => t.MlreReferralReasonId)
                .ForeignKey("dbo.Users", t => t.PersonInChargeId)
                .ForeignKey("dbo.ClaimReasons", t => t.ReferralReasonId)
                .ForeignKey("dbo.ReferralRiData", t => t.ReferralRiDataId)
                .ForeignKey("dbo.ClaimReasons", t => t.RetroReferralReasonId)
                .ForeignKey("dbo.Users", t => t.RetroReviewedById)
                .ForeignKey("dbo.Users", t => t.ReviewedById)
                .ForeignKey("dbo.RiDataWarehouse", t => t.RiDataWarehouseId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.ClaimRegisterId)
                .Index(t => t.RiDataWarehouseId)
                .Index(t => t.ReferralRiDataId)
                .Index(t => t.Status)
                .Index(t => t.RecordType)
                .Index(t => t.InsuredGenderCode)
                .Index(t => t.InsuredTobaccoUsage)
                .Index(t => t.ReferralReasonId)
                .Index(t => t.DateReceivedFullDocuments)
                .Index(t => t.InsuredDateOfBirth)
                .Index(t => t.DateOfCommencement)
                .Index(t => t.CedingCompany)
                .Index(t => t.ClaimCode)
                .Index(t => t.CedingPlanCode)
                .Index(t => t.SumInsured)
                .Index(t => t.SumReinsured)
                .Index(t => t.DateOfEvent)
                .Index(t => t.RiskQuarter)
                .Index(t => t.MlreBenefitCode)
                .Index(t => t.ClaimRecoveryAmount)
                .Index(t => t.ReinsBasisCode)
                .Index(t => t.ClaimCategoryId)
                .Index(t => t.IsRgalRetakaful)
                .Index(t => t.ReceivedAt)
                .Index(t => t.RespondedAt)
                .Index(t => t.TurnAroundTime)
                .Index(t => t.DelayReasonId)
                .Index(t => t.IsRetro)
                .Index(t => t.RetrocessionaireShare)
                .Index(t => t.RetroReferralReasonId)
                .Index(t => t.MlreReferralReasonId)
                .Index(t => t.RetroReviewedById)
                .Index(t => t.RetroReviewedAt)
                .Index(t => t.IsValueAddedService)
                .Index(t => t.IsClaimCaseStudy)
                .Index(t => t.CompletedCaseStudyMaterialAt)
                .Index(t => t.AssessedById)
                .Index(t => t.AssessedAt)
                .Index(t => t.ReviewedById)
                .Index(t => t.ReviewedAt)
                .Index(t => t.AssignedById)
                .Index(t => t.AssignedAt)
                .Index(t => t.TreatyCode)
                .Index(t => t.TreatyType)
                .Index(t => t.TreatyShare)
                .Index(t => t.PersonInChargeId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RetroRegisterBatchDirectRetros",
                c => new
                {
                    RetroRegisterBatchId = c.Int(nullable: false),
                    DirectRetroId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.RetroRegisterBatchId, t.DirectRetroId })
                .ForeignKey("dbo.DirectRetro", t => t.DirectRetroId)
                .ForeignKey("dbo.RetroRegisterBatches", t => t.RetroRegisterBatchId)
                .Index(t => t.RetroRegisterBatchId)
                .Index(t => t.DirectRetroId);

            CreateTable(
                "dbo.RetroRegisterBatches",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    BatchNo = c.Int(nullable: false),
                    BatchDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    TotalInvoice = c.Int(nullable: false),
                    Status = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.BatchNo)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RetroRegisterBatchFiles",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    RetroRegisterBatchId = c.Int(nullable: false),
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
                .ForeignKey("dbo.RetroRegisterBatches", t => t.RetroRegisterBatchId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.RetroRegisterBatchId)
                .Index(t => t.FileName)
                .Index(t => t.HashFileName)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RetroRegisterHistories",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CutOffId = c.Int(nullable: false),
                    RetroRegisterId = c.Int(nullable: false),
                    RetroRegisterBatchId = c.Int(nullable: false),
                    RetroStatementType = c.Int(nullable: false),
                    RetroStatementNo = c.String(maxLength: 30),
                    RetroStatementDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    ReportCompletedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    SendToRetroDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    RetroPartyId = c.Int(),
                    CedantId = c.Int(),
                    TreatyCodeId = c.Int(),
                    RiskQuarter = c.String(maxLength: 10),
                    TreatyNumber = c.String(maxLength: 128),
                    Schedule = c.String(maxLength: 50),
                    TreatyType = c.String(maxLength: 50),
                    LOB = c.String(maxLength: 50),
                    AccountFor = c.String(maxLength: 255),
                    Year1st = c.Double(),
                    Renewal = c.Double(),
                    ReserveCededBegin = c.Double(),
                    ReserveCededEnd = c.Double(),
                    RiskChargeCededBegin = c.Double(),
                    RiskChargeCededEnd = c.Double(),
                    AverageReserveCeded = c.Double(),
                    Gross1st = c.Double(),
                    GrossRen = c.Double(),
                    AltPremium = c.Double(),
                    Discount1st = c.Double(),
                    DiscountRen = c.Double(),
                    DiscountAlt = c.Double(),
                    RiskPremium = c.Double(),
                    Claims = c.Double(),
                    ProfitCommission = c.Double(),
                    SurrenderVal = c.Double(),
                    NoClaimBonus = c.Double(),
                    RetrocessionMarketingFee = c.Double(),
                    AgreedDBCommission = c.Double(),
                    NetTotalAmount = c.Double(),
                    NbCession = c.Int(),
                    NbSumReins = c.Double(),
                    RnCession = c.Int(),
                    RnSumReins = c.Double(),
                    AltCession = c.Int(),
                    AltSumReins = c.Double(),
                    Frequency = c.Int(),
                    PreparedById = c.Int(),
                    OriginalSoaQuarter = c.String(maxLength: 10),
                    RetroConfirmationDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    ValuationGross1st = c.Double(),
                    ValuationGrossRen = c.Double(),
                    ValuationDiscount1st = c.Double(),
                    ValuationDiscountRen = c.Double(),
                    ValuationCom1st = c.Double(),
                    ValuationComRen = c.Double(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedById = c.Int(),
                    SoaDataBatchId = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cedants", t => t.CedantId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.CutOff", t => t.CutOffId)
                .ForeignKey("dbo.Users", t => t.PreparedById)
                .ForeignKey("dbo.RetroParties", t => t.RetroPartyId)
                .ForeignKey("dbo.RetroRegister", t => t.RetroRegisterId)
                .ForeignKey("dbo.RetroRegisterBatches", t => t.RetroRegisterBatchId)
                .ForeignKey("dbo.SoaDataBatches", t => t.SoaDataBatchId)
                .ForeignKey("dbo.TreatyCodes", t => t.TreatyCodeId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.CutOffId)
                .Index(t => t.RetroRegisterId)
                .Index(t => t.RetroRegisterBatchId)
                .Index(t => t.RetroStatementType)
                .Index(t => t.RetroStatementNo)
                .Index(t => t.RetroPartyId)
                .Index(t => t.CedantId)
                .Index(t => t.TreatyCodeId)
                .Index(t => t.PreparedById)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById)
                .Index(t => t.SoaDataBatchId);

            CreateTable(
                "dbo.RetroRegister",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    RetroRegisterBatchId = c.Int(nullable: false),
                    RetroStatementType = c.Int(nullable: false),
                    RetroStatementNo = c.String(maxLength: 30),
                    RetroStatementDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    ReportCompletedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    SendToRetroDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    RetroPartyId = c.Int(),
                    CedantId = c.Int(),
                    TreatyCodeId = c.Int(),
                    RiskQuarter = c.String(maxLength: 10),
                    TreatyNumber = c.String(maxLength: 128),
                    Schedule = c.String(maxLength: 50),
                    TreatyType = c.String(maxLength: 50),
                    LOB = c.String(maxLength: 50),
                    AccountFor = c.String(maxLength: 255),
                    Year1st = c.Double(),
                    Renewal = c.Double(),
                    ReserveCededBegin = c.Double(),
                    ReserveCededEnd = c.Double(),
                    RiskChargeCededBegin = c.Double(),
                    RiskChargeCededEnd = c.Double(),
                    AverageReserveCeded = c.Double(),
                    Gross1st = c.Double(),
                    GrossRen = c.Double(),
                    AltPremium = c.Double(),
                    Discount1st = c.Double(),
                    DiscountRen = c.Double(),
                    DiscountAlt = c.Double(),
                    RiskPremium = c.Double(),
                    Claims = c.Double(),
                    ProfitCommission = c.Double(),
                    SurrenderVal = c.Double(),
                    NoClaimBonus = c.Double(),
                    RetrocessionMarketingFee = c.Double(),
                    AgreedDBCommission = c.Double(),
                    NetTotalAmount = c.Double(),
                    NbCession = c.Int(),
                    NbSumReins = c.Double(),
                    RnCession = c.Int(),
                    RnSumReins = c.Double(),
                    AltCession = c.Int(),
                    AltSumReins = c.Double(),
                    Frequency = c.Int(),
                    PreparedById = c.Int(),
                    OriginalSoaQuarter = c.String(maxLength: 10),
                    RetroConfirmationDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    ValuationGross1st = c.Double(),
                    ValuationGrossRen = c.Double(),
                    ValuationDiscount1st = c.Double(),
                    ValuationDiscountRen = c.Double(),
                    ValuationCom1st = c.Double(),
                    ValuationComRen = c.Double(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedById = c.Int(),
                    SoaDataBatchId = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cedants", t => t.CedantId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.PreparedById)
                .ForeignKey("dbo.RetroParties", t => t.RetroPartyId)
                .ForeignKey("dbo.RetroRegisterBatches", t => t.RetroRegisterBatchId)
                .ForeignKey("dbo.SoaDataBatches", t => t.SoaDataBatchId)
                .ForeignKey("dbo.TreatyCodes", t => t.TreatyCodeId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.RetroRegisterBatchId)
                .Index(t => t.RetroStatementType)
                .Index(t => t.RetroStatementNo)
                .Index(t => t.RetroPartyId)
                .Index(t => t.CedantId)
                .Index(t => t.TreatyCodeId)
                .Index(t => t.PreparedById)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById)
                .Index(t => t.SoaDataBatchId);

            CreateTable(
                "dbo.RetroStatements",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DirectRetroId = c.Int(nullable: false),
                    RiskQuarter = c.String(nullable: false, maxLength: 10),
                    RetroPartyId = c.Int(nullable: false),
                    Status = c.Int(nullable: false),
                    MlreRef = c.String(maxLength: 128),
                    CedingCompany = c.String(maxLength: 255),
                    TreatyNo = c.String(maxLength: 128),
                    TreatyType = c.String(maxLength: 128),
                    FromMlreTo = c.String(maxLength: 128),
                    AccountingPeriod = c.String(maxLength: 10),
                    AccountsFor = c.String(maxLength: 128),
                    DateReportCompleted = c.DateTime(precision: 7, storeType: "datetime2"),
                    DateSendToRetro = c.DateTime(precision: 7, storeType: "datetime2"),
                    ReserveCededBegin = c.Double(),
                    ReserveCededEnd = c.Double(),
                    RiskChargeCededBegin = c.Double(),
                    RiskChargeCededEnd = c.Double(),
                    AverageReserveCeded = c.Double(),
                    RiPremiumNB = c.Double(),
                    RiPremiumRN = c.Double(),
                    QuarterlyRiskPremium = c.Double(),
                    RiDiscountNB = c.Double(),
                    RiDiscountRN = c.Double(),
                    Claims = c.Double(),
                    ProfitComm = c.Double(),
                    SurrenderValue = c.Double(),
                    PaymentToTheReinsurer = c.Double(),
                    TotalNoOfPolicyNB = c.Int(),
                    TotalNoOfPolicyRN = c.Int(),
                    TotalSumReinsuredNB = c.Double(),
                    TotalSumReinsuredRN = c.Double(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.DirectRetro", t => t.DirectRetroId)
                .ForeignKey("dbo.RetroParties", t => t.RetroPartyId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.DirectRetroId)
                .Index(t => t.RetroPartyId)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RetroSummaries",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DirectRetroId = c.Int(nullable: false),
                    Month = c.Int(),
                    Year = c.Int(),
                    Type = c.String(maxLength: 10),
                    NoOfPolicy = c.Int(),
                    TotalSar = c.Double(),
                    TotalRiPremium = c.Double(),
                    TotalDiscount = c.Double(),
                    NoOfClaims = c.Int(),
                    TotalClaims = c.Double(),
                    RetroParty1 = c.String(maxLength: 128),
                    RetroParty2 = c.String(maxLength: 128),
                    RetroParty3 = c.String(maxLength: 128),
                    RetroShare1 = c.Double(),
                    RetroShare2 = c.Double(),
                    RetroShare3 = c.Double(),
                    RetroRiPremium1 = c.Double(),
                    RetroRiPremium2 = c.Double(),
                    RetroRiPremium3 = c.Double(),
                    RetroDiscount1 = c.Double(),
                    RetroDiscount2 = c.Double(),
                    RetroDiscount3 = c.Double(),
                    RetroClaims1 = c.Double(),
                    RetroClaims2 = c.Double(),
                    RetroClaims3 = c.Double(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.DirectRetro", t => t.DirectRetroId)
                .Index(t => t.DirectRetroId)
                .Index(t => t.Month)
                .Index(t => t.Year)
                .Index(t => t.Type)
                .Index(t => t.NoOfPolicy)
                .Index(t => t.TotalSar)
                .Index(t => t.TotalRiPremium)
                .Index(t => t.TotalDiscount)
                .Index(t => t.NoOfClaims)
                .Index(t => t.TotalClaims)
                .Index(t => t.RetroParty1)
                .Index(t => t.RetroParty2)
                .Index(t => t.RetroParty3)
                .Index(t => t.RetroShare1)
                .Index(t => t.RetroShare2)
                .Index(t => t.RetroShare3)
                .Index(t => t.RetroRiPremium1)
                .Index(t => t.RetroRiPremium2)
                .Index(t => t.RetroRiPremium3)
                .Index(t => t.RetroDiscount1)
                .Index(t => t.RetroDiscount2)
                .Index(t => t.RetroDiscount3)
                .Index(t => t.RetroClaims1)
                .Index(t => t.RetroClaims2)
                .Index(t => t.RetroClaims3)
                .Index(t => t.CreatedById);

            CreateTable(
                "dbo.Salutations",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 30),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.SoaData",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    SoaDataBatchId = c.Int(nullable: false),
                    SoaDataFileId = c.Int(),
                    MappingStatus = c.Int(nullable: false),
                    Errors = c.String(storeType: "ntext"),
                    CompanyName = c.String(maxLength: 255),
                    BusinessCode = c.String(maxLength: 12),
                    TreatyId = c.String(maxLength: 30),
                    TreatyCode = c.String(maxLength: 35),
                    TreatyMode = c.String(maxLength: 1),
                    TreatyType = c.String(maxLength: 10),
                    PlanBlock = c.String(maxLength: 35),
                    RiskMonth = c.Int(),
                    SoaQuarter = c.String(maxLength: 32),
                    RiskQuarter = c.String(maxLength: 32),
                    NbPremium = c.Double(),
                    RnPremium = c.Double(),
                    AltPremium = c.Double(),
                    GrossPremium = c.Double(),
                    TotalDiscount = c.Double(),
                    RiskPremium = c.Int(),
                    NoClaimBonus = c.Double(),
                    Levy = c.Double(),
                    Claim = c.Double(),
                    ProfitComm = c.Double(),
                    SurrenderValue = c.Double(),
                    Gst = c.Double(),
                    ModcoReserveIncome = c.Double(),
                    RiDeposit = c.Double(),
                    DatabaseCommission = c.Double(),
                    AdministrationContribution = c.Double(),
                    ShareOfRiCommissionFromCompulsoryCession = c.Double(),
                    RecaptureFee = c.Double(),
                    CreditCardCharges = c.Double(),
                    BrokerageFee = c.Double(),
                    TotalCommission = c.Double(),
                    NetTotalAmount = c.Double(),
                    SoaReceivedDate = c.DateTime(storeType: "date"),
                    BordereauxReceivedDate = c.DateTime(storeType: "date"),
                    StatementStatus = c.String(maxLength: 60),
                    Remarks1 = c.String(maxLength: 60),
                    CurrencyCode = c.String(maxLength: 3),
                    CurrencyRate = c.Double(),
                    SoaStatus = c.String(maxLength: 30),
                    ConfirmationDate = c.DateTime(storeType: "date"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.SoaDataBatches", t => t.SoaDataBatchId)
                .ForeignKey("dbo.SoaDataFiles", t => t.SoaDataFileId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.SoaDataBatchId)
                .Index(t => t.SoaDataFileId)
                .Index(t => t.MappingStatus)
                .Index(t => t.BusinessCode)
                .Index(t => t.TreatyCode)
                .Index(t => t.SoaQuarter)
                .Index(t => t.RiskQuarter)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.SoaDataFiles",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    SoaDataBatchId = c.Int(nullable: false),
                    RawFileId = c.Int(nullable: false),
                    TreatyId = c.Int(),
                    Mode = c.Int(nullable: false),
                    Status = c.Int(nullable: false),
                    Errors = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.RawFiles", t => t.RawFileId)
                .ForeignKey("dbo.SoaDataBatches", t => t.SoaDataBatchId)
                .ForeignKey("dbo.Treaties", t => t.TreatyId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.SoaDataBatchId)
                .Index(t => t.RawFileId)
                .Index(t => t.TreatyId)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.SoaDataBatchStatusFiles",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    SoaDataBatchId = c.Int(nullable: false),
                    StatusHistoryId = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.SoaDataBatches", t => t.SoaDataBatchId)
                .ForeignKey("dbo.StatusHistories", t => t.StatusHistoryId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.SoaDataBatchId)
                .Index(t => t.StatusHistoryId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.SoaDataCompiledSummaries",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    SoaDataBatchId = c.Int(nullable: false),
                    InvoiceType = c.Int(nullable: false),
                    InvoiceDate = c.DateTime(nullable: false),
                    StatementReceivedDate = c.DateTime(),
                    BusinessCode = c.String(maxLength: 12),
                    TreatyCode = c.String(maxLength: 35),
                    SoaQuarter = c.String(maxLength: 32),
                    RiskQuarter = c.String(maxLength: 32),
                    AccountDescription = c.String(maxLength: 128),
                    NbPremium = c.Double(),
                    RnPremium = c.Double(),
                    AltPremium = c.Double(),
                    TotalDiscount = c.Double(),
                    RiskPremium = c.Double(),
                    NoClaimBonus = c.Double(),
                    Levy = c.Double(),
                    Claim = c.Double(),
                    ProfitComm = c.Double(),
                    SurrenderValue = c.Double(),
                    ModcoReserveIncome = c.Double(),
                    RiDeposit = c.Double(),
                    DatabaseCommission = c.Double(),
                    AdministrationContribution = c.Double(),
                    ShareOfRiCommissionFromCompulsoryCession = c.Double(),
                    RecaptureFee = c.Double(),
                    CreditCardCharges = c.Double(),
                    BrokerageFee = c.Double(),
                    NetTotalAmount = c.Double(),
                    ReasonOfAdjustment1 = c.String(maxLength: 128),
                    ReasonOfAdjustment2 = c.String(maxLength: 128),
                    InvoiceNumber1 = c.String(maxLength: 64),
                    InvoiceDate1 = c.DateTime(),
                    Amount1 = c.Double(),
                    InvoiceNumber2 = c.String(maxLength: 64),
                    InvoiceDate2 = c.DateTime(),
                    Amount2 = c.Double(),
                    FilingReference = c.String(maxLength: 128),
                    ServiceFeePercentage = c.Double(),
                    ServiceFee = c.Double(),
                    Sst = c.Double(),
                    TotalAmount = c.Double(),
                    NbDiscount = c.Double(),
                    RnDiscount = c.Double(),
                    AltDiscount = c.Double(),
                    NbCession = c.Int(),
                    RnCession = c.Int(),
                    AltCession = c.Int(),
                    NbSar = c.Double(),
                    RnSar = c.Double(),
                    AltSar = c.Double(),
                    DTH = c.Double(),
                    TPA = c.Double(),
                    TPS = c.Double(),
                    PPD = c.Double(),
                    CCA = c.Double(),
                    CCS = c.Double(),
                    PA = c.Double(),
                    HS = c.Double(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.SoaDataBatches", t => t.SoaDataBatchId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.SoaDataBatchId)
                .Index(t => t.BusinessCode)
                .Index(t => t.TreatyCode)
                .Index(t => t.SoaQuarter)
                .Index(t => t.RiskQuarter)
                .Index(t => t.InvoiceNumber1)
                .Index(t => t.InvoiceNumber2)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.SoaDataPostValidationDifferences",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    SoaDataBatchId = c.Int(nullable: false),
                    Type = c.Int(nullable: false),
                    BusinessCode = c.String(maxLength: 12),
                    TreatyCode = c.String(maxLength: 35),
                    SoaQuarter = c.String(maxLength: 32),
                    RiskQuarter = c.String(maxLength: 32),
                    RiskMonth = c.Int(),
                    GrossPremium = c.Double(),
                    DifferenceNetTotalAmount = c.Double(),
                    DifferencePercetage = c.Double(),
                    CurrencyCode = c.String(maxLength: 3),
                    CurrencyRate = c.Double(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.SoaDataBatches", t => t.SoaDataBatchId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.SoaDataBatchId)
                .Index(t => t.Type)
                .Index(t => t.BusinessCode)
                .Index(t => t.TreatyCode)
                .Index(t => t.SoaQuarter)
                .Index(t => t.RiskQuarter)
                .Index(t => t.RiskMonth)
                .Index(t => t.CurrencyCode)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.SoaDataPostValidations",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    SoaDataBatchId = c.Int(nullable: false),
                    Type = c.Int(nullable: false),
                    BusinessCode = c.String(maxLength: 12),
                    TreatyCode = c.String(maxLength: 35),
                    SoaQuarter = c.String(maxLength: 32),
                    RiskQuarter = c.String(maxLength: 32),
                    RiskMonth = c.Int(),
                    NbPremium = c.Double(),
                    RnPremium = c.Double(),
                    AltPremium = c.Double(),
                    GrossPremium = c.Double(),
                    NbDiscount = c.Double(),
                    RnDiscount = c.Double(),
                    AltDiscount = c.Double(),
                    TotalDiscount = c.Double(),
                    NoClaimBonus = c.Double(),
                    Claim = c.Double(),
                    SurrenderValue = c.Double(),
                    Gst = c.Double(),
                    NetTotalAmount = c.Double(),
                    CurrencyCode = c.String(maxLength: 3),
                    CurrencyRate = c.Double(),
                    NbCession = c.Int(),
                    RnCession = c.Int(),
                    AltCession = c.Int(),
                    NbSar = c.Double(),
                    RnSar = c.Double(),
                    AltSar = c.Double(),
                    DTH = c.Double(),
                    TPA = c.Double(),
                    TPS = c.Double(),
                    PPD = c.Double(),
                    CCA = c.Double(),
                    CCS = c.Double(),
                    PA = c.Double(),
                    HS = c.Double(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.SoaDataBatches", t => t.SoaDataBatchId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.SoaDataBatchId)
                .Index(t => t.Type)
                .Index(t => t.BusinessCode)
                .Index(t => t.TreatyCode)
                .Index(t => t.SoaQuarter)
                .Index(t => t.RiskQuarter)
                .Index(t => t.RiskMonth)
                .Index(t => t.CurrencyCode)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.SoaDataRiDataSummaries",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    SoaDataBatchId = c.Int(nullable: false),
                    BusinessCode = c.String(maxLength: 12),
                    TreatyCode = c.String(maxLength: 35),
                    SoaQuarter = c.String(maxLength: 32),
                    RiskQuarter = c.String(maxLength: 32),
                    RiskMonth = c.Int(),
                    NbPremium = c.Double(),
                    RnPremium = c.Double(),
                    AltPremium = c.Double(),
                    GrossPremium = c.Double(),
                    TotalDiscount = c.Double(),
                    NoClaimBonus = c.Double(),
                    Claim = c.Double(),
                    SurrenderValue = c.Double(),
                    Gst = c.Double(),
                    NetTotalAmount = c.Double(),
                    CurrencyCode = c.String(maxLength: 3),
                    CurrencyRate = c.Double(),
                    NbDiscount = c.Double(),
                    RnDiscount = c.Double(),
                    AltDiscount = c.Double(),
                    NbCession = c.Int(),
                    RnCession = c.Int(),
                    AltCession = c.Int(),
                    NbSar = c.Double(),
                    RnSar = c.Double(),
                    AltSar = c.Double(),
                    DTH = c.Double(),
                    TPA = c.Double(),
                    TPS = c.Double(),
                    PPD = c.Double(),
                    CCA = c.Double(),
                    CCS = c.Double(),
                    PA = c.Double(),
                    HS = c.Double(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.SoaDataBatches", t => t.SoaDataBatchId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.SoaDataBatchId)
                .Index(t => t.BusinessCode)
                .Index(t => t.TreatyCode)
                .Index(t => t.SoaQuarter)
                .Index(t => t.RiskQuarter)
                .Index(t => t.RiskMonth)
                .Index(t => t.CurrencyCode)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.TreatyDiscountTableDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyDiscountTableId = c.Int(nullable: false),
                    CedingPlanCode = c.String(nullable: false, maxLength: 30),
                    BenefitId = c.Int(nullable: false),
                    AgeFrom = c.Int(nullable: false),
                    AgeTo = c.Int(nullable: false),
                    Discount = c.Double(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Benefits", t => t.BenefitId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyDiscountTables", t => t.TreatyDiscountTableId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyDiscountTableId)
                .Index(t => t.BenefitId)
                .Index(t => t.AgeFrom)
                .Index(t => t.AgeTo)
                .Index(t => t.Discount)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.UnderwritingRemarks",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ClaimRegisterId = c.Int(nullable: false),
                    Content = c.String(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClaimRegister", t => t.ClaimRegisterId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.ClaimRegisterId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            AddColumn("dbo.Modules", "Index", c => c.Int(nullable: false));
            AddColumn("dbo.Benefits", "EffectiveStartDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AddColumn("dbo.Benefits", "EffectiveEndDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AddColumn("dbo.Benefits", "ValuationBenefitCodePickListDetailId", c => c.Int());
            AddColumn("dbo.Benefits", "GST", c => c.Boolean(nullable: false));
            AddColumn("dbo.Cedants", "CedingCompanyTypePickListDetailId", c => c.Int());
            AddColumn("dbo.Cedants", "PartyCode", c => c.String(nullable: false, maxLength: 30));
            AddColumn("dbo.Cedants", "AccountCode", c => c.String(maxLength: 10));
            AddColumn("dbo.Documents", "RemarkId", c => c.Int());
            AddColumn("dbo.Documents", "IsPrivate", c => c.Boolean(nullable: false));
            AddColumn("dbo.Documents", "DepartmentId", c => c.Int());
            AddColumn("dbo.Mfrs17CellMappings", "LoaCode", c => c.String(maxLength: 10));
            AddColumn("dbo.Mfrs17CellMappings", "ProfitComm", c => c.String(maxLength: 1));
            AddColumn("dbo.PickLists", "StandardClaimDataOutputId", c => c.Int());
            AddColumn("dbo.PickLists", "StandardSoaDataOutputId", c => c.Int());
            AddColumn("dbo.PickLists", "IsEditable", c => c.Boolean(nullable: false));
            AddColumn("dbo.Mfrs17ReportingDetailRiDatas", "RiDataWarehouseId", c => c.Int());
            AddColumn("dbo.Mfrs17ReportingDetailRiDatas", "RiDataWarehouseHistoryId", c => c.Int(nullable: false));
            AddColumn("dbo.Mfrs17ReportingDetails", "Status", c => c.Int());
            AddColumn("dbo.Mfrs17ReportingDetails", "IsModified", c => c.Boolean(nullable: false));
            AddColumn("dbo.Mfrs17Reportings", "GenerateModifiedOnly", c => c.Boolean());
            AddColumn("dbo.Mfrs17Reportings", "CutOffId", c => c.Int(nullable: false));
            AddColumn("dbo.RiData", "RecordType", c => c.Int(nullable: false));
            AddColumn("dbo.RiData", "OriginalEntryId", c => c.Int());
            AddColumn("dbo.RiData", "PreComputation1Status", c => c.Int(nullable: false));
            AddColumn("dbo.RiData", "PreComputation2Status", c => c.Int(nullable: false));
            AddColumn("dbo.RiData", "PostComputationStatus", c => c.Int(nullable: false));
            AddColumn("dbo.RiData", "PostValidationStatus", c => c.Int(nullable: false));
            AddColumn("dbo.RiData", "ProcessWarehouseStatus", c => c.Int(nullable: false));
            AddColumn("dbo.RiData", "CurrencyRate", c => c.Double());
            AddColumn("dbo.RiData", "NoClaimBonus", c => c.Double());
            AddColumn("dbo.RiData", "SurrenderValue", c => c.Double());
            AddColumn("dbo.RiData", "DatabaseCommision", c => c.Double());
            AddColumn("dbo.RiData", "GrossPremiumAlt", c => c.Double());
            AddColumn("dbo.RiData", "NetPremiumAlt", c => c.Double());
            AddColumn("dbo.RiData", "Layer1FlatExtraPremium", c => c.Double());
            AddColumn("dbo.RiData", "TransactionPremium", c => c.Double());
            AddColumn("dbo.RiData", "OriginalPremium", c => c.Double());
            AddColumn("dbo.RiData", "TransactionDiscount", c => c.Double());
            AddColumn("dbo.RiData", "OriginalDiscount", c => c.Double());
            AddColumn("dbo.RiData", "BrokerageFee", c => c.Double());
            AddColumn("dbo.RiData", "MaxUwRating", c => c.Double());
            AddColumn("dbo.RiData", "RetentionCap", c => c.Double());
            AddColumn("dbo.RiData", "AarCap", c => c.Double());
            AddColumn("dbo.RiData", "RiRate", c => c.Double());
            AddColumn("dbo.RiData", "RiRate2", c => c.Double());
            AddColumn("dbo.RiData", "AnnuityFactor", c => c.Double());
            AddColumn("dbo.RiData", "SumAssuredOffered", c => c.Double());
            AddColumn("dbo.RiData", "UwRatingOffered", c => c.Double());
            AddColumn("dbo.RiData", "FlatExtraAmountOffered", c => c.Double());
            AddColumn("dbo.RiData", "FlatExtraDuration", c => c.Int());
            AddColumn("dbo.RiData", "EffectiveDate", c => c.DateTime());
            AddColumn("dbo.RiData", "OfferLetterSentDate", c => c.DateTime());
            AddColumn("dbo.RiData", "RiskPeriodStartDate", c => c.DateTime());
            AddColumn("dbo.RiData", "RiskPeriodEndDate", c => c.DateTime());
            AddColumn("dbo.RiData", "Mfrs17AnnualCohort", c => c.Int());
            AddColumn("dbo.RiData", "MaxExpiryAge", c => c.Int());
            AddColumn("dbo.RiData", "MinIssueAge", c => c.Int());
            AddColumn("dbo.RiData", "MaxIssueAge", c => c.Int());
            AddColumn("dbo.RiData", "MinAar", c => c.Double());
            AddColumn("dbo.RiData", "MaxAar", c => c.Double());
            AddColumn("dbo.RiData", "CorridorLimit", c => c.Double());
            AddColumn("dbo.RiData", "Abl", c => c.Double());
            AddColumn("dbo.RiData", "RatePerBasisUnit", c => c.Int());
            AddColumn("dbo.RiData", "RiDiscountRate", c => c.Double());
            AddColumn("dbo.RiData", "LargeSaDiscount", c => c.Double());
            AddColumn("dbo.RiData", "GroupSizeDiscount", c => c.Double());
            AddColumn("dbo.RiData", "EwarpNumber", c => c.Int());
            AddColumn("dbo.RiData", "EwarpActionCode", c => c.String(maxLength: 10));
            AddColumn("dbo.RiData", "RetentionShare", c => c.Double());
            AddColumn("dbo.RiData", "AarShare", c => c.Double());
            AddColumn("dbo.RiData", "ProfitComm", c => c.String(maxLength: 1));
            AddColumn("dbo.RiData", "TotalDirectRetroAar", c => c.Double());
            AddColumn("dbo.RiData", "TotalDirectRetroGrossPremium", c => c.Double());
            AddColumn("dbo.RiData", "TotalDirectRetroDiscount", c => c.Double());
            AddColumn("dbo.RiData", "TotalDirectRetroNetPremium", c => c.Double());
            AddColumn("dbo.RiData", "TreatyType", c => c.String(maxLength: 20));
            AddColumn("dbo.RiData", "MaxApLoading", c => c.Double());
            AddColumn("dbo.RiData", "MlreInsuredAttainedAgeAtCurrentMonth", c => c.Int());
            AddColumn("dbo.RiData", "MlreInsuredAttainedAgeAtPreviousMonth", c => c.Int());
            AddColumn("dbo.RiData", "InsuredAttainedAgeCheck", c => c.Boolean(nullable: false));
            AddColumn("dbo.RiData", "MaxExpiryAgeCheck", c => c.Boolean(nullable: false));
            AddColumn("dbo.RiData", "MlrePolicyIssueAge", c => c.Int());
            AddColumn("dbo.RiData", "PolicyIssueAgeCheck", c => c.Boolean(nullable: false));
            AddColumn("dbo.RiData", "MinIssueAgeCheck", c => c.Boolean(nullable: false));
            AddColumn("dbo.RiData", "MaxIssueAgeCheck", c => c.Boolean(nullable: false));
            AddColumn("dbo.RiData", "MaxUwRatingCheck", c => c.Boolean(nullable: false));
            AddColumn("dbo.RiData", "ApLoadingCheck", c => c.Boolean(nullable: false));
            AddColumn("dbo.RiData", "EffectiveDateCheck", c => c.Boolean(nullable: false));
            AddColumn("dbo.RiData", "MinAarCheck", c => c.Boolean(nullable: false));
            AddColumn("dbo.RiData", "MaxAarCheck", c => c.Boolean(nullable: false));
            AddColumn("dbo.RiData", "CorridorLimitCheck", c => c.Boolean(nullable: false));
            AddColumn("dbo.RiData", "AblCheck", c => c.Boolean(nullable: false));
            AddColumn("dbo.RiData", "RetentionCheck", c => c.Boolean(nullable: false));
            AddColumn("dbo.RiData", "AarCheck", c => c.Boolean(nullable: false));
            AddColumn("dbo.RiData", "MlreStandardPremium", c => c.Double());
            AddColumn("dbo.RiData", "MlreSubstandardPremium", c => c.Double());
            AddColumn("dbo.RiData", "MlreFlatExtraPremium", c => c.Double());
            AddColumn("dbo.RiData", "MlreGrossPremium", c => c.Double());
            AddColumn("dbo.RiData", "MlreStandardDiscount", c => c.Double());
            AddColumn("dbo.RiData", "MlreSubstandardDiscount", c => c.Double());
            AddColumn("dbo.RiData", "MlreLargeSaDiscount", c => c.Double());
            AddColumn("dbo.RiData", "MlreGroupSizeDiscount", c => c.Double());
            AddColumn("dbo.RiData", "MlreVitalityDiscount", c => c.Double());
            AddColumn("dbo.RiData", "MlreTotalDiscount", c => c.Double());
            AddColumn("dbo.RiData", "MlreNetPremium", c => c.Double());
            AddColumn("dbo.RiData", "NetPremiumCheck", c => c.Double());
            AddColumn("dbo.RiData", "ServiceFeePercentage", c => c.Double());
            AddColumn("dbo.RiData", "ServiceFee", c => c.Double());
            AddColumn("dbo.RiData", "MlreBrokerageFee", c => c.Double());
            AddColumn("dbo.RiData", "MlreDatabaseCommission", c => c.Double());
            AddColumn("dbo.RiData", "ValidityDayCheck", c => c.Boolean(nullable: false));
            AddColumn("dbo.RiData", "SumAssuredOfferedCheck", c => c.Boolean(nullable: false));
            AddColumn("dbo.RiData", "UwRatingCheck", c => c.Boolean(nullable: false));
            AddColumn("dbo.RiData", "FlatExtraAmountCheck", c => c.Boolean(nullable: false));
            AddColumn("dbo.RiData", "FlatExtraDurationCheck", c => c.Boolean(nullable: false));
            AddColumn("dbo.RiData", "RetroParty1", c => c.String(maxLength: 128));
            AddColumn("dbo.RiData", "RetroParty2", c => c.String(maxLength: 128));
            AddColumn("dbo.RiData", "RetroParty3", c => c.String(maxLength: 128));
            AddColumn("dbo.RiData", "RetroShare1", c => c.Double());
            AddColumn("dbo.RiData", "RetroShare2", c => c.Double());
            AddColumn("dbo.RiData", "RetroShare3", c => c.Double());
            AddColumn("dbo.RiData", "RetroAar1", c => c.Double());
            AddColumn("dbo.RiData", "RetroAar2", c => c.Double());
            AddColumn("dbo.RiData", "RetroAar3", c => c.Double());
            AddColumn("dbo.RiData", "RetroReinsurancePremium1", c => c.Double());
            AddColumn("dbo.RiData", "RetroReinsurancePremium2", c => c.Double());
            AddColumn("dbo.RiData", "RetroReinsurancePremium3", c => c.Double());
            AddColumn("dbo.RiData", "RetroDiscount1", c => c.Double());
            AddColumn("dbo.RiData", "RetroDiscount2", c => c.Double());
            AddColumn("dbo.RiData", "RetroDiscount3", c => c.Double());
            AddColumn("dbo.RiData", "RetroNetPremium1", c => c.Double());
            AddColumn("dbo.RiData", "RetroNetPremium2", c => c.Double());
            AddColumn("dbo.RiData", "RetroNetPremium3", c => c.Double());
            AddColumn("dbo.RiData", "AarShare2", c => c.Double());
            AddColumn("dbo.RiData", "AarCap2", c => c.Double());
            AddColumn("dbo.RiDataBatches", "ProcessWarehouseStatus", c => c.Int(nullable: false));
            AddColumn("dbo.RiDataBatches", "TotalPreComputation1FailedStatus", c => c.Int(nullable: false));
            AddColumn("dbo.RiDataBatches", "TotalPreComputation2FailedStatus", c => c.Int(nullable: false));
            AddColumn("dbo.RiDataBatches", "TotalPreValidationFailedStatus", c => c.Int(nullable: false));
            AddColumn("dbo.RiDataBatches", "TotalPostComputationFailedStatus", c => c.Int(nullable: false));
            AddColumn("dbo.RiDataBatches", "TotalPostValidationFailedStatus", c => c.Int(nullable: false));
            AddColumn("dbo.RiDataBatches", "RecordType", c => c.Int(nullable: false));
            AddColumn("dbo.RiDataBatches", "ReceivedAt", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.RiDataBatches", "SoaDataBatchId", c => c.Int());
            AddColumn("dbo.RiDataBatches", "FinalisedAt", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AddColumn("dbo.Treaties", "BusinessOriginPickListDetailId", c => c.Int());
            AddColumn("dbo.Treaties", "LineOfBusinessPickListDetailId", c => c.Int());
            AddColumn("dbo.Treaties", "BlockDescription", c => c.String(maxLength: 255));
            AddColumn("dbo.RateTables", "PolicyTermFrom", c => c.Int());
            AddColumn("dbo.RateTables", "PolicyTermTo", c => c.Int());
            AddColumn("dbo.RateTables", "PolicyDurationFrom", c => c.Double());
            AddColumn("dbo.RateTables", "PolicyDurationTo", c => c.Double());
            AddColumn("dbo.RateTables", "RateId", c => c.Int());
            AddColumn("dbo.RateTables", "CedantId", c => c.Int());
            AddColumn("dbo.RateTables", "RiDiscountId", c => c.Int());
            AddColumn("dbo.RateTables", "LargeDiscountId", c => c.Int());
            AddColumn("dbo.RateTables", "GroupDiscountId", c => c.Int());
            AddColumn("dbo.Remarks", "IsPrivate", c => c.Boolean(nullable: false));
            AddColumn("dbo.Remarks", "DepartmentId", c => c.Int());
            AddColumn("dbo.Remarks", "HasFollowUp", c => c.Boolean(nullable: false));
            AddColumn("dbo.Remarks", "FollowUpStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Remarks", "FollowUpAt", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AddColumn("dbo.Remarks", "FollowUpUserId", c => c.Int());
            AddColumn("dbo.RiDataComputations", "Step", c => c.Int(nullable: false));
            AddColumn("dbo.TreatyCodes", "AccountFor", c => c.String(storeType: "ntext"));
            AddColumn("dbo.TreatyCodes", "TreatyTypePickListDetailId", c => c.Int());
            AddColumn("dbo.TreatyCodes", "TreatyStatusPickListDetailId", c => c.Int());
            AddColumn("dbo.RiDataPreValidations", "Step", c => c.Int(nullable: false));
            AddColumn("dbo.TreatyBenefitCodeMappings", "ProfitComm", c => c.String(maxLength: 1));
            AddColumn("dbo.TreatyBenefitCodeMappings", "MaxExpiryAge", c => c.Int());
            AddColumn("dbo.TreatyBenefitCodeMappings", "MinIssueAge", c => c.Int());
            AddColumn("dbo.TreatyBenefitCodeMappings", "MaxIssueAge", c => c.Int());
            AddColumn("dbo.TreatyBenefitCodeMappings", "MaxUwRating", c => c.Double());
            AddColumn("dbo.TreatyBenefitCodeMappings", "ApLoading", c => c.Double());
            AddColumn("dbo.TreatyBenefitCodeMappings", "MinAar", c => c.Double());
            AddColumn("dbo.TreatyBenefitCodeMappings", "MaxAar", c => c.Double());
            AddColumn("dbo.TreatyBenefitCodeMappings", "AblAmount", c => c.Double());
            AddColumn("dbo.TreatyBenefitCodeMappings", "RetentionShare", c => c.Double());
            AddColumn("dbo.TreatyBenefitCodeMappings", "RetentionCap", c => c.Double());
            AddColumn("dbo.TreatyBenefitCodeMappings", "RiShare", c => c.Double());
            AddColumn("dbo.TreatyBenefitCodeMappings", "RiShareCap", c => c.Double());
            AddColumn("dbo.TreatyBenefitCodeMappings", "ServiceFee", c => c.Double());
            AddColumn("dbo.TreatyBenefitCodeMappings", "WakalahFee", c => c.Double());
            AlterColumn("dbo.Benefits", "Type", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Benefits", "Description", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Mfrs17ReportingDetailRiDatas", "RiDataId", c => c.Int());
            AlterColumn("dbo.RiData", "RiDataFileId", c => c.Int());
            AlterColumn("dbo.RateTables", "RateTableCode", c => c.String(maxLength: 50));
            AlterColumn("dbo.RiDataComputations", "StandardOutputId", c => c.Int());
            AddPrimaryKey("dbo.Mfrs17ReportingDetailRiDatas", new[] { "Mfrs17ReportingDetailId", "RiDataWarehouseHistoryId" });
            CreateIndex("dbo.PickLists", "StandardClaimDataOutputId");
            CreateIndex("dbo.PickLists", "StandardSoaDataOutputId");
            CreateIndex("dbo.Cedants", "CedingCompanyTypePickListDetailId");
            CreateIndex("dbo.Cedants", "PartyCode");
            CreateIndex("dbo.Cedants", "AccountCode");
            CreateIndex("dbo.Benefits", "Type");
            CreateIndex("dbo.Benefits", "Description");
            CreateIndex("dbo.Benefits", "ValuationBenefitCodePickListDetailId");
            CreateIndex("dbo.Treaties", "BusinessOriginPickListDetailId");
            CreateIndex("dbo.Treaties", "LineOfBusinessPickListDetailId");
            CreateIndex("dbo.Treaties", "BlockDescription");
            CreateIndex("dbo.RiDataBatches", "ProcessWarehouseStatus");
            CreateIndex("dbo.RiDataBatches", "RecordType");
            CreateIndex("dbo.RiDataBatches", "SoaDataBatchId");
            CreateIndex("dbo.TreatyCodes", "TreatyTypePickListDetailId");
            CreateIndex("dbo.TreatyCodes", "TreatyStatusPickListDetailId");
            CreateIndex("dbo.Documents", "RemarkId");
            CreateIndex("dbo.Documents", "DepartmentId");
            CreateIndex("dbo.Remarks", "DepartmentId");
            CreateIndex("dbo.Remarks", "FollowUpUserId");
            CreateIndex("dbo.Mfrs17CellMappings", "ProfitComm");
            CreateIndex("dbo.Mfrs17ReportingDetailRiDatas", "RiDataWarehouseHistoryId");
            CreateIndex("dbo.Mfrs17ReportingDetails", "Status");
            CreateIndex("dbo.Mfrs17ReportingDetails", "IsModified");
            CreateIndex("dbo.Mfrs17Reportings", "CutOffId");
            CreateIndex("dbo.RateTables", "RateTableCode");
            CreateIndex("dbo.RateTables", "PolicyTermFrom");
            CreateIndex("dbo.RateTables", "PolicyTermTo");
            CreateIndex("dbo.RateTables", "PolicyDurationFrom");
            CreateIndex("dbo.RateTables", "PolicyDurationTo");
            CreateIndex("dbo.RateTables", "RateId");
            CreateIndex("dbo.RateTables", "CedantId");
            CreateIndex("dbo.RateTables", "RiDiscountId");
            CreateIndex("dbo.RateTables", "LargeDiscountId");
            CreateIndex("dbo.RateTables", "GroupDiscountId");
            CreateIndex("dbo.RiData", "RiDataFileId");
            CreateIndex("dbo.RiData", "PreComputation1Status");
            CreateIndex("dbo.RiData", "PreComputation2Status");
            CreateIndex("dbo.RiData", "PostComputationStatus");
            CreateIndex("dbo.RiData", "PostValidationStatus");
            CreateIndex("dbo.RiData", "ProcessWarehouseStatus");
            CreateIndex("dbo.RiData", "RetroParty1");
            CreateIndex("dbo.RiData", "RetroParty2");
            CreateIndex("dbo.RiData", "RetroParty3");
            CreateIndex("dbo.RiData", "RetroShare1");
            CreateIndex("dbo.RiData", "RetroShare2");
            CreateIndex("dbo.RiData", "RetroShare3");
            CreateIndex("dbo.RiData", "RetroAar1");
            CreateIndex("dbo.RiData", "RetroAar2");
            CreateIndex("dbo.RiData", "RetroAar3");
            CreateIndex("dbo.RiData", "RetroReinsurancePremium1");
            CreateIndex("dbo.RiData", "RetroReinsurancePremium2");
            CreateIndex("dbo.RiData", "RetroReinsurancePremium3");
            CreateIndex("dbo.RiData", "RetroDiscount1");
            CreateIndex("dbo.RiData", "RetroDiscount2");
            CreateIndex("dbo.RiData", "RetroDiscount3");
            CreateIndex("dbo.RiData", "RetroNetPremium1");
            CreateIndex("dbo.RiData", "RetroNetPremium2");
            CreateIndex("dbo.RiData", "RetroNetPremium3");
            CreateIndex("dbo.RiDataComputations", "Step");
            CreateIndex("dbo.RiDataComputations", "StandardOutputId");
            CreateIndex("dbo.RiDataPreValidations", "Step");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "ProfitComm");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "MaxExpiryAge");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "MinIssueAge");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "MaxIssueAge");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "MaxUwRating");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "ApLoading");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "MinAar");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "MaxAar");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "AblAmount");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "RetentionShare");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "RetentionCap");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "RiShare");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "RiShareCap");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "ServiceFee");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "WakalahFee");
            AddForeignKey("dbo.PickLists", "StandardClaimDataOutputId", "dbo.StandardClaimDataOutputs", "Id");
            AddForeignKey("dbo.PickLists", "StandardSoaDataOutputId", "dbo.StandardSoaDataOutputs", "Id");
            AddForeignKey("dbo.Cedants", "CedingCompanyTypePickListDetailId", "dbo.PickListDetails", "Id");
            AddForeignKey("dbo.Benefits", "ValuationBenefitCodePickListDetailId", "dbo.PickListDetails", "Id");
            AddForeignKey("dbo.Treaties", "BusinessOriginPickListDetailId", "dbo.PickListDetails", "Id");
            AddForeignKey("dbo.Treaties", "LineOfBusinessPickListDetailId", "dbo.PickListDetails", "Id");
            AddForeignKey("dbo.RiDataBatches", "SoaDataBatchId", "dbo.SoaDataBatches", "Id");
            AddForeignKey("dbo.TreatyCodes", "TreatyStatusPickListDetailId", "dbo.PickListDetails", "Id");
            AddForeignKey("dbo.TreatyCodes", "TreatyTypePickListDetailId", "dbo.PickListDetails", "Id");
            AddForeignKey("dbo.Documents", "DepartmentId", "dbo.Departments", "Id");
            AddForeignKey("dbo.Remarks", "DepartmentId", "dbo.Departments", "Id");
            AddForeignKey("dbo.Remarks", "FollowUpUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.Documents", "RemarkId", "dbo.Remarks", "Id");
            AddForeignKey("dbo.Mfrs17Reportings", "CutOffId", "dbo.CutOff", "Id");
            AddForeignKey("dbo.Mfrs17ReportingDetailRiDatas", "RiDataWarehouseHistoryId", "dbo.RiDataWarehouseHistories", "Id");
            AddForeignKey("dbo.RateTables", "CedantId", "dbo.Cedants", "Id");
            AddForeignKey("dbo.RateTables", "GroupDiscountId", "dbo.GroupDiscounts", "Id");
            AddForeignKey("dbo.RateTables", "LargeDiscountId", "dbo.LargeDiscounts", "Id");
            AddForeignKey("dbo.RateTables", "RateId", "dbo.Rates", "Id");
            AddForeignKey("dbo.RateTables", "RiDiscountId", "dbo.RiDiscounts", "Id");
            DropColumn("dbo.Cedants", "Type");
            DropColumn("dbo.RiData", "ComputationStatus");
            DropColumn("dbo.RiDataBatches", "TotalComputationFailedStatus");
            DropColumn("dbo.RiDataBatches", "TotalPreValidationStatus");
            DropColumn("dbo.RateTables", "CedingOccupationCodePickListDetailId");
            DropColumn("dbo.RateTables", "InsuredGenderCodePickListDetailId");
            DropColumn("dbo.RateTables", "CedingTobaccoUsePickListDetailId");
        }

        public override void Down()
        {
            AddColumn("dbo.RateTables", "CedingTobaccoUsePickListDetailId", c => c.Int());
            AddColumn("dbo.RateTables", "InsuredGenderCodePickListDetailId", c => c.Int());
            AddColumn("dbo.RateTables", "CedingOccupationCodePickListDetailId", c => c.Int());
            AddColumn("dbo.RiDataBatches", "TotalPreValidationStatus", c => c.Int(nullable: false));
            AddColumn("dbo.RiDataBatches", "TotalComputationFailedStatus", c => c.Int(nullable: false));
            AddColumn("dbo.RiData", "ComputationStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Cedants", "Type", c => c.Int(nullable: false));
            DropForeignKey("dbo.UnderwritingRemarks", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.UnderwritingRemarks", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.UnderwritingRemarks", "ClaimRegisterId", "dbo.ClaimRegister");
            DropForeignKey("dbo.TreatyDiscountTableDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyDiscountTableDetails", "TreatyDiscountTableId", "dbo.TreatyDiscountTables");
            DropForeignKey("dbo.TreatyDiscountTableDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyDiscountTableDetails", "BenefitId", "dbo.Benefits");
            DropForeignKey("dbo.SoaDataRiDataSummaries", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.SoaDataRiDataSummaries", "SoaDataBatchId", "dbo.SoaDataBatches");
            DropForeignKey("dbo.SoaDataRiDataSummaries", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.SoaDataPostValidations", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.SoaDataPostValidations", "SoaDataBatchId", "dbo.SoaDataBatches");
            DropForeignKey("dbo.SoaDataPostValidations", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.SoaDataPostValidationDifferences", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.SoaDataPostValidationDifferences", "SoaDataBatchId", "dbo.SoaDataBatches");
            DropForeignKey("dbo.SoaDataPostValidationDifferences", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.SoaDataCompiledSummaries", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.SoaDataCompiledSummaries", "SoaDataBatchId", "dbo.SoaDataBatches");
            DropForeignKey("dbo.SoaDataCompiledSummaries", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.SoaDataBatchStatusFiles", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.SoaDataBatchStatusFiles", "StatusHistoryId", "dbo.StatusHistories");
            DropForeignKey("dbo.SoaDataBatchStatusFiles", "SoaDataBatchId", "dbo.SoaDataBatches");
            DropForeignKey("dbo.SoaDataBatchStatusFiles", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.SoaData", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.SoaData", "SoaDataFileId", "dbo.SoaDataFiles");
            DropForeignKey("dbo.SoaDataFiles", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.SoaDataFiles", "TreatyId", "dbo.Treaties");
            DropForeignKey("dbo.SoaDataFiles", "SoaDataBatchId", "dbo.SoaDataBatches");
            DropForeignKey("dbo.SoaDataFiles", "RawFileId", "dbo.RawFiles");
            DropForeignKey("dbo.SoaDataFiles", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.SoaData", "SoaDataBatchId", "dbo.SoaDataBatches");
            DropForeignKey("dbo.SoaData", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Salutations", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.Salutations", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RetroSummaries", "DirectRetroId", "dbo.DirectRetro");
            DropForeignKey("dbo.RetroSummaries", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RetroStatements", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RetroStatements", "RetroPartyId", "dbo.RetroParties");
            DropForeignKey("dbo.RetroStatements", "DirectRetroId", "dbo.DirectRetro");
            DropForeignKey("dbo.RetroStatements", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RetroRegisterHistories", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RetroRegisterHistories", "TreatyCodeId", "dbo.TreatyCodes");
            DropForeignKey("dbo.RetroRegisterHistories", "SoaDataBatchId", "dbo.SoaDataBatches");
            DropForeignKey("dbo.RetroRegisterHistories", "RetroRegisterBatchId", "dbo.RetroRegisterBatches");
            DropForeignKey("dbo.RetroRegisterHistories", "RetroRegisterId", "dbo.RetroRegister");
            DropForeignKey("dbo.RetroRegister", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RetroRegister", "TreatyCodeId", "dbo.TreatyCodes");
            DropForeignKey("dbo.RetroRegister", "SoaDataBatchId", "dbo.SoaDataBatches");
            DropForeignKey("dbo.RetroRegister", "RetroRegisterBatchId", "dbo.RetroRegisterBatches");
            DropForeignKey("dbo.RetroRegister", "RetroPartyId", "dbo.RetroParties");
            DropForeignKey("dbo.RetroRegister", "PreparedById", "dbo.Users");
            DropForeignKey("dbo.RetroRegister", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RetroRegister", "CedantId", "dbo.Cedants");
            DropForeignKey("dbo.RetroRegisterHistories", "RetroPartyId", "dbo.RetroParties");
            DropForeignKey("dbo.RetroRegisterHistories", "PreparedById", "dbo.Users");
            DropForeignKey("dbo.RetroRegisterHistories", "CutOffId", "dbo.CutOff");
            DropForeignKey("dbo.RetroRegisterHistories", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RetroRegisterHistories", "CedantId", "dbo.Cedants");
            DropForeignKey("dbo.RetroRegisterBatchFiles", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RetroRegisterBatchFiles", "RetroRegisterBatchId", "dbo.RetroRegisterBatches");
            DropForeignKey("dbo.RetroRegisterBatchFiles", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RetroRegisterBatchDirectRetros", "RetroRegisterBatchId", "dbo.RetroRegisterBatches");
            DropForeignKey("dbo.RetroRegisterBatches", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RetroRegisterBatches", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RetroRegisterBatchDirectRetros", "DirectRetroId", "dbo.DirectRetro");
            DropForeignKey("dbo.ReferralClaims", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.ReferralClaims", "RiDataWarehouseId", "dbo.RiDataWarehouse");
            DropForeignKey("dbo.ReferralClaims", "ReviewedById", "dbo.Users");
            DropForeignKey("dbo.ReferralClaims", "RetroReviewedById", "dbo.Users");
            DropForeignKey("dbo.ReferralClaims", "RetroReferralReasonId", "dbo.ClaimReasons");
            DropForeignKey("dbo.ReferralClaims", "ReferralRiDataId", "dbo.ReferralRiData");
            DropForeignKey("dbo.ReferralClaims", "ReferralReasonId", "dbo.ClaimReasons");
            DropForeignKey("dbo.ReferralClaims", "PersonInChargeId", "dbo.Users");
            DropForeignKey("dbo.ReferralClaims", "MlreReferralReasonId", "dbo.ClaimReasons");
            DropForeignKey("dbo.ReferralClaims", "DelayReasonId", "dbo.ClaimReasons");
            DropForeignKey("dbo.ReferralClaims", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ReferralClaims", "ClaimRegisterId", "dbo.ClaimRegister");
            DropForeignKey("dbo.ReferralClaims", "ClaimCategoryId", "dbo.ClaimCategories");
            DropForeignKey("dbo.ReferralClaims", "AssignedById", "dbo.Users");
            DropForeignKey("dbo.ReferralClaims", "AssessedById", "dbo.Users");
            DropForeignKey("dbo.RateTables", "RiDiscountId", "dbo.RiDiscounts");
            DropForeignKey("dbo.RiDiscounts", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RiDiscounts", "DiscountTableId", "dbo.DiscountTables");
            DropForeignKey("dbo.RiDiscounts", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RateTables", "RateId", "dbo.Rates");
            DropForeignKey("dbo.RateTables", "LargeDiscountId", "dbo.LargeDiscounts");
            DropForeignKey("dbo.RateTables", "GroupDiscountId", "dbo.GroupDiscounts");
            DropForeignKey("dbo.RateTables", "CedantId", "dbo.Cedants");
            DropForeignKey("dbo.RateDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RateDetails", "RateId", "dbo.Rates");
            DropForeignKey("dbo.Rates", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.Rates", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RateDetails", "InsuredGenderCodePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.RateDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RateDetails", "CedingTobaccoUsePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.RateDetails", "CedingOccupationCodePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.PublicHolidayDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PublicHolidayDetails", "PublicHolidayId", "dbo.PublicHolidays");
            DropForeignKey("dbo.PublicHolidays", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PublicHolidays", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.PublicHolidayDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.PremiumSpreadTableDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PremiumSpreadTableDetails", "PremiumSpreadTableId", "dbo.PremiumSpreadTables");
            DropForeignKey("dbo.PremiumSpreadTableDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.PremiumSpreadTableDetails", "BenefitId", "dbo.Benefits");
            DropForeignKey("dbo.Mfrs17ReportingDetailRiDatas", "RiDataWarehouseHistoryId", "dbo.RiDataWarehouseHistories");
            DropForeignKey("dbo.RiDataWarehouseHistories", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RiDataWarehouseHistories", "RiDataWarehouseId", "dbo.RiDataWarehouse");
            DropForeignKey("dbo.RiDataWarehouseHistories", "CutOffId", "dbo.CutOff");
            DropForeignKey("dbo.RiDataWarehouseHistories", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Mfrs17Reportings", "CutOffId", "dbo.CutOff");
            DropForeignKey("dbo.LargeDiscounts", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.LargeDiscounts", "DiscountTableId", "dbo.DiscountTables");
            DropForeignKey("dbo.LargeDiscounts", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ItemCodeMappings", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.ItemCodeMappings", "TreatyTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.ItemCodeMappings", "StandardSoaDataOutputId", "dbo.StandardSoaDataOutputs");
            DropForeignKey("dbo.ItemCodeMappings", "ItemCodeId", "dbo.ItemCodes");
            DropForeignKey("dbo.ItemCodes", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.ItemCodes", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ItemCodeMappings", "InvoiceFieldPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.ItemCodeMappings", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.InvoiceRegisterValuations", "ValuationBenefitCodeId", "dbo.Benefits");
            DropForeignKey("dbo.InvoiceRegisterValuations", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.InvoiceRegisterValuations", "InvoiceRegisterId", "dbo.InvoiceRegister");
            DropForeignKey("dbo.InvoiceRegisterValuations", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.InvoiceRegisterHistories", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.InvoiceRegisterHistories", "TreatyCodeId", "dbo.TreatyCodes");
            DropForeignKey("dbo.InvoiceRegisterHistories", "SoaDataBatchId", "dbo.SoaDataBatches");
            DropForeignKey("dbo.InvoiceRegisterHistories", "PreparedById", "dbo.Users");
            DropForeignKey("dbo.InvoiceRegisterHistories", "InvoiceRegisterBatchId", "dbo.InvoiceRegisterBatches");
            DropForeignKey("dbo.InvoiceRegisterHistories", "InvoiceRegisterId", "dbo.InvoiceRegister");
            DropForeignKey("dbo.InvoiceRegister", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.InvoiceRegister", "TreatyCodeId", "dbo.TreatyCodes");
            DropForeignKey("dbo.InvoiceRegister", "SoaDataBatchId", "dbo.SoaDataBatches");
            DropForeignKey("dbo.InvoiceRegister", "PreparedById", "dbo.Users");
            DropForeignKey("dbo.InvoiceRegister", "InvoiceRegisterBatchId", "dbo.InvoiceRegisterBatches");
            DropForeignKey("dbo.InvoiceRegister", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.InvoiceRegister", "CedantId", "dbo.Cedants");
            DropForeignKey("dbo.InvoiceRegisterHistories", "CutOffId", "dbo.CutOff");
            DropForeignKey("dbo.InvoiceRegisterHistories", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.InvoiceRegisterHistories", "CedantId", "dbo.Cedants");
            DropForeignKey("dbo.InvoiceRegisterBatchSoaDatas", "SoaDataBatchId", "dbo.SoaDataBatches");
            DropForeignKey("dbo.InvoiceRegisterBatchSoaDatas", "InvoiceRegisterBatchId", "dbo.InvoiceRegisterBatches");
            DropForeignKey("dbo.InvoiceRegisterBatchRemarkDocuments", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.InvoiceRegisterBatchRemarkDocuments", "InvoiceRegisterBatchRemarkId", "dbo.InvoiceRegisterBatchRemarks");
            DropForeignKey("dbo.InvoiceRegisterBatchRemarks", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.InvoiceRegisterBatchRemarks", "InvoiceRegisterBatchId", "dbo.InvoiceRegisterBatches");
            DropForeignKey("dbo.InvoiceRegisterBatchRemarks", "FollowUpUserId", "dbo.Users");
            DropForeignKey("dbo.InvoiceRegisterBatchRemarks", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.InvoiceRegisterBatchRemarkDocuments", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.InvoiceRegisterBatchFiles", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.InvoiceRegisterBatchFiles", "InvoiceRegisterBatchId", "dbo.InvoiceRegisterBatches");
            DropForeignKey("dbo.InvoiceRegisterBatchFiles", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.InvoiceRegisterBatches", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.InvoiceRegisterBatches", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.GroupDiscounts", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.GroupDiscounts", "DiscountTableId", "dbo.DiscountTables");
            DropForeignKey("dbo.GroupDiscounts", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.FinanceProvisioningTransactions", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.FinanceProvisioningTransactions", "FinanceProvisioningId", "dbo.FinanceProvisionings");
            DropForeignKey("dbo.FinanceProvisioningTransactions", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.FinanceProvisioningTransactions", "ClaimRegisterId", "dbo.ClaimRegister");
            DropForeignKey("dbo.FacMasterListingDetails", "FacMasterListingId", "dbo.FacMasterListings");
            DropForeignKey("dbo.FacMasterListings", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.FacMasterListings", "InsuredGenderCodePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.FacMasterListings", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.FacMasterListings", "CedantId", "dbo.Cedants");
            DropForeignKey("dbo.FacMasterListings", "BenefitId", "dbo.Benefits");
            DropForeignKey("dbo.FacMasterListingDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.EventClaimCodeMappingDetails", "EventClaimCodeMappingId", "dbo.EventClaimCodeMappings");
            DropForeignKey("dbo.EventClaimCodeMappings", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.EventClaimCodeMappings", "EventCodeId", "dbo.EventCodes");
            DropForeignKey("dbo.EventClaimCodeMappings", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.EventClaimCodeMappings", "CedantId", "dbo.Cedants");
            DropForeignKey("dbo.EventClaimCodeMappingDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Documents", "RemarkId", "dbo.Remarks");
            DropForeignKey("dbo.Remarks", "FollowUpUserId", "dbo.Users");
            DropForeignKey("dbo.Remarks", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Documents", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.DiscountTables", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.DiscountTables", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.DiscountTables", "CedantId", "dbo.Cedants");
            DropForeignKey("dbo.DirectRetroStatusFiles", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.DirectRetroStatusFiles", "StatusHistoryId", "dbo.StatusHistories");
            DropForeignKey("dbo.DirectRetroStatusFiles", "DirectRetroId", "dbo.DirectRetro");
            DropForeignKey("dbo.DirectRetroStatusFiles", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.DirectRetroProvisioningTransactions", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.DirectRetroProvisioningTransactions", "FinanceProvisioningId", "dbo.FinanceProvisionings");
            DropForeignKey("dbo.FinanceProvisionings", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.FinanceProvisionings", "CutOffId", "dbo.CutOff");
            DropForeignKey("dbo.FinanceProvisionings", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.DirectRetroProvisioningTransactions", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.DirectRetroProvisioningTransactions", "ClaimRegisterId", "dbo.ClaimRegister");
            DropForeignKey("dbo.DirectRetroConfigurationDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.DirectRetroConfigurationDetails", "TreatyDiscountTableId", "dbo.TreatyDiscountTables");
            DropForeignKey("dbo.TreatyDiscountTables", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyDiscountTables", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.DirectRetroConfigurationDetails", "RetroPartyId", "dbo.RetroParties");
            DropForeignKey("dbo.RetroParties", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RetroParties", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RetroParties", "CountryCodePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.DirectRetroConfigurationDetails", "PremiumSpreadTableId", "dbo.PremiumSpreadTables");
            DropForeignKey("dbo.PremiumSpreadTables", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PremiumSpreadTables", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.DirectRetroConfigurationDetails", "DirectRetroConfigurationId", "dbo.DirectRetroConfigurations");
            DropForeignKey("dbo.DirectRetroConfigurations", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.DirectRetroConfigurations", "TreatyCodeId", "dbo.TreatyCodes");
            DropForeignKey("dbo.DirectRetroConfigurationMappings", "DirectRetroConfigurationId", "dbo.DirectRetroConfigurations");
            DropForeignKey("dbo.DirectRetroConfigurationMappings", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.DirectRetroConfigurations", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.DirectRetroConfigurationDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.DirectRetro", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.DirectRetro", "TreatyCodeId", "dbo.TreatyCodes");
            DropForeignKey("dbo.TreatyCodes", "TreatyTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyCodes", "TreatyStatusPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.DirectRetro", "SoaDataBatchId", "dbo.SoaDataBatches");
            DropForeignKey("dbo.DirectRetro", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.DirectRetro", "CedantId", "dbo.Cedants");
            DropForeignKey("dbo.ClaimRegisterHistories", "UpdatedOnBehalfById", "dbo.Users");
            DropForeignKey("dbo.ClaimRegisterHistories", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimRegisterHistories", "SignOffById", "dbo.Users");
            DropForeignKey("dbo.ClaimRegisterHistories", "RiDataWarehouseId", "dbo.RiDataWarehouse");
            DropForeignKey("dbo.ClaimRegisterHistories", "PicDaaId", "dbo.Users");
            DropForeignKey("dbo.ClaimRegisterHistories", "PicClaimId", "dbo.Users");
            DropForeignKey("dbo.ClaimRegisterHistories", "OriginalClaimRegisterId", "dbo.ClaimRegister");
            DropForeignKey("dbo.ClaimRegisterHistories", "CutOffId", "dbo.CutOff");
            DropForeignKey("dbo.CutOff", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.CutOff", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimRegisterHistories", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimRegisterHistories", "ClaimRegisterId", "dbo.ClaimRegister");
            DropForeignKey("dbo.ClaimRegisterHistories", "ClaimReasonId", "dbo.ClaimReasons");
            DropForeignKey("dbo.ClaimRegisterHistories", "ClaimDataConfigId", "dbo.ClaimDataConfigs");
            DropForeignKey("dbo.ClaimRegisterHistories", "ClaimDataBatchId", "dbo.ClaimDataBatches");
            DropForeignKey("dbo.ClaimRegisterHistories", "ClaimDataId", "dbo.ClaimData");
            DropForeignKey("dbo.ClaimRegisterHistories", "ClaimCommitteeUser2Id", "dbo.Users");
            DropForeignKey("dbo.ClaimRegisterHistories", "ClaimCommitteeUser1Id", "dbo.Users");
            DropForeignKey("dbo.ClaimRegisterHistories", "ClaimAssessorId", "dbo.Users");
            DropForeignKey("dbo.ClaimRegisterHistories", "CeoClaimReasonId", "dbo.ClaimReasons");
            DropForeignKey("dbo.ClaimRegister", "UpdatedOnBehalfById", "dbo.Users");
            DropForeignKey("dbo.ClaimRegister", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimRegister", "SignOffById", "dbo.Users");
            DropForeignKey("dbo.ClaimRegister", "RiDataWarehouseId", "dbo.RiDataWarehouse");
            DropForeignKey("dbo.RiDataWarehouse", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RiDataWarehouse", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimRegister", "ReferralRiDataId", "dbo.ReferralRiData");
            DropForeignKey("dbo.ReferralRiData", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.ReferralRiData", "ReferralRiDataFileId", "dbo.ReferralRiDataFiles");
            DropForeignKey("dbo.ReferralRiDataFiles", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.ReferralRiDataFiles", "RawFileId", "dbo.RawFiles");
            DropForeignKey("dbo.ReferralRiDataFiles", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ReferralRiData", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimRegister", "PicDaaId", "dbo.Users");
            DropForeignKey("dbo.ClaimRegister", "PicClaimId", "dbo.Users");
            DropForeignKey("dbo.ClaimRegister", "OriginalClaimRegisterId", "dbo.ClaimRegister");
            DropForeignKey("dbo.ClaimRegister", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimRegister", "ClaimReasonId", "dbo.ClaimReasons");
            DropForeignKey("dbo.ClaimRegister", "ClaimDataConfigId", "dbo.ClaimDataConfigs");
            DropForeignKey("dbo.ClaimRegister", "ClaimDataBatchId", "dbo.ClaimDataBatches");
            DropForeignKey("dbo.ClaimRegister", "ClaimDataId", "dbo.ClaimData");
            DropForeignKey("dbo.ClaimRegister", "ClaimCommitteeUser2Id", "dbo.Users");
            DropForeignKey("dbo.ClaimRegister", "ClaimCommitteeUser1Id", "dbo.Users");
            DropForeignKey("dbo.ClaimRegister", "ClaimAssessorId", "dbo.Users");
            DropForeignKey("dbo.ClaimRegister", "CeoClaimReasonId", "dbo.ClaimReasons");
            DropForeignKey("dbo.ClaimReasons", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimReasons", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimDataValidations", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimDataValidations", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimDataValidations", "ClaimDataConfigId", "dbo.ClaimDataConfigs");
            DropForeignKey("dbo.ClaimDataMappingDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimDataMappingDetails", "PickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.ClaimDataMappingDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimDataMappingDetails", "ClaimDataMappingId", "dbo.ClaimDataMappings");
            DropForeignKey("dbo.ClaimDataMappings", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimDataMappings", "StandardClaimDataOutputId", "dbo.StandardClaimDataOutputs");
            DropForeignKey("dbo.ClaimDataMappings", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimDataMappings", "ClaimDataConfigId", "dbo.ClaimDataConfigs");
            DropForeignKey("dbo.ClaimDataComputations", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimDataComputations", "StandardClaimDataOutputId", "dbo.StandardClaimDataOutputs");
            DropForeignKey("dbo.ClaimDataComputations", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimDataComputations", "ClaimDataConfigId", "dbo.ClaimDataConfigs");
            DropForeignKey("dbo.ClaimDataBatchStatusFiles", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimDataBatchStatusFiles", "StatusHistoryId", "dbo.StatusHistories");
            DropForeignKey("dbo.ClaimDataBatchStatusFiles", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimDataBatchStatusFiles", "ClaimDataBatchId", "dbo.ClaimDataBatches");
            DropForeignKey("dbo.ClaimData", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimData", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimData", "ClaimDataFileId", "dbo.ClaimDataFiles");
            DropForeignKey("dbo.ClaimDataFiles", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimDataFiles", "TreatyId", "dbo.Treaties");
            DropForeignKey("dbo.ClaimDataFiles", "RawFileId", "dbo.RawFiles");
            DropForeignKey("dbo.ClaimDataFiles", "CurrencyCodeId", "dbo.PickListDetails");
            DropForeignKey("dbo.ClaimDataFiles", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimDataFiles", "ClaimDataConfigId", "dbo.ClaimDataConfigs");
            DropForeignKey("dbo.ClaimDataFiles", "ClaimDataBatchId", "dbo.ClaimDataBatches");
            DropForeignKey("dbo.ClaimData", "ClaimDataBatchId", "dbo.ClaimDataBatches");
            DropForeignKey("dbo.ClaimDataBatches", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimDataBatches", "TreatyId", "dbo.Treaties");
            DropForeignKey("dbo.ClaimDataBatches", "SoaDataBatchId", "dbo.SoaDataBatches");
            DropForeignKey("dbo.SoaDataBatches", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.SoaDataBatches", "TreatyId", "dbo.Treaties");
            DropForeignKey("dbo.SoaDataBatches", "RiDataBatchId", "dbo.RiDataBatches");
            DropForeignKey("dbo.RiDataBatches", "SoaDataBatchId", "dbo.SoaDataBatches");
            DropForeignKey("dbo.SoaDataBatches", "CurrencyCodePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.SoaDataBatches", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.SoaDataBatches", "ClaimDataBatchId", "dbo.ClaimDataBatches");
            DropForeignKey("dbo.SoaDataBatches", "CedantId", "dbo.Cedants");
            DropForeignKey("dbo.ClaimDataBatches", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimDataBatches", "ClaimDataConfigId", "dbo.ClaimDataConfigs");
            DropForeignKey("dbo.ClaimDataConfigs", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimDataConfigs", "TreatyId", "dbo.Treaties");
            DropForeignKey("dbo.Treaties", "LineOfBusinessPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.Treaties", "BusinessOriginPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.ClaimDataConfigs", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimDataConfigs", "CedantId", "dbo.Cedants");
            DropForeignKey("dbo.ClaimDataBatches", "CedantId", "dbo.Cedants");
            DropForeignKey("dbo.ClaimCodeMappingDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimCodeMappingDetails", "ClaimCodeMappingId", "dbo.ClaimCodeMappings");
            DropForeignKey("dbo.ClaimCodeMappings", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimCodeMappings", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimCodeMappings", "ClaimCodeId", "dbo.ClaimCodes");
            DropForeignKey("dbo.ClaimChecklistDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimChecklistDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimChecklistDetails", "ClaimChecklistId", "dbo.ClaimChecklists");
            DropForeignKey("dbo.ClaimChecklists", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimChecklists", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimChecklists", "ClaimCodeId", "dbo.ClaimCodes");
            DropForeignKey("dbo.ClaimCategories", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimCategories", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimAuthorityLimitMLRe", "UserId", "dbo.Users");
            DropForeignKey("dbo.ClaimAuthorityLimitMLRe", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimAuthorityLimitMLRe", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.ClaimAuthorityLimitMLRe", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimAuthorityLimitMLReDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimAuthorityLimitMLReDetails", "ClaimCodeId", "dbo.ClaimCodes");
            DropForeignKey("dbo.ClaimAuthorityLimitMLReDetails", "ClaimAuthorityLimitMLReId", "dbo.ClaimAuthorityLimitMLRe");
            DropForeignKey("dbo.ClaimAuthorityLimitCedantDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimAuthorityLimitCedantDetails", "ClaimCodeId", "dbo.ClaimCodes");
            DropForeignKey("dbo.ClaimAuthorityLimitCedantDetails", "ClaimAuthorityLimitCedantId", "dbo.ClaimAuthorityLimitCedants");
            DropForeignKey("dbo.ClaimAuthorityLimitCedants", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimAuthorityLimitCedants", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimAuthorityLimitCedants", "CedantId", "dbo.Cedants");
            DropForeignKey("dbo.CedantWorkgroupUsers", "UserId", "dbo.Users");
            DropForeignKey("dbo.CedantWorkgroupUsers", "CedantWorkgroupId", "dbo.CedantWorkgroups");
            DropForeignKey("dbo.CedantWorkgroupCedants", "CedantWorkgroupId", "dbo.CedantWorkgroups");
            DropForeignKey("dbo.CedantWorkgroups", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.CedantWorkgroups", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.CedantWorkgroupCedants", "CedantId", "dbo.Cedants");
            DropForeignKey("dbo.BenefitDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.BenefitDetails", "EventCodeId", "dbo.EventCodes");
            DropForeignKey("dbo.EventCodes", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.EventCodes", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.BenefitDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.BenefitDetails", "ClaimCodeId", "dbo.ClaimCodes");
            DropForeignKey("dbo.BenefitDetails", "BenefitId", "dbo.Benefits");
            DropForeignKey("dbo.Benefits", "ValuationBenefitCodePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.AuthorizationLimits", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.AuthorizationLimits", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.AuthorizationLimits", "AccessGroupId", "dbo.AccessGroups");
            DropForeignKey("dbo.AnnuityFactorDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.AnnuityFactorDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.AnnuityFactorDetails", "AnnuityFactorId", "dbo.AnnuityFactors");
            DropForeignKey("dbo.AnnuityFactors", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.AnnuityFactors", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.AnnuityFactors", "CedantId", "dbo.Cedants");
            DropForeignKey("dbo.Cedants", "CedingCompanyTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.AnnuityFactorMappings", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.AnnuityFactorMappings", "AnnuityFactorId", "dbo.AnnuityFactors");
            DropForeignKey("dbo.AccountCodeMappings", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.AccountCodeMappings", "TreatyTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.AccountCodeMappings", "TransactionTypeCodePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.PickLists", "StandardSoaDataOutputId", "dbo.StandardSoaDataOutputs");
            DropForeignKey("dbo.StandardSoaDataOutputs", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.StandardSoaDataOutputs", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.PickLists", "StandardClaimDataOutputId", "dbo.StandardClaimDataOutputs");
            DropForeignKey("dbo.StandardClaimDataOutputs", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.StandardClaimDataOutputs", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.AccountCodeMappings", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.AccountCodeMappings", "ClaimCodeId", "dbo.ClaimCodes");
            DropForeignKey("dbo.ClaimCodes", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.ClaimCodes", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.AccountCodeMappings", "AccountCodeId", "dbo.AccountCodes");
            DropForeignKey("dbo.AccountCodes", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.AccountCodes", "CreatedById", "dbo.Users");
            DropIndex("dbo.UnderwritingRemarks", new[] { "UpdatedById" });
            DropIndex("dbo.UnderwritingRemarks", new[] { "CreatedById" });
            DropIndex("dbo.UnderwritingRemarks", new[] { "ClaimRegisterId" });
            DropIndex("dbo.TreatyDiscountTableDetails", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyDiscountTableDetails", new[] { "CreatedById" });
            DropIndex("dbo.TreatyDiscountTableDetails", new[] { "Discount" });
            DropIndex("dbo.TreatyDiscountTableDetails", new[] { "AgeTo" });
            DropIndex("dbo.TreatyDiscountTableDetails", new[] { "AgeFrom" });
            DropIndex("dbo.TreatyDiscountTableDetails", new[] { "BenefitId" });
            DropIndex("dbo.TreatyDiscountTableDetails", new[] { "TreatyDiscountTableId" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "WakalahFee" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "ServiceFee" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "RiShareCap" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "RiShare" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "RetentionCap" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "RetentionShare" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "AblAmount" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "MaxAar" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "MinAar" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "ApLoading" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "MaxUwRating" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "MaxIssueAge" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "MinIssueAge" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "MaxExpiryAge" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "ProfitComm" });
            DropIndex("dbo.SoaDataRiDataSummaries", new[] { "UpdatedById" });
            DropIndex("dbo.SoaDataRiDataSummaries", new[] { "CreatedById" });
            DropIndex("dbo.SoaDataRiDataSummaries", new[] { "CurrencyCode" });
            DropIndex("dbo.SoaDataRiDataSummaries", new[] { "RiskMonth" });
            DropIndex("dbo.SoaDataRiDataSummaries", new[] { "RiskQuarter" });
            DropIndex("dbo.SoaDataRiDataSummaries", new[] { "SoaQuarter" });
            DropIndex("dbo.SoaDataRiDataSummaries", new[] { "TreatyCode" });
            DropIndex("dbo.SoaDataRiDataSummaries", new[] { "BusinessCode" });
            DropIndex("dbo.SoaDataRiDataSummaries", new[] { "SoaDataBatchId" });
            DropIndex("dbo.SoaDataPostValidations", new[] { "UpdatedById" });
            DropIndex("dbo.SoaDataPostValidations", new[] { "CreatedById" });
            DropIndex("dbo.SoaDataPostValidations", new[] { "CurrencyCode" });
            DropIndex("dbo.SoaDataPostValidations", new[] { "RiskMonth" });
            DropIndex("dbo.SoaDataPostValidations", new[] { "RiskQuarter" });
            DropIndex("dbo.SoaDataPostValidations", new[] { "SoaQuarter" });
            DropIndex("dbo.SoaDataPostValidations", new[] { "TreatyCode" });
            DropIndex("dbo.SoaDataPostValidations", new[] { "BusinessCode" });
            DropIndex("dbo.SoaDataPostValidations", new[] { "Type" });
            DropIndex("dbo.SoaDataPostValidations", new[] { "SoaDataBatchId" });
            DropIndex("dbo.SoaDataPostValidationDifferences", new[] { "UpdatedById" });
            DropIndex("dbo.SoaDataPostValidationDifferences", new[] { "CreatedById" });
            DropIndex("dbo.SoaDataPostValidationDifferences", new[] { "CurrencyCode" });
            DropIndex("dbo.SoaDataPostValidationDifferences", new[] { "RiskMonth" });
            DropIndex("dbo.SoaDataPostValidationDifferences", new[] { "RiskQuarter" });
            DropIndex("dbo.SoaDataPostValidationDifferences", new[] { "SoaQuarter" });
            DropIndex("dbo.SoaDataPostValidationDifferences", new[] { "TreatyCode" });
            DropIndex("dbo.SoaDataPostValidationDifferences", new[] { "BusinessCode" });
            DropIndex("dbo.SoaDataPostValidationDifferences", new[] { "Type" });
            DropIndex("dbo.SoaDataPostValidationDifferences", new[] { "SoaDataBatchId" });
            DropIndex("dbo.SoaDataCompiledSummaries", new[] { "UpdatedById" });
            DropIndex("dbo.SoaDataCompiledSummaries", new[] { "CreatedById" });
            DropIndex("dbo.SoaDataCompiledSummaries", new[] { "InvoiceNumber2" });
            DropIndex("dbo.SoaDataCompiledSummaries", new[] { "InvoiceNumber1" });
            DropIndex("dbo.SoaDataCompiledSummaries", new[] { "RiskQuarter" });
            DropIndex("dbo.SoaDataCompiledSummaries", new[] { "SoaQuarter" });
            DropIndex("dbo.SoaDataCompiledSummaries", new[] { "TreatyCode" });
            DropIndex("dbo.SoaDataCompiledSummaries", new[] { "BusinessCode" });
            DropIndex("dbo.SoaDataCompiledSummaries", new[] { "SoaDataBatchId" });
            DropIndex("dbo.SoaDataBatchStatusFiles", new[] { "UpdatedById" });
            DropIndex("dbo.SoaDataBatchStatusFiles", new[] { "CreatedById" });
            DropIndex("dbo.SoaDataBatchStatusFiles", new[] { "StatusHistoryId" });
            DropIndex("dbo.SoaDataBatchStatusFiles", new[] { "SoaDataBatchId" });
            DropIndex("dbo.SoaDataFiles", new[] { "UpdatedById" });
            DropIndex("dbo.SoaDataFiles", new[] { "CreatedById" });
            DropIndex("dbo.SoaDataFiles", new[] { "Status" });
            DropIndex("dbo.SoaDataFiles", new[] { "TreatyId" });
            DropIndex("dbo.SoaDataFiles", new[] { "RawFileId" });
            DropIndex("dbo.SoaDataFiles", new[] { "SoaDataBatchId" });
            DropIndex("dbo.SoaData", new[] { "UpdatedById" });
            DropIndex("dbo.SoaData", new[] { "CreatedById" });
            DropIndex("dbo.SoaData", new[] { "RiskQuarter" });
            DropIndex("dbo.SoaData", new[] { "SoaQuarter" });
            DropIndex("dbo.SoaData", new[] { "TreatyCode" });
            DropIndex("dbo.SoaData", new[] { "BusinessCode" });
            DropIndex("dbo.SoaData", new[] { "MappingStatus" });
            DropIndex("dbo.SoaData", new[] { "SoaDataFileId" });
            DropIndex("dbo.SoaData", new[] { "SoaDataBatchId" });
            DropIndex("dbo.Salutations", new[] { "UpdatedById" });
            DropIndex("dbo.Salutations", new[] { "CreatedById" });
            DropIndex("dbo.RiDataPreValidations", new[] { "Step" });
            DropIndex("dbo.RiDataComputations", new[] { "StandardOutputId" });
            DropIndex("dbo.RiDataComputations", new[] { "Step" });
            DropIndex("dbo.RiData", new[] { "RetroNetPremium3" });
            DropIndex("dbo.RiData", new[] { "RetroNetPremium2" });
            DropIndex("dbo.RiData", new[] { "RetroNetPremium1" });
            DropIndex("dbo.RiData", new[] { "RetroDiscount3" });
            DropIndex("dbo.RiData", new[] { "RetroDiscount2" });
            DropIndex("dbo.RiData", new[] { "RetroDiscount1" });
            DropIndex("dbo.RiData", new[] { "RetroReinsurancePremium3" });
            DropIndex("dbo.RiData", new[] { "RetroReinsurancePremium2" });
            DropIndex("dbo.RiData", new[] { "RetroReinsurancePremium1" });
            DropIndex("dbo.RiData", new[] { "RetroAar3" });
            DropIndex("dbo.RiData", new[] { "RetroAar2" });
            DropIndex("dbo.RiData", new[] { "RetroAar1" });
            DropIndex("dbo.RiData", new[] { "RetroShare3" });
            DropIndex("dbo.RiData", new[] { "RetroShare2" });
            DropIndex("dbo.RiData", new[] { "RetroShare1" });
            DropIndex("dbo.RiData", new[] { "RetroParty3" });
            DropIndex("dbo.RiData", new[] { "RetroParty2" });
            DropIndex("dbo.RiData", new[] { "RetroParty1" });
            DropIndex("dbo.RiData", new[] { "ProcessWarehouseStatus" });
            DropIndex("dbo.RiData", new[] { "PostValidationStatus" });
            DropIndex("dbo.RiData", new[] { "PostComputationStatus" });
            DropIndex("dbo.RiData", new[] { "PreComputation2Status" });
            DropIndex("dbo.RiData", new[] { "PreComputation1Status" });
            DropIndex("dbo.RiData", new[] { "RiDataFileId" });
            DropIndex("dbo.RetroSummaries", new[] { "CreatedById" });
            DropIndex("dbo.RetroSummaries", new[] { "RetroClaims3" });
            DropIndex("dbo.RetroSummaries", new[] { "RetroClaims2" });
            DropIndex("dbo.RetroSummaries", new[] { "RetroClaims1" });
            DropIndex("dbo.RetroSummaries", new[] { "RetroDiscount3" });
            DropIndex("dbo.RetroSummaries", new[] { "RetroDiscount2" });
            DropIndex("dbo.RetroSummaries", new[] { "RetroDiscount1" });
            DropIndex("dbo.RetroSummaries", new[] { "RetroRiPremium3" });
            DropIndex("dbo.RetroSummaries", new[] { "RetroRiPremium2" });
            DropIndex("dbo.RetroSummaries", new[] { "RetroRiPremium1" });
            DropIndex("dbo.RetroSummaries", new[] { "RetroShare3" });
            DropIndex("dbo.RetroSummaries", new[] { "RetroShare2" });
            DropIndex("dbo.RetroSummaries", new[] { "RetroShare1" });
            DropIndex("dbo.RetroSummaries", new[] { "RetroParty3" });
            DropIndex("dbo.RetroSummaries", new[] { "RetroParty2" });
            DropIndex("dbo.RetroSummaries", new[] { "RetroParty1" });
            DropIndex("dbo.RetroSummaries", new[] { "TotalClaims" });
            DropIndex("dbo.RetroSummaries", new[] { "NoOfClaims" });
            DropIndex("dbo.RetroSummaries", new[] { "TotalDiscount" });
            DropIndex("dbo.RetroSummaries", new[] { "TotalRiPremium" });
            DropIndex("dbo.RetroSummaries", new[] { "TotalSar" });
            DropIndex("dbo.RetroSummaries", new[] { "NoOfPolicy" });
            DropIndex("dbo.RetroSummaries", new[] { "Type" });
            DropIndex("dbo.RetroSummaries", new[] { "Year" });
            DropIndex("dbo.RetroSummaries", new[] { "Month" });
            DropIndex("dbo.RetroSummaries", new[] { "DirectRetroId" });
            DropIndex("dbo.RetroStatements", new[] { "UpdatedById" });
            DropIndex("dbo.RetroStatements", new[] { "CreatedById" });
            DropIndex("dbo.RetroStatements", new[] { "Status" });
            DropIndex("dbo.RetroStatements", new[] { "RetroPartyId" });
            DropIndex("dbo.RetroStatements", new[] { "DirectRetroId" });
            DropIndex("dbo.RetroRegister", new[] { "SoaDataBatchId" });
            DropIndex("dbo.RetroRegister", new[] { "UpdatedById" });
            DropIndex("dbo.RetroRegister", new[] { "CreatedById" });
            DropIndex("dbo.RetroRegister", new[] { "PreparedById" });
            DropIndex("dbo.RetroRegister", new[] { "TreatyCodeId" });
            DropIndex("dbo.RetroRegister", new[] { "CedantId" });
            DropIndex("dbo.RetroRegister", new[] { "RetroPartyId" });
            DropIndex("dbo.RetroRegister", new[] { "RetroStatementNo" });
            DropIndex("dbo.RetroRegister", new[] { "RetroStatementType" });
            DropIndex("dbo.RetroRegister", new[] { "RetroRegisterBatchId" });
            DropIndex("dbo.RetroRegisterHistories", new[] { "SoaDataBatchId" });
            DropIndex("dbo.RetroRegisterHistories", new[] { "UpdatedById" });
            DropIndex("dbo.RetroRegisterHistories", new[] { "CreatedById" });
            DropIndex("dbo.RetroRegisterHistories", new[] { "PreparedById" });
            DropIndex("dbo.RetroRegisterHistories", new[] { "TreatyCodeId" });
            DropIndex("dbo.RetroRegisterHistories", new[] { "CedantId" });
            DropIndex("dbo.RetroRegisterHistories", new[] { "RetroPartyId" });
            DropIndex("dbo.RetroRegisterHistories", new[] { "RetroStatementNo" });
            DropIndex("dbo.RetroRegisterHistories", new[] { "RetroStatementType" });
            DropIndex("dbo.RetroRegisterHistories", new[] { "RetroRegisterBatchId" });
            DropIndex("dbo.RetroRegisterHistories", new[] { "RetroRegisterId" });
            DropIndex("dbo.RetroRegisterHistories", new[] { "CutOffId" });
            DropIndex("dbo.RetroRegisterBatchFiles", new[] { "UpdatedById" });
            DropIndex("dbo.RetroRegisterBatchFiles", new[] { "CreatedById" });
            DropIndex("dbo.RetroRegisterBatchFiles", new[] { "Status" });
            DropIndex("dbo.RetroRegisterBatchFiles", new[] { "HashFileName" });
            DropIndex("dbo.RetroRegisterBatchFiles", new[] { "FileName" });
            DropIndex("dbo.RetroRegisterBatchFiles", new[] { "RetroRegisterBatchId" });
            DropIndex("dbo.RetroRegisterBatches", new[] { "UpdatedById" });
            DropIndex("dbo.RetroRegisterBatches", new[] { "CreatedById" });
            DropIndex("dbo.RetroRegisterBatches", new[] { "Status" });
            DropIndex("dbo.RetroRegisterBatches", new[] { "BatchNo" });
            DropIndex("dbo.RetroRegisterBatchDirectRetros", new[] { "DirectRetroId" });
            DropIndex("dbo.RetroRegisterBatchDirectRetros", new[] { "RetroRegisterBatchId" });
            DropIndex("dbo.ReferralClaims", new[] { "UpdatedById" });
            DropIndex("dbo.ReferralClaims", new[] { "CreatedById" });
            DropIndex("dbo.ReferralClaims", new[] { "PersonInChargeId" });
            DropIndex("dbo.ReferralClaims", new[] { "TreatyShare" });
            DropIndex("dbo.ReferralClaims", new[] { "TreatyType" });
            DropIndex("dbo.ReferralClaims", new[] { "TreatyCode" });
            DropIndex("dbo.ReferralClaims", new[] { "AssignedAt" });
            DropIndex("dbo.ReferralClaims", new[] { "AssignedById" });
            DropIndex("dbo.ReferralClaims", new[] { "ReviewedAt" });
            DropIndex("dbo.ReferralClaims", new[] { "ReviewedById" });
            DropIndex("dbo.ReferralClaims", new[] { "AssessedAt" });
            DropIndex("dbo.ReferralClaims", new[] { "AssessedById" });
            DropIndex("dbo.ReferralClaims", new[] { "CompletedCaseStudyMaterialAt" });
            DropIndex("dbo.ReferralClaims", new[] { "IsClaimCaseStudy" });
            DropIndex("dbo.ReferralClaims", new[] { "IsValueAddedService" });
            DropIndex("dbo.ReferralClaims", new[] { "RetroReviewedAt" });
            DropIndex("dbo.ReferralClaims", new[] { "RetroReviewedById" });
            DropIndex("dbo.ReferralClaims", new[] { "MlreReferralReasonId" });
            DropIndex("dbo.ReferralClaims", new[] { "RetroReferralReasonId" });
            DropIndex("dbo.ReferralClaims", new[] { "RetrocessionaireShare" });
            DropIndex("dbo.ReferralClaims", new[] { "IsRetro" });
            DropIndex("dbo.ReferralClaims", new[] { "DelayReasonId" });
            DropIndex("dbo.ReferralClaims", new[] { "TurnAroundTime" });
            DropIndex("dbo.ReferralClaims", new[] { "RespondedAt" });
            DropIndex("dbo.ReferralClaims", new[] { "ReceivedAt" });
            DropIndex("dbo.ReferralClaims", new[] { "IsRgalRetakaful" });
            DropIndex("dbo.ReferralClaims", new[] { "ClaimCategoryId" });
            DropIndex("dbo.ReferralClaims", new[] { "ReinsBasisCode" });
            DropIndex("dbo.ReferralClaims", new[] { "ClaimRecoveryAmount" });
            DropIndex("dbo.ReferralClaims", new[] { "MlreBenefitCode" });
            DropIndex("dbo.ReferralClaims", new[] { "RiskQuarter" });
            DropIndex("dbo.ReferralClaims", new[] { "DateOfEvent" });
            DropIndex("dbo.ReferralClaims", new[] { "SumReinsured" });
            DropIndex("dbo.ReferralClaims", new[] { "SumInsured" });
            DropIndex("dbo.ReferralClaims", new[] { "CedingPlanCode" });
            DropIndex("dbo.ReferralClaims", new[] { "ClaimCode" });
            DropIndex("dbo.ReferralClaims", new[] { "CedingCompany" });
            DropIndex("dbo.ReferralClaims", new[] { "DateOfCommencement" });
            DropIndex("dbo.ReferralClaims", new[] { "InsuredDateOfBirth" });
            DropIndex("dbo.ReferralClaims", new[] { "DateReceivedFullDocuments" });
            DropIndex("dbo.ReferralClaims", new[] { "ReferralReasonId" });
            DropIndex("dbo.ReferralClaims", new[] { "InsuredTobaccoUsage" });
            DropIndex("dbo.ReferralClaims", new[] { "InsuredGenderCode" });
            DropIndex("dbo.ReferralClaims", new[] { "RecordType" });
            DropIndex("dbo.ReferralClaims", new[] { "Status" });
            DropIndex("dbo.ReferralClaims", new[] { "ReferralRiDataId" });
            DropIndex("dbo.ReferralClaims", new[] { "RiDataWarehouseId" });
            DropIndex("dbo.ReferralClaims", new[] { "ClaimRegisterId" });
            DropIndex("dbo.RiDiscounts", new[] { "UpdatedById" });
            DropIndex("dbo.RiDiscounts", new[] { "CreatedById" });
            DropIndex("dbo.RiDiscounts", new[] { "Discount" });
            DropIndex("dbo.RiDiscounts", new[] { "DurationTo" });
            DropIndex("dbo.RiDiscounts", new[] { "DurationFrom" });
            DropIndex("dbo.RiDiscounts", new[] { "DiscountCode" });
            DropIndex("dbo.RiDiscounts", new[] { "DiscountTableId" });
            DropIndex("dbo.RateTables", new[] { "GroupDiscountId" });
            DropIndex("dbo.RateTables", new[] { "LargeDiscountId" });
            DropIndex("dbo.RateTables", new[] { "RiDiscountId" });
            DropIndex("dbo.RateTables", new[] { "CedantId" });
            DropIndex("dbo.RateTables", new[] { "RateId" });
            DropIndex("dbo.RateTables", new[] { "PolicyDurationTo" });
            DropIndex("dbo.RateTables", new[] { "PolicyDurationFrom" });
            DropIndex("dbo.RateTables", new[] { "PolicyTermTo" });
            DropIndex("dbo.RateTables", new[] { "PolicyTermFrom" });
            DropIndex("dbo.RateTables", new[] { "RateTableCode" });
            DropIndex("dbo.Rates", new[] { "UpdatedById" });
            DropIndex("dbo.Rates", new[] { "CreatedById" });
            DropIndex("dbo.Rates", new[] { "RatePerBasis" });
            DropIndex("dbo.Rates", new[] { "ValuationRate" });
            DropIndex("dbo.Rates", new[] { "Code" });
            DropIndex("dbo.RateDetails", new[] { "UpdatedById" });
            DropIndex("dbo.RateDetails", new[] { "CreatedById" });
            DropIndex("dbo.RateDetails", new[] { "PolicyTermRemain" });
            DropIndex("dbo.RateDetails", new[] { "PolicyTerm" });
            DropIndex("dbo.RateDetails", new[] { "IssueAge" });
            DropIndex("dbo.RateDetails", new[] { "AttainedAge" });
            DropIndex("dbo.RateDetails", new[] { "CedingOccupationCodePickListDetailId" });
            DropIndex("dbo.RateDetails", new[] { "CedingTobaccoUsePickListDetailId" });
            DropIndex("dbo.RateDetails", new[] { "InsuredGenderCodePickListDetailId" });
            DropIndex("dbo.RateDetails", new[] { "RateId" });
            DropIndex("dbo.PublicHolidays", new[] { "UpdatedById" });
            DropIndex("dbo.PublicHolidays", new[] { "CreatedById" });
            DropIndex("dbo.PublicHolidays", new[] { "Year" });
            DropIndex("dbo.PublicHolidayDetails", new[] { "UpdatedById" });
            DropIndex("dbo.PublicHolidayDetails", new[] { "CreatedById" });
            DropIndex("dbo.PublicHolidayDetails", new[] { "PublicHolidayId" });
            DropIndex("dbo.PremiumSpreadTableDetails", new[] { "UpdatedById" });
            DropIndex("dbo.PremiumSpreadTableDetails", new[] { "CreatedById" });
            DropIndex("dbo.PremiumSpreadTableDetails", new[] { "PremiumSpread" });
            DropIndex("dbo.PremiumSpreadTableDetails", new[] { "AgeTo" });
            DropIndex("dbo.PremiumSpreadTableDetails", new[] { "AgeFrom" });
            DropIndex("dbo.PremiumSpreadTableDetails", new[] { "BenefitId" });
            DropIndex("dbo.PremiumSpreadTableDetails", new[] { "PremiumSpreadTableId" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "UpdatedById" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "CreatedById" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "AarCap2" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "AarShare2" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroNetPremium3" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroNetPremium2" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroNetPremium1" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroDiscount3" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroDiscount2" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroDiscount1" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroReinsurancePremium3" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroReinsurancePremium2" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroReinsurancePremium1" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroAar3" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroAar2" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroAar1" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroShare3" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroShare2" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroShare1" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroParty3" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroParty2" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroParty1" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "InsuredName" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "CedingBenefitRiskCode" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "CedingBenefitTypeCode" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "CedingPlanCode" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "PolicyNumber" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "TransactionTypeCode" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RiskPeriodYear" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RiskPeriodMonth" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "PremiumFrequencyCode" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "TreatyCode" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "Quarter" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RecordType" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "EndingPolicyStatus" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RiDataWarehouseId" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "CutOffId" });
            DropIndex("dbo.Mfrs17Reportings", new[] { "CutOffId" });
            DropIndex("dbo.Mfrs17ReportingDetails", new[] { "IsModified" });
            DropIndex("dbo.Mfrs17ReportingDetails", new[] { "Status" });
            DropIndex("dbo.Mfrs17ReportingDetailRiDatas", new[] { "RiDataWarehouseHistoryId" });
            DropIndex("dbo.Mfrs17CellMappings", new[] { "ProfitComm" });
            DropIndex("dbo.LargeDiscounts", new[] { "UpdatedById" });
            DropIndex("dbo.LargeDiscounts", new[] { "CreatedById" });
            DropIndex("dbo.LargeDiscounts", new[] { "Discount" });
            DropIndex("dbo.LargeDiscounts", new[] { "AarTo" });
            DropIndex("dbo.LargeDiscounts", new[] { "AarFrom" });
            DropIndex("dbo.LargeDiscounts", new[] { "DiscountCode" });
            DropIndex("dbo.LargeDiscounts", new[] { "DiscountTableId" });
            DropIndex("dbo.ItemCodes", new[] { "UpdatedById" });
            DropIndex("dbo.ItemCodes", new[] { "CreatedById" });
            DropIndex("dbo.ItemCodes", new[] { "ReportingType" });
            DropIndex("dbo.ItemCodes", new[] { "Code" });
            DropIndex("dbo.ItemCodeMappings", new[] { "UpdatedById" });
            DropIndex("dbo.ItemCodeMappings", new[] { "CreatedById" });
            DropIndex("dbo.ItemCodeMappings", new[] { "StandardSoaDataOutputId" });
            DropIndex("dbo.ItemCodeMappings", new[] { "InvoiceFieldPickListDetailId" });
            DropIndex("dbo.ItemCodeMappings", new[] { "TreatyTypePickListDetailId" });
            DropIndex("dbo.ItemCodeMappings", new[] { "ItemCodeId" });
            DropIndex("dbo.InvoiceRegisterValuations", new[] { "UpdatedById" });
            DropIndex("dbo.InvoiceRegisterValuations", new[] { "CreatedById" });
            DropIndex("dbo.InvoiceRegisterValuations", new[] { "ValuationBenefitCodeId" });
            DropIndex("dbo.InvoiceRegisterValuations", new[] { "InvoiceRegisterId" });
            DropIndex("dbo.InvoiceRegister", new[] { "UpdatedById" });
            DropIndex("dbo.InvoiceRegister", new[] { "CreatedById" });
            DropIndex("dbo.InvoiceRegister", new[] { "InvoiceNumber2" });
            DropIndex("dbo.InvoiceRegister", new[] { "InvoiceNumber1" });
            DropIndex("dbo.InvoiceRegister", new[] { "SoaQuarter" });
            DropIndex("dbo.InvoiceRegister", new[] { "PreparedById" });
            DropIndex("dbo.InvoiceRegister", new[] { "TreatyCodeId" });
            DropIndex("dbo.InvoiceRegister", new[] { "RiskQuarter" });
            DropIndex("dbo.InvoiceRegister", new[] { "CedantId" });
            DropIndex("dbo.InvoiceRegister", new[] { "InvoiceNumber" });
            DropIndex("dbo.InvoiceRegister", new[] { "InvoiceReference" });
            DropIndex("dbo.InvoiceRegister", new[] { "InvoiceType" });
            DropIndex("dbo.InvoiceRegister", new[] { "SoaDataBatchId" });
            DropIndex("dbo.InvoiceRegister", new[] { "InvoiceRegisterBatchId" });
            DropIndex("dbo.InvoiceRegisterHistories", new[] { "UpdatedById" });
            DropIndex("dbo.InvoiceRegisterHistories", new[] { "CreatedById" });
            DropIndex("dbo.InvoiceRegisterHistories", new[] { "InvoiceNumber2" });
            DropIndex("dbo.InvoiceRegisterHistories", new[] { "InvoiceNumber1" });
            DropIndex("dbo.InvoiceRegisterHistories", new[] { "SoaQuarter" });
            DropIndex("dbo.InvoiceRegisterHistories", new[] { "PreparedById" });
            DropIndex("dbo.InvoiceRegisterHistories", new[] { "TreatyCodeId" });
            DropIndex("dbo.InvoiceRegisterHistories", new[] { "RiskQuarter" });
            DropIndex("dbo.InvoiceRegisterHistories", new[] { "CedantId" });
            DropIndex("dbo.InvoiceRegisterHistories", new[] { "InvoiceNumber" });
            DropIndex("dbo.InvoiceRegisterHistories", new[] { "InvoiceReference" });
            DropIndex("dbo.InvoiceRegisterHistories", new[] { "InvoiceType" });
            DropIndex("dbo.InvoiceRegisterHistories", new[] { "SoaDataBatchId" });
            DropIndex("dbo.InvoiceRegisterHistories", new[] { "InvoiceRegisterBatchId" });
            DropIndex("dbo.InvoiceRegisterHistories", new[] { "InvoiceRegisterId" });
            DropIndex("dbo.InvoiceRegisterHistories", new[] { "CutOffId" });
            DropIndex("dbo.InvoiceRegisterBatchSoaDatas", new[] { "SoaDataBatchId" });
            DropIndex("dbo.InvoiceRegisterBatchSoaDatas", new[] { "InvoiceRegisterBatchId" });
            DropIndex("dbo.InvoiceRegisterBatchRemarks", new[] { "UpdatedById" });
            DropIndex("dbo.InvoiceRegisterBatchRemarks", new[] { "CreatedById" });
            DropIndex("dbo.InvoiceRegisterBatchRemarks", new[] { "FollowUpUserId" });
            DropIndex("dbo.InvoiceRegisterBatchRemarks", new[] { "InvoiceRegisterBatchId" });
            DropIndex("dbo.InvoiceRegisterBatchRemarkDocuments", new[] { "UpdatedById" });
            DropIndex("dbo.InvoiceRegisterBatchRemarkDocuments", new[] { "CreatedById" });
            DropIndex("dbo.InvoiceRegisterBatchRemarkDocuments", new[] { "HashFileName" });
            DropIndex("dbo.InvoiceRegisterBatchRemarkDocuments", new[] { "FileName" });
            DropIndex("dbo.InvoiceRegisterBatchRemarkDocuments", new[] { "InvoiceRegisterBatchRemarkId" });
            DropIndex("dbo.InvoiceRegisterBatchFiles", new[] { "UpdatedById" });
            DropIndex("dbo.InvoiceRegisterBatchFiles", new[] { "CreatedById" });
            DropIndex("dbo.InvoiceRegisterBatchFiles", new[] { "Status" });
            DropIndex("dbo.InvoiceRegisterBatchFiles", new[] { "HashFileName" });
            DropIndex("dbo.InvoiceRegisterBatchFiles", new[] { "FileName" });
            DropIndex("dbo.InvoiceRegisterBatchFiles", new[] { "InvoiceRegisterBatchId" });
            DropIndex("dbo.InvoiceRegisterBatches", new[] { "UpdatedById" });
            DropIndex("dbo.InvoiceRegisterBatches", new[] { "CreatedById" });
            DropIndex("dbo.InvoiceRegisterBatches", new[] { "Status" });
            DropIndex("dbo.InvoiceRegisterBatches", new[] { "BatchNo" });
            DropIndex("dbo.GroupDiscounts", new[] { "UpdatedById" });
            DropIndex("dbo.GroupDiscounts", new[] { "CreatedById" });
            DropIndex("dbo.GroupDiscounts", new[] { "Discount" });
            DropIndex("dbo.GroupDiscounts", new[] { "GroupSizeTo" });
            DropIndex("dbo.GroupDiscounts", new[] { "GroupSizeFrom" });
            DropIndex("dbo.GroupDiscounts", new[] { "DiscountCode" });
            DropIndex("dbo.GroupDiscounts", new[] { "DiscountTableId" });
            DropIndex("dbo.FinanceProvisioningTransactions", new[] { "UpdatedById" });
            DropIndex("dbo.FinanceProvisioningTransactions", new[] { "CreatedById" });
            DropIndex("dbo.FinanceProvisioningTransactions", new[] { "FinanceProvisioningId" });
            DropIndex("dbo.FinanceProvisioningTransactions", new[] { "ClaimRegisterId" });
            DropIndex("dbo.FacMasterListings", new[] { "UpdatedById" });
            DropIndex("dbo.FacMasterListings", new[] { "CreatedById" });
            DropIndex("dbo.FacMasterListings", new[] { "UwRatingOffered" });
            DropIndex("dbo.FacMasterListings", new[] { "EwarpActionCode" });
            DropIndex("dbo.FacMasterListings", new[] { "SumAssuredOffered" });
            DropIndex("dbo.FacMasterListings", new[] { "BenefitId" });
            DropIndex("dbo.FacMasterListings", new[] { "FlatExtraDuration" });
            DropIndex("dbo.FacMasterListings", new[] { "FlatExtraAmountOffered" });
            DropIndex("dbo.FacMasterListings", new[] { "CedantId" });
            DropIndex("dbo.FacMasterListings", new[] { "InsuredGenderCodePickListDetailId" });
            DropIndex("dbo.FacMasterListings", new[] { "EwarpNumber" });
            DropIndex("dbo.FacMasterListings", new[] { "UniqueId" });
            DropIndex("dbo.FacMasterListingDetails", new[] { "CreatedById" });
            DropIndex("dbo.FacMasterListingDetails", new[] { "PolicyNumber" });
            DropIndex("dbo.FacMasterListingDetails", new[] { "FacMasterListingId" });
            DropIndex("dbo.EventClaimCodeMappings", new[] { "UpdatedById" });
            DropIndex("dbo.EventClaimCodeMappings", new[] { "CreatedById" });
            DropIndex("dbo.EventClaimCodeMappings", new[] { "EventCodeId" });
            DropIndex("dbo.EventClaimCodeMappings", new[] { "CedantId" });
            DropIndex("dbo.EventClaimCodeMappingDetails", new[] { "CedingClaimType" });
            DropIndex("dbo.EventClaimCodeMappingDetails", new[] { "CedingEventCode" });
            DropIndex("dbo.EventClaimCodeMappingDetails", new[] { "CreatedById" });
            DropIndex("dbo.EventClaimCodeMappingDetails", new[] { "Combination" });
            DropIndex("dbo.EventClaimCodeMappingDetails", new[] { "EventClaimCodeMappingId" });
            DropIndex("dbo.Remarks", new[] { "FollowUpUserId" });
            DropIndex("dbo.Remarks", new[] { "DepartmentId" });
            DropIndex("dbo.Documents", new[] { "DepartmentId" });
            DropIndex("dbo.Documents", new[] { "RemarkId" });
            DropIndex("dbo.DiscountTables", new[] { "UpdatedById" });
            DropIndex("dbo.DiscountTables", new[] { "CreatedById" });
            DropIndex("dbo.DiscountTables", new[] { "CedantId" });
            DropIndex("dbo.DirectRetroStatusFiles", new[] { "UpdatedById" });
            DropIndex("dbo.DirectRetroStatusFiles", new[] { "CreatedById" });
            DropIndex("dbo.DirectRetroStatusFiles", new[] { "StatusHistoryId" });
            DropIndex("dbo.DirectRetroStatusFiles", new[] { "DirectRetroId" });
            DropIndex("dbo.FinanceProvisionings", new[] { "UpdatedById" });
            DropIndex("dbo.FinanceProvisionings", new[] { "CreatedById" });
            DropIndex("dbo.FinanceProvisionings", new[] { "Amount" });
            DropIndex("dbo.FinanceProvisionings", new[] { "Record" });
            DropIndex("dbo.FinanceProvisionings", new[] { "CutOffId" });
            DropIndex("dbo.DirectRetroProvisioningTransactions", new[] { "UpdatedById" });
            DropIndex("dbo.DirectRetroProvisioningTransactions", new[] { "CreatedById" });
            DropIndex("dbo.DirectRetroProvisioningTransactions", new[] { "RetroStatementId" });
            DropIndex("dbo.DirectRetroProvisioningTransactions", new[] { "RetroRecovery" });
            DropIndex("dbo.DirectRetroProvisioningTransactions", new[] { "RetroParty" });
            DropIndex("dbo.DirectRetroProvisioningTransactions", new[] { "Quarter" });
            DropIndex("dbo.DirectRetroProvisioningTransactions", new[] { "EntryNo" });
            DropIndex("dbo.DirectRetroProvisioningTransactions", new[] { "CedingCompany" });
            DropIndex("dbo.DirectRetroProvisioningTransactions", new[] { "ClaimId" });
            DropIndex("dbo.DirectRetroProvisioningTransactions", new[] { "IsLatestProvision" });
            DropIndex("dbo.DirectRetroProvisioningTransactions", new[] { "FinanceProvisioningId" });
            DropIndex("dbo.DirectRetroProvisioningTransactions", new[] { "ClaimRegisterId" });
            DropIndex("dbo.TreatyDiscountTables", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyDiscountTables", new[] { "CreatedById" });
            DropIndex("dbo.TreatyDiscountTables", new[] { "Rule" });
            DropIndex("dbo.RetroParties", new[] { "UpdatedById" });
            DropIndex("dbo.RetroParties", new[] { "CreatedById" });
            DropIndex("dbo.RetroParties", new[] { "Status" });
            DropIndex("dbo.RetroParties", new[] { "CountryCodePickListDetailId" });
            DropIndex("dbo.RetroParties", new[] { "Type" });
            DropIndex("dbo.RetroParties", new[] { "Code" });
            DropIndex("dbo.RetroParties", new[] { "Party" });
            DropIndex("dbo.PremiumSpreadTables", new[] { "UpdatedById" });
            DropIndex("dbo.PremiumSpreadTables", new[] { "CreatedById" });
            DropIndex("dbo.PremiumSpreadTables", new[] { "Rule" });
            DropIndex("dbo.DirectRetroConfigurationMappings", new[] { "CreatedById" });
            DropIndex("dbo.DirectRetroConfigurationMappings", new[] { "RetroParty" });
            DropIndex("dbo.DirectRetroConfigurationMappings", new[] { "Combination" });
            DropIndex("dbo.DirectRetroConfigurationMappings", new[] { "DirectRetroConfigurationId" });
            DropIndex("dbo.DirectRetroConfigurations", new[] { "UpdatedById" });
            DropIndex("dbo.DirectRetroConfigurations", new[] { "CreatedById" });
            DropIndex("dbo.DirectRetroConfigurations", new[] { "TreatyCodeId" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "UpdatedById" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "CreatedById" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "TreatyDiscountTableId" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "PremiumSpreadTableId" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "Share" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "RetroPartyId" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "IsDefault" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "ReinsEffDatePolEndDate" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "ReinsEffDatePolStartDate" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "IssueDatePolEndDate" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "IssueDatePolStartDate" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "RiskPeriodEndDate" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "RiskPeriodStartDate" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "DirectRetroConfigurationId" });
            DropIndex("dbo.TreatyCodes", new[] { "TreatyStatusPickListDetailId" });
            DropIndex("dbo.TreatyCodes", new[] { "TreatyTypePickListDetailId" });
            DropIndex("dbo.DirectRetro", new[] { "UpdatedById" });
            DropIndex("dbo.DirectRetro", new[] { "CreatedById" });
            DropIndex("dbo.DirectRetro", new[] { "RetroStatus" });
            DropIndex("dbo.DirectRetro", new[] { "SoaDataBatchId" });
            DropIndex("dbo.DirectRetro", new[] { "SoaQuarter" });
            DropIndex("dbo.DirectRetro", new[] { "TreatyCodeId" });
            DropIndex("dbo.DirectRetro", new[] { "CedantId" });
            DropIndex("dbo.CutOff", new[] { "UpdatedById" });
            DropIndex("dbo.CutOff", new[] { "CreatedById" });
            DropIndex("dbo.CutOff", new[] { "Quarter" });
            DropIndex("dbo.CutOff", new[] { "Status" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "UpdatedById" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "CreatedById" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "CampaignCode" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "CedingTreatyCode" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "SignOffById" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "UpdatedOnBehalfById" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "CeoClaimReasonId" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ClaimCommitteeDateCommented2" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ClaimCommitteeDateCommented1" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ClaimCommitteeUser2Id" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ClaimCommitteeUser1Id" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ClaimAssessorId" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "TransactionDateWop" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "TempS2" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "TempS1" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "TempI2" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "TempI1" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "TempD2" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "TempD1" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "TempA2" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "TempA1" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "SumIns" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "SoaQuarter" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "SaFactor" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RiskQuarter" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RiskPeriodYear" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RiskPeriodMonth" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RetroStatementId3" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RetroStatementId2" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RetroStatementId1" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RetroShare3" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RetroShare2" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RetroShare1" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RetroStatementDate3" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RetroStatementDate2" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RetroStatementDate1" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RetroRecovery3" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RetroRecovery2" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RetroRecovery1" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RetroParty3" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RetroParty2" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RetroParty1" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ReinsEffDatePol" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ReinsBasisCode" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RecordType" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "PolicyDuration" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "PendingProvisionDay" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "MlreShare" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "MlreRetainAmount" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "MlreInvoiceNumber" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "MlreInvoiceDate" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "MlreEventCode" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "MlreBenefitCode" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "Mfrs17ContractCode" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "Mfrs17AnnualCohort" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "Layer1SumRein" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "LateInterest" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "LastTransactionQuarter" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "LastTransactionDate" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "InsuredTobaccoUse" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "InsuredName" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "InsuredGenderCode" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "InsuredDateOfBirth" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "FundsAccountingTypeCode" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ForeignClaimRecoveryAmt" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ExGratia" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "EntryNo" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "DateOfReported" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "DateOfRegister" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "DateOfEvent" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "DateApproved" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "CurrencyCode" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "CurrencyRate" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "CedingPlanCode" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "CedingEventCode" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "CedingCompany" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "CedingClaimType" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "CedingBenefitTypeCode" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "CedingBenefitRiskCode" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "CedantDateOfNotification" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "CedantClaimType" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "CedantClaimEventCode" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "CauseOfEvent" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "AnnualRiPrem" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "AarPayable" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "TreatyType" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "TreatyCode" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ClaimTransactionType" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ClaimRecoveryAmt" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "PolicyTerm" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "PolicyNumber" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ClaimCode" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ClaimId" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "TargetDateToIssueInvoice" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "HasRedFlag" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RefferalCaseIndicator" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "PostValidationStatus" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "PostComputationStatus" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "DuplicationCheckStatus" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ProcessingStatus" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "MappingStatus" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "OffsetStatus" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ProvisionStatus" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ClaimStatus" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "PicDaaId" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "PicClaimId" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ClaimReasonId" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "OriginalClaimRegisterId" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RiDataWarehouseId" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ClaimDataConfigId" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ClaimDataId" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ClaimDataBatchId" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ClaimRegisterId" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "CutOffId" });
            DropIndex("dbo.RiDataWarehouse", new[] { "AarCap2" });
            DropIndex("dbo.RiDataWarehouse", new[] { "AarShare2" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroNetPremium3" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroNetPremium2" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroNetPremium1" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroDiscount3" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroDiscount2" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroDiscount1" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroReinsurancePremium3" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroReinsurancePremium2" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroReinsurancePremium1" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroAar3" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroAar2" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroAar1" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroShare3" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroShare2" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroShare1" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroParty3" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroParty2" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroParty1" });
            DropIndex("dbo.RiDataWarehouse", new[] { "UpdatedById" });
            DropIndex("dbo.RiDataWarehouse", new[] { "CreatedById" });
            DropIndex("dbo.RiDataWarehouse", new[] { "InsuredName" });
            DropIndex("dbo.RiDataWarehouse", new[] { "CedingBenefitRiskCode" });
            DropIndex("dbo.RiDataWarehouse", new[] { "CedingBenefitTypeCode" });
            DropIndex("dbo.RiDataWarehouse", new[] { "CedingPlanCode" });
            DropIndex("dbo.RiDataWarehouse", new[] { "PolicyNumber" });
            DropIndex("dbo.RiDataWarehouse", new[] { "TransactionTypeCode" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RiskPeriodYear" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RiskPeriodMonth" });
            DropIndex("dbo.RiDataWarehouse", new[] { "PremiumFrequencyCode" });
            DropIndex("dbo.RiDataWarehouse", new[] { "TreatyCode" });
            DropIndex("dbo.RiDataWarehouse", new[] { "Quarter" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RecordType" });
            DropIndex("dbo.RiDataWarehouse", new[] { "EndingPolicyStatus" });
            DropIndex("dbo.ReferralRiDataFiles", new[] { "UpdatedById" });
            DropIndex("dbo.ReferralRiDataFiles", new[] { "CreatedById" });
            DropIndex("dbo.ReferralRiDataFiles", new[] { "RawFileId" });
            DropIndex("dbo.ReferralRiData", new[] { "UpdatedById" });
            DropIndex("dbo.ReferralRiData", new[] { "CreatedById" });
            DropIndex("dbo.ReferralRiData", new[] { "UpdatedAt" });
            DropIndex("dbo.ReferralRiData", new[] { "CreatedAt" });
            DropIndex("dbo.ReferralRiData", new[] { "AarCap2" });
            DropIndex("dbo.ReferralRiData", new[] { "AarShare2" });
            DropIndex("dbo.ReferralRiData", new[] { "LastUpdatedDate" });
            DropIndex("dbo.ReferralRiData", new[] { "TreatyType" });
            DropIndex("dbo.ReferralRiData", new[] { "TotalDirectRetroNetPremium" });
            DropIndex("dbo.ReferralRiData", new[] { "TotalDirectRetroDiscount" });
            DropIndex("dbo.ReferralRiData", new[] { "TotalDirectRetroGrossPremium" });
            DropIndex("dbo.ReferralRiData", new[] { "TotalDirectRetroAar" });
            DropIndex("dbo.ReferralRiData", new[] { "ProfitComm" });
            DropIndex("dbo.ReferralRiData", new[] { "AarShare" });
            DropIndex("dbo.ReferralRiData", new[] { "RetentionShare" });
            DropIndex("dbo.ReferralRiData", new[] { "EwarpActionCode" });
            DropIndex("dbo.ReferralRiData", new[] { "EwarpNumber" });
            DropIndex("dbo.ReferralRiData", new[] { "GroupSizeDiscount" });
            DropIndex("dbo.ReferralRiData", new[] { "LargeSaDiscount" });
            DropIndex("dbo.ReferralRiData", new[] { "RiDiscountRate" });
            DropIndex("dbo.ReferralRiData", new[] { "RatePerBasisUnit" });
            DropIndex("dbo.ReferralRiData", new[] { "Abl" });
            DropIndex("dbo.ReferralRiData", new[] { "CorridorLimit" });
            DropIndex("dbo.ReferralRiData", new[] { "MaxAar" });
            DropIndex("dbo.ReferralRiData", new[] { "MinAar" });
            DropIndex("dbo.ReferralRiData", new[] { "MaxIssueAge" });
            DropIndex("dbo.ReferralRiData", new[] { "MinIssueAge" });
            DropIndex("dbo.ReferralRiData", new[] { "MaxExpiryAge" });
            DropIndex("dbo.ReferralRiData", new[] { "Mfrs17AnnualCohort" });
            DropIndex("dbo.ReferralRiData", new[] { "RiskPeriodEndDate" });
            DropIndex("dbo.ReferralRiData", new[] { "RiskPeriodStartDate" });
            DropIndex("dbo.ReferralRiData", new[] { "OfferLetterSentDate" });
            DropIndex("dbo.ReferralRiData", new[] { "EffectiveDate" });
            DropIndex("dbo.ReferralRiData", new[] { "FlatExtraDuration" });
            DropIndex("dbo.ReferralRiData", new[] { "FlatExtraAmountOffered" });
            DropIndex("dbo.ReferralRiData", new[] { "UwRatingOffered" });
            DropIndex("dbo.ReferralRiData", new[] { "SumAssuredOffered" });
            DropIndex("dbo.ReferralRiData", new[] { "AnnuityFactor" });
            DropIndex("dbo.ReferralRiData", new[] { "RiRate2" });
            DropIndex("dbo.ReferralRiData", new[] { "RiRate" });
            DropIndex("dbo.ReferralRiData", new[] { "AarCap" });
            DropIndex("dbo.ReferralRiData", new[] { "RetentionCap" });
            DropIndex("dbo.ReferralRiData", new[] { "MaxUwRating" });
            DropIndex("dbo.ReferralRiData", new[] { "BrokerageFee" });
            DropIndex("dbo.ReferralRiData", new[] { "OriginalDiscount" });
            DropIndex("dbo.ReferralRiData", new[] { "TransactionDiscount" });
            DropIndex("dbo.ReferralRiData", new[] { "OriginalPremium" });
            DropIndex("dbo.ReferralRiData", new[] { "TransactionPremium" });
            DropIndex("dbo.ReferralRiData", new[] { "Layer1FlatExtraPremium" });
            DropIndex("dbo.ReferralRiData", new[] { "NetPremiumAlt" });
            DropIndex("dbo.ReferralRiData", new[] { "GrossPremiumAlt" });
            DropIndex("dbo.ReferralRiData", new[] { "DatabaseCommision" });
            DropIndex("dbo.ReferralRiData", new[] { "SurrenderValue" });
            DropIndex("dbo.ReferralRiData", new[] { "NoClaimBonus" });
            DropIndex("dbo.ReferralRiData", new[] { "CurrencyRate" });
            DropIndex("dbo.ReferralRiData", new[] { "LoaCode" });
            DropIndex("dbo.ReferralRiData", new[] { "Mfrs17TreatyCode" });
            DropIndex("dbo.ReferralRiData", new[] { "Mfrs17CellName" });
            DropIndex("dbo.ReferralRiData", new[] { "Mfrs17BasicRider" });
            DropIndex("dbo.ReferralRiData", new[] { "GstAmount" });
            DropIndex("dbo.ReferralRiData", new[] { "GstVitality" });
            DropIndex("dbo.ReferralRiData", new[] { "GstTotalDiscount" });
            DropIndex("dbo.ReferralRiData", new[] { "GstGrossPremium" });
            DropIndex("dbo.ReferralRiData", new[] { "GstIndicator" });
            DropIndex("dbo.ReferralRiData", new[] { "TaxAmount" });
            DropIndex("dbo.ReferralRiData", new[] { "IndicatorJointLife" });
            DropIndex("dbo.ReferralRiData", new[] { "Layer1NetPremiumAlt" });
            DropIndex("dbo.ReferralRiData", new[] { "Layer1TotalDiscountAlt" });
            DropIndex("dbo.ReferralRiData", new[] { "Layer1GrossPremiumAlt" });
            DropIndex("dbo.ReferralRiData", new[] { "Layer1NetPremium" });
            DropIndex("dbo.ReferralRiData", new[] { "Layer1TotalDiscount" });
            DropIndex("dbo.ReferralRiData", new[] { "Layer1SubstandardDiscount" });
            DropIndex("dbo.ReferralRiData", new[] { "Layer1StandardDiscount" });
            DropIndex("dbo.ReferralRiData", new[] { "Layer1GrossPremium" });
            DropIndex("dbo.ReferralRiData", new[] { "Layer1SubstandardPremium" });
            DropIndex("dbo.ReferralRiData", new[] { "Layer1StandardPremium" });
            DropIndex("dbo.ReferralRiData", new[] { "Layer1InsuredAttainedAge2nd" });
            DropIndex("dbo.ReferralRiData", new[] { "Layer1InsuredAttainedAge" });
            DropIndex("dbo.ReferralRiData", new[] { "Layer1RiShare" });
            DropIndex("dbo.ReferralRiData", new[] { "PolicyAmountSubstandard" });
            DropIndex("dbo.ReferralRiData", new[] { "GhsRoomBoard" });
            DropIndex("dbo.ReferralRiData", new[] { "DependantIndicator" });
            DropIndex("dbo.ReferralRiData", new[] { "CedingPlanCode2" });
            DropIndex("dbo.ReferralRiData", new[] { "GroupEmployeeBasicSalaryMultiplier" });
            DropIndex("dbo.ReferralRiData", new[] { "GroupEmployeeBasicSalaryRevise" });
            DropIndex("dbo.ReferralRiData", new[] { "GroupEmployeeJobCode" });
            DropIndex("dbo.ReferralRiData", new[] { "GroupEmployeeJobType" });
            DropIndex("dbo.ReferralRiData", new[] { "GroupEmployeeBasicSalary" });
            DropIndex("dbo.ReferralRiData", new[] { "GroupSubsidiaryNo" });
            DropIndex("dbo.ReferralRiData", new[] { "GroupSubsidiaryName" });
            DropIndex("dbo.ReferralRiData", new[] { "PolicyTotalLive" });
            DropIndex("dbo.ReferralRiData", new[] { "NoOfEmployee" });
            DropIndex("dbo.ReferralRiData", new[] { "GroupPolicyName" });
            DropIndex("dbo.ReferralRiData", new[] { "GroupPolicyNumber" });
            DropIndex("dbo.ReferralRiData", new[] { "CessionCode" });
            DropIndex("dbo.ReferralRiData", new[] { "AmountCededB4MlreShare2" });
            DropIndex("dbo.ReferralRiData", new[] { "CedantReinsurerCode" });
            DropIndex("dbo.ReferralRiData", new[] { "CedantSar" });
            DropIndex("dbo.ReferralRiData", new[] { "CedingBasicPlanCode" });
            DropIndex("dbo.ReferralRiData", new[] { "CedingPlanCodeOld" });
            DropIndex("dbo.ReferralRiData", new[] { "CedingTreatyCode" });
            DropIndex("dbo.ReferralRiData", new[] { "StaffPlanIndicator" });
            DropIndex("dbo.ReferralRiData", new[] { "CurrencyCode" });
            DropIndex("dbo.ReferralRiData", new[] { "TerritoryOfIssueCode" });
            DropIndex("dbo.ReferralRiData", new[] { "Nationality" });
            DropIndex("dbo.ReferralRiData", new[] { "CampaignCode" });
            DropIndex("dbo.ReferralRiData", new[] { "RiderNumber" });
            DropIndex("dbo.ReferralRiData", new[] { "DefermentPeriod" });
            DropIndex("dbo.ReferralRiData", new[] { "LoanInterestRate" });
            DropIndex("dbo.ReferralRiData", new[] { "ApLoading" });
            DropIndex("dbo.ReferralRiData", new[] { "LineOfBusiness" });
            DropIndex("dbo.ReferralRiData", new[] { "FundCode" });
            DropIndex("dbo.ReferralRiData", new[] { "PolicyLifeNumber" });
            DropIndex("dbo.ReferralRiData", new[] { "PolicyPaymentMethod" });
            DropIndex("dbo.ReferralRiData", new[] { "PolicyReserve" });
            DropIndex("dbo.ReferralRiData", new[] { "PolicyAmountDeath" });
            DropIndex("dbo.ReferralRiData", new[] { "PolicyTermRemain" });
            DropIndex("dbo.ReferralRiData", new[] { "PolicySubstandardPremium" });
            DropIndex("dbo.ReferralRiData", new[] { "PolicyStandardPremium" });
            DropIndex("dbo.ReferralRiData", new[] { "PolicyGrossPremium" });
            DropIndex("dbo.ReferralRiData", new[] { "PolicyStatusCode" });
            DropIndex("dbo.ReferralRiData", new[] { "PolicyNumberOld" });
            DropIndex("dbo.ReferralRiData", new[] { "AdjEndDate" });
            DropIndex("dbo.ReferralRiData", new[] { "AdjBeginDate" });
            DropIndex("dbo.ReferralRiData", new[] { "RiCovPeriod" });
            DropIndex("dbo.ReferralRiData", new[] { "AnnualRiPrem" });
            DropIndex("dbo.ReferralRiData", new[] { "NetPremium" });
            DropIndex("dbo.ReferralRiData", new[] { "TotalDiscount" });
            DropIndex("dbo.ReferralRiData", new[] { "VitalityDiscount" });
            DropIndex("dbo.ReferralRiData", new[] { "SubstandardDiscount" });
            DropIndex("dbo.ReferralRiData", new[] { "StandardDiscount" });
            DropIndex("dbo.ReferralRiData", new[] { "GrossPremium" });
            DropIndex("dbo.ReferralRiData", new[] { "FlatExtraPremium" });
            DropIndex("dbo.ReferralRiData", new[] { "SubstandardPremium" });
            DropIndex("dbo.ReferralRiData", new[] { "StandardPremium" });
            DropIndex("dbo.ReferralRiData", new[] { "FlatExtraTerm2" });
            DropIndex("dbo.ReferralRiData", new[] { "FlatExtraAmount2" });
            DropIndex("dbo.ReferralRiData", new[] { "FlatExtraTerm" });
            DropIndex("dbo.ReferralRiData", new[] { "FlatExtraUnit" });
            DropIndex("dbo.ReferralRiData", new[] { "FlatExtraAmount" });
            DropIndex("dbo.ReferralRiData", new[] { "UnderwriterRatingTerm3" });
            DropIndex("dbo.ReferralRiData", new[] { "UnderwriterRatingUnit3" });
            DropIndex("dbo.ReferralRiData", new[] { "UnderwriterRating3" });
            DropIndex("dbo.ReferralRiData", new[] { "UnderwriterRatingTerm2" });
            DropIndex("dbo.ReferralRiData", new[] { "UnderwriterRatingUnit2" });
            DropIndex("dbo.ReferralRiData", new[] { "UnderwriterRating2" });
            DropIndex("dbo.ReferralRiData", new[] { "UnderwriterRatingTerm" });
            DropIndex("dbo.ReferralRiData", new[] { "UnderwriterRatingUnit" });
            DropIndex("dbo.ReferralRiData", new[] { "UnderwriterRating" });
            DropIndex("dbo.ReferralRiData", new[] { "LoadingType" });
            DropIndex("dbo.ReferralRiData", new[] { "DiscountRate" });
            DropIndex("dbo.ReferralRiData", new[] { "AgeRatedUp" });
            DropIndex("dbo.ReferralRiData", new[] { "RateTable" });
            DropIndex("dbo.ReferralRiData", new[] { "CedantRiRate" });
            DropIndex("dbo.ReferralRiData", new[] { "PremiumCalType" });
            DropIndex("dbo.ReferralRiData", new[] { "DurationMonth" });
            DropIndex("dbo.ReferralRiData", new[] { "DurationDay" });
            DropIndex("dbo.ReferralRiData", new[] { "DurationYear" });
            DropIndex("dbo.ReferralRiData", new[] { "PolicyExpiryDate" });
            DropIndex("dbo.ReferralRiData", new[] { "PolicyTerm" });
            DropIndex("dbo.ReferralRiData", new[] { "ReinsuranceIssueAge2nd" });
            DropIndex("dbo.ReferralRiData", new[] { "ReinsuranceIssueAge" });
            DropIndex("dbo.ReferralRiData", new[] { "InsuredOldIcNumber2nd" });
            DropIndex("dbo.ReferralRiData", new[] { "InsuredNewIcNumber2nd" });
            DropIndex("dbo.ReferralRiData", new[] { "InsuredAttainedAge2nd" });
            DropIndex("dbo.ReferralRiData", new[] { "InsuredDateOfBirth2nd" });
            DropIndex("dbo.ReferralRiData", new[] { "InsuredTobaccoUse2nd" });
            DropIndex("dbo.ReferralRiData", new[] { "InsuredGenderCode2nd" });
            DropIndex("dbo.ReferralRiData", new[] { "InsuredName2nd" });
            DropIndex("dbo.ReferralRiData", new[] { "InsuredOldIcNumber" });
            DropIndex("dbo.ReferralRiData", new[] { "InsuredNewIcNumber" });
            DropIndex("dbo.ReferralRiData", new[] { "InsuredAttainedAge" });
            DropIndex("dbo.ReferralRiData", new[] { "InsuredRegisterNo" });
            DropIndex("dbo.ReferralRiData", new[] { "InsuredOccupationCode" });
            DropIndex("dbo.ReferralRiData", new[] { "InsuredDateOfBirth" });
            DropIndex("dbo.ReferralRiData", new[] { "InsuredTobaccoUse" });
            DropIndex("dbo.ReferralRiData", new[] { "InsuredGenderCode" });
            DropIndex("dbo.ReferralRiData", new[] { "InsuredName" });
            DropIndex("dbo.ReferralRiData", new[] { "AarSpecial3" });
            DropIndex("dbo.ReferralRiData", new[] { "AarSpecial2" });
            DropIndex("dbo.ReferralRiData", new[] { "AarSpecial1" });
            DropIndex("dbo.ReferralRiData", new[] { "Aar" });
            DropIndex("dbo.ReferralRiData", new[] { "AarOri" });
            DropIndex("dbo.ReferralRiData", new[] { "RetentionAmount" });
            DropIndex("dbo.ReferralRiData", new[] { "AmountCededB4MlreShare" });
            DropIndex("dbo.ReferralRiData", new[] { "CurrSumAssured" });
            DropIndex("dbo.ReferralRiData", new[] { "OriSumAssured" });
            DropIndex("dbo.ReferralRiData", new[] { "MlreBenefitCode" });
            DropIndex("dbo.ReferralRiData", new[] { "CedingBenefitRiskCode" });
            DropIndex("dbo.ReferralRiData", new[] { "CedingBenefitTypeCode" });
            DropIndex("dbo.ReferralRiData", new[] { "CedingPlanCode" });
            DropIndex("dbo.ReferralRiData", new[] { "ReinsEffDateBen" });
            DropIndex("dbo.ReferralRiData", new[] { "ReinsEffDatePol" });
            DropIndex("dbo.ReferralRiData", new[] { "IssueDateBen" });
            DropIndex("dbo.ReferralRiData", new[] { "IssueDatePol" });
            DropIndex("dbo.ReferralRiData", new[] { "PolicyNumber" });
            DropIndex("dbo.ReferralRiData", new[] { "TransactionTypeCode" });
            DropIndex("dbo.ReferralRiData", new[] { "RiskPeriodYear" });
            DropIndex("dbo.ReferralRiData", new[] { "RiskPeriodMonth" });
            DropIndex("dbo.ReferralRiData", new[] { "ReportPeriodYear" });
            DropIndex("dbo.ReferralRiData", new[] { "ReportPeriodMonth" });
            DropIndex("dbo.ReferralRiData", new[] { "PremiumFrequencyCode" });
            DropIndex("dbo.ReferralRiData", new[] { "FundsAccountingTypeCode" });
            DropIndex("dbo.ReferralRiData", new[] { "ReinsBasisCode" });
            DropIndex("dbo.ReferralRiData", new[] { "TreatyCode" });
            DropIndex("dbo.ReferralRiData", new[] { "ReferralRiDataFileId" });
            DropIndex("dbo.ClaimRegister", new[] { "UpdatedById" });
            DropIndex("dbo.ClaimRegister", new[] { "CreatedById" });
            DropIndex("dbo.ClaimRegister", new[] { "CampaignCode" });
            DropIndex("dbo.ClaimRegister", new[] { "CedingTreatyCode" });
            DropIndex("dbo.ClaimRegister", new[] { "SignOffById" });
            DropIndex("dbo.ClaimRegister", new[] { "UpdatedOnBehalfById" });
            DropIndex("dbo.ClaimRegister", new[] { "CeoClaimReasonId" });
            DropIndex("dbo.ClaimRegister", new[] { "ClaimCommitteeDateCommented2" });
            DropIndex("dbo.ClaimRegister", new[] { "ClaimCommitteeDateCommented1" });
            DropIndex("dbo.ClaimRegister", new[] { "ClaimCommitteeUser2Id" });
            DropIndex("dbo.ClaimRegister", new[] { "ClaimCommitteeUser1Id" });
            DropIndex("dbo.ClaimRegister", new[] { "ClaimAssessorId" });
            DropIndex("dbo.ClaimRegister", new[] { "TransactionDateWop" });
            DropIndex("dbo.ClaimRegister", new[] { "TempS2" });
            DropIndex("dbo.ClaimRegister", new[] { "TempS1" });
            DropIndex("dbo.ClaimRegister", new[] { "TempI2" });
            DropIndex("dbo.ClaimRegister", new[] { "TempI1" });
            DropIndex("dbo.ClaimRegister", new[] { "TempD2" });
            DropIndex("dbo.ClaimRegister", new[] { "TempD1" });
            DropIndex("dbo.ClaimRegister", new[] { "TempA2" });
            DropIndex("dbo.ClaimRegister", new[] { "TempA1" });
            DropIndex("dbo.ClaimRegister", new[] { "SumIns" });
            DropIndex("dbo.ClaimRegister", new[] { "SoaQuarter" });
            DropIndex("dbo.ClaimRegister", new[] { "SaFactor" });
            DropIndex("dbo.ClaimRegister", new[] { "RiskQuarter" });
            DropIndex("dbo.ClaimRegister", new[] { "RiskPeriodYear" });
            DropIndex("dbo.ClaimRegister", new[] { "RiskPeriodMonth" });
            DropIndex("dbo.ClaimRegister", new[] { "RetroStatementId3" });
            DropIndex("dbo.ClaimRegister", new[] { "RetroStatementId2" });
            DropIndex("dbo.ClaimRegister", new[] { "RetroStatementId1" });
            DropIndex("dbo.ClaimRegister", new[] { "RetroShare3" });
            DropIndex("dbo.ClaimRegister", new[] { "RetroShare2" });
            DropIndex("dbo.ClaimRegister", new[] { "RetroShare1" });
            DropIndex("dbo.ClaimRegister", new[] { "RetroStatementDate3" });
            DropIndex("dbo.ClaimRegister", new[] { "RetroStatementDate2" });
            DropIndex("dbo.ClaimRegister", new[] { "RetroStatementDate1" });
            DropIndex("dbo.ClaimRegister", new[] { "RetroRecovery3" });
            DropIndex("dbo.ClaimRegister", new[] { "RetroRecovery2" });
            DropIndex("dbo.ClaimRegister", new[] { "RetroRecovery1" });
            DropIndex("dbo.ClaimRegister", new[] { "RetroParty3" });
            DropIndex("dbo.ClaimRegister", new[] { "RetroParty2" });
            DropIndex("dbo.ClaimRegister", new[] { "RetroParty1" });
            DropIndex("dbo.ClaimRegister", new[] { "ReinsEffDatePol" });
            DropIndex("dbo.ClaimRegister", new[] { "ReinsBasisCode" });
            DropIndex("dbo.ClaimRegister", new[] { "RecordType" });
            DropIndex("dbo.ClaimRegister", new[] { "PolicyDuration" });
            DropIndex("dbo.ClaimRegister", new[] { "PendingProvisionDay" });
            DropIndex("dbo.ClaimRegister", new[] { "MlreShare" });
            DropIndex("dbo.ClaimRegister", new[] { "MlreRetainAmount" });
            DropIndex("dbo.ClaimRegister", new[] { "MlreInvoiceNumber" });
            DropIndex("dbo.ClaimRegister", new[] { "MlreInvoiceDate" });
            DropIndex("dbo.ClaimRegister", new[] { "MlreEventCode" });
            DropIndex("dbo.ClaimRegister", new[] { "MlreBenefitCode" });
            DropIndex("dbo.ClaimRegister", new[] { "Mfrs17ContractCode" });
            DropIndex("dbo.ClaimRegister", new[] { "Mfrs17AnnualCohort" });
            DropIndex("dbo.ClaimRegister", new[] { "Layer1SumRein" });
            DropIndex("dbo.ClaimRegister", new[] { "LateInterest" });
            DropIndex("dbo.ClaimRegister", new[] { "LastTransactionQuarter" });
            DropIndex("dbo.ClaimRegister", new[] { "LastTransactionDate" });
            DropIndex("dbo.ClaimRegister", new[] { "InsuredTobaccoUse" });
            DropIndex("dbo.ClaimRegister", new[] { "InsuredName" });
            DropIndex("dbo.ClaimRegister", new[] { "InsuredGenderCode" });
            DropIndex("dbo.ClaimRegister", new[] { "InsuredDateOfBirth" });
            DropIndex("dbo.ClaimRegister", new[] { "FundsAccountingTypeCode" });
            DropIndex("dbo.ClaimRegister", new[] { "ForeignClaimRecoveryAmt" });
            DropIndex("dbo.ClaimRegister", new[] { "ExGratia" });
            DropIndex("dbo.ClaimRegister", new[] { "EntryNo" });
            DropIndex("dbo.ClaimRegister", new[] { "DateOfReported" });
            DropIndex("dbo.ClaimRegister", new[] { "DateOfRegister" });
            DropIndex("dbo.ClaimRegister", new[] { "DateOfEvent" });
            DropIndex("dbo.ClaimRegister", new[] { "DateApproved" });
            DropIndex("dbo.ClaimRegister", new[] { "CurrencyCode" });
            DropIndex("dbo.ClaimRegister", new[] { "CurrencyRate" });
            DropIndex("dbo.ClaimRegister", new[] { "CedingPlanCode" });
            DropIndex("dbo.ClaimRegister", new[] { "CedingEventCode" });
            DropIndex("dbo.ClaimRegister", new[] { "CedingCompany" });
            DropIndex("dbo.ClaimRegister", new[] { "CedingClaimType" });
            DropIndex("dbo.ClaimRegister", new[] { "CedingBenefitTypeCode" });
            DropIndex("dbo.ClaimRegister", new[] { "CedingBenefitRiskCode" });
            DropIndex("dbo.ClaimRegister", new[] { "CedantDateOfNotification" });
            DropIndex("dbo.ClaimRegister", new[] { "CedantClaimType" });
            DropIndex("dbo.ClaimRegister", new[] { "CedantClaimEventCode" });
            DropIndex("dbo.ClaimRegister", new[] { "CauseOfEvent" });
            DropIndex("dbo.ClaimRegister", new[] { "AnnualRiPrem" });
            DropIndex("dbo.ClaimRegister", new[] { "AarPayable" });
            DropIndex("dbo.ClaimRegister", new[] { "TreatyType" });
            DropIndex("dbo.ClaimRegister", new[] { "TreatyCode" });
            DropIndex("dbo.ClaimRegister", new[] { "ClaimTransactionType" });
            DropIndex("dbo.ClaimRegister", new[] { "ClaimRecoveryAmt" });
            DropIndex("dbo.ClaimRegister", new[] { "PolicyTerm" });
            DropIndex("dbo.ClaimRegister", new[] { "PolicyNumber" });
            DropIndex("dbo.ClaimRegister", new[] { "ClaimCode" });
            DropIndex("dbo.ClaimRegister", new[] { "ClaimId" });
            DropIndex("dbo.ClaimRegister", new[] { "TargetDateToIssueInvoice" });
            DropIndex("dbo.ClaimRegister", new[] { "HasRedFlag" });
            DropIndex("dbo.ClaimRegister", new[] { "IsReferralCase" });
            DropIndex("dbo.ClaimRegister", new[] { "PostValidationStatus" });
            DropIndex("dbo.ClaimRegister", new[] { "PostComputationStatus" });
            DropIndex("dbo.ClaimRegister", new[] { "DuplicationCheckStatus" });
            DropIndex("dbo.ClaimRegister", new[] { "ProcessingStatus" });
            DropIndex("dbo.ClaimRegister", new[] { "MappingStatus" });
            DropIndex("dbo.ClaimRegister", new[] { "OffsetStatus" });
            DropIndex("dbo.ClaimRegister", new[] { "ProvisionStatus" });
            DropIndex("dbo.ClaimRegister", new[] { "ClaimStatus" });
            DropIndex("dbo.ClaimRegister", new[] { "PicDaaId" });
            DropIndex("dbo.ClaimRegister", new[] { "PicClaimId" });
            DropIndex("dbo.ClaimRegister", new[] { "ClaimReasonId" });
            DropIndex("dbo.ClaimRegister", new[] { "OriginalClaimRegisterId" });
            DropIndex("dbo.ClaimRegister", new[] { "ReferralRiDataId" });
            DropIndex("dbo.ClaimRegister", new[] { "RiDataWarehouseId" });
            DropIndex("dbo.ClaimRegister", new[] { "ClaimDataConfigId" });
            DropIndex("dbo.ClaimRegister", new[] { "ClaimDataId" });
            DropIndex("dbo.ClaimRegister", new[] { "ClaimDataBatchId" });
            DropIndex("dbo.ClaimReasons", new[] { "UpdatedById" });
            DropIndex("dbo.ClaimReasons", new[] { "CreatedById" });
            DropIndex("dbo.ClaimReasons", new[] { "Type" });
            DropIndex("dbo.ClaimDataValidations", new[] { "UpdatedById" });
            DropIndex("dbo.ClaimDataValidations", new[] { "CreatedById" });
            DropIndex("dbo.ClaimDataValidations", new[] { "ClaimDataConfigId" });
            DropIndex("dbo.ClaimDataMappings", new[] { "UpdatedById" });
            DropIndex("dbo.ClaimDataMappings", new[] { "CreatedById" });
            DropIndex("dbo.ClaimDataMappings", new[] { "StandardClaimDataOutputId" });
            DropIndex("dbo.ClaimDataMappings", new[] { "ClaimDataConfigId" });
            DropIndex("dbo.ClaimDataMappingDetails", new[] { "UpdatedById" });
            DropIndex("dbo.ClaimDataMappingDetails", new[] { "CreatedById" });
            DropIndex("dbo.ClaimDataMappingDetails", new[] { "PickListDetailId" });
            DropIndex("dbo.ClaimDataMappingDetails", new[] { "ClaimDataMappingId" });
            DropIndex("dbo.ClaimDataComputations", new[] { "UpdatedById" });
            DropIndex("dbo.ClaimDataComputations", new[] { "CreatedById" });
            DropIndex("dbo.ClaimDataComputations", new[] { "StandardClaimDataOutputId" });
            DropIndex("dbo.ClaimDataComputations", new[] { "Description" });
            DropIndex("dbo.ClaimDataComputations", new[] { "ClaimDataConfigId" });
            DropIndex("dbo.ClaimDataBatchStatusFiles", new[] { "UpdatedById" });
            DropIndex("dbo.ClaimDataBatchStatusFiles", new[] { "CreatedById" });
            DropIndex("dbo.ClaimDataBatchStatusFiles", new[] { "StatusHistoryId" });
            DropIndex("dbo.ClaimDataBatchStatusFiles", new[] { "ClaimDataBatchId" });
            DropIndex("dbo.ClaimDataFiles", new[] { "UpdatedById" });
            DropIndex("dbo.ClaimDataFiles", new[] { "CreatedById" });
            DropIndex("dbo.ClaimDataFiles", new[] { "Status" });
            DropIndex("dbo.ClaimDataFiles", new[] { "CurrencyRate" });
            DropIndex("dbo.ClaimDataFiles", new[] { "CurrencyCodeId" });
            DropIndex("dbo.ClaimDataFiles", new[] { "ClaimDataConfigId" });
            DropIndex("dbo.ClaimDataFiles", new[] { "TreatyId" });
            DropIndex("dbo.ClaimDataFiles", new[] { "RawFileId" });
            DropIndex("dbo.ClaimDataFiles", new[] { "ClaimDataBatchId" });
            DropIndex("dbo.RiDataBatches", new[] { "SoaDataBatchId" });
            DropIndex("dbo.RiDataBatches", new[] { "RecordType" });
            DropIndex("dbo.RiDataBatches", new[] { "ProcessWarehouseStatus" });
            DropIndex("dbo.SoaDataBatches", new[] { "UpdatedById" });
            DropIndex("dbo.SoaDataBatches", new[] { "CreatedById" });
            DropIndex("dbo.SoaDataBatches", new[] { "ClaimDataBatchId" });
            DropIndex("dbo.SoaDataBatches", new[] { "RiDataBatchId" });
            DropIndex("dbo.SoaDataBatches", new[] { "DataUpdateStatus" });
            DropIndex("dbo.SoaDataBatches", new[] { "Status" });
            DropIndex("dbo.SoaDataBatches", new[] { "Type" });
            DropIndex("dbo.SoaDataBatches", new[] { "CurrencyCodePickListDetailId" });
            DropIndex("dbo.SoaDataBatches", new[] { "Quarter" });
            DropIndex("dbo.SoaDataBatches", new[] { "TreatyId" });
            DropIndex("dbo.SoaDataBatches", new[] { "CedantId" });
            DropIndex("dbo.Treaties", new[] { "BlockDescription" });
            DropIndex("dbo.Treaties", new[] { "LineOfBusinessPickListDetailId" });
            DropIndex("dbo.Treaties", new[] { "BusinessOriginPickListDetailId" });
            DropIndex("dbo.ClaimDataConfigs", new[] { "UpdatedById" });
            DropIndex("dbo.ClaimDataConfigs", new[] { "CreatedById" });
            DropIndex("dbo.ClaimDataConfigs", new[] { "Name" });
            DropIndex("dbo.ClaimDataConfigs", new[] { "Code" });
            DropIndex("dbo.ClaimDataConfigs", new[] { "TreatyId" });
            DropIndex("dbo.ClaimDataConfigs", new[] { "CedantId" });
            DropIndex("dbo.ClaimDataBatches", new[] { "UpdatedById" });
            DropIndex("dbo.ClaimDataBatches", new[] { "CreatedById" });
            DropIndex("dbo.ClaimDataBatches", new[] { "Quarter" });
            DropIndex("dbo.ClaimDataBatches", new[] { "SoaDataBatchId" });
            DropIndex("dbo.ClaimDataBatches", new[] { "ClaimDataConfigId" });
            DropIndex("dbo.ClaimDataBatches", new[] { "TreatyId" });
            DropIndex("dbo.ClaimDataBatches", new[] { "CedantId" });
            DropIndex("dbo.ClaimDataBatches", new[] { "Status" });
            DropIndex("dbo.ClaimData", new[] { "CampaignCode" });
            DropIndex("dbo.ClaimData", new[] { "CedingTreatyCode" });
            DropIndex("dbo.ClaimData", new[] { "DateOfReported" });
            DropIndex("dbo.ClaimData", new[] { "UpdatedById" });
            DropIndex("dbo.ClaimData", new[] { "CreatedById" });
            DropIndex("dbo.ClaimData", new[] { "TransactionDateWop" });
            DropIndex("dbo.ClaimData", new[] { "TempS2" });
            DropIndex("dbo.ClaimData", new[] { "TempS1" });
            DropIndex("dbo.ClaimData", new[] { "TempI2" });
            DropIndex("dbo.ClaimData", new[] { "TempI1" });
            DropIndex("dbo.ClaimData", new[] { "TempD2" });
            DropIndex("dbo.ClaimData", new[] { "TempD1" });
            DropIndex("dbo.ClaimData", new[] { "TempA2" });
            DropIndex("dbo.ClaimData", new[] { "TempA1" });
            DropIndex("dbo.ClaimData", new[] { "SumIns" });
            DropIndex("dbo.ClaimData", new[] { "SoaQuarter" });
            DropIndex("dbo.ClaimData", new[] { "SaFactor" });
            DropIndex("dbo.ClaimData", new[] { "RiskQuarter" });
            DropIndex("dbo.ClaimData", new[] { "RiskPeriodYear" });
            DropIndex("dbo.ClaimData", new[] { "RiskPeriodMonth" });
            DropIndex("dbo.ClaimData", new[] { "RetroStatementId3" });
            DropIndex("dbo.ClaimData", new[] { "RetroStatementId2" });
            DropIndex("dbo.ClaimData", new[] { "RetroStatementId1" });
            DropIndex("dbo.ClaimData", new[] { "RetroStatementDate3" });
            DropIndex("dbo.ClaimData", new[] { "RetroStatementDate2" });
            DropIndex("dbo.ClaimData", new[] { "RetroStatementDate1" });
            DropIndex("dbo.ClaimData", new[] { "RetroRecovery3" });
            DropIndex("dbo.ClaimData", new[] { "RetroRecovery2" });
            DropIndex("dbo.ClaimData", new[] { "RetroRecovery1" });
            DropIndex("dbo.ClaimData", new[] { "RetroParty3" });
            DropIndex("dbo.ClaimData", new[] { "RetroParty2" });
            DropIndex("dbo.ClaimData", new[] { "RetroParty1" });
            DropIndex("dbo.ClaimData", new[] { "ReinsEffDatePol" });
            DropIndex("dbo.ClaimData", new[] { "ReinsBasisCode" });
            DropIndex("dbo.ClaimData", new[] { "PolicyDuration" });
            DropIndex("dbo.ClaimData", new[] { "PendingProvisionDay" });
            DropIndex("dbo.ClaimData", new[] { "MlreShare" });
            DropIndex("dbo.ClaimData", new[] { "MlreRetainAmount" });
            DropIndex("dbo.ClaimData", new[] { "MlreInvoiceNumber" });
            DropIndex("dbo.ClaimData", new[] { "MlreInvoiceDate" });
            DropIndex("dbo.ClaimData", new[] { "MlreEventCode" });
            DropIndex("dbo.ClaimData", new[] { "MlreBenefitCode" });
            DropIndex("dbo.ClaimData", new[] { "Mfrs17ContractCode" });
            DropIndex("dbo.ClaimData", new[] { "Mfrs17AnnualCohort" });
            DropIndex("dbo.ClaimData", new[] { "Layer1SumRein" });
            DropIndex("dbo.ClaimData", new[] { "LateInterest" });
            DropIndex("dbo.ClaimData", new[] { "LastTransactionQuarter" });
            DropIndex("dbo.ClaimData", new[] { "LastTransactionDate" });
            DropIndex("dbo.ClaimData", new[] { "InsuredTobaccoUse" });
            DropIndex("dbo.ClaimData", new[] { "InsuredName" });
            DropIndex("dbo.ClaimData", new[] { "InsuredGenderCode" });
            DropIndex("dbo.ClaimData", new[] { "InsuredDateOfBirth" });
            DropIndex("dbo.ClaimData", new[] { "FundsAccountingTypeCode" });
            DropIndex("dbo.ClaimData", new[] { "ForeignClaimRecoveryAmt" });
            DropIndex("dbo.ClaimData", new[] { "ExGratia" });
            DropIndex("dbo.ClaimData", new[] { "EntryNo" });
            DropIndex("dbo.ClaimData", new[] { "DateOfEvent" });
            DropIndex("dbo.ClaimData", new[] { "DateApproved" });
            DropIndex("dbo.ClaimData", new[] { "CurrencyCode" });
            DropIndex("dbo.ClaimData", new[] { "CurrencyRate" });
            DropIndex("dbo.ClaimData", new[] { "CedingPlanCode" });
            DropIndex("dbo.ClaimData", new[] { "CedingEventCode" });
            DropIndex("dbo.ClaimData", new[] { "CedingCompany" });
            DropIndex("dbo.ClaimData", new[] { "CedingClaimType" });
            DropIndex("dbo.ClaimData", new[] { "CedingBenefitTypeCode" });
            DropIndex("dbo.ClaimData", new[] { "CedingBenefitRiskCode" });
            DropIndex("dbo.ClaimData", new[] { "CedantDateOfNotification" });
            DropIndex("dbo.ClaimData", new[] { "CedantClaimType" });
            DropIndex("dbo.ClaimData", new[] { "CedantClaimEventCode" });
            DropIndex("dbo.ClaimData", new[] { "CauseOfEvent" });
            DropIndex("dbo.ClaimData", new[] { "AnnualRiPrem" });
            DropIndex("dbo.ClaimData", new[] { "AarPayable" });
            DropIndex("dbo.ClaimData", new[] { "TreatyType" });
            DropIndex("dbo.ClaimData", new[] { "TreatyCode" });
            DropIndex("dbo.ClaimData", new[] { "ClaimTransactionType" });
            DropIndex("dbo.ClaimData", new[] { "ClaimRecoveryAmt" });
            DropIndex("dbo.ClaimData", new[] { "PolicyTerm" });
            DropIndex("dbo.ClaimData", new[] { "PolicyNumber" });
            DropIndex("dbo.ClaimData", new[] { "PreValidationStatus" });
            DropIndex("dbo.ClaimData", new[] { "PreComputationStatus" });
            DropIndex("dbo.ClaimData", new[] { "MappingStatus" });
            DropIndex("dbo.ClaimData", new[] { "ClaimCode" });
            DropIndex("dbo.ClaimData", new[] { "ClaimId" });
            DropIndex("dbo.ClaimData", new[] { "ClaimDataFileId" });
            DropIndex("dbo.ClaimData", new[] { "ClaimDataBatchId" });
            DropIndex("dbo.ClaimCodeMappings", new[] { "UpdatedById" });
            DropIndex("dbo.ClaimCodeMappings", new[] { "CreatedById" });
            DropIndex("dbo.ClaimCodeMappings", new[] { "ClaimCodeId" });
            DropIndex("dbo.ClaimCodeMappingDetails", new[] { "CreatedById" });
            DropIndex("dbo.ClaimCodeMappingDetails", new[] { "MlreBenefitCode" });
            DropIndex("dbo.ClaimCodeMappingDetails", new[] { "MlreEventCode" });
            DropIndex("dbo.ClaimCodeMappingDetails", new[] { "Combination" });
            DropIndex("dbo.ClaimCodeMappingDetails", new[] { "ClaimCodeMappingId" });
            DropIndex("dbo.ClaimChecklists", new[] { "UpdatedById" });
            DropIndex("dbo.ClaimChecklists", new[] { "CreatedById" });
            DropIndex("dbo.ClaimChecklists", new[] { "ClaimCodeId" });
            DropIndex("dbo.ClaimChecklistDetails", new[] { "UpdatedById" });
            DropIndex("dbo.ClaimChecklistDetails", new[] { "CreatedById" });
            DropIndex("dbo.ClaimChecklistDetails", new[] { "ClaimChecklistId" });
            DropIndex("dbo.ClaimCategories", new[] { "UpdatedById" });
            DropIndex("dbo.ClaimCategories", new[] { "CreatedById" });
            DropIndex("dbo.ClaimCategories", new[] { "Category" });
            DropIndex("dbo.ClaimAuthorityLimitMLReDetails", new[] { "CreatedById" });
            DropIndex("dbo.ClaimAuthorityLimitMLReDetails", new[] { "ClaimCodeId" });
            DropIndex("dbo.ClaimAuthorityLimitMLReDetails", new[] { "ClaimAuthorityLimitMLReId" });
            DropIndex("dbo.ClaimAuthorityLimitMLRe", new[] { "UpdatedById" });
            DropIndex("dbo.ClaimAuthorityLimitMLRe", new[] { "CreatedById" });
            DropIndex("dbo.ClaimAuthorityLimitMLRe", new[] { "UserId" });
            DropIndex("dbo.ClaimAuthorityLimitMLRe", new[] { "DepartmentId" });
            DropIndex("dbo.ClaimAuthorityLimitCedants", new[] { "UpdatedById" });
            DropIndex("dbo.ClaimAuthorityLimitCedants", new[] { "CreatedById" });
            DropIndex("dbo.ClaimAuthorityLimitCedants", new[] { "Remarks" });
            DropIndex("dbo.ClaimAuthorityLimitCedants", new[] { "CedantId" });
            DropIndex("dbo.ClaimAuthorityLimitCedantDetails", new[] { "CreatedById" });
            DropIndex("dbo.ClaimAuthorityLimitCedantDetails", new[] { "ClaimCodeId" });
            DropIndex("dbo.ClaimAuthorityLimitCedantDetails", new[] { "ClaimAuthorityLimitCedantId" });
            DropIndex("dbo.CedantWorkgroupUsers", new[] { "UserId" });
            DropIndex("dbo.CedantWorkgroupUsers", new[] { "CedantWorkgroupId" });
            DropIndex("dbo.CedantWorkgroups", new[] { "UpdatedById" });
            DropIndex("dbo.CedantWorkgroups", new[] { "CreatedById" });
            DropIndex("dbo.CedantWorkgroups", new[] { "Code" });
            DropIndex("dbo.CedantWorkgroupCedants", new[] { "CedantId" });
            DropIndex("dbo.CedantWorkgroupCedants", new[] { "CedantWorkgroupId" });
            DropIndex("dbo.EventCodes", new[] { "UpdatedById" });
            DropIndex("dbo.EventCodes", new[] { "CreatedById" });
            DropIndex("dbo.EventCodes", new[] { "Description" });
            DropIndex("dbo.EventCodes", new[] { "Code" });
            DropIndex("dbo.Benefits", new[] { "ValuationBenefitCodePickListDetailId" });
            DropIndex("dbo.Benefits", new[] { "Description" });
            DropIndex("dbo.Benefits", new[] { "Type" });
            DropIndex("dbo.BenefitDetails", new[] { "UpdatedById" });
            DropIndex("dbo.BenefitDetails", new[] { "CreatedById" });
            DropIndex("dbo.BenefitDetails", new[] { "EventCodeId" });
            DropIndex("dbo.BenefitDetails", new[] { "ClaimCodeId" });
            DropIndex("dbo.BenefitDetails", new[] { "BenefitId" });
            DropIndex("dbo.AuthorizationLimits", new[] { "UpdatedById" });
            DropIndex("dbo.AuthorizationLimits", new[] { "CreatedById" });
            DropIndex("dbo.AuthorizationLimits", new[] { "Percentage" });
            DropIndex("dbo.AuthorizationLimits", new[] { "NegativeAmountTo" });
            DropIndex("dbo.AuthorizationLimits", new[] { "NegativeAmountFrom" });
            DropIndex("dbo.AuthorizationLimits", new[] { "PositiveAmountTo" });
            DropIndex("dbo.AuthorizationLimits", new[] { "PositiveAmountFrom" });
            DropIndex("dbo.AuthorizationLimits", new[] { "AccessGroupId" });
            DropIndex("dbo.Cedants", new[] { "AccountCode" });
            DropIndex("dbo.Cedants", new[] { "PartyCode" });
            DropIndex("dbo.Cedants", new[] { "CedingCompanyTypePickListDetailId" });
            DropIndex("dbo.AnnuityFactorMappings", new[] { "CreatedById" });
            DropIndex("dbo.AnnuityFactorMappings", new[] { "CedingPlanCode" });
            DropIndex("dbo.AnnuityFactorMappings", new[] { "Combination" });
            DropIndex("dbo.AnnuityFactorMappings", new[] { "AnnuityFactorId" });
            DropIndex("dbo.AnnuityFactors", new[] { "UpdatedById" });
            DropIndex("dbo.AnnuityFactors", new[] { "CreatedById" });
            DropIndex("dbo.AnnuityFactors", new[] { "CedantId" });
            DropIndex("dbo.AnnuityFactorDetails", new[] { "UpdatedById" });
            DropIndex("dbo.AnnuityFactorDetails", new[] { "CreatedById" });
            DropIndex("dbo.AnnuityFactorDetails", new[] { "AnnuityFactorValue" });
            DropIndex("dbo.AnnuityFactorDetails", new[] { "PolicyTermRemain" });
            DropIndex("dbo.AnnuityFactorDetails", new[] { "AnnuityFactorId" });
            DropIndex("dbo.StandardSoaDataOutputs", new[] { "UpdatedById" });
            DropIndex("dbo.StandardSoaDataOutputs", new[] { "CreatedById" });
            DropIndex("dbo.StandardClaimDataOutputs", new[] { "UpdatedById" });
            DropIndex("dbo.StandardClaimDataOutputs", new[] { "CreatedById" });
            DropIndex("dbo.PickLists", new[] { "StandardSoaDataOutputId" });
            DropIndex("dbo.PickLists", new[] { "StandardClaimDataOutputId" });
            DropIndex("dbo.ClaimCodes", new[] { "UpdatedById" });
            DropIndex("dbo.ClaimCodes", new[] { "CreatedById" });
            DropIndex("dbo.ClaimCodes", new[] { "Description" });
            DropIndex("dbo.ClaimCodes", new[] { "Code" });
            DropIndex("dbo.AccountCodes", new[] { "UpdatedById" });
            DropIndex("dbo.AccountCodes", new[] { "CreatedById" });
            DropIndex("dbo.AccountCodes", new[] { "ReportingType" });
            DropIndex("dbo.AccountCodes", new[] { "Code" });
            DropIndex("dbo.AccountCodeMappings", new[] { "UpdatedById" });
            DropIndex("dbo.AccountCodeMappings", new[] { "CreatedById" });
            DropIndex("dbo.AccountCodeMappings", new[] { "TransactionTypeCodePickListDetailId" });
            DropIndex("dbo.AccountCodeMappings", new[] { "AccountCodeId" });
            DropIndex("dbo.AccountCodeMappings", new[] { "ClaimCodeId" });
            DropIndex("dbo.AccountCodeMappings", new[] { "TreatyTypePickListDetailId" });
            DropPrimaryKey("dbo.Mfrs17ReportingDetailRiDatas");
            AlterColumn("dbo.RiDataComputations", "StandardOutputId", c => c.Int(nullable: false));
            AlterColumn("dbo.RateTables", "RateTableCode", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.RiData", "RiDataFileId", c => c.Int(nullable: false));
            AlterColumn("dbo.Mfrs17ReportingDetailRiDatas", "RiDataId", c => c.Int(nullable: false));
            AlterColumn("dbo.Benefits", "Description", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Benefits", "Type", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.TreatyBenefitCodeMappings", "WakalahFee");
            DropColumn("dbo.TreatyBenefitCodeMappings", "ServiceFee");
            DropColumn("dbo.TreatyBenefitCodeMappings", "RiShareCap");
            DropColumn("dbo.TreatyBenefitCodeMappings", "RiShare");
            DropColumn("dbo.TreatyBenefitCodeMappings", "RetentionCap");
            DropColumn("dbo.TreatyBenefitCodeMappings", "RetentionShare");
            DropColumn("dbo.TreatyBenefitCodeMappings", "AblAmount");
            DropColumn("dbo.TreatyBenefitCodeMappings", "MaxAar");
            DropColumn("dbo.TreatyBenefitCodeMappings", "MinAar");
            DropColumn("dbo.TreatyBenefitCodeMappings", "ApLoading");
            DropColumn("dbo.TreatyBenefitCodeMappings", "MaxUwRating");
            DropColumn("dbo.TreatyBenefitCodeMappings", "MaxIssueAge");
            DropColumn("dbo.TreatyBenefitCodeMappings", "MinIssueAge");
            DropColumn("dbo.TreatyBenefitCodeMappings", "MaxExpiryAge");
            DropColumn("dbo.TreatyBenefitCodeMappings", "ProfitComm");
            DropColumn("dbo.RiDataPreValidations", "Step");
            DropColumn("dbo.TreatyCodes", "TreatyStatusPickListDetailId");
            DropColumn("dbo.TreatyCodes", "TreatyTypePickListDetailId");
            DropColumn("dbo.TreatyCodes", "AccountFor");
            DropColumn("dbo.RiDataComputations", "Step");
            DropColumn("dbo.Remarks", "FollowUpUserId");
            DropColumn("dbo.Remarks", "FollowUpAt");
            DropColumn("dbo.Remarks", "FollowUpStatus");
            DropColumn("dbo.Remarks", "HasFollowUp");
            DropColumn("dbo.Remarks", "DepartmentId");
            DropColumn("dbo.Remarks", "IsPrivate");
            DropColumn("dbo.RateTables", "GroupDiscountId");
            DropColumn("dbo.RateTables", "LargeDiscountId");
            DropColumn("dbo.RateTables", "RiDiscountId");
            DropColumn("dbo.RateTables", "CedantId");
            DropColumn("dbo.RateTables", "RateId");
            DropColumn("dbo.RateTables", "PolicyDurationTo");
            DropColumn("dbo.RateTables", "PolicyDurationFrom");
            DropColumn("dbo.RateTables", "PolicyTermTo");
            DropColumn("dbo.RateTables", "PolicyTermFrom");
            DropColumn("dbo.Treaties", "BlockDescription");
            DropColumn("dbo.Treaties", "LineOfBusinessPickListDetailId");
            DropColumn("dbo.Treaties", "BusinessOriginPickListDetailId");
            DropColumn("dbo.RiDataBatches", "FinalisedAt");
            DropColumn("dbo.RiDataBatches", "SoaDataBatchId");
            DropColumn("dbo.RiDataBatches", "ReceivedAt");
            DropColumn("dbo.RiDataBatches", "RecordType");
            DropColumn("dbo.RiDataBatches", "TotalPostValidationFailedStatus");
            DropColumn("dbo.RiDataBatches", "TotalPostComputationFailedStatus");
            DropColumn("dbo.RiDataBatches", "TotalPreValidationFailedStatus");
            DropColumn("dbo.RiDataBatches", "TotalPreComputation2FailedStatus");
            DropColumn("dbo.RiDataBatches", "TotalPreComputation1FailedStatus");
            DropColumn("dbo.RiDataBatches", "ProcessWarehouseStatus");
            DropColumn("dbo.RiData", "AarCap2");
            DropColumn("dbo.RiData", "AarShare2");
            DropColumn("dbo.RiData", "RetroNetPremium3");
            DropColumn("dbo.RiData", "RetroNetPremium2");
            DropColumn("dbo.RiData", "RetroNetPremium1");
            DropColumn("dbo.RiData", "RetroDiscount3");
            DropColumn("dbo.RiData", "RetroDiscount2");
            DropColumn("dbo.RiData", "RetroDiscount1");
            DropColumn("dbo.RiData", "RetroReinsurancePremium3");
            DropColumn("dbo.RiData", "RetroReinsurancePremium2");
            DropColumn("dbo.RiData", "RetroReinsurancePremium1");
            DropColumn("dbo.RiData", "RetroAar3");
            DropColumn("dbo.RiData", "RetroAar2");
            DropColumn("dbo.RiData", "RetroAar1");
            DropColumn("dbo.RiData", "RetroShare3");
            DropColumn("dbo.RiData", "RetroShare2");
            DropColumn("dbo.RiData", "RetroShare1");
            DropColumn("dbo.RiData", "RetroParty3");
            DropColumn("dbo.RiData", "RetroParty2");
            DropColumn("dbo.RiData", "RetroParty1");
            DropColumn("dbo.RiData", "FlatExtraDurationCheck");
            DropColumn("dbo.RiData", "FlatExtraAmountCheck");
            DropColumn("dbo.RiData", "UwRatingCheck");
            DropColumn("dbo.RiData", "SumAssuredOfferedCheck");
            DropColumn("dbo.RiData", "ValidityDayCheck");
            DropColumn("dbo.RiData", "MlreDatabaseCommission");
            DropColumn("dbo.RiData", "MlreBrokerageFee");
            DropColumn("dbo.RiData", "ServiceFee");
            DropColumn("dbo.RiData", "ServiceFeePercentage");
            DropColumn("dbo.RiData", "NetPremiumCheck");
            DropColumn("dbo.RiData", "MlreNetPremium");
            DropColumn("dbo.RiData", "MlreTotalDiscount");
            DropColumn("dbo.RiData", "MlreVitalityDiscount");
            DropColumn("dbo.RiData", "MlreGroupSizeDiscount");
            DropColumn("dbo.RiData", "MlreLargeSaDiscount");
            DropColumn("dbo.RiData", "MlreSubstandardDiscount");
            DropColumn("dbo.RiData", "MlreStandardDiscount");
            DropColumn("dbo.RiData", "MlreGrossPremium");
            DropColumn("dbo.RiData", "MlreFlatExtraPremium");
            DropColumn("dbo.RiData", "MlreSubstandardPremium");
            DropColumn("dbo.RiData", "MlreStandardPremium");
            DropColumn("dbo.RiData", "AarCheck");
            DropColumn("dbo.RiData", "RetentionCheck");
            DropColumn("dbo.RiData", "AblCheck");
            DropColumn("dbo.RiData", "CorridorLimitCheck");
            DropColumn("dbo.RiData", "MaxAarCheck");
            DropColumn("dbo.RiData", "MinAarCheck");
            DropColumn("dbo.RiData", "EffectiveDateCheck");
            DropColumn("dbo.RiData", "ApLoadingCheck");
            DropColumn("dbo.RiData", "MaxUwRatingCheck");
            DropColumn("dbo.RiData", "MaxIssueAgeCheck");
            DropColumn("dbo.RiData", "MinIssueAgeCheck");
            DropColumn("dbo.RiData", "PolicyIssueAgeCheck");
            DropColumn("dbo.RiData", "MlrePolicyIssueAge");
            DropColumn("dbo.RiData", "MaxExpiryAgeCheck");
            DropColumn("dbo.RiData", "InsuredAttainedAgeCheck");
            DropColumn("dbo.RiData", "MlreInsuredAttainedAgeAtPreviousMonth");
            DropColumn("dbo.RiData", "MlreInsuredAttainedAgeAtCurrentMonth");
            DropColumn("dbo.RiData", "MaxApLoading");
            DropColumn("dbo.RiData", "TreatyType");
            DropColumn("dbo.RiData", "TotalDirectRetroNetPremium");
            DropColumn("dbo.RiData", "TotalDirectRetroDiscount");
            DropColumn("dbo.RiData", "TotalDirectRetroGrossPremium");
            DropColumn("dbo.RiData", "TotalDirectRetroAar");
            DropColumn("dbo.RiData", "ProfitComm");
            DropColumn("dbo.RiData", "AarShare");
            DropColumn("dbo.RiData", "RetentionShare");
            DropColumn("dbo.RiData", "EwarpActionCode");
            DropColumn("dbo.RiData", "EwarpNumber");
            DropColumn("dbo.RiData", "GroupSizeDiscount");
            DropColumn("dbo.RiData", "LargeSaDiscount");
            DropColumn("dbo.RiData", "RiDiscountRate");
            DropColumn("dbo.RiData", "RatePerBasisUnit");
            DropColumn("dbo.RiData", "Abl");
            DropColumn("dbo.RiData", "CorridorLimit");
            DropColumn("dbo.RiData", "MaxAar");
            DropColumn("dbo.RiData", "MinAar");
            DropColumn("dbo.RiData", "MaxIssueAge");
            DropColumn("dbo.RiData", "MinIssueAge");
            DropColumn("dbo.RiData", "MaxExpiryAge");
            DropColumn("dbo.RiData", "Mfrs17AnnualCohort");
            DropColumn("dbo.RiData", "RiskPeriodEndDate");
            DropColumn("dbo.RiData", "RiskPeriodStartDate");
            DropColumn("dbo.RiData", "OfferLetterSentDate");
            DropColumn("dbo.RiData", "EffectiveDate");
            DropColumn("dbo.RiData", "FlatExtraDuration");
            DropColumn("dbo.RiData", "FlatExtraAmountOffered");
            DropColumn("dbo.RiData", "UwRatingOffered");
            DropColumn("dbo.RiData", "SumAssuredOffered");
            DropColumn("dbo.RiData", "AnnuityFactor");
            DropColumn("dbo.RiData", "RiRate2");
            DropColumn("dbo.RiData", "RiRate");
            DropColumn("dbo.RiData", "AarCap");
            DropColumn("dbo.RiData", "RetentionCap");
            DropColumn("dbo.RiData", "MaxUwRating");
            DropColumn("dbo.RiData", "BrokerageFee");
            DropColumn("dbo.RiData", "OriginalDiscount");
            DropColumn("dbo.RiData", "TransactionDiscount");
            DropColumn("dbo.RiData", "OriginalPremium");
            DropColumn("dbo.RiData", "TransactionPremium");
            DropColumn("dbo.RiData", "Layer1FlatExtraPremium");
            DropColumn("dbo.RiData", "NetPremiumAlt");
            DropColumn("dbo.RiData", "GrossPremiumAlt");
            DropColumn("dbo.RiData", "DatabaseCommision");
            DropColumn("dbo.RiData", "SurrenderValue");
            DropColumn("dbo.RiData", "NoClaimBonus");
            DropColumn("dbo.RiData", "CurrencyRate");
            DropColumn("dbo.RiData", "ProcessWarehouseStatus");
            DropColumn("dbo.RiData", "PostValidationStatus");
            DropColumn("dbo.RiData", "PostComputationStatus");
            DropColumn("dbo.RiData", "PreComputation2Status");
            DropColumn("dbo.RiData", "PreComputation1Status");
            DropColumn("dbo.RiData", "OriginalEntryId");
            DropColumn("dbo.RiData", "RecordType");
            DropColumn("dbo.Mfrs17Reportings", "CutOffId");
            DropColumn("dbo.Mfrs17Reportings", "GenerateModifiedOnly");
            DropColumn("dbo.Mfrs17ReportingDetails", "IsModified");
            DropColumn("dbo.Mfrs17ReportingDetails", "Status");
            DropColumn("dbo.Mfrs17ReportingDetailRiDatas", "RiDataWarehouseHistoryId");
            DropColumn("dbo.Mfrs17ReportingDetailRiDatas", "RiDataWarehouseId");
            DropColumn("dbo.PickLists", "IsEditable");
            DropColumn("dbo.PickLists", "StandardSoaDataOutputId");
            DropColumn("dbo.PickLists", "StandardClaimDataOutputId");
            DropColumn("dbo.Mfrs17CellMappings", "ProfitComm");
            DropColumn("dbo.Mfrs17CellMappings", "LoaCode");
            DropColumn("dbo.Documents", "DepartmentId");
            DropColumn("dbo.Documents", "IsPrivate");
            DropColumn("dbo.Documents", "RemarkId");
            DropColumn("dbo.Cedants", "AccountCode");
            DropColumn("dbo.Cedants", "PartyCode");
            DropColumn("dbo.Cedants", "CedingCompanyTypePickListDetailId");
            DropColumn("dbo.Benefits", "GST");
            DropColumn("dbo.Benefits", "ValuationBenefitCodePickListDetailId");
            DropColumn("dbo.Benefits", "EffectiveEndDate");
            DropColumn("dbo.Benefits", "EffectiveStartDate");
            DropColumn("dbo.Modules", "Index");
            DropTable("dbo.UnderwritingRemarks");
            DropTable("dbo.TreatyDiscountTableDetails");
            DropTable("dbo.SoaDataRiDataSummaries");
            DropTable("dbo.SoaDataPostValidations");
            DropTable("dbo.SoaDataPostValidationDifferences");
            DropTable("dbo.SoaDataCompiledSummaries");
            DropTable("dbo.SoaDataBatchStatusFiles");
            DropTable("dbo.SoaDataFiles");
            DropTable("dbo.SoaData");
            DropTable("dbo.Salutations");
            DropTable("dbo.RetroSummaries");
            DropTable("dbo.RetroStatements");
            DropTable("dbo.RetroRegister");
            DropTable("dbo.RetroRegisterHistories");
            DropTable("dbo.RetroRegisterBatchFiles");
            DropTable("dbo.RetroRegisterBatches");
            DropTable("dbo.RetroRegisterBatchDirectRetros");
            DropTable("dbo.ReferralClaims");
            DropTable("dbo.RiDiscounts");
            DropTable("dbo.Rates");
            DropTable("dbo.RateDetails");
            DropTable("dbo.PublicHolidays");
            DropTable("dbo.PublicHolidayDetails");
            DropTable("dbo.PremiumSpreadTableDetails");
            DropTable("dbo.RiDataWarehouseHistories");
            DropTable("dbo.LargeDiscounts");
            DropTable("dbo.ItemCodes");
            DropTable("dbo.ItemCodeMappings");
            DropTable("dbo.InvoiceRegisterValuations");
            DropTable("dbo.InvoiceRegister");
            DropTable("dbo.InvoiceRegisterHistories");
            DropTable("dbo.InvoiceRegisterBatchSoaDatas");
            DropTable("dbo.InvoiceRegisterBatchRemarks");
            DropTable("dbo.InvoiceRegisterBatchRemarkDocuments");
            DropTable("dbo.InvoiceRegisterBatchFiles");
            DropTable("dbo.InvoiceRegisterBatches");
            DropTable("dbo.GroupDiscounts");
            DropTable("dbo.FinanceProvisioningTransactions");
            DropTable("dbo.FacMasterListings");
            DropTable("dbo.FacMasterListingDetails");
            DropTable("dbo.EventClaimCodeMappings");
            DropTable("dbo.EventClaimCodeMappingDetails");
            DropTable("dbo.DiscountTables");
            DropTable("dbo.DirectRetroStatusFiles");
            DropTable("dbo.FinanceProvisionings");
            DropTable("dbo.DirectRetroProvisioningTransactions");
            DropTable("dbo.TreatyDiscountTables");
            DropTable("dbo.RetroParties");
            DropTable("dbo.PremiumSpreadTables");
            DropTable("dbo.DirectRetroConfigurationMappings");
            DropTable("dbo.DirectRetroConfigurations");
            DropTable("dbo.DirectRetroConfigurationDetails");
            DropTable("dbo.DirectRetro");
            DropTable("dbo.CutOff");
            DropTable("dbo.ClaimRegisterHistories");
            DropTable("dbo.RiDataWarehouse");
            DropTable("dbo.ReferralRiDataFiles");
            DropTable("dbo.ReferralRiData");
            DropTable("dbo.ClaimRegister");
            DropTable("dbo.ClaimReasons");
            DropTable("dbo.ClaimDataValidations");
            DropTable("dbo.ClaimDataMappings");
            DropTable("dbo.ClaimDataMappingDetails");
            DropTable("dbo.ClaimDataComputations");
            DropTable("dbo.ClaimDataBatchStatusFiles");
            DropTable("dbo.ClaimDataFiles");
            DropTable("dbo.SoaDataBatches");
            DropTable("dbo.ClaimDataConfigs");
            DropTable("dbo.ClaimDataBatches");
            DropTable("dbo.ClaimData");
            DropTable("dbo.ClaimCodeMappings");
            DropTable("dbo.ClaimCodeMappingDetails");
            DropTable("dbo.ClaimChecklists");
            DropTable("dbo.ClaimChecklistDetails");
            DropTable("dbo.ClaimCategories");
            DropTable("dbo.ClaimAuthorityLimitMLReDetails");
            DropTable("dbo.ClaimAuthorityLimitMLRe");
            DropTable("dbo.ClaimAuthorityLimitCedants");
            DropTable("dbo.ClaimAuthorityLimitCedantDetails");
            DropTable("dbo.CedantWorkgroupUsers");
            DropTable("dbo.CedantWorkgroups");
            DropTable("dbo.CedantWorkgroupCedants");
            DropTable("dbo.EventCodes");
            DropTable("dbo.BenefitDetails");
            DropTable("dbo.AuthorizationLimits");
            DropTable("dbo.AnnuityFactorMappings");
            DropTable("dbo.AnnuityFactors");
            DropTable("dbo.AnnuityFactorDetails");
            DropTable("dbo.StandardSoaDataOutputs");
            DropTable("dbo.StandardClaimDataOutputs");
            DropTable("dbo.ClaimCodes");
            DropTable("dbo.AccountCodes");
            DropTable("dbo.AccountCodeMappings");
            AddPrimaryKey("dbo.Mfrs17ReportingDetailRiDatas", new[] { "Mfrs17ReportingDetailId", "RiDataId" });
            CreateIndex("dbo.RiDataComputations", "StandardOutputId");
            CreateIndex("dbo.RateTables", "CedingTobaccoUsePickListDetailId");
            CreateIndex("dbo.RateTables", "InsuredGenderCodePickListDetailId");
            CreateIndex("dbo.RateTables", "CedingOccupationCodePickListDetailId");
            CreateIndex("dbo.RateTables", "RateTableCode");
            CreateIndex("dbo.RiData", "ComputationStatus");
            CreateIndex("dbo.RiData", "RiDataFileId");
            CreateIndex("dbo.Mfrs17ReportingDetailRiDatas", "RiDataId");
            CreateIndex("dbo.Benefits", "Description");
            CreateIndex("dbo.Benefits", "Type");
            AddForeignKey("dbo.RateTables", "InsuredGenderCodePickListDetailId", "dbo.PickListDetails", "Id");
            AddForeignKey("dbo.RateTables", "CedingTobaccoUsePickListDetailId", "dbo.PickListDetails", "Id");
            AddForeignKey("dbo.RateTables", "CedingOccupationCodePickListDetailId", "dbo.PickListDetails", "Id");
            AddForeignKey("dbo.Mfrs17ReportingDetailRiDatas", "RiDataId", "dbo.RiData", "Id");
        }
    }
}
