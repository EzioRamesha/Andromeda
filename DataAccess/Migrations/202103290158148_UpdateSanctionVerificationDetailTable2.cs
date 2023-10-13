namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSanctionVerificationDetailTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SanctionVerificationDetails", "PreviousDecision", c => c.Int());
            AddColumn("dbo.SanctionVerificationDetails", "PreviousDecisionRemark", c => c.String(storeType: "ntext"));
            CreateIndex("dbo.SanctionVerificationDetails", "IsWhitelist");
            CreateIndex("dbo.SanctionVerificationDetails", "IsExactMatch");
            CreateIndex("dbo.SanctionVerificationDetails", "PreviousDecision");
        }
        
        public override void Down()
        {
            DropIndex("dbo.SanctionVerificationDetails", new[] { "PreviousDecision" });
            DropIndex("dbo.SanctionVerificationDetails", new[] { "IsExactMatch" });
            DropIndex("dbo.SanctionVerificationDetails", new[] { "IsWhitelist" });
            DropColumn("dbo.SanctionVerificationDetails", "PreviousDecisionRemark");
            DropColumn("dbo.SanctionVerificationDetails", "PreviousDecision");
        }
    }
}
