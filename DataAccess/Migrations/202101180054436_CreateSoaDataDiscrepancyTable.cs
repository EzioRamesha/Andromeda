namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateSoaDataDiscrepancyTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SoaDataDiscrepancies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SoaDataBatchId = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        TreatyCode = c.String(maxLength: 35),
                        CedingPlanCode = c.String(maxLength: 35),
                        CedantAmount = c.Double(),
                        MlreChecking = c.Double(),
                        Discrepancy = c.Double(),
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
                .Index(t => t.TreatyCode)
                .Index(t => t.CedingPlanCode)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SoaDataDiscrepancies", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.SoaDataDiscrepancies", "SoaDataBatchId", "dbo.SoaDataBatches");
            DropForeignKey("dbo.SoaDataDiscrepancies", "CreatedById", "dbo.Users");
            DropIndex("dbo.SoaDataDiscrepancies", new[] { "UpdatedById" });
            DropIndex("dbo.SoaDataDiscrepancies", new[] { "CreatedById" });
            DropIndex("dbo.SoaDataDiscrepancies", new[] { "CedingPlanCode" });
            DropIndex("dbo.SoaDataDiscrepancies", new[] { "TreatyCode" });
            DropIndex("dbo.SoaDataDiscrepancies", new[] { "Type" });
            DropIndex("dbo.SoaDataDiscrepancies", new[] { "SoaDataBatchId" });
            DropTable("dbo.SoaDataDiscrepancies");
        }
    }
}
