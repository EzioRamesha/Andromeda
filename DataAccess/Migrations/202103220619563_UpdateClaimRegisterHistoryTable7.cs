namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdateClaimRegisterHistoryTable7 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ClaimRegisterHistories", new[] { "TargetDateToIssueInvoice" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "CedantDateOfNotification" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "DateApproved" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "DateOfEvent" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "DateOfRegister" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "DateOfReported" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "InsuredDateOfBirth" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "LastTransactionDate" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "MlreInvoiceDate" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ReinsEffDatePol" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RetroStatementDate1" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RetroStatementDate2" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RetroStatementDate3" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "TempD1" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "TempD2" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "TransactionDateWop" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ClaimCommitteeDateCommented1" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ClaimCommitteeDateCommented2" });
            AddColumn("dbo.ClaimRegisterHistories", "DateOfIntimation", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegisterHistories", "TargetDateToIssueInvoice", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegisterHistories", "CedantDateOfNotification", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegisterHistories", "DateApproved", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegisterHistories", "DateOfEvent", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegisterHistories", "DateOfRegister", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegisterHistories", "DateOfReported", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegisterHistories", "InsuredDateOfBirth", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegisterHistories", "LastTransactionDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegisterHistories", "MlreInvoiceDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegisterHistories", "ReinsEffDatePol", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegisterHistories", "RetroStatementDate1", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegisterHistories", "RetroStatementDate2", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegisterHistories", "RetroStatementDate3", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegisterHistories", "TempD1", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegisterHistories", "TempD2", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegisterHistories", "TransactionDateWop", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegisterHistories", "ClaimCommitteeDateCommented1", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegisterHistories", "ClaimCommitteeDateCommented2", c => c.DateTime(precision: 7, storeType: "datetime2"));
            CreateIndex("dbo.ClaimRegisterHistories", "TargetDateToIssueInvoice");
            CreateIndex("dbo.ClaimRegisterHistories", "CedantDateOfNotification");
            CreateIndex("dbo.ClaimRegisterHistories", "DateApproved");
            CreateIndex("dbo.ClaimRegisterHistories", "DateOfEvent");
            CreateIndex("dbo.ClaimRegisterHistories", "DateOfRegister");
            CreateIndex("dbo.ClaimRegisterHistories", "DateOfReported");
            CreateIndex("dbo.ClaimRegisterHistories", "InsuredDateOfBirth");
            CreateIndex("dbo.ClaimRegisterHistories", "LastTransactionDate");
            CreateIndex("dbo.ClaimRegisterHistories", "MlreInvoiceDate");
            CreateIndex("dbo.ClaimRegisterHistories", "ReinsEffDatePol");
            CreateIndex("dbo.ClaimRegisterHistories", "RetroStatementDate1");
            CreateIndex("dbo.ClaimRegisterHistories", "RetroStatementDate2");
            CreateIndex("dbo.ClaimRegisterHistories", "RetroStatementDate3");
            CreateIndex("dbo.ClaimRegisterHistories", "TempD1");
            CreateIndex("dbo.ClaimRegisterHistories", "TempD2");
            CreateIndex("dbo.ClaimRegisterHistories", "TransactionDateWop");
            CreateIndex("dbo.ClaimRegisterHistories", "IssueDatePol");
            CreateIndex("dbo.ClaimRegisterHistories", "PolicyExpiryDate");
            CreateIndex("dbo.ClaimRegisterHistories", "ClaimCommitteeDateCommented1");
            CreateIndex("dbo.ClaimRegisterHistories", "ClaimCommitteeDateCommented2");
            CreateIndex("dbo.ClaimRegisterHistories", "UpdatedOnBehalfAt");
            CreateIndex("dbo.ClaimRegisterHistories", "SignOffDate");
            CreateIndex("dbo.ClaimRegisterHistories", "DateOfIntimation");
        }

        public override void Down()
        {
            DropIndex("dbo.ClaimRegisterHistories", new[] { "DateOfIntimation" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "SignOffDate" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "UpdatedOnBehalfAt" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ClaimCommitteeDateCommented2" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ClaimCommitteeDateCommented1" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "PolicyExpiryDate" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "IssueDatePol" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "TransactionDateWop" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "TempD2" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "TempD1" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RetroStatementDate3" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RetroStatementDate2" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RetroStatementDate1" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ReinsEffDatePol" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "MlreInvoiceDate" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "LastTransactionDate" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "InsuredDateOfBirth" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "DateOfReported" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "DateOfRegister" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "DateOfEvent" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "DateApproved" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "CedantDateOfNotification" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "TargetDateToIssueInvoice" });
            AlterColumn("dbo.ClaimRegisterHistories", "ClaimCommitteeDateCommented2", c => c.DateTime());
            AlterColumn("dbo.ClaimRegisterHistories", "ClaimCommitteeDateCommented1", c => c.DateTime());
            AlterColumn("dbo.ClaimRegisterHistories", "TransactionDateWop", c => c.DateTime());
            AlterColumn("dbo.ClaimRegisterHistories", "TempD2", c => c.DateTime());
            AlterColumn("dbo.ClaimRegisterHistories", "TempD1", c => c.DateTime());
            AlterColumn("dbo.ClaimRegisterHistories", "RetroStatementDate3", c => c.DateTime());
            AlterColumn("dbo.ClaimRegisterHistories", "RetroStatementDate2", c => c.DateTime());
            AlterColumn("dbo.ClaimRegisterHistories", "RetroStatementDate1", c => c.DateTime());
            AlterColumn("dbo.ClaimRegisterHistories", "ReinsEffDatePol", c => c.DateTime());
            AlterColumn("dbo.ClaimRegisterHistories", "MlreInvoiceDate", c => c.DateTime());
            AlterColumn("dbo.ClaimRegisterHistories", "LastTransactionDate", c => c.DateTime());
            AlterColumn("dbo.ClaimRegisterHistories", "InsuredDateOfBirth", c => c.DateTime());
            AlterColumn("dbo.ClaimRegisterHistories", "DateOfReported", c => c.DateTime());
            AlterColumn("dbo.ClaimRegisterHistories", "DateOfRegister", c => c.DateTime());
            AlterColumn("dbo.ClaimRegisterHistories", "DateOfEvent", c => c.DateTime());
            AlterColumn("dbo.ClaimRegisterHistories", "DateApproved", c => c.DateTime());
            AlterColumn("dbo.ClaimRegisterHistories", "CedantDateOfNotification", c => c.DateTime());
            AlterColumn("dbo.ClaimRegisterHistories", "TargetDateToIssueInvoice", c => c.DateTime());
            DropColumn("dbo.ClaimRegisterHistories", "DateOfIntimation");
            CreateIndex("dbo.ClaimRegisterHistories", "ClaimCommitteeDateCommented2");
            CreateIndex("dbo.ClaimRegisterHistories", "ClaimCommitteeDateCommented1");
            CreateIndex("dbo.ClaimRegisterHistories", "TransactionDateWop");
            CreateIndex("dbo.ClaimRegisterHistories", "TempD2");
            CreateIndex("dbo.ClaimRegisterHistories", "TempD1");
            CreateIndex("dbo.ClaimRegisterHistories", "RetroStatementDate3");
            CreateIndex("dbo.ClaimRegisterHistories", "RetroStatementDate2");
            CreateIndex("dbo.ClaimRegisterHistories", "RetroStatementDate1");
            CreateIndex("dbo.ClaimRegisterHistories", "ReinsEffDatePol");
            CreateIndex("dbo.ClaimRegisterHistories", "MlreInvoiceDate");
            CreateIndex("dbo.ClaimRegisterHistories", "LastTransactionDate");
            CreateIndex("dbo.ClaimRegisterHistories", "InsuredDateOfBirth");
            CreateIndex("dbo.ClaimRegisterHistories", "DateOfReported");
            CreateIndex("dbo.ClaimRegisterHistories", "DateOfRegister");
            CreateIndex("dbo.ClaimRegisterHistories", "DateOfEvent");
            CreateIndex("dbo.ClaimRegisterHistories", "DateApproved");
            CreateIndex("dbo.ClaimRegisterHistories", "CedantDateOfNotification");
            CreateIndex("dbo.ClaimRegisterHistories", "TargetDateToIssueInvoice");
        }
    }
}
