namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyCodeTable3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TreatyCodes", "TreatyNo", c => c.String(maxLength: 128));
            CreateIndex("dbo.TreatyCodes", "TreatyNo");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TreatyCodes", new[] { "TreatyNo" });
            DropColumn("dbo.TreatyCodes", "TreatyNo");
        }
    }
}
