namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePerLifeSoaTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PerLifeSoa", "PerLifeAggregationId", c => c.Int());
            CreateIndex("dbo.PerLifeSoa", "PerLifeAggregationId");
            AddForeignKey("dbo.PerLifeSoa", "PerLifeAggregationId", "dbo.PerLifeAggregations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PerLifeSoa", "PerLifeAggregationId", "dbo.PerLifeAggregations");
            DropIndex("dbo.PerLifeSoa", new[] { "PerLifeAggregationId" });
            DropColumn("dbo.PerLifeSoa", "PerLifeAggregationId");
        }
    }
}
