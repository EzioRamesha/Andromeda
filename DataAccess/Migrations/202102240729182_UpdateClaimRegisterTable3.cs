namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClaimRegisterTable3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClaimRegister", "ClaimDecisionStatus", c => c.Int());
            CreateIndex("dbo.ClaimRegister", "ClaimDecisionStatus");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ClaimRegister", new[] { "ClaimDecisionStatus" });
            DropColumn("dbo.ClaimRegister", "ClaimDecisionStatus");
        }
    }
}
