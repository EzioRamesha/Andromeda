namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateRetroRegisterBatchStatusFileTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RetroRegisterBatchStatusFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RetroRegisterBatchId = c.Int(nullable: false),
                        StatusHistoryId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedById = c.Int(nullable: false),
                        UpdatedById = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.RetroRegisterBatches", t => t.RetroRegisterBatchId)
                .ForeignKey("dbo.StatusHistories", t => t.StatusHistoryId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.RetroRegisterBatchId)
                .Index(t => t.StatusHistoryId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RetroRegisterBatchStatusFiles", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RetroRegisterBatchStatusFiles", "StatusHistoryId", "dbo.StatusHistories");
            DropForeignKey("dbo.RetroRegisterBatchStatusFiles", "RetroRegisterBatchId", "dbo.RetroRegisterBatches");
            DropForeignKey("dbo.RetroRegisterBatchStatusFiles", "CreatedById", "dbo.Users");
            DropIndex("dbo.RetroRegisterBatchStatusFiles", new[] { "UpdatedById" });
            DropIndex("dbo.RetroRegisterBatchStatusFiles", new[] { "CreatedById" });
            DropIndex("dbo.RetroRegisterBatchStatusFiles", new[] { "StatusHistoryId" });
            DropIndex("dbo.RetroRegisterBatchStatusFiles", new[] { "RetroRegisterBatchId" });
            DropTable("dbo.RetroRegisterBatchStatusFiles");
        }
    }
}
