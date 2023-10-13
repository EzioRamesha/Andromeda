namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyCodesTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TreatyCodes", new[] { "OldTreatyCode" });
            AlterColumn("dbo.TreatyCodes", "OldTreatyCodeId", c => c.Int());
            DropColumn("dbo.TreatyCodes", "OldTreatyCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TreatyCodes", "OldTreatyCode", c => c.String(maxLength: 30));
            AlterColumn("dbo.TreatyCodes", "OldTreatyCodeId", c => c.Int(nullable: false));
            CreateIndex("dbo.TreatyCodes", "OldTreatyCode");
        }
    }
}
