namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAccountCodeMappingDetailTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountCodeMappingDetails", "BusinessOrigin", c => c.String(maxLength: 30));
            CreateIndex("dbo.AccountCodeMappingDetails", "BusinessOrigin");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AccountCodeMappingDetails", new[] { "BusinessOrigin" });
            DropColumn("dbo.AccountCodeMappingDetails", "BusinessOrigin");
        }
    }
}
