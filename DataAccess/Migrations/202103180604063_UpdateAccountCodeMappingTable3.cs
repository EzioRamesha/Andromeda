namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAccountCodeMappingTable3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountCodeMappings", "BusinessOrigin", c => c.String(storeType: "ntext"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccountCodeMappings", "BusinessOrigin");
        }
    }
}
