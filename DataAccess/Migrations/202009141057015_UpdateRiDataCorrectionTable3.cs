namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataCorrectionTable3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RiDataCorrections", "PolicyNumber", c => c.String(nullable: false, maxLength: 150));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RiDataCorrections", "PolicyNumber", c => c.String(nullable: false, maxLength: 30));
        }
    }
}
