namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateInvoiceRegisterBatchStatusFileTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InvoiceRegisterBatchStatusFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InvoiceRegisterBatchId = c.Int(nullable: false),
                        StatusHistoryId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedById = c.Int(nullable: false),
                        UpdatedById = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.InvoiceRegisterBatches", t => t.InvoiceRegisterBatchId)
                .ForeignKey("dbo.StatusHistories", t => t.StatusHistoryId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.InvoiceRegisterBatchId)
                .Index(t => t.StatusHistoryId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InvoiceRegisterBatchStatusFiles", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.InvoiceRegisterBatchStatusFiles", "StatusHistoryId", "dbo.StatusHistories");
            DropForeignKey("dbo.InvoiceRegisterBatchStatusFiles", "InvoiceRegisterBatchId", "dbo.InvoiceRegisterBatches");
            DropForeignKey("dbo.InvoiceRegisterBatchStatusFiles", "CreatedById", "dbo.Users");
            DropIndex("dbo.InvoiceRegisterBatchStatusFiles", new[] { "UpdatedById" });
            DropIndex("dbo.InvoiceRegisterBatchStatusFiles", new[] { "CreatedById" });
            DropIndex("dbo.InvoiceRegisterBatchStatusFiles", new[] { "StatusHistoryId" });
            DropIndex("dbo.InvoiceRegisterBatchStatusFiles", new[] { "InvoiceRegisterBatchId" });
            DropTable("dbo.InvoiceRegisterBatchStatusFiles");
        }
    }
}
