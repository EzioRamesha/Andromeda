namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClaimDataTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClaimData", "ReportingStatus", c => c.Int(nullable: false));
            CreateIndex("dbo.ClaimData", "ReportingStatus");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ClaimData", new[] { "ReportingStatus" });
            DropColumn("dbo.ClaimData", "ReportingStatus");
        }
    }
}
