namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CreatePerLifeSoaReportTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PerLifeRetroStatements",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PerLifeSoaId = c.Int(nullable: false),
                    RetroPartyId = c.Int(nullable: false),
                    Status = c.Int(nullable: false),
                    MlreRef = c.String(maxLength: 128),
                    CedingCompany = c.String(maxLength: 255),
                    TreatyCode = c.String(maxLength: 35),
                    TreatyNo = c.String(maxLength: 128),
                    Schedule = c.String(maxLength: 20),
                    TreatyType = c.String(maxLength: 128),
                    FromMlreTo = c.String(maxLength: 128),
                    AccountsFor = c.String(storeType: "ntext"),
                    DateReportCompleted = c.DateTime(precision: 7, storeType: "datetime2"),
                    DateSendToRetro = c.DateTime(precision: 7, storeType: "datetime2"),
                    AccountingPeriod = c.String(maxLength: 10),
                    ReserveCededBegin = c.Double(),
                    ReserveCededEnd = c.Double(),
                    RiskChargeCededBegin = c.Double(),
                    RiskChargeCededEnd = c.Double(),
                    AverageReserveCeded = c.Double(),
                    RiPremiumNB = c.Double(),
                    RiPremiumRN = c.Double(),
                    RiPremiumALT = c.Double(),
                    QuarterlyRiskPremium = c.Double(),
                    RetrocessionMarketingFee = c.Double(),
                    RiDiscountNB = c.Double(),
                    RiDiscountRN = c.Double(),
                    RiDiscountALT = c.Double(),
                    AgreedDatabaseComm = c.Double(),
                    GstPayable = c.Double(),
                    NoClaimBonus = c.Double(),
                    Claims = c.Double(),
                    ProfitComm = c.Double(),
                    SurrenderValue = c.Double(),
                    PaymentToTheReinsurer = c.Double(),
                    TotalNoOfPolicyNB = c.Int(),
                    TotalNoOfPolicyRN = c.Int(),
                    TotalNoOfPolicyALT = c.Int(),
                    TotalSumReinsuredNB = c.Double(),
                    TotalSumReinsuredRN = c.Double(),
                    TotalSumReinsuredALT = c.Double(),
                    AccountingPeriod2 = c.String(maxLength: 10),
                    ReserveCededBegin2 = c.Double(),
                    ReserveCededEnd2 = c.Double(),
                    RiskChargeCededBegin2 = c.Double(),
                    RiskChargeCededEnd2 = c.Double(),
                    AverageReserveCeded2 = c.Double(),
                    RiPremiumNB2 = c.Double(),
                    RiPremiumRN2 = c.Double(),
                    RiPremiumALT2 = c.Double(),
                    QuarterlyRiskPremium2 = c.Double(),
                    RetrocessionMarketingFee2 = c.Double(),
                    RiDiscountNB2 = c.Double(),
                    RiDiscountRN2 = c.Double(),
                    RiDiscountALT2 = c.Double(),
                    AgreedDatabaseComm2 = c.Double(),
                    GstPayable2 = c.Double(),
                    NoClaimBonus2 = c.Double(),
                    Claims2 = c.Double(),
                    ProfitComm2 = c.Double(),
                    SurrenderValue2 = c.Double(),
                    PaymentToTheReinsurer2 = c.Double(),
                    TotalNoOfPolicyNB2 = c.Int(),
                    TotalNoOfPolicyRN2 = c.Int(),
                    TotalNoOfPolicyALT2 = c.Int(),
                    TotalSumReinsuredNB2 = c.Double(),
                    TotalSumReinsuredRN2 = c.Double(),
                    TotalSumReinsuredALT2 = c.Double(),
                    AccountingPeriod3 = c.String(maxLength: 10),
                    ReserveCededBegin3 = c.Double(),
                    ReserveCededEnd3 = c.Double(),
                    RiskChargeCededBegin3 = c.Double(),
                    RiskChargeCededEnd3 = c.Double(),
                    AverageReserveCeded3 = c.Double(),
                    RiPremiumNB3 = c.Double(),
                    RiPremiumRN3 = c.Double(),
                    RiPremiumALT3 = c.Double(),
                    QuarterlyRiskPremium3 = c.Double(),
                    RetrocessionMarketingFee3 = c.Double(),
                    RiDiscountNB3 = c.Double(),
                    RiDiscountRN3 = c.Double(),
                    RiDiscountALT3 = c.Double(),
                    AgreedDatabaseComm3 = c.Double(),
                    GstPayable3 = c.Double(),
                    NoClaimBonus3 = c.Double(),
                    Claims3 = c.Double(),
                    ProfitComm3 = c.Double(),
                    SurrenderValue3 = c.Double(),
                    PaymentToTheReinsurer3 = c.Double(),
                    TotalNoOfPolicyNB3 = c.Int(),
                    TotalNoOfPolicyRN3 = c.Int(),
                    TotalNoOfPolicyALT3 = c.Int(),
                    TotalSumReinsuredNB3 = c.Double(),
                    TotalSumReinsuredRN3 = c.Double(),
                    TotalSumReinsuredALT3 = c.Double(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PerLifeSoa", t => t.PerLifeSoaId)
                .ForeignKey("dbo.RetroParties", t => t.RetroPartyId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.PerLifeSoaId)
                .Index(t => t.RetroPartyId)
                .Index(t => t.Status)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.PerLifeSoaSummaries",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PerLifeSoaId = c.Int(nullable: false),
                    RowLabel = c.String(maxLength: 255),
                    Automatic = c.Double(),
                    Facultative = c.Double(),
                    Advantage = c.Double(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PerLifeSoa", t => t.PerLifeSoaId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.PerLifeSoaId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

        }

        public override void Down()
        {
            DropForeignKey("dbo.PerLifeSoaSummaries", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeSoaSummaries", "PerLifeSoaId", "dbo.PerLifeSoa");
            DropForeignKey("dbo.PerLifeSoaSummaries", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeRetroStatements", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeRetroStatements", "RetroPartyId", "dbo.RetroParties");
            DropForeignKey("dbo.PerLifeRetroStatements", "PerLifeSoaId", "dbo.PerLifeSoa");
            DropForeignKey("dbo.PerLifeRetroStatements", "CreatedById", "dbo.Users");
            DropIndex("dbo.PerLifeSoaSummaries", new[] { "UpdatedById" });
            DropIndex("dbo.PerLifeSoaSummaries", new[] { "CreatedById" });
            DropIndex("dbo.PerLifeSoaSummaries", new[] { "PerLifeSoaId" });
            DropIndex("dbo.PerLifeRetroStatements", new[] { "UpdatedById" });
            DropIndex("dbo.PerLifeRetroStatements", new[] { "CreatedById" });
            DropIndex("dbo.PerLifeRetroStatements", new[] { "Status" });
            DropIndex("dbo.PerLifeRetroStatements", new[] { "RetroPartyId" });
            DropIndex("dbo.PerLifeRetroStatements", new[] { "PerLifeSoaId" });
            DropTable("dbo.PerLifeSoaSummaries");
            DropTable("dbo.PerLifeRetroStatements");
        }
    }
}
