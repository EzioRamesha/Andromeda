namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CreateSanctionBlacklistTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SanctionBlacklists",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PolicyNumber = c.String(nullable: false, maxLength: 150),
                    InsuredName = c.String(nullable: false, maxLength: 128),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.PolicyNumber)
                .Index(t => t.InsuredName)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

        }

        public override void Down()
        {
            DropForeignKey("dbo.SanctionBlacklists", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionBlacklists", "CreatedById", "dbo.Users");
            DropIndex("dbo.SanctionBlacklists", new[] { "UpdatedById" });
            DropIndex("dbo.SanctionBlacklists", new[] { "CreatedById" });
            DropIndex("dbo.SanctionBlacklists", new[] { "InsuredName" });
            DropIndex("dbo.SanctionBlacklists", new[] { "PolicyNumber" });
            DropTable("dbo.SanctionBlacklists");
        }
    }
}
