TRUNCATE TABLE [Mfrs17ReportingDetailRiDatas];

ALTER TABLE [Mfrs17ReportingDetailRiDatas] DROP CONSTRAINT [FK_dbo.Mfrs17ReportingDetailRiDatas_dbo.Mfrs17ReportingDetails_Mfrs17ReportingDetailId];
GO

TRUNCATE TABLE [Mfrs17ReportingDetails];
GO

ALTER TABLE [Mfrs17ReportingDetailRiDatas]  WITH CHECK ADD CONSTRAINT [FK_dbo.Mfrs17ReportingDetailRiDatas_dbo.Mfrs17ReportingDetails_Mfrs17ReportingDetailId] FOREIGN KEY([Mfrs17ReportingDetailId])
REFERENCES [Mfrs17ReportingDetails] ([Id])
GO

ALTER TABLE [Mfrs17ReportingDetailRiDatas] CHECK CONSTRAINT [FK_dbo.Mfrs17ReportingDetailRiDatas_dbo.Mfrs17ReportingDetails_Mfrs17ReportingDetailId]
GO

ALTER TABLE [Mfrs17ReportingDetails] DROP CONSTRAINT [FK_dbo.Mfrs17ReportingDetails_dbo.Mfrs17Reportings_Mfrs17ReportingId]
GO

TRUNCATE TABLE [Mfrs17Reportings];

ALTER TABLE [Mfrs17ReportingDetails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Mfrs17ReportingDetails_dbo.Mfrs17Reportings_Mfrs17ReportingId] FOREIGN KEY([Mfrs17ReportingId])
REFERENCES [Mfrs17Reportings] ([Id])
GO

ALTER TABLE [Mfrs17ReportingDetails] CHECK CONSTRAINT [FK_dbo.Mfrs17ReportingDetails_dbo.Mfrs17Reportings_Mfrs17ReportingId]
GO

/*
SELECT 
  name AS 'Constraint',
  is_disabled,
  is_not_trusted
FROM sys.foreign_keys
WHERE name LIKE '%Mfrs%';
*/