CREATE VIEW [dbo].[RiDataWithSoaQuarterRiskQuarter]
AS
SELECT        *, CASE WHEN ri.ReportPeriodMonth IS NOT NULL AND ri.ReportPeriodYear IS NOT NULL 
                         THEN RIGHT(CAST(ri.ReportPeriodYear AS NVARCHAR(10)), 2) + CASE WHEN ri.ReportPeriodMonth BETWEEN 1 AND 3 THEN 'Q1' ELSE CASE WHEN ri.ReportPeriodMonth BETWEEN 4 AND 
                         6 THEN 'Q2' ELSE CASE WHEN ri.ReportPeriodMonth BETWEEN 7 AND 9 THEN 'Q3' ELSE CASE WHEN ri.ReportPeriodMonth BETWEEN 10 AND 12 THEN 'Q4' ELSE '' END END END END ELSE '' END AS SoaQuarter, 
                         CASE WHEN ri.RiskPeriodMonth IS NOT NULL AND ri.RiskPeriodYear IS NOT NULL THEN RIGHT(CAST(ri.RiskPeriodYear AS NVARCHAR(10)), 2) + CASE WHEN ri.RiskPeriodMonth BETWEEN 1 AND 
                         3 THEN 'Q1' ELSE CASE WHEN ri.RiskPeriodMonth BETWEEN 4 AND 6 THEN 'Q2' ELSE CASE WHEN ri.RiskPeriodMonth BETWEEN 7 AND 9 THEN 'Q3' ELSE CASE WHEN ri.RiskPeriodMonth BETWEEN 10 AND 
                         12 THEN 'Q4' ELSE '' END END END END ELSE '' END AS RiskQuarter
FROM            dbo.RiData AS ri
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "ri"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 349
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 1110
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'RiDataWithSoaQuarterRiskQuarter'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'RiDataWithSoaQuarterRiskQuarter'
GO