namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSanctionVerificationDetailTable3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SanctionVerificationDetails", "Rule", c => c.Int(nullable: false));
            AddColumn("dbo.SanctionVerificationDetails", "Category", c => c.String(maxLength: 128));
            CreateIndex("dbo.SanctionVerificationDetails", "Rule");
            CreateIndex("dbo.SanctionVerificationDetails", "Category");
        }
        
        public override void Down()
        {
            DropIndex("dbo.SanctionVerificationDetails", new[] { "Category" });
            DropIndex("dbo.SanctionVerificationDetails", new[] { "Rule" });
            DropColumn("dbo.SanctionVerificationDetails", "Category");
            DropColumn("dbo.SanctionVerificationDetails", "Rule");
        }
    }
}
