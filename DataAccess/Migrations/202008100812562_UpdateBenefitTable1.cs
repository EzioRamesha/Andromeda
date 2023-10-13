namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBenefitTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Benefits", new[] { "Code" });
            AlterColumn("dbo.Benefits", "Code", c => c.String(nullable: false, maxLength: 10));
            CreateIndex("dbo.Benefits", "Code");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Benefits", new[] { "Code" });
            AlterColumn("dbo.Benefits", "Code", c => c.String(nullable: false, maxLength: 20));
            CreateIndex("dbo.Benefits", "Code");
        }
    }
}
