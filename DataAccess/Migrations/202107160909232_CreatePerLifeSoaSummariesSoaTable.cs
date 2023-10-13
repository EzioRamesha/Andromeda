namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CreatePerLifeSoaSummariesSoaTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PerLifeSoaSummariesSoa",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PerLifeSoaId = c.Int(nullable: false),
                    PremiumClaim = c.Int(nullable: false),
                    RowLabel = c.String(maxLength: 255),
                    Individual = c.Double(),
                    Group = c.Double(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PerLifeSoa", t => t.PerLifeSoaId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.PerLifeSoaId)
                .Index(t => t.PremiumClaim)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

        }

        public override void Down()
        {
            DropForeignKey("dbo.PerLifeSoaSummariesSoa", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeSoaSummariesSoa", "PerLifeSoaId", "dbo.PerLifeSoa");
            DropForeignKey("dbo.PerLifeSoaSummariesSoa", "CreatedById", "dbo.Users");
            DropIndex("dbo.PerLifeSoaSummariesSoa", new[] { "UpdatedById" });
            DropIndex("dbo.PerLifeSoaSummariesSoa", new[] { "CreatedById" });
            DropIndex("dbo.PerLifeSoaSummariesSoa", new[] { "PremiumClaim" });
            DropIndex("dbo.PerLifeSoaSummariesSoa", new[] { "PerLifeSoaId" });
            DropTable("dbo.PerLifeSoaSummariesSoa");
        }
    }
}
