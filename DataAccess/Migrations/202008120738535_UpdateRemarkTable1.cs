namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRemarkTable1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Remarks", "Content", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Remarks", "Content", c => c.String(nullable: false, maxLength: 128));
        }
    }
}
