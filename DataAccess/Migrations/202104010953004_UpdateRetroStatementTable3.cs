namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRetroStatementTable3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DirectRetroConfigurationDetails", "TreatyNo", c => c.String(maxLength: 128));
            AddColumn("dbo.DirectRetroConfigurationDetails", "Schedule", c => c.String(maxLength: 20));
            AddColumn("dbo.RetroStatements", "TreatyCode", c => c.String(maxLength: 35));
            AddColumn("dbo.RetroStatements", "Schedule", c => c.String(maxLength: 20));
            AlterColumn("dbo.RetroStatements", "AccountsFor", c => c.String(storeType: "ntext"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RetroStatements", "AccountsFor", c => c.String(maxLength: 128));
            DropColumn("dbo.RetroStatements", "Schedule");
            DropColumn("dbo.RetroStatements", "TreatyCode");
            DropColumn("dbo.DirectRetroConfigurationDetails", "Schedule");
            DropColumn("dbo.DirectRetroConfigurationDetails", "TreatyNo");
        }
    }
}
