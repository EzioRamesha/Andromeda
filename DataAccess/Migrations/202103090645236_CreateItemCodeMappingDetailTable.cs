namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CreateItemCodeMappingDetailTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ItemCodeMappingDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ItemCodeMappingId = c.Int(nullable: false),
                    TreatyType = c.String(maxLength: 20),
                    TreatyCode = c.String(maxLength: 35),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.ItemCodeMappings", t => t.ItemCodeMappingId)
                .Index(t => t.ItemCodeMappingId)
                .Index(t => t.TreatyType)
                .Index(t => t.TreatyCode)
                .Index(t => t.CreatedById);

        }

        public override void Down()
        {
            DropForeignKey("dbo.ItemCodeMappingDetails", "ItemCodeMappingId", "dbo.ItemCodeMappings");
            DropForeignKey("dbo.ItemCodeMappingDetails", "CreatedById", "dbo.Users");
            DropIndex("dbo.ItemCodeMappingDetails", new[] { "CreatedById" });
            DropIndex("dbo.ItemCodeMappingDetails", new[] { "TreatyCode" });
            DropIndex("dbo.ItemCodeMappingDetails", new[] { "TreatyType" });
            DropIndex("dbo.ItemCodeMappingDetails", new[] { "ItemCodeMappingId" });
            DropTable("dbo.ItemCodeMappingDetails");
        }
    }
}
