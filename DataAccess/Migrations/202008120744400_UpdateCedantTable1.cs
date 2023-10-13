namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCedantTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Cedants", new[] { "Remarks" });
            AlterColumn("dbo.Cedants", "Remarks", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cedants", "Remarks", c => c.String(maxLength: 255));
            CreateIndex("dbo.Cedants", "Remarks");
        }
    }
}
