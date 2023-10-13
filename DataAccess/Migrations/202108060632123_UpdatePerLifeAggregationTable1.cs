namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePerLifeAggregationTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PerLifeAggregations", "PersonInChargeId", c => c.Int());
            CreateIndex("dbo.PerLifeAggregations", "PersonInChargeId");
            AddForeignKey("dbo.PerLifeAggregations", "PersonInChargeId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PerLifeAggregations", "PersonInChargeId", "dbo.Users");
            DropIndex("dbo.PerLifeAggregations", new[] { "PersonInChargeId" });
            DropColumn("dbo.PerLifeAggregations", "PersonInChargeId");
        }
    }
}
