namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClaimDataTable3 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ClaimData", new[] { "CedantDateOfNotification" });
            DropIndex("dbo.ClaimData", new[] { "DateApproved" });
            DropIndex("dbo.ClaimData", new[] { "DateOfEvent" });
            DropIndex("dbo.ClaimData", new[] { "InsuredDateOfBirth" });
            DropIndex("dbo.ClaimData", new[] { "LastTransactionDate" });
            DropIndex("dbo.ClaimData", new[] { "MlreInvoiceDate" });
            DropIndex("dbo.ClaimData", new[] { "ReinsEffDatePol" });
            DropIndex("dbo.ClaimData", new[] { "RetroStatementDate1" });
            DropIndex("dbo.ClaimData", new[] { "RetroStatementDate2" });
            DropIndex("dbo.ClaimData", new[] { "RetroStatementDate3" });
            DropIndex("dbo.ClaimData", new[] { "TempD1" });
            DropIndex("dbo.ClaimData", new[] { "TempD2" });
            DropIndex("dbo.ClaimData", new[] { "TransactionDateWop" });
            DropIndex("dbo.ClaimData", new[] { "DateOfReported" });
            AddColumn("dbo.ClaimData", "DateOfIntimation", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimData", "CedantDateOfNotification", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimData", "DateApproved", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimData", "DateOfEvent", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimData", "InsuredDateOfBirth", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimData", "LastTransactionDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimData", "MlreInvoiceDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimData", "ReinsEffDatePol", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimData", "RetroStatementDate1", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimData", "RetroStatementDate2", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimData", "RetroStatementDate3", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimData", "TempD1", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimData", "TempD2", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimData", "TransactionDateWop", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimData", "DateOfReported", c => c.DateTime(precision: 7, storeType: "datetime2"));
            CreateIndex("dbo.ClaimData", "CedantDateOfNotification");
            CreateIndex("dbo.ClaimData", "DateApproved");
            CreateIndex("dbo.ClaimData", "DateOfEvent");
            CreateIndex("dbo.ClaimData", "InsuredDateOfBirth");
            CreateIndex("dbo.ClaimData", "LastTransactionDate");
            CreateIndex("dbo.ClaimData", "MlreInvoiceDate");
            CreateIndex("dbo.ClaimData", "ReinsEffDatePol");
            CreateIndex("dbo.ClaimData", "RetroStatementDate1");
            CreateIndex("dbo.ClaimData", "RetroStatementDate2");
            CreateIndex("dbo.ClaimData", "RetroStatementDate3");
            CreateIndex("dbo.ClaimData", "TempD1");
            CreateIndex("dbo.ClaimData", "TempD2");
            CreateIndex("dbo.ClaimData", "TransactionDateWop");
            CreateIndex("dbo.ClaimData", "IssueDatePol");
            CreateIndex("dbo.ClaimData", "DateOfReported");
            CreateIndex("dbo.ClaimData", "DateOfIntimation");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ClaimData", new[] { "DateOfIntimation" });
            DropIndex("dbo.ClaimData", new[] { "DateOfReported" });
            DropIndex("dbo.ClaimData", new[] { "IssueDatePol" });
            DropIndex("dbo.ClaimData", new[] { "TransactionDateWop" });
            DropIndex("dbo.ClaimData", new[] { "TempD2" });
            DropIndex("dbo.ClaimData", new[] { "TempD1" });
            DropIndex("dbo.ClaimData", new[] { "RetroStatementDate3" });
            DropIndex("dbo.ClaimData", new[] { "RetroStatementDate2" });
            DropIndex("dbo.ClaimData", new[] { "RetroStatementDate1" });
            DropIndex("dbo.ClaimData", new[] { "ReinsEffDatePol" });
            DropIndex("dbo.ClaimData", new[] { "MlreInvoiceDate" });
            DropIndex("dbo.ClaimData", new[] { "LastTransactionDate" });
            DropIndex("dbo.ClaimData", new[] { "InsuredDateOfBirth" });
            DropIndex("dbo.ClaimData", new[] { "DateOfEvent" });
            DropIndex("dbo.ClaimData", new[] { "DateApproved" });
            DropIndex("dbo.ClaimData", new[] { "CedantDateOfNotification" });
            AlterColumn("dbo.ClaimData", "DateOfReported", c => c.DateTime());
            AlterColumn("dbo.ClaimData", "TransactionDateWop", c => c.DateTime());
            AlterColumn("dbo.ClaimData", "TempD2", c => c.DateTime());
            AlterColumn("dbo.ClaimData", "TempD1", c => c.DateTime());
            AlterColumn("dbo.ClaimData", "RetroStatementDate3", c => c.DateTime());
            AlterColumn("dbo.ClaimData", "RetroStatementDate2", c => c.DateTime());
            AlterColumn("dbo.ClaimData", "RetroStatementDate1", c => c.DateTime());
            AlterColumn("dbo.ClaimData", "ReinsEffDatePol", c => c.DateTime());
            AlterColumn("dbo.ClaimData", "MlreInvoiceDate", c => c.DateTime());
            AlterColumn("dbo.ClaimData", "LastTransactionDate", c => c.DateTime());
            AlterColumn("dbo.ClaimData", "InsuredDateOfBirth", c => c.DateTime());
            AlterColumn("dbo.ClaimData", "DateOfEvent", c => c.DateTime());
            AlterColumn("dbo.ClaimData", "DateApproved", c => c.DateTime());
            AlterColumn("dbo.ClaimData", "CedantDateOfNotification", c => c.DateTime());
            DropColumn("dbo.ClaimData", "DateOfIntimation");
            CreateIndex("dbo.ClaimData", "DateOfReported");
            CreateIndex("dbo.ClaimData", "TransactionDateWop");
            CreateIndex("dbo.ClaimData", "TempD2");
            CreateIndex("dbo.ClaimData", "TempD1");
            CreateIndex("dbo.ClaimData", "RetroStatementDate3");
            CreateIndex("dbo.ClaimData", "RetroStatementDate2");
            CreateIndex("dbo.ClaimData", "RetroStatementDate1");
            CreateIndex("dbo.ClaimData", "ReinsEffDatePol");
            CreateIndex("dbo.ClaimData", "MlreInvoiceDate");
            CreateIndex("dbo.ClaimData", "LastTransactionDate");
            CreateIndex("dbo.ClaimData", "InsuredDateOfBirth");
            CreateIndex("dbo.ClaimData", "DateOfEvent");
            CreateIndex("dbo.ClaimData", "DateApproved");
            CreateIndex("dbo.ClaimData", "CedantDateOfNotification");
        }
    }
}
