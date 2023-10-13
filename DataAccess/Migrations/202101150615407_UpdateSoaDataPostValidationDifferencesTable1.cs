namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSoaDataPostValidationDifferencesTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoaDataPostValidationDifferences", "Remark", c => c.String(maxLength: 128));
            AddColumn("dbo.SoaDataPostValidationDifferences", "Check", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoaDataPostValidationDifferences", "Check");
            DropColumn("dbo.SoaDataPostValidationDifferences", "Remark");
        }
    }
}
