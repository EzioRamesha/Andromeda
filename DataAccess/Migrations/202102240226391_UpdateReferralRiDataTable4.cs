namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReferralRiDataTable4 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ReferralRiData", new[] { "LoaCode" });
            AlterColumn("dbo.ReferralRiData", "LoaCode", c => c.String(maxLength: 20));
            CreateIndex("dbo.ReferralRiData", "LoaCode");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ReferralRiData", new[] { "LoaCode" });
            AlterColumn("dbo.ReferralRiData", "LoaCode", c => c.String(maxLength: 10));
            CreateIndex("dbo.ReferralRiData", "LoaCode");
        }
    }
}
