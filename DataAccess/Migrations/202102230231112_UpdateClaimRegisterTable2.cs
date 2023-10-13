namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClaimRegisterTable2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ClaimRegister", "ClaimCommitteeUser2Id", "dbo.Users");
            DropIndex("dbo.ClaimRegister", new[] { "ClaimCommitteeUser2Id" });
            AddColumn("dbo.ClaimRegister", "ClaimCommitteeUser2", c => c.String(maxLength: 128));
            CreateIndex("dbo.ClaimRegister", "ClaimCommitteeUser2");
            DropColumn("dbo.ClaimRegister", "ClaimCommitteeUser2Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ClaimRegister", "ClaimCommitteeUser2Id", c => c.Int());
            DropIndex("dbo.ClaimRegister", new[] { "ClaimCommitteeUser2" });
            DropColumn("dbo.ClaimRegister", "ClaimCommitteeUser2");
            CreateIndex("dbo.ClaimRegister", "ClaimCommitteeUser2Id");
            AddForeignKey("dbo.ClaimRegister", "ClaimCommitteeUser2Id", "dbo.Users", "Id");
        }
    }
}
