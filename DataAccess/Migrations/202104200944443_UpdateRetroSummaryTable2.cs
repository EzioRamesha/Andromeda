namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdateRetroSummaryTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RetroSummaries", "TreatyCode", c => c.String(maxLength: 35));
            AddColumn("dbo.RetroSummaries", "RetroPremiumSpread1", c => c.Double());
            AddColumn("dbo.RetroSummaries", "RetroPremiumSpread2", c => c.Double());
            AddColumn("dbo.RetroSummaries", "RetroPremiumSpread3", c => c.Double());
            AddColumn("dbo.RetroSummaries", "TotalDirectRetroAar", c => c.Double());
            CreateIndex("dbo.RetroSummaries", "TreatyCode");
            CreateIndex("dbo.RetroSummaries", "RetroPremiumSpread1");
            CreateIndex("dbo.RetroSummaries", "RetroPremiumSpread2");
            CreateIndex("dbo.RetroSummaries", "RetroPremiumSpread3");
            CreateIndex("dbo.RetroSummaries", "TotalDirectRetroAar");
        }

        public override void Down()
        {
            DropIndex("dbo.RetroSummaries", new[] { "TotalDirectRetroAar" });
            DropIndex("dbo.RetroSummaries", new[] { "RetroPremiumSpread3" });
            DropIndex("dbo.RetroSummaries", new[] { "RetroPremiumSpread2" });
            DropIndex("dbo.RetroSummaries", new[] { "RetroPremiumSpread1" });
            DropIndex("dbo.RetroSummaries", new[] { "TreatyCode" });
            DropColumn("dbo.RetroSummaries", "TotalDirectRetroAar");
            DropColumn("dbo.RetroSummaries", "RetroPremiumSpread3");
            DropColumn("dbo.RetroSummaries", "RetroPremiumSpread2");
            DropColumn("dbo.RetroSummaries", "RetroPremiumSpread1");
            DropColumn("dbo.RetroSummaries", "TreatyCode");
        }
    }
}
