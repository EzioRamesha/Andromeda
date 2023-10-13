namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class InitDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccessGroups",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DepartmentId = c.Int(nullable: false),
                    Code = c.String(nullable: false, maxLength: 20),
                    Name = c.String(nullable: false, maxLength: 64),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.DepartmentId)
                .Index(t => t.Code)
                .Index(t => t.Name)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.Users",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DepartmentId = c.Int(),
                    Status = c.Int(nullable: false),
                    LoginMethod = c.Int(nullable: false),
                    UserName = c.String(nullable: false, maxLength: 256),
                    Email = c.String(maxLength: 256),
                    EmailConfirmed = c.Boolean(nullable: false),
                    PasswordHash = c.String(),
                    FullName = c.String(maxLength: 128),
                    SecurityStamp = c.String(),
                    LockoutEnabled = c.Boolean(nullable: false),
                    LockoutEndAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    AccessFailedCount = c.Int(nullable: false),
                    SessionId = c.String(),
                    LastLoginAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    PasswordExpiresAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.DepartmentId)
                .Index(t => t.UserName, name: "UserNameIndex")
                .Index(t => t.Email)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.UserClaims",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserId = c.Int(nullable: false),
                    ClaimType = c.String(),
                    ClaimValue = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.Departments",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Code = c.String(nullable: false, maxLength: 20),
                    Name = c.String(nullable: false, maxLength: 64),
                    HodUserId = c.Int(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.HodUserId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Code)
                .Index(t => t.Name)
                .Index(t => t.HodUserId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.UserLogins",
                c => new
                {
                    LoginProvider = c.String(nullable: false, maxLength: 128),
                    ProviderKey = c.String(nullable: false, maxLength: 128),
                    UserId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.UserRoles",
                c => new
                {
                    UserId = c.Int(nullable: false),
                    RoleId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);

            CreateTable(
                "dbo.UserAccessGroups",
                c => new
                {
                    UserId = c.Int(nullable: false),
                    AccessGroupId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.UserId, t.AccessGroupId })
                .ForeignKey("dbo.AccessGroups", t => t.AccessGroupId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.AccessGroupId);

            CreateTable(
                "dbo.AccessMatrices",
                c => new
                {
                    AccessGroupId = c.Int(nullable: false),
                    ModuleId = c.Int(nullable: false),
                    Power = c.String(maxLength: 128),
                })
                .PrimaryKey(t => new { t.AccessGroupId, t.ModuleId })
                .ForeignKey("dbo.AccessGroups", t => t.AccessGroupId)
                .ForeignKey("dbo.Modules", t => t.ModuleId)
                .Index(t => t.AccessGroupId)
                .Index(t => t.ModuleId)
                .Index(t => t.Power);

            CreateTable(
                "dbo.Modules",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DepartmentId = c.Int(nullable: false),
                    Type = c.Int(nullable: false),
                    Controller = c.String(nullable: false, maxLength: 64),
                    Power = c.String(maxLength: 32),
                    PowerAdditional = c.String(maxLength: 128),
                    Editable = c.Boolean(nullable: false),
                    Name = c.String(nullable: false, maxLength: 64),
                    ReportPath = c.String(maxLength: 64),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.DepartmentId)
                .Index(t => t.Type)
                .Index(t => t.Controller)
                .Index(t => t.Name)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.Benefits",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Type = c.String(nullable: false, maxLength: 128),
                    Code = c.String(nullable: false, maxLength: 20),
                    Description = c.String(nullable: false, maxLength: 128),
                    Status = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Type)
                .Index(t => t.Code)
                .Index(t => t.Description)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.Cedants",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Type = c.Int(nullable: false),
                    Name = c.String(nullable: false, maxLength: 255),
                    Code = c.String(nullable: false, maxLength: 10),
                    Status = c.Int(nullable: false),
                    Remarks = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Name)
                .Index(t => t.Code)
                .Index(t => t.Remarks)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.Documents",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ModuleId = c.Int(nullable: false),
                    ObjectId = c.Int(nullable: false),
                    Type = c.Int(nullable: false),
                    FileName = c.String(maxLength: 255),
                    HashFileName = c.String(maxLength: 255),
                    Description = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Modules", t => t.ModuleId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.ModuleId)
                .Index(t => t.ObjectId)
                .Index(t => t.FileName)
                .Index(t => t.HashFileName)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.Mfrs17CellMappingDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Mfrs17CellMappingId = c.Int(nullable: false),
                    Combination = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Mfrs17CellMappings", t => t.Mfrs17CellMappingId)
                .Index(t => t.Mfrs17CellMappingId)
                .Index(t => t.Combination)
                .Index(t => t.CreatedById);

            CreateTable(
                "dbo.Mfrs17CellMappings",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyCodeId = c.Int(nullable: false),
                    ReinsBasisCodePickListDetailId = c.Int(nullable: false),
                    ReinsEffDatePolStartDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    ReinsEffDatePolEndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    CedingPlanCode = c.String(maxLength: 255),
                    BenefitCode = c.String(maxLength: 255),
                    BasicRiderPickListDetailId = c.Int(nullable: false),
                    CellName = c.String(nullable: false, maxLength: 64),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PickListDetails", t => t.BasicRiderPickListDetailId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickListDetails", t => t.ReinsBasisCodePickListDetailId)
                .ForeignKey("dbo.TreatyCodes", t => t.TreatyCodeId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyCodeId)
                .Index(t => t.ReinsBasisCodePickListDetailId)
                .Index(t => t.ReinsEffDatePolStartDate)
                .Index(t => t.ReinsEffDatePolEndDate)
                .Index(t => t.CedingPlanCode)
                .Index(t => t.BenefitCode)
                .Index(t => t.BasicRiderPickListDetailId)
                .Index(t => t.CellName)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.PickListDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PickListId = c.Int(nullable: false),
                    SortIndex = c.Int(nullable: false),
                    Code = c.String(maxLength: 64),
                    Description = c.String(nullable: false, maxLength: 128),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickLists", t => t.PickListId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.PickListId)
                .Index(t => t.Code)
                .Index(t => t.Description)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.PickLists",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DepartmentId = c.Int(nullable: false),
                    StandardOutputId = c.Int(),
                    FieldName = c.String(nullable: false, maxLength: 128),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .ForeignKey("dbo.StandardOutputs", t => t.StandardOutputId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.DepartmentId)
                .Index(t => t.StandardOutputId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.StandardOutputs",
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
                "dbo.TreatyCodes",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyId = c.Int(nullable: false),
                    Code = c.String(nullable: false, maxLength: 30),
                    OldTreatyCodeId = c.Int(nullable: false),
                    OldTreatyCode = c.String(maxLength: 30),
                    Description = c.String(maxLength: 255),
                    Status = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Treaties", t => t.TreatyId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyId)
                .Index(t => t.Code)
                .Index(t => t.OldTreatyCode)
                .Index(t => t.Description)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.Treaties",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatyIdCode = c.String(nullable: false, maxLength: 30),
                    CedantId = c.Int(nullable: false),
                    Description = c.String(maxLength: 255),
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
                .Index(t => t.TreatyIdCode)
                .Index(t => t.CedantId)
                .Index(t => t.Description)
                .Index(t => t.Remarks)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.Mfrs17ReportingDetailRiDatas",
                c => new
                {
                    Mfrs17ReportingDetailId = c.Int(nullable: false),
                    RiDataId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.Mfrs17ReportingDetailId, t.RiDataId })
                .ForeignKey("dbo.Mfrs17ReportingDetails", t => t.Mfrs17ReportingDetailId)
                .ForeignKey("dbo.RiData", t => t.RiDataId)
                .Index(t => t.Mfrs17ReportingDetailId)
                .Index(t => t.RiDataId);

            CreateTable(
                "dbo.Mfrs17ReportingDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Mfrs17ReportingId = c.Int(nullable: false),
                    CedantId = c.Int(nullable: false),
                    TreatyCodeId = c.Int(nullable: false),
                    PremiumFrequencyCodePickListDetailId = c.Int(nullable: false),
                    LatestDataStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    LatestDataEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    Record = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cedants", t => t.CedantId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Mfrs17Reportings", t => t.Mfrs17ReportingId)
                .ForeignKey("dbo.PickListDetails", t => t.PremiumFrequencyCodePickListDetailId)
                .ForeignKey("dbo.TreatyCodes", t => t.TreatyCodeId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Mfrs17ReportingId)
                .Index(t => t.CedantId)
                .Index(t => t.TreatyCodeId)
                .Index(t => t.PremiumFrequencyCodePickListDetailId)
                .Index(t => t.Record)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.Mfrs17Reportings",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Quarter = c.String(nullable: false, maxLength: 64),
                    Status = c.Int(nullable: false),
                    TotalRecord = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Quarter)
                .Index(t => t.Status)
                .Index(t => t.TotalRecord)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RiData",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    RiDataBatchId = c.Int(nullable: false),
                    RiDataFileId = c.Int(nullable: false),
                    MappingStatus = c.Int(nullable: false),
                    ComputationStatus = c.Int(nullable: false),
                    PreValidationStatus = c.Int(nullable: false),
                    FinaliseStatus = c.Int(nullable: false),
                    Errors = c.String(storeType: "ntext"),
                    CustomField = c.String(storeType: "ntext"),
                    TreatyCode = c.String(maxLength: 35),
                    ReinsBasisCode = c.String(maxLength: 30),
                    FundsAccountingTypeCode = c.String(maxLength: 30),
                    PremiumFrequencyCode = c.String(maxLength: 10),
                    ReportPeriodMonth = c.Int(),
                    ReportPeriodYear = c.Int(),
                    RiskPeriodMonth = c.Int(),
                    RiskPeriodYear = c.Int(),
                    TransactionTypeCode = c.String(maxLength: 2),
                    PolicyNumber = c.String(maxLength: 30),
                    IssueDatePol = c.DateTime(precision: 7, storeType: "datetime2"),
                    IssueDateBen = c.DateTime(precision: 7, storeType: "datetime2"),
                    ReinsEffDatePol = c.DateTime(precision: 7, storeType: "datetime2"),
                    ReinsEffDateBen = c.DateTime(precision: 7, storeType: "datetime2"),
                    CedingPlanCode = c.String(maxLength: 30),
                    CedingBenefitTypeCode = c.String(maxLength: 30),
                    CedingBenefitRiskCode = c.String(maxLength: 10),
                    MlreBenefitCode = c.String(maxLength: 10),
                    OriSumAssured = c.Int(),
                    CurrSumAssured = c.Int(),
                    AmountCededB4MlreShare = c.Int(),
                    RetentionAmount = c.Int(),
                    AarOri = c.Int(),
                    Aar = c.Int(),
                    AarSpecial1 = c.Int(),
                    AarSpecial2 = c.Int(),
                    AarSpecial3 = c.Int(),
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
                    DurationYear = c.Int(),
                    DurationDay = c.Int(),
                    DurationMonth = c.Int(),
                    PremiumCalType = c.String(maxLength: 5),
                    CedantRiRate = c.Int(),
                    RateTable = c.String(maxLength: 50),
                    AgeRatedUp = c.Int(),
                    DiscountRate = c.Int(),
                    LoadingType = c.String(maxLength: 10),
                    UnderwriterRating = c.Int(),
                    UnderwriterRatingUnit = c.Int(),
                    UnderwriterRatingTerm = c.Int(),
                    UnderwriterRating2 = c.Int(),
                    UnderwriterRatingUnit2 = c.Int(),
                    UnderwriterRatingTerm2 = c.Int(),
                    UnderwriterRating3 = c.Int(),
                    UnderwriterRatingUnit3 = c.Int(),
                    UnderwriterRatingTerm3 = c.Int(),
                    FlatExtraAmount = c.Int(),
                    FlatExtraUnit = c.Int(),
                    FlatExtraTerm = c.Int(),
                    FlatExtraAmount2 = c.Int(),
                    FlatExtraTerm2 = c.Int(),
                    StandardPremium = c.Int(),
                    SubstandardPremium = c.Int(),
                    FlatExtraPremium = c.Int(),
                    GrossPremium = c.Int(),
                    StandardDiscount = c.Int(),
                    SubstandardDiscount = c.Int(),
                    VitalityDiscount = c.Int(),
                    TotalDiscount = c.Int(),
                    NetPremium = c.Int(),
                    AnnualRiPrem = c.Int(),
                    RiCovPeriod = c.Int(),
                    AdjBeginDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    AdjEndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    PolicyNumberOld = c.String(maxLength: 30),
                    PolicyStatusCode = c.String(maxLength: 20),
                    PolicyGrossPremium = c.Int(),
                    PolicyStandardPremium = c.Int(),
                    PolicySubstandardPremium = c.Int(),
                    PolicyTermRemain = c.Int(),
                    PolicyAmountDeath = c.Int(),
                    PolicyReserve = c.Int(),
                    PolicyPaymentMethod = c.String(maxLength: 10),
                    PolicyLifeNumber = c.Int(),
                    FundCode = c.String(maxLength: 5),
                    LineOfBusiness = c.String(maxLength: 5),
                    ApLoading = c.Int(),
                    LoanInterestRate = c.Int(),
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
                    CedantSar = c.Int(),
                    CedantReinsurerCode = c.String(maxLength: 10),
                    AmountCededB4MlreShare2 = c.Int(),
                    CessionCode = c.String(maxLength: 10),
                    CedantRemark = c.String(maxLength: 255),
                    GroupPolicyNumber = c.String(maxLength: 30),
                    GroupPolicyName = c.String(maxLength: 128),
                    NoOfEmployee = c.Int(),
                    PolicyTotalLive = c.Int(),
                    GroupSubsidiaryName = c.String(maxLength: 128),
                    GroupSubsidiaryNo = c.String(maxLength: 30),
                    GroupEmployeeBasicSalary = c.Int(),
                    GroupEmployeeJobType = c.String(maxLength: 10),
                    GroupEmployeeJobCode = c.String(maxLength: 10),
                    GroupEmployeeBasicSalaryRevise = c.Int(),
                    GroupEmployeeBasicSalaryMultiplier = c.Int(),
                    CedingPlanCode2 = c.String(maxLength: 10),
                    DependantIndicator = c.String(maxLength: 2),
                    GhsRoomBoard = c.Int(),
                    PolicyAmountSubstandard = c.Int(),
                    Layer1RiShare = c.Int(),
                    Layer1InsuredAttainedAge = c.Int(),
                    Layer1InsuredAttainedAge2nd = c.Int(),
                    Layer1StandardPremium = c.Int(),
                    Layer1SubstandardPremium = c.Int(),
                    Layer1GrossPremium = c.Int(),
                    Layer1StandardDiscount = c.Int(),
                    Layer1SubstandardDiscount = c.Int(),
                    Layer1TotalDiscount = c.Int(),
                    Layer1NetPremium = c.Int(),
                    Layer1GrossPremiumAlt = c.Int(),
                    Layer1TotalDiscountAlt = c.Int(),
                    Layer1NetPremiumAlt = c.Int(),
                    SpecialIndicator1 = c.String(),
                    SpecialIndicator2 = c.String(),
                    SpecialIndicator3 = c.String(),
                    IndicatorJointLife = c.String(maxLength: 1),
                    TaxAmount = c.Int(),
                    GstIndicator = c.String(maxLength: 3),
                    GstGrossPremium = c.Int(),
                    GstTotalDiscount = c.Int(),
                    GstVitality = c.Int(),
                    GstAmount = c.Int(),
                    Mfrs17BasicRider = c.String(maxLength: 5),
                    Mfrs17CellName = c.String(maxLength: 30),
                    Mfrs17TreatyCode = c.String(maxLength: 10),
                    LoaCode = c.String(maxLength: 10),
                    TempD1 = c.DateTime(precision: 7, storeType: "datetime2"),
                    TempD2 = c.DateTime(precision: 7, storeType: "datetime2"),
                    TempD3 = c.DateTime(precision: 7, storeType: "datetime2"),
                    TempD4 = c.DateTime(precision: 7, storeType: "datetime2"),
                    TempD5 = c.DateTime(precision: 7, storeType: "datetime2"),
                    TempS1 = c.String(maxLength: 50),
                    TempS2 = c.String(maxLength: 50),
                    TempS3 = c.String(maxLength: 50),
                    TempS4 = c.String(maxLength: 50),
                    TempS5 = c.String(maxLength: 50),
                    TempI1 = c.Int(),
                    TempI2 = c.Int(),
                    TempI3 = c.Int(),
                    TempI4 = c.Int(),
                    TempI5 = c.Int(),
                    TempA1 = c.Int(),
                    TempA2 = c.Int(),
                    TempA3 = c.Int(),
                    TempA4 = c.Int(),
                    TempA5 = c.Int(),
                    TempA6 = c.Int(),
                    TempA7 = c.Int(),
                    TempA8 = c.Int(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.RiDataBatches", t => t.RiDataBatchId)
                .ForeignKey("dbo.RiDataFiles", t => t.RiDataFileId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.RiDataBatchId)
                .Index(t => t.RiDataFileId)
                .Index(t => t.MappingStatus)
                .Index(t => t.ComputationStatus)
                .Index(t => t.PreValidationStatus)
                .Index(t => t.FinaliseStatus)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RiDataBatches",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CedantId = c.Int(nullable: false),
                    TreatyCodeId = c.Int(),
                    RiDataConfigId = c.Int(nullable: false),
                    Configs = c.String(storeType: "ntext"),
                    OverrideProperties = c.String(storeType: "ntext"),
                    Status = c.Int(nullable: false),
                    Quarter = c.String(maxLength: 64),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cedants", t => t.CedantId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.RiDataConfigs", t => t.RiDataConfigId)
                .ForeignKey("dbo.TreatyCodes", t => t.TreatyCodeId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.CedantId)
                .Index(t => t.TreatyCodeId)
                .Index(t => t.RiDataConfigId)
                .Index(t => t.Status)
                .Index(t => t.Quarter)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RiDataConfigs",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CedantId = c.Int(nullable: false),
                    TreatyCodeId = c.Int(),
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
                .ForeignKey("dbo.TreatyCodes", t => t.TreatyCodeId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.CedantId)
                .Index(t => t.TreatyCodeId)
                .Index(t => t.Code)
                .Index(t => t.Name)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RiDataFiles",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    RiDataBatchId = c.Int(nullable: false),
                    RawFileId = c.Int(nullable: false),
                    TreatyCodeId = c.Int(),
                    RiDataConfigId = c.Int(),
                    Configs = c.String(storeType: "ntext"),
                    OverrideProperties = c.String(storeType: "ntext"),
                    Mode = c.Int(nullable: false),
                    Status = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.RawFiles", t => t.RawFileId)
                .ForeignKey("dbo.RiDataBatches", t => t.RiDataBatchId)
                .ForeignKey("dbo.RiDataConfigs", t => t.RiDataConfigId)
                .ForeignKey("dbo.TreatyCodes", t => t.TreatyCodeId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.RiDataBatchId)
                .Index(t => t.RawFileId)
                .Index(t => t.TreatyCodeId)
                .Index(t => t.RiDataConfigId)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RawFiles",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Type = c.Int(nullable: false),
                    Status = c.Int(nullable: false),
                    FileName = c.String(maxLength: 255),
                    HashFileName = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.FileName)
                .Index(t => t.HashFileName)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RateTableDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    RateTableId = c.Int(nullable: false),
                    Combination = c.String(nullable: false, maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById, cascadeDelete: true)
                .ForeignKey("dbo.RateTables", t => t.RateTableId)
                .Index(t => t.RateTableId)
                .Index(t => t.Combination)
                .Index(t => t.CreatedById);

            CreateTable(
                "dbo.RateTables",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    RateTableCode = c.String(nullable: false, maxLength: 64),
                    TreatyCode = c.String(nullable: false, maxLength: 255),
                    BenefitId = c.Int(),
                    CedingPlanCode = c.String(maxLength: 255),
                    CedingOccupationCodePickListDetailId = c.Int(),
                    PremiumFrequencyCodePickListDetailId = c.Int(),
                    InsuredGenderCodePickListDetailId = c.Int(),
                    CedingTobaccoUsePickListDetailId = c.Int(),
                    PolicyAmountFrom = c.Int(),
                    PolicyAmountTo = c.Int(),
                    AttainedAgeFrom = c.Int(),
                    AttainedAgeTo = c.Int(),
                    ReinsEffDatePolStartDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    ReinsEffDatePolEndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Benefits", t => t.BenefitId)
                .ForeignKey("dbo.PickListDetails", t => t.CedingOccupationCodePickListDetailId)
                .ForeignKey("dbo.PickListDetails", t => t.CedingTobaccoUsePickListDetailId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickListDetails", t => t.InsuredGenderCodePickListDetailId)
                .ForeignKey("dbo.PickListDetails", t => t.PremiumFrequencyCodePickListDetailId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.RateTableCode)
                .Index(t => t.TreatyCode)
                .Index(t => t.BenefitId)
                .Index(t => t.CedingPlanCode)
                .Index(t => t.CedingOccupationCodePickListDetailId)
                .Index(t => t.PremiumFrequencyCodePickListDetailId)
                .Index(t => t.InsuredGenderCodePickListDetailId)
                .Index(t => t.CedingTobaccoUsePickListDetailId)
                .Index(t => t.PolicyAmountFrom)
                .Index(t => t.PolicyAmountTo)
                .Index(t => t.AttainedAgeFrom)
                .Index(t => t.AttainedAgeTo)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.Remarks",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ModuleId = c.Int(nullable: false),
                    ObjectId = c.Int(nullable: false),
                    Status = c.Int(nullable: false),
                    Content = c.String(nullable: false, maxLength: 128),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Modules", t => t.ModuleId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.ModuleId)
                .Index(t => t.ObjectId)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RiDataBatchStatusFiles",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    RiDataBatchId = c.Int(nullable: false),
                    StatusHistoryId = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.RiDataBatches", t => t.RiDataBatchId)
                .ForeignKey("dbo.StatusHistories", t => t.StatusHistoryId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.RiDataBatchId)
                .Index(t => t.StatusHistoryId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.StatusHistories",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ModuleId = c.Int(nullable: false),
                    ObjectId = c.Int(nullable: false),
                    Status = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Modules", t => t.ModuleId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.ModuleId)
                .Index(t => t.ObjectId)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RiDataComputations",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    RiDataConfigId = c.Int(nullable: false),
                    SortIndex = c.Int(nullable: false),
                    Description = c.String(nullable: false, maxLength: 128),
                    Condition = c.String(nullable: false, unicode: false, storeType: "text"),
                    StandardOutputId = c.Int(nullable: false),
                    Mode = c.Int(nullable: false),
                    CalculationFormula = c.String(nullable: false, maxLength: 128),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.RiDataConfigs", t => t.RiDataConfigId)
                .ForeignKey("dbo.StandardOutputs", t => t.StandardOutputId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.RiDataConfigId)
                .Index(t => t.Description)
                .Index(t => t.StandardOutputId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RiDataCorrections",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CedantId = c.Int(nullable: false),
                    TreatyCodeId = c.Int(),
                    PolicyNumber = c.String(nullable: false, maxLength: 30),
                    InsuredRegisterNo = c.String(maxLength: 30),
                    InsuredGenderCodePickListDetailId = c.Int(nullable: false),
                    InsuredDateOfBirth = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    InsuredName = c.String(nullable: false, maxLength: 128),
                    CampaignCode = c.String(maxLength: 10),
                    ReinsBasisCodePickListDetailId = c.Int(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cedants", t => t.CedantId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickListDetails", t => t.InsuredGenderCodePickListDetailId)
                .ForeignKey("dbo.PickListDetails", t => t.ReinsBasisCodePickListDetailId)
                .ForeignKey("dbo.TreatyCodes", t => t.TreatyCodeId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.CedantId)
                .Index(t => t.TreatyCodeId)
                .Index(t => t.InsuredGenderCodePickListDetailId)
                .Index(t => t.ReinsBasisCodePickListDetailId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RiDataMappingDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    RiDataMappingId = c.Int(nullable: false),
                    StandardOutputId = c.Int(nullable: false),
                    PickListDetailId = c.Int(nullable: false),
                    RawValue = c.String(nullable: false, maxLength: 128),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickListDetails", t => t.PickListDetailId)
                .ForeignKey("dbo.RiDataMappings", t => t.RiDataMappingId)
                .ForeignKey("dbo.StandardOutputs", t => t.StandardOutputId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.RiDataMappingId)
                .Index(t => t.StandardOutputId)
                .Index(t => t.PickListDetailId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RiDataMappings",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    RiDataConfigId = c.Int(nullable: false),
                    StandardOutputId = c.Int(nullable: false),
                    SortIndex = c.Int(nullable: false),
                    Row = c.Int(nullable: false),
                    RawColumnName = c.String(maxLength: 128),
                    Length = c.Int(),
                    TransformFormula = c.Int(nullable: false),
                    DefaultValue = c.String(maxLength: 128),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.RiDataConfigs", t => t.RiDataConfigId)
                .ForeignKey("dbo.StandardOutputs", t => t.StandardOutputId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.RiDataConfigId)
                .Index(t => t.StandardOutputId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.RiDataPreValidations",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    RiDataConfigId = c.Int(nullable: false),
                    SortIndex = c.Int(nullable: false),
                    Description = c.String(nullable: false, maxLength: 255),
                    Condition = c.String(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.RiDataConfigs", t => t.RiDataConfigId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.RiDataConfigId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.Roles",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 256),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");

            CreateTable(
                "dbo.TreatyBenefitCodeMappingDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Type = c.Int(nullable: false),
                    TreatyBenefitCodeMappingId = c.Int(nullable: false),
                    Combination = c.String(nullable: false, maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyBenefitCodeMappings", t => t.TreatyBenefitCodeMappingId)
                .Index(t => t.Type)
                .Index(t => t.TreatyBenefitCodeMappingId)
                .Index(t => t.Combination)
                .Index(t => t.CreatedById);

            CreateTable(
                "dbo.TreatyBenefitCodeMappings",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CedantId = c.Int(nullable: false),
                    TreatyCodeId = c.Int(nullable: false),
                    BenefitId = c.Int(nullable: false),
                    CedingPlanCode = c.String(nullable: false, maxLength: 255),
                    Description = c.String(),
                    CedingBenefitTypeCode = c.String(nullable: false, maxLength: 255),
                    CedingBenefitRiskCode = c.String(maxLength: 255),
                    CedingTreatyCode = c.String(maxLength: 255),
                    CampaignCode = c.String(maxLength: 255),
                    ReinsEffDatePolStartDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    ReinsEffDatePolEndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    ReinsBasisCodePickListDetailId = c.Int(),
                    AttainedAgeFrom = c.Int(),
                    AttainedAgeTo = c.Int(),
                    ReportingStartDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    ReportingEndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Benefits", t => t.BenefitId)
                .ForeignKey("dbo.Cedants", t => t.CedantId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PickListDetails", t => t.ReinsBasisCodePickListDetailId)
                .ForeignKey("dbo.TreatyCodes", t => t.TreatyCodeId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.CedantId)
                .Index(t => t.TreatyCodeId)
                .Index(t => t.BenefitId)
                .Index(t => t.CedingPlanCode)
                .Index(t => t.CedingBenefitTypeCode)
                .Index(t => t.CedingBenefitRiskCode)
                .Index(t => t.CedingTreatyCode)
                .Index(t => t.CampaignCode)
                .Index(t => t.ReinsBasisCodePickListDetailId)
                .Index(t => t.AttainedAgeFrom)
                .Index(t => t.AttainedAgeTo)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.UserPasswords",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserId = c.Int(nullable: false),
                    PasswordHash = c.String(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.UserTrails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Type = c.Int(nullable: false),
                    Controller = c.String(nullable: false, maxLength: 64),
                    ObjectId = c.Int(nullable: false),
                    Description = c.String(maxLength: 128),
                    IpAddress = c.String(maxLength: 128),
                    Data = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Type)
                .Index(t => t.Controller)
                .Index(t => t.ObjectId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.UserPasswords", "UserId", "dbo.Users");
            DropForeignKey("dbo.TreatyBenefitCodeMappingDetails", "TreatyBenefitCodeMappingId", "dbo.TreatyBenefitCodeMappings");
            DropForeignKey("dbo.TreatyBenefitCodeMappings", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyBenefitCodeMappings", "TreatyCodeId", "dbo.TreatyCodes");
            DropForeignKey("dbo.TreatyBenefitCodeMappings", "ReinsBasisCodePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.TreatyBenefitCodeMappings", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyBenefitCodeMappings", "CedantId", "dbo.Cedants");
            DropForeignKey("dbo.TreatyBenefitCodeMappings", "BenefitId", "dbo.Benefits");
            DropForeignKey("dbo.TreatyBenefitCodeMappingDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RiDataPreValidations", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RiDataPreValidations", "RiDataConfigId", "dbo.RiDataConfigs");
            DropForeignKey("dbo.RiDataPreValidations", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RiDataMappingDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RiDataMappingDetails", "StandardOutputId", "dbo.StandardOutputs");
            DropForeignKey("dbo.RiDataMappingDetails", "RiDataMappingId", "dbo.RiDataMappings");
            DropForeignKey("dbo.RiDataMappings", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RiDataMappings", "StandardOutputId", "dbo.StandardOutputs");
            DropForeignKey("dbo.RiDataMappings", "RiDataConfigId", "dbo.RiDataConfigs");
            DropForeignKey("dbo.RiDataMappings", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RiDataMappingDetails", "PickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.RiDataMappingDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RiDataCorrections", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RiDataCorrections", "TreatyCodeId", "dbo.TreatyCodes");
            DropForeignKey("dbo.RiDataCorrections", "ReinsBasisCodePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.RiDataCorrections", "InsuredGenderCodePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.RiDataCorrections", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RiDataCorrections", "CedantId", "dbo.Cedants");
            DropForeignKey("dbo.RiDataComputations", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RiDataComputations", "StandardOutputId", "dbo.StandardOutputs");
            DropForeignKey("dbo.RiDataComputations", "RiDataConfigId", "dbo.RiDataConfigs");
            DropForeignKey("dbo.RiDataComputations", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RiDataBatchStatusFiles", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RiDataBatchStatusFiles", "StatusHistoryId", "dbo.StatusHistories");
            DropForeignKey("dbo.StatusHistories", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.StatusHistories", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.StatusHistories", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RiDataBatchStatusFiles", "RiDataBatchId", "dbo.RiDataBatches");
            DropForeignKey("dbo.RiDataBatchStatusFiles", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Remarks", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.Remarks", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.Remarks", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RateTableDetails", "RateTableId", "dbo.RateTables");
            DropForeignKey("dbo.RateTables", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RateTables", "PremiumFrequencyCodePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.RateTables", "InsuredGenderCodePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.RateTables", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RateTables", "CedingTobaccoUsePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.RateTables", "CedingOccupationCodePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.RateTables", "BenefitId", "dbo.Benefits");
            DropForeignKey("dbo.RateTableDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Mfrs17ReportingDetailRiDatas", "RiDataId", "dbo.RiData");
            DropForeignKey("dbo.RiData", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RiData", "RiDataFileId", "dbo.RiDataFiles");
            DropForeignKey("dbo.RiDataFiles", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RiDataFiles", "TreatyCodeId", "dbo.TreatyCodes");
            DropForeignKey("dbo.RiDataFiles", "RiDataConfigId", "dbo.RiDataConfigs");
            DropForeignKey("dbo.RiDataFiles", "RiDataBatchId", "dbo.RiDataBatches");
            DropForeignKey("dbo.RiDataFiles", "RawFileId", "dbo.RawFiles");
            DropForeignKey("dbo.RawFiles", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RawFiles", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RiDataFiles", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RiData", "RiDataBatchId", "dbo.RiDataBatches");
            DropForeignKey("dbo.RiDataBatches", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RiDataBatches", "TreatyCodeId", "dbo.TreatyCodes");
            DropForeignKey("dbo.RiDataBatches", "RiDataConfigId", "dbo.RiDataConfigs");
            DropForeignKey("dbo.RiDataConfigs", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RiDataConfigs", "TreatyCodeId", "dbo.TreatyCodes");
            DropForeignKey("dbo.RiDataConfigs", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RiDataConfigs", "CedantId", "dbo.Cedants");
            DropForeignKey("dbo.RiDataBatches", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RiDataBatches", "CedantId", "dbo.Cedants");
            DropForeignKey("dbo.RiData", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Mfrs17ReportingDetailRiDatas", "Mfrs17ReportingDetailId", "dbo.Mfrs17ReportingDetails");
            DropForeignKey("dbo.Mfrs17ReportingDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.Mfrs17ReportingDetails", "TreatyCodeId", "dbo.TreatyCodes");
            DropForeignKey("dbo.Mfrs17ReportingDetails", "PremiumFrequencyCodePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.Mfrs17ReportingDetails", "Mfrs17ReportingId", "dbo.Mfrs17Reportings");
            DropForeignKey("dbo.Mfrs17Reportings", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.Mfrs17Reportings", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Mfrs17ReportingDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Mfrs17ReportingDetails", "CedantId", "dbo.Cedants");
            DropForeignKey("dbo.Mfrs17CellMappingDetails", "Mfrs17CellMappingId", "dbo.Mfrs17CellMappings");
            DropForeignKey("dbo.Mfrs17CellMappings", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.Mfrs17CellMappings", "TreatyCodeId", "dbo.TreatyCodes");
            DropForeignKey("dbo.TreatyCodes", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyCodes", "TreatyId", "dbo.Treaties");
            DropForeignKey("dbo.Treaties", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.Treaties", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Treaties", "CedantId", "dbo.Cedants");
            DropForeignKey("dbo.TreatyCodes", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Mfrs17CellMappings", "ReinsBasisCodePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.Mfrs17CellMappings", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Mfrs17CellMappings", "BasicRiderPickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.PickListDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PickListDetails", "PickListId", "dbo.PickLists");
            DropForeignKey("dbo.PickLists", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PickLists", "StandardOutputId", "dbo.StandardOutputs");
            DropForeignKey("dbo.StandardOutputs", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.StandardOutputs", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.PickLists", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.PickLists", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.PickListDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Mfrs17CellMappingDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Documents", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.Documents", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.Documents", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Cedants", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.Cedants", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Benefits", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.Benefits", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.AccessMatrices", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.Modules", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.Modules", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Modules", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.AccessMatrices", "AccessGroupId", "dbo.AccessGroups");
            DropForeignKey("dbo.AccessGroups", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.AccessGroups", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.AccessGroups", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.UserAccessGroups", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserAccessGroups", "AccessGroupId", "dbo.AccessGroups");
            DropForeignKey("dbo.Users", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Departments", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.Departments", "HodUserId", "dbo.Users");
            DropForeignKey("dbo.Departments", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Users", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropIndex("dbo.UserTrails", new[] { "ObjectId" });
            DropIndex("dbo.UserTrails", new[] { "Controller" });
            DropIndex("dbo.UserTrails", new[] { "Type" });
            DropIndex("dbo.UserPasswords", new[] { "UserId" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "CreatedById" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "AttainedAgeTo" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "AttainedAgeFrom" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "ReinsBasisCodePickListDetailId" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "CampaignCode" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "CedingTreatyCode" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "CedingBenefitRiskCode" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "CedingBenefitTypeCode" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "CedingPlanCode" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "BenefitId" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "TreatyCodeId" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "CedantId" });
            DropIndex("dbo.TreatyBenefitCodeMappingDetails", new[] { "CreatedById" });
            DropIndex("dbo.TreatyBenefitCodeMappingDetails", new[] { "Combination" });
            DropIndex("dbo.TreatyBenefitCodeMappingDetails", new[] { "TreatyBenefitCodeMappingId" });
            DropIndex("dbo.TreatyBenefitCodeMappingDetails", new[] { "Type" });
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.RiDataPreValidations", new[] { "UpdatedById" });
            DropIndex("dbo.RiDataPreValidations", new[] { "CreatedById" });
            DropIndex("dbo.RiDataPreValidations", new[] { "RiDataConfigId" });
            DropIndex("dbo.RiDataMappings", new[] { "UpdatedById" });
            DropIndex("dbo.RiDataMappings", new[] { "CreatedById" });
            DropIndex("dbo.RiDataMappings", new[] { "StandardOutputId" });
            DropIndex("dbo.RiDataMappings", new[] { "RiDataConfigId" });
            DropIndex("dbo.RiDataMappingDetails", new[] { "UpdatedById" });
            DropIndex("dbo.RiDataMappingDetails", new[] { "CreatedById" });
            DropIndex("dbo.RiDataMappingDetails", new[] { "PickListDetailId" });
            DropIndex("dbo.RiDataMappingDetails", new[] { "StandardOutputId" });
            DropIndex("dbo.RiDataMappingDetails", new[] { "RiDataMappingId" });
            DropIndex("dbo.RiDataCorrections", new[] { "UpdatedById" });
            DropIndex("dbo.RiDataCorrections", new[] { "CreatedById" });
            DropIndex("dbo.RiDataCorrections", new[] { "ReinsBasisCodePickListDetailId" });
            DropIndex("dbo.RiDataCorrections", new[] { "InsuredGenderCodePickListDetailId" });
            DropIndex("dbo.RiDataCorrections", new[] { "TreatyCodeId" });
            DropIndex("dbo.RiDataCorrections", new[] { "CedantId" });
            DropIndex("dbo.RiDataComputations", new[] { "UpdatedById" });
            DropIndex("dbo.RiDataComputations", new[] { "CreatedById" });
            DropIndex("dbo.RiDataComputations", new[] { "StandardOutputId" });
            DropIndex("dbo.RiDataComputations", new[] { "Description" });
            DropIndex("dbo.RiDataComputations", new[] { "RiDataConfigId" });
            DropIndex("dbo.StatusHistories", new[] { "UpdatedById" });
            DropIndex("dbo.StatusHistories", new[] { "CreatedById" });
            DropIndex("dbo.StatusHistories", new[] { "Status" });
            DropIndex("dbo.StatusHistories", new[] { "ObjectId" });
            DropIndex("dbo.StatusHistories", new[] { "ModuleId" });
            DropIndex("dbo.RiDataBatchStatusFiles", new[] { "UpdatedById" });
            DropIndex("dbo.RiDataBatchStatusFiles", new[] { "CreatedById" });
            DropIndex("dbo.RiDataBatchStatusFiles", new[] { "StatusHistoryId" });
            DropIndex("dbo.RiDataBatchStatusFiles", new[] { "RiDataBatchId" });
            DropIndex("dbo.Remarks", new[] { "UpdatedById" });
            DropIndex("dbo.Remarks", new[] { "CreatedById" });
            DropIndex("dbo.Remarks", new[] { "Status" });
            DropIndex("dbo.Remarks", new[] { "ObjectId" });
            DropIndex("dbo.Remarks", new[] { "ModuleId" });
            DropIndex("dbo.RateTables", new[] { "UpdatedById" });
            DropIndex("dbo.RateTables", new[] { "CreatedById" });
            DropIndex("dbo.RateTables", new[] { "AttainedAgeTo" });
            DropIndex("dbo.RateTables", new[] { "AttainedAgeFrom" });
            DropIndex("dbo.RateTables", new[] { "PolicyAmountTo" });
            DropIndex("dbo.RateTables", new[] { "PolicyAmountFrom" });
            DropIndex("dbo.RateTables", new[] { "CedingTobaccoUsePickListDetailId" });
            DropIndex("dbo.RateTables", new[] { "InsuredGenderCodePickListDetailId" });
            DropIndex("dbo.RateTables", new[] { "PremiumFrequencyCodePickListDetailId" });
            DropIndex("dbo.RateTables", new[] { "CedingOccupationCodePickListDetailId" });
            DropIndex("dbo.RateTables", new[] { "CedingPlanCode" });
            DropIndex("dbo.RateTables", new[] { "BenefitId" });
            DropIndex("dbo.RateTables", new[] { "TreatyCode" });
            DropIndex("dbo.RateTables", new[] { "RateTableCode" });
            DropIndex("dbo.RateTableDetails", new[] { "CreatedById" });
            DropIndex("dbo.RateTableDetails", new[] { "Combination" });
            DropIndex("dbo.RateTableDetails", new[] { "RateTableId" });
            DropIndex("dbo.RawFiles", new[] { "UpdatedById" });
            DropIndex("dbo.RawFiles", new[] { "CreatedById" });
            DropIndex("dbo.RawFiles", new[] { "HashFileName" });
            DropIndex("dbo.RawFiles", new[] { "FileName" });
            DropIndex("dbo.RiDataFiles", new[] { "UpdatedById" });
            DropIndex("dbo.RiDataFiles", new[] { "CreatedById" });
            DropIndex("dbo.RiDataFiles", new[] { "Status" });
            DropIndex("dbo.RiDataFiles", new[] { "RiDataConfigId" });
            DropIndex("dbo.RiDataFiles", new[] { "TreatyCodeId" });
            DropIndex("dbo.RiDataFiles", new[] { "RawFileId" });
            DropIndex("dbo.RiDataFiles", new[] { "RiDataBatchId" });
            DropIndex("dbo.RiDataConfigs", new[] { "UpdatedById" });
            DropIndex("dbo.RiDataConfigs", new[] { "CreatedById" });
            DropIndex("dbo.RiDataConfigs", new[] { "Name" });
            DropIndex("dbo.RiDataConfigs", new[] { "Code" });
            DropIndex("dbo.RiDataConfigs", new[] { "TreatyCodeId" });
            DropIndex("dbo.RiDataConfigs", new[] { "CedantId" });
            DropIndex("dbo.RiDataBatches", new[] { "UpdatedById" });
            DropIndex("dbo.RiDataBatches", new[] { "CreatedById" });
            DropIndex("dbo.RiDataBatches", new[] { "Quarter" });
            DropIndex("dbo.RiDataBatches", new[] { "Status" });
            DropIndex("dbo.RiDataBatches", new[] { "RiDataConfigId" });
            DropIndex("dbo.RiDataBatches", new[] { "TreatyCodeId" });
            DropIndex("dbo.RiDataBatches", new[] { "CedantId" });
            DropIndex("dbo.RiData", new[] { "UpdatedById" });
            DropIndex("dbo.RiData", new[] { "CreatedById" });
            DropIndex("dbo.RiData", new[] { "FinaliseStatus" });
            DropIndex("dbo.RiData", new[] { "PreValidationStatus" });
            DropIndex("dbo.RiData", new[] { "ComputationStatus" });
            DropIndex("dbo.RiData", new[] { "MappingStatus" });
            DropIndex("dbo.RiData", new[] { "RiDataFileId" });
            DropIndex("dbo.RiData", new[] { "RiDataBatchId" });
            DropIndex("dbo.Mfrs17Reportings", new[] { "UpdatedById" });
            DropIndex("dbo.Mfrs17Reportings", new[] { "CreatedById" });
            DropIndex("dbo.Mfrs17Reportings", new[] { "TotalRecord" });
            DropIndex("dbo.Mfrs17Reportings", new[] { "Status" });
            DropIndex("dbo.Mfrs17Reportings", new[] { "Quarter" });
            DropIndex("dbo.Mfrs17ReportingDetails", new[] { "UpdatedById" });
            DropIndex("dbo.Mfrs17ReportingDetails", new[] { "CreatedById" });
            DropIndex("dbo.Mfrs17ReportingDetails", new[] { "Record" });
            DropIndex("dbo.Mfrs17ReportingDetails", new[] { "PremiumFrequencyCodePickListDetailId" });
            DropIndex("dbo.Mfrs17ReportingDetails", new[] { "TreatyCodeId" });
            DropIndex("dbo.Mfrs17ReportingDetails", new[] { "CedantId" });
            DropIndex("dbo.Mfrs17ReportingDetails", new[] { "Mfrs17ReportingId" });
            DropIndex("dbo.Mfrs17ReportingDetailRiDatas", new[] { "RiDataId" });
            DropIndex("dbo.Mfrs17ReportingDetailRiDatas", new[] { "Mfrs17ReportingDetailId" });
            DropIndex("dbo.Treaties", new[] { "UpdatedById" });
            DropIndex("dbo.Treaties", new[] { "CreatedById" });
            DropIndex("dbo.Treaties", new[] { "Remarks" });
            DropIndex("dbo.Treaties", new[] { "Description" });
            DropIndex("dbo.Treaties", new[] { "CedantId" });
            DropIndex("dbo.Treaties", new[] { "TreatyIdCode" });
            DropIndex("dbo.TreatyCodes", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyCodes", new[] { "CreatedById" });
            DropIndex("dbo.TreatyCodes", new[] { "Description" });
            DropIndex("dbo.TreatyCodes", new[] { "OldTreatyCode" });
            DropIndex("dbo.TreatyCodes", new[] { "Code" });
            DropIndex("dbo.TreatyCodes", new[] { "TreatyId" });
            DropIndex("dbo.StandardOutputs", new[] { "UpdatedById" });
            DropIndex("dbo.StandardOutputs", new[] { "CreatedById" });
            DropIndex("dbo.PickLists", new[] { "UpdatedById" });
            DropIndex("dbo.PickLists", new[] { "CreatedById" });
            DropIndex("dbo.PickLists", new[] { "StandardOutputId" });
            DropIndex("dbo.PickLists", new[] { "DepartmentId" });
            DropIndex("dbo.PickListDetails", new[] { "UpdatedById" });
            DropIndex("dbo.PickListDetails", new[] { "CreatedById" });
            DropIndex("dbo.PickListDetails", new[] { "Description" });
            DropIndex("dbo.PickListDetails", new[] { "Code" });
            DropIndex("dbo.PickListDetails", new[] { "PickListId" });
            DropIndex("dbo.Mfrs17CellMappings", new[] { "UpdatedById" });
            DropIndex("dbo.Mfrs17CellMappings", new[] { "CreatedById" });
            DropIndex("dbo.Mfrs17CellMappings", new[] { "CellName" });
            DropIndex("dbo.Mfrs17CellMappings", new[] { "BasicRiderPickListDetailId" });
            DropIndex("dbo.Mfrs17CellMappings", new[] { "BenefitCode" });
            DropIndex("dbo.Mfrs17CellMappings", new[] { "CedingPlanCode" });
            DropIndex("dbo.Mfrs17CellMappings", new[] { "ReinsEffDatePolEndDate" });
            DropIndex("dbo.Mfrs17CellMappings", new[] { "ReinsEffDatePolStartDate" });
            DropIndex("dbo.Mfrs17CellMappings", new[] { "ReinsBasisCodePickListDetailId" });
            DropIndex("dbo.Mfrs17CellMappings", new[] { "TreatyCodeId" });
            DropIndex("dbo.Mfrs17CellMappingDetails", new[] { "CreatedById" });
            DropIndex("dbo.Mfrs17CellMappingDetails", new[] { "Combination" });
            DropIndex("dbo.Mfrs17CellMappingDetails", new[] { "Mfrs17CellMappingId" });
            DropIndex("dbo.Documents", new[] { "UpdatedById" });
            DropIndex("dbo.Documents", new[] { "CreatedById" });
            DropIndex("dbo.Documents", new[] { "HashFileName" });
            DropIndex("dbo.Documents", new[] { "FileName" });
            DropIndex("dbo.Documents", new[] { "ObjectId" });
            DropIndex("dbo.Documents", new[] { "ModuleId" });
            DropIndex("dbo.Cedants", new[] { "UpdatedById" });
            DropIndex("dbo.Cedants", new[] { "CreatedById" });
            DropIndex("dbo.Cedants", new[] { "Remarks" });
            DropIndex("dbo.Cedants", new[] { "Code" });
            DropIndex("dbo.Cedants", new[] { "Name" });
            DropIndex("dbo.Benefits", new[] { "UpdatedById" });
            DropIndex("dbo.Benefits", new[] { "CreatedById" });
            DropIndex("dbo.Benefits", new[] { "Status" });
            DropIndex("dbo.Benefits", new[] { "Description" });
            DropIndex("dbo.Benefits", new[] { "Code" });
            DropIndex("dbo.Benefits", new[] { "Type" });
            DropIndex("dbo.Modules", new[] { "UpdatedById" });
            DropIndex("dbo.Modules", new[] { "CreatedById" });
            DropIndex("dbo.Modules", new[] { "Name" });
            DropIndex("dbo.Modules", new[] { "Controller" });
            DropIndex("dbo.Modules", new[] { "Type" });
            DropIndex("dbo.Modules", new[] { "DepartmentId" });
            DropIndex("dbo.AccessMatrices", new[] { "Power" });
            DropIndex("dbo.AccessMatrices", new[] { "ModuleId" });
            DropIndex("dbo.AccessMatrices", new[] { "AccessGroupId" });
            DropIndex("dbo.UserAccessGroups", new[] { "AccessGroupId" });
            DropIndex("dbo.UserAccessGroups", new[] { "UserId" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.Departments", new[] { "UpdatedById" });
            DropIndex("dbo.Departments", new[] { "CreatedById" });
            DropIndex("dbo.Departments", new[] { "HodUserId" });
            DropIndex("dbo.Departments", new[] { "Name" });
            DropIndex("dbo.Departments", new[] { "Code" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "UpdatedById" });
            DropIndex("dbo.Users", new[] { "CreatedById" });
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.Users", new[] { "DepartmentId" });
            DropIndex("dbo.AccessGroups", new[] { "UpdatedById" });
            DropIndex("dbo.AccessGroups", new[] { "CreatedById" });
            DropIndex("dbo.AccessGroups", new[] { "Name" });
            DropIndex("dbo.AccessGroups", new[] { "Code" });
            DropIndex("dbo.AccessGroups", new[] { "DepartmentId" });
            DropTable("dbo.UserTrails");
            DropTable("dbo.UserPasswords");
            DropTable("dbo.TreatyBenefitCodeMappings");
            DropTable("dbo.TreatyBenefitCodeMappingDetails");
            DropTable("dbo.Roles");
            DropTable("dbo.RiDataPreValidations");
            DropTable("dbo.RiDataMappings");
            DropTable("dbo.RiDataMappingDetails");
            DropTable("dbo.RiDataCorrections");
            DropTable("dbo.RiDataComputations");
            DropTable("dbo.StatusHistories");
            DropTable("dbo.RiDataBatchStatusFiles");
            DropTable("dbo.Remarks");
            DropTable("dbo.RateTables");
            DropTable("dbo.RateTableDetails");
            DropTable("dbo.RawFiles");
            DropTable("dbo.RiDataFiles");
            DropTable("dbo.RiDataConfigs");
            DropTable("dbo.RiDataBatches");
            DropTable("dbo.RiData");
            DropTable("dbo.Mfrs17Reportings");
            DropTable("dbo.Mfrs17ReportingDetails");
            DropTable("dbo.Mfrs17ReportingDetailRiDatas");
            DropTable("dbo.Treaties");
            DropTable("dbo.TreatyCodes");
            DropTable("dbo.StandardOutputs");
            DropTable("dbo.PickLists");
            DropTable("dbo.PickListDetails");
            DropTable("dbo.Mfrs17CellMappings");
            DropTable("dbo.Mfrs17CellMappingDetails");
            DropTable("dbo.Documents");
            DropTable("dbo.Cedants");
            DropTable("dbo.Benefits");
            DropTable("dbo.Modules");
            DropTable("dbo.AccessMatrices");
            DropTable("dbo.UserAccessGroups");
            DropTable("dbo.UserRoles");
            DropTable("dbo.UserLogins");
            DropTable("dbo.Departments");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.AccessGroups");
        }
    }
}
