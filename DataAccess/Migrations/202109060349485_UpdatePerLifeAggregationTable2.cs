namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePerLifeAggregationTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PerLifeAggregations", "Errors", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PerLifeAggregations", "Errors");
        }
    }
}
