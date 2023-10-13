namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRetroPartyTable3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RetroParties", "AccountCodeId", "dbo.AccountCodes");
            DropIndex("dbo.RetroParties", new[] { "AccountCodeId" });
            AddColumn("dbo.RetroParties", "AccountCode", c => c.String(maxLength: 10));
            CreateIndex("dbo.RetroParties", "AccountCode");
            DropColumn("dbo.RetroParties", "AccountCodeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RetroParties", "AccountCodeId", c => c.Int());
            DropIndex("dbo.RetroParties", new[] { "AccountCode" });
            DropColumn("dbo.RetroParties", "AccountCode");
            CreateIndex("dbo.RetroParties", "AccountCodeId");
            AddForeignKey("dbo.RetroParties", "AccountCodeId", "dbo.AccountCodes", "Id");
        }
    }
}
