namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataFileTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RiDataFiles", "RecordType", c => c.Int(nullable: false));
            CreateIndex("dbo.RiDataFiles", "RecordType");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RiDataFiles", new[] { "RecordType" });
            DropColumn("dbo.RiDataFiles", "RecordType");
        }
    }
}
