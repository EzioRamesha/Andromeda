namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CreateAccountCodeMappingDetailTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountCodeMappingDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    AccountCodeMappingId = c.Int(nullable: false),
                    TreatyType = c.String(maxLength: 20),
                    ClaimCode = c.String(maxLength: 30),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccountCodeMappings", t => t.AccountCodeMappingId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .Index(t => t.AccountCodeMappingId)
                .Index(t => t.TreatyType)
                .Index(t => t.ClaimCode)
                .Index(t => t.CreatedById);

        }

        public override void Down()
        {
            DropForeignKey("dbo.AccountCodeMappingDetails", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.AccountCodeMappingDetails", "AccountCodeMappingId", "dbo.AccountCodeMappings");
            DropIndex("dbo.AccountCodeMappingDetails", new[] { "CreatedById" });
            DropIndex("dbo.AccountCodeMappingDetails", new[] { "ClaimCode" });
            DropIndex("dbo.AccountCodeMappingDetails", new[] { "TreatyType" });
            DropIndex("dbo.AccountCodeMappingDetails", new[] { "AccountCodeMappingId" });
            DropTable("dbo.AccountCodeMappingDetails");
        }
    }
}
