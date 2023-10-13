namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClaimRegisterHistoryTable8 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ClaimCommitteeUser2" });
            AddColumn("dbo.ClaimRegisterHistories", "ClaimCommitteeUser1Name", c => c.String(maxLength: 128));
            AddColumn("dbo.ClaimRegisterHistories", "ClaimCommitteeUser2Id", c => c.Int());
            AddColumn("dbo.ClaimRegisterHistories", "ClaimCommitteeUser2Name", c => c.String(maxLength: 128));
            CreateIndex("dbo.ClaimRegisterHistories", "ClaimCommitteeUser1Name");
            CreateIndex("dbo.ClaimRegisterHistories", "ClaimCommitteeUser2Id");
            CreateIndex("dbo.ClaimRegisterHistories", "ClaimCommitteeUser2Name");
            AddForeignKey("dbo.ClaimRegisterHistories", "ClaimCommitteeUser2Id", "dbo.Users", "Id");
            DropColumn("dbo.ClaimRegisterHistories", "ClaimCommitteeUser2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ClaimRegisterHistories", "ClaimCommitteeUser2", c => c.String(maxLength: 128));
            DropForeignKey("dbo.ClaimRegisterHistories", "ClaimCommitteeUser2Id", "dbo.Users");
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ClaimCommitteeUser2Name" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ClaimCommitteeUser2Id" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ClaimCommitteeUser1Name" });
            DropColumn("dbo.ClaimRegisterHistories", "ClaimCommitteeUser2Name");
            DropColumn("dbo.ClaimRegisterHistories", "ClaimCommitteeUser2Id");
            DropColumn("dbo.ClaimRegisterHistories", "ClaimCommitteeUser1Name");
            CreateIndex("dbo.ClaimRegisterHistories", "ClaimCommitteeUser2");
        }
    }
}
