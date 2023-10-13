namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateProductFeatureMappingRateTableMappingTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RateTableMappingUpload",
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
            
            CreateTable(
                "dbo.TreatyBenefitCodeMappingUpload",
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
            
            AddColumn("dbo.RateTables", "RateTableMappingUploadId", c => c.Int());
            AddColumn("dbo.TreatyBenefitCodeMappings", "TreatyBenefitCodeMappingUploadId", c => c.Int());
            CreateIndex("dbo.RateTables", "RateTableMappingUploadId");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "TreatyBenefitCodeMappingUploadId");
            AddForeignKey("dbo.RateTables", "RateTableMappingUploadId", "dbo.RateTableMappingUpload", "Id");
            AddForeignKey("dbo.TreatyBenefitCodeMappings", "TreatyBenefitCodeMappingUploadId", "dbo.TreatyBenefitCodeMappingUpload", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TreatyBenefitCodeMappings", "TreatyBenefitCodeMappingUploadId", "dbo.TreatyBenefitCodeMappingUpload");
            DropForeignKey("dbo.TreatyBenefitCodeMappingUpload", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyBenefitCodeMappingUpload", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.RateTables", "RateTableMappingUploadId", "dbo.RateTableMappingUpload");
            DropForeignKey("dbo.RateTableMappingUpload", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RateTableMappingUpload", "CreatedById", "dbo.Users");
            DropIndex("dbo.TreatyBenefitCodeMappingUpload", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyBenefitCodeMappingUpload", new[] { "CreatedById" });
            DropIndex("dbo.TreatyBenefitCodeMappingUpload", new[] { "HashFileName" });
            DropIndex("dbo.TreatyBenefitCodeMappingUpload", new[] { "FileName" });
            DropIndex("dbo.TreatyBenefitCodeMappingUpload", new[] { "Status" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "TreatyBenefitCodeMappingUploadId" });
            DropIndex("dbo.RateTableMappingUpload", new[] { "UpdatedById" });
            DropIndex("dbo.RateTableMappingUpload", new[] { "CreatedById" });
            DropIndex("dbo.RateTableMappingUpload", new[] { "HashFileName" });
            DropIndex("dbo.RateTableMappingUpload", new[] { "FileName" });
            DropIndex("dbo.RateTableMappingUpload", new[] { "Status" });
            DropIndex("dbo.RateTables", new[] { "RateTableMappingUploadId" });
            DropColumn("dbo.TreatyBenefitCodeMappings", "TreatyBenefitCodeMappingUploadId");
            DropColumn("dbo.RateTables", "RateTableMappingUploadId");
            DropTable("dbo.TreatyBenefitCodeMappingUpload");
            DropTable("dbo.RateTableMappingUpload");
        }
    }
}
