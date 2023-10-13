namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAccountCodeMappingTable9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountCodeMappings", "TreatyNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccountCodeMappings", "TreatyNumber");
        }
    }
}
