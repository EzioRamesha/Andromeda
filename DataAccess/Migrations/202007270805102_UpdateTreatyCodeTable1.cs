namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyCodeTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TreatyCodes", new[] { "Code" });
            AlterColumn("dbo.TreatyCodes", "Code", c => c.String(nullable: false, maxLength: 35));
            CreateIndex("dbo.TreatyCodes", "Code");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TreatyCodes", new[] { "Code" });
            AlterColumn("dbo.TreatyCodes", "Code", c => c.String(nullable: false, maxLength: 30));
            CreateIndex("dbo.TreatyCodes", "Code");
        }
    }
}
