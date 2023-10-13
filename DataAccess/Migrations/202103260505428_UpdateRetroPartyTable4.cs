namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRetroPartyTable4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RetroParties", "AccountCodeDescription", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RetroParties", "AccountCodeDescription");
        }
    }
}
