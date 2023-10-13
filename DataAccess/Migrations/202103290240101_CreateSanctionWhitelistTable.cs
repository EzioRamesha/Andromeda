namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CreateSanctionWhitelistTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SanctionWhitelists",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PolicyNumber = c.String(nullable: false, maxLength: 150),
                    InsuredName = c.String(nullable: false, maxLength: 128),
                    Reason = c.String(storeType: "ntext"),
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
            DropForeignKey("dbo.SanctionWhitelists", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.SanctionWhitelists", "CreatedById", "dbo.Users");
            DropIndex("dbo.SanctionWhitelists", new[] { "UpdatedById" });
            DropIndex("dbo.SanctionWhitelists", new[] { "CreatedById" });
            DropIndex("dbo.SanctionWhitelists", new[] { "InsuredName" });
            DropIndex("dbo.SanctionWhitelists", new[] { "PolicyNumber" });
            DropTable("dbo.SanctionWhitelists");
        }
    }
}
