namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePerLifeClaimDataTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PerLifeClaimData", "Errors", c => c.String(storeType: "ntext"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PerLifeClaimData", "Errors");
        }
    }
}
