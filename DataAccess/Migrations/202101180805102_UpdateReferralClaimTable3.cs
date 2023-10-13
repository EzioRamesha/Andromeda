namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReferralClaimTable3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReferralClaims", "DecisionStatus", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReferralClaims", "DecisionStatus");
        }
    }
}
