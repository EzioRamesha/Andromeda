namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClaimAuthorityLimitMlreTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClaimAuthorityLimitMLRe", "IsAllowOverwriteApproval", c => c.Boolean(nullable: false));
            CreateIndex("dbo.ClaimAuthorityLimitMLRe", "IsAllowOverwriteApproval");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ClaimAuthorityLimitMLRe", new[] { "IsAllowOverwriteApproval" });
            DropColumn("dbo.ClaimAuthorityLimitMLRe", "IsAllowOverwriteApproval");
        }
    }
}
