﻿namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyDiscountTableDetailTable2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TreatyDiscountTableDetails", "BenefitCode", c => c.String(storeType: "ntext"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TreatyDiscountTableDetails", "BenefitCode", c => c.String(nullable: false));
        }
    }
}
