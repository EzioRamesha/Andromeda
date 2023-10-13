namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatePricingDashboardViews : DbMigration
    {
        public override void Up()
        {
            //PricingDashboardDueDateOverview
            Sql("CREATE OR ALTER VIEW [dbo].[PricingDashboardDueDateOverview] AS " +
                "SELECT TRY_CONVERT(INT,ROW_NUMBER() OVER(ORDER BY t.PricingTeamPickListDetailId ASC)) AS Id, t.* FROM " +
                "(SELECT (SELECT TOP 1 Id FROM PickListDetails where PickListId = 57 AND Code = 'PP') AS PricingTeamPickListDetailId, " +
                "1 AS DueDateOverviewType, COUNT(*) AS NoOfCase FROM TreatyPricingQuotationWorkflows q " +
                "LEFT JOIN PickListDetails p ON p.Id = q.PricingTeamPickListDetailId " +
                "WHERE dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q.PricingDueDate,GETDATE()), GETDATE()) <= 5 " +
                "AND p.Code = 'PP' " +
                "UNION ALL " +
                "SELECT (SELECT TOP 1 Id FROM PickListDetails where PickListId = 57 AND Code = 'PP') AS PricingTeamPickListDetailId, " +
                "2 AS DueDateOverviewType, COUNT(*) AS NoOfCase FROM TreatyPricingQuotationWorkflows q " +
                "LEFT JOIN PickListDetails p ON p.Id = q.PricingTeamPickListDetailId " +
                "WHERE dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q.PricingDueDate,GETDATE()), GETDATE()) > 5 " +
                "AND dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q.PricingDueDate,GETDATE()), GETDATE()) <= 10 " +
                "AND p.Code = 'PP' " +
                "UNION ALL " +
                "SELECT (SELECT TOP 1 Id FROM PickListDetails where PickListId = 57 AND Code = 'PP') AS PricingTeamPickListDetailId, " +
                "3 AS DueDateOverviewType, COUNT(*) AS NoOfCase FROM TreatyPricingQuotationWorkflows q " +
                "LEFT JOIN PickListDetails p ON p.Id = q.PricingTeamPickListDetailId " +
                "WHERE dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q.PricingDueDate,GETDATE()), GETDATE()) > 10 " +
                "AND p.Code = 'PP' " +
                "UNION ALL " +
                "SELECT (SELECT TOP 1 Id FROM PickListDetails where PickListId = 57 AND Code = 'GP') AS PricingTeamPickListDetailId, " +
                "1 AS DueDateOverviewType, COUNT(*) AS NoOfCase FROM TreatyPricingQuotationWorkflows q " +
                "LEFT JOIN PickListDetails p ON p.Id = q.PricingTeamPickListDetailId " +
                "WHERE dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q.PricingDueDate,GETDATE()), GETDATE()) <= 5 " +
                "AND p.Code = 'GP' " +
                "UNION ALL " +
                "SELECT (SELECT TOP 1 Id FROM PickListDetails where PickListId = 57 AND Code = 'GP') AS PricingTeamPickListDetailId, " +
                "2 AS DueDateOverviewType, COUNT(*) AS NoOfCase FROM TreatyPricingQuotationWorkflows q " +
                "LEFT JOIN PickListDetails p ON p.Id = q.PricingTeamPickListDetailId " +
                "WHERE dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q.PricingDueDate,GETDATE()), GETDATE()) > 5 " +
                "AND dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q.PricingDueDate,GETDATE()), GETDATE()) <= 10 " +
                "AND p.Code = 'GP' " +
                "UNION ALL " +
                "SELECT (SELECT TOP 1 Id FROM PickListDetails where PickListId = 57 AND Code = 'GP') AS PricingTeamPickListDetailId, " +
                "3 AS DueDateOverviewType, COUNT(*) AS NoOfCase FROM TreatyPricingQuotationWorkflows q " +
                "LEFT JOIN PickListDetails p ON p.Id = q.PricingTeamPickListDetailId " +
                "WHERE dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q.PricingDueDate,GETDATE()), GETDATE()) > 10 " +
                "AND p.Code = 'GP') t ");

            //PricingDashboardDueDateOverviewIdList
            Sql("CREATE OR ALTER VIEW [dbo].[PricingDashboardDueDateOverviewIdList] AS " +
                "SELECT TRY_CONVERT(INT,ROW_NUMBER() OVER(ORDER BY t.PricingTeamPickListDetailId, t.DueDateOverviewType ASC)) AS Id, t.* FROM " +
                "(SELECT (SELECT TOP 1 Id FROM PickListDetails where PickListId = 57 AND Code = 'PP') AS PricingTeamPickListDetailId, " +
                "1 AS DueDateOverviewType, q.PersonInChargeId AS UserId FROM TreatyPricingQuotationWorkflows q " +
                "LEFT JOIN PickListDetails p ON p.Id = q.PricingTeamPickListDetailId " +
                "WHERE dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q.PricingDueDate,GETDATE()), GETDATE()) <= 5 " +
                "AND p.Code = 'PP' AND q.PersonInChargeId IS NOT NULL " +
                "UNION ALL " +
                "SELECT (SELECT TOP 1 Id FROM PickListDetails where PickListId = 57 AND Code = 'PP') AS PricingTeamPickListDetailId, " +
                "2 AS DueDateOverviewType, q.PersonInChargeId AS UserId FROM TreatyPricingQuotationWorkflows q " +
                "LEFT JOIN PickListDetails p ON p.Id = q.PricingTeamPickListDetailId " +
                "WHERE dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q.PricingDueDate,GETDATE()), GETDATE()) > 5 " +
                "AND dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q.PricingDueDate,GETDATE()), GETDATE()) <= 10 " +
                "AND p.Code = 'PP' AND q.PersonInChargeId IS NOT NULL " +
                "UNION ALL " +
                "SELECT (SELECT TOP 1 Id FROM PickListDetails where PickListId = 57 AND Code = 'PP') AS PricingTeamPickListDetailId, " +
                "3 AS DueDateOverviewType, q.PersonInChargeId AS UserId FROM TreatyPricingQuotationWorkflows q " +
                "LEFT JOIN PickListDetails p ON p.Id = q.PricingTeamPickListDetailId " +
                "WHERE dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q.PricingDueDate,GETDATE()), GETDATE()) > 10 " +
                "AND p.Code = 'PP' AND q.PersonInChargeId IS NOT NULL " +
                "UNION ALL " +
                "SELECT (SELECT TOP 1 Id FROM PickListDetails where PickListId = 57 AND Code = 'GP') AS PricingTeamPickListDetailId, " +
                "1 AS DueDateOverviewType, q.PersonInChargeId AS UserId FROM TreatyPricingQuotationWorkflows q " +
                "LEFT JOIN PickListDetails p ON p.Id = q.PricingTeamPickListDetailId " +
                "WHERE dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q.PricingDueDate,GETDATE()), GETDATE()) <= 5 " +
                "AND p.Code = 'GP' AND q.PersonInChargeId IS NOT NULL " +
                "UNION ALL " +
                "SELECT (SELECT TOP 1 Id FROM PickListDetails where PickListId = 57 AND Code = 'GP') AS PricingTeamPickListDetailId, " +
                "2 AS DueDateOverviewType, q.PersonInChargeId AS UserId FROM TreatyPricingQuotationWorkflows q " +
                "LEFT JOIN PickListDetails p ON p.Id = q.PricingTeamPickListDetailId " +
                "WHERE dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q.PricingDueDate,GETDATE()), GETDATE()) > 5 " +
                "AND dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q.PricingDueDate,GETDATE()), GETDATE()) <= 10 " +
                "AND p.Code = 'GP' AND q.PersonInChargeId IS NOT NULL " +
                "UNION ALL " +
                "SELECT (SELECT TOP 1 Id FROM PickListDetails where PickListId = 57 AND Code = 'GP') AS PricingTeamPickListDetailId, " +
                "3 AS DueDateOverviewType, q.PersonInChargeId AS UserId FROM TreatyPricingQuotationWorkflows q " +
                "LEFT JOIN PickListDetails p ON p.Id = q.PricingTeamPickListDetailId " +
                "WHERE dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q.PricingDueDate,GETDATE()), GETDATE()) > 10 " +
                "AND p.Code = 'GP' AND q.PersonInChargeId IS NOT NULL) t ");

            //PricingDashboardDueDateOverviewPIC
            Sql("CREATE OR ALTER VIEW [dbo].[PricingDashboardDueDateOverviewPIC] AS " +
                "SELECT TRY_CONVERT(INT,ROW_NUMBER() OVER(ORDER BY t.PricingTeamPickListDetailId, t.UserName ASC)) AS Id, t.* FROM " +
                "(SELECT DISTINCT q.PricingTeamPickListDetailId, q.PersonInChargeId AS UserId, u.FullName AS UserName, " +
                "(SELECT COUNT(*) AS NoOfCase FROM TreatyPricingQuotationWorkflows q1 " +
                "LEFT JOIN PickListDetails p ON p.Id = q1.PricingTeamPickListDetailId " +
                "WHERE dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q1.PricingDueDate,GETDATE()), GETDATE()) <= 5 " +
                "AND q1.PricingTeamPickListDetailId = q.PricingTeamPickListDetailId " +
                "AND q1.PersonInChargeId = q.PersonInChargeId) AS NoOfCaseDueDateBelow5, " +
                "(SELECT COUNT(*) AS NoOfCase FROM TreatyPricingQuotationWorkflows q1 " +
                "LEFT JOIN PickListDetails p ON p.Id = q1.PricingTeamPickListDetailId " +
                "WHERE dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q1.PricingDueDate,GETDATE()), GETDATE()) > 5 " +
                "AND dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q1.PricingDueDate,GETDATE()), GETDATE()) <= 10 " +
                "AND q1.PricingTeamPickListDetailId = q.PricingTeamPickListDetailId " +
                "AND q1.PersonInChargeId = q.PersonInChargeId) AS NoOfCaseDueDate6To10, " +
                "(SELECT COUNT(*) AS NoOfCase FROM TreatyPricingQuotationWorkflows q1 " +
                "LEFT JOIN PickListDetails p ON p.Id = q1.PricingTeamPickListDetailId " +
                "WHERE dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q1.PricingDueDate,GETDATE()), GETDATE()) > 10 " +
                "AND q1.PricingTeamPickListDetailId = q.PricingTeamPickListDetailId " +
                "AND q1.PersonInChargeId = q.PersonInChargeId) AS NoOfCaseDueDateExceed10 " +
                "FROM TreatyPricingQuotationWorkflows q " +
                "LEFT JOIN Users u ON u.Id = q.PersonInChargeId " +
                "LEFT JOIN PickListDetails p ON p.Id = q.PricingTeamPickListDetailId " +
                "WHERE q.PersonInChargeId IS NOT NULL AND p.Code = 'PP' " +
                "UNION ALL " +
                "SELECT DISTINCT q.PricingTeamPickListDetailId, q.PersonInChargeId AS UserId, u.FullName AS UserName, " +
                "(SELECT COUNT(*) AS NoOfCase FROM TreatyPricingQuotationWorkflows q1 " +
                "LEFT JOIN PickListDetails p ON p.Id = q1.PricingTeamPickListDetailId " +
                "WHERE dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q1.PricingDueDate,GETDATE()), GETDATE()) <= 5 " +
                "AND q1.PricingTeamPickListDetailId = q.PricingTeamPickListDetailId " +
                "AND q1.PersonInChargeId = q.PersonInChargeId) AS NoOfCaseDueDateBelow5, " +
                "(SELECT COUNT(*) AS NoOfCase FROM TreatyPricingQuotationWorkflows q1 " +
                "LEFT JOIN PickListDetails p ON p.Id = q1.PricingTeamPickListDetailId " +
                "WHERE dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q1.PricingDueDate,GETDATE()), GETDATE()) > 5 " +
                "AND dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q1.PricingDueDate,GETDATE()), GETDATE()) <= 10 " +
                "AND q1.PricingTeamPickListDetailId = q.PricingTeamPickListDetailId " +
                "AND q1.PersonInChargeId = q.PersonInChargeId) AS NoOfCaseDueDate6To10, " +
                "(SELECT COUNT(*) AS NoOfCase FROM TreatyPricingQuotationWorkflows q1 " +
                "LEFT JOIN PickListDetails p ON p.Id = q1.PricingTeamPickListDetailId " +
                "WHERE dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q1.PricingDueDate,GETDATE()), GETDATE()) > 10 " +
                "AND q1.PricingTeamPickListDetailId = q.PricingTeamPickListDetailId " +
                "AND q1.PersonInChargeId = q.PersonInChargeId) AS NoOfCaseDueDateExceed10 " +
                "FROM TreatyPricingQuotationWorkflows q " +
                "LEFT JOIN Users u ON u.Id = q.PersonInChargeId " +
                "LEFT JOIN PickListDetails p ON p.Id = q.PricingTeamPickListDetailId " +
                "WHERE q.PersonInChargeId IS NOT NULL AND p.Code = 'GP') t ");

            //PricingDashboardOutstandingPricingOverviewPIC
            Sql("CREATE OR ALTER VIEW [dbo].[PricingDashboardOutstandingPricingOverviewPIC] AS " +
                "SELECT TRY_CONVERT(INT,ROW_NUMBER() OVER(ORDER BY t.PricingTeamPickListDetailId, t.UserName ASC)) AS Id, t.* FROM " +
                "(SELECT DISTINCT q.PricingTeamPickListDetailId, q.PersonInChargeId AS UserId, u.FullName AS UserName, " +
                "(SELECT COUNT(*) AS NoOfCase FROM TreatyPricingQuotationWorkflows q1 " +
                "LEFT JOIN PickListDetails p ON p.Id = q1.PricingTeamPickListDetailId " +
                "WHERE q1.PricingStatus = 1 AND q1.PricingTeamPickListDetailId = q.PricingTeamPickListDetailId " +
                "AND q1.PersonInChargeId = q.PersonInChargeId) AS Unassigned, " +
                "(SELECT COUNT(*) AS NoOfCase FROM TreatyPricingQuotationWorkflows q1 " +
                "LEFT JOIN PickListDetails p ON p.Id = q1.PricingTeamPickListDetailId " +
                "WHERE q1.PricingStatus = 2 AND q1.PricingTeamPickListDetailId = q.PricingTeamPickListDetailId " +
                "AND q1.PersonInChargeId = q.PersonInChargeId) AS AssessmentInProgress, " +
                "(SELECT COUNT(*) AS NoOfCase FROM TreatyPricingQuotationWorkflows q1 " +
                "LEFT JOIN PickListDetails p ON p.Id = q1.PricingTeamPickListDetailId " +
                "WHERE q1.PricingStatus = 3 AND q1.PricingTeamPickListDetailId = q.PricingTeamPickListDetailId " +
                "AND q1.PersonInChargeId = q.PersonInChargeId) AS PendingTechReview, " +
                "(SELECT COUNT(*) AS NoOfCase FROM TreatyPricingQuotationWorkflows q1 " +
                "LEFT JOIN PickListDetails p ON p.Id = q1.PricingTeamPickListDetailId " +
                "WHERE q1.PricingStatus = 4 AND q1.PricingTeamPickListDetailId = q.PricingTeamPickListDetailId " +
                "AND q1.PersonInChargeId = q.PersonInChargeId) AS PendingPeerReview, " +
                "(SELECT COUNT(*) AS NoOfCase FROM TreatyPricingQuotationWorkflows q1 " +
                "LEFT JOIN PickListDetails p ON p.Id = q1.PricingTeamPickListDetailId " +
                "WHERE q1.PricingStatus = 5 AND q1.PricingTeamPickListDetailId = q.PricingTeamPickListDetailId " +
                "AND q1.PersonInChargeId = q.PersonInChargeId) AS PendingPricingAuthorityReview, " +
                "(SELECT COUNT(*) AS NoOfCase FROM TreatyPricingQuotationWorkflows q1 " +
                "LEFT JOIN PickListDetails p ON p.Id = q1.PricingTeamPickListDetailId " +
                "WHERE q1.PricingStatus = 6 AND q1.PricingTeamPickListDetailId = q.PricingTeamPickListDetailId " +
                "AND q1.PersonInChargeId = q.PersonInChargeId) AS ToUpdateRepo, " +
                "(SELECT COUNT(*) AS NoOfCase FROM TreatyPricingQuotationWorkflows q1 " +
                "LEFT JOIN PickListDetails p ON p.Id = q1.PricingTeamPickListDetailId " +
                "WHERE q1.PricingStatus = 7 AND q1.PricingTeamPickListDetailId = q.PricingTeamPickListDetailId " +
                "AND q1.PersonInChargeId = q.PersonInChargeId) AS UpdatedRepo " +
                "FROM TreatyPricingQuotationWorkflows q " +
                "LEFT JOIN Users u ON u.Id = q.PersonInChargeId " +
                "LEFT JOIN PickListDetails p ON p.Id = q.PricingTeamPickListDetailId " +
                "WHERE q.PersonInChargeId IS NOT NULL AND p.Code = 'PP' " +
                "UNION ALL " +
                "SELECT DISTINCT q.PricingTeamPickListDetailId, q.PersonInChargeId AS UserId, u.FullName AS UserName, " +
                "(SELECT COUNT(*) AS NoOfCase FROM TreatyPricingQuotationWorkflows q1 " +
                "LEFT JOIN PickListDetails p ON p.Id = q1.PricingTeamPickListDetailId " +
                "WHERE q1.PricingStatus = 1 AND q1.PricingTeamPickListDetailId = q.PricingTeamPickListDetailId " +
                "AND q1.PersonInChargeId = q.PersonInChargeId) AS Unassigned, " +
                "(SELECT COUNT(*) AS NoOfCase FROM TreatyPricingQuotationWorkflows q1 " +
                "LEFT JOIN PickListDetails p ON p.Id = q1.PricingTeamPickListDetailId " +
                "WHERE q1.PricingStatus = 2 AND q1.PricingTeamPickListDetailId = q.PricingTeamPickListDetailId " +
                "AND q1.PersonInChargeId = q.PersonInChargeId) AS AssessmentInProgress, " +
                "(SELECT COUNT(*) AS NoOfCase FROM TreatyPricingQuotationWorkflows q1 " +
                "LEFT JOIN PickListDetails p ON p.Id = q1.PricingTeamPickListDetailId " +
                "WHERE q1.PricingStatus = 3 AND q1.PricingTeamPickListDetailId = q.PricingTeamPickListDetailId " +
                "AND q1.PersonInChargeId = q.PersonInChargeId) AS PendingTechReview, " +
                "(SELECT COUNT(*) AS NoOfCase FROM TreatyPricingQuotationWorkflows q1 " +
                "LEFT JOIN PickListDetails p ON p.Id = q1.PricingTeamPickListDetailId " +
                "WHERE q1.PricingStatus = 4 AND q1.PricingTeamPickListDetailId = q.PricingTeamPickListDetailId " +
                "AND q1.PersonInChargeId = q.PersonInChargeId) AS PendingPeerReview, " +
                "(SELECT COUNT(*) AS NoOfCase FROM TreatyPricingQuotationWorkflows q1 " +
                "LEFT JOIN PickListDetails p ON p.Id = q1.PricingTeamPickListDetailId " +
                "WHERE q1.PricingStatus = 5 AND q1.PricingTeamPickListDetailId = q.PricingTeamPickListDetailId " +
                "AND q1.PersonInChargeId = q.PersonInChargeId) AS PendingPricingAuthorityReview, " +
                "(SELECT COUNT(*) AS NoOfCase FROM TreatyPricingQuotationWorkflows q1 " +
                "LEFT JOIN PickListDetails p ON p.Id = q1.PricingTeamPickListDetailId " +
                "WHERE q1.PricingStatus = 6 AND q1.PricingTeamPickListDetailId = q.PricingTeamPickListDetailId " +
                "AND q1.PersonInChargeId = q.PersonInChargeId) AS ToUpdateRepo, " +
                "(SELECT COUNT(*) AS NoOfCase FROM TreatyPricingQuotationWorkflows q1 " +
                "LEFT JOIN PickListDetails p ON p.Id = q1.PricingTeamPickListDetailId " +
                "WHERE q1.PricingStatus = 7 AND q1.PricingTeamPickListDetailId = q.PricingTeamPickListDetailId " +
                "AND q1.PersonInChargeId = q.PersonInChargeId) AS UpdatedRepo " +
                "FROM TreatyPricingQuotationWorkflows q " +
                "LEFT JOIN Users u ON u.Id = q.PersonInChargeId " +
                "LEFT JOIN PickListDetails p ON p.Id = q.PricingTeamPickListDetailId " +
                "WHERE q.PersonInChargeId IS NOT NULL AND p.Code = 'GP') t ");

            //PricingDashboardQuotingCasesByCompany
            Sql("CREATE OR ALTER VIEW [dbo].[PricingDashboardQuotingCasesByCompany] AS " +
                "SELECT TRY_CONVERT(INT,ROW_NUMBER() OVER(ORDER BY t.CedantName ASC)) AS Id, t.* FROM " +
                "(SELECT DISTINCT  " +
                "c.[Name] AS CedantName, c.Id AS CedantId, q.PricingTeamPickListDetailId, " +
                "(SELECT COUNT(*) FROM TreatyPricingQuotationWorkflows q1 WHERE q1.CedantId = q.CedantId AND " +
                "q1.PricingTeamPickListDetailId = q.PricingTeamPickListDetailId AND q1.[Status] = 1) AS QuotingCaseCount, " +
                "(SELECT ISNULL(SUM(TRY_CONVERT(FLOAT,REPLACE(ISNULL(pv.ExpectedRiPremium,0),',',''))),0) FROM TreatyPricingQuotationWorkflows q1 " +
                "LEFT JOIN TreatyPricingWorkflowObjects o ON o.WorkflowId = q1.Id " +
                "LEFT JOIN TreatyPricingProductVersions pv ON pv.Id = o.ObjectVersionId " +
                "WHERE q1.CedantId = q.CedantId AND q1.PricingTeamPickListDetailId = q.PricingTeamPickListDetailId AND " +
                "o.ObjectType = 8 AND q1.[Status] = 1 AND o.[Type] = 1) AS ExpectedRiPremium " +
                "FROM TreatyPricingQuotationWorkflows q " +
                "LEFT JOIN Cedants c ON c.Id = q.CedantId " +
                "LEFT JOIN PickListDetails p ON p.Id = q.PricingTeamPickListDetailId " +
                "WHERE p.Code = 'PP') t " +
                "UNION ALL " +
                "SELECT TRY_CONVERT(INT,ROW_NUMBER() OVER(ORDER BY t.CedantName ASC)) AS Id, t.* FROM " +
                "(SELECT DISTINCT  " +
                "c.[Name] AS CedantName, c.Id AS CedantId, q.PricingTeamPickListDetailId, " +
                "(SELECT COUNT(*) FROM TreatyPricingQuotationWorkflows q1 WHERE q1.CedantId = q.CedantId AND " +
                "q1.PricingTeamPickListDetailId = q.PricingTeamPickListDetailId AND q1.[Status] = 1) AS QuotingCaseCount, " +
                "(SELECT ISNULL(SUM(TRY_CONVERT(FLOAT,REPLACE(ISNULL(pv.ExpectedRiPremium,0),',',''))),0) FROM TreatyPricingQuotationWorkflows q1 " +
                "LEFT JOIN TreatyPricingWorkflowObjects o ON o.WorkflowId = q1.Id " +
                "LEFT JOIN TreatyPricingProductVersions pv ON pv.Id = o.ObjectVersionId " +
                "WHERE q1.CedantId = q.CedantId AND q1.PricingTeamPickListDetailId = q.PricingTeamPickListDetailId AND " +
                "o.ObjectType = 8 AND q1.[Status] = 1 AND o.[Type] = 1) AS ExpectedRiPremium " +
                "FROM TreatyPricingQuotationWorkflows q " +
                "LEFT JOIN Cedants c ON c.Id = q.CedantId " +
                "LEFT JOIN PickListDetails p ON p.Id = q.PricingTeamPickListDetailId " +
                "WHERE p.Code = 'GP') t ");
        }
        
        public override void Down()
        {
            Sql("DROP VIEW [dbo].[PricingDashboardDueDateOverview]");
            Sql("DROP VIEW [dbo].[PricingDashboardDueDateOverviewIdList]");
            Sql("DROP VIEW [dbo].[PricingDashboardDueDateOverviewPIC]");
            Sql("DROP VIEW [dbo].[PricingDashboardOutstandingPricingOverviewPIC]");
            Sql("DROP VIEW [dbo].[PricingDashboardQuotingCasesByCompany]");
        }
    }
}
