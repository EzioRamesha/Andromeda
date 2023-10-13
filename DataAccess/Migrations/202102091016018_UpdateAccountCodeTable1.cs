namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdateAccountCodeTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountCodes", "Type", c => c.Int());
            CreateIndex("dbo.AccountCodes", "Type");
        }

        public override void Down()
        {
            DropIndex("dbo.AccountCodes", new[] { "Type" });
            DropColumn("dbo.AccountCodes", "Type");
        }
    }
}
