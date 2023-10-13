namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdatePremiumSpreadTableDetailTable2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PremiumSpreadTableDetails", new[] { "BenefitId" });
            AddColumn("dbo.PremiumSpreadTableDetails", "BenefitCode", c => c.String(storeType: "ntext"));
            AlterColumn("dbo.PremiumSpreadTableDetails", "BenefitId", c => c.Int());
            CreateIndex("dbo.PremiumSpreadTableDetails", "BenefitId");
        }

        public override void Down()
        {
            DropIndex("dbo.PremiumSpreadTableDetails", new[] { "BenefitId" });
            AlterColumn("dbo.PremiumSpreadTableDetails", "BenefitId", c => c.Int(nullable: false));
            DropColumn("dbo.PremiumSpreadTableDetails", "BenefitCode");
            CreateIndex("dbo.PremiumSpreadTableDetails", "BenefitId");
        }
    }
}
