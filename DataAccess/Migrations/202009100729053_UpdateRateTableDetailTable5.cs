namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRateTableDetailTable5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RateTableDetails", "CreatedById", "dbo.Users");
            AddForeignKey("dbo.RateTableDetails", "CreatedById", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RateTableDetails", "CreatedById", "dbo.Users");
            AddForeignKey("dbo.RateTableDetails", "CreatedById", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
