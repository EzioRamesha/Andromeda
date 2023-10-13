namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSanctionFormatNameTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SanctionFormatNames", "TypeIndex", c => c.Int());
            CreateIndex("dbo.SanctionFormatNames", "TypeIndex");
        }
        
        public override void Down()
        {
            DropIndex("dbo.SanctionFormatNames", new[] { "TypeIndex" });
            DropColumn("dbo.SanctionFormatNames", "TypeIndex");
        }
    }
}
