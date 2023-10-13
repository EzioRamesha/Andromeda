namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePerLifeRetroConfigurationTable1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.PerLifeRetroConfigurationTreaties", name: "BusinessTypePickListDetailId", newName: "FundsAccountingTypePickListDetailId");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.PerLifeRetroConfigurationTreaties", name: "FundsAccountingTypePickListDetailId", newName: "BusinessTypePickListDetailId");
        }
    }
}
