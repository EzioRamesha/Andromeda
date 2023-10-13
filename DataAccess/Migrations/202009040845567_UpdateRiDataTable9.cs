namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataTable9 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RiData", "DurationDay", c => c.Double());
            AlterColumn("dbo.RiData", "DurationMonth", c => c.Double());
            AlterColumn("dbo.RiData", "RiCovPeriod", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RiData", "RiCovPeriod", c => c.Int());
            AlterColumn("dbo.RiData", "DurationMonth", c => c.Int());
            AlterColumn("dbo.RiData", "DurationDay", c => c.Int());
        }
    }
}
