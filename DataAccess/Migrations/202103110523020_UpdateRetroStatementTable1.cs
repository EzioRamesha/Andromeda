namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRetroStatementTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RetroStatements", "RiPremiumALT", c => c.Double());
            AddColumn("dbo.RetroStatements", "RiDiscountALT", c => c.Double());
            AddColumn("dbo.RetroStatements", "GstPayable", c => c.Double());
            AddColumn("dbo.RetroStatements", "TotalNoOfPolicyALT", c => c.Int());
            AddColumn("dbo.RetroStatements", "TotalSumReinsuredALT", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RetroStatements", "TotalSumReinsuredALT");
            DropColumn("dbo.RetroStatements", "TotalNoOfPolicyALT");
            DropColumn("dbo.RetroStatements", "GstPayable");
            DropColumn("dbo.RetroStatements", "RiDiscountALT");
            DropColumn("dbo.RetroStatements", "RiPremiumALT");
        }
    }
}
