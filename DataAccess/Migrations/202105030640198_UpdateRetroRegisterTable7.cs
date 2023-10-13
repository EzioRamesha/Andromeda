namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRetroRegisterTable7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RetroRegister", "ContractCode", c => c.String(maxLength: 35));
            AddColumn("dbo.RetroRegister", "AnnualCohort", c => c.Int());
            AddColumn("dbo.RetroRegister", "ReportingType", c => c.Int(nullable: false));
            CreateIndex("dbo.RetroRegister", "ReportingType");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RetroRegister", new[] { "ReportingType" });
            DropColumn("dbo.RetroRegister", "ReportingType");
            DropColumn("dbo.RetroRegister", "AnnualCohort");
            DropColumn("dbo.RetroRegister", "ContractCode");
        }
    }
}
