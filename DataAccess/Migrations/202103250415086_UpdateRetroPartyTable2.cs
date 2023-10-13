namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRetroPartyTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RetroParties", "AccountCodeId", c => c.Int());
            CreateIndex("dbo.RetroParties", "AccountCodeId");
            AddForeignKey("dbo.RetroParties", "AccountCodeId", "dbo.AccountCodes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RetroParties", "AccountCodeId", "dbo.AccountCodes");
            DropIndex("dbo.RetroParties", new[] { "AccountCodeId" });
            DropColumn("dbo.RetroParties", "AccountCodeId");
        }
    }
}
