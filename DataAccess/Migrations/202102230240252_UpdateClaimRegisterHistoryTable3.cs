namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClaimRegisterHistoryTable3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ClaimRegisterHistories", "ClaimCommitteeUser2Id", "dbo.Users");
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ClaimCommitteeUser2Id" });
            AddColumn("dbo.ClaimRegisterHistories", "ClaimCommitteeUser2", c => c.String(maxLength: 128));
            CreateIndex("dbo.ClaimRegisterHistories", "ClaimCommitteeUser2");
            DropColumn("dbo.ClaimRegisterHistories", "ClaimCommitteeUser2Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ClaimRegisterHistories", "ClaimCommitteeUser2Id", c => c.Int());
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ClaimCommitteeUser2" });
            DropColumn("dbo.ClaimRegisterHistories", "ClaimCommitteeUser2");
            CreateIndex("dbo.ClaimRegisterHistories", "ClaimCommitteeUser2Id");
            AddForeignKey("dbo.ClaimRegisterHistories", "ClaimCommitteeUser2Id", "dbo.Users", "Id");
        }
    }
}
