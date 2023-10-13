namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClaimDataTable4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClaimData", "PolicyExpiryDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            CreateIndex("dbo.ClaimData", "PolicyExpiryDate");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ClaimData", new[] { "PolicyExpiryDate" });
            DropColumn("dbo.ClaimData", "PolicyExpiryDate");
        }
    }
}
