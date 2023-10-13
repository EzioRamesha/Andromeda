namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSanctionVerificationTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SanctionVerifications", "UnprocessedRecords", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SanctionVerifications", "UnprocessedRecords");
        }
    }
}
