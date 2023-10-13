namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataTable6 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.RiData", "PremiumFrequencyCode");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RiData", new[] { "PremiumFrequencyCode" });
        }
    }
}
