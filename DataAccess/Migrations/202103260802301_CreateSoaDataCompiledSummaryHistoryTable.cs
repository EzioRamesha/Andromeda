namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CreateSoaDataCompiledSummaryHistoryTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SoaDataCompiledSummaryHistories",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CutOffId = c.Int(nullable: false),
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
                    Gst = c.Double(),
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
                    TPD = c.Double(),
                    CI = c.Double(),
                    CurrencyCode = c.String(maxLength: 3),
                    CurrencyRate = c.Double(),
                    ContractCode = c.String(maxLength: 35),
                    AnnualCohort = c.Int(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CutOff", t => t.CutOffId)
                .Index(t => t.CutOffId)
                .Index(t => t.SoaDataBatchId)
                .Index(t => t.BusinessCode)
                .Index(t => t.TreatyCode)
                .Index(t => t.SoaQuarter)
                .Index(t => t.RiskQuarter)
                .Index(t => t.InvoiceNumber1)
                .Index(t => t.InvoiceNumber2);

        }

        public override void Down()
        {
            DropForeignKey("dbo.SoaDataCompiledSummaryHistories", "CutOffId", "dbo.CutOff");
            DropIndex("dbo.SoaDataCompiledSummaryHistories", new[] { "InvoiceNumber2" });
            DropIndex("dbo.SoaDataCompiledSummaryHistories", new[] { "InvoiceNumber1" });
            DropIndex("dbo.SoaDataCompiledSummaryHistories", new[] { "RiskQuarter" });
            DropIndex("dbo.SoaDataCompiledSummaryHistories", new[] { "SoaQuarter" });
            DropIndex("dbo.SoaDataCompiledSummaryHistories", new[] { "TreatyCode" });
            DropIndex("dbo.SoaDataCompiledSummaryHistories", new[] { "BusinessCode" });
            DropIndex("dbo.SoaDataCompiledSummaryHistories", new[] { "SoaDataBatchId" });
            DropIndex("dbo.SoaDataCompiledSummaryHistories", new[] { "CutOffId" });
            DropTable("dbo.SoaDataCompiledSummaryHistories");
        }
    }
}
