namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSanctionVerificationDetailTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SanctionVerificationDetails", "Remark", c => c.String(storeType: "ntext"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SanctionVerificationDetails", "Remark");
        }
    }
}
