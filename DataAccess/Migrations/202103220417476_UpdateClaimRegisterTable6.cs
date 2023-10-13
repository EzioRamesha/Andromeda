namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClaimRegisterTable6 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ClaimRegister", new[] { "TargetDateToIssueInvoice" });
            DropIndex("dbo.ClaimRegister", new[] { "CedantDateOfNotification" });
            DropIndex("dbo.ClaimRegister", new[] { "DateApproved" });
            DropIndex("dbo.ClaimRegister", new[] { "DateOfEvent" });
            DropIndex("dbo.ClaimRegister", new[] { "DateOfRegister" });
            DropIndex("dbo.ClaimRegister", new[] { "DateOfReported" });
            DropIndex("dbo.ClaimRegister", new[] { "InsuredDateOfBirth" });
            DropIndex("dbo.ClaimRegister", new[] { "LastTransactionDate" });
            DropIndex("dbo.ClaimRegister", new[] { "MlreInvoiceDate" });
            DropIndex("dbo.ClaimRegister", new[] { "ReinsEffDatePol" });
            DropIndex("dbo.ClaimRegister", new[] { "RetroStatementDate1" });
            DropIndex("dbo.ClaimRegister", new[] { "RetroStatementDate2" });
            DropIndex("dbo.ClaimRegister", new[] { "RetroStatementDate3" });
            DropIndex("dbo.ClaimRegister", new[] { "TempD1" });
            DropIndex("dbo.ClaimRegister", new[] { "TempD2" });
            DropIndex("dbo.ClaimRegister", new[] { "TransactionDateWop" });
            DropIndex("dbo.ClaimRegister", new[] { "ClaimCommitteeDateCommented1" });
            DropIndex("dbo.ClaimRegister", new[] { "ClaimCommitteeDateCommented2" });
            AddColumn("dbo.ClaimRegister", "DateOfIntimation", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegister", "TargetDateToIssueInvoice", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegister", "CedantDateOfNotification", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegister", "DateApproved", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegister", "DateOfEvent", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegister", "DateOfRegister", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegister", "DateOfReported", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegister", "InsuredDateOfBirth", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegister", "LastTransactionDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegister", "MlreInvoiceDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegister", "ReinsEffDatePol", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegister", "RetroStatementDate1", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegister", "RetroStatementDate2", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegister", "RetroStatementDate3", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegister", "TempD1", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegister", "TempD2", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegister", "TransactionDateWop", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegister", "ClaimCommitteeDateCommented1", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ClaimRegister", "ClaimCommitteeDateCommented2", c => c.DateTime(precision: 7, storeType: "datetime2"));
            CreateIndex("dbo.ClaimRegister", "TargetDateToIssueInvoice");
            CreateIndex("dbo.ClaimRegister", "CedantDateOfNotification");
            CreateIndex("dbo.ClaimRegister", "DateApproved");
            CreateIndex("dbo.ClaimRegister", "DateOfEvent");
            CreateIndex("dbo.ClaimRegister", "DateOfRegister");
            CreateIndex("dbo.ClaimRegister", "DateOfReported");
            CreateIndex("dbo.ClaimRegister", "InsuredDateOfBirth");
            CreateIndex("dbo.ClaimRegister", "LastTransactionDate");
            CreateIndex("dbo.ClaimRegister", "MlreInvoiceDate");
            CreateIndex("dbo.ClaimRegister", "ReinsEffDatePol");
            CreateIndex("dbo.ClaimRegister", "RetroStatementDate1");
            CreateIndex("dbo.ClaimRegister", "RetroStatementDate2");
            CreateIndex("dbo.ClaimRegister", "RetroStatementDate3");
            CreateIndex("dbo.ClaimRegister", "TempD1");
            CreateIndex("dbo.ClaimRegister", "TempD2");
            CreateIndex("dbo.ClaimRegister", "TransactionDateWop");
            CreateIndex("dbo.ClaimRegister", "IssueDatePol");
            CreateIndex("dbo.ClaimRegister", "PolicyExpiryDate");
            CreateIndex("dbo.ClaimRegister", "ClaimCommitteeDateCommented1");
            CreateIndex("dbo.ClaimRegister", "ClaimCommitteeDateCommented2");
            CreateIndex("dbo.ClaimRegister", "UpdatedOnBehalfAt");
            CreateIndex("dbo.ClaimRegister", "SignOffDate");
            CreateIndex("dbo.ClaimRegister", "DateOfIntimation");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ClaimRegister", new[] { "DateOfIntimation" });
            DropIndex("dbo.ClaimRegister", new[] { "SignOffDate" });
            DropIndex("dbo.ClaimRegister", new[] { "UpdatedOnBehalfAt" });
            DropIndex("dbo.ClaimRegister", new[] { "ClaimCommitteeDateCommented2" });
            DropIndex("dbo.ClaimRegister", new[] { "ClaimCommitteeDateCommented1" });
            DropIndex("dbo.ClaimRegister", new[] { "PolicyExpiryDate" });
            DropIndex("dbo.ClaimRegister", new[] { "IssueDatePol" });
            DropIndex("dbo.ClaimRegister", new[] { "TransactionDateWop" });
            DropIndex("dbo.ClaimRegister", new[] { "TempD2" });
            DropIndex("dbo.ClaimRegister", new[] { "TempD1" });
            DropIndex("dbo.ClaimRegister", new[] { "RetroStatementDate3" });
            DropIndex("dbo.ClaimRegister", new[] { "RetroStatementDate2" });
            DropIndex("dbo.ClaimRegister", new[] { "RetroStatementDate1" });
            DropIndex("dbo.ClaimRegister", new[] { "ReinsEffDatePol" });
            DropIndex("dbo.ClaimRegister", new[] { "MlreInvoiceDate" });
            DropIndex("dbo.ClaimRegister", new[] { "LastTransactionDate" });
            DropIndex("dbo.ClaimRegister", new[] { "InsuredDateOfBirth" });
            DropIndex("dbo.ClaimRegister", new[] { "DateOfReported" });
            DropIndex("dbo.ClaimRegister", new[] { "DateOfRegister" });
            DropIndex("dbo.ClaimRegister", new[] { "DateOfEvent" });
            DropIndex("dbo.ClaimRegister", new[] { "DateApproved" });
            DropIndex("dbo.ClaimRegister", new[] { "CedantDateOfNotification" });
            DropIndex("dbo.ClaimRegister", new[] { "TargetDateToIssueInvoice" });
            AlterColumn("dbo.ClaimRegister", "ClaimCommitteeDateCommented2", c => c.DateTime());
            AlterColumn("dbo.ClaimRegister", "ClaimCommitteeDateCommented1", c => c.DateTime());
            AlterColumn("dbo.ClaimRegister", "TransactionDateWop", c => c.DateTime());
            AlterColumn("dbo.ClaimRegister", "TempD2", c => c.DateTime());
            AlterColumn("dbo.ClaimRegister", "TempD1", c => c.DateTime());
            AlterColumn("dbo.ClaimRegister", "RetroStatementDate3", c => c.DateTime());
            AlterColumn("dbo.ClaimRegister", "RetroStatementDate2", c => c.DateTime());
            AlterColumn("dbo.ClaimRegister", "RetroStatementDate1", c => c.DateTime());
            AlterColumn("dbo.ClaimRegister", "ReinsEffDatePol", c => c.DateTime());
            AlterColumn("dbo.ClaimRegister", "MlreInvoiceDate", c => c.DateTime());
            AlterColumn("dbo.ClaimRegister", "LastTransactionDate", c => c.DateTime());
            AlterColumn("dbo.ClaimRegister", "InsuredDateOfBirth", c => c.DateTime());
            AlterColumn("dbo.ClaimRegister", "DateOfReported", c => c.DateTime());
            AlterColumn("dbo.ClaimRegister", "DateOfRegister", c => c.DateTime());
            AlterColumn("dbo.ClaimRegister", "DateOfEvent", c => c.DateTime());
            AlterColumn("dbo.ClaimRegister", "DateApproved", c => c.DateTime());
            AlterColumn("dbo.ClaimRegister", "CedantDateOfNotification", c => c.DateTime());
            AlterColumn("dbo.ClaimRegister", "TargetDateToIssueInvoice", c => c.DateTime());
            DropColumn("dbo.ClaimRegister", "DateOfIntimation");
            CreateIndex("dbo.ClaimRegister", "ClaimCommitteeDateCommented2");
            CreateIndex("dbo.ClaimRegister", "ClaimCommitteeDateCommented1");
            CreateIndex("dbo.ClaimRegister", "TransactionDateWop");
            CreateIndex("dbo.ClaimRegister", "TempD2");
            CreateIndex("dbo.ClaimRegister", "TempD1");
            CreateIndex("dbo.ClaimRegister", "RetroStatementDate3");
            CreateIndex("dbo.ClaimRegister", "RetroStatementDate2");
            CreateIndex("dbo.ClaimRegister", "RetroStatementDate1");
            CreateIndex("dbo.ClaimRegister", "ReinsEffDatePol");
            CreateIndex("dbo.ClaimRegister", "MlreInvoiceDate");
            CreateIndex("dbo.ClaimRegister", "LastTransactionDate");
            CreateIndex("dbo.ClaimRegister", "InsuredDateOfBirth");
            CreateIndex("dbo.ClaimRegister", "DateOfReported");
            CreateIndex("dbo.ClaimRegister", "DateOfRegister");
            CreateIndex("dbo.ClaimRegister", "DateOfEvent");
            CreateIndex("dbo.ClaimRegister", "DateApproved");
            CreateIndex("dbo.ClaimRegister", "CedantDateOfNotification");
            CreateIndex("dbo.ClaimRegister", "TargetDateToIssueInvoice");
        }
    }
}
