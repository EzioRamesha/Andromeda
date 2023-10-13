namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReferralRiDataTable3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReferralRiData", "TreatyNumber", c => c.String(maxLength: 128));
            CreateIndex("dbo.ReferralRiData", "TreatyNumber");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ReferralRiData", new[] { "TreatyNumber" });
            DropColumn("dbo.ReferralRiData", "TreatyNumber");
        }
    }
}
