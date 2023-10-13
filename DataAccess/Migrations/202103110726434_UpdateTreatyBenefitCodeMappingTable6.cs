namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyBenefitCodeMappingTable6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TreatyBenefitCodeMappings", "ReinsuranceIssueAgeFrom", c => c.Int());
            AddColumn("dbo.TreatyBenefitCodeMappings", "ReinsuranceIssueAgeTo", c => c.Int());
            CreateIndex("dbo.TreatyBenefitCodeMappings", "ReinsuranceIssueAgeFrom");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "ReinsuranceIssueAgeTo");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "ReinsuranceIssueAgeTo" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "ReinsuranceIssueAgeFrom" });
            DropColumn("dbo.TreatyBenefitCodeMappings", "ReinsuranceIssueAgeTo");
            DropColumn("dbo.TreatyBenefitCodeMappings", "ReinsuranceIssueAgeFrom");
        }
    }
}
