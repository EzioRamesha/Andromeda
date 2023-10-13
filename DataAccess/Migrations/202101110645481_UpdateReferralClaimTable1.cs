namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReferralClaimTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReferralClaims", "DocTurnAroundTime", c => c.Long());
            CreateIndex("dbo.ReferralClaims", "DocTurnAroundTime");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ReferralClaims", new[] { "DocTurnAroundTime" });
            DropColumn("dbo.ReferralClaims", "DocTurnAroundTime");
        }
    }
}
