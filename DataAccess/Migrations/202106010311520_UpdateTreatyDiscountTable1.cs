namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyDiscountTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TreatyDiscountTables", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TreatyDiscountTables", "Description");
        }
    }
}
