namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataCorrectionTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RiDataCorrections", "ApLoading", c => c.Double());
            CreateIndex("dbo.RiDataCorrections", "ApLoading");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RiDataCorrections", new[] { "ApLoading" });
            DropColumn("dbo.RiDataCorrections", "ApLoading");
        }
    }
}
