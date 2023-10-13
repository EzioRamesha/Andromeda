namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CreatePerLifeSoaSummariesByTreatyTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PerLifeSoaSummariesByTreaty",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PerLifeSoaId = c.Int(nullable: false),
                    TreatyCode = c.String(maxLength: 35),
                    ProcessingPeriod = c.String(maxLength: 30),
                    TotalRetroAmount = c.Double(),
                    TotalGrossPremium = c.Double(),
                    TotalNetPremium = c.Double(),
                    TotalDiscount = c.Double(),
                    TotalPolicyCount = c.Double(),
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
                .Index(t => t.TreatyCode)
                .Index(t => t.ProcessingPeriod)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

        }

        public override void Down()
        {
            DropForeignKey("dbo.PerLifeSoaSummariesByTreaty", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeSoaSummariesByTreaty", "PerLifeSoaId", "dbo.PerLifeSoa");
            DropForeignKey("dbo.PerLifeSoaSummariesByTreaty", "CreatedById", "dbo.Users");
            DropIndex("dbo.PerLifeSoaSummariesByTreaty", new[] { "UpdatedById" });
            DropIndex("dbo.PerLifeSoaSummariesByTreaty", new[] { "CreatedById" });
            DropIndex("dbo.PerLifeSoaSummariesByTreaty", new[] { "ProcessingPeriod" });
            DropIndex("dbo.PerLifeSoaSummariesByTreaty", new[] { "TreatyCode" });
            DropIndex("dbo.PerLifeSoaSummariesByTreaty", new[] { "PerLifeSoaId" });
            DropTable("dbo.PerLifeSoaSummariesByTreaty");
        }
    }
}
