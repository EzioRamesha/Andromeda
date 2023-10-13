namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CreateSoaDataHistoryTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SoaDataHistories",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CutOffId = c.Int(nullable: false),
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
                    RiskPremium = c.Double(),
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
                .ForeignKey("dbo.CutOff", t => t.CutOffId)
                .Index(t => t.CutOffId)
                .Index(t => t.SoaDataBatchId)
                .Index(t => t.SoaDataFileId)
                .Index(t => t.MappingStatus)
                .Index(t => t.BusinessCode)
                .Index(t => t.TreatyCode)
                .Index(t => t.SoaQuarter)
                .Index(t => t.RiskQuarter);

        }

        public override void Down()
        {
            DropForeignKey("dbo.SoaDataHistories", "CutOffId", "dbo.CutOff");
            DropIndex("dbo.SoaDataHistories", new[] { "RiskQuarter" });
            DropIndex("dbo.SoaDataHistories", new[] { "SoaQuarter" });
            DropIndex("dbo.SoaDataHistories", new[] { "TreatyCode" });
            DropIndex("dbo.SoaDataHistories", new[] { "BusinessCode" });
            DropIndex("dbo.SoaDataHistories", new[] { "MappingStatus" });
            DropIndex("dbo.SoaDataHistories", new[] { "SoaDataFileId" });
            DropIndex("dbo.SoaDataHistories", new[] { "SoaDataBatchId" });
            DropIndex("dbo.SoaDataHistories", new[] { "CutOffId" });
            DropTable("dbo.SoaDataHistories");
        }
    }
}
