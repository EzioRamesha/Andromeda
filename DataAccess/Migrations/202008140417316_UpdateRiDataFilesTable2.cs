namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataFilesTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RiDataFiles", "Errors", c => c.String(storeType: "ntext"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RiDataFiles", "Errors");
        }
    }
}
