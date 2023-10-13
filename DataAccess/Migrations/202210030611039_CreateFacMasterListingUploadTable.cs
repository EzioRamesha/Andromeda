namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateFacMasterListingUploadTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FacMasterListingUpload",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Status)
                .Index(t => t.FileName)
                .Index(t => t.HashFileName)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FacMasterListingUpload", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.FacMasterListingUpload", "CreatedById", "dbo.Users");
            DropIndex("dbo.FacMasterListingUpload", new[] { "UpdatedById" });
            DropIndex("dbo.FacMasterListingUpload", new[] { "CreatedById" });
            DropIndex("dbo.FacMasterListingUpload", new[] { "HashFileName" });
            DropIndex("dbo.FacMasterListingUpload", new[] { "FileName" });
            DropIndex("dbo.FacMasterListingUpload", new[] { "Status" });
            DropTable("dbo.FacMasterListingUpload");
        }
    }
}
