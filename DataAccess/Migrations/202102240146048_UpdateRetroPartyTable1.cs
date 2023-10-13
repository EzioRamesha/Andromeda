namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRetroPartyTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RetroParties", new[] { "Type" });
            AddColumn("dbo.RetroParties", "IsDirectRetro", c => c.Boolean(nullable: false));
            AddColumn("dbo.RetroParties", "IsPerLifeRetro", c => c.Boolean(nullable: false));
            CreateIndex("dbo.RetroParties", "IsDirectRetro");
            CreateIndex("dbo.RetroParties", "IsPerLifeRetro");
            DropColumn("dbo.RetroParties", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RetroParties", "Type", c => c.Int(nullable: false));
            DropIndex("dbo.RetroParties", new[] { "IsPerLifeRetro" });
            DropIndex("dbo.RetroParties", new[] { "IsDirectRetro" });
            DropColumn("dbo.RetroParties", "IsPerLifeRetro");
            DropColumn("dbo.RetroParties", "IsDirectRetro");
            CreateIndex("dbo.RetroParties", "Type");
        }
    }
}
