namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdatePremiumSpreadTableTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PremiumSpreadTables", "Description", c => c.String(maxLength: 255));
        }

        public override void Down()
        {
            DropColumn("dbo.PremiumSpreadTables", "Description");
        }
    }
}
