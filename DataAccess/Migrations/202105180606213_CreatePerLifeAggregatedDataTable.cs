namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CreatePerLifeAggregatedDataTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PerLifeAggregatedData",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PerLifeAggregationDetailId = c.Int(nullable: false),
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
                    AarOri = c.Double(),
                    Aar = c.Double(),
                    InsuredName = c.String(maxLength: 128),
                    InsuredGenderCode = c.String(maxLength: 1),
                    InsuredTobaccoUse = c.String(maxLength: 1),
                    InsuredDateOfBirth = c.DateTime(precision: 7, storeType: "datetime2"),
                    InsuredOccupationCode = c.String(maxLength: 5),
                    InsuredRegisterNo = c.String(maxLength: 30),
                    InsuredAttainedAge = c.Int(),
                    InsuredNewIcNumber = c.String(maxLength: 15),
                    InsuredOldIcNumber = c.String(maxLength: 15),
                    ReinsuranceIssueAge = c.Int(),
                    PolicyTerm = c.Double(),
                    PolicyExpiryDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    LoadingType = c.String(maxLength: 15),
                    UnderwriterRating = c.Double(),
                    FlatExtraAmount = c.Double(),
                    StandardPremium = c.Double(),
                    SubstandardPremium = c.Double(),
                    FlatExtraPremium = c.Double(),
                    GrossPremium = c.Double(),
                    StandardDiscount = c.Double(),
                    SubstandardDiscount = c.Double(),
                    NetPremium = c.Double(),
                    PolicyNumberOld = c.String(maxLength: 150),
                    PolicyLifeNumber = c.Int(),
                    FundCode = c.String(maxLength: 25),
                    RiderNumber = c.Int(),
                    CampaignCode = c.String(maxLength: 10),
                    Nationality = c.String(maxLength: 20),
                    TerritoryOfIssueCode = c.String(maxLength: 20),
                    CurrencyCode = c.String(maxLength: 3),
                    StaffPlanIndicator = c.String(maxLength: 1),
                    CedingPlanCodeOld = c.String(maxLength: 30),
                    CedingBasicPlanCode = c.String(maxLength: 30),
                    GroupPolicyNumber = c.String(maxLength: 30),
                    GroupPolicyName = c.String(maxLength: 128),
                    GroupSubsidiaryName = c.String(maxLength: 128),
                    GroupSubsidiaryNo = c.String(maxLength: 30),
                    CedingPlanCode2 = c.String(maxLength: 30),
                    DependantIndicator = c.String(maxLength: 2),
                    Mfrs17BasicRider = c.String(maxLength: 5),
                    Mfrs17CellName = c.String(maxLength: 50),
                    Mfrs17ContractCode = c.String(maxLength: 25),
                    LoaCode = c.String(maxLength: 20),
                    RiskPeriodStartDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    RiskPeriodEndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    Mfrs17AnnualCohort = c.Int(),
                    BrokerageFee = c.Double(),
                    ApLoading = c.Double(),
                    EffectiveDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    AnnuityFactor = c.Double(),
                    EndingPolicyStatus = c.String(maxLength: 20),
                    LastUpdatedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    TreatyType = c.String(maxLength: 20),
                    TreatyNumber = c.String(maxLength: 128),
                    RetroPremFreq = c.String(maxLength: 10),
                    LifeBenefitFlag = c.Boolean(nullable: false),
                    RiskQuarter = c.String(maxLength: 64),
                    ProcessingDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    UniqueKeyPerLife = c.String(maxLength: 150),
                    RetroBenefitCode = c.String(maxLength: 10),
                    RetroRatio = c.Double(),
                    AccumulativeRetainAmount = c.Double(),
                    RetroRetainAmount = c.Double(),
                    RetroAmount = c.Double(),
                    RetroGrossPremium = c.Double(),
                    RetroNetPremium = c.Double(),
                    RetroDiscount = c.Double(),
                    RetroExtraPremium = c.Double(),
                    RetroExtraComm = c.Double(),
                    RetroGst = c.Double(),
                    RetroTreaty = c.String(maxLength: 50),
                    RetroClaimId = c.String(maxLength: 30),
                    Soa = c.String(maxLength: 150),
                    RetroIndicator = c.Boolean(nullable: false),
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
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

        }

        public override void Down()
        {
            DropForeignKey("dbo.PerLifeAggregatedData", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeAggregatedData", "PerLifeAggregationDetailId", "dbo.PerLifeAggregationDetails");
            DropForeignKey("dbo.PerLifeAggregatedData", "CreatedById", "dbo.Users");
            DropIndex("dbo.PerLifeAggregatedData", new[] { "UpdatedById" });
            DropIndex("dbo.PerLifeAggregatedData", new[] { "CreatedById" });
            DropIndex("dbo.PerLifeAggregatedData", new[] { "PerLifeAggregationDetailId" });
            DropTable("dbo.PerLifeAggregatedData");
        }
    }
}
