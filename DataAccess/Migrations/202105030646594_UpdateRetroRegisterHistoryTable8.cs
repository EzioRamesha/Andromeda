namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRetroRegisterHistoryTable8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RetroRegisterHistories", "ContractCode", c => c.String(maxLength: 35));
            AddColumn("dbo.RetroRegisterHistories", "AnnualCohort", c => c.Int());
            AddColumn("dbo.RetroRegisterHistories", "ReportingType", c => c.Int(nullable: false));
            CreateIndex("dbo.RetroRegisterHistories", "ReportingType");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RetroRegisterHistories", new[] { "ReportingType" });
            DropColumn("dbo.RetroRegisterHistories", "ReportingType");
            DropColumn("dbo.RetroRegisterHistories", "AnnualCohort");
            DropColumn("dbo.RetroRegisterHistories", "ContractCode");
        }
    }
}
