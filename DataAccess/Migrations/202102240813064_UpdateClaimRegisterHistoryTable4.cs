namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClaimRegisterHistoryTable4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClaimRegisterHistories", "ClaimDecisionStatus", c => c.Int());
            CreateIndex("dbo.ClaimRegisterHistories", "ClaimDecisionStatus");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ClaimDecisionStatus" });
            DropColumn("dbo.ClaimRegisterHistories", "ClaimDecisionStatus");
        }
    }
}
