namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFinanceProvisioningTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FinanceProvisionings", "ProvisionAt", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FinanceProvisionings", "ProvisionAt");
        }
    }
}
