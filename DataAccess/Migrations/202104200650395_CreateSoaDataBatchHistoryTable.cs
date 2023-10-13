namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateSoaDataBatchHistoryTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SoaDataBatchHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CutOffId = c.Int(nullable: false),
                        SoaDataBatchId = c.Int(nullable: false),
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
                        IsAutoCreate = c.Boolean(nullable: false),
                        IsClaimDataAutoCreate = c.Boolean(nullable: false),
                        IsProfitCommissionData = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedById = c.Int(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedById = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CutOff", t => t.CutOffId)
                .Index(t => t.CutOffId)
                .Index(t => t.SoaDataBatchId)
                .Index(t => t.CedantId)
                .Index(t => t.TreatyId)
                .Index(t => t.Quarter)
                .Index(t => t.Type)
                .Index(t => t.Status)
                .Index(t => t.DataUpdateStatus)
                .Index(t => t.RiDataBatchId)
                .Index(t => t.ClaimDataBatchId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SoaDataBatchHistories", "CutOffId", "dbo.CutOff");
            DropIndex("dbo.SoaDataBatchHistories", new[] { "ClaimDataBatchId" });
            DropIndex("dbo.SoaDataBatchHistories", new[] { "RiDataBatchId" });
            DropIndex("dbo.SoaDataBatchHistories", new[] { "DataUpdateStatus" });
            DropIndex("dbo.SoaDataBatchHistories", new[] { "Status" });
            DropIndex("dbo.SoaDataBatchHistories", new[] { "Type" });
            DropIndex("dbo.SoaDataBatchHistories", new[] { "Quarter" });
            DropIndex("dbo.SoaDataBatchHistories", new[] { "TreatyId" });
            DropIndex("dbo.SoaDataBatchHistories", new[] { "CedantId" });
            DropIndex("dbo.SoaDataBatchHistories", new[] { "SoaDataBatchId" });
            DropIndex("dbo.SoaDataBatchHistories", new[] { "CutOffId" });
            DropTable("dbo.SoaDataBatchHistories");
        }
    }
}
