namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePerLifeSoaSummariesTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PerLifeSoaSummaries", "WMOM", c => c.Int(nullable: false));
            CreateIndex("dbo.PerLifeSoaSummaries", "WMOM");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PerLifeSoaSummaries", new[] { "WMOM" });
            DropColumn("dbo.PerLifeSoaSummaries", "WMOM");
        }
    }
}
