namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRetroTreatyTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RetroTreaties", new[] { "Party" });
            AddColumn("dbo.RetroTreaties", "RetroPartyId", c => c.Int());
            AlterColumn("dbo.RetroTreaties", "Party", c => c.String(maxLength: 50));
            CreateIndex("dbo.RetroTreaties", "RetroPartyId");
            AddForeignKey("dbo.RetroTreaties", "RetroPartyId", "dbo.RetroParties", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RetroTreaties", "RetroPartyId", "dbo.RetroParties");
            DropIndex("dbo.RetroTreaties", new[] { "RetroPartyId" });
            AlterColumn("dbo.RetroTreaties", "Party", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.RetroTreaties", "RetroPartyId");
            CreateIndex("dbo.RetroTreaties", "Party");
        }
    }
}
