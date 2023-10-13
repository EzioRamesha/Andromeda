namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClaimRegisterTable7 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ClaimRegister", new[] { "ClaimCommitteeUser2" });
            AddColumn("dbo.ClaimRegister", "ClaimCommitteeUser1Name", c => c.String(maxLength: 128));
            AddColumn("dbo.ClaimRegister", "ClaimCommitteeUser2Id", c => c.Int());
            AddColumn("dbo.ClaimRegister", "ClaimCommitteeUser2Name", c => c.String(maxLength: 128));
            CreateIndex("dbo.ClaimRegister", "ClaimCommitteeUser1Name");
            CreateIndex("dbo.ClaimRegister", "ClaimCommitteeUser2Id");
            CreateIndex("dbo.ClaimRegister", "ClaimCommitteeUser2Name");
            AddForeignKey("dbo.ClaimRegister", "ClaimCommitteeUser2Id", "dbo.Users", "Id");
            DropColumn("dbo.ClaimRegister", "ClaimCommitteeUser2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ClaimRegister", "ClaimCommitteeUser2", c => c.String(maxLength: 128));
            DropForeignKey("dbo.ClaimRegister", "ClaimCommitteeUser2Id", "dbo.Users");
            DropIndex("dbo.ClaimRegister", new[] { "ClaimCommitteeUser2Name" });
            DropIndex("dbo.ClaimRegister", new[] { "ClaimCommitteeUser2Id" });
            DropIndex("dbo.ClaimRegister", new[] { "ClaimCommitteeUser1Name" });
            DropColumn("dbo.ClaimRegister", "ClaimCommitteeUser2Name");
            DropColumn("dbo.ClaimRegister", "ClaimCommitteeUser2Id");
            DropColumn("dbo.ClaimRegister", "ClaimCommitteeUser1Name");
            CreateIndex("dbo.ClaimRegister", "ClaimCommitteeUser2");
        }
    }
}
