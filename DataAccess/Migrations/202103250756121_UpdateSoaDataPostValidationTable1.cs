namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSoaDataPostValidationTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoaDataPostValidations", "TPD", c => c.Double());
            AddColumn("dbo.SoaDataPostValidations", "CI", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoaDataPostValidations", "CI");
            DropColumn("dbo.SoaDataPostValidations", "TPD");
        }
    }
}
