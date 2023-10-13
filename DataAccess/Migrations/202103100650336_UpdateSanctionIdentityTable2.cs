namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSanctionIdentityTable2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.SanctionIdentities", new[] { "IdType" });
            DropIndex("dbo.SanctionIdentities", new[] { "IdNumber" });
            AlterColumn("dbo.SanctionIdentities", "IdType", c => c.String(maxLength: 128));
            AlterColumn("dbo.SanctionIdentities", "IdNumber", c => c.String(maxLength: 128));
            CreateIndex("dbo.SanctionIdentities", "IdType");
            CreateIndex("dbo.SanctionIdentities", "IdNumber");
        }
        
        public override void Down()
        {
            DropIndex("dbo.SanctionIdentities", new[] { "IdNumber" });
            DropIndex("dbo.SanctionIdentities", new[] { "IdType" });
            AlterColumn("dbo.SanctionIdentities", "IdNumber", c => c.String(maxLength: 30));
            AlterColumn("dbo.SanctionIdentities", "IdType", c => c.String(maxLength: 30));
            CreateIndex("dbo.SanctionIdentities", "IdNumber");
            CreateIndex("dbo.SanctionIdentities", "IdType");
        }
    }
}
