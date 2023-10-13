namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateRateDetailUploadTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RateDetailUpload",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RateId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        FileName = c.String(maxLength: 255),
                        HashFileName = c.String(maxLength: 255),
                        Errors = c.String(storeType: "ntext"),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedById = c.Int(nullable: false),
                        UpdatedById = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Rates", t => t.RateId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.RateId)
                .Index(t => t.Status)
                .Index(t => t.FileName)
                .Index(t => t.HashFileName)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RateDetailUpload", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RateDetailUpload", "RateId", "dbo.Rates");
            DropForeignKey("dbo.RateDetailUpload", "CreatedById", "dbo.Users");
            DropIndex("dbo.RateDetailUpload", new[] { "UpdatedById" });
            DropIndex("dbo.RateDetailUpload", new[] { "CreatedById" });
            DropIndex("dbo.RateDetailUpload", new[] { "HashFileName" });
            DropIndex("dbo.RateDetailUpload", new[] { "FileName" });
            DropIndex("dbo.RateDetailUpload", new[] { "Status" });
            DropIndex("dbo.RateDetailUpload", new[] { "RateId" });
            DropTable("dbo.RateDetailUpload");
        }
    }
}
