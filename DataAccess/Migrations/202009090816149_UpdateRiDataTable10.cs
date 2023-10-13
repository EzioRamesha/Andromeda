namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdateRiDataTable10 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RiData", "CedingPlanCode2", c => c.String(maxLength: 30));
        }

        public override void Down()
        {
            AlterColumn("dbo.RiData", "CedingPlanCode2", c => c.String(maxLength: 10));
        }
    }
}
