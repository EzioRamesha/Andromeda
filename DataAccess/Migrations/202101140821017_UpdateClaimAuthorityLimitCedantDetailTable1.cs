namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClaimAuthorityLimitCedantDetailTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClaimAuthorityLimitCedantDetails", "Type", c => c.Int(nullable: false));
            CreateIndex("dbo.ClaimAuthorityLimitCedantDetails", "Type");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ClaimAuthorityLimitCedantDetails", new[] { "Type" });
            DropColumn("dbo.ClaimAuthorityLimitCedantDetails", "Type");
        }
    }
}
